using M2Server.Monster.Monsters;
using M2Server.Player;
using NLog;
using SystemModule;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Enums;
using GuildInfo = M2Server.Guild.GuildInfo;

namespace M2Server.Castle
{
    public class UserCastle : IUserCastle
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private int _power;
        /// <summary>
        /// 科技等级
        /// </summary>
        private int _techLevel;

        private readonly CastleConf castleConf;

        public UserCastle(string sCastleDir)
        {
            MasterGuild = null;
            HomeMap = SystemShare.Config.CastleHomeMap; // '3'
            HomeX = SystemShare.Config.CastleHomeX; // 644
            HomeY = SystemShare.Config.CastleHomeY; // 290
            sName = SystemShare.Config.CastleName; // '沙巴克'
            ConfigDir = sCastleDir;
            PalaceMap = "0150";
            SecretMap = "D701";
            CastleEnvir = null;
            DoorStatus = null;
            IsStartWar = false;
            UnderWar = false;
            ShowOverMsg = false;
            AttackWarList = new List<AttackerInfo>();
            AttackGuildList = new List<IGuild>();
            SaveTick = 0;
            WarRangeX = SystemShare.Config.CastleWarRangeX;
            WarRangeY = SystemShare.Config.CastleWarRangeY;
            EnvirList = new List<string>();
            Archers = new ArcherUnit[12];
            Guards = new ArcherUnit[12];
            string filePath = Path.Combine(M2Share.BasePath, SystemShare.Config.CastleDir, ConfigDir);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            castleConf = new CastleConf(Path.Combine(filePath, CastleConst.SabukWFileName));
        }

        public int TechLevel
        {
            get { return _techLevel; }
            set { SetTechLevel(value); }
        }

        public ArcherUnit[] Archers { get; set; }
        public IList<IGuild> AttackGuildList { get; set; }
        public IList<AttackerInfo> AttackWarList { get; set; }
        public bool ShowOverMsg { get; set; }
        public bool IsStartWar { get; set; }
        public bool UnderWar { get; set; }
        public ArcherUnit CenterWall { get; set; }
        public DateTime ChangeDate { get; set; }
        public DoorStatus DoorStatus { get; set; }
        public int SaveTick { get; set; }
        public int StartCastleWarTick { get; set; }
        public IList<string> EnvirList { get; set; }
        public ArcherUnit[] Guards { get; set; }
        public DateTime IncomeToday { get; set; }
        public ArcherUnit LeftWall { get; set; }
        public ArcherUnit MainDoor { get; set; }
        public IEnvirnoment CastleEnvir { get; set; }
        public IEnvirnoment PalaceEnvir { get; set; }
        public IEnvirnoment SecretEnvir { get; set; }
        public IGuild MasterGuild { get; set; }
        public int HomeX { get; set; }
        public int HomeY { get; set; }
        public int PalaceDoorX { get; set; }
        public int PalaceDoorY { get; set; }
        public int TodayIncome { get; set; }
        public int TotalGold { get; set; }
        public int WarRangeX { get; set; }
        public int WarRangeY { get; set; }
        public ArcherUnit RightWall { get; set; }
        public string ConfigDir { get; set; }
        public string HomeMap { get; set; }
        public string MapName { get; set; }
        public string sName { get; set; }
        public string OwnGuild { get; set; }
        public string PalaceMap { get; set; }
        public string SecretMap { get; set; }
        public DateTime WarDate { get; set; }

        public int Power
        {
            get { return _power; }
            set { SetPower(value); }
        }

