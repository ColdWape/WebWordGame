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

        public ChatHub(/*DataBaseContext dataBaseContext*/)
        {
            //_dataBaseContext = dataBaseContext;
        }

        //public async Task AddToTheGroup(PersonModel usermodel, string groupname, string image)
        public override async Task OnConnectedAsync()
        {
            var userName = Context.User.Identity.Name;
            var connectionId = Context.ConnectionId;
            //_chatManager.ConnectUser(userName, connectionId);
            //await UpdateUsersAsync();
            await Clients.All.SendAsync("f", connectionId);
            await base.OnConnectedAsync();
        }
        public async Task AddToTheGroup(string name, string groupname)
        {
            var userName = Context.User.Identity.Name;
            var connectionId = Context.ConnectionId;
            _chatManager.ConnectUser(userName, connectionId);
            //await UpdateUsersAsync();

            //var userName = name;
            //var connectionId = Context.ConnectionId;
            //_chatManager.ConnectUser(userName, connectionId);
            //await Groups.AddToGroupAsync(connectionId, groupname);

            //await UpdateUsersAsync();


            //string username = usermodel.LoginName;
            //await Groups.AddToGroupAsync(Context.ConnectionId, groupname);

            //await Clients.Group(groupname).SendAsync("ShowNewPerson", usermodel.LoginName, usermodel.ProfileImageId.ImageSource);

        }
         public async Task WhatTheRoomId(int numb)
        {
            await Clients.All.SendAsync("ShowNewPerson", "qwerty", "https://kartinkin.net/uploads/posts/2021-07/1626148911_50-kartinkin-com-p-tokio-drift-anime-anime-krasivo-52.jpg");
        }

        


        //public async Task UpdateUsersAsync()
        //{
        //    var users = _chatManager.Users.Select(x => x.UserName).ToList();
        //    await Clients.All.UpdateUsersAsync(users);
        //}

        //public async Task SendMessageAsync(string userName, string message) =>
        //    await Clients.All.SendMessageAsync(userName, message);



    }
}
