using GameSvr.Actor;
using GameSvr.Conf;
using GameSvr.Items;
using GameSvr.Magic;
using GameSvr.Maps;
using GameSvr.Monster.Monsters;
using GameSvr.Player;
using System.Collections;
using SystemModule;
using SystemModule.Common;
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.RobotPlay
{
    /// <summary>
    /// 假人
    /// </summary>
    public partial class RobotPlayObject : PlayObject
    {
        public long DwTick3F4 = 0;
        public long m_dwSearchTargetTick = 0;
        /// <summary>
        /// 假人启动
        /// </summary>
        public bool m_boAIStart;
        /// <summary>
        /// 挂机地图
        /// </summary>
        public Envirnoment m_ManagedEnvir;
        public PointManager m_PointManager;
        public PointInfo[] m_Path;
        public int m_nPostion;
        public int m_nMoveFailCount;
        public string m_sConfigListFileName = string.Empty;
        public string m_sHeroConfigListFileName = string.Empty;
        public string m_sFilePath = string.Empty;
        public string m_sConfigFileName = string.Empty;
        public string m_sHeroConfigFileName = string.Empty;
        public IList<string> m_BagItemNames;
        public string[] m_UseItemNames;
        public TRunPos m_RunPos;
        /// <summary>
        /// 魔法使用间隔
        /// </summary>
        public long[] m_SkillUseTick;
        public int m_nSelItemType;
        public int m_nIncSelfHealthCount;
        public int m_nIncMasterHealthCount;
        /// <summary>
        /// 攻击方式
        /// </summary>
        public short m_wHitMode;
        public bool m_boSelSelf;
        public byte m_btTaoistUseItemType;
        public long m_dwAutoRepairItemTick;
        public long m_dwAutoAddHealthTick;
        public long m_dwThinkTick;
        public bool m_boDupMode;
        public long m_dwSearchMagic = 0;
        /// <summary>
        /// 低血回城间隔
        /// </summary>
        public long m_dwHPToMapHomeTick = 0;
        /// <summary>
        /// 守护模式
        /// </summary>
        public bool m_boProtectStatus;
        public short m_nProtectTargetX;
        public short m_nProtectTargetY;
        /// <summary>
        /// 到达守护坐标
        /// </summary>
        public bool m_boProtectOK;
        /// <summary>
        /// 是向守护坐标的累计数
        /// </summary>
        public int m_nGotoProtectXYCount;
        public long m_dwPickUpItemTick;
        public MapItem m_SelMapItem;
        /// <summary>
        /// 跑步计时
        /// </summary>
        public long dwTick5F4;
        /// <summary>
        /// 受攻击说话列表
        /// </summary>
        public ArrayList m_AISayMsgList;
        /// <summary>
        /// 绿红毒标识
        /// </summary>
        public byte n_AmuletIndx;
        public bool m_boCanPickIng;
        /// <summary>
        /// 查询魔法
        /// </summary>
        public short m_nSelectMagic;
        /// <summary>
        /// 是否可以使用的魔法(True才可能躲避)
        /// </summary>
        public bool m_boIsUseMagic;
        /// <summary>
        /// 是否可以使用的攻击魔法
        /// </summary>
        public bool m_boIsUseAttackMagic;
        /// <summary>
        /// 最后的方向
        /// </summary>
        public byte m_btLastDirection;
        /// <summary>
        /// 自动躲避间隔
        /// </summary>
        public long m_dwAutoAvoidTick;
        public bool m_boIsNeedAvoid;
        /// <summary>
        /// 假人掉装备机率
        /// </summary>
        public int m_nDropUseItemRate;
        private readonly RobotPlayConf _conf;

        public RobotPlayObject() : base()
        {
            SoftVersionDate = Grobal2.CLIENT_VERSION_NUMBER;
            SoftVersionDateEx = M2Share.GetExVersionNO(Grobal2.CLIENT_VERSION_NUMBER, ref SoftVersionDate);
            AbilCopyToWAbil();
            AttatckMode = 0;
            IsRobot = true;
            LoginNoticeOk = true;
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
            m_PointManager = new PointManager(this);
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
            WalkTick = HUtil32.GetTickCount();
            WalkSpeed = 300;
            m_RunPos = new TRunPos();
            m_Path = new PointInfo[0];
            var sFileName = GetRandomConfigFileName(ChrName, 0);
            if (sFileName == "" || !File.Exists(sFileName))
            {
                if (m_sConfigFileName != "" && File.Exists(m_sConfigFileName))
                {
                    sFileName = m_sConfigFileName;
                }
            }
            _conf = new RobotPlayConf(sFileName);
        }

        /// <summary>
        /// 取随机配置
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="nType"></param>
        /// <returns></returns>
        private string GetRandomConfigFileName(string sName, byte nType)
        {
            int nIndex;
            string sFileName;
            string Str;
            StringList LoadList;
            if (!Directory.Exists(m_sFilePath + "RobotIni"))
            {
                Directory.CreateDirectory(m_sFilePath + "RobotIni");
            }
            sFileName = Path.Combine(m_sFilePath, "RobotIni", sName + ".txt");
            string result;
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
                                    Str = Str.AsSpan()[1..].ToString();
                                }
                                if (Str[2] == '\\')
                                {
                                    Str = Str.AsSpan()[2..].ToString();
                                }
                                if (Str[3] == '\\')
                                {
                                    Str = Str.AsSpan()[3..].ToString();
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
            if (!Ghost && !Death && !m_boAIStart)
            {
                m_ManagedEnvir = Envir;
                m_nProtectTargetX = CurrX;// 守护坐标
                m_nProtectTargetY = CurrY;// 守护坐标
                m_boProtectOK = false;
                m_nGotoProtectXYCount = 0;// 是向守护坐标的累计数
                m_PointManager.PathType = PathType;
                m_PointManager.Initialize(Envir);
                m_boAIStart = true;
                m_nMoveFailCount = 0;
                if (M2Share.g_FunctionNPC != null)
                {
                    ScriptGotoCount = 0;
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
                    ScriptGotoCount = 0;
                    M2Share.g_FunctionNPC.GotoLable(this, "@AIStop", false);
                }
            }
        }

        private void WinExp(int dwExp)
        {
            if (Abil.Level > M2Share.Config.LimitExpLevel)
            {
                dwExp = M2Share.Config.LimitExpValue;
                GetExp(dwExp);
            }
            else if (dwExp > 0)
            {
                dwExp = M2Share.Config.KillMonExpMultiple * dwExp; // 系统指定杀怪经验倍数
                dwExp = MNKillMonExpMultiple * dwExp; // 人物指定的杀怪经验倍数
                dwExp = HUtil32.Round(KillMonExpRate / 100 * dwExp); // 人物指定的杀怪经验倍数
                if (Envir.Flag.boEXPRATE)
                {
                    dwExp = HUtil32.Round(Envir.Flag.nEXPRATE / 100 * dwExp); // 地图上指定杀怪经验倍数
                }
                GetExp(dwExp);
            }
        }

        private void GetExp(int dwExp)
        {
            Abil.Exp += dwExp;
            AddBodyLuck(dwExp * 0.002);
            SendMsg(this, Grobal2.RM_WINEXP, 0, dwExp, 0, 0, "");
            if (Abil.Exp >= Abil.MaxExp)
            {
                Abil.Exp -= Abil.MaxExp;
                if (Abil.Level < M2Share.MAXUPLEVEL)
                {
                    Abil.Level++;
                }
                HasLevelUp(Abil.Level - 1);
                AddBodyLuck(100);
                M2Share.EventSource.AddEventLog(12, MapName + "\t" + Abil.Level + "\t" + Abil.Exp + "\t" + ChrName + "\t" + '0' + "\t" + '0' + "\t" + '1' + "\t" + '0');
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
            PlayObject PlayObject = M2Share.WorldEngine.GetPlayObject(whostr);
            if (PlayObject != null)
            {
                if (!PlayObject.BoReadyRun)
                {
                    return;
                }
                if (!PlayObject.HearWhisper || PlayObject.IsBlockWhisper(ChrName))
                {
                    return;
                }
                if (Permission > 0)
                {
                    PlayObject.SendMsg(PlayObject, Grobal2.RM_WHISPER, 0, M2Share.Config.btGMWhisperMsgFColor, M2Share.Config.btGMWhisperMsgBColor, 0, Format("{0}[{1}级]=> {2}", new object[] { ChrName, Abil.Level, saystr }));
                    // 取得私聊信息
                    // m_GetWhisperHuman 侦听私聊对象
                    if (WhisperHuman != null && !WhisperHuman.Ghost)
                    {
                        WhisperHuman.SendMsg(WhisperHuman, Grobal2.RM_WHISPER, 0, M2Share.Config.btGMWhisperMsgFColor, M2Share.Config.btGMWhisperMsgBColor, 0, Format("{0}[{1}级]=> {2} {3}", new object[] { ChrName, Abil.Level, PlayObject.ChrName, saystr }));
                    }
                    if (PlayObject.WhisperHuman != null && !PlayObject.WhisperHuman.Ghost)
                    {
                        PlayObject.WhisperHuman.SendMsg(PlayObject.WhisperHuman, Grobal2.RM_WHISPER, 0, M2Share.Config.btGMWhisperMsgFColor, M2Share.Config.btGMWhisperMsgBColor, 0, Format("{0}[{1}级]=> {2} {3}", new object[] { ChrName, Abil.Level, PlayObject.ChrName, saystr }));
                    }
                }
                else
                {
                    PlayObject.SendMsg(PlayObject, Grobal2.RM_WHISPER, 0, M2Share.Config.btGMWhisperMsgFColor, M2Share.Config.btWhisperMsgBColor, 0, Format("{0}[{1}级]=> {2}", new object[] { ChrName, Abil.Level, saystr }));
                    if (WhisperHuman != null && !WhisperHuman.Ghost)
                    {
                        WhisperHuman.SendMsg(WhisperHuman, Grobal2.RM_WHISPER, 0, M2Share.Config.btGMWhisperMsgFColor, M2Share.Config.btWhisperMsgBColor, 0, Format("{0}[{1}级]=> {2} {3}", new object[] { ChrName, Abil.Level, PlayObject.ChrName, saystr }));
                    }
                    if (PlayObject.WhisperHuman != null && !PlayObject.WhisperHuman.Ghost)
                    {
                        PlayObject.WhisperHuman.SendMsg(PlayObject.WhisperHuman, Grobal2.RM_WHISPER, 0, M2Share.Config.btGMWhisperMsgFColor, M2Share.Config.btWhisperMsgBColor, 0, Format("{0}[{1}级]=> {2} {3}", new object[] { ChrName, Abil.Level, PlayObject.ChrName, saystr }));
                    }
                }
            }
        }

        protected override void ProcessSayMsg(string sData)
        {
            const string sExceptionMsg = "RobotPlayObject.ProcessSayMsg Msg:%s";
            if (string.IsNullOrEmpty(sData))
            {
                return;
            }
            try
            {
                var sParam1 = string.Empty;
                if (sData.Length > M2Share.Config.SayMsgMaxLen)
                {
                    sData = sData[..M2Share.Config.SayMsgMaxLen];
                }
                if (HUtil32.GetTickCount() >= DisableSayMsgTick)
                {
                    DisableSayMsg = false;
                }
                var boDisableSayMsg = DisableSayMsg;
                //g_DenySayMsgList.Lock;
                //if (g_DenySayMsgList.GetIndex(m_sChrName) >= 0)
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
                        SC = HUtil32.GetValidStr3(SC, ref sParam1, ' ');
                        if (!FilterSendMsg)
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
                                SendGroupText(ChrName + ": " + SC);
                                return;
                            }
                            if (sData[1] == '~') //发送行会消息
                            {
                                if (MyGuild != null)
                                {
                                    SC = sData.Substring(2, sData.Length - 2);
                                    MyGuild.SendGuildMsg(ChrName + ": " + SC);
                                }
                                return;
                            }
                        }
                        if (!Envir.Flag.boQUIZ) //发送黄色喊话消息
                        {
                            if ((HUtil32.GetTickCount() - ShoutMsgTick) > 10 * 1000)
                            {
                                if (Abil.Level <= M2Share.Config.CanShoutMsgLevel)
                                {
                                    SysMsg(Format(M2Share.g_sYouNeedLevelMsg, M2Share.Config.CanShoutMsgLevel + 1), MsgColor.Red, MsgType.Hint);
                                    return;
                                }
                                ShoutMsgTick = HUtil32.GetTickCount();
                                SC = sData.Substring(1, sData.Length - 1);
                                string sCryCryMsg = "(!)" + ChrName + ": " + SC;
                                if (FilterSendMsg)
                                {
                                    SendMsg(null, Grobal2.RM_CRY, 0, 0, 0xFFFF, 0, sCryCryMsg);
                                }
                                else
                                {
                                    M2Share.WorldEngine.CryCry(Grobal2.RM_CRY, Envir, CurrX, CurrY, 50, M2Share.Config.CryMsgFColor, M2Share.Config.CryMsgBColor, sCryCryMsg);
                                }
                                return;
                            }
                            SysMsg(Format(M2Share.g_sYouCanSendCyCyLaterMsg, new object[] { 10 - (HUtil32.GetTickCount() - ShoutMsgTick) / 1000 }), MsgColor.Red, MsgType.Hint);
                            return;
                        }
                        SysMsg(M2Share.g_sThisMapDisableSendCyCyMsg, MsgColor.Red, MsgType.Hint);
                        return;
                    }
                    if (!FilterSendMsg)
                    {
                        SendRefMsg(Grobal2.RM_HEAR, 0, M2Share.Config.btHearMsgFColor, M2Share.Config.btHearMsgBColor, 0, ChrName + ':' + sData);
                    }
                }
            }
            catch (Exception)
            {
                M2Share.Log.Error(Format(sExceptionMsg, new object[] { sData }));
            }
        }

        public UserMagic FindMagic(short wMagIdx)
        {
            UserMagic result = null;
            UserMagic UserMagic;
            for (var i = 0; i < MagicList.Count; i++)
            {
                UserMagic = MagicList[i];
                if (UserMagic.Magic.MagicId == wMagIdx)
                {
                    result = UserMagic;
                    break;
                }
            }
            return result;
        }

        public UserMagic FindMagic(string sMagicName)
        {
            UserMagic result = null;
            for (var i = 0; i < MagicList.Count; i++)
            {
                UserMagic UserMagic = MagicList[i];
                if (string.Compare(UserMagic.Magic.MagicName, sMagicName, StringComparison.OrdinalIgnoreCase) == 0)
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
            if ((HUtil32.GetTickCount() - dwTick5F4) > M2Share.Config.nAIRunIntervalTime)
            {
                result = RobotRunTo(M2Share.GetNextDirection(CurrX, CurrY, nX, nY), false, nX, nY);
                dwTick5F4 = HUtil32.GetTickCount();
                //m_dwStationTick = HUtil32.GetTickCount();// 增加检测人物站立时间
            }
            return result;
        }

        private bool WalkToNext(short nX, short nY)
        {
            bool result = false;
            if (HUtil32.GetTickCount() - DwTick3F4 > M2Share.Config.nAIWalkIntervalTime)
            {
                result = WalkTo(M2Share.GetNextDirection(CurrX, CurrY, nX, nY), false);
                if (result)
                {
                    DwTick3F4 = HUtil32.GetTickCount();
                }
                //m_dwStationTick = HUtil32.GetTickCount();// 增加检测人物站立时间
            }
            return result;
        }

        private bool GotoNextOne(short nX, short nY, bool boRun)
        {
            bool result = false;
            if (Math.Abs(nX - CurrX) <= 2 && Math.Abs(nY - CurrY) <= 2)
            {
                if (Math.Abs(nX - CurrX) <= 1 && Math.Abs(nY - CurrY) <= 1)
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
                    if (HUtil32.GetTickCount() >= DisableSayMsgTick)
                    {
                        DisableSayMsg = false;
                    }
                    boDisableSayMsg = DisableSayMsg;
                    // g_DenySayMsgList.Lock;
                    //if (g_DenySayMsgList.GetIndex(m_sChrName) >= 0)
                    //{
                    //    boDisableSayMsg = true;
                    //}
                    // g_DenySayMsgList.UnLock;
                    if (!boDisableSayMsg)
                    {
                        nPos = sMsg.IndexOf("=>", StringComparison.OrdinalIgnoreCase);
                        if (nPos > 0 && m_AISayMsgList.Count > 0)
                        {
                            sChrName = sMsg.Substring(1 - 1, nPos - 1);
                            sSendMsg = sMsg.Substring(nPos + 3 - 1, sMsg.Length - nPos - 2);
                            Whisper(sChrName, "你猜我是谁.");
                            //Whisper(sChrName, m_AISayMsgList[(M2Share.RandomNumber.Random(m_AISayMsgList.Count)).Next()]);
                            M2Share.Log.Error("TODO Hear...");
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

        private void SearchPickUpItem_SetHideItem(MapItem MapItem)
        {
            VisibleMapItem VisibleMapItem;
            for (var i = 0; i < VisibleItems.Count; i++)
            {
                VisibleMapItem = VisibleItems[i];
                if (VisibleMapItem != null && VisibleMapItem.VisibleFlag > 0)
                {
                    if (VisibleMapItem.MapItem == MapItem)
                    {
                        VisibleMapItem.VisibleFlag = 0;
                        break;
                    }
                }
            }
        }

        private bool SearchPickUpItem_PickUpItem(int nX, int nY)
        {
            bool result = false;
            UserItem UserItem = null;
            MapItem MapItem = Envir.GetItem(nX, nY);
            if (MapItem == null)
            {
                return result;
            }
            if (string.Compare(MapItem.Name, Grobal2.sSTRING_GOLDNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (Envir.DeleteFromMap(nX, nY, CellType.Item, MapItem) == 1)
                {
                    if (this.IncGold(MapItem.Count))
                    {
                        SendRefMsg(Grobal2.RM_ITEMHIDE, 0, MapItem.ActorId, nX, nY, "");
                        result = true;
                        GoldChanged();
                        SearchPickUpItem_SetHideItem(MapItem);
                        Dispose(MapItem);
                    }
                    else
                    {
                        Envir.AddToMap(nX, nY, CellType.Item, MapItem);
                    }
                }
                else
                {
                    Envir.AddToMap(nX, nY, CellType.Item, MapItem);
                }
            }
            else
            {
                // 捡物品
                StdItem StdItem = M2Share.WorldEngine.GetStdItem(MapItem.UserItem.Index);
                if (StdItem != null)
                {
                    if (Envir.DeleteFromMap(nX, nY, CellType.Item, MapItem) == 1)
                    {
                        UserItem = new UserItem();
                        UserItem = MapItem.UserItem;
                        StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                        if (StdItem != null && IsAddWeightAvailable(M2Share.WorldEngine.GetStdItemWeight(UserItem.Index)))
                        {
                            //if (GetCheckItemList(18, StdItem.Name))
                            //{
                            //    // 判断是否为绑定48时物品
                            //    UserItem.AddValue[0] = 2;
                            //    UserItem.MaxDate = HUtil32.IncDayHour(DateTime.Now, 48); // 解绑时间
                            //}
                            if (AddItemToBag(UserItem))
                            {
                                SendRefMsg(Grobal2.RM_ITEMHIDE, 0, MapItem.ActorId, nX, nY, "");
                                this.SendAddItem(UserItem);
                                Abil.Weight = RecalcBagWeight();
                                result = true;
                                SearchPickUpItem_SetHideItem(MapItem);
                                Dispose(MapItem);
                            }
                            else
                            {
                                Dispose(UserItem);
                                Envir.AddToMap(nX, nY, CellType.Item, MapItem);
                            }
                        }
                        else
                        {
                            Dispose(UserItem);
                            Envir.AddToMap(nX, nY, CellType.Item, MapItem);
                        }
                    }
                    else
                    {
                        Dispose(UserItem);
                        Envir.AddToMap(nX, nY, CellType.Item, MapItem);
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
            try
            {
                if ((HUtil32.GetTickCount() - m_dwPickUpItemTick) < nPickUpTime)
                {
                    return result;
                }
                m_dwPickUpItemTick = HUtil32.GetTickCount();
                if (this.IsEnoughBag() && TargetCret == null)
                {
                    boFound = false;
                    if (m_SelMapItem != null)
                    {
                        m_boCanPickIng = true;
                        for (var i = 0; i < VisibleItems.Count; i++)
                        {
                            VisibleMapItem = VisibleItems[i];
                            if (VisibleMapItem != null && VisibleMapItem.VisibleFlag > 0)
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
                        if (SearchPickUpItem_PickUpItem(CurrX, CurrY))
                        {
                            m_boCanPickIng = false;
                            result = true;
                            return result;
                        }
                    }
                    var n01 = 999;
                    VisibleMapItem SelVisibleMapItem = null;
                    boFound = false;
                    if (m_SelMapItem != null)
                    {
                        for (var i = 0; i < VisibleItems.Count; i++)
                        {
                            VisibleMapItem = VisibleItems[i];
                            if (VisibleMapItem != null && VisibleMapItem.VisibleFlag > 0)
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
                        for (var i = 0; i < VisibleItems.Count; i++)
                        {
                            VisibleMapItem = VisibleItems[i];
                            if (VisibleMapItem != null)
                            {
                                if (VisibleMapItem.VisibleFlag > 0)
                                {
                                    var mapItem = VisibleMapItem.MapItem;
                                    if (mapItem != null)
                                    {
                                        if (IsAllowAIPickUpItem(VisibleMapItem.sName) && IsAddWeightAvailable(M2Share.WorldEngine.GetStdItemWeight(mapItem.UserItem.Index)))
                                        {
                                            if (mapItem.OfBaseObject == 0 || mapItem.OfBaseObject == this.ActorId || (M2Share.ActorMgr.Get(mapItem.OfBaseObject).Master == this))
                                            {
                                                if (Math.Abs(VisibleMapItem.nX - CurrX) <= 5 && Math.Abs(VisibleMapItem.nY - CurrY) <= 5)
                                                {
                                                    var n02 = Math.Abs(VisibleMapItem.nX - CurrX) + Math.Abs(VisibleMapItem.nY - CurrY);
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
                        if (CurrX != SelVisibleMapItem.nX || CurrY != SelVisibleMapItem.nY)
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
                M2Share.Log.Error("RobotPlayObject.SearchPickUpItem");
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
            if (Transparent && HideMode)
            {
                StatusArr[PoisonState.STATE_TRANSPARENT] = 1;// 隐身,一动就显身
            }
            if (StatusArr[PoisonState.STONE] != 0 && StatusArr[PoisonState.DONTMOVE] != 0 || StatusArr[PoisonState.LOCKSPELL] != 0)
            {
                return result;// 麻痹不能跑动 
            }
            if (nTargetX != CurrX || nTargetY != CurrY)
            {
                if ((HUtil32.GetTickCount() - DwTick3F4) > MDwTurnIntervalTime)// 转向间隔
                {
                    n10 = nTargetX;
                    n14 = nTargetY;
                    byte nDir = Grobal2.DR_DOWN;
                    if (n10 > CurrX)
                    {
                        nDir = Grobal2.DR_RIGHT;
                        if (n14 > CurrY)
                        {
                            nDir = Grobal2.DR_DOWNRIGHT;
                        }
                        if (n14 < CurrY)
                        {
                            nDir = Grobal2.DR_UPRIGHT;
                        }
                    }
                    else
                    {
                        if (n10 < CurrX)
                        {
                            nDir = Grobal2.DR_LEFT;
                            if (n14 > CurrY)
                            {
                                nDir = Grobal2.DR_DOWNLEFT;
                            }
                            if (n14 < CurrY)
                            {
                                nDir = Grobal2.DR_UPLEFT;
                            }
                        }
                        else
                        {
                            if (n14 > CurrY)
                            {
                                nDir = Grobal2.DR_DOWN;
                            }
                            else if (n14 < CurrY)
                            {
                                nDir = Grobal2.DR_UP;
                            }
                        }
                    }
                    nOldX = CurrX;
                    nOldY = CurrY;
                    WalkTo(nDir, false);
                    if (nTargetX == CurrX && nTargetY == CurrY)
                    {
                        result = true;
                        DwTick3F4 = HUtil32.GetTickCount();
                    }
                    if (!result)
                    {
                        n20 = M2Share.RandomNumber.Random(3);
                        for (var i = Grobal2.DR_UP; i <= Grobal2.DR_UPLEFT; i++)
                        {
                            if (nOldX == CurrX && nOldY == CurrY)
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
                                if (nTargetX == CurrX && nTargetY == CurrY)
                                {
                                    result = true;
                                    DwTick3F4 = HUtil32.GetTickCount();
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
            if (CurrX != m_nProtectTargetX || CurrY != m_nProtectTargetY)
            {
                n10 = m_nProtectTargetX;
                n14 = m_nProtectTargetY;
                DwTick3F4 = HUtil32.GetTickCount();
                nDir = Grobal2.DR_DOWN;
                if (n10 > CurrX)
                {
                    nDir = Grobal2.DR_RIGHT;
                    if (n14 > CurrY)
                    {
                        nDir = Grobal2.DR_DOWNRIGHT;
                    }
                    if (n14 < CurrY)
                    {
                        nDir = Grobal2.DR_UPRIGHT;
                    }
                }
                else
                {
                    if (n10 < CurrX)
                    {
                        nDir = Grobal2.DR_LEFT;
                        if (n14 > CurrY)
                        {
                            nDir = Grobal2.DR_DOWNLEFT;
                        }
                        if (n14 < CurrY)
                        {
                            nDir = Grobal2.DR_UPLEFT;
                        }
                    }
                    else
                    {
                        if (n14 > CurrY)
                        {
                            nDir = Grobal2.DR_DOWN;
                        }
                        else if (n14 < CurrY)
                        {
                            nDir = Grobal2.DR_UP;
                        }
                    }
                }
                nOldX = CurrX;
                nOldY = CurrY;
                if (Math.Abs(CurrX - m_nProtectTargetX) >= 3 || Math.Abs(CurrY - m_nProtectTargetY) >= 3)
                {
                    //m_dwStationTick = HUtil32.GetTickCount();// 增加检测人物站立时间
                    if (!RobotRunTo(nDir, false, m_nProtectTargetX, m_nProtectTargetY))
                    {
                        WalkTo(nDir, false);
                        n20 = M2Share.RandomNumber.Random(3);
                        for (var i = Grobal2.DR_UP; i <= Grobal2.DR_UPLEFT; i++)
                        {
                            if (nOldX == CurrX && nOldY == CurrY)
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
                        if (nOldX == CurrX && nOldY == CurrY)
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
            if (m_boAIStart && TargetCret == null && !m_boCanPickIng && !Ghost && !Death && !FixedHideMode && !StoneMode && StatusArr[PoisonState.STONE] == 0)
            {
                var nX = CurrX;
                var nY = CurrY;
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
                    if (Math.Abs(nX - CurrX) > 2 || Math.Abs(nY - CurrY) > 2)
                    {
                        m_Path = M2Share.FindPath.Find(Envir, CurrX, CurrY, nX, nY, true);
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

        private BaseObject Struck_MINXY(BaseObject AObject, BaseObject BObject)
        {
            BaseObject result;
            int nA = Math.Abs(CurrX - AObject.CurrX) + Math.Abs(CurrY - AObject.CurrY);
            int nB = Math.Abs(CurrX - BObject.CurrX) + Math.Abs(CurrY - BObject.CurrY);
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

        private bool CanWalk(short nCurrX, short nCurrY, short nTargetX, short nTargetY, byte nDir, ref int nStep, bool boFlag)
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
                    if (Envir.GetNextPosition(nCurrX, nCurrY, btDir, 1, ref nX, ref nY) && nX == nTargetX && nY == nTargetY)
                    {
                        nStep = 1;
                        result = true;
                    }
                }
                else
                {
                    if (Envir.GetNextPosition(nCurrX, nCurrY, btDir, 2, ref nX, ref nY) && nX == nTargetX && nY == nTargetY)
                    {
                        nStep = 1;
                        result = true;
                    }
                }
            }
            else
            {
                if (Envir.GetNextPosition(nCurrX, nCurrY, btDir, 1, ref nX, ref nY) && nX == nTargetX && nY == nTargetY)
                {
                    nStep = nStep + 1;
                    return true;
                }
                if (Envir.GetNextPosition(nX, nY, btDir, 1, ref nX, ref nY) && nX == nTargetX && nY == nTargetY)
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
            if (!CanWalk(X1, Y1, X2, Y2, 0, ref nStep, Race != 108))
            {
                PointInfo[] Path = M2Share.FindPath.Find(Envir, X1, Y1, X2, Y2, false);
                if (Path.Length <= 0)
                {
                    return result;
                }

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
            if (Math.Abs(nX - CurrX) <= 2 && Math.Abs(nY - CurrY) <= 2)
            {
                if (Math.Abs(nX - CurrX) <= 1 && Math.Abs(nY - CurrY) <= 1)
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
                PointInfo[] Path = M2Share.FindPath.Find(Envir, CurrX, CurrY, nX, nY, boRun);
                if (Path.Length > 0)
                {
                    for (var i = 0; i < Path.Length; i++)
                    {
                        if (Path[i].nX != CurrX || Path[i].nY != CurrY)
                        {
                            if (Math.Abs(Path[i].nX - CurrX) >= 2 || Math.Abs(Path[i].nY - CurrY) >= 2)
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
                }
            }
            m_RunPos.nAttackCount = 0;
            return result;
        }

        protected override bool Operate(ProcessMessage ProcessMsg)
        {
            bool result = false;
            try
            {
                if (ProcessMsg.wIdent == Grobal2.RM_STRUCK)
                {
                    if (ProcessMsg.BaseObject == this.ActorId)
                    {
                        var AttackBaseObject = M2Share.ActorMgr.Get(ProcessMsg.nParam3);
                        if (AttackBaseObject != null)
                        {
                            if (AttackBaseObject.Race == ActorRace.Play)
                            {
                                SetPkFlag(AttackBaseObject);
                            }
                            SetLastHiter(AttackBaseObject);
                            Struck(AttackBaseObject);
                            BreakHolySeizeMode();
                        }
                        if (M2Share.CastleMgr.IsCastleMember(this) != null && AttackBaseObject != null)
                        {
                            if (AttackBaseObject.Race == ActorRace.Guard)
                            {
                                ((GuardUnit)AttackBaseObject).BoCrimeforCastle = true;
                                ((GuardUnit)AttackBaseObject).CrimeforCastleTime = HUtil32.GetTickCount();
                            }
                        }
                        HealthTick = 0;
                        SpellTick = 0;
                        PerHealth -= 1;
                        PerSpell -= 1;
                        StruckTick = HUtil32.GetTickCount();
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
                M2Share.Log.Error(ex.Message);
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
                if (Envir.GetNextPosition(nCurrX, nCurrY, nDir, 1, ref nCurrX, ref nCurrY))
                {
                    BaseObject BaseObject = (BaseObject)Envir.GetMovingObject(nCurrX, nCurrY, true);
                    if (BaseObject != null && !BaseObject.Death && !BaseObject.Ghost && (!BaseObject.HideMode || CoolEye) && IsProperTarget(BaseObject))
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
            BaseObject BaseObject;
            for (var n10 = 0; n10 < 7; n10++)
            {
                if (Envir.GetNextPosition(CurrX, CurrY, n10, 1, ref nX, ref nY))
                {
                    BaseObject = (BaseObject)Envir.GetMovingObject(nX, nY, true);
                    if (BaseObject != null && !BaseObject.Death && !BaseObject.Ghost && IsProperTarget(BaseObject))
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
            BaseObject BaseObject = (BaseObject)Envir.GetMovingObject(nCurrX, nCurrY, true);
            if (BaseObject != null && !BaseObject.Death && !BaseObject.Ghost && IsProperTarget(BaseObject))
            {
                result++;
            }
            for (var i = 0; i < 7; i++)
            {
                if (Envir.GetNextPosition(nCurrX, nCurrY, i, 1, ref nX, ref nY))
                {
                    BaseObject = (BaseObject)Envir.GetMovingObject(nX, nY, true);
                    if (BaseObject != null && !BaseObject.Death && !BaseObject.Ghost && IsProperTarget(BaseObject))
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        private int GetMasterRange(int nTargetX, int nTargetY)
        {
            if (Master != null)
            {
                short nCurrX = Master.CurrX;
                short nCurrY = Master.CurrY;
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
            if (!Master.SlaveRelax)
            {
                if (Envir != Master.Envir || Math.Abs(CurrX - Master.CurrX) > 20 || Math.Abs(CurrY - Master.CurrY) > 20)
                {
                    boNeed = true;
                }
            }
            if (boNeed)
            {
                Master.GetBackPosition(ref nX, ref nY);
                if (!Master.Envir.CanWalk(nX, nY, true))
                {
                    for (var i = 0; i < 7; i++)
                    {
                        if (Master.Envir.GetNextPosition(Master.CurrX, Master.CurrY, i, 1, ref nX, ref nY))
                        {
                            if (Master.Envir.CanWalk(nX, nY, true))
                            {
                                break;
                            }
                        }
                    }
                }
                DelTargetCreat();
                TargetX = nX;
                TargetY = nY;
                SpaceMove(Master.Envir.MapName, TargetX, TargetY, 1);
                return true;
            }
            Master.GetBackPosition(ref nCurrX, ref nCurrY);
            if (TargetCret == null && !Master.SlaveRelax)
            {
                for (var i = 0; i < 2; i++)
                {
                    // 判断主人是否在英雄对面
                    if (Master.Envir.GetNextPosition(Master.CurrX, Master.CurrY, Master.Direction, i, ref nX, ref nY))
                    {
                        if (CurrX == nX && CurrY == nY)
                        {
                            if (Master.GetBackPosition(ref nX, ref nY) && GotoNext(nX, nY, true))
                            {
                                return true;
                            }
                            for (var k = 0; k < 2; k++)
                            {
                                for (var j = 0; j < 7; j++)
                                {
                                    if (j != Master.Direction)
                                    {
                                        if (Master.Envir.GetNextPosition(Master.CurrX, Master.CurrY, j, k, ref nX, ref nY) && GotoNext(nX, nY, true))
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
                if (Race == 108) // 是否为月灵
                {
                    nStep = 0;
                }
                else
                {
                    nStep = 1;
                }
                if (Math.Abs(CurrX - nCurrX) > nStep || Math.Abs(CurrY - nCurrY) > nStep)
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
                            if (k != Master.Direction)
                            {
                                if (Master.Envir.GetNextPosition(Master.CurrX, Master.CurrY, k, j, ref nX, ref nY) && GotoNextOne(nX, nY, true))
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

        private bool FindVisibleActors(BaseObject ActorObject)
        {
            bool result = false;
            for (var i = 0; i < VisibleActors.Count; i++)
            {
                if (VisibleActors[i].BaseObject == ActorObject)
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
            UserMagic UserMagic = FindMagic(wMagIdx);
            if (UserMagic != null)
            {
                if (!M2Share.MagicMgr.IsWarrSkill(UserMagic.MagIdx))
                {
                    result = UserMagic.Key == 0 || IsRobot;
                }
                else
                {
                    result = UserMagic.Key == 0 || IsRobot;
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
            if (UseItems[Grobal2.U_ARMRINGL] != null && UseItems[Grobal2.U_ARMRINGL].Index > 0 &&
                Math.Round(Convert.ToDouble(UseItems[Grobal2.U_ARMRINGL].Dura / 100)) >= nCount)
            {
                StdItem StdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_ARMRINGL].Index);
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
            for (var i = 0; i < ItemList.Count; i++)
            {
                StdItem StdItem = M2Share.WorldEngine.GetStdItem(ItemList[i].Index);
                if (StdItem != null)
                {
                    if (CheckItemType(nItemType, StdItem) && HUtil32.Round(ItemList[i].Dura / 100) >= nCount)
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
            if (nIndex >= 0 && nIndex < ItemList.Count)
            {
                UserItem UserItem = ItemList[nIndex];
                if (UseItems[Grobal2.U_ARMRINGL].Index > 0)
                {
                    StdItem StdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_ARMRINGL].Index);
                    if (StdItem != null)
                    {
                        if (CheckItemType(nItemType, StdItem))
                        {
                            result = true;
                        }
                        else
                        {
                            ItemList.RemoveAt(nIndex);
                            UserItem AddUserItem = UseItems[Grobal2.U_ARMRINGL];
                            if (AddItemToBag(AddUserItem))
                            {
                                UseItems[Grobal2.U_ARMRINGL] = UserItem;
                                Dispose(UserItem);
                                result = true;
                            }
                            else
                            {
                                ItemList.Add(UserItem);
                                Dispose(AddUserItem);
                            }
                        }
                    }
                    else
                    {
                        ItemList.RemoveAt(nIndex);
                        UseItems[Grobal2.U_ARMRINGL] = UserItem;
                        Dispose(UserItem);
                        result = true;
                    }
                }
                else
                {
                    ItemList.RemoveAt(nIndex);
                    UseItems[Grobal2.U_ARMRINGL] = UserItem;
                    Dispose(UserItem);
                    result = true;
                }
            }
            return result;
        }

        private int GetRangeTargetCount(short nX, short nY, int nRange)
        {
            int result = 0;
            BaseObject BaseObject;
            IList<BaseObject> BaseObjectList = new List<BaseObject>();
            if (Envir.GetMapBaseObjects(nX, nY, nRange, BaseObjectList))
            {
                for (var i = BaseObjectList.Count - 1; i >= 0; i--)
                {
                    BaseObject = BaseObjectList[i];
                    if (BaseObject.HideMode && !CoolEye || !IsProperTarget(BaseObject))
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
            byte btDir = M2Share.GetNextDirection(nCurrX, nCurrY, TargetCret.CurrX, TargetCret.CurrY);
            while (true)
            {
                if (TargetCret.CurrX == nX && TargetCret.CurrY == nY)
                {
                    result = true;
                    break;
                }
                btDir = M2Share.GetNextDirection(nX, nY, TargetCret.CurrX, TargetCret.CurrY);
                if (!Envir.GetNextPosition(nX, nY, btDir, 1, ref nX, ref nY))
                {
                    break;
                }
                if (!Envir.CanWalkEx(nX, nY, true))
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
            short nX = CurrX;
            short nY = CurrY;
            byte btDir = M2Share.GetNextDirection(nX, nY, TargetCret.CurrX, TargetCret.CurrY);
            for (var i = 0; i < nStep; i++)
            {
                if (TargetCret.CurrX == nX && TargetCret.CurrY == nY)
                {
                    result = true;
                    break;
                }
                btDir = M2Share.GetNextDirection(nX, nY, TargetCret.CurrX, TargetCret.CurrY);
                if (!Envir.GetNextPosition(nX, nY, btDir, 1, ref nX, ref nY))
                {
                    break;
                }
                if (!Envir.CanWalkEx(nX, nY, true))
                {
                    break;
                }
            }
            return result;
        }

        private bool CanAttack(short nCurrX, short nCurrY, BaseObject BaseObject, int nRange, ref byte btDir)
        {
            bool result = false;
            short nX = 0;
            short nY = 0;
            btDir = M2Share.GetNextDirection(nCurrX, nCurrY, BaseObject.CurrX, BaseObject.CurrY);
            for (var i = 0; i < nRange; i++)
            {
                if (!Envir.GetNextPosition(nCurrX, nCurrY, btDir, i, ref nX, ref nY))
                {
                    break;
                }
                if (BaseObject.CurrX == nX && BaseObject.CurrY == nY)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private bool CanAttack(BaseObject BaseObject, int nRange, ref byte btDir)
        {
            short nX = 0;
            short nY = 0;
            bool result = false;
            btDir = M2Share.GetNextDirection(CurrX, CurrY, BaseObject.CurrX, BaseObject.CurrY);
            for (var i = 0; i < nRange; i++)
            {
                if (!Envir.GetNextPosition(CurrX, CurrY, btDir, i, ref nX, ref nY))
                {
                    break;
                }
                if (BaseObject.CurrX == nX && BaseObject.CurrY == nY)
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
            UserMagic UserMagic;
            bool result = false;
            switch (Job)
            {
                case PlayJob.Warrior:
                    result = true;
                    break;
                case PlayJob.Wizard:
                    for (var i = 0; i < MagicList.Count; i++)
                    {
                        UserMagic = MagicList[i];
                        switch (UserMagic.MagIdx)
                        {
                            case MagicConst.SKILL_FIREBALL:
                            case MagicConst.SKILL_FIREBALL2:
                            case MagicConst.SKILL_FIRE:
                            case MagicConst.SKILL_SHOOTLIGHTEN:
                            case MagicConst.SKILL_LIGHTENING:
                            case MagicConst.SKILL_EARTHFIRE:
                            case MagicConst.SKILL_FIREBOOM:
                            case MagicConst.SKILL_LIGHTFLOWER:
                            case MagicConst.SKILL_SNOWWIND:
                            case MagicConst.SKILL_GROUPLIGHTENING:
                            case MagicConst.SKILL_47:
                            case MagicConst.SKILL_58:
                                if (GetSpellPoint(UserMagic) <= Abil.MP)
                                {
                                    result = true;
                                    break;
                                }
                                break;
                        }
                    }
                    break;
                case PlayJob.Taoist:
                    for (var i = 0; i < MagicList.Count; i++)
                    {
                        UserMagic = MagicList[i];
                        if (UserMagic.Magic.Job == 2 || UserMagic.Magic.Job == 99)
                        {
                            switch (UserMagic.MagIdx)
                            {
                                case MagicConst.SKILL_AMYOUNSUL:
                                case MagicConst.SKILL_GROUPAMYOUNSUL:// 需要毒药
                                    result = CheckUserItem(1, 2) || CheckUserItem(2, 2);
                                    if (result)
                                    {
                                        result = AllowUseMagic(MagicConst.SKILL_AMYOUNSUL) || AllowUseMagic(MagicConst.SKILL_GROUPAMYOUNSUL);
                                    }
                                    if (result)
                                    {
                                        break;
                                    }
                                    break;
                                case MagicConst.SKILL_FIRECHARM:// 需要符
                                    result = CheckUserItem(5, 1);
                                    if (result)
                                    {
                                        result = AllowUseMagic(MagicConst.SKILL_FIRECHARM);
                                    }
                                    if (result)
                                    {
                                        break;
                                    }
                                    break;
                                case MagicConst.SKILL_59:// 需要符
                                    result = CheckUserItem(5, 5);
                                    if (result)
                                    {
                                        result = AllowUseMagic(MagicConst.SKILL_59);
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

        private bool UseSpell(UserMagic UserMagic, short nTargetX, short nTargetY, BaseObject TargeTBaseObject)
        {
            int n14;
            BaseObject BaseObject;
            bool boIsWarrSkill;
            bool result = false;
            if (!BoCanSpell)
            {
                return false;
            }
            if (Death || StatusArr[PoisonState.LOCKSPELL] != 0)
            {
                return false; // 防麻
            }
            if (StatusArr[PoisonState.STONE] != 0)
            {
                return false;// 防麻
            }
            if (Envir != null)
            {
                if (!Envir.AllowMagics(UserMagic.Magic.MagicName))
                {
                    return false;
                }
            }
            boIsWarrSkill = M2Share.MagicMgr.IsWarrSkill(UserMagic.MagIdx); // 是否是战士技能
            SpellTick -= 450;
            SpellTick = HUtil32._MAX(0, SpellTick);
            switch (UserMagic.MagIdx)
            {
                case MagicConst.SKILL_ERGUM:
                    if (MagicArr[MagicConst.SKILL_ERGUM] != null)
                    {
                        if (!UseThrusting)
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
                case MagicConst.SKILL_BANWOL:
                    if (MagicArr[MagicConst.SKILL_BANWOL] != null)
                    {
                        if (!UseHalfMoon)
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
                case MagicConst.SKILL_FIRESWORD:
                    if (MagicArr[MagicConst.SKILL_FIRESWORD] != null)
                    {
                        result = true;
                    }
                    break;
                case MagicConst.SKILL_MOOTEBO:
                    result = true;
                    if ((HUtil32.GetTickCount() - DoMotaeboTick) > 3000)
                    {
                        DoMotaeboTick = HUtil32.GetTickCount();
                        if (GetAttackDir(TargeTBaseObject, ref Direction))
                        {
                            DoMotaebo(Direction, UserMagic.Level);
                        }
                    }
                    break;
                case MagicConst.SKILL_43:
                    result = true;
                    break;
                default:
                    n14 = M2Share.GetNextDirection(CurrX, CurrY, nTargetX, nTargetY);
                    Direction = (byte)n14;
                    BaseObject = null;
                    if (UserMagic.MagIdx >= 60 && UserMagic.MagIdx <= 65)
                    {
                        if (CretInNearXy(TargeTBaseObject, nTargetX, nTargetY))// 检查目标角色，与目标座标误差范围，如果在误差范围内则修正目标座标
                        {
                            BaseObject = TargeTBaseObject;
                            nTargetX = BaseObject.CurrX;
                            nTargetY = BaseObject.CurrY;
                        }
                    }
                    else
                    {
                        switch (UserMagic.MagIdx)
                        {
                            case MagicConst.SKILL_HEALLING:
                            case MagicConst.SKILL_HANGMAJINBUB:
                            case MagicConst.SKILL_DEJIWONHO:
                            case MagicConst.SKILL_BIGHEALLING:
                            case MagicConst.SKILL_SINSU:
                            case MagicConst.SKILL_UNAMYOUNSUL:
                            case MagicConst.SKILL_46:
                                if (m_boSelSelf)
                                {
                                    BaseObject = this;
                                    nTargetX = CurrX;
                                    nTargetY = CurrY;
                                }
                                else
                                {
                                    if (Master != null)
                                    {
                                        BaseObject = Master;
                                        nTargetX = Master.CurrX;
                                        nTargetY = Master.CurrY;
                                    }
                                    else
                                    {
                                        BaseObject = this;
                                        nTargetX = CurrX;
                                        nTargetY = CurrY;
                                    }
                                }
                                break;
                            default:
                                if (CretInNearXy(TargeTBaseObject, nTargetX, nTargetY))
                                {
                                    BaseObject = TargeTBaseObject;
                                    nTargetX = BaseObject.CurrX;
                                    nTargetY = BaseObject.CurrY;
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

        private bool AutoSpell(UserMagic UserMagic, short nTargetX, short nTargetY, BaseObject BaseObject)
        {
            bool result = false;
            try
            {
                if (BaseObject != null)
                {
                    if (BaseObject.Ghost || BaseObject.Death || BaseObject.WAbil.HP <= 0)
                    {
                        return false;
                    }
                }
                if (!M2Share.MagicMgr.IsWarrSkill(UserMagic.MagIdx))
                {
                    result = M2Share.MagicMgr.DoSpell(this, UserMagic, nTargetX, nTargetY, BaseObject);
                    AttackTick = HUtil32.GetTickCount();
                }
            }
            catch (Exception)
            {
                M2Share.Log.Error(Format("RobotPlayObject.AutoSpell MagID:{0} X:{1} Y:{2}", new object[] { UserMagic.MagIdx, nTargetX, nTargetY }));
            }
            return result;
        }

        private bool Thinking()
        {
            bool result = false;
            try
            {
                if (M2Share.Config.RobotAutoPickUpItem)//&& (g_AllowAIPickUpItemList.Count > 0)
                {
                    if (SearchPickUpItem(500))
                    {
                        result = true;
                    }
                }
                if (Master != null && Master.Ghost)
                {
                    return false;
                }
                if (Master != null && Master.InSafeZone() && InSafeZone())
                {
                    if (Math.Abs(CurrX - Master.CurrX) <= 3 && Math.Abs(CurrY - Master.CurrY) <= 3)
                    {
                        return true;
                    }
                }
                if (HUtil32.GetTickCount() - m_dwThinkTick > 3000)
                {
                    m_dwThinkTick = HUtil32.GetTickCount();
                    if (Envir.GetXyObjCount(CurrX, CurrY) >= 2)
                    {
                        m_boDupMode = true;
                    }
                    if (TargetCret != null)
                    {
                        if (!IsProperTarget(TargetCret))
                        {
                            DelTargetCreat();
                        }
                    }
                }
                if (m_boDupMode)
                {
                    int nOldX = CurrX;
                    int nOldY = CurrY;
                    WalkTo(M2Share.RandomNumber.RandomByte(8), false);
                    //m_dwStationTick = HUtil32.GetTickCount(); // 增加检测人物站立时间
                    if (nOldX != CurrX || nOldY != CurrY)
                    {
                        m_boDupMode = false;
                        result = true;
                    }
                }
            }
            catch
            {
                M2Share.Log.Error("RobotPlayObject.Thinking");
            }
            return result;
        }

        private int CheckTargetXYCount(int nX, int nY, int nRange)
        {
            BaseObject BaseObject;
            int nC;
            int n10 = nRange;
            int result = 0;
            if (VisibleActors.Count > 0)
            {
                for (var i = 0; i < VisibleActors.Count; i++)
                {
                    BaseObject = VisibleActors[i].BaseObject;
                    if (BaseObject != null)
                    {
                        if (!BaseObject.Death)
                        {
                            if (IsProperTarget(BaseObject) && (!BaseObject.HideMode || CoolEye))
                            {
                                nC = Math.Abs(nX - BaseObject.CurrX) + Math.Abs(nY - BaseObject.CurrY);
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
            if (TargetCret != null && HUtil32.GetTickCount() - m_dwAutoAvoidTick > 1100 && (!m_boIsUseAttackMagic || Job == 0))
            {
                if (Job > 0)
                {
                    if (!m_boIsUseMagic && (Math.Abs(TargetCret.CurrX - CurrX) > 3 || Math.Abs(TargetCret.CurrY - CurrY) > 3))
                    {
                        return true;
                    }
                    if ((M2Share.Config.boHeroAttackTarget && Abil.Level < 22 || M2Share.Config.boHeroAttackTao && TargetCret.Abil.MaxHP < 700 &&
                        TargetCret.Race != ActorRace.Play && Job == PlayJob.Taoist) && (Math.Abs(TargetCret.CurrX - CurrX) > 1 || Math.Abs(TargetCret.CurrY - CurrY) > 1))// 道法22前是否物理攻击大于1格时才走向目标
                    {
                        return true;
                    }
                }
                else
                {
                    switch (m_nSelectMagic)
                    {
                        case MagicConst.SKILL_ERGUM:
                            if (AllowUseMagic(12) && Envir.GetNextPosition(CurrX, CurrY, Direction, 2, ref TargetX, ref TargetY))
                            {
                                if (Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 0 || Math.Abs(CurrX - TargetCret.CurrX) == 0 && Math.Abs(CurrY - TargetCret.CurrY) == 2 || Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 2)
                                {
                                    dwAttackTime = HUtil32._MAX(0, (int)M2Share.Config.dwHeroWarrorAttackTime - HitSpeed * M2Share.Config.ItemSpeed); // 防止负数出错
                                    if (HUtil32.GetTickCount() - AttackTick > dwAttackTime)
                                    {
                                        m_wHitMode = 4;
                                        TargetFocusTick = HUtil32.GetTickCount();
                                        Direction = M2Share.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
                                        Attack(TargetCret, Direction);
                                        BreakHolySeizeMode();
                                        AttackTick = HUtil32.GetTickCount();
                                        return result;
                                    }
                                }
                                else
                                {
                                    if (Math.Abs(TargetCret.CurrX - CurrX) > 1 || Math.Abs(TargetCret.CurrY - CurrY) > 1)
                                    {
                                        result = true;
                                        return result;
                                    }
                                }
                            }
                            m_nSelectMagic = 0;
                            if (AllowUseMagic(12))
                            {
                                if (Math.Abs(TargetCret.CurrX - CurrX) > 2 || Math.Abs(TargetCret.CurrY - CurrY) > 2)
                                {
                                    result = true;
                                    return result;
                                }
                            }
                            else if (Math.Abs(TargetCret.CurrX - CurrX) > 1 || Math.Abs(TargetCret.CurrY - CurrY) > 1)
                            {
                                result = true;
                                return result;
                            }
                            break;
                        case 43:
                            if (Envir.GetNextPosition(CurrX, CurrY, Direction, 5, ref TargetX, ref TargetY))
                            {
                                if (Math.Abs(CurrX - TargetCret.CurrX) <= 4 && Math.Abs(CurrY - TargetCret.CurrY) == 0 || Math.Abs(CurrX - TargetCret.CurrX) == 0 && Math.Abs(CurrY - TargetCret.CurrY) <= 4 || Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 2 || Math.Abs(CurrX - TargetCret.CurrX) == 3 && Math.Abs(CurrY - TargetCret.CurrY) == 3 || Math.Abs(CurrX - TargetCret.CurrX) == 4 && Math.Abs(CurrY - TargetCret.CurrY) == 4)
                                {
                                    dwAttackTime = HUtil32._MAX(0, (int)M2Share.Config.dwHeroWarrorAttackTime - HitSpeed * M2Share.Config.ItemSpeed);// 防止负数出错
                                    if (HUtil32.GetTickCount() - AttackTick > dwAttackTime)
                                    {
                                        m_wHitMode = 9;
                                        TargetFocusTick = HUtil32.GetTickCount();
                                        Direction = M2Share.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
                                        Attack(TargetCret, Direction);
                                        BreakHolySeizeMode();
                                        AttackTick = HUtil32.GetTickCount();
                                        return result;
                                    }
                                }
                                else
                                {
                                    if (AllowUseMagic(12))
                                    {
                                        if (Math.Abs(CurrX - TargetCret.CurrX) != 2 && Math.Abs(CurrY - TargetCret.CurrY) != 0 || Math.Abs(CurrX - TargetCret.CurrX) != 0 && Math.Abs(CurrY - TargetCret.CurrY) != 2 || Math.Abs(CurrX - TargetCret.CurrX) != 2 && Math.Abs(CurrY - TargetCret.CurrY) != 2)
                                        {
                                            result = true;
                                            return result;
                                        }
                                    }
                                    else if (Math.Abs(TargetCret.CurrX - CurrX) > 1 || Math.Abs(TargetCret.CurrY - CurrY) > 1)
                                    {
                                        result = true;
                                        return result;
                                    }
                                }
                            }
                            m_nSelectMagic = 0;
                            if (Envir.GetNextPosition(CurrX, CurrY, Direction, 2, ref TargetX, ref TargetY))
                            {
                                if (Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 0 || Math.Abs(CurrX - TargetCret.CurrX) == 0 && Math.Abs(CurrY - TargetCret.CurrY) == 2 || Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 2)
                                {
                                    dwAttackTime = HUtil32._MAX(0, (int)M2Share.Config.dwHeroWarrorAttackTime - HitSpeed * M2Share.Config.ItemSpeed);
                                    // 防止负数出错
                                    if (HUtil32.GetTickCount() - AttackTick > dwAttackTime)
                                    {
                                        m_wHitMode = 9;
                                        TargetFocusTick = HUtil32.GetTickCount();
                                        Direction = M2Share.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
                                        Attack(TargetCret, Direction);
                                        BreakHolySeizeMode();
                                        AttackTick = HUtil32.GetTickCount();
                                        return result;
                                    }
                                }
                                else
                                {
                                    if (Math.Abs(TargetCret.CurrX - CurrX) > 1 || Math.Abs(TargetCret.CurrY - CurrY) > 1)
                                    {
                                        result = true;
                                        return result;
                                    }
                                }
                            }
                            m_nSelectMagic = 0;
                            if (AllowUseMagic(12))
                            {
                                if (Math.Abs(TargetCret.CurrX - CurrX) > 2 || Math.Abs(TargetCret.CurrY - CurrY) > 2)
                                {
                                    result = true;
                                    return result;
                                }
                            }
                            else if (Math.Abs(TargetCret.CurrX - CurrX) > 1 || Math.Abs(TargetCret.CurrY - CurrY) > 1)
                            {
                                result = true;
                                return result;
                            }
                            break;
                        case 7:
                        case 25:
                        case 26:
                            if (Math.Abs(TargetCret.CurrX - CurrX) > 1 || Math.Abs(TargetCret.CurrY - CurrY) > 1)
                            {
                                result = true;
                                m_nSelectMagic = 0;
                                return result;
                            }
                            break;
                        default:
                            if (AllowUseMagic(12))
                            {
                                if (!(Math.Abs(TargetCret.CurrX - CurrX) == 2 && Math.Abs(TargetCret.CurrY - CurrY) == 0 || Math.Abs(TargetCret.CurrX - CurrX) == 1 && Math.Abs(TargetCret.CurrY - CurrY) == 0 || Math.Abs(TargetCret.CurrX - CurrX) == 1 && Math.Abs(TargetCret.CurrY - CurrY) == 1 || Math.Abs(TargetCret.CurrX - CurrX) == 2 && Math.Abs(TargetCret.CurrY - CurrY) == 2 || Math.Abs(TargetCret.CurrX - CurrX) == 0 && Math.Abs(TargetCret.CurrY - CurrY) == 1 || Math.Abs(TargetCret.CurrX - CurrX) == 0 && Math.Abs(TargetCret.CurrY - CurrY) == 2))
                                {
                                    result = true;
                                    return result;
                                }
                                if (Math.Abs(TargetCret.CurrX - CurrX) == 1 && Math.Abs(TargetCret.CurrY - CurrY) == 2 || Math.Abs(TargetCret.CurrX - CurrX) == 2 && Math.Abs(TargetCret.CurrY - CurrY) == 1)
                                {
                                    result = true;
                                    return result;
                                }
                            }
                            else if (Math.Abs(TargetCret.CurrX - CurrX) > 1 || Math.Abs(TargetCret.CurrY - CurrY) > 1)
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
        /// <returns></returns>
        private bool GetGotoXY(BaseObject BaseObject, byte nCode)
        {
            bool result = false;
            switch (nCode)
            {
                case 2:// 刺杀位
                    if (CurrX - 2 <= BaseObject.CurrX && CurrX + 2 >= BaseObject.CurrX && CurrY - 2 <= BaseObject.CurrY && CurrY + 2 >= BaseObject.CurrY && (CurrX != BaseObject.CurrX || CurrY != BaseObject.CurrY))
                    {
                        result = true;
                        if (CurrX - 2 == BaseObject.CurrX && CurrY == BaseObject.CurrY)
                        {
                            TargetX = (short)(CurrX - 2);
                            TargetY = CurrY;
                            return result;
                        }
                        if (CurrX + 2 == BaseObject.CurrX && CurrY == BaseObject.CurrY)
                        {
                            TargetX = (short)(CurrX + 2);
                            TargetY = CurrY;
                            return result;
                        }
                        if (CurrX == BaseObject.CurrX && CurrY - 2 == BaseObject.CurrY)
                        {
                            TargetX = CurrX;
                            TargetY = (short)(CurrY - 2);
                            return result;
                        }
                        if (CurrX == BaseObject.CurrX && CurrY + 2 == BaseObject.CurrY)
                        {
                            TargetX = CurrX;
                            TargetY = (short)(CurrY + 2);
                            return result;
                        }
                        if (CurrX - 2 == BaseObject.CurrX && CurrY - 2 == BaseObject.CurrY)
                        {
                            TargetX = (short)(CurrX - 2);
                            TargetY = (short)(CurrY - 2);
                            return result;
                        }
                        if (CurrX + 2 == BaseObject.CurrX && CurrY - 2 == BaseObject.CurrY)
                        {
                            TargetX = (short)(CurrX + 2);
                            TargetY = (short)(CurrY - 2);
                            return result;
                        }
                        if (CurrX - 2 == BaseObject.CurrX && CurrY + 2 == BaseObject.CurrY)
                        {
                            TargetX = (short)(CurrX - 2);
                            TargetY = (short)(CurrY + 2);
                            return result;
                        }
                        if (CurrX + 2 == BaseObject.CurrX && CurrY + 2 == BaseObject.CurrY)
                        {
                            TargetX = (short)(CurrX + 2);
                            TargetY = (short)(CurrY + 2);
                            return result;
                        }
                    }
                    break;
                case 3:// 3格
                    if (CurrX - 3 <= BaseObject.CurrX && CurrX + 3 >= BaseObject.CurrX && CurrY - 3 <= BaseObject.CurrY && CurrY + 3 >= BaseObject.CurrY && (CurrX != BaseObject.CurrX || CurrY != BaseObject.CurrY))
                    {
                        result = true;
                        if (CurrX - 3 == BaseObject.CurrX && CurrY == BaseObject.CurrY)
                        {
                            TargetX = (short)(CurrX - 3);
                            TargetY = CurrY;
                            return result;
                        }
                        if (CurrX + 3 == BaseObject.CurrX && CurrY == BaseObject.CurrY)
                        {
                            TargetX = (short)(CurrX + 3);
                            TargetY = CurrY;
                            return result;
                        }
                        if (CurrX == BaseObject.CurrX && CurrY - 3 == BaseObject.CurrY)
                        {
                            TargetX = CurrX;
                            TargetY = (short)(CurrY - 3);
                            return result;
                        }
                        if (CurrX == BaseObject.CurrX && CurrY + 3 == BaseObject.CurrY)
                        {
                            TargetX = CurrX;
                            TargetY = (short)(CurrY + 3);
                            return result;
                        }
                        if (CurrX - 3 == BaseObject.CurrX && CurrY - 3 == BaseObject.CurrY)
                        {
                            TargetX = (short)(CurrX - 3);
                            TargetY = (short)(CurrY - 3);
                            return result;
                        }
                        if (CurrX + 3 == BaseObject.CurrX && CurrY - 3 == BaseObject.CurrY)
                        {
                            TargetX = (short)(CurrX + 3);
                            TargetY = (short)(CurrY - 3);
                            return result;
                        }
                        if (CurrX - 3 == BaseObject.CurrX && CurrY + 3 == BaseObject.CurrY)
                        {
                            TargetX = (short)(CurrX - 3);
                            TargetY = (short)(CurrY + 3);
                            return result;
                        }
                        if (CurrX + 3 == BaseObject.CurrX && CurrY + 3 == BaseObject.CurrY)
                        {
                            TargetX = (short)(CurrX + 3);
                            TargetY = (short)(CurrY + 3);
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
        /// <returns></returns>
        private bool RunToTargetXY(short nTargetX, short nTargetY)
        {
            bool result = false;
            if (Transparent && HideMode)
            {
                StatusArr[PoisonState.STATE_TRANSPARENT] = 1;// 隐身,一动就显身
            }
            if (StatusArr[PoisonState.STONE] > 0 && StatusArr[PoisonState.DONTMOVE] != 0 || StatusArr[PoisonState.LOCKSPELL] != 0)// || (m_wStatusArrValue[23] != 0)
            {
                return false; // 麻痹不能跑动 
            }
            if (!BoCanRun) // 禁止跑,则退出
            {
                return false;
            }
            if (HUtil32.GetTickCount() - dwTick5F4 > MDwRunIntervalTime) // 跑步使用单独的变量计数
            {
                var nX = nTargetX;
                var nY = nTargetY;
                byte nDir = M2Share.GetNextDirection(CurrX, CurrY, nX, nY);
                if (!RobotRunTo(nDir, false, nTargetX, nTargetY))
                {
                    result = WalkToTargetXY(nTargetX, nTargetY);
                    if (result)
                    {
                        dwTick5F4 = HUtil32.GetTickCount();
                    }
                }
                else
                {
                    if (Math.Abs(nTargetX - CurrX) <= 1 && Math.Abs(nTargetY - CurrY) <= 1)
                    {
                        result = true;
                        dwTick5F4 = HUtil32.GetTickCount();
                    }
                }
            }
            return result;
        }

        private bool RobotRunTo(byte btDir, bool boFlag, short nDestX, short nDestY)
        {
            const string sExceptionMsg = "[Exception] TBaseObject::RunTo";
            var result = false;
            try
            {
                int nOldX = CurrX;
                int nOldY = CurrY;
                Direction = btDir;
                var canWalk = M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll || M2Share.Config.boSafeAreaLimited && InSafeZone();
                switch (btDir)
                {
                    case Grobal2.DR_UP:
                        if (CurrY > 1 && Envir.CanWalkEx(CurrX, CurrY - 1, canWalk) && Envir.CanWalkEx(CurrX, CurrY - 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX, CurrY - 2, true) > 0)
                        {
                            CurrY -= 2;
                        }
                        break;
                    case Grobal2.DR_UPRIGHT:
                        if (CurrX < Envir.Width - 2 && CurrY > 1 && Envir.CanWalkEx(CurrX + 1, CurrY - 1, canWalk) && Envir.CanWalkEx(CurrX + 2, CurrY - 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 2, CurrY - 2, true) > 0)
                        {
                            CurrX += 2;
                            CurrY -= 2;
                        }
                        break;
                    case Grobal2.DR_RIGHT:
                        if (CurrX < Envir.Width - 2 && Envir.CanWalkEx(CurrX + 1, CurrY, canWalk) && Envir.CanWalkEx(CurrX + 2, CurrY, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 2, CurrY, true) > 0)
                        {
                            CurrX += 2;
                        }
                        break;
                    case Grobal2.DR_DOWNRIGHT:
                        if (CurrX < Envir.Width - 2 && CurrY < Envir.Height - 2 && Envir.CanWalkEx(CurrX + 1, CurrY + 1, canWalk) && Envir.CanWalkEx(CurrX + 2, CurrY + 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 2, CurrY + 2, true) > 0)
                        {
                            CurrX += 2;
                            CurrY += 2;
                        }
                        break;
                    case Grobal2.DR_DOWN:
                        if (CurrY < Envir.Height - 2 && Envir.CanWalkEx(CurrX, CurrY + 1, canWalk) && Envir.CanWalkEx(CurrX, CurrY + 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX, CurrY + 2, true) > 0)
                        {
                            CurrY += 2;
                        }
                        break;
                    case Grobal2.DR_DOWNLEFT:
                        if (CurrX > 1 && CurrY < Envir.Height - 2 && Envir.CanWalkEx(CurrX - 1, CurrY + 1, canWalk) && Envir.CanWalkEx(CurrX - 2, CurrY + 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 2, CurrY + 2, true) > 0)
                        {
                            CurrX -= 2;
                            CurrY += 2;
                        }

                        break;
                    case Grobal2.DR_LEFT:
                        if (CurrX > 1 && Envir.CanWalkEx(CurrX - 1, CurrY, canWalk) && Envir.CanWalkEx(CurrX - 2, CurrY, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 2, CurrY, true) > 0)
                        {
                            CurrX -= 2;
                        }
                        break;
                    case Grobal2.DR_UPLEFT:
                        if (CurrX > 1 && CurrY > 1 && Envir.CanWalkEx(CurrX - 1, CurrY - 1, canWalk) && Envir.CanWalkEx(CurrX - 2, CurrY - 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 2, CurrY - 2, true) > 0)
                        {
                            CurrX -= 2;
                            CurrY -= 2;
                        }
                        break;
                }
                if (CurrX != nOldX || CurrY != nOldY)
                {
                    if (Walk(Grobal2.RM_RUN))
                    {
                        result = true;
                    }
                    else
                    {
                        CurrX = (short)nOldX;
                        CurrY = (short)nOldY;
                        Envir.MoveToMovingObject(nOldX, nOldY, this, CurrX, CurrX, true);
                    }
                }
            }
            catch
            {
                M2Share.Log.Error(sExceptionMsg);
            }
            return result;
        }

        /// <summary>
        /// 走向目标
        /// </summary>
        /// <returns></returns>
        private bool WalkToTargetXY(int nTargetX, int nTargetY)
        {
            int n10;
            int n14;
            int n20;
            int nOldX;
            int nOldY;
            bool result = false;
            if (Transparent && HideMode)
            {
                StatusArr[PoisonState.STATE_TRANSPARENT] = 1;// 隐身,一动就显身
            }
            if (StatusArr[PoisonState.STONE] != 0 && StatusArr[PoisonState.DONTMOVE] != 0 || StatusArr[PoisonState.LOCKSPELL] != 0)
            {
                return false;// 麻痹不能跑动
            }
            if (Math.Abs(nTargetX - CurrX) > 1 || Math.Abs(nTargetY - CurrY) > 1)
            {
                if (HUtil32.GetTickCount() - DwTick3F4 > MDwWalkIntervalTime)
                {
                    n10 = nTargetX;
                    n14 = nTargetY;
                    byte nDir = Grobal2.DR_DOWN;
                    if (n10 > CurrX)
                    {
                        nDir = Grobal2.DR_RIGHT;
                        if (n14 > CurrY)
                        {
                            nDir = Grobal2.DR_DOWNRIGHT;
                        }
                        if (n14 < CurrY)
                        {
                            nDir = Grobal2.DR_UPRIGHT;
                        }
                    }
                    else
                    {
                        if (n10 < CurrX)
                        {
                            nDir = Grobal2.DR_LEFT;
                            if (n14 > CurrY)
                            {
                                nDir = Grobal2.DR_DOWNLEFT;
                            }
                            if (n14 < CurrY)
                            {
                                nDir = Grobal2.DR_UPLEFT;
                            }
                        }
                        else
                        {
                            if (n14 > CurrY)
                            {
                                nDir = Grobal2.DR_DOWN;
                            }
                            else if (n14 < CurrY)
                            {
                                nDir = Grobal2.DR_UP;
                            }
                        }
                    }
                    nOldX = CurrX;
                    nOldY = CurrY;
                    WalkTo(nDir, false);
                    if (Math.Abs(nTargetX - CurrX) <= 1 && Math.Abs(nTargetY - CurrY) <= 1)
                    {
                        result = true;
                        DwTick3F4 = HUtil32.GetTickCount();
                    }
                    if (!result)
                    {
                        n20 = M2Share.RandomNumber.Random(3);
                        for (var i = Grobal2.DR_UP; i <= Grobal2.DR_UPLEFT; i++)
                        {
                            if (nOldX == CurrX && nOldY == CurrY)
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
                                if (Math.Abs(nTargetX - CurrX) <= 1 && Math.Abs(nTargetY - CurrY) <= 1)
                                {
                                    result = true;
                                    DwTick3F4 = HUtil32.GetTickCount();
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
                    if (Math.Abs(CurrX - nTargetX) > 2 || Math.Abs(CurrY - nTargetY) > 2)
                    {
                        if (StatusArr[PoisonState.LOCKRUN] == 0)
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
                    if (Math.Abs(CurrX - nTargetX) > 1 || Math.Abs(CurrY - nTargetY) > 1)
                    {
                        if (StatusArr[PoisonState.LOCKRUN] == 0)
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
                UserMagic UserMagic = FindMagic(m_nSelectMagic);
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
            switch (Job)
            {
                case PlayJob.Warrior:
                    if (AllowUseMagic(26) && HUtil32.GetTickCount() - LatestFireHitTick > 9000)// 烈火
                    {
                        FireHitSkill = true;
                        result = 26;
                        return result;
                    }
                    if ((TargetCret.Race == ActorRace.Play || TargetCret.Master != null) && TargetCret.Abil.Level < Abil.Level)
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
                        if (AllowUseMagic(27) && HUtil32.GetTickCount() - m_SkillUseTick[27] > 10000 && TargetCret.Abil.Level < Abil.Level && WAbil.HP <= Math.Round(Abil.MaxHP * 0.85))
                        {
                            m_SkillUseTick[27] = HUtil32.GetTickCount();
                            result = 27;
                            return result;
                        }
                    }
                    if (TargetCret.Master != null)
                    {
                        ExpHitter = TargetCret.Master;
                    }
                    if (CheckTargetXYCount1(CurrX, CurrY, 1) > 1)
                    {
                        switch (M2Share.RandomNumber.Random(3))
                        {
                            case 0:// 被怪物包围
                                if (AllowUseMagic(41) && HUtil32.GetTickCount() - m_SkillUseTick[41] > 10000 && TargetCret.Abil.Level < Abil.Level && (TargetCret.Race != ActorRace.Play || M2Share.Config.GroupMbAttackPlayObject) && Math.Abs(TargetCret.CurrX - CurrX) <= 3 && Math.Abs(TargetCret.CurrY - CurrY) <= 3)
                                {
                                    m_SkillUseTick[41] = HUtil32.GetTickCount();// 狮子吼
                                    result = 41;
                                    return result;
                                }
                                if (AllowUseMagic(7) && HUtil32.GetTickCount() - m_SkillUseTick[7] > 10000)// 攻杀剑术 
                                {
                                    m_SkillUseTick[7] = HUtil32.GetTickCount();
                                    PowerHit = true;// 开启攻杀
                                    result = 7;
                                    return result;
                                }
                                if (AllowUseMagic(39) && HUtil32.GetTickCount() - m_SkillUseTick[39] > 10000)
                                {
                                    m_SkillUseTick[39] = HUtil32.GetTickCount();// 彻地钉
                                    result = 39;
                                    return result;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_BANWOL))// 半月弯刀
                                {
                                    if (CheckTargetXYCount2(MagicConst.SKILL_BANWOL) > 0)
                                    {
                                        if (!UseHalfMoon)
                                        {
                                            HalfMoonOnOff(true);
                                        }
                                        result = MagicConst.SKILL_BANWOL;
                                        return result;
                                    }
                                }
                                if (AllowUseMagic(40))// 英雄抱月刀法
                                {
                                    if (!CrsHitkill)
                                    {
                                        SkillCrsOnOff(true);
                                    }
                                    result = 40;
                                    return result;
                                }
                                if (AllowUseMagic(12))// 英雄刺杀剑术
                                {
                                    if (!UseThrusting)
                                    {
                                        ThrustingOnOff(true);
                                    }
                                    result = 12;
                                    return result;
                                }
                                break;
                            case 1:
                                if (AllowUseMagic(41) && HUtil32.GetTickCount() - m_SkillUseTick[41] > 10000 && TargetCret.Abil.Level < Abil.Level && (TargetCret.Race != ActorRace.Play || M2Share.Config.GroupMbAttackPlayObject) && Math.Abs(TargetCret.CurrX - CurrX) <= 3 && Math.Abs(TargetCret.CurrY - CurrY) <= 3)
                                {
                                    m_SkillUseTick[41] = HUtil32.GetTickCount(); // 狮子吼
                                    result = 41;
                                    return result;
                                }
                                if (AllowUseMagic(7) && HUtil32.GetTickCount() - m_SkillUseTick[7] > 10000)// 攻杀剑术 
                                {
                                    m_SkillUseTick[7] = HUtil32.GetTickCount();
                                    PowerHit = true;
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
                                    if (!CrsHitkill)
                                    {
                                        SkillCrsOnOff(true);
                                    }
                                    result = 40;
                                    return result;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_BANWOL))// 半月弯刀
                                {
                                    if (CheckTargetXYCount2(MagicConst.SKILL_BANWOL) > 0)
                                    {
                                        if (!UseHalfMoon)
                                        {
                                            HalfMoonOnOff(true);
                                        }
                                        result = MagicConst.SKILL_BANWOL;
                                        return result;
                                    }
                                }
                                if (AllowUseMagic(12))// 英雄刺杀剑术
                                {
                                    if (!UseThrusting)
                                    {
                                        ThrustingOnOff(true);
                                    }
                                    result = 12;
                                    return result;
                                }
                                break;
                            case 2:
                                if (AllowUseMagic(41) && HUtil32.GetTickCount() - m_SkillUseTick[41] > 10000 && TargetCret.Abil.Level < Abil.Level && (TargetCret.Race != ActorRace.Play || M2Share.Config.GroupMbAttackPlayObject) && Math.Abs(TargetCret.CurrX - CurrX) <= 3 && Math.Abs(TargetCret.CurrY - CurrY) <= 3)
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
                                    PowerHit = true;//  开启攻杀
                                    result = 7;
                                    return result;
                                }
                                if (AllowUseMagic(40))// 英雄抱月刀法
                                {
                                    if (!CrsHitkill)
                                    {
                                        SkillCrsOnOff(true);
                                    }
                                    result = 40;
                                    return result;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_BANWOL))// 半月弯刀
                                {
                                    if (CheckTargetXYCount2(MagicConst.SKILL_BANWOL) > 0)
                                    {
                                        if (!UseHalfMoon)
                                        {
                                            HalfMoonOnOff(true);
                                        }
                                        result = MagicConst.SKILL_BANWOL;
                                        return result;
                                    }
                                }
                                if (AllowUseMagic(12)) // 英雄刺杀剑术
                                {
                                    if (!UseThrusting)
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
                        if ((TargetCret.Race == ActorRace.Play || TargetCret.Master != null) && CheckTargetXYCount1(CurrX, CurrY, 1) > 1)
                        {
                            // PK  身边超过2个目标才使用
                            if (AllowUseMagic(40) && (HUtil32.GetTickCount() - m_SkillUseTick[40]) > 3000)// 英雄抱月刀法
                            {
                                m_SkillUseTick[40] = HUtil32.GetTickCount();
                                if (!CrsHitkill)
                                {
                                    SkillCrsOnOff(true);
                                }
                                result = 40;
                                return result;
                            }
                            if ((HUtil32.GetTickCount() - m_SkillUseTick[25]) > 1500)
                            {
                                if (AllowUseMagic(MagicConst.SKILL_BANWOL))
                                {
                                    // 半月弯刀
                                    if (CheckTargetXYCount2(MagicConst.SKILL_BANWOL) > 0)
                                    {
                                        m_SkillUseTick[25] = HUtil32.GetTickCount();
                                        if (!UseHalfMoon)
                                        {
                                            HalfMoonOnOff(true);
                                        }
                                        result = MagicConst.SKILL_BANWOL;
                                        return result;
                                    }
                                }
                            }
                        }
                        if (AllowUseMagic(7) && (HUtil32.GetTickCount() - m_SkillUseTick[7]) > 10000) // 少于三个怪用 刺杀剑术
                        {
                            m_SkillUseTick[7] = HUtil32.GetTickCount();
                            PowerHit = true;// 开启攻杀
                            result = 7;
                            return result;
                        }
                        if (HUtil32.GetTickCount() - m_SkillUseTick[12] > 1000)
                        {
                            if (AllowUseMagic(12))// 英雄刺杀剑术
                            {
                                if (!UseThrusting)
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
                    if (AllowUseMagic(26) && (HUtil32.GetTickCount() - LatestFireHitTick) > 9000)// 烈火
                    {
                        FireHitSkill = true;
                        result = 26;
                        return result;
                    }
                    if (AllowUseMagic(40) && (HUtil32.GetTickCount() - m_SkillUseTick[40]) > 3000 && CheckTargetXYCount1(CurrX, CurrY, 1) > 1)
                    {
                        // 英雄抱月刀法
                        if (!CrsHitkill)
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
                        if (AllowUseMagic(MagicConst.SKILL_BANWOL))// 半月弯刀
                        {
                            if (!UseHalfMoon)
                            {
                                HalfMoonOnOff(true);
                            }
                            m_SkillUseTick[25] = HUtil32.GetTickCount();
                            result = MagicConst.SKILL_BANWOL;
                            return result;
                        }
                    }
                    if ((HUtil32.GetTickCount() - m_SkillUseTick[12]) > 3000)// 英雄刺杀剑术
                    {
                        if (AllowUseMagic(12))
                        {
                            if (!UseThrusting)
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
                        PowerHit = true;
                        m_SkillUseTick[7] = HUtil32.GetTickCount();
                        result = 7;
                        return result;
                    }
                    if ((TargetCret.Race == ActorRace.Play || TargetCret.Master != null) && TargetCret.Abil.Level < Abil.Level && WAbil.HP <= Math.Round(WAbil.MaxHP * 0.6))
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
                        if (AllowUseMagic(27) && TargetCret.Abil.Level < Abil.Level && WAbil.HP <= Math.Round(Abil.MaxHP * 0.6) && HUtil32.GetTickCount() - m_SkillUseTick[27] > 3000)
                        {
                            m_SkillUseTick[27] = HUtil32.GetTickCount();
                            result = 27;
                            return result;
                        }
                    }
                    if (AllowUseMagic(41) && HUtil32.GetTickCount() - m_SkillUseTick[41] > 10000 && TargetCret.Abil.Level < Abil.Level && (TargetCret.Race != ActorRace.Play || M2Share.Config.GroupMbAttackPlayObject) && Math.Abs(TargetCret.CurrX - CurrX) <= 3 && Math.Abs(TargetCret.CurrY - CurrY) <= 3)
                    {
                        m_SkillUseTick[41] = HUtil32.GetTickCount();// 狮子吼
                        result = 41;
                        return result;
                    }
                    break;
                case PlayJob.Wizard: // 法师
                    if (StatusArr[PoisonState.BUBBLEDEFENCEUP] == 0 && !AbilMagBubbleDefence) // 使用 魔法盾
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
                    if ((TargetCret.Race == ActorRace.Play || TargetCret.Master != null) && CheckTargetXYCount3(CurrX, CurrY, 1, 0) > 0 && TargetCret.Abil.Level < Abil.Level)
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
                        if (AllowUseMagic(8) && HUtil32.GetTickCount() - m_SkillUseTick[8] > 3000 && CheckTargetXYCount3(CurrX, CurrY, 1, 0) > 0 && TargetCret.Abil.Level < Abil.Level)
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
                    if (HUtil32.GetTickCount() - m_SkillUseTick[10] > 5000 && Envir.GetNextPosition(CurrX, CurrY, Direction, 5, ref TargetX, ref TargetY))
                    {
                        if ((TargetCret.Race == ActorRace.Play || TargetCret.Master != null) && GetDirBaseObjectsCount(Direction, 5) > 0 && (Math.Abs(CurrX - TargetCret.CurrX) <= 4 && Math.Abs(CurrY - TargetCret.CurrY) == 0 || Math.Abs(CurrX - TargetCret.CurrX) == 0 && Math.Abs(CurrY - TargetCret.CurrY) <= 4 || Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 2 || Math.Abs(CurrX - TargetCret.CurrX) == 3 && Math.Abs(CurrY - TargetCret.CurrY) == 3 || Math.Abs(CurrX - TargetCret.CurrX) == 4 && Math.Abs(CurrY - TargetCret.CurrY) == 4))
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
                        else if (GetDirBaseObjectsCount(Direction, 5) > 1 && (Math.Abs(CurrX - TargetCret.CurrX) <= 4 && Math.Abs(CurrY - TargetCret.CurrY) == 0 || Math.Abs(CurrX - TargetCret.CurrX) == 0 && Math.Abs(CurrY - TargetCret.CurrY) <= 4 || Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 2 || Math.Abs(CurrX - TargetCret.CurrX) == 3 && Math.Abs(CurrY - TargetCret.CurrY) == 3 || Math.Abs(CurrX - TargetCret.CurrX) == 4 && Math.Abs(CurrY - TargetCret.CurrY) == 4))
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
                    if (AllowUseMagic(32) && HUtil32.GetTickCount() - m_SkillUseTick[32] > 10000 && TargetCret.Abil.Level < M2Share.Config.MagTurnUndeadLevel && TargetCret.LifeAttrib == Grobal2.LA_UNDEAD && TargetCret.Abil.Level < Abil.Level - 1)
                    {
                        // 目标为不死系
                        m_SkillUseTick[32] = HUtil32.GetTickCount();
                        result = 32;// 圣言术
                        return result;
                    }
                    if (CheckTargetXYCount(CurrX, CurrY, 2) > 1)// 被怪物包围
                    {
                        if (AllowUseMagic(22) && (HUtil32.GetTickCount() - m_SkillUseTick[22]) > 10000)
                        {
                            if (TargetCret.Race != 101 && TargetCret.Race != 102 && TargetCret.Race != 104) // 除祖玛怪,才放火墙
                            {
                                m_SkillUseTick[22] = HUtil32.GetTickCount();
                                result = 22;// 火墙
                                return result;
                            }
                        }
                        // 地狱雷光,只对祖玛(101,102,104)，沃玛(91,92,97)，野猪(81)系列的
                        // 遇到祖玛的怪应该多用地狱雷光，夹杂雷电术，少用冰咆哮
                        if (new ArrayList(new byte[] { 91, 92, 97, 101, 102, 104 }).Contains(TargetCret.Race))
                        {
                            // 1000 * 4
                            if (AllowUseMagic(24) && (HUtil32.GetTickCount() - m_SkillUseTick[24]) > 4000 && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
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
                            else if (AllowUseMagic(33) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 2) > 2)
                            {
                                result = 33;// 英雄冰咆哮
                                return result;
                            }
                            else if (HUtil32.GetTickCount() - m_SkillUseTick[58] > 1500 && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
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
                                if (AllowUseMagic(92) && (HUtil32.GetTickCount() - m_SkillUseTick[58]) > 1500 && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 92;// 四级流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(58) && (HUtil32.GetTickCount() - m_SkillUseTick[58]) > 1500 && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 58;// 流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(33) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)
                                {
                                    result = 33;// 英雄冰咆哮
                                    return result;
                                }
                                else if (AllowUseMagic(23) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)
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
                                if (AllowUseMagic(92) && (HUtil32.GetTickCount() - m_SkillUseTick[58]) > 1500 && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 92;// 四级流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(58) && (HUtil32.GetTickCount() - m_SkillUseTick[58]) > 1500 && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 58;// 流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(33) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)
                                {
                                    // 火球术,大火球,地狱火,爆裂火焰,冰咆哮  从高到低选择
                                    result = 33;// 冰咆哮
                                    return result;
                                }
                                else if (AllowUseMagic(23) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)
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
                                if (AllowUseMagic(92) && (HUtil32.GetTickCount() - m_SkillUseTick[58]) > 1500 && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 92;// 四级流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(58) && (HUtil32.GetTickCount() - m_SkillUseTick[58]) > 1500 && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 58;// 流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(33) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)
                                {
                                    // 火球术,大火球,地狱火,爆裂火焰 从高到低选择
                                    result = 33;
                                    return result;
                                }
                                else if (AllowUseMagic(23) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)
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
                                if (AllowUseMagic(92) && (HUtil32.GetTickCount() - m_SkillUseTick[58] > 1500) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 92; // 四级流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(58) && (HUtil32.GetTickCount() - m_SkillUseTick[58]) > 1500 && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    m_SkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 58; // 流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(33) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)// 火球术,大火球,地狱火,爆裂火焰 从高到低选择
                                {
                                    result = 33;
                                    return result;
                                }
                                else if (AllowUseMagic(23) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)
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
                            if (TargetCret.Race != 101 && TargetCret.Race != 102 && TargetCret.Race != 104)// 除祖玛怪,才放火墙
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
                    if (AllowUseMagic(32) && TargetCret.Abil.Level < M2Share.Config.MagTurnUndeadLevel && TargetCret.LifeAttrib == Grobal2.LA_UNDEAD && TargetCret.Abil.Level < Abil.Level - 1)
                    {
                        // 目标为不死系
                        result = 32;// 圣言术
                        return result;
                    }
                    if (AllowUseMagic(24) && CheckTargetXYCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
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
                    if (AllowUseMagic(10) && Envir.GetNextPosition(CurrX, CurrY, Direction, 5, ref TargetX, ref TargetY) && (Math.Abs(CurrX - TargetCret.CurrX) <= 4 && Math.Abs(CurrY - TargetCret.CurrY) == 0 || Math.Abs(CurrX - TargetCret.CurrX) == 0 && Math.Abs(CurrY - TargetCret.CurrY) <= 4 || Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 2 || Math.Abs(CurrX - TargetCret.CurrX) == 3 && Math.Abs(CurrY - TargetCret.CurrY) == 3 || Math.Abs(CurrX - TargetCret.CurrX) == 4 && Math.Abs(CurrY - TargetCret.CurrY) == 4))
                    {
                        result = 10; // 英雄疾光电影
                        return result;
                    }
                    if (AllowUseMagic(9) && Envir.GetNextPosition(CurrX, CurrY, Direction, 5, ref TargetX, ref TargetY) && (Math.Abs(CurrX - TargetCret.CurrX) <= 4 && Math.Abs(CurrY - TargetCret.CurrY) == 0 || Math.Abs(CurrX - TargetCret.CurrX) == 0 && Math.Abs(CurrY - TargetCret.CurrY) <= 4 || Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 2 || Math.Abs(CurrX - TargetCret.CurrX) == 3 && Math.Abs(CurrY - TargetCret.CurrY) == 3 || Math.Abs(CurrX - TargetCret.CurrX) == 4 && Math.Abs(CurrY - TargetCret.CurrY) == 4))
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
                        if (TargetCret.Race != 101 && TargetCret.Race != 102 && TargetCret.Race != 104)// 除祖玛怪,才放火墙
                        {
                            result = 22;// 火墙
                            return result;
                        }
                    }
                    break;
                case PlayJob.Taoist:// 道士
                    if (SlaveList.Count == 0 && CheckHeroAmulet(1, 5) && HUtil32.GetTickCount() - m_SkillUseTick[17] > 3000 && (AllowUseMagic(72) || AllowUseMagic(30) || AllowUseMagic(17)) && Abil.MP > 20)
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
                        else if (AllowUseMagic(MagicConst.SKILL_SINSU))// 召唤神兽
                        {
                            result = MagicConst.SKILL_SINSU;
                        }
                        else if (AllowUseMagic(MagicConst.SKILL_SKELLETON)) // 召唤骷髅
                        {
                            result = MagicConst.SKILL_SKELLETON;
                        }
                        return result;
                    }
                    if (StatusArr[PoisonState.BUBBLEDEFENCEUP] == 0 && !AbilMagBubbleDefence)
                    {
                        if (AllowUseMagic(73)) // 道力盾
                        {
                            result = 73;
                            return result;
                        }
                    }
                    if ((TargetCret.Race == ActorRace.Play || TargetCret.Master != null) && CheckTargetXYCount3(CurrX, CurrY, 1, 0) > 0 && TargetCret.Abil.Level <= Abil.Level)
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
                        if (AllowUseMagic(48) && HUtil32.GetTickCount() - m_SkillUseTick[48] > 5000 && CheckTargetXYCount3(CurrX, CurrY, 1, 0) > 0 && TargetCret.Abil.Level <= Abil.Level)
                        {
                            m_SkillUseTick[48] = HUtil32.GetTickCount();
                            result = 48;
                            return result;
                        }
                    }
                    // 绿毒
                    if (TargetCret.StatusArr[PoisonState.DECHEALTH] == 0 && GetUserItemList(2, 1) >= 0 && (M2Share.Config.btHeroSkillMode || !M2Share.Config.btHeroSkillMode && TargetCret.WAbil.HP >= 700
                                                                                                                                                     || TargetCret.Race == ActorRace.Play) && (Math.Abs(TargetCret.CurrX - CurrX) < 7 || Math.Abs(TargetCret.CurrY - CurrY) < 7)
                        && !M2Share.RobotPlayRaceMap.Contains(TargetCret.Race))
                    {
                        // 对于血量超过800的怪用 不毒城墙
                        n_AmuletIndx = 0;
                        switch (M2Share.RandomNumber.Random(2))
                        {
                            case 0:
                                if (AllowUseMagic(38) && HUtil32.GetTickCount() - m_SkillUseTick[38] > 1000)
                                {
                                    if (Envir != null)// 判断地图是否禁用
                                    {
                                        if (Envir.AllowMagics(MagicConst.SKILL_GROUPAMYOUNSUL, 1))
                                        {
                                            m_SkillUseTick[38] = HUtil32.GetTickCount();
                                            result = MagicConst.SKILL_GROUPAMYOUNSUL;// 英雄群体施毒
                                            return result;
                                        }
                                    }
                                }
                                else if ((HUtil32.GetTickCount() - m_SkillUseTick[6]) > 1000)
                                {
                                    if (AllowUseMagic(MagicConst.SKILL_AMYOUNSUL))
                                    {
                                        if (Envir != null)
                                        {
                                            if (Envir.AllowMagics(MagicConst.SKILL_AMYOUNSUL, 1))// 判断地图是否禁用
                                            {
                                                m_SkillUseTick[6] = HUtil32.GetTickCount();
                                                result = MagicConst.SKILL_AMYOUNSUL;// 英雄施毒术
                                                return result;
                                            }
                                        }
                                    }
                                }
                                break;
                            case 1:
                                if ((HUtil32.GetTickCount() - m_SkillUseTick[6]) > 1000)
                                {
                                    if (AllowUseMagic(MagicConst.SKILL_AMYOUNSUL))
                                    {
                                        if (Envir != null)
                                        {
                                            if (Envir.AllowMagics(MagicConst.SKILL_AMYOUNSUL, 1))// 判断地图是否禁用
                                            {
                                                m_SkillUseTick[6] = HUtil32.GetTickCount();
                                                result = MagicConst.SKILL_AMYOUNSUL; // 英雄施毒术
                                                return result;
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    if (TargetCret.StatusArr[PoisonState.DAMAGEARMOR] == 0 && GetUserItemList(2, 2) >= 0 && (M2Share.Config.btHeroSkillMode || !M2Share.Config.btHeroSkillMode && TargetCret.WAbil.HP >= 700
                            || TargetCret.Race == ActorRace.Play) && (Math.Abs(TargetCret.CurrX - CurrX) < 7 || Math.Abs(TargetCret.CurrY - CurrY) < 7)
                        && !M2Share.RobotPlayRaceMap.Contains(TargetCret.Race))
                    {
                        // 对于血量超过100的怪用 不毒城墙
                        n_AmuletIndx = 0;
                        switch (M2Share.RandomNumber.Random(2))
                        {
                            case 0:
                                if (AllowUseMagic(38) && (HUtil32.GetTickCount() - m_SkillUseTick[38]) > 1000)
                                {
                                    if (Envir != null)
                                    {
                                        // 判断地图是否禁用
                                        if (Envir.AllowMagics(MagicConst.SKILL_GROUPAMYOUNSUL, 1))
                                        {
                                            m_SkillUseTick[38] = HUtil32.GetTickCount();
                                            result = MagicConst.SKILL_GROUPAMYOUNSUL; // 英雄群体施毒
                                            return result;
                                        }
                                    }
                                }
                                else if ((HUtil32.GetTickCount() - m_SkillUseTick[6]) > 1000)
                                {
                                    if (AllowUseMagic(MagicConst.SKILL_AMYOUNSUL))
                                    {
                                        if (Envir != null)
                                        {
                                            // 判断地图是否禁用
                                            if (Envir.AllowMagics(MagicConst.SKILL_AMYOUNSUL, 1))
                                            {
                                                m_SkillUseTick[6] = HUtil32.GetTickCount();
                                                result = MagicConst.SKILL_AMYOUNSUL; // 英雄施毒术
                                                return result;
                                            }
                                        }
                                    }
                                }
                                break;
                            case 1:
                                if ((HUtil32.GetTickCount() - m_SkillUseTick[6]) > 1000)
                                {
                                    if (AllowUseMagic(MagicConst.SKILL_AMYOUNSUL))
                                    {
                                        if (Envir != null)
                                        {
                                            // 判断地图是否禁用
                                            if (Envir.AllowMagics(MagicConst.SKILL_AMYOUNSUL, 1))
                                            {
                                                m_SkillUseTick[6] = HUtil32.GetTickCount();
                                                result = MagicConst.SKILL_AMYOUNSUL; // 英雄施毒术
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
                                if (AllowUseMagic(52)) // 诅咒术
                                {
                                    if (TargetCret.Race == ActorRace.Play && (TargetCret as PlayObject).ExtraAbil[(byte)(TargetCret as PlayObject).Job + 6] == 0)
                                    {
                                        result = 52;// 英雄诅咒术
                                        return result;
                                    }
                                }
                                break;
                            case 1:
                                if (AllowUseMagic(52)) // 诅咒术
                                {
                                    if (TargetCret.Race == ActorRace.Play && (TargetCret as PlayObject).ExtraAbil[(byte)(TargetCret as PlayObject).Job + 6] == 0)
                                    {
                                        result = 52;// 英雄诅咒术
                                        return result;
                                    }
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
                                if (AllowUseMagic(52))// 诅咒术
                                {
                                    if (TargetCret.Race == ActorRace.Play && (TargetCret as PlayObject).ExtraAbil[(byte)(TargetCret as PlayObject).Job + 6] == 0)
                                    {
                                        result = 52;
                                        return result;
                                    }
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
                        if (AllowUseMagic(52))// 诅咒术
                        {
                            if (TargetCret.Race == ActorRace.Play && (TargetCret as PlayObject).ExtraAbil[(byte)(TargetCret as PlayObject).Job + 6] == 0)
                            {
                                result = 52;
                                return result;
                            }
                        }
                    }
                    break;
            }
            return result;
        }

        // 战士判断使用
        private int CheckTargetXYCount1(int nX, int nY, int nRange)
        {
            BaseObject BaseObject;
            int result = 0;
            int n10 = nRange;
            if (VisibleActors.Count > 0)
            {
                for (var i = 0; i < VisibleActors.Count; i++)
                {
                    BaseObject = VisibleActors[i].BaseObject;
                    if (BaseObject != null)
                    {
                        if (!BaseObject.Death)
                        {
                            if (IsProperTarget(BaseObject) && (!BaseObject.HideMode || CoolEye))
                            {
                                if (Math.Abs(nX - BaseObject.CurrX) <= n10 && Math.Abs(nY - BaseObject.CurrY) <= n10)
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
            BaseObject BaseObject;
            if (VisibleActors.Count > 0)
            {
                for (var i = 0; i < VisibleActors.Count; i++)
                {
                    switch (nMode)
                    {
                        case MagicConst.SKILL_BANWOL:
                            n10 = (Direction + M2Share.Config.WideAttack[nC]) % 8;
                            break;
                    }
                    if (Envir.GetNextPosition(CurrX, CurrY, n10, 1, ref nX, ref nY))
                    {
                        BaseObject = (BaseObject)Envir.GetMovingObject(nX, nY, true);
                        if (BaseObject != null)
                        {
                            if (!BaseObject.Death)
                            {
                                if (IsProperTarget(BaseObject) && (!BaseObject.HideMode || CoolEye))
                                {
                                    result++;
                                }
                            }
                        }
                    }
                    nC++;
                    switch (nMode)
                    {
                        case MagicConst.SKILL_BANWOL:
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
            BaseObject BaseObject;
            int result = 0;
            int n10 = nRange;
            if (VisibleActors.Count > 0)
            {
                for (var i = 0; i < VisibleActors.Count; i++)
                {
                    BaseObject = VisibleActors[i].BaseObject;
                    if (BaseObject != null)
                    {
                        if (!BaseObject.Death)
                        {
                            if (IsProperTarget(BaseObject) && (!BaseObject.HideMode || CoolEye))
                            {
                                if (Math.Abs(nX - BaseObject.CurrX) <= n10 && Math.Abs(nY - BaseObject.CurrY) <= n10)
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
            UserItem UserItem;
            StdItem AmuletStdItem;
            try
            {
                result = false;
                if (UseItems[Grobal2.U_ARMRINGL].Index > 0)
                {
                    AmuletStdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_ARMRINGL].Index);
                    if (AmuletStdItem != null)
                    {
                        if (AmuletStdItem.StdMode == 25)
                        {
                            switch (nType)
                            {
                                case 1:
                                    if (AmuletStdItem.Shape == 5 && Math.Round(Convert.ToDouble(UseItems[Grobal2.U_ARMRINGL].Dura / 100)) >= nCount)
                                    {
                                        result = true;
                                        return result;
                                    }
                                    break;
                                case 2:
                                    if (AmuletStdItem.Shape <= 2 && Math.Round(Convert.ToDouble(UseItems[Grobal2.U_ARMRINGL].Dura / 100)) >= nCount)
                                    {
                                        result = true;
                                        return result;
                                    }
                                    break;
                            }
                        }
                    }
                }
                if (UseItems[Grobal2.U_BUJUK] != null && UseItems[Grobal2.U_BUJUK].Index > 0)
                {
                    AmuletStdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_BUJUK].Index);
                    if (AmuletStdItem != null)
                    {
                        if (AmuletStdItem.StdMode == 25)
                        {
                            switch (nType)
                            {
                                case 1: // 符
                                    if (AmuletStdItem.Shape == 5 && Math.Round(Convert.ToDouble(UseItems[Grobal2.U_BUJUK].Dura / 100)) >= nCount)
                                    {
                                        result = true;
                                        return result;
                                    }
                                    break;
                                case 2: // 毒
                                    if (AmuletStdItem.Shape <= 2 && Math.Round(Convert.ToDouble(UseItems[Grobal2.U_BUJUK].Dura / 100)) >= nCount)
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
                if (ItemList.Count > 0)
                {
                    for (var i = 0; i < ItemList.Count; i++)
                    {
                        // 人物包裹不为空
                        UserItem = ItemList[i];
                        if (UserItem != null)
                        {
                            AmuletStdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
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
                M2Share.Log.Error("RobotPlayObject.CheckHeroAmulet");
            }
            return result;
        }

        private int GetDirBaseObjectsCount(int m_btDirection, int rang)
        {
            return 0;
        }
    }
}