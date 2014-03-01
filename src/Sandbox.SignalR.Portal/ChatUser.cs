namespace Sandbox.SignalR.Portal
{
    public class ChatUser
    {
        public string Name { get; set; }

        public static readonly ChatUser New = new ChatUser {Name = "(new user)"};
    }
}