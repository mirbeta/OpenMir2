using System;
using System.Collections;
using System.Drawing;
using System.IO;
using SystemModule;

namespace RobotSvr
{
    public class MShare
    {
        public static THumTitle[] g_Titles;
        public static THumTitle g_ActiveTitle = null;
        public static THumTitle[] g_hTitles;
        public static THumTitle g_hActiveTitle = null;
#if DEBUG_LOGIN
        public static byte g_fWZLFirst = 7;
#else
        public static byte g_fWZLFirst = 7;
#endif
        public static bool g_boLogon = false;
        public static bool g_fGoAttack = false;
        // g_QueryWinBottomTick: Longword;
        // g_fGetRenderBottom: Boolean = False;
        public static int g_nDragonRageStateIndex = 0;
        public static int AAX = 26 + 14;
        public static int LMX = 30;
        public static int LMY = 26;
        public static int VIEWWIDTH = 8;
        public static Rectangle g_SkidAD_Rect = null;
        public static Rectangle g_SkidAD_Rect2 = null;
        public static Rectangle G_RC_SQUENGINER = null;
        public static Rectangle G_RC_IMEMODE = null;
        // ====================================物品====================================
        public static byte g_BuildBotTex = 0;
        public static byte g_WinBottomType = 0;
        public static bool g_Windowed = false;
        // g_pkeywords: PString = nil;
        public static bool g_boAutoPickUp = true;
        public static bool g_boPickUpAll = false;
        public static int g_ptItems_Pos = -1;
        public static int g_ptItems_Type = 0;
        public static TCnHashTableSmall g_ItemsFilter_All = null;
        public static TCnHashTableSmall g_ItemsFilter_All_Def = null;
        public static ArrayList g_ItemsFilter_Dress = null;
        public static ArrayList g_ItemsFilter_Weapon = null;
        public static ArrayList g_ItemsFilter_Headgear = null;
        public static ArrayList g_ItemsFilter_Drug = null;
        public static ArrayList g_ItemsFilter_Other = null;
        public static ArrayList g_xMapDescList = null;
        public static ArrayList g_xCurMapDescList = null;
        public static byte[] g_pWsockAddr = new byte[4 + 1];
        // g_dwImgThreadId: Longword = 0;
        // g_hImagesThread: THandle = INVALID_HANDLE_VALUE;
        public static int g_nMagicRange = 8;
        public static int g_TileMapOffSetX = 9;
        public static int g_TileMapOffSetY = 9;
        public static byte g_btMyEnergy = 0;
        public static byte g_btMyLuck = 0;
        public static TItemShine g_tiOKShow =
    {0, 0};
        public static TItemShine g_tiFailShow =
    {0, 0};
        public static TItemShine g_tiOKShow2 =
    {0, 0};
        public static TItemShine g_tiFailShow2 =
    {0, 0};
        public static TItemShine g_spOKShow2 =
    {0, 0};
        public static TItemShine g_spFailShow2 =
    {0, 0};
        public static string g_tiHintStr1 = "";
        public static string g_tiHintStr2 = "";
        public static TMovingItem[] g_TIItems = new TMovingItem[1 + 1];
        public static string g_spHintStr1 = "";
        public static string g_spHintStr2 = "";
        public static TMovingItem[] g_spItems = new TMovingItem[1 + 1];
        public static int g_SkidAD_Count = 0;
        public static int g_SkidAD_Count2 = 0;
        public static string g_lastHeroSel = String.Empty;
        public static THeroInfo[] g_heros;
        public static byte g_ItemWear = 0;
        public static byte g_ShowSuite = 0;
        public static byte g_ShowSuite2 = 0;
        public static byte g_ShowSuite3 = 0;
        public static byte g_SuiteSpSkill = 0;
        public static int g_SuiteIdx = -1;
        public static ArrayList g_SuiteItemsList = null;
        public static ArrayList g_TitlesList = null;
        public static byte g_btSellType = 0;
        public static bool g_showgamegoldinfo = false;
        public static bool SSE_AVAILABLE = false;
        public static int g_lWavMaxVol = 68;
        // -100;
        public static long g_uDressEffectTick = 0;
        public static long g_sDressEffectTick = 0;
        public static long g_hDressEffectTick = 0;
        public static int g_uDressEffectIdx = 0;
        public static int g_sDressEffectIdx = 0;
        public static int g_hDressEffectIdx = 0;
        public static long g_uWeaponEffectTick = 0;
        public static long g_sWeaponEffectTick = 0;
        public static long g_hWeaponEffectTick = 0;
        public static int g_uWeaponEffectIdx = 0;
        public static int g_sWeaponEffectIdx = 0;
        public static int g_hWeaponEffectIdx = 0;
        public static int g_ChatWindowLines = 12;
        public static bool g_LoadBeltConfig = false;
        public static bool g_BeltMode = true;
        public static int g_BeltPositionX = 408;
        public static int g_BeltPositionY = 487;
        public static int g_dwActorLimit = 5;
        public static int g_nProcActorIDx = 0;
        public static bool g_boPointFlash = false;
        public static long g_PointFlashTick = 0;
        public static bool g_boHPointFlash = false;
        public static long g_HPointFlashTick = 0;
        public static TVenationInfo[] g_VenationInfos = {
    {0, 0} ,
    {0, 0} ,
    {0, 0} ,
    {0, 0} };
        // 经络信息
        public static TVenationInfo[] g_hVenationInfos = {
    {0, 0} ,
    {0, 0} ,
    {0, 0} ,
    {0, 0} };
        // 经络信息
        public static bool g_NextSeriesSkill = false;
        public static long g_dwSeriesSkillReadyTick = 0;
        public static int g_nCurrentMagic = 888;
        public static int g_nCurrentMagic2 = 888;
        public static long g_SendFireSerieSkillTick = 0;
        public static long g_IPointLessHintTick = 0;
        public static long g_MPLessHintTick = 0;
        public static int g_SeriesSkillStep = 0;
        public static bool g_SeriesSkillFire_100 = false;
        public static bool g_SeriesSkillFire = false;
        public static bool g_SeriesSkillReady = false;
        public static bool g_SeriesSkillReadyFlash = false;
        // g_TempMagicArr            : array[0..3] of TTempSeriesSkillA;
        public static byte[] g_TempSeriesSkillArr;
        public static byte[] g_HTempSeriesSkillArr;
        public static byte[] g_SeriesSkillArr = new byte[3 + 1];
        // TSeriesSkill;
        public static ArrayList g_SeriesSkillSelList = null;
        public static ArrayList g_hSeriesSkillSelList = null;
        public static long g_dwAutoTecTick = 0;
        public static long g_dwAutoTecHeroTick = 0;
        // g_ProcOnIdleTick: Longword;
        // g_boProcMessagePacket: Boolean = False;
        public static long g_dwProcMessagePacket = 0;
        public static long g_ProcNowTick = 0;
        public static bool g_ProcCanFill = true;
        // g_ProcOnDrawTick: Longword;
        public static long g_ProcOnDrawTick_Effect = 0;
        public static long g_ProcOnDrawTick_Effect2 = 0;
        // g_ProcCanDraw: Boolean;
        // g_ProcCanDraw_Effect      : Boolean;
        // g_ProcCanDraw_Effect2     : Boolean;
        public static long g_dwImgMgrTick = 0;
        public static int g_nImgMgrIdx = 0;
        // ProcImagesCS              : TRTLCriticalSection;
        public static TRTLCriticalSection ProcMsgCS = null;
        public static TRTLCriticalSection ThreadCS = null;
        public static bool g_bIMGBusy = false;
        // g_dwCurrentTick           : PLongWord;
        public static long g_dwThreadTick = 0;
        public static long g_rtime = 0;
        public static long g_dwLastThreadTick = 0;
        public static bool g_boExchgPoison = false;
        public static bool g_boCheckTakeOffPoison = false;
        public static int g_Angle = 0;
        public static bool g_ShowMiniMapXY = false;
        public static bool g_DrawingMiniMap = false;
        public static bool g_DrawMiniBlend = false;
        // g_MiniMapRC: TRect;
        public static bool g_boTakeOnPos = true;
        public static bool g_boHeroTakeOnPos = true;
        public static bool g_boQueryDynCode = false;
        public static bool g_boQuerySelChar = false;
        // g_pRcHeader: pTRcHeader;
        // g_bLoginKey: PBoolean;
        // g_pbInitSock: PBoolean;
        public static bool g_pbRecallHero = false;
        // g_dwCheckTick             : LongWord = 0;
        public static int g_dwCheckCount = 0;
        public static int g_nBookPath = 0;
        public static int g_nBookPage = 0;
        public static int g_HillMerchant = 0;
        public static string g_sBookLabel = "";
        public static int g_MaxExpFilter = 2000;
        public static bool g_boDrawLevelRank = false;
        public static THeroLevelRank[] g_HeroLevelRanks;
        public static THumanLevelRank[] g_HumanLevelRanks;
        public static string[] g_UnBindItems = { "万年雪霜", "疗伤药", "强效太阳水", "强效金创药", "强效魔法药", "金创药(小量)", "魔法药(小量)", "金创药(中量)", "魔法药(中量)", "地牢逃脱卷", "随机传送卷", "回城卷", "行会回城卷" };
        public static string g_sLogoText = "LegendSoft";
        public static string g_sGoldName = "金币";
        public static string g_sGameGoldName = "元宝";
        public static string g_sGamePointName = "泡点";
        public static string g_sWarriorName = "武士";
        // 职业名称
        public static string g_sWizardName = "魔法师";
        // 职业名称
        public static string g_sTaoistName = "道士";
        // 职业名称
        public static string g_sUnKnowName = "未知";
        public static string g_sMainParam1 = String.Empty;
        // 读取设置参数
        public static string g_sMainParam2 = String.Empty;
        // 读取设置参数
        public static string g_sMainParam3 = String.Empty;
        // 读取设置参数
        public static string g_sMainParam4 = String.Empty;
        // 读取设置参数
        public static string g_sMainParam5 = String.Empty;
        // 读取设置参数
        public static string g_sMainParam6 = String.Empty;
        // 读取设置参数
        public static bool g_boCanDraw = true;
        public static bool g_boInitialize = false;
        public static int g_nInitializePer = 0;
        public static bool g_boQueryExit = false;
        public static string g_FontName = String.Empty;
        public static int g_FontSize = 0;
        public static byte[] g_PowerBlock = { 0x55, 0x8B, 0xEC, 0x83, 0xC4, 0xE8, 0x89, 0x55, 0xF8, 0x89, 0x45, 0xFC, 0xC7, 0x45, 0xEC, 0xE8, 0x03, 0x00, 0x00, 0xC7, 0x45, 0xE8, 0x64, 0x00, 0x00, 0x00, 0xDB, 0x45, 0xEC, 0xDB, 0x45, 0xE8, 0xDE, 0xF9, 0xDB, 0x45, 0xFC, 0xDE, 0xC9, 0xDD, 0x5D, 0xF0, 0x9B, 0x8B, 0x45, 0xF8, 0x8B, 0x00, 0x8B, 0x55, 0xF8, 0x89, 0x02, 0xDD, 0x45, 0xF0, 0x8B, 0xE5, 0x5D, 0xC3, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        public static byte[] g_PowerBlock1 = { 0x55, 0x8B, 0xEC, 0x83, 0xC4, 0xE8, 0x89, 0x55, 0xF8, 0x89, 0x45, 0xFC, 0xC7, 0x45, 0xEC, 0x64, 0x00, 0x00, 0x00, 0xC7, 0x45, 0xE8, 0x64, 0x00, 0x00, 0x00, 0xDB, 0x45, 0xEC, 0xDB, 0x45, 0xE8, 0xDE, 0xF9, 0xDB, 0x45, 0xFC, 0xDE, 0xC9, 0xDD, 0x5D, 0xF0, 0x9B, 0x8B, 0x45, 0xF8, 0x8B, 0x00, 0x8B, 0x55, 0xF8, 0x89, 0x02, 0xDD, 0x45, 0xF0, 0x8B, 0xE5, 0x5D, 0xC3, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        // g_RegInfo                 : TRegInfo;
        public static string g_sServerName = String.Empty;
        // 服务器显示名称
        public static string g_sServerMiniName = String.Empty;
        // 服务器名称
        public static string g_psServerAddr = String.Empty;
        public static int g_pnServerPort = 0;
        public static string g_sSelChrAddr = String.Empty;
        public static int g_nSelChrPort = 0;
        public static string g_sRunServerAddr = String.Empty;
        public static int g_nRunServerPort = 0;
        public static int g_nTopDrawPos = 0;
        public static int g_nLeftDrawPos = 0;
        public static bool g_boSendLogin = false;
        // 是否发送登录消息
        public static bool g_boServerConnected = false;
        public static bool g_SoftClosed = false;
        // 小退游戏标记
        public static TChrAction g_ChrAction;
        public static TConnectionStep g_ConnectionStep;
        // g_boSound                 : Boolean;
        // g_boBGSound               : Boolean;
        // g_MainFont: string = '宋体';
        // g_FontArr: array[0..MAXFONT - 1] of string = (
        // '宋体',
        // '新宋体',
        // '仿宋',
        // '楷体',
        // 'Courier New',
        // 'Arial',
        // 'MS Sans Serif',
        // 'Microsoft Sans Serif'
        // );
        // g_OldTime: Longword;
        // g_nCurFont: Integer = 0;
        // g_sCurFontName: string = '宋体';
        public static bool g_boFullScreen = false;
        public static byte g_btMP3Volume = 70;
        public static byte g_btSoundVolume = 70;
        public static bool g_boBGSound = true;
        public static bool g_boSound = true;
        public static bool g_DlgInitialize = false;
        // g_HintSurface_W: TDirectDrawSurface;
        // g_BotSurface: TDirectDrawSurface;
        public static bool g_boFirstActive = true;
        public static bool g_boFirstTime = false;
        public static string g_sMapTitle = String.Empty;
        public static int g_nLastMapMusic = -1;
        public static ArrayList g_SendSayList = null;
        public static int g_SendSayListIdx = 0;
        public static ArrayList g_ServerList = null;
        public static THStringList g_GroupMembers = null;
        public static ArrayList g_SoundList = null;
        public static ArrayList BGMusicList = null;
        public static long g_DxFontsMgrTick = 0;
        public static TClientMagic[,] g_MagicArr = new TClientMagic[2 + 1, 255 + 1];
        public static ArrayList g_MagicList = null;
#if SERIESSKILL
        public static ArrayList g_MagicList2 = null;
        public static ArrayList g_hMagicList2 = null;
#endif // SERIESSKILL
        public static ArrayList g_IPMagicList = null;
        public static ArrayList g_HeroMagicList = null;
        public static ArrayList g_HeroIPMagicList = null;
        public static ArrayList[] g_ShopListArr = new ArrayList[5 + 1];
        public static ArrayList g_SaveItemList = null;
        public static ArrayList g_MenuItemList = null;
        public static ArrayList g_DropedItemList = null;
        public static ArrayList g_ChangeFaceReadyList = null;
        public static ArrayList g_FreeActorList = null;
        public static int g_PoisonIndex = 0;
        public static int g_nBonusPoint = 0;
        public static int g_nSaveBonusPoint = 0;
        public static TNakedAbility g_BonusTick = null;
        public static TNakedAbility g_BonusAbil = null;
        public static TNakedAbility g_NakedAbil = null;
        public static TNakedAbility g_BonusAbilChg = null;
        public static string g_sGuildName = String.Empty;
        public static string g_sGuildRankName = String.Empty;
        public static long g_dwLatestJoinAttackTick = 0;
        // 最后魔法攻击时间
        public static long g_dwLastAttackTick = 0;
        // 最后攻击时间(包括物理攻击及魔法攻击)
        public static long g_dwLastMoveTick = 0;
        // 最后移动时间
        public static long g_dwLatestSpellTick = 0;
        // 最后魔法攻击时间
        public static long g_dwLatestFireHitTick = 0;
        // 最后列火攻击时间
        public static long g_dwLatestSLonHitTick = 0;
        // 最后列火攻击时间
        public static long g_dwLatestTwinHitTick = 0;
        public static long g_dwLatestPursueHitTick = 0;
        public static long g_dwLatestRushHitTick = 0;
        public static long g_dwLatestSmiteHitTick = 0;
        public static long g_dwLatestSmiteLongHitTick = 0;
        public static long g_dwLatestSmiteLongHitTick2 = 0;
        public static long g_dwLatestSmiteLongHitTick3 = 0;
        public static long g_dwLatestSmiteWideHitTick = 0;
        public static long g_dwLatestSmiteWideHitTick2 = 0;
        public static long g_dwLatestRushRushTick = 0;
        // 最后被推动时间
        // g_dwLatestStruckTick      : LongWord; //最后弯腰时间
        // g_dwLatestHitTick         : LongWord; //最后物理攻击时间(用来控制攻击状态不能退出游戏)
        // g_dwLatestMagicTick       : LongWord; //最后放魔法时间(用来控制攻击状态不能退出游戏)
        public static long g_dwMagicDelayTime = 0;
        public static long g_dwMagicPKDelayTime = 0;
        public static int g_nMouseCurrX = 0;
        // 鼠标所在地图位置座标X
        public static int g_nMouseCurrY = 0;
        // 鼠标所在地图位置座标Y
        public static int g_nMouseX = 0;
        // 鼠标所在屏幕位置座标X
        public static int g_nMouseY = 0;
        // 鼠标所在屏幕位置座标Y
        public static int g_nTargetX = 0;
        // 目标座标
        public static int g_nTargetY = 0;
        // 目标座标
        public static TActor g_TargetCret = null;
        public static TActor g_FocusCret = null;
        public static TActor g_MagicTarget = null;
        public static Link g_APQueue = null;
        public static ArrayList g_APPathList = null;
        public static double[,] g_APPass;
        // array[0..MAXX * 3, 0..MAXY * 3] of DWORD;
        public static TActor g_APTagget = null;
        // /////////////////////////////
        public static long g_APRunTick = 0;
        public static long g_APRunTick2 = 0;
        public static TDropItem g_AutoPicupItem = null;
        public static int g_nAPStatus = 0;
        public static bool g_boAPAutoMove = false;
        public static bool g_boLongHit = false;
        public static string g_sAPStr = String.Empty;
        public static int g_boAPXPAttack = 0;
        public static long m_dwSpellTick = 0;
        public static long m_dwRecallTick = 0;
        public static long m_dwDoubluSCTick = 0;
        public static long m_dwPoisonTick = 0;
        public static int m_btMagPassTh = 0;
        public static int g_nTagCount = 0;
        public static long m_dwTargetFocusTick = 0;
        public static THStringList g_APPickUpList = null;
        public static THStringList g_APMobList = null;
        public static THStringList g_ItemDesc = null;
        public static int g_AttackInvTime = 900;
        public static TActor g_AttackTarget = null;
        public static long g_dwSearchEnemyTick = 0;
        public static bool g_boAllowJointAttack = false;
        // ////////////////////////////////////////
        public static byte g_nAPReLogon = 0;
        public static bool g_nAPrlRecallHero = false;
        public static bool g_nAPSendSelChr = false;
        public static bool g_nAPSendNotice = false;
        public static long g_nAPReLogonWaitTick = 0;
        public static int g_nAPReLogonWaitTime = 10 * 1000;
        public static int g_ApLastSelect = 0;
        public static int g_nOverAPZone = 0;
        public static int g_nOverAPZone2 = 0;
        public static bool g_APGoBack = false;
        public static bool g_APGoBack2 = false;
        public static int g_APStep = -1;
        public static int g_APStep2 = -1;
        public static Point[] g_APMapPath;
        public static Point[] g_APMapPath2;
        public static Point g_APLastPoint = null;
        public static Point g_APLastPoint2 = null;
        public static bool g_nApMiniMap = false;
        public static long g_dwBlinkTime = 0;
        public static bool g_boViewBlink = false;
        // g_boAttackSlow            : Boolean;  //腕力不够时慢动作攻击.
        // g_boAttackFast            : Byte = 0;
        // g_boMoveSlow              : Boolean;  //负重不够时慢动作跑
        // g_nMoveSlowLevel          : Integer;
        public static bool g_boMapMoving = false;
        public static bool g_boMapMovingWait = false;
        public static bool g_boCheckBadMapMode = false;
        public static bool g_boCheckSpeedHackDisplay = false;
        public static bool g_boViewMiniMap = false;
        // 是否可视小地图 默认为True
        public static int g_nViewMinMapLv = 0;
        // 小地图显示等级
        public static int g_nMiniMapIndex = 0;
        // 小地图索引编号
        public static int g_nMiniMapX = 0;
        // 小图鼠标指向坐标X
        public static int g_nMiniMapY = 0;
        // 小图鼠标指向坐标Y
        // NPC 相关
        public static int g_nCurMerchant = 0;
        // NPC大对话框
        public static int g_nCurMerchantFaceIdx = 0;
        // Development 2019-01-14
        public static int g_nMDlgX = 0;
        public static int g_nMDlgY = 0;
        public static int g_nStallX = 0;
        public static int g_nStallY = 0;
        public static long g_dwChangeGroupModeTick = 0;
        public static long g_dwDealActionTick = 0;
        public static long g_dwQueryMsgTick = 0;
        public static int g_nDupSelection = 0;
        public static bool g_boAllowGroup = false;
        // 人物信息相关
        public static int g_nMySpeedPoint = 0;
        // 敏捷
        public static int g_nMyHitPoint = 0;
        // 准确
        public static int g_nMyAntiPoison = 0;
        // 魔法躲避
        public static int g_nMyPoisonRecover = 0;
        // 中毒恢复
        public static int g_nMyHealthRecover = 0;
        // 体力恢复
        public static int g_nMySpellRecover = 0;
        // 魔法恢复
        public static int g_nMyAntiMagic = 0;
        // 魔法躲避
        public static int g_nMyHungryState = 0;
        // 饥饿状态
        public static int g_nMyIPowerRecover = 0;
        // 中毒恢复
        public static int g_nMyAddDamage = 0;
        public static int g_nMyDecDamage = 0;
        // g_nMyGameDiamd            : Integer = 0;
        // g_nMyGameGird             : Integer = 0;
        // g_nMyGameGold             : Integer = 0;
        public static int g_nHeroSpeedPoint = 0;
        // 敏捷
        public static int g_nHeroHitPoint = 0;
        // 准确
        public static int g_nHeroAntiPoison = 0;
        // 魔法躲避
        public static int g_nHeroPoisonRecover = 0;
        // 中毒恢复
        public static int g_nHeroHealthRecover = 0;
        // 体力恢复
        public static int g_nHeroSpellRecover = 0;
        // 魔法恢复
        public static int g_nHeroAntiMagic = 0;
        // 魔法躲避
        public static int g_nHeroHungryState = 0;
        // 饥饿状态
        public static int g_nHeroBagSize = 40;
        public static int g_nHeroIPowerRecover = 0;
        public static int g_nHeroAddDamage = 0;
        public static int g_nHeroDecDamage = 0;
        public static short g_wAvailIDDay = 0;
        public static short g_wAvailIDHour = 0;
        public static short g_wAvailIPDay = 0;
        public static short g_wAvailIPHour = 0;
        public static THumActor g_MySelf = null;
        public static THumActor g_MyDrawActor = null;
        public static string g_sAttackMode = "";
        public static string sAttackModeOfAll = "[全体攻击模式]";
        public static string sAttackModeOfPeaceful = "[和平攻击模式]";
        public static string sAttackModeOfDear = "[夫妻攻击模式]";
        public static string sAttackModeOfMaster = "[师徒攻击模式]";
        public static string sAttackModeOfGroup = "[编组攻击模式]";
        public static string sAttackModeOfGuild = "[行会攻击模式]";
        public static string sAttackModeOfRedWhite = "[善恶攻击模式]";
        public static int g_RIWhere = 0;
        public static TMovingItem[] g_RefineItems = new TMovingItem[2 + 1];
        public static int g_BuildAcusesStep = 0;
        public static int g_BuildAcusesProc = 0;
        public static long g_BuildAcusesProcTick = 0;
        public static int g_BuildAcusesSuc = -1;
        public static int g_BuildAcusesSucFrame = 0;
        public static long g_BuildAcusesSucFrameTick = 0;
        public static long g_BuildAcusesFrameTick = 0;
        public static int g_BuildAcusesFrame = 0;
        public static int g_BuildAcusesRate = 0;
        public static TMovingItem[] g_BuildAcuses = new TMovingItem[7 + 1];
        public static int g_BAFirstShape = -1;
        // g_BAFirstShape2           : Integer = -1;
        public static TClientItem[] g_tui = new TClientItem[13 + 1];
        public static TClientItem[] g_UseItems = new TClientItem[Grobal2.U_FASHION + 1];
        public static TClientItem[] g_HeroUseItems = new TClientItem[Grobal2.U_FASHION + 1];
        public static TUserStateInfo UserState1 = null;
        public static TItemShine g_detectItemShine = null;
        public static TItemShine[] UserState1Shine = new TItemShine[Grobal2.U_FASHION + 1];
        public static TItemShine[] g_UseItemsShine = new TItemShine[Grobal2.U_FASHION + 1];
        public static TItemShine[] g_HeroUseItemsShine = new TItemShine[Grobal2.U_FASHION + 1];
        public static TClientItem[] g_ItemArr = new TClientItem[MAXBAGITEMCL - 1 + 1];
        public static TClientItem[] g_HeroItemArr = new TClientItem[MAXBAGITEMCL - 1 + 1];
        public static TItemShine[] g_ItemArrShine = new TItemShine[MAXBAGITEMCL - 1 + 1];
        public static TItemShine[] g_HeroItemArrShine = new TItemShine[MAXBAGITEMCL - 1 + 1];
        public static TItemShine[] g_StallItemArrShine = new TItemShine[10 - 1 + 1];
        public static TItemShine[] g_uStallItemArrShine = new TItemShine[10 - 1 + 1];
        public static TItemShine[] g_DealItemsShine = new TItemShine[10 - 1 + 1];
        public static TItemShine[] g_DealRemoteItemsShine = new TItemShine[20 - 1 + 1];
        public static TItemShine g_MovingItemShine = null;
        public static bool g_boBagLoaded = false;
        public static bool g_boServerChanging = false;
        public static int g_nCaptureSerial = 0;
        // 抓图文件名序号
        // g_nSendCount              : Integer; //发送操作计数
        public static int g_nReceiveCount = 0;
        // 接改操作状态计数
        public static int g_nTestSendCount = 0;
        public static int g_nTestReceiveCount = 0;
        public static int g_nSpellCount = 0;
        // 使用魔法计数
        public static int g_nSpellFailCount = 0;
        // 使用魔法失败计数
        public static int g_nFireCount = 0;
        public static int g_nDebugCount = 0;
        public static int g_nDebugCount1 = 0;
        public static int g_nDebugCount2 = 0;
        // 买卖相关
        public static TClientItem g_SellDlgItem = null;
        public static TMovingItem g_TakeBackItemWait = null;
        public static TMovingItem g_SellDlgItemSellWait = null;
        // TClientItem;
        public static TClientItem g_DetectItem = null;
        public static int g_DetectItemMineID = 0;
        public static TClientItem g_DealDlgItem = null;
        public static bool g_boQueryPrice = false;
        public static long g_dwQueryPriceTime = 0;
        public static string g_sSellPriceStr = String.Empty;
        // 交易相关
        public static TClientItem[] g_DealItems = new TClientItem[9 + 1];
        public static bool g_boYbDealing = false;
        public static TClientPS g_YbDealInfo = null;
        public static TClientItem[] g_YbDealItems = new TClientItem[9 + 1];
        public static TClientItem[] g_DealRemoteItems = new TClientItem[19 + 1];
        public static int g_nDealGold = 0;
        public static int g_nDealRemoteGold = 0;
        public static bool g_boDealEnd = false;
        public static string g_sDealWho = String.Empty;
        public static TClientItem g_MouseItem = null;
        public static TClientItem g_MouseStateItem = null;
        public static TClientItem g_HeroMouseStateItem = null;
        public static TClientItem g_MouseUserStateItem = null;
        public static TClientItem g_HeroMouseItem = null;
        public static TShopItem g_ClickShopItem = null;
        public static bool g_boItemMoving = false;
        public static TMovingItem g_MovingItem = null;
        public static TMovingItem g_OpenBoxItem = null;
        public static TMovingItem g_WaitingUseItem = null;
        public static TMovingItem g_WaitingStallItem = null;
        public static TMovingItem g_WaitingDetectItem = null;
        public static TDropItem g_FocusItem = null;
        public static TDropItem g_FocusItem2 = null;
        public static bool g_boOpenStallSystem = true;
        public static bool g_boAutoLongAttack = true;
        public static bool g_boAutoSay = true;
        public static bool g_boMutiHero = true;
        public static bool g_boSkill_114_MP = false;
        public static bool g_boSkill_68_MP = false;
        public static int g_nDayBright = 0;
        public static int g_nAreaStateValue = 0;
        public static bool g_boNoDarkness = false;
        public static int g_nRunReadyCount = 0;
        public static bool g_boLastViewFog = false;
#if VIEWFOG
        public static bool g_boViewFog = true;
    // 是否显示黑暗
        public static bool g_boForceNotViewFog = true;
    // 免蜡烛
#else
        public static bool g_boViewFog = false;
        // 是否显示黑暗
        public static bool g_boForceNotViewFog = true;
        // 免蜡烛
#endif // VIEWFOG
        public static TClientItem g_EatingItem = null;
        public static long g_dwEatTime = 0;
        // timeout...
        public static long g_dwHeroEatTime = 0;
        public static long g_dwDizzyDelayStart = 0;
        public static long g_dwDizzyDelayTime = 0;
        public static bool g_boDoFadeOut = false;
        // 由亮变暗
        public static bool g_boDoFadeIn = false;
        // 由暗变亮
        public static int g_nFadeIndex = 0;
        public static bool g_boDoFastFadeOut = false;
        public static bool g_boAutoDig = false;
        public static bool g_boAutoSit = false;
        // 自动锄矿
        public static bool g_boSelectMyself = false;
        // 鼠标是否指到自己
        // 游戏速度检测相关变量
        public static long g_dwFirstServerTime = 0;
        public static long g_dwFirstClientTime = 0;
        // ServerTimeGap: int64;
        public static int g_nTimeFakeDetectCount = 0;
        // g_dwSHGetCount            : PLongWord;
        // g_dwSHGetTime             : LongWord;
        // g_dwSHTimerTime           : LongWord;
        // g_nSHFakeCount            : Integer;  //检查机器速度异常次数，如果超过4次则提示速度不稳定
        public static long g_dwLatestClientTime2 = 0;
        public static long g_dwFirstClientTimerTime = 0;
        // timer 矫埃
        public static long g_dwLatestClientTimerTime = 0;
        public static long g_dwFirstClientGetTime = 0;
        // gettickcount 矫埃
        public static long g_dwLatestClientGetTime = 0;
        public static int g_nTimeFakeDetectSum = 0;
        public static int g_nTimeFakeDetectTimer = 0;
        public static long g_dwLastestClientGetTime = 0;
        // 外挂功能变量开始
        public static long g_dwDropItemFlashTime = 5 * 1000;
        // 地面物品闪时间间隔
        public static int g_nHitTime = 1400;
        // 攻击间隔时间间隔  0820
        public static int g_nItemSpeed = 60;
        public static long g_dwSpellTime = 500;
        // 魔法攻间隔时间
        public static bool g_boHero = true;
        public static bool g_boOpenAutoPlay = true;
        public static TColorEffect g_DeathColorEffect = WIL.TColorEffect.ceRed;
        // 死亡颜色  Development 2018-12-29
        public static bool g_boClientCanSet = true;
        public static int g_nEatIteminvTime = 200;
        public static bool g_boCanRunSafeZone = true;
        public static bool g_boCanRunHuman = true;
        public static bool g_boCanRunMon = true;
        public static bool g_boCanRunNpc = true;
        public static bool g_boCanRunAllInWarZone = false;
        public static bool g_boCanStartRun = true;
        // 是否允许免助跑
        public static bool g_boParalyCanRun = false;
        // 麻痹是否可以跑
        public static bool g_boParalyCanWalk = false;
        // 麻痹是否可以走
        public static bool g_boParalyCanHit = false;
        // 麻痹是否可以攻击
        public static bool g_boParalyCanSpell = false;
        // 麻痹是否可以魔法
        public static bool g_boShowRedHPLable = true;
        // 显示血条
        public static bool g_boShowHPNumber = true;
        // 显示血量数字
        public static bool g_boShowJobLevel = true;
        // 显示职业等级
        public static bool g_boDuraAlert = true;
        // 物品持久警告
        public static bool g_boMagicLock = true;
        // 魔法锁定
        public static bool g_boSpeedRate = false;
        public static bool g_boSpeedRateShow = false;
        // g_boAutoPuckUpItem        : Boolean = False;
        public static bool g_boShowHumanInfo = true;
        public static bool g_boShowMonsterInfo = false;
        public static bool g_boShowNpcInfo = false;
        // 外挂功能变量结束
        public static bool g_boQuickPickup = false;
        public static long g_dwAutoPickupTick = 0;
        /// <summary>
        /// 自动捡物品间隔
        /// </summary>
        public static long g_dwAutoPickupTime = 100;
        public static TActor g_MagicLockActor = null;
        public static bool g_boNextTimePowerHit = false;
        public static bool g_boCanLongHit = false;
        public static bool g_boCanWideHit = false;
        public static bool g_boCanCrsHit = false;
        public static bool g_boCanStnHit = false;
        public static bool g_boNextTimeFireHit = false;
        public static bool g_boNextTimeTwinHit = false;
        public static bool g_boNextTimePursueHit = false;
        public static bool g_boNextTimeRushHit = false;
        public static bool g_boNextTimeSmiteHit = false;
        public static bool g_boNextTimeSmiteLongHit = false;
        public static bool g_boNextTimeSmiteLongHit2 = false;
        public static bool g_boNextTimeSmiteLongHit3 = false;
        public static bool g_boNextTimeSmiteWideHit = false;
        public static bool g_boNextTimeSmiteWideHit2 = false;
        public static bool g_boCanSLonHit = false;
        public static bool g_boCanSquHit = false;
        public static THStringList g_ShowItemList = null;
        public static bool g_boDrawTileMap = true;
        public static bool g_boDrawDropItem = true;
        public static int g_nTestX = 71;
        public static int g_nTestY = 212;
        public static int g_nSquHitPoint = 0;
        public static int g_nMaxSquHitPoint = 0;
        public static bool g_boConfigLoaded = false;
        public static byte g_dwCollectExpLv = 0;
        public static bool g_boCollectStateShine = false;
        public static int g_nCollectStateShine = 0;
        public static long g_dwCollectStateShineTick = 0;
        public static long g_dwCollectStateShineTick2 = 0;
        public static long g_dwCollectExp = 0;
        public static long g_dwCollectExpMax = 1;
        public static bool g_boCollectExpShine = false;
        public static int g_boCollectExpShineCount = 0;
        public static long g_dwCollectExpShineTick = 0;
        public static long g_dwCollectIpExp = 0;
        public static long g_dwCollectIpExpMax = 1;
        public static bool g_ReSelChr = false;
        public static bool ShouldUnloadEnglishKeyboardLayout = false;
        public static string LocalModName_Shift = ModName_Shift;
        public static string LocalModName_Ctrl = ModName_Ctrl;
        public static string LocalModName_Alt = ModName_Alt;
        public static string LocalModName_Win = ModName_Win;
        public static int[] g_FSResolutionWidth = { 800, 1024, 1280, 1280, 1366, 1440, 1600, 1680, 1920 };// 电脑分辨率宽度
        public static int[] g_FSResolutionHeight = { 600, 768, 800, 1024, 768, 900, 900, 1050, 1080 };// 电脑分辨率高度
        public static byte g_FScreenMode = 0;
        public static int g_FScreenWidth = SCREENWIDTH;
        public static int g_FScreenHeight = SCREENHEIGHT;
        public static bool g_boClientUI = false;
        public const string REG_SETUP_PATH = "SOFTWARE\\Jason\\Mir2\\Setup";
        public const string REG_SETUP_BITDEPTH = "BitDepth";
        public const string REG_SETUP_DISPLAY = "DisplaySize";
        public const string REG_SETUP_WINDOWS = "Window";
        public const string REG_SETUP_MP3VOLUME = "MusicVolume";
        public const string REG_SETUP_SOUNDVOLUME = "SoundVolume";
        public const string REG_SETUP_MP3OPEN = "MusicOpen";
        public const string REG_SETUP_SOUNDOPEN = "SoundOpen";
        public const int MAXX = 40;
        // SCREENWIDTH div 20;
        public const int MAXY = 30;
        // SCREENWIDTH div 20;
        public const int LONGHEIGHT_IMAGE = 35;
        public const int FLASHBASE = 410;
        public const int SOFFX = 0;
        public const int SOFFY = 0;
        public const int HEALTHBAR_BLACK = 0;
        // HEALTHBAR_RED = 1;
        public const int BARWIDTH = 30;
        public const int BARHEIGHT = 2;
        public const int MAXSYSLINE = 8;
        public const int BOTTOMBOARD = 1;
        public const int AREASTATEICONBASE = 150;
        public const int g_WinBottomRetry = 45;
        // ------------
        public const bool NEWHINTSYS = true;
        // MIR2EX = True;
        public const int NPC_CILCK_INVTIME = 500;
        public const int MAXITEMBOX_WIDTH = 177;
        public const int MAXMAGICLV = 3;
        public const int RUNLOGINCODE = 0;
        public const int CLIENT_VERSION_NUMBER = 120020522;
        public const int STDCLIENT = 0;
        public const int RMCLIENT = 46;
        public const int CLIENTTYPE = RMCLIENT;
        public const int CUSTOMLIBFILE = 0;
        public const int DEBUG = 0;
        public const int LOGICALMAPUNIT = 30;
        // 1108 40;
        public const int HUMWINEFFECTTICK = 200;
        public const int WINLEFT = 100;
        // 窗体左边 图片素材留在左边屏幕内的尺寸为100
        public const int WINTOP = 100;
        // 窗体顶边 图片素材留在顶边屏幕内的尺寸为100
        public const int MINIMAPSIZE = 200;
        // 迷你地图宽度
        public const int DEFAULTCURSOR = 0;
        // 系统默认光标
        public const int IMAGECURSOR = 1;
        // 图形光标
        public const int USECURSOR = DEFAULTCURSOR;
        // 使用什么类型的光标
        public const int MAXBAGITEMCL = 52;
        public const int MAXFONT = 8;
        public const int ENEMYCOLOR = 69;
        public const int HERO_MIIDX_OFFSET = 5000;
        public const int SAVE_MIIDX_OFFSET = HERO_MIIDX_OFFSET + 500;
        public const int STALL_MIIDX_OFFSET = HERO_MIIDX_OFFSET + 500 + 50;
        public const int DETECT_MIIDX_OFFSET = HERO_MIIDX_OFFSET + 500 + 50 + 10 + 1;
        public const int MSGMUCH = 2;
        public static string[] g_sHumAttr = { "金", "木", "水", "火", "土" };
        public static string[] g_DBStateStrArr = { "装", "时", "状", "属", "称", "技", "其" };
        public static string[] g_DBStateStrArrW = { "备", "装", "态", "性", "号", "能", "他" };
        public static string[] g_DBStateStrArrUS = { "装", "时", "称" };
        public static string[] g_DBStateStrArrUSW = { "备", "装", "号" };
        public static string[] g_DBStateStrArr2 = { "状", "技", "经", "连", "其" };
        public static string[] g_DBStateStrArr2W = { "态", "能", "络", "击", "他" };
        public static string[] g_slegend = { "", "传奇神剑", "传奇勋章", "传奇项链", "传奇之冠", "", "传奇护腕", "", "传奇之戒", "", "传奇腰带", "传奇之靴", "", "传奇面巾" };
        public const int MAX_GC_GENERAL = 16;
        public static Rectangle[] g_ptGeneral = new Rectangle[20] {
    {35 + 000, 70 + 23 * 0, 35 + 000 + 72 + 18, 70 + 23 * 0 + 16} ,
    {35 + 000, 70 + 23 * 1, 35 + 000 + 72 + 18, 70 + 23 * 1 + 16} ,
    {35 + 000, 70 + 23 * 2, 35 + 000 + 78 + 18, 70 + 23 * 2 + 16} ,
    {35 + 000, 70 + 23 * 3, 35 + 000 + 96, 70 + 23 * 3 + 16} ,
    {35 + 120, 70 + 23 * 0, 35 + 120 + 72 + 30, 70 + 23 * 0 + 16} ,
    {35 + 120, 70 + 23 * 1, 35 + 120 + 72, 70 + 23 * 1 + 16} ,
    {35 + 120, 70 + 23 * 2, 35 + 120 + 72 + 18, 70 + 23 * 2 + 16} ,
    {35 + 120, 70 + 23 * 3, 35 + 120 + 72, 70 + 23 * 3 + 16} ,
    {35 + 120, 70 + 23 * 4, 35 + 120 + 72 + 18, 70 + 23 * 4 + 16} ,
    {35 + 240, 70 + 23 * 0, 35 + 240 + 72, 70 + 23 * 0 + 16} ,
    {35 + 240, 70 + 23 * 1, 35 + 240 + 72, 70 + 23 * 1 + 16} ,
    {35 + 240, 70 + 23 * 2, 35 + 240 + 48, 70 + 23 * 2 + 16} ,
    {35 + 240, 70 + 23 * 3, 35 + 240 + 72, 70 + 23 * 3 + 16} ,
    {35 + 240, 70 + 23 * 4, 35 + 240 + 72, 70 + 23 * 4 + 16} ,
    {35 + 240, 70 + 23 * 5, 35 + 240 + 72, 70 + 23 * 5 + 16} ,
    {35 + 120, 70 + 23 * 5, 35 + 120 + 72, 70 + 23 * 5 + 16} ,
    {35 + 000, 70 + 23 * 5, 35 + 000 + 96, 70 + 23 * 5 + 16} };
        // 0
        // 1
        // 2
        // 3
        // 4
        // '显示过滤',                     //5
        // 6
        // '拣取过滤',                     //7
        // 8
        // 9
        public static string[] g_caGeneral = { "人名显示(Z)", "持久警告(X)", "免Shift键(C)", "显示经验过滤", "物品提示(ESC)", "显示过滤", "隐藏人物翅膀", "预留(无作用)", "隐藏尸体(V)", "自动修理", "屏幕震动", "音效", "预留", "数字飘血", "装备比较", "稳如泰山", "显示称号" };
        // 0
        // 1
        // 2
        // 3
        // 4
        // 5
        // 6
        // '钩选此项将不自动拣取未显示\在地面上的物品', //7
        // 8
        // 9
        public static string[] g_HintGeneral = { "钩选此项将全屏显示玩家名字", "钩选此项在装备持久低时进行提示", "钩选此项将不需要按Shift也能\\攻击其他玩家", "钩选此项将隐藏聊天栏中低于\\设置的经验值提示", "钩选此项将显示地面未过滤的\\物品的名字", "钩选此项将过滤部分地面物品的显示", "钩选此项将不显示人物翅膀效果，可避免\\电脑较差且翅膀显示过多而导致卡问题", "", "钩选此项将隐藏已死亡的怪物\\尸体，避免资源效果，游戏更流畅", "钩选此项将自动修理身上装备\\包裹需放置有一定持久的修复神水", "钩选此项将触发游戏中的屏幕震动效果", "钩选此项将启用游戏音效反之\\则关闭游戏的音效", "", "钩选此项将触发开启伤害值显示", "是否开启装备比较功能", "钩选此项将屏蔽人物受打击时的后昂动作", "钩选此项将显示人物头顶的称号" };
        public static bool[] g_gcGeneral = { true, true, false, true, true, true, false, true, false, true, true, true, true, false, false, true, true };
        public static Color[] g_clGeneral = { System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver };
        public static Color[] g_clGeneralDef = { System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver };
        // ====================================Protect====================================
        public const int MAX_GC_PROTECT = 11;
        // 0
        // 1
        // 2
        // 3
        // 4
        // 5
        // 6
        // 0
        // 1
        public static Rectangle[] g_ptProtect = new Rectangle[]{
    {35 + 000, 70 + 24 * 0, 35 + 000 + 20, 70 + 24 * 0 + 16} ,
    {35 + 000, 70 + 24 * 1, 35 + 000 + 20, 70 + 24 * 1 + 16} ,
    {35 + 000, 70 + 24 * 2, 35 + 000 + 20, 70 + 24 * 2 + 16} ,
    {35 + 000, 70 + 24 * 3, 35 + 000 + 20, 70 + 24 * 3 + 16} ,
    {35 + 000, 70 + 24 * 4, 35 + 000 + 20, 70 + 24 * 4 + 16} ,
    {35 + 000, 70 + 24 * 5, 35 + 000 + 20, 70 + 24 * 5 + 16} ,
    {35 + 000, 70 + 24 * 6, 35 + 000 + 72, 70 + 24 * 6 + 16} ,
    {35 + 180, 70 + 24 * 0, 35 + 180 + 20, 70 + 24 * 0 + 16} ,
    {35 + 180, 70 + 24 * 1, 35 + 180 + 20, 70 + 24 * 1 + 16} ,
    {35 + 180, 70 + 24 * 3, 35 + 180 + 20, 70 + 24 * 3 + 16} ,
    {35 + 180, 70 + 24 * 5, 35 + 180 + 20, 70 + 24 * 5 + 16} ,
    {35 + 180, 70 + 24 * 6, 35 + 180 + 20, 70 + 24 * 6 + 16} };
        // 0
        // 1
        // 2
        // 3
        // 4
        // 5
        // 6
        // 7
        // 8
        // 9
        // 10
        public static string[] g_caProtect = { "HP               毫秒", "MP               毫秒", "", "HP               毫秒", "", "HP               毫秒", "卷轴类型", "HP               毫秒", "MP               毫秒", "HP               毫秒", "HP", "MP不足允许使用特殊药品" };
        // shape = 2
        // shape = 1
        // shape = 3
        // shape = 5
        public static string[] g_sRenewBooks = { "随机传送卷", "地牢逃脱卷", "回城卷", "行会回城卷", "盟重传送石", "比奇传送石", "随机传送石", "", "", "", "", "" };
        public static bool[] g_gcProtect = { false, false, false, false, false, false, false, true, true, true, false, true };
        public static int[] g_gnProtectPercent = { 10, 10, 10, 10, 10, 10, 0, 88, 88, 88, 20, 00 };
        public static int[] g_gnProtectTime = { 4000, 4000, 4000, 4000, 4000, 4000, 4000, 4000, 4000, 1000, 1000, 1000 };
        public static Color[] g_clProtect = { System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Lime };
        // ====================================Tec====================================
        public const int MAX_GC_TEC = 14;
        // 0
        // 1
        // 2
        // 3
        // 4
        // 5
        // 6
        // 7
        // 8
        // 9
        // 0
        // 1
        // 2
        // 3
        // 4
        // 5
        // 6
        // 7
        // 8
        // 9
        public static string[] g_HintTec = { "钩选此项将开启刀刀刺杀", "钩选此项将开启智能半月", "钩选此项将自动凝聚烈火剑法", "钩选此项将自动凝聚逐日剑法", "钩选此项将自动开启魔法盾", "钩选此项英雄将自动开启魔法盾", "钩选此项道士将自动使用隐身术", "", "", "钩选此项将自动凝聚雷霆剑法", "钩选此项将自动进行隔位刺杀", "钩选此项将自动凝聚断空斩", "钩选此项英雄将不使用连击打怪\\方便玩家之间进行PK", "钩选此项将自动凝聚开天斩", "钩选此项：施展魔法超过允许距离时，会自动跑近目标并释放魔法" };
        // 0
        // 1
        // 2
        // 3
        // 4
        // 5
        // 6
        // 7
        // 8
        // 9
        public static string[] g_caTec = { "刀刀刺杀", "智能半月", "自动烈火", "逐日剑法", "自动开盾", "持续开盾(英雄)", "自动隐身", "时间间隔", "", "自动雷霆", "隔位刺杀", "自动断空斩", "英雄连击不打怪", "自动开天斩", "自动调节魔法距离" };
        public static string[] g_sMagics = { "火球术", "治愈术", "大火球", "施毒术", "攻杀剑术", "抗拒火环", "地狱火", "疾光电影", "雷电术", "雷电术", "雷电术", "雷电术", "雷电术", "开天斩", "开天斩" };
        public const int g_gnTecPracticeKey = 0;
        public static bool[] g_gcTec = { true, true, true, true, true, true, false, false, false, false, false, false, false, true, false };
        public static int[] g_gnTecTime = { 0, 0, 0, 0, 0, 0, 0, 0, 4000, 0, 0, 0, 0, 0, 0 };
        public static Color[] g_clTec = { System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver };
        // ====================================Assistant====================================
        public const int MAX_GC_ASS = 6;
        // 0
        // 1
        // 2
        // 3
        // 4
        // 5
        public static Rectangle[] g_ptAss = {
    {35 + 000, 70 + 24 * 0, 35 + 000 + 142, 70 + 24 * 0 + 16} ,
    {35 + 000, 70 + 24 * 1, 35 + 000 + 72, 70 + 24 * 1 + 16} ,
    {35 + 000, 70 + 24 * 2, 35 + 000 + 72, 70 + 24 * 2 + 16} ,
    {35 + 000, 70 + 24 * 3, 35 + 000 + 72, 70 + 24 * 3 + 16} ,
    {35 + 000, 70 + 24 * 4, 35 + 000 + 72, 70 + 24 * 4 + 16} ,
    {35 + 000, 70 + 24 * 5, 35 + 000 + 120, 70 + 24 * 5 + 16} ,
    {35 + 000, 70 + 24 * 6, 35 + 000 + 120, 70 + 24 * 6 + 16} };
        // 0
        // 1
        // 2
        // 3
        // 4
        public static string[] g_HintAss = { "", "", "", "", "", "可以自己编辑要显示和拾取的物品，开启\\此功能后，将替换掉 [物品] 选项卡的设置", "" };
        // 0
        // 1
        // 2
        // 3
        // 4
        public static string[] g_caAss = { "开启挂机(Ctrl+Alt+X)", "红药用完回城", "蓝药用完回城", "符毒用完回城", "背包满时回城", "自定物品过滤(钩选编辑)", "自定打怪过滤(钩选编辑)" };
        public static bool[] g_gcAss = { false, false, false, false, false, false, false };
        public static Color[] g_clAss = { System.Drawing.Color.Lime, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver };
        // ====================================HotKey====================================
        public const int MAX_GC_HOTKEY = 8;
        // 0
        // 2
        // 3
        // 4
        // 5
        // 6
        // 7
        // 8
        public static string[] g_caHotkey = { "启用自定快捷键", "召唤英雄", "英雄攻击目标", "使用合击技能", "英雄攻击模式", "英雄守护模式", "切换攻击模式", "切换小地图", "释放连击" };
        public static bool[] g_gcHotkey = { false, false, false, false, false, false, false, false, false };
        public static int[] g_gnHotkey = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static Color[] g_clHotkey = { System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Lime };
        public const int MAX_GC_ITEMS = 7;
        public const Rectangle g_ptItemsA =
    {25 + 194, 68 + 18 * 7 + 23, 25 + 194 + 80, 68 + 18 * 7 + 16 + 23};
        public const Rectangle g_ptAutoPickUp =
    {25 + 267, 68 + 18 * 7 + 23, 25 + 267 + 80, 68 + 18 * 7 + 16 + 23};
        // 0
        // 1
        // 2
        // 3
        // 4
        // 5
        public static TCItemRule[] g_caItems = { null, null, null, null, null, null, null, null };
        public static TCItemRule[] g_caItems2 = { null, null, null, null, null, null, null, null };
        public static Color[] g_clItems = { System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver, System.Drawing.Color.Silver };
        public const int MAX_SERIESSKILL_POINT = 4;
        public const int g_HitSpeedRate = 0;
        public const int g_MagSpeedRate = 0;
        public const int g_MoveSpeedRate = 0;
        public const bool g_boFlashMission = false;
        public const bool g_boNewMission = false;
        // 新任务
        public const long g_dwNewMission = 0;
        // 11
        // 21
        // 31
        // 老狮子吼
        // 火焰冰
        // 41
        // 51
        // 59
        // 67
        // 68
        // 69
        // 70
        // 71
        // 72
        // 73
        // 74
        // 75
        // 100
        // 101
        // 102
        // 103
        // 104
        // 105
        // 106
        // 107
        // 108
        // 109
        // 110
        // 111
        public static string[] g_asSkillDesc = { "凝聚自身魔力发射一枚火球\\攻击目标", "释放精神之力恢复自己或者\\他人的体力", "提高自身的攻击命中率", "通过与精神之力沟通，可以\\提高战斗时的命中率", "凝聚自身魔力发射一枚大火\\球攻击目标", "配合特殊药粉可以指定某个\\目标中毒", "攻击时有机率造成大幅伤害", "将身边的人或者怪兽推开", "向前挥出一堵火焰墙，使法\\术区域内的敌人受到伤害", "积蓄一道光电，使直线上所\\有敌人受到伤害", "从空中召唤一道雷电攻击敌人", "隔位施展剑气，使敌人受到\\大幅伤害", "将精神之力附着在护身符上，\\远程攻击目标", "使用护身符提高范围内友方\\的魔法防御力", "使用护身符提高范围内友方\\的防御力", "被限制在咒语中的怪兽不能\\移动或攻击圈外敌人", "使用护身符从地狱深处召唤\\骷髅，分担召唤者受到的伤害", "在自身周围释放精神之力使\\怪物无法察觉你的存在", "通达大量释放精神之力，能\\够隐藏范围内的人", "通过闪光电击使敌人瘫痪，\\甚至可以使怪物成为忠实的仆人", "利用强大魔力打乱空间，从\\而达到随机传送目的的法术", "在地面上产生火焰，使踏入\\的敌人受到伤害", "产生高热的火焰，使法术区\\域内的敌人受到伤害", "能够呼唤出一股强力的雷光\\风暴，伤害所有围在身边的敌人", "使用劲气可同时攻击环绕自\\身周围的敌人", "召唤火精灵附在武器上，从\\而造成强力的额外伤害", "用肩膀把敌人撞开，如果撞\\到障碍物将会对自己造成伤害", "使用精神力查看目标体力", "恢复自己和周围所有玩家的\\体力", "使用护身符召唤一只强大神\\兽作为自己的随从", "使用自身魔力制造一个魔法\\盾减少施法者受到的伤害", "有机率一击杀死不死生物", "召唤强力的暴风雪，使法术\\区域内的敌人受到伤害", "解除友方身上中的各种毒", "", "将法力凝结成冰攻击目标，\\有一定几率使对方暂时石化", "召唤强力的雷电，使法术\\区域内的敌人受到伤害", "配合特殊药粉可以指定某个\\区域内的目标中毒", "彻地钉，武士远程攻击技能", "使用劲气可同时攻击环绕\\自身周围的敌人", "使用劲气如雷震般将自身\\周围怪物暂时石化", "使用劲气造成处于自身前\\方大面积的敌人受伤", "召唤雷电灵附在武器上，\\从而造成强力的额外伤害，\\有一定几率使敌人麻痹", "产生巨大的魔力推力同时\\给敌人造成一定的伤害", "吸取对方一定的MP，同时\\产生巨大的魔力伤害", "", "召唤出可以熔化天日的火龙气焰", "一种内功的修炼，可以推\\开周围的怪物而得以防身的作用", "解除友方身上中的各种中毒状态", "瞬间提升自己的精神力", "飓风破", "诅咒术", "血咒", "骷髅咒", "", "剑气凝聚成形，瞬间化作\\一道光影，突袭身前的敌人", "驱使护身符，伤害同时吸取\\对方生命值", "召唤一阵猛烈的火雨，使\\法术区域内的敌人受到伤害", "", "将雷霆万钧之力凝于双刀之\\尖，对敌人造成致命伤害", "将刀光幻化成缕缕虹光环绕\\四周，对敌人造成致命伤害", "召唤天裂之力于利刃之中，\\对敌人造成致命伤害", "召唤噬魂沼泽，对敌人造成\\致命伤害", "炙热的龙焰与冻绝的冰息，\\汇合成内敛之气，爆发于瞬间", "召唤出可以熔化天日的火龙气焰", "凝聚力量的顶点，幻化出一柄\\巨剑，爆发出毁天灭地的威力", "", "", "", "将自己的属性召唤到身边", "远距离擒获怪物到自身前方", "使用法力自由移动到指定位置", "", "", "可以用来减低对方给予\\你的伤害", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "近身攻击，冲撞单体目标，\\在迫使其后退的同时，造成伤害", "左右开弓，挥剑重砍。\\近身攻击，对单体目标造成伤害", "跳起重击地面造成巨大伤害。\\远程攻击，对三步内的\\单体目标造成伤害", "横扫千军犹入无人之境。\\范围攻击，以自身为中心，\\对5*5范围内造成伤害", "犹如凤凰涅磐般的致命一击。\\远程攻击，对单体目标造成伤害", "跃起后发出强烈的魔法气场。\\远程攻击，对单体目标造成伤害", "蓄力重击，冻裂地面形成冰刺。\\范围攻击，以目标为中心，\\对5*5范围内造成持续伤害", "伤害非常恐怖的双龙出击。\\远程攻击，对单体目标造成伤害", "放出圣兽对目标发起攻击。\\远程攻击，对单体目标造成伤害", "双手运气，推出八卦掌攻击敌人。\\远程攻击，对单体目标造成伤害", "让人难以招架的连环符。\\远程攻击，对单体目标造成伤害", "万剑齐发，天地同归。\\范围攻击，以目标为中心，\\对5*5范围内造成伤害", "范围攻击，以目标为中心，\\对3*3范围内造成持续伤害", "跳起重击地面造成巨大伤害\\远程攻击，对四步内的\\单体目标造成伤害", "跳起重击地面造成巨大伤害\\远程攻击，对屏幕内的\\目标造成巨大伤害", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
        public const int WH_KEYBOARD_LL = 13;
        public const int LLKHF_ALTDOWN = 0x20;
        // Windows 2000/XP multimedia keys (adapted from winuser.h and renamed to avoid potential conflicts)
        // See also: http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/WindowsUserInterface/UserInput/VirtualKeyCodes.asp
        public const int _VK_BROWSER_BACK = 0xA6;
        // Browser Back key
        public const int _VK_BROWSER_FORWARD = 0xA7;
        // Browser Forward key
        public const int _VK_BROWSER_REFRESH = 0xA8;
        // Browser Refresh key
        public const int _VK_BROWSER_STOP = 0xA9;
        // Browser Stop key
        public const int _VK_BROWSER_SEARCH = 0xAA;
        // Browser Search key
        public const int _VK_BROWSER_FAVORITES = 0xAB;
        // Browser Favorites key
        public const int _VK_BROWSER_HOME = 0xAC;
        // Browser Start and Home key
        public const int _VK_VOLUME_MUTE = 0xAD;
        // Volume Mute key
        public const int _VK_VOLUME_DOWN = 0xAE;
        // Volume Down key
        public const int _VK_VOLUME_UP = 0xAF;
        // Volume Up key
        public const int _VK_MEDIA_NEXT_TRACK = 0xB0;
        // Next Track key
        public const int _VK_MEDIA_PREV_TRACK = 0xB1;
        // Previous Track key
        public const int _VK_MEDIA_STOP = 0xB2;
        // Stop Media key
        public const int _VK_MEDIA_PLAY_PAUSE = 0xB3;
        // Play/Pause Media key
        public const int _VK_LAUNCH_MAIL = 0xB4;
        // Start Mail key
        public const int _VK_LAUNCH_MEDIA_SELECT = 0xB5;
        // Select Media key
        public const int _VK_LAUNCH_APP1 = 0xB6;
        // Start Application 1 key
        public const int _VK_LAUNCH_APP2 = 0xB7;
        // Start Application 2 key
        // Self-invented names for the extended keys
        public const string NAME_VK_BROWSER_BACK = "Browser Back";
        public const string NAME_VK_BROWSER_FORWARD = "Browser Forward";
        public const string NAME_VK_BROWSER_REFRESH = "Browser Refresh";
        public const string NAME_VK_BROWSER_STOP = "Browser Stop";
        public const string NAME_VK_BROWSER_SEARCH = "Browser Search";
        public const string NAME_VK_BROWSER_FAVORITES = "Browser Favorites";
        public const string NAME_VK_BROWSER_HOME = "Browser Start/Home";
        public const string NAME_VK_VOLUME_MUTE = "Volume Mute";
        public const string NAME_VK_VOLUME_DOWN = "Volume Down";
        public const string NAME_VK_VOLUME_UP = "Volume Up";
        public const string NAME_VK_MEDIA_NEXT_TRACK = "Next Track";
        public const string NAME_VK_MEDIA_PREV_TRACK = "Previous Track";
        public const string NAME_VK_MEDIA_STOP = "Stop Media";
        public const string NAME_VK_MEDIA_PLAY_PAUSE = "Play/Pause Media";
        public const string NAME_VK_LAUNCH_MAIL = "Start Mail";
        public const string NAME_VK_LAUNCH_MEDIA_SELECT = "Select Media";
        public const string NAME_VK_LAUNCH_APP1 = "Start Application 1";
        public const string NAME_VK_LAUNCH_APP2 = "Start Application 2";
        public const string CONFIGFILE = "Config\\%s.ini";
        // *******************************************************************************
        public const string g_affiche0 = "游戏音效已关闭！";
        public const string g_affiche1 = "健康游戏公告";
        public const string g_affiche2 = "抵制不良游戏 拒绝盗版游戏 注意自我保护 谨防受骗上当 适度游戏益脑";
        public const string g_affiche3 = "沉迷游戏伤身 合理安排时间 享受健康生活 严厉打击赌博 营造和谐环境";
        public const string mmsyst = "winmm.dll";
        public const string kernel32 = "kernel32.dll";
        public const string HotKeyAtomPrefix = "HotKeyManagerHotKey";
        public const string ModName_Shift = "Shift";
        public const string ModName_Ctrl = "Ctrl";
        public const string ModName_Alt = "Alt";
        public const string ModName_Win = "Win";
        public const int VK2_SHIFT = 32;
        public const int VK2_CONTROL = 64;
        public const int VK2_ALT = 128;
        public const int VK2_WIN = 256;
        public const int SCREENWIDTH = 800;
        public const int SCREENHEIGHT = 600;
        public static string[] g_levelstring = { "一", "二", "三", "四", "五", "六", "七", "八" };

        public string GetMySelfStringVar_GetVarTextFieldFunc(string VarText)
        {
            string result;
            result = "";
            // 转换为大写
            VarText = VarText.UpperCase(VarText);
            if (VarText == "$SERVERNAME")
            {
                // 选择角色界面标签变量
                result = MShare.g_sServerName;
            }
            else if (VarText == "$MAP")
            {
                // 地图名称
                result = MShare.g_sMapTitle;
            }
            else if (VarText == "$LOCALTIME")
            {
                // 本地时间
                result = FormatDateTime("HH:MM:SS", DateTime.Now);
            }
            else if (VarText == "$MAPAREASTATE")
            {
                // 当前位置区域状态
                if ((MShare.g_nAreaStateValue & 8) != 0)
                {
                    result = "8";
                }
                else if ((MShare.g_nAreaStateValue & 4) != 0)
                {
                    result = "攻城区域";
                }
                else if ((MShare.g_nAreaStateValue & 2) != 0)
                {
                    result = "安全区域";
                }
                else if ((MShare.g_nAreaStateValue & 1) != 0)
                {
                    result = "竞技区域";
                }
            }
            if ((ClMain.SelectChrScene != null))
            {
                if (VarText == "$SELECTCHRNAME1")
                {
                    result = ClMain.SelectChrScene.ChrArr[0].UserChr.Name;
                }
                else if (VarText == "$SELECTCHRLEVEL1")
                {
                    if (ClMain.SelectChrScene.ChrArr[0].UserChr.Name != "")
                    {
                        result = ClMain.SelectChrScene.ChrArr[0].UserChr.Level.ToString();
                    }
                }
                else if (VarText == "$SELECTCHRJOB1")
                {
                    if (ClMain.SelectChrScene.ChrArr[0].UserChr.Name != "")
                    {
                        result = MShare.GetJobName(ClMain.SelectChrScene.ChrArr[0].UserChr.Job);
                    }
                }
                else if (VarText == "$SELECTCHRNAME2")
                {
                    result = ClMain.SelectChrScene.ChrArr[1].UserChr.Name;
                }
                else if (VarText == "$SELECTCHRLEVEL2")
                {
                    if (ClMain.SelectChrScene.ChrArr[1].UserChr.Name != "")
                    {
                        result = ClMain.SelectChrScene.ChrArr[1].UserChr.Level.ToString();
                    }
                }
                else if (VarText == "$SELECTCHRJOB2")
                {
                    if (ClMain.SelectChrScene.ChrArr[1].UserChr.Name != "")
                    {
                        result = MShare.GetJobName(ClMain.SelectChrScene.ChrArr[1].UserChr.Job);
                    }
                }
            }
            // 人物信息
            if (MShare.g_MySelf != null)
            {
                // 人物属性标签变量
                if (VarText == "$USERNAME")
                {
                    result = MShare.g_MySelf.m_sUserName;
                }
                else if (VarText == "$HP")
                {
                    result = MShare.g_MySelf.m_Abil.HP.ToString();
                }
                else if (VarText == "$MP")
                {
                    result = MShare.g_MySelf.m_Abil.MP.ToString();
                }
                else if (VarText == "$MAXHP")
                {
                    result = MShare.g_MySelf.m_Abil.MaxHP.ToString();
                }
                else if (VarText == "$MAXMP")
                {
                    result = MShare.g_MySelf.m_Abil.MaxMP.ToString();
                }
                else if (VarText == "$X")
                {
                    result = MShare.g_MySelf.m_nCurrX.ToString();
                }
                else if (VarText == "$Y")
                {
                    result = MShare.g_MySelf.m_nCurrY.ToString();
                }
                else if (VarText == "$LEVEL")
                {
                    result = MShare.g_MySelf.m_Abil.Level.ToString();
                }
            }
            // 英雄信息
            if ((MShare.g_MySelf != null) && (MShare.g_MySelf.m_HeroObject != null))
            {
                if (VarText == "$HEROGLORY")
                {
                    result = MShare.g_MySelf.m_HeroObject.m_wGloryPoint.ToString();
                }
            }
            return result;
        }

        // 自定义UI字符串改变事件
        public static void GetMySelfStringVar(ref string Text)
        {
            string S;
            string sVarText;
            int nPos;
            int nPos2;
            string ShowText;
            if (Text == "")
            {
                return;
            }
            S = Text;
            nPos = S.IndexOf("<");
            if (nPos > 0)
            {
                ShowText = S.Substring(1 - 1, nPos - 1);
                nPos2 = S.IndexOf(">");
                sVarText = S.Substring(nPos + 1 - 1, nPos2 - nPos - 1);
                ShowText = ShowText + GetMySelfStringVar_GetVarTextFieldFunc(sVarText);
                S = S.Substring(nPos2 + 1 - 1, 255);
                while (true)
                {
                    nPos = S.IndexOf("<");
                    nPos2 = S.IndexOf(">");
                    ShowText = ShowText + S.Substring(1 - 1, nPos - 1);
                    if (nPos + nPos2 > 0)
                    {
                        sVarText = S.Substring(nPos + 1 - 1, nPos2 - nPos - 1);
                        ShowText = ShowText + GetMySelfStringVar_GetVarTextFieldFunc(sVarText);
                        S = S.Substring(nPos2 + 1 - 1, 255);
                    }
                    else
                    {
                        break;
                    }
                }
                Text = ShowText + S;
            }
        }

        // 得到地图文件名称自定义路径
        public static string GetMapDirAndName(string sFileName)
        {
            string result;
            if (File.Exists(WMFile.Units.WMFile.MAPDIRNAME + sFileName + ".map"))
            {
                result = WMFile.Units.WMFile.MAPDIRNAME + sFileName + ".map";
            }
            else
            {
                result = WMFile.Units.WMFile.OLDMAPDIRNAME + sFileName + ".map";
            }
            return result;
        }

        public static void ShowMsg(string Str)
        {
            ClMain.DScreen.AddChatBoardString(Str, System.Drawing.Color.White, System.Drawing.Color.Black);
        }

        // 123456
        public static void LoadMapDesc()
        {
            int i;
            string szFileName;
            string szLine;
            ArrayList xsl;
            string szMapTitle;
            string szPointX;
            string szPointY;
            string szPlaceName;
            string szColor;
            string szFullMap;
            int nPointX;
            int nPointY;
            int nFullMap;
            Color nColor;
            TMapDescInfo pMapDescInfo;
            szFileName = ".\\data\\MapDesc2.dat";
            if (File.Exists(szFileName))
            {
                xsl = new ArrayList();
                xsl.LoadFromFile(szFileName);
                for (i = 0; i < xsl.Count; i++)
                {
                    szLine = xsl[i];
                    if ((szLine == "") || (szLine[1] == ";"))
                    {
                        continue;
                    }
                    szLine = HGEGUI.Units.HGEGUI.GetValidStr3(szLine, ref szMapTitle, new string[] { "," });
                    szLine = HGEGUI.Units.HGEGUI.GetValidStr3(szLine, ref szPointX, new string[] { "," });
                    szLine = HGEGUI.Units.HGEGUI.GetValidStr3(szLine, ref szPointY, new string[] { "," });
                    szLine = HGEGUI.Units.HGEGUI.GetValidStr3(szLine, ref szPlaceName, new string[] { "," });
                    szLine = HGEGUI.Units.HGEGUI.GetValidStr3(szLine, ref szColor, new string[] { "," });
                    szLine = HGEGUI.Units.HGEGUI.GetValidStr3(szLine, ref szFullMap, new string[] { "," });
                    nPointX = HUtil32.Str_ToInt(szPointX, -1);
                    nPointY = HUtil32.Str_ToInt(szPointY, -1);
                    nColor = Convert.ToInt32(szColor);
                    nFullMap = HUtil32.Str_ToInt(szFullMap, -1);
                    if ((szPlaceName != "") && (szMapTitle != "") && (nPointX >= 0) && (nPointY >= 0) && (nFullMap >= 0))
                    {
                        pMapDescInfo = new TMapDescInfo();
                        pMapDescInfo.szMapTitle = szMapTitle;
                        pMapDescInfo.szPlaceName = szPlaceName;
                        pMapDescInfo.nPointX = nPointX;
                        pMapDescInfo.nPointY = nPointY;
                        pMapDescInfo.nColor = nColor;
                        pMapDescInfo.nFullMap = nFullMap;
                        // DebugOutStr(string.Format('%.8x', [pMapDescInfo.nColor]));
                        g_xMapDescList.Add(szMapTitle, ((pMapDescInfo) as Object));
                    }
                }
                xsl.Free;
            }
        }

        public static int GetTickCount()
        {
            return SystemModule.HUtil32.GetTickCount(); ;
        }

        // stdcall;
        public static bool IsDetectItem(int idx)
        {
            bool result;
            result = idx == DETECT_MIIDX_OFFSET;
            return result;
        }

        public static bool IsBagItem(int idx)
        {
            bool result;
            result = idx >= 6 && idx <= Grobal2.MAXBAGITEM - 1;
            return result;
        }

        public static bool IsEquItem(int idx)
        {
            bool result;
            int sel;
            result = false;
            if (idx < 0)
            {
                sel = -(idx + 1);
                result = sel >= 0 && sel <= Grobal2.U_FASHION;
            }
            return result;
        }

        public static bool IsStorageItem(int idx)
        {
            bool result;
            result = (idx >= SAVE_MIIDX_OFFSET) && (idx < SAVE_MIIDX_OFFSET + 46);
            return result;
        }

        public static bool IsStallItem(int idx)
        {
            bool result;
            result = (idx >= STALL_MIIDX_OFFSET) && (idx < STALL_MIIDX_OFFSET + 10);
            return result;
        }

        public static void ResetSeriesSkillVar()
        {
            g_nCurrentMagic = 888;
            g_nCurrentMagic2 = 888;
            g_SeriesSkillStep = 0;
            g_SeriesSkillFire = false;
            g_SeriesSkillFire_100 = false;
            g_SeriesSkillReady = false;
            g_NextSeriesSkill = false;
            //FillChar(g_VenationInfos);            //FillChar(g_TempSeriesSkillArr);            //FillChar(g_HTempSeriesSkillArr);            //FillChar(g_SeriesSkillArr);        }
        }

        public static int GetSeriesSkillIcon(int id)
        {
            int result;
            result = -1;
            switch (id)
            {
                case 100:
                    result = 950;
                    break;
                case 101:
                    result = 952;
                    break;
                case 102:
                    result = 956;
                    break;
                case 103:
                    result = 954;
                    break;
                case 104:
                    result = 942;
                    break;
                case 105:
                    result = 946;
                    break;
                case 106:
                    result = 940;
                    break;
                case 107:
                    result = 944;
                    break;
                case 108:
                    result = 934;
                    break;
                case 109:
                    result = 936;
                    break;
                case 110:
                    result = 932;
                    break;
                case 111:
                    result = 930;
                    break;
                case 112:
                    result = 944;
                    break;
            }
            return result;
        }

        public static void CheckSpeedCount(int Count)
        {
            g_dwCheckCount++;
            if (g_dwCheckCount > Count)
            {
                g_dwCheckCount = 0;
                // g_ModuleDetect.FCheckTick := 0;
            }
        }

        // procedure SaveUserConfig(sUserName: string);
        public static bool IsPersentHP(int nMin, int nMax)
        {
            bool result;
            result = false;
            if (nMax != 0)
            {
                // or (nMax - nMin > 1500)
                result = (Math.Round((nMin / nMax) * 100) < g_gnProtectPercent[0]);
            }
            return result;
        }

        public static bool IsPersentMP(int nMin, int nMax)
        {
            bool result;
            result = false;
            if (nMax != 0)
            {
                // or (nMax - nMin > 1500)
                result = (Math.Round((nMin / nMax) * 100) < g_gnProtectPercent[1]);
            }
            return result;
        }

        public static bool IsPersentSpc(int nMin, int nMax)
        {
            bool result;
            result = false;
            if (nMax != 0)
            {
                // or (nMax - nMin > 6000)
                result = (Math.Round((nMin / nMax) * 100) < g_gnProtectPercent[3]);
            }
            return result;
        }

        public static bool IsPersentBook(int nMin, int nMax)
        {
            bool result;
            result = false;
            if (nMax != 0)
            {
                // or (nMax - nMin > 6000)
                result = (Math.Round((nMin / nMax) * 100) < g_gnProtectPercent[5]);
            }
            return result;
        }

        public static bool IsPersentHPHero(int nMin, int nMax)
        {
            bool result;
            result = false;
            if (nMax != 0)
            {
                // or (nMax - nMin > 1500)
                result = (Math.Round((nMin / nMax) * 100) < g_gnProtectPercent[7]);
            }
            return result;
        }

        public static bool IsPersentMPHero(int nMin, int nMax)
        {
            bool result;
            result = false;
            if (nMax != 0)
            {
                // or (nMax - nMin > 1500)
                result = (Math.Round((nMin / nMax) * 100) < g_gnProtectPercent[8]);
            }
            return result;
        }

        public static bool IsPersentSpcHero(int nMin, int nMax)
        {
            bool result;
            result = false;
            if (nMax != 0)
            {
                // or (nMax - nMin > 6000)
                result = (Math.Round((nMin / nMax) * 100) < g_gnProtectPercent[9]);
            }
            return result;
        }

        // 取得职业名称
        // 0 武士
        // 1 魔法师
        // 2 道士
        public static string GetJobName(int nJob)
        {
            string result;
            result = "";
            switch (nJob)
            {
                case 0:
                    result = g_sWarriorName;
                    break;
                case 1:
                    result = g_sWizardName;
                    break;
                case 2:
                    result = g_sTaoistName;
                    break;
                default:
                    result = g_sUnKnowName;
                    break;
            }
            return result;
        }

        // procedure ClearShowItemList();
        public static string GetItemType(TItemType ItemType)
        {
            string result;
            switch (ItemType)
            {
                case TItemType.i_HPDurg:
                    result = "金创药";
                    break;
                case TItemType.i_MPDurg:
                    result = "魔法药";
                    break;
                case TItemType.i_HPMPDurg:
                    result = "高级药";
                    break;
                case TItemType.i_OtherDurg:
                    result = "其它药品";
                    break;
                case TItemType.i_Weapon:
                    result = "武器";
                    break;
                case TItemType.i_Dress:
                    result = "衣服";
                    break;
                case TItemType.i_Helmet:
                    result = "头盔";
                    break;
                case TItemType.i_Necklace:
                    result = "项链";
                    break;
                case TItemType.i_Armring:
                    result = "手镯";
                    break;
                case TItemType.i_Ring:
                    result = "戒指";
                    break;
                case TItemType.i_Belt:
                    result = "腰带";
                    break;
                case TItemType.i_Boots:
                    result = "鞋子";
                    break;
                case TItemType.i_Charm:
                    result = "宝石";
                    break;
                case TItemType.i_Book:
                    result = "技能书";
                    break;
                case TItemType.i_PosionDurg:
                    result = "毒药";
                    break;
                case TItemType.i_UseItem:
                    result = "消耗品";
                    break;
                case TItemType.i_Scroll:
                    result = "卷类";
                    break;
                case TItemType.i_Stone:
                    result = "矿石";
                    break;
                case TItemType.i_Gold:
                    result = "金币";
                    break;
                case TItemType.i_Other:
                    result = "其它";
                    break;
            }
            return result;
        }

        public static bool GetItemShowFilter(string sItemName)
        {
            bool result;
            result = g_ShowItemList.IndexOf(sItemName) > -1;
            return result;
        }

        public static void LoadUserConfig(string sUserName)
        {
            //FileStream ini;
            //FileStream ini2;
            //string sFileName;
            //ArrayList Strings;
            //int i;
            //int no;
            //string sn;
            //string so;
            //sFileName = ".\\Config\\" + g_sServerName + "." + sUserName + ".Set";
            //ini = new FileStream(sFileName);
            //// base
            //g_gcGeneral[0] = ini.ReadBool("Basic", "ShowActorName", g_gcGeneral[0]);
            //g_gcGeneral[1] = ini.ReadBool("Basic", "DuraWarning", g_gcGeneral[1]);
            //g_gcGeneral[2] = ini.ReadBool("Basic", "AutoAttack", g_gcGeneral[2]);
            //g_gcGeneral[3] = ini.ReadBool("Basic", "ShowExpFilter", g_gcGeneral[3]);
            //g_MaxExpFilter = ini.ReadInteger("Basic", "ShowExpFilterMax", g_MaxExpFilter);
            //g_gcGeneral[4] = ini.ReadBool("Basic", "ShowDropItems", g_gcGeneral[4]);
            //g_gcGeneral[5] = ini.ReadBool("Basic", "ShowDropItemsFilter", g_gcGeneral[5]);
            //g_gcGeneral[6] = ini.ReadBool("Basic", "ShowHumanWing", g_gcGeneral[6]);
            //g_boAutoPickUp = ini.ReadBool("Basic", "AutoPickUp", g_boAutoPickUp);
            //g_gcGeneral[7] = ini.ReadBool("Basic", "AutoPickUpFilter", g_gcGeneral[7]);
            //g_boPickUpAll = ini.ReadBool("Basic", "PickUpAllItem", g_boPickUpAll);
            //g_gcGeneral[8] = ini.ReadBool("Basic", "HideDeathBody", g_gcGeneral[8]);
            //g_gcGeneral[9] = ini.ReadBool("Basic", "AutoFixItem", g_gcGeneral[9]);
            //g_gcGeneral[10] = ini.ReadBool("Basic", "ShakeScreen", g_gcGeneral[10]);
            //g_gcGeneral[13] = ini.ReadBool("Basic", "StruckShow", g_gcGeneral[13]);
            //g_gcGeneral[15] = ini.ReadBool("Basic", "HideStruck", g_gcGeneral[15]);
            //g_gcGeneral[14] = ini.ReadBool("Basic", "CompareItems", g_gcGeneral[14]);
            //ini2 = new FileStream(".\\lscfg.ini");
            //g_gcGeneral[11] = ini2.ReadBool("Setup", "EffectSound", g_gcGeneral[11]);
            //g_gcGeneral[12] = ini2.ReadBool("Setup", "EffectBKGSound", g_gcGeneral[12]);
            //g_lWavMaxVol = ini2.ReadInteger("Setup", "EffectSoundLevel", g_lWavMaxVol);
            //ini2.Free;
            //g_HitSpeedRate = ini.ReadInteger("Basic", "HitSpeedRate", g_HitSpeedRate);
            //g_MagSpeedRate = ini.ReadInteger("Basic", "MagSpeedRate", g_MagSpeedRate);
            //g_MoveSpeedRate = ini.ReadInteger("Basic", "MoveSpeedRate", g_MoveSpeedRate);
            //// Protect
            //g_gcProtect[0] = ini.ReadBool("Protect", "RenewHPIsAuto", g_gcProtect[0]);
            //g_gcProtect[1] = ini.ReadBool("Protect", "RenewMPIsAuto", g_gcProtect[1]);
            //g_gcProtect[3] = ini.ReadBool("Protect", "RenewSpecialIsAuto", g_gcProtect[3]);
            //g_gcProtect[5] = ini.ReadBool("Protect", "RenewBookIsAuto", g_gcProtect[5]);
            //g_gcProtect[7] = ini.ReadBool("Protect", "HeroRenewHPIsAuto", g_gcProtect[7]);
            //g_gcProtect[8] = ini.ReadBool("Protect", "HeroRenewMPIsAuto", g_gcProtect[8]);
            //g_gcProtect[9] = ini.ReadBool("Protect", "HeroRenewSpecialIsAuto", g_gcProtect[9]);
            //g_gcProtect[10] = ini.ReadBool("Protect", "HeroSidestep", g_gcProtect[10]);
            //g_gcProtect[11] = ini.ReadBool("Protect", "RenewSpecialIsAuto_MP", g_gcProtect[11]);
            //g_gnProtectTime[0] = ini.ReadInteger("Protect", "RenewHPTime", g_gnProtectTime[0]);
            //g_gnProtectTime[1] = ini.ReadInteger("Protect", "RenewMPTime", g_gnProtectTime[1]);
            //g_gnProtectTime[3] = ini.ReadInteger("Protect", "RenewSpecialTime", g_gnProtectTime[3]);
            //g_gnProtectTime[5] = ini.ReadInteger("Protect", "RenewBookTime", g_gnProtectTime[5]);
            //g_gnProtectTime[7] = ini.ReadInteger("Protect", "HeroRenewHPTime", g_gnProtectTime[7]);
            //g_gnProtectTime[8] = ini.ReadInteger("Protect", "HeroRenewMPTime", g_gnProtectTime[8]);
            //g_gnProtectTime[9] = ini.ReadInteger("Protect", "HeroRenewSpecialTime", g_gnProtectTime[9]);
            //g_gnProtectPercent[0] = ini.ReadInteger("Protect", "RenewHPPercent", g_gnProtectPercent[0]);
            //g_gnProtectPercent[1] = ini.ReadInteger("Protect", "RenewMPPercent", g_gnProtectPercent[1]);
            //g_gnProtectPercent[3] = ini.ReadInteger("Protect", "RenewSpecialPercent", g_gnProtectPercent[3]);
            //g_gnProtectPercent[7] = ini.ReadInteger("Protect", "HeroRenewHPPercent", g_gnProtectPercent[7]);
            //g_gnProtectPercent[8] = ini.ReadInteger("Protect", "HeroRenewMPPercent", g_gnProtectPercent[8]);
            //g_gnProtectPercent[9] = ini.ReadInteger("Protect", "HeroRenewSpecialPercent", g_gnProtectPercent[9]);
            //g_gnProtectPercent[10] = ini.ReadInteger("Protect", "HeroPerSidestep", g_gnProtectPercent[10]);
            //g_gnProtectPercent[5] = ini.ReadInteger("Protect", "RenewBookPercent", g_gnProtectPercent[5]);
            //g_gnProtectPercent[6] = ini.ReadInteger("Protect", "RenewBookNowBookIndex", g_gnProtectPercent[6]);
            //ClMain.frmMain.SendClientMessage(Grobal2.CM_HEROSIDESTEP, MakeLong(((int)g_gcProtect[10]), g_gnProtectPercent[10]), 0, 0, 0);
            //g_gcTec[0] = ini.ReadBool("Tec", "SmartLongHit", g_gcTec[0]);
            //g_gcTec[10] = ini.ReadBool("Tec", "SmartLongHit2", g_gcTec[10]);
            //g_gcTec[11] = ini.ReadBool("Tec", "SmartSLongHit", g_gcTec[11]);
            //g_gcTec[1] = ini.ReadBool("Tec", "SmartWideHit", g_gcTec[1]);
            //g_gcTec[2] = ini.ReadBool("Tec", "SmartFireHit", g_gcTec[2]);
            //g_gcTec[3] = ini.ReadBool("Tec", "SmartPureHit", g_gcTec[3]);
            //g_gcTec[4] = ini.ReadBool("Tec", "SmartShield", g_gcTec[4]);
            //g_gcTec[5] = ini.ReadBool("Tec", "SmartShieldHero", g_gcTec[5]);
            //g_gcTec[6] = ini.ReadBool("Tec", "SmartTransparence", g_gcTec[6]);
            //g_gcTec[9] = ini.ReadBool("Tec", "SmartThunderHit", g_gcTec[9]);
            //g_gcTec[7] = ini.ReadBool("AutoPractice", "PracticeIsAuto", g_gcTec[7]);
            //g_gnTecTime[8] = ini.ReadInteger("AutoPractice", "PracticeTime", g_gnTecTime[8]);
            //g_gnTecPracticeKey = ini.ReadInteger("AutoPractice", "PracticeKey", g_gnTecPracticeKey);
            //g_gcTec[12] = ini.ReadBool("Tec", "HeroSeriesSkillFilter", g_gcTec[12]);
            //g_gcTec[13] = ini.ReadBool("Tec", "SLongHit", g_gcTec[13]);
            //g_gcTec[14] = ini.ReadBool("Tec", "SmartGoMagic", g_gcTec[14]);
            //ClMain.frmMain.SendClientMessage(Grobal2.CM_HEROSERIESSKILLCONFIG, MakeLong(((int)g_gcTec[12]), 0), 0, 0, 0);
            //g_gcHotkey[0] = ini.ReadBool("Hotkey", "UseHotkey", g_gcHotkey[0]);
            //FrmDlg.DEHeroCallHero.SetOfHotKey(ini.ReadInteger("Hotkey", "HeroCallHero", 0));
            //FrmDlg.DEHeroSetAttackState.SetOfHotKey(ini.ReadInteger("Hotkey", "HeroSetAttackState", 0));
            //FrmDlg.DEHeroSetGuard.SetOfHotKey(ini.ReadInteger("Hotkey", "HeroSetGuard", 0));
            //FrmDlg.DEHeroSetTarget.SetOfHotKey(ini.ReadInteger("Hotkey", "HeroSetTarget", 0));
            //FrmDlg.DEHeroUnionHit.SetOfHotKey(ini.ReadInteger("Hotkey", "HeroUnionHit", 0));
            //FrmDlg.DESwitchAttackMode.SetOfHotKey(ini.ReadInteger("Hotkey", "SwitchAttackMode", 0));
            //FrmDlg.DESwitchMiniMap.SetOfHotKey(ini.ReadInteger("Hotkey", "SwitchMiniMap", 0));
            //FrmDlg.DxEditSSkill.SetOfHotKey(ini.ReadInteger("Hotkey", "SerieSkill", 0));
            //g_ShowItemList.LoadFromFile(".\\Data\\Filter.dat");
            //// ============================================================================
            //// g_gcAss[0] := ini.ReadBool('Ass', '0', g_gcAss[0]);
            //g_gcAss[1] = ini.ReadBool("Ass", "1", g_gcAss[1]);
            //g_gcAss[2] = ini.ReadBool("Ass", "2", g_gcAss[2]);
            //g_gcAss[3] = ini.ReadBool("Ass", "3", g_gcAss[3]);
            //g_gcAss[4] = ini.ReadBool("Ass", "4", g_gcAss[4]);
            //g_gcAss[5] = ini.ReadBool("Ass", "5", g_gcAss[5]);
            //g_gcAss[6] = ini.ReadBool("Ass", "6", g_gcAss[6]);
            //g_APPickUpList.Clear();
            //g_APMobList.Clear();
            //Strings = new ArrayList();
            //if (g_gcAss[5])
            //{
            //    sFileName = ".\\Config\\" + g_sServerName + "." + sUserName + ".ItemFilterEx.txt";
            //    if (File.Exists(sFileName))
            //    {
            //        Strings.LoadFromFile(sFileName);
            //    }
            //    else
            //    {
            //        Strings.SaveToFile(sFileName);
            //    }
            //    for (i = 0; i < Strings.Count; i++)
            //    {
            //        if ((Strings[i] == "") || (Strings[i][1] == ";"))
            //        {
            //            continue;
            //        }
            //        so = HGEGUI.Units.HGEGUI.GetValidStr3(Strings[i], ref sn, new string[] { ",", " ", "\09" });
            //        no = ((int)so != "");
            //        g_APPickUpList.Add(sn, ((no) as Object));
            //    }
            //}
            //if (g_gcAss[6])
            //{
            //    sFileName = ".\\Config\\" + g_sServerName + "." + sUserName + ".MonFilter.txt";
            //    if (File.Exists(sFileName))
            //    {
            //        Strings.LoadFromFile(sFileName);
            //    }
            //    else
            //    {
            //        Strings.SaveToFile(sFileName);
            //    }
            //    for (i = 0; i < Strings.Count; i++)
            //    {
            //        if ((Strings[i] == "") || (Strings[i][1] == ";"))
            //        {
            //            continue;
            //        }
            //        // , nil
            //        g_APMobList.Add(Strings[i]);
            //    }
            //}
        }

        public static void LoadItemDesc()
        {
            const string fItemDesc = ".\\data\\ItemDesc.dat";
            int i;
            string Name;
            string desc;
            string ps;
            ArrayList temp;
            // g_ItemDesc
            if (File.Exists(fItemDesc))
            {
                temp = new ArrayList();
                temp.LoadFromFile(fItemDesc);
                for (i = 0; i < temp.Count; i++)
                {
                    if (temp[i] == "")
                    {
                        continue;
                    }
                    desc = HGEGUI.Units.HGEGUI.GetValidStr3(temp[i], ref Name, new string[] { "=" });
                    desc = desc.Replace("\\", "");
                    ps = new string();
                    ps = desc;
                    if ((Name != "") && (desc != ""))
                    {
                        // g_ItemDesc.Put(name, TObject(ps));
                        g_ItemDesc.Add(Name, ((ps) as Object));
                    }
                }
                temp.Free;
            }
        }

        public static int GetLevelColor(byte iLevel)
        {
            int result;
            switch (iLevel)
            {
                case 0:
                    result = 0x00FFFFFF;
                    break;
                case 1:
                    result = 0x004AD663;
                    break;
                case 2:
                    result = 0x00E9A000;
                    break;
                case 3:
                    result = 0x00FF35B1;
                    break;
                case 4:
                    result = 0x000061EB;
                    break;
                case 5:
                    result = 0x005CF4FF;
                    break;
                case 15:
                    result = Color.Gray.ToArgb();
                    break;
                default:
                    result = 0x005CF4FF;
                    break;
            }
            return result;
        }

        public static void LoadItemFilter()
        {
            int i;
            int n;
            string s;
            string s0;
            string s1;
            string s2;
            string s3;
            string s4;
            string fn;
            ArrayList ls;
            TCItemRule p;
            TCItemRule p2;
            fn = ".\\Data\\lsDefaultItemFilter.txt";
            if (File.Exists(fn))
            {
                ls = new ArrayList();
                ls.LoadFromFile(fn);
                for (i = 0; i < ls.Count; i++)
                {
                    s = ls[i];
                    if (s == "")
                    {
                        continue;
                    }
                    s = HGEGUI.Units.HGEGUI.GetValidStr3(s, ref s0, new string[] { "," });
                    s = HGEGUI.Units.HGEGUI.GetValidStr3(s, ref s1, new string[] { "," });
                    s = HGEGUI.Units.HGEGUI.GetValidStr3(s, ref s2, new string[] { "," });
                    s = HGEGUI.Units.HGEGUI.GetValidStr3(s, ref s3, new string[] { "," });
                    s = HGEGUI.Units.HGEGUI.GetValidStr3(s, ref s4, new string[] { "," });
                    p = new TCItemRule();
                    p.Name = s0;
                    p.rare = s2 == "1";
                    p.pick = s3 == "1";
                    p.Show = s4 == "1";
                    g_ItemsFilter_All.Put(s0, ((p) as Object));
                    p2 = new TCItemRule();
                    p2 = p;
                    g_ItemsFilter_All_Def.Put(s0, ((p2) as Object));
                    n = Convert.ToInt32(s1);
                    switch (n)
                    {
                        case 0:
                            g_ItemsFilter_Dress.Add(s0, ((p) as Object));
                            break;
                        case 1:
                            g_ItemsFilter_Weapon.Add(s0, ((p) as Object));
                            break;
                        case 2:
                            g_ItemsFilter_Headgear.Add(s0, ((p) as Object));
                            break;
                        case 3:
                            g_ItemsFilter_Drug.Add(s0, ((p) as Object));
                            break;
                        default:
                            g_ItemsFilter_Other.Add(s0, ((p) as Object));
                            break;
                            // 服装
                    }
                }
                ls.Free;
            }
        }

        public static void LoadItemFilter2()
        {
            int i;
            string s;
            string s0;
            string s2;
            string s3;
            string s4;
            string fn;
            ArrayList ls;
            TCItemRule p;
            TCItemRule p2;
            bool b2;
            bool b3;
            bool b4;
            fn = ".\\Config\\" + g_sServerName + "." + ClMain.frmMain.m_sCharName + ".ItemFilter.txt";
            // DScreen.AddChatBoardString(fn, clWhite, clBlue);
            if (File.Exists(fn))
            {
                // DScreen.AddChatBoardString('1', clWhite, clBlue);
                ls = new ArrayList();
                ls.LoadFromFile(fn);
                for (i = 0; i < ls.Count; i++)
                {
                    s = ls[i];
                    if (s == "")
                    {
                        continue;
                    }
                    s = HGEGUI.Units.HGEGUI.GetValidStr3(s, ref s0, new string[] { "," });
                    s = HGEGUI.Units.HGEGUI.GetValidStr3(s, ref s2, new string[] { "," });
                    s = HGEGUI.Units.HGEGUI.GetValidStr3(s, ref s3, new string[] { "," });
                    s = HGEGUI.Units.HGEGUI.GetValidStr3(s, ref s4, new string[] { "," });
                    p = ((TCItemRule)(g_ItemsFilter_All_Def.GetValues(s0)));
                    if (p != null)
                    {
                        // DScreen.AddChatBoardString('2', clWhite, clBlue);
                        b2 = s2 == "1";
                        b3 = s3 == "1";
                        b4 = s4 == "1";
                        if ((b2 != p.rare) || (b3 != p.pick) || (b4 != p.Show))
                        {
                            // DScreen.AddChatBoardString('3', clWhite, clBlue);
                            p2 = ((TCItemRule)(g_ItemsFilter_All.GetValues(s0)));
                            if (p2 != null)
                            {
                                // DScreen.AddChatBoardString('4', clWhite, clBlue);
                                p2.rare = b2;
                                p2.pick = b3;
                                p2.Show = b4;
                            }
                        }
                    }
                }
                ls.Free;
            }
        }

        public static void SaveItemFilter()
        {
            // 退出增量保存
            int i;
            ArrayList ls;
            TCItemRule p;
            TCItemRule p2;
            string fn;
            fn = ".\\Config\\" + g_sServerName + "." + ClMain.frmMain.m_sCharName + ".ItemFilter.txt";
            ls = new ArrayList();
            for (i = 0; i < g_ItemsFilter_All.Count; i++)
            {
                p = ((TCItemRule)(g_ItemsFilter_All.GetValues(g_ItemsFilter_All.Keys[i])));
                p2 = ((TCItemRule)(g_ItemsFilter_All_Def.GetValues(g_ItemsFilter_All_Def.Keys[i])));
                if (p.Name == p2.Name)
                {
                    if ((p.rare != p2.rare) || (p.pick != p2.pick) || (p.Show != p2.Show))
                    {
                        ls.Add(string.Format("%s,%d,%d,%d", new byte[] { p.Name, ((byte)p.rare), ((byte)p.pick), ((byte)p.Show) }));
                    }
                }
            }
            if (ls.Count > 0)
            {
                ls.SaveToFile(fn);
            }
            ls.Free;
        }

        public static TClientSuiteItems getSuiteHint(ref int idx, string s, byte gender)
        {
            TClientSuiteItems result;
            int i;
            TClientSuiteItems p;
            result = null;
            if ((idx > 12) || (idx < 0))
            {
                return result;
            }
            for (i = 0; i < g_SuiteItemsList.Count; i++)
            {
                p = g_SuiteItemsList[i];
                if (((p.asSuiteName[0] == "") || (gender == p.Gender)) && ((s).ToLower().CompareTo((p.asSuiteName[idx]).ToLower()) == 0))
                {
                    result = p;
                    break;
                }
            }
            idx = -1;
            return result;
        }

        public static int GetItemWhere(TClientItem clientItem)
        {
            int result;
            result = -1;
            if (clientItem.Item.Name == "")
            {
                return result;
            }
            switch (clientItem.Item.StdMode)
            {
                case 10:
                case 11:
                    result = Grobal2.U_DRESS;
                    break;
                case 5:
                case 6:
                    result = Grobal2.U_WEAPON;
                    break;
                case 30:
                    result = Grobal2.U_RIGHTHAND;
                    break;
                case 19:
                case 20:
                case 21:
                    result = Grobal2.U_NECKLACE;
                    break;
                case 15:
                    result = Grobal2.U_HELMET;
                    break;
                case 16:
                    break;
                case 24:
                case 26:
                    result = Grobal2.U_ARMRINGL;
                    break;
                case 22:
                case 23:
                    result = Grobal2.U_RINGL;
                    break;
                case 25:
                    result = Grobal2.U_BUJUK;
                    break;
                case 27:
                    result = Grobal2.U_BELT;
                    break;
                case 28:
                    result = Grobal2.U_BOOTS;
                    break;
                case 7:
                case 29:
                    result = Grobal2.U_CHARM;
                    break;
            }
            return result;
        }

        public static bool GetSecretAbil(TClientItem CurrMouseItem)
        {
            bool result;
            int i;
            int start;
            byte adv;
            byte cnt;
            string s;
            result = false;
            if (!(new ArrayList(new int[] { 5, 6, 10, 15, 26 }).Contains(CurrMouseItem.s.StdMode)))
            {
                return result;
            }
            return result;
        }

        public static void InitClientItems()
        {
            //FillChar(g_MagicArr);            
            //FillChar(g_TakeBackItemWait);           
            //FillChar(g_UseItems);           
            //FillChar(g_ItemArr);           
            //FillChar(g_HeroUseItems);        
            //FillChar(g_HeroItemArr);     
            //FillChar(g_RefineItems);     
            //FillChar(g_BuildAcuses);     
            //FillChar(g_DetectItem);    
            //FillChar(g_TIItems);        
            //FillChar(g_spItems);     
            //FillChar(g_ItemArr);        
            //FillChar(g_HeroItemArr);  
            //FillChar(g_DealItems);       
            //FillChar(g_YbDealItems);         
            //FillChar(g_DealRemoteItems);
        }

        public static byte GetTIHintString1(int idx, TClientItem ci, string iname)
        {
            byte result;
            result = 0;
            g_tiHintStr1 = "";
            //switch (idx)
            //{
            //    case 0:
            //        g_tiHintStr1 = "我收藏天下的奇珍异宝，走南闯北几十年了，各种神器见过不少，把你要鉴定的装备放在桌子上吧！";
            //        FrmDlg.DBTIbtn1.btnState = tdisable;
            //        FrmDlg.DBTIbtn1.Caption = "普通鉴定";
            //        FrmDlg.DBTIbtn2.btnState = tdisable;
            //        FrmDlg.DBTIbtn2.Caption = "高级鉴定";
            //        break;
            //    case 1:
            //        if ((ci == null) || (ci.s.Name == ""))
            //        {
            //            return result;
            //        }
            //        if (ci.Item.Eva.EvaTimesMax == 0)
            //        {
            //            g_tiHintStr1 = "标志了不可鉴定的物品我是鉴定不了的，你换一个吧。";
            //            FrmDlg.DBTIbtn1.btnState = tdisable;
            //            FrmDlg.DBTIbtn1.Caption = "普通鉴定";
            //            FrmDlg.DBTIbtn2.btnState = tdisable;
            //            FrmDlg.DBTIbtn2.Caption = "高级鉴定";
            //            return result;
            //        }
            //        if (ci.Item.Eva.EvaTimes < ci.Item.Eva.EvaTimesMax)
            //        {
            //            if (FrmDlg.DWTI.tag == 1)
            //            {
            //                switch (ci.Item.Eva.EvaTimes)
            //                {
            //                    case 0:
            //                        g_tiHintStr1 = "第一次鉴定我需要一个一级鉴定卷轴，你快去收集一个吧！";
            //                        FrmDlg.DBTIbtn1.btnState = tnor;
            //                        FrmDlg.DBTIbtn1.Caption = "普通一鉴";
            //                        FrmDlg.DBTIbtn2.btnState = tnor;
            //                        FrmDlg.DBTIbtn2.Caption = "高级一鉴";
            //                        break;
            //                    case 1:
            //                        g_tiHintStr1 = "第二次鉴定我需要一个二级鉴定卷轴，你快去收集一个吧！";
            //                        FrmDlg.DBTIbtn1.btnState = tnor;
            //                        FrmDlg.DBTIbtn1.Caption = "普通二鉴";
            //                        FrmDlg.DBTIbtn2.btnState = tnor;
            //                        FrmDlg.DBTIbtn2.Caption = "高级二鉴";
            //                        break;
            //                    case 2:
            //                        g_tiHintStr1 = "第三次鉴定我需要一个三级鉴定卷轴，你快去收集一个吧！";
            //                        FrmDlg.DBTIbtn1.btnState = tnor;
            //                        FrmDlg.DBTIbtn1.Caption = "普通三鉴";
            //                        FrmDlg.DBTIbtn2.btnState = tnor;
            //                        FrmDlg.DBTIbtn2.Caption = "高级三鉴";
            //                        break;
            //                    default:
            //                        g_tiHintStr1 = "我需要一个三级鉴定卷轴来鉴定你这个装备。";
            //                        FrmDlg.DBTIbtn1.btnState = tnor;
            //                        FrmDlg.DBTIbtn1.Caption = "普通三鉴";
            //                        FrmDlg.DBTIbtn2.btnState = tnor;
            //                        FrmDlg.DBTIbtn2.Caption = "高级三鉴";
            //                        break;
            //                }
            //            }
            //            else if (FrmDlg.DWTI.tag == 2)
            //            {
            //                FrmDlg.DBTIbtn1.btnState = tnor;
            //                FrmDlg.DBTIbtn1.Caption = "更换";
            //            }
            //            result = ci.Item.Eva.EvaTimes;
            //        }
            //        else
            //        {
            //            g_tiHintStr1 = string.Format("你的这件%s已经不能再鉴定了。", new string[] { ci.s.Name });
            //            FrmDlg.DBTIbtn1.btnState = tdisable;
            //            FrmDlg.DBTIbtn1.Caption = "普通鉴定";
            //            FrmDlg.DBTIbtn2.btnState = tdisable;
            //            FrmDlg.DBTIbtn2.Caption = "高级鉴定";
            //        }
            //        break;
            //    case 2:
            //        g_tiHintStr1 = string.Format("借助卷轴的力量，我已经帮你发现了你这%s的潜能。", new string[] { iname });
            //        break;
            //    case 3:
            //        g_tiHintStr1 = string.Format("借助卷轴的力量，我已经帮你发现了你这%s的神秘潜能。", new string[] { iname });
            //        break;
            //    case 4:
            //        g_tiHintStr1 = string.Format("这%s虽然没能发现更大的潜能，但是他拥有感应其他宝物存在的特殊能力。", new string[] { iname });
            //        break;
            //    case 5:
            //        g_tiHintStr1 = string.Format("我并没能从你的这个%s上发现更多的潜能。你不要沮丧，我会给你额外的补偿。", new string[] { iname });
            //        break;
            //    case 6:
            //        g_tiHintStr1 = string.Format("我并没能从你的这个%s上发现更多的潜能。", new string[] { iname });
            //        break;
            //    case 7:
            //        g_tiHintStr1 = string.Format("我并没能从你的这个%s上发现更多的潜能。你的宝物已经不可鉴定。", new string[] { iname });
            //        break;
            //    case 8:
            //        g_tiHintStr1 = "你缺少宝物或者卷轴。";
            //        break;
            //    case 9:
            //        g_tiHintStr1 = string.Format("恭喜你的宝物被鉴定为主宰装备，你获得了%s。", new string[] { iname });
            //        break;
            //    case 10:
            //        g_tiHintStr1 = "待鉴物品错误或不存在！";
            //        break;
            //    case 11:
            //        g_tiHintStr1 = string.Format("你的这件%s不可以鉴定！", new string[] { iname });
            //        break;
            //    case 12:
            //        FrmDlg.DBTIbtn1.btnState = tdisable;
            //        FrmDlg.DBTIbtn2.btnState = tdisable;
            //        g_tiHintStr1 = string.Format("以我目前的能力，%s只能先鉴定到这里了。", new string[] { iname });
            //        break;
            //    case 30:
            //        g_tiHintStr1 = "鉴定卷轴错误或不存在！";
            //        break;
            //    case 31:
            //        g_tiHintStr1 = string.Format("我需要一个%s级鉴定卷轴，你的卷轴不符合要求！", new string[] { iname });
            //        break;
            //    case 32:
            //        g_tiHintStr1 = string.Format("高级鉴定失败，你的%s消失了！", new string[] { iname });
            //        break;
            //    case 33:
            //        g_tiHintStr1 = string.Format("服务器没有%s的数据，高级鉴定失败！", new string[] { iname });
            //        break;
            //}
            return result;
        }

        public static byte GetTIHintString1(int idx)
        {
            return GetTIHintString1(idx, null);
        }

        public static byte GetTIHintString1(int idx, TClientItem ci)
        {
            return GetTIHintString1(idx, ci, "");
        }

        public static byte GetTIHintString2(int idx, TClientItem ci, string iname)
        {
            byte result;
            result = 0;
            /*g_tiHintStr1 = "";
            switch (idx)
            {
                case 0:
                    g_tiHintStr1 = "如果你不喜欢已经鉴定过了的宝物，你可以把他给我，我平素最爱收藏各种宝物，我会给你一个一模一样的没鉴定过的装备作为补偿。";
                    FrmDlg.DBTIbtn1.btnState = tdisable;
                    break;
                case 1:
                    g_tiHintStr1 = string.Format("这个%s，看上去不错，我这里正好有没有鉴定过的各种%s你可以挑一把，要换的话，你要给我一个幸运符。", new string[] { ci.s.Name, ci.s.Name });
                    FrmDlg.DBTIbtn1.btnState = tnor;
                    break;
                case 2:
                    g_tiHintStr1 = string.Format("我已经给了你一把没鉴定过的%s，跟你原来的%s没鉴定过之前是一模一样的！", new string[] { iname, iname });
                    break;
                case 3:
                    g_tiHintStr1 = "缺少宝物或材料。";
                    break;
                case 4:
                    g_tiHintStr1 = string.Format("你的这件%s并没有鉴定过。", new string[] { iname });
                    break;
                case 5:
                    g_tiHintStr1 = "材料不符合，请放入幸运符。";
                    break;
                case 6:
                    g_tiHintStr1 = "该物品框只能放鉴定过的宝物，你的东西不符合，我已经将它放回你的包裹了。";
                    break;
                case 7:
                    g_tiHintStr1 = "该物品框只能放幸运符，你的东西不符合，我已经将它放回你的包裹了。";
                    break;
                case 8:
                    g_tiHintStr1 = "宝物更换失败。";
                    break;
            }*/
            return result;
        }

        public static byte GetTIHintString2(int idx)
        {
            return GetTIHintString2(idx, null);
        }

        public static byte GetTIHintString2(int idx, TClientItem ci)
        {
            return GetTIHintString2(idx, ci, "");
        }

        public static byte GetSpHintString1(int idx, TClientItem ci, string iname)
        {
            byte result;
            result = 0;
            /*g_spHintStr1 = "";
            switch (idx)
            {
                case 0:
                    g_spHintStr1 = "你可以跟别人购买神秘卷轴，也可以自己制作神秘卷轴来解读宝物的神秘属性。";
                    break;
                case 1:
                    g_spHintStr1 = "这次解读不幸失败，解读幸运值、神秘卷轴" + "的等级和熟练度过低可能导致解读失败，不" + "要失望，再接再厉吧。";
                    break;
                case 2:
                    g_spHintStr1 = "找不到鉴定物品或卷轴";
                    break;
                case 3:
                    FrmDlg.DBSP.btnState = tdisable;
                    g_spHintStr1 = "没有可鉴定的神秘属性";
                    break;
                case 4:
                    g_spHintStr1 = "装备不符合神秘解读要求";
                    break;
                case 5:
                    g_spHintStr1 = "卷轴类型不符合";
                    break;
                case 6:
                    g_spHintStr1 = "卷轴等级不符合";
                    break;
                case 7:
                    g_spHintStr1 = "借助神秘卷轴的帮助，已经帮你解读出了一个神秘属性";
                    break;
                case 10:
                    g_spHintStr1 = "神秘卷轴制作成功。";
                    break;
                case 11:
                    g_spHintStr1 = "这次制作不幸的失败了，可能是因为你的神" + "秘解读技能等级还不够高，或者是你制作的" + "卷轴等级太高了";
                    break;
                case 12:
                    g_spHintStr1 = "找不到羊皮卷。";
                    break;
                case 13:
                    g_spHintStr1 = "请放入羊皮卷。";
                    break;
                case 14:
                    g_spHintStr1 = "精力值不够。";
                    break;
                case 15:
                    g_spHintStr1 = "没有解读技能，制作失败。";
                    break;
            }*/
            return result;
        }

        public static byte GetSpHintString1(int idx)
        {
            return GetSpHintString1(idx, null);
        }

        public static byte GetSpHintString1(int idx, TClientItem ci)
        {
            return GetSpHintString1(idx, ci, "");
        }

        public static byte GetSpHintString2(int idx, TClientItem ci, string iname)
        {
            byte result;
            result = 0;
            g_spHintStr1 = "";
            switch (idx)
            {
                case 0:
                    g_spHintStr1 = "你可以把你对鉴宝的心得还有你的鉴定经验写在神秘卷轴上，这样的话，就可以帮助更多人解读神秘属性。";
                    break;
            }
            return result;
        }

        public static byte GetSpHintString2(int idx)
        {
            return GetSpHintString2(idx, null);
        }

        public static byte GetSpHintString2(int idx, TClientItem ci)
        {
            return GetSpHintString2(idx, ci, "");
        }

        public static void AutoPutOntiBooks()
        {
            int i;
            TClientItem cu;
            //if ((g_TIItems[0].Item.Item.Name != "") && (g_TIItems[0].Item.Item.Eva.EvaTimesMax > 0) && (g_TIItems[0].Item.Item.Eva.EvaTimes < g_TIItems[0].Item.Item.Eva.EvaTimesMax) && ((g_TIItems[1].Item.Item.Name == "") || (g_TIItems[1].Item.Item.StdMode != 56) || !(g_TIItems[1].Item.Item.Shape >= 1 && g_TIItems[1].Item.Item.Shape <= 3) || (g_TIItems[1].Item.Item.Shape != g_TIItems[0].Item.Item.Eva.EvaTimes + 1)))
            //{
            //    for (i = MAXBAGITEMCL - 1; i >= 6; i--)
            //    {
            //        if ((g_ItemArr[i].Item.Name != "") && (g_ItemArr[i].Item.StdMode == 56) && (g_ItemArr[i].Item.Shape == g_TIItems[0].Item.Eva.EvaTimes + 1))
            //        {
            //            if (g_TIItems[1].Item.Item.Name != "")
            //            {
            //                cu = g_TIItems[1].Item;
            //                g_TIItems[1].Item = g_ItemArr[i];
            //                g_TIItems[1].Index = i;
            //                g_ItemArr[i] = cu;
            //            }
            //            else
            //            {
            //                g_TIItems[1].Item = g_ItemArr[i];
            //                g_TIItems[1].Index = i;
            //                g_ItemArr[i].Item.Name = "";
            //            }
            //            break;
            //        }
            //    }
            //}
        }

        public static void AutoPutOntiSecretBooks()
        {
            int i;
            TClientItem cu;
            //if (FrmDlg.DWSP.Visible && (FrmDlg.DWSP.tag == 1) && (g_spItems[0].Item.Item.Name != "") && (g_spItems[0].Item.Item.Eva.EvaTimesMax > 0) && ((g_spItems[1].Item.Item.Name == "") || (g_spItems[1].Item.Item.StdMode != 56) || (g_spItems[1].Item.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             s.Shape != 0)))
            //{
            //    for (i = MAXBAGITEMCL - 1; i >= 6; i--)
            //    {
            //        if ((g_ItemArr[i].s.Name != "") && (g_ItemArr[i].s.StdMode == 56) && (g_ItemArr[i].s.Shape == 0))
            //        {
            //            if (g_spItems[1].Item.Item.Name != "")
            //            {
            //                cu = g_spItems[1].Item;
            //                g_spItems[1].Item = g_ItemArr[i];
            //                g_spItems[1].Index = i;
            //                g_ItemArr[i] = cu;
            //            }
            //            else
            //            {
            //                g_spItems[1].Item = g_ItemArr[i];
            //                g_spItems[1].Index = i;
            //                g_ItemArr[i].s.Name = "";
            //            }
            //            break;
            //        }
            //    }
            //}
        }

        public static void AutoPutOntiCharms()
        {
            int i;
            TClientItem cu;
            //if ((g_TIItems[0].Item.Item.Name != "") && (g_TIItems[0].Item.Item.Eva.EvaTimesMax > 0) && (g_TIItems[0].Item.Item.Eva.EvaTimes > 0) && ((g_TIItems[1].Item.Item.Name == "") || (g_TIItems[1].Item.Item.StdMode != 41) || (g_TIItems[1].Item.Item.Shape != 30)))
            //{
            //    for (i = MAXBAGITEMCL - 1; i >= 6; i--)
            //    {
            //        if ((g_ItemArr[i].s.Name != "") && (g_ItemArr[i].s.StdMode == 41) && (g_ItemArr[i].s.Shape == 30))
            //        {
            //            if (g_TIItems[1].Item.Item.Name != "")
            //            {
            //                cu = g_TIItems[1].Item;
            //                g_TIItems[1].Item = g_ItemArr[i];
            //                g_TIItems[1].Index = i;
            //                g_ItemArr[i] = cu;
            //            }
            //            else
            //            {
            //                g_TIItems[1].Item = g_ItemArr[i];
            //                g_TIItems[1].Index = i;
            //                g_ItemArr[i].Item.Name = "";
            //            }
            //            break;
            //        }
            //    }
            //}
        }

        public static bool GetSuiteAbil(int idx, int Shape, ref byte[] sa)
        {
            bool result = false;
            //FillChar(sa);           
            //switch (idx)
            //{
            //    case 1:
            //        result = true;
            //        for (i = TtSuiteAbil.GetLowerBound(0); i <= TtSuiteAbil.GetUpperBound(0); i++ )
            //        {
            //            if ((g_UseItems[i].s.Name != "") && ((g_UseItems[i].s.Shape == Shape) || (g_UseItems[i].s.AniCount == Shape)))
            //            {
            //                sa[i] = 1;
            //            }
            //        }
            //        break;
            //    case 2:
            //        result = true;
            //        for (i = Grobal2.byte.GetLowerBound(0); i <= Grobal2.byte.GetUpperBound(0); i++ )
            //        {
            //            if ((g_HeroUseItems[i].s.Name != "") && ((g_HeroUseItems[i].s.Shape == Shape) || (g_HeroUseItems[i].s.AniCount == Shape)))
            //            {
            //                sa[i] = 1;
            //            }
            //        }
            //        break;
            //    case 3:
            //        result = true;
            //        for (i = Grobal2.byte.GetLowerBound(0); i <= Grobal2.byte.GetUpperBound(0); i++ )
            //        {
            //            if ((UserState1.UseItems[i].s.Name != "") && ((UserState1.UseItems[i].s.Shape == Shape) || (UserState1.UseItems[i].s.AniCount == Shape)))
            //            {
            //                sa[i] = 1;
            //            }
            //        }
            //        break;
            //}
            return result;
        }

        public static void InitScreenConfig()
        {
            // 屏幕顶部滚动公告-范围
            //g_SkidAD_Rect.Left = 5;
            //g_SkidAD_Rect.Top = 7;
            //g_SkidAD_Rect.Right = SCREENWIDTH - 5;
            //g_SkidAD_Rect.Bottom = 7 + 20;
            //g_SkidAD_Rect2.Left = 183;
            //g_SkidAD_Rect2.Top = 6;
            //g_SkidAD_Rect2.Right = SCREENWIDTH - 208;
            //g_SkidAD_Rect2.Bottom = 6 + 20;
            //G_RC_SQUENGINER.Left = 78;
            //G_RC_SQUENGINER.Top = 90;
            //G_RC_SQUENGINER.Right = G_RC_SQUENGINER.Left + 16;
            //G_RC_SQUENGINER.Bottom = G_RC_SQUENGINER.Top + 95;
            //G_RC_IMEMODE.Left = SCREENWIDTH - 270 - 65;
            //G_RC_IMEMODE.Top = 105;
            //G_RC_IMEMODE.Right = G_RC_IMEMODE.Left + 60;
            //G_RC_IMEMODE.Bottom = G_RC_IMEMODE.Top + 9;
        }

        public static bool IsInMyRange(TActor Act)
        {
            bool result;
            result = false;
            if ((Act == null) || (g_MySelf == null))
            {
                return result;
            }
            if ((Math.Abs(Act.m_nCurrX - g_MySelf.m_nCurrX) <= (g_TileMapOffSetX - 2)) && (Math.Abs(Act.m_nCurrY - g_MySelf.m_nCurrY) <= (g_TileMapOffSetY - 1)))
            {
                result = true;
            }
            return result;
        }

        public static bool IsItemInMyRange(int X, int Y)
        {
            bool result;
            result = false;
            if ((g_MySelf == null))
            {
                return result;
            }
            if ((Math.Abs(X - g_MySelf.m_nCurrX) <= HUtil32._MIN(24, g_TileMapOffSetX + 9)) && (Math.Abs(Y - g_MySelf.m_nCurrY) <= HUtil32._MIN(24, (g_TileMapOffSetY + 10))))
            {
                result = true;
            }
            return result;
        }

        public static TClientStdItem GetTitle(int nItemIdx)
        {
            TClientStdItem result;
            result = null;
            nItemIdx -= 1;
            if ((nItemIdx >= 0) && (g_TitlesList.Count > nItemIdx))
            {
                if (((TStdItem)(g_TitlesList[nItemIdx])).Name != "")
                {
                    result = g_TitlesList[nItemIdx];
                }
            }
            return result;
        }

        public void initialization()
        {
            //g_APPass = new double();
            //g_dwThreadTick = new long();
            //g_dwThreadTick = 0;
            //g_pbRecallHero = new bool();
            //g_pbRecallHero = false;
            //InitializeCriticalSection(ProcMsgCS);
            //InitializeCriticalSection(ThreadCS);
            //g_APPickUpList = new THStringList();
            //g_APMobList = new THStringList();
            //g_ItemsFilter_All = new object();
            //g_ItemsFilter_All_Def = new object();
            //g_ItemsFilter_Dress = new object();
            //g_ItemsFilter_Weapon = new object();
            //g_ItemsFilter_Headgear = new object();
            //g_ItemsFilter_Drug = new object();
            //g_ItemsFilter_Other = new object();
            //g_SuiteItemsList = new object();
            //g_TitlesList = new object();
            //g_xMapDescList = new object();
            //g_xCurMapDescList = new object();
        }

        public void finalization()
        {
            //Dispose(g_APPass);
            //DeleteCriticalSection(ProcMsgCS);
            //DeleteCriticalSection(ThreadCS);
            //g_APPickUpList.Free;
            //g_APMobList.Free;
            //g_ItemsFilter_All.Free;
            //g_ItemsFilter_All_Def.Free;
            //g_ItemsFilter_Dress.Free;
            //g_ItemsFilter_Weapon.Free;
            //g_ItemsFilter_Headgear.Free;
            //g_ItemsFilter_Drug.Free;
            //g_ItemsFilter_Other.Free;
            //g_SuiteItemsList.Free;
            //g_xMapDescList.Free;
            //g_xCurMapDescList.Free;
        }

    }

    public struct TVaInfo
    {
        public string cap;
        public Rectangle[] pt1;
        public Rectangle[] pt2;
        public string[] str1;
        public string[] Hint;
    } // end TVaInfo

    public struct TFindNode
    {
        public int X;
        public int Y;
    } // end TFindNode

    public struct Tree
    {
        public int H;
        public int X;
        public int Y;
        public byte Dir;
        public Tree Father;
    }

    public struct Link
    {
        public Tree Node;
        public int F;
        public Link Next;
    }

    public struct TVirusSign
    {
        public int Offset;
        public string CodeSign;
    }

    public struct TMovingItem
    {
        public int Index;
        public TClientItem Item;
    } 

    public struct TCleintBox
    {
        public int Index;
        public TClientItem Item;
    }

    public struct TMoveHMShow
    {
        public TDirectDrawSurface Surface;
        public long dwMoveHpTick;
    }

    public struct TShowItem
    {
        public string sItemName;
        public TItemType ItemType;
        public bool boAutoPickup;
        public bool boShowName;
        public int nFColor;
        public int nBColor;
    } // end TShowItem

    public struct TMapDescInfo
    {
        public string szMapTitle;
        public string szPlaceName;
        public int nPointX;
        public int nPointY;
        public Color nColor;
        public int nFullMap;
    } // end TMapDescInfo

    public struct TItemShine
    {
        public int idx;
        public long tick;
    } // end TItemShine

    public struct TSeriesSkill
    {
        public byte wMagid;
        public byte nStep;
        public bool bSpell;
    } // end TSeriesSkill

    public struct TTempSeriesSkillA
    {
        public TClientMagic pm;
        public bool bo;
    } // end TTempSeriesSkillA

    public enum TTimerCommand
    {
        tcSoftClose,
        tcReSelConnect,
        tcFastQueryChr,
        tcQueryItemPrice
    } // end TTimerCommand

    public enum TChrAction
    {
        caWalk,
        caRun,
        caHorseRun,
        caHit,
        caSpell,
        caSitdown
    } // end TChrAction

    public enum TConnectionStep
    {
        cnsIntro,
        cnsLogin,
        cnsSelChr,
        cnsReSelChr,
        cnsPlay
    } // end TConnectionStep

    public enum TItemType
    {
        i_HPDurg,
        i_MPDurg,
        i_HPMPDurg,
        i_OtherDurg,
        i_Weapon,
        i_Dress,
        i_Helmet,
        i_Necklace,
        i_Armring,
        i_Ring,
        i_Belt,
        i_Boots,
        i_Charm,
        i_Book,
        i_PosionDurg,
        i_UseItem,
        i_Scroll,
        i_Stone,
        i_Gold,
        i_Other
    } // end TItemType

}