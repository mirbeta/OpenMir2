using System.IO;

namespace M2Server
{
    public class TOMagic
    {
        public short wMagicID;
        public byte btEffectType;
        public byte btEffect;
        public short wSpell;
        public short wPower;
        public byte btTrainLv;
        public byte btJob;
        public int dwDelayTime;
        public byte btDefSpell;
        public byte btDefPower;
        public short wMaxPower;
        public byte btDefMaxPower;

        public TOMagic()
        {
        }

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
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
}

