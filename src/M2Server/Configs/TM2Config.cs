using SystemModule;

namespace M2Server
{
    public class TM2Config
    {
        public int nConfigSize;
        public string sServerName;
        public string sServerIPaddr;
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
        public int nUserFull;
        /// <summary>
        /// 怪物行动速度(默认300)
        /// </summary>
        public int nZenFastStep;
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
        public string sPlugDir;
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
        public int[] WideAttack;
        public int[] CrsAttack;
        public int[, ,] SpitMap;
        public string sHomeMap;
        public short nHomeX;
        public short nHomeY;
        public string sRedHomeMap;
        public short nRedHomeX;
        public short nRedHomeY;
        public string sRedDieHomeMap;
        public short nRedDieHomeX;
        public short nRedDieHomeY;
        public bool boJobHomePoint;
        public string sWarriorHomeMap;
        public short nWarriorHomeX;
        public short nWarriorHomeY;
        public string sWizardHomeMap;
        public short nWizardHomeX;
        public short nWizardHomeY;
        public string sTaoistHomeMap;
        public short nTaoistHomeX;
        public short nTaoistHomeY;
        public int dwDecPkPointTime;
        public int nDecPkPointCount;
        public int dwPKFlagTime;
        public int nKillHumanAddPKPoint;
        public int nKillHumanDecLuckPoint;
        public int dwDecLightItemDrugTime;
        public int nSafeZoneSize;
        public int nStartPointSize;
        public int dwHumanGetMsgTime;
        public int nGroupMembersMax;
        public int nWarrMon;
        public int nWizardMon;
        public int nTaosMon;
        public int nMonHum;
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
        public int nUpgradeWeaponMaxPoint;
        public int nUpgradeWeaponPrice;
        public int dwUPgradeWeaponGetBackTime;
        public int nClearExpireUpgradeWeaponDays;
        public int nUpgradeWeaponDCRate;
        public int nUpgradeWeaponDCTwoPointRate;
        public int nUpgradeWeaponDCThreePointRate;
        public int nUpgradeWeaponSCRate;
        public int nUpgradeWeaponSCTwoPointRate;
        public int nUpgradeWeaponSCThreePointRate;
        public int nUpgradeWeaponMCRate;
        public int nUpgradeWeaponMCTwoPointRate;
        public int nUpgradeWeaponMCThreePointRate;
        public int dwProcessMonstersTime;
        public int dwRegenMonstersTime;
        public int nMonGenRate;
        public int nProcessMonRandRate;
        public int nProcessMonLimitCount;
        public int nSoftVersionDate;
        public bool boCanOldClientLogon;
        public int dwConsoleShowUserCountTime;
        public int dwShowLineNoticeTime;
        public int nLineNoticeColor;
        public int nStartCastleWarDays;
        public int nStartCastlewarTime;
        public int dwShowCastleWarEndMsgTime;
        public int dwCastleWarTime;
        public int dwGetCastleTime;
        public int dwGuildWarTime;
        public int nBuildGuildPrice;
        public int nGuildWarPrice;
        public int nMakeDurgPrice;
        public int nHumanMaxGold;
        public int nHumanTryModeMaxGold;
        public int nTryModeLevel;
        public bool boTryModeUseStorage;
        public int nCanShoutMsgLevel;
        public bool boShowMakeItemMsg;
        public bool boShutRedMsgShowGMName;
        public int nSayMsgMaxLen;
        public int dwSayMsgTime;
        public int nSayMsgCount;
        public int dwDisableSayMsgTime;
        public int nSayRedMsgMaxLen;
        public bool boShowGuildName;
        public bool boShowRankLevelName;
        public bool boMonSayMsg;
        public int nStartPermission;
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
        public bool boSendOnlineCount;
        public int nSendOnlineCountRate;
        public int dwclickNpcTime;
        // NPC点击间隔
        public int dwthcHumanTime;
        // 踢出未登陆人物
        public int dwQueryBagItemsTick;
        // 包裹刷新间隔
        public int dwSendOnlineTime;
        public int dwSaveHumanRcdTime;
        public int dwHumanFreeDelayTime;
        public int dwMakeGhostTime;
        public int dwClearDropOnFloorItemTime;
        public int dwFloorItemCanPickUpTime;
        public bool boPasswordLockSystem;
        // 是否启用密码保护系统
        public bool boLockDealAction;
        // 是否锁定交易操作
        public bool boLockDropAction;
        // 是否锁定扔物品操作
        public bool boLockGetBackItemAction;
        // 是否锁定取仓库操作
        public bool boLockHumanLogin;
        // 是否锁定走操作
        public bool boLockWalkAction;
        // 是否锁定走操作
        public bool boLockRunAction;
        // 是否锁定跑操作
        public bool boLockHitAction;
        // 是否锁定攻击操作
        public bool boLockSpellAction;
        // 是否锁定魔法操作
        public bool boLockSendMsgAction;
        // 是否锁定发信息操作
        public bool boLockUserItemAction;
        // 是否锁定使用物品操作
        public bool boLockInObModeAction;
        // 锁定时进入隐身状态
        public int nPasswordErrorCountLock;
        // 输入密码错误超过 指定次数则锁定密码
        public bool boPasswordErrorKick;
        // 输入密码错误超过限制则踢下线
        public int nSendRefMsgRange;
        public bool boDecLampDura;
        public bool boHungerSystem;
        public bool boHungerDecHP;
        public bool boHungerDecPower;
        public bool boDiableHumanRun;
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
        // 前景
        public byte btHearMsgBColor;
        // 背景
        public byte btWhisperMsgFColor;
        // 前景
        public byte btWhisperMsgBColor;
        // 背景
        public byte btGMWhisperMsgFColor;
        // 前景
        public byte btGMWhisperMsgBColor;
        // 背景
        public byte btCryMsgFColor;
        // 前景
        public byte btCryMsgBColor;
        // 背景
        public byte btGreenMsgFColor;
        // 前景
        public byte btGreenMsgBColor;
        // 背景
        public byte btBlueMsgFColor;
        // 前景
        public byte btBlueMsgBColor;
        // 背景
        public byte btRedMsgFColor;
        // 前景
        public byte btRedMsgBColor;
        // 背景
        public byte btGuildMsgFColor;
        // 前景
        public byte btGuildMsgBColor;
        // 背景
        public byte btGroupMsgFColor;
        // 前景
        public byte btGroupMsgBColor;
        // 背景
        public byte btCustMsgFColor;
        // 前景
        public byte btCustMsgBColor;
        // 背景
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
        public bool boSendCurTickCount;
        // 客户端时间
        public int nMakeMineHitRate;
        // 挖矿命中率
        public int nMakeMineRate;
        // 挖矿率
        public int nStoneTypeRate;
        public int nStoneTypeRateMin;
        public int nGoldStoneMin;
        public int nGoldStoneMax;
        public int nSilverStoneMin;
        public int nSilverStoneMax;
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
        // 16180 + 1820;
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
        public int[] GlobalVal;
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
        public int nSuperRepairPriceRate;
        // 特修价格倍数
        public int nRepairItemDecDura;
        // 普通修理掉持久数(特持久上限减下限再除以此数为减的数值)
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
        public int nAttackPosionRate;
        public int nAttackPosionTime;
        public int dwRevivalTime;
        // 复活间隔时间
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
        public int nMagicAttackRage;
        // 魔法锁定范围
        public bool nBoBoall;
        public int nDropItemRage;
        // 爆物范围
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
        public int nMagTurnUndeadLevel;
        // 圣言怪物等级限制
        public int nMagTammingLevel;
        // 诱惑之光怪物等级限制
        public int nMagTammingTargetLevel;
        // 诱惑怪物相差等级机率，此数字越小机率越大；
        public int nMagTammingHPRate;
        // 成功机率=怪物最高HP 除以 此倍率，此倍率越大诱惑机率越高
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
        public int dwHitIntervalTime;
        // 攻击间隔
        public int dwMagicHitIntervalTime;
        // 魔法间隔
        public int dwRunIntervalTime;
        // 跑步间隔
        public int dwWalkIntervalTime;
        // 走路间隔
        public int dwTurnIntervalTime;
        // 换方向间隔
        public bool boControlActionInterval;
        public bool boControlWalkHit;
        public bool boControlRunLongHit;
        public bool boControlRunHit;
        public bool boControlRunMagic;
        public int dwActionIntervalTime;
        // 组合操作间隔
        public int dwRunLongHitIntervalTime;
        // 跑位刺杀间隔
        public int dwRunHitIntervalTime;
        // 跑位攻击间隔
        public int dwWalkHitIntervalTime;
        // 走位攻击间隔
        public int dwRunMagicIntervalTime;
        // 跑位魔法间隔
        public bool boDisableStruck;
        // 不显示人物弯腰动作
        public bool boDisableSelfStruck;
        // 自己不显示人物弯腰动作
        public int dwStruckTime;
        // 人物弯腰停留时间
        public int dwKillMonExpMultiple;
        // 杀怪经验倍数
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
        public byte HPStoneStartRate;
        // 气血石
        public byte MPStoneStartRate;
        // 魔血石
        public int HPStoneIntervalTime;
        // 气血石
        public int MPStoneIntervalTime;
        // 魔血石
        public byte HPStoneAddRate;
        // 气血石
        public byte MPStoneAddRate;
        // 魔血石
        public int HPStoneDecDura;
        // 气血石
        public int MPStoneDecDura;
        // 魔血石
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
        /// 刷新间隔
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
        public string sDBSocketRecvText;
        public bool boDBSocketWorking;
        public int nDBSocketRecvCount;
        public int nDBReceiveMaxTime;
        public int nDBSocketWSAErrCode;
        public int nDBSocketErrorCount;
        public int nServerFile_CRCB;
        public int nClientFile1_CRC;
        public int nClientFile2_CRC;
        public int nClientFile3_CRC;
        public short[] GlobaDyMval;
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

