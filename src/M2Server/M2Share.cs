using M2Server.CommandSystem;
using M2Server.Configs;
using SystemModule;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using SystemModule.Common;

namespace M2Server
{
    public struct TItemBind
    {
        public int nMakeIdex;
        public int nItemIdx;
        public string sBindName;
    }

    public static class M2Share
    {
        public static int nServerIndex = 0;
        public static int ShareFileNameNum = 0;
        public static int g_nServerTickDifference = 0;
        public static ObjectSystem ObjectSystem = null;
        public static ServerConfig ServerConf = null;
        public static GameCmdConfig CommandConf = null;
        public static StringConfig StringConf = null;
        public static ExpsConfig ExpConf = null;
        /// <summary>
        /// 寻路
        /// </summary>
        public static TFindPath g_FindPath;
        /// <summary>
        /// 游戏命令系统
        /// </summary>
        public static CommandManager CommandSystem = null;
        public static LocalDB LocalDB = null;
        public static MirLog LogSystem = null;
        public static RandomNumber RandomNumber = null;
        public static GroupServer GroupServer = null;
        public static DataServer DataServer = null;
        public static ScriptSystem ScriptSystem = null;
        public static GateSystem RunSocket = null;
        public static GateServer GateServer = null;
        public static ArrayList LogStringList = null;
        public static ArrayList LogonCostLogList = null;
        public static TMapManager g_MapManager = null;
        public static ItemUnit ItemUnit = null;
        public static MagicManager MagicManager = null;
        public static TNoticeManager NoticeManager = null;
        public static GuildManager GuildManager = null;
        public static EventManager EventManager = null;
        public static CastleManager CastleManager = null;
        public static TFrontEngine FrontEngine = null;
        public static UserEngine UserEngine = null;
        public static RobotManage RobotManage = null;
        public static Dictionary<string, IList<TMakeItem>> g_MakeItemList = null;
        public static IList<TStartPoint> StartPointList = null;
        public static TStartPoint g_RedStartPoint = null;
        public static TRouteInfo[] ServerTableList = null;
        public static ConcurrentDictionary<string, long> g_DenySayMsgList = null;
        public static Dictionary<string,int> MiniMapList = null;
        public static Dictionary<int, string> g_UnbindList = null;
        /// <summary>
        /// 游戏公告列表
        /// </summary>
        public static StringList LineNoticeList = null;
        public static IList<TQDDinfo> QuestDiaryList = null;
        public static ArrayList AbuseTextList = null;
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
        public static Dictionary<string, TMonDrop> g_MonDropLimitLIst = null;
        public static IList<string> g_DisableTakeOffList = null;
        // 禁止取下物品列表
        public static IList<string> g_ChatLoggingList = null;
        public static IList<TItemBind> g_ItemBindIPaddr = null;
        public static IList<TItemBind> g_ItemBindAccount = null;
        public static IList<TItemBind> g_ItemBindCharName = null;
        public static IList<string> g_UnMasterList = null;
        // 出师记录表
        public static IList<string> g_UnForceMasterList = null;
        // 强行出师记录表
        public static IList<string> g_GameLogItemNameList = null;
        // 游戏日志物品名
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
        public static object LogMsgCriticalSection = null;
        public static object ProcessMsgCriticalSection = null;
        public static object UserDBSection = null;
        public static object ProcessHumanCriticalSection = null;
        public static int g_nTotalHumCount = 0;
        public static bool g_boMission = false;
        public static string g_sMissionMap = string.Empty;
        public static short g_nMissionX = 0;
        public static short g_nMissionY = 0;
        public static bool boStartReady = false;
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
        public static int g_dwHumLimit = 30;
        public static int g_dwMonLimit = 30;
        public static int g_dwZenLimit = 5;
        public static int g_dwNpcLimit = 5;
        public static int g_dwSocLimit = 10;
        public static int g_dwSocCheckTimeOut = 50;
        public static int nDecLimit = 20;
        public const string sConfigFileName = "!Setup.txt";
        public const string sExpConfigFileName = "Exps.ini";
        public const string sCommandFileName = "Command.ini";
        public const string sStringFileName = "String.ini";
        public static int dwRunDBTimeMax = 0;
        public static int g_nGameTime = 0;
        public static TNormNpc g_ManageNPC = null;
        public static TNormNpc g_RobotNPC = null;
        public static TMerchant g_FunctionNPC = null;
        public static IList<TDynamicVar> g_DynamicVarList = null;
        public static int nCurrentMonthly = 0;
        public static int nTotalTimeUsage = 0;
        public static int nLastMonthlyTotalUsage = 0;
        public static int nGrossTotalCnt = 0;
        public static int nGrossResetCnt = 0;
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
        public static TM2Config g_Config = null;
        public static int[] g_dwOldNeedExps = new int[Grobal2.MAXCHANGELEVEL];
        public static TGameCommand g_GameCommand = new TGameCommand();

