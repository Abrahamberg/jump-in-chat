using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {

        private readonly ISessions _sessions;

        public ChatHub(ISessions sessions)
        {
            _sessions = sessions;
        }

        public async Task SendMessage(string userName, string message)
        {
            var session = _sessions.Get(Guid.NewGuid());

            if (!session.Users.Any(u => u.NicName == userName)) {
                var user = new SessionUser(Guid.NewGuid(), userName, Context.ConnectionId, "");
                session.Add(user);
            }


            if (message.StartsWith("/"))
                await SendToUser(userName,message);
            else
            await Clients.All.SendAsync("ReceiveMessage", userName, message);
        }

        private async Task SendToUser(string user, string message)
        {
            var userName = message.Split(" ")[0].Replace("/", "");
            var mainMessage = message.Substring(message.IndexOf(" "));
            var session = _sessions.Get(Guid.NewGuid());
            var ConnectionId = session.Users.First(x => x.NicName == userName).ConnectionId;


            await  Clients.Client(ConnectionId).SendAsync("ReceivePrivate", user, mainMessage); ;
        }
    }
}