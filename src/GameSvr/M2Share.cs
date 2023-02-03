using GameSvr.Actor;
using GameSvr.Castle;
using GameSvr.Conf;
using GameSvr.Conf.Model;
using GameSvr.DataSource;
using GameSvr.Event;
using GameSvr.GameGate;
using GameSvr.Guild;
using GameSvr.Items;
using GameSvr.Magic;
using GameSvr.Maps;
using GameSvr.Notices;
using GameSvr.Npc;
using GameSvr.Robots;
using GameSvr.Script;
using GameSvr.Services;
using GameSvr.World;
using NLog;
using System.Collections;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr
{
    public static class M2Share
    {
        public static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 服务端启动路径
        /// </summary>
        public static readonly string BasePath;
        /// <summary>
        /// 服务器编号
        /// </summary>
        public static byte ServerIndex = 0;
        /// <summary>
        /// 服务器启动时间
        /// </summary>
        public static int StartTick = 0;
        public static int ShareFileNameNum = 0;
        public static int ServerTickDifference = 0;
        public static readonly ReaderWriterLockWrapper SyncLock = new ReaderWriterLockWrapper();
        public static readonly ActorMgr ActorMgr = null;
        public static readonly ServerConf ServerConf;
        public static readonly GameSvrConf Config;
        private static readonly StringConf StringConf;
        private static readonly ExpsConf ExpConf;
        private static readonly GlobalConf GlobalConf;
        private static readonly GameSettingConf GameSetting;
        /// <summary>
        /// 寻路
        /// </summary>
        public static readonly FindPath FindPath;
        /// <summary>
        /// 地图对象管理
        /// </summary>
        public static readonly CellObjectMgr CellObjectMgr;
        public static readonly LocalDB LocalDb;
        public static readonly CommonDB CommonDb;
        public static readonly RandomNumber RandomNumber;
        public static DBService DataServer = null;
        public static ScriptSystem ScriptSystem = null;
        public static GameGateMgr GateMgr = null;
        public static GameEventSource EventSource;
        public static MapManager MapMgr = null;
        public static CustomItem CustomItemMgr = null;
        public static MagicManager MagicMgr = null;
        public static NoticeManager NoticeMgr = null;
        public static GuildManager GuildMgr = null;
        public static EventManager EventMgr = null;
        public static CastleManager CastleMgr = null;
        public static TFrontEngine FrontEngine = null;
        public static WorldServer WorldEngine = null;
        public static RobotManage RobotMgr = null;
        public static NormNpc ManageNPC = null;
        public static NormNpc RobotNPC = null;
        public static Merchant FunctionNPC = null;
        public static int HighLevelHuman;
        public static int HighPKPointHuman;
        public static int HighDCHuman;
        public static int HighMCHuman;
        public static int HighSCHuman;
        public static int HighOnlineHuman;
        public static Dictionary<string, IList<MakeItem>> MakeItemList = null;
        public static IList<StartPoint> StartPointList = null;
        public static TRouteInfo[] ServerTableList = null;
        public static IList<IList<TQDDinfo>> QuestDiaryList = null;
        public static StringList AbuseTextList = null;
        public static ConcurrentDictionary<string, long> DenySayMsgList = null;
        public static ConcurrentDictionary<string, int> MiniMapList = null;
        public static IList<DealOffInfo> SellOffItemList = null;
        public static ArrayList LogonCostLogList = null;
        /// <summary>
        /// 解包物品列表
        /// </summary>
        public static Dictionary<int, string> UnbindList = null;
        /// <summary>
        /// 游戏变量列表
        /// </summary>
        public static Dictionary<string, DynamicVar> DynamicVarList = null;
        /// <summary>
        /// 游戏公告列表
        /// </summary>
        public static IList<string> LineNoticeList = null;
        /// <summary>
        /// 怪物说话信息列表
        /// </summary>
        public static Dictionary<string, IList<MonsterSayMsg>> MonSayMsgList = null;
        /// <summary>
        /// /禁止制造物品列表
        /// </summary>
        public static IList<string> DisableMakeItemList = null;
        /// <summary>
        /// 禁止制造物品列表
        /// </summary>
        public static IList<string> EnableMakeItemList = null;
        /// <summary>
        /// 禁止出售物品列表
        /// </summary>
        public static IList<string> DisableSellOffList = null;
        /// <summary>
        /// 禁止移动地图列表
        /// </summary>
        public static StringList DisableMoveMapList = null;
        /// <summary>
        /// 禁止发信息名称列表
        /// </summary>
        public static IList<string> DisableSendMsgList = null;
        /// <summary>
        /// 怪物爆物品限制
        /// </summary>
        public static ConcurrentDictionary<string, MonsterLimitDrop> MonDropLimitLIst = null;
        /// <summary>
        /// 禁止取下物品列表
        /// </summary>
        public static Dictionary<int, string> DisableTakeOffList = null;
        public static IList<ItemBind> ItemBindIPaddr = null;
        public static IList<ItemBind> ItemBindDieNoDropName = null;
        public static IList<ItemBind> ItemBindAccount = null;
        public static IList<ItemBind> ItemBindChrName = null;
        /// <summary>
        /// 出师记录表
        /// </summary>
        public static IList<string> UnMasterList = null;
        /// <summary>
        /// 强行出师记录表
        /// </summary>
        public static IList<string> UnForceMasterList = null;
        /// <summary>
        /// 游戏日志物品名
        /// </summary>
        public static IList<string> GameLogItemNameList = null;
        public static bool GameLogGold = true;
        public static bool GameLogGameGold = true;
        public static bool GameLogGamePoint = true;
        public static bool GameLogHumanDie = true;
        /// <summary>
        /// IP过滤列表
        /// </summary>
        public static IList<string> DenyIPAddrList = null;
        /// <summary>
        /// 角色过滤列表
        /// </summary>        
        public static IList<string> DenyChrNameList = null;
        /// <summary>
        /// 登录帐号过滤列表
        /// </summary>
        public static IList<string> DenyAccountList = null;
        /// <summary>
        /// 不清除怪物列表
        /// </summary>
        public static IList<string> NoClearMonLIst = null;
        /// <summary>
        /// 不清除怪物列表
        /// </summary>
        public static IList<string> NoHptoexpMonLIst = null;
        public static object ProcessMsgCriticalSection = null;
        public static object UserDBCriticalSection = null;
        public static object ProcessHumanCriticalSection = null;
        public static int TotalHumCount = 0;
        public static bool BoMission = false;
        public static string MissionMap = string.Empty;
        public static short MissionX = 0;
        public static short MissionY = 0;
        public static bool StartReady = false;
        public static bool FilterWord = false;
        public static int SockCountMin = 0;
        public static int SockCountMax = 0;
        public static int HumCountMin = 0;
        public static int HumCountMax = 0;
        public static int UsrRotCountMin = 0;
        public static int UsrRotCountMax = 0;
        public static int UsrRotCountTick = 0;
        public static int ProcessHumanLoopTime = 0;
        public static int HumLimit = 30;
        public static int MonLimit = 30;
        public static int ZenLimit = 5;
        public static int NpcLimit = 5;
        public static int SocLimit = 10;
        public static int SocCheckTimeOut = 50;
        public static int DecLimit = 20;
        public static byte GameTime = 0;
        public static char GMRedMsgCmd = '!';
        public static int GMREDMSGCMD = 6;
        public static int SendOnlineTick = 0;
        public static int SpiritMutinyTick = 0;
        public static int[] OldNeedExps = new int[Grobal2.MaxChangeLevel];
        public static int CurrentMerchantIndex = 0;
        public static readonly HashSet<byte> ItemDamageRevivalMap = new HashSet<byte>() { 114, 160, 161, 162 };
        public static readonly HashSet<byte> IsAccessoryMap = new HashSet<byte> { 19, 20, 21, 22, 23, 24, 26 };
        public static readonly HashSet<byte> StdModeMap = new HashSet<byte>() { 15, 19, 20, 21, 22, 23, 24, 26 };
        public static readonly HashSet<byte> RobotPlayRaceMap = new HashSet<byte>() { 55, 79, 109, 110, 111, 128, 143, 145, 147, 151, 153, 156 };

        public static string GetGoodTick => string.Format(Settings.sSTATUS_GOOD, HUtil32.GetTickCount());

        static M2Share()
        {
            BasePath = AppContext.BaseDirectory;
            ServerConf = new ServerConf(Path.Combine(BasePath, ConfConst.ConfigFileName));
            StringConf = new StringConf(Path.Combine(BasePath, ConfConst.StringFileName));
            ExpConf = new ExpsConf(Path.Combine(BasePath, ConfConst.ExpConfigFileName));
            GlobalConf = new GlobalConf(Path.Combine(BasePath, ConfConst.GlobalConfigFileName));
            GameSetting = new GameSettingConf(Path.Combine(BasePath, ConfConst.GameSettingFileName));
            Config = new GameSvrConf();
            RandomNumber = RandomNumber.GetInstance();
            ActorMgr = new ActorMgr();
            LocalDb = new LocalDB();
            CommonDb = new CommonDB();
            FindPath = new FindPath();
            CellObjectMgr = new CellObjectMgr();
        }

        public static bool LoadLineNotice(string FileName)
        {
            bool result = false;
            int i;
            string sText;
            if (File.Exists(FileName))
            {
                LineNoticeList.Clear();
                StringList LoadList = new StringList();
                LoadList.LoadFromFile(FileName);
                i = 0;
                while (true)
                {
                    if (LoadList.Count <= i)
                    {
                        break;
                    }
                    sText = LoadList[i].Trim();
                    if (string.IsNullOrEmpty(sText))
                    {
                        LoadList.RemoveAt(i);
                        continue;
                    }
                    LineNoticeList.Add(sText);
                    i++;
                }
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 随机获取其他服务器
        /// </summary>
        /// <returns></returns>
        public static bool GetMultiServerAddrPort(byte serverIndex, ref string sIPaddr, ref int nPort)
        {
            bool result = false;
            for (int i = 0; i < ServerTableList.Length; i++)
            {
                TRouteInfo routeInfo = ServerTableList[i];
                if (routeInfo == null)
                {
                    continue;
                }
                if (routeInfo.nGateCount <= 0)
                {
                    continue;
                }
                if (routeInfo.nServerIdx == serverIndex)
                {
                    sIPaddr = GetRandpmRoute(routeInfo, ref nPort);
                    result = true;
                    break;
                }
            }
            return result;
        }

        private static readonly Regex scriptRegex = new Regex("(?<=(<))[.\\s\\S]*?(?=(>))", RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.RightToLeft);

        public static MatchCollection MatchScriptLabel(string script)
        {
            return scriptRegex.Matches(script);
        }

        private static string GetRandpmRoute(TRouteInfo RouteInfo, ref int nGatePort)
        {
            int nC = RandomNumber.Random(RouteInfo.nGateCount);
            nGatePort = RouteInfo.nGameGatePort[nC];
            return RouteInfo.sGameGateIP[nC];
        }

        public static int GetExVersionNO(int nVersionDate, ref int nOldVerstionDate)
        {
            int result = 0;
            if (nVersionDate > 10000000)
            {
                while (nVersionDate > 10000000)
                {
                    nVersionDate -= 10000;
                    result += 100000000;
                }
            }
            nOldVerstionDate = nVersionDate;
            return result;
        }

        public static byte GetNextDirection(short sx, short sy, short dx, short dy)
        {
            short flagx;
            short flagy;
            byte result = Grobal2.DR_DOWN;
            if (sx < dx)
            {
                flagx = 1;
            }
            else if (sx == dx)
            {
                flagx = 0;
            }
            else
            {
                flagx = -1;
            }
            if (Math.Abs(sy - dy) > 2)
            {
                if (sx >= dx - 1 && sx <= dx + 1)
                {
                    flagx = 0;
                }
            }
            if (sy < dy)
            {
                flagy = 1;
            }
            else if (sy == dy)
            {
                flagy = 0;
            }
            else
            {
                flagy = -1;
            }
            if (Math.Abs(sx - dx) > 2)
            {
                if (sy > dy - 1 && sy <= dy + 1)
                {
                    flagy = 0;
                }
            }
            if (flagx == 0 && flagy == -1)
            {
                result = Grobal2.DR_UP;
            }
            if (flagx == 1 && flagy == -1)
            {
                result = Grobal2.DR_UPRIGHT;
            }
            if (flagx == 1 && flagy == 0)
            {
                result = Grobal2.DR_RIGHT;
            }
            if (flagx == 1 && flagy == 1)
            {
                result = Grobal2.DR_DOWNRIGHT;
            }
            if (flagx == 0 && flagy == 1)
            {
                result = Grobal2.DR_DOWN;
            }
            if (flagx == -1 && flagy == 1)
            {
                result = Grobal2.DR_DOWNLEFT;
            }
            if (flagx == -1 && flagy == 0)
            {
                result = Grobal2.DR_LEFT;
            }
            if (flagx == -1 && flagy == -1)
            {
                result = Grobal2.DR_UPLEFT;
            }
            return result;
        }

        public static bool CheckUserItems(int nIdx, Items.StdItem StdItem)
        {
            bool result = false;
            switch (nIdx)
            {
                case Grobal2.U_DRESS:
                    if (StdItem.StdMode == 10 || StdItem.StdMode == 11)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_WEAPON:
                    if (StdItem.StdMode == 5 || StdItem.StdMode == 6)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_RIGHTHAND:
                    if (StdItem.StdMode == 29 || StdItem.StdMode == 30 || StdItem.StdMode == 28)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_NECKLACE:
                    if (StdItem.StdMode == 19 || StdItem.StdMode == 20 || StdItem.StdMode == 21)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_HELMET:
                    if (StdItem.StdMode == 15)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_ARMRINGL:
                    if (StdItem.StdMode == 24 || StdItem.StdMode == 25 || StdItem.StdMode == 26)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_ARMRINGR:
                    if (StdItem.StdMode == 24 || StdItem.StdMode == 26)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_RINGL:
                case Grobal2.U_RINGR:
                    if (StdItem.StdMode == 22 || StdItem.StdMode == 23)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_BUJUK:
                    if (StdItem.StdMode == 25 || StdItem.StdMode == 51)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_BELT:
                    if (StdItem.StdMode == 54 || StdItem.StdMode == 64)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_BOOTS:
                    if (StdItem.StdMode == 52 || StdItem.StdMode == 62)
                    {
                        result = true;
                    }
                    break;
                case Grobal2.U_CHARM:
                    if (StdItem.StdMode == 53 || StdItem.StdMode == 63)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        public static DateTime AddDateTimeOfDay(DateTime DateTime, int nDay)
        {
            DateTime result = DateTime.Now;
            //if (nDay > 0)
            //{
            //    nDay -= 1;
            //    Year = DateTime.Year;
            //    Month = DateTime.Month;
            //    Day = DateTime.Day;
            //    while (true)
            //    {
            //        if (MonthDays[false][Month] >= (Day + nDay))
            //        {
            //            break;
            //        }
            //        nDay = (Day + nDay) - MonthDays[false][Month] - 1;
            //        Day = 1;
            //        if (Month <= 11)
            //        {
            //            Month ++;
            //            continue;
            //        }
            //        Month = 1;
            //        if (Year == 99)
            //        {
            //            Year = 2000;
            //            continue;
            //        }
            //        Year ++;
            //    }
            //    Day += nDay;
            //    result = new DateTime(Year, Month ,Day);
            //}
            //else
            //{
            //    result = DateTime;
            //}
            return result;
        }

        public static ushort GetGoldShape(int nGold)
        {
            ushort result = 112;
            if (nGold >= 30)
            {
                result = 113;
            }
            if (nGold >= 70)
            {
                result = 114;
            }
            if (nGold >= 300)
            {
                result = 115;
            }
            if (nGold >= 1000)
            {
                result = 116;
            }
            return result;
        }

        // 金币在地上显示的外形ID
        public static int GetRandomLook(int nBaseLook, int nRage)
        {
            return nBaseLook + RandomNumber.Random(nRage);
        }

        public static bool CheckGuildName(string sGuildName)
        {
            bool result = true;
            if (sGuildName.Length > Config.GuildNameLen)
            {
                result = false;
                return result;
            }
            for (int i = 0; i < sGuildName.Length - 1; i++)
            {
                if (sGuildName[i] < '0' || sGuildName[i] == '/' || sGuildName[i] == '\\' || sGuildName[i] == ':' || sGuildName[i] == '*' || sGuildName[i] == ' '
                    || sGuildName[i] == '\"' || sGuildName[i] == '\'' || sGuildName[i] == '<' || sGuildName[i] == '|' || sGuildName[i] == '?' || sGuildName[i] == '>')
                {
                    result = false;
                }
            }
            return result;
        }

        public static int GetItemNumber()
        {
            Config.ItemNumber++;
            if (Config.ItemNumber > int.MaxValue / 2 - 1)
            {
                Config.ItemNumber = 1;
            }
            return Config.ItemNumber + HUtil32.GetTickCount();
        }

        public static int GetItemNumberEx()
        {
            Config.ItemNumberEx++;
            if (Config.ItemNumberEx < int.MaxValue / 2)
            {
                Config.ItemNumberEx = int.MaxValue / 2;
            }
            if (Config.ItemNumberEx > int.MaxValue - 1)
            {
                Config.ItemNumberEx = int.MaxValue / 2;
            }
            return Config.ItemNumberEx + HUtil32.GetTickCount();
        }

        public static string FilterShowName(string sName)
        {
            string result = "";
            bool bo11 = false;
            if (string.IsNullOrEmpty(sName))
            {
                return sName;
            }
            for (int i = 0; i < sName.Length; i++)
            {
                if (sName[i] >= '0' && sName[i] <= '9' || sName[i] == '-')
                {
                    result = sName[..i];
                    bo11 = true;
                    break;
                }
            }
            if (!bo11)
            {
                result = sName;
            }
            return result;
        }

        /// <summary>
        /// 获取服务器变量
        /// </summary>
        /// <param name="sText"></param>
        /// <returns></returns>
        public static int GetValNameNo(string sText)
        {
            int result = -1;
            int nValNo;
            ReadOnlySpan<char> ValText = sText.AsSpan();
            if (sText.Length >= 2)
            {
                char valType = char.ToUpper(sText[0]);
                switch (valType)
                {
                    case 'P':
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.StrToInt(ValText.Slice(1, 2), -1);
                            if ((nValNo >= 0) && (nValNo < 100))
                            {
                                result = nValNo;
                            }
                        }
                        else
                        {
                            nValNo = HUtil32.StrToInt(sText[1].ToString(), -1);
                            if ((nValNo >= 0) && (nValNo < 10))
                            {
                                result = nValNo;
                            }
                        }
                        break;
                    case 'G':
                        if (sText.Length == 4)
                        {
                            nValNo = HUtil32.StrToInt(ValText.Slice(1, 3), -1);
                            if ((nValNo < 500) && (nValNo > 99))
                            {
                                result = nValNo + 700;
                            }
                        }
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.StrToInt(ValText.Slice(1, 2), -1);
                            if ((nValNo >= 0) && (nValNo < 100))
                            {
                                result = nValNo + 100;
                            }
                        }
                        else
                        {
                            nValNo = HUtil32.StrToInt(sText[1].ToString(), -1);
                            if ((nValNo >= 0) && (nValNo < 10))
                            {
                                result = nValNo + 100;
                            }
                        }
                        break;
                    case 'M':
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.StrToInt(ValText.Slice(1, 2), -1);
                            if ((nValNo >= 0) && (nValNo < 100))
                            {
                                result = nValNo + 300;
                            }
                        }
                        else
                        {
                            nValNo = HUtil32.StrToInt(sText[1].ToString(), -1);
                            if ((nValNo >= 0) && (nValNo < 10))
                            {
                                result = nValNo + 300;
                            }
                        }
                        break;
                    case 'I':
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.StrToInt(ValText.Slice(1, 2), -1);
                            if ((nValNo >= 0) && (nValNo < 100))
                            {
                                result = nValNo + 400;
                            }
                        }
                        else
                        {
                            nValNo = HUtil32.StrToInt(sText[1].ToString(), -1);
                            if ((nValNo >= 0) && (nValNo < 10))
                            {
                                result = nValNo + 400;
                            }
                        }
                        break;
                    case 'D':
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.StrToInt(ValText.Slice(1, 2), -1);
                            if ((nValNo >= 0) && (nValNo < 100))
                            {
                                result = nValNo + 200;
                            }
                        }
                        else
                        {
                            nValNo = HUtil32.StrToInt(sText[1].ToString(), -1);
                            if ((nValNo >= 0) && (nValNo < 10))
                            {
                                result = nValNo + 200;
                            }
                        }
                        break;
                    case 'N':
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.StrToInt(ValText.Slice(1, 2), -1);
                            if ((nValNo >= 0) && (nValNo < 100))
                            {
                                result = nValNo + 500;
                            }
                        }
                        else
                        {
                            nValNo = HUtil32.StrToInt(sText[1].ToString(), -1);
                            if ((nValNo >= 0) && (nValNo < 10))
                            {
                                result = nValNo + 500;
                            }
                        }
                        break;
                    case 'S':
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.StrToInt(ValText.Slice(2 - 1, 2), -1);
                            if ((nValNo >= 0) && (nValNo < 100))
                            {
                                result = nValNo + 600;
                            }
                        }
                        else
                        {
                            nValNo = HUtil32.StrToInt(sText[1].ToString(), -1);
                            if ((nValNo >= 0) && (nValNo < 10))
                            {
                                result = nValNo + 600;
                            }
                        }
                        break;
                    case 'A':
                        if (sText.Length == 4)
                        {
                            nValNo = HUtil32.StrToInt(ValText.Slice(1, 3), -1);
                            if ((nValNo < 500) && (nValNo > 99))
                            {
                                result = nValNo + 1100;
                            }
                        }
                        else
                        {
                            if (sText.Length == 3)
                            {
                                nValNo = HUtil32.StrToInt(ValText.Slice(1, 2), -1);
                                if ((nValNo >= 0) && (nValNo < 100))
                                {
                                    result = nValNo + 700;
                                }
                            }
                            else
                            {
                                nValNo = HUtil32.StrToInt(sText[1].ToString(), -1);
                                if ((nValNo >= 0) && (nValNo < 10))
                                {
                                    result = nValNo + 700;
                                }
                            }
                        }
                        break;
                    case 'T':
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.StrToInt(ValText.Slice(2 - 1, 3), -1);
                            if ((nValNo >= 0) && (nValNo < 100))
                            {
                                result = nValNo + 700;
                            }
                        }
                        else
                        {
                            nValNo = HUtil32.StrToInt(sText[1].ToString(), -1);
                            if ((nValNo >= 0) && (nValNo < 10))
                            {
                                result = nValNo + 700;
                            }
                        }
                        break;
                    case 'E':
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.StrToInt(ValText.Slice(2 - 1, 2), -1);
                            if ((nValNo >= 0) && (nValNo < 100))
                            {
                                result = nValNo + 1600;
                            }
                        }
                        else
                        {
                            nValNo = HUtil32.StrToInt(sText[1].ToString(), -1);
                            if ((nValNo >= 0) && (nValNo < 10))
                            {
                                result = nValNo + 1600;
                            }
                        }
                        break;
                    case 'W':
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.StrToInt(ValText.Slice(2 - 1, 2), -1);
                            if ((nValNo >= 0) && (nValNo < 100))
                            {
                                result = nValNo + 1700;
                            }
                        }
                        else
                        {
                            nValNo = HUtil32.StrToInt(sText[1].ToString(), -1);
                            if ((nValNo >= 0) && (nValNo < 10))
                            {
                                result = nValNo + 1700;
                            }
                        }
                        break;
                }
            }
            return result;
        }

        public static bool IsAccessory(ushort nIndex)
        {
            bool result;
            StdItem item = WorldEngine.GetStdItem(nIndex);
            if (IsAccessoryMap.Contains(item.StdMode))
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public static IList<MakeItem> GetMakeItemInfo(string sItemName)
        {
            if (MakeItemList.TryGetValue(sItemName, out IList<MakeItem> itemList))
            {
                return itemList;
            }
            return null;
        }

        public static string GetStartPointInfo(int nIndex, ref short nX, ref short nY)
        {
            string result = string.Empty;
            nX = 0;
            nY = 0;
            if (nIndex >= 0 && nIndex < StartPointList.Count)
            {
                StartPoint StartPoint = StartPointList[nIndex];
                if (StartPoint != null)
                {
                    nX = StartPoint.m_nCurrX;
                    nY = StartPoint.m_nCurrY;
                    result = StartPoint.m_sMapName;
                }
            }
            return result;
        }

        public static void AddLogonCostLog(string sMsg)
        {
            LogonCostLogList.Add(sMsg);
        }

        public static void TrimStringList(StringList sList)
        {
            int n8;
            string sC;
            n8 = 0;
            while (true)
            {
                if (sList.Count <= n8)
                {
                    break;
                }
                sC = sList[n8].Trim();
                if (sC == "")
                {
                    sList.RemoveAt(n8);
                    continue;
                }
                n8++;
            }
        }

        public static bool CanMakeItem(string sItemName)
        {
            bool result;
            result = false;
            //g_DisableMakeItemList.__Lock();
            //try {
            //    for (I = 0; I < g_DisableMakeItemList.Count; I ++ )
            //    {
            //        if ((g_DisableMakeItemList[I]).CompareTo((sItemName)) == 0)
            //        {
            //            result = false;
            //            return result;
            //        }
            //    }
            //} finally {
            //    g_DisableMakeItemList.UnLock();
            //}
            //g_EnableMakeItemList.__Lock();
            //try {
            //    for (I = 0; I < g_EnableMakeItemList.Count; I ++ )
            //    {
            //        if ((g_EnableMakeItemList[I]).CompareTo((sItemName)) == 0)
            //        {
            //            result = true;
            //            break;
            //        }
            //    }
            //} finally {
            //    g_EnableMakeItemList.UnLock();
            //}
            return result;
        }

        public static bool CanMoveMap(string sMapName)
        {
            bool result;
            int I;
            result = true;
            try
            {
                for (I = 0; I < DisableMoveMapList.Count; I++)
                {
                    //if ((g_DisableMoveMapList[I]).CompareTo((sMapName)) == 0)
                    //{
                    //    result = false;
                    //    break;
                    //}
                }
            }
            finally
            {
            }
            return result;
        }

        public static bool CanSellItem(string sItemName)
        {
            bool result;
            int i;
            result = true;
            try
            {
                for (i = 0; i < DisableSellOffList.Count; i++)
                {
                    //if ((g_DisableSellOffList[i]).CompareTo((sItemName)) == 0)
                    //{
                    //    result = false;
                    //    break;
                    //}
                }
            }
            finally
            {
            }
            return result;
        }

        public static bool LoadItemBindIPaddr()
        {
            StringList LoadList = null;
            string sMakeIndex = string.Empty;
            string sItemIndex = string.Empty;
            string sBindName = string.Empty;
            bool result = false;
            string sFileName = M2Share.GetEnvirFilePath("ItemBindIPaddr.txt");
            if (File.Exists(sFileName))
            {
                LoadList = new StringList();
                try
                {
                    for (int i = 0; i < ItemBindIPaddr.Count; i++)
                    {
                        ItemBindIPaddr[i] = null;
                    }
                    ItemBindIPaddr.Clear();
                    LoadList.LoadFromFile(sFileName);
                    for (int i = 0; i < LoadList.Count; i++)
                    {
                        string sLineText = LoadList[i].Trim();
                        if (sLineText[0] == ';')
                        {
                            continue;
                        }
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sItemIndex, HUtil32.Separator);
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sMakeIndex, HUtil32.Separator);
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sBindName, HUtil32.Separator);
                        int nMakeIndex = HUtil32.StrToInt(sMakeIndex, -1);
                        int nItemIndex = HUtil32.StrToInt(sItemIndex, -1);
                        if ((nMakeIndex > 0) && (nItemIndex > 0) && (sBindName != ""))
                        {
                            ItemBind ItemBind = new ItemBind();
                            ItemBind.nMakeIdex = nMakeIndex;
                            ItemBind.nItemIdx = nItemIndex;
                            ItemBind.sBindName = sBindName;
                            ItemBindIPaddr.Add(ItemBind);
                        }
                    }
                }
                finally
                {
                }
                result = true;
            }
            else
            {
                LoadList.SaveToFile(sFileName);
            }
            return result;
        }

        public static bool SaveItemBindIPaddr()
        {
            bool result;
            string sFileName = GetEnvirFilePath("ItemBindIPaddr.txt");
            //SaveList = new StringList();
            //try {
            //    for (I = 0; I < g_ItemBindIPaddr.Count; I++)
            //    {
            //        ItemBind = g_ItemBindIPaddr[I];
            //        SaveList.Add((ItemBind.nItemIdx).ToString() + "\t" + (ItemBind.nMakeIdex).ToString() + "\t" + ItemBind.sBindName);
            //    }
            //} finally {
            //}
            //SaveList.SaveToFile(sFileName);
            //SaveList.Free;
            result = true;
            return result;
        }

        public static bool LoadItemBindAccount()
        {
            StringList LoadList = null;
            string sMakeIndex = string.Empty;
            string sItemIndex = string.Empty;
            string sBindName = string.Empty;
            bool result = false;
            string sFileName = GetEnvirFilePath("ItemBindAccount.txt");
            if (File.Exists(sFileName))
            {
                LoadList = new StringList();
                try
                {
                    for (int i = 0; i < ItemBindAccount.Count; i++)
                    {
                        ItemBindAccount[i] = null;
                    }
                    ItemBindAccount.Clear();
                    LoadList.LoadFromFile(sFileName);
                    for (int i = 0; i < LoadList.Count; i++)
                    {
                        string sLineText = LoadList[i].Trim();
                        if (sLineText[0] == ';')
                        {
                            continue;
                        }
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sItemIndex, HUtil32.Separator);
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sMakeIndex, HUtil32.Separator);
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sBindName, HUtil32.Separator);
                        int nMakeIndex = HUtil32.StrToInt(sMakeIndex, -1);
                        int nItemIndex = HUtil32.StrToInt(sItemIndex, -1);
                        if ((nMakeIndex > 0) && (nItemIndex > 0) && (sBindName != ""))
                        {
                            ItemBind ItemBind = new ItemBind();
                            ItemBind.nMakeIdex = nMakeIndex;
                            ItemBind.nItemIdx = nItemIndex;
                            ItemBind.sBindName = sBindName;
                            ItemBindAccount.Add(ItemBind);
                        }
                    }
                }
                finally
                {
                }
                result = true;
            }
            else
            {
                LoadList.SaveToFile(sFileName);
            }
            return result;
        }

        public static bool SaveItemBindAccount()
        {
            string sFileName = M2Share.GetEnvirFilePath("ItemBindAccount.txt");
            //SaveList = new StringList();
            //g_ItemBindAccount.__Lock();
            //try {
            //    for (I = 0; I < g_ItemBindAccount.Count; I ++ )
            //    {
            //        ItemBind = g_ItemBindAccount[I];
            //        SaveList.Add((ItemBind.nItemIdx).ToString() + "\t" + (ItemBind.nMakeIdex).ToString() + "\t" + ItemBind.sBindName);
            //    }
            //} finally {
            //    g_ItemBindAccount.UnLock();
            //}

            //SaveList.SaveToFile(sFileName);

            //SaveList.Free;
            bool result = true;
            return result;
        }

        public static bool LoadItemBindChrName()
        {
            string sMakeIndex = string.Empty;
            string sItemIndex = string.Empty;
            string sBindName = string.Empty;
            bool result = false;
            string sFileName = GetEnvirFilePath("ItemBindChrName.txt");
            StringList LoadList = null;
            if (File.Exists(sFileName))
            {
                LoadList = new StringList();
                for (int I = 0; I < ItemBindChrName.Count; I++)
                {
                    ItemBindChrName[I] = null;
                }
                ItemBindChrName.Clear();
                LoadList.LoadFromFile(sFileName);
                for (int i = 0; i < LoadList.Count; i++)
                {
                    string sLineText = LoadList[i].Trim();
                    if (sLineText[0] == ';')
                    {
                        continue;
                    }
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sItemIndex, HUtil32.Separator);
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sMakeIndex, HUtil32.Separator);
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sBindName, HUtil32.Separator);
                    int nMakeIndex = HUtil32.StrToInt(sMakeIndex, -1);
                    int nItemIndex = HUtil32.StrToInt(sItemIndex, -1);
                    if ((nMakeIndex > 0) && (nItemIndex > 0) && (sBindName != ""))
                    {
                        ItemBind ItemBind = new ItemBind();
                        ItemBind.nMakeIdex = nMakeIndex;
                        ItemBind.nItemIdx = nItemIndex;
                        ItemBind.sBindName = sBindName;
                        ItemBindChrName.Add(ItemBind);
                    }
                }
                result = true;
            }
            else
            {
                LoadList.SaveToFile(sFileName);
            }
            return result;
        }

        public static bool SaveItemBindChrName()
        {
            bool result = false;
            string sFileName = GetEnvirFilePath("ItemBindChrName.txt");
            //g_ItemBindChrName.__Lock();
            //try {
            //    for (I = 0; I < g_ItemBindChrName.Count; I++)
            //    {
            //        ItemBind = g_ItemBindChrName[I];
            //        SaveList.Add((ItemBind.nItemIdx).ToString() + "\t" + (ItemBind.nMakeIdex).ToString() + "\t" + ItemBind.sBindName);
            //    }
            //} finally {
            //    g_ItemBindChrName.UnLock();
            //}
            //SaveList.SaveToFile(sFileName);
            ////SaveList.Free;
            //result = true;
            return result;
        }

        public static bool LoadDisableMakeItem()
        {
            bool result = false;
            string sFileName = GetEnvirFilePath("DisableMakeItem.txt");
            ArrayList LoadList = new ArrayList();
            //if (File.Exists(sFileName))
            //{
            //    g_DisableMakeItemList.__Lock();
            //    try {
            //        g_DisableMakeItemList.Clear();
            //        LoadList.LoadFromFile(sFileName);
            //        for (I = 0; I < LoadList.Count; I ++ )
            //        {
            //            g_DisableMakeItemList.Add(LoadList[I].Trim());
            //        }
            //    } finally {
            //        g_DisableMakeItemList.UnLock();
            //    }
            //    result = true;
            //}
            //else
            //{
            //    LoadList.SaveToFile(sFileName);
            //}
            //LoadList.Free;
            return result;
        }

        public static bool SaveDisableMakeItem()
        {
            string sFileName = GetEnvirFilePath("DisableMakeItem.txt");
            //g_DisableMakeItemList.SaveToFile(sFileName);
            return true;
        }

        public static bool LoadUnMasterList()
        {
            bool result = false;
            string sFileName = GetEnvirFilePath("UnMaster.txt");
            ArrayList LoadList = new ArrayList();
            //if (File.Exists(sFileName))
            //{
            //    g_UnMasterList.__Lock();
            //    try {
            //        g_UnMasterList.Clear();

            //        LoadList.LoadFromFile(sFileName);
            //        for (I = 0; I < LoadList.Count; I ++ )
            //        {
            //            g_UnMasterList.Add(LoadList[I].Trim());
            //        }
            //    } finally {
            //        g_UnMasterList.UnLock();
            //    }
            //    result = true;
            //}
            //else
            //{
            //    LoadList.SaveToFile(sFileName);
            //}
            //LoadList.Free;
            return result;
        }

        public static bool SaveUnMasterList()
        {
            string sFileName = GetEnvirFilePath("UnMaster.txt");
            //g_UnMasterList.SaveToFile(sFileName);
            return true;
        }

        public static bool LoadUnForceMasterList()
        {
            bool result = false;
            string sFileName = GetEnvirFilePath("UnForceMaster.txt");
            StringList LoadList = null;
            if (File.Exists(sFileName))
            {
                UnForceMasterList.Clear();
                LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (int i = 0; i < LoadList.Count; i++)
                {
                    UnForceMasterList.Add(LoadList[i].Trim());
                }
                result = true;
            }
            else
            {
                LoadList.SaveToFile(sFileName);
            }
            return result;
        }

        public static bool SaveUnForceMasterList()
        {
            string sFileName = GetEnvirFilePath("UnForceMaster.txt");
            //g_UnForceMasterList.SaveToFile(sFileName);
            return true;
        }

        public static bool LoadEnableMakeItem()
        {
            bool result = false;
            string sFileName = GetEnvirFilePath("EnableMakeItem.txt");
            //if (File.Exists(sFileName))
            //{
            //    g_EnableMakeItemList.__Lock();
            //    try {
            //        g_EnableMakeItemList.Clear();
            //        LoadList.LoadFromFile(sFileName);
            //        for (I = 0; I < LoadList.Count; I++)
            //        {
            //            g_EnableMakeItemList.Add(LoadList[I].Trim());
            //        }
            //    } finally {
            //        g_EnableMakeItemList.UnLock();
            //    }
            //    result = true;
            //}
            //else
            //{
            //    LoadList.SaveToFile(sFileName);
            //}
            // LoadList.Free;
            return result;
        }

        public static bool SaveEnableMakeItem()
        {
            string sFileName = M2Share.GetEnvirFilePath("EnableMakeItem.txt");
            //g_EnableMakeItemList.SaveToFile(sFileName);
            return true;
        }

        public static bool LoadDisableMoveMap()
        {
            bool result = false;
            string sFileName = M2Share.GetEnvirFilePath("DisableMoveMap.txt");
            StringList LoadList = null;
            if (File.Exists(sFileName))
            {
                LoadList = new StringList();
                DisableMoveMapList.Clear();
                LoadList.LoadFromFile(sFileName);
                for (int i = 0; i < LoadList.Count; i++)
                {
                    DisableMoveMapList.Add(LoadList[i].Trim());
                }
                result = true;
            }
            else
            {
                LoadList.SaveToFile(sFileName);
            }
            return result;
        }

        public static bool SaveDisableMoveMap()
        {
            string sFileName = GetEnvirFilePath("DisableMoveMap.txt");
            DisableMoveMapList.SaveToFile(sFileName);
            return true;
        }

        public static bool LoadAllowSellOffItem()
        {
            bool result = false;
            string sFileName = GetEnvirFilePath("DisableSellOffItem.txt");
            StringList LoadList;
            if (File.Exists(sFileName))
            {
                try
                {
                    LoadList = new StringList();
                    DisableSellOffList.Clear();
                    LoadList.LoadFromFile(sFileName);
                    for (int i = 0; i < LoadList.Count; i++)
                    {
                        DisableSellOffList.Add(LoadList[i].Trim());
                    }
                }
                finally
                {
                }
                result = true;
            }
            else
            {
                LoadList = new StringList();
                LoadList.SaveToFile(sFileName);
            }
            return result;
        }

        public static bool SaveAllowSellOffItem()
        {
            string sFileName = GetEnvirFilePath("DisableSellOffItem.txt");
            //g_DisableSellOffList.SaveToFile(sFileName);
            return true;
        }

        public static bool SaveChatLog()
        {
            //if (File.Exists(sFileName))
            //{
            //    LoadList = new ArrayList();
            //    LoadList.LoadFromFile(sFileName);
            //    g_ChatLoggingList.Add(LoadList);
            //    //LoadList.Free;
            //}
            //else
            //{
            //    g_ChatLoggingList.__Lock();
            //}
            //try {
            //    g_ChatLoggingList.SaveToFile(sFileName);
            //} finally {
            //    g_ChatLoggingList.UnLock();
            //}
            return true;
        }

        public static int GetUseItemIdx(string sName)
        {
            int result = -1;
            if (string.Compare(sName, Settings.DRESSNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 0;
            }
            else if (string.Compare(sName, Settings.WEAPONNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 1;
            }
            else if (string.Compare(sName, Settings.RIGHTHANDNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 2;
            }
            else if (string.Compare(sName, Settings.NECKLACENAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 3;
            }
            else if (string.Compare(sName, Settings.HELMETNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 4;
            }
            else if (string.Compare(sName, Settings.ARMRINGLNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 5;
            }
            else if (string.Compare(sName, Settings.ARMRINGRNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 6;
            }
            else if (string.Compare(sName, Settings.RINGLNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 7;
            }
            else if (string.Compare(sName, Settings.RINGRNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 8;
            }
            else if (string.Compare(sName, Settings.BUJUKNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 9;
            }
            else if (string.Compare(sName, Settings.BELTNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 10;
            }
            else if (string.Compare(sName, Settings.BOOTSNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 11;
            }
            else if (string.Compare(sName, Settings.CHARMNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                result = 12;
            }
            return result;
        }

        public static string GetUseItemName(int nIndex)
        {
            string result = string.Empty;
            switch (nIndex)
            {
                case 0:
                    result = Settings.DRESSNAME;
                    break;
                case 1:
                    result = Settings.WEAPONNAME;
                    break;
                case 2:
                    result = Settings.RIGHTHANDNAME;
                    break;
                case 3:
                    result = Settings.NECKLACENAME;
                    break;
                case 4:
                    result = Settings.HELMETNAME;
                    break;
                case 5:
                    result = Settings.ARMRINGLNAME;
                    break;
                case 6:
                    result = Settings.ARMRINGRNAME;
                    break;
                case 7:
                    result = Settings.RINGLNAME;
                    break;
                case 8:
                    result = Settings.RINGRNAME;
                    break;
                case 9:
                    result = Settings.BUJUKNAME;
                    break;
                case 10:
                    result = Settings.BELTNAME;
                    break;
                case 11:
                    result = Settings.BOOTSNAME;
                    break;
                case 12:
                    result = Settings.CHARMNAME;
                    break;
            }
            return result;
        }

        public static bool LoadDisableSendMsgList()
        {
            bool result = false;
            string sFileName = M2Share.GetEnvirFilePath("DisableSendMsgList.txt");
            ArrayList LoadList = new ArrayList();
            //if (File.Exists(sFileName))
            //{
            //    g_DisableSendMsgList.Clear();
            //    LoadList.LoadFromFile(sFileName);
            //    for (I = 0; I < LoadList.Count; I ++ )
            //    {
            //        g_DisableSendMsgList.Add(LoadList[I].Trim());
            //    }
            //    result = true;
            //}
            //else
            //{
            //    LoadList.SaveToFile(sFileName);
            //}
            //LoadList.Free;
            return result;
        }

        public static bool LoadMonDropLimitList()
        {
            string sItemName = string.Empty;
            string sItemCount = string.Empty;
            bool result = false;
            string sFileName = GetEnvirFilePath("MonDropLimitList.txt");
            StringList LoadList = new StringList();
            if (File.Exists(sFileName))
            {
                MonDropLimitLIst.Clear();
                LoadList.LoadFromFile(sFileName);
                for (int i = 0; i < LoadList.Count; i++)
                {
                    string sLineText = LoadList[i].Trim();
                    if ((sLineText == "") || (sLineText[0] == ';'))
                    {
                        continue;
                    }
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sItemName, new[] { ' ', '/', ',', '\t' });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sItemCount, new[] { ' ', '/', ',', '\t' });
                    int nItemCount = HUtil32.StrToInt(sItemCount, -1);
                    if ((!string.IsNullOrEmpty(sItemName)) && (nItemCount >= 0))
                    {
                        MonsterLimitDrop MonDrop = new MonsterLimitDrop();
                        MonDrop.ItemName = sItemName;
                        MonDrop.DropCount = 0;
                        MonDrop.NoDropCount = 0;
                        MonDrop.CountLimit = nItemCount;
                        MonDropLimitLIst.TryAdd(sItemName, MonDrop);
                    }
                }
                result = true;
            }
            else
            {
                LoadList.SaveToFile(sFileName);
            }
            return result;
        }

        public static bool SaveMonDropLimitList()
        {
            string sFileName = M2Share.GetEnvirFilePath("MonDropLimitList.txt");
            StringList LoadList = new StringList();
            foreach (KeyValuePair<string, MonsterLimitDrop> item in MonDropLimitLIst)
            {
                MonsterLimitDrop monDrop = item.Value;
                string sLineText = monDrop.ItemName + "\t" + (monDrop.CountLimit).ToString();
                LoadList.Add(sLineText);
            }
            LoadList.SaveToFile(sFileName);
            return true;
        }

        public static bool LoadDisableTakeOffList()
        {
            string sItemName = string.Empty;
            string sItemIdx = string.Empty;
            bool result = false;
            string sFileName = GetEnvirFilePath("DisableTakeOffList.txt");
            StringList LoadList = new StringList();
            if (File.Exists(sFileName))
            {
                LoadList.LoadFromFile(sFileName);
                DisableTakeOffList.Clear();
                for (int i = 0; i < LoadList.Count; i++)
                {
                    string sLineText = LoadList[i].Trim();
                    if ((sLineText == "") || (sLineText[0] == ';'))
                    {
                        continue;
                    }
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sItemName, new[] { ' ', '/', ',', '\t' });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sItemIdx, new[] { ' ', '/', ',', '\t' });
                    int nItemIdx = HUtil32.StrToInt(sItemIdx, -1);
                    if ((!string.IsNullOrEmpty(sItemName)) && (nItemIdx >= 0))
                    {
                        DisableTakeOffList.Add(nItemIdx, sItemName);
                    }
                }
                result = true;
            }
            else
            {
                LoadList.SaveToFile(sFileName);
            }
            return result;
        }

        public static bool SaveDisableTakeOffList()
        {
            string sFileName = GetEnvirFilePath("DisableTakeOffList.txt");
            StringList LoadList = new StringList();
            foreach (KeyValuePair<int, string> item in DisableTakeOffList)
            {
                string sLineText = item.Value + "\t" + item.Key;
                LoadList.Add(sLineText);
            }
            LoadList.SaveToFile(sFileName);
            return true;
        }

        public static bool InDisableTakeOffList(int nItemIdx)
        {
            return DisableTakeOffList.ContainsKey(nItemIdx - 1);
        }

        public static void SaveDisableSendMsgList()
        {
            string sFileName = GetEnvirFilePath("DisableSendMsgList.txt");
            StringList LoadList = new StringList();
            for (int i = 0; i < DisableSendMsgList.Count; i++)
            {
                LoadList.Add(DisableSendMsgList[i]);
            }
            LoadList.SaveToFile(sFileName);
        }

        public static bool GetDisableSendMsgList(string sHumanName)
        {
            bool result = false;
            //for (I = 0; I < g_DisableSendMsgList.Count; I ++ )
            //{
            //    if ((sHumanName).CompareTo((g_DisableSendMsgList[I])) == 0)
            //    {
            //        result = true;
            //        break;
            //    }
            //}
            return result;
        }

        public static bool LoadGameLogItemNameList()
        {
            bool result = false;
            string sFileName = GetEnvirFilePath("GameLogItemNameList.txt");
            StringList LoadList = new StringList();
            if (File.Exists(sFileName))
            {
                GameLogItemNameList.Clear();
                LoadList.LoadFromFile(sFileName);
                if (LoadList.Count == 1 && LoadList[0].StartsWith("*"))
                {
                    GameLogItemNameList.Add("*");
                    return true;
                }
                for (int i = 0; i < LoadList.Count; i++)
                {
                    GameLogItemNameList.Add(LoadList[i].Trim());
                }
                result = true;
            }
            else
            {
                LoadList.SaveToFile(sFileName);
            }
            return result;
        }

        public static byte GetGameLogItemNameList(string sItemName)
        {
            byte result = 0;
            if (GameLogItemNameList.Count == 1 && GameLogItemNameList[0].StartsWith("*"))
            {
                return 1;
            }
            for (int i = 0; i < GameLogItemNameList.Count; i++)
            {
                if (string.Compare(sItemName, GameLogItemNameList[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = 1;
                    break;
                }
            }
            return result;
        }

        public static bool SaveGameLogItemNameList()
        {
            bool result;
            string sFileName = GetEnvirFilePath("GameLogItemNameList.txt");
            try
            {

                //g_GameLogItemNameList.SaveToFile(sFileName);
            }
            finally
            {
            }
            result = true;
            return result;
        }

        public static bool LoadDenyIPAddrList()
        {
            bool result = false;
            string sFileName = GetEnvirFilePath("DenyIPAddrList.txt");
            //if (File.Exists(sFileName))
            //{
            //    g_DenyIPAddrList.__Lock();
            //    try {
            //        g_DenyIPAddrList.Clear();
            //        LoadList.LoadFromFile(sFileName);
            //        for (I = 0; I < LoadList.Count; I ++ )
            //        {
            //            g_DenyIPAddrList.Add(LoadList[I].Trim());
            //        }
            //    } finally {
            //        g_DenyIPAddrList.UnLock();
            //    }
            //    result = true;
            //}
            //else
            //{
            //    LoadList.SaveToFile(sFileName);
            //}
            //LoadList.Free;
            return result;
        }

        public static bool GetDenyIPAddrList(string sIPaddr)
        {
            bool result = false;
            try
            {
                for (int i = 0; i < DenyIPAddrList.Count; i++)
                {
                    //if ((sIPaddr).CompareTo((g_DenyIPAddrList[I])) == 0)
                    //{
                    //    result = true;
                    //    break;
                    //}
                }
            }
            finally
            {
            }
            return result;
        }

        public static bool SaveDenyIPAddrList()
        {
            string sFileName = GetEnvirFilePath("DenyIPAddrList.txt");
            //SaveList = new StringList();
            //g_DenyIPAddrList.__Lock();
            //try {
            //    for (I = 0; I < g_DenyIPAddrList.Count; I ++ )
            //    {
            //        if (((int)g_DenyIPAddrList.Values[I]) != 0)
            //        {
            //            SaveList.Add(g_DenyIPAddrList[I]);
            //        }
            //    }
            //    SaveList.SaveToFile(sFileName);
            //} finally {
            //    g_DenyIPAddrList.UnLock();
            //}
            //SaveList.Free;
            bool result = true;
            return result;
        }

        public static bool LoadDenyChrNameList()
        {
            bool result = false;
            string sFileName = GetEnvirFilePath("DenyChrNameList.txt");
            //if (File.Exists(sFileName))
            //{
            //    g_DenyChrNameList.__Lock();
            //    try {
            //        g_DenyChrNameList.Clear();

            //        LoadList.LoadFromFile(sFileName);
            //        for (I = 0; I < LoadList.Count; I ++ )
            //        {
            //            g_DenyChrNameList.Add(LoadList[I].Trim());
            //        }
            //    } finally {
            //        g_DenyChrNameList.UnLock();
            //    }
            //    result = true;
            //}
            //else
            //{
            //    LoadList.SaveToFile(sFileName);
            //}
            //LoadList.Free;
            return result;
        }

        public static bool GetDenyChrNameList(string sChrName)
        {
            bool result = false;
            try
            {
                //for (I = 0; I < g_DenyChrNameList.Count; I ++ )
                //{
                //    if ((sChrName).CompareTo((g_DenyChrNameList[I])) == 0)
                //    {
                //        result = true;
                //        break;
                //    }
                //}
            }
            finally
            {
            }
            return result;
        }

        public static bool SaveDenyChrNameList()
        {
            string sFileName = GetEnvirFilePath("DenyChrNameList.txt");
            //SaveList = new StringList();
            //g_DenyChrNameList.__Lock();
            //try {
            //    for (I = 0; I < g_DenyChrNameList.Count; I ++ )
            //    {
            //        if (((int)g_DenyChrNameList.Values[I]) != 0)
            //        {
            //            SaveList.Add(g_DenyChrNameList[I]);
            //        }
            //    }
            //    SaveList.SaveToFile(sFileName);
            //} finally {
            //    g_DenyChrNameList.UnLock();
            //}
            //SaveList.Free;
            return true;
        }

        public static bool LoadDenyAccountList()
        {
            bool result = false;
            string sFileName = GetEnvirFilePath("DenyAccountList.txt");
            new ArrayList();
            if (File.Exists(sFileName))
            {
                try
                {
                    DenyAccountList.Clear();
                    //LoadList.LoadFromFile(sFileName);
                    //for (I = 0; I < LoadList.Count; I++)
                    //{
                    //    g_DenyAccountList.Add(LoadList[I].Trim());
                    //}
                }
                finally
                {
                }
                result = true;
            }
            else
            {
                //LoadList.SaveToFile(sFileName);
            }
            //LoadList.Free;
            return result;
        }

        public static bool GetDenyAccountList(string sAccount)
        {
            bool result = false;
            try
            {
                for (int I = 0; I < DenyAccountList.Count; I++)
                {
                    //if ((sAccount).CompareTo((g_DenyAccountList[I])) == 0)
                    //{
                    //    result = true;
                    //    break;
                    //}
                }
            }
            finally
            {
            }
            return result;
        }

        public static bool SaveDenyAccountList()
        {
            bool result;
            string sFileName = GetEnvirFilePath("DenyAccountList.txt");
            //SaveList = new StringList();
            //g_DenyAccountList.__Lock();
            //try {
            //    for (var I = 0; I < g_DenyAccountList.Count; I++)
            //    {
            //        if (((int)g_DenyAccountList.Values[I]) != 0)
            //        {
            //            SaveList.Add(g_DenyAccountList[I]);
            //        }
            //    }
            //    SaveList.SaveToFile(sFileName);
            //} finally {
            //    g_DenyAccountList.UnLock();
            //}
            //SaveList.Free;
            result = true;
            return result;
        }

        public static bool LoadNoClearMonList()
        {
            bool result = false;
            string sFileName = GetEnvirFilePath("NoClearMonList.txt");
            StringList LoadList = null;
            if (File.Exists(sFileName))
            {
                LoadList = new StringList();
                NoClearMonLIst.Clear();
                LoadList.LoadFromFile(sFileName);
                for (int i = 0; i < LoadList.Count; i++)
                {
                    NoClearMonLIst.Add(LoadList[i].Trim());
                }
                result = true;
            }
            if (LoadList != null)
            {
                LoadList.SaveToFile(sFileName);
            }
            return result;
        }

        public static bool GetNoHptoexpMonList(string sMonName)
        {
            bool result = false;
            try
            {
                //for (var i = 0; i < g_NoHptoexpMonLIst.Count; i++)
                //{
                //    if ((sMonName).CompareTo((g_NoHptoexpMonLIst[i])) == 0)
                //    {
                //        result = true;
                //        break;
                //    }
                //}
            }
            finally
            {
            }
            return result;
        }

        public static bool GetNoClearMonList(string sMonName)
        {
            bool result = false;
            //for (var I = 0; I < g_NoClearMonLIst.Count; I++)
            //{
            //    if ((sMonName).CompareTo((g_NoClearMonLIst[I])) == 0)
            //    {
            //        result = true;
            //        break;
            //    }
            //}
            return result;
        }

        public static bool SaveNoHptoexpMonList()
        {
            bool result;
            string sFileName = GetEnvirFilePath("NoHptoExpMonList.txt");
            //SaveList = new StringList();
            //g_NoHptoexpMonLIst.__Lock();
            //try {
            //    for (I = 0; I < g_NoHptoexpMonLIst.Count; I++)
            //    {
            //        SaveList.Add(g_NoHptoexpMonLIst[I]);
            //    }
            //    SaveList.SaveToFile(sFileName);
            //} finally {
            //    g_NoHptoexpMonLIst.UnLock();
            //}
            //SaveList.Free;
            result = true;
            return result;
        }

        public static bool SaveNoClearMonList()
        {
            bool result;
            int I;
            string sFileName;
            StringList SaveList;
            sFileName = GetEnvirFilePath("NoClearMonList.txt");
            SaveList = new StringList();
            try
            {
                for (I = 0; I < NoClearMonLIst.Count; I++)
                {
                    //SaveList.Add(g_NoClearMonLIst[I]);
                }
                SaveList.SaveToFile(sFileName);
            }
            finally
            {
            }
            // SaveList.Free;
            result = true;
            return result;
        }

        public static bool LoadMonSayMsg()
        {
            string sStatus = string.Empty;
            string sRate = string.Empty;
            string sColor = string.Empty;
            string sMonName = string.Empty;
            string sSayMsg = string.Empty;
            int nStatus;
            int nRate;
            int nColor;
            StringList LoadList;
            string sLineText;
            MonsterSayMsg MonSayMsg;
            bool result = false;
            string sFileName = GetEnvirFilePath("GenMsg.txt");
            if (File.Exists(sFileName))
            {
                MonSayMsgList.Clear();
                LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (int i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i].Trim();
                    if (sLineText != "" && sLineText[1] < ';')
                    {
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sStatus, new[] { ' ', '/', ',', '\t' });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sRate, new[] { ' ', '/', ',', '\t' });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sColor, new[] { ' ', '/', ',', '\t' });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sMonName, new[] { ' ', '/', ',', '\t' });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sSayMsg, new[] { ' ', '/', ',', '\t' });
                        if (sStatus != "" && sRate != "" && sColor != "" && sMonName != "" && sSayMsg != "")
                        {
                            nStatus = HUtil32.StrToInt(sStatus, -1);
                            nRate = HUtil32.StrToInt(sRate, -1);
                            nColor = HUtil32.StrToInt(sColor, -1);
                            if (nStatus >= 0 && nRate >= 0 && nColor >= 0)
                            {
                                MonSayMsg = new MonsterSayMsg();
                                switch (nStatus)
                                {
                                    case 0:
                                        MonSayMsg.State = MonStatus.KillHuman;
                                        break;
                                    case 1:
                                        MonSayMsg.State = MonStatus.UnderFire;
                                        break;
                                    case 2:
                                        MonSayMsg.State = MonStatus.Die;
                                        break;
                                    case 3:
                                        MonSayMsg.State = MonStatus.MonGen;
                                        break;
                                    default:
                                        MonSayMsg.State = MonStatus.UnderFire;
                                        break;
                                }
                                switch (nColor)
                                {
                                    case 0:
                                        MonSayMsg.Color = MsgColor.Red;
                                        break;
                                    case 1:
                                        MonSayMsg.Color = MsgColor.Green;
                                        break;
                                    case 2:
                                        MonSayMsg.Color = MsgColor.Blue;
                                        break;
                                    case 3:
                                        MonSayMsg.Color = MsgColor.White;
                                        break;
                                    default:
                                        MonSayMsg.Color = MsgColor.White;
                                        break;
                                }
                                MonSayMsg.nRate = nRate;
                                MonSayMsg.sSayMsg = sSayMsg;
                                //for (II = 0; II < g_MonSayMsgList.Count; II ++ )
                                //{
                                //    if ((g_MonSayMsgList[II]).CompareTo((sMonName)) == 0)
                                //    {

                                //        ((g_MonSayMsgList.Values[II]) as ArrayList).Add(MonSayMsg);
                                //        boSearch = true;
                                //        break;
                                //    }
                                //}
                                //if (!boSearch)
                                //{
                                //    MonSayList = new ArrayList();
                                //    MonSayList.Add(MonSayMsg);
                                //    g_MonSayMsgList.Add(sMonName, ((MonSayList) as Object));
                                //}
                            }
                        }
                    }
                }
                //LoadList.Free;
                result = true;
            }
            return result;
        }

        public static void LoadConfig()
        {
            ServerConf.LoadConfig();
            StringConf.LoadString();
            ExpConf.LoadConfig();
            GlobalConf.LoadConfig();
            GameSetting.LoadConfig();
        }

        public static string GetIPLocal(string sIPaddr)
        {
            return "未知!!!";
        }

        // 是否记录物品日志
        // 返回 FALSE 为记录
        // 返回 TRUE  为不记录
        public static bool IsCheapStuff(byte tByte)
        {
            bool result;
            if (tByte < 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        // sIPaddr 为当前IP
        // dIPaddr 为要比较的IP
        // * 号为通配符
        public static bool CompareIPaddr(string sIPaddr, string dIPaddr)
        {
            bool result = false;
            if (sIPaddr == "" || dIPaddr == "")
            {
                return result;
            }
            if (dIPaddr[1] == '*')
            {
                result = true;
                return result;
            }
            int nPos = dIPaddr.IndexOf('*');
            if (nPos > 0)
            {
                result = HUtil32.CompareLStr(sIPaddr, dIPaddr, nPos - 1);
            }
            else
            {
                result = sIPaddr.CompareTo(dIPaddr) == 0;
            }
            return result;
        }

        public static string GetEnvirFilePath(string filePath)
        {
            return Path.Combine(BasePath, Config.EnvirDir, filePath);
        }

        public static string GetEnvirFilePath(string dirPath, string filePath)
        {
            return Path.Combine(BasePath, Config.EnvirDir, dirPath, filePath);
        }

        public static string GetNoticeFilePath(string filePath)
        {
            return Path.Combine(BasePath, Config.NoticeDir, filePath);
        }
    }
}