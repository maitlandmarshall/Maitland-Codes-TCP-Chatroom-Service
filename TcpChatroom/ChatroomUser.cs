using System.Net.Sockets;

namespace TcpChatroom
{
    public class ChatroomUser
    {
        private static uint LastId = 0;

        public uint Id { get; set; }
        public string Name { get; set; }

        internal TcpClient TcpClient { get; }

        public ChatroomUser(TcpClient client)
        {
            this.TcpClient = client;

            LastId++;

            this.Id = LastId;
        }
    }
}
