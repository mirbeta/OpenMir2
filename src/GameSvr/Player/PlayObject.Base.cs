using GameSvr.Actor;
using GameSvr.Event;
using GameSvr.GameCommand;
using GameSvr.Guild;
using GameSvr.Items;
using GameSvr.Magic;
using GameSvr.Maps;
using GameSvr.Npc;
using GameSvr.RobotPlay;
using GameSvr.Script;
using System.Collections;
using SystemModule;
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.Player
{
    public partial class PlayObject
    {
        /// <summary>
        /// 性别
        /// </summary>
        public PlayGender Gender;
        /// <summary>
        /// 人物的头发
        /// </summary>
        public byte Hair;
        /// <summary>
        /// 人物的职业 (0:战士 1：法师 2:道士)
        /// </summary>
        public PlayJob Job;
        /// <summary>
        /// 登录帐号名
        /// </summary>
        public string UserAccount;
        /// <summary>
        /// 人物IP地址
        /// </summary>
        public string LoginIpAddr = string.Empty;
        public string LoginIpLocal = string.Empty;
        /// <summary>
        /// 账号过期
        /// </summary>
        public bool AccountExpired;
        /// <summary>
        /// 账号游戏点数检查时间
        /// </summary>
        public int AccountExpiredTick;
        public long ExpireTime;
        public int ExpireCount;
        public int QueryExpireTick;
        /// <summary>
        /// 权限等级
        /// </summary>
        public byte Permission;
        /// <summary>
        /// 人物身上最多可带金币数
        /// </summary>
        public int GoldMax;
        /// <summary>
        /// 允许行会传送
        /// </summary>
        public bool AllowGuildReCall = false;
        /// <summary>
        /// 允许私聊
        /// </summary>
        public bool HearWhisper;
        /// <summary>
        /// 允许群聊
        /// </summary>
        public bool BanShout;
        /// <summary>
        /// 拒绝行会聊天
        /// </summary>
        public bool BanGuildChat;
        /// <summary>
        /// 是不允许交易
        /// </summary>
        internal bool AllowDeal;
        /// <summary>
        /// 检查重叠人物使用
        /// </summary>
        public bool BoDuplication;
        /// <summary>
        /// 检查重叠人物间隔
        /// </summary>
        public int DupStartTick = 0;
        /// <summary>
        /// 是否用了神水
        /// </summary>
        protected bool UserUnLockDurg;
        /// <summary>
        /// 允许组队
        /// </summary>
        public bool AllowGroup;
        /// <summary>
        /// 允许加入行会
        /// </summary>        
        internal bool AllowGuild;
        /// <summary>
        /// 交易对象
        /// </summary>
        protected PlayObject DealCreat;
        /// <summary>
        /// 正在交易
        /// </summary>
        protected bool Dealing;
        /// <summary>
        /// 交易最后操作时间
        /// </summary>
        internal int DealLastTick = 0;
        /// <summary>
        /// 交易的金币数量
        /// </summary>
        public int DealGolds;
        /// <summary>
        /// 确认交易标志
        /// </summary>
        public bool DealSuccess = false;
        /// <summary>
        /// 回城地图
        /// </summary>
        public string HomeMap;
        /// <summary>
        /// 回城座标X
        /// </summary>
        public short HomeX = 0;
        /// <summary>
        /// 回城座标Y
        /// </summary>
        public short HomeY = 0;
        /// <summary>
        /// 记忆使用间隔
        /// </summary>
        public int GroupRcallTick;
        public short GroupRcallTime;
        /// <summary>
        /// 行会传送
        /// </summary>
        public bool GuildMove = false;
        public ClientMesaagePacket MDefMsg;
        /// <summary>
        /// 在行会占争地图中死亡次数
        /// </summary>
        public ushort FightZoneDieCount;
        /// <summary>
        /// 祈祷
        /// </summary>
        protected bool MBopirit = false;
        /// <summary>
        /// 野蛮冲撞间隔
        /// </summary>
        public int DoMotaeboTick = 0;
        protected bool CrsHitkill = false;
        public bool MBo43Kill = false;
        protected bool RedUseHalfMoon;
        protected bool UseThrusting;
        protected bool UseHalfMoon;
        /// <summary>
        /// 交易列表
        /// </summary>
        public IList<UserItem> DealItemList;
        /// <summary>
        /// 仓库物品列表
        /// </summary>
        internal readonly IList<UserItem> StorageItemList;
        /// <summary>
        /// 禁止私聊人员列表
        /// </summary>
        public IList<string> LockWhisperList;
        /// <summary>
        /// 力量物品(影响力量的物品)
        /// </summary>
        public bool BoPowerItem = false;
        public bool AllowGroupReCall;
        public int HungerStatus = 0;
        public int BonusPoint = 0;
        public byte BtB2;
        /// <summary>
        /// 人物攻击变色标志
        /// </summary>
        public bool PvpFlag;
        /// <summary>
        /// 减PK值时间`
        /// </summary>
        private int _decPkPointTick;
        /// <summary>
        /// 人物的PK值
        /// </summary>
        public int PkPoint;
        /// <summary>
        /// 人物攻击变色时间长度
        /// </summary>
        public int PvpNameColorTick;
        /// <summary>
        /// 是否在开行会战
        /// </summary>
        public bool InGuildWarArea;
        public GuildInfo MyGuild;
        public short GuildRankNo;
        public string GuildRankName = string.Empty;
        public string ScriptLable = string.Empty;
        public string MSOldSayMsg;
        public int MNSayMsgCount = 0;
        public int MDwSayMsgTick;
        public bool MBoDisableSayMsg;
        public int MDwDisableSayMsgTick;
        public int MDwCheckDupObjTick;
        public int DiscountForNightTick;
        public bool MBoInSafeArea;
        /// <summary>
        /// 喊话消息间隔
        /// </summary>
        protected int ShoutMsgTick;
        protected bool SmashSet = false;
        protected bool HwanDevilSet = false;
        protected bool PuritySet = false;
        protected bool MundaneSet = false;
        protected bool NokChiSet = false;
        protected bool TaoBuSet = false;
        protected bool FiveStringSet = false;
        public byte MBtValNpcType;
        public byte MBtValType;
        public byte MBtValLabel;
        /// <summary>
        /// 掉物品
        /// </summary>
        public bool NoDropItem = false;
        /// <summary>
        /// 探测项链使用间隔
        /// </summary>
        public int ProbeTick;
        /// <summary>
        /// 传送戒指使用间隔
        /// </summary>
        public int TeleportTick;
        protected int DecHungerPointTick;
        /// <summary>
        /// 气血石
        /// </summary>
        protected int AutoAddHpmpMode = 0;
        public int CheckHpmpTick = 0;
        public int KickOffLineTick = 0;
        /// <summary>
        /// 挂机
        /// </summary>
        public bool OffLineFlag = false;
        /// <summary>
        /// 挂机字符
        /// </summary>
        public string MsOffLineLeaveword = string.Empty;
        public int MNSocket = 0;
        /// <summary>
        /// 人物连接到游戏网关SOCKETID
        /// </summary>
        public ushort MNGSocketIdx = 0;
        /// <summary>
        /// 人物所在网关号
        /// </summary>
        public byte MNGateIdx = 0;
        public int MNSoftVersionDate = 0;
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime MDLogonTime;
        /// <summary>
        /// 战领沙城时间
        /// </summary>
        public int MDwLogonTick;
        /// <summary>
        /// 是否进入游戏完成
        /// </summary>
        public bool MBoReadyRun;
        /// <summary>
        /// 当前会话ID
        /// </summary>
        public int MNSessionId = 0;
        /// <summary>
        /// 人物当前付费模式
        /// 1:试玩
        /// 2:付费
        /// 3:测试
        /// </summary>
        public byte PayMent;
        public byte MNPayMode = 0;
        /// <summary>
        /// 全局会话信息
        /// </summary>
        public TSessInfo MSessInfo;
        public int MDwLoadTick = 0;
        /// <summary>
        /// 人物当前所在服务器序号
        /// </summary>
        public byte ServerIndex = 0;
        public bool MBoEmergencyClose;
        /// <summary>
        /// 掉线标志
        /// </summary>
        public bool MBoSoftClose;
        /// <summary>
        /// 断线标志(@kick 命令)
        /// </summary>
        public bool MBoKickFlag;
        /// <summary>
        /// 是否重连
        /// </summary>
        public bool MBoReconnection;
        public bool MBoRcdSaved;
        public bool MBoSwitchData;
        public bool MBoSwitchDataOk = false;
        public string MSSwitchDataTempFile = string.Empty;
        public int MNWriteChgDataErrCount;
        public string MSSwitchMapName = string.Empty;
        public short MNSwitchMapX = 0;
        public short MNSwitchMapY = 0;
        public bool MBoSwitchDataSended;
        public int MDwChgDataWritedTick = 0;
        /// <summary>
        /// 攻击间隔
        /// </summary>
        public int MDwHitIntervalTime;
        /// <summary>
        /// 魔法间隔
        /// </summary>
        public int MDwMagicHitIntervalTime;
        /// <summary>
        /// 走路间隔
        /// </summary>
        public int MDwRunIntervalTime;
        /// <summary>
        /// 走路间隔
        /// </summary>
        public int MDwWalkIntervalTime;
        /// <summary>
        /// 换方向间隔
        /// </summary>
        public int MDwTurnIntervalTime;
        /// <summary>
        /// 组合操作间隔
        /// </summary>
        public int MDwActionIntervalTime;
        /// <summary>
        /// 移动刺杀间隔
        /// </summary>
        public int MDwRunLongHitIntervalTime;
        /// <summary>
        /// 跑位攻击间隔
        /// </summary>        
        public int MDwRunHitIntervalTime;
        /// <summary>
        /// 走位攻击间隔
        /// </summary>        
        public int MDwWalkHitIntervalTime;
        /// <summary>
        /// 跑位魔法间隔
        /// </summary>        
        public int MDwRunMagicIntervalTime;
        /// <summary>
        /// 魔法攻击时间
        /// </summary>        
        public int MDwMagicAttackTick;
        /// <summary>
        /// 魔法攻击间隔时间
        /// </summary>
        public int MDwMagicAttackInterval;
        /// <summary>
        /// 攻击时间
        /// </summary>
        public int MDwAttackTick;
        /// <summary>
        /// 人物跑动时间
        /// </summary>
        public int MDwMoveTick;
        /// <summary>
        /// 人物攻击计数
        /// </summary>
        public int MDwAttackCount;
        /// <summary>
        /// 人物攻击计数
        /// </summary>
        public int MDwAttackCountA;
        /// <summary>
        /// 魔法攻击计数
        /// </summary>
        public int MDwMagicAttackCount;
        /// <summary>
        /// 人物跑计数
        /// </summary>
        public int MDwMoveCount;
        /// <summary>
        /// 超速计数
        /// </summary>
        public int MNOverSpeedCount;
        /// <summary>
        /// 复活戒指
        /// </summary>
        internal bool Revival = false;
        /// <summary>
        /// 传送戒指
        /// </summary>
        public bool Teleport = false;
        /// <summary>
        /// 麻痹戒指
        /// </summary>
        internal bool Paralysis = false;
        /// <summary>
        /// 防麻痹
        /// </summary>
        internal bool UnParalysis = false;
        /// <summary>
        /// 火焰戒指
        /// </summary>
        private bool _flameRing = false;
        /// <summary>
        /// 治愈戒指
        /// </summary>
        private bool RecoveryRing;
        /// <summary>
        /// 未知戒指
        /// </summary>
        protected bool AngryRing = false;
        /// <summary>
        /// 护身戒指
        /// </summary>
        internal bool MagicShield = false;
        /// <summary>
        /// 防护身
        /// </summary>
        internal readonly bool UnMagicShield = false;
        /// <summary>
        /// 活力戒指
        /// </summary>
        private bool _muscleRing = false;
        /// <summary>
        /// 探测项链
        /// </summary>
        public bool ProbeNecklace = false;
        /// <summary>
        /// 防复活
        /// </summary>
        internal readonly bool UnRevival = false;
        /// <summary>
        /// 记忆全套
        /// </summary>
        public bool RecallSuite;
        /// <summary>
        /// 魔血一套
        /// </summary>
        protected int MoXieSuite;
        /// <summary>
        /// 虹魔一套
        /// </summary>
        internal int SuckupEnemyHealthRate;
        internal double SuckupEnemyHealth;
        public double BodyLuck;
        public int BodyLuckLevel;
        public bool MBoDieInFight3Zone;
        public string MSGotoNpcLabel = string.Empty;
        public bool TakeDlgItem = false;
        public int DlgItemIndex = 0;
        public int MNDelayCall;
        public int MDwDelayCallTick = 0;
        public bool MBoDelayCall;
        public int MDelayCallNpc;
        public string DelayCallLabel = string.Empty;
        public TScript MScript;
        public BaseObject LastNpc = null;
        /// <summary>
        /// 职业属性点
        /// </summary>
        public NakedAbility BonusAbil;
        /// <summary>
        /// 玩家的变量P
        /// </summary>
        public int[] MNVal;
        /// <summary>
        /// 玩家的变量M
        /// </summary>
        public int[] MNMval;
        /// <summary>
        /// 玩家的变量D
        /// </summary>
        public int[] MDyVal;
        /// <summary>
        /// 玩家的变量
        /// </summary>
        public string[] MNSval;
        /// <summary>
        /// 人物变量  N
        /// </summary>
        public int[] MNInteger;
        /// <summary>
        /// 人物变量  S
        /// </summary>
        public string[] MSString;
        /// <summary>
        /// 服务器变量 W 0-20 个人服务器数值变量，不可保存，不可操作
        /// </summary>
        public string[] MServerStrVal;
        /// <summary>
        /// E 0-20 个人服务器字符串变量，不可保存，不可操作
        /// </summary>
        public int[] MServerIntVal;
        /// <summary>
        /// 技能表
        /// </summary>
        public readonly IList<UserMagic> MagicList;
        public PlayObject GroupOwner;
        /// <summary>
        /// 组成员
        /// </summary>
        public IList<PlayObject> GroupMembers;
        public string MSPlayDiceLabel = string.Empty;
        public bool MBoTimeRecall;
        public int MDwTimeRecallTick = 0;
        public string MSMoveMap = string.Empty;
        public short MNMoveX;
        public short MNMoveY;
        /// <summary>
        /// 保存人物数据时间间隔
        /// </summary>
        public int MDwSaveRcdTick;
        public byte MBtBright;
        public bool MBoNewHuman;
        public bool MBoSendNotice;
        public int MDwWaitLoginNoticeOkTick;
        public bool MBoLoginNoticeOk;
        public bool Bo6Ab;
        public int ShowLineNoticeTick;
        public int ShowLineNoticeIdx;
        public int SoftVersionDateEx;
        /// <summary>
        /// 可点击脚本标签字典
        /// </summary>
        private readonly Hashtable CanJmpScriptLableMap;
        public int ScriptGotoCount = 0;
        public string ScriptCurrLable = string.Empty;
        // 用于处理 @back 脚本命令
        public string ScriptGoBackLable = string.Empty;
        // 用于处理 @back 脚本命令
        public int MDwTurnTick;
        public int MWOldIdent = 0;
        public byte MBtOldDir = 0;
        /// <summary>
        /// 第一个操作
        /// </summary>
        public bool MBoFirstAction = false;
        /// <summary>
        /// 二次操作之间间隔时间
        /// </summary>        
        public int MDwActionTick;
        /// <summary>
        /// 配偶名称
        /// </summary>
        public string MSDearName;
        public PlayObject MDearHuman;
        /// <summary>
        /// 是否允许夫妻传送
        /// </summary>
        public bool MBoCanDearRecall;
        public bool MBoCanMasterRecall;
        /// <summary>
        /// 夫妻传送时间
        /// </summary>
        public int MDwDearRecallTick;
        public int MDwMasterRecallTick;
        /// <summary>
        /// 师徒名称
        /// </summary>
        public string MSMasterName;
        public PlayObject MMasterHuman;
        public IList<PlayObject> MMasterList;
        public bool MBoMaster = false;
        /// <summary>
        /// 声望点
        /// </summary>
        public byte MBtCreditPoint = 0;
        /// <summary>
        /// 离婚次数
        /// </summary>        
        public byte MBtMarryCount = 0;
        /// <summary>
        /// 转生等级
        /// </summary>
        public byte MBtReLevel = 0;
        public byte MBtReColorIdx;
        public int MDwReColorTick = 0;
        /// <summary>
        /// 杀怪经验倍数
        /// </summary>
        public int MNKillMonExpMultiple;
        /// <summary>
        /// 处理消息循环时间控制
        /// </summary>        
        public int MDwGetMsgTick = 0;
        public bool MBoSetStoragePwd;
        public bool MBoReConfigPwd;
        public bool MBoCheckOldPwd;
        public bool MBoUnLockPwd;
        public bool MBoUnLockStoragePwd;
        public bool MBoPasswordLocked;
        // 锁密码
        public byte MBtPwdFailCount;
        /// <summary>
        /// 是否启用锁登录功能
        /// </summary>
        public bool MBoLockLogon;
        /// <summary>
        /// 是否打开登录锁
        /// </summary>        
        public bool MBoLockLogoned;
        public string MSTempPwd;
        public string MSStoragePwd;
        public BaseObject MPoseBaseObject = null;
        public bool MBoStartMarry = false;
        public bool MBoStartMaster = false;
        public bool MBoStartUnMarry = false;
        public bool MBoStartUnMaster = false;
        /// <summary>
        /// 禁止发方字(发的文字只能自己看到)
        /// </summary>
        public bool FilterSendMsg;
        /// <summary>
        /// 杀怪经验倍数(此数除以 100 为真正倍数)
        /// </summary>        
        public int KillMonExpRate;
        /// <summary>
        /// 人物攻击力倍数(此数除以 100 为真正倍数)
        /// </summary>        
        public int PowerRate;
        public int KillMonExpRateTime = 0;
        public int PowerRateTime = 0;
        public int ExpRateTick;
        /// <summary>
        /// 技巧项链
        /// </summary>
        private bool FastTrain = false;
        /// <summary>
        /// 是否允许使用物品
        /// </summary>
        public bool MBoCanUseItem;
        public bool MBoCanDeal;
        public bool MBoCanDrop;
        public bool MBoCanGetBackItem;
        public bool MBoCanWalk;
        public bool MBoCanRun;
        public bool MBoCanHit;
        public bool MBoCanSpell;
        public bool MBoCanSendMsg;
        public int MNMemberType;
        // 会员类型
        public int MNMemberLevel;
        // 会员等级
        public bool MBoSendMsgFlag;
        // 发祝福语标志
        public bool MBoChangeItemNameFlag;
        /// <summary>
        /// 游戏币
        /// </summary>
        public int MNGameGold;
        /// <summary>
        /// 是否自动减游戏币
        /// </summary>        
        public bool MBoDecGameGold;
        public int MDwDecGameGoldTime;
        public int MDwDecGameGoldTick;
        public int MNDecGameGold;
        // 一次减点数
        public bool MBoIncGameGold;
        // 是否自动加游戏币
        public int MDwIncGameGoldTime;
        public int MDwIncGameGoldTick;
        public int MNIncGameGold;
        // 一次减点数
        public int MNGamePoint;
        // 游戏点数
        public int MDwIncGamePointTick;
        public int MNPayMentPoint;
        public int MDwPayMentPointTick = 0;
        public int MDwDecHpTick = 0;
        public int MDwIncHpTick = 0;
        /// <summary>
        /// PK 死亡掉经验，不够经验就掉等级
        /// </summary>
        private int PkDieLostExp;
        /// <summary>
        /// PK 死亡掉等级
        /// </summary>
        private byte PkDieLostLevel;
        public PlayObject MGetWhisperHuman;
        /// <summary>
        /// 清理无效对象间隔
        /// </summary>
        public int ClearInvalidObjTick = 0;
        public short MWContribution;
        // 贡献度
        public string MSRankLevelName = string.Empty;
        // 显示名称格式串
        public bool MBoFilterAction = false;
        public int MDwAutoGetExpTick;
        public int MNAutoGetExpTime = 0;
        public int MNAutoGetExpPoint;
        public Envirnoment MAutoGetExpEnvir;
        public bool MBoAutoGetExpInSafeZone = false;
        public Dictionary<string, TDynamicVar> DynamicVarMap;
        public short MDwClientTick;
        /// <summary>
        /// 进入速度测试模式
        /// </summary>
        public bool MBoTestSpeedMode;
        public int MDwDelayTime = 0;
        public string MSRandomNo = string.Empty;
        public int MDwQueryBagItemsTick = 0;
        public bool MBoTimeGoto;
        public int MDwTimeGotoTick;
        public string MSTimeGotoLable;
        public BaseObject MTimeGotoNpc;
        /// <summary>
        /// 个人定时器
        /// </summary>
        public int[] AutoTimerTick;
        /// <summary>
        /// 个人定时器 时间间隔
        /// </summary>
        public int[] AutoTimerStatus;
        /// <summary>
        /// 0-攻击力增加 1-魔法增加  2-道术增加(无极真气) 3-攻击速度 4-HP增加(酒气护体)
        /// 5-增加MP上限 6-减攻击力 7-减魔法 8-减道术 9-减HP 10-减MP 11-敏捷 12-增加防
        /// 13-增加魔防 14-增加道术上限(虎骨酒) 15-连击伤害增加(醉八打) 16-内力恢复速度增加(何首养气酒)
        /// 17-内力瞬间恢复增加(何首凝神酒) 18-增加斗转上限(培元酒) 19-不死状态 21-弟子心法激活
        /// 22-移动减速 23-定身(十步一杀)
        /// </summary>
        internal ushort[] ExtraAbil;
        internal byte[] ExtraAbilFlag;
        /// <summary>
        /// 0-攻击力增加 1-魔法增加  2-道术增加(无极真气) 3-攻击速度 4-HP增加(酒气护体)
        /// 5-增加MP上限 6-减攻击力 7-减魔法 8-减道术 9-减HP 10-减MP 11-敏捷 12-增加防
        /// 13-增加魔防 14-增加道术上限(虎骨酒) 15-连击伤害增加(醉八打) 16-内力恢复速度增加(何首养气酒)
        /// 17-内力瞬间恢复增加(何首凝神酒) 18-增加斗转上限(培元酒) 19-不死状态 20-道术+上下限(除魔药剂类) 21-弟子心法激活
        /// 22-移动减速 23-定身(十步一杀)
        /// </summary>
        internal int[] ExtraAbilTimes;
        /// <summary>
        /// 包裹刷新时间
        /// </summary>
        public int MDwClickNpcTime = 0;
        /// <summary>
        /// 是否开通元宝交易服务
        /// </summary>
        public bool BoYbdeal;
        /// <summary>
        /// 确认元宝寄售标志
        /// </summary>
        public bool MBoSellOffOk = false;
        /// <summary>
        /// 元宝寄售物品列表
        /// </summary>
        private IList<UserItem> SellOffItemList;
        public byte[] QuestUnitOpen;
        public byte[] QuestUnit;
        public byte[] QuestFlag;

        public PlayObject()
        {
            Race = ActorRace.Play;
            Hair = 0;
            Job = PlayJob.Warrior;
            HomeMap = "0";
            DealGolds = 0;
            DealItemList = new List<UserItem>();
            StorageItemList = new List<UserItem>();
            LockWhisperList = new List<string>();
            MBoEmergencyClose = false;
            MBoSwitchData = false;
            MBoReconnection = false;
            MBoKickFlag = false;
            MBoSoftClose = false;
            MBoReadyRun = false;
            MDwSaveRcdTick = HUtil32.GetTickCount();
            DecHungerPointTick = HUtil32.GetTickCount();
            GroupRcallTick = HUtil32.GetTickCount();
            WantRefMsg = true;
            MBoRcdSaved = false;
            MBoDieInFight3Zone = false;
            MNDelayCall = 0;
            MBoDelayCall = false;
            MDelayCallNpc = 0;
            MScript = null;
            MBoTimeRecall = false;
            MSMoveMap = "";
            MNMoveX = 0;
            MNMoveY = 0;
            RunTick = HUtil32.GetTickCount();
            RunTime = 250;
            SearchTime = 1000;
            SearchTick = HUtil32.GetTickCount();
            AllowGroup = false;
            AllowGuild = false;
            ViewRange = 12;
            InGuildWarArea = false;
            MBoNewHuman = false;
            MBoLoginNoticeOk = false;
            Bo6Ab = false;
            BonusAbil = new NakedAbility();
            AccountExpired = false;
            MBoSendNotice = false;
            MDwCheckDupObjTick = HUtil32.GetTickCount();
            DiscountForNightTick = HUtil32.GetTickCount();
            MBoInSafeArea = false;
            MDwMagicAttackTick = HUtil32.GetTickCount();
            MDwMagicAttackInterval = 0;
            MDwAttackTick = HUtil32.GetTickCount();
            MDwMoveTick = HUtil32.GetTickCount();
            MDwTurnTick = HUtil32.GetTickCount();
            MDwActionTick = HUtil32.GetTickCount();
            MDwAttackCount = 0;
            MDwAttackCountA = 0;
            MDwMagicAttackCount = 0;
            MDwMoveCount = 0;
            MNOverSpeedCount = 0;
            MSOldSayMsg = "";
            MDwSayMsgTick = HUtil32.GetTickCount();
            MBoDisableSayMsg = false;
            MDwDisableSayMsgTick = HUtil32.GetTickCount();
            MDLogonTime = DateTime.Now;
            MDwLogonTick = HUtil32.GetTickCount();
            MBoSwitchData = false;
            MBoSwitchDataSended = false;
            MNWriteChgDataErrCount = 0;
            ShowLineNoticeTick = HUtil32.GetTickCount();
            ShowLineNoticeIdx = 0;
            SoftVersionDateEx = 0;
            CanJmpScriptLableMap = new Hashtable(StringComparer.OrdinalIgnoreCase);
            MagicList = new List<UserMagic>();
            MNKillMonExpMultiple = 1;
            KillMonExpRate = 100;
            ExpRateTick = HUtil32.GetTickCount();
            PowerRate = 100;
            MBoSetStoragePwd = false;
            MBoReConfigPwd = false;
            MBoCheckOldPwd = false;
            MBoUnLockPwd = false;
            MBoUnLockStoragePwd = false;
            MBoPasswordLocked = false;
            PvpFlag = false;
            // 锁仓库
            MBtPwdFailCount = 0;
            MSTempPwd = "";
            MSStoragePwd = "";
            FilterSendMsg = false;
            MBoCanDeal = true;
            MBoCanDrop = true;
            MBoCanGetBackItem = true;
            MBoCanWalk = true;
            MBoCanRun = true;
            MBoCanHit = true;
            MBoCanSpell = true;
            MBoCanUseItem = true;
            MNMemberType = 0;
            MNMemberLevel = 0;
            MNGameGold = 0;
            MBoDecGameGold = false;
            MNDecGameGold = 1;
            MDwDecGameGoldTick = HUtil32.GetTickCount();
            MDwDecGameGoldTime = 60 * 1000;
            MBoIncGameGold = false;
            MNIncGameGold = 1;
            MDwIncGameGoldTick = HUtil32.GetTickCount();
            MDwIncGameGoldTime = 60 * 1000;
            MNGamePoint = 0;
            MDwIncGamePointTick = HUtil32.GetTickCount();
            MNPayMentPoint = 0;
            MDearHuman = null;
            MMasterHuman = null;
            MMasterList = new List<PlayObject>();
            MBoSendMsgFlag = false;
            MBoChangeItemNameFlag = false;
            MBoCanMasterRecall = false;
            MBoCanDearRecall = false;
            MDwDearRecallTick = HUtil32.GetTickCount();
            MDwMasterRecallTick = HUtil32.GetTickCount();
            MBtReColorIdx = 0;
            MGetWhisperHuman = null;
            OnHorse = false;
            MWContribution = 0;
            MSRankLevelName = M2Share.g_sRankLevelName;
            FixedHideMode = true;
            MNVal = new int[100];
            MNMval = new int[100];
            MDyVal = new int[100];
            MNSval = new string[100];
            MNInteger = new int[100];
            MSString = new string[100];
            MServerStrVal = new string[100];
            MServerIntVal = new int[100];
            ExtraAbil = new ushort[7];
            ExtraAbilTimes = new int[7];
            ExtraAbilFlag = new byte[7];
            HearWhisper = true;
            BanShout = true;
            BanGuildChat = true;
            AllowDeal = true;
            MDwAutoGetExpTick = HUtil32.GetTickCount();
            _decPkPointTick = HUtil32.GetTickCount();
            MNAutoGetExpPoint = 0;
            MAutoGetExpEnvir = null;
            MDwHitIntervalTime = M2Share.Config.HitIntervalTime;// 攻击间隔
            MDwMagicHitIntervalTime = M2Share.Config.MagicHitIntervalTime;// 魔法间隔
            MDwRunIntervalTime = M2Share.Config.RunIntervalTime;// 走路间隔
            MDwWalkIntervalTime = M2Share.Config.WalkIntervalTime;// 走路间隔
            MDwTurnIntervalTime = M2Share.Config.TurnIntervalTime;// 换方向间隔
            MDwActionIntervalTime = M2Share.Config.ActionIntervalTime;// 组合操作间隔
            MDwRunLongHitIntervalTime = M2Share.Config.RunLongHitIntervalTime;// 组合操作间隔
            MDwRunHitIntervalTime = M2Share.Config.RunHitIntervalTime;// 组合操作间隔
            MDwWalkHitIntervalTime = M2Share.Config.WalkHitIntervalTime;// 组合操作间隔
            MDwRunMagicIntervalTime = M2Share.Config.RunMagicIntervalTime;// 跑位魔法间隔
            DynamicVarMap = new Dictionary<string, TDynamicVar>(StringComparer.OrdinalIgnoreCase);
            MSessInfo = null;
            MBoTestSpeedMode = false;
            MBoLockLogon = true;
            MBoLockLogoned = false;
            MBoTimeGoto = false;
            MDwTimeGotoTick = HUtil32.GetTickCount();
            MSTimeGotoLable = "";
            MTimeGotoNpc = null;
            AutoTimerTick = new int[20];
            AutoTimerStatus = new int[20];
            MapCell = CellType.Play;
            QueryExpireTick = 60 * 1000;
            AccountExpiredTick = HUtil32.GetTickCount();
            GoldMax = M2Share.Config.HumanMaxGold;
            QuestUnitOpen = new byte[128];
            QuestUnit = new byte[128];
            QuestFlag = new byte[128];
            GroupMembers = new List<PlayObject>();
            MSRandomNo = M2Share.RandomNumber.Random(999999).ToString();
        }

        private void SendNotice()
        {
            var loadList = new List<string>();
            M2Share.NoticeMgr.GetNoticeMsg("Notice", loadList);
            var sNoticeMsg = string.Empty;
            if (loadList.Count > 0)
            {
                for (var i = 0; i < loadList.Count; i++)
                {
                    sNoticeMsg = sNoticeMsg + loadList[i] + "\x20\x1B";
                }
            }
            SendDefMessage(Grobal2.SM_SENDNOTICE, 2000, 0, 0, 0, sNoticeMsg.Replace("/r/n/r/n ", ""));
        }

        public void RunNotice()
        {
            ProcessMessage msg;
            const string sExceptionMsg = "[Exception] TPlayObject::RunNotice";
            if (MBoEmergencyClose || MBoKickFlag || MBoSoftClose)
            {
                if (MBoKickFlag)
                {
                    SendDefMessage(Grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0, "");
                }
                MakeGhost();
            }
            else
            {
                try
                {
                    if (!MBoSendNotice)
                    {
                        SendNotice();
                        MBoSendNotice = true;
                        MDwWaitLoginNoticeOkTick = HUtil32.GetTickCount();
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - MDwWaitLoginNoticeOkTick) > 10 * 1000)
                        {
                            MBoEmergencyClose = true;
                        }
                        while (GetMessage(out msg))
                        {
                            if (msg.wIdent == Grobal2.CM_LOGINNOTICEOK)
                            {
                                MBoLoginNoticeOk = true;
                                MDwClientTick = (short)msg.nParam1;
                                SysMsg(MDwClientTick.ToString(), MsgColor.Red, MsgType.Notice);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    M2Share.Log.Error(sExceptionMsg);
                }
            }
        }

        private void SendLogon()
        {
            var messageBodyWl = new MessageBodyWL();
            MDefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_LOGON, ActorId, CurrX, CurrY, HUtil32.MakeWord(Direction, Light));
            messageBodyWl.Param1 = GetFeatureToLong();
            messageBodyWl.Param2 = CharStatus;
            if (AllowGroup)
            {
                messageBodyWl.Tag1 = HUtil32.MakeLong(HUtil32.MakeWord(1, 0), GetFeatureEx());
            }
            else
            {
                messageBodyWl.Tag1 = 0;
            }
            messageBodyWl.Tag2 = 0;
            SendSocket(MDefMsg, EDCode.EncodeBuffer(messageBodyWl));
            var nRecog = GetFeatureToLong();
            SendDefMessage(Grobal2.SM_FEATURECHANGED, ActorId, HUtil32.LoWord(nRecog), HUtil32.HiWord(nRecog), GetFeatureEx(), "");
            SendDefMessage(Grobal2.SM_ATTACKMODE, (byte)AttatckMode, 0, 0, 0, "");
        }

        /// <summary>
        /// 玩家登录
        /// </summary>
        public void UserLogon()
        {
            var sIPaddr = "127.0.0.1";
            const string sExceptionMsg = "[Exception] TPlayObject::UserLogon";
            const string sCheckIPaddrFail = "登录IP地址不匹配!!!";
            try
            {
                if (M2Share.Config.TestServer)
                {
                    if (Abil.Level < M2Share.Config.TestLevel)
                    {
                        Abil.Level = (byte)M2Share.Config.TestLevel;
                    }
                    if (Gold < M2Share.Config.TestGold)
                    {
                        Gold = M2Share.Config.TestGold;
                    }
                }
                if (M2Share.Config.TestServer || M2Share.Config.ServiceMode)
                {
                    PayMent = 3;
                }
                MapMoveTick = HUtil32.GetTickCount();
                MDLogonTime = DateTime.Now;
                MDwLogonTick = HUtil32.GetTickCount();
                Initialize();
                SendMsg(this, Grobal2.RM_LOGON, 0, 0, 0, 0, "");
                if (Abil.Level <= 7)
                {
                    if (GetRangeHumanCount() >= 80)
                    {
                        MapRandomMove(Envir.MapName, 0);
                    }
                }
                if (MBoDieInFight3Zone)
                {
                    MapRandomMove(Envir.MapName, 0);
                }
                if (M2Share.WorldEngine.GetHumPermission(ChrName, ref sIPaddr, ref Permission))
                {
                    if (M2Share.Config.PermissionSystem)
                    {
                        if (!M2Share.CompareIPaddr(LoginIpAddr, sIPaddr))
                        {
                            SysMsg(sCheckIPaddrFail, MsgColor.Red, MsgType.Hint);
                            MBoEmergencyClose = true;
                        }
                    }
                }
                GetStartPoint();
                for (var i = MagicList.Count - 1; i >= 0; i--)
                {
                    CheckSeeHealGauge(MagicList[i]);
                }
                UserItem userItem;
                if (MBoNewHuman)
                {
                    userItem = new UserItem();
                    if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.Candle, ref userItem))
                    {
                        ItemList.Add(userItem);
                    }
                    else
                    {
                        Dispose(userItem);
                    }
                    userItem = new UserItem();
                    if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.BasicDrug, ref userItem))
                    {
                        ItemList.Add(userItem);
                    }
                    else
                    {
                        Dispose(userItem);
                    }
                    userItem = new UserItem();
                    if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.WoodenSword, ref userItem))
                    {
                        ItemList.Add(userItem);
                    }
                    else
                    {
                        Dispose(userItem);
                    }
                    userItem = new UserItem();
                    var sItem = Gender == PlayGender.Man
                        ? M2Share.Config.ClothsMan
                        : M2Share.Config.ClothsWoman;
                    if (M2Share.WorldEngine.CopyToUserItemFromName(sItem, ref userItem))
                    {
                        ItemList.Add(userItem);
                    }
                    else
                    {
                        Dispose(userItem);
                    }
                }
                // 检查背包中的物品是否合法
                for (var i = ItemList.Count - 1; i >= 0; i--)
                {
                    if (!string.IsNullOrEmpty(M2Share.WorldEngine.GetStdItemName(ItemList[i].Index))) continue;
                    Dispose(ItemList[i]);
                    ItemList.RemoveAt(i);
                }
                // 检查人物身上的物品是否符合使用规则
                if (M2Share.Config.CheckUserItemPlace)
                {
                    for (var i = 0; i < UseItems.Length; i++)
                    {
                        if (UseItems[i] == null || UseItems[i].Index <= 0) continue;
                        var stdItem = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                        if (stdItem != null)
                        {
                            if (!M2Share.CheckUserItems(i, stdItem))
                            {
                                if (!AddItemToBag(UseItems[i]))
                                {
                                    ItemList.Insert(0, UseItems[i]);
                                }
                                UseItems[i].Index = 0;
                            }
                        }
                        else
                        {
                            UseItems[i].Index = 0;
                        }
                    }
                }
                // 检查背包中是否有复制品
                for (var i = ItemList.Count - 1; i >= 0; i--)
                {
                    var sItemName = M2Share.WorldEngine.GetStdItemName(ItemList[i].Index);
                    for (var j = i - 1; j >= 0; j--)
                    {
                        var userItem1 = ItemList[j];
                        if (M2Share.WorldEngine.GetStdItemName(userItem1.Index) == sItemName && ItemList[i].MakeIndex == userItem1.MakeIndex)
                        {
                            ItemList.RemoveAt(j);
                            break;
                        }
                    }
                }
                for (var i = 0; i < StatusArrTick.Length; i++)
                {
                    if (StatusArr[i] > 0)
                    {
                        StatusArrTick[i] = HUtil32.GetTickCount();
                    }
                }
                CharStatus = GetCharStatus();
                RecalcLevelAbilitys();
                RecalcAbilitys();
                Abil.MaxExp = GetLevelExp(Abil.Level);// 登录重新取得升级所需经验值
                WAbil.Exp = Abil.Exp;
                WAbil.MaxExp = Abil.MaxExp;
                if (BtB2 == 0)
                {
                    PkPoint = 0;
                    BtB2++;
                }
                if (Gold > M2Share.Config.HumanMaxGold * 2 && M2Share.Config.HumanMaxGold > 0)
                {
                    Gold = M2Share.Config.HumanMaxGold * 2;
                }
                if (!Bo6Ab)
                {
                    if (MNSoftVersionDate < M2Share.Config.SoftVersionDate) //登录版本号验证
                    {
                        SysMsg(M2Share.sClientSoftVersionError, MsgColor.Red, MsgType.Hint);
                        SysMsg(M2Share.sDownLoadNewClientSoft, MsgColor.Red, MsgType.Hint);
                        SysMsg(M2Share.sForceDisConnect, MsgColor.Red, MsgType.Hint);
                        MBoEmergencyClose = true;
                        return;
                    }
                    if (SoftVersionDateEx == 0 && M2Share.Config.boOldClientShowHiLevel)
                    {
                        SysMsg(M2Share.sClientSoftVersionTooOld, MsgColor.Blue, MsgType.Hint);
                        SysMsg(M2Share.sDownLoadAndUseNewClient, MsgColor.Red, MsgType.Hint);
                        if (!M2Share.Config.CanOldClientLogon)
                        {
                            SysMsg(M2Share.sClientSoftVersionError, MsgColor.Red, MsgType.Hint);
                            SysMsg(M2Share.sDownLoadNewClientSoft, MsgColor.Red, MsgType.Hint);
                            SysMsg(M2Share.sForceDisConnect, MsgColor.Red, MsgType.Hint);
                            MBoEmergencyClose = true;
                            return;
                        }
                    }
                    switch (AttatckMode)
                    {
                        case AttackMode.HAM_ALL:// [攻击模式: 全体攻击]
                            SysMsg(M2Share.sAttackModeOfAll, MsgColor.Green, MsgType.Hint);
                            break;
                        case AttackMode.HAM_PEACE:// [攻击模式: 和平攻击]
                            SysMsg(M2Share.sAttackModeOfPeaceful, MsgColor.Green, MsgType.Hint);
                            break;
                        case AttackMode.HAM_DEAR:// [攻击模式: 和平攻击]
                            SysMsg(M2Share.sAttackModeOfDear, MsgColor.Green, MsgType.Hint);
                            break;
                        case AttackMode.HAM_MASTER:// [攻击模式: 和平攻击]
                            SysMsg(M2Share.sAttackModeOfMaster, MsgColor.Green, MsgType.Hint);
                            break;
                        case AttackMode.HAM_GROUP:// [攻击模式: 编组攻击]
                            SysMsg(M2Share.sAttackModeOfGroup, MsgColor.Green, MsgType.Hint);
                            break;
                        case AttackMode.HAM_GUILD:// [攻击模式: 行会攻击]
                            SysMsg(M2Share.sAttackModeOfGuild, MsgColor.Green, MsgType.Hint);
                            break;
                        case AttackMode.HAM_PKATTACK:// [攻击模式: 红名攻击]
                            SysMsg(M2Share.sAttackModeOfRedWhite, MsgColor.Green, MsgType.Hint);
                            break;
                    }
                    SysMsg(M2Share.sStartChangeAttackModeHelp, MsgColor.Green, MsgType.Hint);// 使用组合快捷键 CTRL-H 更改攻击...
                    if (M2Share.Config.TestServer)
                    {
                        SysMsg(M2Share.sStartNoticeMsg, MsgColor.Green, MsgType.Hint);// 欢迎进入本服务器进行游戏...
                    }
                    if (M2Share.WorldEngine.PlayObjectCount > M2Share.Config.TestUserLimit)
                    {
                        if (Permission < 2)
                        {
                            SysMsg(M2Share.sOnlineUserFull, MsgColor.Red, MsgType.Hint);
                            SysMsg(M2Share.sForceDisConnect, MsgColor.Red, MsgType.Hint);
                            MBoEmergencyClose = true;
                        }
                    }
                }
                MBtBright = (byte)M2Share.g_nGameTime;
                SendMsg(this, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                SendMsg(this, Grobal2.RM_SUBABILITY, 0, 0, 0, 0, "");
                SendMsg(this, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                SendMsg(this, Grobal2.RM_DAYCHANGING, 0, 0, 0, 0, "");
                SendMsg(this, Grobal2.RM_SENDUSEITEMS, 0, 0, 0, 0, "");
                SendMsg(this, Grobal2.RM_SENDMYMAGIC, 0, 0, 0, 0, "");
                MyGuild = M2Share.GuildMgr.MemberOfGuild(ChrName);
                if (MyGuild != null)
                {
                    GuildRankName = MyGuild.GetRankName(this, ref GuildRankNo);
                    for (var i = MyGuild.GuildWarList.Count - 1; i >= 0; i--)
                    {
                        SysMsg(MyGuild.GuildWarList[i] + " 正在与本行会进行行会战.", MsgColor.Green, MsgType.Hint);
                    }
                }
                RefShowName();
                if (PayMent == 1)
                {
                    if (!Bo6Ab)
                    {
                        SysMsg(M2Share.sYouNowIsTryPlayMode, MsgColor.Red, MsgType.Hint);
                    }
                    GoldMax = M2Share.Config.HumanTryModeMaxGold;
                    if (Abil.Level > M2Share.Config.TryModeLevel)
                    {
                        SysMsg("测试状态可以使用到第 " + M2Share.Config.TryModeLevel, MsgColor.Red, MsgType.Hint);
                        SysMsg("链接中断，请到以下地址获得收费相关信息。(http://www.mir2.com)", MsgColor.Red, MsgType.Hint);
                        MBoEmergencyClose = true;
                    }
                }
                if (PayMent == 3 && !Bo6Ab)
                {
                    SysMsg(M2Share.g_sNowIsFreePlayMode, MsgColor.Green, MsgType.Hint);
                }
                if (M2Share.Config.VentureServer)
                {
                    SysMsg("当前服务器运行于冒险模式.", MsgColor.Green, MsgType.Hint);
                }
                if (MagicArr[MagicConst.SKILL_ERGUM] != null && !UseThrusting)
                {
                    UseThrusting = true;
                    SendSocket("+LNG");
                }
                if (Envir.Flag.boNORECONNECT)
                {
                    MapRandomMove(Envir.Flag.sNoReConnectMap, 0);
                }
                if (CheckDenyLogon())// 如果人物在禁止登录列表里则直接掉线而不执行下面内容
                {
                    return;
                }
                if (M2Share.g_ManageNPC != null)
                {
                    M2Share.g_ManageNPC.GotoLable(this, "@Login", false);
                }
                FixedHideMode = false;
                if (!string.IsNullOrEmpty(MSDearName))
                {
                    CheckMarry();
                }
                CheckMaster();
                FilterSendMsg = M2Share.GetDisableSendMsgList(ChrName);
                // 密码保护系统
                if (M2Share.Config.PasswordLockSystem)
                {
                    if (MBoPasswordLocked)
                    {
                        MBoCanGetBackItem = !M2Share.Config.LockGetBackItemAction;
                    }
                    if (M2Share.Config.LockHumanLogin && MBoLockLogon && MBoPasswordLocked)
                    {
                        MBoCanDeal = !M2Share.Config.LockDealAction;
                        MBoCanDrop = !M2Share.Config.LockDropAction;
                        MBoCanUseItem = !M2Share.Config.LockUserItemAction;
                        MBoCanWalk = !M2Share.Config.LockWalkAction;
                        MBoCanRun = !M2Share.Config.LockRunAction;
                        MBoCanHit = !M2Share.Config.LockHitAction;
                        MBoCanSpell = !M2Share.Config.LockSpellAction;
                        MBoCanSendMsg = !M2Share.Config.LockSendMsgAction;
                        ObMode = M2Share.Config.LockInObModeAction;
                        AdminMode = M2Share.Config.LockInObModeAction;
                        SysMsg(M2Share.g_sActionIsLockedMsg + " 开锁命令: @" + CommandMgr.Commands.LockLogon.CmdName, MsgColor.Red, MsgType.Hint);
                        SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, M2Share.g_sActionIsLockedMsg + "\\ \\" + "密码命令: @" + CommandMgr.Commands.PasswordLock.CmdName);
                    }
                    if (!MBoPasswordLocked)
                    {
                        SysMsg(Format(M2Share.g_sPasswordNotSetMsg, CommandMgr.Commands.PasswordLock.CmdName), MsgColor.Red, MsgType.Hint);
                    }
                    if (!MBoLockLogon && MBoPasswordLocked)
                    {
                        SysMsg(Format(M2Share.g_sNotPasswordProtectMode, CommandMgr.Commands.LockLogon.CmdName), MsgColor.Red, MsgType.Hint);
                    }
                    SysMsg(M2Share.g_sActionIsLockedMsg + " 开锁命令: @" + CommandMgr.Commands.Unlock.CmdName, MsgColor.Red, MsgType.Hint);
                    SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, M2Share.g_sActionIsLockedMsg + "\\ \\" + "开锁命令: @" + CommandMgr.Commands.Unlock.CmdName + '\\' + "加锁命令: @" + CommandMgr.Commands.Lock.CmdName + '\\' + "设置密码命令: @" + CommandMgr.Commands.SetPassword.CmdName + '\\' + "修改密码命令: @" + CommandMgr.Commands.ChgPassword.CmdName);
                }
                // 重置泡点方面计时
                MDwIncGamePointTick = HUtil32.GetTickCount();
                MDwIncGameGoldTick = HUtil32.GetTickCount();
                MDwAutoGetExpTick = HUtil32.GetTickCount();
                GetSellOffGlod();// 检查是否有元宝寄售交易结束还没得到元宝
            }
            catch (Exception e)
            {
                M2Share.Log.Error(sExceptionMsg);
                M2Share.Log.Error(e.StackTrace);
            }
            // ReadAllBook();
        }

        /// <summary>
        /// 使用祝福油
        /// </summary>
        /// <returns></returns>
        private bool WeaptonMakeLuck()
        {
            if (UseItems[Grobal2.U_WEAPON] == null || UseItems[Grobal2.U_WEAPON].Index <= 0)
            {
                return false;
            }
            var nRand = 0;
            var stdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_WEAPON].Index);
            if (stdItem != null)
            {
                nRand = Math.Abs(HUtil32.HiByte(stdItem.DC) - HUtil32.LoByte(stdItem.DC)) / 5;
            }
            if (M2Share.RandomNumber.Random(M2Share.Config.WeaponMakeUnLuckRate) == 1)
            {
                MakeWeaponUnlock();
            }
            else
            {
                var boMakeLuck = false;
                if (UseItems[Grobal2.U_WEAPON].Desc[4] > 0)
                {
                    UseItems[Grobal2.U_WEAPON].Desc[4] -= 1;
                    SysMsg(M2Share.g_sWeaptonMakeLuck, MsgColor.Green, MsgType.Hint);
                    boMakeLuck = true;
                }
                else if (UseItems[Grobal2.U_WEAPON].Desc[3] < M2Share.Config.WeaponMakeLuckPoint1)
                {
                    UseItems[Grobal2.U_WEAPON].Desc[3]++;
                    SysMsg(M2Share.g_sWeaptonMakeLuck, MsgColor.Green, MsgType.Hint);
                    boMakeLuck = true;
                }
                else if (UseItems[Grobal2.U_WEAPON].Desc[3] < M2Share.Config.WeaponMakeLuckPoint2 && M2Share.RandomNumber.Random(nRand + M2Share.Config.WeaponMakeLuckPoint2Rate) == 1)
                {
                    UseItems[Grobal2.U_WEAPON].Desc[3]++;
                    SysMsg(M2Share.g_sWeaptonMakeLuck, MsgColor.Green, MsgType.Hint);
                    boMakeLuck = true;
                }
                else if (UseItems[Grobal2.U_WEAPON].Desc[3] < M2Share.Config.WeaponMakeLuckPoint3 && M2Share.RandomNumber.Random(nRand * M2Share.Config.WeaponMakeLuckPoint3Rate) == 1)
                {
                    UseItems[Grobal2.U_WEAPON].Desc[3]++;
                    SysMsg(M2Share.g_sWeaptonMakeLuck, MsgColor.Green, MsgType.Hint);
                    boMakeLuck = true;
                }
                if (Race == ActorRace.Play)
                {
                    RecalcAbilitys();
                    SendMsg(this, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                    SendMsg(this, Grobal2.RM_SUBABILITY, 0, 0, 0, 0, "");
                }
                if (!boMakeLuck)
                {
                    SysMsg(M2Share.g_sWeaptonNotMakeLuck, MsgColor.Green, MsgType.Hint);
                }
            }
            return true;
        }

        /// <summary>
        /// 修复武器
        /// </summary>
        /// <returns></returns>
        private bool RepairWeapon()
        {
            if (UseItems[Grobal2.U_WEAPON] == null)
            {
                return false;
            }
            var userItem = UseItems[Grobal2.U_WEAPON];
            if (userItem.Index <= 0 || userItem.DuraMax <= userItem.Dura)
            {
                return false;
            }
            userItem.DuraMax -= (ushort)((userItem.DuraMax - userItem.Dura) / M2Share.Config.RepairItemDecDura);
            var nDura = HUtil32._MIN(5000, userItem.DuraMax - userItem.Dura);
            if (nDura <= 0) return false;
            userItem.Dura += (ushort)nDura;
            SendMsg(this, Grobal2.RM_DURACHANGE, 1, userItem.Dura, userItem.DuraMax, 0, "");
            SysMsg(M2Share.g_sWeaponRepairSuccess, MsgColor.Green, MsgType.Hint);
            return true;
        }

        /// <summary>
        /// 特修武器
        /// </summary>
        /// <returns></returns>
        private bool SuperRepairWeapon()
        {
            if (UseItems[Grobal2.U_WEAPON] == null || UseItems[Grobal2.U_WEAPON].Index <= 0)
            {
                return false;
            }
            UseItems[Grobal2.U_WEAPON].Dura = UseItems[Grobal2.U_WEAPON].DuraMax;
            SendMsg(this, Grobal2.RM_DURACHANGE, 1, UseItems[Grobal2.U_WEAPON].Dura, UseItems[Grobal2.U_WEAPON].DuraMax, 0, "");
            SysMsg(M2Share.g_sWeaponRepairSuccess, MsgColor.Green, MsgType.Hint);
            return true;
        }

        private void WinLottery()
        {
            var nGold = 0;
            var nWinLevel = 0;
            var nRate = M2Share.RandomNumber.Random(M2Share.Config.WinLotteryRate);
            if (nRate >= M2Share.Config.WinLottery6Min && nRate <= M2Share.Config.WinLottery6Max)
            {
                if (M2Share.Config.WinLotteryCount < M2Share.Config.NoWinLotteryCount)
                {
                    nGold = M2Share.Config.WinLottery6Gold;
                    nWinLevel = 6;
                    M2Share.Config.WinLotteryLevel6++;
                }
            }
            else if (nRate >= M2Share.Config.WinLottery5Min && nRate <= M2Share.Config.WinLottery5Max)
            {
                if (M2Share.Config.WinLotteryCount < M2Share.Config.NoWinLotteryCount)
                {
                    nGold = M2Share.Config.WinLottery5Gold;
                    nWinLevel = 5;
                    M2Share.Config.WinLotteryLevel5++;
                }
            }
            else if (nRate >= M2Share.Config.WinLottery4Min && nRate <= M2Share.Config.WinLottery4Max)
            {
                if (M2Share.Config.WinLotteryCount < M2Share.Config.NoWinLotteryCount)
                {
                    nGold = M2Share.Config.WinLottery4Gold;
                    nWinLevel = 4;
                    M2Share.Config.WinLotteryLevel4++;
                }
            }
            else if (nRate >= M2Share.Config.WinLottery3Min && nRate <= M2Share.Config.WinLottery3Max)
            {
                if (M2Share.Config.WinLotteryCount < M2Share.Config.NoWinLotteryCount)
                {
                    nGold = M2Share.Config.WinLottery3Gold;
                    nWinLevel = 3;
                    M2Share.Config.WinLotteryLevel3++;
                }
            }
            else if (nRate >= M2Share.Config.WinLottery2Min && nRate <= M2Share.Config.WinLottery2Max)
            {
                if (M2Share.Config.WinLotteryCount < M2Share.Config.NoWinLotteryCount)
                {
                    nGold = M2Share.Config.WinLottery2Gold;
                    nWinLevel = 2;
                    M2Share.Config.WinLotteryLevel2++;
                }
            }
            else if (M2Share.Config.WinLottery1Min + M2Share.Config.WinLottery1Max == nRate)
            {
                if (M2Share.Config.WinLotteryCount < M2Share.Config.NoWinLotteryCount)
                {
                    nGold = M2Share.Config.WinLottery1Gold;
                    nWinLevel = 1;
                    M2Share.Config.WinLotteryLevel1++;
                }
            }
            if (nGold > 0)
            {
                switch (nWinLevel)
                {
                    case 1:
                        SysMsg(M2Share.g_sWinLottery1Msg, MsgColor.Green, MsgType.Hint);
                        break;
                    case 2:
                        SysMsg(M2Share.g_sWinLottery2Msg, MsgColor.Green, MsgType.Hint);
                        break;
                    case 3:
                        SysMsg(M2Share.g_sWinLottery3Msg, MsgColor.Green, MsgType.Hint);
                        break;
                    case 4:
                        SysMsg(M2Share.g_sWinLottery4Msg, MsgColor.Green, MsgType.Hint);
                        break;
                    case 5:
                        SysMsg(M2Share.g_sWinLottery5Msg, MsgColor.Green, MsgType.Hint);
                        break;
                    case 6:
                        SysMsg(M2Share.g_sWinLottery6Msg, MsgColor.Green, MsgType.Hint);
                        break;
                }
                if (IncGold(nGold))
                {
                    GoldChanged();
                }
                else
                {
                    DropGoldDown(nGold, true, null, null);
                }
            }
            else
            {
                M2Share.Config.NoWinLotteryCount += 500;
                SysMsg(M2Share.g_sNotWinLotteryMsg, MsgColor.Red, MsgType.Hint);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            for (var i = 0; i < MagicList.Count; i++)
            {
                if (MagicList[i].Level >= 4)
                {
                    MagicList[i].Level = 0;
                }
            }
            AddBodyLuck(0);
        }

        /// <summary>
        /// 下属杀死怪物触发
        /// </summary>
        /// <param name="killObject"></param>
        internal void SlaveKillTargetTrigger(BaseObject killObject)
        {
            /*ExpHitter.GainSlaveExp(WAbil.Level);
            var tExp = ExpHitter.Master.CalcGetExp(WAbil.Level, FightExp);
            if (!M2Share.Config.VentureServer)
            {
                if (ExpHitter.Master.IsRobot)
                {
                    ((RobotPlayObject)ExpHitter.Master).GainExp(tExp);
                }
                else
                {
                    ((PlayObject)ExpHitter.Master).GainExp(tExp);
                }
            }*/
        }

        /// <summary>
        /// 角色杀死怪物触发
        /// </summary>
        internal void KillTargetTrigger(BaseObject killObject)
        {
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(this, "@PlayKillMob", false);
            }
            var monsterExp = CalcGetExp(WAbil.Level, killObject.FightExp);
            if (!M2Share.Config.VentureServer)
            {
                if (IsRobot)
                {
                    ((RobotPlayObject)ExpHitter).GainExp(monsterExp);
                }
                else
                {
                    GainExp(monsterExp);
                }
            }
            // 是否执行任务脚本
            if (Envir.IsCheapStuff())// 地图是否有任务脚本
            {
                Merchant QuestNPC;
                if (GroupOwner != null)
                {
                    for (var i = 0; i < GroupOwner.GroupMembers.Count; i++)
                    {
                        PlayObject GroupHuman = GroupOwner.GroupMembers[i];
                        bool tCheck;
                        if (!GroupHuman.Death && Envir == GroupHuman.Envir && Math.Abs(CurrX - GroupHuman.CurrX) <= 12 && Math.Abs(CurrX - GroupHuman.CurrX) <= 12 && this == GroupHuman)
                        {
                            tCheck = false;
                        }
                        else
                        {
                            tCheck = true;
                        }
                        QuestNPC = Envir.GetQuestNpc(GroupHuman, ChrName, "", tCheck);
                        if (QuestNPC != null)
                        {
                            QuestNPC.Click(GroupHuman);
                        }
                    }
                }
                QuestNPC = Envir.GetQuestNpc(this, ChrName, "", false);
                if (QuestNPC != null)
                {
                    QuestNPC.Click(this);
                }
            }
            try
            {
                var boPK = false;
                if (!M2Share.Config.VentureServer && !Envir.Flag.boFightZone && !Envir.Flag.boFight3Zone)
                {
                    if (PvpLevel() < 2)
                    {
                        if ((killObject.Race == ActorRace.Play) || (killObject.Race == ActorRace.NPC))//允许NPC杀死人物
                        {
                            boPK = true;
                        }
                        if (killObject.Master != null && killObject.Master.Race == ActorRace.Play)
                        {
                            killObject = killObject.Master;
                            boPK = true;
                        }
                    }
                }
                if (boPK && Race == ActorRace.Play && killObject.Race == ActorRace.Play)
                {
                    var guildwarkill = false;
                    var targetObject = ((PlayObject)killObject);
                    if (MyGuild != null && targetObject.MyGuild != null)
                    {
                        if (GetGuildRelation(this, targetObject) == 2)
                        {
                            guildwarkill = true;
                        }
                    }
                    else
                    {
                        var Castle = M2Share.CastleMgr.InCastleWarArea(this);
                        if ((Castle != null && Castle.UnderWar) || (InGuildWarArea))
                        {
                            guildwarkill = true;
                        }
                    }
                    if (!guildwarkill)
                    {
                        if ((M2Share.Config.IsKillHumanWinLevel || M2Share.Config.IsKillHumanWinExp || Envir.Flag.boPKWINLEVEL || Envir.Flag.boPKWINEXP))
                        {
                            PvpDie(targetObject);
                        }
                        else
                        {
                            if (!killObject.IsGoodKilling(this))
                            {
                                targetObject.IncPkPoint(M2Share.Config.KillHumanAddPKPoint);
                                killObject.SysMsg(M2Share.g_sYouMurderedMsg, MsgColor.Red, MsgType.Hint);
                                SysMsg(Format(M2Share.g_sYouKilledByMsg, killObject.ChrName), MsgColor.Red, MsgType.Hint);
                                targetObject.AddBodyLuck(-M2Share.Config.KillHumanDecLuckPoint);
                                if (PvpLevel() < 1)
                                {
                                    if (M2Share.RandomNumber.Random(5) == 0)
                                    {
                                        killObject.MakeWeaponUnlock();
                                    }
                                }
                            }
                            else
                            {
                                killObject.SysMsg(M2Share.g_sYouprotectedByLawOfDefense, MsgColor.Green, MsgType.Hint);
                            }
                        }
                        if (killObject.Race == ActorRace.Play)// 检查攻击人是否用了着经验或等级装备
                        {
                            if (targetObject.PkDieLostExp > 0)
                            {
                                if (Abil.Exp >= targetObject.PkDieLostExp)
                                {
                                    Abil.Exp -= targetObject.PkDieLostExp;
                                }
                                else
                                {
                                    Abil.Exp = 0;
                                }
                            }
                            if (targetObject.PkDieLostLevel > 0)
                            {
                                if (Abil.Level >= targetObject.PkDieLostLevel)
                                {
                                    Abil.Level -= targetObject.PkDieLostLevel;
                                }
                                else
                                {
                                    Abil.Level = 0;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                M2Share.Log.Error(ex);
            }
            if (!Envir.Flag.boFightZone && !Envir.Flag.boFight3Zone && !killObject.Animal)
            {
                var AttackBaseObject = killObject;
                if (killObject.Master != null)
                {
                    AttackBaseObject = killObject.Master;
                }
                if (killObject.Race != ActorRace.Play)
                {
                    killObject.DropUseItems(this);
                    if (Master == null && (!NoItem || !Envir.Flag.boNODROPITEM))
                    {
                        killObject.ScatterBagItems(this);
                    }
                    if (killObject.Race >= ActorRace.Animal && Master == null && (!NoItem || !Envir.Flag.boNODROPITEM))
                    {
                        killObject.ScatterGolds(this);
                    }
                }
                else
                {
                    if (!NoItem || !Envir.Flag.boNODROPITEM)//允许设置 m_boNoItem 后人物死亡不掉物品
                    {
                        if (AttackBaseObject != null)
                        {
                            if (M2Share.Config.KillByHumanDropUseItem && AttackBaseObject.Race == ActorRace.Play || M2Share.Config.KillByMonstDropUseItem && AttackBaseObject.Race != ActorRace.Play)
                            {
                                killObject.DropUseItems(null);
                            }
                        }
                        else
                        {
                            killObject.DropUseItems(null);
                        }
                        if (M2Share.Config.DieScatterBag)
                        {
                            killObject.ScatterBagItems(null);
                        }
                        if (M2Share.Config.DieDropGold)
                        {
                            killObject.ScatterGolds(null);
                        }
                    }
                    AddBodyLuck(-(50 - (50 - WAbil.Level * 5)));
                }
            }
        }

        public override void Die()
        {
            if (Race == ActorRace.Play)
            {
                string tStr;
                if (GroupOwner != null)
                {
                    GroupOwner.DelMember(this);// 人物死亡立即退组，以防止组队刷经验
                }
                if (LastHiter != null)
                {
                    if (LastHiter.Race == ActorRace.Play)
                    {
                        tStr = LastHiter.ChrName;
                    }
                    else
                    {
                        tStr = '#' + LastHiter.ChrName;
                    }
                }
                else
                {
                    tStr = "####";
                }
                M2Share.EventSource.AddEventLog(GameEventLogType.PlayDie, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + "FZ-" + HUtil32.BoolToIntStr(Envir.Flag.boFightZone) + "_F3-" + HUtil32.BoolToIntStr(Envir.Flag.boFight3Zone) + "\t" + '0' + "\t" + '1' + "\t" + tStr);
            }
            base.Die();
        }

        public override void RecalcAbilitys()
        {
            base.RecalcAbilitys();
            if (Race == ActorRace.Play)
            {
                var mhRing = false;
                var mhBracelet = false;
                var mhNecklace = false;
                RecoveryRing = false;
                AngryRing = false;
                MagicShield = false;
                MoXieSuite = 0;
                SuckupEnemyHealthRate = 0;
                SuckupEnemyHealth = 0;
                bool[] cghi = new bool[4] { false, false, false, false };
                var shRing = false;
                var shBracelet = false;
                var shNecklace = false;
                var hpRing = false;
                var hpBracelet = false;
                var mpRing = false;
                var mpBracelet = false;
                var hpmpRing = false;
                var hpmpBracelet = false;
                var hppNecklace = false;
                var hppBracelet = false;
                var hppRing = false;
                var choWeapon = false;
                var choNecklace = false;
                var choRing = false;
                var choHelmet = false;
                var choBracelet = false;
                var psetNecklace = false;
                var psetBracelet = false;
                var psetRing = false;
                var hsetNecklace = false;
                var hsetBracelet = false;
                var hsetRing = false;
                var ysetNecklace = false;
                var ysetBracelet = false;
                var ysetRing = false;
                var bonesetWeapon = false;
                var bonesetHelmet = false;
                var bonesetDress = false;
                var bugsetNecklace = false;
                var bugsetRing = false;
                var bugsetBracelet = false;
                var ptsetBelt = false;
                var ptsetBoots = false;
                var ptsetNecklace = false;
                var ptsetBracelet = false;
                var ptsetRing = false;
                var kssetBelt = false;
                var kssetBoots = false;
                var kssetNecklace = false;
                var kssetBracelet = false;
                var kssetRing = false;
                var rubysetBelt = false;
                var rubysetBoots = false;
                var rubysetNecklace = false;
                var rubysetBracelet = false;
                var rubysetRing = false;
                var strongPtsetBelt = false;
                var strongPtsetBoots = false;
                var strongPtsetNecklace = false;
                var strongPtsetBracelet = false;
                var strongPtsetRing = false;
                var strongKssetBelt = false;
                var strongKssetBoots = false;
                var strongKssetNecklace = false;
                var strongKssetBracelet = false;
                var strongKssetRing = false;
                var strongRubysetBelt = false;
                var strongRubysetBoots = false;
                var strongRubysetNecklace = false;
                var strongRubysetBracelet = false;
                var strongRubysetRing = false;
                var dragonsetRingLeft = false;
                var dragonsetRingRight = false;
                var dragonsetBraceletLeft = false;
                var dragonsetBraceletRight = false;
                var dragonsetNecklace = false;
                var dragonsetDress = false;
                var dragonsetHelmet = false;
                var dragonsetWeapon = false;
                var dragonsetBoots = false;
                var dragonsetBelt = false;
                var dsetWingdress = false;
                for (var i = 0; i < UseItems.Length; i++)
                {
                    if (UseItems[i] != null && (UseItems[i].Index > 0))
                    {
                        StdItem stdItem;
                        if (UseItems[i].Dura == 0)
                        {
                            stdItem = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                            if (stdItem != null)
                            {
                                if ((i == Grobal2.U_WEAPON) || (i == Grobal2.U_RIGHTHAND))
                                {
                                    WAbil.HandWeight = (byte)(WAbil.HandWeight + stdItem.Weight);
                                }
                                else
                                {
                                    WAbil.WearWeight = (byte)(WAbil.WearWeight + stdItem.Weight);
                                }
                            }
                            continue;
                        }
                        stdItem = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                        ApplyItemParameters(UseItems[i], stdItem, ref AddAbil);
                        ApplyItemParametersEx(UseItems[i], ref WAbil);
                        if (stdItem != null)
                        {
                            if ((i == Grobal2.U_WEAPON) || (i == Grobal2.U_RIGHTHAND))
                            {
                                WAbil.HandWeight = (byte)(WAbil.HandWeight + stdItem.Weight);
                            }
                            else
                            {
                                WAbil.WearWeight = (byte)(WAbil.WearWeight + stdItem.Weight);
                            }
                            switch (i)
                            {
                                case Grobal2.U_WEAPON:
                                case Grobal2.U_ARMRINGL:
                                case Grobal2.U_ARMRINGR:
                                    {
                                        if ((stdItem.SpecialPwr <= -1) && (stdItem.SpecialPwr >= -50))
                                        {
                                            AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + (-stdItem.SpecialPwr));
                                        }
                                        if ((stdItem.SpecialPwr <= -51) && (stdItem.SpecialPwr >= -100))
                                        {
                                            AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + (stdItem.SpecialPwr + 50));
                                        }
                                        switch (stdItem.Shape)
                                        {
                                            case ItemShapeConst.CCHO_WEAPON:
                                                choWeapon = true;
                                                break;
                                            case ItemShapeConst.BONESET_WEAPON_SHAPE when (stdItem.StdMode == 6):
                                                bonesetWeapon = true;
                                                break;
                                            case DragonConst.DRAGON_WEAPON_SHAPE:
                                                dragonsetWeapon = true;
                                                break;
                                        }
                                        break;
                                    }
                                case Grobal2.U_NECKLACE:
                                    {
                                        switch (stdItem.Shape)
                                        {
                                            case ItemShapeConst.NECTLACE_FASTTRAINING_ITEM:
                                                FastTrain = true;
                                                break;
                                            case ItemShapeConst.NECTLACE_SEARCH_ITEM:
                                                ProbeNecklace = true;
                                                break;
                                            case ItemShapeConst.NECKLACE_GI_ITEM:
                                                cghi[1] = true;
                                                break;
                                            case ItemShapeConst.NECKLACE_OF_MANATOHEALTH:
                                                mhNecklace = true;
                                                MoXieSuite = MoXieSuite + stdItem.AniCount;
                                                break;
                                            case ItemShapeConst.NECKLACE_OF_SUCKHEALTH:
                                                shNecklace = true;
                                                SuckupEnemyHealthRate = SuckupEnemyHealthRate + stdItem.AniCount;
                                                break;
                                            case ItemShapeConst.NECKLACE_OF_HPPUP:
                                                hppNecklace = true;
                                                break;
                                            case ItemShapeConst.CCHO_NECKLACE:
                                                choNecklace = true;
                                                break;
                                            case ItemShapeConst.PSET_NECKLACE_SHAPE:
                                                psetNecklace = true;
                                                break;
                                            case ItemShapeConst.HSET_NECKLACE_SHAPE:
                                                hsetNecklace = true;
                                                break;
                                            case ItemShapeConst.YSET_NECKLACE_SHAPE:
                                                ysetNecklace = true;
                                                break;
                                            case ItemShapeConst.BUGSET_NECKLACE_SHAPE:
                                                bugsetNecklace = true;
                                                break;
                                            case ItemShapeConst.PTSET_NECKLACE_SHAPE:
                                                ptsetNecklace = true;
                                                break;
                                            case ItemShapeConst.KSSET_NECKLACE_SHAPE:
                                                kssetNecklace = true;
                                                break;
                                            case ItemShapeConst.RUBYSET_NECKLACE_SHAPE:
                                                rubysetNecklace = true;
                                                break;
                                            case ItemShapeConst.STRONG_PTSET_NECKLACE_SHAPE:
                                                strongPtsetNecklace = true;
                                                break;
                                            case ItemShapeConst.STRONG_KSSET_NECKLACE_SHAPE:
                                                strongKssetNecklace = true;
                                                break;
                                            case ItemShapeConst.STRONG_RUBYSET_NECKLACE_SHAPE:
                                                strongRubysetNecklace = true;
                                                break;
                                            case DragonConst.DRAGON_NECKLACE_SHAPE:
                                                dragonsetNecklace = true;
                                                break;
                                        }
                                        break;
                                    }
                                case Grobal2.U_RINGR:
                                case Grobal2.U_RINGL:
                                    {
                                        switch (stdItem.Shape)
                                        {
                                            case ItemShapeConst.RING_TRANSPARENT_ITEM:
                                                StatusArr[PoisonState.STATE_TRANSPARENT] = 60000;
                                                HideMode = true;
                                                break;
                                            case ItemShapeConst.RING_SPACEMOVE_ITEM:
                                                Teleport = true;
                                                break;
                                            case ItemShapeConst.RING_MAKESTONE_ITEM:
                                                Paralysis = true;
                                                break;
                                            case ItemShapeConst.RING_REVIVAL_ITEM:
                                                Revival = true;
                                                break;
                                            case ItemShapeConst.RING_FIREBALL_ITEM:
                                                _flameRing = true;
                                                break;
                                            case ItemShapeConst.RING_HEALING_ITEM:
                                                RecoveryRing = true;
                                                break;
                                            case ItemShapeConst.RING_ANGERENERGY_ITEM:
                                                AngryRing = true;
                                                break;
                                            case ItemShapeConst.RING_MAGICSHIELD_ITEM:
                                                MagicShield = true;
                                                break;
                                            case ItemShapeConst.RING_SUPERSTRENGTH_ITEM:
                                                _muscleRing = true;
                                                break;
                                            case ItemShapeConst.RING_CHUN_ITEM:
                                                cghi[0] = true;
                                                break;
                                            case ItemShapeConst.RING_OF_MANATOHEALTH:
                                                mhRing = true;
                                                MoXieSuite = MoXieSuite + stdItem.AniCount;
                                                break;
                                            case ItemShapeConst.RING_OF_SUCKHEALTH:
                                                shRing = true;
                                                SuckupEnemyHealthRate = SuckupEnemyHealthRate + stdItem.AniCount;
                                                break;
                                            case ItemShapeConst.RING_OF_HPUP:
                                                hpRing = true;
                                                break;
                                            case ItemShapeConst.RING_OF_MPUP:
                                                mpRing = true;
                                                break;
                                            case ItemShapeConst.RING_OF_HPMPUP:
                                                hpmpRing = true;
                                                break;
                                            case ItemShapeConst.RING_OH_HPPUP:
                                                hppRing = true;
                                                break;
                                            case ItemShapeConst.CCHO_RING:
                                                choRing = true;
                                                break;
                                            case ItemShapeConst.PSET_RING_SHAPE:
                                                psetRing = true;
                                                break;
                                            case ItemShapeConst.HSET_RING_SHAPE:
                                                hsetRing = true;
                                                break;
                                            case ItemShapeConst.YSET_RING_SHAPE:
                                                ysetRing = true;
                                                break;
                                            case ItemShapeConst.BUGSET_RING_SHAPE:
                                                bugsetRing = true;
                                                break;
                                            case ItemShapeConst.PTSET_RING_SHAPE:
                                                ptsetRing = true;
                                                break;
                                            case ItemShapeConst.KSSET_RING_SHAPE:
                                                kssetRing = true;
                                                break;
                                            case ItemShapeConst.RUBYSET_RING_SHAPE:
                                                rubysetRing = true;
                                                break;
                                            case ItemShapeConst.STRONG_PTSET_RING_SHAPE:
                                                strongPtsetRing = true;
                                                break;
                                            case ItemShapeConst.STRONG_KSSET_RING_SHAPE:
                                                strongKssetRing = true;
                                                break;
                                            case ItemShapeConst.STRONG_RUBYSET_RING_SHAPE:
                                                strongRubysetRing = true;
                                                break;
                                            case DragonConst.DRAGON_RING_SHAPE:
                                                {
                                                    if ((i == Grobal2.U_RINGL))
                                                    {
                                                        dragonsetRingLeft = true;
                                                    }
                                                    if ((i == Grobal2.U_RINGR))
                                                    {
                                                        dragonsetRingRight = true;
                                                    }
                                                    break;
                                                }
                                        }
                                        break;
                                    }
                            }
                            switch (i)
                            {
                                case Grobal2.U_ARMRINGL:
                                case Grobal2.U_ARMRINGR:
                                    {
                                        switch (stdItem.Shape)
                                        {
                                            case ItemShapeConst.ARMRING_HAP_ITEM:
                                                cghi[2] = true;
                                                break;
                                            case ItemShapeConst.BRACELET_OF_MANATOHEALTH:
                                                mhBracelet = true;
                                                MoXieSuite = MoXieSuite + stdItem.AniCount;
                                                break;
                                            case ItemShapeConst.BRACELET_OF_SUCKHEALTH:
                                                shBracelet = true;
                                                SuckupEnemyHealthRate = SuckupEnemyHealthRate + stdItem.AniCount;
                                                break;
                                            case ItemShapeConst.BRACELET_OF_HPUP:
                                                hpBracelet = true;
                                                break;
                                            case ItemShapeConst.BRACELET_OF_MPUP:
                                                mpBracelet = true;
                                                break;
                                            case ItemShapeConst.BRACELET_OF_HPMPUP:
                                                hpmpBracelet = true;
                                                break;
                                            case ItemShapeConst.BRACELET_OF_HPPUP:
                                                hppBracelet = true;
                                                break;
                                            case ItemShapeConst.CCHO_BRACELET:
                                                choBracelet = true;
                                                break;
                                            case ItemShapeConst.PSET_BRACELET_SHAPE:
                                                psetBracelet = true;
                                                break;
                                            case ItemShapeConst.HSET_BRACELET_SHAPE:
                                                hsetBracelet = true;
                                                break;
                                            case ItemShapeConst.YSET_BRACELET_SHAPE:
                                                ysetBracelet = true;
                                                break;
                                            case ItemShapeConst.BUGSET_BRACELET_SHAPE:
                                                bugsetBracelet = true;
                                                break;
                                            case ItemShapeConst.PTSET_BRACELET_SHAPE:
                                                ptsetBracelet = true;
                                                break;
                                            case ItemShapeConst.KSSET_BRACELET_SHAPE:
                                                kssetBracelet = true;
                                                break;
                                            case ItemShapeConst.RUBYSET_BRACELET_SHAPE:
                                                rubysetBracelet = true;
                                                break;
                                            case ItemShapeConst.STRONG_PTSET_BRACELET_SHAPE:
                                                strongPtsetBracelet = true;
                                                break;
                                            case ItemShapeConst.STRONG_KSSET_BRACELET_SHAPE:
                                                strongKssetBracelet = true;
                                                break;
                                            case ItemShapeConst.STRONG_RUBYSET_BRACELET_SHAPE:
                                                strongRubysetBracelet = true;
                                                break;
                                            case DragonConst.DRAGON_BRACELET_SHAPE:
                                                {
                                                    if ((i == Grobal2.U_ARMRINGL))
                                                    {
                                                        dragonsetBraceletLeft = true;
                                                    }
                                                    if ((i == Grobal2.U_ARMRINGR))
                                                    {
                                                        dragonsetBraceletRight = true;
                                                    }
                                                    break;
                                                }
                                        }
                                        break;
                                    }
                                case Grobal2.U_HELMET:
                                    {
                                        switch (stdItem.Shape)
                                        {
                                            case ItemShapeConst.HELMET_IL_ITEM:
                                                cghi[3] = true;
                                                break;
                                            case ItemShapeConst.CCHO_HELMET:
                                                choHelmet = true;
                                                break;
                                            case ItemShapeConst.BONESET_HELMET_SHAPE:
                                                bonesetHelmet = true;
                                                break;
                                            case DragonConst.DRAGON_HELMET_SHAPE:
                                                dragonsetHelmet = true;
                                                break;
                                        }
                                        break;
                                    }
                                case Grobal2.U_DRESS:
                                    {
                                        switch (stdItem.Shape)
                                        {
                                            case ItemShapeConst.DRESS_SHAPE_WING:
                                                dsetWingdress = true;
                                                break;
                                            case ItemShapeConst.BONESET_DRESS_SHAPE:
                                                bonesetDress = true;
                                                break;
                                            case DragonConst.DRAGON_DRESS_SHAPE:
                                                dragonsetDress = true;
                                                break;
                                        }
                                        break;
                                    }
                                case Grobal2.U_BELT:
                                    {
                                        switch (stdItem.Shape)
                                        {
                                            case ItemShapeConst.PTSET_BELT_SHAPE:
                                                ptsetBelt = true;
                                                break;
                                            case ItemShapeConst.KSSET_BELT_SHAPE:
                                                kssetBelt = true;
                                                break;
                                            case ItemShapeConst.RUBYSET_BELT_SHAPE:
                                                rubysetBelt = true;
                                                break;
                                            case ItemShapeConst.STRONG_PTSET_BELT_SHAPE:
                                                strongPtsetBelt = true;
                                                break;
                                            case ItemShapeConst.STRONG_KSSET_BELT_SHAPE:
                                                strongKssetBelt = true;
                                                break;
                                            case ItemShapeConst.STRONG_RUBYSET_BELT_SHAPE:
                                                strongRubysetBelt = true;
                                                break;
                                            case DragonConst.DRAGON_BELT_SHAPE:
                                                dragonsetBelt = true;
                                                break;
                                        }
                                        break;
                                    }
                                case Grobal2.U_BOOTS:
                                    {
                                        switch (stdItem.Shape)
                                        {
                                            case ItemShapeConst.PTSET_BOOTS_SHAPE:
                                                ptsetBoots = true;
                                                break;
                                            case ItemShapeConst.KSSET_BOOTS_SHAPE:
                                                kssetBoots = true;
                                                break;
                                            case ItemShapeConst.RUBYSET_BOOTS_SHAPE:
                                                rubysetBoots = true;
                                                break;
                                            case ItemShapeConst.STRONG_PTSET_BOOTS_SHAPE:
                                                strongPtsetBoots = true;
                                                break;
                                            case ItemShapeConst.STRONG_KSSET_BOOTS_SHAPE:
                                                strongKssetBoots = true;
                                                break;
                                            case ItemShapeConst.STRONG_RUBYSET_BOOTS_SHAPE:
                                                strongRubysetBoots = true;
                                                break;
                                            case DragonConst.DRAGON_BOOTS_SHAPE:
                                                dragonsetBoots = true;
                                                break;
                                        }
                                        break;
                                    }
                                case Grobal2.U_CHARM:
                                    {
                                        if ((stdItem.StdMode == 53) && (stdItem.Shape == ItemShapeConst.SHAPE_OF_LUCKYLADLE))
                                        {
                                            AddAbil.Luck = (byte)(HUtil32._MIN(255, AddAbil.Luck + 1));
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }
                if (cghi[0] && cghi[1] && cghi[2] && cghi[3])
                {
                    RecallSuite = true;
                }
                if (mhNecklace && mhBracelet && mhRing)
                {
                    MoXieSuite = MoXieSuite + 50;
                }
                if (shNecklace && shBracelet && shRing)
                {
                    AddAbil.HIT = (ushort)(AddAbil.HIT + 2);
                }
                if (hpBracelet && hpRing)
                {
                    AddAbil.HP = (ushort)(AddAbil.HP + 50);
                }
                if (mpBracelet && mpRing)
                {
                    AddAbil.MP = (ushort)(AddAbil.MP + 50);
                }
                if (hpmpBracelet && hpmpRing)
                {
                    AddAbil.HP = (ushort)(AddAbil.HP + 30);
                    AddAbil.MP = (ushort)(AddAbil.MP + 30);
                }
                if (hppNecklace && hppBracelet && hppRing)
                {
                    AddAbil.HP = (ushort)(AddAbil.HP + ((WAbil.MaxHP * 30) / 100));
                    AddAbil.AC = (ushort)(AddAbil.AC + HUtil32.MakeWord(2, 2));
                }
                if (choWeapon && choNecklace && choRing && choHelmet && choBracelet)
                {
                    AddAbil.HitSpeed = (ushort)(AddAbil.HitSpeed + 4);
                    AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(2, 5));
                }
                if (psetBracelet && psetRing)
                {
                    AddAbil.HitSpeed = (ushort)(AddAbil.HitSpeed + 2);
                    if (psetNecklace)
                    {
                        AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(1, 3));
                    }
                }
                if (hsetBracelet && hsetRing)
                {
                    WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 20);
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 5));
                    if (hsetNecklace)
                    {
                        AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(1, 2));
                    }
                }
                if (ysetBracelet && ysetRing)
                {
                    AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + 3);
                    if (ysetNecklace)
                    {
                        AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(1, 2));
                    }
                }
                if (bonesetWeapon && bonesetHelmet && bonesetDress)
                {
                    AddAbil.AC = (ushort)(AddAbil.AC + HUtil32.MakeWord(0, 2));
                    AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(0, 1));
                    AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(0, 1));
                }
                if (bugsetNecklace && bugsetRing && bugsetBracelet)
                {
                    AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(0, 1));
                    AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(0, 1));
                    AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(0, 1));
                    AddAbil.AntiMagic = (ushort)(AddAbil.AntiMagic + 1);
                    AddAbil.AntiPoison = (ushort)(AddAbil.AntiPoison + 1);
                }
                if (ptsetBelt && ptsetBoots && ptsetNecklace && ptsetBracelet && ptsetRing)
                {
                    AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(0, 2));
                    AddAbil.AC = (ushort)(AddAbil.AC + HUtil32.MakeWord(0, 2));
                    WAbil.MaxHandWeight = (byte)(HUtil32._MIN(255, WAbil.MaxHandWeight + 1));
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 2));
                }
                if (kssetBelt && kssetBoots && kssetNecklace && kssetBracelet && kssetRing)
                {
                    AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(0, 2));
                    AddAbil.AC = (ushort)(AddAbil.AC + HUtil32.MakeWord(0, 1));
                    AddAbil.MAC = (ushort)(AddAbil.MAC + HUtil32.MakeWord(0, 1));
                    AddAbil.SPEED = (ushort)(AddAbil.SPEED + 1);
                    WAbil.MaxHandWeight = (byte)(HUtil32._MIN(255, WAbil.MaxHandWeight + 1));
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 2));
                }
                if (rubysetBelt && rubysetBoots && rubysetNecklace && rubysetBracelet && rubysetRing)
                {
                    AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(0, 2));
                    AddAbil.MAC = (ushort)(AddAbil.MAC + HUtil32.MakeWord(0, 2));
                    WAbil.MaxHandWeight = (byte)(HUtil32._MIN(255, WAbil.MaxHandWeight + 1));
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 2));
                }
                if (strongPtsetBelt && strongPtsetBoots && strongPtsetNecklace && strongPtsetBracelet && strongPtsetRing)
                {
                    AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(0, 3));
                    AddAbil.HP = (ushort)(AddAbil.HP + 30);
                    AddAbil.HitSpeed = (ushort)(AddAbil.HitSpeed + 2);
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 2));
                }
                if (strongKssetBelt && strongKssetBoots && strongKssetNecklace && strongKssetBracelet && strongKssetRing)
                {
                    AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(0, 2));
                    AddAbil.HP = (ushort)(AddAbil.HP + 15);
                    AddAbil.MP = (ushort)(AddAbil.MP + 20);
                    AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + 1);
                    AddAbil.HIT = (ushort)(AddAbil.HIT + 1);
                    AddAbil.SPEED = (ushort)(AddAbil.SPEED + 1);
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 2));
                }
                if (strongRubysetBelt && strongRubysetBoots && strongRubysetNecklace && strongRubysetBracelet && strongRubysetRing)
                {
                    AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(0, 2));
                    AddAbil.MP = (ushort)(AddAbil.MP + 40);
                    AddAbil.SPEED = (ushort)(AddAbil.SPEED + 2);
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 2));
                }
                if (dragonsetRingLeft && dragonsetRingRight && dragonsetBraceletLeft && dragonsetBraceletRight && dragonsetNecklace && dragonsetDress && dragonsetHelmet && dragonsetWeapon && dragonsetBoots && dragonsetBelt)
                {
                    AddAbil.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(AddAbil.AC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + 4));
                    AddAbil.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(AddAbil.MAC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + 4));
                    AddAbil.Luck = (byte)(HUtil32._MIN(255, AddAbil.Luck + 2));
                    AddAbil.HitSpeed = (ushort)(AddAbil.HitSpeed + 2);
                    AddAbil.AntiMagic = (ushort)(AddAbil.AntiMagic + 6);
                    AddAbil.AntiPoison = (ushort)(AddAbil.AntiPoison + 6);
                    WAbil.MaxHandWeight = (byte)(HUtil32._MIN(255, WAbil.MaxHandWeight + 34));
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 27));
                    WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 120);
                    WAbil.MaxHP = (ushort)(WAbil.MaxHP + 70);
                    WAbil.MaxMP = (ushort)(WAbil.MaxMP + 80);
                    AddAbil.SPEED = (ushort)(AddAbil.SPEED + 1);
                    AddAbil.DC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(AddAbil.DC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.DC) + 4));
                    AddAbil.MC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(AddAbil.MC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MC) + 3));
                    AddAbil.SC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(AddAbil.SC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.SC) + 3));
                }
                else
                {
                    if (dragonsetDress && dragonsetHelmet && dragonsetWeapon && dragonsetBoots && dragonsetBelt)
                    {
                        WAbil.MaxHandWeight = (byte)(HUtil32._MIN(255, WAbil.MaxHandWeight + 34));
                        WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 50);
                        AddAbil.SPEED = (ushort)(AddAbil.SPEED + 1);
                        AddAbil.DC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(AddAbil.DC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.DC) + 4));
                        AddAbil.MC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(AddAbil.MC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MC) + 3));
                        AddAbil.SC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(AddAbil.SC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.SC) + 3));
                    }
                    else if (dragonsetDress && dragonsetBoots && dragonsetBelt)
                    {
                        WAbil.MaxHandWeight = (byte)(HUtil32._MIN(255, WAbil.MaxHandWeight + 17));
                        WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 30);
                        AddAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.DC) + 1));
                        AddAbil.MC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MC) + 1));
                        AddAbil.SC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.SC) + 1));
                    }
                    else if (dragonsetDress && dragonsetHelmet && dragonsetWeapon)
                    {
                        AddAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.DC) + 2));
                        AddAbil.MC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MC) + 1));
                        AddAbil.SC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.SC) + 1));
                        AddAbil.SPEED = (ushort)(AddAbil.SPEED + 1);
                    }
                    if (dragonsetRingLeft && dragonsetRingRight && dragonsetBraceletLeft && dragonsetBraceletRight && dragonsetNecklace)
                    {
                        WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 27));
                        WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 50);
                        AddAbil.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(AddAbil.AC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + 3));
                        AddAbil.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(AddAbil.MAC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + 3));
                    }
                    else if ((dragonsetRingLeft || dragonsetRingRight) && dragonsetBraceletLeft && dragonsetBraceletRight && dragonsetNecklace)
                    {
                        WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 17));
                        WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 30);
                        AddAbil.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(AddAbil.AC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + 1));
                        AddAbil.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(AddAbil.MAC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + 1));
                    }
                    else if (dragonsetRingLeft && dragonsetRingRight && (dragonsetBraceletLeft || dragonsetBraceletRight) && dragonsetNecklace)
                    {
                        WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 17));
                        WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 30);
                        AddAbil.AC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + 2));
                        AddAbil.MAC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + 2));
                    }
                    else if ((dragonsetRingLeft || dragonsetRingRight) && (dragonsetBraceletLeft || dragonsetBraceletRight) && dragonsetNecklace)
                    {
                        WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 17));
                        WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 30);
                        AddAbil.AC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + 1));
                        AddAbil.MAC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + 1));
                    }
                    else
                    {
                        if (dragonsetBraceletLeft && dragonsetBraceletRight)
                        {
                            AddAbil.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(AddAbil.AC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC)));
                            AddAbil.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(AddAbil.MAC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC)));
                        }
                        if (dragonsetRingLeft && dragonsetRingRight)
                        {
                            AddAbil.AC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + 1));
                            AddAbil.MAC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + 1));
                        }
                    }
                }
                if (dsetWingdress && (Abil.Level >= 20))
                {
                    switch (Abil.Level)
                    {
                        case < 40:
                            AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(0, 1));
                            AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(0, 2));
                            AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(0, 2));
                            AddAbil.AC = (ushort)(AddAbil.AC + HUtil32.MakeWord(2, 3));
                            AddAbil.MAC = (ushort)(AddAbil.MAC + HUtil32.MakeWord(0, 2));
                            break;
                        case < 50:
                            AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(0, 3));
                            AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(0, 4));
                            AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(0, 4));
                            AddAbil.AC = (ushort)(AddAbil.AC + HUtil32.MakeWord(5, 5));
                            AddAbil.MAC = (ushort)(AddAbil.MAC + HUtil32.MakeWord(1, 2));
                            break;
                        default:
                            AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(0, 5));
                            AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(0, 6));
                            AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(0, 6));
                            AddAbil.AC = (ushort)(AddAbil.AC + HUtil32.MakeWord(9, 7));
                            AddAbil.MAC = (ushort)(AddAbil.MAC + HUtil32.MakeWord(2, 4));
                            break;
                    }
                }
                WAbil.Weight = RecalcBagWeight();

                if (_flameRing)
                {
                    AddItemSkill(M2Share.AM_FIREBALL);
                }
                else
                {
                    DelItemSkill(M2Share.AM_FIREBALL);
                }
                if (RecoveryRing)
                {
                    AddItemSkill(M2Share.AM_HEALING);
                }
                else
                {
                    DelItemSkill(M2Share.AM_HEALING);
                }
                if (_muscleRing)
                {
                    WAbil.MaxWeight = (ushort)(WAbil.MaxWeight * 2);
                    WAbil.MaxWearWeight = (byte)HUtil32._MIN(255, WAbil.MaxWearWeight * 2);
                    if ((WAbil.MaxHandWeight * 2 > 255))
                    {
                        WAbil.MaxHandWeight = 255;
                    }
                    else
                    {
                        WAbil.MaxHandWeight = (byte)(WAbil.MaxHandWeight * 2);
                    }
                }
                if (MoXieSuite > 0)
                {
                    if (MoXieSuite >= WAbil.MaxMP)
                    {
                        MoXieSuite = WAbil.MaxMP - 1;
                    }
                    WAbil.MaxMP = (ushort)(WAbil.MaxMP - MoXieSuite);
                    WAbil.MaxHP = (ushort)(WAbil.MaxHP + MoXieSuite);
                    if ((Race == ActorRace.Play) && (WAbil.HP > WAbil.MaxHP))
                    {
                        WAbil.HP = WAbil.MaxHP;
                    }
                }
                if ((Race == ActorRace.Play) && (WAbil.HP > WAbil.MaxHP) && (!mhNecklace && !mhBracelet && !mhRing))
                {
                    WAbil.HP = WAbil.MaxHP;
                }
                if ((Race == ActorRace.Play) && (WAbil.MP > WAbil.MaxMP))
                {
                    WAbil.MP = WAbil.MaxMP;
                }
                if (ExtraAbil[AbilConst.EABIL_DCUP] > 0)
                {
                    WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), (ushort)(HUtil32.HiByte(WAbil.DC) + ExtraAbil[AbilConst.EABIL_DCUP]));
                }
                if (ExtraAbil[AbilConst.EABIL_MCUP] > 0)
                {
                    WAbil.MC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.MC), (ushort)(HUtil32.HiByte(WAbil.MC) + ExtraAbil[AbilConst.EABIL_MCUP]));
                }
                if (ExtraAbil[AbilConst.EABIL_SCUP] > 0)
                {
                    WAbil.SC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.SC), (ushort)(HUtil32.HiByte(WAbil.SC) + ExtraAbil[AbilConst.EABIL_SCUP]));
                }
                if (ExtraAbil[AbilConst.EABIL_HITSPEEDUP] > 0)
                {
                    HitSpeed = (ushort)(HitSpeed + ExtraAbil[AbilConst.EABIL_HITSPEEDUP]);
                }
                if (ExtraAbil[AbilConst.EABIL_HPUP] > 0)
                {
                    WAbil.MaxHP = (ushort)(WAbil.MaxHP + ExtraAbil[AbilConst.EABIL_HPUP]);
                }
                if (ExtraAbil[AbilConst.EABIL_MPUP] > 0)
                {
                    WAbil.MaxMP = (ushort)(WAbil.MaxMP + ExtraAbil[AbilConst.EABIL_MPUP]);
                }
                if (ExtraAbil[AbilConst.EABIL_PWRRATE] > 0)
                {
                    WAbil.DC = HUtil32.MakeWord((ushort)((HUtil32.LoByte(WAbil.DC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100), (ushort)((HUtil32.HiByte(WAbil.DC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100));
                    WAbil.MC = HUtil32.MakeWord((ushort)((HUtil32.LoByte(WAbil.MC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100), (ushort)((HUtil32.HiByte(WAbil.MC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100));
                    WAbil.SC = HUtil32.MakeWord((ushort)((HUtil32.LoByte(WAbil.SC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100), (ushort)((HUtil32.HiByte(WAbil.SC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100));
                }
                if (Race == ActorRace.Play)
                {
                    var fastmoveflag = UseItems[Grobal2.U_BOOTS] != null && UseItems[Grobal2.U_BOOTS].Dura > 0 && UseItems[Grobal2.U_BOOTS].Index == M2Share.INDEX_MIRBOOTS;
                    if (fastmoveflag)
                    {
                        StatusArr[PoisonState.FASTMOVE] = 60000;
                    }
                    else
                    {
                        StatusArr[PoisonState.FASTMOVE] = 0;
                    }
                    //if ((Abil.Level >= EfftypeConst.EFFECTIVE_HIGHLEVEL))
                    //{
                    //    if (BoHighLevelEffect)
                    //    {
                    //        StatusArr[Grobal2.STATE_50LEVELEFFECT] = 60000;
                    //    }
                    //    else
                    //    {
                    //        StatusArr[Grobal2.STATE_50LEVELEFFECT] = 0;
                    //    }
                    //}
                    //else
                    //{
                    //    StatusArr[Grobal2.STATE_50LEVELEFFECT] = 0;
                    //}
                    CharStatus = GetCharStatus();
                    StatusChanged();
                    SendUpdateMsg(this, Grobal2.RM_CHARSTATUSCHANGED, HitSpeed, CharStatus, 0, 0, "");
                }
                RecalcAdjusBonus();

                var oldlight = Light;
                Light = GetMyLight();
                if (oldlight != Light)
                {
                    SendRefMsg(Grobal2.RM_CHANGELIGHT, 0, 0, 0, 0, "");
                }
            }
        }

        protected override void UpdateVisibleGay(BaseObject baseObject)
        {
            var boIsVisible = false;
            VisibleBaseObject visibleBaseObject;
            if (baseObject.Race == ActorRace.Play || baseObject.Master != null)
            {
                IsVisibleActive = true;// 如果是人物或宝宝则置TRUE
            }
            for (var i = 0; i < VisibleActors.Count; i++)
            {
                visibleBaseObject = VisibleActors[i];
                if (visibleBaseObject.BaseObject == baseObject)
                {
                    visibleBaseObject.VisibleFlag = VisibleFlag.Invisible;
                    boIsVisible = true;
                    break;
                }
            }
            if (boIsVisible)
            {
                return;
            }
            visibleBaseObject = new VisibleBaseObject
            {
                VisibleFlag = VisibleFlag.Hidden,
                BaseObject = baseObject
            };
            VisibleActors.Add(visibleBaseObject);
            if (baseObject.Race == ActorRace.Play)
            {
                SendWhisperMsg((PlayObject)baseObject);
            }
        }

        public override void SearchViewRange()
        {
            for (var i = VisibleItems.Count - 1; i >= 0; i--)
            {
                VisibleItems[i].VisibleFlag = 0;
            }
            for (var i = VisibleEvents.Count - 1; i >= 0; i--)
            {
                VisibleEvents[i].VisibleFlag = 0;
            }
            for (var i = VisibleActors.Count - 1; i >= 0; i--)
            {
                VisibleActors[i].VisibleFlag = 0;
            }
            var nStartX = (short)(CurrX - ViewRange);
            var nEndX = (short)(CurrX + ViewRange);
            var nStartY = (short)(CurrY - ViewRange);
            var nEndY = (short)(CurrY + ViewRange);
            try
            {
                for (var nX = nStartX; nX <= nEndX; nX++)
                {
                    for (var nY = nStartY; nY <= nEndY; nY++)
                    {
                        var cellSuccess = false;
                        var cellInfo = Envir.GetCellInfo(nX, nY, ref cellSuccess);
                        if (cellSuccess && cellInfo.IsAvailable)
                        {
                            var nIdx = 0;
                            while (true)
                            {
                                if (cellInfo.Count <= nIdx)
                                {
                                    break;
                                }
                                var cellObject = cellInfo.ObjList[nIdx];
                                if (cellObject != null)
                                {
                                    if (cellObject.ActorObject)
                                    {
                                        if ((HUtil32.GetTickCount() - cellObject.AddTime) >= 60 * 1000)
                                        {
                                            cellInfo.Remove(cellObject);
                                            if (cellInfo.Count > 0)
                                            {
                                                continue;
                                            }
                                            cellInfo.Dispose();
                                            break;
                                        }
                                        BaseObject baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                                        if (baseObject != null && !baseObject.Invisible)
                                        {
                                            if (!baseObject.Ghost && !baseObject.FixedHideMode && !baseObject.ObMode)
                                            {
                                                if (Race < ActorRace.Animal || Master != null || CrazyMode || NastyMode || WantRefMsg || baseObject.Master != null && Math.Abs(baseObject.CurrX - CurrX) <= 3 && Math.Abs(baseObject.CurrY - CurrY) <= 3 || baseObject.Race == ActorRace.Play)
                                                {
                                                    UpdateVisibleGay(baseObject);
                                                    if (baseObject.MapCell == CellType.Monster && MapCell == CellType.Play && !ObMode && !baseObject.FixedHideMode)
                                                    {
                                                        //我的视野 进入对方的攻击范围
                                                        if (Math.Abs(baseObject.CurrX - CurrX) <= (ViewRange - baseObject.ViewRange) && Math.Abs(baseObject.CurrY - CurrY) <= (ViewRange - baseObject.ViewRange))
                                                        {
                                                            baseObject.UpdateMonsterVisible(this);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (Race == ActorRace.Play)
                                    {
                                        if (cellObject.CellType == CellType.Item)
                                        {
                                            if ((HUtil32.GetTickCount() - cellObject.AddTime) > M2Share.Config.ClearDropOnFloorItemTime)// 60 * 60 * 1000
                                            {
                                                cellInfo.Remove(cellObject);
                                                if (cellInfo.Count > 0)
                                                {
                                                    continue;
                                                }
                                                cellInfo.Dispose();
                                                break;
                                            }
                                            var mapItem = (MapItem)M2Share.CellObjectSystem.Get(cellObject.CellObjId);
                                            UpdateVisibleItem(nX, nY, mapItem);
                                            if (mapItem.OfBaseObject > 0 || mapItem.DropBaseObject > 0)
                                            {
                                                if ((HUtil32.GetTickCount() - mapItem.CanPickUpTick) > M2Share.Config.FloorItemCanPickUpTime) // 2 * 60 * 1000
                                                {
                                                    mapItem.OfBaseObject = 0;
                                                    mapItem.DropBaseObject = 0;
                                                }
                                                else
                                                {
                                                    if (M2Share.ActorMgr.Get(mapItem.OfBaseObject) != null)
                                                    {
                                                        if (M2Share.ActorMgr.Get(mapItem.OfBaseObject).Ghost)
                                                        {
                                                            mapItem.OfBaseObject = 0;
                                                        }
                                                    }
                                                    if (M2Share.ActorMgr.Get(mapItem.DropBaseObject) != null)
                                                    {
                                                        if (M2Share.ActorMgr.Get(mapItem.DropBaseObject).Ghost)
                                                        {
                                                            mapItem.DropBaseObject = 0;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (cellObject.CellType == CellType.Event)
                                        {
                                            EventInfo mapEvent = (EventInfo)M2Share.CellObjectSystem.Get(cellObject.CellObjId);
                                            if (mapEvent.Visible)
                                            {
                                                UpdateVisibleEvent(nX, nY, mapEvent);
                                            }
                                        }
                                    }
                                }
                                nIdx++;
                            }
                        }
                    }
                }
                var n18 = 0;
                while (true)
                {
                    if (VisibleActors.Count <= n18)
                    {
                        break;
                    }
                    var visibleBaseObject = VisibleActors[n18];
                    if (visibleBaseObject.VisibleFlag == 0)
                    {
                        if (Race == ActorRace.Play)
                        {
                            var baseObject = visibleBaseObject.BaseObject;
                            if (!baseObject.FixedHideMode && !baseObject.Ghost)//防止人物退出时发送重复的消息占用带宽，人物进入隐身模式时人物不消失问题
                            {
                                SendMsg(baseObject, Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
                            }
                        }
                        VisibleActors.RemoveAt(n18);
                        Dispose(visibleBaseObject);
                        continue;
                    }
                    if (Race == ActorRace.Play && visibleBaseObject.VisibleFlag == VisibleFlag.Hidden)
                    {
                        var baseObject = visibleBaseObject.BaseObject;
                        if (baseObject != this)
                        {
                            if (baseObject.Death)
                            {
                                if (baseObject.Skeleton)
                                {
                                    SendMsg(baseObject, Grobal2.RM_SKELETON, baseObject.Direction, baseObject.CurrX, baseObject.CurrY, 0, "");
                                }
                                else
                                {
                                    SendMsg(baseObject, Grobal2.RM_DEATH, baseObject.Direction, baseObject.CurrX, baseObject.CurrY, 0, "");
                                }
                            }
                            else
                            {
                                SendMsg(baseObject, Grobal2.RM_TURN, baseObject.Direction, baseObject.CurrX, baseObject.CurrY, 0, baseObject.GetShowName());
                            }
                        }
                    }
                    n18++;
                }

                var I = 0;
                while (true)
                {
                    if (VisibleItems.Count <= I)
                    {
                        break;
                    }
                    var visibleMapItem = VisibleItems[I];
                    if (visibleMapItem.VisibleFlag == 0)
                    {
                        SendMsg(this, Grobal2.RM_ITEMHIDE, 0, visibleMapItem.MapItem.ActorId, visibleMapItem.nX, visibleMapItem.nY, "");
                        VisibleItems.RemoveAt(I);
                        Dispose(visibleMapItem);
                        continue;
                    }
                    if (visibleMapItem.VisibleFlag == VisibleFlag.Hidden)
                    {
                        SendMsg(this, Grobal2.RM_ITEMSHOW, visibleMapItem.wLooks, visibleMapItem.MapItem.ActorId, visibleMapItem.nX, visibleMapItem.nY, visibleMapItem.sName);
                    }
                    I++;
                }
                I = 0;
                while (true)
                {
                    if (VisibleEvents.Count <= I)
                    {
                        break;
                    }
                    EventInfo mapEvent = VisibleEvents[I];
                    if (mapEvent.VisibleFlag == VisibleFlag.Visible)
                    {
                        SendMsg(this, Grobal2.RM_HIDEEVENT, 0, mapEvent.Id, mapEvent.nX, mapEvent.nY, "");
                        VisibleEvents.RemoveAt(I);
                        continue;
                    }
                    if (mapEvent.VisibleFlag == VisibleFlag.Hidden)
                    {
                        SendMsg(this, Grobal2.RM_SHOWEVENT, (short)mapEvent.EventType, mapEvent.Id, HUtil32.MakeLong(mapEvent.nX, (short)mapEvent.EventParam), mapEvent.nY, "");
                    }
                    I++;
                }
            }
            catch (Exception e)
            {
                M2Share.Log.Error(e.StackTrace);
                KickException();
            }
        }

        /// <summary>
        /// 显示玩家名字
        /// </summary>
        /// <returns></returns>
        public override string GetShowName()
        {
            var result = string.Empty;
            var sChrName = string.Empty;
            var sGuildName = string.Empty;
            var sDearName = string.Empty;
            var sMasterName = string.Empty;
            const string sExceptionMsg = "[Exception] TPlayObject::GetShowName";
            try
            {
                if (MyGuild != null)
                {
                    var castle = M2Share.CastleMgr.IsCastleMember(this);
                    if (castle != null)
                    {
                        sGuildName = M2Share.g_sCastleGuildName.Replace("%castlename", castle.sName);
                        sGuildName = sGuildName.Replace("%guildname", MyGuild.sGuildName);
                        sGuildName = sGuildName.Replace("%rankname", GuildRankName);
                    }
                    else
                    {
                        castle = M2Share.CastleMgr.InCastleWarArea(this);// 01/25 多城堡
                        if (M2Share.Config.ShowGuildName || castle != null && castle.UnderWar || InGuildWarArea)
                        {
                            sGuildName = M2Share.g_sNoCastleGuildName.Replace("%guildname", MyGuild.sGuildName);
                            sGuildName = sGuildName.Replace("%rankname", GuildRankName);
                        }
                    }
                }
                if (!M2Share.Config.ShowRankLevelName)
                {
                    if (MBtReLevel > 0)
                    {
                        switch (Job)
                        {
                            case PlayJob.Warrior:
                                sChrName = M2Share.g_sWarrReNewName.Replace("%chrname", ChrName);
                                break;
                            case PlayJob.Wizard:
                                sChrName = M2Share.g_sWizardReNewName.Replace("%chrname", ChrName);
                                break;
                            case PlayJob.Taoist:
                                sChrName = M2Share.g_sTaosReNewName.Replace("%chrname", ChrName);
                                break;
                        }
                    }
                    else
                    {
                        sChrName = ChrName;
                    }
                }
                else
                {
                    sChrName = Format(MSRankLevelName, ChrName);
                }
                if (!string.IsNullOrEmpty(MSMasterName))
                {
                    if (MBoMaster)
                    {
                        sMasterName = Format(M2Share.g_sMasterName, MSMasterName);
                    }
                    else
                    {
                        sMasterName = Format(M2Share.g_sNoMasterName, MSMasterName);
                    }
                }
                if (!string.IsNullOrEmpty(MSDearName))
                {
                    if (Gender == PlayGender.Man)
                    {
                        sDearName = Format(M2Share.g_sManDearName, MSDearName);
                    }
                    else
                    {
                        sDearName = Format(M2Share.g_sWoManDearName, MSDearName);
                    }
                }
                var sShowName = M2Share.g_sHumanShowName.Replace("%chrname", sChrName);
                sShowName = sShowName.Replace("%guildname", sGuildName);
                sShowName = sShowName.Replace("%dearname", sDearName);
                sShowName = sShowName.Replace("%mastername", sMasterName);
                result = sShowName;
            }
            catch (Exception e)
            {
                M2Share.Log.Error(sExceptionMsg);
                M2Share.Log.Error(e.Message);
            }
            return result;
        }

        public override void MakeGhost()
        {
            const string sExceptionMsg = "[Exception] TPlayObject::MakeGhost";
            try
            {
                if (M2Share.g_HighLevelHuman == this)
                {
                    M2Share.g_HighLevelHuman = null;
                }
                if (M2Share.g_HighPKPointHuman == this)
                {
                    M2Share.g_HighPKPointHuman = null;
                }
                if (M2Share.g_HighDCHuman == this)
                {
                    M2Share.g_HighDCHuman = null;
                }
                if (M2Share.g_HighMCHuman == this)
                {
                    M2Share.g_HighMCHuman = null;
                }
                if (M2Share.g_HighSCHuman == this)
                {
                    M2Share.g_HighSCHuman = null;
                }
                if (M2Share.g_HighOnlineHuman == this)
                {
                    M2Share.g_HighOnlineHuman = null;
                }
                // 人物下线后通知配偶，并把对方的相关记录清空
                string sSayMsg;
                if (MDearHuman != null)
                {
                    if (Gender == PlayGender.Man)
                    {
                        sSayMsg = M2Share.g_sManLongOutDearOnlineMsg.Replace("%d", MSDearName);
                        sSayMsg = sSayMsg.Replace("%s", ChrName);
                        sSayMsg = sSayMsg.Replace("%m", Envir.MapDesc);
                        sSayMsg = sSayMsg.Replace("%x", CurrX.ToString());
                        sSayMsg = sSayMsg.Replace("%y", CurrY.ToString());
                        MDearHuman.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                    }
                    else
                    {
                        sSayMsg = M2Share.g_sWoManLongOutDearOnlineMsg.Replace("%d", MSDearName);
                        sSayMsg = sSayMsg.Replace("%s", ChrName);
                        sSayMsg = sSayMsg.Replace("%m", Envir.MapDesc);
                        sSayMsg = sSayMsg.Replace("%x", CurrX.ToString());
                        sSayMsg = sSayMsg.Replace("%y", CurrY.ToString());
                        MDearHuman.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                    }
                    MDearHuman.MDearHuman = null;
                    MDearHuman = null;
                }
                if (MMasterHuman != null || MMasterList.Count > 0)
                {
                    if (MBoMaster)
                    {
                        for (var i = MMasterList.Count - 1; i >= 0; i--)
                        {
                            var human = MMasterList[i];
                            sSayMsg = M2Share.g_sMasterLongOutMasterListOnlineMsg.Replace("%s", ChrName);
                            sSayMsg = sSayMsg.Replace("%m", Envir.MapDesc);
                            sSayMsg = sSayMsg.Replace("%x", CurrX.ToString());
                            sSayMsg = sSayMsg.Replace("%y", CurrY.ToString());
                            human.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                            human.MMasterHuman = null;
                        }
                    }
                    else
                    {
                        if (MMasterHuman == null)
                        {
                            return;
                        }
                        sSayMsg = M2Share.g_sMasterListLongOutMasterOnlineMsg.Replace("%d", MSMasterName);
                        sSayMsg = sSayMsg.Replace("%s", ChrName);
                        sSayMsg = sSayMsg.Replace("%m", Envir.MapDesc);
                        sSayMsg = sSayMsg.Replace("%x", CurrX.ToString());
                        sSayMsg = sSayMsg.Replace("%y", CurrY.ToString());
                        MMasterHuman.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                        // 如果为大徒弟则将对方的记录清空
                        if (MMasterHuman.MSMasterName == ChrName)
                        {
                            MMasterHuman.MMasterHuman = null;
                        }
                        for (var i = 0; i < MMasterHuman.MMasterList.Count; i++)
                        {
                            if (MMasterHuman.MMasterList[i] == this)
                            {
                                MMasterHuman.MMasterList.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.Log.Error(sExceptionMsg);
                M2Share.Log.Error(e.Message);
            }
            base.MakeGhost();
        }

        internal override void ScatterBagItems(BaseObject itemOfCreat)
        {
            const int dropWide = 2;
            if (AngryRing || NoDropItem || Envir.Flag.boNODROPITEM)
            {
                return;// 不死戒指
            }
            const string sExceptionMsg = "[Exception] TPlayObject::ScatterBagItems";
            try
            {
                if (ItemList.Count > 0)
                {
                    IList<DeleteItem> delList = new List<DeleteItem>();
                    var boDropall = M2Share.Config.DieRedScatterBagAll && PvpLevel() >= 2;
                    for (var i = ItemList.Count - 1; i >= 0; i--)
                    {
                        if (boDropall || M2Share.RandomNumber.Random(M2Share.Config.DieScatterBagRate) == 0)
                        {
                            if (DropItemDown(ItemList[i], dropWide, true, itemOfCreat, this))
                            {
                                if (Race == ActorRace.Play)
                                {
                                    delList.Add(new DeleteItem()
                                    {
                                        ItemName = M2Share.WorldEngine.GetStdItemName(ItemList[i].Index),
                                        MakeIndex = ItemList[i].MakeIndex
                                    });
                                }
                                Dispose(ItemList[i]);
                                ItemList.RemoveAt(i);
                            }
                        }
                    }
                    if (delList.Count > 0)
                    {
                        var objectId = HUtil32.Sequence();
                        M2Share.ActorMgr.AddOhter(objectId, delList);
                        SendMsg(this, Grobal2.RM_SENDDELITEMLIST, 0, objectId, 0, 0, "");
                    }
                }
            }
            catch
            {
                M2Share.Log.Error(sExceptionMsg);
            }
        }

        internal void AddItemSkill(int nIndex)
        {
            MagicInfo magic = null;
            switch (nIndex)
            {
                case 1:
                    magic = M2Share.WorldEngine.FindMagic(M2Share.Config.FireBallSkill);
                    break;
                case 2:
                    magic = M2Share.WorldEngine.FindMagic(M2Share.Config.HealSkill);
                    break;
            }
            if (magic != null)
            {
                if (!IsTrainingSkill(magic.MagicId))
                {
                    var userMagic = new UserMagic
                    {
                        Magic = magic,
                        MagIdx = magic.MagicId,
                        Key = (char)0,
                        Level = 1,
                        TranPoint = 0
                    };
                    MagicList.Add(userMagic);
                    SendAddMagic(userMagic);
                }
            }
        }

        public void SetExpiredTime(int expiredTime)
        {
            if (Abil.Level > M2Share.g_nExpErienceLevel)
            {
                ExpireTime = HUtil32.GetTickCount() + (60 * 1000);
                ExpireCount = expiredTime;
            }
        }

        private void CheckExpiredTime()
        {
            ExpireCount = ExpireCount - 1;
            switch (ExpireCount)
            {
                case 30:
                    SysMsg("您的账号游戏时间即将到期，您将在[30:00]分钟后断开服务器。", MsgColor.Blue, MsgType.System);
                    break;
                case > 0 and <= 10:
                    SysMsg($"您的账号游戏时间即将到期，您将在[{ExpireCount}:00]分钟后断开服务器。", MsgColor.Blue, MsgType.System);
                    break;
                case <= 0:
                    ExpireTime = 0;
                    ExpireCount = 0;
                    AccountExpired = true;
                    SysMsg("您的账号游戏时间已到，请访问(https://mir2.sdo.com)进行充值，全服务器账号共享游戏时间。", MsgColor.Blue, MsgType.System);
                    break;
            }
        }

        public int GetQuestFalgStatus(int nFlag)
        {
            var result = 0;
            nFlag -= 1;
            if (nFlag < 0)
            {
                return result;
            }
            var n10 = nFlag / 8;
            var n14 = nFlag % 8;
            if ((n10 - QuestFlag.Length) < 0)
            {
                if (((128 >> n14) & QuestFlag[n10]) != 0)
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                }
            }
            return result;
        }

        public void SetQuestFlagStatus(int nFlag, int nValue)
        {
            nFlag -= 1;
            if (nFlag < 0)
            {
                return;
            }
            var n10 = nFlag / 8;
            var n14 = nFlag % 8;
            if ((n10 - QuestFlag.Length) < 0)
            {
                var bt15 = QuestFlag[n10];
                if (nValue == 0)
                {
                    QuestFlag[n10] = (byte)((~(128 >> n14)) & bt15);
                }
                else
                {
                    QuestFlag[n10] = (byte)((128 >> n14) | bt15);
                }
            }
        }

        public int GetQuestUnitOpenStatus(int nFlag)
        {
            var result = 0;
            nFlag -= 1;
            if (nFlag < 0)
            {
                return result;
            }
            var n10 = nFlag / 8;
            var n14 = nFlag % 8;
            if ((n10 - QuestUnitOpen.Length) < 0)
            {
                if (((128 >> n14) & QuestUnitOpen[n10]) != 0)
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                }
            }
            return result;
        }

        public void SetQuestUnitOpenStatus(int nFlag, int nValue)
        {
            nFlag -= 1;
            if (nFlag < 0)
            {
                return;
            }
            var n10 = nFlag / 8;
            var n14 = nFlag % 8;
            if ((n10 - QuestUnitOpen.Length) < 0)
            {
                var bt15 = QuestUnitOpen[n10];
                if (nValue == 0)
                {
                    QuestUnitOpen[n10] = (byte)((~(128 >> n14)) & bt15);
                }
                else
                {
                    QuestUnitOpen[n10] = (byte)((128 >> n14) | bt15);
                }
            }
        }

        public int GetQuestUnitStatus(int nFlag)
        {
            var result = 0;
            nFlag -= 1;
            if (nFlag < 0)
            {
                return result;
            }
            var n10 = nFlag / 8;
            var n14 = nFlag % 8;
            if ((n10 - QuestUnit.Length) < 0)
            {
                if (((128 >> n14) & QuestUnit[n10]) != 0)
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                }
            }
            return result;
        }

        public void SetQuestUnitStatus(int nFlag, int nValue)
        {
            nFlag -= 1;
            if (nFlag < 0)
            {
                return;
            }
            var n10 = nFlag / 8;
            var n14 = nFlag % 8;
            if ((n10 - QuestUnit.Length) < 0)
            {
                var bt15 = QuestUnit[n10];
                if (nValue == 0)
                {
                    QuestUnit[n10] = (byte)((~(128 >> n14)) & bt15);
                }
                else
                {
                    QuestUnit[n10] = (byte)((128 >> n14) | bt15);
                }
            }
        }

        protected override byte GetNamecolor()
        {
            var result = base.GetNamecolor();
            var pvpLevel = PvpLevel();
            switch (pvpLevel)
            {
                case 1:
                    result = M2Share.Config.btPKLevel1NameColor;
                    break;
                case >= 2:
                    result = M2Share.Config.btPKLevel2NameColor;
                    break;
            }
            return result;
        }

        protected override bool IsAttackTarget(BaseObject baseObject)
        {
            var result = base.IsAttackTarget(baseObject);
            if (Race > ActorRace.Play) return result;
            switch (AttatckMode)
            {
                case AttackMode.HAM_ALL:
                    if ((baseObject.Race < ActorRace.NPC) || (baseObject.Race > ActorRace.PeaceNpc))
                    {
                        result = true;
                    }
                    if (M2Share.Config.PveServer)
                    {
                        result = true;
                    }
                    break;
                case AttackMode.HAM_PEACE:
                    if (baseObject.Race >= ActorRace.Animal)
                    {
                        result = true;
                    }
                    break;
                case AttackMode.HAM_DEAR:
                    if (baseObject != MDearHuman)
                    {
                        result = true;
                    }
                    break;
                case AttackMode.HAM_MASTER:
                    if (baseObject.Race == ActorRace.Play)
                    {
                        result = true;
                        if (MBoMaster)
                        {
                            for (var i = 0; i < MMasterList.Count; i++)
                            {
                                if (MMasterList[i] == baseObject)
                                {
                                    result = false;
                                    break;
                                }
                            }
                        }
                        if (((PlayObject)baseObject).MBoMaster)
                        {
                            for (var i = 0; i < ((PlayObject)baseObject).MMasterList.Count; i++)
                            {
                                if (((PlayObject)baseObject).MMasterList[i] == this)
                                {
                                    result = false;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        result = true;
                    }
                    break;
                case AttackMode.HAM_GROUP:
                    if ((baseObject.Race < ActorRace.NPC) || (baseObject.Race > ActorRace.PeaceNpc))
                    {
                        result = true;
                    }
                    if (baseObject.Race == ActorRace.Play)
                    {
                        if (IsGroupMember(baseObject))
                        {
                            result = false;
                        }
                    }
                    if (M2Share.Config.PveServer)
                    {
                        result = true;
                    }
                    break;
                case AttackMode.HAM_GUILD:
                    if ((baseObject.Race < ActorRace.NPC) || (baseObject.Race > ActorRace.PeaceNpc))
                    {
                        result = true;
                    }
                    if (baseObject.Race == ActorRace.Play)
                    {
                        if (MyGuild != null)
                        {
                            if (MyGuild.IsMember(baseObject.ChrName))
                            {
                                result = false;
                            }
                            if (GuildWarArea && (((PlayObject)baseObject).MyGuild != null))
                            {
                                if (MyGuild.IsAllyGuild(((PlayObject)baseObject).MyGuild))
                                {
                                    result = false;
                                }
                            }
                        }
                    }
                    if (M2Share.Config.PveServer)
                    {
                        result = true;
                    }
                    break;
                case AttackMode.HAM_PKATTACK:
                    if ((baseObject.Race < ActorRace.NPC) || (baseObject.Race > ActorRace.PeaceNpc))
                    {
                        result = true;
                    }
                    if (baseObject.Race == ActorRace.Play)
                    {
                        if (PvpLevel() >= 2)
                        {
                            result = ((PlayObject)baseObject).PvpLevel() < 2;
                        }
                        else
                        {
                            result = ((PlayObject)baseObject).PvpLevel() >= 2;
                        }
                    }
                    if (M2Share.Config.PveServer)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        public override bool IsProperFriend(BaseObject attackTarget)
        {
            if (attackTarget.Race == ActorRace.Play)
            {
                var result = IsProperIsFriend(attackTarget);
                if (attackTarget.Race < ActorRace.Animal)
                {
                    return result;
                }
                if (attackTarget.Master == this)
                {
                    return true;
                }
                if (attackTarget.Master != null)
                {
                    return IsProperIsFriend(attackTarget.Master);
                }
                if (attackTarget.Race > ActorRace.Play) return result;
                var targetObject = (PlayObject)attackTarget;
                if (!targetObject.InGuildWarArea)
                {
                    if (M2Share.Config.boPKLevelProtect)// 新人保护
                    {
                        if (Abil.Level > M2Share.Config.nPKProtectLevel)// 如果大于指定等级
                        {
                            if (!targetObject.PvpFlag && targetObject.WAbil.Level <= M2Share.Config.nPKProtectLevel && targetObject.PvpLevel() < 2)// 被攻击的人物小指定等级没有红名，则不可以攻击。
                            {
                                return false;
                            }
                        }
                        if (Abil.Level <= M2Share.Config.nPKProtectLevel)// 如果小于指定等级
                        {
                            if (!targetObject.PvpFlag && targetObject.WAbil.Level > M2Share.Config.nPKProtectLevel && targetObject.PvpLevel() < 2)
                            {
                                return false;
                            }
                        }
                    }
                    // 大于指定级别的红名人物不可以杀指定级别未红名的人物。
                    if (PvpLevel() >= 2 && Abil.Level > M2Share.Config.nRedPKProtectLevel)
                    {
                        if (targetObject.Abil.Level <= M2Share.Config.nRedPKProtectLevel && targetObject.PvpLevel() < 2)
                        {
                            return false;
                        }
                    }
                    // 小于指定级别的非红名人物不可以杀指定级别红名人物。
                    if (Abil.Level <= M2Share.Config.nRedPKProtectLevel && PvpLevel() < 2)
                    {
                        if (targetObject.PvpLevel() >= 2 && targetObject.Abil.Level > M2Share.Config.nRedPKProtectLevel)
                        {
                            return false;
                        }
                    }
                    if (((HUtil32.GetTickCount() - MapMoveTick) < 3000) || ((HUtil32.GetTickCount() - targetObject.MapMoveTick) < 3000))
                    {
                        result = false;
                    }
                }
                return result;
            }
            return base.IsProperFriend(attackTarget);
        }

        protected override byte GetChrColor(BaseObject baseObject)
        {
            if (baseObject.Race == ActorRace.Play)
            {
                byte result = NameColor;
                var targetObject = (PlayObject)baseObject;
                if (targetObject.PvpLevel() < 2)
                {
                    if (targetObject.PvpFlag)
                    {
                        result = M2Share.Config.btPKFlagNameColor;
                    }
                    var n10 = GetGuildRelation(this, targetObject);
                    switch (n10)
                    {
                        case 1:
                        case 3:
                            result = M2Share.Config.btAllyAndGuildNameColor;
                            break;
                        case 2:
                            result = M2Share.Config.WarGuildNameColor;
                            break;
                    }
                    if (targetObject.Envir.Flag.boFight3Zone)
                    {
                        result = MyGuild == targetObject.MyGuild ? M2Share.Config.btAllyAndGuildNameColor : M2Share.Config.WarGuildNameColor;
                    }
                }
                var castle = M2Share.CastleMgr.InCastleWarArea(targetObject);
                if ((castle != null) && castle.UnderWar && InGuildWarArea && targetObject.InGuildWarArea)
                {
                    result = M2Share.Config.InFreePKAreaNameColor;
                    GuildWarArea = true;
                    if (MyGuild == null)
                    {
                        return result;
                    }
                    if (castle.IsMasterGuild(MyGuild))
                    {
                        if ((MyGuild == targetObject.MyGuild) || MyGuild.IsAllyGuild(targetObject.MyGuild))
                        {
                            result = M2Share.Config.btAllyAndGuildNameColor;
                        }
                        else
                        {
                            if (castle.IsAttackGuild(targetObject.MyGuild))
                            {
                                result = M2Share.Config.WarGuildNameColor;
                            }
                        }
                    }
                    else
                    {
                        if (castle.IsAttackGuild(MyGuild))
                        {
                            if ((MyGuild == targetObject.MyGuild) || MyGuild.IsAllyGuild(targetObject.MyGuild))
                            {
                                result = M2Share.Config.btAllyAndGuildNameColor;
                            }
                            else
                            {
                                if (castle.IsMember(targetObject))
                                {
                                    result = M2Share.Config.WarGuildNameColor;
                                }
                            }
                        }
                    }
                }
                return result;
            }
            return base.GetChrColor(baseObject);
        }

        protected override void RecalcHitSpeed()
        {
            if (Race == ActorRace.Play)
            {
                NakedAbility bonusTick = null;
                switch (Job)
                {
                    case PlayJob.Warrior:
                        bonusTick = M2Share.Config.BonusAbilofWarr;
                        HitPoint = (byte)(M2Share.DEFHIT + BonusAbil.Hit / bonusTick.Hit);
                        SpeedPoint = (byte)(M2Share.DEFSPEED + BonusAbil.Speed / bonusTick.Speed);
                        break;
                    case PlayJob.Wizard:
                        bonusTick = M2Share.Config.BonusAbilofWizard;
                        HitPoint = (byte)(M2Share.DEFHIT + BonusAbil.Hit / bonusTick.Hit);
                        SpeedPoint = (byte)(M2Share.DEFSPEED + BonusAbil.Speed / bonusTick.Speed);
                        break;
                    case PlayJob.Taoist:
                        bonusTick = M2Share.Config.BonusAbilofTaos;
                        SpeedPoint = (byte)(M2Share.DEFSPEED + BonusAbil.Speed / bonusTick.Speed + 3);
                        break;
                }
            }
            for (var i = 0; i < MagicList.Count; i++)
            {
                var userMagic = MagicList[i];
                MagicArr[userMagic.MagIdx] = userMagic;
                switch (userMagic.MagIdx)
                {
                    case MagicConst.SKILL_ONESWORD: // 基本剑法
                        if (userMagic.Level > 0)
                        {
                            HitPoint = (byte)(HitPoint + HUtil32.Round(9 / 3 * userMagic.Level));
                        }
                        break;
                    case MagicConst.SKILL_ILKWANG: // 精神力战法
                        if (userMagic.Level > 0)
                        {
                            HitPoint = (byte)(HitPoint + HUtil32.Round(8 / 3 * userMagic.Level));
                        }
                        break;
                    case MagicConst.SKILL_YEDO: // 攻杀剑法
                        if (userMagic.Level > 0)
                        {
                            HitPoint = (byte)(HitPoint + HUtil32.Round(3 / 3 * userMagic.Level));
                        }
                        HitPlus = (byte)(M2Share.DEFHIT + userMagic.Level);
                        AttackSkillCount = (byte)(7 - userMagic.Level);
                        AttackSkillPointCount = M2Share.RandomNumber.RandomByte(AttackSkillCount);
                        break;
                    case MagicConst.SKILL_FIRESWORD: // 烈火剑法
                        HitDouble = (byte)(4 + userMagic.Level * 4);
                        break;
                }
            }
            base.RecalcHitSpeed();
        }

        protected UserMagic GetMagicInfo(int nMagicId)
        {
            for (var i = 0; i < MagicList.Count; i++)
            {
                var userMagic = MagicList[i];
                if (userMagic.Magic.MagicId == nMagicId)
                {
                    return userMagic;
                }
            }
            return null;
        }

        public bool IsTrainingSkill(int nIndex)
        {
            for (var i = 0; i < MagicList.Count; i++)
            {
                var userMagic = MagicList[i];
                if ((userMagic != null) && (userMagic.MagIdx == nIndex))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsProperIsFriend(BaseObject cret)
        {
            var result = false;
            if (cret.Race == ActorRace.Play)
            {
                switch (AttatckMode)
                {
                    case AttackMode.HAM_ALL:
                        result = true;
                        break;
                    case AttackMode.HAM_PEACE:
                        result = true;
                        break;
                    case AttackMode.HAM_DEAR:
                        if ((this == cret) || (cret == MDearHuman))
                        {
                            result = true;
                        }
                        break;
                    case AttackMode.HAM_MASTER:
                        if (this == cret)
                        {
                            result = true;
                        }
                        else if (MBoMaster)
                        {
                            for (var i = 0; i < MMasterList.Count; i++)
                            {
                                if (MMasterList[i] == cret)
                                {
                                    result = true;
                                    break;
                                }
                            }
                        }
                        else if (((PlayObject)cret).MBoMaster)
                        {
                            for (var i = 0; i < ((PlayObject)cret).MMasterList.Count; i++)
                            {
                                if (((PlayObject)cret).MMasterList[i] == this)
                                {
                                    result = true;
                                    break;
                                }
                            }
                        }
                        break;
                    case AttackMode.HAM_GROUP:
                        if (cret == this)
                        {
                            result = true;
                        }
                        if (IsGroupMember(cret))
                        {
                            result = true;
                        }
                        break;
                    case AttackMode.HAM_GUILD:
                        if (cret == this)
                        {
                            result = true;
                        }
                        if (MyGuild != null)
                        {
                            if (MyGuild.IsMember(cret.ChrName))
                            {
                                result = true;
                            }
                            if (GuildWarArea && ((cret as PlayObject).MyGuild != null))
                            {
                                if (MyGuild.IsAllyGuild((cret as PlayObject).MyGuild))
                                {
                                    result = true;
                                }
                            }
                        }
                        break;
                    case AttackMode.HAM_PKATTACK:
                        if (cret == this)
                        {
                            result = true;
                        }
                        if (PvpLevel() >= 2)
                        {
                            if ((cret as PlayObject).PvpLevel() < 2)
                            {
                                result = true;
                            }
                        }
                        else
                        {
                            if ((cret as PlayObject).PvpLevel() >= 2)
                            {
                                result = true;
                            }
                        }
                        break;
                }
            }
            return result;
        }

        internal void AddBodyLuck(double dLuck)
        {
            if ((dLuck > 0) && (BodyLuck < 5 * M2Share.BODYLUCKUNIT))
            {
                BodyLuck = BodyLuck + dLuck;
            }
            if ((dLuck < 0) && (BodyLuck > -(5 * M2Share.BODYLUCKUNIT)))
            {
                BodyLuck = BodyLuck + dLuck;
            }
            var n = Convert.ToInt32(BodyLuck / M2Share.BODYLUCKUNIT);
            if (n > 5)
            {
                n = 5;
            }
            if (n < -10)
            {
                n = -10;
            }
            BodyLuckLevel = n;
        }

        public void SetPkFlag(BaseObject baseObject)
        {
            if (baseObject.Race == ActorRace.Play)
            {
                var targetObject = (PlayObject)baseObject;
                if ((PvpLevel() < 2) && (targetObject.PvpLevel() < 2) && (!Envir.Flag.boFightZone) && (!Envir.Flag.boFight3Zone) && !PvpFlag)
                {
                    targetObject.PvpNameColorTick = HUtil32.GetTickCount();
                    if (!targetObject.PvpFlag)
                    {
                        targetObject.PvpFlag = true;
                        targetObject.RefNameColor();
                    }
                }
            }
        }

        public void ChangePkStatus(bool boWarFlag)
        {
            if (InGuildWarArea != boWarFlag)
            {
                InGuildWarArea = boWarFlag;
                NameColorChanged = true;
            }
        }

        public byte PvpLevel()
        {
            return (byte)(PkPoint / 100);
        }

        internal void CheckPkStatus()
        {
            if (PvpFlag && ((HUtil32.GetTickCount() - PvpNameColorTick) > M2Share.Config.dwPKFlagTime)) // 60 * 1000
            {
                PvpFlag = false;
                RefNameColor();
            }
        }

        public void IncPkPoint(int nPoint)
        {
            var oldPvpLevel = PvpLevel();
            PkPoint += nPoint;
            if (PvpLevel() != oldPvpLevel)
            {
                if (PvpLevel() <= 2)
                {
                    RefNameColor();
                }
            }
        }

        private void DecPkPoint(int nPoint)
        {
            var pvpLevel = PvpLevel();
            PkPoint -= nPoint;
            if (PkPoint < 0)
            {
                PkPoint = 0;
            }
            if ((PvpLevel() != pvpLevel) && (pvpLevel > 0) && (pvpLevel <= 2))
            {
                RefNameColor();
            }
        }

        public void TrainSkill(UserMagic userMagic, int nTranPoint)
        {
            if (FastTrain)
            {
                nTranPoint = nTranPoint * 3;
            }
            userMagic.TranPoint += nTranPoint;
        }

        public bool IsGuildMaster()
        {
            return (MyGuild != null) && (GuildRankNo == 1);
        }

        internal void ApplyItemParameters(UserItem uitem, StdItem stdItem, ref AddAbility aabil)
        {
            var item = M2Share.WorldEngine.GetStdItem(uitem.Index);
            if (item != null)
            {
                var clientItem = new ClientItem();
                item.GetUpgradeStdItem(uitem, ref clientItem);
                ApplyItemParametersByJob(uitem, ref clientItem);
                switch (item.StdMode)
                {
                    case 5:
                    case 6:
                        aabil.HIT = (ushort)(aabil.HIT + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed + stdItem.RealAttackSpeed(HUtil32.HiByte(clientItem.Item.MAC)));
                        aabil.Luck = (byte)(aabil.Luck + HUtil32.LoByte(clientItem.Item.AC));
                        aabil.UnLuck = (byte)(aabil.UnLuck + HUtil32.LoByte(clientItem.Item.MAC));
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        if (clientItem.Item.SpecialPwr >= 1 && clientItem.Item.SpecialPwr <= 10)
                        {
                            aabil.WeaponStrong = (byte)clientItem.Item.SpecialPwr;
                        }
                        break;
                    case 10:
                    case 11:
                        aabil.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC)), (ushort)(HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC)));
                        aabil.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC)), (ushort)(HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC)));
                        aabil.SPEED = (ushort)(aabil.SPEED + clientItem.Item.Agility);
                        aabil.AntiMagic = (ushort)(aabil.AntiMagic + clientItem.Item.MgAvoid);
                        aabil.AntiPoison = (ushort)(aabil.AntiPoison + clientItem.Item.ToxAvoid);
                        aabil.HP = (ushort)(aabil.HP + clientItem.Item.HpAdd);
                        aabil.MP = (ushort)(aabil.MP + clientItem.Item.MpAdd);
                        if (clientItem.Item.EffType1 > 0)
                        {
                            switch (clientItem.Item.EffType1)
                            {
                                case EfftypeConst.EFFTYPE_HP_MP_ADD:
                                    if ((aabil.HealthRecover + clientItem.Item.EffRate1 > 65000))
                                    {
                                        aabil.HealthRecover = 65000;
                                    }
                                    else
                                    {
                                        aabil.HealthRecover = (ushort)(aabil.HealthRecover + clientItem.Item.EffRate1);
                                    }
                                    if ((aabil.SpellRecover + clientItem.Item.EffValue1 > 65000))
                                    {
                                        aabil.SpellRecover = 65000;
                                    }
                                    else
                                    {
                                        aabil.SpellRecover = (ushort)(aabil.SpellRecover + clientItem.Item.EffValue1);
                                    }
                                    break;
                            }
                        }
                        if (clientItem.Item.EffType2 > 0)
                        {
                            switch (clientItem.Item.EffType2)
                            {
                                case EfftypeConst.EFFTYPE_HP_MP_ADD:
                                    if ((aabil.HealthRecover + clientItem.Item.EffRate2 > 65000))
                                    {
                                        aabil.HealthRecover = 65000;
                                    }
                                    else
                                    {
                                        aabil.HealthRecover = (ushort)(aabil.HealthRecover + clientItem.Item.EffRate2);
                                    }
                                    if ((aabil.SpellRecover + clientItem.Item.EffValue2 > 65000))
                                    {
                                        aabil.SpellRecover = 65000;
                                    }
                                    else
                                    {
                                        aabil.SpellRecover = (ushort)(aabil.SpellRecover + clientItem.Item.EffValue2);
                                    }
                                    break;
                            }
                        }
                        if (clientItem.Item.EffType1 == EfftypeConst.EFFTYPE_LUCK_ADD)
                        {
                            if (aabil.Luck + clientItem.Item.EffValue1 > 255)
                            {
                                aabil.Luck = byte.MaxValue;
                            }
                            else
                            {
                                aabil.Luck = (byte)(aabil.Luck + clientItem.Item.EffValue1);
                            }
                        }
                        else if (clientItem.Item.EffType2 == EfftypeConst.EFFTYPE_LUCK_ADD)
                        {
                            if (aabil.Luck + clientItem.Item.EffValue2 > 255)
                            {
                                aabil.Luck = byte.MaxValue;
                            }
                            else
                            {
                                aabil.Luck = (byte)(aabil.Luck + clientItem.Item.EffValue2);
                            }
                        }
                        break;
                    case 15:
                        aabil.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC)), (ushort)(HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC)));
                        aabil.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC)), (ushort)(HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC)));
                        aabil.HIT = (ushort)(aabil.HIT + clientItem.Item.Accurate);
                        aabil.AntiMagic = (ushort)(aabil.AntiMagic + clientItem.Item.MgAvoid);
                        aabil.AntiPoison = (ushort)(aabil.AntiPoison + clientItem.Item.ToxAvoid);
                        break;
                    case 19:
                        aabil.AntiMagic = (ushort)(aabil.AntiMagic + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.UnLuck = (byte)(aabil.UnLuck + HUtil32.LoByte(clientItem.Item.MAC));
                        aabil.Luck = (byte)(aabil.Luck + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed + clientItem.Item.AtkSpd);
                        aabil.HIT = (ushort)(aabil.HIT + clientItem.Item.Accurate);
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        break;
                    case 20:
                        aabil.HIT = (ushort)(aabil.HIT + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.SPEED = (ushort)(aabil.SPEED + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed + clientItem.Item.AtkSpd);
                        aabil.AntiMagic = (ushort)(aabil.AntiMagic + clientItem.Item.MgAvoid);
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        break;
                    case 21:
                        aabil.HealthRecover = (ushort)(aabil.HealthRecover + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.SpellRecover = (ushort)(aabil.SpellRecover + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed + HUtil32.LoByte(clientItem.Item.AC));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed - HUtil32.LoByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed + clientItem.Item.AtkSpd);
                        aabil.HIT = (ushort)(aabil.HIT + clientItem.Item.Accurate);
                        aabil.AntiMagic = (ushort)(aabil.AntiMagic + clientItem.Item.MgAvoid);
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        break;
                    case 22:
                        aabil.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC)), (ushort)(HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC)));
                        aabil.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC)), (ushort)(HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC)));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed + clientItem.Item.AtkSpd);
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        aabil.HIT = (ushort)(aabil.HIT + clientItem.Item.Accurate);
                        aabil.HP = (ushort)(aabil.HP + clientItem.Item.HpAdd);
                        break;
                    case 23:
                        aabil.AntiPoison = (ushort)(aabil.AntiPoison + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.PoisonRecover = (ushort)(aabil.PoisonRecover + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed + HUtil32.LoByte(clientItem.Item.AC));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed - HUtil32.LoByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed + clientItem.Item.AtkSpd);
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        break;
                    case 24:
                    case 26:
                        if (clientItem.Item.SpecialPwr >= 1 && clientItem.Item.SpecialPwr <= 10)
                        {
                            aabil.WeaponStrong = (byte)clientItem.Item.SpecialPwr;
                        }
                        switch (item.StdMode)
                        {
                            case 24:
                                aabil.HIT = (ushort)(aabil.HIT + HUtil32.HiByte(clientItem.Item.AC));
                                aabil.SPEED = (ushort)(aabil.SPEED + HUtil32.HiByte(clientItem.Item.MAC));
                                break;
                            case 26:
                                aabil.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC)), (ushort)(HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC)));
                                aabil.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC)), (ushort)(HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC)));
                                aabil.HIT = (ushort)(aabil.HIT + clientItem.Item.Accurate);
                                aabil.SPEED = (ushort)(aabil.SPEED + clientItem.Item.Agility);
                                aabil.MP = (ushort)(aabil.MP + clientItem.Item.MpAdd);
                                break;
                        }
                        break;
                    case 52:
                        aabil.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC)), (ushort)(HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC)));
                        aabil.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC)), (ushort)(HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC)));
                        aabil.SPEED = (ushort)(aabil.SPEED + clientItem.Item.Agility);
                        break;
                    case 54:
                        aabil.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC)), (ushort)(HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC)));
                        aabil.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC)), (ushort)(HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC)));
                        aabil.HIT = (ushort)(aabil.HIT + clientItem.Item.Accurate);
                        aabil.SPEED = (ushort)(aabil.SPEED + clientItem.Item.Agility);
                        aabil.AntiPoison = (ushort)(aabil.AntiPoison + clientItem.Item.ToxAvoid);
                        break;
                    case 53:
                        aabil.HP = (ushort)(aabil.HP + clientItem.Item.HpAdd);
                        aabil.MP = (ushort)(aabil.MP + clientItem.Item.MpAdd);
                        break;
                    default:
                        aabil.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC)), (ushort)(HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC)));
                        aabil.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC)), (ushort)(HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC)));
                        break;
                }
                aabil.DC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.DC) + HUtil32.LoByte(clientItem.Item.DC)), (ushort)HUtil32._MIN(255, HUtil32.HiByte(aabil.DC) + HUtil32.HiByte(clientItem.Item.DC)));
                aabil.MC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.MC) + HUtil32.LoByte(clientItem.Item.MC)), (ushort)HUtil32._MIN(255, HUtil32.HiByte(aabil.MC) + HUtil32.HiByte(clientItem.Item.MC)));
                aabil.SC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.SC) + HUtil32.LoByte(clientItem.Item.SC)), (ushort)HUtil32._MIN(255, HUtil32.HiByte(aabil.SC) + HUtil32.HiByte(clientItem.Item.SC)));
            }
        }

        internal void ApplyItemParametersEx(UserItem uitem, ref Ability aWabil)
        {
            StdItem item = M2Share.WorldEngine.GetStdItem(uitem.Index);
            if (item != null)
            {
                var clientItem = new ClientItem();
                item.GetUpgradeStdItem(uitem, ref clientItem);
                switch (item.StdMode)
                {
                    case 52:
                        if (clientItem.Item.EffType1 > 0)
                        {
                            switch (clientItem.Item.EffType1)
                            {
                                case EfftypeConst.EFFTYPE_TWOHAND_WEHIGHT_ADD:
                                    if ((aWabil.MaxHandWeight + clientItem.Item.EffValue1 > 255))
                                    {
                                        aWabil.MaxHandWeight = byte.MaxValue;
                                    }
                                    else
                                    {
                                        aWabil.MaxHandWeight = (byte)(aWabil.MaxHandWeight + clientItem.Item.EffValue1);
                                    }
                                    break;
                                case EfftypeConst.EFFTYPE_EQUIP_WHEIGHT_ADD:
                                    if ((aWabil.MaxWearWeight + clientItem.Item.EffValue1 > 255))
                                    {
                                        aWabil.MaxWearWeight = byte.MaxValue;
                                    }
                                    else
                                    {
                                        aWabil.MaxWearWeight = (byte)(aWabil.MaxWearWeight + clientItem.Item.EffValue1);
                                    }
                                    break;
                            }
                        }
                        if (clientItem.Item.EffType2 > 0)
                        {
                            switch (clientItem.Item.EffType2)
                            {
                                case EfftypeConst.EFFTYPE_TWOHAND_WEHIGHT_ADD:
                                    if ((aWabil.MaxHandWeight + clientItem.Item.EffValue2 > 255))
                                    {
                                        aWabil.MaxHandWeight = byte.MaxValue;
                                    }
                                    else
                                    {
                                        aWabil.MaxHandWeight = (byte)(aWabil.MaxHandWeight + clientItem.Item.EffValue2);
                                    }
                                    break;
                                case EfftypeConst.EFFTYPE_EQUIP_WHEIGHT_ADD:
                                    if ((aWabil.MaxWearWeight + clientItem.Item.EffValue2 > 255))
                                    {
                                        aWabil.MaxWearWeight = byte.MaxValue;
                                    }
                                    else
                                    {
                                        aWabil.MaxWearWeight = (byte)(aWabil.MaxWearWeight + clientItem.Item.EffValue2);
                                    }
                                    break;
                            }
                        }
                        break;
                    case 54:
                        if (clientItem.Item.EffType1 > 0)
                        {
                            switch (clientItem.Item.EffType1)
                            {
                                case EfftypeConst.EFFTYPE_BAG_WHIGHT_ADD:
                                    if ((aWabil.MaxWeight + clientItem.Item.EffValue1 > 65000))
                                    {
                                        aWabil.MaxWeight = 65000;
                                    }
                                    else
                                    {
                                        aWabil.MaxWeight = (ushort)(aWabil.MaxWeight + clientItem.Item.EffValue1);
                                    }
                                    break;
                            }
                        }
                        if (clientItem.Item.EffType2 > 0)
                        {
                            switch (clientItem.Item.EffType2)
                            {
                                case EfftypeConst.EFFTYPE_BAG_WHIGHT_ADD:
                                    if ((aWabil.MaxWeight + clientItem.Item.EffValue2 > 65000))
                                    {
                                        aWabil.MaxWeight = 65000;
                                    }
                                    else
                                    {
                                        aWabil.MaxWeight = (ushort)(aWabil.MaxWeight + clientItem.Item.EffValue2);
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
        }

        protected void ChangeItemByJob(ref ClientItem citem, int level)
        {
            if ((citem.Item.StdMode == 22) && (citem.Item.Shape == DragonConst.DRAGON_RING_SHAPE))
            {
                switch (Job)
                {
                    case PlayJob.Warrior:
                        citem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC) + 4));
                        citem.Item.MC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Wizard:
                        citem.Item.DC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Taoist:
                        citem.Item.MC = 0;
                        break;
                }
            }
            else if ((citem.Item.StdMode == 26) && (citem.Item.Shape == DragonConst.DRAGON_BRACELET_SHAPE))
            {
                switch (Job)
                {
                    case PlayJob.Warrior:
                        citem.Item.DC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(citem.Item.DC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC) + 2));
                        citem.Item.MC = 0;
                        citem.Item.SC = 0;
                        citem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.AC) + 1));
                        break;
                    case PlayJob.Wizard:
                        citem.Item.DC = 0;
                        citem.Item.SC = 0;
                        citem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.AC) + 1));
                        break;
                    case PlayJob.Taoist:
                        citem.Item.MC = 0;
                        break;
                }
            }
            else if ((citem.Item.StdMode == 19) && (citem.Item.Shape == DragonConst.DRAGON_NECKLACE_SHAPE))
            {
                switch (Job)
                {
                    case PlayJob.Warrior:
                        citem.Item.MC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Wizard:
                        citem.Item.DC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Taoist:
                        citem.Item.DC = 0;
                        citem.Item.MC = 0;
                        break;
                }
            }
            else if (((citem.Item.StdMode == 10) || (citem.Item.StdMode == 11)) && (citem.Item.Shape == DragonConst.DRAGON_DRESS_SHAPE))
            {
                switch (Job)
                {
                    case PlayJob.Warrior:
                        citem.Item.MC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Wizard:
                        citem.Item.DC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Taoist:
                        citem.Item.DC = 0;
                        citem.Item.MC = 0;
                        break;
                }
            }
            else if ((citem.Item.StdMode == 15) && (citem.Item.Shape == DragonConst.DRAGON_HELMET_SHAPE))
            {
                switch (Job)
                {
                    case PlayJob.Warrior:
                        citem.Item.MC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Wizard:
                        citem.Item.DC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Taoist:
                        citem.Item.DC = 0;
                        citem.Item.MC = 0;
                        break;
                }
            }
            else if (((citem.Item.StdMode == 5) || (citem.Item.StdMode == 6)) && (citem.Item.Shape == DragonConst.DRAGON_WEAPON_SHAPE))
            {
                switch (Job)
                {
                    case PlayJob.Warrior:
                        citem.Item.DC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(citem.Item.DC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC) + 28));
                        citem.Item.MC = 0;
                        citem.Item.SC = 0;
                        citem.Item.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(citem.Item.AC) - 2), HUtil32.HiByte(citem.Item.AC));
                        break;
                    case PlayJob.Wizard:
                        citem.Item.SC = 0;
                        if (HUtil32.HiByte(citem.Item.MAC) > 12)
                        {
                            citem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.MAC), (ushort)(HUtil32.HiByte(citem.Item.MAC) - 12));
                        }
                        else
                        {
                            citem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.MAC), 0);
                        }
                        break;
                    case PlayJob.Taoist:
                        citem.Item.DC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(citem.Item.DC) + 2), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC) + 10));
                        citem.Item.MC = 0;
                        citem.Item.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(citem.Item.AC) - 2), HUtil32.HiByte(citem.Item.AC));
                        break;
                }
            }
            else if ((citem.Item.StdMode == 53))
            {
                if ((citem.Item.Shape == ShapeConst.LOLLIPOP_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            citem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC) + 2));
                            citem.Item.MC = 0;
                            citem.Item.SC = 0;
                            break;
                        case PlayJob.Wizard:
                            citem.Item.DC = 0;
                            citem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.MC) + 2));
                            citem.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
                            citem.Item.DC = 0;
                            citem.Item.MC = 0;
                            citem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.SC) + 2));
                            break;
                    }
                }
                else if ((citem.Item.Shape == ShapeConst.GOLDMEDAL_SHAPE) || (citem.Item.Shape == ShapeConst.SILVERMEDAL_SHAPE) || (citem.Item.Shape == ShapeConst.BRONZEMEDAL_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            citem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC)));
                            citem.Item.MC = 0;
                            citem.Item.SC = 0;
                            break;
                        case PlayJob.Wizard:
                            citem.Item.DC = 0;
                            citem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.MC)));
                            citem.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
                            citem.Item.DC = 0;
                            citem.Item.MC = 0;
                            citem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.SC)));
                            break;
                    }
                }
            }
        }

        private void ApplyItemParametersByJob(UserItem uitem, ref ClientItem std)
        {
            var item = M2Share.WorldEngine.GetStdItem(uitem.Index);
            if (item != null)
            {
                if ((item.StdMode == 22) && (item.Shape == DragonConst.DRAGON_RING_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            std.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(item.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.DC) + 4));
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Wizard:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
                            std.Item.MC = 0;
                            break;
                    }
                }
                else if ((item.StdMode == 26) && (item.Shape == DragonConst.DRAGON_BRACELET_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            std.Item.DC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(item.DC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.DC) + 2));
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            std.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(item.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.AC) + 1));
                            break;
                        case PlayJob.Wizard:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            std.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(item.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.AC) + 1));
                            break;
                        case PlayJob.Taoist:
                            std.Item.MC = 0;
                            break;
                    }
                }
                else if ((item.StdMode == 19) && (item.Shape == DragonConst.DRAGON_NECKLACE_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Wizard:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
                            std.Item.DC = 0;
                            std.Item.MC = 0;
                            break;
                    }
                }
                else if (((item.StdMode == 10) || (item.StdMode == 11)) && (item.Shape == DragonConst.DRAGON_DRESS_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Wizard:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
                            std.Item.DC = 0;
                            std.Item.MC = 0;
                            break;
                    }
                }
                else if ((item.StdMode == 15) && (item.Shape == DragonConst.DRAGON_HELMET_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Wizard:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
                            std.Item.DC = 0;
                            std.Item.MC = 0;
                            break;
                    }
                }
                else if (((item.StdMode == 5) || (item.StdMode == 6)) && (item.Shape == DragonConst.DRAGON_WEAPON_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            std.Item.DC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(item.DC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.DC) + 28));
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            std.Item.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(item.AC) - 2), HUtil32.HiByte(item.AC));
                            break;
                        case PlayJob.Wizard:
                            std.Item.SC = 0;
                            if (HUtil32.HiByte(item.MAC) > 12)
                            {
                                std.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(item.MAC), (ushort)(HUtil32.HiByte(item.MAC) - 12));
                            }
                            else
                            {
                                std.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(item.MAC), 0);
                            }
                            break;
                        case PlayJob.Taoist:
                            std.Item.DC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(item.DC) + 2), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.DC) + 10));
                            std.Item.MC = 0;
                            std.Item.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(item.AC) - 2), HUtil32.HiByte(item.AC));
                            break;
                    }
                }
                else if ((item.StdMode == 53))
                {
                    if ((item.Shape == ShapeConst.LOLLIPOP_SHAPE))
                    {
                        switch (Job)
                        {
                            case PlayJob.Warrior:
                                std.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(item.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.DC) + 2));
                                std.Item.MC = 0;
                                std.Item.SC = 0;
                                break;
                            case PlayJob.Wizard:
                                std.Item.DC = 0;
                                std.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(item.MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.MC) + 2));
                                std.Item.SC = 0;
                                break;
                            case PlayJob.Taoist:
                                std.Item.DC = 0;
                                std.Item.MC = 0;
                                std.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(item.SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.SC) + 2));
                                break;
                        }
                    }
                    else if ((item.Shape == ShapeConst.GOLDMEDAL_SHAPE) || (item.Shape == ShapeConst.SILVERMEDAL_SHAPE) || (item.Shape == ShapeConst.BRONZEMEDAL_SHAPE))
                    {
                        switch (Job)
                        {
                            case PlayJob.Warrior:
                                std.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(item.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.DC)));
                                std.Item.MC = 0;
                                std.Item.SC = 0;
                                break;
                            case PlayJob.Wizard:
                                std.Item.DC = 0;
                                std.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(item.MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.MC)));
                                std.Item.SC = 0;
                                break;
                            case PlayJob.Taoist:
                                std.Item.DC = 0;
                                std.Item.MC = 0;
                                std.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(item.SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.SC)));
                                break;
                        }
                    }
                }
                if (((item.StdMode == 10) || (item.StdMode == 11)) && (item.Shape == ItemShapeConst.DRESS_SHAPE_PBKING))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            std.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(item.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.DC) + 2));
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            std.Item.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(item.AC) + 2), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.AC) + 4));
                            std.Item.MpAdd = item.MpAdd + 30;
                            break;
                        case PlayJob.Wizard:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            std.Item.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(item.MAC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.MAC) + 2));
                            std.Item.HpAdd = item.HpAdd + 30;
                            break;
                        case PlayJob.Taoist:
                            std.Item.DC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(item.DC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.DC)));
                            std.Item.MC = 0;
                            std.Item.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(item.AC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.AC)));
                            std.Item.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(item.MAC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.MAC)));
                            std.Item.HpAdd = item.HpAdd + 20;
                            std.Item.MpAdd = item.MpAdd + 10;
                            break;
                    }
                }
            }
        }

        private void DeleteNameSkill(string sSkillName)
        {
            for (var i = 0; i < MagicList.Count; i++)
            {
                var userMagic = MagicList[i];
                if (userMagic.Magic.MagicName == sSkillName)
                {
                    SendDelMagic(userMagic);
                    MagicList.RemoveAt(i);
                    break;
                }
            }
        }

        private void DelItemSkill(int nIndex)
        {
            if (Race != ActorRace.Play)
            {
                return;
            }
            switch (nIndex)
            {
                case 1:
                    if (Job != PlayJob.Wizard)
                    {
                        DeleteNameSkill(M2Share.Config.FireBallSkill);
                    }
                    break;
                case 2:
                    if (Job != PlayJob.Taoist)
                    {
                        DeleteNameSkill(M2Share.Config.HealSkill);
                    }
                    break;
            }
        }

        private void DelMember(PlayObject baseObject)
        {
            if (GroupOwner != baseObject)
            {
                for (var i = 0; i < GroupMembers.Count; i++)
                {
                    if (GroupMembers[i] == baseObject)
                    {
                        baseObject.LeaveGroup();
                        GroupMembers.RemoveAt(i);
                        break;
                    }
                }
            }
            else
            {
                for (var i = GroupMembers.Count - 1; i >= 0; i--)
                {
                    GroupMembers[i].LeaveGroup();
                    GroupMembers.RemoveAt(i);
                }
            }
            if (Race == ActorRace.Play)
            {
                var playObject = this;
                if (!playObject.CancelGroup())
                {
                    playObject.SendDefMessage(Grobal2.SM_GROUPCANCEL, 0, 0, 0, 0, "");
                }
                else
                {
                    playObject.SendGroupMembers();
                }
            }
        }

        private bool IsGroupMember(BaseObject target)
        {
            var result = false;
            if (GroupOwner == null)
            {
                return false;
            }
            for (var i = 0; i < GroupOwner.GroupMembers.Count; i++)
            {
                if (GroupOwner.GroupMembers[i] == target)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private void LeaveGroup()
        {
            const string sExitGropMsg = "{0} 已经退出了本组.";
            SendGroupText(Format(sExitGropMsg, ChrName));
            GroupOwner = null;
            SendMsg(this, Grobal2.RM_GROUPCANCEL, 0, 0, 0, 0, "");
        }

        public void SendGroupText(string sMsg)
        {
            sMsg = M2Share.Config.GroupMsgPreFix + sMsg;
            if (GroupOwner != null)
            {
                for (var i = 0; i < GroupOwner.GroupMembers.Count; i++)
                {
                    GroupOwner.GroupMembers[i].SendMsg(this, Grobal2.RM_GROUPMESSAGE, 0, M2Share.Config.GroupMsgFColor,
                        M2Share.Config.GroupMsgBColor, 0, sMsg);
                }
            }
        }
    }
}