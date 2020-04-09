using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TcpChatroom
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatroomServer server = new ChatroomServer();
            server.Start();

            MessageFactory factory = new MessageFactory();

            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(IPAddress.Loopback, 1337);

            Stream clientStream = tcpClient.GetStream();

            // Read the initial message from the server
            MessageHeader header = factory.ReadMessageHeader(clientStream);
            string msg = factory.ReadMessage(header, clientStream);

            // Message should be "whats ur name"?
            factory.WriteMessage("DIK FUK", clientStream);
        }
    }
}
