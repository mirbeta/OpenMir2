using System;
using System.Collections;
using System.Collections.Generic;
using SystemModule;

namespace GameSvr
{
    public partial class TPlayObject
    {
        public TDefaultMessage m_DefMsg;
        public string m_sOldSayMsg = string.Empty;
        public int m_nSayMsgCount = 0;
        public int m_dwSayMsgTick = 0;
        public bool m_boDisableSayMsg = false;
        public int m_dwDisableSayMsgTick = 0;
        public int m_dwCheckDupObjTick = 0;
        public int dwTick578 = 0;
        public int dwTick57C = 0;
        public bool m_boInSafeArea = false;
        /// <summary>
        /// 登录帐号名
        /// </summary>
        public string m_sUserID;
        /// <summary>
        /// 人物IP地址
        /// </summary>
        public string m_sIPaddr = string.Empty;
        public string m_sIPLocal = string.Empty;
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
        public int m_dwLogonTick = 0;
        /// <summary>
        /// 是否进入游戏完成
        /// </summary>
        public bool m_boReadyRun = false;
        /// <summary>
        /// 当前会话ID
        /// </summary>
        public int m_nSessionID = 0;
        /// <summary>
        /// 人物当前模式(测试/付费模式)
        /// </summary>
        public int m_nPayMent = 0;
        public int m_nPayMode = 0;
        /// <summary>
        /// 全局会话信息
        /// </summary>
        public TSessInfo m_SessInfo = null;
        public int m_dwLoadTick = 0;
        /// <summary>
        /// 人物当前所在服务器序号
        /// </summary>
        public int m_nServerIndex = 0;
        public bool m_boEmergencyClose = false;
        /// <summary>
        /// 掉线标志
        /// </summary>
        public bool m_boSoftClose = false;
        /// <summary>
        /// 断线标志(@kick 命令)
        /// </summary>
        public bool m_boKickFlag = false;
        /// <summary>
        /// 是否重连
        /// </summary>
        public bool m_boReconnection = false;
        public bool m_boRcdSaved = false;
        public bool m_boSwitchData = false;
        public bool m_boSwitchDataOK = false;
        public string m_sSwitchDataTempFile = string.Empty;
        public int m_nWriteChgDataErrCount = 0;
        public string m_sSwitchMapName = string.Empty;
        public short m_nSwitchMapX = 0;
        public short m_nSwitchMapY = 0;
        public bool m_boSwitchDataSended = false;
        public int m_dwChgDataWritedTick = 0;
        /// <summary>
        /// 攻击间隔
        /// </summary>
        public int m_dwHitIntervalTime = 0;
        /// <summary>
        /// 魔法间隔
        /// </summary>
        public int m_dwMagicHitIntervalTime = 0;
        /// <summary>
        /// 走路间隔
        /// </summary>
        public int m_dwRunIntervalTime = 0;
        /// <summary>
        /// 走路间隔
        /// </summary>
        public int m_dwWalkIntervalTime = 0;
        /// <summary>
        /// 换方向间隔
        /// </summary>
        public int m_dwTurnIntervalTime = 0;
        /// <summary>
        /// 组合操作间隔
        /// </summary>
        public int m_dwActionIntervalTime = 0;
        /// <summary>
        /// 移动刺杀间隔
        /// </summary>
        public int m_dwRunLongHitIntervalTime = 0;
        /// <summary>
        /// 跑位攻击间隔
        /// </summary>        
        public int m_dwRunHitIntervalTime = 0;
        /// <summary>
        /// 走位攻击间隔
        /// </summary>        
        public int m_dwWalkHitIntervalTime = 0;
        /// <summary>
        /// 跑位魔法间隔
        /// </summary>        
        public int m_dwRunMagicIntervalTime = 0;
        /// <summary>
        /// 魔法攻击时间
        /// </summary>        
        public int m_dwMagicAttackTick = 0;
        /// <summary>
        /// 魔法攻击间隔时间
        /// </summary>
        public int m_dwMagicAttackInterval = 0;
        /// <summary>
        /// 攻击时间
        /// </summary>
        public int m_dwAttackTick = 0;
        /// <summary>
        /// 人物跑动时间
        /// </summary>
        public int m_dwMoveTick = 0;
        /// <summary>
        /// 人物攻击计数
        /// </summary>
        public int m_dwAttackCount = 0;
        /// <summary>
        /// 人物攻击计数
        /// </summary>
        public int m_dwAttackCountA = 0;
        /// <summary>
        /// 魔法攻击计数
        /// </summary>
        public int m_dwMagicAttackCount = 0;
        /// <summary>
        /// 人物跑计数
        /// </summary>
        public int m_dwMoveCount = 0;
        /// <summary>
        /// 人物跑计数
        /// </summary>
        public int m_dwMoveCountA = 0;
        /// <summary>
        /// 超速计数
        /// </summary>
        public int m_nOverSpeedCount = 0;
        public bool m_boDieInFight3Zone = false;
        public string m_sGotoNpcLabel = string.Empty;
        public int m_nDelayCall = 0;
        public int m_dwDelayCallTick = 0;
        public bool m_boDelayCall = false;
        public int m_DelayCallNPC = 0;
        public string m_sDelayCallLabel = string.Empty;
        public TScript m_Script = null;
        public TBaseObject m_NPC = null;
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
        public bool m_boTimeRecall = false;
        public int m_dwTimeRecallTick = 0;
        public string m_sMoveMap = string.Empty;
        public short m_nMoveX = 0;
        public short m_nMoveY = 0;
        /// <summary>
        /// 保存人物数据时间间隔
        /// </summary>
        public int m_dwSaveRcdTick = 0;
        public byte m_btBright = 0;
        public bool m_boNewHuman = false;
        public bool m_boSendNotice = false;
        public int m_dwWaitLoginNoticeOKTick = 0;
        public bool m_boLoginNoticeOK = false;
        public bool bo6AB = false;
        /// <summary>
        /// 帐号过期
        /// </summary>
        public bool m_boExpire = false;
        public int m_dwShowLineNoticeTick = 0;
        public int m_nShowLineNoticeIdx = 0;
        public int m_nSoftVersionDateEx = 0;
        /// <summary>
        /// 可点击脚本标签字典
        /// </summary>
        private Hashtable m_CanJmpScriptLableList = null;
        public int m_nScriptGotoCount = 0;
        public string m_sScriptCurrLable = string.Empty;
        // 用于处理 @back 脚本命令
        public string m_sScriptGoBackLable = string.Empty;
        // 用于处理 @back 脚本命令
        public int m_dwTurnTick = 0;
        public int m_wOldIdent = 0;
        public byte m_btOldDir = 0;
        /// <summary>
        /// 第一个操作
        /// </summary>
        public bool m_boFirstAction = false;
        /// <summary>
        /// 二次操作之间间隔时间
        /// </summary>        
        public int m_dwActionTick = 0;
        /// <summary>
        /// 配偶名称
        /// </summary>
        public string m_sDearName;
        public TPlayObject m_DearHuman = null;
        /// <summary>
        /// 是否允许夫妻传送
        /// </summary>
        public bool m_boCanDearRecall = false;
        public bool m_boCanMasterRecall = false;
        /// <summary>
        /// 夫妻传送时间
        /// </summary>
        public int m_dwDearRecallTick = 0;
        public int m_dwMasterRecallTick = 0;
        /// <summary>
        /// 师徒名称
        /// </summary>
        public string m_sMasterName;
        public TPlayObject m_MasterHuman = null;
        public IList<TPlayObject> m_MasterList = null;
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
        public byte m_btReColorIdx = 0;
        public int m_dwReColorTick = 0;
        /// <summary>
        /// 杀怪经验倍数
        /// </summary>
        public int m_nKillMonExpMultiple = 0;
        /// <summary>
        /// 处理消息循环时间控制
        /// </summary>        
        public int m_dwGetMsgTick = 0;
        public bool m_boSetStoragePwd = false;
        public bool m_boReConfigPwd = false;
        public bool m_boCheckOldPwd = false;
        public bool m_boUnLockPwd = false;
        public bool m_boUnLockStoragePwd = false;
        public bool m_boPasswordLocked = false;
        // 锁密码
        public byte m_btPwdFailCount = 0;
        /// <summary>
        /// 是否启用锁登录功能
        /// </summary>
        public bool m_boLockLogon = false;
        /// <summary>
        /// 是否打开登录锁
        /// </summary>        
        public bool m_boLockLogoned = false;
        public string m_sTempPwd;
        public string m_sStoragePwd;
        public TBaseObject m_PoseBaseObject = null;
        public bool m_boStartMarry = false;
        public bool m_boStartMaster = false;
        public bool m_boStartUnMarry = false;
        public bool m_boStartUnMaster = false;
        /// <summary>
        /// 禁止发方字(发的文字只能自己看到)
        /// </summary>
        public bool m_boFilterSendMsg = false;
        /// <summary>
        /// 杀怪经验倍数(此数除以 100 为真正倍数)
        /// </summary>        
        public int m_nKillMonExpRate = 0;
        /// <summary>
        /// 人物攻击力倍数(此数除以 100 为真正倍数)
        /// </summary>        
        public int m_nPowerRate = 0;
        public int m_dwKillMonExpRateTime = 0;
        public int m_dwPowerRateTime = 0;
        public int m_dwRateTick = 0;
        /// <summary>
        /// 是否允许使用物品
        /// </summary>
        public bool m_boCanUseItem = false;
        public bool m_boCanDeal = false;
        public bool m_boCanDrop = false;
        public bool m_boCanGetBackItem = false;
        public bool m_boCanWalk = false;
        public bool m_boCanRun = false;
        public bool m_boCanHit = false;
        public bool m_boCanSpell = false;
        public bool m_boCanSendMsg = false;
        public int m_nMemberType = 0;
        // 会员类型
        public int m_nMemberLevel = 0;
        // 会员等级
        public bool m_boSendMsgFlag = false;
        // 发祝福语标志
        public bool m_boChangeItemNameFlag = false;
        /// <summary>
        /// 游戏币
        /// </summary>
        public int m_nGameGold = 0;
        /// <summary>
        /// 是否自动减游戏币
        /// </summary>        
        public bool m_boDecGameGold = false;
        public int m_dwDecGameGoldTime = 0;
        public int m_dwDecGameGoldTick = 0;
        public int m_nDecGameGold = 0;
        // 一次减点数
        public bool m_boIncGameGold = false;
        // 是否自动加游戏币
        public int m_dwIncGameGoldTime = 0;
        public int m_dwIncGameGoldTick = 0;
        public int m_nIncGameGold = 0;
        // 一次减点数
        public int m_nGamePoint = 0;
        // 游戏点数
        public int m_dwIncGamePointTick = 0;
        public int m_nPayMentPoint = 0;
        public int m_dwPayMentPointTick = 0;
        public int m_dwDecHPTick = 0;
        public int m_dwIncHPTick = 0;
        public TPlayObject m_GetWhisperHuman = null;
        public int m_dwClearObjTick = 0;
        public short m_wContribution = 0;
        // 贡献度
        public string m_sRankLevelName = string.Empty;
        // 显示名称格式串
        public bool m_boFilterAction = false;
        public bool m_boClientFlag = false;
        public byte m_nStep = 0;
        public int m_nClientFlagMode = 0;
        public int m_dwAutoGetExpTick = 0;
        public int m_nAutoGetExpTime = 0;
        public int m_nAutoGetExpPoint = 0;
        public Envirnoment m_AutoGetExpEnvir = null;
        public bool m_boAutoGetExpInSafeZone = false;
        public Dictionary<string, TDynamicVar> m_DynamicVarList = null;
        public short m_dwClientTick = 0;
        /// <summary>
        /// 进入速度测试模式
        /// </summary>
        public bool m_boTestSpeedMode = false;
        public int m_dwDelayTime = 0;
        public string m_sRandomNo = string.Empty;
        public int m_dwQueryBagItemsTick = 0;
        public bool m_boTimeGoto;
        public int m_dwTimeGotoTick;
        public string m_sTimeGotoLable;
        public TBaseObject m_TimeGotoNPC;
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
        private IList<TUserItem> m_SellOffItemList = null;