        public void Initialize()
        {
            ArcherUnit ObjUnit;
            LoadConfig();
            LoadAttackSabukWall();
            if (SystemShare.MapMgr.GetMapOfServerIndex(MapName) == M2Share.ServerIndex)
            {
                PalaceEnvir = SystemShare.MapMgr.FindMap(PalaceMap);
                if (PalaceEnvir == null) _logger.Warn($"皇宫地图{PalaceMap}没找到!!!");
                SecretEnvir = SystemShare.MapMgr.FindMap(SecretMap);
                if (SecretEnvir == null) _logger.Warn($"密道地图{SecretMap}没找到!!!");
                CastleEnvir = SystemShare.MapMgr.FindMap(MapName);
                if (CastleEnvir != null)
                {
                    MainDoor.BaseObject = SystemShare.WorldEngine.RegenMonsterByName(MapName, MainDoor.nX, MainDoor.nY, MainDoor.sName);
                    if (MainDoor.BaseObject != null)
                    {
                        MainDoor.BaseObject.WAbil.HP = MainDoor.nHP;
                        MainDoor.BaseObject.Castle = this;
                        if (MainDoor.nStatus)
                        {
                            ((CastleDoor)MainDoor.BaseObject).Open();
                        }
                    }
                    else
                    {
                        _logger.Warn("[Error] UserCastle.Initialize MainDoor.UnitObj = nil");
                    }
                    LeftWall.BaseObject = SystemShare.WorldEngine.RegenMonsterByName(MapName, LeftWall.nX, LeftWall.nY, LeftWall.sName);
                    if (LeftWall.BaseObject != null)
                    {
                        LeftWall.BaseObject.WAbil.HP = LeftWall.nHP;
                        LeftWall.BaseObject.Castle = this;
                    }
                    else
                    {
                        _logger.Warn("[错误信息] 城堡初始化城门失败，检查怪物数据库里有没城门的设置: " + MainDoor.sName);
                    }
                    CenterWall.BaseObject = SystemShare.WorldEngine.RegenMonsterByName(MapName, CenterWall.nX, CenterWall.nY, CenterWall.sName);
                    if (CenterWall.BaseObject != null)
                    {
                        CenterWall.BaseObject.WAbil.HP = CenterWall.nHP;
                        CenterWall.BaseObject.Castle = this;
                    }
                    else
                    {
                        _logger.Warn("[错误信息] 城堡初始化左城墙失败，检查怪物数据库里有没左城墙的设置: " + LeftWall.sName);
                    }
                    RightWall.BaseObject = SystemShare.WorldEngine.RegenMonsterByName(MapName, RightWall.nX, RightWall.nY, RightWall.sName);
                    if (RightWall.BaseObject != null)
                    {
                        RightWall.BaseObject.WAbil.HP = RightWall.nHP;
                        RightWall.BaseObject.Castle = this;
                    }
                    else
                    {
                        _logger.Warn("[错误信息] 城堡初始化中城墙失败，检查怪物数据库里有没中城墙的设置: " + CenterWall.sName);
                    }
                    for (int i = 0; i < Archers.Length; i++)
                    {
                        ObjUnit = Archers[i];
                        if (ObjUnit.nHP <= 0) continue;
                        ObjUnit.BaseObject = SystemShare.WorldEngine.RegenMonsterByName(MapName, ObjUnit.nX, ObjUnit.nY, ObjUnit.sName);
                        if (ObjUnit.BaseObject != null)
                        {
                            ObjUnit.BaseObject.WAbil.HP = Archers[i].nHP;
                            ObjUnit.BaseObject.Castle = this;
                            ((GuardUnit)ObjUnit.BaseObject).GuardDirection = 3;
                        }
                        else
                        {
                            _logger.Warn("[错误信息] 城堡初始化弓箭手失败，检查怪物数据库里有没弓箭手的设置: " + ObjUnit.sName);
                        }
                    }
                    for (int i = 0; i < Guards.Length; i++)
                    {
                        ObjUnit = Guards[i];
                        if (ObjUnit.nHP <= 0) continue;
                        ObjUnit.BaseObject = SystemShare.WorldEngine.RegenMonsterByName(MapName, ObjUnit.nX, ObjUnit.nY, ObjUnit.sName);
                        if (ObjUnit.BaseObject != null)
                            ObjUnit.BaseObject.WAbil.HP = Guards[i].nHP;
                        else
                            _logger.Warn("[错误信息] 城堡初始化守卫失败(检查怪物数据库里有没守卫怪物)");
                    }
                    //for (int i = 0; i < CastleEnvir.DoorList.Count; i++)
                    //{
                    //    Door = CastleEnvir.DoorList[i];
                    //    if (Math.Abs(Door.nX - PalaceDoorX) <= 3 && Math.Abs(Door.nY - PalaceDoorY) <= 3)
                    //    {
                    //        DoorStatus = Door.Status;
                    //    }
                    //}
                }
                else
                {
                    _logger.Warn($"[错误信息] 城堡所在地图不存在(检查地图配置文件里是否有地图{MapName}的设置)");
                }
            }
        }

