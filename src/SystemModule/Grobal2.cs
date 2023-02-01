using SystemModule.Packets.ClientPackets;

namespace SystemModule
{
    public class Grobal2
    {
        public const int CLIENT_VERSION_NUMBER = 120040918;
        public const uint RUNGATECODE = 0xAA55AA55 + 0x00450045;
        /// <summary>
        /// 最大魔法技能数
        /// </summary>
        public const byte MaxMagicCount = 54;
        public const string sSTRING_GOLDNAME = "金币";
        /// <summary>
        /// 最高等级
        /// </summary>
        public const byte MaxLevel = byte.MaxValue;
        /// <summary>
        /// 最高经验等级
        /// </summary>
        public const ushort MaxChangeLevel = 1000;
        /// <summary>
        /// 属下最高等级
        /// </summary>
        public const byte SlaveMaxLevel = 50;
        /// <summary>
        /// 记录游戏金币日志
        /// </summary>
        public const byte LogGameGold = 1;
        /// <summary>
        /// 记录声望日志
        /// </summary>
        public const byte LogGamePoint = 2;
        /// <summary>
        /// 组队最大人数
        /// </summary>
        public const byte GroupMax = 11;
        /// <summary>
        /// 物品类型(物品属性读取)
        /// </summary>
        public const byte MAX_STATUS_ATTRIBUTE = 12;
        
        public const byte ET_DIGOUTZOMBI = 1;
        public const byte ET_MINE = 2;
        public const byte ET_PILESTONES = 3;
        public const byte ET_HOLYCURTAIN = 4;
        /// <summary>
        /// 火墙事件
        /// </summary>
        public const byte ET_FIRE = 5;
        public const byte ET_SCULPEICE = 6;

        public const byte GM_OPEN = 1;
        public const byte GM_CLOSE = 2;
        public const byte GM_CHECKSERVER = 3;
        public const byte GM_CHECKCLIENT = 4;
        public const byte GM_DATA = 5;
        public const byte GM_SERVERUSERINDEX = 6;
        public const byte GM_RECEIVE_OK = 7;
        
        public const byte DR_UP = 0;
        public const byte DR_UPRIGHT = 1;
        public const byte DR_RIGHT = 2;
        public const byte DR_DOWNRIGHT = 3;
        public const byte DR_DOWN = 4;
        public const byte DR_DOWNLEFT = 5;
        public const byte DR_LEFT = 6;
        public const byte DR_UPLEFT = 7;

        /// <summary>
        /// 衣服
        /// </summary>
        public const byte U_DRESS = 0;
        /// <summary>
        /// 武器
        /// </summary>
        public const byte U_WEAPON = 1;
        /// <summary>
        /// 右手
        /// </summary>
        public const byte U_RIGHTHAND = 2;
        /// <summary>
        /// 项链
        /// </summary>
        public const byte U_NECKLACE = 3;
        /// <summary>
        /// 头盔
        /// </summary>
        public const byte U_HELMET = 4;
        /// <summary>
        /// 左手手镯,符
        /// </summary>
        public const byte U_ARMRINGL = 5;
        /// <summary>
        /// 右手手镯
        /// </summary>
        public const byte U_ARMRINGR = 6;
        /// <summary>
        /// 左戒指
        /// </summary>
        public const byte U_RINGL = 7;
        /// <summary>
        /// 右戒指
        /// </summary>
        public const byte U_RINGR = 8;
        /// <summary>
        /// 物品
        /// </summary>
        public const byte U_BUJUK = 9;
        /// <summary>
        /// 腰带
        /// </summary>
        public const byte U_BELT = 10;
        /// <summary>
        /// 鞋
        /// </summary>
        public const byte U_BOOTS = 11;
        /// <summary>
        /// 宝石
        /// </summary>
        public const byte U_CHARM = 12;

        public const byte DEFBLOCKSIZE = 16;
        /// <summary>
        /// 最大包裹数
        /// </summary>
        public const byte MaxBagItem = 46;
        public const byte LA_UNDEAD = 1;

        public static CommandPacket MakeDefaultMsg(int msg, int Recog, int param, int tag, int series)
        {
            var result = new CommandPacket
            {
                Ident = (ushort)msg,
                Param = (ushort)param,
                Tag = (ushort)tag,
                Series = (ushort)series,
                Recog = Recog
            };
            return result;
        }

        public static int MakeMonsterFeature(byte btRaceImg, byte btWeapon, ushort wAppr)
        {
            return HUtil32.MakeLong(HUtil32.MakeWord(btRaceImg, btWeapon), wAppr);
        }

        public static int MakeHumanFeature(byte btRaceImg, byte btDress, byte btWeapon, byte btHair)
        {
            return HUtil32.MakeLong(HUtil32.MakeWord(btRaceImg, btWeapon), HUtil32.MakeWord(btHair, btDress));
        }
    }
}