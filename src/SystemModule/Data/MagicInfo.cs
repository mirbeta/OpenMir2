using System.IO;
using SystemModule.Extensions;

namespace SystemModule.Data
{
    public class MagicInfo : Packets.Packets
    {
        /// <summary>
        /// 技能ID
        /// </summary>
        public ushort MagicId;
        /// <summary>
        /// 技能名称
        /// </summary>
        public string MagicName;
        /// <summary>
        /// 动作效果
        /// </summary>
        public byte EffectType;
        /// <summary>
        /// 魔法效果
        /// </summary>
        public byte Effect;
        /// <summary>
        /// 魔法消耗
        /// </summary>
        public ushort Spell;
        /// <summary>
        /// 基本威力
        /// </summary>
        public ushort Power;
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
        public byte TrainLv;
        /// <summary>
        /// 职业 0-战 1-法 2-道
        /// </summary>
        public byte Job;
        /// <summary>
        /// 技能使用延时
        /// </summary>
        public int DelayTime;
        /// <summary>
        /// 升级魔法
        /// </summary>
        public byte DefSpell;
        /// <summary>
        /// 升级威力
        /// </summary>
        public byte DefPower;
        /// <summary>
        /// 最大威力
        /// </summary>
        public ushort MaxPower;
        /// <summary>
        /// 升级最大威力
        /// </summary>
        public byte DefMaxPower;
        /// <summary>
        /// 备注说明
        /// </summary>
        public string Desc;

        public MagicInfo()
        {
            TrainLevel = new byte[4];
            MaxTrain = new int[4];
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(MagicId);
            writer.WriteAsciiString(MagicName, 14);
            writer.Write(EffectType);
            writer.Write(Effect);
            writer.Write((byte)0);
            writer.Write(Spell);
            writer.Write(Power);
            writer.Write(TrainLevel[0]);
            writer.Write(TrainLevel[1]);
            writer.Write(TrainLevel[2]);
            writer.Write(TrainLevel[3]);
            writer.Write(MaxTrain[0]);
            writer.Write(MaxTrain[1]);
            writer.Write(MaxTrain[2]);
            writer.Write(MaxTrain[3]);
            writer.Write(TrainLv);
            writer.Write(Job);
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write(DelayTime);
            writer.Write(DefSpell);
            writer.Write(DefPower);
            writer.Write(MaxPower);
            writer.Write(DefMaxPower);
            if (string.IsNullOrEmpty(Desc))
            {
                writer.WriteAsciiString("", 15);
            }
            else
            {
                writer.WriteAsciiString(Desc, 15);
            }
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write((byte)0);
        }
    }
}