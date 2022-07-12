using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebWordGame.Models;

namespace WebWordGame.Controllers
{
    public class GameController : Controller
    {
        private readonly DataBaseContext _dataBaseContext;

        private IHubContext<ChatHub> _hubContext;

        public GameController(DataBaseContext dataBaseContext, IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
            _dataBaseContext = dataBaseContext;
        }





        



        [Authorize(Roles = "admin, person")]
        [HttpGet]
        public IActionResult Queue()
        {
            if (_dataBaseContext.People.First(u => u.LoginName == User.Identity.Name).IsInGame)
            {
                //return RedirectToAction("JoinToTheGame", $"game", new { UserName = User.Identity.Name, roomId =
                //                        _dataBaseContext.People.First(u => u.LoginName == User.Identity.Name).LastVisitedRoom });
                return JoinToTheGame(User.Identity.Name, _dataBaseContext.People.First(u => u.LoginName == User.Identity.Name).LastVisitedRoom);

            }
            ViewBag.Games = _dataBaseContext.Games.Include(q => q.roomGamers).Include(p => p.People);

            return View();
        }

        [Authorize(Roles = "admin, person")]

        [HttpPost]
        public IActionResult Queue(string gameName, int quantityOfGamers)
        {
            if (gameName != null)
            {
                PersonModel person = _dataBaseContext.People.FirstOrDefault(p => p.LoginName == User.Identity.Name);

                GameModel newGame = new GameModel
                {
                    Name = gameName,
                    GameStatus = "Created",
                    MaximumNumbersOfGamers = quantityOfGamers,
                    Creator = person.LoginName,
                    Winner = null

                };


                newGame.People.Add(person);


                _dataBaseContext.Games.Add(newGame);
                _dataBaseContext.SaveChanges();

                newGame.access_token = Crypto.Hash(Convert.ToString(newGame.Id));
                _dataBaseContext.SaveChanges();

                //_hubContext.Groups.AddToGroupAsync(person.LoginName, Convert.ToString(newGame.Id));



                return RedirectToAction("battlefield", $"game", new { gameId = newGame.Id, access_link = Crypto.Hash(Convert.ToString(newGame.Id)) });

            }

            return View();
        }


        [Authorize(Roles = "admin, person")]

        [HttpGet]
        public IActionResult BattleField(int gameId, string access_link)
        {
            GameModel g = _dataBaseContext.Games.FirstOrDefault(g => g.Id == gameId);


            if (g != null && Crypto.VerifyHashed(g.access_token, Convert.ToString(gameId)) && Crypto.VerifyHashed(access_link, Convert.ToString(gameId)))
            {
                PersonModel person = _dataBaseContext.People.Include(g => g.Games).First(p => p.LoginName == User.Identity.Name);

                if (person.Games.FirstOrDefault(g => g.Id == gameId) != null)
                {
                    person.LastVisitedRoom = gameId;
                    _dataBaseContext.RoomGamers.Where(i => i.GameId == gameId).First(p => p.Person == person).ConnectedToTheGame = true;
                    _dataBaseContext.SaveChanges();
                    ViewBag.gameId = gameId;
                    ViewBag.Game = _dataBaseContext.Games.Include(u => u.roomGamers).Include(p => p.People).FirstOrDefault(g => g.Id == gameId);
                    ViewBag.Photos = _dataBaseContext.Images;
                    ViewBag.gamezz = _dataBaseContext.ContextId;
                    //ViewBag.Human = _dataBaseContext.People.Include(i => i.ProfileImageId).First(i => i.LoginName == User.Identity.Name);
                    return View();
                }

            }
            return RedirectToAction("queue", "game");
        }

