using GameSvr.Actor;
using GameSvr.Castle;
using GameSvr.Command;
using GameSvr.Conf;
using GameSvr.Conf.Model;
using GameSvr.DataStores;
using GameSvr.Event;
using GameSvr.GateWay;
using GameSvr.Guild;
using GameSvr.Items;
using GameSvr.Magic;
using GameSvr.Maps;
using GameSvr.Notices;
using GameSvr.Npc;
using GameSvr.Robots;
using GameSvr.Script;
using GameSvr.Services;
using GameSvr.World;
using System.Collections;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using SystemModule;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr
{
    public enum PlayJob : byte
    {
        /// <summary>
        /// 战士
        /// </summary>
        Warrior = 0,
        /// <summary>
        /// 法师
        /// </summary>
        Wizard = 1,
        /// <summary>
        /// 道士
        /// </summary>
        Taoist = 2,
        /// <summary>
        /// 未知
        /// </summary>
        None = 3
    }

    public enum AttackMode : byte
    {
        /// <summary>
        /// 全体攻击模式
        /// </summary>
        HAM_ALL = 0,
        /// <summary>
        /// 和平攻击模式
        /// </summary>
        HAM_PEACE = 1,
        /// <summary>
        /// 夫妻攻击模式
        /// </summary>
        HAM_DEAR = 2,
        /// <summary>
        /// 师徒攻击模式
        /// </summary>
        HAM_MASTER = 3,
        /// <summary>
        /// 组队攻击模式
        /// </summary>
        HAM_GROUP = 4,
        /// <summary>
        /// 行会攻击模式
        /// </summary>
        HAM_GUILD = 5,
        /// <summary>
        /// 红名攻击模式
        /// </summary>
        HAM_PKATTACK = 6
    }

    public struct TItemBind
    {
        public int nMakeIdex;
        public int nItemIdx;
        public string sBindName;
    }

    public static class M2Share
    {
        /// <summary>
        /// 服务器编号
        /// </summary>
        public static byte ServerIndex = 0;
        /// <summary>
        /// 服务器启动时间
        /// </summary>
        public static int StartTick = 0;
        public static int ShareFileNameNum = 0;
        public static int ServerTickDifference = 0;
        public static readonly ReaderWriterLockWrapper SyncLock = new ReaderWriterLockWrapper();
        public static ActorMgr ActorMgr = null;
        public static ServerConf ServerConf;
        private static StringConf StringConf;
        private static ExpsConf ExpConf;
        private static GlobalConf GlobalConf;
        /// <summary>
        /// 寻路
        /// </summary>
        public static FindPath FindPath;
        /// <summary>
        /// 游戏命令
        /// </summary>
        public static CommandManager CommandMgr = null;
        /// <summary>
        /// 地图对象管理
        /// </summary>
        public static CellObjectMgr CellObjectSystem;
        public static LocalDB LocalDb;
        public static CommonDB CommonDb;
        public static readonly MirLog Log;
        public static readonly RandomNumber RandomNumber;
        public static DBService DataServer = null;
        public static ScriptSystem ScriptSystem = null;
        public static GameGateMgr GateMgr = null;
        public static ConcurrentQueue<string> ItemLogQueue = null;
        public static MapManager MapMgr = null;
        public static CustomItem ItemUnit = null;
        public static MagicManager MagicMgr = null;
        public static NoticeManager NoticeMgr = null;
        public static GuildManager GuildMgr = null;
        public static EventManager EventMgr = null;
        public static CastleManager CastleMgr = null;
        public static TFrontEngine FrontEngine = null;
        public static WorldServer WorldEngine = null;
        public static RobotManage RobotMgr = null;
        public static Dictionary<string, IList<MakeItem>> MakeItemList = null;
        public static IList<StartPoint> StartPointList = null;
        public static StartPoint g_RedStartPoint = null;
        public static TRouteInfo[] ServerTableList = null;
        public static ConcurrentDictionary<string, long> DenySayMsgList = null;
        public static ConcurrentDictionary<string, int> MiniMapList = null;
        public static Dictionary<int, string> g_UnbindList = null;
        public static IList<DealOffInfo> sSellOffItemList = null;
        public static ArrayList LogonCostLogList = null;
        /// <summary>
        /// 游戏公告列表
        /// </summary>
        public static IList<string> LineNoticeList = null;
        public static IList<IList<TQDDinfo>> QuestDiaryList = null;
        public static StringList AbuseTextList = null;
        /// <summary>
        /// 怪物说话信息列表
        /// </summary>
        public static Dictionary<string, IList<TMonSayMsg>> g_MonSayMsgList = null;
        /// <summary>
        /// /禁止制造物品列表
        /// </summary>
        public static IList<string> g_DisableMakeItemList = null;
        /// <summary>
        /// 禁止制造物品列表
        /// </summary>
        public static IList<string> g_EnableMakeItemList = null;
        /// <summary>
        /// 禁止出售物品列表
        /// </summary>
        public static IList<string> g_DisableSellOffList = null;
        /// <summary>
        /// 禁止移动地图列表
        /// </summary>
        public static IList<string> g_DisableMoveMapList = null;
        /// <summary>
        /// 禁止发信息名称列表
        /// </summary>
        public static IList<string> g_DisableSendMsgList = null;
        /// <summary>
        /// 怪物爆物品限制
        /// </summary>
        public static ConcurrentDictionary<string, TMonDrop> g_MonDropLimitLIst = null;
        /// <summary>
        /// 禁止取下物品列表
        /// </summary>
        public static IList<string> g_DisableTakeOffList = null;
        public static IList<TItemBind> g_ItemBindIPaddr = null;
        public static IList<TItemBind> g_ItemBindAccount = null;
        public static IList<TItemBind> g_ItemBindCharName = null;
        /// <summary>
        /// 出师记录表
        /// </summary>
        public static IList<string> g_UnMasterList = null;
        /// <summary>
        /// 强行出师记录表
        /// </summary>
        public static IList<string> g_UnForceMasterList = null;
        /// <summary>
        /// 游戏日志物品名
        /// </summary>
        public static IList<string> g_GameLogItemNameList = null;
        public static bool g_boGameLogGold = false;
        public static bool g_boGameLogGameGold = false;
        public static bool g_boGameLogGamePoint = false;
        public static bool g_boGameLogHumanDie = false;
        public static IList<string> g_DenyIPAddrList = null;
        // IP过滤列表
        public static IList<string> g_DenyChrNameList = null;
        // 角色过滤列表
        public static IList<string> g_DenyAccountList = null;
        // 登录帐号过滤列表
        public static IList<string> g_NoClearMonLIst = null;
        // 不清除怪物列表
        public static IList<string> g_NoHptoexpMonLIst = null;
        // 不清除怪物列表
        public static object ProcessMsgCriticalSection = null;
        public static object UserDBSection = null;
        public static object ProcessHumanCriticalSection = null;
        public static int g_nTotalHumCount = 0;
        public static bool g_boMission = false;
        public static string g_sMissionMap = string.Empty;
        public static short g_nMissionX = 0;
        public static short g_nMissionY = 0;
        public static bool StartReady = false;
        public static bool boFilterWord = false;
        public static int g_nBaseObjTimeMin = 0;
        public static int g_nBaseObjTimeMax = 0;
        public static int g_nSockCountMin = 0;
        public static int g_nSockCountMax = 0;
        public static int g_nHumCountMin = 0;
        public static int g_nHumCountMax = 0;
        public static int dwUsrRotCountMin = 0;
        public static int dwUsrRotCountMax = 0;
        public static int g_dwUsrRotCountTick = 0;
        public static int g_nProcessHumanLoopTime = 0;
        public static int HumLimit = 30;
        public static int MonLimit = 30;
        public static int ZenLimit = 5;
        public static int NpcLimit = 5;
        public static int SocLimit = 10;
        public static int g_dwSocCheckTimeOut = 50;
        public static int nDecLimit = 20;
        public static string BasePath;
        public static int dwRunDBTimeMax = 0;
        public static int g_nGameTime = 0;
        public static NormNpc g_ManageNPC = null;
        public static NormNpc g_RobotNPC = null;
        public static Merchant g_FunctionNPC = null;
        public static Dictionary<string, TDynamicVar> g_DynamicVarList = null;
        public static char g_GMRedMsgCmd = '!';
        public static int g_nGMREDMSGCMD = 6;
        public static int g_dwSendOnlineTick = 0;
        public static object g_HighLevelHuman = null;
        public static object g_HighPKPointHuman = null;
        public static object g_HighDCHuman = null;
        public static object g_HighMCHuman = null;
        public static object g_HighSCHuman = null;
        public static object g_HighOnlineHuman = null;
        public static int g_dwSpiritMutinyTick = 0;
        public static GameSvrConf Config;
        public static int[] g_dwOldNeedExps = new int[Grobal2.MaxChangeLevel];
        public static IList<GameCmd> CustomCommands = new List<GameCmd>();
        public static GameCommand GameCommand = new GameCommand();
        public static string sClientSoftVersionError = "游戏版本错误!!!";
        public static string sDownLoadNewClientSoft = "请到网站上下载最新版本游戏客户端软件。";
        public static string sForceDisConnect = "连接被强行中断!!!";
        public static string sClientSoftVersionTooOld = "您现在使用的客户端软件版本太老了，大量的游戏效果新将无法使用。";
        public static string sDownLoadAndUseNewClient = "为了更好的进行游戏，请下载最新的客户端软件!!!";
        public static string sOnlineUserFull = "可允许的玩家数量已满";
        public static string sYouNowIsTryPlayMode = "你现在处于测试中，你可以在七级以前使用，但是会限制你的一些功能.";
        public static string g_sNowIsFreePlayMode = "当前服务器运行于测试模式.";
        public static string sAttackModeOfAll = "[攻击模式: 全体攻击]";
        public static string sAttackModeOfPeaceful = "[攻击模式: 和平攻击]";
        public static string sAttackModeOfDear = "[攻击模式: 夫妻攻击]";
        public static string sAttackModeOfMaster = "[攻击模式: 师徒攻击]";
        public static string sAttackModeOfGroup = "[攻击模式: 编组攻击]";
        public static string sAttackModeOfGuild = "[攻击模式: 行会攻击]";
        public static string sAttackModeOfRedWhite = "[攻击模式: 红名攻击]";
        public static string sStartChangeAttackModeHelp = "使用组合快捷键 CTRL-H 更改攻击模式...";
        public static string sStartNoticeMsg = "欢迎进入本服务器进行游戏...";
        public static string sThrustingOn = "启用刺杀剑法";
        public static string sThrustingOff = "关闭刺杀剑法";
        public static string sHalfMoonOn = "开启半月弯刀";
        public static string sHalfMoonOff = "关闭半月弯刀";
        public static string sCrsHitOn = "开启光风斩";
        public static string sCrsHitOff = "关闭光风斩";
        public static string sRedHalfMoonOn = "开启破空剑";
        public static string sRedHalfMoonOff = "关闭破空剑";
        public static string sTwinHitOn = "开启龙影剑法";
        public static string sTwinHitOff = "关闭龙影剑法";
        public static string sFireSpiritsSummoned = "召唤烈火精灵成功...";
        public static string sFireSpiritsFail = "召唤烈火精灵失败";
        public static string sSpiritsGone = "召唤烈火结束!!!";
        public static string sMateDoTooweak = "冲撞力不够!!!";
        public static string g_sTheWeaponBroke = "武器破碎!!!";
        public static string sTheWeaponRefineSuccessfull = "升级成功!!!";
        public static string sYouPoisoned = "中毒了!!!";
        public static string sPetRest = "下属：休息";
        public static string sPetAttack = "下属：攻击";
        public static string sWearNotOfWoMan = "非女性用品!!!";
        public static string sWearNotOfMan = "非男性用品!!!";
        public static string sHandWeightNot = "腕力不够!!!";
        public static string sWearWeightNot = "负重力不够!!!";
        public static string g_sItemIsNotThisAccount = "此物品不为此帐号所有!!!";
        public static string g_sItemIsNotThisIPaddr = "此物品不为此IP所有!!!";
        public static string g_sItemIsNotThisCharName = "此物品不为你所有!!!";
        public static string g_sLevelNot = "等级不够!!!";
        public static string g_sJobOrLevelNot = "职业不对或等级不够!!!";
        public static string g_sJobOrDCNot = "职业不对或攻击力不够!!!";
        public static string g_sJobOrMCNot = "职业不对或魔法力不够!!!";
        public static string g_sJobOrSCNot = "职业不对或道术不够!!!";
        public static string g_sDCNot = "攻击力不够!!!";
        public static string g_sMCNot = "魔法力不够!!!";
        public static string g_sSCNot = "道术不够!!!";
        public static string g_sCreditPointNot = "声望点不够!!!";
        public static string g_sReNewLevelNot = "转生等级不够!!!";
        public static string g_sGuildNot = "加入了行会才可以使用此物品!!!";
        public static string g_sGuildMasterNot = "行会掌门才可以使用此物品!!!";
        public static string g_sSabukHumanNot = "沙城成员才可以使用此物品!!!";
        public static string g_sSabukMasterManNot = "沙城城主才可以使用此物品!!!";
        public static string g_sMemberNot = "会员才可以使用此物品!!!";
        public static string g_sMemberTypeNot = "指定类型的会员可以使用此物品!!!";
        public static string g_sCanottWearIt = "此物品不适使用!!!";
        public static string sCanotUseDrugOnThisMap = "此地图不允许使用任何药品!!!";
        public static string sGameMasterMode = "已进入管理员模式";
        public static string sReleaseGameMasterMode = "已退出管理员模式";
        public static string sObserverMode = "已进入隐身模式";
        public static string g_sReleaseObserverMode = "已退出隐身模式";
        public static string sSupermanMode = "已进入无敌模式";
        public static string sReleaseSupermanMode = "已退出无敌模式";
        public static string sYouFoundNothing = "未获取任何物品!!!";
        public static string g_sNoPasswordLockSystemMsg = "游戏密码保护系统还没有启用!!!";
        public static string g_sAlreadySetPasswordMsg = "仓库早已设置了一个密码，如需要修改密码请使用修改密码命令!!!";
        public static string g_sReSetPasswordMsg = "请重复输入一次仓库密码：";
        public static string g_sPasswordOverLongMsg = "输入的密码长度不正确!!!，密码长度必须在 4 - 7 的范围内，请重新设置密码。";
        public static string g_sReSetPasswordOKMsg = "密码设置成功!!，仓库已经自动上锁，请记好您的仓库密码，在取仓库时需要使用此密码开锁。";
        public static string g_sReSetPasswordNotMatchMsg = "二次输入的密码不一致，请重新设置密码!!!";
        public static string g_sPleaseInputUnLockPasswordMsg = "请输入仓库密码：";
        public static string g_sStorageUnLockOKMsg = "密码输入成功!!!，仓库已经开锁。";
        public static string g_sPasswordUnLockOKMsg = "密码输入成功!!!，密码系统已经开锁。";
        public static string g_sStorageAlreadyUnLockMsg = "仓库早已解锁!!!";
        public static string g_sStorageNoPasswordMsg = "仓库还没设置密码!!!";
        public static string g_sUnLockPasswordFailMsg = "密码输入错误!!!，请检查好再输入。";
        public static string g_sLockStorageSuccessMsg = "仓库加锁成功。";
        public static string g_sStoragePasswordClearMsg = "仓库密码已清除!!!";
        public static string g_sPleaseUnloadStoragePasswordMsg = "请先解锁密码再使用此命令清除密码!!!";
        public static string g_sStorageAlreadyLockMsg = "仓库早已加锁了!!!";
        public static string g_sStoragePasswordLockedMsg = "由于密码输入错误超过三次，仓库密码已被锁定!!!";
        public static string g_sSetPasswordMsg = "请输入一个长度为 4 - 7 位的仓库密码: ";
        public static string g_sPleaseInputOldPasswordMsg = "请输入原仓库密码: ";
        public static string g_sOldPasswordIsClearMsg = "密码已清除。";
        public static string g_sPleaseUnLockPasswordMsg = "请先解锁仓库密码后再用此命令清除密码!!!";
        public static string g_sNoPasswordSetMsg = "仓库还没设置密码，请用设置密码命令设置仓库密码!!!";
        public static string g_sOldPasswordIncorrectMsg = "输入的原仓库密码不正确!!!";
        public static string g_sStorageIsLockedMsg = "仓库已被加锁，请先输入仓库正确的开锁密码，再取物品!!!";
        public static string g_sActionIsLockedMsg = "你当前已启用密码保护系统，请先输入正确的密码，才可以正常游戏!!!";
        public static string g_sPasswordNotSetMsg = "对不起，没有设置仓库密码此功能无法使用，设置仓库密码请输入指令 @{0}";
        public static string g_sNotPasswordProtectMode = "你正处于非保护模式，如想你的装备更加安全，请输入指令 @{0}";
        public static string g_sCanotDropGoldMsg = "太少的金币不允许扔在地上!!!";
        public static string g_sCanotDropInSafeZoneMsg = "安全区不允许扔东西在地上!!!";
        public static string g_sCanotDropItemMsg = "当前无法进行此操作!!!";
        public static string g_sCanotUseItemMsg = "当前无法进行此操作!!!";
        public static string g_sCanotTryDealMsg = "当前无法进行此操作!!!";
        public static string g_sPleaseTryDealLaterMsg = "请稍候再交易!!!";
        public static string g_sDealItemsDenyGetBackMsg = "交易的金币或物品不可以取回，要取回请取消再重新交易!!!";
        public static string g_sDisableDealItemsMsg = "交易功能暂时关闭!!!";
        public static string g_sDealActionCancelMsg = "交易取消!!!";
        public static string g_sPoseDisableDealMsg = "对方禁止进入交易";
        public static string g_sDealSuccessMsg = "交易成功...";
        public static string g_sDealOKTooFast = "过早按了成交按钮。";
        public static string g_sYourBagSizeTooSmall = "你的背包空间不够，无法装下对方交易给你的物品!!!";
        public static string g_sDealHumanBagSizeTooSmall = "交易对方的背包空间不够，无法装下对方交易给你的物品!!!";
        public static string g_sYourGoldLargeThenLimit = "你的所带的金币太多，无法装下对方交易给你的金币!!!";
        public static string g_sDealHumanGoldLargeThenLimit = "交易对方的所带的金币太多，无法装下对方交易给你的金币!!!";
        public static string g_sYouDealOKMsg = "你已经确认交易了。";
        public static string g_sPoseDealOKMsg = "对方已经确认交易了。";
        public static string g_sKickClientUserMsg = "请不要使用非法外挂软件!!!";
        public static string g_sStartMarryManMsg = "[{0}]: {1} 与 {2} 的婚礼现在开始...";
        public static string g_sStartMarryWoManMsg = "[{0}]: {1} 与 {2} 的婚礼现在开始...";
        public static string g_sStartMarryManAskQuestionMsg = "[{0}]: {1} 你愿意娶 {2} 小姐为妻，并照顾她一生一世吗？";
        public static string g_sStartMarryWoManAskQuestionMsg = "[{0}]: {1} 你愿意娶 {2} 小姐为妻，并照顾她一生一世吗？";
        public static string g_sMarryManAnswerQuestionMsg = "[{0}]: 我愿意!!!，{1} 小姐我会尽我一生的时间来照顾您，让您过上快乐美满的日子的。";
        public static string g_sMarryManAskQuestionMsg = "[{0}]: {1} 你愿意嫁给 {2} 先生为妻，并照顾他一生一世吗？";
        public static string g_sMarryWoManAnswerQuestionMsg = "[{0}]: 我愿意!!!，{2} 先生我愿意让你来照顾我，保护我。";
        public static string g_sMarryWoManGetMarryMsg = "[{0}]: 我宣布 {1} 先生与 {2} 小姐正式成为合法夫妻。";
        public static string g_sMarryWoManDenyMsg = "[{0}]: {1} 你这个好色之徒，谁会愿意嫁给你呀!!!，癞蛤蟆想吃天鹅肉。";
        public static string g_sMarryWoManCancelMsg = "[{0}]: 真是可惜，二个人这个时候才翻脸，你们培养好感情后再来找我吧!!!";
        public static string g_sfUnMarryManLoginMsg = "你的老婆{0}已经强行与你脱离了夫妻关系了!!!";
        public static string g_sfUnMarryWoManLoginMsg = "你的老公{0}已经强行与你脱离了夫妻关系了!!!";
        public static string g_sManLoginDearOnlineSelfMsg = "你的老婆{0}当前位于{1}({2}:{3})。";
        public static string g_sManLoginDearOnlineDearMsg = "你的老公{0}在:{1}({2}:{3})上线了!!!。";
        public static string g_sWoManLoginDearOnlineSelfMsg = "你的老公当前位于{0}({1}:{2})。";
        public static string g_sWoManLoginDearOnlineDearMsg = "你的老婆{0}在:{1}({2}:{3}) 上线了!!!。";
        public static string g_sManLoginDearNotOnlineMsg = "你的老婆现在不在线!!!";
        public static string g_sWoManLoginDearNotOnlineMsg = "你的老公现在不在线!!!";
        public static string g_sManLongOutDearOnlineMsg = "你的老公在:{0}({1}:{2})下线了!!!。";
        public static string g_sWoManLongOutDearOnlineMsg = "你的老婆在:{0}({1}:{2})下线了!!!。";
        public static string g_sYouAreNotMarryedMsg = "你都没结婚查什么？";
        public static string g_sYourWifeNotOnlineMsg = "你的老婆还没有上线!!!";
        public static string g_sYourHusbandNotOnlineMsg = "你的老公还没有上线!!!";
        public static string g_sYourWifeNowLocateMsg = "你的老婆现在位于:";
        public static string g_sYourHusbandSearchLocateMsg = "你的老公正在找你，他现在位于:";
        public static string g_sYourHusbandNowLocateMsg = "你的老公现在位于:";
        public static string g_sYourWifeSearchLocateMsg = "你的老婆正在找你，他现在位于:";
        public static string g_sfUnMasterLoginMsg = "你的徒弟{0}已经背判师门了!!!";
        public static string g_sfUnMasterListLoginMsg = "你的师父{0}已经将你逐出师门了!!!";
        public static string g_sMasterListOnlineSelfMsg = "你的师父{0}当前位于{1}({2}:{3})。";
        public static string g_sMasterListOnlineMasterMsg = "你的徒弟{0}在:{1}({2}:{3})上线了!!!。";
        public static string g_sMasterOnlineSelfMsg = "你的徒弟当前位于{0}({1}:{2})。";
        public static string g_sMasterOnlineMasterListMsg = "你的师父{0}在:{1}({2}:{3}) 上线了!!!。";
        public static string g_sMasterLongOutMasterListOnlineMsg = "你的师父在:{0}({1}:{2})下线了!!!。";
        public static string g_sMasterListLongOutMasterOnlineMsg = "你的徒弟{0}在:{1}({2}:{3})下线了!!!。";
        public static string g_sMasterListNotOnlineMsg = "你的师父现不在线!!!";
        public static string g_sMasterNotOnlineMsg = "你的徒弟现不在线!!!";
        public static string g_sYouAreNotMasterMsg = "你都没师徒关系查什么？";
        public static string g_sYourMasterNotOnlineMsg = "你的师父还没有上线!!!";
        public static string g_sYourMasterListNotOnlineMsg = "你的徒弟还没有上线!!!";
        public static string g_sYourMasterNowLocateMsg = "你的师父现在位于:";
        public static string g_sYourMasterListSearchLocateMsg = "你的徒弟正在找你，他现在位于:";
        public static string g_sYourMasterListNowLocateMsg = "你的徒弟现在位于:";
        public static string g_sYourMasterSearchLocateMsg = "你的师父正在找你，他现在位于:";
        public static string g_sYourMasterListUnMasterOKMsg = "你的徒弟{0}已经圆满出师了!!!";
        public static string g_sYouAreUnMasterOKMsg = "你已经出师了!!!";
        public static string g_sUnMasterLoginMsg = "你的一个徒弟已经圆满出师了!!!";
        public static string g_sNPCSayUnMasterOKMsg = "[{0}]: 我宣布{1}与{2}正式脱离师徒关系。";
        public static string g_sNPCSayForceUnMasterMsg = "[{0}]: 我宣布{1}与{2}已经正式脱离师徒关系!!!";
        public static string g_sMyInfo = string.Empty;
        public static string g_sSendOnlineCountMsg = "当前在线人数: {0}";
        public static string g_sOpenedDealMsg = "开始交易。";
        public static string g_sSendCustMsgCanNotUseNowMsg = "祝福语功能还没有开放!!!";
        public static string g_sSubkMasterMsgCanNotUseNowMsg = "城主发信息功能还没有开放!!!";
        public static string g_sWeaponRepairSuccess = "武器修复成功...";
        public static string g_sDefenceUpTime = "防御力增加{0}秒";
        public static string g_sMagDefenceUpTime = "魔法防御力增加{0}秒";
        public static string g_sAttPowerUpTime = "物理攻击力增加{0}分钟{1}秒 ";
        public static string g_sAttPowerDownTime = "物理攻击力减少了{0}分钟{1}秒";
        public static string g_sWinLottery1Msg = "祝贺您，中了一等奖。";
        public static string g_sWinLottery2Msg = "祝贺您，中了二等奖。";
        public static string g_sWinLottery3Msg = "祝贺您，中了三等奖。";
        public static string g_sWinLottery4Msg = "祝贺您，中了四等奖。";
        public static string g_sWinLottery5Msg = "祝贺您，中了五等奖。";
        public static string g_sWinLottery6Msg = "祝贺您，中了六等奖。";
        public static string g_sNotWinLotteryMsg = "等下次机会吧!!!";
        public static string g_sWeaptonMakeLuck = "武器被加幸运了...";
        public static string g_sWeaptonNotMakeLuck = "无效!!!";
        public static string g_sTheWeaponIsCursed = "您的武器被诅咒了。";
        public static string g_sCanotTakeOffItem = "无法取下物品!!!";
        public static string g_sJoinGroup = "{0} 已加入小组.";
        public static string g_sTryModeCanotUseStorage = "试玩模式不可以使用仓库功能!!!";
        public static string g_sCanotGetItems = "无法携带更多的东西!!!";
        public static string g_sYourIPaddrDenyLogon = "你当前登录的IP地址已被禁止登录了!!!";
        public static string g_sYourAccountDenyLogon = "你当前登录的帐号已被禁止登录了!!!";
        public static string g_sYourCharNameDenyLogon = "你当前登录的人物已被禁止登录了!!!";
        public static string g_sCanotPickUpItem = "在一定时间以内无法捡起此物品!!!";
        public static string g_sQUERYBAGITEMS = "一定时间内不能连续刷新背包物品...";
        public static string g_sCanotSendmsg = "无法发送信息.";
        public static string g_sUserDenyWhisperMsg = " 拒绝私聊!!!";
        public static string g_sUserNotOnLine = "  没有在线!!!";
        public static string g_sRevivalRecoverMsg = "复活戒指生效，体力恢复.";
        public static string g_sClientVersionTooOld = "由于您使用的客户端版本太老了，无法正确显示人物信息!!!";
        public static string g_sCastleGuildName = "(%castlename)%guildname[%rankname]";
        public static string g_sNoCastleGuildName = "%guildname[%rankname]";
        public static string g_sWarrReNewName = "%chrname\\*<圣>*";
        public static string g_sWizardReNewName = "%chrname\\*<神>*";
        public static string g_sTaosReNewName = "%chrname\\*<尊>*";
        public static string g_sRankLevelName = "{0}\\平民";
        public static string g_sManDearName = "{0}的老公";
        public static string g_sWoManDearName = "{0}的妻子";
        public static string g_sMasterName = "{0}的师傅";
        public static string g_sNoMasterName = "{0}的徒弟";
        public static string g_sHumanShowName = "%chrname\\%guildname\\%dearname\\%mastername";
        public static string g_sChangePermissionMsg = "当前权限等级为:{0}";
        public static string g_sChangeKillMonExpRateMsg = "经验倍数:{0} 时长{1}秒";
        public static string g_sChangePowerRateMsg = "攻击力倍数:{0} 时长{1}秒";
        public static string g_sChangeMemberLevelMsg = "当前会员等级为:{0}";
        public static string g_sChangeMemberTypeMsg = "当前会员类型为:{0}";
        public static string g_sScriptChangeHumanHPMsg = "当前HP值为:{0}";
        public static string g_sScriptChangeHumanMPMsg = "当前MP值为:{0}";
        public static string g_sScriptGuildAuraePointNoGuild = "你还没加入行会!!!";
        public static string g_sScriptGuildAuraePointMsg = "你的行会人气度为:{0}";
        public static string g_sScriptGuildBuildPointNoGuild = "你还没加入行会!!!";
        public static string g_sScriptGuildBuildPointMsg = "你的行会的建筑度为:{0}";
        public static string g_sScriptGuildFlourishPointNoGuild = "你还没加入行会!!!";
        public static string g_sScriptGuildFlourishPointMsg = "你的行会的繁荣度为:{0}";
        public static string g_sScriptGuildStabilityPointNoGuild = "你的行会的建筑度为:{0}";
        public static string g_sScriptGuildStabilityPointMsg = "你的行会的安定度为:{0}";
        public static string g_sScriptChiefItemCountMsg = "你的行会的超级装备数为:{0}";
        public static string g_sDisableSayMsg = "[由于你重复发相同的内容，{0}分钟内你将被禁止发言...]";
        public static string g_sOnlineCountMsg = "在线数: {0}";
        public static string g_sTotalOnlineCountMsg = "总在线数: {0}";
        public static string g_sYouNeedLevelMsg = "你的等级要在{0}级以上才能用此功能!!!";
        public static string g_sThisMapDisableSendCyCyMsg = "本地图不允许喊话!!!";
        public static string g_sYouCanSendCyCyLaterMsg = "{0}秒后才可以再发文字!!!";
        public static string g_sYouIsDisableSendMsg = "禁止聊天!!!";
        public static string g_sYouMurderedMsg = "你犯了谋杀罪!!!";
        public static string g_sYouKilledByMsg = "你被{0}杀害了!!!";
        public static string g_sYouprotectedByLawOfDefense = "[你受到正当规则保护。]";
        public static string g_sYourUseItemIsNul = "你的{0}处没有放上装备!!!";

        public const string g_sVersion = "引擎版本: 1.00 Build 20161001";
        public const string g_sUpDateTime = "更新日期: 2016/10/01";
        private const string sSTATUS_FAIL = "+FL/{0}";
        private const string sSTATUS_GOOD = "+GD/{0}";

        public const ushort MAXUPLEVEL = ushort.MaxValue;
        public const ushort MAXHUMPOWER = 1000;
        public const ushort BODYLUCKUNIT = 5000;
        public const ushort DEFHIT = 5;
        public const ushort DEFSPEED = 15;


        public const string U_DRESSNAME = "衣服";
        public const string U_WEAPONNAME = "武器";
        public const string U_RIGHTHANDNAME = "照明物";
        public const string U_NECKLACENAME = "项链";
        public const string U_HELMETNAME = "头盔";
        public const string U_ARMRINGLNAME = "左手镯";
        public const string U_ARMRINGRNAME = "右手镯";
        public const string U_RINGLNAME = "左戒指";
        public const string U_RINGRNAME = "右戒指";
        public const string U_BUJUKNAME = "物品";
        public const string U_BELTNAME = "腰带";
        public const string U_BOOTSNAME = "鞋子";
        public const string U_CHARMNAME = "宝石";

        public static string GetGoodTick => string.Format(sSTATUS_GOOD, HUtil32.GetTickCount());

        public static bool LoadLineNotice(string FileName)
        {
            var result = false;
            int i;
            string sText;
            StringList LoadList = null;
            if (File.Exists(FileName))
            {
                LoadList = new StringList();
                LoadList.LoadFromFile(FileName);
                i = 0;
                while (true)
                {
                    if (LoadList.Count <= i)
                    {
                        break;
                    }
                    sText = LoadList[i].Trim();
                    if (string.IsNullOrEmpty(sText))
                    {
                        LoadList.RemoveAt(i);
                        continue;
                    }
                    LineNoticeList.Add(sText);
                    i++;
                }
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 随机获取其他服务器
        /// </summary>
        /// <param name="serverIndex"></param>
        /// <param name="sIPaddr"></param>
        /// <param name="nPort"></param>
        /// <returns></returns>
        public static bool GetMultiServerAddrPort(byte serverIndex, ref string sIPaddr, ref int nPort)
        {
            var result = false;
            for (var i = 0; i < ServerTableList.Length; i++)
            {
                var routeInfo = ServerTableList[i];
                if (routeInfo == null)
                {
                    continue;
                }
                if (routeInfo.nGateCount <= 0)
                {
                    continue;
                }
                if (routeInfo.nServerIdx == serverIndex)
                {
                    sIPaddr = GetRandpmRoute(routeInfo, ref nPort);
                    result = true;
                    break;
                }
            }
            return result;
        }

        private static string GetRandpmRoute(TRouteInfo RouteInfo, ref int nGatePort)
        {
            var nC = RandomNumber.Random(RouteInfo.nGateCount);
            nGatePort = RouteInfo.nGameGatePort[nC];
            return RouteInfo.sGameGateIP[nC];
        }

        static M2Share()
        {
            //todo 优化配置文件读取方式
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                BasePath = "/Volumes/Data/MirServer/Mir200";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            {
                BasePath = "/opt/MirServer/Mir200";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                BasePath = "D:/MirServer/Mir200";
            }
            else
            {
                throw new Exception("不受支持的操作系统");
            }
            ServerConf = new ServerConf(Path.Combine(BasePath, ConfConst.sConfigFileName));
            StringConf = new StringConf(Path.Combine(BasePath, ConfConst.sStringFileName));
            ExpConf = new ExpsConf(Path.Combine(BasePath, ConfConst.sExpConfigFileName));
            GlobalConf = new GlobalConf(Path.Combine(BasePath, ConfConst.sGlobalConfigFileName));
            Log = new MirLog();
            Config = new GameSvrConf();
            RandomNumber = RandomNumber.GetInstance();
        }

        public static int GetExVersionNO(int nVersionDate, ref int nOldVerstionDate)
        {
            var result = 0;
            if (nVersionDate > 10000000)
            {
                while (nVersionDate > 10000000)
                {
                    nVersionDate -= 10000;
                    result += 100000000;
                }
            }
            nOldVerstionDate = nVersionDate;
            return result;
        }

        public static byte GetNextDirection(int sx, int sy, int dx, int dy)
        {
            int flagx;
            int flagy;
            byte result = Grobal2.DR_DOWN;
            if (sx < dx)
            {
                flagx = 1;
            }
            else if (sx == dx)
            {
                flagx = 0;
            }
            else
            {
                flagx = -1;
            }
            if (Math.Abs(sy - dy) > 2)
            {
                if (sx >= dx - 1 && sx <= dx + 1)
                {
                    flagx = 0;
                }
            }
            if (sy < dy)
            {
                flagy = 1;
            }
            else if (sy == dy)
            {
                flagy = 0;
            }
            else
            {
                flagy = -1;
            }
            if (Math.Abs(sx - dx) > 2)
            {
                if (sy > dy - 1 && sy <= dy + 1)
                {
                    flagy = 0;
                }
            }
            if (flagx == 0 && flagy == -1)
            {
                result = Grobal2.DR_UP;
            }
            if (flagx == 1 && flagy == -1)
            {
                result = Grobal2.DR_UPRIGHT;
            }
            if (flagx == 1 && flagy == 0)
            {
                result = Grobal2.DR_RIGHT;
            }
            if (flagx == 1 && flagy == 1)
            {
                result = Grobal2.DR_DOWNRIGHT;
            }
            if (flagx == 0 && flagy == 1)
            {
                result = Grobal2.DR_DOWN;
            }
            if (flagx == -1 && flagy == 1)
            {
                result = Grobal2.DR_DOWNLEFT;
            }
            if (flagx == -1 && flagy == 0)
            {
                result = Grobal2.DR_LEFT;
            }
            if (flagx == -1 && flagy == -1)
            {
                result = Grobal2.DR_UPLEFT;
            }
            return result;
        }

        public static bool CheckUserItems(int nIdx, Items.StdItem StdItem)
        {
            var result = false;
            switch (nIdx)
            {
                case Grobal2.U_DRESS:
                    if (StdItem.StdMode == 10 || StdItem.StdMode == 11)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_WEAPON:
                    if (StdItem.StdMode == 5 || StdItem.StdMode == 6)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_RIGHTHAND:
                    if (StdItem.StdMode == 29 || StdItem.StdMode == 30 || StdItem.StdMode == 28)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_NECKLACE:
                    if (StdItem.StdMode == 19 || StdItem.StdMode == 20 || StdItem.StdMode == 21)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_HELMET:
                    if (StdItem.StdMode == 15)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_ARMRINGL:
                    if (StdItem.StdMode == 24 || StdItem.StdMode == 25 || StdItem.StdMode == 26)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_ARMRINGR:
                    if (StdItem.StdMode == 24 || StdItem.StdMode == 26)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_RINGL:
                case Grobal2.U_RINGR:
                    if (StdItem.StdMode == 22 || StdItem.StdMode == 23)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_BUJUK:
                    if (StdItem.StdMode == 25 || StdItem.StdMode == 51)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_BELT:
                    if (StdItem.StdMode == 54 || StdItem.StdMode == 64)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_BOOTS:
                    if (StdItem.StdMode == 52 || StdItem.StdMode == 62)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_CHARM:
                    if (StdItem.StdMode == 53 || StdItem.StdMode == 63)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        public static DateTime AddDateTimeOfDay(DateTime DateTime, int nDay)
        {
            var result = DateTime.Now;
            //if (nDay > 0)
            //{
            //    nDay -= 1;
            //    Year = DateTime.Year;
            //    Month = DateTime.Month;
            //    Day = DateTime.Day;
            //    while (true)
            //    {
            //        if (MonthDays[false][Month] >= (Day + nDay))
            //        {
            //            break;
            //        }
            //        nDay = (Day + nDay) - MonthDays[false][Month] - 1;
            //        Day = 1;
            //        if (Month <= 11)
            //        {
            //            Month ++;
            //            continue;
            //        }
            //        Month = 1;
            //        if (Year == 99)
            //        {
            //            Year = 2000;
            //            continue;
            //        }
            //        Year ++;
            //    }
            //    Day += nDay;
            //    result = new DateTime(Year, Month ,Day);
            //}
            //else
            //{
            //    result = DateTime;
            //}
            return result;
        }

        public static ushort GetGoldShape(int nGold)
        {
            ushort result = 112;
            if (nGold >= 30)
            {
                result = 113;
            }
            if (nGold >= 70)
            {
                result = 114;
            }
            if (nGold >= 300)
            {
                result = 115;
            }
            if (nGold >= 1000)
            {
                result = 116;
            }
            return result;
        }

        // 金币在地上显示的外形ID
        public static int GetRandomLook(int nBaseLook, int nRage)
        {
            return nBaseLook + RandomNumber.Random(nRage);
        }

        public static bool CheckGuildName(string sGuildName)
        {
            var result = true;
            if (sGuildName.Length > Config.GuildNameLen)
            {
                result = false;
                return result;
            }
            for (var i = 0; i < sGuildName.Length - 1; i++)
            {
                if (sGuildName[i] < '0' || sGuildName[i] == '/' || sGuildName[i] == '\\' || sGuildName[i] == ':' || sGuildName[i] == '*' || sGuildName[i] == ' '
                    || sGuildName[i] == '\"' || sGuildName[i] == '\'' || sGuildName[i] == '<' || sGuildName[i] == '|' || sGuildName[i] == '?' || sGuildName[i] == '>')
                {
                    result = false;
                }
            }
            return result;
        }

        public static int GetItemNumber()
        {
            Config.ItemNumber++;
            if (Config.ItemNumber > int.MaxValue / 2 - 1)
            {
                Config.ItemNumber = 1;
            }
            return Config.ItemNumber;
        }

        public static int GetItemNumberEx()
        {
            Config.ItemNumberEx++;
            if (Config.ItemNumberEx < int.MaxValue / 2)
            {
                Config.ItemNumberEx = int.MaxValue / 2;
            }
            if (Config.ItemNumberEx > int.MaxValue - 1)
            {
                Config.ItemNumberEx = int.MaxValue / 2;
            }
            return Config.ItemNumberEx;
        }

        public static string FilterShowName(string sName)
        {
            var result = "";
            var sC = "";
            var bo11 = false;
            if (string.IsNullOrEmpty(sName))
            {
                return sName;
            }
            for (var i = 0; i < sName.Length - 1; i++)
            {
                if (sName[i] >= '0' && sName[i] <= '9' || sName[i] == '-')
                {
                    result = sName.Substring(0, i);
                    sC = sName.Substring(i, sName.Length - i);
                    bo11 = true;
                    break;
                }
            }
            if (!bo11)
            {
                result = sName;
            }
            return result;
        }

        public static byte sub_4B2F80(int nDir, int nRage)
        {
            return (byte)((nDir + nRage) % 8);
        }

        /// <summary>
        /// 获取服务器变量
        /// </summary>
        /// <param name="sText"></param>
        /// <returns></returns>
        public static int GetValNameNo(string sText)
        {
            var result = -1;
            int nValNo;
            if (sText.Length >= 2)
            {
                var valType = char.ToUpper(sText[0]);
                switch (valType)
                {
                    case 'P':
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.Str_ToInt(sText.Substring(1, 2), -1);
                            if ((nValNo >= 0) && (nValNo < 100))
                            {
                                result = nValNo;
                            }
                        }
                        else
                        {
                            nValNo = HUtil32.Str_ToInt(sText[1].ToString(), -1);
                            if ((nValNo >= 0) && (nValNo < 10))
                            {
                                result = nValNo;
                            }
                        }
                        break;
                    case 'G':
                        if (sText.Length == 4)
                        {
                            nValNo = HUtil32.Str_ToInt(sText.Substring(1, 3), -1);
                            if ((nValNo < 500) && (nValNo > 99))
                            {
                                result = nValNo + 700;
                            }
                        }
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.Str_ToInt(sText.Substring(1, 2), -1);
                            if ((nValNo >= 0) && (nValNo < 100))
                            {
                                result = nValNo + 100;
                            }
                        }
                        else
                        {
                            nValNo = HUtil32.Str_ToInt(sText[1].ToString(), -1);
                            if ((nValNo >= 0) && (nValNo < 10))
                            {
                                result = nValNo + 100;
                            }
                        }
                        break;
                    case 'M':
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.Str_ToInt(sText.Substring(1, 2), -1);
                            if ((nValNo >= 0) && (nValNo < 100))
                            {
                                result = nValNo + 300;
                            }
                        }
                        else
                        {
                            nValNo = HUtil32.Str_ToInt(sText[1].ToString(), -1);
                            if ((nValNo >= 0) && (nValNo < 10))
                            {
                                result = nValNo + 300;
                            }
                        }
                        break;
                    case 'I':
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.Str_ToInt(sText.Substring(1, 2), -1);
                            if ((nValNo >= 0) && (nValNo < 100))
                            {
                                result = nValNo + 400;
                            }
                        }
                        else
                        {
                            nValNo = HUtil32.Str_ToInt(sText[1].ToString(), -1);
                            if ((nValNo >= 0) && (nValNo < 10))
                            {
                                result = nValNo + 400;
                            }
                        }
                        break;
                    case 'D':
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.Str_ToInt(sText.Substring(1, 2), -1);
                            if ((nValNo >= 0) && (nValNo < 100))
                            {
                                result = nValNo + 200;
                            }
                        }
                        else
                        {
                            nValNo = HUtil32.Str_ToInt(sText[1].ToString(), -1);
                            if ((nValNo >= 0) && (nValNo < 10))
                            {
                                result = nValNo + 200;
                            }
                        }
                        break;
                    case 'N':
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.Str_ToInt(sText.Substring(1, 2), -1);
                            if ((nValNo >= 0) && (nValNo < 100))
                            {
                                result = nValNo + 500;
                            }
                        }
                        else
                        {
                            nValNo = HUtil32.Str_ToInt(sText[1].ToString(), -1);
                            if ((nValNo >= 0) && (nValNo < 10))
                            {
                                result = nValNo + 500;
                            }
                        }
                        break;
                    case 'S':
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.Str_ToInt(sText.Substring(2 - 1, 2), -1);
                            if ((nValNo >= 0) && (nValNo < 100))
                            {
                                result = nValNo + 600;
                            }
                        }
                        else
                        {
                            nValNo = HUtil32.Str_ToInt(sText[1].ToString(), -1);
                            if ((nValNo >= 0) && (nValNo < 10))
                            {
                                result = nValNo + 600;
                            }
                        }
                        break;
                    case 'A':
                        if (sText.Length == 4)
                        {
                            nValNo = HUtil32.Str_ToInt(sText.Substring(1, 3), -1);
                            if ((nValNo < 500) && (nValNo > 99))
                            {
                                result = nValNo + 1100;
                            }
                        }
                        else
                        {
                            if (sText.Length == 3)
                            {
                                nValNo = HUtil32.Str_ToInt(sText.Substring(1, 2), -1);
                                if ((nValNo >= 0) && (nValNo < 100))
                                {
                                    result = nValNo + 700;
                                }
                            }
                            else
                            {
                                nValNo = HUtil32.Str_ToInt(sText[1].ToString(), -1);
                                if ((nValNo >= 0) && (nValNo < 10))
                                {
                                    result = nValNo + 700;
                                }
                            }
                        }
                        break;
                    case 'T':
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.Str_ToInt(sText.Substring(2 - 1, 3), -1);
                            if ((nValNo >= 0) && (nValNo < 100))
                            {
                                result = nValNo + 700;
                            }
                        }
                        else
                        {
                            nValNo = HUtil32.Str_ToInt(sText[1].ToString(), -1);
                            if ((nValNo >= 0) && (nValNo < 10))
                            {
                                result = nValNo + 700;
                            }
                        }
                        break;
                    case 'E':
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.Str_ToInt(sText.Substring(2 - 1, 2), -1);
                            if ((nValNo >= 0) && (nValNo < 100))
                            {
                                result = nValNo + 1600;
                            }
                        }
                        else
                        {
                            nValNo = HUtil32.Str_ToInt(sText[1].ToString(), -1);
                            if ((nValNo >= 0) && (nValNo < 10))
                            {
                                result = nValNo + 1600;
                            }
                        }
                        break;
                    case 'W':
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.Str_ToInt(sText.Substring(2 - 1, 2), -1);
                            if ((nValNo >= 0) && (nValNo < 100))
                            {
                                result = nValNo + 1700;
                            }
                        }
                        else
                        {
                            nValNo = HUtil32.Str_ToInt(sText[1].ToString(), -1);
                            if ((nValNo >= 0) && (nValNo < 10))
                            {
                                result = nValNo + 1700;
                            }
                        }
                        break;
                }
            }
            return result;
        }

        public static bool IsAccessory(ushort nIndex)
        {
            bool result;
            var item = WorldEngine.GetStdItem(nIndex);
            if (new ArrayList(new byte[] { 19, 20, 21, 22, 23, 24, 26 }).Contains(item.StdMode))// 修正错误
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public static IList<MakeItem> GetMakeItemInfo(string sItemName)
        {
            if (MakeItemList.TryGetValue(sItemName, out var itemList))
            {
                return itemList;
            }
            return null;
        }

        public static string GetStartPointInfo(int nIndex, ref short nX, ref short nY)
        {
            var result = string.Empty;
            nX = 0;
            nY = 0;
            if (nIndex >= 0 && nIndex < StartPointList.Count)
            {
                var StartPoint = StartPointList[nIndex];
                if (StartPoint != null)
                {
                    nX = StartPoint.m_nCurrX;
                    nY = StartPoint.m_nCurrY;
                    result = StartPoint.m_sMapName;
                }
            }
            return result;
        }

        public static void AddGameDataLog(string sMsg)
        {
            ItemLogQueue.Enqueue(sMsg);
        }

        public static void AddLogonCostLog(string sMsg)
        {
            LogonCostLogList.Add(sMsg);
        }

        public static void TrimStringList(StringList sList)
        {
            int n8;
            string sC;
            n8 = 0;
            while (true)
            {
                if (sList.Count <= n8)
                {
                    break;
                }
                sC = sList[n8].Trim();
                if (sC == "")
                {
                    sList.RemoveAt(n8);
                    continue;
                }
                n8++;
            }
        }

        public static bool CanMakeItem(string sItemName)
        {
            bool result;
            result = false;
            //g_DisableMakeItemList.__Lock();
            //try {
            //    for (I = 0; I < g_DisableMakeItemList.Count; I ++ )
            //    {
            //        if ((g_DisableMakeItemList[I]).CompareTo((sItemName)) == 0)
            //        {
            //            result = false;
            //            return result;
            //        }
            //    }
            //} finally {
            //    g_DisableMakeItemList.UnLock();
            //}
            //g_EnableMakeItemList.__Lock();
            //try {
            //    for (I = 0; I < g_EnableMakeItemList.Count; I ++ )
            //    {
            //        if ((g_EnableMakeItemList[I]).CompareTo((sItemName)) == 0)
            //        {
            //            result = true;
            //            break;
            //        }
            //    }
            //} finally {
            //    g_EnableMakeItemList.UnLock();
            //}
            return result;
        }

        public static bool CanMoveMap(string sMapName)
        {
            bool result;
            int I;
            result = true;
            try
            {
                for (I = 0; I < g_DisableMoveMapList.Count; I++)
                {
                    //if ((g_DisableMoveMapList[I]).CompareTo((sMapName)) == 0)
                    //{
                    //    result = false;
                    //    break;
                    //}
                }
            }
            finally
            {
            }
            return result;
        }

        public static bool CanSellItem(string sItemName)
        {
            bool result;
            int i;
            result = true;
            try
            {
                for (i = 0; i < g_DisableSellOffList.Count; i++)
                {
                    //if ((g_DisableSellOffList[i]).CompareTo((sItemName)) == 0)
                    //{
                    //    result = false;
                    //    break;
                    //}
                }
            }
            finally
            {
            }
            return result;
        }

        public static bool LoadItemBindIPaddr()
        {
            bool result;
            ArrayList LoadList;
            var sFileName = string.Empty;
            var sLineText = string.Empty;
            var sMakeIndex = string.Empty;
            var sItemIndex = string.Empty;
            var sBindName = string.Empty;
            result = false;
            sFileName = Config.EnvirDir + "ItemBindIPaddr.txt";
            LoadList = new ArrayList();
            //if (File.Exists(sFileName))
            //{
            //    g_ItemBindIPaddr.__Lock();
            //    try {
            //        for (I = 0; I < g_ItemBindIPaddr.Count; I++)
            //        {
            //            Dispose(((g_ItemBindIPaddr[I]) as TItemBind));
            //        }
            //        g_ItemBindIPaddr.Clear();
            //        LoadList.LoadFromFile(sFileName);
            //        for (I = 0; I < LoadList.Count; I++)
            //        {
            //            sLineText = LoadList[I].Trim();
            //            if (sLineText[1] == ';')
            //            {
            //                continue;
            //            }
            //            sLineText = HUtil32.GetValidStr3(sLineText, ref sItemIndex, new string[] { " ", ",", "\t" });
            //            sLineText = HUtil32.GetValidStr3(sLineText, ref sMakeIndex, new string[] { " ", ",", "\t" });
            //            sLineText = HUtil32.GetValidStr3(sLineText, ref sBindName, new string[] { " ", ",", "\t" });
            //            nMakeIndex = HUtil32.Str_ToInt(sMakeIndex, -1);
            //            nItemIndex = HUtil32.Str_ToInt(sItemIndex, -1);
            //            if ((nMakeIndex > 0) && (nItemIndex > 0) && (sBindName != ""))
            //            {
            //                ItemBind = new TItemBind();
            //                ItemBind.nMakeIdex = nMakeIndex;
            //                ItemBind.nItemIdx = nItemIndex;
            //                ItemBind.sBindName = sBindName;
            //                g_ItemBindIPaddr.Add(ItemBind);
            //            }
            //        }
            //    } finally {
            //        g_ItemBindIPaddr.UnLock();
            //    }
            //    result = true;
            //}
            //else
            //{
            //    LoadList.SaveToFile(sFileName);
            //}
            //LoadList.Free;
            return result;
        }

        public static bool SaveItemBindIPaddr()
        {
            bool result;
            result = false;
            var sFileName = M2Share.BasePath + Config.EnvirDir + "ItemBindIPaddr.txt";
            //SaveList = new StringList();
            //try {
            //    for (I = 0; I < g_ItemBindIPaddr.Count; I++)
            //    {
            //        ItemBind = g_ItemBindIPaddr[I];
            //        SaveList.Add((ItemBind.nItemIdx).ToString() + "\t" + (ItemBind.nMakeIdex).ToString() + "\t" + ItemBind.sBindName);
            //    }
            //} finally {
            //}
            //SaveList.SaveToFile(sFileName);
            //SaveList.Free;
            result = true;
            return result;
        }

        public static bool LoadItemBindAccount()
        {
            ArrayList LoadList;
            var sMakeIndex = string.Empty;
            var sItemInde = string.Empty;
            var sBindName = string.Empty;
            var result = false;
            string sFileName = M2Share.BasePath + Config.EnvirDir + "ItemBindAccount.txt";
            LoadList = new ArrayList();
            //if (File.Exists(sFileName))
            //{
            //    g_ItemBindAccount.__Lock();
            //    try {
            //        for (I = 0; I < g_ItemBindAccount.Count; I++)
            //        {
            //            Dispose(((g_ItemBindAccount[I]) as TItemBind));
            //        }
            //        g_ItemBindAccount.Clear();
            //        LoadList.LoadFromFile(sFileName);
            //        for (I = 0; I < LoadList.Count; I++)
            //        {
            //            sLineText = LoadList[I].Trim();
            //            if (sLineText[1] == ';')
            //            {
            //                continue;
            //            }
            //            sLineText = HUtil32.GetValidStr3(sLineText, ref sItemIndex, new string[] { " ", ",", "\t" });
            //            sLineText = HUtil32.GetValidStr3(sLineText, ref sMakeIndex, new string[] { " ", ",", "\t" });
            //            sLineText = HUtil32.GetValidStr3(sLineText, ref sBindName, new string[] { " ", ",", "\t" });
            //            nMakeIndex = HUtil32.Str_ToInt(sMakeIndex, -1);
            //            nItemIndex = HUtil32.Str_ToInt(sItemIndex, -1);
            //            if ((nMakeIndex > 0) && (nItemIndex > 0) && (sBindName != ""))
            //            {
            //                ItemBind = new TItemBind();
            //                ItemBind.nMakeIdex = nMakeIndex;
            //                ItemBind.nItemIdx = nItemIndex;
            //                ItemBind.sBindName = sBindName;
            //                g_ItemBindAccount.Add(ItemBind);
            //            }
            //        }
            //    } finally {
            //        g_ItemBindAccount.UnLock();
            //    }
            //    result = true;
            //}
            //else
            //{
            //    LoadList.SaveToFile(sFileName);
            //}
            //LoadList.Free;
            return result;
        }

        public static bool SaveItemBindAccount()
        {
            var result = false;
            var sFileName = Config.EnvirDir + "ItemBindAccount.txt";
            //SaveList = new StringList();
            //g_ItemBindAccount.__Lock();
            //try {
            //    for (I = 0; I < g_ItemBindAccount.Count; I ++ )
            //    {
            //        ItemBind = g_ItemBindAccount[I];
            //        SaveList.Add((ItemBind.nItemIdx).ToString() + "\t" + (ItemBind.nMakeIdex).ToString() + "\t" + ItemBind.sBindName);
            //    }
            //} finally {
            //    g_ItemBindAccount.UnLock();
            //}

            //SaveList.SaveToFile(sFileName);

            //SaveList.Free;
            result = true;
            return result;
        }

        public static bool LoadItemBindCharName()
        {
            var sMakeIndex = string.Empty;
            var sBindName = string.Empty;
            var result = false;
            string sFileName = M2Share.BasePath + Config.EnvirDir + "ItemBindChrName.txt";
            //if (File.Exists(sFileName))
            //{
            //    g_ItemBindCharName.__Lock();
            //    try {
            //        for (I = 0; I < g_ItemBindCharName.Count; I ++ )
            //        {
            //            Dispose(((g_ItemBindCharName[I]) as TItemBind));
            //        }
            //        g_ItemBindCharName.Clear();

            //        LoadList.LoadFromFile(sFileName);
            //        for (I = 0; I < LoadList.Count; I ++ )
            //        {
            //            sLineText = LoadList[I].Trim();
            //            if (sLineText[1] == ';')
            //            {
            //                continue;
            //            }
            //            sLineText = HUtil32.GetValidStr3(sLineText, ref sItemIndex, new string[] {" ", ",", "\t"});
            //            sLineText = HUtil32.GetValidStr3(sLineText, ref sMakeIndex, new string[] {" ", ",", "\t"});
            //            sLineText = HUtil32.GetValidStr3(sLineText, ref sBindName, new string[] {" ", ",", "\t"});
            //            nMakeIndex = HUtil32.Str_ToInt(sMakeIndex,  -1);
            //            nItemIndex = HUtil32.Str_ToInt(sItemIndex,  -1);
            //            if ((nMakeIndex > 0) && (nItemIndex > 0) && (sBindName != ""))
            //            {
            //                ItemBind = new TItemBind();
            //                ItemBind.nMakeIdex = nMakeIndex;
            //                ItemBind.nItemIdx = nItemIndex;
            //                ItemBind.sBindName = sBindName;
            //                g_ItemBindCharName.Add(ItemBind);
            //            }
            //        }
            //    } finally {
            //        g_ItemBindCharName.UnLock();
            //    }
            //    result = true;
            //}
            //else
            //{

            //    LoadList.SaveToFile(sFileName);
            //}

            //LoadList.Free;
            return result;
        }

        public static bool SaveItemBindCharName()
        {
            var result = false;
            var sFileName = M2Share.BasePath + Config.EnvirDir + "ItemBindChrName.txt";
            //g_ItemBindCharName.__Lock();
            //try {
            //    for (I = 0; I < g_ItemBindCharName.Count; I++)
            //    {
            //        ItemBind = g_ItemBindCharName[I];
            //        SaveList.Add((ItemBind.nItemIdx).ToString() + "\t" + (ItemBind.nMakeIdex).ToString() + "\t" + ItemBind.sBindName);
            //    }
            //} finally {
            //    g_ItemBindCharName.UnLock();
            //}
            //SaveList.SaveToFile(sFileName);
            ////SaveList.Free;
            //result = true;
            return result;
        }

        public static bool LoadDisableMakeItem()
        {
            var result = false;
            var sFileName = M2Share.BasePath + Config.EnvirDir + "DisableMakeItem.txt";
            var LoadList = new ArrayList();
            //if (File.Exists(sFileName))
            //{
            //    g_DisableMakeItemList.__Lock();
            //    try {
            //        g_DisableMakeItemList.Clear();
            //        LoadList.LoadFromFile(sFileName);
            //        for (I = 0; I < LoadList.Count; I ++ )
            //        {
            //            g_DisableMakeItemList.Add(LoadList[I].Trim());
            //        }
            //    } finally {
            //        g_DisableMakeItemList.UnLock();
            //    }
            //    result = true;
            //}
            //else
            //{
            //    LoadList.SaveToFile(sFileName);
            //}
            //LoadList.Free;
            return result;
        }

        public static bool SaveDisableMakeItem()
        {
            string sFileName = M2Share.BasePath + Config.EnvirDir + "DisableMakeItem.txt";
            //g_DisableMakeItemList.SaveToFile(sFileName);
            return true;
        }

        public static bool LoadUnMasterList()
        {
            bool result = false;
            string sFileName = M2Share.BasePath + Config.EnvirDir + "UnMaster.txt";
            ArrayList LoadList = new ArrayList();
            //if (File.Exists(sFileName))
            //{
            //    g_UnMasterList.__Lock();
            //    try {
            //        g_UnMasterList.Clear();

            //        LoadList.LoadFromFile(sFileName);
            //        for (I = 0; I < LoadList.Count; I ++ )
            //        {
            //            g_UnMasterList.Add(LoadList[I].Trim());
            //        }
            //    } finally {
            //        g_UnMasterList.UnLock();
            //    }
            //    result = true;
            //}
            //else
            //{
            //    LoadList.SaveToFile(sFileName);
            //}
            //LoadList.Free;
            return result;
        }

        public static bool SaveUnMasterList()
        {
            string sFileName = Config.EnvirDir + "UnMaster.txt";
            //g_UnMasterList.SaveToFile(sFileName);
            return true;
        }

        public static bool LoadUnForceMasterList()
        {
            var result = false;
            var sFileName = M2Share.BasePath + Config.EnvirDir + "UnForceMaster.txt";
            //if (File.Exists(sFileName))
            //{
            //    g_UnForceMasterList.__Lock();
            //    try {
            //        g_UnForceMasterList.Clear();
            //        LoadList.LoadFromFile(sFileName);
            //        for (I = 0; I < LoadList.Count; I++)
            //        {
            //            g_UnForceMasterList.Add(LoadList[I].Trim());
            //        }
            //    } finally {
            //        g_UnForceMasterList.UnLock();
            //    }
            //    result = true;
            //}
            //else
            //{
            //    LoadList.SaveToFile(sFileName);
            //}
            //LoadList.Free;
            return result;
        }

        public static bool SaveUnForceMasterList()
        {
            string sFileName = M2Share.BasePath + Config.EnvirDir + "UnForceMaster.txt";
            //g_UnForceMasterList.SaveToFile(sFileName);
            return true;
        }

        public static bool LoadEnableMakeItem()
        {
            var result = false;
            string sFileName = M2Share.BasePath + Config.EnvirDir + "EnableMakeItem.txt";
            //if (File.Exists(sFileName))
            //{
            //    g_EnableMakeItemList.__Lock();
            //    try {
            //        g_EnableMakeItemList.Clear();
            //        LoadList.LoadFromFile(sFileName);
            //        for (I = 0; I < LoadList.Count; I++)
            //        {
            //            g_EnableMakeItemList.Add(LoadList[I].Trim());
            //        }
            //    } finally {
            //        g_EnableMakeItemList.UnLock();
            //    }
            //    result = true;
            //}
            //else
            //{
            //    LoadList.SaveToFile(sFileName);
            //}
            // LoadList.Free;
            return result;
        }

        public static bool SaveEnableMakeItem()
        {
            string sFileName = Config.EnvirDir + "EnableMakeItem.txt";
            //g_EnableMakeItemList.SaveToFile(sFileName);
            return true;
        }

        public static bool LoadDisableMoveMap()
        {
            var result = false;
            var sFileName = Config.EnvirDir + "DisableMoveMap.txt";
            //if (File.Exists(sFileName))
            //{
            //    g_DisableMoveMapList.__Lock();
            //    try {
            //        g_DisableMoveMapList.Clear();
            //        LoadList.LoadFromFile(sFileName);
            //        for (var I = 0; I < LoadList.Count; I++)
            //        {
            //            g_DisableMoveMapList.Add(LoadList[I].Trim());
            //        }
            //    } finally {
            //        g_DisableMoveMapList.UnLock();
            //    }
            //    result = true;
            //}
            //else
            //{
            //    LoadList.SaveToFile(sFileName);
            //}
            //LoadList.Free;
            return result;
        }

        public static bool SaveDisableMoveMap()
        {
            string sFileName = Config.EnvirDir + "DisableMoveMap.txt";
            //g_DisableMoveMapList.SaveToFile(sFileName);
            return true;
        }

        public static bool LoadAllowSellOffItem()
        {
            var result = false;
            var sFileName = M2Share.BasePath + Config.EnvirDir + "DisableSellOffItem.txt";
            //if (File.Exists(sFileName))
            //{
            //    g_DisableSellOffList.__Lock();
            //    try {
            //        g_DisableSellOffList.Clear();

            //        LoadList.LoadFromFile(sFileName);
            //        for (I = 0; I < LoadList.Count; I ++ )
            //        {
            //            g_DisableSellOffList.Add(LoadList[I].Trim());
            //        }
            //    } finally {
            //        g_DisableSellOffList.UnLock();
            //    }
            //    result = true;
            //}
            //else
            //{
            //    LoadList.SaveToFile(sFileName);
            //}
            //LoadList.Free;
            return result;
        }

        public static bool SaveAllowSellOffItem()
        {
            string sFileName = M2Share.BasePath + Config.EnvirDir + "DisableSellOffItem.txt";
            //g_DisableSellOffList.SaveToFile(sFileName);
            return true;
        }

        public static bool SaveChatLog()
        {
            //if (File.Exists(sFileName))
            //{
            //    LoadList = new ArrayList();
            //    LoadList.LoadFromFile(sFileName);
            //    g_ChatLoggingList.Add(LoadList);
            //    //LoadList.Free;
            //}
            //else
            //{
            //    g_ChatLoggingList.__Lock();
            //}
            //try {
            //    g_ChatLoggingList.SaveToFile(sFileName);
            //} finally {
            //    g_ChatLoggingList.UnLock();
            //}
            return true;
        }

        public static int GetUseItemIdx(string sName)
        {
            int result = -1;
            if (string.Compare(sName, U_DRESSNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 0;
            }
            else if (string.Compare(sName, U_WEAPONNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 1;
            }
            else if (string.Compare(sName, U_RIGHTHANDNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 2;
            }
            else if (string.Compare(sName, U_NECKLACENAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 3;
            }
            else if (string.Compare(sName, U_HELMETNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 4;
            }
            else if (string.Compare(sName, U_ARMRINGLNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 5;
            }
            else if (string.Compare(sName, U_ARMRINGRNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 6;
            }
            else if (string.Compare(sName, U_RINGLNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 7;
            }
            else if (string.Compare(sName, U_RINGRNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 8;
            }
            else if (string.Compare(sName, U_BUJUKNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 9;
            }
            else if (string.Compare(sName, U_BELTNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 10;
            }
            else if (string.Compare(sName, U_BOOTSNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 11;
            }
            else if (string.Compare(sName, U_CHARMNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 12;
            }
            return result;
        }

        public static string GetUseItemName(int nIndex)
        {
            var result = string.Empty;
            switch (nIndex)
            {
                case 0:
                    result = U_DRESSNAME;
                    break;
                case 1:
                    result = U_WEAPONNAME;
                    break;
                case 2:
                    result = U_RIGHTHANDNAME;
                    break;
                case 3:
                    result = U_NECKLACENAME;
                    break;
                case 4:
                    result = U_HELMETNAME;
                    break;
                case 5:
                    result = U_ARMRINGLNAME;
                    break;
                case 6:
                    result = U_ARMRINGRNAME;
                    break;
                case 7:
                    result = U_RINGLNAME;
                    break;
                case 8:
                    result = U_RINGRNAME;
                    break;
                case 9:
                    result = U_BUJUKNAME;
                    break;
                case 10:
                    result = U_BELTNAME;
                    break;
                case 11:
                    result = U_BOOTSNAME;
                    break;
                case 12:
                    result = U_CHARMNAME;
                    break;
            }
            return result;
        }

        public static bool LoadDisableSendMsgList()
        {
            var result = false;
            string sFileName = Config.EnvirDir + "DisableSendMsgList.txt";
            ArrayList LoadList = new ArrayList();
            //if (File.Exists(sFileName))
            //{
            //    g_DisableSendMsgList.Clear();

            //    LoadList.LoadFromFile(sFileName);
            //    for (I = 0; I < LoadList.Count; I ++ )
            //    {
            //        g_DisableSendMsgList.Add(LoadList[I].Trim());
            //    }
            //    result = true;
            //}
            //else
            //{
            //    LoadList.SaveToFile(sFileName);
            //}
            //LoadList.Free;
            return result;
        }

        public static bool LoadMonDropLimitList()
        {
            var sLineText = string.Empty;
            var sItemName = string.Empty;
            var sItemCount = string.Empty;
            var result = false;
            string sFileName = M2Share.BasePath + Config.EnvirDir + "MonDropLimitList.txt";
            var LoadList = new ArrayList();
            //if (File.Exists(sFileName))
            //{
            //    g_MonDropLimitLIst.Clear();
            //    LoadList.LoadFromFile(sFileName);
            //    for (I = 0; I < LoadList.Count; I ++ )
            //    {
            //        sLineText = LoadList[I].Trim();
            //        if ((sLineText == "") || (sLineText[1] == ';'))
            //        {
            //            continue;
            //        }
            //        sLineText = HUtil32.GetValidStr3(sLineText, ref sItemName, new string[] {" ", "/", ",", "\t"});
            //        sLineText = HUtil32.GetValidStr3(sLineText, ref sItemCount, new string[] {" ", "/", ",", "\t"});
            //        nItemCount = HUtil32.Str_ToInt(sItemCount,  -1);
            //        if ((!string.IsNullOrEmpty(sItemName)) && (nItemCount >= 0))
            //        {
            //            MonDrop = new TMonDrop();
            //            MonDrop.sItemName = sItemName;
            //            MonDrop.nDropCount = 0;
            //            MonDrop.nNoDropCount = 0;
            //            MonDrop.nCountLimit = nItemCount;
            //            g_MonDropLimitLIst.Add(sItemName, MonDrop);
            //        }
            //    }
            //    result = true;
            //}
            //else
            //{
            //    LoadList.SaveToFile(sFileName);
            //}
            //LoadList.Free;
            return result;
        }

        public static bool SaveMonDropLimitList()
        {
            bool result;
            var sFileName = Config.EnvirDir + "MonDropLimitList.txt";
            var LoadList = new ArrayList();
            //for (I = 0; I < g_MonDropLimitLIst.Count; I ++ )
            //{

            //    MonDrop = ((TMonDrop)(g_MonDropLimitLIst.Values[I]));
            //    sLineText = MonDrop.sItemName + "\t" + (MonDrop.nCountLimit).ToString();
            //    LoadList.Add(sLineText);
            //}
            //LoadList.SaveToFile(sFileName);
            //LoadList.Free;
            result = true;
            return result;
        }

        public static bool LoadDisableTakeOffList()
        {
            var sItemName = string.Empty;
            var result = false;
            var sFileName = M2Share.BasePath + Config.EnvirDir + "DisableTakeOffList.txt";
            var LoadList = new ArrayList();
            //if (File.Exists(sFileName))
            //{
            //    LoadList.LoadFromFile(sFileName);
            //    g_DisableTakeOffList.__Lock();
            //    try {
            //        g_DisableTakeOffList.Clear();
            //        for (I = 0; I < LoadList.Count; I ++ )
            //        {
            //            sLineText = LoadList[I].Trim();
            //            if ((sLineText == "") || (sLineText[1] == ';'))
            //            {
            //                continue;
            //            }
            //            sLineText = HUtil32.GetValidStr3(sLineText, ref sItemName, new string[] {" ", "/", ",", "\t"});
            //            sLineText = HUtil32.GetValidStr3(sLineText, ref sItemIdx, new string[] {" ", "/", ",", "\t"});
            //            nItemIdx = HUtil32.Str_ToInt(sItemIdx,  -1);
            //            if ((!string.IsNullOrEmpty(sItemName)) && (nItemIdx >= 0))
            //            {
            //                g_DisableTakeOffList.Add(sItemName, ((nItemIdx) as Object));
            //            }
            //        }
            //    } finally {
            //        g_DisableTakeOffList.UnLock();
            //    }
            //    result = true;
            //}
            //else
            //{
            //    LoadList.SaveToFile(sFileName);
            //}
            //LoadList.Free;
            return result;
        }

        public static bool SaveDisableTakeOffList()
        {
            bool result;
            ArrayList LoadList;
            string sFileName;
            sFileName = M2Share.BasePath + Config.EnvirDir + "DisableTakeOffList.txt";
            LoadList = new ArrayList();
            //g_DisableTakeOffList.__Lock();
            //try {
            //    for (I = 0; I < g_DisableTakeOffList.Count; I++)
            //    {
            //        sLineText = g_DisableTakeOffList[I] + "\t" + (((int)g_DisableTakeOffList.Values[I])).ToString();
            //        LoadList.Add(sLineText);
            //    }
            //} finally {
            //    g_DisableTakeOffList.UnLock();
            //}
            //LoadList.SaveToFile(sFileName);
            //LoadList.Free;
            result = true;
            return result;
        }

        public static bool InDisableTakeOffList(int nItemIdx)
        {
            bool result = false;
            //for (I = 0; I < g_DisableTakeOffList.Count; I ++ )
            //{
            //    if (((int)g_DisableTakeOffList.Values[I]) == nItemIdx - 1)
            //    {
            //        result = true;
            //        break;
            //    }
            //}
            return result;
        }

        public static bool SaveDisableSendMsgList()
        {
            bool result;
            string sFileName = M2Share.BasePath + Config.EnvirDir + "DisableSendMsgList.txt";
            ArrayList LoadList = new ArrayList();
            for (var i = 0; i < g_DisableSendMsgList.Count; i++)
            {
                LoadList.Add(g_DisableSendMsgList[i]);
            }
            //LoadList.SaveToFile(sFileName);
            result = true;
            return result;
        }

        public static bool GetDisableSendMsgList(string sHumanName)
        {
            bool result;
            result = false;
            //for (I = 0; I < g_DisableSendMsgList.Count; I ++ )
            //{
            //    if ((sHumanName).CompareTo((g_DisableSendMsgList[I])) == 0)
            //    {
            //        result = true;
            //        break;
            //    }
            //}
            return result;
        }

        public static bool LoadGameLogItemNameList()
        {
            var result = false;
            var sFileName = M2Share.BasePath + Config.EnvirDir + "GameLogItemNameList.txt";
            var LoadList = new ArrayList();
            //if (File.Exists(sFileName))
            //{
            //    g_GameLogItemNameList.__Lock();
            //    try {
            //        g_GameLogItemNameList.Clear();

            //        LoadList.LoadFromFile(sFileName);
            //        for (I = 0; I < LoadList.Count; I ++ )
            //        {
            //            g_GameLogItemNameList.Add(LoadList[I].Trim());
            //        }
            //    } finally {
            //        g_GameLogItemNameList.UnLock();
            //    }
            //    result = true;
            //}
            //else
            //{
            //    LoadList.SaveToFile(sFileName);
            //}
            //LoadList.Free;
            return result;
        }

        public static byte GetGameLogItemNameList(string sItemName)
        {
            byte result = 0;
            //for (I = 0; I < g_GameLogItemNameList.Count; I ++ )
            //{
            //    if ((sItemName).CompareTo((g_GameLogItemNameList[I])) == 0)
            //    {
            //        result = 1;
            //        break;
            //    }
            //}
            return result;
        }

        public static bool SaveGameLogItemNameList()
        {
            bool result;
            var sFileName = M2Share.BasePath + Config.EnvirDir + "GameLogItemNameList.txt";
            try
            {

                //g_GameLogItemNameList.SaveToFile(sFileName);
            }
            finally
            {
            }
            result = true;
            return result;
        }

        public static bool LoadDenyIPAddrList()
        {
            var result = false;
            var sFileName = M2Share.BasePath + Config.EnvirDir + "DenyIPAddrList.txt";
            //if (File.Exists(sFileName))
            //{
            //    g_DenyIPAddrList.__Lock();
            //    try {
            //        g_DenyIPAddrList.Clear();
            //        LoadList.LoadFromFile(sFileName);
            //        for (I = 0; I < LoadList.Count; I ++ )
            //        {
            //            g_DenyIPAddrList.Add(LoadList[I].Trim());
            //        }
            //    } finally {
            //        g_DenyIPAddrList.UnLock();
            //    }
            //    result = true;
            //}
            //else
            //{
            //    LoadList.SaveToFile(sFileName);
            //}
            //LoadList.Free;
            return result;
        }

        public static bool GetDenyIPAddrList(string sIPaddr)
        {
            bool result = false;
            try
            {
                for (var i = 0; i < g_DenyIPAddrList.Count; i++)
                {
                    //if ((sIPaddr).CompareTo((g_DenyIPAddrList[I])) == 0)
                    //{
                    //    result = true;
                    //    break;
                    //}
                }
            }
            finally
            {
            }
            return result;
        }

        public static bool SaveDenyIPAddrList()
        {
            string sFileName = M2Share.BasePath + Config.EnvirDir + "DenyIPAddrList.txt";
            //SaveList = new StringList();
            //g_DenyIPAddrList.__Lock();
            //try {
            //    for (I = 0; I < g_DenyIPAddrList.Count; I ++ )
            //    {
            //        if (((int)g_DenyIPAddrList.Values[I]) != 0)
            //        {
            //            SaveList.Add(g_DenyIPAddrList[I]);
            //        }
            //    }
            //    SaveList.SaveToFile(sFileName);
            //} finally {
            //    g_DenyIPAddrList.UnLock();
            //}
            //SaveList.Free;
            var result = true;
            return result;
        }

        public static bool LoadDenyChrNameList()
        {
            var result = false;
            string sFileName = M2Share.BasePath + Config.EnvirDir + "DenyChrNameList.txt";
            //if (File.Exists(sFileName))
            //{
            //    g_DenyChrNameList.__Lock();
            //    try {
            //        g_DenyChrNameList.Clear();

            //        LoadList.LoadFromFile(sFileName);
            //        for (I = 0; I < LoadList.Count; I ++ )
            //        {
            //            g_DenyChrNameList.Add(LoadList[I].Trim());
            //        }
            //    } finally {
            //        g_DenyChrNameList.UnLock();
            //    }
            //    result = true;
            //}
            //else
            //{
            //    LoadList.SaveToFile(sFileName);
            //}
            //LoadList.Free;
            return result;
        }

        public static bool GetDenyChrNameList(string sChrName)
        {
            bool result;
            result = false;
            try
            {
                //for (I = 0; I < g_DenyChrNameList.Count; I ++ )
                //{
                //    if ((sChrName).CompareTo((g_DenyChrNameList[I])) == 0)
                //    {
                //        result = true;
                //        break;
                //    }
                //}
            }
            finally
            {
            }
            return result;
        }

        public static bool SaveDenyChrNameList()
        {
            bool result;
            string sFileName;
            sFileName = M2Share.BasePath + Config.EnvirDir + "DenyChrNameList.txt";
            //SaveList = new StringList();
            //g_DenyChrNameList.__Lock();
            //try {
            //    for (I = 0; I < g_DenyChrNameList.Count; I ++ )
            //    {

            //        if (((int)g_DenyChrNameList.Values[I]) != 0)
            //        {
            //            SaveList.Add(g_DenyChrNameList[I]);
            //        }
            //    }

            //    SaveList.SaveToFile(sFileName);
            //} finally {
            //    g_DenyChrNameList.UnLock();
            //}
            //SaveList.Free;
            result = true;
            return result;
        }

        public static bool LoadDenyAccountList()
        {
            var result = false;
            var sFileName = M2Share.BasePath + Config.EnvirDir + "DenyAccountList.txt";
            new ArrayList();
            if (File.Exists(sFileName))
            {
                try
                {
                    g_DenyAccountList.Clear();
                    //LoadList.LoadFromFile(sFileName);
                    //for (I = 0; I < LoadList.Count; I++)
                    //{
                    //    g_DenyAccountList.Add(LoadList[I].Trim());
                    //}
                }
                finally
                {
                }
                result = true;
            }
            else
            {
                //LoadList.SaveToFile(sFileName);
            }
            //LoadList.Free;
            return result;
        }

        public static bool GetDenyAccountList(string sAccount)
        {
            bool result = false;
            try
            {
                for (var I = 0; I < g_DenyAccountList.Count; I++)
                {
                    //if ((sAccount).CompareTo((g_DenyAccountList[I])) == 0)
                    //{
                    //    result = true;
                    //    break;
                    //}
                }
            }
            finally
            {
            }
            return result;
        }

        public static bool SaveDenyAccountList()
        {
            bool result;
            string sFileName;
            sFileName = M2Share.BasePath + Config.EnvirDir + "DenyAccountList.txt";
            //SaveList = new StringList();
            //g_DenyAccountList.__Lock();
            //try {
            //    for (var I = 0; I < g_DenyAccountList.Count; I++)
            //    {
            //        if (((int)g_DenyAccountList.Values[I]) != 0)
            //        {
            //            SaveList.Add(g_DenyAccountList[I]);
            //        }
            //    }
            //    SaveList.SaveToFile(sFileName);
            //} finally {
            //    g_DenyAccountList.UnLock();
            //}
            //SaveList.Free;
            result = true;
            return result;
        }

        public static bool LoadNoClearMonList()
        {
            var result = false;
            var sFileName = Path.Combine(M2Share.BasePath, Config.EnvirDir, "NoClearMonList.txt");
            StringList LoadList = null;
            if (File.Exists(sFileName))
            {
                LoadList = new StringList();
                g_NoClearMonLIst.Clear();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    g_NoClearMonLIst.Add(LoadList[i].Trim());
                }
                result = true;
            }
            if (LoadList != null)
            {
                LoadList.SaveToFile(sFileName);
            }
            return result;
        }

        public static bool GetNoHptoexpMonList(string sMonName)
        {
            bool result;
            result = false;
            try
            {
                //for (var i = 0; i < g_NoHptoexpMonLIst.Count; i++)
                //{
                //    if ((sMonName).CompareTo((g_NoHptoexpMonLIst[i])) == 0)
                //    {
                //        result = true;
                //        break;
                //    }
                //}
            }
            finally
            {
            }
            return result;
        }

        public static bool GetNoClearMonList(string sMonName)
        {
            var result = false;
            //for (var I = 0; I < g_NoClearMonLIst.Count; I++)
            //{
            //    if ((sMonName).CompareTo((g_NoClearMonLIst[I])) == 0)
            //    {
            //        result = true;
            //        break;
            //    }
            //}
            return result;
        }

        public static bool SaveNoHptoexpMonList()
        {
            bool result;
            string sFileName;
            sFileName = M2Share.BasePath + Config.EnvirDir + "NoHptoExpMonList.txt";
            //SaveList = new StringList();
            //g_NoHptoexpMonLIst.__Lock();
            //try {
            //    for (I = 0; I < g_NoHptoexpMonLIst.Count; I++)
            //    {
            //        SaveList.Add(g_NoHptoexpMonLIst[I]);
            //    }
            //    SaveList.SaveToFile(sFileName);
            //} finally {
            //    g_NoHptoexpMonLIst.UnLock();
            //}
            //SaveList.Free;
            result = true;
            return result;
        }

        public static bool SaveNoClearMonList()
        {
            bool result;
            int I;
            string sFileName;
            StringList SaveList;
            sFileName = M2Share.BasePath + Config.EnvirDir + "NoClearMonList.txt";
            SaveList = new StringList();
            try
            {
                for (I = 0; I < g_NoClearMonLIst.Count; I++)
                {
                    //SaveList.Add(g_NoClearMonLIst[I]);
                }
                SaveList.SaveToFile(sFileName);
            }
            finally
            {
            }
            // SaveList.Free;
            result = true;
            return result;
        }

        public static bool LoadMonSayMsg()
        {
            var sStatus = string.Empty;
            var sRate = string.Empty;
            var sColor = string.Empty;
            var sMonName = string.Empty;
            var sSayMsg = string.Empty;
            int nStatus;
            int nRate;
            int nColor;
            StringList LoadList;
            string sLineText;
            TMonSayMsg MonSayMsg;
            var result = false;
            var sFileName = M2Share.BasePath + Config.EnvirDir + "GenMsg.txt";
            if (File.Exists(sFileName))
            {
                g_MonSayMsgList.Clear();
                LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i].Trim();
                    if (sLineText != "" && sLineText[1] < ';')
                    {
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sStatus, new string[] { " ", "/", ",", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sRate, new string[] { " ", "/", ",", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sColor, new string[] { " ", "/", ",", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sMonName, new string[] { " ", "/", ",", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sSayMsg, new string[] { " ", "/", ",", "\t" });
                        if (sStatus != "" && sRate != "" && sColor != "" && sMonName != "" && sSayMsg != "")
                        {
                            nStatus = HUtil32.Str_ToInt(sStatus, -1);
                            nRate = HUtil32.Str_ToInt(sRate, -1);
                            nColor = HUtil32.Str_ToInt(sColor, -1);
                            if (nStatus >= 0 && nRate >= 0 && nColor >= 0)
                            {
                                MonSayMsg = new TMonSayMsg();
                                switch (nStatus)
                                {
                                    case 0:
                                        MonSayMsg.State = MonStatus.KillHuman;
                                        break;
                                    case 1:
                                        MonSayMsg.State = MonStatus.UnderFire;
                                        break;
                                    case 2:
                                        MonSayMsg.State = MonStatus.Die;
                                        break;
                                    case 3:
                                        MonSayMsg.State = MonStatus.MonGen;
                                        break;
                                    default:
                                        MonSayMsg.State = MonStatus.UnderFire;
                                        break;
                                }
                                switch (nColor)
                                {
                                    case 0:
                                        MonSayMsg.Color = MsgColor.Red;
                                        break;
                                    case 1:
                                        MonSayMsg.Color = MsgColor.Green;
                                        break;
                                    case 2:
                                        MonSayMsg.Color = MsgColor.Blue;
                                        break;
                                    case 3:
                                        MonSayMsg.Color = MsgColor.White;
                                        break;
                                    default:
                                        MonSayMsg.Color = MsgColor.White;
                                        break;
                                }
                                MonSayMsg.nRate = nRate;
                                MonSayMsg.sSayMsg = sSayMsg;
                                //for (II = 0; II < g_MonSayMsgList.Count; II ++ )
                                //{
                                //    if ((g_MonSayMsgList[II]).CompareTo((sMonName)) == 0)
                                //    {

                                //        ((g_MonSayMsgList.Values[II]) as ArrayList).Add(MonSayMsg);
                                //        boSearch = true;
                                //        break;
                                //    }
                                //}
                                //if (!boSearch)
                                //{
                                //    MonSayList = new ArrayList();
                                //    MonSayList.Add(MonSayMsg);
                                //    g_MonSayMsgList.Add(sMonName, ((MonSayList) as Object));
                                //}
                            }
                        }
                    }
                }
                //LoadList.Free;
                result = true;
            }
            return result;
        }

        public static void LoadConfig()
        {
            ServerConf.LoadConfig();
            StringConf.LoadString();
            ExpConf.LoadConfig();
            GlobalConf.LoadConfig();
        }

        public static string GetIPLocal(string sIPaddr)
        {
            return "未知!!!";
        }

        // 是否记录物品日志
        // 返回 FALSE 为记录
        // 返回 TRUE  为不记录
        public static bool IsCheapStuff(byte tByte)
        {
            bool result;
            if (tByte < 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        // sIPaddr 为当前IP
        // dIPaddr 为要比较的IP
        // * 号为通配符
        public static bool CompareIPaddr(string sIPaddr, string dIPaddr)
        {
            var result = false;
            if (sIPaddr == "" || dIPaddr == "")
            {
                return result;
            }
            if (dIPaddr[1] == '*')
            {
                result = true;
                return result;
            }
            var nPos = dIPaddr.IndexOf('*');
            if (nPos > 0)
            {
                result = HUtil32.CompareLStr(sIPaddr, dIPaddr, nPos - 1);
            }
            else
            {
                result = sIPaddr.CompareTo(dIPaddr) == 0;
            }
            return result;
        }
    }
}