        public TM2Config()
        {
            nConfigSize = 0;
            sServerName = "SKY引擎";
            sServerIPaddr = "127.0.0.1";
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
            nUserFull = 1000;
            nZenFastStep = 300;
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
            sPlugDir = ".\\";
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
            MonUpLvNeedKillCount = new int[] { 0, 0, 50, 100, 200, 300, 600, 1200 };
            SlaveColor = new byte[] { 0xFF, 0xFE, 0x93, 0x9A, 0xE5, 0xA8, 0xB4, 0xFC, 249 };
            dwNeedExps = new int[Grobal2.MAXCHANGELEVEL];
            WideAttack = new int[] { 7, 1, 2 };
            CrsAttack = new int[] { 7, 1, 2, 3, 4, 5, 6 };
            SpitMap = new int[,,]{
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

            sHomeMap = "0";
            nHomeX = 289;
            nHomeY = 618;
            sRedHomeMap = "3";
            nRedHomeX = 845;
            nRedHomeY = 674;
            sRedDieHomeMap = "3";
            nRedDieHomeX = 839;
            nRedDieHomeY = 668;
            boJobHomePoint = false;
            sWarriorHomeMap = "0";
            nWarriorHomeX = 289;
            nWarriorHomeY = 618;
            sWizardHomeMap = "0";
            nWizardHomeX = 650;
            nWizardHomeY = 631;
            sTaoistHomeMap = "0";
            nTaoistHomeX = 334;
            nTaoistHomeY = 266;
            dwDecPkPointTime = 2 * 60 * 1000;
            nDecPkPointCount = 1;
            dwPKFlagTime = 60 * 1000;
            nKillHumanAddPKPoint = 100;
            nKillHumanDecLuckPoint = 500;
            dwDecLightItemDrugTime = 500;
            nSafeZoneSize = 10;
            nStartPointSize = 2;
            dwHumanGetMsgTime = 200;
            nGroupMembersMax = 10;
            nWarrMon = 10;
            nWizardMon = 10;
            nTaosMon = 10;
            nMonHum = 10;
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
            nUpgradeWeaponMaxPoint = 20;
            nUpgradeWeaponPrice = 10000;
            dwUPgradeWeaponGetBackTime = 60 * 60 * 1000;
            nClearExpireUpgradeWeaponDays = 8;
            nUpgradeWeaponDCRate = 100;
            nUpgradeWeaponDCTwoPointRate = 30;
            nUpgradeWeaponDCThreePointRate = 200;
            nUpgradeWeaponSCRate = 100;
            nUpgradeWeaponSCTwoPointRate = 30;
            nUpgradeWeaponSCThreePointRate = 200;
            nUpgradeWeaponMCRate = 100;
            nUpgradeWeaponMCTwoPointRate = 30;
            nUpgradeWeaponMCThreePointRate = 200;
            dwProcessMonstersTime = 10;
            dwRegenMonstersTime = 200;
            nMonGenRate = 10;
            nProcessMonRandRate = 5;
            nProcessMonLimitCount = 5;
            nSoftVersionDate = 20020522;
            boCanOldClientLogon = true;
            dwConsoleShowUserCountTime = 10 * 60 * 1000;
            dwShowLineNoticeTime = 5 * 60 * 1000;
            nLineNoticeColor = 2;
            nStartCastleWarDays = 4;
            nStartCastlewarTime = 20;
            dwShowCastleWarEndMsgTime = 10 * 60 * 1000;
            dwCastleWarTime = 3 * 60 * 60 * 1000;
            dwGetCastleTime = 10 * 60 * 1000;
            dwGuildWarTime = 3 * 60 * 60 * 1000;
            nBuildGuildPrice = 1000000;
            nGuildWarPrice = 30000;
            nMakeDurgPrice = 100;
            nHumanMaxGold = 10000000;
            nHumanTryModeMaxGold = 100000;
            nTryModeLevel = 7;
            boTryModeUseStorage = false;
            nCanShoutMsgLevel = 7;
            boShowMakeItemMsg = false;
            boShutRedMsgShowGMName = false;
            nSayMsgMaxLen = 80;
            dwSayMsgTime = 3 * 1000;
            nSayMsgCount = 2;
            dwDisableSayMsgTime = 60 * 1000;
            nSayRedMsgMaxLen = 255;
            boShowGuildName = true;
            boShowRankLevelName = false;
            boMonSayMsg = false;
            nStartPermission = 0;
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
            boSendOnlineCount = true;
            nSendOnlineCountRate = 10;
            dwclickNpcTime = 1000;  //NPC点击间隔
            dwQueryBagItemsTick = 2 * 60 * 1000;  //包裹刷新间隔
            dwSendOnlineTime = 5 * 60 * 1000;
            dwSaveHumanRcdTime = 10 * 60 * 1000;
            dwHumanFreeDelayTime = 5 * 60 * 1000;
            dwMakeGhostTime = 3 * 60 * 1000;
            dwClearDropOnFloorItemTime = 60 * 60 * 1000;
            dwFloorItemCanPickUpTime = 2 * 60 * 1000;
            boPasswordLockSystem = false;  //是否启用密码保护系统
            boLockDealAction = false;  //是否锁定交易操作
            boLockDropAction = false;  //是否锁定扔物品操作
            boLockGetBackItemAction = false;  //是否锁定取仓库操作
            boLockHumanLogin = false;  //是否锁定走操作
            boLockWalkAction = false;  //是否锁定走操作
            boLockRunAction = false;  //是否锁定跑操作
            boLockHitAction = false;  //是否锁定攻击操作
            boLockSpellAction = false;  //是否锁定魔法操作
            boLockSendMsgAction = false;  //是否锁定发信息操作
            boLockUserItemAction = false;  //是否锁定使用物品操作
            boLockInObModeAction = false;  //锁定时进入隐身状态
            nPasswordErrorCountLock = 3; //输入密码错误超过 指定次数则锁定密码
            boPasswordErrorKick = false; //输入密码错误超过限制则踢下线
            nSendRefMsgRange = 12;
            boDecLampDura = true;
            boHungerSystem = false;
            boHungerDecHP = false;
            boHungerDecPower = false;
            boDiableHumanRun = false;
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
            boSendCurTickCount = true;  //客户端时间
            nMakeMineHitRate = 4; //挖矿命中率
            nMakeMineRate = 12; //挖矿率
            nStoneTypeRate = 120;
            nStoneTypeRateMin = 56;
            nGoldStoneMin = 1;
            nGoldStoneMax = 2;
            nSilverStoneMin = 3;
            nSilverStoneMax = 20;
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
            GlobalVal = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
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
            nAttackPosionRate = 5;
            nAttackPosionTime = 5;
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
            nMagicAttackRage = 8; //魔法锁定范围
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
            nMagTurnUndeadLevel = 50; //圣言怪物等级限制
            nMagTammingLevel = 50; //诱惑之光怪物等级限制
            nMagTammingTargetLevel = 10; //诱惑怪物相差等级机率，此数字越小机率越大；
            nMagTammingHPRate = 100; //成功机率=怪物最高HP 除以 此倍率，此倍率越大诱惑机率越高
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
            sAIHomeMap= "3";
            nAIHomeX= 330;
            nAIHomeY= 330;
            boHPAutoMoveMap = false;//低血回城
            boAutoRepairItem = true;
            boAutoPickUpItem = false;
            boRenewHealth = true;
            btHeroSkillMode = true;
            dwHeroWarrorAttackTime = 1660;
            dwHeroWizardAttackTime = 1880;
            dwHeroTaoistAttackTime = 1880;
            sAIConfigListFileName = @"D:\MirServer\Mir200\Envir\QuestDiary\机器人配置文件列表.txt";
            sHeroAIConfigListFileName = @"D:\MirServer\Mir200\Envir\QuestDiary\机器人配置文件列表.txt";
            boHeroAttackTarget = true;
        }
    }
}