        [Authorize(Roles = "admin, person")]
        public IActionResult checkForLeaving(string leaverName, int roomId)
        {
            if (_dataBaseContext.Games.FirstOrDefault(b => b.Id == roomId) != null)
            {
                GameModel game = _dataBaseContext.Games.Include(rg => rg.roomGamers).First(b => b.Id == roomId);
                if (game.Creator == leaverName && game.GameStatus == "Created")
                {
                    
                    _hubContext.Clients.Group(Convert.ToString(roomId)).SendAsync("autoLeaving");
                    

                    _dataBaseContext.Games.Remove(game);
                    _dataBaseContext.SaveChanges();


                }
                if (game.GameStatus == "Started")
                {
                    PersonModel person = _dataBaseContext.People.First(n => n.LoginName == leaverName);
                    _hubContext.Clients.Client(game.roomGamers.First(i => i.PersonId == person.Id).ConnectId).SendAsync("showConfirmForDisconnectMessage");
                    return NoContent();
                }
            }
            return RedirectToAction("queue", "game");

        }
        [Authorize(Roles = "admin, person")]

        public IActionResult LeaveFromGame(string leaverName, int roomId)
        {
            GameModel game = _dataBaseContext.Games.Include(u => u.roomGamers).Include(p => p.People).FirstOrDefault(g => g.Id == roomId);

            //if (game.Creator == leaverName && game.GameStatus == "Created")
            //{
            //    //foreach (var item in game.People)
            //    //{

            //    //    _hubContext.Clients.Group(Convert.ToString(item.Id)).SendAsync("autoLeaving");
            //    //}
            //    _hubContext.Clients.Group(Convert.ToString(roomId)).SendAsync("autoLeaving");
            //    //game.GameStatus = "Cancelled";
            //    _dataBaseContext.Games.Remove(game);
            //    _dataBaseContext.SaveChanges();


            //}
            if (game.GameStatus == "Started")
            {
                if (game.roomGamers.First(i => i.Person.LoginName == leaverName).IsActive)
                {
                    NextPlayer(leaverName, game.Id);
                    game.People.First(i => i.LoginName == leaverName).IsInGame = false;
                    _dataBaseContext.SaveChanges();
                }
                game.People.First(i => i.LoginName == leaverName).IsInGame = false;
                _dataBaseContext.SaveChanges();
            }

            return RedirectToAction("queue", "game");
        }

        [Authorize(Roles = "admin, person")]
        [HttpPost]
        public IActionResult JoinToTheGame(string UserName, int roomId)
        {

            GameModel game = _dataBaseContext.Games.Include(u => u.roomGamers).Include(p => p.People).FirstOrDefault(g => g.Id == Convert.ToInt32(roomId));
            PersonModel person = _dataBaseContext.People.FirstOrDefault(p => p.LoginName == UserName);
            if (game != null)
            {
                if (game.quantityOfConnectedPeoples + 1 <= game.MaximumNumbersOfGamers && game.GameStatus == "Created")
                {
                    game.People.Add(person);
                    _dataBaseContext.SaveChanges();
                    return RedirectToAction("battlefield", $"game", new { gameId = roomId, access_link = Crypto.Hash(Convert.ToString(roomId)) });


                }

                //|| game.GameStatus == "Started" && game.roomGamers.FirstOrDefault(gamer => gamer.Person.LoginName == person.LoginName) != null

                if (game.GameStatus == "Started" && game.roomGamers.Where(i => i.GameId == roomId).FirstOrDefault(gamer => gamer.Person.LoginName == person.LoginName) != null)
                {
                    return RedirectToAction("battlefield", $"game", new { gameId = roomId, access_link = Crypto.Hash(Convert.ToString(roomId)) });

                }

            }


            //_________________________________________________________
            //Тестовое присоединение

            //if (game.People.Count + 1 <= game.MaximumNumbersOfGamers)
            //{
            //    if (game.roomGamers.First(rg => rg.Person.LoginName == UserName) == null)
            //    {
            //        game.People.Add(person);
            //        _dataBaseContext.SaveChanges();

            //    }
            //    else
            //    {
            //        game.roomGamers.First(rg => rg.Person.LoginName == UserName).ConnectedToTheGame = true;
            //        _dataBaseContext.SaveChanges();
            //    }

            //    return RedirectToAction("battlefield", $"game", new { gameId = roomId });

            //}

            //_________________________________________________________



            //person = _dataBaseContext.People.Include(im => im.ProfileImageId).FirstOrDefault(p => p.LoginName == UserName);

            //await _hubContext.Clients.All.SendAsync("ShowNewPerson", person.LoginName, person.ProfileImageId.ImageSource);

            ////await _hubContext.Groups.AddToGroupAsync(person.LoginName, Convert.ToString(game.Id));

            //ChatHub hub = new();

            //await hub.OnConnectedAsync();

            ////await _hubContext.Clients.Group(Convert.ToString(game.Id)).SendAsync("ShowNewPerson", person.LoginName, person.ProfileImageId.ImageSource);

            ////await _hubContext.Clients.Groups(Convert.ToString(game.Id)).SendAsync("ShowNewPerson", person.LoginName, person.ProfileImageId.ImageSource);

            return RedirectToAction("queue", "game");

            //_hubContext.Clients.Caller("")
        }

