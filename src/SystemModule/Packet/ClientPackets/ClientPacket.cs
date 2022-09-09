using System.IO;

namespace SystemModule.Packet.ClientPackets
{
    /// <summary>
    /// 客户端消息
    /// </summary>
    public class ClientPacket : Packets
    {
        public int UID;
        public ushort Cmd;
        public ushort X;
        public ushort Y;
        public ushort Direct;
        public int ID1;
        public ushort Cmd1;
        public int ID2;
        public ushort PosX;
        public ushort PosY;
        public ushort Cmd2;
        public ushort IDLo;
        public ushort Magic;
        public ushort IDHi;
        public int UID1;
        public ushort Cmd3;
        public byte b1;
        public byte b2;
        public byte b3;
        public byte b4;
        public int NID;
        public ushort Command;
        public ushort Pos;
        public ushort Dir;
        public ushort WID;
        public double Head;
        public ushort Cmd4;
        public ushort Zero1;
        public double Tail;
        public int Recog;
        public ushort Ident;
        public ushort Param;
        public ushort Tag;
        public ushort Series;
        public int OtpCode;

        public const int PackSize = 12;

        protected override void ReadPacket(BinaryReader reader)
        {
            Recog = reader.ReadInt32();
            Ident = reader.ReadUInt16();
            Param = reader.ReadUInt16();
            Tag = reader.ReadUInt16();
            Series = reader.ReadUInt16();

            Cmd = Ident;
            Cmd1 = Ident;
            Cmd2 = Ident;
            Cmd3 = Ident;
            Cmd4 = Ident;
            Command = Ident;

            UID = Recog;
            Head = Recog;
            NID = Recog;
            UID1 = Recog;
            PosX = (ushort)Recog;
            ID1 = Recog;

            X = Param;
            IDLo = Param;
            b1 = (byte)Param;
            Pos = Param;
            Zero1 = Param;

            Y = Tag;
            Dir = Tag;
            b3 = (byte)Tag;
            Magic = Tag;

            Direct = Series;
            WID = Series;
            IDHi = Series;
        }

        protected override void WritePacket(BinaryWriter writer)
        {
           writer.Write(Recog);
           writer.Write(Ident);
           writer.Write(Param);
           writer.Write(Tag);
           writer.Write(Series);
        }
    }
}