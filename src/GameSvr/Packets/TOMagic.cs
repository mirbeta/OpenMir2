using System.IO;
using SystemModule;

namespace GameSvr
{
    public class TOMagic : Packets
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

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(wMagicID);
            writer.Write(btEffectType);
            writer.Write(btEffect);
            writer.Write(wSpell);
            writer.Write(wPower);
            writer.Write(btTrainLv);
            writer.Write(btJob);
            writer.Write(dwDelayTime);
            writer.Write(btDefSpell);
            writer.Write(btDefPower);
            writer.Write(wMaxPower);
            writer.Write(btDefMaxPower);
        }
    }
}