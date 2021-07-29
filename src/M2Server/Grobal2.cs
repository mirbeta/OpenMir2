using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace M2Server
{

    /// <summary>
    /// 假人登陆结构
    /// </summary>
    public class TAILogon
    {
        public string sCharName;//名字
        public string sMapName;//地图
        public string sConfigFileName;//人物配置路径
        public string sHeroConfigFileName;//英雄配置路径
        public string sFilePath;
        public string sConfigListFileName;//人物配置列表目录
        public string sHeroConfigListFileName;//英雄配置列表目录
        public short nX;//X坐标
        public short nY;//Y坐标
    }


    public enum MessageType
    {
        Success = 0,
        Error = 1
    }

    public enum MessageColor
    {
        Red = 0,
        Black = 1,
        Green = 2
    }

    public enum MessageLevel
    {
        Hihg = 3,
        Low = 2,
        None = 1
    }
    
    public class TMapWalkXY
    {
        public int nWalkStep;
        // 走步长
        public int nMonCount;
        // 怪数量
        public int nMonRange;
        // 怪范围
        public int nMastrRange;
        // 主体范围
        public short nX;
        public short nY;
    }
    
    public enum TPathType
    {
        /// <summary>
        /// 动态
        /// </summary>
        t_Dynamic,
        /// <summary>
        /// 固定
        /// </summary>
        t_Fixed
    } 
    
    public class TRunPos
    {
        public byte btDirection;
        // 1顺时针 2逆时针
        public int nAttackCount;
    }

    public struct TChrMsg
    {
        public int Ident;
        public int X;
        public int Y;
        public int Dir;
        public int State;
        public int feature;
        public string saying;
        public int Sound;
    }

    public class TRouteInfo
    {
        public int nServerIdx;
        public int nGateCount;
        public string sSelGateIP;
        public string[] sGameGateIP;
        public int[] nGameGatePort;

        public TRouteInfo()
        {
            sGameGateIP = new string[16];
            nGameGatePort = new int[16];
        }
    }

    public class TOUserStateInfo
    {
        public int Feature;
        public string UserName;
        public string GuildName;
        public string GuildRankName;
        public short NameColor;
        public TOClientItem[] UseItems;
    }

    public struct TClientGoods
    {
        public string Name;
        public int SubMenu;
        public int Price;
        public int Stock;
        public int Grade;
    }

    public struct THumInfo
    {
        public bool boDeleted;
        public bool boSelected;
        public string[] sAccount;
        public DateTime dModDate;
        public string[] sChrName;
        public byte btCount;
        public TRecordHeader Header;
    }

    public class TMapItem
    {
        public int Id;
        public string Name;
        public ushort Looks;
        public byte AniCount;
        public int Reserved;
        public int Count;
        public object DropBaseObject;
        public object OfBaseObject;
        public int dwCanPickUpTick;
        public TUserItem UserItem;
    }

    public class TDoorStatus
    {
        public bool bo01;
        public int n04;
        public bool boOpened;
        public int dwOpenTick;
        public int nRefCount;
    }

    public class TDoorInfo
    {
        public int nX;
        public int nY;
        public TDoorStatus Status;
        public int n08;
    }

    public class TMapFlag
    {
        public bool boSAFE;
        public int nL;
        public int nNEEDSETONFlag;
        public int nNeedONOFF;
        public int nMUSICID;
        public bool boDarkness;
        public bool boDayLight;
        public bool boFightZone;
        public bool boFight3Zone;
        public bool boQUIZ;
        public bool boNORECONNECT;
        public string sNoReConnectMap;
        public bool boMUSIC;
        public bool boEXPRATE;
        public int nEXPRATE;
        public bool boPKWINLEVEL;
        public int nPKWINLEVEL;
        public bool boPKWINEXP;
        public int nPKWINEXP;
        public bool boPKLOSTLEVEL;
        public int nPKLOSTLEVEL;
        public bool boPKLOSTEXP;
        public int nPKLOSTEXP;
        public bool boDECHP;
        public int nDECHPPOINT;
        public int nDECHPTIME;
        public bool boINCHP;
        public int nINCHPPOINT;
        public int nINCHPTIME;
        public bool boDECGAMEGOLD;
        public int nDECGAMEGOLD;
        public int nDECGAMEGOLDTIME;
        public bool boDECGAMEPOINT;
        public int nDECGAMEPOINT;
        public int nDECGAMEPOINTTIME;
        public bool boINCGAMEGOLD;
        public int nINCGAMEGOLD;
        public int nINCGAMEGOLDTIME;
        public bool boINCGAMEPOINT;
        public int nINCGAMEPOINT;
        public int nINCGAMEPOINTTIME;
        public bool boRUNHUMAN;
        public bool boRUNMON;
        public bool boNEEDHOLE;
        public bool boNORECALL;
        public bool boNOGUILDRECALL;
        public bool boNODEARRECALL;
        public bool boNOMASTERRECALL;
        public bool boNORANDOMMOVE;
        public bool boNODRUG;
        public bool boMINE;
        public bool boMINE2;
        public bool boNOPOSITIONMOVE;
        public bool boNODROPITEM;
        public bool boNOTHROWITEM;
        public bool boNOHORSE;
        public bool boNOCHAT;
        public bool boKILLFUNC;
        public int nKILLFUNCNO;
        public bool boNOHUMNOMON;
    } 

    public class TAddAbility
    {
        public ushort wHP;
        public ushort wMP;
        public ushort wHitPoint;
        public ushort wSpeedPoint;
        public int wAC;
        public int wMAC;
        public int wDC;
        public int wMC;
        public int wSC;
        public ushort wAntiPoison;
        public ushort wPoisonRecover;
        public ushort wHealthRecover;
        public ushort wSpellRecover;
        public ushort wAntiMagic;
        public byte btLuck;
        public byte btUnLuck;
        public byte btWeaponStrong;
        public ushort nHitSpeed;
        public byte btUndead;
        public ushort Weight;
        public ushort WearWeight;
        public ushort HandWeight;

        public TAddAbility()
        { }
    } 

    public class TProcessMessage
    {
        public int wIdent;
        public int wParam;
        public int nParam1;
        public int nParam2;
        public int nParam3;
        public int dwDeliveryTime;
        public int BaseObject;
        public bool boLateDelivery;
        public string sMsg;
    }

    public class TSessInfo
    {
        public int nSessionID;
        public string sAccount;
        public string sIPaddr;
        public int nPayMent;
        public int nPayMode;
        public int nSessionStatus;
        public int dwStartTick;
        public int dwActiveTick;
        public int nRefCount;
        public int nSocket;
        public int nGateIdx;
        public int nGSocketIdx;
        public int dwNewUserTick;
        public int nSoftVersionDate;
    }

    public struct TScriptQuestInfo
    {
        public short wFlag;
        public byte btValue;
        public int nRandRage;
    } 

    public class TScript
    {
        public bool boQuest;
        public TScriptQuestInfo[] QuestInfo;
        //public IList<TSayingRecord> RecordList;
        public Dictionary<string, TSayingRecord> RecordList;
        public int nQuest;
    }

    public struct TGameCmd
    {
        public string sCmd;
        public int nPerMissionMin;
        public int nPerMissionMax;
    }

    public class TLoadDBInfo
    {
        public int nGateIdx;
        public int nSocket;
        public string sAccount;
        public string sCharName;
        public int nSessionID;
        public string sIPaddr;
        public int nSoftVersionDate;
        public int nPayMent;
        public int nPayMode;
        public int nGSocketIdx;
        public int dwNewUserTick;
        public object PlayObject;
        public int nReLoadCount;
    }

    public class TGoldChangeInfo
    {
        public string sGameMasterName;
        public string sGetGoldUser;
        public int nGold;
    }

    public class TSaveRcd
    {
        public string sAccount;
        public string sChrName;
        public int nSessionID;
        public TPlayObject PlayObject;
        public THumDataInfo HumanRcd;
        public int nReTryCount;

        public TSaveRcd()
        {
            HumanRcd = new THumDataInfo();
        }
    }

    public class TStartPoint
    {
        public string m_sMapName;
        public short m_nCurrX;
        public short m_nCurrY;
        public bool m_boNotAllowSay;
        public int m_nRange;
        public int m_nType;
        public int m_nPkZone;
        public int m_nPkFire;
        public byte m_btShape;
    } 

    public class TMonGenInfo
    {
        public string sMapName;
        public int nX;
        public int nY;
        public string sMonName;
        public int nRange;
        public int nCount;
        public int dwZenTime;
        public int nMissionGenRate;
        public IList<TBaseObject> CertList;
        public int CertCount;
        public object Envir;
        public int nRace;
        public int dwStartTick;
    }

    public struct TMonInfo
    {
        public IList<TMonItem> ItemList;
        public string sName;
        public byte btRace;
        public byte btRaceImg;
        public ushort wAppr;
        public ushort wLevel;
        public byte btLifeAttrib;
        public short wCoolEye;
        public int dwExp;
        public ushort wHP;
        public ushort wMP;
        public ushort wAC;
        public ushort wMAC;
        public ushort wDC;
        public ushort wMaxDC;
        public ushort wMC;
        public ushort wSC;
        public ushort wSpeed;
        public ushort wHitPoint;
        public ushort wWalkSpeed;
        public ushort wWalkStep;
        public ushort wWalkWait;
        public ushort wAttackSpeed;
        public ushort wAntiPush;
        public bool boAggro;
        public bool boTame;
    } 

    public class TMonItem
    {
        public int MaxPoint;
        public int SelPoint;
        public string ItemName;
        public int Count;
    } 

    public struct TUnbindInfo
    {
        public int nUnbindCode;
        public string sItemName;
    }

    public struct TSlaveInfo
    {
        public string sSlaveName;
        public byte btSlaveLevel;
        public int dwRoyaltySec;
        public int nKillCount;
        public byte btSalveLevel;
        public byte btSlaveExpLevel;
        public ushort nHP;
        public ushort nMP;
    }

    public class TSwitchDataInfo
    {
        public string sMap;
        public short wX;
        public short wY;
        public TAbility Abil;
        public string sChrName;
        public int nCode;
        public bool boC70;
        public bool boBanShout;
        public bool boHearWhisper;
        public bool boBanGuildChat;
        public bool boAdminMode;
        public bool boObMode;
        public IList<string> BlockWhisperArr;
        public TSlaveInfo[] SlaveArr;
        public ushort[] StatusValue;
        public int[] StatusTimeOut;
        public int dwWaitTime;
    }

    public struct TIPaddr
    {
        public string sIpaddr;
        public string dIPaddr;
    }

    public class TGateInfo
    {
        /// <summary>
        /// 网关是否已启用
        /// </summary>
        public bool boUsed;
        public Socket Socket;
        public string SocketId;
        /// <summary>
        /// 网关IP
        /// </summary>
        public string sAddr;
        /// <summary>
        /// 端口
        /// </summary>
        public int nPort;
        public int n520;
        /// <summary>
        /// 玩家列表
        /// </summary>
        public IList<TGateUserInfo> UserList;
        /// <summary>
        /// 在线人数
        /// </summary>
        public int nUserCount;
        public byte[] Buffer;
        public int nBuffLen;
        public IList<byte[]> BufferList;
        public bool boSendKeepAlive;
        public int nSendChecked;
        public int nSendBlockCount;
        public int dwStartTime;
        /// <summary>
        /// 列队数据
        /// </summary>
        public int nSendMsgCount;
        /// <summary>
        /// 剩余数据
        /// </summary>
        public int nSendRemainCount;
        /// <summary>
        /// 发送间隔
        /// </summary>
        public int dwSendTick;
        public int nSendMsgBytes;
        /// <summary>
        /// 发送的字节数量
        /// </summary>
        public int nSendBytesCount;
        /// <summary>
        /// 发送数据
        /// </summary>
        public int nSendedMsgCount;
        /// <summary>
        /// 发送数量
        /// </summary>
        public int nSendCount;
        /// <summary>
        /// 上次心跳时间
        /// </summary>
        public int dwSendCheckTick;
    }

    public class TGateUserInfo
    {
        /// <summary>
        /// 人物对象
        /// </summary>
        public TPlayObject PlayObject;
        public int nSessionID;
        /// <summary>
        /// 账号
        /// </summary>
        public string sAccount;
        public int nGSocketIdx;
        /// <summary>
        /// 玩家IP
        /// </summary>
        public string sIPaddr;
        /// <summary>
        /// 认证是否通过
        /// </summary>
        public bool boCertification;
        /// <summary>
        /// 玩家名称
        /// </summary>
        public string sCharName;
        /// <summary>
        /// 客户端版本号
        /// </summary>
        public int nClientVersion;
        /// <summary>
        /// 当前会话信息
        /// </summary>
        public TSessInfo SessInfo;
        public int nSocket;
        public TFrontEngine FrontEngine;
        public UserEngine UserEngine;
        public int dwNewUserTick;
    } 

    public struct TRecallMigic
    {
        public int nHumLevel;
        public string sMonName;
        public int nCount;
        public int nLevel;
    }

    public struct TAdminInfo
    {
        public int nLv;
        public string sChrName;
        public string sIPaddr;
    } 

    public class TMonDrop
    {
        public string sItemName;
        public int nDropCount;
        public int nNoDropCount;
        public int nCountLimit;
    }

    public class TMonSayMsg
    {
        public TMonStatus State;
        public TMsgColor Color;
        public int nRate;
        public string sSayMsg;
    } 

    public class TDynamicVar
    {
        public string sName;
        public TVarType VarType;
        public int nInternet;
        public string sString;
    }

    public class TItemName
    {
        public int nMakeIndex;
        public int nItemIndex;
        public string sItemName = string.Empty;
    }

    public struct TSrvNetInfo
    {
        public string sIPaddr;
        public int nPort;
    }

    public class TUserOpenInfo
    {
        public string sChrName;
        public TLoadDBInfo LoadUser;
        public THumDataInfo HumanRcd;
    }

    public class TGateObj
    {
        public TEnvirnoment DEnvir;
        public short nDMapX;
        public short nDMapY;
        public bool boFlag;
    }

    public struct TMapQuestInfo
    {
        public int nFlag;
        public int nValue;
        public string sMonName;
        public string sItemName;
        public bool boGrouped;
        public object NPC;
    } 

    public enum TMsgColor
    {
        c_Red,
        c_Green,
        c_Blue,
        c_White
    }

    public enum TMsgType
    {
        t_System,
        t_Notice,
        t_Hint,
        t_Say,
        t_Castle,
        t_Cust,
        t_GM,
        t_Mon
    }

    public enum TMonStatus
    {
        s_KillHuman,
        s_UnderFire,
        s_Die,
        s_MonGen
    }

    public enum TVarType
    {
        vNone,
        VInteger,
        VString
    }
}