        public TPlayObject() : base()
        {
            m_btRaceServer = Grobal2.RC_PLAYOBJECT;
            m_boEmergencyClose = false;
            m_boSwitchData = false;
            m_boReconnection = false;
            m_boKickFlag = false;
            m_boSoftClose = false;
            m_boReadyRun = false;
            m_dwSaveRcdTick = HUtil32.GetTickCount();
            m_boWantRefMsg = true;
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
            m_dwRunTick = HUtil32.GetTickCount();
            m_nRunTime = 250;
            m_dwSearchTime = 1000;
            m_dwSearchTick = HUtil32.GetTickCount();
            m_nViewRange = 12;
            m_boNewHuman = false;
            m_boLoginNoticeOK = false;
            bo6AB = false;
            m_boExpire = false;
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
            m_dwMoveCountA = 0;
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
            m_MasterList = new List<TPlayObject>();
            m_boSendMsgFlag = false;
            m_boChangeItemNameFlag = false;
            m_boCanMasterRecall = false;
            m_boCanDearRecall = false;
            m_dwDearRecallTick = HUtil32.GetTickCount();
            m_dwMasterRecallTick = HUtil32.GetTickCount();
            m_btReColorIdx = 0;
            m_GetWhisperHuman = null;
            m_boOnHorse = false;
            m_wContribution = 0;
            m_sRankLevelName = M2Share.g_sRankLevelName;
            m_boFixedHideMode = true;
            m_nStep = 0;
            m_nVal = new int[100];
            m_nMval = new int[100];
            m_DyVal = new int[100];
            m_nSval = new string[100];
            m_nInteger = new int[100];
            m_sString = new string[100];
            m_ServerStrVal = new string[100];
            m_ServerIntVal = new int[100];
            m_nClientFlagMode = -1;
            m_dwAutoGetExpTick = HUtil32.GetTickCount();
            m_nAutoGetExpPoint = 0;
            m_AutoGetExpEnvir = null;
            m_dwHitIntervalTime = M2Share.g_Config.dwHitIntervalTime;// 攻击间隔
            m_dwMagicHitIntervalTime = M2Share.g_Config.dwMagicHitIntervalTime;// 魔法间隔
            m_dwRunIntervalTime = M2Share.g_Config.dwRunIntervalTime;// 走路间隔
            m_dwWalkIntervalTime = M2Share.g_Config.dwWalkIntervalTime;// 走路间隔
            m_dwTurnIntervalTime = M2Share.g_Config.dwTurnIntervalTime;// 换方向间隔
            m_dwActionIntervalTime = M2Share.g_Config.dwActionIntervalTime;// 组合操作间隔
            m_dwRunLongHitIntervalTime = M2Share.g_Config.dwRunLongHitIntervalTime;// 组合操作间隔
            m_dwRunHitIntervalTime = M2Share.g_Config.dwRunHitIntervalTime;// 组合操作间隔
            m_dwWalkHitIntervalTime = M2Share.g_Config.dwWalkHitIntervalTime;// 组合操作间隔
            m_dwRunMagicIntervalTime = M2Share.g_Config.dwRunMagicIntervalTime;// 跑位魔法间隔
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
            m_sRandomNo = M2Share.RandomNumber.Random(999999).ToString();
        }

