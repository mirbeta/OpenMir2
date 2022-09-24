using Spectre.Console;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Conf.Model
{
    public class GameSvrConf
    {
        public string sDBType;
        public string ConnctionString;
        public string ServerName;
        public string ServerIPaddr;
        public string sWebSite;
        public string sBbsSite;
        public string sClientDownload;
        public string sQQ;
        public string sPhone;
        public string sBankAccount0;
        public string sBankAccount1;
        public string sBankAccount2;
        public string sBankAccount3;
        public string sBankAccount4;
        public string sBankAccount5;
        public string sBankAccount6;
        public string sBankAccount7;
        public string sBankAccount8;
        public string sBankAccount9;
        public int nServerNumber;
        /// <summary>
        /// 不刷怪模式
        /// </summary>
        public bool VentureServer;
        /// <summary>
        /// 测试模式
        /// </summary>
        public bool TestServer;
        /// <summary>
        /// 免费模式
        /// </summary>
        public bool ServiceMode;
        /// <summary>
        /// PEV模式
        /// </summary>
        public bool PveServer;
        public int TestLevel;
        public int TestGold;
        public int TestUserLimit;
        public int SendBlock;
        public int CheckBlock;
        public bool boDropLargeBlock;
        public int AvailableBlock;
        public int GateLoad;
        /// <summary>
        /// 服务器上线人数（默认1000）
        /// </summary>
        public int UserFull;
        /// <summary>
        /// 怪物行动速度(默认300)
        /// </summary>
        public int ZenFastStep;
        public string sGateAddr;
        public int nGatePort;
        public string sDBAddr;
        public int nDBPort;
        public string sIDSAddr;
        public int nIDSPort;
        /// <summary>
        /// Master节点服务器IP
        /// </summary>
        public string MsgSrvAddr;
        /// <summary>
        /// Master节点服务器端口
        /// </summary>
        public int MsgSrvPort;
        public string LogServerAddr;
        public int LogServerPort;
        public bool DiscountForNightTime;
        public int HalfFeeStart;
        public int HalfFeeEnd;
        public bool ViewHackMessage;
        public bool ViewAdmissionFailure;
        public string BaseDir;
        public string GuildDir;
        public string GuildFile;
        public string VentureDir;
        public string ConLogDir;
        public string CastleDir;
        public string CastleFile;
        public string EnvirDir;
        public string MapDir;
        public string NoticeDir;
        public string LogDir;
        public string ClientFile1;
        public string ClientFile2;
        public string ClientFile3;
        public string ClothsMan;
        public string ClothsWoman;
        public string WoodenSword;
        public string Candle;
        public string BasicDrug;
        public string GoldStone;
        public string SilverStone;
        public string SteelStone;
        public string CopperStone;
        public string BlackStone;
        public string GemStone1;
        public string GemStone2;
        public string GemStone3;
        public string GemStone4;
        public string[] Zuma;
        public string Bee;
        public string Spider;
        public string WomaHorn;
        public string ZumaPiece;
        public string GameGoldName;
        public string GamePointName;
        public string PayMentPointName;
        public int DBSocket;
        public int HealthFillTime;
        public int SpellFillTime;
        public int MonUpLvNeedKillBase;
        public int MonUpLvRate;
        public int[] MonUpLvNeedKillCount;
        public byte[] SlaveColor;
        public int[] NeedExps;
        public byte[] WideAttack;
        public byte[] CrsAttack;
        public byte[,,] SpitMap;
        public string HomeMap;
        public short HomeX;
        public short HomeY;
        public string RedHomeMap;
        public short RedHomeX;
        public short RedHomeY;
        public string RedDieHomeMap;
        public short RedDieHomeX;
        public short RedDieHomeY;
        public bool JobHomePoint;
        public string WarriorHomeMap;
        public short WarriorHomeX;
        public short WarriorHomeY;
        public string WizardHomeMap;
        public short WizardHomeX;
        public short WizardHomeY;
        public string TaoistHomeMap;
        public short TaoistHomeX;
        public short TaoistHomeY;
        public int DecPkPointTime;
        public int DecPkPointCount;
        public int dwPKFlagTime;
        public int KillHumanAddPKPoint;
        public int KillHumanDecLuckPoint;
        public int DecLightItemDrugTime;
        /// <summary>
        /// 安全区范围大小
        /// </summary>
        public int SafeZoneSize;
        public int StartPointSize;
        public int HumanGetMsgTime;
        public int GroupMembersMax;
        public int WarrMon;
        public int WizardMon;
        public int TaosMon;
        public int MonHum;
        public string FireBallSkill;
        public string HealSkill;
        public string RingSkill;
        public byte[] ReNewNameColor;
        public int ReNewNameColorTime;
        public bool ReNewChangeColor;
        public bool ReNewLevelClearExp;
        public NakedAbility BonusAbilofWarr;
        public NakedAbility BonusAbilofWizard;
        public NakedAbility BonusAbilofTaos;
        public NakedAbility NakedAbilofWarr;
        public NakedAbility NakedAbilofWizard;
        public NakedAbility NakedAbilofTaos;
        /// <summary>
        /// 武器升级做高点数
        /// </summary>
        public int UpgradeWeaponMaxPoint;
        /// <summary>
        /// 武器升级价格
        /// </summary>
        public int UpgradeWeaponPrice;
        /// <summary>
        /// 武器升级所需时间（秒）
        /// </summary>
        public int UPgradeWeaponGetBackTime;
        /// <summary>
        /// 清理多少天内没取走的升级数据
        /// </summary>
        public int ClearExpireUpgradeWeaponDays;
        /// <summary>
        /// 武器升级攻击力成功几率
        /// </summary>
        public int UpgradeWeaponDCRate;
        /// <summary>
        /// 武器升级攻击力成功几率
        /// </summary>
        public int UpgradeWeaponDCTwoPointRate;
        /// <summary>
        /// 武器升级攻击力成功几率
        /// </summary>
        public int UpgradeWeaponDCThreePointRate;
        /// <summary>
        /// 武器升级道术成功几率
        /// </summary>
        public int UpgradeWeaponSCRate;
        /// <summary>
        /// 武器升级道术成功几率
        /// </summary>
        public int UpgradeWeaponSCTwoPointRate;
        /// <summary>
        /// 武器升级道术成功几率
        /// </summary>
        public int UpgradeWeaponSCThreePointRate;
        /// <summary>
        /// 武器升级魔法成功几率
        /// </summary>
        public int UpgradeWeaponMCRate;
        /// <summary>
        /// 武器升级魔法成功几率
        /// </summary>
        public int UpgradeWeaponMCTwoPointRate;
        /// <summary>
        /// 武器升级魔法成功几率
        /// </summary>
        public int UpgradeWeaponMCThreePointRate;
        /// <summary>
        /// 怪物处理间隔
        /// 处理怪物间隔时间，此设置数字越大，怪物行动越慢
        /// </summary>
        public int ProcessMonstersTime;
        /// <summary>
        /// 怪物刷新间隔
        /// 刷怪间隔控制，数字越大，刷怪速度越慢
        /// </summary>
        public int RegenMonstersTime;
        /// <summary>
        /// 怪物刷新倍率
        /// 刷怪倍率，倍率除以10为实际倍率(设置为10则为1:1)，此倍率以刷怪文件设置为准，数字越大，刷怪数量越小
        /// </summary>
        public int MonGenRate;
        public int ProcessMonRandRate;
        public int ProcessMonLimitCount;
        public int SoftVersionDate;
        public bool CanOldClientLogon;
        public int ConsoleShowUserCountTime;
        public int ShowLineNoticeTime;
        public int LineNoticeColor;
        public int StartCastleWarDays;
        public int StartCastlewarTime;
        public int ShowCastleWarEndMsgTime;
        public int CastleWarTime;
        public int GetCastleTime;
        public int GuildWarTime;
        public int BuildGuildPrice;
        public int GuildWarPrice;
        public int MakeDurgPrice;
        /// <summary>
        /// 玩家最大金币上限
        /// </summary>
        public int HumanMaxGold;
        public int HumanTryModeMaxGold;
        public int TryModeLevel;
        public bool TryModeUseStorage;
        public int CanShoutMsgLevel;
        public bool ShowMakeItemMsg;
        public bool ShutRedMsgShowGMName;
        public int SayMsgMaxLen;
        public int SayMsgTime;
        public int SayMsgCount;
        public int DisableSayMsgTime;
        public int SayRedMsgMaxLen;
        public bool ShowGuildName;
        public bool ShowRankLevelName;
        public bool MonSayMsg;
        public int StartPermission;
        public bool IsKillHumanWinLevel;
        public bool IsKilledLostLevel;
        public bool IsKillHumanWinExp;
        public bool IsKilledLostExp;
        public int KillHumanWinLevel;
        public int KilledLostLevel;
        public int KillHumanWinExp;
        public int KillHumanLostExp;
        public int HumanLevelDiffer;
        public int MonsterPowerRate;
        public int ItemsPowerRate;
        public int ItemsACPowerRate;
        /// <summary>
        /// 是否显示在线人数
        /// </summary>
        public bool SendOnlineCount;
        public int SendOnlineCountRate;
        /// <summary>
        /// NPC点击间隔
        /// </summary>
        public int ClickNpcTime;
        /// <summary>
        /// 踢出未登陆人物
        /// </summary>
        public int ThcHumanTime;
        /// <summary>
        /// 包裹刷新间隔
        /// </summary>
        public int QueryBagItemsTick;
        /// <summary>
        /// 发送在线人数间隔时间
        /// </summary>
        public int SendOnlineTime;
        public int SaveHumanRcdTime;
        public int HumanFreeDelayTime;
        public int MakeGhostTime;
        public int ClearDropOnFloorItemTime;
        public int FloorItemCanPickUpTime;
        /// <summary>
        /// 是否启用密码保护系统
        /// </summary>
        public bool PasswordLockSystem;
        /// <summary>
        /// 是否锁定交易操作
        /// </summary>
        public bool LockDealAction;
        /// <summary>
        /// 是否锁定扔物品操作
        /// </summary>
        public bool LockDropAction;
        /// <summary>
        /// 是否锁定取仓库操作
        /// </summary>
        public bool LockGetBackItemAction;
        /// <summary>
        /// 是否锁定走操作
        /// </summary>
        public bool LockHumanLogin;
        /// <summary>
        /// 是否锁定走操作
        /// </summary>
        public bool LockWalkAction;
        /// <summary>
        /// 是否锁定跑操作
        /// </summary>
        public bool LockRunAction;
        /// <summary>
        /// 是否锁定攻击操作
        /// </summary>
        public bool LockHitAction;
        /// <summary>
        /// 是否锁定魔法操作
        /// </summary>
        public bool LockSpellAction;
        /// <summary>
        /// 是否锁定发信息操作
        /// </summary>
        public bool LockSendMsgAction;
        /// <summary>
        /// 是否锁定使用物品操作
        /// </summary>
        public bool LockUserItemAction;
        /// <summary>
        /// 锁定时进入隐身状态
        /// </summary>
        public bool LockInObModeAction;
        /// <summary>
        /// 输入密码错误超过 指定次数则锁定密码
        /// </summary>
        public int PasswordErrorCountLock;
        /// <summary>
        /// 输入密码错误超过限制则踢下线
        /// </summary>
        public bool PasswordErrorKick;
        /// <summary>
        /// 消息发送范围
        /// </summary>
        public byte SendRefMsgRange;
        public bool DecLampDura;
        public bool HungerSystem;
        public bool HungerDecHP;
        public bool HungerDecPower;
        public bool DiableHumanRun;
        public bool boRunHuman;
        public bool boRunMon;
        public bool boRunNpc;
        public bool boRunGuard;
        public bool boWarDisHumRun;
        public bool boGMRunAll;
        public bool boSafeZoneRunAll;
        public int TryDealTime;
        public int DealOKTime;
        /// <summary>
        /// 是否禁止取回交易物品
        /// </summary>
        public bool CanNotGetBackDeal;
        public bool DisableDeal;
        public int MasterOKLevel;
        public int MasterOKCreditPoint;
        public int nMasterOKBonusPoint;
        public bool boPKLevelProtect;
        public int nPKProtectLevel;
        public int nRedPKProtectLevel;
        public int ItemPowerRate;
        public int ItemExpRate;
        public int ScriptGotoCountLimit;
        public byte btHearMsgFColor;
        public byte btHearMsgBColor;
        public byte btWhisperMsgFColor;
        public byte btWhisperMsgBColor;
        public byte btGMWhisperMsgFColor;
        public byte btGMWhisperMsgBColor;
        public byte CryMsgFColor;
        public byte CryMsgBColor;
        public byte GreenMsgFColor;
        public byte GreenMsgBColor;
        public byte BlueMsgFColor;
        public byte BlueMsgBColor;
        public byte RedMsgFColor;
        public byte RedMsgBColor;
        public byte GuildMsgFColor;
        public byte GuildMsgBColor;
        public byte GroupMsgFColor;
        public byte GroupMsgBColor;
        public byte CustMsgFColor;
        public byte CustMsgBColor;
        public byte PurpleMsgFColor;
        public byte PurpleMsgBColor;
        public int MonRandomAddValue;
        public int MakeRandomAddValue;
        public int WeaponDCAddValueMaxLimit;
        public int WeaponDCAddValueRate;
        public int WeaponMCAddValueMaxLimit;
        public int WeaponMCAddValueRate;
        public int WeaponSCAddValueMaxLimit;
        public int WeaponSCAddValueRate;
        public int DressDCAddRate;
        public int DressDCAddValueMaxLimit;
        public int DressDCAddValueRate;
        public int DressMCAddRate;
        public int DressMCAddValueMaxLimit;
        public int DressMCAddValueRate;
        public int DressSCAddRate;
        public int DressSCAddValueMaxLimit;
        public int nDressSCAddValueRate;
        public int NeckLace202124DCAddRate;
        public int NeckLace202124DCAddValueMaxLimit;
        public int NeckLace202124DCAddValueRate;
        public int NeckLace202124MCAddRate;
        public int NeckLace202124MCAddValueMaxLimit;
        public int NeckLace202124MCAddValueRate;
        public int NeckLace202124SCAddRate;
        public int NeckLace202124SCAddValueMaxLimit;
        public int NeckLace202124SCAddValueRate;
        public int NeckLace19DCAddRate;
        public int NeckLace19DCAddValueMaxLimit;
        public int NeckLace19DCAddValueRate;
        public int NeckLace19MCAddRate;
        public int NeckLace19MCAddValueMaxLimit;
        public int NeckLace19MCAddValueRate;
        public int NeckLace19SCAddRate;
        public int NeckLace19SCAddValueMaxLimit;
        public int NeckLace19SCAddValueRate;
        public int ArmRing26DCAddRate;
        public int ArmRing26DCAddValueMaxLimit;
        public int ArmRing26DCAddValueRate;
        public int ArmRing26MCAddRate;
        public int ArmRing26MCAddValueMaxLimit;
        public int ArmRing26MCAddValueRate;
        public int ArmRing26SCAddRate;
        public int ArmRing26SCAddValueMaxLimit;
        public int ArmRing26SCAddValueRate;
        public int Ring22DCAddRate;
        public int Ring22DCAddValueMaxLimit;
        public int Ring22DCAddValueRate;
        public int Ring22MCAddRate;
        public int Ring22MCAddValueMaxLimit;
        public int Ring22MCAddValueRate;
        public int Ring22SCAddRate;
        public int Ring22SCAddValueMaxLimit;
        public int Ring22SCAddValueRate;
        public int Ring23DCAddRate;
        public int Ring23DCAddValueMaxLimit;
        public int Ring23DCAddValueRate;
        public int Ring23MCAddRate;
        public int Ring23MCAddValueMaxLimit;
        public int Ring23MCAddValueRate;
        public int Ring23SCAddRate;
        public int Ring23SCAddValueMaxLimit;
        public int Ring23SCAddValueRate;
        public int HelMetDCAddRate;
        public int HelMetDCAddValueMaxLimit;
        public int HelMetDCAddValueRate;
        public int HelMetMCAddRate;
        public int HelMetMCAddValueMaxLimit;
        public int HelMetMCAddValueRate;
        public int HelMetSCAddRate;
        public int HelMetSCAddValueMaxLimit;
        public int HelMetSCAddValueRate;
        public int UnknowHelMetACAddRate;
        public int UnknowHelMetACAddValueMaxLimit;
        public int UnknowHelMetMACAddRate;
        public int UnknowHelMetMACAddValueMaxLimit;
        public int UnknowHelMetDCAddRate;
        public int UnknowHelMetDCAddValueMaxLimit;
        public int UnknowHelMetMCAddRate;
        public int UnknowHelMetMCAddValueMaxLimit;
        public int UnknowHelMetSCAddRate;
        public int UnknowHelMetSCAddValueMaxLimit;
        public int UnknowRingACAddRate;
        public int UnknowRingACAddValueMaxLimit;
        public int UnknowRingMACAddRate;
        public int UnknowRingMACAddValueMaxLimit;
        public int UnknowRingDCAddRate;
        public int UnknowRingDCAddValueMaxLimit;
        public int UnknowRingMCAddRate;
        public int UnknowRingMCAddValueMaxLimit;
        public int UnknowRingSCAddRate;
        public int UnknowRingSCAddValueMaxLimit;
        public int UnknowNecklaceACAddRate;
        public int UnknowNecklaceACAddValueMaxLimit;
        public int UnknowNecklaceMACAddRate;
        public int UnknowNecklaceMACAddValueMaxLimit;
        public int UnknowNecklaceDCAddRate;
        public int UnknowNecklaceDCAddValueMaxLimit;
        public int UnknowNecklaceMCAddRate;
        public int UnknowNecklaceMCAddValueMaxLimit;
        public int UnknowNecklaceSCAddRate;
        public int UnknowNecklaceSCAddValueMaxLimit;
        public int MonOneDropGoldCount;
        /// <summary>
        /// 客户端时间
        /// </summary>
        public bool SendCurTickCount;
        /// <summary>
        /// 挖矿命中率
        /// </summary>        
        public int MakeMineHitRate;
        /// <summary>
        /// 挖矿率
        /// </summary>        
        public int MakeMineRate;
        public int StoneTypeRate;
        public int StoneTypeRateMin;
        public int GoldStoneMin;
        public int GoldStoneMax;
        public int SilverStoneMin;
        public int SilverStoneMax;
        public int SteelStoneMin;
        public int SteelStoneMax;
        public int BlackStoneMin;
        public int BlackStoneMax;
        public int StoneMinDura;
        public int StoneGeneralDuraRate;
        public int StoneAddDuraRate;
        public int StoneAddDuraMax;
        public int WinLottery6Min;
        public int WinLottery6Max;
        public int WinLottery5Min;
        public int WinLottery5Max;
        public int WinLottery4Min;
        public int WinLottery4Max;
        public int WinLottery3Min;
        public int WinLottery3Max;
        public int WinLottery2Min;
        public int WinLottery2Max;
        public int WinLottery1Min;
        public int WinLottery1Max;
        public int WinLottery1Gold;
        public int WinLottery2Gold;
        public int WinLottery3Gold;
        public int WinLottery4Gold;
        public int WinLottery5Gold;
        public int WinLottery6Gold;
        public int WinLotteryRate;
        public int WinLotteryCount;
        public int NoWinLotteryCount;
        public int WinLotteryLevel1;
        public int WinLotteryLevel2;
        public int WinLotteryLevel3;
        public int WinLotteryLevel4;
        public int WinLotteryLevel5;
        public int WinLotteryLevel6;
        public int ItemNumber;
        public int ItemNumberEx;
        public int GuildRecallTime;
        public int GroupRecallTime;
        public bool ControlDropItem;
        public bool InSafeDisableDrop;
        public int CanDropGold;
        public int CanDropPrice;
        public bool SendCustemMsg;
        public bool SubkMasterSendMsg;
        /// <summary>
        /// 特修价格倍数
        /// </summary>
        public int SuperRepairPriceRate;
        /// <summary>
        /// 普通修理掉持久数(特持久上限减下限再除以此数为减的数值)
        /// </summary>        
        public int RepairItemDecDura;
        public bool DieScatterBag;
        public int DieScatterBagRate;
        public bool DieRedScatterBagAll;
        public int DieDropUseItemRate;
        public int DieRedDropUseItemRate;
        public bool DieDropGold;
        public bool KillByHumanDropUseItem;
        public bool KillByMonstDropUseItem;
        public bool KickExpireHuman;
        public int GuildRankNameLen;
        public int GuildMemberMaxLimit;
        public int GuildNameLen;
        public int nCastleNameLen;
        /// <summary>
        /// 中毒几率
        /// </summary>
        public int AttackPosionRate;
        /// <summary>
        /// 中毒持续时间
        /// </summary>
        public ushort AttackPosionTime;
        /// <summary>
        /// 复活间隔时间
        /// </summary>
        public int RevivalTime;
        public bool boUserMoveCanDupObj;
        public bool boUserMoveCanOnItem;
        public int dwUserMoveTime;
        public int dwPKDieLostExpRate;
        public int nPKDieLostLevelRate;
        public byte btPKFlagNameColor;
        public byte btPKLevel1NameColor;
        public byte btPKLevel2NameColor;
        public byte btAllyAndGuildNameColor;
        public byte WarGuildNameColor;
        public byte InFreePKAreaNameColor;
        public bool SpiritMutiny;
        public int SpiritMutinyTime;
        public int SpiritPowerRate;
        public bool MasterDieMutiny;
        public int MasterDieMutinyRate;
        public int MasterDieMutinyPower;
        public int MasterDieMutinySpeed;
        public bool BBMonAutoChangeColor;
        public int BBMonAutoChangeColorTime;
        public bool boOldClientShowHiLevel;
        public bool ShowScriptActionMsg;
        public int RunSocketDieLoopLimit;
        public bool ThreadRun;
        public bool ShowExceptionMsg;
        public bool ShowPreFixMsg;
        /// <summary>
        /// 魔法锁定范围
        /// </summary>
        public int MagicAttackRage;
        public bool nBoBoall;
        /// <summary>
        /// 物品掉落范围
        /// </summary>
        public int DropItemRage;
        public string Skeleton;
        public int SkeletonCount;
        public TRecallMigic[] SkeletonArray;
        public string Dragon;
        public string Dragon1;
        public int DragonCount;
        public TRecallMigic[] DragonArray;
        public string Angel;
        public int AmyOunsulPoint;
        public bool DisableInSafeZoneFireCross;
        public bool GroupMbAttackPlayObject;
        public int PosionDecHealthTime;
        /// <summary>
        /// 中红毒着持久及减防量（实际大小为 12 / 10）
        /// </summary>
        public int PosionDamagarmor;
        public bool LimitSwordLong;
        public int SwordLongPowerRate;
        public int FireBoomRage;
        public int SnowWindRange;
        public int ElecBlizzardRange;
        /// <summary>
        /// 圣言怪物等级限制
        /// </summary>
        public int MagTurnUndeadLevel;
        /// <summary>
        /// 诱惑之光怪物等级限制
        /// </summary>        
        public int MagTammingLevel;
        /// <summary>
        /// 诱惑怪物相差等级机率，此数字越小机率越大；
        /// </summary>        
        public int MagTammingTargetLevel;
        /// <summary>
        /// 成功机率=怪物最高HP 除以 此倍率，此倍率越大诱惑机率越高
        /// </summary>        
        public int MagTammingHPRate;
        public int MagTammingCount;
        public int MabMabeHitRandRate;
        public int MabMabeHitMinLvLimit;
        public int MabMabeHitSucessRate;
        public int MabMabeHitMabeTimeRate;
        public string CastleName;
        public string CastleHomeMap;
        public int CastleHomeX;
        public int CastleHomeY;
        public int CastleWarRangeX;
        public int CastleWarRangeY;
        public int CastleTaxRate;
        public bool GetAllNpcTax;
        public int HireGuardPrice;
        public int HireArcherPrice;
        public int CastleGoldMax;
        public int CastleOneDayGold;
        public int RepairDoorPrice;
        public int RepairWallPrice;
        public int CastleMemberPriceRate;
        public int MaxHitMsgCount;
        public int MaxSpellMsgCount;
        public int MaxRunMsgCount;
        public int MaxWalkMsgCount;
        public int MaxTurnMsgCount;
        public int MaxSitDonwMsgCount;
        public int MaxDigUpMsgCount;
        public bool SpellSendUpdateMsg;
        public bool ActionSendActionMsg;
        public bool KickOverSpeed;
        public int SpeedControlMode;
        public int OverSpeedKickCount;
        public int DropOverSpeed;
        /// <summary>
        /// 攻击间隔(ms)
        /// </summary>
        public int HitIntervalTime;
        /// <summary>
        /// 魔法间隔(ms)
        /// </summary>        
        public int MagicHitIntervalTime;
        /// <summary>
        /// 跑步间隔(ms)
        /// </summary>        
        public int RunIntervalTime;
        /// <summary>
        /// 走路间隔(ms)
        /// </summary>        
        public int WalkIntervalTime;
        /// <summary>
        /// 换方向间隔(ms)
        /// </summary>        
        public int TurnIntervalTime;
        public bool boControlActionInterval;
        public bool boControlWalkHit;
        public bool boControlRunLongHit;
        public bool boControlRunHit;
        public bool boControlRunMagic;
        /// <summary>
        /// 组合操作间隔
        /// </summary>
        public int ActionIntervalTime;
        /// <summary>
        /// 跑位刺杀间隔
        /// </summary>        
        public int RunLongHitIntervalTime;
        /// <summary>
        /// 跑位攻击间隔
        /// </summary>        
        public int RunHitIntervalTime;
        /// <summary>
        /// 走位攻击间隔
        /// </summary>        
        public int WalkHitIntervalTime;
        /// <summary>
        /// 跑位魔法间隔
        /// </summary>        
        public int RunMagicIntervalTime;
        /// <summary>
        /// 不显示人物弯腰动作
        /// </summary>        
        public bool DisableStruck;
        /// <summary>
        /// 自己不显示人物弯腰动作
        /// </summary>        
        public bool DisableSelfStruck;
        /// <summary>
        /// 人物弯腰停留时间
        /// </summary>        
        public int StruckTime;
        /// <summary>
        /// 杀怪经验倍数
        /// </summary>        
        public int KillMonExpMultiple;
        public int RequestVersion;
        public bool HighLevelKillMonFixExp;
        public bool HighLevelGroupFixExp;
        public bool MonDelHptoExp;
        public int MonHptoExpLevel;
        public int MonHptoExpmax;
        public bool UseFixExp;
        public int BaseExp;
        public int AddExp;
        public int LimitExpLevel;
        public int LimitExpValue;
        public bool AddUserItemNewValue;
        public string LineNoticePreFix;
        public string SysMsgPreFix;
        public string GuildMsgPreFix;
        public string GroupMsgPreFix;
        public string HintMsgPreFix;
        public string GameManagerRedMsgPreFix;
        public string MonSayMsgPreFix;
        public string CustMsgPreFix;
        public string CastleMsgPreFix;
        public string GuildNotice;
        public string GuildWar;
        public string GuildAll;
        public string GuildMember;
        public string GuildMemberRank;
        public string GuildChief;
        public bool boKickAllUser;
        public bool TestSpeedMode;
        /// <summary>
        /// 气血石
        /// </summary>
        public byte HPStoneStartRate;
        /// <summary>
        /// 魔血石
        /// </summary>        
        public byte MPStoneStartRate;
        /// <summary>
        /// 气血石
        /// </summary>        
        public int HPStoneIntervalTime;
        /// <summary>
        /// 魔血石
        /// </summary>        
        public int MpStoneIntervalTime;
        /// <summary>
        /// 气血石
        /// </summary>        
        public byte HPStoneAddRate;
        /// <summary>
        /// 魔血石
        /// </summary>        
        public byte MPStoneAddRate;
        /// <summary>
        /// 气血石
        /// </summary>        
        public int HPStoneDecDura;
        /// <summary>
        /// 魔血石
        /// </summary>        
        public int MPStoneDecDura;
        public ClientConf ClientConf;
        public int WeaponMakeUnLuckRate;
        public int WeaponMakeLuckPoint1;
        public int WeaponMakeLuckPoint2;
        public int WeaponMakeLuckPoint3;
        public int WeaponMakeLuckPoint2Rate;
        public int WeaponMakeLuckPoint3Rate;
        public bool CheckUserItemPlace;
        public int nClientKey;
        public int nLevelValueOfTaosHP;
        public double nLevelValueOfTaosHPRate;
        public int nLevelValueOfTaosMP;
        public int nLevelValueOfWizardHP;
        public double nLevelValueOfWizardHPRate;
        public int nLevelValueOfWarrHP;
        public double nLevelValueOfWarrHPRate;
        /// <summary>
        /// 怪物处理线程数量
        /// 建议高于5w以上怪物启用2个线程，虽然可以提高处理速度，但不是越多越好,默认值=2。
        /// 如未设置该值或者填写0则该值默认为当前CPU核心数*2，建议自己手动设置该值。
        /// 在单怪物5w数量左右还是存在一定的行动延缓,预计要限制单怪物在刷怪线程列表的最大数量，超过刷怪限制则进入新刷怪线程
        /// todo 假如使用该方法需要预留线程4个.超过预留线程则不会在刷新新怪物.有怪物死亡减少则继续刷新，持续进入该循环
        /// </summary>
        public int ProcessMonsterMultiThreadLimit;
        /// <summary>
        /// 怪物线程预留线程数
        /// </summary>
        public int ProcessMonsterRetainThreadLimit;
        /// <summary>
        /// 当前怪物处理线程最大上限值.
        /// 默认值=40000
        /// 40000值为比较合理和保守的设定，单线程下40000值能够稳定运行
        /// </summary>
        public int MonsterThreadLimit;
        /// <summary>
        /// 怪物处理间隔
        /// 空闲时处理怪物检测次数，数字越大，怪物行动越迟钝,但节约服务器资源,默认值=2。
        /// </summary>
        public int ProcessMonsterInterval;
        public bool boCheckFail;
        public int nAppIconCrc;
        public bool boIDSocketConnected;
        public object UserIDSection;
        public string sIDSocketRecvText;
        public int IDSocket;
        public int nIDSocketRecvIncLen;
        public int nIDSocketRecvMaxLen;
        public int nIDSocketRecvCount;
        public int nIDReceiveMaxTime;
        public int nIDSocketWSAErrCode;
        public int nIDSocketErrorCount;
        public int nLoadDBCount;
        public int nLoadDBErrorCount;
        public int nSaveDBCount;
        public int nDBQueryID;
        public bool boDBSocketConnected;
        public int nDBSocketRecvIncLen;
        public int nDBSocketRecvMaxLen;
        public int nDBSocketRecvCount;
        public int nDBReceiveMaxTime;
        public int nDBSocketWSAErrCode;
        public int nDBSocketErrorCount;
        public int nServerFile_CRCB;
        public int nClientFile1_CRC;
        public int nClientFile2_CRC;
        public int nClientFile3_CRC;
        /// <summary>
        /// 不可保存的变量 I
        /// </summary>
        public int[] GlobaDyMval;
        /// <summary>
        /// 变量可保存 G
        /// </summary>
        public int[] GlobalVal;
        /// <summary>
        /// 变量可保存 A
        /// </summary>
        public string[] GlobalAVal;
        public int nM2Crc;
        public int nCheckLicenseFail;
        public int dwCheckTick;
        public int nCheckFail;
        public bool boDropGoldToPlayBag;
        public int dwSendWhisperTime;
        public int nSendWhisperPlayCount;
        public bool boMoveCanDupObj;
        public int nProcessTick;
        public int nProcessTime;
        public int nDBSocketSendLen;
        public bool PermissionSystem;
        /// <summary>
        /// 机器人自动拾取物品
        /// </summary>
        public bool RobotAutoPickUpItem;
        /// <summary>
        /// 道22后是否物理攻击
        /// </summary>
        public bool boHeroAttackTao;
        /// <summary>
        /// 道法22前是否物理攻击
        /// </summary>
        public bool boHeroAttackTarget;
        /// <summary>
        /// 安全区不受控制
        /// </summary>
        public bool boSafeAreaLimited;
        /// <summary>
        /// 机器人运行间隔时间
        /// </summary>
        public long nAIRunIntervalTime;
        /// <summary>
        /// 机器人走路间隔时间
        /// </summary>
        public long nAIWalkIntervalTime;
        /// <summary>
        /// 机器人血量低于多少开始回血（百分比）
        /// </summary>
        public int nRenewPercent;
        public string sAIHomeMap;
        public short nAIHomeX;
        public short nAIHomeY;
        public bool boHPAutoMoveMap;//低血回城
        public bool boAutoRepairItem;
        public bool boRenewHealth;
        public long nAIWarrorAttackTime;
        public long nAIWizardAttackTime;
        public long nAITaoistAttackTime;
        /// <summary>
        /// 不管目标血值,全部可以使用施毒术否则目标血值达700时使用
        /// </summary>
        public bool btHeroSkillMode;
        public long dwHeroWarrorAttackTime;//战士英雄的攻击速度
        public long dwHeroWizardAttackTime;//法师英雄的攻击速度
        public long dwHeroTaoistAttackTime;//道士英雄的攻击速度
        public string sAIConfigListFileName;
        public string sHeroAIConfigListFileName;
        /// <summary>
        /// 寄售系统每次扣多少金币(默认10000金币)
        /// </summary>
        public int DecUserGameGold;
        /// <summary>
        /// 关闭游戏引擎的加速控制
        /// </summary>
        public bool CloseSpeedHackCheck;
        /// <summary>
        /// NPC名字颜色控制(0-255)
        /// </summary>
        public byte NpcNameColor;

        public GameSvrConf()
        {
            ServerName = "SKY引擎";
            ServerIPaddr = "127.0.0.1";
            sWebSite = "http=//www.jsym2.com";
            sBbsSite = "http=//bbs.jsym2.com";
            sClientDownload = "http=//www.jsym2.com";
            sQQ = "88888888";
            sPhone = "123456789";
            sBankAccount0 = "银行信息";
            sBankAccount1 = "银行信息";
            sBankAccount2 = "银行信息";
            sBankAccount3 = "银行信息";
            sBankAccount4 = "银行信息";
            sBankAccount5 = "银行信息";
            sBankAccount6 = "银行信息";
            sBankAccount7 = "银行信息";
            sBankAccount8 = "银行信息";
            sBankAccount9 = "银行信息";
            nServerNumber = 0;
            VentureServer = false;
            TestServer = true;
            ServiceMode = false;
            PveServer = false;
            TestLevel = 1;
            TestGold = 0;
            TestUserLimit = 1000;
            SendBlock = 1024;
            CheckBlock = 4069;
            AvailableBlock = 8000;
            GateLoad = 0;
            UserFull = 1000;
            ZenFastStep = 300;
            sGateAddr = "127.0.0.1";
            nGatePort = 5000;
            sDBAddr = "127.0.0.1";
            nDBPort = 6000;
            sIDSAddr = "127.0.0.1";
            nIDSPort = 5600;
            MsgSrvAddr = "127.0.0.1";
            MsgSrvPort = 4900;
            LogServerAddr = "127.0.0.1";
            LogServerPort = 10000;
            DiscountForNightTime = false;
            HalfFeeStart = 2;
            HalfFeeEnd = 10;
            ViewHackMessage = false;
            ViewAdmissionFailure = false;
            BaseDir = ".\\Share\\";
            GuildDir = ".\\GuildBase\\Guilds\\";
            GuildFile = ".\\GuildBase\\GuildList.txt";
            VentureDir = ".\\ShareV\\";
            ConLogDir = ".\\ConLog\\";
            CastleDir = ".\\Envir\\Castle\\";
            CastleFile = ".\\Envir\\Castle\\List.txt";
            EnvirDir = ".\\Envir\\";
            MapDir = ".\\Map\\";
            NoticeDir = ".\\Notice\\";
            LogDir = ".\\Log\\";
            ClientFile1 = "mir.1";
            ClientFile2 = "mir.Dat";
            ClientFile3 = "mir.3";
            ClothsMan = "布衣(男)";
            ClothsWoman = "布衣(女)";
            WoodenSword = "木剑";
            Candle = "蜡烛";
            BasicDrug = "金创药(小量)";
            GoldStone = "金矿";
            SilverStone = "银矿";
            SteelStone = "铁矿";
            CopperStone = "铜矿";
            BlackStone = "黑铁矿";
            GemStone1 = "金刚石矿";
            GemStone2 = "绿宝石矿";
            GemStone3 = "红宝石矿";
            GemStone4 = "白宝石矿";
            Zuma = new string[] { "祖玛卫士", "祖玛雕像", "祖玛弓箭手", "楔蛾" };
            Bee = "蝙蝠";
            Spider = "爆裂蜘蛛";
            WomaHorn = "沃玛号角";
            ZumaPiece = "祖玛头像";
            GameGoldName = "元宝";
            GamePointName = "游戏点";
            PayMentPointName = "荣誉值";
            DBSocket = 0;
            HealthFillTime = 300;
            SpellFillTime = 800;
            MonUpLvNeedKillBase = 100;
            MonUpLvRate = 16;
            MonUpLvNeedKillCount = new int[] { 0, 0, 50, 100, 200, 300, 600, 1200, 2400, 4800, 9600, 19200, 38400, 76800, 153600 };
            SlaveColor = new byte[] { 0xFF, 0xFE, 0x93, 0x9A, 0xE5, 0xA8, 0xB4, 0xFC, 249, 250, 250, 250, 250, 250, 250 };
            NeedExps = new int[Grobal2.MaxChangeLevel];
            WideAttack = new byte[] { 7, 1, 2 };
            CrsAttack = new byte[] { 7, 1, 2, 3, 4, 5, 6 };
            SpitMap = new byte[,,]{
                {{0, 0, 1, 0, 0}, //DR_UP
                {0, 0, 1, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0}},
                {{0, 0, 0, 0, 1}, //DR_UPRIGHT
                {0, 0, 0, 1, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0}},
                {{0, 0, 0, 0, 0}, //DR_RIGHT
                {0, 0, 0, 0, 0},
                {0, 0, 0, 1, 1},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0}},
                {{0, 0, 0, 0, 0}, //DR_DOWNRIGHT
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 1, 0},
                {0, 0, 0, 0, 1}},
                {{0, 0, 0, 0, 0}, //DR_DOWN
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 1, 0, 0},
                {0, 0, 1, 0, 0}},
                {{0, 0, 0, 0, 0}, //DR_DOWNLEFT
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 1, 0, 0, 0},
                {1, 0, 0, 0, 0}},
                {{0, 0, 0, 0, 0}, //DR_LEFT
                {0, 0, 0, 0, 0},
                {1, 1, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0}},
                {{1, 0, 0, 0, 0}, //DR_UPLEFT
                {0, 1, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0}}
                };
            HomeMap = "0";
            HomeX = 289;
            HomeY = 618;
            RedHomeMap = "3";
            RedHomeX = 845;
            RedHomeY = 674;
            RedDieHomeMap = "3";
            RedDieHomeX = 839;
            RedDieHomeY = 668;
            JobHomePoint = false;
            WarriorHomeMap = "0";
            WarriorHomeX = 289;
            WarriorHomeY = 618;
            WizardHomeMap = "0";
            WizardHomeX = 650;
            WizardHomeY = 631;
            TaoistHomeMap = "0";
            TaoistHomeX = 334;
            TaoistHomeY = 266;
            DecPkPointTime = 2 * 60 * 1000;
            DecPkPointCount = 1;
            dwPKFlagTime = 60 * 1000;
            KillHumanAddPKPoint = 100;
            KillHumanDecLuckPoint = 500;
            DecLightItemDrugTime = 500;
            SafeZoneSize = 10;
            StartPointSize = 2;
            HumanGetMsgTime = 200;
            GroupMembersMax = 10;
            WarrMon = 10;
            WizardMon = 10;
            TaosMon = 10;
            MonHum = 10;
            FireBallSkill = "火球术";
            HealSkill = "治愈术";
            ReNewNameColor = new byte[] { 0xFF, 0xFE, 0x93, 0x9A, 0xE5, 0xA8, 0xB4, 0xFC, 0xB4, 0xFC };
            ReNewNameColorTime = 2000;
            ReNewChangeColor = true;
            ReNewLevelClearExp = true;
            BonusAbilofWarr = new NakedAbility { DC = 17, MC = 20, SC = 20, AC = 20, MAC = 20, HP = 1, MP = 3, Hit = 20, Speed = 35, Reserved = 0 };
            BonusAbilofWizard = new NakedAbility { DC = 17, MC = 25, SC = 30, AC = 20, MAC = 15, HP = 2, MP = 1, Hit = 25, Speed = 35, Reserved = 0 };
            BonusAbilofTaos = new NakedAbility { DC = 20, MC = 30, SC = 17, AC = 20, MAC = 15, HP = 2, MP = 1, Hit = 30, Speed = 30, Reserved = 0 };
            NakedAbilofWarr = new NakedAbility { DC = 512, MC = 2560, SC = 20, AC = 768, MAC = 1280, HP = 0, MP = 0, Hit = 0, Speed = 0, Reserved = 0 };
            NakedAbilofWizard = new NakedAbility { DC = 512, MC = 512, SC = 2560, AC = 1280, MAC = 768, HP = 0, MP = 0, Hit = 5, Speed = 0, Reserved = 0 };
            NakedAbilofTaos = new NakedAbility { DC = 20, MC = 30, SC = 17, AC = 20, MAC = 15, HP = 2, MP = 1, Hit = 30, Speed = 30, Reserved = 0 };
            UpgradeWeaponMaxPoint = 20;
            UpgradeWeaponPrice = 10000;
            UPgradeWeaponGetBackTime = 60 * 60 * 1000;
            ClearExpireUpgradeWeaponDays = 8;
            UpgradeWeaponDCRate = 100;
            UpgradeWeaponDCTwoPointRate = 30;
            UpgradeWeaponDCThreePointRate = 200;
            UpgradeWeaponSCRate = 100;
            UpgradeWeaponSCTwoPointRate = 30;
            UpgradeWeaponSCThreePointRate = 200;
            UpgradeWeaponMCRate = 100;
            UpgradeWeaponMCTwoPointRate = 30;
            UpgradeWeaponMCThreePointRate = 200;
            ProcessMonstersTime = 10;
            RegenMonstersTime = 200;
            MonGenRate = 10;
            ProcessMonRandRate = 5;
            ProcessMonLimitCount = 5;
            SoftVersionDate = 20020522;
            CanOldClientLogon = true;
            ConsoleShowUserCountTime = 10 * 60 * 1000;
            ShowLineNoticeTime = 5 * 60 * 1000;
            LineNoticeColor = 2;
            StartCastleWarDays = 4;
            StartCastlewarTime = 20;
            ShowCastleWarEndMsgTime = 10 * 60 * 1000;
            CastleWarTime = 3 * 60 * 60 * 1000;
            GetCastleTime = 10 * 60 * 1000;
            GuildWarTime = 3 * 60 * 60 * 1000;
            BuildGuildPrice = 1000000;
            GuildWarPrice = 30000;
            MakeDurgPrice = 100;
            HumanMaxGold = 10000000;
            HumanTryModeMaxGold = 100000;
            TryModeLevel = 7;
            TryModeUseStorage = false;
            CanShoutMsgLevel = 7;
            ShowMakeItemMsg = false;
            ShutRedMsgShowGMName = false;
            SayMsgMaxLen = 80;
            SayMsgTime = 3 * 1000;
            SayMsgCount = 2;
            DisableSayMsgTime = 60 * 1000;
            SayRedMsgMaxLen = 255;
            ShowGuildName = true;
            ShowRankLevelName = false;
            MonSayMsg = false;
            StartPermission = 0;
            IsKillHumanWinLevel = false;
            IsKilledLostLevel = false;
            IsKillHumanWinExp = false;
            IsKilledLostExp = false;
            KillHumanWinLevel = 1;
            KilledLostLevel = 1;
            KillHumanWinExp = 100000;
            KillHumanLostExp = 100000;
            HumanLevelDiffer = 10;
            MonsterPowerRate = 10;
            ItemsPowerRate = 10;
            ItemsACPowerRate = 10;
            SendOnlineCount = true;
            SendOnlineCountRate = 10;
            ClickNpcTime = 1000;  //NPC点击间隔
            QueryBagItemsTick = 2 * 60 * 1000;  //包裹刷新间隔
            SendOnlineTime = 5 * 60 * 1000;
            SaveHumanRcdTime = 10 * 60 * 1000;
            HumanFreeDelayTime = 5 * 60 * 1000;
            MakeGhostTime = 3 * 60 * 1000;
            ClearDropOnFloorItemTime = 60 * 60 * 1000;
            FloorItemCanPickUpTime = 2 * 60 * 1000;
            PasswordLockSystem = false;  //是否启用密码保护系统
            LockDealAction = false;  //是否锁定交易操作
            LockDropAction = false;  //是否锁定扔物品操作
            LockGetBackItemAction = false;  //是否锁定取仓库操作
            LockHumanLogin = false;  //是否锁定走操作
            LockWalkAction = false;  //是否锁定走操作
            LockRunAction = false;  //是否锁定跑操作
            LockHitAction = false;  //是否锁定攻击操作
            LockSpellAction = false;  //是否锁定魔法操作
            LockSendMsgAction = false;  //是否锁定发信息操作
            LockUserItemAction = false;  //是否锁定使用物品操作
            LockInObModeAction = false;  //锁定时进入隐身状态
            PasswordErrorCountLock = 3; //输入密码错误超过 指定次数则锁定密码
            PasswordErrorKick = false; //输入密码错误超过限制则踢下线
            SendRefMsgRange = 12;
            DecLampDura = true;
            HungerSystem = false;
            HungerDecHP = false;
            HungerDecPower = false;
            DiableHumanRun = false;
            boRunHuman = false;
            boRunMon = false;
            boRunNpc = false;
            boRunGuard = false;
            boWarDisHumRun = false;
            boGMRunAll = true;
            TryDealTime = 3000;
            DealOKTime = 1000;
            CanNotGetBackDeal = true;
            DisableDeal = false;
            MasterOKLevel = 500;
            MasterOKCreditPoint = 0;
            nMasterOKBonusPoint = 0;
            boPKLevelProtect = false;
            nPKProtectLevel = 10;
            nRedPKProtectLevel = 10;
            ItemPowerRate = 10000;
            ItemExpRate = 10000;
            ScriptGotoCountLimit = 30;
            btHearMsgFColor = 0x00; //前景
            btHearMsgBColor = 0xFF; //背景
            btWhisperMsgFColor = 0xFC; //前景
            btWhisperMsgBColor = 0xFF; //背景
            btGMWhisperMsgFColor = 0xFF; //前景
            btGMWhisperMsgBColor = 0x38; //背景
            CryMsgFColor = 0x0; //前景
            CryMsgBColor = 0x97; //背景
            GreenMsgFColor = 0xDB; //前景
            GreenMsgBColor = 0xFF; //背景
            BlueMsgFColor = 0xFF; //前景
            BlueMsgBColor = 0xFC; //背景
            RedMsgFColor = 0xFF; //前景
            RedMsgBColor = 0x38; //背景
            GuildMsgFColor = 0xDB; //前景
            GuildMsgBColor = 0xFF; //背景
            GroupMsgFColor = 0xC4; //前景
            GroupMsgBColor = 0xFF; //背景
            CustMsgFColor = 0xFC; //前景
            CustMsgBColor = 0xFF; //背景
            PurpleMsgFColor = 0xFF;
            PurpleMsgBColor = 253;
            MonRandomAddValue = 10;
            MakeRandomAddValue = 10;
            WeaponDCAddValueMaxLimit = 12;
            WeaponDCAddValueRate = 15;
            WeaponMCAddValueMaxLimit = 12;
            WeaponMCAddValueRate = 15;
            WeaponSCAddValueMaxLimit = 12;
            WeaponSCAddValueRate = 15;
            DressDCAddRate = 40;
            DressDCAddValueMaxLimit = 6;
            DressDCAddValueRate = 20;
            DressMCAddRate = 40;
            DressMCAddValueMaxLimit = 6;
            DressMCAddValueRate = 20;
            DressSCAddRate = 40;
            DressSCAddValueMaxLimit = 6;
            nDressSCAddValueRate = 20;
            NeckLace202124DCAddRate = 40;
            NeckLace202124DCAddValueMaxLimit = 6;
            NeckLace202124DCAddValueRate = 20;
            NeckLace202124MCAddRate = 40;
            NeckLace202124MCAddValueMaxLimit = 6;
            NeckLace202124MCAddValueRate = 20;
            NeckLace202124SCAddRate = 40;
            NeckLace202124SCAddValueMaxLimit = 6;
            NeckLace202124SCAddValueRate = 20;
            NeckLace19DCAddRate = 30;
            NeckLace19DCAddValueMaxLimit = 6;
            NeckLace19DCAddValueRate = 20;
            NeckLace19MCAddRate = 30;
            NeckLace19MCAddValueMaxLimit = 6;
            NeckLace19MCAddValueRate = 20;
            NeckLace19SCAddRate = 30;
            NeckLace19SCAddValueMaxLimit = 6;
            NeckLace19SCAddValueRate = 20;
            ArmRing26DCAddRate = 30;
            ArmRing26DCAddValueMaxLimit = 6;
            ArmRing26DCAddValueRate = 20;
            ArmRing26MCAddRate = 30;
            ArmRing26MCAddValueMaxLimit = 6;
            ArmRing26MCAddValueRate = 20;
            ArmRing26SCAddRate = 30;
            ArmRing26SCAddValueMaxLimit = 6;
            ArmRing26SCAddValueRate = 20;
            Ring22DCAddRate = 30;
            Ring22DCAddValueMaxLimit = 6;
            Ring22DCAddValueRate = 20;
            Ring22MCAddRate = 30;
            Ring22MCAddValueMaxLimit = 6;
            Ring22MCAddValueRate = 20;
            Ring22SCAddRate = 30;
            Ring22SCAddValueMaxLimit = 6;
            Ring22SCAddValueRate = 20;
            Ring23DCAddRate = 30;
            Ring23DCAddValueMaxLimit = 6;
            Ring23DCAddValueRate = 20;
            Ring23MCAddRate = 30;
            Ring23MCAddValueMaxLimit = 6;
            Ring23MCAddValueRate = 20;
            Ring23SCAddRate = 30;
            Ring23SCAddValueMaxLimit = 6;
            Ring23SCAddValueRate = 20;
            HelMetDCAddRate = 30;
            HelMetDCAddValueMaxLimit = 6;
            HelMetDCAddValueRate = 20;
            HelMetMCAddRate = 30;
            HelMetMCAddValueMaxLimit = 6;
            HelMetMCAddValueRate = 20;
            HelMetSCAddRate = 30;
            HelMetSCAddValueMaxLimit = 6;
            HelMetSCAddValueRate = 20;
            UnknowHelMetACAddRate = 20;
            UnknowHelMetACAddValueMaxLimit = 4;
            UnknowHelMetMACAddRate = 20;
            UnknowHelMetMACAddValueMaxLimit = 4;
            UnknowHelMetDCAddRate = 30;
            UnknowHelMetDCAddValueMaxLimit = 3;
            UnknowHelMetMCAddRate = 30;
            UnknowHelMetMCAddValueMaxLimit = 3;
            UnknowHelMetSCAddRate = 30;
            UnknowHelMetSCAddValueMaxLimit = 3;
            UnknowRingACAddRate = 20;
            UnknowRingACAddValueMaxLimit = 4;
            UnknowRingMACAddRate = 20;
            UnknowRingMACAddValueMaxLimit = 4;
            UnknowRingDCAddRate = 20;
            UnknowRingDCAddValueMaxLimit = 6;
            UnknowRingMCAddRate = 20;
            UnknowRingMCAddValueMaxLimit = 6;
            UnknowRingSCAddRate = 20;
            UnknowRingSCAddValueMaxLimit = 6;
            UnknowNecklaceACAddRate = 20;
            UnknowNecklaceACAddValueMaxLimit = 5;
            UnknowNecklaceMACAddRate = 20;
            UnknowNecklaceMACAddValueMaxLimit = 5;
            UnknowNecklaceDCAddRate = 30;
            UnknowNecklaceDCAddValueMaxLimit = 5;
            UnknowNecklaceMCAddRate = 30;
            UnknowNecklaceMCAddValueMaxLimit = 5;
            UnknowNecklaceSCAddRate = 30;
            UnknowNecklaceSCAddValueMaxLimit = 5;
            MonOneDropGoldCount = 2000;
            SendCurTickCount = true;  //客户端时间
            MakeMineHitRate = 4; //挖矿命中率
            MakeMineRate = 12; //挖矿率
            StoneTypeRate = 120;
            StoneTypeRateMin = 56;
            GoldStoneMin = 1;
            GoldStoneMax = 2;
            SilverStoneMin = 3;
            SilverStoneMax = 20;
            SteelStoneMin = 21;
            SteelStoneMax = 45;
            BlackStoneMin = 46;
            BlackStoneMax = 56;
            StoneMinDura = 3000;
            StoneGeneralDuraRate = 13000;
            StoneAddDuraRate = 20;
            StoneAddDuraMax = 10000;
            WinLottery6Min = 1;
            WinLottery6Max = 4999;
            WinLottery5Min = 14000;
            WinLottery5Max = 15999;
            WinLottery4Min = 16000;
            WinLottery4Max = 16149;
            WinLottery3Min = 16150;
            WinLottery3Max = 16169;
            WinLottery2Min = 16170;
            WinLottery2Max = 16179;
            WinLottery1Min = 16180;
            WinLottery1Max = 16185;//16180 + 1820;
            WinLottery1Gold = 1000000;
            WinLottery2Gold = 200000;
            WinLottery3Gold = 100000;
            WinLottery4Gold = 10000;
            WinLottery5Gold = 1000;
            WinLottery6Gold = 500;
            WinLotteryRate = 30000;
            WinLotteryCount = 0;
            NoWinLotteryCount = 0;
            WinLotteryLevel1 = 0;
            WinLotteryLevel2 = 0;
            WinLotteryLevel3 = 0;
            WinLotteryLevel4 = 0;
            WinLotteryLevel5 = 0;
            WinLotteryLevel6 = 0;
            GlobalVal = new int[500];
            GlobaDyMval = new int[500];
            GlobalAVal = new string[500];
            ItemNumber = 0;
            ItemNumberEx = int.MaxValue / 2;
            GuildRecallTime = 180;
            GroupRecallTime = 180;
            ControlDropItem = false;
            InSafeDisableDrop = false;
            CanDropGold = 1000;
            CanDropPrice = 500;
            SendCustemMsg = true;
            SubkMasterSendMsg = true;
            SuperRepairPriceRate = 3; //特修价格倍数
            RepairItemDecDura = 30; //普通修理掉持久数(特持久上限减下限再除以此数为减的数值)
            DieScatterBag = true;
            DieScatterBagRate = 3;
            DieRedScatterBagAll = true;
            DieDropUseItemRate = 30;
            DieRedDropUseItemRate = 15;
            DieDropGold = false;
            KillByHumanDropUseItem = false;
            KillByMonstDropUseItem = true;
            KickExpireHuman = false;
            GuildRankNameLen = 16;
            GuildMemberMaxLimit = 200;
            GuildNameLen = 16;
            AttackPosionRate = 5;
            AttackPosionTime = 5;
            RevivalTime = 60 * 1000; //复活间隔时间
            boUserMoveCanDupObj = false;
            boUserMoveCanOnItem = true;
            dwUserMoveTime = 10;
            dwPKDieLostExpRate = 1000;
            nPKDieLostLevelRate = 20000;
            btPKFlagNameColor = 0x2F;
            btPKLevel1NameColor = 0xFB;
            btPKLevel2NameColor = 0xF9;
            btAllyAndGuildNameColor = 0xB4;
            WarGuildNameColor = 0x45;
            InFreePKAreaNameColor = 0xDD;
            SpiritMutiny = false;
            SpiritMutinyTime = 30 * 60 * 1000;
            SpiritPowerRate = 2;
            MasterDieMutiny = false;
            MasterDieMutinyRate = 5;
            MasterDieMutinyPower = 10;
            MasterDieMutinySpeed = 5;
            BBMonAutoChangeColor = false;
            BBMonAutoChangeColorTime = 3000;
            boOldClientShowHiLevel = true;
            ShowScriptActionMsg = true;
            RunSocketDieLoopLimit = 100;
            ThreadRun = false;
            ShowExceptionMsg = false;
            ShowPreFixMsg = false;
            MagicAttackRage = 8; //魔法锁定范围
            nBoBoall = true;
            DropItemRage = 3; //爆物范围
            Skeleton = "变异骷髅";
            SkeletonCount = 1;
            SkeletonArray = new TRecallMigic[10];
            Dragon = "神兽";
            Dragon1 = "神兽1";
            DragonCount = 1;
            DragonArray = new TRecallMigic[10];
            Angel = "精灵";
            AmyOunsulPoint = 10;
            DisableInSafeZoneFireCross = false;
            GroupMbAttackPlayObject = false;
            PosionDecHealthTime = 2500;
            PosionDamagarmor = 12;//中红毒着持久及减防量（实际大小为 12 / 10）
            LimitSwordLong = false;
            SwordLongPowerRate = 100;
            FireBoomRage = 1;
            SnowWindRange = 1;
            ElecBlizzardRange = 2;
            MagTurnUndeadLevel = 50; //圣言怪物等级限制
            MagTammingLevel = 50; //诱惑之光怪物等级限制
            MagTammingTargetLevel = 10; //诱惑怪物相差等级机率，此数字越小机率越大；
            MagTammingHPRate = 100; //成功机率=怪物最高HP 除以 此倍率，此倍率越大诱惑机率越高
            MagTammingCount = 5;
            MabMabeHitRandRate = 100;
            MabMabeHitMinLvLimit = 10;
            MabMabeHitSucessRate = 21;
            MabMabeHitMabeTimeRate = 20;
            CastleName = "沙巴克";
            CastleHomeMap = "3";
            CastleHomeX = 644;
            CastleHomeY = 290;
            CastleWarRangeX = 100;
            CastleWarRangeY = 100;
            CastleTaxRate = 5;
            GetAllNpcTax = false;
            HireGuardPrice = 300000;
            HireArcherPrice = 300000;
            CastleGoldMax = 10000000;
            CastleOneDayGold = 2000000;
            RepairDoorPrice = 2000000;
            RepairWallPrice = 500000;
            CastleMemberPriceRate = 80;
            MaxHitMsgCount = 1;
            MaxSpellMsgCount = 1;
            MaxRunMsgCount = 1;
            MaxWalkMsgCount = 1;
            MaxTurnMsgCount = 1;
            MaxSitDonwMsgCount = 1;
            MaxDigUpMsgCount = 1;
            SpellSendUpdateMsg = true;
            ActionSendActionMsg = true;
            KickOverSpeed = false;
            SpeedControlMode = 0;
            OverSpeedKickCount = 4;
            DropOverSpeed = 10;
            HitIntervalTime = 900; //攻击间隔
            MagicHitIntervalTime = 800; //魔法间隔
            RunIntervalTime = 600; //跑步间隔
            WalkIntervalTime = 600; //走路间隔
            TurnIntervalTime = 600; //换方向间隔
            boControlActionInterval = true;
            boControlWalkHit = true;
            boControlRunLongHit = true;
            boControlRunHit = true;
            boControlRunMagic = true;
            ActionIntervalTime = 350; //组合操作间隔
            RunLongHitIntervalTime = 800; //跑位刺杀间隔
            RunHitIntervalTime = 800; //跑位攻击间隔
            WalkHitIntervalTime = 800; //走位攻击间隔
            RunMagicIntervalTime = 900; //跑位魔法间隔
            DisableStruck = false; //不显示人物弯腰动作
            DisableSelfStruck = false; //自己不显示人物弯腰动作
            StruckTime = 100; //人物弯腰停留时间
            KillMonExpMultiple = 1; //杀怪经验倍数
            RequestVersion = 98;
            HighLevelKillMonFixExp = false;
            HighLevelGroupFixExp = true;
            MonDelHptoExp = false;
            MonHptoExpLevel = 100;
            MonHptoExpmax = 1;
            UseFixExp = true;
            BaseExp = 100000000;
            AddExp = 1000000;
            LimitExpLevel = 1000;
            LimitExpValue = 1;
            AddUserItemNewValue = true;
            LineNoticePreFix = "〖公告〗";
            SysMsgPreFix = "〖系统〗";
            GuildMsgPreFix = "〖行会〗";
            GroupMsgPreFix = "〖组队〗";
            HintMsgPreFix = "〖提示〗";
            GameManagerRedMsgPreFix = "〖ＧＭ〗";
            MonSayMsgPreFix = "〖怪物〗";
            CustMsgPreFix = "〖祝福〗";
            CastleMsgPreFix = "〖城主〗";
            GuildNotice = "公告";
            GuildWar = "敌对行会";
            GuildAll = "联盟行会";
            GuildMember = "行会成员";
            GuildMemberRank = "行会成员";
            GuildChief = "掌门人";
            boKickAllUser = false;
            TestSpeedMode = false;
            HPStoneStartRate = 80; //气血石
            MPStoneStartRate = 80; //魔血石
            HPStoneIntervalTime = 1000; //气血石
            MpStoneIntervalTime = 1000; //魔血石
            HPStoneAddRate = 10; //气血石
            MPStoneAddRate = 10; //魔血石
            HPStoneDecDura = 1000; //气血石
            MPStoneDecDura = 1000; //魔血石
            ClientConf = new ClientConf()
            {
                boGameAssist = true,
                boWhisperRecord = true,
                boMaketSystem = true,
                boNoFog = false,
                boStallSystem = true,
                boShowHpBar = true,
                boShowHpNumber = true,
                boNoStruck = false,
                boFastMove = false,
                boShowGameShop = true
            };
            WeaponMakeUnLuckRate = 20;
            WeaponMakeLuckPoint1 = 1;
            WeaponMakeLuckPoint2 = 3;
            WeaponMakeLuckPoint3 = 7;
            WeaponMakeLuckPoint2Rate = 6;
            WeaponMakeLuckPoint3Rate = 10 + 30;
            CheckUserItemPlace = true;
            nClientKey = 6534;
            nClientKey = 500;
            nLevelValueOfTaosHP = 6;
            nLevelValueOfTaosHPRate = 2.5;
            nLevelValueOfTaosMP = 8;
            nLevelValueOfWizardHP = 15;
            nLevelValueOfWizardHPRate = 1.8;
            nLevelValueOfWarrHP = 4;
            nLevelValueOfWarrHPRate = 4.5;
            ProcessMonsterInterval = 2;
            ProcessMonsterMultiThreadLimit = 2;
            ProcessMonsterRetainThreadLimit = 2;
            nDBSocketSendLen = 0;
            PermissionSystem = false;
            nRenewPercent = 60;
            nAIRunIntervalTime = 1050;
            nAIWalkIntervalTime = 1800;
            nAIWarrorAttackTime = 2080;
            nAIWizardAttackTime = 2150;
            nAITaoistAttackTime = 2150;
            sAIHomeMap = "3";
            nAIHomeX = 330;
            nAIHomeY = 330;
            boHPAutoMoveMap = false;//低血回城
            boAutoRepairItem = true;
            RobotAutoPickUpItem = false;
            boRenewHealth = true;
            btHeroSkillMode = true;
            dwHeroWarrorAttackTime = 1660;
            dwHeroWizardAttackTime = 1880;
            dwHeroTaoistAttackTime = 1880;
            sAIConfigListFileName = @"D:\SystemModule\Mir200\Envir\QuestDiary\机器人配置文件列表.txt";
            sHeroAIConfigListFileName = @"D:\SystemModule\Mir200\Envir\QuestDiary\机器人配置文件列表.txt";
            boHeroAttackTarget = true;
            DecUserGameGold = 10000;
            CloseSpeedHackCheck = true;
            ConnctionString = "server=127.0.0.1;uid=root;pwd=;database=mir2;";
            sDBType = "MySQL";
            NpcNameColor = 255;
        }
    }
}