using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace WebWordGame
{
    [Authorize]
    public class CommunicationHub : Hub<ICommunicationHub>
    {
        private readonly ChatManager _chatManager;
        private const string _defaultGroupName = "General";

        public CommunicationHub(ChatManager chatManager)
            => _chatManager = chatManager;

        
        public override async Task OnConnectedAsync()
        {
            var userName = Context.User.Identity.Name;
            var connectionId = Context.ConnectionId;
            _chatManager.ConnectUser(userName, connectionId);
            await Groups.AddToGroupAsync(connectionId, _defaultGroupName);
            await UpdateUsersAsync();
            await base.OnConnectedAsync();
        }

        
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var isUserRemoved = _chatManager.DisconnectUser(Context.ConnectionId);
            if (!isUserRemoved)
            {
                await base.OnDisconnectedAsync(exception);
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, _defaultGroupName);
            await UpdateUsersAsync();
            await base.OnDisconnectedAsync(exception);
        }
        
        

        public async Task UpdateUsersAsync()
        {
            var users = _chatManager.Users.Select(x => x.UserName).ToList();
            await Clients.All.UpdateUsersAsync(users);
        }     
        
        public async Task SendMessageAsync(string userName, string message) => 
            await Clients.All.SendMessageAsync(userName, message);
    }
}
