using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Channels;
using SystemModule;
using SystemModule.Common;

namespace GameGate
{
    public class GateShare
    {
        public static object CS_MainLog = null;
        public static object CS_FilterMsg = null;
        public static IList<string> MainLogMsgList = null;
        public static int nShowLogLevel = 0;
        public static int ServerCount = 1;
        public static string GateClass = "GameGate";
        public static string GateName = "游戏网关";
        public static string TitleName = "SKY引擎";
        public static string GateAddr = "10.10.0.168";
        public static int GatePort = 7200;
        public static bool boServerReady;
        /// <summary>
        /// 显示B 或 KB
        /// </summary>
        public static bool boShowBite = true;
        public static bool boServiceStart = false;
        /// <summary>
        /// 网关是否就绪
        /// </summary>
        public static bool boGateReady = false;
        /// <summary>
        ///  网关游戏服务器之间检测超时时间长度
        /// </summary>
        public static long dwCheckServerTimeOutTime = 3 * 60 * 1000;
        public static IList<string> AbuseList = null;
        public static ConcurrentDictionary<string, int> SessionIndex;
        /// <summary>
        /// 是否显示SOCKET接收的信息
        /// </summary>
        public static bool boShowSckData = true;
        public static string sReplaceWord = "*";
        /// <summary>
        /// 接收封包（客户端-》网关）
        /// </summary>
        public static Channel<TSendUserData> ReviceMsgList = null;
        /// <summary>
        /// 发送封包（网关-》客户端）
        /// </summary>
        public static Channel<TSendUserData> SendMsgList = null;
        /// <summary>
        /// 转发封包（数据引擎-》网关）
        /// </summary>
        public static Channel<ForwardMessage> ForwardMsgList = null;
        public static int nCurrConnCount = 0;
        public static bool boSendHoldTimeOut = false;
        public static long dwSendHoldTick = 0;
        public static long dwCheckRecviceTick = 0;
        public static long dwCheckRecviceMin = 0;
        public static long dwCheckRecviceMax = 0;
        public static long dwCheckServerTick = 0;
        public static long dwCheckServerTimeMin = 0;
        public static long dwCheckServerTimeMax = 0;
        /// <summary>
        /// 累计接受数据大小
        /// </summary>
        public static int NReviceMsgSize;
        public static bool boDecodeMsgLock = false;
        public static long dwProcessReviceMsgTimeLimit = 0;
        public static long dwProcessSendMsgTimeLimit = 0;
        /// <summary>
        /// 禁止连接IP列表
        /// </summary>
        public static List<string> BlockIPList = null;
        /// <summary>
        /// 临时禁止连接IP列表
        /// </summary>
        public static List<string> TempBlockIPList = null;
        public static int nMaxConnOfIPaddr = 50;
        public static int nMaxClientPacketSize = 7000;
        public static int nNomClientPacketSize = 150;
        public static long dwClientCheckTimeOut = 50;
        public static int nMaxOverNomSizeCount = 2;
        public static int nMaxClientMsgCount = 15;
        public static TBlockIPMethod BlockMethod = TBlockIPMethod.mDisconnect;
        public static bool bokickOverPacketSize = true;
        /// <summary>
        /// 发送给客户端数据包大小限制
        /// </summary>
        public static int nClientSendBlockSize = 1000;
        /// <summary>
        /// 客户端连接会话超时(指定时间内未有数据传输)
        /// </summary>
        public static long dwClientTimeOutTime = 5000;
        public static IniFile Conf = null;
        private static string sConfigFileName = "Config.ini";
        /// <summary>
        /// 发言字符长度
        /// </summary>
        public static int nSayMsgMaxLen = 70;
        /// <summary>
        /// 发言间隔时间
        /// </summary>
        public static long dwSayMsgTime = 1000;
        /// <summary>
        /// 攻击间隔时间
        /// </summary>
        public static long dwHitTime = 300;
        /// <summary>
        /// 会话超时时间
        /// </summary>
        public static long dwSessionTimeOutTime = 60 * 60 * 1000;
        public const int MSGMAXLENGTH = 20000;
        public const int SENDCHECKSIZE = 512;
        public const int SENDCHECKSIZEMAX = 2048;
        /// <summary>
        /// 超速显示
        /// </summary>
        public static bool boCheckBoxShowData = true;
        // 外挂控制相关变量
        /// <summary>
        /// 是否启动了外挂控制
        /// </summary>
        public static bool boStartSpeedCheck = true;
        /// <summary>
        /// 是否启动了攻击控制
        /// </summary>
        public static bool boStartHitCheck = true;
        /// <summary>
        /// 是否启动了魔法控制
        /// </summary>
        public static bool boStartSpellCheck = true;
        /// <summary>
        /// 是否启动了走路控制
        /// </summary>
        public static bool boStartWalkCheck = true;
        /// <summary>
        /// 是否启动了跑步控制
        /// </summary>
        public static bool boStartRunCheck = true;
        /// <summary>
        /// 是否启动了转身控制
        /// </summary>
        public static bool boStartTurnCheck = true;
        /// <summary>
        /// 是否启动了挖肉控制
        /// </summary>
        public static bool boStartButchCheck = true;
        /// <summary>
        /// 是否启动了吃药控制
        /// </summary>
        public static bool boStartEatCheck = true;
        /// <summary>
        /// 是否启动了移动攻击控制
        /// </summary>
        public static bool boStartRunhitCheck = true;
        /// <summary>
        /// 是否启动了移动魔法控制
        /// </summary>
        public static bool boStartRunspellCheck = true;
        /// <summary>
        /// 是否启动了带有攻击并发控制
        /// </summary>
        public static bool boStartConHitMaxCheck = true;
        /// <summary>
        /// 是否启动了带有魔法并发控制
        /// </summary>
        public static bool boStartConSpellMaxCheck = true;
        /// <summary>
        /// 攻击速度间隔
        /// </summary>
        public static int dwSpinEditHitTime = 672;
        /// <summary>
        /// 魔法速度间隔
        /// </summary>
        public static int dwSpinEditSpellTime = 1100;
        /// <summary>
        /// 走路速度间隔
        /// </summary>
        public static int dwSpinEditWalkTime = 540;
        /// <summary>
        /// 跑步速度间隔
        /// </summary>
        public static int dwSpinEditRunTime = 520;
        /// <summary>
        /// 转身速度间隔
        /// </summary>
        public static int dwSpinEditTurnTime = 100;
        /// <summary>
        /// 挖肉速度间隔
        /// </summary>
        public static int dwSpinEditButchTime = 500;
        /// <summary>
        /// 吃药速度间隔
        /// </summary>
        public static int dwSpinEditEatTime = 200;
        /// <summary>
        /// 捡起速度间隔
        /// </summary>
        public static int dwSpinEditPickupTime = 100;
        /// <summary>
        /// 移动攻击速度间隔
        /// </summary>
        public static int dwSpinEditRunhitTime = 320;
        /// <summary>
        /// 移动魔法速度间隔
        /// </summary>
        public static int dwSpinEditRunspellTime = 350;
        /// <summary>
        /// 带有攻击并发个数
        /// </summary>
        public static byte dwSpinEditConHitMaxTime = 10;
        /// <summary>
        /// 带有魔法并发个数
        /// </summary>
        public static byte dwSpinEditConSpellMaxTime = 10;
        /// <summary>
        /// 攻击加速处理
        /// </summary>
        public static byte dwComboBoxHitCheck = 2;
        /// <summary>
        /// 魔法加速处理
        /// </summary>
        public static byte dwComboBoxSpellCheck = 2;
        /// <summary>
        /// 走路加速处理
        /// </summary>
        public static byte dwComboBoxWalkCheck = 3;
        /// <summary>
        /// 跑步加速处理
        /// </summary>
        public static byte dwComboBoxRunCheck = 3;
        /// <summary>
        /// 转身加速处理
        /// </summary>
        public static byte dwComboBoxTurnCheck = 0;
        /// <summary>
        /// 挖肉加速处理
        /// </summary>
        public static byte dwComboBoxButchCheck = 0;
        /// <summary>
        /// 吃药加速处理
        /// </summary>
        public static byte dwComboBoxEatCheck = 1;
        /// <summary>
        /// 移动攻击加速处理
        /// </summary>
        public static byte dwComboBoxRunhitCheck = 1;
        /// <summary>
        /// 移动魔法加速处理
        /// </summary>
        public static byte dwComboBoxRunspellCheck = 1;
        /// <summary>
        /// 带有攻击并发处理
        /// </summary>
        public static byte dwComboBoxConHitMaxCheck = 2;
        /// <summary>
        /// 带有魔法并发处理
        /// </summary>
        public static byte dwComboBoxConSpellMaxCheck = 2;
        /// <summary>
        /// 每次加速的累加值
        /// </summary>
        public static int nIncErrorCount = 5;
        /// <summary>
        /// 正常动作的减少值
        /// </summary>
        public static int nDecErrorCount = 1;
        /// <summary>
        /// 装备加速校正因数
        /// </summary>
        public static int nItemSpeedCount = 60;
        /// <summary>
        /// 攻击累加值限制
        /// </summary>
        public static int nSpinEditHitCount = 50;
        /// <summary>
        /// 魔法累加值限制
        /// </summary>
        public static int nSpinEditSpellCount = 50;
        /// <summary>
        /// 走路累加值限制
        /// </summary>
        public static int nSpinEditWalkCount = 50;
        /// <summary>
        /// 跑步累加值限制
        /// </summary>
        public static int nSpinEditRunCount = 50;
        /// <summary>
        /// 去掉引擎广告(100%)
        /// </summary>
        public static bool boAdvertiCheck = true;
        /// <summary>
        /// 封掉变速齿轮( 99%)
        /// </summary>
        public static bool boSupSpeederCheck = true;
        /// <summary>
        /// 封掉超级加速(100%)
        /// </summary>
        public static bool boSuperNeverCheck = true;
        /// <summary>
        /// 封掉特殊攻击( 85%)
        /// </summary>
        public static bool boAfterHitCheck = true;
        /// <summary>
        /// 封掉挖地暗杀( 99%)
        /// </summary>
        public static bool boDarkHitCheck = true;
        /// <summary>
        /// 封掉一步三格(100%)
        /// </summary>
        public static bool boWalk3caseCheck = true;
        /// <summary>
        /// 封掉飞取装备( 75%)
        /// </summary>
        public static bool boFeiDnItemsCheck = true;
        /// <summary>
        /// 封掉组合攻击(100%)
        /// </summary>
        public static bool boCombinationCheck = true;
        /// <summary>
        /// 发言间隔时间
        /// </summary>
        public static long TrinidadMsgTime = 3000;
        /// <summary>
        /// 传送间隔时间
        /// </summary>
        public static long dwSayMoveTime = 10000;
        public static byte boFilterMode = 4;
        public static string sMsgFilter = "@传";
        public static string sModeFilter = "@传送";
        public static string sCharFilter = "您发送的信息里包含了非法字符.";
        public static string boMsgUserName = "$$$$$";
        public static string sWarningMsg = "[提示]: 请爱护游戏环境，关闭加速外挂重新登陆！";
        public static string yWarningMsg = "[提示]: 发现超速动作，服务器将延迟您当前操作！";
        public static string jWarningMsg = "[提示]: 发现超速动作，服务器将停顿您当前操作！";
        public static string fWarningMsg = "[提示]: 短时间内无法使用此功能！";
        public static string gWarningMsg = "[提示]: 发言速度太快了！";
        public static byte btMsgFColorS = 251;
        public static byte btMsgBColorS = 0;
        public static byte btMsgFColorY = 219;
        public static byte btMsgBColorY = 255;
        public static byte btMsgFColorJ = 255;
        public static byte btMsgBColorJ = 56;
        public static byte btRedMsgFColor = 0xFF;
        public static byte btRedMsgBColor = 0x38;
        public static byte btGreenMsgFColor = 0xDB;
        public static byte btGreenMsgBColor = 0xFF;
        public static byte btBlueMsgFColor = 0xFF;
        public static byte btBlueMsgBColor = 0xFC;
        public static byte btOLMsgFColor = 0xFC;
        public static byte btOLMsgBColor = 0x0;
        public static byte btYYMsgFColor = 0xFD;
        public static byte btYYMsgBColor = 0xFF;
        /// <summary>
        /// 玩家开挂记录列表
        /// </summary>
        public static ConcurrentDictionary<string,string> GameSpeedList = null;
        public static ConcurrentDictionary<string, ForwardClientService> _ClientGateMap;
        private static ConcurrentDictionary<int, bool> Magic_Attack_Array;
        private static ConcurrentDictionary<int, int> MagicDelayTimeMap;
        public static ConcurrentDictionary<string, ForwardClientService> ServerGateList;
        public static ConcurrentDictionary<string, UserClientSession> UserSessions;
        public static List<WeightedItem<ForwardClientService>> m_ServerGateList = new List<WeightedItem<ForwardClientService>>();
            