        public static string sClientSoftVersionError = "游戏版本错误！！！";
        public static string sDownLoadNewClientSoft = "请到网站上下载最新版本游戏客户端软件。";
        public static string sForceDisConnect = "连接被强行中断！！！";
        public static string sClientSoftVersionTooOld = "您现在使用的客户端软件版本太老了，大量的游戏效果新将无法使用。";
        public static string sDownLoadAndUseNewClient = "为了更好的进行游戏，请下载最新的客户端软件！！！";
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
        public static string sSpiritsGone = "召唤烈火结束！！！";
        public static string sMateDoTooweak = "冲撞力不够！！！";
        public static string g_sTheWeaponBroke = "武器破碎！！！";
        public static string sTheWeaponRefineSuccessfull = "升级成功！！！";
        public static string sYouPoisoned = "中毒了！！！";
        public static string sPetRest = "下属：休息";
        public static string sPetAttack = "下属：攻击";
        public static string sWearNotOfWoMan = "非女性用品！！！";
        public static string sWearNotOfMan = "非男性用品！！！";
        public static string sHandWeightNot = "腕力不够！！！";
        public static string sWearWeightNot = "负重力不够！！！";
        public static string g_sItemIsNotThisAccount = "此物品不为此帐号所有！！！";
        public static string g_sItemIsNotThisIPaddr = "此物品不为此IP所有！！！";
        public static string g_sItemIsNotThisCharName = "此物品不为你所有！！！";
        public static string g_sLevelNot = "等级不够！！！";
        public static string g_sJobOrLevelNot = "职业不对或等级不够！！！";
        public static string g_sJobOrDCNot = "职业不对或攻击力不够！！！";
        public static string g_sJobOrMCNot = "职业不对或魔法力不够！！！";
        public static string g_sJobOrSCNot = "职业不对或道术不够！！！";
        public static string g_sDCNot = "攻击力不够！！！";
        public static string g_sMCNot = "魔法力不够！！！";
        public static string g_sSCNot = "道术不够！！！";
        public static string g_sCreditPointNot = "声望点不够！！！";
        public static string g_sReNewLevelNot = "转生等级不够！！！";
        public static string g_sGuildNot = "加入了行会才可以使用此物品！！！";
        public static string g_sGuildMasterNot = "行会掌门才可以使用此物品！！！";
        public static string g_sSabukHumanNot = "沙城成员才可以使用此物品！！！";
        public static string g_sSabukMasterManNot = "沙城城主才可以使用此物品！！！";
        public static string g_sMemberNot = "会员才可以使用此物品！！！";
        public static string g_sMemberTypeNot = "指定类型的会员可以使用此物品！！！";
        public static string g_sCanottWearIt = "此物品不适使用！！！";
        public static string sCanotUseDrugOnThisMap = "此地图不允许使用任何药品！！！";
        public static string sGameMasterMode = "已进入管理员模式";
        public static string sReleaseGameMasterMode = "已退出管理员模式";
        public static string sObserverMode = "已进入隐身模式";
        public static string g_sReleaseObserverMode = "已退出隐身模式";
        public static string sSupermanMode = "已进入无敌模式";
        public static string sReleaseSupermanMode = "已退出无敌模式";
        public static string sYouFoundNothing = "未获取任何物品！！！";
        public static string g_sNoPasswordLockSystemMsg = "游戏密码保护系统还没有启用！！！";
        public static string g_sAlreadySetPasswordMsg = "仓库早已设置了一个密码，如需要修改密码请使用修改密码命令！！！";
        public static string g_sReSetPasswordMsg = "请重复输入一次仓库密码：";
        public static string g_sPasswordOverLongMsg = "输入的密码长度不正确！！！，密码长度必须在 4 - 7 的范围内，请重新设置密码。";
        public static string g_sReSetPasswordOKMsg = "密码设置成功！！，仓库已经自动上锁，请记好您的仓库密码，在取仓库时需要使用此密码开锁。";
        public static string g_sReSetPasswordNotMatchMsg = "二次输入的密码不一致，请重新设置密码！！！";
        public static string g_sPleaseInputUnLockPasswordMsg = "请输入仓库密码：";
        public static string g_sStorageUnLockOKMsg = "密码输入成功！！！，仓库已经开锁。";
        public static string g_sPasswordUnLockOKMsg = "密码输入成功！！！，密码系统已经开锁。";
        public static string g_sStorageAlreadyUnLockMsg = "仓库早已解锁！！！";
        public static string g_sStorageNoPasswordMsg = "仓库还没设置密码！！！";
        public static string g_sUnLockPasswordFailMsg = "密码输入错误！！！，请检查好再输入。";
        public static string g_sLockStorageSuccessMsg = "仓库加锁成功。";
        public static string g_sStoragePasswordClearMsg = "仓库密码已清除！！！";
        public static string g_sPleaseUnloadStoragePasswordMsg = "请先解锁密码再使用此命令清除密码！！！";
        public static string g_sStorageAlreadyLockMsg = "仓库早已加锁了！！！";
        public static string g_sStoragePasswordLockedMsg = "由于密码输入错误超过三次，仓库密码已被锁定！！！";
        public static string g_sSetPasswordMsg = "请输入一个长度为 4 - 7 位的仓库密码: ";
        public static string g_sPleaseInputOldPasswordMsg = "请输入原仓库密码: ";
        public static string g_sOldPasswordIsClearMsg = "密码已清除。";
        public static string g_sPleaseUnLockPasswordMsg = "请先解锁仓库密码后再用此命令清除密码！！！";
        public static string g_sNoPasswordSetMsg = "仓库还没设置密码，请用设置密码命令设置仓库密码！！！";
        public static string g_sOldPasswordIncorrectMsg = "输入的原仓库密码不正确！！！";
        public static string g_sStorageIsLockedMsg = "仓库已被加锁，请先输入仓库正确的开锁密码，再取物品！！！";
        public static string g_sActionIsLockedMsg = "你当前已启用密码保护系统，请先输入正确的密码，才可以正常游戏！！！";
        public static string g_sPasswordNotSetMsg = "对不起，没有设置仓库密码此功能无法使用，设置仓库密码请输入指令 @%s";
        public static string g_sNotPasswordProtectMode = "你正处于非保护模式，如想你的装备更加安全，请输入指令 @%s";
        public static string g_sCanotDropGoldMsg = "太少的金币不允许扔在地上！！！";
        public static string g_sCanotDropInSafeZoneMsg = "安全区不允许扔东西在地上！！！";
        public static string g_sCanotDropItemMsg = "当前无法进行此操作！！！";
        public static string g_sCanotUseItemMsg = "当前无法进行此操作！！！";
        public static string g_sCanotTryDealMsg = "当前无法进行此操作！！！";
        public static string g_sPleaseTryDealLaterMsg = "请稍候再交易！！！";
        public static string g_sDealItemsDenyGetBackMsg = "交易的金币或物品不可以取回，要取回请取消再重新交易！！！";
        public static string g_sDisableDealItemsMsg = "交易功能暂时关闭！！！";
        public static string g_sDealActionCancelMsg = "交易取消！！！";
        public static string g_sPoseDisableDealMsg = "对方禁止进入交易";
        public static string g_sDealSuccessMsg = "交易成功...";
        public static string g_sDealOKTooFast = "过早按了成交按钮。";
        public static string g_sYourBagSizeTooSmall = "你的背包空间不够，无法装下对方交易给你的物品！！！";
        public static string g_sDealHumanBagSizeTooSmall = "交易对方的背包空间不够，无法装下对方交易给你的物品！！！";
        public static string g_sYourGoldLargeThenLimit = "你的所带的金币太多，无法装下对方交易给你的金币！！！";
        public static string g_sDealHumanGoldLargeThenLimit = "交易对方的所带的金币太多，无法装下对方交易给你的金币！！！";
        public static string g_sYouDealOKMsg = "你已经确认交易了。";
        public static string g_sPoseDealOKMsg = "对方已经确认交易了。";
        public static string g_sKickClientUserMsg = "请不要使用非法外挂软件！！！";
        public static string g_sStartMarryManMsg = "[%n]: %s 与 %d 的婚礼现在开始...";
        public static string g_sStartMarryWoManMsg = "[%n]: %d 与 %s 的婚礼现在开始...";
        public static string g_sStartMarryManAskQuestionMsg = "[%n]: %s 你愿意娶 %d 小姐为妻，并照顾她一生一世吗？";
        public static string g_sStartMarryWoManAskQuestionMsg = "[%n]: %d 你愿意娶 %s 小姐为妻，并照顾她一生一世吗？";
        public static string g_sMarryManAnswerQuestionMsg = "[%s]: 我愿意！！！，%d 小姐我会尽我一生的时间来照顾您，让您过上快乐美满的日子的。";
        public static string g_sMarryManAskQuestionMsg = "[%n]: %d 你愿意嫁给 %s 先生为妻，并照顾他一生一世吗？";
        public static string g_sMarryWoManAnswerQuestionMsg = "[%s]: 我愿意！！！，%d 先生我愿意让你来照顾我，保护我。";
        public static string g_sMarryWoManGetMarryMsg = "[%n]: 我宣布 %d 先生与 %s 小姐正式成为合法夫妻。";
        public static string g_sMarryWoManDenyMsg = "[%s]: %d 你这个好色之徒，谁会愿意嫁给你呀！！！，癞蛤蟆想吃天鹅肉。";
        public static string g_sMarryWoManCancelMsg = "[%n]: 真是可惜，二个人这个时候才翻脸，你们培养好感情后再来找我吧！！！";
        public static string g_sfUnMarryManLoginMsg = "你的老婆%d已经强行与你脱离了夫妻关系了！！！";
        public static string g_sfUnMarryWoManLoginMsg = "你的老公%d已经强行与你脱离了夫妻关系了！！！";
        public static string g_sManLoginDearOnlineSelfMsg = "你的老婆%d当前位于%m(%x:%y)。";
        public static string g_sManLoginDearOnlineDearMsg = "你的老公%s在:%m(%x:%y)上线了！！！。";
        public static string g_sWoManLoginDearOnlineSelfMsg = "你的老公当前位于%m(%x:%y)。";
        public static string g_sWoManLoginDearOnlineDearMsg = "你的老婆%s在:%m(%x:%y) 上线了！！！。";
        public static string g_sManLoginDearNotOnlineMsg = "你的老婆现在不在线！！！";
        public static string g_sWoManLoginDearNotOnlineMsg = "你的老公现在不在线！！！";
        public static string g_sManLongOutDearOnlineMsg = "你的老公在:%m(%x:%y)下线了！！！。";
        public static string g_sWoManLongOutDearOnlineMsg = "你的老婆在:%m(%x:%y)下线了！！！。";
        public static string g_sYouAreNotMarryedMsg = "你都没结婚查什么？";
        public static string g_sYourWifeNotOnlineMsg = "你的老婆还没有上线！！！";
        public static string g_sYourHusbandNotOnlineMsg = "你的老公还没有上线！！！";
        public static string g_sYourWifeNowLocateMsg = "你的老婆现在位于:";
        public static string g_sYourHusbandSearchLocateMsg = "你的老公正在找你，他现在位于:";
        public static string g_sYourHusbandNowLocateMsg = "你的老公现在位于:";
        public static string g_sYourWifeSearchLocateMsg = "你的老婆正在找你，他现在位于:";
        public static string g_sfUnMasterLoginMsg = "你的一个徒弟已经背判师门了！！！";
        public static string g_sfUnMasterListLoginMsg = "你的师父%d已经将你逐出师门了！！！";
        public static string g_sMasterListOnlineSelfMsg = "你的师父%d当前位于%m(%x:%y)。";
        public static string g_sMasterListOnlineMasterMsg = "你的徒弟%s在:%m(%x:%y)上线了！！！。";
        public static string g_sMasterOnlineSelfMsg = "你的徒弟当前位于%m(%x:%y)。";
        public static string g_sMasterOnlineMasterListMsg = "你的师父%s在:%m(%x:%y) 上线了！！！。";
        public static string g_sMasterLongOutMasterListOnlineMsg = "你的师父在:%m(%x:%y)下线了！！！。";
        public static string g_sMasterListLongOutMasterOnlineMsg = "你的徒弟%s在:%m(%x:%y)下线了！！！。";
        public static string g_sMasterListNotOnlineMsg = "你的师父现不在线！！！";
        public static string g_sMasterNotOnlineMsg = "你的徒弟现不在线！！！";
        public static string g_sYouAreNotMasterMsg = "你都没师徒关系查什么？";
        public static string g_sYourMasterNotOnlineMsg = "你的师父还没有上线！！！";
        public static string g_sYourMasterListNotOnlineMsg = "你的徒弟还没有上线！！！";
        public static string g_sYourMasterNowLocateMsg = "你的师父现在位于:";
        public static string g_sYourMasterListSearchLocateMsg = "你的徒弟正在找你，他现在位于:";
        public static string g_sYourMasterListNowLocateMsg = "你的徒弟现在位于:";
        public static string g_sYourMasterSearchLocateMsg = "你的师父正在找你，他现在位于:";
        public static string g_sYourMasterListUnMasterOKMsg = "你的徒弟%d已经圆满出师了！！！";
        public static string g_sYouAreUnMasterOKMsg = "你已经出师了！！！";
        public static string g_sUnMasterLoginMsg = "你的一个徒弟已经圆满出师了！！！";
        public static string g_sNPCSayUnMasterOKMsg = "[%n]: 我宣布%d与%s正式脱离师徒关系。";
        public static string g_sNPCSayForceUnMasterMsg = "[%n]: 我宣布%s与%d已经正式脱离师徒关系！！！";
        public static string g_sMyInfo = string.Empty;
        // '『<人物名称>』:  %name 『<当前位置>』:  %map (%x:%y) 『<当前等级>』:  %level 『<金 币 数>』:  %gold 『<PK 点 数>』:  %pk 『<生 命 值>』:  %minhp/%maxhp 『<魔 法 值>』:  %minmp/%maxmp 『<攻 击 力>』:  %mindc/%maxdc 『<魔 法 力>』:  %minmc/%maxmc 『<道 术 力>』:  %minsc/%maxsc 『<登录时间>』:  %logontime 『<在线时长>』:  %logontimeint 分钟';
        public static string g_sSendOnlineCountMsg = "当前在线人数: %c";
        public static string g_sOpenedDealMsg = "开始交易。";
        public static string g_sSendCustMsgCanNotUseNowMsg = "祝福语功能还没有开放！！！";
        public static string g_sSubkMasterMsgCanNotUseNowMsg = "城主发信息功能还没有开放！！！";
        public static string g_sWeaponRepairSuccess = "武器修复成功...";
        public static string g_sDefenceUpTime = "防御力增加%d秒";
        public static string g_sMagDefenceUpTime = "魔法防御力增加%d秒";
        public static string g_sAttPowerUpTime = "物理攻击力增加%d分钟%d秒 ";
        public static string g_sAttPowerDownTime = "物理攻击力减少了%d分钟%d秒";
        public static string g_sWinLottery1Msg = "祝贺您，中了一等奖。";
        public static string g_sWinLottery2Msg = "祝贺您，中了二等奖。";
        public static string g_sWinLottery3Msg = "祝贺您，中了三等奖。";
        public static string g_sWinLottery4Msg = "祝贺您，中了四等奖。";
        public static string g_sWinLottery5Msg = "祝贺您，中了五等奖。";
        public static string g_sWinLottery6Msg = "祝贺您，中了六等奖。";
        public static string g_sNotWinLotteryMsg = "等下次机会吧！！！";
        public static string g_sWeaptonMakeLuck = "武器被加幸运了...";
        public static string g_sWeaptonNotMakeLuck = "无效！！！";
        public static string g_sTheWeaponIsCursed = "你的武器被诅咒了！！！";
        public static string g_sCanotTakeOffItem = "无法取下物品！！！";
        public static string g_sJoinGroup = "%s 已加入小组.";
        public static string g_sTryModeCanotUseStorage = "试玩模式不可以使用仓库功能！！！";
        public static string g_sCanotGetItems = "无法携带更多的东西！！！";
        public static string g_sEnableDearRecall = "允许夫妻传送！！！";
        public static string g_sDisableDearRecall = "禁止夫妻传送！！！";
        public static string g_sEnableMasterRecall = "允许师徒传送！！！";
        public static string g_sDisableMasterRecall = "禁止师徒传送！！！";
        public static string g_sNowCurrDateTime = "当前日期时间: ";
        public static string g_sEnableHearWhisper = "[允许私聊]";
        public static string g_sDisableHearWhisper = "[禁止私聊]";
        public static string g_sEnableShoutMsg = "[允许群聊]";
        public static string g_sDisableShoutMsg = "[禁止群聊]";
        public static string g_sEnableDealMsg = "[允许交易]";
        public static string g_sDisableDealMsg = "[禁止交易]";
        public static string g_sEnableGuildChat = "[允许行会聊天]";
        public static string g_sDisableGuildChat = "[禁止行会聊天]";
        public static string g_sEnableJoinGuild = "[允许加入行会]";
        public static string g_sDisableJoinGuild = "[禁止加入行会]";
        public static string g_sEnableAuthAllyGuild = "[允许行会联盟]";
        public static string g_sDisableAuthAllyGuild = "[禁止行会联盟]";
        public static string g_sEnableGroupRecall = "[允许天地合一]";
        public static string g_sDisableGroupRecall = "[禁止天地合一]";
        public static string g_sEnableGuildRecall = "[允许行会合一]";
        public static string g_sDisableGuildRecall = "[禁止行会合一]";
        public static string g_sPleaseInputPassword = "请输入密码:";
        public static string g_sTheMapDisableMove = "地图%s(%s)不允许传送！！！";
        public static string g_sTheMapNotFound = "%s 此地图号不存在！！！";
        public static string g_sYourIPaddrDenyLogon = "你当前登录的IP地址已被禁止登录了！！！";
        public static string g_sYourAccountDenyLogon = "你当前登录的帐号已被禁止登录了！！！";
        public static string g_sYourCharNameDenyLogon = "你当前登录的人物已被禁止登录了！！！";
        public static string g_sCanotPickUpItem = "在一定时间以内无法捡起此物品！！！";
        public static string g_sQUERYBAGITEMS = "一定时间内不能连续刷新背包物品...";
        public static string g_sCanotSendmsg = "无法发送信息.";
        public static string g_sUserDenyWhisperMsg = " 拒绝私聊！！！";
        public static string g_sUserNotOnLine = "  没有在线！！！";
        public static string g_sRevivalRecoverMsg = "复活戒指生效，体力恢复.";
        public static string g_sClientVersionTooOld = "由于您使用的客户端版本太老了，无法正确显示人物信息！！！";
        public static string g_sCastleGuildName = "(%castlename)%guildname[%rankname]";
        public static string g_sNoCastleGuildName = "%guildname[%rankname]";
        public static string g_sWarrReNewName = "%chrname\\*<圣>*";
        public static string g_sWizardReNewName = "%chrname\\*<神>*";
        public static string g_sTaosReNewName = "%chrname\\*<尊>*";
        public static string g_sRankLevelName = "{0}\\平民";
        public static string g_sManDearName = "%s的老公";
        public static string g_sWoManDearName = "%s的老婆";
        public static string g_sMasterName = "%s的师父";
        public static string g_sNoMasterName = "%s的徒弟";
        public static string g_sHumanShowName = "%chrname\\%guildname\\%dearname\\%mastername";
        public static string g_sChangePermissionMsg = "当前权限等级为:%d";
        public static string g_sChangeKillMonExpRateMsg = "经验倍数:%g 时长%d秒";
        public static string g_sChangePowerRateMsg = "攻击力倍数:%g 时长%d秒";
        public static string g_sChangeMemberLevelMsg = "当前会员等级为:%d";
        public static string g_sChangeMemberTypeMsg = "当前会员类型为:%d";
        public static string g_sScriptChangeHumanHPMsg = "当前HP值为:%d";
        public static string g_sScriptChangeHumanMPMsg = "当前MP值为:%d";
        public static string g_sScriptGuildAuraePointNoGuild = "你还没加入行会！！！";
        public static string g_sScriptGuildAuraePointMsg = "你的行会人气度为:%d";
        public static string g_sScriptGuildBuildPointNoGuild = "你还没加入行会！！！";
        public static string g_sScriptGuildBuildPointMsg = "你的行会的建筑度为:%d";
        public static string g_sScriptGuildFlourishPointNoGuild = "你还没加入行会！！！";
        public static string g_sScriptGuildFlourishPointMsg = "你的行会的繁荣度为:%d";
        public static string g_sScriptGuildStabilityPointNoGuild = "你的行会的建筑度为:%d";
        public static string g_sScriptGuildStabilityPointMsg = "你的行会的安定度为:%d";
        public static string g_sScriptChiefItemCountMsg = "你的行会的超级装备数为:%d";
        public static string g_sDisableSayMsg = "[由于你重复发相同的内容，%d分钟内你将被禁止发言...]";
        public static string g_sOnlineCountMsg = "在线数: %d";
        public static string g_sTotalOnlineCountMsg = "总在线数: %d";
        public static string g_sYouNeedLevelMsg = "你的等级要在%d级以上才能用此功能！！！";
        public static string g_sThisMapDisableSendCyCyMsg = "本地图不允许喊话！！！";
        public static string g_sYouCanSendCyCyLaterMsg = "%d秒后才可以再发文字！！！";
        public static string g_sYouIsDisableSendMsg = "禁止聊天！！！";
        public static string g_sYouMurderedMsg = "你犯了谋杀罪！！！";
        public static string g_sYouKilledByMsg = "你被%s杀害了！！！";
        public static string g_sYouProtectedByLawOfDefense = "[你受到正当规则保护。]";
        public static string g_sYourUseItemIsNul = "你的%s处没有放上装备！！！";
        // ===============================================================
        public static int nIPLocal = -1;
        public static int nAddGameDataLog = -1;

        public const string g_sProductName = "程序名称：SKY防攻击服务器引擎";
        public const string g_sVersion = "引擎版本: 1.00 Build 20161001";
        public const string g_sUpDateTime = "更新日期: 2016/10/01";
        public const string g_sProgram = "程序制作: 仙侣情缘";
        public const string g_sWebSite = "程序网站: http://www.jsym2.com";
        public const string g_sBbsSite = "程序论坛: http://bbs.jsym2.com";

