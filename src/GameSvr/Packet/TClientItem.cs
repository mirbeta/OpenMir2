using System.IO;
using SystemModule;

namespace GameSvr
{
    public class TClientItem
    {
        public TStdItem Item;
        public int MakeIndex;
        public ushort Dura;
        public ushort DuraMax;

        public TClientItem()
        {
            Item = new TStdItem();
        }

        public byte[] GetPacket()
        {
            using var memoryStream = new MemoryStream();
            var backingStream = new BinaryWriter(memoryStream);

            backingStream.Write(Item.GetPacket());
            backingStream.Write(MakeIndex);
            backingStream.Write(Dura);
            backingStream.Write(DuraMax);

            var stream = backingStream.BaseStream as MemoryStream;
            return stream.ToArray();
        }
    }
}

