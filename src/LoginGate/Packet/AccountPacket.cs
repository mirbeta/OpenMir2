using System;
using System.IO;
using SystemModule;

namespace LoginGate
{
    public class AccountPacket : Packets
    {
        private readonly AccountPacktType PacketType;
        private readonly int SocketId;
        private readonly byte[] Buffer;

        public AccountPacket(AccountPacktType type, int socketId, byte[] buffer)
        {
            PacketType = type;
            SocketId = socketId;
            Buffer = buffer;
        }

        public AccountPacket(AccountPacktType type, int socketId, string str)
        {
            PacketType = type;
            SocketId = socketId;
            if (!string.IsNullOrEmpty(str))
            {
                Buffer = HUtil32.GetBytes(str);
            }
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write('%');
            switch (PacketType)
            {
                case AccountPacktType.Data:
                    writer.Write('A');
                    break;
                case AccountPacktType.Open:
                    writer.Write('O');
                    break;
                case AccountPacktType.Leave:
                    writer.Write('X');
                    break;
            }
            writer.Write(SocketId);
            if (Buffer.Length > 0)
            {
                writer.Write('/');
                writer.Write(Buffer);
            }
            writer.Write('$');
        }
    }

    public enum AccountPacktType : byte
    {
        Data = 0,
        Open = 1,
        Leave = 2
    }
}