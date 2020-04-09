using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace TcpChatroom
{
    public class MessageFactory
    {
        public void WriteMessage(string message, Stream clientStream, uint userId = 0)
        {
            MessageHeader messageHeader = new MessageHeader
            {
                Length = (uint)Encoding.UTF8.GetBytes(message).Length,
                UserId = userId
            };

            // Now we must write the message header through the stream
            // The message header reserves the first 32 bytes for the total length
            // And the next 32 bytes for the UserId

            sw.Write(messageHeader.Length);
            sw.Write(messageHeader.UserId);

            sw.Write(message);

            sw.Flush();
        }

        public MessageHeader ReadMessageHeader(Stream sr)
        {
            byte[] messageHeaderLengthBuffer = new byte[4];
            byte[] messageHeaderUserIdBuffer = new byte[4];

            sr.Read(messageHeaderLengthBuffer, 0, messageHeaderLengthBuffer.Length);
            sr.Read(messageHeaderUserIdBuffer, 0, messageHeaderUserIdBuffer.Length);

            uint messageHeaderLength = BitConverter.ToUInt32(messageHeaderLengthBuffer);
            uint messageHeaderUserId = BitConverter.ToUInt32(messageHeaderUserIdBuffer);

            return new MessageHeader
            {
                Length = messageHeaderLength,
                UserId = messageHeaderUserId
            };
        }

        public string ReadMessage (MessageHeader header, Stream ns)
        {
            byte[] messageBuffer = new byte[1024];

            // Read the actual message
            ns.Read(messageBuffer, 0, (int)header.Length);

            string message = Encoding.UTF8.GetString(messageBuffer);

            return message;
        }
    }
}
