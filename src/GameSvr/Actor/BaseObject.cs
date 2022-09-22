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
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;
using SystemModule.Packet.ServerPackets;

namespace GameSvr.Actor
{
    public partial class BaseObject : EntityId
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
        /// <summary>
        /// 状态值
        /// </summary>
        public int CharStatus;
        protected int CharStatusEx;
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
        /// 骑马
        /// </summary>
        public bool OnHorse;
        /// <summary>
        /// 马类型
        /// </summary>
        public byte HorseType;
        /// <summary>
        /// /衣服特效(如天外飞仙衣服效果)
        /// </summary>
        private byte DressEffType;
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
        internal bool AllowGuild;
        public byte BtB2;
        public int IncHealth;
        public int IncSpell;
        public int IncHealing;
        private int IncHpStoneTime;
        private int IncMpStoneTime;
        /// <summary>
        /// 在行会占争地图中死亡次数
        /// </summary>
        public int FightZoneDieCount;
        public NakedAbility BonusAbil;
        private NakedAbility CurBonusAbil;
        public int BonusPoint = 0;
        public int HungerStatus = 0;
        public bool AllowGuildReCall = false;
        public double BodyLuck;
        public int BodyLuckLevel;
        public short GroupRcallTime;
        public bool AllowGroupReCall;
        public byte[] QuestUnitOpen;
        public byte[] QuestUnit;
        public byte[] QuestFlag;
        /// <summary>
        /// 怪物经验值
        /// </summary>
        public int FightExp = 0;
        /// <summary>
        /// 基本属性
        /// </summary>
        public Ability Abil;
        /// <summary>
        /// 角色属性
        /// </summary>
        public Ability WAbil;
        /// <summary>
        /// 附加属性
        /// </summary>
        private AddAbility AddAbil;
        /// <summary>
        /// 视觉范围大小
        /// </summary>
        protected byte ViewRange;
        /// <summary>
        /// 人物状态属性值 
        /// 0-绿毒(减HP) 1-红毒(减MP) 2-防、魔防为0(唯我独尊3级) 3-不能跑动(中蛛网)
        /// 4-不能移动(中战连击) 5-麻痹(石化) 6-减血，被连击技能万剑归宗击中后掉血
        /// 7-冰冻(不能跑动，不能魔法) 8-隐身 9-防御力(神圣战甲术) 10-魔御力(幽灵盾) 11-魔法盾
        /// </summary>
        internal ushort[] StatusArr;
        /// <summary>
        /// 人物状态持续的开始时间
        /// </summary>
        internal int[] StatusArrTick;
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
        /// 外观代码
        /// </summary>
        public ushort Appr;
        /// <summary>
        /// 角色类型
        /// </summary>
        public byte Race;
        /// <summary>
        /// 在地图上的类型
        /// </summary>
        public CellType MapCell;
        /// <summary>
        /// 角色外形
        /// </summary>
        public byte RaceImg;
        public bool BoRace;
        /// <summary>
        /// 人物攻击准确度
        /// </summary>
        public byte HitPoint;
        /// <summary>
        /// 额外攻击伤害(攻杀)
        /// </summary>
        private ushort HitPlus;
        /// <summary>
        /// 双倍攻击伤害(烈火专用)
        /// </summary>
        private ushort HitDouble;
        /// <summary>
        /// 记忆使用间隔
        /// </summary>
        public int GroupRcallTick;
        /// <summary>
        /// 记忆全套
        /// </summary>
        public bool RecallSuite;
        public ushort HealthRecover;
        public ushort SpellRecover;
        public byte AntiPoison;
        public ushort PoisonRecover;
        public ushort AntiMagic;
        /// <summary>
        /// 人物的幸运值
        /// </summary>
        public int Luck;
        public int PerHealth;
        public int PerHealing;
        public int PerSpell;
        /// <summary>
        /// 增加攻击的间隔
        /// </summary>
        public int IncHealthSpellTick;
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
        /// <summary>
        /// 不死系,1-为不死系
        /// </summary>
        public byte LifeAttrib;
        /// <summary>
        /// 否可以看到隐身人物(视线范围) 
        /// </summary>
        public byte CoolEyeCode = 0;
        /// <summary>
        /// 是否可以看到隐身人物
        /// </summary>
        public bool CoolEye;
        public BaseObject GroupOwner;
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
        internal bool AllowDeal;
        /// <summary>
        /// 禁止私聊人员列表
        /// </summary>
        public IList<string> LockWhisperList;
        /// <summary>
        /// 喊话消息间隔
        /// </summary>
        protected int ShoutMsgTick;
        /// <summary>
        /// 是否被召唤(主人)
        /// </summary>
        public BaseObject Master;
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
        internal IList<BaseObject> SlaveList;
        /// <summary>
        /// 宝宝攻击状态(休息/攻击)
        /// </summary>
        public bool SlaveRelax = false;
        /// <summary>
        /// 下属攻击状态
        /// </summary>
        public AttackMode AttatckMode;
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
        protected bool GuildWarArea;
        /// <summary>
        /// 所属城堡
        /// </summary>
        public TUserCastle Castle;
        public bool Bo2B0;
        public int MDw2B4Tick = 0;
        /// <summary>
        /// 无敌模式
        /// </summary>
        public bool SuperMan;
        public bool Bo2B9;
        public bool Bo2Ba;
        /// <summary>
        /// 是否是动物
        /// </summary>
        public bool Animal;
        /// <summary>
        /// 死亡是否不掉物品
        /// </summary>
        public bool NoItem;
        /// <summary>
        /// 隐身模式
        /// </summary>
        public bool FixedHideMode;
        /// <summary>
        /// 不能冲撞模式(即敌人不能使用野蛮冲撞技能攻击)
        /// </summary>
        public bool StickMode;
        /// <summary>
        /// 被打到是否减慢行走速度,等级小于50的怪 F-减慢 T-不减慢
        /// </summary>
        public bool Bo2Bf;
        /// <summary>
        /// 非攻击模式 F-可攻击 T-不攻击
        /// </summary>
        public bool NoAttackMode;
        public bool NoTame;
        /// <summary>
        /// 尸体
        /// </summary>
        public bool Skeleton;
        /// <summary>
        /// 肉的品质
        /// </summary>
        public ushort MeatQuality;
        /// <summary>
        /// 身体坚韧性
        /// </summary>
        public int BodyLeathery;
        /// <summary>
        /// 不能走动模式(困魔咒)
        /// </summary>
        public bool HolySeize;
        /// <summary>
        /// 不能走动间隔(困魔咒)
        /// </summary>
        public int HolySeizeTick;
        /// <summary>
        /// 不能走动时长(困魔咒)
        /// </summary>
        public int HolySeizeInterval;
        /// <summary>
        /// 狂暴模式
        /// </summary>
        public bool CrazyMode;
        /// <summary>
        /// 狂暴间隔
        /// </summary>
        private int CrazyModeTick;
        /// <summary>
        /// 狂暴时常
        /// </summary>
        private int CrazyModeInterval;
        /// <summary>
        /// 心灵启示
        /// </summary>
        public bool ShowHp;
        /// <summary>
        /// 心灵启示检查时间
        /// </summary>
        public int ShowHpTick = 0;
        /// <summary>
        /// 心灵启示有效时长
        /// </summary>
        public int ShowHpInterval = 0;
        /// <summary>
        /// 检查重叠人物使用
        /// </summary>
        public bool Bo2F0;
        /// <summary>
        /// 检查重叠人物间隔
        /// </summary>
        public int DupObjTick = 0;
        public Envirnoment Envir;
        /// <summary>
        /// 尸体清除
        /// </summary>
        public bool Ghost;
        /// <summary>
        /// 尸体清除间隔
        /// </summary>
        public int GhostTick;
        /// <summary>
        /// 死亡
        /// </summary>
        public bool Death;
        /// <summary>
        /// 死亡间隔
        /// </summary>
        public int DeathTick;
        public bool Invisible;
        /// <summary>
        /// 是否可以复活
        /// </summary>
        public bool CanReAlive;
        /// <summary>
        /// 复活时间
        /// </summary>
        public int ReAliveTick = 0;
        public MonGenInfo MonGen;
        /// <summary>
        /// 怪物所拿的武器
        /// </summary>
        public byte MonsterWeapon = 0;
        /// <summary>
        /// 弯腰间隔
        /// </summary>
        public int StruckTick = 0;
        /// <summary>
        /// 刷新消息
        /// </summary>
        protected bool WantRefMsg;
        /// <summary>
        /// 增加到地图是否成功
        /// </summary>
        public bool AddtoMapSuccess;
        /// <summary>
        /// 换地图时，跑走不考虑坐标
        /// </summary>
        public bool MBo316;
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
        protected BaseObject DealCreat;
        public GuildInfo MyGuild;
        public int GuildRankNo;
        public string GuildRankName = string.Empty;
        public string ScriptLable = string.Empty;
        protected byte AttackSkillCount;
        protected byte AttackSkillPointCount;
        public bool Mission;
        public short MissionX = 0;
        public short MissionY = 0;
        /// <summary>
        /// 隐身戒指
        /// </summary>
        public bool HideMode;
        /// <summary>
        /// 石像化(被麻痹)
        /// </summary>
        public bool StoneMode;
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
        protected bool Paralysis = false;
        /// <summary>
        /// 防麻痹
        /// </summary>
        internal bool UnParalysis = false;
        /// <summary>
        /// 复活戒指
        /// </summary>
        private bool Revival = false;
        /// <summary>
        /// 防复活
        /// </summary>
        private bool UnRevival = false;
        /// <summary>
        /// 复活戒指使用间隔计数
        /// </summary>
        private int RevivalTick = 0;
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
        private bool MagicShield = false;
        /// <summary>
        /// 防护身
        /// </summary>
        private bool UnMagicShield = false;
        /// <summary>
        /// 活力戒指
        /// </summary>
        private bool MuscleRing = false;
        /// <summary>
        /// 技巧项链
        /// </summary>
        private bool FastTrain = false;
        /// <summary>
        /// 探测项链
        /// </summary>
        public bool ProbeNecklace = false;
        /// <summary>
        /// 行会传送
        /// </summary>
        public bool GuildMove = false;
        /// <summary>
        /// 无敌(未完全)物品
        /// </summary>
        protected bool SuperManItem;
        /// <summary>
        /// 祈祷
        /// </summary>
        protected bool MBopirit = false;
        /// <summary>
        /// 掉物品
        /// </summary>
        public bool NoDropItem = false;
        /// <summary>
        /// 死亡是不是掉装备
        /// </summary>
        public bool NoDropUseItem = false;
        /// <summary>
        /// 经验物品
        /// </summary>
        protected bool BoExpItem = false;
        /// <summary>
        /// 力量物品(影响力量的物品)
        /// </summary>
        protected bool BoPowerItem = false;
        /// <summary>
        /// 经验物品值
        /// </summary>
        protected int ExpItem = 0;
        /// <summary>
        /// 力量物品值
        /// </summary>
        protected int PowerItem = 0;
        /// <summary>
        /// PK 死亡掉经验，不够经验就掉等级
        /// </summary>
        protected int PkDieLostExp;
        /// <summary>
        /// PK 死亡掉等级
        /// </summary>
        protected int PkDieLostLevel;
        /// <summary>
        /// 心灵启示
        /// </summary>
        public bool AbilSeeHealGauge;
        /// <summary>
        /// 魔法盾
        /// </summary>
        protected bool AbilMagBubbleDefence;
        /// <summary>
        /// 魔法盾等级
        /// </summary>
        protected byte MagBubbleDefenceLevel;
        /// <summary>
        /// 搜索时间
        /// </summary>
        public int SearchTime;
        /// <summary>
        /// 搜索间隔
        /// </summary>
        public int SearchTick;
        /// <summary>
        /// 上次运行时间
        /// </summary>
        public int RunTick;
        /// <summary>
        /// 运行时间
        /// </summary>
        public int RunTime;
        /// <summary>
        /// 特别指定为 此类型  加血间隔
        /// </summary>
        protected int HealthTick;
        protected int SpellTick;
        public BaseObject TargetCret;
        public int TargetFocusTick = 0;
        /// <summary>
        /// 人物被对方杀害时对方指针
        /// </summary>
        public BaseObject LastHiter;
        public int LastHiterTick;
        public BaseObject ExpHitter;
        protected int ExpHitterTick;
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
        protected bool PvpFlag;
        /// <summary>
        /// 人物攻击变色时间长度
        /// </summary>
        protected int MDwPkTick;
        /// <summary>
        /// 魔血一套
        /// </summary>
        protected int MoXieSuite;
        /// <summary>
        /// 虹魔一套
        /// </summary>
        protected int SuckupEnemyHealthRate;
        protected double SuckupEnemyHealth;
        /// <summary>
        /// 中毒处理间隔时间
        /// </summary>
        protected int PoisoningTick;
        /// <summary>
        /// 减PK值时间`
        /// </summary>
        private int DecPkPointTick;
        protected int DecLightItemDrugTick;
        protected int VerifyTick;
        protected int CheckRoyaltyTick;
        protected int DecHungerPointTick;
        protected int MDwHpmpTick;
        protected readonly IList<SendMessage> MsgList;
        private readonly IList<BaseObject> VisibleHumanList;
        protected readonly IList<VisibleMapItem> VisibleItems;
        protected readonly IList<MirEvent> VisibleEvents;
        protected int SendRefMsgTick;
        /// <summary>
        /// 是否在开行会战
        /// </summary>
        protected bool InFreePkArea;
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
        public int ProcessRunCount;
        /// <summary>
        /// 可见精灵列表
        /// </summary>
        public readonly IList<VisibleBaseObject> VisibleActors;
        /// <summary>
        /// 物品列表
        /// </summary>
        public IList<UserItem> ItemList;
        /// <summary>
        /// 交易列表
        /// </summary>
        public IList<UserItem> DealItemList;
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
        public readonly IList<UserMagic> MagicList;
        /// <summary>
        /// 身上物品
        /// </summary>
        public UserItem[] UseItems;
        public IList<TMonSayMsg> SayMsgList;
        /// <summary>
        /// 仓库物品列表
        /// </summary>
        internal readonly IList<UserItem> StorageItemList;
        /// <summary>
        /// 走路速度
        /// </summary>
        public int WalkSpeed;
        /// <summary>
        /// 行走步伐
        /// </summary>
        public int WalkStep = 0;
        protected int WalkCount;
        public int WalkWait = 0;
        protected int WalkWaitTick;
        /// <summary>
        /// 步行等待锁定
        /// </summary>
        protected bool WalkWaitLocked;
        /// <summary>
        /// 下次攻击时间
        /// </summary>
        public int NextHitTime;
        protected UserMagic[] MagicArr;
        protected bool PowerHit;
        protected bool UseThrusting;
        protected bool UseHalfMoon;
        protected bool RedUseHalfMoon;
        protected bool FireHitSkill;
        protected bool CrsHitkill = false;
        public bool TwinHitSkill;
        public bool MBo43Kill = false;
        public int LatestFireHitTick = 0;
        public int DoMotaeboTick = 0;
        public int LatestTwinHitTick = 0;
        /// <summary>
        /// 是否刷新在地图上信息
        /// </summary>
        protected readonly bool DenyRefStatus;
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
        /// <summary>
        /// 挂机
        /// </summary>
        public bool OffLineFlag = false;
        /// <summary>
        /// 挂机字符
        /// </summary>
        public string MSOffLineLeaveword = string.Empty;
        public int KickOffLineTick = 0;
        public bool NastyMode;
        /// <summary>
        /// 气血石
        /// </summary>
        protected int AutoAddHpmpMode = 0;
        public int CheckHpmpTick = 0;
        public long DwTick3F4 = 0;
        public byte m_btValNPCType;
        public byte m_btValType;
        public byte m_btValLabel;
        /// <summary>
        /// 是否机器人
        /// </summary>
        public bool IsRobot;

