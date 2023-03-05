using GameSrv.Actor;
using GameSrv.Conf;
using GameSrv.Items;
using GameSrv.Magic;
using GameSrv.Maps;
using GameSrv.Monster.Monsters;
using GameSrv.Player;
using System.Collections;
using SystemModule.Common;
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.RobotPlay
{
    /// <summary>
    /// 假人
    /// </summary>
    public partial class RobotPlayer : PlayObject
    {
        public long DwTick3F4 = 0;
        public long MDwSearchTargetTick = 0;
        /// <summary>
        /// 假人启动
        /// </summary>
        public bool MBoAiStart;
        /// <summary>
        /// 挂机地图
        /// </summary>
        public Envirnoment MManagedEnvir;
        public PointManager MPointManager;
        public PointInfo[] MPath;
        public int MNPostion;
        public int MoveFailCount;
        public string ConfigListFileName = string.Empty;
        public string FilePath = string.Empty;
        public string ConfigFileName = string.Empty;
        public string MSHeroConfigListFileName = string.Empty;
        public string MSHeroConfigFileName = string.Empty;
        public IList<string> BagItemNames;
        public string[] UseItemNames;
        public TRunPos MRunPos;
        /// <summary>
        /// 魔法使用间隔
        /// </summary>
        public long[] MSkillUseTick;
        public int MNSelItemType;
        public int MNIncSelfHealthCount;
        public int MNIncMasterHealthCount;
        /// <summary>
        /// 攻击方式
        /// </summary>
        public short HitMode;
        public bool BoSelSelf;
        public byte MBtTaoistUseItemType;
        public long MDwAutoRepairItemTick;
        public long MDwAutoAddHealthTick;
        public long MDwThinkTick;
        public bool MBoDupMode;
        public long MDwSearchMagic = 0;
        /// <summary>
        /// 低血回城间隔
        /// </summary>
        public long MDwHpToMapHomeTick = 0;
        /// <summary>
        /// 守护模式
        /// </summary>
        public bool ProtectStatus;
        public short ProtectTargetX;
        public short ProtectTargetY;
        /// <summary>
        /// 到达守护坐标
        /// </summary>
        public bool ProtectDest;
        /// <summary>
        /// 是向守护坐标的累计数
        /// </summary>
        public int MNGotoProtectXyCount;
        public long MDwPickUpItemTick;
        public MapItem MSelMapItem;
        /// <summary>
        /// 跑步计时
        /// </summary>
        public long DwTick5F4;
        /// <summary>
        /// 受攻击说话列表
        /// </summary>
        public ArrayList AiSayMsgList;
        /// <summary>
        /// 绿红毒标识
        /// </summary>
        public byte NAmuletIndx;
        /// <summary>
        /// 正在拾取物品
        /// </summary>
        public bool CanPickIng;
        /// <summary>
        /// 查询魔法
        /// </summary>
        public short AutoMagicId;
        /// <summary>
        /// 是否可以使用的魔法(True才可能躲避)
        /// </summary>
        public bool AutoUseMagic;
        /// <summary>
        /// 是否可以使用的攻击魔法
        /// </summary>
        public bool MBoIsUseAttackMagic;
        /// <summary>
        /// 最后的方向
        /// </summary>
        public byte MBtLastDirection;
        /// <summary>
        /// 自动躲避间隔
        /// </summary>
        public long AutoAvoidTick;
        public bool IsNeedAvoid;
        /// <summary>
        /// 假人掉装备机率
        /// </summary>
        public int DropUseItemRate;

        public RobotPlayer() : base()
        {
            SoftVersionDate = M2Share.Config.SoftVersionDate;
            SoftVersionDateEx = Grobal2.CLIENT_VERSION_NUMBER;
            AbilCopyToWAbil();
            IsRobot = true;
            LoginNoticeOk = true;
            MBoAiStart = false; // 开始挂机
            MManagedEnvir = null; // 挂机地图
            MPath = null;
            MNPostion = -1;
            UseItemNames = new string[13];
            BagItemNames = new List<string>();
            MPointManager = new PointManager(this);
            MSkillUseTick = new long[60];// 魔法使用间隔
            MNSelItemType = 1;
            MNIncSelfHealthCount = 0;
            MNIncMasterHealthCount = 0;
            BoSelSelf = false;
            MBtTaoistUseItemType = 0;
            MDwAutoAddHealthTick = HUtil32.GetTickCount();
            MDwAutoRepairItemTick = HUtil32.GetTickCount();
            MDwThinkTick = HUtil32.GetTickCount();
            MBoDupMode = false;
            ProtectStatus = false;// 守护模式
            ProtectDest = true;// 到达守护坐标
            MNGotoProtectXyCount = 0;// 是向守护坐标的累计数
            MSelMapItem = null;
            MDwPickUpItemTick = HUtil32.GetTickCount();
            AiSayMsgList = new ArrayList();// 受攻击说话列表
            NAmuletIndx = 0;
            CanPickIng = false;
            AutoMagicId = 0;
            AutoUseMagic = false;// 是否能躲避
            MBoIsUseAttackMagic = false;
            MBtLastDirection = Dir;
            AutoAvoidTick = HUtil32.GetTickCount();// 自动躲避间隔
            IsNeedAvoid = false;// 是否需要躲避
            WalkTick = HUtil32.GetTickCount();
            WalkSpeed = 300;
            MRunPos = new TRunPos();
            MPath = new PointInfo[0];
            LoadConfig();
        }

        public void Start(FindPathType pathType)
        {
            if (!Ghost && !Death && !MBoAiStart)
            {
                MManagedEnvir = Envir;
                ProtectDest = false;
                ProtectTargetX = CurrX;// 守护坐标
                ProtectTargetY = CurrY;// 守护坐标
                MNGotoProtectXyCount = 0;// 是向守护坐标的累计数
                MPointManager.PathType = pathType;
                MPointManager.Initialize(Envir);
                MBoAiStart = true;
                MoveFailCount = 0;
                if (M2Share.FunctionNPC != null)
                {
                    ScriptGotoCount = 0;
                    M2Share.FunctionNPC.GotoLable(this, "@AIStart", false);
                }
            }
        }

        public void Stop()
        {
            if (MBoAiStart)
            {
                MBoAiStart = false;
                MoveFailCount = 0;
                MPath = null;
                MNPostion = -1;
                if (M2Share.FunctionNPC != null)
                {
                    ScriptGotoCount = 0;
                    M2Share.FunctionNPC.GotoLable(this, "@AIStop", false);
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
                    dwExp = HUtil32.Round(Envir.Flag.ExpRate / 100 * dwExp); // 地图上指定杀怪经验倍数
                }
                GetExp(dwExp);
            }
        }

        private void GetExp(int dwExp)
        {
            Abil.Exp += dwExp;
            AddBodyLuck(dwExp * 0.002);
            SendMsg(this, Messages.RM_WINEXP, 0, dwExp, 0, 0, "");
            if (Abil.Exp >= Abil.MaxExp)
            {
                Abil.Exp -= Abil.MaxExp;
                if (Abil.Level < Settings.MAXUPLEVEL)
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
            if (MBoAiStart)
            {
                MBoAiStart = false;
            }
            base.MakeGhost();
        }

        protected override void Whisper(string whostr, string saystr)
        {
            PlayObject playObject = M2Share.WorldEngine.GetPlayObject(whostr);
            if (playObject != null)
            {
                if (!playObject.BoReadyRun)
                {
                    return;
                }
                if (!playObject.HearWhisper || playObject.IsBlockWhisper(ChrName))
                {
                    return;
                }
                if (Permission > 0)
                {
                    playObject.SendMsg(playObject, Messages.RM_WHISPER, 0, M2Share.Config.btGMWhisperMsgFColor, M2Share.Config.btGMWhisperMsgBColor, 0, Format("{0}[{1}级]=> {2}", new object[] { ChrName, Abil.Level, saystr }));
                    // 取得私聊信息
                    // m_GetWhisperHuman 侦听私聊对象
                    if (WhisperHuman != null && !WhisperHuman.Ghost)
                    {
                        WhisperHuman.SendMsg(WhisperHuman, Messages.RM_WHISPER, 0, M2Share.Config.btGMWhisperMsgFColor, M2Share.Config.btGMWhisperMsgBColor, 0, Format("{0}[{1}级]=> {2} {3}", new object[] { ChrName, Abil.Level, playObject.ChrName, saystr }));
                    }
                    if (playObject.WhisperHuman != null && !playObject.WhisperHuman.Ghost)
                    {
                        playObject.WhisperHuman.SendMsg(playObject.WhisperHuman, Messages.RM_WHISPER, 0, M2Share.Config.btGMWhisperMsgFColor, M2Share.Config.btGMWhisperMsgBColor, 0, Format("{0}[{1}级]=> {2} {3}", new object[] { ChrName, Abil.Level, playObject.ChrName, saystr }));
                    }
                }
                else
                {
                    playObject.SendMsg(playObject, Messages.RM_WHISPER, 0, M2Share.Config.btGMWhisperMsgFColor, M2Share.Config.btWhisperMsgBColor, 0, Format("{0}[{1}级]=> {2}", new object[] { ChrName, Abil.Level, saystr }));
                    if (WhisperHuman != null && !WhisperHuman.Ghost)
                    {
                        WhisperHuman.SendMsg(WhisperHuman, Messages.RM_WHISPER, 0, M2Share.Config.btGMWhisperMsgFColor, M2Share.Config.btWhisperMsgBColor, 0, Format("{0}[{1}级]=> {2} {3}", new object[] { ChrName, Abil.Level, playObject.ChrName, saystr }));
                    }
                    if (playObject.WhisperHuman != null && !playObject.WhisperHuman.Ghost)
                    {
                        playObject.WhisperHuman.SendMsg(playObject.WhisperHuman, Messages.RM_WHISPER, 0, M2Share.Config.btGMWhisperMsgFColor, M2Share.Config.btWhisperMsgBColor, 0, Format("{0}[{1}级]=> {2} {3}", new object[] { ChrName, Abil.Level, playObject.ChrName, saystr }));
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
                string sParam1 = string.Empty;
                if (sData.Length > M2Share.Config.SayMsgMaxLen)
                {
                    sData = sData[..M2Share.Config.SayMsgMaxLen];
                }
                if (HUtil32.GetTickCount() >= DisableSayMsgTick)
                {
                    DisableSayMsg = false;
                }
                bool boDisableSayMsg = DisableSayMsg;
                //g_DenySayMsgList.Lock;
                //if (g_DenySayMsgList.GetIndex(m_sChrName) >= 0)
                //{
                //    boDisableSayMsg = true;
                //}
                // g_DenySayMsgList.UnLock;
                if (!boDisableSayMsg)
                {
                    string sc;
                    if (sData[0] == '/')
                    {
                        sc = sData[1..];
                        sc = HUtil32.GetValidStr3(sc, ref sParam1, ' ');
                        if (!FilterSendMsg)
                        {
                            Whisper(sParam1, sc);
                        }
                        return;
                    }
                    if (sData[0] == '!')
                    {
                        if (sData.Length >= 2)
                        {
                            if (sData[1] == '!')//发送组队消息
                            {
                                sc = sData[2..];
                                SendGroupText(ChrName + ": " + sc);
                                return;
                            }
                            if (sData[1] == '~') //发送行会消息
                            {
                                if (MyGuild != null)
                                {
                                    sc = sData[2..];
                                    MyGuild.SendGuildMsg(ChrName + ": " + sc);
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
                                    SysMsg(Format(Settings.YouNeedLevelMsg, M2Share.Config.CanShoutMsgLevel + 1), MsgColor.Red, MsgType.Hint);
                                    return;
                                }
                                ShoutMsgTick = HUtil32.GetTickCount();
                                sc = sData[1..];
                                string sCryCryMsg = "(!)" + ChrName + ": " + sc;
                                if (FilterSendMsg)
                                {
                                    SendMsg(null, Messages.RM_CRY, 0, 0, 0xFFFF, 0, sCryCryMsg);
                                }
                                else
                                {
                                    M2Share.WorldEngine.CryCry(Messages.RM_CRY, Envir, CurrX, CurrY, 50, M2Share.Config.CryMsgFColor, M2Share.Config.CryMsgBColor, sCryCryMsg);
                                }
                                return;
                            }
                            SysMsg(Format(Settings.YouCanSendCyCyLaterMsg, new object[] { 10 - (HUtil32.GetTickCount() - ShoutMsgTick) / 1000 }), MsgColor.Red, MsgType.Hint);
                            return;
                        }
                        SysMsg(Settings.ThisMapDisableSendCyCyMsg, MsgColor.Red, MsgType.Hint);
                        return;
                    }
                    if (!FilterSendMsg)
                    {
                        SendRefMsg(Messages.RM_HEAR, 0, M2Share.Config.btHearMsgFColor, M2Share.Config.btHearMsgBColor, 0, ChrName + ':' + sData);
                    }
                }
            }
            catch (Exception)
            {
                M2Share.Logger.Error(Format(sExceptionMsg, new object[] { sData }));
            }
        }

        public UserMagic FindMagic(short wMagIdx)
        {
            UserMagic result = null;
            for (int i = 0; i < MagicList.Count; i++)
            {
                var userMagic = MagicList[i];
                if (userMagic.Magic.MagicId == wMagIdx)
                {
                    result = userMagic;
                    break;
                }
            }
            return result;
        }

        public UserMagic FindMagic(string sMagicName)
        {
            UserMagic result = null;
            for (int i = 0; i < MagicList.Count; i++)
            {
                UserMagic userMagic = MagicList[i];
                if (string.Compare(userMagic.Magic.MagicName, sMagicName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = userMagic;
                    break;
                }
            }
            return result;
        }

        private bool RunToNext(short nX, short nY)
        {
            bool result = false;
            if ((HUtil32.GetTickCount() - DwTick5F4) > M2Share.Config.nAIRunIntervalTime)
            {
                result = RobotRunTo(M2Share.GetNextDirection(CurrX, CurrY, nX, nY), false, nX, nY);
                DwTick5F4 = HUtil32.GetTickCount();
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
            MRunPos.AttackCount = 0;
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
                case Messages.RM_HEAR:
                    break;
                case Messages.RM_WHISPER:
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
                        if (nPos > 0 && AiSayMsgList.Count > 0)
                        {
                            sChrName = sMsg[..(nPos - 1)];
                            sSendMsg = sMsg.Substring(nPos + 3 - 1, sMsg.Length - nPos - 2);
                            Whisper(sChrName, "你猜我是谁.");
                            //Whisper(sChrName, m_AISayMsgList[(M2Share.RandomNumber.Random(m_AISayMsgList.Count)).Next()]);
                            M2Share.Logger.Error("TODO Hear...");
                        }
                    }
                    break;
                case Messages.RM_CRY:
                    break;
                case Messages.RM_SYSMESSAGE:
                    break;
                case Messages.RM_GROUPMESSAGE:
                    break;
                case Messages.RM_GUILDMESSAGE:
                    break;
                case Messages.RM_MERCHANTSAY:
                    break;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private void SearchPickUpItem_SetHideItem(MapItem mapItem)
        {
            VisibleMapItem visibleMapItem;
            for (int i = 0; i < VisibleItems.Count; i++)
            {
                visibleMapItem = VisibleItems[i];
                if (visibleMapItem != null && visibleMapItem.VisibleFlag > 0)
                {
                    if (visibleMapItem.MapItem == mapItem)
                    {
                        visibleMapItem.VisibleFlag = 0;
                        break;
                    }
                }
            }
        }

        private bool SearchPickUpItem_PickUpItem(int nX, int nY)
        {
            bool result = false;
            UserItem userItem = null;
            MapItem mapItem = Envir.GetItem(nX, nY);
            if (mapItem == null)
            {
                return result;
            }
            if (string.Compare(mapItem.Name, Grobal2.StringGoldName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (Envir.DeleteFromMap(nX, nY, CellType.Item, mapItem.ItemId, null) == 1)
                {
                    if (this.IncGold(mapItem.Count))
                    {
                        SendRefMsg(Messages.RM_ITEMHIDE, 0, mapItem.ItemId, nX, nY, "");
                        result = true;
                        GoldChanged();
                        SearchPickUpItem_SetHideItem(mapItem);
                        Dispose(mapItem);
                    }
                    else
                    {
                        Envir.AddToMap(nX, nY, CellType.Item, mapItem.ItemId, mapItem);
                    }
                }
                else
                {
                    Envir.AddToMap(nX, nY, CellType.Item, mapItem.ItemId, mapItem);
                }
            }
            else
            {
                // 捡物品
                StdItem stdItem = M2Share.WorldEngine.GetStdItem(mapItem.UserItem.Index);
                if (stdItem != null)
                {
                    if (Envir.DeleteFromMap(nX, nY, CellType.Item, mapItem.ItemId, null) == 1)
                    {
                        userItem = new UserItem();
                        userItem = mapItem.UserItem;
                        stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                        if (stdItem != null && IsAddWeightAvailable(M2Share.WorldEngine.GetStdItemWeight(userItem.Index)))
                        {
                            //if (GetCheckItemList(18, StdItem.Name))
                            //{
                            //    // 判断是否为绑定48时物品
                            //    UserItem.AddValue[0] = 2;
                            //    UserItem.MaxDate = HUtil32.IncDayHour(DateTime.Now, 48); // 解绑时间
                            //}
                            if (AddItemToBag(userItem))
                            {
                                SendRefMsg(Messages.RM_ITEMHIDE, 0, mapItem.ItemId, nX, nY, "");
                                this.SendAddItem(userItem);
                                Abil.Weight = RecalcBagWeight();
                                result = true;
                                SearchPickUpItem_SetHideItem(mapItem);
                                Dispose(mapItem);
                            }
                            else
                            {
                                Dispose(userItem);
                                Envir.AddToMap(nX, nY, CellType.Item, mapItem.ItemId, mapItem);
                            }
                        }
                        else
                        {
                            Dispose(userItem);
                            Envir.AddToMap(nX, nY, CellType.Item, mapItem.ItemId, mapItem);
                        }
                    }
                    else
                    {
                        Dispose(userItem);
                        Envir.AddToMap(nX, nY, CellType.Item, mapItem.ItemId, mapItem);
                    }
                }
            }
            return result;
        }

        private bool SearchPickUpItem(int nPickUpTime)
        {
            bool result = false;
            VisibleMapItem visibleMapItem = null;
            bool boFound;
            try
            {
                if ((HUtil32.GetTickCount() - MDwPickUpItemTick) < nPickUpTime)
                {
                    return result;
                }
                MDwPickUpItemTick = HUtil32.GetTickCount();
                if (this.IsEnoughBag() && TargetCret == null)
                {
                    boFound = false;
                    if (MSelMapItem != null)
                    {
                        CanPickIng = true;
                        for (int i = 0; i < VisibleItems.Count; i++)
                        {
                            visibleMapItem = VisibleItems[i];
                            if (visibleMapItem != null && visibleMapItem.VisibleFlag > 0)
                            {
                                if (visibleMapItem.MapItem == MSelMapItem)
                                {
                                    boFound = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!boFound)
                    {
                        MSelMapItem = null;
                    }
                    if (MSelMapItem != null)
                    {
                        if (SearchPickUpItem_PickUpItem(CurrX, CurrY))
                        {
                            CanPickIng = false;
                            result = true;
                            return result;
                        }
                    }
                    int n01 = 999;
                    VisibleMapItem selVisibleMapItem = null;
                    boFound = false;
                    if (MSelMapItem != null)
                    {
                        for (int i = 0; i < VisibleItems.Count; i++)
                        {
                            visibleMapItem = VisibleItems[i];
                            if (visibleMapItem != null && visibleMapItem.VisibleFlag > 0)
                            {
                                if (visibleMapItem.MapItem == MSelMapItem)
                                {
                                    selVisibleMapItem = visibleMapItem;
                                    boFound = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!boFound)
                    {
                        for (int i = 0; i < VisibleItems.Count; i++)
                        {
                            visibleMapItem = VisibleItems[i];
                            if (visibleMapItem != null)
                            {
                                if (visibleMapItem.VisibleFlag > 0)
                                {
                                    MapItem mapItem = visibleMapItem.MapItem;
                                    if (mapItem != null)
                                    {
                                        if (IsAllowAiPickUpItem(visibleMapItem.sName) && IsAddWeightAvailable(M2Share.WorldEngine.GetStdItemWeight(mapItem.UserItem.Index)))
                                        {
                                            if (mapItem.OfBaseObject == 0 || mapItem.OfBaseObject == this.ActorId || (M2Share.ActorMgr.Get(mapItem.OfBaseObject).Master == this))
                                            {
                                                if (Math.Abs(visibleMapItem.nX - CurrX) <= 5 && Math.Abs(visibleMapItem.nY - CurrY) <= 5)
                                                {
                                                    int n02 = Math.Abs(visibleMapItem.nX - CurrX) + Math.Abs(visibleMapItem.nY - CurrY);
                                                    if (n02 < n01)
                                                    {
                                                        n01 = n02;
                                                        selVisibleMapItem = visibleMapItem;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (selVisibleMapItem != null)
                    {
                        MSelMapItem = selVisibleMapItem.MapItem;
                        if (MSelMapItem != null)
                        {
                            CanPickIng = true;
                        }
                        else
                        {
                            CanPickIng = false;
                        }
                        if (CurrX != selVisibleMapItem.nX || CurrY != selVisibleMapItem.nY)
                        {
                            WalkToTargetXy2(selVisibleMapItem.nX, visibleMapItem.nY);
                            result = true;
                        }
                    }
                    else
                    {
                        CanPickIng = false;
                    }
                }
                else
                {
                    MSelMapItem = null;
                    CanPickIng = false;
                }
            }
            catch
            {
                M2Share.Logger.Error("RobotPlayObject.SearchPickUpItem");
            }
            return result;
        }

        private static bool IsAllowAiPickUpItem(string sName)
        {
            return true;
        }

        private bool WalkToTargetXy2(int nTargetX, int nTargetY)
        {
            int n10;
            int n14;
            int n20;
            int nOldX;
            int nOldY;
            bool result = false;
            if (Transparent && HideMode)
            {
                StatusTimeArr[PoisonState.STATETRANSPARENT] = 1;// 隐身,一动就显身
            }
            if (StatusTimeArr[PoisonState.STONE] != 0 && StatusTimeArr[PoisonState.DONTMOVE] != 0 || StatusTimeArr[PoisonState.LOCKSPELL] != 0)
            {
                return result;// 麻痹不能跑动 
            }
            if (nTargetX != CurrX || nTargetY != CurrY)
            {
                if ((HUtil32.GetTickCount() - DwTick3F4) > TurnIntervalTime)// 转向间隔
                {
                    n10 = nTargetX;
                    n14 = nTargetY;
                    var nDir = Direction.Down;
                    if (n10 > CurrX)
                    {
                        nDir = Direction.Right;
                        if (n14 > CurrY)
                        {
                            nDir = Direction.DownRight;
                        }
                        if (n14 < CurrY)
                        {
                            nDir = Direction.UpRight;
                        }
                    }
                    else
                    {
                        if (n10 < CurrX)
                        {
                            nDir = Direction.Left;
                            if (n14 > CurrY)
                            {
                                nDir = Direction.DownLeft;
                            }
                            if (n14 < CurrY)
                            {
                                nDir = Direction.UpLeft;
                            }
                        }
                        else
                        {
                            if (n14 > CurrY)
                            {
                                nDir = Direction.Down;
                            }
                            else if (n14 < CurrY)
                            {
                                nDir = Direction.Up;
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
                        for (byte i = Direction.Up; i <= Direction.UpLeft; i++)
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
                                    nDir = Direction.UpLeft;
                                }
                                if (nDir > Direction.UpLeft)
                                {
                                    nDir = Direction.Up;
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
            if (CurrX != ProtectTargetX || CurrY != ProtectTargetY)
            {
                n10 = ProtectTargetX;
                n14 = ProtectTargetY;
                DwTick3F4 = HUtil32.GetTickCount();
                nDir = Direction.Down;
                if (n10 > CurrX)
                {
                    nDir = Direction.Right;
                    if (n14 > CurrY)
                    {
                        nDir = Direction.DownRight;
                    }
                    if (n14 < CurrY)
                    {
                        nDir = Direction.UpRight;
                    }
                }
                else
                {
                    if (n10 < CurrX)
                    {
                        nDir = Direction.Left;
                        if (n14 > CurrY)
                        {
                            nDir = Direction.DownLeft;
                        }
                        if (n14 < CurrY)
                        {
                            nDir = Direction.UpLeft;
                        }
                    }
                    else
                    {
                        if (n14 > CurrY)
                        {
                            nDir = Direction.Down;
                        }
                        else if (n14 < CurrY)
                        {
                            nDir = Direction.Up;
                        }
                    }
                }
                nOldX = CurrX;
                nOldY = CurrY;
                if (Math.Abs(CurrX - ProtectTargetX) >= 3 || Math.Abs(CurrY - ProtectTargetY) >= 3)
                {
                    //m_dwStationTick = HUtil32.GetTickCount();// 增加检测人物站立时间
                    if (!RobotRunTo(nDir, false, ProtectTargetX, ProtectTargetY))
                    {
                        WalkTo(nDir, false);
                        n20 = M2Share.RandomNumber.Random(3);
                        for (byte i = Direction.Up; i <= Direction.UpLeft; i++)
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
                                    nDir = Direction.UpLeft;
                                }
                                if (nDir > Direction.UpLeft)
                                {
                                    nDir = Direction.Up;
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
                    for (byte i = Direction.Up; i <= Direction.UpLeft; i++)
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
                                nDir = Direction.UpLeft;
                            }
                            if (nDir > Direction.UpLeft)
                            {
                                nDir = Direction.Up;
                            }
                            WalkTo(nDir, false);
                        }
                    }
                }
            }
        }

        protected override void Wondering()
        {
            if (MBoAiStart && TargetCret == null && !CanPickIng && !Ghost && !Death && !FixedHideMode && !StoneMode && StatusTimeArr[PoisonState.STONE] == 0)
            {
                short nX = CurrX;
                short nY = CurrY;
                if (MPath != null && MPath.Length > 0 && MNPostion < MPath.Length)
                {
                    if (!GotoNextOne(MPath[MNPostion].nX, MPath[MNPostion].nY, true))
                    {
                        MPath = null;
                        MNPostion = -1;
                        MoveFailCount++;
                        MNPostion++;
                    }
                    else
                    {
                        MoveFailCount = 0;
                        return;
                    }
                }
                else
                {
                    MPath = null;
                    MNPostion = -1;
                }
                if (MPointManager.GetPoint(ref nX, ref nY))
                {
                    if (Math.Abs(nX - CurrX) > 2 || Math.Abs(nY - CurrY) > 2)
                    {
                        MPath = M2Share.FindPath.Find(Envir, CurrX, CurrY, nX, nY, true);
                        MNPostion = 0;
                        if (MPath.Length > 0 && MNPostion < MPath.Length)
                        {
                            if (!GotoNextOne(MPath[MNPostion].nX, MPath[MNPostion].nY, true))
                            {
                                MPath = null;
                                MNPostion = -1;
                                MoveFailCount++;
                            }
                            else
                            {
                                MoveFailCount = 0;
                                MNPostion++;

                                return;
                            }
                        }
                        else
                        {
                            MPath = null;
                            MNPostion = -1;
                            MoveFailCount++;
                        }
                    }
                    else
                    {
                        if (GotoNextOne(nX, nY, true))
                        {
                            MoveFailCount = 0;
                        }
                        else
                        {
                            MoveFailCount++;
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
                        WalkTo(Dir, false);
                    }
                    MPath = null;
                    MNPostion = -1;
                    MoveFailCount++;
                }
            }
            if (MoveFailCount >= 3)
            {
                if (M2Share.RandomNumber.Random(2) == 1)
                {
                    TurnTo(M2Share.RandomNumber.RandomByte(8));
                }
                else
                {
                    WalkTo(Dir, false);
                }
                MPath = null;
                MNPostion = -1;
                MoveFailCount = 0;
            }
        }

        private BaseObject Struck_MINXY(BaseObject aObject, BaseObject bObject)
        {
            BaseObject result;
            int nA = Math.Abs(CurrX - aObject.CurrX) + Math.Abs(CurrY - aObject.CurrY);
            int nB = Math.Abs(CurrX - bObject.CurrX) + Math.Abs(CurrY - bObject.CurrY);
            if (nA > nB)
            {
                result = bObject;
            }
            else
            {
                result = aObject;
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

        private bool IsGotoXy(short x1, short y1, short x2, short y2)
        {
            bool result = false;
            int nStep = 0;
            //0代替-1
            if (!CanWalk(x1, y1, x2, y2, 0, ref nStep, Race != 108))
            {
                PointInfo[] path = M2Share.FindPath.Find(Envir, x1, y1, x2, y2, false);
                if (path.Length <= 0)
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
                PointInfo[] path = M2Share.FindPath.Find(Envir, CurrX, CurrY, nX, nY, boRun);
                if (path.Length > 0)
                {
                    for (int i = 0; i < path.Length; i++)
                    {
                        if (path[i].nX != CurrX || path[i].nY != CurrY)
                        {
                            if (Math.Abs(path[i].nX - CurrX) >= 2 || Math.Abs(path[i].nY - CurrY) >= 2)
                            {
                                result = RunToNext(path[i].nX, path[i].nY);
                            }
                            else
                            {
                                result = WalkToNext(path[i].nX, path[i].nY);
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
            MRunPos.AttackCount = 0;
            return result;
        }

        protected override bool Operate(ProcessMessage processMsg)
        {
            bool result = false;
            try
            {
                if (processMsg.wIdent == Messages.RM_STRUCK)
                {
                    if (processMsg.ActorId == this.ActorId)
                    {
                        BaseObject attackBaseObject = M2Share.ActorMgr.Get(processMsg.nParam3);
                        if (attackBaseObject != null)
                        {
                            if (attackBaseObject.Race == ActorRace.Play)
                            {
                                SetPkFlag(attackBaseObject);
                            }
                            SetLastHiter(attackBaseObject);
                            Struck(attackBaseObject);
                            BreakHolySeizeMode();
                        }
                        if (M2Share.CastleMgr.IsCastleMember(this) != null && attackBaseObject != null)
                        {
                            if (attackBaseObject.Race == ActorRace.Guard)
                            {
                                ((GuardUnit)attackBaseObject).BoCrimeforCastle = true;
                                ((GuardUnit)attackBaseObject).CrimeforCastleTime = HUtil32.GetTickCount();
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
                    result = base.Operate(processMsg);
                }
            }
            catch (Exception ex)
            {
                M2Share.Logger.Error(ex.Message);
            }
            return result;
        }

        private int GetRangeTargetCountByDir(byte nDir, short nX, short nY, int nRange)
        {
            int result = 0;
            short nCurrX = nX;
            short nCurrY = nY;
            for (int i = 0; i < nRange; i++)
            {
                if (Envir.GetNextPosition(nCurrX, nCurrY, nDir, 1, ref nCurrX, ref nCurrY))
                {
                    BaseObject baseObject = Envir.GetMovingObject(nCurrX, nCurrY, true);
                    if (baseObject != null && !baseObject.Death && !baseObject.Ghost && (!baseObject.HideMode || CoolEye) && IsProperTarget(baseObject))
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
            BaseObject baseObject;
            for (int n10 = 0; n10 < 7; n10++)
            {
                if (Envir.GetNextPosition(CurrX, CurrY, (byte)n10, 1, ref nX, ref nY))
                {
                    baseObject = Envir.GetMovingObject(nX, nY, true);
                    if (baseObject != null && !baseObject.Death && !baseObject.Ghost && IsProperTarget(baseObject))
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
            BaseObject baseObject = Envir.GetMovingObject(nCurrX, nCurrY, true);
            if (baseObject != null && !baseObject.Death && !baseObject.Ghost && IsProperTarget(baseObject))
            {
                result++;
            }
            for (int i = 0; i < 7; i++)
            {
                if (Envir.GetNextPosition(nCurrX, nCurrY, (byte)i, 1, ref nX, ref nY))
                {
                    baseObject = Envir.GetMovingObject(nX, nY, true);
                    if (baseObject != null && !baseObject.Death && !baseObject.Ghost && IsProperTarget(baseObject))
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
                    for (int i = 0; i < 7; i++)
                    {
                        if (Master.Envir.GetNextPosition(Master.CurrX, Master.CurrY, (byte)i, 1, ref nX, ref nY))
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
                for (int i = 0; i < 2; i++)
                {
                    // 判断主人是否在英雄对面
                    if (Master.Envir.GetNextPosition(Master.CurrX, Master.CurrY, Master.Dir, i, ref nX, ref nY))
                    {
                        if (CurrX == nX && CurrY == nY)
                        {
                            if (Master.GetBackPosition(ref nX, ref nY) && GotoNext(nX, nY, true))
                            {
                                return true;
                            }
                            for (int k = 0; k < 2; k++)
                            {
                                for (int j = 0; j < 7; j++)
                                {
                                    if (j != Master.Dir)
                                    {
                                        if (Master.Envir.GetNextPosition(Master.CurrX, Master.CurrY, (byte)j, k, ref nX, ref nY) && GotoNext(nX, nY, true))
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
                    for (int j = 0; j < 2; j++)
                    {
                        for (int k = 0; k < 7; k++)
                        {
                            if (k != Master.Dir)
                            {
                                if (Master.Envir.GetNextPosition(Master.CurrX, Master.CurrY, (byte)k, j, ref nX, ref nY) && GotoNextOne(nX, nY, true))
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

        private bool FindVisibleActors(BaseObject actorObject)
        {
            bool result = false;
            for (int i = 0; i < VisibleActors.Count; i++)
            {
                if (VisibleActors[i].BaseObject == actorObject)
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
            UserMagic userMagic = FindMagic(wMagIdx);
            if (userMagic != null)
            {
                if (!MagicManager.IsWarrSkill(userMagic.MagIdx))
                {
                    result = userMagic.Key == 0 || IsRobot;
                }
                else
                {
                    result = userMagic.Key == 0 || IsRobot;
                }
            }
            return result;
        }

        private bool CheckUserItem(int nItemType, int nCount)
        {
            return CheckUserItemType(nItemType, nCount) || GetUserItemList(nItemType, nCount) >= 0;
        }

        private static bool CheckItemType(int nItemType, StdItem stdItem)
        {
            bool result = false;
            switch (nItemType)
            {
                case 1:
                    if (stdItem.StdMode == 25 && stdItem.Shape == 1)
                    {
                        result = true;
                    }
                    break;
                case 2:
                    if (stdItem.StdMode == 25 && stdItem.Shape == 2)
                    {
                        result = true;
                    }
                    break;
                case 3:
                    if (stdItem.StdMode == 25 && stdItem.Shape == 3)
                    {
                        result = true;
                    }
                    break;
                case 5:
                    if (stdItem.StdMode == 25 && stdItem.Shape == 5)
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
            if (UseItems[ItemLocation.ArmRingl] != null && UseItems[ItemLocation.ArmRingl].Index > 0 &&
                Math.Round(Convert.ToDouble(UseItems[ItemLocation.ArmRingl].Dura / 100)) >= nCount)
            {
                StdItem stdItem = M2Share.WorldEngine.GetStdItem(UseItems[ItemLocation.ArmRingl].Index);
                if (stdItem != null)
                {
                    result = CheckItemType(nItemType, stdItem);
                }
            }
            return result;
        }

        // 检测包裹中是否有符和毒
        // nType 为指定类型 5 为护身符 1,2 为毒药   3,诅咒术专用
        private int GetUserItemList(int nItemType, int nCount)
        {
            int result = -1;
            for (int i = 0; i < ItemList.Count; i++)
            {
                StdItem stdItem = M2Share.WorldEngine.GetStdItem(ItemList[i].Index);
                if (stdItem != null)
                {
                    if (CheckItemType(nItemType, stdItem) && HUtil32.Round(ItemList[i].Dura / 100) >= nCount)
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
                UserItem userItem = ItemList[nIndex];
                if (UseItems[ItemLocation.ArmRingl].Index > 0)
                {
                    StdItem stdItem = M2Share.WorldEngine.GetStdItem(UseItems[ItemLocation.ArmRingl].Index);
                    if (stdItem != null)
                    {
                        if (CheckItemType(nItemType, stdItem))
                        {
                            result = true;
                        }
                        else
                        {
                            ItemList.RemoveAt(nIndex);
                            UserItem addUserItem = UseItems[ItemLocation.ArmRingl];
                            if (AddItemToBag(addUserItem))
                            {
                                UseItems[ItemLocation.ArmRingl] = userItem;
                                Dispose(userItem);
                                result = true;
                            }
                            else
                            {
                                ItemList.Add(userItem);
                                Dispose(addUserItem);
                            }
                        }
                    }
                    else
                    {
                        ItemList.RemoveAt(nIndex);
                        UseItems[ItemLocation.ArmRingl] = userItem;
                        Dispose(userItem);
                        result = true;
                    }
                }
                else
                {
                    ItemList.RemoveAt(nIndex);
                    UseItems[ItemLocation.ArmRingl] = userItem;
                    Dispose(userItem);
                    result = true;
                }
            }
            return result;
        }

        private int GetRangeTargetCount(short nX, short nY, int nRange)
        {
            int result = 0;
            BaseObject baseObject;
            IList<BaseObject> baseObjectList = new List<BaseObject>();
            if (Envirnoment.GetMapBaseObjects(nX, nY, nRange, baseObjectList))
            {
                for (int i = baseObjectList.Count - 1; i >= 0; i--)
                {
                    baseObject = baseObjectList[i];
                    if (baseObject.HideMode && !CoolEye || !IsProperTarget(baseObject))
                    {
                        baseObjectList.RemoveAt(i);
                    }
                }
                return baseObjectList.Count;
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
            for (int i = 0; i < nStep; i++)
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

        private bool CanAttack(short nCurrX, short nCurrY, BaseObject baseObject, int nRange, ref byte btDir)
        {
            bool result = false;
            short nX = 0;
            short nY = 0;
            btDir = M2Share.GetNextDirection(nCurrX, nCurrY, baseObject.CurrX, baseObject.CurrY);
            for (int i = 0; i < nRange; i++)
            {
                if (!Envir.GetNextPosition(nCurrX, nCurrY, btDir, i, ref nX, ref nY))
                {
                    break;
                }
                if (baseObject.CurrX == nX && baseObject.CurrY == nY)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private bool CanAttack(BaseObject baseObject, int nRange, ref byte btDir)
        {
            short nX = 0;
            short nY = 0;
            bool result = false;
            btDir = M2Share.GetNextDirection(CurrX, CurrY, baseObject.CurrX, baseObject.CurrY);
            for (int i = 0; i < nRange; i++)
            {
                if (!Envir.GetNextPosition(CurrX, CurrY, btDir, i, ref nX, ref nY))
                {
                    break;
                }
                if (baseObject.CurrX == nX && baseObject.CurrY == nY)
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
            UserMagic userMagic;
            bool result = false;
            switch (Job)
            {
                case PlayJob.Warrior:
                    result = true;
                    break;
                case PlayJob.Wizard:
                    for (int i = 0; i < MagicList.Count; i++)
                    {
                        userMagic = MagicList[i];
                        switch (userMagic.MagIdx)
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
                                if (GetSpellPoint(userMagic) <= Abil.MP)
                                {
                                    result = true;
                                    break;
                                }
                                break;
                        }
                    }
                    break;
                case PlayJob.Taoist:
                    for (int i = 0; i < MagicList.Count; i++)
                    {
                        userMagic = MagicList[i];
                        if (userMagic.Magic.Job == 2 || userMagic.Magic.Job == 99)
                        {
                            switch (userMagic.MagIdx)
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

        private bool UseSpell(UserMagic userMagic, short nTargetX, short nTargetY, BaseObject targetBaseObject)
        {
            int n14;
            BaseObject baseObject;
            bool result = false;
            if (!IsCanSpell)
            {
                return false;
            }
            if (Death || StatusTimeArr[PoisonState.LOCKSPELL] != 0)
            {
                return false; // 防麻
            }
            if (StatusTimeArr[PoisonState.STONE] != 0)
            {
                return false;// 防麻
            }
            if (Envir != null)
            {
                if (!Envirnoment.AllowMagics(userMagic.Magic.MagicName))
                {
                    return false;
                }
            }
            var boIsWarrSkill = MagicManager.IsWarrSkill(userMagic.MagIdx);// 是否是战士技能
            SpellTick -= 450;
            SpellTick = HUtil32._MAX(0, SpellTick);
            switch (userMagic.MagIdx)
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
                        if (GetAttackDir(targetBaseObject, ref Dir))
                        {
                            DoMotaebo(Dir, userMagic.Level);
                        }
                    }
                    break;
                case MagicConst.SKILL_43:
                    result = true;
                    break;
                default:
                    n14 = M2Share.GetNextDirection(CurrX, CurrY, nTargetX, nTargetY);
                    Dir = (byte)n14;
                    baseObject = null;
                    if (userMagic.MagIdx >= 60 && userMagic.MagIdx <= 65)
                    {
                        if (CretInNearXy(targetBaseObject, nTargetX, nTargetY))// 检查目标角色，与目标座标误差范围，如果在误差范围内则修正目标座标
                        {
                            baseObject = targetBaseObject;
                            nTargetX = baseObject.CurrX;
                            nTargetY = baseObject.CurrY;
                        }
                    }
                    else
                    {
                        switch (userMagic.MagIdx)
                        {
                            case MagicConst.SKILL_HEALLING:
                            case MagicConst.SKILL_HANGMAJINBUB:
                            case MagicConst.SKILL_DEJIWONHO:
                            case MagicConst.SKILL_BIGHEALLING:
                            case MagicConst.SKILL_SINSU:
                            case MagicConst.SKILL_UNAMYOUNSUL:
                            case MagicConst.SKILL_46:
                                if (BoSelSelf)
                                {
                                    baseObject = this;
                                    nTargetX = CurrX;
                                    nTargetY = CurrY;
                                }
                                else
                                {
                                    if (Master != null)
                                    {
                                        baseObject = Master;
                                        nTargetX = Master.CurrX;
                                        nTargetY = Master.CurrY;
                                    }
                                    else
                                    {
                                        baseObject = this;
                                        nTargetX = CurrX;
                                        nTargetY = CurrY;
                                    }
                                }
                                break;
                            default:
                                if (CretInNearXy(targetBaseObject, nTargetX, nTargetY))
                                {
                                    baseObject = targetBaseObject;
                                    nTargetX = baseObject.CurrX;
                                    nTargetY = baseObject.CurrY;
                                }
                                break;
                        }
                    }
                    if (!AutoSpell(userMagic, nTargetX, nTargetY, baseObject))
                    {
                        SendRefMsg(Messages.RM_MAGICFIREFAIL, 0, 0, 0, 0, "");
                    }
                    result = true;
                    break;
            }
            return result;
        }

        private bool AutoSpell(UserMagic userMagic, short nTargetX, short nTargetY, BaseObject baseObject)
        {
            bool result = false;
            try
            {
                if (baseObject != null)
                {
                    if (baseObject.Ghost || baseObject.Death || baseObject.WAbil.HP <= 0)
                    {
                        return false;
                    }
                }
                if (!MagicManager.IsWarrSkill(userMagic.MagIdx))
                {
                    result = MagicManager.DoSpell(this, userMagic, nTargetX, nTargetY, baseObject);
                    AttackTick = HUtil32.GetTickCount();
                }
            }
            catch (Exception)
            {
                M2Share.Logger.Error(Format("RobotPlayObject.AutoSpell MagID:{0} X:{1} Y:{2}", new object[] { userMagic.MagIdx, nTargetX, nTargetY }));
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
                if (HUtil32.GetTickCount() - MDwThinkTick > 3000)
                {
                    MDwThinkTick = HUtil32.GetTickCount();
                    if (Envir.GetXyObjCount(CurrX, CurrY) >= 2)
                    {
                        MBoDupMode = true;
                    }
                    if (TargetCret != null)
                    {
                        if (!IsProperTarget(TargetCret))
                        {
                            DelTargetCreat();
                        }
                    }
                }
                if (MBoDupMode)
                {
                    int nOldX = CurrX;
                    int nOldY = CurrY;
                    WalkTo(M2Share.RandomNumber.RandomByte(8), false);
                    //m_dwStationTick = HUtil32.GetTickCount(); // 增加检测人物站立时间
                    if (nOldX != CurrX || nOldY != CurrY)
                    {
                        MBoDupMode = false;
                        result = true;
                    }
                }
            }
            catch
            {
                M2Share.Logger.Error("RobotPlayObject.Thinking");
            }
            return result;
        }

        private int CheckTargetXyCount(int nX, int nY, int nRange)
        {
            int nC;
            int n10 = nRange;
            int result = 0;
            if (VisibleActors.Count > 0)
            {
                for (int i = 0; i < VisibleActors.Count; i++)
                {
                    var baseObject = VisibleActors[i].BaseObject;
                    if (baseObject != null)
                    {
                        if (!baseObject.Death)
                        {
                            if (IsProperTarget(baseObject) && (!baseObject.HideMode || CoolEye))
                            {
                                nC = Math.Abs(nX - baseObject.CurrX) + Math.Abs(nY - baseObject.CurrY);
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
        private bool IsNeedGotoXy()
        {
            bool result = false;
            long dwAttackTime;
            if (TargetCret != null && HUtil32.GetTickCount() - AutoAvoidTick > 1100 && (!MBoIsUseAttackMagic || Job == 0))
            {
                if (Job > 0)
                {
                    if (!AutoUseMagic && (Math.Abs(TargetCret.CurrX - CurrX) > 3 || Math.Abs(TargetCret.CurrY - CurrY) > 3))
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
                    switch (AutoMagicId)
                    {
                        case MagicConst.SKILL_ERGUM:
                            if (AllowUseMagic(12) && Envir.GetNextPosition(CurrX, CurrY, Dir, 2, ref TargetX, ref TargetY))
                            {
                                if (Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 0 || Math.Abs(CurrX - TargetCret.CurrX) == 0 && Math.Abs(CurrY - TargetCret.CurrY) == 2 || Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 2)
                                {
                                    dwAttackTime = HUtil32._MAX(0, (int)M2Share.Config.dwHeroWarrorAttackTime - HitSpeed * M2Share.Config.ItemSpeed); // 防止负数出错
                                    if (HUtil32.GetTickCount() - AttackTick > dwAttackTime)
                                    {
                                        HitMode = 4;
                                        TargetFocusTick = HUtil32.GetTickCount();
                                        Dir = M2Share.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
                                        Attack(TargetCret, Dir);
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
                            AutoMagicId = 0;
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
                            if (Envir.GetNextPosition(CurrX, CurrY, Dir, 5, ref TargetX, ref TargetY))
                            {
                                if (Math.Abs(CurrX - TargetCret.CurrX) <= 4 && Math.Abs(CurrY - TargetCret.CurrY) == 0 || Math.Abs(CurrX - TargetCret.CurrX) == 0 && Math.Abs(CurrY - TargetCret.CurrY) <= 4 || Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 2 || Math.Abs(CurrX - TargetCret.CurrX) == 3 && Math.Abs(CurrY - TargetCret.CurrY) == 3 || Math.Abs(CurrX - TargetCret.CurrX) == 4 && Math.Abs(CurrY - TargetCret.CurrY) == 4)
                                {
                                    dwAttackTime = HUtil32._MAX(0, (int)M2Share.Config.dwHeroWarrorAttackTime - HitSpeed * M2Share.Config.ItemSpeed);// 防止负数出错
                                    if (HUtil32.GetTickCount() - AttackTick > dwAttackTime)
                                    {
                                        HitMode = 9;
                                        TargetFocusTick = HUtil32.GetTickCount();
                                        Dir = M2Share.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
                                        Attack(TargetCret, Dir);
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
                            AutoMagicId = 0;
                            if (Envir.GetNextPosition(CurrX, CurrY, Dir, 2, ref TargetX, ref TargetY))
                            {
                                if (Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 0 || Math.Abs(CurrX - TargetCret.CurrX) == 0 && Math.Abs(CurrY - TargetCret.CurrY) == 2 || Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 2)
                                {
                                    dwAttackTime = HUtil32._MAX(0, (int)M2Share.Config.dwHeroWarrorAttackTime - HitSpeed * M2Share.Config.ItemSpeed);
                                    // 防止负数出错
                                    if (HUtil32.GetTickCount() - AttackTick > dwAttackTime)
                                    {
                                        HitMode = 9;
                                        TargetFocusTick = HUtil32.GetTickCount();
                                        Dir = M2Share.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
                                        Attack(TargetCret, Dir);
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
                            AutoMagicId = 0;
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
                                AutoMagicId = 0;
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
        private bool GetGotoXy(BaseObject baseObject, byte nCode)
        {
            bool result = false;
            switch (nCode)
            {
                case 2:// 刺杀位
                    if (CurrX - 2 <= baseObject.CurrX && CurrX + 2 >= baseObject.CurrX && CurrY - 2 <= baseObject.CurrY && CurrY + 2 >= baseObject.CurrY && (CurrX != baseObject.CurrX || CurrY != baseObject.CurrY))
                    {
                        result = true;
                        if (CurrX - 2 == baseObject.CurrX && CurrY == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX - 2);
                            TargetY = CurrY;
                            return result;
                        }
                        if (CurrX + 2 == baseObject.CurrX && CurrY == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX + 2);
                            TargetY = CurrY;
                            return result;
                        }
                        if (CurrX == baseObject.CurrX && CurrY - 2 == baseObject.CurrY)
                        {
                            TargetX = CurrX;
                            TargetY = (short)(CurrY - 2);
                            return result;
                        }
                        if (CurrX == baseObject.CurrX && CurrY + 2 == baseObject.CurrY)
                        {
                            TargetX = CurrX;
                            TargetY = (short)(CurrY + 2);
                            return result;
                        }
                        if (CurrX - 2 == baseObject.CurrX && CurrY - 2 == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX - 2);
                            TargetY = (short)(CurrY - 2);
                            return result;
                        }
                        if (CurrX + 2 == baseObject.CurrX && CurrY - 2 == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX + 2);
                            TargetY = (short)(CurrY - 2);
                            return result;
                        }
                        if (CurrX - 2 == baseObject.CurrX && CurrY + 2 == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX - 2);
                            TargetY = (short)(CurrY + 2);
                            return result;
                        }
                        if (CurrX + 2 == baseObject.CurrX && CurrY + 2 == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX + 2);
                            TargetY = (short)(CurrY + 2);
                            return result;
                        }
                    }
                    break;
                case 3:// 3格
                    if (CurrX - 3 <= baseObject.CurrX && CurrX + 3 >= baseObject.CurrX && CurrY - 3 <= baseObject.CurrY && CurrY + 3 >= baseObject.CurrY && (CurrX != baseObject.CurrX || CurrY != baseObject.CurrY))
                    {
                        result = true;
                        if (CurrX - 3 == baseObject.CurrX && CurrY == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX - 3);
                            TargetY = CurrY;
                            return result;
                        }
                        if (CurrX + 3 == baseObject.CurrX && CurrY == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX + 3);
                            TargetY = CurrY;
                            return result;
                        }
                        if (CurrX == baseObject.CurrX && CurrY - 3 == baseObject.CurrY)
                        {
                            TargetX = CurrX;
                            TargetY = (short)(CurrY - 3);
                            return result;
                        }
                        if (CurrX == baseObject.CurrX && CurrY + 3 == baseObject.CurrY)
                        {
                            TargetX = CurrX;
                            TargetY = (short)(CurrY + 3);
                            return result;
                        }
                        if (CurrX - 3 == baseObject.CurrX && CurrY - 3 == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX - 3);
                            TargetY = (short)(CurrY - 3);
                            return result;
                        }
                        if (CurrX + 3 == baseObject.CurrX && CurrY - 3 == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX + 3);
                            TargetY = (short)(CurrY - 3);
                            return result;
                        }
                        if (CurrX - 3 == baseObject.CurrX && CurrY + 3 == baseObject.CurrY)
                        {
                            TargetX = (short)(CurrX - 3);
                            TargetY = (short)(CurrY + 3);
                            return result;
                        }
                        if (CurrX + 3 == baseObject.CurrX && CurrY + 3 == baseObject.CurrY)
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
        private bool RunToTargetXy(short nTargetX, short nTargetY)
        {
            bool result = false;
            if (Transparent && HideMode)
            {
                StatusTimeArr[PoisonState.STATETRANSPARENT] = 1;// 隐身,一动就显身
            }
            if (StatusTimeArr[PoisonState.STONE] > 0 && StatusTimeArr[PoisonState.DONTMOVE] != 0 || StatusTimeArr[PoisonState.LOCKSPELL] != 0)// || (m_wStatusTimeArrValue[23] != 0)
            {
                return false; // 麻痹不能跑动 
            }
            if (!IsCanRun) // 禁止跑,则退出
            {
                return false;
            }
            if (HUtil32.GetTickCount() - DwTick5F4 > RunIntervalTime) // 跑步使用单独的变量计数
            {
                short nX = nTargetX;
                short nY = nTargetY;
                byte nDir = M2Share.GetNextDirection(CurrX, CurrY, nX, nY);
                if (!RobotRunTo(nDir, false, nTargetX, nTargetY))
                {
                    result = WalkToTargetXy(nTargetX, nTargetY);
                    if (result)
                    {
                        DwTick5F4 = HUtil32.GetTickCount();
                    }
                }
                else
                {
                    if (Math.Abs(nTargetX - CurrX) <= 1 && Math.Abs(nTargetY - CurrY) <= 1)
                    {
                        result = true;
                        DwTick5F4 = HUtil32.GetTickCount();
                    }
                }
            }
            return result;
        }

        private bool RobotRunTo(byte btDir, bool boFlag, short nDestX, short nDestY)
        {
            const string sExceptionMsg = "[Exception] TBaseObject::RunTo";
            bool result = false;
            try
            {
                int nOldX = CurrX;
                int nOldY = CurrY;
                Dir = btDir;
                bool canWalk = M2Share.Config.DiableHumanRun || Permission > 9 && M2Share.Config.boGMRunAll || M2Share.Config.boSafeAreaLimited && InSafeZone();
                switch (btDir)
                {
                    case Direction.Up:
                        if (CurrY > 1 && Envir.CanWalkEx(CurrX, CurrY - 1, canWalk) && Envir.CanWalkEx(CurrX, CurrY - 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX, CurrY - 2, true))
                        {
                            CurrY -= 2;
                        }
                        break;
                    case Direction.UpRight:
                        if (CurrX < Envir.Width - 2 && CurrY > 1 && Envir.CanWalkEx(CurrX + 1, CurrY - 1, canWalk) && Envir.CanWalkEx(CurrX + 2, CurrY - 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 2, CurrY - 2, true))
                        {
                            CurrX += 2;
                            CurrY -= 2;
                        }
                        break;
                    case Direction.Right:
                        if (CurrX < Envir.Width - 2 && Envir.CanWalkEx(CurrX + 1, CurrY, canWalk) && Envir.CanWalkEx(CurrX + 2, CurrY, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 2, CurrY, true))
                        {
                            CurrX += 2;
                        }
                        break;
                    case Direction.DownRight:
                        if (CurrX < Envir.Width - 2 && CurrY < Envir.Height - 2 && Envir.CanWalkEx(CurrX + 1, CurrY + 1, canWalk) && Envir.CanWalkEx(CurrX + 2, CurrY + 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX + 2, CurrY + 2, true))
                        {
                            CurrX += 2;
                            CurrY += 2;
                        }
                        break;
                    case Direction.Down:
                        if (CurrY < Envir.Height - 2 && Envir.CanWalkEx(CurrX, CurrY + 1, canWalk) && Envir.CanWalkEx(CurrX, CurrY + 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX, CurrY + 2, true))
                        {
                            CurrY += 2;
                        }
                        break;
                    case Direction.DownLeft:
                        if (CurrX > 1 && CurrY < Envir.Height - 2 && Envir.CanWalkEx(CurrX - 1, CurrY + 1, canWalk) && Envir.CanWalkEx(CurrX - 2, CurrY + 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 2, CurrY + 2, true))
                        {
                            CurrX -= 2;
                            CurrY += 2;
                        }

                        break;
                    case Direction.Left:
                        if (CurrX > 1 && Envir.CanWalkEx(CurrX - 1, CurrY, canWalk) && Envir.CanWalkEx(CurrX - 2, CurrY, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 2, CurrY, true))
                        {
                            CurrX -= 2;
                        }
                        break;
                    case Direction.UpLeft:
                        if (CurrX > 1 && CurrY > 1 && Envir.CanWalkEx(CurrX - 1, CurrY - 1, canWalk) && Envir.CanWalkEx(CurrX - 2, CurrY - 2, canWalk) && Envir.MoveToMovingObject(CurrX, CurrY, this, CurrX - 2, CurrY - 2, true))
                        {
                            CurrX -= 2;
                            CurrY -= 2;
                        }
                        break;
                }
                if (CurrX != nOldX || CurrY != nOldY)
                {
                    if (Walk(Messages.RM_RUN))
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
                M2Share.Logger.Error(sExceptionMsg);
            }
            return result;
        }

        /// <summary>
        /// 走向目标
        /// </summary>
        /// <returns></returns>
        private bool WalkToTargetXy(int nTargetX, int nTargetY)
        {
            int n10;
            int n14;
            int n20;
            int nOldX;
            int nOldY;
            bool result = false;
            if (Transparent && HideMode)
            {
                StatusTimeArr[PoisonState.STATETRANSPARENT] = 1;// 隐身,一动就显身
            }
            if (StatusTimeArr[PoisonState.STONE] != 0 && StatusTimeArr[PoisonState.DONTMOVE] != 0 || StatusTimeArr[PoisonState.LOCKSPELL] != 0)
            {
                return false;// 麻痹不能跑动
            }
            if (Math.Abs(nTargetX - CurrX) > 1 || Math.Abs(nTargetY - CurrY) > 1)
            {
                if (HUtil32.GetTickCount() - DwTick3F4 > WalkIntervalTime)
                {
                    n10 = nTargetX;
                    n14 = nTargetY;
                    byte nDir = Direction.Down;
                    if (n10 > CurrX)
                    {
                        nDir = Direction.Right;
                        if (n14 > CurrY)
                        {
                            nDir = Direction.DownRight;
                        }
                        if (n14 < CurrY)
                        {
                            nDir = Direction.UpRight;
                        }
                    }
                    else
                    {
                        if (n10 < CurrX)
                        {
                            nDir = Direction.Left;
                            if (n14 > CurrY)
                            {
                                nDir = Direction.DownLeft;
                            }
                            if (n14 < CurrY)
                            {
                                nDir = Direction.UpLeft;
                            }
                        }
                        else
                        {
                            if (n14 > CurrY)
                            {
                                nDir = Direction.Down;
                            }
                            else if (n14 < CurrY)
                            {
                                nDir = Direction.Up;
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
                        for (byte i = Direction.Up; i <= Direction.UpLeft; i++)
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
                                    nDir = Direction.UpLeft;
                                }
                                if (nDir > Direction.UpLeft)
                                {
                                    nDir = Direction.Up;
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
        /// <returns></returns>
        private bool GotoTargetXy(short nTargetX, short nTargetY, int nCode)
        {
            bool result = false;
            switch (nCode)
            {
                case 0:// 正常模式
                    if (Math.Abs(CurrX - nTargetX) > 2 || Math.Abs(CurrY - nTargetY) > 2)
                    {
                        if (StatusTimeArr[PoisonState.LOCKRUN] == 0)
                        {
                            result = RunToTargetXy(nTargetX, nTargetY);
                        }
                        else
                        {
                            result = WalkToTargetXy2(nTargetX, nTargetY);// 转向
                        }
                    }
                    else
                    {
                        result = WalkToTargetXy2(nTargetX, nTargetY);// 转向
                    }
                    break;
                case 1:// 躲避模式
                    if (Math.Abs(CurrX - nTargetX) > 1 || Math.Abs(CurrY - nTargetY) > 1)
                    {
                        if (StatusTimeArr[PoisonState.LOCKRUN] == 0)
                        {
                            result = RunToTargetXy(nTargetX, nTargetY);
                        }
                        else
                        {
                            result = WalkToTargetXy2(nTargetX, nTargetY);// 转向
                        }
                    }
                    else
                    {
                        result = WalkToTargetXy2(nTargetX, nTargetY);// 转向
                    }
                    break;
            }
            return result;
        }

        private void SearchMagic()
        {
            AutoMagicId = SelectMagic();
            if (AutoMagicId > 0)
            {
                UserMagic userMagic = FindMagic(AutoMagicId);
                if (userMagic != null)
                {
                    MBoIsUseAttackMagic = IsUseAttackMagic();
                }
                else
                {
                    MBoIsUseAttackMagic = false;
                }
            }
            else
            {
                MBoIsUseAttackMagic = false;
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
                        if (AllowUseMagic(27) && HUtil32.GetTickCount() - MSkillUseTick[27] > 10000)
                        {
                            // pk时如果对方等级比自己低就每隔一段时间用一次野蛮  
                            MSkillUseTick[27] = HUtil32.GetTickCount();
                            result = 27;
                            return result;
                        }
                    }
                    else
                    {
                        // 打怪使用 
                        if (AllowUseMagic(27) && HUtil32.GetTickCount() - MSkillUseTick[27] > 10000 && TargetCret.Abil.Level < Abil.Level && WAbil.HP <= Math.Round(Abil.MaxHP * 0.85))
                        {
                            MSkillUseTick[27] = HUtil32.GetTickCount();
                            result = 27;
                            return result;
                        }
                    }
                    if (TargetCret.Master != null)
                    {
                        ExpHitter = TargetCret.Master;
                    }
                    if (CheckTargetXyCount1(CurrX, CurrY, 1) > 1)
                    {
                        switch (M2Share.RandomNumber.Random(3))
                        {
                            case 0:// 被怪物包围
                                if (AllowUseMagic(41) && HUtil32.GetTickCount() - MSkillUseTick[41] > 10000 && TargetCret.Abil.Level < Abil.Level && (TargetCret.Race != ActorRace.Play || M2Share.Config.GroupMbAttackPlayObject) && Math.Abs(TargetCret.CurrX - CurrX) <= 3 && Math.Abs(TargetCret.CurrY - CurrY) <= 3)
                                {
                                    MSkillUseTick[41] = HUtil32.GetTickCount();// 狮子吼
                                    result = 41;
                                    return result;
                                }
                                if (AllowUseMagic(7) && HUtil32.GetTickCount() - MSkillUseTick[7] > 10000)// 攻杀剑术 
                                {
                                    MSkillUseTick[7] = HUtil32.GetTickCount();
                                    PowerHit = true;// 开启攻杀
                                    result = 7;
                                    return result;
                                }
                                if (AllowUseMagic(39) && HUtil32.GetTickCount() - MSkillUseTick[39] > 10000)
                                {
                                    MSkillUseTick[39] = HUtil32.GetTickCount();// 彻地钉
                                    result = 39;
                                    return result;
                                }
                                if (AllowUseMagic(MagicConst.SKILL_BANWOL))// 半月弯刀
                                {
                                    if (CheckTargetXyCount2(MagicConst.SKILL_BANWOL) > 0)
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
                                if (AllowUseMagic(41) && HUtil32.GetTickCount() - MSkillUseTick[41] > 10000 && TargetCret.Abil.Level < Abil.Level && (TargetCret.Race != ActorRace.Play || M2Share.Config.GroupMbAttackPlayObject) && Math.Abs(TargetCret.CurrX - CurrX) <= 3 && Math.Abs(TargetCret.CurrY - CurrY) <= 3)
                                {
                                    MSkillUseTick[41] = HUtil32.GetTickCount(); // 狮子吼
                                    result = 41;
                                    return result;
                                }
                                if (AllowUseMagic(7) && HUtil32.GetTickCount() - MSkillUseTick[7] > 10000)// 攻杀剑术 
                                {
                                    MSkillUseTick[7] = HUtil32.GetTickCount();
                                    PowerHit = true;
                                    result = 7;
                                    return result;
                                }
                                if (AllowUseMagic(39) && HUtil32.GetTickCount() - MSkillUseTick[39] > 10000)
                                {
                                    MSkillUseTick[39] = HUtil32.GetTickCount(); // 英雄彻地钉
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
                                    if (CheckTargetXyCount2(MagicConst.SKILL_BANWOL) > 0)
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
                                if (AllowUseMagic(41) && HUtil32.GetTickCount() - MSkillUseTick[41] > 10000 && TargetCret.Abil.Level < Abil.Level && (TargetCret.Race != ActorRace.Play || M2Share.Config.GroupMbAttackPlayObject) && Math.Abs(TargetCret.CurrX - CurrX) <= 3 && Math.Abs(TargetCret.CurrY - CurrY) <= 3)
                                {
                                    MSkillUseTick[41] = HUtil32.GetTickCount();// 狮子吼
                                    result = 41;
                                    return result;
                                }
                                if (AllowUseMagic(39) && HUtil32.GetTickCount() - MSkillUseTick[39] > 10000)
                                {
                                    MSkillUseTick[39] = HUtil32.GetTickCount();// 英雄彻地钉
                                    result = 39;
                                    return result;
                                }
                                if (AllowUseMagic(7) && HUtil32.GetTickCount() - MSkillUseTick[7] > 10000)// 攻杀剑术
                                {
                                    MSkillUseTick[7] = HUtil32.GetTickCount();
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
                                    if (CheckTargetXyCount2(MagicConst.SKILL_BANWOL) > 0)
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
                        if ((TargetCret.Race == ActorRace.Play || TargetCret.Master != null) && CheckTargetXyCount1(CurrX, CurrY, 1) > 1)
                        {
                            // PK  身边超过2个目标才使用
                            if (AllowUseMagic(40) && (HUtil32.GetTickCount() - MSkillUseTick[40]) > 3000)// 英雄抱月刀法
                            {
                                MSkillUseTick[40] = HUtil32.GetTickCount();
                                if (!CrsHitkill)
                                {
                                    SkillCrsOnOff(true);
                                }
                                result = 40;
                                return result;
                            }
                            if ((HUtil32.GetTickCount() - MSkillUseTick[25]) > 1500)
                            {
                                if (AllowUseMagic(MagicConst.SKILL_BANWOL))
                                {
                                    // 半月弯刀
                                    if (CheckTargetXyCount2(MagicConst.SKILL_BANWOL) > 0)
                                    {
                                        MSkillUseTick[25] = HUtil32.GetTickCount();
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
                        if (AllowUseMagic(7) && (HUtil32.GetTickCount() - MSkillUseTick[7]) > 10000) // 少于三个怪用 刺杀剑术
                        {
                            MSkillUseTick[7] = HUtil32.GetTickCount();
                            PowerHit = true;// 开启攻杀
                            result = 7;
                            return result;
                        }
                        if (HUtil32.GetTickCount() - MSkillUseTick[12] > 1000)
                        {
                            if (AllowUseMagic(12))// 英雄刺杀剑术
                            {
                                if (!UseThrusting)
                                {
                                    ThrustingOnOff(true);
                                }
                                MSkillUseTick[12] = HUtil32.GetTickCount();
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
                    if (AllowUseMagic(40) && (HUtil32.GetTickCount() - MSkillUseTick[40]) > 3000 && CheckTargetXyCount1(CurrX, CurrY, 1) > 1)
                    {
                        // 英雄抱月刀法
                        if (!CrsHitkill)
                        {
                            SkillCrsOnOff(true);
                        }
                        MSkillUseTick[40] = HUtil32.GetTickCount();
                        result = 40;
                        return result;
                    }
                    if (AllowUseMagic(39) && (HUtil32.GetTickCount() - MSkillUseTick[39]) > 3000) // 英雄彻地钉
                    {
                        MSkillUseTick[39] = HUtil32.GetTickCount();
                        result = 39;
                        return result;
                    }
                    if ((HUtil32.GetTickCount() - MSkillUseTick[25]) > 3000)
                    {
                        if (AllowUseMagic(MagicConst.SKILL_BANWOL))// 半月弯刀
                        {
                            if (!UseHalfMoon)
                            {
                                HalfMoonOnOff(true);
                            }
                            MSkillUseTick[25] = HUtil32.GetTickCount();
                            result = MagicConst.SKILL_BANWOL;
                            return result;
                        }
                    }
                    if ((HUtil32.GetTickCount() - MSkillUseTick[12]) > 3000)// 英雄刺杀剑术
                    {
                        if (AllowUseMagic(12))
                        {
                            if (!UseThrusting)
                            {
                                ThrustingOnOff(true);
                            }
                            MSkillUseTick[12] = HUtil32.GetTickCount();
                            result = 12;
                            return result;
                        }
                    }
                    if (AllowUseMagic(7) && (HUtil32.GetTickCount() - MSkillUseTick[7]) > 3000)// 攻杀剑术
                    {
                        PowerHit = true;
                        MSkillUseTick[7] = HUtil32.GetTickCount();
                        result = 7;
                        return result;
                    }
                    if ((TargetCret.Race == ActorRace.Play || TargetCret.Master != null) && TargetCret.Abil.Level < Abil.Level && WAbil.HP <= Math.Round(WAbil.MaxHP * 0.6))
                    {
                        // PK时,使用野蛮冲撞
                        if (AllowUseMagic(27) && (HUtil32.GetTickCount() - MSkillUseTick[27]) > 3000)
                        {
                            MSkillUseTick[27] = HUtil32.GetTickCount();
                            result = 27;
                            return result;
                        }
                    }
                    else
                    {
                        if (AllowUseMagic(27) && TargetCret.Abil.Level < Abil.Level && WAbil.HP <= Math.Round(Abil.MaxHP * 0.6) && HUtil32.GetTickCount() - MSkillUseTick[27] > 3000)
                        {
                            MSkillUseTick[27] = HUtil32.GetTickCount();
                            result = 27;
                            return result;
                        }
                    }
                    if (AllowUseMagic(41) && HUtil32.GetTickCount() - MSkillUseTick[41] > 10000 && TargetCret.Abil.Level < Abil.Level && (TargetCret.Race != ActorRace.Play || M2Share.Config.GroupMbAttackPlayObject) && Math.Abs(TargetCret.CurrX - CurrX) <= 3 && Math.Abs(TargetCret.CurrY - CurrY) <= 3)
                    {
                        MSkillUseTick[41] = HUtil32.GetTickCount();// 狮子吼
                        result = 41;
                        return result;
                    }
                    break;
                case PlayJob.Wizard: // 法师
                    if (StatusTimeArr[PoisonState.BubbleDefenceUP] == 0 && !AbilMagBubbleDefence) // 使用 魔法盾
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
                    if ((TargetCret.Race == ActorRace.Play || TargetCret.Master != null) && CheckTargetXyCount3(CurrX, CurrY, 1, 0) > 0 && TargetCret.Abil.Level < Abil.Level)
                    {
                        // PK时,旁边有人贴身,使用抗拒火环
                        if (AllowUseMagic(8) && HUtil32.GetTickCount() - MSkillUseTick[8] > 3000)
                        {
                            MSkillUseTick[8] = HUtil32.GetTickCount();
                            result = 8;
                            return result;
                        }
                    }
                    else
                    {
                        // 打怪,怪级低于自己,并且有怪包围自己就用 抗拒火环
                        if (AllowUseMagic(8) && HUtil32.GetTickCount() - MSkillUseTick[8] > 3000 && CheckTargetXyCount3(CurrX, CurrY, 1, 0) > 0 && TargetCret.Abil.Level < Abil.Level)
                        {
                            MSkillUseTick[8] = HUtil32.GetTickCount();
                            result = 8;
                            return result;
                        }
                    }
                    if (AllowUseMagic(45) && HUtil32.GetTickCount() - MSkillUseTick[45] > 3000)
                    {
                        MSkillUseTick[45] = HUtil32.GetTickCount();
                        result = 45;// 英雄灭天火
                        return result;
                    }
                    if (HUtil32.GetTickCount() - MSkillUseTick[10] > 5000 && Envir.GetNextPosition(CurrX, CurrY, Dir, 5, ref TargetX, ref TargetY))
                    {
                        if ((TargetCret.Race == ActorRace.Play || TargetCret.Master != null) && GetDirBaseObjectsCount(Dir, 5) > 0 && (Math.Abs(CurrX - TargetCret.CurrX) <= 4 && Math.Abs(CurrY - TargetCret.CurrY) == 0 || Math.Abs(CurrX - TargetCret.CurrX) == 0 && Math.Abs(CurrY - TargetCret.CurrY) <= 4 || Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 2 || Math.Abs(CurrX - TargetCret.CurrX) == 3 && Math.Abs(CurrY - TargetCret.CurrY) == 3 || Math.Abs(CurrX - TargetCret.CurrX) == 4 && Math.Abs(CurrY - TargetCret.CurrY) == 4))
                        {
                            if (AllowUseMagic(10))
                            {
                                MSkillUseTick[10] = HUtil32.GetTickCount();
                                result = 10;// 英雄疾光电影 
                                return result;
                            }
                            else if (AllowUseMagic(9))
                            {
                                MSkillUseTick[10] = HUtil32.GetTickCount();
                                result = 9;// 地狱火
                                return result;
                            }
                        }
                        else if (GetDirBaseObjectsCount(Dir, 5) > 1 && (Math.Abs(CurrX - TargetCret.CurrX) <= 4 && Math.Abs(CurrY - TargetCret.CurrY) == 0 || Math.Abs(CurrX - TargetCret.CurrX) == 0 && Math.Abs(CurrY - TargetCret.CurrY) <= 4 || Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 2 || Math.Abs(CurrX - TargetCret.CurrX) == 3 && Math.Abs(CurrY - TargetCret.CurrY) == 3 || Math.Abs(CurrX - TargetCret.CurrX) == 4 && Math.Abs(CurrY - TargetCret.CurrY) == 4))
                        {
                            if (AllowUseMagic(10))
                            {
                                MSkillUseTick[10] = HUtil32.GetTickCount();
                                result = 10;// 英雄疾光电影 
                                return result;
                            }
                            else if (AllowUseMagic(9))
                            {
                                MSkillUseTick[10] = HUtil32.GetTickCount();
                                result = 9;// 地狱火
                                return result;
                            }
                        }
                    }
                    if (AllowUseMagic(32) && HUtil32.GetTickCount() - MSkillUseTick[32] > 10000 && TargetCret.Abil.Level < M2Share.Config.MagTurnUndeadLevel && TargetCret.LifeAttrib == Grobal2.LA_UNDEAD && TargetCret.Abil.Level < Abil.Level - 1)
                    {
                        // 目标为不死系
                        MSkillUseTick[32] = HUtil32.GetTickCount();
                        result = 32;// 圣言术
                        return result;
                    }
                    if (CheckTargetXyCount(CurrX, CurrY, 2) > 1)// 被怪物包围
                    {
                        if (AllowUseMagic(22) && (HUtil32.GetTickCount() - MSkillUseTick[22]) > 10000)
                        {
                            if (TargetCret.Race != 101 && TargetCret.Race != 102 && TargetCret.Race != 104) // 除祖玛怪,才放火墙
                            {
                                MSkillUseTick[22] = HUtil32.GetTickCount();
                                result = 22;// 火墙
                                return result;
                            }
                        }
                        // 地狱雷光,只对祖玛(101,102,104)，沃玛(91,92,97)，野猪(81)系列的
                        // 遇到祖玛的怪应该多用地狱雷光，夹杂雷电术，少用冰咆哮
                        if (new ArrayList(new byte[] { 91, 92, 97, 101, 102, 104 }).Contains(TargetCret.Race))
                        {
                            // 1000 * 4
                            if (AllowUseMagic(24) && (HUtil32.GetTickCount() - MSkillUseTick[24]) > 4000 && CheckTargetXyCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                            {
                                MSkillUseTick[24] = HUtil32.GetTickCount();
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
                            else if (AllowUseMagic(33) && CheckTargetXyCount(TargetCret.CurrX, TargetCret.CurrY, 2) > 2)
                            {
                                result = 33;// 英雄冰咆哮
                                return result;
                            }
                            else if (HUtil32.GetTickCount() - MSkillUseTick[58] > 1500 && CheckTargetXyCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                            {
                                if (AllowUseMagic(92))
                                {
                                    MSkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 92;// 四级流星火雨
                                    return result;
                                }
                                if (AllowUseMagic(58))
                                {
                                    MSkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 58;// 流星火雨
                                    return result;
                                }
                            }
                        }
                        switch (M2Share.RandomNumber.Random(4))// 随机选择魔法
                        {
                            case 0: // 火球术,大火球,雷电术,爆裂火焰,英雄冰咆哮,流星火雨 从高到低选择
                                if (AllowUseMagic(92) && (HUtil32.GetTickCount() - MSkillUseTick[58]) > 1500 && CheckTargetXyCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    MSkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 92;// 四级流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(58) && (HUtil32.GetTickCount() - MSkillUseTick[58]) > 1500 && CheckTargetXyCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    MSkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 58;// 流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(33) && CheckTargetXyCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)
                                {
                                    result = 33;// 英雄冰咆哮
                                    return result;
                                }
                                else if (AllowUseMagic(23) && CheckTargetXyCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)
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
                                if (AllowUseMagic(92) && (HUtil32.GetTickCount() - MSkillUseTick[58]) > 1500 && CheckTargetXyCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    MSkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 92;// 四级流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(58) && (HUtil32.GetTickCount() - MSkillUseTick[58]) > 1500 && CheckTargetXyCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    MSkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 58;// 流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(33) && CheckTargetXyCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)
                                {
                                    // 火球术,大火球,地狱火,爆裂火焰,冰咆哮  从高到低选择
                                    result = 33;// 冰咆哮
                                    return result;
                                }
                                else if (AllowUseMagic(23) && CheckTargetXyCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)
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
                                if (AllowUseMagic(92) && (HUtil32.GetTickCount() - MSkillUseTick[58]) > 1500 && CheckTargetXyCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    MSkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 92;// 四级流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(58) && (HUtil32.GetTickCount() - MSkillUseTick[58]) > 1500 && CheckTargetXyCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    MSkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 58;// 流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(33) && CheckTargetXyCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)
                                {
                                    // 火球术,大火球,地狱火,爆裂火焰 从高到低选择
                                    result = 33;
                                    return result;
                                }
                                else if (AllowUseMagic(23) && CheckTargetXyCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)
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
                                if (AllowUseMagic(92) && (HUtil32.GetTickCount() - MSkillUseTick[58] > 1500) && CheckTargetXyCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    MSkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 92; // 四级流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(58) && (HUtil32.GetTickCount() - MSkillUseTick[58]) > 1500 && CheckTargetXyCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
                                {
                                    MSkillUseTick[58] = HUtil32.GetTickCount();
                                    result = 58; // 流星火雨
                                    return result;
                                }
                                else if (AllowUseMagic(33) && CheckTargetXyCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)// 火球术,大火球,地狱火,爆裂火焰 从高到低选择
                                {
                                    result = 33;
                                    return result;
                                }
                                else if (AllowUseMagic(23) && CheckTargetXyCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 1)
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
                        if (AllowUseMagic(22) && HUtil32.GetTickCount() - MSkillUseTick[22] > 10000)
                        {
                            if (TargetCret.Race != 101 && TargetCret.Race != 102 && TargetCret.Race != 104)// 除祖玛怪,才放火墙
                            {
                                MSkillUseTick[22] = HUtil32.GetTickCount();
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
                    if ((HUtil32.GetTickCount() - MSkillUseTick[58]) > 1500)
                    {
                        if (AllowUseMagic(92))// 四级流星火雨
                        {
                            MSkillUseTick[58] = HUtil32.GetTickCount();
                            result = 92;
                            return result;
                        }
                        if (AllowUseMagic(58)) // 流星火雨
                        {
                            MSkillUseTick[58] = HUtil32.GetTickCount();
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
                    if (AllowUseMagic(24) && CheckTargetXyCount(TargetCret.CurrX, TargetCret.CurrY, 3) > 2)
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
                    if (AllowUseMagic(10) && Envir.GetNextPosition(CurrX, CurrY, Dir, 5, ref TargetX, ref TargetY) && (Math.Abs(CurrX - TargetCret.CurrX) <= 4 && Math.Abs(CurrY - TargetCret.CurrY) == 0 || Math.Abs(CurrX - TargetCret.CurrX) == 0 && Math.Abs(CurrY - TargetCret.CurrY) <= 4 || Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 2 || Math.Abs(CurrX - TargetCret.CurrX) == 3 && Math.Abs(CurrY - TargetCret.CurrY) == 3 || Math.Abs(CurrX - TargetCret.CurrX) == 4 && Math.Abs(CurrY - TargetCret.CurrY) == 4))
                    {
                        result = 10; // 英雄疾光电影
                        return result;
                    }
                    if (AllowUseMagic(9) && Envir.GetNextPosition(CurrX, CurrY, Dir, 5, ref TargetX, ref TargetY) && (Math.Abs(CurrX - TargetCret.CurrX) <= 4 && Math.Abs(CurrY - TargetCret.CurrY) == 0 || Math.Abs(CurrX - TargetCret.CurrX) == 0 && Math.Abs(CurrY - TargetCret.CurrY) <= 4 || Math.Abs(CurrX - TargetCret.CurrX) == 2 && Math.Abs(CurrY - TargetCret.CurrY) == 2 || Math.Abs(CurrX - TargetCret.CurrX) == 3 && Math.Abs(CurrY - TargetCret.CurrY) == 3 || Math.Abs(CurrX - TargetCret.CurrX) == 4 && Math.Abs(CurrY - TargetCret.CurrY) == 4))
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
                    if (SlaveList.Count == 0 && CheckHeroAmulet(1, 5) && HUtil32.GetTickCount() - MSkillUseTick[17] > 3000 && (AllowUseMagic(72) || AllowUseMagic(30) || AllowUseMagic(17)) && Abil.MP > 20)
                    {
                        MSkillUseTick[17] = HUtil32.GetTickCount(); // 默认,从高到低
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
                    if (StatusTimeArr[PoisonState.BubbleDefenceUP] == 0 && !AbilMagBubbleDefence)
                    {
                        if (AllowUseMagic(73)) // 道力盾
                        {
                            result = 73;
                            return result;
                        }
                    }
                    if ((TargetCret.Race == ActorRace.Play || TargetCret.Master != null) && CheckTargetXyCount3(CurrX, CurrY, 1, 0) > 0 && TargetCret.Abil.Level <= Abil.Level)
                    {
                        // PK时,旁边有人贴身,使用气功波
                        if (AllowUseMagic(48) && HUtil32.GetTickCount() - MSkillUseTick[48] > 3000)
                        {
                            MSkillUseTick[48] = HUtil32.GetTickCount();
                            result = 48;
                            return result;
                        }
                    }
                    else
                    {
                        // 打怪,怪级低于自己,并且有怪包围自己就用 气功波
                        if (AllowUseMagic(48) && HUtil32.GetTickCount() - MSkillUseTick[48] > 5000 && CheckTargetXyCount3(CurrX, CurrY, 1, 0) > 0 && TargetCret.Abil.Level <= Abil.Level)
                        {
                            MSkillUseTick[48] = HUtil32.GetTickCount();
                            result = 48;
                            return result;
                        }
                    }
                    // 绿毒
                    if (TargetCret.StatusTimeArr[PoisonState.DECHEALTH] == 0 && GetUserItemList(2, 1) >= 0 && (M2Share.Config.btHeroSkillMode || !M2Share.Config.btHeroSkillMode && TargetCret.WAbil.HP >= 700
                                                                                                                                                     || TargetCret.Race == ActorRace.Play) && (Math.Abs(TargetCret.CurrX - CurrX) < 7 || Math.Abs(TargetCret.CurrY - CurrY) < 7)
                        && !M2Share.RobotPlayRaceMap.Contains(TargetCret.Race))
                    {
                        // 对于血量超过800的怪用 不毒城墙
                        NAmuletIndx = 0;
                        switch (M2Share.RandomNumber.Random(2))
                        {
                            case 0:
                                if (AllowUseMagic(38) && HUtil32.GetTickCount() - MSkillUseTick[38] > 1000)
                                {
                                    if (Envir != null)// 判断地图是否禁用
                                    {
                                        if (Envirnoment.AllowMagics(MagicConst.SKILL_GROUPAMYOUNSUL, 1))
                                        {
                                            MSkillUseTick[38] = HUtil32.GetTickCount();
                                            result = MagicConst.SKILL_GROUPAMYOUNSUL;// 英雄群体施毒
                                            return result;
                                        }
                                    }
                                }
                                else if ((HUtil32.GetTickCount() - MSkillUseTick[6]) > 1000)
                                {
                                    if (AllowUseMagic(MagicConst.SKILL_AMYOUNSUL))
                                    {
                                        if (Envir != null)
                                        {
                                            if (Envirnoment.AllowMagics(MagicConst.SKILL_AMYOUNSUL, 1))// 判断地图是否禁用
                                            {
                                                MSkillUseTick[6] = HUtil32.GetTickCount();
                                                result = MagicConst.SKILL_AMYOUNSUL;// 英雄施毒术
                                                return result;
                                            }
                                        }
                                    }
                                }
                                break;
                            case 1:
                                if ((HUtil32.GetTickCount() - MSkillUseTick[6]) > 1000)
                                {
                                    if (AllowUseMagic(MagicConst.SKILL_AMYOUNSUL))
                                    {
                                        if (Envir != null)
                                        {
                                            if (Envirnoment.AllowMagics(MagicConst.SKILL_AMYOUNSUL, 1))// 判断地图是否禁用
                                            {
                                                MSkillUseTick[6] = HUtil32.GetTickCount();
                                                result = MagicConst.SKILL_AMYOUNSUL; // 英雄施毒术
                                                return result;
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    if (TargetCret.StatusTimeArr[PoisonState.DAMAGEARMOR] == 0 && GetUserItemList(2, 2) >= 0 && (M2Share.Config.btHeroSkillMode || !M2Share.Config.btHeroSkillMode && TargetCret.WAbil.HP >= 700
                            || TargetCret.Race == ActorRace.Play) && (Math.Abs(TargetCret.CurrX - CurrX) < 7 || Math.Abs(TargetCret.CurrY - CurrY) < 7)
                        && !M2Share.RobotPlayRaceMap.Contains(TargetCret.Race))
                    {
                        // 对于血量超过100的怪用 不毒城墙
                        NAmuletIndx = 0;
                        switch (M2Share.RandomNumber.Random(2))
                        {
                            case 0:
                                if (AllowUseMagic(38) && (HUtil32.GetTickCount() - MSkillUseTick[38]) > 1000)
                                {
                                    if (Envir != null)
                                    {
                                        // 判断地图是否禁用
                                        if (Envirnoment.AllowMagics(MagicConst.SKILL_GROUPAMYOUNSUL, 1))
                                        {
                                            MSkillUseTick[38] = HUtil32.GetTickCount();
                                            result = MagicConst.SKILL_GROUPAMYOUNSUL; // 英雄群体施毒
                                            return result;
                                        }
                                    }
                                }
                                else if ((HUtil32.GetTickCount() - MSkillUseTick[6]) > 1000)
                                {
                                    if (AllowUseMagic(MagicConst.SKILL_AMYOUNSUL))
                                    {
                                        if (Envir != null)
                                        {
                                            // 判断地图是否禁用
                                            if (Envirnoment.AllowMagics(MagicConst.SKILL_AMYOUNSUL, 1))
                                            {
                                                MSkillUseTick[6] = HUtil32.GetTickCount();
                                                result = MagicConst.SKILL_AMYOUNSUL; // 英雄施毒术
                                                return result;
                                            }
                                        }
                                    }
                                }
                                break;
                            case 1:
                                if ((HUtil32.GetTickCount() - MSkillUseTick[6]) > 1000)
                                {
                                    if (AllowUseMagic(MagicConst.SKILL_AMYOUNSUL))
                                    {
                                        if (Envir != null)
                                        {
                                            // 判断地图是否禁用
                                            if (Envirnoment.AllowMagics(MagicConst.SKILL_AMYOUNSUL, 1))
                                            {
                                                MSkillUseTick[6] = HUtil32.GetTickCount();
                                                result = MagicConst.SKILL_AMYOUNSUL; // 英雄施毒术
                                                return result;
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    if (AllowUseMagic(51) && (HUtil32.GetTickCount() - MSkillUseTick[51]) > 5000)// 英雄飓风破 
                    {
                        MSkillUseTick[51] = HUtil32.GetTickCount();
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
                                if (AllowUseMagic(13) && (HUtil32.GetTickCount() - MSkillUseTick[13]) > 3000)
                                {
                                    result = 13;// 英雄灵魂火符
                                    MSkillUseTick[13] = HUtil32.GetTickCount();
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
                                if (AllowUseMagic(13) && (HUtil32.GetTickCount() - MSkillUseTick[13]) > 3000)
                                {
                                    result = 13;// 英雄灵魂火符
                                    MSkillUseTick[13] = HUtil32.GetTickCount();
                                    return result;
                                }
                                break;
                            case 2:
                                if (AllowUseMagic(13) && (HUtil32.GetTickCount() - MSkillUseTick[13]) > 3000)
                                {
                                    result = 13;// 英雄灵魂火符
                                    MSkillUseTick[13] = HUtil32.GetTickCount();
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
        private int CheckTargetXyCount1(int nX, int nY, int nRange)
        {
            int result = 0;
            if (VisibleActors.Count > 0)
            {
                for (int i = 0; i < VisibleActors.Count; i++)
                {
                    BaseObject baseObject = VisibleActors[i].BaseObject;
                    if (baseObject != null)
                    {
                        if (!baseObject.Death)
                        {
                            if (IsProperTarget(baseObject) && (!baseObject.HideMode || CoolEye))
                            {
                                if (Math.Abs(nX - baseObject.CurrX) <= nRange && Math.Abs(nY - baseObject.CurrY) <= nRange)
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
        private int CheckTargetXyCount2(short nMode)
        {
            int result = 0;
            int nC = 0;
            int n10 = 0;
            short nX = 0;
            short nY = 0;
            if (VisibleActors.Count > 0)
            {
                for (int i = 0; i < VisibleActors.Count; i++)
                {
                    switch (nMode)
                    {
                        case MagicConst.SKILL_BANWOL:
                            n10 = (Dir + M2Share.Config.WideAttack[nC]) % 8;
                            break;
                    }
                    if (Envir.GetNextPosition(CurrX, CurrY, (byte)n10, 1, ref nX, ref nY))
                    {
                        BaseObject baseObject = Envir.GetMovingObject(nX, nY, true);
                        if (baseObject != null)
                        {
                            if (!baseObject.Death)
                            {
                                if (IsProperTarget(baseObject) && (!baseObject.HideMode || CoolEye))
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
        /// <returns></returns>
        private int CheckTargetXyCount3(int nX, int nY, int nRange, int nCount)
        {
            int result = 0;
            if (VisibleActors.Count > 0)
            {
                for (int i = 0; i < VisibleActors.Count; i++)
                {
                    BaseObject baseObject = VisibleActors[i].BaseObject;
                    if (baseObject != null)
                    {
                        if (!baseObject.Death)
                        {
                            if (IsProperTarget(baseObject) && (!baseObject.HideMode || CoolEye))
                            {
                                if (Math.Abs(nX - baseObject.CurrX) <= nRange && Math.Abs(nY - baseObject.CurrY) <= nRange)
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
            StdItem amuletStdItem;
            try
            {
                if (UseItems[ItemLocation.ArmRingl].Index > 0)
                {
                    amuletStdItem = M2Share.WorldEngine.GetStdItem(UseItems[ItemLocation.ArmRingl].Index);
                    if (amuletStdItem != null)
                    {
                        if (amuletStdItem.StdMode == 25)
                        {
                            switch (nType)
                            {
                                case 1:
                                    if (amuletStdItem.Shape == 5 && Math.Round(Convert.ToDouble(UseItems[ItemLocation.ArmRingl].Dura / 100)) >= nCount)
                                    {
                                        result = true;
                                        return result;
                                    }
                                    break;
                                case 2:
                                    if (amuletStdItem.Shape <= 2 && Math.Round(Convert.ToDouble(UseItems[ItemLocation.ArmRingl].Dura / 100)) >= nCount)
                                    {
                                        result = true;
                                        return result;
                                    }
                                    break;
                            }
                        }
                    }
                }
                if (UseItems[ItemLocation.Bujuk] != null && UseItems[ItemLocation.Bujuk].Index > 0)
                {
                    amuletStdItem = M2Share.WorldEngine.GetStdItem(UseItems[ItemLocation.Bujuk].Index);
                    if (amuletStdItem != null)
                    {
                        if (amuletStdItem.StdMode == 25)
                        {
                            switch (nType)
                            {
                                case 1: // 符
                                    if (amuletStdItem.Shape == 5 && Math.Round(Convert.ToDouble(UseItems[ItemLocation.Bujuk].Dura / 100)) >= nCount)
                                    {
                                        result = true;
                                        return result;
                                    }
                                    break;
                                case 2: // 毒
                                    if (amuletStdItem.Shape <= 2 && Math.Round(Convert.ToDouble(UseItems[ItemLocation.Bujuk].Dura / 100)) >= nCount)
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
                    for (int i = 0; i < ItemList.Count; i++)
                    {
                        // 人物包裹不为空
                        UserItem userItem = ItemList[i];
                        if (userItem != null)
                        {
                            amuletStdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                            if (amuletStdItem != null)
                            {
                                if (amuletStdItem.StdMode == 25)
                                {
                                    switch (nType)
                                    {
                                        case 1:
                                            if (amuletStdItem.Shape == 5 && HUtil32.Round(userItem.Dura / 100) >= nCount)
                                            {
                                                result = true;
                                                return result;
                                            }
                                            break;
                                        case 2:
                                            if (amuletStdItem.Shape <= 2 && HUtil32.Round(userItem.Dura / 100) >= nCount)
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
                M2Share.Logger.Error("RobotPlayObject.CheckHeroAmulet");
            }
            return result;
        }

        private static int GetDirBaseObjectsCount(int mBtDirection, int rang)
        {
            return 0;
        }
    }
}