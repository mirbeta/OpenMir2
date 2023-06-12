using System.Collections;
using M2Server;
using M2Server.Actor;
using M2Server.Magic;
using M2Server.Monster.Monsters;
using M2Server.Player;
using SystemModule;
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Events;
using SystemModule.Packets.ClientPackets;

namespace RobotSystem
{
    /// <summary>
    /// 假人
    /// </summary>
    public partial class RobotPlayer : PlayObject, IRobotPlayer
    {
        public long WalkIntervalTick = 0;
        public long SearchTargetTick = 0;
        /// <summary>
        /// 假人启动
        /// </summary>
        public bool RobotStart;
        /// <summary>
        /// 挂机地图
        /// </summary>
        public IEnvirnoment ManagedEnvir;
        public PointManager PointManager;
        public PointInfo[] MPath;
        public int MNPostion;
        public int MoveFailCount;
        public string ConfigListFileName = string.Empty;
        public string FilePath = string.Empty;
        public string ConfigFileName = string.Empty;
        public string HeroConfigListFileName = string.Empty;
        public IList<string> BagItemNames;
        public string[] UseItemNames;
        public TRunPos MRunPos;
        /// <summary>
        /// 技能使用间隔
        /// </summary>
        public long[] SkillUseTick;
        /// <summary>
        /// 攻击方式
        /// </summary>
        public short HitMode;
        public bool BoSelSelf;
        public long AutoRepairItemTick;
        public long AutoAddHealthTick;
        /// <summary>
        /// 思考时间
        /// </summary>
        public long ThinkTick;
        public bool MBoDupMode;
        public long SearchUseMagic = 0;
        /// <summary>
        /// 低血回城间隔
        /// </summary>
        public long MDwHpToMapHomeTick = 0;
        /// <summary>
        /// 守护模式
        /// </summary>
        public bool ProtectStatus;
        public short ProtectTargetX;
        public short ProtectTargetY;
        /// <summary>
        /// 到达守护坐标
        /// </summary>
        public bool ProtectDest;
        /// <summary>
        /// 是向守护坐标的累计数
        /// </summary>
        public int GotoProtectXyCount;
        /// <summary>
        /// 拾取物品间隔
        /// </summary>
        public long PickUpItemTick;
        public MapItem MSelMapItem;
        /// <summary>
        /// 跑步计时
        /// </summary>
        public long RunIntervalTick;
        /// <summary>
        /// 受攻击说话列表
        /// </summary>
        public ArrayList AiSayMsgList;
        /// <summary>
        /// 绿红毒标识
        /// </summary>
        public byte NAmuletIndx;
        /// <summary>
        /// 正在拾取物品
        /// </summary>
        public bool CanPickIng;
        /// <summary>
        /// 查询魔法
        /// </summary>
        public short AutoMagicId;
        /// <summary>
        /// 是否可以使用的魔法(True才可能躲避)
        /// </summary>
        public bool AutoUseMagic;
        /// <summary>
        /// 是否可以使用的攻击魔法
        /// </summary>
        public bool BoUseAttackMagic;
        /// <summary>
        /// 最后的方向
        /// </summary>
        public byte LastDirection;
        /// <summary>
        /// 自动躲避间隔
        /// </summary>
        public long AutoAvoidTick;
        public bool IsNeedAvoid;
        /// <summary>
        /// 假人掉装备机率
        /// </summary>
        public int DropUseItemRate;
        /// <summary>
        /// 死亡复活
        /// </summary>
        public bool Resurrection;

        public RobotPlayer()
        {
            SoftVersionDate = SystemShare.Config.SoftVersionDate;
            SoftVersionDateEx = Grobal2.ClientVersionNumber;
            AbilCopyToWAbil();
            IsRobot = true;
            LoginNoticeOk = true;
            RobotStart = false; // 开始挂机
            ManagedEnvir = null; // 挂机地图
            MPath = null;
            MNPostion = -1;
            UseItemNames = new string[13];
            BagItemNames = new List<string>();
            PointManager = new PointManager(this);
            SkillUseTick = new long[60];// 魔法使用间隔
            BoSelSelf = false;
            AutoAddHealthTick = HUtil32.GetTickCount();
            AutoRepairItemTick = HUtil32.GetTickCount();
            ThinkTick = HUtil32.GetTickCount();
            MBoDupMode = false;
            ProtectStatus = false;// 守护模式
            ProtectDest = true;// 到达守护坐标
            GotoProtectXyCount = 0;// 是向守护坐标的累计数
            PickUpItemTick = HUtil32.GetTickCount();
            AiSayMsgList = new ArrayList();// 受攻击说话列表
            NAmuletIndx = 0;
            CanPickIng = false;
            AutoMagicId = 0;
            AutoUseMagic = false;// 是否能躲避
            BoUseAttackMagic = false;
            LastDirection = Dir;
            AutoAvoidTick = HUtil32.GetTickCount();// 自动躲避间隔
            IsNeedAvoid = false;// 是否需要躲避
            WalkTick = HUtil32.GetTickCount();
            WalkSpeed = 300;
            MRunPos = new TRunPos();
            MPath = Array.Empty<PointInfo>();
            LoadConfig();
        }

        public void Start(FindPathType pathType)
        {
            if (!Ghost && !Death && !RobotStart)
            {
                ManagedEnvir = Envir;
                ProtectDest = false;
                ProtectTargetX = CurrX;// 守护坐标
                ProtectTargetY = CurrY;// 守护坐标
                GotoProtectXyCount = 0;// 是向守护坐标的累计数
                PointManager.PathType = pathType;
                PointManager.Initialize(Envir);
                RobotStart = true;
                MoveFailCount = 0;
                if (SystemShare.FunctionNPC != null)
                {
                    ScriptGotoCount = 0;
                    // ModuleShare.FunctionNPC.GotoLable(this, "@AIStart", false);
                }
            }
        }

        public void Stop()
        {
            if (RobotStart)
            {
                RobotStart = false;
                MoveFailCount = 0;
                MPath = null;
                MNPostion = -1;
                if (SystemShare.FunctionNPC != null)
                {
                    ScriptGotoCount = 0;
                    // ModuleShare.FunctionNPC.GotoLable(this, "@AIStop", false);
                }
            }
        }

        public ushort GetFeatureEx()
        {
            throw new NotImplementedException();
        }

        void IActor.WinExp(int exp)
        {
            WinExp(exp);
        }

        public void ApplyMeatQuality()
        {
            throw new NotImplementedException();
        }

        public bool MakePosion(int nType, ushort nTime, int nPoint)
        {
            throw new NotImplementedException();
        }

        public void SetLastHiter(IActor actor)
        {
            throw new NotImplementedException();
        }

        public void SetTargetCreat(IActor actor)
        {
            throw new NotImplementedException();
        }

        public IActor GetPoseCreate()
        {
            throw new NotImplementedException();
        }

        public void OnEnvirnomentChanged()
        {
            throw new NotImplementedException();
        }

        public void AddMessage(SendMessage sendMessage)
        {
            throw new NotImplementedException();
        }

        private void WinExp(int dwExp)
        {
            if (Abil.Level > SystemShare.Config.LimitExpLevel)
            {
                dwExp = SystemShare.Config.LimitExpValue;
                GetExp(dwExp);
            }
            else if (dwExp > 0)
            {
                dwExp = SystemShare.Config.KillMonExpMultiple * dwExp; // 系统指定杀怪经验倍数
                dwExp = MNKillMonExpMultiple * dwExp; // 人物指定的杀怪经验倍数
                dwExp = HUtil32.Round(KillMonExpRate / 100.0 * dwExp); // 人物指定的杀怪经验倍数
                if (Envir.Flag.boEXPRATE)
                {
                    dwExp = HUtil32.Round(Envir.Flag.ExpRate / 100.0 * dwExp); // 地图上指定杀怪经验倍数
                }
                GetExp(dwExp);
            }
        }

        private void GetExp(int dwExp)
        {
            Abil.Exp += dwExp;
            AddBodyLuck(dwExp * 0.002);
            SendMsg(Messages.RM_WINEXP, 0, dwExp, 0, 0);
            if (Abil.Exp >= Abil.MaxExp)
            {
                Abil.Exp -= Abil.MaxExp;
                if (Abil.Level < Settings.MAXUPLEVEL)
                {
                    Abil.Level++;
                }
                HasLevelUp(Abil.Level - 1);
                AddBodyLuck(100);
                //SystemShare.EventSource.AddEventLog(12, MapName + "\t" + Abil.Level + "\t" + Abil.Exp + "\t" + ChrName + "\t" + '0' + "\t" + '0' + "\t" + '1' + "\t" + '0');
                IncHealthSpell(2000, 2000);
            }
        }

        public int ActorId { get; }
        public string ChrName { get; set; }
        public byte NameColor { get; set; }
        public short CurrX { get; set; }
        public short CurrY { get; set; }
        public byte Dir { get; set; }
        public string MapName { get; set; }
        public string MapFileName { get; set; }
        public int Gold { get; set; }
        public int CharStatus { get; set; }
        public int CharStatusEx { get; set; }
        public MonGenInfo MonGen { get; set; }
        public Ability Abil { get; set; }
        public Ability WAbil { get; set; }
        public AddAbility AddAbil { get; set; }
        public byte ViewRange { get; set; }
        public ushort[] StatusTimeArr { get; set; }
        public int[] StatusArrTick { get; set; }
        public bool UnParalysis { get; set; }
        public ushort Appr { get; set; }
        public byte Race { get; set; }
        public CellType CellType { get; set; }
        public byte RaceImg { get; set; }
        public byte HitPoint { get; set; }
        public byte AntiPoison { get; set; }
        public ushort AntiMagic { get; set; }
        public byte GreenPoisoningPoint { get; set; }
        public byte SpeedPoint { get; set; }
        public byte CoolEyeCode { get; set; }
        public bool CoolEye { get; set; }
        public IActor Master { get; set; }
        public IActor GetMaster()
        {
            throw new NotImplementedException();
        }

        public byte LifeAttrib { get; set; }
        public IList<IActor> SlaveList { get; set; }
        public bool SlaveRelax { get; set; }
        public byte Light { get; set; }
        public IUserCastle Castle { get; set; }
        public bool SuperMan { get; set; }
        public bool Animal { get; set; }
        public bool NoItem { get; set; }
        public bool FixedHideMode { get; set; }
        public bool StickMode { get; set; }
        public bool RushMode { get; set; }
        public bool NoTame { get; set; }
        public bool Skeleton { get; set; }
        public byte BodyLeathery { get; set; }
        public bool ShowHp { get; set; }
        public int ShowHpTick { get; set; }
        public int ShowHpInterval { get; set; }
        public IEnvirnoment Envir { get; set; }
        public bool Ghost { get; set; }
        public int GhostTick { get; set; }
        public bool Death { get; set; }
        public int DeathTick { get; set; }
        public bool Invisible { get; set; }
        public bool CanReAlive { get; set; }
        public int ReAliveTick { get; set; }
        public byte MonsterWeapon { get; set; }
        public int StruckTick { get; set; }
        public bool WantRefMsg { get; set; }
        public bool AddtoMapSuccess { get; set; }
        public bool SpaceMoved { get; set; }
        public bool Mission { get; set; }
        public short MissionX { get; set; }
        public short MissionY { get; set; }
        public bool HideMode { get; set; }
        public bool StoneMode { get; set; }
        public bool Transparent { get; set; }
        public bool AdminMode { get; set; }
        public bool ObMode { get; set; }
        public int SearchTime { get; set; }
        public int SearchTick { get; set; }
        public int RunTick { get; set; }
        public int RunTime { get; set; }
        public int HealthTick { get; set; }
        public int SpellTick { get; set; }
        public IActor TargetCret { get; set; }
        public int TargetFocusTick { get; set; }
        public IActor LastHiter { get; set; }
        public int LastHiterTick { get; set; }
        public IActor ExpHitter { get; set; }
        public int ExpHitterTick { get; set; }
        public int PoisoningTick { get; set; }
        public int VerifyTick { get; set; }
        public int AutoRecoveryTick { get; set; }
        public IList<int> VisibleHumanList { get; set; }
        public bool IsVisibleActive { get; set; }
        public IList<VisibleBaseObject> VisibleActors { get; set; }
        public IList<UserItem> ItemList { get; set; }
        public int SendRefMsgTick { get; set; }
        public int AttackTick { get; set; }
        public int WalkTick { get; set; }
        public int WalkSpeed { get; set; }
        public int NextHitTime { get; set; }
        public bool DenyRefStatus { get; set; }
        public bool AddToMaped { get; set; }
        public bool DelFormMaped { get; set; }
        public bool AutoChangeColor { get; set; }
        public int AutoChangeColorTick { get; set; }
        public byte AutoChangeIdx { get; set; }
        public bool FixColor { get; set; }
        public byte FixColorIdx { get; set; }
        public int FixStatus { get; set; }
        public bool FastParalysis { get; set; }
        public bool NastyMode { get; set; }
        public bool IsRobot { get; set; }

        public override void MakeGhost()
        {
            if (RobotStart)
            {
                RobotStart = false;
            }
            base.MakeGhost();
        }

        public void SendSelfDelayMsg(int wIdent, int wParam, int lParam1, int lParam2, int lParam3, string sMsg, int dwDelay)
        {
            throw new NotImplementedException();
        }

        public void SendRefMsg(int wIdent, int wParam, int nParam1, int nParam2, int nParam3, string sMsg)
        {
            throw new NotImplementedException();
        }

        public void SendMsg(int wIdent, int wParam, int nParam1, int nParam2, int nParam3, string sMsg = "")
        {
            throw new NotImplementedException();
        }

        public void SpaceMove(string sMap, short nX, short nY, int nInt)
        {
            throw new NotImplementedException();
        }

        public void SendStruckDelayMsg(int wIdent, int wParam, int lParam1, int lParam2, int lParam3, string sMsg, int dwDelay)
        {
            throw new NotImplementedException();
        }

        public void SendStruckDelayMsg(int actorId, int wIdent, int wParam, int lParam1, int lParam2, int lParam3, string sMsg,
            int dwDelay)
        {
            throw new NotImplementedException();
        }

        public void SendMsg(IActor baseObject, int wIdent, int wParam, int nParam1, int nParam2, int nParam3, string sMsg = "")
        {
            throw new NotImplementedException();
        }

        public void SendActionMsg(int wIdent, int wParam, int lParam1, int lParam2, int lParam3, string sMsg)
        {
            throw new NotImplementedException();
        }

        public void SendUpdateMsg(int wIdent, int wParam, int lParam1, int lParam2, int lParam3, string sMsg)
        {
            throw new NotImplementedException();
        }

        public bool InSafeZone()
        {
            throw new NotImplementedException();
        }

        public void ScatterBagItems(int itemOfCreat)
        {
            throw new NotImplementedException();
        }

        public int GetFeature(IActor baseObject)
        {
            throw new NotImplementedException();
        }

        public int GetFeatureToLong()
        {
            throw new NotImplementedException();
        }

        public void ScatterGolds(int goldOfCreat)
        {
            throw new NotImplementedException();
        }

        public void StruckDamage(int nDamage)
        {
            throw new NotImplementedException();
        }

        public string GetShowName()
        {
            throw new NotImplementedException();
        }

        public void RefNameColor()
        {
            throw new NotImplementedException();
        }

        public void RefShowName()
        {
            throw new NotImplementedException();
        }

        public byte SlaveExpLevel { get; set; }
        public byte GetNameColor()
        {
            throw new NotImplementedException();
        }

        public void RecalcAbilitys()
        {
            throw new NotImplementedException();
        }

        public bool GetBackPosition(ref short nX, ref short nY)
        {
            throw new NotImplementedException();
        }

        public int CharPushed(byte nDir, byte nPushCount)
        {
            throw new NotImplementedException();
        }

        public ushort GetHitStruckDamage(IActor target, int nDamage)
        {
            throw new NotImplementedException();
        }

        public int GetMagStruckDamage(IActor baseObject, int nDamage)
        {
            throw new NotImplementedException();
        }

        public bool DefenceUp(int nSec)
        {
            throw new NotImplementedException();
        }

        public bool MagDefenceUp(int nSec)
        {
            throw new NotImplementedException();
        }

