using System;
using System.IO;

namespace SystemModule
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

        public TCmdPack()
        {

        }

        public TCmdPack(byte[] buffer)
        {
            var binaryReader = new BinaryReader(new MemoryStream(buffer, 0, PackSize));
            Recog = binaryReader.ReadInt32();
            Ident = binaryReader.ReadUInt16();
            Param = binaryReader.ReadUInt16();
            Tag = binaryReader.ReadUInt16();
            Series = binaryReader.ReadUInt16();
                    
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
            b1 =(byte) Param;
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

        public byte[] GetPacket(byte msgType = 6)
        {
            using var memoryStream = new MemoryStream();
            var backingStream = new BinaryWriter(memoryStream);
            switch (msgType)
            {
                case 0:
                    backingStream.Write(UID);
                    backingStream.Write(Cmd);
                    backingStream.Write(X);
                    backingStream.Write(Y);
                    backingStream.Write(Direct);
                    break;
                case 1:
                    backingStream.Write(ID1);
                    backingStream.Write(Cmd1);
                    backingStream.Write(ID2);
                    break;
                case 2:
                    backingStream.Write(PosX);
                    backingStream.Write(PosY);
                    backingStream.Write(Cmd2);
                    backingStream.Write(IDLo);
                    backingStream.Write(Magic);
                    backingStream.Write(IDHi);
                    break;
                case 3:
                    backingStream.Write(UID1);
                    backingStream.Write(Cmd3);
                    backingStream.Write(b1);
                    backingStream.Write(b2);
                    backingStream.Write(b3);
                    backingStream.Write(b4);
                    break;
                case 4:
                    backingStream.Write(NID);
                    backingStream.Write(Command);
                    backingStream.Write(Pos);
                    backingStream.Write(Dir);
                    backingStream.Write(WID);
                    break;
                case 5:
                    backingStream.Write(Head);
                    backingStream.Write(Cmd4);
                    backingStream.Write(Zero1);
                    backingStream.Write(Tail);
                    break;
                case 6:
                    backingStream.Write(Recog);
                    backingStream.Write(Ident);
                    backingStream.Write(Param);
                    backingStream.Write(Tag);
                    backingStream.Write(Series);
                    break;
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            var data = new byte[memoryStream.Length];
            memoryStream.Read(data, 0, data.Length);
            return data;
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