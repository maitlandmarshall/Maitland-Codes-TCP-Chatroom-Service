using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpChatroom
{
    public class ChatroomServer
    {
        private const uint ServerUserId = 0;

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private TcpListener tcpListener;
        private readonly List<ChatroomUser> chatroomUsers = new List<ChatroomUser>();
        private readonly MessageFactory messageFactory = new MessageFactory();

        public async Task Start()
        {
            this.tcpListener = new TcpListener(IPAddress.Any, 1337);
            this.tcpListener.Start();

            // Put the tcpListener client handler in it's own task so we can continue and connect with the TcpClient
            _ = Task.Run(async () =>
            {
                while (!this.cancellationTokenSource.IsCancellationRequested)
                {
                    TcpClient tcpClient = await this.tcpListener.AcceptTcpClientAsync();

                    this.chatroomUsers.Add(new ChatroomUser(tcpClient));

                    _ = this.HandleTcpClientAsync(tcpClient);
                }
            });
        }

        private async Task HandleTcpClientAsync(TcpClient client)
        {
            // When the TcpClient connects, send it a message

            using NetworkStream ns = client.GetStream();
            using StreamWriter sw = new StreamWriter(ns);
            using StreamReader sr = new StreamReader(ns);

            string helloMessage = "Hello, what is your name?";

            this.messageFactory.WriteMessage(helloMessage, ns, ServerUserId);

            // Read the header of the message
            MessageHeader header = this.messageFactory.ReadMessageHeader(ns);
            string name = this.messageFactory.ReadMessage(header, ns);

            // Write the message to all the clients in the chatroom
            foreach (ChatroomUser c in this.chatroomUsers)
            {
                this.messageFactory.WriteMessage($"Everyone FUCKIN WELCOME {header.UserId} to the chatroom.", c.TcpClient.GetStream(), ServerUserId);
            }

            // Flush the stream so the client gets the message
            await ns.FlushAsync();
        }
    }
}