        public static void AddMainLogMsg(string Msg, int nLevel)
        {
            string tMsg;
            try
            {
                HUtil32.EnterCriticalSection(CS_MainLog);
                tMsg = "[" + DateTime.Now + "] " + Msg;
                MainLogMsgList.Add(tMsg);
            }
            finally
            {
              HUtil32.LeaveCriticalSection(CS_MainLog);
            }
        }

        public static void LoadAbuseFile()
        {
            AddMainLogMsg("正在加载文字过滤配置信息...", 4);
            var sFileName = ".\\WordFilter.txt";
            if (File.Exists(sFileName))
            {
                try
                {
                    HUtil32.EnterCriticalSection(CS_FilterMsg);
                    //AbuseList.LoadFromFile(sFileName);
                }
                finally
                {
                    HUtil32.LeaveCriticalSection(CS_FilterMsg);
                }
            }
            AddMainLogMsg("文字过滤信息加载完成...", 4);
        }

        public static void LoadBlockIPFile()
        {
            AddMainLogMsg("正在加载IP过滤配置信息...", 4);
            var sFileName = ".\\BlockIPList.txt";
            if (File.Exists(sFileName))
            {
                //BlockIPList.LoadFromFile(sFileName);
            }
            AddMainLogMsg("IP过滤配置信息加载完成...", 4);
        }