        [Authorize(Roles = "admin, person")]

        public IActionResult StartGame(string roomId)
        {
            GameModel game = _dataBaseContext.Games.Include(u => u.roomGamers).Include(p => p.People).FirstOrDefault(g => g.Id == Convert.ToInt32(roomId));

            
            if (game.roomGamers.Count > 1)
            {

                

                game.GameStatus = "Started";
                game.StartTime = DateTime.Now;



                Random rand = new Random();
                int temp = rand.Next(1, _dataBaseContext.Words.Count());
                WordModel firstWord = _dataBaseContext.Words.First(w => w.Id == temp);

                _hubContext.Clients.Group(roomId).SendAsync("OpponentWordAdd", firstWord.Name, "System");
                _hubContext.Clients.Group(roomId).SendAsync("hideStartBtn");

                _dataBaseContext.Messages.Add(new MessageModel
                {
                    TextMeassage = firstWord.Name,
                    Sender = "System",
                    Date = DateTime.Now,
                    Game = game
                }); ;

                int[] OrderOfTheMove = new int[game.roomGamers.Count];
                for (int i = 0; i < OrderOfTheMove.Length; i++)
                {
                    int tempRand = rand.Next(1, OrderOfTheMove.Length + 1);
                    if (!(OrderOfTheMove.Contains(tempRand)))
                    {
                        OrderOfTheMove[i] = tempRand;
                    }
                    else
                    {
                        i--;
                    }
                }
                //foreach (var elem in game.roomGamers)
                //{
                //    OrderOfTheMove += 1;
                //    elem.OrderOfTheMove = OrderOfTheMove;
                //}
                int counter = 0;
                foreach (var gamer in game.roomGamers)
                {
                    
                    if (gamer.ConnectedToTheGame)
                    {
                        gamer.OrderOfTheMove = OrderOfTheMove[counter];

                        gamer.Person.IsInGame = true;
                        counter++;
                    }
                    else
                    {
                        game.People.Remove(gamer.Person);
                    }
                }
                _dataBaseContext.SaveChanges();


                game.roomGamers.FirstOrDefault(order => order.OrderOfTheMove == 1).IsActive = true;
                _dataBaseContext.SaveChanges();

                //_hubContext.Clients.Group(roomId).SendAsync("whoMustMove", game.roomGamers.FirstOrDefault(order => order.OrderOfTheMove == 1).Person.LoginName);
                _hubContext.Clients.GroupExcept(Convert.ToString(game.Id), game.roomGamers.First(i => i.IsActive == true).ConnectId).SendAsync("whoMustMoveNoActiveGamer", game.roomGamers.First(i => i.IsActive == true).Person.LoginName);
                _hubContext.Clients.Client(game.roomGamers.First(i => i.IsActive == true).ConnectId).SendAsync("whoMustMoveActiveGamer");

            }



            return NoContent();
        }

        [Authorize(Roles = "admin, person")]

