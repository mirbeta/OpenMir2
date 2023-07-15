using System.Collections;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Events;
using SystemModule.Packets.ClientPackets;

namespace SystemModule
{
    public interface IPlayerActor : IActor
    {
        bool OnHorse { get; set; }
        byte HorseType { get; set; }
        AttackMode AttatckMode { get; set; }
        UserItem[] UseItems { get; set; }
        ushort IncHealth { get; set; }
        ushort IncSpell { get; set; }
        ushort IncHealing { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        PlayGender Gender { get; set; }
        /// <summary>
        /// 人物的头发
        /// </summary>
        byte Hair { get; set; }
        /// <summary>
        /// 人物的职业 (0:战士 1：法师 2:道士)
        /// </summary>
        PlayJob Job { get; set; }
        /// <summary>
        /// 登录帐号名
        /// </summary>
        string UserAccount { get; set; }
        /// <summary>
        /// 人物IP地址
        /// </summary>
        string LoginIpAddr { get; set; }
        string LoginIpLocal { get; set; }
        /// <summary>
        /// 账号过期
        /// </summary>
        bool AccountExpired { get; set; }
        /// <summary>
        /// 账号游戏点数检查时间
        /// </summary>
        int AccountExpiredTick { get; set; }
        long ExpireTime { get; set; }
        int ExpireCount { get; set; }
        int QueryExpireTick { get; set; }
        /// <summary>
        /// 权限等级
        /// </summary>
        byte Permission { get; set; }
        /// <summary>
        /// 人物的幸运值
        /// </summary>
        byte Luck { get; set; }
        /// <summary>
        /// 人物身上最多可带金币数
        /// </summary>
        int GoldMax { get; set; }
        /// <summary>
        /// 行会占争范围
        /// </summary>
        bool GuildWarArea { get; set; }
        /// <summary>
        /// 允许行会传送
        /// </summary>
        bool AllowGuildReCall { get; set; }
        /// <summary>
        /// 允许私聊
        /// </summary>
        bool HearWhisper { get; set; }
        /// <summary>
        /// 允许群聊
        /// </summary>
        bool BanShout { get; set; }
        /// <summary>
        /// 拒绝行会聊天
        /// </summary>
        bool BanGuildChat { get; set; }
        /// <summary>
        /// 是否允许交易
        /// </summary>
        bool AllowDeal { get; set; }
        /// <summary>
        /// 检查重叠人物使用
        /// </summary>
        bool BoDuplication { get; set; }
        /// <summary>
        /// 检查重叠人物间隔
        /// </summary>
        int DupStartTick { get; set; }
        /// <summary>
        /// 是否用了神水
        /// </summary>
        bool UserUnLockDurg { get; set; }
        /// <summary>
        /// 允许组队
        /// </summary>
        bool AllowGroup { get; set; }
        /// <summary>
        /// 允许加入行会
        /// </summary>        
        bool AllowGuild { get; set; }
        /// <summary>
        /// 交易对象
        /// </summary>
        IPlayerActor DealCreat { get; set; }
        /// <summary>
        /// 正在交易
        /// </summary>
        bool Dealing { get; set; }
        /// <summary>
        /// 交易最后操作时间
        /// </summary>
        int DealLastTick { get; set; }
        /// <summary>
        /// 交易的金币数量
        /// </summary>
        int DealGolds { get; set; }
        /// <summary>
        /// 确认交易标志
        /// </summary>
        bool DealSuccess { get; set; }
        /// <summary>
        /// 回城地图
        /// </summary>
        string HomeMap { get; set; }
        /// <summary>
        /// 回城座标X
        /// </summary>
        short HomeX { get; set; }
        /// <summary>
        /// 回城座标Y
        /// </summary>
        short HomeY { get; set; }
        /// <summary>
        /// 记忆使用间隔
        /// </summary>
        int GroupRcallTick { get; set; }
        short GroupRcallTime { get; set; }
        /// <summary>
        /// 行会传送
        /// </summary>
        bool GuildMove { get; set; }
        CommandMessage ClientMsg { get; set; }
        /// <summary>
        /// 在行会占争地图中死亡次数
        /// </summary>
        ushort FightZoneDieCount { get; set; }
        /// <summary>
        /// 祈祷
        /// </summary>
        bool IsSpirit { get; set; }
        /// <summary>
        /// 野蛮冲撞间隔
        /// </summary>
        int DoMotaeboTick { get; set; }
        bool CrsHitkill { get; set; }
        bool MBo43Kill { get; set; }
        bool RedUseHalfMoon { get; set; }
        bool UseThrusting { get; set; }
        bool UseHalfMoon { get; set; }
        /// <summary>
        /// 魔法技能
        /// </summary>
        UserMagic[] MagicArr { get; set; }
        /// <summary>
        /// 攻杀剑法
        /// </summary>
        bool PowerHit { get; set; }
        /// <summary>
        /// 烈火剑法
        /// </summary>
        bool FireHitSkill { get; set; }
        /// <summary>
        /// 烈火剑法
        /// </summary>
        bool TwinHitSkill { get; set; }
        /// <summary>
        /// 额外攻击伤害(攻杀)
        /// </summary>
        ushort HitPlus { get; set; }
        /// <summary>
        /// 双倍攻击伤害(烈火专用)
        /// </summary>
        ushort HitDouble { get; set; }
        int LatestFireHitTick { get; set; }
        int LatestTwinHitTick { get; set; }
        /// <summary>
        /// 力量物品值
        /// </summary>
        byte PowerItem { get; set; }
        /// <summary>
        /// 交易列表
        /// </summary>
        IList<UserItem> DealItemList { get; set; }
        /// <summary>
        /// 仓库物品列表
        /// </summary>
        IList<UserItem> StorageItemList { get; set; }
        /// <summary>
        /// 可见事件列表
        /// </summary>
        IList<MapEvent> VisibleEvents { get; set; }
        /// <summary>
        /// 可见物品列表
        /// </summary>
        IList<VisibleMapItem> VisibleItems { get; set; }
        /// <summary>
        /// 禁止私聊人员列表
        /// </summary>
        IList<string> LockWhisperList { get; set; }
        /// <summary>
        /// 力量物品(影响力量的物品)
        /// </summary>
        bool BoPowerItem { get; set; }
        bool AllowGroupReCall { get; set; }
        int HungerStatus { get; set; }
        int BonusPoint { get; set; }
        byte BtB2 { get; set; }
        /// <summary>
        /// 人物攻击变色标志
        /// </summary>
        bool PvpFlag { get; set; }
        /// <summary>
        /// 减PK值时间`
        /// </summary>
        int DecPkPointTick { get; set; }
        /// <summary>
        /// 人物的PK值
        /// </summary>
        int PkPoint { get; set; }
        /// <summary>
        /// 人物攻击变色时间长度
        /// </summary>
        int PvpNameColorTick { get; set; }
        bool NameColorChanged { get; set; }
        /// <summary>
        /// 是否在开行会战
        /// </summary>
        bool InGuildWarArea { get; set; }
        IGuild MyGuild { get; set; }
        short GuildRankNo { get; set; }
        string GuildRankName { get; set; }
        string ScriptLable { get; set; }
        ScriptInfo Script { get; set; }
        int SayMsgCount { get; set; }
        int SayMsgTick { get; set; }
        bool DisableSayMsg { get; set; }
        int DisableSayMsgTick { get; set; }
        int CheckDupObjTick { get; set; }
        int DiscountForNightTick { get; set; }
        /// <summary>
        /// 是否在安全区域
        /// </summary>
        bool IsSafeArea { get; set; }
        /// <summary>
        /// 喊话消息间隔
        /// </summary>
        int ShoutMsgTick { get; set; }
        byte AttackSkillCount { get; set; }
        byte AttackSkillPointCount { get; set; }
        bool SmashSet { get; set; }
        bool HwanDevilSet { get; set; }
        bool PuritySet { get; set; }
        bool MundaneSet { get; set; }
        bool NokChiSet { get; set; }
        bool TaoBuSet { get; set; }
        bool FiveStringSet { get; set; }
        byte ValNpcType { get; set; }
        byte ValType { get; set; }
        byte ValLabel { get; set; }
        /// <summary>
        /// 复活戒指使用间隔时间
        /// </summary>
        int RevivalTick { get; set; }
        /// <summary>
        /// 掉物品
        /// </summary>
        bool NoDropItem { get; set; }
        /// <summary>
        /// 探测项链使用间隔
        /// </summary>
        int ProbeTick { get; set; }
        /// <summary>
        /// 传送戒指使用间隔
        /// </summary>
        int TeleportTick { get; set; }
        int DecHungerPointTick { get; set; }
        /// <summary>
        /// 气血石
        /// </summary>
        int AutoAddHpmpMode { get; set; }
        int CheckHpmpTick { get; set; }
        int KickOffLineTick { get; set; }
        /// <summary>
        /// 挂机
        /// </summary>
        bool OffLineFlag { get; set; }
        /// <summary>
        /// 挂机字符
        /// </summary>
        string OffLineLeaveWord { get; set; }
        /// <summary>
        /// Socket Handle
        /// </summary>
        int SocketId { get; set; }
        /// <summary>
        /// 人物连接到游戏网关SOCKETID
        /// </summary>
        ushort SocketIdx { get; set; }
        /// <summary>
        /// 人物所在网关号
        /// </summary>
        int GateIdx { get; set; }
        int SoftVersionDate { get; set; }
        /// <summary>
        /// 登录时间戳
        /// </summary>
        long LogonTime { get; set; }
        /// <summary>
        /// 战领沙城时间
        /// </summary>
        int LogonTick { get; set; }
        /// <summary>
        /// 是否进入游戏完成
        /// </summary>
        bool BoReadyRun { get; set; }
        /// <summary>
        /// 移动间隔
        /// </summary>
        int MapMoveTick { get; set; }
        /// <summary>
        /// 人物当前付费模式
        /// 1:试玩
        /// 2:付费
        /// 3:测试
        /// </summary>
        byte PayMent { get; set; }
        byte PayMode { get; set; }
        /// <summary>
        /// 当前会话ID
        /// </summary>
        int SessionId { get; set; }
        /// <summary>
        /// 全局会话信息
        /// </summary>
        AccountSession SessInfo { get; set; }
        int LoadTick { get; set; }
        /// <summary>
        /// 人物当前所在服务器序号
        /// </summary>
        byte ServerIndex { get; set; }
        /// <summary>
        /// 超时关闭链接
        /// </summary>
        bool BoEmergencyClose { get; set; }
        /// <summary>
        /// 掉线标志
        /// </summary>
        bool BoSoftClose { get; set; }
        /// <summary>
        /// 断线标志(@kick 命令)
        /// </summary>
        bool BoKickFlag { get; set; }
        /// <summary>
        /// 是否重连
        /// </summary>
        bool BoReconnection { get; set; }
        bool RcdSaved { get; set; }
        bool SwitchData { get; set; }
        bool SwitchDataOk { get; set; }
        string SwitchDataTempFile { get; set; }
        int WriteChgDataErrCount { get; set; }
        string SwitchMapName { get; set; }
        short SwitchMapX { get; set; }
        short SwitchMapY { get; set; }
        bool SwitchDataSended { get; set; }
        int ChgDataWritedTick { get; set; }
        /// <summary>
        /// 心灵启示
        /// </summary>
        bool AbilSeeHealGauge { get; set; }
        /// <summary>
        /// 攻击间隔
        /// </summary>
        int HitIntervalTime { get; set; }
        /// <summary>
        /// 魔法间隔
        /// </summary>
        int MagicHitIntervalTime { get; set; }
        /// <summary>
        /// 走路间隔
        /// </summary>
        int RunIntervalTime { get; set; }
        /// <summary>
        /// 走路间隔
        /// </summary>
        int WalkIntervalTime { get; set; }
        /// <summary>
        /// 换方向间隔
        /// </summary>
        int TurnIntervalTime { get; set; }
        /// <summary>
        /// 组合操作间隔
        /// </summary>
        int ActionIntervalTime { get; set; }
        /// <summary>
        /// 移动刺杀间隔
        /// </summary>
        int RunLongHitIntervalTime { get; set; }
        /// <summary>
        /// 跑位攻击间隔
        /// </summary>        
        int RunHitIntervalTime { get; set; }
        /// <summary>
        /// 走位攻击间隔
        /// </summary>        
        int WalkHitIntervalTime { get; set; }
        /// <summary>
        /// 跑位魔法间隔
        /// </summary>        
        int RunMagicIntervalTime { get; set; }
        /// <summary>
        /// 魔法攻击时间
        /// </summary>        
        int MagicAttackTick { get; set; }
        /// <summary>
        /// 魔法攻击间隔时间
        /// </summary>
        int MagicAttackInterval { get; set; }
        /// <summary>
        /// 人物跑动时间
        /// </summary>
        int MoveTick { get; set; }
        /// <summary>
        /// 人物攻击计数
        /// </summary>
        int AttackCount { get; set; }
        /// <summary>
        /// 人物攻击计数
        /// </summary>
        int AttackCountA { get; set; }
        /// <summary>
        /// 魔法攻击计数
        /// </summary>
        int MagicAttackCount { get; set; }
        /// <summary>
        /// 人物跑计数
        /// </summary>
        int MoveCount { get; set; }
        /// <summary>
        /// 超速计数
        /// </summary>
        int OverSpeedCount { get; set; }
        /// <summary>
        /// 复活戒指
        /// </summary>
        bool Revival { get; set; }
        /// <summary>
        /// 传送戒指
        /// </summary>
        bool Teleport { get; set; }
        /// <summary>
        /// 麻痹戒指
        /// </summary>
        bool Paralysis { get; set; }
        /// <summary>
        /// 火焰戒指
        /// </summary>
        bool FlameRing { get; set; }
        /// <summary>
        /// 治愈戒指
        /// </summary>
        bool RecoveryRing { get; set; }
        /// <summary>
        /// 未知戒指
        /// </summary>
        bool AngryRing { get; set; }
        /// <summary>
        /// 护身戒指
        /// </summary>
        bool MagicShield { get; set; }
        /// <summary>
        /// 防护身
        /// </summary>
        bool UnMagicShield { get; set; }
        /// <summary>
        /// 活力戒指
        /// </summary>
        bool MuscleRing { get; set; }
        /// <summary>
        /// 探测项链
        /// </summary>
        bool ProbeNecklace { get; set; }
        /// <summary>
        /// 防复活
        /// </summary>
        bool UnRevival { get; set; }
        /// <summary>
        /// 记忆全套
        /// </summary>
        bool RecallSuite { get; set; }
        /// <summary>
        /// 魔血一套
        /// </summary>
        int MoXieSuite { get; set; }
        /// <summary>
        /// 虹魔一套
        /// </summary>
        int SuckupEnemyHealthRate { get; set; }
        double SuckupEnemyHealth { get; set; }
        double BodyLuck { get; set; }
        int BodyLuckLevel { get; set; }
        bool DieInFight3Zone { get; set; }
        string GotoNpcLabel { get; set; }
        bool TakeDlgItem { get; set; }
        int DlgItemIndex { get; set; }
        int DelayCall { get; set; }
        int DelayCallTick { get; set; }
        bool IsDelayCall { get; set; }
        int DelayCallNpc { get; set; }
        string DelayCallLabel { get; set; }
        int LastNpc { get; set; }
        /// <summary>
        /// 职业属性点
        /// </summary>
        NakedAbility BonusAbil { get; set; }
        /// <summary>
        /// 玩家的变量P
        /// </summary>
        int[] MNVal { get; set; }
        /// <summary>
        /// 玩家的变量M
        /// </summary>
        int[] MNMval { get; set; }
        /// <summary>
        /// 玩家的变量D
        /// </summary>
        int[] MDyVal { get; set; }
        /// <summary>
        /// 玩家的变量
        /// </summary>
        string[] MNSval { get; set; }
        /// <summary>
        /// 人物变量  N
        /// </summary>
        int[] MNInteger { get; set; }
        /// <summary>
        /// 人物变量  S
        /// </summary>
        string[] MSString { get; set; }
        /// <summary>
        /// 服务器变量 W 0-20 个人服务器数值变量，不可保存，不可操作
        /// </summary>
        string[] ServerStrVal { get; set; }
        /// <summary>
        /// E 0-20 个人服务器字符串变量，不可保存，不可操作
        /// </summary>
        int[] ServerIntVal { get; set; }
        Dictionary<string, int> IntegerList { get; set; }
        Dictionary<string, string> m_StringList { get; set; }
        string ScatterItemName { get; set; }
        string ScatterItemOwnerName { get; set; }
        int ScatterItemX { get; set; }
        int ScatterItemY { get; set; }
        string ScatterItemMapName { get; set; }
        string ScatterItemMapDesc { get; set; }
        /// <summary>
        /// 技能表
        /// </summary>
        IList<UserMagic> MagicList { get; set; }
        /// <summary>
        /// 组队长
        /// </summary>
        int GroupOwner { get; set; }
        /// <summary>
        /// 组成员
        /// </summary>
        IList<IPlayerActor> GroupMembers { get; set; }
        string PlayDiceLabel { get; set; }
        bool IsTimeRecall { get; set; }
        int TimeRecallTick { get; set; }
        string TimeRecallMoveMap { get; set; }
        short TimeRecallMoveX { get; set; }
        short TimeRecallMoveY { get; set; }
        /// <summary>
        /// 减少勋章持久间隔
        /// </summary>
        int DecLightItemDrugTick { get; set; }
        /// <summary>
        /// 保存人物数据时间间隔
        /// </summary>
        int SaveRcdTick { get; set; }
        byte Bright { get; set; }
        bool IsNewHuman { get; set; }
        bool IsSendNotice { get; set; }
        int WaitLoginNoticeOkTick { get; set; }
        bool LoginNoticeOk { get; set; }
        /// <summary>
        /// 试玩模式
        /// </summary>
        bool TryPlayMode { get; set; }
        int ShowLineNoticeTick { get; set; }
        int ShowLineNoticeIdx { get; set; }
        int SoftVersionDateEx { get; set; }
        /// <summary>
        /// 可点击脚本标签字典
        /// </summary>
        Hashtable CanJmpScriptLableMap { get; set; }
        int ScriptGotoCount { get; set; }
        /// <summary>
        /// 用于处理 @back 脚本命令
        /// </summary>
        string ScriptCurrLable { get; set; }
        /// <summary>
        /// 用于处理 @back 脚本命令
        /// </summary>        
        string ScriptGoBackLable { get; set; }
        /// <summary>
        /// 转身间隔
        /// </summary>
        int TurnTick { get; set; }
        int OldIdent { get; set; }
        byte MBtOldDir { get; set; }
        /// <summary>
        /// 第一个操作
        /// </summary>
        bool IsFirstAction { get; set; }
        /// <summary>
        /// 二次操作之间间隔时间
        /// </summary>        
        int ActionTick { get; set; }
        /// <summary>
        /// 配偶名称
        /// </summary>
        string DearName { get; set; }
        IPlayerActor DearHuman { get; set; }
        /// <summary>
        /// 是否允许夫妻传送
        /// </summary>
        bool CanDearRecall { get; set; }
        /// <summary>
        /// 是否允许师徒传送
        /// </summary>
        bool CanMasterRecall { get; set; }
        /// <summary>
        /// 夫妻传送时间
        /// </summary>
        int DearRecallTick { get; set; }
        int MasterRecallTick { get; set; }
        /// <summary>
        /// 师徒名称
        /// </summary>
        string MasterName { get; set; }
        IPlayerActor MasterHuman { get; set; }
        IList<IActor> MasterList { get; set; }
        bool IsMaster { get; set; }
        /// <summary>
        /// 对面玩家
        /// </summary>
        int PoseBaseObject { get; set; }
        /// <summary>
        /// 声望点
        /// </summary>
        byte CreditPoint { get; set; }
        /// <summary>
        /// 离婚次数
        /// </summary>        
        byte MarryCount { get; set; }
        /// <summary>
        /// 转生等级
        /// </summary>
        byte ReLevel { get; set; }

        byte ReColorIdx { get; set; }

        int ReColorTick { get; set; }

        /// <summary>
        /// 杀怪经验倍数
        /// </summary>
        int MNKillMonExpMultiple { get; set; }

        /// <summary>
        /// 处理消息循环时间控制
        /// </summary>        
        int GetMessageTick { get; set; }

        bool IsSetStoragePwd { get; set; }

        bool IsReConfigPwd { get; set; }

        bool IsCheckOldPwd { get; set; }

        bool IsUnLockPwd { get; set; }

        bool IsUnLockStoragePwd { get; set; }

        /// <summary>
        /// 锁密码
        /// </summary>
        bool IsPasswordLocked { get; set; }

        byte PwdFailCount { get; set; }
        /// <summary>
        /// 是否启用锁登录功能
        /// </summary>
        bool IsLockLogon { get; set; }

        /// <summary>
        /// 是否打开登录锁
        /// </summary>        
        bool IsLockLogoned { get; set; }

        string MSTempPwd { get; set; }

        string StoragePwd { get; set; }

        bool IsStartMarry { get; set; }

        bool IsStartMaster { get; set; }

        bool IsStartUnMarry { get; set; }

        bool IsStartUnMaster { get; set; }

        /// <summary>
        /// 禁止发方字(发的文字只能自己看到)
        /// </summary>
        bool FilterSendMsg { get; set; }

        /// <summary>
        /// 杀怪经验倍数(此数除以 100 为真正倍数)
        /// </summary>        
        int KillMonExpRate { get; set; }

        /// <summary>
        /// 人物攻击力倍数(此数除以 100 为真正倍数)
        /// </summary>        
        int PowerRate { get; set; }

        int KillMonExpRateTime { get; set; }

        int PowerRateTime { get; set; }

        int ExpRateTick { get; set; }

        /// <summary>
        /// 技巧项链
        /// </summary>
        bool FastTrain { get; set; }

        /// <summary>
        /// 是否允许使用物品
        /// </summary>
        bool BoCanUseItem { get; set; }

        /// <summary>
        /// 是否允许交易物品
        /// </summary>
        bool IsCanDeal { get; set; }

        bool IsCanDrop { get; set; }

        bool IsCanGetBackItem { get; set; }

        bool IsCanWalk { get; set; }

        bool IsCanRun { get; set; }

        bool IsCanHit { get; set; }

        bool IsCanSpell { get; set; }

        bool IsCanSendMsg { get; set; }

        /// <summary>
        /// 会员类型
        /// </summary>
        int MemberType { get; set; }

        /// <summary>
        /// 会员等级
        /// </summary> 
        byte MemberLevel { get; set; }

        /// <summary>
        /// 发祝福语标志
        /// </summary> 
        bool BoSendMsgFlag { get; set; }

        bool BoChangeItemNameFlag { get; set; }

        /// <summary>
        /// 游戏币
        /// </summary>
        int GameGold { get; set; }

        /// <summary>
        /// 是否自动减游戏币
        /// </summary>        
        bool BoDecGameGold { get; set; }

        int DecGameGoldTime { get; set; }

        int DecGameGoldTick { get; set; }

        int DecGameGold { get; set; }

        // 一次减点数
        bool BoIncGameGold { get; set; }

        // 是否自动加游戏币
        int IncGameGoldTime { get; set; }

        int IncGameGoldTick { get; set; }

        int IncGameGold { get; set; }

        // 一次减点数
        int GamePoint { get; set; }

        // 游戏点数
        int IncGamePointTick { get; set; }

        int PayMentPoint { get; set; }

        int PayMentPointTick { get; set; }

        int DecHpTick { get; set; }

        int IncHpTick { get; set; }

        /// <summary>
        /// PK 死亡掉经验，不够经验就掉等级
        /// </summary>
        int PkDieLostExp { get; set; }

        /// <summary>
        /// PK 死亡掉等级
        /// </summary>
        byte PkDieLostLevel { get; set; }

        /// <summary>
        /// 私聊对象
        /// </summary>
        IActor WhisperHuman { get; set; }

        /// <summary>
        /// 清理无效对象间隔
        /// </summary>
        int ClearInvalidObjTick { get; set; }

        short Contribution { get; set; }

        string RankLevelName { get; set; }

        bool IsFilterAction { get; set; }

        int AutoGetExpTick { get; set; }

        int AutoGetExpTime { get; set; }

        int AutoGetExpPoint { get; set; }

        IEnvirnoment AutoGetExpEnvir { get; set; }

        bool AutoGetExpInSafeZone { get; set; }

        Dictionary<string, DynamicVar> DynamicVarMap { get; set; }

        short ClientTick { get; set; }

        /// <summary>
        /// 进入速度测试模式
        /// </summary>
        bool TestSpeedMode { get; set; }

        string RandomNo { get; set; }

        /// <summary>
        /// 刷新包裹间隔
        /// </summary>
        int QueryBagItemsTick { get; set; }

        bool IsTimeGoto { get; set; }

        int TimeGotoTick { get; set; }

        string TimeGotoLable { get; set; }

        IActor TimeGotoNpc { get; set; }

        /// <summary>
        /// 个人定时器
        /// </summary>
        int[] AutoTimerTick { get; set; }

        /// <summary>
        /// 个人定时器 时间间隔
        /// </summary>
        int[] AutoTimerStatus { get; set; }

        /// <summary>
        /// 0-攻击力增加 1-魔法增加  2-道术增加(无极真气) 3-攻击速度 4-HP增加(酒气护体)
        /// 5-增加MP上限 6-减攻击力 7-减魔法 8-减道术 9-减HP 10-减MP 11-敏捷 12-增加防
        /// 13-增加魔防 14-增加道术上限(虎骨酒) 15-连击伤害增加(醉八打) 16-内力恢复速度增加(何首养气酒)
        /// 17-内力瞬间恢复增加(何首凝神酒) 18-增加斗转上限(培元酒) 19-不死状态 21-弟子心法激活
        /// 22-移动减速 23-定身(十步一杀)
        /// </summary>
        ushort[] ExtraAbil { get; set; }

        byte[] ExtraAbilFlag { get; set; }

        /// <summary>
        /// 0-攻击力增加 1-魔法增加  2-道术增加(无极真气) 3-攻击速度 4-HP增加(酒气护体)
        /// 5-增加MP上限 6-减攻击力 7-减魔法 8-减道术 9-减HP 10-减MP 11-敏捷 12-增加防
        /// 13-增加魔防 14-增加道术上限(虎骨酒) 15-连击伤害增加(醉八打) 16-内力恢复速度增加(何首养气酒)
        /// 17-内力瞬间恢复增加(何首凝神酒) 18-增加斗转上限(培元酒) 19-不死状态 20-道术+上下限(除魔药剂类) 21-弟子心法激活
        /// 22-移动减速 23-定身(十步一杀)
        /// </summary>
        int[] ExtraAbilTimes { get; set; }

        /// <summary>
        /// 点击NPC时间
        /// </summary>
        int ClickNpcTime { get; set; }

        /// <summary>
        /// 是否开通元宝交易服务
        /// </summary>
        bool SaleDeal { get; set; }

        /// <summary>
        /// 确认元宝寄售标志
        /// </summary>
        bool SellOffConfirm { get; set; }

        /// <summary>
        /// 元宝寄售物品列表
        /// </summary>
        IList<UserItem> SellOffItemList { get; set; }

        byte[] QuestUnitOpen { get; set; }

        byte[] QuestUnit { get; set; }

        byte[] QuestFlag { get; set; }

        MarketUser MarketUser { get; set; }

        int FightExp { get; set; }

        void FeatureChanged();

        void RunNotice();

        void UserLogon();

        void SearchViewRange();

        void GameTimeChanged();

        void Run();

        void Disappear();

        void DealCancelA();

        int GetMyStatus();

        void HasLevelUp(int level);

        bool IsAddWeightAvailable(int nWeight);

        void MapRandomMove(string sMapName, int nInt);

        void SetQuestFlagStatus(int nFlag, int nValue);

        int GetQuestUnitOpenStatus(int nFlag);

        void SetQuestUnitOpenStatus(int nFlag, int nValue);

        int GetQuestUnitStatus(int nFlag);

        void SetQuestUnitStatus(int nFlag, int nValue);

        IActor MakeSlave(string sMonName, int nMakeLevel, int nExpLevel, int nMaxMob, int dwRoyaltySec);

        void SysMsg(string sMsg, MsgColor msgColor, MsgType msgType);

        string GetMyInfo();

        byte PvpLevel();

        UserMagic GetMagicInfo(string magicName);

        UserItem QuestCheckItem(string sItemName, ref int nCount, ref int nParam, ref int nDura);

        bool CheckItemBindUse(UserItem userItem);

        bool IsGuildMaster();

        int GetQuestFalgStatus(int nFlag);

        void SendDelItems(UserItem userItem);

        void SendAddItem(UserItem userItem);

        void DelBagItem(UserItem userItem);

        bool DelBagItem(int makeIndex, string itemName);

        void SendUpdateItem(UserItem userItem);

        void SendDelMagic(UserMagic userMagic);

        void SendAddMagic(UserMagic userMagic);

        bool QuestTakeCheckItem(UserItem userItem);

        bool IncGold(int gold);

        bool DecGold(int gold);

        void GoldChanged();

        void GameGoldChanged();

        bool DropItemDown(UserItem userItem, int nScatterRange, bool boDieDrop, int itemOfCreat, int dropCreat);

        UserItem CheckItemCount(string sItemName, ref int nCount);

        bool SellOffInTime(int time);

        bool GetFrontPosition(ref short nX, ref short nY);

        bool IsTrainingSkill(int nIndex);

        void ClientQueryBagItems();

        void SendGroupText(string sText);

        void SendDefMessage(short wIdent, int nRecog, int nParam, int nTag, int nSeries);

        void SendDefMessage(short wIdent, int nRecog, int nParam, int nTag, int nSeries, string sMsg);

        void RefRankInfo(short nRankNo, string sRankName);

        void ReAlive();

        void LeaveGroup();

        void JoinGroup(IPlayerActor actor);

        bool IsBlockWhisper(string sName);

        void Whisper(string sName, string sText);

        void ProcessUserLineMsg(string sText);

        bool LableIsCanJmp(string sText);

        bool AddItemToBag(UserItem userItem);

        UserItem CheckItems(string itemName);

        void ReQuestGuildWar(string guildName);

        void RecallHuman(string chrName);

        void SendUseItems();

        void StatusChanged();

        int GetCharStatus();

        void RefMyStatus();

        byte GetBackDir(byte dir);

        void ClientGuildAlly();

        void ClientGuildBreakAlly(string guildName);

        bool IsEnoughBag();

        void DealCancel();

        void AbilCopyToWAbil();

        void ClearStatusTime();

        void DelMember(IPlayerActor playerActor);

        bool InSafeArea();

        void OpenDealDlg(IPlayerActor playerActor);

        bool CanMove(short nX, short nY, bool boFlag);

        bool CanRun(short nCurrX, short nCurrY, short nX, short nY, bool boFlag);

        void GainExp(int dwExp);

        void AddBodyLuck(double dLuck);

        void SetPkFlag(IActor baseObject);

        void IncPkPoint(int nPoint);

        void MakeWeaponUnlock();

        void AttPowerUp(int nPower, int nTime);

        void ChangeSpaceMove(IEnvirnoment envir, short nX, short nY);

        void ChangeItemByJob(ref ClientItem citem, int level);

        void NpcGotoLable(INormNpc actor, string sLable, bool boMaster);

        void GetScriptLabel(string sMsg);

        void SendPriorityMsg(int wIdent, int wParam, int nParam1, int nParam2, int nParam3, string sMsg = "", MessagePriority Priority = MessagePriority.Normal);

        void SendSocket(CommandMessage defMsg, string sMsg);
    }
}