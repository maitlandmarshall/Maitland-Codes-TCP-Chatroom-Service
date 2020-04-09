using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChatroomLib.Tests
{
    [TestClass]
    public class MessageContractByteSerializerTests
    {
        [TestMethod]
        public void SerializeMessageContract_ARandomString_DeserializesCorrectly()
        {
            MessageContractByteSerializer serializer = new MessageContractByteSerializer();
            using MemoryStream ms = new MemoryStream();

            Random randy = new Random();

            for (int i = 0; i < 100; i++)
            {
                byte[] randBuffer = new byte[randy.Next(32, 1024)];
                randy.NextBytes(randBuffer);

                string randString = Encoding.ASCII.GetString(randBuffer);

                MessageContract contract = new MessageContract
                {
                    Message = randString
                };

                int bytesWritten = serializer.SerializeMessageContract(contract, ms);

                ms.Position -= bytesWritten;

                MessageContract deserializedContract = serializer.DeserializeStreamIntoMessageContract(ms);

                Assert.AreEqual(contract.Message, deserializedContract.Message);
            }
        }
    }
}