        public const int MAXUPLEVEL = 500;
        public const int MAXHUMPOWER = 1000;
        public const int BODYLUCKUNIT = 5000;
        // 10?
        public const int HAM_ALL = 0;
        public const int HAM_PEACE = 1;
        public const int HAM_DEAR = 2;
        public const int HAM_MASTER = 3;
        public const int HAM_GROUP = 4;
        public const int HAM_GUILD = 5;
        /// <summary>
        /// 红名攻击模式
        /// </summary>
        public const int HAM_PKATTACK = 6;
        public const int DEFHIT = 5;
        public const int DEFSPEED = 15;
        public const int jWarr = 0;
        public const int jWizard = 1;
        public const int jTaos = 2;
        public const int SIZEOFTHUMAN = 3588;
        public const int MONSTER_SANDMOB = 3;
        public const int MONSTER_ROCKMAN = 4;
        public const int MONSTER_RON = 9;
        public const int MONSTER_MINORNUMA = 18;
        public const int ARCHER_POLICE = 20;
        public const int SUPREGUARD = 11;
        public const int PETSUPREGUARD = 12;
        public const int ANIMAL_CHICKEN = 51;
        public const int ANIMAL_DEER = 52;
        public const int ANIMAL_WOLF = 53;
        public const int TRAINER = 55;
        public const int MONSTER_OMA = 80;
        public const int MONSTER_OMAKNIGHT = 81;
        public const int MONSTER_SPITSPIDER = 82;
        public const int MONSTER_STICK = 85;
        public const int MONSTER_DUALAXE = 87;
        public const int MONSTER_THONEDARK = 93;
        public const int MONSTER_LIGHTZOMBI = 94;
        public const int MONSTER_DIGOUTZOMBI = 95;
        public const int MONSTER_ZILKINZOMBI = 96;
        public const int MONSTER_WHITESKELETON = 100;
        public const int MONSTER_BEEQUEEN = 103;
        public const int MONSTER_BEE = 125;
        public const int MONSTER_MAGUNGSA = 143;
        public const int MONSTER_SCULTURE = 101;
        public const int MONSTER_SCULTUREKING = 102;
        public const int MONSTER_ARCHERGUARD = 112;
        public const int MONSTER_ELFMONSTER = 113;
        public const int MONSTER_ELFWARRIOR = 114;
        public const string sMAN = "MAN";
        public const string sSUNRAISE = "SUNRAISE";
        public const string sDAY = "DAY";
        public const string sSUNSET = "SUNSET";
        public const string sNIGHT = "NIGHT";
        public const string sWarrior = "Warrior";
        public const string sWizard = "Wizard";
        public const string sTaos = "Taoist";
        public const string sSUN = "SUN";
        public const string sMON = "MON";
        public const string sTUE = "TUE";
        public const string sWED = "WED";
        public const string sTHU = "THU";
        public const string sFRI = "FRI";
        public const string sSAT = "SAT";
        // 脚本常量
        public const string sCHECK = "CHECK";
        public const int nCHECK = 1;
        public const string sRANDOM = "RANDOM";
        public const int nRANDOM = 2;
        public const string sGENDER = "GENDER";
        public const int nGENDER = 3;
        public const string sDAYTIME = "DAYTIME";
        public const int nDAYTIME = 4;
        public const string sCHECKOPEN = "CHECKOPEN";
        public const int nCHECKOPEN = 5;
        public const string sCHECKUNIT = "CHECKUNIT";
        public const int nCHECKUNIT = 6;
        public const string sCHECKLEVEL = "CHECKLEVEL";
        public const int nCHECKLEVEL = 7;
        public const string sCHECKJOB = "CHECKJOB";
        public const int nCHECKJOB = 8;
        public const string sCHECKBBCOUNT = "CHECKBBCOUNT";
        public const int nCHECKBBCOUNT = 9;
        public const string sCHECKITEM = "CHECKITEM";
        public const int nCHECKITEM = 20;
        public const string sCHECKITEMW = "CHECKITEMW";
        public const int nCHECKITEMW = 21;
        public const string sCHECKGOLD = "CHECKGOLD";
        public const int nCHECKGOLD = 22;
        public const string sISTAKEITEM = "ISTAKEITEM";
        public const int nISTAKEITEM = 23;
        public const string sCHECKDURA = "CHECKDURA";
        public const int nCHECKDURA = 24;
        public const string sCHECKDURAEVA = "CHECKDURAEVA";
        public const int nCHECKDURAEVA = 25;
        public const string sDAYOFWEEK = "DAYOFWEEK";
        public const int nDAYOFWEEK = 26;
        public const string sHOUR = "HOUR";
        public const int nHOUR = 27;
        public const string sMIN = "MIN";
        public const int nMIN = 28;
        public const string sCHECKPKPOINT = "CHECKPKPOINT";
        public const int nCHECKPKPOINT = 29;
        public const string sCHECKLUCKYPOINT = "CHECKLUCKYPOINT";
        public const int nCHECKLUCKYPOINT = 30;
        public const string sCHECKMONMAP = "CHECKMONMAP";
        public const int nCHECKMONMAP = 31;
        public const string sCHECKMONAREA = "CHECKMONAREA";
        public const int nCHECKMONAREA = 32;
        public const string sCHECKHUM = "CHECKHUM";
        public const int nCHECKHUM = 33;
        public const string sCHECKBAGGAGE = "CHECKBAGGAGE";
        public const int nCHECKBAGGAGE = 34;
        public const string sEQUAL = "EQUAL";
        public const int nEQUAL = 35;
        public const string sLARGE = "LARGE";
        public const int nLARGE = 36;
        public const string sSMALL = "SMALL";
        public const int nSMALL = 37;
        public const string sSC_CHECKMAGIC = "CHECKMAGIC";
        public const int nSC_CHECKMAGIC = 38;
        public const string sSC_CHKMAGICLEVEL = "CHKMAGICLEVEL";
        public const int nSC_CHKMAGICLEVEL = 39;
        public const string sSC_CHECKMONRECALL = "CHECKMONRECALL";
        public const int nSC_CHECKMONRECALL = 40;
        public const string sSC_CHECKHORSE = "CHECKHORSE";
        public const int nSC_CHECKHORSE = 41;
        public const string sSC_CHECKRIDING = "CHECKRIDING";
        public const int nSC_CHECKRIDING = 42;
        public const string sSC_STARTDAILYQUEST = "STARTDAILYQUEST";
        public const int nSC_STARTDAILYQUEST = 45;
        public const string sSC_CHECKDAILYQUEST = "CHECKDAILYQUEST";
        public const int nSC_CHECKDAILYQUEST = 46;
        public const string sSC_RANDOMEX = "RANDOMEX";
        public const int nSC_RANDOMEX = 47;
        public const string sCHECKNAMELIST = "CHECKNAMELIST";
        public const int nCHECKNAMELIST = 48;
        public const string sSC_CHECKWEAPONLEVEL = "CHECKWEAPONLEVEL";
        public const int nSC_CHECKWEAPONLEVEL = 49;
        public const string sSC_CHECKWEAPONATOM = "CHECKWEAPONATOM";
        public const int nSC_CHECKWEAPONATOM = 50;
        public const string sSC_CHECKREFINEWEAPON = "CHECKREFINEWEAPON";
        public const int nSC_CHECKREFINEWEAPON = 51;
        public const string sSC_CHECKWEAPONMCTYPE = "CHECKWEAPONMCTYPE";
        public const int nSC_CHECKWEAPONMCTYPE = 52;
        public const string sSC_CHECKREFINEITEM = "CHECKREFINEITEM";
        public const int nSC_CHECKREFINEITEM = 53;
        public const string sSC_HASWEAPONATOM = "HASWEAPONATOM";
        public const int nSC_HASWEAPONATOM = 54;
        public const string sSC_ISGUILDMASTER = "ISGUILDMASTER";
        public const int nSC_ISGUILDMASTER = 55;
        public const string sSC_CANPROPOSECASTLEWAR = "CANPROPOSECASTLEWAR";
        public const int nSC_CANPROPOSECASTLEWAR = 56;
        public const string sSC_CANHAVESHOOTER = "CANHAVESHOOTER";
        public const int nSC_CANHAVESHOOTER = 57;
        public const string sSC_CHECKFAME = "CHECKFAME";
        public const int nSC_CHECKFAME = 58;
        public const string sSC_ISONCASTLEWAR = "ISONCASTLEWAR";
        public const int nSC_ISONCASTLEWAR = 59;
        public const string sSC_ISONREADYCASTLEWAR = "ISONREADYCASTLEWAR";
        public const int nSC_ISONREADYCASTLEWAR = 60;
        public const string sSC_ISCASTLEGUILD = "ISCASTLEGUILD";
        public const int nSC_ISCASTLEGUILD = 61;
        public const string sSC_ISATTACKGUILD = "ISATTACKGUILD";
        // 是否为攻城方
        public const int nSC_ISATTACKGUILD = 63;
        public const string sSC_ISDEFENSEGUILD = "ISDEFENSEGUILD";
        // 是否为守城方
        public const int nSC_ISDEFENSEGUILD = 65;
        public const string sSC_CHECKSHOOTER = "CHECKSHOOTER";
        public const int nSC_CHECKSHOOTER = 66;
        public const string sSC_CHECKSAVEDSHOOTER = "CHECKSAVEDSHOOTER";
        public const int nSC_CHECKSAVEDSHOOTER = 67;
        public const string sSC_HASGUILD = "HAVEGUILD";
        // 是否加入行会
        public const int nSC_HASGUILD = 68;
        public const string sSC_CHECKCASTLEDOOR = "CHECKCASTLEDOOR";
        // 检查城门
        public const int nSC_CHECKCASTLEDOOR = 69;
        public const string sSC_CHECKCASTLEDOOROPEN = "CHECKCASTLEDOOROPEN";
        // 城门是否打开
        public const int nSC_CHECKCASTLEDOOROPEN = 70;
        public const string sSC_CHECKPOS = "CHECKPOS";
        public const int nSC_CHECKPOS = 71;
        public const string sSC_CANCHARGESHOOTER = "CANCHARGESHOOTER";
        public const int nSC_CANCHARGESHOOTER = 72;
        public const string sSC_ISATTACKALLYGUILD = "ISATTACKALLYGUILD";
        // 是否为攻城方联盟行会
        public const int nSC_ISATTACKALLYGUILD = 73;
        public const string sSC_ISDEFENSEALLYGUILD = "ISDEFENSEALLYGUILD";
        // 是否为守城方联盟行会
        public const int nSC_ISDEFENSEALLYGUILD = 74;
        public const string sSC_TESTTEAM = "TESTTEAM";
        public const int nSC_TESTTEAM = 75;
        public const string sSC_ISSYSOP = "ISSYSOP";
        public const int nSC_ISSYSOP = 76;
        public const string sSC_ISADMIN = "ISADMIN";
        public const int nSC_ISADMIN = 77;
        public const string sSC_CHECKBONUS = "CHECKBONUS";
        public const int nSC_CHECKBONUS = 78;
        public const string sSC_CHECKMARRIAGE = "CHECKMARRIAGE";
        public const int nSC_CHECKMARRIAGE = 79;
        public const string sSC_CHECKMARRIAGERING = "CHECKMARRIAGERING";
        public const int nSC_CHECKMARRIAGERING = 80;
        public const string sSC_CHECKGMETERM = "CHECKGMETERM";
        public const int nSC_CHECKGMETERM = 100;
        public const string sSC_CHECKOPENGME = "CHECKOPENGME";
        public const int nSC_CHECKOPENGME = 101;
        public const string sSC_CHECKENTERGMEMAP = "CHECKENTERGMEMAP";
        public const int nSC_CHECKENTERGMEMAP = 102;
        public const string sSC_CHECKSERVER = "CHECKSERVER";
        public const int nSC_CHECKSERVER = 103;
        public const string sSC_ELARGE = "ELARGE";
        public const int nSC_ELARGE = 104;
        public const string sSC_ESMALL = "ESMALL";
        public const int nSC_ESMALL = 105;
        public const string sSC_CHECKGROUPCOUNT = "CHECKGROUPCOUNT";
        public const int nSC_CHECKGROUPCOUNT = 106;
        public const string sSC_CHECKACCESSORY = "CHECKACCESSORY";
        public const int nSC_CHECKACCESSORY = 107;
        public const string sSC_ONERROR = "ONERROR";
        public const int nSC_ONERROR = 108;
        public const string sSC_CHECKARMOR = "CHECKARMOR";
        public const int nSC_CHECKARMOR = 109;
        public const string sCHECKACCOUNTLIST = "CHECKACCOUNTLIST";
        public const int nCHECKACCOUNTLIST = 135;
        public const string sCHECKIPLIST = "CHECKIPLIST";
        public const int nCHECKIPLIST = 136;
        public const string sCHECKCREDITPOINT = "CHECKCREDITPOINT";
        public const int nCHECKCREDITPOINT = 137;
        public const string sSC_CHECKPOSEDIR = "CHECKPOSEDIR";
        public const int nSC_CHECKPOSEDIR = 138;
        public const string sSC_CHECKPOSELEVEL = "CHECKPOSELEVEL";
        public const int nSC_CHECKPOSELEVEL = 139;
        public const string sSC_CHECKPOSEGENDER = "CHECKPOSEGENDER";
        public const int nSC_CHECKPOSEGENDER = 140;
        public const string sSC_CHECKLEVELEX = "CHECKLEVELEX";
        public const int nSC_CHECKLEVELEX = 141;
        public const string sSC_CHECKBONUSPOINT = "CHECKBONUSPOINT";
        public const int nSC_CHECKBONUSPOINT = 142;
        public const string sSC_CHECKMARRY = "CHECKMARRY";
        public const int nSC_CHECKMARRY = 143;
        public const string sSC_CHECKPOSEMARRY = "CHECKPOSEMARRY";
        public const int nSC_CHECKPOSEMARRY = 144;
        public const string sSC_CHECKMARRYCOUNT = "CHECKMARRYCOUNT";
        public const int nSC_CHECKMARRYCOUNT = 145;
        public const string sSC_CHECKMASTER = "CHECKMASTER";
        public const int nSC_CHECKMASTER = 146;
        public const string sSC_HAVEMASTER = "HAVEMASTER";
        public const int nSC_HAVEMASTER = 147;
        public const string sSC_CHECKPOSEMASTER = "CHECKPOSEMASTER";
        public const int nSC_CHECKPOSEMASTER = 148;
        public const string sSC_POSEHAVEMASTER = "POSEHAVEMASTER";
        public const int nSC_POSEHAVEMASTER = 149;
        public const string sSC_CHECKISMASTER = "CHECKPOSEISMASTER";
        public const int nSC_CHECKISMASTER = 150;
        public const string sSC_CHECKPOSEISMASTER = "CHECKISMASTER";
        public const int nSC_CHECKPOSEISMASTER = 151;
        public const string sSC_CHECKNAMEIPLIST = "CHECKNAMEIPLIST";
        public const int nSC_CHECKNAMEIPLIST = 152;
        public const string sSC_CHECKACCOUNTIPLIST = "CHECKACCOUNTIPLIST";
        public const int nSC_CHECKACCOUNTIPLIST = 153;
        public const string sSC_CHECKSLAVECOUNT = "CHECKSLAVECOUNT";
        public const int nSC_CHECKSLAVECOUNT = 154;
        public const string sSC_CHECKCASTLEMASTER = "ISCASTLEMASTER";
        public const int nSC_CHECKCASTLEMASTER = 155;
        public const string sSC_ISNEWHUMAN = "ISNEWHUMAN";
        public const int nSC_ISNEWHUMAN = 156;
        public const string sSC_CHECKMEMBERTYPE = "CHECKMEMBERTYPE";
        public const int nSC_CHECKMEMBERTYPE = 157;
        public const string sSC_CHECKMEMBERLEVEL = "CHECKMEMBERLEVEL";
        public const int nSC_CHECKMEMBERLEVEL = 158;
        public const string sSC_CHECKGAMEGOLD = "CHECKGAMEGOLD";
        public const int nSC_CHECKGAMEGOLD = 159;
        public const string sSC_CHECKGAMEPOINT = "CHECKGAMEPOINT";
        public const int nSC_CHECKGAMEPOINT = 160;
        public const string sSC_CHECKNAMELISTPOSITION = "CHECKNAMELISTPOSITION";
        public const int nSC_CHECKNAMELISTPOSITION = 161;
        public const string sSC_CHECKGUILDLIST = "CHECKGUILDLIST";
        public const int nSC_CHECKGUILDLIST = 162;
        public const string sSC_CHECKRENEWLEVEL = "CHECKRENEWLEVEL";
        public const int nSC_CHECKRENEWLEVEL = 163;
        public const string sSC_CHECKSLAVELEVEL = "CHECKSLAVELEVEL";
        public const int nSC_CHECKSLAVELEVEL = 164;
        public const string sSC_CHECKSLAVENAME = "CHECKSLAVENAME";
        public const int nSC_CHECKSLAVENAME = 165;
        public const string sSC_CHECKCREDITPOINT = "CHECKCREDITPOINT";
        public const int nSC_CHECKCREDITPOINT = 166;
        public const string sSC_CHECKOFGUILD = "CHECKOFGUILD";
        public const int nSC_CHECKOFGUILD = 167;
        public const string sSC_CHECKPAYMENT = "CHECKPAYMENT";
        public const int nSC_CHECKPAYMENT = 168;
        public const string sSC_CHECKUSEITEM = "CHECKUSEITEM";
        public const int nSC_CHECKUSEITEM = 169;
        public const string sSC_CHECKBAGSIZE = "CHECKBAGSIZE";
        public const int nSC_CHECKBAGSIZE = 170;
        public const string sSC_CHECKLISTCOUNT = "CHECKLISTCOUNT";
        public const int nSC_CHECKLISTCOUNT = 171;
        public const string sSC_CHECKDC = "CHECKDC";
        public const int nSC_CHECKDC = 172;
        public const string sSC_CHECKMC = "CHECKMC";
        public const int nSC_CHECKMC = 173;
        public const string sSC_CHECKSC = "CHECKSC";
        public const int nSC_CHECKSC = 174;
        public const string sSC_CHECKHP = "CHECKHP";
        public const int nSC_CHECKHP = 175;
        public const string sSC_CHECKMP = "CHECKMP";
        public const int nSC_CHECKMP = 176;
        public const string sSC_CHECKITEMTYPE = "CHECKITEMTYPE";
        public const int nSC_CHECKITEMTYPE = 180;
        public const string sSC_CHECKEXP = "CHECKEXP";
        public const int nSC_CHECKEXP = 181;
        public const string sSC_CHECKCASTLEGOLD = "CHECKCASTLEGOLD";
        public const int nSC_CHECKCASTLEGOLD = 182;
        public const string sSC_PASSWORDERRORCOUNT = "PASSWORDERRORCOUNT";
        public const int nSC_PASSWORDERRORCOUNT = 183;
        public const string sSC_ISLOCKPASSWORD = "ISLOCKPASSWORD";
        public const int nSC_ISLOCKPASSWORD = 184;
        public const string sSC_ISLOCKSTORAGE = "ISLOCKSTORAGE";
        public const int nSC_ISLOCKSTORAGE = 185;
        public const string sSC_CHECKBUILDPOINT = "CHECKGUILDBUILDPOINT";
        public const int nSC_CHECKBUILDPOINT = 186;
        public const string sSC_CHECKAURAEPOINT = "CHECKGUILDAURAEPOINT";
        public const int nSC_CHECKAURAEPOINT = 187;
        public const string sSC_CHECKSTABILITYPOINT = "CHECKGUILDSTABILITYPOINT";
        public const int nSC_CHECKSTABILITYPOINT = 188;
        public const string sSC_CHECKFLOURISHPOINT = "CHECKGUILDFLOURISHPOINT";
        public const int nSC_CHECKFLOURISHPOINT = 189;
        public const string sSC_CHECKCONTRIBUTION = "CHECKCONTRIBUTION";
        // 贡献度
        public const int nSC_CHECKCONTRIBUTION = 190;
        public const string sSC_CHECKRANGEMONCOUNT = "CHECKRANGEMONCOUNT";
        // 检查一个区域中有多少怪
        public const int nSC_CHECKRANGEMONCOUNT = 191;
        public const string sSC_CHECKITEMADDVALUE = "CHECKITEMADDVALUE";
        public const int nSC_CHECKITEMADDVALUE = 192;
        public const string sSC_CHECKINMAPRANGE = "CHECKINMAPRANGE";
        public const int nSC_CHECKINMAPRANGE = 193;
        public const string sSC_CASTLECHANGEDAY = "CASTLECHANGEDAY";
        public const int nSC_CASTLECHANGEDAY = 194;
        public const string sSC_CASTLEWARDAY = "CASTLEWARAY";
        public const int nSC_CASTLEWARDAY = 195;
        public const string sSC_ONLINELONGMIN = "ONLINELONGMIN";
        public const int nSC_ONLINELONGMIN = 196;
        public const string sSC_CHECKGUILDCHIEFITEMCOUNT = "CHECKGUILDCHIEFITEMCOUNT";
        public const int nSC_CHECKGUILDCHIEFITEMCOUNT = 197;
        public const string sSC_CHECKNAMEDATELIST = "CHECKNAMEDATELIST";
        public const int nSC_CHECKNAMEDATELIST = 198;
        public const string sSC_CHECKMAPHUMANCOUNT = "CHECKMAPHUMANCOUNT";
        public const int nSC_CHECKMAPHUMANCOUNT = 199;
        public const string sSC_CHECKMAPMONCOUNT = "CHECKMAPMONCOUNT";
        public const int nSC_CHECKMAPMONCOUNT = 200;
        public const string sSC_CHECKVAR = "CHECKVAR";
        public const int nSC_CHECKVAR = 201;
        public const string sSC_CHECKSERVERNAME = "CHECKSERVERNAME";
        public const int nSC_CHECKSERVERNAME = 202;
        public const string sSC_CHECKMAP = "CHECKMAP";
        public const int nSC_CHECKMAP = 203;
        public const string sSC_REVIVESLAVE = "REVIVESLAVES";
        public const int nSC_REVIVESLAVE = 206;
        public const string sSC_CHECKMAGICLVL = "CHECKMAGICLVL";
        public const int nSC_CHECKMAGICLVL = 207;
        public const string sSC_CHECKGROUPCLASS = "CHECKGROUPCLASS";
        public const int nSC_CHECKGROUPCLASS = 208;
        // ==================================================================
        public const string sCheckDiemon = "CHECKDIEMON";
        // 检查人物死亡被指定怪物杀死
        public const int nCheckDiemon = 209;
        public const string scheckkillplaymon = "CHECKKILLPLAYMON";
        // 检查杀死怪物
        public const int ncheckkillplaymon = 210;
        public const string sSC_CHECKRANDOMNO = "CHECKRANDOMNO";
        // 检测输入的验证码是否正确
        public const int nSC_CHECKRANDOMNO = 212;
        public const string sSC_CHECKISONMAP = "ISONMAP";
        // 检测当前人是否在MAP地图上
        public const int nSC_CHECKISONMAP = 213;
        public const string sSC_KILLBYHUM = "KILLBYHUM";
        // 是否被人杀
        public const int nSC_KILLBYHUM = 214;
        public const string sSC_KILLBYMON = "KILLBYMON";
        // 是否被怪杀
        public const int nSC_KILLBYMON = 215;
        public const string sSC_CHECKINSAFEZONE = "INSAFEZONE";
        // 检测人物是否在安全区
        public const int nSC_CHECKINSAFEZONE = 216;
        public const string sSC_ISGROUPMASTER = "ISGROUPMASTER";
        // 检测是否组长
        public const int nSC_ISGROUPMASTER = 217;
        // Action
        public const string sSET = "SET";
        public const int nSET = 1;
        public const string sTAKE = "TAKE";
        public const int nTAKE = 2;
        public const string sSC_GIVE = "GIVE";
        public const int nSC_GIVE = 3;
        public const string sTAKEW = "TAKEW";
        public const int nTAKEW = 4;
        public const string sCLOSE = "CLOSE";
        public const int nCLOSE = 5;
        public const string sRESET = "RESET";
        public const int nRESET = 6;
        public const string sSETOPEN = "SETOPEN";
        public const int nSETOPEN = 7;
        public const string sSETUNIT = "SETUNIT";
        public const int nSETUNIT = 8;
        public const string sRESETUNIT = "RESETUNIT";
        public const int nRESETUNIT = 9;
        public const string sBREAK = "BREAK";
        public const int nBREAK = 10;
        public const string sTIMERECALL = "TIMERECALL";
        public const int nTIMERECALL = 11;
        public const string sSC_PARAM1 = "PARAM1";
        public const int nSC_PARAM1 = 12;
        public const string sSC_PARAM2 = "PARAM2";
        public const int nSC_PARAM2 = 13;
        public const string sSC_PARAM3 = "PARAM3";
        public const int nSC_PARAM3 = 14;
        public const string sSC_PARAM4 = "PARAM4";
        public const int nSC_PARAM4 = 15;
        public const string sSC_EXEACTION = "EXEACTION";
        public const int nSC_EXEACTION = 16;
        public const string sMAPMOVE = "MAPMOVE";
        public const int nMAPMOVE = 19;
        public const string sMAP = "MAP";
        public const int nMAP = 20;
        public const string sTAKECHECKITEM = "TAKECHECKITEM";
        public const int nTAKECHECKITEM = 21;
        public const string sMONGEN = "MONGEN";
        public const int nMONGEN = 22;
        public const string sSC_MONGENP = "MONGENP";
        public const int nSC_MONGENP = 23;
        public const string sMONCLEAR = "MONCLEAR";
        public const int nMONCLEAR = 24;
        public const string sMOV = "MOV";
        public const int nMOV = 25;
        public const string sINC = "INC";
        public const int nINC = 26;
        public const string sDEC = "DEC";
        public const int nDEC = 27;
        public const string sSUM = "SUM";
        public const int nSUM = 28;
        public const string sSC_DIV = "DIV";
        public const string sSC_MUL = "MUL";
        public const string sSC_PERCENT = "PERCENT";
        public const string sBREAKTIMERECALL = "BREAKTIMERECALL";
        public const int nBREAKTIMERECALL = 29;
        public const string sSENDMSG = "SENDMSG";
        public const int nSENDMSG = 30;
        public const string sCHANGEMODE = "CHANGEMODE";
        public const int nCHANGEMODE = 31;
        public const string sPKPOINT = "PKPOINT";
        public const int nPKPOINT = 32;
        public const string sCHANGEXP = "CHANGEXP";
        public const int nCHANGEXP = 33;
        public const string sSC_RECALLMOB = "RECALLMOB";
        public const int nSC_RECALLMOB = 34;
        public const string sKICK = "KICK";
        public const int nKICK = 35;
        public const string sMOVR = "MOVR";
        public const int nMOVR = 50;
        public const string sEXCHANGEMAP = "EXCHANGEMAP";
        public const int nEXCHANGEMAP = 51;
        public const string sRECALLMAP = "RECALLMAP";
        public const int nRECALLMAP = 52;
        public const string sADDBATCH = "ADDBATCH";
        public const int nADDBATCH = 53;
        public const string sBATCHDELAY = "BATCHDELAY";
        public const int nBATCHDELAY = 54;
        public const string sBATCHMOVE = "BATCHMOVE";
        public const int nBATCHMOVE = 55;
        public const string sPLAYDICE = "PLAYDICE";
        public const int nPLAYDICE = 56;
        public const string sSC_PASTEMAP = "PASTEMAP";
        public const string sSC_LOADGEN = "LOADGEN";
        public const string sADDNAMELIST = "ADDNAMELIST";
        public const int nADDNAMELIST = 57;
        public const string sDELNAMELIST = "DELNAMELIST";
        public const int nDELNAMELIST = 58;
        public const string sADDGUILDLIST = "ADDGUILDLIST";
        public const int nADDGUILDLIST = 59;
        public const string sDELGUILDLIST = "DELGUILDLIST";
        public const int nDELGUILDLIST = 60;
        public const string sADDACCOUNTLIST = "ADDACCOUNTLIST";
        public const int nADDACCOUNTLIST = 61;
        public const string sDELACCOUNTLIST = "DELACCOUNTLIST";
        public const int nDELACCOUNTLIST = 62;
        public const string sADDIPLIST = "ADDIPLIST";
        public const int nADDIPLIST = 63;
        public const string sDELIPLIST = "DELIPLIST";
        public const int nDELIPLIST = 64;
        public const string sGOQUEST = "GOQUEST";
        public const int nGOQUEST = 100;
        public const string sENDQUEST = "ENDQUEST";
        public const int nENDQUEST = 101;
        public const string sGOTO = "GOTO";
        public const int nGOTO = 102;
        public const string sSC_HAIRCOLOR = "HAIRCOLOR";
        public const int nSC_HAIRCOLOR = 104;
        public const string sSC_WEARCOLOR = "WEARCOLOR";
        public const int nSC_WEARCOLOR = 105;
        public const string sSC_HAIRSTYLE = "HAIRSTYLE";
        public const int nSC_HAIRSTYLE = 106;
        public const string sSC_MONRECALL = "MONRECALL";
        public const int nSC_MONRECALL = 107;
        public const string sSC_HORSECALL = "HORSECALL";
        public const int nSC_HORSECALL = 108;
        public const string sSC_HAIRRNDCOL = "HAIRRNDCOL";
        public const int nSC_HAIRRNDCOL = 109;
        public const string sSC_RANDSETDAILYQUEST = "RANDSETDAILYQUEST";
        public const int nSC_RANDSETDAILYQUEST = 110;
        public const string sSC_REFINEWEAPON = "REFINEWEAPON";
        public const int nSC_REFINEWEAPON = 113;
        public const string sSC_RECALLGROUPMEMBERS = "RECALLGROUPMEMBERS";
        public const int nSC_RECALLGROUPMEMBERS = 117;
        public const string sSC_MAPTING = "MAPTING";
        public const int nSC_MAPTING = 118;
        public const string sSC_WRITEWEAPONNAME = "WRITEWEAPONNAME";
        public const int nSC_WRITEWEAPONNAME = 119;
        public const string sSC_DELAYGOTO = "DELAYGOTO";
        public const int nSC_DELAYGOTO = 120;
        public const string sSC_ENABLECMD = "ENABLECMD";
        public const int nSC_ENABLECMD = 121;
        public const string sSC_LINEMSG = "LINEMSG";
        public const int nSC_LINEMSG = 122;
        public const string sSC_EVENTMSG = "EVENTMSG";
        public const int nSC_EVENTMSG = 123;
        public const string sSC_SOUNDMSG = "SOUNDMSG";
        public const int nSC_SOUNDMSG = 124;
        public const string sSC_SETMISSION = "SETMISSION";
        public const int nSC_SETMISSION = 125;
        public const string sSC_CLEARMISSION = "CLEARMISSION";
        public const int nSC_CLEARMISSION = 126;
        public const string sSC_MONPWR = "MONPWR";
        public const int nSC_MONPWR = 127;
        public const string sSC_ENTER_OK = "ENTER_OK";
        public const int nSC_ENTER_OK = 128;
        public const string sSC_ENTER_FAIL = "ENTER_FAIL";
        public const int nSC_ENTER_FAIL = 129;
        public const string sSC_MONADDITEM = "MONADDITEM";
        public const int nSC_MONADDITEM = 130;
        public const string sSC_CHANGEWEATHER = "CHANGEWEATHER";
        public const int nSC_CHANGEWEATHER = 131;
        public const string sSC_CHANGEWEAPONATOM = "CHANGEWEAPONATOM";
        public const int nSC_CHANGEWEAPONATOM = 132;
        public const string sSC_GETREPAIRCOST = "GETREPAIRCOST";
        public const int nSC_GETREPAIRCOST = 134;
        public const string sSC_KILLHORSE = "KILLHORSE";
        public const int nSC_KILLHORSE = 133;
        public const string sSC_REPAIRITEM = "REPAIRITEM";
        public const int nSC_REPAIRITEM = 135;
        public const string sSC_USEREMERGENCYCLOSE = "USEREMERGENCYCLOSE";
        public const int nSC_USEREMERGENCYCLOSE = 138;
        public const string sSC_BUILDGUILD = "BUILDGUILD";
        public const int nSC_BUILDGUILD = 139;
        public const string sSC_GUILDWAR = "GUILDWAR";
        public const int nSC_GUILDWAR = 140;
        public const string sSC_CHANGEUSERNAME = "CHANGEUSERNAME";
        public const int nSC_CHANGEUSERNAME = 141;
        public const string sSC_CHANGEMONLEVEL = "CHANGEMONLEVEL";
        public const int nSC_CHANGEMONLEVEL = 142;
        public const string sSC_DROPITEMMAP = "DROPITEMMAP";
        public const int nSC_DROPITEMMAP = 143;
        public const string sSC_CLEARITEMMAP = "CLEARITEMMAP";
        public const int nSC_CLEARITEMMAP = 170;
        public const string sSC_PROPOSECASTLEWAR = "PROPOSECASTLEWAR";
        public const int nSC_PROPOSECASTLEWAR = 144;
        public const string sSC_FINISHCASTLEWAR = "FINISHCASTLEWAR";
        public const int nSC_FINISHCASTLEWAR = 145;
        public const string sSC_MOVENPC = "MOVENPC";
        public const int nSC_MOVENPC = 146;
        public const string sSC_SPEAK = "SPEAK";
        public const int nSC_SPEAK = 147;
        public const string sSC_SENDCMD = "SENDCMD";
        public const int nSC_SENDCMD = 148;
        public const string sSC_INCFAME = "INCFAME";
        public const int nSC_INCFAME = 149;
        public const string sSC_DECFAME = "DECFAME";
        public const int nSC_DECFAME = 150;
        public const string sSC_CAPTURECASTLEFLAG = "CAPTURECASTLEFLAG";
        public const int nSC_CAPTURECASTLEFLAG = 151;
        public const string sSC_MAKESHOOTER = "MAKESHOOTER";
        public const int nSC_MAKESHOOTER = 153;
        public const string sSC_KILLSHOOTER = "KILLSHOOTER";
        public const int nSC_KILLSHOOTER = 154;
        public const string sSC_LEAVESHOOTER = "LEAVESHOOTER";
        public const int nSC_LEAVESHOOTER = 155;
        public const string sSC_CHANGEMAPATTR = "CHANGEMAPATTR";
        public const int nSC_CHANGEMAPATTR = 157;
        public const string sSC_RESETMAPATTR = "RESETMAPATTR";
        public const int nSC_RESETMAPATTR = 158;
        public const string sSC_MAKECASTLEDOOR = "MAKECASTLEDOOR";
        public const int nSC_MAKECASTLEDOOR = 159;
        public const string sSC_REPAIRCASTLEDOOR = "REPAIRCASTLEDOOR";
        public const int nSC_REPAIRCASTLEDOOR = 160;
        public const string sSC_CHARGESHOOTER = "CHARGESHOOTER";
        public const int nSC_CHARGESHOOTER = 161;
        public const string sSC_SETAREAATTR = "SETAREAATTR";
        public const int nSC_SETAREAATTR = 162;
        public const string sSC_CLEARDELAYGOTO = "CLEARDELAYGOTO";
        public const int nSC_CLEARDELAYGOTO = 163;
        public const string sSC_TESTFLAG = "TESTFLAG";
        public const int nSC_TESTFLAG = 164;
        public const string sSC_APPLYFLAG = "APPLYFLAG";
        public const int nSC_APPLYFLAG = 165;
        public const string sSC_PASTEFLAG = "PASTEFLAG";
        public const int nSC_PASTEFLAG = 166;
        public const string sSC_GETBACKCASTLEGOLD = "GETBACKCASTLEGOLD";
        public const int nSC_GETBACKCASTLEGOLD = 167;
        public const string sSC_GETBACKUPGITEM = "GETBACKUPGITEM";
        public const int nSC_GETBACKUPGITEM = 168;
        public const string sSC_TINGWAR = "TINGWAR";
        public const int nSC_TINGWAR = 169;
        public const string sSC_SAVEPASSWD = "SAVEPASSWD";
        public const int nSC_SAVEPASSWD = 171;
        public const string sSC_CREATENPC = "CREATENPC";
        public const int nSC_CREATENPC = 172;
        public const string sSC_TAKEBONUS = "TAKEBONUS";
        public const int nSC_TAKEBONUS = 173;
        public const string sSC_SYSMSG = "SYSMSG";
        public const int nSC_SYSMSG = 174;
        public const string sSC_LOADVALUE = "LOADVALUE";
        public const int nSC_LOADVALUE = 175;
        public const string sSC_SAVEVALUE = "SAVEVALUE";
        public const int nSC_SAVEVALUE = 176;
        public const string sSC_SAVELOG = "SAVELOG";
        public const int nSC_SAVELOG = 177;
        public const string sSC_GETMARRIED = "GETMARRIED";
        public const int nSC_GETMARRIED = 178;
        public const string sSC_DIVORCE = "DIVORCE";
        public const int nSC_DIVORCE = 189;
        public const string sSC_CAPTURESAYING = "CAPTURESAYING";
        public const int nSC_CAPTURESAYING = 190;
        public const string sSC_CANCELMARRIAGERING = "CANCELMARRIAGERING";
        public const int nSC_CANCELMARRIAGERING = 191;
        public const string sSC_OPENUSERMARKET = "OPENUSERMARKET";
        public const int nSC_OPENUSERMARKET = 192;
        public const string sSC_SETTYPEUSERMARKET = "SETTYPEUSERMARKET";
        public const int nSC_SETTYPEUSERMARKET = 193;
        public const string sSC_CHECKSOLDITEMSUSERMARKET = "CHECKSOLDITEMSUSERMARKET";
        public const int nSC_CHECKSOLDITEMSUSERMARKET = 194;
        public const string sSC_SETGMEMAP = "SETGMEMAP";
        public const int nSC_SETGMEMAP = 200;
        public const string sSC_SETGMEPOINT = "SETGMEPOINT";
        public const int nSC_SETGMEPOINT = 201;
        public const string sSC_SETGMETIME = "SETGMETIME";
        public const int nSC_SETGMETIME = 209;
        public const string sSC_STARTNEWGME = "STARTNEWGME";
        public const int nSC_STARTNEWGME = 202;
        public const string sSC_MOVETOGMEMAP = "MOVETOGMEMAP";
        public const int mSC_MOVETOGMEMAP = 203;
        public const string sSC_FINISHGME = "FINISHGME";
        public const int nSC_FINISHGME = 204;
        public const string sSC_CONTINUEGME = "CONTINUEGME";
        public const int nSC_CONTINUEGME = 205;
        public const string sSC_SETGMEPLAYTIME = "SETGMEPLAYTIME";
        public const int nSC_SETGMEPLAYTIME = 206;
        public const string sSC_SETGMEPAUSETIME = "SETGMEPAUSETIME";
        public const int nSC_SETGMEPAUSETIME = 207;
        public const string sSC_SETGMELIMITUSER = "SETGMELIMITUSER";
        public const int nSC_SETGMELIMITUSER = 208;
        public const string sSC_SETEVENTMAP = "SETEVENTMAP";
        public const int nSC_SETEVENTMAP = 210;
        public const string sSC_RESETEVENTMAP = "RESETEVENTMAP";
        public const int nSC_RESETEVENTMAP = 211;
        public const string sSC_TESTREFINEPOINTS = "TESTREFINEPOINTS";
        public const int nSC_TESTREFINEPOINTS = 220;
        public const string sSC_RESETREFINEWEAPON = "RESETREFINEWEAPON";
        public const int nSC_RESETREFINEWEAPON = 221;
        public const string sSC_TESTREFINEACCESSORIES = "TESTREFINEACCESSORIES";
        public const int nSC_TESTREFINEACCESSORIES = 222;
        public const string sSC_REFINEACCESSORIES = "REFINEACCESSORIES";
        public const int nSC_REFINEACCESSORIES = 223;
        public const string sSC_APPLYMONMISSION = "APPLYMONMISSION";
        public const int nSC_APPLYMONMISSION = 225;
        public const string sSC_MAPMOVER = "MAPMOVER";
        public const int nSC_MAPMOVER = 226;
        public const string sSC_ADDSTR = "ADDSTR";
        public const int nSC_ADDSTR = 227;
        public const string sSC_SETEVENTDAMAGE = "SETEVENTDAMAGE";
        public const int nSC_SETEVENTDAMAGE = 228;
        public const string sSC_FORMATSTR = "FORMATSTR";
        public const int nSC_FORMATSTR = 229;
        public const string sSC_CLEARPATH = "CLEARPATH";
        public const int nSC_CLEARPATH = 230;
        public const string sSC_ADDPATH = "ADDPATH";
        public const int nSC_ADDPATH = 231;
        public const string sSC_APPLYPATH = "APPLYPATH";
        public const int nSC_APPLYPATH = 232;
        public const string sSC_MAPSPELL = "MAPSPELL";
        public const int nSC_MAPSPELL = 233;
        public const string sSC_GIVEEXP = "GIVEEXP";
        public const int nSC_GIVEEXP = 234;
        public const string sSC_GROUPMOVE = "GROUPMOVE";
        public const int nSC_GROUPMOVE = 235;
        public const string sSC_GIVEEXPMAP = "GIVEEXPMAP";
        public const int nSC_GIVEEXPMAP = 236;
        public const string sSC_APPLYMONEX = "APPLYMONEX";
        public const int nSC_APPLYMONEX = 237;
        public const string sSC_CLEARNAMELIST = "CLEARNAMELIST";
        public const int nSC_CLEARNAMELIST = 238;
        public const string sSC_TINGCASTLEVISITOR = "TINGCASTLEVISITOR";
        public const int nSC_TINGCASTLEVISITOR = 239;
        public const string sSC_MAKEHEALZONE = "MAKEHEALZONE";
        public const int nSC_MAKEHEALZONE = 240;
        public const string sSC_MAKEDAMAGEZONE = "MAKEDAMAGEZONE";
        public const int nSC_MAKEDAMAGEZONE = 241;
        public const string sSC_CLEARZONE = "CLEARZONE";
        public const int nSC_CLEARZONE = 242;
        public const string sSC_READVALUESQL = "READVALUESQL";
        public const int nSC_READVALUESQL = 250;
        public const string sSC_READSTRINGSQL = "READSTRINGSQL";
        public const int nSC_READSTRINGSQL = 255;
        public const string sSC_WRITEVALUESQL = "WRITEVALUESQL";
        public const int nSC_WRITEVALUESQL = 251;
        public const string sSC_INCVALUESQL = "INCVALUESQL";
        public const int nSC_INCVALUESQL = 252;
        public const string sSC_DECVALUESQL = "DECVALUESQL";
        public const int nSC_DECVALUESQL = 253;
        public const string sSC_UPDATEVALUESQL = "UPDATEVALUESQL";
        public const int nSC_UPDATEVALUESQL = 254;
        public const string sSC_KILLSLAVE = "KILLSLAVE";
        public const int nSC_KILLSLAVE = 260;
        public const string sSC_SETITEMEVENT = "SETITEMEVENT";
        public const int nSC_SETITEMEVENT = 261;
        public const string sSC_REMOVEITEMEVENT = "REMOVEITEMEVENT";
        public const int nSC_REMOVEITEMEVENT = 262;
        public const string sSC_RETURN = "RETURN";
        public const int nSC_RETURN = 263;
        public const string sSC_CLEARCASTLEOWNER = "CLEARCASTLEOWNER";
        public const int nSC_CLEARCASTLEOWNER = 270;
        public const string sSC_DISSOLUTIONGUILD = "DISSOLUTIONGUILD";
        public const int nSC_DISSOLUTIONGUILD = 271;
        public const string sSC_CHANGEGENDER = "CHANGEGENDER";
        public const int nSC_CHANGEGENDER = 272;
        public const string sSC_SETFAME = "SETFAME";
        public const int nSC_SETFAME = 273;
        public const string sSC_CHANGELEVEL = "CHANGELEVEL";
        public const int nSC_CHANGELEVEL = 300;
        public const string sSC_MARRY = "MARRY";
        public const int nSC_MARRY = 301;
        public const string sSC_UNMARRY = "UNMARRY";
        public const int nSC_UNMARRY = 302;
        public const string sSC_GETMARRY = "GETMARRY";
        public const int nSC_GETMARRY = 303;
        public const string sSC_GETMASTER = "GETMASTER";
        public const int nSC_GETMASTER = 304;
        public const string sSC_CLEARSKILL = "CLEARSKILL";
        public const int nSC_CLEARSKILL = 305;
        public const string sSC_DELNOJOBSKILL = "DELNOJOBSKILL";
        public const int nSC_DELNOJOBSKILL = 306;
        public const string sSC_DELSKILL = "DELSKILL";
        public const int nSC_DELSKILL = 307;
        public const string sSC_ADDSKILL = "ADDSKILL";
        public const int nSC_ADDSKILL = 308;
        public const string sSC_SKILLLEVEL = "SKILLLEVEL";
        public const int nSC_SKILLLEVEL = 309;
        public const string sSC_CHANGEPKPOINT = "CHANGEPKPOINT";
        public const int nSC_CHANGEPKPOINT = 310;
        public const string sSC_CHANGEEXP = "CHANGEEXP";
        public const int nSC_CHANGEEXP = 311;
        public const string sSC_CHANGEJOB = "CHANGEJOB";
        public const int nSC_CHANGEJOB = 312;
        public const string sSC_MISSION = "MISSION";
        public const int nSC_MISSION = 313;
        public const string sSC_MOBPLACE = "MOBPLACE";
        public const int nSC_MOBPLACE = 314;
        public const string sSC_SETMEMBERTYPE = "SETMEMBERTYPE";
        public const int nSC_SETMEMBERTYPE = 315;
        public const string sSC_SETMEMBERLEVEL = "SETMEMBERLEVEL";
        public const int nSC_SETMEMBERLEVEL = 316;
        public const string sSC_GAMEGOLD = "GAMEGOLD";
        public const int nSC_GAMEGOLD = 317;
        public const string sSC_AUTOADDGAMEGOLD = "AUTOADDGAMEGOLD";
        public const int nSC_AUTOADDGAMEGOLD = 318;
        public const string sSC_AUTOSUBGAMEGOLD = "AUTOSUBGAMEGOLD";
        public const int nSC_AUTOSUBGAMEGOLD = 319;
        public const string sSC_CHANGENAMECOLOR = "CHANGENAMECOLOR";
        public const int nSC_CHANGENAMECOLOR = 320;
        public const string sSC_CLEARPASSWORD = "CLEARPASSWORD";
        public const int nSC_CLEARPASSWORD = 321;
        public const string sSC_RENEWLEVEL = "RENEWLEVEL";
        public const int nSC_RENEWLEVEL = 322;
        public const string sSC_KILLMONEXPRATE = "KILLMONEXPRATE";
        public const int nSC_KILLMONEXPRATE = 323;
        public const string sSC_POWERRATE = "POWERRATE";
        public const int nSC_POWERRATE = 324;
        public const string sSC_CHANGEMODE = "CHANGEMODE";
        public const int nSC_CHANGEMODE = 325;
        public const string sSC_CHANGEPERMISSION = "CHANGEPERMISSION";
        public const int nSC_CHANGEPERMISSION = 326;
        public const string sSC_KILL = "KILL";
        public const int nSC_KILL = 327;
        public const string sSC_KICK = "KICK";
        public const int nSC_KICK = 328;
        public const string sSC_BONUSPOINT = "BONUSPOINT";
        public const int nSC_BONUSPOINT = 329;
        public const string sSC_RESTRENEWLEVEL = "RESTRENEWLEVEL";
        public const int nSC_RESTRENEWLEVEL = 330;
        public const string sSC_DELMARRY = "DELMARRY";
        public const int nSC_DELMARRY = 331;
        public const string sSC_DELMASTER = "DELMASTER";
        public const int nSC_DELMASTER = 332;
        public const string sSC_MASTER = "MASTER";
        public const int nSC_MASTER = 333;
        public const string sSC_UNMASTER = "UNMASTER";
        public const int nSC_UNMASTER = 334;
        public const string sSC_CREDITPOINT = "CREDITPOINT";
        public const int nSC_CREDITPOINT = 335;
        public const string sSC_CLEARNEEDITEMS = "CLEARNEEDITEMS";
        public const int nSC_CLEARNEEDITEMS = 336;
        public const string sSC_CLEARMAKEITEMS = "CLEARMAKEITEMS";
        public const int nSC_CLEARMAEKITEMS = 337;
        public const string sSC_SETSENDMSGFLAG = "SETSENDMSGFLAG";
        public const int nSC_SETSENDMSGFLAG = 338;
        public const string sSC_UPGRADEITEMS = "UPGRADEITEM";
        public const int nSC_UPGRADEITEMS = 339;
        public const string sSC_UPGRADEITEMSEX = "UPGRADEITEMEX";
        public const int nSC_UPGRADEITEMSEX = 340;
        public const string sSC_MONGENEX = "MONGENEX";
        public const int nSC_MONGENEX = 341;
        public const string sSC_CLEARMAPMON = "CLEARMAPMON";
        public const int nSC_CLEARMAPMON = 342;
        public const string sSC_SETMAPMODE = "SETMPAMODE";
        public const int nSC_SETMAPMODE = 343;
        public const string sSC_GAMEPOINT = "GAMEPOINT";
        public const int nSC_GAMEPOINT = 344;
        public const string sSC_PKZONE = "PKZONE";
        public const int nSC_PKZONE = 345;
        public const string sSC_RESTBONUSPOINT = "RESTBONUSPOINT";
        public const int nSC_RESTBONUSPOINT = 346;
        public const string sSC_TAKECASTLEGOLD = "TAKECASTLEGOLD";
        public const int nSC_TAKECASTLEGOLD = 347;
        public const string sSC_HUMANHP = "HUMANHP";
        public const int nSC_HUMANHP = 348;
        public const string sSC_HUMANMP = "HUMANMP";
        public const int nSC_HUMANMP = 349;
        public const string sSC_BUILDPOINT = "GUILDBUILDPOINT";
        public const int nSC_BUILDPOINT = 350;
        public const string sSC_AURAEPOINT = "GUILDAURAEPOINT";
        public const int nSC_AURAEPOINT = 351;
        public const string sSC_STABILITYPOINT = "GUILDSTABILITYPOINT";
        public const int nSC_STABILITYPOINT = 352;
        public const string sSC_FLOURISHPOINT = "GUILDFLOURISHPOINT";
        public const int nSC_FLOURISHPOINT = 353;
        // 'OPENMAGICBOX'
        public const string sSC_OPENMAGICBOX = "OPENITEMBOX";
        public const int nSC_OPENMAGICBOX = 354;
        public const string sSC_SETRANKLEVELNAME = "SETRANKLEVELNAME";
        public const int nSC_SETRANKLEVELNAME = 355;
        public const string sSC_GMEXECUTE = "GMEXECUTE";
        public const int nSC_GMEXECUTE = 356;
        public const string sSC_GUILDCHIEFITEMCOUNT = "GUILDCHIEFITEMCOUNT";
        public const int nSC_GUILDCHIEFITEMCOUNT = 357;
        public const string sSC_ADDNAMEDATELIST = "ADDNAMEDATELIST";
        public const int nSC_ADDNAMEDATELIST = 358;
        public const string sSC_DELNAMEDATELIST = "DELNAMEDATELIST";
        public const int nSC_DELNAMEDATELIST = 359;
        public const string sSC_MOBFIREBURN = "MOBFIREBURN";
        public const int nSC_MOBFIREBURN = 360;
        public const string sSC_MESSAGEBOX = "MESSAGEBOX";
        public const int nSC_MESSAGEBOX = 361;
        public const string sSC_SETSCRIPTFLAG = "SETSCRIPTFLAG";
        // 设置用于NPC输入框操作的控制标志
        public const int nSC_SETSCRIPTFLAG = 362;
        public const string sSC_SETAUTOGETEXP = "SETAUTOGETEXP";
        public const int nSC_SETAUTOGETEXP = 363;
        public const string sSC_VAR = "VAR";
        public const int nSC_VAR = 364;
        public const string sSC_LOADVAR = "LOADVAR";
        public const int nSC_LOADVAR = 365;
        public const string sSC_SAVEVAR = "SAVEVAR";
        public const int nSC_SAVEVAR = 366;
        public const string sSC_CALCVAR = "CALCVAR";
        public const int nSC_CALCVAR = 367;
        public const string sSC_GUILDRECALL = "GUILDRECALL";
        public const int nSC_GUILDRECALL = 368;
        public const string sSC_GROUPADDLIST = "GROUPADDLIST";
        public const int nSC_GROUPADDLIST = 369;
        public const string sSC_CLEARLIST = "CLEARLIST";
        public const int nSC_CLEARLIST = 370;
        public const string sSC_GROUPRECALL = "GROUPRECALL";
        public const int nSC_GROUPRECALL = 371;
        public const string sSC_GROUPMOVEMAP = "GROUPMOVEMAP";
        public const int nSC_GROUPMOVEMAP = 372;
        public const string sSC_SAVESLAVES = "SAVESLAVES";
        public const int nSC_SAVESLAVES = 373;
        // =========================================================
        public const string sCHECKUSERDATE = "CHECKUSERDATE";
        // 检查会员时间
        public const int nCHECKUSERDATE = 375;
        public const string sADDUSERDATE = "ADDUSERDATE";
        // 加入会员人物及时间
        public const int nADDUSERDATE = 376;
        public const string sDELUSERDATE = "DELUSERDATE";
        // 删除会员人物及时间
        public const int nDELUSERDATE = 377;
        public const string sSC_OffLine = "OFFLINE";
        // 增加挂机
        public const int nSC_OffLine = 379;
        public const string sSC_REPAIRALL = "REPAIRALL";
        // 特修身上所有装备
        public const int nSC_REPAIRALL = 380;
        public const string sSC_SETRANDOMNO = "SETRANDOMNO";
        // 产生一个随机数字
        public const int nSC_SETRANDOMNO = 381;
        public const string sSC_QUERYBAGITEMS = "QUERYBAGITEMS";
        // 刷新包裹物品命令
        public const int nSC_QUERYBAGITEMS = 382;
        public const string sSC_ISHIGH = "ISHIGH";
        public const int nSC_ISHIGH = 383;
        // =================================================================
        public const string sOFFLINEMSG = "@@offlinemsg";
        // 增加挂机
        public const string sSL_SENDMSG = "@@sendmsg";
        public const string sSUPERREPAIR = "@s_repair";
        public const string sSUPERREPAIROK = "~@s_repair";
        public const string sSUPERREPAIRFAIL = "@fail_s_repair";
        public const string sREPAIR = "@repair";
        public const string sREPAIROK = "~@repair";
        public const string sBUY = "@buy";
        public const string sSELL = "@sell";
        public const string sMAKEDURG = "@makedrug";
        public const string sPRICES = "@prices";
        public const string sSTORAGE = "@storage";
        public const string sGETBACK = "@getback";
        public const string sUPGRADENOW = "@upgradenow";
        public const string sUPGRADEING = "~@upgradenow_ing";
        public const string sUPGRADEOK = "~@upgradenow_ok";
        public const string sUPGRADEFAIL = "~@upgradenow_fail";
        public const string sGETBACKUPGNOW = "@getbackupgnow";
        public const string sGETBACKUPGOK = "~@getbackupgnow_ok";
        public const string sGETBACKUPGFAIL = "~@getbackupgnow_fail";
        public const string sGETBACKUPGFULL = "~@getbackupgnow_bagfull";
        public const string sGETBACKUPGING = "~@getbackupgnow_ing";
        public const string sEXIT = "@exit";
        public const string sBACK = "@back";
        public const string sMAIN = "@main";
        public const string sFAILMAIN = "~@main";
        public const string sGETMASTER = "@@getmaster";
        public const string sGETMARRY = "@@getmarry";
        public const string sUSEITEMNAME = "@@useitemname";
        public const string sBUILDGUILDNOW = "@@buildguildnow";
        public const string sSCL_GUILDWAR = "@@guildwar";
        public const string sDONATE = "@@donate";
        public const string sREQUESTCASTLEWAR = "@requestcastlewarnow";
        public const string sCASTLENAME = "@@castlename";
        public const string sWITHDRAWAL = "@@withdrawal";
        public const string sRECEIPTS = "@@receipts";
        public const string sOPENMAINDOOR = "@openmaindoor";
        public const string sCLOSEMAINDOOR = "@closemaindoor";
        public const string sREPAIRDOORNOW = "@repairdoornow";
        public const string sREPAIRWALLNOW1 = "@repairwallnow1";
        public const string sREPAIRWALLNOW2 = "@repairwallnow2";
        public const string sREPAIRWALLNOW3 = "@repairwallnow3";
        public const string sHIREARCHERNOW = "@hirearchernow";
        public const string sHIREGUARDNOW = "@hireguardnow";
        public const string sHIREGUARDOK = "@hireguardok";
        public const string sMarket_Def = "Market_Def";
        public const string sNpc_def = "Npc_def";
        public const string g_sGameLogMsg1 = "%d\09%s\09%d\09%d\09%s\09%s\09%d\09%s\09%s";
        public const string g_sHumanDieEvent = "人物死亡事件";
        public const string g_sHitOverSpeed = "[攻击超速] {0} 间隔:{1} 数量:{2}";
        public const string g_sRunOverSpeed = "[跑步超速] {0} 间隔:{1} 数量:{2}";
        public const string g_sWalkOverSpeed = "[行走超速] {0} 间隔:{1} 数量:{2}";
        public const string g_sSpellOverSpeed = "[魔法超速] {0} 间隔:{1} 数量:{2}";
        public const string g_sBunOverSpeed = "[游戏超速] {0} 间隔:{1} 数量:{2}";
        public const string g_sGameCommandPermissionTooLow = "权限不够！！！";
        public const string g_sGameCommandParamUnKnow = "命令格式: @{0} {1}";
        public const string g_sGameCommandMoveHelpMsg = "地图号";
        public const string g_sGameCommandPositionMoveHelpMsg = "地图号 座标X 座标Y";
        public const string g_sGameCommandPositionMoveCanotMoveToMap = "无法移动到地图: %s X:%s Y:%s";
        public const string g_sGameCommandInfoHelpMsg = "人物名称";
        public const string g_sNowNotOnLineOrOnOtherServer = "{0} 现在不在线，或在其它服务器上！！！";
        public const string g_sGameCommandMobCountHelpMsg = "地图号";
        public const string g_sGameCommandMobCountMapNotFound = "指定的地图不存在！！！";
        public const string g_sGameCommandMobCountMonsterCount = "怪物数量：{0}";
        public const string g_sGameCommandHumanCountHelpMsg = "地图号";
        public const string g_sGameCommandKickHumanHelpMsg = "人物名称";
        public const string g_sGameCommandTingHelpMsg = "人物名称";
        public const string g_sGameCommandSuperTingHelpMsg = "人物名称 范围(0-10)";
        public const string g_sGameCommandMapMoveHelpMsg = "源地图  目标地图";
        public const string g_sGameCommandMapMoveMapNotFound = "地图{0}不存在！！！";
        public const string g_sGameCommandShutupHelpMsg = "人物名称  时间长度(分钟)";
        public const string g_sGameCommandShutupHumanMsg = "{0} 已被禁言{1}分钟";
        public const string g_sGameCommandGamePointHelpMsg = "人物名称 控制符(+,-,=) 游戏点数(1-100000000)";
        public const string g_sGameCommandGamePointHumanMsg = "你的游戏点已增加{0}点，当前总点数为{1}点。";
        public const string g_sGameCommandGamePointGMMsg = "{0}的游戏点已增加{1}点，当前总点数为{2}点。";
        public const string g_sGameCommandCreditPointHelpMsg = "人物名称 控制符(+,-,=) 声望点数(0-255)";
        public const string g_sGameCommandCreditPointHumanMsg = "你的声望点已增加%d点，当前总声望点数为%d点。";
        public const string g_sGameCommandCreditPointGMMsg = "%s的声望点已增加%d点，当前总声望点数为%d点。";
        public const string g_sGameCommandGameGoldHelpMsg = " 人物名称 控制符(+,-,=) 游戏币(1-200000000)";
        public const string g_sGameCommandGameGoldHumanMsg = "你的%s已增加%d，当前拥有%d%s。";
        public const string g_sGameCommandGameGoldGMMsg = "%s的%s已增加%d，当前拥有%d%s。";
        public const string g_sGameCommandMapInfoMsg = "地图名称: {0}({1})";
        public const string g_sGameCommandMapInfoSizeMsg = "地图大小: X({0}) Y({1})";
        public const string g_sGameCommandShutupReleaseHelpMsg = "人物名称";
        public const string g_sGameCommandShutupReleaseCanSendMsg = "你已经恢复聊天功能！！！";
        public const string g_sGameCommandShutupReleaseHumanCanSendMsg = "{0} 已经恢复聊天。";
        public const string g_sGameCommandShutupListIsNullMsg = "禁言列表为空！！！";
        public const string g_sGameCommandLevelConsoleMsg = "[等级调整] {0} ({1} -> {2})";
        public const string g_sGameCommandSbkGoldHelpMsg = "城堡名称 控制符(=、-、+) 金币数(1-100000000)";
        public const string g_sGameCommandSbkGoldCastleNotFoundMsg = "城堡{0}未找到！！！";
        public const string g_sGameCommandSbkGoldShowMsg = "{0}的金币数为: {1} 今天收入: {2}";
        public const string g_sGameCommandRecallHelpMsg = "人物名称";
        public const string g_sGameCommandReGotoHelpMsg = "人物名称";
        public const string g_sGameCommandShowHumanFlagHelpMsg = "人物名称 标识号";
        public const string g_sGameCommandShowHumanFlagONMsg = "%s: [%d] = ON";
        public const string g_sGameCommandShowHumanFlagOFFMsg = "%s: [%d] = OFF";
        public const string g_sGameCommandShowHumanUnitHelpMsg = "人物名称 单元号";
        public const string g_sGameCommandShowHumanUnitONMsg = "%s: [%d] = ON";
        public const string g_sGameCommandShowHumanUnitOFFMsg = "%s: [%d] = OFF";
        public const string g_sGameCommandMobHelpMsg = "怪物名称 数量 等级";
        public const string g_sGameCommandMobMsg = "怪物名称不正确或其它未问题！！！";
        public const string g_sGameCommandMobNpcHelpMsg = "NPC名称 脚本文件名 外形(数字) 属沙城(0,1)";
        public const string g_sGameCommandNpcScriptHelpMsg = "？？？？";
        public const string g_sGameCommandDelNpcMsg = "命令使用方法不正确，必须与NPC面对面，才能使用此命令！！！";
        public const string g_sGameCommandRecallMobHelpMsg = "怪物名称 数量 等级";
        public const string g_sGameCommandLuckPointHelpMsg = "人物名称 控制符 幸运点数";
        public const string g_sGameCommandLuckPointMsg = "%s 的幸运点数为:%d/%g 幸运值为:%d";
        public const string g_sGameCommandLotteryTicketMsg = "已中彩票数:%d 未中彩票数:%d 一等奖:%d 二等奖:%d 三等奖:%d 四等奖:%d 五等奖:%d 六等奖:%d ";
        public const string g_sGameCommandReloadGuildHelpMsg = "行会名称";
        public const string g_sGameCommandReloadGuildOnMasterserver = "此命令只能在主游戏服务器上执行！！！";
        public const string g_sGameCommandReloadGuildNotFoundGuildMsg = "未找到行会%s！！！";
        public const string g_sGameCommandReloadGuildSuccessMsg = "行会%s重加载成功...";
        public const string g_sGameCommandReloadLineNoticeSuccessMsg = "重新加载公告设置信息完成。";
        public const string g_sGameCommandReloadLineNoticeFailMsg = "重新加载公告设置信息失败！！！";
        public const string g_sGameCommandFreePKHelpMsg = "人物名称";
        public const string g_sGameCommandFreePKHumanMsg = "你的PK值已经被清除...";
        public const string g_sGameCommandFreePKMsg = "%s的PK值已经被清除...";
        public const string g_sGameCommandPKPointHelpMsg = "人物名称";
        public const string g_sGameCommandPKPointMsg = "%s的PK点数为:%d";
        public const string g_sGameCommandIncPkPointHelpMsg = "人物名称 PK点数";
        public const string g_sGameCommandIncPkPointAddPointMsg = "%s的PK值已增加%d点...";
        public const string g_sGameCommandIncPkPointDecPointMsg = "%s的PK值已减少%d点...";
        public const string g_sGameCommandHumanLocalHelpMsg = "人物名称";
        public const string g_sGameCommandHumanLocalMsg = "%s来自:%s";
        public const string g_sGameCommandPrvMsgHelpMsg = "人物名称";
        public const string g_sGameCommandPrvMsgUnLimitMsg = "%s 已从禁止私聊列表中删除...";
        public const string g_sGameCommandPrvMsgLimitMsg = "%s 已被加入禁止私聊列表...";
        public const string g_sGamecommandMakeHelpMsg = " 物品名称  数量";
        public const string g_sGamecommandMakeItemNameOrPerMissionNot = "输入的物品名称不正确，或权限不够！！！";
        public const string g_sGamecommandMakeInCastleWarRange = "攻城区域，禁止使用此功能！！！";
        public const string g_sGamecommandMakeInSafeZoneRange = "非安全区，禁止使用此功能！！！";
        public const string g_sGamecommandMakeItemNameNotFound = "%s 物品名称不正确！！！";
        public const string g_sGamecommandSuperMakeHelpMsg = "身上没指定物品！！！";
        public const string g_sGameCommandViewWhisperHelpMsg = " 人物名称";
        public const string g_sGameCommandViewWhisperMsg1 = "已停止侦听%s的私聊信息...";
        public const string g_sGameCommandViewWhisperMsg2 = "正在侦听%s的私聊信息...";
        public const string g_sGameCommandReAliveHelpMsg = " 人物名称";
        public const string g_sGameCommandReAliveMsg = "%s 已获重生.";
        public const string g_sGameCommandChangeJobHelpMsg = " 人物名称 职业类型(Warr Wizard Taos)";
        public const string g_sGameCommandChangeJobMsg = "%s 的职业更改成功。";
        public const string g_sGameCommandChangeJobHumanMsg = "职业更改成功。";
        public const string g_sGameCommandTestGetBagItemsHelpMsg = "(用于测试升级武器方面参数)";
        public const string g_sGameCommandShowUseItemInfoHelpMsg = "人物名称";
        public const string g_sGameCommandBindUseItemHelpMsg = "人物名称 物品类型 绑定方法";
        public const string g_sGameCommandBindUseItemNoItemMsg = "%s的%s没有戴物品！！！";
        public const string g_sGameCommandBindUseItemAlreadBindMsg = "%s的%s上的物品早已绑定过了！！！";
        public const string g_sGameCommandMobFireBurnHelpMsg = "命令格式: %s %s %s %s %s %s %s";
        public const string g_sGameCommandMobFireBurnMapNotFountMsg = "地图%s 不存在";
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

