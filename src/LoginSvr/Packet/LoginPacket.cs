using System;
using System.IO;
using SystemModule;

namespace LoginSvr.Packet
{
    internal class LoginPacket : Packets
    {
        public char PacketType;
        public int SocketId;
        public byte[] Body;

        public char StartChar;
        public char EndChar;

        public LoginPacket(byte[] buffer) : base(buffer)
        {
            StartChar = ReadChar();
            PacketType = ReadChar();
            SocketId = ReadInt32();
            ReadChar();
            var bodyLen = buffer.Length - 1 - 1 - 4 - 1 - 1;
            Body = ReadBytes(bodyLen);
            EndChar = ReadChar();
        }

        protected override void ReadPacket(BinaryReader reader)
        {

        }

        protected override void WritePacket(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}