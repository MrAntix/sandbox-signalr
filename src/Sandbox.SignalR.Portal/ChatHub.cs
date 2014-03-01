using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNet.SignalR;

namespace Sandbox.SignalR.Portal
{
    public class ChatHub : Hub
    {
        static readonly ConcurrentDictionary<string, ChatUser> Users;

        static ChatHub()
        {
            Users = new ConcurrentDictionary<string, ChatUser>();
        }

        public void Send(string message)
        {
            ChatUser current;
            if (Users.TryGetValue(Context.ConnectionId, out current))
            {
                Clients.All.broadcastMessage(current.Name, message);
            }
        }

        public void SetUser(ChatUser user)
        {
            ChatUser current;
            if (Users.TryGetValue(Context.ConnectionId, out current))
            {
                Users.TryUpdate(Context.ConnectionId, user, current);
            }

            WriteLine("SetUser");
        }

        public override Task OnConnected()
        {
            WriteLine("Connected");

            Users.TryAdd(Context.ConnectionId, ChatUser.New);

            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            WriteLine("Disconnected");

            ChatUser user;
            Users.TryRemove(Context.ConnectionId, out user);

            return base.OnDisconnected();
        }

        public override Task OnReconnected()
        {
            WriteLine("Reconnected");

            Users.AddOrUpdate(Context.ConnectionId, ChatUser.New, (o, n) => n);

            return base.OnReconnected();
        }

        void WriteLine(string text)
        {
            using (var file = new StreamWriter(@"C:\temp\hub.txt", true))
            {
                ChatUser current;
                var name = ChatUser.New.Name;
                if (Users.TryGetValue(Context.ConnectionId, out current))
                {
                    name = current.Name;
                }

                file.WriteLine(
                    "{0} ({1}): {2}",
                    name,
                    Context.ConnectionId,
                    text
                    );
            }
        }
    }
}