        public bool OnHorse { get; set; }
        public byte HorseType { get; set; }
        public AttackMode AttatckMode { get; set; }
        public UserItem[] UseItems { get; set; }
        public ushort IncHealth { get; set; }
        public ushort IncSpell { get; set; }
        public ushort IncHealing { get; set; }
        public PlayGender Gender { get; set; }
        public byte Hair { get; set; }
        public PlayJob Job { get; set; }
        public string UserAccount { get; set; }
        public string LoginIpAddr { get; set; }
        public string LoginIpLocal { get; set; }
        public bool AccountExpired { get; set; }
        public int AccountExpiredTick { get; set; }
        public long ExpireTime { get; set; }
        public int ExpireCount { get; set; }
        public int QueryExpireTick { get; set; }
        public byte Permission { get; set; }
        public byte Luck { get; set; }
        public int GoldMax { get; set; }
        public bool GuildWarArea { get; set; }
        public bool AllowGuildReCall { get; set; }
        public bool HearWhisper { get; set; }
        public bool BanShout { get; set; }
        public bool BanGuildChat { get; set; }
        public bool AllowDeal { get; set; }
        public bool BoDuplication { get; set; }
        public int DupStartTick { get; set; }
        public bool UserUnLockDurg { get; set; }
        public bool AllowGroup { get; set; }
        public bool AllowGuild { get; set; }
        public IPlayerActor DealCreat { get; set; }
        public bool Dealing { get; set; }
        public int DealLastTick { get; set; }
        public int DealGolds { get; set; }
        public bool DealSuccess { get; set; }
        public string HomeMap { get; set; }
        public short HomeX { get; set; }
        public short HomeY { get; set; }
        public int GroupRcallTick { get; set; }
        public short GroupRcallTime { get; set; }
        public bool GuildMove { get; set; }
        public CommandMessage ClientMsg { get; set; }
        public ushort FightZoneDieCount { get; set; }
        public bool IsSpirit { get; set; }
        public int DoMotaeboTick { get; set; }
        public bool CrsHitkill { get; set; }
        public bool MBo43Kill { get; set; }
        public bool RedUseHalfMoon { get; set; }
        public bool UseThrusting { get; set; }
        public bool UseHalfMoon { get; set; }
        public UserMagic[] MagicArr { get; set; }
        public bool PowerHit { get; set; }
        public bool FireHitSkill { get; set; }
        public bool TwinHitSkill { get; set; }
        public ushort HitPlus { get; set; }
        public ushort HitDouble { get; set; }
        public int LatestFireHitTick { get; set; }
        public int LatestTwinHitTick { get; set; }
        public byte PowerItem { get; set; }
        public IList<UserItem> DealItemList { get; set; }
        public IList<UserItem> StorageItemList { get; set; }
        public IList<MapEvent> VisibleEvents { get; set; }
        public IList<VisibleMapItem> VisibleItems { get; set; }
        public IList<string> LockWhisperList { get; set; }
        public bool BoPowerItem { get; set; }
        public bool AllowGroupReCall { get; set; }
        public int HungerStatus { get; set; }
        public int BonusPoint { get; set; }
        public byte BtB2 { get; set; }
        public bool PvpFlag { get; set; }
        public int DecPkPointTick { get; set; }
        public int PkPoint { get; set; }
        public int PvpNameColorTick { get; set; }
        public bool NameColorChanged { get; set; }
        public bool InGuildWarArea { get; set; }
        public IGuild MyGuild { get; set; }
        public short GuildRankNo { get; set; }
        public string GuildRankName { get; set; }
        public string ScriptLable { get; set; }
        public int SayMsgCount { get; set; }
        public int SayMsgTick { get; set; }
        public bool DisableSayMsg { get; set; }
        public int DisableSayMsgTick { get; set; }
        public int CheckDupObjTick { get; set; }
        public int DiscountForNightTick { get; set; }
        public bool IsSafeArea { get; set; }
        public int ShoutMsgTick { get; set; }
        public byte AttackSkillCount { get; set; }
        public byte AttackSkillPointCount { get; set; }
        public bool SmashSet { get; set; }
        public bool HwanDevilSet { get; set; }
        public bool PuritySet { get; set; }
        public bool MundaneSet { get; set; }
        public bool NokChiSet { get; set; }
        public bool TaoBuSet { get; set; }
        public bool FiveStringSet { get; set; }
        public byte ValNpcType { get; set; }
        public byte ValType { get; set; }
        public byte ValLabel { get; set; }
        public int RevivalTick { get; set; }
        public bool NoDropItem { get; set; }
        public int ProbeTick { get; set; }
        public int TeleportTick { get; set; }
        public int DecHungerPointTick { get; set; }
        public int AutoAddHpmpMode { get; set; }
        public int CheckHpmpTick { get; set; }
        public int KickOffLineTick { get; set; }
        public bool OffLineFlag { get; set; }
        public string OffLineLeaveWord { get; set; }
        public int SocketId { get; set; }
        public ushort SocketIdx { get; set; }
        public int GateIdx { get; set; }
        public int SoftVersionDate { get; set; }
        public long LogonTime { get; set; }
        public int LogonTick { get; set; }
        public bool BoReadyRun { get; set; }
        public int MapMoveTick { get; set; }
        public byte PayMent { get; set; }
        public byte PayMode { get; set; }
        public int SessionId { get; set; }
        public PlayerSession SessInfo { get; set; }
        public int LoadTick { get; set; }
        public byte ServerIndex { get; set; }
        public bool BoEmergencyClose { get; set; }
        public bool BoSoftClose { get; set; }
        public bool BoKickFlag { get; set; }
        public bool BoReconnection { get; set; }
        public bool RcdSaved { get; set; }
        public bool SwitchData { get; set; }
        public bool SwitchDataOk { get; set; }
        public string SwitchDataTempFile { get; set; }
        public int WriteChgDataErrCount { get; set; }
        public string SwitchMapName { get; set; }
        public short SwitchMapX { get; set; }
        public short SwitchMapY { get; set; }
        public bool SwitchDataSended { get; set; }
        public int ChgDataWritedTick { get; set; }
        public bool AbilSeeHealGauge { get; set; }
        public int HitIntervalTime { get; set; }
        public int MagicHitIntervalTime { get; set; }
        public int RunIntervalTime { get; set; }
        public int WalkIntervalTime { get; set; }
        public int TurnIntervalTime { get; set; }
        public int ActionIntervalTime { get; set; }
        public int RunLongHitIntervalTime { get; set; }
        public int RunHitIntervalTime { get; set; }
        public int WalkHitIntervalTime { get; set; }
        public int RunMagicIntervalTime { get; set; }
        public int MagicAttackTick { get; set; }
        public int MagicAttackInterval { get; set; }
        public int MoveTick { get; set; }
        public int AttackCount { get; set; }
        public int AttackCountA { get; set; }
        public int MagicAttackCount { get; set; }
        public int MoveCount { get; set; }
        public int OverSpeedCount { get; set; }
        public bool Revival { get; set; }
        public bool Teleport { get; set; }
        public bool Paralysis { get; set; }
        public bool FlameRing { get; set; }
        public bool RecoveryRing { get; set; }
        public bool AngryRing { get; set; }
        public bool MagicShield { get; set; }
        public bool UnMagicShield { get; set; }
        public bool MuscleRing { get; set; }
        public bool ProbeNecklace { get; set; }
        public bool UnRevival { get; set; }
        public bool RecallSuite { get; set; }
        public int MoXieSuite { get; set; }
        public int SuckupEnemyHealthRate { get; set; }
        public double SuckupEnemyHealth { get; set; }
        public double BodyLuck { get; set; }
        public int BodyLuckLevel { get; set; }
        public bool DieInFight3Zone { get; set; }
        public string GotoNpcLabel { get; set; }
        public bool TakeDlgItem { get; set; }
        public int DlgItemIndex { get; set; }
        public int DelayCall { get; set; }
        public int DelayCallTick { get; set; }
        public bool IsDelayCall { get; set; }
        public int DelayCallNpc { get; set; }
        public string DelayCallLabel { get; set; }
        public int LastNpc { get; set; }
        public NakedAbility BonusAbil { get; set; }
        public int[] MNVal { get; set; }
        public int[] MNMval { get; set; }
        public int[] MDyVal { get; set; }
        public string[] MNSval { get; set; }
        public int[] MNInteger { get; set; }
        public string[] MSString { get; set; }
        public string[] MServerStrVal { get; set; }
        public int[] MServerIntVal { get; set; }
        public Dictionary<string, int> m_IntegerList { get; set; }
        public Dictionary<string, string> m_StringList { get; set; }
        public string ScatterItemName { get; set; }
        public string ScatterItemOwnerName { get; set; }
        public int ScatterItemX { get; set; }
        public int ScatterItemY { get; set; }
        public string ScatterItemMapName { get; set; }
        public string ScatterItemMapDesc { get; set; }
        public IList<UserMagic> MagicList { get; set; }
        public int GroupOwner { get; set; }
        public IList<IPlayerActor> GroupMembers { get; set; }
        public string PlayDiceLabel { get; set; }
        public bool IsTimeRecall { get; set; }
        public int TimeRecallTick { get; set; }
        public string TimeRecallMoveMap { get; set; }
        public short TimeRecallMoveX { get; set; }
        public short TimeRecallMoveY { get; set; }
        public int DecLightItemDrugTick { get; set; }
        public int SaveRcdTick { get; set; }
        public byte Bright { get; set; }
        public bool IsNewHuman { get; set; }
        public bool IsSendNotice { get; set; }
        public int WaitLoginNoticeOkTick { get; set; }
        public bool LoginNoticeOk { get; set; }
        public bool TryPlayMode { get; set; }
        public int ShowLineNoticeTick { get; set; }
        public int ShowLineNoticeIdx { get; set; }
        public int SoftVersionDateEx { get; set; }
        public Hashtable CanJmpScriptLableMap { get; set; }
        public int ScriptGotoCount { get; set; }
        public string ScriptCurrLable { get; set; }
        public string ScriptGoBackLable { get; set; }
        public int TurnTick { get; set; }
        public int OldIdent { get; set; }
        public byte MBtOldDir { get; set; }
        public bool IsFirstAction { get; set; }
        public int ActionTick { get; set; }
        public string DearName { get; set; }
        public IPlayerActor DearHuman { get; set; }
        public bool CanDearRecall { get; set; }
        public bool CanMasterRecall { get; set; }
        public int DearRecallTick { get; set; }
        public int MasterRecallTick { get; set; }
        public string MasterName { get; set; }
        public IPlayerActor MasterHuman { get; set; }
        public IList<IActor> MasterList { get; set; }
        public bool IsMaster { get; set; }
        public int PoseBaseObject { get; set; }
        public byte CreditPoint { get; set; }
        public byte MarryCount { get; set; }
        public byte ReLevel { get; set; }
        public byte ReColorIdx { get; set; }
        public int ReColorTick { get; set; }
        public int MNKillMonExpMultiple { get; set; }
        public int GetMessageTick { get; set; }
        public bool IsSetStoragePwd { get; set; }
        public bool IsReConfigPwd { get; set; }
        public bool IsCheckOldPwd { get; set; }
        public bool IsUnLockPwd { get; set; }
        public bool IsUnLockStoragePwd { get; set; }
        public bool IsPasswordLocked { get; set; }
        public byte PwdFailCount { get; set; }
        public bool IsLockLogon { get; set; }
        public bool IsLockLogoned { get; set; }
        public string MSTempPwd { get; set; }
        public string StoragePwd { get; set; }
        public bool IsStartMarry { get; set; }
        public bool IsStartMaster { get; set; }
        public bool IsStartUnMarry { get; set; }
        public bool IsStartUnMaster { get; set; }
        public bool FilterSendMsg { get; set; }
        public int KillMonExpRate { get; set; }
        public int PowerRate { get; set; }
        public int KillMonExpRateTime { get; set; }
        public int PowerRateTime { get; set; }
        public int ExpRateTick { get; set; }
        public bool FastTrain { get; set; }
        public bool BoCanUseItem { get; set; }
        public bool IsCanDeal { get; set; }
        public bool IsCanDrop { get; set; }
        public bool IsCanGetBackItem { get; set; }
        public bool IsCanWalk { get; set; }
        public bool IsCanRun { get; set; }
        public bool IsCanHit { get; set; }
        public bool IsCanSpell { get; set; }
        public bool IsCanSendMsg { get; set; }
        public int MemberType { get; set; }
        public byte MemberLevel { get; set; }
        public bool BoSendMsgFlag { get; set; }
        public bool BoChangeItemNameFlag { get; set; }
        public int GameGold { get; set; }
        public bool BoDecGameGold { get; set; }
        public int DecGameGoldTime { get; set; }
        public int DecGameGoldTick { get; set; }
        public int DecGameGold { get; set; }
        public bool BoIncGameGold { get; set; }
        public int IncGameGoldTime { get; set; }
        public int IncGameGoldTick { get; set; }
        public int IncGameGold { get; set; }
        public int GamePoint { get; set; }
        public int IncGamePointTick { get; set; }
        public int PayMentPoint { get; set; }
        public int PayMentPointTick { get; set; }
        public int DecHpTick { get; set; }
        public int IncHpTick { get; set; }
        public int PkDieLostExp { get; set; }
        public byte PkDieLostLevel { get; set; }
        public IActor WhisperHuman { get; set; }
        public int ClearInvalidObjTick { get; set; }
        public short Contribution { get; set; }
        public string RankLevelName { get; set; }
        public bool IsFilterAction { get; set; }
        public int AutoGetExpTick { get; set; }
        public int AutoGetExpTime { get; set; }
        public int AutoGetExpPoint { get; set; }
        public IEnvirnoment AutoGetExpEnvir { get; set; }
        public bool AutoGetExpInSafeZone { get; set; }
        public Dictionary<string, DynamicVar> DynamicVarMap { get; set; }
        public short ClientTick { get; set; }
        public bool TestSpeedMode { get; set; }
        public string RandomNo { get; set; }
        public int QueryBagItemsTick { get; set; }
        public bool IsTimeGoto { get; set; }
        public int TimeGotoTick { get; set; }
        public string TimeGotoLable { get; set; }
        public IActor TimeGotoNpc { get; set; }
        public int[] AutoTimerTick { get; set; }
        public int[] AutoTimerStatus { get; set; }
        public ushort[] ExtraAbil { get; set; }
        public byte[] ExtraAbilFlag { get; set; }
        public int[] ExtraAbilTimes { get; set; }
        public int ClickNpcTime { get; set; }
        public bool SaleDeal { get; set; }
        public bool SellOffConfirm { get; set; }
        public IList<UserItem> SellOffItemList { get; set; }
        public byte[] QuestUnitOpen { get; set; }
        public byte[] QuestUnit { get; set; }
        public byte[] QuestFlag { get; set; }
        public MarketUser MarketUser { get; set; }
        public bool FlagReadyToSellCheck { get; set; }
        public int FightExp { get; set; }
        public void FeatureChanged()
        {
            throw new NotImplementedException();
        }

        public void RunNotice()
        {
            throw new NotImplementedException();
        }

        public void UserLogon()
        {
            throw new NotImplementedException();
        }

        public void GameTimeChanged()
        {
            throw new NotImplementedException();
        }

        public void Disappear()
        {
            throw new NotImplementedException();
        }

        public void DealCancelA()
        {
            throw new NotImplementedException();
        }

        public int GetMyStatus()
        {
            throw new NotImplementedException();
        }

        public void HasLevelUp(int level)
        {
            throw new NotImplementedException();
        }

        public bool IsAddWeightAvailable(int nWeight)
        {
            throw new NotImplementedException();
        }

        public void MapRandomMove(string sMapName, int nInt)
        {
            throw new NotImplementedException();
        }

        public void SetQuestFlagStatus(int nFlag, int nValue)
        {
            throw new NotImplementedException();
        }

        public int GetQuestUnitOpenStatus(int nFlag)
        {
            throw new NotImplementedException();
        }

        public void SetQuestUnitOpenStatus(int nFlag, int nValue)
        {
            throw new NotImplementedException();
        }

        public int GetQuestUnitStatus(int nFlag)
        {
            throw new NotImplementedException();
        }

        public void SetQuestUnitStatus(int nFlag, int nValue)
        {
            throw new NotImplementedException();
        }

        public IActor MakeSlave(string sMonName, int nMakeLevel, int nExpLevel, int nMaxMob, int dwRoyaltySec)
        {
            throw new NotImplementedException();
        }

        public void SysMsg(string sMsg, MsgColor msgColor, MsgType msgType)
        {
            throw new NotImplementedException();
        }

        public string GetMyInfo()
        {
            throw new NotImplementedException();
        }

        public byte PvpLevel()
        {
            throw new NotImplementedException();
        }

        public UserMagic GetMagicInfo(string magicName)
        {
            throw new NotImplementedException();
        }

        public UserItem QuestCheckItem(string sItemName, ref int nCount, ref int nParam, ref int nDura)
        {
            throw new NotImplementedException();
        }

        public bool CheckItemBindUse(UserItem userItem)
        {
            throw new NotImplementedException();
        }

        public bool IsGuildMaster()
        {
            throw new NotImplementedException();
        }

        public int GetQuestFalgStatus(int nFlag)
        {
            throw new NotImplementedException();
        }

        public void SendDelItems(UserItem userItem)
        {
            throw new NotImplementedException();
        }

        public void SendAddItem(UserItem userItem)
        {
            throw new NotImplementedException();
        }

        public void DelBagItem(UserItem userItem)
        {
            throw new NotImplementedException();
        }

        public bool DelBagItem(int makeIndex, string itemName)
        {
            throw new NotImplementedException();
        }

        public void SendUpdateItem(UserItem userItem)
        {
            throw new NotImplementedException();
        }

        public void SendDelMagic(UserMagic userMagic)
        {
            throw new NotImplementedException();
        }

        public void SendAddMagic(UserMagic userMagic)
        {
            throw new NotImplementedException();
        }

        public bool QuestTakeCheckItem(UserItem userItem)
        {
            throw new NotImplementedException();
        }

        public bool IncGold(int gold)
        {
            throw new NotImplementedException();
        }

        public bool DecGold(int gold)
        {
            throw new NotImplementedException();
        }

        public void GoldChanged()
        {
            throw new NotImplementedException();
        }

        public void GameGoldChanged()
        {
            throw new NotImplementedException();
        }

        public bool DropItemDown(UserItem userItem, int nScatterRange, bool boDieDrop, int itemOfCreat, int dropCreat)
        {
            throw new NotImplementedException();
        }

        public UserItem CheckItemCount(string sItemName, ref int nCount)
        {
            throw new NotImplementedException();
        }

        public bool SellOffInTime(int time)
        {
            throw new NotImplementedException();
        }

        public bool GetFrontPosition(ref short nX, ref short nY)
        {
            throw new NotImplementedException();
        }

        public bool IsTrainingSkill(int nIndex)
        {
            throw new NotImplementedException();
        }

        public void ClientQueryBagItems()
        {
            throw new NotImplementedException();
        }

        public void SendGroupText(string sText)
        {
            throw new NotImplementedException();
        }

        public void SendDefMessage(short wIdent, int nRecog, int nParam, int nTag, int nSeries)
        {
            throw new NotImplementedException();
        }

        public void SendDefMessage(short wIdent, int nRecog, int nParam, int nTag, int nSeries, string sMsg)
        {
            throw new NotImplementedException();
        }

        public void RefRankInfo(short nRankNo, string sRankName)
        {
            throw new NotImplementedException();
        }

        public void ReAlive()
        {
            throw new NotImplementedException();
        }

        public void LeaveGroup()
        {
            throw new NotImplementedException();
        }

        public void JoinGroup(IPlayerActor actor)
        {
            throw new NotImplementedException();
        }

        public bool IsBlockWhisper(string sName)
        {
            throw new NotImplementedException();
        }

        public override void Whisper(string whostr, string saystr)
        {
            var playObject = SystemShare.WorldEngine.GetPlayObject(whostr);
            if (playObject != null)
            {
                if (!playObject.BoReadyRun)
                {
                    return;
                }
                if (!playObject.HearWhisper || playObject.IsBlockWhisper(ChrName))
                {
                    return;
                }
                if (Permission > 0)
                {
                    playObject.SendMsg(playObject, Messages.RM_WHISPER, 0, SystemShare.Config.btGMWhisperMsgFColor, SystemShare.Config.btGMWhisperMsgBColor, 0, Format("{0}[{1}级]=> {2}", ChrName, Abil.Level, saystr));
                    // 取得私聊信息
                    // m_GetWhisperHuman 侦听私聊对象
                    if (WhisperHuman != null && !WhisperHuman.Ghost)
                    {
                        WhisperHuman.SendMsg(WhisperHuman, Messages.RM_WHISPER, 0, SystemShare.Config.btGMWhisperMsgFColor, SystemShare.Config.btGMWhisperMsgBColor, 0, Format("{0}[{1}级]=> {2} {3}", ChrName, Abil.Level, playObject.ChrName, saystr));
                    }
                    if (playObject.WhisperHuman != null && !playObject.WhisperHuman.Ghost)
                    {
                        playObject.WhisperHuman.SendMsg(playObject.WhisperHuman, Messages.RM_WHISPER, 0, SystemShare.Config.btGMWhisperMsgFColor, SystemShare.Config.btGMWhisperMsgBColor, 0, Format("{0}[{1}级]=> {2} {3}", ChrName, Abil.Level, playObject.ChrName, saystr));
                    }
                }
                else
                {
                    playObject.SendMsg(playObject, Messages.RM_WHISPER, 0, SystemShare.Config.btGMWhisperMsgFColor, SystemShare.Config.btWhisperMsgBColor, 0, Format("{0}[{1}级]=> {2}", ChrName, Abil.Level, saystr));
                    if (WhisperHuman != null && !WhisperHuman.Ghost)
                    {
                        WhisperHuman.SendMsg(WhisperHuman, Messages.RM_WHISPER, 0, SystemShare.Config.btGMWhisperMsgFColor, SystemShare.Config.btWhisperMsgBColor, 0, Format("{0}[{1}级]=> {2} {3}", ChrName, Abil.Level, playObject.ChrName, saystr));
                    }
                    if (playObject.WhisperHuman != null && !playObject.WhisperHuman.Ghost)
                    {
                        playObject.WhisperHuman.SendMsg(playObject.WhisperHuman, Messages.RM_WHISPER, 0, SystemShare.Config.btGMWhisperMsgFColor, SystemShare.Config.btWhisperMsgBColor, 0, Format("{0}[{1}级]=> {2} {3}", ChrName, Abil.Level, playObject.ChrName, saystr));
                    }
                }
            }
        }

        public void ProcessUserLineMsg(string sText)
        {
            throw new NotImplementedException();
        }

        public bool LableIsCanJmp(string sText)
        {
            throw new NotImplementedException();
        }

        public bool AddItemToBag(UserItem userItem)
        {
            throw new NotImplementedException();
        }

        public UserItem CheckItems(string itemName)
        {
            throw new NotImplementedException();
        }

        public void ReQuestGuildWar(string guildName)
        {
            throw new NotImplementedException();
        }

        public void RecallHuman(string chrName)
        {
            throw new NotImplementedException();
        }

        public void SendUseItems()
        {
            throw new NotImplementedException();
        }

        public void StatusChanged()
        {
            throw new NotImplementedException();
        }

        public int GetCharStatus()
        {
            throw new NotImplementedException();
        }

        public void RefMyStatus()
        {
            throw new NotImplementedException();
        }

        public byte GetBackDir(byte dir)
        {
            throw new NotImplementedException();
        }

        public void ClientGuildAlly()
        {
            throw new NotImplementedException();
        }

        public void ClientGuildBreakAlly(string guildName)
        {
            throw new NotImplementedException();
        }

        public bool IsEnoughBag()
        {
            throw new NotImplementedException();
        }

        public void DealCancel()
        {
            throw new NotImplementedException();
        }