        public static void CopyStdItemToOStdItem(TStdItem StdItem, TOStdItem OStdItem)
        {
            OStdItem.Name = StdItem.Name;
            OStdItem.StdMode = StdItem.StdMode;
            OStdItem.Shape = StdItem.Shape;
            OStdItem.Weight = StdItem.Weight;
            OStdItem.AniCount = StdItem.AniCount;
            OStdItem.Source = (byte)StdItem.Source;
            OStdItem.Reserved = StdItem.reserved;
            OStdItem.NeedIdentify = StdItem.NeedIdentify;
            OStdItem.Looks = StdItem.Looks;
            OStdItem.DuraMax = (ushort)StdItem.DuraMax;
            OStdItem.AC = HUtil32.MakeWord(HUtil32._MIN(byte.MaxValue, HUtil32.LoWord(StdItem.AC)), HUtil32._MIN(byte.MaxValue, HUtil32.HiWord(StdItem.AC)));
            OStdItem.MAC = HUtil32.MakeWord(HUtil32._MIN(byte.MaxValue, HUtil32.LoWord(StdItem.MAC)), HUtil32._MIN(byte.MaxValue, HUtil32.HiWord(StdItem.MAC)));
            OStdItem.DC = HUtil32.MakeWord(HUtil32._MIN(byte.MaxValue, HUtil32.LoWord(StdItem.DC)), HUtil32._MIN(byte.MaxValue, HUtil32.HiWord(StdItem.DC)));
            OStdItem.MC = HUtil32.MakeWord(HUtil32._MIN(byte.MaxValue, HUtil32.LoWord(StdItem.MC)), HUtil32._MIN(byte.MaxValue, HUtil32.HiWord(StdItem.MC)));
            OStdItem.SC = HUtil32.MakeWord(HUtil32._MIN(byte.MaxValue, HUtil32.LoWord(StdItem.SC)), HUtil32._MIN(byte.MaxValue, HUtil32.HiWord(StdItem.SC)));
            OStdItem.Need = (byte)StdItem.Need;
            OStdItem.NeedLevel = (byte)StdItem.NeedLevel;
            OStdItem.Price = (int)StdItem.Price;
        }

