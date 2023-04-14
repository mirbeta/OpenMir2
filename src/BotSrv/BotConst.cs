using System.Collections.Generic;

namespace BotSrv
{
    public static class BotConst
    {
        public const short ScreenWidth = 800;
        public const short ScreenHeight = 600;
        public const short TileMapOffSetX = 9;
        public const short TileMapOffSetY = 9;
        public const byte MagicRange = 8;
        public const int UNITX = 48;
        public const int UNITY = 32;
        public const int MAXX = 30;
        public const int MAXY = 40;
        public const int CLIENT_VERSION_NUMBER = 120020522;
        public const int RMCLIENT = 46;
        public const int CLIENTTYPE = RMCLIENT;
        public const int LOGICALMAPUNIT = 30;
        public const int MaxBagItemcl = 52;
        public const int HERO_MIIDX_OFFSET = 5000;
        public const int SAVE_MIIDX_OFFSET = HERO_MIIDX_OFFSET + 500;
        public const int STALL_MIIDX_OFFSET = HERO_MIIDX_OFFSET + 500 + 50;
        public const int DETECT_MIIDX_OFFSET = HERO_MIIDX_OFFSET + 500 + 50 + 10 + 1;
        public const int MSGMUCH = 2;
        public const int g_gnTecPracticeKey = 0;
        public const char Activebuf = '*';
        public const string MAPDIRNAME = "Map/";
        public const string GoldName = "金币";
        public const string GameGoldName = "元宝";
        public const string GamePointName = "泡点";
        public const string WarriorName = "武士";
        public const string WizardName = "魔法师";
        public const string TaoistName = "道士";
        public const string UnKnowName = "未知";
        public const string sAttackModeOfAll = "[全体攻击模式]";
        public const string sAttackModeOfPeaceful = "[和平攻击模式]";
        public const string sAttackModeOfDear = "[夫妻攻击模式]";
        public const string sAttackModeOfMaster = "[师徒攻击模式]";
        public const string sAttackModeOfGroup = "[编组攻击模式]";
        public const string sAttackModeOfGuild = "[行会攻击模式]";
        public const string sAttackModeOfRedWhite = "[善恶攻击模式]";
        public static Dictionary<string, string> g_ShowItemList = new Dictionary<string, string>();
        public static string[] g_sRenewBooks = new string[] { "随机传送卷", "地牢逃脱卷", "回城卷", "行会回城卷", "盟重传送石", "比奇传送石", "随机传送石", };
        public static string[] g_UnBindItems = { "万年雪霜", "疗伤药", "强效太阳水", "强效金创药", "强效魔法药", "金创药(小量)", "魔法药(小量)", "金创药(中量)", "魔法药(中量)", "地牢逃脱卷", "随机传送卷", "回城卷", "行会回城卷" };
        public static string[] g_HintTec = { "钩选此项将开启刀刀刺杀", "钩选此项将开启智能半月", "钩选此项将自动凝聚烈火剑法", "钩选此项将自动凝聚逐日剑法", "钩选此项将自动开启魔法盾", "钩选此项英雄将自动开启魔法盾", "钩选此项道士将自动使用隐身术", "", "", "钩选此项将自动凝聚雷霆剑法", "钩选此项将自动进行隔位刺杀", "钩选此项将自动凝聚断空斩", "钩选此项英雄将不使用连击打怪\\方便玩家之间进行PK", "钩选此项将自动凝聚开天斩", "钩选此项：施展魔法超过允许距离时，会自动跑近目标并释放魔法" };
        public static string[] g_caTec = { "刀刀刺杀", "智能半月", "自动烈火", "逐日剑法", "自动开盾", "持续开盾(英雄)", "自动隐身", "时间间隔", "", "自动雷霆", "隔位刺杀", "自动断空斩", "英雄连击不打怪", "自动开天斩", "自动调节魔法距离" };
        public static string[] g_sMagics = { "火球术", "治愈术", "大火球", "施毒术", "攻杀剑术", "抗拒火环", "地狱火", "疾光电影", "雷电术", "雷电术", "雷电术", "雷电术", "雷电术", "开天斩", "开天斩" };
        public static Dictionary<string, string> g_ItemsFilter_All = null;
        public static Dictionary<string, string> g_ItemsFilter_All_Def = null;
    }
}