namespace M2Server
{
    public class grobal2
    {
        public const string VERSION_NUMBER_STR = "当前版本：20161001";
        public const int VERSION_NUMBER = 20020522;
        public const int CLIENT_VERSION_NUMBER = 120040918;
        public const int CM_POWERBLOCK = 0;
        // Damian
        public const int MapNameLen = 16;
        public const int ActorNameLen = 14;
        public const int DR_UP = 0;
        public const int DR_UPRIGHT = 1;
        public const int DR_RIGHT = 2;
        public const int DR_DOWNRIGHT = 3;
        public const int DR_DOWN = 4;
        public const int DR_DOWNLEFT = 5;
        public const int DR_LEFT = 6;
        public const int DR_UPLEFT = 7;
        /// <summary>
        /// 衣服
        /// </summary>
        public const int U_DRESS = 0;
        /// <summary>
        /// 武器
        /// </summary>
        public const int U_WEAPON = 1;
        public const int U_RIGHTHAND = 2;
        public const int U_NECKLACE = 3;
        public const int U_HELMET = 4;
        public const int U_ARMRINGL = 5;
        public const int U_ARMRINGR = 6;
        public const int U_RINGL = 7;
        public const int U_RINGR = 8;
        public const int U_BUJUK = 9;
        public const int U_BELT = 10;
        public const int U_BOOTS = 11;
        public const int U_CHARM = 12;
        public const int DEFBLOCKSIZE = 16;
        public const int BUFFERSIZE = 10000;
        public const int LOGICALMAPUNIT = 40;
        public const int UNITX = 48;
        public const int UNITY = 32;
        public const int HALFX = 24;
        public const int HALFY = 16;
        public const int MAXBAGITEM = 52;
        public const int HOWMANYMAGICS = 20;
        public const int USERITEMMAX = 46;
        // 用户最大的物品
        public const int MaxSkillLevel = 3;
        public const int MAX_STATUS_ATTRIBUTE = 12;
        // 物品类型(物品属性读取)
        public const int ITEM_WEAPON = 0;
        // 武器
        public const int ITEM_ARMOR = 1;
        // 装备
        public const int ITEM_ACCESSORY = 2;
        // 辅助物品
        public const int ITEM_ETC = 3;
        // 其它物品
        public const int ITEM_LEECHDOM = 4;
        // 药水
        public const int ITEM_GOLD = 10;
        // 金币
        public const int POISON_DECHEALTH = 0;
        // 中毒类型 - 绿毒
        public const int POISON_DAMAGEARMOR = 1;
        // 中毒类型 - 红毒
        public const int POISON_LOCKSPELL = 2;
        public const int POISON_DONTMOVE = 4;
        public const int POISON_STONE = 5;
        public const int STATE_LOCKRUN = 3;//不能跑动(中蛛网)
        public const int POISON_68 = 68;
        public const int STATE_TRANSPARENT = 8;
        public const int STATE_DEFENCEUP = 9;
        public const int STATE_MAGDEFENCEUP = 10;
        public const int STATE_BUBBLEDEFENCEUP = 11;
        public const int STATE_STONE_MODE = 0x00000001;
        public const int STATE_OPENHEATH = 0x00000002;
        // 眉仿 傍俺惑怕
        public const int ET_DIGOUTZOMBI = 1;
        // 粱厚啊 顶颇绊 唱柯 如利
        public const int ET_MINE = 2;
        // 堡籍捞 概厘登绢 乐澜
        public const int ET_PILESTONES = 3;
        // 倒公歹扁
        public const int ET_HOLYCURTAIN = 4;
        // 搬拌
        public const int ET_FIRE = 5;
        public const int ET_SCULPEICE = 6;
        // 林付空狼 倒柄柳 炼阿
        public const int RCC_MERCHANT = 50;
        public const int RCC_GUARD = 12;
        public const int RCC_USERHUMAN = 0;
        public const int CM_QUERYUSERSTATE = 82;
        public const int CM_QUERYUSERNAME = 80;
        public const int CM_QUERYBAGITEMS = 81;
        public const int CM_QUERYCHR = 100;
        public const int CM_NEWCHR = 101;
        public const int CM_DELCHR = 102;
        public const int CM_SELCHR = 103;
        public const int CM_SELECTSERVER = 104;
        public const int CM_OPENDOOR = 1002;
        public const int CM_SOFTCLOSE = 1009;
        public const int CM_DROPITEM = 1000;
        public const int CM_PICKUP = 1001;
        public const int CM_TAKEONITEM = 1003;
        public const int CM_TAKEOFFITEM = 1004;
        public const int CM_1005 = 1005;
        public const int CM_EAT = 1006;
        public const int CM_BUTCH = 1007;
        public const int CM_MAGICKEYCHANGE = 1008;
        public const int CM_CLICKNPC = 1010;
        public const int CM_MERCHANTDLGSELECT = 1011;
        public const int CM_MERCHANTQUERYSELLPRICE = 1012;
        public const int CM_USERSELLITEM = 1013;
        public const int CM_USERBUYITEM = 1014;
        public const int CM_USERGETDETAILITEM = 1015;
        public const int CM_DROPGOLD = 1016;
        public const int CM_1017 = 1017;
        public const int CM_LOGINNOTICEOK = 1018;
        public const int CM_GROUPMODE = 1019;
        public const int CM_CREATEGROUP = 1020;
        public const int CM_ADDGROUPMEMBER = 1021;
        public const int CM_DELGROUPMEMBER = 1022;
        public const int CM_USERREPAIRITEM = 1023;
        public const int CM_MERCHANTQUERYREPAIRCOST = 1024;
        public const int CM_DEALTRY = 1025;
        public const int CM_DEALADDITEM = 1026;
        public const int CM_DEALDELITEM = 1027;
        public const int CM_DEALCANCEL = 1028;
        public const int CM_DEALCHGGOLD = 1029;
        public const int CM_DEALEND = 1030;
        public const int CM_USERSTORAGEITEM = 1031;
        public const int CM_USERTAKEBACKSTORAGEITEM = 1032;
        public const int CM_WANTMINIMAP = 1033;
        public const int CM_USERMAKEDRUGITEM = 1034;
        public const int CM_OPENGUILDDLG = 1035;
        public const int CM_GUILDHOME = 1036;
        public const int CM_GUILDMEMBERLIST = 1037;
        public const int CM_GUILDADDMEMBER = 1038;
        public const int CM_GUILDDELMEMBER = 1039;
        public const int CM_GUILDUPDATENOTICE = 1040;
        public const int CM_GUILDUPDATERANKINFO = 1041;
        public const int CM_1042 = 1042;
        public const int CM_ADJUST_BONUS = 1043;
        public const int CM_GUILDALLY = 1044;
        public const int CM_GUILDBREAKALLY = 1045;
        public const int CM_SPEEDHACKUSER = 10430;
        // ??
        public const int CM_PROTOCOL = 2000;
        public const int CM_IDPASSWORD = 2001;
        public const int CM_ADDNEWUSER = 2002;
        public const int CM_CHANGEPASSWORD = 2003;
        public const int CM_UPDATEUSER = 2004;
        public const int CM_THROW = 3005;
        public const int CM_TURN = 3010;
        public const int CM_WALK = 3011;
        public const int CM_SITDOWN = 3012;
        public const int CM_RUN = 3013;
        public const int CM_HIT = 3014;
        public const int CM_HEAVYHIT = 3015;
        public const int CM_BIGHIT = 3016;
        public const int CM_SPELL = 3017;
        public const int CM_POWERHIT = 3018;
        public const int CM_LONGHIT = 3019;
        public const int CM_WIDEHIT = 3024;
        public const int CM_FIREHIT = 3025;
        public const int CM_SAY = 3030;
        public const int SM_41 = 4;
        public const int SM_THROW = 5;
        public const int SM_RUSH = 6;
        public const int SM_RUSHKUNG = 7;
        public const int SM_FIREHIT = 8;
        // 烈火
        public const int SM_BACKSTEP = 9;
        public const int SM_TURN = 10;
        public const int SM_WALK = 11;
        // 走
        public const int SM_SITDOWN = 12;
        public const int SM_RUN = 13;
        public const int SM_HIT = 14;
        // 砍
        public const int SM_HEAVYHIT = 15;
        public const int SM_BIGHIT = 16;
        public const int SM_SPELL = 17;
        // 使用魔法
        public const int SM_POWERHIT = 18;
        public const int SM_LONGHIT = 19;
        // 刺杀
        public const int SM_DIGUP = 20;
        public const int SM_DIGDOWN = 21;
        public const int SM_FLYAXE = 22;
        public const int SM_LIGHTING = 23;
        public const int SM_WIDEHIT = 24;
        public const int SM_CRSHIT = 25;
        public const int SM_TWINHIT = 26;
        public const int SM_ALIVE = 27;
        public const int SM_MOVEFAIL = 28;
        public const int SM_HIDE = 29;
        public const int SM_DISAPPEAR = 30;
        public const int SM_STRUCK = 31;
        // 弯腰
        public const int SM_DEATH = 32;
        public const int SM_SKELETON = 33;
        public const int SM_NOWDEATH = 34;
        public const int SM_HEAR = 40;
        public const int SM_FEATURECHANGED = 41;
        public const int SM_USERNAME = 42;
        public const int SM_43 = 43;
        public const int SM_WINEXP = 44;
        public const int SM_LEVELUP = 45;
        public const int SM_DAYCHANGING = 46;
        public const int SM_LOGON = 50;
        public const int SM_NEWMAP = 51;
        public const int SM_ABILITY = 52;
        public const int SM_HEALTHSPELLCHANGED = 53;
        public const int SM_MAPDESCRIPTION = 54;
        public const int SM_SPELL2 = 117;
        public const int SM_SYSMESSAGE = 100;
        public const int SM_GROUPMESSAGE = 101;
        public const int SM_CRY = 102;
        public const int SM_WHISPER = 103;
        public const int SM_GUILDMESSAGE = 104;
        public const int SM_ADDITEM = 200;
        public const int SM_BAGITEMS = 201;
        public const int SM_DELITEM = 202;
        public const int SM_UPDATEITEM = 203;
        public const int SM_ADDMAGIC = 210;
        public const int SM_SENDMYMAGIC = 211;
        public const int SM_DELMAGIC = 212;
        public const int SM_ATTACKMODE = 213;
        // 攻击模式
        public const int SM_CERTIFICATION_SUCCESS = 500;
        public const int SM_CERTIFICATION_FAIL = 501;
        public const int SM_ID_NOTFOUND = 502;
        public const int SM_PASSWD_FAIL = 503;
        public const int SM_NEWID_SUCCESS = 504;
        public const int SM_NEWID_FAIL = 505;
        public const int SM_CHGPASSWD_SUCCESS = 506;
        public const int SM_CHGPASSWD_FAIL = 507;
        public const int SM_QUERYCHR = 520;
        public const int SM_NEWCHR_SUCCESS = 521;
        public const int SM_NEWCHR_FAIL = 522;
        public const int SM_DELCHR_SUCCESS = 523;
        public const int SM_DELCHR_FAIL = 524;
        public const int SM_STARTPLAY = 525;
        public const int SM_STARTFAIL = 526;
        // SM_USERFULL
        public const int SM_QUERYCHR_FAIL = 527;
        public const int SM_OUTOFCONNECTION = 528;
        // ?
        public const int SM_PASSOK_SELECTSERVER = 529;
        public const int SM_SELECTSERVER_OK = 530;
        public const int SM_NEEDUPDATE_ACCOUNT = 531;
        public const int SM_UPDATEID_SUCCESS = 532;
        public const int SM_UPDATEID_FAIL = 533;
        public const int SM_DROPITEM_SUCCESS = 600;
        public const int SM_DROPITEM_FAIL = 601;
        public const int SM_ITEMSHOW = 610;
        public const int SM_ITEMHIDE = 611;
        public const int SM_OPENDOOR_OK = 612;
        public const int SM_OPENDOOR_LOCK = 613;
        public const int SM_CLOSEDOOR = 614;
        public const int SM_TAKEON_OK = 615;
        public const int SM_TAKEON_FAIL = 616;
        public const int SM_TAKEOFF_OK = 619;
        public const int SM_TAKEOFF_FAIL = 620;
        public const int SM_SENDUSEITEMS = 621;
        public const int SM_WEIGHTCHANGED = 622;
        public const int SM_CLEAROBJECTS = 633;
        public const int SM_CHANGEMAP = 634;
        public const int SM_EAT_OK = 635;
        public const int SM_EAT_FAIL = 636;
        public const int SM_BUTCH = 637;
        public const int SM_MAGICFIRE = 638;
        public const int SM_MAGICFIRE_FAIL = 639;
        public const int SM_MAGIC_LVEXP = 640;
        public const int SM_DURACHANGE = 642;
        public const int SM_MERCHANTSAY = 643;
        public const int SM_MERCHANTDLGCLOSE = 644;
        public const int SM_SENDGOODSLIST = 645;
        public const int SM_SENDUSERSELL = 646;
        public const int SM_SENDBUYPRICE = 647;
        public const int SM_USERSELLITEM_OK = 648;
        public const int SM_USERSELLITEM_FAIL = 649;
        public const int SM_BUYITEM_SUCCESS = 650;
        // ?
        public const int SM_BUYITEM_FAIL = 651;
        // ?
        public const int SM_SENDDETAILGOODSLIST = 652;
        public const int SM_GOLDCHANGED = 653;
        public const int SM_CHANGELIGHT = 654;
        public const int SM_LAMPCHANGEDURA = 655;
        public const int SM_CHANGENAMECOLOR = 656;
        public const int SM_CHARSTATUSCHANGED = 657;
        public const int SM_SENDNOTICE = 658;
        public const int SM_GROUPMODECHANGED = 659;
        public const int SM_CREATEGROUP_OK = 660;
        public const int SM_CREATEGROUP_FAIL = 661;
        public const int SM_GROUPADDMEM_OK = 662;
        public const int SM_GROUPDELMEM_OK = 663;
        public const int SM_GROUPADDMEM_FAIL = 664;
        public const int SM_GROUPDELMEM_FAIL = 665;
        public const int SM_GROUPCANCEL = 666;
        public const int SM_GROUPMEMBERS = 667;
        public const int SM_SENDUSERREPAIR = 668;
        public const int SM_USERREPAIRITEM_OK = 669;
        public const int SM_USERREPAIRITEM_FAIL = 670;
        public const int SM_SENDREPAIRCOST = 671;
        public const int SM_DEALMENU = 673;
        public const int SM_DEALTRY_FAIL = 674;
        public const int SM_DEALADDITEM_OK = 675;
        public const int SM_DEALADDITEM_FAIL = 676;
        public const int SM_DEALDELITEM_OK = 677;
        public const int SM_DEALDELITEM_FAIL = 678;
        public const int SM_DEALCANCEL = 681;
        public const int SM_DEALREMOTEADDITEM = 682;
        public const int SM_DEALREMOTEDELITEM = 683;
        public const int SM_DEALCHGGOLD_OK = 684;
        public const int SM_DEALCHGGOLD_FAIL = 685;
        public const int SM_DEALREMOTECHGGOLD = 686;
        public const int SM_DEALSUCCESS = 687;
        public const int SM_SENDUSERSTORAGEITEM = 700;
        public const int SM_STORAGE_OK = 701;
        public const int SM_STORAGE_FULL = 702;
        public const int SM_STORAGE_FAIL = 703;
        public const int SM_SAVEITEMLIST = 704;
        public const int SM_TAKEBACKSTORAGEITEM_OK = 705;
        public const int SM_TAKEBACKSTORAGEITEM_FAIL = 706;
        public const int SM_TAKEBACKSTORAGEITEM_FULLBAG = 707;
        public const int SM_AREASTATE = 766;
        public const int SM_MYSTATUS = 708;
        public const int SM_DELITEMS = 709;
        public const int SM_READMINIMAP_OK = 710;
        public const int SM_READMINIMAP_FAIL = 711;
        public const int SM_SENDUSERMAKEDRUGITEMLIST = 712;
        public const int SM_MAKEDRUG_SUCCESS = 713;
        public const int SM_MAKEDRUG_FAIL = 714;
        public const int SM_716 = 716;
        public const int SM_CHANGEGUILDNAME = 750;
        public const int SM_SENDUSERSTATE = 751;
        public const int SM_SUBABILITY = 752;
        public const int SM_OPENGUILDDLG = 753;
        public const int SM_OPENGUILDDLG_FAIL = 754;
        public const int SM_SENDGUILDMEMBERLIST = 756;
        public const int SM_GUILDADDMEMBER_OK = 757;
        public const int SM_GUILDADDMEMBER_FAIL = 758;
        public const int SM_GUILDDELMEMBER_OK = 759;
        public const int SM_GUILDDELMEMBER_FAIL = 760;
        public const int SM_GUILDRANKUPDATE_FAIL = 761;
        public const int SM_BUILDGUILD_OK = 762;
        public const int SM_BUILDGUILD_FAIL = 763;
        public const int SM_DONATE_OK = 764;
        public const int SM_DONATE_FAIL = 765;
        public const int SM_MENU_OK = 767;
        // ?
        public const int SM_GUILDMAKEALLY_OK = 768;
        public const int SM_GUILDMAKEALLY_FAIL = 769;
        public const int SM_GUILDBREAKALLY_OK = 770;
        // ?
        public const int SM_GUILDBREAKALLY_FAIL = 771;
        // ?
        public const int SM_DLGMSG = 772;
        // Jacky
        public const int SM_SPACEMOVE_HIDE = 800;
        public const int SM_SPACEMOVE_SHOW = 801;
        public const int SM_RECONNECT = 802;
        public const int SM_GHOST = 803;
        public const int SM_SHOWEVENT = 804;
        public const int SM_HIDEEVENT = 805;
        public const int SM_SPACEMOVE_HIDE2 = 806;
        public const int SM_SPACEMOVE_SHOW2 = 807;
        public const int SM_TIMECHECK_MSG = 810;
        public const int SM_ADJUST_BONUS = 811;
        // ?
        public const int SM_OPENHEALTH = 1100;
        public const int SM_CLOSEHEALTH = 1101;
        public const int SM_CHANGEFACE = 1104;
        public const int SM_BREAKWEAPON = 1102;
        public const int SM_INSTANCEHEALGUAGE = 1103;
        // ??
        public const int SM_VERSION_FAIL = 1106;
        public const int SM_ITEMUPDATE = 1500;
        public const int SM_MONSTERSAY = 1501;
        public const int SM_EXCHGTAKEON_OK = 65023;
        public const int SM_EXCHGTAKEON_FAIL = 65024;
        public const int SM_TEST = 65037;
        public const int SM_ACTION_MIN = 65070;
        public const int SM_ACTION_MAX = 65071;
        public const int SM_ACTION2_MIN = 65072;
        public const int SM_ACTION2_MAX = 65073;
        public const int CM_SERVERREGINFO = 65074;
        // -------------------------------------
        public const int CM_GETGAMELIST = 5001;
        public const int SM_SENDGAMELIST = 5002;
        public const int CM_GETBACKPASSWORD = 5003;
        public const int SM_GETBACKPASSWD_SUCCESS = 5005;
        public const int SM_GETBACKPASSWD_FAIL = 5006;
        public const int SM_SERVERCONFIG = 5007;
        public const int SM_GAMEGOLDNAME = 5008;
        public const int SM_PASSWORD = 5009;
        public const int SM_HORSERUN = 5010;
        public const int UNKNOWMSG = 199;
        // 以下几个正确
        public const int SS_OPENSESSION = 100;
        public const int SS_CLOSESESSION = 101;
        public const int SS_KEEPALIVE = 104;
        public const int SS_KICKUSER = 111;
        public const int SS_SERVERLOAD = 113;
        public const int SS_200 = 200;
        public const int SS_201 = 201;
        public const int SS_202 = 202;
        public const int SS_203 = 203;
        public const int SS_204 = 204;
        public const int SS_205 = 205;
        public const int SS_206 = 206;
        public const int SS_207 = 207;
        public const int SS_208 = 208;
        public const int SS_209 = 209;
        public const int SS_210 = 210;
        public const int SS_211 = 211;
        public const int SS_212 = 212;
        public const int SS_213 = 213;
        public const int SS_214 = 214;
        public const int SS_WHISPER = 299;
        // ?????
        // 不正确
        // Damian
        public const int SS_SERVERINFO = 103;
        public const int SS_SOFTOUTSESSION = 102;
        public const int SS_LOGINCOST = 30002;
        // Damian
        public const int DBR_FAIL = 2000;
        public const int DB_LOADHUMANRCD = 100;
        public const int DB_SAVEHUMANRCD = 101;
        public const int DB_SAVEHUMANRCDEX = 102;
        // ?
        public const int DBR_LOADHUMANRCD = 1100;
        public const int DBR_SAVEHUMANRCD = 1102;
        // ?
        public const int SG_FORMHANDLE = 32001;
        public const int SG_STARTNOW = 32002;
        public const int SG_STARTOK = 32003;
        public const int SG_CHECKCODEADDR = 32004;
        public const int SG_USERACCOUNT = 32005;
        public const int SG_USERACCOUNTCHANGESTATUS = 32006;
        public const int SG_USERACCOUNTNOTFOUND = 32007;
        public const int GS_QUIT = 32101;
        public const int GS_USERACCOUNT = 32102;
        public const int GS_CHANGEACCOUNTINFO = 32103;
        public const int WM_SENDPROCMSG = 32104;
        // Damian
        public const uint RUNGATECODE = 0xAA55AA55;
        public const int GM_OPEN = 1;
        public const int GM_CLOSE = 2;
        public const int GM_CHECKSERVER = 3;
        // Send check signal to Server
        public const int GM_CHECKCLIENT = 4;
        // Send check signal to Client
        public const int GM_DATA = 5;
        public const int GM_SERVERUSERINDEX = 6;
        public const int GM_RECEIVE_OK = 7;
        public const int GM_TEST = 20;
        // M2Server
        public const int GROUPMAX = 11;
        public const int CM_42HIT = 42;
        public const int CM_PASSWORD = 2001;
        public const int CM_CHGPASSWORD = 2002;
        public const int CM_SETPASSWORD = 2004;
        public const int CM_HORSERUN = 3035;
        // ------------未知消息码
        public const int CM_CRSHIT = 3036;
        // ------------未知消息码
        public const int CM_3037 = 3037;
        public const int CM_TWINHIT = 3038;
        public const int CM_QUERYUSERSET = 3040;
        // Damian
        public const int SM_PLAYDICE = 8001;
        public const int SM_PASSWORDSTATUS = 8002;
        public const int SM_NEEDPASSWORD = 8003;
        public const int SM_GETREGINFO = 8004;
        public const int DATA_BUFSIZE = 1024;
        public const int RUNGATEMAX = 8;
        // MAX_STATUS_ATTRIBUTE = 13;
        public const int MAXMAGIC = 54;
        public const string PN_GETRGB = "GetRGB";
        public const string PN_GAMEDATALOG = "GameDataLog";
        public const string PN_SENDBROADCASTMSG = "SendBroadcastMsg";
        public const string sSTRING_GOLDNAME = "金币";
        public const short MAXLEVEL = short.MaxValue;
        public const int MAXCHANGELEVEL = 1000;
        public const int SLAVEMAXLEVEL = 50;
        public const int LOG_GAMEGOLD = 1;
        public const int LOG_GAMEPOINT = 2;
        public const int RC_PLAYOBJECT = 0;
        public const int RC_GUARD = 11;
        public const int RC_PEACENPC = 15;
        public const int RC_ANIMAL = 50;
        public const int RC_EXERCISE = 55;
        // 练功师
        public const int RC_PLAYCLONE = 60;
        // 人型怪物
        public const int RC_MONSTER = 80;
        public const int RC_NPC = 10;
        public const int RC_ARCHERGUARD = 112;
        public const int RC_135 = 135;
        // 魔王岭弓箭手
        public const int RC_136 = 136;
        // 魔王岭弓箭手
        public const int RC_153 = 153;
        // 任务怪物
        public const int RM_TURN = 10001;
        public const int RM_WALK = 10002;
        public const int RM_HORSERUN = 50003;
        public const int RM_RUN = 10003;
        public const int RM_HIT = 10004;
        public const int RM_BIGHIT = 10006;
        public const int RM_HEAVYHIT = 10007;
        public const int RM_SPELL = 10008;
        public const int RM_SPELL2 = 10009;
        public const int RM_MOVEFAIL = 10010;
        public const int RM_LONGHIT = 10011;
        public const int RM_WIDEHIT = 10012;
        public const int RM_FIREHIT = 10014;
        public const int RM_CRSHIT = 10015;
        public const int RM_DEATH = 10021;
        public const int RM_SKELETON = 10024;
        public const int RM_LOGON = 10050;
        public const int RM_ABILITY = 10051;
        public const int RM_HEALTHSPELLCHANGED = 10052;
        public const int RM_DAYCHANGING = 10053;
        public const int RM_10101 = 10101;
        public const int RM_WEIGHTCHANGED = 10115;
        public const int RM_FEATURECHANGED = 10116;
        public const int RM_BUTCH = 10119;
        public const int RM_MAGICFIRE = 10120;
        public const int RM_MAGICFIREFAIL = 10121;
        public const int RM_SENDMYMAGIC = 10122;
        public const int RM_MAGIC_LVEXP = 10123;
        public const int RM_DURACHANGE = 10125;
        public const int RM_MERCHANTDLGCLOSE = 10127;
        public const int RM_SENDGOODSLIST = 10128;
        public const int RM_SENDUSERSELL = 10129;
        public const int RM_SENDBUYPRICE = 10130;
        public const int RM_USERSELLITEM_OK = 10131;
        public const int RM_USERSELLITEM_FAIL = 10132;
        public const int RM_BUYITEM_SUCCESS = 10133;
        public const int RM_BUYITEM_FAIL = 10134;
        public const int RM_SENDDETAILGOODSLIST = 10135;
        public const int RM_GOLDCHANGED = 10136;
        public const int RM_CHANGELIGHT = 10137;
        public const int RM_LAMPCHANGEDURA = 10138;
        public const int RM_CHARSTATUSCHANGED = 10139;
        public const int RM_GROUPCANCEL = 10140;
        public const int RM_SENDUSERREPAIR = 10141;
        public const int RM_SENDUSERSREPAIR = 50142;
        public const int RM_SENDREPAIRCOST = 10142;
        public const int RM_USERREPAIRITEM_OK = 10143;
        public const int RM_USERREPAIRITEM_FAIL = 10144;
        public const int RM_USERSTORAGEITEM = 10146;
        public const int RM_USERGETBACKITEM = 10147;
        public const int RM_SENDDELITEMLIST = 10148;
        public const int RM_USERMAKEDRUGITEMLIST = 10149;
        public const int RM_MAKEDRUG_SUCCESS = 10150;
        public const int RM_MAKEDRUG_FAIL = 10151;
        public const int RM_ALIVE = 10153;
        public const int RM_10155 = 10155;
        public const int RM_DIGUP = 10200;
        public const int RM_DIGDOWN = 10201;
        public const int RM_FLYAXE = 10202;
        public const int RM_LIGHTING = 10204;
        public const int RM_10205 = 10205;
        public const int RM_CHANGEGUILDNAME = 10301;
        public const int RM_SUBABILITY = 10302;
        public const int RM_BUILDGUILD_OK = 10303;
        public const int RM_BUILDGUILD_FAIL = 10304;
        public const int RM_DONATE_OK = 10305;
        public const int RM_DONATE_FAIL = 10306;
        public const int RM_MENU_OK = 10309;
        public const int RM_RECONNECTION = 10332;
        public const int RM_HIDEEVENT = 10333;
        public const int RM_SHOWEVENT = 10334;
        public const int RM_10401 = 10401;
        public const int RM_OPENHEALTH = 10410;
        public const int RM_CLOSEHEALTH = 10411;
        public const int RM_BREAKWEAPON = 10413;
        public const int RM_10414 = 10414;
        public const int RM_CHANGEFACE = 10415;
        public const int RM_PASSWORD = 10416;
        public const int RM_PLAYDICE = 10500;
        public const int RM_HEAR = 11001;
        public const int RM_WHISPER = 11002;
        public const int RM_CRY = 11003;
        public const int RM_SYSMESSAGE = 11004;
        public const int RM_GROUPMESSAGE = 11005;
        public const int RM_SYSMESSAGE2 = 11006;
        public const int RM_GUILDMESSAGE = 11007;
        public const int RM_SYSMESSAGE3 = 11008;
        public const int RM_MERCHANTSAY = 11009;
        public const int RM_ZEN_BEE = 8020;
        public const int RM_DELAYMAGIC = 8021;
        public const int RM_STRUCK = 8018;
        public const int RM_MAGSTRUCK_MINE = 8030;
        public const int RM_MAGHEALING = 8034;
        public const int RM_POISON = 8037;
        public const int RM_DOOPENHEALTH = 8040;
        public const int RM_SPACEMOVE_FIRE2 = 8042;
        public const int RM_DELAYPUSHED = 8043;
        public const int RM_MAGSTRUCK = 8044;
        public const int RM_TRANSPARENT = 8045;
        public const int RM_DOOROPEN = 8046;
        public const int RM_DOORCLOSE = 8047;
        public const int RM_DISAPPEAR = 8061;
        public const int RM_SPACEMOVE_FIRE = 8062;
        public const int RM_SENDUSEITEMS = 8074;
        public const int RM_WINEXP = 8075;
        public const int RM_ADJUST_BONUS = 8078;
        public const int RM_ITEMSHOW = 8082;
        public const int RM_GAMEGOLDCHANGED = 8084;
        public const int RM_ITEMHIDE = 8085;
        public const int RM_LEVELUP = 8086;
        public const int RM_CHANGENAMECOLOR = 8090;
        public const int RM_PUSH = 8092;
        public const int RM_CLEAROBJECTS = 8097;
        public const int RM_CHANGEMAP = 8098;
        public const int RM_SPACEMOVE_SHOW2 = 8099;
        public const int RM_SPACEMOVE_SHOW = 8100;
        public const int RM_USERNAME = 8101;
        public const int RM_MYSTATUS = 8102;
        public const int RM_STRUCK_MAG = 8103;
        public const int RM_RUSH = 8104;
        public const int RM_RUSHKUNG = 8105;
        public const int RM_PASSWORDSTATUS = 8106;
        public const int RM_POWERHIT = 8107;
        public const int RM_41 = 9041;
        public const int RM_TWINHIT = 9042;
        public const int RM_43 = 9043;

