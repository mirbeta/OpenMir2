using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SystemModule.Common;

namespace M2Server
{
    /// <summary>
    /// 假人
    /// </summary>
    public class TAIPlayObject : TPlayObject
    {
        public long m_dwSearchTargetTick = 0;
        /// <summary>
        /// 假人启动
        /// </summary>
        public bool m_boAIStart = false;
        /// <summary>
        /// 挂机地图
        /// </summary>
        public TEnvirnoment m_ManagedEnvir = null;
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
        public TMapItem m_SelMapItem = null;
        /// <summary>
        /// 使用合击间隔
        /// </summary>
        public long m_dwHeroUseSpellTick = 0;
        /// <summary>
        /// 跑步计时
        /// </summary>
        public long dwTick5F4 = 0;
        /// <summary>
        /// 连击技能列表
        /// </summary>
        public ArrayList m_BatterMagicList = null;
        /// <summary>
        /// 受攻击说话列表
        /// </summary>
        public ArrayList m_AISayMsgList = null;
        /// <summary>
        /// 自动召唤英雄
        /// </summary>
        public bool m_boAutoRecallHero = false;
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

        public TAIPlayObject() : base()
        {
            m_nSoftVersionDate = grobal2.CLIENT_VERSION_NUMBER;
            m_nSoftVersionDateEx = M2Share.GetExVersionNO(grobal2.CLIENT_VERSION_NUMBER, ref m_nSoftVersionDate);
            AbilCopyToWAbil();
            m_btAttatckMode = 0;
            m_boAI = true;
            m_boLoginNoticeOK = true;
            m_boAIStart = false;
            // 开始挂机
            m_ManagedEnvir = null;
            // 挂机地图
            m_Path = null;
            m_nPostion = -1;
            m_sFilePath = "";
            m_sConfigFileName = "";
            m_sHeroConfigFileName = "";
            m_sConfigListFileName = "";
            m_sHeroConfigListFileName = "";
            m_UseItemNames = new string[13];
            //FillChar(m_UseItemNames, sizeof(Grobal2.string), '\0');
            //FillChar(m_RunPos, sizeof(TRunPos), '\0');
            m_BagItemNames = new List<string>();
            m_PointManager = new TPointManager(this);
            m_SkillUseTick = new long[59];
            //FillChar(m_SkillUseTick, sizeof(m_SkillUseTick), 0);
            // 魔法使用间隔
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
            m_dwHeroUseSpellTick = HUtil32.GetTickCount();// 使用合击间隔
            m_AISayMsgList = new ArrayList();// 受攻击说话列表
            m_boAutoRecallHero = false;// 自动召唤英雄
            n_AmuletIndx = 0;
            m_boCanPickIng = false;
            m_nSelectMagic = 0;
            m_boIsUseMagic = false;// 是否能躲避
            m_boIsUseAttackMagic = false;
            m_btLastDirection = m_btDirection;
            m_dwAutoAvoidTick = HUtil32.GetTickCount();// 自动躲避间隔
            m_boIsNeedAvoid = false;// 是否需要躲避
            m_dwWalkTick = HUtil32.GetTickCount();
            m_nWalkSpeed = 300;
            m_RunPos = new TRunPos();
            m_Path = new PointInfo[0];
        }

