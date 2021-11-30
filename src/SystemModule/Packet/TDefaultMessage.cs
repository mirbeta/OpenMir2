using System;
using System.IO;

namespace SystemModule.Packages
{
    public class TCmdPack
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

        public const int PackSize = 12;
        
        public byte[] ToByte()
        {
            return null;
        }

        public TCmdPack()
        {

        }

        public TCmdPack(byte[] buffer,int decodeLen)
        {
            var binaryReader = new BinaryReader(new MemoryStream(buffer));
            switch (decodeLen)
            {
                case 8:
                    ID1 = binaryReader.ReadInt32();
                    Cmd1 = binaryReader.ReadUInt16();
                    ID2 = binaryReader.ReadInt32();
                    break;
                case 14:
                    UID = binaryReader.ReadInt32();
                    Cmd = binaryReader.ReadUInt16();
                    X = binaryReader.ReadUInt16();
                    Y = binaryReader.ReadUInt16();
                    Direct = binaryReader.ReadUInt16();
                    break;
                case 12:
                    Recog = binaryReader.ReadInt32();
                    Ident = binaryReader.ReadUInt16();
                    Param = binaryReader.ReadUInt16();
                    Tag = binaryReader.ReadUInt16();
                    Series = binaryReader.ReadUInt16();
                    Cmd = Ident;
                    break;
            }
        }

        public byte[] GetPacket(byte msgType)
        {
            using var memoryStream = new MemoryStream();
            var backingStream = new BinaryWriter(memoryStream);
            if (msgType == 0)
            {
                backingStream.Write(UID);
                backingStream.Write(Cmd);
                backingStream.Write(X);
                backingStream.Write(Y);
                backingStream.Write(Direct);
            }
            if (msgType == 1)
            {
                backingStream.Write(ID1);
                backingStream.Write(Cmd1);
                backingStream.Write(ID2);
            }
            if (msgType == 2)
            {
                backingStream.Write(PosX);
                backingStream.Write(PosY);
                backingStream.Write(Cmd2);
                backingStream.Write(IDLo);
                backingStream.Write(Magic);
                backingStream.Write(IDHi);
            }
            if (msgType == 3)
            {
                backingStream.Write(UID1);
                backingStream.Write(Cmd3);
                backingStream.Write(b1);
                backingStream.Write(b2);
                backingStream.Write(b3);
                backingStream.Write(b4);
            }
            if (msgType == 4)
            {
                backingStream.Write(NID);
                backingStream.Write(Command);
                backingStream.Write(Pos);
                backingStream.Write(Dir);
                backingStream.Write(WID);
            }
            if (msgType == 5)
            {
                backingStream.Write(Head);
                backingStream.Write(Cmd4);
                backingStream.Write(Zero1);
                backingStream.Write(Tail);
            }
            if (msgType == 6)
            {
                backingStream.Write(Recog);
                backingStream.Write(Ident);
                backingStream.Write(Param);
                backingStream.Write(Tag);
                backingStream.Write(Series);
            }
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
                default:
                    Console.WriteLine(buffSize);
                    break;
            }
        }
    }
}