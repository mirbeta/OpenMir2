using System;
using System.IO;

namespace SystemModule.Packet.ServerPackets
{
    public enum PacketType : byte
    {
        Enter = 0,
        Leave = 1,
        Data = 2,
        KeepAlive = 3
    }

    public class GatePacket : Packets
    {
        public PacketType PacketType { get; set; }
        public string SocketId { get; set; }
        public short BuffLen { get; set; }
        public byte[] Body { get; set; }
        public char StartChar { get; set; }
        public char EndChar { get; set; }

        protected override void ReadPacket(BinaryReader reader)
        {
            StartChar = reader.ReadChar();
            PacketType = (PacketType)reader.ReadByte();
            SocketId = reader.ReadString();
            BuffLen = reader.ReadInt16();
            Body = reader.ReadBytes(BuffLen);
            EndChar = reader.ReadChar();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(StartChar);
            writer.Write((byte)PacketType);
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
}