        public static int GetSocketIndex(string connectionId)
        {
            var socketIndex = 0;
            if (SessionIndex.TryGetValue(connectionId, out socketIndex))
            {
                return socketIndex;
            }
            return socketIndex;
        }

        public static void DelSocketIndex(string connectionId)
        {
            var socketIndex = 0;
            SessionIndex.TryRemove(connectionId, out socketIndex);
        }

        /// <summary>
        /// 获取用户链接对应网关
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        public static ForwardClientService GetUserClient(string connectionId)
        {
            return _ClientGateMap.TryGetValue(connectionId, out var userClinet) ? userClinet : null;
        }
        
        /// <summary>
        /// 从字典删除用户和网关对应关系
        /// </summary>
        /// <param name="connectionId"></param>
        public static void DeleteUserClient(string connectionId)
        {
            _ClientGateMap.TryRemove(connectionId, out var userClinet);
        }

        public static void Initialization()
        {
            Conf = new IniFile(Path.Combine(AppContext.BaseDirectory, sConfigFileName));
            Conf.Load();
            nShowLogLevel = Conf.ReadInteger(GateClass, "ShowLogLevel", nShowLogLevel);
            CS_MainLog = new object();
            CS_FilterMsg = new object();
            MainLogMsgList = new List<string>();
            AbuseList = new List<string>();
            ReviceMsgList = Channel.CreateUnbounded<TSendUserData>();
            SendMsgList = Channel.CreateUnbounded<TSendUserData>();
            ForwardMsgList = Channel.CreateUnbounded<ForwardMessage>();
            boShowSckData = false;
            BlockIPList = new List<string>();
            TempBlockIPList = new List<string>();
            SessionIndex = new ConcurrentDictionary<string, int>();
            _ClientGateMap = new ConcurrentDictionary<string, ForwardClientService>();
            Magic_Attack_Array = new ConcurrentDictionary<int, bool>();
            MagicDelayTimeMap = new ConcurrentDictionary<int, int>();
            ServerGateList = new ConcurrentDictionary<string, ForwardClientService>();
            UserSessions = new ConcurrentDictionary<string, UserClientSession>();
            GameSpeedList = new ConcurrentDictionary<string, string>();
        }

