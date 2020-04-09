using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ChatroomLib
{
    public class MessageContractByteSerializer
    {
        public byte[] SerializeMessageContract (MessageContract contract)
        {
            string contractMessage = contract.Message;

            if (String.IsNullOrEmpty(contractMessage))
                throw new ArgumentNullException($"{nameof(contract)}.{nameof(contract.Message)} must have a value.");

            byte[] contractMessageBytes = Encoding.UTF8.GetBytes(contractMessage);

            contract.Header = new MessageContractHeader
            {
                Length = contractMessageBytes.Length
            };

            using MemoryStream ms = new MemoryStream();
            using BinaryWriter bw = new BinaryWriter(ms);

            // The first 32 bits (4 bytes) will be the contract header length
            bw.Write(contract.Header.Length);

            // The actual message will now be written
            bw.Write(contractMessageBytes);

            return ms.ToArray();
        } 

        public MessageContract DeserializeBytesIntoMessageContract (byte[] bytes)
        {
            // First read the first four bytes to get the total length
            int msgByteLength = BitConverter.ToInt32(bytes, 0);

            // Then read the rest of the message
            string message = Encoding.UTF8.GetString(bytes.Skip(4).ToArray());

            return new MessageContract
            {
                Header = new MessageContractHeader
                {
                    Length = msgByteLength
                },
                Message = message
            };
        }

        public int SerializeMessageContract(MessageContract contract, Stream stream)
        {
            string contractMessage = contract.Message;

            if (String.IsNullOrEmpty(contractMessage))
                throw new ArgumentNullException($"{nameof(contract)}.{nameof(contract.Message)} must have a value.");

            byte[] contractMessageBytes = Encoding.UTF8.GetBytes(contractMessage);

            contract.Header = new MessageContractHeader
            {
                Length = contractMessageBytes.Length
            };

            BinaryWriter bw = new BinaryWriter(stream);

            // The first 32 bits (4 bytes) will be the contract header length
            bw.Write(contract.Header.Length);

            // The actual message will now be written
            bw.Write(contractMessageBytes);

            return 4 + contractMessageBytes.Length;
        }

        public MessageContract DeserializeStreamIntoMessageContract (Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);

            // First read the first four bytes to get the total length
            int msgByteLength = br.ReadInt32();

            // Then read the rest of the message
            byte[] msgBytes = br.ReadBytes(msgByteLength);
            string message = Encoding.UTF8.GetString(msgBytes);

            return new MessageContract
            {
                Header = new MessageContractHeader
                {
                    Length = msgByteLength
                },
                Message = message
            };
        }
    }
}
