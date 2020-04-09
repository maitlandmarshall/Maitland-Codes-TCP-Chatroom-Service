using System;
using System.Collections.Generic;
using System.Text;

namespace TcpChatroom
{
    public class Chatroom
    {
        public List<ChatroomUser> Users { get; } = new List<ChatroomUser>();
    }
}