        public void AbilCopyToWAbil()
        {
            throw new NotImplementedException();
        }

        public void ClearStatusTime()
        {
            throw new NotImplementedException();
        }

        public void DelMember(IPlayerActor playerActor)
        {
            throw new NotImplementedException();
        }

        public bool InSafeArea()
        {
            throw new NotImplementedException();
        }

        public void OpenDealDlg(IPlayerActor playerActor)
        {
            throw new NotImplementedException();
        }

        public bool CanMove(short nX, short nY, bool boFlag)
        {
            throw new NotImplementedException();
        }

        public bool CanRun(short nCurrX, short nCurrY, short nX, short nY, bool boFlag)
        {
            throw new NotImplementedException();
        }

        public void GainExp(int dwExp)
        {
            throw new NotImplementedException();
        }

        public void AddBodyLuck(double dLuck)
        {
            throw new NotImplementedException();
        }

        public void SetPkFlag(IActor baseObject)
        {
            throw new NotImplementedException();
        }

        public void IncPkPoint(int nPoint)
        {
            throw new NotImplementedException();
        }

        public void MakeWeaponUnlock()
        {
            throw new NotImplementedException();
        }

        public void AttPowerUp(int nPower, int nTime)
        {
            throw new NotImplementedException();
        }

        public void ChangeSpaceMove(IEnvirnoment envir, short nX, short nY)
        {
            throw new NotImplementedException();
        }

        public void ChangeItemByJob(ref ClientItem citem, int level)
        {
            throw new NotImplementedException();
        }

        public void SendSocket(CommandMessage defMsg, string sMsg)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessSayMsg(string sData)
        {
            const string sExceptionMsg = "RoboPlayObject.ProcessSayMsg Msg:%s";
            if (string.IsNullOrEmpty(sData))
            {
                return;
            }
            try
            {
                var sParam1 = string.Empty;
                if (sData.Length > SystemShare.Config.SayMsgMaxLen)
                {
                    sData = sData[..SystemShare.Config.SayMsgMaxLen];
                }
                if (HUtil32.GetTickCount() >= DisableSayMsgTick)
                {
                    DisableSayMsg = false;
                }
                var boDisableSayMsg = DisableSayMsg;
                //g_DenySayMsgList.Lock;
                //if (g_DenySayMsgList.GetIndex(m_sChrName) >= 0)
                //{
                //    boDisableSayMsg = true;
                //}
                // g_DenySayMsgList.UnLock;
                if (!boDisableSayMsg)
                {
                    string sc;
                    if (sData[0] == '/')
                    {
                        sc = sData[1..];
                        sc = HUtil32.GetValidStr3(sc, ref sParam1, ' ');
                        if (!FilterSendMsg)
                        {
                            Whisper(sParam1, sc);
                        }
                        return;
                    }
                    if (sData[0] == '!')
                    {
                        if (sData.Length >= 2)
                        {
                            if (sData[1] == '!')//发送组队消息
                            {
                                sc = sData[2..];
                                SendGroupText(ChrName + ": " + sc);
                                return;
                            }
                            if (sData[1] == '~') //发送行会消息
                            {
                                if (MyGuild != null)
                                {
                                    sc = sData[2..];
                                    MyGuild.SendGuildMsg(ChrName + ": " + sc);
                                }
                                return;
                            }
                        }
                        if (!Envir.Flag.boQUIZ) //发送黄色喊话消息
                        {
                            if ((HUtil32.GetTickCount() - ShoutMsgTick) > 10 * 1000)
                            {
                                if (Abil.Level <= SystemShare.Config.CanShoutMsgLevel)
                                {
                                    SysMsg(Format(Settings.YouNeedLevelMsg, SystemShare.Config.CanShoutMsgLevel + 1), MsgColor.Red, MsgType.Hint);
                                    return;
                                }
                                ShoutMsgTick = HUtil32.GetTickCount();
                                sc = sData[1..];
                                var sCryCryMsg = "(!)" + ChrName + ": " + sc;
                                if (FilterSendMsg)
                                {
                                    SendMsg(null, Messages.RM_CRY, 0, 0, 0xFFFF, 0, sCryCryMsg);
                                }
                                else
                                {
                                    SystemShare.WorldEngine.CryCry(Messages.RM_CRY, Envir, CurrX, CurrY, 50, SystemShare.Config.CryMsgFColor, SystemShare.Config.CryMsgBColor, sCryCryMsg);
                                }
                                return;
                            }
                            SysMsg(Format(Settings.YouCanSendCyCyLaterMsg, 10 - (HUtil32.GetTickCount() - ShoutMsgTick) / 1000), MsgColor.Red, MsgType.Hint);
                            return;
                        }
                        SysMsg(Settings.ThisMapDisableSendCyCyMsg, MsgColor.Red, MsgType.Hint);
                        return;
                    }
                    if (!FilterSendMsg)
                    {
                        SendRefMsg(Messages.RM_HEAR, 0, SystemShare.Config.btHearMsgFColor, SystemShare.Config.btHearMsgBColor, 0, ChrName + ':' + sData);
                    }
                }
            }
            catch (Exception)
            {
                SystemShare.Logger.Error(Format(sExceptionMsg, sData));
            }
        }

        private UserMagic FindMagic(short wMagIdx)
        {
            UserMagic result = null;
            for (var i = 0; i < MagicList.Count; i++)
            {
                var userMagic = MagicList[i];
                if (userMagic.Magic.MagicId == wMagIdx)
                {
                    result = userMagic;
                    break;
                }
            }
            return result;
        }

