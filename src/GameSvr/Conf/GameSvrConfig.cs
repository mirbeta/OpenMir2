using SystemModule;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Conf
{
    public class GameSvrConfig
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
        public bool boVentureServer;
        public bool boTestServer;
        public bool boServiceMode;
        public bool boNonPKServer;
        public int nTestLevel;
        public int nTestGold;
        public int nTestUserLimit;
        public int nSendBlock;
        public int nCheckBlock;
        public bool boDropLargeBlock;
        public int nAvailableBlock;
        public int nGateLoad;
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
        public string sMsgSrvAddr;
        /// <summary>
        /// Master节点服务器端口
        /// </summary>
        public int nMsgSrvPort;
        public string sLogServerAddr;
        public int nLogServerPort;
        public bool boDiscountForNightTime;
        public int nHalfFeeStart;
        public int nHalfFeeEnd;
        public bool boViewHackMessage;
        public bool boViewAdmissionFailure;
        public string sBaseDir;
        public string sGuildDir;
        public string sGuildFile;
        public string sVentureDir;
        public string sConLogDir;
        public string sCastleDir;
        public string sCastleFile;
        public string sEnvirDir;
        public string sMapDir;
        public string sNoticeDir;
        public string sLogDir;
        public string sClientFile1;
        public string sClientFile2;
        public string sClientFile3;
        public string sClothsMan;
        public string sClothsWoman;
        public string sWoodenSword;
        public string sCandle;
        public string sBasicDrug;
        public string sGoldStone;
        public string sSilverStone;
        public string sSteelStone;
        public string sCopperStone;
        public string sBlackStone;
        public string sGemStone1;
        public string sGemStone2;
        public string sGemStone3;
        public string sGemStone4;
        public string[] sZuma;
        public string sBee;
        public string sSpider;
        public string sWomaHorn;
        public string sZumaPiece;
        public string sGameGoldName;
        public string sGamePointName;
        public string sPayMentPointName;
        public int DBSocket;
        public int nHealthFillTime;
        public int nSpellFillTime;
        public int nMonUpLvNeedKillBase;
        public int nMonUpLvRate;
        public int[] MonUpLvNeedKillCount;
        public byte[] SlaveColor;
        public int[] dwNeedExps;
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
        public int nKillHumanAddPKPoint;
        public int nKillHumanDecLuckPoint;
        public int dwDecLightItemDrugTime;
        public int nSafeZoneSize;
        public int nStartPointSize;
        public int dwHumanGetMsgTime;
        public int nGroupMembersMax;
        public int WarrMon;
        public int WizardMon;
        public int TaosMon;
        public int MonHum;
        public string sFireBallSkill;
        public string sHealSkill;
        public string sRingSkill;
        public byte[] ReNewNameColor;
        public int dwReNewNameColorTime;
        public bool boReNewChangeColor;
        public bool boReNewLevelClearExp;
        public TNakedAbility BonusAbilofWarr;
        public TNakedAbility BonusAbilofWizard;
        public TNakedAbility BonusAbilofTaos;
        public TNakedAbility NakedAbilofWarr;
        public TNakedAbility NakedAbilofWizard;
        public TNakedAbility NakedAbilofTaos;
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
        public int dwProcessMonstersTime;
        public int dwRegenMonstersTime;
        public int nMonGenRate;
        public int ProcessMonRandRate;
        public int ProcessMonLimitCount;
        public int SoftVersionDate;
        public bool boCanOldClientLogon;
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
        public bool boKillHumanWinLevel;
        public bool boKilledLostLevel;
        public bool boKillHumanWinExp;
        public bool boKilledLostExp;
        public int nKillHumanWinLevel;
        public int nKilledLostLevel;
        public int nKillHumanWinExp;
        public int nKillHumanLostExp;
        public int nHumanLevelDiffer;
        public int nMonsterPowerRate;
        public int nItemsPowerRate;
        public int nItemsACPowerRate;
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
        private bool LockGetBackItemAction;
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
        public bool boDecLampDura;
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
        public int dwTryDealTime;
        public int dwDealOKTime;
        public bool boCanNotGetBackDeal;
        public bool boDisableDeal;
        public int nMasterOKLevel;
        public int nMasterOKCreditPoint;
        public int nMasterOKBonusPoint;
        public bool boPKLevelProtect;
        public int nPKProtectLevel;
        public int nRedPKProtectLevel;
        public int nItemPowerRate;
        public int nItemExpRate;
        public int nScriptGotoCountLimit;
        public byte btHearMsgFColor;
        public byte btHearMsgBColor;
        public byte btWhisperMsgFColor;
        public byte btWhisperMsgBColor;
        public byte btGMWhisperMsgFColor;
        public byte btGMWhisperMsgBColor;
        public byte btCryMsgFColor;
        public byte btCryMsgBColor;
        public byte btGreenMsgFColor;
        public byte btGreenMsgBColor;
        public byte btBlueMsgFColor;
        public byte btBlueMsgBColor;
        public byte btRedMsgFColor;
        public byte btRedMsgBColor;
        public byte btGuildMsgFColor;
        public byte btGuildMsgBColor;
        public byte btGroupMsgFColor;
        public byte btGroupMsgBColor;
        public byte btCustMsgFColor;
        public byte btCustMsgBColor;
        public byte btPurpleMsgFColor;
        public byte btPurpleMsgBColor;
        public int nMonRandomAddValue;
        public int nMakeRandomAddValue;
        public int nWeaponDCAddValueMaxLimit;
        public int nWeaponDCAddValueRate;
        public int nWeaponMCAddValueMaxLimit;
        public int nWeaponMCAddValueRate;
        public int nWeaponSCAddValueMaxLimit;
        public int nWeaponSCAddValueRate;
        public int nDressDCAddRate;
        public int nDressDCAddValueMaxLimit;
        public int nDressDCAddValueRate;
        public int nDressMCAddRate;
        public int nDressMCAddValueMaxLimit;
        public int nDressMCAddValueRate;
        public int nDressSCAddRate;
        public int nDressSCAddValueMaxLimit;
        public int nDressSCAddValueRate;
        public int nNeckLace202124DCAddRate;
        public int nNeckLace202124DCAddValueMaxLimit;
        public int nNeckLace202124DCAddValueRate;
        public int nNeckLace202124MCAddRate;
        public int nNeckLace202124MCAddValueMaxLimit;
        public int nNeckLace202124MCAddValueRate;
        public int nNeckLace202124SCAddRate;
        public int nNeckLace202124SCAddValueMaxLimit;
        public int nNeckLace202124SCAddValueRate;
        public int nNeckLace19DCAddRate;
        public int nNeckLace19DCAddValueMaxLimit;
        public int nNeckLace19DCAddValueRate;
        public int nNeckLace19MCAddRate;
        public int nNeckLace19MCAddValueMaxLimit;
        public int nNeckLace19MCAddValueRate;
        public int nNeckLace19SCAddRate;
        public int nNeckLace19SCAddValueMaxLimit;
        public int nNeckLace19SCAddValueRate;
        public int nArmRing26DCAddRate;
        public int nArmRing26DCAddValueMaxLimit;
        public int nArmRing26DCAddValueRate;
        public int nArmRing26MCAddRate;
        public int nArmRing26MCAddValueMaxLimit;
        public int nArmRing26MCAddValueRate;
        public int nArmRing26SCAddRate;
        public int nArmRing26SCAddValueMaxLimit;
        public int nArmRing26SCAddValueRate;
        public int nRing22DCAddRate;
        public int nRing22DCAddValueMaxLimit;
        public int nRing22DCAddValueRate;
        public int nRing22MCAddRate;
        public int nRing22MCAddValueMaxLimit;
        public int nRing22MCAddValueRate;
        public int nRing22SCAddRate;
        public int nRing22SCAddValueMaxLimit;
        public int nRing22SCAddValueRate;
        public int nRing23DCAddRate;
        public int nRing23DCAddValueMaxLimit;
        public int nRing23DCAddValueRate;
        public int nRing23MCAddRate;
        public int nRing23MCAddValueMaxLimit;
        public int nRing23MCAddValueRate;
        public int nRing23SCAddRate;
        public int nRing23SCAddValueMaxLimit;
        public int nRing23SCAddValueRate;
        public int nHelMetDCAddRate;
        public int nHelMetDCAddValueMaxLimit;
        public int nHelMetDCAddValueRate;
        public int nHelMetMCAddRate;
        public int nHelMetMCAddValueMaxLimit;
        public int nHelMetMCAddValueRate;
        public int nHelMetSCAddRate;
        public int nHelMetSCAddValueMaxLimit;
        public int nHelMetSCAddValueRate;
        public int nUnknowHelMetACAddRate;
        public int nUnknowHelMetACAddValueMaxLimit;
        public int nUnknowHelMetMACAddRate;
        public int nUnknowHelMetMACAddValueMaxLimit;
        public int nUnknowHelMetDCAddRate;
        public int nUnknowHelMetDCAddValueMaxLimit;
        public int nUnknowHelMetMCAddRate;
        public int nUnknowHelMetMCAddValueMaxLimit;
        public int nUnknowHelMetSCAddRate;
        public int nUnknowHelMetSCAddValueMaxLimit;
        public int nUnknowRingACAddRate;
        public int nUnknowRingACAddValueMaxLimit;
        public int nUnknowRingMACAddRate;
        public int nUnknowRingMACAddValueMaxLimit;
        public int nUnknowRingDCAddRate;
        public int nUnknowRingDCAddValueMaxLimit;
        public int nUnknowRingMCAddRate;
        public int nUnknowRingMCAddValueMaxLimit;
        public int nUnknowRingSCAddRate;
        public int nUnknowRingSCAddValueMaxLimit;
        public int nUnknowNecklaceACAddRate;
        public int nUnknowNecklaceACAddValueMaxLimit;
        public int nUnknowNecklaceMACAddRate;
        public int nUnknowNecklaceMACAddValueMaxLimit;
        public int nUnknowNecklaceDCAddRate;
        public int nUnknowNecklaceDCAddValueMaxLimit;
        public int nUnknowNecklaceMCAddRate;
        public int nUnknowNecklaceMCAddValueMaxLimit;
        public int nUnknowNecklaceSCAddRate;
        public int nUnknowNecklaceSCAddValueMaxLimit;
        public int nMonOneDropGoldCount;
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
        public int nSteelStoneMin;
        public int nSteelStoneMax;
        public int nBlackStoneMin;
        public int nBlackStoneMax;
        public int nStoneMinDura;
        public int nStoneGeneralDuraRate;
        public int nStoneAddDuraRate;
        public int nStoneAddDuraMax;
        public int nWinLottery6Min;
        public int nWinLottery6Max;
        public int nWinLottery5Min;
        public int nWinLottery5Max;
        public int nWinLottery4Min;
        public int nWinLottery4Max;
        public int nWinLottery3Min;
        public int nWinLottery3Max;
        public int nWinLottery2Min;
        public int nWinLottery2Max;
        public int nWinLottery1Min;
        public int nWinLottery1Max;
        public int nWinLottery1Gold;
        public int nWinLottery2Gold;
        public int nWinLottery3Gold;
        public int nWinLottery4Gold;
        public int nWinLottery5Gold;
        public int nWinLottery6Gold;
        public int nWinLotteryRate;
        public int nWinLotteryCount;
        public int nNoWinLotteryCount;
        public int nWinLotteryLevel1;
        public int nWinLotteryLevel2;
        public int nWinLotteryLevel3;
        public int nWinLotteryLevel4;
        public int nWinLotteryLevel5;
        public int nWinLotteryLevel6;
        public int nItemNumber;
        public int nItemNumberEx;
        public int nGuildRecallTime;
        public int nGroupRecallTime;
        public bool boControlDropItem;
        public bool boInSafeDisableDrop;
        public int nCanDropGold;
        public int nCanDropPrice;
        public bool boSendCustemMsg;
        public bool boSubkMasterSendMsg;
        /// <summary>
        /// 特修价格倍数
        /// </summary>
        public int nSuperRepairPriceRate;
        /// <summary>
        /// 普通修理掉持久数(特持久上限减下限再除以此数为减的数值)
        /// </summary>        
        public int nRepairItemDecDura;
        public bool boDieScatterBag;
        public int nDieScatterBagRate;
        public bool boDieRedScatterBagAll;
        public int nDieDropUseItemRate;
        public int nDieRedDropUseItemRate;
        public bool boDieDropGold;
        public bool boKillByHumanDropUseItem;
        public bool boKillByMonstDropUseItem;
        public bool boKickExpireHuman;
        public int nGuildRankNameLen;
        public int nGuildMemberMaxLimit;
        public int nGuildNameLen;
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
        public int dwRevivalTime;
        public bool boUserMoveCanDupObj;
        public bool boUserMoveCanOnItem;
        public int dwUserMoveTime;
        public int dwPKDieLostExpRate;
        public int nPKDieLostLevelRate;
        public byte btPKFlagNameColor;
        public byte btPKLevel1NameColor;
        public byte btPKLevel2NameColor;
        public byte btAllyAndGuildNameColor;
        public byte btWarGuildNameColor;
        public byte btInFreePKAreaNameColor;
        public bool boSpiritMutiny;
        public int dwSpiritMutinyTime;
        public int nSpiritPowerRate;
        public bool boMasterDieMutiny;
        public int nMasterDieMutinyRate;
        public int nMasterDieMutinyPower;
        public int nMasterDieMutinySpeed;
        public bool boBBMonAutoChangeColor;
        public int dwBBMonAutoChangeColorTime;
        public bool boOldClientShowHiLevel;
        public bool boShowScriptActionMsg;
        public int nRunSocketDieLoopLimit;
        public bool boThreadRun;
        public bool boShowExceptionMsg;
        public bool boShowPreFixMsg;
        /// <summary>
        /// 魔法锁定范围
        /// </summary>
        public int MagicAttackRage;
        public bool nBoBoall;
        /// <summary>
        /// 物品掉落范围
        /// </summary>
        public int nDropItemRage;
        public string sSkeleton;
        public int nSkeletonCount;
        public TRecallMigic[] SkeletonArray;
        public string sDragon;
        public string sDragon1;
        public int nDragonCount;
        public TRecallMigic[] DragonArray;
        public string sAngel;
        public int nAmyOunsulPoint;
        public bool boDisableInSafeZoneFireCross;
        public bool boGroupMbAttackPlayObject;
        public int dwPosionDecHealthTime;
        /// <summary>
        /// 中红毒着持久及减防量（实际大小为 12 / 10）
        /// </summary>
        public int nPosionDamagarmor;
        public bool boLimitSwordLong;
        public int nSwordLongPowerRate;
        public int nFireBoomRage;
        public int nSnowWindRange;
        public int nElecBlizzardRange;
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
        public int nMagTammingCount;
        public int nMabMabeHitRandRate;
        public int nMabMabeHitMinLvLimit;
        public int nMabMabeHitSucessRate;
        public int nMabMabeHitMabeTimeRate;
        public string sCastleName;
        public string sCastleHomeMap;
        public int nCastleHomeX;
        public int nCastleHomeY;
        public int nCastleWarRangeX;
        public int nCastleWarRangeY;
        public int nCastleTaxRate;
        public bool boGetAllNpcTax;
        public int nHireGuardPrice;
        public int nHireArcherPrice;
        public int nCastleGoldMax;
        public int nCastleOneDayGold;
        public int nRepairDoorPrice;
        public int nRepairWallPrice;
        public int nCastleMemberPriceRate;
        public int nMaxHitMsgCount;
        public int nMaxSpellMsgCount;
        public int nMaxRunMsgCount;
        public int nMaxWalkMsgCount;
        public int nMaxTurnMsgCount;
        public int nMaxSitDonwMsgCount;
        public int nMaxDigUpMsgCount;
        public bool boSpellSendUpdateMsg;
        public bool boActionSendActionMsg;
        public bool boKickOverSpeed;
        public int btSpeedControlMode;
        public int nOverSpeedKickCount;
        public int dwDropOverSpeed;
        /// <summary>
        /// 攻击间隔(ms)
        /// </summary>
        public int dwHitIntervalTime;
        /// <summary>
        /// 魔法间隔(ms)
        /// </summary>        
        public int dwMagicHitIntervalTime;
        /// <summary>
        /// 跑步间隔(ms)
        /// </summary>        
        public int dwRunIntervalTime;
        /// <summary>
        /// 走路间隔(ms)
        /// </summary>        
        public int dwWalkIntervalTime;
        /// <summary>
        /// 换方向间隔(ms)
        /// </summary>        
        public int dwTurnIntervalTime;
        public bool boControlActionInterval;
        public bool boControlWalkHit;
        public bool boControlRunLongHit;
        public bool boControlRunHit;
        public bool boControlRunMagic;
        /// <summary>
        /// 组合操作间隔
        /// </summary>
        public int dwActionIntervalTime;
        /// <summary>
        /// 跑位刺杀间隔
        /// </summary>        
        public int dwRunLongHitIntervalTime;
        /// <summary>
        /// 跑位攻击间隔
        /// </summary>        
        public int dwRunHitIntervalTime;
        /// <summary>
        /// 走位攻击间隔
        /// </summary>        
        public int dwWalkHitIntervalTime;
        /// <summary>
        /// 跑位魔法间隔
        /// </summary>        
        public int dwRunMagicIntervalTime;
        /// <summary>
        /// 不显示人物弯腰动作
        /// </summary>        
        public bool boDisableStruck;
        /// <summary>
        /// 自己不显示人物弯腰动作
        /// </summary>        
        public bool boDisableSelfStruck;
        /// <summary>
        /// 人物弯腰停留时间
        /// </summary>        
        public int dwStruckTime;
        /// <summary>
        /// 杀怪经验倍数
        /// </summary>        
        public int dwKillMonExpMultiple;
        public int dwRequestVersion;
        public bool boHighLevelKillMonFixExp;
        public bool boHighLevelGroupFixExp;
        public bool boMonDelHptoExp;
        public int MonHptoExpLevel;
        public int MonHptoExpmax;
        public bool boUseFixExp;
        public int nBaseExp;
        public int nAddExp;
        public int nLimitExpLevel;
        public int nLimitExpValue;
        public bool boAddUserItemNewValue;
        public string sLineNoticePreFix;
        public string sSysMsgPreFix;
        public string sGuildMsgPreFix;
        public string sGroupMsgPreFix;
        public string sHintMsgPreFix;
        public string sGMRedMsgpreFix;
        public string sMonSayMsgpreFix;
        public string sCustMsgpreFix;
        public string sCastleMsgpreFix;
        public string sGuildNotice;
        public string sGuildWar;
        public string sGuildAll;
        public string sGuildMember;
        public string sGuildMemberRank;
        public string sGuildChief;
        public bool boKickAllUser;
        public bool boTestSpeedMode;
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
        public int MPStoneIntervalTime;
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
        public TClientConf ClientConf;
        public int nWeaponMakeUnLuckRate;
        public int nWeaponMakeLuckPoint1;
        public int nWeaponMakeLuckPoint2;
        public int nWeaponMakeLuckPoint3;
        public int nWeaponMakeLuckPoint2Rate;
        public int nWeaponMakeLuckPoint3Rate;
        public bool boCheckUserItemPlace;
        public int nClientKey;
        public int nLevelValueOfTaosHP;
        public double nLevelValueOfTaosHPRate;
        public int nLevelValueOfTaosMP;
        public int nLevelValueOfWizardHP;
        public double nLevelValueOfWizardHPRate;
        public int nLevelValueOfWarrHP;
        public double nLevelValueOfWarrHPRate;
        /// <summary>
        /// 怪物刷新间隔
        /// </summary>
        public int nProcessMonsterInterval;
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
        /// 假人自动拾取物品
        /// </summary>
        public bool boAutoPickUpItem;
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
        public int nDecUserGameGold;
        /// <summary>
        /// 关闭游戏引擎的加速控制
        /// </summary>
        public bool boSpeedHackCheck;
        /// <summary>
        /// NPC名字颜色控制(0-255)
        /// </summary>
        public byte NpcNameColor;

        public GameSvrConfig()
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
            boVentureServer = false;
            boTestServer = true;
            boServiceMode = false;
            boNonPKServer = false;
            nTestLevel = 1;
            nTestGold = 0;
            nTestUserLimit = 1000;
            nSendBlock = 1024;
            nCheckBlock = 4069;
            nAvailableBlock = 8000;
            nGateLoad = 0;
            UserFull = 1000;
            ZenFastStep = 300;
            sGateAddr = "127.0.0.1";
            nGatePort = 5000;
            sDBAddr = "127.0.0.1";
            nDBPort = 6000;
            sIDSAddr = "127.0.0.1";
            nIDSPort = 5600;
            sMsgSrvAddr = "127.0.0.1";
            nMsgSrvPort = 4900;
            sLogServerAddr = "127.0.0.1";
            nLogServerPort = 10000;
            boDiscountForNightTime = false;
            nHalfFeeStart = 2;
            nHalfFeeEnd = 10;
            boViewHackMessage = false;
            boViewAdmissionFailure = false;
            sBaseDir = ".\\Share\\";
            sGuildDir = ".\\GuildBase\\Guilds\\";
            sGuildFile = ".\\GuildBase\\GuildList.txt";
            sVentureDir = ".\\ShareV\\";
            sConLogDir = ".\\ConLog\\";
            sCastleDir = ".\\Envir\\Castle\\";
            sCastleFile = ".\\Envir\\Castle\\List.txt";
            sEnvirDir = ".\\Envir\\";
            sMapDir = ".\\Map\\";
            sNoticeDir = ".\\Notice\\";
            sLogDir = ".\\Log\\";
            sClientFile1 = "mir.1";
            sClientFile2 = "mir.Dat";
            sClientFile3 = "mir.3";
            sClothsMan = "布衣(男)";
            sClothsWoman = "布衣(女)";
            sWoodenSword = "木剑";
            sCandle = "蜡烛";
            sBasicDrug = "金创药(小量)";
            sGoldStone = "金矿";
            sSilverStone = "银矿";
            sSteelStone = "铁矿";
            sCopperStone = "铜矿";
            sBlackStone = "黑铁矿";
            sGemStone1 = "金刚石矿";
            sGemStone2 = "绿宝石矿";
            sGemStone3 = "红宝石矿";
            sGemStone4 = "白宝石矿";
            sZuma = new string[] { "祖玛卫士", "祖玛雕像", "祖玛弓箭手", "楔蛾" };
            sBee = "蝙蝠";
            sSpider = "爆裂蜘蛛";
            sWomaHorn = "沃玛号角";
            sZumaPiece = "祖玛头像";
            sGameGoldName = "元宝";
            sGamePointName = "游戏点";
            sPayMentPointName = "荣誉值";
            DBSocket = 0;
            nHealthFillTime = 300;
            nSpellFillTime = 800;
            nMonUpLvNeedKillBase = 100;
            nMonUpLvRate = 16;
            MonUpLvNeedKillCount = new int[] { 0, 0, 50, 100, 200, 300, 600, 1200, 2400, 4800, 9600, 19200, 38400, 76800, 153600 };
            SlaveColor = new byte[] { 0xFF, 0xFE, 0x93, 0x9A, 0xE5, 0xA8, 0xB4, 0xFC, 249, 250, 250, 250, 250, 250, 250 };
            dwNeedExps = new int[Grobal2.MaxChangeLevel];
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
            nKillHumanAddPKPoint = 100;
            nKillHumanDecLuckPoint = 500;
            dwDecLightItemDrugTime = 500;
            nSafeZoneSize = 10;
            nStartPointSize = 2;
            dwHumanGetMsgTime = 200;
            nGroupMembersMax = 10;
            WarrMon = 10;
            WizardMon = 10;
            TaosMon = 10;
            MonHum = 10;
            sFireBallSkill = "火球术";
            sHealSkill = "治愈术";
            ReNewNameColor = new byte[] { 0xFF, 0xFE, 0x93, 0x9A, 0xE5, 0xA8, 0xB4, 0xFC, 0xB4, 0xFC };
            dwReNewNameColorTime = 2000;
            boReNewChangeColor = true;
            boReNewLevelClearExp = true;
            BonusAbilofWarr = new TNakedAbility { DC = 17, MC = 20, SC = 20, AC = 20, MAC = 20, HP = 1, MP = 3, Hit = 20, Speed = 35, X2 = 0 };
            BonusAbilofWizard = new TNakedAbility { DC = 17, MC = 25, SC = 30, AC = 20, MAC = 15, HP = 2, MP = 1, Hit = 25, Speed = 35, X2 = 0 };
            BonusAbilofTaos = new TNakedAbility { DC = 20, MC = 30, SC = 17, AC = 20, MAC = 15, HP = 2, MP = 1, Hit = 30, Speed = 30, X2 = 0 };
            NakedAbilofWarr = new TNakedAbility { DC = 512, MC = 2560, SC = 20, AC = 768, MAC = 1280, HP = 0, MP = 0, Hit = 0, Speed = 0, X2 = 0 };
            NakedAbilofWizard = new TNakedAbility { DC = 512, MC = 512, SC = 2560, AC = 1280, MAC = 768, HP = 0, MP = 0, Hit = 5, Speed = 0, X2 = 0 };
            NakedAbilofTaos = new TNakedAbility { DC = 20, MC = 30, SC = 17, AC = 20, MAC = 15, HP = 2, MP = 1, Hit = 30, Speed = 30, X2 = 0 };
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
            dwProcessMonstersTime = 10;
            dwRegenMonstersTime = 200;
            nMonGenRate = 10;
            ProcessMonRandRate = 5;
            ProcessMonLimitCount = 5;
            SoftVersionDate = 20020522;
            boCanOldClientLogon = true;
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
            boKillHumanWinLevel = false;
            boKilledLostLevel = false;
            boKillHumanWinExp = false;
            boKilledLostExp = false;
            nKillHumanWinLevel = 1;
            nKilledLostLevel = 1;
            nKillHumanWinExp = 100000;
            nKillHumanLostExp = 100000;
            nHumanLevelDiffer = 10;
            nMonsterPowerRate = 10;
            nItemsPowerRate = 10;
            nItemsACPowerRate = 10;
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
            boDecLampDura = true;
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
            dwTryDealTime = 3000;
            dwDealOKTime = 1000;
            boCanNotGetBackDeal = true;
            boDisableDeal = false;
            nMasterOKLevel = 500;
            nMasterOKCreditPoint = 0;
            nMasterOKBonusPoint = 0;
            boPKLevelProtect = false;
            nPKProtectLevel = 10;
            nRedPKProtectLevel = 10;
            nItemPowerRate = 10000;
            nItemExpRate = 10000;
            nScriptGotoCountLimit = 30;
            btHearMsgFColor = 0x00; //前景
            btHearMsgBColor = 0xFF; //背景
            btWhisperMsgFColor = 0xFC; //前景
            btWhisperMsgBColor = 0xFF; //背景
            btGMWhisperMsgFColor = 0xFF; //前景
            btGMWhisperMsgBColor = 0x38; //背景
            btCryMsgFColor = 0x0; //前景
            btCryMsgBColor = 0x97; //背景
            btGreenMsgFColor = 0xDB; //前景
            btGreenMsgBColor = 0xFF; //背景
            btBlueMsgFColor = 0xFF; //前景
            btBlueMsgBColor = 0xFC; //背景
            btRedMsgFColor = 0xFF; //前景
            btRedMsgBColor = 0x38; //背景
            btGuildMsgFColor = 0xDB; //前景
            btGuildMsgBColor = 0xFF; //背景
            btGroupMsgFColor = 0xC4; //前景
            btGroupMsgBColor = 0xFF; //背景
            btCustMsgFColor = 0xFC; //前景
            btCustMsgBColor = 0xFF; //背景
            btPurpleMsgFColor = 0xFF;
            btPurpleMsgBColor = 253;
            nMonRandomAddValue = 10;
            nMakeRandomAddValue = 10;
            nWeaponDCAddValueMaxLimit = 12;
            nWeaponDCAddValueRate = 15;
            nWeaponMCAddValueMaxLimit = 12;
            nWeaponMCAddValueRate = 15;
            nWeaponSCAddValueMaxLimit = 12;
            nWeaponSCAddValueRate = 15;
            nDressDCAddRate = 40;
            nDressDCAddValueMaxLimit = 6;
            nDressDCAddValueRate = 20;
            nDressMCAddRate = 40;
            nDressMCAddValueMaxLimit = 6;
            nDressMCAddValueRate = 20;
            nDressSCAddRate = 40;
            nDressSCAddValueMaxLimit = 6;
            nDressSCAddValueRate = 20;
            nNeckLace202124DCAddRate = 40;
            nNeckLace202124DCAddValueMaxLimit = 6;
            nNeckLace202124DCAddValueRate = 20;
            nNeckLace202124MCAddRate = 40;
            nNeckLace202124MCAddValueMaxLimit = 6;
            nNeckLace202124MCAddValueRate = 20;
            nNeckLace202124SCAddRate = 40;
            nNeckLace202124SCAddValueMaxLimit = 6;
            nNeckLace202124SCAddValueRate = 20;
            nNeckLace19DCAddRate = 30;
            nNeckLace19DCAddValueMaxLimit = 6;
            nNeckLace19DCAddValueRate = 20;
            nNeckLace19MCAddRate = 30;
            nNeckLace19MCAddValueMaxLimit = 6;
            nNeckLace19MCAddValueRate = 20;
            nNeckLace19SCAddRate = 30;
            nNeckLace19SCAddValueMaxLimit = 6;
            nNeckLace19SCAddValueRate = 20;
            nArmRing26DCAddRate = 30;
            nArmRing26DCAddValueMaxLimit = 6;
            nArmRing26DCAddValueRate = 20;
            nArmRing26MCAddRate = 30;
            nArmRing26MCAddValueMaxLimit = 6;
            nArmRing26MCAddValueRate = 20;
            nArmRing26SCAddRate = 30;
            nArmRing26SCAddValueMaxLimit = 6;
            nArmRing26SCAddValueRate = 20;
            nRing22DCAddRate = 30;
            nRing22DCAddValueMaxLimit = 6;
            nRing22DCAddValueRate = 20;
            nRing22MCAddRate = 30;
            nRing22MCAddValueMaxLimit = 6;
            nRing22MCAddValueRate = 20;
            nRing22SCAddRate = 30;
            nRing22SCAddValueMaxLimit = 6;
            nRing22SCAddValueRate = 20;
            nRing23DCAddRate = 30;
            nRing23DCAddValueMaxLimit = 6;
            nRing23DCAddValueRate = 20;
            nRing23MCAddRate = 30;
            nRing23MCAddValueMaxLimit = 6;
            nRing23MCAddValueRate = 20;
            nRing23SCAddRate = 30;
            nRing23SCAddValueMaxLimit = 6;
            nRing23SCAddValueRate = 20;
            nHelMetDCAddRate = 30;
            nHelMetDCAddValueMaxLimit = 6;
            nHelMetDCAddValueRate = 20;
            nHelMetMCAddRate = 30;
            nHelMetMCAddValueMaxLimit = 6;
            nHelMetMCAddValueRate = 20;
            nHelMetSCAddRate = 30;
            nHelMetSCAddValueMaxLimit = 6;
            nHelMetSCAddValueRate = 20;
            nUnknowHelMetACAddRate = 20;
            nUnknowHelMetACAddValueMaxLimit = 4;
            nUnknowHelMetMACAddRate = 20;
            nUnknowHelMetMACAddValueMaxLimit = 4;
            nUnknowHelMetDCAddRate = 30;
            nUnknowHelMetDCAddValueMaxLimit = 3;
            nUnknowHelMetMCAddRate = 30;
            nUnknowHelMetMCAddValueMaxLimit = 3;
            nUnknowHelMetSCAddRate = 30;
            nUnknowHelMetSCAddValueMaxLimit = 3;
            nUnknowRingACAddRate = 20;
            nUnknowRingACAddValueMaxLimit = 4;
            nUnknowRingMACAddRate = 20;
            nUnknowRingMACAddValueMaxLimit = 4;
            nUnknowRingDCAddRate = 20;
            nUnknowRingDCAddValueMaxLimit = 6;
            nUnknowRingMCAddRate = 20;
            nUnknowRingMCAddValueMaxLimit = 6;
            nUnknowRingSCAddRate = 20;
            nUnknowRingSCAddValueMaxLimit = 6;
            nUnknowNecklaceACAddRate = 20;
            nUnknowNecklaceACAddValueMaxLimit = 5;
            nUnknowNecklaceMACAddRate = 20;
            nUnknowNecklaceMACAddValueMaxLimit = 5;
            nUnknowNecklaceDCAddRate = 30;
            nUnknowNecklaceDCAddValueMaxLimit = 5;
            nUnknowNecklaceMCAddRate = 30;
            nUnknowNecklaceMCAddValueMaxLimit = 5;
            nUnknowNecklaceSCAddRate = 30;
            nUnknowNecklaceSCAddValueMaxLimit = 5;
            nMonOneDropGoldCount = 2000;
            SendCurTickCount = true;  //客户端时间
            MakeMineHitRate = 4; //挖矿命中率
            MakeMineRate = 12; //挖矿率
            StoneTypeRate = 120;
            StoneTypeRateMin = 56;
            GoldStoneMin = 1;
            GoldStoneMax = 2;
            SilverStoneMin = 3;
            SilverStoneMax = 20;
            nSteelStoneMin = 21;
            nSteelStoneMax = 45;
            nBlackStoneMin = 46;
            nBlackStoneMax = 56;
            nStoneMinDura = 3000;
            nStoneGeneralDuraRate = 13000;
            nStoneAddDuraRate = 20;
            nStoneAddDuraMax = 10000;
            nWinLottery6Min = 1;
            nWinLottery6Max = 4999;
            nWinLottery5Min = 14000;
            nWinLottery5Max = 15999;
            nWinLottery4Min = 16000;
            nWinLottery4Max = 16149;
            nWinLottery3Min = 16150;
            nWinLottery3Max = 16169;
            nWinLottery2Min = 16170;
            nWinLottery2Max = 16179;
            nWinLottery1Min = 16180;
            nWinLottery1Max = 16185;//16180 + 1820;
            nWinLottery1Gold = 1000000;
            nWinLottery2Gold = 200000;
            nWinLottery3Gold = 100000;
            nWinLottery4Gold = 10000;
            nWinLottery5Gold = 1000;
            nWinLottery6Gold = 500;
            nWinLotteryRate = 30000;
            nWinLotteryCount = 0;
            nNoWinLotteryCount = 0;
            nWinLotteryLevel1 = 0;
            nWinLotteryLevel2 = 0;
            nWinLotteryLevel3 = 0;
            nWinLotteryLevel4 = 0;
            nWinLotteryLevel5 = 0;
            nWinLotteryLevel6 = 0;
            GlobalVal = new int[500];
            GlobaDyMval = new int[500];
            GlobalAVal = new string[500];
            nItemNumber = 0;
            nItemNumberEx = int.MaxValue / 2;
            nGuildRecallTime = 180;
            nGroupRecallTime = 180;
            boControlDropItem = false;
            boInSafeDisableDrop = false;
            nCanDropGold = 1000;
            nCanDropPrice = 500;
            boSendCustemMsg = true;
            boSubkMasterSendMsg = true;
            nSuperRepairPriceRate = 3; //特修价格倍数
            nRepairItemDecDura = 30; //普通修理掉持久数(特持久上限减下限再除以此数为减的数值)
            boDieScatterBag = true;
            nDieScatterBagRate = 3;
            boDieRedScatterBagAll = true;
            nDieDropUseItemRate = 30;
            nDieRedDropUseItemRate = 15;
            boDieDropGold = false;
            boKillByHumanDropUseItem = false;
            boKillByMonstDropUseItem = true;
            boKickExpireHuman = false;
            nGuildRankNameLen = 16;
            nGuildMemberMaxLimit = 200;
            nGuildNameLen = 16;
            AttackPosionRate = 5;
            AttackPosionTime = 5;
            dwRevivalTime = 60 * 1000; //复活间隔时间
            boUserMoveCanDupObj = false;
            boUserMoveCanOnItem = true;
            dwUserMoveTime = 10;
            dwPKDieLostExpRate = 1000;
            nPKDieLostLevelRate = 20000;
            btPKFlagNameColor = 0x2F;
            btPKLevel1NameColor = 0xFB;
            btPKLevel2NameColor = 0xF9;
            btAllyAndGuildNameColor = 0xB4;
            btWarGuildNameColor = 0x45;
            btInFreePKAreaNameColor = 0xDD;
            boSpiritMutiny = false;
            dwSpiritMutinyTime = 30 * 60 * 1000;
            nSpiritPowerRate = 2;
            boMasterDieMutiny = false;
            nMasterDieMutinyRate = 5;
            nMasterDieMutinyPower = 10;
            nMasterDieMutinySpeed = 5;
            boBBMonAutoChangeColor = false;
            dwBBMonAutoChangeColorTime = 3000;
            boOldClientShowHiLevel = true;
            boShowScriptActionMsg = true;
            nRunSocketDieLoopLimit = 100;
            boThreadRun = false;
            boShowExceptionMsg = false;
            boShowPreFixMsg = false;
            MagicAttackRage = 8; //魔法锁定范围
            nBoBoall = true;
            nDropItemRage = 3; //爆物范围
            sSkeleton = "变异骷髅";
            nSkeletonCount = 1;
            SkeletonArray = new TRecallMigic[10];
            sDragon = "神兽";
            sDragon1 = "神兽1";
            nDragonCount = 1;
            DragonArray = new TRecallMigic[10];
            sAngel = "精灵";
            nAmyOunsulPoint = 10;
            boDisableInSafeZoneFireCross = false;
            boGroupMbAttackPlayObject = false;
            dwPosionDecHealthTime = 2500;
            nPosionDamagarmor = 12;//中红毒着持久及减防量（实际大小为 12 / 10）
            boLimitSwordLong = false;
            nSwordLongPowerRate = 100;
            nFireBoomRage = 1;
            nSnowWindRange = 1;
            nElecBlizzardRange = 2;
            MagTurnUndeadLevel = 50; //圣言怪物等级限制
            MagTammingLevel = 50; //诱惑之光怪物等级限制
            MagTammingTargetLevel = 10; //诱惑怪物相差等级机率，此数字越小机率越大；
            MagTammingHPRate = 100; //成功机率=怪物最高HP 除以 此倍率，此倍率越大诱惑机率越高
            nMagTammingCount = 5;
            nMabMabeHitRandRate = 100;
            nMabMabeHitMinLvLimit = 10;
            nMabMabeHitSucessRate = 21;
            nMabMabeHitMabeTimeRate = 20;
            sCastleName = "沙巴克";
            sCastleHomeMap = "3";
            nCastleHomeX = 644;
            nCastleHomeY = 290;
            nCastleWarRangeX = 100;
            nCastleWarRangeY = 100;
            nCastleTaxRate = 5;
            boGetAllNpcTax = false;
            nHireGuardPrice = 300000;
            nHireArcherPrice = 300000;
            nCastleGoldMax = 10000000;
            nCastleOneDayGold = 2000000;
            nRepairDoorPrice = 2000000;
            nRepairWallPrice = 500000;
            nCastleMemberPriceRate = 80;
            nMaxHitMsgCount = 1;
            nMaxSpellMsgCount = 1;
            nMaxRunMsgCount = 1;
            nMaxWalkMsgCount = 1;
            nMaxTurnMsgCount = 1;
            nMaxSitDonwMsgCount = 1;
            nMaxDigUpMsgCount = 1;
            boSpellSendUpdateMsg = true;
            boActionSendActionMsg = true;
            boKickOverSpeed = false;
            btSpeedControlMode = 0;
            nOverSpeedKickCount = 4;
            dwDropOverSpeed = 10;
            dwHitIntervalTime = 900; //攻击间隔
            dwMagicHitIntervalTime = 800; //魔法间隔
            dwRunIntervalTime = 600; //跑步间隔
            dwWalkIntervalTime = 600; //走路间隔
            dwTurnIntervalTime = 600; //换方向间隔
            boControlActionInterval = true;
            boControlWalkHit = true;
            boControlRunLongHit = true;
            boControlRunHit = true;
            boControlRunMagic = true;
            dwActionIntervalTime = 350; //组合操作间隔
            dwRunLongHitIntervalTime = 800; //跑位刺杀间隔
            dwRunHitIntervalTime = 800; //跑位攻击间隔
            dwWalkHitIntervalTime = 800; //走位攻击间隔
            dwRunMagicIntervalTime = 900; //跑位魔法间隔
            boDisableStruck = false; //不显示人物弯腰动作
            boDisableSelfStruck = false; //自己不显示人物弯腰动作
            dwStruckTime = 100; //人物弯腰停留时间
            dwKillMonExpMultiple = 1; //杀怪经验倍数
            dwRequestVersion = 98;
            boHighLevelKillMonFixExp = false;
            boHighLevelGroupFixExp = true;
            boMonDelHptoExp = false;
            MonHptoExpLevel = 100;
            MonHptoExpmax = 1;
            boUseFixExp = true;
            nBaseExp = 100000000;
            nAddExp = 1000000;
            nLimitExpLevel = 1000;
            nLimitExpValue = 1;
            boAddUserItemNewValue = true;
            sLineNoticePreFix = "〖公告〗";
            sSysMsgPreFix = "〖系统〗";
            sGuildMsgPreFix = "〖行会〗";
            sGroupMsgPreFix = "〖组队〗";
            sHintMsgPreFix = "〖提示〗";
            sGMRedMsgpreFix = "〖ＧＭ〗";
            sMonSayMsgpreFix = "〖怪物〗";
            sCustMsgpreFix = "〖祝福〗";
            sCastleMsgpreFix = "〖城主〗";
            sGuildNotice = "公告";
            sGuildWar = "敌对行会";
            sGuildAll = "联盟行会";
            sGuildMember = "行会成员";
            sGuildMemberRank = "行会成员";
            sGuildChief = "掌门人";
            boKickAllUser = false;
            boTestSpeedMode = false;
            HPStoneStartRate = 80; //气血石
            MPStoneStartRate = 80; //魔血石
            HPStoneIntervalTime = 1000; //气血石
            MPStoneIntervalTime = 1000; //魔血石
            HPStoneAddRate = 10; //气血石
            MPStoneAddRate = 10; //魔血石
            HPStoneDecDura = 1000; //气血石
            MPStoneDecDura = 1000; //魔血石
            ClientConf = new TClientConf()
            {
                boClientCanSet = true,
                boRunHuman = false,
                boRunMon = false,
                boRunNpc = false,
                boWarRunAll = false,
                btDieColor = 5,
                wSpellTime = 500,
                wHitIime = 1400,
                wItemFlashTime = 5 * 100,
                btItemSpeed = 25,
                boCanStartRun = false,
                boParalyCanRun = false,
                boParalyCanWalk = false,
                boParalyCanHit = false,
                boParalyCanSpell = false,
                boShowRedHPLable = false,
                boShowHPNumber = false,
                boShowJobLevel = true,
                boDuraAlert = true,
                boMagicLock = false,
                boAutoPuckUpItem = false
            };
            nWeaponMakeUnLuckRate = 20;
            nWeaponMakeLuckPoint1 = 1;
            nWeaponMakeLuckPoint2 = 3;
            nWeaponMakeLuckPoint3 = 7;
            nWeaponMakeLuckPoint2Rate = 6;
            nWeaponMakeLuckPoint3Rate = 10 + 30;
            boCheckUserItemPlace = true;
            nClientKey = 6534;
            nClientKey = 500;
            nLevelValueOfTaosHP = 6;
            nLevelValueOfTaosHPRate = 2.5;
            nLevelValueOfTaosMP = 8;
            nLevelValueOfWizardHP = 15;
            nLevelValueOfWizardHPRate = 1.8;
            nLevelValueOfWarrHP = 4;
            nLevelValueOfWarrHPRate = 4.5;
            nProcessMonsterInterval = 2;
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
            boAutoPickUpItem = false;
            boRenewHealth = true;
            btHeroSkillMode = true;
            dwHeroWarrorAttackTime = 1660;
            dwHeroWizardAttackTime = 1880;
            dwHeroTaoistAttackTime = 1880;
            sAIConfigListFileName = @"D:\SystemModule\Mir200\Envir\QuestDiary\机器人配置文件列表.txt";
            sHeroAIConfigListFileName = @"D:\SystemModule\Mir200\Envir\QuestDiary\机器人配置文件列表.txt";
            boHeroAttackTarget = true;
            nDecUserGameGold = 10000;
            boSpeedHackCheck = true;
            ConnctionString = "server=127.0.0.1;uid=root;pwd=;database=mir2;";
            sDBType = "MySQL";
            NpcNameColor = 255;
        }
    }
}