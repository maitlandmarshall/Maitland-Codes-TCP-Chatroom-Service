using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpClientVideo
{
    [TestClass]
    public class TcpClientTests
    {
        [TestMethod]
        public async Task ReadFromNetworkStream_TcpListenerEndpoint_DownloadsStreamAsync()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 1337);
            listener.Start();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            // Put the tcpListener client handler in it's own task so we can continue and connect with the TcpClient
            _ = Task.Run(async () =>
            {
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    var tcpClient = await listener.AcceptTcpClientAsync();
                    _ = this.HandleTcpClientAsync(tcpClient);
                }
            });


            using TcpClient client = new TcpClient();
            await client.ConnectAsync(IPAddress.Loopback, 1337);

            using NetworkStream ns = client.GetStream();
            using StreamReader sr = new StreamReader(ns);

            string message = await sr.ReadToEndAsync();

            Assert.AreEqual(message, "YO DAWG WASSAP?");
        }

        private async Task HandleTcpClientAsync (TcpClient client)
        {
            // When the TcpClient connects, send it a message

            using NetworkStream ns = client.GetStream();
            using StreamWriter sw = new StreamWriter(ns);

            await sw.WriteAsync("YO DAWG WASSAP?");

            // Flush the stream so the client gets the message
            await ns.FlushAsync();
        }
    }
}