        ~TAIPlayObject()
        {
            //m_AISayMsgList.Free;
            m_Path = null;
            //m_BagItemNames.Free;
            //m_PointManager.Free;
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
                //if (g_FunctionNPC != null)
                //{
                    m_nScriptGotoCount = 0;
                    //g_FunctionNPC.GotoLable(this, "@AIStart", false, false);
                //}
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
                //if (g_FunctionNPC != null)
                //{
                    m_nScriptGotoCount = 0;
                    //g_FunctionNPC.GotoLable(this, "@AIStop", false, false);
                //}
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
                    PlayObject.SendMsg(PlayObject, grobal2.RM_WHISPER, 0, M2Share.g_Config.btGMWhisperMsgFColor, M2Share.g_Config.btGMWhisperMsgBColor, 0, format("%s[%d级]=> %s", new object[] { m_sCharName, m_Abil.Level, saystr }));
                    // 取得私聊信息
                    // m_GetWhisperHuman 侦听私聊对象
                    if ((m_GetWhisperHuman != null) && (!m_GetWhisperHuman.m_boGhost))
                    {
                        m_GetWhisperHuman.SendMsg(m_GetWhisperHuman, grobal2.RM_WHISPER, 0, M2Share.g_Config.btGMWhisperMsgFColor, M2Share.g_Config.btGMWhisperMsgBColor, 0, format("%s[%d级]=> %s %s", new object[] { m_sCharName, m_Abil.Level, PlayObject.m_sCharName, saystr }));
                    }
                    if ((PlayObject.m_GetWhisperHuman != null) && (!PlayObject.m_GetWhisperHuman.m_boGhost))
                    {
                        PlayObject.m_GetWhisperHuman.SendMsg(PlayObject.m_GetWhisperHuman, grobal2.RM_WHISPER, 0, M2Share.g_Config.btGMWhisperMsgFColor, M2Share.g_Config.btGMWhisperMsgBColor, 0, format("%s[%d级]=> %s %s", new object[] { m_sCharName, m_Abil.Level, PlayObject.m_sCharName, saystr }));
                    }
                }
                else
                {
                    PlayObject.SendMsg(PlayObject, grobal2.RM_WHISPER, 0, M2Share.g_Config.btGMWhisperMsgFColor, M2Share.g_Config.btWhisperMsgBColor, 0, format("%s[%d级]=> %s", new object[] { m_sCharName, m_Abil.Level, saystr }));
                    if ((m_GetWhisperHuman != null) && (!m_GetWhisperHuman.m_boGhost))
                    {
                        m_GetWhisperHuman.SendMsg(m_GetWhisperHuman, grobal2.RM_WHISPER, 0, M2Share.g_Config.btGMWhisperMsgFColor, M2Share.g_Config.btWhisperMsgBColor, 0, format("%s[%d级]=> %s %s", new object[] { m_sCharName, m_Abil.Level, PlayObject.m_sCharName, saystr }));
                    }
                    if ((PlayObject.m_GetWhisperHuman != null) && (!PlayObject.m_GetWhisperHuman.m_boGhost))
                    {
                        PlayObject.m_GetWhisperHuman.SendMsg(PlayObject.m_GetWhisperHuman, grobal2.RM_WHISPER, 0, M2Share.g_Config.btGMWhisperMsgFColor, M2Share.g_Config.btWhisperMsgBColor, 0, format("%s[%d级]=> %s %s", new object[] { m_sCharName, m_Abil.Level, PlayObject.m_sCharName, saystr }));
                    }
                }
            }
        }

        public override void ProcessSayMsg(string sData)
        {
            bool boDisableSayMsg;
            string sParam1 = string.Empty;
            const string sExceptionMsg = "TAIPlayObject.ProcessSayMsg Msg:%s";
            if (sData == "")
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
                        SC = sData.Substring(2 - 1, sData.Length - 1);
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
                            if (sData[1] == '!')
                            {
                                SC = sData.Substring(2, sData.Length - 2);
                                SendGroupText(m_sCharName + ": " + SC);
                                return;
                            }
                            if (sData[1] == '~')
                            {
                                if (m_MyGuild != null)
                                {
                                    SC = sData.Substring(2, sData.Length - 2);
                                    m_MyGuild.SendGuildMsg(m_sCharName + ": " + SC);
                                }
                                return;
                            }
                        }
                        if (!m_PEnvir.Flag.boQUIZ)
                        {
                            if ((HUtil32.GetTickCount() - m_dwShoutMsgTick) > 10 * 1000)
                            {
                                if (m_Abil.Level <= M2Share.g_Config.nCanShoutMsgLevel)
                                {
                                    SysMsg(format(M2Share.g_sYouNeedLevelMsg, new object[] { M2Share.g_Config.nCanShoutMsgLevel + 1 }), TMsgColor.c_Red, TMsgType.t_Hint);
                                    return;
                                }
                                m_dwShoutMsgTick = HUtil32.GetTickCount();
                                SC = sData.Substring(2 - 1, sData.Length - 1);
                                string sCryCryMsg = "(!)" + m_sCharName + ": " + SC;
                                if (m_boFilterSendMsg)
                                {
                                    SendMsg(null, grobal2.RM_CRY, 0, 0, 0xFFFF, 0, sCryCryMsg);
                                }
                                else
                                {
                                    M2Share.UserEngine.CryCry(grobal2.RM_CRY, m_PEnvir, m_nCurrX, m_nCurrY, 50, M2Share.g_Config.btCryMsgFColor, M2Share.g_Config.btCryMsgBColor, sCryCryMsg);
                                }
                                return;
                            }
                            SysMsg(format(M2Share.g_sYouCanSendCyCyLaterMsg, new object[] { 10 - (HUtil32.GetTickCount() - m_dwShoutMsgTick) / 1000 }), TMsgColor.c_Red, TMsgType.t_Hint);
                            return;
                        }
                        SysMsg(M2Share.g_sThisMapDisableSendCyCyMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        return;
                    }
                    if (!m_boFilterSendMsg)
                    {
                        SendRefMsg(grobal2.RM_HEAR, 0, M2Share.g_Config.btHearMsgFColor, M2Share.g_Config.btHearMsgBColor, 0, m_sCharName + ':' + sData);
                    }
                }
            }
            catch (Exception E)
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
                if ((UserMagic.MagicInfo.sMagicName).ToLower().CompareTo((sMagicName).ToLower()) == 0)
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
            if (HUtil32.GetTickCount() - dwTick5F4 > M2Share.g_Config.nAIRunIntervalTime)
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

        protected bool GotoNextOne(short nX, short nY, bool boRun)
        {
            bool result = false;
            if ((Math.Abs(nX - m_nCurrX) <= 2) && (Math.Abs(nY - m_nCurrY) <= 2))
            {
                if ((Math.Abs(nX - m_nCurrX) <= 1) && (Math.Abs(nY - m_nCurrY) <= 1))
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
                case grobal2.RM_HEAR:
                    break;
                case grobal2.RM_WHISPER:
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
                        nPos = sMsg.IndexOf("=>");
                        if ((nPos > 0) && (m_AISayMsgList.Count > 0))
                        {
                            sChrName = sMsg.Substring(1 - 1, nPos - 1);
                            sSendMsg = sMsg.Substring(nPos + 3 - 1, sMsg.Length - nPos - 2);
                            //Whisper(sChrName, m_AISayMsgList[(new System.Random(m_AISayMsgList.Count)).Next()]);
                            Console.WriteLine("TODO Hear...");
                        }
                    }
                    break;
                case grobal2.RM_CRY:
                    break;
                case grobal2.RM_SYSMESSAGE:
                    break;
                case grobal2.RM_GROUPMESSAGE:
                    break;
                case grobal2.RM_GUILDMESSAGE:
                    break;
                case grobal2.RM_MERCHANTSAY:
                    break;
            }
        }

        /// <summary>
        /// 取随机配置
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="nType"></param>
        /// <returns></returns>
        public string GetRandomConfigFileName(string sName, byte nType)
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
            switch (nType)
            {
                case 0:
                    if ((m_sConfigListFileName != "") && File.Exists(m_sConfigListFileName))
                    {
                        LoadList = new StringList();
                        LoadList.LoadFromFile(m_sConfigListFileName);
                        nIndex = (new System.Random(LoadList.Count)).Next();
                        if ((nIndex >= 0) && (nIndex < LoadList.Count))
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
                case 1:
                    if ((m_sHeroConfigListFileName != "") && File.Exists(m_sHeroConfigListFileName))
                    {
                        LoadList = new StringList();
                        LoadList.LoadFromFile(m_sHeroConfigListFileName);
                        nIndex = (new System.Random(LoadList.Count)).Next();
                        if ((nIndex >= 0) && (nIndex < LoadList.Count))
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

        public override void Initialize()
        {
            byte nAttatckMode;
            string sLineText;
            string sMagicName;
            string sItemName;
            string sSayMsg;
            IniFile ItemIni;
            IList<string> TempList;
            TUserItem UserItem;
            TMagic Magic;
            TUserMagic UserMagic;
            TItem StdItem;
            var sFileName = GetRandomConfigFileName(m_sCharName, 0);
            if ((sFileName == "") || (!File.Exists(sFileName)))
            {
                if ((m_sConfigFileName != "") && File.Exists(m_sConfigFileName))
                {
                    sFileName = m_sConfigFileName;
                }
            }
            if ((sFileName != "") && File.Exists(sFileName))
            {
                ItemIni = new IniFile(sFileName);
                ItemIni.Load();
                if (ItemIni != null)
                {
                    m_boNoDropItem = ItemIni.ReadBool("Info", "NoDropItem", true);// 是否掉包裹物品
                    m_boNoDropUseItem = ItemIni.ReadBool("Info", "DropUseItem", true);// 是否掉装备
                    m_nDropUseItemRate = ItemIni.ReadInteger("Info", "DropUseItemRate", 100);// 掉装备机率
                    m_btJob = (byte)ItemIni.ReadInteger("Info", "Job", 0);
                    m_btGender = (byte)ItemIni.ReadInteger("Info", "Gender", 0);
                    m_btHair = (byte)ItemIni.ReadInteger("Info", "Hair", 0);
                    m_Abil.Level = (byte)ItemIni.ReadInteger("Info", "Level", 1);
                    m_Abil.MaxExp = GetLevelExp(m_Abil.Level);
                    m_boProtectStatus = ItemIni.ReadBool("Info", "ProtectStatus", false);// 是否守护模式
                    nAttatckMode = (byte)ItemIni.ReadInteger("Info", "AttatckMode", 6);// 攻击模式
                    if (nAttatckMode >= 0 && nAttatckMode <= 6)
                    {
                        m_btAttatckMode = nAttatckMode;
                    }
                    sLineText = ItemIni.ReadString("Info", "UseSkill", "");
                    if (sLineText != "")
                    {
                        TempList = new List<string>();
                        try
                        {
                            //HUtil32.ArrestStringEx(new char[] { '|', '\\', '/', ',' }, new object[] { }, sLineText, TempList);
                            for (var i = 0; i < TempList.Count; i++)
                            {
                                sMagicName = TempList[i].Trim();
                                if (FindMagic(sMagicName) == null)
                                {
                                    Magic = M2Share.UserEngine.FindMagic(sMagicName);
                                    if (Magic != null)
                                    {
                                        if ((Magic.btJob == 99) || (Magic.btJob == m_btJob))
                                        {
                                            UserMagic = new TUserMagic();
                                            UserMagic.MagicInfo = Magic;
                                            UserMagic.wMagIdx = Magic.wMagicID;
                                            UserMagic.btLevel = 3;
                                            UserMagic.btKey = 0;
                                            UserMagic.nTranPoint = Magic.MaxTrain[3];
                                            m_MagicList.Add(UserMagic);
                                        }
                                    }
                                }
                            }
                        }
                        finally
                        {
                            //TempList.Free;
                        }
                    }
                    sLineText = ItemIni.ReadString("Info", "InitItems", "");
                    if (sLineText != "")
                    {
                        TempList = new List<string>();
                        try
                        {
                            //ExtractStrings(new char[] { '|', '\\', '/', ',' }, new object[] { }, sLineText, TempList);
                            for (var i = 0; i < TempList.Count; i++)
                            {
                                sItemName = TempList[i].Trim();
                                StdItem = M2Share.UserEngine.GetStdItem(sItemName);
                                if (StdItem != null)
                                {
                                    UserItem = new TUserItem();
                                    if (M2Share.UserEngine.CopyToUserItemFromName(sItemName, ref UserItem))
                                    {
                                        if (new ArrayList(new int[] { 15, 19, 20, 21, 22, 23, 24, 26 }).Contains(StdItem.StdMode))
                                        {
                                            if ((StdItem.Shape == 130) || (StdItem.Shape == 131) || (StdItem.Shape == 132))
                                            {
                                                //M2Share.UserEngine.GetUnknowItemValue(UserItem);
                                            }
                                        }
                                        if (!AddItemToBag(UserItem))
                                        {
                                            Dispose(UserItem);
                                            break;
                                        }
                                        m_BagItemNames.Add(StdItem.Name);
                                    }
                                    else
                                    {
                                        Dispose(UserItem);
                                    }
                                }
                            }
                        }
                        finally
                        {
                            //TempList.Free;
                        }
                    }
                    for (var i = 0; i <= 9; i++)
                    {
                        sSayMsg = ItemIni.ReadString("MonSay", (i).ToString(), "");
                        if (sSayMsg != "")
                        {
                            m_AISayMsgList.Add(sSayMsg);
                        }
                        else
                        {
                            break;
                        }
                    }
                    m_UseItemNames[grobal2.U_DRESS] = ItemIni.ReadString("UseItems", "UseItems0", ""); // '衣服';
                    m_UseItemNames[grobal2.U_WEAPON] = ItemIni.ReadString("UseItems", "UseItems1", ""); // '武器';
                    m_UseItemNames[grobal2.U_RIGHTHAND] = ItemIni.ReadString("UseItems", "UseItems2", ""); // '照明物';
                    m_UseItemNames[grobal2.U_NECKLACE] = ItemIni.ReadString("UseItems", "UseItems3", ""); // '项链';
                    m_UseItemNames[grobal2.U_HELMET] = ItemIni.ReadString("UseItems", "UseItems4", ""); // '头盔';
                    m_UseItemNames[grobal2.U_ARMRINGL] = ItemIni.ReadString("UseItems", "UseItems5", ""); // '左手镯';
                    m_UseItemNames[grobal2.U_ARMRINGR] = ItemIni.ReadString("UseItems", "UseItems6", ""); // '右手镯';
                    m_UseItemNames[grobal2.U_RINGL] = ItemIni.ReadString("UseItems", "UseItems7", ""); // '左戒指';
                    m_UseItemNames[grobal2.U_RINGR] = ItemIni.ReadString("UseItems", "UseItems8", ""); // '右戒指';
                    for (var i = grobal2.U_DRESS; i <= grobal2.U_CHARM; i++)
                    {
                        if (m_UseItemNames[i] != "")
                        {
                            StdItem = M2Share.UserEngine.GetStdItem(m_UseItemNames[i]);
                            if (StdItem != null)
                            {
                                UserItem = new TUserItem();
                                if (M2Share.UserEngine.CopyToUserItemFromName(m_UseItemNames[i], ref UserItem))
                                {
                                    if (new ArrayList(new int[] { 15, 19, 20, 21, 22, 23, 24, 26 }).Contains(StdItem.StdMode))
                                    {
                                        if ((StdItem.Shape == 130) || (StdItem.Shape == 131) || (StdItem.Shape == 132))
                                        {
                                            //M2Share.UserEngine.GetUnknowItemValue(UserItem);
                                        }
                                    }
                                }
                                m_UseItems[i] = UserItem;
                                Dispose(UserItem);
                            }
                        }
                    }
                }
            }
            base.Initialize();
        }

        public void SearchPickUpItem_SetHideItem(TMapItem MapItem)
        {
            TVisibleMapItem VisibleMapItem;
            for (var i = 0; i < m_VisibleItems.Count; i++)
            {
                VisibleMapItem = m_VisibleItems[i];
                if ((VisibleMapItem != null) && (VisibleMapItem.nVisibleFlag > 0))
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
            TItem StdItem;
            TMapItem MapItem = m_PEnvir.GetItem(nX, nY);
            if (MapItem == null)
            {
                return result;
            }
            if (MapItem.Name.ToLower().CompareTo((grobal2.sSTRING_GOLDNAME).ToLower()) == 0)
            {
                if (m_PEnvir.DeleteFromMap(nX, nY, grobal2.OS_ITEMOBJECT, MapItem) == 1)
                {
                    if (this.IncGold(MapItem.Count))
                    {
                        SendRefMsg(grobal2.RM_ITEMHIDE, 0, MapItem.Id, nX, nY, "");
                        result = true;
                        GoldChanged();
                        SearchPickUpItem_SetHideItem(MapItem);
                        Dispose(MapItem);
                    }
                    else
                    {
                        m_PEnvir.AddToMap(nX, nY, grobal2.OS_ITEMOBJECT, MapItem);
                    }
                }
                else
                {
                    m_PEnvir.AddToMap(nX, nY, grobal2.OS_ITEMOBJECT, MapItem);
                }
            }
            else
            {
                // 捡物品
                /*if (new ArrayList(new int[] { 2, 3 }).Contains(MapItem.UserItem.AddValue[0]))
                {
                    return result;
                }*/
                // 绑定物品不能捡 20110528
                StdItem = M2Share.UserEngine.GetStdItem(MapItem.UserItem.wIndex);
                if (StdItem != null)
                {
                    if (m_PEnvir.DeleteFromMap(nX, nY, grobal2.OS_ITEMOBJECT, MapItem) == 1)
                    {
                        UserItem = new TUserItem();
                        //FillChar(UserItem, sizeof(TUserItem), '\0');
                        UserItem = MapItem.UserItem;
                        StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                        if ((StdItem != null) && IsAddWeightAvailable(M2Share.UserEngine.GetStdItemWeight(UserItem.wIndex)))
                        {
                            //if (GetCheckItemList(18, StdItem.Name))
                            //{
                            //    // 判断是否为绑定48时物品
                            //    UserItem.AddValue[0] = 2;
                            //    UserItem.MaxDate = HUtil32.IncDayHour(DateTime.Now, 48); // 解绑时间
                            //}
                            if (AddItemToBag(UserItem))
                            {
                                SendRefMsg(grobal2.RM_ITEMHIDE, 0, MapItem.Id, nX, nY, "");
                                this.SendAddItem(UserItem);
                                m_WAbil.Weight = RecalcBagWeight();
                                result = true;
                                SearchPickUpItem_SetHideItem(MapItem);
                                Dispose(MapItem);
                            }
                            else
                            {
                                Dispose(UserItem);
                                m_PEnvir.AddToMap(nX, nY, grobal2.OS_ITEMOBJECT, MapItem);
                            }
                        }
                        else
                        {
                            Dispose(UserItem);
                            m_PEnvir.AddToMap(nX, nY, grobal2.OS_ITEMOBJECT, MapItem);
                        }
                    }
                    else
                    {
                        Dispose(UserItem);
                        m_PEnvir.AddToMap(nX, nY, grobal2.OS_ITEMOBJECT, MapItem);
                    }
                }
            }
            return result;
        }

        private bool SearchPickUpItem(int nPickUpTime)
        {
            bool result = false;
            TVisibleMapItem VisibleMapItem = null;
            bool boFound;
            int n01;
            int n02;
            try
            {
                if (HUtil32.GetTickCount() - m_dwPickUpItemTick < nPickUpTime)
                {
                    return result;
                }
                m_dwPickUpItemTick = HUtil32.GetTickCount();
                if (this.IsEnoughBag() && (m_TargetCret == null))
                {
                    boFound = false;
                    if (m_SelMapItem != null)
                    {
                        m_boCanPickIng = true;
                        for (var i = 0; i < m_VisibleItems.Count; i++)
                        {
                            VisibleMapItem = m_VisibleItems[i];
                            if ((VisibleMapItem != null) && (VisibleMapItem.nVisibleFlag > 0))
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
                    TVisibleMapItem SelVisibleMapItem = null;
                    boFound = false;
                    if (m_SelMapItem != null)
                    {
                        for (var i = 0; i < m_VisibleItems.Count; i++)
                        {
                            VisibleMapItem = m_VisibleItems[i];
                            if ((VisibleMapItem != null) && (VisibleMapItem.nVisibleFlag > 0))
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
                            if ((VisibleMapItem != null))
                            {
                                if (VisibleMapItem.nVisibleFlag > 0)
                                {
                                    TMapItem MapItem = VisibleMapItem.MapItem;
                                    if (MapItem != null)
                                    {
                                        if (IsAllowAIPickUpItem(VisibleMapItem.sName) && IsAddWeightAvailable(M2Share.UserEngine.GetStdItemWeight(MapItem.UserItem.wIndex)))
                                        {
                                            if ((MapItem.OfBaseObject == null) || (MapItem.OfBaseObject == this) || (((TBaseObject)MapItem.OfBaseObject).m_Master == this))
                                            {
                                                if ((Math.Abs(VisibleMapItem.nX - m_nCurrX) <= 5) && (Math.Abs(VisibleMapItem.nY - m_nCurrY) <= 5))
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
                        if ((m_nCurrX != SelVisibleMapItem.nX) || (m_nCurrY != SelVisibleMapItem.nY))
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
                m_wStatusTimeArr[grobal2.STATE_TRANSPARENT] = 1;// 隐身,一动就显身
            }
            if (((m_wStatusTimeArr[grobal2.POISON_STONE] != 0) && (!M2Share.g_Config.ClientConf.boParalyCanSpell)) || (m_wStatusTimeArr[grobal2.POISON_DONTMOVE] != 0) || (m_wStatusTimeArr[grobal2.POISON_LOCKSPELL] != 0))
            {
                return result;// 麻痹不能跑动 
            }
            if ((nTargetX != m_nCurrX) || (nTargetY != m_nCurrY))
            {
                if (HUtil32.GetTickCount() - dwTick3F4 > m_dwTurnIntervalTime)// 转向间隔
                {
                    n10 = nTargetX;
                    n14 = nTargetY;
                    byte nDir = grobal2.DR_DOWN;
                    if (n10 > m_nCurrX)
                    {
                        nDir = grobal2.DR_RIGHT;
                        if (n14 > m_nCurrY)
                        {
                            nDir = grobal2.DR_DOWNRIGHT;
                        }
                        if (n14 < m_nCurrY)
                        {
                            nDir = grobal2.DR_UPRIGHT;
                        }
                    }
                    else
                    {
                        if (n10 < m_nCurrX)
                        {
                            nDir = grobal2.DR_LEFT;
                            if (n14 > m_nCurrY)
                            {
                                nDir = grobal2.DR_DOWNLEFT;
                            }
                            if (n14 < m_nCurrY)
                            {
                                nDir = grobal2.DR_UPLEFT;
                            }
                        }
                        else
                        {
                            if (n14 > m_nCurrY)
                            {
                                nDir = grobal2.DR_DOWN;
                            }
                            else if (n14 < m_nCurrY)
                            {
                                nDir = grobal2.DR_UP;
                            }
                        }
                    }
                    nOldX = m_nCurrX;
                    nOldY = m_nCurrY;
                    WalkTo(nDir, false);
                    if ((nTargetX == m_nCurrX) && (nTargetY == m_nCurrY))
                    {
                        result = true;
                        dwTick3F4 = HUtil32.GetTickCount();
                    }
                    if (!result)
                    {
                        n20 = (new System.Random(3)).Next();
                        for (var i = grobal2.DR_UP; i <= grobal2.DR_UPLEFT; i++)
                        {
                            if ((nOldX == m_nCurrX) && (nOldY == m_nCurrY))
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
                                    nDir = grobal2.DR_UPLEFT;
                                }
                                if ((nDir > grobal2.DR_UPLEFT))
                                {
                                    nDir = grobal2.DR_UP;
                                }
                                WalkTo(nDir, false);
                                if ((nTargetX == m_nCurrX) && (nTargetY == m_nCurrY))
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
            if (((m_nCurrX != m_nProtectTargetX) || (m_nCurrY != m_nProtectTargetY)))
            {
                n10 = m_nProtectTargetX;
                n14 = m_nProtectTargetY;
                dwTick3F4 = HUtil32.GetTickCount();
                nDir = grobal2.DR_DOWN;
                if (n10 > m_nCurrX)
                {
                    nDir = grobal2.DR_RIGHT;
                    if (n14 > m_nCurrY)
                    {
                        nDir = grobal2.DR_DOWNRIGHT;
                    }
                    if (n14 < m_nCurrY)
                    {
                        nDir = grobal2.DR_UPRIGHT;
                    }
                }
                else
                {
                    if (n10 < m_nCurrX)
                    {
                        nDir = grobal2.DR_LEFT;
                        if (n14 > m_nCurrY)
                        {
                            nDir = grobal2.DR_DOWNLEFT;
                        }
                        if (n14 < m_nCurrY)
                        {
                            nDir = grobal2.DR_UPLEFT;
                        }
                    }
                    else
                    {
                        if (n14 > m_nCurrY)
                        {
                            nDir = grobal2.DR_DOWN;
                        }
                        else if (n14 < m_nCurrY)
                        {
                            nDir = grobal2.DR_UP;
                        }
                    }
                }
                nOldX = m_nCurrX;
                nOldY = m_nCurrY;
                if ((Math.Abs(m_nCurrX - m_nProtectTargetX) >= 3) || (Math.Abs(m_nCurrY - m_nProtectTargetY) >= 3))
                {
                    //m_dwStationTick = HUtil32.GetTickCount();// 增加检测人物站立时间
                    if (!RunTo1(nDir, false, m_nProtectTargetX, m_nProtectTargetY))
                    {
                        WalkTo(nDir, false);
                        n20 = (new System.Random(3)).Next();
                        for (var i = grobal2.DR_UP; i <= grobal2.DR_UPLEFT; i++)
                        {
                            if ((nOldX == m_nCurrX) && (nOldY == m_nCurrY))
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
                                    nDir = grobal2.DR_UPLEFT;
                                }
                                if ((nDir > grobal2.DR_UPLEFT))
                                {
                                    nDir = grobal2.DR_UP;
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
                    n20 = (new System.Random(3)).Next();
                    for (var i = grobal2.DR_UP; i <= grobal2.DR_UPLEFT; i++)
                    {
                        if ((nOldX == m_nCurrX) && (nOldY == m_nCurrY))
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
                                nDir = grobal2.DR_UPLEFT;
                            }
                            if ((nDir > grobal2.DR_UPLEFT))
                            {
                                nDir = grobal2.DR_UP;
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
            if (m_boAIStart && (m_TargetCret == null) && !m_boCanPickIng && !m_boGhost && !m_boDeath && !m_boFixedHideMode && !m_boStoneMode && (m_wStatusTimeArr[grobal2.POISON_STONE] == 0))
            {
                nX = m_nCurrX;
                nY = m_nCurrY;
                if ((m_Path != null) && (m_Path.Length > 0) && (m_nPostion < m_Path.Length))
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
                    if ((Math.Abs(nX - m_nCurrX) > 2) || (Math.Abs(nY - m_nCurrY) > 2))
                    {
                        m_Path = M2Share.g_FindPath.FindPath(m_PEnvir, m_nCurrX, m_nCurrY, nX, nY, true);
                        m_nPostion = 0;
                        if ((m_Path.Length > 0) && (m_nPostion < m_Path.Length))
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
                    if (((new Random(2)).Next() == 1))
                    {
                        TurnTo((byte)(new Random(8)).Next());
                    }
                    else
                    {
                        WalkTo(m_btDirection, false);
                    }
                    m_Path = null;
                    m_nPostion = -1;
                    m_nMoveFailCount++;
                }
            }
            if (m_nMoveFailCount >= 3)
            {
                if (((new Random(2)).Next() == 1))
                {
                    TurnTo((byte)(new Random(8)).Next());
                }
                else
                {
                    WalkTo(m_btDirection, false);
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

        public override void Struck(TBaseObject hiter)
        {
            bool boDisableSayMsg;
            m_dwStruckTick = HUtil32.GetTickCount();
            if (hiter != null)
            {
                if ((m_TargetCret == null) && IsProperTarget(hiter))
                {
                    SetTargetCreat(hiter);
                }
                else
                {
                    if ((hiter.m_btRaceServer == grobal2.RC_PLAYOBJECT) || ((hiter.m_Master != null) && (hiter.GetMaster().m_btRaceServer == grobal2.RC_PLAYOBJECT)))
                    {
                        if ((m_TargetCret != null) && ((m_TargetCret.m_btRaceServer == grobal2.RC_PLAYOBJECT) || ((m_TargetCret.m_Master != null) && (m_TargetCret.GetMaster().m_btRaceServer == grobal2.RC_PLAYOBJECT))))
                        {
                            if ((Struck_MINXY(m_TargetCret, hiter) == hiter) || ((new System.Random(6)).Next() == 0))
                            {
                                SetTargetCreat(hiter);
                            }
                        }
                        else
                        {
                            SetTargetCreat(hiter);
                        }
                    }
                    else
                    {
                        if (((m_TargetCret != null) && (Struck_MINXY(m_TargetCret, hiter) == hiter)) || ((new System.Random(6)).Next() == 0))
                        {
                            if ((m_btJob > 0) || ((m_TargetCret != null) && (HUtil32.GetTickCount() - m_dwTargetFocusTick > 1000 * 3)))
                            {
                                if (IsProperTarget(hiter))
                                {
                                    SetTargetCreat(hiter);
                                }
                            }
                        }
                    }
                }
                if ((hiter.m_btRaceServer == grobal2.RC_PLAYOBJECT) && (!hiter.m_boAI) && (m_TargetCret == hiter))
                {
                    if (((new Random(8)).Next() == 0) && (m_AISayMsgList.Count > 0))
                    {
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
                        //g_DenySayMsgList.UnLock;
                        if (!boDisableSayMsg)
                        {
                            SendRefMsg(grobal2.RM_HEAR, 0, M2Share.g_Config.btHearMsgFColor, M2Share.g_Config.btHearMsgBColor, 0, m_sCharName + ':' + m_AISayMsgList[(new System.Random(m_AISayMsgList.Count)).Next()]);
                        }
                    }
                }
            }
            if (m_boAnimal)
            {
                m_nMeatQuality = (ushort)(m_nMeatQuality - (new System.Random(300)).Next());
                if (m_nMeatQuality < 0)
                {
                    m_nMeatQuality = 0;
                }
            }
            m_dwHitTick = (ushort)(m_dwHitTick + (150 - HUtil32._MIN(130, m_Abil.Level * 4)));
        }

        protected override void SearchTarget()
        {
            if (((m_TargetCret == null) || (HUtil32.GetTickCount() - m_dwSearchTargetTick > 1000)) && m_boAIStart)
            {
                m_dwSearchTargetTick = HUtil32.GetTickCount();
                if ((m_TargetCret == null) || (!((m_TargetCret != null) && (m_TargetCret.m_btRaceServer == grobal2.RC_PLAYOBJECT)) || ((m_TargetCret.m_Master != null) && (m_TargetCret.m_Master.m_btRaceServer == grobal2.RC_PLAYOBJECT)) || (HUtil32.GetTickCount() - m_dwStruckTick > 15000)))
                {
                    base.SearchTarget();
                }
            }
        }

        public override void Die()
        {
            if (m_boAIStart)
            {
                m_boAIStart = false;
            }
            base.Die();
            //制造一个人工智障
            M2Share.UserEngine.AddAILogon(new TAILogon()
            {
                sCharName = this.m_sCharName,
                sMapName = "0",
                sConfigFileName = "",
                sHeroConfigFileName = "",
                sFilePath = M2Share.g_Config.sEnvirDir,
                sConfigListFileName = M2Share.g_Config.sAIConfigListFileName,
                sHeroConfigListFileName = M2Share.g_Config.sHeroAIConfigListFileName,
                nX = 285,
                nY = 608
            });
        }

        private bool CanWalk(short nCurrX, short nCurrY, int nTargetX, int nTargetY, byte nDir, ref int nStep, bool boFlag)
        {
            bool result = false;
            short nX = 0;
            short nY = 0;
            nStep = 0;
            byte btDir;
            if ((nDir >= 0) && (nDir <= 7))
            {
                btDir = nDir;
            }
            else
            {
                btDir = M2Share.GetNextDirection(nCurrX, nCurrY, nTargetX, nTargetY);
            }
            if (boFlag)
            {
                if ((Math.Abs(nCurrX - nTargetX) <= 1) && (Math.Abs(nCurrY - nTargetY) <= 1))
                {
                    if (m_PEnvir.GetNextPosition(nCurrX, nCurrY, btDir, 1, ref nX, ref nY) && (nX == nTargetX) && (nY == nTargetY))
                    {
                        nStep = 1;
                        result = true;
                    }
                }
                else
                {
                    if (m_PEnvir.GetNextPosition(nCurrX, nCurrY, btDir, 2, ref nX, ref nY) && (nX == nTargetX) && (nY == nTargetY))
                    {
                        nStep = 1;
                        result = true;
                    }
                }
            }
            else
            {
                if (m_PEnvir.GetNextPosition(nCurrX, nCurrY, btDir, 1, ref nX, ref nY) && (nX == nTargetX) && (nY == nTargetY))
                {
                    nStep = nStep + 1;
                    result = true;
                    return result;
                }
                else
                {
                    return result;
                }
                if (m_PEnvir.GetNextPosition(nX, nY, btDir, 1, ref nX, ref nY) && (nX == nTargetX) && (nY == nTargetY))
                {
                    nStep = nStep + 1;
                    result = true;
                    return result;
                }
            }
            return result;
        }

        private bool IsGotoXY(short X1, short Y1, short X2, short Y2)
        {
            bool result = false;
            int nStep = 0;
            PointInfo[] Path;
            //0代替-1
            if (!CanWalk(X1, Y1, X2, Y2, 0, ref nStep, m_btRaceServer != 108))
            {
                Path = M2Share.g_FindPath.FindPath(m_PEnvir, X1, Y1, X2, Y2, false);
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
            PointInfo[] Path;
            if ((Math.Abs(nX - m_nCurrX) <= 2) && (Math.Abs(nY - m_nCurrY) <= 2))
            {
                if ((Math.Abs(nX - m_nCurrX) <= 1) && (Math.Abs(nY - m_nCurrY) <= 1))
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
                Path = M2Share.g_FindPath.FindPath(m_PEnvir, m_nCurrX, m_nCurrY, nX, nY, boRun);
                if (Path.Length > 0)
                {
                    for (var i = 0; i < Path.Length; i++)
                    {
                        if ((Path[i].nX != m_nCurrX) || (Path[i].nY != m_nCurrY))
                        {
                            if ((Math.Abs(Path[i].nX - m_nCurrX) >= 2) || (Math.Abs(Path[i].nY - m_nCurrY) >= 2))
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

        public override bool Operate(TProcessMessage ProcessMsg)
        {
            bool result = false;
            TBaseObject AttackBaseObject;
            try
            {
                if (ProcessMsg.wIdent == grobal2.RM_STRUCK)
                {
                    if (ProcessMsg.BaseObject == this.ObjectId)
                    {
                        AttackBaseObject = M2Share.ObjectSystem.Get(ProcessMsg.nParam3);
                        if (AttackBaseObject != null)
                        {
                            if (AttackBaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT)
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
            catch
            {
                //M2Share.MainOutMessage(format(sExceptionMsg0, new int[] { m_sCharName, ProcessMsg.wIdent, ProcessMsg.BaseObject, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.sMsg, nError }));
            }
            return result;
        }

        private int GetRangeTargetCountByDir(byte nDir, short nX, short nY, int nRange)
        {
            int result = 0;
            TBaseObject BaseObject;
            short nCurrX = nX;
            short nCurrY = nY;
            for (var i = 1; i <= nRange; i++)
            {
                if (m_PEnvir.GetNextPosition(nCurrX, nCurrY, nDir, 1, ref nCurrX, ref nCurrY))
                {
                    BaseObject = (TBaseObject)m_PEnvir.GetMovingObject(nCurrX, nCurrY, true);
                    if ((BaseObject != null) && (!BaseObject.m_boDeath) && (!BaseObject.m_boGhost) && (!BaseObject.m_boHideMode || m_boCoolEye) && IsProperTarget(BaseObject))
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
            for (var n10 = 0; n10 <= 7; n10++)
            {
                if (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, n10, 1, ref nX, ref nY))
                {
                    BaseObject = (TBaseObject)m_PEnvir.GetMovingObject(nX, nY, true);
                    if ((BaseObject != null) && (!BaseObject.m_boDeath) && (!BaseObject.m_boGhost) && IsProperTarget(BaseObject))
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
            int n10;
            short nX = 0;
            short nY = 0;
            TBaseObject BaseObject = (TBaseObject)m_PEnvir.GetMovingObject(nCurrX, nCurrY, true);
            if ((BaseObject != null) && (!BaseObject.m_boDeath) && (!BaseObject.m_boGhost) && IsProperTarget(BaseObject))
            {
                result++;
            }
            for (n10 = 0; n10 <= 7; n10++)
            {
                if (m_PEnvir.GetNextPosition(nCurrX, nCurrY, n10, 1, ref nX, ref nY))
                {
                    BaseObject = ((TBaseObject)(m_PEnvir.GetMovingObject(nX, nY, true)));
                    if ((BaseObject != null) && (!BaseObject.m_boDeath) && (!BaseObject.m_boGhost) && IsProperTarget(BaseObject))
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        private int GetMasterRange(int nTargetX, int nTargetY)
        {
            int result = 0;
            if ((m_Master != null))
            {
                short nCurrX = m_Master.m_nCurrX;
                short nCurrY = m_Master.m_nCurrY;
                result = Math.Abs(nCurrX - nTargetX) + Math.Abs(nCurrY - nTargetY);
            }
            return result;
        }

        private bool FollowMaster()
        {
            short nX = 0;
            short nY = 0;
            short nCurrX = 0;
            short nCurrY = 0;
            int nStep;
            bool boNeed = false;
            bool result = false;
            if ((!m_Master.m_boSlaveRelax))
            {
                if ((m_PEnvir != m_Master.m_PEnvir) || (Math.Abs(m_nCurrX - m_Master.m_nCurrX) > 20) || (Math.Abs(m_nCurrY - m_Master.m_nCurrY) > 20))
                {
                    boNeed = true;
                }
            }
            if (boNeed)
            {
                m_Master.GetBackPosition(ref nX, ref nY);
                if (!m_Master.m_PEnvir.CanWalk(nX, nY, true))
                {
                    for (var i = 0; i <= 7; i++)
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
                SpaceMove(m_Master.m_PEnvir.sMapName, m_nTargetX, m_nTargetY, 1);
                result = true;
                return result;
            }
            m_Master.GetBackPosition(ref nCurrX, ref nCurrY);
            if ((m_TargetCret == null) && (!m_Master.m_boSlaveRelax))
            {
                for (var I = 1; I <= 2; I++)
                {
                    // 判断主人是否在英雄对面
                    if (m_Master.m_PEnvir.GetNextPosition(m_Master.m_nCurrX, m_Master.m_nCurrY, m_Master.m_btDirection, I, ref nX, ref nY))
                    {
                        if ((m_nCurrX == nX) && (m_nCurrY == nY))
                        {
                            if (m_Master.GetBackPosition(ref nX, ref nY) && GotoNext(nX, nY, true))
                            {
                                result = true;
                                return result;
                            }
                            for (var k = 1; k <= 2; k++)
                            {
                                for (var j = 0; j <= 7; j++)
                                {
                                    if (j != m_Master.m_btDirection)
                                    {
                                        if (m_Master.m_PEnvir.GetNextPosition(m_Master.m_nCurrX, m_Master.m_nCurrY, j, k, ref nX, ref nY) && GotoNext(nX, nY, true))
                                        {
                                            result = true;
                                            return result;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
                if (m_btRaceServer == 108)
                {
                    nStep = 0;
                }
                else
                {
                    nStep = 1;
                }
                // 是否为月灵
                if ((Math.Abs(m_nCurrX - nCurrX) > nStep) || (Math.Abs(m_nCurrY - nCurrY) > nStep))
                {
                    if (GotoNextOne(nCurrX, nCurrY, true))
                    {
                        return result;
                    }
                    if (GotoNextOne(nX, nY, true))
                    {
                        return result;
                    }
                    for (var j = 1; j <= 2; j++)
                    {
                        for (var k = 0; k <= 7; k++)
                        {
                            if (k != m_Master.m_btDirection)
                            {
                                if (m_Master.m_PEnvir.GetNextPosition(m_Master.m_nCurrX, m_Master.m_nCurrY, k, j, ref nX, ref nY) && GotoNextOne(nX, nY, true))
                                {
                                    result = true;
                                    return result;
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
                if ((m_VisibleActors[i].BaseObject == ActorObject))
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
                    result = (UserMagic.btKey == 0) || m_boAI;
                }
                else
                {
                    result = (UserMagic.btKey == 0) || m_boAI;
                }
            }
            return result;
        }

        private bool CheckUserItem(int nItemType, int nCount)
        {
            return CheckUserItemType(nItemType, nCount) || (GetUserItemList(nItemType, nCount) >= 0);
        }

        private bool CheckItemType(int nItemType, TItem StdItem)
        {
            bool result = false;
            switch (nItemType)
            {
                case 1:
                    if ((StdItem.StdMode == 25) && (StdItem.Shape == 1))
                    {
                        result = true;
                    }
                    break;
                case 2:
                    if ((StdItem.StdMode == 25) && (StdItem.Shape == 2))
                    {
                        result = true;
                    }
                    break;
                case 3:
                    if ((StdItem.StdMode == 25) && (StdItem.Shape == 3))
                    {
                        result = true;
                    }
                    break;
                case 5:
                    if ((StdItem.StdMode == 25) && (StdItem.Shape == 5))
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
            TItem StdItem;
            if ((m_UseItems[grobal2.U_ARMRINGL].wIndex > 0) && (Math.Round(Convert.ToDouble(m_UseItems[grobal2.U_ARMRINGL].Dura / 100)) >= nCount))
            {
                StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[grobal2.U_ARMRINGL].wIndex);
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
            TUserItem UserItem;
            TItem StdItem;
            for (var i = 0; i < m_ItemList.Count; i++)
            {
                UserItem = m_ItemList[i];
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (StdItem != null)
                {
                    if (CheckItemType(nItemType, StdItem) && (HUtil32.Round(UserItem.Dura / 100) >= nCount))
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
            TUserItem UserItem;
            TUserItem AddUserItem;
            TItem StdItem;
            bool result = false;
            if ((nIndex >= 0) && (nIndex < m_ItemList.Count))
            {
                UserItem = m_ItemList[nIndex];
                if (m_UseItems[grobal2.U_ARMRINGL].wIndex > 0)
                {
                    StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[grobal2.U_ARMRINGL].wIndex);
                    if (StdItem != null)
                    {
                        if (CheckItemType(nItemType, StdItem))
                        {
                            result = true;
                        }
                        else
                        {
                            m_ItemList.RemoveAt(nIndex);
                            AddUserItem = new TUserItem();
                            AddUserItem = m_UseItems[grobal2.U_ARMRINGL];
                            if (AddItemToBag(AddUserItem))
                            {
                                m_UseItems[grobal2.U_ARMRINGL] = UserItem;
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
                        m_UseItems[grobal2.U_ARMRINGL] = UserItem;
                        Dispose(UserItem);
                        result = true;
                    }
                }
                else
                {
                    m_ItemList.RemoveAt(nIndex);
                    m_UseItems[grobal2.U_ARMRINGL] = UserItem;
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
                    if ((BaseObject.m_boHideMode && !m_boCoolEye) || (!IsProperTarget(BaseObject)))
                    {
                        BaseObjectList.RemoveAt(i);
                    }
                }
                result = BaseObjectList.Count;
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
                if ((m_TargetCret.m_nCurrX == nX) && (m_TargetCret.m_nCurrY == nY))
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
            for (var i = 1; i <= nStep; i++)
            {
                if ((m_TargetCret.m_nCurrX == nX) && (m_TargetCret.m_nCurrY == nY))
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
            for (var i = 1; i <= nRange; i++)
            {
                if (!m_PEnvir.GetNextPosition(nCurrX, nCurrY, btDir, i, ref nX, ref nY))
                {
                    break;
                }
                if ((BaseObject.m_nCurrX == nX) && (BaseObject.m_nCurrY == nY))
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
            for (var i = 1; i <= nRange; i++)
            {
                if (!m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, btDir, i, ref nX, ref nY))
                {
                    break;
                }
                if ((BaseObject.m_nCurrX == nX) && (BaseObject.m_nCurrY == nY))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        // 1 为护身符 2 为毒药
        private bool IsUseAttackMagic()
        {
            // 检测是否可以使用攻击魔法
            TUserMagic UserMagic;
            bool result = false;
            switch (m_btJob)
            {
                case 0:
                    result = true;
                    break;
                case 1:
                    for (var i = 0; i < m_MagicList.Count; i++)
                    {
                        UserMagic = m_MagicList[i];
                        switch (UserMagic.wMagIdx)
                        {
                            case grobal2.SKILL_FIREBALL:
                            case grobal2.SKILL_FIREBALL2:
                            case grobal2.SKILL_FIRE:
                            case grobal2.SKILL_SHOOTLIGHTEN:
                            case grobal2.SKILL_LIGHTENING:
                            case grobal2.SKILL_EARTHFIRE:
                            case grobal2.SKILL_FIREBOOM:
                            case grobal2.SKILL_LIGHTFLOWER:
                            case grobal2.SKILL_SNOWWIND:
                            case grobal2.SKILL_GROUPLIGHTENING:
                            case grobal2.SKILL_47:
                            case grobal2.SKILL_58:
                                if (GetSpellPoint(UserMagic) <= m_WAbil.MP)
                                {
                                    result = true;
                                    break;
                                }
                                break;
                        }
                    }
                    break;
                case 2:
                    for (var i = 0; i < m_MagicList.Count; i++)
                    {
                        UserMagic = m_MagicList[i];
                        if (new ArrayList(new int[] { 2, 99 }).Contains(UserMagic.MagicInfo.btJob))
                        {
                            switch (UserMagic.wMagIdx)
                            {
                                // 6 施毒术
                                // 38 群体施毒术
                                case grobal2.SKILL_AMYOUNSUL:
                                case grobal2.SKILL_GROUPAMYOUNSUL:// 需要毒药
                                    result = CheckUserItem(1, 2) || CheckUserItem(2, 2);
                                    if (result)
                                    {
                                        result = AllowUseMagic(grobal2.SKILL_AMYOUNSUL) || AllowUseMagic(grobal2.SKILL_GROUPAMYOUNSUL);
                                    }
                                    if (result)
                                    {
                                        break;
                                    }
                                    break;
                                case grobal2.SKILL_FIRECHARM:// 需要符
                                    result = CheckUserItem(5, 1);
                                    if (result)
                                    {
                                        result = AllowUseMagic(grobal2.SKILL_FIRECHARM);
                                    }
                                    if (result)
                                    {
                                        break;
                                    }
                                    break;
                                case grobal2.SKILL_59:// 需要符
                                    result = CheckUserItem(5, 5);
                                    if (result)
                                    {
                                        result = AllowUseMagic(grobal2.SKILL_59);
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
            int nSpellPoint;
            int n14;
            TBaseObject BaseObject;
            bool boIsWarrSkill;
            bool result = false;
            if (!m_boCanSpell)
            {
                return result;
            }
            // 7
            if (m_boDeath || (m_wStatusTimeArr[grobal2.POISON_LOCKSPELL] != 0))
            {
                return result;
            }
            // 防麻
            if ((m_wStatusTimeArr[grobal2.POISON_STONE] != 0) && !M2Share.g_Config.ClientConf.boParalyCanSpell)
            {
                return result;
            }
            // 防麻
            if (m_PEnvir != null)
            {
                /*if (m_PEnvir.m_boNOSKILL)
                {
                    return result;
                }*/
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
                case grobal2.SKILL_ERGUM:
                    if (m_MagicErgumSkill != null)
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
                case grobal2.SKILL_BANWOL:
                    if (m_MagicBanwolSkill != null)
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
                case grobal2.SKILL_FIRESWORD:
                    if (m_MagicFireSwordSkill != null)
                    {
                        result = true;
                    }
                    break;
                case grobal2.SKILL_MOOTEBO:
                    result = true;
                    if ((HUtil32.GetTickCount() - m_dwDoMotaeboTick) > 3000)
                    {
                        m_dwDoMotaeboTick = HUtil32.GetTickCount();
                        if (GetAttackDir(TargeTBaseObject, ref m_btDirection))
                        {
                            DoMotaebo(m_btDirection, UserMagic.btLevel);
                        }
                    }
                    break;
                case grobal2.SKILL_43:
                    result = true;
                    break;
                default:
                    n14 = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, nTargetX, nTargetY);
                    m_btDirection = (byte)n14;
                    BaseObject = null;
                    // 检查目标角色，与目标座标误差范围，如果在误差范围内则修正目标座标
                    if (UserMagic.wMagIdx >= 60 && UserMagic.wMagIdx <= 65)
                    {
                        // 如果是合击锁定目标
                        if (CretInNearXY(TargeTBaseObject, nTargetX, nTargetY))
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
                            case grobal2.SKILL_HEALLING:
                            case grobal2.SKILL_HANGMAJINBUB:
                            case grobal2.SKILL_DEJIWONHO:
                            case grobal2.SKILL_BIGHEALLING:
                            case grobal2.SKILL_SINSU:
                            case grobal2.SKILL_UNAMYOUNSUL:
                            case grobal2.SKILL_46:
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
                        SendRefMsg(grobal2.RM_MAGICFIREFAIL, 0, 0, 0, 0, "");
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
                    if (BaseObject.m_boGhost || BaseObject.m_boDeath || (BaseObject.m_WAbil.HP <= 0))
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
            catch (Exception E)
            {
                M2Share.MainOutMessage(format("{%s} TAIPlayObject.AutoSpell MagID:%d X:%d Y:%d", new int[] { UserMagic.wMagIdx, nTargetX, nTargetY }));
            }
            return result;
        }

        public int DoThink_CheckTargetXYCount(int nX, int nY, int nRange)
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

        public bool DoThink_TargetNeedRunPos()
        {
            return (m_TargetCret.m_btRaceServer == grobal2.RC_PLAYOBJECT) || (m_TargetCret.m_btRaceServer == 108);
        }

        public bool DoThink_CanRunPos(int nAttackCount)
        {
            return m_RunPos.nAttackCount >= nAttackCount;
        }

        public bool DoThink_MotaeboPos(short wMagicID)
        {
            bool result = false;
            short nTargetX = 0;
            short nTargetY = 0;
            byte btNewDir;
            if ((wMagicID == 27) && (m_Master != null) && (m_TargetCret != null) && AllowUseMagic(27) && (m_TargetCret.m_Abil.Level < m_Abil.Level) && (HUtil32.GetTickCount() - m_SkillUseTick[27] > 1000 * 10))
            {
                btNewDir = M2Share.GetNextDirection(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, m_Master.m_nCurrX, m_Master.m_nCurrY);
                if (m_PEnvir.GetNextPosition(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, GetBackDir(btNewDir), 1, ref nTargetX, ref nTargetY))
                {
                    result = m_PEnvir.CanWalk(nTargetX, nTargetY, true);
                }
            }
            return result;
        }

        public bool DoThink_MagPushArround(int MagicID,short wMagicID)
        {
            bool result = false;
            TBaseObject ActorObject;
            byte btNewDir;
            short nTargetX = 0;
            short nTargetY = 0;
            if ((m_TargetCret != null) && (m_Abil.Level > m_TargetCret.m_Abil.Level) && (Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 1) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) <= 1))
            {
                btNewDir = M2Share.GetNextDirection(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, m_nCurrX, m_nCurrY);
                if (m_PEnvir.GetNextPosition(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, GetBackDir(btNewDir), 1, ref nTargetX, ref nTargetY))
                {
                    result = m_PEnvir.CanWalk(nTargetX, nTargetY, true);
                }
                if (result)
                {
                    return result;
                }
            }
            if (wMagicID == MagicID)
            {
                for (var i = 0; i < m_VisibleActors.Count; i++)
                {
                    ActorObject = m_VisibleActors[i].BaseObject;
                    if ((Math.Abs(m_nCurrX - ActorObject.m_nCurrX) <= 1) && (Math.Abs(m_nCurrY - ActorObject.m_nCurrY) <= 1))
                    {
                        if ((!ActorObject.m_boDeath) && (ActorObject != this) && IsProperTarget(ActorObject))
                        {
                            if ((m_Abil.Level > ActorObject.m_Abil.Level) && (!ActorObject.m_boStickMode))
                            {
                                btNewDir = M2Share.GetNextDirection(ActorObject.m_nCurrX, ActorObject.m_nCurrY, m_nCurrX, m_nCurrY);
                                if (m_PEnvir.GetNextPosition(ActorObject.m_nCurrX, ActorObject.m_nCurrY, GetBackDir(btNewDir), 1, ref nTargetX, ref nTargetY))
                                {
                                    if (m_PEnvir.CanWalk(nTargetX, nTargetY, true))
                                    {
                                        result = true;
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

        private int DoThink(short wMagicID)
        {
            byte btDir = 0;
            int nRange;
            int result = -1;
            switch (m_btJob)
            {
                case 0: // 1=野蛮冲撞 2=无法攻击到目标需要移动 3=走位
                    if (DoThink_MotaeboPos(wMagicID))
                    {
                        result = 1;
                    }
                    else
                    {
                        nRange = 1;
                        if (wMagicID == 43)
                        {
                            nRange = 4;
                        }
                        if ((wMagicID == 12))
                        {
                            nRange = 2;
                        }
                        // 连击技能
                        if ((new ArrayList(new int[] { 60 }).Contains(wMagicID)))
                        {
                            nRange = 6;
                        }
                        result = 2;
                        if ((new ArrayList(new int[] { 61, 62 }).Contains(wMagicID)) || CanAttack(m_TargetCret, nRange, ref btDir))
                        {
                            result = 0;
                        }
                        if (((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 2) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 2)))
                        {
                            if ((result == 0) && (!(new ArrayList(new int[] { 60, 61, 62 }).Contains(wMagicID))))
                            {
                                if (DoThink_TargetNeedRunPos())
                                {
                                    if (DoThink_CanRunPos(5))
                                    {
                                        result = 5;
                                    }
                                }
                                else
                                {
                                    if (DoThink_CanRunPos(20))
                                    {
                                        result = 5;
                                    }
                                }
                            }
                            if (((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 6) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 6)))
                            {
                                result = 2;
                            }
                        }
                    }
                    break;
                case 1:
                    if ((wMagicID == 8) && DoThink_MagPushArround(wMagicID, wMagicID))
                    {
                        return result;
                    }
                    // 1=躲避 2=追击 3=魔法直线攻击不到目标 4=无法攻击到目标需要移动 5=走位
                    if (IsUseAttackMagic())
                    {
                        // GetNearTargetCount
                        if (DoThink_CheckTargetXYCount(m_nCurrX, m_nCurrY, 2) > 0)
                        {
                            result = 1;
                        }
                        else if (((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 6) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 6)))
                        {
                            result = 2;
                        }
                        else if ((new ArrayList(new object[] { grobal2.SKILL_FIREBALL, grobal2.SKILL_FIREBALL2 }).Contains(wMagicID)) && (!CanAttack(m_TargetCret, 10, ref btDir)))
                        {
                            result = 3;
                        }
                        // GetNearTargetCount
                        else if (DoThink_TargetNeedRunPos() && DoThink_CanRunPos(5) && (DoThink_CheckTargetXYCount(m_nCurrX, m_nCurrY, 2) > 0))
                        {
                            result = 5;
                        }
                    }
                    else
                    {
                        if ((!GetAttackDir(m_TargetCret, 1, ref btDir)))
                        {
                            result = 4;
                        }
                    }
                    break;
                case 2:
                    if ((wMagicID == 48) && DoThink_MagPushArround(wMagicID, wMagicID))
                    {
                        return result;
                    }
                    // 1=躲避 2=追击 3=魔法直线攻击不到目标 4=无法攻击到目标需要移动 5=走位
                    if (IsUseAttackMagic())
                    {
                        // GetNearTargetCount
                        if (DoThink_CheckTargetXYCount(m_nCurrX, m_nCurrY, 2) > 0)
                        {
                            result = 1;
                        }
                        else if (((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 6) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 6)))
                        {
                            result = 2;
                        }
                        else if ((wMagicID == grobal2.SKILL_FIRECHARM) && (!CanAttack(m_TargetCret, 10, ref btDir)))
                        {
                            result = 3;
                        }
                        // GetNearTargetCount
                        else if (DoThink_TargetNeedRunPos() && DoThink_CanRunPos(5) && (DoThink_CheckTargetXYCount(m_nCurrX, m_nCurrY, 2) > 0))
                        {
                            result = 5;
                        }
                    }
                    else
                    {
                        if ((!GetAttackDir(m_TargetCret, 1, ref btDir)))
                        {
                            result = 4;
                        }
                    }
                    break;
            }
            return result;
        }

        public TMapWalkXY ActThink_FindGoodPathA(TMapWalkXY[] WalkStep, int nRange, int nType)
        {
            TMapWalkXY result = null;
            int n10= Int32.MaxValue;
            int nMastrRange;
            int nMonCount;
            TMapWalkXY MapWalkXY= null;
            TMapWalkXY MapWalkXYA= null;
            //FillChar(result, sizeof(TMapWalkXY), 0);
            for (var i = grobal2.DR_UP; i <= grobal2.DR_UPLEFT; i++)
            {
                if ((WalkStep[i].nWalkStep > 0) && (Math.Abs(WalkStep[i].nX - m_TargetCret.m_nCurrX) >= nRange) && (Math.Abs(WalkStep[i].nY - m_TargetCret.m_nCurrY) >= nRange))
                {
                    if ((WalkStep[i].nMonCount < n10))
                    {
                        n10 = WalkStep[i].nMonCount;
                        MapWalkXY = WalkStep[i];
                    }
                }
            }
            if ((MapWalkXY != null) && (m_Master != null))
            {
                nMonCount = MapWalkXY.nMonCount;
                nMastrRange = MapWalkXY.nMastrRange;
                n10 = Int32.MaxValue;
                MapWalkXYA = MapWalkXY;
                MapWalkXY = null;
                for (var i = grobal2.DR_UP; i <= grobal2.DR_UPLEFT; i++)
                {
                    if ((WalkStep[i].nWalkStep > 0) && (WalkStep[i].nMonCount <= nMonCount) && (Math.Abs(WalkStep[i].nX - m_TargetCret.m_nCurrX) >= nRange) && (Math.Abs(WalkStep[i].nY - m_TargetCret.m_nCurrY) >= nRange))
                    {
                        if ((WalkStep[i].nMastrRange < n10) && (WalkStep[i].nMastrRange < nMastrRange))
                        {
                            n10 = WalkStep[i].nMastrRange;
                            MapWalkXY = WalkStep[i];
                        }
                    }
                }
                if (MapWalkXY == null)
                {
                    MapWalkXY = MapWalkXYA;
                }
            }
            if (MapWalkXY != null)
            {
                result = MapWalkXY;
            }
            return result;
        }

        public TMapWalkXY ActThink_FindGoodPathB(TMapWalkXY[] WalkStep, int nType)
        {
            TMapWalkXY result = null;
            int nMastrRange;
            int nMonCount;
            TMapWalkXY MapWalkXY = null;
            int n10 = Int32.MaxValue;
            //FillChar(result, sizeof(TMapWalkXY), 0);
            for (var i = grobal2.DR_UP; i <= grobal2.DR_UPLEFT; i++)
            {
                if ((WalkStep[i].nWalkStep > 0))
                {
                    if ((WalkStep[i].nMonCount < n10))
                    {
                        n10 = WalkStep[i].nMonCount;
                        MapWalkXY = WalkStep[i];
                    }
                }
            }
            if ((MapWalkXY != null) && (m_Master != null))
            {
                nMonCount = MapWalkXY.nMonCount;
                nMastrRange = MapWalkXY.nMastrRange;
                n10 = Int32.MaxValue;
                TMapWalkXY MapWalkXYA = MapWalkXY;
                MapWalkXY = null;
                for (var i = grobal2.DR_UP; i <= grobal2.DR_UPLEFT; i++)
                {
                    if ((WalkStep[i].nWalkStep > 0) && (WalkStep[i].nMonCount <= nMonCount))
                    {
                        if ((WalkStep[i].nMastrRange < n10) && (WalkStep[i].nMastrRange < nMastrRange))
                        {
                            n10 = WalkStep[i].nMastrRange;
                            MapWalkXY = WalkStep[i];
                        }
                    }
                }
                if (MapWalkXY == null)
                {
                    MapWalkXY = MapWalkXYA;
                }
            }
            if (MapWalkXY != null)
            {
                result = MapWalkXY;
            }
            return result;
        }

        public TMapWalkXY ActThink_FindMinRange(TMapWalkXY[] WalkStep)
        {
            TMapWalkXY result = null;
            int n10 = Int32.MaxValue;
            int n1C;
            int nMonCount;
            TMapWalkXY MapWalkXY= null;
            TMapWalkXY MapWalkXYA= null;
            //FillChar(result, sizeof(TMapWalkXY), 0);
            for (var i = grobal2.DR_UP; i <= grobal2.DR_UPLEFT; i++)
            {
                if ((WalkStep[i].nWalkStep > 0))
                {
                    n1C = Math.Abs(WalkStep[i].nX - m_TargetCret.m_nCurrX) + Math.Abs(WalkStep[i].nY - m_TargetCret.m_nCurrY);
                    if ((n1C < n10))
                    {
                        n10 = n1C;
                        MapWalkXY = WalkStep[i];
                    }
                }
            }
            if (MapWalkXY != null)
            {
                nMonCount = MapWalkXY.nMonCount;
                MapWalkXYA = MapWalkXY;
                MapWalkXY = null;
                for (var i = grobal2.DR_UP; i <= grobal2.DR_UPLEFT; i++)
                {
                    if ((WalkStep[i].nWalkStep > 0) && (WalkStep[i].nMonCount <= nMonCount))
                    {
                        n1C = Math.Abs(WalkStep[i].nX - m_TargetCret.m_nCurrX) + Math.Abs(WalkStep[i].nY - m_TargetCret.m_nCurrY);
                        if ((n1C <= n10))
                        {
                            n10 = n1C;
                            MapWalkXY = WalkStep[i];
                        }
                    }
                }
                if (MapWalkXY == null)
                {
                    MapWalkXY = MapWalkXYA;
                }
            }
            if (MapWalkXY != null)
            {
                result = MapWalkXY;
            }
            return result;
        }

        public bool ActThink_CanWalkNextPosition(short nX, short nY, int nRange, byte btDir, bool boFlag)
        {
            // 检测下一步在不在攻击位
            short nCurrX = 0;
            short nCurrY = 0;
            bool result = false;
            if (m_PEnvir.GetNextPosition(nX, nY, btDir, 1, ref nCurrX, ref nCurrY) && CanMove(nX, nY, nCurrX, nCurrY, false) && !boFlag || CanAttack(nCurrX, nCurrY, m_TargetCret, nRange, ref btDir))
            {
                result = true;
                return result;
            }
            if (m_PEnvir.GetNextPosition(nX, nY, btDir, 2, ref nCurrX, ref nCurrY) && CanMove(nX, nY, nCurrX, nCurrY, false) && !boFlag || CanAttack(nCurrX, nCurrY, m_TargetCret, nRange, ref btDir))
            {
                result = true;
                return result;
            }
            return result;
        }

        public bool ActThink_FindPosOfSelf(TMapWalkXY[] WalkStep, int nRange, bool boFlag)
        {
            bool result = false;
            byte btDir = 0;
            short nCurrX = 0;
            short nCurrY = 0;
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 8, 0);
            for (var i = grobal2.DR_UP; i <= grobal2.DR_UPLEFT; i++)
            {
                if (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, i, nRange, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false))
                {
                    if (!boFlag || CanAttack(nCurrX, nCurrY, m_TargetCret, nRange, ref btDir))
                    {
                        WalkStep[i].nWalkStep = nRange;
                        WalkStep[i].nX = nCurrX;
                        WalkStep[i].nY = nCurrY;
                        WalkStep[i].nMonRange = Math.Abs(nCurrX - m_TargetCret.m_nCurrX) + Math.Abs(nCurrY - m_TargetCret.m_nCurrY);
                        WalkStep[i].nMonCount = GetNearTargetCount(nCurrX, nCurrY);
                        WalkStep[i].nMastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool ActThink__FindPosOfSelf(TMapWalkXY[] WalkStep, int nRange, bool boFlag)
        {
            byte btDir = 0;
            short nCurrX = 0;
            short nCurrY = 0;
            bool result = false;
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 8, 0);
            for (var i = grobal2.DR_UP; i <= grobal2.DR_UPLEFT; i++)
            {
                if (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, i, nRange, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false))
                {
                    if (!boFlag || CanAttack(nCurrX, nCurrY, m_TargetCret, nRange, ref btDir) || ActThink_CanWalkNextPosition(nCurrX, nCurrY, nRange, (byte)i, boFlag))
                    {
                        WalkStep[i].nWalkStep = nRange;
                        WalkStep[i].nX = nCurrX;
                        WalkStep[i].nY = nCurrY;
                        WalkStep[i].nMonRange = Math.Abs(nCurrX - m_TargetCret.m_nCurrX) + Math.Abs(nCurrY - m_TargetCret.m_nCurrY);
                        WalkStep[i].nMonCount = GetNearTargetCount(nCurrX, nCurrY);
                        WalkStep[i].nMastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool ActThink_FindPosOfTarget(TMapWalkXY[] WalkStep, short nTargetX, short nTargetY, int nRange, bool boFlag)
        {
            bool result = false;
            byte btDir = 0;
            short nCurrX = 0;
            short nCurrY = 0;
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 8, 0);
            for (var i = grobal2.DR_UP; i <= grobal2.DR_UPLEFT; i++)
            {
                if (m_PEnvir.GetNextPosition(nTargetX, nTargetY, i, nRange, ref nCurrX, ref nCurrY) && m_PEnvir.CanWalkEx(nCurrX, nCurrY, false))
                {
                    if ((!boFlag || CanAttack(nCurrX, nCurrY, m_TargetCret, nRange, ref btDir)) && IsGotoXY(m_nCurrX, m_nCurrY, nCurrX, nCurrY))
                    {
                        WalkStep[i].nWalkStep = nRange;
                        WalkStep[i].nX = nCurrX;
                        WalkStep[i].nY = nCurrY;
                        WalkStep[i].nMonRange = Math.Abs(nCurrX - nTargetX) + Math.Abs(nCurrY - nTargetY);
                        WalkStep[i].nMastrRange = GetMasterRange(nCurrX, nCurrY);
                        WalkStep[i].nMonCount = GetRangeTargetCount(nCurrX, nCurrY, 2);
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool ActThink_FindPos(TMapWalkXY[] WalkStep, int nRange, bool boFlag)
        {
            bool result = false;
            byte btDir = 0;
            short nCurrX = 0;
            short nCurrY = 0;
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 8, 0);
            for (var i = grobal2.DR_UP; i <= grobal2.DR_UPLEFT; i++)
            {
                if (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, i, 2, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false))
                {
                    if (!boFlag || CanAttack(nCurrX, nCurrY, m_TargetCret, nRange, ref btDir))
                    {
                        WalkStep[i].nWalkStep = nRange;
                        WalkStep[i].nX = nCurrX;
                        WalkStep[i].nY = nCurrY;
                        WalkStep[i].nMonRange = Math.Abs(nCurrX - m_TargetCret.m_nCurrX) + Math.Abs(nCurrY - m_TargetCret.m_nCurrY);
                        WalkStep[i].nMonCount = GetNearTargetCount(nCurrX, nCurrY);
                        WalkStep[i].nMastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            if (result)
            {
                return result;
            }
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 8, 0);
            for (var i = grobal2.DR_UP; i <= grobal2.DR_UPLEFT; i++)
            {
                if (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, i, 1, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false))
                {
                    if (!boFlag || CanAttack(nCurrX, nCurrY, m_TargetCret, nRange, ref btDir))
                    {
                        WalkStep[i].nWalkStep = nRange;
                        WalkStep[i].nX = nCurrX;
                        WalkStep[i].nY = nCurrY;
                        WalkStep[i].nMonRange = Math.Abs(nCurrX - m_TargetCret.m_nCurrX) + Math.Abs(nCurrY - m_TargetCret.m_nCurrY);
                        WalkStep[i].nMonCount = GetNearTargetCount(nCurrX, nCurrY);
                        WalkStep[i].nMastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool ActThink__FindPos(TMapWalkXY[] WalkStep, int nRange, bool boFlag)
        {
            bool result = false;
            byte btDir = 0;
            short nCurrX = 0;
            short nCurrY = 0;
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 8, 0);
            for (var i = grobal2.DR_UP; i <= grobal2.DR_UPLEFT; i++)
            {
                if (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, i, 1, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false))
                {
                    if (!boFlag || CanAttack(nCurrX, nCurrY, m_TargetCret, nRange, ref btDir) || ActThink_CanWalkNextPosition(nCurrX, nCurrY, nRange, (byte)i, boFlag))
                    {
                        WalkStep[i].nWalkStep = nRange;
                        WalkStep[i].nX = nCurrX;
                        WalkStep[i].nY = nCurrY;
                        WalkStep[i].nMonRange = Math.Abs(nCurrX - m_TargetCret.m_nCurrX) + Math.Abs(nCurrY - m_TargetCret.m_nCurrY);
                        WalkStep[i].nMonCount = GetNearTargetCount(nCurrX, nCurrY);
                        WalkStep[i].nMastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            if (result)
            {
                return result;
            }
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 8, 0);
            for (var i = grobal2.DR_UP; i <= grobal2.DR_UPLEFT; i++)
            {
                if (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, i, 2, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false))
                {
                    if (!boFlag || CanAttack(nCurrX, nCurrY, m_TargetCret, nRange, ref btDir) || ActThink_CanWalkNextPosition(nCurrX, nCurrY, nRange, (byte)i, boFlag))
                    {
                        WalkStep[i].nWalkStep = nRange;
                        WalkStep[i].nX = nCurrX;
                        WalkStep[i].nY = nCurrY;
                        WalkStep[i].nMonRange = Math.Abs(nCurrX - m_TargetCret.m_nCurrX) + Math.Abs(nCurrY - m_TargetCret.m_nCurrY);
                        WalkStep[i].nMonCount = GetNearTargetCount(nCurrX, nCurrY);
                        WalkStep[i].nMastrRange = GetMasterRange(nCurrX, nCurrY);
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool ActThink_WalkToRightPos(int wMagicID)
        {
            bool boFlag;
            int nRange;
            TMapWalkXY[] WalkStep = null;
            TMapWalkXY MapWalkXY;
            bool result = false;
            try
            {
                boFlag = (m_btRaceServer == 108) || (new ArrayList(new object[] { grobal2.SKILL_FIREBALL, grobal2.SKILL_FIREBALL2, grobal2.SKILL_FIRECHARM }).Contains(wMagicID)) || (m_btJob == 0);
                if ((m_btJob == 0) || (wMagicID <= 0))
                {
                    nRange = 1;
                    if (wMagicID == 43)
                    {
                        nRange = 4;
                    }
                    if ((wMagicID == 12))
                    {
                        nRange = 2;
                    }
                    if ((new ArrayList(new int[] { 60, 61, 62 }).Contains(wMagicID)))
                    {
                        nRange = 6;
                    }
                    for (var i = nRange; i >= 1; i--)
                    {
                        if (ActThink_FindPosOfTarget(WalkStep, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, i, boFlag))
                        {
                            MapWalkXY = ActThink_FindGoodPathB(WalkStep, 0);
                            if ((MapWalkXY.nWalkStep > 0))
                            {
                                // if RunToTargetXY(MapWalkXY.nX, MapWalkXY.nY) then begin
                                if (GotoNext(MapWalkXY.nX, MapWalkXY.nY, m_btRaceServer != 108))
                                {
                                    m_RunPos.btDirection = 0;
                                    result = true;
                                    return result;
                                }
                            }
                        }
                    }
                    for (var i = 2; i >= 1; i--)
                    {
                        if (ActThink_FindPosOfSelf(WalkStep, i, boFlag))
                        {
                            if (m_Master != null)
                            {
                                MapWalkXY = ActThink_FindGoodPathB(WalkStep, 1);
                            }
                            else
                            {
                                MapWalkXY = ActThink_FindGoodPathB(WalkStep, 0);
                            }
                            if ((MapWalkXY.nWalkStep > 0))
                            {
                                // if RunToTargetXY(MapWalkXY.nX, MapWalkXY.nY) then begin
                                if (GotoNext(MapWalkXY.nX, MapWalkXY.nY, m_btRaceServer != 108))
                                {
                                    m_RunPos.btDirection = 0;
                                    result = true;
                                    return result;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (wMagicID > 0)
                    {
                        nRange = HUtil32._MAX((new System.Random(3)).Next(), 2);
                    }
                    else
                    {
                        nRange = 1;
                    }
                    boFlag = (m_btRaceServer == 108) || (new ArrayList(new object[] { grobal2.SKILL_FIREBALL, grobal2.SKILL_FIREBALL2, grobal2.SKILL_FIRECHARM }).Contains(wMagicID)) || (nRange == 1);
                    for (var i = 2; i >= 1; i--)
                    {
                        if (ActThink_FindPosOfSelf(WalkStep, i, boFlag))
                        {
                            MapWalkXY = ActThink_FindGoodPathA(WalkStep, nRange, 0);
                            if ((MapWalkXY.nWalkStep > 0))
                            {
                                if (GotoNextOne(MapWalkXY.nX, MapWalkXY.nY, m_btRaceServer != 108))
                                {
                                    m_RunPos.btDirection = 0;
                                    result = true;
                                    return result;
                                }
                            }
                        }
                    }
                    for (var i = 2; i >= 1; i--)
                    {
                        if (ActThink__FindPosOfSelf(WalkStep, i, boFlag))
                        {
                            MapWalkXY = ActThink_FindMinRange(WalkStep);
                            if ((MapWalkXY.nWalkStep > 0))
                            {
                                if (GotoNextOne(MapWalkXY.nX, MapWalkXY.nY, m_btRaceServer != 108))
                                {
                                    m_RunPos.btDirection = 0;
                                    result = true;
                                    return result;
                                }
                            }
                        }
                    }
                    for (var i = nRange; i >= 1; i--)
                    {
                        if (ActThink_FindPosOfTarget(WalkStep, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, i, boFlag))
                        {
                            MapWalkXY = ActThink_FindGoodPathB(WalkStep, 0);
                            if ((MapWalkXY.nWalkStep > 0))
                            {
                                if (GotoNextOne(MapWalkXY.nX, MapWalkXY.nY, m_btRaceServer != 108))
                                {
                                    m_RunPos.btDirection = 0;
                                    result = true;
                                    return result;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                M2Share.MainOutMessage("WalkToRightPos:" + m_sCharName);
            }
            return result;
        }

        public bool ActThink_AvoidTarget(short wMagicID)
        {
            int nRange;
            short nX = 0;
            short nY = 0;
            bool boFlag;
            TMapWalkXY[] WalkStep = null;
            bool result = false;
            nRange = HUtil32._MAX((new System.Random(3)).Next(), 2);
            boFlag = (m_btRaceServer == 108) || (new ArrayList(new object[] { grobal2.SKILL_FIREBALL, grobal2.SKILL_FIREBALL2, grobal2.SKILL_FIRECHARM }).Contains(wMagicID));
            byte btDir;
            TMapWalkXY MapWalkXY;
            for (var i = nRange; i >= 1; i--)
            {
                if (ActThink_FindPosOfSelf(WalkStep, i, boFlag))
                {
                    MapWalkXY = ActThink_FindGoodPathB(WalkStep, 0);
                    if ((MapWalkXY.nWalkStep > 0))
                    {
                        btDir = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, MapWalkXY.nX, MapWalkXY.nY);
                        if (GotoNextOne(MapWalkXY.nX, MapWalkXY.nY, m_btRaceServer != 108))
                        {
                            if ((m_btRaceServer != 108))
                            {
                                for (var j = nRange; j >= 1; j--)
                                {
                                    // 再跑1次
                                    if (m_PEnvir.GetNextPosition(MapWalkXY.nX, MapWalkXY.nY, btDir, j, ref nX, ref nY) && m_PEnvir.CanWalkEx(nX, nY, true) && (GetNearTargetCount(nX, nY) <= MapWalkXY.nMonCount))
                                    {
                                        GotoNextOne(nX, nY, m_btRaceServer != 108);
                                        break;
                                    }
                                }
                            }
                            m_RunPos.btDirection = 0;
                            result = true;
                            return result;
                        }
                    }
                }
            }
            for (var i = nRange; i >= 1; i--)
            {
                if (ActThink__FindPosOfSelf(WalkStep, i, boFlag))
                {
                    MapWalkXY = ActThink_FindGoodPathB(WalkStep, 0);
                    if ((MapWalkXY.nWalkStep > 0))
                    {
                        btDir = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, MapWalkXY.nX, MapWalkXY.nY);
                        if (GotoNextOne(MapWalkXY.nX, MapWalkXY.nY, m_btRaceServer != 108))
                        {
                            for (var j = nRange; j >= 1; j--)
                            {
                                // 再跑1次
                                if (m_PEnvir.GetNextPosition(MapWalkXY.nX, MapWalkXY.nY, btDir, j, ref nX, ref nY) && m_PEnvir.CanWalkEx(nX, nY, true) && (GetNearTargetCount(nX, nY) <= MapWalkXY.nMonCount))
                                {
                                    MapWalkXY.nX = nX;
                                    MapWalkXY.nY = nY;
                                    GotoNextOne(MapWalkXY.nX, MapWalkXY.nY, m_btRaceServer != 108);
                                    break;
                                }
                            }
                            m_RunPos.btDirection = 0;
                            result = true;
                            return result;
                        }
                    }
                }
            }
            return result;
        }

        public bool ActThink_FollowTarget(short wMagicID)
        {
            int nRange = 2;
            TMapWalkXY[] WalkStep = null;
            TMapWalkXY MapWalkXY;
            bool result = false;
            bool boFlag = (m_btRaceServer == 108) || (new ArrayList(new object[] { grobal2.SKILL_FIREBALL, grobal2.SKILL_FIREBALL2, grobal2.SKILL_FIRECHARM }).Contains(wMagicID));
            for (var i = nRange; i >= 1; i--)
            {
                if (ActThink_FindPosOfSelf(WalkStep, i, boFlag))
                {
                    MapWalkXY = ActThink_FindMinRange(WalkStep);
                    if ((MapWalkXY.nWalkStep > 0))
                    {
                        if (GotoNextOne(MapWalkXY.nX, MapWalkXY.nY, m_btRaceServer != 108))
                        {
                            m_RunPos.btDirection = 0;
                            result = true;
                            return result;
                        }
                    }
                }
            }
            for (var i = nRange; i >= 1; i--)
            {
                if (ActThink__FindPosOfSelf(WalkStep, i, boFlag))
                {
                    MapWalkXY = ActThink_FindMinRange(WalkStep);
                    if ((MapWalkXY.nWalkStep > 0))
                    {
                        if (GotoNextOne(MapWalkXY.nX, MapWalkXY.nY, m_btRaceServer != 108))
                        {
                            m_RunPos.btDirection = 0;
                            result = true;
                            return result;
                        }
                    }
                }
            }
            return result;
        }

        public bool ActThink_MotaeboPos(short wMagicID)
        {
            bool result = false;
            short nTargetX = 0;
            short nTargetY = 0;
            byte btNewDir;
            if ((m_TargetCret == null) || (m_Master == null))
            {
                return result;
            }
            if ((GetPoseCreate() == m_TargetCret) || (m_TargetCret.GetPoseCreate() == this))
            {
                btNewDir = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
                if (m_PEnvir.GetNextPosition(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, btNewDir, 1, ref nTargetX, ref nTargetY))
                {
                    if (m_PEnvir.CanWalk(nTargetX, nTargetY, true))
                    {
                        result = true;
                        return result;
                    }
                }
            }
            result = ActThink_WalkToRightPos(wMagicID);
            return result;
        }

        public TMapWalkXY ActThink_FindPosOfDir(int nDir, int nRange, bool boFlag)
        {
            TMapWalkXY result = null;
            short nCurrX = 0;
            short nCurrY = 0;
            //FillChar(result, sizeof(TMapWalkXY), 0);
            if (m_PEnvir.GetNextPosition(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, nDir, nRange, ref nCurrX, ref nCurrY) && CanMove(nCurrX, nCurrY, false) && ((boFlag && CanLineAttack(nCurrX, nCurrY)) || !boFlag) && IsGotoXY(m_nCurrX, m_nCurrY, nCurrX, nCurrY))
            {
                result.nWalkStep = nRange;
                result.nX = nCurrX;
                result.nY = nCurrY;
                result.nMonRange = Math.Abs(nCurrX - m_TargetCret.m_nCurrX) + Math.Abs(nCurrY - m_TargetCret.m_nCurrY);
                result.nMonCount = GetNearTargetCount(nCurrX, nCurrY);
                result.nMastrRange = GetMasterRange(nCurrX, nCurrY);
            }
            return result;
        }

        public byte ActThink_RunPosAttack_GetNextRunPos(byte btDir, bool boTurn)
        {
            byte result = 0;
            if (boTurn)
            {
                switch (btDir)
                {
                    case grobal2.DR_UP:
                        result = grobal2.DR_RIGHT;
                        break;
                    case grobal2.DR_UPRIGHT:
                        result = grobal2.DR_DOWNRIGHT;
                        break;
                    case grobal2.DR_RIGHT:
                        result = grobal2.DR_DOWN;
                        break;
                    case grobal2.DR_DOWNRIGHT:
                        result = grobal2.DR_DOWNLEFT;
                        break;
                    case grobal2.DR_DOWN:
                        result = grobal2.DR_LEFT;
                        break;
                    case grobal2.DR_DOWNLEFT:
                        result = grobal2.DR_UPLEFT;
                        break;
                    case grobal2.DR_LEFT:
                        result = grobal2.DR_UP;
                        break;
                    case grobal2.DR_UPLEFT:
                        result = grobal2.DR_UPRIGHT;
                        break;
                }
            }
            else
            {
                switch (btDir)
                {
                    case grobal2.DR_UP:
                        result = grobal2.DR_LEFT;
                        break;
                    case grobal2.DR_UPRIGHT:
                        result = grobal2.DR_UPLEFT;
                        break;
                    case grobal2.DR_RIGHT:
                        result = grobal2.DR_UP;
                        break;
                    case grobal2.DR_DOWNRIGHT:
                        result = grobal2.DR_UPRIGHT;
                        break;
                    case grobal2.DR_DOWN:
                        result = grobal2.DR_RIGHT;
                        break;
                    case grobal2.DR_DOWNLEFT:
                        result = grobal2.DR_DOWNRIGHT;
                        break;
                    case grobal2.DR_LEFT:
                        result = grobal2.DR_DOWN;
                        break;
                    case grobal2.DR_UPLEFT:
                        result = grobal2.DR_DOWNLEFT;
                        break;
                }
            }
            return result;
        }

        public bool ActThink_RunPosAttack(int wMagicID)
        {
            TMapWalkXY[] WalkStep = new TMapWalkXY[1 + 1];
            TMapWalkXY MapWalkXY;
            byte btNewDir1;
            byte btNewDir2;
            int nRange;
            bool boFlag;
            int nNearTargetCount;
            bool result = false;
            byte btDir = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
            btNewDir1 = ActThink_RunPosAttack_GetNextRunPos(btDir, true);
            btNewDir2 = ActThink_RunPosAttack_GetNextRunPos(btDir, false);
            //FillChar(WalkStep, sizeof(TMapWalkXY) * 2, 0);
            if (m_btJob == 0)
            {
                nRange = 1;
                if ((wMagicID == 43))
                {
                    nRange = 2;
                }
                if ((wMagicID == 12))
                {
                    nRange = 2;
                }
                if ((new ArrayList(new int[] { 60, 61, 62 }).Contains(wMagicID)))
                {
                    nRange = 6;
                }
                WalkStep[0] = ActThink_FindPosOfDir(btNewDir1, nRange, true);
                WalkStep[1] = ActThink_FindPosOfDir(btNewDir2, nRange, true);
            }
            else
            {
                nRange = 2;
                boFlag = false;
                WalkStep[0] = ActThink_FindPosOfDir(btNewDir1, nRange, boFlag);
                WalkStep[1] = ActThink_FindPosOfDir(btNewDir2, nRange, boFlag);
            }
            nNearTargetCount = GetNearTargetCount(m_nCurrX, m_nCurrY);
            MapWalkXY = null;
            if ((WalkStep[0].nWalkStep > 0) && (WalkStep[1].nWalkStep > 0))
            {
                if (m_RunPos.btDirection > 0)
                {
                    MapWalkXY = WalkStep[1];
                }
                else
                {
                    MapWalkXY = WalkStep[0];
                }
                if ((nNearTargetCount < WalkStep[0].nMonCount) && (nNearTargetCount < WalkStep[1].nMonCount))
                {
                    MapWalkXY = null;
                }
                else if ((m_RunPos.btDirection > 0) && (nNearTargetCount < WalkStep[1].nMonCount))
                {
                    MapWalkXY = null;
                }
                else if ((m_RunPos.btDirection <= 0) && (nNearTargetCount < WalkStep[0].nMonCount))
                {
                    MapWalkXY = null;
                }
                if ((nNearTargetCount > 0) && (MapWalkXY != null) && (MapWalkXY.nMonCount > nNearTargetCount))
                {
                    MapWalkXY = null;
                }
            }
            else if ((WalkStep[0].nWalkStep > 0))
            {
                MapWalkXY = WalkStep[0];
                if ((nNearTargetCount < WalkStep[0].nMonCount))
                {
                    MapWalkXY = null;
                }
                m_RunPos.btDirection = 0;
            }
            else if ((WalkStep[1].nWalkStep > 0))
            {
                MapWalkXY = WalkStep[1];
                if ((nNearTargetCount < WalkStep[1].nMonCount))
                {
                    MapWalkXY = null;
                }
                m_RunPos.btDirection = 1;
            }
            if ((MapWalkXY != null))
            {
                if (GotoNextOne(MapWalkXY.nX, MapWalkXY.nY, m_btRaceServer != 108))
                {
                    result = true;
                }
            }
            if (!result)
            {
                m_RunPos.nAttackCount = 0;
            }
            return result;
        }

        private bool ActThink(short wMagicID)
        {
            bool result = false;
            int nCode = 0;
            int nError = 0;
            int nThinkCount = 0;
            try
            {
                while (true)
                {
                    if ((m_TargetCret == null) || (wMagicID > 255))
                    {
                        break;
                    }
                    nThinkCount = nThinkCount + 1;
                    nCode = DoThink(wMagicID);
                    nError = 1;
                    switch (m_btJob)
                    {
                        case 0:
                            switch (nCode)
                            {
                                case 2:
                                    nError = 2;
                                    if (ActThink_WalkToRightPos(wMagicID))
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        // 无法走到正确的攻击坐标
                                        nError = 3;
                                        DelTargetCreat();
                                        if (nThinkCount < 2)
                                        {
                                            nError = 4;
                                            SearchTarget();
                                            nError = 5;
                                            continue;
                                        }
                                    }
                                    break;
                                case 5:
                                    nError = 6;
                                    if (ActThink_RunPosAttack(wMagicID))
                                    {
                                        result = true;
                                    }
                                    nError = 7;
                                    break;
                            }
                            break;
                        case 1:
                        case 2:
                            switch (nCode)
                            {
                                case 1:
                                    nError = 8;
                                    result = ActThink_AvoidTarget(wMagicID);
                                    nError = 9;
                                    break;
                                case 2:
                                    nError = 10;
                                    if (ActThink_FollowTarget(wMagicID))
                                    {
                                        nError = 11;
                                        result = true;
                                    }
                                    else
                                    {
                                        // 无法走到正确的攻击坐标
                                        nError = 12;
                                        DelTargetCreat();
                                        nError = 13;
                                        if (nThinkCount < 2)
                                        {
                                            nError = 14;
                                            SearchTarget();
                                            nError = 15;
                                            continue;
                                        }
                                    }
                                    break;
                                case 3:
                                case 4:
                                    nError = 16;
                                    if (ActThink_WalkToRightPos(wMagicID))
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        // 无法走到正确的攻击坐标
                                        nError = 3;
                                        DelTargetCreat();
                                        if (nThinkCount < 2)
                                        {
                                            nError = 4;
                                            SearchTarget();
                                            nError = 5;
                                            continue;
                                        }
                                    }
                                    nError = 17;
                                    break;
                                case 5:
                                    nError = 24;
                                    result = ActThink_RunPosAttack(wMagicID);
                                    nError = 25;
                                    break;
                            }
                            break;
                    }
                    break;
                }
            }
            catch
            {
                M2Share.MainOutMessage(format("TAIPlayObject::ActThink Name:%s Code:%d ", new object[] { m_sCharName, nCode }));
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
                if ((m_Master != null) && (m_Master.m_boGhost))
                {
                    return result;
                }
                if ((m_Master != null) && m_Master.InSafeZone() && InSafeZone())
                {
                    if ((Math.Abs(m_nCurrX - m_Master.m_nCurrX) <= 3) && (Math.Abs(m_nCurrY - m_Master.m_nCurrY) <= 3))
                    {
                        result = true;
                        return result;
                    }
                }
                if ((HUtil32.GetTickCount() - m_dwThinkTick) > 3000)
                {
                    m_dwThinkTick = HUtil32.GetTickCount();
                    if (m_PEnvir.GetXYObjCount(m_nCurrX, m_nCurrY) >= 2)
                    {
                        m_boDupMode = true;
                    }
                    if ((m_TargetCret != null))
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
                    WalkTo((byte)(new System.Random(8)).Next(), false);
                    //m_dwStationTick = HUtil32.GetTickCount(); // 增加检测人物站立时间
                    if ((nOldX != m_nCurrX) || (nOldY != m_nCurrY))
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

        public override void Run()
        {
            int nSelectMagic;
            int nWhere;
            int nPercent;
            int nValue;
            TItem StdItem;
            TUserItem UserItem;
            bool boRecalcAbilitys;
            bool boFind = false;
            try
            {
                if (!m_boGhost && !m_boDeath && !m_boFixedHideMode && !m_boStoneMode && (m_wStatusTimeArr[grobal2.POISON_STONE] == 0))
                {
                    if (HUtil32.GetTickCount() - m_dwWalkTick > m_nWalkSpeed)
                    {
                        m_dwWalkTick = HUtil32.GetTickCount();
                        if ((m_TargetCret != null))
                        {
                            if (m_TargetCret.m_boDeath || m_TargetCret.m_boGhost || m_TargetCret.InSafeZone() || (m_TargetCret.m_PEnvir != m_PEnvir) || (Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) > 11) || (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) > 11))
                            {
                                DelTargetCreat();
                            }
                        }
                        if (!m_boAIStart)
                        {
                            DelTargetCreat();
                        }
                        SearchTarget();
                        if (m_ManagedEnvir != m_PEnvir) // 所在地图不是挂机地图则清空目标
                        {
                            DelTargetCreat();
                        }
                        if (Thinking())
                        {
                            base.Run();
                            return;
                        }
                        if (m_boProtectStatus) // 守护状态
                        {
                            if ((m_nProtectTargetX == 0) || (m_nProtectTargetY == 0))
                            {
                                // 取守护坐标
                                m_nProtectTargetX = m_nCurrX;// 守护坐标
                                m_nProtectTargetY = m_nCurrY;// 守护坐标
                            }
                            if (!m_boProtectOK && (m_ManagedEnvir != null) && (m_TargetCret == null))
                            {
                                GotoProtect();
                                m_nGotoProtectXYCount++;
                                if ((Math.Abs(m_nCurrX - m_nProtectTargetX) <= 3) && (Math.Abs(m_nCurrY - m_nProtectTargetY) <= 3))
                                {
                                    m_btDirection = (byte)(new System.Random(8)).Next();
                                    m_boProtectOK = true;
                                    m_nGotoProtectXYCount = 0;// 是向守护坐标的累计数
                                }
                                if ((m_nGotoProtectXYCount > 20) && !m_boProtectOK)
                                {
                                    // 20次还没有走到守护坐标，则飞回坐标上
                                    if ((Math.Abs(m_nCurrX - m_nProtectTargetX) > 13) || (Math.Abs(m_nCurrY - m_nProtectTargetY) > 13))
                                    {
                                        SpaceMove(m_ManagedEnvir.sMapName, m_nProtectTargetX, m_nProtectTargetY, 1);
                                        m_btDirection = (byte)(new System.Random(8)).Next();
                                        m_boProtectOK = true;
                                        m_nGotoProtectXYCount = 0;// 是向守护坐标的累计数
                                    }
                                }
                                base.Run();
                                return;
                            }
                        }
                        if (m_TargetCret != null)
                        {
                            if (AttackTarget())// 攻击
                            {
                                base.Run();
                                return;
                            }
                            else if (IsNeedAvoid()) // 自动躲避
                            {
                                m_dwActionTick = HUtil32.GetTickCount() - 10;
                                AutoAvoid();
                                base.Run();
                                return;
                            }
                            else
                            {
                                if (IsNeedGotoXY())// 是否走向目标
                                {
                                    m_dwActionTick = HUtil32.GetTickCount();
                                    m_nTargetX = m_TargetCret.m_nCurrX;
                                    m_nTargetY = m_TargetCret.m_nCurrY;
                                    if (AllowUseMagic(12) && (m_btJob == 0))
                                    {
                                        GetGotoXY(m_TargetCret, 2);
                                    }
                                    if ((m_btJob > 0))
                                    {
                                        if ((M2Share.g_Config.boHeroAttackTarget && (m_Abil.Level < 22)) || (M2Share.g_Config.boHeroAttackTao && (m_TargetCret.m_WAbil.MaxHP < 700) && (m_btJob == 2) && (m_TargetCret.m_btRaceServer != grobal2.RC_PLAYOBJECT)))
                                        {
                                            // 道法22前是否物理攻击
                                            if (m_Master != null)
                                            {
                                                if ((Math.Abs(m_Master.m_nCurrX - m_nCurrX) > 6) || (Math.Abs(m_Master.m_nCurrY - m_nCurrY) > 6))
                                                {
                                                    base.Run();
                                                    return;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            GetGotoXY(m_TargetCret, 3); // 道法只走向目标3格范围
                                        }
                                    }
                                    GotoTargetXY(m_nTargetX, m_nTargetY, 0);
                                    base.Run();
                                    return;
                                }
                            }
                        }

                        if (m_boAI && !m_boGhost && !m_boDeath)
                        {
                            if (M2Share.g_Config.boHPAutoMoveMap)
                            {
                                if ((m_WAbil.HP <= Math.Round(m_WAbil.MaxHP * 0.3)) && (HUtil32.GetTickCount() - m_dwHPToMapHomeTick > 15000))
                                {
                                    // 低血时回城或回守护点 20110512
                                    m_dwHPToMapHomeTick = HUtil32.GetTickCount();
                                    DelTargetCreat();
                                    if (m_boProtectStatus) // 守护状态
                                    {
                                        SpaceMove(m_ManagedEnvir.sMapName, m_nProtectTargetX, m_nProtectTargetY, 1);// 地图移动
                                        m_btDirection = (byte)(new System.Random(8)).Next();
                                        m_boProtectOK = true;
                                        m_nGotoProtectXYCount = 0; // 是向守护坐标的累计数 20090203
                                    }
                                    else
                                    {
                                        // 不是守护状态，直接回城
                                        MoveToHome(); // 移动到回城点
                                    }
                                }
                            }
                            if (M2Share.g_Config.boAutoRepairItem)
                            {
                                if (HUtil32.GetTickCount() - m_dwAutoRepairItemTick > 15000)
                                {
                                    m_dwAutoRepairItemTick = HUtil32.GetTickCount();
                                    boRecalcAbilitys = false;
                                    for (nWhere = m_UseItemNames.GetLowerBound(0); nWhere <= m_UseItemNames.GetUpperBound(0); nWhere++)
                                    {
                                        if ((m_UseItemNames[nWhere] != "") && (m_UseItems[nWhere].wIndex <= 0))
                                        {
                                            StdItem = M2Share.UserEngine.GetStdItem(m_UseItemNames[nWhere]);
                                            if (StdItem != null)
                                            {
                                                UserItem = new TUserItem();
                                                if (M2Share.UserEngine.CopyToUserItemFromName(m_UseItemNames[nWhere], ref UserItem))
                                                {
                                                    boRecalcAbilitys = true;
                                                    if (new ArrayList(new int[] { 15, 19, 20, 21, 22, 23, 24, 26 }).Contains(StdItem.StdMode))
                                                    {
                                                        if ((StdItem.Shape == 130) || (StdItem.Shape == 131) || (StdItem.Shape == 132))
                                                        {
                                                            //M2Share.UserEngine.GetUnknowItemValue(UserItem);
                                                        }
                                                    }
                                                }
                                                m_UseItems[nWhere] = UserItem;
                                                Dispose(UserItem);
                                            }
                                        }
                                    }
                                    if (m_BagItemNames.Count > 0)
                                    {
                                        for (var i = 0; i < m_BagItemNames.Count; i++)
                                        {
                                            for (var j = 0; j < m_ItemList.Count; j++)
                                            {
                                                UserItem = m_ItemList[j];
                                                if (UserItem != null)
                                                {
                                                    StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                                                    if (StdItem != null)
                                                    {
                                                        boFind = false;
                                                        if ((StdItem.Name).ToLower().CompareTo((m_BagItemNames[i]).ToLower()) == 0)
                                                        {
                                                            boFind = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }

                                            if (!boFind)
                                            {
                                                UserItem = new TUserItem();
                                                if (M2Share.UserEngine.CopyToUserItemFromName(m_BagItemNames[i], ref UserItem))
                                                {
                                                    if (!AddItemToBag(UserItem))
                                                    {
                                                        Dispose(UserItem);
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    Dispose(UserItem);
                                                }
                                            }
                                        }
                                    }

                                    for (nWhere = m_UseItems.GetLowerBound(0); nWhere <= m_UseItems.GetUpperBound(0); nWhere++)
                                    {
                                        if (m_UseItems[nWhere] != null && m_UseItems[nWhere].wIndex > 0)
                                        {
                                            StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[nWhere].wIndex);
                                            if (StdItem != null)
                                            {
                                                if ((m_UseItems[nWhere].DuraMax > m_UseItems[nWhere].Dura) && (StdItem.StdMode != 43))
                                                {
                                                    /*if (PlugOfCheckCanItem(3, StdItem.Name, false, 0, 0))
                                                    {
                                                        continue;
                                                    }*/
                                                    m_UseItems[nWhere].Dura = m_UseItems[nWhere].DuraMax;
                                                }
                                            }
                                        }
                                    }
                                    if (boRecalcAbilitys)
                                    {
                                        RecalcAbilitys();
                                    }
                                }
                            }
                            if (M2Share.g_Config.boRenewHealth)
                            {
                                // 自动增加HP MP
                                if (HUtil32.GetTickCount() - m_dwAutoAddHealthTick > 5000)
                                {
                                    m_dwAutoAddHealthTick = HUtil32.GetTickCount();
                                    nPercent = m_WAbil.HP * 100 / m_WAbil.MaxHP;
                                    nValue = m_WAbil.MaxHP / 10;
                                    if (nPercent < M2Share.g_Config.nRenewPercent)
                                    {
                                        if (m_WAbil.HP + nValue >= m_WAbil.MaxHP)
                                        {
                                            m_WAbil.HP = m_WAbil.MaxHP;
                                        }
                                        else
                                        {
                                            m_WAbil.HP += (ushort)nValue;
                                        }
                                    }
                                    nValue = m_WAbil.MaxMP / 10;
                                    nPercent = m_WAbil.MP * 100 / m_WAbil.MaxMP;
                                    if (nPercent < M2Share.g_Config.nRenewPercent)
                                    {
                                        if (m_WAbil.MP + nValue >= m_WAbil.MaxMP)
                                        {
                                            m_WAbil.MP = m_WAbil.MaxMP;
                                        }
                                        else
                                        {
                                            m_WAbil.MP += (ushort)nValue;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (!m_boGhost && !m_boDeath && !m_boFixedHideMode && !m_boStoneMode && (m_wStatusTimeArr[grobal2.POISON_STONE] == 0))
                    {
                        if (m_boProtectStatus && (m_TargetCret == null))
                        {
                            // 守护状态
                            if ((Math.Abs(m_nCurrX - m_nProtectTargetX) > 50) || (Math.Abs(m_nCurrY - m_nProtectTargetY) > 50))
                            {
                                m_boProtectOK = false;
                            }
                        }
                        if ((m_TargetCret == null))
                        {
                            if ((m_Master != null))
                            {
                                FollowMaster();
                            }
                            else
                            {
                                Wondering();
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                // M2Share.MainOutMessage(format("{%s} TAIPlayObject.Run Code:%d", new byte[] { nCode }));
            }
            base.Run();
        }

        public override bool IsProtectTarget(TBaseObject BaseObject)
        {
            return base.IsProtectTarget(BaseObject);
        }

        public override bool IsAttackTarget(TBaseObject BaseObject)
        {
            return base.IsAttackTarget(BaseObject);
        }
        
        public override bool IsProperTarget(TBaseObject BaseObject)
        {
            bool result = false;
            if (BaseObject != null)
            {
                if (base.IsProperTarget(BaseObject))
                {
                    result = true;
                    if (BaseObject.m_Master != null)
                    {
                        if ((BaseObject.m_Master == this) || ((BaseObject.m_Master.m_boAI) && !m_boInFreePKArea))
                        {
                            result = false;
                        }
                    }
                    if (BaseObject.m_boAI && !m_boInFreePKArea)
                    {
                        result = false;
                    }
                    // 假人不攻击假人,行会战除外
                    switch (BaseObject.m_btRaceServer)
                    {
                        case grobal2.RC_ARCHERGUARD:
                        case 55:// 不主动攻击练功师 弓箭手
                            if (BaseObject.m_TargetCret != this)
                            {
                                result = false;
                            }
                            break;
                        case 10:
                        case 11:
                        case 12: // 不攻击大刀卫士
                            result = false;
                            break;
                        case 110:
                        case 111:
                        case 158: // 沙巴克城门,沙巴克左城墙,宠物类
                            result = false;
                            break;
                    }
                }
                else
                {
                    if (m_btAttatckMode == M2Share.HAM_PKATTACK)
                    {
                        // 红名模式，除红名目标外，受人攻击时才还击
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
                        if (m_boAI && !result)
                        {
                            if ((BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT) || (BaseObject.m_Master != null))
                            {
                                if (BaseObject.m_TargetCret != null)
                                {
                                    if (BaseObject.m_TargetCret == this)
                                    {
                                        result = true;
                                    }
                                }
                                if (BaseObject.m_LastHiter != null)
                                {
                                    if (BaseObject.m_LastHiter == this)
                                    {
                                        result = true;
                                    }
                                }
                                if (BaseObject.m_ExpHitter != null)
                                {
                                    if (BaseObject.m_LastHiter == this)
                                    {
                                        result = true;
                                    }
                                }
                            }
                        }
                        if ((BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT) || (BaseObject.m_Master != null))
                        {
                            // 安全区不能打人物和英雄
                            if (BaseObject.InSafeZone() || InSafeZone())
                            {
                                result = false;
                            }
                        }
                        if ((BaseObject.m_Master == this))
                        {
                            result = false;
                        }
                        if (BaseObject.m_boAI && (!m_boInFreePKArea || (BaseObject.PKLevel() < 2)))
                        {
                            result = false;
                        }
                        // 假人不攻击假人,行会战除外
                        switch (BaseObject.m_btRaceServer)
                        {
                            case grobal2.RC_ARCHERGUARD:
                            case 55:// 不主动攻击练功师 弓箭手
                                if (BaseObject.m_TargetCret != this)
                                {
                                    result = false;
                                }
                                break;
                            case 10:
                            case 11:
                            case 12:// 不攻击大刀卫士
                                result = false;
                                break;
                            case 110:
                            case 111:
                            case 158:// 沙巴克城门,沙巴克左城墙,宠物类
                                result = false;
                                break;
                        }
                    }
                }
            }
            return result;
        }

        public override bool IsProperFriend(TBaseObject BaseObject)
        {
            return base.IsProperFriend(BaseObject);
        }

        public override void SearchViewRange()
        {
            int I;
            int nStartX;
            int nEndX;
            int nStartY;
            int nEndY;
            int n18;
            int n1C;
            int nIdx;
            int n25;
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            TBaseObject BaseObject;
            TMapItem MapItem;
            TEvent MapEvent;
            TVisibleBaseObject VisibleBaseObject;
            TVisibleMapItem VisibleMapItem;
            int nVisibleFlag;
            long dwRunTick;
            const string sExceptionMsg1 = "{%s} TAIPlayObject::SearchViewRange Code:%d";
            try
            {
                if (m_boGhost)
                {
                    return;
                }
                if (m_VisibleItems.Count > 0)
                {
                    for (I = 0; I < m_VisibleItems.Count; I++)
                    {
                        m_VisibleItems[I].nVisibleFlag = 0;
                    }
                }
            }
            catch
            {
                M2Share.MainOutMessage(sExceptionMsg1);
                KickException();
            }
            try
            {
                nStartX = m_nCurrX - m_nViewRange;
                nEndX = m_nCurrX + m_nViewRange;
                nStartY = m_nCurrY - m_nViewRange;
                nEndY = m_nCurrY + m_nViewRange;
                dwRunTick = HUtil32.GetTickCount();
                for (n18 = nStartX; n18 <= nEndX; n18++)
                {
                    for (n1C = nStartY; n1C <= nEndY; n1C++)
                    {
                        if (m_PEnvir.GetMapCellInfo(n18, n1C, ref MapCellInfo) && (MapCellInfo.ObjList != null))
                        {
                            nIdx = 0;
                            while (true)
                            {
                                if ((HUtil32.GetTickCount() - dwRunTick) > 500)
                                {
                                    break;
                                }
                                if (MapCellInfo != null)
                                {
                                    if ((MapCellInfo.ObjList != null) && (MapCellInfo.ObjList.Count <= 0))
                                    {
                                        MapCellInfo.ObjList = null;
                                        break;
                                    }
                                }
                                if (MapCellInfo.ObjList.Count <= nIdx)
                                {
                                    break;
                                }
                                try
                                {
                                    OSObject = MapCellInfo.ObjList[nIdx];
                                }
                                catch
                                {
                                    MapCellInfo.ObjList.RemoveAt(nIdx);
                                    continue;
                                }
                                if (OSObject != null)
                                {
                                    if ((!OSObject.boObjectDisPose))
                                    {
                                        switch (OSObject.btType)
                                        {
                                            case grobal2.OS_MOVINGOBJECT:
                                                if ((HUtil32.GetTickCount() - OSObject.dwAddTime) >= 60000)
                                                {
                                                    OSObject.boObjectDisPose = true;
                                                    Dispose(OSObject);
                                                    MapCellInfo.ObjList.RemoveAt(nIdx);
                                                    if (MapCellInfo.ObjList.Count <= 0)
                                                    {
                                                        MapCellInfo.ObjList = null;
                                                        break;
                                                    }
                                                    continue;
                                                }
                                                BaseObject = (TBaseObject)OSObject.CellObj;
                                                if (BaseObject != null)
                                                {
                                                    if (!BaseObject.m_boGhost && !BaseObject.m_boFixedHideMode && !BaseObject.m_boObMode)
                                                    {
                                                        if ((m_btRaceServer < grobal2.RC_ANIMAL) || (m_Master != null) || m_boCrazyMode || m_boWantRefMsg || ((BaseObject.m_Master != null) && (Math.Abs(BaseObject.m_nCurrX - m_nCurrX) <= 3) && (Math.Abs(BaseObject.m_nCurrY - m_nCurrY) <= 3)) || (BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT))
                                                        {
                                                            UpdateVisibleGay(BaseObject);
                                                        }
                                                    }
                                                }
                                                break;
                                            case grobal2.OS_ITEMOBJECT:
                                                if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                                                {
                                                    if (((HUtil32.GetTickCount() - OSObject.dwAddTime) > M2Share.g_Config.dwClearDropOnFloorItemTime))
                                                    {
                                                        if (((TMapItem)(OSObject.CellObj)) != null)
                                                        {
                                                            Dispose(((TMapItem)(OSObject.CellObj)));
                                                        }
                                                        if (OSObject != null)
                                                        {
                                                            OSObject.boObjectDisPose = true;
                                                            Dispose(OSObject);
                                                        }
                                                        MapCellInfo.ObjList.RemoveAt(nIdx);
                                                        if (MapCellInfo.ObjList.Count <= 0)
                                                        {
                                                            MapCellInfo.ObjList = null;
                                                            break;
                                                        }
                                                        continue;
                                                    }
                                                    MapItem = ((TMapItem)(OSObject.CellObj));
                                                    UpdateVisibleItem(n18, n1C, MapItem);
                                                    if ((MapItem.OfBaseObject != null) || (MapItem.DropBaseObject != null))
                                                    {
                                                        if ((HUtil32.GetTickCount() - MapItem.dwCanPickUpTick) > M2Share.g_Config.dwFloorItemCanPickUpTime)
                                                        {
                                                            MapItem.OfBaseObject = null;
                                                            MapItem.DropBaseObject = null;
                                                        }
                                                        else
                                                        {
                                                            if (MapItem.OfBaseObject != null)
                                                            {
                                                                if (((TBaseObject)MapItem.OfBaseObject).m_boGhost)
                                                                {
                                                                    MapItem.OfBaseObject = null;
                                                                }
                                                            }
                                                            if (MapItem.DropBaseObject != null)
                                                            {
                                                                if (((TBaseObject)MapItem.DropBaseObject).m_boGhost)
                                                                {
                                                                    MapItem.DropBaseObject = null;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                break;
                                            case grobal2.OS_EVENTOBJECT:
                                                if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                                                {
                                                    if (OSObject.CellObj != null)
                                                    {
                                                        MapEvent = ((TEvent)(OSObject.CellObj));
                                                        UpdateVisibleEvent(n18, n1C, MapEvent);
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                                nIdx++;
                            }
                        }
                    }
                }
            }
            catch (Exception E)
            {
                // M2Share.MainOutMessage(format(sExceptionMsg2, new byte[] { n24, m_sCharName, m_sMapName, m_nCurrX, m_nCurrY, nCheckCode }));
                KickException();
            }
            try
            {
                n18 = 0;
                while (true)
                {
                    try
                    {
                        if (m_VisibleActors.Count <= n18)
                        {
                            break;
                        }
                        try
                        {
                            VisibleBaseObject = m_VisibleActors[n18];
                            nVisibleFlag = VisibleBaseObject.nVisibleFlag;
                        }
                        catch
                        {
                            m_VisibleActors.RemoveAt(n18);
                            if (m_VisibleActors.Count > 0)
                            {
                                continue;
                            }
                            break;
                        }
                        switch (VisibleBaseObject.nVisibleFlag)
                        {
                            case 0:
                                if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                                {
                                    BaseObject = VisibleBaseObject.BaseObject;
                                    if (BaseObject != null)
                                    {
                                        if ((!BaseObject.m_boFixedHideMode) && (!BaseObject.m_boGhost))
                                        {
                                            SendMsg(BaseObject, grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
                                        }
                                    }
                                }
                                m_VisibleActors.RemoveAt(n18);
                                if (VisibleBaseObject != null)
                                {
                                    Dispose(VisibleBaseObject);
                                }
                                continue;
                            case 2:
                                if ((m_btRaceServer == grobal2.RC_PLAYOBJECT))
                                {
                                    BaseObject = VisibleBaseObject.BaseObject;
                                    if ((BaseObject != null))
                                    {
                                        if ((BaseObject != this) && (!BaseObject.m_boGhost) && !m_boGhost)
                                        {
                                            if (BaseObject.m_boDeath)
                                            {
                                                if (BaseObject.m_boSkeleton)
                                                {
                                                    SendMsg(BaseObject, grobal2.RM_SKELETON, BaseObject.m_btDirection, BaseObject.m_nCurrX, BaseObject.m_nCurrY, 0, "");
                                                }
                                                else
                                                {
                                                    SendMsg(BaseObject, grobal2.RM_DEATH, BaseObject.m_btDirection, BaseObject.m_nCurrX, BaseObject.m_nCurrY, 0, "");
                                                }
                                            }
                                            else
                                            {
                                                if (BaseObject != null)
                                                {
                                                    SendMsg(BaseObject, grobal2.RM_TURN, BaseObject.m_btDirection, BaseObject.m_nCurrX, BaseObject.m_nCurrY, 0, BaseObject.GetShowName());
                                                }
                                            }
                                        }
                                    }
                                }
                                VisibleBaseObject.nVisibleFlag = 0;
                                break;
                            case 1:
                                VisibleBaseObject.nVisibleFlag = 0;
                                break;
                        }
                    }
                    catch
                    {
                        break;
                    }
                    n18++;
                }
            }
            catch (Exception E)
            {
                // M2Share.MainOutMessage(format(sExceptionMsg2, new byte[] { n24, m_sCharName, m_sMapName, m_nCurrX, m_nCurrY, nCheckCode }));
                KickException();
            }
            try
            {
                I = 0;
                while (true)
                {
                    try
                    {
                        if (m_VisibleItems.Count <= I)
                        {
                            break;
                        }
                        try
                        {
                            VisibleMapItem = m_VisibleItems[I];
                            nVisibleFlag = VisibleMapItem.nVisibleFlag;
                        }
                        catch
                        {
                            m_VisibleItems.RemoveAt(I);
                            if (m_VisibleItems.Count > 0)
                            {
                                continue;
                            }
                            break;
                        }
                        if (VisibleMapItem.nVisibleFlag == 0)
                        {
                            m_VisibleItems.RemoveAt(I);
                            VisibleMapItem = null;
                            if (m_VisibleItems.Count > 0)
                            {
                                continue;
                            }
                            break;
                        }
                    }
                    catch
                    {
                        break;
                    }
                    I++;
                }
                I = 0;
                while (true)
                {
                    try
                    {
                        if (m_VisibleEvents.Count <= I)
                        {
                            break;
                        }
                        try
                        {
                            MapEvent = m_VisibleEvents[I];
                            nVisibleFlag = MapEvent.nVisibleFlag;
                        }
                        catch
                        {
                            m_VisibleEvents.RemoveAt(I);
                            if (m_VisibleEvents.Count > 0)
                            {
                                continue;
                            }
                            break;
                        }
                        if (MapEvent != null)
                        {
                            switch (MapEvent.nVisibleFlag)
                            {
                                case 0:
                                    SendMsg(this, grobal2.RM_HIDEEVENT, 0, MapEvent.Id, MapEvent.m_nX, MapEvent.m_nY, "");
                                    m_VisibleEvents.RemoveAt(I);
                                    if (m_VisibleEvents.Count > 0)
                                    {
                                        continue;
                                    }
                                    break;
                                case 1:
                                    MapEvent.nVisibleFlag = 0;
                                    break;
                                case 2:
                                    SendMsg(this, grobal2.RM_SHOWEVENT, MapEvent.m_nEventType, MapEvent.Id, HUtil32.MakeLong(MapEvent.m_nX, MapEvent.m_nEventParam), MapEvent.m_nY, "");
                                    MapEvent.nVisibleFlag = 0;
                                    break;
                            }
                        }
                    }
                    catch
                    {
                        break;
                    }
                    I++;
                }
            }
            catch
            {
                //M2Share.MainOutMessage(m_sCharName + ',' + m_sMapName + ',' + (m_nCurrX).ToString() + ',' + (m_nCurrY).ToString() + ',' + " SearchViewRange 3 CheckCode:" + (nCheckCode).ToString());
                KickException();
            }
        }

        /// <summary>
        /// 物理攻击
        /// </summary>
        /// <param name="wHitMode"></param>
        /// <returns></returns>
        private bool WarrAttackTarget1(short wHitMode)
        {
            bool result = false;
            byte bt06 = 0;
            bool boHit;
            try
            {
                if (m_TargetCret != null)
                {
                    boHit = GetAttackDir(m_TargetCret, ref bt06);
                    if (!boHit && ((wHitMode == 4) || (wHitMode == 15)))
                    {
                        boHit = GetAttackDir(m_TargetCret, 2, ref bt06);// 防止隔位刺杀无效果
                    }
                    if (boHit)
                    {
                        m_dwTargetFocusTick = HUtil32.GetTickCount();
                        AttackDir(m_TargetCret, wHitMode, bt06);
                        m_dwActionTick = HUtil32.GetTickCount();
                        BreakHolySeizeMode();
                        result = true;
                    }
                    else
                    {
                        if (m_TargetCret.m_PEnvir == m_PEnvir)
                        {
                            SetTargetXY(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
                        }
                        else
                        {
                            DelTargetCreat();
                        }
                    }
                }
            }
            catch
            {
                M2Share.MainOutMessage("TAIPlayObject.WarrAttackTarget");
            }
            return result;
        }

        /// <summary>
        /// 战士攻击
        /// </summary>
        /// <returns></returns>
        private bool WarrorAttackTarget1()
        {
            TUserMagic UserMagic;
            bool result = false;
            try
            {
                m_wHitMode = 0;
                if (m_WAbil.MP > 0)
                {
                    if (m_TargetCret != null)
                    {
                        if (((m_WAbil.HP <= Math.Round(m_WAbil.MaxHP * 0.25)) || m_TargetCret.m_boCrazyMode))
                        {
                            // 注释,战不躲避
                            if (AllowUseMagic(12))
                            {
                                // 血少时或目标疯狂模式时，做隔位刺杀 
                                if (!(((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 2) && (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 0)) || ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 1) && (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 0)) || ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 1) && (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 1)) || ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 2) && (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 2)) || ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 0) && (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 1)) || ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 0) && (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 2))))
                                {
                                    GetGotoXY(m_TargetCret, 2);
                                    GotoTargetXY(m_nTargetX, m_nTargetY, 0);
                                }
                            }
                        }
                    }
                    SearchMagic();
                    if (m_nSelectMagic > 0)
                    {
                        UserMagic = FindMagic(m_nSelectMagic);
                        if ((UserMagic != null))
                        {
                            if ((UserMagic.btKey == 0))
                            {
                                switch (m_nSelectMagic)
                                {
                                    // 技能打开状态才能使用
                                    // Modify the A .. B: 27, 39, 41, 60 .. 65, 68, 75, SKILL_101, SKILL_102
                                    case 27:
                                    case 39:
                                    case 41:
                                    case 60:
                                    case 68:
                                    case 75:
                                        if (m_TargetCret != null)
                                        {
                                            result = UseSpell(UserMagic, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, m_TargetCret); // 战士魔法
                                            m_dwHitTick = HUtil32.GetTickCount();
                                            return result;
                                        }
                                        break;
                                    case 7:
                                        m_wHitMode = 3;
                                        break;
                                    case 12:
                                        m_wHitMode = 4;
                                        break;
                                    case 25: // 四级刺杀
                                        m_wHitMode = 5;
                                        break;
                                    case 26: // 圆月弯刀(四级半月弯刀)
                                        if (UseSpell(UserMagic, m_nCurrX, m_nCurrY, m_TargetCret))
                                        {
                                            m_wHitMode = 7;
                                        }
                                        break;
                                    case 40: // 使用烈火
                                        m_wHitMode = 8;
                                        break;
                                    case 43: // 抱月刀法
                                        if (UseSpell(UserMagic, m_nCurrX, m_nCurrY, m_TargetCret))
                                        {
                                            m_wHitMode = 9;
                                        }
                                        break;
                                    case 42: // 开天斩
                                        if (UseSpell(UserMagic, m_nCurrX, m_nCurrY, m_TargetCret))
                                        {
                                            m_wHitMode = 12;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }

                result = WarrAttackTarget1(m_wHitMode);
                if (result)
                {
                    m_dwHitTick = HUtil32.GetTickCount();
                }
            }
            catch
            {
                // M2Share.MainOutMessage(format("{%s} TAIPlayObject.WarrorAttackTarget Code:%d", new byte[] { nCode }));
            }
            return result;
        }

        /// <summary>
        /// 法师攻击
        /// </summary>
        /// <returns></returns>
        private bool WizardAttackTarget1()
        {
            TUserMagic UserMagic;
            int n14;
            bool result = false;
            try
            {
                m_wHitMode = 0;
                SearchMagic(); // 查询魔法
                if (m_nSelectMagic == 0)
                {
                    m_boIsUseMagic = true;// 是否能躲避
                }
                if (m_nSelectMagic > 0)
                {
                    if ((m_TargetCret != null))
                    {
                        if (!MagCanHitTarget(m_nCurrX, m_nCurrY, m_TargetCret) || ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 7) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 7)))
                        {
                            // 魔法不能打到怪
                            if (m_nSelectMagic != 10)// 除疾光电影外
                            {
                                GetGotoXY(m_TargetCret, 3);// 法只走向目标3格范围
                                GotoTargetXY(m_nTargetX, m_nTargetY, 0);
                            }
                        }
                    }
                    UserMagic = FindMagic(m_nSelectMagic);
                    if ((UserMagic != null))
                    {
                        if ((UserMagic.btKey == 0))// 技能打开状态才能使用
                        {
                            m_dwHitTick = HUtil32.GetTickCount();
                            result = UseSpell(UserMagic, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, m_TargetCret);// 使用魔法
                            return result;
                        }
                    }
                }
                m_dwHitTick = HUtil32.GetTickCount();
                if (M2Share.g_Config.boHeroAttackTarget && (m_Abil.Level < 22))
                {
                    m_boIsUseMagic = false;// 是否能躲避
                    result = WarrAttackTarget1(m_wHitMode);
                }
            }
            catch
            {
                M2Share.MainOutMessage("TAIPlayObject.WizardAttackTarget");
            }
            return result;
        }

        /// <summary>
        /// 道士攻击
        /// </summary>
        /// <returns></returns>
        private bool TaoistAttackTarget1()
        {
            bool result = false;
            TUserMagic UserMagic;
            int n14;
            try
            {
                m_wHitMode = 0;
                if (m_TargetCret != null)
                {
                    if (M2Share.g_Config.boHeroAttackTao && (m_TargetCret.m_btRaceServer != grobal2.RC_PLAYOBJECT)) // 22级砍血量的怪 
                    {
                        if (m_TargetCret.m_WAbil.MaxHP >= 700)
                        {
                            SearchMagic();// 查询魔法
                        }
                        else
                        {
                            if (HUtil32.GetTickCount() - m_dwSearchMagic > 1300) // 增加查询魔法的间隔
                            {
                                SearchMagic();// 查询魔法
                                m_dwSearchMagic = HUtil32.GetTickCount();
                            }
                            else
                            {
                                m_boIsUseAttackMagic = false;// 可以走向目标
                            }
                        }
                    }
                    else
                    {
                        SearchMagic(); // 查询魔法
                    }
                }
                if (m_nSelectMagic == 0)
                {
                    m_boIsUseMagic = true;// 是否能躲避 
                }
                if (m_nSelectMagic > 0)
                {
                    if ((m_TargetCret != null))
                    {
                        if ((!MagCanHitTarget(m_nCurrX, m_nCurrY, m_TargetCret)) || ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 7) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 7)))
                        {
                            // 魔法不能打到怪
                            if (M2Share.g_Config.boHeroAttackTao && (m_TargetCret.m_btRaceServer != grobal2.RC_PLAYOBJECT))// 22级砍血量的怪
                            {
                                if (m_TargetCret.m_WAbil.MaxHP >= 700)
                                {
                                    GetGotoXY(m_TargetCret, 3); // 道只走向目标3格范围
                                    GotoTargetXY(m_nTargetX, m_nTargetY, 0);
                                }
                            }
                            else
                            {
                                GetGotoXY(m_TargetCret, 3); // 道只走向目标3格范围
                                GotoTargetXY(m_nTargetX, m_nTargetY, 0);
                            }
                        }
                    }
                    switch (m_nSelectMagic)
                    {
                        case grobal2.SKILL_HEALLING:// 治愈术 
                            if ((m_WAbil.HP <= Math.Round(m_WAbil.MaxHP * 0.7)))
                            {
                                UserMagic = FindMagic(m_nSelectMagic);
                                if ((UserMagic != null) && (UserMagic.btKey == 0))// 技能打开状态才能使用
                                {
                                    UseSpell(UserMagic, m_nCurrX, m_nCurrY, null);
                                    m_dwHitTick = HUtil32.GetTickCount();
                                    if (M2Share.g_Config.boHeroAttackTao && (m_TargetCret.m_btRaceServer != grobal2.RC_PLAYOBJECT))// 22级砍血量的怪
                                    {
                                        if ((m_TargetCret.m_WAbil.MaxHP >= 700))
                                        {
                                            m_boIsUseMagic = true;
                                            return result;
                                        }
                                        else
                                        {
                                            m_nSelectMagic = 0;
                                        }
                                    }
                                    else
                                    {
                                        m_boIsUseMagic = true;
                                        return result;
                                    }
                                }
                            }
                            break;
                        case grobal2.SKILL_BIGHEALLING:// 群体治疗术
                            if ((m_WAbil.HP <= Math.Round(m_WAbil.MaxHP * 0.7)))
                            {
                                UserMagic = FindMagic(m_nSelectMagic);
                                if ((UserMagic != null) && (UserMagic.btKey == 0))// 技能打开状态才能使用
                                {
                                    UseSpell(UserMagic, m_nCurrX, m_nCurrY, this);
                                    m_dwHitTick = HUtil32.GetTickCount();
                                    if (M2Share.g_Config.boHeroAttackTao && (m_TargetCret.m_btRaceServer != grobal2.RC_PLAYOBJECT))// 22级砍血量的怪 
                                    {
                                        if ((m_TargetCret.m_WAbil.MaxHP >= 700))
                                        {
                                            m_boIsUseMagic = true;// 能躲避
                                            return result;
                                        }
                                        else
                                        {
                                            m_nSelectMagic = 0;
                                        }
                                    }
                                    else
                                    {
                                        m_boIsUseMagic = true;// 能躲避
                                        return result;
                                    }
                                }
                            }
                            break;
                        case grobal2.SKILL_FIRECHARM:// 灵符火符
                            if (!MagCanHitTarget(m_nCurrX, m_nCurrY, m_TargetCret))
                            {
                                GetGotoXY(m_TargetCret, 3);
                                GotoTargetXY(m_nTargetX, m_nTargetY, 1);
                            }
                            break;
                        case grobal2.SKILL_AMYOUNSUL:
                        case grobal2.SKILL_GROUPAMYOUNSUL:
                            if ((m_TargetCret.m_wStatusTimeArr[grobal2.POISON_DECHEALTH] == 0) && (GetUserItemList(2, 1) >= 0))
                            {
                                n_AmuletIndx = 1;
                            }
                            else if ((m_TargetCret.m_wStatusTimeArr[grobal2.POISON_DAMAGEARMOR] == 0) && (GetUserItemList(2, 2) >= 0))
                            {
                                n_AmuletIndx = 2;
                            }
                            break;
                        case grobal2.SKILL_CLOAK:
                        case grobal2.SKILL_BIGCLOAK: // 集体隐身术  隐身术
                            UserMagic = FindMagic(m_nSelectMagic);
                            if ((UserMagic != null) && (UserMagic.btKey == 0))// 技能打开状态才能使用
                            {
                                UseSpell(UserMagic, m_nCurrX, m_nCurrY, this);
                                m_dwHitTick = HUtil32.GetTickCount();
                                if (M2Share.g_Config.boHeroAttackTao && (m_TargetCret.m_btRaceServer != grobal2.RC_PLAYOBJECT))// 22级砍血量的怪 
                                {
                                    if (m_TargetCret.m_WAbil.MaxHP >= 700)
                                    {
                                        m_boIsUseMagic = false;
                                        return result;
                                    }
                                    else
                                    {
                                        m_nSelectMagic = 0;
                                    }
                                }
                                else
                                {
                                    m_boIsUseMagic = false;
                                    return result;
                                }
                            }
                            break;
                        case grobal2.SKILL_SKELLETON:
                        case grobal2.SKILL_SINSU:
                            UserMagic = FindMagic(m_nSelectMagic);
                            if ((UserMagic != null) && (UserMagic.btKey == 0))
                            {
                                UseSpell(UserMagic, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, m_TargetCret); // 使用魔法
                                m_dwHitTick = HUtil32.GetTickCount();
                                if (M2Share.g_Config.boHeroAttackTao && (m_TargetCret.m_btRaceServer != grobal2.RC_PLAYOBJECT))
                                {
                                    // 22级砍血量的怪
                                    if ((m_TargetCret.m_WAbil.MaxHP >= 700))
                                    {
                                        m_boIsUseMagic = true; // 能躲避
                                        return result;
                                    }
                                    else
                                    {
                                        m_nSelectMagic = 0;
                                    }
                                }
                                else
                                {
                                    m_boIsUseMagic = true; // 能躲避
                                    return result;
                                }
                            }
                            break;
                    }
                    UserMagic = FindMagic(m_nSelectMagic);
                    if ((UserMagic != null))
                    {
                        if ((UserMagic.btKey == 0))   // 技能打开状态才能使用 
                        {
                            m_dwHitTick = HUtil32.GetTickCount();
                            result = UseSpell(UserMagic, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, m_TargetCret); // 使用魔法
                            if ((m_TargetCret.m_WAbil.MaxHP >= 700) || (!M2Share.g_Config.boHeroAttackTao))
                            {
                                return result;
                            }
                        }
                    }
                }
                m_dwHitTick = HUtil32.GetTickCount();
                if ((m_WAbil.HP <= Math.Round(m_WAbil.MaxHP * 0.15)))
                {
                    m_boIsUseMagic = true;
                }
                // 是否能躲避 
                // 增加人形条件
                if ((M2Share.g_Config.boHeroAttackTarget && (m_Abil.Level < 22)) || ((m_TargetCret.m_WAbil.MaxHP < 700) && M2Share.g_Config.boHeroAttackTao && (m_TargetCret.m_btRaceServer != grobal2.RC_PLAYOBJECT)))
                {
                    // 20090106 道士22级前是否物理攻击  怪等级小于英雄时
                    if ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 1) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 1))
                    {
                        // 道走近目标砍 
                        GotoTargetXY(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 0);
                    }
                    m_boIsUseMagic = false;// 是否能躲避
                    result = WarrAttackTarget1(m_wHitMode);
                }
            }
            catch
            {
                // M2Share.MainOutMessage('{异常} TAIPlayObject.TaoistAttackTarget');
            }
            return result;
        }

        private bool AttackTarget()
        {
            long dwAttackTime;
            bool result = false;
            try
            {
                if ((m_TargetCret != null))
                {
                    if (InSafeZone())// 英雄进入安全区内就不打PK目标
                    {
                        if ((m_TargetCret.m_btRaceServer == grobal2.RC_PLAYOBJECT))
                        {
                            m_TargetCret = null;
                            return result;
                        }
                    }
                    if (m_TargetCret == this) // 防止英雄自己打自己
                    {
                        m_TargetCret = null;
                        return result;
                    }
                }
                byte nCode = 2;
                m_dwTargetFocusTick = HUtil32.GetTickCount();
                if (m_boDeath || m_boGhost)
                {
                    return result;
                }
                if (m_boAI && (m_TargetCret != null) && (HUtil32.GetTickCount() - m_dwHeroUseSpellTick > 12000))
                {
                    // 英雄连击停止后才使用合击
                    m_dwHeroUseSpellTick = HUtil32.GetTickCount();// 自动使用合击间隔
                    //ClientHeroUseSpell();
                    m_boIsUseMagic = false;// 是否能躲避
                    result = true;
                    return result;
                }
                switch (m_btJob)
                {
                    case 0:
                        if ((HUtil32.GetTickCount() - m_dwHitTick > M2Share.g_Config.nAIWarrorAttackTime))
                        {
                            m_boIsUseMagic = false;// 是否能躲避
                            nCode = 8;
                            result = WarrorAttackTarget1();
                        }
                        break;
                    case 1:
                        nCode = 4;
                        if ((HUtil32.GetTickCount() - m_dwHitTick > M2Share.g_Config.nAIWizardAttackTime))// 连击也不受间隔控制
                        {
                            nCode = 41;
                            m_dwHitTick = HUtil32.GetTickCount();
                            m_boIsUseMagic = false;// 是否能躲避
                            nCode = 7;
                            result = WizardAttackTarget1();
                            m_nSelectMagic = 0;
                            return result;
                        }
                        m_nSelectMagic = 0;
                        break;
                    case 2:
                        if ((HUtil32.GetTickCount() - m_dwHitTick > M2Share.g_Config.nAITaoistAttackTime))
                        {
                            // 连击也不受间隔控制 20100408
                            m_dwHitTick = HUtil32.GetTickCount();
                            m_boIsUseMagic = false; // 是否能躲避
                            nCode = 6;
                            result = TaoistAttackTarget1();
                            m_nSelectMagic = 0;
                            return result;
                        }
                        m_nSelectMagic = 0;
                        break;
                }
            }
            catch
            {
                M2Share.MainOutMessage("TAIPlayObject.AttackTarget");
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
        /// 是否需要躲避
        /// </summary>
        /// <returns></returns>
        private bool IsNeedAvoid()
        {
            bool result = false;
            try
            {
                if (((HUtil32.GetTickCount() - m_dwAutoAvoidTick) > 1100) && m_boIsUseMagic && !m_boDeath)
                {
                    // 血低于15%时,必定要躲 20080711
                    if ((m_btJob > 0) && ((m_nSelectMagic == 0) || (m_WAbil.HP <= Math.Round(m_WAbil.MaxHP * 0.15))))
                    {
                        m_dwAutoAvoidTick = HUtil32.GetTickCount();
                        if (M2Share.g_Config.boHeroAttackTarget && (m_Abil.Level < 22)) // 22级前道法不躲避
                        {
                            if (m_btJob == 1)// 法放魔法后要躲
                            {
                                if (CheckTargetXYCount(m_nCurrX, m_nCurrY, 4) > 0)
                                {
                                    result = true;
                                    return result;
                                }
                            }
                        }
                        else
                        {
                            switch (m_btJob)
                            {
                                case 1:
                                    if (CheckTargetXYCount(m_nCurrX, m_nCurrY, 4) > 0)
                                    {
                                        result = true;
                                        return result;
                                    }
                                    break;
                                case 2:
                                    if (m_TargetCret != null)
                                    {
                                        if (M2Share.g_Config.boHeroAttackTao && (m_TargetCret.m_btRaceServer != grobal2.RC_PLAYOBJECT)) // 22级砍血量的怪 20090108
                                        {
                                            if ((m_TargetCret.m_WAbil.MaxHP >= 700))
                                            {
                                                if ((CheckTargetXYCount(m_nCurrX, m_nCurrY, 4) > 0))
                                                {
                                                    result = true;
                                                    return result;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (CheckTargetXYCount(m_nCurrX, m_nCurrY, 4) > 0)
                                            {
                                                result = true;
                                                return result;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (CheckTargetXYCount(m_nCurrX, m_nCurrY, 4) > 0)
                                        {
                                            result = true;
                                            return result;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
                M2Share.MainOutMessage("TAIPlayObject.IsNeedAvoid");
            }
            return result;
        }

        // 气功波，抗拒火环使用
        // 检测指定方向和范围内坐标的怪物数量
        private int CheckTargetXYCountOfDirection(int nX, int nY, int nDir, int nRange)
        {
            int result = 0;
            TBaseObject BaseObject;
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
                                switch (nDir)
                                {
                                    case grobal2.DR_UP:
                                        if ((Math.Abs(nX - BaseObject.m_nCurrX) <= nRange) && ((BaseObject.m_nCurrY - nY) >= 0 && (BaseObject.m_nCurrY - nY) <= nRange))
                                        {
                                            result++;
                                        }
                                        break;
                                    case grobal2.DR_UPRIGHT:
                                        if (((BaseObject.m_nCurrX - nX) >= 0 && (BaseObject.m_nCurrX - nX) <= nRange) && ((BaseObject.m_nCurrY - nY) >= 0 && (BaseObject.m_nCurrY - nY) <= nRange))
                                        {
                                            result++;
                                        }
                                        break;
                                    case grobal2.DR_RIGHT:
                                        if (((BaseObject.m_nCurrX - nX) >= 0 && (BaseObject.m_nCurrX - nX) <= nRange) && (Math.Abs(nY - BaseObject.m_nCurrY) <= nRange))
                                        {
                                            result++;
                                        }
                                        break;
                                    case grobal2.DR_DOWNRIGHT:
                                        if (((BaseObject.m_nCurrX - nX) >= 0 && (BaseObject.m_nCurrX - nX) <= nRange) && ((nY - BaseObject.m_nCurrY) >= 0 && (nY - BaseObject.m_nCurrY) <= nRange))
                                        {
                                            result++;
                                        }
                                        break;
                                    case grobal2.DR_DOWN:
                                        if ((Math.Abs(nX - BaseObject.m_nCurrX) <= nRange) && ((nY - BaseObject.m_nCurrY) >= 0 && (nY - BaseObject.m_nCurrY) <= nRange))
                                        {
                                            result++;
                                        }
                                        break;
                                    case grobal2.DR_DOWNLEFT:
                                        if (((nX - BaseObject.m_nCurrX) >= 0 && (nX - BaseObject.m_nCurrX) <= nRange) && ((nY - BaseObject.m_nCurrY) >= 0 && (nY - BaseObject.m_nCurrY) <= nRange))
                                        {
                                            result++;
                                        }
                                        break;
                                    case grobal2.DR_LEFT:
                                        if (((nX - BaseObject.m_nCurrX) >= 0 && (nX - BaseObject.m_nCurrX) <= nRange) && (Math.Abs(nY - BaseObject.m_nCurrY) <= nRange))
                                        {
                                            result++;
                                        }
                                        break;
                                    case grobal2.DR_UPLEFT:
                                        if (((nX - BaseObject.m_nCurrX) >= 0 && (nX - BaseObject.m_nCurrX) <= nRange) && ((BaseObject.m_nCurrY - nY) >= 0 && (BaseObject.m_nCurrY - nY) <= nRange))
                                        {
                                            result++;
                                        }
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
        /// 自动躲避
        /// </summary>
        /// <returns></returns>
        public int AutoAvoid_GetAvoidDir()
        {
            int n10;
            int n14;
            n10 = m_TargetCret.m_nCurrX;
            n14 = m_TargetCret.m_nCurrY;
            int result = grobal2.DR_DOWN;
            if (n10 > m_nCurrX)
            {
                result = grobal2.DR_LEFT;
                if (n14 > m_nCurrY)
                {
                    result = grobal2.DR_DOWNLEFT;
                }
                if (n14 < m_nCurrY)
                {
                    result = grobal2.DR_UPLEFT;
                }
            }
            else
            {
                if (n10 < m_nCurrX)
                {
                    result = grobal2.DR_RIGHT;
                    if (n14 > m_nCurrY)
                    {
                        result = grobal2.DR_DOWNRIGHT;
                    }
                    if (n14 < m_nCurrY)
                    {
                        result = grobal2.DR_UPRIGHT;
                    }
                }
                else
                {
                    if (n14 > m_nCurrY)
                    {
                        result = grobal2.DR_UP;
                    }
                    else if (n14 < m_nCurrY)
                    {
                        result = grobal2.DR_DOWN;
                    }
                }
            }
            return result;
        }

        public byte AutoAvoid_GetDirXY(int nTargetX, int nTargetY)
        {
            int n10 = nTargetX;
            int n14 = nTargetY;
            byte result = grobal2.DR_DOWN;
            if (n10 > m_nCurrX)
            {
                result = grobal2.DR_RIGHT;
                if (n14 > m_nCurrY)
                {
                    result = grobal2.DR_DOWNRIGHT;
                }
                if (n14 < m_nCurrY)
                {
                    result = grobal2.DR_UPRIGHT;
                }
            }
            else
            {
                if (n10 < m_nCurrX)
                {
                    result = grobal2.DR_LEFT;
                    if (n14 > m_nCurrY)
                    {
                        result = grobal2.DR_DOWNLEFT;
                    }
                    if (n14 < m_nCurrY)
                    {
                        result = grobal2.DR_UPLEFT;
                    }
                }
                else
                {
                    if (n14 > m_nCurrY)
                    {
                        result = grobal2.DR_DOWN;
                    }
                    else if (n14 < m_nCurrY)
                    {
                        result = grobal2.DR_UP;
                    }
                }
            }
            return result;
        }

        public bool AutoAvoid_GetGotoXY(byte nDir, ref short nTargetX, ref short nTargetY)
        {
            int n01 = 0;
            while (true)
            {
                switch (nDir)
                {
                    case grobal2.DR_UP:
                        if (m_PEnvir.CanWalk(nTargetX, nTargetY, false) && (CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0))
                        {
                            nTargetY -= 2;
                            break;
                        }
                        else
                        {
                            if (n01 >= 8)
                            {
                                break;
                            }
                            nTargetY -= 2;
                            n01 += 2;
                            continue;
                        }
                    case grobal2.DR_UPRIGHT:
                        if (m_PEnvir.CanWalk(nTargetX, nTargetY, false) && (CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0))
                        {
                            nTargetX += 2;
                            nTargetY -= 2;
                            break;
                        }
                        else
                        {
                            if (n01 >= 8)
                            {
                                break;
                            }
                            nTargetX += 2;
                            nTargetY -= 2;
                            n01 += 2;
                            continue;
                        }
                    case grobal2.DR_RIGHT:
                        if (m_PEnvir.CanWalk(nTargetX, nTargetY, false) && (CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0))
                        {
                            nTargetX += 2;
                            break;
                        }
                        else
                        {
                            if (n01 >= 8)
                            {
                                break;
                            }
                            nTargetX += 2;
                            n01 += 2;
                            continue;
                        }
                    case grobal2.DR_DOWNRIGHT:
                        if (m_PEnvir.CanWalk(nTargetX, nTargetY, false) && (CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0))
                        {
                            nTargetX += 2;
                            nTargetY += 2;
                            break;
                        }
                        else
                        {
                            if (n01 >= 8)
                            {
                                break;
                            }
                            nTargetX += 2;
                            nTargetY += 2;
                            n01 += 2;
                            continue;
                        }
                    case grobal2.DR_DOWN:
                        if (m_PEnvir.CanWalk(nTargetX, nTargetY, false) && (CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0))
                        {
                            nTargetY += 2;
                            break;
                        }
                        else
                        {
                            if (n01 >= 8)
                            {
                                break;
                            }
                            nTargetY += 2;
                            n01 += 2;
                            continue;
                        }
                    case grobal2.DR_DOWNLEFT:
                        if (m_PEnvir.CanWalk(nTargetX, nTargetY, false) && (CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0))
                        {
                            nTargetX -= 2;
                            nTargetY += 2;
                            break;
                        }
                        else
                        {
                            if (n01 >= 8)
                            {
                                break;
                            }
                            nTargetX -= 2;
                            nTargetY += 2;
                            n01 += 2;
                            continue;
                        }
                    case grobal2.DR_LEFT:
                        if (m_PEnvir.CanWalk(nTargetX, nTargetY, false) && (CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0))
                        {
                            nTargetX -= 2;
                            break;
                        }
                        else
                        {
                            if (n01 >= 8)
                            {
                                break;
                            }
                            nTargetX -= 2;
                            n01 += 2;
                            continue;
                        }
                    case grobal2.DR_UPLEFT:
                        if (m_PEnvir.CanWalk(nTargetX, nTargetY, false) && (CheckTargetXYCountOfDirection(nTargetX, nTargetY, nDir, 3) == 0))
                        {
                            nTargetX -= 2;
                            nTargetY -= 2;
                            break;
                        }
                        else
                        {
                            if (n01 >= 8)
                            {
                                break;
                            }
                            nTargetX -= 2;
                            nTargetY -= 2;
                            n01 += 2;
                            continue;
                        }
                    default:
                        break;
                }
            }
            bool result;
            return result;
        }

        public bool AutoAvoid_GetAvoidXY(ref short nTargetX, ref short nTargetY)
        {
            int n10;
            byte nDir = 0;
            short nX = nTargetX;
            short nY = nTargetY;
            bool result = AutoAvoid_GetGotoXY(m_btLastDirection, ref nTargetX, ref nTargetY);
            n10 = 0;
            while (true)
            {
                if (n10 >= 7)
                {
                    break;
                }
                if (result)
                {
                    break;
                }
                nTargetX = nX;
                nTargetY = nY;
                nDir = (byte)(new System.Random(7)).Next();
                result = AutoAvoid_GetGotoXY(nDir, ref nTargetX, ref nTargetY);
                n10++;
            }
            m_btLastDirection = (byte)nDir;
            return result;
        }

        /// <summary>
        /// 是否需要躲避
        /// </summary>
        /// <returns></returns>
        private bool AutoAvoid()
        {
            bool result = true;
            if ((m_TargetCret != null) && !m_TargetCret.m_boDeath)
            {
                byte nDir = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
                nDir = GetBackDir(nDir);
                m_PEnvir.GetNextPosition(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, nDir, 5, ref m_nTargetX, ref m_nTargetY);
                result = GotoTargetXY(m_nTargetX, m_nTargetY, 1);
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
            if ((m_TargetCret != null) && (HUtil32.GetTickCount() - m_dwAutoAvoidTick > 1100) && (!m_boIsUseAttackMagic || (m_btJob == 0)))
            {
                if (m_btJob > 0)
                {
                    if (!m_boIsUseMagic && ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 3) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 3)))
                    {
                        result = true;
                        return result;
                    }
                    if (((M2Share.g_Config.boHeroAttackTarget && (m_Abil.Level < 22)) || (M2Share.g_Config.boHeroAttackTao && (m_TargetCret.m_WAbil.MaxHP < 700) && (m_TargetCret.m_btRaceServer != grobal2.RC_PLAYOBJECT) && (m_btJob == 2))) && ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 1) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 1)))
                    {
                        // 道法22前是否物理攻击 20090210 大于1格时才走向目标
                        result = true;
                        return result;
                    }
                }
                else
                {
                    switch (m_nSelectMagic)
                    {
                        case grobal2.SKILL_ERGUM:
                            if (AllowUseMagic(12) && (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, m_btDirection, 2, ref m_nTargetX, ref m_nTargetY)))
                            {
                                if (((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 2) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 0)) || ((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 0) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 2)) || ((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 2) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 2)))
                                {
                                    dwAttackTime = HUtil32._MAX(0, ((int)M2Share.g_Config.dwHeroWarrorAttackTime) - m_nHitSpeed * M2Share.g_Config.ClientConf.btItemSpeed); // 防止负数出错
                                    if ((HUtil32.GetTickCount() - m_dwHitTick > dwAttackTime))
                                    {
                                        m_wHitMode = 4;
                                        m_dwTargetFocusTick = HUtil32.GetTickCount();
                                        m_btDirection = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
                                        Attack(m_TargetCret, m_btDirection);
                                        BreakHolySeizeMode();
                                        m_dwHitTick = HUtil32.GetTickCount();
                                        return result;
                                    }
                                }
                                else
                                {
                                    if ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 1) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 1))
                                    {
                                        result = true;
                                        return result;
                                    }
                                }
                            }
                            m_nSelectMagic = 0;
                            if (AllowUseMagic(12))
                            {
                                if ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 2) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 2))
                                {
                                    result = true;
                                    return result;
                                }
                            }
                            else if ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 1) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 1))
                            {
                                result = true;
                                return result;
                            }
                            break;
                        case 43:
                            if (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, m_btDirection, 5, ref m_nTargetX, ref m_nTargetY))
                            {
                                if (((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 4) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 0)) || ((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 0) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) <= 4)) || (((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 2) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 2)) || ((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 3) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 3)) || ((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 4) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 4))))
                                {
                                    dwAttackTime = HUtil32._MAX(0, ((int)M2Share.g_Config.dwHeroWarrorAttackTime) - m_nHitSpeed * M2Share.g_Config.ClientConf.btItemSpeed);// 防止负数出错
                                    if ((HUtil32.GetTickCount() - m_dwHitTick > dwAttackTime))
                                    {
                                        m_wHitMode = 9;
                                        m_dwTargetFocusTick = HUtil32.GetTickCount();
                                        m_btDirection = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
                                        Attack(m_TargetCret, m_btDirection);
                                        BreakHolySeizeMode();
                                        m_dwHitTick = HUtil32.GetTickCount();
                                        return result;
                                    }
                                }
                                else
                                {
                                    if (AllowUseMagic(12))
                                    {
                                        if (((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) != 2) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) != 0)) || ((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) != 0) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) != 2)) || ((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) != 2) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) != 2)))
                                        {
                                            result = true;
                                            return result;
                                        }
                                    }
                                    else if ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 1) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 1))
                                    {
                                        result = true;
                                        return result;
                                    }
                                }
                            }
                            m_nSelectMagic = 0;
                            if (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, m_btDirection, 2, ref m_nTargetX, ref m_nTargetY))
                            {
                                if ((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 2) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 0) || (Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 0) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 2) || (Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 2) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 2))
                                {
                                    dwAttackTime = HUtil32._MAX(0, ((int)M2Share.g_Config.dwHeroWarrorAttackTime) - m_nHitSpeed * M2Share.g_Config.ClientConf.btItemSpeed);
                                    // 防止负数出错
                                    if ((HUtil32.GetTickCount() - m_dwHitTick > dwAttackTime))
                                    {
                                        m_wHitMode = 9;
                                        m_dwTargetFocusTick = HUtil32.GetTickCount();
                                        m_btDirection = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
                                        Attack(m_TargetCret, m_btDirection);
                                        BreakHolySeizeMode();
                                        m_dwHitTick = HUtil32.GetTickCount();
                                        return result;
                                    }
                                }
                                else
                                {
                                    if ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 1) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 1))
                                    {
                                        result = true;
                                        return result;
                                    }
                                }
                            }
                            m_nSelectMagic = 0;
                            if (AllowUseMagic(12))
                            {
                                if ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 2) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 2))
                                {
                                    result = true;
                                    return result;
                                }
                            }
                            else if ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 1) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 1))
                            {
                                result = true;
                                return result;
                            }
                            break;
                        case 7:
                        case 25:
                        case 26:
                            if ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 1) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 1))
                            {
                                result = true;
                                m_nSelectMagic = 0;
                                return result;
                            }
                            break;
                        default:
                            if (AllowUseMagic(12))
                            {
                                if (!(((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 2) && (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 0)) || ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 1) && (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 0)) || ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 1) && (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 1)) || ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 2) && (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 2)) || ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 0) && (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 1)) || ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 0) && (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 2))))
                                {
                                    result = true;
                                    return result;
                                }
                                if (((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 1) && (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 2)) || ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) == 2) && (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) == 1)))
                                {
                                    result = true;
                                    return result;
                                }
                            }
                            else if ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 1) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 1))
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

        // 是否走向目标
        // 取刺杀位
        private bool GetGotoXY(TBaseObject BaseObject, byte nCode)
        {
            bool result = false;
            switch (nCode)
            {
                case 2:
                    // 刺杀位
                    if ((m_nCurrX - 2 <= BaseObject.m_nCurrX) && (m_nCurrX + 2 >= BaseObject.m_nCurrX) && (m_nCurrY - 2 <= BaseObject.m_nCurrY) && (m_nCurrY + 2 >= BaseObject.m_nCurrY) && ((m_nCurrX != BaseObject.m_nCurrX) || (m_nCurrY != BaseObject.m_nCurrY)))
                    {
                        result = true;
                        if (((m_nCurrX - 2) == BaseObject.m_nCurrX) && (m_nCurrY == BaseObject.m_nCurrY))
                        {
                            m_nTargetX = (short)(m_nCurrX - 2);
                            m_nTargetY = m_nCurrY;
                            return result;
                        }
                        if (((m_nCurrX + 2) == BaseObject.m_nCurrX) && (m_nCurrY == BaseObject.m_nCurrY))
                        {
                            m_nTargetX = (short)(m_nCurrX + 2);
                            m_nTargetY = m_nCurrY;
                            return result;
                        }
                        if ((m_nCurrX == BaseObject.m_nCurrX) && ((m_nCurrY - 2) == BaseObject.m_nCurrY))
                        {
                            m_nTargetX = m_nCurrX;
                            m_nTargetY = (short)(m_nCurrY - 2);
                            return result;
                        }
                        if ((m_nCurrX == BaseObject.m_nCurrX) && ((m_nCurrY + 2) == BaseObject.m_nCurrY))
                        {
                            m_nTargetX = m_nCurrX;
                            m_nTargetY = (short)(m_nCurrY + 2);
                            return result;
                        }
                        if (((m_nCurrX - 2) == BaseObject.m_nCurrX) && ((m_nCurrY - 2) == BaseObject.m_nCurrY))
                        {
                            m_nTargetX = (short)(m_nCurrX - 2);
                            m_nTargetY = (short)(m_nCurrY - 2);
                            return result;
                        }
                        if (((m_nCurrX + 2) == BaseObject.m_nCurrX) && ((m_nCurrY - 2) == BaseObject.m_nCurrY))
                        {
                            m_nTargetX = (short)(m_nCurrX + 2);
                            m_nTargetY = (short)(m_nCurrY - 2);
                            return result;
                        }
                        if (((m_nCurrX - 2) == BaseObject.m_nCurrX) && ((m_nCurrY + 2) == BaseObject.m_nCurrY))
                        {
                            m_nTargetX = (short)(m_nCurrX - 2);
                            m_nTargetY = (short)(m_nCurrY + 2);
                            return result;
                        }
                        if (((m_nCurrX + 2) == BaseObject.m_nCurrX) && ((m_nCurrY + 2) == BaseObject.m_nCurrY))
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
                    if ((m_nCurrX - 3 <= BaseObject.m_nCurrX) && (m_nCurrX + 3 >= BaseObject.m_nCurrX) && (m_nCurrY - 3 <= BaseObject.m_nCurrY) && (m_nCurrY + 3 >= BaseObject.m_nCurrY) && ((m_nCurrX != BaseObject.m_nCurrX) || (m_nCurrY != BaseObject.m_nCurrY)))
                    {
                        result = true;
                        if (((m_nCurrX - 3) == BaseObject.m_nCurrX) && (m_nCurrY == BaseObject.m_nCurrY))
                        {
                            m_nTargetX = (short)(m_nCurrX - 3);
                            m_nTargetY = m_nCurrY;
                            return result;
                        }
                        if (((m_nCurrX + 3) == BaseObject.m_nCurrX) && (m_nCurrY == BaseObject.m_nCurrY))
                        {
                            m_nTargetX = (short)(m_nCurrX + 3);
                            m_nTargetY = m_nCurrY;
                            return result;
                        }
                        if ((m_nCurrX == BaseObject.m_nCurrX) && ((m_nCurrY - 3) == BaseObject.m_nCurrY))
                        {
                            m_nTargetX = m_nCurrX;
                            m_nTargetY = (short)(m_nCurrY - 3);
                            return result;
                        }
                        if ((m_nCurrX == BaseObject.m_nCurrX) && ((m_nCurrY + 3) == BaseObject.m_nCurrY))
                        {
                            m_nTargetX = m_nCurrX;
                            m_nTargetY = (short)(m_nCurrY + 3);
                            return result;
                        }
                        if (((m_nCurrX - 3) == BaseObject.m_nCurrX) && ((m_nCurrY - 3) == BaseObject.m_nCurrY))
                        {
                            m_nTargetX = (short)(m_nCurrX - 3);
                            m_nTargetY = (short)(m_nCurrY - 3);
                            return result;
                        }
                        if (((m_nCurrX + 3) == BaseObject.m_nCurrX) && ((m_nCurrY - 3) == BaseObject.m_nCurrY))
                        {
                            m_nTargetX = (short)(m_nCurrX + 3);
                            m_nTargetY = (short)(m_nCurrY - 3);
                            return result;
                        }
                        if (((m_nCurrX - 3) == BaseObject.m_nCurrX) && ((m_nCurrY + 3) == BaseObject.m_nCurrY))
                        {
                            m_nTargetX = (short)(m_nCurrX - 3);
                            m_nTargetY = (short)(m_nCurrY + 3);
                            return result;
                        }
                        if (((m_nCurrX + 3) == BaseObject.m_nCurrX) && ((m_nCurrY + 3) == BaseObject.m_nCurrY))
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

        // 检测指定方向和范围内坐标的怪物数量
        // 跑到目标坐标
        private bool RunToTargetXY(short nTargetX, short nTargetY)
        {
            int n10;
            int n14;
            bool result = false;
            if (m_boTransparent && m_boHideMode)
            {
                m_wStatusTimeArr[grobal2.STATE_TRANSPARENT] = 1;// 隐身,一动就显身
            }
            if (((m_wStatusTimeArr[grobal2.POISON_STONE] > 0) && (!M2Share.g_Config.ClientConf.boParalyCanSpell)) || (m_wStatusTimeArr[grobal2.POISON_DONTMOVE] != 0) || (m_wStatusTimeArr[grobal2.POISON_LOCKSPELL] != 0))// || (m_wStatusArrValue[23] != 0)
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
                    if ((Math.Abs(nTargetX - m_nCurrX) <= 1) && (Math.Abs(nTargetY - m_nCurrY) <= 1))
                    {
                        result = true;
                        dwTick5F4 = HUtil32.GetTickCount();
                    }
                }
            }
            return result;
        }

        public bool RunTo1(byte btDir, bool boFlag,short nDestX, short nDestY)
        {
            const string sExceptionMsg = "[Exception] TBaseObject::RunTo";
            var result = false;
            try
            {
                int nOldX = m_nCurrX;
                int nOldY = m_nCurrY;
                m_btDirection = btDir;
                switch (btDir)
                {
                    case grobal2.DR_UP:
                        if ((m_nCurrY > 1) &&
                          (m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY - 1, M2Share.g_Config.boDiableHumanRun || ((m_btPermission > 9) && M2Share.g_Config.boGMRunAll)) || (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
                        (m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY - 2, M2Share.g_Config.boDiableHumanRun || ((m_btPermission > 9) && M2Share.g_Config.boGMRunAll)) || (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
                        (m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX, m_nCurrY - 2, true) > 0))
                        {
                            m_nCurrY -= 2;
                        }
                        break;
                    case grobal2.DR_UPRIGHT:
                        if ((m_nCurrX < m_PEnvir.wWidth - 2) &&
                          (m_nCurrY > 1) &&
                          (m_PEnvir.CanWalkEx(m_nCurrX + 1, m_nCurrY - 1, M2Share.g_Config.boDiableHumanRun || ((m_btPermission > 9) && M2Share.g_Config.boGMRunAll)) || (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
                        (m_PEnvir.CanWalkEx(m_nCurrX + 2, m_nCurrY - 2, M2Share.g_Config.boDiableHumanRun || ((m_btPermission > 9) && M2Share.g_Config.boGMRunAll)) || (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
                        (m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX + 2, m_nCurrY - 2, true) > 0))
                        {
                            m_nCurrX += 2;
                            m_nCurrY -= 2;
                        }
                        break;
                    case grobal2.DR_RIGHT:
                        if ((m_nCurrX < m_PEnvir.wWidth - 2) &&
  (m_PEnvir.CanWalkEx(m_nCurrX + 1, m_nCurrY, M2Share.g_Config.boDiableHumanRun || ((m_btPermission > 9) && M2Share.g_Config.boGMRunAll)) || (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
  (m_PEnvir.CanWalkEx(m_nCurrX + 2, m_nCurrY, M2Share.g_Config.boDiableHumanRun || ((m_btPermission > 9) && M2Share.g_Config.boGMRunAll)) || (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
    (m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX + 2, m_nCurrY, true) > 0))
                        {
                            m_nCurrX += 2;
                        }
                        break;
                    case grobal2.DR_DOWNRIGHT:
                        if ((m_nCurrX < m_PEnvir.wWidth - 2) &&
  (m_nCurrY < m_PEnvir.wHeight - 2) &&
  (m_PEnvir.CanWalkEx(m_nCurrX + 1, m_nCurrY + 1, M2Share.g_Config.boDiableHumanRun || ((m_btPermission > 9) && M2Share.g_Config.boGMRunAll)) || (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
  (m_PEnvir.CanWalkEx(m_nCurrX + 2, m_nCurrY + 2, M2Share.g_Config.boDiableHumanRun || ((m_btPermission > 9) && M2Share.g_Config.boGMRunAll)) || (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
    (m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX + 2, m_nCurrY + 2, true) > 0))
                        {
                            m_nCurrX += 2;
                            m_nCurrY += 2;
                        }
                        break;
                    case grobal2.DR_DOWN:
                        if ((m_nCurrY < m_PEnvir.wHeight - 2) &&
  (m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY + 1, M2Share.g_Config.boDiableHumanRun || ((m_btPermission > 9) && M2Share.g_Config.boGMRunAll)) || (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
  (m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY + 2, M2Share.g_Config.boDiableHumanRun || ((m_btPermission > 9) && M2Share.g_Config.boGMRunAll)) || (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
    (m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX, m_nCurrY + 2, true) > 0))
                        {
                            m_nCurrY += 2;
                        }
                        break;
                    case grobal2.DR_DOWNLEFT:
                        if ((m_nCurrX > 1) &&
  (m_nCurrY < m_PEnvir.wHeight - 2) &&
  (m_PEnvir.CanWalkEx(m_nCurrX - 1, m_nCurrY + 1, M2Share.g_Config.boDiableHumanRun || ((m_btPermission > 9) && M2Share.g_Config.boGMRunAll)) || (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
  (m_PEnvir.CanWalkEx(m_nCurrX - 2, m_nCurrY + 2, M2Share.g_Config.boDiableHumanRun || ((m_btPermission > 9) && M2Share.g_Config.boGMRunAll)) || (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
    (m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX - 2, m_nCurrY + 2, true) > 0))
                        {
                            m_nCurrX -= 2;
                            m_nCurrY += 2;
                        }

                        break;
                    case grobal2.DR_LEFT:
                        if ((m_nCurrX > 1) &&
  (m_PEnvir.CanWalkEx(m_nCurrX - 1, m_nCurrY, M2Share.g_Config.boDiableHumanRun || ((m_btPermission > 9) && M2Share.g_Config.boGMRunAll)) || (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
  (m_PEnvir.CanWalkEx(m_nCurrX - 2, m_nCurrY, M2Share.g_Config.boDiableHumanRun || ((m_btPermission > 9) && M2Share.g_Config.boGMRunAll)) || (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
    (m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX - 2, m_nCurrY, true) > 0))
                        {
                            m_nCurrX -= 2;
                        }
                        break;
                    case grobal2.DR_UPLEFT:
                        if ((m_nCurrX > 1) && (m_nCurrY > 1) &&
 (m_PEnvir.CanWalkEx(m_nCurrX - 1, m_nCurrY - 1, M2Share.g_Config.boDiableHumanRun || ((m_btPermission > 9) && M2Share.g_Config.boGMRunAll)) || (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
  (m_PEnvir.CanWalkEx(m_nCurrX - 2, m_nCurrY - 2, M2Share.g_Config.boDiableHumanRun || ((m_btPermission > 9) && M2Share.g_Config.boGMRunAll)) || (M2Share.g_Config.boSafeAreaLimited && InSafeZone())) &&
    (m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX - 2, m_nCurrY - 2, true) > 0))
                        {
                            m_nCurrX -= 2;
                            m_nCurrY -= 2;
                        }
                        break;
                }
                if (m_nCurrX != nOldX || m_nCurrY != nOldY)
                {
                    if (Walk(grobal2.RM_RUN))
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
                m_wStatusTimeArr[grobal2.STATE_TRANSPARENT] = 1;// 隐身,一动就显身
            }
            if (((m_wStatusTimeArr[grobal2.POISON_STONE] != 0) && (!M2Share.g_Config.ClientConf.boParalyCanSpell)) || (m_wStatusTimeArr[grobal2.POISON_DONTMOVE] != 0) || (m_wStatusTimeArr[grobal2.POISON_LOCKSPELL] != 0))
            {
                return result;// 麻痹不能跑动
            }
            if ((Math.Abs(nTargetX - m_nCurrX) > 1) || (Math.Abs(nTargetY - m_nCurrY) > 1))
            {
                if (HUtil32.GetTickCount() - dwTick3F4 > m_dwWalkIntervalTime)
                {
                    n10 = nTargetX;
                    n14 = nTargetY;
                    byte nDir = grobal2.DR_DOWN;
                    if (n10 > m_nCurrX)
                    {
                        nDir = grobal2.DR_RIGHT;
                        if (n14 > m_nCurrY)
                        {
                            nDir = grobal2.DR_DOWNRIGHT;
                        }
                        if (n14 < m_nCurrY)
                        {
                            nDir = grobal2.DR_UPRIGHT;
                        }
                    }
                    else
                    {
                        if (n10 < m_nCurrX)
                        {
                            nDir = grobal2.DR_LEFT;
                            if (n14 > m_nCurrY)
                            {
                                nDir = grobal2.DR_DOWNLEFT;
                            }
                            if (n14 < m_nCurrY)
                            {
                                nDir = grobal2.DR_UPLEFT;
                            }
                        }
                        else
                        {
                            if (n14 > m_nCurrY)
                            {
                                nDir = grobal2.DR_DOWN;
                            }
                            else if (n14 < m_nCurrY)
                            {
                                nDir = grobal2.DR_UP;
                            }
                        }
                    }
                    nOldX = m_nCurrX;
                    nOldY = m_nCurrY;
                    WalkTo(nDir, false);
                    if ((Math.Abs(nTargetX - m_nCurrX) <= 1) && (Math.Abs(nTargetY - m_nCurrY) <= 1))
                    {
                        result = true;
                        dwTick3F4 = HUtil32.GetTickCount();
                    }
                    if (!result)
                    {
                        n20 = (new System.Random(3)).Next();
                        for (var i = grobal2.DR_UP; i <= grobal2.DR_UPLEFT; i++)
                        {
                            if ((nOldX == m_nCurrX) && (nOldY == m_nCurrY))
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
                                    nDir = grobal2.DR_UPLEFT;
                                }
                                if ((nDir > grobal2.DR_UPLEFT))
                                {
                                    nDir = grobal2.DR_UP;
                                }
                                WalkTo(nDir, false);
                                if ((Math.Abs(nTargetX - m_nCurrX) <= 1) && (Math.Abs(nTargetY - m_nCurrY) <= 1))
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

        // 取刺杀位
        private bool GotoTargetXY(short nTargetX, short nTargetY, int nCode)
        {
            bool result = false;
            switch (nCode)
            {
                case 0:// 正常模式
                    if ((Math.Abs(m_nCurrX - nTargetX) > 2) || (Math.Abs(m_nCurrY - nTargetY) > 2))
                    {
                        if (m_wStatusTimeArr[grobal2.STATE_LOCKRUN] == 0)
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
                    if ((Math.Abs(m_nCurrX - nTargetX) > 1) || (Math.Abs(m_nCurrY - nTargetY) > 1))
                    {
                        if (m_wStatusTimeArr[grobal2.STATE_LOCKRUN] == 0)
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
                case 0:
                    if (AllowUseMagic(26) && ((HUtil32.GetTickCount() - m_dwLatestFireHitTick) > 9000))// 烈火
                    {
                        m_boFireHitSkill = true;
                        result = 26;
                        return result;
                    }
                    if (((m_TargetCret.m_btRaceServer == grobal2.RC_PLAYOBJECT) || (m_TargetCret.m_Master != null)) && (m_TargetCret.m_Abil.Level < m_Abil.Level))
                    {
                        // PK时,使用野蛮冲撞  20080826 血低于800时使用
                        if (AllowUseMagic(27) && ((HUtil32.GetTickCount() - m_SkillUseTick[27]) > 10000))
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
                        if (AllowUseMagic(27) && ((HUtil32.GetTickCount() - m_SkillUseTick[27]) > 10000) && (m_TargetCret.m_Abil.Level < m_Abil.Level) && (m_WAbil.HP <= Math.Round(m_WAbil.MaxHP * 0.85)))
                        {
                            m_SkillUseTick[27] = HUtil32.GetTickCount();
                            result = 27;
                            return result;
                        }
                    }
                    if ((m_TargetCret.m_Master != null))
                    {
                        m_ExpHitter = m_TargetCret.m_Master;
                    }
                    if (CheckTargetXYCount1(m_nCurrX, m_nCurrY, 1) > 1)
                    {
                        switch ((new System.Random(3)).Next())
                        {
                            case 0:// 被怪物包围
                                if (AllowUseMagic(41) && (HUtil32.GetTickCount() - m_SkillUseTick[41] > 10000) && (m_TargetCret.m_Abil.Level < m_Abil.Level) && (((m_TargetCret.m_btRaceServer != grobal2.RC_PLAYOBJECT)) || M2Share.g_Config.boGroupMbAttackPlayObject) && (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) <= 3) && (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) <= 3))
                                {
                                    m_SkillUseTick[41] = HUtil32.GetTickCount();// 狮子吼
                                    result = 41;
                                    return result;
                                }
                                if (AllowUseMagic(7) && ((HUtil32.GetTickCount() - m_SkillUseTick[7]) > 10000))// 攻杀剑术 
                                {
                                    m_SkillUseTick[7] = HUtil32.GetTickCount();
                                    m_boPowerHit = true;// 开启攻杀
                                    result = 7;
                                    return result;
                                }
                                if (AllowUseMagic(39) && (HUtil32.GetTickCount() - m_SkillUseTick[39] > 10000))
                                {
                                    m_SkillUseTick[39] = HUtil32.GetTickCount();// 英雄彻地钉
                                    result = 39;
                                    return result;
                                }
                                if (AllowUseMagic(grobal2.SKILL_BANWOL))// 英雄半月弯刀
                                {
                                    if (CheckTargetXYCount2(grobal2.SKILL_BANWOL) > 0)
                                    {
                                        if (!m_boUseHalfMoon)
                                        {
                                            HalfMoonOnOff(true);
                                        }
                                        result = grobal2.SKILL_BANWOL;
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
                                if (AllowUseMagic(41) && (HUtil32.GetTickCount() - m_SkillUseTick[41] > 10000) && (m_TargetCret.m_Abil.Level < m_Abil.Level) && (((m_TargetCret.m_btRaceServer != grobal2.RC_PLAYOBJECT)) || M2Share.g_Config.boGroupMbAttackPlayObject) && (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) <= 3) && (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) <= 3))
                                {
                                    m_SkillUseTick[41] = HUtil32.GetTickCount(); // 狮子吼
                                    result = 41;
                                    return result;
                                }
                                if (AllowUseMagic(7) && ((HUtil32.GetTickCount() - m_SkillUseTick[7]) > 10000))// 攻杀剑术 
                                {
                                    m_SkillUseTick[7] = HUtil32.GetTickCount();
                                    m_boPowerHit = true;
                                    result = 7;
                                    return result;
                                }
                                if (AllowUseMagic(39) && (HUtil32.GetTickCount() - m_SkillUseTick[39] > 10000))
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
                                if (AllowUseMagic(grobal2.SKILL_BANWOL))// 英雄半月弯刀
                                {
                                    if (CheckTargetXYCount2(grobal2.SKILL_BANWOL) > 0)
                                    {
                                        if (!m_boUseHalfMoon)
                                        {
                                            HalfMoonOnOff(true);
                                        }
                                        result = grobal2.SKILL_BANWOL;
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
                                if (AllowUseMagic(41) && (HUtil32.GetTickCount() - m_SkillUseTick[41] > 10000) && (m_TargetCret.m_Abil.Level < m_Abil.Level) && (((m_TargetCret.m_btRaceServer != grobal2.RC_PLAYOBJECT)) || M2Share.g_Config.boGroupMbAttackPlayObject) && (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) <= 3) && (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) <= 3))
                                {
                                    m_SkillUseTick[41] = HUtil32.GetTickCount();// 狮子吼
                                    result = 41;
                                    return result;
                                }
                                if (AllowUseMagic(39) && (HUtil32.GetTickCount() - m_SkillUseTick[39] > 10000))
                                {
                                    m_SkillUseTick[39] = HUtil32.GetTickCount();// 英雄彻地钉
                                    result = 39;
                                    return result;
                                }
                                if (AllowUseMagic(7) && ((HUtil32.GetTickCount() - m_SkillUseTick[7]) > 10000))// 攻杀剑术
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
                                if (AllowUseMagic(grobal2.SKILL_BANWOL))// 英雄半月弯刀
                                {
                                    if (CheckTargetXYCount2(grobal2.SKILL_BANWOL) > 0)
                                    {
                                        if (!m_boUseHalfMoon)
                                        {
                                            HalfMoonOnOff(true);
                                        }
                                        result = grobal2.SKILL_BANWOL;
                                        return result;
                                    }
                                }
                                if (AllowUseMagic(12))
                                {
                                    // 英雄刺杀剑术
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
                        if (((m_TargetCret.m_btRaceServer == grobal2.RC_PLAYOBJECT) || (m_TargetCret.m_Master != null)) && (CheckTargetXYCount1(m_nCurrX, m_nCurrY, 1) > 1))
                        {
                            // PK  身边超过2个目标才使用
                            if (AllowUseMagic(40) && (HUtil32.GetTickCount() - m_SkillUseTick[40] > 3000))
                            {
                                // 英雄抱月刀法
                                m_SkillUseTick[40] = HUtil32.GetTickCount();
                                if (!m_boCrsHitkill)
                                {
                                    SkillCrsOnOff(true);
                                }
                                result = 40;
                                return result;
                            }
                            if ((HUtil32.GetTickCount() - m_SkillUseTick[25] > 1500))
                            {
                                if (AllowUseMagic(grobal2.SKILL_BANWOL))
                                {
                                    // 英雄半月弯刀
                                    if (CheckTargetXYCount2(grobal2.SKILL_BANWOL) > 0)
                                    {
                                        m_SkillUseTick[25] = HUtil32.GetTickCount();
                                        if (!m_boUseHalfMoon)
                                        {
                                            HalfMoonOnOff(true);
                                        }
                                        result = grobal2.SKILL_BANWOL;
                                        return result;
                                    }
                                }
                            }
                        }
                        // 20071213增加 少于三个怪用 刺杀剑术
                        // 10 * 1000
                        if (AllowUseMagic(7) && ((HUtil32.GetTickCount() - m_SkillUseTick[7]) > 10000))
                        {
                            // 攻杀剑术
                            m_SkillUseTick[7] = HUtil32.GetTickCount();
                            m_boPowerHit = true;// 开启攻杀
                            result = 7;
                            return result;
                        }
                        if ((HUtil32.GetTickCount() - m_SkillUseTick[12] > 1000))
                        {
                            if (AllowUseMagic(12))
                            {
                                // 英雄刺杀剑术
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
                    if (AllowUseMagic(26) && (HUtil32.GetTickCount() - m_dwLatestFireHitTick > 9000))
                    {
                        // 烈火
                        m_boFireHitSkill = true;
                        result = 26;
                        return result;
                    }
                    if (AllowUseMagic(40) && (HUtil32.GetTickCount() - m_SkillUseTick[40] > 3000) && (CheckTargetXYCount1(m_nCurrX, m_nCurrY, 1) > 1))
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
                    if (AllowUseMagic(39) && (HUtil32.GetTickCount() - m_SkillUseTick[39] > 3000)) // 英雄彻地钉
                    {
                        m_SkillUseTick[39] = HUtil32.GetTickCount();
                        result = 39;
                        return result;
                    }
                    if ((HUtil32.GetTickCount() - m_SkillUseTick[25] > 3000))
                    {
                        if (AllowUseMagic(grobal2.SKILL_BANWOL))// 英雄半月弯刀
                        {
                            if (!m_boUseHalfMoon)
                            {
                                HalfMoonOnOff(true);
                            }
                            m_SkillUseTick[25] = HUtil32.GetTickCount();
                            result = grobal2.SKILL_BANWOL;
                            return result;
                        }
                    }
                    if ((HUtil32.GetTickCount() - m_SkillUseTick[12] > 3000))
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
                    if (AllowUseMagic(7) && (HUtil32.GetTickCount() - m_SkillUseTick[7] > 3000))
                    {
                        // 攻杀剑术
                        m_boPowerHit = true;
                        m_SkillUseTick[7] = HUtil32.GetTickCount();
                        result = 7;
                        return result;
                    }
                    if (((m_TargetCret.m_btRaceServer == grobal2.RC_PLAYOBJECT) || (m_TargetCret.m_Master != null)) && (m_TargetCret.m_Abil.Level < m_Abil.Level) && (m_WAbil.HP <= Math.Round(m_WAbil.MaxHP * 0.6)))
                    {
                        // PK时,使用野蛮冲撞
                        if (AllowUseMagic(27) && (HUtil32.GetTickCount() - m_SkillUseTick[27] > 3000))
                        {
                            m_SkillUseTick[27] = HUtil32.GetTickCount();
                            result = 27;
                            return result;
                        }
                    }
                    else
                    {
                        if (AllowUseMagic(27) && (m_TargetCret.m_Abil.Level < m_Abil.Level) && (m_WAbil.HP <= Math.Round(m_WAbil.MaxHP * 0.6)) && (HUtil32.GetTickCount() - m_SkillUseTick[27] > 3000))
                        {
                            m_SkillUseTick[27] = HUtil32.GetTickCount();
                            result = 27;
                            return result;
                        }
                    }
                    if (AllowUseMagic(41) && (HUtil32.GetTickCount() - m_SkillUseTick[41] > 10000) && (m_TargetCret.m_Abil.Level < m_Abil.Level) && (((m_TargetCret.m_btRaceServer != grobal2.RC_PLAYOBJECT)) || M2Share.g_Config.boGroupMbAttackPlayObject) && (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) <= 3) && (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) <= 3))
                    {
                        m_SkillUseTick[41] = HUtil32.GetTickCount();// 狮子吼
                        result = 41;
                        return result;
                    }
                    break;
                case 1: // 法师
                    if ((m_wStatusTimeArr[grobal2.STATE_BUBBLEDEFENCEUP] == 0) && !m_boAbilMagBubbleDefence) // 使用 魔法盾
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
                    if (((m_TargetCret.m_btRaceServer == grobal2.RC_PLAYOBJECT) || (m_TargetCret.m_Master != null)) && (CheckTargetXYCount3(m_nCurrX, m_nCurrY, 1, 0) > 0) && (m_TargetCret.m_WAbil.Level < m_WAbil.Level))
                    {
                        // PK时,旁边有人贴身,使用抗拒火环
                        if (AllowUseMagic(8) && ((HUtil32.GetTickCount() - m_SkillUseTick[8]) > 3000))
                        {
                            m_SkillUseTick[8] = HUtil32.GetTickCount();
                            result = 8;
                            return result;
                        }
                    }
                    else
                    {
                        // 打怪,怪级低于自己,并且有怪包围自己就用 抗拒火环
                        if (AllowUseMagic(8) && ((HUtil32.GetTickCount() - m_SkillUseTick[8]) > 3000) && (CheckTargetXYCount3(m_nCurrX, m_nCurrY, 1, 0) > 0) && (m_TargetCret.m_WAbil.Level < m_WAbil.Level))
                        {
                            m_SkillUseTick[8] = HUtil32.GetTickCount();
                            result = 8;
                            return result;
                        }
                    }
                    if (AllowUseMagic(45) && (HUtil32.GetTickCount() - m_SkillUseTick[45] > 3000))
                    {
                        m_SkillUseTick[45] = HUtil32.GetTickCount();
                        result = 45;// 英雄灭天火
                        return result;
                    }
                    if ((HUtil32.GetTickCount() - m_SkillUseTick[10] > 5000) && m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, m_btDirection, 5, ref m_nTargetX, ref m_nTargetY))
                    {
                        if (((m_TargetCret.m_btRaceServer == grobal2.RC_PLAYOBJECT) || (m_TargetCret.m_Master != null)) && (GetDirBaseObjectsCount(m_btDirection, 5) > 0) && (((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 4) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 0)) || ((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 0) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) <= 4)) || (((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 2) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 2)) || ((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 3) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 3)) || ((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 4) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 4)))))
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
                        else if ((GetDirBaseObjectsCount(m_btDirection, 5) > 1) && (((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 4) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 0)) || ((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 0) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) <= 4)) || (((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 2) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 2)) || ((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 3) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 3)) || ((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 4) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 4)))))
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
                    if (AllowUseMagic(32) && (HUtil32.GetTickCount() - m_SkillUseTick[32] > 10000) && (m_TargetCret.m_Abil.Level < M2Share.g_Config.nMagTurnUndeadLevel) && (m_TargetCret.m_btLifeAttrib == grobal2.LA_UNDEAD) && (m_TargetCret.m_WAbil.Level < m_WAbil.Level - 1))
                    {
                        // 目标为不死系
                        m_SkillUseTick[32] = HUtil32.GetTickCount();
                        result = 32;// 圣言术
                        return result;
                    }
                    if (CheckTargetXYCount(m_nCurrX, m_nCurrY, 2) > 1)
                    {
                        // 被怪物包围
                        if (AllowUseMagic(22) && (HUtil32.GetTickCount() - m_SkillUseTick[22] > 10000))
                        {
                            if ((m_TargetCret.m_btRaceServer != 101) && (m_TargetCret.m_btRaceServer != 102) && (m_TargetCret.m_btRaceServer != 104))
                            {
                                // 除祖玛怪,才放火墙
                                m_SkillUseTick[22] = HUtil32.GetTickCount();
                                result = 22;// 火墙
                                return result;
                            }
                        }
                        // 地狱雷光,只对祖玛(101,102,104)，沃玛(91,92,97)，野猪(81)系列的用   20080217
                        // 遇到祖玛的怪应该多用地狱雷光，夹杂雷电术，少用冰咆哮 20080228
                        if (new ArrayList(new int[] { 91, 92, 97, 101, 102, 104 }).Contains(m_TargetCret.m_btRaceServer))
                        {
                            // 1000 * 4
                            if (AllowUseMagic(24) && (HUtil32.GetTickCount() - m_SkillUseTick[24] > 4000) && (CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2))
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
                            else if (AllowUseMagic(33) && (CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 2) > 2))
                            {
                                result = 33;// 英雄冰咆哮
                                return result;
                            }
                            else if ((HUtil32.GetTickCount() - m_SkillUseTick[58] > 1500) && (CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2))
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
                        switch ((new System.Random(4)).Next())
                        {
                            case 0:
                                // 随机选择魔法
                                // 火球术,大火球,雷电术,爆裂火焰,英雄冰咆哮,流星火雨 从高到低选择
                                if (AllowUseMagic(92) && (HUtil32.GetTickCount() - m_SkillUseTick[58] > 1500) && (CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2))
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 92;// 四级流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(58) && (HUtil32.GetTickCount() - m_SkillUseTick[58] > 1500) && (CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2))
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 58;// 流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(33) && (CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 1))
                                {
                                    result = 33;// 英雄冰咆哮
                                    return result;
                                }
                                else if (AllowUseMagic(23) && (CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 1))
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
                                if (AllowUseMagic(92) && (HUtil32.GetTickCount() - m_SkillUseTick[58] > 1500) && (CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2))
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 92;// 四级流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(58) && (HUtil32.GetTickCount() - m_SkillUseTick[58] > 1500) && (CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2))
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 58;// 流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(33) && (CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 1))
                                {
                                    // 火球术,大火球,地狱火,爆裂火焰,冰咆哮  从高到低选择
                                    result = 33;// 冰咆哮
                                    return result;
                                }
                                else if (AllowUseMagic(23) && (CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 1))
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
                                if (AllowUseMagic(92) && (HUtil32.GetTickCount() - m_SkillUseTick[58] > 1500) && (CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2))
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 92;// 四级流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(58) && (HUtil32.GetTickCount() - m_SkillUseTick[58] > 1500) && (CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2))
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 58;// 流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(33) && (CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 1))
                                {
                                    // 火球术,大火球,地狱火,爆裂火焰 从高到低选择
                                    result = 33;
                                    return result;
                                }
                                else if (AllowUseMagic(23) && (CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 1))
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
                                    result = 44;
                                    // 寒冰掌
                                    return result;
                                }
                                if (AllowUseMagic(92) && (HUtil32.GetTickCount() - m_SkillUseTick[58] > 1500) && (CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2))
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 92;
                                    // 四级流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(58) && (HUtil32.GetTickCount() - m_SkillUseTick[58] > 1500) && (CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2))
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 58;
                                    // 流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(33) && (CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 1))
                                {
                                    // 火球术,大火球,地狱火,爆裂火焰 从高到低选择
                                    result = 33;
                                    return result;
                                }
                                else if (AllowUseMagic(23) && (CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 1))
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
                        if (AllowUseMagic(22) && (HUtil32.GetTickCount() - m_SkillUseTick[22] > 10000))
                        {
                            if ((m_TargetCret.m_btRaceServer != 101) && (m_TargetCret.m_btRaceServer != 102) && (m_TargetCret.m_btRaceServer != 104))
                            {
                                // 除祖玛怪,才放火墙
                                m_SkillUseTick[22] = HUtil32.GetTickCount();
                                result = 22;
                                return result;
                            }
                        }
                        switch ((new System.Random(4)).Next())
                        {
                            case 0:// 随机选择魔法
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
                    // 从高到低使用魔法 20080710
                    if ((HUtil32.GetTickCount() - m_SkillUseTick[58] > 1500))
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
                    if (AllowUseMagic(32) && (m_TargetCret.m_Abil.Level < M2Share.g_Config.nMagTurnUndeadLevel) && (m_TargetCret.m_btLifeAttrib == grobal2.LA_UNDEAD) && (m_TargetCret.m_WAbil.Level < m_WAbil.Level - 1))
                    {
                        // 目标为不死系
                        result = 32;// 圣言术
                        return result;
                    }
                    if (AllowUseMagic(24) && (CheckTargetXYCount(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, 3) > 2))
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
                    if (AllowUseMagic(10) && m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, m_btDirection, 5, ref m_nTargetX, ref m_nTargetY) && (((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 4) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 0)) || ((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 0) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) <= 4)) || (((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 2) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 2)) || ((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 3) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 3)) || ((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 4) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 4)))))
                    {
                        result = 10; // 英雄疾光电影
                        return result;
                    }
                    if (AllowUseMagic(9) && m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, m_btDirection, 5, ref m_nTargetX, ref m_nTargetY) && (((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 4) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 0)) || ((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 0) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) <= 4)) || (((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 2) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 2)) || ((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 3) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 3)) || ((Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) == 4) && (Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) == 4)))))
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
                        if ((m_TargetCret.m_btRaceServer != 101) && (m_TargetCret.m_btRaceServer != 102) && (m_TargetCret.m_btRaceServer != 104))// 除祖玛怪,才放火墙
                        {
                            result = 22;// 火墙
                            return result;
                        }
                    }
                    break;
                case 2:// 道士
                    if ((m_SlaveList.Count == 0) && CheckHeroAmulet(1, 5) && (HUtil32.GetTickCount() - m_SkillUseTick[17] > 3000) && (AllowUseMagic(72) || AllowUseMagic(30) || AllowUseMagic(17)) && (m_WAbil.MP > 20))
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
                        else if (AllowUseMagic(30))// 召唤神兽
                        {
                            result = 30;
                        }
                        else if (AllowUseMagic(17)) // 召唤骷髅
                        {
                            result = 17;
                        }
                        return result;
                    }
                    if ((m_wStatusTimeArr[grobal2.STATE_BUBBLEDEFENCEUP] == 0) && !m_boAbilMagBubbleDefence)
                    {
                        if (AllowUseMagic(73)) // 道力盾
                        {
                            result = 73;
                            return result;
                        }
                    }
                    if (((m_TargetCret.m_btRaceServer == grobal2.RC_PLAYOBJECT) || (m_TargetCret.m_Master != null)) && (CheckTargetXYCount3(m_nCurrX, m_nCurrY, 1, 0) > 0) && (m_TargetCret.m_WAbil.Level <= m_WAbil.Level))
                    {
                        // PK时,旁边有人贴身,使用气功波
                        // 3 * 1000
                        if (AllowUseMagic(48) && ((HUtil32.GetTickCount() - m_SkillUseTick[48]) > 3000))
                        {
                            m_SkillUseTick[48] = HUtil32.GetTickCount();
                            result = 48;
                            return result;
                        }
                    }
                    else
                    {
                        // 打怪,怪级低于自己,并且有怪包围自己就用 气功波
                        // 20090108 由3秒改到5秒
                        if (AllowUseMagic(48) && ((HUtil32.GetTickCount() - m_SkillUseTick[48]) > 5000) && (CheckTargetXYCount3(m_nCurrX, m_nCurrY, 1, 0) > 0) && (m_TargetCret.m_WAbil.Level <= m_WAbil.Level))
                        {
                            m_SkillUseTick[48] = HUtil32.GetTickCount();
                            result = 48;
                            return result;
                        }
                    }
                    // 绿毒
                    if ((m_TargetCret.m_wStatusTimeArr[grobal2.POISON_DECHEALTH] == 0) && (GetUserItemList(2, 1) >= 0) && ((M2Share.g_Config.btHeroSkillMode) || (!M2Share.g_Config.btHeroSkillMode && (m_TargetCret.m_Abil.HP >= 700)) || ((m_TargetCret.m_btRaceServer == grobal2.RC_PLAYOBJECT))) && ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) < 7) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) < 7)) && (!(new ArrayList(new int[] { 55, 79, 109, 110, 111, 128, 143, 145, 147, 151, 153, 156 }).Contains(m_TargetCret.m_btRaceServer))))
                    {
                        // 对于血量超过800的怪用  修改距离 20080704 不毒城墙
                        n_AmuletIndx = 0;
                        switch ((new System.Random(2)).Next())
                        {
                            case 0:
                                if (AllowUseMagic(38) && (HUtil32.GetTickCount() - m_SkillUseTick[38] > 1000))
                                {
                                    if (m_PEnvir != null)// 判断地图是否禁用
                                    {
                                        if (m_PEnvir.AllowMagics(grobal2.SKILL_GROUPAMYOUNSUL, 1))
                                        {
                                            m_SkillUseTick[38] = HUtil32.GetTickCount();
                                            result = grobal2.SKILL_GROUPAMYOUNSUL;// 英雄群体施毒
                                            return result;
                                        }
                                    }
                                }
                                else if ((HUtil32.GetTickCount() - m_SkillUseTick[6] > 1000))
                                {
                                    if (AllowUseMagic(grobal2.SKILL_AMYOUNSUL))
                                    {
                                        if (m_PEnvir != null)
                                        {
                                            // 判断地图是否禁用
                                            if (m_PEnvir.AllowMagics(grobal2.SKILL_AMYOUNSUL, 1))
                                            {
                                                m_SkillUseTick[6] = HUtil32.GetTickCount();
                                                result = grobal2.SKILL_AMYOUNSUL;// 英雄施毒术
                                                return result;
                                            }
                                        }
                                    }
                                }
                                break;
                            case 1:
                                if ((HUtil32.GetTickCount() - m_SkillUseTick[6] > 1000))
                                {
                                    if (AllowUseMagic(grobal2.SKILL_AMYOUNSUL))
                                    {
                                        if (m_PEnvir != null)
                                        {
                                            // 判断地图是否禁用
                                            if (m_PEnvir.AllowMagics(grobal2.SKILL_AMYOUNSUL, 1))
                                            {
                                                m_SkillUseTick[6] = HUtil32.GetTickCount();
                                                result = grobal2.SKILL_AMYOUNSUL; // 英雄施毒术
                                                return result;
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    if ((m_TargetCret.m_wStatusTimeArr[grobal2.POISON_DAMAGEARMOR] == 0) && (GetUserItemList(2, 2) >= 0) && ((M2Share.g_Config.btHeroSkillMode) || (!M2Share.g_Config.btHeroSkillMode && (m_TargetCret.m_Abil.HP >= 700)) || ((m_TargetCret.m_btRaceServer == grobal2.RC_PLAYOBJECT))) && ((Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) < 7) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) < 7)) && (!(new ArrayList(new int[] { 55, 79, 109, 110, 111, 128, 143, 145, 147, 151, 153, 156 }).Contains(m_TargetCret.m_btRaceServer))))
                    {
                        // 对于血量超过100的怪用 不毒城墙
                        n_AmuletIndx = 0;
                        switch ((new System.Random(2)).Next())
                        {
                            case 0:
                                if (AllowUseMagic(38) && (HUtil32.GetTickCount() - m_SkillUseTick[38] > 1000))
                                {
                                    if (m_PEnvir != null)
                                    {
                                        // 判断地图是否禁用
                                        if (m_PEnvir.AllowMagics(grobal2.SKILL_GROUPAMYOUNSUL, 1))
                                        {
                                            m_SkillUseTick[38] = HUtil32.GetTickCount();
                                            result = grobal2.SKILL_GROUPAMYOUNSUL; // 英雄群体施毒
                                            return result;
                                        }
                                    }
                                }
                                else if ((HUtil32.GetTickCount() - m_SkillUseTick[6] > 1000))
                                {
                                    if (AllowUseMagic(grobal2.SKILL_AMYOUNSUL))
                                    {
                                        if (m_PEnvir != null)
                                        {
                                            // 判断地图是否禁用
                                            if (m_PEnvir.AllowMagics(grobal2.SKILL_AMYOUNSUL, 1))
                                            {
                                                m_SkillUseTick[6] = HUtil32.GetTickCount();
                                                result = grobal2.SKILL_AMYOUNSUL; // 英雄施毒术
                                                return result;
                                            }
                                        }
                                    }
                                }
                                break;
                            case 1:
                                if ((HUtil32.GetTickCount() - m_SkillUseTick[6] > 1000))
                                {
                                    if (AllowUseMagic(grobal2.SKILL_AMYOUNSUL))
                                    {
                                        if (m_PEnvir != null)
                                        {
                                            // 判断地图是否禁用
                                            if (m_PEnvir.AllowMagics(grobal2.SKILL_AMYOUNSUL, 1))
                                            {
                                                m_SkillUseTick[6] = HUtil32.GetTickCount();
                                                result = grobal2.SKILL_AMYOUNSUL; // 英雄施毒术
                                                return result;
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    if (AllowUseMagic(51) && (HUtil32.GetTickCount() - m_SkillUseTick[51] > 5000))// 英雄飓风破 
                    {
                        m_SkillUseTick[51] = HUtil32.GetTickCount();
                        result = 51;
                        return result;
                    }
                    if (CheckHeroAmulet(1, 1))
                    {
                        switch ((new System.Random(3)).Next())
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
                                if (AllowUseMagic(13) && (HUtil32.GetTickCount() - m_SkillUseTick[13] > 3000))
                                {
                                    result = 13;// 英雄灵魂火符
                                    m_SkillUseTick[13] = HUtil32.GetTickCount();
                                    return result;
                                }
                                if (AllowUseMagic(52) && (m_TargetCret.m_wStatusArrValue[m_TargetCret.m_btJob + 6] == 0)) // 诅咒术
                                {
                                    result = 52;// 英雄诅咒术
                                    return result;
                                }
                                break;
                            case 1:
                                if (AllowUseMagic(52) && (m_TargetCret.m_wStatusArrValue[m_TargetCret.m_btJob + 6] == 0)) // 诅咒术
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
                                if (AllowUseMagic(13) && (HUtil32.GetTickCount() - m_SkillUseTick[13] > 3000))
                                {
                                    result = 13;// 英雄灵魂火符
                                    m_SkillUseTick[13] = HUtil32.GetTickCount();
                                    return result;
                                }
                                break;
                            case 2:
                                if (AllowUseMagic(13) && (HUtil32.GetTickCount() - m_SkillUseTick[13] > 3000))
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
                                if (AllowUseMagic(52) && (m_TargetCret.m_wStatusArrValue[m_TargetCret.m_btJob + 6] == 0))// 诅咒术
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
                        if (AllowUseMagic(52) && (m_TargetCret.m_wStatusArrValue[m_TargetCret.m_btJob + 6] == 0))// 诅咒术
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
                                if ((Math.Abs(nX - BaseObject.m_nCurrX) <= n10) && (Math.Abs(nY - BaseObject.m_nCurrY) <= n10))
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
                        case grobal2.SKILL_BANWOL:
                            n10 = (m_btDirection + M2Share.g_Config.WideAttack[nC]) % 8;
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
                        case grobal2.SKILL_BANWOL:
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
                                if ((Math.Abs(nX - BaseObject.m_nCurrX) <= n10) && (Math.Abs(nY - BaseObject.m_nCurrY) <= n10))
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

        // 走向目标
        // 参数 nType 为指定类型 1 为护身符 2 为毒药    nCount 为持久,即数量
        private bool CheckHeroAmulet(int nType, int nCount)
        {
            bool result = false;
            TUserItem UserItem;
            TItem AmuletStdItem;
            try
            {
                result = false;
                if (m_UseItems[grobal2.U_ARMRINGL].wIndex > 0)
                {
                    AmuletStdItem = M2Share.UserEngine.GetStdItem(m_UseItems[grobal2.U_ARMRINGL].wIndex);
                    if ((AmuletStdItem != null))
                    {
                        if ((AmuletStdItem.StdMode == 25))
                        {
                            switch (nType)
                            {
                                case 1:
                                    if ((AmuletStdItem.Shape == 5) && (Math.Round(Convert.ToDouble(m_UseItems[grobal2.U_ARMRINGL].Dura / 100)) >= nCount))
                                    {
                                        result = true;
                                        return result;
                                    }
                                    break;
                                case 2:
                                    if ((AmuletStdItem.Shape <= 2) && (Math.Round(Convert.ToDouble(m_UseItems[grobal2.U_ARMRINGL].Dura / 100)) >= nCount))
                                    {
                                        result = true;
                                        return result;
                                    }
                                    break;
                            }
                        }
                    }
                }
                if (m_UseItems[grobal2.U_BUJUK].wIndex > 0)
                {
                    AmuletStdItem = M2Share.UserEngine.GetStdItem(m_UseItems[grobal2.U_BUJUK].wIndex);
                    if (AmuletStdItem != null)
                    {
                        if ((AmuletStdItem.StdMode == 25))
                        {
                            switch (nType)
                            {
                                case 1: // 符
                                    if ((AmuletStdItem.Shape == 5) && (Math.Round(Convert.ToDouble(m_UseItems[grobal2.U_BUJUK].Dura / 100)) >= nCount))
                                    {
                                        result = true;
                                        return result;
                                    }
                                    break;
                                case 2: // 毒
                                    if ((AmuletStdItem.Shape <= 2) && (Math.Round(Convert.ToDouble(m_UseItems[grobal2.U_BUJUK].Dura / 100)) >= nCount))
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
                                            if ((AmuletStdItem.Shape == 5) && (HUtil32.Round(UserItem.Dura / 100) >= nCount))
                                            {
                                                result = true;
                                                return result;
                                            }
                                            break;
                                        case 2:
                                            if ((AmuletStdItem.Shape <= 2) && (HUtil32.Round(UserItem.Dura / 100) >= nCount))
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
            catch (Exception E)
            {
                M2Share.MainOutMessage("TAIPlayObject.CheckHeroAmulet");
            }
            return result;
        }

        public int GetDirBaseObjectsCount(int m_btDirection, int rang)
        {
            return 0;
        }
    }
}