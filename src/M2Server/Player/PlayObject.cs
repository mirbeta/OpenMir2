using M2Server.Actor;
using System.Collections;
using System.Runtime.CompilerServices;
using SystemModule;
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Events;
using SystemModule.Packets.ClientPackets;
using SystemModule.Packets.ServerPackets;

namespace M2Server.Player
{
    public partial class PlayObject : CharacterObject, IPlayerActor
    {
        /// <summary>
        /// 性别
        /// </summary>
        public PlayGender Gender { get; set; }
        /// <summary>
        /// 人物的头发
        /// </summary>
        public byte Hair { get; set; }
        /// <summary>
        /// 人物的职业 (0:战士 1：法师 2:道士)
        /// </summary>
        public PlayJob Job { get; set; }
        /// <summary>
        /// 登录帐号名
        /// </summary>
        public string UserAccount { get; set; }
        /// <summary>
        /// 人物IP地址
        /// </summary>
        public string LoginIpAddr { get; set; }
        public string LoginIpLocal { get; set; }
        /// <summary>
        /// 账号过期
        /// </summary>
        public bool AccountExpired { get; set; }
        /// <summary>
        /// 账号游戏点数检查时间
        /// </summary>
        public int AccountExpiredTick { get; set; }
        public long ExpireTime { get; set; }
        public int ExpireCount { get; set; }
        public int QueryExpireTick { get; set; }
        /// <summary>
        /// 权限等级
        /// </summary>
        public byte Permission { get; set; }
        /// <summary>
        /// 人物的幸运值
        /// </summary>
        public byte Luck { get; set; }
        /// <summary>
        /// 人物身上最多可带金币数
        /// </summary>
        public int GoldMax { get; set; }
        /// <summary>
        /// 行会占争范围
        /// </summary>
        public bool GuildWarArea { get; set; }
        /// <summary>
        /// 允许行会传送
        /// </summary>
        public bool AllowGuildReCall { get; set; }
        /// <summary>
        /// 允许私聊
        /// </summary>
        public bool HearWhisper { get; set; }
        /// <summary>
        /// 允许群聊
        /// </summary>
        public bool BanShout { get; set; }
        /// <summary>
        /// 拒绝行会聊天
        /// </summary>
        public bool BanGuildChat { get; set; }
        /// <summary>
        /// 是否允许交易
        /// </summary>
        public bool AllowDeal { get; set; }
        /// <summary>
        /// 检查重叠人物使用
        /// </summary>
        public bool BoDuplication { get; set; }
        /// <summary>
        /// 检查重叠人物间隔
        /// </summary>
        public int DupStartTick { get; set; }
        /// <summary>
        /// 是否用了神水
        /// </summary>
        public bool UserUnLockDurg { get; set; }
        /// <summary>
        /// 允许组队
        /// </summary>
        public bool AllowGroup { get; set; }
        /// <summary>
        /// 允许加入行会
        /// </summary>        
        public bool AllowGuild { get; set; }
        /// <summary>
        /// 交易对象
        /// </summary>
        public IPlayerActor DealCreat { get; set; }
        /// <summary>
        /// 正在交易
        /// </summary>
        public bool Dealing { get; set; }
        /// <summary>
        /// 交易最后操作时间
        /// </summary>
        public int DealLastTick { get; set; }
        /// <summary>
        /// 交易的金币数量
        /// </summary>
        public int DealGolds { get; set; }
        /// <summary>
        /// 确认交易标志
        /// </summary>
        public bool DealSuccess { get; set; }
        /// <summary>
        /// 回城地图
        /// </summary>
        public string HomeMap { get; set; }
        /// <summary>
        /// 回城座标X
        /// </summary>
        public short HomeX { get; set; }
        /// <summary>
        /// 回城座标Y
        /// </summary>
        public short HomeY { get; set; }
        /// <summary>
        /// 记忆使用间隔
        /// </summary>
        public int GroupRcallTick { get; set; }
        public short GroupRcallTime { get; set; }
        /// <summary>
        /// 行会传送
        /// </summary>
        public bool GuildMove { get; set; }
        public CommandMessage ClientMsg { get; set; }
        /// <summary>
        /// 在行会占争地图中死亡次数
        /// </summary>
        public ushort FightZoneDieCount { get; set; }
        /// <summary>
        /// 祈祷
        /// </summary>
        public bool IsSpirit { get; set; }
        /// <summary>
        /// 野蛮冲撞间隔
        /// </summary>
        public int DoMotaeboTick { get; set; }
        public bool CrsHitkill { get; set; }
        public bool MBo43Kill { get; set; }
        public bool RedUseHalfMoon { get; set; }
        public bool UseThrusting { get; set; }
        public bool UseHalfMoon { get; set; }
        /// <summary>
        /// 魔法技能
        /// </summary>
        public UserMagic[] MagicArr { get; set; }
        /// <summary>
        /// 攻杀剑法
        /// </summary>
        public bool PowerHit { get; set; }
        /// <summary>
        /// 烈火剑法
        /// </summary>
        public bool FireHitSkill { get; set; }
        /// <summary>
        /// 烈火剑法
        /// </summary>
        public bool TwinHitSkill { get; set; }
        /// <summary>
        /// 额外攻击伤害(攻杀)
        /// </summary>
        public ushort HitPlus { get; set; }
        /// <summary>
        /// 双倍攻击伤害(烈火专用)
        /// </summary>
        public ushort HitDouble { get; set; }
        public int LatestFireHitTick { get; set; }
        public int LatestTwinHitTick { get; set; }
        /// <summary>
        /// 力量物品值
        /// </summary>
        public byte PowerItem { get; set; }
        /// <summary>
        /// 交易列表
        /// </summary>
        public IList<UserItem> DealItemList { get; set; }
        /// <summary>
        /// 仓库物品列表
        /// </summary>
        public IList<UserItem> StorageItemList { get; set; }
        /// <summary>
        /// 可见事件列表
        /// </summary>
        public IList<MapEvent> VisibleEvents { get; set; }
        /// <summary>
        /// 可见物品列表
        /// </summary>
        public IList<VisibleMapItem> VisibleItems { get; set; }
        /// <summary>
        /// 禁止私聊人员列表
        /// </summary>
        public IList<string> LockWhisperList { get; set; }
        /// <summary>
        /// 力量物品(影响力量的物品)
        /// </summary>
        public bool BoPowerItem { get; set; }
        public bool AllowGroupReCall { get; set; }
        public int HungerStatus { get; set; }
        public int BonusPoint { get; set; }
        public byte BtB2 { get; set; }
        /// <summary>
        /// 人物攻击变色标志
        /// </summary>
        public bool PvpFlag { get; set; }
        /// <summary>
        /// 减PK值时间`
        /// </summary>
        public int DecPkPointTick { get; set; }
        /// <summary>
        /// 人物的PK值
        /// </summary>
        public int PkPoint { get; set; }
        /// <summary>
        /// 人物攻击变色时间长度
        /// </summary>
        public int PvpNameColorTick { get; set; }
        public bool NameColorChanged { get; set; }
        /// <summary>
        /// 是否在开行会战
        /// </summary>
        public bool InGuildWarArea { get; set; }
        public IGuild MyGuild { get; set; }
        public short GuildRankNo { get; set; }
        public string GuildRankName { get; set; }
        public string ScriptLable { get; set; }
        public ScriptInfo Script { get; set; }
        public int SayMsgCount { get; set; }
        public int SayMsgTick { get; set; }
        public bool DisableSayMsg { get; set; }
        public int DisableSayMsgTick { get; set; }
        public int CheckDupObjTick { get; set; }
        public int DiscountForNightTick { get; set; }
        /// <summary>
        /// 是否在安全区域
        /// </summary>
        public bool IsSafeArea { get; set; }
        /// <summary>
        /// 喊话消息间隔
        /// </summary>
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
        /// <summary>
        /// 复活戒指使用间隔时间
        /// </summary>
        public int RevivalTick { get; set; }
        /// <summary>
        /// 掉物品
        /// </summary>
        public bool NoDropItem { get; set; }
        /// <summary>
        /// 探测项链使用间隔
        /// </summary>
        public int ProbeTick { get; set; }
        /// <summary>
        /// 传送戒指使用间隔
        /// </summary>
        public int TeleportTick { get; set; }
        public int DecHungerPointTick { get; set; }
        /// <summary>
        /// 气血石
        /// </summary>
        public int AutoAddHpmpMode { get; set; }
        public int CheckHpmpTick { get; set; }
        public int KickOffLineTick { get; set; }
        /// <summary>
        /// 挂机
        /// </summary>
        public bool OffLineFlag { get; set; }
        /// <summary>
        /// 挂机字符
        /// </summary>
        public string OffLineLeaveWord { get; set; }
        /// <summary>
        /// Socket Handle
        /// </summary>
        public int SocketId { get; set; }
        /// <summary>
        /// 人物连接到游戏网关SOCKETID
        /// </summary>
        public ushort SocketIdx { get; set; }
        /// <summary>
        /// 人物所在网关号
        /// </summary>
        public int GateIdx { get; set; }
        public int SoftVersionDate { get; set; }
        /// <summary>
        /// 登录时间戳
        /// </summary>
        public long LogonTime { get; set; }
        /// <summary>
        /// 战领沙城时间
        /// </summary>
        public int LogonTick { get; set; }
        /// <summary>
        /// 是否进入游戏完成
        /// </summary>
        public bool BoReadyRun { get; set; }
        /// <summary>
        /// 移动间隔
        /// </summary>
        public int MapMoveTick { get; set; }
        /// <summary>
        /// 人物当前付费模式
        /// 1:试玩
        /// 2:付费
        /// 3:测试
        /// </summary>
        public byte PayMent { get; set; }
        public byte PayMode { get; set; }
        /// <summary>
        /// 当前会话ID
        /// </summary>
        public int SessionId { get; set; }
        /// <summary>
        /// 全局会话信息
        /// </summary>
        public PlayerSession SessInfo { get; set; }
        public int LoadTick { get; set; }
        /// <summary>
        /// 人物当前所在服务器序号
        /// </summary>
        public byte ServerIndex { get; set; }
        /// <summary>
        /// 超时关闭链接
        /// </summary>
        public bool BoEmergencyClose { get; set; }
        /// <summary>
        /// 掉线标志
        /// </summary>
        public bool BoSoftClose { get; set; }
        /// <summary>
        /// 断线标志(@kick 命令)
        /// </summary>
        public bool BoKickFlag { get; set; }
        /// <summary>
        /// 是否重连
        /// </summary>
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
        /// <summary>
        /// 心灵启示
        /// </summary>
        public bool AbilSeeHealGauge { get; set; }
        /// <summary>
        /// 攻击间隔
        /// </summary>
        public int HitIntervalTime { get; set; }
        /// <summary>
        /// 魔法间隔
        /// </summary>
        public int MagicHitIntervalTime { get; set; }
        /// <summary>
        /// 走路间隔
        /// </summary>
        public int RunIntervalTime { get; set; }
        /// <summary>
        /// 走路间隔
        /// </summary>
        public int WalkIntervalTime { get; set; }
        /// <summary>
        /// 换方向间隔
        /// </summary>
        public int TurnIntervalTime { get; set; }
        /// <summary>
        /// 组合操作间隔
        /// </summary>
        public int ActionIntervalTime { get; set; }
        /// <summary>
        /// 移动刺杀间隔
        /// </summary>
        public int RunLongHitIntervalTime { get; set; }
        /// <summary>
        /// 跑位攻击间隔
        /// </summary>        
        public int RunHitIntervalTime { get; set; }
        /// <summary>
        /// 走位攻击间隔
        /// </summary>        
        public int WalkHitIntervalTime { get; set; }
        /// <summary>
        /// 跑位魔法间隔
        /// </summary>        
        public int RunMagicIntervalTime { get; set; }
        /// <summary>
        /// 魔法攻击时间
        /// </summary>        
        public int MagicAttackTick { get; set; }
        /// <summary>
        /// 魔法攻击间隔时间
        /// </summary>
        public int MagicAttackInterval { get; set; }
        /// <summary>
        /// 人物跑动时间
        /// </summary>
        public int MoveTick { get; set; }
        /// <summary>
        /// 人物攻击计数
        /// </summary>
        public int AttackCount { get; set; }
        /// <summary>
        /// 人物攻击计数
        /// </summary>
        public int AttackCountA { get; set; }
        /// <summary>
        /// 魔法攻击计数
        /// </summary>
        public int MagicAttackCount { get; set; }
        /// <summary>
        /// 人物跑计数
        /// </summary>
        public int MoveCount { get; set; }
        /// <summary>
        /// 超速计数
        /// </summary>
        public int OverSpeedCount { get; set; }
        /// <summary>
        /// 复活戒指
        /// </summary>
        public bool Revival { get; set; }
        /// <summary>
        /// 传送戒指
        /// </summary>
        public bool Teleport { get; set; }
        /// <summary>
        /// 麻痹戒指
        /// </summary>
        public bool Paralysis { get; set; }
        /// <summary>
        /// 火焰戒指
        /// </summary>
        public bool FlameRing { get; set; }
        /// <summary>
        /// 治愈戒指
        /// </summary>
        public bool RecoveryRing { get; set; }
        /// <summary>
        /// 未知戒指
        /// </summary>
        public bool AngryRing { get; set; }
        /// <summary>
        /// 护身戒指
        /// </summary>
        public bool MagicShield { get; set; }
        /// <summary>
        /// 防护身
        /// </summary>
        public bool UnMagicShield { get; set; }
        /// <summary>
        /// 活力戒指
        /// </summary>
        public bool MuscleRing { get; set; }
        /// <summary>
        /// 探测项链
        /// </summary>
        public bool ProbeNecklace { get; set; }
        /// <summary>
        /// 防复活
        /// </summary>
        public bool UnRevival { get; set; }
        /// <summary>
        /// 记忆全套
        /// </summary>
        public bool RecallSuite { get; set; }
        /// <summary>
        /// 魔血一套
        /// </summary>
        public int MoXieSuite { get; set; }
        /// <summary>
        /// 虹魔一套
        /// </summary>
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
        /// <summary>
        /// 职业属性点
        /// </summary>
        public NakedAbility BonusAbil { get; set; }
        /// <summary>
        /// 玩家的变量P
        /// </summary>
        public int[] MNVal { get; set; }
        /// <summary>
        /// 玩家的变量M
        /// </summary>
        public int[] MNMval { get; set; }
        /// <summary>
        /// 玩家的变量D
        /// </summary>
        public int[] MDyVal { get; set; }
        /// <summary>
        /// 玩家的变量
        /// </summary>
        public string[] MNSval { get; set; }
        /// <summary>
        /// 人物变量  N
        /// </summary>
        public int[] MNInteger { get; set; }
        /// <summary>
        /// 人物变量  S
        /// </summary>
        public string[] MSString { get; set; }
        /// <summary>
        /// 服务器变量 W 0-20 个人服务器数值变量，不可保存，不可操作
        /// </summary>
        public string[] ServerStrVal { get; set; }
        /// <summary>
        /// E 0-20 个人服务器字符串变量，不可保存，不可操作
        /// </summary>
        public int[] ServerIntVal { get; set; }
        public Dictionary<string, int> IntegerList { get; set; }
        public Dictionary<string, string> m_StringList { get; set; }
        public string ScatterItemName { get; set; }
        public string ScatterItemOwnerName { get; set; }
        public int ScatterItemX { get; set; }
        public int ScatterItemY { get; set; }
        public string ScatterItemMapName { get; set; }
        public string ScatterItemMapDesc { get; set; }
        /// <summary>
        /// 技能表
        /// </summary>
        public IList<UserMagic> MagicList { get; set; }
        /// <summary>
        /// 组队长
        /// </summary>
        public int GroupOwner { get; set; }
        /// <summary>
        /// 组成员
        /// </summary>
        public IList<IPlayerActor> GroupMembers { get; set; }
        public string PlayDiceLabel { get; set; }
        public bool IsTimeRecall { get; set; }
        public int TimeRecallTick { get; set; }
        public string TimeRecallMoveMap { get; set; }
        public short TimeRecallMoveX { get; set; }
        public short TimeRecallMoveY { get; set; }
        /// <summary>
        /// 减少勋章持久间隔
        /// </summary>
        public int DecLightItemDrugTick { get; set; }
        /// <summary>
        /// 保存人物数据时间间隔
        /// </summary>
        public int SaveRcdTick { get; set; }
        public byte Bright { get; set; }
        public bool IsNewHuman { get; set; }
        public bool IsSendNotice { get; set; }
        public int WaitLoginNoticeOkTick { get; set; }
        public bool LoginNoticeOk { get; set; }
        /// <summary>
        /// 试玩模式
        /// </summary>
        public bool TryPlayMode { get; set; }
        public int ShowLineNoticeTick { get; set; }
        public int ShowLineNoticeIdx { get; set; }
        public int SoftVersionDateEx { get; set; }
        /// <summary>
        /// 可点击脚本标签字典
        /// </summary>
        public Hashtable CanJmpScriptLableMap { get; set; }
        public int ScriptGotoCount { get; set; }
        /// <summary>
        /// 用于处理 @back 脚本命令
        /// </summary>
        public string ScriptCurrLable { get; set; }
        /// <summary>
        /// 用于处理 @back 脚本命令
        /// </summary>        
        public string ScriptGoBackLable { get; set; }
        /// <summary>
        /// 转身间隔
        /// </summary>
        public int TurnTick { get; set; }
        public int OldIdent { get; set; }
        public byte MBtOldDir { get; set; }
        /// <summary>
        /// 第一个操作
        /// </summary>
        public bool IsFirstAction { get; set; }
        /// <summary>
        /// 二次操作之间间隔时间
        /// </summary>        
        public int ActionTick { get; set; }
        /// <summary>
        /// 配偶名称
        /// </summary>
        public string DearName { get; set; }
        public IPlayerActor DearHuman { get; set; }
        /// <summary>
        /// 是否允许夫妻传送
        /// </summary>
        public bool CanDearRecall { get; set; }
        public bool CanMasterRecall { get; set; }
        /// <summary>
        /// 夫妻传送时间
        /// </summary>
        public int DearRecallTick { get; set; }
        public int MasterRecallTick { get; set; }
        /// <summary>
        /// 师徒名称
        /// </summary>
        public string MasterName { get; set; }
        public IPlayerActor MasterHuman { get; set; }
        public IList<IActor> MasterList { get; set; }
        public bool IsMaster { get; set; }
        /// <summary>
        /// 对面玩家
        /// </summary>
        public int PoseBaseObject { get; set; }
        /// <summary>
        /// 声望点
        /// </summary>
        public byte CreditPoint { get; set; }
        /// <summary>
        /// 离婚次数
        /// </summary>        
        public byte MarryCount { get; set; }
        /// <summary>
        /// 转生等级
        /// </summary>
        public byte ReLevel { get; set; }
        public byte ReColorIdx { get; set; }
        public int ReColorTick { get; set; }
        /// <summary>
        /// 杀怪经验倍数
        /// </summary>
        public int MNKillMonExpMultiple { get; set; }
        /// <summary>
        /// 处理消息循环时间控制
        /// </summary>        
        public int GetMessageTick { get; set; }
        public bool IsSetStoragePwd { get; set; }
        public bool IsReConfigPwd { get; set; }
        public bool IsCheckOldPwd { get; set; }
        public bool IsUnLockPwd { get; set; }
        public bool IsUnLockStoragePwd { get; set; }
        /// <summary>
        /// 锁密码
        /// </summary>
        public bool IsPasswordLocked { get; set; }
        public byte PwdFailCount { get; set; }
        /// <summary>
        /// 是否启用锁登录功能
        /// </summary>
        public bool IsLockLogon { get; set; }
        /// <summary>
        /// 是否打开登录锁
        /// </summary>        
        public bool IsLockLogoned { get; set; }
        public string MSTempPwd { get; set; }
        public string StoragePwd { get; set; }
        public bool IsStartMarry { get; set; }
        public bool IsStartMaster { get; set; }
        public bool IsStartUnMarry { get; set; }
        public bool IsStartUnMaster { get; set; }
        /// <summary>
        /// 禁止发方字(发的文字只能自己看到)
        /// </summary>
        public bool FilterSendMsg { get; set; }
        /// <summary>
        /// 杀怪经验倍数(此数除以 100 为真正倍数)
        /// </summary>        
        public int KillMonExpRate { get; set; }
        /// <summary>
        /// 人物攻击力倍数(此数除以 100 为真正倍数)
        /// </summary>        
        public int PowerRate { get; set; }
        public int KillMonExpRateTime { get; set; }
        public int PowerRateTime { get; set; }
        public int ExpRateTick { get; set; }
        /// <summary>
        /// 技巧项链
        /// </summary>
        public bool FastTrain { get; set; }
        /// <summary>
        /// 是否允许使用物品
        /// </summary>
        public bool BoCanUseItem { get; set; }
        /// <summary>
        /// 是否允许交易物品
        /// </summary>
        public bool IsCanDeal { get; set; }
        public bool IsCanDrop { get; set; }
        public bool IsCanGetBackItem { get; set; }
        public bool IsCanWalk { get; set; }
        public bool IsCanRun { get; set; }
        public bool IsCanHit { get; set; }
        public bool IsCanSpell { get; set; }
        public bool IsCanSendMsg { get; set; }
        /// <summary>
        /// 会员类型
        /// </summary>
        public int MemberType { get; set; }
        /// <summary>
        /// 会员等级
        /// </summary> 
        public byte MemberLevel { get; set; }
        /// <summary>
        /// 发祝福语标志
        /// </summary> 
        public bool BoSendMsgFlag { get; set; }
        public bool BoChangeItemNameFlag { get; set; }
        /// <summary>
        /// 游戏币
        /// </summary>
        public int GameGold { get; set; }
        /// <summary>
        /// 是否自动减游戏币
        /// </summary>        
        public bool BoDecGameGold { get; set; }
        public int DecGameGoldTime { get; set; }
        public int DecGameGoldTick { get; set; }
        public int DecGameGold { get; set; }
        // 一次减点数
        public bool BoIncGameGold { get; set; }
        // 是否自动加游戏币
        public int IncGameGoldTime { get; set; }
        public int IncGameGoldTick { get; set; }
        public int IncGameGold { get; set; }
        // 一次减点数
        public int GamePoint { get; set; }
        // 游戏点数
        public int IncGamePointTick { get; set; }
        public int PayMentPoint { get; set; }
        public int PayMentPointTick { get; set; }
        public int DecHpTick { get; set; }
        public int IncHpTick { get; set; }
        /// <summary>
        /// PK 死亡掉经验，不够经验就掉等级
        /// </summary>
        public int PkDieLostExp { get; set; }
        /// <summary>
        /// PK 死亡掉等级
        /// </summary>
        public byte PkDieLostLevel { get; set; }
        /// <summary>
        /// 私聊对象
        /// </summary>
        public IActor WhisperHuman { get; set; }
        /// <summary>
        /// 清理无效对象间隔
        /// </summary>
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
        /// <summary>
        /// 进入速度测试模式
        /// </summary>
        public bool TestSpeedMode { get; set; }
        public string RandomNo { get; set; }
        /// <summary>
        /// 刷新包裹间隔
        /// </summary>
        public int QueryBagItemsTick { get; set; }
        public bool IsTimeGoto { get; set; }
        public int TimeGotoTick { get; set; }
        public string TimeGotoLable { get; set; }
        public IActor TimeGotoNpc { get; set; }
        /// <summary>
        /// 个人定时器
        /// </summary>
        public int[] AutoTimerTick { get; set; }
        /// <summary>
        /// 个人定时器 时间间隔
        /// </summary>
        public int[] AutoTimerStatus { get; set; }
        /// <summary>
        /// 0-攻击力增加 1-魔法增加  2-道术增加(无极真气) 3-攻击速度 4-HP增加(酒气护体)
        /// 5-增加MP上限 6-减攻击力 7-减魔法 8-减道术 9-减HP 10-减MP 11-敏捷 12-增加防
        /// 13-增加魔防 14-增加道术上限(虎骨酒) 15-连击伤害增加(醉八打) 16-内力恢复速度增加(何首养气酒)
        /// 17-内力瞬间恢复增加(何首凝神酒) 18-增加斗转上限(培元酒) 19-不死状态 21-弟子心法激活
        /// 22-移动减速 23-定身(十步一杀)
        /// </summary>
        public ushort[] ExtraAbil { get; set; }
        public byte[] ExtraAbilFlag { get; set; }
        /// <summary>
        /// 0-攻击力增加 1-魔法增加  2-道术增加(无极真气) 3-攻击速度 4-HP增加(酒气护体)
        /// 5-增加MP上限 6-减攻击力 7-减魔法 8-减道术 9-减HP 10-减MP 11-敏捷 12-增加防
        /// 13-增加魔防 14-增加道术上限(虎骨酒) 15-连击伤害增加(醉八打) 16-内力恢复速度增加(何首养气酒)
        /// 17-内力瞬间恢复增加(何首凝神酒) 18-增加斗转上限(培元酒) 19-不死状态 20-道术+上下限(除魔药剂类) 21-弟子心法激活
        /// 22-移动减速 23-定身(十步一杀)
        /// </summary>
        public int[] ExtraAbilTimes { get; set; }
        /// <summary>
        /// 点击NPC时间
        /// </summary>
        public int ClickNpcTime { get; set; }
        /// <summary>
        /// 是否开通元宝交易服务
        /// </summary>
        public bool SaleDeal { get; set; }
        /// <summary>
        /// 确认元宝寄售标志
        /// </summary>
        public bool SellOffConfirm { get; set; }
        /// <summary>
        /// 元宝寄售物品列表
        /// </summary>
        public IList<UserItem> SellOffItemList { get; set; }
        public byte[] QuestUnitOpen { get; set; }
        public byte[] QuestUnit { get; set; }
        public byte[] QuestFlag { get; set; }
        public MarketUser MarketUser { get; set; }

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
            BoEmergencyClose = false;
            SwitchData = false;
            BoReconnection = false;
            BoKickFlag = false;
            BoSoftClose = false;
            BoReadyRun = false;
            SaveRcdTick = HUtil32.GetTickCount();
            DecHungerPointTick = HUtil32.GetTickCount();
            GroupRcallTick = HUtil32.GetTickCount();
            WantRefMsg = true;
            RcdSaved = false;
            DieInFight3Zone = false;
            DelayCall = 0;
            IsDelayCall = false;
            DelayCallNpc = 0;
            IsTimeRecall = false;
            TimeRecallMoveX = 0;
            TimeRecallMoveY = 0;
            RunTick = HUtil32.GetTickCount();
            RunTime = 250;
            SearchTime = 1000;
            SearchTick = HUtil32.GetTickCount();
            AllowGroup = false;
            AllowGuild = false;
            ViewRange = 12;
            InGuildWarArea = false;
            IsNewHuman = false;
            LoginNoticeOk = false;
            AttatckMode = 0;
            TryPlayMode = false;
            BonusAbil = new NakedAbility();
            AccountExpired = false;
            IsSendNotice = false;
            CheckDupObjTick = HUtil32.GetTickCount();
            DiscountForNightTick = HUtil32.GetTickCount();
            IsSafeArea = false;
            MagicAttackTick = HUtil32.GetTickCount();
            MagicAttackInterval = 0;
            AttackTick = HUtil32.GetTickCount();
            MoveTick = HUtil32.GetTickCount();
            TurnTick = HUtil32.GetTickCount();
            ActionTick = HUtil32.GetTickCount();
            AttackCount = 0;
            AttackCountA = 0;
            MagicAttackCount = 0;
            MoveCount = 0;
            OverSpeedCount = 0;
            SayMsgTick = HUtil32.GetTickCount();
            DisableSayMsg = false;
            DisableSayMsgTick = HUtil32.GetTickCount();
            LogonTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            LogonTick = HUtil32.GetTickCount();
            SwitchData = false;
            SwitchDataSended = false;
            WriteChgDataErrCount = 0;
            ShowLineNoticeTick = HUtil32.GetTickCount();
            ShowLineNoticeIdx = 0;
            SoftVersionDateEx = 0;
            CanJmpScriptLableMap = new Hashtable(StringComparer.OrdinalIgnoreCase);
            MagicList = new List<UserMagic>();
            MNKillMonExpMultiple = 1;
            KillMonExpRate = 100;
            ExpRateTick = HUtil32.GetTickCount();
            PowerRate = 100;
            IsSetStoragePwd = false;
            IsReConfigPwd = false;
            IsCheckOldPwd = false;
            IsUnLockPwd = false;
            IsUnLockStoragePwd = false;
            IsPasswordLocked = false;
            PvpFlag = false;
            // 锁仓库
            PwdFailCount = 0;
            FilterSendMsg = false;
            IsCanDeal = true;
            IsCanDrop = true;
            IsCanGetBackItem = true;
            IsCanWalk = true;
            IsCanRun = true;
            IsCanHit = true;
            IsCanSpell = true;
            BoCanUseItem = true;
            MemberType = 0;
            MemberLevel = 0;
            GameGold = 0;
            BoDecGameGold = false;
            DecGameGold = 1;
            DecGameGoldTick = HUtil32.GetTickCount();
            DecGameGoldTime = 60 * 1000;
            DecLightItemDrugTick = HUtil32.GetTickCount();
            BoIncGameGold = false;
            IncGameGold = 1;
            IncGameGoldTick = HUtil32.GetTickCount();
            IncGameGoldTime = 60 * 1000;
            GamePoint = 0;
            IncGamePointTick = HUtil32.GetTickCount();
            PayMentPoint = 0;
            DearHuman = null;
            MasterHuman = null;
            MasterList = new List<IActor>();
            BoSendMsgFlag = false;
            BoChangeItemNameFlag = false;
            CanMasterRecall = false;
            CanDearRecall = false;
            DearRecallTick = HUtil32.GetTickCount();
            MasterRecallTick = HUtil32.GetTickCount();
            ReColorIdx = 0;
            WhisperHuman = null;
            OnHorse = false;
            Contribution = 0;
            HitPlus = 0;
            HitDouble = 0;
            RankLevelName = Settings.RankLevelName;
            FixedHideMode = true;
            MNVal = new int[100];
            MNMval = new int[100];
            MDyVal = new int[100];
            MNSval = new string[100];
            MNInteger = new int[100];
            MSString = new string[100];
            ServerStrVal = new string[100];
            ServerIntVal = new int[100];
            ExtraAbil = new ushort[7];
            ExtraAbilTimes = new int[7];
            ExtraAbilFlag = new byte[7];
            HearWhisper = true;
            BanShout = true;
            BanGuildChat = true;
            AllowDeal = true;
            AutoGetExpTick = HUtil32.GetTickCount();
            DecPkPointTick = HUtil32.GetTickCount();
            AutoGetExpPoint = 0;
            AutoGetExpEnvir = null;
            HitIntervalTime = SystemShare.Config.HitIntervalTime;// 攻击间隔
            MagicHitIntervalTime = SystemShare.Config.MagicHitIntervalTime;// 魔法间隔
            RunIntervalTime = SystemShare.Config.RunIntervalTime;// 走路间隔
            WalkIntervalTime = SystemShare.Config.WalkIntervalTime;// 走路间隔
            TurnIntervalTime = SystemShare.Config.TurnIntervalTime;// 换方向间隔
            ActionIntervalTime = SystemShare.Config.ActionIntervalTime;// 组合操作间隔
            RunLongHitIntervalTime = SystemShare.Config.RunLongHitIntervalTime;// 组合操作间隔
            RunHitIntervalTime = SystemShare.Config.RunHitIntervalTime;// 组合操作间隔
            WalkHitIntervalTime = SystemShare.Config.WalkHitIntervalTime;// 组合操作间隔
            RunMagicIntervalTime = SystemShare.Config.RunMagicIntervalTime;// 跑位魔法间隔
            DynamicVarMap = new Dictionary<string, DynamicVar>(StringComparer.OrdinalIgnoreCase);
            SessInfo = null;
            TestSpeedMode = false;
            IsLockLogon = true;
            IsLockLogoned = false;
            IsTimeGoto = false;
            TimeGotoTick = HUtil32.GetTickCount();
            TimeGotoLable = "";
            TimeGotoNpc = null;
            AutoTimerTick = new int[20];
            AutoTimerStatus = new int[20];
            CellType = CellType.Play;
            QueryExpireTick = 60 * 1000;
            AccountExpiredTick = HUtil32.GetTickCount();
            GoldMax = SystemShare.Config.HumanMaxGold;
            QuestUnitOpen = new byte[128];
            QuestUnit = new byte[128];
            QuestFlag = new byte[128];
            MagicArr = new UserMagic[50];
            GroupMembers = new List<IPlayerActor>();
            VisibleEvents = new List<MapEvent>();
            VisibleItems = new List<VisibleMapItem>();
            ItemList = new List<UserItem>(Grobal2.MaxBagItem);
            MapMoveTick = HUtil32.GetTickCount();
            RandomNo = M2Share.RandomNumber.Random(999999).ToString();
        }

