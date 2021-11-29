using System.IO;

namespace SystemModule.Packages
{
    public class TCmdPack
    {
        public int UID;
        public short Cmd;
        public short X;
        public short Y;
        public short Direct;
        public int ID1;
        public short Cmd1;
        public int ID2;
        public short PosX;
        public short PosY;
        public short Cmd2;
        public short IDLo;
        public short Magic;
        public short IDHi;
        public int UID1;
        public short Cmd3;
        public byte b1;
        public byte b2;
        public byte b3;
        public byte b4;
        public int NID;
        public short Command;
        public short Pos;
        public short Dir;
        public short WID;
        public double Head;
        public short Cmd4;
        public short Zero1;
        public double Tail;
        public int Recog;
        public ushort Ident;
        public ushort Param;
        public ushort Tag;
        public ushort Series;

        public byte[] ToByte()
        {
            return null;
        }

        public TCmdPack()
        {

        }

        public TCmdPack(byte[] buffer)
        {

        }

        public byte[] GetPacket()
        {
            using var memoryStream = new MemoryStream();
            var backingStream = new BinaryWriter(memoryStream);
            backingStream.Write(Recog);

            backingStream.Write(UID);
            backingStream.Write(Cmd);
            backingStream.Write(X);
            backingStream.Write(Y);
            backingStream.Write(Direct);
            backingStream.Write(ID1);
            backingStream.Write(Cmd1);
            backingStream.Write(ID2);
            backingStream.Write(PosX);
            backingStream.Write(PosY);
            backingStream.Write(Cmd2);
            backingStream.Write(IDLo);
            backingStream.Write(Magic);
            backingStream.Write(IDHi);
            backingStream.Write(UID1);
            backingStream.Write(Cmd3);
            backingStream.Write(b1);
            backingStream.Write(b2);
            backingStream.Write(b3);
            backingStream.Write(b4);
            backingStream.Write(NID);
            backingStream.Write(Command);
            backingStream.Write(Pos);
            backingStream.Write(Dir);
            backingStream.Write(WID);
            backingStream.Write(Head);
            backingStream.Write(Cmd4);
            backingStream.Write(Zero1);
            backingStream.Write(Tail);
            backingStream.Write(Recog);
            backingStream.Write(Ident);
            backingStream.Write(Param);
            backingStream.Write(Tag);
            backingStream.Write(Series);

            var stream = backingStream.BaseStream as MemoryStream;
            return stream?.ToArray();
        }
    }


    public class TDefaultMessage : TCmdPack
    {
        public TDefaultMessage()
        {

        }

        public TDefaultMessage(byte[] buff)
        {
            var binaryReader = new BinaryReader(new MemoryStream(buff));
            Recog = binaryReader.ReadInt32();
            Ident = binaryReader.ReadUInt16();
            Param = binaryReader.ReadUInt16();
            Tag = binaryReader.ReadUInt16();
            Series = binaryReader.ReadUInt16();
        }

        public TDefaultMessage(byte[] buff, byte buffSize)
        {
            switch (buffSize)
            {
                case 12://TDefaultMessage
                    var binaryReader = new BinaryReader(new MemoryStream(buff, 0, buffSize));
                    Recog = binaryReader.ReadInt32();
                    Ident = binaryReader.ReadUInt16();
                    Param = binaryReader.ReadUInt16();
                    Tag = binaryReader.ReadUInt16();
                    Series = binaryReader.ReadUInt16();
                    break;
            }
        }
    }
}