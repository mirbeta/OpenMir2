using System;
using System.IO;

namespace SystemModule.Packet.ServerPackets
{
    public class ServerDataMessage : Packets
    {
        public ServerDataType Type { get; set; }
        public int SocketId { get; set; }
        public short BuffLen { get; set; }
        public byte[] Body { get; set; }
        public char StartChar { get; set; }
        public char EndChar { get; set; }
        
        protected override void ReadPacket(BinaryReader reader)
        {
            StartChar = reader.ReadChar();
            Type = (ServerDataType)reader.ReadByte();
            SocketId = reader.ReadInt32();
            BuffLen = reader.ReadInt16();
            Body = reader.ReadBytes(BuffLen);
            EndChar = reader.ReadChar();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(StartChar);
            writer.Write((byte)Type);
            writer.Write(SocketId);
            if (Body == null || Body.Length <= 0)
            {
                writer.Write((short)0);
                writer.Write(Array.Empty<byte>());
            }
            else
            {
                writer.Write(BuffLen);
                writer.Write(Body);
            }
            writer.Write(EndChar);
        }
    }
    
    public enum ServerDataType : byte
    {
        Enter = 0,
        Leave = 1,
        Data = 2,
        KeepAlive = 3
    }
}