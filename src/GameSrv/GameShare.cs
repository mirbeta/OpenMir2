using GameSrv.Robots;
using M2Server;
using M2Server.Castle;
using M2Server.Event;
using M2Server.Guild;
using M2Server.Items;
using M2Server.Maps.AutoPath;
using M2Server.Notices;
using M2Server.Npc;
using NLog;
using System.Text.RegularExpressions;
using GameSrv.DataSource;
using GameSrv.Maps;
using GameSrv.Network;
using GameSrv.Services;
using GameSrv.Word.Managers;
using GameSrv.Word.Threads;
using ScriptSystem;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSrv
{
    public static class GameShare
    {
        public static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 启动路径
        /// </summary>
        public static readonly string BasePath;
        /// <summary>
        /// 服务器编号
        /// </summary>
        public static byte ServerIndex = 0;
        /// <summary>
        /// 服务器启动时间
        /// </summary>
        public static long StartTime;
        public static readonly WordStatistics Statistics;
        public static readonly ActorMgr ActorMgr;
        /// <summary>
        /// 寻路
        /// </summary>
        public static readonly FindPath FindPath;
        /// <summary>
        /// 地图对象管理
        /// </summary>
        public static readonly CellObjectMgr CellObjectMgr;
        public static readonly LocalDb LocalDb;
        public static readonly CommonDB CommonDb;
        public static readonly RandomNumber RandomNumber;
        public static MapQuestManager QuestManager;
        public static DBService DataServer = null;
        public static MarketService MarketService = null;
        public static ChatChannelService ChatChannel = null;
        public static ThreadSocketMgr SocketMgr = null;
        public static GameEventSource EventSource;
        public static MapManager MapMgr = null;
        public static CustomItem CustomItemMgr = null;
        public static ScriptEngine ScriptEngine = null;
        public static ScriptParsers ScriptParsers;
        public static MarketManager MarketManager = null;
        public static NoticeManager NoticeMgr = null;
        public static GuildManager GuildMgr = null;
        public static EventManager EventMgr = null;
        public static CastleManager CastleMgr = null;
        public static FrontEngine FrontEngine = null;
        public static RobotManage RobotMgr = null;
        public static NormNpc ManageNPC = null;
        public static NormNpc RobotNPC = null;
        public static IMerchant FunctionNPC = null;
        public static NetworkMonitor NetworkMonitor;
        public static SystemProcessor SystemProcess;
        public static UserProcessor UserProcessor;
        public static RobotProcessor RobotProcessor;
        public static MerchantProcessor MerchantProcessor;
        public static GeneratorProcessor GeneratorProcessor;
        public static EventProcessor EventProcessor;
        public static StorageProcessor StorageProcessor;
        public static TimedRobotProcessor TimedRobotProcessor;
        public static int TotalHumCount = 0;
        public static bool BoMission = false;
        public static string MissionMap = string.Empty;
        public static short MissionX = 0;
        public static short MissionY = 0;
        public static bool StartReady = false;
        public static bool FilterWord = false;
        public static int HumLimit = 30;
        public static int MonLimit = 30;
        public static int ZenLimit = 5;
        public static int NpcLimit = 5;
        public static int SocLimit = 10;
        public static int SocCheckTimeOut = 50;
        public static int DecLimit = 20;
        public static byte GameTime = 0;
        public static char GMRedMsgCmd = '!';
        public static byte GMREDMSGCMD = 6;
        public static int SendOnlineTick = 0;
        public static int SpiritMutinyTick = 0;
        public static int[] OldNeedExps = new int[Grobal2.MaxChangeLevel];
        public static int CurrentMerchantIndex = 0;
        public static readonly HashSet<byte> ItemDamageRevivalMap = new HashSet<byte>() { 114, 160, 161, 162 };
        public static readonly HashSet<byte> IsAccessoryMap = new HashSet<byte> { 19, 20, 21, 22, 23, 24, 26 };
        public static readonly HashSet<byte> StdModeMap = new HashSet<byte>() { 15, 19, 20, 21, 22, 23, 24, 26 };
        public static readonly HashSet<byte> RobotPlayRaceMap = new HashSet<byte>() { 55, 79, 109, 110, 111, 128, 143, 145, 147, 151, 153, 156 };
        public static readonly ServerConf ServerConf;
        public static readonly GameSvrConf Config;
        private static readonly StringConf StringConf;
        private static readonly ExpsConf ExpConf;
        private static readonly GlobalConf GlobalConf;
        private static readonly GameSettingConf GameSetting;

        static GameShare()
        {
            BasePath = AppContext.BaseDirectory;
            ServerConf = new ServerConf(Path.Combine(BasePath, ConfConst.ServerFileName));
            StringConf = new StringConf(Path.Combine(BasePath, ConfConst.StringFileName));
            ExpConf = new ExpsConf(Path.Combine(BasePath, ConfConst.ExpConfigFileName));
            GlobalConf = new GlobalConf(Path.Combine(BasePath, ConfConst.GlobalConfigFileName));
            GameSetting = new GameSettingConf(Path.Combine(BasePath, ConfConst.GameSettingFileName));
            Config = new GameSvrConf();
            RandomNumber = RandomNumber.GetInstance();
            ScriptEngine = new ScriptEngine();
            ScriptParsers = new ScriptParsers();
            Statistics = new WordStatistics();
            ActorMgr = new ActorMgr();
            LocalDb = new LocalDb();
            CommonDb = new CommonDB();
            FindPath = new FindPath();
            CellObjectMgr = new CellObjectMgr();
            NetworkMonitor = new NetworkMonitor();
            SystemProcess = new SystemProcessor();
            UserProcessor = new UserProcessor();
            RobotProcessor = new RobotProcessor();
            MerchantProcessor = new MerchantProcessor();
            GeneratorProcessor = new GeneratorProcessor();
            EventProcessor = new EventProcessor();
            StorageProcessor = new StorageProcessor();
            TimedRobotProcessor = new TimedRobotProcessor();
            StartTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public static string GetGoodTick => string.Format(Settings.sSTATUS_GOOD, HUtil32.GetTickCount());

        /// <summary>
        /// 随机获取其他服务器
        /// </summary>
        /// <returns></returns>
        public static bool GetMultiServerAddrPort(byte serverIndex, ref string sIPaddr, ref int nPort)
        {
            var result = false;
            for (var i = 0; i < M2Share.ServerTableList.Length; i++)
            {
                var routeInfo = M2Share.ServerTableList[i];
                if (routeInfo == null)
                {
                    continue;
                }
                if (routeInfo.GateCount <= 0)
                {
                    continue;
                }
                if (routeInfo.ServerIdx == serverIndex)
                {
                    sIPaddr = GetRandpmRoute(routeInfo, ref nPort);
                    result = true;
                    break;
                }
            }
            return result;
        }

        private static readonly Regex ScriptRegex = new Regex("(?<=(<))[.\\s\\S]*?(?=(>))", RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.RightToLeft);

        public static MatchCollection MatchScriptLabel(string script)
        {
            return ScriptRegex.Matches(script);
        }

        private static string GetRandpmRoute(TRouteInfo routeInfo, ref int gatePort)
        {
            var random = RandomNumber.Random(routeInfo.GateCount);
            gatePort = routeInfo.GameGatePort[random];
            return routeInfo.GameGateIP[random];
        }

        public static int GetExVersionNO(int nVersionDate, ref int nOldVerstionDate)
        {
            var result = 0;
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
                return Direction.Up;
            }
            if (flagx == 1 && flagy == -1)
            {
                return Direction.UpRight;
            }
            if (flagx == 1 && flagy == 0)
            {
                return Direction.Right;
            }
            if (flagx == 1 && flagy == 1)
            {
                return Direction.DownRight;
            }
            if (flagx == 0 && flagy == 1)
            {
                return Direction.Down;
            }
            if (flagx == -1 && flagy == 1)
            {
                return Direction.DownLeft;
            }
            if (flagx == -1 && flagy == 0)
            {
                return Direction.Left;
            }
            if (flagx == -1 && flagy == -1)
            {
                return Direction.UpLeft;
            }
            return Direction.Down;
        }

        public static bool CheckUserItems(int nIdx, StdItem StdItem)
        {
            var result = false;
            switch (nIdx)
            {
                case ItemLocation.Dress:
                    if (StdItem.StdMode == 10 || StdItem.StdMode == 11)
                    {
                        result = true;
                    }
                    break;
                case ItemLocation.Weapon:
                    if (StdItem.StdMode == 5 || StdItem.StdMode == 6)
                    {
                        result = true;
                    }
                    break;
                case ItemLocation.RighThand:
                    if (StdItem.StdMode == 29 || StdItem.StdMode == 30 || StdItem.StdMode == 28)
                    {
                        result = true;
                    }
                    break;
                case ItemLocation.Necklace:
                    if (StdItem.StdMode == 19 || StdItem.StdMode == 20 || StdItem.StdMode == 21)
                    {
                        result = true;
                    }
                    break;
                case ItemLocation.Helmet:
                    if (StdItem.StdMode == 15)
                    {
                        result = true;
                    }
                    break;
                case ItemLocation.ArmRingl:
                    if (StdItem.StdMode == 24 || StdItem.StdMode == 25 || StdItem.StdMode == 26)
                    {
                        result = true;
                    }
                    break;
                case ItemLocation.ArmRingr:
                    if (StdItem.StdMode == 24 || StdItem.StdMode == 26)
                    {
                        result = true;
                    }
                    break;
                case ItemLocation.Ringl:
                case ItemLocation.Ringr:
                    if (StdItem.StdMode == 22 || StdItem.StdMode == 23)
                    {
                        result = true;
                    }
                    break;
                case ItemLocation.Bujuk:
                    if (StdItem.StdMode == 25 || StdItem.StdMode == 51)
                    {
                        result = true;
                    }
                    break;
                case ItemLocation.Belt:
                    if (StdItem.StdMode == 54 || StdItem.StdMode == 64)
                    {
                        result = true;
                    }
                    break;
                case ItemLocation.Boots:
                    if (StdItem.StdMode == 52 || StdItem.StdMode == 62)
                    {
                        result = true;
                    }
                    break;
                case ItemLocation.Charm:
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
            var result = DateTime.Now;
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

        /// <summary>
        /// 金币在地上显示的外形ID
        /// </summary>
        /// <returns></returns>
        public static int GetRandomLook(int nBaseLook, int nRage)
        {
            return nBaseLook + RandomNumber.Random(nRage);
        }

        public static bool CheckGuildName(string sGuildName)
        {
            var result = true;
            if (sGuildName.Length > Config.GuildNameLen)
            {
                return false;
            }
            for (var i = 0; i < sGuildName.Length - 1; i++)
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
            if (string.IsNullOrEmpty(sName))
            {
                return sName;
            }
            var result = string.Empty;
            var bo11 = false;
            for (var i = 0; i < sName.Length; i++)
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
        /// 获取变量
        /// </summary>
        /// <param name="sText"></param>
        /// <returns></returns>
        public static int GetValNameNo(string sText)
        {
            var result = -1;
            ReadOnlySpan<char> valText = sText.AsSpan();
            if (sText.Length >= 2)
            {
                var valType = char.ToUpper(sText[0]);
                int nValNo;
                switch (valType)
                {
                    case 'P':
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.StrToInt(valText.Slice(1, 2), -1);
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
                            nValNo = HUtil32.StrToInt(valText.Slice(1, 3), -1);
                            if ((nValNo < 500) && (nValNo > 99))
                            {
                                result = nValNo + 700;
                            }
                        }
                        if (sText.Length == 3)
                        {
                            nValNo = HUtil32.StrToInt(valText.Slice(1, 2), -1);
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
                            nValNo = HUtil32.StrToInt(valText.Slice(1, 2), -1);
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
                            nValNo = HUtil32.StrToInt(valText.Slice(1, 2), -1);
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
                            nValNo = HUtil32.StrToInt(valText.Slice(1, 2), -1);
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
                            nValNo = HUtil32.StrToInt(valText.Slice(1, 2), -1);
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
                            nValNo = HUtil32.StrToInt(valText.Slice(2 - 1, 2), -1);
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
                            nValNo = HUtil32.StrToInt(valText.Slice(1, 3), -1);
                            if ((nValNo < 500) && (nValNo > 99))
                            {
                                result = nValNo + 1100;
                            }
                        }
                        else
                        {
                            if (sText.Length == 3)
                            {
                                nValNo = HUtil32.StrToInt(valText.Slice(1, 2), -1);
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
                            nValNo = HUtil32.StrToInt(valText.Slice(2 - 1, 3), -1);
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
                            nValNo = HUtil32.StrToInt(valText.Slice(2 - 1, 2), -1);
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
                            nValNo = HUtil32.StrToInt(valText.Slice(2 - 1, 2), -1);
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
            var item = ItemSystem.GetStdItem(nIndex);
            return IsAccessoryMap.Contains(item.StdMode);
        }

        public static IList<MakeItem> GetMakeItemInfo(string sItemName)
        {
            if (M2Share.MakeItemList.TryGetValue(sItemName, out IList<MakeItem> itemList))
            {
                return itemList;
            }
            return null;
        }

        public static string GetStartPointInfo(int nIndex, ref short nX, ref short nY)
        {
            var result = string.Empty;
            nX = 0;
            nY = 0;
            if (nIndex >= 0 && nIndex < M2Share.StartPointList.Count)
            {
                var startPoint = M2Share.StartPointList[nIndex];
                if (!string.IsNullOrEmpty(startPoint.MapName))
                {
                    nX = startPoint.CurrX;
                    nY = startPoint.CurrY;
                    result = startPoint.MapName;
                }
            }
            return result;
        }

        public static void AddLogonCostLog(string sMsg)
        {
            M2Share.LogonCostLogList.Add(sMsg);
        }

        public static void TrimStringList(StringList sList)
        {
            var n8 = 0;
            while (true)
            {
                if (sList.Count <= n8)
                {
                    break;
                }
                var line = sList[n8].Trim();
                if (string.IsNullOrEmpty(line))
                {
                    sList.RemoveAt(n8);
                    continue;
                }
                n8++;
            }
        }

        public static bool CanMakeItem(string sItemName)
        {
            var result = false;
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

        public static bool SaveItemBindAccount()
        {
            var sFileName = GetEnvirFilePath("ItemBindAccount.txt");
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
            var result = true;
            return result;
        }

        public static bool SaveItemBindChrName()
        {
            var result = false;
            var sFileName = GetEnvirFilePath("ItemBindChrName.txt");
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
            var result = false;
            var sFileName = GetEnvirFilePath("DisableMakeItem.txt");
            //var LoadList = new ArrayList();
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
            var sFileName = GetEnvirFilePath("DisableMakeItem.txt");
            //g_DisableMakeItemList.SaveToFile(sFileName);
            return true;
        }

        public static bool LoadUnMasterList()
        {
            var result = false;
            var sFileName = GetEnvirFilePath("UnMaster.txt");
            //var LoadList = new ArrayList();
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
            var sFileName = GetEnvirFilePath("UnMaster.txt");
            //g_UnMasterList.SaveToFile(sFileName);
            return true;
        }

        public static bool SaveUnForceMasterList()
        {
            var sFileName = GetEnvirFilePath("UnForceMaster.txt");
            //g_UnForceMasterList.SaveToFile(sFileName);
            return true;
        }

        public static bool LoadEnableMakeItem()
        {
            var result = false;
            var sFileName = GetEnvirFilePath("EnableMakeItem.txt");
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
            var sFileName = GetEnvirFilePath("EnableMakeItem.txt");
            //g_EnableMakeItemList.SaveToFile(sFileName);
            return true;
        }

        public static bool SaveAllowSellOffItem()
        {
            var sFileName = GetEnvirFilePath("DisableSellOffItem.txt");
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
        
        public static string GetUseItemName(int nIndex)
        {
            var result = string.Empty;
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
            var sFileName = GetEnvirFilePath("DisableSendMsgList.txt");
            //var LoadList = new ArrayList();
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
            return false;
        }


        public static bool GetDisableSendMsgList(string sHumanName)
        {
            var result = false;
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

        public static bool SaveGameLogItemNameList()
        {
            var sFileName = GetEnvirFilePath("GameLogItemNameList.txt");
            //g_GameLogItemNameList.SaveToFile(sFileName);
            return true;
        }

        public static bool LoadDenyIPAddrList()
        {
            var result = false;
            var sFileName = GetEnvirFilePath("DenyIPAddrList.txt");
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

        public static bool SaveDenyIPAddrList()
        {
            var sFileName = GetEnvirFilePath("DenyIPAddrList.txt");
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
            return true;
        }

        public static bool LoadDenyChrNameList()
        {
            var result = false;
            var sFileName = GetEnvirFilePath("DenyChrNameList.txt");
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
            var result = false;
            //for (I = 0; I < g_DenyChrNameList.Count; I ++ )
            //{
            //    if ((sChrName).CompareTo((g_DenyChrNameList[I])) == 0)
            //    {
            //        result = true;
            //        break;
            //    }
            //}
            return result;
        }

        public static bool SaveDenyChrNameList()
        {
            var sFileName = GetEnvirFilePath("DenyChrNameList.txt");
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

        public static bool SaveDenyAccountList()
        {
            var sFileName = GetEnvirFilePath("DenyAccountList.txt");
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
            return true;
        }

        public static bool GetNoHptoexpMonList(string sMonName)
        {
            var result = false;
            //for (var i = 0; i < g_NoHptoexpMonLIst.Count; i++)
            //{
            //    if ((sMonName).CompareTo((g_NoHptoexpMonLIst[i])) == 0)
            //    {
            //        result = true;
            //        break;
            //    }
            //}
            return result;
        }

        public static bool GetNoClearMonList(string sMonName)
        {
            var result = false;
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
            var sFileName = GetEnvirFilePath("NoHptoExpMonList.txt");
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
            return true;
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
            return false;
        }

        // sIPaddr 为当前IP
        // dIPaddr 为要比较的IP
        // * 号为通配符
        public static bool CompareIPaddr(string sIPaddr, string dIPaddr)
        {
            bool result;
            if (string.IsNullOrEmpty(sIPaddr) || string.IsNullOrEmpty(dIPaddr))
            {
                return false;
            }
            if (dIPaddr[1] == '*')
            {
                return true;
            }
            var nPos = dIPaddr.IndexOf('*');
            if (nPos > 0)
            {
                result = HUtil32.CompareLStr(sIPaddr, dIPaddr, nPos - 1);
            }
            else
            {
                result = string.Compare(sIPaddr, dIPaddr, StringComparison.OrdinalIgnoreCase) == 0;
            }
            return result;
        }

        public static int MakeMonsterFeature(byte btRaceImg, byte btWeapon, ushort wAppr)
        {
            return HUtil32.MakeLong(HUtil32.MakeWord(btRaceImg, btWeapon), wAppr);
        }

        public static int MakeHumanFeature(byte btRaceImg, byte btDress, byte btWeapon, byte btHair)
        {
            return HUtil32.MakeLong(HUtil32.MakeWord(btRaceImg, btWeapon), HUtil32.MakeWord(btHair, btDress));
        }

        public static string GetEnvirFilePath(string filePath)
        {
            if (filePath.StartsWith(".."))
            {
                return Path.Combine(BasePath, Config.EnvirDir, filePath[3..]);
            }
            return Path.Combine(BasePath, Config.EnvirDir, filePath);
        }

        public static string GetEnvirFilePath(string dirPath, string filePath)
        {
            if (filePath.StartsWith(".."))
            {
                return Path.Combine(BasePath, Config.EnvirDir, filePath[3..]);
            }
            return Path.Combine(BasePath, Config.EnvirDir, dirPath, filePath);
        }

        public static string GetNoticeFilePath(string filePath)
        {
            if (filePath.StartsWith(".."))
            {
                return Path.Combine(BasePath, Config.EnvirDir, filePath[3..]);
            }
            return Path.Combine(BasePath, Config.NoticeDir, filePath);
        }
    }
}