        private UserMagic FindMagic(string sMagicName)
        {
            UserMagic result = null;
            for (var i = 0; i < MagicList.Count; i++)
            {
                var userMagic = MagicList[i];
                if (string.Compare(userMagic.Magic.MagicName, sMagicName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = userMagic;
                    break;
                }
            }
            return result;
        }

        private bool RunToNext(short nX, short nY)
        {
            var result = false;
            if ((HUtil32.GetTickCount() - RunIntervalTick) > SystemShare.Config.nAIRunIntervalTime)
            {
                result = RobotRunTo(SystemShare.GetNextDirection(CurrX, CurrY, nX, nY), false, nX, nY);
                RunIntervalTick = HUtil32.GetTickCount();
                //m_dwStationTick = HUtil32.GetTickCount();// 增加检测人物站立时间
            }
            return result;
        }

        private bool WalkToNext(short nX, short nY)
        {
            var result = false;
            if (HUtil32.GetTickCount() - WalkIntervalTick > SystemShare.Config.nAIWalkIntervalTime)
            {
                result = WalkTo(SystemShare.GetNextDirection(CurrX, CurrY, nX, nY), false);
                if (result)
                {
                    WalkIntervalTick = HUtil32.GetTickCount();
                }
                //m_dwStationTick = HUtil32.GetTickCount();// 增加检测人物站立时间
            }
            return result;
        }

        private bool GotoNextOne(short nX, short nY, bool boRun)
        {
            var result = false;
            if (Math.Abs(nX - CurrX) <= 2 && Math.Abs(nY - CurrY) <= 2)
            {
                if (Math.Abs(nX - CurrX) <= 1 && Math.Abs(nY - CurrY) <= 1)
                {
                    result = WalkToNext(nX, nY);
                }
                else
                {
                    result = RunToNext(nX, nY);
                }
            }
            MRunPos.AttackCount = 0;
            return result;
        }

        public void Hear(int nIndex, string sMsg)
        {
            switch (nIndex)
            {
                case Messages.RM_HEAR:
                    break;
                case Messages.RM_WHISPER:
                    if (HUtil32.GetTickCount() >= DisableSayMsgTick)
                    {
                        DisableSayMsg = false;
                    }
                    var boDisableSayMsg = DisableSayMsg;
                    // g_DenySayMsgList.Lock;
                    //if (g_DenySayMsgList.GetIndex(m_sChrName) >= 0)
                    //{
                    //    boDisableSayMsg = true;
                    //}
                    // g_DenySayMsgList.UnLock;
                    if (!boDisableSayMsg)
                    {
                        var nPos = sMsg.IndexOf("=>", StringComparison.OrdinalIgnoreCase);
                        if (nPos > 0 && AiSayMsgList.Count > 0)
                        {
                            var sChrName = sMsg[..(nPos - 1)];
                            var sSendMsg = sMsg.Substring(nPos + 3 - 1, sMsg.Length - nPos - 2);
                            Whisper(sChrName, "你猜我是谁.");
                            //Whisper(sChrName, m_AISayMsgList[(SystemShare.RandomNumber.Random(m_AISayMsgList.Count)).Next()]);
                            SystemShare.Logger.Error("TODO Hear...");
                        }
                    }
                    break;
                case Messages.RM_CRY:
                    break;
                case Messages.RM_SYSMESSAGE:
                    break;
                case Messages.RM_GROUPMESSAGE:
                    break;
                case Messages.RM_GUILDMESSAGE:
                    break;
                case Messages.RM_MERCHANTSAY:
                    break;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private void SearchPickUpItemSetHideItem(MapItem mapItem)
        {
            for (var i = 0; i < VisibleItems.Count; i++)
            {
                var visibleMapItem = VisibleItems[i];
                if (visibleMapItem != null && visibleMapItem.VisibleFlag > 0)
                {
                    if (visibleMapItem.MapItem == mapItem)
                    {
                        visibleMapItem.VisibleFlag = 0;
                        break;
                    }
                }
            }
        }

        private bool SearchPickUpItemPickUpItem(short nX, short nY)
        {
            var result = false;
            MapItem mapItem = default;
            var success = Envir.GetItem(nX, nY, ref mapItem);
            if (!success)
            {
                return false;
            }
            if (string.Compare(mapItem.Name, Grobal2.StringGoldName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (Envir.DeleteFromMap(nX, nY, CellType.Item, mapItem.ItemId, null) == 1)
                {
                    if (IncGold(mapItem.Count))
                    {
                        SendRefMsg(Messages.RM_ITEMHIDE, 0, mapItem.ItemId, nX, nY, "");
                        result = true;
                        GoldChanged();
                        SearchPickUpItemSetHideItem(mapItem);
                        Dispose(mapItem);
                    }
                    else
                    {
                        Envir.AddItemToMap(nX, nY, mapItem);
                    }
                }
                else
                {
                    Envir.AddItemToMap(nX, nY, mapItem);
                }
            }
            else
            {
                // 捡物品
                var stdItem = SystemShare.ItemSystem.GetStdItem(mapItem.UserItem.Index);
                if (stdItem != null)
                {
                    UserItem userItem = null;
                    if (Envir.DeleteFromMap(nX, nY, CellType.Item, mapItem.ItemId, null) == 1)
                    {
                        userItem = mapItem.UserItem;
                        stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                        if (stdItem != null && IsAddWeightAvailable(SystemShare.ItemSystem.GetStdItemWeight(userItem.Index)))
                        {
                            //if (GetCheckItemList(18, StdItem.Name))
                            //{
                            //    // 判断是否为绑定48时物品
                            //    UserItem.AddValue[0] = 2;
                            //    UserItem.MaxDate = HUtil32.IncDayHour(DateTime.Now, 48); // 解绑时间
                            //}
                            if (AddItemToBag(userItem))
                            {
                                SendRefMsg(Messages.RM_ITEMHIDE, 0, mapItem.ItemId, nX, nY, "");
                                SendAddItem(userItem);
                                Abil.Weight = RecalcBagWeight();
                                result = true;
                                SearchPickUpItemSetHideItem(mapItem);
                                Dispose(mapItem);
                            }
                            else
                            {
                                Dispose(userItem);
                                Envir.AddItemToMap(nX, nY, mapItem);
                            }
                        }
                        else
                        {
                            Dispose(userItem);
                            Envir.AddItemToMap(nX, nY, mapItem);
                        }
                    }
                    else
                    {
                        Dispose(userItem);
                        Envir.AddItemToMap(nX, nY, mapItem);
                    }
                }
            }
            return result;
        }

        private bool SearchPickUpItem(int nPickUpTime)
        {
            var result = false;
            VisibleMapItem visibleMapItem = null;
            try
            {
                if ((HUtil32.GetTickCount() - PickUpItemTick) < nPickUpTime)
                {
                    return false;
                }
                PickUpItemTick = HUtil32.GetTickCount();
                if (IsEnoughBag() && TargetCret == null)
                {
                    var boFound = false;
                    if (MSelMapItem.ItemId > 0)
                    {
                        CanPickIng = true;
                        for (var i = 0; i < VisibleItems.Count; i++)
                        {
                            visibleMapItem = VisibleItems[i];
                            if (visibleMapItem != null && visibleMapItem.VisibleFlag > 0)
                            {
                                if (visibleMapItem.MapItem == MSelMapItem)
                                {
                                    boFound = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!boFound)
                    {
                        MSelMapItem = default;
                    }
                    if (MSelMapItem.ItemId > 0)
                    {
                        if (SearchPickUpItemPickUpItem(CurrX, CurrY))
                        {
                            CanPickIng = false;
                            return true;
                        }
                    }
                    var n01 = 999;
                    VisibleMapItem selVisibleMapItem = null;
                    boFound = false;
                    if (MSelMapItem.ItemId > 0)
                    {
                        for (var i = 0; i < VisibleItems.Count; i++)
                        {
                            visibleMapItem = VisibleItems[i];
                            if (visibleMapItem != null && visibleMapItem.VisibleFlag > 0)
                            {
                                if (visibleMapItem.MapItem == MSelMapItem)
                                {
                                    selVisibleMapItem = visibleMapItem;
                                    boFound = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!boFound)
                    {
                        for (var i = 0; i < VisibleItems.Count; i++)
                        {
                            visibleMapItem = VisibleItems[i];
                            if (visibleMapItem != null)
                            {
                                if (visibleMapItem.VisibleFlag > 0)
                                {
                                    var mapItem = visibleMapItem.MapItem;
                                    if (mapItem.ItemId > 0)
                                    {
                                        if (IsAllowAiPickUpItem(visibleMapItem.sName) && IsAddWeightAvailable(SystemShare.ItemSystem.GetStdItemWeight(mapItem.UserItem.Index)))
                                        {
                                            if (mapItem.OfBaseObject == 0 || mapItem.OfBaseObject == ActorId || (SystemShare.ActorMgr.Get(mapItem.OfBaseObject).Master == this))
                                            {
                                                if (Math.Abs(visibleMapItem.nX - CurrX) <= 5 && Math.Abs(visibleMapItem.nY - CurrY) <= 5)
                                                {
                                                    var n02 = Math.Abs(visibleMapItem.nX - CurrX) + Math.Abs(visibleMapItem.nY - CurrY);
                                                    if (n02 < n01)
                                                    {
                                                        n01 = n02;
                                                        selVisibleMapItem = visibleMapItem;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (selVisibleMapItem != null)
                    {
                        MSelMapItem = selVisibleMapItem.MapItem;
                        if (MSelMapItem.ItemId > 0)
                        {
                            CanPickIng = true;
                        }
                        else
                        {
                            CanPickIng = false;
                        }
                        if (CurrX != selVisibleMapItem.nX || CurrY != selVisibleMapItem.nY)
                        {
                            WalkToTargetXy2(selVisibleMapItem.nX, visibleMapItem.nY);
                            result = true;
                        }
                    }
                    else
                    {
                        CanPickIng = false;
                    }
                }
                else
                {
                    CanPickIng = false;
                }
            }
            catch
            {
                SystemShare.Logger.Error("RoboPlayObject.SearchPickUpItem");
            }
            return result;
        }

        private static bool IsAllowAiPickUpItem(string sName)
        {
            return true;
        }

        private bool WalkToTargetXy2(short nTargetX, short nTargetY)
        {
            var result = false;
            if (Transparent && HideMode)
            {
                StatusTimeArr[PoisonState.STATETRANSPARENT] = 1;// 隐身,一动就显身
            }
            if (StatusTimeArr[PoisonState.STONE] != 0 && StatusTimeArr[PoisonState.DONTMOVE] != 0 || StatusTimeArr[PoisonState.LOCKSPELL] != 0)
            {
                return false;// 麻痹不能跑动 
            }
            if (nTargetX != CurrX || nTargetY != CurrY)
            {
                if ((HUtil32.GetTickCount() - WalkIntervalTick) > TurnIntervalTime)// 转向间隔
                {
                    var n10 = nTargetX;
                    var n14 = nTargetY;
                    var nDir = Direction.Down;
                    if (n10 > CurrX)
                    {
                        nDir = Direction.Right;
                        if (n14 > CurrY)
                        {
                            nDir = Direction.DownRight;
                        }
                        if (n14 < CurrY)
                        {
                            nDir = Direction.UpRight;
                        }
                    }
                    else
                    {
                        if (n10 < CurrX)
                        {
                            nDir = Direction.Left;
                            if (n14 > CurrY)
                            {
                                nDir = Direction.DownLeft;
                            }
                            if (n14 < CurrY)
                            {
                                nDir = Direction.UpLeft;
                            }
                        }
                        else
                        {
                            if (n14 > CurrY)
                            {
                                nDir = Direction.Down;
                            }
                            else if (n14 < CurrY)
                            {
                                nDir = Direction.Up;
                            }
                        }
                    }
                    int nOldX = CurrX;
                    int nOldY = CurrY;
                    WalkTo(nDir, false);
                    if (nTargetX == CurrX && nTargetY == CurrY)
                    {
                        result = true;
                        WalkIntervalTick = HUtil32.GetTickCount();
                    }
                    if (!result)
                    {
                        var n20 = SystemShare.RandomNumber.Random(3);
                        for (var i = Direction.Up; i <= Direction.UpLeft; i++)
                        {
                            if (nOldX == CurrX && nOldY == CurrY)
                            {
                                if (n20 != 0)
                                {
                                    nDir++;
                                }
                                else if (nDir > 0)
                                {
                                    nDir -= 1;
                                }
                                else
                                {
                                    nDir = Direction.UpLeft;
                                }
                                if (nDir > Direction.UpLeft)
                                {
                                    nDir = Direction.Up;
                                }
                                WalkTo(nDir, false);
                                if (nTargetX == CurrX && nTargetY == CurrY)
                                {
                                    result = true;
                                    WalkIntervalTick = HUtil32.GetTickCount();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        private void GotoProtect()
        {
            if (CurrX != ProtectTargetX || CurrY != ProtectTargetY)
            {
                int n10 = ProtectTargetX;
                int n14 = ProtectTargetY;
                WalkIntervalTick = HUtil32.GetTickCount();
                var nDir = Direction.Down;
                if (n10 > CurrX)
                {
                    nDir = Direction.Right;
                    if (n14 > CurrY)
                    {
                        nDir = Direction.DownRight;
                    }
                    if (n14 < CurrY)
                    {
                        nDir = Direction.UpRight;
                    }
                }
                else
                {
                    if (n10 < CurrX)
                    {
                        nDir = Direction.Left;
                        if (n14 > CurrY)
                        {
                            nDir = Direction.DownLeft;
                        }
                        if (n14 < CurrY)
                        {
                            nDir = Direction.UpLeft;
                        }
                    }
                    else
                    {
                        if (n14 > CurrY)
                        {
                            nDir = Direction.Down;
                        }
                        else if (n14 < CurrY)
                        {
                            nDir = Direction.Up;
                        }
                    }
                }
                var nOldX = CurrX;
                var nOldY = CurrY;
                if (Math.Abs(CurrX - ProtectTargetX) >= 3 || Math.Abs(CurrY - ProtectTargetY) >= 3)
                {
                    //m_dwStationTick = HUtil32.GetTickCount();// 增加检测人物站立时间
                    if (!RobotRunTo(nDir, false, ProtectTargetX, ProtectTargetY))
                    {
                        WalkTo(nDir, false);
                        var n20 = SystemShare.RandomNumber.Random(3);
                        for (var i = Direction.Up; i <= Direction.UpLeft; i++)
                        {
                            if (nOldX == CurrX && nOldY == CurrY)
                            {
                                if (n20 != 0)
                                {
                                    nDir++;
                                }
                                else if (nDir > 0)
                                {
                                    nDir -= 1;
                                }
                                else
                                {
                                    nDir = Direction.UpLeft;
                                }
                                if (nDir > Direction.UpLeft)
                                {
                                    nDir = Direction.Up;
                                }
                                WalkTo(nDir, false);
                            }
                        }
                    }
                }
                else
                {
                    WalkTo(nDir, false);
                    //m_dwStationTick = HUtil32.GetTickCount();// 增加检测人物站立时间
                    var n20 = SystemShare.RandomNumber.Random(3);
                    for (var i = Direction.Up; i <= Direction.UpLeft; i++)
                    {
                        if (nOldX == CurrX && nOldY == CurrY)
                        {
                            if (n20 != 0)
                            {
                                nDir++;
                            }
                            else if (nDir > 0)
                            {
                                nDir -= 1;
                            }
                            else
                            {
                                nDir = Direction.UpLeft;
                            }
                            if (nDir > Direction.UpLeft)
                            {
                                nDir = Direction.Up;
                            }
                            WalkTo(nDir, false);
                        }
                    }
                }
            }
        }

        protected override void Wondering()
        {
            if (RobotStart && TargetCret == null && !CanPickIng && !Ghost && !Death && !FixedHideMode && !StoneMode && StatusTimeArr[PoisonState.STONE] == 0)
            {
                var nX = CurrX;
                var nY = CurrY;
                if (MPath != null && MPath.Length > 0 && MNPostion < MPath.Length)
                {
                    if (!GotoNextOne(MPath[MNPostion].nX, MPath[MNPostion].nY, true))
                    {
                        MPath = null;
                        MNPostion = -1;
                        MoveFailCount++;
                        MNPostion++;
                    }
                    else
                    {
                        MoveFailCount = 0;
                        return;
                    }
                }
                else
                {
                    MPath = null;
                    MNPostion = -1;
                }
                if (PointManager.GetPoint(ref nX, ref nY))
                {
                    if (Math.Abs(nX - CurrX) > 2 || Math.Abs(nY - CurrY) > 2)
                    {
                        MPath = M2Share.FindPath.Find(Envir, CurrX, CurrY, nX, nY, true);
                        MNPostion = 0;
                        if (MPath.Length > 0 && MNPostion < MPath.Length)
                        {
                            if (!GotoNextOne(MPath[MNPostion].nX, MPath[MNPostion].nY, true))
                            {
                                MPath = null;
                                MNPostion = -1;
                                MoveFailCount++;
                            }
                            else
                            {
                                MoveFailCount = 0;
                                MNPostion++;
                                return;
                            }
                        }
                        else
                        {
                            MPath = null;
                            MNPostion = -1;
                            MoveFailCount++;
                        }
                    }
                    else
                    {
                        if (GotoNextOne(nX, nY, true))
                        {
                            MoveFailCount = 0;
                        }
                        else
                        {
                            MoveFailCount++;
                        }
                    }
                }
                else
                {
                    if (SystemShare.RandomNumber.Random(2) == 1)
                    {
                        TurnTo(SystemShare.RandomNumber.RandomByte(8));
                    }
                    else
                    {
                        WalkTo(Dir, false);
                    }
                    MPath = null;
                    MNPostion = -1;
                    MoveFailCount++;
                }
            }
            if (MoveFailCount >= 3)
            {
                if (SystemShare.RandomNumber.Random(2) == 1)
                {
                    TurnTo(SystemShare.RandomNumber.RandomByte(8));
                }
                else
                {
                    WalkTo(Dir, false);
                }
                MPath = null;
                MNPostion = -1;
                MoveFailCount = 0;
            }
        }

        private IActor StruckMinXY(IActor aObject, IActor bObject)
        {
            var nA = Math.Abs(CurrX - aObject.CurrX) + Math.Abs(CurrY - aObject.CurrY);
            var nB = Math.Abs(CurrX - bObject.CurrX) + Math.Abs(CurrY - bObject.CurrY);
            return nA > nB ? bObject : aObject;
        }

        private bool CanWalk(short nCurrX, short nCurrY, short nTargetX, short nTargetY, byte nDir, ref int nStep, bool boFlag)
        {
            var result = false;
            short nX = 0;
            short nY = 0;
            nStep = 0;
            byte btDir;
            if (nDir > 0 && nDir < 8)
            {
                btDir = nDir;
            }
            else
            {
                btDir = SystemShare.GetNextDirection(nCurrX, nCurrY, nTargetX, nTargetY);
            }
            if (boFlag)
            {
                if (Math.Abs(nCurrX - nTargetX) <= 1 && Math.Abs(nCurrY - nTargetY) <= 1)
                {
                    if (Envir.GetNextPosition(nCurrX, nCurrY, btDir, 1, ref nX, ref nY) && nX == nTargetX && nY == nTargetY)
                    {
                        nStep = 1;
                        result = true;
                    }
                }
                else
                {
                    if (Envir.GetNextPosition(nCurrX, nCurrY, btDir, 2, ref nX, ref nY) && nX == nTargetX && nY == nTargetY)
                    {
                        nStep = 1;
                        result = true;
                    }
                }
            }
            else
            {
                if (Envir.GetNextPosition(nCurrX, nCurrY, btDir, 1, ref nX, ref nY) && nX == nTargetX && nY == nTargetY)
                {
                    nStep = nStep + 1;
                    return true;
                }
                if (Envir.GetNextPosition(nX, nY, btDir, 1, ref nX, ref nY) && nX == nTargetX && nY == nTargetY)
                {
                    nStep = nStep + 1;
                    return true;
                }
            }
            return result;
        }

        private bool IsGotoXy(short x1, short y1, short x2, short y2)
        {
            var nStep = 0;
            //0代替-1
            if (!CanWalk(x1, y1, x2, y2, 0, ref nStep, Race != 108))
            {
                PointInfo[] path = M2Share.FindPath.Find(Envir, x1, y1, x2, y2, false);
                if (path.Length <= 0)
                {
                    return false;
                }
            }
            return true;
        }

        private bool GotoNext(short nX, short nY, bool boRun)
        {
            var result = false;
            var nStep = 0;
            if (Math.Abs(nX - CurrX) <= 2 && Math.Abs(nY - CurrY) <= 2)
            {
                if (Math.Abs(nX - CurrX) <= 1 && Math.Abs(nY - CurrY) <= 1)
                {
                    result = WalkToNext(nX, nY);
                }
                else
                {
                    result = RunToNext(nX, nY);
                }
                nStep = 1;
            }
            if (!result)
            {
                PointInfo[] path = M2Share.FindPath.Find(Envir, CurrX, CurrY, nX, nY, boRun);
                if (path.Length > 0)
                {
                    for (var i = 0; i < path.Length; i++)
                    {
                        if (path[i].nX != CurrX || path[i].nY != CurrY)
                        {
                            if (Math.Abs(path[i].nX - CurrX) >= 2 || Math.Abs(path[i].nY - CurrY) >= 2)
                            {
                                result = RunToNext(path[i].nX, path[i].nY);
                            }
                            else
                            {
                                result = WalkToNext(path[i].nX, path[i].nY);
                            }
                            if (result)
                            {
                                nStep++;
                            }
                            else
                            {
                                break;
                            }
                            if (nStep >= 3)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            MRunPos.AttackCount = 0;
            return result;
        }

        protected override bool Operate(ProcessMessage processMsg)
        {
            var result = false;
            try
            {
                if (processMsg.wIdent == Messages.RM_STRUCK)
                {
                    if (processMsg.ActorId == ActorId)
                    {
                        var attackBaseObject = SystemShare.ActorMgr.Get(processMsg.nParam3);
                        if (attackBaseObject != null)
                        {
                            if (attackBaseObject.Race == ActorRace.Play)
                            {
                                SetPkFlag(attackBaseObject);
                            }
                            SetLastHiter(attackBaseObject);
                            Struck(attackBaseObject);
                        }
                        if (SystemShare.CastleMgr.IsCastleMember(this) != null && attackBaseObject != null)
                        {
                            if (attackBaseObject.Race == ActorRace.Guard)
                            {
                                ((GuardUnit)attackBaseObject).CrimeforCastle = true;
                                ((GuardUnit)attackBaseObject).CrimeforCastleTime = HUtil32.GetTickCount();
                            }
                        }
                        HealthTick = 0;
                        SpellTick = 0;
                        PerHealth -= 1;
                        PerSpell -= 1;
                        StruckTick = HUtil32.GetTickCount();
                    }
                    result = true;
                }
                else
                {
                    result = base.Operate(processMsg);
                }
            }
            catch (Exception ex)
            {
                SystemShare.Logger.Error(ex.Message);
            }
            return result;
        }

        private int GetRangeTargetCountByDir(byte nDir, short nX, short nY, int nRange)
        {
            var result = 0;
            var nCurrX = nX;
            var nCurrY = nY;
            for (var i = 0; i < nRange; i++)
            {
                if (Envir.GetNextPosition(nCurrX, nCurrY, nDir, 1, ref nCurrX, ref nCurrY))
                {
                    var baseObject = Envir.GetMovingObject(nCurrX, nCurrY, true);
                    if (baseObject != null && !baseObject.Death && !baseObject.Ghost && (!baseObject.HideMode || CoolEye) && IsProperTarget(baseObject))
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        private int GetNearTargetCount()
        {
            var result = 0;
            short nX = 0;
            short nY = 0;
            for (var n10 = 0; n10 < 7; n10++)
            {
                if (Envir.GetNextPosition(CurrX, CurrY, (byte)n10, 1, ref nX, ref nY))
                {
                    var baseObject = Envir.GetMovingObject(nX, nY, true);
                    if (baseObject != null && !baseObject.Death && !baseObject.Ghost && IsProperTarget(baseObject))
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        private int GetNearTargetCount(short nCurrX, short nCurrY)
        {
            var result = 0;
            short nX = 0;
            short nY = 0;
            var baseObject = Envir.GetMovingObject(nCurrX, nCurrY, true);
            if (baseObject != null && !baseObject.Death && !baseObject.Ghost && IsProperTarget(baseObject))
            {
                result++;
            }
            for (byte i = 0; i < 7; i++)
            {
                if (Envir.GetNextPosition(nCurrX, nCurrY, i, 1, ref nX, ref nY))
                {
                    baseObject = Envir.GetMovingObject(nX, nY, true);
                    if (baseObject != null && !baseObject.Death && !baseObject.Ghost && IsProperTarget(baseObject))
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        private int GetMasterRange(int nTargetX, int nTargetY)
        {
            if (Master != null)
            {
                var nCurrX = Master.CurrX;
                var nCurrY = Master.CurrY;
                return Math.Abs(nCurrX - nTargetX) + Math.Abs(nCurrY - nTargetY);
            }
            return 0;
        }

        /// <summary>
        /// 跟随主人
        /// </summary>
        /// <returns></returns>
        private void FollowMaster()
        {
            short nX = 0;
            short nY = 0;
            short nCurrX = 0;
            short nCurrY = 0;
            var boNeed = false;
            if (!Master.SlaveRelax)
            {
                if (Envir != Master.Envir || Math.Abs(CurrX - Master.CurrX) > 20 || Math.Abs(CurrY - Master.CurrY) > 20)
                {
                    boNeed = true;
                }
            }
            if (boNeed)
            {
                Master.GetBackPosition(ref nX, ref nY);
                if (!Master.Envir.CanWalk(nX, nY, true))
                {
                    for (byte i = 0; i < 7; i++)
                    {
                        if (Master.Envir.GetNextPosition(Master.CurrX, Master.CurrY, i, 1, ref nX, ref nY))
                        {
                            if (Master.Envir.CanWalk(nX, nY, true))
                            {
                                break;
                            }
                        }
                    }
                }
                DelTargetCreat();
                TargetX = nX;
                TargetY = nY;
                SpaceMove(Master.Envir.MapName, TargetX, TargetY, 1);
                return;
            }
            Master.GetBackPosition(ref nCurrX, ref nCurrY);
            if (TargetCret == null && !Master.SlaveRelax)
            {
                for (byte i = 0; i < 2; i++)
                {
                    if (Master.Envir.GetNextPosition(Master.CurrX, Master.CurrY, Master.Dir, i, ref nX, ref nY))// 判断主人是否在英雄对面
                    {
                        if (CurrX == nX && CurrY == nY)
                        {
                            if (Master.GetBackPosition(ref nX, ref nY) && GotoNext(nX, nY, true))
                            {
                                return;
                            }
                            for (var k = 0; k < 2; k++)
                            {
                                for (byte j = 0; j < 7; j++)
                                {
                                    if (j != Master.Dir)
                                    {
                                        if (Master.Envir.GetNextPosition(Master.CurrX, Master.CurrY, j, k, ref nX, ref nY) && GotoNext(nX, nY, true))
                                        {
                                            return;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
                var nStep = Race == 108 ? 0 : 1;
                if (Math.Abs(CurrX - nCurrX) > nStep || Math.Abs(CurrY - nCurrY) > nStep)
                {
                    if (GotoNextOne(nCurrX, nCurrY, true))
                    {
                        return;
                    }
                    if (GotoNextOne(nX, nY, true))
                    {
                        return;
                    }
                    for (var j = 0; j < 2; j++)
                    {
                        for (byte k = 0; k < 7; k++)
                        {
                            if (k != Master.Dir)
                            {
                                if (Master.Envir.GetNextPosition(Master.CurrX, Master.CurrY, k, j, ref nX, ref nY) && GotoNextOne(nX, nY, true))
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool FindVisibleActors(IActor actorObject)
        {
            var result = false;
            for (var i = 0; i < VisibleActors.Count; i++)
            {
                if (VisibleActors[i].BaseObject == actorObject)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private bool AllowUseMagic(short magIdx)
        {
            var result = false;
            var userMagic = FindMagic(magIdx);
            if (userMagic != null)
            {
                if (!MagicManager.IsWarrSkill(userMagic.MagIdx))
                {
                    result = userMagic.Key == 0 || IsRobot;
                }
                else
                {
                    result = userMagic.Key == 0 || IsRobot;
                }
            }
            return result;
        }

        private bool CheckUserItem(int nItemType, int nCount)
        {
            return CheckUserItemType(nItemType, nCount) || GetUserItemList(nItemType, nCount) >= 0;
        }

        private static bool CheckItemType(int nItemType, StdItem stdItem)
        {
            var result = false;
            switch (nItemType)
            {
                case 1:
                    if (stdItem.StdMode == 25 && stdItem.Shape == 1)
                    {
                        result = true;
                    }
                    break;
                case 2:
                    if (stdItem.StdMode == 25 && stdItem.Shape == 2)
                    {
                        result = true;
                    }
                    break;
                case 3:
                    if (stdItem.StdMode == 25 && stdItem.Shape == 3)
                    {
                        result = true;
                    }
                    break;
                case 5:
                    if (stdItem.StdMode == 25 && stdItem.Shape == 5)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        // 自动换毒符
        private bool CheckUserItemType(int nItemType, int nCount)
        {
            var result = false;
            if (UseItems[ItemLocation.ArmRingl] != null && UseItems[ItemLocation.ArmRingl].Index > 0 &&
                Math.Round(Convert.ToDouble(UseItems[ItemLocation.ArmRingl].Dura / 100)) >= nCount)
            {
                var stdItem = SystemShare.ItemSystem.GetStdItem(UseItems[ItemLocation.ArmRingl].Index);
                if (stdItem != null)
                {
                    result = CheckItemType(nItemType, stdItem);
                }
            }
            return result;
        }

        // 检测包裹中是否有符和毒
        // nType 为指定类型 5 为护身符 1,2 为毒药   3,诅咒术专用
        private int GetUserItemList(int nItemType, int nCount)
        {
            var result = -1;
            for (var i = 0; i < ItemList.Count; i++)
            {
                var stdItem = SystemShare.ItemSystem.GetStdItem(ItemList[i].Index);
                if (stdItem != null)
                {
                    if (CheckItemType(nItemType, stdItem) && HUtil32.Round(ItemList[i].Dura / 100.0) >= nCount)
                    {
                        result = i;
                        break;
                    }
                }
            }
            return result;
        }

        // 自动换毒符
        private bool UseItem(int nItemType, int nIndex)
        {
            var result = false;
            if (nIndex >= 0 && nIndex < ItemList.Count)
            {
                var userItem = ItemList[nIndex];
                if (UseItems[ItemLocation.ArmRingl].Index > 0)
                {
                    var stdItem = SystemShare.ItemSystem.GetStdItem(UseItems[ItemLocation.ArmRingl].Index);
                    if (stdItem != null)
                    {
                        if (CheckItemType(nItemType, stdItem))
                        {
                            result = true;
                        }
                        else
                        {
                            ItemList.RemoveAt(nIndex);
                            var addUserItem = UseItems[ItemLocation.ArmRingl];
                            if (AddItemToBag(addUserItem))
                            {
                                UseItems[ItemLocation.ArmRingl] = userItem;
                                Dispose(userItem);
                                result = true;
                            }
                            else
                            {
                                ItemList.Add(userItem);
                                Dispose(addUserItem);
                            }
                        }
                    }
                    else
                    {
                        ItemList.RemoveAt(nIndex);
                        UseItems[ItemLocation.ArmRingl] = userItem;
                        Dispose(userItem);
                        result = true;
                    }
                }
                else
                {
                    ItemList.RemoveAt(nIndex);
                    UseItems[ItemLocation.ArmRingl] = userItem;
                    Dispose(userItem);
                    result = true;
                }
            }
            return result;
        }

        private int GetRangeTargetCount(short nX, short nY, int nRange)
        {
            IList<BaseObject> baseObjectList = new List<BaseObject>();
            if (Envirnoment.GetMapBaseObjects(nX, nY, nRange, baseObjectList))
            {
                for (var i = baseObjectList.Count - 1; i >= 0; i--)
                {
                    var baseObject = baseObjectList[i];
                    if (baseObject.HideMode && !CoolEye || !IsProperTarget(baseObject))
                    {
                        baseObjectList.RemoveAt(i);
                    }
                }
                return baseObjectList.Count;
            }
            return 0;
        }

        // 目标是否和自己在一条线上，用来检测直线攻击的魔法是否可以攻击到目标
        private bool CanLineAttack(short nCurrX, short nCurrY)
        {
            var result = false;
            var nX = nCurrX;
            var nY = nCurrY;
            //byte btDir = SystemShare.GetNextDirection(nCurrX, nCurrY, TargetCret.CurrX, TargetCret.CurrY);
            while (true)
            {
                if (TargetCret.CurrX == nX && TargetCret.CurrY == nY)
                {
                    result = true;
                    break;
                }
                var btDir = SystemShare.GetNextDirection(nX, nY, TargetCret.CurrX, TargetCret.CurrY);
                if (!Envir.GetNextPosition(nX, nY, btDir, 1, ref nX, ref nY))
                {
                    break;
                }
                if (!Envir.CanWalkEx(nX, nY, true))
                {
                    break;
                }
            }
            return result;
        }

        // 是否是能直线攻击
        private bool CanLineAttack(int nStep)
        {
            var result = false;
            var nX = CurrX;
            var nY = CurrY;
            //byte btDir = SystemShare.GetNextDirection(nX, nY, TargetCret.CurrX, TargetCret.CurrY);
            for (var i = 0; i < nStep; i++)
            {
                if (TargetCret.CurrX == nX && TargetCret.CurrY == nY)
                {
                    result = true;
                    break;
                }
                var btDir = SystemShare.GetNextDirection(nX, nY, TargetCret.CurrX, TargetCret.CurrY);
                if (!Envir.GetNextPosition(nX, nY, btDir, 1, ref nX, ref nY))
                {
                    break;
                }
                if (!Envir.CanWalkEx(nX, nY, true))
                {
                    break;
                }
            }
            return result;
        }

        private bool CanAttack(short nCurrX, short nCurrY, IActor targetObject, int nRange, ref byte btDir)
        {
            var result = false;
            short nX = 0;
            short nY = 0;
            btDir = SystemShare.GetNextDirection(nCurrX, nCurrY, targetObject.CurrX, targetObject.CurrY);
            for (var i = 0; i < nRange; i++)
            {
                if (!Envir.GetNextPosition(nCurrX, nCurrY, btDir, i, ref nX, ref nY))
                {
                    break;
                }
                if (targetObject.CurrX == nX && targetObject.CurrY == nY)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private bool CanAttack(IActor targetObject, int nRange, ref byte btDir)
        {
            short nX = 0;
            short nY = 0;
            var result = false;
            btDir = SystemShare.GetNextDirection(CurrX, CurrY, targetObject.CurrX, targetObject.CurrY);
            for (var i = 0; i < nRange; i++)
            {
                if (!Envir.GetNextPosition(CurrX, CurrY, btDir, i, ref nX, ref nY))
                {
                    break;
                }
                if (targetObject.CurrX == nX && targetObject.CurrY == nY)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 检测是否可以使用攻击魔法
        /// </summary>
        /// <returns></returns>
        private bool IsUseAttackMagic()
        {
            UserMagic userMagic;
            var result = false;
            switch (Job)
            {
                case PlayJob.Warrior:
                    result = true;
                    break;
                case PlayJob.Wizard:
                    for (var i = 0; i < MagicList.Count; i++)
                    {
                        userMagic = MagicList[i];
                        switch (userMagic.MagIdx)
                        {
                            case MagicConst.SKILL_FIREBALL:
                            case MagicConst.SKILL_FIREBALL2:
                            case MagicConst.SKILL_FIRE:
                            case MagicConst.SKILL_SHOOTLIGHTEN:
                            case MagicConst.SKILL_LIGHTENING:
                            case MagicConst.SKILL_EARTHFIRE:
                            case MagicConst.SKILL_FIREBOOM:
                            case MagicConst.SKILL_LIGHTFLOWER:
                            case MagicConst.SKILL_SNOWWIND:
                            case MagicConst.SKILL_GROUPLIGHTENING:
                            case MagicConst.SKILL_47:
                            case MagicConst.SKILL_58:
                                if (GetSpellPoint(userMagic) <= Abil.MP)
                                {
                                    result = true;
                                    break;
                                }
                                break;
                        }
                    }
                    break;
                case PlayJob.Taoist:
                    for (var i = 0; i < MagicList.Count; i++)
                    {
                        userMagic = MagicList[i];
                        if (userMagic.Magic.Job == 2 || userMagic.Magic.Job == 99)
                        {
                            switch (userMagic.MagIdx)
                            {
                                case MagicConst.SKILL_AMYOUNSUL:
                                case MagicConst.SKILL_GROUPAMYOUNSUL:// 需要毒药
                                    result = CheckUserItem(1, 2) || CheckUserItem(2, 2);
                                    if (result)
                                    {
                                        result = AllowUseMagic(MagicConst.SKILL_AMYOUNSUL) || AllowUseMagic(MagicConst.SKILL_GROUPAMYOUNSUL);
                                    }
                                    if (result)
                                    {
                                        break;
                                    }
                                    break;
                                case MagicConst.SKILL_FIRECHARM:// 需要符
                                    result = CheckUserItem(5, 1);
                                    if (result)
                                    {
                                        result = AllowUseMagic(MagicConst.SKILL_FIRECHARM);
                                    }
                                    if (result)
                                    {
                                        break;
                                    }
                                    break;
                                case MagicConst.SKILL_59:// 需要符
                                    result = CheckUserItem(5, 5);
                                    if (result)
                                    {
                                        result = AllowUseMagic(MagicConst.SKILL_59);
                                    }
                                    if (result)
                                    {
                                        break;
                                    }
                                    break;
                            }
                        }
                    }
                    break;
            }
            return result;
        }

        private bool UseSpell(UserMagic userMagic, short nTargetX, short nTargetY, IActor targetObject)
        {
            var result = false;
            if (!IsCanSpell)
            {
                return false;
            }
            if (Death || StatusTimeArr[PoisonState.LOCKSPELL] != 0)
            {
                return false; // 防麻
            }
            if (StatusTimeArr[PoisonState.STONE] != 0)
            {
                return false;// 防麻
            }
            if (Envir != null)
            {
                if (!Envirnoment.AllowMagics(userMagic.Magic.MagicName))
                {
                    return false;
                }
            }
            SpellTick -= 450;
            SpellTick = HUtil32._MAX(0, SpellTick);
            switch (userMagic.MagIdx)
            {
                case MagicConst.SKILL_ERGUM:
                    if (MagicArr[MagicConst.SKILL_ERGUM] != null)
                    {
                        if (!UseThrusting)
                        {
                            ThrustingOnOff(true);
                        }
                        else
                        {
                            ThrustingOnOff(false);
                        }
                    }
                    result = true;
                    break;
                case MagicConst.SKILL_BANWOL:
                    if (MagicArr[MagicConst.SKILL_BANWOL] != null)
                    {
                        if (!UseHalfMoon)
                        {
                            HalfMoonOnOff(true);
                        }
                        else
                        {
                            HalfMoonOnOff(false);
                        }
                    }
                    result = true;
                    break;
                case MagicConst.SKILL_FIRESWORD:
                    if (MagicArr[MagicConst.SKILL_FIRESWORD] != null)
                    {
                        result = true;
                    }
                    break;
                case MagicConst.SKILL_MOOTEBO:
                    result = true;
                    if ((HUtil32.GetTickCount() - DoMotaeboTick) > 3000)
                    {
                        DoMotaeboTick = HUtil32.GetTickCount();
                        byte attDir = 0;
                        if (GetAttackDir(targetObject, ref attDir))
                        {
                            Dir = attDir;
                            DoMotaebo(Dir, userMagic.Level);
                        }
                    }
                    break;
                case MagicConst.SKILL_43:
                    result = true;
                    break;
                default:
                    int n14 = SystemShare.GetNextDirection(CurrX, CurrY, nTargetX, nTargetY);
                    Dir = (byte)n14;
                    IActor baseObject = null;
                    if (userMagic.MagIdx >= 60 && userMagic.MagIdx <= 65)
                    {
                        if (CretInNearXy(targetObject, nTargetX, nTargetY))// 检查目标角色，与目标座标误差范围，如果在误差范围内则修正目标座标
                        {
                            baseObject = targetObject;
                            nTargetX = baseObject.CurrX;
                            nTargetY = baseObject.CurrY;
                        }
                    }
                    else
                    {
                        switch (userMagic.MagIdx)
                        {
                            case MagicConst.SKILL_HEALLING:
                            case MagicConst.SKILL_HANGMAJINBUB:
                            case MagicConst.SKILL_DEJIWONHO:
                            case MagicConst.SKILL_BIGHEALLING:
                            case MagicConst.SKILL_SINSU:
                            case MagicConst.SKILL_UNAMYOUNSUL:
                            case MagicConst.SKILL_46:
                                if (BoSelSelf)
                                {
                                    baseObject = this;
                                    nTargetX = CurrX;
                                    nTargetY = CurrY;
                                }
                                else
                                {
                                    if (Master != null)
                                    {
                                        baseObject = Master;
                                        nTargetX = Master.CurrX;
                                        nTargetY = Master.CurrY;
                                    }
                                    else
                                    {
                                        baseObject = this;
                                        nTargetX = CurrX;
                                        nTargetY = CurrY;
                                    }
                                }
                                break;
                            default:
                                if (CretInNearXy(targetObject, nTargetX, nTargetY))
                                {
                                    baseObject = targetObject;
                                    nTargetX = baseObject.CurrX;
                                    nTargetY = baseObject.CurrY;
                                }
                                break;
                        }
                    }
                    if (!AutoSpell(userMagic, nTargetX, nTargetY, baseObject))
                    {
                        SendRefMsg(Messages.RM_MAGICFIREFAIL, 0, 0, 0, 0, "");
                    }
                    result = true;
                    break;
            }
            return result;
        }

        private bool AutoSpell(UserMagic userMagic, short nTargetX, short nTargetY, IActor targetObject)
        {
            var result = false;
            try
            {
                if (targetObject != null)
                {
                    if (targetObject.Ghost || targetObject.Death || targetObject.WAbil.HP <= 0)
                    {
                        return false;
                    }
                }
                if (!MagicManager.IsWarrSkill(userMagic.MagIdx))
                {
                    result = MagicManager.DoSpell(this, userMagic, nTargetX, nTargetY, targetObject);
                    AttackTick = HUtil32.GetTickCount();
                }
            }
            catch (Exception)
            {
                SystemShare.Logger.Error(Format("RoboPlayObject.AutoSpell MagID:{0} X:{1} Y:{2}", userMagic.MagIdx, nTargetX, nTargetY));
            }
            return result;
        }

        private bool Thinking()
        {
            var result = false;
            try
            {
                if (SystemShare.Config.RobotAutoPickUpItem)//&& (g_AllowAIPickUpItemList.Count > 0)
                {
                    if (SearchPickUpItem(500))
                    {
                        result = true;
                    }
                }
                if (Master != null && Master.Ghost)
                {
                    return false;
                }
                if (Master != null && Master.InSafeZone() && InSafeZone())
                {
                    if (Math.Abs(CurrX - Master.CurrX) <= 3 && Math.Abs(CurrY - Master.CurrY) <= 3)
                    {
                        return true;
                    }
                }
                if (HUtil32.GetTickCount() - ThinkTick > 3000)
                {
                    ThinkTick = HUtil32.GetTickCount();
                    if (Envir.GetXYObjCount(CurrX, CurrY) >= 2)
                    {
                        MBoDupMode = true;
                    }
                    if (TargetCret != null)
                    {
                        if (!IsProperTarget(TargetCret))
                        {
                            DelTargetCreat();
                        }
                    }
                }
                if (MBoDupMode)
                {
                    int nOldX = CurrX;
                    int nOldY = CurrY;
                    WalkTo(SystemShare.RandomNumber.RandomByte(8), false);
                    //m_dwStationTick = HUtil32.GetTickCount(); // 增加检测人物站立时间
                    if (nOldX != CurrX || nOldY != CurrY)
                    {
                        MBoDupMode = false;
                        result = true;
                    }
                }
            }
            catch
            {
                SystemShare.Logger.Error("RoboPlayObject.Thinking");
            }
            return result;
        }

        /// <summary>
        /// 是否走向目标
        /// </summary>
        /// <returns></returns>
        private bool IsNeedGotoXy()
        {
            if (TargetCret != null && HUtil32.GetTickCount() - AutoAvoidTick > 1100 && (!BoUseAttackMagic || Job == 0))
            {
                if (Job > 0)
                {
                    if (!AutoUseMagic && (Math.Abs(TargetCret.CurrX - CurrX) > 3 || Math.Abs(TargetCret.CurrY - CurrY) > 3))
                    {
                        return true;
                    }
                    if ((AttackLevelTarget() || TaoLevelHitAttack() && TargetCret.Abil.MaxHP < 700 && Job == PlayJob.Taoist) && (Math.Abs(TargetCret.CurrX - CurrX) > 1 || Math.Abs(TargetCret.CurrY - CurrY) > 1))// 道法22前是否物理攻击大于1格时才走向目标
                    {
                        return true;
                    }
                }
                else
                {
                    long dwAttackTime;
                    switch (AutoMagicId)
                    {
                        case MagicConst.SKILL_ERGUM:
                            if (AllowUseMagic(MagicConst.SKILL_ERGUM) && Envir.GetNextPosition(CurrX, CurrY, Dir, 2, ref TargetX, ref TargetY))
                            {
                                if (Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 0 || Math.Abs(CurrX - TargetCret.CurrX) == 0 && Math.Abs(CurrY - TargetCret.CurrY) == 2 || Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 2)
                                {
                                    dwAttackTime = HUtil32._MAX(0, (int)SystemShare.Config.dwHeroWarrorAttackTime - HitSpeed * SystemShare.Config.ItemSpeed); // 防止负数出错
                                    if (HUtil32.GetTickCount() - AttackTick > dwAttackTime)
                                    {
                                        HitMode = 4;
                                        TargetFocusTick = HUtil32.GetTickCount();
                                        Dir = SystemShare.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
                                        Attack(TargetCret, Dir);
                                        AttackTick = HUtil32.GetTickCount();
                                        return false;
                                    }
                                }
                                else
                                {
                                    if (Math.Abs(TargetCret.CurrX - CurrX) > 1 || Math.Abs(TargetCret.CurrY - CurrY) > 1)
                                    {
                                        return true;
                                    }
                                }
                            }
                            AutoMagicId = 0;
                            if (AllowUseMagic(MagicConst.SKILL_ERGUM))
                            {
                                if (Math.Abs(TargetCret.CurrX - CurrX) > 2 || Math.Abs(TargetCret.CurrY - CurrY) > 2)
                                {
                                    return true;
                                }
                            }
                            else if (Math.Abs(TargetCret.CurrX - CurrX) > 1 || Math.Abs(TargetCret.CurrY - CurrY) > 1)
                            {
                                return true;
                            }
                            break;
                        case 43:
                            if (Envir.GetNextPosition(CurrX, CurrY, Dir, 5, ref TargetX, ref TargetY))
                            {
                                if (Math.Abs(CurrX - TargetCret.CurrX) <= 4 && Math.Abs(CurrY - TargetCret.CurrY) == 0 || Math.Abs(CurrX - TargetCret.CurrX) == 0 && Math.Abs(CurrY - TargetCret.CurrY) <= 4 || Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 2 || Math.Abs(CurrX - TargetCret.CurrX) == 3 && Math.Abs(CurrY - TargetCret.CurrY) == 3 || Math.Abs(CurrX - TargetCret.CurrX) == 4 && Math.Abs(CurrY - TargetCret.CurrY) == 4)
                                {
                                    dwAttackTime = HUtil32._MAX(0, (int)SystemShare.Config.dwHeroWarrorAttackTime - HitSpeed * SystemShare.Config.ItemSpeed);// 防止负数出错
                                    if (HUtil32.GetTickCount() - AttackTick > dwAttackTime)
                                    {
                                        HitMode = 9;
                                        TargetFocusTick = HUtil32.GetTickCount();
                                        Dir = SystemShare.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
                                        Attack(TargetCret, Dir);
                                        AttackTick = HUtil32.GetTickCount();
                                        return false;
                                    }
                                }
                                else
                                {
                                    if (AllowUseMagic(MagicConst.SKILL_ERGUM))
                                    {
                                        if (Math.Abs(CurrX - TargetCret.CurrX) != 2 && Math.Abs(CurrY - TargetCret.CurrY) != 0 || Math.Abs(CurrX - TargetCret.CurrX) != 0 && Math.Abs(CurrY - TargetCret.CurrY) != 2 || Math.Abs(CurrX - TargetCret.CurrX) != 2 && Math.Abs(CurrY - TargetCret.CurrY) != 2)
                                        {
                                            return true;
                                        }
                                    }
                                    else if (Math.Abs(TargetCret.CurrX - CurrX) > 1 || Math.Abs(TargetCret.CurrY - CurrY) > 1)
                                    {
                                        return true;
                                    }
                                }
                            }
                            AutoMagicId = 0;
                            if (Envir.GetNextPosition(CurrX, CurrY, Dir, 2, ref TargetX, ref TargetY))
                            {
                                if (Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 0 || Math.Abs(CurrX - TargetCret.CurrX) == 0 && Math.Abs(CurrY - TargetCret.CurrY) == 2 || Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 2)
                                {
                                    dwAttackTime = HUtil32._MAX(0, (int)SystemShare.Config.dwHeroWarrorAttackTime - HitSpeed * SystemShare.Config.ItemSpeed);// 防止负数出错
                                    if (HUtil32.GetTickCount() - AttackTick > dwAttackTime)
                                    {
                                        HitMode = 9;
                                        TargetFocusTick = HUtil32.GetTickCount();
                                        Dir = SystemShare.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
                                        Attack(TargetCret, Dir);
                                        AttackTick = HUtil32.GetTickCount();
                                        return false;
                                    }
                                }
                                else
                                {
                                    if (Math.Abs(TargetCret.CurrX - CurrX) > 1 || Math.Abs(TargetCret.CurrY - CurrY) > 1)
                                    {
                                        return true;
                                    }
                                }
                            }
                            AutoMagicId = 0;
                            if (AllowUseMagic(MagicConst.SKILL_ERGUM))
                            {
                                if (Math.Abs(TargetCret.CurrX - CurrX) > 2 || Math.Abs(TargetCret.CurrY - CurrY) > 2)
                                {
                                    return true;
                                }
                            }
                            else if (Math.Abs(TargetCret.CurrX - CurrX) > 1 || Math.Abs(TargetCret.CurrY - CurrY) > 1)
                            {
                                return true;
                            }
                            break;
                        case 7:
                        case 25:
                        case 26:
                            if (Math.Abs(TargetCret.CurrX - CurrX) > 1 || Math.Abs(TargetCret.CurrY - CurrY) > 1)
                            {
                                AutoMagicId = 0;
                                return true;
                            }
                            break;
                        default:
                            if (AllowUseMagic(MagicConst.SKILL_ERGUM))
                            {
                                if (!(Math.Abs(TargetCret.CurrX - CurrX) == 2 && Math.Abs(TargetCret.CurrY - CurrY) == 0 || Math.Abs(TargetCret.CurrX - CurrX) == 1 && Math.Abs(TargetCret.CurrY - CurrY) == 0 || Math.Abs(TargetCret.CurrX - CurrX) == 1 && Math.Abs(TargetCret.CurrY - CurrY) == 1 || Math.Abs(TargetCret.CurrX - CurrX) == 2 && Math.Abs(TargetCret.CurrY - CurrY) == 2 || Math.Abs(TargetCret.CurrX - CurrX) == 0 && Math.Abs(TargetCret.CurrY - CurrY) == 1 || Math.Abs(TargetCret.CurrX - CurrX) == 0 && Math.Abs(TargetCret.CurrY - CurrY) == 2))
                                {
                                    return true;
                                }
                                if (Math.Abs(TargetCret.CurrX - CurrX) == 1 && Math.Abs(TargetCret.CurrY - CurrY) == 2 || Math.Abs(TargetCret.CurrX - CurrX) == 2 && Math.Abs(TargetCret.CurrY - CurrY) == 1)
                                {
                                    return true;
                                }
                            }
                            else if (Math.Abs(TargetCret.CurrX - CurrX) > 1 || Math.Abs(TargetCret.CurrY - CurrY) > 1)
                            {
                                return true;
                            }
                            break;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 走向目标
        /// </summary>
        /// <returns></returns>
        private bool GetGotoXy(IActor baseObject, byte nCode)
        {
            var result = false;
            switch (nCode)
            {
                case 2:// 刺杀位
                    if (CurrX - 2 <= baseObject.CurrX && CurrX + 2 >= baseObject.CurrX && CurrY - 2 <= baseObject.CurrY && CurrY + 2 >= baseObject.CurrY && (CurrX != baseObject.CurrX || CurrY != baseObject.CurrY))
                    {
                        result = true;
                        if (CurrX - 2 == baseObject.CurrX && CurrY == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX - 2);
                            TargetY = CurrY;
                            return true;
                        }
                        if (CurrX + 2 == baseObject.CurrX && CurrY == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX + 2);
                            TargetY = CurrY;
                            return true;
                        }
                        if (CurrX == baseObject.CurrX && CurrY - 2 == baseObject.CurrY)
                        {
                            TargetX = CurrX;
                            TargetY = (short)(CurrY - 2);
                            return true;
                        }
                        if (CurrX == baseObject.CurrX && CurrY + 2 == baseObject.CurrY)
                        {
                            TargetX = CurrX;
                            TargetY = (short)(CurrY + 2);
                            return true;
                        }
                        if (CurrX - 2 == baseObject.CurrX && CurrY - 2 == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX - 2);
                            TargetY = (short)(CurrY - 2);
                            return true;
                        }
                        if (CurrX + 2 == baseObject.CurrX && CurrY - 2 == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX + 2);
                            TargetY = (short)(CurrY - 2);
                            return true;
                        }
                        if (CurrX - 2 == baseObject.CurrX && CurrY + 2 == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX - 2);
                            TargetY = (short)(CurrY + 2);
                            return true;
                        }
                        if (CurrX + 2 == baseObject.CurrX && CurrY + 2 == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX + 2);
                            TargetY = (short)(CurrY + 2);
                            return true;
                        }
                    }
                    break;
                case 3:// 3格
                    if (CurrX - 3 <= baseObject.CurrX && CurrX + 3 >= baseObject.CurrX && CurrY - 3 <= baseObject.CurrY && CurrY + 3 >= baseObject.CurrY && (CurrX != baseObject.CurrX || CurrY != baseObject.CurrY))
                    {
                        result = true;
                        if (CurrX - 3 == baseObject.CurrX && CurrY == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX - 3);
                            TargetY = CurrY;
                            return true;
                        }
                        if (CurrX + 3 == baseObject.CurrX && CurrY == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX + 3);
                            TargetY = CurrY;
                            return true;
                        }
                        if (CurrX == baseObject.CurrX && CurrY - 3 == baseObject.CurrY)
                        {
                            TargetX = CurrX;
                            TargetY = (short)(CurrY - 3);
                            return true;
                        }
                        if (CurrX == baseObject.CurrX && CurrY + 3 == baseObject.CurrY)
                        {
                            TargetX = CurrX;
                            TargetY = (short)(CurrY + 3);
                            return true;
                        }
                        if (CurrX - 3 == baseObject.CurrX && CurrY - 3 == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX - 3);
                            TargetY = (short)(CurrY - 3);
                            return true;
                        }
                        if (CurrX + 3 == baseObject.CurrX && CurrY - 3 == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX + 3);
                            TargetY = (short)(CurrY - 3);
                            return true;
                        }
                        if (CurrX - 3 == baseObject.CurrX && CurrY + 3 == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX - 3);
                            TargetY = (short)(CurrY + 3);
                            return true;
                        }
                        if (CurrX + 3 == baseObject.CurrX && CurrY + 3 == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX + 3);
                            TargetY = (short)(CurrY + 3);
                            return true;
                        }
                    }
                    break;
            }
            return result;
        }

        /// <summary>
        /// 跑到目标坐标
        /// </summary>
        /// <returns></returns>
        private bool RunToTargetXy(short nTargetX, short nTargetY)
        {
            var result = false;
            if (Transparent && HideMode)
            {
                StatusTimeArr[PoisonState.STATETRANSPARENT] = 1;// 隐身,一动就显身
            }
            if (StatusTimeArr[PoisonState.STONE] > 0 && StatusTimeArr[PoisonState.DONTMOVE] != 0 || StatusTimeArr[PoisonState.LOCKSPELL] != 0)// || (m_wStatusTimeArrValue[23] != 0)
            {
                return false; // 麻痹不能跑动 
            }
            if (!IsCanRun) // 禁止跑,则退出
            {
                return false;
            }
            if (HUtil32.GetTickCount() - RunIntervalTick > RunIntervalTime) // 跑步使用单独的变量计数
            {
                var nX = nTargetX;
                var nY = nTargetY;
                var nDir = SystemShare.GetNextDirection(CurrX, CurrY, nX, nY);
                if (!RobotRunTo(nDir, false, nTargetX, nTargetY))
                {
                    result = WalkToTargetXy(nTargetX, nTargetY);
                    if (result)
                    {
                        RunIntervalTick = HUtil32.GetTickCount();
                    }
                }
                else
                {
                    if (Math.Abs(nTargetX - CurrX) <= 1 && Math.Abs(nTargetY - CurrY) <= 1)
                    {
                        result = true;
                        RunIntervalTick = HUtil32.GetTickCount();
                    }
                }
            }
            return result;
        }

        private bool RobotRunTo(byte btDir, bool boFlag, short nDestX, short nDestY)
        {
            const string sExceptionMsg = "[Exception] TBaseObject::RunTo";
            var result = false;
            try
            {
                int nOldX = CurrX;
                int nOldY = CurrY;
                Dir = btDir;
                var canWalk = SystemShare.Config.DiableHumanRun || Permission > 9 && SystemShare.Config.boGMRunAll || SystemShare.Config.boSafeAreaLimited && InSafeZone();
                switch (btDir)
                {
                    case Direction.Up:
                        if (CurrY > 1 && Envir.CanWalkEx(CurrX, CurrY - 1, canWalk) && Envir.CanWalkEx(CurrX, CurrY - 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX, CurrY - 2, true))
                        {
                            CurrY -= 2;
                        }
                        break;
                    case Direction.UpRight:
                        if (CurrX < Envir.Width - 2 && CurrY > 1 && Envir.CanWalkEx(CurrX + 1, CurrY - 1, canWalk) && Envir.CanWalkEx(CurrX + 2, CurrY - 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 2, CurrY - 2, true))
                        {
                            CurrX += 2;
                            CurrY -= 2;
                        }
                        break;
                    case Direction.Right:
                        if (CurrX < Envir.Width - 2 && Envir.CanWalkEx(CurrX + 1, CurrY, canWalk) && Envir.CanWalkEx(CurrX + 2, CurrY, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 2, CurrY, true))
                        {
                            CurrX += 2;
                        }
                        break;
                    case Direction.DownRight:
                        if (CurrX < Envir.Width - 2 && CurrY < Envir.Height - 2 && Envir.CanWalkEx(CurrX + 1, CurrY + 1, canWalk) && Envir.CanWalkEx(CurrX + 2, CurrY + 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 2, CurrY + 2, true))
                        {
                            CurrX += 2;
                            CurrY += 2;
                        }
                        break;
                    case Direction.Down:
                        if (CurrY < Envir.Height - 2 && Envir.CanWalkEx(CurrX, CurrY + 1, canWalk) && Envir.CanWalkEx(CurrX, CurrY + 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX, CurrY + 2, true))
                        {
                            CurrY += 2;
                        }
                        break;
                    case Direction.DownLeft:
                        if (CurrX > 1 && CurrY < Envir.Height - 2 && Envir.CanWalkEx(CurrX - 1, CurrY + 1, canWalk) && Envir.CanWalkEx(CurrX - 2, CurrY + 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 2, CurrY + 2, true))
                        {
                            CurrX -= 2;
                            CurrY += 2;
                        }

                        break;
                    case Direction.Left:
                        if (CurrX > 1 && Envir.CanWalkEx(CurrX - 1, CurrY, canWalk) && Envir.CanWalkEx(CurrX - 2, CurrY, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 2, CurrY, true))
                        {
                            CurrX -= 2;
                        }
                        break;
                    case Direction.UpLeft:
                        if (CurrX > 1 && CurrY > 1 && Envir.CanWalkEx(CurrX - 1, CurrY - 1, canWalk) && Envir.CanWalkEx(CurrX - 2, CurrY - 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 2, CurrY - 2, true))
                        {
                            CurrX -= 2;
                            CurrY -= 2;
                        }
                        break;
                }
                if (CurrX != nOldX || CurrY != nOldY)
                {
                    if (Walk(Messages.RM_RUN))
                    {
                        result = true;
                    }
                    else
                    {
                        CurrX = (short)nOldX;
                        CurrY = (short)nOldY;
                        Envir.MoveToMovingObject(nOldX, nOldY, this, CurrX, CurrX, true);
                    }
                }
            }
            catch
            {
                SystemShare.Logger.Error(sExceptionMsg);
            }
            return result;
        }

        /// <summary>
        /// 走向目标
        /// </summary>
        /// <returns></returns>
        private bool WalkToTargetXy(int nTargetX, int nTargetY)
        {
            var result = false;
            if (Transparent && HideMode)
            {
                StatusTimeArr[PoisonState.STATETRANSPARENT] = 1;// 隐身,一动就显身
            }
            if (StatusTimeArr[PoisonState.STONE] != 0 && StatusTimeArr[PoisonState.DONTMOVE] != 0 || StatusTimeArr[PoisonState.LOCKSPELL] != 0)
            {
                return false;// 麻痹不能跑动
            }
            if (Math.Abs(nTargetX - CurrX) > 1 || Math.Abs(nTargetY - CurrY) > 1)
            {
                if (HUtil32.GetTickCount() - WalkIntervalTick > WalkIntervalTime)
                {
                    var n10 = nTargetX;
                    var n14 = nTargetY;
                    var nDir = Direction.Down;
                    if (n10 > CurrX)
                    {
                        nDir = Direction.Right;
                        if (n14 > CurrY)
                        {
                            nDir = Direction.DownRight;
                        }
                        if (n14 < CurrY)
                        {
                            nDir = Direction.UpRight;
                        }
                    }
                    else
                    {
                        if (n10 < CurrX)
                        {
                            nDir = Direction.Left;
                            if (n14 > CurrY)
                            {
                                nDir = Direction.DownLeft;
                            }
                            if (n14 < CurrY)
                            {
                                nDir = Direction.UpLeft;
                            }
                        }
                        else
                        {
                            if (n14 > CurrY)
                            {
                                nDir = Direction.Down;
                            }
                            else if (n14 < CurrY)
                            {
                                nDir = Direction.Up;
                            }
                        }
                    }
                    int nOldX = CurrX;
                    int nOldY = CurrY;
                    WalkTo(nDir, false);
                    if (Math.Abs(nTargetX - CurrX) <= 1 && Math.Abs(nTargetY - CurrY) <= 1)
                    {
                        result = true;
                        WalkIntervalTick = HUtil32.GetTickCount();
                    }
                    if (!result)
                    {
                        var n20 = SystemShare.RandomNumber.Random(3);
                        for (var i = Direction.Up; i <= Direction.UpLeft; i++)
                        {
                            if (nOldX == CurrX && nOldY == CurrY)
                            {
                                if (n20 != 0)
                                {
                                    nDir++;
                                }
                                else if (nDir > 0)
                                {
                                    nDir -= 1;
                                }
                                else
                                {
                                    nDir = Direction.UpLeft;
                                }
                                if (nDir > Direction.UpLeft)
                                {
                                    nDir = Direction.Up;
                                }
                                WalkTo(nDir, false);
                                if (Math.Abs(nTargetX - CurrX) <= 1 && Math.Abs(nTargetY - CurrY) <= 1)
                                {
                                    result = true;
                                    WalkIntervalTick = HUtil32.GetTickCount();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 走到目标
        /// </summary>
        /// <returns></returns>
        private bool GotoTargetXy(short nTargetX, short nTargetY, int nCode)
        {
            var result = false;
            switch (nCode)
            {
                case 0:// 正常模式
                    if (Math.Abs(CurrX - nTargetX) > 2 || Math.Abs(CurrY - nTargetY) > 2)
                    {
                        if (StatusTimeArr[PoisonState.LOCKRUN] == 0)
                        {
                            result = RunToTargetXy(nTargetX, nTargetY);
                        }
                        else
                        {
                            result = WalkToTargetXy2(nTargetX, nTargetY);// 转向
                        }
                    }
                    else
                    {
                        result = WalkToTargetXy2(nTargetX, nTargetY);// 转向
                    }
                    break;
                case 1:// 躲避模式
                    if (Math.Abs(CurrX - nTargetX) > 1 || Math.Abs(CurrY - nTargetY) > 1)
                    {
                        if (StatusTimeArr[PoisonState.LOCKRUN] == 0)
                        {
                            result = RunToTargetXy(nTargetX, nTargetY);
                        }
                        else
                        {
                            result = WalkToTargetXy2(nTargetX, nTargetY);// 转向
                        }
                    }
                    else
                    {
                        result = WalkToTargetXy2(nTargetX, nTargetY);// 转向
                    }
                    break;
            }
            return result;
        }

        private void SearchMagic()
        {
            AutoMagicId = SelectMagic();
            if (AutoMagicId > 0)
            {
                var userMagic = FindMagic(AutoMagicId);
                if (userMagic != null)
                {
                    BoUseAttackMagic = IsUseAttackMagic();
                }
                else
                {
                    BoUseAttackMagic = false;
                }
            }
            else
            {
                BoUseAttackMagic = false;
            }
        }

        private short SelectMagic()
        {
            short result = 0;
            switch (Job)
            {
                case PlayJob.Warrior:
                    if (AllowUseMagic(MagicConst.SKILL_FIRESWORD) && HUtil32.GetTickCount() - LatestFireHitTick > 9000)// 烈火
                    {
                        FireHitSkill = true;
                        return MagicConst.SKILL_FIRESWORD;
                    }
                    if ((TargetCret.Race == ActorRace.Play || TargetCret.Master != null) && TargetCret.Abil.Level < Abil.Level)// PK时,使用野蛮冲撞 
                    {
                        if (AllowUseMagic(MagicConst.SKILL_MOOTEBO) && CheckMagicInterval(MagicConst.SKILL_MOOTEBO, 10000))// pk时如果对方等级比自己低就每隔一段时间用一次野蛮  
                        {
                            return MagicConst.SKILL_MOOTEBO;
                        }
                    }
                    else
                    {
                        if (AllowUseMagic(MagicConst.SKILL_MOOTEBO) && CheckMagicInterval(MagicConst.SKILL_MOOTEBO, 10000) && TargetCret.Abil.Level < Abil.Level && WAbil.HP <= Math.Round(Abil.MaxHP * 0.85))// 打怪使用 
                        {
                            return MagicConst.SKILL_MOOTEBO;
                        }
                    }
                    if (TargetCret.Master != null)
                    {
                        ExpHitter = TargetCret.Master;
                    }
                    if (CheckTargetXyCount1(CurrX, CurrY, 1) > 1)
                    {
                        switch (SystemShare.RandomNumber.Random(3))// 被怪物包围
                        {
                            case 0:
                                if (AllowUseMagic(41) && CheckMagicInterval(41, 10000) && TargetCret.Abil.Level < Abil.Level && (TargetCret.Race != ActorRace.Play || SystemShare.Config.GroupMbAttackPlayObject) && Math.Abs(TargetCret.CurrX - CurrX) <= 3 && Math.Abs(TargetCret.CurrY - CurrY) <= 3)
                                {
                                    return 41;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_YEDO) && CheckMagicInterval(MagicConst.SKILL_YEDO, 10000))// 攻杀剑术 
                                {
                                    PowerHit = true;// 开启攻杀
                                    return MagicConst.SKILL_YEDO;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_GROUPDEDING) && CheckMagicInterval(MagicConst.SKILL_GROUPDEDING, 10000))
                                {
                                    return MagicConst.SKILL_GROUPDEDING;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_BANWOL))// 半月弯刀
                                {
                                    if (CheckTargetXyCount2(MagicConst.SKILL_BANWOL) > 0)
                                    {
                                        if (!UseHalfMoon)
                                        {
                                            HalfMoonOnOff(true);
                                        }
                                        return MagicConst.SKILL_BANWOL;
                                    }
                                }
                                if (AllowUseMagic(40))// 英雄抱月刀法
                                {
                                    if (!CrsHitkill)
                                    {
                                        SkillCrsOnOff(true);
                                    }
                                    return 40;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_ERGUM))// 英雄刺杀剑术
                                {
                                    if (!UseThrusting)
                                    {
                                        ThrustingOnOff(true);
                                    }
                                    return MagicConst.SKILL_ERGUM;
                                }
                                break;
                            case 1:
                                if (AllowUseMagic(41) && CheckMagicInterval(41, 10000) && TargetCret.Abil.Level < Abil.Level && (TargetCret.Race != ActorRace.Play || SystemShare.Config.GroupMbAttackPlayObject) && Math.Abs(TargetCret.CurrX - CurrX) <= 3 && Math.Abs(TargetCret.CurrY - CurrY) <= 3)
                                {
                                    return 41;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_YEDO) && CheckMagicInterval(MagicConst.SKILL_YEDO, 10000))// 攻杀剑术 
                                {
                                    PowerHit = true;
                                    return MagicConst.SKILL_YEDO;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_GROUPDEDING) && CheckMagicInterval(MagicConst.SKILL_GROUPDEDING, 10000))
                                {
                                    return MagicConst.SKILL_GROUPDEDING;
                                }
                                if (AllowUseMagic(40))// 英雄抱月刀法
                                {
                                    if (!CrsHitkill)
                                    {
                                        SkillCrsOnOff(true);
                                    }
                                    return 40;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_BANWOL))// 半月弯刀
                                {
                                    if (CheckTargetXyCount2(MagicConst.SKILL_BANWOL) > 0)
                                    {
                                        if (!UseHalfMoon)
                                        {
                                            HalfMoonOnOff(true);
                                        }
                                        return MagicConst.SKILL_BANWOL;
                                    }
                                }
                                if (AllowUseMagic(MagicConst.SKILL_ERGUM))// 英雄刺杀剑术
                                {
                                    if (!UseThrusting)
                                    {
                                        ThrustingOnOff(true);
                                    }
                                    return MagicConst.SKILL_ERGUM;
                                }
                                break;
                            case 2:
                                if (AllowUseMagic(41) && CheckMagicInterval(41, 10000) && TargetCret.Abil.Level < Abil.Level && (TargetCret.Race != ActorRace.Play || SystemShare.Config.GroupMbAttackPlayObject) && Math.Abs(TargetCret.CurrX - CurrX) <= 3 && Math.Abs(TargetCret.CurrY - CurrY) <= 3)
                                {
                                    return 41;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_GROUPDEDING) && CheckMagicInterval(MagicConst.SKILL_GROUPDEDING, 10000))
                                {
                                    return MagicConst.SKILL_GROUPDEDING;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_YEDO) && CheckMagicInterval(MagicConst.SKILL_YEDO, 10000))// 攻杀剑术
                                {
                                    PowerHit = true;//  开启攻杀
                                    return MagicConst.SKILL_YEDO;
                                }
                                if (AllowUseMagic(40))// 英雄抱月刀法
                                {
                                    if (!CrsHitkill)
                                    {
                                        SkillCrsOnOff(true);
                                    }
                                    return 40;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_BANWOL))// 半月弯刀
                                {
                                    if (CheckTargetXyCount2(MagicConst.SKILL_BANWOL) > 0)
                                    {
                                        if (!UseHalfMoon)
                                        {
                                            HalfMoonOnOff(true);
                                        }
                                        return MagicConst.SKILL_BANWOL;
                                    }
                                }
                                if (AllowUseMagic(MagicConst.SKILL_ERGUM)) // 英雄刺杀剑术
                                {
                                    if (!UseThrusting)
                                    {
                                        ThrustingOnOff(true);
                                    }
                                    return 12;
                                }
                                break;
                        }
                    }
                    else
                    {
                        if ((TargetCret.Race == ActorRace.Play || TargetCret.Master != null) && CheckTargetXyCount1(CurrX, CurrY, 1) > 1) // PK  身边超过2个目标才使用
                        {
                            if (AllowUseMagic(40) && CheckMagicInterval(40, 3000))// 英雄抱月刀法
                            {
                                if (!CrsHitkill)
                                {
                                    SkillCrsOnOff(true);
                                }
                                return 40;
                            }
                            if (CheckMagicInterval(MagicConst.SKILL_BANWOL, 3000))
                            {
                                if (AllowUseMagic(MagicConst.SKILL_BANWOL))// 半月弯刀
                                {
                                    if (CheckTargetXyCount2(MagicConst.SKILL_BANWOL) > 0)
                                    {
                                        SkillUseTick[25] = HUtil32.GetTickCount();
                                        if (!UseHalfMoon)
                                        {
                                            HalfMoonOnOff(true);
                                        }
                                        return MagicConst.SKILL_BANWOL;
                                    }
                                }
                            }
                        }
                        if (AllowUseMagic(MagicConst.SKILL_YEDO) && CheckMagicInterval(MagicConst.SKILL_YEDO, 10000)) // 少于三个怪用 刺杀剑术
                        {
                            PowerHit = true;// 开启攻杀
                            return MagicConst.SKILL_YEDO;
                        }
                        if (CheckMagicInterval(MagicConst.SKILL_ERGUM, 1000))
                        {
                            if (AllowUseMagic(MagicConst.SKILL_ERGUM))// 英雄刺杀剑术
                            {
                                if (!UseThrusting)
                                {
                                    ThrustingOnOff(true);
                                }
                                return MagicConst.SKILL_ERGUM;
                            }
                        }
                    }
                    // 从高到低使用魔法
                    if (AllowUseMagic(MagicConst.SKILL_FIRESWORD) && (HUtil32.GetTickCount() - LatestFireHitTick) > 9000)// 烈火
                    {
                        FireHitSkill = true;
                        return MagicConst.SKILL_FIRESWORD;
                    }
                    if (AllowUseMagic(40) && CheckMagicInterval(40, 3000) && CheckTargetXyCount1(CurrX, CurrY, 1) > 1)// 英雄抱月刀法
                    {
                        if (!CrsHitkill)
                        {
                            SkillCrsOnOff(true);
                        }
                        return 40;
                    }
                    if (AllowUseMagic(MagicConst.SKILL_GROUPDEDING) && CheckMagicInterval(MagicConst.SKILL_GROUPDEDING, 3000)) // 英雄彻地钉
                    {
                        return MagicConst.SKILL_GROUPDEDING;
                    }
                    if (CheckMagicInterval(MagicConst.SKILL_BANWOL, 3000))
                    {
                        if (AllowUseMagic(MagicConst.SKILL_BANWOL))// 半月弯刀
                        {
                            if (!UseHalfMoon)
                            {
                                HalfMoonOnOff(true);
                            }
                            return MagicConst.SKILL_BANWOL;
                        }
                    }
                    if (CheckMagicInterval(MagicConst.SKILL_ERGUM, 3000))// 英雄刺杀剑术
                    {
                        if (AllowUseMagic(MagicConst.SKILL_ERGUM))
                        {
                            if (!UseThrusting)
                            {
                                ThrustingOnOff(true);
                            }
                            SkillUseTick[12] = HUtil32.GetTickCount();
                            return MagicConst.SKILL_ERGUM;
                        }
                    }
                    if (AllowUseMagic(MagicConst.SKILL_YEDO) && CheckMagicInterval(MagicConst.SKILL_YEDO, 3000))// 攻杀剑术
                    {
                        PowerHit = true;
                        return MagicConst.SKILL_YEDO;
                    }
                    if ((TargetCret.Race == ActorRace.Play || TargetCret.Master != null) && TargetCret.Abil.Level < Abil.Level && WAbil.HP <= Math.Round(WAbil.MaxHP * 0.6))// PK时,使用野蛮冲撞
                    {
                        if (AllowUseMagic(MagicConst.SKILL_MOOTEBO) && CheckMagicInterval(MagicConst.SKILL_MOOTEBO, 3000))
                        {
                            SkillUseTick[27] = HUtil32.GetTickCount();
                            return MagicConst.SKILL_MOOTEBO;
                        }
                    }
                    else
                    {
                        if (AllowUseMagic(MagicConst.SKILL_MOOTEBO) && TargetCret.Abil.Level < Abil.Level && WAbil.HP <= Math.Round(Abil.MaxHP * 0.6) && CheckMagicInterval(MagicConst.SKILL_MOOTEBO, 3000))
                        {
                            return MagicConst.SKILL_MOOTEBO;
                        }
                    }
                    if (AllowUseMagic(41) && CheckMagicInterval(41, 10000) && TargetCret.Abil.Level < Abil.Level && (TargetCret.Race != ActorRace.Play || SystemShare.Config.GroupMbAttackPlayObject) && Math.Abs(TargetCret.CurrX - CurrX) <= 3 && Math.Abs(TargetCret.CurrY - CurrY) <= 3)
                    {
                        return 41;
                    }
                    break;
                case PlayJob.Wizard: // 法师
                    if (StatusTimeArr[PoisonState.BubbleDefenceUP] == 0 && !AbilMagBubbleDefence) // 使用 魔法盾
                    {
                        if (AllowUseMagic(66)) // 4级魔法盾
                        {
                            return 66;
                        }
                        if (AllowUseMagic(MagicConst.SKILL_SHIELD))
                        {
                            return MagicConst.SKILL_SHIELD;
                        }
                    }
                    if ((TargetCret.Race == ActorRace.Play || TargetCret.Master != null) && CheckTargetXyCount3(CurrX, CurrY, 1, 0) > 0 && TargetCret.Abil.Level < Abil.Level)// PK时,旁边有人贴身,使用抗拒火环
                    {
                        if (AllowUseMagic(MagicConst.SKILL_FIREWIND) && CheckMagicInterval(8, 3000))
                        {
                            return MagicConst.SKILL_FIREWIND;
                        }
                    }
                    else
                    {
                        if (AllowUseMagic(MagicConst.SKILL_FIREWIND) && CheckMagicInterval(MagicConst.SKILL_FIREWIND, 3000) && CheckTargetXyCount3(CurrX, CurrY, 1, 0) > 0 && TargetCret.Abil.Level < Abil.Level)// 打怪,怪级低于自己,并且有怪包围自己就用 抗拒火环
                        {
                            return MagicConst.SKILL_FIREWIND;
                        }
                    }
                    if (AllowUseMagic(45) && CheckMagicInterval(45, 3000))
                    {
                        return 45;
                    }
                    if (CheckMagicInterval(10, 5000) && Envir.GetNextPosition(CurrX, CurrY, Dir, 5, ref TargetX, ref TargetY))
                    {
                        if ((TargetCret.Race == ActorRace.Play || TargetCret.Master != null) && GetDirBaseObjectsCount(Dir, 5) > 0 && (Math.Abs(CurrX - TargetCret.CurrX) <= 4 && Math.Abs(CurrY - TargetCret.CurrY) == 0 || Math.Abs(CurrX - TargetCret.CurrX) == 0 && Math.Abs(CurrY - TargetCret.CurrY) <= 4 || Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 2 || Math.Abs(CurrX - TargetCret.CurrX) == 3 && Math.Abs(CurrY - TargetCret.CurrY) == 3 || Math.Abs(CurrX - TargetCret.CurrX) == 4 && Math.Abs(CurrY - TargetCret.CurrY) == 4))
                        {
                            if (AllowUseMagic(MagicConst.SKILL_SHOOTLIGHTEN))
                            {
                                SkillUseTick[MagicConst.SKILL_SHOOTLIGHTEN] = HUtil32.GetTickCount();
                                return MagicConst.SKILL_SHOOTLIGHTEN;
                            }
                            if (AllowUseMagic(MagicConst.SKILL_FIRE))
                            {
                                SkillUseTick[MagicConst.SKILL_SHOOTLIGHTEN] = HUtil32.GetTickCount();
                                return MagicConst.SKILL_FIRE;
                            }
                        }
                        else if (GetDirBaseObjectsCount(Dir, 5) > 1 && (Math.Abs(CurrX - TargetCret.CurrX) <= 4 && Math.Abs(CurrY - TargetCret.CurrY) == 0 || Math.Abs(CurrX - TargetCret.CurrX) == 0 && Math.Abs(CurrY - TargetCret.CurrY) <= 4 || Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 2 || Math.Abs(CurrX - TargetCret.CurrX) == 3 && Math.Abs(CurrY - TargetCret.CurrY) == 3 || Math.Abs(CurrX - TargetCret.CurrX) == 4 && Math.Abs(CurrY - TargetCret.CurrY) == 4))
                        {
                            if (AllowUseMagic(MagicConst.SKILL_SHOOTLIGHTEN))
                            {
                                SkillUseTick[10] = HUtil32.GetTickCount();
                                return MagicConst.SKILL_SHOOTLIGHTEN;
                            }
                            if (AllowUseMagic(MagicConst.SKILL_FIRE))
                            {
                                SkillUseTick[10] = HUtil32.GetTickCount();
                                return MagicConst.SKILL_FIRE;
                            }
                        }
                    }
                    if (AllowUseMagic(MagicConst.SKILL_KILLUNDEAD) && CheckMagicInterval(MagicConst.SKILL_KILLUNDEAD, 10000) && TargetCret.Abil.Level < SystemShare.Config.MagTurnUndeadLevel && TargetCret.LifeAttrib == Grobal2.LA_UNDEAD && TargetCret.Abil.Level < Abil.Level - 1) // 目标为不死系
                    {
                        return MagicConst.SKILL_KILLUNDEAD;
                    }
                    if (CheckTargetXYCount(CurrX, CurrY, 2) > 1)// 被怪物包围
                    {
                        if (AllowUseMagic(MagicConst.SKILL_EARTHFIRE) && CheckMagicInterval(22, 10000))
                        {
                            if (TargetCret.Race != 101 && TargetCret.Race != 102 && TargetCret.Race != 104) // 除祖玛怪,才放火墙
                            {
                                SkillUseTick[MagicConst.SKILL_EARTHFIRE] = HUtil32.GetTickCount();
                                return MagicConst.SKILL_EARTHFIRE;
                            }
                        }
                        // 地狱雷光,只对祖玛(101,102,104)，沃玛(91,92,97)，野猪(81)系列的
                        // 遇到祖玛的怪应该多用地狱雷光，夹杂雷电术，少用冰咆哮
                        if (new ArrayList(new byte[] { 91, 92, 97, 101, 102, 104 }).Contains(TargetCret.Race))
                        {
                            if (AllowUseMagic(MagicConst.SKILL_LIGHTFLOWER) && CheckMagicInterval(24, 4000) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                            {
                                return MagicConst.SKILL_LIGHTFLOWER;
                            }
                            if (AllowUseMagic(MagicConst.SKILL_LIGHTENINGFOUR))// 四级雷电术
                            {
                                return MagicConst.SKILL_LIGHTENINGFOUR;
                            }
                            if (AllowUseMagic(MagicConst.SKILL_LIGHTENING))
                            {
                                return MagicConst.SKILL_LIGHTENING;
                            }
                            if (AllowUseMagic(MagicConst.SKILL_SNOWWIND) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 2) > 2)
                            {
                                return MagicConst.SKILL_SNOWWIND;
                            }
                            if (CheckMagicInterval(58, 1500) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                            {
                                if (AllowUseMagic(92))// 四级流星火雨
                                {
                                    SkillUseTick[58] = HUtil32.GetTickCount();
                                    return 92;
                                }
                                if (AllowUseMagic(58))// 流星火雨
                                {
                                    SkillUseTick[58] = HUtil32.GetTickCount();
                                    return 58;
                                }
                            }
                        }
                        switch (SystemShare.RandomNumber.Random(4))// 随机选择魔法
                        {
                            case 0: // 火球术,大火球,雷电术,爆裂火焰,英雄冰咆哮,流星火雨 从高到低选择
                                if (AllowUseMagic(92) && CheckMagicInterval(58, 1500) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    return 92;// 四级流星火雨
                                }
                                if (AllowUseMagic(58) && CheckMagicInterval(58, 1500) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    return 58;// 流星火雨
                                }
                                if (AllowUseMagic(MagicConst.SKILL_SNOWWIND) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)
                                {
                                    return MagicConst.SKILL_SNOWWIND;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBOOM) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)
                                {
                                    return MagicConst.SKILL_FIREBOOM;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_LIGHTENINGFOUR))// 四级雷电术
                                {
                                    return MagicConst.SKILL_LIGHTENINGFOUR;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_LIGHTENING))
                                {
                                    return MagicConst.SKILL_LIGHTENING;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBALL2))
                                {
                                    return MagicConst.SKILL_FIREBALL2;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBALL))
                                {
                                    return MagicConst.SKILL_FIREBALL;
                                }
                                if (AllowUseMagic(37))// 英雄群体雷电
                                {
                                    return 37;
                                }
                                if (AllowUseMagic(47))// 火龙焰
                                {
                                    return 47;
                                }
                                if (AllowUseMagic(44))// 寒冰掌
                                {
                                    return 44;
                                }
                                break;
                            case 1:
                                if (AllowUseMagic(37))
                                {
                                    return 37;
                                }
                                if (AllowUseMagic(47))
                                {
                                    return 47;
                                }
                                if (AllowUseMagic(44))// 寒冰掌
                                {
                                    return 44;
                                }
                                if (AllowUseMagic(92) && CheckMagicInterval(58, 1500) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    SkillUseTick[58] = HUtil32.GetTickCount();
                                    return 92;// 四级流星火雨
                                }
                                if (AllowUseMagic(58) && CheckMagicInterval(58, 1500) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    SkillUseTick[58] = HUtil32.GetTickCount();
                                    return 58;// 流星火雨
                                }
                                if (AllowUseMagic(MagicConst.SKILL_SNOWWIND) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)// 火球术,大火球,地狱火,爆裂火焰,冰咆哮  从高到低选择
                                {
                                    return MagicConst.SKILL_SNOWWIND;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBOOM) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)
                                {
                                    return MagicConst.SKILL_FIREBOOM;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_LIGHTENINGFOUR))// 四级雷电术
                                {
                                    return MagicConst.SKILL_LIGHTENINGFOUR;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_LIGHTENING))
                                {
                                    return MagicConst.SKILL_LIGHTENING;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBALL2))
                                {
                                    return MagicConst.SKILL_FIREBALL2;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBALL))
                                {
                                    return MagicConst.SKILL_FIREBALL;
                                }
                                break;
                            case 2:
                                if (AllowUseMagic(47))
                                {
                                    return 47;
                                }
                                if (AllowUseMagic(44))// 寒冰掌
                                {
                                    return 44;
                                }
                                if (AllowUseMagic(92) && CheckMagicInterval(58, 1500) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    SkillUseTick[58] = HUtil32.GetTickCount();
                                    return 92;// 四级流星火雨
                                }
                                if (AllowUseMagic(58) && CheckMagicInterval(58, 1500) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    SkillUseTick[58] = HUtil32.GetTickCount();
                                    return 58;// 流星火雨
                                }
                                if (AllowUseMagic(MagicConst.SKILL_SNOWWIND) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)
                                {
                                    // 火球术,大火球,地狱火,爆裂火焰 从高到低选择
                                    return MagicConst.SKILL_SNOWWIND;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBOOM) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)
                                {
                                    return MagicConst.SKILL_FIREBOOM;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_LIGHTENINGFOUR))// 四级雷电术
                                {
                                    return MagicConst.SKILL_LIGHTENINGFOUR;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_LIGHTENING))
                                {
                                    return MagicConst.SKILL_LIGHTENING;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBALL2))
                                {
                                    return MagicConst.SKILL_FIREBALL2;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBALL))
                                {
                                    return MagicConst.SKILL_FIREBALL;
                                }
                                if (AllowUseMagic(37))
                                {
                                    return 37;
                                }
                                break;
                            case 3:
                                if (AllowUseMagic(44))// 寒冰掌
                                {
                                    return 44;
                                }
                                if (AllowUseMagic(92) && CheckMagicInterval(58, 1500) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    SkillUseTick[58] = HUtil32.GetTickCount();
                                    return 92;// 四级流星火雨
                                }
                                if (AllowUseMagic(58) && CheckMagicInterval(58, 1500) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    SkillUseTick[58] = HUtil32.GetTickCount();
                                    return 58;// 流星火雨
                                }
                                if (AllowUseMagic(MagicConst.SKILL_SNOWWIND) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)// 火球术,大火球,地狱火,爆裂火焰 从高到低选择
                                {
                                    return MagicConst.SKILL_SNOWWIND;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBOOM) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)
                                {
                                    return MagicConst.SKILL_FIREBOOM;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_LIGHTENINGFOUR))
                                {
                                    return MagicConst.SKILL_LIGHTENINGFOUR;// 四级雷电术
                                }
                                if (AllowUseMagic(MagicConst.SKILL_LIGHTENING))
                                {
                                    return MagicConst.SKILL_LIGHTENING;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBALL2))
                                {
                                    return MagicConst.SKILL_FIREBALL2;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBALL))
                                {
                                    return MagicConst.SKILL_FIREBALL;
                                }
                                if (AllowUseMagic(37))
                                {
                                    return 37;
                                }
                                if (AllowUseMagic(47))
                                {
                                    return 47;
                                }
                                break;
                        }
                    }
                    else
                    {
                        // 只有一个怪时所用的魔法
                        if (AllowUseMagic(MagicConst.SKILL_EARTHFIRE) && CheckMagicInterval(22, 10000))
                        {
                            if (TargetCret.Race != 101 && TargetCret.Race != 102 && TargetCret.Race != 104)// 除祖玛怪,才放火墙
                            {
                                SkillUseTick[22] = HUtil32.GetTickCount();
                                return MagicConst.SKILL_EARTHFIRE;
                            }
                        }
                        switch (SystemShare.RandomNumber.Random(4))// 随机选择魔法
                        {
                            case 0:
                                if (AllowUseMagic(MagicConst.SKILL_LIGHTENINGFOUR))// 四级雷电术
                                {
                                    return MagicConst.SKILL_LIGHTENINGFOUR;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_LIGHTENING))
                                {
                                    return MagicConst.SKILL_LIGHTENING;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_SNOWWIND))
                                {
                                    return MagicConst.SKILL_SNOWWIND;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBOOM))// 火球术,大火球,地狱火,爆裂火焰 从高到低选择
                                {
                                    return MagicConst.SKILL_FIREBOOM;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBALL2))
                                {
                                    return MagicConst.SKILL_FIREBALL2;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBALL))
                                {
                                    return MagicConst.SKILL_FIREBALL;
                                }
                                if (AllowUseMagic(37))
                                {
                                    return 37;
                                }
                                if (AllowUseMagic(47))
                                {
                                    return 47;
                                }
                                if (AllowUseMagic(44))
                                {
                                    return 44;
                                }
                                break;
                            case 1:
                                if (AllowUseMagic(37))
                                {
                                    return 37;
                                }
                                if (AllowUseMagic(47))
                                {
                                    return 47;
                                }
                                if (AllowUseMagic(44))
                                {
                                    return 44;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_LIGHTENINGFOUR))// 四级雷电术
                                {
                                    return MagicConst.SKILL_LIGHTENINGFOUR;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_LIGHTENING))
                                {
                                    return MagicConst.SKILL_LIGHTENING;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_SNOWWIND))
                                {
                                    return MagicConst.SKILL_SNOWWIND;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBOOM))
                                {
                                    return MagicConst.SKILL_FIREBOOM;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBALL2))
                                {
                                    return MagicConst.SKILL_FIREBALL2;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBALL))
                                {
                                    return MagicConst.SKILL_FIREBALL;
                                }
                                break;
                            case 2:
                                if (AllowUseMagic(47))
                                {
                                    return 47;
                                }
                                if (AllowUseMagic(44))
                                {
                                    return 44;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_LIGHTENINGFOUR))// 四级雷电术
                                {
                                    return MagicConst.SKILL_LIGHTENINGFOUR;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_LIGHTENING))
                                {
                                    return MagicConst.SKILL_LIGHTENING;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_SNOWWIND))
                                {
                                    return MagicConst.SKILL_SNOWWIND;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBOOM))
                                {
                                    return MagicConst.SKILL_FIREBOOM;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBALL2))
                                {
                                    return MagicConst.SKILL_FIREBALL2;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBALL))
                                {
                                    return MagicConst.SKILL_FIREBALL;
                                }
                                if (AllowUseMagic(37))
                                {
                                    return 37;
                                }
                                break;
                            case 3:
                                if (AllowUseMagic(44))
                                {
                                    return 44;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_LIGHTENINGFOUR))// 四级雷电术
                                {
                                    return MagicConst.SKILL_LIGHTENINGFOUR;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_LIGHTENING))
                                {
                                    return MagicConst.SKILL_LIGHTENING;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_SNOWWIND))
                                {
                                    return MagicConst.SKILL_SNOWWIND;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBOOM))
                                {
                                    return MagicConst.SKILL_FIREBOOM;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBALL2))
                                {
                                    return MagicConst.SKILL_FIREBALL2;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIREBALL))
                                {
                                    return MagicConst.SKILL_FIREBALL;
                                }
                                if (AllowUseMagic(37))
                                {
                                    return 37;
                                }
                                if (AllowUseMagic(47))
                                {
                                    return 47;
                                }
                                break;
                        }
                    }
                    // 从高到低使用魔法 
                    if (CheckMagicInterval(58, 1500))
                    {
                        if (AllowUseMagic(92))// 四级流星火雨
                        {
                            SkillUseTick[58] = HUtil32.GetTickCount();
                            return 92;
                        }
                        if (AllowUseMagic(58)) // 流星火雨
                        {
                            SkillUseTick[58] = HUtil32.GetTickCount();
                            return 58;
                        }
                    }
                    if (AllowUseMagic(47))// 火龙焰
                    {
                        return 47;
                    }
                    if (AllowUseMagic(45))// 英雄灭天火
                    {
                        return 45;
                    }
                    if (AllowUseMagic(44))
                    {
                        return 44;
                    }
                    if (AllowUseMagic(37))// 英雄群体雷电
                    {
                        return 37;
                    }
                    if (AllowUseMagic(MagicConst.SKILL_SNOWWIND))// 英雄冰咆哮
                    {
                        return MagicConst.SKILL_SNOWWIND;
                    }
                    if (AllowUseMagic(32) && TargetCret.Abil.Level < SystemShare.Config.MagTurnUndeadLevel && TargetCret.LifeAttrib == Grobal2.LA_UNDEAD && TargetCret.Abil.Level < Abil.Level - 1)// 目标为不死系
                    {
                        return 32;// 圣言术
                    }
                    if (AllowUseMagic(MagicConst.SKILL_LIGHTFLOWER) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                    {
                        return MagicConst.SKILL_LIGHTFLOWER;
                    }
                    if (AllowUseMagic(MagicConst.SKILL_FIREBOOM))
                    {
                        return MagicConst.SKILL_FIREBOOM;
                    }
                    if (AllowUseMagic(MagicConst.SKILL_LIGHTENINGFOUR))// 四级雷电术
                    {
                        return MagicConst.SKILL_LIGHTENINGFOUR;
                    }
                    if (AllowUseMagic(MagicConst.SKILL_LIGHTENING))
                    {
                        return MagicConst.SKILL_LIGHTENING;
                    }
                    if (AllowUseMagic(MagicConst.SKILL_SHOOTLIGHTEN) && Envir.GetNextPosition(CurrX, CurrY, Dir, 5, ref TargetX, ref TargetY) && (Math.Abs(CurrX - TargetCret.CurrX) <= 4 && Math.Abs(CurrY - TargetCret.CurrY) == 0 || Math.Abs(CurrX - TargetCret.CurrX) == 0 && Math.Abs(CurrY - TargetCret.CurrY) <= 4 || Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 2 || Math.Abs(CurrX - TargetCret.CurrX) == 3 && Math.Abs(CurrY - TargetCret.CurrY) == 3 || Math.Abs(CurrX - TargetCret.CurrX) == 4 && Math.Abs(CurrY - TargetCret.CurrY) == 4))
                    {
                        return MagicConst.SKILL_SHOOTLIGHTEN;
                    }
                    if (AllowUseMagic(MagicConst.SKILL_FIRE) && Envir.GetNextPosition(CurrX, CurrY, Dir, 5, ref TargetX, ref TargetY) && (Math.Abs(CurrX - TargetCret.CurrX) <= 4 && Math.Abs(CurrY - TargetCret.CurrY) == 0 || Math.Abs(CurrX - TargetCret.CurrX) == 0 && Math.Abs(CurrY - TargetCret.CurrY) <= 4 || Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 2 || Math.Abs(CurrX - TargetCret.CurrX) == 3 && Math.Abs(CurrY - TargetCret.CurrY) == 3 || Math.Abs(CurrX - TargetCret.CurrX) == 4 && Math.Abs(CurrY - TargetCret.CurrY) == 4))
                    {
                        return MagicConst.SKILL_FIRE;
                    }
                    if (AllowUseMagic(MagicConst.SKILL_FIREBALL2))
                    {
                        return MagicConst.SKILL_FIREBALL2;
                    }
                    if (AllowUseMagic(MagicConst.SKILL_FIREBALL))
                    {
                        return MagicConst.SKILL_FIREBALL;
                    }
                    if (AllowUseMagic(MagicConst.SKILL_EARTHFIRE))
                    {
                        if (TargetCret.Race != 101 && TargetCret.Race != 102 && TargetCret.Race != 104)// 除祖玛怪,才放火墙
                        {
                            return MagicConst.SKILL_EARTHFIRE;
                        }
                    }
                    break;
                case PlayJob.Taoist:// 道士
                    if (SlaveList.Count == 0 && CheckHeroAmulet(1, 5) && CheckMagicInterval(17, 3000) && (AllowUseMagic(72) || AllowUseMagic(30) || AllowUseMagic(17)) && Abil.MP > 20)
                    {
                        if (AllowUseMagic(104)) // 召唤火灵
                        {
                            result = 104;
                        }
                        else if (AllowUseMagic(72)) // 召唤月灵
                        {
                            result = 72;
                        }
                        else if (AllowUseMagic(MagicConst.SKILL_SINSU))// 召唤神兽
                        {
                            result = MagicConst.SKILL_SINSU;
                        }
                        else if (AllowUseMagic(MagicConst.SKILL_SKELLETON)) // 召唤骷髅
                        {
                            result = MagicConst.SKILL_SKELLETON;
                        }
                        return result;
                    }
                    if (StatusTimeArr[PoisonState.BubbleDefenceUP] == 0 && !AbilMagBubbleDefence)
                    {
                        if (AllowUseMagic(73)) // 道力盾
                        {
                            return 73;
                        }
                    }
                    if ((TargetCret.Race == ActorRace.Play || TargetCret.Master != null) && CheckTargetXyCount3(CurrX, CurrY, 1, 0) > 0 && TargetCret.Abil.Level <= Abil.Level)// PK时,旁边有人贴身,使用气功波
                    {
                        if (AllowUseMagic(48) && CheckMagicInterval(48, 3000))
                        {
                            return 48;
                        }
                    }
                    else
                    {
                        if (AllowUseMagic(48) && CheckMagicInterval(48, 5000) && CheckTargetXyCount3(CurrX, CurrY, 1, 0) > 0 && TargetCret.Abil.Level <= Abil.Level)// 打怪,怪级低于自己,并且有怪包围自己就用 气功波
                        {
                            return 48;
                        }
                    }
                    // 绿毒
                    if (TargetCret.StatusTimeArr[PoisonState.DECHEALTH] == 0 && GetUserItemList(2, 1) >= 0 && (SystemShare.Config.btHeroSkillMode || !SystemShare.Config.btHeroSkillMode && TargetCret.WAbil.HP >= 700
                                                                                                                                                     || TargetCret.Race == ActorRace.Play) && (Math.Abs(TargetCret.CurrX - CurrX) < 7 || Math.Abs(TargetCret.CurrY - CurrY) < 7)
                        && !SystemShare.RobotPlayRaceMap.Contains(TargetCret.Race))// 对于血量超过800的怪用 不毒城墙
                    {
                        NAmuletIndx = 0;
                        switch (SystemShare.RandomNumber.Random(2))
                        {
                            case 0:
                                if (AllowUseMagic(38) && CheckMagicInterval(38, 1000))
                                {
                                    if (Envir != null)// 判断地图是否禁用
                                    {
                                        if (Envirnoment.AllowMagics(MagicConst.SKILL_GROUPAMYOUNSUL, 1))
                                        {
                                            SkillUseTick[38] = HUtil32.GetTickCount();
                                            return MagicConst.SKILL_GROUPAMYOUNSUL;
                                        }
                                    }
                                }
                                else if (CheckMagicInterval(MagicConst.SKILL_AMYOUNSUL, 1000))
                                {
                                    if (AllowUseMagic(MagicConst.SKILL_AMYOUNSUL))
                                    {
                                        if (Envir != null)
                                        {
                                            if (Envirnoment.AllowMagics(MagicConst.SKILL_AMYOUNSUL, 1))// 判断地图是否禁用
                                            {
                                                SkillUseTick[6] = HUtil32.GetTickCount();
                                                return MagicConst.SKILL_AMYOUNSUL;
                                            }
                                        }
                                    }
                                }
                                break;
                            case 1:
                                if (CheckMagicInterval(MagicConst.SKILL_AMYOUNSUL, 1000))
                                {
                                    if (AllowUseMagic(MagicConst.SKILL_AMYOUNSUL))
                                    {
                                        if (Envir != null)
                                        {
                                            if (Envirnoment.AllowMagics(MagicConst.SKILL_AMYOUNSUL, 1))// 判断地图是否禁用
                                            {
                                                SkillUseTick[6] = HUtil32.GetTickCount();
                                                return MagicConst.SKILL_AMYOUNSUL;
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    if (TargetCret.StatusTimeArr[PoisonState.DAMAGEARMOR] == 0 && GetUserItemList(2, 2) >= 0 && (SystemShare.Config.btHeroSkillMode || !SystemShare.Config.btHeroSkillMode && TargetCret.WAbil.HP >= 700
                            || TargetCret.Race == ActorRace.Play) && (Math.Abs(TargetCret.CurrX - CurrX) < 7 || Math.Abs(TargetCret.CurrY - CurrY) < 7)
                        && !SystemShare.RobotPlayRaceMap.Contains(TargetCret.Race))// 对于血量超过700的怪用 不毒城墙
                    {
                        NAmuletIndx = 0;
                        switch (SystemShare.RandomNumber.Random(2))
                        {
                            case 0:
                                if (AllowUseMagic(38) && CheckMagicInterval(38, 1000))
                                {
                                    if (Envir != null)
                                    {
                                        if (Envirnoment.AllowMagics(MagicConst.SKILL_GROUPAMYOUNSUL, 1))// 判断地图是否禁用
                                        {
                                            SkillUseTick[38] = HUtil32.GetTickCount();
                                            return MagicConst.SKILL_GROUPAMYOUNSUL;
                                        }
                                    }
                                }
                                else if (CheckMagicInterval(MagicConst.SKILL_AMYOUNSUL, 1000))
                                {
                                    if (AllowUseMagic(MagicConst.SKILL_AMYOUNSUL))
                                    {
                                        if (Envir != null)
                                        {
                                            if (Envirnoment.AllowMagics(MagicConst.SKILL_AMYOUNSUL, 1))// 判断地图是否禁用
                                            {
                                                SkillUseTick[6] = HUtil32.GetTickCount();
                                                return MagicConst.SKILL_AMYOUNSUL;
                                            }
                                        }
                                    }
                                }
                                break;
                            case 1:
                                if (CheckMagicInterval(MagicConst.SKILL_AMYOUNSUL, 1000))
                                {
                                    if (AllowUseMagic(MagicConst.SKILL_AMYOUNSUL))
                                    {
                                        if (Envir != null)
                                        {
                                            if (Envirnoment.AllowMagics(MagicConst.SKILL_AMYOUNSUL, 1)) // 判断地图是否禁用
                                            {
                                                SkillUseTick[6] = HUtil32.GetTickCount();
                                                return MagicConst.SKILL_AMYOUNSUL;
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    if (AllowUseMagic(51) && CheckMagicInterval(51, 5000))// 英雄飓风破 
                    {
                        return 51;
                    }
                    if (CheckHeroAmulet(1, 1))
                    {
                        switch (SystemShare.RandomNumber.Random(3))// 使用符的魔法
                        {
                            case 0:
                                if (AllowUseMagic(94))// 英雄四级噬血术
                                {
                                    return 94;
                                }
                                if (AllowUseMagic(59)) // 英雄噬血术
                                {
                                    return 59;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIRECHARM) && CheckMagicInterval(13, 3000))
                                {
                                    return MagicConst.SKILL_FIRECHARM;
                                }
                                if (AllowUseMagic(52)) // 诅咒术
                                {
                                    if (TargetCret.Race == ActorRace.Play && ((IPlayerActor)TargetCret).ExtraAbil[(byte)((IPlayerActor)TargetCret).Job + 6] == 0)
                                    {
                                        return 52;// 英雄诅咒术
                                    }
                                }
                                break;
                            case 1:
                                if (AllowUseMagic(52)) // 诅咒术
                                {
                                    if (TargetCret.Race == ActorRace.Play && ((IPlayerActor)TargetCret).ExtraAbil[(byte)((IPlayerActor)TargetCret).Job + 6] == 0)
                                    {
                                        return 52;// 英雄诅咒术
                                    }
                                }
                                if (AllowUseMagic(94))// 英雄四级噬血术
                                {
                                    return 94;
                                }
                                if (AllowUseMagic(59))// 英雄噬血术
                                {
                                    return 59;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_FIRECHARM) && CheckMagicInterval(13, 3000))
                                {
                                    return MagicConst.SKILL_FIRECHARM;
                                }
                                break;
                            case 2:
                                if (AllowUseMagic(MagicConst.SKILL_FIRECHARM) && CheckMagicInterval(13, 3000))
                                {
                                    return MagicConst.SKILL_FIRECHARM;
                                }
                                if (AllowUseMagic(94))
                                {
                                    return 94;// 英雄四级噬血术
                                }
                                if (AllowUseMagic(59))
                                {
                                    return 59;// 英雄噬血术
                                }
                                if (AllowUseMagic(52))// 诅咒术
                                {
                                    if (TargetCret.Race == ActorRace.Play && ((IPlayerActor)TargetCret).ExtraAbil[(byte)((IPlayerActor)TargetCret).Job + 6] == 0)
                                    {
                                        return 52;
                                    }
                                }
                                break;
                        }
                        // 技能从高到低选择 
                        if (AllowUseMagic(94))
                        {
                            return 94;// 英雄四级噬血术
                        }
                        if (AllowUseMagic(59))// 英雄噬血术
                        {
                            return 59;
                        }
                        if (AllowUseMagic(54)) // 英雄骷髅咒
                        {
                            return 54;
                        }
                        if (AllowUseMagic(53))// 英雄血咒
                        {
                            return 53;
                        }
                        if (AllowUseMagic(51))// 英雄飓风破
                        {
                            return 51;
                        }
                        if (AllowUseMagic(13))// 英雄灵魂火符
                        {
                            return 13;
                        }
                        if (AllowUseMagic(52))// 诅咒术
                        {
                            if (TargetCret.Race == ActorRace.Play && ((IPlayerActor)TargetCret).ExtraAbil[(byte)((IPlayerActor)TargetCret).Job + 6] == 0)
                            {
                                return 52;
                            }
                        }
                    }
                    break;
            }
            return result;
        }

        private bool CheckMagicInterval(ushort magicId, int interval)
        {
            if (HUtil32.GetTickCount() - SkillUseTick[magicId] > interval)
            {
                SkillUseTick[magicId] = HUtil32.GetTickCount();
                return true;
            }
            return false;
        }

        // 战士判断使用
        private int CheckTargetXyCount1(int nX, int nY, int nRange)
        {
            var result = 0;
            if (VisibleActors.Count > 0)
            {
                for (var i = 0; i < VisibleActors.Count; i++)
                {
                    var baseObject = VisibleActors[i].BaseObject;
                    if (baseObject != null)
                    {
                        if (!baseObject.Death)
                        {
                            if (IsProperTarget(baseObject) && (!baseObject.HideMode || CoolEye))
                            {
                                if (Math.Abs(nX - baseObject.CurrX) <= nRange && Math.Abs(nY - baseObject.CurrY) <= nRange)
                                {
                                    result++;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        // 半月弯刀判断目标函数
        private int CheckTargetXyCount2(short nMode)
        {
            var result = 0;
            var nC = 0;
            var n10 = 0;
            short nX = 0;
            short nY = 0;
            if (VisibleActors.Count > 0)
            {
                for (var i = 0; i < VisibleActors.Count; i++)
                {
                    switch (nMode)
                    {
                        case MagicConst.SKILL_BANWOL:
                            n10 = (Dir + SystemShare.Config.WideAttack[nC]) % 8;
                            break;
                    }
                    if (Envir.GetNextPosition(CurrX, CurrY, (byte)n10, 1, ref nX, ref nY))
                    {
                        var baseObject = Envir.GetMovingObject(nX, nY, true);
                        if (baseObject != null)
                        {
                            if (!baseObject.Death)
                            {
                                if (IsProperTarget(baseObject) && (!baseObject.HideMode || CoolEye))
                                {
                                    result++;
                                }
                            }
                        }
                    }
                    nC++;
                    switch (nMode)
                    {
                        case MagicConst.SKILL_BANWOL:
                            if (nC >= 3)
                            {
                                break;
                            }
                            break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 气功波，抗拒火环使用
        /// </summary>
        /// <returns></returns>
        private int CheckTargetXyCount3(int nX, int nY, int nRange, int nCount)
        {
            var result = 0;
            if (VisibleActors.Count > 0)
            {
                for (var i = 0; i < VisibleActors.Count; i++)
                {
                    var baseObject = VisibleActors[i].BaseObject;
                    if (baseObject != null)
                    {
                        if (!baseObject.Death)
                        {
                            if (IsProperTarget(baseObject) && (!baseObject.HideMode || CoolEye))
                            {
                                if (Math.Abs(nX - baseObject.CurrX) <= nRange && Math.Abs(nY - baseObject.CurrY) <= nRange)
                                {
                                    result++;
                                    if (result > nCount)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        // 参数 nType 为指定类型 1 为护身符 2 为毒药    nCount 为持久,即数量
        private bool CheckHeroAmulet(int nType, int nCount)
        {
            var result = false;
            StdItem amuletStdItem;
            try
            {
                if (UseItems[ItemLocation.ArmRingl].Index > 0)
                {
                    amuletStdItem = SystemShare.ItemSystem.GetStdItem(UseItems[ItemLocation.ArmRingl].Index);
                    if (amuletStdItem != null)
                    {
                        if (amuletStdItem.StdMode == 25)
                        {
                            switch (nType)
                            {
                                case 1:
                                    if (amuletStdItem.Shape == 5 && Math.Round(Convert.ToDouble(UseItems[ItemLocation.ArmRingl].Dura / 100)) >= nCount)
                                    {
                                        result = true;
                                        return result;
                                    }
                                    break;
                                case 2:
                                    if (amuletStdItem.Shape <= 2 && Math.Round(Convert.ToDouble(UseItems[ItemLocation.ArmRingl].Dura / 100)) >= nCount)
                                    {
                                        result = true;
                                        return result;
                                    }
                                    break;
                            }
                        }
                    }
                }
                if (UseItems[ItemLocation.Bujuk] != null && UseItems[ItemLocation.Bujuk].Index > 0)
                {
                    amuletStdItem = SystemShare.ItemSystem.GetStdItem(UseItems[ItemLocation.Bujuk].Index);
                    if (amuletStdItem != null)
                    {
                        if (amuletStdItem.StdMode == 25)
                        {
                            switch (nType)
                            {
                                case 1: // 符
                                    if (amuletStdItem.Shape == 5 && Math.Round(Convert.ToDouble(UseItems[ItemLocation.Bujuk].Dura / 100)) >= nCount)
                                    {
                                        result = true;
                                        return result;
                                    }
                                    break;
                                case 2: // 毒
                                    if (amuletStdItem.Shape <= 2 && Math.Round(Convert.ToDouble(UseItems[ItemLocation.Bujuk].Dura / 100)) >= nCount)
                                    {
                                        result = true;
                                        return result;
                                    }
                                    break;
                            }
                        }
                    }
                }
                // 检测人物包裹是否存在毒,护身符
                if (ItemList.Count > 0)
                {
                    for (var i = 0; i < ItemList.Count; i++)
                    {
                        // 人物包裹不为空
                        var userItem = ItemList[i];
                        if (userItem != null)
                        {
                            amuletStdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                            if (amuletStdItem != null)
                            {
                                if (amuletStdItem.StdMode == 25)
                                {
                                    switch (nType)
                                    {
                                        case 1:
                                            if (amuletStdItem.Shape == 5 && HUtil32.Round(userItem.Dura / 100.0) >= nCount)
                                            {
                                                result = true;
                                                return result;
                                            }
                                            break;
                                        case 2:
                                            if (amuletStdItem.Shape <= 2 && HUtil32.Round(userItem.Dura / 100.0) >= nCount)
                                            {
                                                result = true;
                                                return result;
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                SystemShare.Logger.Error("RoboPlayObject.CheckHeroAmulet");
            }
            return result;
        }

        private static int GetDirBaseObjectsCount(int mBtDirection, int rang)
        {
            return 0;
        }
    }
}