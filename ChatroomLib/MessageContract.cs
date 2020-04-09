using System;
using System.Collections.Generic;
using System.Text;

namespace ChatroomLib
{
    public class MessageContractHeader
    {
        public int Length { get; set; }
    }

    public class MessageContract
    {
        internal MessageContractHeader Header { get; set; }

        public string Message { get; set; }
    }
}
