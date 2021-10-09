using System.IO;

namespace GameSvr
{
    public class TOMagic
    {
        public ushort wMagicID;
        public byte btEffectType;
        public byte btEffect;
        public ushort wSpell;
        public ushort wPower;
        public byte btTrainLv;
        public byte btJob;
        public int dwDelayTime;
        public byte btDefSpell;
        public byte btDefPower;
        public ushort wMaxPower;
        public byte btDefMaxPower;

        public TOMagic()
        {
        }

        public byte[] GetPacket()
        {
            using var memoryStream = new MemoryStream();
            var backingStream = new BinaryWriter(memoryStream);
            backingStream.Write(wMagicID);
            backingStream.Write(btEffectType);
            backingStream.Write(btEffect);
            backingStream.Write(wSpell);
            backingStream.Write(wPower);
            backingStream.Write(btTrainLv);
            backingStream.Write(btJob);
            backingStream.Write(dwDelayTime);
            backingStream.Write(btDefSpell);
            backingStream.Write(btDefPower);
            backingStream.Write(wMaxPower);
            backingStream.Write(btDefMaxPower);

            var stream = backingStream.BaseStream as MemoryStream;
            return stream.ToArray();
        }

    }
}

