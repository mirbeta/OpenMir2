using GameSvr.Actor;
using GameSvr.Event;
using GameSvr.GameCommand;
using GameSvr.Magic;
using GameSvr.Maps;
using GameSvr.Script;
using System.Collections;
using SystemModule;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.Player
{
    public partial class PlayObject
    {
        public ClientMesaagePacket m_DefMsg;
        public string m_sOldSayMsg = string.Empty;
        public int m_nSayMsgCount = 0;
        public int m_dwSayMsgTick;
        public bool m_boDisableSayMsg;
        public int m_dwDisableSayMsgTick;
        public int m_dwCheckDupObjTick;
        public int dwTick578;
        public int dwTick57C;
        public bool m_boInSafeArea;
        /// <summary>
        /// 登录帐号名
        /// </summary>
        public string UserID;
        /// <summary>
        /// 人物IP地址
        /// </summary>
        public string m_sIPaddr = string.Empty;
        public string m_sIPLocal = string.Empty;
        /// <summary>
        /// 账号过期
        /// </summary>
        public bool AccountExpired;
        /// <summary>
        /// 账号游戏点数检查时间
        /// </summary>
        public int AccountExpiredTick;
        public long ExpireTime;
        public int ExpireCount;
        public int QueryExpireTick;
        public int m_nSocket = 0;
        /// <summary>
        /// 人物连接到游戏网关SOCKETID
        /// </summary>
        public ushort m_nGSocketIdx = 0;
        /// <summary>
        /// 人物所在网关号
        /// </summary>
        public int m_nGateIdx = 0;
        public int m_nSoftVersionDate = 0;
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime m_dLogonTime;
        /// <summary>
        /// 战领沙城时间
        /// </summary>
        public int m_dwLogonTick;
        /// <summary>
        /// 是否进入游戏完成
        /// </summary>
        public bool m_boReadyRun;
        /// <summary>
        /// 当前会话ID
        /// </summary>
        public int m_nSessionID = 0;
        /// <summary>
        /// 人物当前付费模式
        /// 1:试玩
        /// 2:付费
        /// 3:测试
        /// </summary>
        public int PayMent;
        public int m_nPayMode = 0;
        /// <summary>
        /// 全局会话信息
        /// </summary>
        public TSessInfo m_SessInfo;
        public int m_dwLoadTick = 0;
        /// <summary>
        /// 人物当前所在服务器序号
        /// </summary>
        public int m_nServerIndex = 0;
        public bool m_boEmergencyClose;
        /// <summary>
        /// 掉线标志
        /// </summary>
        public bool m_boSoftClose;
        /// <summary>
        /// 断线标志(@kick 命令)
        /// </summary>
        public bool m_boKickFlag;
        /// <summary>
        /// 是否重连
        /// </summary>
        public bool m_boReconnection;
        public bool m_boRcdSaved;
        public bool m_boSwitchData;
        public bool m_boSwitchDataOK = false;
        public string m_sSwitchDataTempFile = string.Empty;
        public int m_nWriteChgDataErrCount;
        public string m_sSwitchMapName = string.Empty;
        public short m_nSwitchMapX = 0;
        public short m_nSwitchMapY = 0;
        public bool m_boSwitchDataSended;
        public int m_dwChgDataWritedTick = 0;
        /// <summary>
        /// 攻击间隔
        /// </summary>
        public int m_dwHitIntervalTime;
        /// <summary>
        /// 魔法间隔
        /// </summary>
        public int m_dwMagicHitIntervalTime;
        /// <summary>
        /// 走路间隔
        /// </summary>
        public int m_dwRunIntervalTime;
        /// <summary>
        /// 走路间隔
        /// </summary>
        public int m_dwWalkIntervalTime;
        /// <summary>
        /// 换方向间隔
        /// </summary>
        public int m_dwTurnIntervalTime;
        /// <summary>
        /// 组合操作间隔
        /// </summary>
        public int m_dwActionIntervalTime;
        /// <summary>
        /// 移动刺杀间隔
        /// </summary>
        public int m_dwRunLongHitIntervalTime;
        /// <summary>
        /// 跑位攻击间隔
        /// </summary>        
        public int m_dwRunHitIntervalTime;
        /// <summary>
        /// 走位攻击间隔
        /// </summary>        
        public int m_dwWalkHitIntervalTime;
        /// <summary>
        /// 跑位魔法间隔
        /// </summary>        
        public int m_dwRunMagicIntervalTime;
        /// <summary>
        /// 魔法攻击时间
        /// </summary>        
        public int m_dwMagicAttackTick;
        /// <summary>
        /// 魔法攻击间隔时间
        /// </summary>
        public int m_dwMagicAttackInterval;
        /// <summary>
        /// 攻击时间
        /// </summary>
        public int m_dwAttackTick;
        /// <summary>
        /// 人物跑动时间
        /// </summary>
        public int m_dwMoveTick;
        /// <summary>
        /// 人物攻击计数
        /// </summary>
        public int m_dwAttackCount;
        /// <summary>
        /// 人物攻击计数
        /// </summary>
        public int m_dwAttackCountA;
        /// <summary>
        /// 魔法攻击计数
        /// </summary>
        public int m_dwMagicAttackCount;
        /// <summary>
        /// 人物跑计数
        /// </summary>
        public int m_dwMoveCount;
        /// <summary>
        /// 超速计数
        /// </summary>
        public int m_nOverSpeedCount;
        public bool m_boDieInFight3Zone;
        public string m_sGotoNpcLabel = string.Empty;
        public bool TakeDlgItem = false;
        public int DlgItemIndex = 0;
        public int m_nDelayCall;
        public int m_dwDelayCallTick = 0;
        public bool m_boDelayCall;
        public int m_DelayCallNPC;
        public string m_sDelayCallLabel = string.Empty;
        public TScript m_Script;
        public BaseObject m_NPC = null;
        /// <summary>
        /// 玩家的变量P
        /// </summary>
        public int[] m_nVal;
        /// <summary>
        /// 玩家的变量M
        /// </summary>
        public int[] m_nMval;
        /// <summary>
        /// 玩家的变量D
        /// </summary>
        public int[] m_DyVal;
        /// <summary>
        /// 玩家的变量
        /// </summary>
        public string[] m_nSval;
        /// <summary>
        /// 人物变量  N
        /// </summary>
        public int[] m_nInteger;
        /// <summary>
        /// 人物变量  S
        /// </summary>
        public string[] m_sString;
        /// <summary>
        /// 服务器变量 W 0-20 个人服务器数值变量，不可保存，不可操作
        /// </summary>
        public string[] m_ServerStrVal;
        /// <summary>
        /// E 0-20 个人服务器字符串变量，不可保存，不可操作
        /// </summary>
        public int[] m_ServerIntVal;
        public string m_sPlayDiceLabel = string.Empty;
        public bool m_boTimeRecall;
        public int m_dwTimeRecallTick = 0;
        public string m_sMoveMap = string.Empty;
        public short m_nMoveX;
        public short m_nMoveY;
        /// <summary>
        /// 保存人物数据时间间隔
        /// </summary>
        public int m_dwSaveRcdTick;
        public byte m_btBright;
        public bool m_boNewHuman;
        public bool m_boSendNotice;
        public int m_dwWaitLoginNoticeOKTick;
        public bool m_boLoginNoticeOK;
        public bool bo6AB;
        public int m_dwShowLineNoticeTick;
        public int m_nShowLineNoticeIdx;
        public int m_nSoftVersionDateEx;
        /// <summary>
        /// 可点击脚本标签字典
        /// </summary>
        private readonly Hashtable m_CanJmpScriptLableList;
        public int m_nScriptGotoCount = 0;
        public string m_sScriptCurrLable = string.Empty;
        // 用于处理 @back 脚本命令
        public string m_sScriptGoBackLable = string.Empty;
        // 用于处理 @back 脚本命令
        public int m_dwTurnTick;
        public int m_wOldIdent = 0;
        public byte m_btOldDir = 0;
        /// <summary>
        /// 第一个操作
        /// </summary>
        public bool m_boFirstAction = false;
        /// <summary>
        /// 二次操作之间间隔时间
        /// </summary>        
        public int m_dwActionTick;
        /// <summary>
        /// 配偶名称
        /// </summary>
        public string m_sDearName;
        public PlayObject m_DearHuman;
        /// <summary>
        /// 是否允许夫妻传送
        /// </summary>
        public bool m_boCanDearRecall;
        public bool m_boCanMasterRecall;
        /// <summary>
        /// 夫妻传送时间
        /// </summary>
        public int m_dwDearRecallTick;
        public int m_dwMasterRecallTick;
        /// <summary>
        /// 师徒名称
        /// </summary>
        public string m_sMasterName;
        public PlayObject m_MasterHuman;
        public IList<PlayObject> m_MasterList;
        public bool m_boMaster = false;
        /// <summary>
        /// 声望点
        /// </summary>
        public byte m_btCreditPoint = 0;
        /// <summary>
        /// 离婚次数
        /// </summary>        
        public byte m_btMarryCount = 0;
        /// <summary>
        /// 转生等级
        /// </summary>
        public byte m_btReLevel = 0;
        public byte m_btReColorIdx;
        public int m_dwReColorTick = 0;
        /// <summary>
        /// 杀怪经验倍数
        /// </summary>
        public int m_nKillMonExpMultiple;
        /// <summary>
        /// 处理消息循环时间控制
        /// </summary>        
        public int m_dwGetMsgTick = 0;
        public bool m_boSetStoragePwd;
        public bool m_boReConfigPwd;
        public bool m_boCheckOldPwd;
        public bool m_boUnLockPwd;
        public bool m_boUnLockStoragePwd;
        public bool m_boPasswordLocked;
        // 锁密码
        public byte m_btPwdFailCount;
        /// <summary>
        /// 是否启用锁登录功能
        /// </summary>
        public bool m_boLockLogon;
        /// <summary>
        /// 是否打开登录锁
        /// </summary>        
        public bool m_boLockLogoned;
        public string m_sTempPwd;
        public string m_sStoragePwd;
        public BaseObject m_PoseBaseObject = null;
        public bool m_boStartMarry = false;
        public bool m_boStartMaster = false;
        public bool m_boStartUnMarry = false;
        public bool m_boStartUnMaster = false;
        /// <summary>
        /// 禁止发方字(发的文字只能自己看到)
        /// </summary>
        public bool m_boFilterSendMsg;
        /// <summary>
        /// 杀怪经验倍数(此数除以 100 为真正倍数)
        /// </summary>        
        public int m_nKillMonExpRate;
        /// <summary>
        /// 人物攻击力倍数(此数除以 100 为真正倍数)
        /// </summary>        
        public int m_nPowerRate;
        public int m_dwKillMonExpRateTime = 0;
        public int m_dwPowerRateTime = 0;
        public int m_dwRateTick;
        /// <summary>
        /// 是否允许使用物品
        /// </summary>
        public bool m_boCanUseItem;
        public bool m_boCanDeal;
        public bool m_boCanDrop;
        public bool m_boCanGetBackItem;
        public bool m_boCanWalk;
        public bool m_boCanRun;
        public bool m_boCanHit;
        public bool m_boCanSpell;
        public bool m_boCanSendMsg;
        public int m_nMemberType;
        // 会员类型
        public int m_nMemberLevel;
        // 会员等级
        public bool m_boSendMsgFlag;
        // 发祝福语标志
        public bool m_boChangeItemNameFlag;
        /// <summary>
        /// 游戏币
        /// </summary>
        public int m_nGameGold;
        /// <summary>
        /// 是否自动减游戏币
        /// </summary>        
        public bool m_boDecGameGold;
        public int m_dwDecGameGoldTime;
        public int m_dwDecGameGoldTick;
        public int m_nDecGameGold;
        // 一次减点数
        public bool m_boIncGameGold;
        // 是否自动加游戏币
        public int m_dwIncGameGoldTime;
        public int m_dwIncGameGoldTick;
        public int m_nIncGameGold;
        // 一次减点数
        public int m_nGamePoint;
        // 游戏点数
        public int m_dwIncGamePointTick;
        public int m_nPayMentPoint;
        public int m_dwPayMentPointTick = 0;
        public int m_dwDecHPTick = 0;
        public int m_dwIncHPTick = 0;
        public PlayObject m_GetWhisperHuman;
        public int m_dwClearObjTick = 0;
        public short m_wContribution;
        // 贡献度
        public string m_sRankLevelName = string.Empty;
        // 显示名称格式串
        public bool m_boFilterAction = false;
        public int m_dwAutoGetExpTick;
        public int m_nAutoGetExpTime = 0;
        public int m_nAutoGetExpPoint;
        public Envirnoment m_AutoGetExpEnvir;
        public bool m_boAutoGetExpInSafeZone = false;
        public Dictionary<string, TDynamicVar> m_DynamicVarList;
        public short m_dwClientTick;
        /// <summary>
        /// 进入速度测试模式
        /// </summary>
        public bool m_boTestSpeedMode;
        public int m_dwDelayTime = 0;
        public string m_sRandomNo = string.Empty;
        public int m_dwQueryBagItemsTick = 0;
        public bool m_boTimeGoto;
        public int m_dwTimeGotoTick;
        public string m_sTimeGotoLable;
        public BaseObject m_TimeGotoNPC;
        /// <summary>
        /// 个人定时器
        /// </summary>
        public int[] AutoTimerTick;
        /// <summary>
        /// 个人定时器 时间间隔
        /// </summary>
        public int[] AutoTimerStatus;
        /// <summary>
        /// 包裹刷新时间
        /// </summary>
        public int m_dwClickNpcTime = 0;
        /// <summary>
        /// 是否开通元宝交易服务
        /// </summary>
        public bool bo_YBDEAL;
        /// <summary>
        /// 确认元宝寄售标志
        /// </summary>
        public bool m_boSellOffOK = false;
        /// <summary>
        /// 元宝寄售物品列表
        /// </summary>
        private IList<UserItem> m_SellOffItemList;


        public PlayObject() : base()
        {
            Race = ActorRace.Play;
            m_boEmergencyClose = false;
            m_boSwitchData = false;
            m_boReconnection = false;
            m_boKickFlag = false;
            m_boSoftClose = false;
            m_boReadyRun = false;
            m_dwSaveRcdTick = HUtil32.GetTickCount();
            WantRefMsg = true;
            m_boRcdSaved = false;
            m_boDieInFight3Zone = false;
            m_sGotoNpcLabel = "";
            m_nDelayCall = 0;
            m_sDelayCallLabel = "";
            m_boDelayCall = false;
            m_DelayCallNPC = 0;
            m_Script = null;
            m_boTimeRecall = false;
            m_sMoveMap = "";
            m_nMoveX = 0;
            m_nMoveY = 0;
            RunTick = HUtil32.GetTickCount();
            RunTime = 250;
            SearchTime = 1000;
            SearchTick = HUtil32.GetTickCount();
            ViewRange = 12;
            m_boNewHuman = false;
            m_boLoginNoticeOK = false;
            bo6AB = false;
            AccountExpired = false;
            m_boSendNotice = false;
            m_dwCheckDupObjTick = HUtil32.GetTickCount();
            dwTick578 = HUtil32.GetTickCount();
            dwTick57C = HUtil32.GetTickCount();
            m_boInSafeArea = false;
            m_dwMagicAttackTick = HUtil32.GetTickCount();
            m_dwMagicAttackInterval = 0;
            m_dwAttackTick = HUtil32.GetTickCount();
            m_dwMoveTick = HUtil32.GetTickCount();
            m_dwTurnTick = HUtil32.GetTickCount();
            m_dwActionTick = HUtil32.GetTickCount();
            m_dwAttackCount = 0;
            m_dwAttackCountA = 0;
            m_dwMagicAttackCount = 0;
            m_dwMoveCount = 0;
            m_nOverSpeedCount = 0;
            m_sOldSayMsg = "";
            m_dwSayMsgTick = HUtil32.GetTickCount();
            m_boDisableSayMsg = false;
            m_dwDisableSayMsgTick = HUtil32.GetTickCount();
            m_dLogonTime = DateTime.Now;
            m_dwLogonTick = HUtil32.GetTickCount();
            m_boSwitchData = false;
            m_boSwitchDataSended = false;
            m_nWriteChgDataErrCount = 0;
            m_dwShowLineNoticeTick = HUtil32.GetTickCount();
            m_nShowLineNoticeIdx = 0;
            m_nSoftVersionDateEx = 0;
            m_CanJmpScriptLableList = new Hashtable(StringComparer.OrdinalIgnoreCase);
            m_nKillMonExpMultiple = 1;
            m_nKillMonExpRate = 100;
            m_dwRateTick = HUtil32.GetTickCount();
            m_nPowerRate = 100;
            m_boSetStoragePwd = false;
            m_boReConfigPwd = false;
            m_boCheckOldPwd = false;
            m_boUnLockPwd = false;
            m_boUnLockStoragePwd = false;
            m_boPasswordLocked = false;
            // 锁仓库
            m_btPwdFailCount = 0;
            m_sTempPwd = "";
            m_sStoragePwd = "";
            m_boFilterSendMsg = false;
            m_boCanDeal = true;
            m_boCanDrop = true;
            m_boCanGetBackItem = true;
            m_boCanWalk = true;
            m_boCanRun = true;
            m_boCanHit = true;
            m_boCanSpell = true;
            m_boCanUseItem = true;
            m_nMemberType = 0;
            m_nMemberLevel = 0;
            m_nGameGold = 0;
            m_boDecGameGold = false;
            m_nDecGameGold = 1;
            m_dwDecGameGoldTick = HUtil32.GetTickCount();
            m_dwDecGameGoldTime = 60 * 1000;
            m_boIncGameGold = false;
            m_nIncGameGold = 1;
            m_dwIncGameGoldTick = HUtil32.GetTickCount();
            m_dwIncGameGoldTime = 60 * 1000;
            m_nGamePoint = 0;
            m_dwIncGamePointTick = HUtil32.GetTickCount();
            m_nPayMentPoint = 0;
            m_DearHuman = null;
            m_MasterHuman = null;
            m_MasterList = new List<PlayObject>();
            m_boSendMsgFlag = false;
            m_boChangeItemNameFlag = false;
            m_boCanMasterRecall = false;
            m_boCanDearRecall = false;
            m_dwDearRecallTick = HUtil32.GetTickCount();
            m_dwMasterRecallTick = HUtil32.GetTickCount();
            m_btReColorIdx = 0;
            m_GetWhisperHuman = null;
            OnHorse = false;
            m_wContribution = 0;
            m_sRankLevelName = M2Share.g_sRankLevelName;
            FixedHideMode = true;
            m_nVal = new int[100];
            m_nMval = new int[100];
            m_DyVal = new int[100];
            m_nSval = new string[100];
            m_nInteger = new int[100];
            m_sString = new string[100];
            m_ServerStrVal = new string[100];
            m_ServerIntVal = new int[100];
            m_dwAutoGetExpTick = HUtil32.GetTickCount();
            m_nAutoGetExpPoint = 0;
            m_AutoGetExpEnvir = null;
            m_dwHitIntervalTime = M2Share.Config.HitIntervalTime;// 攻击间隔
            m_dwMagicHitIntervalTime = M2Share.Config.MagicHitIntervalTime;// 魔法间隔
            m_dwRunIntervalTime = M2Share.Config.RunIntervalTime;// 走路间隔
            m_dwWalkIntervalTime = M2Share.Config.WalkIntervalTime;// 走路间隔
            m_dwTurnIntervalTime = M2Share.Config.TurnIntervalTime;// 换方向间隔
            m_dwActionIntervalTime = M2Share.Config.ActionIntervalTime;// 组合操作间隔
            m_dwRunLongHitIntervalTime = M2Share.Config.RunLongHitIntervalTime;// 组合操作间隔
            m_dwRunHitIntervalTime = M2Share.Config.RunHitIntervalTime;// 组合操作间隔
            m_dwWalkHitIntervalTime = M2Share.Config.WalkHitIntervalTime;// 组合操作间隔
            m_dwRunMagicIntervalTime = M2Share.Config.RunMagicIntervalTime;// 跑位魔法间隔
            m_DynamicVarList = new Dictionary<string, TDynamicVar>(StringComparer.OrdinalIgnoreCase);
            m_SessInfo = null;
            m_boTestSpeedMode = false;
            m_boLockLogon = true;
            m_boLockLogoned = false;
            m_boTimeGoto = false;
            m_dwTimeGotoTick = HUtil32.GetTickCount();
            m_sTimeGotoLable = "";
            m_TimeGotoNPC = null;
            AutoTimerTick = new int[20];
            AutoTimerStatus = new int[20];
            MapCell = CellType.Play;
            QueryExpireTick = 60 * 1000;
            AccountExpiredTick = HUtil32.GetTickCount();
            m_sRandomNo = M2Share.RandomNumber.Random(999999).ToString();
        }

        private void SendNotice()
        {
            var LoadList = new List<string>();
            M2Share.NoticeMgr.GetNoticeMsg("Notice", LoadList);
            var sNoticeMsg = string.Empty;
            if (LoadList.Count > 0)
            {
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sNoticeMsg = sNoticeMsg + LoadList[i] + "\x20\x1B";
                }
            }

            SendDefMessage(Grobal2.SM_SENDNOTICE, 2000, 0, 0, 0, sNoticeMsg.Replace("/r/n/r/n ", ""));
        }

        public void RunNotice()
        {
            ProcessMessage Msg = null;
            const string sExceptionMsg = "[Exception] TPlayObject::RunNotice";
            if (m_boEmergencyClose || m_boKickFlag || m_boSoftClose)
            {
                if (m_boKickFlag)
                {
                    SendDefMessage(Grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0, "");
                }
                MakeGhost();
            }
            else
            {
                try
                {
                    if (!m_boSendNotice)
                    {
                        SendNotice();
                        m_boSendNotice = true;
                        m_dwWaitLoginNoticeOKTick = HUtil32.GetTickCount();
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - m_dwWaitLoginNoticeOKTick) > 10 * 1000)
                        {
                            m_boEmergencyClose = true;
                        }
                        while (GetMessage(ref Msg))
                        {
                            if (Msg.wIdent == Grobal2.CM_LOGINNOTICEOK)
                            {
                                m_boLoginNoticeOK = true;
                                m_dwClientTick = (short)Msg.nParam1;
                                SysMsg(m_dwClientTick.ToString(), MsgColor.Red, MsgType.Notice);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    M2Share.Log.LogError(sExceptionMsg);
                }
            }
        }

        private void SendLogon()
        {
            var MessageBodyWL = new MessageBodyWL();
            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_LOGON, ActorId, CurrX, CurrY, HUtil32.MakeWord(Direction, (ushort)Light));
            MessageBodyWL.Param1 = GetFeatureToLong();
            MessageBodyWL.Param2 = CharStatus;
            if (AllowGroup)
            {
                MessageBodyWL.Tag1 = HUtil32.MakeLong(HUtil32.MakeWord(1, 0), GetFeatureEx());
            }
            else
            {
                MessageBodyWL.Tag1 = 0;
            }
            MessageBodyWL.Tag2 = 0;
            SendSocket(m_DefMsg, EDCode.EncodeBuffer(MessageBodyWL));
            var nRecog = GetFeatureToLong();
            SendDefMessage(Grobal2.SM_FEATURECHANGED, ActorId, HUtil32.LoWord(nRecog), HUtil32.HiWord(nRecog), GetFeatureEx(), "");
            SendDefMessage(Grobal2.SM_ATTACKMODE, (byte)AttatckMode, 0, 0, 0, "");
        }

        /// <summary>
        /// 玩家登录
        /// </summary>
        public void UserLogon()
        {
            UserItem UserItem;
            var sIPaddr = "127.0.0.1";
            const string sExceptionMsg = "[Exception] TPlayObject::UserLogon";
            const string sCheckIPaddrFail = "登录IP地址不匹配!!!";
            try
            {
                if (M2Share.Config.TestServer)
                {
                    if (Abil.Level < M2Share.Config.TestLevel)
                    {
                        Abil.Level = (byte)M2Share.Config.TestLevel;
                    }
                    if (Gold < M2Share.Config.TestGold)
                    {
                        Gold = M2Share.Config.TestGold;
                    }
                }
                if (M2Share.Config.TestServer || M2Share.Config.ServiceMode)
                {
                    PayMent = 3;
                }
                MapMoveTick = HUtil32.GetTickCount();
                m_dLogonTime = DateTime.Now;
                m_dwLogonTick = HUtil32.GetTickCount();
                Initialize();
                SendMsg(this, Grobal2.RM_LOGON, 0, 0, 0, 0, "");
                if (Abil.Level <= 7)
                {
                    if (GetRangeHumanCount() >= 80)
                    {
                        MapRandomMove(Envir.MapName, 0);
                    }
                }
                if (m_boDieInFight3Zone)
                {
                    MapRandomMove(Envir.MapName, 0);
                }
                if (M2Share.WorldEngine.GetHumPermission(ChrName, ref sIPaddr, ref Permission))
                {
                    if (M2Share.Config.PermissionSystem)
                    {
                        if (!M2Share.CompareIPaddr(m_sIPaddr, sIPaddr))
                        {
                            SysMsg(sCheckIPaddrFail, MsgColor.Red, MsgType.Hint);
                            m_boEmergencyClose = true;
                        }
                    }
                }
                GetStartPoint();
                for (var i = MagicList.Count - 1; i >= 0; i--)
                {
                    CheckSeeHealGauge(MagicList[i]);
                }
                if (m_boNewHuman)
                {
                    UserItem = new UserItem();
                    if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.Candle, ref UserItem))
                    {
                        ItemList.Add(UserItem);
                    }
                    else
                    {
                        Dispose(UserItem);
                    }
                    UserItem = new UserItem();
                    if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.BasicDrug, ref UserItem))
                    {
                        ItemList.Add(UserItem);
                    }
                    else
                    {
                        Dispose(UserItem);
                    }
                    UserItem = new UserItem();
                    if (M2Share.WorldEngine.CopyToUserItemFromName(M2Share.Config.WoodenSword, ref UserItem))
                    {
                        ItemList.Add(UserItem);
                    }
                    else
                    {
                        Dispose(UserItem);
                    }
                    UserItem = new UserItem();
                    var sItem = Gender == PlayGender.Man
                        ? M2Share.Config.ClothsMan
                        : M2Share.Config.ClothsWoman;
                    if (M2Share.WorldEngine.CopyToUserItemFromName(sItem, ref UserItem))
                    {
                        ItemList.Add(UserItem);
                    }
                    else
                    {
                        Dispose(UserItem);
                    }
                }
                // 检查背包中的物品是否合法
                for (var i = ItemList.Count - 1; i >= 0; i--)
                {
                    UserItem = ItemList[i];
                    if (!string.IsNullOrEmpty(M2Share.WorldEngine.GetStdItemName(UserItem.Index))) continue;
                    Dispose(ItemList[i]);
                    ItemList.RemoveAt(i);
                }
                // 检查人物身上的物品是否符合使用规则
                if (M2Share.Config.CheckUserItemPlace)
                {
                    for (var i = 0; i < UseItems.Length; i++)
                    {
                        if (UseItems[i] == null || UseItems[i].Index <= 0) continue;
                        var StdItem = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                        if (StdItem != null)
                        {
                            if (!M2Share.CheckUserItems(i, StdItem))
                            {
                                UserItem = UseItems[i];
                                if (!AddItemToBag(UserItem))
                                {
                                    ItemList.Insert(0, UserItem);
                                }
                                UseItems[i].Index = 0;
                            }
                        }
                        else
                        {
                            UseItems[i].Index = 0;
                        }
                    }
                }
                // 检查背包中是否有复制品
                for (var i = ItemList.Count - 1; i >= 0; i--)
                {
                    UserItem = ItemList[i];
                    var sItemName = M2Share.WorldEngine.GetStdItemName(UserItem.Index);
                    for (var j = i - 1; j >= 0; j--)
                    {
                        var UserItem1 = ItemList[j];
                        if (M2Share.WorldEngine.GetStdItemName(UserItem1.Index) == sItemName && UserItem.MakeIndex == UserItem1.MakeIndex)
                        {
                            ItemList.RemoveAt(j);
                            break;
                        }
                    }
                }
                for (var i = 0; i < StatusArrTick.Length; i++)
                {
                    if (StatusArr[i] > 0)
                    {
                        StatusArrTick[i] = HUtil32.GetTickCount();
                    }
                }
                CharStatus = GetCharStatus();
                RecalcLevelAbilitys();
                RecalcAbilitys();
                Abil.MaxExp = GetLevelExp(Abil.Level);// 登录重新取得升级所需经验值
                WAbil.Exp = Abil.Exp;
                WAbil.MaxExp = Abil.MaxExp;
                if (BtB2 == 0)
                {
                    PkPoint = 0;
                    BtB2++;
                }
                if (Gold > M2Share.Config.HumanMaxGold * 2 && M2Share.Config.HumanMaxGold > 0)
                {
                    Gold = M2Share.Config.HumanMaxGold * 2;
                }
                if (!bo6AB)
                {
                    if (m_nSoftVersionDate < M2Share.Config.SoftVersionDate) //登录版本号验证
                    {
                        SysMsg(M2Share.sClientSoftVersionError, MsgColor.Red, MsgType.Hint);
                        SysMsg(M2Share.sDownLoadNewClientSoft, MsgColor.Red, MsgType.Hint);
                        SysMsg(M2Share.sForceDisConnect, MsgColor.Red, MsgType.Hint);
                        m_boEmergencyClose = true;
                        return;
                    }
                    if (m_nSoftVersionDateEx == 0 && M2Share.Config.boOldClientShowHiLevel)
                    {
                        SysMsg(M2Share.sClientSoftVersionTooOld, MsgColor.Blue, MsgType.Hint);
                        SysMsg(M2Share.sDownLoadAndUseNewClient, MsgColor.Red, MsgType.Hint);
                        if (!M2Share.Config.CanOldClientLogon)
                        {
                            SysMsg(M2Share.sClientSoftVersionError, MsgColor.Red, MsgType.Hint);
                            SysMsg(M2Share.sDownLoadNewClientSoft, MsgColor.Red, MsgType.Hint);
                            SysMsg(M2Share.sForceDisConnect, MsgColor.Red, MsgType.Hint);
                            m_boEmergencyClose = true;
                            return;
                        }
                    }
                    switch (AttatckMode)
                    {
                        case AttackMode.HAM_ALL:// [攻击模式: 全体攻击]
                            SysMsg(M2Share.sAttackModeOfAll, MsgColor.Green, MsgType.Hint);
                            break;
                        case AttackMode.HAM_PEACE:// [攻击模式: 和平攻击]
                            SysMsg(M2Share.sAttackModeOfPeaceful, MsgColor.Green, MsgType.Hint);
                            break;
                        case AttackMode.HAM_DEAR:// [攻击模式: 和平攻击]
                            SysMsg(M2Share.sAttackModeOfDear, MsgColor.Green, MsgType.Hint);
                            break;
                        case AttackMode.HAM_MASTER:// [攻击模式: 和平攻击]
                            SysMsg(M2Share.sAttackModeOfMaster, MsgColor.Green, MsgType.Hint);
                            break;
                        case AttackMode.HAM_GROUP:// [攻击模式: 编组攻击]
                            SysMsg(M2Share.sAttackModeOfGroup, MsgColor.Green, MsgType.Hint);
                            break;
                        case AttackMode.HAM_GUILD:// [攻击模式: 行会攻击]
                            SysMsg(M2Share.sAttackModeOfGuild, MsgColor.Green, MsgType.Hint);
                            break;
                        case AttackMode.HAM_PKATTACK:// [攻击模式: 红名攻击]
                            SysMsg(M2Share.sAttackModeOfRedWhite, MsgColor.Green, MsgType.Hint);
                            break;
                    }
                    SysMsg(M2Share.sStartChangeAttackModeHelp, MsgColor.Green, MsgType.Hint);// 使用组合快捷键 CTRL-H 更改攻击...
                    if (M2Share.Config.TestServer)
                    {
                        SysMsg(M2Share.sStartNoticeMsg, MsgColor.Green, MsgType.Hint);// 欢迎进入本服务器进行游戏...
                    }
                    if (M2Share.WorldEngine.PlayObjectCount > M2Share.Config.TestUserLimit)
                    {
                        if (Permission < 2)
                        {
                            SysMsg(M2Share.sOnlineUserFull, MsgColor.Red, MsgType.Hint);
                            SysMsg(M2Share.sForceDisConnect, MsgColor.Red, MsgType.Hint);
                            m_boEmergencyClose = true;
                        }
                    }
                }
                m_btBright = (byte)M2Share.g_nGameTime;
                SendMsg(this, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                SendMsg(this, Grobal2.RM_SUBABILITY, 0, 0, 0, 0, "");
                SendMsg(this, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                SendMsg(this, Grobal2.RM_DAYCHANGING, 0, 0, 0, 0, "");
                SendMsg(this, Grobal2.RM_SENDUSEITEMS, 0, 0, 0, 0, "");
                SendMsg(this, Grobal2.RM_SENDMYMAGIC, 0, 0, 0, 0, "");
                MyGuild = M2Share.GuildMgr.MemberOfGuild(ChrName);
                if (MyGuild != null)
                {
                    GuildRankName = MyGuild.GetRankName(this, ref GuildRankNo);
                    for (var i = MyGuild.GuildWarList.Count - 1; i >= 0; i--)
                    {
                        SysMsg(MyGuild.GuildWarList[i] + " 正在与本行会进行行会战.", MsgColor.Green, MsgType.Hint);
                    }
                }
                RefShowName();
                if (PayMent == 1)
                {
                    if (!bo6AB)
                    {
                        SysMsg(M2Share.sYouNowIsTryPlayMode, MsgColor.Red, MsgType.Hint);
                    }
                    GoldMax = M2Share.Config.HumanTryModeMaxGold;
                    if (Abil.Level > M2Share.Config.TryModeLevel)
                    {
                        SysMsg("测试状态可以使用到第 " + M2Share.Config.TryModeLevel, MsgColor.Red, MsgType.Hint);
                        SysMsg("链接中断，请到以下地址获得收费相关信息。(http://www.mir2.com)", MsgColor.Red, MsgType.Hint);
                        m_boEmergencyClose = true;
                    }
                }
                if (PayMent == 3 && !bo6AB)
                {
                    SysMsg(M2Share.g_sNowIsFreePlayMode, MsgColor.Green, MsgType.Hint);
                }
                if (M2Share.Config.VentureServer)
                {
                    SysMsg("当前服务器运行于冒险模式.", MsgColor.Green, MsgType.Hint);
                }
                if (MagicArr[MagicConst.SKILL_ERGUM] != null && !UseThrusting)
                {
                    UseThrusting = true;
                    SendSocket("+LNG");
                }
                if (Envir.Flag.boNORECONNECT)
                {
                    MapRandomMove(Envir.Flag.sNoReConnectMap, 0);
                }
                if (CheckDenyLogon())// 如果人物在禁止登录列表里则直接掉线而不执行下面内容
                {
                    return;
                }
                if (M2Share.g_ManageNPC != null)
                {
                    M2Share.g_ManageNPC.GotoLable(this, "@Login", false);
                }
                FixedHideMode = false;
                if (!string.IsNullOrEmpty(m_sDearName))
                {
                    CheckMarry();
                }
                CheckMaster();
                m_boFilterSendMsg = M2Share.GetDisableSendMsgList(ChrName);
                // 密码保护系统
                if (M2Share.Config.PasswordLockSystem)
                {
                    if (m_boPasswordLocked)
                    {
                        m_boCanGetBackItem = !M2Share.Config.LockGetBackItemAction;
                    }
                    if (M2Share.Config.LockHumanLogin && m_boLockLogon && m_boPasswordLocked)
                    {
                        m_boCanDeal = !M2Share.Config.LockDealAction;
                        m_boCanDrop = !M2Share.Config.LockDropAction;
                        m_boCanUseItem = !M2Share.Config.LockUserItemAction;
                        m_boCanWalk = !M2Share.Config.LockWalkAction;
                        m_boCanRun = !M2Share.Config.LockRunAction;
                        m_boCanHit = !M2Share.Config.LockHitAction;
                        m_boCanSpell = !M2Share.Config.LockSpellAction;
                        m_boCanSendMsg = !M2Share.Config.LockSendMsgAction;
                        ObMode = M2Share.Config.LockInObModeAction;
                        AdminMode = M2Share.Config.LockInObModeAction;
                        SysMsg(M2Share.g_sActionIsLockedMsg + " 开锁命令: @" + CommandMgr.Commands.LockLogon.CmdName, MsgColor.Red, MsgType.Hint);
                        SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, M2Share.g_sActionIsLockedMsg + "\\ \\" + "密码命令: @" + CommandMgr.Commands.PasswordLock.CmdName);
                    }
                    if (!m_boPasswordLocked)
                    {
                        SysMsg(Format(M2Share.g_sPasswordNotSetMsg, CommandMgr.Commands.PasswordLock.CmdName), MsgColor.Red, MsgType.Hint);
                    }
                    if (!m_boLockLogon && m_boPasswordLocked)
                    {
                        SysMsg(Format(M2Share.g_sNotPasswordProtectMode, CommandMgr.Commands.LockLogon.CmdName), MsgColor.Red, MsgType.Hint);
                    }
                    SysMsg(M2Share.g_sActionIsLockedMsg + " 开锁命令: @" + CommandMgr.Commands.Unlock.CmdName, MsgColor.Red, MsgType.Hint);
                    SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, M2Share.g_sActionIsLockedMsg + "\\ \\" + "开锁命令: @" + CommandMgr.Commands.Unlock.CmdName + '\\' + "加锁命令: @" + CommandMgr.Commands.Lock.CmdName + '\\' + "设置密码命令: @" + CommandMgr.Commands.SetPassword.CmdName + '\\' + "修改密码命令: @" + CommandMgr.Commands.ChgPassword.CmdName);
                }
                // 重置泡点方面计时
                m_dwIncGamePointTick = HUtil32.GetTickCount();
                m_dwIncGameGoldTick = HUtil32.GetTickCount();
                m_dwAutoGetExpTick = HUtil32.GetTickCount();
                GetSellOffGlod();// 检查是否有元宝寄售交易结束还没得到元宝
            }
            catch (Exception e)
            {
                M2Share.Log.LogError(sExceptionMsg);
                M2Share.Log.LogError(e.StackTrace);
            }
            // ReadAllBook();
        }

        /// <summary>
        /// 使用祝福油
        /// </summary>
        /// <returns></returns>
        private bool WeaptonMakeLuck()
        {
            if (UseItems[Grobal2.U_WEAPON] == null && UseItems[Grobal2.U_WEAPON].Index <= 0)
            {
                return false;
            }
            var nRand = 0;
            var StdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_WEAPON].Index);
            if (StdItem != null)
            {
                nRand = Math.Abs(HUtil32.HiByte(StdItem.DC) - HUtil32.LoByte(StdItem.DC)) / 5;
            }
            if (M2Share.RandomNumber.Random(M2Share.Config.WeaponMakeUnLuckRate) == 1)
            {
                MakeWeaponUnlock();
            }
            else
            {
                var boMakeLuck = false;
                if (UseItems[Grobal2.U_WEAPON].Desc[4] > 0)
                {
                    UseItems[Grobal2.U_WEAPON].Desc[4] -= 1;
                    SysMsg(M2Share.g_sWeaptonMakeLuck, MsgColor.Green, MsgType.Hint);
                    boMakeLuck = true;
                }
                else if (UseItems[Grobal2.U_WEAPON].Desc[3] < M2Share.Config.WeaponMakeLuckPoint1)
                {
                    UseItems[Grobal2.U_WEAPON].Desc[3]++;
                    SysMsg(M2Share.g_sWeaptonMakeLuck, MsgColor.Green, MsgType.Hint);
                    boMakeLuck = true;
                }
                else if (UseItems[Grobal2.U_WEAPON].Desc[3] < M2Share.Config.WeaponMakeLuckPoint2 && M2Share.RandomNumber.Random(nRand + M2Share.Config.WeaponMakeLuckPoint2Rate) == 1)
                {
                    UseItems[Grobal2.U_WEAPON].Desc[3]++;
                    SysMsg(M2Share.g_sWeaptonMakeLuck, MsgColor.Green, MsgType.Hint);
                    boMakeLuck = true;
                }
                else if (UseItems[Grobal2.U_WEAPON].Desc[3] < M2Share.Config.WeaponMakeLuckPoint3 && M2Share.RandomNumber.Random(nRand * M2Share.Config.WeaponMakeLuckPoint3Rate) == 1)
                {
                    UseItems[Grobal2.U_WEAPON].Desc[3]++;
                    SysMsg(M2Share.g_sWeaptonMakeLuck, MsgColor.Green, MsgType.Hint);
                    boMakeLuck = true;
                }
                if (Race == ActorRace.Play)
                {
                    RecalcAbilitys();
                    SendMsg(this, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                    SendMsg(this, Grobal2.RM_SUBABILITY, 0, 0, 0, 0, "");
                }
                if (!boMakeLuck)
                {
                    SysMsg(M2Share.g_sWeaptonNotMakeLuck, MsgColor.Green, MsgType.Hint);
                }
            }
            return true;
        }

        /// <summary>
        /// 修复武器
        /// </summary>
        /// <returns></returns>
        private bool RepairWeapon()
        {
            if (UseItems[Grobal2.U_WEAPON] == null)
            {
                return false;
            }
            var UserItem = UseItems[Grobal2.U_WEAPON];
            if (UserItem.Index <= 0 || UserItem.DuraMax <= UserItem.Dura)
            {
                return false;
            }
            UserItem.DuraMax -= (ushort)((UserItem.DuraMax - UserItem.Dura) / M2Share.Config.RepairItemDecDura);
            var nDura = HUtil32._MIN(5000, UserItem.DuraMax - UserItem.Dura);
            if (nDura <= 0) return false;
            UserItem.Dura += (ushort)nDura;
            SendMsg(this, Grobal2.RM_DURACHANGE, 1, UserItem.Dura, UserItem.DuraMax, 0, "");
            SysMsg(M2Share.g_sWeaponRepairSuccess, MsgColor.Green, MsgType.Hint);
            return true;
        }

        /// <summary>
        /// 特修武器
        /// </summary>
        /// <returns></returns>
        private bool SuperRepairWeapon()
        {
            if (UseItems[Grobal2.U_WEAPON] == null && UseItems[Grobal2.U_WEAPON].Index <= 0)
            {
                return false;
            }
            UseItems[Grobal2.U_WEAPON].Dura = UseItems[Grobal2.U_WEAPON].DuraMax;
            SendMsg(this, Grobal2.RM_DURACHANGE, 1, UseItems[Grobal2.U_WEAPON].Dura, UseItems[Grobal2.U_WEAPON].DuraMax, 0, "");
            SysMsg(M2Share.g_sWeaponRepairSuccess, MsgColor.Green, MsgType.Hint);
            return true;
        }

        private void WinLottery()
        {
            var nGold = 0;
            var nWinLevel = 0;
            var nRate = M2Share.RandomNumber.Random(M2Share.Config.WinLotteryRate);
            if (nRate >= M2Share.Config.WinLottery6Min && nRate <= M2Share.Config.WinLottery6Max)
            {
                if (M2Share.Config.WinLotteryCount < M2Share.Config.NoWinLotteryCount)
                {
                    nGold = M2Share.Config.WinLottery6Gold;
                    nWinLevel = 6;
                    M2Share.Config.WinLotteryLevel6++;
                }
            }
            else if (nRate >= M2Share.Config.WinLottery5Min && nRate <= M2Share.Config.WinLottery5Max)
            {
                if (M2Share.Config.WinLotteryCount < M2Share.Config.NoWinLotteryCount)
                {
                    nGold = M2Share.Config.WinLottery5Gold;
                    nWinLevel = 5;
                    M2Share.Config.WinLotteryLevel5++;
                }
            }
            else if (nRate >= M2Share.Config.WinLottery4Min && nRate <= M2Share.Config.WinLottery4Max)
            {
                if (M2Share.Config.WinLotteryCount < M2Share.Config.NoWinLotteryCount)
                {
                    nGold = M2Share.Config.WinLottery4Gold;
                    nWinLevel = 4;
                    M2Share.Config.WinLotteryLevel4++;
                }
            }
            else if (nRate >= M2Share.Config.WinLottery3Min && nRate <= M2Share.Config.WinLottery3Max)
            {
                if (M2Share.Config.WinLotteryCount < M2Share.Config.NoWinLotteryCount)
                {
                    nGold = M2Share.Config.WinLottery3Gold;
                    nWinLevel = 3;
                    M2Share.Config.WinLotteryLevel3++;
                }
            }
            else if (nRate >= M2Share.Config.WinLottery2Min && nRate <= M2Share.Config.WinLottery2Max)
            {
                if (M2Share.Config.WinLotteryCount < M2Share.Config.NoWinLotteryCount)
                {
                    nGold = M2Share.Config.WinLottery2Gold;
                    nWinLevel = 2;
                    M2Share.Config.WinLotteryLevel2++;
                }
            }
            else if (M2Share.Config.WinLottery1Min + M2Share.Config.WinLottery1Max == nRate)
            {
                if (M2Share.Config.WinLotteryCount < M2Share.Config.NoWinLotteryCount)
                {
                    nGold = M2Share.Config.WinLottery1Gold;
                    nWinLevel = 1;
                    M2Share.Config.WinLotteryLevel1++;
                }
            }
            if (nGold > 0)
            {
                switch (nWinLevel)
                {
                    case 1:
                        SysMsg(M2Share.g_sWinLottery1Msg, MsgColor.Green, MsgType.Hint);
                        break;
                    case 2:
                        SysMsg(M2Share.g_sWinLottery2Msg, MsgColor.Green, MsgType.Hint);
                        break;
                    case 3:
                        SysMsg(M2Share.g_sWinLottery3Msg, MsgColor.Green, MsgType.Hint);
                        break;
                    case 4:
                        SysMsg(M2Share.g_sWinLottery4Msg, MsgColor.Green, MsgType.Hint);
                        break;
                    case 5:
                        SysMsg(M2Share.g_sWinLottery5Msg, MsgColor.Green, MsgType.Hint);
                        break;
                    case 6:
                        SysMsg(M2Share.g_sWinLottery6Msg, MsgColor.Green, MsgType.Hint);
                        break;
                }
                if (IncGold(nGold))
                {
                    GoldChanged();
                }
                else
                {
                    DropGoldDown(nGold, true, null, null);
                }
            }
            else
            {
                M2Share.Config.NoWinLotteryCount += 500;
                SysMsg(M2Share.g_sNotWinLotteryMsg, MsgColor.Red, MsgType.Hint);
            }
        }

        public override void RecalcAbilitys()
        {
            base.RecalcAbilitys();
            RecalcAdjusBonus();
        }

        protected override void UpdateVisibleGay(BaseObject BaseObject)
        {
            var boIsVisible = false;
            VisibleBaseObject VisibleBaseObject;
            if (BaseObject.Race == ActorRace.Play || BaseObject.Master != null)
            {
                IsVisibleActive = true;// 如果是人物或宝宝则置TRUE
            }
            for (var i = 0; i < VisibleActors.Count; i++)
            {
                VisibleBaseObject = VisibleActors[i];
                if (VisibleBaseObject.BaseObject == BaseObject)
                {
                    VisibleBaseObject.VisibleFlag = VisibleFlag.Invisible;
                    boIsVisible = true;
                    break;
                }
            }
            if (boIsVisible)
            {
                return;
            }
            VisibleBaseObject = new VisibleBaseObject
            {
                VisibleFlag = VisibleFlag.Hidden,
                BaseObject = BaseObject
            };
            VisibleActors.Add(VisibleBaseObject);
            if (BaseObject.Race == ActorRace.Play)
            {
                SendWhisperMsg(BaseObject as PlayObject);
            }
        }

        public override void SearchViewRange()
        {
            for (var i = VisibleItems.Count - 1; i >= 0; i--)
            {
                VisibleItems[i].VisibleFlag = 0;
            }
            for (var i = VisibleEvents.Count - 1; i >= 0; i--)
            {
                VisibleEvents[i].VisibleFlag = 0;
            }
            for (var i = VisibleActors.Count - 1; i >= 0; i--)
            {
                VisibleActors[i].VisibleFlag = 0;
            }
            var nStartX = (short)(CurrX - ViewRange);
            var nEndX = (short)(CurrX + ViewRange);
            var nStartY = (short)(CurrY - ViewRange);
            var nEndY = (short)(CurrY + ViewRange);
            try
            {
                for (var nX = nStartX; nX <= nEndX; nX++)
                {
                    for (var nY = nStartY; nY <= nEndY; nY++)
                    {
                        var cellSuccess = false;
                        var cellInfo = Envir.GetCellInfo(nX, nY, ref cellSuccess);
                        if (cellSuccess && cellInfo.IsAvailable)
                        {
                            var nIdx = 0;
                            while (true)
                            {
                                if (cellInfo.Count <= nIdx)
                                {
                                    break;
                                }
                                var cellObject = cellInfo.ObjList[nIdx];
                                if (cellObject != null)
                                {
                                    if (cellObject.ActorObject)
                                    {
                                        if ((HUtil32.GetTickCount() - cellObject.AddTime) >= 60 * 1000)
                                        {
                                            cellInfo.Remove(cellObject);
                                            if (cellInfo.Count > 0)
                                            {
                                                continue;
                                            }
                                            cellInfo.Dispose();
                                            break;
                                        }
                                        BaseObject BaseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                                        if (BaseObject != null && !BaseObject.Invisible)
                                        {
                                            if (!BaseObject.Ghost && !BaseObject.FixedHideMode && !BaseObject.ObMode)
                                            {
                                                if (Race < ActorRace.Animal || Master != null || CrazyMode || NastyMode || WantRefMsg || BaseObject.Master != null && Math.Abs(BaseObject.CurrX - CurrX) <= 3 && Math.Abs(BaseObject.CurrY - CurrY) <= 3 || BaseObject.Race == ActorRace.Play)
                                                {
                                                    UpdateVisibleGay(BaseObject);
                                                    if (BaseObject.MapCell == CellType.Monster && this.MapCell == CellType.Play && !this.ObMode && !BaseObject.FixedHideMode)
                                                    {
                                                        BaseObject.UpdateMonsterVisible(this);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (Race == ActorRace.Play)
                                    {
                                        if (cellObject.CellType == CellType.Item)
                                        {
                                            if ((HUtil32.GetTickCount() - cellObject.AddTime) > M2Share.Config.ClearDropOnFloorItemTime)// 60 * 60 * 1000
                                            {
                                                cellInfo.Remove(cellObject);
                                                if (cellInfo.Count > 0)
                                                {
                                                    continue;
                                                }
                                                cellInfo.Dispose();
                                                break;
                                            }
                                            var mapItem = (MapItem)M2Share.CellObjectSystem.Get(cellObject.CellObjId); ;
                                            UpdateVisibleItem(nX, nY, mapItem);
                                            if (mapItem.OfBaseObject > 0 || mapItem.DropBaseObject > 0)
                                            {
                                                if ((HUtil32.GetTickCount() - mapItem.CanPickUpTick) > M2Share.Config.FloorItemCanPickUpTime) // 2 * 60 * 1000
                                                {
                                                    mapItem.OfBaseObject = 0;
                                                    mapItem.DropBaseObject = 0;
                                                }
                                                else
                                                {
                                                    if (M2Share.ActorMgr.Get(mapItem.OfBaseObject) != null)
                                                    {
                                                        if (M2Share.ActorMgr.Get(mapItem.OfBaseObject).Ghost)
                                                        {
                                                            mapItem.OfBaseObject = 0;
                                                        }
                                                    }
                                                    if (M2Share.ActorMgr.Get(mapItem.DropBaseObject) != null)
                                                    {
                                                        if (M2Share.ActorMgr.Get(mapItem.DropBaseObject).Ghost)
                                                        {
                                                            mapItem.DropBaseObject = 0;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (cellObject.CellType == CellType.Event)
                                        {
                                            EventInfo MapEvent = (EventInfo)M2Share.CellObjectSystem.Get(cellObject.CellObjId);
                                            if (MapEvent.Visible)
                                            {
                                                UpdateVisibleEvent(nX, nY, MapEvent);
                                            }
                                        }
                                    }
                                }
                                nIdx++;
                            }
                        }
                    }
                }
                var n18 = 0;
                while (true)
                {
                    if (VisibleActors.Count <= n18)
                    {
                        break;
                    }
                    var VisibleBaseObject = VisibleActors[n18];
                    if (VisibleBaseObject.VisibleFlag == 0)
                    {
                        if (Race == ActorRace.Play)
                        {
                            var BaseObject = VisibleBaseObject.BaseObject;
                            if (!BaseObject.FixedHideMode && !BaseObject.Ghost)//防止人物退出时发送重复的消息占用带宽，人物进入隐身模式时人物不消失问题
                            {
                                SendMsg(BaseObject, Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
                            }
                        }
                        VisibleActors.RemoveAt(n18);
                        Dispose(VisibleBaseObject);
                        continue;
                    }
                    if (Race == ActorRace.Play && VisibleBaseObject.VisibleFlag == VisibleFlag.Hidden)
                    {
                        var BaseObject = VisibleBaseObject.BaseObject;
                        if (BaseObject != this)
                        {
                            if (BaseObject.Death)
                            {
                                if (BaseObject.Skeleton)
                                {
                                    SendMsg(BaseObject, Grobal2.RM_SKELETON, BaseObject.Direction, BaseObject.CurrX, BaseObject.CurrY, 0, "");
                                }
                                else
                                {
                                    SendMsg(BaseObject, Grobal2.RM_DEATH, BaseObject.Direction, BaseObject.CurrX, BaseObject.CurrY, 0, "");
                                }
                            }
                            else
                            {
                                SendMsg(BaseObject, Grobal2.RM_TURN, BaseObject.Direction, BaseObject.CurrX, BaseObject.CurrY, 0, BaseObject.GetShowName());
                            }
                        }
                    }
                    n18++;
                }

                var I = 0;
                while (true)
                {
                    if (VisibleItems.Count <= I)
                    {
                        break;
                    }
                    var VisibleMapItem = VisibleItems[I];
                    if (VisibleMapItem.VisibleFlag == 0)
                    {
                        SendMsg(this, Grobal2.RM_ITEMHIDE, 0, VisibleMapItem.MapItem.ActorId, VisibleMapItem.nX, VisibleMapItem.nY, "");
                        VisibleItems.RemoveAt(I);
                        Dispose(VisibleMapItem);
                        continue;
                    }
                    if (VisibleMapItem.VisibleFlag == VisibleFlag.Hidden)
                    {
                        SendMsg(this, Grobal2.RM_ITEMSHOW, VisibleMapItem.wLooks, VisibleMapItem.MapItem.ActorId, VisibleMapItem.nX, VisibleMapItem.nY, VisibleMapItem.sName);
                    }
                    I++;
                }
                I = 0;
                while (true)
                {
                    if (VisibleEvents.Count <= I)
                    {
                        break;
                    }
                    EventInfo MapEvent = VisibleEvents[I];
                    if (MapEvent.VisibleFlag == VisibleFlag.Visible)
                    {
                        SendMsg(this, Grobal2.RM_HIDEEVENT, 0, MapEvent.Id, MapEvent.nX, MapEvent.nY, "");
                        VisibleEvents.RemoveAt(I);
                        continue;
                    }
                    if (MapEvent.VisibleFlag == VisibleFlag.Hidden)
                    {
                        SendMsg(this, Grobal2.RM_SHOWEVENT, (short)MapEvent.EventType, MapEvent.Id, HUtil32.MakeLong(MapEvent.nX, (short)MapEvent.EventParam), MapEvent.nY, "");
                    }
                    I++;
                }
            }
            catch (Exception e)
            {
                M2Share.Log.LogError(e.StackTrace);
                KickException();
            }
        }

        /// <summary>
        /// 显示玩家名字
        /// </summary>
        /// <returns></returns>
        public override string GetShowName()
        {
            var result = string.Empty;
            string sShowName;
            var sChrName = string.Empty;
            var sGuildName = string.Empty;
            var sDearName = string.Empty;
            var sMasterName = string.Empty;
            const string sExceptionMsg = "[Exception] TPlayObject::GetShowName";
            try
            {
                if (MyGuild != null)
                {
                    var Castle = M2Share.CastleMgr.IsCastleMember(this);
                    if (Castle != null)
                    {
                        sGuildName = M2Share.g_sCastleGuildName.Replace("%castlename", Castle.sName);
                        sGuildName = sGuildName.Replace("%guildname", MyGuild.sGuildName);
                        sGuildName = sGuildName.Replace("%rankname", GuildRankName);
                    }
                    else
                    {
                        Castle = M2Share.CastleMgr.InCastleWarArea(this);// 01/25 多城堡
                        if (M2Share.Config.ShowGuildName || Castle != null && Castle.UnderWar || InFreePkArea)
                        {
                            sGuildName = M2Share.g_sNoCastleGuildName.Replace("%guildname", MyGuild.sGuildName);
                            sGuildName = sGuildName.Replace("%rankname", GuildRankName);
                        }
                    }
                }
                if (!M2Share.Config.ShowRankLevelName)
                {
                    if (m_btReLevel > 0)
                    {
                        switch (Job)
                        {
                            case PlayJob.Warrior:
                                sChrName = M2Share.g_sWarrReNewName.Replace("%chrname", ChrName);
                                break;
                            case PlayJob.Wizard:
                                sChrName = M2Share.g_sWizardReNewName.Replace("%chrname", ChrName);
                                break;
                            case PlayJob.Taoist:
                                sChrName = M2Share.g_sTaosReNewName.Replace("%chrname", ChrName);
                                break;
                        }
                    }
                    else
                    {
                        sChrName = ChrName;
                    }
                }
                else
                {
                    sChrName = Format(m_sRankLevelName, ChrName);
                }
                if (!string.IsNullOrEmpty(m_sMasterName))
                {
                    if (m_boMaster)
                    {
                        sMasterName = Format(M2Share.g_sMasterName, m_sMasterName);
                    }
                    else
                    {
                        sMasterName = Format(M2Share.g_sNoMasterName, m_sMasterName);
                    }
                }
                if (!string.IsNullOrEmpty(m_sDearName))
                {
                    if (Gender == PlayGender.Man)
                    {
                        sDearName = Format(M2Share.g_sManDearName, m_sDearName);
                    }
                    else
                    {
                        sDearName = Format(M2Share.g_sWoManDearName, m_sDearName);
                    }
                }
                sShowName = M2Share.g_sHumanShowName.Replace("%chrname", sChrName);
                sShowName = sShowName.Replace("%guildname", sGuildName);
                sShowName = sShowName.Replace("%dearname", sDearName);
                sShowName = sShowName.Replace("%mastername", sMasterName);
                result = sShowName;
            }
            catch (Exception e)
            {
                M2Share.Log.LogError(sExceptionMsg);
                M2Share.Log.LogError(e.Message);
            }
            return result;
        }

        public override void MakeGhost()
        {
            string sSayMsg;
            PlayObject Human;
            const string sExceptionMsg = "[Exception] TPlayObject::MakeGhost";
            try
            {
                if (M2Share.g_HighLevelHuman == this)
                {
                    M2Share.g_HighLevelHuman = null;
                }
                if (M2Share.g_HighPKPointHuman == this)
                {
                    M2Share.g_HighPKPointHuman = null;
                }
                if (M2Share.g_HighDCHuman == this)
                {
                    M2Share.g_HighDCHuman = null;
                }
                if (M2Share.g_HighMCHuman == this)
                {
                    M2Share.g_HighMCHuman = null;
                }
                if (M2Share.g_HighSCHuman == this)
                {
                    M2Share.g_HighSCHuman = null;
                }
                if (M2Share.g_HighOnlineHuman == this)
                {
                    M2Share.g_HighOnlineHuman = null;
                }
                // 人物下线后通知配偶，并把对方的相关记录清空
                if (m_DearHuman != null)
                {
                    if (Gender == PlayGender.Man)
                    {
                        sSayMsg = M2Share.g_sManLongOutDearOnlineMsg.Replace("%d", m_sDearName);
                        sSayMsg = sSayMsg.Replace("%s", ChrName);
                        sSayMsg = sSayMsg.Replace("%m", Envir.MapDesc);
                        sSayMsg = sSayMsg.Replace("%x", CurrX.ToString());
                        sSayMsg = sSayMsg.Replace("%y", CurrY.ToString());
                        m_DearHuman.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                    }
                    else
                    {
                        sSayMsg = M2Share.g_sWoManLongOutDearOnlineMsg.Replace("%d", m_sDearName);
                        sSayMsg = sSayMsg.Replace("%s", ChrName);
                        sSayMsg = sSayMsg.Replace("%m", Envir.MapDesc);
                        sSayMsg = sSayMsg.Replace("%x", CurrX.ToString());
                        sSayMsg = sSayMsg.Replace("%y", CurrY.ToString());
                        m_DearHuman.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                    }
                    m_DearHuman.m_DearHuman = null;
                    m_DearHuman = null;
                }
                if (m_MasterHuman != null || m_MasterList.Count > 0)
                {
                    if (m_boMaster)
                    {
                        for (var i = m_MasterList.Count - 1; i >= 0; i--)
                        {
                            Human = m_MasterList[i];
                            sSayMsg = M2Share.g_sMasterLongOutMasterListOnlineMsg.Replace("%s", ChrName);
                            sSayMsg = sSayMsg.Replace("%m", Envir.MapDesc);
                            sSayMsg = sSayMsg.Replace("%x", CurrX.ToString());
                            sSayMsg = sSayMsg.Replace("%y", CurrY.ToString());
                            Human.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                            Human.m_MasterHuman = null;
                        }
                    }
                    else
                    {
                        if (m_MasterHuman == null)
                        {
                            return;
                        }
                        sSayMsg = M2Share.g_sMasterListLongOutMasterOnlineMsg.Replace("%d", m_sMasterName);
                        sSayMsg = sSayMsg.Replace("%s", ChrName);
                        sSayMsg = sSayMsg.Replace("%m", Envir.MapDesc);
                        sSayMsg = sSayMsg.Replace("%x", CurrX.ToString());
                        sSayMsg = sSayMsg.Replace("%y", CurrY.ToString());
                        m_MasterHuman.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                        // 如果为大徒弟则将对方的记录清空
                        if (m_MasterHuman.m_sMasterName == ChrName)
                        {
                            m_MasterHuman.m_MasterHuman = null;
                        }
                        for (var i = 0; i < m_MasterHuman.m_MasterList.Count; i++)
                        {
                            if (m_MasterHuman.m_MasterList[i] == this)
                            {
                                m_MasterHuman.m_MasterList.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.Log.LogError(sExceptionMsg);
                M2Share.Log.LogError(e.Message);
            }
            base.MakeGhost();
        }

        protected override void ScatterBagItems(BaseObject ItemOfCreat)
        {
            const int DropWide = 2;
            UserItem pu;
            const string sExceptionMsg = "[Exception] TPlayObject::ScatterBagItems";
            IList<DeleteItem> DelList = null;
            if (AngryRing || NoDropItem || Envir.Flag.boNODROPITEM)
            {
                return;// 不死戒指
            }
            var boDropall = M2Share.Config.DieRedScatterBagAll && PvpLevel() >= 2;
            try
            {
                for (var i = ItemList.Count - 1; i >= 0; i--)
                {
                    if (boDropall || M2Share.RandomNumber.Random(M2Share.Config.DieScatterBagRate) == 0)
                    {
                        if (DropItemDown(ItemList[i], DropWide, true, ItemOfCreat, this))
                        {
                            pu = ItemList[i];
                            if (Race == ActorRace.Play)
                            {
                                if (DelList == null)
                                {
                                    DelList = new List<DeleteItem>();
                                }
                                DelList.Add(new DeleteItem()
                                {
                                    ItemName = M2Share.WorldEngine.GetStdItemName(pu.Index),
                                    MakeIndex = pu.MakeIndex
                                });
                            }
                            Dispose(ItemList[i]);
                            ItemList.RemoveAt(i);
                        }
                    }
                }
                if (DelList != null)
                {
                    var ObjectId = HUtil32.Sequence();
                    M2Share.ActorMgr.AddOhter(ObjectId, DelList);
                    this.SendMsg(this, Grobal2.RM_SENDDELITEMLIST, 0, ObjectId, 0, 0, "");
                }
            }
            catch
            {
                M2Share.Log.LogError(sExceptionMsg);
            }
        }

        public void SetExpiredTime(int expiredTime)
        {
            if (Abil.Level > M2Share.g_nExpErienceLevel)
            {
                ExpireTime = HUtil32.GetTickCount() + (60 * 1000);
                ExpireCount = expiredTime;
            }
        }

        private void CheckExpiredTime()
        {
            ExpireCount = ExpireCount - 1;
            switch (ExpireCount)
            {
                case 30:
                    SysMsg("您的账号游戏时间即将到期，您将在[30:00]分钟后断开服务器。", MsgColor.Blue, MsgType.System);
                    break;
                case > 0 and <= 10:
                    SysMsg($"您的账号游戏时间即将到期，您将在[{ExpireCount}:00]分钟后断开服务器。", MsgColor.Blue, MsgType.System);
                    break;
                case <= 0:
                    ExpireTime = 0;
                    ExpireCount = 0;
                    AccountExpired = true;
                    SysMsg("您的账号游戏时间已到，请访问(https://mir2.sdo.com)进行充值，全服务器账号共享游戏时间。", MsgColor.Blue, MsgType.System);
                    break;
            }
        }
    }
}