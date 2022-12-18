using MemoryPack;

namespace SystemModule.Packets.ClientPackets
{
    /// <summary>
    /// 客户端消息
    /// </summary>
    public struct ClientCommandPacket
    {
        [MemoryPackInclude]
        public int Recog;
        [MemoryPackInclude]
        public ushort Ident;
        [MemoryPackInclude]
        public ushort Param;
        [MemoryPackInclude]
        public ushort Tag;
        [MemoryPackInclude]
        public ushort Series;

        public const int PackSize = 12;

        //protected override void ReadPacket(BinaryReader reader)
        //{
        //    Recog = reader.ReadInt32();
        //    Ident = reader.ReadUInt16();
        //    Param = reader.ReadUInt16();
        //    Tag = reader.ReadUInt16();
        //    Series = reader.ReadUInt16();

        //    Cmd = Ident;
        //    Cmd1 = Ident;
        //    Cmd2 = Ident;
        //    Cmd3 = Ident;
        //    Cmd4 = Ident;
        //    Command = Ident;

        //    UID = Recog;
        //    Head = Recog;
        //    NID = Recog;
        //    UID1 = Recog;
        //    PosX = (ushort)Recog;
        //    ID1 = Recog;

        //    X = Param;
        //    IDLo = Param;
        //    b1 = (byte)Param;
        //    Pos = Param;
        //    Zero1 = Param;

        //    Y = Tag;
        //    Dir = Tag;
        //    b3 = (byte)Tag;
        //    Magic = Tag;

        //    Direct = Series;
        //    WID = Series;
        //    IDHi = Series;
        //}

        //protected override void WritePacket(BinaryWriter writer)
        //{
        //    writer.Write(Recog);
        //    writer.Write(Ident);
        //    writer.Write(Param);
        //    writer.Write(Tag);
        //    writer.Write(Series);
        //}
    }
}