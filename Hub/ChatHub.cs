using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebWordGame.Models;

namespace WebWordGame
{
    public class ChatHub : Hub
    {

        private readonly DataBaseContext _dataBaseContext;
        private readonly ChatManager _chatManager;

        public ChatHub(/*ChatManager chatManager,*/DataBaseContext dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
            //_chatManager = chatManager;
        }

        //public async Task AddToTheGroup(PersonModel usermodel, string groupname, string image)
        public override async Task OnConnectedAsync()
        {




            var userName = Context.User.Identity.Name;
            PersonModel person = _dataBaseContext.People.Include(gamez => gamez.Games).Include(ph => ph.ProfileImageId).FirstOrDefault(u => u.LoginName == userName);


            string groupname = null;
            foreach (var item in person.Games.OrderByDescending(i => i.Id))
            {
                if (item.GameStatus != "Ended")
                {
                    groupname = Convert.ToString(item.Id);
                    break;
                }
            }


            var connectionId = Context.ConnectionId;
            RoomGamer roomGamer = _dataBaseContext.RoomGamers.First(i => i.Person == person);
            roomGamer.ConnectedToTheGame = true;
            roomGamer.ConnectId = connectionId;
            _dataBaseContext.SaveChanges();
            if (groupname != null)
            {
                await Groups.AddToGroupAsync(connectionId, groupname);

            }

            //_chatManager.ConnectUser(userName, connectionId);



            //_chatManager.ConnectUser(userName, connectionId);

            //GameModel game = _dataBaseContext.Games.FirstOrDefault(i => i.Id == Convert.ToInt32(groupname));
            //game.People.Add(person);
            //_dataBaseContext.SaveChanges();

            await Clients.GroupExcept(groupname, connectionId).SendAsync("ShowNewPerson", person.LoginName, person.ProfileImageId.ImageSource);
            await Clients.All.SendAsync("f", connectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            //var isUserRemoved = _chatManager.DisconnectUser(Context.ConnectionId);
            //if (!isUserRemoved)
            //{
            //    await base.OnDisconnectedAsync(exception);
            //}
            
            var userName = Context.User.Identity.Name;
            PersonModel person = _dataBaseContext.People.Include(gamez => gamez.Games).FirstOrDefault(u => u.LoginName == userName);

            string groupname = null;
            foreach (var item in person.Games)
            {
                if (item.GameStatus != "Ended")
                {
                    groupname = Convert.ToString(item.Id);
                    break;
                }
            }

            var connectionId = Context.ConnectionId;

            if (groupname != null)
            {
                await Groups.RemoveFromGroupAsync(connectionId, groupname);

                RoomGamer roomGamer = _dataBaseContext.RoomGamers.First(i => i.Person == person);
                roomGamer.ConnectedToTheGame = false;

                //GameModel game = _dataBaseContext.Games.FirstOrDefault(i => i.Id == Convert.ToInt32(groupname));
                //if (game != null)
                //{
                //    game.People.Remove(person);
                //}

                await Clients.GroupExcept(groupname, connectionId).SendAsync("HideDisconnectedUser", person.LoginName);

            }


            _dataBaseContext.SaveChanges();


            await base.OnDisconnectedAsync(exception);
        }





        //public override async Task OnDisconnectedAsync(Exception exception)
        //{
        //    var userName = Context.User.Identity.Name;
        //    PersonModel person = _dataBaseContext.People.Include(gamez => gamez.Games).FirstOrDefault(u => u.LoginName == userName);
        //    string groupname = null;
        //    foreach (var item in person.Games)
        //    {
        //        if (item.GameStatus != "Ended")
        //        {
        //            groupname = Convert.ToString(item.Id);
        //            break;
        //        }
        //    }

        //    var connectionId = Context.ConnectionId;

        //    if (groupname != null)
        //    {
        //        await Groups.RemoveFromGroupAsync(connectionId, groupname);

        //    }


        //    //_chatManager.ConnectUser(userName, connectionId);
        //    //await UpdateUsersAsync();
        //    await Clients.GroupExcept(groupname, connectionId).SendAsync("HideDisconnectedUser", person.LoginName);
        //    await Clients.All.SendAsync("f", connectionId);
        //    await base.OnDisconnectedAsync();
        //}







        //public async Task AddToTheGroup(string name, string groupname)
        //{
        //    var userName = Context.User.Identity.Name;
        //    var connectionId = Context.ConnectionId;
        //    _chatManager.ConnectUser(userName, connectionId);
        //    //await UpdateUsersAsync();

        //    //var userName = name;
        //    //var connectionId = Context.ConnectionId;
        //    //_chatManager.ConnectUser(userName, connectionId);
        //    //await Groups.AddToGroupAsync(connectionId, groupname);

        //    //await UpdateUsersAsync();


        //    //string username = usermodel.LoginName;
        //    //await Groups.AddToGroupAsync(Context.ConnectionId, groupname);

        //    //await Clients.Group(groupname).SendAsync("ShowNewPerson", usermodel.LoginName, usermodel.ProfileImageId.ImageSource);

        //}





        //public async Task UpdateUsersAsync()
        //{
        //    var users = _chatManager.Users.Select(x => x.UserName).ToList();
        //    await Clients.All.UpdateUsersAsync(users);
        //}

        //public async Task SendMessageAsync(string userName, string message) =>
        //    await Clients.All.SendMessageAsync(userName, message);



    }
}
