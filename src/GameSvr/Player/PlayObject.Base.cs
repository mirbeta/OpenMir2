using GameSvr.Actor;
using GameSvr.Event;
using GameSvr.GameCommand;
using GameSvr.Items;
using GameSvr.Magic;
using GameSvr.Maps;
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
        public ClientMesaagePacket m_DefMsg;
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
        public IList<ClientUserItem> DealItemList;
        /// <summary>
        /// 仓库物品列表
        /// </summary>
        internal readonly IList<ClientUserItem> StorageItemList;
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
        private int DecPkPointTick;
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
        public bool InFreePkArea;
        public string m_sOldSayMsg;
        public int m_nSayMsgCount = 0;
        public int m_dwSayMsgTick;
        public bool m_boDisableSayMsg;
        public int m_dwDisableSayMsgTick;
        public int m_dwCheckDupObjTick;
        public int DiscountForNightTick;
        public bool m_boInSafeArea;
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
        public byte m_btValNPCType;
        public byte m_btValType;
        public byte m_btValLabel;
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
        public string MSOffLineLeaveword = string.Empty;
        /// <summary>
        /// 登录帐号名
        /// </summary>
        public string UserID;
        /// <summary>
        /// 人物IP地址
        /// </summary>
        public string m_sIPaddr = string.Empty;
        public string m_sIPLocal = string.Empty;
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
        public int m_nSocket = 0;
        /// <summary>
        /// 人物连接到游戏网关SOCKETID
        /// </summary>
        public ushort m_nGSocketIdx = 0;
        /// <summary>
        /// 人物所在网关号
        /// </summary>
        public int m_nGateIdx = 0;
        public int m_nSoftVersionDate = 0;
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime m_dLogonTime;
        /// <summary>
        /// 战领沙城时间
        /// </summary>
        public int m_dwLogonTick;
        /// <summary>
        /// 是否进入游戏完成
        /// </summary>
        public bool m_boReadyRun;
        /// <summary>
        /// 当前会话ID
        /// </summary>
        public int m_nSessionID = 0;
        /// <summary>
        /// 人物当前付费模式
        /// 1:试玩
        /// 2:付费
        /// 3:测试
        /// </summary>
        public int PayMent;
        public int m_nPayMode = 0;
        /// <summary>
        /// 全局会话信息
        /// </summary>
        public TSessInfo m_SessInfo;
        public int m_dwLoadTick = 0;
        /// <summary>
        /// 人物当前所在服务器序号
        /// </summary>
        public int m_nServerIndex = 0;
        public bool m_boEmergencyClose;
        /// <summary>
        /// 掉线标志
        /// </summary>
        public bool m_boSoftClose;
        /// <summary>
        /// 断线标志(@kick 命令)
        /// </summary>
        public bool m_boKickFlag;
        /// <summary>
        /// 是否重连
        /// </summary>
        public bool m_boReconnection;
        public bool m_boRcdSaved;
        public bool m_boSwitchData;
        public bool m_boSwitchDataOK = false;
        public string m_sSwitchDataTempFile = string.Empty;
        public int m_nWriteChgDataErrCount;
        public string m_sSwitchMapName = string.Empty;
        public short m_nSwitchMapX = 0;
        public short m_nSwitchMapY = 0;
        public bool m_boSwitchDataSended;
        public int m_dwChgDataWritedTick = 0;
        /// <summary>
        /// 攻击间隔
        /// </summary>
        public int m_dwHitIntervalTime;
        /// <summary>
        /// 魔法间隔
        /// </summary>
        public int m_dwMagicHitIntervalTime;
        /// <summary>
        /// 走路间隔
        /// </summary>
        public int m_dwRunIntervalTime;
        /// <summary>
        /// 走路间隔
        /// </summary>
        public int m_dwWalkIntervalTime;
        /// <summary>
        /// 换方向间隔
        /// </summary>
        public int m_dwTurnIntervalTime;
        /// <summary>
        /// 组合操作间隔
        /// </summary>
        public int m_dwActionIntervalTime;
        /// <summary>
        /// 移动刺杀间隔
        /// </summary>
        public int m_dwRunLongHitIntervalTime;
        /// <summary>
        /// 跑位攻击间隔
        /// </summary>        
        public int m_dwRunHitIntervalTime;
        /// <summary>
        /// 走位攻击间隔
        /// </summary>        
        public int m_dwWalkHitIntervalTime;
        /// <summary>
        /// 跑位魔法间隔
        /// </summary>        
        public int m_dwRunMagicIntervalTime;
        /// <summary>
        /// 魔法攻击时间
        /// </summary>        
        public int m_dwMagicAttackTick;
        /// <summary>
        /// 魔法攻击间隔时间
        /// </summary>
        public int m_dwMagicAttackInterval;
        /// <summary>
        /// 攻击时间
        /// </summary>
        public int m_dwAttackTick;
        /// <summary>
        /// 人物跑动时间
        /// </summary>
        public int m_dwMoveTick;
        /// <summary>
        /// 人物攻击计数
        /// </summary>
        public int m_dwAttackCount;
        /// <summary>
        /// 人物攻击计数
        /// </summary>
        public int m_dwAttackCountA;
        /// <summary>
        /// 魔法攻击计数
        /// </summary>
        public int m_dwMagicAttackCount;
        /// <summary>
        /// 人物跑计数
        /// </summary>
        public int m_dwMoveCount;
        /// <summary>
        /// 超速计数
        /// </summary>
        public int m_nOverSpeedCount;
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
        private bool FlameRing = false;
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
        private bool MuscleRing = false;
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
        public double BodyLuck;
        public int BodyLuckLevel;
        public bool m_boDieInFight3Zone;
        public string m_sGotoNpcLabel = string.Empty;
        public bool TakeDlgItem = false;
        public int DlgItemIndex = 0;
        public int m_nDelayCall;
        public int m_dwDelayCallTick = 0;
        public bool m_boDelayCall;
        public int m_DelayCallNPC;
        public string m_sDelayCallLabel = string.Empty;
        public TScript m_Script;
        public BaseObject m_NPC = null;
        /// <summary>
        /// 玩家的变量P
        /// </summary>
        public int[] m_nVal;
        /// <summary>
        /// 玩家的变量M
        /// </summary>
        public int[] m_nMval;
        /// <summary>
        /// 玩家的变量D
        /// </summary>
        public int[] m_DyVal;
        /// <summary>
        /// 玩家的变量
        /// </summary>
        public string[] m_nSval;
        /// <summary>
        /// 人物变量  N
        /// </summary>
        public int[] m_nInteger;
        /// <summary>
        /// 人物变量  S
        /// </summary>
        public string[] m_sString;
        /// <summary>
        /// 服务器变量 W 0-20 个人服务器数值变量，不可保存，不可操作
        /// </summary>
        public string[] m_ServerStrVal;
        /// <summary>
        /// E 0-20 个人服务器字符串变量，不可保存，不可操作
        /// </summary>
        public int[] m_ServerIntVal;
        public string m_sPlayDiceLabel = string.Empty;
        public bool m_boTimeRecall;
        public int m_dwTimeRecallTick = 0;
        public string m_sMoveMap = string.Empty;
        public short m_nMoveX;
        public short m_nMoveY;
        /// <summary>
        /// 保存人物数据时间间隔
        /// </summary>
        public int m_dwSaveRcdTick;
        public byte m_btBright;
        public bool m_boNewHuman;
        public bool m_boSendNotice;
        public int m_dwWaitLoginNoticeOKTick;
        public bool m_boLoginNoticeOK;
        public bool bo6AB;
        public int m_dwShowLineNoticeTick;
        public int m_nShowLineNoticeIdx;
        public int m_nSoftVersionDateEx;
        /// <summary>
        /// 可点击脚本标签字典
        /// </summary>
        private readonly Hashtable m_CanJmpScriptLableList;
        public int m_nScriptGotoCount = 0;
        public string m_sScriptCurrLable = string.Empty;
        // 用于处理 @back 脚本命令
        public string m_sScriptGoBackLable = string.Empty;
        // 用于处理 @back 脚本命令
        public int m_dwTurnTick;
        public int m_wOldIdent = 0;
        public byte m_btOldDir = 0;
        /// <summary>
        /// 第一个操作
        /// </summary>
        public bool m_boFirstAction = false;
        /// <summary>
        /// 二次操作之间间隔时间
        /// </summary>        
        public int m_dwActionTick;
        /// <summary>
        /// 配偶名称
        /// </summary>
        public string m_sDearName;
        public PlayObject m_DearHuman;
        /// <summary>
        /// 是否允许夫妻传送
        /// </summary>
        public bool m_boCanDearRecall;
        public bool m_boCanMasterRecall;
        /// <summary>
        /// 夫妻传送时间
        /// </summary>
        public int m_dwDearRecallTick;
        public int m_dwMasterRecallTick;
        /// <summary>
        /// 师徒名称
        /// </summary>
        public string m_sMasterName;
        public PlayObject m_MasterHuman;
        public IList<PlayObject> m_MasterList;
        public bool m_boMaster = false;
        /// <summary>
        /// 声望点
        /// </summary>
        public byte m_btCreditPoint = 0;
        /// <summary>
        /// 离婚次数
        /// </summary>        
        public byte m_btMarryCount = 0;
        /// <summary>
        /// 转生等级
        /// </summary>
        public byte m_btReLevel = 0;
        public byte m_btReColorIdx;
        public int m_dwReColorTick = 0;
        /// <summary>
        /// 杀怪经验倍数
        /// </summary>
        public int m_nKillMonExpMultiple;
        /// <summary>
        /// 处理消息循环时间控制
        /// </summary>        
        public int m_dwGetMsgTick = 0;
        public bool m_boSetStoragePwd;
        public bool m_boReConfigPwd;
        public bool m_boCheckOldPwd;
        public bool m_boUnLockPwd;
        public bool m_boUnLockStoragePwd;
        public bool m_boPasswordLocked;
        // 锁密码
        public byte m_btPwdFailCount;
        /// <summary>
        /// 是否启用锁登录功能
        /// </summary>
        public bool m_boLockLogon;
        /// <summary>
        /// 是否打开登录锁
        /// </summary>        
        public bool m_boLockLogoned;
        public string m_sTempPwd;
        public string m_sStoragePwd;
        public BaseObject m_PoseBaseObject = null;
        public bool m_boStartMarry = false;
        public bool m_boStartMaster = false;
        public bool m_boStartUnMarry = false;
        public bool m_boStartUnMaster = false;
        /// <summary>
        /// 禁止发方字(发的文字只能自己看到)
        /// </summary>
        public bool m_boFilterSendMsg;
        /// <summary>
        /// 杀怪经验倍数(此数除以 100 为真正倍数)
        /// </summary>        
        public int m_nKillMonExpRate;
        /// <summary>
        /// 人物攻击力倍数(此数除以 100 为真正倍数)
        /// </summary>        
        public int m_nPowerRate;
        public int m_dwKillMonExpRateTime = 0;
        public int m_dwPowerRateTime = 0;
        public int m_dwRateTick;
        /// <summary>
        /// 技巧项链
        /// </summary>
        private bool FastTrain = false;
        /// <summary>
        /// 是否允许使用物品
        /// </summary>
        public bool m_boCanUseItem;
        public bool m_boCanDeal;
        public bool m_boCanDrop;
        public bool m_boCanGetBackItem;
        public bool m_boCanWalk;
        public bool m_boCanRun;
        public bool m_boCanHit;
        public bool m_boCanSpell;
        public bool m_boCanSendMsg;
        public int m_nMemberType;
        // 会员类型
        public int m_nMemberLevel;
        // 会员等级
        public bool m_boSendMsgFlag;
        // 发祝福语标志
        public bool m_boChangeItemNameFlag;
        /// <summary>
        /// 游戏币
        /// </summary>
        public int m_nGameGold;
        /// <summary>
        /// 是否自动减游戏币
        /// </summary>        
        public bool m_boDecGameGold;
        public int m_dwDecGameGoldTime;
        public int m_dwDecGameGoldTick;
        public int m_nDecGameGold;
        // 一次减点数
        public bool m_boIncGameGold;
        // 是否自动加游戏币
        public int m_dwIncGameGoldTime;
        public int m_dwIncGameGoldTick;
        public int m_nIncGameGold;
        // 一次减点数
        public int m_nGamePoint;
        // 游戏点数
        public int m_dwIncGamePointTick;
        public int m_nPayMentPoint;
        public int m_dwPayMentPointTick = 0;
        public int m_dwDecHPTick = 0;
        public int m_dwIncHPTick = 0;
        public PlayObject m_GetWhisperHuman;
        public int m_dwClearObjTick = 0;
        public short m_wContribution;
        // 贡献度
        public string m_sRankLevelName = string.Empty;
        // 显示名称格式串
        public bool m_boFilterAction = false;
        public int m_dwAutoGetExpTick;
        public int m_nAutoGetExpTime = 0;
        public int m_nAutoGetExpPoint;
        public Envirnoment m_AutoGetExpEnvir;
        public bool m_boAutoGetExpInSafeZone = false;
        public Dictionary<string, TDynamicVar> m_DynamicVarList;
        public short m_dwClientTick;
        /// <summary>
        /// 进入速度测试模式
        /// </summary>
        public bool m_boTestSpeedMode;
        public int m_dwDelayTime = 0;
        public string m_sRandomNo = string.Empty;
        public int m_dwQueryBagItemsTick = 0;
        public bool m_boTimeGoto;
        public int m_dwTimeGotoTick;
        public string m_sTimeGotoLable;
        public BaseObject m_TimeGotoNPC;
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
        public int m_dwClickNpcTime = 0;
        /// <summary>
        /// 是否开通元宝交易服务
        /// </summary>
        public bool bo_YBDEAL;
        /// <summary>
        /// 确认元宝寄售标志
        /// </summary>
        public bool m_boSellOffOK = false;
        /// <summary>
        /// 元宝寄售物品列表
        /// </summary>
        private IList<ClientUserItem> m_SellOffItemList;
        public byte[] QuestUnitOpen;
        public byte[] QuestUnit;
        public byte[] QuestFlag;

        public PlayObject() : base()
        {
            Race = ActorRace.Play;
            HomeMap = "0";
            DealGolds = 0;
            DealItemList = new List<ClientUserItem>();
            StorageItemList = new List<ClientUserItem>();
            LockWhisperList = new List<string>();
            m_boEmergencyClose = false;
            m_boSwitchData = false;
            m_boReconnection = false;
            m_boKickFlag = false;
            m_boSoftClose = false;
            m_boReadyRun = false;
            m_dwSaveRcdTick = HUtil32.GetTickCount();
            DecHungerPointTick = HUtil32.GetTickCount();
            GroupRcallTick = HUtil32.GetTickCount();
            WantRefMsg = true;
            m_boRcdSaved = false;
            m_boDieInFight3Zone = false;
            m_sGotoNpcLabel = "";
            m_nDelayCall = 0;
            m_sDelayCallLabel = "";
            m_boDelayCall = false;
            m_DelayCallNPC = 0;
            m_Script = null;
            m_boTimeRecall = false;
            m_sMoveMap = "";
            m_nMoveX = 0;
            m_nMoveY = 0;
            RunTick = HUtil32.GetTickCount();
            RunTime = 250;
            SearchTime = 1000;
            SearchTick = HUtil32.GetTickCount();
            AllowGroup = false;
            AllowGuild = false;
            ViewRange = 12;
            InFreePkArea = false;
            m_boNewHuman = false;
            m_boLoginNoticeOK = false;
            bo6AB = false;
            AccountExpired = false;
            m_boSendNotice = false;
            m_dwCheckDupObjTick = HUtil32.GetTickCount();
            DiscountForNightTick = HUtil32.GetTickCount();
            m_boInSafeArea = false;
            m_dwMagicAttackTick = HUtil32.GetTickCount();
            m_dwMagicAttackInterval = 0;
            m_dwAttackTick = HUtil32.GetTickCount();
            m_dwMoveTick = HUtil32.GetTickCount();
            m_dwTurnTick = HUtil32.GetTickCount();
            m_dwActionTick = HUtil32.GetTickCount();
            m_dwAttackCount = 0;
            m_dwAttackCountA = 0;
            m_dwMagicAttackCount = 0;
            m_dwMoveCount = 0;
            m_nOverSpeedCount = 0;
            m_sOldSayMsg = "";
            m_dwSayMsgTick = HUtil32.GetTickCount();
            m_boDisableSayMsg = false;
            m_dwDisableSayMsgTick = HUtil32.GetTickCount();
            m_dLogonTime = DateTime.Now;
            m_dwLogonTick = HUtil32.GetTickCount();
            m_boSwitchData = false;
            m_boSwitchDataSended = false;
            m_nWriteChgDataErrCount = 0;
            m_dwShowLineNoticeTick = HUtil32.GetTickCount();
            m_nShowLineNoticeIdx = 0;
            m_nSoftVersionDateEx = 0;
            m_CanJmpScriptLableList = new Hashtable(StringComparer.OrdinalIgnoreCase);
            m_nKillMonExpMultiple = 1;
            m_nKillMonExpRate = 100;
            m_dwRateTick = HUtil32.GetTickCount();
            m_nPowerRate = 100;
            m_boSetStoragePwd = false;
            m_boReConfigPwd = false;
            m_boCheckOldPwd = false;
            m_boUnLockPwd = false;
            m_boUnLockStoragePwd = false;
            m_boPasswordLocked = false;
            PvpFlag = false;
            // 锁仓库
            m_btPwdFailCount = 0;
            m_sTempPwd = "";
            m_sStoragePwd = "";
            m_boFilterSendMsg = false;
            m_boCanDeal = true;
            m_boCanDrop = true;
            m_boCanGetBackItem = true;
            m_boCanWalk = true;
            m_boCanRun = true;
            m_boCanHit = true;
            m_boCanSpell = true;
            m_boCanUseItem = true;
            m_nMemberType = 0;
            m_nMemberLevel = 0;
            m_nGameGold = 0;
            m_boDecGameGold = false;
            m_nDecGameGold = 1;
            m_dwDecGameGoldTick = HUtil32.GetTickCount();
            m_dwDecGameGoldTime = 60 * 1000;
            m_boIncGameGold = false;
            m_nIncGameGold = 1;
            m_dwIncGameGoldTick = HUtil32.GetTickCount();
            m_dwIncGameGoldTime = 60 * 1000;
            m_nGamePoint = 0;
            m_dwIncGamePointTick = HUtil32.GetTickCount();
            m_nPayMentPoint = 0;
            m_DearHuman = null;
            m_MasterHuman = null;
            m_MasterList = new List<PlayObject>();
            m_boSendMsgFlag = false;
            m_boChangeItemNameFlag = false;
            m_boCanMasterRecall = false;
            m_boCanDearRecall = false;
            m_dwDearRecallTick = HUtil32.GetTickCount();
            m_dwMasterRecallTick = HUtil32.GetTickCount();
            m_btReColorIdx = 0;
            m_GetWhisperHuman = null;
            OnHorse = false;
            m_wContribution = 0;
            m_sRankLevelName = M2Share.g_sRankLevelName;
            FixedHideMode = true;
            m_nVal = new int[100];
            m_nMval = new int[100];
            m_DyVal = new int[100];
            m_nSval = new string[100];
            m_nInteger = new int[100];
            m_sString = new string[100];
            m_ServerStrVal = new string[100];
            m_ServerIntVal = new int[100];
            ExtraAbil = new ushort[7];
            ExtraAbilTimes = new int[7];
            ExtraAbilFlag = new byte[7];
            HearWhisper = true;
            BanShout = true;
            BanGuildChat = true;
            AllowDeal = true;
            m_dwAutoGetExpTick = HUtil32.GetTickCount();
            DecPkPointTick = HUtil32.GetTickCount();
            m_nAutoGetExpPoint = 0;
            m_AutoGetExpEnvir = null;
            m_dwHitIntervalTime = M2Share.Config.HitIntervalTime;// 攻击间隔
            m_dwMagicHitIntervalTime = M2Share.Config.MagicHitIntervalTime;// 魔法间隔
            m_dwRunIntervalTime = M2Share.Config.RunIntervalTime;// 走路间隔
            m_dwWalkIntervalTime = M2Share.Config.WalkIntervalTime;// 走路间隔
            m_dwTurnIntervalTime = M2Share.Config.TurnIntervalTime;// 换方向间隔
            m_dwActionIntervalTime = M2Share.Config.ActionIntervalTime;// 组合操作间隔
            m_dwRunLongHitIntervalTime = M2Share.Config.RunLongHitIntervalTime;// 组合操作间隔
            m_dwRunHitIntervalTime = M2Share.Config.RunHitIntervalTime;// 组合操作间隔
            m_dwWalkHitIntervalTime = M2Share.Config.WalkHitIntervalTime;// 组合操作间隔
            m_dwRunMagicIntervalTime = M2Share.Config.RunMagicIntervalTime;// 跑位魔法间隔
            m_DynamicVarList = new Dictionary<string, TDynamicVar>(StringComparer.OrdinalIgnoreCase);
            m_SessInfo = null;
            m_boTestSpeedMode = false;
            m_boLockLogon = true;
            m_boLockLogoned = false;
            m_boTimeGoto = false;
            m_dwTimeGotoTick = HUtil32.GetTickCount();
            m_sTimeGotoLable = "";
            m_TimeGotoNPC = null;
            AutoTimerTick = new int[20];
            AutoTimerStatus = new int[20];
            MapCell = CellType.Play;
            QueryExpireTick = 60 * 1000;
            AccountExpiredTick = HUtil32.GetTickCount();
            GoldMax = M2Share.Config.HumanMaxGold;
            QuestUnitOpen = new byte[128];
            QuestUnit = new byte[128];
            QuestFlag = new byte[128];
            m_sRandomNo = M2Share.RandomNumber.Random(999999).ToString();
        }

        private void SendNotice()
        {
            var LoadList = new List<string>();
            M2Share.NoticeMgr.GetNoticeMsg("Notice", LoadList);
            var sNoticeMsg = string.Empty;
            if (LoadList.Count > 0)
            {
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sNoticeMsg = sNoticeMsg + LoadList[i] + "\x20\x1B";
                }
            }

            SendDefMessage(Grobal2.SM_SENDNOTICE, 2000, 0, 0, 0, sNoticeMsg.Replace("/r/n/r/n ", ""));
        }

        public void RunNotice()
        {
            ProcessMessage Msg;
            const string sExceptionMsg = "[Exception] TPlayObject::RunNotice";
            if (m_boEmergencyClose || m_boKickFlag || m_boSoftClose)
            {
                if (m_boKickFlag)
                {
                    SendDefMessage(Grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0, "");
                }
                MakeGhost();
            }
            else
            {
                try
                {
                    if (!m_boSendNotice)
                    {
                        SendNotice();
                        m_boSendNotice = true;
                        m_dwWaitLoginNoticeOKTick = HUtil32.GetTickCount();
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - m_dwWaitLoginNoticeOKTick) > 10 * 1000)
                        {
                            m_boEmergencyClose = true;
                        }
                        while (GetMessage(out Msg))
                        {
                            if (Msg.wIdent == Grobal2.CM_LOGINNOTICEOK)
                            {
                                m_boLoginNoticeOK = true;
                                m_dwClientTick = (short)Msg.nParam1;
                                SysMsg(m_dwClientTick.ToString(), MsgColor.Red, MsgType.Notice);
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
            var MessageBodyWL = new MessageBodyWL();
            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_LOGON, ActorId, CurrX, CurrY, HUtil32.MakeWord(Direction, Light));
            MessageBodyWL.Param1 = GetFeatureToLong();
            MessageBodyWL.Param2 = CharStatus;
            if (AllowGroup)
            {
                MessageBodyWL.Tag1 = HUtil32.MakeLong(HUtil32.MakeWord(1, 0), GetFeatureEx());
            }
            else
            {
                MessageBodyWL.Tag1 = 0;
            }
            MessageBodyWL.Tag2 = 0;
            SendSocket(m_DefMsg, EDCode.EncodeBuffer(MessageBodyWL));
            var nRecog = GetFeatureToLong();
            SendDefMessage(Grobal2.SM_FEATURECHANGED, ActorId, HUtil32.LoWord(nRecog), HUtil32.HiWord(nRecog), GetFeatureEx(), "");
            SendDefMessage(Grobal2.SM_ATTACKMODE, (byte)AttatckMode, 0, 0, 0, "");
        }

        /// <summary>
        /// 玩家登录
        /// </summary>
        public void UserLogon()
        {
            ClientUserItem UserItem;
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
                m_dLogonTime = DateTime.Now;
                m_dwLogonTick = HUtil32.GetTickCount();
                Initialize();
                SendMsg(this, Grobal2.RM_LOGON, 0, 0, 0, 0, "");
                if (Abil.Level <= 7)
                {
                    if (GetRangeHumanCount() >= 80)
                    {
                        MapRandomMove(Envir.MapName, 0);
                    }
                }
                if (m_boDieInFight3Zone)
                {
                    MapRandomMove(Envir.MapName, 0);
                }
                if (M2Share.WorldEngine.GetHumPermission(ChrName, ref sIPaddr, ref Permission))
                {
                    if (M2Share.Config.PermissionSystem)
                    {
                        if (!M2Share.CompareIPaddr(m_sIPaddr, sIPaddr))
                        {
                            SysMsg(sCheckIPaddrFail, MsgColor.Red, MsgType.Hint);
                            m_boEmergencyClose = true;
                        }
                    }
                }
                GetStartPoint();
                for (var i = MagicList.Count - 1; i >= 0; i--)
                {
                    CheckSeeHealGauge(MagicList[i]);
                }
                if (m_boNewHuman)
                {
                    UserItem = new ClientUserItem();
                    if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.Candle, ref UserItem))
                    {
                        ItemList.Add(UserItem);
                    }
                    else
                    {
                        Dispose(UserItem);
                    }
                    UserItem = new ClientUserItem();
                    if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.BasicDrug, ref UserItem))
                    {
                        ItemList.Add(UserItem);
                    }
                    else
                    {
                        Dispose(UserItem);
                    }
                    UserItem = new ClientUserItem();
                    if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.WoodenSword, ref UserItem))
                    {
                        ItemList.Add(UserItem);
                    }
                    else
                    {
                        Dispose(UserItem);
                    }
                    UserItem = new ClientUserItem();
                    var sItem = Gender == PlayGender.Man
                        ? M2Share.Config.ClothsMan
                        : M2Share.Config.ClothsWoman;
                    if (M2Share.WorldEngine.CopyToUserItemFromName(sItem, ref UserItem))
                    {
                        ItemList.Add(UserItem);
                    }
                    else
                    {
                        Dispose(UserItem);
                    }
                }
                // 检查背包中的物品是否合法
                for (var i = ItemList.Count - 1; i >= 0; i--)
                {
                    UserItem = ItemList[i];
                    if (!string.IsNullOrEmpty(M2Share.WorldEngine.GetStdItemName(UserItem.Index))) continue;
                    Dispose(ItemList[i]);
                    ItemList.RemoveAt(i);
                }
                // 检查人物身上的物品是否符合使用规则
                if (M2Share.Config.CheckUserItemPlace)
                {
                    for (var i = 0; i < UseItems.Length; i++)
                    {
                        if (UseItems[i] == null || UseItems[i].Index <= 0) continue;
                        var StdItem = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                        if (StdItem != null)
                        {
                            if (!M2Share.CheckUserItems(i, StdItem))
                            {
                                UserItem = UseItems[i];
                                if (!AddItemToBag(UserItem))
                                {
                                    ItemList.Insert(0, UserItem);
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
                    UserItem = ItemList[i];
                    var sItemName = M2Share.WorldEngine.GetStdItemName(UserItem.Index);
                    for (var j = i - 1; j >= 0; j--)
                    {
                        var UserItem1 = ItemList[j];
                        if (M2Share.WorldEngine.GetStdItemName(UserItem1.Index) == sItemName && UserItem.MakeIndex == UserItem1.MakeIndex)
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
                if (!bo6AB)
                {
                    if (m_nSoftVersionDate < M2Share.Config.SoftVersionDate) //登录版本号验证
                    {
                        SysMsg(M2Share.sClientSoftVersionError, MsgColor.Red, MsgType.Hint);
                        SysMsg(M2Share.sDownLoadNewClientSoft, MsgColor.Red, MsgType.Hint);
                        SysMsg(M2Share.sForceDisConnect, MsgColor.Red, MsgType.Hint);
                        m_boEmergencyClose = true;
                        return;
                    }
                    if (m_nSoftVersionDateEx == 0 && M2Share.Config.boOldClientShowHiLevel)
                    {
                        SysMsg(M2Share.sClientSoftVersionTooOld, MsgColor.Blue, MsgType.Hint);
                        SysMsg(M2Share.sDownLoadAndUseNewClient, MsgColor.Red, MsgType.Hint);
                        if (!M2Share.Config.CanOldClientLogon)
                        {
                            SysMsg(M2Share.sClientSoftVersionError, MsgColor.Red, MsgType.Hint);
                            SysMsg(M2Share.sDownLoadNewClientSoft, MsgColor.Red, MsgType.Hint);
                            SysMsg(M2Share.sForceDisConnect, MsgColor.Red, MsgType.Hint);
                            m_boEmergencyClose = true;
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
                            m_boEmergencyClose = true;
                        }
                    }
                }
                m_btBright = (byte)M2Share.g_nGameTime;
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
                    if (!bo6AB)
                    {
                        SysMsg(M2Share.sYouNowIsTryPlayMode, MsgColor.Red, MsgType.Hint);
                    }
                    GoldMax = M2Share.Config.HumanTryModeMaxGold;
                    if (Abil.Level > M2Share.Config.TryModeLevel)
                    {
                        SysMsg("测试状态可以使用到第 " + M2Share.Config.TryModeLevel, MsgColor.Red, MsgType.Hint);
                        SysMsg("链接中断，请到以下地址获得收费相关信息。(http://www.mir2.com)", MsgColor.Red, MsgType.Hint);
                        m_boEmergencyClose = true;
                    }
                }
                if (PayMent == 3 && !bo6AB)
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
                if (!string.IsNullOrEmpty(m_sDearName))
                {
                    CheckMarry();
                }
                CheckMaster();
                m_boFilterSendMsg = M2Share.GetDisableSendMsgList(ChrName);
                // 密码保护系统
                if (M2Share.Config.PasswordLockSystem)
                {
                    if (m_boPasswordLocked)
                    {
                        m_boCanGetBackItem = !M2Share.Config.LockGetBackItemAction;
                    }
                    if (M2Share.Config.LockHumanLogin && m_boLockLogon && m_boPasswordLocked)
                    {
                        m_boCanDeal = !M2Share.Config.LockDealAction;
                        m_boCanDrop = !M2Share.Config.LockDropAction;
                        m_boCanUseItem = !M2Share.Config.LockUserItemAction;
                        m_boCanWalk = !M2Share.Config.LockWalkAction;
                        m_boCanRun = !M2Share.Config.LockRunAction;
                        m_boCanHit = !M2Share.Config.LockHitAction;
                        m_boCanSpell = !M2Share.Config.LockSpellAction;
                        m_boCanSendMsg = !M2Share.Config.LockSendMsgAction;
                        ObMode = M2Share.Config.LockInObModeAction;
                        AdminMode = M2Share.Config.LockInObModeAction;
                        SysMsg(M2Share.g_sActionIsLockedMsg + " 开锁命令: @" + CommandMgr.Commands.LockLogon.CmdName, MsgColor.Red, MsgType.Hint);
                        SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, M2Share.g_sActionIsLockedMsg + "\\ \\" + "密码命令: @" + CommandMgr.Commands.PasswordLock.CmdName);
                    }
                    if (!m_boPasswordLocked)
                    {
                        SysMsg(Format(M2Share.g_sPasswordNotSetMsg, CommandMgr.Commands.PasswordLock.CmdName), MsgColor.Red, MsgType.Hint);
                    }
                    if (!m_boLockLogon && m_boPasswordLocked)
                    {
                        SysMsg(Format(M2Share.g_sNotPasswordProtectMode, CommandMgr.Commands.LockLogon.CmdName), MsgColor.Red, MsgType.Hint);
                    }
                    SysMsg(M2Share.g_sActionIsLockedMsg + " 开锁命令: @" + CommandMgr.Commands.Unlock.CmdName, MsgColor.Red, MsgType.Hint);
                    SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, M2Share.g_sActionIsLockedMsg + "\\ \\" + "开锁命令: @" + CommandMgr.Commands.Unlock.CmdName + '\\' + "加锁命令: @" + CommandMgr.Commands.Lock.CmdName + '\\' + "设置密码命令: @" + CommandMgr.Commands.SetPassword.CmdName + '\\' + "修改密码命令: @" + CommandMgr.Commands.ChgPassword.CmdName);
                }
                // 重置泡点方面计时
                m_dwIncGamePointTick = HUtil32.GetTickCount();
                m_dwIncGameGoldTick = HUtil32.GetTickCount();
                m_dwAutoGetExpTick = HUtil32.GetTickCount();
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
            if (UseItems[Grobal2.U_WEAPON] == null && UseItems[Grobal2.U_WEAPON].Index <= 0)
            {
                return false;
            }
            var nRand = 0;
            var StdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_WEAPON].Index);
            if (StdItem != null)
            {
                nRand = Math.Abs(HUtil32.HiByte(StdItem.DC) - HUtil32.LoByte(StdItem.DC)) / 5;
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
            var UserItem = UseItems[Grobal2.U_WEAPON];
            if (UserItem.Index <= 0 || UserItem.DuraMax <= UserItem.Dura)
            {
                return false;
            }
            UserItem.DuraMax -= (ushort)((UserItem.DuraMax - UserItem.Dura) / M2Share.Config.RepairItemDecDura);
            var nDura = HUtil32._MIN(5000, UserItem.DuraMax - UserItem.Dura);
            if (nDura <= 0) return false;
            UserItem.Dura += (ushort)nDura;
            SendMsg(this, Grobal2.RM_DURACHANGE, 1, UserItem.Dura, UserItem.DuraMax, 0, "");
            SysMsg(M2Share.g_sWeaponRepairSuccess, MsgColor.Green, MsgType.Hint);
            return true;
        }

        /// <summary>
        /// 特修武器
        /// </summary>
        /// <returns></returns>
        private bool SuperRepairWeapon()
        {
            if (UseItems[Grobal2.U_WEAPON] == null && UseItems[Grobal2.U_WEAPON].Index <= 0)
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
                                                FlameRing = true;
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
                                                MuscleRing = true;
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

                if (FlameRing)
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
                if (MuscleRing)
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

        protected override void UpdateVisibleGay(BaseObject BaseObject)
        {
            var boIsVisible = false;
            VisibleBaseObject VisibleBaseObject;
            if (BaseObject.Race == ActorRace.Play || BaseObject.Master != null)
            {
                IsVisibleActive = true;// 如果是人物或宝宝则置TRUE
            }
            for (var i = 0; i < VisibleActors.Count; i++)
            {
                VisibleBaseObject = VisibleActors[i];
                if (VisibleBaseObject.BaseObject == BaseObject)
                {
                    VisibleBaseObject.VisibleFlag = VisibleFlag.Invisible;
                    boIsVisible = true;
                    break;
                }
            }
            if (boIsVisible)
            {
                return;
            }
            VisibleBaseObject = new VisibleBaseObject
            {
                VisibleFlag = VisibleFlag.Hidden,
                BaseObject = BaseObject
            };
            VisibleActors.Add(VisibleBaseObject);
            if (BaseObject.Race == ActorRace.Play)
            {
                SendWhisperMsg(BaseObject as PlayObject);
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
                                        BaseObject BaseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                                        if (BaseObject != null && !BaseObject.Invisible)
                                        {
                                            if (!BaseObject.Ghost && !BaseObject.FixedHideMode && !BaseObject.ObMode)
                                            {
                                                if (Race < ActorRace.Animal || Master != null || CrazyMode || NastyMode || WantRefMsg || BaseObject.Master != null && Math.Abs(BaseObject.CurrX - CurrX) <= 3 && Math.Abs(BaseObject.CurrY - CurrY) <= 3 || BaseObject.Race == ActorRace.Play)
                                                {
                                                    UpdateVisibleGay(BaseObject);
                                                    if (BaseObject.MapCell == CellType.Monster && this.MapCell == CellType.Play && !this.ObMode && !BaseObject.FixedHideMode)
                                                    {
                                                        //我的视野 进入对方的攻击范围
                                                        if (Math.Abs(BaseObject.CurrX - CurrX) <= (ViewRange - BaseObject.ViewRange) && Math.Abs(BaseObject.CurrY - CurrY) <= (ViewRange - BaseObject.ViewRange))
                                                        {
                                                            BaseObject.UpdateMonsterVisible(this);
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
                                            EventInfo MapEvent = (EventInfo)M2Share.CellObjectSystem.Get(cellObject.CellObjId);
                                            if (MapEvent.Visible)
                                            {
                                                UpdateVisibleEvent(nX, nY, MapEvent);
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
                    var VisibleBaseObject = VisibleActors[n18];
                    if (VisibleBaseObject.VisibleFlag == 0)
                    {
                        if (Race == ActorRace.Play)
                        {
                            var BaseObject = VisibleBaseObject.BaseObject;
                            if (!BaseObject.FixedHideMode && !BaseObject.Ghost)//防止人物退出时发送重复的消息占用带宽，人物进入隐身模式时人物不消失问题
                            {
                                SendMsg(BaseObject, Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
                            }
                        }
                        VisibleActors.RemoveAt(n18);
                        Dispose(VisibleBaseObject);
                        continue;
                    }
                    if (Race == ActorRace.Play && VisibleBaseObject.VisibleFlag == VisibleFlag.Hidden)
                    {
                        var BaseObject = VisibleBaseObject.BaseObject;
                        if (BaseObject != this)
                        {
                            if (BaseObject.Death)
                            {
                                if (BaseObject.Skeleton)
                                {
                                    SendMsg(BaseObject, Grobal2.RM_SKELETON, BaseObject.Direction, BaseObject.CurrX, BaseObject.CurrY, 0, "");
                                }
                                else
                                {
                                    SendMsg(BaseObject, Grobal2.RM_DEATH, BaseObject.Direction, BaseObject.CurrX, BaseObject.CurrY, 0, "");
                                }
                            }
                            else
                            {
                                SendMsg(BaseObject, Grobal2.RM_TURN, BaseObject.Direction, BaseObject.CurrX, BaseObject.CurrY, 0, BaseObject.GetShowName());
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
                    var VisibleMapItem = VisibleItems[I];
                    if (VisibleMapItem.VisibleFlag == 0)
                    {
                        SendMsg(this, Grobal2.RM_ITEMHIDE, 0, VisibleMapItem.MapItem.ActorId, VisibleMapItem.nX, VisibleMapItem.nY, "");
                        VisibleItems.RemoveAt(I);
                        Dispose(VisibleMapItem);
                        continue;
                    }
                    if (VisibleMapItem.VisibleFlag == VisibleFlag.Hidden)
                    {
                        SendMsg(this, Grobal2.RM_ITEMSHOW, VisibleMapItem.wLooks, VisibleMapItem.MapItem.ActorId, VisibleMapItem.nX, VisibleMapItem.nY, VisibleMapItem.sName);
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
                    EventInfo MapEvent = VisibleEvents[I];
                    if (MapEvent.VisibleFlag == VisibleFlag.Visible)
                    {
                        SendMsg(this, Grobal2.RM_HIDEEVENT, 0, MapEvent.Id, MapEvent.nX, MapEvent.nY, "");
                        VisibleEvents.RemoveAt(I);
                        continue;
                    }
                    if (MapEvent.VisibleFlag == VisibleFlag.Hidden)
                    {
                        SendMsg(this, Grobal2.RM_SHOWEVENT, (short)MapEvent.EventType, MapEvent.Id, HUtil32.MakeLong(MapEvent.nX, (short)MapEvent.EventParam), MapEvent.nY, "");
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
            string sShowName;
            var sChrName = string.Empty;
            var sGuildName = string.Empty;
            var sDearName = string.Empty;
            var sMasterName = string.Empty;
            const string sExceptionMsg = "[Exception] TPlayObject::GetShowName";
            try
            {
                if (MyGuild != null)
                {
                    var Castle = M2Share.CastleMgr.IsCastleMember(this);
                    if (Castle != null)
                    {
                        sGuildName = M2Share.g_sCastleGuildName.Replace("%castlename", Castle.sName);
                        sGuildName = sGuildName.Replace("%guildname", MyGuild.sGuildName);
                        sGuildName = sGuildName.Replace("%rankname", GuildRankName);
                    }
                    else
                    {
                        Castle = M2Share.CastleMgr.InCastleWarArea(this);// 01/25 多城堡
                        if (M2Share.Config.ShowGuildName || Castle != null && Castle.UnderWar || InFreePkArea)
                        {
                            sGuildName = M2Share.g_sNoCastleGuildName.Replace("%guildname", MyGuild.sGuildName);
                            sGuildName = sGuildName.Replace("%rankname", GuildRankName);
                        }
                    }
                }
                if (!M2Share.Config.ShowRankLevelName)
                {
                    if (m_btReLevel > 0)
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
                    sChrName = Format(m_sRankLevelName, ChrName);
                }
                if (!string.IsNullOrEmpty(m_sMasterName))
                {
                    if (m_boMaster)
                    {
                        sMasterName = Format(M2Share.g_sMasterName, m_sMasterName);
                    }
                    else
                    {
                        sMasterName = Format(M2Share.g_sNoMasterName, m_sMasterName);
                    }
                }
                if (!string.IsNullOrEmpty(m_sDearName))
                {
                    if (Gender == PlayGender.Man)
                    {
                        sDearName = Format(M2Share.g_sManDearName, m_sDearName);
                    }
                    else
                    {
                        sDearName = Format(M2Share.g_sWoManDearName, m_sDearName);
                    }
                }
                sShowName = M2Share.g_sHumanShowName.Replace("%chrname", sChrName);
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
            string sSayMsg;
            PlayObject Human;
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
                if (m_DearHuman != null)
                {
                    if (Gender == PlayGender.Man)
                    {
                        sSayMsg = M2Share.g_sManLongOutDearOnlineMsg.Replace("%d", m_sDearName);
                        sSayMsg = sSayMsg.Replace("%s", ChrName);
                        sSayMsg = sSayMsg.Replace("%m", Envir.MapDesc);
                        sSayMsg = sSayMsg.Replace("%x", CurrX.ToString());
                        sSayMsg = sSayMsg.Replace("%y", CurrY.ToString());
                        m_DearHuman.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                    }
                    else
                    {
                        sSayMsg = M2Share.g_sWoManLongOutDearOnlineMsg.Replace("%d", m_sDearName);
                        sSayMsg = sSayMsg.Replace("%s", ChrName);
                        sSayMsg = sSayMsg.Replace("%m", Envir.MapDesc);
                        sSayMsg = sSayMsg.Replace("%x", CurrX.ToString());
                        sSayMsg = sSayMsg.Replace("%y", CurrY.ToString());
                        m_DearHuman.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                    }
                    m_DearHuman.m_DearHuman = null;
                    m_DearHuman = null;
                }
                if (m_MasterHuman != null || m_MasterList.Count > 0)
                {
                    if (m_boMaster)
                    {
                        for (var i = m_MasterList.Count - 1; i >= 0; i--)
                        {
                            Human = m_MasterList[i];
                            sSayMsg = M2Share.g_sMasterLongOutMasterListOnlineMsg.Replace("%s", ChrName);
                            sSayMsg = sSayMsg.Replace("%m", Envir.MapDesc);
                            sSayMsg = sSayMsg.Replace("%x", CurrX.ToString());
                            sSayMsg = sSayMsg.Replace("%y", CurrY.ToString());
                            Human.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                            Human.m_MasterHuman = null;
                        }
                    }
                    else
                    {
                        if (m_MasterHuman == null)
                        {
                            return;
                        }
                        sSayMsg = M2Share.g_sMasterListLongOutMasterOnlineMsg.Replace("%d", m_sMasterName);
                        sSayMsg = sSayMsg.Replace("%s", ChrName);
                        sSayMsg = sSayMsg.Replace("%m", Envir.MapDesc);
                        sSayMsg = sSayMsg.Replace("%x", CurrX.ToString());
                        sSayMsg = sSayMsg.Replace("%y", CurrY.ToString());
                        m_MasterHuman.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                        // 如果为大徒弟则将对方的记录清空
                        if (m_MasterHuman.m_sMasterName == ChrName)
                        {
                            m_MasterHuman.m_MasterHuman = null;
                        }
                        for (var i = 0; i < m_MasterHuman.m_MasterList.Count; i++)
                        {
                            if (m_MasterHuman.m_MasterList[i] == this)
                            {
                                m_MasterHuman.m_MasterList.RemoveAt(i);
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

        protected override void ScatterBagItems(BaseObject ItemOfCreat)
        {
            const int DropWide = 2;
            ClientUserItem pu;
            const string sExceptionMsg = "[Exception] TPlayObject::ScatterBagItems";
            IList<DeleteItem> DelList = null;
            if (AngryRing || NoDropItem || Envir.Flag.boNODROPITEM)
            {
                return;// 不死戒指
            }
            var boDropall = M2Share.Config.DieRedScatterBagAll && PvpLevel() >= 2;
            try
            {
                for (var i = ItemList.Count - 1; i >= 0; i--)
                {
                    if (boDropall || M2Share.RandomNumber.Random(M2Share.Config.DieScatterBagRate) == 0)
                    {
                        if (DropItemDown(ItemList[i], DropWide, true, ItemOfCreat, this))
                        {
                            pu = ItemList[i];
                            if (Race == ActorRace.Play)
                            {
                                if (DelList == null)
                                {
                                    DelList = new List<DeleteItem>();
                                }
                                DelList.Add(new DeleteItem()
                                {
                                    ItemName = M2Share.WorldEngine.GetStdItemName(pu.Index),
                                    MakeIndex = pu.MakeIndex
                                });
                            }
                            Dispose(ItemList[i]);
                            ItemList.RemoveAt(i);
                        }
                    }
                }
                if (DelList != null)
                {
                    var ObjectId = HUtil32.Sequence();
                    M2Share.ActorMgr.AddOhter(ObjectId, DelList);
                    this.SendMsg(this, Grobal2.RM_SENDDELITEMLIST, 0, ObjectId, 0, 0, "");
                }
            }
            catch
            {
                M2Share.Log.Error(sExceptionMsg);
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
                    if (baseObject != this.m_DearHuman)
                    {
                        result = true;
                    }
                    break;
                case AttackMode.HAM_MASTER:
                    if (baseObject.Race == ActorRace.Play)
                    {
                        result = true;
                        if (this.m_boMaster)
                        {
                            for (var i = 0; i < this.m_MasterList.Count; i++)
                            {
                                if (this.m_MasterList[i] == baseObject)
                                {
                                    result = false;
                                    break;
                                }
                            }
                        }
                        if (((PlayObject)baseObject).m_boMaster)
                        {
                            for (var i = 0; i < ((PlayObject)baseObject).m_MasterList.Count; i++)
                            {
                                if (((PlayObject)baseObject).m_MasterList[i] == this)
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
                            if (GuildWarArea && (baseObject.MyGuild != null))
                            {
                                if (MyGuild.IsAllyGuild(baseObject.MyGuild))
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
                            result = (baseObject as PlayObject).PvpLevel() < 2;
                        }
                        else
                        {
                            result = (baseObject as PlayObject).PvpLevel() >= 2;
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
            var result = base.IsProperFriend(attackTarget);
            if (attackTarget.Race > ActorRace.Play) return result;
            var targetObject = attackTarget as PlayObject;
            if (!targetObject.InFreePkArea)
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
        
        public void SetPkFlag(PlayObject baseObject)
        {
            if (baseObject.Race == ActorRace.Play)
            {
                if ((PvpLevel() < 2) && ((baseObject as PlayObject).PvpLevel() < 2) && (!Envir.Flag.boFightZone) && (!Envir.Flag.boFight3Zone) && !PvpFlag)
                {
                    baseObject.PvpNameColorTick = HUtil32.GetTickCount();
                    if (!baseObject.PvpFlag)
                    {
                        baseObject.PvpFlag = true;
                        baseObject.RefNameColor();
                    }
                }
            }
        }

        public void ChangePkStatus(bool boWarFlag)
        {
            if (InFreePkArea != boWarFlag)
            {
                InFreePkArea = boWarFlag;
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
    }
}