        private void LoadConfig()
        {
            castleConf.LoadConfig(this);
            MasterGuild = SystemShare.GuildMgr.FindGuild(OwnGuild);
        }

        private void SaveConfigFile()
        {
            castleConf.SaveConfig(this);
        }

        /// <summary>
        /// 读取沙巴克战役列表
        /// </summary>
        private void LoadAttackSabukWall()
        {
            string sabukwallPath = Path.Combine(M2Share.BasePath, SystemShare.Config.CastleDir, ConfigDir);
            string guildName = string.Empty;
            if (!Directory.Exists(sabukwallPath))
                Directory.CreateDirectory(sabukwallPath);
            string sFileName = Path.Combine(sabukwallPath, CastleConst.AttackSabukWallList);
            if (!File.Exists(sFileName)) return;
            using StringList loadList = new StringList();
            loadList.LoadFromFile(sFileName);
            for (int i = 0; i < AttackWarList.Count; i++)
            {
                AttackWarList[i] = default;
            }
            AttackWarList.Clear();
            for (int i = 0; i < loadList.Count; i++)
            {
                string sData = loadList[i];
                string s20 = HUtil32.GetValidStr3(sData, ref guildName, new[] { ' ', '\t' });
                IGuild guild = SystemShare.GuildMgr.FindGuild(guildName);
                if (guild == null) continue;
                AttackerInfo attackerInfo = new AttackerInfo();
                HUtil32.ArrestStringEx(s20, "\"", "\"", ref s20);
                if (DateTime.TryParse(s20, out DateTime time))
                {
                    attackerInfo.AttackDate = time;
                }
                else
                {
                    attackerInfo.AttackDate = DateTime.Now;
                }
                attackerInfo.sGuildName = guildName;
                attackerInfo.Guild = guild;
                AttackWarList.Add(attackerInfo);
            }
        }

        /// <summary>
        /// 保存沙巴克战役列表
        /// </summary>
        private void SaveAttackSabukWall()
        {
            string sabukwallPath = Path.Combine(M2Share.BasePath, SystemShare.Config.CastleDir, ConfigDir);
            if (!Directory.Exists(sabukwallPath))
                Directory.CreateDirectory(sabukwallPath);
            string sFileName = Path.Combine(sabukwallPath, CastleConst.AttackSabukWallList);
            using StringList loadLis = new StringList(AttackWarList.Count);
            for (int i = 0; i < AttackWarList.Count; i++)
            {
                AttackerInfo attackerInfo = AttackWarList[i];
                loadLis.Add(attackerInfo.sGuildName + "       \"" + attackerInfo.AttackDate + '\"');
            }
            loadLis.SaveToFile(sFileName);
        }

