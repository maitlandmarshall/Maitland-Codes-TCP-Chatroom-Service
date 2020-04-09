using System;
using System.Collections.Generic;
using System.Text;

namespace TcpChatroom
{
    public class MessageHeader
    {
        public uint Length { get; set; }
        public uint UserId { get; set; }
    }
}