        public static void InitMagicAttackMap()
        {
            //Magic_Attack_Array.TryAdd();
        }

        public static void InitMagicDelayTimeMap()
        {
            MagicDelayTimeMap[1] = 1000; //火球术
            MagicDelayTimeMap[2] = 1000; //治愈术
            MagicDelayTimeMap[5] = 1000; //大火球
            MagicDelayTimeMap[6] = 1000; //施毒术
            MagicDelayTimeMap[8] = 960; //抗拒火环
            MagicDelayTimeMap[9] = 1000; //地狱火
            MagicDelayTimeMap[10] = 1000; //疾光电影
            MagicDelayTimeMap[11] = 1000; //雷电术
            MagicDelayTimeMap[13] = 1000; //灵魂火符
            MagicDelayTimeMap[14] = 1000; //幽灵盾
            MagicDelayTimeMap[15] = 1000; //神圣战甲术
            MagicDelayTimeMap[16] = 1000; //困魔咒
            MagicDelayTimeMap[17] = 1000; //召唤骷髅
            MagicDelayTimeMap[18] = 1000; //隐身术
            MagicDelayTimeMap[19] = 1000; //集体隐身术
            MagicDelayTimeMap[20] = 1000; //诱惑之光
            MagicDelayTimeMap[21] = 1000; //瞬息移动
            MagicDelayTimeMap[22] = 1000; //火墙
            MagicDelayTimeMap[23] = 1000; //爆裂火焰
            MagicDelayTimeMap[24] = 1000; //地狱雷光
            MagicDelayTimeMap[28] = 1000; //心灵启示
            MagicDelayTimeMap[29] = 1000; //群体治疗术
            MagicDelayTimeMap[30] = 1000; //召唤神兽
            MagicDelayTimeMap[32] = 1000; //圣言术
            MagicDelayTimeMap[33] = 950; //冰咆哮
            MagicDelayTimeMap[34] = 1000; //解毒术
            MagicDelayTimeMap[35] = 1000; //狮吼功
            MagicDelayTimeMap[36] = 1000; //火焰冰
            MagicDelayTimeMap[37] = 1000; //群体雷电术
            MagicDelayTimeMap[38] = 1000; //群体施毒术
            MagicDelayTimeMap[39] = 960; //彻地钉
            MagicDelayTimeMap[41] = 960; //狮子吼
            MagicDelayTimeMap[44] = 1000; //寒冰掌
            MagicDelayTimeMap[45] = 1000; //灭天火
            MagicDelayTimeMap[46] = 1000; //分身术
            MagicDelayTimeMap[47] = 1000; //火龙焰
        }

        public static void Add(string name, ForwardClientService userClientService)
        {
            if (ServerGateList.ContainsKey(name))
            {
                return;
            }
            ServerGateList.TryAdd(name, userClientService);
        }

        public static void Delete(string name)
        {
            ServerGateList.TryRemove(name, out var userClientService);
        }

        public static ForwardClientService GetClientService()
        {
            //TODO 根据配置文件有四种模式  默认随机
            //1.轮询分配
            //2.总是分配到最小资源 即网关在线人数最小的那个
            //3.一直分配到一个 直到当前玩家达到配置上线，则开始分配到其他可用网关
            //4.按权重分配
            var userList = new List<ForwardClientService>(ServerGateList.Values);
            var random = new System.Random().Next(userList.Count);
            return userList[random];
        }
    } 
}