        public void Run()
        {
            const string sWarStartMsg = "[{0} 攻城战已经开始]";
            const string sWarStopTimeMsg = "[{0} 攻城战离结束还有{1}分钟]";
            const string sExceptionMsg = "[Exception] TUserCastle::Run";
            try
            {
                if (M2Share.ServerIndex != SystemShare.MapMgr.GetMapOfServerIndex(MapName)) return;
                int Year = DateTime.Now.Year;
                int Month = DateTime.Now.Month;
                int Day = DateTime.Now.Day;
                int wYear = IncomeToday.Year;
                int wMonth = IncomeToday.Month;
                int wDay = IncomeToday.Day;
                if (Year != wYear || Month != wMonth || Day != wDay)
                {
                    TodayIncome = 0;
                    IncomeToday = DateTime.Now;
                    IsStartWar = false;
                }
                if (!IsStartWar && !UnderWar)
                {
                    int hour = DateTime.Now.Hour;
                    if (hour == SystemShare.Config.StartCastlewarTime) // 20
                    {
                        IsStartWar = true;
                        AttackGuildList.Clear();
                        for (int i = AttackWarList.Count - 1; i >= 0; i--)
                        {
                            AttackerInfo attackerInfo = AttackWarList[i];
                            wYear = attackerInfo.AttackDate.Year;
                            wMonth = attackerInfo.AttackDate.Month;
                            wDay = attackerInfo.AttackDate.Day;
                            if (Year == wYear && Month == wMonth && Day == wDay)
                            {
                                UnderWar = true;
                                ShowOverMsg = false;
                                WarDate = DateTime.Now;
                                StartCastleWarTick = HUtil32.GetTickCount();
                                AttackGuildList.Add(attackerInfo.Guild);
                                AttackWarList.RemoveAt(i);
                            }
                        }
                        if (UnderWar)
                        {
                            AttackGuildList.Add(MasterGuild);
                            StartWallconquestWar();
                            SaveAttackSabukWall();
                            SystemShare.WorldEngine.SendServerGroupMsg(Messages.SS_212, M2Share.ServerIndex, "");
                            string s20 = string.Format(sWarStartMsg, sName);
                            SystemShare.WorldEngine.SendBroadCastMsgExt(s20, MsgType.System);
                            SystemShare.WorldEngine.SendServerGroupMsg(Messages.SS_204, M2Share.ServerIndex, s20);
                            _logger.Info(s20);
                            MainDoorControl(true);
                        }
                    }
                }
                for (int i = 0; i < Guards.Length; i++)
                {
                    if (Guards[i].BaseObject != null && Guards[i].BaseObject.Ghost)
                    {
                        Guards[i].BaseObject = null;
                    }
                }
                for (int i = 0; i < Archers.Length; i++)
                {
                    if (Archers[i].BaseObject != null && Archers[i].BaseObject.Ghost)
                    {
                        Archers[i].BaseObject = null;
                    }
                }
                if (UnderWar)
                {
                    if (LeftWall.BaseObject != null) LeftWall.BaseObject.StoneMode = false;
                    if (CenterWall.BaseObject != null) CenterWall.BaseObject.StoneMode = false;
                    if (RightWall.BaseObject != null) RightWall.BaseObject.StoneMode = false;
                    if (!ShowOverMsg)
                    {
                        if ((HUtil32.GetTickCount() - StartCastleWarTick) > (SystemShare.Config.CastleWarTime - SystemShare.Config.ShowCastleWarEndMsgTime)) // 3 * 60 * 60 * 1000 - 10 * 60 * 1000
                        {
                            ShowOverMsg = true;
                            string s20 = string.Format(sWarStopTimeMsg, sName, SystemShare.Config.ShowCastleWarEndMsgTime / (60 * 1000));
                            SystemShare.WorldEngine.SendBroadCastMsgExt(s20, MsgType.System);
                            SystemShare.WorldEngine.SendServerGroupMsg(Messages.SS_204, M2Share.ServerIndex, s20);
                            _logger.Warn(s20);
                        }
                    }
                    if ((HUtil32.GetTickCount() - StartCastleWarTick) > SystemShare.Config.CastleWarTime)
                    {
                        StopWallconquestWar();
                    }
                }
                else
                {
                    if (LeftWall.BaseObject != null) LeftWall.BaseObject.StoneMode = true;
                    if (CenterWall.BaseObject != null) CenterWall.BaseObject.StoneMode = true;
                    if (RightWall.BaseObject != null) RightWall.BaseObject.StoneMode = true;
                }
            }
            catch
            {
                _logger.Error(sExceptionMsg);
            }
        }

        public void Save()
        {
            SaveConfigFile();
            SaveAttackSabukWall();
        }