        protected BaseObject()
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
            Appr = 0;
            Bo2B9 = true;
            ViewRange = 5;
            HomeMap = "0";
            Permission = 0;
            Light = 0;
            NameColor = 255;
            HitPlus = 0;
            HitDouble = 0;
            BodyLuck = 0;
            GroupRcallTime = 0;
            GroupRcallTick = HUtil32.GetTickCount();
            RecallSuite = false;
            BoRace = false;
            Bo2Ba = false;
            AbilSeeHealGauge = false;
            PowerHit = false;
            UseThrusting = false;
            UseHalfMoon = false;
            RedUseHalfMoon = false;
            FireHitSkill = false;
            TwinHitSkill = false;
            HitPoint = 5;
            SpeedPoint = 15;
            HitSpeed = 0;
            LifeAttrib = 0;
            AntiPoison = 0;
            PoisonRecover = 0;
            HealthRecover = 0;
            SpellRecover = 0;
            AntiMagic = 0;
            Luck = 0;
            IncSpell = 0;
            IncHealth = 0;
            IncHealing = 0;
            IncHpStoneTime = HUtil32.GetTickCount();
            IncMpStoneTime = HUtil32.GetTickCount();
            PerHealth = 5;
            PerHealing = 5;
            PerSpell = 5;
            IncHealthSpellTick = HUtil32.GetTickCount();
            GreenPoisoningPoint = 0;
            FightZoneDieCount = 0;
            GoldMax = M2Share.Config.HumanMaxGold;
            CharStatus = 0;
            CharStatusEx = 0;
            StatusArr = new ushort[15];
            StatusArrTick = new int[15];
            ExtraAbil = new ushort[7];
            ExtraAbilTimes = new int[7];
            ExtraAbilFlag = new byte[7];
            BonusAbil = new NakedAbility();
            CurBonusAbil = new NakedAbility();
            AllowGroup = false;
            AllowGuild = false;
            BtB2 = 0;
            AttatckMode = 0;
            InFreePkArea = false;
            GuildWarArea = false;
            Bo2B0 = false;
            SuperMan = false;
            Skeleton = false;
            Bo2Bf = false;
            HolySeize = false;
            CrazyMode = false;
            ShowHp = false;
            Bo2F0 = false;
            Animal = false;
            NoItem = false;
            BodyLeathery = 50;
            FixedHideMode = false;
            StickMode = false;
            NoAttackMode = false;
            NoTame = false;
            PvpFlag = false;
            MoXieSuite = 0;
            SuckupEnemyHealthRate = 0;
            SuckupEnemyHealth = 0;
            AddAbil = new AddAbility();
            MsgList = new List<SendMessage>();
            VisibleHumanList = new List<BaseObject>();
            VisibleActors = new List<VisibleBaseObject>();
            VisibleItems = new List<VisibleMapItem>();
            VisibleEvents = new List<MirEvent>();
            ItemList = new List<UserItem>();
            DealItemList = new List<UserItem>();
            IsVisibleActive = false;
            ProcessRunCount = 0;
            DealGolds = 0;
            MagicList = new List<UserMagic>();
            StorageItemList = new List<UserItem>();
            UseItems = new UserItem[13];
            GroupOwner = null;
            Castle = null;
            Master = null;
            KillMonCount = 0;
            SlaveExpLevel = 0;
            GroupMembers = new List<PlayObject>();
            HearWhisper = true;
            BanShout = true;
            BanGuildChat = true;
            AllowDeal = true;
            AllowGroupReCall = false;
            LockWhisperList = new List<string>();
            SlaveList = new List<BaseObject>();
            Abil = new Ability();
            QuestUnitOpen = new byte[128];
            QuestUnit = new byte[128];
            QuestFlag = new byte[128];
            Abil = new Ability
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
            Mission = false;
            HideMode = false;
            StoneMode = false;
            CoolEye = false;
            UserUnLockDurg = false;
            Transparent = false;
            AdminMode = false;
            ObMode = false;
            RunTick = HUtil32.GetTickCount() + M2Share.RandomNumber.Random(1500);
            RunTime = 250;
            SearchTime = M2Share.RandomNumber.Random(2000) + 2000;
            SearchTick = HUtil32.GetTickCount();
            DecPkPointTick = HUtil32.GetTickCount();
            DecLightItemDrugTick = HUtil32.GetTickCount();
            PoisoningTick = HUtil32.GetTickCount();
            VerifyTick = HUtil32.GetTickCount();
            CheckRoyaltyTick = HUtil32.GetTickCount();
            DecHungerPointTick = HUtil32.GetTickCount();
            MDwHpmpTick = HUtil32.GetTickCount();
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
            HealthTick = 0;
            SpellTick = 0;
            TargetCret = null;
            LastHiter = null;
            ExpHitter = null;
            SayMsgList = null;
            DenyRefStatus = false;
            HorseType = 0;
            DressEffType = 0;
            PkDieLostExp = 0;
            PkDieLostLevel = 0;
            AddToMaped = true;
            AutoChangeColor = false;
            AutoChangeColorTick = HUtil32.GetTickCount();
            AutoChangeIdx = 0;
            FixColor = false;
            FixColorIdx = 0;
            FixStatus = -1;
            FastParalysis = false;
            NastyMode = false;
            MagicArr = new UserMagic[100];
            M2Share.ActorMgr.Add(ActorId, this);
        }

        public void ChangePkStatus(bool boWarFlag)
        {
            if (InFreePkArea != boWarFlag)
            {
                InFreePkArea = boWarFlag;
                NameColorChanged = true;
            }
        }

        /// <summary>
        /// 获取物品掉落位置
        /// </summary>
        /// <returns></returns>
        private bool GetDropPosition(int nOrgX, int nOrgY, int nRange, ref int pX, ref int pY)
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
                        pX = nOrgX + iii;
                        pY = nOrgY + ii;
                        if (Envir.GetItemEx(pX, pY, ref nItemCount) == 0)
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
                                n28 = pX;
                                n2C = pY;
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
                    pX = n28;
                    pY = n2C;
                }
                else
                {
                    pX = nOrgX;
                    pY = nOrgY;
                }
            }
            return result;
        }

        public bool DropItemDown(UserItem userItem, int nScatterRange, bool boDieDrop, BaseObject itemOfCreat,
            BaseObject dropCreat)
        {
            var result = false;
            var dx = 0;
            var dy = 0;
            if (userItem == null)
            {
                return false;
            }
            var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
            if (stdItem != null)
            {
                if (stdItem.StdMode == 40)
                {
                    int idura = userItem.Dura;
                    idura = idura - 2000;
                    if (idura < 0)
                    {
                        idura = 0;
                    }

                    userItem.Dura = (ushort)idura;
                }
                var mapItem = new MapItem
                {
                    UserItem = userItem,
                    Name = CustomItem.GetItemName(userItem), // 取自定义物品名称
                    Looks = stdItem.Looks
                };
                if (stdItem.StdMode == 45)
                {
                    mapItem.Looks = (ushort)M2Share.GetRandomLook(mapItem.Looks, stdItem.Shape);
                }
                mapItem.AniCount = stdItem.AniCount;
                mapItem.Reserved = 0;
                mapItem.Count = 1;
                mapItem.OfBaseObject = itemOfCreat == null ? 0 : itemOfCreat.ActorId;
                mapItem.CanPickUpTick = HUtil32.GetTickCount();
                mapItem.DropBaseObject = dropCreat.ActorId;
                GetDropPosition(CurrX, CurrY, nScatterRange, ref dx, ref dy);
                var pr = (MapItem)Envir.AddToMap(dx, dy, CellType.Item, mapItem);
                if (pr == mapItem)
                {
                    SendRefMsg(Grobal2.RM_ITEMSHOW, mapItem.Looks, mapItem.ActorId, dx, dy, mapItem.Name);
                    string logcap;
                    if (boDieDrop)
                    {
                        logcap = "15";
                    }
                    else
                    {
                        logcap = "7";
                    }
                    if (!M2Share.IsCheapStuff(stdItem.StdMode))
                    {
                        if (stdItem.NeedIdentify == 1)
                        {
                            M2Share.AddGameDataLog(logcap + "\t" + MapName + "\t" + CurrX + "\t" + CurrY + "\t" + CharName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" +
                                                   HUtil32.BoolToIntStr(Race == Grobal2.RC_PLAYOBJECT) + "\t" + '0');
                        }
                    }
                    result = true;
                }
                else
                {
                    mapItem = null;
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
        
        public void HasLevelUp(int nLevel)
        {
            Abil.MaxExp = GetLevelExp(Abil.Level);
            RecalcLevelAbilitys();
            RecalcAbilitys();
            SendMsg(this, Grobal2.RM_LEVELUP, 0, Abil.Exp, 0, 0, "");
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(this as PlayObject, "@LevelUp", false);
            }
        }

        protected bool WalkTo(byte btDir, bool boFlag)
        {
            short n20 = 0;
            short n24 = 0;
            bool bo29;
            const string sExceptionMsg = "[Exception] TBaseObject::WalkTo";
            var result = false;
            if (HolySeize)
            {
                return result;
            }
            try
            {
                var oldX = CurrX;
                var oldY = CurrY;
                Direction = btDir;
                short newX = 0;
                short newY = 0;
                switch (btDir)
                {
                    case Grobal2.DR_UP:
                        newX = CurrX;
                        newY = (short)(CurrY - 1);
                        break;
                    case Grobal2.DR_UPRIGHT:
                        newX = (short)(CurrX + 1);
                        newY = (short)(CurrY - 1);
                        break;
                    case Grobal2.DR_RIGHT:
                        newX = (short)(CurrX + 1);
                        newY = CurrY;
                        break;
                    case Grobal2.DR_DOWNRIGHT:
                        newX = (short)(CurrX + 1);
                        newY = (short)(CurrY + 1);
                        break;
                    case Grobal2.DR_DOWN:
                        newX = CurrX;
                        newY = (short)(CurrY + 1);
                        break;
                    case Grobal2.DR_DOWNLEFT:
                        newX = (short)(CurrX - 1);
                        newY = (short)(CurrY + 1);
                        break;
                    case Grobal2.DR_LEFT:
                        newX = (short)(CurrX - 1);
                        newY = CurrY;
                        break;
                    case Grobal2.DR_UPLEFT:
                        newX = (short)(CurrX - 1);
                        newY = (short)(CurrY - 1);
                        break;
                }
                if (newX >= 0 && Envir.Width - 1 >= newX && newY >= 0 && Envir.Height - 1 >= newY)
                {
                    bo29 = true;
                    if (Bo2Ba && !Envir.CanSafeWalk(newX, newY))
                    {
                        bo29 = false;
                    }
                    if (Master != null)
                    {
                        Master.Envir.GetNextPosition(Master.CurrX, Master.CurrY, Master.Direction, 1, ref n20, ref n24);
                        if (newX == n20 && newY == n24)
                        {
                            bo29 = false;
                        }
                    }
                    if (bo29)
                    {
                        if (Envir.MoveToMovingObject(CurrX, CurrY, this, newX, newY, boFlag) > 0)
                        {
                            CurrX = newX;
                            CurrY = newY;
                        }
                    }
                }
                if (CurrX != oldX || CurrY != oldY)
                {
                    if (Walk(Grobal2.RM_WALK))
                    {
                        if (Transparent && HideMode)
                        {
                            StatusArr[StatuStateConst.STATE_TRANSPARENT] = 1;
                        }
                        result = true;
                    }
                    else
                    {
                        Envir.DeleteFromMap(CurrX, CurrY, MapCell, this);
                        CurrX = oldX;
                        CurrY = oldY;
                        Envir.AddToMap(CurrX, CurrY, MapCell, this);
                    }
                }
            }
            catch (Exception ex)
            {
                M2Share.Log.Error(sExceptionMsg);
                M2Share.Log.Error(ex.StackTrace);
            }
            return result;
        }

        protected bool IsGroupMember(BaseObject target)
        {
            var result = false;
            if (GroupOwner == null)
            {
                return result;
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

        public int PvpLevel()
        {
            return PkPoint / 100;
        }

        protected void HealthSpellChanged()
        {
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                SendUpdateMsg(this, Grobal2.RM_HEALTHSPELLCHANGED, 0, 0, 0, 0, "");
            }

            if (ShowHp)
            {
                SendRefMsg(Grobal2.RM_HEALTHSPELLCHANGED, 0, 0, 0, 0, "");
            }
        }

        private int CalcGetExp(int nLevel, int nExp)
        {
            int result;
            if (M2Share.Config.HighLevelKillMonFixExp || (Abil.Level < (nLevel + 10)))
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
                tCount = M2Share.Config.MonUpLvNeedKillCount[SlaveExpLevel];
            }
            else
            {
                tCount = 0;
            }
            return (Abil.Level * M2Share.Config.MonUpLvRate) - Abil.Level + M2Share.Config.MonUpLvNeedKillBase + tCount;
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

        protected bool DropGoldDown(int nGold, bool boFalg, BaseObject goldOfCreat, BaseObject dropGoldCreat)
        {
            var result = false;
            var nX = 0;
            var nY = 0;
            string s20;
            var dropWide = HUtil32._MIN(M2Share.Config.DropItemRage, 7);
            var mapItem = new MapItem
            {
                Name = Grobal2.sSTRING_GOLDNAME,
                Count = nGold,
                Looks = M2Share.GetGoldShape(nGold),
                OfBaseObject = goldOfCreat?.ActorId ?? 0,
                CanPickUpTick = HUtil32.GetTickCount(),
                DropBaseObject = dropGoldCreat.ActorId
            };
            GetDropPosition(CurrX, CurrY, 3, ref nX, ref nY);
            var mapItemA = (MapItem)Envir.AddToMap(nX, nY, CellType.Item, mapItem);
            if (mapItemA != null)
            {
                if (mapItemA.ActorId != mapItem.ActorId)
                {
                    mapItem = mapItemA;
                }
                SendRefMsg(Grobal2.RM_ITEMSHOW, mapItem.Looks, mapItem.ActorId, nX, nY, mapItem.Name);
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
                        M2Share.AddGameDataLog(s20 + "\t" + MapName + "\t" + CurrX + "\t" + CurrY + "\t" + CharName + "\t" + Grobal2.sSTRING_GOLDNAME + "\t" + nGold + "\t" + HUtil32.BoolToIntStr(Race == Grobal2.RC_PLAYOBJECT) + "\t" + '0');
                    }
                }
                result = true;
            }
            else
            {
                mapItem = null;
            }
            return result;
        }

        private int GetGuildRelation(BaseObject cert1, BaseObject cert2)
        {
            var result = 0;
            GuildWarArea = false;
            if ((cert1.MyGuild == null) || (cert2.MyGuild == null))
            {
                return 0;
            }
            if (cert1.InSafeArea() || cert2.InSafeArea())
            {
                return 0;
            }
            if (cert1.MyGuild.GuildWarList.Count <= 0)
            {
                return 0;
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

        protected void AddBodyLuck(double dLuck)
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

        protected void MakeWeaponUnlock()
        {
            if (UseItems[Grobal2.U_WEAPON] == null)
            {
                return;
            }
            if (UseItems[Grobal2.U_WEAPON].Index <= 0)
            {
                return;
            }
            if (UseItems[Grobal2.U_WEAPON].Desc[3] > 0)
            {
                UseItems[Grobal2.U_WEAPON].Desc[3] -= 1;
                SysMsg(M2Share.g_sTheWeaponIsCursed, MsgColor.Red, MsgType.Hint);
            }
            else
            {
                if (UseItems[Grobal2.U_WEAPON].Desc[4] < 10)
                {
                    UseItems[Grobal2.U_WEAPON].Desc[4]++;
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
            if (nPower < 0)
            {
                nPower = 0;
            }
            if (Luck > 0)
            {
                if (M2Share.RandomNumber.Random(10 - HUtil32._MIN(9, Luck)) == 0)
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
                if (Luck < 0)
                {
                    if (M2Share.RandomNumber.Random(10 - HUtil32._MAX(0, -Luck)) == 0)
                    {
                        result = nBasePower;
                    }
                }
            }
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                PlayObject playObject = this as PlayObject;
                result = HUtil32.Round(result * (playObject.m_nPowerRate / 100));
                if (playObject.BoPowerItem)
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
        private void DamageHealth(ushort nDamage)
        {
            if (((LastHiter == null) || !LastHiter.UnMagicShield) && MagicShield && (nDamage > 0) && (WAbil.MP > 0))
            {
                var nSpdam = HUtil32.Round(nDamage * 1.5);
                if (WAbil.MP >= nSpdam)
                {
                    WAbil.MP = (ushort)(WAbil.MP - nSpdam);
                    nSpdam = 0;
                }
                else
                {
                    nSpdam = nSpdam - WAbil.MP;
                    WAbil.MP = 0;
                }
                nDamage = (ushort)HUtil32.Round(nSpdam / 1.5);
                HealthSpellChanged();
            }
            if (nDamage > 0)
            {
                if ((WAbil.HP - nDamage) > 0)
                {
                    WAbil.HP = (ushort)(WAbil.HP - nDamage);
                }
                else
                {
                    WAbil.HP = 0;
                }
            }
            else
            {
                if ((WAbil.HP - nDamage) < WAbil.MaxHP)
                {
                    WAbil.HP = (ushort)(WAbil.HP - nDamage);
                }
                else
                {
                    WAbil.HP = WAbil.MaxHP;
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
            var result = 0;
            var olddir = Direction;
            int oldx = CurrX;
            int oldy = CurrY;
            Direction = nDir;
            var nBackDir = GetBackDir(nDir);
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
            BaseObject baseObject;
            var tcount = 0;
            for (var i = 0; i < 12; i++)
            {
                baseObject = Envir.GetMovingObject(sx, sy, true) as BaseObject;
                if (baseObject != null)
                {
                    if (IsProperTarget(baseObject))
                    {
                        if (M2Share.RandomNumber.Random(10) >= baseObject.AntiMagic)
                        {
                            if (undeadattack)
                            {
                                magpwr = HUtil32.Round(magpwr * 1.5);
                            }
                            baseObject.SendDelayMsg(this, Grobal2.RM_MAGSTRUCK, 0, magpwr, 0, 0, "", 600);
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
            if (ShowHp)
            {
                ShowHp = false;
                CharStatusEx = CharStatusEx ^ StatuStateConst.STATE_OPENHEATH;
                CharStatus = GetCharStatus();
                SendRefMsg(Grobal2.RM_CLOSEHEALTH, 0, 0, 0, 0, "");
            }
        }

        private void MakeOpenHealth()
        {
            ShowHp = true;
            CharStatusEx = CharStatusEx | StatuStateConst.STATE_OPENHEATH;
            CharStatus = GetCharStatus();
            SendRefMsg(Grobal2.RM_OPENHEALTH, 0, WAbil.HP, WAbil.MaxHP, 0, "");
        }

        protected void IncHealthSpell(int nHp, int nMp)
        {
            if ((nHp < 0) || (nMp < 0))
            {
                return;
            }
            if ((WAbil.HP + nHp) >= WAbil.MaxHP)
            {
                WAbil.HP = WAbil.MaxHP;
            }
            else
            {
                WAbil.HP += (ushort)nHp;
            }
            if ((WAbil.MP + nMp) >= WAbil.MaxMP)
            {
                WAbil.MP = WAbil.MaxMP;
            }
            else
            {
                WAbil.MP += (ushort)nMp;
            }
            HealthSpellChanged();
        }

        private void ItemDamageRevivalRing()
        {
            for (var i = 0; i < UseItems.Length; i++)
            {
                if (UseItems[i] != null && UseItems[i].Index > 0)
                {
                    var pSItem = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                    if (pSItem != null)
                    {
                        if (M2Share.ItemDamageRevivalMap.Contains(pSItem.Shape) || (((i == Grobal2.U_WEAPON) || (i == Grobal2.U_RIGHTHAND)) && M2Share.ItemDamageRevivalMap.Contains(pSItem.AniCount)))
                        {
                            var nDura = UseItems[i].Dura;
                            var tDura = (ushort)HUtil32.Round(nDura / 1000.0);
                            nDura -= 1000;
                            if (nDura <= 0)
                            {
                                nDura = 0;
                                UseItems[i].Dura = nDura;
                                if (Race == Grobal2.RC_PLAYOBJECT)
                                {
                                    var playObject = this as PlayObject;
                                    playObject.SendDelItems(UseItems[i]);
                                }
                                UseItems[i].Index = 0;
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
            var envir = this.Envir;
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
                    if ((nX < (envir.Width - 1)) && (nY > 0))
                    {
                        nX++;
                        nY -= 1;
                    }
                    break;
                case Grobal2.DR_RIGHT:
                    if (nX < (envir.Width - 1))
                    {
                        nX++;
                    }
                    break;
                case Grobal2.DR_DOWNRIGHT:
                    if ((nX < (envir.Width - 1)) && (nY < (envir.Height - 1)))
                    {
                        nX++;
                        nY++;
                    }
                    break;
                case Grobal2.DR_DOWN:
                    if (nY < (envir.Height - 1))
                    {
                        nY++;
                    }
                    break;
                case Grobal2.DR_DOWNLEFT:
                    if ((nX > 0) && (nY < (envir.Height - 1)))
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
            return true;
        }

        private bool SpaceMove_GetRandXY(Envirnoment envir, ref short nX, ref short nY)
        {
            int n14;
            short n18;
            int n1C;
            var result = false;
            if (envir.Width < 80)
            {
                n18 = 3;
            }
            else
            {
                n18 = 10;
            }
            if (envir.Height < 150)
            {
                if (envir.Height < 50)
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
                if (envir.CanWalk(nX, nY, true))
                {
                    result = true;
                    break;
                }

                if (nX < (envir.Width - n1C - 1))
                {
                    nX += n18;
                }
                else
                {
                    nX = (short)M2Share.RandomNumber.Random(envir.Width);
                    if (nY < (envir.Height - n1C - 1))
                    {
                        nY += n18;
                    }
                    else
                    {
                        nY = (short)M2Share.RandomNumber.Random(envir.Height);
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
            var envir = M2Share.MapMgr.FindMap(sMap);
            if (envir != null)
            {
                if (M2Share.ServerIndex == envir.ServerIndex)
                {
                    var oldEnvir = this.Envir;
                    nOldX = CurrX;
                    nOldY = CurrY;
                    bo21 = false;
                    this.Envir.DeleteFromMap(CurrX, CurrY, MapCell, this);
                    VisibleHumanList.Clear();
                    for (var i = 0; i < VisibleItems.Count; i++)
                    {
                        VisibleItems[i] = null;
                    }
                    VisibleItems.Clear();
                    for (var i = 0; i < VisibleActors.Count; i++)
                    {
                        VisibleActors[i] = null;
                    }
                    VisibleActors.Clear();
                    VisibleEvents.Clear();
                    this.Envir = envir;
                    MapName = envir.MapName;
                    MapFileName = envir.MapFileName;
                    CurrX = nX;
                    CurrY = nY;
                    if (SpaceMove_GetRandXY(this.Envir, ref CurrX, ref CurrY))
                    {
                        this.Envir.AddToMap(CurrX, CurrY, MapCell, this);
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
                        MBo316 = true;
                        bo21 = true;
                    }
                    if (!bo21)
                    {
                        this.Envir = oldEnvir;
                        CurrX = (short)nOldX;
                        CurrY = (short)nOldY;
                        this.Envir.AddToMap(CurrX, CurrY, MapCell, this);
                    }
                    OnEnvirnomentChanged();
                }
                else
                {
                    if (SpaceMove_GetRandXY(envir, ref nX, ref nY))
                    {
                        if (Race == Grobal2.RC_PLAYOBJECT)
                        {
                            DisappearA();
                            MBo316 = true;
                            var playObject = this as PlayObject;
                            playObject.m_sSwitchMapName = envir.MapName;
                            playObject.m_nSwitchMapX = nX;
                            playObject.m_nSwitchMapY = nY;
                            playObject.m_boSwitchData = true;
                            playObject.m_nServerIndex = envir.ServerIndex;
                            playObject.m_boEmergencyClose = true;
                            playObject.m_boReconnection = true;
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

        public BaseObject MakeSlave(string sMonName, int nMakeLevel, int nExpLevel, int nMaxMob, int dwRoyaltySec)
        {
            short nX = 0;
            short nY = 0;
            if (SlaveList.Count < nMaxMob)
            {
                GetFrontPosition(ref nX, ref nY);
                var monObj = M2Share.WorldEngine.RegenMonsterByName(Envir.MapName, nX, nY, sMonName);
                if (monObj != null)
                {
                    monObj.Master = this;
                    monObj.MasterRoyaltyTick = HUtil32.GetTickCount() + (dwRoyaltySec * 1000);
                    monObj.SlaveMakeLevel = (byte)nMakeLevel;
                    monObj.SlaveExpLevel = (byte)nExpLevel;
                    monObj.RecalcAbilitys();
                    if (monObj.WAbil.HP < monObj.WAbil.MaxHP)
                    {
                        monObj.WAbil.HP = (ushort)(monObj.WAbil.HP + (monObj.WAbil.MaxHP - monObj.WAbil.HP) / 2);
                    }
                    monObj.RefNameColor();
                    SlaveList.Add(monObj);
                    return monObj;
                }
            }
            return null;
        }

        /// <summary>
        /// 地图随机移动
        /// </summary>
        public void MapRandomMove(string sMapName, int nInt)
        {
            int nEgdey;
            var envir = M2Share.MapMgr.FindMap(sMapName);
            if (envir != null)
            {
                if (envir.Height < 150)
                {
                    if (envir.Height < 30)
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

                var nX = (short)(M2Share.RandomNumber.Random(envir.Width - nEgdey - 1) + nEgdey);
                var nY = (short)(M2Share.RandomNumber.Random(envir.Height - nEgdey - 1) + nEgdey);
                SpaceMove(sMapName, nX, nY, nInt);
            }
        }

        public bool AddItemToBag(UserItem userItem)
        {
            var result = false;
            if (ItemList.Count < Grobal2.MAXBAGITEM)
            {
                ItemList.Add(userItem);
                WeightChanged();
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 检查心灵启示
        /// </summary>
        protected void CheckSeeHealGauge(UserMagic magic)
        {
            if (magic.Magic.MagicId == 28)
            {
                if (magic.Level >= 2)
                {
                    AbilSeeHealGauge = true;
                }
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

        private bool KillFunc()
        {
            const string sExceptionMsg = "[Exception] TBaseObject::KillFunc";
            var result = false;
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
                M2Share.Log.Error(sExceptionMsg);
                M2Share.Log.Error(e.Message);
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
                if (UseItems[Grobal2.U_RIGHTHAND] != null && UseItems[Grobal2.U_RIGHTHAND].Index > 0)
                {
                    var stdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_RIGHTHAND].Index);
                    if ((stdItem == null) || (stdItem.SpecialPwr != 0))
                    {
                        return;
                    }
                    var nOldDura = HUtil32.Round((ushort)(UseItems[Grobal2.U_RIGHTHAND].Dura / 1000));
                    var nDura = 0;
                    if (M2Share.Config.DecLampDura)
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
                            var playObject = this as PlayObject;
                            playObject.SendDelItems(UseItems[Grobal2.U_RIGHTHAND]);
                        }
                        UseItems[Grobal2.U_RIGHTHAND].Index = 0;
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
                M2Share.Log.Error(sExceptionMsg);
            }
        }

        public BaseObject GetPoseCreate()
        {
            short nX = 0;
            short nY = 0;
            if (GetFrontPosition(ref nX, ref nY))
            {
                return (BaseObject)Envir.GetMovingObject(nX, nY, true);
            }
            return null;
        }

        protected bool GetAttackDir(BaseObject baseObject, ref byte btDir)
        {
            var result = false;
            if ((CurrX - 1 <= baseObject.CurrX) && (CurrX + 1 >= baseObject.CurrX) &&
                (CurrY - 1 <= baseObject.CurrY) && (CurrY + 1 >= baseObject.CurrY) &&
                ((CurrX != baseObject.CurrX) || (CurrY != baseObject.CurrY)))
            {
                result = true;
                if (((CurrX - 1) == baseObject.CurrX) && (CurrY == baseObject.CurrY))
                {
                    btDir = Grobal2.DR_LEFT;
                    return result;
                }

                if (((CurrX + 1) == baseObject.CurrX) && (CurrY == baseObject.CurrY))
                {
                    btDir = Grobal2.DR_RIGHT;
                    return result;
                }

                if ((CurrX == baseObject.CurrX) && ((CurrY - 1) == baseObject.CurrY))
                {
                    btDir = Grobal2.DR_UP;
                    return result;
                }

                if ((CurrX == baseObject.CurrX) && ((CurrY + 1) == baseObject.CurrY))
                {
                    btDir = Grobal2.DR_DOWN;
                    return result;
                }

                if (((CurrX - 1) == baseObject.CurrX) && ((CurrY - 1) == baseObject.CurrY))
                {
                    btDir = Grobal2.DR_UPLEFT;
                    return result;
                }

                if (((CurrX + 1) == baseObject.CurrX) && ((CurrY - 1) == baseObject.CurrY))
                {
                    btDir = Grobal2.DR_UPRIGHT;
                    return result;
                }

                if (((CurrX - 1) == baseObject.CurrX) && ((CurrY + 1) == baseObject.CurrY))
                {
                    btDir = Grobal2.DR_DOWNLEFT;
                    return result;
                }

                if (((CurrX + 1) == baseObject.CurrX) && ((CurrY + 1) == baseObject.CurrY))
                {
                    btDir = Grobal2.DR_DOWNRIGHT;
                    return result;
                }

                btDir = 0;
            }

            return result;
        }

        protected bool GetAttackDir(BaseObject baseObject, int nRange, ref byte btDir)
        {
            short nX = 0;
            short nY = 0;
            btDir = M2Share.GetNextDirection(CurrX, CurrY, baseObject.CurrX, baseObject.CurrY);
            if (Envir.GetNextPosition(CurrX, CurrY, btDir, nRange, ref nX, ref nY))
            {
                return baseObject == (BaseObject)Envir.GetMovingObject(nX, nY, true);
            }

            return false;
        }

        protected bool TargetInSpitRange(BaseObject baseObject, ref byte btDir)
        {
            var result = false;
            if ((Math.Abs(baseObject.CurrX - CurrX) <= 2) && (Math.Abs(baseObject.CurrY - CurrY) <= 2))
            {
                var nX = baseObject.CurrX - CurrX;
                var nY = baseObject.CurrY - CurrY;
                if ((Math.Abs(nX) <= 1) && (Math.Abs(nY) <= 1))
                {
                    GetAttackDir(baseObject, ref btDir);
                    result = true;
                    return result;
                }

                nX += 2;
                nY += 2;
                if ((nX >= 0) && (nX <= 4) && (nY >= 0) && (nY <= 4))
                {
                    btDir = M2Share.GetNextDirection(CurrX, CurrY, baseObject.CurrX, baseObject.CurrY);
                    if (M2Share.Config.SpitMap[btDir, nY, nX] == 1)
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
            for (var i = 0; i < ItemList.Count; i++)
            {
                var userItem = ItemList[i];
                var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (stdItem != null)
                {
                    result += stdItem.Weight;
                }
            }
            return result;
        }
        
        private void AddItemSkill(int nIndex)
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
                        Key = 0,
                        Level = 1,
                        TranPoint = 0
                    };
                    MagicList.Add(userMagic);
                    if (Race == Grobal2.RC_PLAYOBJECT)
                    {
                        (this as PlayObject).SendAddMagic(userMagic);
                    }
                }
            }
        }

        private bool AddToMap()
        {
            var point = Envir.AddToMap(CurrX, CurrY, MapCell, this);
            var result = point != null;
            if (!FixedHideMode)
            {
                SendRefMsg(Grobal2.RM_TURN, Direction, CurrX, CurrY, 0, "");
            }
            return result;
        }

        /// <summary>
        /// 计算施法魔法值
        /// </summary>
        /// <param name="userMagic"></param>
        /// <returns></returns>
        private ushort GetMagicSpell(UserMagic userMagic)
        {
            return (ushort)HUtil32.Round(userMagic.Magic.Spell / (userMagic.Magic.TrainLv + 1) * (userMagic.Level + 1));
        }

        private void CheckPkStatus()
        {
            if (PvpFlag && ((HUtil32.GetTickCount() - MDwPkTick) > M2Share.Config.dwPKFlagTime)) // 60 * 1000
            {
                PvpFlag = false;
                RefNameColor();
            }
        }

        /// <summary>
        /// 减少魔法值
        /// </summary>
        /// <param name="nSpellPoint"></param>
        protected void DamageSpell(ushort nSpellPoint)
        {
            if (nSpellPoint > 0)
            {
                if ((WAbil.MP - nSpellPoint) > 0)
                {
                    WAbil.MP -= nSpellPoint;
                }
                else
                {
                    WAbil.MP = 0;
                }
            }
            else
            {
                if ((WAbil.MP - nSpellPoint) < WAbil.MaxMP)
                {
                    WAbil.MP -= nSpellPoint;
                }
                else
                {
                    WAbil.MP = WAbil.MaxMP;
                }
            }
        }

        private void DelItemSkill_DeleteSkill(string sSkillName)
        {
            for (var i = 0; i < MagicList.Count; i++)
            {
                var userMagic = MagicList[i];
                if (userMagic.Magic.MagicName == sSkillName)
                {
                    var playObject = this as PlayObject;
                    playObject.SendDelMagic(userMagic);
                    userMagic = null;
                    MagicList.RemoveAt(i);
                    break;
                }
            }
        }

        private void DelItemSkill(int nIndex)
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
                        DelItemSkill_DeleteSkill(M2Share.Config.FireBallSkill);
                    }

                    break;
                case 2:
                    if (Job != PlayJob.Taoist)
                    {
                        DelItemSkill_DeleteSkill(M2Share.Config.HealSkill);
                    }

                    break;
            }
        }

        public void DelMember(BaseObject baseObject)
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
            var playObject = this as PlayObject;
            if (!playObject.CancelGroup())
            {
                playObject.SendDefMessage(Grobal2.SM_GROUPCANCEL, 0, 0, 0, 0, "");
            }
            else
            {
                playObject.SendGroupMembers();
            }
        }

        protected void DoDamageWeapon(int nWeaponDamage)
        {
            if (UseItems[Grobal2.U_WEAPON] == null || UseItems[Grobal2.U_WEAPON].Index <= 0)
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
                    var playObject = this as PlayObject;
                    playObject.SendDelItems(UseItems[Grobal2.U_WEAPON]);
                    var stdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_WEAPON].Index);
                    if (stdItem.NeedIdentify == 1)
                    {
                        M2Share.AddGameDataLog('3' + "\t" + MapName + "\t" + CurrX + "\t" + CurrY + "\t" +
                                               CharName + "\t" + stdItem.Name + "\t" +
                                               UseItems[Grobal2.U_WEAPON].MakeIndex + "\t" +
                                               HUtil32.BoolToIntStr(Race == Grobal2.RC_PLAYOBJECT) + "\t" +
                                               '0');
                    }
                }
                UseItems[Grobal2.U_WEAPON].Index = 0;
                SendMsg(this, Grobal2.RM_DURACHANGE, Grobal2.U_WEAPON, nDura, UseItems[Grobal2.U_WEAPON].DuraMax, 0, "");
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

        protected byte GetCharColor(BaseObject baseObject)
        {
            TUserCastle castle;
            var result = baseObject.GetNamecolor();
            if (baseObject.Race == Grobal2.RC_PLAYOBJECT)
            {
                if (baseObject.PvpLevel() < 2)
                {
                    if (baseObject.PvpFlag)
                    {
                        result = M2Share.Config.btPKFlagNameColor;
                    }

                    var n10 = GetGuildRelation(this, baseObject);
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

                    if (baseObject.Envir.Flag.boFight3Zone)
                    {
                        if (MyGuild == baseObject.MyGuild)
                        {
                            result = M2Share.Config.btAllyAndGuildNameColor;
                        }
                        else
                        {
                            result = M2Share.Config.WarGuildNameColor;
                        }
                    }
                }

                castle = M2Share.CastleMgr.InCastleWarArea(baseObject);
                if ((castle != null) && castle.UnderWar && InFreePkArea && baseObject.InFreePkArea)
                {
                    result = M2Share.Config.InFreePKAreaNameColor;
                    GuildWarArea = true;
                    if (MyGuild == null)
                    {
                        return result;
                    }

                    if (castle.IsMasterGuild(MyGuild))
                    {
                        if ((MyGuild == baseObject.MyGuild) || MyGuild.IsAllyGuild(baseObject.MyGuild))
                        {
                            result = M2Share.Config.btAllyAndGuildNameColor;
                        }
                        else
                        {
                            if (castle.IsAttackGuild(baseObject.MyGuild))
                            {
                                result = M2Share.Config.WarGuildNameColor;
                            }
                        }
                    }
                    else
                    {
                        if (castle.IsAttackGuild(MyGuild))
                        {
                            if ((MyGuild == baseObject.MyGuild) || MyGuild.IsAllyGuild(baseObject.MyGuild))
                            {
                                result = M2Share.Config.btAllyAndGuildNameColor;
                            }
                            else
                            {
                                if (castle.IsMember(baseObject))
                                {
                                    result = M2Share.Config.WarGuildNameColor;
                                }
                            }
                        }
                    }
                }
            }
            else if (baseObject.Race == Grobal2.RC_NPC) //增加NPC名字颜色单独控制
            {
                result = M2Share.Config.NpcNameColor;
                if (baseObject.CrazyMode) //疯狂模式(红名)
                {
                    result = 0xF9;
                }

                if (baseObject.HolySeize) //不能走动模式(困魔咒)
                {
                    result = 0x7D;
                }
            }
            else
            {
                if (baseObject.SlaveExpLevel <= Grobal2.SlaveMaxLevel)
                {
                    result = M2Share.Config.SlaveColor[baseObject.SlaveExpLevel];
                }
                else
                {
                    result = 255;
                }

                if (baseObject.CrazyMode)
                {
                    result = 0xF9;
                }

                if (baseObject.HolySeize)
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
                result = M2Share.Config.NeedExps[nLevel];
            }
            else
            {
                result = M2Share.Config.NeedExps[M2Share.Config.NeedExps.Length];
            }
            return result;
        }

        private byte GetNamecolor()
        {
            var result = NameColor;
            if (PvpLevel() == 1)
            {
                result = M2Share.Config.btPKLevel1NameColor;
            }
            if (PvpLevel() >= 2)
            {
                result = M2Share.Config.btPKLevel2NameColor;
            }
            return result;
        }

        public void HearMsg(string sMsg)
        {
            if (!string.IsNullOrEmpty(sMsg))
            {
                SendMsg(null, Grobal2.RM_HEAR, 0, M2Share.Config.btHearMsgFColor, M2Share.Config.btHearMsgBColor, 0,
                    sMsg);
            }
        }

        protected bool InSafeArea()
        {
            if (Envir == null)
            {
                return false;
            }
            var result = Envir.Flag.boSAFE;
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
            WAbil.DC = (ushort)HUtil32.MakeLong(HUtil32.LoWord(WAbil.DC), HUtil32.HiWord(WAbil.DC));
            var n8 = 0;
            if ((Race == MonsterConst.MonsterWhiteskeleton) ||
                (Race == MonsterConst.MonsterElfmonster) ||
                (Race == MonsterConst.MonsterElfwarrior))
            {
                WAbil.DC = (ushort)HUtil32.MakeLong(HUtil32.LoWord(WAbil.DC), HUtil32.Round((SlaveExpLevel * 0.1 + 0.3) * 3.0 * SlaveExpLevel + HUtil32.HiWord(WAbil.DC)));
                n8 = n8 + HUtil32.Round((SlaveExpLevel * 0.1 + 0.3) * WAbil.MaxHP) * SlaveExpLevel;
                n8 = n8 + WAbil.MaxHP;
                if (SlaveExpLevel > 0)
                {
                    WAbil.MaxHP = (ushort)n8;
                }
                else
                {
                    WAbil.MaxHP = WAbil.MaxHP;
                }
            }
            else
            {
                n8 = WAbil.MaxHP;
                WAbil.DC = (ushort)HUtil32.MakeLong(HUtil32.LoWord(WAbil.DC), HUtil32.Round(SlaveExpLevel * 2 + HUtil32.HiWord(WAbil.DC)));
                n8 = n8 + HUtil32.Round(WAbil.MaxHP * 0.15) * SlaveExpLevel;
                WAbil.MaxHP = (ushort)HUtil32._MIN(HUtil32.Round(WAbil.MaxHP + SlaveExpLevel * 60), n8);
            }
        }

        public void SendFirstMsg(BaseObject baseObject, short wIdent, short wParam, int lParam1, int lParam2,
            int lParam3, string sMsg)
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
                        DeliveryTime = 0,
                        BaseObject = baseObject.ActorId
                    };
                    if (!string.IsNullOrEmpty(sMsg))
                    {
                        sendMessage.Buff = sMsg;
                    }

                    MsgList.Insert(0, sendMessage);
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
        }

        public void SendMsg(BaseObject baseObject, int wIdent, int wParam, int nParam1, int nParam2, int nParam3,
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
                    BaseObject = baseObject.ActorId,
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
        public void SendDelayMsg(BaseObject baseObject, int wIdent, int wParam, int lParam1, int lParam2, int lParam3, string sMsg, int dwDelay)
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
                        BaseObject = baseObject.ActorId,
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
        public void SendDelayMsg(int baseObject, short wIdent, int wParam, int lParam1, int lParam2, int lParam3,
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
                        LateDelivery = true
                    };
                    if (baseObject == Grobal2.RM_STRUCK)
                    {
                        sendMessage.BaseObject = Grobal2.RM_STRUCK;
                    }
                    else
                    {
                        sendMessage.BaseObject = baseObject;
                    }

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

        private void SendUpdateDelayMsg(BaseObject baseObject, short wIdent, short wParam, int lParam1, int lParam2,
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
                    var sendMessage = MsgList[i];
                    if ((sendMessage.wIdent == wIdent) && (sendMessage.nParam1 == lParam1))
                    {
                        MsgList.RemoveAt(i);
                        Dispose(sendMessage);
                        continue;
                    }
                    i++;
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }

            SendDelayMsg(baseObject.ActorId, wIdent, wParam, lParam1, lParam2, lParam3, sMsg, dwDelay);
        }

        public void SendUpdateMsg(BaseObject baseObject, int wIdent, int wParam, int lParam1, int lParam2, int lParam3,
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

                    var sendMessage = MsgList[i];
                    if (sendMessage.wIdent == wIdent)
                    {
                        MsgList.RemoveAt(i);
                        Dispose(sendMessage);
                        continue;
                    }

                    i++;
                }
            }
            finally
            {

                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }

            SendMsg(baseObject, wIdent, wParam, lParam1, lParam2, lParam3, sMsg);
        }

        public void SendActionMsg(BaseObject baseObject, int wIdent, int wParam, int lParam1, int lParam2, int lParam3,
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
                    var sendMessage = MsgList[i];
                    if ((sendMessage.wIdent == Grobal2.CM_TURN) || (sendMessage.wIdent == Grobal2.CM_WALK) ||
                        (sendMessage.wIdent == Grobal2.CM_SITDOWN) || (sendMessage.wIdent == Grobal2.CM_HORSERUN) ||
                        (sendMessage.wIdent == Grobal2.CM_RUN) || (sendMessage.wIdent == Grobal2.CM_HIT) ||
                        (sendMessage.wIdent == Grobal2.CM_HEAVYHIT) || (sendMessage.wIdent == Grobal2.CM_BIGHIT) ||
                        (sendMessage.wIdent == Grobal2.CM_POWERHIT) || (sendMessage.wIdent == Grobal2.CM_LONGHIT) ||
                        (sendMessage.wIdent == Grobal2.CM_WIDEHIT) || (sendMessage.wIdent == Grobal2.CM_FIREHIT))
                    {
                        MsgList.RemoveAt(i);
                        Dispose(sendMessage);
                        continue;
                    }
                    i++;
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }

            SendMsg(baseObject, wIdent, wParam, lParam1, lParam2, lParam3, sMsg);
        }

        protected virtual bool GetMessage(ref ProcessMessage msg)
        {
            var result = false;
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
                    msg = new ProcessMessage();
                    msg.wIdent = sendMessage.wIdent;
                    msg.wParam = sendMessage.wParam;
                    msg.nParam1 = sendMessage.nParam1;
                    msg.nParam2 = sendMessage.nParam2;
                    msg.nParam3 = sendMessage.nParam3;
                    if (sendMessage.BaseObject > 0)
                    {
                        msg.BaseObject = sendMessage.BaseObject;
                    }
                    msg.DeliveryTime = sendMessage.DeliveryTime;
                    msg.LateDelivery = sendMessage.LateDelivery;
                    msg.Msg = !string.IsNullOrEmpty(sendMessage.Buff) ? sendMessage.Buff : string.Empty;
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

        public bool GetMapBaseObjects(Envirnoment envir, int nX, int nY, int nRage, IList<BaseObject> rList)
        {
            const string sExceptionMsg = "[Exception] TBaseObject::GetMapBaseObjects";
            if (rList == null)
            {
                return false;
            }
            try
            {
                var nStartX = nX - nRage;
                var nEndX = nX + nRage;
                var nStartY = nY - nRage;
                var nEndY = nY + nRage;
                for (var x = nStartX; x <= nEndX; x++)
                {
                    for (var y = nStartY; y <= nEndY; y++)
                    {
                        var cellsuccess = false;
                        var cellInfo = envir.GetCellInfo(x, y, ref cellsuccess);
                        if (cellsuccess && cellInfo.IsAvailable)
                        {
                            for (var i = 0; i < cellInfo.Count; i++)
                            {
                                var osObject = cellInfo.ObjList[i];
                                if (osObject != null && osObject.ActorObject)
                                {
                                    var baseObject = M2Share.ActorMgr.Get(osObject.CellObjId);
                                    if (baseObject != null && !baseObject.Death && !baseObject.Ghost)
                                    {
                                        rList.Add(baseObject);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                M2Share.Log.Error(sExceptionMsg);
            }
            return true;
        }

        public void SendRefMsg(int wIdent, int wParam, int nParam1, int nParam2, int nParam3, string sMsg)
        {
            const string sExceptionMsg = "[Exception] TBaseObject::SendRefMsg Name = {0}";
            if (Envir == null)
            {
                M2Share.Log.Error(CharName + " SendRefMsg nil PEnvir ");
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
                BaseObject baseObject;
                if (((HUtil32.GetTickCount() - SendRefMsgTick) >= 500) || (VisibleHumanList.Count == 0))
                {
                    SendRefMsgTick = HUtil32.GetTickCount();
                    VisibleHumanList.Clear();
                    var nLx = (short)(CurrX - M2Share.Config.SendRefMsgRange); // 12
                    var nHx = (short)(CurrX + M2Share.Config.SendRefMsgRange); // 12
                    var nLy = (short)(CurrY - M2Share.Config.SendRefMsgRange); // 12
                    var nHy = (short)(CurrY + M2Share.Config.SendRefMsgRange); // 12
                    for (var nCx = nLx; nCx <= nHx; nCx++)
                    {
                        for (var nCy = nLy; nCy <= nHy; nCy++)
                        {
                            var cellsuccess = false;
                            var cellInfo = Envir.GetCellInfo(nCx, nCy, ref cellsuccess);
                            if (cellsuccess)
                            {
                                if (cellInfo.IsAvailable)
                                {
                                    for (var i = 0; i < cellInfo.Count; i++)
                                    {
                                        var osObject = cellInfo.ObjList[i];
                                        if (osObject != null)
                                        {
                                            if (osObject.ActorObject)
                                            {
                                                if ((HUtil32.GetTickCount() - osObject.AddTime) >= 60 * 1000)
                                                {
                                                    cellInfo.Remove(osObject);
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
                                                        baseObject = M2Share.ActorMgr.Get(osObject.CellObjId);
                                                        if ((baseObject != null) && !baseObject.Ghost)
                                                        {
                                                            if (baseObject.Race == Grobal2.RC_PLAYOBJECT)
                                                            {
                                                                baseObject.SendMsg(this, wIdent, wParam, nParam1, nParam2, nParam3, sMsg);
                                                                VisibleHumanList.Add(baseObject);
                                                            }
                                                            else if (baseObject.WantRefMsg)
                                                            {
                                                                if ((wIdent == Grobal2.RM_STRUCK) || (wIdent == Grobal2.RM_HEAR) || (wIdent == Grobal2.RM_DEATH))
                                                                {
                                                                    baseObject.SendMsg(this, wIdent, wParam, nParam1, nParam2, nParam3, sMsg);
                                                                    VisibleHumanList.Add(baseObject);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        cellInfo.Remove(osObject);
                                                        if (cellInfo.Count <= 0)
                                                        {
                                                            cellInfo.Dispose();
                                                        }
                                                        M2Share.Log.Error(Format(sExceptionMsg, CharName));
                                                        M2Share.Log.Error(e.Message);
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
                    baseObject = VisibleHumanList[nC];
                    if (baseObject.Ghost)
                    {
                        continue;
                    }

                    if ((baseObject.Envir == Envir) && (Math.Abs(baseObject.CurrX - CurrX) < 11) &&
                        (Math.Abs(baseObject.CurrY - CurrY) < 11))
                    {
                        if (baseObject.Race == Grobal2.RC_PLAYOBJECT)
                        {
                            baseObject.SendMsg(this, wIdent, wParam, nParam1, nParam2, nParam3, sMsg);
                        }
                        else if (baseObject.WantRefMsg)
                        {
                            if ((wIdent == Grobal2.RM_STRUCK) || (wIdent == Grobal2.RM_HEAR) ||
                                (wIdent == Grobal2.RM_DEATH))
                            {
                                baseObject.SendMsg(this, wIdent, wParam, nParam1, nParam2, nParam3, sMsg);
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

        public int GetFeature(BaseObject baseObject)
        {
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                byte nDress = 0;
                StdItem stdItem = null;
                if (UseItems[Grobal2.U_DRESS] != null && UseItems[Grobal2.U_DRESS].Index > 0) // 衣服
                {
                    stdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_DRESS].Index);
                    if (stdItem != null)
                    {
                        nDress = (byte)(stdItem.Shape * 2);
                    }
                }

                nDress += (byte)Gender;
                byte nWeapon = 0;
                if (UseItems[Grobal2.U_WEAPON] != null && UseItems[Grobal2.U_WEAPON].Index > 0) // 武器
                {
                    stdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_WEAPON].Index);
                    if (stdItem != null)
                    {
                        nWeapon = (byte)(stdItem.Shape * 2);
                    }
                }

                nWeapon += (byte)Gender;
                var nHair = (byte)(Hair * 2 + (byte)Gender);
                return Grobal2.MakeHumanFeature(0, nDress, nWeapon, nHair);
            }

            var bo25 = baseObject != null && baseObject.BoRace;
            if (bo25)
            {
                var nRaceImg = RaceImg;
                var nAppr = (byte)Appr;
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
            return Grobal2.MakeMonsterFeature(RaceImg, MonsterWeapon, Appr);
        }

        public int GetCharStatus()
        {
            //0x80000000 指十六进制值，转成二进制则为10000000000000000000000000000000 然后Shr右移
            //例：I为3,右移3位，得到二进制值：10000000000000000000000000000
            //    I为6,右移6位，得到二进制值: 10000000000000000000000000
            //or 代表运算, 需要两个运算数，即两个数的位运算，只有其中一个是1就返回1; 都是0才返回0
            //and 表示 当对应位均为1时返回1，其余为0
            //从上面算法得到，最终 nStatus得到是1,
            var nStatus = 0;
            for (var i = 0; i < StatusArr.Length; i++)
            {
                if (StatusArr[i] > 0)
                {
                    nStatus = (int)((long)nStatus | (0x80000000 >> i));
                }
            }
            return nStatus | (CharStatusEx & 0x0000FFFF);
        }

        public void AbilCopyToWAbil()
        {
            WAbil = Abil;
        }

        public virtual void Initialize()
        {
            AbilCopyToWAbil();
            for (var i = 0; i < MagicList.Count; i++)
            {
                if (MagicList[i].Level >= 4)
                {
                    MagicList[i].Level = 0;
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
            if (M2Share.Config.MonSayMsg)
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
            Envir.DeleteFromMap(CurrX, CurrY, MapCell, this);
            SendRefMsg(Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
        }

        protected void KickException()
        {
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                MapName = M2Share.Config.HomeMap;
                CurrX = M2Share.Config.HomeX;
                CurrY = M2Share.Config.HomeY;
                var playObject = this as PlayObject;
                playObject.m_boEmergencyClose = true;
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
            var result = true;
            if (Envir == null)
            {
                M2Share.Log.Error("Walk nil PEnvir");
                return true;
            }
            try
            {
                var cellsuccess = false;
                var cellInfo = Envir.GetCellInfo(CurrX, CurrY, ref cellsuccess);
                if (cellsuccess && cellInfo.IsAvailable)
                {
                    for (var i = 0; i < cellInfo.Count; i++)
                    {
                        var osObject = cellInfo.ObjList[i];
                        if (osObject == null)
                        {
                            continue;
                        }
                        switch (osObject.CellType)
                        {
                            case CellType.Route:
                                var gateObj = (GateObject)M2Share.CellObjectSystem.Get(osObject.CellObjId);
                                if (gateObj != null)
                                {
                                    if (Race == Grobal2.RC_PLAYOBJECT)
                                    {
                                        if (Envir.ArroundDoorOpened(CurrX, CurrY))
                                        {
                                            if ((!gateObj.DEnvir.Flag.boNEEDHOLE) || (M2Share.EventMgr.GetEvent(Envir, CurrX, CurrY, Grobal2.ET_DIGOUTZOMBI) != null))
                                            {
                                                if (M2Share.ServerIndex == gateObj.DEnvir.ServerIndex)
                                                {
                                                    if (!EnterAnotherMap(gateObj.DEnvir, gateObj.nDMapX, gateObj.nDMapY))
                                                    {
                                                        result = false;
                                                    }
                                                }
                                                else
                                                {
                                                    DisappearA();
                                                    MBo316 = true;
                                                    var playObject = this as PlayObject;
                                                    playObject.m_sSwitchMapName = gateObj.DEnvir.MapName;
                                                    playObject.m_nSwitchMapX = gateObj.nDMapX;
                                                    playObject.m_nSwitchMapY = gateObj.nDMapY;
                                                    playObject.m_boSwitchData = true;
                                                    playObject.m_nServerIndex = gateObj.DEnvir.ServerIndex;
                                                    playObject.m_boEmergencyClose = true;
                                                    playObject.m_boReconnection = true;
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
                            case CellType.Event:
                                {
                                    MirEvent mapEvent = null;
                                    var owinEvent = (MirEvent)M2Share.CellObjectSystem.Get(osObject.CellObjId);
                                    if (owinEvent.OwnBaseObject != null)
                                    {
                                        mapEvent = (MirEvent)M2Share.CellObjectSystem.Get(osObject.CellObjId);
                                    }
                                    if (mapEvent != null)
                                    {
                                        if (mapEvent.OwnBaseObject.IsProperTarget(this))
                                        {
                                            SendMsg(mapEvent.OwnBaseObject, Grobal2.RM_MAGSTRUCK_MINE, 0, mapEvent.Damage, 0, 0, "");
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
                M2Share.Log.Error(Format(sExceptionMsg, CharName, MapName, CurrX, CurrY));
                M2Share.Log.Error(e.Message);
            }
            return result;
        }

        /// <summary>
        /// 切换地图
        /// </summary>
        private bool EnterAnotherMap(Envirnoment envir, short nDMapX, short nDMapY)
        {
            var result = false;
            const string sExceptionMsg = "[Exception] TBaseObject::EnterAnotherMap";
            try
            {
                if (Abil.Level < envir.RequestLevel)
                {
                    SysMsg($"需要 {envir.Flag.nL - 1} 级以上才能进入 {envir.MapDesc}", MsgColor.Red, MsgType.Hint);
                    return false;
                }
                if (envir.QuestNpc != null)
                {
                    envir.QuestNpc.Click(this as PlayObject);
                }
                if (envir.Flag.nNEEDSETONFlag >= 0)
                {
                    if (GetQuestFalgStatus(envir.Flag.nNEEDSETONFlag) != envir.Flag.nNeedONOFF)
                    {
                        return false;
                    }
                }
                var cellsuccess = false;
                envir.GetCellInfo(nDMapX, nDMapY, ref cellsuccess);
                if (!cellsuccess)
                {
                    return false;
                }
                var castle = M2Share.CastleMgr.IsCastlePalaceEnvir(envir);
                if ((castle != null) && (Race == Grobal2.RC_PLAYOBJECT))
                {
                    if (!castle.CheckInPalace(CurrX, CurrY, this))
                    {
                        return false;
                    }
                }
                if (envir.Flag.boNOHORSE)
                {
                    OnHorse = false;
                }
                var oldEnvir = this.Envir;
                var nOldX = CurrX;
                var nOldY = CurrY;
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
                this.Envir = envir;
                MapName = envir.MapName;
                MapFileName = envir.MapFileName;
                CurrX = nDMapX;
                CurrY = nDMapY;
                SendMsg(this, Grobal2.RM_CHANGEMAP, 0, 0, 0, 0, envir.MapFileName);
                if (AddToMap())
                {
                    MapMoveTick = HUtil32.GetTickCount();
                    MBo316 = true;
                    result = true;
                }
                else
                {
                    this.Envir = oldEnvir;
                    CurrX = nOldX;
                    CurrY = nOldY;
                    this.Envir.AddToMap(CurrX, CurrY, MapCell, this);
                }
                OnEnvirnomentChanged();
                if (Race == Grobal2.RC_PLAYOBJECT) // 复位泡点，及金币，时间
                {
                    (this as PlayObject).m_dwIncGamePointTick = HUtil32.GetTickCount();
                    (this as PlayObject).m_dwIncGameGoldTick = HUtil32.GetTickCount();
                    (this as PlayObject).m_dwAutoGetExpTick = HUtil32.GetTickCount();
                }
                if (this.Envir.Flag.boFight3Zone && (this.Envir.Flag.boFight3Zone != oldEnvir.Flag.boFight3Zone))
                {
                    RefShowName();
                }
            }
            catch
            {
                M2Share.Log.Error(sExceptionMsg);
            }

            return result;
        }

        protected void TurnTo(byte nDir)
        {
            Direction = nDir;
            SendRefMsg(Grobal2.RM_TURN, nDir, CurrX, CurrY, 0, "");
        }

        public void SysMsg(string sMsg, MsgColor msgColor, MsgType msgType)
        {
            if (M2Share.Config.ShowPreFixMsg)
            {
                switch (msgType)
                {
                    case MsgType.Mon:
                        sMsg = M2Share.Config.MonSayMsgPreFix + sMsg;
                        break;
                    case MsgType.Hint:
                        sMsg = M2Share.Config.HintMsgPreFix + sMsg;
                        break;
                    case MsgType.GameManger:
                        sMsg = M2Share.Config.GameManagerRedMsgPreFix + sMsg;
                        break;
                    case MsgType.System:
                        sMsg = M2Share.Config.SysMsgPreFix + sMsg;
                        break;
                    case MsgType.Cust:
                        sMsg = M2Share.Config.CustMsgPreFix + sMsg;
                        break;
                    case MsgType.Castle:
                        sMsg = M2Share.Config.CastleMsgPreFix + sMsg;
                        break;
                }
            }

            if (msgType == MsgType.Notice) // 如果发的是公告
            {
                var str = string.Empty;
                var fColor = string.Empty;
                var bColor = string.Empty;
                var nTime = string.Empty;
                if (sMsg[0] == '[') // 顶部滚动公告
                {
                    sMsg = HUtil32.ArrestStringEx(sMsg, "[", "]", ref str);
                    bColor = HUtil32.GetValidStrCap(str, ref fColor, new string[] { "," });
                    if (M2Share.Config.ShowPreFixMsg)
                    {
                        sMsg = M2Share.Config.LineNoticePreFix + sMsg;
                    }

                    SendMsg(this, Grobal2.RM_MOVEMESSAGE, 0, HUtil32.Str_ToInt(fColor, 255),
                        HUtil32.Str_ToInt(bColor, 255), 0, sMsg);
                }
                else if (sMsg[0] == '<') // 聊天框彩色公告
                {
                    sMsg = HUtil32.ArrestStringEx(sMsg, "<", ">", ref str);
                    bColor = HUtil32.GetValidStrCap(str, ref fColor, new string[] { "," });
                    if (M2Share.Config.ShowPreFixMsg)
                    {
                        sMsg = M2Share.Config.LineNoticePreFix + sMsg;
                    }

                    SendMsg(this, Grobal2.RM_SYSMESSAGE, 0, HUtil32.Str_ToInt(fColor, 255),
                        HUtil32.Str_ToInt(bColor, 255), 0, sMsg);
                }
                else if (sMsg[0] == '{') // 屏幕居中公告
                {
                    sMsg = HUtil32.ArrestStringEx(sMsg, "{", "}", ref str);
                    str = HUtil32.GetValidStrCap(str, ref fColor, new string[] { "," });
                    str = HUtil32.GetValidStrCap(str, ref bColor, new string[] { "," });
                    str = HUtil32.GetValidStrCap(str, ref nTime, new string[] { "," });
                    if (M2Share.Config.ShowPreFixMsg)
                    {
                        sMsg = M2Share.Config.LineNoticePreFix + sMsg;
                    }

                    SendMsg(this, Grobal2.RM_MOVEMESSAGE, 1, HUtil32.Str_ToInt(fColor, 255),
                        HUtil32.Str_ToInt(bColor, 255), HUtil32.Str_ToInt(nTime, 0), sMsg);
                }
                else
                {
                    switch (msgColor)
                    {
                        case MsgColor.Red: // 控制公告的颜色
                            if (M2Share.Config.ShowPreFixMsg)
                            {
                                sMsg = M2Share.Config.LineNoticePreFix + sMsg;
                            }

                            SendMsg(this, Grobal2.RM_SYSMESSAGE, 0, M2Share.Config.RedMsgFColor,
                                M2Share.Config.RedMsgBColor, 0, sMsg);
                            break;
                        case MsgColor.Green:
                            if (M2Share.Config.ShowPreFixMsg)
                            {
                                sMsg = M2Share.Config.LineNoticePreFix + sMsg;
                            }

                            SendMsg(this, Grobal2.RM_SYSMESSAGE, 0, M2Share.Config.GreenMsgFColor,
                                M2Share.Config.GreenMsgBColor, 0, sMsg);
                            break;
                        case MsgColor.Blue:
                            if (M2Share.Config.ShowPreFixMsg)
                            {
                                sMsg = M2Share.Config.LineNoticePreFix + sMsg;
                            }

                            SendMsg(this, Grobal2.RM_SYSMESSAGE, 0, M2Share.Config.BlueMsgFColor,
                                M2Share.Config.BlueMsgBColor, 0, sMsg);
                            break;
                    }
                }
            }
            else
            {
                switch (msgColor)
                {
                    case MsgColor.Green:
                        SendMsg(this, Grobal2.RM_SYSMESSAGE, 0, M2Share.Config.GreenMsgFColor,
                            M2Share.Config.GreenMsgBColor, 0, sMsg);
                        break;
                    case MsgColor.Blue:
                        SendMsg(this, Grobal2.RM_SYSMESSAGE, 0, M2Share.Config.BlueMsgFColor,
                            M2Share.Config.BlueMsgBColor, 0, sMsg);
                        break;
                    default:
                        if (msgType == MsgType.Cust)
                        {
                            SendMsg(this, Grobal2.RM_SYSMESSAGE, 0, M2Share.Config.CustMsgFColor,
                                M2Share.Config.CustMsgBColor, 0, sMsg);
                        }
                        else
                        {
                            SendMsg(this, Grobal2.RM_SYSMESSAGE, 0, M2Share.Config.RedMsgFColor,
                                M2Share.Config.RedMsgBColor, 0, sMsg);
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// 怪物说话
        /// </summary>
        /// <param name="attackBaseObject"></param>
        /// <param name="monStatus"></param>
        protected void MonsterSayMsg(BaseObject attackBaseObject, MonStatus monStatus)
        {
            if (SayMsgList == null)
            {
                return;
            }

            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                return;
            }

            var sAttackName = string.Empty;
            if (attackBaseObject != null)
            {
                if ((attackBaseObject.Race != Grobal2.RC_PLAYOBJECT) && (attackBaseObject.Master == null))
                {
                    return;
                }

                if (attackBaseObject.Master != null)
                {
                    sAttackName = attackBaseObject.Master.CharName;
                }
                else
                {
                    sAttackName = attackBaseObject.CharName;
                }
            }

            TMonSayMsg monSayMsg = null;
            var sMsg = string.Empty;
            for (var i = 0; i < SayMsgList.Count; i++)
            {
                monSayMsg = SayMsgList[i];
                sMsg = monSayMsg.sSayMsg.Replace("%s", M2Share.FilterShowName(CharName));
                sMsg = sMsg.Replace("%d", sAttackName);
                if ((monSayMsg.State == monStatus) && (M2Share.RandomNumber.Random(monSayMsg.nRate) == 0))
                {
                    if (monStatus == MonStatus.MonGen)
                    {
                        M2Share.WorldEngine.SendBroadCastMsg(sMsg, MsgType.Mon);
                        break;
                    }

                    if (monSayMsg.Color == MsgColor.White)
                    {
                        ProcessSayMsg(sMsg);
                    }
                    else
                    {
                        attackBaseObject.SysMsg(sMsg, monSayMsg.Color, MsgType.Mon);
                    }

                    break;
                }
            }
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

        /// <summary>
        /// 设置肉的品质
        /// </summary>
        protected void ApplyMeatQuality()
        {
            for (var i = 0; i < ItemList.Count; i++)
            {
                var stdItem = M2Share.WorldEngine.GetStdItem(ItemList[i].Index);
                if (stdItem != null)
                {
                    if (stdItem.StdMode == 40)
                    {
                        ItemList[i].Dura = MeatQuality;
                    }
                }
            }
        }

        protected bool TakeBagItems(BaseObject baseObject)
        {
            var result = false;
            while (true)
            {
                if (baseObject.ItemList.Count <= 0)
                {
                    break;
                }
                var userItem = baseObject.ItemList[0];
                if (!AddItemToBag(userItem))
                {
                    break;
                }
                if (this is PlayObject)
                {
                    var playObject = this as PlayObject;
                    playObject.SendAddItem(userItem);
                    result = true;
                }
                baseObject.ItemList.RemoveAt(0);
            }
            return result;
        }

        /// <summary>
        /// 散落金币
        /// </summary>
        /// <param name="goldOfCreat"></param>
        private void ScatterGolds(BaseObject goldOfCreat)
        {
            int I;
            int nGold;
            if (Gold > 0)
            {
                I = 0;
                while (true)
                {
                    if (Gold > M2Share.Config.MonOneDropGoldCount)
                    {
                        nGold = M2Share.Config.MonOneDropGoldCount;
                        Gold = Gold - M2Share.Config.MonOneDropGoldCount;
                    }
                    else
                    {
                        nGold = Gold;
                        Gold = 0;
                    }
                    if (nGold > 0)
                    {
                        if (!DropGoldDown(nGold, true, goldOfCreat, this))
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

        public void SetLastHiter(BaseObject baseObject)
        {
            LastHiter = baseObject;
            LastHiterTick = HUtil32.GetTickCount();
            if (ExpHitter == null)
            {
                ExpHitter = baseObject;
                ExpHitterTick = HUtil32.GetTickCount();
            }
            else
            {
                if (ExpHitter == baseObject)
                {
                    ExpHitterTick = HUtil32.GetTickCount();
                }
            }
        }

        public void SetPkFlag(BaseObject baseObject)
        {
            if ((PvpLevel() < 2) && (baseObject.PvpLevel() < 2) && (!Envir.Flag.boFightZone) &&
                (!Envir.Flag.boFight3Zone) && !PvpFlag)
            {
                baseObject.MDwPkTick = HUtil32.GetTickCount();
                if (!baseObject.PvpFlag)
                {
                    baseObject.PvpFlag = true;
                    baseObject.RefNameColor();
                }
            }
        }

        protected bool IsGoodKilling(BaseObject cert)
        {
            return cert.PvpFlag;
        }

        private bool IsAttackTarget_sub_4C88E4()
        {
            return true;
        }

        /// <summary>
        /// 是否可以攻击的目标
        /// </summary>
        /// <param name="baseObject"></param>
        /// <returns></returns>
        protected virtual bool IsAttackTarget(BaseObject baseObject)
        {
            var result = false;
            if ((baseObject == null) || (baseObject == this))
            {
                return false;
            }

            if (Race >= Grobal2.RC_ANIMAL)
            {
                if (Master != null)
                {
                    if ((Master.LastHiter == baseObject) || (Master.ExpHitter == baseObject) ||
                        (Master.TargetCret == baseObject))
                    {
                        result = true;
                    }

                    if (baseObject.TargetCret != null)
                    {
                        if ((baseObject.TargetCret == Master) || (baseObject.TargetCret.Master == Master) &&
                            (baseObject.Race != Grobal2.RC_PLAYOBJECT))
                        {
                            result = true;
                        }
                    }

                    if ((baseObject.TargetCret == this) && (baseObject.Race >= Grobal2.RC_ANIMAL))
                    {
                        result = true;
                    }

                    if (baseObject.Master != null)
                    {
                        if ((baseObject.Master == Master.LastHiter) ||
                            (baseObject.Master == Master.TargetCret))
                        {
                            result = true;
                        }
                    }

                    if (baseObject.Master == Master)
                    {
                        result = false;
                    }

                    if (baseObject.HolySeize)
                    {
                        result = false;
                    }

                    if (Master.SlaveRelax)
                    {
                        result = false;
                    }

                    if (baseObject.Race == Grobal2.RC_PLAYOBJECT)
                    {
                        if (baseObject.InSafeZone())
                        {
                            result = false;
                        }
                    }

                    BreakCrazyMode();
                }
                else
                {
                    if (baseObject.Race == Grobal2.RC_PLAYOBJECT)
                    {
                        result = true;
                    }

                    if ((Race > Grobal2.RC_PEACENPC) && (Race < Grobal2.RC_ANIMAL))
                    {
                        result = true;
                    }

                    if (baseObject.Master != null)
                    {
                        result = true;
                    }
                }

                if (CrazyMode && ((baseObject.Race == Grobal2.RC_PLAYOBJECT) ||
                                      (baseObject.Race > Grobal2.RC_PEACENPC)))
                {
                    result = true;
                }

                if (NastyMode && ((baseObject.Race < Grobal2.RC_NPC) ||
                                      (baseObject.Race > Grobal2.RC_PEACENPC)))
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
                            if ((baseObject.Race < Grobal2.RC_NPC) ||
                                (baseObject.Race > Grobal2.RC_PEACENPC))
                            {
                                result = true;
                            }

                            if (M2Share.Config.PveServer)
                            {
                                result = IsAttackTarget_sub_4C88E4();
                            }

                            break;
                        case AttackMode.HAM_PEACE:
                            if (baseObject.Race >= Grobal2.RC_ANIMAL)
                            {
                                result = true;
                            }

                            break;
                        case AttackMode.HAM_DEAR:
                            if (baseObject != (this as PlayObject).m_DearHuman)
                            {
                                result = true;
                            }

                            break;
                        case AttackMode.HAM_MASTER:
                            if (baseObject.Race == Grobal2.RC_PLAYOBJECT)
                            {
                                result = true;
                                if ((this as PlayObject).m_boMaster)
                                {
                                    for (var i = 0; i < (this as PlayObject).m_MasterList.Count; i++)
                                    {
                                        if ((this as PlayObject).m_MasterList[i] == baseObject)
                                        {
                                            result = false;
                                            break;
                                        }
                                    }
                                }

                                if ((baseObject as PlayObject).m_boMaster)
                                {
                                    for (var i = 0; i < (baseObject as PlayObject).m_MasterList.Count; i++)
                                    {
                                        if ((baseObject as PlayObject).m_MasterList[i] == this)
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
                            if ((baseObject.Race < Grobal2.RC_NPC) ||
                                (baseObject.Race > Grobal2.RC_PEACENPC))
                            {
                                result = true;
                            }

                            if (baseObject.Race == Grobal2.RC_PLAYOBJECT)
                            {
                                if (IsGroupMember(baseObject))
                                {
                                    result = false;
                                }
                            }

                            if (M2Share.Config.PveServer)
                            {
                                result = IsAttackTarget_sub_4C88E4();
                            }

                            break;
                        case AttackMode.HAM_GUILD:
                            if ((baseObject.Race < Grobal2.RC_NPC) ||
                                (baseObject.Race > Grobal2.RC_PEACENPC))
                            {
                                result = true;
                            }

                            if (baseObject.Race == Grobal2.RC_PLAYOBJECT)
                            {
                                if (MyGuild != null)
                                {
                                    if (MyGuild.IsMember(baseObject.CharName))
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
                                result = IsAttackTarget_sub_4C88E4();
                            }

                            break;
                        case AttackMode.HAM_PKATTACK:
                            if ((baseObject.Race < Grobal2.RC_NPC) ||
                                (baseObject.Race > Grobal2.RC_PEACENPC))
                            {
                                result = true;
                            }

                            if (baseObject.Race == Grobal2.RC_PLAYOBJECT)
                            {
                                if (PvpLevel() >= 2)
                                {
                                    if (baseObject.PvpLevel() < 2)
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
                                    if (baseObject.PvpLevel() >= 2)
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        result = false;
                                    }
                                }
                            }

                            if (M2Share.Config.PveServer)
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

            if (baseObject.AdminMode || baseObject.StoneMode)
            {
                result = false;
            }

            return result;
        }

        public virtual bool IsProperTarget(BaseObject baseObject)
        {
            var result = IsAttackTarget(baseObject);
            if (result)
            {
                if ((Race == Grobal2.RC_PLAYOBJECT) && (baseObject.Race == Grobal2.RC_PLAYOBJECT))
                {
                    result = IsProtectTarget(baseObject);
                }
            }

            if ((baseObject != null) && (Race == Grobal2.RC_PLAYOBJECT) && (baseObject.Master != null) &&
                (baseObject.Race != Grobal2.RC_PLAYOBJECT))
            {
                if (baseObject.Master == this)
                {
                    if (AttatckMode != AttackMode.HAM_ALL)
                    {
                        result = false;
                    }
                }
                else
                {
                    result = IsAttackTarget(baseObject.Master);
                    if (InSafeZone() || baseObject.InSafeZone())
                    {
                        result = false;
                    }
                }
            }
            return result;
        }

        protected void WeightChanged()
        {
            WAbil.Weight = RecalcBagWeight();
            SendUpdateMsg(this, Grobal2.RM_WEIGHTCHANGED, 0, 0, 0, 0, "");
        }

        public bool InSafeZone()
        {
            if (Envir == null)
            {
                return true;
            }
            var result = Envir.Flag.boSAFE;
            if (result)
            {
                return true;
            }
            if ((Envir.MapName != M2Share.Config.RedHomeMap) ||
                (Math.Abs(CurrX - M2Share.Config.RedHomeX) > M2Share.Config.SafeZoneSize) ||
                (Math.Abs(CurrY - M2Share.Config.RedHomeY) > M2Share.Config.SafeZoneSize))
            {
                result = false;
            }
            else
            {
                return true;
            }
            for (var i = 0; i < M2Share.StartPointList.Count; i++)
            {
                if (M2Share.StartPointList[i].m_sMapName == Envir.MapName)
                {
                    if (M2Share.StartPointList[i] != null)
                    {
                        int nSafeX = M2Share.StartPointList[i].m_nCurrX;
                        int nSafeY = M2Share.StartPointList[i].m_nCurrY;
                        if ((Math.Abs(CurrX - nSafeX) <= M2Share.Config.SafeZoneSize) &&
                            (Math.Abs(CurrY - nSafeY) <= M2Share.Config.SafeZoneSize))
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        public bool InSafeZone(Envirnoment envir, int nX, int nY)
        {
            if (this.Envir == null)
            {
                return true;
            }
            var result = this.Envir.Flag.boSAFE;
            if (result)
            {
                return true;
            }
            if ((envir.MapName != M2Share.Config.RedHomeMap) ||
                (Math.Abs(nX - M2Share.Config.RedHomeX) > M2Share.Config.SafeZoneSize) ||
                (Math.Abs(nY - M2Share.Config.RedHomeY) > M2Share.Config.SafeZoneSize))
            {
                result = false;
            }
            else
            {
                return true;
            }
            for (var i = 0; i < M2Share.StartPointList.Count; i++)
            {
                if (M2Share.StartPointList[i].m_sMapName == envir.MapName)
                {
                    if (M2Share.StartPointList[i] != null)
                    {
                        int nSafeX = M2Share.StartPointList[i].m_nCurrX;
                        int nSafeY = M2Share.StartPointList[i].m_nCurrY;
                        if ((Math.Abs(nX - nSafeX) <= M2Share.Config.SafeZoneSize) &&
                            (Math.Abs(nY - nSafeY) <= M2Share.Config.SafeZoneSize))
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
            HolySeize = true;
            HolySeizeTick = HUtil32.GetTickCount();
            HolySeizeInterval = dwInterval;
            RefNameColor();
        }

        public void BreakHolySeizeMode()
        {
            HolySeize = false;
            RefNameColor();
        }

        public void OpenCrazyMode(int nTime)
        {
            CrazyMode = true;
            CrazyModeTick = HUtil32.GetTickCount();
            CrazyModeInterval = nTime * 1000;
            RefNameColor();
        }

        public void BreakCrazyMode()
        {
            if (CrazyMode)
            {
                CrazyMode = false;
                RefNameColor();
            }
        }

        private void LeaveGroup()
        {
            const string sExitGropMsg = "{0} 已经退出了本组.";
            SendGroupText(Format(sExitGropMsg, CharName));
            GroupOwner = null;
            SendMsg(this, Grobal2.RM_GROUPCANCEL, 0, 0, 0, 0, "");
        }

        protected UserMagic GetMagicInfo(int nMagicId)
        {
            UserMagic result = null;
            UserMagic userMagic;
            for (var i = 0; i < MagicList.Count; i++)
            {
                userMagic = MagicList[i];
                if (userMagic.Magic.MagicId == nMagicId)
                {
                    result = userMagic;
                    break;
                }
            }

            return result;
        }

        public void TrainSkill(UserMagic userMagic, int nTranPoint)
        {
            if (FastTrain)
            {
                nTranPoint = nTranPoint * 3;
            }
            userMagic.TranPoint += nTranPoint;
        }

        public bool CheckMagicLevelup(UserMagic userMagic)
        {
            var result = false;
            int nLevel;
            if ((userMagic.Level < 4) && (userMagic.Magic.TrainLv >= userMagic.Level))
            {
                nLevel = userMagic.Level;
            }
            else
            {
                nLevel = 0;
            }

            if ((userMagic.Magic.TrainLv > userMagic.Level) &&
                (userMagic.Magic.MaxTrain[nLevel] <= userMagic.TranPoint))
            {
                if (userMagic.Magic.TrainLv > userMagic.Level)
                {
                    userMagic.TranPoint -= userMagic.Magic.MaxTrain[nLevel];
                    userMagic.Level++;
                    SendUpdateDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, userMagic.Magic.MagicId, userMagic.Level,
                        userMagic.TranPoint, "", 800);
                    CheckSeeHealGauge(userMagic);
                }
                else
                {
                    userMagic.TranPoint = userMagic.Magic.MaxTrain[nLevel];
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
            var nFlag = -1;
            GetFrontPosition(ref nX, ref nY);
            if (sSlaveName == M2Share.Config.Dragon)
            {
                nFlag = 1;
            }
            for (var i = SlaveList.Count - 1; i >= 0; i--)
            {
                if (nFlag == 1)
                {
                    if ((SlaveList[i].CharName == M2Share.Config.Dragon) ||
                        (SlaveList[i].CharName == M2Share.Config.Dragon1))
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

        public ushort GetHitStruckDamage(BaseObject target, int nDamage)
        {
            int nArmor;
            var nRnd = HUtil32.LoByte(WAbil.AC) + M2Share.RandomNumber.Random(Math.Abs(HUtil32.HiByte(WAbil.AC) - HUtil32.LoByte(WAbil.AC)) + 1);
            if (nRnd > 0)
            {
                nArmor = HUtil32.LoByte(WAbil.AC) + M2Share.RandomNumber.Random(nRnd);
            }
            else
            {
                nArmor = HUtil32.LoByte(WAbil.AC);
            }
            nDamage = HUtil32._MAX(0, nDamage - nArmor);
            if (nDamage > 0)
            {
                if ((LifeAttrib == Grobal2.LA_UNDEAD) && (target != null))
                {
                    nDamage += target.AddAbil.UndeadPower;
                }
                if (AbilMagBubbleDefence)
                {
                    nDamage = HUtil32.Round(nDamage / 100 * (MagBubbleDefenceLevel + 2) * 8);
                    DamageBubbleDefence(nDamage);
                }
            }
            return (ushort)nDamage;
        }

        public int GetMagStruckDamage(BaseObject baseObject, int nDamage)
        {
            var n14 = HUtil32.LoByte(WAbil.MAC) + M2Share.RandomNumber.Random(HUtil32.HiByte(WAbil.MAC) - HUtil32.LoByte(WAbil.MAC) + 1);
            nDamage = HUtil32._MAX(0, nDamage - n14);
            if ((LifeAttrib == Grobal2.LA_UNDEAD) && (baseObject != null))
            {
                nDamage += AddAbil.UndeadPower;
            }
            if ((nDamage > 0) && AbilMagBubbleDefence)
            {
                nDamage = HUtil32.Round(nDamage / 1.0e2 * (MagBubbleDefenceLevel + 2) * 8.0);
                DamageBubbleDefence(nDamage);
            }
            return nDamage;
        }

        public void StruckDamage(ushort nDamage)
        {
            int nDam;
            int nDura;
            int nOldDura;
            PlayObject playObject;
            StdItem stdItem = null;
            bool bo19;
            if (nDamage <= 0)
            {
                return;
            }
            if ((Race >= 50) && (LastHiter != null) && (LastHiter.Race == Grobal2.RC_PLAYOBJECT)) // 人攻击怪物
            {
                switch (LastHiter.Job)
                {
                    case PlayJob.Warrior:
                        nDamage = (ushort)(nDamage * M2Share.Config.WarrMon / 10);
                        break;
                    case PlayJob.Wizard:
                        nDamage = (ushort)(nDamage * M2Share.Config.WizardMon / 10);
                        break;
                    case PlayJob.Taoist:
                        nDamage = (ushort)(nDamage * M2Share.Config.TaosMon / 10);
                        break;
                }
            }
            if ((Race == Grobal2.RC_PLAYOBJECT) && (LastHiter != null) && (LastHiter.Master != null)) // 怪物攻击人
            {
                nDamage = (ushort)(nDamage * M2Share.Config.MonHum / 10);
            }
            nDam = M2Share.RandomNumber.Random(10) + 5; // 1 0x62
            if (StatusArr[StatuStateConst.POISON_DAMAGEARMOR] > 0)
            {
                nDam = HUtil32.Round(nDam * (M2Share.Config.PosionDamagarmor / 10)); // 1.2
                nDamage = (ushort)HUtil32.Round(nDamage * (M2Share.Config.PosionDamagarmor / 10)); // 1.2
            }
            bo19 = false;
            if (UseItems[Grobal2.U_DRESS] != null && UseItems[Grobal2.U_DRESS].Index > 0)
            {
                nDura = UseItems[Grobal2.U_DRESS].Dura;
                nOldDura = HUtil32.Round(nDura / 1000);
                nDura -= nDam;
                if (nDura <= 0)
                {
                    if (Race == Grobal2.RC_PLAYOBJECT)
                    {
                        playObject = this as PlayObject;
                        playObject.SendDelItems(UseItems[Grobal2.U_DRESS]);
                        stdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_DRESS].Index);
                        if (stdItem.NeedIdentify == 1)
                        {
                            M2Share.AddGameDataLog('3' + "\t" + MapName + "\t" + CurrX + "\t" + CurrY + "\t" +
                                                   CharName + "\t" + stdItem.Name + "\t" +
                                                   UseItems[Grobal2.U_DRESS].MakeIndex + "\t"
                                                   + HUtil32.BoolToIntStr(Race == Grobal2.RC_PLAYOBJECT) +
                                                   "\t" + '0');
                        }
                        UseItems[Grobal2.U_DRESS].Index = 0;
                        FeatureChanged();
                    }
                    UseItems[Grobal2.U_DRESS].Index = 0;
                    UseItems[Grobal2.U_DRESS].Dura = 0;
                    bo19 = true;
                }
                else
                {
                    UseItems[Grobal2.U_DRESS].Dura = (ushort)nDura;
                }

                if (nOldDura != HUtil32.Round(nDura / 1000))
                {
                    SendMsg(this, Grobal2.RM_DURACHANGE, Grobal2.U_DRESS, nDura, UseItems[Grobal2.U_DRESS].DuraMax, 0,"");
                }
            }

            for (var i = 0; i < UseItems.Length; i++)
            {
                if ((UseItems[i] != null) && (UseItems[i].Index > 0) && (M2Share.RandomNumber.Random(8) == 0))
                {
                    nDura = UseItems[i].Dura;
                    nOldDura = HUtil32.Round(nDura / 1000);
                    nDura -= nDam;
                    if (nDura <= 0)
                    {
                        if (Race == Grobal2.RC_PLAYOBJECT)
                        {
                            playObject = this as PlayObject;
                            playObject.SendDelItems(UseItems[i]);
                            stdItem = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                            if (stdItem.NeedIdentify == 1)
                            {
                                M2Share.AddGameDataLog('3' + "\t" + MapName + "\t" + CurrX + "\t" + CurrY +
                                                       "\t" + CharName + "\t" + stdItem.Name + "\t" +
                                                       UseItems[i].MakeIndex + "\t"
                                                       + HUtil32.BoolToIntStr(Race == Grobal2.RC_PLAYOBJECT) +
                                                       "\t" + '0');
                            }
                            UseItems[i].Index = 0;
                            FeatureChanged();
                        }
                        UseItems[i].Index = 0;
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

        public virtual string GetBaseObjectInfo()
        {
            var result = CharName + ' ' + "地图:" + MapName + '(' + Envir.MapDesc + ") " + "座标:" + CurrX +
                         '/' + CurrY + ' ' + "等级:" + Abil.Level + ' ' + "经验:" + Abil.Exp + ' ' + "生命值: " + WAbil.HP + '-' + WAbil.MaxHP + ' ' + "魔法值: " + WAbil.MP + '-' +
                         WAbil.MaxMP + ' ' + "攻击力: " + HUtil32.LoByte(WAbil.DC) + '-' +
                         HUtil32.HiByte(WAbil.DC) + ' ' + "魔法力: " + HUtil32.LoByte(WAbil.MC) + '-' + HUtil32.HiByte(WAbil.MC) + ' ' + "道术: " +
                         HUtil32.LoByte(WAbil.SC) + '-' + HUtil32.HiByte(WAbil.SC) + ' ' + "防御力: " + HUtil32.LoByte(WAbil.AC) + '-' + HUtil32.HiByte(WAbil.AC) + ' ' + "魔防力: " +
                         HUtil32.LoByte(WAbil.MAC) + '-' + HUtil32.HiByte(WAbil.MAC) + ' ' + "准确:" + HitPoint + ' ' + "敏捷:" + SpeedPoint;
            return result;
        }

        public bool GetBackPosition(ref short nX, ref short nY)
        {
            bool result;
            Envirnoment envir;
            envir = this.Envir;
            nX = CurrX;
            nY = CurrY;
            switch (Direction)
            {
                case Grobal2.DR_UP:
                    if (nY < (envir.Height - 1))
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
                    if (nX < (envir.Width - 1))
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
                    if ((nX < (envir.Width - 1)) && (nY < (envir.Height - 1)))
                    {
                        nX++;
                        nY++;
                    }

                    break;
                case Grobal2.DR_UPRIGHT:
                    if ((nX < (envir.Width - 1)) && (nY > 0))
                    {
                        nX -= 1;
                        nY++;
                    }

                    break;
                case Grobal2.DR_DOWNLEFT:
                    if ((nX > 0) && (nY < (envir.Height - 1)))
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

        public bool MakePosion(int nType, ushort nTime, int nPoint)
        {
            var result = false;
            if (nType < Grobal2.MAX_STATUS_ATTRIBUTE)
            {
                var nOldCharStatus = CharStatus;
                if (StatusArr[nType] > 0)
                {
                    if (StatusArr[nType] < nTime)
                    {
                        StatusArr[nType] = nTime;
                    }
                }
                else
                {
                    StatusArr[nType] = nTime;
                }
                StatusArrTick[nType] = HUtil32.GetTickCount();
                CharStatus = GetCharStatus();
                GreenPoisoningPoint = (byte)nPoint;
                if (nOldCharStatus != CharStatus)
                {
                    StatusChanged();
                }

                if (Race == Grobal2.RC_PLAYOBJECT)
                {
                    SysMsg(Format(M2Share.sYouPoisoned, nTime, nPoint), MsgColor.Red, MsgType.Hint);
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
            var result = false;
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

        protected bool GetRecallXy(short nX, short nY, int nRange, ref short nDx, ref short nDy)
        {
            var result = false;
            if (Envir.GetMovingObject(nX, nY, true) == null)
            {
                result = true;
                nDx = nX;
                nDy = nY;
            }

            if (!result)
            {
                for (var i = 0; i < nRange; i++)
                {
                    for (var j = -i; j <= i; j++)
                    {
                        for (var k = -i; k <= i; k++)
                        {
                            nDx = (short)(nX + k);
                            nDy = (short)(nY + j);
                            if (Envir.GetMovingObject(nDx, nDy, true) == null)
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
                nDx = nX;
                nDy = nY;
            }

            return result;
        }

        public bool IsTrainingSkill(int nIndex)
        {
            var result = false;
            UserMagic userMagic;
            for (var i = 0; i < MagicList.Count; i++)
            {
                userMagic = MagicList[i];
                if ((userMagic != null) && (userMagic.MagIdx == nIndex))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        private void DamageBubbleDefence(int nInt)
        {
            if (StatusArr[StatuStateConst.STATE_BUBBLEDEFENCEUP] > 0)
            {
                if (StatusArr[StatuStateConst.STATE_BUBBLEDEFENCEUP] > 3)
                {
                    StatusArr[StatuStateConst.STATE_BUBBLEDEFENCEUP] -= 3;
                }
                else
                {
                    StatusArr[StatuStateConst.STATE_BUBBLEDEFENCEUP] = 1;
                }
            }
        }

        public bool IsGuildMaster()
        {
            return (MyGuild != null) && (GuildRankNo == 1);
        }

        public bool MagCanHitTarget(short nX, short nY, BaseObject targeTBaseObject)
        {
            var result = false;
            int n18;
            if (targeTBaseObject == null)
            {
                return result;
            }

            var n20 = Math.Abs(nX - targeTBaseObject.CurrX) + Math.Abs(nY - targeTBaseObject.CurrY);
            var n14 = 0;
            while (n14 < 13)
            {
                n18 = M2Share.GetNextDirection(nX, nY, targeTBaseObject.CurrX, targeTBaseObject.CurrY);
                if (Envir.GetNextPosition(nX, nY, n18, 1, ref nX, ref nY) && Envir.IsValidCell(nX, nY))
                {
                    if ((nX == targeTBaseObject.CurrX) && (nY == targeTBaseObject.CurrY))
                    {
                        result = true;
                        break;
                    }
                    else
                    {
                        var n1C = Math.Abs(nX - targeTBaseObject.CurrX) + Math.Abs(nY - targeTBaseObject.CurrY);
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

        private bool IsProperFriend_IsFriend(BaseObject cret)
        {
            var result = false;
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
                            for (var i = 0; i < (this as PlayObject).m_MasterList.Count; i++)
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
                            for (var i = 0; i < (cret as PlayObject).m_MasterList.Count; i++)
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

                        if (PvpLevel() >= 2)
                        {
                            if (cret.PvpLevel() < 2)
                            {
                                result = true;
                            }
                        }
                        else
                        {
                            if (cret.PvpLevel() >= 2)
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
            var result = 0;
            var nStartX = nX - nRange;
            var nEndX = nX + nRange;
            var nStartY = nY - nRange;
            var nEndY = nY + nRange;
            for (var cX = nStartX; cX <= nEndX; cX++)
            {
                for (var cY = nStartY; cY <= nEndY; cY++)
                {
                    var cellsuccess = false;
                    var cellInfo = Envir.GetCellInfo(cX, cY, ref cellsuccess);
                    if (cellsuccess && cellInfo.IsAvailable)
                    {
                        for (var k = 0; k < cellInfo.Count; k++)
                        {
                            var osObject = cellInfo.ObjList[k];
                            if ((osObject != null) && (osObject.CellType == CellType.Play || osObject.CellType == CellType.Monster))
                            {
                                var baseObject = M2Share.ActorMgr.Get(osObject.CellObjId);
                                if ((baseObject != null) && (!baseObject.Ghost))
                                {
                                    if (IsProperFriend(baseObject))
                                    {
                                        if (btState == 0)
                                        {
                                            baseObject.DefenceUp(nSec);
                                        }
                                        else
                                        {
                                            baseObject.MagDefenceUp(nSec);
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
            var result = false;
            if (StatusArr[StatuStateConst.STATE_DEFENCEUP] > 0)
            {
                if (StatusArr[StatuStateConst.STATE_DEFENCEUP] < nSec)
                {
                    StatusArr[StatuStateConst.STATE_DEFENCEUP] = nSec;
                    result = true;
                }
            }
            else
            {
                StatusArr[StatuStateConst.STATE_DEFENCEUP] = nSec;
                result = true;
            }

            StatusArrTick[StatuStateConst.STATE_DEFENCEUP] = HUtil32.GetTickCount();
            SysMsg(Format(M2Share.g_sDefenceUpTime, nSec), MsgColor.Green, MsgType.Hint);
            RecalcAbilitys();
            SendMsg(this, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
            return result;
        }

        public bool AttPowerUp(int nPower, int nTime)
        {
            ExtraAbil[0] = (ushort)nPower;
            ExtraAbilTimes[0] = HUtil32.GetTickCount() + nTime * 1000;
            var nMin = nTime / 60;
            var nSec = nTime % 60;
            SysMsg(Format(M2Share.g_sAttPowerUpTime, nMin, nSec), MsgColor.Green, MsgType.Hint);
            RecalcAbilitys();
            SendMsg(this, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
            return true;
        }

        private bool MagDefenceUp(ushort nSec)
        {
            var result = false;
            if (StatusArr[StatuStateConst.STATE_MAGDEFENCEUP] > 0)
            {
                if (StatusArr[StatuStateConst.STATE_MAGDEFENCEUP] < nSec)
                {
                    StatusArr[StatuStateConst.STATE_MAGDEFENCEUP] = nSec;
                    result = true;
                }
            }
            else
            {
                StatusArr[StatuStateConst.STATE_MAGDEFENCEUP] = nSec;
                result = true;
            }
            StatusArrTick[StatuStateConst.STATE_MAGDEFENCEUP] = HUtil32.GetTickCount();
            SysMsg(Format(M2Share.g_sMagDefenceUpTime, nSec), MsgColor.Green, MsgType.Hint);
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
            if (StatusArr[StatuStateConst.STATE_BUBBLEDEFENCEUP] != 0)
            {
                return false;
            }
            var nOldStatus = CharStatus;
            StatusArr[StatuStateConst.STATE_BUBBLEDEFENCEUP] = nSec;
            StatusArrTick[StatuStateConst.STATE_BUBBLEDEFENCEUP] = HUtil32.GetTickCount();
            CharStatus = GetCharStatus();
            if (nOldStatus != CharStatus)
            {
                StatusChanged();
            }
            AbilMagBubbleDefence = true;
            MagBubbleDefenceLevel = nLevel;
            return true;
        }

        public UserItem CheckItemCount(string sItemName, ref int nCount)
        {
            UserItem result = null;
            nCount = 0;
            for (var i = 0; i < UseItems.Length; i++)
            {
                if (UseItems[i] == null)
                {
                    continue;
                }

                var sName = M2Share.WorldEngine.GetStdItemName(UseItems[i].Index);
                if (string.Compare(sName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = UseItems[i];
                    nCount++;
                }
            }

            return result;
        }

        public UserItem CheckItems(string sItemName)
        {
            UserItem result = null;
            UserItem userItem;
            for (var i = 0; i < ItemList.Count; i++)
            {
                userItem = ItemList[i];
                if (userItem == null)
                {
                    continue;
                }

                if (string.Compare(M2Share.WorldEngine.GetStdItemName(userItem.Index), sItemName,
                        StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = userItem;
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
            UserItem userItem;
            var result = false;
            for (var i = 0; i < ItemList.Count; i++)
            {
                userItem = ItemList[i];
                if ((userItem.MakeIndex == nItemIndex) &&
                    string.Compare(M2Share.WorldEngine.GetStdItemName(userItem.Index), sItemName,
                        StringComparison.OrdinalIgnoreCase) == 0)
                {
                    Dispose(userItem);
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
                                 M2Share.Config.DiableHumanRun ||
                                 ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                             (M2Share.Config.boSafeAreaLimited && InSafeZone()))
                            && (Envir.CanWalkEx(nCurrX, nCurrY - 2,
                                    M2Share.Config.DiableHumanRun ||
                                    ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                                (M2Share.Config.boSafeAreaLimited && InSafeZone())))
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
                                 M2Share.Config.DiableHumanRun ||
                                 ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                             (M2Share.Config.boSafeAreaLimited && InSafeZone())) &&
                            (Envir.CanWalkEx(nCurrX + 2, nCurrY - 2,
                                 M2Share.Config.DiableHumanRun ||
                                 ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                             (M2Share.Config.boSafeAreaLimited && InSafeZone())))
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
                                M2Share.Config.DiableHumanRun ||
                                ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                            M2Share.Config.boSafeAreaLimited && InSafeZone() &&
                            (Envir.CanWalkEx(nCurrX + 2, nCurrY,
                                 M2Share.Config.DiableHumanRun ||
                                 ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                             (M2Share.Config.boSafeAreaLimited && InSafeZone())))
                        {
                            result = true;
                            return result;
                        }
                    }

                    break;
                case Grobal2.DR_DOWNRIGHT:
                    if ((nCurrX < Envir.Width - 2) && (nCurrY < Envir.Height - 2) &&
                        (Envir.CanWalkEx(nCurrX + 1, nCurrY + 1,
                             M2Share.Config.DiableHumanRun || ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                         M2Share.Config.boSafeAreaLimited && InSafeZone()) &&
                        (Envir.CanWalkEx(nCurrX + 2, nCurrY + 2,
                             M2Share.Config.DiableHumanRun || ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                         (M2Share.Config.boSafeAreaLimited && InSafeZone())))
                    {
                        result = true;
                        return result;
                    }

                    break;
                case Grobal2.DR_DOWN:
                    if ((nCurrY < Envir.Height - 2) &&
                        (Envir.CanWalkEx(nCurrX, nCurrY + 1,
                             M2Share.Config.DiableHumanRun ||
                             ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                         M2Share.Config.boSafeAreaLimited && InSafeZone() &&
                         (Envir.CanWalkEx(nCurrX, nCurrY + 2,
                              M2Share.Config.DiableHumanRun ||
                              ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                          (M2Share.Config.boSafeAreaLimited && InSafeZone()))))
                    {
                        result = true;
                        return result;
                    }

                    break;
                case Grobal2.DR_DOWNLEFT:
                    if ((nCurrX > 1) && (nCurrY < Envir.Height - 2) &&
                        (Envir.CanWalkEx(nCurrX - 1, nCurrY + 1,
                             M2Share.Config.DiableHumanRun || ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                         (M2Share.Config.boSafeAreaLimited && InSafeZone())) &&
                        (Envir.CanWalkEx(nCurrX - 2, nCurrY + 2,
                             M2Share.Config.DiableHumanRun || ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                         (M2Share.Config.boSafeAreaLimited && InSafeZone())))
                    {
                        result = true;
                        return result;
                    }

                    break;
                case Grobal2.DR_LEFT:
                    if ((nCurrX > 1) &&
                        (Envir.CanWalkEx(nCurrX - 1, nCurrY,
                             M2Share.Config.DiableHumanRun || ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                         (M2Share.Config.boSafeAreaLimited && InSafeZone())) &&
                        (Envir.CanWalkEx(nCurrX - 2, nCurrY,
                             M2Share.Config.DiableHumanRun || ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                         (M2Share.Config.boSafeAreaLimited && InSafeZone())))
                    {
                        result = true;
                        return result;
                    }

                    break;
                case Grobal2.DR_UPLEFT:
                    if ((nCurrX > 1) && (nCurrY > 1) && (Envir.CanWalkEx(nCurrX - 1, nCurrY - 1,
                                                             M2Share.Config.DiableHumanRun ||
                                                             ((Permission > 9) && M2Share.Config.boGMRunAll))
                                                         || (M2Share.Config.boSafeAreaLimited && InSafeZone())) &&
                        (Envir.CanWalkEx(nCurrX - 2, nCurrY - 2,
                             M2Share.Config.DiableHumanRun || ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                         (M2Share.Config.boSafeAreaLimited && InSafeZone())))
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
                                 M2Share.Config.DiableHumanRun ||
                                 ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                             (M2Share.Config.boSafeAreaLimited && InSafeZone()))
                            && (Envir.CanWalkEx(CurrX, CurrY - 2,
                                    M2Share.Config.DiableHumanRun ||
                                    ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                                (M2Share.Config.boSafeAreaLimited && InSafeZone())))
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
                                 M2Share.Config.DiableHumanRun ||
                                 ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                             (M2Share.Config.boSafeAreaLimited && InSafeZone())) &&
                            (Envir.CanWalkEx(CurrX + 2, CurrY - 2,
                                 M2Share.Config.DiableHumanRun ||
                                 ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                             (M2Share.Config.boSafeAreaLimited && InSafeZone())))
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
                                M2Share.Config.DiableHumanRun ||
                                ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                            M2Share.Config.boSafeAreaLimited && InSafeZone() &&
                            (Envir.CanWalkEx(CurrX + 2, CurrY,
                                 M2Share.Config.DiableHumanRun ||
                                 ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                             (M2Share.Config.boSafeAreaLimited && InSafeZone())))
                        {
                            result = true;
                            return result;
                        }
                    }

                    break;
                case Grobal2.DR_DOWNRIGHT:
                    if ((CurrX < Envir.Width - 2) && (CurrY < Envir.Height - 2) &&
                        (Envir.CanWalkEx(CurrX + 1, CurrY + 1,
                             M2Share.Config.DiableHumanRun || ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                         M2Share.Config.boSafeAreaLimited && InSafeZone()) &&
                        (Envir.CanWalkEx(CurrX + 2, CurrY + 2,
                             M2Share.Config.DiableHumanRun || ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                         (M2Share.Config.boSafeAreaLimited && InSafeZone())))
                    {
                        result = true;
                        return result;
                    }

                    break;
                case Grobal2.DR_DOWN:
                    if ((CurrY < Envir.Height - 2) &&
                        (Envir.CanWalkEx(CurrX, CurrY + 1,
                             M2Share.Config.DiableHumanRun ||
                             ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                         M2Share.Config.boSafeAreaLimited && InSafeZone() &&
                         (Envir.CanWalkEx(CurrX, CurrY + 2,
                              M2Share.Config.DiableHumanRun ||
                              ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                          (M2Share.Config.boSafeAreaLimited && InSafeZone()))))
                    {
                        result = true;
                        return result;
                    }

                    break;
                case Grobal2.DR_DOWNLEFT:
                    if ((CurrX > 1) && (CurrY < Envir.Height - 2) &&
                        (Envir.CanWalkEx(CurrX - 1, CurrY + 1,
                             M2Share.Config.DiableHumanRun || ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                         (M2Share.Config.boSafeAreaLimited && InSafeZone())) &&
                        (Envir.CanWalkEx(CurrX - 2, CurrY + 2,
                             M2Share.Config.DiableHumanRun || ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                         (M2Share.Config.boSafeAreaLimited && InSafeZone())))
                    {
                        result = true;
                        return result;
                    }

                    break;
                case Grobal2.DR_LEFT:
                    if ((CurrX > 1) &&
                        (Envir.CanWalkEx(CurrX - 1, CurrY,
                             M2Share.Config.DiableHumanRun || ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                         (M2Share.Config.boSafeAreaLimited && InSafeZone())) &&
                        (Envir.CanWalkEx(CurrX - 2, CurrY,
                             M2Share.Config.DiableHumanRun || ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                         (M2Share.Config.boSafeAreaLimited && InSafeZone())))
                    {
                        result = true;
                        return result;
                    }

                    break;
                case Grobal2.DR_UPLEFT:
                    if ((CurrX > 1) && (CurrY > 1) && (Envir.CanWalkEx(CurrX - 1, CurrY - 1,
                                                                 M2Share.Config.DiableHumanRun ||
                                                                 ((Permission > 9) && M2Share.Config.boGMRunAll))
                                                             || (M2Share.Config.boSafeAreaLimited && InSafeZone())) &&
                        (Envir.CanWalkEx(CurrX - 2, CurrY - 2,
                             M2Share.Config.DiableHumanRun || ((Permission > 9) && M2Share.Config.boGMRunAll)) ||
                         (M2Share.Config.boSafeAreaLimited && InSafeZone())))
                    {
                        result = true;
                        return result;
                    }

                    break;
            }

            return false;
        }

        public BaseObject GetMaster()
        {
            if (Race != Grobal2.RC_PLAYOBJECT)
            {
                var masterObject = Master;
                if (masterObject != null)
                {
                    while (true)
                    {
                        if (masterObject.Master != null)
                        {
                            masterObject = masterObject.Master;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                return masterObject;
            }

            return null;
        }

        public bool ReAliveEx(MonGenInfo monGen)
        {
            WAbil = Abil;
            Gold = 0;
            //m_boStrike = false;
            NoItem = false;
            StoneMode = false;
            Skeleton = false;
            HolySeize = false;
            CrazyMode = false;
            ShowHp = false;
            //m_boPlayerDupMode = false;
            FixedHideMode = false;

            if (this is CastleDoor)
            {
                ((CastleDoor)this).IsOpened = false;
                this.StickMode = true;
            }

            if (this is MagicMonster)
            {
                ((MagicMonster)this).DupMode = false;
            }

            if (this is MagicMonObject)
            {
                ((MagicMonObject)this).UseMagic = false;
            }

            if (this is RockManObject)
            {
                this.HideMode = false;
            }

            if (this is WallStructure)
            {
                ((WallStructure)this).SetMapFlaged = false;
            }

            if (this is SoccerBall)
            {
                ((SoccerBall)this).N550 = 0;
                ((SoccerBall)this).TargetX = -1;
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
                ((WhiteSkeleton)this).MBoIsFirst = true;
                this.FixedHideMode = true;
            }

            if (this is ScultureMonster)
            {
                this.FixedHideMode = true;
            }

            if (this is ScultureKingMonster)
            {
                this.StoneMode = true;
                this.CharStatusEx = StatuStateConst.STATE_STONE_MODE;
            }

            if (this is ElfMonster)
            {
                this.FixedHideMode = true;
                this.NoAttackMode = true;
                ((ElfMonster)this).BoIsFirst = true;
            }

            if (this is ElfWarriorMonster)
            {
                this.FixedHideMode = true;
                ((ElfWarriorMonster)this).BoIsFirst = true;
                ((ElfWarriorMonster)this).UsePoison = false;
            }

            if (this is ElectronicScolpionMon)
            {
                ((ElectronicScolpionMon)this).UseMagic = false;
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
                this.StickMode = true;
            }

            MeatQuality = (ushort)(M2Share.RandomNumber.Random(3500) + 3000);
            //m_nBodyLeathery = m_nPerBodyLeathery;
            ProcessRunCount = 0;
            //m_nPushedCount = 0;
            //m_nBodyState = 0;

            switch (this.Race)
            {
                case 51:
                    MeatQuality = (ushort)(M2Share.RandomNumber.Random(3500) + 3000);
                    BodyLeathery = 50;
                    break;
                case 52:
                    if (M2Share.RandomNumber.Random(30) == 0)
                    {
                        MeatQuality = (ushort)(M2Share.RandomNumber.Random(20000) + 10000);
                        BodyLeathery = 150;
                    }
                    else
                    {
                        MeatQuality = (ushort)(M2Share.RandomNumber.Random(8000) + 8000);
                        BodyLeathery = 150;
                    }

                    break;
                case 53:
                    MeatQuality = (ushort)(M2Share.RandomNumber.Random(8000) + 8000);
                    BodyLeathery = 150;
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
                    StickMode = false;
                    break;
                case 170:
                    StickMode = true;
                    break;
            }

            UseItems = new UserItem[8];
            for (var i = 0; i < ItemList.Count; i++)
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

            var nX = monGen.X - monGen.Range + M2Share.RandomNumber.Random(monGen.Range * 2 + 1);
            var nY = monGen.Y - monGen.Range + M2Share.RandomNumber.Random(monGen.Range * 2 + 1);
            var mBoErrorOnInit = true;
            if (Envir.CanWalk(nX, nY, true))
            {
                CurrX = (short)nX;
                CurrY = (short)nY;
                if (AddToMap())
                {
                    mBoErrorOnInit = false;
                }
            }

            var nRange = 0;
            var nRange2 = 0;
            if (mBoErrorOnInit)
            {
                if (Envir.Width < 50)
                {
                    nRange = 2;
                }
                else
                {
                    nRange = 3;
                }

                if (Envir.Height < 250)
                {
                    if (Envir.Height < 30)
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

                    addObj = Envir.AddToMap(nX, nY, MapCell, this);
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
                Envir.AddToMap(CurrX, CurrY, MapCell, this);
            }
            Abil.HP = Abil.MaxHP;
            Abil.MP = Abil.MaxMP;
            WAbil.HP = WAbil.MaxHP;
            WAbil.MP = WAbil.MaxMP;
            RecalcAbilitys();
            Death = false;
            Invisible = false;

            SendRefMsg(Grobal2.RM_TURN, Direction, CurrX, CurrY, GetFeatureToLong(), "");

            if (M2Share.Config.MonSayMsg)
            {
                MonsterSayMsg(null, MonStatus.MonGen);
            }

            return true;
        }

        public void OnEnvirnomentChanged()
        {
            if (CanReAlive)
            {
                if ((MonGen != null) && (MonGen.Envir != Envir))
                {
                    CanReAlive = false;
                    if (MonGen.ActiveCount > 0)
                    {
                        MonGen.ActiveCount--;
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
            //                ((TPlayObject)(this)).protectedPowerPointChanged();
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

        protected void Dispose(object obj)
        {
            obj = null;
        }

        protected string Format(string str, params object[] par)
        {
            return string.Format(str, par);
        }
    }
}