using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace M2Server
{
    public partial class TBaseObject
    {
        public int ObjectId;
        public string m_sMapName;
        public string m_sMapFileName;
        public string m_sCharName;
        /// <summary>
        /// 人物所在座标X
        /// </summary>
        public short m_nCurrX = 0;
        /// <summary>
        /// 人物所在座标Y
        /// </summary>
        public short m_nCurrY = 0;
        /// <summary>
        /// 人物所在方向
        /// </summary>
        public byte m_btDirection = 0;
        /// <summary>
        /// 人物的性别
        /// </summary>
        public byte m_btGender = 0;
        /// <summary>
        /// 人物的头发
        /// </summary>
        public byte m_btHair = 0;
        /// <summary>
        /// 人物的职业
        /// </summary>
        public byte m_btJob = 0;
        /// <summary>
        /// 人物金币数
        /// </summary>
        public int m_nGold = 0;
        public TAbility m_Abil = null;
        /// <summary>
        /// 状态值
        /// </summary>
        public int m_nCharStatus = 0;
        /// <summary>
        /// 回城地图
        /// </summary>
        public string m_sHomeMap;
        /// <summary>
        /// 回城座标X
        /// </summary>
        public short m_nHomeX = 0;
        /// <summary>
        /// 回城座标Y
        /// </summary>
        public short m_nHomeY = 0;
        public bool bo94 = false;
        public bool m_boOnHorse = false;
        public byte m_btHorseType = 0;
        public byte m_btDressEffType = 0;
        /// <summary>
        /// 人物的PK值
        /// </summary>
        public int m_nPkPoint = 0;
        /// <summary>
        /// 允许组队
        /// </summary>
        public bool m_boAllowGroup = false;
        /// <summary>
        /// 允许加入行会
        /// </summary>        
        public bool m_boAllowGuild = false;
        public byte btB2 = 0;
        public int m_nIncHealth = 0;
        public int m_nIncSpell = 0;
        public int m_nIncHealing = 0;
        public int m_nIncHPStoneTime = 0;
        public int m_nIncMPStoneTime = 0;
        /// <summary>
        /// 在行会占争地图中死亡次数
        /// </summary>
        public int m_nFightZoneDieCount = 0;
        public int nC4 = 0;
        public byte btC8 = 0;
        public TNakedAbility m_BonusAbil = null;
        public TNakedAbility m_CurBonusAbil = null;
        public int m_nBonusPoint = 0;
        public int m_nHungerStatus = 0;
        public bool m_boAllowGuildReCall = false;
        public double m_dBodyLuck = 0;
        public int m_nBodyLuckLevel = 0;
        public short m_wGroupRcallTime = 0;
        public bool m_boAllowGroupReCall = false;
        public byte[] m_QuestUnitOpen;
        public byte[] m_QuestUnit;
        public byte[] m_QuestFlag;
        public int m_nCharStatusEx = 0;
        /// <summary>
        /// 怪物经验值
        /// </summary>
        public int m_dwFightExp = 0;
        public TAbility m_WAbil = null;
        public TAddAbility m_AddAbil = null;
        /// <summary>
        /// 可视范围大小
        /// </summary>
        public int m_nViewRange = 0;
        public ushort[] m_wStatusTimeArr;
        public int[] m_dwStatusArrTick = new int[12];
        public short[] m_wStatusArrValue = new short[6];
        public int[] m_dwStatusArrTimeOutTick = new int[6];
        public short m_wAppr = 0;
        /// <summary>
        /// 角色类型
        /// </summary>
        public byte m_btRaceServer = 0;
        /// <summary>
        /// 角色外形
        /// </summary>
        public byte m_btRaceImg = 0;
        /// <summary>
        /// 人物攻击准确度
        /// </summary>
        public byte m_btHitPoint = 0;
        public short m_nHitPlus = 0;
        public short m_nHitDouble = 0;
        /// <summary>
        /// 记忆使用间隔
        /// </summary>
        public int m_dwGroupRcallTick = 0;
        /// <summary>
        /// 记忆全套
        /// </summary>
        public bool m_boRecallSuite = false;
        public bool m_boRaceImg = false;
        public short m_nHealthRecover = 0;
        public short m_nSpellRecover = 0;
        public byte m_btAntiPoison = 0;
        public short m_nPoisonRecover = 0;
        public short m_nAntiMagic = 0;
        /// <summary>
        /// 人物的幸运值
        /// </summary>
        public int m_nLuck = 0;
        public int m_nPerHealth = 0;
        public int m_nPerHealing = 0;
        public int m_nPerSpell = 0;
        public int m_dwIncHealthSpellTick = 0;
        /// <summary>
        /// 中绿毒降HP点数
        /// </summary>
        public byte m_btGreenPoisoningPoint = 0;
        /// <summary>
        /// 人物身上最多可带金币数
        /// </summary>
        public int m_nGoldMax = 0;
        /// <summary>
        /// 敏捷度
        /// </summary>
        public byte m_btSpeedPoint = 0;
        /// <summary>
        /// 权限等级
        /// </summary>
        public byte m_btPermission = 0;
        public short m_nHitSpeed = 0;
        public byte m_btLifeAttrib = 0;
        public byte m_btCoolEye = 0;
        public TBaseObject m_GroupOwner = null;
        /// <summary>
        /// 组成员
        /// </summary>
        public IList<TPlayObject> m_GroupMembers = null;
        /// <summary>
        /// 允许私聊
        /// </summary>
        public bool m_boHearWhisper = false;
        /// <summary>
        /// 允许群聊
        /// </summary>
        public bool m_boBanShout = false;
        /// <summary>
        /// 拒绝行会聊天
        /// </summary>
        public bool m_boBanGuildChat = false;
        /// <summary>
        /// 是不允许交易
        /// </summary>
        public bool m_boAllowDeal = false;
        /// <summary>
        /// 禁止私聊人员列表
        /// </summary>
        public IList<string> m_BlockWhisperList = null;
        public int m_dwShoutMsgTick = 0;
        /// <summary>
        /// 是否被召唤(主人)
        /// </summary>
        public TBaseObject m_Master = null;
       /// <summary>
       /// 怪物叛变时间
       /// </summary>
        public int m_dwMasterRoyaltyTick = 0;
        public int m_dwMasterTick = 0;
        /// <summary>
        /// 杀怪计数
        /// </summary>
        public int m_nKillMonCount = 0;
        /// <summary>
        /// 宝宝等级(1-7)
        /// </summary>
        public byte m_btSlaveExpLevel = 0;
        /// <summary>
        /// 召唤等级
        /// </summary>
        public byte m_btSlaveMakeLevel = 0;
        /// <summary>
        /// 下属列表
        /// </summary>        
        public IList<TBaseObject> m_SlaveList = null;
        public byte bt2A0 = 0;
        /// <summary>
        /// 宝宝攻击状态(休息/攻击)
        /// </summary>
        public bool m_boSlaveRelax = false;
        /// <summary>
        /// 下属攻击状态
        /// </summary>
        public byte m_btAttatckMode = 0;
        /// <summary>
        /// 人物名字的颜色
        /// </summary>        
        public byte m_btNameColor = 0;
        /// <summary>
        /// 亮度
        /// </summary>
        public int m_nLight = 0;
        /// <summary>
        /// 行会占争范围
        /// </summary>
        public bool m_boGuildWarArea = false;
        /// <summary>
        /// 所属城堡
        /// </summary>
        public TUserCastle m_Castle = null;
        public bool bo2B0 = false;
        public int m_dw2B4Tick = 0;
        /// <summary>
        /// 无敌模式
        /// </summary>
        public bool m_boSuperMan = false;
        public bool bo2B9 = false;
        public bool bo2BA = false;
        public bool m_boAnimal = false;
        public bool m_boNoItem = false;
        public bool m_boFixedHideMode = false;
        public bool m_boStickMode = false;
        public bool bo2BF = false;
        public bool m_boNoAttackMode = false;
        public bool m_boNoTame = false;
        public bool m_boSkeleton = false;
        public int m_nMeatQuality = 0;
        public int m_nBodyLeathery = 0;
        public bool m_boHolySeize = false;
        public int m_dwHolySeizeTick = 0;
        public int m_dwHolySeizeInterval = 0;
        public bool m_boCrazyMode = false;
        public int m_dwCrazyModeTick = 0;
        public int m_dwCrazyModeInterval = 0;
        public bool m_boShowHP = false;
        public int m_dwShowHPTick = 0;
        // 0x2E8  心灵启示检查时间(Dword)
        public int m_dwShowHPInterval = 0;
        // 0x2EC  心灵启示有效时长(Dword)
        public bool bo2F0 = false;
        public int m_dwDupObjTick = 0;
        public TEnvirnoment m_PEnvir = null;
        public bool m_boGhost = false;
        public int m_dwGhostTick = 0;
        public bool m_boDeath = false;
        public int m_dwDeathTick = 0;
        /// <summary>
        /// 怪物所拿的武器
        /// </summary>
        public byte m_btMonsterWeapon = 0;
        public int m_dwStruckTick = 0;
        public bool m_boWantRefMsg = false;
        public bool m_boAddtoMapSuccess = false;
        public bool m_bo316 = false;
        public bool m_boDealing = false;
        /// <summary>
        /// 交易最后操作时间
        /// </summary>
        public int m_DealLastTick = 0;
        public TBaseObject m_DealCreat = null;
        public TGuild m_MyGuild = null;
        public int m_nGuildRankNo = 0;
        public string m_sGuildRankName = string.Empty;
        public string m_sScriptLable = string.Empty;
        public byte m_btAttackSkillCount = 0;
        public byte m_btAttackSkillPointCount = 0;
        public bool m_boMission = false;
        public short m_nMissionX = 0;
        public short m_nMissionY = 0;
        public bool m_boHideMode = false;
        // 0x344  隐身戒指(Byte)
        public bool m_boStoneMode = false;
        public bool m_boCoolEye = false;
        // 0x346  //是否可以看到隐身人物
        public bool m_boUserUnLockDurg = false;
        // 0x347  //是否用了神水
        public bool m_boTransparent = false;
        // 0x348  //魔法隐身了
        public bool m_boAdminMode = false;
        // 0x349  管理模式(Byte)
        public bool m_boObMode = false;
        // 0x34A  隐身模式(Byte)
        public bool m_boTeleport = false;
        // 0x34B  传送戒指(Byte)
        public bool m_boParalysis = false;
        // 0x34C  麻痹戒指(Byte)
        public bool m_boUnParalysis = false;
        public bool m_boRevival = false;
        // 0x34D  复活戒指(Byte)
        public bool m_boUnRevival = false;
        // 防复活
        public int m_dwRevivalTick = 0;
        // 0x350  复活戒指使用间隔计数(Dword)
        public bool m_boFlameRing = false;
        // 0x354  火焰戒指(Byte)
        public bool m_boRecoveryRing = false;
        // 0x355  治愈戒指(Byte)
        public bool m_boAngryRing = false;
        // 0x356  未知戒指(Byte)
        public bool m_boMagicShield = false;
        // 0x357  护身戒指(Byte)
        public bool m_boUnMagicShield = false;
        // 防护身
        public bool m_boMuscleRing = false;
        // 0x358  活力戒指(Byte)
        public bool m_boFastTrain = false;
        // 0x359  技巧项链(Byte)
        public bool m_boProbeNecklace = false;
        // 0x35A  探测项链(Byte)
        public bool m_boGuildMove = false;
        // 行会传送
        public bool m_boSupermanItem = false;
        public bool m_bopirit = false;
        // 祈祷
        public bool m_boNoDropItem = false;
        public bool m_boNoDropUseItem = false;
        public bool m_boExpItem = false;
        public bool m_boPowerItem = false;
        public int m_rExpItem = 0;
        public int m_rPowerItem = 0;
        public int m_dwPKDieLostExp = 0;
        // PK 死亡掉经验，不够经验就掉等级
        public int m_nPKDieLostLevel = 0;
        // PK 死亡掉等级
        public bool m_boAbilSeeHealGauge = false;
        // 0x35B  //心灵启示
        public bool m_boAbilMagBubbleDefence = false;
        // 0x35C  //魔法盾
        public byte m_btMagBubbleDefenceLevel = 0;
        public int m_dwSearchTime = 0;
        public int m_dwSearchTick = 0;
        /// <summary>
        /// 上次运行时间
        /// </summary>
        public int m_dwRunTick = 0;
        public int m_nRunTime = 0;
        public int m_nHealthTick = 0;
        public int m_nSpellTick = 0;
        public TBaseObject m_TargetCret = null;
        public int m_dwTargetFocusTick = 0;
        /// <summary>
        /// 人物被对方杀害时对方指针
        /// </summary>
        public TBaseObject m_LastHiter = null;
        public int m_LastHiterTick = 0;
        public TBaseObject m_ExpHitter = null;
        public int m_ExpHitterTick = 0;
        /// <summary>
        /// 传送戒指使用间隔
        /// </summary>
        public int m_dwTeleportTick = 0;
        /// <summary>
        /// 探测项链使用间隔
        /// </summary>
        public int m_dwProbeTick = 0;
        public int m_dwMapMoveTick = 0;
        /// <summary>
        /// 人物攻击变色标志
        /// </summary>
        public bool m_boPKFlag = false;
        /// <summary>
        /// 人物攻击变色时间长度
        /// </summary>
        public int m_dwPKTick = 0;
        public int m_nMoXieSuite = 0;
        // 0x3A4  魔血一套(Dword)
        public int m_nHongMoSuite = 0;
        // 0x3A8 虹魔一套(Dword)
        public double m_db3B0 = 0;
        /// <summary>
        /// 中毒处理间隔时间
        /// </summary>
        public int m_dwPoisoningTick = 0;
        /// <summary>
        /// 减PK值时间
        /// </summary>
        public int m_dwDecPkPointTick = 0;
        public int m_DecLightItemDrugTick = 0;
        public int m_dwVerifyTick = 0;
        public int m_dwCheckRoyaltyTick = 0;
        public int m_dwDecHungerPointTick = 0;
        public int m_dwHPMPTick = 0;
        public IList<SendMessage> m_MsgList = null;
        public IList<TBaseObject> m_VisibleHumanList = null;
        public IList<TVisibleMapItem> m_VisibleItems = null;
        public IList<TEvent> m_VisibleEvents = null;
        public int m_SendRefMsgTick = 0;
        /// <summary>
        /// 是否在开行会战
        /// </summary>
        public bool m_boInFreePKArea = false;
        public int m_dwHitTick = 0;
        public int m_dwWalkTick = 0;
        public int m_dwSearchEnemyTick = 0;
        public bool m_boNameColorChanged = false;
        /// <summary>
        /// 是否在可视范围内有人物,及宝宝
        /// </summary>
        public bool m_boIsVisibleActive = false;
        /// <summary>
        /// 当前处理数量
        /// </summary>
        public short m_nProcessRunCount = 0;
        public IList<TVisibleBaseObject> m_VisibleActors = null;
        /// <summary>
        /// 人物背包(Dword)数量
        /// </summary>
        public IList<TUserItem> m_ItemList = null;
        /// <summary>
        /// 交易列表
        /// </summary>
        public IList<TUserItem> m_DealItemList = null;
        /// <summary>
        /// 交易的金币数量
        /// </summary>
        public int m_nDealGolds = 0;
        /// <summary>
        /// 确认交易标志
        /// </summary>
        public bool m_boDealOK = false;
        /// <summary>
        /// 技能表
        /// </summary>
        public IList<TUserMagic> m_MagicList = null;
        public TUserItem[] m_UseItems;
        public IList<TMonSayMsg> m_SayMsgList = null;
        public IList<TUserItem> m_StorageItemList = null;
        public int m_nWalkSpeed = 0;
        public int m_nWalkStep = 0;
        public int m_nWalkCount = 0;
        public int m_dwWalkWait = 0;
        public int m_dwWalkWaitTick = 0;
        public bool m_boWalkWaitLocked = false;
        public int m_nNextHitTime = 0;
        public TUserMagic m_MagicOneSwordSkill = null;
        public TUserMagic m_MagicPowerHitSkill = null;
        /// <summary>
        /// 刺杀剑法
        /// </summary>
        public TUserMagic m_MagicErgumSkill = null;
        /// <summary>
        /// 半月弯刀
        /// </summary>
        public TUserMagic m_MagicBanwolSkill = null;
        public TUserMagic m_MagicRedBanwolSkill = null;
        public TUserMagic m_MagicFireSwordSkill = null;
        public TUserMagic m_MagicCrsSkill = null;
        public TUserMagic m_Magic41Skill = null;
        public TUserMagic m_MagicTwnHitSkill = null;
        public TUserMagic m_Magic43Skill = null;
        public bool m_boPowerHit = false;
        public bool m_boUseThrusting = false;
        public bool m_boUseHalfMoon = false;
        public bool m_boRedUseHalfMoon = false;
        public bool m_boFireHitSkill = false;
        public bool m_boCrsHitkill = false;
        public bool m_bo41kill = false;
        public bool m_boTwinHitSkill = false;
        public bool m_bo43kill = false;
        public int m_dwLatestFireHitTick = 0;
        public int m_dwDoMotaeboTick = 0;
        public int m_dwLatestTwinHitTick = 0;
        public bool m_boDenyRefStatus = false;
        // 是否刷新在地图上信息；
        public bool m_boAddToMaped = false;
        // 是否增加地图计数
        public bool m_boDelFormMaped = false;
        // 是否从地图中删除计数
        public bool m_boAutoChangeColor = false;
        public int m_dwAutoChangeColorTick = 0;
        public int m_nAutoChangeIdx = 0;
        public bool m_boFixColor = false;
        // 固定颜色
        public int m_nFixColorIdx = 0;
        public int m_nFixStatus = 0;
        public bool m_boFastParalysis = false;
        // 快速麻痹，受攻击后麻痹立即消失
        public bool m_boSmashSet = false;
        public bool m_boHwanDevilSet = false;
        public bool m_boPuritySet = false;
        public bool m_boMundaneSet = false;
        public bool m_boNokChiSet = false;
        public bool m_boTaoBuSet = false;
        public bool m_boFiveStringSet = false;
        public bool m_boOffLineFlag = false;
        // 挂机
        public string m_sOffLineLeaveword = string.Empty;
        // 挂机字符
        public int m_dwKickOffLineTick = 0;
        public bool m_boNastyMode = false;
        public int m_nAutoAddHPMPMode = 0;
        // 气血石
        public int m_dwCheckHPMPTick = 0;

        public TBaseObject()
        {
            ObjectId = HUtil32.Sequence();
            m_boGhost = false;
            m_dwGhostTick = 0;
            m_boDeath = false;
            m_dwDeathTick = 0;
            m_SendRefMsgTick = HUtil32.GetTickCount();
            m_btDirection = 4;
            m_btRaceServer = grobal2.RC_ANIMAL;
            m_btRaceImg = 0;
            m_btHair = 0;
            m_btJob = M2Share.jWarr;
            m_nGold = 0;
            m_wAppr = 0;
            bo2B9 = true;
            m_nViewRange = 5;
            m_sHomeMap = "0";
            bo94 = false;
            m_btPermission = 0;
            m_nLight = 0;
            m_btNameColor = 255;
            m_nHitPlus = 0;
            m_nHitDouble = 0;
            m_dBodyLuck = 0;
            m_wGroupRcallTime = 0;
            m_dwGroupRcallTick = HUtil32.GetTickCount();
            m_boRecallSuite = false;
            m_boRaceImg = false;
            bo2BA = false;
            m_boAbilSeeHealGauge = false;
            m_boPowerHit = false;
            m_boUseThrusting = false;
            m_boUseHalfMoon = false;
            m_boRedUseHalfMoon = false;
            m_boFireHitSkill = false;
            m_boTwinHitSkill = false;
            m_btHitPoint = 5;
            m_btSpeedPoint = 15;
            m_nHitSpeed = 0;
            m_btLifeAttrib = 0;
            m_btAntiPoison = 0;
            m_nPoisonRecover = 0;
            m_nHealthRecover = 0;
            m_nSpellRecover = 0;
            m_nAntiMagic = 0;
            m_nLuck = 0;
            m_nIncSpell = 0;
            m_nIncHealth = 0;
            m_nIncHealing = 0;
            m_nIncHPStoneTime = HUtil32.GetTickCount();
            m_nIncMPStoneTime = HUtil32.GetTickCount();
            m_nPerHealth = 5;
            m_nPerHealing = 5;
            m_nPerSpell = 5;
            m_dwIncHealthSpellTick = HUtil32.GetTickCount();
            m_btGreenPoisoningPoint = 0;
            m_nFightZoneDieCount = 0;
            m_nGoldMax = M2Share.g_Config.nHumanMaxGold;
            m_nCharStatus = 0;
            m_nCharStatusEx = 0;
            m_wStatusTimeArr = new ushort[12];// FillChar(m_wStatusTimeArr, sizeof(grobal2.short), '\0');
            m_BonusAbil = new TNakedAbility();// FillChar(m_BonusAbil, sizeof(TNakedAbility), '\0');
            m_CurBonusAbil = new TNakedAbility();// FillChar(m_CurBonusAbil, sizeof(TNakedAbility), '\0');
            m_wStatusArrValue = new short[6];// FillChar(m_wStatusArrValue, sizeof(m_wStatusArrValue), 0);
            m_dwStatusArrTimeOutTick = new int[6];// FillChar(m_dwStatusArrTimeOutTick, sizeof(m_dwStatusArrTimeOutTick), '\0');
            m_boAllowGroup = false;
            m_boAllowGuild = false;
            btB2 = 0;
            m_btAttatckMode = 0;
            m_boInFreePKArea = false;
            m_boGuildWarArea = false;
            bo2B0 = false;
            m_boSuperMan = false;
            m_boSkeleton = false;
            bo2BF = false;
            m_boHolySeize = false;
            m_boCrazyMode = false;
            m_boShowHP = false;
            bo2F0 = false;
            m_boAnimal = false;
            m_boNoItem = false;
            m_nBodyLeathery = 50;
            m_boFixedHideMode = false;
            m_boStickMode = false;
            m_boNoAttackMode = false;
            m_boNoTame = false;
            m_boPKFlag = false;
            m_nMoXieSuite = 0;
            m_nHongMoSuite = 0;
            m_db3B0 = 0;
            m_AddAbil = new TAddAbility();//FillChar(m_AddAbil, sizeof(TAddAbility), '\0');
            m_MsgList = new List<SendMessage>();
            m_VisibleHumanList = new List<TBaseObject>();
            m_VisibleActors = new List<TVisibleBaseObject>();
            m_VisibleItems = new List<TVisibleMapItem>();
            m_VisibleEvents = new List<TEvent>();
            m_ItemList = new List<TUserItem>();
            m_DealItemList = new List<TUserItem>();
            m_boIsVisibleActive = false;
            m_nProcessRunCount = 0;
            m_nDealGolds = 0;
            m_MagicList = new List<TUserMagic>();
            m_StorageItemList = new List<TUserItem>();
            //FillChar(m_UseItems, sizeof(grobal2.TUserItem), 0);
            m_UseItems = new TUserItem[13];
            m_MagicOneSwordSkill = null;
            m_MagicPowerHitSkill = null;
            m_MagicErgumSkill = null;
            m_MagicBanwolSkill = null;
            m_MagicRedBanwolSkill = null;
            m_MagicFireSwordSkill = null;
            m_MagicCrsSkill = null;
            m_Magic41Skill = null;
            m_MagicTwnHitSkill = null;
            m_Magic43Skill = null;
            m_GroupOwner = null;
            m_Castle = null;
            m_Master = null;
            m_nKillMonCount = 0;
            m_btSlaveExpLevel = 0;
            bt2A0 = 0;
            m_GroupMembers = new List<TPlayObject>();
            m_boHearWhisper = true;
            m_boBanShout = true;
            m_boBanGuildChat = true;
            m_boAllowDeal = true;
            m_boAllowGroupReCall = false;
            m_BlockWhisperList = new List<string>();
            m_SlaveList = new  List<TBaseObject>();
            m_WAbil = new TAbility();// FillChar(m_WAbil, sizeof(TAbility), '\0');
            m_QuestUnitOpen = new byte[128];//FillChar(m_QuestUnitOpen, sizeof(grobal2.byte), '\0');
            m_QuestUnit = new byte[128];// FillChar(m_QuestUnit, sizeof(grobal2.byte), '\0');
            m_QuestFlag = new byte[128];
            m_Abil = new TAbility
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
            m_boWantRefMsg = false;
            m_boDealing = false;
            m_DealCreat = null;
            m_MyGuild = null;
            m_nGuildRankNo = 0;
            m_sGuildRankName = "";
            m_sScriptLable = "";
            m_boMission = false;
            m_boHideMode = false;
            m_boStoneMode = false;
            m_boCoolEye = false;
            m_boUserUnLockDurg = false;
            m_boTransparent = false;
            m_boAdminMode = false;
            m_boObMode = false;
            m_dwRunTick = HUtil32.GetTickCount() + M2Share.RandomNumber.Random(1500);
            m_nRunTime = 250;
            m_dwSearchTime = M2Share.RandomNumber.Random(2000) + 2000;
            m_dwSearchTick = HUtil32.GetTickCount();
            m_dwDecPkPointTick = HUtil32.GetTickCount();
            m_DecLightItemDrugTick = HUtil32.GetTickCount();
            m_dwPoisoningTick = HUtil32.GetTickCount();
            m_dwVerifyTick = HUtil32.GetTickCount();
            m_dwCheckRoyaltyTick = HUtil32.GetTickCount();
            m_dwDecHungerPointTick = HUtil32.GetTickCount();
            m_dwHPMPTick = HUtil32.GetTickCount();
            m_dwShoutMsgTick = 0;
            m_dwTeleportTick = 0;
            m_dwProbeTick = 0;
            m_dwMapMoveTick = HUtil32.GetTickCount();
            m_dwMasterTick = 0;
            m_nWalkSpeed = 1400;
            m_nNextHitTime = 2000;
            m_nWalkCount = 0;
            m_dwWalkWaitTick = HUtil32.GetTickCount();
            m_boWalkWaitLocked = false;
            m_nHealthTick = 0;
            m_nSpellTick = 0;
            m_TargetCret = null;
            m_LastHiter = null;
            m_ExpHitter = null;
            m_SayMsgList = null;
            m_boDenyRefStatus = false;
            m_btHorseType = 0;
            m_btDressEffType = 0;
            m_dwPKDieLostExp = 0;
            m_nPKDieLostLevel = 0;
            m_boAddToMaped = true;
            m_boAutoChangeColor = false;
            m_dwAutoChangeColorTick = HUtil32.GetTickCount();
            m_nAutoChangeIdx = 0;
            m_boFixColor = false;
            m_nFixColorIdx = 0;
            m_nFixStatus = -1;
            m_boFastParalysis = false;
            m_boNastyMode = false;
            M2Share.ObjectSystem.Add(ObjectId, this);
        }

        public void ChangePKStatus(bool boWarFlag)
        {
            if (m_boInFreePKArea != boWarFlag)
            {
                m_boInFreePKArea = boWarFlag;
                m_boNameColorChanged = true;
            }
        }

        public bool GetDropPosition(int nOrgX, int nOrgY, int nRange, ref int nDX, ref int nDY)
        {
            var result = false;
            var nItemCount = 0;
            var n24 = 999;
            var n28 = 0;
            var n2C = 0;
            for (var i = 1; i <= nRange; i++)
            {
                for (var ii = -i; ii <= i; ii++)
                {
                    for (var iii = -i; iii <= i; iii++)
                    {
                        nDX = nOrgX + iii;
                        nDY = nOrgY + ii;
                        if (m_PEnvir.GetItemEx(nDX, nDY, ref nItemCount) == null)
                        {
                            if (m_PEnvir.bo2C)
                            {
                                result = true;
                                break;
                            }
                        }
                        else
                        {
                            if (m_PEnvir.bo2C && n24 > nItemCount)
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

        public bool DropItemDown(TUserItem UserItem, int nScatterRange, bool boDieDrop, TBaseObject ItemOfCreat, TBaseObject DropCreat)
        {
            bool result = false;
            int dx = 0;
            int dy = 0;
            int idura;
            TMapItem MapItem;
            TMapItem pr;
            string logcap;
            TItem StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
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
                    UserItem.Dura = (short)idura;
                }
                MapItem = new TMapItem
                {
                    UserItem = UserItem,
                    Name = ItmUnit.GetItemName(UserItem),// 取自定义物品名称
                    Looks = StdItem.Looks
                };
                if (StdItem.StdMode == 45)
                {
                    MapItem.Looks = (short)M2Share.GetRandomLook(MapItem.Looks, StdItem.Shape);
                }
                MapItem.AniCount = StdItem.AniCount;
                MapItem.Reserved = 0;
                MapItem.Count = 1;
                MapItem.OfBaseObject = ItemOfCreat;
                MapItem.dwCanPickUpTick = HUtil32.GetTickCount();
                MapItem.DropBaseObject = DropCreat;
                GetDropPosition(m_nCurrX, m_nCurrY, nScatterRange, ref dx, ref dy);
                pr = (TMapItem)m_PEnvir.AddToMap(dx, dy, grobal2.OS_ITEMOBJECT, MapItem);
                if (pr == MapItem)
                {
                    SendRefMsg(grobal2.RM_ITEMSHOW, MapItem.Looks, MapItem.Id, dx, dy, MapItem.Name);
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
                            M2Share.AddGameDataLog(logcap + "\t" + m_sMapName + "\t" + m_nCurrX.ToString() + "\t" + m_nCurrY.ToString() + "\t" + m_sCharName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex.ToString() + "\t" + HUtil32.BoolToIntStr(m_btRaceServer == grobal2.RC_PLAYOBJECT) + "\t" + '0');
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
            if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
            {
                SendUpdateMsg(this, grobal2.RM_GOLDCHANGED, 0, 0, 0, 0, "");
            }
        }

        public void GameGoldChanged()
        {
            if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
            {
                SendUpdateMsg(this, grobal2.RM_GAMEGOLDCHANGED, 0, 0, 0, 0, "");
            }
        }

        public void RecalcLevelAbilitys()
        {
            int n;
            double nLevel = m_Abil.Level;
            switch (m_btJob)
            {
                case M2Share.jTaos:
                    m_Abil.MaxHP = (short)HUtil32._MIN(short.MaxValue, 14 + HUtil32.Round((nLevel / M2Share.g_Config.nLevelValueOfTaosHP + M2Share.g_Config.nLevelValueOfTaosHPRate) * nLevel));
                    m_Abil.MaxMP = (short)HUtil32._MIN(short.MaxValue, 13 + HUtil32.Round(nLevel / M2Share.g_Config.nLevelValueOfTaosMP * 2.2 * nLevel));
                    m_Abil.MaxWeight = (short)(50 + HUtil32.Round(nLevel / 4 * nLevel));
                    m_Abil.MaxWearWeight = (short)(15 + HUtil32.Round(nLevel / 50 * nLevel));
                    m_Abil.MaxHandWeight = (short)(12 + HUtil32.Round(nLevel / 42 * nLevel));
                    n = (int)(nLevel / 7);
                    m_Abil.DC = HUtil32.MakeLong(HUtil32._MAX(n - 1, 0), HUtil32._MAX(1, n));
                    m_Abil.MC = 0;
                    m_Abil.SC = HUtil32.MakeLong(HUtil32._MAX(n - 1, 0), HUtil32._MAX(1, n));
                    m_Abil.AC = 0;
                    n = HUtil32.Round(nLevel / 6);
                    m_Abil.MAC = HUtil32.MakeLong(n / 2, n + 1);
                    break;
                case M2Share.jWizard:
                    m_Abil.MaxHP = (short)HUtil32._MIN(short.MaxValue, 14 + HUtil32.Round((nLevel / M2Share.g_Config.nLevelValueOfWizardHP + M2Share.g_Config.nLevelValueOfWizardHPRate) * nLevel));
                    m_Abil.MaxMP = (short)HUtil32._MIN(short.MaxValue, 13 + HUtil32.Round((nLevel / 5 + 2) * 2.2 * nLevel));
                    m_Abil.MaxWeight = (short)(50 + HUtil32.Round(nLevel / 5 * nLevel));
                    m_Abil.MaxWearWeight = (short)HUtil32._MIN(short.MaxValue, 15 + HUtil32.Round(nLevel / 100 * nLevel));
                    m_Abil.MaxHandWeight = (short)(12 + HUtil32.Round(nLevel / 90 * nLevel));
                    n = (int)(nLevel / 7);
                    m_Abil.DC = HUtil32.MakeLong(HUtil32._MAX(n - 1, 0), HUtil32._MAX(1, n));
                    m_Abil.MC = HUtil32.MakeLong(HUtil32._MAX(n - 1, 0), HUtil32._MAX(1, n));
                    m_Abil.SC = 0;
                    m_Abil.AC = 0;
                    m_Abil.MAC = 0;
                    break;
                case M2Share.jWarr:
                    m_Abil.MaxHP = (short)HUtil32._MIN(short.MaxValue, 14 + HUtil32.Round((nLevel / M2Share.g_Config.nLevelValueOfWarrHP + M2Share.g_Config.nLevelValueOfWarrHPRate + nLevel / 20) * nLevel));
                    m_Abil.MaxMP = (short)HUtil32._MIN(short.MaxValue, 11 + HUtil32.Round(nLevel * 3.5));
                    m_Abil.MaxWeight = (short)(50 + HUtil32.Round(nLevel / 3 * nLevel));
                    m_Abil.MaxWearWeight = (short)(15 + HUtil32.Round(nLevel / 20 * nLevel));
                    m_Abil.MaxHandWeight = (short)(12 + HUtil32.Round(nLevel / 13 * nLevel));
                    m_Abil.DC = HUtil32.MakeLong(HUtil32._MAX((int)(nLevel / 5) - 1, 1), HUtil32._MAX(1, (int)(nLevel / 5)));
                    m_Abil.SC = 0;
                    m_Abil.MC = 0;
                    m_Abil.AC = HUtil32.MakeLong(0, nLevel / 7);
                    m_Abil.MAC = 0;
                    break;
            }
            if (m_Abil.HP > m_Abil.MaxHP)
            {
                m_Abil.HP = m_Abil.MaxHP;
            }
            if (m_Abil.MP > m_Abil.MaxMP)
            {
                m_Abil.MP = m_Abil.MaxMP;
            }
        }

        public void HasLevelUp(int nLevel)
        {
            m_Abil.MaxExp = GetLevelExp(m_Abil.Level);
            RecalcLevelAbilitys();
            RecalcAbilitys();
            SendMsg(this, grobal2.RM_LEVELUP, 0, m_Abil.Exp, 0, 0, "");
#if FOR_ABIL_POINT
            // 4/16老 何磐 利侩
            if (prevlevel + 1 == Abil.Level)
            {
                BonusPoint = BonusPoint + GetBonusPoint(Job, Abil.Level);
                SendMsg(this, grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
            }
            else
            {
                if (prevlevel != Abil.Level)
                {
                    // 焊呈胶 器牢飘甫 贸澜何磐 促矫 拌魂茄促.
                    BonusPoint = GetLevelBonusSum(Job, Abil.Level);
                    FillChar(BonusAbil, sizeof(TNakedAbility), '\0');
                    FillChar(CurBonusAbil, sizeof(TNakedAbility), '\0');
                    RecalcLevelAbilitys();
                    SendMsg(this, grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                }
            }
#endif
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(this as TPlayObject, "@LevelUp", false);
            }
        }

        public bool WalkTo(byte btDir, bool boFlag)
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
                nOX = m_nCurrX;
                nOY = m_nCurrY;
                m_btDirection = btDir;
                nNX = 0;
                nNY = 0;
                switch (btDir)
                {
                    case grobal2.DR_UP:
                        nNX = m_nCurrX;
                        nNY = (short)(m_nCurrY - 1);
                        break;
                    case grobal2.DR_UPRIGHT:
                        nNX = (short)(m_nCurrX + 1);
                        nNY = (short)(m_nCurrY - 1);
                        break;
                    case grobal2.DR_RIGHT:
                        nNX = (short)(m_nCurrX + 1);
                        nNY = m_nCurrY;
                        break;
                    case grobal2.DR_DOWNRIGHT:
                        nNX = (short)(m_nCurrX + 1);
                        nNY = (short)(m_nCurrY + 1);
                        break;
                    case grobal2.DR_DOWN:
                        nNX = m_nCurrX;
                        nNY = (short)(m_nCurrY + 1);
                        break;
                    case grobal2.DR_DOWNLEFT:
                        nNX = (short)(m_nCurrX - 1);
                        nNY = (short)(m_nCurrY + 1);
                        break;
                    case grobal2.DR_LEFT:
                        nNX = (short)(m_nCurrX - 1);
                        nNY = m_nCurrY;
                        break;
                    case grobal2.DR_UPLEFT:
                        nNX = (short)(m_nCurrX - 1);
                        nNY = (short)(m_nCurrY - 1);
                        break;
                }
                if (nNX >= 0 && m_PEnvir.wWidth - 1 >= nNX && nNY >= 0 && m_PEnvir.wHeight - 1 >= nNY)
                {
                    bo29 = true;
                    if (bo2BA && !m_PEnvir.CanSafeWalk(nNX, nNY))
                    {
                        bo29 = false;
                    }
                    if (m_Master != null)
                    {
                        m_Master.m_PEnvir.GetNextPosition(m_Master.m_nCurrX, m_Master.m_nCurrY, m_Master.m_btDirection, 1, ref n20, ref n24);
                        if (nNX == n20 && nNY == n24)
                        {
                            bo29 = false;
                        }
                    }
                    if (bo29)
                    {
                        if (m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, nNX, nNY, boFlag) > 0)
                        {
                            m_nCurrX = nNX;
                            m_nCurrY = nNY;
                        }
                    }
                }
                if (m_nCurrX != nOX || m_nCurrY != nOY)
                {
                    if (Walk(grobal2.RM_WALK))
                    {
                        if (m_boTransparent && m_boHideMode)
                        {
                            m_wStatusTimeArr[grobal2.STATE_TRANSPARENT] = 1;
                        }
                        result = true;
                    }
                    else
                    {
                        m_PEnvir.DeleteFromMap(m_nCurrX, m_nCurrY, grobal2.OS_MOVINGOBJECT, this);
                        m_nCurrX = nOX;
                        m_nCurrY = nOY;
                        m_PEnvir.AddToMap(m_nCurrX, m_nCurrY, grobal2.OS_MOVINGOBJECT, this);
                    }
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
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
            for (int i = 0; i < m_GroupOwner.m_GroupMembers.Count; i++)
            {
                if (m_GroupOwner.m_GroupMembers[i] == target)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public int PKLevel()
        {
            return m_nPkPoint / 100;
        }

        public void HealthSpellChanged()
        {
            if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
            {
                SendUpdateMsg(this, grobal2.RM_HEALTHSPELLCHANGED, 0, 0, 0, 0, "");
            }
            if (m_boShowHP)
            {
                SendRefMsg(grobal2.RM_HEALTHSPELLCHANGED, 0, 0, 0, 0, "");
            }
        }

        public int CalcGetExp(int nLevel, int nExp)
        {
            int result;
            if (M2Share.g_Config.boHighLevelKillMonFixExp || (m_Abil.Level < (nLevel + 10)))
            {
                result = nExp;
            }
            else
            {
                result = nExp - HUtil32.Round(nExp / 15 * (m_Abil.Level - (nLevel + 10)));
            }
            if (result <= 0)
            {
                result = 1;
            }
            return result;
        }

        public void RefNameColor()
        {
            SendRefMsg(grobal2.RM_CHANGENAMECOLOR, 0, 0, 0, 0, "");
        }

        public int GainSlaveExp_GetUpKillCount()
        {
            int tCount;
            if (m_btSlaveExpLevel < grobal2.SLAVEMAXLEVEL - 2)
            {
                tCount = M2Share.g_Config.MonUpLvNeedKillCount[m_btSlaveExpLevel];
            }
            else
            {
                tCount = 0;
            }
            return (m_Abil.Level * M2Share.g_Config.nMonUpLvRate) - m_Abil.Level + M2Share.g_Config.nMonUpLvNeedKillBase + tCount;
        }

        public void GainSlaveExp(int nLevel)
        {
            m_nKillMonCount += nLevel;
            if (GainSlaveExp_GetUpKillCount() < m_nKillMonCount)
            {
                m_nKillMonCount -= GainSlaveExp_GetUpKillCount();
                if (m_btSlaveExpLevel < (m_btSlaveMakeLevel * 2 + 1))
                {
                    m_btSlaveExpLevel++;
                    RecalcAbilitys();
                    RefNameColor();
                }
            }
        }

        public bool DropGoldDown(int nGold, bool boFalg, TBaseObject GoldOfCreat, TBaseObject DropGoldCreat)
        {
            bool result = false;
            int nX = 0;
            int nY = 0;
            string s20;
            int DropWide = HUtil32._MIN(M2Share.g_Config.nDropItemRage, 7);
            TMapItem MapItem = new TMapItem
            {
                Name = grobal2.sSTRING_GOLDNAME,
                Count = nGold,
                Looks = M2Share.GetGoldShape(nGold),
                OfBaseObject = GoldOfCreat,
                dwCanPickUpTick = HUtil32.GetTickCount(),
                DropBaseObject = DropGoldCreat
            };
            GetDropPosition(m_nCurrX, m_nCurrY, 3, ref nX, ref nY);
            TMapItem MapItemA = (TMapItem) m_PEnvir.AddToMap(nX, nY, grobal2.OS_ITEMOBJECT, MapItem);
            if (MapItemA != null)
            {
                if (MapItemA != MapItem)
                {
                    MapItem = null;
                    MapItem = MapItemA;
                }

                SendRefMsg(grobal2.RM_ITEMSHOW, MapItem.Looks, MapItem.Id, nX, nY, MapItem.Name);
                if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
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
                        M2Share.AddGameDataLog(s20 + "\t" + m_sMapName + "\t" + m_nCurrX.ToString() + "\t" +
                                               m_nCurrY.ToString() + "\t" + m_sCharName + "\t" +
                                               grobal2.sSTRING_GOLDNAME + "\t" + nGold.ToString() + "\t" +
                                               HUtil32.BoolToIntStr(m_btRaceServer == grobal2.RC_PLAYOBJECT) + "\t" +
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
            m_boGuildWarArea = false;
            if ((cert1.m_MyGuild == null) || (cert2.m_MyGuild == null))
            {
                return result;
            }
            if (cert1.InSafeArea() || cert2.InSafeArea())
            {
                return result;
            }
            if (cert1.m_MyGuild.GuildWarList.Count <= 0)
            {
                return result;
            }
            m_boGuildWarArea = true;
            if (cert1.m_MyGuild.IsWarGuild(cert2.m_MyGuild) && cert2.m_MyGuild.IsWarGuild(cert1.m_MyGuild))
            {
                result = 2;
            }
            if (cert1.m_MyGuild == cert2.m_MyGuild)
            {
                result = 1;
            }
            if (cert1.m_MyGuild.IsAllyGuild(cert2.m_MyGuild) && cert2.m_MyGuild.IsAllyGuild(cert1.m_MyGuild))
            {
                result = 3;
            }
            return result;
        }

        public void IncPkPoint(int nPoint)
        {
            int nOldPKLevel;
            nOldPKLevel = PKLevel();
            m_nPkPoint += nPoint;
            if (PKLevel() != nOldPKLevel)
            {
                if (PKLevel() <= 2)
                {
                    RefNameColor();
                }
            }
        }

        public void AddBodyLuck(double dLuck)
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

        public void MakeWeaponUnlock()
        {
            if (m_UseItems[grobal2.U_WEAPON] == null)
            {
                return;
            }
            if (m_UseItems[grobal2.U_WEAPON].wIndex <= 0)
            {
                return;
            }
            if (m_UseItems[grobal2.U_WEAPON].btValue[3] > 0)
            {
                m_UseItems[grobal2.U_WEAPON].btValue[3] -= 1;
                SysMsg(M2Share.g_sTheWeaponIsCursed, TMsgColor.c_Red, TMsgType.t_Hint);
            }
            else
            {
                if (m_UseItems[grobal2.U_WEAPON].btValue[4] < 10)
                {
                    m_UseItems[grobal2.U_WEAPON].btValue[4]++;
                    SysMsg(M2Share.g_sTheWeaponIsCursed, TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
            if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
            {
                RecalcAbilitys();
                SendMsg(this, grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                SendMsg(this, grobal2.RM_SUBABILITY, 0, 0, 0, 0, "");
            }
        }

        public int GetAttackPower(int nBasePower, int nPower)
        {
            int result;
            TPlayObject PlayObject;
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
            if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
            {
                PlayObject = this as TPlayObject;
                // Result:=Result * PlayObject.m_nPowerMult + ROUND(Result * (PlayObject.m_nPowerMultPoint / 100));
                result = HUtil32.Round(result * (PlayObject.m_nPowerRate / 100));
                if (PlayObject.m_boPowerItem)
                {
                    result = HUtil32.Round(m_rPowerItem * result);
                }
            }
            if (m_boAutoChangeColor)
            {
                result = result * m_nAutoChangeIdx + 1;
            }
            if (m_boFixColor)
            {
                result = result * m_nFixColorIdx + 1;
            }
            return result;
        }

        /// <summary>
        /// 减少生命值
        /// </summary>
        /// <param name="nDamage"></param>
        public void DamageHealth(int nDamage)
        {
            int nSpdam;
            if (((m_LastHiter == null) || !m_LastHiter.m_boUnMagicShield) && m_boMagicShield && (nDamage > 0) && (m_WAbil.MP > 0))
            {
                nSpdam = HUtil32.Round(nDamage * 1.5);
                if (m_WAbil.MP >= nSpdam)
                {
                    m_WAbil.MP = (short)(m_WAbil.MP - nSpdam);
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
                    m_WAbil.HP = (short)(m_WAbil.HP - nDamage);
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
                    m_WAbil.HP = (short)(m_WAbil.HP - nDamage);
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
                case grobal2.DR_UP:
                    result = grobal2.DR_DOWN;
                    break;
                case grobal2.DR_DOWN:
                    result = grobal2.DR_UP;
                    break;
                case grobal2.DR_LEFT:
                    result = grobal2.DR_RIGHT;
                    break;
                case grobal2.DR_RIGHT:
                    result = grobal2.DR_LEFT;
                    break;
                case grobal2.DR_UPLEFT:
                    result = grobal2.DR_DOWNRIGHT;
                    break;
                case grobal2.DR_UPRIGHT:
                    result = grobal2.DR_DOWNLEFT;
                    break;
                case grobal2.DR_DOWNLEFT:
                    result = grobal2.DR_UPRIGHT;
                    break;
                case grobal2.DR_DOWNRIGHT:
                    result = grobal2.DR_UPLEFT;
                    break;
            }
            return result;
        }

        public int CharPushed(byte nDir, int nPushCount)
        {
            short nx = 0;
            short ny = 0;
            int result = 0;
            byte olddir = m_btDirection;
            int oldx = m_nCurrX;
            int oldy = m_nCurrY;
            m_btDirection = nDir;
            byte nBackDir = GetBackDir(nDir);
            for (var i = 0; i < nPushCount; i++)
            {
                GetFrontPosition(ref nx, ref ny);
                if (m_PEnvir.CanWalk(nx, ny, false))
                {
                    if (m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, nx, ny, false) > 0)
                    {
                        m_nCurrX = nx;
                        m_nCurrY = ny;
                        // SendRefMsg(RM_PUSH, GetBackDir(ndir), m_nCurrX, m_nCurrY, 0, '');
                        SendRefMsg(grobal2.RM_PUSH, nBackDir, m_nCurrX, m_nCurrY, 0, "");
                        result++;
                        if (m_btRaceServer >= grobal2.RC_ANIMAL)
                        {
                            m_dwWalkTick = m_dwWalkTick + 800;
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
            m_btDirection = nBackDir;
            if (result == 0)
            {
                m_btDirection = olddir;
            }
            return result;
        }

        public int MagPassThroughMagic(short sx, short sy, short tx, short ty, int ndir, int magpwr, bool undeadattack)
        {
            TBaseObject BaseObject;
            int tcount = 0;
            for (int i = 0; i <= 12; i++)
            {
                BaseObject = m_PEnvir.GetMovingObject(sx, sy, true) as TBaseObject;
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
                            BaseObject.SendDelayMsg(this, grobal2.RM_MAGSTRUCK, 0, magpwr, 0, 0, "", 600);
                            tcount++;
                        }
                    }
                }
                if (!((Math.Abs(sx - tx) <= 0) && (Math.Abs(sy - ty) <= 0)))
                {
                    ndir = M2Share.GetNextDirection(sx, sy, tx, ty);
                    if (!m_PEnvir.GetNextPosition(sx, sy, ndir, 1, ref sx, ref sy))
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            int result = tcount;
            return result;
        }

        public virtual string GetShowName()
        {
            string sShowName = m_sCharName;
            string result = M2Share.FilterShowName(sShowName);
            if ((m_Master != null) && !m_Master.m_boObMode)
            {
                result = result + '(' + m_Master.m_sCharName + ')';
            }
            return result;
        }

        public virtual void RecalcAbilitys()
        {
           short wOldHP;
            short wOldMP;
            bool boOldHideMode;
            int nOldLight;
            TItem StdItem;
            bool[] boRecallSuite = new bool[4];
            bool[] boMoXieSuite = new bool[3];
            bool[] boSpirit = new bool[4];
            bool boHongMoSuite1;
            bool boHongMoSuite2;
            bool boHongMoSuite3;
            bool boSmash1;
            bool boSmash2;
            bool boSmash3;
            bool boHwanDevil1;
            bool boHwanDevil2;
            bool boHwanDevil3;
            bool boPurity1;
            bool boPurity2;
            bool boPurity3;
            bool boMundane1;
            bool boMundane2;
            bool boNokChi1;
            bool boNokChi2;
            bool boTaoBu1;
            bool boTaoBu2;
            bool boFiveString1;
            bool boFiveString2;
            bool boFiveString3;
            m_AddAbil = new TAddAbility();//FillChar(m_AddAbil, sizeof(TAddAbility), '\0');
            wOldHP = m_WAbil.HP;
            wOldMP = m_WAbil.MP;
            m_WAbil = m_Abil;
            m_WAbil.HP = wOldHP;
            m_WAbil.MP = wOldMP;
            m_WAbil.Weight = 0;
            m_WAbil.WearWeight = 0;
            m_WAbil.HandWeight = 0;
            m_btAntiPoison = 0;
            m_nPoisonRecover = 0;
            m_nHealthRecover = 0;
            m_nSpellRecover = 0;
            m_nAntiMagic = 1;
            m_nLuck = 0;
            m_nHitSpeed = 0;
            m_boExpItem = false;
            m_rExpItem = 0;
            m_boPowerItem = false;
            m_rPowerItem = 0;
            boOldHideMode = m_boHideMode;
            m_boHideMode = false;
            m_boTeleport = false;
            m_boParalysis = false;
            m_boRevival = false;
            m_boUnRevival = false;
            m_boFlameRing = false;
            m_boRecoveryRing = false;
            m_boAngryRing = false;
            m_boMagicShield = false;
            m_boUnMagicShield = false;
            m_boMuscleRing = false;
            m_boFastTrain = false;
            m_boProbeNecklace = false;
            m_boSupermanItem = false;
            m_boGuildMove = false;
            m_boUnParalysis = false;
            m_boExpItem = false;
            m_boPowerItem = false;
            m_boNoDropItem = false;
            m_boNoDropUseItem = false;
            m_bopirit = false;
            m_btHorseType = 0;
            m_btDressEffType = 0;
            m_nAutoAddHPMPMode = 0;
            // 气血石
            m_nMoXieSuite = 0;
            boMoXieSuite[0] = false;
            boMoXieSuite[1] = false;
            boMoXieSuite[2] = false;
            m_db3B0 = 0;
            m_nHongMoSuite = 0;
            boHongMoSuite1 = false;
            boHongMoSuite2 = false;
            boHongMoSuite3 = false;
            boSpirit[0] = false;
            boSpirit[1] = false;
            boSpirit[2] = false;
            boSpirit[3] = false;
            m_boRecallSuite = false;
            boRecallSuite[0] = false;
            boRecallSuite[1] = false;
            boRecallSuite[2] = false;
            boRecallSuite[3] = false;
            m_boSmashSet = false;
            boSmash1 = false;
            boSmash2 = false;
            boSmash3 = false;
            m_boHwanDevilSet = false;
            boHwanDevil1 = false;
            boHwanDevil2 = false;
            boHwanDevil3 = false;
            m_boPuritySet = false;
            boPurity1 = false;
            boPurity2 = false;
            boPurity3 = false;
            m_boMundaneSet = false;
            boMundane1 = false;
            boMundane2 = false;
            m_boNokChiSet = false;
            boNokChi1 = false;
            boNokChi2 = false;
            m_boTaoBuSet = false;
            boTaoBu1 = false;
            boTaoBu2 = false;
            m_boFiveStringSet = false;
            boFiveString1 = false;
            boFiveString2 = false;
            boFiveString3 = false;
            m_dwPKDieLostExp = 0;
            m_nPKDieLostLevel = 0;
            for (var i = m_UseItems.GetLowerBound(0); i <= m_UseItems.GetUpperBound(0); i++)
            {
                if (m_UseItems[i] == null)
                {
                    continue;
                }
                if ((m_UseItems[i].wIndex <= 0) || (m_UseItems[i].Dura <= 0))
                {
                    continue;
                }
                StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[i].wIndex);
                if (StdItem == null)
                {
                    continue;
                }
                StdItem.ApplyItemParameters(ref m_AddAbil);
                if ((i == grobal2.U_WEAPON) || (i == grobal2.U_RIGHTHAND) || (i == grobal2.U_DRESS))
                {
                    if (i == grobal2.U_DRESS)
                    {
                        m_WAbil.WearWeight += StdItem.Weight;
                    }
                    else
                    {
                        m_WAbil.HandWeight += StdItem.Weight;
                    }
                    // 新增开始
                    if (StdItem.AniCount == 120)
                    {
                        m_boFastTrain = true;
                    }
                    if (StdItem.AniCount == 121)
                    {
                        m_boProbeNecklace = true;
                    }
                    if (StdItem.AniCount == 145)
                    {
                        m_boGuildMove = true;
                    }
                    if (StdItem.AniCount == 111)
                    {
                        // 8 0x70
                        m_wStatusTimeArr[grobal2.STATE_TRANSPARENT] = 6 * 10 * 1000;
                        m_boHideMode = true;
                    }
                    if (StdItem.AniCount == 112)
                    {
                        m_boTeleport = true;
                    }
                    if (StdItem.AniCount == 113)
                    {
                        m_boParalysis = true;
                    }
                    if (StdItem.AniCount == 114)
                    {
                        m_boRevival = true;
                    }
                    if (StdItem.AniCount == 115)
                    {
                        m_boFlameRing = true;
                    }
                    if (StdItem.AniCount == 116)
                    {
                        m_boRecoveryRing = true;
                    }
                    if (StdItem.AniCount == 117)
                    {
                        m_boAngryRing = true;
                    }
                    if (StdItem.AniCount == 118)
                    {
                        m_boMagicShield = true;
                    }
                    if (StdItem.AniCount == 119)
                    {
                        m_boMuscleRing = true;
                    }
                    if (StdItem.AniCount == 135)
                    {
                        boMoXieSuite[0] = true;
                        m_nMoXieSuite += StdItem.Weight / 10;
                    }
                    if (StdItem.AniCount == 138)
                    {
                        m_nHongMoSuite += StdItem.Weight;
                    }
                    if (StdItem.AniCount == 139)
                    {
                        m_boUnParalysis = true;
                    }
                    if (StdItem.AniCount == 140)
                    {
                        m_boSupermanItem = true;
                    }
                    if (StdItem.AniCount == 141)
                    {
                        m_boExpItem = true;
                        m_rExpItem = m_rExpItem + (m_UseItems[i].Dura / M2Share.g_Config.nItemExpRate);
                    }
                    if (StdItem.AniCount == 142)
                    {
                        m_boPowerItem = true;
                        m_rPowerItem = m_rPowerItem + (m_UseItems[i].Dura / M2Share.g_Config.nItemPowerRate);
                    }
                    if (StdItem.AniCount == 182)
                    {
                        m_boExpItem = true;
                        m_rExpItem = m_rExpItem + (m_UseItems[i].DuraMax / M2Share.g_Config.nItemExpRate);
                    }
                    if (StdItem.AniCount == 183)
                    {
                        m_boPowerItem = true;
                        m_rPowerItem = m_rPowerItem + (m_UseItems[i].DuraMax / M2Share.g_Config.nItemPowerRate);
                    }
                    if (StdItem.AniCount == 143)
                    {
                        m_boUnMagicShield = true;
                    }
                    if (StdItem.AniCount == 144)
                    {
                        m_boUnRevival = true;
                    }
                    if (StdItem.AniCount == 170)
                    {
                        m_boAngryRing = true;
                    }
                    if (StdItem.AniCount == 171)
                    {
                        m_boNoDropItem = true;
                    }
                    if (StdItem.AniCount == 172)
                    {
                        m_boNoDropUseItem = true;
                    }
                    if (StdItem.AniCount == 150)
                    {
                        // 麻痹护身
                        m_boParalysis = true;
                        m_boMagicShield = true;
                    }
                    if (StdItem.AniCount == 151)
                    {
                        // 麻痹火球
                        m_boParalysis = true;
                        m_boFlameRing = true;
                    }
                    if (StdItem.AniCount == 152)
                    {
                        // 麻痹防御
                        m_boParalysis = true;
                        m_boRecoveryRing = true;
                    }
                    if (StdItem.AniCount == 153)
                    {
                        // 麻痹负载
                        m_boParalysis = true;
                        m_boMuscleRing = true;
                    }
                    if (StdItem.Shape == 154)
                    {
                        // 护身火球
                        m_boMagicShield = true;
                        m_boFlameRing = true;
                    }
                    if (StdItem.AniCount == 155)
                    {
                        // 护身防御
                        m_boMagicShield = true;
                        m_boRecoveryRing = true;
                    }
                    if (StdItem.AniCount == 156)
                    {
                        // 护身负载
                        m_boMagicShield = true;
                        m_boMuscleRing = true;
                    }
                    if (StdItem.AniCount == 157)
                    {
                        // 传送麻痹
                        m_boTeleport = true;
                        m_boParalysis = true;
                    }
                    if (StdItem.AniCount == 158)
                    {
                        // 传送护身
                        m_boTeleport = true;
                        m_boMagicShield = true;
                    }
                    if (StdItem.AniCount == 159)
                    {
                        // 传送探测
                        m_boTeleport = true;
                        m_boProbeNecklace = true;
                    }
                    if (StdItem.AniCount == 160)
                    {
                        // 传送复活
                        m_boTeleport = true;
                        m_boRevival = true;
                    }
                    if (StdItem.AniCount == 161)
                    {
                        // 麻痹复活
                        m_boParalysis = true;
                        m_boRevival = true;
                    }
                    if (StdItem.AniCount == 162)
                    {
                        // 护身复活
                        m_boMagicShield = true;
                        m_boRevival = true;
                    }
                    if (StdItem.AniCount == 180)
                    {
                        // PK 死亡掉经验
                        m_dwPKDieLostExp = StdItem.DuraMax * M2Share.g_Config.dwPKDieLostExpRate;
                        // m_nPKDieLostLevel:=1;
                    }
                    if (StdItem.AniCount == 181)
                    {
                        // PK 死亡掉等级
                        m_nPKDieLostLevel = StdItem.DuraMax / M2Share.g_Config.nPKDieLostLevelRate;
                    }
                    // 新增结束
                }
                else
                {
                    m_WAbil.WearWeight += StdItem.Weight;
                }
                m_WAbil.Weight += StdItem.Weight;
                if (i == grobal2.U_WEAPON)
                {
                    if ((StdItem.Source - 1 - 10) < 0)
                    {
                        m_AddAbil.btWeaponStrong = (byte)StdItem.Source;// 强度+
                    }
                    if ((StdItem.Source <= -1) && (StdItem.Source >= -50))  // -1 to -50
                    {
                        m_AddAbil.btUndead = (byte)(m_AddAbil.btUndead + -StdItem.Source);// Holy+
                    }
                    if ((StdItem.Source <= -51) && (StdItem.Source >= -100))// -51 to -100
                    {
                        m_AddAbil.btUndead = (byte)(m_AddAbil.btUndead + (StdItem.Source + 50));// Holy-
                    }
                    continue;
                }
                if (i == grobal2.U_RIGHTHAND)
                {
                    if (StdItem.Shape >= 1 && StdItem.Shape <= 50)
                    {
                        m_btDressEffType = StdItem.Shape;
                    }
                    if (StdItem.Shape >= 51 && StdItem.Shape <= 100)
                    {
                        m_btHorseType = (byte)(StdItem.Shape - 50);
                    }
                    continue;
                }
                if (i == grobal2.U_DRESS)
                {
                    if (m_UseItems[i].btValue[5] > 0)
                    {
                        m_btDressEffType = m_UseItems[i].btValue[5];
                    }
                    if (StdItem.AniCount > 0)
                    {
                        m_btDressEffType = StdItem.AniCount;
                    }
                    if (StdItem.Light)
                    {
                        m_nLight = 3;
                    }
                    continue;
                }
                // 新增开始
                if (StdItem.Shape == 139)
                {
                    m_boUnParalysis = true;
                }
                if (StdItem.Shape == 140)
                {
                    m_boSupermanItem = true;
                }
                if (StdItem.Shape == 141)
                {
                    m_boExpItem = true;
                    m_rExpItem = m_rExpItem + (m_UseItems[i].Dura / M2Share.g_Config.nItemExpRate);
                }
                if (StdItem.Shape == 142)
                {
                    m_boPowerItem = true;
                    m_rPowerItem = m_rPowerItem + (m_UseItems[i].Dura / M2Share.g_Config.nItemPowerRate);
                }
                if (StdItem.Shape == 182)
                {
                    m_boExpItem = true;
                    m_rExpItem = m_rExpItem + (m_UseItems[i].DuraMax / M2Share.g_Config.nItemExpRate);
                }
                if (StdItem.Shape == 183)
                {
                    m_boPowerItem = true;
                    m_rPowerItem = m_rPowerItem + (m_UseItems[i].DuraMax / M2Share.g_Config.nItemPowerRate);
                }
                if (StdItem.Shape == 143)
                {
                    m_boUnMagicShield = true;
                }
                if (StdItem.Shape == 144)
                {
                    m_boUnRevival = true;
                }
                if (StdItem.Shape == 170)
                {
                    m_boAngryRing = true;
                }
                if (StdItem.Shape == 171)
                {
                    m_boNoDropItem = true;
                }
                if (StdItem.Shape == 172)
                {
                    m_boNoDropUseItem = true;
                }
                if (StdItem.Shape == 150)
                {
                    // 麻痹护身
                    m_boParalysis = true;
                    m_boMagicShield = true;
                }
                if (StdItem.Shape == 151)
                {
                    // 麻痹火球
                    m_boParalysis = true;
                    m_boFlameRing = true;
                }
                if (StdItem.Shape == 152)
                {
                    // 麻痹防御
                    m_boParalysis = true;
                    m_boRecoveryRing = true;
                }
                if (StdItem.Shape == 153)
                {
                    // 麻痹负载
                    m_boParalysis = true;
                    m_boMuscleRing = true;
                }
                if (StdItem.Shape == 154)
                {
                    // 护身火球
                    m_boMagicShield = true;
                    m_boFlameRing = true;
                }
                if (StdItem.Shape == 155)
                {
                    // 护身防御
                    m_boMagicShield = true;
                    m_boRecoveryRing = true;
                }
                if (StdItem.Shape == 156)
                {
                    // 护身负载
                    m_boMagicShield = true;
                    m_boMuscleRing = true;
                }
                if (StdItem.Shape == 157)
                {
                    // 传送麻痹
                    m_boTeleport = true;
                    m_boParalysis = true;
                }
                if (StdItem.Shape == 158)
                {
                    // 传送护身
                    m_boTeleport = true;
                    m_boMagicShield = true;
                }
                if (StdItem.Shape == 159)
                {
                    // 传送探测
                    m_boTeleport = true;
                    m_boProbeNecklace = true;
                }
                if (StdItem.Shape == 160)
                {
                    // 传送复活
                    m_boTeleport = true;
                    m_boRevival = true;
                }
                if (StdItem.Shape == 161)
                {
                    // 麻痹复活
                    m_boParalysis = true;
                    m_boRevival = true;
                }
                if (StdItem.Shape == 162)
                {
                    // 护身复活
                    m_boMagicShield = true;
                    m_boRevival = true;
                }
                if (StdItem.Shape == 180)
                {
                    // PK 死亡掉经验
                    m_dwPKDieLostExp = StdItem.DuraMax * M2Share.g_Config.dwPKDieLostExpRate;
                    // m_nPKDieLostLevel:=1;
                }
                if (StdItem.Shape == 181)
                {
                    // PK 死亡掉等级
                    m_nPKDieLostLevel = StdItem.DuraMax / M2Share.g_Config.nPKDieLostLevelRate;
                }
                // 新增结束
                // if (i = U_NECKLACE) then begin
                if (StdItem.Shape == 120)
                {
                    m_boFastTrain = true;
                }
                if (StdItem.Shape == 121)
                {
                    m_boProbeNecklace = true;
                }
                if (StdItem.Shape == 123)
                {
                    boRecallSuite[0] = true;
                }
                if (StdItem.Shape == 145)
                {
                    m_boGuildMove = true;
                }
                if (StdItem.Shape == 127)
                {
                    boSpirit[0] = true;
                }
                if (StdItem.Shape == 135)
                {
                    boMoXieSuite[0] = true;
                    m_nMoXieSuite += StdItem.AniCount;
                }
                if (StdItem.Shape == 138)
                {
                    boHongMoSuite1 = true;
                    m_nHongMoSuite += StdItem.AniCount;
                }
                if (StdItem.Shape == 200)
                {
                    boSmash1 = true;
                }
                if (StdItem.Shape == 203)
                {
                    boHwanDevil1 = true;
                }
                if (StdItem.Shape == 206)
                {
                    boPurity1 = true;
                }
                if (StdItem.Shape == 216)
                {
                    boFiveString1 = true;
                }
                if (StdItem.Shape == 111)
                {
                    m_wStatusTimeArr[grobal2.STATE_TRANSPARENT] = 6 * 10 * 1000;
                    m_boHideMode = true;
                }
                if (StdItem.Shape == 112)
                {
                    m_boTeleport = true;
                }
                if (StdItem.Shape == 113)
                {
                    m_boParalysis = true;
                }
                if (StdItem.Shape == 114)
                {
                    m_boRevival = true;
                }
                if (StdItem.Shape == 115)
                {
                    m_boFlameRing = true;
                }
                if (StdItem.Shape == 116)
                {
                    m_boRecoveryRing = true;
                }
                if (StdItem.Shape == 117)
                {
                    m_boAngryRing = true;
                }
                if (StdItem.Shape == 118)
                {
                    m_boMagicShield = true;
                }
                if (StdItem.Shape == 119)
                {
                    m_boMuscleRing = true;
                }
                if (StdItem.Shape == 122)
                {
                    boRecallSuite[1] = true;
                }
                if (StdItem.Shape == 128)
                {
                    boSpirit[1] = true;
                }
                if (StdItem.Shape == 133)
                {
                    boMoXieSuite[1] = true;
                    m_nMoXieSuite += StdItem.AniCount;
                }
                if (StdItem.Shape == 136)
                {
                    boHongMoSuite2 = true;
                    m_nHongMoSuite += StdItem.AniCount;
                }
                if (StdItem.Shape == 201)
                {
                    boSmash2 = true;
                }
                if (StdItem.Shape == 204)
                {
                    boHwanDevil2 = true;
                }
                if (StdItem.Shape == 207)
                {
                    boPurity2 = true;
                }
                if (StdItem.Shape == 210)
                {
                    boMundane1 = true;
                }
                if (StdItem.Shape == 212)
                {
                    boNokChi1 = true;
                }
                if (StdItem.Shape == 214)
                {
                    boTaoBu1 = true;
                }
                if (StdItem.Shape == 217)
                {
                    boFiveString2 = true;
                }
                // end;
                // if (i = U_ARMRINGL) or (i = U_ARMRINGR) then begin
                if ((StdItem.Source <= -1) && (StdItem.Source >= -50))
                {
                    // -1 to -50
                    m_AddAbil.btUndead = (byte)(m_AddAbil.btUndead + -StdItem.Source);
                    // Holy+
                }
                if ((StdItem.Source <= -51) && (StdItem.Source >= -100))
                {
                    // -51 to -100
                    m_AddAbil.btUndead = (byte)(m_AddAbil.btUndead + (StdItem.Source + 50));
                    // Holy-
                }
                if (StdItem.Shape == 124)
                {
                    boRecallSuite[2] = true;
                }
                if (StdItem.Shape == 126)
                {
                    boSpirit[2] = true;
                }
                if (StdItem.Shape == 145)
                {
                    m_boGuildMove = true;
                }
                if (StdItem.Shape == 134)
                {
                    boMoXieSuite[2] = true;
                    m_nMoXieSuite += StdItem.AniCount;
                }
                if (StdItem.Shape == 137)
                {
                    boHongMoSuite3 = true;
                    m_nHongMoSuite += StdItem.AniCount;
                }
                if (StdItem.Shape == 202)
                {
                    boSmash3 = true;
                }
                if (StdItem.Shape == 205)
                {
                    boHwanDevil3 = true;
                }
                if (StdItem.Shape == 208)
                {
                    boPurity3 = true;
                }
                if (StdItem.Shape == 211)
                {
                    boMundane2 = true;
                }
                if (StdItem.Shape == 213)
                {
                    boNokChi2 = true;
                }
                if (StdItem.Shape == 215)
                {
                    boTaoBu2 = true;
                }
                if (StdItem.Shape == 218)
                {
                    boFiveString3 = true;
                }
                // end;
                // if (i = U_HELMET) then begin
                if (StdItem.Shape == 125)
                {
                    boRecallSuite[3] = true;
                }
                if (StdItem.Shape == 129)
                {
                    boSpirit[3] = true;
                }
                // end;
            }
            if (boRecallSuite[0] && boRecallSuite[1] && boRecallSuite[2] && boRecallSuite[3])
            {
                m_boRecallSuite = true;
            }
            if (boMoXieSuite[0] && boMoXieSuite[1] && boMoXieSuite[2])
            {
                m_nMoXieSuite += 50;
            }
            if (boHongMoSuite1 && boHongMoSuite2 && boHongMoSuite3)
            {
                m_AddAbil.wHitPoint += 2;
            }
            if (boSpirit[0] && boSpirit[1] && boSpirit[2] && boSpirit[3])
            {
                m_bopirit = true;
            }
            if (boSmash1 && boSmash2 && boSmash3)
            {
                m_boSmashSet = true;
            }
            if (boHwanDevil1 && boHwanDevil2 && boHwanDevil3)
            {
                m_boHwanDevilSet = true;
            }
            if (boPurity1 && boPurity2 && boPurity3)
            {
                m_boPuritySet = true;
            }
            if (boMundane1 && boMundane2)
            {
                m_boMundaneSet = true;
            }
            if (boNokChi1 && boNokChi2)
            {
                m_boNokChiSet = true;
            }
            if (boTaoBu1 && boTaoBu2)
            {
                m_boTaoBuSet = true;
            }
            if (boFiveString1 && boFiveString2 && boFiveString3)
            {
                m_boFiveStringSet = true;
            }
            m_WAbil.Weight = (short)RecalcBagWeight();
            if (m_boTransparent && (m_wStatusTimeArr[grobal2.STATE_TRANSPARENT] > 0))
            {
                m_boHideMode = true;
            }
            if (m_boHideMode)
            {
                if (!boOldHideMode)
                {
                    m_nCharStatus = GetCharStatus();
                    StatusChanged();
                }
            }
            else
            {
                if (boOldHideMode)
                {
                    m_wStatusTimeArr[grobal2.STATE_TRANSPARENT] = 0;
                    m_nCharStatus = GetCharStatus();
                    StatusChanged();
                }
            }
            if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
            {
                // 01-20 增加此行，只有类型为人物的角色才重新计算攻击敏捷
                RecalcHitSpeed();
            }
            nOldLight = m_nLight;
            if ((m_UseItems[grobal2.U_RIGHTHAND] != null) && (m_UseItems[grobal2.U_RIGHTHAND].wIndex > 0) && (m_UseItems[grobal2.U_RIGHTHAND].Dura > 0))
            {
                m_nLight = 3;
            }
            else
            {
                m_nLight = 0;
            }
            if (nOldLight != m_nLight)
            {
                SendRefMsg(grobal2.RM_CHANGELIGHT, 0, 0, 0, 0, "");
            }
            m_btSpeedPoint += (byte)m_AddAbil.wSpeedPoint;
            m_btHitPoint += (byte)m_AddAbil.wHitPoint;
            m_btAntiPoison += (byte)m_AddAbil.wAntiPoison;
            m_nPoisonRecover += m_AddAbil.wPoisonRecover;
            m_nHealthRecover += m_AddAbil.wHealthRecover;
            m_nSpellRecover += m_AddAbil.wSpellRecover;
            m_nAntiMagic += m_AddAbil.wAntiMagic;
            m_nLuck += m_AddAbil.btLuck;
            m_nLuck -= m_AddAbil.btUnLuck;
            m_nHitSpeed = m_AddAbil.nHitSpeed;
            m_WAbil.MaxWeight += m_AddAbil.Weight;
            m_WAbil.MaxWearWeight += m_AddAbil.WearWeight;
            m_WAbil.MaxHandWeight += m_AddAbil.HandWeight;
            m_WAbil.MaxHP = (short)HUtil32._MIN(short.MaxValue, m_Abil.MaxHP + m_AddAbil.wHP);
            m_WAbil.MaxMP = (short)HUtil32._MIN(short.MaxValue, m_Abil.MaxMP + m_AddAbil.wMP);
            m_WAbil.AC = HUtil32.MakeLong(HUtil32.LoWord(m_AddAbil.wAC) + HUtil32.LoWord(m_Abil.AC), HUtil32.HiWord(m_AddAbil.wAC) + HUtil32.HiWord(m_Abil.AC));
            m_WAbil.MAC = HUtil32.MakeLong(HUtil32.LoWord(m_AddAbil.wMAC) + HUtil32.LoWord(m_Abil.MAC), HUtil32.HiWord(m_AddAbil.wMAC) + HUtil32.HiWord(m_Abil.MAC));
            m_WAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(m_AddAbil.wDC) + HUtil32.LoWord(m_Abil.DC), HUtil32.HiWord(m_AddAbil.wDC) + HUtil32.HiWord(m_Abil.DC));
            m_WAbil.MC = HUtil32.MakeLong(HUtil32.LoWord(m_AddAbil.wMC) + HUtil32.LoWord(m_Abil.MC), HUtil32.HiWord(m_AddAbil.wMC) + HUtil32.HiWord(m_Abil.MC));
            m_WAbil.SC = HUtil32.MakeLong(HUtil32.LoWord(m_AddAbil.wSC) + HUtil32.LoWord(m_Abil.SC), HUtil32.HiWord(m_AddAbil.wSC) + HUtil32.HiWord(m_Abil.SC));
            if (m_wStatusTimeArr[grobal2.STATE_DEFENCEUP] > 0)
            {
                m_WAbil.AC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.AC), HUtil32.HiWord(m_WAbil.AC) + 2 + (m_Abil.Level / 7));
            }
            if (m_wStatusTimeArr[grobal2.STATE_MAGDEFENCEUP] > 0)
            {
                m_WAbil.MAC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.MAC), HUtil32.HiWord(m_WAbil.MAC) + 2 + (m_Abil.Level / 7));
            }
            if (m_wStatusArrValue[0] > 0)
            {
                m_WAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.DC), HUtil32.HiWord(m_WAbil.DC) + 2 + m_wStatusArrValue[0]);
            }
            if (m_wStatusArrValue[1] > 0)
            {
                m_WAbil.MC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.MC), HUtil32.HiWord(m_WAbil.MC) + 2 + m_wStatusArrValue[1]);
            }
            if (m_wStatusArrValue[2] > 0)
            {
                m_WAbil.SC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.SC), HUtil32.HiWord(m_WAbil.SC) + 2 + m_wStatusArrValue[2]);
            }
            if (m_wStatusArrValue[3] > 0)
            {
                m_nHitSpeed += m_wStatusArrValue[3];
            }
            if (m_wStatusArrValue[4] > 0)
            {
                m_WAbil.MaxHP = (short)HUtil32._MIN(short.MaxValue, m_WAbil.MaxHP + m_wStatusArrValue[4]);
            }
            if (m_wStatusArrValue[5] > 0)
            {
                m_WAbil.MaxMP = (short)HUtil32._MIN(short.MaxValue, m_WAbil.MaxMP + m_wStatusArrValue[5]);
            }
            if (m_boFlameRing)
            {
                AddItemSkill(1);
            }
            else
            {
                DelItemSkill(1);
            }
            if (m_boRecoveryRing)
            {
                AddItemSkill(2);
            }
            else
            {
                DelItemSkill(2);
            }
            if (m_boMuscleRing)
            {
                // 活力
                m_WAbil.MaxWeight += m_WAbil.MaxWeight;
                m_WAbil.MaxWearWeight += m_WAbil.MaxWearWeight;
                m_WAbil.MaxHandWeight += m_WAbil.MaxHandWeight;
            }
            if (m_nMoXieSuite > 0)
            {
                // 魔血
                if (m_WAbil.MaxMP <= m_nMoXieSuite)
                {
                    m_nMoXieSuite = m_WAbil.MaxMP - 1;
                }
                m_WAbil.MaxMP -= (short)m_nMoXieSuite;
                // Inc(m_WAbil.MaxHP,m_nMoXieSuite);
                m_WAbil.MaxHP = (short)HUtil32._MIN(short.MaxValue, m_WAbil.MaxHP + m_nMoXieSuite);
            }
            if (m_bopirit)
            {
                // Bonus DC Min +2,DC Max +5,A.Speed + 2
                m_WAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.DC) + 2, HUtil32.HiWord(m_WAbil.DC) + 2 + 5);
                m_nHitSpeed += 2;
            }
            if (m_boSmashSet)
            {
                // Attack Speed +1, DC1-3
                m_WAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.DC) + 1, HUtil32.HiWord(m_WAbil.DC) + 2 + 3);
                m_nHitSpeed++;
            }
            if (m_boHwanDevilSet)
            {
                // Hand Carrying Weight Increase +5, Bag Weight Limit Increase +20, +MC 1-2
                m_WAbil.MaxHandWeight += 5;
                m_WAbil.MaxWeight += 20;
                m_WAbil.MC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.MC) + 1, HUtil32.HiWord(m_WAbil.MC) + 2 + 2);
            }
            if (m_boPuritySet)
            {
                // Holy +3, Sc 1-2
                m_AddAbil.btUndead = (byte)(m_AddAbil.btUndead + -3);
                m_WAbil.SC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.SC) + 1, HUtil32.HiWord(m_WAbil.SC) + 2 + 2);
            }
            if (m_boMundaneSet)
            {
                // Bonus of Hp+50
                m_WAbil.MaxHP = (short)HUtil32._MIN(short.MaxValue, m_WAbil.MaxHP + 50);
            }
            if (m_boNokChiSet)
            {
                // Bonus of Mp+50
                m_WAbil.MaxMP = (short)HUtil32._MIN(short.MaxValue, m_WAbil.MaxMP + 50);
            }
            if (m_boTaoBuSet)
            {
                // Bonus of Hp+30, Mp+30
                m_WAbil.MaxHP = (short)HUtil32._MIN(short.MaxValue, m_WAbil.MaxHP + 30);
                m_WAbil.MaxMP = (short)HUtil32._MIN(short.MaxValue, m_WAbil.MaxMP + 30);
            }
            if (m_boFiveStringSet)
            {
                // Bonus of Hp +30%, Ac+2
                m_WAbil.MaxHP = (short)HUtil32._MIN(short.MaxValue, m_WAbil.MaxHP / 100 * 30);
                m_btHitPoint += 2;
            }
            if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
            {
                SendUpdateMsg(this, grobal2.RM_CHARSTATUSCHANGED, m_nHitSpeed, m_nCharStatus, 0, 0, "");
            }
            if (m_btRaceServer >= grobal2.RC_ANIMAL)
            {
                MonsterRecalcAbilitys();
            }
            // 限制最高属性
            m_WAbil.AC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(m_WAbil.AC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(m_WAbil.AC)));
            m_WAbil.MAC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(m_WAbil.MAC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(m_WAbil.MAC)));
            m_WAbil.DC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(m_WAbil.DC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(m_WAbil.DC)));
            m_WAbil.MC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(m_WAbil.MC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(m_WAbil.MC)));
            m_WAbil.SC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(m_WAbil.SC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(m_WAbil.SC)));
            if (M2Share.g_Config.boHungerSystem && M2Share.g_Config.boHungerDecPower)
            {
                if (HUtil32.RangeInDefined(m_nHungerStatus, 0, 999))
                {
                    m_WAbil.DC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.DC) * 0.2), HUtil32.Round(HUtil32.HiWord(m_WAbil.DC) * 0.2));
                    m_WAbil.MC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.MC) * 0.2), HUtil32.Round(HUtil32.HiWord(m_WAbil.MC) * 0.2));
                    m_WAbil.SC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.SC) * 0.2), HUtil32.Round(HUtil32.HiWord(m_WAbil.SC) * 0.2));
                }
                else if (HUtil32.RangeInDefined(m_nHungerStatus, 1000, 1999))
                {
                    m_WAbil.DC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.DC) * 0.4), HUtil32.Round(HUtil32.HiWord(m_WAbil.DC) * 0.4));
                    m_WAbil.MC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.MC) * 0.4), HUtil32.Round(HUtil32.HiWord(m_WAbil.MC) * 0.4));
                    m_WAbil.SC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.SC) * 0.4), HUtil32.Round(HUtil32.HiWord(m_WAbil.SC) * 0.4));
                }
                else if (HUtil32.RangeInDefined(m_nHungerStatus, 2000, 2999))
                {
                    m_WAbil.DC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.DC) * 0.6), HUtil32.Round(HUtil32.HiWord(m_WAbil.DC) * 0.6));
                    m_WAbil.MC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.MC) * 0.6), HUtil32.Round(HUtil32.HiWord(m_WAbil.MC) * 0.6));
                    m_WAbil.SC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.SC) * 0.6), HUtil32.Round(HUtil32.HiWord(m_WAbil.SC) * 0.6));
                }
                else if (HUtil32.RangeInDefined(m_nHungerStatus, 3000, 3000))
                {
                    m_WAbil.DC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.DC) * 0.9), HUtil32.Round(HUtil32.HiWord(m_WAbil.DC) * 0.9));
                    m_WAbil.MC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.MC) * 0.9), HUtil32.Round(HUtil32.HiWord(m_WAbil.MC) * 0.9));
                    m_WAbil.SC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.SC) * 0.9), HUtil32.Round(HUtil32.HiWord(m_WAbil.SC) * 0.9));
                }
            }
        }

        public void BreakOpenHealth()
        {
            if (m_boShowHP)
            {
                m_boShowHP = false;
                m_nCharStatusEx = m_nCharStatusEx ^ grobal2.STATE_OPENHEATH;
                m_nCharStatus = GetCharStatus();
                SendRefMsg(grobal2.RM_CLOSEHEALTH, 0, 0, 0, 0, "");
            }
        }

        public void MakeOpenHealth()
        {
            m_boShowHP = true;
            m_nCharStatusEx = m_nCharStatusEx | grobal2.STATE_OPENHEATH;
            m_nCharStatus = GetCharStatus();
            SendRefMsg(grobal2.RM_OPENHEALTH, 0, m_WAbil.HP, m_WAbil.MaxHP, 0, "");
        }

        public void IncHealthSpell(int nHP, int nMP)
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
                m_WAbil.HP += (short)nHP;
            }
            if ((m_WAbil.MP + nMP) >= m_WAbil.MaxMP)
            {
                m_WAbil.MP = m_WAbil.MaxMP;
            }
            else
            {
                m_WAbil.MP += (short)nMP;
            }
            HealthSpellChanged();
        }

        public void ItemDamageRevivalRing()
        {
            TItem pSItem;
            int nDura;
            int tDura;
            TPlayObject PlayObject;
            for (int i = m_UseItems.GetLowerBound(0); i <= m_UseItems.GetUpperBound(0); i++)
            {
                if (m_UseItems[i] != null && m_UseItems[i].wIndex > 0)
                {
                    pSItem = M2Share.UserEngine.GetStdItem(m_UseItems[i].wIndex);
                    if (pSItem != null)
                    {
                        if (new ArrayList(new int[] { 114, 160, 161, 162 }).Contains(pSItem.Shape) || (((i == grobal2.U_WEAPON) || (i == grobal2.U_RIGHTHAND)) && new ArrayList(new int[] { 114, 160, 161, 162 }).Contains(pSItem.AniCount)))
                        {
                            nDura = m_UseItems[i].Dura;
                            tDura = HUtil32.Round(nDura / 1000);// 1.03
                            nDura -= 1000;
                            if (nDura <= 0)
                            {
                                nDura = 0;
                                m_UseItems[i].Dura = (short)nDura;
                                if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                                {
                                    PlayObject = this as TPlayObject;
                                    PlayObject.SendDelItems(m_UseItems[i]);
                                }
                                m_UseItems[i].wIndex = 0;
                                RecalcAbilitys();
                            }
                            else
                            {
                                m_UseItems[i].Dura = (short)nDura;
                            }
                            if (tDura != HUtil32.Round(nDura / 1000)) // 1.03
                            {
                                SendMsg(this, grobal2.RM_DURACHANGE, (short)i, nDura, m_UseItems[i].DuraMax, 0, "");
                            }
                        }
                    }
                }
            }
        }

        public virtual void Run()
        {
            int i;
            bool boChg;
            bool boNeedRecalc;
            int nHP;
            int nMP;
            int n18;
            int dwC;
            int dwInChsTime;
            TProcessMessage ProcessMsg = null;
            TBaseObject BaseObject;
            int dwRunTick;
            int nInteger;
            TItem StdItem;
            int nCount;
            int dCount;
            int bCount;
            const string sExceptionMsg0 = "[Exception] TBaseObject::Run 0";
            const string sExceptionMsg1 = "[Exception] TBaseObject::Run 1";
            const string sExceptionMsg2 = "[Exception] TBaseObject::Run 2";
            const string sExceptionMsg3 = "[Exception] TBaseObject::Run 3";
            const string sExceptionMsg4 = "[Exception] TBaseObject::Run 4 Code:%d";
            const string sExceptionMsg5 = "[Exception] TBaseObject::Run 5";
            const string sExceptionMsg6 = "[Exception] TBaseObject::Run 6";
            const string sExceptionMsg7 = "[Exception] TBaseObject::Run 7";
            dwRunTick = HUtil32.GetTickCount();
            try
            {
                while (GetMessage(ref ProcessMsg))
                {
                    Operate(ProcessMsg);
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg0);
                M2Share.ErrorMessage(e.Message);
            }
            try
            {
                if (m_boSuperMan)
                {
                    m_WAbil.HP = m_WAbil.MaxHP;
                    m_WAbil.MP = m_WAbil.MaxMP;
                }
                dwC = (HUtil32.GetTickCount() - m_dwHPMPTick) / 20;
                m_dwHPMPTick = HUtil32.GetTickCount();
                m_nHealthTick += dwC;
                m_nSpellTick += dwC;
                if (!m_boDeath)
                {
                    if ((m_WAbil.HP < m_WAbil.MaxHP) && (m_nHealthTick >= M2Share.g_Config.nHealthFillTime))
                    {
                        n18 = (m_WAbil.MaxHP / 75) + 1;
                        // nPlus = m_WAbility.MaxHP / 15 + 1;
                        if ((m_WAbil.HP + n18) < m_WAbil.MaxHP)
                        {
                            m_WAbil.HP += (short)n18;
                        }
                        else
                        {
                            m_WAbil.HP = m_WAbil.MaxHP;
                        }
                        HealthSpellChanged();
                    }
                    if ((m_WAbil.MP < m_WAbil.MaxMP) && (m_nSpellTick >= M2Share.g_Config.nSpellFillTime))
                    {
                        n18 = (m_WAbil.MaxMP / 18) + 1;
                        if ((m_WAbil.MP + n18) < m_WAbil.MaxMP)
                        {
                            m_WAbil.MP += (short)n18;
                        }
                        else
                        {
                            m_WAbil.MP = m_WAbil.MaxMP;
                        }
                        HealthSpellChanged();
                    }
                    if (m_WAbil.HP == 0)
                    {
                        // 防复活
                        // 60 * 1000
                        if (((m_LastHiter == null) || !m_LastHiter.m_boUnRevival) && m_boRevival && (HUtil32.GetTickCount() - m_dwRevivalTick > M2Share.g_Config.dwRevivalTime))
                        {
                            m_dwRevivalTick = HUtil32.GetTickCount();
                            ItemDamageRevivalRing();
                            m_WAbil.HP = m_WAbil.MaxHP;
                            HealthSpellChanged();
                            // '复活戒指生效，体力恢复'
                            SysMsg(M2Share.g_sRevivalRecoverMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                        }
                        if (m_WAbil.HP == 0)
                        {
                            Die();
                        }
                    }
                    if (m_nHealthTick >= M2Share.g_Config.nHealthFillTime)
                    {
                        m_nHealthTick = 0;
                    }
                    if (m_nSpellTick >= M2Share.g_Config.nSpellFillTime)
                    {
                        m_nSpellTick = 0;
                    }
                }
                else
                {
                    // 3 * 60 * 1000
                    if (HUtil32.GetTickCount() - m_dwDeathTick > M2Share.g_Config.dwMakeGhostTime)
                    {
                        MakeGhost();
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg1);
                M2Share.ErrorMessage(e.Message);
            }
            try
            {
                if (!m_boDeath && ((m_nIncSpell > 0) || (m_nIncHealth > 0) || (m_nIncHealing > 0)))
                {
                    dwInChsTime = 600 - HUtil32._MIN(400, m_Abil.Level * 10);
                    if (((HUtil32.GetTickCount() - m_dwIncHealthSpellTick) >= dwInChsTime) && !m_boDeath)
                    {
                        dwC = HUtil32._MIN(200, HUtil32.GetTickCount() - m_dwIncHealthSpellTick - dwInChsTime);
                        m_dwIncHealthSpellTick = HUtil32.GetTickCount() + dwC;
                        if ((m_nIncSpell > 0) || (m_nIncHealth > 0) || (m_nPerHealing > 0))
                        {
                            if (m_nPerHealth <= 0)
                            {
                                m_nPerHealth = 1;
                            }
                            if (m_nPerSpell <= 0)
                            {
                                m_nPerSpell = 1;
                            }
                            if (m_nPerHealing <= 0)
                            {
                                m_nPerHealing = 1;
                            }
                            if (m_nIncHealth < m_nPerHealth)
                            {
                                nHP = m_nIncHealth;
                                m_nIncHealth = 0;
                            }
                            else
                            {
                                nHP = m_nPerHealth;
                                m_nIncHealth -= m_nPerHealth;
                            }
                            if (m_nIncSpell < m_nPerSpell)
                            {
                                nMP = m_nIncSpell;
                                m_nIncSpell = 0;
                            }
                            else
                            {
                                nMP = m_nPerSpell;
                                m_nIncSpell -= m_nPerSpell;
                            }
                            if (m_nIncHealing < m_nPerHealing)
                            {
                                nHP += m_nIncHealing;
                                m_nIncHealing = 0;
                            }
                            else
                            {
                                nHP += m_nPerHealing;
                                m_nIncHealing -= m_nPerHealing;
                            }
                            m_nPerHealth = m_Abil.Level / 10 + 5;
                            m_nPerSpell = m_Abil.Level / 10 + 5;
                            m_nPerHealing = 5;
                            IncHealthSpell(nHP, nMP);
                            if (m_WAbil.HP == m_WAbil.MaxHP)
                            {
                                m_nIncHealth = 0;
                                m_nIncHealing = 0;
                            }
                            if (m_WAbil.MP == m_WAbil.MaxMP)
                            {
                                m_nIncSpell = 0;
                            }
                        }
                    }
                }
                else
                {
                    m_dwIncHealthSpellTick = HUtil32.GetTickCount();
                }
                if ((m_nHealthTick < -M2Share.g_Config.nHealthFillTime) && (m_WAbil.HP > 1))
                {
                    // Jacky ????
                    m_WAbil.HP -= 1;
                    m_nHealthTick += M2Share.g_Config.nHealthFillTime;
                    HealthSpellChanged();
                }
                // 检查HP/MP值是否大于最大值，大于则降低到正常大小
                boNeedRecalc = false;
                if (m_WAbil.HP > m_WAbil.MaxHP)
                {
                    boNeedRecalc = true;
                    m_WAbil.HP = (short)(m_WAbil.MaxHP - 1);
                }
                if (m_WAbil.MP > m_WAbil.MaxMP)
                {
                    boNeedRecalc = true;
                    m_WAbil.MP = (short)(m_WAbil.MaxMP - 1);
                }
                if (boNeedRecalc)
                {
                    HealthSpellChanged();
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg2);
            }
            // 血气石处理开始
            try
            {
                if (m_UseItems[grobal2.U_CHARM] != null)
                {
                    if (!m_boDeath && new ArrayList(new int[] { grobal2.RC_PLAYOBJECT, grobal2.RC_PLAYCLONE }).Contains(m_btRaceServer))
                    {
                        // 加HP
                        if ((m_nIncHealth == 0) && (m_UseItems[grobal2.U_CHARM].wIndex > 0) && ((HUtil32.GetTickCount() - m_nIncHPStoneTime) > M2Share.g_Config.HPStoneIntervalTime) && ((m_WAbil.HP / m_WAbil.MaxHP * 100) < M2Share.g_Config.HPStoneStartRate))
                        {
                            m_nIncHPStoneTime = HUtil32.GetTickCount();
                            StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[grobal2.U_CHARM].wIndex);
                            if ((StdItem.StdMode == 7) && new ArrayList(new int[] { 1, 3 }).Contains(StdItem.Shape))
                            {
                                nCount = m_UseItems[grobal2.U_CHARM].Dura * 10;
                                bCount = Convert.ToInt32(nCount / M2Share.g_Config.HPStoneAddRate);
                                dCount = m_WAbil.MaxHP - m_WAbil.HP;
                                if (dCount > bCount)
                                {
                                    dCount = bCount;
                                }
                                if (nCount > dCount)
                                {
                                    m_nIncHealth += dCount;
                                    m_UseItems[grobal2.U_CHARM].Dura -= (short)HUtil32.Round(dCount / 10);
                                }
                                else
                                {
                                    nCount = 0;
                                    m_nIncHealth += nCount;
                                    m_UseItems[grobal2.U_CHARM].Dura = 0;
                                }
                                if (m_UseItems[grobal2.U_CHARM].Dura >= 1000)
                                {
                                    if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                                    {
                                        SendMsg(this, grobal2.RM_DURACHANGE, grobal2.U_CHARM, m_UseItems[grobal2.U_CHARM].Dura, m_UseItems[grobal2.U_CHARM].DuraMax, 0, "");
                                    }
                                }
                                else
                                {
                                    m_UseItems[grobal2.U_CHARM].Dura = 0;
                                    if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                                    {
                                        (this as TPlayObject).SendDelItems(m_UseItems[grobal2.U_CHARM]);
                                    }
                                    m_UseItems[grobal2.U_CHARM].wIndex = 0;
                                }
                            }
                        }
                        // 加MP
                        if ((m_nIncSpell == 0) && (m_UseItems[grobal2.U_CHARM].wIndex > 0) && ((HUtil32.GetTickCount() - m_nIncMPStoneTime) > M2Share.g_Config.MPStoneIntervalTime) && ((m_WAbil.MP / m_WAbil.MaxMP * 100) < M2Share.g_Config.MPStoneStartRate))
                        {
                            m_nIncMPStoneTime = HUtil32.GetTickCount();
                            StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[grobal2.U_CHARM].wIndex);
                            if ((StdItem.StdMode == 7) && new ArrayList(new int[] { 2, 3 }).Contains(StdItem.Shape))
                            {
                                nCount = m_UseItems[grobal2.U_CHARM].Dura * 10;
                                bCount = Convert.ToInt32(nCount / M2Share.g_Config.MPStoneAddRate);
                                dCount = m_WAbil.MaxMP - m_WAbil.MP;
                                if (dCount > bCount)
                                {
                                    dCount = bCount;
                                }
                                if (nCount > dCount)
                                {
                                    // Dec(nCount,dCount);
                                    m_nIncSpell += dCount;
                                    m_UseItems[grobal2.U_CHARM].Dura -= (short)HUtil32.Round(dCount / 10);
                                }
                                else
                                {
                                    nCount = 0;
                                    m_nIncSpell += nCount;
                                    m_UseItems[grobal2.U_CHARM].Dura = 0;
                                }
                                if (m_UseItems[grobal2.U_CHARM].Dura >= 1000)
                                {
                                    if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                                    {
                                        SendMsg(this, grobal2.RM_DURACHANGE, grobal2.U_CHARM, m_UseItems[grobal2.U_CHARM].Dura, m_UseItems[grobal2.U_CHARM].DuraMax, 0, "");
                                    }
                                }
                                else
                                {
                                    m_UseItems[grobal2.U_CHARM].Dura = 0;
                                    if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                                    {
                                        (this as TPlayObject).SendDelItems(m_UseItems[grobal2.U_CHARM]);
                                    }
                                    m_UseItems[grobal2.U_CHARM].wIndex = 0;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg7);
            }
            // 血气石处理结束
            // TBaseObject.Run 3 清理目标对象
            try
            {
                if (m_TargetCret != null)
                {
                    // 08/06 增加，弓箭卫士在人物进入房间后再出来，还会攻击人物(人物的攻击目标没清除)
                    if (((HUtil32.GetTickCount() - m_dwTargetFocusTick) > 30000) || m_TargetCret.m_boDeath || m_TargetCret.m_boGhost || (m_TargetCret.m_PEnvir != m_PEnvir) || (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 15) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 15))
                    {
                        m_TargetCret = null;
                    }
                }
                if (m_LastHiter != null)
                {
                    if (((HUtil32.GetTickCount() - m_LastHiterTick) > 30000) || m_LastHiter.m_boDeath || m_LastHiter.m_boGhost)
                    {
                        m_LastHiter = null;
                    }
                }
                if (m_ExpHitter != null)
                {
                    if (((HUtil32.GetTickCount() - m_ExpHitterTick) > 6000) || m_ExpHitter.m_boDeath || m_ExpHitter.m_boGhost)
                    {
                        m_ExpHitter = null;
                    }
                }
                if (m_Master != null)
                {
                    m_boNoItem = true;
                    // 宝宝变色
                    if (m_boAutoChangeColor && (HUtil32.GetTickCount() - m_dwAutoChangeColorTick > M2Share.g_Config.dwBBMonAutoChangeColorTime))
                    {
                        m_dwAutoChangeColorTick = HUtil32.GetTickCount();
                        switch (m_nAutoChangeIdx)
                        {
                            case 0:
                                nInteger = grobal2.STATE_TRANSPARENT;
                                break;
                            case 1:
                                nInteger = grobal2.POISON_STONE;
                                break;
                            case 2:
                                nInteger = grobal2.POISON_DONTMOVE;
                                break;
                            case 3:
                                nInteger = grobal2.POISON_68;
                                break;
                            case 4:
                                nInteger = grobal2.POISON_DECHEALTH;
                                break;
                            case 5:
                                nInteger = grobal2.POISON_LOCKSPELL;
                                break;
                            case 6:
                                nInteger = grobal2.POISON_DAMAGEARMOR;
                                break;
                            default:
                                m_nAutoChangeIdx = 0;
                                nInteger = grobal2.STATE_TRANSPARENT;
                                break;
                        }
                        m_nAutoChangeIdx++;
                        m_nCharStatus = (int)((m_nCharStatusEx & 0xFFFFF) | ((0x80000000 >> nInteger) | 0));
                        StatusChanged();
                    }
                    if (m_boFixColor && (m_nFixStatus != m_nCharStatus))
                    {
                        switch (m_nFixColorIdx)
                        {
                            case 0:
                                nInteger = grobal2.STATE_TRANSPARENT;
                                break;
                            case 1:
                                nInteger = grobal2.POISON_STONE;
                                break;
                            case 2:
                                nInteger = grobal2.POISON_DONTMOVE;
                                break;
                            case 3:
                                nInteger = grobal2.POISON_68;
                                break;
                            case 4:
                                nInteger = grobal2.POISON_DECHEALTH;
                                break;
                            case 5:
                                nInteger = grobal2.POISON_LOCKSPELL;
                                break;
                            case 6:
                                nInteger = grobal2.POISON_DAMAGEARMOR;
                                break;
                            default:
                                m_nFixColorIdx = 0;
                                nInteger = grobal2.STATE_TRANSPARENT;
                                break;
                        }
                        m_nCharStatus = (int)((m_nCharStatusEx & 0xFFFFF) | ((0x80000000 >> nInteger) | 0));
                        m_nFixStatus = m_nCharStatus;
                        StatusChanged();
                    }
                    // 宝宝在主人死亡后死亡处理

                    if (m_Master.m_boDeath && ((HUtil32.GetTickCount() - m_Master.m_dwDeathTick) > 1000))
                    {
                        if (M2Share.g_Config.boMasterDieMutiny && (m_Master.m_LastHiter != null) && (M2Share.RandomNumber.Random(M2Share.g_Config.nMasterDieMutinyRate) == 0))
                        {
                            m_Master = null;
                            m_btSlaveExpLevel = (byte)M2Share.g_Config.SlaveColor.GetUpperBound(0);
                            RecalcAbilitys();
                            m_WAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.DC) * M2Share.g_Config.nMasterDieMutinyPower, HUtil32.HiWord(m_WAbil.DC) * M2Share.g_Config.nMasterDieMutinyPower);
                            m_nWalkSpeed = m_nWalkSpeed / M2Share.g_Config.nMasterDieMutinySpeed;
                            RefNameColor();
                            RefShowName();
                        }
                        else
                        {
                            m_WAbil.HP = 0;
                        }
                    }
                    if (m_Master.m_boGhost && ((HUtil32.GetTickCount() - m_Master.m_dwGhostTick) > 1000))
                    {
                        MakeGhost();
                    }
                }
                // 清除宝宝列表中已经死亡及叛变的宝宝信息
                for (i = m_SlaveList.Count - 1; i >= 0; i--)
                {
                    if (m_SlaveList[i].m_boDeath || m_SlaveList[i].m_boGhost || (m_SlaveList[i].m_Master != this))
                    {
                        m_SlaveList.RemoveAt(i);
                    }
                }
                if (m_boHolySeize && ((HUtil32.GetTickCount() - m_dwHolySeizeTick) > m_dwHolySeizeInterval))
                {
                    BreakHolySeizeMode();
                }
                if (m_boCrazyMode && ((HUtil32.GetTickCount() - m_dwCrazyModeTick) > m_dwCrazyModeInterval))
                {
                    BreakCrazyMode();
                }
                if (m_boShowHP && ((HUtil32.GetTickCount() - m_dwShowHPTick) > m_dwShowHPInterval))
                {
                    BreakOpenHealth();
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg3);
            }
            try
            {
                // 减少PK值开始
                // 120000
                if ((HUtil32.GetTickCount() - m_dwDecPkPointTick) > M2Share.g_Config.dwDecPkPointTime)
                {
                    m_dwDecPkPointTick = HUtil32.GetTickCount();
                    if (m_nPkPoint > 0)
                    {
                        DecPKPoint(M2Share.g_Config.nDecPkPointCount);
                    }
                }
                if ((HUtil32.GetTickCount() - m_DecLightItemDrugTick) > M2Share.g_Config.dwDecLightItemDrugTime)
                {
                    m_DecLightItemDrugTick += M2Share.g_Config.dwDecLightItemDrugTime;
                    if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                    {
                        UseLamp();
                        CheckPKStatus();
                    }
                }
                if ((HUtil32.GetTickCount() - m_dwCheckRoyaltyTick) > 10000)
                {
                    m_dwCheckRoyaltyTick = HUtil32.GetTickCount();
                    if (m_Master != null)
                    {
                        if ((M2Share.g_dwSpiritMutinyTick > HUtil32.GetTickCount()) && (m_btSlaveExpLevel < 5))
                        {
                            m_dwMasterRoyaltyTick = 0;
                        }
                        if (HUtil32.GetTickCount() > m_dwMasterRoyaltyTick)
                        {
                            for (i = 0; i < m_Master.m_SlaveList.Count; i++)
                            {
                                if (m_Master.m_SlaveList[i] == this)
                                {
                                    m_Master.m_SlaveList.RemoveAt(i);
                                    break;
                                }
                            }
                            m_Master = null;
                            m_WAbil.HP = (short)(m_WAbil.HP / 10);
                            RefShowName();
                        }
                        if (m_dwMasterTick != 0)
                        {
                            if ((HUtil32.GetTickCount() - m_dwMasterTick) > 12 * 60 * 60 * 1000)
                            {
                                m_WAbil.HP = 0;
                            }
                        }
                    }
                }

                if ((HUtil32.GetTickCount() - m_dwVerifyTick) > 30 * 1000)
                {
                    m_dwVerifyTick = HUtil32.GetTickCount();
                    // 清组队已死亡成员
                    if (m_GroupOwner != null)
                    {
                        if (m_GroupOwner.m_boDeath || m_GroupOwner.m_boGhost)
                        {
                            m_GroupOwner = null;
                        }
                    }

                    if (m_GroupOwner == this)
                    {
                        for (i = m_GroupMembers.Count - 1; i >= 0; i--)
                        {
                            BaseObject = m_GroupMembers[i];
                            if (BaseObject.m_boDeath || BaseObject.m_boGhost)
                            {
                                m_GroupMembers.RemoveAt(i);
                            }
                        }
                    }
                    // 检查交易双方 状态
                    if ((m_DealCreat != null) && m_DealCreat.m_boGhost)
                    {
                        m_DealCreat = null;
                    }
                    if (!m_boDenyRefStatus)
                    {
                        m_PEnvir.VerifyMapTime(m_nCurrX, m_nCurrY, this);// 刷新在地图上位置的时间
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg4);
                M2Share.ErrorMessage(e.Message);
            }
            try
            {
                boChg = false;
                boNeedRecalc = false;
                for (i = m_dwStatusArrTick.GetLowerBound(0); i <= m_dwStatusArrTick.GetUpperBound(0); i++)
                {
                    if ((m_wStatusTimeArr[i] > 0) && (m_wStatusTimeArr[i] < 60000))
                    {
                        if ((HUtil32.GetTickCount() - m_dwStatusArrTick[i]) > 1000)
                        {
                            m_wStatusTimeArr[i] -= 1;
                            m_dwStatusArrTick[i] += 1000;
                            if (m_wStatusTimeArr[i] == 0)
                            {
                                boChg = true;
                                switch (i)
                                {
                                    case grobal2.STATE_TRANSPARENT:
                                        m_boHideMode = false;
                                        break;
                                    case grobal2.STATE_DEFENCEUP:
                                        boNeedRecalc = true;
                                        SysMsg("Defense strength is back to normal.", TMsgColor.c_Green, TMsgType.t_Hint);
                                        break;
                                    case grobal2.STATE_MAGDEFENCEUP:
                                        boNeedRecalc = true;
                                        SysMsg("Magical defense strength is back to normal.", TMsgColor.c_Green, TMsgType.t_Hint);
                                        break;
                                    case grobal2.STATE_BUBBLEDEFENCEUP:
                                        m_boAbilMagBubbleDefence = false;
                                        break;
                                }
                            }
                        }
                    }
                }
                for (i = m_wStatusArrValue.GetLowerBound(0); i <= m_wStatusArrValue.GetUpperBound(0); i++)
                {
                    if (m_wStatusArrValue[i] > 0)
                    {
                        if (HUtil32.GetTickCount() > m_dwStatusArrTimeOutTick[i])
                        {
                            m_wStatusArrValue[i] = 0;
                            boNeedRecalc = true;
                            switch (i)
                            {
                                case 0:
                                    SysMsg("Removed temporarily increased destructive power.", TMsgColor.c_Green, TMsgType.t_Hint);
                                    break;
                                case 1:
                                    SysMsg("Removed temporarily increased magic power.", TMsgColor.c_Green, TMsgType.t_Hint);
                                    break;
                                case 2:
                                    SysMsg("Removed temporarily increased zen power.", TMsgColor.c_Green, TMsgType.t_Hint);
                                    break;
                                case 3:
                                    SysMsg("Removed temporarily increased hitting speed.", TMsgColor.c_Green, TMsgType.t_Hint);
                                    break;
                                case 4:
                                    SysMsg("Removed temporarily increased HP.", TMsgColor.c_Green, TMsgType.t_Hint);
                                    break;
                                case 5:
                                    SysMsg("Removed temporarily increased MP.", TMsgColor.c_Green, TMsgType.t_Hint);
                                    break;
                                case 6:
                                    SysMsg("Removed temporarily decreased attack ability.", TMsgColor.c_Green, TMsgType.t_Hint);
                                    break;
                            }
                        }
                    }
                }
                if (boChg)
                {
                    m_nCharStatus = GetCharStatus();
                    StatusChanged();
                }
                if (boNeedRecalc)
                {
                    RecalcAbilitys();
                    SendMsg(this, grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                }
            }
            catch (Exception)
            {
                M2Share.ErrorMessage(sExceptionMsg5);
            }
            try
            {
                if ((HUtil32.GetTickCount() - m_dwPoisoningTick) > M2Share.g_Config.dwPosionDecHealthTime)
                {
                    m_dwPoisoningTick = HUtil32.GetTickCount();
                    if (m_wStatusTimeArr[grobal2.POISON_DECHEALTH] > 0)
                    {
                        if (m_boAnimal)
                        {
                            m_nMeatQuality -= 1000;
                        }
                        DamageHealth(m_btGreenPoisoningPoint + 1);
                        m_nHealthTick = 0;
                        m_nSpellTick = 0;
                        HealthSpellChanged();
                    }
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg6);
            }
            M2Share.g_nBaseObjTimeMin = HUtil32.GetTickCount() - dwRunTick;
            if (M2Share.g_nBaseObjTimeMax < M2Share.g_nBaseObjTimeMin)
            {
                M2Share.g_nBaseObjTimeMax = M2Share.g_nBaseObjTimeMin;
            }
        }

        public bool GetFrontPosition(ref short nX, ref short nY)
        {
            bool result;
            TEnvirnoment Envir = m_PEnvir;
            nX = m_nCurrX;
            nY = m_nCurrY;
            switch (m_btDirection)
            {
                case grobal2.DR_UP:
                    if (nY > 0)
                    {
                        nY -= 1;
                    }
                    break;
                case grobal2.DR_UPRIGHT:
                    if ((nX < (Envir.wWidth - 1)) && (nY > 0))
                    {
                        nX++;
                        nY -= 1;
                    }
                    break;
                case grobal2.DR_RIGHT:
                    if (nX < (Envir.wWidth - 1))
                    {
                        nX++;
                    }
                    break;
                case grobal2.DR_DOWNRIGHT:
                    if ((nX < (Envir.wWidth - 1)) && (nY < (Envir.wHeight - 1)))
                    {
                        nX++;
                        nY++;
                    }
                    break;
                case grobal2.DR_DOWN:
                    if (nY < (Envir.wHeight - 1))
                    {
                        nY++;
                    }
                    break;
                case grobal2.DR_DOWNLEFT:
                    if ((nX > 0) && (nY < (Envir.wHeight - 1)))
                    {
                        nX -= 1;
                        nY++;
                    }
                    break;
                case grobal2.DR_LEFT:
                    if (nX > 0)
                    {
                        nX -= 1;
                    }
                    break;
                case grobal2.DR_UPLEFT:
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

        public bool SpaceMove_GetRandXY(TEnvirnoment Envir, ref short nX, ref short nY)
        {
            bool result;
            int n14;
            short n18;
            int n1C;
            result = false;
            if (Envir.wWidth < 80)
            {
                n18 = 3;
            }
            else
            {
                n18 = 10;
            }
            if (Envir.wHeight < 150)
            {
                if (Envir.wHeight < 50)
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
                if (nX < (Envir.wWidth - n1C - 1))
                {
                    nX += n18;
                }
                else
                {
                    nX = (short)M2Share.RandomNumber.Random(Envir.wWidth);
                    if (nY < (Envir.wHeight - n1C - 1))
                    {
                        nY += n18;
                    }
                    else
                    {
                        nY = (short)M2Share.RandomNumber.Random(Envir.wHeight);
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
            TPlayObject PlayObject;
            TEnvirnoment Envir = M2Share.g_MapManager.FindMap(sMap);
            if (Envir != null)
            {
                if (M2Share.nServerIndex == Envir.nServerIndex)
                {
                    TEnvirnoment OldEnvir = m_PEnvir;
                    nOldX = m_nCurrX;
                    nOldY = m_nCurrY;
                    bo21 = false;
                    m_PEnvir.DeleteFromMap(m_nCurrX, m_nCurrY, grobal2.OS_MOVINGOBJECT, this);
                    m_VisibleHumanList.Clear();
                    for (int i = 0; i < m_VisibleItems.Count; i++)
                    {
                        m_VisibleItems[i] = null;
                    }
                    m_VisibleItems.Clear();
                    for (int i = 0; i < m_VisibleActors.Count; i++)
                    {
                        m_VisibleActors[i] = null;
                    }
                    m_VisibleActors.Clear();
                    m_VisibleEvents.Clear();// 01/21 移动时清除列表
                    m_PEnvir = Envir;
                    m_sMapName = Envir.sMapName;
                    m_sMapFileName = Envir.m_sMapFileName;
                    m_nCurrX = nX;
                    m_nCurrY = nY;
                    if (SpaceMove_GetRandXY(m_PEnvir, ref m_nCurrX, ref m_nCurrY))
                    {
                        m_PEnvir.AddToMap(m_nCurrX, m_nCurrY, grobal2.OS_MOVINGOBJECT, this);
                        SendMsg(this, grobal2.RM_CLEAROBJECTS, 0, 0, 0, 0, "");
                        SendMsg(this, grobal2.RM_CHANGEMAP, 0, 0, 0, 0, m_sMapFileName);
                        if (nInt == 1)
                        {
                            SendRefMsg(grobal2.RM_SPACEMOVE_SHOW2, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
                        }
                        else
                        {
                            SendRefMsg(grobal2.RM_SPACEMOVE_SHOW, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
                        }
                        m_dwMapMoveTick = HUtil32.GetTickCount();
                        m_bo316 = true;
                        bo21 = true;
                    }
                    if (!bo21)
                    {
                        m_PEnvir = OldEnvir;
                        m_nCurrX = (short)nOldX;
                        m_nCurrY = (short)nOldY;
                        m_PEnvir.AddToMap(m_nCurrX, m_nCurrY, grobal2.OS_MOVINGOBJECT, this);
                    }
                }
                else
                {
                    if (SpaceMove_GetRandXY(Envir, ref nX, ref nY))
                    {
                        if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                        {
                            DisappearA();
                            m_bo316 = true;
                            PlayObject = this as TPlayObject;
                            PlayObject.m_sSwitchMapName = Envir.sMapName;
                            PlayObject.m_nSwitchMapX = nX;
                            PlayObject.m_nSwitchMapY = nY;
                            PlayObject.m_boSwitchData = true;
                            PlayObject.m_nServerIndex = Envir.nServerIndex;
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
            SendRefMsg(grobal2.RM_USERNAME, 0, 0, 0, 0, GetShowName());
        }

        public virtual bool Operate(TProcessMessage ProcessMsg)
        {
            int nDamage;
            int nTargetX;
            int nTargetY;
            int nPower;
            int nRage;
            TBaseObject TargetBaseObject;
            const string sExceptionMsg = "[Exception] TBaseObject::Operate ";
            bool result = false;
            try
            {
                switch (ProcessMsg.wIdent)
                {
                    case grobal2.RM_MAGSTRUCK:
                    case grobal2.RM_MAGSTRUCK_MINE:
                        if ((ProcessMsg.wIdent == grobal2.RM_MAGSTRUCK) && (m_btRaceServer >= grobal2.RC_ANIMAL) && !bo2BF && (m_Abil.Level < 50))
                        {
                            m_dwWalkTick = m_dwWalkTick + 800 + M2Share.RandomNumber.Random(1000);
                        }
                        nDamage = GetMagStruckDamage(null, ProcessMsg.nParam1);
                        if (nDamage > 0)
                        {
                            StruckDamage(nDamage);
                            HealthSpellChanged();
                            SendRefMsg(grobal2.RM_STRUCK_MAG, (short)nDamage, m_WAbil.HP, m_WAbil.MaxHP, ProcessMsg.BaseObject, "");
                            TargetBaseObject = M2Share.ObjectSystem.Get(ProcessMsg.BaseObject);// M2Share.ObjectSystem.Get(ProcessMsg.BaseObject);
                            if (M2Share.g_Config.boMonDelHptoExp)
                            {
                                if (TargetBaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT)
                                {
                                    if ((TargetBaseObject as TPlayObject).m_WAbil.Level <= M2Share.g_Config.MonHptoExpLevel)
                                    {
                                        if (!M2Share.GetNoHptoexpMonList(m_sCharName))
                                        {
                                            (TargetBaseObject as TPlayObject).GainExp(GetMagStruckDamage(TargetBaseObject, nDamage) * M2Share.g_Config.MonHptoExpmax);
                                        }
                                    }
                                }
                                if (TargetBaseObject.m_btRaceServer == grobal2.RC_PLAYCLONE)
                                {
                                    if (TargetBaseObject.m_Master != null)
                                    {
                                        if ((TargetBaseObject.m_Master as TPlayObject).m_WAbil.Level <= M2Share.g_Config.MonHptoExpLevel)
                                        {
                                            if (!M2Share.GetNoHptoexpMonList(m_sCharName))
                                            {
                                                (TargetBaseObject.m_Master as TPlayObject).GainExp(GetMagStruckDamage(TargetBaseObject, nDamage) * M2Share.g_Config.MonHptoExpmax);
                                            }
                                        }
                                    }
                                }
                            }
                            if (m_btRaceServer != grobal2.RC_PLAYOBJECT)
                            {
                                if (m_boAnimal)
                                {
                                    m_nMeatQuality -= nDamage * 1000;
                                }
                                SendMsg(this, grobal2.RM_STRUCK, (short)nDamage, m_WAbil.HP, m_WAbil.MaxHP, ProcessMsg.BaseObject, "");
                            }
                        }
                        if (m_boFastParalysis)
                        {
                            m_wStatusTimeArr[grobal2.POISON_STONE] = 1;
                            m_boFastParalysis = false;
                        }
                        break;
                    case grobal2.RM_MAGHEALING:
                        if ((m_nIncHealing + ProcessMsg.nParam1) < 300)
                        {
                            if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                            {
                                m_nIncHealing += ProcessMsg.nParam1;
                                m_nPerHealing = 5;
                            }
                            else
                            {
                                m_nIncHealing += ProcessMsg.nParam1;
                                m_nPerHealing = 5;
                            }
                        }
                        else
                        {
                            m_nIncHealing = 300;
                        }
                        break;
                    case grobal2.RM_10101:
                        SendRefMsg(ProcessMsg.BaseObject, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.sMsg);
                        if ((ProcessMsg.BaseObject == grobal2.RM_STRUCK) && (m_btRaceServer != grobal2.RC_PLAYOBJECT))
                        {
                            SendMsg(this, ProcessMsg.BaseObject, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.sMsg);
                        }
                        if (m_boFastParalysis)
                        {
                            m_wStatusTimeArr[grobal2.POISON_STONE] = 1;
                            m_boFastParalysis = false;
                        }
                        break;
                    case grobal2.RM_DELAYMAGIC:
                        nPower = ProcessMsg.wParam;
                        nTargetX = HUtil32.LoWord(ProcessMsg.nParam1);
                        nTargetY = HUtil32.HiWord(ProcessMsg.nParam1);
                        nRage = ProcessMsg.nParam2;
                        TargetBaseObject = M2Share.ObjectSystem.Get(ProcessMsg.nParam3);// M2Share.ObjectSystem.Get(ProcessMsg.nParam3);
                        if ((TargetBaseObject != null) && (TargetBaseObject.GetMagStruckDamage(this, nPower) > 0))
                        {
                            SetTargetCreat(TargetBaseObject);
                            if (TargetBaseObject.m_btRaceServer >= grobal2.RC_ANIMAL)
                            {
                                nPower = HUtil32.Round(nPower / 1.2);
                            }
                            if ((Math.Abs(nTargetX - TargetBaseObject.m_nCurrX) <= nRage) && (Math.Abs(nTargetY - TargetBaseObject.m_nCurrY) <= nRage))
                            {
                                TargetBaseObject.SendMsg(this, grobal2.RM_MAGSTRUCK, 0, nPower, 0, 0, "");
                            }
                        }
                        break;
                    case grobal2.RM_10155:
                        MapRandomMove(ProcessMsg.sMsg, ProcessMsg.wParam);
                        break;
                    case grobal2.RM_DELAYPUSHED:
                        nPower = ProcessMsg.wParam;
                        nTargetX = HUtil32.LoWord(ProcessMsg.nParam1);
                        nTargetY = HUtil32.HiWord(ProcessMsg.nParam1);
                        nRage = ProcessMsg.nParam2;
                        TargetBaseObject = M2Share.ObjectSystem.Get(ProcessMsg.nParam3);// M2Share.ObjectSystem.Get(ProcessMsg.nParam3);
                        if (TargetBaseObject != null)
                        {
                            TargetBaseObject.CharPushed((byte)nPower, nRage);
                        }
                        break;
                    case grobal2.RM_POISON:
                        TargetBaseObject = M2Share.ObjectSystem.Get(ProcessMsg.nParam2);// ((ProcessMsg.nParam2) as TBaseObject);
                        if (TargetBaseObject != null)
                        {
                            if (IsProperTarget(TargetBaseObject))
                            {
                                SetTargetCreat(TargetBaseObject);
                                if ((m_btRaceServer == grobal2.RC_PLAYOBJECT) && (TargetBaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT))
                                {
                                    SetPKFlag(TargetBaseObject);
                                }
                                SetLastHiter(TargetBaseObject);
                            }
                            MakePosion(ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam3);// 中毒类型
                        }
                        else
                        {
                            MakePosion(ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam3);// 中毒类型
                        }
                        break;
                    case grobal2.RM_TRANSPARENT:
                        M2Share.MagicManager.MagMakePrivateTransparent(this, ProcessMsg.nParam1);
                        break;
                    case grobal2.RM_DOOPENHEALTH:
                        MakeOpenHealth();
                        break;
                    default:
                        Debug.WriteLine(format("人物: {0} 消息: Ident {1} Param {2} P1 {3} P2 {3} P3 {4} Msg {5}", m_sCharName, ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.sMsg));
                        break;
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.Message);
            }
            return result;
        }

        public TBaseObject MakeSlave(string sMonName, int nMakeLevel, int nExpLevel, int nMaxMob, int dwRoyaltySec)
        {
            short nX = 0;
            short nY = 0;
            TBaseObject MonObj;
            TBaseObject result = null;
            if (m_SlaveList.Count < nMaxMob)
            {
                GetFrontPosition(ref nX, ref nY);
                MonObj = M2Share.UserEngine.RegenMonsterByName(m_PEnvir.sMapName, nX, nY, sMonName);
                if (MonObj != null)
                {
                    MonObj.m_Master = this;
                    MonObj.m_dwMasterRoyaltyTick = HUtil32.GetTickCount() + dwRoyaltySec * 1000;
                    MonObj.m_btSlaveMakeLevel = (byte)nMakeLevel;
                    MonObj.m_btSlaveExpLevel = (byte)nExpLevel;
                    MonObj.RecalcAbilitys();
                    if (MonObj.m_WAbil.HP < MonObj.m_WAbil.MaxHP)
                    {
                        MonObj.m_WAbil.HP = (short)(MonObj.m_WAbil.HP + (MonObj.m_WAbil.MaxHP - MonObj.m_WAbil.HP) / 2);
                    }
                    MonObj.RefNameColor();
                    m_SlaveList.Add(MonObj);
                    result = MonObj;
                }
            }
            return result;
        }

        public void MapRandomMove(string sMapName, int nInt)
        {
            int nEgdey;
            TEnvirnoment Envir = M2Share.g_MapManager.FindMap(sMapName);
            if (Envir != null)
            {
                if (Envir.wHeight < 150)
                {
                    if (Envir.wHeight < 30)
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
                short nX = (short)(M2Share.RandomNumber.Random(Envir.wWidth - nEgdey - 1) + nEgdey);
                short nY = (short)(M2Share.RandomNumber.Random(Envir.wHeight - nEgdey - 1) + nEgdey);
                SpaceMove(sMapName, nX, nY, nInt);
            }
        }

        public bool AddItemToBag(TUserItem UserItem)
        {
            bool result = false;
            if (m_ItemList.Count < grobal2.MAXBAGITEM)
            {
                m_ItemList.Add(UserItem);
                WeightChanged();
                result = true;
            }
            return result;
        }

        public void sub_4C713C(TUserMagic Magic)
        {
            if (Magic.MagicInfo.wMagicID == 28)
            {
                if (Magic.btLevel >= 2)
                {
                    m_boAbilSeeHealGauge = true;
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
            int result;
            int n10;
            int n14;
            result = 0;
            nFlag -= 1;
            if (nFlag < 0)
            {
                return result;
            }
            n10 = nFlag / 8;
            n14 = nFlag % 8;
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
            int n10;
            int n14;
            byte bt15;
            nFlag -= 1;
            if (nFlag < 0)
            {
                return;
            }
            n10 = nFlag / 8;
            n14 = nFlag % 8;
            if ((n10 - m_QuestUnitOpen.Length) < 0)
            {
                bt15 = m_QuestUnitOpen[n10];
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
            int n10;
            int n14;
            byte bt15;
            nFlag -= 1;
            if (nFlag < 0)
            {
                return;
            }
            n10 = nFlag / 8;
            n14 = nFlag % 8;
            if ((n10 - m_QuestUnit.Length) < 0)
            {
                bt15 = m_QuestUnit[n10];
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

        public bool KillFunc()
        {
            const string sExceptionMsg = "[Exception] TBaseObject::KillFunc";
            bool result = false;
            try
            {
                if ((M2Share.g_FunctionNPC != null) && (m_PEnvir != null) && m_PEnvir.Flag.boKILLFUNC)
                {
                    if (m_btRaceServer != grobal2.RC_PLAYOBJECT)
                    {
                        if (m_ExpHitter != null)
                        {
                            if (m_ExpHitter.m_btRaceServer == grobal2.RC_PLAYOBJECT)
                            {
                                M2Share.g_FunctionNPC.GotoLable(m_ExpHitter as TPlayObject, "@KillPlayMon" + m_PEnvir.Flag.nKILLFUNCNO.ToString(), false);
                            }
                            if (m_ExpHitter.m_Master != null)
                            {
                                M2Share.g_FunctionNPC.GotoLable(m_ExpHitter.m_Master as TPlayObject, "@KillPlayMon" + m_PEnvir.Flag.nKILLFUNCNO.ToString(), false);
                            }
                        }
                        else
                        {
                            if (m_LastHiter != null)
                            {
                                if (m_LastHiter.m_btRaceServer == grobal2.RC_PLAYOBJECT)
                                {
                                    M2Share.g_FunctionNPC.GotoLable(m_LastHiter as TPlayObject, "@KillPlayMon" + m_PEnvir.Flag.nKILLFUNCNO.ToString(), false);
                                }
                                if (m_LastHiter.m_Master != null)
                                {
                                    M2Share.g_FunctionNPC.GotoLable(m_LastHiter.m_Master as TPlayObject, "@KillPlayMon" + m_PEnvir.Flag.nKILLFUNCNO.ToString(), false);
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((m_LastHiter != null) && (m_LastHiter.m_btRaceServer == grobal2.RC_PLAYOBJECT))
                        {
                            M2Share.g_FunctionNPC.GotoLable(m_LastHiter as TPlayObject, "@KillPlay" + m_PEnvir.Flag.nKILLFUNCNO.ToString(), false);
                        }
                    }
                    result = true;
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.Message);
            }
            return result;
        }

        private void UseLamp()
        {
            int nOldDura;
            int nDura;
            TPlayObject PlayObject;
            TItem Stditem;
            const string sExceptionMsg = "[Exception] TBaseObject::UseLamp";
            try
            {
                if (m_UseItems[grobal2.U_RIGHTHAND] != null && m_UseItems[grobal2.U_RIGHTHAND].wIndex > 0)
                {
                    Stditem = M2Share.UserEngine.GetStdItem(m_UseItems[grobal2.U_RIGHTHAND].wIndex);
                    if ((Stditem == null) || (Stditem.Source != 0))
                    {
                        return;
                    }
                    nOldDura = HUtil32.Round(m_UseItems[grobal2.U_RIGHTHAND].Dura / 1000);
                    if (M2Share.g_Config.boDecLampDura)
                    {
                        nDura = m_UseItems[grobal2.U_RIGHTHAND].Dura - 1;
                    }
                    else
                    {
                        nDura = m_UseItems[grobal2.U_RIGHTHAND].Dura;
                    }
                    if (nDura <= 0)
                    {
                        m_UseItems[grobal2.U_RIGHTHAND].Dura = 0;
                        if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                        {
                            PlayObject = this as TPlayObject;
                            PlayObject.SendDelItems(m_UseItems[grobal2.U_RIGHTHAND]);
                        }
                        m_UseItems[grobal2.U_RIGHTHAND].wIndex = 0;
                        m_nLight = 0;
                        SendRefMsg(grobal2.RM_CHANGELIGHT, 0, 0, 0, 0, "");
                        SendMsg(this, grobal2.RM_LAMPCHANGEDURA, 0, m_UseItems[grobal2.U_RIGHTHAND].Dura, 0, 0, "");
                        RecalcAbilitys();
                        // FeatureChanged(); 01/21 取消 蜡烛是本人才可以看到的，不需要发送广播信息
                    }
                    else
                    {
                        m_UseItems[grobal2.U_RIGHTHAND].Dura = (short)nDura;
                    }
                    if (nOldDura != HUtil32.Round(nDura / 1000))
                    {
                        SendMsg(this, grobal2.RM_LAMPCHANGEDURA, 0, m_UseItems[grobal2.U_RIGHTHAND].Dura, 0, 0, "");
                    }
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
        }

        public TBaseObject GetPoseCreate()
        {
            short nX = 0;
            short nY = 0;
            TBaseObject result = null;
            if (GetFrontPosition(ref nX, ref nY))
            {
                result = (TBaseObject)m_PEnvir.GetMovingObject(nX, nY, true);
            }
            return result;
        }

        public bool GetAttackDir(TBaseObject BaseObject, ref byte btDir)
        {
            bool result = false;
            if ((m_nCurrX - 1 <= BaseObject.m_nCurrX) && (m_nCurrX + 1 >= BaseObject.m_nCurrX) && (m_nCurrY - 1 <= BaseObject.m_nCurrY) && (m_nCurrY + 1 >= BaseObject.m_nCurrY) && ((m_nCurrX != BaseObject.m_nCurrX) || (m_nCurrY != BaseObject.m_nCurrY)))
            {
                result = true;
                if (((m_nCurrX - 1) == BaseObject.m_nCurrX) && (m_nCurrY == BaseObject.m_nCurrY))
                {
                    btDir = grobal2.DR_LEFT;
                    return result;
                }
                if (((m_nCurrX + 1) == BaseObject.m_nCurrX) && (m_nCurrY == BaseObject.m_nCurrY))
                {
                    btDir = grobal2.DR_RIGHT;
                    return result;
                }
                if ((m_nCurrX == BaseObject.m_nCurrX) && ((m_nCurrY - 1) == BaseObject.m_nCurrY))
                {
                    btDir = grobal2.DR_UP;
                    return result;
                }
                if ((m_nCurrX == BaseObject.m_nCurrX) && ((m_nCurrY + 1) == BaseObject.m_nCurrY))
                {
                    btDir = grobal2.DR_DOWN;
                    return result;
                }
                if (((m_nCurrX - 1) == BaseObject.m_nCurrX) && ((m_nCurrY - 1) == BaseObject.m_nCurrY))
                {
                    btDir = grobal2.DR_UPLEFT;
                    return result;
                }
                if (((m_nCurrX + 1) == BaseObject.m_nCurrX) && ((m_nCurrY - 1) == BaseObject.m_nCurrY))
                {
                    btDir = grobal2.DR_UPRIGHT;
                    return result;
                }
                if (((m_nCurrX - 1) == BaseObject.m_nCurrX) && ((m_nCurrY + 1) == BaseObject.m_nCurrY))
                {
                    btDir = grobal2.DR_DOWNLEFT;
                    return result;
                }
                if (((m_nCurrX + 1) == BaseObject.m_nCurrX) && ((m_nCurrY + 1) == BaseObject.m_nCurrY))
                {
                    btDir = grobal2.DR_DOWNRIGHT;
                    return result;
                }
                btDir = 0;
            }
            return result;
        }

        public bool TargetInSpitRange(TBaseObject BaseObject, ref byte btDir)
        {
            bool result = false;
            if ((Math.Abs(BaseObject.m_nCurrX - m_nCurrX) <= 2) && (Math.Abs(BaseObject.m_nCurrY - m_nCurrY) <= 2))
            {
                int nX = BaseObject.m_nCurrX - m_nCurrX;
                int nY = BaseObject.m_nCurrY - m_nCurrY;
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
                    btDir = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, BaseObject.m_nCurrX, BaseObject.m_nCurrY);
                    if (M2Share.g_Config.SpitMap[btDir, nY, nX] == 1)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        private int RecalcBagWeight()
        {
            int result = 0;
            TUserItem UserItem;
            TItem StdItem;
            for (int i = 0; i < m_ItemList.Count; i++)
            {
                UserItem = m_ItemList[i];
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (StdItem != null)
                {
                    result += StdItem.Weight;
                }
            }
            return result;
        }

        private void RecalcHitSpeed()
        {
            TUserMagic UserMagic;
            TNakedAbility BonusTick = null;
            switch (m_btJob)
            {
                case M2Share.jWarr:
                    BonusTick = M2Share.g_Config.BonusAbilofWarr;
                    break;
                case M2Share.jWizard:
                    BonusTick = M2Share.g_Config.BonusAbilofWizard;
                    break;
                case M2Share.jTaos:
                    BonusTick = M2Share.g_Config.BonusAbilofTaos;
                    break;
            }
            m_btHitPoint = (byte)(M2Share.DEFHIT + m_BonusAbil.Hit / BonusTick.Hit);
            switch (m_btJob)
            {
                case M2Share.jTaos:
                    m_btSpeedPoint = (byte)(M2Share.DEFSPEED + m_BonusAbil.Speed / BonusTick.Speed + 3);
                    break;
                default:
                    m_btSpeedPoint = (byte)(M2Share.DEFSPEED + m_BonusAbil.Speed / BonusTick.Speed);
                    break;
            }
            m_nHitPlus = 0;
            m_nHitDouble = 0;
            m_MagicOneSwordSkill = null;
            m_MagicPowerHitSkill = null;
            m_MagicErgumSkill = null;
            m_MagicBanwolSkill = null;
            m_MagicRedBanwolSkill = null;
            m_MagicFireSwordSkill = null;
            m_MagicCrsSkill = null;
            m_Magic41Skill = null;
            m_MagicTwnHitSkill = null;
            m_Magic43Skill = null;
            for (int i = 0; i < m_MagicList.Count; i++)
            {
                UserMagic = m_MagicList[i];
                switch (UserMagic.wMagIdx)
                {
                    case grobal2.SKILL_ONESWORD:// 内功心法
                        m_MagicOneSwordSkill = UserMagic;
                        if (UserMagic.btLevel > 0)
                        {
                            m_btHitPoint = (byte)(m_btHitPoint + HUtil32.Round(9 / 3 * UserMagic.btLevel));
                        }
                        break;
                    case grobal2.SKILL_YEDO:// 攻杀剑法
                        m_MagicPowerHitSkill = UserMagic;
                        if (UserMagic.btLevel > 0)
                        {
                            m_btHitPoint = (byte)(m_btHitPoint + HUtil32.Round(3 / 3 * UserMagic.btLevel));
                        }
                        m_nHitPlus = (byte)(M2Share.DEFHIT + UserMagic.btLevel);
                        m_btAttackSkillCount = (byte)(7 - UserMagic.btLevel);
                        m_btAttackSkillPointCount = (byte)M2Share.RandomNumber.Random(m_btAttackSkillCount);
                        break;
                    case grobal2.SKILL_ERGUM:// 刺杀剑法
                        m_MagicErgumSkill = UserMagic;
                        break;
                    case grobal2.SKILL_BANWOL:// 半月弯刀
                        m_MagicBanwolSkill = UserMagic;
                        break;
                    case grobal2.SKILL_REDBANWOL:
                        m_MagicRedBanwolSkill = UserMagic;
                        break;
                    case grobal2.SKILL_FIRESWORD:// 烈火剑法
                        m_MagicFireSwordSkill = UserMagic;
                        m_nHitDouble = (byte)(4 + UserMagic.btLevel * 4);
                        break;
                    case grobal2.SKILL_ILKWANG:// 基本剑法
                        m_MagicOneSwordSkill = UserMagic;
                        if (UserMagic.btLevel > 0)
                        {
                            m_btHitPoint = (byte)(m_btHitPoint + HUtil32.Round(8 / 3 * UserMagic.btLevel));
                        }
                        break;
                    case grobal2.SKILL_CROSSMOON:
                        m_MagicCrsSkill = UserMagic;
                        break;
                    case 41:
                        m_Magic41Skill = UserMagic;
                        break;
                    case grobal2.SKILL_TWINBLADE:
                        m_MagicTwnHitSkill = UserMagic;
                        break;
                    case 43:
                        m_Magic43Skill = UserMagic;
                        break;
                }
            }
        }

        public void AddItemSkill(int nIndex)
        {
            TMagic Magic = null;
            TUserMagic UserMagic;
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
                    UserMagic = new TUserMagic
                    {
                        MagicInfo = Magic,
                        wMagIdx = Magic.wMagicID,
                        btKey = 0,
                        btLevel = 1,
                        nTranPoint = 0
                    };
                    m_MagicList.Add(UserMagic);
                    if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                    {
                        (this as TPlayObject).SendAddMagic(UserMagic);
                    }
                }
            }
        }

        private bool AddToMap()
        {
            bool result;
            object Point = m_PEnvir.AddToMap(m_nCurrX, m_nCurrY, grobal2.OS_MOVINGOBJECT, this);
            if (Point != null)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            if (!m_boFixedHideMode)
            {
                SendRefMsg(grobal2.RM_TURN, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
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
            return (short)HUtil32.Round(UserMagic.MagicInfo.wSpell / (UserMagic.MagicInfo.btTrainLv + 1) * (UserMagic.btLevel + 1));
        }

        /// <summary>
        /// 武器升级设置
        /// </summary>
        /// <param name="UserItem"></param>
        public void AttackDir_CheckWeaponUpgradeStatus(TUserItem UserItem)
        {
            if ((UserItem.btValue[0] + UserItem.btValue[1] + UserItem.btValue[2]) < M2Share.g_Config.nUpgradeWeaponMaxPoint)
            {
                if (UserItem.btValue[10] == 1)
                {
                    UserItem.wIndex = 0;
                }
                if (HUtil32.RangeInDefined(UserItem.btValue[10], 10, 13))
                { 
                        UserItem.btValue[0] = (byte)(UserItem.btValue[0] + UserItem.btValue[10] - 9);
                }
                if (HUtil32.RangeInDefined(UserItem.btValue[10], 20, 23))
                {
                    UserItem.btValue[1] = (byte)(UserItem.btValue[1] + UserItem.btValue[10] - 19);
                }
                if (HUtil32.RangeInDefined(UserItem.btValue[10], 30, 33))
                {
                    UserItem.btValue[2] = (byte)(UserItem.btValue[2] + UserItem.btValue[10] - 29);
                }
            }
            else
            {
                UserItem.wIndex = 0;
            }
            UserItem.btValue[10] = 0;
        }

        public void AttackDir_CheckWeaponUpgrade()
        {
            TUserItem UseItems;
            TPlayObject PlayObject;
            TItem StdItem;
            if (m_UseItems[grobal2.U_WEAPON].btValue[10] > 0)
            {
                UseItems = m_UseItems[grobal2.U_WEAPON];
                AttackDir_CheckWeaponUpgradeStatus(m_UseItems[grobal2.U_WEAPON]);
                if (m_UseItems[grobal2.U_WEAPON].wIndex == 0)
                {
                    SysMsg(M2Share.g_sTheWeaponBroke, TMsgColor.c_Red, TMsgType.t_Hint);
                    PlayObject = this as TPlayObject;
                    PlayObject.SendDelItems(UseItems);
                    // PlayObject.StatusChanged;
                    SendRefMsg(grobal2.RM_BREAKWEAPON, 0, 0, 0, 0, "");
                    StdItem = M2Share.UserEngine.GetStdItem(UseItems.wIndex);
                    if (StdItem.NeedIdentify == 1)
                    {
                        // UserEngine.GetStdItemName(UseItems.wIndex) + #9 +
                        M2Share.AddGameDataLog("21" + "\t" + m_sMapName + "\t" + m_nCurrX.ToString() + "\t" + m_nCurrY.ToString() + "\t" + m_sCharName + "\t" + StdItem.Name + "\t" + UseItems.MakeIndex.ToString() + "\t" + '1' + "\t" + '0');
                    }
                    FeatureChanged();
                }
                else
                {
                    SysMsg(M2Share.sTheWeaponRefineSuccessfull, TMsgColor.c_Red, TMsgType.t_Hint);
                    PlayObject = this as TPlayObject;
                    PlayObject.SendUpdateItem(m_UseItems[grobal2.U_WEAPON]);
                    StdItem = M2Share.UserEngine.GetStdItem(UseItems.wIndex);
                    if (StdItem.NeedIdentify == 1)
                    {
                        // UserEngine.GetStdItemName(UseItems.wIndex) + #9 +
                        M2Share.AddGameDataLog("20" + "\t" + m_sMapName + "\t" + m_nCurrX.ToString() + "\t" + m_nCurrY.ToString() + "\t" + m_sCharName + "\t" + StdItem.Name + "\t" + UseItems.MakeIndex.ToString() + "\t" + '1' + "\t" + '0');
                    }
                    RecalcAbilitys();
                    SendMsg(this, grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                    SendMsg(this, grobal2.RM_SUBABILITY, 0, 0, 0, 0, "");
                }
            }
        }

        public virtual void AttackDir(TBaseObject TargeTBaseObject, short wHitMode, byte nDir)
        {
            TBaseObject AttackTarget;
            bool boPowerHit;
            bool boFireHit;
            bool boCrsHit;
            bool bo41;
            bool boTwinHit;
            bool bo43;
            short wIdent;
            const string sExceptionMsg = "[Exception] TBaseObject::AttackDir";
            try
            {
                if ((wHitMode == 5) && (m_MagicBanwolSkill != null)) // 半月
                {
                    if (m_WAbil.MP > 0)
                    {
                        DamageSpell((short)(m_MagicBanwolSkill.MagicInfo.btDefSpell + GetMagicSpell(m_MagicBanwolSkill)));
                        HealthSpellChanged();
                    }
                    else
                    {
                        wHitMode = grobal2.RM_HIT;
                    }
                }
                if ((wHitMode == 12) && (m_MagicRedBanwolSkill != null))
                {
                    if (m_WAbil.MP > 0)
                    {
                        DamageSpell((short)(m_MagicRedBanwolSkill.MagicInfo.btDefSpell + GetMagicSpell(m_MagicRedBanwolSkill)));
                        HealthSpellChanged();
                    }
                    else
                    {
                        wHitMode = grobal2.RM_HIT;
                    }
                }
                if ((wHitMode == 8) && (m_MagicCrsSkill != null))
                {
                    if (m_WAbil.MP > 0)
                    {
                        DamageSpell((short)(m_MagicCrsSkill.MagicInfo.btDefSpell + GetMagicSpell(m_MagicCrsSkill)));
                        HealthSpellChanged();
                    }
                    else
                    {
                        wHitMode = grobal2.RM_HIT;
                    }
                }

                m_btDirection = nDir;
                if (TargeTBaseObject == null)
                {
                    AttackTarget = GetPoseCreate();
                }
                else
                {
                    AttackTarget = TargeTBaseObject;
                }
                if ((AttackTarget != null) && (m_UseItems[grobal2.U_WEAPON] != null) && (m_UseItems[grobal2.U_WEAPON].wIndex > 0))
                {
                    AttackDir_CheckWeaponUpgrade();
                }

                boPowerHit = m_boPowerHit;
                boFireHit = m_boFireHitSkill;
                boCrsHit = m_boCrsHitkill;
                bo41 = m_bo41kill;
                boTwinHit = m_boTwinHitSkill;
                bo43 = m_bo43kill;
                if (_Attack(ref wHitMode, AttackTarget))
                {
                    SetTargetCreat(AttackTarget);
                }
                wIdent = grobal2.RM_HIT;
                if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                {
                    switch (wHitMode)
                    {
                        case 0:
                            wIdent = grobal2.RM_HIT;
                            break;
                        case 1:
                            wIdent = grobal2.RM_HEAVYHIT;
                            break;
                        case 2:
                            wIdent = grobal2.RM_BIGHIT;
                            break;
                        case 3:
                            if (boPowerHit)
                            {
                                wIdent = grobal2.RM_SPELL2;
                            }
                            break;
                        case 4:
                            if (m_MagicErgumSkill != null)
                            {
                                wIdent = grobal2.RM_LONGHIT;
                            }
                            break;
                        case 5:
                            if (m_MagicBanwolSkill != null)
                            {
                                wIdent = grobal2.RM_WIDEHIT;
                            }
                            break;
                        case 7:
                            if (boFireHit)
                            {
                                wIdent = grobal2.RM_FIREHIT;
                            }
                            break;
                        case 8:
                            if (m_MagicCrsSkill != null)
                            {
                                wIdent = grobal2.RM_CRSHIT;
                            }
                            break;
                        case 9:
                            if (boTwinHit)
                            {
                                wIdent = grobal2.RM_TWINHIT;
                            }
                            break;
                        case 12:
                            if (m_MagicRedBanwolSkill != null)
                            {
                                wIdent = grobal2.RM_WIDEHIT;
                            }
                            break;
                    }
                }

                SendAttackMsg(wIdent, m_btDirection, m_nCurrX, m_nCurrY);
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.Message);
            }
        }

        private void CheckPKStatus()
        {
            if (m_boPKFlag && ((HUtil32.GetTickCount() - m_dwPKTick) > M2Share.g_Config.dwPKFlagTime))// 60 * 1000
            {
                m_boPKFlag = false;
                RefNameColor();
            }
        }

        /// <summary>
        /// 减少魔法值
        /// </summary>
        /// <param name="nSpellPoint"></param>
        public void DamageSpell(short nSpellPoint)
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

        private void DecPKPoint(int nPoint)
        {
            int nC = PKLevel();
            m_nPkPoint -= nPoint;
            if (m_nPkPoint < 0)
            {
                m_nPkPoint = 0;
            }
            if ((PKLevel() != nC) && (nC > 0) && (nC <= 2))
            {
                RefNameColor();
            }
        }

        public void DelItemSkill_DeleteSkill(string sSkillName)
        {
            TUserMagic UserMagic;
            TPlayObject PlayObject;
            for (int i = 0; i < m_MagicList.Count; i++)
            {
                UserMagic = m_MagicList[i];
                if (UserMagic.MagicInfo.sMagicName == sSkillName)
                {
                    PlayObject = this as TPlayObject;
                    PlayObject.SendDelMagic(UserMagic);
                    UserMagic = null;
                    m_MagicList.RemoveAt(i);
                    break;
                }
            }
        }

        public void DelItemSkill(int nIndex)
        {
            if (m_btRaceServer != grobal2.RC_PLAYOBJECT)
            {
                return;
            }
            switch (nIndex)
            {
                case 1:
                    if (m_btJob != M2Share.jWizard)
                    {
                        DelItemSkill_DeleteSkill(M2Share.g_Config.sFireBallSkill);
                    }
                    break;
                case 2:
                    if (m_btJob != M2Share.jTaos)
                    {
                        DelItemSkill_DeleteSkill(M2Share.g_Config.sHealSkill);
                    }
                    break;
            }
        }

        public void DelMember(TBaseObject BaseObject)
        {
            TPlayObject PlayObject;
            if (m_GroupOwner != BaseObject)
            {
                for (int i = 0; i < m_GroupMembers.Count; i++)
                {
                    if (m_GroupMembers[i] == BaseObject)
                    {
                        BaseObject.LeaveGroup();
                        m_GroupMembers.RemoveAt(i);
                        break;
                    }
                }
            }
            else
            {
                for (int i = m_GroupMembers.Count - 1; i >= 0; i--)
                {
                    m_GroupMembers[i].LeaveGroup();
                    m_GroupMembers.RemoveAt(i);
                }
            }
            PlayObject = this as TPlayObject;
            if (!PlayObject.CancelGroup())
            {
                PlayObject.SendDefMessage(grobal2.SM_GROUPCANCEL, 0, 0, 0, 0, "");
            }
            else
            {
                PlayObject.SendGroupMembers();
            }
        }

        public void DoDamageWeapon(int nWeaponDamage)
        {
            int nDura;
            int nDuraPoint;
            TPlayObject PlayObject;
            TItem StdItem;
            if (m_UseItems[grobal2.U_WEAPON] != null && m_UseItems[grobal2.U_WEAPON].wIndex <= 0)
            {
                return;
            }
            nDura = m_UseItems[grobal2.U_WEAPON].Dura;
            nDuraPoint = HUtil32.Round(nDura / 1.03);
            nDura -= nWeaponDamage;
            if (nDura <= 0)
            {
                nDura = 0;
                m_UseItems[grobal2.U_WEAPON].Dura = (short)nDura;
                if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                {
                    PlayObject = this as TPlayObject;
                    PlayObject.SendDelItems(m_UseItems[grobal2.U_WEAPON]);
                    StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[grobal2.U_WEAPON].wIndex);
                    if (StdItem.NeedIdentify == 1)
                    {
                        // UserEngine.GetStdItemName(m_UseItems[U_WEAPON].wIndex) + #9 +
                        M2Share.AddGameDataLog('3' + "\t" + m_sMapName + "\t" + m_nCurrX.ToString() + "\t" + m_nCurrY.ToString() + "\t" + m_sCharName + "\t" + StdItem.Name + "\t" + m_UseItems[grobal2.U_WEAPON].MakeIndex.ToString() + "\t" + HUtil32.BoolToIntStr(m_btRaceServer == grobal2.RC_PLAYOBJECT) + "\t" + '0');
                    }
                }
                m_UseItems[grobal2.U_WEAPON].wIndex = 0;
                SendMsg(this, grobal2.RM_DURACHANGE, grobal2.U_WEAPON, nDura, m_UseItems[grobal2.U_WEAPON].DuraMax, 0, "");
            }
            else
            {
                m_UseItems[grobal2.U_WEAPON].Dura = (short)nDura;
            }
            if ((nDura / 1.03) != nDuraPoint)
            {
                SendMsg(this, grobal2.RM_DURACHANGE, grobal2.U_WEAPON, m_UseItems[grobal2.U_WEAPON].Dura, m_UseItems[grobal2.U_WEAPON].DuraMax, 0, "");
            }
        }

        public byte GetCharColor(TBaseObject BaseObject)
        {
            int n10;
            TUserCastle Castle;
            byte result = BaseObject.GetNamecolor();
            if (BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT)
            {
                if (BaseObject.PKLevel() < 2)
                {
                    if (BaseObject.m_boPKFlag)
                    {
                        result = M2Share.g_Config.btPKFlagNameColor;
                    }
                    n10 = GetGuildRelation(this, BaseObject);
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
                    if (BaseObject.m_PEnvir.Flag.boFight3Zone)
                    {
                        if (m_MyGuild == BaseObject.m_MyGuild)
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
                if ((Castle != null) && Castle.m_boUnderWar && m_boInFreePKArea && BaseObject.m_boInFreePKArea)
                {
                    result = M2Share.g_Config.btInFreePKAreaNameColor;
                    m_boGuildWarArea = true;
                    if (m_MyGuild == null)
                    {
                        return result;
                    }
                    if (Castle.IsMasterGuild(m_MyGuild))
                    {
                        if ((m_MyGuild == BaseObject.m_MyGuild) || m_MyGuild.IsAllyGuild(BaseObject.m_MyGuild))
                        {
                            result = M2Share.g_Config.btAllyAndGuildNameColor;
                        }
                        else
                        {
                            if (Castle.IsAttackGuild(BaseObject.m_MyGuild))
                            {
                                result = M2Share.g_Config.btWarGuildNameColor;
                            }
                        }
                    }
                    else
                    {
                        if (Castle.IsAttackGuild(m_MyGuild))
                        {
                            if ((m_MyGuild == BaseObject.m_MyGuild) || m_MyGuild.IsAllyGuild(BaseObject.m_MyGuild))
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
            else
            {
                if (BaseObject.m_btSlaveExpLevel < grobal2.SLAVEMAXLEVEL)
                {
                    result = M2Share.g_Config.SlaveColor[BaseObject.m_btSlaveExpLevel];
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
            if (nLevel <= grobal2.MAXLEVEL)
            {
                result = M2Share.g_Config.dwNeedExps[nLevel];
            }
            else
            {
                result = M2Share.g_Config.dwNeedExps[M2Share.g_Config.dwNeedExps.GetUpperBound(0)];
            }
            return result;
        }

        private byte GetNamecolor()
        {
            byte result = m_btNameColor;
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
                SendMsg(null, grobal2.RM_HEAR, 0, M2Share.g_Config.btHearMsgFColor, M2Share.g_Config.btHearMsgBColor, 0, sMsg);
            }
        }

        public bool InSafeArea()
        {
            bool result = false;
            int n14;
            int n18;
            if (m_PEnvir == null)
            {
                return result;
            }
            result = m_PEnvir.Flag.boSAFE;
            if (result)
            {
                return result;
            }
            for (int i = 0; i < M2Share.StartPointList.Count; i++)
            {
                if (M2Share.StartPointList[i].m_sMapName == m_PEnvir.sMapName)
                {
                    if (M2Share.StartPointList[i] != null)
                    {
                        n14 = M2Share.StartPointList[i].m_nCurrX;
                        n18 = M2Share.StartPointList[i].m_nCurrY;
                        if ((Math.Abs(m_nCurrX - n14) <= 60) && (Math.Abs(m_nCurrY - n18) <= 60))
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        public void MonsterRecalcAbilitys()
        {
            m_WAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.DC), HUtil32.HiWord(m_Abil.DC));
            int n8 = 0;
            if ((m_btRaceServer == M2Share.MONSTER_WHITESKELETON) || (m_btRaceServer == M2Share.MONSTER_ELFMONSTER) || (m_btRaceServer == M2Share.MONSTER_ELFWARRIOR))
            {
                m_WAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.DC), HUtil32.Round((m_btSlaveExpLevel * 0.1 + 0.3) * 3.0 * m_btSlaveExpLevel + HUtil32.HiWord(m_WAbil.DC)));
                n8 = n8 + HUtil32.Round((m_btSlaveExpLevel * 0.1 + 0.3) * m_Abil.MaxHP) * m_btSlaveExpLevel;
                n8 = n8 + m_Abil.MaxHP;
                if (m_btSlaveExpLevel > 0)
                {
                    m_WAbil.MaxHP = (short)n8;
                }
                else
                {
                    m_WAbil.MaxHP = m_Abil.MaxHP;
                }
            }
            else
            {
                n8 = m_Abil.MaxHP;
                m_WAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.DC), HUtil32.Round(m_btSlaveExpLevel * 2 + HUtil32.HiWord(m_WAbil.DC)));
                n8 = n8 + HUtil32.Round(m_Abil.MaxHP * 0.15) * m_btSlaveExpLevel;
                m_WAbil.MaxHP = (short)HUtil32._MIN(HUtil32.Round(m_Abil.MaxHP + m_btSlaveExpLevel * 60), n8);
                // m_WAbil.MAC:=0; 01/20 取消此行，防止怪物升级后魔防变0
            }
            // m_btHitPoint:=15; 01/20 取消此行，防止怪物升级后准确率变15
        }

        public void SendFirstMsg(TBaseObject BaseObject, short wIdent, short wParam, int lParam1, int lParam2, int lParam3, string sMsg)
        {
            SendMessage SendMessage;
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                if (!m_boGhost)
                {
                    SendMessage = new SendMessage
                    {
                        wIdent = wIdent,
                        wParam = wParam,
                        nParam1 = lParam1,
                        nParam2 = lParam2,
                        nParam3 = lParam3,
                        dwDeliveryTime = 0,
                        BaseObject = BaseObject
                    };
                    if (!string.IsNullOrEmpty(sMsg))
                    {
                        SendMessage.Buff = sMsg;
                    }
                    else
                    {
                        SendMessage.Buff = null;
                    }
                    m_MsgList.Insert(0, SendMessage);
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
        }

        public void SendMsg(TBaseObject BaseObject, int wIdent, int wParam, int nParam1, int nParam2, int nParam3, string sMsg)
        {
            SendMessage SendMessage;
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                if (!m_boGhost)
                {
                    SendMessage = new SendMessage
                    {
                        wIdent = wIdent,
                        wParam = wParam,
                        nParam1 = nParam1,
                        nParam2 = nParam2,
                        nParam3 = nParam3,
                        dwDeliveryTime = 0,
                        BaseObject = BaseObject,
                        boLateDelivery = false
                    };
                    if (!string.IsNullOrEmpty(sMsg))
                    {
                        SendMessage.Buff = sMsg;
                    }
                    m_MsgList.Add(SendMessage);
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
        }

        public void SendDelayMsg(TBaseObject BaseObject, int wIdent, int wParam, int lParam1, int lParam2, int lParam3, string sMsg, int dwDelay)
        {
            SendMessage SendMessage;
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                if (!m_boGhost)
                {
                    SendMessage = new SendMessage
                    {
                        wIdent = wIdent,
                        wParam = wParam,
                        nParam1 = lParam1,
                        nParam2 = lParam2,
                        nParam3 = lParam3,
                        dwDeliveryTime = HUtil32.GetTickCount() + dwDelay,
                        BaseObject = BaseObject,
                        boLateDelivery = true
                    };
                    if (!string.IsNullOrEmpty(sMsg))
                    {
                        SendMessage.Buff = sMsg;
                    }
                    else
                    {
                        SendMessage.Buff = null;
                    }
                    m_MsgList.Add(SendMessage);
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
        }

        public void SendDelayMsg(int BaseObject, short wIdent, short wParam, int lParam1, int lParam2, int lParam3, string sMsg, int dwDelay)
        {
            SendMessage SendMessage;
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                if (!m_boGhost)
                {
                    SendMessage = new SendMessage
                    {
                        wIdent = wIdent,
                        wParam = wParam,
                        nParam1 = lParam1,
                        nParam2 = lParam2,
                        nParam3 = lParam3,
                        dwDeliveryTime = HUtil32.GetTickCount() + dwDelay,
                        BaseObject = M2Share.ObjectSystem.Get(BaseObject),
                        boLateDelivery = true
                    };
                    if (!string.IsNullOrEmpty(sMsg))
                    {
                        SendMessage.Buff = sMsg;
                    }
                    else
                    {
                        SendMessage.Buff = null;
                    }
                    m_MsgList.Add(SendMessage);
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
        }

        private void SendUpdateDelayMsg(TBaseObject BaseObject, short wIdent, short wParam, int lParam1, int lParam2, int lParam3, string sMsg, int dwDelay)
        {
            SendMessage SendMessage;
            int i;
            HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            try
            {
                i = 0;
                while (true)
                {
                    if (m_MsgList.Count <= i)
                    {
                        break;
                    }
                    SendMessage = m_MsgList[i];
                    if ((SendMessage.wIdent == wIdent) && (SendMessage.nParam1 == lParam1))
                    {
                        m_MsgList.RemoveAt(i);
                        if (SendMessage.Buff != null)
                        {
                            //FreeMem(SendMessage.Buff);
                        }
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
            SendDelayMsg(BaseObject, wIdent, wParam, lParam1, lParam2, lParam3, sMsg, dwDelay);
        }

        public void SendUpdateMsg(TBaseObject BaseObject, int wIdent, int wParam, int lParam1, int lParam2, int lParam3, string sMsg)
        {
            SendMessage SendMessage;
            int i;
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                i = 0;
                while (true)
                {
                    if (m_MsgList.Count <= i)
                    {
                        break;
                    }
                    SendMessage = m_MsgList[i];
                    if (SendMessage.wIdent == wIdent)
                    {
                        m_MsgList.RemoveAt(i);
                        if (SendMessage.Buff != null)
                        {
                            //FreeMem(SendMessage.Buff);
                        }
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

        public void SendActionMsg(TBaseObject BaseObject, int wIdent, int wParam, int lParam1, int lParam2, int lParam3, string sMsg)
        {
            SendMessage SendMessage;
            int i;
            HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            try
            {
                i = 0;
                while (true)
                {
                    if (m_MsgList.Count <= i)
                    {
                        break;
                    }
                    SendMessage = m_MsgList[i];
                    if ((SendMessage.wIdent == grobal2.CM_TURN) || (SendMessage.wIdent == grobal2.CM_WALK) || (SendMessage.wIdent == grobal2.CM_SITDOWN) || (SendMessage.wIdent == grobal2.CM_HORSERUN) || (SendMessage.wIdent == grobal2.CM_RUN) || (SendMessage.wIdent == grobal2.CM_HIT) || (SendMessage.wIdent == grobal2.CM_HEAVYHIT) || (SendMessage.wIdent == grobal2.CM_BIGHIT) || (SendMessage.wIdent == grobal2.CM_POWERHIT) || (SendMessage.wIdent == grobal2.CM_LONGHIT) || (SendMessage.wIdent == grobal2.CM_WIDEHIT) || (SendMessage.wIdent == grobal2.CM_FIREHIT))
                    {
                        m_MsgList.RemoveAt(i);
                        if (SendMessage.Buff != null)
                        {
                            //FreeMem(SendMessage.Buff);
                        }
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

        public virtual bool GetMessage(ref TProcessMessage Msg)
        {
            bool result = false;
            int I;
            SendMessage SendMessage;
            HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            try
            {
                I = 0;
                while (m_MsgList.Count > I)
                {
                    if (m_MsgList.Count <= I)
                    {
                        break;
                    }
                    SendMessage = m_MsgList[I];
                    //if ((SendMessage.dwDeliveryTime != 0) && (HUtil32.GetTickCount() < SendMessage.dwDeliveryTime))
                    //{
                    //    I++;
                    //    continue;
                    //}
                    m_MsgList.RemoveAt(I);
                    Msg = new TProcessMessage
                    {
                        wIdent = 0
                    };
                    Msg.wIdent = SendMessage.wIdent;
                    Msg.wParam = SendMessage.wParam;
                    Msg.nParam1 = SendMessage.nParam1;
                    Msg.nParam2 = SendMessage.nParam2;
                    Msg.nParam3 = SendMessage.nParam3;
                    if (SendMessage.BaseObject != null)
                    {
                        Msg.BaseObject = SendMessage.BaseObject.ObjectId;
                    }
                    Msg.dwDeliveryTime = SendMessage.dwDeliveryTime;
                    Msg.boLateDelivery = SendMessage.boLateDelivery;
                    if (!string.IsNullOrEmpty(SendMessage.Buff))
                    {
                        Msg.sMsg = SendMessage.Buff;
                    }
                    else
                    {
                        Msg.sMsg = string.Empty;
                    }
                    SendMessage = null;
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

        public bool GetMapBaseObjects(TEnvirnoment tEnvir, int nX, int nY, int nRage, IList<TBaseObject> rList)
        {
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
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
                        if (tEnvir.GetMapCellInfo(x, y, ref MapCellInfo) && (MapCellInfo.ObjList != null))
                        {
                            for (var j = 0; j < MapCellInfo.ObjList.Count; j++)
                            {
                                OSObject = MapCellInfo.ObjList[j];
                                if ((OSObject != null) && (OSObject.btType == grobal2.OS_MOVINGOBJECT))
                                {
                                    BaseObject = OSObject.CellObj as TBaseObject;
                                    if ((BaseObject != null) && (!BaseObject.m_boDeath) && (!BaseObject.m_boGhost))
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
                M2Share.ErrorMessage(sExceptionMsg);
            }
            return true;
        }

        public void SendRefMsg(int wIdent, int wParam, int nParam1, int nParam2, int nParam3, string sMsg)
        {
            int II;
            int nC;
            int nCX;
            int nCY;
            int nLX;
            int nLY;
            int nHX;
            int nHY;
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            TBaseObject BaseObject;
            const string sExceptionMsg = "[Exception] TBaseObject::SendRefMsg Name = {0}";
            if (m_PEnvir == null)
            {
                M2Share.ErrorMessage(m_sCharName + " SendRefMsg nil PEnvir ");
                return;
            }
            // 01/21 增加，原来直接不发信息，如果隐身模式则只发送信息给自己
            if (m_boObMode || m_boFixedHideMode)
            {
                SendMsg(this, wIdent, wParam, nParam1, nParam2, nParam3, sMsg);
                return;
            }
            HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            try
            {
                if (((HUtil32.GetTickCount() - m_SendRefMsgTick) >= 500) || (m_VisibleHumanList.Count == 0))
                {
                    m_SendRefMsgTick = HUtil32.GetTickCount();
                    m_VisibleHumanList.Clear();
                    nLX = m_nCurrX - M2Share.g_Config.nSendRefMsgRange; // 12
                    nHX = m_nCurrX + M2Share.g_Config.nSendRefMsgRange; // 12
                    nLY = m_nCurrY - M2Share.g_Config.nSendRefMsgRange; // 12
                    nHY = m_nCurrY + M2Share.g_Config.nSendRefMsgRange; // 12
                    for (nCX = nLX; nCX <= nHX; nCX++)
                    {
                        for (nCY = nLY; nCY <= nHY; nCY++)
                        {
                            if (m_PEnvir.GetMapCellInfo(nCX, nCY, ref MapCellInfo))
                            {
                                if (MapCellInfo.ObjList != null)
                                {
                                    for (II = MapCellInfo.ObjList.Count - 1; II >= 0; II--)
                                    {
                                        OSObject = MapCellInfo.ObjList[II];
                                        if (OSObject != null)
                                        {
                                            if (OSObject.btType == grobal2.OS_MOVINGOBJECT)
                                            {
                                                if ((HUtil32.GetTickCount() - OSObject.dwAddTime) >= 60 * 1000)
                                                {
                                                    OSObject = null;
                                                    MapCellInfo.ObjList.RemoveAt(II);
                                                    if (MapCellInfo.ObjList.Count <= 0)
                                                    {
                                                        //MapCellInfo.ObjList.Free;
                                                        MapCellInfo.ObjList = null;
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        BaseObject = OSObject.CellObj as TBaseObject;
                                                        if ((BaseObject != null) && !BaseObject.m_boGhost)
                                                        {
                                                            if (BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT)
                                                            {
                                                                BaseObject.SendMsg(this, wIdent, wParam, nParam1, nParam2, nParam3, sMsg);
                                                                m_VisibleHumanList.Add(BaseObject);
                                                            }
                                                            else if (BaseObject.m_boWantRefMsg)
                                                            {
                                                                if ((wIdent == grobal2.RM_STRUCK) || (wIdent == grobal2.RM_HEAR) || (wIdent == grobal2.RM_DEATH))
                                                                {
                                                                    BaseObject.SendMsg(this, wIdent, wParam, nParam1, nParam2, nParam3, sMsg);
                                                                    m_VisibleHumanList.Add(BaseObject);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        MapCellInfo.ObjList.RemoveAt(II);
                                                        if (MapCellInfo.ObjList.Count <= 0)
                                                        {
                                                            //MapCellInfo.ObjList.Free;
                                                            MapCellInfo.ObjList = null;
                                                        }
                                                        M2Share.ErrorMessage(format(sExceptionMsg, m_sCharName));
                                                        M2Share.ErrorMessage(e.Message);
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
                for (nC = 0; nC < m_VisibleHumanList.Count; nC++)
                {
                    BaseObject = m_VisibleHumanList[nC];
                    if (BaseObject.m_boGhost)
                    {
                        continue;
                    }
                    if ((BaseObject.m_PEnvir == m_PEnvir) && (Math.Abs(BaseObject.m_nCurrX - m_nCurrX) < 11) && (Math.Abs(BaseObject.m_nCurrY - m_nCurrY) < 11))
                    {
                        if (BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT)
                        {
                            BaseObject.SendMsg(this, wIdent, wParam, nParam1, nParam2, nParam3, sMsg);
                        }
                        else if (BaseObject.m_boWantRefMsg)
                        {
                            if ((wIdent == grobal2.RM_STRUCK) || (wIdent == grobal2.RM_HEAR) || (wIdent == grobal2.RM_DEATH))
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

        public virtual void UpdateVisibleGay(TBaseObject BaseObject)
        {
            bool boIsVisible = false;
            TVisibleBaseObject VisibleBaseObject;
            if ((BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT) || (BaseObject.m_Master != null))
            {
                m_boIsVisibleActive = true;
            }
            // 如果是人物或宝宝则置TRUE
            for (int i = 0; i < m_VisibleActors.Count; i++)
            {
                VisibleBaseObject = m_VisibleActors[i];
                if (VisibleBaseObject.BaseObject == BaseObject)
                {
                    VisibleBaseObject.nVisibleFlag = 1;
                    boIsVisible = true;
                    break;
                }
            }
            if (boIsVisible)
            {
                return;
            }
            VisibleBaseObject = new TVisibleBaseObject
            {
                nVisibleFlag = 2,
                BaseObject = BaseObject
            };
            m_VisibleActors.Add(VisibleBaseObject);
        }

        public void UpdateVisibleItem(int wX, int wY, TMapItem MapItem)
        {
            TVisibleMapItem VisibleMapItem;
            bool boIsVisible = false;
            for (int i = 0; i < m_VisibleItems.Count; i++)
            {
                VisibleMapItem = m_VisibleItems[i];
                if (VisibleMapItem.MapItem == MapItem)
                {
                    VisibleMapItem.nVisibleFlag = 1;
                    boIsVisible = true;
                    break;
                }
            }
            if (boIsVisible)
            {
                return;
            }
            VisibleMapItem = new TVisibleMapItem
            {
                nVisibleFlag = 2,
                nX = wX,
                nY = wY,
                MapItem = MapItem,
                sName = MapItem.Name,
                wLooks = MapItem.Looks
            };
            m_VisibleItems.Add(VisibleMapItem);
        }

        public void UpdateVisibleEvent(int wX, int wY, TEvent MapEvent)
        {
            bool boIsVisible = false;
            TEvent __Event;
            for (int i = 0; i < m_VisibleEvents.Count; i++)
            {
                __Event = m_VisibleEvents[i];
                if (__Event == MapEvent)
                {
                    __Event.nVisibleFlag = 1;
                    boIsVisible = true;
                    break;
                }
            }
            if (boIsVisible)
            {
                return;
            }
            MapEvent.nVisibleFlag = 2;
            MapEvent.m_nX = wX;
            MapEvent.m_nY = wY;
            m_VisibleEvents.Add(MapEvent);
        }

        public bool IsVisibleHuman()
        {
            bool result = false;
            TVisibleBaseObject VisibleBaseObject;
            for (int i = 0; i < m_VisibleActors.Count; i++)
            {
                VisibleBaseObject = m_VisibleActors[i];
                if ((VisibleBaseObject.BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT) || (VisibleBaseObject.BaseObject.m_Master != null))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public virtual void SearchViewRange()
        {
            int nStartX;
            int nEndX;
            int nStartY;
            int nEndY;
            int n18;
            int n1C;
            int nIdx;
            int n24;
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            TBaseObject BaseObject;
            TVisibleBaseObject VisibleBaseObject;
            const string sExceptionMsg1 = "[Exception] TBaseObject::SearchViewRange";
            const string sExceptionMsg2 = "[Exception] TBaseObject::SearchViewRange 1-%d %s %s %d %d %d";
            if (m_PEnvir == null)
            {
                M2Share.ErrorMessage("SearchViewRange nil PEnvir");
                return;
            }

            n24 = 0;
            m_boIsVisibleActive = false;// 先置为FALSE
            try
            {
                for (var i = 0; i < m_VisibleActors.Count; i++)
                {
                    m_VisibleActors[i].nVisibleFlag = 0;
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg1);
                KickException();
            }

            nStartX = m_nCurrX - m_nViewRange;
            nEndX = m_nCurrX + m_nViewRange;
            nStartY = m_nCurrY - m_nViewRange;
            nEndY = m_nCurrY + m_nViewRange;
            try
            {
                for (n18 = nStartX; n18 <= nEndX; n18++)
                {
                    for (n1C = nStartY; n1C <= nEndY; n1C++)
                    {
                        if (m_PEnvir.GetMapCellInfo(n18, n1C, ref MapCellInfo) && (MapCellInfo.ObjList != null))
                        {
                            n24 = 1;
                            nIdx = 0;
                            while (true)
                            {
                                if (MapCellInfo.ObjList.Count <= nIdx)
                                {
                                    break;
                                }
                                OSObject = MapCellInfo.ObjList[nIdx];
                                if (OSObject != null)
                                {
                                    if (OSObject.btType == grobal2.OS_MOVINGOBJECT)
                                    {
                                        if ((HUtil32.GetTickCount() - OSObject.dwAddTime) >= 60 * 1000)
                                        {
                                            OSObject = null;
                                            MapCellInfo.ObjList.RemoveAt(nIdx);
                                            if (MapCellInfo.ObjList.Count > 0)
                                            {
                                                continue;
                                            }
                                            MapCellInfo.ObjList = null;
                                            break;
                                        }

                                        BaseObject = OSObject.CellObj as TBaseObject;
                                        if (BaseObject != null)
                                        {
                                            if (!BaseObject.m_boGhost && !BaseObject.m_boFixedHideMode && !BaseObject.m_boObMode)
                                            {
                                                if ((m_btRaceServer < grobal2.RC_ANIMAL) || (m_Master != null) || m_boCrazyMode || m_boNastyMode || m_boWantRefMsg || ((BaseObject.m_Master != null) && (Math.Abs(BaseObject.m_nCurrX - m_nCurrX) <= 3) && (Math.Abs(BaseObject.m_nCurrY - m_nCurrY) <= 3)) || (BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT))
                                                {
                                                    UpdateVisibleGay(BaseObject);
                                                }
                                            }
                                        }
                                    }
                                }
                                nIdx++;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(format(sExceptionMsg2, new object[] { n24, m_sCharName, m_sMapName, m_nCurrX, m_nCurrY }));
                M2Share.ErrorMessage(e.Message);
                KickException();
            }

            n24 = 2;
            try
            {
                n18 = 0;
                while (true)
                {
                    if (m_VisibleActors.Count <= n18)
                    {
                        break;
                    }

                    VisibleBaseObject = m_VisibleActors[n18];
                    if (VisibleBaseObject.nVisibleFlag == 0)
                    {
                        m_VisibleActors.RemoveAt(n18);
                        Dispose(VisibleBaseObject);
                        continue;
                    }

                    n18++;
                }
            }
            catch
            {
                M2Share.ErrorMessage(format(sExceptionMsg2, new object[] { n24, m_sCharName, m_sMapName, m_nCurrX, m_nCurrY }));
                KickException();
            }
        }

        public int GetFeatureToLong()
        {
            return GetFeature(null);
        }

        public short GetFeatureEx()
        {
            short result;
            if (m_boOnHorse)
            {
                result = HUtil32.MakeWord(m_btHorseType, m_btDressEffType);
            }
            else
            {
                result = HUtil32.MakeWord(0, m_btDressEffType);
            }
            return result;
        }

        public int GetFeature(TBaseObject BaseObject)
        {
            int result;
            byte nDress;
            byte nWeapon;
            byte nHair;
            byte nRaceImg;
            byte nAppr;
            TItem StdItem;
            bool bo25;
            if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
            {
                nDress = 0;
                if (m_UseItems[grobal2.U_DRESS] != null && m_UseItems[grobal2.U_DRESS].wIndex > 0)// 衣服
                {
                    StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[grobal2.U_DRESS].wIndex);
                    if (StdItem != null)
                    {
                        nDress = (byte)(StdItem.Shape * 2);
                    }
                }
                nDress += m_btGender;
                nWeapon = 0;
                if (m_UseItems[grobal2.U_WEAPON] != null && m_UseItems[grobal2.U_WEAPON].wIndex > 0)// 武器
                {
                    StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[grobal2.U_WEAPON].wIndex);
                    if (StdItem != null)
                    {
                        nWeapon = (byte)(StdItem.Shape * 2);
                    }
                }
                nWeapon += m_btGender;
                nHair = (byte)(m_btHair * 2 + m_btGender);
                result = grobal2.MakeHumanFeature(0, nDress, nWeapon, nHair);
                return result;
            }
            bo25 = false;
            if ((BaseObject != null) && BaseObject.m_boRaceImg)
            {
                bo25 = true;
            }
            if (bo25)
            {
                nRaceImg = m_btRaceImg;
                nAppr = (byte)m_wAppr;
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
                result = grobal2.MakeMonsterFeature(nRaceImg, m_btMonsterWeapon, nAppr);
                return result;
            }
            result = grobal2.MakeMonsterFeature(m_btRaceImg, m_btMonsterWeapon, m_wAppr);
            return result;
        }

        public int GetCharStatus()
        {
            int result = 0;
            int nStatus = 0;
            for (int i = m_wStatusTimeArr.GetLowerBound(0); i <= m_wStatusTimeArr.GetUpperBound(0); i++)
            {
                if (m_wStatusTimeArr[i] > 0)
                {
                    nStatus = Convert.ToInt32((0x80000000 >> i) | nStatus);
                }
            }
            result = (m_nCharStatusEx & 0xFFFFF) | nStatus;
            return result;
        }

        public void AbilCopyToWAbil()
        {
            m_WAbil = m_Abil;
        }

        public virtual void Initialize()
        {
            TUserMagic UserMagic;
            AbilCopyToWAbil();
            for (int i = 0; i < m_MagicList.Count; i++)
            {
                UserMagic = m_MagicList[i];
                if (UserMagic.btLevel >= 4)
                {
                    UserMagic.btLevel = 0;
                }
            }
            m_boAddtoMapSuccess = true;
            if (m_PEnvir.CanWalk(m_nCurrX, m_nCurrY, true) && AddToMap())
            {
                m_boAddtoMapSuccess = false;
            }
            m_nCharStatus = GetCharStatus();
            AddBodyLuck(0);
            LoadSayMsg();
            if (M2Share.g_Config.boMonSayMsg)
            {
                MonsterSayMsg(null, TMonStatus.s_MonGen);
            }
        }

        /// <summary>
        /// 取怪物说话信息列表
        /// </summary>
        public void LoadSayMsg()
        {
            for (var i = 0; i < M2Share.g_MonSayMsgList.Count; i++)
            {
                if (M2Share.g_MonSayMsgList.TryGetValue(m_sCharName, out m_SayMsgList))
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
            SendRefMsg(grobal2.RM_FEATURECHANGED, GetFeatureEx(), GetFeatureToLong(), 0, 0, "");
        }

        public void StatusChanged()
        {
            SendRefMsg(grobal2.RM_CHARSTATUSCHANGED, m_nHitSpeed, m_nCharStatus, 0, 0, "");
        }

        public void DisappearA()
        {
            m_PEnvir.DeleteFromMap(m_nCurrX, m_nCurrY, grobal2.OS_MOVINGOBJECT, this);
            SendRefMsg(grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
        }

        public void KickException()
        {
            TPlayObject PlayObject;
            if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
            {
                m_sMapName = M2Share.g_Config.sHomeMap;
                m_nCurrX = M2Share.g_Config.nHomeX;
                m_nCurrY = M2Share.g_Config.nHomeY;
                PlayObject = this as TPlayObject;
                PlayObject.m_boEmergencyClose = true;
            }
            else
            {
                m_boDeath = true;
                m_dwDeathTick = HUtil32.GetTickCount();
                MakeGhost();
            }
        }

        public bool Walk(int nIdent)
        {
            bool result;
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            TGateObj GateObj;
            bool bo1D;
            TEvent __Event;
            TPlayObject PlayObject;
            int nCheckCode;
            const string sExceptionMsg = "[Exception] TBaseObject::Walk  CheckCode:%d %s %s %d:%d";
            result = true;
            nCheckCode = -1;
            if (m_PEnvir == null)
            {
                M2Share.ErrorMessage("Walk nil PEnvir");
                return result;
            }
            try
            {
                nCheckCode = 1;
                bo1D = m_PEnvir.GetMapCellInfo(m_nCurrX, m_nCurrY, ref MapCellInfo);
                GateObj = null;
                __Event = null;
                nCheckCode = 2;
                if (bo1D && (MapCellInfo.ObjList != null))
                {
                    for (int i = 0; i < MapCellInfo.ObjList.Count; i++)
                    {
                        OSObject = MapCellInfo.ObjList[i];
                        if (OSObject.btType == grobal2.OS_GATEOBJECT)
                        {
                            GateObj = (TGateObj)OSObject.CellObj;
                        }
                        if (OSObject.btType == grobal2.OS_EVENTOBJECT)
                        {
                            if (((TEvent)OSObject.CellObj).m_OwnBaseObject != null)
                            {
                                __Event = (TEvent)OSObject.CellObj;
                            }
                        }
                        if (OSObject.btType == grobal2.OS_MAPEVENT)
                        {
                        }
                        if (OSObject.btType == grobal2.OS_DOOR)
                        {
                        }
                        if (OSObject.btType == grobal2.OS_ROON)
                        {
                        }
                    }
                }
                nCheckCode = 3;
                if (__Event != null)
                {
                    if (__Event.m_OwnBaseObject.IsProperTarget(this))
                    {
                        SendMsg(__Event.m_OwnBaseObject, grobal2.RM_MAGSTRUCK_MINE, 0, __Event.m_nDamage, 0, 0, "");
                    }
                }
                nCheckCode = 4;
                if (result && (GateObj != null))
                {
                    if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                    {
                        if (m_PEnvir.ArroundDoorOpened(m_nCurrX, m_nCurrY))
                        {
                            if ((!GateObj.DEnvir.Flag.boNEEDHOLE) || (M2Share.EventManager.GetEvent(m_PEnvir, m_nCurrX, m_nCurrY, grobal2.ET_DIGOUTZOMBI) != null))
                            {
                                if (M2Share.nServerIndex == GateObj.DEnvir.nServerIndex)
                                {
                                    if (!EnterAnotherMap(GateObj.DEnvir, GateObj.nDMapX, GateObj.nDMapY))
                                    {
                                        result = false;
                                    }
                                }
                                else
                                {
                                    DisappearA();
                                    m_bo316 = true;
                                    PlayObject = this as TPlayObject;
                                    PlayObject.m_sSwitchMapName = GateObj.DEnvir.sMapName;
                                    PlayObject.m_nSwitchMapX = GateObj.nDMapX;
                                    PlayObject.m_nSwitchMapY = GateObj.nDMapY;
                                    PlayObject.m_boSwitchData = true;
                                    PlayObject.m_nServerIndex = GateObj.DEnvir.nServerIndex;
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
                else
                {
                    nCheckCode = 5;
                    if (result)
                    {
                        nCheckCode = 6;
                        SendRefMsg(nIdent, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(format(sExceptionMsg, new object[] { nCheckCode, m_sCharName, m_sMapName, m_nCurrX, m_nCurrY }));
                M2Share.ErrorMessage(e.Message);
            }
            return result;
        }

        public bool EnterAnotherMap(TEnvirnoment Envir, int nDMapX, int nDMapY)
        {
            bool result = false;
            TMapCellinfo MapCellInfo = null;
            TEnvirnoment OldEnvir;
            int nOldX;
            int nOldY;
            TUserCastle Castle;
            const string sExceptionMsg1 = "[Exception] TBaseObject::EnterAnotherMap -> MsgTargetList Clear";
            const string sExceptionMsg2 = "[Exception] TBaseObject::EnterAnotherMap -> VisbleItems Dispose";
            const string sExceptionMsg3 = "[Exception] TBaseObject::EnterAnotherMap -> VisbleItems Clear";
            const string sExceptionMsg4 = "[Exception] TBaseObject::EnterAnotherMap -> VisbleEvents Clear";
            const string sExceptionMsg5 = "[Exception] TBaseObject::EnterAnotherMap -> VisbleActors Dispose";
            const string sExceptionMsg6 = "[Exception] TBaseObject::EnterAnotherMap -> VisbleActors Clear";
            const string sExceptionMsg7 = "[Exception] TBaseObject::EnterAnotherMap";
            try
            {
                if (m_Abil.Level < Envir.nRequestLevel)
                {
                    return result;
                }
                if (Envir.QuestNPC != null)
                {
                    ((TMerchant)Envir.QuestNPC).Click(this as TPlayObject);
                }
                if (Envir.Flag.nNEEDSETONFlag >= 0)
                {
                    if (GetQuestFalgStatus(Envir.Flag.nNEEDSETONFlag) != Envir.Flag.nNeedONOFF)
                    {
                        return result;
                    }
                }
                if (!Envir.GetMapCellInfo(nDMapX, nDMapY, ref MapCellInfo))
                {
                    return result;
                }
                Castle = M2Share.CastleManager.IsCastlePalaceEnvir(Envir);
                if ((Castle != null) && (m_btRaceServer == grobal2.RC_PLAYOBJECT))
                {
                    if (!Castle.CheckInPalace(m_nCurrX, m_nCurrY, this))
                    {
                        return result;
                    }
                }
                if (Envir.Flag.boNOHORSE)
                {
                    m_boOnHorse = false;
                }
                OldEnvir = m_PEnvir;
                nOldX = m_nCurrX;
                nOldY = m_nCurrY;
                DisappearA();
                try
                {
                    m_VisibleHumanList.Clear();
                }
                catch
                {
                    M2Share.ErrorMessage(sExceptionMsg1);
                }
                try
                {
                    for (var i = 0; i < m_VisibleItems.Count; i++)
                    {
                        m_VisibleItems[i] = null;
                    }
                }
                catch
                {
                    M2Share.ErrorMessage(sExceptionMsg2);
                }
                try
                {
                    m_VisibleItems.Clear();
                }
                catch
                {
                    M2Share.ErrorMessage(sExceptionMsg3);
                }
                try
                {
                    m_VisibleEvents.Clear();
                }
                catch
                {
                    M2Share.ErrorMessage(sExceptionMsg4);
                }
                try
                {
                    for (var i = 0; i < m_VisibleActors.Count; i++)
                    {
                        m_VisibleActors[i] = null;
                    }
                }
                catch
                {
                    M2Share.ErrorMessage(sExceptionMsg5);
                }
                try
                {
                    m_VisibleActors.Clear();
                }
                catch
                {
                    M2Share.ErrorMessage(sExceptionMsg6);
                }
                SendMsg(this, grobal2.RM_CLEAROBJECTS, 0, 0, 0, 0, "");
                m_PEnvir = Envir;
                m_sMapName = Envir.sMapName;
                m_sMapFileName = Envir.m_sMapFileName;
                m_nCurrX = (short)nDMapX;
                m_nCurrY = (short)nDMapY;
                SendMsg(this, grobal2.RM_CHANGEMAP, 0, 0, 0, 0, Envir.m_sMapFileName);
                if (AddToMap())
                {

                    m_dwMapMoveTick = HUtil32.GetTickCount();
                    m_bo316 = true;
                    result = true;
                }
                else
                {
                    m_PEnvir = OldEnvir;
                    m_nCurrX = (short)nOldX;
                    m_nCurrY = (short)nOldY;
                    m_PEnvir.AddToMap(m_nCurrX, m_nCurrY, grobal2.OS_MOVINGOBJECT, this);
                }
                if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                {
                    // 复位泡点，及金币，时间
                    (this as TPlayObject).m_dwIncGamePointTick = HUtil32.GetTickCount();
                    (this as TPlayObject).m_dwIncGameGoldTick = HUtil32.GetTickCount();
                    (this as TPlayObject).m_dwAutoGetExpTick = HUtil32.GetTickCount();
                }
                if (m_PEnvir.Flag.boFight3Zone && (m_PEnvir.Flag.boFight3Zone != OldEnvir.Flag.boFight3Zone))
                {
                    RefShowName();
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg7);
            }
            return result;
        }

        public void TurnTo(byte nDir)
        {
            m_btDirection = nDir;
            SendRefMsg(grobal2.RM_TURN, nDir, m_nCurrX, m_nCurrY, 0, "");
        }

        public virtual void ProcessSayMsg(string sMsg)
        {
            string sCharName;
            if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
            {
                sCharName = m_sCharName;
            }
            else
            {
                sCharName = M2Share.FilterShowName(m_sCharName);
            }
            SendRefMsg(grobal2.RM_HEAR, 0, M2Share.g_Config.btHearMsgFColor, M2Share.g_Config.btHearMsgBColor, 0, sCharName + ':' + sMsg);
        }

        public void SysMsg(string sMsg, TMsgColor MsgColor, TMsgType MsgType)
        {
            if (M2Share.g_Config.boShowPreFixMsg)
            {
                switch (MsgType)
                {
                    case TMsgType.t_Mon:
                        sMsg = M2Share.g_Config.sMonSayMsgpreFix + sMsg;
                        break;
                    case TMsgType.t_Hint:
                        sMsg = M2Share.g_Config.sHintMsgPreFix + sMsg;
                        break;
                    case TMsgType.t_GM:
                        sMsg = M2Share.g_Config.sGMRedMsgpreFix + sMsg;
                        break;
                    case TMsgType.t_System:
                        sMsg = M2Share.g_Config.sSysMsgPreFix + sMsg;
                        break;
                    case TMsgType.t_Notice:
                        sMsg = M2Share.g_Config.sLineNoticePreFix + sMsg;
                        break;
                    case TMsgType.t_Cust:
                        sMsg = M2Share.g_Config.sCustMsgpreFix + sMsg;
                        break;
                    case TMsgType.t_Castle:
                        sMsg = M2Share.g_Config.sCastleMsgpreFix + sMsg;
                        break;
                }
            }
            switch (MsgColor)
            {
                case TMsgColor.c_Green:
                    SendMsg(this, grobal2.RM_SYSMESSAGE, 0, M2Share.g_Config.btGreenMsgFColor, M2Share.g_Config.btGreenMsgBColor, 0, sMsg);
                    break;
                case TMsgColor.c_Blue:
                    SendMsg(this, grobal2.RM_SYSMESSAGE, 0, M2Share.g_Config.btBlueMsgFColor, M2Share.g_Config.btBlueMsgBColor, 0, sMsg);
                    break;
                default:
                    if (MsgType == TMsgType.t_Cust)
                    {
                        SendMsg(this, grobal2.RM_SYSMESSAGE, 0, M2Share.g_Config.btCustMsgFColor, M2Share.g_Config.btCustMsgBColor, 0, sMsg);
                    }
                    else
                    {
                        SendMsg(this, grobal2.RM_SYSMESSAGE, 0, M2Share.g_Config.btRedMsgFColor, M2Share.g_Config.btRedMsgBColor, 0, sMsg);
                    }
                    break;
            }
        }

        /// <summary>
        /// 怪物说话
        /// </summary>
        /// <param name="AttackBaseObject"></param>
        /// <param name="MonStatus"></param>
        public void MonsterSayMsg(TBaseObject AttackBaseObject, TMonStatus MonStatus)
        {
            string sAttackName = string.Empty;
            if (m_SayMsgList == null)
            {
                return;
            }
            if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
            {
                return;
            }
            if (AttackBaseObject != null)
            {
                if ((AttackBaseObject.m_btRaceServer != grobal2.RC_PLAYOBJECT) && (AttackBaseObject.m_Master == null))
                {
                    return;
                }
                if (AttackBaseObject.m_Master != null)
                {
                    sAttackName = AttackBaseObject.m_Master.m_sCharName;
                }
                else
                {
                    sAttackName = AttackBaseObject.m_sCharName;
                }
            }
            TMonSayMsg MonSayMsg = null;
            string sMsg = string.Empty;
            for (var i = 0; i < m_SayMsgList.Count; i++)
            {
                MonSayMsg = m_SayMsgList[i];
                sMsg = MonSayMsg.sSayMsg.Replace("%s", M2Share.FilterShowName(m_sCharName));
                sMsg = sMsg.Replace("%d", sAttackName);
                if ((MonSayMsg.State == MonStatus) && (M2Share.RandomNumber.Random(MonSayMsg.nRate) == 0))
                {
                    if (MonStatus == TMonStatus.s_MonGen)
                    {
                        M2Share.UserEngine.SendBroadCastMsg(sMsg, TMsgType.t_Mon);
                        break;
                    }
                    if (MonSayMsg.Color == TMsgColor.c_White)
                    {
                        ProcessSayMsg(sMsg);
                    }
                    else
                    {
                        AttackBaseObject.SysMsg(sMsg, MonSayMsg.Color, TMsgType.t_Mon);
                    }
                    break;
                }
            }
        }

        public void SendGroupText(string sMsg)
        {
            TPlayObject PlayObject;
            sMsg = M2Share.g_Config.sGroupMsgPreFix + sMsg;
            if (m_GroupOwner != null)
            {
                for (int i = 0; i < m_GroupOwner.m_GroupMembers.Count; i++)
                {
                    PlayObject = m_GroupOwner.m_GroupMembers[i];
                    PlayObject.SendMsg(this, grobal2.RM_GROUPMESSAGE, 0, M2Share.g_Config.btGroupMsgFColor, M2Share.g_Config.btGroupMsgBColor, 0, sMsg);
                }
            }
        }

        public virtual void MakeGhost()
        {
            m_boGhost = true;
            m_dwGhostTick = HUtil32.GetTickCount();
            DisappearA();
        }

        public void ApplyMeatQuality()
        {
            TItem StdItem;
            TUserItem UserItem;
            for (int i = 0; i < m_ItemList.Count; i++)
            {
                UserItem = m_ItemList[i];
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (StdItem != null)
                {
                    if (StdItem.StdMode == 40)
                    {
                        UserItem.Dura = (short)m_nMeatQuality;
                    }
                }
            }
        }

        public bool TakeBagItems(TBaseObject BaseObject)
        {
            bool result = false;
            TUserItem UserItem;
            TPlayObject PlayObject;
            while (true)
            {
                if (BaseObject.m_ItemList.Count <= 0)
                {
                    break;
                }
                UserItem = BaseObject.m_ItemList[0];
                if (!AddItemToBag(UserItem))
                {
                    break;
                }
                if (this is TPlayObject)
                {
                    PlayObject = this as TPlayObject;
                    PlayObject.SendAddItem(UserItem);
                    result = true;
                }
                BaseObject.m_ItemList.RemoveAt(0);
            }
            return result;
        }

        public virtual void ScatterBagItems(TBaseObject ItemOfCreat)
        {
            int DropWide;
            TUserItem UserItem;
            TItem StdItem;
            bool boCanNotDrop;
            const string sExceptionMsg = "[Exception] TBaseObject::ScatterBagItems";
            try
            {
                DropWide = HUtil32._MIN(M2Share.g_Config.nDropItemRage, 7);// 3
                if ((m_btRaceServer == grobal2.RC_PLAYCLONE) && (m_Master != null))
                {
                    return;
                }
                for (int i = m_ItemList.Count - 1; i >= 0; i--)
                {
                    UserItem = m_ItemList[i];
                    StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    boCanNotDrop = false;
                    if (StdItem != null)
                    {
                        TMonDrop MonDrop = null;
                        if (M2Share.g_MonDropLimitLIst.TryGetValue(StdItem.Name, out MonDrop))
                        {
                            if (MonDrop.nDropCount < MonDrop.nCountLimit)
                            {
                                MonDrop.nDropCount++;
                                M2Share.g_MonDropLimitLIst[StdItem.Name] = MonDrop;
                            }
                            else
                            {
                                MonDrop.nNoDropCount++;
                                boCanNotDrop = true;
                            }
                            break;
                        }
                    }
                    if (boCanNotDrop)
                    {
                        continue;
                    }
                    if (DropItemDown(UserItem, DropWide, true, ItemOfCreat, this))
                    {
                        Dispose(UserItem);
                        m_ItemList.RemoveAt(i);
                    }
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
        }

        public void ScatterGolds(TBaseObject GoldOfCreat)
        {
            int I;
            int nGold;
            if (m_nGold > 0)
            {
                I = 0;
                while (true)
                {
                    if (m_nGold > M2Share.g_Config.nMonOneDropGoldCount)
                    {
                        nGold = M2Share.g_Config.nMonOneDropGoldCount;
                        m_nGold = m_nGold - M2Share.g_Config.nMonOneDropGoldCount;
                    }
                    else
                    {
                        nGold = m_nGold;
                        m_nGold = 0;
                    }
                    if (nGold > 0)
                    {
                        if (!DropGoldDown(nGold, true, GoldOfCreat, this))
                        {
                            m_nGold = m_nGold + nGold;
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

        public virtual void DropUseItems(TBaseObject BaseObject)
        {
            int nC;
            int nRate;
            TItem StdItem;
            IList<int> DropItemList = null;
            const string sExceptionMsg = "[Exception] TBaseObject::DropUseItems";
            try
            {
                if (m_boNoDropUseItem)
                {
                    return;
                }
                if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                {
                    nC = 0;
                    while (true)
                    {
                        if (m_UseItems[nC] == null)
                        {
                            nC++;
                            continue;
                        }
                        StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[nC].wIndex);
                        if (StdItem != null)
                        {
                            if ((StdItem.Reserved & 8) != 0)
                            {
                                if (DropItemList == null)
                                {
                                    DropItemList = new List<int>();
                                }
                                DropItemList.Add(m_UseItems[nC].MakeIndex);
                                if (StdItem.NeedIdentify == 1)
                                {
                                    M2Share.AddGameDataLog("16" + "\t" + m_sMapName + "\t" + m_nCurrX.ToString() + "\t" + m_nCurrY.ToString() + "\t" + m_sCharName + "\t" + StdItem.Name + "\t" + m_UseItems[nC].MakeIndex.ToString() + "\t" + HUtil32.BoolToIntStr(m_btRaceServer == grobal2.RC_PLAYOBJECT) + "\t" + '0');
                                }
                                m_UseItems[nC].wIndex = 0;
                            }
                        }
                        nC++;
                        if (nC >= 9)
                        {
                            break;
                        }
                    }
                }
                if (PKLevel() > 2)
                {
                    nRate = 15;
                }
                else
                {
                    nRate = 30;
                }
                nC = 0;
                while (true)
                {
                    if (M2Share.RandomNumber.Random(nRate) == 0)
                    {
                        if (m_UseItems[nC] == null)
                        {
                            nC++;
                            continue;
                        }
                        if (DropItemDown(m_UseItems[nC], 2, true, BaseObject, this))
                        {
                            StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[nC].wIndex);
                            if (StdItem != null)
                            {
                                if ((StdItem.Reserved & 10) == 0)
                                {
                                    if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                                    {
                                        if (DropItemList == null)
                                        {
                                            DropItemList = new List<int>();
                                        }
                                        DropItemList.Add(m_UseItems[nC].MakeIndex);
                                    }
                                    m_UseItems[nC].wIndex = 0;
                                }
                            }
                        }
                    }
                    nC++;
                    if (nC >= 9)
                    {
                        break;
                    }
                }
                if (DropItemList != null)
                {
                    var ObjectId = HUtil32.Sequence();
                    M2Share.ObjectSystem.AddOhter(ObjectId, DropItemList);
                    SendMsg(this, grobal2.RM_SENDDELITEMLIST, 0, ObjectId, 0, 0, "");
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
        }

        public void SetLastHiter(TBaseObject BaseObject)
        {
            m_LastHiter = BaseObject;
            m_LastHiterTick = HUtil32.GetTickCount();
            if (m_ExpHitter == null)
            {
                m_ExpHitter = BaseObject;
                m_ExpHitterTick = HUtil32.GetTickCount();
            }
            else
            {
                if (m_ExpHitter == BaseObject)
                {
                    m_ExpHitterTick = HUtil32.GetTickCount();
                }
            }
        }

        public void SetPKFlag(TBaseObject BaseObject)
        {
            if ((PKLevel() < 2) && (BaseObject.PKLevel() < 2) && (!m_PEnvir.Flag.boFightZone) && (!m_PEnvir.Flag.boFight3Zone) && !m_boPKFlag)
            {
                BaseObject.m_dwPKTick = HUtil32.GetTickCount();
                if (!BaseObject.m_boPKFlag)
                {
                    BaseObject.m_boPKFlag = true;
                    BaseObject.RefNameColor();
                }
            }
        }

        public bool IsGoodKilling(TBaseObject cert)
        {
            bool result = false;
            if (cert.m_boPKFlag)
            {
                result = true;
            }
            return result;
        }

        public bool IsAttackTarget_sub_4C88E4()
        {
            return true;
        }

        public virtual bool IsAttackTarget(TBaseObject BaseObject)
        {
            bool result = false;
            int I;
            if ((BaseObject == null) || (BaseObject == this))
            {
                return result;
            }
            if (m_btRaceServer >= grobal2.RC_ANIMAL)
            {
                if (m_Master != null)
                {
                    if ((m_Master.m_LastHiter == BaseObject) || (m_Master.m_ExpHitter == BaseObject) || (m_Master.m_TargetCret == BaseObject))
                    {
                        result = true;
                    }
                    if (BaseObject.m_TargetCret != null)
                    {
                        if ((BaseObject.m_TargetCret == m_Master) || (BaseObject.m_TargetCret.m_Master == m_Master) && (BaseObject.m_btRaceServer != grobal2.RC_PLAYOBJECT))
                        {
                            result = true;
                        }
                    }
                    if ((BaseObject.m_TargetCret == this) && (BaseObject.m_btRaceServer >= grobal2.RC_ANIMAL))
                    {
                        result = true;
                    }
                    if (BaseObject.m_Master != null)
                    {
                        if ((BaseObject.m_Master == m_Master.m_LastHiter) || (BaseObject.m_Master == m_Master.m_TargetCret))
                        {
                            result = true;
                        }
                    }
                    if (BaseObject.m_Master == m_Master)
                    {
                        result = false;
                    }
                    if (BaseObject.m_boHolySeize)
                    {
                        result = false;
                    }
                    if (m_Master.m_boSlaveRelax)
                    {
                        result = false;
                    }
                    if (BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT)
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
                    if (BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT)
                    {
                        result = true;
                    }
                    // 15
                    // 50
                    if ((m_btRaceServer > grobal2.RC_PEACENPC) && (m_btRaceServer < grobal2.RC_ANIMAL))
                    {
                        result = true;
                    }
                    if (BaseObject.m_Master != null)
                    {
                        result = true;
                    }
                }
                if (m_boCrazyMode && ((BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT) || (BaseObject.m_btRaceServer > grobal2.RC_PEACENPC)))
                {
                    result = true;
                }
                if (m_boNastyMode && ((BaseObject.m_btRaceServer < grobal2.RC_NPC) || (BaseObject.m_btRaceServer > grobal2.RC_PEACENPC)))
                {
                    result = true;
                }
            }
            else
            {
                if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                {
                    switch (m_btAttatckMode)
                    {
                        case M2Share.HAM_ALL:
                            if ((BaseObject.m_btRaceServer < grobal2.RC_NPC) || (BaseObject.m_btRaceServer > grobal2.RC_PEACENPC))
                            {
                                result = true;
                            }
                            if (M2Share.g_Config.boNonPKServer)
                            {
                                result = IsAttackTarget_sub_4C88E4();
                            }
                            break;
                        case M2Share.HAM_PEACE:
                            // 1
                            if (BaseObject.m_btRaceServer >= grobal2.RC_ANIMAL)
                            {
                                result = true;
                            }
                            break;
                        case M2Share.HAM_DEAR:
                            if (BaseObject != (this as TPlayObject).m_DearHuman)
                            {
                                result = true;
                            }
                            break;
                        case M2Share.HAM_MASTER:
                            if (BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT)
                            {
                                result = true;
                                if ((this as TPlayObject).m_boMaster)
                                {
                                    for (I = 0; I < (this as TPlayObject).m_MasterList.Count; I++)
                                    {
                                        if ((this as TPlayObject).m_MasterList[I] == BaseObject)
                                        {
                                            result = false;
                                            break;
                                        }
                                    }
                                }
                                if ((BaseObject as TPlayObject).m_boMaster)
                                {
                                    for (I = 0; I < (BaseObject as TPlayObject).m_MasterList.Count; I++)
                                    {
                                        if ((BaseObject as TPlayObject).m_MasterList[I] == this)
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
                        case M2Share.HAM_GROUP:
                            // 2
                            if ((BaseObject.m_btRaceServer < grobal2.RC_NPC) || (BaseObject.m_btRaceServer > grobal2.RC_PEACENPC))
                            {
                                result = true;
                            }
                            if (BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT)
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
                        case M2Share.HAM_GUILD:
                            // 3
                            if ((BaseObject.m_btRaceServer < grobal2.RC_NPC) || (BaseObject.m_btRaceServer > grobal2.RC_PEACENPC))
                            {
                                result = true;
                            }
                            if (BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT)
                            {
                                if (m_MyGuild != null)
                                {
                                    if (m_MyGuild.IsMember(BaseObject.m_sCharName))
                                    {
                                        result = false;
                                    }
                                    if (m_boGuildWarArea && (BaseObject.m_MyGuild != null))
                                    {
                                        if (m_MyGuild.IsAllyGuild(BaseObject.m_MyGuild))
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
                        case M2Share.HAM_PKATTACK:
                            // 4
                            if ((BaseObject.m_btRaceServer < grobal2.RC_NPC) || (BaseObject.m_btRaceServer > grobal2.RC_PEACENPC))
                            {
                                result = true;
                            }
                            if (BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT)
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
            if (BaseObject.m_boAdminMode || BaseObject.m_boStoneMode)
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
                if ((m_btRaceServer == grobal2.RC_PLAYOBJECT) && (BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT))
                {
                    result = IsProtectTarget(BaseObject);
                }
            }
            if ((BaseObject != null) && (m_btRaceServer == grobal2.RC_PLAYOBJECT) && (BaseObject.m_Master != null) && (BaseObject.m_btRaceServer != grobal2.RC_PLAYOBJECT))
            {
                if (BaseObject.m_Master == this)
                {
                    if (m_btAttatckMode != M2Share.HAM_ALL)
                    {
                        result = false;
                    }
                }
                else
                {
                    result = IsAttackTarget(BaseObject.m_Master);
                    if (InSafeZone() || BaseObject.InSafeZone())
                    {
                        result = false;
                    }
                }
            }
            return result;
        }

        public void WeightChanged()
        {
            m_WAbil.Weight = (short)RecalcBagWeight();
            SendUpdateMsg(this, grobal2.RM_WEIGHTCHANGED, 0, 0, 0, 0, "");
        }

        public bool InSafeZone()
        {
            bool result;
            int nSafeX;
            int nSafeY;
            if (m_PEnvir == null)// 修正机器人刷火墙的错误
            {
                result = true;
                return result;
            }
            result = m_PEnvir.Flag.boSAFE;
            if (result)
            {
                return result;
            }
            if ((m_PEnvir.sMapName != M2Share.g_Config.sRedHomeMap) || (Math.Abs(m_nCurrX - M2Share.g_Config.nRedHomeX) > M2Share.g_Config.nSafeZoneSize) || (Math.Abs(m_nCurrY - M2Share.g_Config.nRedHomeY) > M2Share.g_Config.nSafeZoneSize))
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
                if (M2Share.StartPointList[i].m_sMapName == m_PEnvir.sMapName)
                {
                    if (M2Share.StartPointList[i] != null)
                    {
                        nSafeX = M2Share.StartPointList[i].m_nCurrX;
                        nSafeY = M2Share.StartPointList[i].m_nCurrY;
                        if ((Math.Abs(m_nCurrX - nSafeX) <= M2Share.g_Config.nSafeZoneSize) && (Math.Abs(m_nCurrY - nSafeY) <= M2Share.g_Config.nSafeZoneSize))
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        public bool InSafeZone(TEnvirnoment Envir, int nX, int nY)
        {
            bool result;
            int nSafeX;
            int nSafeY;
            if (m_PEnvir == null)  // 修正机器人刷火墙的错误
            {
                result = true;
                return result;
            }
            result = m_PEnvir.Flag.boSAFE;
            if (result)
            {
                return result;
            }
            if ((Envir.sMapName != M2Share.g_Config.sRedHomeMap) || (Math.Abs(nX - M2Share.g_Config.nRedHomeX) > M2Share.g_Config.nSafeZoneSize) || (Math.Abs(nY - M2Share.g_Config.nRedHomeY) > M2Share.g_Config.nSafeZoneSize))
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
                if (M2Share.StartPointList[i].m_sMapName == Envir.sMapName)
                {
                    if (M2Share.StartPointList[i] != null)
                    {
                        nSafeX = M2Share.StartPointList[i].m_nCurrX;
                        nSafeY = M2Share.StartPointList[i].m_nCurrY;
                        if ((Math.Abs(nX - nSafeX) <= M2Share.g_Config.nSafeZoneSize) && (Math.Abs(nY - nSafeY) <= M2Share.g_Config.nSafeZoneSize))
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
            SendGroupText(format(sExitGropMsg, m_sCharName));
            m_GroupOwner = null;
            SendMsg(this, grobal2.RM_GROUPCANCEL, 0, 0, 0, 0, "");
        }

        public TUserMagic GetMagicInfo(int nMagicID)
        {
            TUserMagic result = null;
            TUserMagic UserMagic;
            for (int i = 0; i < m_MagicList.Count; i++)
            {
                UserMagic = m_MagicList[i];
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
            if (m_boFastTrain)
            {
                nTranPoint = nTranPoint * 3;
            }
            UserMagic.nTranPoint += nTranPoint;
        }

        public bool CheckMagicLevelup(TUserMagic UserMagic)
        {
            bool result = false;
            int n10;
            if ((UserMagic.btLevel < 4) && (UserMagic.MagicInfo.btTrainLv >= UserMagic.btLevel))
            {
                n10 = UserMagic.btLevel;
            }
            else
            {
                n10 = 0;
            }
            if ((UserMagic.MagicInfo.btTrainLv > UserMagic.btLevel) && (UserMagic.MagicInfo.MaxTrain[n10] <= UserMagic.nTranPoint))
            {
                if (UserMagic.MagicInfo.btTrainLv > UserMagic.btLevel)
                {
                    UserMagic.nTranPoint -= UserMagic.MagicInfo.MaxTrain[n10];
                    UserMagic.btLevel++;
                    SendUpdateDelayMsg(this, grobal2.RM_MAGIC_LVEXP, 0, UserMagic.MagicInfo.wMagicID, UserMagic.btLevel, UserMagic.nTranPoint, "", 800);
                    sub_4C713C(UserMagic);
                }
                else
                {
                    UserMagic.nTranPoint = UserMagic.MagicInfo.MaxTrain[n10];
                }
                result = true;
            }
            return result;
        }

        public virtual void SetTargetCreat(TBaseObject BaseObject)
        {
            m_TargetCret = BaseObject;
            m_dwTargetFocusTick = HUtil32.GetTickCount();
        }

        public virtual void DelTargetCreat()
        {
            m_TargetCret = null;
        }

        public void RecallSlave(string sSlaveName)
        {
            short nX = 0;
            short nY = 0;
            int nFlag;
            nFlag = -1;
            GetFrontPosition(ref nX, ref nY);
            if (sSlaveName == M2Share.g_Config.sDragon)
            {
                nFlag = 1;
            }
            for (int i = m_SlaveList.Count - 1; i >= 0; i--)
            {
                if (nFlag == 1)
                {
                    if ((m_SlaveList[i].m_sCharName == M2Share.g_Config.sDragon) || (m_SlaveList[i].m_sCharName == M2Share.g_Config.sDragon1))
                    {
                        m_SlaveList[i].SpaceMove(m_PEnvir.sMapName, nX, nY, 1);
                        break;
                    }
                }
                else if (m_SlaveList[i].m_sCharName == sSlaveName)
                {
                    m_SlaveList[i].SpaceMove(m_PEnvir.sMapName, nX, nY, 1);
                    break;
                }
            }
        }

        // 攻击角色
        public bool _Attack_DirectAttack(TBaseObject BaseObject, int nSecPwr)
        {
            bool result = false;
            if ((m_btRaceServer == grobal2.RC_PLAYOBJECT) || (BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT) || !(InSafeZone() && BaseObject.InSafeZone()))
            {
                if (IsProperTarget(BaseObject))
                {
                    if (M2Share.RandomNumber.Random(BaseObject.m_btSpeedPoint) < m_btHitPoint)
                    {
                        BaseObject.StruckDamage(nSecPwr);
                        BaseObject.SendDelayMsg(grobal2.RM_STRUCK, grobal2.RM_10101, (short)nSecPwr, BaseObject.m_WAbil.HP, BaseObject.m_WAbil.MaxHP, ObjectId, "", 500);
                        if (BaseObject.m_btRaceServer != grobal2.RC_PLAYOBJECT)
                        {
                            BaseObject.SendMsg(BaseObject, grobal2.RM_STRUCK, (short)nSecPwr, BaseObject.m_WAbil.HP, BaseObject.m_WAbil.MaxHP, ObjectId, "");
                        }
                        result = true;
                    }
                }
            }
            return result;
        }

        // 刺杀前面一个位置的攻击
        public bool _Attack_SwordLongAttack(int nSecPwr)
        {
            bool result;
            short nX = 0;
            short nY = 0;
            TBaseObject BaseObject;
            result = false;
            nSecPwr = HUtil32.Round(nSecPwr * M2Share.g_Config.nSwordLongPowerRate / 100);
            if (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, m_btDirection, 2, ref nX, ref nY))
            {
                BaseObject = (TBaseObject)m_PEnvir.GetMovingObject(nX, nY, true);
                if (BaseObject != null)
                {
                    if ((nSecPwr > 0) && IsProperTarget(BaseObject))
                    {
                        result = _Attack_DirectAttack(BaseObject, nSecPwr);
                        SetTargetCreat(BaseObject);
                    }
                    result = true;
                }
            }
            return result;
        }

        // 半月攻击
        public bool _Attack_SwordWideAttack(int nSecPwr)
        {
            bool result;
            int nC;
            int n10;
            short nX = 0;
            short nY = 0;
            TBaseObject BaseObject;
            result = false;
            nC = 0;
            while (true)
            {
                n10 = (m_btDirection + M2Share.g_Config.WideAttack[nC]) % 8;
                if (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, n10, 1, ref nX, ref nY))
                {
                    BaseObject = (TBaseObject)m_PEnvir.GetMovingObject(nX, nY, true);
                    if ((nSecPwr > 0) && (BaseObject != null) && IsProperTarget(BaseObject))
                    {
                        result = _Attack_DirectAttack(BaseObject, nSecPwr);
                        SetTargetCreat(BaseObject);
                    }
                }
                nC++;
                if (nC >= 3)
                {
                    break;
                }
            }
            return result;
        }

        public bool _Attack_CrsWideAttack(int nSecPwr)
        {
            bool result;
            int nC = 0;
            int n10 = 0;
            short nX = 0;
            short nY = 0;
            TBaseObject BaseObject;
            result = false;
            nC = 0;
            while (true)
            {
                n10 = (m_btDirection + M2Share.g_Config.CrsAttack[nC]) % 8;
                if (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, n10, 1, ref nX, ref nY))
                {
                    BaseObject = (TBaseObject)m_PEnvir.GetMovingObject(nX, nY, true);
                    if ((nSecPwr > 0) && (BaseObject != null) && IsProperTarget(BaseObject))
                    {
                        result = _Attack_DirectAttack(BaseObject, nSecPwr);
                        SetTargetCreat(BaseObject);
                    }
                }
                nC++;
                if (nC >= 7)
                {
                    break;
                }
            }
            return result;
        }

        public void _Attack_sub_4C1E5C_sub_4C1DC0(ref TBaseObject BaseObject, byte btDir, ref short nX, ref short nY, int nSecPwr)
        {
            if (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, btDir, 1, ref nX, ref nY))
            {
                BaseObject = (TBaseObject)m_PEnvir.GetMovingObject(nX, nY, true);
                if ((nSecPwr > 0) && (BaseObject != null))
                {
                    _Attack_DirectAttack(BaseObject, nSecPwr);
                }
            }
        }

        public void _Attack_sub_4C1E5C(int nSecPwr)
        {
            byte btDir = 0;
            short nX = 0;
            short nY = 0;
            TBaseObject BaseObject = null;
            btDir = m_btDirection;
            m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, btDir, 1, ref nX, ref nY);
            _Attack_sub_4C1E5C_sub_4C1DC0(ref BaseObject, btDir, ref nX, ref nY, nSecPwr);
            btDir = M2Share.sub_4B2F80(m_btDirection, 2);
            _Attack_sub_4C1E5C_sub_4C1DC0(ref BaseObject, btDir, ref nX, ref nY, nSecPwr);
            btDir = M2Share.sub_4B2F80(m_btDirection, 6);
            _Attack_sub_4C1E5C_sub_4C1DC0(ref BaseObject, btDir, ref nX, ref nY, nSecPwr);
        }

        public bool _Attack(ref short wHitMode, TBaseObject AttackTarget)
        {
            int n20;
            bool result = false;
            try
            {
                bool bo21 = false;
                int nWeaponDamage = 0;
                int nPower = 0;
                int nSecPwr = 0;
                if (AttackTarget != null)
                {
                    nPower = GetAttackPower(HUtil32.LoWord(m_WAbil.DC), HUtil32.HiWord(m_WAbil.DC) - HUtil32.LoWord(m_WAbil.DC));
                    if ((wHitMode == 3) && m_boPowerHit)
                    {
                        m_boPowerHit = false;
                        nPower += m_nHitPlus;
                        bo21 = true;
                    }
                    if ((wHitMode == 7) && m_boFireHitSkill) // 烈火剑法
                    {
                        m_boFireHitSkill = false;
                        m_dwLatestFireHitTick = HUtil32.GetTickCount();// Jacky 禁止双烈火
                        nPower = nPower + HUtil32.Round(nPower / 100 * (m_nHitDouble * 10));
                        bo21 = true;
                    }
                    if ((wHitMode == 9) && m_boTwinHitSkill) // 烈火剑法
                    {
                        m_boTwinHitSkill = false;
                        m_dwLatestTwinHitTick = HUtil32.GetTickCount();// Jacky 禁止双烈火
                        nPower = nPower + HUtil32.Round(nPower / 100 * (m_nHitDouble * 10));
                        bo21 = true;
                    }
                }
                else
                {
                    nPower = GetAttackPower(HUtil32.LoWord(m_WAbil.DC), HUtil32.HiWord(m_WAbil.DC) - HUtil32.LoWord(m_WAbil.DC));
                    if ((wHitMode == 3) && m_boPowerHit)
                    {
                        m_boPowerHit = false;
                        nPower += m_nHitPlus;
                        bo21 = true;
                    }
                    // Jacky 防止砍空刀刀烈火
                    if ((wHitMode == 7) && m_boFireHitSkill)
                    {
                        m_boFireHitSkill = false;
                        m_dwLatestFireHitTick = HUtil32.GetTickCount();// Jacky 禁止双烈火
                    }
                    if ((wHitMode == 9) && m_boTwinHitSkill)
                    {
                        m_boTwinHitSkill = false;
                        m_dwLatestTwinHitTick = HUtil32.GetTickCount();// Jacky 禁止双烈火
                    }
                }
                if (wHitMode == 4)
                {
                    // 刺杀
                    nSecPwr = 0;
                    if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                    {
                        if (m_MagicErgumSkill != null)
                        {
                            nSecPwr = HUtil32.Round(nPower / (m_MagicErgumSkill.MagicInfo.btTrainLv + 2) * (m_MagicErgumSkill.btLevel + 2));
                        }
                    }
                    if (nSecPwr > 0)
                    {
                        if (!_Attack_SwordLongAttack(nSecPwr) && M2Share.g_Config.boLimitSwordLong)
                        {
                            wHitMode = 0;
                        }
                    }
                }
                if (wHitMode == 5)
                {
                    nSecPwr = 0;
                    if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                    {
                        if (m_MagicBanwolSkill != null)
                        {
                            nSecPwr = HUtil32.Round(nPower / (m_MagicBanwolSkill.MagicInfo.btTrainLv + 10) * (m_MagicBanwolSkill.btLevel + 2));
                        }
                    }
                    if (nSecPwr > 0)
                    {
                        _Attack_SwordWideAttack(nSecPwr);
                    }
                }
                if (wHitMode == 12)
                {
                    nSecPwr = 0;
                    if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                    {
                        if (m_MagicRedBanwolSkill != null)
                        {
                            nSecPwr = HUtil32.Round(nPower / (m_MagicRedBanwolSkill.MagicInfo.btTrainLv + 10) * (m_MagicRedBanwolSkill.btLevel + 2));
                        }
                    }
                    if (nSecPwr > 0)
                    {
                        _Attack_SwordWideAttack(nSecPwr);
                    }
                }

                if (wHitMode == 6)
                {
                    nSecPwr = 0;
                    if (nSecPwr > 0)
                    {
                        _Attack_sub_4C1E5C(nSecPwr);
                    }
                }
                if (wHitMode == 8)
                {
                    nSecPwr = 0;
                    if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                    {
                        if (m_MagicCrsSkill != null)
                        {
                            nSecPwr = HUtil32.Round(nPower / (m_MagicCrsSkill.MagicInfo.btTrainLv + 10) * (m_MagicCrsSkill.btLevel + 2));
                        }
                    }
                    if (nSecPwr > 0)
                    {
                        _Attack_CrsWideAttack(nSecPwr);
                    }
                }
                if (AttackTarget == null)
                {
                    return result;
                }
                if (IsProperTarget(AttackTarget))
                {
                    if (AttackTarget.m_btHitPoint > 0)
                    {
                        if (m_btHitPoint < M2Share.RandomNumber.Random(AttackTarget.m_btSpeedPoint))
                        {
                            nPower = 0;
                        }
                    }
                }
                else
                {
                    nPower = 0;
                }

                if (nPower > 0)
                {
                    nPower = AttackTarget.GetHitStruckDamage(this, nPower);
                    nWeaponDamage = M2Share.RandomNumber.Random(5) + 2 - m_AddAbil.btWeaponStrong;
                }

                if (nPower > 0)
                {
                    AttackTarget.StruckDamage(nPower);
                    AttackTarget.SendDelayMsg(grobal2.RM_STRUCK, grobal2.RM_10101, (short)nPower, AttackTarget.m_WAbil.HP, AttackTarget.m_WAbil.MaxHP, ObjectId, "", 200);
                    if (!AttackTarget.m_boUnParalysis && m_boParalysis && (M2Share.RandomNumber.Random(AttackTarget.m_btAntiPoison + M2Share.g_Config.nAttackPosionRate) == 0))
                    {
                        AttackTarget.MakePosion(grobal2.POISON_STONE, M2Share.g_Config.nAttackPosionTime, 0);
                    }
                    // 虹魔，吸血
                    if (m_nHongMoSuite > 0)
                    {
                        m_db3B0 = nPower / 100 * m_nHongMoSuite;
                        if (m_db3B0 >= 2.0)
                        {
                            n20 = Convert.ToInt32(m_db3B0);
                            m_db3B0 = n20;
                            DamageHealth(-n20);
                        }
                    }

                    if ((m_MagicOneSwordSkill != null) && (m_btRaceServer == grobal2.RC_PLAYOBJECT) && (m_MagicOneSwordSkill.btLevel < 3) && (m_MagicOneSwordSkill.MagicInfo.TrainLevel[m_MagicOneSwordSkill.btLevel] <= m_Abil.Level))
                    {
                        (this as TPlayObject).TrainSkill(m_MagicOneSwordSkill, M2Share.RandomNumber.Random(3) + 1);
                        if (!(this as TPlayObject).CheckMagicLevelup(m_MagicOneSwordSkill))
                        {
                            SendDelayMsg(this, grobal2.RM_MAGIC_LVEXP, 0, m_MagicOneSwordSkill.MagicInfo.wMagicID, m_MagicOneSwordSkill.btLevel, m_MagicOneSwordSkill.nTranPoint, "", 3000);
                        }
                    }
                    if (bo21 && (m_MagicPowerHitSkill != null) && (m_btRaceServer == grobal2.RC_PLAYOBJECT) && (m_MagicPowerHitSkill.btLevel < 3) && (m_MagicPowerHitSkill.MagicInfo.TrainLevel[m_MagicPowerHitSkill.btLevel] <= m_Abil.Level))
                    {
                        (this as TPlayObject).TrainSkill(m_MagicPowerHitSkill, M2Share.RandomNumber.Random(3) + 1);
                        if (!(this as TPlayObject).CheckMagicLevelup(m_MagicPowerHitSkill))
                        {
                            SendDelayMsg(this, grobal2.RM_MAGIC_LVEXP, 0, m_MagicPowerHitSkill.MagicInfo.wMagicID, m_MagicPowerHitSkill.btLevel, m_MagicPowerHitSkill.nTranPoint, "", 3000);
                        }
                    }

                    if ((wHitMode == 4) && (m_MagicErgumSkill != null) && (m_btRaceServer == grobal2.RC_PLAYOBJECT) && (m_MagicErgumSkill.btLevel < 3) && (m_MagicErgumSkill.MagicInfo.TrainLevel[m_MagicErgumSkill.btLevel] <= m_Abil.Level))
                    {
                        (this as TPlayObject).TrainSkill(m_MagicErgumSkill, 1);
                        if (!(this as TPlayObject).CheckMagicLevelup(m_MagicErgumSkill))
                        {
                            SendDelayMsg(this, grobal2.RM_MAGIC_LVEXP, 0, m_MagicErgumSkill.MagicInfo.wMagicID, m_MagicErgumSkill.btLevel, m_MagicErgumSkill.nTranPoint, "", 3000);
                        }
                    }

                    if ((wHitMode == 5) && (m_MagicBanwolSkill != null) && (m_btRaceServer == grobal2.RC_PLAYOBJECT) && (m_MagicBanwolSkill.btLevel < 3) && (m_MagicBanwolSkill.MagicInfo.TrainLevel[m_MagicBanwolSkill.btLevel] <= m_Abil.Level))
                    {
                        (this as TPlayObject).TrainSkill(m_MagicBanwolSkill, 1);
                        if (!(this as TPlayObject).CheckMagicLevelup(m_MagicBanwolSkill))
                        {
                            SendDelayMsg(this, grobal2.RM_MAGIC_LVEXP, 0, m_MagicBanwolSkill.MagicInfo.wMagicID, m_MagicBanwolSkill.btLevel, m_MagicBanwolSkill.nTranPoint, "", 3000);
                        }
                    }
                    if ((wHitMode == 12) && (m_MagicRedBanwolSkill != null) && (m_btRaceServer == grobal2.RC_PLAYOBJECT) && (m_MagicRedBanwolSkill.btLevel < 3) && (m_MagicRedBanwolSkill.MagicInfo.TrainLevel[m_MagicRedBanwolSkill.btLevel] <= m_Abil.Level))
                    {
                        (this as TPlayObject).TrainSkill(m_MagicRedBanwolSkill, 1);
                        if (!(this as TPlayObject).CheckMagicLevelup(m_MagicRedBanwolSkill))
                        {
                            SendDelayMsg(this, grobal2.RM_MAGIC_LVEXP, 0, m_MagicRedBanwolSkill.MagicInfo.wMagicID, m_MagicRedBanwolSkill.btLevel, m_MagicRedBanwolSkill.nTranPoint, "", 3000);
                        }
                    }

                    if ((wHitMode == 7) && (m_MagicFireSwordSkill != null) && (m_btRaceServer == grobal2.RC_PLAYOBJECT) && (m_MagicFireSwordSkill.btLevel < 3) && (m_MagicFireSwordSkill.MagicInfo.TrainLevel[m_MagicFireSwordSkill.btLevel] <= m_Abil.Level))
                    {
                        (this as TPlayObject).TrainSkill(m_MagicFireSwordSkill, 1);
                        if (!(this as TPlayObject).CheckMagicLevelup(m_MagicFireSwordSkill))
                        {
                            SendDelayMsg(this, grobal2.RM_MAGIC_LVEXP, 0, m_MagicFireSwordSkill.MagicInfo.wMagicID, m_MagicFireSwordSkill.btLevel, m_MagicFireSwordSkill.nTranPoint, "", 3000);
                        }
                    }
                    if ((wHitMode == 8) && (m_MagicCrsSkill != null) && (m_btRaceServer == grobal2.RC_PLAYOBJECT) && (m_MagicCrsSkill.btLevel < 3) && (m_MagicCrsSkill.MagicInfo.TrainLevel[m_MagicCrsSkill.btLevel] <= m_Abil.Level))
                    {
                        (this as TPlayObject).TrainSkill(m_MagicCrsSkill, 1);
                        if (!(this as TPlayObject).CheckMagicLevelup(m_MagicCrsSkill))
                        {
                            SendDelayMsg(this, grobal2.RM_MAGIC_LVEXP, 0, m_MagicCrsSkill.MagicInfo.wMagicID, m_MagicCrsSkill.btLevel, m_MagicCrsSkill.nTranPoint, "", 3000);
                        }
                    }
                    if ((wHitMode == 9) && (m_MagicTwnHitSkill != null) && (m_btRaceServer == grobal2.RC_PLAYOBJECT) && (m_MagicTwnHitSkill.btLevel < 3) && (m_MagicTwnHitSkill.MagicInfo.TrainLevel[m_MagicTwnHitSkill.btLevel] <= m_Abil.Level))
                    {
                        (this as TPlayObject).TrainSkill(m_MagicTwnHitSkill, 1);
                        if (!(this as TPlayObject).CheckMagicLevelup(m_MagicTwnHitSkill))
                        {
                            SendDelayMsg(this, grobal2.RM_MAGIC_LVEXP, 0, m_MagicTwnHitSkill.MagicInfo.wMagicID, m_MagicTwnHitSkill.btLevel, m_MagicTwnHitSkill.nTranPoint, "", 3000);
                        }
                    }
                    result = true;
                    if (M2Share.g_Config.boMonDelHptoExp)
                    {
                        if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                        {
                            if ((this as TPlayObject).m_WAbil.Level <= M2Share.g_Config.MonHptoExpLevel)
                            {
                                if (!M2Share.GetNoHptoexpMonList(AttackTarget.m_sCharName))
                                {
                                    (this as TPlayObject).GainExp(nPower * M2Share.g_Config.MonHptoExpmax);
                                }
                            }
                        }
                        if (m_btRaceServer == grobal2.RC_PLAYCLONE)
                        {
                            if (m_Master != null)
                            {
                                if ((m_Master as TPlayObject).m_WAbil.Level <= M2Share.g_Config.MonHptoExpLevel)
                                {
                                    if (!M2Share.GetNoHptoexpMonList(AttackTarget.m_sCharName))
                                    {
                                        (m_Master as TPlayObject).GainExp(nPower * M2Share.g_Config.MonHptoExpmax);
                                    }
                                }
                            }
                        }
                    }
                }

                if ((nWeaponDamage > 0) && (m_UseItems[grobal2.U_WEAPON] != null) && (m_UseItems[grobal2.U_WEAPON].wIndex > 0))
                {
                    DoDamageWeapon(nWeaponDamage);
                }
                if (AttackTarget.m_btRaceServer != grobal2.RC_PLAYOBJECT)
                {
                    AttackTarget.SendMsg(AttackTarget, grobal2.RM_STRUCK, (short)nPower, AttackTarget.m_WAbil.HP, AttackTarget.m_WAbil.MaxHP, ObjectId, "");
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(e.Message);
            }
            return result;
        }

        public void SendAttackMsg(short wIdent, byte btDir, int nX, int nY)
        {
            SendRefMsg(wIdent, btDir, nX, nY, 0, "");
        }

        public int GetHitStruckDamage(TBaseObject Target, int nDamage)
        {
            int result;
            int nArmor;
            int nRnd = HUtil32.HiWord(m_WAbil.AC) - HUtil32.LoWord(m_WAbil.AC) + 1;
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
                if ((m_btLifeAttrib == grobal2.LA_UNDEAD) && (Target != null))
                {
                    nDamage += Target.m_AddAbil.btUndead;
                }
                if (m_boAbilMagBubbleDefence)
                {
                    nDamage = HUtil32.Round(nDamage / 1.0e2 * (m_btMagBubbleDefenceLevel + 2) * 8.0);
                    DamageBubbleDefence(nDamage);
                }
            }
            result = nDamage;
            return result;
        }

        public int GetMagStruckDamage(TBaseObject BaseObject, int nDamage)
        {
            int result;
            int n14 = HUtil32.LoWord(m_WAbil.MAC) + M2Share.RandomNumber.Random(HUtil32.HiWord(m_WAbil.MAC) - HUtil32.LoWord(m_WAbil.MAC) + 1);
            nDamage = HUtil32._MAX(0, nDamage - n14);
            if ((m_btLifeAttrib == grobal2.LA_UNDEAD) && (BaseObject != null))
            {
                nDamage += m_AddAbil.btUndead;
            }
            if ((nDamage > 0) && m_boAbilMagBubbleDefence)
            {
                nDamage = HUtil32.Round(nDamage / 1.0e2 * (m_btMagBubbleDefenceLevel + 2) * 8.0);
                DamageBubbleDefence(nDamage);
            }
            result = nDamage;
            return result;
        }

        public void StruckDamage(int nDamage)
        {
            int nDam;
            int nDura;
            int nOldDura;
            TPlayObject PlayObject;
            TItem StdItem;
            bool bo19;
            if (nDamage <= 0)
            {
                return;
            }
            // 人攻击怪物
            if ((m_btRaceServer >= 50) && (m_LastHiter != null) && (m_LastHiter.m_btRaceServer == grobal2.RC_PLAYOBJECT))
            {
                switch (m_LastHiter.m_btJob)
                {
                    case 0:
                        nDamage = nDamage * M2Share.g_Config.nWarrMon / 10;
                        break;
                    case 1:
                        nDamage = nDamage * M2Share.g_Config.nWizardMon / 10;
                        break;
                    case 2:
                        nDamage = nDamage * M2Share.g_Config.nTaosMon / 10;
                        break;
                }
            }
            // 怪物攻击人
            if ((m_btRaceServer == grobal2.RC_PLAYOBJECT) && (m_LastHiter != null) && (m_LastHiter.m_Master != null))
            {
                nDamage = nDamage * M2Share.g_Config.nMonHum / 10;
            }
            nDam = M2Share.RandomNumber.Random(10) + 5;  // 1 0x62
            if (m_wStatusTimeArr[grobal2.POISON_DAMAGEARMOR] > 0)
            {
                nDam = HUtil32.Round(nDam * (M2Share.g_Config.nPosionDamagarmor / 10));// 1.2
                nDamage = HUtil32.Round(nDamage * (M2Share.g_Config.nPosionDamagarmor / 10));// 1.2
            }
            bo19 = false;
            if (m_UseItems[grobal2.U_DRESS] != null && m_UseItems[grobal2.U_DRESS].wIndex > 0)
            {
                nDura = m_UseItems[grobal2.U_DRESS].Dura;
                nOldDura = HUtil32.Round(nDura / 1000);
                nDura -= nDam;
                if (nDura <= 0)
                {
                    if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                    {
                        PlayObject = this as TPlayObject;
                        PlayObject.SendDelItems(m_UseItems[grobal2.U_DRESS]);
                        StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[grobal2.U_DRESS].wIndex);
                        if (StdItem.NeedIdentify == 1)
                        {
                            // UserEngine.GetStdItemName(m_UseItems[U_DRESS].wIndex) + #9 +
                            M2Share.AddGameDataLog('3' + "\t" + m_sMapName + "\t" + m_nCurrX.ToString() + "\t" + m_nCurrY.ToString() + "\t" + m_sCharName + "\t" + StdItem.Name + "\t" + m_UseItems[grobal2.U_DRESS].MakeIndex.ToString() + "\t" 
                                + HUtil32.BoolToIntStr(m_btRaceServer == grobal2.RC_PLAYOBJECT) + "\t" + '0');
                        }
                        m_UseItems[grobal2.U_DRESS].wIndex = 0;
                        FeatureChanged();
                    }
                    m_UseItems[grobal2.U_DRESS].wIndex = 0;
                    m_UseItems[grobal2.U_DRESS].Dura = 0;
                    bo19 = true;
                }
                else
                {
                    m_UseItems[grobal2.U_DRESS].Dura = (short)nDura;
                }
                if (nOldDura != HUtil32.Round(nDura / 1000))
                {
                    SendMsg(this, grobal2.RM_DURACHANGE, grobal2.U_DRESS, nDura, m_UseItems[grobal2.U_DRESS].DuraMax, 0, "");
                }
            }
            for (var i = m_UseItems.GetLowerBound(0); i <= m_UseItems.GetUpperBound(0); i++)
            {
                if ((m_UseItems[i] != null) && (m_UseItems[i].wIndex > 0) && (M2Share.RandomNumber.Random(8) == 0))
                {
                    nDura = m_UseItems[i].Dura;
                    nOldDura = HUtil32.Round(nDura / 1000);
                    nDura -= nDam;
                    if (nDura <= 0)
                    {
                        if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                        {
                            PlayObject = this as TPlayObject;
                            PlayObject.SendDelItems(m_UseItems[i]);
                            StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[i].wIndex);
                            if (StdItem.NeedIdentify == 1)
                            {
                                M2Share.AddGameDataLog('3' + "\t" + m_sMapName + "\t" + m_nCurrX.ToString() + "\t" + m_nCurrY.ToString() + "\t" + m_sCharName + "\t" + StdItem.Name + "\t" + m_UseItems[i].MakeIndex.ToString() + "\t" 
                                    + HUtil32.BoolToIntStr(m_btRaceServer == grobal2.RC_PLAYOBJECT) + "\t" + '0');
                            }
                            m_UseItems[i].wIndex = 0;
                            FeatureChanged();
                        }
                        m_UseItems[i].wIndex = 0;
                        m_UseItems[i].Dura = 0;
                        bo19 = true;
                    }
                    else
                    {
                        m_UseItems[i].Dura = (short)nDura;
                    }
                    if (nOldDura != HUtil32.Round(nDura / 1000))
                    {
                        SendMsg(this, grobal2.RM_DURACHANGE, (short)i, nDura, m_UseItems[i].DuraMax, 0, "");
                    }
                }
            }
            if (bo19)
            {
                RecalcAbilitys();
            }
            DamageHealth(nDamage);
        }

        public string GeTBaseObjectInfo()
        {
            string result = m_sCharName + ' ' + "地图:" + m_sMapName + '(' + m_PEnvir.sMapDesc + ") " + "座标:" + m_nCurrX.ToString() + '/' + m_nCurrY.ToString() + ' ' + "等级:" + m_Abil.Level.ToString() + ' ' + "经验:" + m_Abil.Exp.ToString() + ' ' 
                + "生命值: " + m_WAbil.HP.ToString() + '-' + m_WAbil.MaxHP.ToString() + ' ' + "魔法值: " + m_WAbil.MP.ToString() + '-' + m_WAbil.MaxMP.ToString() + ' ' + "攻击力: " + HUtil32.LoWord(m_WAbil.DC).ToString() + '-' + HUtil32.HiWord(m_WAbil.DC).ToString() + ' ' 
                + "魔法力: " + HUtil32.LoWord(m_WAbil.MC).ToString() + '-' + HUtil32.HiWord(m_WAbil.MC).ToString() + ' ' + "道术: " + HUtil32.LoWord(m_WAbil.SC).ToString() + '-' + HUtil32.HiWord(m_WAbil.SC).ToString() + ' ' 
                + "防御力: " + HUtil32.LoWord(m_WAbil.AC).ToString() + '-' + HUtil32.HiWord(m_WAbil.AC).ToString() + ' ' + "魔防力: " + HUtil32.LoWord(m_WAbil.MAC).ToString() + '-' + HUtil32.HiWord(m_WAbil.MAC).ToString() + ' ' + "准确:" + m_btHitPoint.ToString() + ' '
                + "敏捷:" + m_btSpeedPoint.ToString();
            return result;
        }

        public bool GetBackPosition(ref short nX, ref short nY)
        {
            bool result;
            TEnvirnoment Envir;
            Envir = m_PEnvir;
            nX = m_nCurrX;
            nY = m_nCurrY;
            switch (m_btDirection)
            {
                case grobal2.DR_UP:
                    if (nY < (Envir.wHeight - 1))
                    {
                        nY++;
                    }
                    break;
                case grobal2.DR_DOWN:
                    if (nY > 0)
                    {
                        nY -= 1;
                    }
                    break;
                case grobal2.DR_LEFT:
                    if (nX < (Envir.wWidth - 1))
                    {
                        nX++;
                    }
                    break;
                case grobal2.DR_RIGHT:
                    if (nX > 0)
                    {
                        nX -= 1;
                    }
                    break;
                case grobal2.DR_UPLEFT:
                    if ((nX < (Envir.wWidth - 1)) && (nY < (Envir.wHeight - 1)))
                    {
                        nX++;
                        nY++;
                    }
                    break;
                case grobal2.DR_UPRIGHT:
                    if ((nX < (Envir.wWidth - 1)) && (nY > 0))
                    {
                        nX -= 1;
                        nY++;
                    }
                    break;
                case grobal2.DR_DOWNLEFT:
                    if ((nX > 0) && (nY < (Envir.wHeight - 1)))
                    {
                        nX++;
                        nY -= 1;
                    }
                    break;
                case grobal2.DR_DOWNRIGHT:
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
            int nOldCharStatus;
            if (nType < grobal2.MAX_STATUS_ATTRIBUTE)
            {
                nOldCharStatus = m_nCharStatus;
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
                m_nCharStatus = GetCharStatus();
                m_btGreenPoisoningPoint = (byte)nPoint;
                if (nOldCharStatus != m_nCharStatus)
                {
                    StatusChanged();
                }
                if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                {
                    SysMsg(format(M2Share.sYouPoisoned, new object[] { nTime, nPoint }), TMsgColor.c_Red, TMsgType.t_Hint);
                }
                result = true;
            }
            return result;
        }

        public bool sub_4DD704()
        {
            bool result = false;
            SendMessage SendMessage;
            HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            try
            {
                for (int i = 0; i < m_MsgList.Count; i++)
                {
                    SendMessage = m_MsgList[i];
                    if (SendMessage.wIdent == grobal2.RM_10401)
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

        public bool sub_4C5370(short nX, short nY, int nRange, ref short nDX, ref short nDY)
        {
            bool result = false;
            if (m_PEnvir.GetMovingObject(nX, nY, true) == null)
            {
                result = true;
                nDX = nX;
                nDY = nY;
            }
            if (!result)
            {
                for (int i = 0; i <= nRange; i++)
                {
                    for (int j = -i; j <= i; j++)
                    {
                        for (int k = -i; k <= i; k++)
                        {
                            nDX = (short)(nX + k);
                            nDY = (short)(nY + j);
                            if (m_PEnvir.GetMovingObject(nDX, nDY, true) == null)
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
            for (int i = 0; i < m_MagicList.Count; i++)
            {
                UserMagic = m_MagicList[i];
                if ((UserMagic != null) && (UserMagic.wMagIdx == nIndex))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void DamageBubbleDefence(int nInt)
        {
            if (m_wStatusTimeArr[grobal2.STATE_BUBBLEDEFENCEUP] > 0)
            {
                if (m_wStatusTimeArr[grobal2.STATE_BUBBLEDEFENCEUP] > 3)
                {
                    m_wStatusTimeArr[grobal2.STATE_BUBBLEDEFENCEUP] -= 3;
                }
                else
                {
                    m_wStatusTimeArr[grobal2.STATE_BUBBLEDEFENCEUP] = 1;
                }
            }
        }

        public bool IsGuildMaster()
        {
            bool result = false;
            if ((m_MyGuild != null) && (m_nGuildRankNo == 1))
            {
                result = true;
            }
            return result;
        }

        public bool MagCanHitTarget(short nX, short nY, TBaseObject TargeTBaseObject)
        {
            bool result = false;
            int n18;
            if (TargeTBaseObject == null)
            {
                return result;
            }
            int n20 = Math.Abs(nX - TargeTBaseObject.m_nCurrX) + Math.Abs(nY - TargeTBaseObject.m_nCurrY);
            int n14 = 0;
            while (n14 < 13)
            {
                n18 = M2Share.GetNextDirection(nX, nY, TargeTBaseObject.m_nCurrX, TargeTBaseObject.m_nCurrY);
                if (m_PEnvir.GetNextPosition(nX, nY, n18, 1, ref nX, ref nY) && m_PEnvir.IsValidCell(nX, nY))
                {
                    if ((nX == TargeTBaseObject.m_nCurrX) && (nY == TargeTBaseObject.m_nCurrY))
                    {
                        result = true;
                        break;
                    }
                    else
                    {
                        int n1C = Math.Abs(nX - TargeTBaseObject.m_nCurrX) + Math.Abs(nY - TargeTBaseObject.m_nCurrY);
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

        public bool IsProperFriend_IsFriend(TBaseObject cret)
        {
            bool result = false;
            if (cret.m_btRaceServer == grobal2.RC_PLAYOBJECT)
            {
                switch (m_btAttatckMode)
                {
                    case M2Share.HAM_ALL:
                        result = true;
                        break;
                    case M2Share.HAM_PEACE:
                        result = true;
                        break;
                    case M2Share.HAM_DEAR:
                        if ((this == cret) || (cret == (this as TPlayObject).m_DearHuman))
                        {
                            result = true;
                        }
                        break;
                    case M2Share.HAM_MASTER:
                        if (this == cret)
                        {
                            result = true;
                        }
                        else if ((this as TPlayObject).m_boMaster)
                        {
                            for (int i = 0; i < (this as TPlayObject).m_MasterList.Count; i++)
                            {
                                if ((this as TPlayObject).m_MasterList[i] == cret)
                                {
                                    result = true;
                                    break;
                                }
                            }
                        }
                        else if ((cret as TPlayObject).m_boMaster)
                        {
                            for (int i = 0; i < (cret as TPlayObject).m_MasterList.Count; i++)
                            {
                                if ((cret as TPlayObject).m_MasterList[i] == this)
                                {
                                    result = true;
                                    break;
                                }
                            }
                        }
                        break;
                    case M2Share.HAM_GROUP:
                        if (cret == this)
                        {
                            result = true;
                        }
                        if (IsGroupMember(cret))
                        {
                            result = true;
                        }
                        break;
                    case M2Share.HAM_GUILD:
                        if (cret == this)
                        {
                            result = true;
                        }
                        if (m_MyGuild != null)
                        {
                            if (m_MyGuild.IsMember(cret.m_sCharName))
                            {
                                result = true;
                            }
                            if (m_boGuildWarArea && (cret.m_MyGuild != null))
                            {
                                if (m_MyGuild.IsAllyGuild(cret.m_MyGuild))
                                {
                                    result = true;
                                }
                            }
                        }
                        break;
                    case M2Share.HAM_PKATTACK:
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

        public virtual bool IsProperFriend(TBaseObject BaseObject)
        {
            bool result = false;
            if (BaseObject == null)
            {
                return result;
            }
            if (m_btRaceServer >= grobal2.RC_ANIMAL)
            {
                if (BaseObject.m_btRaceServer >= grobal2.RC_ANIMAL)
                {
                    result = true;
                }
                if (BaseObject.m_Master != null)
                {
                    result = false;
                }
                return result;
            }
            if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
            {
                result = IsProperFriend_IsFriend(BaseObject);
                if (BaseObject.m_btRaceServer < grobal2.RC_ANIMAL)
                {
                    return result;
                }
                if (BaseObject.m_Master == this)
                {
                    result = true;
                    return result;
                }
                if (BaseObject.m_Master != null)
                {
                    result = IsProperFriend_IsFriend(BaseObject.m_Master);
                    return result;
                }
            }
            else
            {
                result = true;
            }
            return result;
        }

        public int MagMakeDefenceArea(int nX, int nY, int nRange, int nSec, byte btState)
        {
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            TBaseObject BaseObject;
            int result = 0;
            int nStartX = nX - nRange;
            int nEndX = nX + nRange;
            int nStartY = nY - nRange;
            int nEndY = nY + nRange;
            for (int i = nStartX; i <= nEndX; i++)
            {
                for (int j = nStartY; j <= nEndY; j++)
                {
                    if (m_PEnvir.GetMapCellInfo(i, j, ref MapCellInfo) && (MapCellInfo.ObjList != null))
                    {
                        for (int k = 0; k < MapCellInfo.ObjList.Count; k++)
                        {
                            OSObject = MapCellInfo.ObjList[k];
                            if ((OSObject != null) && (OSObject.btType == grobal2.OS_MOVINGOBJECT))
                            {
                                BaseObject = OSObject.CellObj as TBaseObject;
                                if ((BaseObject != null) && (!BaseObject.m_boGhost))
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

        public bool DefenceUp(int nSec)
        {
            bool result = false;
            if (m_wStatusTimeArr[grobal2.STATE_DEFENCEUP] > 0)
            {
                if (m_wStatusTimeArr[grobal2.STATE_DEFENCEUP] < nSec)
                {
                    m_wStatusTimeArr[grobal2.STATE_DEFENCEUP] = (ushort)nSec;
                    result = true;
                }
            }
            else
            {
                m_wStatusTimeArr[grobal2.STATE_DEFENCEUP] = (ushort)nSec;
                result = true;
            }
            m_dwStatusArrTick[grobal2.STATE_DEFENCEUP] = HUtil32.GetTickCount();
            SysMsg(format(M2Share.g_sDefenceUpTime, nSec), TMsgColor.c_Green, TMsgType.t_Hint);
            RecalcAbilitys();
            SendMsg(this, grobal2.RM_ABILITY, 0, 0, 0, 0, "");
            return result;
        }

        public bool AttPowerUp(int nPower, int nTime)
        {
            bool result = false;
            m_wStatusArrValue[0] = (short)nPower;
            m_dwStatusArrTimeOutTick[0] = HUtil32.GetTickCount() + nTime * 1000;
            int nMin = nTime / 60;
            int nSec = nTime % 60;
            SysMsg(format(M2Share.g_sAttPowerUpTime, new object[] { nMin, nSec }), TMsgColor.c_Green, TMsgType.t_Hint);
            RecalcAbilitys();
            SendMsg(this, grobal2.RM_ABILITY, 0, 0, 0, 0, "");
            result = true;
            return result;
        }

        public bool MagDefenceUp(int nSec)
        {
            bool result = false;
            if (m_wStatusTimeArr[grobal2.STATE_MAGDEFENCEUP] > 0)
            {
                if (m_wStatusTimeArr[grobal2.STATE_MAGDEFENCEUP] < nSec)
                {
                    m_wStatusTimeArr[grobal2.STATE_MAGDEFENCEUP] = (ushort)nSec;
                    result = true;
                }
            }
            else
            {
                m_wStatusTimeArr[grobal2.STATE_MAGDEFENCEUP] = (ushort)nSec;
                result = true;
            }
            m_dwStatusArrTick[grobal2.STATE_MAGDEFENCEUP] = HUtil32.GetTickCount();
            SysMsg(format(M2Share.g_sMagDefenceUpTime, nSec), TMsgColor.c_Green, TMsgType.t_Hint);
            RecalcAbilitys();
            SendMsg(this, grobal2.RM_ABILITY, 0, 0, 0, 0, "");
            return result;
        }

        /// <summary>
        /// 魔法盾
        /// </summary>
        /// <returns></returns>
        public bool MagBubbleDefenceUp(int nLevel, int nSec)
        {
            bool result = false;
            int nOldStatus;
            if (m_wStatusTimeArr[grobal2.STATE_BUBBLEDEFENCEUP] != 0)
            {
                return result;
            }
            nOldStatus = m_nCharStatus;
            m_wStatusTimeArr[grobal2.STATE_BUBBLEDEFENCEUP] = (ushort)nSec;
            m_dwStatusArrTick[grobal2.STATE_BUBBLEDEFENCEUP] = HUtil32.GetTickCount();
            m_nCharStatus = GetCharStatus();
            if (nOldStatus != m_nCharStatus)
            {
                StatusChanged();
            }
            m_boAbilMagBubbleDefence = true;
            m_btMagBubbleDefenceLevel = (byte)nLevel;
            result = true;
            return result;
        }

        public TUserItem sub_4C4CD4(string sItemName, ref int nCount)
        {
            TUserItem result = null;
            string sName;
            nCount = 0;
            for (int i = m_UseItems.GetLowerBound(0); i <= m_UseItems.GetUpperBound(0); i++)
            {
                if (m_UseItems[i] == null)
                {
                    continue;
                }
                sName = M2Share.UserEngine.GetStdItemName(m_UseItems[i].wIndex);
                if (string.Compare(sName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = m_UseItems[i];
                    nCount++;
                }
            }
            return result;
        }

        public TUserItem CheckItems(string sItemName)
        {
            TUserItem result = null;
            TUserItem UserItem;
            for (int i = 0; i < m_ItemList.Count; i++)
            {
                UserItem = m_ItemList[i];
                if (UserItem == null)
                {
                    continue;
                }
                if (string.Compare(M2Share.UserEngine.GetStdItemName(UserItem.wIndex), sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = UserItem;
                    break;
                }
            }
            return result;
        }

        public int sub_4C3538()
        {
            int result;
            int nC;
            int n10;
            result = 0;
            nC = -1;
            while (nC != 2)
            {
                n10 = -1;
                while (n10 != 2)
                {
                    if (!m_PEnvir.CanWalk(m_nCurrX + nC, m_nCurrY + n10, false))
                    {
                        if ((nC != 0) || (n10 != 0))
                        {
                            result++;
                        }
                    }
                    n10++;
                }
                nC++;
            }
            return result;
        }

        public bool DelBagItem(int nIndex)
        {
            bool result = false;
            if ((nIndex < 0) || (nIndex >= m_ItemList.Count))
            {
                return result;
            }
            Dispose(m_ItemList[nIndex]);
            m_ItemList.RemoveAt(nIndex);
            result = true;
            return result;
        }

        public bool DelBagItem(int nItemIndex, string sItemName)
        {
            TUserItem UserItem;
            bool result = false;
            for (int i = 0; i < m_ItemList.Count; i++)
            {
                UserItem = m_ItemList[i];
                if ((UserItem.MakeIndex == nItemIndex) && string.Compare(M2Share.UserEngine.GetStdItemName(UserItem.wIndex), sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    Dispose(UserItem);
                    m_ItemList.RemoveAt(i);
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

