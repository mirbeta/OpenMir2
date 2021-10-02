using System.IO;

namespace SystemModule
{
    public class TMagic
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
        public byte[] TrainLevel;
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

        public TMagic()
        {
            TrainLevel = new byte[4];
            MaxTrain = new int[4];
        }

        public byte[] GetPacket()
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