        public bool InCastleWarArea(IEnvirnoment envir, int nX, int nY)
        {
            if (envir == null)
            {
                return false;
            }
            if (envir == CastleEnvir && Math.Abs(HomeX - nX) < WarRangeX && Math.Abs(HomeY - nY) < WarRangeY) return true;
            if (envir == PalaceEnvir || envir == SecretEnvir) return true;
            for (int i = 0; i < EnvirList.Count; i++)// 增加取得城堡所有地图列表
            {
                if (string.Compare(EnvirList[i], envir.MapName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsMember(PlayObject member)
        {
            return IsMasterGuild(member.MyGuild);
        }

        // 检查是否为攻城方行会的联盟行会
        public bool IsAttackAllyGuild(GuildInfo Guild)
        {
            bool result = false;
            for (int i = 0; i < AttackGuildList.Count; i++)
            {
                IGuild AttackGuild = AttackGuildList[i];
                if (AttackGuild != MasterGuild && AttackGuild.IsAllyGuild(Guild))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        // 检查是否为攻城方行会
        public bool IsAttackGuild(GuildInfo Guild)
        {
            bool result = false;
            for (int i = 0; i < AttackGuildList.Count; i++)
            {
                IGuild AttackGuild = AttackGuildList[i];
                if (AttackGuild != MasterGuild && AttackGuild == Guild)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public bool CanGetCastle(GuildInfo guild)
        {
            if ((HUtil32.GetTickCount() - StartCastleWarTick) <= SystemShare.Config.GetCastleTime)
            {
                return false;
            }
            IList<IPlayerActor> playObjectList = new List<IPlayerActor>();
            SystemShare.WorldEngine.GetMapRageHuman(PalaceEnvir, 0, 0, 1000, ref playObjectList);
            bool result = true;
            for (int i = 0; i < playObjectList.Count; i++)
            {
                IPlayerActor playObject = (IPlayerActor)playObjectList[i];
                if (!playObject.Death && playObject.MyGuild != guild)
                {
                    result = false;
                    break;
                }
            }
            playObjectList.Clear();
            return result;
        }

        public void GetCastle(GuildInfo Guild)
        {
            const string sGetCastleMsg = "[{0} 已被 {1} 占领]";
            IGuild OldGuild = MasterGuild;
            MasterGuild = Guild;
            OwnGuild = Guild.GuildName;
            ChangeDate = DateTime.Now;
            SaveConfigFile();
            if (OldGuild != null)
            {
                OldGuild.RefMemberName();
            }
            if (MasterGuild != null)
            {
                MasterGuild.RefMemberName();//刷新旧的行会信息
            }
            string castleMessage = string.Format(sGetCastleMsg, sName, OwnGuild);
            SystemShare.WorldEngine.SendBroadCastMsgExt(castleMessage, MsgType.System);
            SystemShare.WorldEngine.SendServerGroupMsg(Messages.SS_204, M2Share.ServerIndex, castleMessage);
            _logger.Info(castleMessage);
        }

        /// <summary>
        /// 开始攻城战役
        /// </summary>
        public void StartWallconquestWar()
        {
            IList<IPlayerActor> playObjectList = new List<IPlayerActor>();
            SystemShare.WorldEngine.GetMapRageHuman(PalaceEnvir, HomeX, HomeY, 100, ref playObjectList);
            for (int i = 0; i < playObjectList.Count; i++)
            {
                playObjectList[i].RefShowName();
            }
        }

        /// <summary>
        /// 停止沙巴克攻城战役
        /// </summary>
        public void StopWallconquestWar()
        {
            const string sWallWarStop = "[{0} 攻城战已经结束]";
            UnderWar = false;
            AttackGuildList.Clear();
            IList<PlayObject> ListC = new List<PlayObject>();
            SystemShare.WorldEngine.GetMapOfRangeHumanCount(CastleEnvir, HomeX, HomeY, 100);
            for (int i = 0; i < ListC.Count; i++)
            {
                PlayObject PlayObject = ListC[i];
                PlayObject.ChangePkStatus(false);
                if (PlayObject.MyGuild != MasterGuild)
                {
                    PlayObject.MapRandomMove(PlayObject.HomeMap, 0);
                }
            }
            string stopMessage = string.Format(sWallWarStop, sName);
            SystemShare.WorldEngine.SendBroadCastMsgExt(stopMessage, MsgType.System);
            SystemShare.WorldEngine.SendServerGroupMsg(Messages.SS_204, M2Share.ServerIndex, stopMessage);
            _logger.Info(stopMessage);
        }

        public int WithDrawalGolds(IPlayerActor PlayObject, int nGold)
        {
            throw new NotImplementedException();
        }

        public int InPalaceGuildCount()
        {
            return AttackGuildList.Count;
        }

        public bool IsMember(IPlayerActor member)
        {
            return IsMasterGuild(member.MyGuild);
        }

        public bool IsDefenseAllyGuild(GuildInfo guild)
        {
            if (!UnderWar) return false; // 如果未开始攻城，则无效
            return MasterGuild != null && MasterGuild.IsAllyGuild(guild);
        }

        /// <summary>
        /// 是否为守城方行会
        /// </summary>
        /// <param name="guild"></param>
        /// <returns></returns>
        public bool IsDefenseGuild(GuildInfo guild)
        {
            if (!UnderWar) return false;// 如果未开始攻城，则无效
            return guild == MasterGuild;
        }

        public bool IsMasterGuild(IGuild guild)
        {
            return MasterGuild != null && MasterGuild == guild;
        }

        public void GetCastle(IGuild Guild)
        {
            throw new NotImplementedException();
        }

        public short GetHomeX()
        {
            return (short)(HomeX - 4 + M2Share.RandomNumber.Random(9));
        }

        public short GetHomeY()
        {
            return (short)(HomeY - 4 + M2Share.RandomNumber.Random(9));
        }

        public string GetMapName()
        {
            return MapName;
        }

        public bool CheckInPalace(int nX, int nY, PlayObject targetObject)
        {
            bool result = IsMasterGuild(targetObject.MyGuild);
            return result ? result : CheckInPalace(nX, nY);
        }

        public bool AddAttackerInfo(IGuild Guild)
        {
            throw new NotImplementedException();
        }

        public bool CanGetCastle(IGuild guild)
        {
            throw new NotImplementedException();
        }

        public bool CheckInPalace(int nX, int nY)
        {
            bool result = false;
            ArcherUnit ObjUnit = LeftWall;
            if (ObjUnit.BaseObject != null && ObjUnit.BaseObject.Death && ObjUnit.BaseObject.CurrX == nX &&
                ObjUnit.BaseObject.CurrY == nY) result = true;
            ObjUnit = CenterWall;
            if (ObjUnit.BaseObject != null && ObjUnit.BaseObject.Death && ObjUnit.BaseObject.CurrX == nX &&
                ObjUnit.BaseObject.CurrY == nY) result = true;
            ObjUnit = RightWall;
            if (ObjUnit.BaseObject != null && ObjUnit.BaseObject.Death && ObjUnit.BaseObject.CurrX == nX &&
                ObjUnit.BaseObject.CurrY == nY) result = true;
            return result;
        }

        public bool CheckInPalace(int nX, int nY, IPlayerActor targetObject)
        {
            throw new NotImplementedException();
        }

        public string GetWarDate()
        {
            const string WarDateMsg = "{0}年{1}月{2}日";
            string result = string.Empty;
            if (AttackWarList.Count <= 0) return result;
            AttackerInfo AttackerInfo = AttackWarList[0];
            int Year = AttackerInfo.AttackDate.Year;
            int Month = AttackerInfo.AttackDate.Month;
            int Day = AttackerInfo.AttackDate.Day;
            return string.Format(WarDateMsg, Year, Month, Day);
        }

        public string GetAttackWarList()
        {
            string result = string.Empty;
            short wYear = 0;
            short wMonth = 0;
            short wDay = 0;
            int n10 = 0;
            for (int i = 0; i < AttackWarList.Count; i++)
            {
                AttackerInfo AttackerInfo = AttackWarList[i];
                int Year = AttackerInfo.AttackDate.Year;
                int Month = AttackerInfo.AttackDate.Month;
                int Day = AttackerInfo.AttackDate.Day;
                if (Year != wYear || Month != wMonth || Day != wDay)
                {
                    wYear = (short)Year;
                    wMonth = (short)Month;
                    wDay = (short)Day;
                    if (!string.IsNullOrEmpty(result))
                    {
                        result = result + '\\';
                    }
                    result = result + wYear + '年' + wMonth + '月' + wDay + "日\\";
                    n10 = 0;
                }
                if (n10 > 40)
                {
                    result = result + '\\';
                    n10 = 0;
                }
                string s20 = '\"' + AttackerInfo.sGuildName + '\"';
                n10 += s20.Length;
                result = result + s20;
            }
            return result;
        }

        /// <summary>
        /// 增加每日收入
        /// </summary>
        public void IncRateGold(int nGold)
        {
            int nInGold = HUtil32.Round(nGold * (SystemShare.Config.CastleTaxRate / 100.0));
            if (TodayIncome + nInGold <= SystemShare.Config.CastleOneDayGold)
            {
                TodayIncome += nInGold;
            }
            else
            {
                if (TodayIncome >= SystemShare.Config.CastleOneDayGold)
                {
                    nInGold = 0;
                }
                else
                {
                    nInGold = SystemShare.Config.CastleOneDayGold - TodayIncome;
                    TodayIncome = SystemShare.Config.CastleOneDayGold;
                }
            }
            if (nInGold > 0)
            {
                if (TotalGold + nInGold < SystemShare.Config.CastleGoldMax)
                    TotalGold += nInGold;
                else
                    TotalGold = SystemShare.Config.CastleGoldMax;
            }
            if ((HUtil32.GetTickCount() - SaveTick) > 10 * 60 * 1000)
            {
                SaveTick = HUtil32.GetTickCount();
                if (M2Share.GameLogGold)
                {
                    //  M2Share.EventSource.AddEventLog(GameEventLogType.CastleTodayIncome, '0' + "\t" + '0' + "\t" + '0' + "\t" + "autosave" + "\t" + Grobal2.StringGoldName + "\t" + TotalGold + "\t" + '1' + "\t" + '0');
                }
            }
        }

        /// <summary>
        /// 提取每日收入
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="nGold"></param>
        /// <returns></returns>
        public int WithDrawalGolds(PlayObject PlayObject, int nGold)
        {
            int result = -1;
            if (nGold <= 0)
            {
                result = -4;
                return result;
            }
            if (MasterGuild == PlayObject.MyGuild && PlayObject.GuildRankNo == 1)
            {
                if (nGold <= TotalGold)
                {
                    if (PlayObject.Gold + nGold <= SystemShare.Config.HumanMaxGold)
                    {
                        TotalGold -= nGold;
                        PlayObject.IncGold(nGold);
                        if (M2Share.GameLogGold)
                        {
                            // M2Share.EventSource.AddEventLog(22, PlayObject.MapName + "\t" + PlayObject.CurrX + "\t" + PlayObject.CurrY + "\t" + PlayObject.ChrName + "\t" + Grobal2.StringGoldName + "\t" + nGold + "\t" + '1' + "\t" + '0');
                        }
                        PlayObject.GoldChanged();
                        result = 1;
                    }
                    else
                    {
                        result = -3;
                    }
                }
                else
                {
                    result = -2;
                }
            }
            return result;
        }

        public int ReceiptGolds(PlayObject PlayObject, int nGold)
        {
            int result = -1;
            if (nGold <= 0)
            {
                result = -4;
                return result;
            }
            if (MasterGuild == PlayObject.MyGuild && PlayObject.GuildRankNo == 1 && nGold > 0)
            {
                if (nGold <= PlayObject.Gold)
                {
                    if (TotalGold + nGold <= SystemShare.Config.CastleGoldMax)
                    {
                        PlayObject.Gold -= nGold;
                        TotalGold += nGold;
                        if (M2Share.GameLogGold)
                        {
                            // M2Share.EventSource.AddEventLog(GameEventLogType.CastleReceiptGolds, PlayObject.MapName + "\t" + PlayObject.CurrX + "\t" + PlayObject.CurrY + "\t" + PlayObject.ChrName + "\t" + Grobal2.StringGoldName + "\t" + nGold + "\t" + '1' + "\t" + '0');
                        }
                        PlayObject.GoldChanged();
                        result = 1;
                    }
                    else
                    {
                        result = -3;
                    }
                }
                else
                {
                    result = -2;
                }
            }
            return result;
        }

        /// <summary>
        /// 城门控制
        /// </summary>
        /// <param name="boClose"></param>
        public void MainDoorControl(bool boClose)
        {
            if (MainDoor.BaseObject != null && !MainDoor.BaseObject.Ghost)
            {
                if (boClose)
                {
                    if (((CastleDoor)MainDoor.BaseObject).IsOpened)
                    {
                        ((CastleDoor)MainDoor.BaseObject).Close();
                    }
                }
                else
                {
                    if (!((CastleDoor)MainDoor.BaseObject).IsOpened)
                    {
                        ((CastleDoor)MainDoor.BaseObject).Open();
                    }
                }
            }
        }

        public int ReceiptGolds(IPlayerActor PlayObject, int nGold)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 修复城门
        /// </summary>
        /// <returns></returns>
        public bool RepairDoor()
        {
            bool result = false;
            ArcherUnit castleDoor = MainDoor;
            if (castleDoor.BaseObject == null || UnderWar || castleDoor.BaseObject.WAbil.HP >= castleDoor.BaseObject.WAbil.MaxHP)
            {
                return false;
            }
            if (!castleDoor.BaseObject.Death)
            {
                if ((HUtil32.GetTickCount() - castleDoor.BaseObject.StruckTick) > 60 * 1000)
                {
                    castleDoor.BaseObject.WAbil.HP = castleDoor.BaseObject.WAbil.MaxHP;
                    ((CastleDoor)castleDoor.BaseObject).RefStatus();
                    result = true;
                }
            }
            else
            {
                if ((HUtil32.GetTickCount() - castleDoor.BaseObject.StruckTick) > 60 * 1000)
                {
                    castleDoor.BaseObject.WAbil.HP = castleDoor.BaseObject.WAbil.MaxHP;
                    castleDoor.BaseObject.Death = false;
                    ((CastleDoor)castleDoor.BaseObject).IsOpened = false;
                    ((CastleDoor)castleDoor.BaseObject).RefStatus();
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 修复城墙
        /// </summary>
        /// <param name="nWallIndex"></param>
        /// <returns></returns>
        public bool RepairWall(int nWallIndex)
        {
            bool result = false;
            IActor Wall = null;
            switch (nWallIndex)
            {
                case 1:
                    Wall = LeftWall.BaseObject;
                    break;
                case 2:
                    Wall = CenterWall.BaseObject;
                    break;
                case 3:
                    Wall = RightWall.BaseObject;
                    break;
            }
            if (Wall == null || UnderWar || Wall.WAbil.HP >= Wall.WAbil.MaxHP)
            {
                return false;
            }
            if (!Wall.Death)
            {
                if ((HUtil32.GetTickCount() - Wall.StruckTick) > 60 * 1000)
                {
                    Wall.WAbil.HP = Wall.WAbil.MaxHP;
                    ((WallStructure)Wall).RefStatus();
                    result = true;
                }
            }
            else
            {
                if ((HUtil32.GetTickCount() - Wall.StruckTick) > 60 * 1000)
                {
                    Wall.WAbil.HP = Wall.WAbil.MaxHP;
                    Wall.Death = false;
                    ((WallStructure)Wall).RefStatus();
                    result = true;
                }
            }
            return result;
        }

        public bool AddAttackerInfo(GuildInfo Guild)
        {
            if (InAttackerList(Guild)) return false;
            AttackerInfo AttackerInfo = new AttackerInfo();
            AttackerInfo.AttackDate = M2Share.AddDateTimeOfDay(DateTime.Now, SystemShare.Config.StartCastleWarDays);
            AttackerInfo.sGuildName = Guild.GuildName;
            AttackerInfo.Guild = Guild;
            AttackWarList.Add(AttackerInfo);
            SaveAttackSabukWall();
            SystemShare.WorldEngine.SendServerGroupMsg(Messages.SS_212, M2Share.ServerIndex, "");
            AttackerInfo.Dispose();
            return true;
        }

        private bool InAttackerList(GuildInfo Guild)
        {
            bool result = false;
            for (int i = 0; i < AttackWarList.Count; i++)
            {
                if (AttackWarList[i].Guild == Guild)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private void SetPower(int nPower)
        {
            _power = nPower;
        }

        private void SetTechLevel(int nLevel)
        {
            _techLevel = nLevel;
        }

        public bool IsAttackAllyGuild(SystemModule.GuildInfo Guild)
        {
            throw new NotImplementedException();
        }

        public bool IsAttackGuild(SystemModule.GuildInfo Guild)
        {
            throw new NotImplementedException();
        }

        public bool IsDefenseAllyGuild(SystemModule.GuildInfo guild)
        {
            throw new NotImplementedException();
        }

        public bool IsDefenseGuild(SystemModule.GuildInfo guild)
        {
            throw new NotImplementedException();
        }

        public bool IsMasterGuild(SystemModule.GuildInfo guild)
        {
            throw new NotImplementedException();
        }

        public bool IsAttackAllyGuild(IGuild Guild)
        {
            throw new NotImplementedException();
        }

        public bool IsAttackGuild(IGuild Guild)
        {
            throw new NotImplementedException();
        }

        public bool IsDefenseAllyGuild(IGuild guild)
        {
            throw new NotImplementedException();
        }

        public bool IsDefenseGuild(IGuild guild)
        {
            throw new NotImplementedException();
        }
    }
}