        public static bool LoadLineNotice(string FileName)
        {
            var result = false;
            int i;
            string sText;
            if (File.Exists(FileName))
            {
                LineNoticeList.LoadFromFile(FileName);
                i = 0;
                while (true)
                {
                    if (LineNoticeList.Count <= i)
                    {
                        break;
                    }
                    sText = LineNoticeList[i].Trim();
                    if (sText == "")
                    {
                        //LineNoticeList.Remove(i);
                        continue;
                    }
                    LineNoticeList[i] = sText;
                    i++;
                }
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 随机获取其他服务器
        /// </summary>
        /// <param name="btServerIndex"></param>
        /// <param name="sIPaddr"></param>
        /// <param name="nPort"></param>
        /// <returns></returns>
        public static bool GetMultiServerAddrPort(byte btServerIndex, ref string sIPaddr, ref int nPort)
        {
            TRouteInfo RouteInfo;
            var result = false;
            for (var i = 0; i < M2Share.ServerTableList.Length; i++)
            {
                RouteInfo = M2Share.ServerTableList[i];
                if (RouteInfo.nGateCount <= 0)
                {
                    continue;
                }
                if (RouteInfo.nServerIdx == btServerIndex)
                {
                    sIPaddr = GetRandpmRoute(RouteInfo, ref nPort);
                    result = true;
                    break;
                }
            }
            return result;
        }

        public static string GetRandpmRoute(TRouteInfo RouteInfo, ref int nGatePort)
        {
            var nC = M2Share.RandomNumber.Random(RouteInfo.nGateCount);
            nGatePort = RouteInfo.nGameGatePort[nC];
            return RouteInfo.sGameGateIP[nC];
        }

        static M2Share()
        {
            ServerConf = new ServerConfig();
            CommandConf = new GameCmdConfig();
            StringConf = new StringConfig();
            ExpConf = new ExpsConfig();
            RandomNumber = RandomNumber.GetInstance();
            LogSystem = new MirLog();
            g_Config = new TM2Config();
        }

        public static void MainOutMessage(string Msg)
        {
            Console.WriteLine('[' + DateTime.Now.ToString() + "] " + Msg);
        }

        public static void MainOutMessage(string Msg, MessageType messageType = MessageType.Success, MessageLevel messageLevel = MessageLevel.None, ConsoleColor messageColor = ConsoleColor.White)
        {
            LogSystem.LogInfo(Msg, messageType, messageLevel: messageLevel, messageColor: messageColor);
        }

        public static void ErrorMessage(string Msg, MessageType messageType = MessageType.Error, MessageLevel messageLevel = MessageLevel.None, ConsoleColor messageColor = ConsoleColor.Red)
        {
            LogSystem.LogInfo(Msg, messageType, messageLevel: messageLevel, messageColor: messageColor);
        }

        public static int GetExVersionNO(int nVersionDate, ref int nOldVerstionDate)
        {
            int result;
            result = 0;
            nOldVerstionDate = 0;
            if (nVersionDate > 100000000)
            {
                while (nVersionDate > 100000000)
                {
                    nVersionDate -= 100000000;
                    result += 100000000;
                }
            }
            nOldVerstionDate = nVersionDate;
            return result;
        }

        public static byte GetNextDirection(int sx, int sy, int dx, int dy)
        {
            byte result;
            int flagx;
            int flagy;
            result = Grobal2.DR_DOWN;
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

        public static bool CheckUserItems(int nIdx, MirItem StdItem)
        {
            var result = false;
            switch (nIdx)
            {
                case Grobal2.U_DRESS:
                    if (new ArrayList(new byte[] { 10, 11 }).Contains(StdItem.StdMode))
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
            //        
            //        if (MonthDays[false][Month] >= (Day + nDay))
            //        {
            //            break;
            //        }
            //        
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
            var result = nBaseLook + M2Share.RandomNumber.Random(nRage);
            return result;
        }

        public static bool CheckGuildName(string sGuildName)
        {
            var result = true;
            if (sGuildName.Length > g_Config.nGuildNameLen)
            {
                result = false;
                return result;
            }
            for (var i = 0; i <= sGuildName.Length; i++)
            {
                if (sGuildName[i] < '0' || sGuildName[i] == '/' || sGuildName[i] == '\\' || sGuildName[i] == ':' || sGuildName[i] == '*' || sGuildName[i] == ' ' || sGuildName[i] == '\"' || sGuildName[i] == '\'' || sGuildName[i] == '<' || sGuildName[i] == '|' || sGuildName[i] == '?' || sGuildName[i] == '>')
                {
                    result = false;
                }
            }
            return result;
        }

        public static int GetItemNumber()
        {
            g_Config.nItemNumber++;
            if (g_Config.nItemNumber > int.MaxValue / 2 - 1)
            {
                g_Config.nItemNumber = 1;
            }
            return g_Config.nItemNumber;
        }

        public static int GetItemNumberEx()
        {
            g_Config.nItemNumberEx++;
            if (g_Config.nItemNumberEx < int.MaxValue / 2)
            {
                g_Config.nItemNumberEx = int.MaxValue / 2;
            }
            if (g_Config.nItemNumberEx > int.MaxValue - 1)
            {
                g_Config.nItemNumberEx = int.MaxValue / 2;
            }
            return g_Config.nItemNumberEx;
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
            for (var i = 0; i <= sName.Length - 1; i++)
            {
                if (sName[i] >= '0' && sName[i] <= '9' || sName[i] == '-')
                {
                    result = sName.Substring(0, i - 1);
                    sC = sName.Substring(i + 1, sName.Length - i - 1);
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

        public static int GetValNameNo(string sText)
        {
            var result = -1;
            int nValNo;
            if (sText.Length >= 2)
            {
                if (char.ToUpper(sText[0]) == 'P')
                {
                    nValNo = HUtil32.Str_ToInt(sText[1].ToString(), -1);
                    if (nValNo < 10)
                    {
                        result = nValNo;
                    }
                }
                if (char.ToUpper(sText[0]) == 'G')
                {
                    if (sText.Length == 3)
                    {
                        nValNo = HUtil32.Str_ToInt(sText.Substring(2 - 1, 2), -1);
                        if (nValNo < 20)
                        {
                            result = nValNo + 100;
                        }
                    }
                    else
                    {
                        nValNo = HUtil32.Str_ToInt(sText[1].ToString(), -1);
                        if (nValNo < 10)
                        {
                            result = nValNo + 100;
                        }
                    }
                }
                if (char.ToUpper(sText[0]) == 'M')
                {
                    if (sText.Length == 3)
                    {
                        nValNo = HUtil32.Str_ToInt(sText.Substring(2 - 1, 2), -1);
                        if (nValNo < 100)
                        {
                            result = nValNo + 300;
                        }
                    }
                    else
                    {
                        nValNo = HUtil32.Str_ToInt(sText[1].ToString(), -1);
                        if (nValNo < 10)
                        {
                            result = nValNo + 300;
                        }
                    }
                }
                if (char.ToUpper(sText[0]) == 'I')
                {
                    if (sText.Length == 3)
                    {
                        nValNo = HUtil32.Str_ToInt(sText.Substring(2 - 1, 2), -1);
                        if (nValNo < 100)
                        {
                            result = nValNo + 400;
                        }
                    }
                    else
                    {
                        nValNo = HUtil32.Str_ToInt(sText[1].ToString(), -1);
                        if (nValNo < 10)
                        {
                            result = nValNo + 400;
                        }
                    }
                }
                if (char.ToUpper(sText[0]) == 'D')
                {
                    nValNo = HUtil32.Str_ToInt(sText[1].ToString(), -1);
                    if (nValNo < 10)
                    {
                        result = nValNo + 200;
                    }
                }
            }
            return result;
        }

        public static bool IsAccessory(int nIndex)
        {
            bool result;
            var Item = UserEngine.GetStdItem(nIndex);
            if (new ArrayList(new int[] { 19, 20, 21, 22, 23, 24, 26 }).Contains(Item.StdMode))// 修正错误
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public static IList<TMakeItem> GetMakeItemInfo(string sItemName)
        {
            if (g_MakeItemList.TryGetValue(sItemName, out var itemList))
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
            HUtil32.EnterCriticalSection(LogMsgCriticalSection);
            try
            {
                LogStringList.Add(sMsg);
            }
            finally
            {
                HUtil32.LeaveCriticalSection(LogMsgCriticalSection);
            }
        }

        public static void AddLogonCostLog(string sMsg)
        {
            HUtil32.EnterCriticalSection(LogMsgCriticalSection);
            try
            {
                LogonCostLogList.Add(sMsg);
            }
            finally
            {
                HUtil32.LeaveCriticalSection(LogMsgCriticalSection);
            }
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
                    //sList.Remove(n8);
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
            //        if ((g_DisableMakeItemList[I]).ToLower().CompareTo((sItemName).ToLower()) == 0)
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
            //        if ((g_EnableMakeItemList[I]).ToLower().CompareTo((sItemName).ToLower()) == 0)
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
                    //if ((g_DisableMoveMapList[I]).ToLower().CompareTo((sMapName).ToLower()) == 0)
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
                    //if ((g_DisableSellOffList[i]).ToLower().CompareTo((sItemName).ToLower()) == 0)
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
            sFileName = g_Config.sEnvirDir + "ItemBindIPaddr.txt";
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
            string sFileName;
            result = false;
            sFileName = g_Config.sEnvirDir + "ItemBindIPaddr.txt";
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
            bool result;
            ArrayList LoadList;
            string sFileName;
            var sMakeIndex = string.Empty;
            var sItemInde = string.Empty;
            var sBindName = string.Empty;
            result = false;
            sFileName = g_Config.sEnvirDir + "ItemBindAccount.txt";
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
            bool result;
            string sFileName;
            result = false;
            sFileName = g_Config.sEnvirDir + "ItemBindAccount.txt";
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
            bool result;
            ArrayList LoadList;
            string sFileName;
            var sMakeIndex = string.Empty;
            var sBindName = string.Empty;
            result = false;
            sFileName = g_Config.sEnvirDir + "ItemBindChrName.txt";
            LoadList = new ArrayList();
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
            bool result;
            StringList LoadList;
            string sFileName;
            result = false;
            sFileName = g_Config.sEnvirDir + "ItemBindChrName.txt";
            LoadList = new StringList();
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
            bool result;
            ArrayList LoadList;
            string sFileName;
            result = false;
            sFileName = g_Config.sEnvirDir + "DisableMakeItem.txt";
            LoadList = new ArrayList();
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
            string  sFileName = g_Config.sEnvirDir + "DisableMakeItem.txt";
            //g_DisableMakeItemList.SaveToFile(sFileName);
            return true;
        }

        public static bool LoadUnMasterList()
        {
            bool result = false;
            string sFileName = g_Config.sEnvirDir + "UnMaster.txt";
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
            string sFileName = g_Config.sEnvirDir + "UnMaster.txt";
            //g_UnMasterList.SaveToFile(sFileName);
            return true;
        }

        public static bool LoadUnForceMasterList()
        {
            bool result;
            ArrayList LoadList;
            string sFileName;
            result = false;
            sFileName = g_Config.sEnvirDir + "UnForceMaster.txt";
            LoadList = new ArrayList();
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
            string sFileName = g_Config.sEnvirDir + "UnForceMaster.txt";
            //g_UnForceMasterList.SaveToFile(sFileName);
            return true;
        }

        public static bool LoadEnableMakeItem()
        {
            bool result;
            ArrayList LoadList;
            string sFileName;
            result = false;
            sFileName = g_Config.sEnvirDir + "EnableMakeItem.txt";
            LoadList = new ArrayList();
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
            string  sFileName = g_Config.sEnvirDir + "EnableMakeItem.txt";
            //g_EnableMakeItemList.SaveToFile(sFileName);
            return true;
        }

        public static bool LoadDisableMoveMap()
        {
            bool result;
            ArrayList LoadList;
            string sFileName;
            result = false;
            sFileName = g_Config.sEnvirDir + "DisableMoveMap.txt";
            LoadList = new ArrayList();
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
            string  sFileName = g_Config.sEnvirDir + "DisableMoveMap.txt";
            //g_DisableMoveMapList.SaveToFile(sFileName);
            return true;
        }

        public static bool LoadAllowSellOffItem()
        {
            bool result;
            ArrayList LoadList;
            string sFileName;
            result = false;
            sFileName = g_Config.sEnvirDir + "DisableSellOffItem.txt";
            LoadList = new ArrayList();
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
            string sFileName = g_Config.sEnvirDir + "DisableSellOffItem.txt";
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
            int result= -1;
            if (string.Compare(sName, U_DRESSNAME, StringComparison.Ordinal) == 0)
            {
                result = 0;
            }
            else if (string.Compare(sName, U_WEAPONNAME, StringComparison.Ordinal) == 0)
            {
                result = 1;
            }
            else if (string.Compare(sName, U_RIGHTHANDNAME, StringComparison.Ordinal) == 0)
            {
                result = 2;
            }
            else if (string.Compare(sName, U_NECKLACENAME, StringComparison.Ordinal) == 0)
            {
                result = 3;
            }
            else if (string.Compare(sName, U_HELMETNAME, StringComparison.Ordinal) == 0)
            {
                result = 4;
            }
            else if (string.Compare(sName, U_ARMRINGLNAME, StringComparison.Ordinal) == 0)
            {
                result = 5;
            }
            else if (string.Compare(sName, U_ARMRINGRNAME, StringComparison.Ordinal) == 0)
            {
                result = 6;
            }
            else if (string.Compare(sName, U_RINGLNAME, StringComparison.Ordinal) == 0)
            {
                result = 7;
            }
            else if (string.Compare(sName, U_RINGRNAME, StringComparison.Ordinal) == 0)
            {
                result = 8;
            }
            else if (string.Compare(sName, U_BUJUKNAME, StringComparison.Ordinal) == 0)
            {
                result = 9;
            }
            else if (string.Compare(sName, U_BELTNAME, StringComparison.Ordinal) == 0)
            {
                result = 10;
            }
            else if (string.Compare(sName, U_BOOTSNAME, StringComparison.Ordinal) == 0)
            {
                result = 11;
            }
            else if (string.Compare(sName, U_CHARMNAME, StringComparison.Ordinal) == 0)
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
            bool result;
            result = false;
            string sFileName = g_Config.sEnvirDir + "DisableSendMsgList.txt";
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
            bool result;
            ArrayList LoadList;
            var sLineText = string.Empty;
            var sFileName = string.Empty;
            var sItemName = string.Empty;
            var sItemCount = string.Empty;
            result = false;
            sFileName = g_Config.sEnvirDir + "MonDropLimitList.txt";
            LoadList = new ArrayList();
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
            //        if ((sItemName != "") && (nItemCount >= 0))
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
            ArrayList LoadList;
            string sFileName;
            sFileName = g_Config.sEnvirDir + "MonDropLimitList.txt";
            LoadList = new ArrayList();
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
            bool result;
            ArrayList LoadList;
            string sFileName;
            var sItemName = string.Empty;
            result = false;
            sFileName = g_Config.sEnvirDir + "DisableTakeOffList.txt";
            LoadList = new ArrayList();
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
            //            if ((sItemName != "") && (nItemIdx >= 0))
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
            sFileName = g_Config.sEnvirDir + "DisableTakeOffList.txt";
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
            bool result= false;
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
            string sFileName = g_Config.sEnvirDir + "DisableSendMsgList.txt";
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
            //    if ((sHumanName).ToLower().CompareTo((g_DisableSendMsgList[I]).ToLower()) == 0)
            //    {
            //        result = true;
            //        break;
            //    }
            //}
            return result;
        }

        public static bool LoadGameLogItemNameList()
        {
            bool result;
            ArrayList LoadList;
            string sFileName;
            result = false;
            sFileName = g_Config.sEnvirDir + "GameLogItemNameList.txt";
            LoadList = new ArrayList();
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
            byte result= 0;
            //for (I = 0; I < g_GameLogItemNameList.Count; I ++ )
            //{
            //    if ((sItemName).ToLower().CompareTo((g_GameLogItemNameList[I]).ToLower()) == 0)
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
            string sFileName;
            sFileName = g_Config.sEnvirDir + "GameLogItemNameList.txt";
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
            bool result;
            ArrayList LoadList;
            string sFileName;
            result = false;
            sFileName = g_Config.sEnvirDir + "DenyIPAddrList.txt";
            LoadList = new ArrayList();
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
            bool result= false;
            try
            {
                for (var i = 0; i < g_DenyIPAddrList.Count; i++)
                {
                    //if ((sIPaddr).ToLower().CompareTo((g_DenyIPAddrList[I]).ToLower()) == 0)
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
            bool result;
            string sFileName;
            sFileName = g_Config.sEnvirDir + "DenyIPAddrList.txt";
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
            result = true;
            return result;
        }

        public static bool LoadDenyChrNameList()
        {
            bool result;
            ArrayList LoadList;
            string sFileName;
            result = false;
            sFileName = g_Config.sEnvirDir + "DenyChrNameList.txt";
            LoadList = new ArrayList();
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
                //    if ((sChrName).ToLower().CompareTo((g_DenyChrNameList[I]).ToLower()) == 0)
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
            sFileName = g_Config.sEnvirDir + "DenyChrNameList.txt";
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
            bool result;
            ArrayList LoadList;
            string sFileName;
            result = false;
            sFileName = g_Config.sEnvirDir + "DenyAccountList.txt";
            LoadList = new ArrayList();
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
            bool result= false;
            try
            {
                for (var I = 0; I < g_DenyAccountList.Count; I++)
                {
                    //if ((sAccount).ToLower().CompareTo((g_DenyAccountList[I]).ToLower()) == 0)
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
            sFileName = g_Config.sEnvirDir + "DenyAccountList.txt";
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
            var sFileName = Path.Combine(g_Config.sEnvirDir, "NoClearMonList.txt");
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
            else
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
                //    if ((sMonName).ToLower().CompareTo((g_NoHptoexpMonLIst[i]).ToLower()) == 0)
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
            //    if ((sMonName).ToLower().CompareTo((g_NoClearMonLIst[I]).ToLower()) == 0)
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
            sFileName = g_Config.sEnvirDir + "NoHptoExpMonList.txt";
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
            sFileName = g_Config.sEnvirDir + "NoClearMonList.txt";
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
            bool result;
            int I;
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
            string sFileName;
            result = false;
            sFileName = g_Config.sEnvirDir + "GenMsg.txt";
            if (File.Exists(sFileName))
            {
                g_MonSayMsgList.Clear();
                LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (I = 0; I < LoadList.Count; I++)
                {
                    sLineText = LoadList[I].Trim();
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
                                        MonSayMsg.State = TMonStatus.s_KillHuman;
                                        break;
                                    case 1:
                                        MonSayMsg.State = TMonStatus.s_UnderFire;
                                        break;
                                    case 2:
                                        MonSayMsg.State = TMonStatus.s_Die;
                                        break;
                                    case 3:
                                        MonSayMsg.State = TMonStatus.s_MonGen;
                                        break;
                                    default:
                                        MonSayMsg.State = TMonStatus.s_UnderFire;
                                        break;
                                }
                                switch (nColor)
                                {
                                    case 0:
                                        MonSayMsg.Color = TMsgColor.c_Red;
                                        break;
                                    case 1:
                                        MonSayMsg.Color = TMsgColor.c_Green;
                                        break;
                                    case 2:
                                        MonSayMsg.Color = TMsgColor.c_Blue;
                                        break;
                                    case 3:
                                        MonSayMsg.Color = TMsgColor.c_White;
                                        break;
                                    default:
                                        MonSayMsg.Color = TMsgColor.c_White;
                                        break;
                                }
                                MonSayMsg.nRate = nRate;
                                MonSayMsg.sSayMsg = sSayMsg;
                                //for (II = 0; II < g_MonSayMsgList.Count; II ++ )
                                //{
                                //    if ((g_MonSayMsgList[II]).ToLower().CompareTo((sMonName).ToLower()) == 0)
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
        }

        public static string GetIPLocal(string sIPaddr)
        {
            string result;
            var sLocal = new char[254 + 1];
            result = "未知！！！";
            return result;
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
            bool result;
            int nPos;
            result = false;
            if (sIPaddr == "" || dIPaddr == "")
            {
                return result;
            }
            if (dIPaddr[1] == '*')
            {
                result = true;
                return result;
            }
            nPos = dIPaddr.IndexOf('*');
            if (nPos > 0)
            {
                result = HUtil32.CompareLStr(sIPaddr, dIPaddr, nPos - 1);
            }
            else
            {
                result = sIPaddr.ToLower().CompareTo(dIPaddr.ToLower()) == 0;
            }
            return result;
        }
    }
}
