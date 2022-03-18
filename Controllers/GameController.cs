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





        public IActionResult CheckFunc(int gameID)
        {
            if (gameID == 1)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                return RedirectToAction("GameTypeChoosingPage", "home");
            }
        }


        

        [HttpGet]
        public IActionResult Queue()
        {
            ViewBag.Games = _dataBaseContext.Games.Include(p => p.People).ToList();

            return View();
        }

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
                    Creator = person.LoginName
                };


                newGame.People.Add(person);
                


                _dataBaseContext.Games.Add(newGame);
                _dataBaseContext.SaveChanges();

                _hubContext.Groups.AddToGroupAsync(person.LoginName, Convert.ToString(newGame.Id));
                
                return RedirectToAction("battlefield", $"game", new{ gameId = newGame.Id});
                
            }

            return View();
        }



        [HttpGet]
        public IActionResult BattleField(int gameId)
        {
            ViewBag.gameId = gameId;
            ViewBag.Game = _dataBaseContext.Games.Include(u => u.roomGamers).Include(p => p.People).FirstOrDefault(g => g.Id == gameId);
            ViewBag.Photos = _dataBaseContext.Images;
            ViewBag.gamezz = _dataBaseContext.ContextId;
            //ViewBag.Human = _dataBaseContext.People.Include(i => i.ProfileImageId).First(i => i.LoginName == User.Identity.Name);
            return View();
        }

       
        [HttpPost]
        public async Task<IActionResult> JoinToTheGame(string UserName, int roomId)
        {
            GameModel game = _dataBaseContext.Games.FirstOrDefault(g => g.Id == roomId);
            PersonModel person = _dataBaseContext.People.FirstOrDefault(p => p.LoginName == UserName);

            game.People.Add(person);
            _dataBaseContext.SaveChanges();

            //person = _dataBaseContext.People.Include(im => im.ProfileImageId).FirstOrDefault(p => p.LoginName == UserName);


            ////await _hubContext.Groups.AddToGroupAsync(person.LoginName, Convert.ToString(game.Id));

            //ChatHub hub = new();

            //await hub.OnConnectedAsync();
            
            ////await _hubContext.Clients.Group(Convert.ToString(game.Id)).SendAsync("ShowNewPerson", person.LoginName, person.ProfileImageId.ImageSource);

            ////await _hubContext.Clients.Groups(Convert.ToString(game.Id)).SendAsync("ShowNewPerson", person.LoginName, person.ProfileImageId.ImageSource);


             return RedirectToAction("battlefield", $"game", new{ gameId = roomId });

        }
        
    }
}
