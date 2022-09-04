using GameSvr.Castle;
using GameSvr.Event;
using GameSvr.Guild;
using GameSvr.Items;
using GameSvr.Magic;
using GameSvr.Maps;
using GameSvr.Monster;
using GameSvr.Monster.Monsters;
using GameSvr.Player;
using System.Collections;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Actor
{
    public partial class TBaseObject : EntityId
    {
        public string MapName;
        public string MapFileName;
        /// <summary>
        /// 名称
        /// </summary>
        public string CharName;
        /// <summary>
        /// 所在座标X
        /// </summary>
        public short CurrX;
        /// <summary>
        /// 所在座标Y
        /// </summary>
        public short CurrY;
        /// <summary>
        /// 所在方向
        /// </summary>
        public byte Direction;
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
        /// 人物金币数
        /// </summary>
        public int Gold;
        public TAbility Abil;
        /// <summary>
        /// 状态值
        /// </summary>
        public int CharStatus;
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
        public bool OnHorse;
        public byte HorseType;
        protected byte DressEffType;
        /// <summary>
        /// 人物的PK值
        /// </summary>
        public int PkPoint;
        /// <summary>
        /// 允许组队
        /// </summary>
        public bool AllowGroup;
        /// <summary>
        /// 允许加入行会
        /// </summary>        
        protected bool AllowGuild;
        public byte btB2;
        public int IncHealth;
        public int IncSpell;
        public int m_nIncHealing;
        protected int m_nIncHPStoneTime;
        protected int m_nIncMPStoneTime;
        /// <summary>
        /// 在行会占争地图中死亡次数
        /// </summary>
        public int FightZoneDieCount;
        public TNakedAbility BonusAbil;
        protected TNakedAbility CurBonusAbil;
        public int m_nBonusPoint = 0;
        public int m_nHungerStatus = 0;
        public bool m_boAllowGuildReCall = false;
        public double m_dBodyLuck;
        public int m_nBodyLuckLevel;
        public short m_wGroupRcallTime;
        public bool m_boAllowGroupReCall;
        public byte[] m_QuestUnitOpen;
        public byte[] m_QuestUnit;
        public byte[] m_QuestFlag;
        protected long m_nCharStatusEx;
        /// <summary>
        /// 怪物经验值
        /// </summary>
        public int m_dwFightExp = 0;
        public TAbility m_WAbil;
        private TAddAbility m_AddAbil;
        /// <summary>
        /// 视觉范围大小
        /// </summary>
        protected byte ViewRange;
        public ushort[] m_wStatusTimeArr = new ushort[12];
        public int[] m_dwStatusArrTick = new int[12];
        public ushort[] m_wStatusArrValue;
        public int[] m_dwStatusArrTimeOutTick;
        public ushort m_wAppr;
        /// <summary>
        /// 角色类型
        /// </summary>
        public byte Race;
        /// <summary>
        /// 角色外形
        /// </summary>
        public byte RaceImg;
        /// <summary>
        /// 人物攻击准确度
        /// </summary>
        public byte m_btHitPoint;
        private ushort m_nHitPlus;
        private ushort m_nHitDouble;
        /// <summary>
        /// 记忆使用间隔
        /// </summary>
        public int GroupRcallTick;
        /// <summary>
        /// 记忆全套
        /// </summary>
        public bool m_boRecallSuite;

        public bool m_boRaceImg;
        public ushort m_nHealthRecover;
        public ushort m_nSpellRecover;
        public byte m_btAntiPoison;
        public ushort m_nPoisonRecover;
        public ushort m_nAntiMagic;
        /// <summary>
        /// 人物的幸运值
        /// </summary>
        public int m_nLuck;
        public int m_nPerHealth;
        public int m_nPerHealing;
        public int m_nPerSpell;
        public int m_dwIncHealthSpellTick;
        /// <summary>
        /// 中绿毒降HP点数
        /// </summary>
        private byte GreenPoisoningPoint;
        /// <summary>
        /// 人物身上最多可带金币数
        /// </summary>
        public int GoldMax;
        /// <summary>
        /// 敏捷度
        /// </summary>
        public byte SpeedPoint;
        /// <summary>
        /// 权限等级
        /// </summary>
        public byte Permission;
        /// <summary>
        /// 攻击速度
        /// </summary>
        protected ushort HitSpeed;
        public byte m_btLifeAttrib;
        public byte m_btCoolEye = 0;
        public TBaseObject m_GroupOwner;
        /// <summary>
        /// 组成员
        /// </summary>
        public IList<PlayObject> GroupMembers;
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
        protected bool AllowDeal;
        /// <summary>
        /// 禁止私聊人员列表
        /// </summary>
        public IList<string> m_BlockWhisperList;
        protected int ShoutMsgTick;
        /// <summary>
        /// 是否被召唤(主人)
        /// </summary>
        public TBaseObject Master;
        /// <summary>
        /// 怪物叛变时间
        /// </summary>
        public int MasterRoyaltyTick;
        public int MasterTick;
        /// <summary>
        /// 杀怪计数
        /// </summary>
        public int KillMonCount;
        /// <summary>
        /// 宝宝等级(1-7)
        /// </summary>
        public byte SlaveExpLevel;
        /// <summary>
        /// 召唤等级
        /// </summary>
        public byte SlaveMakeLevel;
        /// <summary>
        /// 下属列表
        /// </summary>        
        internal IList<TBaseObject> SlaveList;
        /// <summary>
        /// 宝宝攻击状态(休息/攻击)
        /// </summary>
        public bool SlaveRelax = false;
        /// <summary>
        /// 下属攻击状态
        /// </summary>
        public AttackMode AttatckMode = 0;
        /// <summary>
        /// 人物名字的颜色
        /// </summary>        
        public byte NameColor;
        /// <summary>
        /// 亮度
        /// </summary>
        public int Light;
        /// <summary>
        /// 行会占争范围
        /// </summary>
        private bool GuildWarArea;
        /// <summary>
        /// 所属城堡
        /// </summary>
        public TUserCastle Castle;
        public bool bo2B0;
        public int m_dw2B4Tick = 0;
        /// <summary>
        /// 无敌模式
        /// </summary>
        public bool SuperMan;
        public bool bo2B9;
        public bool bo2BA;
        public bool Animal;
        public bool m_boNoItem;
        public bool FixedHideMode;
        public bool m_boStickMode;
        public bool bo2BF;
        public bool m_boNoAttackMode;
        public bool m_boNoTame;
        public bool m_boSkeleton;
        public ushort m_nMeatQuality;
        public int m_nBodyLeathery;
        public bool m_boHolySeize;
        public int m_dwHolySeizeTick;
        public int m_dwHolySeizeInterval;
        public bool m_boCrazyMode;
        public int m_dwCrazyModeTick;
        public int m_dwCrazyModeInterval;
        public bool m_boShowHP;
        /// <summary>
        /// 心灵启示检查时间
        /// </summary>
        public int m_dwShowHPTick = 0;
        /// <summary>
        /// 心灵启示有效时长
        /// </summary>
        public int m_dwShowHPInterval = 0;
        public bool bo2F0;
        public int m_dwDupObjTick = 0;
        public Envirnoment Envir;
        public bool Ghost;
        public int GhostTick;
        public bool Death;
        public int DeathTick;
        public bool m_boInvisible;
        public bool m_boCanReAlive;
        /// <summary>
        /// 复活时间
        /// </summary>
        public int ReAliveTick = 0;
        public MonGenInfo MonGen;
        /// <summary>
        /// 怪物所拿的武器
        /// </summary>
        public byte MonsterWeapon = 0;
        public int StruckTick = 0;
        protected bool WantRefMsg;
        public bool AddtoMapSuccess;
        public bool m_bo316;
        /// <summary>
        /// 正在交易
        /// </summary>
        protected bool Dealing;
        /// <summary>
        /// 交易最后操作时间
        /// </summary>
        internal int DealLastTick = 0;
        /// <summary>
        /// 交易对象
        /// </summary>
        protected TBaseObject DealCreat;
        public GuildInfo MyGuild;
        public int GuildRankNo;
        public string GuildRankName = string.Empty;
        public string ScriptLable = string.Empty;
        protected byte AttackSkillCount;
        protected byte AttackSkillPointCount;
        public bool m_boMission;
        public short m_nMissionX = 0;
        public short m_nMissionY = 0;
        /// <summary>
        /// 隐身戒指
        /// </summary>
        public bool HideMode;
        public bool StoneMode;
        /// <summary>
        /// 是否可以看到隐身人物
        /// </summary>
        public bool CoolEye;
        /// <summary>
        /// 是否用了神水
        /// </summary>
        protected bool UserUnLockDurg;
        /// <summary>
        /// 魔法隐身了
        /// </summary>
        public bool Transparent;
        /// <summary>
        /// 管理模式
        /// </summary>
        public bool AdminMode;
        /// <summary>
        /// 隐身模式
        /// </summary>
        public bool ObMode;
        /// <summary>
        /// 传送戒指
        /// </summary>
        public bool Teleport = false;
        /// <summary>
        /// 麻痹戒指
        /// </summary>
        internal bool Paralysis = false;
        internal bool UnParalysis = false;
        /// <summary>
        /// 复活戒指
        /// </summary>
        internal bool Revival = false;
        /// <summary>
        /// 防复活
        /// </summary>
        internal bool UnRevival = false;
        /// <summary>
        /// 复活戒指使用间隔计数
        /// </summary>
        internal int RevivalTick = 0;
        /// <summary>
        /// 火焰戒指
        /// </summary>
        internal bool FlameRing = false;
        /// <summary>
        /// 治愈戒指
        /// </summary>
        internal bool RecoveryRing;
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
        internal bool UnMagicShield = false;
        /// <summary>
        /// 活力戒指
        /// </summary>
        internal bool MuscleRing = false;
        /// <summary>
        /// 技巧项链
        /// </summary>
        internal bool FastTrain = false;
        /// <summary>
        /// 探测项链
        /// </summary>
        public bool ProbeNecklace = false;
        /// <summary>
        /// 行会传送
        /// </summary>
        public bool GuildMove = false;

        private bool SuperManItem;

        /// <summary>
        /// 祈祷
        /// </summary>
        internal bool m_bopirit = false;

        public bool NoDropItem = false;
        public bool NoDropUseItem = false;
        internal bool m_boExpItem = false;
        internal bool m_boPowerItem = false;
        internal int ExpItem = 0;
        internal int PowerItem = 0;

        /// <summary>
        /// PK 死亡掉经验，不够经验就掉等级
        /// </summary>
        internal int m_dwPKDieLostExp;
        /// <summary>
        /// PK 死亡掉等级
        /// </summary>
        internal int m_nPKDieLostLevel;
        /// <summary>
        /// 心灵启示
        /// </summary>
        public bool AbilSeeHealGauge;
        /// <summary>
        /// 魔法盾
        /// </summary>
        internal bool AbilMagBubbleDefence;
        /// <summary>
        /// 魔法盾等级
        /// </summary>
        internal byte MagBubbleDefenceLevel;
        public int SearchTime;
        public int SearchTick;
        /// <summary>
        /// 上次运行时间
        /// </summary>
        public int m_dwRunTick;
        public int m_nRunTime;
        internal int m_nHealthTick;
        internal int m_nSpellTick;
        public TBaseObject TargetCret;
        public int TargetFocusTick = 0;
        /// <summary>
        /// 人物被对方杀害时对方指针
        /// </summary>
        public TBaseObject LastHiter;
        public int LastHiterTick;
        public TBaseObject ExpHitter;
        internal int ExpHitterTick;
        /// <summary>
        /// 传送戒指使用间隔
        /// </summary>
        public int TeleportTick;
        /// <summary>
        /// 探测项链使用间隔
        /// </summary>
        public int ProbeTick;
        protected int MapMoveTick;
        /// <summary>
        /// 人物攻击变色标志
        /// </summary>
        internal bool m_boPKFlag;
        /// <summary>
        /// 人物攻击变色时间长度
        /// </summary>
        internal int m_dwPKTick;
        /// <summary>
        /// 魔血一套
        /// </summary>
        internal int m_nMoXieSuite;
        /// <summary>
        /// 虹魔一套
        /// </summary>
        internal int m_nHongMoSuite;
        public double m_db3B0;
        /// <summary>
        /// 中毒处理间隔时间
        /// </summary>
        internal int PoisoningTick;
        /// <summary>
        /// 减PK值时间
        /// </summary>
        internal int m_dwDecPkPointTick;
        internal int m_DecLightItemDrugTick;
        internal int m_dwVerifyTick;
        internal int m_dwCheckRoyaltyTick;
        internal int m_dwDecHungerPointTick;
        internal int m_dwHPMPTick;
        internal IList<SendMessage> MsgList;
        internal IList<TBaseObject> VisibleHumanList;
        internal IList<VisibleMapItem> VisibleItems;
        internal IList<MirEvent> VisibleEvents;
        internal int SendRefMsgTick;
        /// <summary>
        /// 是否在开行会战
        /// </summary>
        protected bool InFreePKArea;
        protected int AttackTick = 0;
        protected int WalkTick;
        protected int SearchEnemyTick = 0;
        protected bool NameColorChanged;
        /// <summary>
        /// 是否在可视范围内有人物,及宝宝
        /// </summary>
        public bool IsVisibleActive;
        /// <summary>
        /// 当前处理数量
        /// </summary>
        public short ProcessRunCount;
        /// <summary>
        /// 可见玩家列表
        /// </summary>
        public IList<VisibleBaseObject> VisibleActors;
        /// <summary>
        /// 人物背包
        /// </summary>
        public IList<TUserItem> ItemList;
        /// <summary>
        /// 交易列表
        /// </summary>
        public IList<TUserItem> DealItemList;
        /// <summary>
        /// 交易的金币数量
        /// </summary>
        public int DealGolds;
        /// <summary>
        /// 确认交易标志
        /// </summary>
        public bool DealSuccess = false;
        /// <summary>
        /// 技能表
        /// </summary>
        public IList<TUserMagic> MagicList;

        /// <summary>
        /// 身上物品
        /// </summary>
        public TUserItem[] UseItems;
        public IList<TMonSayMsg> SayMsgList;
        /// <summary>
        /// 仓库物品列表
        /// </summary>
        public IList<TUserItem> StorageItemList;
        /// <summary>
        /// 走路速度
        /// </summary>
        public int WalkSpeed;
        public int WalkStep = 0;
        protected int WalkCount;
        public int WalkWait = 0;
        protected int WalkWaitTick;
        protected bool WalkWaitLocked;
        /// <summary>
        /// 下次攻击时间
        /// </summary>
        public int NextHitTime;
        protected TUserMagic[] MagicArr;
        protected bool PowerHit;
        protected bool UseThrusting;
        protected bool UseHalfMoon;
        protected bool RedUseHalfMoon;
        protected bool FireHitSkill;
        protected bool CrsHitkill = false;
        public bool m_bo41kill = false;
        public bool m_boTwinHitSkill;
        public bool m_bo43kill = false;
        public int m_dwLatestFireHitTick = 0;
        public int m_dwDoMotaeboTick = 0;
        public int m_dwLatestTwinHitTick = 0;
        /// <summary>
        /// 是否刷新在地图上信息
        /// </summary>
        protected bool DenyRefStatus;
        /// <summary>
        /// 是否增加地图计数
        /// </summary>
        public bool AddToMaped;
        /// <summary>
        /// 是否从地图中删除计数
        /// </summary>
        public bool DelFormMaped = false;
        public bool AutoChangeColor;
        protected int AutoChangeColorTick;
        protected int AutoChangeIdx;
        /// <summary>
        /// 固定颜色
        /// </summary>
        public bool FixColor;
        public int FixColorIdx;
        protected int FixStatus;
        /// <summary>
        /// 快速麻痹，受攻击后麻痹立即消失
        /// </summary>
        protected bool FastParalysis;
        protected bool SmashSet = false;
        protected bool HwanDevilSet = false;
        protected bool PuritySet = false;
        protected bool MundaneSet = false;
        protected bool NokChiSet = false;
        protected bool TaoBuSet = false;
        protected bool FiveStringSet = false;
        public bool OffLineFlag = false;
        // 挂机
        public string m_sOffLineLeaveword = string.Empty;
        // 挂机字符
        public int m_dwKickOffLineTick = 0;
        public bool m_boNastyMode;
        /// <summary>
        /// 气血石
        /// </summary>
        public int m_nAutoAddHPMPMode = 0;
        public int m_dwCheckHPMPTick = 0;
        public long dwTick3F4 = 0;
        /// <summary>
        /// 是否机器人
        /// </summary>
        public bool IsRobot;

        public TBaseObject()
        {
            Ghost = false;
            GhostTick = 0;
            Death = false;
            DeathTick = 0;
            SendRefMsgTick = HUtil32.GetTickCount();
            Direction = 4;
            Race = Grobal2.RC_ANIMAL;
            RaceImg = 0;
            Hair = 0;
            Job = PlayJob.Warrior;
            Gold = 0;
            m_wAppr = 0;
            bo2B9 = true;
            ViewRange = 5;
            HomeMap = "0";
            Permission = 0;
            Light = 0;
            NameColor = 255;
            m_nHitPlus = 0;
            m_nHitDouble = 0;
            m_dBodyLuck = 0;
            m_wGroupRcallTime = 0;
            GroupRcallTick = HUtil32.GetTickCount();
            m_boRecallSuite = false;
            m_boRaceImg = false;
            bo2BA = false;
            AbilSeeHealGauge = false;
            PowerHit = false;
            UseThrusting = false;
            UseHalfMoon = false;
            RedUseHalfMoon = false;
            FireHitSkill = false;
            m_boTwinHitSkill = false;
            m_btHitPoint = 5;
            SpeedPoint = 15;
            HitSpeed = 0;
            m_btLifeAttrib = 0;
            m_btAntiPoison = 0;
            m_nPoisonRecover = 0;
            m_nHealthRecover = 0;
            m_nSpellRecover = 0;
            m_nAntiMagic = 0;
            m_nLuck = 0;
            IncSpell = 0;
            IncHealth = 0;
            m_nIncHealing = 0;
            m_nIncHPStoneTime = HUtil32.GetTickCount();
            m_nIncMPStoneTime = HUtil32.GetTickCount();
            m_nPerHealth = 5;
            m_nPerHealing = 5;
            m_nPerSpell = 5;
            m_dwIncHealthSpellTick = HUtil32.GetTickCount();
            GreenPoisoningPoint = 0;
            FightZoneDieCount = 0;
            GoldMax = M2Share.g_Config.nHumanMaxGold;
            CharStatus = 0;
            m_nCharStatusEx = 0;
            m_wStatusTimeArr = new ushort[12];
            BonusAbil = new TNakedAbility();
            CurBonusAbil = new TNakedAbility();
            m_wStatusArrValue = new ushort[6];
            m_dwStatusArrTimeOutTick = new int[6];
            AllowGroup = false;
            AllowGuild = false;
            btB2 = 0;
            AttatckMode = 0;
            InFreePKArea = false;
            GuildWarArea = false;
            bo2B0 = false;
            SuperMan = false;
            m_boSkeleton = false;
            bo2BF = false;
            m_boHolySeize = false;
            m_boCrazyMode = false;
            m_boShowHP = false;
            bo2F0 = false;
            Animal = false;
            m_boNoItem = false;
            m_nBodyLeathery = 50;
            FixedHideMode = false;
            m_boStickMode = false;
            m_boNoAttackMode = false;
            m_boNoTame = false;
            m_boPKFlag = false;
            m_nMoXieSuite = 0;
            m_nHongMoSuite = 0;
            m_db3B0 = 0;
            m_AddAbil = new TAddAbility();
            MsgList = new List<SendMessage>();
            VisibleHumanList = new List<TBaseObject>();
            VisibleActors = new List<VisibleBaseObject>();
            VisibleItems = new List<VisibleMapItem>();
            VisibleEvents = new List<MirEvent>();
            ItemList = new List<TUserItem>();
            DealItemList = new List<TUserItem>();
            IsVisibleActive = false;
            ProcessRunCount = 0;
            DealGolds = 0;
            MagicList = new List<TUserMagic>();
            StorageItemList = new List<TUserItem>();
            UseItems = new TUserItem[13];
            m_GroupOwner = null;
            Castle = null;
            Master = null;
            KillMonCount = 0;
            SlaveExpLevel = 0;
            GroupMembers = new List<PlayObject>();
            HearWhisper = true;
            BanShout = true;
            BanGuildChat = true;
            AllowDeal = true;
            m_boAllowGroupReCall = false;
            m_BlockWhisperList = new List<string>();
            SlaveList = new List<TBaseObject>();
            m_WAbil = new TAbility();
            m_QuestUnitOpen = new byte[128];
            m_QuestUnit = new byte[128];
            m_QuestFlag = new byte[128];
            Abil = new TAbility
            {
                Level = 1,
                AC = 0,
                MAC = 0,
                DC = HUtil32.MakeLong(1, 4),
                MC = HUtil32.MakeLong(1, 2),
                SC = HUtil32.MakeLong(1, 2),
                HP = 15,
                MP = 15,
                MaxHP = 15,
                MaxMP = 15,
                Exp = 0,
                MaxExp = 50,
                Weight = 0,
                MaxWeight = 100
            };
            WantRefMsg = false;
            Dealing = false;
            DealCreat = null;
            MyGuild = null;
            GuildRankNo = 0;
            GuildRankName = "";
            ScriptLable = "";
            m_boMission = false;
            HideMode = false;
            StoneMode = false;
            CoolEye = false;
            UserUnLockDurg = false;
            Transparent = false;
            AdminMode = false;
            ObMode = false;
            m_dwRunTick = HUtil32.GetTickCount() + M2Share.RandomNumber.Random(1500);
            m_nRunTime = 250;
            SearchTime = M2Share.RandomNumber.Random(2000) + 2000;
            SearchTick = HUtil32.GetTickCount();
            m_dwDecPkPointTick = HUtil32.GetTickCount();
            m_DecLightItemDrugTick = HUtil32.GetTickCount();
            PoisoningTick = HUtil32.GetTickCount();
            m_dwVerifyTick = HUtil32.GetTickCount();
            m_dwCheckRoyaltyTick = HUtil32.GetTickCount();
            m_dwDecHungerPointTick = HUtil32.GetTickCount();
            m_dwHPMPTick = HUtil32.GetTickCount();
            ShoutMsgTick = 0;
            TeleportTick = 0;
            ProbeTick = 0;
            MapMoveTick = HUtil32.GetTickCount();
            MasterTick = 0;
            WalkSpeed = 1400;
            NextHitTime = 2000;
            WalkCount = 0;
            WalkWaitTick = HUtil32.GetTickCount();
            WalkWaitLocked = false;
            m_nHealthTick = 0;
            m_nSpellTick = 0;
            TargetCret = null;
            LastHiter = null;
            ExpHitter = null;
            SayMsgList = null;
            DenyRefStatus = false;
            HorseType = 0;
            DressEffType = 0;
            m_dwPKDieLostExp = 0;
            m_nPKDieLostLevel = 0;
            AddToMaped = true;
            AutoChangeColor = false;
            AutoChangeColorTick = HUtil32.GetTickCount();
            AutoChangeIdx = 0;
            FixColor = false;
            FixColorIdx = 0;
            FixStatus = -1;
            FastParalysis = false;
            m_boNastyMode = false;
            MagicArr = new TUserMagic[100];
            M2Share.ActorMgr.Add(ObjectId, this);
        }

        public void ChangePKStatus(bool boWarFlag)
        {
            if (InFreePKArea != boWarFlag)
            {
                InFreePKArea = boWarFlag;
                NameColorChanged = true;
            }
        }

        public bool GetDropPosition(int nOrgX, int nOrgY, int nRange, ref int nDX, ref int nDY)
        {
            var result = false;
            var nItemCount = 0;
            var n24 = 999;
            var n28 = 0;
            var n2C = 0;
            for (var i = 0; i < nRange; i++)
            {
                for (var ii = -i; ii <= i; ii++)
                {
                    for (var iii = -i; iii <= i; iii++)
                    {
                        nDX = nOrgX + iii;
                        nDY = nOrgY + ii;
                        if (Envir.GetItemEx(nDX, nDY, ref nItemCount) == 0)
                        {
                            if (Envir.Bo2C)
                            {
                                result = true;
                                break;
                            }
                        }
                        else
                        {
                            if (Envir.Bo2C && n24 > nItemCount)
                            {
                                n24 = nItemCount;
                                n28 = nDX;
                                n2C = nDY;
                            }
                        }
                    }

                    if (result)
                    {
                        break;
                    }
                }

                if (result)
                {
                    break;
                }
            }

            if (!result)
            {
                if (n24 < 8)
                {
                    nDX = n28;
                    nDY = n2C;
                }
                else
                {
                    nDX = nOrgX;
                    nDY = nOrgY;
                }
            }

            return result;
        }

        public bool DropItemDown(TUserItem UserItem, int nScatterRange, bool boDieDrop, TBaseObject ItemOfCreat,
            TBaseObject DropCreat)
        {
            bool result = false;
            int dx = 0;
            int dy = 0;
            int idura;
            MapItem MapItem;
            MapItem pr;
            string logcap;
            if (UserItem == null)
            {
                return false;
            }

            StdItem StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
            if (StdItem != null)
            {
                if (StdItem.StdMode == 40)
                {
                    idura = UserItem.Dura;
                    idura = idura - 2000;
                    if (idura < 0)
                    {
                        idura = 0;
                    }

                    UserItem.Dura = (ushort)idura;
                }

                MapItem = new MapItem
                {
                    UserItem = UserItem,
                    Name = ItmUnit.GetItemName(UserItem), // 取自定义物品名称
                    Looks = StdItem.Looks
                };
                if (StdItem.StdMode == 45)
                {
                    MapItem.Looks = (ushort)M2Share.GetRandomLook(MapItem.Looks, StdItem.Shape);
                }

                MapItem.AniCount = StdItem.AniCount;
                MapItem.Reserved = 0;
                MapItem.Count = 1;
                MapItem.OfBaseObject = ItemOfCreat.ObjectId;
                MapItem.CanPickUpTick = HUtil32.GetTickCount();
                MapItem.DropBaseObject = DropCreat.ObjectId;
                GetDropPosition(CurrX, CurrY, nScatterRange, ref dx, ref dy);
                pr = (MapItem)Envir.AddToMap(dx, dy, CellType.ItemObject, MapItem);
                if (pr == MapItem)
                {
                    SendRefMsg(Grobal2.RM_ITEMSHOW, MapItem.Looks, MapItem.ObjectId, dx, dy, MapItem.Name);
                    if (boDieDrop)
                    {
                        logcap = "15";
                    }
                    else
                    {
                        logcap = "7";
                    }

                    if (!M2Share.IsCheapStuff(StdItem.StdMode))
                    {
                        if (StdItem.NeedIdentify == 1)
                        {
                            M2Share.AddGameDataLog(logcap + "\t" + MapName + "\t" + CurrX + "\t" + CurrY +
                                                   "\t" + CharName + "\t" + StdItem.Name + "\t" +
                                                   UserItem.MakeIndex + "\t" +
                                                   HUtil32.BoolToIntStr(Race == Grobal2.RC_PLAYOBJECT) +
                                                   "\t" + '0');
                        }
                    }

                    result = true;
                }
                else
                {
                    MapItem = null;
                }
            }

            return result;
        }

        public void GoldChanged()
        {
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                SendUpdateMsg(this, Grobal2.RM_GOLDCHANGED, 0, 0, 0, 0, "");
            }
        }

        public void GameGoldChanged()
        {
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                SendUpdateMsg(this, Grobal2.RM_GAMEGOLDCHANGED, 0, 0, 0, 0, "");
            }
        }

        public void RecalcLevelAbilitys()
        {
            int n;
            double nLevel = Abil.Level;
            switch (Job)
            {
                case PlayJob.Taoist:
                    Abil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue,
                        14 + HUtil32.Round((nLevel / M2Share.g_Config.nLevelValueOfTaosHP +
                                            M2Share.g_Config.nLevelValueOfTaosHPRate) * nLevel));
                    Abil.MaxMP = (ushort)HUtil32._MIN(short.MaxValue,
                        13 + HUtil32.Round(nLevel / M2Share.g_Config.nLevelValueOfTaosMP * 2.2 * nLevel));
                    Abil.MaxWeight = (ushort)(50 + HUtil32.Round(nLevel / 4 * nLevel));
                    Abil.MaxWearWeight = (byte)(15 + HUtil32.Round(nLevel / 50 * nLevel));
                    if ((12 + HUtil32.Round((Abil.Level / 13) * Abil.Level)) > 255)
                    {
                        Abil.MaxHandWeight = byte.MaxValue;
                    }
                    else
                    {
                        Abil.MaxHandWeight = (byte)(12 + HUtil32.Round(nLevel / 42 * nLevel));
                    }

                    n = (int)(nLevel / 7);
                    Abil.DC = HUtil32.MakeLong(HUtil32._MAX(n - 1, 0), HUtil32._MAX(1, n));
                    Abil.MC = 0;
                    Abil.SC = HUtil32.MakeLong(HUtil32._MAX(n - 1, 0), HUtil32._MAX(1, n));
                    Abil.AC = 0;
                    n = HUtil32.Round(nLevel / 6);
                    Abil.MAC = HUtil32.MakeLong(n / 2, n + 1);
                    break;
                case PlayJob.Wizard:
                    Abil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue,
                        14 + HUtil32.Round((nLevel / M2Share.g_Config.nLevelValueOfWizardHP +
                                            M2Share.g_Config.nLevelValueOfWizardHPRate) * nLevel));
                    Abil.MaxMP = (ushort)HUtil32._MIN(short.MaxValue,
                        13 + HUtil32.Round((nLevel / 5 + 2) * 2.2 * nLevel));
                    Abil.MaxWeight = (ushort)(50 + HUtil32.Round(nLevel / 5 * nLevel));
                    Abil.MaxWearWeight =
                        (byte)HUtil32._MIN(short.MaxValue, 15 + HUtil32.Round(nLevel / 100 * nLevel));
                    Abil.MaxHandWeight = (byte)(12 + HUtil32.Round(nLevel / 90 * nLevel));
                    n = (int)(nLevel / 7);
                    Abil.DC = HUtil32.MakeLong(HUtil32._MAX(n - 1, 0), HUtil32._MAX(1, n));
                    Abil.MC = HUtil32.MakeLong(HUtil32._MAX(n - 1, 0), HUtil32._MAX(1, n));
                    Abil.SC = 0;
                    Abil.AC = 0;
                    Abil.MAC = 0;
                    break;
                case PlayJob.Warrior:
                    Abil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue,
                        14 + HUtil32.Round((nLevel / M2Share.g_Config.nLevelValueOfWarrHP +
                                            M2Share.g_Config.nLevelValueOfWarrHPRate + nLevel / 20) * nLevel));
                    Abil.MaxMP = (ushort)HUtil32._MIN(short.MaxValue, 11 + HUtil32.Round(nLevel * 3.5));
                    Abil.MaxWeight = (ushort)(50 + HUtil32.Round(nLevel / 3 * nLevel));
                    Abil.MaxWearWeight = (byte)(15 + HUtil32.Round(nLevel / 20 * nLevel));
                    Abil.MaxHandWeight = (byte)(12 + HUtil32.Round(nLevel / 13 * nLevel));
                    Abil.DC = HUtil32.MakeLong(HUtil32._MAX((int)(nLevel / 5) - 1, 1),
                        HUtil32._MAX(1, (int)(nLevel / 5)));
                    Abil.SC = 0;
                    Abil.MC = 0;
                    Abil.AC = (ushort)HUtil32.MakeLong(0, nLevel / 7);
                    Abil.MAC = 0;
                    break;
            }

            if (Abil.HP > Abil.MaxHP)
            {
                Abil.HP = Abil.MaxHP;
            }

            if (Abil.MP > Abil.MaxMP)
            {
                Abil.MP = Abil.MaxMP;
            }
        }

        public void HasLevelUp(int nLevel)
        {
            Abil.MaxExp = GetLevelExp(Abil.Level);
            RecalcLevelAbilitys();
            RecalcAbilitys();
            SendMsg(this, Grobal2.RM_LEVELUP, 0, (int)Abil.Exp, 0, 0, "");
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(this as PlayObject, "@LevelUp", false);
            }
        }

        protected bool WalkTo(byte btDir, bool boFlag)
        {
            short nOX = 0;
            short nOY = 0;
            short nNX = 0;
            short nNY = 0;
            short n20 = 0;
            short n24 = 0;
            bool bo29;
            const string sExceptionMsg = "[Exception] TBaseObject::WalkTo";
            bool result = false;
            if (m_boHolySeize)
            {
                return result;
            }

            try
            {
                nOX = CurrX;
                nOY = CurrY;
                Direction = btDir;
                nNX = 0;
                nNY = 0;
                switch (btDir)
                {
                    case Grobal2.DR_UP:
                        nNX = CurrX;
                        nNY = (short)(CurrY - 1);
                        break;
                    case Grobal2.DR_UPRIGHT:
                        nNX = (short)(CurrX + 1);
                        nNY = (short)(CurrY - 1);
                        break;
                    case Grobal2.DR_RIGHT:
                        nNX = (short)(CurrX + 1);
                        nNY = CurrY;
                        break;
                    case Grobal2.DR_DOWNRIGHT:
                        nNX = (short)(CurrX + 1);
                        nNY = (short)(CurrY + 1);
                        break;
                    case Grobal2.DR_DOWN:
                        nNX = CurrX;
                        nNY = (short)(CurrY + 1);
                        break;
                    case Grobal2.DR_DOWNLEFT:
                        nNX = (short)(CurrX - 1);
                        nNY = (short)(CurrY + 1);
                        break;
                    case Grobal2.DR_LEFT:
                        nNX = (short)(CurrX - 1);
                        nNY = CurrY;
                        break;
                    case Grobal2.DR_UPLEFT:
                        nNX = (short)(CurrX - 1);
                        nNY = (short)(CurrY - 1);
                        break;
                }

                if (nNX >= 0 && Envir.Width - 1 >= nNX && nNY >= 0 && Envir.Height - 1 >= nNY)
                {
                    bo29 = true;
                    if (bo2BA && !Envir.CanSafeWalk(nNX, nNY))
                    {
                        bo29 = false;
                    }

                    if (Master != null)
                    {
                        Master.Envir.GetNextPosition(Master.CurrX, Master.CurrY, Master.Direction, 1,
                            ref n20, ref n24);
                        if (nNX == n20 && nNY == n24)
                        {
                            bo29 = false;
                        }
                    }

                    if (bo29)
                    {
                        if (Envir.MoveToMovingObject(CurrX, CurrY, this, nNX, nNY, boFlag) > 0)
                        {
                            CurrX = nNX;
                            CurrY = nNY;
                        }
                    }
                }

                if (CurrX != nOX || CurrY != nOY)
                {
                    if (Walk(Grobal2.RM_WALK))
                    {
                        if (Transparent && HideMode)
                        {
                            m_wStatusTimeArr[Grobal2.STATE_TRANSPARENT] = 1;
                        }

                        result = true;
                    }
                    else
                    {
                        Envir.DeleteFromMap(CurrX, CurrY, CellType.MovingObject, this);
                        CurrX = nOX;
                        CurrY = nOY;
                        Envir.AddToMap(CurrX, CurrY, CellType.MovingObject, this);
                    }
                }
            }
            catch
            {
                M2Share.LogSystem.Error(sExceptionMsg);
            }

            return result;
        }

        public bool IsGroupMember(TBaseObject target)
        {
            bool result = false;
            if (m_GroupOwner == null)
            {
                return result;
            }

            for (int i = 0; i < m_GroupOwner.GroupMembers.Count; i++)
            {
                if (m_GroupOwner.GroupMembers[i] == target)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public int PKLevel()
        {
            return PkPoint / 100;
        }

        public void HealthSpellChanged()
        {
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                SendUpdateMsg(this, Grobal2.RM_HEALTHSPELLCHANGED, 0, 0, 0, 0, "");
            }

            if (m_boShowHP)
            {
                SendRefMsg(Grobal2.RM_HEALTHSPELLCHANGED, 0, 0, 0, 0, "");
            }
        }

        public int CalcGetExp(int nLevel, int nExp)
        {
            int result;
            if (M2Share.g_Config.boHighLevelKillMonFixExp || (Abil.Level < (nLevel + 10)))
            {
                result = nExp;
            }
            else
            {
                result = nExp - HUtil32.Round(nExp / 15 * (Abil.Level - (nLevel + 10)));
            }

            if (result <= 0)
            {
                result = 1;
            }

            return result;
        }

        public void RefNameColor()
        {
            SendRefMsg(Grobal2.RM_CHANGENAMECOLOR, 0, 0, 0, 0, "");
        }

        private int GainSlaveUpKillCount()
        {
            int tCount;
            if (SlaveExpLevel < Grobal2.SlaveMaxLevel - 2)
            {
                tCount = M2Share.g_Config.MonUpLvNeedKillCount[SlaveExpLevel];
            }
            else
            {
                tCount = 0;
            }

            return (Abil.Level * M2Share.g_Config.nMonUpLvRate) - Abil.Level +
                   M2Share.g_Config.nMonUpLvNeedKillBase + tCount;
        }

        private void GainSlaveExp(int nLevel)
        {
            KillMonCount += nLevel;
            if (GainSlaveUpKillCount() < KillMonCount)
            {
                KillMonCount -= GainSlaveUpKillCount();
                if (SlaveExpLevel < (SlaveMakeLevel * 2 + 1))
                {
                    SlaveExpLevel++;
                    RecalcAbilitys();
                    RefNameColor();
                }
            }
        }

        protected bool DropGoldDown(int nGold, bool boFalg, TBaseObject GoldOfCreat, TBaseObject DropGoldCreat)
        {
            bool result = false;
            int nX = 0;
            int nY = 0;
            string s20;
            int DropWide = HUtil32._MIN(M2Share.g_Config.nDropItemRage, 7);
            MapItem MapItem = new MapItem
            {
                Name = Grobal2.sSTRING_GOLDNAME,
                Count = nGold,
                Looks = M2Share.GetGoldShape(nGold),
                OfBaseObject = GoldOfCreat.ObjectId,
                CanPickUpTick = HUtil32.GetTickCount(),
                DropBaseObject = DropGoldCreat.ObjectId
            };
            GetDropPosition(CurrX, CurrY, 3, ref nX, ref nY);
            MapItem MapItemA = (MapItem)Envir.AddToMap(nX, nY, CellType.ItemObject, MapItem);
            if (MapItemA != null)
            {
                if (MapItemA != MapItem)
                {
                    MapItem = null;
                    MapItem = MapItemA;
                }

                SendRefMsg(Grobal2.RM_ITEMSHOW, MapItem.Looks, MapItem.ObjectId, nX, nY, MapItem.Name);
                if (Race == Grobal2.RC_PLAYOBJECT)
                {
                    if (boFalg)
                    {
                        s20 = "15";
                    }
                    else
                    {
                        s20 = "7";
                    }

                    if (M2Share.g_boGameLogGold)
                    {
                        M2Share.AddGameDataLog(s20 + "\t" + MapName + "\t" + CurrX + "\t" + CurrY + "\t" +
                                               CharName + "\t" + Grobal2.sSTRING_GOLDNAME + "\t" + nGold + "\t" +
                                               HUtil32.BoolToIntStr(Race == Grobal2.RC_PLAYOBJECT) + "\t" +
                                               '0');
                    }
                }

                result = true;
            }
            else
            {
                MapItem = null;
            }

            return result;
        }

        public int GetGuildRelation(TBaseObject cert1, TBaseObject cert2)
        {
            int result = 0;
            GuildWarArea = false;
            if ((cert1.MyGuild == null) || (cert2.MyGuild == null))
            {
                return result;
            }

            if (cert1.InSafeArea() || cert2.InSafeArea())
            {
                return result;
            }

            if (cert1.MyGuild.GuildWarList.Count <= 0)
            {
                return result;
            }

            GuildWarArea = true;
            if (cert1.MyGuild.IsWarGuild(cert2.MyGuild) && cert2.MyGuild.IsWarGuild(cert1.MyGuild))
            {
                result = 2;
            }

            if (cert1.MyGuild == cert2.MyGuild)
            {
                result = 1;
            }

            if (cert1.MyGuild.IsAllyGuild(cert2.MyGuild) && cert2.MyGuild.IsAllyGuild(cert1.MyGuild))
            {
                result = 3;
            }

            return result;
        }

        protected void IncPkPoint(int nPoint)
        {
            var nOldPKLevel = PKLevel();
            PkPoint += nPoint;
            if (PKLevel() != nOldPKLevel)
            {
                if (PKLevel() <= 2)
                {
                    RefNameColor();
                }
            }
        }

        private void DecPKPoint(int nPoint)
        {
            int nC = PKLevel();
            PkPoint -= nPoint;
            if (PkPoint < 0)
            {
                PkPoint = 0;
            }

            if ((PKLevel() != nC) && (nC > 0) && (nC <= 2))
            {
                RefNameColor();
            }
        }

        protected void AddBodyLuck(double dLuck)
        {
            if ((dLuck > 0) && (m_dBodyLuck < 5 * M2Share.BODYLUCKUNIT))
            {
                m_dBodyLuck = m_dBodyLuck + dLuck;
            }

            if ((dLuck < 0) && (m_dBodyLuck > -(5 * M2Share.BODYLUCKUNIT)))
            {
                m_dBodyLuck = m_dBodyLuck + dLuck;
            }

            int n = Convert.ToInt32(m_dBodyLuck / M2Share.BODYLUCKUNIT);
            if (n > 5)
            {
                n = 5;
            }

            if (n < -10)
            {
                n = -10;
            }

            m_nBodyLuckLevel = n;
        }

        protected void MakeWeaponUnlock()
        {
            if (UseItems[Grobal2.U_WEAPON] == null)
            {
                return;
            }

            if (UseItems[Grobal2.U_WEAPON].wIndex <= 0)
            {
                return;
            }

            if (UseItems[Grobal2.U_WEAPON].btValue[3] > 0)
            {
                UseItems[Grobal2.U_WEAPON].btValue[3] -= 1;
                SysMsg(M2Share.g_sTheWeaponIsCursed, MsgColor.Red, MsgType.Hint);
            }
            else
            {
                if (UseItems[Grobal2.U_WEAPON].btValue[4] < 10)
                {
                    UseItems[Grobal2.U_WEAPON].btValue[4]++;
                    SysMsg(M2Share.g_sTheWeaponIsCursed, MsgColor.Red, MsgType.Hint);
                }
            }

            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                RecalcAbilitys();
                SendMsg(this, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                SendMsg(this, Grobal2.RM_SUBABILITY, 0, 0, 0, 0, "");
            }
        }

        public ushort GetAttackPower(int nBasePower, int nPower)
        {
            int result;
            PlayObject PlayObject;
            if (nPower < 0)
            {
                nPower = 0;
            }

            if (m_nLuck > 0)
            {
                if (M2Share.RandomNumber.Random(10 - HUtil32._MIN(9, m_nLuck)) == 0)
                {
                    result = nBasePower + nPower;
                }
                else
                {
                    result = nBasePower + M2Share.RandomNumber.Random(nPower + 1);
                }
            }
            else
            {
                result = nBasePower + M2Share.RandomNumber.Random(nPower + 1);
                if (m_nLuck < 0)
                {
                    if (M2Share.RandomNumber.Random(10 - HUtil32._MAX(0, -m_nLuck)) == 0)
                    {
                        result = nBasePower;
                    }
                }
            }

            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                PlayObject = this as PlayObject;
                result = HUtil32.Round(result * (PlayObject.m_nPowerRate / 100));
                if (PlayObject.m_boPowerItem)
                {
                    result = HUtil32.Round(PowerItem * result);
                }
            }

            if (AutoChangeColor)
            {
                result = result * AutoChangeIdx + 1;
            }

            if (FixColor)
            {
                result = result * FixColorIdx + 1;
            }

            return (ushort)result;
        }

        /// <summary>
        /// 减少生命值
        /// </summary>
        /// <param name="nDamage"></param>
        private void DamageHealth(int nDamage)
        {
            if (((LastHiter == null) || !LastHiter.UnMagicShield) && MagicShield && (nDamage > 0) &&
                (m_WAbil.MP > 0))
            {
                var nSpdam = HUtil32.Round(nDamage * 1.5);
                if (m_WAbil.MP >= nSpdam)
                {
                    m_WAbil.MP = (ushort)(m_WAbil.MP - nSpdam);
                    nSpdam = 0;
                }
                else
                {
                    nSpdam = nSpdam - m_WAbil.MP;
                    m_WAbil.MP = 0;
                }

                nDamage = HUtil32.Round(nSpdam / 1.5);
                HealthSpellChanged();
            }

            if (nDamage > 0)
            {
                if ((m_WAbil.HP - nDamage) > 0)
                {
                    m_WAbil.HP = (ushort)(m_WAbil.HP - nDamage);
                }
                else
                {
                    m_WAbil.HP = 0;
                }
            }
            else
            {
                if ((m_WAbil.HP - nDamage) < m_WAbil.MaxHP)
                {
                    m_WAbil.HP = (ushort)(m_WAbil.HP - nDamage);
                }
                else
                {
                    m_WAbil.HP = m_WAbil.MaxHP;
                }
            }
        }

        public byte GetBackDir(int nDir)
        {
            byte result = 0;
            switch (nDir)
            {
                case Grobal2.DR_UP:
                    result = Grobal2.DR_DOWN;
                    break;
                case Grobal2.DR_DOWN:
                    result = Grobal2.DR_UP;
                    break;
                case Grobal2.DR_LEFT:
                    result = Grobal2.DR_RIGHT;
                    break;
                case Grobal2.DR_RIGHT:
                    result = Grobal2.DR_LEFT;
                    break;
                case Grobal2.DR_UPLEFT:
                    result = Grobal2.DR_DOWNRIGHT;
                    break;
                case Grobal2.DR_UPRIGHT:
                    result = Grobal2.DR_DOWNLEFT;
                    break;
                case Grobal2.DR_DOWNLEFT:
                    result = Grobal2.DR_UPRIGHT;
                    break;
                case Grobal2.DR_DOWNRIGHT:
                    result = Grobal2.DR_UPLEFT;
                    break;
            }

            return result;
        }

        public int CharPushed(byte nDir, int nPushCount)
        {
            short nx = 0;
            short ny = 0;
            int result = 0;
            byte olddir = Direction;
            int oldx = CurrX;
            int oldy = CurrY;
            Direction = nDir;
            byte nBackDir = GetBackDir(nDir);
            for (var i = 0; i < nPushCount; i++)
            {
                GetFrontPosition(ref nx, ref ny);
                if (Envir.CanWalk(nx, ny, false))
                {
                    if (Envir.MoveToMovingObject(CurrX, CurrY, this, nx, ny, false) > 0)
                    {
                        CurrX = nx;
                        CurrY = ny;
                        SendRefMsg(Grobal2.RM_PUSH, nBackDir, CurrX, CurrY, 0, "");
                        result++;
                        if (Race >= Grobal2.RC_ANIMAL)
                        {
                            WalkTick = WalkTick + 800;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            Direction = nBackDir;
            if (result == 0)
            {
                Direction = olddir;
            }

            return result;
        }

        public int MagPassThroughMagic(short sx, short sy, short tx, short ty, int ndir, int magpwr, bool undeadattack)
        {
            TBaseObject BaseObject;
            int tcount = 0;
            for (int i = 0; i < 12; i++)
            {
                BaseObject = Envir.GetMovingObject(sx, sy, true) as TBaseObject;
                if (BaseObject != null)
                {
                    if (IsProperTarget(BaseObject))
                    {
                        if (M2Share.RandomNumber.Random(10) >= BaseObject.m_nAntiMagic)
                        {
                            if (undeadattack)
                            {
                                magpwr = HUtil32.Round(magpwr * 1.5);
                            }

                            BaseObject.SendDelayMsg(this, Grobal2.RM_MAGSTRUCK, 0, magpwr, 0, 0, "", 600);
                            tcount++;
                        }
                    }
                }

                if (!((Math.Abs(sx - tx) <= 0) && (Math.Abs(sy - ty) <= 0)))
                {
                    ndir = M2Share.GetNextDirection(sx, sy, tx, ty);
                    if (!Envir.GetNextPosition(sx, sy, ndir, 1, ref sx, ref sy))
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            return tcount;
        }

        private void BreakOpenHealth()
        {
            if (m_boShowHP)
            {
                m_boShowHP = false;
                m_nCharStatusEx = m_nCharStatusEx ^ Grobal2.STATE_OPENHEATH;
                CharStatus = GetCharStatus();
                SendRefMsg(Grobal2.RM_CLOSEHEALTH, 0, 0, 0, 0, "");
            }
        }

        private void MakeOpenHealth()
        {
            m_boShowHP = true;
            m_nCharStatusEx = m_nCharStatusEx | Grobal2.STATE_OPENHEATH;
            CharStatus = GetCharStatus();
            SendRefMsg(Grobal2.RM_OPENHEALTH, 0, m_WAbil.HP, m_WAbil.MaxHP, 0, "");
        }

        protected void IncHealthSpell(int nHP, int nMP)
        {
            if ((nHP < 0) || (nMP < 0))
            {
                return;
            }

            if ((m_WAbil.HP + nHP) >= m_WAbil.MaxHP)
            {
                m_WAbil.HP = m_WAbil.MaxHP;
            }
            else
            {
                m_WAbil.HP += (ushort)nHP;
            }

            if ((m_WAbil.MP + nMP) >= m_WAbil.MaxMP)
            {
                m_WAbil.MP = m_WAbil.MaxMP;
            }
            else
            {
                m_WAbil.MP += (ushort)nMP;
            }

            HealthSpellChanged();
        }

        private void ItemDamageRevivalRing()
        {
            StdItem pSItem;
            ushort nDura;
            ushort tDura;
            PlayObject PlayObject;
            for (int i = 0; i < UseItems.Length; i++)
            {
                if (UseItems[i] != null && UseItems[i].wIndex > 0)
                {
                    pSItem = M2Share.UserEngine.GetStdItem(UseItems[i].wIndex);
                    if (pSItem != null)
                    {
                        if (new ArrayList(new byte[] { 114, 160, 161, 162 }).Contains(pSItem.Shape) ||
                            (((i == Grobal2.U_WEAPON) || (i == Grobal2.U_RIGHTHAND)) &&
                             new ArrayList(new byte[] { 114, 160, 161, 162 }).Contains(pSItem.AniCount)))
                        {
                            nDura = UseItems[i].Dura;
                            tDura = (ushort)HUtil32.Round(nDura / 1000.0); // 1.03
                            nDura -= 1000;
                            if (nDura <= 0)
                            {
                                nDura = 0;
                                UseItems[i].Dura = nDura;
                                if (Race == Grobal2.RC_PLAYOBJECT)
                                {
                                    PlayObject = this as PlayObject;
                                    PlayObject.SendDelItems(UseItems[i]);
                                }

                                UseItems[i].wIndex = 0;
                                RecalcAbilitys();
                            }
                            else
                            {
                                UseItems[i].Dura = nDura;
                            }

                            if (tDura != HUtil32.Round(nDura / 1000.0)) // 1.03
                            {
                                SendMsg(this, Grobal2.RM_DURACHANGE, i, nDura, UseItems[i].DuraMax, 0, "");
                            }
                        }
                    }
                }
            }
        }

        public bool GetFrontPosition(ref short nX, ref short nY)
        {
            bool result;
            Envirnoment Envir = this.Envir;
            nX = CurrX;
            nY = CurrY;
            switch (Direction)
            {
                case Grobal2.DR_UP:
                    if (nY > 0)
                    {
                        nY -= 1;
                    }

                    break;
                case Grobal2.DR_UPRIGHT:
                    if ((nX < (Envir.Width - 1)) && (nY > 0))
                    {
                        nX++;
                        nY -= 1;
                    }

                    break;
                case Grobal2.DR_RIGHT:
                    if (nX < (Envir.Width - 1))
                    {
                        nX++;
                    }

                    break;
                case Grobal2.DR_DOWNRIGHT:
                    if ((nX < (Envir.Width - 1)) && (nY < (Envir.Height - 1)))
                    {
                        nX++;
                        nY++;
                    }

                    break;
                case Grobal2.DR_DOWN:
                    if (nY < (Envir.Height - 1))
                    {
                        nY++;
                    }

                    break;
                case Grobal2.DR_DOWNLEFT:
                    if ((nX > 0) && (nY < (Envir.Height - 1)))
                    {
                        nX -= 1;
                        nY++;
                    }

                    break;
                case Grobal2.DR_LEFT:
                    if (nX > 0)
                    {
                        nX -= 1;
                    }

                    break;
                case Grobal2.DR_UPLEFT:
                    if ((nX > 0) && (nY > 0))
                    {
                        nX -= 1;
                        nY -= 1;
                    }

                    break;
            }

            result = true;
            return result;
        }

        private bool SpaceMove_GetRandXY(Envirnoment Envir, ref short nX, ref short nY)
        {
            int n14;
            short n18;
            int n1C;
            bool result = false;
            if (Envir.Width < 80)
            {
                n18 = 3;
            }
            else
            {
                n18 = 10;
            }

            if (Envir.Height < 150)
            {
                if (Envir.Height < 50)
                {
                    n1C = 2;
                }
                else
                {
                    n1C = 15;
                }
            }
            else
            {
                n1C = 50;
            }

            n14 = 0;
            while (true)
            {
                if (Envir.CanWalk(nX, nY, true))
                {
                    result = true;
                    break;
                }

                if (nX < (Envir.Width - n1C - 1))
                {
                    nX += n18;
                }
                else
                {
                    nX = (short)M2Share.RandomNumber.Random(Envir.Width);
                    if (nY < (Envir.Height - n1C - 1))
                    {
                        nY += n18;
                    }
                    else
                    {
                        nY = (short)M2Share.RandomNumber.Random(Envir.Height);
                    }
                }

                n14++;
                if (n14 >= 201)
                {
                    break;
                }
            }

            return result;
        }

        public void SpaceMove(string sMap, short nX, short nY, int nInt)
        {
            int nOldX;
            int nOldY;
            bool bo21;
            PlayObject PlayObject;
            Envirnoment Envir = M2Share.MapManager.FindMap(sMap);
            if (Envir != null)
            {
                if (M2Share.nServerIndex == Envir.ServerIndex)
                {
                    Envirnoment OldEnvir = this.Envir;
                    nOldX = CurrX;
                    nOldY = CurrY;
                    bo21 = false;
                    this.Envir.DeleteFromMap(CurrX, CurrY, CellType.MovingObject, this);
                    VisibleHumanList.Clear();
                    for (int i = 0; i < VisibleItems.Count; i++)
                    {
                        VisibleItems[i] = null;
                    }

                    VisibleItems.Clear();
                    for (int i = 0; i < VisibleActors.Count; i++)
                    {
                        VisibleActors[i] = null;
                    }

                    VisibleActors.Clear();
                    VisibleEvents.Clear();
                    this.Envir = Envir;
                    MapName = Envir.MapName;
                    MapFileName = Envir.MapFileName;
                    CurrX = nX;
                    CurrY = nY;
                    if (SpaceMove_GetRandXY(this.Envir, ref CurrX, ref CurrY))
                    {
                        this.Envir.AddToMap(CurrX, CurrY, CellType.MovingObject, this);
                        SendMsg(this, Grobal2.RM_CLEAROBJECTS, 0, 0, 0, 0, "");
                        SendMsg(this, Grobal2.RM_CHANGEMAP, 0, 0, 0, 0, MapFileName);
                        if (nInt == 1)
                        {
                            SendRefMsg(Grobal2.RM_SPACEMOVE_SHOW2, Direction, CurrX, CurrY, 0, "");
                        }
                        else
                        {
                            SendRefMsg(Grobal2.RM_SPACEMOVE_SHOW, Direction, CurrX, CurrY, 0, "");
                        }

                        MapMoveTick = HUtil32.GetTickCount();
                        m_bo316 = true;
                        bo21 = true;
                    }

                    if (!bo21)
                    {
                        this.Envir = OldEnvir;
                        CurrX = (short)nOldX;
                        CurrY = (short)nOldY;
                        this.Envir.AddToMap(CurrX, CurrY, CellType.MovingObject, this);
                    }

                    OnEnvirnomentChanged();
                }
                else
                {
                    if (SpaceMove_GetRandXY(Envir, ref nX, ref nY))
                    {
                        if (Race == Grobal2.RC_PLAYOBJECT)
                        {
                            DisappearA();
                            m_bo316 = true;
                            PlayObject = this as PlayObject;
                            PlayObject.m_sSwitchMapName = Envir.MapName;
                            PlayObject.m_nSwitchMapX = nX;
                            PlayObject.m_nSwitchMapY = nY;
                            PlayObject.m_boSwitchData = true;
                            PlayObject.m_nServerIndex = Envir.ServerIndex;
                            PlayObject.m_boEmergencyClose = true;
                            PlayObject.m_boReconnection = true;
                        }
                        else
                        {
                            KickException();
                        }
                    }
                }
            }
        }

        public void RefShowName()
        {
            SendRefMsg(Grobal2.RM_USERNAME, 0, 0, 0, 0, GetShowName());
        }

        public TBaseObject MakeSlave(string sMonName, int nMakeLevel, int nExpLevel, int nMaxMob, int dwRoyaltySec)
        {
            short nX = 0;
            short nY = 0;
            TBaseObject result = null;
            if (SlaveList.Count < nMaxMob)
            {
                GetFrontPosition(ref nX, ref nY);
                var MonObj = M2Share.UserEngine.RegenMonsterByName(Envir.MapName, nX, nY, sMonName);
                if (MonObj != null)
                {
                    MonObj.Master = this;
                    MonObj.MasterRoyaltyTick = HUtil32.GetTickCount() + (dwRoyaltySec * 1000);
                    MonObj.SlaveMakeLevel = (byte)nMakeLevel;
                    MonObj.SlaveExpLevel = (byte)nExpLevel;
                    MonObj.RecalcAbilitys();
                    if (MonObj.m_WAbil.HP < MonObj.m_WAbil.MaxHP)
                    {
                        MonObj.m_WAbil.HP =
                            (ushort)(MonObj.m_WAbil.HP + (MonObj.m_WAbil.MaxHP - MonObj.m_WAbil.HP) / 2);
                    }

                    MonObj.RefNameColor();
                    SlaveList.Add(MonObj);
                    result = MonObj;
                }
            }

            return result;
        }

        /// <summary>
        /// 地图随机移动
        /// </summary>
        /// <param name="sMapName"></param>
        /// <param name="nInt"></param>
        public void MapRandomMove(string sMapName, int nInt)
        {
            int nEgdey;
            Envirnoment Envir = M2Share.MapManager.FindMap(sMapName);
            if (Envir != null)
            {
                if (Envir.Height < 150)
                {
                    if (Envir.Height < 30)
                    {
                        nEgdey = 2;
                    }
                    else
                    {
                        nEgdey = 20;
                    }
                }
                else
                {
                    nEgdey = 50;
                }

                short nX = (short)(M2Share.RandomNumber.Random(Envir.Width - nEgdey - 1) + nEgdey);
                short nY = (short)(M2Share.RandomNumber.Random(Envir.Height - nEgdey - 1) + nEgdey);
                SpaceMove(sMapName, nX, nY, nInt);
            }
        }

        public bool AddItemToBag(TUserItem UserItem)
        {
            bool result = false;
            if (ItemList.Count < Grobal2.MAXBAGITEM)
            {
                ItemList.Add(UserItem);
                WeightChanged();
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 检查心灵启示
        /// </summary>
        /// <param name="Magic"></param>
        protected void CheckSeeHealGauge(TUserMagic Magic)
        {
            if (Magic.MagicInfo.wMagicID == 28)
            {
                if (Magic.btLevel >= 2)
                {
                    AbilSeeHealGauge = true;
                }
            }
        }

        public int GetQuestFalgStatus(int nFlag)
        {
            int result = 0;
            nFlag -= 1;
            if (nFlag < 0)
            {
                return result;
            }

            int n10 = nFlag / 8;
            int n14 = nFlag % 8;
            if ((n10 - m_QuestFlag.Length) < 0)
            {
                if (((128 >> n14) & m_QuestFlag[n10]) != 0)
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

            int n10 = nFlag / 8;
            int n14 = nFlag % 8;
            if ((n10 - m_QuestFlag.Length) < 0)
            {
                byte bt15 = m_QuestFlag[n10];
                if (nValue == 0)
                {
                    m_QuestFlag[n10] = (byte)((~(128 >> n14)) & bt15);
                }
                else
                {
                    m_QuestFlag[n10] = (byte)((128 >> n14) | bt15);
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
            if ((n10 - m_QuestUnitOpen.Length) < 0)
            {
                if (((128 >> n14) & m_QuestUnitOpen[n10]) != 0)
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
            if ((n10 - m_QuestUnitOpen.Length) < 0)
            {
                var bt15 = m_QuestUnitOpen[n10];
                if (nValue == 0)
                {
                    m_QuestUnitOpen[n10] = (byte)((~(128 >> n14)) & bt15);
                }
                else
                {
                    m_QuestUnitOpen[n10] = (byte)((128 >> n14) | bt15);
                }
            }
        }

        public int GetQuestUnitStatus(int nFlag)
        {
            int result = 0;
            nFlag -= 1;
            if (nFlag < 0)
            {
                return result;
            }

            int n10 = nFlag / 8;
            int n14 = nFlag % 8;
            if ((n10 - m_QuestUnit.Length) < 0)
            {
                if (((128 >> n14) & m_QuestUnit[n10]) != 0)
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
            if ((n10 - m_QuestUnit.Length) < 0)
            {
                var bt15 = m_QuestUnit[n10];
                if (nValue == 0)
                {
                    m_QuestUnit[n10] = (byte)((~(128 >> n14)) & bt15);
                }
                else
                {
                    m_QuestUnit[n10] = (byte)((128 >> n14) | bt15);
                }
            }
        }

        private bool KillFunc()
        {
            const string sExceptionMsg = "[Exception] TBaseObject::KillFunc";
            bool result = false;
            try
            {
                if ((M2Share.g_FunctionNPC != null) && (Envir != null) && Envir.Flag.boKILLFUNC)
                {
                    if (Race != Grobal2.RC_PLAYOBJECT)
                    {
                        if (ExpHitter != null)
                        {
                            if (ExpHitter.Race == Grobal2.RC_PLAYOBJECT)
                            {
                                M2Share.g_FunctionNPC.GotoLable(ExpHitter as PlayObject,
                                    "@KillPlayMon" + Envir.Flag.nKILLFUNCNO, false);
                            }

                            if (ExpHitter.Master != null)
                            {
                                M2Share.g_FunctionNPC.GotoLable(ExpHitter.Master as PlayObject,
                                    "@KillPlayMon" + Envir.Flag.nKILLFUNCNO, false);
                            }
                        }
                        else
                        {
                            if (LastHiter != null)
                            {
                                if (LastHiter.Race == Grobal2.RC_PLAYOBJECT)
                                {
                                    M2Share.g_FunctionNPC.GotoLable(LastHiter as PlayObject,
                                        "@KillPlayMon" + Envir.Flag.nKILLFUNCNO, false);
                                }

                                if (LastHiter.Master != null)
                                {
                                    M2Share.g_FunctionNPC.GotoLable(LastHiter.Master as PlayObject,
                                        "@KillPlayMon" + Envir.Flag.nKILLFUNCNO, false);
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((LastHiter != null) && (LastHiter.Race == Grobal2.RC_PLAYOBJECT))
                        {
                            M2Share.g_FunctionNPC.GotoLable(LastHiter as PlayObject,
                                "@KillPlay" + Envir.Flag.nKILLFUNCNO, false);
                        }
                    }

                    result = true;
                }
            }
            catch (Exception e)
            {
                M2Share.LogSystem.Error(sExceptionMsg);
                M2Share.LogSystem.Error(e.Message);
            }

            return result;
        }

        /// <summary>
        /// 蜡烛勋章减少持久
        /// </summary>
        private void UseLamp()
        {
            const string sExceptionMsg = "[Exception] TBaseObject::UseLamp";
            try
            {
                if (UseItems[Grobal2.U_RIGHTHAND] != null && UseItems[Grobal2.U_RIGHTHAND].wIndex > 0)
                {
                    var stdItem = M2Share.UserEngine.GetStdItem(UseItems[Grobal2.U_RIGHTHAND].wIndex);
                    if ((stdItem == null) || (stdItem.Source != 0))
                    {
                        return;
                    }

                    var nOldDura = HUtil32.Round(UseItems[Grobal2.U_RIGHTHAND].Dura / 1000);
                    int nDura = 0;
                    if (M2Share.g_Config.boDecLampDura)
                    {
                        nDura = UseItems[Grobal2.U_RIGHTHAND].Dura - 1;
                    }
                    else
                    {
                        nDura = UseItems[Grobal2.U_RIGHTHAND].Dura;
                    }

                    if (nDura <= 0)
                    {
                        UseItems[Grobal2.U_RIGHTHAND].Dura = 0;
                        if (Race == Grobal2.RC_PLAYOBJECT)
                        {
                            var PlayObject = this as PlayObject;
                            PlayObject.SendDelItems(UseItems[Grobal2.U_RIGHTHAND]);
                        }

                        UseItems[Grobal2.U_RIGHTHAND].wIndex = 0;
                        Light = 0;
                        SendRefMsg(Grobal2.RM_CHANGELIGHT, 0, 0, 0, 0, "");
                        SendMsg(this, Grobal2.RM_LAMPCHANGEDURA, 0, UseItems[Grobal2.U_RIGHTHAND].Dura, 0, 0, "");
                        RecalcAbilitys();
                    }
                    else
                    {
                        UseItems[Grobal2.U_RIGHTHAND].Dura = (ushort)nDura;
                    }

                    if (nOldDura != HUtil32.Round(nDura / 1000))
                    {
                        SendMsg(this, Grobal2.RM_LAMPCHANGEDURA, 0, UseItems[Grobal2.U_RIGHTHAND].Dura, 0, 0, "");
                    }
                }
            }
            catch
            {
                M2Share.LogSystem.Error(sExceptionMsg);
            }
        }

        public TBaseObject GetPoseCreate()
        {
            short nX = 0;
            short nY = 0;
            TBaseObject result = null;
            if (GetFrontPosition(ref nX, ref nY))
            {
                result = (TBaseObject)Envir.GetMovingObject(nX, nY, true);
            }

            return result;
        }

        public bool GetAttackDir(TBaseObject BaseObject, ref byte btDir)
        {
            bool result = false;
            if ((CurrX - 1 <= BaseObject.CurrX) && (CurrX + 1 >= BaseObject.CurrX) &&
                (CurrY - 1 <= BaseObject.CurrY) && (CurrY + 1 >= BaseObject.CurrY) &&
                ((CurrX != BaseObject.CurrX) || (CurrY != BaseObject.CurrY)))
            {
                result = true;
                if (((CurrX - 1) == BaseObject.CurrX) && (CurrY == BaseObject.CurrY))
                {
                    btDir = Grobal2.DR_LEFT;
                    return result;
                }

                if (((CurrX + 1) == BaseObject.CurrX) && (CurrY == BaseObject.CurrY))
                {
                    btDir = Grobal2.DR_RIGHT;
                    return result;
                }

                if ((CurrX == BaseObject.CurrX) && ((CurrY - 1) == BaseObject.CurrY))
                {
                    btDir = Grobal2.DR_UP;
                    return result;
                }

                if ((CurrX == BaseObject.CurrX) && ((CurrY + 1) == BaseObject.CurrY))
                {
                    btDir = Grobal2.DR_DOWN;
                    return result;
                }

                if (((CurrX - 1) == BaseObject.CurrX) && ((CurrY - 1) == BaseObject.CurrY))
                {
                    btDir = Grobal2.DR_UPLEFT;
                    return result;
                }

                if (((CurrX + 1) == BaseObject.CurrX) && ((CurrY - 1) == BaseObject.CurrY))
                {
                    btDir = Grobal2.DR_UPRIGHT;
                    return result;
                }

                if (((CurrX - 1) == BaseObject.CurrX) && ((CurrY + 1) == BaseObject.CurrY))
                {
                    btDir = Grobal2.DR_DOWNLEFT;
                    return result;
                }

                if (((CurrX + 1) == BaseObject.CurrX) && ((CurrY + 1) == BaseObject.CurrY))
                {
                    btDir = Grobal2.DR_DOWNRIGHT;
                    return result;
                }

                btDir = 0;
            }

            return result;
        }

        public bool GetAttackDir(TBaseObject BaseObject, int nRange, ref byte btDir)
        {
            short nX = 0;
            short nY = 0;
            btDir = M2Share.GetNextDirection(CurrX, CurrY, BaseObject.CurrX, BaseObject.CurrY);
            if (Envir.GetNextPosition(CurrX, CurrY, btDir, nRange, ref nX, ref nY))
            {
                return BaseObject == (TBaseObject)Envir.GetMovingObject(nX, nY, true);
            }

            return false;
        }

        public bool TargetInSpitRange(TBaseObject BaseObject, ref byte btDir)
        {
            bool result = false;
            if ((Math.Abs(BaseObject.CurrX - CurrX) <= 2) && (Math.Abs(BaseObject.CurrY - CurrY) <= 2))
            {
                int nX = BaseObject.CurrX - CurrX;
                int nY = BaseObject.CurrY - CurrY;
                if ((Math.Abs(nX) <= 1) && (Math.Abs(nY) <= 1))
                {
                    GetAttackDir(BaseObject, ref btDir);
                    result = true;
                    return result;
                }

                nX += 2;
                nY += 2;
                if ((nX >= 0) && (nX <= 4) && (nY >= 0) && (nY <= 4))
                {
                    btDir = M2Share.GetNextDirection(CurrX, CurrY, BaseObject.CurrX, BaseObject.CurrY);
                    if (M2Share.g_Config.SpitMap[btDir, nY, nX] == 1)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 计算包裹物品重量
        /// </summary>
        /// <returns></returns>
        protected ushort RecalcBagWeight()
        {
            ushort result = 0;
            TUserItem UserItem;
            StdItem StdItem;
            for (int i = 0; i < ItemList.Count; i++)
            {
                UserItem = ItemList[i];
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (StdItem != null)
                {
                    result += StdItem.Weight;
                }
            }

            return result;
        }

        /// <summary>
        /// 计算攻击速度
        /// </summary>
        private void RecalcHitSpeed()
        {
            TUserMagic UserMagic;
            TNakedAbility BonusTick = null;
            switch (Job)
            {
                case PlayJob.Warrior:
                    BonusTick = M2Share.g_Config.BonusAbilofWarr;
                    break;
                case PlayJob.Wizard:
                    BonusTick = M2Share.g_Config.BonusAbilofWizard;
                    break;
                case PlayJob.Taoist:
                    BonusTick = M2Share.g_Config.BonusAbilofTaos;
                    break;
            }

            m_btHitPoint = (byte)(M2Share.DEFHIT + BonusAbil.Hit / BonusTick.Hit);
            switch (Job)
            {
                case PlayJob.Taoist:
                    SpeedPoint = (byte)(M2Share.DEFSPEED + BonusAbil.Speed / BonusTick.Speed + 3);
                    break;
                default:
                    SpeedPoint = (byte)(M2Share.DEFSPEED + BonusAbil.Speed / BonusTick.Speed);
                    break;
            }

            m_nHitPlus = 0;
            m_nHitDouble = 0;
            for (int i = 0; i < MagicList.Count; i++)
            {
                UserMagic = MagicList[i];
                MagicArr[UserMagic.wMagIdx] = UserMagic;
                switch (UserMagic.wMagIdx)
                {
                    case SpellsDef.SKILL_ONESWORD: // 基本剑法
                        if (UserMagic.btLevel > 0)
                        {
                            m_btHitPoint = (byte)(m_btHitPoint + HUtil32.Round(9 / 3 * UserMagic.btLevel));
                        }

                        break;
                    case SpellsDef.SKILL_ILKWANG: // 精神力战法
                        if (UserMagic.btLevel > 0)
                        {
                            m_btHitPoint = (byte)(m_btHitPoint + HUtil32.Round(8 / 3 * UserMagic.btLevel));
                        }

                        break;
                    case SpellsDef.SKILL_YEDO: // 攻杀剑法
                        if (UserMagic.btLevel > 0)
                        {
                            m_btHitPoint = (byte)(m_btHitPoint + HUtil32.Round(3 / 3 * UserMagic.btLevel));
                        }

                        m_nHitPlus = (byte)(M2Share.DEFHIT + UserMagic.btLevel);
                        AttackSkillCount = (byte)(7 - UserMagic.btLevel);
                        AttackSkillPointCount = M2Share.RandomNumber.RandomByte(AttackSkillCount);
                        break;
                    case SpellsDef.SKILL_FIRESWORD: // 烈火剑法
                        m_nHitDouble = (byte)(4 + UserMagic.btLevel * 4);
                        break;
                }
            }
        }

        public void AddItemSkill(int nIndex)
        {
            TMagic Magic = null;
            switch (nIndex)
            {
                case 1:
                    Magic = M2Share.UserEngine.FindMagic(M2Share.g_Config.sFireBallSkill);
                    break;
                case 2:
                    Magic = M2Share.UserEngine.FindMagic(M2Share.g_Config.sHealSkill);
                    break;
            }

            if (Magic != null)
            {
                if (!IsTrainingSkill(Magic.wMagicID))
                {
                    var UserMagic = new TUserMagic
                    {
                        MagicInfo = Magic,
                        wMagIdx = Magic.wMagicID,
                        btKey = 0,
                        btLevel = 1,
                        nTranPoint = 0
                    };
                    MagicList.Add(UserMagic);
                    if (Race == Grobal2.RC_PLAYOBJECT)
                    {
                        (this as PlayObject).SendAddMagic(UserMagic);
                    }
                }
            }
        }

        private bool AddToMap()
        {
            bool result;
            object Point = Envir.AddToMap(CurrX, CurrY, CellType.MovingObject, this);
            if (Point != null)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            if (!FixedHideMode)
            {
                SendRefMsg(Grobal2.RM_TURN, Direction, CurrX, CurrY, 0, "");
            }

            return result;
        }

        /// <summary>
        /// 计算施法魔法值
        /// </summary>
        /// <param name="UserMagic"></param>
        /// <returns></returns>
        public short GetMagicSpell(TUserMagic UserMagic)
        {
            return (short)HUtil32.Round(UserMagic.MagicInfo.wSpell / (UserMagic.MagicInfo.btTrainLv + 1) *
                                        (UserMagic.btLevel + 1));
        }

        private void CheckPKStatus()
        {
            if (m_boPKFlag && ((HUtil32.GetTickCount() - m_dwPKTick) > M2Share.g_Config.dwPKFlagTime)) // 60 * 1000
            {
                m_boPKFlag = false;
                RefNameColor();
            }
        }

        /// <summary>
        /// 减少魔法值
        /// </summary>
        /// <param name="nSpellPoint"></param>
        public void DamageSpell(ushort nSpellPoint)
        {
            if (nSpellPoint > 0)
            {
                if ((m_WAbil.MP - nSpellPoint) > 0)
                {
                    m_WAbil.MP -= nSpellPoint;
                }
                else
                {
                    m_WAbil.MP = 0;
                }
            }
            else
            {
                if ((m_WAbil.MP - nSpellPoint) < m_WAbil.MaxMP)
                {
                    m_WAbil.MP -= nSpellPoint;
                }
                else
                {
                    m_WAbil.MP = m_WAbil.MaxMP;
                }
            }
        }

        public void DelItemSkill_DeleteSkill(string sSkillName)
        {
            TUserMagic UserMagic;
            PlayObject PlayObject;
            for (int i = 0; i < MagicList.Count; i++)
            {
                UserMagic = MagicList[i];
                if (UserMagic.MagicInfo.sMagicName == sSkillName)
                {
                    PlayObject = this as PlayObject;
                    PlayObject.SendDelMagic(UserMagic);
                    UserMagic = null;
                    MagicList.RemoveAt(i);
                    break;
                }
            }
        }

        public void DelItemSkill(int nIndex)
        {
            if (Race != Grobal2.RC_PLAYOBJECT)
            {
                return;
            }

            switch (nIndex)
            {
                case 1:
                    if (Job != PlayJob.Wizard)
                    {
                        DelItemSkill_DeleteSkill(M2Share.g_Config.sFireBallSkill);
                    }

                    break;
                case 2:
                    if (Job != PlayJob.Taoist)
                    {
                        DelItemSkill_DeleteSkill(M2Share.g_Config.sHealSkill);
                    }

                    break;
            }
        }

        public void DelMember(TBaseObject BaseObject)
        {
            PlayObject PlayObject;
            if (m_GroupOwner != BaseObject)
            {
                for (int i = 0; i < GroupMembers.Count; i++)
                {
                    if (GroupMembers[i] == BaseObject)
                    {
                        BaseObject.LeaveGroup();
                        GroupMembers.RemoveAt(i);
                        break;
                    }
                }
            }
            else
            {
                for (int i = GroupMembers.Count - 1; i >= 0; i--)
                {
                    GroupMembers[i].LeaveGroup();
                    GroupMembers.RemoveAt(i);
                }
            }

            PlayObject = this as PlayObject;
            if (!PlayObject.CancelGroup())
            {
                PlayObject.SendDefMessage(Grobal2.SM_GROUPCANCEL, 0, 0, 0, 0, "");
            }
            else
            {
                PlayObject.SendGroupMembers();
            }
        }

        public void DoDamageWeapon(int nWeaponDamage)
        {
            if (UseItems[Grobal2.U_WEAPON] == null || UseItems[Grobal2.U_WEAPON].wIndex <= 0)
            {
                return;
            }

            int nDura = UseItems[Grobal2.U_WEAPON].Dura;
            var nDuraPoint = HUtil32.Round(nDura / 1.03);
            nDura -= nWeaponDamage;
            if (nDura <= 0)
            {
                nDura = 0;
                UseItems[Grobal2.U_WEAPON].Dura = (ushort)nDura;
                if (Race == Grobal2.RC_PLAYOBJECT)
                {
                    var PlayObject = this as PlayObject;
                    PlayObject.SendDelItems(UseItems[Grobal2.U_WEAPON]);
                    var StdItem = M2Share.UserEngine.GetStdItem(UseItems[Grobal2.U_WEAPON].wIndex);
                    if (StdItem.NeedIdentify == 1)
                    {
                        M2Share.AddGameDataLog('3' + "\t" + MapName + "\t" + CurrX + "\t" + CurrY + "\t" +
                                               CharName + "\t" + StdItem.Name + "\t" +
                                               UseItems[Grobal2.U_WEAPON].MakeIndex + "\t" +
                                               HUtil32.BoolToIntStr(Race == Grobal2.RC_PLAYOBJECT) + "\t" +
                                               '0');
                    }
                }

                UseItems[Grobal2.U_WEAPON].wIndex = 0;
                SendMsg(this, Grobal2.RM_DURACHANGE, Grobal2.U_WEAPON, nDura, UseItems[Grobal2.U_WEAPON].DuraMax, 0,
                    "");
            }
            else
            {
                UseItems[Grobal2.U_WEAPON].Dura = (ushort)nDura;
            }

            if ((nDura / 1.03) != nDuraPoint)
            {
                SendMsg(this, Grobal2.RM_DURACHANGE, Grobal2.U_WEAPON, UseItems[Grobal2.U_WEAPON].Dura,
                    UseItems[Grobal2.U_WEAPON].DuraMax, 0, "");
            }
        }

        protected byte GetCharColor(TBaseObject BaseObject)
        {
            TUserCastle Castle;
            byte result = BaseObject.GetNamecolor();
            if (BaseObject.Race == Grobal2.RC_PLAYOBJECT)
            {
                if (BaseObject.PKLevel() < 2)
                {
                    if (BaseObject.m_boPKFlag)
                    {
                        result = M2Share.g_Config.btPKFlagNameColor;
                    }

                    var n10 = GetGuildRelation(this, BaseObject);
                    switch (n10)
                    {
                        case 1:
                        case 3:
                            result = M2Share.g_Config.btAllyAndGuildNameColor;
                            break;
                        case 2:
                            result = M2Share.g_Config.btWarGuildNameColor;
                            break;
                    }

                    if (BaseObject.Envir.Flag.boFight3Zone)
                    {
                        if (MyGuild == BaseObject.MyGuild)
                        {
                            result = M2Share.g_Config.btAllyAndGuildNameColor;
                        }
                        else
                        {
                            result = M2Share.g_Config.btWarGuildNameColor;
                        }
                    }
                }

                Castle = M2Share.CastleManager.InCastleWarArea(BaseObject);
                if ((Castle != null) && Castle.m_boUnderWar && InFreePKArea && BaseObject.InFreePKArea)
                {
                    result = M2Share.g_Config.btInFreePKAreaNameColor;
                    GuildWarArea = true;
                    if (MyGuild == null)
                    {
                        return result;
                    }

                    if (Castle.IsMasterGuild(MyGuild))
                    {
                        if ((MyGuild == BaseObject.MyGuild) || MyGuild.IsAllyGuild(BaseObject.MyGuild))
                        {
                            result = M2Share.g_Config.btAllyAndGuildNameColor;
                        }
                        else
                        {
                            if (Castle.IsAttackGuild(BaseObject.MyGuild))
                            {
                                result = M2Share.g_Config.btWarGuildNameColor;
                            }
                        }
                    }
                    else
                    {
                        if (Castle.IsAttackGuild(MyGuild))
                        {
                            if ((MyGuild == BaseObject.MyGuild) || MyGuild.IsAllyGuild(BaseObject.MyGuild))
                            {
                                result = M2Share.g_Config.btAllyAndGuildNameColor;
                            }
                            else
                            {
                                if (Castle.IsMember(BaseObject))
                                {
                                    result = M2Share.g_Config.btWarGuildNameColor;
                                }
                            }
                        }
                    }
                }
            }
            else if (BaseObject.Race == Grobal2.RC_NPC) //增加NPC名字颜色单独控制
            {
                result = M2Share.g_Config.NpcNameColor;
                if (BaseObject.m_boCrazyMode) //疯狂模式(红名)
                {
                    result = 0xF9;
                }

                if (BaseObject.m_boHolySeize) //不能走动模式(困魔咒)
                {
                    result = 0x7D;
                }
            }
            else
            {
                if (BaseObject.SlaveExpLevel <= Grobal2.SlaveMaxLevel)
                {
                    result = M2Share.g_Config.SlaveColor[BaseObject.SlaveExpLevel];
                }
                else
                {
                    result = 255;
                }

                if (BaseObject.m_boCrazyMode)
                {
                    result = 0xF9;
                }

                if (BaseObject.m_boHolySeize)
                {
                    result = 0x7D;
                }
            }

            return result;
        }

        public int GetLevelExp(int nLevel)
        {
            int result;
            if (nLevel <= Grobal2.MaxLevel)
            {
                result = M2Share.g_Config.dwNeedExps[nLevel];
            }
            else
            {
                result = M2Share.g_Config.dwNeedExps[M2Share.g_Config.dwNeedExps.Length];
            }

            return result;
        }

        private byte GetNamecolor()
        {
            byte result = NameColor;
            if (PKLevel() == 1)
            {
                result = M2Share.g_Config.btPKLevel1NameColor;
            }

            if (PKLevel() >= 2)
            {
                result = M2Share.g_Config.btPKLevel2NameColor;
            }

            return result;
        }

        public void HearMsg(string sMsg)
        {
            if (!string.IsNullOrEmpty(sMsg))
            {
                SendMsg(null, Grobal2.RM_HEAR, 0, M2Share.g_Config.btHearMsgFColor, M2Share.g_Config.btHearMsgBColor, 0,
                    sMsg);
            }
        }

        protected bool InSafeArea()
        {
            if (Envir == null)
            {
                return false;
            }
            bool result = Envir.Flag.boSAFE;
            if (result)
            {
                return true;
            }
            for (var i = 0; i < M2Share.StartPointList.Count; i++)
            {
                if (M2Share.StartPointList[i].m_sMapName == Envir.MapName)
                {
                    if (M2Share.StartPointList[i] != null)
                    {
                        int cX = M2Share.StartPointList[i].m_nCurrX;
                        int cY = M2Share.StartPointList[i].m_nCurrY;
                        if ((Math.Abs(CurrX - cX) <= 60) && (Math.Abs(CurrY - cY) <= 60))
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        private void MonsterRecalcAbilitys()
        {
            m_WAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.DC), HUtil32.HiWord(Abil.DC));
            int n8 = 0;
            if ((Race == MonsterConst.MONSTER_WHITESKELETON) ||
                (Race == MonsterConst.MONSTER_ELFMONSTER) ||
                (Race == MonsterConst.MONSTER_ELFWARRIOR))
            {
                m_WAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.DC),
                    HUtil32.Round((SlaveExpLevel * 0.1 + 0.3) * 3.0 * SlaveExpLevel + HUtil32.HiWord(m_WAbil.DC)));
                n8 = n8 + HUtil32.Round((SlaveExpLevel * 0.1 + 0.3) * Abil.MaxHP) * SlaveExpLevel;
                n8 = n8 + Abil.MaxHP;
                if (SlaveExpLevel > 0)
                {
                    m_WAbil.MaxHP = (ushort)n8;
                }
                else
                {
                    m_WAbil.MaxHP = Abil.MaxHP;
                }
            }
            else
            {
                n8 = Abil.MaxHP;
                m_WAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.DC),
                    HUtil32.Round(SlaveExpLevel * 2 + HUtil32.HiWord(m_WAbil.DC)));
                n8 = n8 + HUtil32.Round(Abil.MaxHP * 0.15) * SlaveExpLevel;
                m_WAbil.MaxHP = (ushort)HUtil32._MIN(HUtil32.Round(Abil.MaxHP + SlaveExpLevel * 60), n8);
            }
        }

        public void SendFirstMsg(TBaseObject BaseObject, short wIdent, short wParam, int lParam1, int lParam2,
            int lParam3, string sMsg)
        {
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                if (!Ghost)
                {
                    var SendMessage = new SendMessage
                    {
                        wIdent = wIdent,
                        wParam = wParam,
                        nParam1 = lParam1,
                        nParam2 = lParam2,
                        nParam3 = lParam3,
                        DeliveryTime = 0,
                        BaseObject = BaseObject.ObjectId
                    };
                    if (!string.IsNullOrEmpty(sMsg))
                    {
                        SendMessage.Buff = sMsg;
                    }

                    MsgList.Insert(0, SendMessage);
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
        }

        public void SendMsg(TBaseObject BaseObject, int wIdent, int wParam, int nParam1, int nParam2, int nParam3,
            string sMsg)
        {
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                var sendMessage = new SendMessage
                {
                    wIdent = wIdent,
                    wParam = wParam,
                    nParam1 = nParam1,
                    nParam2 = nParam2,
                    nParam3 = nParam3,
                    DeliveryTime = 0,
                    BaseObject = BaseObject.ObjectId,
                    LateDelivery = false
                };
                if (!Ghost)
                {
                    if (!string.IsNullOrEmpty(sMsg))
                    {
                        sendMessage.Buff = sMsg;
                    }

                    MsgList.Add(sendMessage);
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
        }

        /// <summary>
        /// 发送延时消息
        /// </summary>
        public void SendDelayMsg(TBaseObject BaseObject, int wIdent, int wParam, int lParam1, int lParam2, int lParam3,
            string sMsg, int dwDelay)
        {
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                if (!Ghost)
                {
                    var sendMessage = new SendMessage
                    {
                        wIdent = wIdent,
                        wParam = wParam,
                        nParam1 = lParam1,
                        nParam2 = lParam2,
                        nParam3 = lParam3,
                        DeliveryTime = HUtil32.GetTickCount() + dwDelay,
                        BaseObject = BaseObject.ObjectId,
                        LateDelivery = true
                    };
                    if (!string.IsNullOrEmpty(sMsg))
                    {
                        sendMessage.Buff = sMsg;
                    }

                    MsgList.Add(sendMessage);
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
        }

        /// <summary>
        /// 发送延时消息
        /// </summary>
        public void SendDelayMsg(int BaseObject, short wIdent, int wParam, int lParam1, int lParam2, int lParam3,
            string sMsg, int dwDelay)
        {
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                if (!Ghost)
                {
                    var SendMessage = new SendMessage
                    {
                        wIdent = wIdent,
                        wParam = wParam,
                        nParam1 = lParam1,
                        nParam2 = lParam2,
                        nParam3 = lParam3,
                        DeliveryTime = HUtil32.GetTickCount() + dwDelay,
                        LateDelivery = true
                    };
                    if (BaseObject == Grobal2.RM_STRUCK)
                    {
                        SendMessage.BaseObject = Grobal2.RM_STRUCK;
                    }
                    else
                    {
                        SendMessage.BaseObject = BaseObject;
                    }

                    if (!string.IsNullOrEmpty(sMsg))
                    {
                        SendMessage.Buff = sMsg;
                    }

                    MsgList.Add(SendMessage);
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
        }

        private void SendUpdateDelayMsg(TBaseObject BaseObject, short wIdent, short wParam, int lParam1, int lParam2,
            int lParam3, string sMsg, int dwDelay)
        {
            int i;
            HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            try
            {
                i = 0;
                while (true)
                {
                    if (MsgList.Count <= i)
                    {
                        break;
                    }

                    var SendMessage = MsgList[i];
                    if ((SendMessage.wIdent == wIdent) && (SendMessage.nParam1 == lParam1))
                    {
                        MsgList.RemoveAt(i);
                        Dispose(SendMessage);
                        continue;
                    }

                    i++;
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }

            SendDelayMsg(BaseObject.ObjectId, wIdent, wParam, lParam1, lParam2, lParam3, sMsg, dwDelay);
        }

        public void SendUpdateMsg(TBaseObject BaseObject, int wIdent, int wParam, int lParam1, int lParam2, int lParam3,
            string sMsg)
        {
            int i;
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                i = 0;
                while (true)
                {
                    if (MsgList.Count <= i)
                    {
                        break;
                    }

                    var SendMessage = MsgList[i];
                    if (SendMessage.wIdent == wIdent)
                    {
                        MsgList.RemoveAt(i);
                        Dispose(SendMessage);
                        continue;
                    }

                    i++;
                }
            }
            finally
            {

                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }

            SendMsg(BaseObject, wIdent, wParam, lParam1, lParam2, lParam3, sMsg);
        }

        public void SendActionMsg(TBaseObject BaseObject, int wIdent, int wParam, int lParam1, int lParam2, int lParam3,
            string sMsg)
        {
            int i;
            HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            try
            {
                i = 0;
                while (true)
                {
                    if (MsgList.Count <= i)
                    {
                        break;
                    }

                    var SendMessage = MsgList[i];
                    if ((SendMessage.wIdent == Grobal2.CM_TURN) || (SendMessage.wIdent == Grobal2.CM_WALK) ||
                        (SendMessage.wIdent == Grobal2.CM_SITDOWN) || (SendMessage.wIdent == Grobal2.CM_HORSERUN) ||
                        (SendMessage.wIdent == Grobal2.CM_RUN) || (SendMessage.wIdent == Grobal2.CM_HIT) ||
                        (SendMessage.wIdent == Grobal2.CM_HEAVYHIT) || (SendMessage.wIdent == Grobal2.CM_BIGHIT) ||
                        (SendMessage.wIdent == Grobal2.CM_POWERHIT) || (SendMessage.wIdent == Grobal2.CM_LONGHIT) ||
                        (SendMessage.wIdent == Grobal2.CM_WIDEHIT) || (SendMessage.wIdent == Grobal2.CM_FIREHIT))
                    {
                        MsgList.RemoveAt(i);
                        Dispose(SendMessage);
                        continue;
                    }

                    i++;
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }

            SendMsg(BaseObject, wIdent, wParam, lParam1, lParam2, lParam3, sMsg);
        }

        protected virtual bool GetMessage(ref TProcessMessage Msg)
        {
            bool result = false;
            int I;
            HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            try
            {
                I = 0;
                while (MsgList.Count > I)
                {
                    if (MsgList.Count <= I)
                    {
                        break;
                    }

                    var sendMessage = MsgList[I];
                    if ((sendMessage.DeliveryTime != 0) && (HUtil32.GetTickCount() < sendMessage.DeliveryTime)) //延时消息
                    {
                        I++;
                        continue;
                    }

                    MsgList.RemoveAt(I);
                    Msg = new TProcessMessage();
                    Msg.wIdent = sendMessage.wIdent;
                    Msg.wParam = sendMessage.wParam;
                    Msg.nParam1 = sendMessage.nParam1;
                    Msg.nParam2 = sendMessage.nParam2;
                    Msg.nParam3 = sendMessage.nParam3;
                    if (sendMessage.BaseObject > 0)
                    {
                        Msg.BaseObject = sendMessage.BaseObject;
                    }

                    Msg.dwDeliveryTime = sendMessage.DeliveryTime;
                    Msg.boLateDelivery = sendMessage.LateDelivery;
                    Msg.sMsg = !string.IsNullOrEmpty(sendMessage.Buff) ? sendMessage.Buff : string.Empty;
                    result = true;
                    break;
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }

            return result;
        }

        public bool GetMapBaseObjects(Envirnoment tEnvir, int nX, int nY, int nRage, IList<TBaseObject> rList)
        {
            MapCellInfo cellInfo;
            CellObject OSObject;
            TBaseObject BaseObject;
            const string sExceptionMsg = "[Exception] TBaseObject::GetMapBaseObjects";
            if (rList == null)
            {
                return false;
            }

            try
            {
                int nStartX = nX - nRage;
                int nEndX = nX + nRage;
                int nStartY = nY - nRage;
                int nEndY = nY + nRage;
                for (var x = nStartX; x <= nEndX; x++)
                {
                    for (var y = nStartY; y <= nEndY; y++)
                    {
                        var cellsuccess = false;
                        cellInfo = tEnvir.GetCellInfo(x, y, ref cellsuccess);
                        if (cellsuccess && (cellInfo.ObjList != null))
                        {
                            for (var i = 0; i < cellInfo.Count; i++)
                            {
                                OSObject = cellInfo.ObjList[i];
                                if ((OSObject != null) && (OSObject.CellType == CellType.MovingObject))
                                {
                                    BaseObject = M2Share.ActorMgr.Get(OSObject.CellObjId);
                                    if ((BaseObject != null) && (!BaseObject.Death) && (!BaseObject.Ghost))
                                    {
                                        rList.Add(BaseObject);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                M2Share.LogSystem.Error(sExceptionMsg);
            }

            return true;
        }

        public void SendRefMsg(int wIdent, int wParam, int nParam1, int nParam2, int nParam3, string sMsg)
        {
            const string sExceptionMsg = "[Exception] TBaseObject::SendRefMsg Name = {0}";
            if (Envir == null)
            {
                M2Share.LogSystem.Error(CharName + " SendRefMsg nil PEnvir ");
                return;
            }

            if (ObMode || FixedHideMode)
            {
                SendMsg(this, wIdent, wParam, nParam1, nParam2, nParam3, sMsg); // 如果隐身模式则只发送信息给自己
                return;
            }

            HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            try
            {
                TBaseObject BaseObject;
                if (((HUtil32.GetTickCount() - SendRefMsgTick) >= 500) || (VisibleHumanList.Count == 0))
                {
                    SendRefMsgTick = HUtil32.GetTickCount();
                    VisibleHumanList.Clear();
                    var nLX = (short)(CurrX - M2Share.g_Config.nSendRefMsgRange); // 12
                    var nHX = (short)(CurrX + M2Share.g_Config.nSendRefMsgRange); // 12
                    var nLY = (short)(CurrY - M2Share.g_Config.nSendRefMsgRange); // 12
                    var nHY = (short)(CurrY + M2Share.g_Config.nSendRefMsgRange); // 12
                    for (var nCX = nLX; nCX <= nHX; nCX++)
                    {
                        for (var nCY = nLY; nCY <= nHY; nCY++)
                        {
                            var cellsuccess = false;
                            var cellInfo = Envir.GetCellInfo(nCX, nCY, ref cellsuccess);
                            if (cellsuccess)
                            {
                                if (cellInfo.ObjList != null)
                                {
                                    for (var i = 0; i < cellInfo.Count; i++)
                                    {
                                        var OSObject = cellInfo.ObjList[i];
                                        if (OSObject != null)
                                        {
                                            if (OSObject.CellType == CellType.MovingObject)
                                            {
                                                if ((HUtil32.GetTickCount() - OSObject.AddTime) >= 60 * 1000)
                                                {
                                                    cellInfo.Remove(OSObject);
                                                    if (cellInfo.Count <= 0)
                                                    {
                                                        cellInfo.Dispose();
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        BaseObject = M2Share.ActorMgr.Get(OSObject.CellObjId);
                                                        if ((BaseObject != null) && !BaseObject.Ghost)
                                                        {
                                                            if (BaseObject.Race == Grobal2.RC_PLAYOBJECT)
                                                            {
                                                                BaseObject.SendMsg(this, wIdent, wParam, nParam1,
                                                                    nParam2, nParam3, sMsg);
                                                                VisibleHumanList.Add(BaseObject);
                                                            }
                                                            else if (BaseObject.WantRefMsg)
                                                            {
                                                                if ((wIdent == Grobal2.RM_STRUCK) ||
                                                                    (wIdent == Grobal2.RM_HEAR) ||
                                                                    (wIdent == Grobal2.RM_DEATH))
                                                                {
                                                                    BaseObject.SendMsg(this, wIdent, wParam, nParam1,
                                                                        nParam2, nParam3, sMsg);
                                                                    VisibleHumanList.Add(BaseObject);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        cellInfo.Remove(OSObject);
                                                        if (cellInfo.Count <= 0)
                                                        {
                                                            cellInfo.Dispose();
                                                        }

                                                        M2Share.LogSystem.Error(format(sExceptionMsg, CharName));
                                                        M2Share.LogSystem.Error(e.Message);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    return;
                }

                for (var nC = 0; nC < VisibleHumanList.Count; nC++)
                {
                    BaseObject = VisibleHumanList[nC];
                    if (BaseObject.Ghost)
                    {
                        continue;
                    }

                    if ((BaseObject.Envir == Envir) && (Math.Abs(BaseObject.CurrX - CurrX) < 11) &&
                        (Math.Abs(BaseObject.CurrY - CurrY) < 11))
                    {
                        if (BaseObject.Race == Grobal2.RC_PLAYOBJECT)
                        {
                            BaseObject.SendMsg(this, wIdent, wParam, nParam1, nParam2, nParam3, sMsg);
                        }
                        else if (BaseObject.WantRefMsg)
                        {
                            if ((wIdent == Grobal2.RM_STRUCK) || (wIdent == Grobal2.RM_HEAR) ||
                                (wIdent == Grobal2.RM_DEATH))
                            {
                                BaseObject.SendMsg(this, wIdent, wParam, nParam1, nParam2, nParam3, sMsg);
                            }
                        }
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
        }

        public int GetFeatureToLong()
        {
            return GetFeature(null);
        }

        public ushort GetFeatureEx()
        {
            ushort result;
            if (OnHorse)
            {
                result = HUtil32.MakeWord(HorseType, DressEffType);
            }
            else
            {
                result = HUtil32.MakeWord(0, DressEffType);
            }

            return result;
        }

        public int GetFeature(TBaseObject BaseObject)
        {
            StdItem StdItem;
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                byte nDress = 0;
                if (UseItems[Grobal2.U_DRESS] != null && UseItems[Grobal2.U_DRESS].wIndex > 0) // 衣服
                {
                    StdItem = M2Share.UserEngine.GetStdItem(UseItems[Grobal2.U_DRESS].wIndex);
                    if (StdItem != null)
                    {
                        nDress = (byte)(StdItem.Shape * 2);
                    }
                }

                nDress += (byte)Gender;
                byte nWeapon = 0;
                if (UseItems[Grobal2.U_WEAPON] != null && UseItems[Grobal2.U_WEAPON].wIndex > 0) // 武器
                {
                    StdItem = M2Share.UserEngine.GetStdItem(UseItems[Grobal2.U_WEAPON].wIndex);
                    if (StdItem != null)
                    {
                        nWeapon = (byte)(StdItem.Shape * 2);
                    }
                }

                nWeapon += (byte)Gender;
                byte nHair = (byte)(Hair * 2 + (byte)Gender);
                return Grobal2.MakeHumanFeature(0, nDress, nWeapon, nHair);
            }

            var bo25 = BaseObject != null && BaseObject.m_boRaceImg;
            if (bo25)
            {
                byte nRaceImg = RaceImg;
                byte nAppr = (byte)m_wAppr;
                switch (nAppr)
                {
                    case 0:
                        nRaceImg = 12;
                        nAppr = 5;
                        break;
                    case 1:
                        nRaceImg = 11;
                        nAppr = 9;
                        break;
                    case 160:
                        nRaceImg = 10;
                        nAppr = 0;
                        break;
                    case 161:
                        nRaceImg = 10;
                        nAppr = 1;
                        break;
                    case 162:
                        nRaceImg = 11;
                        nAppr = 6;
                        break;
                    case 163:
                        nRaceImg = 11;
                        nAppr = 3;
                        break;
                }

                return Grobal2.MakeMonsterFeature(nRaceImg, MonsterWeapon, nAppr);
            }

            return Grobal2.MakeMonsterFeature(RaceImg, MonsterWeapon, m_wAppr);
        }

        public int GetCharStatus()
        {
            long nStatus = 0;
            for (int i = 0; i < m_wStatusTimeArr.Length; i++)
            {
                if (m_wStatusTimeArr[i] > 0)
                {
                    nStatus = (0x80000000 >> i) | nStatus;
                }
            }

            var status = (m_nCharStatusEx & 0x0000FFFF) | nStatus;
            return status >= int.MaxValue ? 0 : (int)status;
        }

        public void AbilCopyToWAbil()
        {
            m_WAbil = Abil;
        }

        public virtual void Initialize()
        {
            AbilCopyToWAbil();
            for (var i = 0; i < MagicList.Count; i++)
            {
                if (MagicList[i].btLevel >= 4)
                {
                    MagicList[i].btLevel = 0;
                }
            }

            AddtoMapSuccess = true;
            if (Envir.CanWalk(CurrX, CurrY, true) && AddToMap())
            {
                AddtoMapSuccess = false;
            }

            CharStatus = GetCharStatus();
            AddBodyLuck(0);
            LoadSayMsg();
            if (M2Share.g_Config.boMonSayMsg)
            {
                MonsterSayMsg(null, MonStatus.MonGen);
            }
        }

        /// <summary>
        /// 取怪物说话信息列表
        /// </summary>
        private void LoadSayMsg()
        {
            for (var i = 0; i < M2Share.g_MonSayMsgList.Count; i++)
            {
                if (M2Share.g_MonSayMsgList.TryGetValue(CharName, out SayMsgList))
                {
                    break;
                }
            }
        }

        public virtual void Disappear()
        {

        }

        public void FeatureChanged()
        {
            SendRefMsg(Grobal2.RM_FEATURECHANGED, GetFeatureEx(), GetFeatureToLong(), 0, 0, "");
        }

        public void StatusChanged()
        {
            SendRefMsg(Grobal2.RM_CHARSTATUSCHANGED, HitSpeed, CharStatus, 0, 0, "");
        }

        protected void DisappearA()
        {
            Envir.DeleteFromMap(CurrX, CurrY, CellType.MovingObject, this);
            SendRefMsg(Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
        }

        protected void KickException()
        {
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                MapName = M2Share.g_Config.sHomeMap;
                CurrX = M2Share.g_Config.nHomeX;
                CurrY = M2Share.g_Config.nHomeY;
                PlayObject PlayObject = this as PlayObject;
                PlayObject.m_boEmergencyClose = true;
            }
            else
            {
                Death = true;
                DeathTick = HUtil32.GetTickCount();
                MakeGhost();
            }
        }

        protected bool Walk(int nIdent)
        {
            const string sExceptionMsg = "[Exception] TBaseObject::Walk {0} {1} {2}:{3}";
            bool result = true;
            if (Envir == null)
            {
                M2Share.LogSystem.Error("Walk nil PEnvir");
                return result;
            }

            try
            {
                bool cellsuccess = false;
                var cellInfo = Envir.GetCellInfo(CurrX, CurrY, ref cellsuccess);
                if (cellsuccess && (cellInfo.ObjList != null))
                {
                    for (var i = 0; i < cellInfo.Count; i++)
                    {
                        var OSObject = cellInfo.ObjList[i];
                        switch (OSObject.CellType)
                        {
                            case CellType.GateObject:
                                var GateObj = (GateObject)M2Share.CellObjectSystem.Get(OSObject.CellObjId);
                                ;
                                if ((GateObj != null))
                                {
                                    if (Race == Grobal2.RC_PLAYOBJECT)
                                    {
                                        if (Envir.ArroundDoorOpened(CurrX, CurrY))
                                        {
                                            if ((!GateObj.DEnvir.Flag.boNEEDHOLE) ||
                                                (M2Share.EventManager.GetEvent(Envir, CurrX, CurrY,
                                                    Grobal2.ET_DIGOUTZOMBI) != null))
                                            {
                                                if (M2Share.nServerIndex == GateObj.DEnvir.ServerIndex)
                                                {
                                                    if (!EnterAnotherMap(GateObj.DEnvir, GateObj.nDMapX,
                                                            GateObj.nDMapY))
                                                    {
                                                        result = false;
                                                    }
                                                }
                                                else
                                                {
                                                    DisappearA();
                                                    m_bo316 = true;
                                                    var PlayObject = this as PlayObject;
                                                    PlayObject.m_sSwitchMapName = GateObj.DEnvir.MapName;
                                                    PlayObject.m_nSwitchMapX = GateObj.nDMapX;
                                                    PlayObject.m_nSwitchMapY = GateObj.nDMapY;
                                                    PlayObject.m_boSwitchData = true;
                                                    PlayObject.m_nServerIndex = GateObj.DEnvir.ServerIndex;
                                                    PlayObject.m_boEmergencyClose = true;
                                                    PlayObject.m_boReconnection = true;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        result = false;
                                    }
                                }

                                break;
                            case CellType.EventObject:
                            {
                                MirEvent mapEvent = null;
                                var owinEvent = (MirEvent)M2Share.CellObjectSystem.Get(OSObject.CellObjId);
                                if (owinEvent.OwnBaseObject != null)
                                {
                                    mapEvent = (MirEvent)M2Share.CellObjectSystem.Get(OSObject.CellObjId);
                                    ;
                                }

                                if (mapEvent != null)
                                {
                                    if (mapEvent.OwnBaseObject.IsProperTarget(this))
                                    {
                                        SendMsg(mapEvent.OwnBaseObject, Grobal2.RM_MAGSTRUCK_MINE, 0, mapEvent.Damage,
                                            0, 0, "");
                                    }
                                }

                                break;
                            }
                            case CellType.MapEvent:
                                break;
                            case CellType.Door:
                                break;
                            case CellType.Roon:
                                break;
                        }
                    }
                }

                if (result)
                {
                    SendRefMsg(nIdent, Direction, CurrX, CurrY, 0, "");
                }
            }
            catch (Exception e)
            {
                M2Share.LogSystem.Error(format(sExceptionMsg,
                    new object[] { CharName, MapName, CurrX, CurrY }));
                M2Share.LogSystem.Error(e.Message);
            }

            return result;
        }

        /// <summary>
        /// 切换地图
        /// </summary>
        private bool EnterAnotherMap(Envirnoment Envir, short nDMapX, short nDMapY)
        {
            bool result = false;
            const string sExceptionMsg7 = "[Exception] TBaseObject::EnterAnotherMap";
            try
            {
                if (Abil.Level < Envir.RequestLevel)
                {
                    SysMsg($"需要 {Envir.Flag.nL - 1} 级以上才能进入 {Envir.MapDesc}", MsgColor.Red, MsgType.Hint);
                    return false;
                }

                if (Envir.QuestNpc != null)
                {
                    Envir.QuestNpc.Click(this as PlayObject);
                }

                if (Envir.Flag.nNEEDSETONFlag >= 0)
                {
                    if (GetQuestFalgStatus(Envir.Flag.nNEEDSETONFlag) != Envir.Flag.nNeedONOFF)
                    {
                        return false;
                    }
                }

                var cellsuccess = false;
                Envir.GetCellInfo(nDMapX, nDMapY, ref cellsuccess);
                if (!cellsuccess)
                {
                    return false;
                }

                var Castle = M2Share.CastleManager.IsCastlePalaceEnvir(Envir);
                if ((Castle != null) && (Race == Grobal2.RC_PLAYOBJECT))
                {
                    if (!Castle.CheckInPalace(CurrX, CurrY, this))
                    {
                        return false;
                    }
                }

                if (Envir.Flag.boNOHORSE)
                {
                    OnHorse = false;
                }

                var OldEnvir = this.Envir;
                short nOldX = CurrX;
                short nOldY = CurrY;
                DisappearA();
                VisibleHumanList.Clear();
                for (var i = 0; i < VisibleItems.Count; i++)
                {
                    VisibleItems[i] = null;
                }

                VisibleItems.Clear();
                VisibleEvents.Clear();
                for (var i = 0; i < VisibleActors.Count; i++)
                {
                    VisibleActors[i] = null;
                }

                VisibleActors.Clear();
                SendMsg(this, Grobal2.RM_CLEAROBJECTS, 0, 0, 0, 0, "");
                this.Envir = Envir;
                MapName = Envir.MapName;
                MapFileName = Envir.MapFileName;
                CurrX = nDMapX;
                CurrY = nDMapY;
                SendMsg(this, Grobal2.RM_CHANGEMAP, 0, 0, 0, 0, Envir.MapFileName);
                if (AddToMap())
                {
                    MapMoveTick = HUtil32.GetTickCount();
                    m_bo316 = true;
                    result = true;
                }
                else
                {
                    this.Envir = OldEnvir;
                    CurrX = nOldX;
                    CurrY = nOldY;
                    this.Envir.AddToMap(CurrX, CurrY, CellType.MovingObject, this);
                }

                OnEnvirnomentChanged();
                if (Race == Grobal2.RC_PLAYOBJECT) // 复位泡点，及金币，时间
                {
                    (this as PlayObject).m_dwIncGamePointTick = HUtil32.GetTickCount();
                    (this as PlayObject).m_dwIncGameGoldTick = HUtil32.GetTickCount();
                    (this as PlayObject).m_dwAutoGetExpTick = HUtil32.GetTickCount();
                }

                if (this.Envir.Flag.boFight3Zone && (this.Envir.Flag.boFight3Zone != OldEnvir.Flag.boFight3Zone))
                {
                    RefShowName();
                }
            }
            catch
            {
                M2Share.LogSystem.Error(sExceptionMsg7);
            }

            return result;
        }

        protected void TurnTo(byte nDir)
        {
            Direction = nDir;
            SendRefMsg(Grobal2.RM_TURN, nDir, CurrX, CurrY, 0, "");
        }

        public void SysMsg(string sMsg, MsgColor MsgColor, MsgType MsgType)
        {
            if (M2Share.g_Config.boShowPreFixMsg)
            {
                switch (MsgType)
                {
                    case MsgType.Mon:
                        sMsg = M2Share.g_Config.sMonSayMsgpreFix + sMsg;
                        break;
                    case MsgType.Hint:
                        sMsg = M2Share.g_Config.sHintMsgPreFix + sMsg;
                        break;
                    case MsgType.GM:
                        sMsg = M2Share.g_Config.sGMRedMsgpreFix + sMsg;
                        break;
                    case MsgType.System:
                        sMsg = M2Share.g_Config.sSysMsgPreFix + sMsg;
                        break;
                    case MsgType.Cust:
                        sMsg = M2Share.g_Config.sCustMsgpreFix + sMsg;
                        break;
                    case MsgType.Castle:
                        sMsg = M2Share.g_Config.sCastleMsgpreFix + sMsg;
                        break;
                }
            }

            if (MsgType == MsgType.Notice) // 如果发的是公告
            {
                string str = string.Empty;
                string FColor = string.Empty;
                string BColor = string.Empty;
                string nTime = string.Empty;
                if (sMsg[0] == '[') // 顶部滚动公告
                {
                    sMsg = HUtil32.ArrestStringEx(sMsg, "[", "]", ref str);
                    BColor = HUtil32.GetValidStrCap(str, ref FColor, new string[] { "," });
                    if (M2Share.g_Config.boShowPreFixMsg)
                    {
                        sMsg = M2Share.g_Config.sLineNoticePreFix + sMsg;
                    }

                    SendMsg(this, Grobal2.RM_MOVEMESSAGE, 0, HUtil32.Str_ToInt(FColor, 255),
                        HUtil32.Str_ToInt(BColor, 255), 0, sMsg);
                }
                else if (sMsg[0] == '<') // 聊天框彩色公告
                {
                    sMsg = HUtil32.ArrestStringEx(sMsg, "<", ">", ref str);
                    BColor = HUtil32.GetValidStrCap(str, ref FColor, new string[] { "," });
                    if (M2Share.g_Config.boShowPreFixMsg)
                    {
                        sMsg = M2Share.g_Config.sLineNoticePreFix + sMsg;
                    }

                    SendMsg(this, Grobal2.RM_SYSMESSAGE, 0, HUtil32.Str_ToInt(FColor, 255),
                        HUtil32.Str_ToInt(BColor, 255), 0, sMsg);
                }
                else if (sMsg[0] == '{') // 屏幕居中公告
                {
                    sMsg = HUtil32.ArrestStringEx(sMsg, "{", "}", ref str);
                    str = HUtil32.GetValidStrCap(str, ref FColor, new string[] { "," });
                    str = HUtil32.GetValidStrCap(str, ref BColor, new string[] { "," });
                    str = HUtil32.GetValidStrCap(str, ref nTime, new string[] { "," });
                    if (M2Share.g_Config.boShowPreFixMsg)
                    {
                        sMsg = M2Share.g_Config.sLineNoticePreFix + sMsg;
                    }

                    SendMsg(this, Grobal2.RM_MOVEMESSAGE, 1, HUtil32.Str_ToInt(FColor, 255),
                        HUtil32.Str_ToInt(BColor, 255), HUtil32.Str_ToInt(nTime, 0), sMsg);
                }
                else
                {
                    switch (MsgColor)
                    {
                        case MsgColor.Red: // 控制公告的颜色
                            if (M2Share.g_Config.boShowPreFixMsg)
                            {
                                sMsg = M2Share.g_Config.sLineNoticePreFix + sMsg;
                            }

                            SendMsg(this, Grobal2.RM_SYSMESSAGE, 0, M2Share.g_Config.btRedMsgFColor,
                                M2Share.g_Config.btRedMsgBColor, 0, sMsg);
                            break;
                        case MsgColor.Green:
                            if (M2Share.g_Config.boShowPreFixMsg)
                            {
                                sMsg = M2Share.g_Config.sLineNoticePreFix + sMsg;
                            }

                            SendMsg(this, Grobal2.RM_SYSMESSAGE, 0, M2Share.g_Config.btGreenMsgFColor,
                                M2Share.g_Config.btGreenMsgBColor, 0, sMsg);
                            break;
                        case MsgColor.Blue:
                            if (M2Share.g_Config.boShowPreFixMsg)
                            {
                                sMsg = M2Share.g_Config.sLineNoticePreFix + sMsg;
                            }

                            SendMsg(this, Grobal2.RM_SYSMESSAGE, 0, M2Share.g_Config.btBlueMsgFColor,
                                M2Share.g_Config.btBlueMsgBColor, 0, sMsg);
                            break;
                    }
                }
            }
            else
            {
                switch (MsgColor)
                {
                    case MsgColor.Green:
                        SendMsg(this, Grobal2.RM_SYSMESSAGE, 0, M2Share.g_Config.btGreenMsgFColor,
                            M2Share.g_Config.btGreenMsgBColor, 0, sMsg);
                        break;
                    case MsgColor.Blue:
                        SendMsg(this, Grobal2.RM_SYSMESSAGE, 0, M2Share.g_Config.btBlueMsgFColor,
                            M2Share.g_Config.btBlueMsgBColor, 0, sMsg);
                        break;
                    default:
                        if (MsgType == MsgType.Cust)
                        {
                            SendMsg(this, Grobal2.RM_SYSMESSAGE, 0, M2Share.g_Config.btCustMsgFColor,
                                M2Share.g_Config.btCustMsgBColor, 0, sMsg);
                        }
                        else
                        {
                            SendMsg(this, Grobal2.RM_SYSMESSAGE, 0, M2Share.g_Config.btRedMsgFColor,
                                M2Share.g_Config.btRedMsgBColor, 0, sMsg);
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// 怪物说话
        /// </summary>
        /// <param name="AttackBaseObject"></param>
        /// <param name="MonStatus"></param>
        protected void MonsterSayMsg(TBaseObject AttackBaseObject, MonStatus MonStatus)
        {
            if (SayMsgList == null)
            {
                return;
            }

            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                return;
            }

            string sAttackName = string.Empty;
            if (AttackBaseObject != null)
            {
                if ((AttackBaseObject.Race != Grobal2.RC_PLAYOBJECT) && (AttackBaseObject.Master == null))
                {
                    return;
                }

                if (AttackBaseObject.Master != null)
                {
                    sAttackName = AttackBaseObject.Master.CharName;
                }
                else
                {
                    sAttackName = AttackBaseObject.CharName;
                }
            }

            TMonSayMsg MonSayMsg = null;
            string sMsg = string.Empty;
            for (var i = 0; i < SayMsgList.Count; i++)
            {
                MonSayMsg = SayMsgList[i];
                sMsg = MonSayMsg.sSayMsg.Replace("%s", M2Share.FilterShowName(CharName));
                sMsg = sMsg.Replace("%d", sAttackName);
                if ((MonSayMsg.State == MonStatus) && (M2Share.RandomNumber.Random(MonSayMsg.nRate) == 0))
                {
                    if (MonStatus == MonStatus.MonGen)
                    {
                        M2Share.UserEngine.SendBroadCastMsg(sMsg, MsgType.Mon);
                        break;
                    }

                    if (MonSayMsg.Color == MsgColor.White)
                    {
                        ProcessSayMsg(sMsg);
                    }
                    else
                    {
                        AttackBaseObject.SysMsg(sMsg, MonSayMsg.Color, MsgType.Mon);
                    }

                    break;
                }
            }
        }

        public void SendGroupText(string sMsg)
        {
            PlayObject PlayObject;
            sMsg = M2Share.g_Config.sGroupMsgPreFix + sMsg;
            if (m_GroupOwner != null)
            {
                for (int i = 0; i < m_GroupOwner.GroupMembers.Count; i++)
                {
                    PlayObject = m_GroupOwner.GroupMembers[i];
                    PlayObject.SendMsg(this, Grobal2.RM_GROUPMESSAGE, 0, M2Share.g_Config.btGroupMsgFColor,
                        M2Share.g_Config.btGroupMsgBColor, 0, sMsg);
                }
            }
        }

        /// <summary>
        /// 设置肉的品质
        /// </summary>
        protected void ApplyMeatQuality()
        {
            for (var i = 0; i < ItemList.Count; i++)
            {
                var StdItem = M2Share.UserEngine.GetStdItem(ItemList[i].wIndex);
                if (StdItem != null)
                {
                    if (StdItem.StdMode == 40)
                    {
                        ItemList[i].Dura = m_nMeatQuality;
                    }
                }
            }
        }

        protected bool TakeBagItems(TBaseObject BaseObject)
        {
            bool result = false;
            while (true)
            {
                if (BaseObject.ItemList.Count <= 0)
                {
                    break;
                }

                var UserItem = BaseObject.ItemList[0];
                if (!AddItemToBag(UserItem))
                {
                    break;
                }

                if (this is PlayObject)
                {
                    var PlayObject = this as PlayObject;
                    PlayObject.SendAddItem(UserItem);
                    result = true;
                }

                BaseObject.ItemList.RemoveAt(0);
            }

            return result;
        }

        /// <summary>
        /// 散落金币
        /// </summary>
        /// <param name="GoldOfCreat"></param>
        private void ScatterGolds(TBaseObject GoldOfCreat)
        {
            int I;
            int nGold;
            if (Gold > 0)
            {
                I = 0;
                while (true)
                {
                    if (Gold > M2Share.g_Config.nMonOneDropGoldCount)
                    {
                        nGold = M2Share.g_Config.nMonOneDropGoldCount;
                        Gold = Gold - M2Share.g_Config.nMonOneDropGoldCount;
                    }
                    else
                    {
                        nGold = Gold;
                        Gold = 0;
                    }

                    if (nGold > 0)
                    {
                        if (!DropGoldDown(nGold, true, GoldOfCreat, this))
                        {
                            Gold = Gold + nGold;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }

                    I++;
                    if (I >= 17)
                    {
                        break;
                    }
                }

                GoldChanged();
            }
        }

        public void SetLastHiter(TBaseObject BaseObject)
        {
            LastHiter = BaseObject;
            LastHiterTick = HUtil32.GetTickCount();
            if (ExpHitter == null)
            {
                ExpHitter = BaseObject;
                ExpHitterTick = HUtil32.GetTickCount();
            }
            else
            {
                if (ExpHitter == BaseObject)
                {
                    ExpHitterTick = HUtil32.GetTickCount();
                }
            }
        }

        public void SetPKFlag(TBaseObject BaseObject)
        {
            if ((PKLevel() < 2) && (BaseObject.PKLevel() < 2) && (!Envir.Flag.boFightZone) &&
                (!Envir.Flag.boFight3Zone) && !m_boPKFlag)
            {
                BaseObject.m_dwPKTick = HUtil32.GetTickCount();
                if (!BaseObject.m_boPKFlag)
                {
                    BaseObject.m_boPKFlag = true;
                    BaseObject.RefNameColor();
                }
            }
        }

        protected bool IsGoodKilling(TBaseObject cert)
        {
            return cert.m_boPKFlag;
        }

        protected bool IsAttackTarget_sub_4C88E4()
        {
            return true;
        }

        /// <summary>
        /// 是否可以攻击的目标
        /// </summary>
        /// <param name="BaseObject"></param>
        /// <returns></returns>
        protected virtual bool IsAttackTarget(TBaseObject BaseObject)
        {
            bool result = false;
            if ((BaseObject == null) || (BaseObject == this))
            {
                return false;
            }

            if (Race >= Grobal2.RC_ANIMAL)
            {
                if (Master != null)
                {
                    if ((Master.LastHiter == BaseObject) || (Master.ExpHitter == BaseObject) ||
                        (Master.TargetCret == BaseObject))
                    {
                        result = true;
                    }

                    if (BaseObject.TargetCret != null)
                    {
                        if ((BaseObject.TargetCret == Master) || (BaseObject.TargetCret.Master == Master) &&
                            (BaseObject.Race != Grobal2.RC_PLAYOBJECT))
                        {
                            result = true;
                        }
                    }

                    if ((BaseObject.TargetCret == this) && (BaseObject.Race >= Grobal2.RC_ANIMAL))
                    {
                        result = true;
                    }

                    if (BaseObject.Master != null)
                    {
                        if ((BaseObject.Master == Master.LastHiter) ||
                            (BaseObject.Master == Master.TargetCret))
                        {
                            result = true;
                        }
                    }

                    if (BaseObject.Master == Master)
                    {
                        result = false;
                    }

                    if (BaseObject.m_boHolySeize)
                    {
                        result = false;
                    }

                    if (Master.SlaveRelax)
                    {
                        result = false;
                    }

                    if (BaseObject.Race == Grobal2.RC_PLAYOBJECT)
                    {
                        if (BaseObject.InSafeZone())
                        {
                            result = false;
                        }
                    }

                    BreakCrazyMode();
                }
                else
                {
                    if (BaseObject.Race == Grobal2.RC_PLAYOBJECT)
                    {
                        result = true;
                    }

                    if ((Race > Grobal2.RC_PEACENPC) && (Race < Grobal2.RC_ANIMAL))
                    {
                        result = true;
                    }

                    if (BaseObject.Master != null)
                    {
                        result = true;
                    }
                }

                if (m_boCrazyMode && ((BaseObject.Race == Grobal2.RC_PLAYOBJECT) ||
                                      (BaseObject.Race > Grobal2.RC_PEACENPC)))
                {
                    result = true;
                }

                if (m_boNastyMode && ((BaseObject.Race < Grobal2.RC_NPC) ||
                                      (BaseObject.Race > Grobal2.RC_PEACENPC)))
                {
                    result = true;
                }
            }
            else
            {
                if (Race == Grobal2.RC_PLAYOBJECT)
                {
                    switch (AttatckMode)
                    {
                        case AttackMode.HAM_ALL:
                            if ((BaseObject.Race < Grobal2.RC_NPC) ||
                                (BaseObject.Race > Grobal2.RC_PEACENPC))
                            {
                                result = true;
                            }

                            if (M2Share.g_Config.boNonPKServer)
                            {
                                result = IsAttackTarget_sub_4C88E4();
                            }

                            break;
                        case AttackMode.HAM_PEACE:
                            if (BaseObject.Race >= Grobal2.RC_ANIMAL)
                            {
                                result = true;
                            }

                            break;
                        case AttackMode.HAM_DEAR:
                            if (BaseObject != (this as PlayObject).m_DearHuman)
                            {
                                result = true;
                            }

                            break;
                        case AttackMode.HAM_MASTER:
                            if (BaseObject.Race == Grobal2.RC_PLAYOBJECT)
                            {
                                result = true;
                                if ((this as PlayObject).m_boMaster)
                                {
                                    for (var i = 0; i < (this as PlayObject).m_MasterList.Count; i++)
                                    {
                                        if ((this as PlayObject).m_MasterList[i] == BaseObject)
                                        {
                                            result = false;
                                            break;
                                        }
                                    }
                                }

                                if ((BaseObject as PlayObject).m_boMaster)
                                {
                                    for (var i = 0; i < (BaseObject as PlayObject).m_MasterList.Count; i++)
                                    {
                                        if ((BaseObject as PlayObject).m_MasterList[i] == this)
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
                            if ((BaseObject.Race < Grobal2.RC_NPC) ||
                                (BaseObject.Race > Grobal2.RC_PEACENPC))
                            {
                                result = true;
                            }

                            if (BaseObject.Race == Grobal2.RC_PLAYOBJECT)
                            {
                                if (IsGroupMember(BaseObject))
                                {
                                    result = false;
                                }
                            }

                            if (M2Share.g_Config.boNonPKServer)
                            {
                                result = IsAttackTarget_sub_4C88E4();
                            }

                            break;
                        case AttackMode.HAM_GUILD:
                            if ((BaseObject.Race < Grobal2.RC_NPC) ||
                                (BaseObject.Race > Grobal2.RC_PEACENPC))
                            {
                                result = true;
                            }

                            if (BaseObject.Race == Grobal2.RC_PLAYOBJECT)
                            {
                                if (MyGuild != null)
                                {
                                    if (MyGuild.IsMember(BaseObject.CharName))
                                    {
                                        result = false;
                                    }

                                    if (GuildWarArea && (BaseObject.MyGuild != null))
                                    {
                                        if (MyGuild.IsAllyGuild(BaseObject.MyGuild))
                                        {
                                            result = false;
                                        }
                                    }
                                }
                            }

                            if (M2Share.g_Config.boNonPKServer)
                            {
                                result = IsAttackTarget_sub_4C88E4();
                            }

                            break;
                        case AttackMode.HAM_PKATTACK:
                            if ((BaseObject.Race < Grobal2.RC_NPC) ||
                                (BaseObject.Race > Grobal2.RC_PEACENPC))
                            {
                                result = true;
                            }

                            if (BaseObject.Race == Grobal2.RC_PLAYOBJECT)
                            {
                                if (PKLevel() >= 2)
                                {
                                    if (BaseObject.PKLevel() < 2)
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        result = false;
                                    }
                                }
                                else
                                {
                                    if (BaseObject.PKLevel() >= 2)
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        result = false;
                                    }
                                }
                            }

                            if (M2Share.g_Config.boNonPKServer)
                            {
                                result = IsAttackTarget_sub_4C88E4();
                            }

                            break;
                    }
                }
                else
                {
                    result = true;
                }
            }

            if (BaseObject.AdminMode || BaseObject.StoneMode)
            {
                result = false;
            }

            return result;
        }

        public virtual bool IsProperTarget(TBaseObject BaseObject)
        {
            bool result = IsAttackTarget(BaseObject);
            if (result)
            {
                if ((Race == Grobal2.RC_PLAYOBJECT) && (BaseObject.Race == Grobal2.RC_PLAYOBJECT))
                {
                    result = IsProtectTarget(BaseObject);
                }
            }

            if ((BaseObject != null) && (Race == Grobal2.RC_PLAYOBJECT) && (BaseObject.Master != null) &&
                (BaseObject.Race != Grobal2.RC_PLAYOBJECT))
            {
                if (BaseObject.Master == this)
                {
                    if (AttatckMode != AttackMode.HAM_ALL)
                    {
                        result = false;
                    }
                }
                else
                {
                    result = IsAttackTarget(BaseObject.Master);
                    if (InSafeZone() || BaseObject.InSafeZone())
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        protected void WeightChanged()
        {
            m_WAbil.Weight = RecalcBagWeight();
            SendUpdateMsg(this, Grobal2.RM_WEIGHTCHANGED, 0, 0, 0, 0, "");
        }

        public bool InSafeZone()
        {
            int nSafeX;
            int nSafeY;
            if (Envir == null)
            {
                return true;
            }

            var result = Envir.Flag.boSAFE;
            if (result)
            {
                return true;
            }

            if ((Envir.MapName != M2Share.g_Config.sRedHomeMap) ||
                (Math.Abs(CurrX - M2Share.g_Config.nRedHomeX) > M2Share.g_Config.nSafeZoneSize) ||
                (Math.Abs(CurrY - M2Share.g_Config.nRedHomeY) > M2Share.g_Config.nSafeZoneSize))
            {
                result = false;
            }
            else
            {
                result = true;
            }

            if (result)
            {
                return result;
            }

            for (int i = 0; i < M2Share.StartPointList.Count; i++)
            {
                if (M2Share.StartPointList[i].m_sMapName == Envir.MapName)
                {
                    if (M2Share.StartPointList[i] != null)
                    {
                        nSafeX = M2Share.StartPointList[i].m_nCurrX;
                        nSafeY = M2Share.StartPointList[i].m_nCurrY;
                        if ((Math.Abs(CurrX - nSafeX) <= M2Share.g_Config.nSafeZoneSize) &&
                            (Math.Abs(CurrY - nSafeY) <= M2Share.g_Config.nSafeZoneSize))
                        {
                            result = true;
                        }
                    }
                }
            }

            return result;
        }

        public bool InSafeZone(Envirnoment Envir, int nX, int nY)
        {
            int nSafeX;
            int nSafeY;
            if (this.Envir == null)
            {
                return true;
            }

            bool result = this.Envir.Flag.boSAFE;
            if (result)
            {
                return true;
            }

            if ((Envir.MapName != M2Share.g_Config.sRedHomeMap) ||
                (Math.Abs(nX - M2Share.g_Config.nRedHomeX) > M2Share.g_Config.nSafeZoneSize) ||
                (Math.Abs(nY - M2Share.g_Config.nRedHomeY) > M2Share.g_Config.nSafeZoneSize))
            {
                result = false;
            }
            else
            {
                return true;
            }

            for (int i = 0; i < M2Share.StartPointList.Count; i++)
            {
                if (M2Share.StartPointList[i].m_sMapName == Envir.MapName)
                {
                    if (M2Share.StartPointList[i] != null)
                    {
                        nSafeX = M2Share.StartPointList[i].m_nCurrX;
                        nSafeY = M2Share.StartPointList[i].m_nCurrY;
                        if ((Math.Abs(nX - nSafeX) <= M2Share.g_Config.nSafeZoneSize) &&
                            (Math.Abs(nY - nSafeY) <= M2Share.g_Config.nSafeZoneSize))
                        {
                            result = true;
                        }
                    }
                }
            }

            return result;
        }

        public void OpenHolySeizeMode(int dwInterval)
        {
            m_boHolySeize = true;
            m_dwHolySeizeTick = HUtil32.GetTickCount();
            m_dwHolySeizeInterval = dwInterval;
            RefNameColor();
        }

        public void BreakHolySeizeMode()
        {
            m_boHolySeize = false;
            RefNameColor();
        }

        public void OpenCrazyMode(int nTime)
        {
            m_boCrazyMode = true;
            m_dwCrazyModeTick = HUtil32.GetTickCount();
            m_dwCrazyModeInterval = nTime * 1000;
            RefNameColor();
        }

        public void BreakCrazyMode()
        {
            if (m_boCrazyMode)
            {
                m_boCrazyMode = false;
                RefNameColor();
            }
        }

        private void LeaveGroup()
        {
            const string sExitGropMsg = "{0} 已经退出了本组.";
            SendGroupText(format(sExitGropMsg, CharName));
            m_GroupOwner = null;
            SendMsg(this, Grobal2.RM_GROUPCANCEL, 0, 0, 0, 0, "");
        }

        protected TUserMagic GetMagicInfo(int nMagicID)
        {
            TUserMagic result = null;
            TUserMagic UserMagic;
            for (int i = 0; i < MagicList.Count; i++)
            {
                UserMagic = MagicList[i];
                if (UserMagic.MagicInfo.wMagicID == nMagicID)
                {
                    result = UserMagic;
                    break;
                }
            }

            return result;
        }

        public void TrainSkill(TUserMagic UserMagic, int nTranPoint)
        {
            if (FastTrain)
            {
                nTranPoint = nTranPoint * 3;
            }

            UserMagic.nTranPoint += nTranPoint;
        }

        public bool CheckMagicLevelup(TUserMagic UserMagic)
        {
            bool result = false;
            int nLevel;
            if ((UserMagic.btLevel < 4) && (UserMagic.MagicInfo.btTrainLv >= UserMagic.btLevel))
            {
                nLevel = UserMagic.btLevel;
            }
            else
            {
                nLevel = 0;
            }

            if ((UserMagic.MagicInfo.btTrainLv > UserMagic.btLevel) &&
                (UserMagic.MagicInfo.MaxTrain[nLevel] <= UserMagic.nTranPoint))
            {
                if (UserMagic.MagicInfo.btTrainLv > UserMagic.btLevel)
                {
                    UserMagic.nTranPoint -= UserMagic.MagicInfo.MaxTrain[nLevel];
                    UserMagic.btLevel++;
                    SendUpdateDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, UserMagic.MagicInfo.wMagicID, UserMagic.btLevel,
                        UserMagic.nTranPoint, "", 800);
                    CheckSeeHealGauge(UserMagic);
                }
                else
                {
                    UserMagic.nTranPoint = UserMagic.MagicInfo.MaxTrain[nLevel];
                }

                result = true;
            }

            return result;
        }

        /// <summary>
        /// 召唤属下
        /// </summary>
        /// <param name="sSlaveName"></param>
        public void RecallSlave(string sSlaveName)
        {
            short nX = 0;
            short nY = 0;
            int nFlag = -1;
            GetFrontPosition(ref nX, ref nY);
            if (sSlaveName == M2Share.g_Config.sDragon)
            {
                nFlag = 1;
            }

            for (int i = SlaveList.Count - 1; i >= 0; i--)
            {
                if (nFlag == 1)
                {
                    if ((SlaveList[i].CharName == M2Share.g_Config.sDragon) ||
                        (SlaveList[i].CharName == M2Share.g_Config.sDragon1))
                    {
                        SlaveList[i].SpaceMove(Envir.MapName, nX, nY, 1);
                        break;
                    }
                }
                else if (SlaveList[i].CharName == sSlaveName)
                {
                    SlaveList[i].SpaceMove(Envir.MapName, nX, nY, 1);
                    break;
                }
            }
        }

        public ushort GetHitStruckDamage(TBaseObject Target, int nDamage)
        {
            int nArmor;
            var nRnd = HUtil32.HiWord(m_WAbil.AC) - HUtil32.LoWord(m_WAbil.AC) + 1;
            if (nRnd > 0)
            {
                nArmor = HUtil32.LoWord(m_WAbil.AC) + M2Share.RandomNumber.Random(nRnd);
            }
            else
            {
                nArmor = HUtil32.LoWord(m_WAbil.AC);
            }

            nDamage = HUtil32._MAX(0, nDamage - nArmor);
            if (nDamage > 0)
            {
                if ((m_btLifeAttrib == Grobal2.LA_UNDEAD) && (Target != null))
                {
                    nDamage += Target.m_AddAbil.btUndead;
                }

                if (AbilMagBubbleDefence)
                {
                    nDamage = HUtil32.Round(nDamage / 100 * (MagBubbleDefenceLevel + 2) * 8);
                    DamageBubbleDefence(nDamage);
                }
            }

            return (ushort)nDamage;
        }

        public int GetMagStruckDamage(TBaseObject BaseObject, int nDamage)
        {
            int n14 = HUtil32.LoWord(m_WAbil.MAC) +
                      M2Share.RandomNumber.Random(HUtil32.HiWord(m_WAbil.MAC) - HUtil32.LoWord(m_WAbil.MAC) + 1);
            nDamage = HUtil32._MAX(0, nDamage - n14);
            if ((m_btLifeAttrib == Grobal2.LA_UNDEAD) && (BaseObject != null))
            {
                nDamage += m_AddAbil.btUndead;
            }

            if ((nDamage > 0) && AbilMagBubbleDefence)
            {
                nDamage = HUtil32.Round(nDamage / 1.0e2 * (MagBubbleDefenceLevel + 2) * 8.0);
                DamageBubbleDefence(nDamage);
            }

            return nDamage;
        }

        public void StruckDamage(int nDamage)
        {
            int nDam;
            int nDura;
            int nOldDura;
            PlayObject PlayObject;
            StdItem StdItem;
            bool bo19;
            if (nDamage <= 0)
            {
                return;
            }

            if ((Race >= 50) && (LastHiter != null) &&
                (LastHiter.Race == Grobal2.RC_PLAYOBJECT)) // 人攻击怪物
            {
                switch (LastHiter.Job)
                {
                    case PlayJob.Warrior:
                        nDamage = nDamage * M2Share.g_Config.nWarrMon / 10;
                        break;
                    case PlayJob.Wizard:
                        nDamage = nDamage * M2Share.g_Config.nWizardMon / 10;
                        break;
                    case PlayJob.Taoist:
                        nDamage = nDamage * M2Share.g_Config.nTaosMon / 10;
                        break;
                }
            }

            if ((Race == Grobal2.RC_PLAYOBJECT) && (LastHiter != null) &&
                (LastHiter.Master != null)) // 怪物攻击人
            {
                nDamage = nDamage * M2Share.g_Config.nMonHum / 10;
            }

            nDam = M2Share.RandomNumber.Random(10) + 5; // 1 0x62
            if (m_wStatusTimeArr[Grobal2.POISON_DAMAGEARMOR] > 0)
            {
                nDam = HUtil32.Round(nDam * (M2Share.g_Config.nPosionDamagarmor / 10)); // 1.2
                nDamage = HUtil32.Round(nDamage * (M2Share.g_Config.nPosionDamagarmor / 10)); // 1.2
            }

            bo19 = false;
            if (UseItems[Grobal2.U_DRESS] != null && UseItems[Grobal2.U_DRESS].wIndex > 0)
            {
                nDura = UseItems[Grobal2.U_DRESS].Dura;
                nOldDura = HUtil32.Round(nDura / 1000);
                nDura -= nDam;
                if (nDura <= 0)
                {
                    if (Race == Grobal2.RC_PLAYOBJECT)
                    {
                        PlayObject = this as PlayObject;
                        PlayObject.SendDelItems(UseItems[Grobal2.U_DRESS]);
                        StdItem = M2Share.UserEngine.GetStdItem(UseItems[Grobal2.U_DRESS].wIndex);
                        if (StdItem.NeedIdentify == 1)
                        {
                            M2Share.AddGameDataLog('3' + "\t" + MapName + "\t" + CurrX + "\t" + CurrY + "\t" +
                                                   CharName + "\t" + StdItem.Name + "\t" +
                                                   UseItems[Grobal2.U_DRESS].MakeIndex + "\t"
                                                   + HUtil32.BoolToIntStr(Race == Grobal2.RC_PLAYOBJECT) +
                                                   "\t" + '0');
                        }

                        UseItems[Grobal2.U_DRESS].wIndex = 0;
                        FeatureChanged();
                    }

                    UseItems[Grobal2.U_DRESS].wIndex = 0;
                    UseItems[Grobal2.U_DRESS].Dura = 0;
                    bo19 = true;
                }
                else
                {
                    UseItems[Grobal2.U_DRESS].Dura = (ushort)nDura;
                }

                if (nOldDura != HUtil32.Round(nDura / 1000))
                {
                    SendMsg(this, Grobal2.RM_DURACHANGE, Grobal2.U_DRESS, nDura, UseItems[Grobal2.U_DRESS].DuraMax, 0,
                        "");
                }
            }

            for (var i = 0; i < UseItems.Length; i++)
            {
                if ((UseItems[i] != null) && (UseItems[i].wIndex > 0) && (M2Share.RandomNumber.Random(8) == 0))
                {
                    nDura = UseItems[i].Dura;
                    nOldDura = HUtil32.Round(nDura / 1000);
                    nDura -= nDam;
                    if (nDura <= 0)
                    {
                        if (Race == Grobal2.RC_PLAYOBJECT)
                        {
                            PlayObject = this as PlayObject;
                            PlayObject.SendDelItems(UseItems[i]);
                            StdItem = M2Share.UserEngine.GetStdItem(UseItems[i].wIndex);
                            if (StdItem.NeedIdentify == 1)
                            {
                                M2Share.AddGameDataLog('3' + "\t" + MapName + "\t" + CurrX + "\t" + CurrY +
                                                       "\t" + CharName + "\t" + StdItem.Name + "\t" +
                                                       UseItems[i].MakeIndex + "\t"
                                                       + HUtil32.BoolToIntStr(Race == Grobal2.RC_PLAYOBJECT) +
                                                       "\t" + '0');
                            }

                            UseItems[i].wIndex = 0;
                            FeatureChanged();
                        }

                        UseItems[i].wIndex = 0;
                        UseItems[i].Dura = 0;
                        bo19 = true;
                    }
                    else
                    {
                        UseItems[i].Dura = (ushort)nDura;
                    }

                    if (nOldDura != HUtil32.Round(nDura / 1000))
                    {
                        SendMsg(this, Grobal2.RM_DURACHANGE, i, nDura, UseItems[i].DuraMax, 0, "");
                    }
                }
            }

            if (bo19)
            {
                RecalcAbilitys();
            }

            DamageHealth(nDamage);
        }

        public virtual string GeTBaseObjectInfo()
        {
            string result = CharName + ' ' + "地图:" + MapName + '(' + Envir.MapDesc + ") " + "座标:" + CurrX +
                            '/' + CurrY + ' ' + "等级:" + Abil.Level + ' ' + "经验:" + Abil.Exp + ' '
                            + "生命值: " + m_WAbil.HP + '-' + m_WAbil.MaxHP + ' ' + "魔法值: " + m_WAbil.MP + '-' +
                            m_WAbil.MaxMP + ' ' + "攻击力: " + HUtil32.LoWord(m_WAbil.DC) + '-' +
                            HUtil32.HiWord(m_WAbil.DC) + ' '
                            + "魔法力: " + HUtil32.LoWord(m_WAbil.MC) + '-' + HUtil32.HiWord(m_WAbil.MC) + ' ' + "道术: " +
                            HUtil32.LoWord(m_WAbil.SC) + '-' + HUtil32.HiWord(m_WAbil.SC) + ' '
                            + "防御力: " + HUtil32.LoWord(m_WAbil.AC) + '-' + HUtil32.HiWord(m_WAbil.AC) + ' ' + "魔防力: " +
                            HUtil32.LoWord(m_WAbil.MAC) + '-' + HUtil32.HiWord(m_WAbil.MAC) + ' ' + "准确:" +
                            m_btHitPoint + ' '
                            + "敏捷:" + SpeedPoint;
            return result;
        }

        public bool GetBackPosition(ref short nX, ref short nY)
        {
            bool result;
            Envirnoment Envir;
            Envir = this.Envir;
            nX = CurrX;
            nY = CurrY;
            switch (Direction)
            {
                case Grobal2.DR_UP:
                    if (nY < (Envir.Height - 1))
                    {
                        nY++;
                    }

                    break;
                case Grobal2.DR_DOWN:
                    if (nY > 0)
                    {
                        nY -= 1;
                    }

                    break;
                case Grobal2.DR_LEFT:
                    if (nX < (Envir.Width - 1))
                    {
                        nX++;
                    }

                    break;
                case Grobal2.DR_RIGHT:
                    if (nX > 0)
                    {
                        nX -= 1;
                    }

                    break;
                case Grobal2.DR_UPLEFT:
                    if ((nX < (Envir.Width - 1)) && (nY < (Envir.Height - 1)))
                    {
                        nX++;
                        nY++;
                    }

                    break;
                case Grobal2.DR_UPRIGHT:
                    if ((nX < (Envir.Width - 1)) && (nY > 0))
                    {
                        nX -= 1;
                        nY++;
                    }

                    break;
                case Grobal2.DR_DOWNLEFT:
                    if ((nX > 0) && (nY < (Envir.Height - 1)))
                    {
                        nX++;
                        nY -= 1;
                    }

                    break;
                case Grobal2.DR_DOWNRIGHT:
                    if ((nX > 0) && (nY > 0))
                    {
                        nX -= 1;
                        nY -= 1;
                    }

                    break;
            }

            result = true;
            return result;
        }

        public bool MakePosion(int nType, int nTime, int nPoint)
        {
            bool result = false;
            if (nType < Grobal2.MAX_STATUS_ATTRIBUTE)
            {
                var nOldCharStatus = CharStatus;
                if (m_wStatusTimeArr[nType] > 0)
                {
                    if (m_wStatusTimeArr[nType] < nTime)
                    {
                        m_wStatusTimeArr[nType] = (ushort)nTime;
                    }
                }
                else
                {
                    m_wStatusTimeArr[nType] = (ushort)nTime;
                }

                m_dwStatusArrTick[nType] = HUtil32.GetTickCount();
                CharStatus = GetCharStatus();
                GreenPoisoningPoint = (byte)nPoint;
                if (nOldCharStatus != CharStatus)
                {
                    StatusChanged();
                }

                if (Race == Grobal2.RC_PLAYOBJECT)
                {
                    SysMsg(format(M2Share.sYouPoisoned, nTime, nPoint), MsgColor.Red, MsgType.Hint);
                }

                result = true;
            }

            return result;
        }

        /// <summary>
        /// 检查是否正有跨服数据
        /// </summary>
        /// <returns></returns>
        public bool CheckServerMakeSlave()
        {
            bool result = false;
            HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            try
            {
                for (var i = 0; i < MsgList.Count; i++)
                {
                    if (MsgList[i].wIdent == Grobal2.RM_10401)
                    {
                        result = true;
                        break;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }

            return result;
        }

        protected bool GetRecallXY(short nX, short nY, int nRange, ref short nDX, ref short nDY)
        {
            bool result = false;
            if (Envir.GetMovingObject(nX, nY, true) == null)
            {
                result = true;
                nDX = nX;
                nDY = nY;
            }

            if (!result)
            {
                for (int i = 0; i < nRange; i++)
                {
                    for (int j = -i; j <= i; j++)
                    {
                        for (int k = -i; k <= i; k++)
                        {
                            nDX = (short)(nX + k);
                            nDY = (short)(nY + j);
                            if (Envir.GetMovingObject(nDX, nDY, true) == null)
                            {
                                result = true;
                                break;
                            }
                        }

                        if (result)
                        {
                            break;
                        }
                    }

                    if (result)
                    {
                        break;
                    }
                }
            }

            if (!result)
            {
                nDX = nX;
                nDY = nY;
            }

            return result;
        }

        public bool IsTrainingSkill(int nIndex)
        {
            bool result = false;
            TUserMagic UserMagic;
            for (int i = 0; i < MagicList.Count; i++)
            {
                UserMagic = MagicList[i];
                if ((UserMagic != null) && (UserMagic.wMagIdx == nIndex))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        private void DamageBubbleDefence(int nInt)
        {
            if (m_wStatusTimeArr[Grobal2.STATE_BUBBLEDEFENCEUP] > 0)
            {
                if (m_wStatusTimeArr[Grobal2.STATE_BUBBLEDEFENCEUP] > 3)
                {
                    m_wStatusTimeArr[Grobal2.STATE_BUBBLEDEFENCEUP] -= 3;
                }
                else
                {
                    m_wStatusTimeArr[Grobal2.STATE_BUBBLEDEFENCEUP] = 1;
                }
            }
        }

        public bool IsGuildMaster()
        {
            return (MyGuild != null) && (GuildRankNo == 1);
        }

        public bool MagCanHitTarget(short nX, short nY, TBaseObject TargeTBaseObject)
        {
            bool result = false;
            int n18;
            if (TargeTBaseObject == null)
            {
                return result;
            }

            int n20 = Math.Abs(nX - TargeTBaseObject.CurrX) + Math.Abs(nY - TargeTBaseObject.CurrY);
            int n14 = 0;
            while (n14 < 13)
            {
                n18 = M2Share.GetNextDirection(nX, nY, TargeTBaseObject.CurrX, TargeTBaseObject.CurrY);
                if (Envir.GetNextPosition(nX, nY, n18, 1, ref nX, ref nY) && Envir.IsValidCell(nX, nY))
                {
                    if ((nX == TargeTBaseObject.CurrX) && (nY == TargeTBaseObject.CurrY))
                    {
                        result = true;
                        break;
                    }
                    else
                    {
                        int n1C = Math.Abs(nX - TargeTBaseObject.CurrX) + Math.Abs(nY - TargeTBaseObject.CurrY);
                        if (n1C > n20)
                        {
                            result = true;
                            break;
                        }

                        n1C = n20;
                    }
                }
                else
                {
                    break;
                }

                n14++;
            }

            return result;
        }

        private bool IsProperFriend_IsFriend(TBaseObject cret)
        {
            bool result = false;
            if (cret.Race == Grobal2.RC_PLAYOBJECT)
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
                        if ((this == cret) || (cret == (this as PlayObject).m_DearHuman))
                        {
                            result = true;
                        }

                        break;
                    case AttackMode.HAM_MASTER:
                        if (this == cret)
                        {
                            result = true;
                        }
                        else if ((this as PlayObject).m_boMaster)
                        {
                            for (int i = 0; i < (this as PlayObject).m_MasterList.Count; i++)
                            {
                                if ((this as PlayObject).m_MasterList[i] == cret)
                                {
                                    result = true;
                                    break;
                                }
                            }
                        }
                        else if ((cret as PlayObject).m_boMaster)
                        {
                            for (int i = 0; i < (cret as PlayObject).m_MasterList.Count; i++)
                            {
                                if ((cret as PlayObject).m_MasterList[i] == this)
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
                            if (MyGuild.IsMember(cret.CharName))
                            {
                                result = true;
                            }

                            if (GuildWarArea && (cret.MyGuild != null))
                            {
                                if (MyGuild.IsAllyGuild(cret.MyGuild))
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

                        if (PKLevel() >= 2)
                        {
                            if (cret.PKLevel() < 2)
                            {
                                result = true;
                            }
                        }
                        else
                        {
                            if (cret.PKLevel() >= 2)
                            {
                                result = true;
                            }
                        }

                        break;
                }
            }

            return result;
        }

        public int MagMakeDefenceArea(int nX, int nY, int nRange, ushort nSec, byte btState)
        {
            int result = 0;
            int nStartX = nX - nRange;
            int nEndX = nX + nRange;
            int nStartY = nY - nRange;
            int nEndY = nY + nRange;
            for (int i = nStartX; i <= nEndX; i++)
            {
                for (int j = nStartY; j <= nEndY; j++)
                {
                    var cellsuccess = false;
                    var cellInfo = Envir.GetCellInfo(i, j, ref cellsuccess);
                    if (cellsuccess && (cellInfo.ObjList != null))
                    {
                        for (var k = 0; k < cellInfo.Count; k++)
                        {
                            var OSObject = cellInfo.ObjList[k];
                            if ((OSObject != null) && (OSObject.CellType == CellType.MovingObject))
                            {
                                var BaseObject = M2Share.ActorMgr.Get(OSObject.CellObjId);
                                ;
                                if ((BaseObject != null) && (!BaseObject.Ghost))
                                {
                                    if (IsProperFriend(BaseObject))
                                    {
                                        if (btState == 0)
                                        {
                                            BaseObject.DefenceUp(nSec);
                                        }
                                        else
                                        {
                                            BaseObject.MagDefenceUp(nSec);
                                        }

                                        result++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        private bool DefenceUp(ushort nSec)
        {
            bool result = false;
            if (m_wStatusTimeArr[Grobal2.STATE_DEFENCEUP] > 0)
            {
                if (m_wStatusTimeArr[Grobal2.STATE_DEFENCEUP] < nSec)
                {
                    m_wStatusTimeArr[Grobal2.STATE_DEFENCEUP] = nSec;
                    result = true;
                }
            }
            else
            {
                m_wStatusTimeArr[Grobal2.STATE_DEFENCEUP] = nSec;
                result = true;
            }

            m_dwStatusArrTick[Grobal2.STATE_DEFENCEUP] = HUtil32.GetTickCount();
            SysMsg(format(M2Share.g_sDefenceUpTime, nSec), MsgColor.Green, MsgType.Hint);
            RecalcAbilitys();
            SendMsg(this, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
            return result;
        }

        public bool AttPowerUp(int nPower, int nTime)
        {
            m_wStatusArrValue[0] = (ushort)nPower;
            m_dwStatusArrTimeOutTick[0] = HUtil32.GetTickCount() + nTime * 1000;
            int nMin = nTime / 60;
            int nSec = nTime % 60;
            SysMsg(format(M2Share.g_sAttPowerUpTime, nMin, nSec), MsgColor.Green, MsgType.Hint);
            RecalcAbilitys();
            SendMsg(this, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
            return true;
        }

        private bool MagDefenceUp(ushort nSec)
        {
            bool result = false;
            if (m_wStatusTimeArr[Grobal2.STATE_MAGDEFENCEUP] > 0)
            {
                if (m_wStatusTimeArr[Grobal2.STATE_MAGDEFENCEUP] < nSec)
                {
                    m_wStatusTimeArr[Grobal2.STATE_MAGDEFENCEUP] = nSec;
                    result = true;
                }
            }
            else
            {
                m_wStatusTimeArr[Grobal2.STATE_MAGDEFENCEUP] = nSec;
                result = true;
            }

            m_dwStatusArrTick[Grobal2.STATE_MAGDEFENCEUP] = HUtil32.GetTickCount();
            SysMsg(format(M2Share.g_sMagDefenceUpTime, nSec), MsgColor.Green, MsgType.Hint);
            RecalcAbilitys();
            SendMsg(this, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
            return result;
        }

        /// <summary>
        /// 魔法盾
        /// </summary>
        /// <returns></returns>
        public bool MagBubbleDefenceUp(byte nLevel, ushort nSec)
        {
            if (m_wStatusTimeArr[Grobal2.STATE_BUBBLEDEFENCEUP] != 0)
            {
                return false;
            }

            var nOldStatus = CharStatus;
            m_wStatusTimeArr[Grobal2.STATE_BUBBLEDEFENCEUP] = nSec;
            m_dwStatusArrTick[Grobal2.STATE_BUBBLEDEFENCEUP] = HUtil32.GetTickCount();
            CharStatus = GetCharStatus();
            if (nOldStatus != CharStatus)
            {
                StatusChanged();
            }

            AbilMagBubbleDefence = true;
            MagBubbleDefenceLevel = nLevel;
            return true;
        }

        public TUserItem CheckItemCount(string sItemName, ref int nCount)
        {
            TUserItem result = null;
            nCount = 0;
            for (int i = 0; i < UseItems.Length; i++)
            {
                if (UseItems[i] == null)
                {
                    continue;
                }

                var sName = M2Share.UserEngine.GetStdItemName(UseItems[i].wIndex);
                if (string.Compare(sName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = UseItems[i];
                    nCount++;
                }
            }

            return result;
        }

        public TUserItem CheckItems(string sItemName)
        {
            TUserItem result = null;
            TUserItem UserItem;
            for (int i = 0; i < ItemList.Count; i++)
            {
                UserItem = ItemList[i];
                if (UserItem == null)
                {
                    continue;
                }

                if (string.Compare(M2Share.UserEngine.GetStdItemName(UserItem.wIndex), sItemName,
                        StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = UserItem;
                    break;
                }
            }

            return result;
        }

        protected void DelBagItem(int nIndex)
        {
            if ((nIndex < 0) || (nIndex >= ItemList.Count))
            {
                return;
            }

            Dispose(ItemList[nIndex]);
            ItemList.RemoveAt(nIndex);
        }

        public bool DelBagItem(int nItemIndex, string sItemName)
        {
            TUserItem UserItem;
            bool result = false;
            for (int i = 0; i < ItemList.Count; i++)
            {
                UserItem = ItemList[i];
                if ((UserItem.MakeIndex == nItemIndex) &&
                    string.Compare(M2Share.UserEngine.GetStdItemName(UserItem.wIndex), sItemName,
                        StringComparison.OrdinalIgnoreCase) == 0)
                {
                    Dispose(UserItem);
                    ItemList.RemoveAt(i);
                    result = true;
                    break;
                }
            }

            if (result)
            {
                WeightChanged();
            }

            return result;
        }

        public bool CanMove(short nX, short nY, bool boFlag)
        {
            if (Math.Abs(CurrX - nX) <= 1 && Math.Abs(CurrX - nY) <= 1)
            {
                return Envir.CanWalkEx(nX, nY, boFlag);
            }

            return CanRun(nX, nY, boFlag);
        }

        public bool CanMove(short nCurrX, short nCurrY, short nX, short nY, bool boFlag)
        {
            if ((Math.Abs(nCurrX - nX) <= 1) && (Math.Abs(nCurrY - nY) <= 1))
            {
                return Envir.CanWalkEx(nX, nY, boFlag);
            }
            else
            {
                return CanRun(nCurrX, nCurrY, nX, nY, boFlag);
            }
        }

        public bool CanRun(short nCurrX, short nCurrY, short nX, short nY, bool boFlag)
        {
            var result = false;
            var btDir = M2Share.GetNextDirection(nCurrX, nCurrY, nX, nY);
            switch (btDir)
            {
                case Grobal2.DR_UP:
                    if (nCurrY > 1)
                    {
                        if ((Envir.CanWalkEx(nCurrX, nCurrY - 1,
                                 M2Share.g_Config.boDiableHumanRun ||
                                 ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                             (M2Share.g_Config.boSafeAreaLimited && InSafeZone()))
                            && (Envir.CanWalkEx(nCurrX, nCurrY - 2,
                                    M2Share.g_Config.boDiableHumanRun ||
                                    ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                                (M2Share.g_Config.boSafeAreaLimited && InSafeZone())))
                        {
                            result = true;
                            return result;
                        }
                    }

                    break;
                case Grobal2.DR_UPRIGHT:
                    if (nCurrX < Envir.Width - 2 && nCurrY > 1)
                    {
                        if ((Envir.CanWalkEx(nCurrX + 1, nCurrY - 1,
                                 M2Share.g_Config.boDiableHumanRun ||
                                 ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                             (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
                            (Envir.CanWalkEx(nCurrX + 2, nCurrY - 2,
                                 M2Share.g_Config.boDiableHumanRun ||
                                 ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                             (M2Share.g_Config.boSafeAreaLimited && InSafeZone())))
                        {
                            result = true;
                            return result;
                        }
                    }

                    break;
                case Grobal2.DR_RIGHT:
                    if (nCurrX < Envir.Width - 2)
                    {
                        if (Envir.CanWalkEx(nCurrX + 1, nCurrY,
                                M2Share.g_Config.boDiableHumanRun ||
                                ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                            (M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
                            (Envir.CanWalkEx(nCurrX + 2, nCurrY,
                                 M2Share.g_Config.boDiableHumanRun ||
                                 ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                             (M2Share.g_Config.boSafeAreaLimited && InSafeZone())))
                        {
                            result = true;
                            return result;
                        }
                    }

                    break;
                case Grobal2.DR_DOWNRIGHT:
                    if ((nCurrX < Envir.Width - 2) && (nCurrY < Envir.Height - 2) &&
                        (Envir.CanWalkEx(nCurrX + 1, nCurrY + 1,
                             M2Share.g_Config.boDiableHumanRun || ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                         M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
                        (Envir.CanWalkEx(nCurrX + 2, nCurrY + 2,
                             M2Share.g_Config.boDiableHumanRun || ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                         (M2Share.g_Config.boSafeAreaLimited && InSafeZone())))
                    {
                        result = true;
                        return result;
                    }

                    break;
                case Grobal2.DR_DOWN:
                    if ((nCurrY < Envir.Height - 2) &&
                        (Envir.CanWalkEx(nCurrX, nCurrY + 1,
                             M2Share.g_Config.boDiableHumanRun ||
                             ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                         (M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
                         (Envir.CanWalkEx(nCurrX, nCurrY + 2,
                              M2Share.g_Config.boDiableHumanRun ||
                              ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                          (M2Share.g_Config.boSafeAreaLimited && InSafeZone()))))
                    {
                        result = true;
                        return result;
                    }

                    break;
                case Grobal2.DR_DOWNLEFT:
                    if ((nCurrX > 1) && (nCurrY < Envir.Height - 2) &&
                        (Envir.CanWalkEx(nCurrX - 1, nCurrY + 1,
                             M2Share.g_Config.boDiableHumanRun || ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                         (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
                        (Envir.CanWalkEx(nCurrX - 2, nCurrY + 2,
                             M2Share.g_Config.boDiableHumanRun || ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                         (M2Share.g_Config.boSafeAreaLimited && InSafeZone())))
                    {
                        result = true;
                        return result;
                    }

                    break;
                case Grobal2.DR_LEFT:
                    if ((nCurrX > 1) &&
                        (Envir.CanWalkEx(nCurrX - 1, nCurrY,
                             M2Share.g_Config.boDiableHumanRun || ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                         (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
                        (Envir.CanWalkEx(nCurrX - 2, nCurrY,
                             M2Share.g_Config.boDiableHumanRun || ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                         (M2Share.g_Config.boSafeAreaLimited && InSafeZone())))
                    {
                        result = true;
                        return result;
                    }

                    break;
                case Grobal2.DR_UPLEFT:
                    if ((nCurrX > 1) && (nCurrY > 1) && (Envir.CanWalkEx(nCurrX - 1, nCurrY - 1,
                                                             M2Share.g_Config.boDiableHumanRun ||
                                                             ((Permission > 9) && M2Share.g_Config.boGMRunAll))
                                                         || (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
                        (Envir.CanWalkEx(nCurrX - 2, nCurrY - 2,
                             M2Share.g_Config.boDiableHumanRun || ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                         (M2Share.g_Config.boSafeAreaLimited && InSafeZone())))
                    {
                        result = true;
                        return result;
                    }

                    break;
            }

            return false;
        }

        private bool CanRun(short nX, short nY, bool boFlag)
        {
            var result = false;
            var btDir = M2Share.GetNextDirection(CurrX, CurrY, nX, nY);
            switch (btDir)
            {
                case Grobal2.DR_UP:
                    if (CurrY > 1)
                    {
                        if ((Envir.CanWalkEx(CurrX, CurrY - 1,
                                 M2Share.g_Config.boDiableHumanRun ||
                                 ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                             (M2Share.g_Config.boSafeAreaLimited && InSafeZone()))
                            && (Envir.CanWalkEx(CurrX, CurrY - 2,
                                    M2Share.g_Config.boDiableHumanRun ||
                                    ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                                (M2Share.g_Config.boSafeAreaLimited && InSafeZone())))
                        {
                            result = true;
                            return result;
                        }
                    }

                    break;
                case Grobal2.DR_UPRIGHT:
                    if (CurrX < Envir.Width - 2 && CurrY > 1)
                    {
                        if ((Envir.CanWalkEx(CurrX + 1, CurrY - 1,
                                 M2Share.g_Config.boDiableHumanRun ||
                                 ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                             (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
                            (Envir.CanWalkEx(CurrX + 2, CurrY - 2,
                                 M2Share.g_Config.boDiableHumanRun ||
                                 ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                             (M2Share.g_Config.boSafeAreaLimited && InSafeZone())))
                        {
                            result = true;
                            return result;
                        }
                    }

                    break;
                case Grobal2.DR_RIGHT:
                    if (CurrX < Envir.Width - 2)
                    {
                        if (Envir.CanWalkEx(CurrX + 1, CurrY,
                                M2Share.g_Config.boDiableHumanRun ||
                                ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                            (M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
                            (Envir.CanWalkEx(CurrX + 2, CurrY,
                                 M2Share.g_Config.boDiableHumanRun ||
                                 ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                             (M2Share.g_Config.boSafeAreaLimited && InSafeZone())))
                        {
                            result = true;
                            return result;
                        }
                    }

                    break;
                case Grobal2.DR_DOWNRIGHT:
                    if ((CurrX < Envir.Width - 2) && (CurrY < Envir.Height - 2) &&
                        (Envir.CanWalkEx(CurrX + 1, CurrY + 1,
                             M2Share.g_Config.boDiableHumanRun || ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                         M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
                        (Envir.CanWalkEx(CurrX + 2, CurrY + 2,
                             M2Share.g_Config.boDiableHumanRun || ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                         (M2Share.g_Config.boSafeAreaLimited && InSafeZone())))
                    {
                        result = true;
                        return result;
                    }

                    break;
                case Grobal2.DR_DOWN:
                    if ((CurrY < Envir.Height - 2) &&
                        (Envir.CanWalkEx(CurrX, CurrY + 1,
                             M2Share.g_Config.boDiableHumanRun ||
                             ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                         (M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
                         (Envir.CanWalkEx(CurrX, CurrY + 2,
                              M2Share.g_Config.boDiableHumanRun ||
                              ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                          (M2Share.g_Config.boSafeAreaLimited && InSafeZone()))))
                    {
                        result = true;
                        return result;
                    }

                    break;
                case Grobal2.DR_DOWNLEFT:
                    if ((CurrX > 1) && (CurrY < Envir.Height - 2) &&
                        (Envir.CanWalkEx(CurrX - 1, CurrY + 1,
                             M2Share.g_Config.boDiableHumanRun || ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                         (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
                        (Envir.CanWalkEx(CurrX - 2, CurrY + 2,
                             M2Share.g_Config.boDiableHumanRun || ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                         (M2Share.g_Config.boSafeAreaLimited && InSafeZone())))
                    {
                        result = true;
                        return result;
                    }

                    break;
                case Grobal2.DR_LEFT:
                    if ((CurrX > 1) &&
                        (Envir.CanWalkEx(CurrX - 1, CurrY,
                             M2Share.g_Config.boDiableHumanRun || ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                         (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
                        (Envir.CanWalkEx(CurrX - 2, CurrY,
                             M2Share.g_Config.boDiableHumanRun || ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                         (M2Share.g_Config.boSafeAreaLimited && InSafeZone())))
                    {
                        result = true;
                        return result;
                    }

                    break;
                case Grobal2.DR_UPLEFT:
                    if ((CurrX > 1) && (CurrY > 1) && (Envir.CanWalkEx(CurrX - 1, CurrY - 1,
                                                                 M2Share.g_Config.boDiableHumanRun ||
                                                                 ((Permission > 9) && M2Share.g_Config.boGMRunAll))
                                                             || (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
                        (Envir.CanWalkEx(CurrX - 2, CurrY - 2,
                             M2Share.g_Config.boDiableHumanRun || ((Permission > 9) && M2Share.g_Config.boGMRunAll)) ||
                         (M2Share.g_Config.boSafeAreaLimited && InSafeZone())))
                    {
                        result = true;
                        return result;
                    }

                    break;
            }

            return false;
        }

        public TBaseObject GetMaster()
        {
            if (Race != Grobal2.RC_PLAYOBJECT)
            {
                TBaseObject MasterObject = Master;
                if (MasterObject != null)
                {
                    while (true)
                    {
                        if (MasterObject.Master != null)
                        {
                            MasterObject = MasterObject.Master;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                return MasterObject;
            }

            return null;
        }

        public bool ReAliveEx(MonGenInfo MonGen)
        {
            m_WAbil = Abil;
            Gold = 0;
            //m_boStrike = false;
            m_boNoItem = false;
            StoneMode = false;
            m_boSkeleton = false;
            m_boHolySeize = false;
            m_boCrazyMode = false;
            m_boShowHP = false;
            //m_boPlayerDupMode = false;
            FixedHideMode = false;

            if (this is CastleDoor)
            {
                ((CastleDoor)(this)).m_boOpened = false;
                this.m_boStickMode = true;
            }

            if (this is MagicMonster)
            {
                ((MagicMonster)(this)).m_boDupMode = false;
            }

            if (this is MagicMonObject)
            {
                ((MagicMonObject)(this)).m_boUseMagic = false;
            }

            if (this is RockManObject)
            {
                this.HideMode = false;
            }

            if (this is WallStructure)
            {
                ((WallStructure)(this)).boSetMapFlaged = false;
            }

            if (this is SoccerBall)
            {
                ((SoccerBall)(this)).n550 = 0;
                ((SoccerBall)(this)).m_nTargetX = -1;
            }

            if (this is FrostTiger)
            {
                //((TFrostTiger)(this)).m_boApproach = false;
            }

            if (this is CowKingMonster)
            {
                /*((TCowKingMonster)(this)).m_boCowKingMon = true;
                ((TCowKingMonster)(this)).m_nDangerLevel = 0;
                ((TCowKingMonster)(this)).m_boDanger = false;
                ((TCowKingMonster)(this)).m_boCrazy = false;*/
            }

            if (this is DigOutZombi)
            {
                this.FixedHideMode = true;
            }

            if (this is WhiteSkeleton)
            {
                ((WhiteSkeleton)(this)).m_boIsFirst = true;
                this.FixedHideMode = true;
            }

            if (this is ScultureMonster)
            {
                this.FixedHideMode = true;
            }

            if (this is ScultureKingMonster)
            {
                this.StoneMode = true;
                this.m_nCharStatusEx = Grobal2.STATE_STONE_MODE;
            }

            if (this is ElfMonster)
            {
                this.FixedHideMode = true;
                this.m_boNoAttackMode = true;
                ((ElfMonster)(this)).boIsFirst = true;
            }

            if (this is ElfWarriorMonster)
            {
                this.FixedHideMode = true;
                ((ElfWarriorMonster)(this)).boIsFirst = true;
                ((ElfWarriorMonster)(this)).m_boUsePoison = false;
            }

            if (this is ElectronicScolpionMon)
            {
                ((ElectronicScolpionMon)(this)).m_boUseMagic = false;
                //((TElectronicScolpionMon)(this)).m_boApproach = false;
            }

            if (this is DoubleCriticalMonster)
            {
                //((TDoubleCriticalMonster)(this)).m_n7A0 = 0;
            }

            if (this is StickMonster)
            {
                this.SearchTick = HUtil32.GetTickCount();
                this.FixedHideMode = true;
                this.m_boStickMode = true;
            }

            m_nMeatQuality = (ushort)(M2Share.RandomNumber.Random(3500) + 3000);
            //m_nBodyLeathery = m_nPerBodyLeathery;
            ProcessRunCount = 0;
            //m_nPushedCount = 0;
            //m_nBodyState = 0;

            switch (this.Race)
            {
                case 51:
                    m_nMeatQuality = (ushort)(M2Share.RandomNumber.Random(3500) + 3000);
                    m_nBodyLeathery = 50;
                    break;
                case 52:
                    if (M2Share.RandomNumber.Random(30) == 0)
                    {
                        m_nMeatQuality = (ushort)(M2Share.RandomNumber.Random(20000) + 10000);
                        m_nBodyLeathery = 150;
                    }
                    else
                    {
                        m_nMeatQuality = (ushort)(M2Share.RandomNumber.Random(8000) + 8000);
                        m_nBodyLeathery = 150;
                    }

                    break;
                case 53:
                    m_nMeatQuality = (ushort)(M2Share.RandomNumber.Random(8000) + 8000);
                    m_nBodyLeathery = 150;
                    break;
                case 54:
                    Animal = true;
                    break;
                case 95:
                    if (M2Share.RandomNumber.Random(2) == 0)
                    {
                        // m_boSafeWalk = true;
                    }

                    break;
                case 96:
                    if (M2Share.RandomNumber.Random(4) == 0)
                    {
                        // m_boSafeWalk = true;
                    }

                    break;
                case 97:
                    if (M2Share.RandomNumber.Random(2) == 0)
                    {
                        // m_boSafeWalk = true;
                    }

                    break;
                case 169:
                    m_boStickMode = false;
                    break;
                case 170:
                    m_boStickMode = true;
                    break;
            }

            UseItems = new TUserItem[8];
            for (int i = 0; i < ItemList.Count; i++)
            {
                ItemList[i] = null;
            }

            ItemList.Clear();

            OnEnvirnomentChanged();
            CharStatus = GetCharStatus();
            StatusChanged();
            if (Envir == null)
            {
                return false;
            }

            var nX = (MonGen.nX - MonGen.nRange) + M2Share.RandomNumber.Random(MonGen.nRange * 2 + 1);
            var nY = (MonGen.nY - MonGen.nRange) + M2Share.RandomNumber.Random(MonGen.nRange * 2 + 1);
            var m_boErrorOnInit = true;
            if (Envir.CanWalk(nX, nY, true))
            {
                CurrX = (short)nX;
                CurrY = (short)nY;
                if (AddToMap())
                {
                    m_boErrorOnInit = false;
                }
            }

            var nRange = 0;
            var nRange2 = 0;
            if (m_boErrorOnInit)
            {
                if (Envir.Width < 50)
                {
                    nRange = 2;
                }
                else
                {
                    nRange = 3;
                }

                if ((Envir.Height < 250))
                {
                    if ((Envir.Height < 30))
                    {
                        nRange2 = 2;
                    }
                    else
                    {
                        nRange2 = 20;
                    }
                }
                else
                {
                    nRange2 = 50;
                }
            }

            var nC = 0;
            object addObj = null;
            var nX2 = CurrX;
            var nY2 = CurrY;
            while (true)
            {
                if (!Envir.CanWalk(nX, nY, false))
                {
                    if ((Envir.Width - nRange2 - 1) > nX)
                    {
                        nX = nX + nRange;
                    }
                    else
                    {
                        nX = M2Share.RandomNumber.Random(Envir.Width / 2) + nRange2;
                    }

                    if (Envir.Height - nRange2 - 1 > nY)
                    {
                        nY = nY + nRange;
                    }
                    else
                    {
                        nY = M2Share.RandomNumber.Random(Envir.Height / 2) + nRange2;
                    }
                }
                else
                {
                    CurrX = (short)nX;
                    CurrY = (short)nY;
                    addObj = Envir.AddToMap(nX, nY, CellType.MovingObject, this);
                    break;
                }

                nC++;
                if (nC > 46)
                {
                    break;
                }
            }

            if (addObj == null)
            {
                CurrX = nX2;
                CurrY = nY2;
                Envir.AddToMap(CurrX, CurrY, CellType.MovingObject, this);
            }

            Abil.HP = Abil.MaxHP;
            Abil.MP = Abil.MaxMP;
            m_WAbil.HP = m_WAbil.MaxHP;
            m_WAbil.MP = m_WAbil.MaxMP;

            RecalcAbilitys();

            Death = false;
            m_boInvisible = false;

            SendRefMsg(Grobal2.RM_TURN, Direction, CurrX, CurrY, GetFeatureToLong(), "");

            if (M2Share.g_Config.boMonSayMsg)
            {
                MonsterSayMsg(null, MonStatus.MonGen);
            }

            return true;
        }

        internal void OnEnvirnomentChanged()
        {
            if (m_boCanReAlive)
            {
                if ((MonGen != null) && (MonGen.Envir != Envir))
                {
                    m_boCanReAlive = false;
                    if (MonGen.nActiveCount > 0)
                    {
                        MonGen.nActiveCount--;
                    }

                    MonGen = null;
                }
            }
            //if ((m_PEnvir != null))
            //{
            //    if (m_nLastMapSecret != m_PEnvir.Flag.nSecret)
            //    {
            //        if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
            //        {
            //            if ((m_btRaceServer = Grobal2.RC_PLAYOBJECT) && (m_nLastMapSecret != -1))
            //            {
            //                var i = GetFeatureToLong();
            //                var sSENDMSG = string.Empty;
            //                var nSafeX = GetTitleIndex();
            //                if (nSafeX > 0)
            //                {
            //                    var MessageBodyW = new TMessageBodyW();
            //                    MessageBodyW.Param1 = HUtil32.MakeWord(nSafeX, 0);
            //                    MessageBodyW.Param2 = 0;
            //                    MessageBodyW.Tag1 = 0;
            //                    MessageBodyW.Tag2 = 0;
            //                    sSENDMSG = EDcode.EncodeBuffer(@MessageBodyW);
            //                }
            //                ((TPlayObject)(this)).m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_FEATURECHANGED, this.ObjectId, HUtil32.LoWord(i), HUtil32.HiWord(i), GetFeatureEx());
            //                ((TPlayObject)(this)).SendSocket(((TPlayObject)(this)).m_DefMsg, sSENDMSG);
            //                ((TPlayObject)(this)).InternalPowerPointChanged();
            //                SendUpdateMsg(this, Grobal2.RM_USERNAME, 0, 0, 0, 0, GetShowName());
            //            }
            //            HealthSpellChanged();
            //        }
            //        m_nLastMapSecret = m_PEnvir.Flag.nSecret;
            //    }
            //}
            //m_nCurEnvirIdx = -1;
            //m_nCastleEnvirListIdx = -1;
            //m_CurSafeZoneList.Clear();
            //for (int i = 0; i < M2Share.StartPointList.Count; i++)
            //{
            //    var StartPointInfo = M2Share.StartPointList[i];
            //    if (StartPointInfo.m_sMapName == m_PEnvir.sMapName)
            //    {
            //        m_CurSafeZoneList.Add(StartPointInfo);
            //    }
            //}
            //if ((m_btRaceServer == Grobal2.RC_PLAYOBJECT) && !((TPlayObject)(this)).m_boOffLineFlag)
            //{
            //   ((TPlayObject)(this)).CheckMapEvent(5, "");
            //}
        }

        internal void Dispose(object obj)
        {
            obj = null;
        }

        internal string format(string str, params object[] par)
        {
            return string.Format(str, par);
        }
    }
}