        public override void Initialize()
        {
            base.Initialize();
            for (int i = 0; i < MagicList.Count; i++)
            {
                if (MagicList[i].Level >= 4)
                {
                    MagicList[i].Level = 0;
                }
            }
            AddBodyLuck(0);
        }

        private void SendNotice()
        {
            //todo 优化
            List<string> loadList = new List<string>();
            M2Share.NoticeMgr.GetNoticeMsg("Notice", loadList);
            string sNoticeMsg = string.Empty;
            if (loadList.Count > 0)
            {
                for (int i = 0; i < loadList.Count; i++)
                {
                    sNoticeMsg = sNoticeMsg + loadList[i] + "\x20\x1B";
                }
            }
            SendDefMessage(Messages.SM_SENDNOTICE, 2000, 0, 0, 0, sNoticeMsg.Replace("/r/n/r/n ", ""));
        }

        public void RunNotice()
        {
            const string sExceptionMsg = "[Exception] PlayObject::RunNotice";
            if (BoEmergencyClose || BoKickFlag || BoSoftClose)
            {
                if (BoKickFlag)
                {
                    SendDefMessage(Messages.SM_OUTOFCONNECTION, 0, 0, 0, 0);
                }
                MakeGhost();
            }
            else
            {
                try
                {
                    if (!IsSendNotice)
                    {
                        SendNotice();
                        IsSendNotice = true;
                        WaitLoginNoticeOkTick = HUtil32.GetTickCount();
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - WaitLoginNoticeOkTick) > 10 * 1000)
                        {
                            BoEmergencyClose = true;
                        }
                        ProcessMessage msg = default;
                        while (GetMessage(ref msg))
                        {
                            if (msg.wIdent == Messages.CM_LOGINNOTICEOK)
                            {
                                LoginNoticeOk = true;
                                ClientTick = (short)msg.nParam1;
                                SysMsg(ClientTick.ToString(), MsgColor.Red, MsgType.Notice);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    M2Share.Logger.Error(sExceptionMsg);
                }
            }
        }

        /// <summary>
        /// 发送登录消息
        /// </summary>
        private void SendLogon()
        {
            MessageBodyWL messageBodyWl = default;
            ClientMsg = Messages.MakeMessage(Messages.SM_LOGON, ActorId, CurrX, CurrY, HUtil32.MakeWord(Dir, Light));
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
            SendSocket(ClientMsg, EDCode.EncodePacket(messageBodyWl));
            int nRecog = GetFeatureToLong();
            SendDefMessage(Messages.SM_FEATURECHANGED, ActorId, HUtil32.LoWord(nRecog), HUtil32.HiWord(nRecog), GetFeatureEx());
            SendDefMessage(Messages.SM_ATTACKMODE, (byte)AttatckMode, 0, 0, 0);
        }

        /// <summary>
        /// 玩家登录
        /// </summary>
        public void UserLogon()
        {
            const string sExceptionMsg = "[Exception] PlayObject::UserLogon";
            try
            {
                if (SystemShare.Config.TestServer)
                {
                    if (Abil.Level < SystemShare.Config.TestLevel)
                    {
                        Abil.Level = (byte)SystemShare.Config.TestLevel;
                    }
                    if (Gold < SystemShare.Config.TestGold)
                    {
                        Gold = SystemShare.Config.TestGold;
                    }
                }
                if (SystemShare.Config.TestServer || SystemShare.Config.ServiceMode)
                {
                    PayMent = 3;
                }
                MapMoveTick = HUtil32.GetTickCount();
                LogonTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                LogonTick = HUtil32.GetTickCount();
                Initialize();
                SendPriorityMsg(Messages.RM_LOGON, 0, 0, 0, 0, "", MessagePriority.High);
                if (Abil.Level <= 7)
                {
                    if (GetRangeHumanCount() >= 80)
                    {
                        MapRandomMove(Envir.MapName, 0);
                    }
                }
                if (DieInFight3Zone)
                {
                    MapRandomMove(Envir.MapName, 0);
                }
                //if (SystemShare.WorldEngine.GetHumPermission(ChrName, ref sIPaddr, ref Permission))
                //{
                //    if (SystemShare.Config.PermissionSystem)
                //    {
                //        if (!M2Share.CompareIPaddr(LoginIpAddr, sIPaddr))
                //        {
                //            SysMsg(sCheckIPaddrFail, MsgColor.Red, MsgType.Hint);
                //            BoEmergencyClose = true;
                //        }
                //    }
                //}
                GetStartPoint();
                for (int i = MagicList.Count - 1; i >= 0; i--)
                {
                    CheckSeeHealGauge(MagicList[i]);
                }
                UserItem userItem;
                if (IsNewHuman)
                {
                    userItem = new UserItem();
                    if (SystemShare.ItemSystem.CopyToUserItemFromName(SystemShare.Config.Candle, ref userItem))
                    {
                        ItemList.Add(userItem);
                    }
                    else
                    {
                        Dispose(userItem);
                    }
                    userItem = new UserItem();
                    if (SystemShare.ItemSystem.CopyToUserItemFromName(SystemShare.Config.BasicDrug, ref userItem))
                    {
                        ItemList.Add(userItem);
                    }
                    else
                    {
                        Dispose(userItem);
                    }
                    userItem = new UserItem();
                    if (SystemShare.ItemSystem.CopyToUserItemFromName(SystemShare.Config.WoodenSword, ref userItem))
                    {
                        ItemList.Add(userItem);
                    }
                    else
                    {
                        Dispose(userItem);
                    }
                    userItem = new UserItem();
                    string sItem = Gender == PlayGender.Man
                        ? SystemShare.Config.ClothsMan
                        : SystemShare.Config.ClothsWoman;
                    if (SystemShare.ItemSystem.CopyToUserItemFromName(sItem, ref userItem))
                    {
                        ItemList.Add(userItem);
                    }
                    else
                    {
                        Dispose(userItem);
                    }
                }
                // 检查背包中的物品是否合法
                for (int i = ItemList.Count - 1; i >= 0; i--)
                {
                    if (!string.IsNullOrEmpty(SystemShare.ItemSystem.GetStdItemName(ItemList[i].Index))) continue;
                    Dispose(ItemList[i]);
                    ItemList.RemoveAt(i);
                }
                // 检查人物身上的物品是否符合使用规则
                if (SystemShare.Config.CheckUserItemPlace)
                {
                    for (int i = 0; i < UseItems.Length; i++)
                    {
                        if (UseItems[i] == null || UseItems[i].Index <= 0) continue;
                        StdItem stdItem = SystemShare.ItemSystem.GetStdItem(UseItems[i].Index);
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
                for (int i = ItemList.Count - 1; i >= 0; i--)
                {
                    string sItemName = SystemShare.ItemSystem.GetStdItemName(ItemList[i].Index);
                    for (int j = i - 1; j >= 0; j--)
                    {
                        UserItem userItem1 = ItemList[j];
                        if (SystemShare.ItemSystem.GetStdItemName(userItem1.Index) == sItemName && ItemList[i].MakeIndex == userItem1.MakeIndex)
                        {
                            ItemList.RemoveAt(j);
                            break;
                        }
                    }
                }
                for (int i = 0; i < StatusArrTick.Length; i++)
                {
                    if (StatusTimeArr[i] > 0)
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
                if (Gold > SystemShare.Config.HumanMaxGold * 2 && SystemShare.Config.HumanMaxGold > 0)
                {
                    Gold = SystemShare.Config.HumanMaxGold * 2;
                }
                if (!TryPlayMode)
                {
                    if (SoftVersionDate < SystemShare.Config.SoftVersionDate)//登录版本号验证
                    {
                        SysMsg(Settings.ClientSoftVersionError, MsgColor.Red, MsgType.Hint);
                        SysMsg(Settings.DownLoadNewClientSoft, MsgColor.Red, MsgType.Hint);
                        SysMsg(Settings.ForceDisConnect, MsgColor.Red, MsgType.Hint);
                        BoEmergencyClose = true;
                        return;
                    }
                    if (SoftVersionDateEx == 0 && SystemShare.Config.boOldClientShowHiLevel)
                    {
                        SysMsg(Settings.ClientSoftVersionTooOld, MsgColor.Blue, MsgType.Hint);
                        SysMsg(Settings.DownLoadAndUseNewClient, MsgColor.Red, MsgType.Hint);
                        if (!SystemShare.Config.CanOldClientLogon)
                        {
                            SysMsg(Settings.ClientSoftVersionError, MsgColor.Red, MsgType.Hint);
                            SysMsg(Settings.DownLoadNewClientSoft, MsgColor.Red, MsgType.Hint);
                            SysMsg(Settings.ForceDisConnect, MsgColor.Red, MsgType.Hint);
                            BoEmergencyClose = true;
                            return;
                        }
                    }
                    switch (AttatckMode)
                    {
                        case AttackMode.HAM_ALL:// [攻击模式: 全体攻击]
                            SysMsg(Settings.AttackModeOfAll, MsgColor.Green, MsgType.Hint);
                            break;
                        case AttackMode.HAM_PEACE:// [攻击模式: 和平攻击]
                            SysMsg(Settings.AttackModeOfPeaceful, MsgColor.Green, MsgType.Hint);
                            break;
                        case AttackMode.HAM_DEAR:// [攻击模式: 和平攻击]
                            SysMsg(Settings.AttackModeOfDear, MsgColor.Green, MsgType.Hint);
                            break;
                        case AttackMode.HAM_MASTER:// [攻击模式: 和平攻击]
                            SysMsg(Settings.AttackModeOfMaster, MsgColor.Green, MsgType.Hint);
                            break;
                        case AttackMode.HAM_GROUP:// [攻击模式: 编组攻击]
                            SysMsg(Settings.AttackModeOfGroup, MsgColor.Green, MsgType.Hint);
                            break;
                        case AttackMode.HAM_GUILD:// [攻击模式: 行会攻击]
                            SysMsg(Settings.AttackModeOfGuild, MsgColor.Green, MsgType.Hint);
                            break;
                        case AttackMode.HAM_PKATTACK:// [攻击模式: 红名攻击]
                            SysMsg(Settings.AttackModeOfRedWhite, MsgColor.Green, MsgType.Hint);
                            break;
                    }
                    SysMsg(Settings.StartChangeAttackModeHelp, MsgColor.Green, MsgType.Hint);// 使用组合快捷键 CTRL-H 更改攻击...
                    if (SystemShare.Config.TestServer)
                    {
                        SysMsg(Settings.StartNoticeMsg, MsgColor.Green, MsgType.Hint);// 欢迎进入本服务器进行游戏...
                    }
                    if (SystemShare.WorldEngine.PlayObjectCount > SystemShare.Config.TestUserLimit)
                    {
                        if (Permission < 2)
                        {
                            SysMsg(Settings.OnlineUserFull, MsgColor.Red, MsgType.Hint);
                            SysMsg(Settings.ForceDisConnect, MsgColor.Red, MsgType.Hint);
                            BoEmergencyClose = true;
                        }
                    }
                }
                Bright = M2Share.GameTime;
                SendPriorityMsg(Messages.RM_ABILITY, 0, 0, 0, 0, "", MessagePriority.High);
                SendPriorityMsg(Messages.RM_SUBABILITY, 0, 0, 0, 0, "", MessagePriority.High);
                SendPriorityMsg(Messages.RM_ADJUST_BONUS, 0, 0, 0, 0);
                SendPriorityMsg(Messages.RM_DAYCHANGING, 0, 0, 0, 0);
                SendPriorityMsg(Messages.RM_SENDUSEITEMS, 0, 0, 0, 0, "", MessagePriority.High);
                SendPriorityMsg(Messages.RM_SENDMYMAGIC, 0, 0, 0, 0, "", MessagePriority.High);
                MyGuild = SystemShare.GuildMgr.MemberOfGuild(ChrName);
                if (MyGuild != null)
                {
                    short rankNo = 0;
                    GuildRankName = MyGuild.GetRankName(this, ref rankNo);
                    GuildRankNo = rankNo;
                    for (int i = MyGuild.GuildWarList.Count - 1; i >= 0; i--)
                    {
                        SysMsg(MyGuild.GuildWarList[i] + " 正在与本行会进行行会战.", MsgColor.Green, MsgType.Hint);
                    }
                }
                RefShowName();
                if (PayMent == 1)
                {
                    if (!TryPlayMode)
                    {
                        SysMsg(Settings.YouNowIsTryPlayMode, MsgColor.Red, MsgType.Hint);
                    }
                    GoldMax = SystemShare.Config.HumanTryModeMaxGold;
                    if (Abil.Level > SystemShare.Config.TryModeLevel)
                    {
                        SysMsg("测试状态可以使用到第 " + SystemShare.Config.TryModeLevel, MsgColor.Red, MsgType.Hint);
                        SysMsg("链接中断，请到以下地址获得收费相关信息。(https://www.mir2.com)", MsgColor.Red, MsgType.Hint);
                        BoEmergencyClose = true;
                    }
                }
                if (PayMent == 3 && !TryPlayMode)
                {
                    SysMsg(Settings.NowIsFreePlayMode, MsgColor.Green, MsgType.Hint);
                }
                if (SystemShare.Config.VentureServer)
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
                if (SystemShare.ManageNPC != null)
                {
                    SystemShare.ManageNPC.GotoLable(this, "@Login", false);
                }
                FixedHideMode = false;
                if (!string.IsNullOrEmpty(DearName))
                {
                    CheckMarry();
                }
                CheckMaster();
                FilterSendMsg = M2Share.GetDisableSendMsgList(ChrName);
                // 密码保护系统
                if (SystemShare.Config.PasswordLockSystem)
                {
                    if (IsPasswordLocked)
                    {
                        IsCanGetBackItem = !SystemShare.Config.LockGetBackItemAction;
                    }
                    if (SystemShare.Config.LockHumanLogin && IsLockLogon && IsPasswordLocked)
                    {
                        IsCanDeal = !SystemShare.Config.LockDealAction;
                        IsCanDrop = !SystemShare.Config.LockDropAction;
                        BoCanUseItem = !SystemShare.Config.LockUserItemAction;
                        IsCanWalk = !SystemShare.Config.LockWalkAction;
                        IsCanRun = !SystemShare.Config.LockRunAction;
                        IsCanHit = !SystemShare.Config.LockHitAction;
                        IsCanSpell = !SystemShare.Config.LockSpellAction;
                        IsCanSendMsg = !SystemShare.Config.LockSendMsgAction;
                        ObMode = SystemShare.Config.LockInObModeAction;
                        AdminMode = SystemShare.Config.LockInObModeAction;
                        // SysMsg(Settings.ActionIsLockedMsg + " 开锁命令: @" + CommandMgr.GameCommands.LockLogon.CmdName, MsgColor.Red, MsgType.Hint);
                        //  SendMsg(ModuleShare.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.ActionIsLockedMsg + "\\ \\" + "密码命令: @" + CommandMgr.GameCommands.PasswordLock.CmdName);
                    }
                    if (!IsPasswordLocked)
                    {
                        //  SysMsg(Format(Settings.PasswordNotSetMsg, CommandMgr.GameCommands.PasswordLock.CmdName), MsgColor.Red, MsgType.Hint);
                    }
                    if (!IsLockLogon && IsPasswordLocked)
                    {
                        //  SysMsg(Format(Settings.NotPasswordProtectMode, CommandMgr.GameCommands.LockLogon.CmdName), MsgColor.Red, MsgType.Hint);
                    }
                    //SysMsg(Settings.ActionIsLockedMsg + " 开锁命令: @" + CommandMgr.GameCommands.Unlock.CmdName, MsgColor.Red, MsgType.Hint);
                    // SendMsg(ModuleShare.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.ActionIsLockedMsg + "\\ \\" + "开锁命令: @" + CommandMgr.GameCommands.Unlock.CmdName + '\\' + "加锁命令: @" + CommandMgr.GameCommands.Lock.CmdName + '\\' + "设置密码命令: @" + CommandMgr.GameCommands.SetPassword.CmdName + '\\' + "修改密码命令: @" + CommandMgr.GameCommands.ChgPassword.CmdName);
                }
                // 重置泡点方面计时
                IncGamePointTick = HUtil32.GetTickCount();
                IncGameGoldTick = HUtil32.GetTickCount();
                AutoGetExpTick = HUtil32.GetTickCount();
                GetSellOffGlod();// 检查是否有元宝寄售交易结束还没得到元宝
            }
            catch (Exception e)
            {
                M2Share.Logger.Error(sExceptionMsg);
                M2Share.Logger.Error(e.StackTrace);
            }
            // ReadAllBook();
        }

        /// <summary>
        /// 使用祝福油
        /// </summary>
        /// <returns></returns>
        private bool WeaptonMakeLuck()
        {
            if (UseItems[ItemLocation.Weapon] == null || UseItems[ItemLocation.Weapon].Index <= 0)
            {
                return false;
            }
            int nRand = 0;
            StdItem stdItem = SystemShare.ItemSystem.GetStdItem(UseItems[ItemLocation.Weapon].Index);
            if (stdItem != null)
            {
                nRand = Math.Abs(HUtil32.HiByte(stdItem.DC) - HUtil32.LoByte(stdItem.DC)) / 5;
            }
            if (M2Share.RandomNumber.Random(SystemShare.Config.WeaponMakeUnLuckRate) == 1)
            {
                MakeWeaponUnlock();
            }
            else
            {
                bool boMakeLuck = false;
                if (UseItems[ItemLocation.Weapon].Desc[4] > 0)
                {
                    UseItems[ItemLocation.Weapon].Desc[4] -= 1;
                    SysMsg(Settings.WeaptonMakeLuck, MsgColor.Green, MsgType.Hint);
                    boMakeLuck = true;
                }
                else if (UseItems[ItemLocation.Weapon].Desc[3] < SystemShare.Config.WeaponMakeLuckPoint1)
                {
                    UseItems[ItemLocation.Weapon].Desc[3]++;
                    SysMsg(Settings.WeaptonMakeLuck, MsgColor.Green, MsgType.Hint);
                    boMakeLuck = true;
                }
                else if (UseItems[ItemLocation.Weapon].Desc[3] < SystemShare.Config.WeaponMakeLuckPoint2 && M2Share.RandomNumber.Random(nRand + SystemShare.Config.WeaponMakeLuckPoint2Rate) == 1)
                {
                    UseItems[ItemLocation.Weapon].Desc[3]++;
                    SysMsg(Settings.WeaptonMakeLuck, MsgColor.Green, MsgType.Hint);
                    boMakeLuck = true;
                }
                else if (UseItems[ItemLocation.Weapon].Desc[3] < SystemShare.Config.WeaponMakeLuckPoint3 && M2Share.RandomNumber.Random(nRand * SystemShare.Config.WeaponMakeLuckPoint3Rate) == 1)
                {
                    UseItems[ItemLocation.Weapon].Desc[3]++;
                    SysMsg(Settings.WeaptonMakeLuck, MsgColor.Green, MsgType.Hint);
                    boMakeLuck = true;
                }
                if (Race == ActorRace.Play)
                {
                    RecalcAbilitys();
                    SendMsg(Messages.RM_ABILITY, 0, 0, 0, 0);
                    SendMsg(Messages.RM_SUBABILITY, 0, 0, 0, 0);
                }
                if (!boMakeLuck)
                {
                    SysMsg(Settings.WeaptonNotMakeLuck, MsgColor.Green, MsgType.Hint);
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
            if (UseItems[ItemLocation.Weapon] == null)
            {
                return false;
            }
            UserItem userItem = UseItems[ItemLocation.Weapon];
            if (userItem.Index <= 0 || userItem.DuraMax <= userItem.Dura)
            {
                return false;
            }
            userItem.DuraMax -= (ushort)((userItem.DuraMax - userItem.Dura) / SystemShare.Config.RepairItemDecDura);
            ushort nDura = (ushort)HUtil32._MIN(5000, userItem.DuraMax - userItem.Dura);
            if (nDura <= 0) return false;
            userItem.Dura += nDura;
            SendMsg(Messages.RM_DURACHANGE, 1, userItem.Dura, userItem.DuraMax, 0);
            SysMsg(Settings.WeaponRepairSuccess, MsgColor.Green, MsgType.Hint);
            return true;
        }

        /// <summary>
        /// 特修武器
        /// </summary>
        /// <returns></returns>
        private bool SuperRepairWeapon()
        {
            if (UseItems[ItemLocation.Weapon] == null || UseItems[ItemLocation.Weapon].Index <= 0)
            {
                return false;
            }
            UseItems[ItemLocation.Weapon].Dura = UseItems[ItemLocation.Weapon].DuraMax;
            SendMsg(Messages.RM_DURACHANGE, 1, UseItems[ItemLocation.Weapon].Dura, UseItems[ItemLocation.Weapon].DuraMax, 0);
            SysMsg(Settings.WeaponRepairSuccess, MsgColor.Green, MsgType.Hint);
            return true;
        }

        public void MakeWeaponUnlock()
        {
            if (UseItems[ItemLocation.Weapon] == null)
            {
                return;
            }
            if (UseItems[ItemLocation.Weapon].Index <= 0)
            {
                return;
            }
            if (UseItems[ItemLocation.Weapon].Desc[3] > 0)
            {
                UseItems[ItemLocation.Weapon].Desc[3] -= 1;
                SysMsg(Settings.TheWeaponIsCursed, MsgColor.Red, MsgType.Hint);
            }
            else
            {
                if (UseItems[ItemLocation.Weapon].Desc[4] < 10)
                {
                    UseItems[ItemLocation.Weapon].Desc[4]++;
                    SysMsg(Settings.TheWeaponIsCursed, MsgColor.Red, MsgType.Hint);
                }
            }
            RecalcAbilitys();
            SendMsg(Messages.RM_ABILITY, 0, 0, 0, 0);
            SendMsg(Messages.RM_SUBABILITY, 0, 0, 0, 0);
        }

        /// <summary>
        /// 角色杀死目标触发
        /// </summary>
        private void KillTargetTrigger(int actorId, int fightExp)
        {
            var killObject = SystemShare.ActorMgr.Get(actorId);
            if (killObject == null)
            {
                return;
            }
            if (SystemShare.FunctionNPC != null)
            {
                SystemShare.FunctionNPC.GotoLable(this, "@PlayKillMob", false);
            }
            int monsterExp = CalcGetExp(WAbil.Level, fightExp);
            if (!SystemShare.Config.VentureServer)
            {
                if (IsRobot && ExpHitter != null && ExpHitter.Race == ActorRace.Play)
                {
                    // ((IRobotPlayer)ExpHitter).GainExp(monsterExp);
                }
                else
                {
                    GainExp(monsterExp);
                }
            }
            // 是否执行任务脚本
            if (Envir.IsCheapStuff())// 地图是否有任务脚本
            {
                IMerchant QuestNPC;
                if (GroupOwner != 0)
                {
                    IPlayerActor groupOwnerPlay = (IPlayerActor)SystemShare.ActorMgr.Get(GroupOwner);
                    for (int i = 0; i < groupOwnerPlay.GroupMembers.Count; i++)
                    {
                        IPlayerActor groupHuman = groupOwnerPlay.GroupMembers[i];
                        bool tCheck;
                        if (!groupHuman.Death && Envir == groupHuman.Envir && Math.Abs(CurrX - groupHuman.CurrX) <= 12 && Math.Abs(CurrX - groupHuman.CurrX) <= 12 && this == groupHuman)
                        {
                            tCheck = false;
                        }
                        else
                        {
                            tCheck = true;
                        }
                        //QuestNPC = Envir.GetQuestNpc(groupHuman, ChrName, "", tCheck);
                        //if (QuestNPC != null)
                        //{
                        //    QuestNPC.Click(groupHuman);
                        //}
                    }
                }
                //QuestNPC = Envir.GetQuestNpc(this, ChrName, "", false);
                //if (QuestNPC != null)
                //{
                //    QuestNPC.Click(this);
                //}
            }
            try
            {
                bool boPK = false;
                if (!SystemShare.Config.VentureServer && !Envir.Flag.FightZone && !Envir.Flag.Fight3Zone)
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
                    bool guildwarkill = false;
                    IPlayerActor targetObject = ((IPlayerActor)killObject);
                    if (MyGuild != null && targetObject.MyGuild != null)
                    {
                        if (GetGuildRelation(this, targetObject) == 2)
                        {
                            guildwarkill = true;
                        }
                    }
                    else
                    {
                        IUserCastle Castle = SystemShare.CastleMgr.InCastleWarArea(this);
                        if ((Castle != null && Castle.UnderWar) || (InGuildWarArea))
                        {
                            guildwarkill = true;
                        }
                    }
                    if (!guildwarkill)
                    {
                        if ((SystemShare.Config.IsKillHumanWinLevel || SystemShare.Config.IsKillHumanWinExp || Envir.Flag.boPKWINLEVEL || Envir.Flag.boPKWINEXP))
                        {
                            PvpDie(targetObject);
                        }
                        else
                        {
                            if (!IsGoodKilling(this))
                            {
                                targetObject.IncPkPoint(SystemShare.Config.KillHumanAddPKPoint);
                                targetObject.SysMsg(Settings.YouMurderedMsg, MsgColor.Red, MsgType.Hint);
                                SysMsg(Format(Settings.YouKilledByMsg, targetObject.ChrName), MsgColor.Red, MsgType.Hint);
                                targetObject.AddBodyLuck(-SystemShare.Config.KillHumanDecLuckPoint);
                                if (PvpLevel() < 1)
                                {
                                    if (M2Share.RandomNumber.Random(5) == 0)
                                    {
                                        targetObject.MakeWeaponUnlock();
                                    }
                                }
                            }
                            else
                            {
                                targetObject.SysMsg(Settings.YouprotectedByLawOfDefense, MsgColor.Green, MsgType.Hint);
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
                M2Share.Logger.Error(ex);
            }
            if (!Envir.Flag.FightZone && !Envir.Flag.Fight3Zone && killObject.Race == ActorRace.Play)
            {
                IActor AttackBaseObject = killObject;
                if (killObject.Master != null)
                {
                    AttackBaseObject = killObject.Master;
                }
                if (!NoItem || !Envir.Flag.NoDropItem)//允许设置 m_boNoItem 后人物死亡不掉物品
                {
                    if (AttackBaseObject != null)
                    {
                        if (SystemShare.Config.KillByHumanDropUseItem && AttackBaseObject.Race == ActorRace.Play || SystemShare.Config.KillByMonstDropUseItem && AttackBaseObject.Race != ActorRace.Play)
                        {
                            killObject.DropUseItems(0);
                        }
                    }
                    else
                    {
                        killObject.DropUseItems(0);
                    }
                    if (SystemShare.Config.DieScatterBag)
                    {
                        killObject.ScatterBagItems(0);
                    }
                    if (SystemShare.Config.DieDropGold)
                    {
                        killObject.ScatterGolds(0);
                    }
                }
                AddBodyLuck(-(50 - (50 - WAbil.Level * 5)));
            }
            if ((SystemShare.FunctionNPC != null) && (Envir != null) && Envir.Flag.boKILLFUNC)
            {
                if (killObject.Race != ActorRace.Play) //怪杀死玩家
                {
                    if (ExpHitter != null)
                    {
                        if (ExpHitter.Race == ActorRace.Play)
                        {
                            SystemShare.FunctionNPC.GotoLable(ExpHitter as PlayObject, "@KillPlayMon" + Envir.Flag.nKILLFUNCNO, false);
                        }
                        if (ExpHitter.Master != null)
                        {
                            SystemShare.FunctionNPC.GotoLable(ExpHitter.Master as PlayObject, "@KillPlayMon" + Envir.Flag.nKILLFUNCNO, false);
                        }
                    }
                    else
                    {
                        if (LastHiter != null)
                        {
                            if (LastHiter.Race == ActorRace.Play)
                            {
                                SystemShare.FunctionNPC.GotoLable(LastHiter as PlayObject, "@KillPlayMon" + Envir.Flag.nKILLFUNCNO, false);
                            }
                            if (LastHiter.Master != null)
                            {
                                SystemShare.FunctionNPC.GotoLable(LastHiter.Master as PlayObject, "@KillPlayMon" + Envir.Flag.nKILLFUNCNO, false);
                            }
                        }
                    }
                }
                else
                {
                    if ((LastHiter != null) && (LastHiter.Race == ActorRace.Play))
                    {
                        SystemShare.FunctionNPC.GotoLable(LastHiter as PlayObject, "@KillPlay" + Envir.Flag.nKILLFUNCNO, false);
                    }
                }
            }
        }

        protected override bool Walk(int nIdent)
        {
            const string sExceptionMsg = "[Exception] BaseObject::Walk {0} {1} {2}:{3}";
            bool result = true;
            try
            {
                if (!Envir.CellMatch(CurrX, CurrY))
                {
                    return true;
                }
                ref MapCellInfo cellInfo = ref Envir.GetCellInfo(CurrX, CurrY, out bool cellSuccess);
                if (cellSuccess && cellInfo.IsAvailable)
                {
                    for (int i = 0; i < cellInfo.ObjList.Count; i++)
                    {
                        CellObject cellObject = cellInfo.ObjList[i];
                        switch (cellObject.CellType)
                        {
                            case CellType.MapRoute:
                                MapRouteItem mapRoute = M2Share.CellObjectMgr.Get<MapRouteItem>(cellObject.CellObjId);
                                if (mapRoute.Envir != null)
                                {
                                    if (Envir.ArroundDoorOpened(CurrX, CurrY))
                                    {
                                        if ((!mapRoute.Envir.Flag.boNEEDHOLE) || (SystemShare.EventMgr.GetEvent(Envir, CurrX, CurrY, Grobal2.ET_DIGOUTZOMBI) != null))
                                        {
                                            if (M2Share.ServerIndex == mapRoute.Envir.ServerIndex)
                                            {
                                                if (!EnterAnotherMap(mapRoute.Envir, mapRoute.X, mapRoute.Y))
                                                {
                                                    result = false;
                                                }
                                            }
                                            else
                                            {
                                                DisappearA();
                                                SpaceMoved = true;
                                                ChangeSpaceMove(mapRoute.Envir, mapRoute.X, mapRoute.Y);
                                            }
                                        }
                                    }
                                }
                                break;
                            case CellType.Event:
                                MapEvent mapEvent = null;
                                MapEvent owinEvent = M2Share.CellObjectMgr.Get<MapEvent>(cellObject.CellObjId);
                                if (owinEvent.OwnBaseObject != null)
                                {
                                    mapEvent = M2Share.CellObjectMgr.Get<MapEvent>(cellObject.CellObjId);
                                }
                                if (mapEvent != null)
                                {
                                    if (mapEvent.OwnBaseObject.IsProperTarget(this))
                                    {
                                        SendMsg(mapEvent.OwnBaseObject, Messages.RM_MAGSTRUCK_MINE, 0, mapEvent.Damage, 0, 0);
                                    }
                                }
                                break;
                        }
                    }
                }
                if (result)
                {
                    SendRefMsg(nIdent, Dir, CurrX, CurrY, 0, "");
                }
            }
            catch (Exception e)
            {
                M2Share.Logger.Error(Format(sExceptionMsg, ChrName, MapName, CurrX, CurrY));
                M2Share.Logger.Error(e.Message);
            }
            return result;
        }

        public override bool IsProperTarget(IActor baseObject)
        {
            if (baseObject == null || baseObject.ActorId == this.ActorId)
            {
                return false;
            }
            var result = IsAttackTarget(baseObject);//先检查攻击模式
            if (result)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    return IsProtectTarget(baseObject);
                }
                if ((baseObject.Master != null) && (baseObject.Race != ActorRace.Play))
                {
                    if (baseObject.Master == this)
                    {
                        if (AttatckMode != AttackMode.HAM_ALL)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        result = base.IsAttackTarget(baseObject);//是否可以攻击对方召唤者
                        if (IsProtectTarget(baseObject))
                        {
                            result = false;
                        }
                    }
                }
                if (result)
                {
                    return result;
                }
                return base.IsAttackTarget(baseObject);
            }
            return false;
        }

        public override void Die()
        {
            if (SuperMan)
            {
                return;
            }
            IncSpell = 0;
            IncHealth = 0;
            IncHealing = 0;
            string tStr;
            if (GroupOwner != 0)
            {
                IPlayerActor groupOwnerPlay = (IPlayerActor)SystemShare.ActorMgr.Get(GroupOwner);
                groupOwnerPlay.DelMember(this);// 人物死亡立即退组，以防止组队刷经验
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
            //M2Share.EventSource.AddEventLog(GameEventLogType.PlayDie, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + "FZ-" + HUtil32.BoolToIntStr(Envir.Flag.FightZone) + "_F3-" + HUtil32.BoolToIntStr(Envir.Flag.Fight3Zone) + "\t" + '0' + "\t" + '1' + "\t" + tStr);
            base.Die();
            SendSelfDelayMsg(Messages.RM_MASTERDIEMUTINY, 0, 0, 0, 0, "", 1000);
        }

        protected override void KickException()
        {
            MapName = SystemShare.Config.HomeMap;
            CurrX = SystemShare.Config.HomeX;
            CurrY = SystemShare.Config.HomeY;
            BoEmergencyClose = true;
            SendSelfDelayMsg(Messages.RM_MASTERDIEMUTINY, 0, 0, 0, 0, "", 1000);
        }

        public override void RecalcAbilitys()
        {
            base.RecalcAbilitys();
            Luck = 0;
            Luck = (byte)(Luck + AddAbil.Luck);
            Luck = (byte)(Luck - AddAbil.UnLuck);
            if (Race == ActorRace.Play)
            {
                bool mhRing = false;
                bool mhBracelet = false;
                bool mhNecklace = false;
                RecoveryRing = false;
                AngryRing = false;
                MagicShield = false;
                MoXieSuite = 0;
                SuckupEnemyHealthRate = 0;
                SuckupEnemyHealth = 0;
                FastTrain = false;
                HitSpeed = 0;
                bool[] spiritArr = new bool[4] { false, false, false, false };
                bool[] cghi = new bool[4] { false, false, false, false };
                bool shRing = false;
                bool shBracelet = false;
                bool shNecklace = false;
                bool hpRing = false;
                bool hpBracelet = false;
                bool mpRing = false;
                bool mpBracelet = false;
                bool hpmpRing = false;
                bool hpmpBracelet = false;
                bool hppNecklace = false;
                bool hppBracelet = false;
                bool hppRing = false;
                bool choWeapon = false;
                bool choNecklace = false;
                bool choRing = false;
                bool choHelmet = false;
                bool choBracelet = false;
                bool psetNecklace = false;
                bool psetBracelet = false;
                bool psetRing = false;
                bool hsetNecklace = false;
                bool hsetBracelet = false;
                bool hsetRing = false;
                bool ysetNecklace = false;
                bool ysetBracelet = false;
                bool ysetRing = false;
                bool bonesetWeapon = false;
                bool bonesetHelmet = false;
                bool bonesetDress = false;
                bool bugsetNecklace = false;
                bool bugsetRing = false;
                bool bugsetBracelet = false;
                bool ptsetBelt = false;
                bool ptsetBoots = false;
                bool ptsetNecklace = false;
                bool ptsetBracelet = false;
                bool ptsetRing = false;
                bool kssetBelt = false;
                bool kssetBoots = false;
                bool kssetNecklace = false;
                bool kssetBracelet = false;
                bool kssetRing = false;
                bool rubysetBelt = false;
                bool rubysetBoots = false;
                bool rubysetNecklace = false;
                bool rubysetBracelet = false;
                bool rubysetRing = false;
                bool strongPtsetBelt = false;
                bool strongPtsetBoots = false;
                bool strongPtsetNecklace = false;
                bool strongPtsetBracelet = false;
                bool strongPtsetRing = false;
                bool strongKssetBelt = false;
                bool strongKssetBoots = false;
                bool strongKssetNecklace = false;
                bool strongKssetBracelet = false;
                bool strongKssetRing = false;
                bool strongRubysetBelt = false;
                bool strongRubysetBoots = false;
                bool strongRubysetNecklace = false;
                bool strongRubysetBracelet = false;
                bool strongRubysetRing = false;
                bool dragonsetRingLeft = false;
                bool dragonsetRingRight = false;
                bool dragonsetBraceletLeft = false;
                bool dragonsetBraceletRight = false;
                bool dragonsetNecklace = false;
                bool dragonsetDress = false;
                bool dragonsetHelmet = false;
                bool dragonsetWeapon = false;
                bool dragonsetBoots = false;
                bool dragonsetBelt = false;
                bool dsetWingdress = false;
                AddAbility addAbilTemp = AddAbil;
                Ability abilityTemp = WAbil;
                for (int i = 0; i < UseItems.Length; i++)
                {
                    if (UseItems[i] != null && (UseItems[i].Index > 0))
                    {
                        StdItem stdItem;
                        if (UseItems[i].Dura == 0)
                        {
                            stdItem = SystemShare.ItemSystem.GetStdItem(UseItems[i].Index);
                            if (stdItem != null)
                            {
                                if ((i == ItemLocation.Weapon) || (i == ItemLocation.RighThand))
                                {
                                    abilityTemp.HandWeight = (byte)(abilityTemp.HandWeight + stdItem.Weight);
                                }
                                else
                                {
                                    abilityTemp.WearWeight = (byte)(abilityTemp.WearWeight + stdItem.Weight);
                                }
                            }
                            continue;
                        }
                        stdItem = SystemShare.ItemSystem.GetStdItem(UseItems[i].Index);
                        ApplyItemParameters(UseItems[i], stdItem, ref addAbilTemp);
                        ApplyItemParametersEx(UseItems[i], ref abilityTemp);
   
                        if (stdItem != null)
                        {
                            if ((i == ItemLocation.Weapon) || (i == ItemLocation.RighThand))
                            {
                                abilityTemp.HandWeight = (byte)(abilityTemp.HandWeight + stdItem.Weight);
                            }
                            else
                            {
                                abilityTemp.WearWeight = (byte)(abilityTemp.WearWeight + stdItem.Weight);
                            }
                            switch (i)
                            {
                                case ItemLocation.Weapon:
                                case ItemLocation.ArmRingl:
                                case ItemLocation.ArmRingr:
                                    {
                                        if ((stdItem.SpecialPwr <= -1) && (stdItem.SpecialPwr >= -50))
                                        {
                                            addAbilTemp.UndeadPower = (byte)(addAbilTemp.UndeadPower + (-stdItem.SpecialPwr));
                                        }
                                        if ((stdItem.SpecialPwr <= -51) && (stdItem.SpecialPwr >= -100))
                                        {
                                            addAbilTemp.UndeadPower = (byte)(addAbilTemp.UndeadPower + (stdItem.SpecialPwr + 50));
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
                                case ItemLocation.Necklace:
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
                                case ItemLocation.Ringr:
                                case ItemLocation.Ringl:
                                    {
                                        switch (stdItem.Shape)
                                        {
                                            case ItemShapeConst.RING_TRANSPARENT_ITEM:
                                                StatusTimeArr[PoisonState.STATETRANSPARENT] = 60000;
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
                                                    if ((i == ItemLocation.Ringl))
                                                    {
                                                        dragonsetRingLeft = true;
                                                    }
                                                    if ((i == ItemLocation.Ringr))
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
                                case ItemLocation.ArmRingl:
                                case ItemLocation.ArmRingr:
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
                                                    if ((i == ItemLocation.ArmRingl))
                                                    {
                                                        dragonsetBraceletLeft = true;
                                                    }
                                                    if ((i == ItemLocation.ArmRingr))
                                                    {
                                                        dragonsetBraceletRight = true;
                                                    }
                                                    break;
                                                }
                                        }
                                        break;
                                    }
                                case ItemLocation.Helmet:
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
                                case ItemLocation.Dress:
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
                                case ItemLocation.Belt:
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
                                case ItemLocation.Boots:
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
                                case ItemLocation.Charm:
                                    {
                                        if ((stdItem.StdMode == 53) && (stdItem.Shape == ItemShapeConst.SHAPE_OF_LUCKYLADLE))
                                        {
                                            addAbilTemp.Luck = (byte)(HUtil32._MIN(255, addAbilTemp.Luck + 1));
                                        }
                                        break;
                                    }
                            }

                            switch (stdItem.StdMode)
                            {
                                case ItemShapeConst.SpiritItem1:
                                    spiritArr[0] = true;
                                    break;
                                case ItemShapeConst.SpiritItem2:
                                    spiritArr[1] = true;
                                    break;
                                case ItemShapeConst.SpiritItem3:
                                    spiritArr[2] = true;
                                    break;
                                case ItemShapeConst.SpiritItem4:
                                    spiritArr[3] = true;
                                    break;
                            }
                        }
                    }
                }
                if (cghi[0] && cghi[1] && cghi[2] && cghi[3]) //记忆套装
                {
                    RecallSuite = true;
                }
                if (mhNecklace && mhBracelet && mhRing)
                {
                    MoXieSuite = MoXieSuite + 50;
                }
                if (shNecklace && shBracelet && shRing)
                {
                    addAbilTemp.HIT = (ushort)(addAbilTemp.HIT + 2);
                }
                if (hpBracelet && hpRing)
                {
                    addAbilTemp.HP = (ushort)(addAbilTemp.HP + 50);
                }
                if (mpBracelet && mpRing)
                {
                    addAbilTemp.MP = (ushort)(addAbilTemp.MP + 50);
                }
                if (hpmpBracelet && hpmpRing)
                {
                    addAbilTemp.HP = (ushort)(addAbilTemp.HP + 30);
                    addAbilTemp.MP = (ushort)(addAbilTemp.MP + 30);
                }
                if (hppNecklace && hppBracelet && hppRing)
                {
                    addAbilTemp.HP = (ushort)(addAbilTemp.HP + ((abilityTemp.MaxHP * 30) / 100));
                    addAbilTemp.AC = (ushort)(addAbilTemp.AC + HUtil32.MakeWord(2, 2));
                }
                if (choWeapon && choNecklace && choRing && choHelmet && choBracelet)
                {
                    addAbilTemp.HitSpeed = (ushort)(addAbilTemp.HitSpeed + 4);
                    addAbilTemp.DC = (ushort)(addAbilTemp.DC + HUtil32.MakeWord(2, 5));
                }
                if (psetBracelet && psetRing)
                {
                    addAbilTemp.HitSpeed = (ushort)(addAbilTemp.HitSpeed + 2);
                    if (psetNecklace)
                    {
                        addAbilTemp.DC = (ushort)(addAbilTemp.DC + HUtil32.MakeWord(1, 3));
                    }
                }
                if (hsetBracelet && hsetRing)
                {
                    abilityTemp.MaxWeight = (ushort)(abilityTemp.MaxWeight + 20);
                    abilityTemp.MaxWearWeight = (byte)(HUtil32._MIN(255, abilityTemp.MaxWearWeight + 5));
                    if (hsetNecklace)
                    {
                        addAbilTemp.MC = (ushort)(addAbilTemp.MC + HUtil32.MakeWord(1, 2));
                    }
                }
                if (ysetBracelet && ysetRing)
                {
                    addAbilTemp.UndeadPower = (byte)(addAbilTemp.UndeadPower + 3);
                    if (ysetNecklace)
                    {
                        addAbilTemp.SC = (ushort)(addAbilTemp.SC + HUtil32.MakeWord(1, 2));
                    }
                }
                if (bonesetWeapon && bonesetHelmet && bonesetDress)
                {
                    addAbilTemp.AC = (ushort)(addAbilTemp.AC + HUtil32.MakeWord(0, 2));
                    addAbilTemp.MC = (ushort)(addAbilTemp.MC + HUtil32.MakeWord(0, 1));
                    addAbilTemp.SC = (ushort)(addAbilTemp.SC + HUtil32.MakeWord(0, 1));
                }
                if (bugsetNecklace && bugsetRing && bugsetBracelet)
                {
                    addAbilTemp.DC = (ushort)(addAbilTemp.DC + HUtil32.MakeWord(0, 1));
                    addAbilTemp.MC = (ushort)(addAbilTemp.MC + HUtil32.MakeWord(0, 1));
                    addAbilTemp.SC = (ushort)(addAbilTemp.SC + HUtil32.MakeWord(0, 1));
                    addAbilTemp.AntiMagic = (ushort)(addAbilTemp.AntiMagic + 1);
                    addAbilTemp.AntiPoison = (ushort)(addAbilTemp.AntiPoison + 1);
                }
                if (ptsetBelt && ptsetBoots && ptsetNecklace && ptsetBracelet && ptsetRing)
                {
                    addAbilTemp.DC = (ushort)(addAbilTemp.DC + HUtil32.MakeWord(0, 2));
                    addAbilTemp.AC = (ushort)(addAbilTemp.AC + HUtil32.MakeWord(0, 2));
                    abilityTemp.MaxHandWeight = (byte)(HUtil32._MIN(255, abilityTemp.MaxHandWeight + 1));
                    abilityTemp.MaxWearWeight = (byte)(HUtil32._MIN(255, abilityTemp.MaxWearWeight + 2));
                }
                if (kssetBelt && kssetBoots && kssetNecklace && kssetBracelet && kssetRing)
                {
                    addAbilTemp.SC = (ushort)(addAbilTemp.SC + HUtil32.MakeWord(0, 2));
                    addAbilTemp.AC = (ushort)(addAbilTemp.AC + HUtil32.MakeWord(0, 1));
                    addAbilTemp.MAC = (ushort)(addAbilTemp.MAC + HUtil32.MakeWord(0, 1));
                    addAbilTemp.SPEED = (ushort)(addAbilTemp.SPEED + 1);
                    abilityTemp.MaxHandWeight = (byte)(HUtil32._MIN(255, abilityTemp.MaxHandWeight + 1));
                    abilityTemp.MaxWearWeight = (byte)(HUtil32._MIN(255, abilityTemp.MaxWearWeight + 2));
                }
                if (rubysetBelt && rubysetBoots && rubysetNecklace && rubysetBracelet && rubysetRing)
                {
                    addAbilTemp.MC = (ushort)(addAbilTemp.MC + HUtil32.MakeWord(0, 2));
                    addAbilTemp.MAC = (ushort)(addAbilTemp.MAC + HUtil32.MakeWord(0, 2));
                    abilityTemp.MaxHandWeight = (byte)(HUtil32._MIN(255, abilityTemp.MaxHandWeight + 1));
                    abilityTemp.MaxWearWeight = (byte)(HUtil32._MIN(255, abilityTemp.MaxWearWeight + 2));
                }
                if (strongPtsetBelt && strongPtsetBoots && strongPtsetNecklace && strongPtsetBracelet && strongPtsetRing)
                {
                    addAbilTemp.DC = (ushort)(addAbilTemp.DC + HUtil32.MakeWord(0, 3));
                    addAbilTemp.HP = (ushort)(addAbilTemp.HP + 30);
                    addAbilTemp.HitSpeed = (ushort)(addAbilTemp.HitSpeed + 2);
                    abilityTemp.MaxWearWeight = (byte)(HUtil32._MIN(255, abilityTemp.MaxWearWeight + 2));
                }
                if (strongKssetBelt && strongKssetBoots && strongKssetNecklace && strongKssetBracelet && strongKssetRing)
                {
                    addAbilTemp.SC = (ushort)(addAbilTemp.SC + HUtil32.MakeWord(0, 2));
                    addAbilTemp.HP = (ushort)(addAbilTemp.HP + 15);
                    addAbilTemp.MP = (ushort)(addAbilTemp.MP + 20);
                    addAbilTemp.UndeadPower = (byte)(addAbilTemp.UndeadPower + 1);
                    addAbilTemp.HIT = (ushort)(addAbilTemp.HIT + 1);
                    addAbilTemp.SPEED = (ushort)(addAbilTemp.SPEED + 1);
                    abilityTemp.MaxWearWeight = (byte)(HUtil32._MIN(255, abilityTemp.MaxWearWeight + 2));
                }
                if (strongRubysetBelt && strongRubysetBoots && strongRubysetNecklace && strongRubysetBracelet && strongRubysetRing)
                {
                    addAbilTemp.MC = (ushort)(addAbilTemp.MC + HUtil32.MakeWord(0, 2));
                    addAbilTemp.MP = (ushort)(addAbilTemp.MP + 40);
                    addAbilTemp.SPEED = (ushort)(addAbilTemp.SPEED + 2);
                    abilityTemp.MaxWearWeight = (byte)(HUtil32._MIN(255, abilityTemp.MaxWearWeight + 2));
                }
                if (dragonsetRingLeft && dragonsetRingRight && dragonsetBraceletLeft && dragonsetBraceletRight && dragonsetNecklace && dragonsetDress && dragonsetHelmet && dragonsetWeapon && dragonsetBoots && dragonsetBelt)
                {
                    addAbilTemp.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(addAbilTemp.AC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.AC) + 4));
                    addAbilTemp.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(addAbilTemp.MAC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.MAC) + 4));
                    addAbilTemp.Luck = (byte)(HUtil32._MIN(255, addAbilTemp.Luck + 2));
                    addAbilTemp.HitSpeed = (ushort)(addAbilTemp.HitSpeed + 2);
                    addAbilTemp.AntiMagic = (ushort)(addAbilTemp.AntiMagic + 6);
                    addAbilTemp.AntiPoison = (ushort)(addAbilTemp.AntiPoison + 6);
                    abilityTemp.MaxHandWeight = (byte)(HUtil32._MIN(255, abilityTemp.MaxHandWeight + 34));
                    abilityTemp.MaxWearWeight = (byte)(HUtil32._MIN(255, abilityTemp.MaxWearWeight + 27));
                    abilityTemp.MaxWeight = (ushort)(abilityTemp.MaxWeight + 120);
                    abilityTemp.MaxHP = (ushort)(abilityTemp.MaxHP + 70);
                    abilityTemp.MaxMP = (ushort)(abilityTemp.MaxMP + 80);
                    addAbilTemp.SPEED = (ushort)(addAbilTemp.SPEED + 1);
                    addAbilTemp.DC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(addAbilTemp.DC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.DC) + 4));
                    addAbilTemp.MC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(addAbilTemp.MC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.MC) + 3));
                    addAbilTemp.SC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(addAbilTemp.SC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.SC) + 3));
                }
                else
                {
                    if (dragonsetDress && dragonsetHelmet && dragonsetWeapon && dragonsetBoots && dragonsetBelt)
                    {
                        abilityTemp.MaxHandWeight = (byte)(HUtil32._MIN(255, abilityTemp.MaxHandWeight + 34));
                        abilityTemp.MaxWeight = (ushort)(abilityTemp.MaxWeight + 50);
                        addAbilTemp.SPEED = (ushort)(addAbilTemp.SPEED + 1);
                        addAbilTemp.DC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(addAbilTemp.DC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.DC) + 4));
                        addAbilTemp.MC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(addAbilTemp.MC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.MC) + 3));
                        addAbilTemp.SC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(addAbilTemp.SC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.SC) + 3));
                    }
                    else if (dragonsetDress && dragonsetBoots && dragonsetBelt)
                    {
                        abilityTemp.MaxHandWeight = (byte)(HUtil32._MIN(255, abilityTemp.MaxHandWeight + 17));
                        abilityTemp.MaxWeight = (ushort)(abilityTemp.MaxWeight + 30);
                        addAbilTemp.DC = HUtil32.MakeWord(HUtil32.LoByte(addAbilTemp.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.DC) + 1));
                        addAbilTemp.MC = HUtil32.MakeWord(HUtil32.LoByte(addAbilTemp.MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.MC) + 1));
                        addAbilTemp.SC = HUtil32.MakeWord(HUtil32.LoByte(addAbilTemp.SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.SC) + 1));
                    }
                    else if (dragonsetDress && dragonsetHelmet && dragonsetWeapon)
                    {
                        addAbilTemp.DC = HUtil32.MakeWord(HUtil32.LoByte(addAbilTemp.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.DC) + 2));
                        addAbilTemp.MC = HUtil32.MakeWord(HUtil32.LoByte(addAbilTemp.MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.MC) + 1));
                        addAbilTemp.SC = HUtil32.MakeWord(HUtil32.LoByte(addAbilTemp.SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.SC) + 1));
                        addAbilTemp.SPEED = (ushort)(addAbilTemp.SPEED + 1);
                    }
                    if (dragonsetRingLeft && dragonsetRingRight && dragonsetBraceletLeft && dragonsetBraceletRight && dragonsetNecklace)
                    {
                        abilityTemp.MaxWearWeight = (byte)(HUtil32._MIN(255, abilityTemp.MaxWearWeight + 27));
                        abilityTemp.MaxWeight = (ushort)(abilityTemp.MaxWeight + 50);
                        addAbilTemp.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(addAbilTemp.AC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.AC) + 3));
                        addAbilTemp.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(addAbilTemp.MAC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.MAC) + 3));
                    }
                    else if ((dragonsetRingLeft || dragonsetRingRight) && dragonsetBraceletLeft && dragonsetBraceletRight && dragonsetNecklace)
                    {
                        abilityTemp.MaxWearWeight = (byte)(HUtil32._MIN(255, abilityTemp.MaxWearWeight + 17));
                        abilityTemp.MaxWeight = (ushort)(abilityTemp.MaxWeight + 30);
                        addAbilTemp.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(addAbilTemp.AC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.AC) + 1));
                        addAbilTemp.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(addAbilTemp.MAC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.MAC) + 1));
                    }
                    else if (dragonsetRingLeft && dragonsetRingRight && (dragonsetBraceletLeft || dragonsetBraceletRight) && dragonsetNecklace)
                    {
                        abilityTemp.MaxWearWeight = (byte)(HUtil32._MIN(255, abilityTemp.MaxWearWeight + 17));
                        abilityTemp.MaxWeight = (ushort)(abilityTemp.MaxWeight + 30);
                        addAbilTemp.AC = HUtil32.MakeWord(HUtil32.LoByte(addAbilTemp.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.AC) + 2));
                        addAbilTemp.MAC = HUtil32.MakeWord(HUtil32.LoByte(addAbilTemp.MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.MAC) + 2));
                    }
                    else if ((dragonsetRingLeft || dragonsetRingRight) && (dragonsetBraceletLeft || dragonsetBraceletRight) && dragonsetNecklace)
                    {
                        abilityTemp.MaxWearWeight = (byte)(HUtil32._MIN(255, abilityTemp.MaxWearWeight + 17));
                        abilityTemp.MaxWeight = (ushort)(abilityTemp.MaxWeight + 30);
                        addAbilTemp.AC = HUtil32.MakeWord(HUtil32.LoByte(addAbilTemp.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.AC) + 1));
                        addAbilTemp.MAC = HUtil32.MakeWord(HUtil32.LoByte(addAbilTemp.MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.MAC) + 1));
                    }
                    else
                    {
                        if (dragonsetBraceletLeft && dragonsetBraceletRight)
                        {
                            addAbilTemp.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(addAbilTemp.AC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.AC)));
                            addAbilTemp.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(addAbilTemp.MAC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.MAC)));
                        }
                        if (dragonsetRingLeft && dragonsetRingRight)
                        {
                            addAbilTemp.AC = HUtil32.MakeWord(HUtil32.LoByte(addAbilTemp.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.AC) + 1));
                            addAbilTemp.MAC = HUtil32.MakeWord(HUtil32.LoByte(addAbilTemp.MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(addAbilTemp.MAC) + 1));
                        }
                    }
                }
                if (dsetWingdress && (Abil.Level >= 20))
                {
                    switch (Abil.Level)
                    {
                        case < 40:
                            addAbilTemp.DC = (ushort)(addAbilTemp.DC + HUtil32.MakeWord(0, 1));
                            addAbilTemp.MC = (ushort)(addAbilTemp.MC + HUtil32.MakeWord(0, 2));
                            addAbilTemp.SC = (ushort)(addAbilTemp.SC + HUtil32.MakeWord(0, 2));
                            addAbilTemp.AC = (ushort)(addAbilTemp.AC + HUtil32.MakeWord(2, 3));
                            addAbilTemp.MAC = (ushort)(addAbilTemp.MAC + HUtil32.MakeWord(0, 2));
                            break;
                        case < 50:
                            addAbilTemp.DC = (ushort)(addAbilTemp.DC + HUtil32.MakeWord(0, 3));
                            addAbilTemp.MC = (ushort)(addAbilTemp.MC + HUtil32.MakeWord(0, 4));
                            addAbilTemp.SC = (ushort)(addAbilTemp.SC + HUtil32.MakeWord(0, 4));
                            addAbilTemp.AC = (ushort)(addAbilTemp.AC + HUtil32.MakeWord(5, 5));
                            addAbilTemp.MAC = (ushort)(addAbilTemp.MAC + HUtil32.MakeWord(1, 2));
                            break;
                        default:
                            addAbilTemp.DC = (ushort)(addAbilTemp.DC + HUtil32.MakeWord(0, 5));
                            addAbilTemp.MC = (ushort)(addAbilTemp.MC + HUtil32.MakeWord(0, 6));
                            addAbilTemp.SC = (ushort)(addAbilTemp.SC + HUtil32.MakeWord(0, 6));
                            addAbilTemp.AC = (ushort)(addAbilTemp.AC + HUtil32.MakeWord(9, 7));
                            addAbilTemp.MAC = (ushort)(addAbilTemp.MAC + HUtil32.MakeWord(2, 4));
                            break;
                    }
                }
                abilityTemp.Weight = RecalcBagWeight();

                if (FlameRing)
                {
                    AddItemSkill(Settings.AM_FIREBALL);
                }
                else
                {
                    DelItemSkill(Settings.AM_FIREBALL);
                }
                if (RecoveryRing)
                {
                    AddItemSkill(Settings.AM_HEALING);
                }
                else
                {
                    DelItemSkill(Settings.AM_HEALING);
                }
                if (MuscleRing)
                {
                    abilityTemp.MaxWeight = (ushort)(abilityTemp.MaxWeight * 2);
                    abilityTemp.MaxWearWeight = (byte)HUtil32._MIN(255, abilityTemp.MaxWearWeight * 2);
                    if ((abilityTemp.MaxHandWeight * 2 > 255))
                    {
                        abilityTemp.MaxHandWeight = 255;
                    }
                    else
                    {
                        abilityTemp.MaxHandWeight = (byte)(abilityTemp.MaxHandWeight * 2);
                    }
                }
                if (MoXieSuite > 0) //魔血套装
                {
                    if (MoXieSuite >= abilityTemp.MaxMP)
                    {
                        MoXieSuite = abilityTemp.MaxMP - 1;
                    }
                    abilityTemp.MaxMP = (ushort)(abilityTemp.MaxMP - MoXieSuite);
                    abilityTemp.MaxHP = (ushort)(abilityTemp.MaxHP + MoXieSuite);
                    if ((Race == ActorRace.Play) && (abilityTemp.HP > abilityTemp.MaxHP))
                    {
                        abilityTemp.HP = abilityTemp.MaxHP;
                    }
                }
                if (spiritArr[0] && spiritArr[2] && spiritArr[3] && spiritArr[4]) //祈祷套装
                {
                    IsSpirit = true;
                }
                if ((Race == ActorRace.Play) && (abilityTemp.HP > abilityTemp.MaxHP) && (!mhNecklace && !mhBracelet && !mhRing))
                {
                    abilityTemp.HP = abilityTemp.MaxHP;
                }
                if ((Race == ActorRace.Play) && (abilityTemp.MP > abilityTemp.MaxMP))
                {
                    abilityTemp.MP = abilityTemp.MaxMP;
                }
                if (ExtraAbil[AbilConst.EABIL_DCUP] > 0)
                {
                    abilityTemp.DC = HUtil32.MakeWord(HUtil32.LoByte(abilityTemp.DC), (ushort)(HUtil32.HiByte(abilityTemp.DC) + ExtraAbil[AbilConst.EABIL_DCUP]));
                }
                if (ExtraAbil[AbilConst.EABIL_MCUP] > 0)
                {
                    abilityTemp.MC = HUtil32.MakeWord(HUtil32.LoByte(abilityTemp.MC), (ushort)(HUtil32.HiByte(abilityTemp.MC) + ExtraAbil[AbilConst.EABIL_MCUP]));
                }
                if (ExtraAbil[AbilConst.EABIL_SCUP] > 0)
                {
                    abilityTemp.SC = HUtil32.MakeWord(HUtil32.LoByte(abilityTemp.SC), (ushort)(HUtil32.HiByte(abilityTemp.SC) + ExtraAbil[AbilConst.EABIL_SCUP]));
                }
                if (ExtraAbil[AbilConst.EABIL_HITSPEEDUP] > 0)
                {
                    HitSpeed = (ushort)(HitSpeed + ExtraAbil[AbilConst.EABIL_HITSPEEDUP]);
                }
                if (ExtraAbil[AbilConst.EABIL_HPUP] > 0)
                {
                    abilityTemp.MaxHP = (ushort)(abilityTemp.MaxHP + ExtraAbil[AbilConst.EABIL_HPUP]);
                }
                if (ExtraAbil[AbilConst.EABIL_MPUP] > 0)
                {
                    abilityTemp.MaxMP = (ushort)(abilityTemp.MaxMP + ExtraAbil[AbilConst.EABIL_MPUP]);
                }
                if (ExtraAbil[AbilConst.EABIL_PWRRATE] > 0)
                {
                    abilityTemp.DC = HUtil32.MakeWord((ushort)((HUtil32.LoByte(abilityTemp.DC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100), (ushort)((HUtil32.HiByte(abilityTemp.DC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100));
                    abilityTemp.MC = HUtil32.MakeWord((ushort)((HUtil32.LoByte(abilityTemp.MC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100), (ushort)((HUtil32.HiByte(abilityTemp.MC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100));
                    abilityTemp.SC = HUtil32.MakeWord((ushort)((HUtil32.LoByte(abilityTemp.SC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100), (ushort)((HUtil32.HiByte(abilityTemp.SC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100));
                }
                if (Race == ActorRace.Play)
                {
                    bool fastmoveflag = UseItems[ItemLocation.Boots] != null && UseItems[ItemLocation.Boots].Dura > 0 && UseItems[ItemLocation.Boots].Index == Settings.INDEX_MIRBOOTS;
                    if (fastmoveflag)
                    {
                        StatusTimeArr[PoisonState.FASTMOVE] = 60000;
                    }
                    else
                    {
                        StatusTimeArr[PoisonState.FASTMOVE] = 0;
                    }
                    //if ((Abil.Level >= EfftypeConst.EFFECTIVE_HIGHLEVEL))
                    //{
                    //    if (BoHighLevelEffect)
                    //    {
                    //        StatusTimeArr[Grobal2.STATE_50LEVELEFFECT] = 60000;
                    //    }
                    //    else
                    //    {
                    //        StatusTimeArr[Grobal2.STATE_50LEVELEFFECT] = 0;
                    //    }
                    //}
                    //else
                    //{
                    //    StatusTimeArr[Grobal2.STATE_50LEVELEFFECT] = 0;
                    //}
                    CharStatus = GetCharStatus();
                    StatusChanged();
                    SendUpdateMsg(Messages.RM_CHARSTATUSCHANGED, HitSpeed, CharStatus, 0, 0, "");
                }
                RecalcAdjusBonus();



                byte oldlight = Light;
                Light = GetMyLight();
                if (oldlight != Light)
                {
                    SendRefMsg(Messages.RM_CHANGELIGHT, 0, 0, 0, 0, "");
                }
                if (IsSpirit)
                {
                    SendSelfDelayMsg(Messages.RM_SPIRITSUITE, 0, 0, 0, 0, "", 500);
                }
                SpeedPoint = (byte)(SpeedPoint + AddAbil.SPEED);
                HitPoint = (byte)(HitPoint + AddAbil.HIT);
                AntiPoison = (byte)(AntiPoison + AddAbil.AntiPoison);
                PoisonRecover = (ushort)(PoisonRecover + AddAbil.PoisonRecover);
                HealthRecover = (ushort)(HealthRecover + AddAbil.HealthRecover);
                SpellRecover = (ushort)(SpellRecover + AddAbil.SpellRecover);
                AntiMagic = (ushort)(AntiMagic + AddAbil.AntiMagic);
                Luck = (byte)(Luck + AddAbil.Luck);
                Luck = (byte)(Luck - AddAbil.UnLuck);
                HitSpeed = AddAbil.HitSpeed;
                abilityTemp.MaxHP = (ushort)(Abil.MaxHP + AddAbil.HP);
                abilityTemp.MaxMP = (ushort)(Abil.MaxMP + AddAbil.MP);
                abilityTemp.AC = HUtil32.MakeWord((byte)HUtil32._MIN(255, HUtil32.LoByte(AddAbil.AC) + HUtil32.LoByte(Abil.AC)), (byte)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + HUtil32.HiByte(Abil.AC)));
                abilityTemp.MAC = HUtil32.MakeWord((byte)HUtil32._MIN(255, HUtil32.LoByte(AddAbil.MAC) + HUtil32.LoByte(Abil.MAC)), (byte)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + HUtil32.HiByte(Abil.MAC)));
                abilityTemp.DC = HUtil32.MakeWord((byte)HUtil32._MIN(255, HUtil32.LoByte(AddAbil.DC) + HUtil32.LoByte(Abil.DC)), (byte)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.DC) + HUtil32.HiByte(Abil.DC)));
                abilityTemp.MC = HUtil32.MakeWord((byte)HUtil32._MIN(255, HUtil32.LoByte(AddAbil.MC) + HUtil32.LoByte(Abil.MC)), (byte)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MC) + HUtil32.HiByte(Abil.MC)));
                abilityTemp.SC = HUtil32.MakeWord((byte)HUtil32._MIN(255, HUtil32.LoByte(AddAbil.SC) + HUtil32.LoByte(Abil.SC)), (byte)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.SC) + HUtil32.HiByte(Abil.SC)));

                AddAbil = addAbilTemp;
                WAbil = abilityTemp;
            }
        }

        public override int GetAttackPower(int nBasePower, int nPower)
        {
            if (nPower < 0)
            {
                nPower = 0;
            }
            int result = 0;
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
                if (Luck <= 0)
                {
                    if (M2Share.RandomNumber.Random(10 - HUtil32._MAX(0, -Luck)) == 0)
                    {
                        result = nBasePower;
                    }
                }
            }
            result = HUtil32.Round(result * (PowerRate / 100.0));
            if (BoPowerItem)
            {
                result = HUtil32.Round(PowerItem * (double)result);
            }
            if (AutoChangeColor)
            {
                result = result * AutoChangeIdx + 1;
            }
            if (FixColor)
            {
                result = result * FixColorIdx + 1;
            }
            return result;
        }

        public override void StruckDamage(int nDamage)
        {
            base.StruckDamage(nDamage);
            ushort nDam = (ushort)(M2Share.RandomNumber.Random(10) + 5);
            if (StatusTimeArr[PoisonState.DAMAGEARMOR] > 0)
            {
                nDam = (ushort)HUtil32.Round(nDam * (SystemShare.Config.PosionDamagarmor / 10.0)); // 1.2
            }
            bool boRecalcAbi = false;
            ushort nDura;
            int nOldDura;
            if (UseItems[ItemLocation.Dress] != null && UseItems[ItemLocation.Dress].Index > 0)
            {
                nDura = UseItems[ItemLocation.Dress].Dura;
                nOldDura = HUtil32.Round(nDura / 1000.0);
                nDura -= nDam;
                if (nDura <= 0)
                {
                    SendDelItems(UseItems[ItemLocation.Dress]);
                    StdItem stdItem = SystemShare.ItemSystem.GetStdItem(UseItems[ItemLocation.Dress].Index);
                    if (stdItem.NeedIdentify == 1)
                    {
                        //M2Share.EventSource.AddEventLog(3, MapName + "\t" + CurrX + "\t" + CurrY + "\t" +
                        //                                   ChrName + "\t" + stdItem.Name + "\t" +
                        //                                   UseItems[ItemLocation.Dress].MakeIndex + "\t"
                        //                                   + HUtil32.BoolToIntStr(Race == ActorRace.Play) +
                        //                                   "\t" + '0');
                    }
                    UseItems[ItemLocation.Dress].Index = 0;
                    FeatureChanged();
                    UseItems[ItemLocation.Dress].Index = 0;
                    UseItems[ItemLocation.Dress].Dura = 0;
                    boRecalcAbi = true;
                }
                else
                {
                    UseItems[ItemLocation.Dress].Dura = nDura;
                }
                if (nOldDura != HUtil32.Round(nDura / 1000.0))
                {
                    SendMsg(Messages.RM_DURACHANGE, ItemLocation.Dress, nDura, UseItems[ItemLocation.Dress].DuraMax, 0);
                }
            }

            for (int i = 0; i < UseItems.Length; i++)
            {
                if ((UseItems[i] != null) && (UseItems[i].Index > 0) && (M2Share.RandomNumber.Random(8) == 0))
                {
                    nDura = UseItems[i].Dura;
                    nOldDura = HUtil32.Round(nDura / 1000.0);
                    nDura -= nDam;
                    if (nDura <= 0)
                    {
                        SendDelItems(UseItems[i]);
                        StdItem stdItem = SystemShare.ItemSystem.GetStdItem(UseItems[i].Index);
                        if (stdItem.NeedIdentify == 1)
                        {
                            //M2Share.EventSource.AddEventLog(3, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + stdItem.Name + "\t" +
                            //                       UseItems[i].MakeIndex + "\t" + HUtil32.BoolToIntStr(Race == ActorRace.Play) + "\t" + '0');
                        }
                        UseItems[i].Index = 0;
                        FeatureChanged();
                        UseItems[i].Index = 0;
                        UseItems[i].Dura = 0;
                        boRecalcAbi = true;
                    }
                    else
                    {
                        UseItems[i].Dura = nDura;
                    }
                    if (nOldDura != HUtil32.Round(nDura / 1000.0))
                    {
                        SendMsg(Messages.RM_DURACHANGE, i, nDura, UseItems[i].DuraMax, 0);
                    }
                }
            }
            if (boRecalcAbi)
            {
                RecalcAbilitys();
                SendMsg(Messages.RM_ABILITY, 0, 0, 0, 0);
                SendMsg(Messages.RM_SUBABILITY, 0, 0, 0, 0);
            }
        }

        /// <summary>
        /// 更新玩家自身可见的玩家和怪物
        /// </summary>
        /// <param name="baseObject"></param>
        public override void UpdateVisibleGay(IActor baseObject)
        {
            bool boIsVisible = false;
            VisibleBaseObject visibleBaseObject;
            if (baseObject.Race == ActorRace.Play || baseObject.Master != null)
            {
                IsVisibleActive = true;// 如果是人物或宝宝则置TRUE
            }
            for (int i = 0; i < VisibleActors.Count; i++)
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
                VisibleFlag = VisibleFlag.Show,
                BaseObject = baseObject
            };
            VisibleActors.Add(visibleBaseObject);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void SearchViewRange()
        {
            for (int i = VisibleItems.Count - 1; i >= 0; i--)
            {
                VisibleItems[i].VisibleFlag = VisibleFlag.Hidden;
            }
            for (int i = VisibleEvents.Count - 1; i >= 0; i--)
            {
                VisibleEvents[i].VisibleFlag = VisibleFlag.Hidden;
            }
            for (int i = VisibleActors.Count - 1; i >= 0; i--)
            {
                VisibleActors[i].VisibleFlag = VisibleFlag.Hidden;
            }
            short nStartX = (short)(CurrX - ViewRange);
            short nEndX = (short)(CurrX + ViewRange);
            short nStartY = (short)(CurrY - ViewRange);
            short nEndY = (short)(CurrY + ViewRange);
            try
            {
                //todo 需要要优化整个方法
                for (short nX = nStartX; nX <= nEndX; nX++)
                {
                    for (short nY = nStartY; nY <= nEndY; nY++)
                    {
                        ref MapCellInfo cellInfo = ref Envir.GetCellInfo(nX, nY, out bool cellSuccess);
                        if (cellSuccess && cellInfo.IsAvailable)
                        {
                            var nIdx = 0;
                            while (true)
                            {
                                if (cellInfo.Count <= nIdx)
                                {
                                    break;
                                }
                                CellObject cellObject = cellInfo.ObjList[nIdx];
                                switch (cellObject.CellType)
                                {
                                    case CellType.Play:
                                    case CellType.Monster:
                                    case CellType.Merchant:
                                        //if ((HUtil32.GetTickCount() - cellObject.AddTime) >= 60 * 1000)
                                        //{
                                        //    cellInfo.Remove(nIdx);
                                        //    if (cellInfo.Count > 0)
                                        //    {
                                        //        continue;
                                        //    }
                                        //    cellInfo.Clear();
                                        //    break;
                                        //}
                                        IActor baseObject = SystemShare.ActorMgr.Get(cellObject.CellObjId);
                                        if (baseObject != null && !baseObject.Invisible)
                                        {
                                            if (!baseObject.Ghost && !baseObject.FixedHideMode && !baseObject.ObMode)
                                            {
                                                if (Race < ActorRace.Animal || Master != null || WantRefMsg || baseObject.Master != null && Math.Abs(baseObject.CurrX - CurrX) <= 3 && Math.Abs(baseObject.CurrY - CurrY) <= 3 || baseObject.Race == ActorRace.Play)
                                                {
                                                    UpdateVisibleGay(baseObject);//更新自己的视野对象
                                                    if (baseObject.CellType == CellType.Monster && !ObMode && !FixedHideMode) //进入附近怪物视野
                                                    {
                                                        if (Math.Abs(baseObject.CurrX - CurrX) <= (ViewRange - baseObject.ViewRange) && Math.Abs(baseObject.CurrY - CurrY) <= (ViewRange - baseObject.ViewRange))
                                                        {
                                                            SystemShare.ActorMgr.SendMessage(baseObject.ActorId, Messages.RM_UPDATEVIEWRANGE, this.ActorId, 0, 0, 0, "");// 发送消息更新对方的视野
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case CellType.Item:
                                        if ((HUtil32.GetTickCount() - cellObject.AddTime) > SystemShare.Config.ClearDropOnFloorItemTime)// 60 * 60 * 1000
                                        {
                                            cellInfo.Remove(nIdx);
                                            if (cellInfo.Count > 0)
                                            {
                                                continue;
                                            }
                                            cellInfo.Clear();
                                            break;
                                        }
                                        MapItem mapItem = M2Share.CellObjectMgr.Get<MapItem>(cellObject.CellObjId);
                                        if (mapItem.ItemId == 0)
                                        {
                                            continue;
                                        }
                                        UpdateVisibleItem(nX, nY, mapItem);
                                        if (mapItem.OfBaseObject > 0 || mapItem.DropBaseObject > 0)
                                        {
                                            if ((HUtil32.GetTickCount() - mapItem.CanPickUpTick) > SystemShare.Config.FloorItemCanPickUpTime)// 2 * 60 * 1000
                                            {
                                                mapItem.OfBaseObject = 0;
                                                mapItem.DropBaseObject = 0;
                                            }
                                            else
                                            {
                                                if (SystemShare.ActorMgr.Get(mapItem.OfBaseObject) != null)
                                                {
                                                    if (SystemShare.ActorMgr.Get(mapItem.OfBaseObject).Ghost)
                                                    {
                                                        mapItem.OfBaseObject = 0;
                                                    }
                                                }
                                                if (SystemShare.ActorMgr.Get(mapItem.DropBaseObject) != null)
                                                {
                                                    if (SystemShare.ActorMgr.Get(mapItem.DropBaseObject).Ghost)
                                                    {
                                                        mapItem.DropBaseObject = 0;
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case CellType.Event:
                                        MapEvent mapEvent = M2Share.CellObjectMgr.Get<MapEvent>(cellObject.CellObjId);
                                        if (mapEvent == null)
                                        {
                                            continue;
                                        }
                                        if (mapEvent.Visible)
                                        {
                                            UpdateVisibleEvent(nX, nY, mapEvent);
                                        }
                                        break;
                                }
                                nIdx++;
                            }
                        }
                    }
                }
                int n18 = 0;
                while (true)
                {
                    if (VisibleActors.Count <= n18)
                    {
                        break;
                    }
                    VisibleBaseObject visibleBaseObject = VisibleActors[n18];
                    if (visibleBaseObject.VisibleFlag == VisibleFlag.Hidden)
                    {
                        IActor baseObject = visibleBaseObject.BaseObject;
                        if (!baseObject.FixedHideMode && !baseObject.Ghost)//防止人物退出时发送重复的消息占用带宽，人物进入隐身模式时人物不消失问题
                        {
                            SendMsg(baseObject, Messages.RM_DISAPPEAR, 0, 0, 0, 0);
                        }
                        VisibleActors.RemoveAt(n18);
                        Dispose(visibleBaseObject);
                        continue;
                    }
                    if (visibleBaseObject.VisibleFlag == VisibleFlag.Show)
                    {
                        IActor baseObject = visibleBaseObject.BaseObject;
                        if (baseObject != this)
                        {
                            if (baseObject.Death)
                            {
                                if (baseObject.Skeleton)
                                {
                                    SendMsg(baseObject, Messages.RM_SKELETON, baseObject.Dir, baseObject.CurrX, baseObject.CurrY, 0);
                                }
                                else
                                {
                                    SendMsg(baseObject, Messages.RM_DEATH, baseObject.Dir, baseObject.CurrX, baseObject.CurrY, 0);
                                }
                            }
                            else
                            {
                                SendMsg(baseObject, Messages.RM_TURN, baseObject.Dir, baseObject.CurrX, baseObject.CurrY, 0, baseObject.GetShowName());
                            }
                        }
                    }
                    n18++;
                }

                int I = 0;
                while (true)
                {
                    if (VisibleItems.Count <= I)
                    {
                        break;
                    }
                    VisibleMapItem visibleMapItem = VisibleItems[I];
                    if (visibleMapItem.VisibleFlag == VisibleFlag.Hidden)
                    {
                        SendMsg(Messages.RM_ITEMHIDE, 0, visibleMapItem.MapItem.ItemId, visibleMapItem.nX, visibleMapItem.nY);
                        VisibleItems.RemoveAt(I);
                        Dispose(visibleMapItem);
                        continue;
                    }
                    if (visibleMapItem.VisibleFlag == VisibleFlag.Show)
                    {
                        SendMsg(Messages.RM_ITEMSHOW, visibleMapItem.wLooks, visibleMapItem.MapItem.ItemId, visibleMapItem.nX, visibleMapItem.nY, visibleMapItem.sName);
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
                    MapEvent mapEvent = VisibleEvents[I];
                    if (mapEvent.VisibleFlag == VisibleFlag.Hidden)
                    {
                        SendMsg(Messages.RM_HIDEEVENT, 0, mapEvent.Id, mapEvent.nX, mapEvent.nY);
                        VisibleEvents.RemoveAt(I);
                        continue;
                    }
                    if (mapEvent.VisibleFlag == VisibleFlag.Show)
                    {
                        SendMsg(Messages.RM_SHOWEVENT, (short)mapEvent.EventType, mapEvent.Id, HUtil32.MakeLong(mapEvent.nX, (short)mapEvent.EventParam), mapEvent.nY);
                    }
                    I++;
                }
            }
            catch (Exception e)
            {
                M2Share.Logger.Error(e.StackTrace);
                KickException();
            }
        }

        /// <summary>
        /// 显示玩家名字
        /// </summary>
        /// <returns></returns>
        public override string GetShowName()
        {
            string result = string.Empty;
            string sChrName = string.Empty;
            string sGuildName = string.Empty;
            string sDearName = string.Empty;
            string sMasterName = string.Empty;
            const string sExceptionMsg = "[Exception] PlayObject::GetShowName";
            try
            {
                if (MyGuild != null)
                {
                    IUserCastle castle = SystemShare.CastleMgr.IsCastleMember(this);
                    if (castle != null)
                    {
                        sGuildName = Settings.CastleGuildName.Replace("%castlename", castle.sName);
                        sGuildName = sGuildName.Replace("%guildname", MyGuild.GuildName);
                        sGuildName = sGuildName.Replace("%rankname", GuildRankName);
                    }
                    else
                    {
                        castle = SystemShare.CastleMgr.InCastleWarArea(this);// 01/25 多城堡
                        if (SystemShare.Config.ShowGuildName || castle != null && castle.UnderWar || InGuildWarArea)
                        {
                            sGuildName = Settings.NoCastleGuildName.Replace("%guildname", MyGuild.GuildName);
                            sGuildName = sGuildName.Replace("%rankname", GuildRankName);
                        }
                    }
                }
                if (!SystemShare.Config.ShowRankLevelName)
                {
                    if (ReLevel > 0)
                    {
                        switch (Job)
                        {
                            case PlayJob.Warrior:
                                sChrName = Settings.WarrReNewName.Replace("%chrname", ChrName);
                                break;
                            case PlayJob.Wizard:
                                sChrName = Settings.WizardReNewName.Replace("%chrname", ChrName);
                                break;
                            case PlayJob.Taoist:
                                sChrName = Settings.TaosReNewName.Replace("%chrname", ChrName);
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
                    sChrName = Format(RankLevelName, ChrName);
                }
                if (!string.IsNullOrEmpty(MasterName))
                {
                    if (IsMaster)
                    {
                        sMasterName = Format(Settings.MasterName, MasterName);
                    }
                    else
                    {
                        sMasterName = Format(Settings.NoMasterName, MasterName);
                    }
                }
                if (!string.IsNullOrEmpty(DearName))
                {
                    if (Gender == PlayGender.Man)
                    {
                        sDearName = Format(Settings.ManDearName, DearName);
                    }
                    else
                    {
                        sDearName = Format(Settings.WoManDearName, DearName);
                    }
                }
                string sShowName = Settings.HumanShowName.Replace("%chrname", sChrName);
                sShowName = sShowName.Replace("%guildname", sGuildName);
                sShowName = sShowName.Replace("%dearname", sDearName);
                sShowName = sShowName.Replace("%mastername", sMasterName);
                result = sShowName;
            }
            catch (Exception e)
            {
                M2Share.Logger.Error(sExceptionMsg);
                M2Share.Logger.Error(e.Message);
            }
            return result;
        }

        /// <summary>
        /// 计算角色外形代码
        /// </summary>
        /// <returns></returns>
        public override int GetFeature(IActor baseObject)
        {
            byte nDress = 0;
            StdItem stdItem;
            if (UseItems[ItemLocation.Dress] != null && UseItems[ItemLocation.Dress].Index > 0) // 衣服
            {
                stdItem = SystemShare.ItemSystem.GetStdItem(UseItems[ItemLocation.Dress].Index);
                if (stdItem != null)
                {
                    nDress = (byte)(stdItem.Shape * 2);
                }
            }
            PlayGender playGender = Gender;
            nDress += (byte)playGender;
            byte nWeapon = (byte)playGender;
            if (UseItems[ItemLocation.Weapon] != null && UseItems[ItemLocation.Weapon].Index > 0) // 武器
            {
                stdItem = SystemShare.ItemSystem.GetStdItem(UseItems[ItemLocation.Weapon].Index);
                if (stdItem != null)
                {
                    nWeapon += (byte)(stdItem.Shape * 2);
                }
            }
            byte nHair = (byte)(Hair * 2 + (byte)playGender);
            return M2Share.MakeHumanFeature(0, nDress, nWeapon, nHair);
        }

        public override void MakeGhost()
        {
            const string sExceptionMsg = "[Exception] PlayObject::MakeGhost";
            try
            {
                if (M2Share.HighLevelHuman == ActorId)
                {
                    M2Share.HighLevelHuman = 0;
                }
                if (M2Share.HighPKPointHuman == ActorId)
                {
                    M2Share.HighPKPointHuman = 0;
                }
                if (M2Share.HighDCHuman == ActorId)
                {
                    M2Share.HighDCHuman = 0;
                }
                if (M2Share.HighMCHuman == ActorId)
                {
                    M2Share.HighMCHuman = 0;
                }
                if (M2Share.HighSCHuman == ActorId)
                {
                    M2Share.HighSCHuman = 0;
                }
                if (M2Share.HighOnlineHuman == ActorId)
                {
                    M2Share.HighOnlineHuman = 0;
                }
                // 人物下线后通知配偶，并把对方的相关记录清空
                string sSayMsg;
                if (DearHuman != null)
                {
                    if (Gender == PlayGender.Man)
                    {
                        sSayMsg = Settings.ManLongOutDearOnlineMsg.Replace("%d", DearName);
                        sSayMsg = sSayMsg.Replace("%s", ChrName);
                        sSayMsg = sSayMsg.Replace("%m", Envir.MapDesc);
                        sSayMsg = sSayMsg.Replace("%x", CurrX.ToString());
                        sSayMsg = sSayMsg.Replace("%y", CurrY.ToString());
                        DearHuman.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                    }
                    else
                    {
                        sSayMsg = Settings.WoManLongOutDearOnlineMsg.Replace("%d", DearName);
                        sSayMsg = sSayMsg.Replace("%s", ChrName);
                        sSayMsg = sSayMsg.Replace("%m", Envir.MapDesc);
                        sSayMsg = sSayMsg.Replace("%x", CurrX.ToString());
                        sSayMsg = sSayMsg.Replace("%y", CurrY.ToString());
                        DearHuman.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                    }
                    DearHuman.DearHuman = null;
                    DearHuman = null;
                }
                if (MasterHuman != null || MasterList.Count > 0)
                {
                    if (IsMaster)
                    {
                        for (int i = MasterList.Count - 1; i >= 0; i--)
                        {
                            IPlayerActor human = (IPlayerActor)MasterList[i];
                            sSayMsg = Settings.MasterLongOutMasterListOnlineMsg.Replace("%s", ChrName);
                            sSayMsg = sSayMsg.Replace("%m", Envir.MapDesc);
                            sSayMsg = sSayMsg.Replace("%x", CurrX.ToString());
                            sSayMsg = sSayMsg.Replace("%y", CurrY.ToString());
                            human.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                            human.MasterHuman = null;
                        }
                    }
                    else
                    {
                        if (MasterHuman == null)
                        {
                            return;
                        }
                        sSayMsg = Settings.MasterListLongOutMasterOnlineMsg.Replace("%d", MasterName);
                        sSayMsg = sSayMsg.Replace("%s", ChrName);
                        sSayMsg = sSayMsg.Replace("%m", Envir.MapDesc);
                        sSayMsg = sSayMsg.Replace("%x", CurrX.ToString());
                        sSayMsg = sSayMsg.Replace("%y", CurrY.ToString());
                        MasterHuman.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                        // 如果为大徒弟则将对方的记录清空
                        if (MasterHuman.MasterName == ChrName)
                        {
                            MasterHuman.MasterHuman = null;
                        }
                        for (int i = 0; i < MasterHuman.MasterList.Count; i++)
                        {
                            if (MasterHuman.MasterList[i] == this)
                            {
                                MasterHuman.MasterList.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.Logger.Error(sExceptionMsg);
                M2Share.Logger.Error(e.Message);
            }
            base.MakeGhost();
            if (Ghost)
            {
                SendSelfDelayMsg(Messages.RM_MASTERDIEGHOST, 0, 0, 0, 0, "", 1000);
            }
        }

        public override void ScatterBagItems(int itemOfCreat)
        {
            const int dropWide = 2;
            if (AngryRing || NoDropItem || Envir.Flag.NoDropItem)
            {
                return;// 不死戒指
            }
            const string sExceptionMsg = "[Exception] PlayObject::ScatterBagItems";
            try
            {
                if (ItemList.Count > 0)
                {
                    IList<DeleteItem> delList = new List<DeleteItem>();
                    bool boDropall = SystemShare.Config.DieRedScatterBagAll && PvpLevel() >= 2;
                    for (int i = ItemList.Count - 1; i >= 0; i--)
                    {
                        if (boDropall || M2Share.RandomNumber.Random(SystemShare.Config.DieScatterBagRate) == 0)
                        {
                            if (DropItemDown(ItemList[i], dropWide, true, itemOfCreat, ActorId))
                            {
                                delList.Add(new DeleteItem()
                                {
                                    ItemName = SystemShare.ItemSystem.GetStdItemName(ItemList[i].Index),
                                    MakeIndex = ItemList[i].MakeIndex
                                });
                                Dispose(ItemList[i]);
                                ItemList.RemoveAt(i);
                            }
                        }
                    }
                    if (delList.Count > 0)
                    {
                        int objectId = HUtil32.Sequence();
                        SystemShare.ActorMgr.AddOhter(objectId, delList);
                        SendMsg(Messages.RM_SENDDELITEMLIST, 0, objectId, 0, 0);
                    }
                }
            }
            catch
            {
                M2Share.Logger.Error(sExceptionMsg);
            }
        }

        public override byte GetNameColor()
        {
            byte pvpLevel = PvpLevel();
            if (pvpLevel == 0)
            {
                return base.GetNameColor();
            }
            return pvpLevel >= 2 ? SystemShare.Config.PKLevel2NameColor : SystemShare.Config.PKLevel1NameColor;
        }

        protected override byte GetChrColor(IActor baseObject)
        {
            if (baseObject.Race == ActorRace.Play)
            {
                byte result = baseObject.NameColor;
                IPlayerActor targetObject = (IPlayerActor)baseObject;
                if (targetObject.PvpLevel() < 2)
                {
                    if (targetObject.PvpFlag)
                    {
                        result = SystemShare.Config.PKFlagNameColor;
                    }
                    int n10 = GetGuildRelation(this, targetObject);
                    switch (n10)
                    {
                        case 1:
                        case 3:
                            result = SystemShare.Config.AllyAndGuildNameColor;
                            break;
                        case 2:
                            result = SystemShare.Config.WarGuildNameColor;
                            break;
                    }
                    if (targetObject.Envir.Flag.Fight3Zone)
                    {
                        result = MyGuild == targetObject.MyGuild ? SystemShare.Config.AllyAndGuildNameColor : SystemShare.Config.WarGuildNameColor;
                    }
                }
                IUserCastle castle = SystemShare.CastleMgr.InCastleWarArea(targetObject);
                if ((castle != null) && castle.UnderWar && InGuildWarArea && targetObject.InGuildWarArea)
                {
                    result = SystemShare.Config.InFreePKAreaNameColor;
                    GuildWarArea = true;
                    if (MyGuild == null)
                    {
                        return result;
                    }
                    if (castle.IsMasterGuild(MyGuild))
                    {
                        if ((MyGuild == targetObject.MyGuild) || MyGuild.IsAllyGuild(targetObject.MyGuild))
                        {
                            result = SystemShare.Config.AllyAndGuildNameColor;
                        }
                        else
                        {
                            if (castle.IsAttackGuild(targetObject.MyGuild))
                            {
                                result = SystemShare.Config.WarGuildNameColor;
                            }
                        }
                    }
                    else
                    {
                        if (castle.IsAttackGuild(MyGuild))
                        {
                            if ((MyGuild == targetObject.MyGuild) || MyGuild.IsAllyGuild(targetObject.MyGuild))
                            {
                                result = SystemShare.Config.AllyAndGuildNameColor;
                            }
                            else
                            {
                                if (castle.IsMember(targetObject))
                                {
                                    result = SystemShare.Config.WarGuildNameColor;
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
            HitPlus = 0;
            HitDouble = 0;
            NakedAbility bonusTick;
            switch (Job)
            {
                case PlayJob.Warrior:
                    bonusTick = SystemShare.Config.BonusAbilofWarr;
                    HitPoint = (byte)(Settings.DEFHIT + BonusAbil.Hit / bonusTick.Hit);
                    SpeedPoint = (byte)(Settings.DEFSPEED + BonusAbil.Speed / bonusTick.Speed);
                    break;
                case PlayJob.Wizard:
                    bonusTick = SystemShare.Config.BonusAbilofWizard;
                    HitPoint = (byte)(Settings.DEFHIT + BonusAbil.Hit / bonusTick.Hit);
                    SpeedPoint = (byte)(Settings.DEFSPEED + BonusAbil.Speed / bonusTick.Speed);
                    break;
                case PlayJob.Taoist:
                    bonusTick = SystemShare.Config.BonusAbilofTaos;
                    SpeedPoint = (byte)(Settings.DEFSPEED + BonusAbil.Speed / bonusTick.Speed + 3);
                    break;
            }
            for (int i = 0; i < MagicList.Count; i++)
            {
                UserMagic userMagic = MagicList[i];
                MagicArr[userMagic.MagIdx] = userMagic;
                switch (userMagic.MagIdx)
                {
                    case MagicConst.SKILL_ONESWORD: // 基本剑法
                        if (userMagic.Level > 0)
                        {
                            HitPoint = (byte)(HitPoint + HUtil32.Round(9 / 3.0 * userMagic.Level));
                        }
                        break;
                    case MagicConst.SKILL_ILKWANG: // 精神力战法
                        if (userMagic.Level > 0)
                        {
                            HitPoint = (byte)(HitPoint + HUtil32.Round(8 / 3.0 * userMagic.Level));
                        }
                        break;
                    case MagicConst.SKILL_YEDO: // 攻杀剑法
                        if (userMagic.Level > 0)
                        {
                            HitPoint = (byte)(HitPoint + HUtil32.Round(3 / 3.0 * userMagic.Level));
                        }
                        HitPlus = (byte)(Settings.DEFHIT + userMagic.Level);
                        AttackSkillCount = (byte)(7 - userMagic.Level);
                        AttackSkillPointCount = M2Share.RandomNumber.RandomByte(AttackSkillCount);
                        break;
                    case MagicConst.SKILL_FIRESWORD: // 烈火剑法
                        HitDouble = (byte)(4 + userMagic.Level * 4);
                        break;
                }
            }
        }

        /// <summary>
        /// 切换地图
        /// </summary>
        internal bool EnterAnotherMap(IEnvirnoment envir, short nDMapX, short nDMapY)
        {
            bool result = false;
            const string sExceptionMsg = "[Exception] BaseObject::EnterAnotherMap";
            try
            {
                if (Abil.Level < envir.EnterLevel)
                {
                    SysMsg($"需要 {envir.Flag.RequestLevel - 1} 级以上才能进入 {envir.MapDesc}", MsgColor.Red, MsgType.Hint);
                    return false;
                }
                //if (envir.QuestNpc != null)
                //{
                //    envir.QuestNpc.Click(this);
                //}
                if (envir.Flag.NeedSetonFlag >= 0)
                {
                    if (GetQuestFalgStatus(envir.Flag.NeedSetonFlag) != envir.Flag.NeedOnOff)
                    {
                        return false;
                    }
                }
                if (!envir.CellValid(nDMapX, nDMapY))
                {
                    return false;
                }
                IUserCastle castle = SystemShare.CastleMgr.IsCastlePalaceEnvir(envir);
                if ((castle != null))
                {
                    if (!castle.CheckInPalace(CurrX, CurrY))
                    {
                        return false;
                    }
                }
                if (envir.Flag.NoHorse)
                {
                    OnHorse = false;
                }
                IEnvirnoment oldEnvir = Envir;
                short nOldX = CurrX;
                short nOldY = CurrY;
                DisappearA();
                VisibleHumanList.Clear();
                for (int i = 0; i < VisibleItems.Count; i++)
                {
                    VisibleItems[i] = null;
                }
                VisibleItems.Clear();
                VisibleEvents.Clear();
                for (int i = 0; i < VisibleActors.Count; i++)
                {
                    VisibleActors[i] = null;
                }
                VisibleActors.Clear();
                SendMsg(Messages.RM_CLEAROBJECTS, 0, 0, 0, 0);
                Envir = envir;
                MapName = envir.MapName;
                MapFileName = envir.MapFileName;
                CurrX = nDMapX;
                CurrY = nDMapY;
                SendMsg(Messages.RM_CHANGEMAP, 0, 0, 0, 0, envir.MapFileName);
                if (AddToMap())
                {
                    //MapMoveTick = HUtil32.GetTickCount();
                    SpaceMoved = true;
                    result = true;
                }
                else
                {
                    Envir = oldEnvir;
                    CurrX = nOldX;
                    CurrY = nOldY;
                    Envir.AddMapObject(CurrX, CurrY, CellType, this.ActorId, this);
                }
                OnEnvirnomentChanged();
                // 复位泡点，及金币，时间
                IncGamePointTick = HUtil32.GetTickCount();
                IncGameGoldTick = HUtil32.GetTickCount();
                AutoGetExpTick = HUtil32.GetTickCount();
                if (Envir.Flag.Fight3Zone && (Envir.Flag.Fight3Zone != oldEnvir.Flag.Fight3Zone))
                {
                    RefShowName();
                }
            }
            catch
            {
                M2Share.Logger.Error(sExceptionMsg);
            }
            return result;
        }

        private void WinLottery()
        {
            int nGold = 0;
            int nWinLevel = 0;
            int nRate = M2Share.RandomNumber.Random(SystemShare.Config.WinLotteryRate);
            if (nRate >= SystemShare.Config.WinLottery6Min && nRate <= SystemShare.Config.WinLottery6Max)
            {
                if (SystemShare.Config.WinLotteryCount < SystemShare.Config.NoWinLotteryCount)
                {
                    nGold = SystemShare.Config.WinLottery6Gold;
                    nWinLevel = 6;
                    SystemShare.Config.WinLotteryLevel6++;
                }
            }
            else if (nRate >= SystemShare.Config.WinLottery5Min && nRate <= SystemShare.Config.WinLottery5Max)
            {
                if (SystemShare.Config.WinLotteryCount < SystemShare.Config.NoWinLotteryCount)
                {
                    nGold = SystemShare.Config.WinLottery5Gold;
                    nWinLevel = 5;
                    SystemShare.Config.WinLotteryLevel5++;
                }
            }
            else if (nRate >= SystemShare.Config.WinLottery4Min && nRate <= SystemShare.Config.WinLottery4Max)
            {
                if (SystemShare.Config.WinLotteryCount < SystemShare.Config.NoWinLotteryCount)
                {
                    nGold = SystemShare.Config.WinLottery4Gold;
                    nWinLevel = 4;
                    SystemShare.Config.WinLotteryLevel4++;
                }
            }
            else if (nRate >= SystemShare.Config.WinLottery3Min && nRate <= SystemShare.Config.WinLottery3Max)
            {
                if (SystemShare.Config.WinLotteryCount < SystemShare.Config.NoWinLotteryCount)
                {
                    nGold = SystemShare.Config.WinLottery3Gold;
                    nWinLevel = 3;
                    SystemShare.Config.WinLotteryLevel3++;
                }
            }
            else if (nRate >= SystemShare.Config.WinLottery2Min && nRate <= SystemShare.Config.WinLottery2Max)
            {
                if (SystemShare.Config.WinLotteryCount < SystemShare.Config.NoWinLotteryCount)
                {
                    nGold = SystemShare.Config.WinLottery2Gold;
                    nWinLevel = 2;
                    SystemShare.Config.WinLotteryLevel2++;
                }
            }
            else if (SystemShare.Config.WinLottery1Min + SystemShare.Config.WinLottery1Max == nRate)
            {
                if (SystemShare.Config.WinLotteryCount < SystemShare.Config.NoWinLotteryCount)
                {
                    nGold = SystemShare.Config.WinLottery1Gold;
                    nWinLevel = 1;
                    SystemShare.Config.WinLotteryLevel1++;
                }
            }
            if (nGold > 0)
            {
                switch (nWinLevel)
                {
                    case 1:
                        SysMsg(Settings.WinLottery1Msg, MsgColor.Green, MsgType.Hint);
                        break;
                    case 2:
                        SysMsg(Settings.WinLottery2Msg, MsgColor.Green, MsgType.Hint);
                        break;
                    case 3:
                        SysMsg(Settings.WinLottery3Msg, MsgColor.Green, MsgType.Hint);
                        break;
                    case 4:
                        SysMsg(Settings.WinLottery4Msg, MsgColor.Green, MsgType.Hint);
                        break;
                    case 5:
                        SysMsg(Settings.WinLottery5Msg, MsgColor.Green, MsgType.Hint);
                        break;
                    case 6:
                        SysMsg(Settings.WinLottery6Msg, MsgColor.Green, MsgType.Hint);
                        break;
                }
                if (IncGold(nGold))
                {
                    GoldChanged();
                }
                else
                {
                    DropGoldDown(nGold, true, 0, 0);
                }
            }
            else
            {
                SystemShare.Config.NoWinLotteryCount += 500;
                SysMsg(Settings.NotWinLotteryMsg, MsgColor.Red, MsgType.Hint);
            }
        }

        internal void AddItemSkill(int nIndex)
        {
            MagicInfo magic = null;
            switch (nIndex)
            {
                case 1:
                    magic = SystemShare.WorldEngine.FindMagic(SystemShare.Config.FireBallSkill);
                    break;
                case 2:
                    magic = SystemShare.WorldEngine.FindMagic(SystemShare.Config.HealSkill);
                    break;
            }
            if (magic != null)
            {
                if (!IsTrainingSkill(magic.MagicId))
                {
                    UserMagic userMagic = new UserMagic
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
            if (Abil.Level > Settings.ExpErienceLevel)
            {
                ExpireTime = HUtil32.GetTickCount() + (60 * 1000);
                ExpireCount = expiredTime;
            }
        }

        private void CheckExpiredTime()
        {
            ExpireCount--;
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
                    SysMsg("您的账号游戏时间已到期，访问(https://mir2.sdo.com)购买充值，所有游戏大区均可账号共享游戏时间。", MsgColor.Blue, MsgType.System);
                    break;
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
            int n10 = nFlag / 8;
            int n14 = nFlag % 8;
            if ((n10 - QuestFlag.Length) < 0)
            {
                byte bt15 = QuestFlag[n10];
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
            int result = 0;
            nFlag -= 1;
            if (nFlag < 0)
            {
                return result;
            }
            int n10 = nFlag / 8;
            int n14 = nFlag % 8;
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
            int n10 = nFlag / 8;
            int n14 = nFlag % 8;
            if ((n10 - QuestUnitOpen.Length) < 0)
            {
                byte bt15 = QuestUnitOpen[n10];
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
            int result = 0;
            nFlag -= 1;
            if (nFlag < 0)
            {
                return result;
            }
            int n10 = nFlag / 8;
            int n14 = nFlag % 8;
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
            int n10 = nFlag / 8;
            int n14 = nFlag % 8;
            if ((n10 - QuestUnit.Length) < 0)
            {
                byte bt15 = QuestUnit[n10];
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

        /// <summary>
        /// 检查武器是否升级
        /// </summary>
        private void CheckWeaponUpgrade()
        {
            if (UseItems[ItemLocation.Weapon] != null && UseItems[ItemLocation.Weapon].Desc[ItemAttr.WeaponUpgrade] > 0)
            {
                UserItem useItems = new UserItem(UseItems[ItemLocation.Weapon]);
                CheckWeaponUpgradeStatus(ref UseItems[ItemLocation.Weapon]);
                StdItem StdItem = SystemShare.ItemSystem.GetStdItem(useItems.Index);
                if (UseItems[ItemLocation.Weapon].Index == 0)
                {
                    SysMsg(Settings.TheWeaponBroke, MsgColor.Red, MsgType.Hint);
                    SendDelItems(useItems);
                    SendRefMsg(Messages.RM_BREAKWEAPON, 0, 0, 0, 0, "");
                    if (StdItem?.NeedIdentify == 1)
                    {
                        // M2Share.EventSource.AddEventLog(21, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + StdItem?.Name + "\t" + useItems.MakeIndex + "\t" + '1' + "\t" + '0');
                    }
                    FeatureChanged();
                }
                else
                {
                    SysMsg(Settings.TheWeaponRefineSuccessfull, MsgColor.Red, MsgType.Hint);
                    SendUpdateItem(UseItems[ItemLocation.Weapon]);
                    if (StdItem.NeedIdentify == 1)
                    {
                        // M2Share.EventSource.AddEventLog(20, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + StdItem.Name + "\t" + useItems.MakeIndex + "\t" + '1' + "\t" + '0');
                    }
                    RecalcAbilitys();
                    SendMsg(Messages.RM_ABILITY, 0, 0, 0, 0);
                    SendMsg(Messages.RM_SUBABILITY, 0, 0, 0, 0);
                }
            }
        }

        /// <summary>
        /// 检查武器升级状态
        /// </summary>
        private static void CheckWeaponUpgradeStatus(ref UserItem userItem)
        {
            if ((userItem.Desc[0] + userItem.Desc[1] + userItem.Desc[2]) < SystemShare.Config.UpgradeWeaponMaxPoint)
            {
                if (userItem.Desc[ItemAttr.WeaponUpgrade] == 1)
                {
                    userItem.Index = 0;
                }
                if (HUtil32.RangeInDefined(userItem.Desc[ItemAttr.WeaponUpgrade], 10, 13))
                {
                    userItem.Desc[0] = (byte)(userItem.Desc[0] + userItem.Desc[ItemAttr.WeaponUpgrade] - 9);
                }
                if (HUtil32.RangeInDefined(userItem.Desc[ItemAttr.WeaponUpgrade], 20, 23))
                {
                    userItem.Desc[1] = (byte)(userItem.Desc[1] + userItem.Desc[ItemAttr.WeaponUpgrade] - 19);
                }
                if (HUtil32.RangeInDefined(userItem.Desc[ItemAttr.WeaponUpgrade], 30, 33))
                {
                    userItem.Desc[2] = (byte)(userItem.Desc[2] + userItem.Desc[ItemAttr.WeaponUpgrade] - 29);
                }
            }
            else
            {
                userItem.Index = 0;
            }
            userItem.Desc[ItemAttr.WeaponUpgrade] = 0;
        }

        internal UserMagic GetMagicInfo(int nMagicId)
        {
            for (int i = 0; i < MagicList.Count; i++)
            {
                UserMagic userMagic = MagicList[i];
                if (userMagic.Magic.MagicId == nMagicId)
                {
                    return userMagic;
                }
            }
            return null;
        }

        public UserMagic GetMagicInfo(string sMagicName)
        {
            UserMagic result = null;
            for (int i = 0; i < this.MagicList.Count; i++)
            {
                var userMagic = this.MagicList[i];
                if (string.Compare(userMagic.Magic.MagicName, sMagicName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = userMagic;
                    break;
                }
            }
            return result;
        }

        private bool IsProperIsFriend(IActor cret)
        {
            bool result = false;
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
                        if ((this == cret) || (cret == DearHuman))
                        {
                            result = true;
                        }
                        break;
                    case AttackMode.HAM_MASTER:
                        if (this == cret)
                        {
                            result = true;
                        }
                        else if (IsMaster)
                        {
                            for (int i = 0; i < MasterList.Count; i++)
                            {
                                if (MasterList[i] == cret)
                                {
                                    result = true;
                                    break;
                                }
                            }
                        }
                        else if (((IPlayerActor)cret).IsMaster)
                        {
                            for (int i = 0; i < ((IPlayerActor)cret).MasterList.Count; i++)
                            {
                                if (((IPlayerActor)cret).MasterList[i] == this)
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
                            if (GuildWarArea && (((IPlayerActor)cret).MyGuild != null))
                            {
                                if (MyGuild.IsAllyGuild(((IPlayerActor)cret).MyGuild))
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
                            if (((IPlayerActor)cret).PvpLevel() < 2)
                            {
                                result = true;
                            }
                        }
                        else
                        {
                            if (((IPlayerActor)cret).PvpLevel() >= 2)
                            {
                                result = true;
                            }
                        }
                        break;
                }
            }
            return result;
        }

        public void AddBodyLuck(double dLuck)
        {
            if ((dLuck > 0) && (BodyLuck < 5 * Settings.BODYLUCKUNIT))
            {
                BodyLuck = BodyLuck + dLuck;
            }
            if ((dLuck < 0) && (BodyLuck > -(5 * Settings.BODYLUCKUNIT)))
            {
                BodyLuck = BodyLuck + dLuck;
            }
            int n = Convert.ToInt32(BodyLuck / Settings.BODYLUCKUNIT);
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

        public void SetPkFlag(IActor baseObject)
        {
            if (baseObject.Race == ActorRace.Play)
            {
                IPlayerActor targetObject = (IPlayerActor)baseObject;
                if ((PvpLevel() < 2) && (targetObject.PvpLevel() < 2) && (!Envir.Flag.FightZone) && (!Envir.Flag.Fight3Zone) && !PvpFlag)
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
            if (PvpFlag && ((HUtil32.GetTickCount() - PvpNameColorTick) > SystemShare.Config.dwPKFlagTime)) // 60 * 1000
            {
                PvpFlag = false;
                RefNameColor();
            }
        }

        public void IncPkPoint(int nPoint)
        {
            byte oldPvpLevel = PvpLevel();
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
            byte pvpLevel = PvpLevel();
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

        internal void ApplyItemParameters(UserItem uitem, StdItem item, ref AddAbility aabil)
        {
            if (item != null)
            {
                ClientItem clientItem = new ClientItem();
                SystemShare.ItemSystem.GetUpgradeStdItem(item, uitem, ref clientItem);
                ApplyItemParametersByJob(uitem, ref clientItem);
                switch (item.StdMode)
                {
                    case 5:
                    case 6:
                        aabil.HIT = (ushort)(aabil.HIT + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed + SystemShare.ItemSystem.RealAttackSpeed(HUtil32.HiByte(clientItem.Item.MAC)));
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

        internal static void ApplyItemParametersEx(UserItem uitem, ref Ability aWabil)
        {
            StdItem item = SystemShare.ItemSystem.GetStdItem(uitem.Index);
            if (item != null)
            {
                ClientItem clientItem = new ClientItem();
                SystemShare.ItemSystem.GetUpgradeStdItem(item, uitem, ref clientItem);
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

        public void ChangeItemByJob(ref ClientItem citem, int level)
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

        public void NpcGotoLable(INormNpc actor, string sLable, bool boMaster)
        {
            if (actor != null && !string.IsNullOrEmpty(sLable))
            {
                ScriptGotoCount = 0;
                actor.GotoLable(this, sLable, false);
            }
        }

        private void ApplyItemParametersByJob(UserItem uitem, ref ClientItem std)
        {
            StdItem item = SystemShare.ItemSystem.GetStdItem(uitem.Index);
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
            for (int i = 0; i < MagicList.Count; i++)
            {
                UserMagic userMagic = MagicList[i];
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
                        DeleteNameSkill(SystemShare.Config.FireBallSkill);
                    }
                    break;
                case 2:
                    if (Job != PlayJob.Taoist)
                    {
                        DeleteNameSkill(SystemShare.Config.HealSkill);
                    }
                    break;
            }
        }

        public void DelMember(IPlayerActor baseObject)
        {
            if (GroupOwner != baseObject.ActorId)
            {
                for (int i = 0; i < GroupMembers.Count; i++)
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
                for (int i = GroupMembers.Count - 1; i >= 0; i--)
                {
                    GroupMembers[i].LeaveGroup();
                    GroupMembers.RemoveAt(i);
                }
            }
            if (!this.CancelGroup())
            {
                this.SendDefMessage(Messages.SM_GROUPCANCEL, 0, 0, 0, 0);
            }
            else
            {
                this.SendGroupMembers();
            }
        }

        private bool IsGroupMember(IActor target)
        {
            bool result = false;
            if (GroupOwner == 0)
            {
                return false;
            }
            IPlayerActor groupOwnerPlay = (IPlayerActor)SystemShare.ActorMgr.Get(GroupOwner);
            for (int i = 0; i < groupOwnerPlay.GroupMembers.Count; i++)
            {
                if (groupOwnerPlay.GroupMembers[i] == target)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void LeaveGroup()
        {
            const string sExitGroupMsg = "{0} 已经退出了本组.";
            SendGroupText(Format(sExitGroupMsg, ChrName));
            GroupOwner = 0;
            SendMsg(Messages.RM_GROUPCANCEL, 0, 0, 0, 0);
        }

        public void SendGroupText(string sMsg)
        {
            sMsg = SystemShare.Config.GroupMsgPreFix + sMsg;
            if (GroupOwner != 0)
            {
                IPlayerActor groupOwnerPlay = (IPlayerActor)SystemShare.ActorMgr.Get(GroupOwner);
                for (int i = 0; i < groupOwnerPlay.GroupMembers.Count; i++)
                {
                    groupOwnerPlay.GroupMembers[i].SendMsg(this, Messages.RM_GROUPMESSAGE, 0, SystemShare.Config.GroupMsgFColor, SystemShare.Config.GroupMsgBColor, 0, sMsg);
                }
            }
        }

        public bool CheckMagicLevelUp(UserMagic userMagic)
        {
            bool result = false;
            int nLevel;
            if ((userMagic.Level < 4) && (userMagic.Magic.TrainLv >= userMagic.Level))
            {
                nLevel = userMagic.Level;
            }
            else
            {
                nLevel = 0;
            }
            if ((userMagic.Magic.TrainLv > userMagic.Level) && (userMagic.Magic.MaxTrain[nLevel] <= userMagic.TranPoint))
            {
                if (userMagic.Magic.TrainLv > userMagic.Level)
                {
                    userMagic.TranPoint -= userMagic.Magic.MaxTrain[nLevel];
                    userMagic.Level++;
                    UpdateDelayMsg(Messages.RM_MAGIC_LVEXP, 0, userMagic.Magic.MagicId, userMagic.Level, userMagic.TranPoint, "", 800);
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
        /// 心灵启示
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

        public void HasLevelUp(int nLevel)
        {
            Abil.MaxExp = GetLevelExp(Abil.Level);
            RecalcLevelAbilitys();
            RecalcAbilitys();
            SendMsg(Messages.RM_LEVELUP, 0, Abil.Exp, 0, 0);
            if (SystemShare.FunctionNPC != null)
            {
                // ModuleShare.FunctionNPC.GotoLable(this, "@LevelUp", false);
            }
        }

        public void RecalcLevelAbilitys()
        {
            int n;
            byte nLevel = Abil.Level;
            switch (this.Job)
            {
                case PlayJob.Taoist:
                    Abil.MaxHP = (ushort)HUtil32._MIN(ushort.MaxValue, 14 + HUtil32.Round((nLevel / (double)SystemShare.Config.nLevelValueOfTaosHP + SystemShare.Config.nLevelValueOfTaosHPRate) * nLevel));
                    Abil.MaxMP = (ushort)HUtil32._MIN(ushort.MaxValue, 13 + HUtil32.Round(nLevel / (double)SystemShare.Config.nLevelValueOfTaosMP * 2.2 * nLevel));
                    Abil.MaxWeight = (ushort)(50 + HUtil32.Round(nLevel / 4.0 * nLevel));
                    Abil.MaxWearWeight = (byte)HUtil32._MIN(byte.MaxValue, (15 + HUtil32.Round(nLevel / 50.0 * nLevel)));
                    if ((12 + HUtil32.Round(Abil.Level / 13.0 * Abil.Level)) > 255)
                    {
                        Abil.MaxHandWeight = byte.MaxValue;
                    }
                    else
                    {
                        Abil.MaxHandWeight = (byte)(12 + HUtil32.Round(nLevel / 42.0 * nLevel));
                    }
                    n = nLevel / 7;
                    Abil.DC = HUtil32.MakeWord((ushort)HUtil32._MAX(n - 1, 0), (ushort)HUtil32._MAX(1, n));
                    Abil.MC = 0;
                    Abil.SC = HUtil32.MakeWord((ushort)HUtil32._MAX(n - 1, 0), (ushort)HUtil32._MAX(1, n));
                    Abil.AC = 0;
                    n = HUtil32.Round(nLevel / 6.0);
                    Abil.MAC = HUtil32.MakeWord((ushort)(n / 2), (ushort)(n + 1));
                    break;
                case PlayJob.Wizard:
                    Abil.MaxHP = (ushort)HUtil32._MIN(ushort.MaxValue, 14 + HUtil32.Round(((nLevel / (double)SystemShare.Config.nLevelValueOfWizardHP) + SystemShare.Config.nLevelValueOfWizardHPRate) * nLevel));
                    Abil.MaxMP = (ushort)HUtil32._MIN(ushort.MaxValue, 13 + HUtil32.Round(((nLevel / (double)5) + 2) * 2.2 * nLevel));
                    Abil.MaxWeight = (ushort)(50 + HUtil32.Round(nLevel / 5.0 * nLevel));
                    Abil.MaxWearWeight = (byte)HUtil32._MIN(byte.MaxValue, 15 + HUtil32.Round(nLevel / 100.0 * nLevel));
                    Abil.MaxHandWeight = (byte)(12 + HUtil32.Round(nLevel / 90.0 * nLevel));
                    n = nLevel / 7;
                    Abil.DC = HUtil32.MakeWord((ushort)HUtil32._MAX(n - 1, 0), (ushort)HUtil32._MAX(1, n));
                    Abil.MC = HUtil32.MakeWord((ushort)HUtil32._MAX(n - 1, 0), (ushort)HUtil32._MAX(1, n));
                    Abil.SC = 0;
                    Abil.AC = 0;
                    Abil.MAC = 0;
                    break;
                case PlayJob.Warrior:
                    Abil.MaxHP = (ushort)HUtil32._MIN(ushort.MaxValue, 14 + HUtil32.Round(((nLevel / (double)SystemShare.Config.nLevelValueOfWarrHP) + SystemShare.Config.nLevelValueOfWarrHPRate + (nLevel / (double)20)) * nLevel));
                    Abil.MaxMP = (ushort)HUtil32._MIN(ushort.MaxValue, 11 + HUtil32.Round(nLevel * 3.5));
                    Abil.MaxWeight = (ushort)(50 + HUtil32.Round(nLevel / 3.0 * nLevel));
                    Abil.MaxWearWeight = (byte)HUtil32._MIN(byte.MaxValue, (15 + HUtil32.Round(nLevel / 20.0 * nLevel)));
                    Abil.MaxHandWeight = (byte)(12 + HUtil32.Round(nLevel / 13.0 * nLevel));
                    Abil.DC = HUtil32.MakeWord((ushort)HUtil32._MAX(nLevel / 5 - 1, 1), (ushort)HUtil32._MAX(1, nLevel / 5));
                    Abil.SC = 0;
                    Abil.MC = 0;
                    Abil.AC = HUtil32.MakeWord(0, (ushort)(nLevel / 7));
                    Abil.MAC = 0;
                    break;
                case PlayJob.None:
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

        /// <summary>
        /// 无极真气
        /// </summary>
        /// <returns></returns>
        public void AttPowerUp(int nPower, int nTime)
        {
            this.ExtraAbil[0] = (ushort)nPower;
            this.ExtraAbilTimes[0] = HUtil32.GetTickCount() + nTime * 1000;
            SysMsg(Format(Settings.AttPowerUpTime, nTime / 60, nTime % 60), MsgColor.Green, MsgType.Hint);
            RecalcAbilitys();
            SendMsg(Messages.RM_ABILITY, 0, 0, 0, 0);
        }

        public UserItem CheckItemCount(string sItemName, ref int nCount)
        {
            UserItem result = null;
            nCount = 0;
            for (int i = 0; i < UseItems.Length; i++)
            {
                if (UseItems[i] == null)
                {
                    continue;
                }
                string sName = SystemShare.ItemSystem.GetStdItemName(UseItems[i].Index);
                if (string.Compare(sName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = UseItems[i];
                    nCount++;
                }
            }
            return result;
        }

        /// <summary>
        /// 减少复活戒指持久
        /// </summary>
        internal void ItemDamageRevivalRing()
        {
            for (int i = 0; i < UseItems.Length; i++)
            {
                if (UseItems[i] != null && UseItems[i].Index > 0)
                {
                    StdItem pSItem = SystemShare.ItemSystem.GetStdItem(UseItems[i].Index);
                    if (pSItem != null)
                    {
                        if (M2Share.ItemDamageRevivalMap.Contains(pSItem.Shape) || (((i == ItemLocation.Weapon) || (i == ItemLocation.RighThand)) && M2Share.ItemDamageRevivalMap.Contains(pSItem.AniCount)))
                        {
                            ushort nDura = UseItems[i].Dura;
                            ushort tDura = (ushort)HUtil32.Round(nDura / 1000.0);
                            nDura -= 1000;
                            if (nDura <= 0)
                            {
                                nDura = 0;
                                UseItems[i].Dura = nDura;
                                if (Race == ActorRace.Play)
                                {
                                    this.SendDelItems(UseItems[i]);
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
                                SendMsg(Messages.RM_DURACHANGE, i, nDura, UseItems[i].DuraMax, 0);
                            }
                        }
                    }
                }
            }
        }

        private static bool IsGoodKilling(PlayObject cert)
        {
            return cert.PvpFlag;
        }

        internal UserMagic GetAttackMagic(int magicId)
        {
            return MagicArr[magicId];
        }

        internal byte GetMyLight()
        {
            byte currentLight = 0;
            if (Abil.Level >= EfftypeConst.EFFECTIVE_HIGHLEVEL)
            {
                currentLight = 1;
            }
            for (byte i = ItemLocation.Dress; i <= ItemLocation.Charm; i++)
            {
                if (UseItems[i] == null)
                {
                    continue;
                }
                if ((UseItems[i].Index > 0) && (UseItems[i].Dura > 0))
                {
                    StdItem stdItem = SystemShare.ItemSystem.GetStdItem(UseItems[i].Index);
                    if (stdItem != null)
                    {
                        if (currentLight < stdItem.Light)
                        {
                            currentLight = stdItem.Light;
                        }
                    }
                }
            }
            return currentLight;
        }

        /// <summary>
        /// 更新可见物品列表
        /// </summary>
        protected void UpdateVisibleItem(short wX, short wY, MapItem MapItem)
        {
            VisibleMapItem visibleMapItem = null;
            bool boIsVisible = false;
            for (int i = 0; i < VisibleItems.Count; i++)
            {
                visibleMapItem = VisibleItems[i];
                if (visibleMapItem.MapItem == MapItem)
                {
                    visibleMapItem.VisibleFlag = VisibleFlag.Invisible;
                    boIsVisible = true;
                    break;
                }
            }
            if (boIsVisible)
            {
                return;
            }
            visibleMapItem ??= new VisibleMapItem
            {
                VisibleFlag = VisibleFlag.Show,
                nX = wX,
                nY = wY,
                MapItem = MapItem,
                sName = MapItem.Name,
                wLooks = MapItem.Looks
            };
            VisibleItems.Add(visibleMapItem);
        }

        protected void UpdateVisibleEvent(short wX, short wY, MapEvent MapEvent)
        {
            bool boIsVisible = false;
            for (int i = 0; i < VisibleEvents.Count; i++)
            {
                MapEvent mapEvent = VisibleEvents[i];
                if (mapEvent == MapEvent)
                {
                    mapEvent.VisibleFlag = VisibleFlag.Invisible;
                    boIsVisible = true;
                    break;
                }
            }
            if (boIsVisible)
            {
                return;
            }
            MapEvent.VisibleFlag = VisibleFlag.Show;
            MapEvent.nX = wX;
            MapEvent.nY = wY;
            VisibleEvents.Add(MapEvent);
        }
    }
}