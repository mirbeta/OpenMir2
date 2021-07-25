using System.IO;
using SystemModule;

namespace M2Server
{
    public class TMagic
    {
        public ushort wMagicID;
        public string sMagicName;
        public byte btEffectType;
        public byte btEffect;
        public ushort wSpell;
        public ushort wPower;
        public byte[] TrainLevel;
        public int[] MaxTrain;
        public byte btTrainLv;
        public byte btJob;
        public int dwDelayTime;
        public byte btDefSpell;
        public byte btDefPower;
        public ushort wMaxPower;
        public byte btDefMaxPower;
        public string sDescr;

        public TMagic()
        {
            TrainLevel = new byte[4];
            MaxTrain = new int[4];
        }

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);
                backingStream.Write(wMagicID);
                backingStream.Write(sMagicName.ToByte(13));
                backingStream.Write(btEffectType);
                backingStream.Write(btEffect);
                backingStream.Write((byte)0);
                backingStream.Write(wSpell);
                backingStream.Write(wPower);
                backingStream.Write(TrainLevel);
                backingStream.Write((byte)0);
                backingStream.Write((byte)0);
                backingStream.Write(MaxTrain[0]);
                backingStream.Write(MaxTrain[1]);
                backingStream.Write(MaxTrain[2]);
                backingStream.Write(MaxTrain[3]);
                backingStream.Write(btTrainLv);
                backingStream.Write(btJob);
                backingStream.Write((byte)0);
                backingStream.Write((byte)0);
                backingStream.Write(dwDelayTime);
                backingStream.Write(btDefSpell);
                backingStream.Write(btDefPower);
                backingStream.Write(wMaxPower);
                backingStream.Write(btDefMaxPower);
                backingStream.Write(sDescr.ToByte(19));

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }

    }
}