        // -------Start Inter Server Msg-------
        public const int ISM_GROUPSERVERHEART = 100;
        public const int ISM_USERSERVERCHANGE = 200;
        public const int ISM_USERLOGON = 201;
        public const int ISM_USERLOGOUT = 202;
        public const int ISM_WHISPER = 203;
        public const int ISM_SYSOPMSG = 204;
        public const int ISM_ADDGUILD = 205;
        public const int ISM_DELGUILD = 206;
        public const int ISM_RELOADGUILD = 207;
        public const int ISM_GUILDMSG = 208;
        public const int ISM_CHATPROHIBITION = 209;
        public const int ISM_CHATPROHIBITIONCANCEL = 210;
        public const int ISM_CHANGECASTLEOWNER = 211;
        public const int ISM_RELOADCASTLEINFO = 212;
        public const int ISM_RELOADADMIN = 213;
        // -------End Inter Server Msg-------
        // Friend System -------------
        public const int ISM_FRIEND_INFO = 214;
        public const int ISM_FRIEND_DELETE = 215;
        public const int ISM_FRIEND_OPEN = 216;
        public const int ISM_FRIEND_CLOSE = 217;
        public const int ISM_FRIEND_RESULT = 218;
        // Tag System ----------------
        public const int ISM_TAG_SEND = 219;
        public const int ISM_TAG_RESULT = 220;
        // User System --------------
        public const int ISM_USER_INFO = 221;
        public const int ISM_CHANGESERVERRECIEVEOK = 222;
        public const int ISM_RELOADCHATLOG = 223;
        public const int ISM_MARKETOPEN = 224;
        public const int ISM_MARKETCLOSE = 225;
        // relationship --------------
        public const int ISM_LM_DELETE = 226;
        public const int ISM_RELOADMAKEITEMLIST = 227;
        public const int ISM_GUILDMEMBER_RECALL = 228;
        public const int ISM_RELOADGUILDAGIT = 229;
        public const int ISM_LM_WHISPER = 230;
        public const int ISM_GMWHISPER = 231;
        public const int ISM_LM_LOGIN = 232;
        public const int ISM_LM_LOGOUT = 233;
        public const int ISM_REQUEST_RECALL = 234;
        public const int ISM_RECALL = 235;
        public const int ISM_LM_LOGIN_REPLY = 236;
        public const int ISM_LM_KILLED_MSG = 237;
        public const int ISM_REQUEST_LOVERRECALL = 238;
        public const int ISM_STANDARDTICKREQ = 239;
        public const int ISM_STANDARDTICK = 240;
        public const int ISM_GUILDWAR = 241;