        public IActionResult SendTheWord(string theWord, string username, int gameId, int timeLeft)
        {
            GameModel game = _dataBaseContext.Games.Include(u => u.roomGamers).Include(p => p.People).Include(w => w.Messages).FirstOrDefault(g => g.Id == gameId);
            if (game != null)
            {

                string gamerId = game.roomGamers.FirstOrDefault(n => n.Person.LoginName == username).ConnectId;
                if (game.GameStatus == "Started")
                {




                    string theLastWord = game.Messages.OrderByDescending(i => i.Id).First().TextMeassage;
                    RoomGamer activeGamer = null;
                    foreach (var item in game.roomGamers)
                    {
                        if (item.IsActive)
                        {
                            //activeGamer = item.Person.LoginName;
                            activeGamer = item;
                            break;
                        }
                    }

                    if (username == activeGamer.Person.LoginName)
                    {
                        if (theWord != null)
                        {
                            theWord = theWord.ToLower();

                            WordModel word = _dataBaseContext.Words.FirstOrDefault(w => w.Name == theWord);
                            if (word != null)
                            {
                                if (game.Messages.FirstOrDefault(w => w.TextMeassage == theWord) == null)
                                {
                                    if (theLastWord[theLastWord.Length - 1] == theWord[0] ||
                                        theLastWord[theLastWord.Length - 1] == 'ь' && theLastWord[theLastWord.Length - 2] == theWord[0] ||
                                        theLastWord[theLastWord.Length - 1] == 'ы' && theLastWord[theLastWord.Length - 2] == theWord[0] ||
                                        theLastWord[theLastWord.Length - 1] == 'ъ' && theLastWord[theLastWord.Length - 2] == theWord[0])
                                    {
                                        _dataBaseContext.Messages.Add(new MessageModel
                                        {
                                            TextMeassage = theWord,
                                            Sender = username,
                                            Date = DateTime.Now,
                                            Game = game
                                        });



                                        word.NumberOfUses += 1;
                                        int startNumber = _dataBaseContext.Words.Sum(u => u.NumberOfUses);
                                        _dataBaseContext.GameInfo.First().QuantityOfUsedWords = startNumber;
                                        _dataBaseContext.SaveChanges();

                                        int quantityDivides = 0;
                                        //int startNumber = _dataBaseContext.GameInfo.First().QuantityOfUsedWords;
                                        //int startNumber = _dataBaseContext.Words.Sum(u => u.NumberOfUses);


                                        while (startNumber > 100)
                                        {
                                            startNumber /= word.NumberOfUses;
                                            quantityDivides += 1;
                                        }

                                        activeGamer.Score += quantityDivides * timeLeft / 10;
                                        activeGamer.Person.TotalScore += quantityDivides * timeLeft / 10; ;
                                        _dataBaseContext.SaveChanges();

                                        _hubContext.Clients.GroupExcept(Convert.ToString(gameId), gamerId).SendAsync("OpponentWordAdd", theWord, username);
                                        _hubContext.Clients.Client(gamerId).SendAsync("UsersWordAdd", theWord, username);
                                        _hubContext.Clients.Group(Convert.ToString(gameId)).SendAsync("changeUserScore", username, activeGamer.Score);

                                        if (activeGamer.Score >= 255)
                                        {
                                            game.GameStatus = "Ended";
                                            game.GameEnded = DateTime.Now;
                                            foreach (var person in game.People)
                                            {
                                                person.IsInGame = false;
                                            }
                                            activeGamer.IsWinner = true;
                                            game.Winner = activeGamer.Person.LoginName;
                                            activeGamer.Person.QuantityOfWins++;

                                            _dataBaseContext.SaveChanges();

                                            _hubContext.Clients.Group(Convert.ToString(gameId)).SendAsync("whoMustMoveNoActiveGamer", " ");

                                            _hubContext.Clients.Group(Convert.ToString(gameId)).SendAsync("endedGameMessage", username, activeGamer.Score);

                                            return NoContent();
                                        }



                                        if (game.quantityOfConnectedPeoples > 1)
                                        {
                                            bool isSwappedPlayer = false;
                                            int numberOfOrder = activeGamer.OrderOfTheMove + 1;
                                            while (!isSwappedPlayer)
                                            {
                                                if (numberOfOrder <= game.roomGamers.Count)
                                                {
                                                    if (game.roomGamers.First(i => i.OrderOfTheMove == numberOfOrder).ConnectedToTheGame)
                                                    {
                                                        activeGamer.IsActive = false;
                                                        RoomGamer nextPlayerToMove = game.roomGamers.FirstOrDefault(swap => swap.OrderOfTheMove == numberOfOrder);
                                                        nextPlayerToMove.IsActive = true;
                                                        _dataBaseContext.SaveChanges();
                                                        //_hubContext.Clients.Group(Convert.ToString(gameId)).SendAsync("whoMustMove", nextPlayerToMove.Person.LoginName);
                                                        _hubContext.Clients.GroupExcept(Convert.ToString(gameId), nextPlayerToMove.ConnectId).SendAsync("whoMustMoveNoActiveGamer", nextPlayerToMove.Person.LoginName);
                                                        _hubContext.Clients.Client(nextPlayerToMove.ConnectId).SendAsync("whoMustMoveActiveGamer", nextPlayerToMove.Person.LoginName);
                                                        isSwappedPlayer = true;
                                                    }
                                                    else
                                                    {
                                                        numberOfOrder++;
                                                    }

                                                }
                                                else
                                                {
                                                    numberOfOrder = 1;
                                                    if (game.roomGamers.First(i => i.OrderOfTheMove == numberOfOrder).ConnectedToTheGame)
                                                    {
                                                        activeGamer.IsActive = false;
                                                        RoomGamer nextPlayerToMove = game.roomGamers.FirstOrDefault(swap => swap.OrderOfTheMove == numberOfOrder);
                                                        nextPlayerToMove.IsActive = true;
                                                        _dataBaseContext.SaveChanges();
                                                        //_hubContext.Clients.Group(Convert.ToString(gameId)).SendAsync("whoMustMove", nextPlayerToMove.Person.LoginName);
                                                        _hubContext.Clients.GroupExcept(Convert.ToString(gameId), nextPlayerToMove.ConnectId).SendAsync("whoMustMoveNoActiveGamer", nextPlayerToMove.Person.LoginName);
                                                        _hubContext.Clients.Client(nextPlayerToMove.ConnectId).SendAsync("whoMustMoveActiveGamer", nextPlayerToMove.Person.LoginName);
                                                        isSwappedPlayer = true;
                                                    }
                                                    else
                                                    {
                                                        numberOfOrder++;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            game.GameStatus = "Ended";
                                            game.GameEnded = DateTime.Now;
                                            foreach (var person in game.People)
                                            {
                                                person.IsInGame = false;
                                            }
                                            activeGamer.IsWinner = true;
                                            game.Winner = activeGamer.Person.LoginName;

                                            _dataBaseContext.SaveChanges();

                                            _hubContext.Clients.Group(Convert.ToString(gameId)).SendAsync("whoMustMoveNoActiveGamer", " ");

                                            _hubContext.Clients.Group(Convert.ToString(gameId)).SendAsync("endedGameMessage", username, activeGamer.Score);

                                            return NoContent();
                                        }


                                        //if (activeGamer.OrderOfTheMove + 1 <= game.roomGamers.Count)
                                        //{
                                        //    activeGamer.IsActive = false;
                                        //    RoomGamer nextPlayerToMove = game.roomGamers.FirstOrDefault(swap => swap.OrderOfTheMove == activeGamer.OrderOfTheMove + 1);
                                        //    nextPlayerToMove.IsActive = true;
                                        //    _dataBaseContext.SaveChanges();
                                        //    //_hubContext.Clients.Group(Convert.ToString(gameId)).SendAsync("whoMustMove", nextPlayerToMove.Person.LoginName);
                                        //    _hubContext.Clients.GroupExcept(Convert.ToString(gameId), nextPlayerToMove.ConnectId).SendAsync("whoMustMoveNoActiveGamer", nextPlayerToMove.Person.LoginName);
                                        //    _hubContext.Clients.Client(nextPlayerToMove.ConnectId).SendAsync("whoMustMoveActiveGamer", nextPlayerToMove.Person.LoginName);
                                        //}
                                        //else
                                        //{
                                        //    activeGamer.IsActive = false;
                                        //    RoomGamer nextPlayerToMove = game.roomGamers.FirstOrDefault(swap => swap.OrderOfTheMove == 1);
                                        //    nextPlayerToMove.IsActive = true;
                                        //    _dataBaseContext.SaveChanges();
                                        //    //_hubContext.Clients.Group(Convert.ToString(gameId)).SendAsync("whoMustMove", nextPlayerToMove.Person.LoginName);
                                        //    _hubContext.Clients.GroupExcept(Convert.ToString(gameId), nextPlayerToMove.ConnectId).SendAsync("whoMustMoveNoActiveGamer", nextPlayerToMove.Person.LoginName);
                                        //    _hubContext.Clients.Client(nextPlayerToMove.ConnectId).SendAsync("whoMustMoveActiveGamer", nextPlayerToMove.Person.LoginName);
                                        //}

                                        //_hubContext.Clients.Group(Convert.ToString(gameId)).SendAsync("UsersWordAdd", theWord, username);
                                        //_hubContext.Clients.GroupExcept(Convert.ToString(gameId), activeGamerId).SendAsync("UsersWordAdd", theWord, username);
                                        //_hubContext.Clients.User(activeGamerId).SendAsync("OpponentWordAdd",  theWord, username);
                                    }
                                    else
                                    {
                                        //СЛОВО ДОЛЖНО НАЧИНАТЬСЯ НА БУКВУ Х
                                        if (theLastWord[theLastWord.Length - 1] == 'ь' ||
                                            theLastWord[theLastWord.Length - 1] == 'ъ' ||
                                            theLastWord[theLastWord.Length - 1] == 'ы')
                                        {
                                            _hubContext.Clients.Client(gamerId).SendAsync("wordStartWithWrongLetter", theLastWord[theLastWord.Length - 2]);
                                        }
                                        else
                                        {
                                            _hubContext.Clients.Client(gamerId).SendAsync("wordStartWithWrongLetter", theLastWord[theLastWord.Length - 1]);
                                        }

                                    }
                                }
                                else
                                {
                                    //СЛОВО БЫЛО ИСПОЛЬЗОВАНО
                                    _hubContext.Clients.Client(gamerId).SendAsync("wordHasBeenUsed");
                                }
                            }
                            else
                            {
                                //ТАКОГО СЛОВА НЕ СУЩЕСТВУЕТ
                                _hubContext.Clients.Client(gamerId).SendAsync("wordDoesntExist");
                            }
                        }
                        else
                        {
                            //ПУСТОЕ ПОЛЕ
                            _hubContext.Clients.Client(gamerId).SendAsync("emptyField");

                        }

                    }

                    else
                    {
                        //СЕЙЧАС НЕ ВАША ОЧЕРЕДЬ
                        _hubContext.Clients.Client(gamerId).SendAsync("queueMistake");
                    }

                }
                else if (game.GameStatus == "Created")
                {
                    _hubContext.Clients.Client(gamerId).SendAsync("gameWasntStarted");

                }
                else
                {
                    _hubContext.Clients.Client(gamerId).SendAsync("anotherMistakeWithSendingMessage");
                }
            return NoContent();
            }
            return RedirectToAction("queue", "game");

        }



        [Authorize(Roles = "admin, person")]

        public IActionResult NextPlayer(string username, int gameId)
        {
            GameModel game = _dataBaseContext.Games.Include(u => u.roomGamers).Include(p => p.People).Include(w => w.Messages).FirstOrDefault(g => g.Id == gameId);

            RoomGamer gamer = _dataBaseContext.RoomGamers.Where(i => i.GameId == gameId).FirstOrDefault(f => f.Person.LoginName == username);
            //if (gamer.IsActive)
            //{
            //    if (gamer.OrderOfTheMove + 1 <= game.roomGamers.Count)
            //    {
            //        gamer.IsActive = false;
            //        RoomGamer nextPlayerToMove = game.roomGamers.FirstOrDefault(swap => swap.OrderOfTheMove == gamer.OrderOfTheMove + 1);
            //        nextPlayerToMove.IsActive = true;
            //        _dataBaseContext.SaveChanges();
            //        _hubContext.Clients.GroupExcept(Convert.ToString(gameId), nextPlayerToMove.ConnectId).SendAsync("whoMustMoveNoActiveGamer", nextPlayerToMove.Person.LoginName, User.Identity.Name);
            //        _hubContext.Clients.Client(nextPlayerToMove.ConnectId).SendAsync("whoMustMoveActiveGamer", nextPlayerToMove.Person.LoginName, User.Identity.Name);

            //    }
            //    else
            //    {
            //        gamer.IsActive = false;
            //        RoomGamer nextPlayerToMove = game.roomGamers.FirstOrDefault(swap => swap.OrderOfTheMove == 1);
            //        nextPlayerToMove.IsActive = true;
            //        _dataBaseContext.SaveChanges();
            //        //_hubContext.Clients.Group(Convert.ToString(gameId)).SendAsync("whoMustMoveNoActiveGamer", nextPlayerToMove.Person.LoginName);
            //        _hubContext.Clients.GroupExcept(Convert.ToString(gameId), nextPlayerToMove.ConnectId).SendAsync("whoMustMoveNoActiveGamer", nextPlayerToMove.Person.LoginName, User.Identity.Name);
            //        _hubContext.Clients.Client(nextPlayerToMove.ConnectId).SendAsync("whoMustMoveActiveGamer", nextPlayerToMove.Person.LoginName, User.Identity.Name);
            //    }
            //}


            //return NoContent();

            if (gamer.IsActive)
            {
                if (game.quantityOfConnectedPeoples > 1)
                {
                    bool isSwappedPlayer = false;
                    int numberOfOrder = gamer.OrderOfTheMove + 1;
                    while (!isSwappedPlayer)
                    {
                        if (numberOfOrder <= game.roomGamers.Count)
                        {
                            if (game.roomGamers.First(i => i.OrderOfTheMove == numberOfOrder).ConnectedToTheGame)
                            {
                                gamer.IsActive = false;
                                RoomGamer nextPlayerToMove = game.roomGamers.FirstOrDefault(swap => swap.OrderOfTheMove == numberOfOrder);
                                nextPlayerToMove.IsActive = true;
                                _dataBaseContext.SaveChanges();
                                //_hubContext.Clients.Group(Convert.ToString(gameId)).SendAsync("whoMustMove", nextPlayerToMove.Person.LoginName);
                                _hubContext.Clients.GroupExcept(Convert.ToString(gameId), nextPlayerToMove.ConnectId).SendAsync("whoMustMoveNoActiveGamer", nextPlayerToMove.Person.LoginName);
                                _hubContext.Clients.Client(nextPlayerToMove.ConnectId).SendAsync("whoMustMoveActiveGamer", nextPlayerToMove.Person.LoginName);
                                isSwappedPlayer = true;
                            }
                            else
                            {
                                numberOfOrder++;
                            }

                        }
                        else
                        {
                            numberOfOrder = 1;
                            if (game.roomGamers.First(i => i.OrderOfTheMove == numberOfOrder).ConnectedToTheGame)
                            {
                                gamer.IsActive = false;
                                RoomGamer nextPlayerToMove = game.roomGamers.FirstOrDefault(swap => swap.OrderOfTheMove == numberOfOrder);
                                nextPlayerToMove.IsActive = true;
                                _dataBaseContext.SaveChanges();
                                //_hubContext.Clients.Group(Convert.ToString(gameId)).SendAsync("whoMustMove", nextPlayerToMove.Person.LoginName);
                                _hubContext.Clients.GroupExcept(Convert.ToString(gameId), nextPlayerToMove.ConnectId).SendAsync("whoMustMoveNoActiveGamer", nextPlayerToMove.Person.LoginName);
                                _hubContext.Clients.Client(nextPlayerToMove.ConnectId).SendAsync("whoMustMoveActiveGamer", nextPlayerToMove.Person.LoginName);
                                isSwappedPlayer = true;
                            }
                            else
                            {
                                numberOfOrder++;
                            }
                        }
                    }
                }
                else
                {
                    game.GameStatus = "Ended";
                    game.GameEnded = DateTime.Now;
                    foreach (var person in game.People)
                    {
                        person.IsInGame = false;
                    }
                    gamer.IsWinner = true;
                    game.Winner = gamer.Person.LoginName;

                    gamer.Person.QuantityOfWins++;
                    gamer.Person.TotalScore += gamer.Score;
                    _dataBaseContext.SaveChanges();

                    _hubContext.Clients.Group(Convert.ToString(gameId)).SendAsync("whoMustMoveNoActiveGamer", " ");

                    _hubContext.Clients.Group(Convert.ToString(gameId)).SendAsync("endedGameMessage", username, gamer.Score);

                    return NoContent();
                }
            }



            return NoContent();

        }



    }
}