        private void SendNotice()
        {
            var LoadList = new List<string>();
            M2Share.NoticeManager.GetNoticeMsg("Notice", LoadList);
            var sNoticeMsg = string.Empty;
            if (LoadList.Count > 0)
            {
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sNoticeMsg = sNoticeMsg + LoadList[i] + "\x20\x1B";
                }
            }
            LoadList = null;
            SendDefMessage(Grobal2.SM_SENDNOTICE, 2000, 0, 0, 0, sNoticeMsg.Replace("/r/n/r/n ", ""));
        }

        public void RunNotice()
        {
            TProcessMessage Msg = null;
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
                catch
                {
                    M2Share.ErrorMessage(sExceptionMsg);
                }
            }
        }

        private void SendLogon()
        {
            var MessageBodyWL = new TMessageBodyWL();
            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_LOGON, ObjectId, m_nCurrX, m_nCurrY, HUtil32.MakeWord(m_btDirection, m_nLight));
            MessageBodyWL.lParam1 = GetFeatureToLong();
            MessageBodyWL.lParam2 = (int)m_nCharStatus;
            if (m_boAllowGroup)
            {
                MessageBodyWL.lTag1 = HUtil32.MakeLong(HUtil32.MakeWord(1, 0), GetFeatureEx());
            }
            else
            {
                MessageBodyWL.lTag1 = 0;
            }
            MessageBodyWL.lTag2 = 0;
            SendSocket(m_DefMsg, EDcode.EncodeBuffer(MessageBodyWL));
            var nRecog = GetFeatureToLong();
            SendDefMessage(Grobal2.SM_FEATURECHANGED, ObjectId, HUtil32.LoWord(nRecog), HUtil32.HiWord(nRecog), GetFeatureEx(), "");
            SendDefMessage(Grobal2.SM_ATTACKMODE, m_btAttatckMode, 0, 0, 0, "");
        }

        /// <summary>
        /// 玩家登录
        /// </summary>
        public void UserLogon()
        {
            TUserItem UserItem;
            var sIPaddr = "127.0.0.1";
            const string sExceptionMsg = "[Exception] TPlayObject::UserLogon";
            const string sCheckIPaddrFail = "登录IP地址不匹配!!!";
            try
            {
                if (M2Share.g_Config.boTestServer)
                {
                    if (m_Abil.Level < M2Share.g_Config.nTestLevel)
                    {
                        m_Abil.Level = (ushort)M2Share.g_Config.nTestLevel;
                    }
                    if (m_nGold < M2Share.g_Config.nTestGold)
                    {
                        m_nGold = M2Share.g_Config.nTestGold;
                    }
                }
                if (M2Share.g_Config.boTestServer || M2Share.g_Config.boServiceMode)
                {
                    m_nPayMent = 3;
                }
                m_dwMapMoveTick = HUtil32.GetTickCount();
                m_dLogonTime = DateTime.Now;
                m_dwLogonTick = HUtil32.GetTickCount();
                Initialize();
                SendMsg(this, Grobal2.RM_LOGON, 0, 0, 0, 0, "");
                if (m_Abil.Level <= 7)
                {
                    if (GetRangeHumanCount() >= 80)
                    {
                        MapRandomMove(m_PEnvir.sMapName, 0);
                    }
                }
                if (m_boDieInFight3Zone)
                {
                    MapRandomMove(m_PEnvir.sMapName, 0);
                }
                if (M2Share.UserEngine.GetHumPermission(m_sCharName, ref sIPaddr, ref m_btPermission))
                {
                    if (M2Share.g_Config.PermissionSystem)
                    {
                        if (!M2Share.CompareIPaddr(m_sIPaddr, sIPaddr))
                        {
                            SysMsg(sCheckIPaddrFail, MsgColor.Red, MsgType.Hint);
                            m_boEmergencyClose = true;
                        }
                    }
                }
                GetStartPoint();
                for (var i = m_MagicList.Count - 1; i >= 0; i--)
                {
                    CheckSeeHealGauge(m_MagicList[i]);
                }
                if (m_boNewHuman)
                {
                    UserItem = new TUserItem();
                    if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sCandle, ref UserItem))
                    {
                        m_ItemList.Add(UserItem);
                    }
                    else
                    {
                        Dispose(UserItem);
                    }
                    UserItem = new TUserItem();
                    if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sBasicDrug, ref UserItem))
                    {
                        m_ItemList.Add(UserItem);
                    }
                    else
                    {
                        Dispose(UserItem);
                    }
                    UserItem = new TUserItem();
                    if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sWoodenSword, ref UserItem))
                    {
                        m_ItemList.Add(UserItem);
                    }
                    else
                    {
                        Dispose(UserItem);
                    }
                    UserItem = new TUserItem();
                    var sItem = m_btGender == PlayGender.Man
                        ? M2Share.g_Config.sClothsMan
                        : M2Share.g_Config.sClothsWoman;
                    if (M2Share.UserEngine.CopyToUserItemFromName(sItem, ref UserItem))
                    {
                        m_ItemList.Add(UserItem);
                    }
                    else
                    {
                        Dispose(UserItem);
                    }
                }
                // 检查背包中的物品是否合法
                for (var i = m_ItemList.Count - 1; i >= 0; i--)
                {
                    UserItem = m_ItemList[i];
                    if (!string.IsNullOrEmpty(M2Share.UserEngine.GetStdItemName(UserItem.wIndex))) continue;
                    Dispose(m_ItemList[i]);
                    m_ItemList.RemoveAt(i);
                }
                // 检查人物身上的物品是否符合使用规则
                if (M2Share.g_Config.boCheckUserItemPlace)
                {
                    for (var i = m_UseItems.GetLowerBound(0); i <= m_UseItems.GetUpperBound(0); i++)
                    {
                        if (m_UseItems[i] == null || m_UseItems[i].wIndex <= 0) continue;
                        var StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[i].wIndex);
                        if (StdItem != null)
                        {
                            if (!M2Share.CheckUserItems(i, StdItem))
                            {
                                UserItem = m_UseItems[i];
                                if (!AddItemToBag(UserItem))
                                {
                                    m_ItemList.Insert(0, UserItem);
                                }
                                m_UseItems[i].wIndex = 0;
                            }
                        }
                        else
                        {
                            m_UseItems[i].wIndex = 0;
                        }
                    }
                }
                // 检查背包中是否有复制品
                for (var i = m_ItemList.Count - 1; i >= 0; i--)
                {
                    UserItem = m_ItemList[i];
                    var sItemName = M2Share.UserEngine.GetStdItemName(UserItem.wIndex);
                    for (var j = i - 1; j >= 0; j--)
                    {
                        var UserItem1 = m_ItemList[j];
                        if (M2Share.UserEngine.GetStdItemName(UserItem1.wIndex) == sItemName && UserItem.MakeIndex == UserItem1.MakeIndex)
                        {
                            m_ItemList.RemoveAt(j);
                            break;
                        }
                    }
                }
                for (var i = m_dwStatusArrTick.GetLowerBound(0); i <= m_dwStatusArrTick.GetUpperBound(0); i++)
                {
                    if (m_wStatusTimeArr[i] > 0)
                    {
                        m_dwStatusArrTick[i] = HUtil32.GetTickCount();
                    }
                }
                m_nCharStatus = GetCharStatus();
                RecalcLevelAbilitys();
                RecalcAbilitys();
                if (btB2 == 0)
                {
                    m_nPkPoint = 0;
                    btB2++;
                }
                if (m_nGold > M2Share.g_Config.nHumanMaxGold * 2 && M2Share.g_Config.nHumanMaxGold > 0)
                {
                    m_nGold = M2Share.g_Config.nHumanMaxGold * 2;
                }
                if (!bo6AB)
                {
                    if (m_nSoftVersionDate < M2Share.g_Config.nSoftVersionDate)
                    {
                        SysMsg(M2Share.sClientSoftVersionError, MsgColor.Red, MsgType.Hint);
                        SysMsg(M2Share.sDownLoadNewClientSoft, MsgColor.Red, MsgType.Hint);
                        SysMsg(M2Share.sForceDisConnect, MsgColor.Red, MsgType.Hint);
                        m_boEmergencyClose = true;
                        return;
                    }
                    if (m_nSoftVersionDateEx == 0 && M2Share.g_Config.boOldClientShowHiLevel)
                    {
                        SysMsg(M2Share.sClientSoftVersionTooOld, MsgColor.Blue, MsgType.Hint);
                        SysMsg(M2Share.sDownLoadAndUseNewClient, MsgColor.Red, MsgType.Hint);
                        if (!M2Share.g_Config.boCanOldClientLogon)
                        {
                            SysMsg(M2Share.sClientSoftVersionError, MsgColor.Red, MsgType.Hint);
                            SysMsg(M2Share.sDownLoadNewClientSoft, MsgColor.Red, MsgType.Hint);
                            SysMsg(M2Share.sForceDisConnect, MsgColor.Red, MsgType.Hint);
                            m_boEmergencyClose = true;
                            return;
                        }
                    }
                    switch (m_btAttatckMode)
                    {
                        case M2Share.HAM_ALL:// [攻击模式: 全体攻击]
                            SysMsg(M2Share.sAttackModeOfAll, MsgColor.Green, MsgType.Hint);
                            break;
                        case M2Share.HAM_PEACE:// [攻击模式: 和平攻击]
                            SysMsg(M2Share.sAttackModeOfPeaceful, MsgColor.Green, MsgType.Hint);
                            break;
                        case M2Share.HAM_DEAR:// [攻击模式: 和平攻击]
                            SysMsg(M2Share.sAttackModeOfDear, MsgColor.Green, MsgType.Hint);
                            break;
                        case M2Share.HAM_MASTER:// [攻击模式: 和平攻击]
                            SysMsg(M2Share.sAttackModeOfMaster, MsgColor.Green, MsgType.Hint);
                            break;
                        case M2Share.HAM_GROUP:// [攻击模式: 编组攻击]
                            SysMsg(M2Share.sAttackModeOfGroup, MsgColor.Green, MsgType.Hint);
                            break;
                        case M2Share.HAM_GUILD:// [攻击模式: 行会攻击]
                            SysMsg(M2Share.sAttackModeOfGuild, MsgColor.Green, MsgType.Hint);
                            break;
                        case M2Share.HAM_PKATTACK:// [攻击模式: 红名攻击]
                            SysMsg(M2Share.sAttackModeOfRedWhite, MsgColor.Green, MsgType.Hint);
                            break;
                    }
                    SysMsg(M2Share.sStartChangeAttackModeHelp, MsgColor.Green, MsgType.Hint);// 使用组合快捷键 CTRL-H 更改攻击...
                    if (M2Share.g_Config.boTestServer)
                    {
                        SysMsg(M2Share.sStartNoticeMsg, MsgColor.Green, MsgType.Hint);// 欢迎进入本服务器进行游戏...
                    }
                    if (M2Share.UserEngine.PlayObjectCount > M2Share.g_Config.nTestUserLimit)
                    {
                        if (m_btPermission < 2)
                        {
                            SysMsg(M2Share.sOnlineUserFull, MsgColor.Red, MsgType.Hint);
                            SysMsg(M2Share.sForceDisConnect, MsgColor.Red, MsgType.Hint);
                            m_boEmergencyClose = true;
                        }
                    }
                }
                m_btBright = (byte)M2Share.g_nGameTime;
                m_Abil.MaxExp = GetLevelExp(m_Abil.Level);// 登录重新取得升级所需经验值
                SendMsg(this, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                SendMsg(this, Grobal2.RM_SUBABILITY, 0, 0, 0, 0, "");
                SendMsg(this, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                SendMsg(this, Grobal2.RM_DAYCHANGING, 0, 0, 0, 0, "");
                SendMsg(this, Grobal2.RM_SENDUSEITEMS, 0, 0, 0, 0, "");
                SendMsg(this, Grobal2.RM_SENDMYMAGIC, 0, 0, 0, 0, "");
                // FeatureChanged(); //增加，广播人物骑马信息
                m_MyGuild = M2Share.GuildManager.MemberOfGuild(m_sCharName);
                if (m_MyGuild != null)
                {
                    m_sGuildRankName = m_MyGuild.GetRankName(this, ref m_nGuildRankNo);
                    for (var i = m_MyGuild.GuildWarList.Count - 1; i >= 0; i--)
                    {
                        SysMsg(m_MyGuild.GuildWarList[i] + " 正在与本行会进行行会战.", MsgColor.Green, MsgType.Hint);
                    }
                }
                RefShowName();
                if (m_nPayMent == 1)
                {
                    if (!bo6AB)
                    {
                        SysMsg(M2Share.sYouNowIsTryPlayMode, MsgColor.Red, MsgType.Hint);
                    }
                    m_nGoldMax = M2Share.g_Config.nHumanTryModeMaxGold;
                    if (m_Abil.Level > M2Share.g_Config.nTryModeLevel)
                    {
                        SysMsg("测试状态可以使用到第 " + M2Share.g_Config.nTryModeLevel, MsgColor.Red, MsgType.Hint);
                        SysMsg("链接中断，请到以下地址获得收费相关信息。(http://www.mir2.com)", MsgColor.Red, MsgType.Hint);
                        m_boEmergencyClose = true;
                    }
                }
                if (m_nPayMent == 3 && !bo6AB)
                {
                    SysMsg(M2Share.g_sNowIsFreePlayMode, MsgColor.Green, MsgType.Hint);
                }
                if (M2Share.g_Config.boVentureServer)
                {
                    SysMsg("当前服务器运行于冒险模式.", MsgColor.Green, MsgType.Hint);
                }
                if (m_MagicArr[SpellsDef.SKILL_ERGUM] != null && !m_boUseThrusting)
                {
                    m_boUseThrusting = true;
                    SendSocket("+LNG");
                }
                if (m_PEnvir.Flag.boNORECONNECT)
                {
                    MapRandomMove(m_PEnvir.Flag.sNoReConnectMap, 0);
                }
                if (CheckDenyLogon())// 如果人物在禁止登录列表里则直接掉线而不执行下面内容
                {
                    return;
                }
                if (M2Share.g_ManageNPC != null)
                {
                    M2Share.g_ManageNPC.GotoLable(this, "@Login", false);
                }
                m_boFixedHideMode = false;
                if (!string.IsNullOrEmpty(m_sDearName))
                {
                    CheckMarry();
                }
                CheckMaster();
                m_boFilterSendMsg = M2Share.GetDisableSendMsgList(m_sCharName);
                // 密码保护系统
                if (M2Share.g_Config.boPasswordLockSystem)
                {
                    if (m_boPasswordLocked)
                    {
                        m_boCanGetBackItem = !M2Share.g_Config.boLockGetBackItemAction;
                    }
                    if (M2Share.g_Config.boLockHumanLogin && m_boLockLogon && m_boPasswordLocked)
                    {
                        m_boCanDeal = !M2Share.g_Config.boLockDealAction;
                        m_boCanDrop = !M2Share.g_Config.boLockDropAction;
                        m_boCanUseItem = !M2Share.g_Config.boLockUserItemAction;
                        m_boCanWalk = !M2Share.g_Config.boLockWalkAction;
                        m_boCanRun = !M2Share.g_Config.boLockRunAction;
                        m_boCanHit = !M2Share.g_Config.boLockHitAction;
                        m_boCanSpell = !M2Share.g_Config.boLockSpellAction;
                        m_boCanSendMsg = !M2Share.g_Config.boLockSendMsgAction;
                        m_boObMode = M2Share.g_Config.boLockInObModeAction;
                        m_boAdminMode = M2Share.g_Config.boLockInObModeAction;
                        SysMsg(M2Share.g_sActionIsLockedMsg + " 开锁命令: @" + M2Share.g_GameCommand.LOCKLOGON.sCmd, MsgColor.Red, MsgType.Hint);
                        SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sActionIsLockedMsg + "\\ \\" + "密码命令: @" + M2Share.g_GameCommand.PASSWORDLOCK.sCmd);
                    }
                    if (!m_boPasswordLocked)
                    {
                        SysMsg(format(M2Share.g_sPasswordNotSetMsg, M2Share.g_GameCommand.PASSWORDLOCK.sCmd), MsgColor.Red, MsgType.Hint);
                    }
                    if (!m_boLockLogon && m_boPasswordLocked)
                    {
                        SysMsg(format(M2Share.g_sNotPasswordProtectMode, M2Share.g_GameCommand.LOCKLOGON.sCmd), MsgColor.Red, MsgType.Hint);
                    }
                    SysMsg(M2Share.g_sActionIsLockedMsg + " 开锁命令: @" + M2Share.g_GameCommand.UNLOCK.sCmd, MsgColor.Red, MsgType.Hint);
                    SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sActionIsLockedMsg + "\\ \\" + "开锁命令: @" + M2Share.g_GameCommand.UNLOCK.sCmd + '\\' + "加锁命令: @" + M2Share.g_GameCommand.__LOCK.sCmd + '\\' + "设置密码命令: @" + M2Share.g_GameCommand.SETPASSWORD.sCmd + '\\' + "修改密码命令: @" + M2Share.g_GameCommand.CHGPASSWORD.sCmd);
                }
                // 重置泡点方面计时
                m_dwIncGamePointTick = HUtil32.GetTickCount();
                m_dwIncGameGoldTick = HUtil32.GetTickCount();
                m_dwAutoGetExpTick = HUtil32.GetTickCount();
                GetSellOffGlod();// 检查是否有元宝寄售交易结束还没得到元宝
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.StackTrace);
            }
            // ReadAllBook();
        }

        /// <summary>
        /// 使用祝福油
        /// </summary>
        /// <returns></returns>
        private bool WeaptonMakeLuck()
        {
            if (m_UseItems[Grobal2.U_WEAPON] == null && m_UseItems[Grobal2.U_WEAPON].wIndex <= 0)
            {
                return false;
            }
            var nRand = 0;
            var StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[Grobal2.U_WEAPON].wIndex);
            if (StdItem != null)
            {
                nRand = Math.Abs(StdItem.Dc2 - StdItem.Dc) / 5;
            }
            if (M2Share.RandomNumber.Random(M2Share.g_Config.nWeaponMakeUnLuckRate) == 1)
            {
                MakeWeaponUnlock();
            }
            else
            {
                var boMakeLuck = false;
                if (m_UseItems[Grobal2.U_WEAPON].btValue[4] > 0)
                {
                    m_UseItems[Grobal2.U_WEAPON].btValue[4] -= 1;
                    SysMsg(M2Share.g_sWeaptonMakeLuck, MsgColor.Green, MsgType.Hint);
                    boMakeLuck = true;
                }
                else if (m_UseItems[Grobal2.U_WEAPON].btValue[3] < M2Share.g_Config.nWeaponMakeLuckPoint1)
                {
                    m_UseItems[Grobal2.U_WEAPON].btValue[3]++;
                    SysMsg(M2Share.g_sWeaptonMakeLuck, MsgColor.Green, MsgType.Hint);
                    boMakeLuck = true;
                }
                else if (m_UseItems[Grobal2.U_WEAPON].btValue[3] < M2Share.g_Config.nWeaponMakeLuckPoint2 && M2Share.RandomNumber.Random(nRand + M2Share.g_Config.nWeaponMakeLuckPoint2Rate) == 1)
                {
                    m_UseItems[Grobal2.U_WEAPON].btValue[3]++;
                    SysMsg(M2Share.g_sWeaptonMakeLuck, MsgColor.Green, MsgType.Hint);
                    boMakeLuck = true;
                }
                else if (m_UseItems[Grobal2.U_WEAPON].btValue[3] < M2Share.g_Config.nWeaponMakeLuckPoint3 && M2Share.RandomNumber.Random(nRand * M2Share.g_Config.nWeaponMakeLuckPoint3Rate) == 1)
                {
                    m_UseItems[Grobal2.U_WEAPON].btValue[3]++;
                    SysMsg(M2Share.g_sWeaptonMakeLuck, MsgColor.Green, MsgType.Hint);
                    boMakeLuck = true;
                }
                if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
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
            if (m_UseItems[Grobal2.U_WEAPON] == null)
            {
                return false;
            }
            var UserItem = m_UseItems[Grobal2.U_WEAPON];
            if (UserItem.wIndex <= 0 || UserItem.DuraMax <= UserItem.Dura)
            {
                return false;
            }
            UserItem.DuraMax -= (ushort)((UserItem.DuraMax - UserItem.Dura) / M2Share.g_Config.nRepairItemDecDura);
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
            if (m_UseItems[Grobal2.U_WEAPON] == null && m_UseItems[Grobal2.U_WEAPON].wIndex <= 0)
            {
                return false;
            }
            m_UseItems[Grobal2.U_WEAPON].Dura = m_UseItems[Grobal2.U_WEAPON].DuraMax;
            SendMsg(this, Grobal2.RM_DURACHANGE, 1, m_UseItems[Grobal2.U_WEAPON].Dura, m_UseItems[Grobal2.U_WEAPON].DuraMax, 0, "");
            SysMsg(M2Share.g_sWeaponRepairSuccess, MsgColor.Green, MsgType.Hint);
            return true;
        }

        private void WinLottery()
        {
            var nGold = 0;
            var nWinLevel = 0;
            var nRate = M2Share.RandomNumber.Random(M2Share.g_Config.nWinLotteryRate);
            if (nRate >= M2Share.g_Config.nWinLottery6Min && nRate <= M2Share.g_Config.nWinLottery6Max)
            {
                if (M2Share.g_Config.nWinLotteryCount < M2Share.g_Config.nNoWinLotteryCount)
                {
                    nGold = M2Share.g_Config.nWinLottery6Gold;
                    nWinLevel = 6;
                    M2Share.g_Config.nWinLotteryLevel6++;
                }
            }
            else if (nRate >= M2Share.g_Config.nWinLottery5Min && nRate <= M2Share.g_Config.nWinLottery5Max)
            {
                if (M2Share.g_Config.nWinLotteryCount < M2Share.g_Config.nNoWinLotteryCount)
                {
                    nGold = M2Share.g_Config.nWinLottery5Gold;
                    nWinLevel = 5;
                    M2Share.g_Config.nWinLotteryLevel5++;
                }
            }
            else if (nRate >= M2Share.g_Config.nWinLottery4Min && nRate <= M2Share.g_Config.nWinLottery4Max)
            {
                if (M2Share.g_Config.nWinLotteryCount < M2Share.g_Config.nNoWinLotteryCount)
                {
                    nGold = M2Share.g_Config.nWinLottery4Gold;
                    nWinLevel = 4;
                    M2Share.g_Config.nWinLotteryLevel4++;
                }
            }
            else if (nRate >= M2Share.g_Config.nWinLottery3Min && nRate <= M2Share.g_Config.nWinLottery3Max)
            {
                if (M2Share.g_Config.nWinLotteryCount < M2Share.g_Config.nNoWinLotteryCount)
                {
                    nGold = M2Share.g_Config.nWinLottery3Gold;
                    nWinLevel = 3;
                    M2Share.g_Config.nWinLotteryLevel3++;
                }
            }
            else if (nRate >= M2Share.g_Config.nWinLottery2Min && nRate <= M2Share.g_Config.nWinLottery2Max)
            {
                if (M2Share.g_Config.nWinLotteryCount < M2Share.g_Config.nNoWinLotteryCount)
                {
                    nGold = M2Share.g_Config.nWinLottery2Gold;
                    nWinLevel = 2;
                    M2Share.g_Config.nWinLotteryLevel2++;
                }
            }
            else if (new ArrayList(new int[] { M2Share.g_Config.nWinLottery1Min + M2Share.g_Config.nWinLottery1Max }).Contains(nRate))
            {
                if (M2Share.g_Config.nWinLotteryCount < M2Share.g_Config.nNoWinLotteryCount)
                {
                    nGold = M2Share.g_Config.nWinLottery1Gold;
                    nWinLevel = 1;
                    M2Share.g_Config.nWinLotteryLevel1++;
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
                M2Share.g_Config.nNoWinLotteryCount += 500;
                SysMsg(M2Share.g_sNotWinLotteryMsg, MsgColor.Red, MsgType.Hint);
            }
        }

        public override void RecalcAbilitys()
        {
            base.RecalcAbilitys();
            RecalcAdjusBonus();
        }

        protected override void UpdateVisibleGay(TBaseObject BaseObject)
        {
            var boIsVisible = false;
            TVisibleBaseObject VisibleBaseObject;
            if (BaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT || BaseObject.m_Master != null)
            {
                m_boIsVisibleActive = true;// 如果是人物或宝宝则置TRUE
            }
            for (var i = 0; i < m_VisibleActors.Count; i++)
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
            if (BaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
            {
                SendWhisperMsg(BaseObject as TPlayObject);
            }
        }

        public override void SearchViewRange()
        {
            MapCellinfo MapCellInfo;
            TBaseObject BaseObject = null;
            Event MapEvent = null;
            for (var i = m_VisibleItems.Count - 1; i >= 0; i--)
            {
                m_VisibleItems[i].nVisibleFlag = 0;
            }
            for (var i = m_VisibleEvents.Count - 1; i >= 0; i--)
            {
                m_VisibleEvents[i].nVisibleFlag = 0;
            }
            for (var i = m_VisibleActors.Count - 1; i >= 0; i--)
            {
                m_VisibleActors[i].nVisibleFlag = 0;
            }
            var nStartX = m_nCurrX - m_nViewRange;
            var nEndX = m_nCurrX + m_nViewRange;
            var nStartY = m_nCurrY - m_nViewRange;
            var nEndY = m_nCurrY + m_nViewRange;
            try
            {
                for (var n20 = nStartX; n20 <= nEndX; n20++)
                {
                    for (var n1C = nStartY; n1C <= nEndY; n1C++)
                    {
                        var mapCell = false;
                        MapCellInfo = m_PEnvir.GetMapCellInfo(n20, n1C, ref mapCell);
                        if (mapCell && MapCellInfo.ObjList != null)
                        {
                            var nIdx = 0;
                            while (true)
                            {
                                if (MapCellInfo.Count <= nIdx)
                                {
                                    break;
                                }
                                var OSObject = MapCellInfo.ObjList[nIdx];
                                if (OSObject != null)
                                {
                                    if (OSObject.CellType == CellType.OS_MOVINGOBJECT)
                                    {
                                        if ((HUtil32.GetTickCount() - OSObject.dwAddTime) >= 60 * 1000)
                                        {
                                            Dispose(OSObject);
                                            MapCellInfo.Remove(nIdx);
                                            if (MapCellInfo.Count > 0)
                                            {
                                                continue;
                                            }
                                            MapCellInfo.Dispose();
                                            break;
                                        }
                                        BaseObject = (TBaseObject)OSObject.CellObj;
                                        if (BaseObject != null && !BaseObject.m_boInvisible)
                                        {
                                            if (!BaseObject.m_boGhost && !BaseObject.m_boFixedHideMode && !BaseObject.m_boObMode)
                                            {
                                                if (m_btRaceServer < Grobal2.RC_ANIMAL || m_Master != null || m_boCrazyMode || m_boNastyMode || m_boWantRefMsg || BaseObject.m_Master != null && Math.Abs(BaseObject.m_nCurrX - m_nCurrX) <= 3 && Math.Abs(BaseObject.m_nCurrY - m_nCurrY) <= 3 || BaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                                                {
                                                    UpdateVisibleGay(BaseObject);
                                                }
                                            }
                                        }
                                    }
                                    if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                                    {
                                        if (OSObject.CellType == CellType.OS_ITEMOBJECT)
                                        {
                                            if ((HUtil32.GetTickCount() - OSObject.dwAddTime) > M2Share.g_Config.dwClearDropOnFloorItemTime)// 60 * 60 * 1000
                                            {
                                                Dispose(OSObject.CellObj);
                                                Dispose(OSObject);
                                                MapCellInfo.Remove(nIdx);
                                                if (MapCellInfo.Count > 0)
                                                {
                                                    continue;
                                                }
                                                MapCellInfo.Dispose();
                                                break;
                                            }
                                            var MapItem = (MapItem)OSObject.CellObj;
                                            UpdateVisibleItem(n20, n1C, MapItem);
                                            if (MapItem.OfBaseObject != null || MapItem.DropBaseObject != null)
                                            {
                                                if ((HUtil32.GetTickCount() - MapItem.CanPickUpTick) > M2Share.g_Config.dwFloorItemCanPickUpTime) // 2 * 60 * 1000
                                                {
                                                    MapItem.OfBaseObject = null;
                                                    MapItem.DropBaseObject = null;
                                                }
                                                else
                                                {
                                                    if (MapItem.OfBaseObject as TBaseObject != null)
                                                    {
                                                        if ((MapItem.OfBaseObject as TBaseObject).m_boGhost)
                                                        {
                                                            MapItem.OfBaseObject = null;
                                                        }
                                                    }
                                                    if (MapItem.DropBaseObject as TBaseObject != null)
                                                    {
                                                        if ((MapItem.DropBaseObject as TBaseObject).m_boGhost)
                                                        {
                                                            MapItem.DropBaseObject = null;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (OSObject.CellType == CellType.OS_EVENTOBJECT)
                                        {
                                            MapEvent = (Event)OSObject.CellObj;
                                            if (MapEvent.m_boVisible)
                                            {
                                                UpdateVisibleEvent(n20, n1C, MapEvent);
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
                    if (m_VisibleActors.Count <= n18)
                    {
                        break;
                    }
                    var VisibleBaseObject = m_VisibleActors[n18];
                    if (VisibleBaseObject.nVisibleFlag == 0)
                    {
                        if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                        {
                            BaseObject = VisibleBaseObject.BaseObject;
                            if (!BaseObject.m_boFixedHideMode && !BaseObject.m_boGhost)//防止人物退出时发送重复的消息占用带宽，人物进入隐身模式时人物不消失问题
                            {
                                SendMsg(BaseObject, Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
                            }
                        }
                        m_VisibleActors.RemoveAt(n18);
                        Dispose(VisibleBaseObject);
                        continue;
                    }
                    if (m_btRaceServer == Grobal2.RC_PLAYOBJECT && VisibleBaseObject.nVisibleFlag == 2)
                    {
                        BaseObject = VisibleBaseObject.BaseObject;
                        if (BaseObject != this)
                        {
                            if (BaseObject.m_boDeath)
                            {
                                if (BaseObject.m_boSkeleton)
                                {
                                    SendMsg(BaseObject, Grobal2.RM_SKELETON, BaseObject.m_btDirection, BaseObject.m_nCurrX, BaseObject.m_nCurrY, 0, "");
                                }
                                else
                                {
                                    SendMsg(BaseObject, Grobal2.RM_DEATH, BaseObject.m_btDirection, BaseObject.m_nCurrX, BaseObject.m_nCurrY, 0, "");
                                }
                            }
                            else
                            {
                                SendMsg(BaseObject, Grobal2.RM_TURN, BaseObject.m_btDirection, BaseObject.m_nCurrX, BaseObject.m_nCurrY, 0, BaseObject.GetShowName());
                            }
                        }
                    }
                    n18++;
                }

                var I = 0;
                while (true)
                {
                    if (m_VisibleItems.Count <= I)
                    {
                        break;
                    }
                    var VisibleMapItem = m_VisibleItems[I];
                    if (VisibleMapItem.nVisibleFlag == 0)
                    {
                        SendMsg(this, Grobal2.RM_ITEMHIDE, 0, VisibleMapItem.MapItem.Id, VisibleMapItem.nX, VisibleMapItem.nY, "");
                        m_VisibleItems.RemoveAt(I);
                        Dispose(VisibleMapItem);
                        continue;
                    }
                    if (VisibleMapItem.nVisibleFlag == 2)
                    {
                        SendMsg(this, Grobal2.RM_ITEMSHOW, VisibleMapItem.wLooks, VisibleMapItem.MapItem.Id, VisibleMapItem.nX, VisibleMapItem.nY, VisibleMapItem.sName);
                    }
                    I++;
                }
                I = 0;
                while (true)
                {
                    if (m_VisibleEvents.Count <= I)
                    {
                        break;
                    }
                    MapEvent = m_VisibleEvents[I];
                    if (MapEvent.nVisibleFlag == 0)
                    {
                        SendMsg(this, Grobal2.RM_HIDEEVENT, 0, MapEvent.Id, MapEvent.m_nX, MapEvent.m_nY, "");
                        m_VisibleEvents.RemoveAt(I);
                        continue;
                    }
                    if (MapEvent.nVisibleFlag == 2)
                    {
                        SendMsg(this, Grobal2.RM_SHOWEVENT, (short)MapEvent.m_nEventType, MapEvent.Id, HUtil32.MakeLong(MapEvent.m_nX, MapEvent.m_nEventParam), MapEvent.m_nY, "");
                    }
                    I++;
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(e.StackTrace);
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
            var sCharName = string.Empty;
            var sGuildName = string.Empty;
            var sDearName = string.Empty;
            var sMasterName = string.Empty;
            const string sExceptionMsg = "[Exception] TPlayObject::GetShowName";
            try
            {
                if (m_MyGuild != null)
                {
                    var Castle = M2Share.CastleManager.IsCastleMember(this);
                    if (Castle != null)
                    {
                        sGuildName = M2Share.g_sCastleGuildName.Replace("%castlename", Castle.m_sName);
                        sGuildName = sGuildName.Replace("%guildname", m_MyGuild.sGuildName);
                        sGuildName = sGuildName.Replace("%rankname", m_sGuildRankName);
                    }
                    else
                    {
                        Castle = M2Share.CastleManager.InCastleWarArea(this);// 01/25 多城堡
                        if (M2Share.g_Config.boShowGuildName || Castle != null && Castle.m_boUnderWar || m_boInFreePKArea)
                        {
                            sGuildName = M2Share.g_sNoCastleGuildName.Replace("%guildname", m_MyGuild.sGuildName);
                            sGuildName = sGuildName.Replace("%rankname", m_sGuildRankName);
                        }
                    }
                }
                if (!M2Share.g_Config.boShowRankLevelName)
                {
                    if (m_btReLevel > 0)
                    {
                        switch (m_btJob)
                        {
                            case M2Share.jWarr:
                                sCharName = M2Share.g_sWarrReNewName.Replace("%chrname", m_sCharName);
                                break;
                            case M2Share.jWizard:
                                sCharName = M2Share.g_sWizardReNewName.Replace("%chrname", m_sCharName);
                                break;
                            case M2Share.jTaos:
                                sCharName = M2Share.g_sTaosReNewName.Replace("%chrname", m_sCharName);
                                break;
                        }
                    }
                    else
                    {
                        sCharName = m_sCharName;
                    }
                }
                else
                {
                    sCharName = format(m_sRankLevelName, m_sCharName);
                }
                if (!string.IsNullOrEmpty(m_sMasterName))
                {
                    if (m_boMaster)
                    {
                        sMasterName = format(M2Share.g_sMasterName, m_sMasterName);
                    }
                    else
                    {
                        sMasterName = format(M2Share.g_sNoMasterName, m_sMasterName);
                    }
                }
                if (!string.IsNullOrEmpty(m_sDearName))
                {
                    if (m_btGender == PlayGender.Man)
                    {
                        sDearName = format(M2Share.g_sManDearName, m_sDearName);
                    }
                    else
                    {
                        sDearName = format(M2Share.g_sWoManDearName, m_sDearName);
                    }
                }
                sShowName = M2Share.g_sHumanShowName.Replace("%chrname", sCharName);
                sShowName = sShowName.Replace("%guildname", sGuildName);
                sShowName = sShowName.Replace("%dearname", sDearName);
                sShowName = sShowName.Replace("%mastername", sMasterName);
                result = sShowName;
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.Message);
            }
            return result;
        }

        public override void MakeGhost()
        {
            string sSayMsg;
            TPlayObject Human;
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
                    if (m_btGender == PlayGender.Man)
                    {
                        sSayMsg = M2Share.g_sManLongOutDearOnlineMsg.Replace("%d", m_sDearName);
                        sSayMsg = sSayMsg.Replace("%s", m_sCharName);
                        sSayMsg = sSayMsg.Replace("%m", m_PEnvir.sMapDesc);
                        sSayMsg = sSayMsg.Replace("%x", m_nCurrX.ToString());
                        sSayMsg = sSayMsg.Replace("%y", m_nCurrY.ToString());
                        m_DearHuman.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                    }
                    else
                    {
                        sSayMsg = M2Share.g_sWoManLongOutDearOnlineMsg.Replace("%d", m_sDearName);
                        sSayMsg = sSayMsg.Replace("%s", m_sCharName);
                        sSayMsg = sSayMsg.Replace("%m", m_PEnvir.sMapDesc);
                        sSayMsg = sSayMsg.Replace("%x", m_nCurrX.ToString());
                        sSayMsg = sSayMsg.Replace("%y", m_nCurrY.ToString());
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
                            sSayMsg = M2Share.g_sMasterLongOutMasterListOnlineMsg.Replace("%s", m_sCharName);
                            sSayMsg = sSayMsg.Replace("%m", m_PEnvir.sMapDesc);
                            sSayMsg = sSayMsg.Replace("%x", m_nCurrX.ToString());
                            sSayMsg = sSayMsg.Replace("%y", m_nCurrY.ToString());
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
                        sSayMsg = sSayMsg.Replace("%s", m_sCharName);
                        sSayMsg = sSayMsg.Replace("%m", m_PEnvir.sMapDesc);
                        sSayMsg = sSayMsg.Replace("%x", m_nCurrX.ToString());
                        sSayMsg = sSayMsg.Replace("%y", m_nCurrY.ToString());
                        m_MasterHuman.SysMsg(sSayMsg, MsgColor.Red, MsgType.Hint);
                        // 如果为大徒弟则将对方的记录清空
                        if (m_MasterHuman.m_sMasterName == m_sCharName)
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
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.Message);
            }
            base.MakeGhost();
        }

        protected override void ScatterBagItems(TBaseObject ItemOfCreat)
        {
            const int DropWide = 2;
            TUserItem pu;
            const string sExceptionMsg = "[Exception] TPlayObject::ScatterBagItems";
            IList<TDeleteItem> DelList = null;
            if (m_boAngryRing || m_boNoDropItem || m_PEnvir.Flag.boNODROPITEM)
            {
                return;// 不死戒指
            }
            var boDropall = M2Share.g_Config.boDieRedScatterBagAll && PKLevel() >= 2;
            try
            {
                for (var i = m_ItemList.Count - 1; i >= 0; i--)
                {
                    if (boDropall || M2Share.RandomNumber.Random(M2Share.g_Config.nDieScatterBagRate) == 0)
                    {
                        if (DropItemDown(m_ItemList[i], DropWide, true, ItemOfCreat, this))
                        {
                            pu = m_ItemList[i];
                            if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                            {
                                if (DelList == null)
                                {
                                    DelList = new List<TDeleteItem>();
                                }
                                DelList.Add(new TDeleteItem()
                                {
                                    sItemName = M2Share.UserEngine.GetStdItemName(pu.wIndex),
                                    MakeIndex = pu.MakeIndex
                                });
                            }
                            Dispose(m_ItemList[i]);
                            m_ItemList.RemoveAt(i);
                        }
                    }
                }
                if (DelList != null)
                {
                    var ObjectId = HUtil32.Sequence();
                    M2Share.ObjectManager.AddOhter(ObjectId, DelList);
                    this.SendMsg(this, Grobal2.RM_SENDDELITEMLIST, 0, ObjectId, 0, 0, "");
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
        }
    }
}