        public const int OS_EVENTOBJECT = 1;
        public const int OS_MOVINGOBJECT = 2;
        public const int OS_ITEMOBJECT = 3;
        public const int OS_GATEOBJECT = 4;
        public const int OS_MAPEVENT = 5;
        public const int OS_DOOR = 6;
        public const int OS_ROON = 7;
        /// <summary>
        /// 火球术
        /// </summary>
        public const int SKILL_FIREBALL = 1;
        /// <summary>
        /// 大火球
        /// </summary>
        public const int SKILL_HEALLING = 2;
        public const int SKILL_ONESWORD = 3;
        /// <summary>
        /// 基本剑法
        /// </summary>
        public const int SKILL_ILKWANG = 4;
        /// <summary>
        /// 未知
        /// </summary>
        public const int SKILL_FIREBALL2 = 5;
        /// <summary>
        /// 施毒术
        /// </summary>
        public const int SKILL_AMYOUNSUL = 6;
        /// <summary>
        /// 攻杀剑法
        /// </summary>
        public const int SKILL_YEDO = 7;
        /// <summary>
        /// 抗拒火环
        /// </summary>
        public const int SKILL_FIREWIND = 8;
        /// <summary>
        /// 地狱火
        /// </summary>
        public const int SKILL_FIRE = 9;
        /// <summary>
        /// 疾光电影
        /// </summary>
        public const int SKILL_SHOOTLIGHTEN = 10;
        /// <summary>
        /// 雷电术
        /// </summary>
        public const int SKILL_LIGHTENING = 11;
        /// <summary>
        /// 刺杀剑法
        /// </summary>
        public const int SKILL_ERGUM = 12;
        /// <summary>
        /// 灵魂火符
        /// </summary>
        public const int SKILL_FIRECHARM = 13;
        /// <summary>
        /// 幽灵盾
        /// </summary>
        public const int SKILL_HANGMAJINBUB = 14;
        /// <summary>
        /// 神圣战甲术
        /// </summary>
        public const int SKILL_DEJIWONHO = 15;
        /// <summary>
        /// 捆魔咒
        /// </summary>
        public const int SKILL_HOLYSHIELD = 16;
        /// <summary>
        /// 召唤骷髅
        /// </summary>
        public const int SKILL_SKELLETON = 17;
        /// <summary>
        /// 隐身术
        /// </summary>
        public const int SKILL_CLOAK = 18;
        /// <summary>
        /// 集体隐身术
        /// </summary>
        public const int SKILL_BIGCLOAK = 19;
        /// <summary>
        /// 诱惑之光
        /// </summary>
        public const int SKILL_TAMMING = 20;
        /// <summary>
        /// 瞬息移动
        /// </summary>
        public const int SKILL_SPACEMOVE = 21;
        /// <summary>
        /// 火墙
        /// </summary>
        public const int SKILL_EARTHFIRE = 22;
        /// <summary>
        /// 爆裂火焰
        /// </summary>
        public const int SKILL_FIREBOOM = 23;
        /// <summary>
        /// 地狱雷光
        /// </summary>
        public const int SKILL_LIGHTFLOWER = 24;
        /// <summary>
        /// 半月弯刀
        /// </summary>
        public const int SKILL_BANWOL = 25;
        /// <summary>
        /// 烈火剑法
        /// </summary>
        public const int SKILL_FIRESWORD = 26;
        /// <summary>
        /// 野蛮冲撞
        /// </summary>
        public const int SKILL_MOOTEBO = 27;
        /// <summary>
        /// 心灵启示
        /// </summary>
        public const int SKILL_SHOWHP = 28;
        /// <summary>
        /// 群体治疗术
        /// </summary>
        public const int SKILL_BIGHEALLING = 29;
        /// <summary>
        /// 召唤神兽
        /// </summary>
        public const int SKILL_SINSU = 30;
        /// <summary>
        /// 魔法盾
        /// </summary>
        public const int SKILL_SHIELD = 31;
        /// <summary>
        /// 圣言术
        /// </summary>
        public const int SKILL_KILLUNDEAD = 32;
        /// <summary>
        /// 冰咆哮
        /// </summary>
        public const int SKILL_SNOWWIND = 33;
        /// <summary>
        /// 解毒术
        /// </summary>
        public const int SKILL_UNAMYOUNSUL = 40;
        // Purification
        public const int SKILL_WINDTEBO = 35;
        /// <summary>
        /// 冰焰
        /// </summary>
        public const int SKILL_MABE = 50;
        /// <summary>
        /// 群体雷电术
        /// </summary>
        public const int SKILL_GROUPLIGHTENING = 42;
        /// <summary>
        /// 群体施毒术
        /// </summary>
        public const int SKILL_GROUPAMYOUNSUL = 48;
        /// <summary>
        /// 地钉
        /// </summary>
        public const int SKILL_GROUPDEDING = 39;
        public const int SKILL_CROSSMOON = 34;
        // CHM
        public const int SKILL_ANGEL = 41;
        public const int SKILL_TWINBLADE = 38;
        public const int SKILL_43 = 43;
        public const int SKILL_44 = 44;
        /// <summary>
        /// 灭天火
        /// </summary>
        public const int SKILL_45 = 45;
        /// <summary>
        /// 分身术
        /// </summary>
        public const int SKILL_46 = 46;
        /// <summary>
        /// 火龙气焰
        /// </summary>
        public const int SKILL_47 = 47;
        /// <summary>
        /// 气功波
        /// </summary>
        public const int SKILL_ENERGYREPULSOR = 37;
        /// <summary>
        /// 净化术
        /// </summary>
        public const int SKILL_49 = 49;
        /// <summary>
        /// 无极真气
        /// </summary>
        public const int SKILL_UENHANCER = 36;
        public const int SKILL_51 = 51;
        public const int SKILL_52 = 52;
        public const int SKILL_53 = 53;
        public const int SKILL_54 = 54;
        public const int SKILL_55 = 55;
        public const int SKILL_REDBANWOL = 56;
        public const int SKILL_57 = 57;
        public const int SKILL_58 = 58;
        public const int SKILL_59 = 59;
        
        public const int LA_UNDEAD = 1;
        public const string sENCYPTSCRIPTFLAG = "不知道是什么字符串";
        public const string sSTATUS_FAIL = "+FAIL/";
        public const string sSTATUS_GOOD = "+GOOD/";

        public static TDefaultMessage MakeDefaultMsg(int msg, int Recog, int param, int tag, int series)
        {
            var result = new TDefaultMessage();
            result.Ident = (ushort)msg;
            result.Param = (ushort)param;
            result.Tag = (ushort)tag;
            result.Series = (ushort)series;
            result.Recog = Recog;
            return result;
        }

        public static int MakeMonsterFeature(byte btRaceImg, byte btWeapon, ushort wAppr)
        {
            int result;
            result = HUtil32.MakeLong(HUtil32.MakeWord(btRaceImg, btWeapon), wAppr);
            return result;
        }

        public static int MakeHumanFeature(byte btRaceImg, byte btDress, byte btWeapon, byte btHair)
        {
            return HUtil32.MakeLong(HUtil32.MakeWord(btRaceImg, btWeapon), HUtil32.MakeWord(btHair, btDress));
        }
    }
}

