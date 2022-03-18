using System;
using System.Collections.Generic;
using System.Linq;

namespace WebWordGame
{

    public class ChatUser
    {
        private readonly List<ChatConnection> _connections;
        public string UserName { get; }

        public ChatUser(string userName)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            _connections = new List<ChatConnection>();
        }

        public IEnumerable<ChatConnection> Connections => _connections;

        public void AppendConnection(string connectionId) 
        {
            if (connectionId == null)
            {
                throw new ArgumentNullException(nameof(connectionId));
            }

            var connection = new ChatConnection
            {
                ConnectionId = connectionId
            };

            _connections.Add(connection);
        }

      
        public void RemoveConnection(string connectionId)
        {
            if (connectionId == null)
            {
                throw new ArgumentNullException(nameof(connectionId));
            }

            var connection = _connections.SingleOrDefault(x => x.ConnectionId.Equals(connectionId));
            if (connection == null)
            {
                return;
            }
            _connections.Remove(connection);
        }
    }
}
