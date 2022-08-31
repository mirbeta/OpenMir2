using GameSvr.Actor;
using GameSvr.Configs;
using GameSvr.Items;
using GameSvr.Magic;
using GameSvr.Maps;
using GameSvr.Player;
using System.Collections;
using SystemModule;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.RobotPlay
{
    /// <summary>
    /// 假人
    /// </summary>
    public partial class RobotPlayObject : TPlayObject
    {
        public long m_dwSearchTargetTick = 0;
        /// <summary>
        /// 假人启动
        /// </summary>
        public bool m_boAIStart = false;
        /// <summary>
        /// 挂机地图
        /// </summary>
        public Envirnoment m_ManagedEnvir = null;
        public TPointManager m_PointManager = null;
        public PointInfo[] m_Path;
        public int m_nPostion = 0;
        public int m_nMoveFailCount = 0;
        public string m_sConfigListFileName = string.Empty;
        public string m_sHeroConfigListFileName = string.Empty;
        public string m_sFilePath = string.Empty;
        public string m_sConfigFileName = string.Empty;
        public string m_sHeroConfigFileName = string.Empty;
        public IList<string> m_BagItemNames = null;
        public string[] m_UseItemNames;
        public TRunPos m_RunPos = null;
        /// <summary>
        /// 魔法使用间隔
        /// </summary>
        public long[] m_SkillUseTick;
        public int m_nSelItemType = 0;
        public int m_nIncSelfHealthCount = 0;
        public int m_nIncMasterHealthCount = 0;
        /// <summary>
        /// 攻击方式
        /// </summary>
        public short m_wHitMode = 0;
        public bool m_boSelSelf = false;
        public byte m_btTaoistUseItemType = 0;
        public long m_dwAutoRepairItemTick = 0;
        public long m_dwAutoAddHealthTick = 0;
        public long m_dwThinkTick = 0;
        public bool m_boDupMode = false;
        public long m_dwSearchMagic = 0;
        /// <summary>
        /// 低血回城间隔
        /// </summary>
        public long m_dwHPToMapHomeTick = 0;
        /// <summary>
        /// 守护模式
        /// </summary>
        public bool m_boProtectStatus = false;
        public short m_nProtectTargetX = 0;
        public short m_nProtectTargetY = 0;
        /// <summary>
        /// 到达守护坐标
        /// </summary>
        public bool m_boProtectOK = false;
        /// <summary>
        /// 是向守护坐标的累计数
        /// </summary>
        public int m_nGotoProtectXYCount = 0;
        public long m_dwPickUpItemTick = 0;
        public MapItem m_SelMapItem = null;
        /// <summary>
        /// 跑步计时
        /// </summary>
        public long dwTick5F4 = 0;
        /// <summary>
        /// 受攻击说话列表
        /// </summary>
        public ArrayList m_AISayMsgList = null;
        /// <summary>
        /// 绿红毒标识
        /// </summary>
        public byte n_AmuletIndx = 0;
        public bool m_boCanPickIng = false;
        /// <summary>
        /// 查询魔法
        /// </summary>
        public short m_nSelectMagic = 0;
        /// <summary>
        /// 是否可以使用的魔法(True才可能躲避)
        /// </summary>
        public bool m_boIsUseMagic = false;
        /// <summary>
        /// 是否可以使用的攻击魔法
        /// </summary>
        public bool m_boIsUseAttackMagic = false;
        /// <summary>
        /// 最后的方向
        /// </summary>
        public byte m_btLastDirection = 0;
        /// <summary>
        /// 自动躲避间隔
        /// </summary>
        public long m_dwAutoAvoidTick = 0;
        public bool m_boIsNeedAvoid = false;
        /// <summary>
        /// 假人掉装备机率
        /// </summary>
        public int m_nDropUseItemRate;
        private readonly AIObjectConf _conf = null;

        public RobotPlayObject() : base()
        {
            m_nSoftVersionDate = Grobal2.CLIENT_VERSION_NUMBER;
            m_nSoftVersionDateEx = M2Share.GetExVersionNO(Grobal2.CLIENT_VERSION_NUMBER, ref m_nSoftVersionDate);
            AbilCopyToWAbil();
            m_btAttatckMode = 0;
            m_boAI = true;
            m_boLoginNoticeOK = true;
            m_boAIStart = false; // 开始挂机
            m_ManagedEnvir = null; // 挂机地图
            m_Path = null;
            m_nPostion = -1;
            m_sFilePath = "";
            m_sConfigFileName = "";
            m_sHeroConfigFileName = "";
            m_sConfigListFileName = "";
            m_sHeroConfigListFileName = "";
            m_UseItemNames = new string[13];
            m_BagItemNames = new List<string>();
            m_PointManager = new TPointManager(this);
            m_SkillUseTick = new long[59];// 魔法使用间隔
            m_nSelItemType = 1;
            m_nIncSelfHealthCount = 0;
            m_nIncMasterHealthCount = 0;
            m_boSelSelf = false;
            m_btTaoistUseItemType = 0;
            m_dwAutoAddHealthTick = HUtil32.GetTickCount();
            m_dwAutoRepairItemTick = HUtil32.GetTickCount();
            m_dwThinkTick = HUtil32.GetTickCount();
            m_boDupMode = false;
            m_boProtectStatus = false;// 守护模式
            m_boProtectOK = true;// 到达守护坐标
            m_nGotoProtectXYCount = 0;// 是向守护坐标的累计数
            m_SelMapItem = null;
            m_dwPickUpItemTick = HUtil32.GetTickCount();
            m_AISayMsgList = new ArrayList();// 受攻击说话列表
            n_AmuletIndx = 0;
            m_boCanPickIng = false;
            m_nSelectMagic = 0;
            m_boIsUseMagic = false;// 是否能躲避
            m_boIsUseAttackMagic = false;
            m_btLastDirection = Direction;
            m_dwAutoAvoidTick = HUtil32.GetTickCount();// 自动躲避间隔
            m_boIsNeedAvoid = false;// 是否需要躲避
            m_dwWalkTick = HUtil32.GetTickCount();
            m_nWalkSpeed = 300;
            m_RunPos = new TRunPos();
            m_Path = new PointInfo[0];
            var sFileName = GetRandomConfigFileName(m_sCharName, 0);
            if (sFileName == "" || !File.Exists(sFileName))
            {
                if (m_sConfigFileName != "" && File.Exists(m_sConfigFileName))
                {
                    sFileName = m_sConfigFileName;
                }
            }
            _conf = new AIObjectConf(sFileName);
        }

        /// <summary>
        /// 取随机配置
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="nType"></param>
        /// <returns></returns>
        private string GetRandomConfigFileName(string sName, byte nType)
        {
            string result = string.Empty;
            int nIndex;
            string sFileName;
            string Str;
            StringList LoadList;
            if (!Directory.Exists(m_sFilePath + "RobotIni"))
            {
                Directory.CreateDirectory(m_sFilePath + "RobotIni");
            }
            sFileName = Path.Combine(m_sFilePath, "RobotIni", sName + ".txt");
            if (File.Exists(sFileName))
            {
                result = sFileName;
                return result;
            }
            result = sFileName = Path.Combine(m_sFilePath, "RobotIni", "默认.txt");
            switch (nType)
            {
                case 0:
                    if (m_sConfigListFileName != "" && File.Exists(m_sConfigListFileName))
                    {
                        LoadList = new StringList();
                        LoadList.LoadFromFile(m_sConfigListFileName);
                        nIndex = M2Share.RandomNumber.Random(LoadList.Count);
                        if (nIndex >= 0 && nIndex < LoadList.Count)
                        {
                            Str = LoadList[nIndex];
                            if (!string.IsNullOrEmpty(Str))
                            {
                                if (Str[1] == '\\')
                                {
                                    Str = Str.Substring(1, Str.Length - 1);
                                }
                                if (Str[2] == '\\')
                                {
                                    Str = Str.Substring(2, Str.Length - 2);
                                }
                                if (Str[3] == '\\')
                                {
                                    Str = Str.Substring(3, Str.Length - 3);
                                }
                            }
                            result = m_sFilePath + Str;
                        }
                    }
                    break;
                case 1:
                    if (m_sHeroConfigListFileName != "" && File.Exists(m_sHeroConfigListFileName))
                    {
                        LoadList = new StringList();
                        LoadList.LoadFromFile(m_sHeroConfigListFileName);
                        nIndex = M2Share.RandomNumber.Random(LoadList.Count);
                        if (nIndex >= 0 && nIndex < LoadList.Count)
                        {
                            Str = LoadList[nIndex];
                            if (Str != "")
                            {
                                if (Str[1] == '\\')
                                {
                                    Str = Str.Substring(1, Str.Length - 1);
                                }
                                if (Str[2] == '\\')
                                {
                                    Str = Str.Substring(2, Str.Length - 2);
                                }
                                if (Str[3] == '\\')
                                {
                                    Str = Str.Substring(3, Str.Length - 3);
                                }
                            }
                            result = m_sFilePath + Str;
                        }
                    }
                    break;
            }
            return result;
        }

        public void Start(TPathType PathType)
        {
            if (!m_boGhost && !m_boDeath && !m_boAIStart)
            {
                m_ManagedEnvir = m_PEnvir;
                m_nProtectTargetX = m_nCurrX;// 守护坐标
                m_nProtectTargetY = m_nCurrY;// 守护坐标
                m_boProtectOK = false;
                m_nGotoProtectXYCount = 0;// 是向守护坐标的累计数
                m_PointManager.PathType = PathType;
                m_PointManager.Initialize(m_PEnvir);
                m_boAIStart = true;
                m_nMoveFailCount = 0;
                if (M2Share.g_FunctionNPC != null)
                {
                    m_nScriptGotoCount = 0;
                    M2Share.g_FunctionNPC.GotoLable(this, "@AIStart", false);
                }
            }
        }

        public void Stop()
        {
            if (m_boAIStart)
            {
                m_boAIStart = false;
                m_nMoveFailCount = 0;
                m_Path = null;
                m_nPostion = -1;
                if (M2Share.g_FunctionNPC != null)
                {
                    m_nScriptGotoCount = 0;
                    M2Share.g_FunctionNPC.GotoLable(this, "@AIStop", false);
                }
            }
        }

        private void WinExp(int dwExp)
        {
            if (m_Abil.Level > M2Share.g_Config.nLimitExpLevel)
            {
                dwExp = M2Share.g_Config.nLimitExpValue;
                GetExp(dwExp);
            }
            else if (dwExp > 0)
            {
                dwExp = M2Share.g_Config.dwKillMonExpMultiple * dwExp; // 系统指定杀怪经验倍数
                dwExp = m_nKillMonExpMultiple * dwExp; // 人物指定的杀怪经验倍数
                dwExp = HUtil32.Round(m_nKillMonExpRate / 100 * dwExp); // 人物指定的杀怪经验倍数
                if (m_PEnvir.Flag.boEXPRATE)
                {
                    dwExp = HUtil32.Round(m_PEnvir.Flag.nEXPRATE / 100 * dwExp); // 地图上指定杀怪经验倍数
                }
                if (m_boExpItem) // 物品经验倍数
                {
                    dwExp = HUtil32.Round(m_rExpItem * dwExp);
                }
                GetExp(dwExp);
            }
        }

        private void GetExp(int dwExp)
        {
            m_Abil.Exp += dwExp;
            AddBodyLuck(dwExp * 0.002);
            SendMsg(this, Grobal2.RM_WINEXP, 0, dwExp, 0, 0, "");
            if (m_Abil.Exp >= m_Abil.MaxExp)
            {
                m_Abil.Exp -= m_Abil.MaxExp;
                if (m_Abil.Level < M2Share.MAXUPLEVEL)
                {
                    m_Abil.Level++;
                }
                HasLevelUp(m_Abil.Level - 1);
                AddBodyLuck(100);
                M2Share.AddGameDataLog("12" + "\t" + m_sMapName + "\t" + m_Abil.Level + "\t" + m_Abil.Exp +
                                       "\t" + m_sCharName + "\t" + '0' + "\t" + '0' + "\t" + '1' + "\t" + '0');
                IncHealthSpell(2000, 2000);
            }
        }

        public override void MakeGhost()
        {
            if (m_boAIStart)
            {
                m_boAIStart = false;
            }
            base.MakeGhost();
        }

        protected override void Whisper(string whostr, string saystr)
        {
            TPlayObject PlayObject = M2Share.UserEngine.GetPlayObject(whostr);
            if (PlayObject != null)
            {
                if (!PlayObject.m_boReadyRun)
                {
                    return;
                }
                if (!PlayObject.m_boHearWhisper || PlayObject.IsBlockWhisper(m_sCharName))
                {
                    return;
                }
                if (m_btPermission > 0)
                {
                    PlayObject.SendMsg(PlayObject, Grobal2.RM_WHISPER, 0, M2Share.g_Config.btGMWhisperMsgFColor, M2Share.g_Config.btGMWhisperMsgBColor, 0, format("{0}[{1}级]=> {2}", new object[] { m_sCharName, m_Abil.Level, saystr }));
                    // 取得私聊信息
                    // m_GetWhisperHuman 侦听私聊对象
                    if (m_GetWhisperHuman != null && !m_GetWhisperHuman.m_boGhost)
                    {
                        m_GetWhisperHuman.SendMsg(m_GetWhisperHuman, Grobal2.RM_WHISPER, 0, M2Share.g_Config.btGMWhisperMsgFColor, M2Share.g_Config.btGMWhisperMsgBColor, 0, format("{0}[{1}级]=> {2} {3}", new object[] { m_sCharName, m_Abil.Level, PlayObject.m_sCharName, saystr }));
                    }
                    if (PlayObject.m_GetWhisperHuman != null && !PlayObject.m_GetWhisperHuman.m_boGhost)
                    {
                        PlayObject.m_GetWhisperHuman.SendMsg(PlayObject.m_GetWhisperHuman, Grobal2.RM_WHISPER, 0, M2Share.g_Config.btGMWhisperMsgFColor, M2Share.g_Config.btGMWhisperMsgBColor, 0, format("{0}[{1}级]=> {2} {3}", new object[] { m_sCharName, m_Abil.Level, PlayObject.m_sCharName, saystr }));
                    }
                }
                else
                {
                    PlayObject.SendMsg(PlayObject, Grobal2.RM_WHISPER, 0, M2Share.g_Config.btGMWhisperMsgFColor, M2Share.g_Config.btWhisperMsgBColor, 0, format("{0}[{1}级]=> {2}", new object[] { m_sCharName, m_Abil.Level, saystr }));
                    if (m_GetWhisperHuman != null && !m_GetWhisperHuman.m_boGhost)
                    {
                        m_GetWhisperHuman.SendMsg(m_GetWhisperHuman, Grobal2.RM_WHISPER, 0, M2Share.g_Config.btGMWhisperMsgFColor, M2Share.g_Config.btWhisperMsgBColor, 0, format("{0}[{1}级]=> {2} {3}", new object[] { m_sCharName, m_Abil.Level, PlayObject.m_sCharName, saystr }));
                    }
                    if (PlayObject.m_GetWhisperHuman != null && !PlayObject.m_GetWhisperHuman.m_boGhost)
                    {
                        PlayObject.m_GetWhisperHuman.SendMsg(PlayObject.m_GetWhisperHuman, Grobal2.RM_WHISPER, 0, M2Share.g_Config.btGMWhisperMsgFColor, M2Share.g_Config.btWhisperMsgBColor, 0, format("{0}[{1}级]=> {2} {3}", new object[] { m_sCharName, m_Abil.Level, PlayObject.m_sCharName, saystr }));
                    }
                }
            }
        }

        protected override void ProcessSayMsg(string sData)
        {
            bool boDisableSayMsg;
            string sParam1 = string.Empty;
            const string sExceptionMsg = "TAIPlayObject.ProcessSayMsg Msg:%s";
            if (string.IsNullOrEmpty(sData))
            {
                return;
            }
            try
            {
                if (sData.Length > M2Share.g_Config.nSayMsgMaxLen)
                {
                    sData = sData.Substring(0, M2Share.g_Config.nSayMsgMaxLen);
                }
                if (HUtil32.GetTickCount() >= m_dwDisableSayMsgTick)
                {
                    m_boDisableSayMsg = false;
                }
                boDisableSayMsg = m_boDisableSayMsg;
                //g_DenySayMsgList.Lock;
                //if (g_DenySayMsgList.GetIndex(m_sCharName) >= 0)
                //{
                //    boDisableSayMsg = true;
                //}
                // g_DenySayMsgList.UnLock;
                if (!boDisableSayMsg)
                {
                    string SC;
                    if (sData[0] == '/')
                    {
                        SC = sData.Substring(1, sData.Length - 1);
                        SC = HUtil32.GetValidStr3(SC, ref sParam1, new char[] { ' ' });
                        if (!m_boFilterSendMsg)
                        {
                            Whisper(sParam1, SC);
                        }
                        return;
                    }
                    if (sData[0] == '!')
                    {
                        if (sData.Length >= 2)
                        {
                            if (sData[1] == '!')//发送组队消息
                            {
                                SC = sData.Substring(2, sData.Length - 2);
                                SendGroupText(m_sCharName + ": " + SC);
                                return;
                            }
                            if (sData[1] == '~') //发送行会消息
                            {
                                if (m_MyGuild != null)
                                {
                                    SC = sData.Substring(2, sData.Length - 2);
                                    m_MyGuild.SendGuildMsg(m_sCharName + ": " + SC);
                                }
                                return;
                            }
                        }
                        if (!m_PEnvir.Flag.boQUIZ) //发送黄色喊话消息
                        {
                            if ((HUtil32.GetTickCount() - m_dwShoutMsgTick) > 10 * 1000)
                            {
                                if (m_Abil.Level <= M2Share.g_Config.nCanShoutMsgLevel)
                                {
                                    SysMsg(format(M2Share.g_sYouNeedLevelMsg, M2Share.g_Config.nCanShoutMsgLevel + 1), MsgColor.Red, MsgType.Hint);
                                    return;
                                }
                                m_dwShoutMsgTick = HUtil32.GetTickCount();
                                SC = sData.Substring(1, sData.Length - 1);
                                string sCryCryMsg = "(!)" + m_sCharName + ": " + SC;
                                if (m_boFilterSendMsg)
                                {
                                    SendMsg(null, Grobal2.RM_CRY, 0, 0, 0xFFFF, 0, sCryCryMsg);
                                }
                                else
                                {
                                    M2Share.UserEngine.CryCry(Grobal2.RM_CRY, m_PEnvir, m_nCurrX, m_nCurrY, 50, M2Share.g_Config.btCryMsgFColor, M2Share.g_Config.btCryMsgBColor, sCryCryMsg);
                                }
                                return;
                            }
                            SysMsg(format(M2Share.g_sYouCanSendCyCyLaterMsg, new object[] { 10 - (HUtil32.GetTickCount() - m_dwShoutMsgTick) / 1000 }), MsgColor.Red, MsgType.Hint);
                            return;
                        }
                        SysMsg(M2Share.g_sThisMapDisableSendCyCyMsg, MsgColor.Red, MsgType.Hint);
                        return;
                    }
                    if (!m_boFilterSendMsg)
                    {
                        SendRefMsg(Grobal2.RM_HEAR, 0, M2Share.g_Config.btHearMsgFColor, M2Share.g_Config.btHearMsgBColor, 0, m_sCharName + ':' + sData);
                    }
                }
            }
            catch (Exception)
            {
                M2Share.MainOutMessage(format(sExceptionMsg, new object[] { sData }));
            }
        }

        public TUserMagic FindMagic(short wMagIdx)
        {
            TUserMagic result = null;
            TUserMagic UserMagic;
            for (var i = 0; i < m_MagicList.Count; i++)
            {
                UserMagic = m_MagicList[i];
                if (UserMagic.MagicInfo.wMagicID == wMagIdx)
                {
                    result = UserMagic;
                    break;
                }
            }
            return result;
        }

        public TUserMagic FindMagic(string sMagicName)
        {
            TUserMagic result = null;
            for (var i = 0; i < m_MagicList.Count; i++)
            {
                TUserMagic UserMagic = m_MagicList[i];
                if (String.Compare(UserMagic.MagicInfo.sMagicName, sMagicName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = UserMagic;
                    break;
                }
            }
            return result;
        }

        private bool RunToNext(short nX, short nY)
        {
            bool result = false;
            if ((HUtil32.GetTickCount() - dwTick5F4) > M2Share.g_Config.nAIRunIntervalTime)
            {
                result = RunTo1(M2Share.GetNextDirection(m_nCurrX, m_nCurrY, nX, nY), false, nX, nY);
                dwTick5F4 = HUtil32.GetTickCount();
                //m_dwStationTick = HUtil32.GetTickCount();// 增加检测人物站立时间
            }
            return result;
        }

        private bool WalkToNext(int nX, int nY)
        {
            bool result = false;
            if (HUtil32.GetTickCount() - dwTick3F4 > M2Share.g_Config.nAIWalkIntervalTime)
            {
                result = WalkTo(M2Share.GetNextDirection(m_nCurrX, m_nCurrY, nX, nY), false);
                if (result)
                {
                    dwTick3F4 = HUtil32.GetTickCount();
                }
                //m_dwStationTick = HUtil32.GetTickCount();// 增加检测人物站立时间
            }
            return result;
        }

        private bool GotoNextOne(short nX, short nY, bool boRun)
        {
            bool result = false;
            if (Math.Abs(nX - m_nCurrX) <= 2 && Math.Abs(nY - m_nCurrY) <= 2)
            {
                if (Math.Abs(nX - m_nCurrX) <= 1 && Math.Abs(nY - m_nCurrY) <= 1)
                {
                    result = WalkToNext(nX, nY);
                }
                else
                {
                    result = RunToNext(nX, nY);
                }
            }
            m_RunPos.nAttackCount = 0;
            return result;
        }

        public void Hear(int nIndex, string sMsg)
        {
            int nPos;
            bool boDisableSayMsg;
            string sChrName;
            string sSendMsg;
            switch (nIndex)
            {
                case Grobal2.RM_HEAR:
                    break;
                case Grobal2.RM_WHISPER:
                    if (HUtil32.GetTickCount() >= m_dwDisableSayMsgTick)
                    {
                        m_boDisableSayMsg = false;
                    }
                    boDisableSayMsg = m_boDisableSayMsg;
                    // g_DenySayMsgList.Lock;
                    //if (g_DenySayMsgList.GetIndex(m_sCharName) >= 0)
                    //{
                    //    boDisableSayMsg = true;
                    //}
                    // g_DenySayMsgList.UnLock;
                    if (!boDisableSayMsg)
                    {
                        nPos = sMsg.IndexOf("=>", StringComparison.Ordinal);
                        if (nPos > 0 && m_AISayMsgList.Count > 0)
                        {
                            sChrName = sMsg.Substring(1 - 1, nPos - 1);
                            sSendMsg = sMsg.Substring(nPos + 3 - 1, sMsg.Length - nPos - 2);
                            Whisper(sChrName, "你猜我是谁.");
                            //Whisper(sChrName, m_AISayMsgList[(M2Share.RandomNumber.Random(m_AISayMsgList.Count)).Next()]);
                            Console.WriteLine("TODO Hear...");
                        }
                    }
                    break;
                case Grobal2.RM_CRY:
                    break;
                case Grobal2.RM_SYSMESSAGE:
                    break;
                case Grobal2.RM_GROUPMESSAGE:
                    break;
                case Grobal2.RM_GUILDMESSAGE:
                    break;
                case Grobal2.RM_MERCHANTSAY:
                    break;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void SearchPickUpItem_SetHideItem(MapItem MapItem)
        {
            VisibleMapItem VisibleMapItem;
            for (var i = 0; i < m_VisibleItems.Count; i++)
            {
                VisibleMapItem = m_VisibleItems[i];
                if (VisibleMapItem != null && VisibleMapItem.nVisibleFlag > 0)
                {
                    if (VisibleMapItem.MapItem == MapItem)
                    {
                        VisibleMapItem.nVisibleFlag = 0;
                        break;
                    }
                }
            }
        }

        public bool SearchPickUpItem_PickUpItem(int nX, int nY)
        {
            bool result = false;
            TUserItem UserItem = null;
            StdItem StdItem;
            MapItem MapItem = m_PEnvir.GetItem(nX, nY);
            if (MapItem == null)
            {
                return result;
            }
            if (string.Compare(MapItem.Name, Grobal2.sSTRING_GOLDNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (m_PEnvir.DeleteFromMap(nX, nY, CellType.OS_ITEMOBJECT, MapItem) == 1)
                {
                    if (this.IncGold(MapItem.Count))
                    {
                        SendRefMsg(Grobal2.RM_ITEMHIDE, 0, MapItem.ObjectId, nX, nY, "");
                        result = true;
                        GoldChanged();
                        SearchPickUpItem_SetHideItem(MapItem);
                        Dispose(MapItem);
                    }
                    else
                    {
                        m_PEnvir.AddToMap(nX, nY, CellType.OS_ITEMOBJECT, MapItem);
                    }
                }
                else
                {
                    m_PEnvir.AddToMap(nX, nY, CellType.OS_ITEMOBJECT, MapItem);
                }
            }
            else
            {
                // 捡物品
                StdItem = M2Share.UserEngine.GetStdItem(MapItem.UserItem.wIndex);
                if (StdItem != null)
                {
                    if (m_PEnvir.DeleteFromMap(nX, nY, CellType.OS_ITEMOBJECT, MapItem) == 1)
                    {
                        UserItem = new TUserItem();
                        UserItem = MapItem.UserItem;
                        StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                        if (StdItem != null && IsAddWeightAvailable(M2Share.UserEngine.GetStdItemWeight(UserItem.wIndex)))
                        {
                            //if (GetCheckItemList(18, StdItem.Name))
                            //{
                            //    // 判断是否为绑定48时物品
                            //    UserItem.AddValue[0] = 2;
                            //    UserItem.MaxDate = HUtil32.IncDayHour(DateTime.Now, 48); // 解绑时间
                            //}
                            if (AddItemToBag(UserItem))
                            {
                                SendRefMsg(Grobal2.RM_ITEMHIDE, 0, MapItem.ObjectId, nX, nY, "");
                                this.SendAddItem(UserItem);
                                m_WAbil.Weight = RecalcBagWeight();
                                result = true;
                                SearchPickUpItem_SetHideItem(MapItem);
                                Dispose(MapItem);
                            }
                            else
                            {
                                Dispose(UserItem);
                                m_PEnvir.AddToMap(nX, nY, CellType.OS_ITEMOBJECT, MapItem);
                            }
                        }
                        else
                        {
                            Dispose(UserItem);
                            m_PEnvir.AddToMap(nX, nY, CellType.OS_ITEMOBJECT, MapItem);
                        }
                    }
                    else
                    {
                        Dispose(UserItem);
                        m_PEnvir.AddToMap(nX, nY, CellType.OS_ITEMOBJECT, MapItem);
                    }
                }
            }
            return result;
        }

        private bool SearchPickUpItem(int nPickUpTime)
        {
            bool result = false;
            VisibleMapItem VisibleMapItem = null;
            bool boFound;
            int n01;
            int n02;
            try
            {
                if ((HUtil32.GetTickCount() - m_dwPickUpItemTick) < nPickUpTime)
                {
                    return result;
                }
                m_dwPickUpItemTick = HUtil32.GetTickCount();
                if (this.IsEnoughBag() && m_TargetCret == null)
                {
                    boFound = false;
                    if (m_SelMapItem != null)
                    {
                        m_boCanPickIng = true;
                        for (var i = 0; i < m_VisibleItems.Count; i++)
                        {
                            VisibleMapItem = m_VisibleItems[i];
                            if (VisibleMapItem != null && VisibleMapItem.nVisibleFlag > 0)
                            {
                                if (VisibleMapItem.MapItem == m_SelMapItem)
                                {
                                    boFound = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!boFound)
                    {
                        m_SelMapItem = null;
                    }
                    if (m_SelMapItem != null)
                    {
                        if (SearchPickUpItem_PickUpItem(m_nCurrX, m_nCurrY))
                        {
                            m_boCanPickIng = false;
                            result = true;
                            return result;
                        }
                    }
                    n01 = 999;
                    VisibleMapItem SelVisibleMapItem = null;
                    boFound = false;
                    if (m_SelMapItem != null)
                    {
                        for (var i = 0; i < m_VisibleItems.Count; i++)
                        {
                            VisibleMapItem = m_VisibleItems[i];
                            if (VisibleMapItem != null && VisibleMapItem.nVisibleFlag > 0)
                            {
                                if (VisibleMapItem.MapItem == m_SelMapItem)
                                {
                                    SelVisibleMapItem = VisibleMapItem;
                                    boFound = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!boFound)
                    {
                        for (var i = 0; i < m_VisibleItems.Count; i++)
                        {
                            VisibleMapItem = m_VisibleItems[i];
                            if (VisibleMapItem != null)
                            {
                                if (VisibleMapItem.nVisibleFlag > 0)
                                {
                                    MapItem MapItem = VisibleMapItem.MapItem;
                                    if (MapItem != null)
                                    {
                                        if (IsAllowAIPickUpItem(VisibleMapItem.sName) && IsAddWeightAvailable(M2Share.UserEngine.GetStdItemWeight(MapItem.UserItem.wIndex)))
                                        {
                                            if (MapItem.OfBaseObject == null || MapItem.OfBaseObject == this || ((TBaseObject)MapItem.OfBaseObject).m_Master == this)
                                            {
                                                if (Math.Abs(VisibleMapItem.nX - m_nCurrX) <= 5 && Math.Abs(VisibleMapItem.nY - m_nCurrY) <= 5)
                                                {
                                                    n02 = Math.Abs(VisibleMapItem.nX - m_nCurrX) + Math.Abs(VisibleMapItem.nY - m_nCurrY);
                                                    if (n02 < n01)
                                                    {
                                                        n01 = n02;
                                                        SelVisibleMapItem = VisibleMapItem;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (SelVisibleMapItem != null)
                    {
                        m_SelMapItem = SelVisibleMapItem.MapItem;
                        if (m_SelMapItem != null)
                        {
                            m_boCanPickIng = true;
                        }
                        else
                        {
                            m_boCanPickIng = false;
                        }
                        if (m_nCurrX != SelVisibleMapItem.nX || m_nCurrY != SelVisibleMapItem.nY)
                        {
                            WalkToTargetXY2(SelVisibleMapItem.nX, VisibleMapItem.nY);
                            result = true;
                        }
                    }
                    else
                    {
                        m_boCanPickIng = false;
                    }
                }
                else
                {
                    m_SelMapItem = null;
                    m_boCanPickIng = false;
                }
            }
            catch
            {
                M2Share.MainOutMessage(" TAIPlayObject.SearchPickUpItem");
            }
            return result;
        }

        private bool IsAllowAIPickUpItem(string sName)
        {
            return true;
        }

        private bool WalkToTargetXY2(int nTargetX, int nTargetY)
        {
            int n10;
            int n14;
            int n20;
            int nOldX;
            int nOldY;
            bool result = false;
            if (m_boTransparent && m_boHideMode)
            {
                m_wStatusTimeArr[Grobal2.STATE_TRANSPARENT] = 1;// 隐身,一动就显身
            }
            if (m_wStatusTimeArr[Grobal2.POISON_STONE] != 0 && !M2Share.g_Config.ClientConf.boParalyCanSpell || m_wStatusTimeArr[Grobal2.POISON_DONTMOVE] != 0 || m_wStatusTimeArr[Grobal2.POISON_LOCKSPELL] != 0)
            {
                return result;// 麻痹不能跑动 
            }
            if (nTargetX != m_nCurrX || nTargetY != m_nCurrY)
            {
                if ((HUtil32.GetTickCount() - dwTick3F4) > m_dwTurnIntervalTime)// 转向间隔
                {
                    n10 = nTargetX;
                    n14 = nTargetY;
                    byte nDir = Grobal2.DR_DOWN;
                    if (n10 > m_nCurrX)
                    {
                        nDir = Grobal2.DR_RIGHT;
                        if (n14 > m_nCurrY)
                        {
                            nDir = Grobal2.DR_DOWNRIGHT;
                        }
                        if (n14 < m_nCurrY)
                        {
                            nDir = Grobal2.DR_UPRIGHT;
                        }
                    }
                    else
                    {
                        if (n10 < m_nCurrX)
                        {
                            nDir = Grobal2.DR_LEFT;
                            if (n14 > m_nCurrY)
                            {
                                nDir = Grobal2.DR_DOWNLEFT;
                            }
                            if (n14 < m_nCurrY)
                            {
                                nDir = Grobal2.DR_UPLEFT;
                            }
                        }
                        else
                        {
                            if (n14 > m_nCurrY)
                            {
                                nDir = Grobal2.DR_DOWN;
                            }
                            else if (n14 < m_nCurrY)
                            {
                                nDir = Grobal2.DR_UP;
                            }
                        }
                    }
                    nOldX = m_nCurrX;
                    nOldY = m_nCurrY;
                    WalkTo(nDir, false);
                    if (nTargetX == m_nCurrX && nTargetY == m_nCurrY)
                    {
                        result = true;
                        dwTick3F4 = HUtil32.GetTickCount();
                    }
                    if (!result)
                    {
                        n20 = M2Share.RandomNumber.Random(3);
                        for (var i = Grobal2.DR_UP; i <= Grobal2.DR_UPLEFT; i++)
                        {
                            if (nOldX == m_nCurrX && nOldY == m_nCurrY)
                            {
                                if (n20 != 0)
                                {
                                    nDir++;
                                }
                                else if (nDir > 0)
                                {
                                    nDir -= 1;
                                }
                                else
                                {
                                    nDir = Grobal2.DR_UPLEFT;
                                }
                                if (nDir > Grobal2.DR_UPLEFT)
                                {
                                    nDir = Grobal2.DR_UP;
                                }
                                WalkTo(nDir, false);
                                if (nTargetX == m_nCurrX && nTargetY == m_nCurrY)
                                {
                                    result = true;
                                    dwTick3F4 = HUtil32.GetTickCount();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        private void GotoProtect()
        {
            byte nDir;
            int n10;
            int n14;
            int n20;
            int nOldX;
            int nOldY;
            if (m_nCurrX != m_nProtectTargetX || m_nCurrY != m_nProtectTargetY)
            {
                n10 = m_nProtectTargetX;
                n14 = m_nProtectTargetY;
                dwTick3F4 = HUtil32.GetTickCount();
                nDir = Grobal2.DR_DOWN;
                if (n10 > m_nCurrX)
                {
                    nDir = Grobal2.DR_RIGHT;
                    if (n14 > m_nCurrY)
                    {
                        nDir = Grobal2.DR_DOWNRIGHT;
                    }
                    if (n14 < m_nCurrY)
                    {
                        nDir = Grobal2.DR_UPRIGHT;
                    }
                }
                else
                {
                    if (n10 < m_nCurrX)
                    {
                        nDir = Grobal2.DR_LEFT;
                        if (n14 > m_nCurrY)
                        {
                            nDir = Grobal2.DR_DOWNLEFT;
                        }
                        if (n14 < m_nCurrY)
                        {
                            nDir = Grobal2.DR_UPLEFT;
                        }
                    }
                    else
                    {
                        if (n14 > m_nCurrY)
                        {
                            nDir = Grobal2.DR_DOWN;
                        }
                        else if (n14 < m_nCurrY)
                        {
                            nDir = Grobal2.DR_UP;
                        }
                    }
                }
                nOldX = m_nCurrX;
                nOldY = m_nCurrY;
                if (Math.Abs(m_nCurrX - m_nProtectTargetX) >= 3 || Math.Abs(m_nCurrY - m_nProtectTargetY) >= 3)
                {
                    //m_dwStationTick = HUtil32.GetTickCount();// 增加检测人物站立时间
                    if (!RunTo1(nDir, false, m_nProtectTargetX, m_nProtectTargetY))
                    {
                        WalkTo(nDir, false);
                        n20 = M2Share.RandomNumber.Random(3);
                        for (var i = Grobal2.DR_UP; i <= Grobal2.DR_UPLEFT; i++)
                        {
                            if (nOldX == m_nCurrX && nOldY == m_nCurrY)
                            {
                                if (n20 != 0)
                                {
                                    nDir++;
                                }
                                else if (nDir > 0)
                                {
                                    nDir -= 1;
                                }
                                else
                                {
                                    nDir = Grobal2.DR_UPLEFT;
                                }
                                if (nDir > Grobal2.DR_UPLEFT)
                                {
                                    nDir = Grobal2.DR_UP;
                                }
                                WalkTo(nDir, false);
                            }
                        }
                    }
                }
                else
                {
                    WalkTo(nDir, false);
                    //m_dwStationTick = HUtil32.GetTickCount();// 增加检测人物站立时间
                    n20 = M2Share.RandomNumber.Random(3);
                    for (var i = Grobal2.DR_UP; i <= Grobal2.DR_UPLEFT; i++)
                    {
                        if (nOldX == m_nCurrX && nOldY == m_nCurrY)
                        {
                            if (n20 != 0)
                            {
                                nDir++;
                            }
                            else if (nDir > 0)
                            {
                                nDir -= 1;
                            }
                            else
                            {
                                nDir = Grobal2.DR_UPLEFT;
                            }
                            if (nDir > Grobal2.DR_UPLEFT)
                            {
                                nDir = Grobal2.DR_UP;
                            }
                            WalkTo(nDir, false);
                        }
                    }
                }
            }
        }

        protected override void Wondering()
        {
            short nX = 0;
            short nY = 0;
            if (m_boAIStart && m_TargetCret == null && !m_boCanPickIng && !m_boGhost && !m_boDeath && !m_boFixedHideMode && !m_boStoneMode && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                nX = m_nCurrX;
                nY = m_nCurrY;
                if (m_Path != null && m_Path.Length > 0 && m_nPostion < m_Path.Length)
                {
                    if (!GotoNextOne(m_Path[m_nPostion].nX, m_Path[m_nPostion].nY, true))
                    {
                        m_Path = null;
                        m_nPostion = -1;
                        m_nMoveFailCount++;
                        m_nPostion++;
                    }
                    else
                    {
                        m_nMoveFailCount = 0;
                        return;
                    }
                }
                else
                {
                    m_Path = null;
                    m_nPostion = -1;
                }
                if (m_PointManager.GetPoint(ref nX, ref nY))
                {
                    if (Math.Abs(nX - m_nCurrX) > 2 || Math.Abs(nY - m_nCurrY) > 2)
                    {
                        m_Path = M2Share.g_FindPath.Find(m_PEnvir, m_nCurrX, m_nCurrY, nX, nY, true);
                        m_nPostion = 0;
                        if (m_Path.Length > 0 && m_nPostion < m_Path.Length)
                        {
                            if (!GotoNextOne(m_Path[m_nPostion].nX, m_Path[m_nPostion].nY, true))
                            {
                                m_Path = null;
                                m_nPostion = -1;
                                m_nMoveFailCount++;
                            }
                            else
                            {
                                m_nMoveFailCount = 0;
                                m_nPostion++;

                                return;
                            }
                        }
                        else
                        {
                            m_Path = null;
                            m_nPostion = -1;
                            m_nMoveFailCount++;
                        }
                    }
                    else
                    {
                        if (GotoNextOne(nX, nY, true))
                        {
                            m_nMoveFailCount = 0;
                        }
                        else
                        {
                            m_nMoveFailCount++;
                        }
                    }
                }
                else
                {
                    if (M2Share.RandomNumber.Random(2) == 1)
                    {
                        TurnTo(M2Share.RandomNumber.RandomByte(8));
                    }
                    else
                    {
                        WalkTo(Direction, false);
                    }
                    m_Path = null;
                    m_nPostion = -1;
                    m_nMoveFailCount++;
                }
            }
            if (m_nMoveFailCount >= 3)
            {
                if (M2Share.RandomNumber.Random(2) == 1)
                {
                    TurnTo(M2Share.RandomNumber.RandomByte(8));
                }
                else
                {
                    WalkTo(Direction, false);
                }
                m_Path = null;
                m_nPostion = -1;
                m_nMoveFailCount = 0;
            }
        }

        public TBaseObject Struck_MINXY(TBaseObject AObject, TBaseObject BObject)
        {
            TBaseObject result;
            int nA = Math.Abs(m_nCurrX - AObject.m_nCurrX) + Math.Abs(m_nCurrY - AObject.m_nCurrY);
            int nB = Math.Abs(m_nCurrX - BObject.m_nCurrX) + Math.Abs(m_nCurrY - BObject.m_nCurrY);
            if (nA > nB)
            {
                result = BObject;
            }
            else
            {
                result = AObject;
            }
            return result;
        }

        private bool CanWalk(short nCurrX, short nCurrY, int nTargetX, int nTargetY, byte nDir, ref int nStep, bool boFlag)
        {
            bool result = false;
            short nX = 0;
            short nY = 0;
            nStep = 0;
            byte btDir;
            if (nDir > 0 && nDir < 8)
            {
                btDir = nDir;
            }
            else
            {
                btDir = M2Share.GetNextDirection(nCurrX, nCurrY, nTargetX, nTargetY);
            }
            if (boFlag)
            {
                if (Math.Abs(nCurrX - nTargetX) <= 1 && Math.Abs(nCurrY - nTargetY) <= 1)
                {
                    if (m_PEnvir.GetNextPosition(nCurrX, nCurrY, btDir, 1, ref nX, ref nY) && nX == nTargetX && nY == nTargetY)
                    {
                        nStep = 1;
                        result = true;
                    }
                }
                else
                {
                    if (m_PEnvir.GetNextPosition(nCurrX, nCurrY, btDir, 2, ref nX, ref nY) && nX == nTargetX && nY == nTargetY)
                    {
                        nStep = 1;
                        result = true;
                    }
                }
            }
            else
            {
                if (m_PEnvir.GetNextPosition(nCurrX, nCurrY, btDir, 1, ref nX, ref nY) && nX == nTargetX && nY == nTargetY)
                {
                    nStep = nStep + 1;
                    return true;
                }
                if (m_PEnvir.GetNextPosition(nX, nY, btDir, 1, ref nX, ref nY) && nX == nTargetX && nY == nTargetY)
                {
                    nStep = nStep + 1;
                    return true;
                }
            }
            return result;
        }

        private bool IsGotoXY(short X1, short Y1, short X2, short Y2)
        {
            bool result = false;
            int nStep = 0;
            //0代替-1
            if (!CanWalk(X1, Y1, X2, Y2, 0, ref nStep, m_btRaceServer != 108))
            {
                PointInfo[] Path = M2Share.g_FindPath.Find(m_PEnvir, X1, Y1, X2, Y2, false);
                if (Path.Length <= 0)
                {
                    return result;
                }
                Path = null;
                result = true;
            }
            else
            {
                result = true;
            }
            return result;
        }

        private bool GotoNext(short nX, short nY, bool boRun)
        {
            bool result = false;
            int nStep = 0;
            if (Math.Abs(nX - m_nCurrX) <= 2 && Math.Abs(nY - m_nCurrY) <= 2)
            {
                if (Math.Abs(nX - m_nCurrX) <= 1 && Math.Abs(nY - m_nCurrY) <= 1)
                {
                    result = WalkToNext(nX, nY);
                }
                else
                {
                    result = RunToNext(nX, nY);
                }
                nStep = 1;
            }
            if (!result)
            {
                PointInfo[] Path = M2Share.g_FindPath.Find(m_PEnvir, m_nCurrX, m_nCurrY, nX, nY, boRun);
                if (Path.Length > 0)
                {
                    for (var i = 0; i < Path.Length; i++)
                    {
                        if (Path[i].nX != m_nCurrX || Path[i].nY != m_nCurrY)
                        {
                            if (Math.Abs(Path[i].nX - m_nCurrX) >= 2 || Math.Abs(Path[i].nY - m_nCurrY) >= 2)
                            {
                                result = RunToNext(Path[i].nX, Path[i].nY);
                            }
                            else
                            {
                                result = WalkToNext(Path[i].nX, Path[i].nY);
                            }
                            if (result)
                            {
                                nStep++;
                            }
                            else
                            {
                                break;
                            }
                            if (nStep >= 3)
                            {
                                break;
                            }
                        }
                    }
                    Path = null;
                }
            }
            m_RunPos.nAttackCount = 0;
            return result;
        }

        protected override bool Operate(TProcessMessage ProcessMsg)
        {
            bool result = false;
            TBaseObject AttackBaseObject;
            try
            {
                if (ProcessMsg.wIdent == Grobal2.RM_STRUCK)
                {
                    if (ProcessMsg.BaseObject == this.ObjectId)
                    {
                        AttackBaseObject = M2Share.ObjectManager.Get(ProcessMsg.nParam3);
                        if (AttackBaseObject != null)
                        {
                            if (AttackBaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                            {
                                SetPKFlag(AttackBaseObject);
                            }
                            SetLastHiter(AttackBaseObject);
                            Struck(AttackBaseObject);
                            BreakHolySeizeMode();
                        }
                        if (M2Share.CastleManager.IsCastleMember(this) != null && AttackBaseObject != null)
                        {
                            AttackBaseObject.bo2B0 = true;
                            AttackBaseObject.m_dw2B4Tick = HUtil32.GetTickCount();
                        }
                        m_nHealthTick = 0;
                        m_nSpellTick = 0;
                        m_nPerHealth -= 1;
                        m_nPerSpell -= 1;
                        m_dwStruckTick = HUtil32.GetTickCount();
                    }
                    result = true;
                }
                else
                {
                    result = base.Operate(ProcessMsg);
                }
            }
            catch (Exception ex)
            {
                M2Share.MainOutMessage(ex.Message);
            }
            return result;
        }

        private int GetRangeTargetCountByDir(byte nDir, short nX, short nY, int nRange)
        {
            int result = 0;
            short nCurrX = nX;
            short nCurrY = nY;
            for (var i = 0; i < nRange; i++)
            {
                if (m_PEnvir.GetNextPosition(nCurrX, nCurrY, nDir, 1, ref nCurrX, ref nCurrY))
                {
                    TBaseObject BaseObject = (TBaseObject)m_PEnvir.GetMovingObject(nCurrX, nCurrY, true);
                    if (BaseObject != null && !BaseObject.m_boDeath && !BaseObject.m_boGhost && (!BaseObject.m_boHideMode || m_boCoolEye) && IsProperTarget(BaseObject))
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        private int GetNearTargetCount()
        {
            int result = 0;
            short nX = 0;
            short nY = 0;
            TBaseObject BaseObject;
            for (var n10 = 0; n10 < 7; n10++)
            {
                if (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, n10, 1, ref nX, ref nY))
                {
                    BaseObject = (TBaseObject)m_PEnvir.GetMovingObject(nX, nY, true);
                    if (BaseObject != null && !BaseObject.m_boDeath && !BaseObject.m_boGhost && IsProperTarget(BaseObject))
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        private int GetNearTargetCount(short nCurrX, short nCurrY)
        {
            int result = 0;
            short nX = 0;
            short nY = 0;
            TBaseObject BaseObject = (TBaseObject)m_PEnvir.GetMovingObject(nCurrX, nCurrY, true);
            if (BaseObject != null && !BaseObject.m_boDeath && !BaseObject.m_boGhost && IsProperTarget(BaseObject))
            {
                result++;
            }
            for (var i = 0; i < 7; i++)
            {
                if (m_PEnvir.GetNextPosition(nCurrX, nCurrY, i, 1, ref nX, ref nY))
                {
                    BaseObject = (TBaseObject)m_PEnvir.GetMovingObject(nX, nY, true);
                    if (BaseObject != null && !BaseObject.m_boDeath && !BaseObject.m_boGhost && IsProperTarget(BaseObject))
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        private int GetMasterRange(int nTargetX, int nTargetY)
        {
            if (m_Master != null)
            {
                short nCurrX = m_Master.m_nCurrX;
                short nCurrY = m_Master.m_nCurrY;
                return Math.Abs(nCurrX - nTargetX) + Math.Abs(nCurrY - nTargetY);
            }
            return 0;
        }

        /// <summary>
        /// 跟随主人
        /// </summary>
        /// <returns></returns>
        private bool FollowMaster()
        {
            short nX = 0;
            short nY = 0;
            short nCurrX = 0;
            short nCurrY = 0;
            int nStep;
            bool boNeed = false;
            bool result = false;
            if (!m_Master.m_boSlaveRelax)
            {
                if (m_PEnvir != m_Master.m_PEnvir || Math.Abs(m_nCurrX - m_Master.m_nCurrX) > 20 || Math.Abs(m_nCurrY - m_Master.m_nCurrY) > 20)
                {
                    boNeed = true;
                }
            }
            if (boNeed)
            {
                m_Master.GetBackPosition(ref nX, ref nY);
                if (!m_Master.m_PEnvir.CanWalk(nX, nY, true))
                {
                    for (var i = 0; i < 7; i++)
                    {
                        if (m_Master.m_PEnvir.GetNextPosition(m_Master.m_nCurrX, m_Master.m_nCurrY, i, 1, ref nX, ref nY))
                        {
                            if (m_Master.m_PEnvir.CanWalk(nX, nY, true))
                            {
                                break;
                            }
                        }
                    }
                }
                DelTargetCreat();
                m_nTargetX = nX;
                m_nTargetY = nY;
                SpaceMove(m_Master.m_PEnvir.SMapName, m_nTargetX, m_nTargetY, 1);
                return true;
            }
            m_Master.GetBackPosition(ref nCurrX, ref nCurrY);
            if (m_TargetCret == null && !m_Master.m_boSlaveRelax)
            {
                for (var i = 0; i < 2; i++)
                {
                    // 判断主人是否在英雄对面
                    if (m_Master.m_PEnvir.GetNextPosition(m_Master.m_nCurrX, m_Master.m_nCurrY, m_Master.Direction, i, ref nX, ref nY))
                    {
                        if (m_nCurrX == nX && m_nCurrY == nY)
                        {
                            if (m_Master.GetBackPosition(ref nX, ref nY) && GotoNext(nX, nY, true))
                            {
                                return true;
                            }
                            for (var k = 0; k < 2; k++)
                            {
                                for (var j = 0; j < 7; j++)
                                {
                                    if (j != m_Master.Direction)
                                    {
                                        if (m_Master.m_PEnvir.GetNextPosition(m_Master.m_nCurrX, m_Master.m_nCurrY, j, k, ref nX, ref nY) && GotoNext(nX, nY, true))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
                if (m_btRaceServer == 108) // 是否为月灵
                {
                    nStep = 0;
                }
                else
                {
                    nStep = 1;
                }
                if (Math.Abs(m_nCurrX - nCurrX) > nStep || Math.Abs(m_nCurrY - nCurrY) > nStep)
                {
                    if (GotoNextOne(nCurrX, nCurrY, true))
                    {
                        return result;
                    }
                    if (GotoNextOne(nX, nY, true))
                    {
                        return result;
                    }
                    for (var j = 0; j < 2; j++)
                    {
                        for (var k = 0; k < 7; k++)
                        {
                            if (k != m_Master.Direction)
                            {
                                if (m_Master.m_PEnvir.GetNextPosition(m_Master.m_nCurrX, m_Master.m_nCurrY, k, j, ref nX, ref nY) && GotoNextOne(nX, nY, true))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        private bool FindVisibleActors(TBaseObject ActorObject)
        {
            bool result = false;
            for (var i = 0; i < m_VisibleActors.Count; i++)
            {
                if (m_VisibleActors[i].BaseObject == ActorObject)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private bool AllowUseMagic(short wMagIdx)
        {
            bool result = false;
            TUserMagic UserMagic = FindMagic(wMagIdx);
            if (UserMagic != null)
            {
                if (!M2Share.MagicManager.IsWarrSkill(UserMagic.wMagIdx))
                {
                    result = UserMagic.btKey == 0 || m_boAI;
                }
                else
                {
                    result = UserMagic.btKey == 0 || m_boAI;
                }
            }
            return result;
        }

        private bool CheckUserItem(int nItemType, int nCount)
        {
            return CheckUserItemType(nItemType, nCount) || GetUserItemList(nItemType, nCount) >= 0;
        }

        private bool CheckItemType(int nItemType, StdItem StdItem)
        {
            bool result = false;
            switch (nItemType)
            {
                case 1:
                    if (StdItem.StdMode == 25 && StdItem.Shape == 1)
                    {
                        result = true;
                    }
                    break;
                case 2:
                    if (StdItem.StdMode == 25 && StdItem.Shape == 2)
                    {
                        result = true;
                    }
                    break;
                case 3:
                    if (StdItem.StdMode == 25 && StdItem.Shape == 3)
                    {
                        result = true;
                    }
                    break;
                case 5:
                    if (StdItem.StdMode == 25 && StdItem.Shape == 5)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        // 自动换毒符
        private bool CheckUserItemType(int nItemType, int nCount)
        {
            bool result = false;
            if (m_UseItems[Grobal2.U_ARMRINGL] != null && m_UseItems[Grobal2.U_ARMRINGL].wIndex > 0 &&
                Math.Round(Convert.ToDouble(m_UseItems[Grobal2.U_ARMRINGL].Dura / 100)) >= nCount)
            {
                StdItem StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[Grobal2.U_ARMRINGL].wIndex);
                if (StdItem != null)
                {
                    result = CheckItemType(nItemType, StdItem);
                }
            }
            return result;
        }

        // 检测包裹中是否有符和毒
        // nType 为指定类型 5 为护身符 1,2 为毒药   3,诅咒术专用
        private int GetUserItemList(int nItemType, int nCount)
        {
            int result = -1;
            for (var i = 0; i < m_ItemList.Count; i++)
            {
                StdItem StdItem = M2Share.UserEngine.GetStdItem(m_ItemList[i].wIndex);
                if (StdItem != null)
                {
                    if (CheckItemType(nItemType, StdItem) && HUtil32.Round(m_ItemList[i].Dura / 100) >= nCount)
                    {
                        result = i;
                        break;
                    }
                }
            }
            return result;
        }

        // 自动换毒符
        private bool UseItem(int nItemType, int nIndex)
        {
            bool result = false;
            if (nIndex >= 0 && nIndex < m_ItemList.Count)
            {
                TUserItem UserItem = m_ItemList[nIndex];
                if (m_UseItems[Grobal2.U_ARMRINGL].wIndex > 0)
                {
                    StdItem StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[Grobal2.U_ARMRINGL].wIndex);
                    if (StdItem != null)
                    {
                        if (CheckItemType(nItemType, StdItem))
                        {
                            result = true;
                        }
                        else
                        {
                            m_ItemList.RemoveAt(nIndex);
                            TUserItem AddUserItem = m_UseItems[Grobal2.U_ARMRINGL];
                            if (AddItemToBag(AddUserItem))
                            {
                                m_UseItems[Grobal2.U_ARMRINGL] = UserItem;
                                Dispose(UserItem);
                                result = true;
                            }
                            else
                            {
                                m_ItemList.Add(UserItem);
                                Dispose(AddUserItem);
                            }
                        }
                    }
                    else
                    {
                        m_ItemList.RemoveAt(nIndex);
                        m_UseItems[Grobal2.U_ARMRINGL] = UserItem;
                        Dispose(UserItem);
                        result = true;
                    }
                }
                else
                {
                    m_ItemList.RemoveAt(nIndex);
                    m_UseItems[Grobal2.U_ARMRINGL] = UserItem;
                    Dispose(UserItem);
                    result = true;
                }
            }
            return result;
        }

        private int GetRangeTargetCount(short nX, short nY, int nRange)
        {
            int result = 0;
            TBaseObject BaseObject;
            IList<TBaseObject> BaseObjectList = new List<TBaseObject>();
            if (m_PEnvir.GetMapBaseObjects(nX, nY, nRange, BaseObjectList))
            {
                for (var i = BaseObjectList.Count - 1; i >= 0; i--)
                {
                    BaseObject = BaseObjectList[i];
                    if (BaseObject.m_boHideMode && !m_boCoolEye || !IsProperTarget(BaseObject))
                    {
                        BaseObjectList.RemoveAt(i);
                    }
                }
                return BaseObjectList.Count;
            }
            return result;
        }

        // 目标是否和自己在一条线上，用来检测直线攻击的魔法是否可以攻击到目标
        private bool CanLineAttack(short nCurrX, short nCurrY)
        {
            bool result = false;
            short nX = nCurrX;
            short nY = nCurrY;
            byte btDir = M2Share.GetNextDirection(nCurrX, nCurrY, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
            while (true)
            {
                if (m_TargetCret.m_nCurrX == nX && m_TargetCret.m_nCurrY == nY)
                {
                    result = true;
                    break;
                }
                btDir = M2Share.GetNextDirection(nX, nY, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
                if (!m_PEnvir.GetNextPosition(nX, nY, btDir, 1, ref nX, ref nY))
                {
                    break;
                }
                if (!m_PEnvir.CanWalkEx(nX, nY, true))
                {
                    break;
                }
            }
            return result;
        }

        // 是否是能直线攻击
        private bool CanLineAttack(int nStep)
        {
            bool result = false;
            short nX = m_nCurrX;
            short nY = m_nCurrY;
            byte btDir = M2Share.GetNextDirection(nX, nY, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
            for (var i = 0; i < nStep; i++)
            {
                if (m_TargetCret.m_nCurrX == nX && m_TargetCret.m_nCurrY == nY)
                {
                    result = true;
                    break;
                }
                btDir = M2Share.GetNextDirection(nX, nY, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
                if (!m_PEnvir.GetNextPosition(nX, nY, btDir, 1, ref nX, ref nY))
                {
                    break;
                }
                if (!m_PEnvir.CanWalkEx(nX, nY, true))
                {
                    break;
                }
            }
            return result;
        }

        private bool CanAttack(short nCurrX, short nCurrY, TBaseObject BaseObject, int nRange, ref byte btDir)
        {
            bool result = false;
            short nX = 0;
            short nY = 0;
            btDir = M2Share.GetNextDirection(nCurrX, nCurrY, BaseObject.m_nCurrX, BaseObject.m_nCurrY);
            for (var i = 0; i < nRange; i++)
            {
                if (!m_PEnvir.GetNextPosition(nCurrX, nCurrY, btDir, i, ref nX, ref nY))
                {
                    break;
                }
                if (BaseObject.m_nCurrX == nX && BaseObject.m_nCurrY == nY)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private bool CanAttack(TBaseObject BaseObject, int nRange, ref byte btDir)
        {
            short nX = 0;
            short nY = 0;
            bool result = false;
            btDir = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, BaseObject.m_nCurrX, BaseObject.m_nCurrY);
            for (var i = 0; i < nRange; i++)
            {
                if (!m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, btDir, i, ref nX, ref nY))
                {
                    break;
                }
                if (BaseObject.m_nCurrX == nX && BaseObject.m_nCurrY == nY)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 检测是否可以使用攻击魔法
        /// </summary>
        /// <returns></returns>
        private bool IsUseAttackMagic()
        {
            TUserMagic UserMagic;
            bool result = false;
            switch (m_btJob)
            {
                case PlayJob.Warrior:
                    result = true;
                    break;
                case PlayJob.Wizard:
                    for (var i = 0; i < m_MagicList.Count; i++)
                    {
                        UserMagic = m_MagicList[i];
                        switch (UserMagic.wMagIdx)
                        {
                            case SpellsDef.SKILL_FIREBALL:
                            case SpellsDef.SKILL_FIREBALL2:
                            case SpellsDef.SKILL_FIRE:
                            case SpellsDef.SKILL_SHOOTLIGHTEN:
                            case SpellsDef.SKILL_LIGHTENING:
                            case SpellsDef.SKILL_EARTHFIRE:
                            case SpellsDef.SKILL_FIREBOOM:
                            case SpellsDef.SKILL_LIGHTFLOWER:
                            case SpellsDef.SKILL_SNOWWIND:
                            case SpellsDef.SKILL_GROUPLIGHTENING:
                            case SpellsDef.SKILL_47:
                            case SpellsDef.SKILL_58:
                                if (GetSpellPoint(UserMagic) <= m_WAbil.MP)
                                {
                                    result = true;
                                    break;
                                }
                                break;
                        }
                    }
                    break;
                case PlayJob.Taoist:
                    for (var i = 0; i < m_MagicList.Count; i++)
                    {
                        UserMagic = m_MagicList[i];
                        if (UserMagic.MagicInfo.btJob == 2 || UserMagic.MagicInfo.btJob == 99)
                        {
                            switch (UserMagic.wMagIdx)
                            {
                                case SpellsDef.SKILL_AMYOUNSUL:
                                case SpellsDef.SKILL_GROUPAMYOUNSUL:// 需要毒药
                                    result = CheckUserItem(1, 2) || CheckUserItem(2, 2);
                                    if (result)
                                    {
                                        result = AllowUseMagic(SpellsDef.SKILL_AMYOUNSUL) || AllowUseMagic(SpellsDef.SKILL_GROUPAMYOUNSUL);
                                    }
                                    if (result)
                                    {
                                        break;
                                    }
                                    break;
                                case SpellsDef.SKILL_FIRECHARM:// 需要符
                                    result = CheckUserItem(5, 1);
                                    if (result)
                                    {
                                        result = AllowUseMagic(SpellsDef.SKILL_FIRECHARM);
                                    }
                                    if (result)
                                    {
                                        break;
                                    }
                                    break;
                                case SpellsDef.SKILL_59:// 需要符
                                    result = CheckUserItem(5, 5);
                                    if (result)
                                    {
                                        result = AllowUseMagic(SpellsDef.SKILL_59);
                                    }
                                    if (result)
                                    {
                                        break;
                                    }
                                    break;
                            }
                        }
                    }
                    break;
            }
            return result;
        }

        private bool UseSpell(TUserMagic UserMagic, short nTargetX, short nTargetY, TBaseObject TargeTBaseObject)
        {
            int n14;
            TBaseObject BaseObject;
            bool boIsWarrSkill;
            bool result = false;
            if (!m_boCanSpell)
            {
                return result;
            }
            if (m_boDeath || m_wStatusTimeArr[Grobal2.POISON_LOCKSPELL] != 0)
            {
                return result; // 防麻
            }
            if (m_wStatusTimeArr[Grobal2.POISON_STONE] != 0 && !M2Share.g_Config.ClientConf.boParalyCanSpell)
            {
                return result;// 防麻
            }
            if (m_PEnvir != null)
            {
                if (!m_PEnvir.AllowMagics(UserMagic.MagicInfo.sMagicName))
                {
                    return result;
                }
            }
            boIsWarrSkill = M2Share.MagicManager.IsWarrSkill(UserMagic.wMagIdx); // 是否是战士技能
            m_nSpellTick -= 450;
            m_nSpellTick = HUtil32._MAX(0, m_nSpellTick);
            switch (UserMagic.wMagIdx)
            {
                case SpellsDef.SKILL_ERGUM:
                    if (m_MagicArr[SpellsDef.SKILL_ERGUM] != null)
                    {
                        if (!m_boUseThrusting)
                        {
                            ThrustingOnOff(true);
                        }
                        else
                        {
                            ThrustingOnOff(false);
                        }
                    }
                    result = true;
                    break;
                case SpellsDef.SKILL_BANWOL:
                    if (m_MagicArr[SpellsDef.SKILL_BANWOL] != null)
                    {
                        if (!m_boUseHalfMoon)
                        {
                            HalfMoonOnOff(true);
                        }
                        else
                        {
                            HalfMoonOnOff(false);
                        }
                    }
                    result = true;
                    break;
                case SpellsDef.SKILL_FIRESWORD:
                    if (m_MagicArr[SpellsDef.SKILL_FIRESWORD] != null)
                    {
                        result = true;
                    }
                    break;
                case SpellsDef.SKILL_MOOTEBO:
                    result = true;
                    if ((HUtil32.GetTickCount() - m_dwDoMotaeboTick) > 3000)
                    {
                        m_dwDoMotaeboTick = HUtil32.GetTickCount();
                        if (GetAttackDir(TargeTBaseObject, ref Direction))
                        {
                            DoMotaebo(Direction, UserMagic.btLevel);
                        }
                    }
                    break;
                case SpellsDef.SKILL_43:
                    result = true;
                    break;
                default:
                    n14 = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, nTargetX, nTargetY);
                    Direction = (byte)n14;
                    BaseObject = null;
                    if (UserMagic.wMagIdx >= 60 && UserMagic.wMagIdx <= 65)
                    {
                        if (CretInNearXY(TargeTBaseObject, nTargetX, nTargetY))// 检查目标角色，与目标座标误差范围，如果在误差范围内则修正目标座标
                        {
                            BaseObject = TargeTBaseObject;
                            nTargetX = BaseObject.m_nCurrX;
                            nTargetY = BaseObject.m_nCurrY;
                        }
                    }
                    else
                    {
                        switch (UserMagic.wMagIdx)
                        {
                            case SpellsDef.SKILL_HEALLING:
                            case SpellsDef.SKILL_HANGMAJINBUB:
                            case SpellsDef.SKILL_DEJIWONHO:
                            case SpellsDef.SKILL_BIGHEALLING:
                            case SpellsDef.SKILL_SINSU:
                            case SpellsDef.SKILL_UNAMYOUNSUL:
                            case SpellsDef.SKILL_46:
                                if (m_boSelSelf)
                                {
                                    BaseObject = this;
                                    nTargetX = m_nCurrX;
                                    nTargetY = m_nCurrY;
                                }
                                else
                                {
                                    if (m_Master != null)
                                    {
                                        BaseObject = m_Master;
                                        nTargetX = m_Master.m_nCurrX;
                                        nTargetY = m_Master.m_nCurrY;
                                    }
                                    else
                                    {
                                        BaseObject = this;
                                        nTargetX = m_nCurrX;
                                        nTargetY = m_nCurrY;
                                    }
                                }
                                break;
                            default:
                                if (CretInNearXY(TargeTBaseObject, nTargetX, nTargetY))
                                {
                                    BaseObject = TargeTBaseObject;
                                    nTargetX = BaseObject.m_nCurrX;
                                    nTargetY = BaseObject.m_nCurrY;
                                }
                                break;
                        }
                    }
                    if (!AutoSpell(UserMagic, nTargetX, nTargetY, BaseObject))
                    {
                        SendRefMsg(Grobal2.RM_MAGICFIREFAIL, 0, 0, 0, 0, "");
                    }
                    result = true;
                    break;
            }
            return result;
        }

        private bool AutoSpell(TUserMagic UserMagic, short nTargetX, short nTargetY, TBaseObject BaseObject)
        {
            bool result = false;
            try
            {
                if (BaseObject != null)
                {
                    if (BaseObject.m_boGhost || BaseObject.m_boDeath || BaseObject.m_WAbil.HP <= 0)
                    {
                        return result;
                    }
                }
                if (!M2Share.MagicManager.IsWarrSkill(UserMagic.wMagIdx))
                {
                    result = M2Share.MagicManager.DoSpell(this, UserMagic, nTargetX, nTargetY, BaseObject);
                    m_dwHitTick = HUtil32.GetTickCount();
                }
            }
            catch (Exception)
            {
                M2Share.MainOutMessage(format("TAIPlayObject.AutoSpell MagID:{0} X:{1} Y:{2}", new object[] { UserMagic.wMagIdx, nTargetX, nTargetY }));
            }
            return result;
        }

        public bool Thinking()
        {
            bool result = false;
            int nOldX;
            int nOldY;
            try
            {
                if (M2Share.g_Config.boAutoPickUpItem)//&& (g_AllowAIPickUpItemList.Count > 0)
                {
                    if (SearchPickUpItem(500))
                    {
                        result = true;
                    }
                }
                if (m_Master != null && m_Master.m_boGhost)
                {
                    return result;
                }
                if (m_Master != null && m_Master.InSafeZone() && InSafeZone())
                {
                    if (Math.Abs(m_nCurrX - m_Master.m_nCurrX) <= 3 && Math.Abs(m_nCurrY - m_Master.m_nCurrY) <= 3)
                    {
                        result = true;
                        return result;
                    }
                }
                if (HUtil32.GetTickCount() - m_dwThinkTick > 3000)
                {
                    m_dwThinkTick = HUtil32.GetTickCount();
                    if (m_PEnvir.GetXyObjCount(m_nCurrX, m_nCurrY) >= 2)
                    {
                        m_boDupMode = true;
                    }
                    if (m_TargetCret != null)
                    {
                        if (!IsProperTarget(m_TargetCret))
                        {
                            DelTargetCreat();
                        }
                    }
                }
                if (m_boDupMode)
                {
                    nOldX = m_nCurrX;
                    nOldY = m_nCurrY;
                    WalkTo(M2Share.RandomNumber.RandomByte(8), false);
                    //m_dwStationTick = HUtil32.GetTickCount(); // 增加检测人物站立时间
                    if (nOldX != m_nCurrX || nOldY != m_nCurrY)
                    {
                        m_boDupMode = false;
                        result = true;
                    }
                }
            }
            catch
            {
                M2Share.MainOutMessage("TAIPlayObject.Thinking");
            }
            return result;
        }

        private int CheckTargetXYCount(int nX, int nY, int nRange)
        {
            TBaseObject BaseObject;
            int nC;
            int n10 = nRange;
            int result = 0;
            if (m_VisibleActors.Count > 0)
            {
                for (var i = 0; i < m_VisibleActors.Count; i++)
                {
                    BaseObject = m_VisibleActors[i].BaseObject;
                    if (BaseObject != null)
                    {
                        if (!BaseObject.m_boDeath)
                        {
                            if (IsProperTarget(BaseObject) && (!BaseObject.m_boHideMode || m_boCoolEye))
                            {
                                nC = Math.Abs(nX - BaseObject.m_nCurrX) + Math.Abs(nY - BaseObject.m_nCurrY);
                                if (nC <= n10)
                                {
                                    result++;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 是否走向目标
        /// </summary>
        /// <returns></returns>
        private bool IsNeedGotoXY()
        {
            bool result = false;
            long dwAttackTime;
            if (m_TargetCret != null && HUtil32.GetTickCount() - m_dwAutoAvoidTick > 1100 && (!m_boIsUseAttackMagic || m_btJob == 0))
            {
                if (m_btJob > 0)
                {
                    if (!m_boIsUseMagic && (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 3 || Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 3))
                    {
                        return true;
                    }
                    if ((M2Share.g_Config.boHeroAttackTarget && m_Abil.Level < 22 || M2Share.g_Config.boHeroAttackTao && m_TargetCret.m_WAbil.MaxHP < 700 && 
                        m_TargetCret.m_btRaceServer != Grobal2.RC_PLAYOBJECT && m_btJob == PlayJob.Taoist) && (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 1 || Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 1))// 道法22前是否物理攻击大于1格时才走向目标
                    {
                        return true;
                    }
                }
                else
                {
                    switch (m_nSelectMagic)
                    {
                        case SpellsDef.SKILL_ERGUM:
                            if (AllowUseMagic(12) && m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, Direction, 2, ref m_nTargetX, ref m_nTargetY))
                            {
                                if (Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 2 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 0 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 0 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 2 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 2 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 2)
                                {
                                    dwAttackTime = HUtil32._MAX(0, (int)M2Share.g_Config.dwHeroWarrorAttackTime - m_nHitSpeed * M2Share.g_Config.ClientConf.btItemSpeed); // 防止负数出错
                                    if (HUtil32.GetTickCount() - m_dwHitTick > dwAttackTime)
                                    {
                                        m_wHitMode = 4;
                                        m_dwTargetFocusTick = HUtil32.GetTickCount();
                                        Direction = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
                                        Attack(m_TargetCret, Direction);
                                        BreakHolySeizeMode();
                                        m_dwHitTick = HUtil32.GetTickCount();
                                        return result;
                                    }
                                }
                                else
                                {
                                    if (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 1 || Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 1)
                                    {
                                        result = true;
                                        return result;
                                    }
                                }
                            }
                            m_nSelectMagic = 0;
                            if (AllowUseMagic(12))
                            {
                                if (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 2 || Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 2)
                                {
                                    result = true;
                                    return result;
                                }
                            }
                            else if (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 1 || Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 1)
                            {
                                result = true;
                                return result;
                            }
                            break;
                        case 43:
                            if (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, Direction, 5, ref m_nTargetX, ref m_nTargetY))
                            {
                                if (Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 4 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 0 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 0 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) <= 4 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 2 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 2 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 3 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 3 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 4 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 4)
                                {
                                    dwAttackTime = HUtil32._MAX(0, (int)M2Share.g_Config.dwHeroWarrorAttackTime - m_nHitSpeed * M2Share.g_Config.ClientConf.btItemSpeed);// 防止负数出错
                                    if (HUtil32.GetTickCount() - m_dwHitTick > dwAttackTime)
                                    {
                                        m_wHitMode = 9;
                                        m_dwTargetFocusTick = HUtil32.GetTickCount();
                                        Direction = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
                                        Attack(m_TargetCret, Direction);
                                        BreakHolySeizeMode();
                                        m_dwHitTick = HUtil32.GetTickCount();
                                        return result;
                                    }
                                }
                                else
                                {
                                    if (AllowUseMagic(12))
                                    {
                                        if (Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) != 2 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) != 0 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) != 0 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) != 2 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) != 2 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) != 2)
                                        {
                                            result = true;
                                            return result;
                                        }
                                    }
                                    else if (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 1 || Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 1)
                                    {
                                        result = true;
                                        return result;
                                    }
                                }
                            }
                            m_nSelectMagic = 0;
                            if (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, Direction, 2, ref m_nTargetX, ref m_nTargetY))
                            {
                                if (Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 2 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 0 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 0 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 2 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 2 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 2)
                                {
                                    dwAttackTime = HUtil32._MAX(0, (int)M2Share.g_Config.dwHeroWarrorAttackTime - m_nHitSpeed * M2Share.g_Config.ClientConf.btItemSpeed);
                                    // 防止负数出错
                                    if (HUtil32.GetTickCount() - m_dwHitTick > dwAttackTime)
                                    {
                                        m_wHitMode = 9;
                                        m_dwTargetFocusTick = HUtil32.GetTickCount();
                                        Direction = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
                                        Attack(m_TargetCret, Direction);
                                        BreakHolySeizeMode();
                                        m_dwHitTick = HUtil32.GetTickCount();
                                        return result;
                                    }
                                }
                                else
                                {
                                    if (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 1 || Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 1)
                                    {
                                        result = true;
                                        return result;
                                    }
                                }
                            }
                            m_nSelectMagic = 0;
                            if (AllowUseMagic(12))
                            {
                                if (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 2 || Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 2)
                                {
                                    result = true;
                                    return result;
                                }
                            }
                            else if (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 1 || Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 1)
                            {
                                result = true;
                                return result;
                            }
                            break;
                        case 7:
                        case 25:
                        case 26:
                            if (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 1 || Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 1)
                            {
                                result = true;
                                m_nSelectMagic = 0;
                                return result;
                            }
                            break;
                        default:
                            if (AllowUseMagic(12))
                            {
                                if (!(Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 2 && Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 0 || Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 1 && Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 0 || Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 1 && Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 1 || Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 2 && Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 2 || Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 0 && Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 1 || Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 0 && Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 2))
                                {
                                    result = true;
                                    return result;
                                }
                                if (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 1 && Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 2 || Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 2 && Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 1)
                                {
                                    result = true;
                                    return result;
                                }
                            }
                            else if (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 1 || Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 1)
                            {
                                result = true;
                                return result;
                            }
                            break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 走向目标
        /// </summary>
        /// <param name="BaseObject"></param>
        /// <param name="nCode"></param>
        /// <returns></returns>
        private bool GetGotoXY(TBaseObject BaseObject, byte nCode)
        {
            bool result = false;
            switch (nCode)
            {
                case 2:
                    // 刺杀位
                    if (m_nCurrX - 2 <= BaseObject.m_nCurrX && m_nCurrX + 2 >= BaseObject.m_nCurrX && m_nCurrY - 2 <= BaseObject.m_nCurrY && m_nCurrY + 2 >= BaseObject.m_nCurrY && (m_nCurrX != BaseObject.m_nCurrX || m_nCurrY != BaseObject.m_nCurrY))
                    {
                        result = true;
                        if (m_nCurrX - 2 == BaseObject.m_nCurrX && m_nCurrY == BaseObject.m_nCurrY)
                        {
                            m_nTargetX = (short)(m_nCurrX - 2);
                            m_nTargetY = m_nCurrY;
                            return result;
                        }
                        if (m_nCurrX + 2 == BaseObject.m_nCurrX && m_nCurrY == BaseObject.m_nCurrY)
                        {
                            m_nTargetX = (short)(m_nCurrX + 2);
                            m_nTargetY = m_nCurrY;
                            return result;
                        }
                        if (m_nCurrX == BaseObject.m_nCurrX && m_nCurrY - 2 == BaseObject.m_nCurrY)
                        {
                            m_nTargetX = m_nCurrX;
                            m_nTargetY = (short)(m_nCurrY - 2);
                            return result;
                        }
                        if (m_nCurrX == BaseObject.m_nCurrX && m_nCurrY + 2 == BaseObject.m_nCurrY)
                        {
                            m_nTargetX = m_nCurrX;
                            m_nTargetY = (short)(m_nCurrY + 2);
                            return result;
                        }
                        if (m_nCurrX - 2 == BaseObject.m_nCurrX && m_nCurrY - 2 == BaseObject.m_nCurrY)
                        {
                            m_nTargetX = (short)(m_nCurrX - 2);
                            m_nTargetY = (short)(m_nCurrY - 2);
                            return result;
                        }
                        if (m_nCurrX + 2 == BaseObject.m_nCurrX && m_nCurrY - 2 == BaseObject.m_nCurrY)
                        {
                            m_nTargetX = (short)(m_nCurrX + 2);
                            m_nTargetY = (short)(m_nCurrY - 2);
                            return result;
                        }
                        if (m_nCurrX - 2 == BaseObject.m_nCurrX && m_nCurrY + 2 == BaseObject.m_nCurrY)
                        {
                            m_nTargetX = (short)(m_nCurrX - 2);
                            m_nTargetY = (short)(m_nCurrY + 2);
                            return result;
                        }
                        if (m_nCurrX + 2 == BaseObject.m_nCurrX && m_nCurrY + 2 == BaseObject.m_nCurrY)
                        {
                            m_nTargetX = (short)(m_nCurrX + 2);
                            m_nTargetY = (short)(m_nCurrY + 2);
                            return result;
                        }
                    }
                    break;
                case 3:
                    // 2
                    // 3格
                    if (m_nCurrX - 3 <= BaseObject.m_nCurrX && m_nCurrX + 3 >= BaseObject.m_nCurrX && m_nCurrY - 3 <= BaseObject.m_nCurrY && m_nCurrY + 3 >= BaseObject.m_nCurrY && (m_nCurrX != BaseObject.m_nCurrX || m_nCurrY != BaseObject.m_nCurrY))
                    {
                        result = true;
                        if (m_nCurrX - 3 == BaseObject.m_nCurrX && m_nCurrY == BaseObject.m_nCurrY)
                        {
                            m_nTargetX = (short)(m_nCurrX - 3);
                            m_nTargetY = m_nCurrY;
                            return result;
                        }
                        if (m_nCurrX + 3 == BaseObject.m_nCurrX && m_nCurrY == BaseObject.m_nCurrY)
                        {
                            m_nTargetX = (short)(m_nCurrX + 3);
                            m_nTargetY = m_nCurrY;
                            return result;
                        }
                        if (m_nCurrX == BaseObject.m_nCurrX && m_nCurrY - 3 == BaseObject.m_nCurrY)
                        {
                            m_nTargetX = m_nCurrX;
                            m_nTargetY = (short)(m_nCurrY - 3);
                            return result;
                        }
                        if (m_nCurrX == BaseObject.m_nCurrX && m_nCurrY + 3 == BaseObject.m_nCurrY)
                        {
                            m_nTargetX = m_nCurrX;
                            m_nTargetY = (short)(m_nCurrY + 3);
                            return result;
                        }
                        if (m_nCurrX - 3 == BaseObject.m_nCurrX && m_nCurrY - 3 == BaseObject.m_nCurrY)
                        {
                            m_nTargetX = (short)(m_nCurrX - 3);
                            m_nTargetY = (short)(m_nCurrY - 3);
                            return result;
                        }
                        if (m_nCurrX + 3 == BaseObject.m_nCurrX && m_nCurrY - 3 == BaseObject.m_nCurrY)
                        {
                            m_nTargetX = (short)(m_nCurrX + 3);
                            m_nTargetY = (short)(m_nCurrY - 3);
                            return result;
                        }
                        if (m_nCurrX - 3 == BaseObject.m_nCurrX && m_nCurrY + 3 == BaseObject.m_nCurrY)
                        {
                            m_nTargetX = (short)(m_nCurrX - 3);
                            m_nTargetY = (short)(m_nCurrY + 3);
                            return result;
                        }
                        if (m_nCurrX + 3 == BaseObject.m_nCurrX && m_nCurrY + 3 == BaseObject.m_nCurrY)
                        {
                            m_nTargetX = (short)(m_nCurrX + 3);
                            m_nTargetY = (short)(m_nCurrY + 3);
                            return result;
                        }
                    }
                    break;
            }
            return result;
        }

        /// <summary>
        /// 跑到目标坐标
        /// </summary>
        /// <param name="nTargetX"></param>
        /// <param name="nTargetY"></param>
        /// <returns></returns>
        private bool RunToTargetXY(short nTargetX, short nTargetY)
        {
            int n10;
            int n14;
            bool result = false;
            if (m_boTransparent && m_boHideMode)
            {
                m_wStatusTimeArr[Grobal2.STATE_TRANSPARENT] = 1;// 隐身,一动就显身
            }
            if (m_wStatusTimeArr[Grobal2.POISON_STONE] > 0 && !M2Share.g_Config.ClientConf.boParalyCanSpell || m_wStatusTimeArr[Grobal2.POISON_DONTMOVE] != 0 || m_wStatusTimeArr[Grobal2.POISON_LOCKSPELL] != 0)// || (m_wStatusArrValue[23] != 0)
            {
                return result; // 麻痹不能跑动 
            }
            if (!m_boCanRun) // 禁止跑,则退出
            {
                return result;
            }
            if (HUtil32.GetTickCount() - dwTick5F4 > m_dwRunIntervalTime) // 跑步使用单独的变量计数
            {
                n10 = nTargetX;
                n14 = nTargetY;
                byte nDir = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, n10, n14);
                if (!RunTo1(nDir, false, nTargetX, nTargetY))
                {
                    result = WalkToTargetXY(nTargetX, nTargetY);
                    if (result)
                    {
                        dwTick5F4 = HUtil32.GetTickCount();
                    }
                }
                else
                {
                    if (Math.Abs(nTargetX - m_nCurrX) <= 1 && Math.Abs(nTargetY - m_nCurrY) <= 1)
                    {
                        result = true;
                        dwTick5F4 = HUtil32.GetTickCount();
                    }
                }
            }
            return result;
        }

        public bool RunTo1(byte btDir, bool boFlag, short nDestX, short nDestY)
        {
            const string sExceptionMsg = "[Exception] TBaseObject::RunTo";
            var result = false;
            try
            {
                int nOldX = m_nCurrX;
                int nOldY = m_nCurrY;
                Direction = btDir;
                switch (btDir)
                {
                    case Grobal2.DR_UP:
                        if (m_nCurrY > 1 &&
                          (m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY - 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) || M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
                        (m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY - 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) || M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
                        m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX, m_nCurrY - 2, true) > 0)
                        {
                            m_nCurrY -= 2;
                        }
                        break;
                    case Grobal2.DR_UPRIGHT:
                        if (m_nCurrX < m_PEnvir.WWidth - 2 &&
                          m_nCurrY > 1 &&
                          (m_PEnvir.CanWalkEx(m_nCurrX + 1, m_nCurrY - 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) || M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
                        (m_PEnvir.CanWalkEx(m_nCurrX + 2, m_nCurrY - 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) || M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
                        m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX + 2, m_nCurrY - 2, true) > 0)
                        {
                            m_nCurrX += 2;
                            m_nCurrY -= 2;
                        }
                        break;
                    case Grobal2.DR_RIGHT:
                        if (m_nCurrX < m_PEnvir.WWidth - 2 &&
  (m_PEnvir.CanWalkEx(m_nCurrX + 1, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) || M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
  (m_PEnvir.CanWalkEx(m_nCurrX + 2, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) || M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
    m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX + 2, m_nCurrY, true) > 0)
                        {
                            m_nCurrX += 2;
                        }
                        break;
                    case Grobal2.DR_DOWNRIGHT:
                        if (m_nCurrX < m_PEnvir.WWidth - 2 &&
  m_nCurrY < m_PEnvir.WHeight - 2 &&
  (m_PEnvir.CanWalkEx(m_nCurrX + 1, m_nCurrY + 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) || M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
  (m_PEnvir.CanWalkEx(m_nCurrX + 2, m_nCurrY + 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) || M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
    m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX + 2, m_nCurrY + 2, true) > 0)
                        {
                            m_nCurrX += 2;
                            m_nCurrY += 2;
                        }
                        break;
                    case Grobal2.DR_DOWN:
                        if (m_nCurrY < m_PEnvir.WHeight - 2 &&
  (m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY + 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) || M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
  (m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY + 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) || M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
    m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX, m_nCurrY + 2, true) > 0)
                        {
                            m_nCurrY += 2;
                        }
                        break;
                    case Grobal2.DR_DOWNLEFT:
                        if (m_nCurrX > 1 &&
  m_nCurrY < m_PEnvir.WHeight - 2 &&
  (m_PEnvir.CanWalkEx(m_nCurrX - 1, m_nCurrY + 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) || M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
  (m_PEnvir.CanWalkEx(m_nCurrX - 2, m_nCurrY + 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) || M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
    m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX - 2, m_nCurrY + 2, true) > 0)
                        {
                            m_nCurrX -= 2;
                            m_nCurrY += 2;
                        }

                        break;
                    case Grobal2.DR_LEFT:
                        if (m_nCurrX > 1 &&
  (m_PEnvir.CanWalkEx(m_nCurrX - 1, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) || M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
  (m_PEnvir.CanWalkEx(m_nCurrX - 2, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) || M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
    m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX - 2, m_nCurrY, true) > 0)
                        {
                            m_nCurrX -= 2;
                        }
                        break;
                    case Grobal2.DR_UPLEFT:
                        if (m_nCurrX > 1 && m_nCurrY > 1 &&
 (m_PEnvir.CanWalkEx(m_nCurrX - 1, m_nCurrY - 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) || M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
  (m_PEnvir.CanWalkEx(m_nCurrX - 2, m_nCurrY - 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) || M2Share.g_Config.boSafeAreaLimited && InSafeZone()) &&
    m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX - 2, m_nCurrY - 2, true) > 0)
                        {
                            m_nCurrX -= 2;
                            m_nCurrY -= 2;
                        }
                        break;
                }
                if (m_nCurrX != nOldX || m_nCurrY != nOldY)
                {
                    if (Walk(Grobal2.RM_RUN))
                    {
                        result = true;
                    }
                    else
                    {
                        m_nCurrX = (short)nOldX;
                        m_nCurrY = (short)nOldY;
                        m_PEnvir.MoveToMovingObject(nOldX, nOldY, this, m_nCurrX, m_nCurrX, true);
                    }
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
            return result;
        }

        /// <summary>
        /// 走向目标
        /// </summary>
        /// <param name="nTargetX"></param>
        /// <param name="nTargetY"></param>
        /// <returns></returns>
        private bool WalkToTargetXY(int nTargetX, int nTargetY)
        {
            int n10;
            int n14;
            int n20;
            int nOldX;
            int nOldY;
            bool result = false;
            if (m_boTransparent && m_boHideMode)
            {
                m_wStatusTimeArr[Grobal2.STATE_TRANSPARENT] = 1;// 隐身,一动就显身
            }
            if (m_wStatusTimeArr[Grobal2.POISON_STONE] != 0 && !M2Share.g_Config.ClientConf.boParalyCanSpell || m_wStatusTimeArr[Grobal2.POISON_DONTMOVE] != 0 || m_wStatusTimeArr[Grobal2.POISON_LOCKSPELL] != 0)
            {
                return result;// 麻痹不能跑动
            }
            if (Math.Abs(nTargetX - m_nCurrX) > 1 || Math.Abs(nTargetY - m_nCurrY) > 1)
            {
                if (HUtil32.GetTickCount() - dwTick3F4 > m_dwWalkIntervalTime)
                {
                    n10 = nTargetX;
                    n14 = nTargetY;
                    byte nDir = Grobal2.DR_DOWN;
                    if (n10 > m_nCurrX)
                    {
                        nDir = Grobal2.DR_RIGHT;
                        if (n14 > m_nCurrY)
                        {
                            nDir = Grobal2.DR_DOWNRIGHT;
                        }
                        if (n14 < m_nCurrY)
                        {
                            nDir = Grobal2.DR_UPRIGHT;
                        }
                    }
                    else
                    {
                        if (n10 < m_nCurrX)
                        {
                            nDir = Grobal2.DR_LEFT;
                            if (n14 > m_nCurrY)
                            {
                                nDir = Grobal2.DR_DOWNLEFT;
                            }
                            if (n14 < m_nCurrY)
                            {
                                nDir = Grobal2.DR_UPLEFT;
                            }
                        }
                        else
                        {
                            if (n14 > m_nCurrY)
                            {
                                nDir = Grobal2.DR_DOWN;
                            }
                            else if (n14 < m_nCurrY)
                            {
                                nDir = Grobal2.DR_UP;
                            }
                        }
                    }
                    nOldX = m_nCurrX;
                    nOldY = m_nCurrY;
                    WalkTo(nDir, false);
                    if (Math.Abs(nTargetX - m_nCurrX) <= 1 && Math.Abs(nTargetY - m_nCurrY) <= 1)
                    {
                        result = true;
                        dwTick3F4 = HUtil32.GetTickCount();
                    }
                    if (!result)
                    {
                        n20 = M2Share.RandomNumber.Random(3);
                        for (var i = Grobal2.DR_UP; i <= Grobal2.DR_UPLEFT; i++)
                        {
                            if (nOldX == m_nCurrX && nOldY == m_nCurrY)
                            {
                                if (n20 != 0)
                                {
                                    nDir++;
                                }
                                else if (nDir > 0)
                                {
                                    nDir -= 1;
                                }
                                else
                                {
                                    nDir = Grobal2.DR_UPLEFT;
                                }
                                if (nDir > Grobal2.DR_UPLEFT)
                                {
                                    nDir = Grobal2.DR_UP;
                                }
                                WalkTo(nDir, false);
                                if (Math.Abs(nTargetX - m_nCurrX) <= 1 && Math.Abs(nTargetY - m_nCurrY) <= 1)
                                {
                                    result = true;
                                    dwTick3F4 = HUtil32.GetTickCount();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 走到目标
        /// </summary>
        /// <param name="nTargetX"></param>
        /// <param name="nTargetY"></param>
        /// <param name="nCode"></param>
        /// <returns></returns>
        private bool GotoTargetXY(short nTargetX, short nTargetY, int nCode)
        {
            bool result = false;
            switch (nCode)
            {
                case 0:// 正常模式
                    if (Math.Abs(m_nCurrX - nTargetX) > 2 || Math.Abs(m_nCurrY - nTargetY) > 2)
                    {
                        if (m_wStatusTimeArr[Grobal2.STATE_LOCKRUN] == 0)
                        {
                            result = RunToTargetXY(nTargetX, nTargetY);
                        }
                        else
                        {
                            result = WalkToTargetXY2(nTargetX, nTargetY);// 转向
                        }
                    }
                    else
                    {
                        result = WalkToTargetXY2(nTargetX, nTargetY);// 转向
                    }
                    break;
                case 1:// 躲避模式
                    if (Math.Abs(m_nCurrX - nTargetX) > 1 || Math.Abs(m_nCurrY - nTargetY) > 1)
                    {
                        if (m_wStatusTimeArr[Grobal2.STATE_LOCKRUN] == 0)
                        {
                            result = RunToTargetXY(nTargetX, nTargetY);
                        }
                        else
                        {
                            result = WalkToTargetXY2(nTargetX, nTargetY);// 转向
                        }
                    }
                    else
                    {
                        result = WalkToTargetXY2(nTargetX, nTargetY);// 转向
                    }
                    break;
            }
            return result;
        }

        private void SearchMagic()
        {
            m_nSelectMagic = SelectMagic();
            if (m_nSelectMagic > 0)
            {
                TUserMagic UserMagic = FindMagic(m_nSelectMagic);
                if (UserMagic != null)
                {
                    m_boIsUseAttackMagic = IsUseAttackMagic();
                }
                else
                {
                    m_boIsUseAttackMagic = false;
                }
            }
            else
            {
                m_boIsUseAttackMagic = false;
            }
        }

        private short SelectMagic()
        {
            short result = 0;
            switch (m_btJob)
            {
                case PlayJob.Warrior:
                    if (AllowUseMagic(26) && HUtil32.GetTickCount() - m_dwLatestFireHitTick > 9000)// 烈火
                    {
                        m_boFireHitSkill = true;
                        result = 26;
                        return result;
                    }
                    if ((m_TargetCret.m_btRaceServer == Grobal2.RC_PLAYOBJECT || m_TargetCret.m_Master != null) && m_TargetCret.m_Abil.Level < m_Abil.Level)
                    {
                        // PK时,使用野蛮冲撞 
                        if (AllowUseMagic(27) && HUtil32.GetTickCount() - m_SkillUseTick[27] > 10000)
                        {
                            // pk时如果对方等级比自己低就每隔一段时间用一次野蛮  
                            m_SkillUseTick[27] = HUtil32.GetTickCount();
                            result = 27;
                            return result;
                        }
                    }
                    else
                    {
                        // 打怪使用 
                        if (AllowUseMagic(27) && HUtil32.GetTickCount() - m_SkillUseTick[27] > 10000 && m_TargetCret.m_Abil.Level < m_Abil.Level && m_WAbil.HP <= Math.Round(m_WAbil.MaxHP * 0.85))
                        {
                            m_SkillUseTick[27] = HUtil32.GetTickCount();
                            result = 27;
                            return result;
                        }
                    }
                    if (m_TargetCret.m_Master != null)
                    {
                        m_ExpHitter = m_TargetCret.m_Master;
                    }
                    if (CheckTargetXYCount1(m_nCurrX, m_nCurrY, 1) > 1)
                    {
                        switch (M2Share.RandomNumber.Random(3))
                        {
                            case 0:// 被怪物包围
                                if (AllowUseMagic(41) && HUtil32.GetTickCount() - m_SkillUseTick[41] > 10000 && m_TargetCret.m_Abil.Level < m_Abil.Level && (m_TargetCret.m_btRaceServer != Grobal2.RC_PLAYOBJECT || M2Share.g_Config.boGroupMbAttackPlayObject) && Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) <= 3 && Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) <= 3)
                                {
                                    m_SkillUseTick[41] = HUtil32.GetTickCount();// 狮子吼
                                    result = 41;
                                    return result;
                                }
                                if (AllowUseMagic(7) && HUtil32.GetTickCount() - m_SkillUseTick[7] > 10000)// 攻杀剑术 
                                {
                                    m_SkillUseTick[7] = HUtil32.GetTickCount();
                                    m_boPowerHit = true;// 开启攻杀
                                    result = 7;
                                    return result;
                                }
                                if (AllowUseMagic(39) && HUtil32.GetTickCount() - m_SkillUseTick[39] > 10000)
                                {
                                    m_SkillUseTick[39] = HUtil32.GetTickCount();// 彻地钉
                                    result = 39;
                                    return result;
                                }
                                if (AllowUseMagic(SpellsDef.SKILL_BANWOL))// 半月弯刀
                                {
                                    if (CheckTargetXYCount2(SpellsDef.SKILL_BANWOL) > 0)
                                    {
                                        if (!m_boUseHalfMoon)
                                        {
                                            HalfMoonOnOff(true);
                                        }
                                        result = SpellsDef.SKILL_BANWOL;
                                        return result;
                                    }
                                }
                                if (AllowUseMagic(40))// 英雄抱月刀法
                                {
                                    if (!m_boCrsHitkill)
                                    {
                                        SkillCrsOnOff(true);
                                    }
                                    result = 40;
                                    return result;
                                }
                                if (AllowUseMagic(12))// 英雄刺杀剑术
                                {
                                    if (!m_boUseThrusting)
                                    {
                                        ThrustingOnOff(true);
                                    }
                                    result = 12;
                                    return result;
                                }
                                break;
                            case 1:
                                if (AllowUseMagic(41) && HUtil32.GetTickCount() - m_SkillUseTick[41] > 10000 && m_TargetCret.m_Abil.Level < m_Abil.Level && (m_TargetCret.m_btRaceServer != Grobal2.RC_PLAYOBJECT || M2Share.g_Config.boGroupMbAttackPlayObject) && Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) <= 3 && Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) <= 3)
                                {
                                    m_SkillUseTick[41] = HUtil32.GetTickCount(); // 狮子吼
                                    result = 41;
                                    return result;
                                }
                                if (AllowUseMagic(7) && HUtil32.GetTickCount() - m_SkillUseTick[7] > 10000)// 攻杀剑术 
                                {
                                    m_SkillUseTick[7] = HUtil32.GetTickCount();
                                    m_boPowerHit = true;
                                    result = 7;
                                    return result;
                                }
                                if (AllowUseMagic(39) && HUtil32.GetTickCount() - m_SkillUseTick[39] > 10000)
                                {
                                    m_SkillUseTick[39] = HUtil32.GetTickCount(); // 英雄彻地钉
                                    result = 39;
                                    return result;
                                }
                                if (AllowUseMagic(40))// 英雄抱月刀法
                                {
                                    if (!m_boCrsHitkill)
                                    {
                                        SkillCrsOnOff(true);
                                    }
                                    result = 40;
                                    return result;
                                }
                                if (AllowUseMagic(SpellsDef.SKILL_BANWOL))// 半月弯刀
                                {
                                    if (CheckTargetXYCount2(SpellsDef.SKILL_BANWOL) > 0)
                                    {
                                        if (!m_boUseHalfMoon)
                                        {
                                            HalfMoonOnOff(true);
                                        }
                                        result = SpellsDef.SKILL_BANWOL;
                                        return result;
                                    }
                                }
                                if (AllowUseMagic(12))// 英雄刺杀剑术
                                {
                                    if (!m_boUseThrusting)
                                    {
                                        ThrustingOnOff(true);
                                    }
                                    result = 12;
                                    return result;
                                }
                                break;
                            case 2:
                                if (AllowUseMagic(41) && HUtil32.GetTickCount() - m_SkillUseTick[41] > 10000 && m_TargetCret.m_Abil.Level < m_Abil.Level && (m_TargetCret.m_btRaceServer != Grobal2.RC_PLAYOBJECT || M2Share.g_Config.boGroupMbAttackPlayObject) && Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) <= 3 && Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) <= 3)
                                {
                                    m_SkillUseTick[41] = HUtil32.GetTickCount();// 狮子吼
                                    result = 41;
                                    return result;
                                }
                                if (AllowUseMagic(39) && HUtil32.GetTickCount() - m_SkillUseTick[39] > 10000)
                                {
                                    m_SkillUseTick[39] = HUtil32.GetTickCount();// 英雄彻地钉
                                    result = 39;
                                    return result;
                                }
                                if (AllowUseMagic(7) && HUtil32.GetTickCount() - m_SkillUseTick[7] > 10000)// 攻杀剑术
                                {
                                    m_SkillUseTick[7] = HUtil32.GetTickCount();
                                    m_boPowerHit = true;//  开启攻杀
                                    result = 7;
                                    return result;
                                }
                                if (AllowUseMagic(40))// 英雄抱月刀法
                                {
                                    if (!m_boCrsHitkill)
                                    {
                                        SkillCrsOnOff(true);
                                    }
                                    result = 40;
                                    return result;
                                }
                                if (AllowUseMagic(SpellsDef.SKILL_BANWOL))// 半月弯刀
                                {
                                    if (CheckTargetXYCount2(SpellsDef.SKILL_BANWOL) > 0)
                                    {
                                        if (!m_boUseHalfMoon)
                                        {
                                            HalfMoonOnOff(true);
                                        }
                                        result = SpellsDef.SKILL_BANWOL;
                                        return result;
                                    }
                                }
                                if (AllowUseMagic(12)) // 英雄刺杀剑术
                                {
                                    if (!m_boUseThrusting)
                                    {
                                        ThrustingOnOff(true);
                                    }
                                    result = 12;
                                    return result;
                                }
                                break;
                        }
                    }
                    else
                    {
                        if ((m_TargetCret.m_btRaceServer == Grobal2.RC_PLAYOBJECT || m_TargetCret.m_Master != null) && CheckTargetXYCount1(m_nCurrX, m_nCurrY, 1) > 1)
                        {
                            // PK  身边超过2个目标才使用
                            if (AllowUseMagic(40) && (HUtil32.GetTickCount() - m_SkillUseTick[40]) > 3000)// 英雄抱月刀法
                            {
                                m_SkillUseTick[40] = HUtil32.GetTickCount();
                                if (!m_boCrsHitkill)
                                {
                                    SkillCrsOnOff(true);
                                }
                                result = 40;
                                return result;
                            }
                            if ((HUtil32.GetTickCount() - m_SkillUseTick[25]) > 1500)
                            {
                                if (AllowUseMagic(SpellsDef.SKILL_BANWOL))
                                {
                                    // 半月弯刀
                                    if (CheckTargetXYCount2(SpellsDef.SKILL_BANWOL) > 0)
                                    {
                                        m_SkillUseTick[25] = HUtil32.GetTickCount();
                                        if (!m_boUseHalfMoon)
                                        {
                                            HalfMoonOnOff(true);
                                        }
                                        result = SpellsDef.SKILL_BANWOL;
                                        return result;
                                    }
                                }
                            }
                        }
                        if (AllowUseMagic(7) && (HUtil32.GetTickCount() - m_SkillUseTick[7]) > 10000) // 少于三个怪用 刺杀剑术
                        {
                            m_SkillUseTick[7] = HUtil32.GetTickCount();
                            m_boPowerHit = true;// 开启攻杀
                            result = 7;
                            return result;
                        }
                        if (HUtil32.GetTickCount() - m_SkillUseTick[12] > 1000)
                        {
                            if (AllowUseMagic(12))// 英雄刺杀剑术
                            {
                                if (!m_boUseThrusting)
                                {
                                    ThrustingOnOff(true);
                                }
                                m_SkillUseTick[12] = HUtil32.GetTickCount();
                                result = 12;
                                return result;
                            }
                        }
                    }
                    // 从高到低使用魔法
                    if (AllowUseMagic(26) && (HUtil32.GetTickCount() - m_dwLatestFireHitTick) > 9000)// 烈火
                    {
                        m_boFireHitSkill = true;
                        result = 26;
                        return result;
                    }
                    if (AllowUseMagic(40) && (HUtil32.GetTickCount() - m_SkillUseTick[40]) > 3000 && CheckTargetXYCount1(m_nCurrX, m_nCurrY, 1) > 1)
                    {
                        // 英雄抱月刀法
                        if (!m_boCrsHitkill)
                        {
                            SkillCrsOnOff(true);
                        }
                        m_SkillUseTick[40] = HUtil32.GetTickCount();
                        result = 40;
                        return result;
                    }
                    if (AllowUseMagic(39) && (HUtil32.GetTickCount() - m_SkillUseTick[39]) > 3000) // 英雄彻地钉
                    {
                        m_SkillUseTick[39] = HUtil32.GetTickCount();
                        result = 39;
                        return result;
                    }
                    if ((HUtil32.GetTickCount() - m_SkillUseTick[25]) > 3000)
                    {
                        if (AllowUseMagic(SpellsDef.SKILL_BANWOL))// 半月弯刀
                        {
                            if (!m_boUseHalfMoon)
                            {
                                HalfMoonOnOff(true);
                            }
                            m_SkillUseTick[25] = HUtil32.GetTickCount();
                            result = SpellsDef.SKILL_BANWOL;
                            return result;
                        }
                    }
                    if ((HUtil32.GetTickCount() - m_SkillUseTick[12]) > 3000)// 英雄刺杀剑术
                    {
                        if (AllowUseMagic(12))
                        {
                            if (!m_boUseThrusting)
                            {
                                ThrustingOnOff(true);
                            }
                            m_SkillUseTick[12] = HUtil32.GetTickCount();
                            result = 12;
                            return result;
                        }
                    }
                    if (AllowUseMagic(7) && (HUtil32.GetTickCount() - m_SkillUseTick[7]) > 3000)// 攻杀剑术
                    {
                        m_boPowerHit = true;
                        m_SkillUseTick[7] = HUtil32.GetTickCount();
                        result = 7;
                        return result;
                    }
                    if ((m_TargetCret.m_btRaceServer == Grobal2.RC_PLAYOBJECT || m_TargetCret.m_Master != null) && m_TargetCret.m_Abil.Level < m_Abil.Level && m_WAbil.HP <= Math.Round(m_WAbil.MaxHP * 0.6))
                    {
                        // PK时,使用野蛮冲撞
                        if (AllowUseMagic(27) && (HUtil32.GetTickCount() - m_SkillUseTick[27]) > 3000)
                        {
                            m_SkillUseTick[27] = HUtil32.GetTickCount();
                            result = 27;
                            return result;
                        }
                    }
                    else
                    {
                        if (AllowUseMagic(27) && m_TargetCret.m_Abil.Level < m_Abil.Level && m_WAbil.HP <= Math.Round(m_WAbil.MaxHP * 0.6) && HUtil32.GetTickCount() - m_SkillUseTick[27] > 3000)
                        {
                            m_SkillUseTick[27] = HUtil32.GetTickCount();
                            result = 27;
                            return result;
                        }
                    }
                    if (AllowUseMagic(41) && HUtil32.GetTickCount() - m_SkillUseTick[41] > 10000 && m_TargetCret.m_Abil.Level < m_Abil.Level && (m_TargetCret.m_btRaceServer != Grobal2.RC_PLAYOBJECT || M2Share.g_Config.boGroupMbAttackPlayObject) && Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) <= 3 && Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) <= 3)
                    {
                        m_SkillUseTick[41] = HUtil32.GetTickCount();// 狮子吼
                        result = 41;
                        return result;
                    }
                    break;
                case PlayJob.Wizard: // 法师
                    if (m_wStatusTimeArr[Grobal2.STATE_BUBBLEDEFENCEUP] == 0 && !m_boAbilMagBubbleDefence) // 使用 魔法盾
                    {
                        if (AllowUseMagic(66)) // 4级魔法盾
                        {
                            result = 66;
                            return result;
                        }
                        if (AllowUseMagic(31))
                        {
                            result = 31;
                            return result;
                        }
                    }
                    if ((m_TargetCret.m_btRaceServer == Grobal2.RC_PLAYOBJECT || m_TargetCret.m_Master != null) && CheckTargetXYCount3(m_nCurrX, m_nCurrY, 1, 0) > 0 && m_TargetCret.m_WAbil.Level < m_WAbil.Level)
                    {
                        // PK时,旁边有人贴身,使用抗拒火环
                        if (AllowUseMagic(8) && HUtil32.GetTickCount() - m_SkillUseTick[8] > 3000)
                        {
                            m_SkillUseTick[8] = HUtil32.GetTickCount();
                            result = 8;
                            return result;
                        }
                    }
                    else
                    {
                        // 打怪,怪级低于自己,并且有怪包围自己就用 抗拒火环
                        if (AllowUseMagic(8) && HUtil32.GetTickCount() - m_SkillUseTick[8] > 3000 && CheckTargetXYCount3(m_nCurrX, m_nCurrY, 1, 0) > 0 && m_TargetCret.m_WAbil.Level < m_WAbil.Level)
                        {
                            m_SkillUseTick[8] = HUtil32.GetTickCount();
                            result = 8;
                            return result;
                        }
                    }
                    if (AllowUseMagic(45) && HUtil32.GetTickCount() - m_SkillUseTick[45] > 3000)
                    {
                        m_SkillUseTick[45] = HUtil32.GetTickCount();
                        result = 45;// 英雄灭天火
                        return result;
                    }
                    if (HUtil32.GetTickCount() - m_SkillUseTick[10] > 5000 && m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, Direction, 5, ref m_nTargetX, ref m_nTargetY))
                    {
                        if ((m_TargetCret.m_btRaceServer == Grobal2.RC_PLAYOBJECT || m_TargetCret.m_Master != null) && GetDirBaseObjectsCount(Direction, 5) > 0 && (Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 4 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 0 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 0 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) <= 4 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 2 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 2 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 3 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 3 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 4 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 4))
                        {
                            if (AllowUseMagic(10))
                            {
                                m_SkillUseTick[10] = HUtil32.GetTickCount();
                                result = 10;// 英雄疾光电影 
                                return result;
                            }
                            else if (AllowUseMagic(9))
                            {
                                m_SkillUseTick[10] = HUtil32.GetTickCount();
                                result = 9;// 地狱火
                                return result;
                            }
                        }
                        else if (GetDirBaseObjectsCount(Direction, 5) > 1 && (Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 4 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 0 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 0 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) <= 4 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 2 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 2 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 3 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 3 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 4 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 4))
                        {
                            if (AllowUseMagic(10))
                            {
                                m_SkillUseTick[10] = HUtil32.GetTickCount();
                                result = 10;// 英雄疾光电影 
                                return result;
                            }
                            else if (AllowUseMagic(9))
                            {
                                m_SkillUseTick[10] = HUtil32.GetTickCount();
                                result = 9;// 地狱火
                                return result;
                            }
                        }
                    }
                    if (AllowUseMagic(32) && HUtil32.GetTickCount() - m_SkillUseTick[32] > 10000 && m_TargetCret.m_Abil.Level < M2Share.g_Config.nMagTurnUndeadLevel && m_TargetCret.m_btLifeAttrib == Grobal2.LA_UNDEAD && m_TargetCret.m_WAbil.Level < m_WAbil.Level - 1)
                    {
                        // 目标为不死系
                        m_SkillUseTick[32] = HUtil32.GetTickCount();
                        result = 32;// 圣言术
                        return result;
                    }
                    if (CheckTargetXYCount(m_nCurrX, m_nCurrY, 2) > 1)// 被怪物包围
                    {
                        if (AllowUseMagic(22) && (HUtil32.GetTickCount() - m_SkillUseTick[22]) > 10000)
                        {
                            if (m_TargetCret.m_btRaceServer != 101 && m_TargetCret.m_btRaceServer != 102 && m_TargetCret.m_btRaceServer != 104) // 除祖玛怪,才放火墙
                            {
                                m_SkillUseTick[22] = HUtil32.GetTickCount();
                                result = 22;// 火墙
                                return result;
                            }
                        }
                        // 地狱雷光,只对祖玛(101,102,104)，沃玛(91,92,97)，野猪(81)系列的
                        // 遇到祖玛的怪应该多用地狱雷光，夹杂雷电术，少用冰咆哮
                        if (new ArrayList(new byte[] { 91, 92, 97, 101, 102, 104 }).Contains(m_TargetCret.m_btRaceServer))
                        {
                            // 1000 * 4
                            if (AllowUseMagic(24) && (HUtil32.GetTickCount() - m_SkillUseTick[24]) > 4000 && CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2)
                            {
                                m_SkillUseTick[24] = HUtil32.GetTickCount();
                                result = 24;// 地狱雷光
                                return result;
                            }
                            else if (AllowUseMagic(91))
                            {
                                result = 91;// 四级雷电术
                                return result;
                            }
                            else if (AllowUseMagic(11))
                            {
                                result = 11;// 英雄雷电术
                                return result;
                            }
                            else if (AllowUseMagic(33) && CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 2) > 2)
                            {
                                result = 33;// 英雄冰咆哮
                                return result;
                            }
                            else if (HUtil32.GetTickCount() - m_SkillUseTick[58] > 1500 && CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2)
                            {
                                if (AllowUseMagic(92))
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 92;// 四级流星火雨
                                    return result;
                                }
                                if (AllowUseMagic(58))
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 58;// 流星火雨
                                    return result;
                                }
                            }
                        }
                        switch (M2Share.RandomNumber.Random(4))// 随机选择魔法
                        {
                            case 0: // 火球术,大火球,雷电术,爆裂火焰,英雄冰咆哮,流星火雨 从高到低选择
                                if (AllowUseMagic(92) && (HUtil32.GetTickCount() - m_SkillUseTick[58]) > 1500 && CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2)
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 92;// 四级流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(58) && (HUtil32.GetTickCount() - m_SkillUseTick[58]) > 1500 && CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2)
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 58;// 流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(33) && CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 1)
                                {
                                    result = 33;// 英雄冰咆哮
                                    return result;
                                }
                                else if (AllowUseMagic(23) && CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 1)
                                {
                                    result = 23;// 爆裂火焰
                                    return result;
                                }
                                else if (AllowUseMagic(91))
                                {
                                    result = 91;// 四级雷电术
                                    return result;
                                }
                                else if (AllowUseMagic(11))
                                {
                                    result = 11;// 英雄雷电术
                                    return result;
                                }
                                else if (AllowUseMagic(5))
                                {
                                    result = 5;// 大火球
                                    return result;
                                }
                                else if (AllowUseMagic(1))
                                {
                                    result = 1;// 火球术
                                    return result;
                                }
                                if (AllowUseMagic(37))
                                {
                                    result = 37;// 英雄群体雷电
                                    return result;
                                }
                                if (AllowUseMagic(47))
                                {
                                    result = 47;// 火龙焰
                                    return result;
                                }
                                if (AllowUseMagic(44))
                                {
                                    result = 44;// 寒冰掌
                                    return result;
                                }
                                break;
                            case 1:
                                if (AllowUseMagic(37))
                                {
                                    result = 37;
                                    return result;
                                }
                                if (AllowUseMagic(47))
                                {
                                    result = 47;
                                    return result;
                                }
                                if (AllowUseMagic(44))
                                {
                                    result = 44;// 寒冰掌
                                    return result;
                                }
                                if (AllowUseMagic(92) && (HUtil32.GetTickCount() - m_SkillUseTick[58]) > 1500 && CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2)
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 92;// 四级流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(58) && (HUtil32.GetTickCount() - m_SkillUseTick[58]) > 1500 && CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2)
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 58;// 流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(33) && CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 1)
                                {
                                    // 火球术,大火球,地狱火,爆裂火焰,冰咆哮  从高到低选择
                                    result = 33;// 冰咆哮
                                    return result;
                                }
                                else if (AllowUseMagic(23) && CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 1)
                                {
                                    result = 23;// 爆裂火焰
                                    return result;
                                }
                                else if (AllowUseMagic(91))
                                {
                                    result = 91;// 四级雷电术
                                    return result;
                                }
                                else if (AllowUseMagic(11))
                                {
                                    result = 11;// 英雄雷电术
                                    return result;
                                }
                                else if (AllowUseMagic(5))
                                {
                                    result = 5;// 大火球
                                    return result;
                                }
                                else if (AllowUseMagic(1))
                                {
                                    result = 1;// 火球术
                                    return result;
                                }
                                break;
                            case 2:
                                if (AllowUseMagic(47))
                                {
                                    result = 47;
                                    return result;
                                }
                                if (AllowUseMagic(44))
                                {
                                    result = 44;// 寒冰掌
                                    return result;
                                }
                                if (AllowUseMagic(92) && (HUtil32.GetTickCount() - m_SkillUseTick[58]) > 1500 && CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2)
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 92;// 四级流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(58) && (HUtil32.GetTickCount() - m_SkillUseTick[58]) > 1500 && CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2)
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 58;// 流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(33) && CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 1)
                                {
                                    // 火球术,大火球,地狱火,爆裂火焰 从高到低选择
                                    result = 33;
                                    return result;
                                }
                                else if (AllowUseMagic(23) && CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 1)
                                {
                                    result = 23;// 爆裂火焰
                                    return result;
                                }
                                else if (AllowUseMagic(91))
                                {
                                    result = 91;// 四级雷电术
                                    return result;
                                }
                                else if (AllowUseMagic(11))
                                {
                                    result = 11;// 英雄雷电术
                                    return result;
                                }
                                else if (AllowUseMagic(5))
                                {
                                    result = 5;// 大火球
                                    return result;
                                }
                                else if (AllowUseMagic(1))
                                {
                                    result = 1;// 火球术
                                    return result;
                                }
                                if (AllowUseMagic(37))
                                {
                                    result = 37;
                                    return result;
                                }
                                break;
                            case 3:
                                if (AllowUseMagic(44))
                                {
                                    result = 44; // 寒冰掌
                                    return result;
                                }
                                if (AllowUseMagic(92) && (HUtil32.GetTickCount() - m_SkillUseTick[58] > 1500) && CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2)
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 92; // 四级流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(58) && (HUtil32.GetTickCount() - m_SkillUseTick[58]) > 1500 && CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2)
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 58; // 流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(33) && CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 1)// 火球术,大火球,地狱火,爆裂火焰 从高到低选择
                                {
                                    result = 33;
                                    return result;
                                }
                                else if (AllowUseMagic(23) && CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 1)
                                {
                                    result = 23;// 爆裂火焰
                                    return result;
                                }
                                else if (AllowUseMagic(91))
                                {
                                    result = 91;// 四级雷电术
                                    return result;
                                }
                                else if (AllowUseMagic(11))
                                {
                                    result = 11;// 英雄雷电术
                                    return result;
                                }
                                else if (AllowUseMagic(5))
                                {
                                    result = 5;// 大火球
                                    return result;
                                }
                                else if (AllowUseMagic(1))
                                {
                                    result = 1;// 火球术
                                    return result;
                                }
                                if (AllowUseMagic(37))
                                {
                                    result = 37;
                                    return result;
                                }
                                if (AllowUseMagic(47))
                                {
                                    result = 47;
                                    return result;
                                }
                                break;
                        }
                    }
                    else
                    {
                        // 只有一个怪时所用的魔法
                        if (AllowUseMagic(22) && HUtil32.GetTickCount() - m_SkillUseTick[22] > 10000)
                        {
                            if (m_TargetCret.m_btRaceServer != 101 && m_TargetCret.m_btRaceServer != 102 && m_TargetCret.m_btRaceServer != 104)// 除祖玛怪,才放火墙
                            {
                                m_SkillUseTick[22] = HUtil32.GetTickCount();
                                result = 22;
                                return result;
                            }
                        }
                        switch (M2Share.RandomNumber.Random(4))// 随机选择魔法
                        {
                            case 0:
                                if (AllowUseMagic(91))
                                {
                                    result = 91;// 四级雷电术
                                    return result;
                                }
                                else if (AllowUseMagic(11))
                                {
                                    result = 11;// 雷电术
                                    return result;
                                }
                                else if (AllowUseMagic(33))
                                {
                                    result = 33;
                                    return result;
                                }
                                else if (AllowUseMagic(23)) // 火球术,大火球,地狱火,爆裂火焰 从高到低选择
                                {
                                    result = 23;// 爆裂火焰
                                    return result;
                                }
                                else if (AllowUseMagic(5))
                                {
                                    result = 5;// 大火球
                                    return result;
                                }
                                else if (AllowUseMagic(1))
                                {
                                    result = 1;// 火球术
                                    return result;
                                }
                                if (AllowUseMagic(37))
                                {
                                    result = 37;
                                    return result;
                                }
                                if (AllowUseMagic(47))
                                {
                                    result = 47;
                                    return result;
                                }
                                if (AllowUseMagic(44))
                                {
                                    result = 44;
                                    return result;
                                }
                                break;
                            case 1:
                                if (AllowUseMagic(37))
                                {
                                    result = 37;
                                    return result;
                                }
                                if (AllowUseMagic(47))
                                {
                                    result = 47;
                                    return result;
                                }
                                if (AllowUseMagic(44))
                                {
                                    result = 44;
                                    return result;
                                }
                                if (AllowUseMagic(91))
                                {
                                    result = 91;// 四级雷电术
                                    return result;
                                }
                                else if (AllowUseMagic(11))
                                {
                                    result = 11;// 雷电术
                                    return result;
                                }
                                else if (AllowUseMagic(33))
                                {
                                    result = 33;
                                    return result;
                                }
                                else if (AllowUseMagic(23))
                                {
                                    result = 23;// 爆裂火焰
                                    return result;
                                }
                                else if (AllowUseMagic(5))
                                {
                                    result = 5;// 大火球
                                    return result;
                                }
                                else if (AllowUseMagic(1))
                                {
                                    result = 1;// 火球术
                                    return result;
                                }
                                break;
                            case 2:
                                if (AllowUseMagic(47))
                                {
                                    result = 47;
                                    return result;
                                }
                                if (AllowUseMagic(44))
                                {
                                    result = 44;
                                    return result;
                                }
                                if (AllowUseMagic(91))
                                {
                                    result = 91;// 四级雷电术
                                    return result;
                                }
                                else if (AllowUseMagic(11))
                                {
                                    result = 11;// 雷电术
                                    return result;
                                }
                                else if (AllowUseMagic(33))
                                {
                                    result = 33;
                                    return result;
                                }
                                else if (AllowUseMagic(23))
                                {
                                    result = 23;// 爆裂火焰
                                    return result;
                                }
                                else if (AllowUseMagic(5))
                                {
                                    result = 5;// 大火球
                                    return result;
                                }
                                else if (AllowUseMagic(1))
                                {
                                    result = 1;// 火球术
                                    return result;
                                }
                                if (AllowUseMagic(37))
                                {
                                    result = 37;
                                    return result;
                                }
                                break;
                            case 3:
                                if (AllowUseMagic(44))
                                {
                                    result = 44;
                                    return result;
                                }
                                if (AllowUseMagic(91))
                                {
                                    result = 91;// 四级雷电术
                                    return result;
                                }
                                else if (AllowUseMagic(11))
                                {
                                    result = 11;// 雷电术
                                    return result;
                                }
                                else if (AllowUseMagic(33))
                                {
                                    result = 33;
                                    return result;
                                }
                                else if (AllowUseMagic(23))
                                {
                                    result = 23;// 爆裂火焰
                                    return result;
                                }
                                else if (AllowUseMagic(5))
                                {
                                    result = 5;// 大火球
                                    return result;
                                }
                                else if (AllowUseMagic(1))
                                {
                                    result = 1;// 火球术
                                    return result;
                                }
                                if (AllowUseMagic(37))
                                {
                                    result = 37;
                                    return result;
                                }
                                if (AllowUseMagic(47))
                                {
                                    result = 47;
                                    return result;
                                }
                                break;
                        }
                    }
                    // 从高到低使用魔法 
                    if ((HUtil32.GetTickCount() - m_SkillUseTick[58]) > 1500)
                    {
                        if (AllowUseMagic(92))// 四级流星火雨
                        {
                            m_SkillUseTick[58] = HUtil32.GetTickCount();
                            result = 92;
                            return result;
                        }
                        if (AllowUseMagic(58)) // 流星火雨
                        {
                            m_SkillUseTick[58] = HUtil32.GetTickCount();
                            result = 58;
                            return result;
                        }
                    }
                    if (AllowUseMagic(47))
                    {
                        // 火龙焰
                        result = 47;
                        return result;
                    }
                    if (AllowUseMagic(45))
                    {
                        // 英雄灭天火
                        result = 45;
                        return result;
                    }
                    if (AllowUseMagic(44))
                    {
                        result = 44;
                        return result;
                    }
                    if (AllowUseMagic(37))
                    {
                        // 英雄群体雷电
                        result = 37;
                        return result;
                    }
                    if (AllowUseMagic(33))
                    {
                        // 英雄冰咆哮
                        result = 33;
                        return result;
                    }
                    if (AllowUseMagic(32) && m_TargetCret.m_Abil.Level < M2Share.g_Config.nMagTurnUndeadLevel && m_TargetCret.m_btLifeAttrib == Grobal2.LA_UNDEAD && m_TargetCret.m_WAbil.Level < m_WAbil.Level - 1)
                    {
                        // 目标为不死系
                        result = 32;// 圣言术
                        return result;
                    }
                    if (AllowUseMagic(24) && CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2)
                    {
                        result = 24;// 地狱雷光
                        return result;
                    }
                    if (AllowUseMagic(23))
                    {
                        result = 23;// 爆裂火焰
                        return result;
                    }
                    if (AllowUseMagic(91))
                    {
                        result = 91; // 四级雷电术
                        return result;
                    }
                    if (AllowUseMagic(11))
                    {
                        result = 11;// 英雄雷电术
                        return result;
                    }
                    if (AllowUseMagic(10) && m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, Direction, 5, ref m_nTargetX, ref m_nTargetY) && (Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 4 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 0 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 0 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) <= 4 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 2 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 2 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 3 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 3 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 4 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 4))
                    {
                        result = 10; // 英雄疾光电影
                        return result;
                    }
                    if (AllowUseMagic(9) && m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, Direction, 5, ref m_nTargetX, ref m_nTargetY) && (Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 4 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 0 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 0 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) <= 4 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 2 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 2 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 3 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 3 || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 4 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 4))
                    {
                        result = 9; // 地狱火
                        return result;
                    }
                    if (AllowUseMagic(5))
                    {
                        result = 5; // 大火球
                        return result;
                    }
                    if (AllowUseMagic(1))
                    {
                        result = 1; // 火球术
                        return result;
                    }
                    if (AllowUseMagic(22))
                    {
                        if (m_TargetCret.m_btRaceServer != 101 && m_TargetCret.m_btRaceServer != 102 && m_TargetCret.m_btRaceServer != 104)// 除祖玛怪,才放火墙
                        {
                            result = 22;// 火墙
                            return result;
                        }
                    }
                    break;
                case PlayJob.Taoist:// 道士
                    if (m_SlaveList.Count == 0 && CheckHeroAmulet(1, 5) && HUtil32.GetTickCount() - m_SkillUseTick[17] > 3000 && (AllowUseMagic(72) || AllowUseMagic(30) || AllowUseMagic(17)) && m_WAbil.MP > 20)
                    {
                        m_SkillUseTick[17] = HUtil32.GetTickCount(); // 默认,从高到低
                        if (AllowUseMagic(104)) // 召唤火灵
                        {
                            result = 104;
                        }
                        else if (AllowUseMagic(72)) // 召唤月灵
                        {
                            result = 72;
                        }
                        else if (AllowUseMagic(SpellsDef.SKILL_SINSU))// 召唤神兽
                        {
                            result = SpellsDef.SKILL_SINSU;
                        }
                        else if (AllowUseMagic(SpellsDef.SKILL_SKELLETON)) // 召唤骷髅
                        {
                            result = SpellsDef.SKILL_SKELLETON;
                        }
                        return result;
                    }
                    if (m_wStatusTimeArr[Grobal2.STATE_BUBBLEDEFENCEUP] == 0 && !m_boAbilMagBubbleDefence)
                    {
                        if (AllowUseMagic(73)) // 道力盾
                        {
                            result = 73;
                            return result;
                        }
                    }
                    if ((m_TargetCret.m_btRaceServer == Grobal2.RC_PLAYOBJECT || m_TargetCret.m_Master != null) && CheckTargetXYCount3(m_nCurrX, m_nCurrY, 1, 0) > 0 && m_TargetCret.m_WAbil.Level <= m_WAbil.Level)
                    {
                        // PK时,旁边有人贴身,使用气功波
                        if (AllowUseMagic(48) && HUtil32.GetTickCount() - m_SkillUseTick[48] > 3000)
                        {
                            m_SkillUseTick[48] = HUtil32.GetTickCount();
                            result = 48;
                            return result;
                        }
                    }
                    else
                    {
                        // 打怪,怪级低于自己,并且有怪包围自己就用 气功波
                        if (AllowUseMagic(48) && HUtil32.GetTickCount() - m_SkillUseTick[48] > 5000 && CheckTargetXYCount3(m_nCurrX, m_nCurrY, 1, 0) > 0 && m_TargetCret.m_WAbil.Level <= m_WAbil.Level)
                        {
                            m_SkillUseTick[48] = HUtil32.GetTickCount();
                            result = 48;
                            return result;
                        }
                    }
                    // 绿毒
                    if (m_TargetCret.m_wStatusTimeArr[Grobal2.POISON_DECHEALTH] == 0 && GetUserItemList(2, 1) >= 0 && (M2Share.g_Config.btHeroSkillMode || !M2Share.g_Config.btHeroSkillMode && m_TargetCret.m_Abil.HP >= 700
                        || m_TargetCret.m_btRaceServer == Grobal2.RC_PLAYOBJECT) && (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) < 7 || Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) < 7)
                        && !new ArrayList(new byte[] { 55, 79, 109, 110, 111, 128, 143, 145, 147, 151, 153, 156 }).Contains(m_TargetCret.m_btRaceServer))
                    {
                        // 对于血量超过800的怪用 不毒城墙
                        n_AmuletIndx = 0;
                        switch (M2Share.RandomNumber.Random(2))
                        {
                            case 0:
                                if (AllowUseMagic(38) && HUtil32.GetTickCount() - m_SkillUseTick[38] > 1000)
                                {
                                    if (m_PEnvir != null)// 判断地图是否禁用
                                    {
                                        if (m_PEnvir.AllowMagics(SpellsDef.SKILL_GROUPAMYOUNSUL, 1))
                                        {
                                            m_SkillUseTick[38] = HUtil32.GetTickCount();
                                            result = SpellsDef.SKILL_GROUPAMYOUNSUL;// 英雄群体施毒
                                            return result;
                                        }
                                    }
                                }
                                else if ((HUtil32.GetTickCount() - m_SkillUseTick[6]) > 1000)
                                {
                                    if (AllowUseMagic(SpellsDef.SKILL_AMYOUNSUL))
                                    {
                                        if (m_PEnvir != null)
                                        {
                                            if (m_PEnvir.AllowMagics(SpellsDef.SKILL_AMYOUNSUL, 1))// 判断地图是否禁用
                                            {
                                                m_SkillUseTick[6] = HUtil32.GetTickCount();
                                                result = SpellsDef.SKILL_AMYOUNSUL;// 英雄施毒术
                                                return result;
                                            }
                                        }
                                    }
                                }
                                break;
                            case 1:
                                if ((HUtil32.GetTickCount() - m_SkillUseTick[6]) > 1000)
                                {
                                    if (AllowUseMagic(SpellsDef.SKILL_AMYOUNSUL))
                                    {
                                        if (m_PEnvir != null)
                                        {
                                            if (m_PEnvir.AllowMagics(SpellsDef.SKILL_AMYOUNSUL, 1))// 判断地图是否禁用
                                            {
                                                m_SkillUseTick[6] = HUtil32.GetTickCount();
                                                result = SpellsDef.SKILL_AMYOUNSUL; // 英雄施毒术
                                                return result;
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    if (m_TargetCret.m_wStatusTimeArr[Grobal2.POISON_DAMAGEARMOR] == 0 && GetUserItemList(2, 2) >= 0 && (M2Share.g_Config.btHeroSkillMode || !M2Share.g_Config.btHeroSkillMode && m_TargetCret.m_Abil.HP >= 700
                        || m_TargetCret.m_btRaceServer == Grobal2.RC_PLAYOBJECT) && (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) < 7 || Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) < 7)
                        && !new ArrayList(new byte[] { 55, 79, 109, 110, 111, 128, 143, 145, 147, 151, 153, 156 }).Contains(m_TargetCret.m_btRaceServer))
                    {
                        // 对于血量超过100的怪用 不毒城墙
                        n_AmuletIndx = 0;
                        switch (M2Share.RandomNumber.Random(2))
                        {
                            case 0:
                                if (AllowUseMagic(38) && (HUtil32.GetTickCount() - m_SkillUseTick[38]) > 1000)
                                {
                                    if (m_PEnvir != null)
                                    {
                                        // 判断地图是否禁用
                                        if (m_PEnvir.AllowMagics(SpellsDef.SKILL_GROUPAMYOUNSUL, 1))
                                        {
                                            m_SkillUseTick[38] = HUtil32.GetTickCount();
                                            result = SpellsDef.SKILL_GROUPAMYOUNSUL; // 英雄群体施毒
                                            return result;
                                        }
                                    }
                                }
                                else if ((HUtil32.GetTickCount() - m_SkillUseTick[6]) > 1000)
                                {
                                    if (AllowUseMagic(SpellsDef.SKILL_AMYOUNSUL))
                                    {
                                        if (m_PEnvir != null)
                                        {
                                            // 判断地图是否禁用
                                            if (m_PEnvir.AllowMagics(SpellsDef.SKILL_AMYOUNSUL, 1))
                                            {
                                                m_SkillUseTick[6] = HUtil32.GetTickCount();
                                                result = SpellsDef.SKILL_AMYOUNSUL; // 英雄施毒术
                                                return result;
                                            }
                                        }
                                    }
                                }
                                break;
                            case 1:
                                if ((HUtil32.GetTickCount() - m_SkillUseTick[6]) > 1000)
                                {
                                    if (AllowUseMagic(SpellsDef.SKILL_AMYOUNSUL))
                                    {
                                        if (m_PEnvir != null)
                                        {
                                            // 判断地图是否禁用
                                            if (m_PEnvir.AllowMagics(SpellsDef.SKILL_AMYOUNSUL, 1))
                                            {
                                                m_SkillUseTick[6] = HUtil32.GetTickCount();
                                                result = SpellsDef.SKILL_AMYOUNSUL; // 英雄施毒术
                                                return result;
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    if (AllowUseMagic(51) && (HUtil32.GetTickCount() - m_SkillUseTick[51]) > 5000)// 英雄飓风破 
                    {
                        m_SkillUseTick[51] = HUtil32.GetTickCount();
                        result = 51;
                        return result;
                    }
                    if (CheckHeroAmulet(1, 1))
                    {
                        switch (M2Share.RandomNumber.Random(3))
                        {
                            case 0: // 使用符的魔法
                                if (AllowUseMagic(94))
                                {
                                    result = 94; // 英雄四级噬血术
                                    return result;
                                }
                                if (AllowUseMagic(59))
                                {
                                    result = 59; // 英雄噬血术
                                    return result;
                                }
                                if (AllowUseMagic(13) && (HUtil32.GetTickCount() - m_SkillUseTick[13]) > 3000)
                                {
                                    result = 13;// 英雄灵魂火符
                                    m_SkillUseTick[13] = HUtil32.GetTickCount();
                                    return result;
                                }
                                if (AllowUseMagic(52) && m_TargetCret.m_wStatusArrValue[(byte)m_TargetCret.m_btJob + 6] == 0) // 诅咒术
                                {
                                    result = 52;// 英雄诅咒术
                                    return result;
                                }
                                break;
                            case 1:
                                if (AllowUseMagic(52) && m_TargetCret.m_wStatusArrValue[(byte)m_TargetCret.m_btJob + 6] == 0) // 诅咒术
                                {
                                    result = 52;
                                    return result;
                                }
                                if (AllowUseMagic(94))
                                {
                                    result = 94;// 英雄四级噬血术
                                    return result;
                                }
                                if (AllowUseMagic(59))
                                {
                                    result = 59;// 英雄噬血术
                                    return result;
                                }
                                if (AllowUseMagic(13) && (HUtil32.GetTickCount() - m_SkillUseTick[13]) > 3000)
                                {
                                    result = 13;// 英雄灵魂火符
                                    m_SkillUseTick[13] = HUtil32.GetTickCount();
                                    return result;
                                }
                                break;
                            case 2:
                                if (AllowUseMagic(13) && (HUtil32.GetTickCount() - m_SkillUseTick[13]) > 3000)
                                {
                                    result = 13;// 英雄灵魂火符
                                    m_SkillUseTick[13] = HUtil32.GetTickCount();
                                    return result;
                                }
                                if (AllowUseMagic(94))
                                {
                                    result = 94;// 英雄四级噬血术
                                    return result;
                                }
                                if (AllowUseMagic(59))
                                {
                                    result = 59;// 英雄噬血术
                                    return result;
                                }
                                if (AllowUseMagic(52) && m_TargetCret.m_wStatusArrValue[(byte)m_TargetCret.m_btJob + 6] == 0)// 诅咒术
                                {
                                    result = 52;
                                    return result;
                                }
                                break;
                        }
                        // 技能从高到低选择 
                        if (AllowUseMagic(94))
                        {
                            result = 94;// 英雄四级噬血术
                            return result;
                        }
                        if (AllowUseMagic(59))// 英雄噬血术
                        {
                            result = 59;
                            return result;
                        }
                        if (AllowUseMagic(54)) // 英雄骷髅咒
                        {
                            result = 54;
                            return result;
                        }
                        if (AllowUseMagic(53))// 英雄血咒
                        {
                            result = 53;
                            return result;
                        }
                        if (AllowUseMagic(51))// 英雄飓风破
                        {
                            result = 51;
                            return result;
                        }
                        if (AllowUseMagic(13))// 英雄灵魂火符
                        {
                            result = 13;
                            return result;
                        }
                        if (AllowUseMagic(52) && m_TargetCret.m_wStatusArrValue[(byte)m_TargetCret.m_btJob + 6] == 0)// 诅咒术
                        {
                            result = 52;
                            return result;
                        }
                    }
                    break;
            }
            return result;
        }

        // 战士判断使用
        private int CheckTargetXYCount1(int nX, int nY, int nRange)
        {
            TBaseObject BaseObject;
            int result = 0;
            int n10 = nRange;
            if (m_VisibleActors.Count > 0)
            {
                for (var i = 0; i < m_VisibleActors.Count; i++)
                {
                    BaseObject = m_VisibleActors[i].BaseObject;
                    if (BaseObject != null)
                    {
                        if (!BaseObject.m_boDeath)
                        {
                            if (IsProperTarget(BaseObject) && (!BaseObject.m_boHideMode || m_boCoolEye))
                            {
                                if (Math.Abs(nX - BaseObject.m_nCurrX) <= n10 && Math.Abs(nY - BaseObject.m_nCurrY) <= n10)
                                {
                                    result++;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        // 半月弯刀判断目标函数
        private int CheckTargetXYCount2(short nMode)
        {
            int result = 0;
            int nC = 0;
            int n10 = 0;
            short nX = 0;
            short nY = 0;
            TBaseObject BaseObject;
            if (m_VisibleActors.Count > 0)
            {
                for (var i = 0; i < m_VisibleActors.Count; i++)
                {
                    switch (nMode)
                    {
                        case SpellsDef.SKILL_BANWOL:
                            n10 = (Direction + M2Share.g_Config.WideAttack[nC]) % 8;
                            break;
                    }
                    if (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, n10, 1, ref nX, ref nY))
                    {
                        BaseObject = (TBaseObject)m_PEnvir.GetMovingObject(nX, nY, true);
                        if (BaseObject != null)
                        {
                            if (!BaseObject.m_boDeath)
                            {
                                if (IsProperTarget(BaseObject) && (!BaseObject.m_boHideMode || m_boCoolEye))
                                {
                                    result++;
                                }
                            }
                        }
                    }
                    nC++;
                    switch (nMode)
                    {
                        case SpellsDef.SKILL_BANWOL:
                            if (nC >= 3)
                            {
                                break;
                            }
                            break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 气功波，抗拒火环使用
        /// </summary>
        /// <param name="nX"></param>
        /// <param name="nY"></param>
        /// <param name="nRange"></param>
        /// <param name="nCount"></param>
        /// <returns></returns>
        private int CheckTargetXYCount3(int nX, int nY, int nRange, int nCount)
        {
            TBaseObject BaseObject;
            int result = 0;
            int n10 = nRange;
            if (m_VisibleActors.Count > 0)
            {
                for (var i = 0; i < m_VisibleActors.Count; i++)
                {
                    BaseObject = m_VisibleActors[i].BaseObject;
                    if (BaseObject != null)
                    {
                        if (!BaseObject.m_boDeath)
                        {
                            if (IsProperTarget(BaseObject) && (!BaseObject.m_boHideMode || m_boCoolEye))
                            {
                                if (Math.Abs(nX - BaseObject.m_nCurrX) <= n10 && Math.Abs(nY - BaseObject.m_nCurrY) <= n10)
                                {
                                    result++;
                                    if (result > nCount)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        // 参数 nType 为指定类型 1 为护身符 2 为毒药    nCount 为持久,即数量
        private bool CheckHeroAmulet(int nType, int nCount)
        {
            bool result = false;
            TUserItem UserItem;
            StdItem AmuletStdItem;
            try
            {
                result = false;
                if (m_UseItems[Grobal2.U_ARMRINGL].wIndex > 0)
                {
                    AmuletStdItem = M2Share.UserEngine.GetStdItem(m_UseItems[Grobal2.U_ARMRINGL].wIndex);
                    if (AmuletStdItem != null)
                    {
                        if (AmuletStdItem.StdMode == 25)
                        {
                            switch (nType)
                            {
                                case 1:
                                    if (AmuletStdItem.Shape == 5 && Math.Round(Convert.ToDouble(m_UseItems[Grobal2.U_ARMRINGL].Dura / 100)) >= nCount)
                                    {
                                        result = true;
                                        return result;
                                    }
                                    break;
                                case 2:
                                    if (AmuletStdItem.Shape <= 2 && Math.Round(Convert.ToDouble(m_UseItems[Grobal2.U_ARMRINGL].Dura / 100)) >= nCount)
                                    {
                                        result = true;
                                        return result;
                                    }
                                    break;
                            }
                        }
                    }
                }
                if (m_UseItems[Grobal2.U_BUJUK] != null && m_UseItems[Grobal2.U_BUJUK].wIndex > 0)
                {
                    AmuletStdItem = M2Share.UserEngine.GetStdItem(m_UseItems[Grobal2.U_BUJUK].wIndex);
                    if (AmuletStdItem != null)
                    {
                        if (AmuletStdItem.StdMode == 25)
                        {
                            switch (nType)
                            {
                                case 1: // 符
                                    if (AmuletStdItem.Shape == 5 && Math.Round(Convert.ToDouble(m_UseItems[Grobal2.U_BUJUK].Dura / 100)) >= nCount)
                                    {
                                        result = true;
                                        return result;
                                    }
                                    break;
                                case 2: // 毒
                                    if (AmuletStdItem.Shape <= 2 && Math.Round(Convert.ToDouble(m_UseItems[Grobal2.U_BUJUK].Dura / 100)) >= nCount)
                                    {
                                        result = true;
                                        return result;
                                    }
                                    break;
                            }
                        }
                    }
                }
                // 检测人物包裹是否存在毒,护身符
                if (m_ItemList.Count > 0)
                {
                    for (var i = 0; i < m_ItemList.Count; i++)
                    {
                        // 人物包裹不为空
                        UserItem = m_ItemList[i];
                        if (UserItem != null)
                        {
                            AmuletStdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                            if (AmuletStdItem != null)
                            {
                                if (AmuletStdItem.StdMode == 25)
                                {
                                    switch (nType)
                                    {
                                        case 1:
                                            if (AmuletStdItem.Shape == 5 && HUtil32.Round(UserItem.Dura / 100) >= nCount)
                                            {
                                                result = true;
                                                return result;
                                            }
                                            break;
                                        case 2:
                                            if (AmuletStdItem.Shape <= 2 && HUtil32.Round(UserItem.Dura / 100) >= nCount)
                                            {
                                                result = true;
                                                return result;
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                M2Share.MainOutMessage("TAIPlayObject.CheckHeroAmulet");
            }
            return result;
        }

        private int GetDirBaseObjectsCount(int m_btDirection, int rang)
        {
            return 0;
        }
    }
}