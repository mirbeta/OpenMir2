using System.IO;
using SystemModule.Extensions;

namespace SystemModule.Packet.ServerPackets
{
    public class MagicInfo : Packets
    {
        /// <summary>
        /// 技能ID
        /// </summary>
        public ushort wMagicID;
        /// <summary>
        /// 技能名称
        /// </summary>
        public string sMagicName;
        /// <summary>
        /// 动作效果
        /// </summary>
        public byte btEffectType;
        /// <summary>
        /// 魔法效果
        /// </summary>
        public byte btEffect;
        /// <summary>
        /// 魔法消耗
        /// </summary>
        public ushort wSpell;
        /// <summary>
        /// 基本威力
        /// </summary>
        public ushort wPower;
        /// <summary>
        /// 技能等级
        /// </summary>
        public short[] TrainLevel;
        /// <summary>
        /// 技能等级最高修炼点
        /// </summary>
        public int[] MaxTrain;
        /// <summary>
        /// 修炼等级
        /// </summary>
        public byte btTrainLv;
        /// <summary>
        /// 职业 0-战 1-法 2-道
        /// </summary>
        public byte btJob;
        /// <summary>
        /// 技能使用延时
        /// </summary>
        public int dwDelayTime;
        /// <summary>
        /// 升级魔法
        /// </summary>
        public byte btDefSpell;
        /// <summary>
        /// 升级威力
        /// </summary>
        public byte btDefPower;
        /// <summary>
        /// 最大威力
        /// </summary>
        public ushort wMaxPower;
        /// <summary>
        /// 升级最大威力
        /// </summary>
        public byte btDefMaxPower;
        /// <summary>
        /// 备注说明
        /// </summary>
        public string sDescr;
        public byte btClass;

        public MagicInfo()
        {
            TrainLevel = new short[4];
            MaxTrain = new int[4];
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(wMagicID);
            writer.WriteAsciiString(sMagicName, 12);
            writer.Write(btEffectType);
            writer.Write(btEffect);
            writer.Write(wSpell);
            writer.Write(wPower);
            writer.Write(TrainLevel[0]);
            writer.Write(TrainLevel[1]);
            writer.Write(TrainLevel[2]);
            writer.Write(TrainLevel[3]);
            writer.Write(MaxTrain[0]);
            writer.Write(MaxTrain[1]);
            writer.Write(MaxTrain[2]);
            writer.Write(MaxTrain[3]);
            writer.Write(btTrainLv);
            writer.Write(btJob);
            writer.Write(dwDelayTime);
            writer.Write(btDefSpell);
            writer.Write(btDefPower);
            writer.Write(wMaxPower);
            writer.Write(btDefMaxPower);
            writer.WriteAsciiString(sDescr, 14);
            writer.Write(btClass);
        }
    }
}