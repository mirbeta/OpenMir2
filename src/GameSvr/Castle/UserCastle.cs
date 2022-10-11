using GameSvr.Actor;
using GameSvr.Guild;
using GameSvr.Maps;
using GameSvr.Monster.Monsters;
using GameSvr.Player;
using NLog;
using SystemModule;
using SystemModule.Common;
using SystemModule.Data;

namespace GameSvr.Castle
{
    public class TUserCastle
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 守卫列表
        /// </summary>
        public readonly ArcherUnit[] Archers = new ArcherUnit[CastleConst.MaxCastleArcher];
        /// <summary>
        /// 攻城行会列表
        /// </summary>
        private readonly IList<GuildInfo> AttackGuildList;
        /// <summary>
        /// 攻城列表
        /// </summary>
        private readonly IList<AttackerInfo> AttackWarList;
        /// <summary>
        /// 是否显示攻城战役结束消息
        /// </summary>
        public bool ShowOverMsg;
        /// <summary>
        /// 是否开始攻城
        /// </summary>
        private bool IsStartWar;
        public bool UnderWar;
        public ArcherUnit CenterWall;
        public DateTime ChangeDate;
        /// <summary>
        /// 城门状态
        /// </summary>
        public DoorStatus DoorStatus;
        /// <summary>
        /// 是否已显示攻城结束信息
        /// </summary>
        private int SaveTick;
        public int StartCastleWarTick;
        public IList<string> EnvirList;
        public ArcherUnit[] Guards = new ArcherUnit[4];
        public DateTime IncomeToday;
        public ArcherUnit LeftWall;
        public ArcherUnit MainDoor;
        /// <summary>
        /// 城堡地图
        /// </summary>
        public Envirnoment CastleEnvir;
        /// <summary>
        /// 皇宫地图
        /// </summary>
        public Envirnoment PalaceEnvir;
        /// <summary>
        /// 密道地图
        /// </summary>
        public Envirnoment SecretEnvir;
        /// <summary>
        /// 所属行会名称
        /// </summary>
        public GuildInfo MasterGuild;
        /// <summary>
        /// 行会回城点X
        /// </summary>
        public int HomeX;
        /// <summary>
        /// 行会回城点Y
        /// </summary>
        public int HomeY;
        public int PalaceDoorX;
        public int PalaceDoorY;
        private int _power;
        /// <summary>
        /// 科技等级
        /// </summary>
        private int _techLevel;
        /// <summary>
        /// 今日收入
        /// </summary>
        public int TodayIncome;
        /// <summary>
        /// 收入多少金币
        /// </summary>
        public int TotalGold;
        public int WarRangeX;
        public int WarRangeY;
        /// <summary>
        /// 皇宫右城墙
        /// </summary>
        public ArcherUnit RightWall;
        /// <summary>
        /// 皇宫门状态
        /// </summary>
        public string ConfigDir = string.Empty;
        /// <summary>
        /// 行会回城点地图
        /// </summary>
        public string HomeMap = string.Empty;
        /// <summary>
        /// 城堡所在地图名
        /// </summary>
        public string MapName = string.Empty;
        /// <summary>
        /// 城堡名称
        /// </summary>
        public string sName = string.Empty;
        /// <summary>
        /// 所属行会
        /// </summary>
        public string OwnGuild = string.Empty;
        /// <summary>
        /// 皇宫地图名称
        /// </summary>
        public string PalaceMap = string.Empty;
        /// <summary>
        /// 密道地图名称
        /// </summary>
        public string SecretMap = string.Empty;
        public DateTime m_WarDate;
        private readonly CastleConfMgr castleConf;

        public TUserCastle(string sCastleDir)
        {
            MasterGuild = null;
            HomeMap = M2Share.Config.CastleHomeMap; // '3'
            HomeX = M2Share.Config.CastleHomeX; // 644
            HomeY = M2Share.Config.CastleHomeY; // 290
            sName = M2Share.Config.CastleName; // '沙巴克'
            ConfigDir = sCastleDir;
            PalaceMap = "0150";
            SecretMap = "D701";
            CastleEnvir = null;
            DoorStatus = null;
            IsStartWar = false;
            UnderWar = false;
            ShowOverMsg = false;
            AttackWarList = new List<AttackerInfo>();
            AttackGuildList = new List<GuildInfo>();
            SaveTick = 0;
            WarRangeX = M2Share.Config.CastleWarRangeX;
            WarRangeY = M2Share.Config.CastleWarRangeY;
            EnvirList = new List<string>();
            var filePath = Path.Combine(M2Share.BasePath, M2Share.Config.CastleDir, ConfigDir);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            castleConf = new CastleConfMgr(Path.Combine(filePath, CastleConst.SabukWFileName));
        }

        public int TechLevel
        {
            get { return _techLevel; }
            set { SetTechLevel(value); }
        }

        public int Power
        {
            get { return _power; }
            set { SetPower(value); }
        }

        public void Initialize()
        {
            ArcherUnit ObjUnit;
            DoorInfo Door;
            LoadConfig();
            LoadAttackSabukWall();
            if (M2Share.MapMgr.GetMapOfServerIndex(MapName) == M2Share.ServerIndex)
            {
                PalaceEnvir = M2Share.MapMgr.FindMap(PalaceMap);
                if (PalaceEnvir == null) _logger.Warn($"皇宫地图{PalaceMap}没找到!!!");
                SecretEnvir = M2Share.MapMgr.FindMap(SecretMap);
                if (SecretEnvir == null) _logger.Warn($"密道地图{SecretMap}没找到!!!");
                CastleEnvir = M2Share.MapMgr.FindMap(MapName);
                if (CastleEnvir != null)
                {
                    MainDoor.BaseObject = M2Share.WorldEngine.RegenMonsterByName(MapName, MainDoor.nX, MainDoor.nY, MainDoor.sName);
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
                    LeftWall.BaseObject = M2Share.WorldEngine.RegenMonsterByName(MapName, LeftWall.nX, LeftWall.nY, LeftWall.sName);
                    if (LeftWall.BaseObject != null)
                    {
                        LeftWall.BaseObject.WAbil.HP = LeftWall.nHP;
                        LeftWall.BaseObject.Castle = this;
                    }
                    else
                    {
                        _logger.Warn("[错误信息] 城堡初始化城门失败，检查怪物数据库里有没城门的设置: " + MainDoor.sName);
                    }
                    CenterWall.BaseObject = M2Share.WorldEngine.RegenMonsterByName(MapName, CenterWall.nX, CenterWall.nY, CenterWall.sName);
                    if (CenterWall.BaseObject != null)
                    {
                        CenterWall.BaseObject.WAbil.HP = CenterWall.nHP;
                        CenterWall.BaseObject.Castle = this;
                    }
                    else
                    {
                        _logger.Warn("[错误信息] 城堡初始化左城墙失败，检查怪物数据库里有没左城墙的设置: " + LeftWall.sName);
                    }
                    RightWall.BaseObject = M2Share.WorldEngine.RegenMonsterByName(MapName, RightWall.nX, RightWall.nY, RightWall.sName);
                    if (RightWall.BaseObject != null)
                    {
                        RightWall.BaseObject.WAbil.HP = RightWall.nHP;
                        RightWall.BaseObject.Castle = this;
                    }
                    else
                    {
                        _logger.Warn("[错误信息] 城堡初始化中城墙失败，检查怪物数据库里有没中城墙的设置: " + CenterWall.sName);
                    }
                    for (var i = 0; i < Archers.Length; i++)
                    {
                        ObjUnit = Archers[i];
                        if (ObjUnit.nHP <= 0) continue;
                        ObjUnit.BaseObject = M2Share.WorldEngine.RegenMonsterByName(MapName, ObjUnit.nX, ObjUnit.nY, ObjUnit.sName);
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
                    for (var i = 0; i < Guards.Length; i++)
                    {
                        ObjUnit = Guards[i];
                        if (ObjUnit.nHP <= 0) continue;
                        ObjUnit.BaseObject = M2Share.WorldEngine.RegenMonsterByName(MapName, ObjUnit.nX, ObjUnit.nY, ObjUnit.sName);
                        if (ObjUnit.BaseObject != null)
                            ObjUnit.BaseObject.WAbil.HP = Guards[i].nHP;
                        else
                            _logger.Warn("[错误信息] 城堡初始化守卫失败(检查怪物数据库里有没守卫怪物)");
                    }
                    for (var i = 0; i < CastleEnvir.DoorList.Count; i++)
                    {
                        Door = CastleEnvir.DoorList[i];
                        if (Math.Abs(Door.nX - PalaceDoorX) <= 3 && Math.Abs(Door.nY - PalaceDoorY) <= 3)
                        {
                            DoorStatus = Door.Status;
                        }
                    }
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
            MasterGuild = M2Share.GuildMgr.FindGuild(OwnGuild);
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
            var sabukwallPath = Path.Combine(M2Share.BasePath, M2Share.Config.CastleDir, ConfigDir);
            var guildName = string.Empty;
            if (!Directory.Exists(sabukwallPath))
                Directory.CreateDirectory(sabukwallPath);
            var sFileName = Path.Combine(sabukwallPath, CastleConst.AttackSabukWallList);
            if (!File.Exists(sFileName)) return;
            var loadList = new StringList();
            loadList.LoadFromFile(sFileName);
            for (var i = 0; i < AttackWarList.Count; i++)
            {
                AttackWarList[i] = null;
            }
            AttackWarList.Clear();
            for (var i = 0; i < loadList.Count; i++)
            {
                var sData = loadList[i];
                var s20 = HUtil32.GetValidStr3(sData, ref guildName, new[] { " ", "\t" });
                var guild = M2Share.GuildMgr.FindGuild(guildName);
                if (guild == null) continue;
                var attackerInfo = new AttackerInfo();
                HUtil32.ArrestStringEx(s20, "\"", "\"", ref s20);
                if (DateTime.TryParse(s20, out var time))
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
            var sabukwallPath = Path.Combine(M2Share.BasePath, M2Share.Config.CastleDir, ConfigDir);
            if (!Directory.Exists(sabukwallPath))
                Directory.CreateDirectory(sabukwallPath);
            var sFileName = Path.Combine(sabukwallPath, CastleConst.AttackSabukWallList);
            using var loadLis = new StringList();
            for (var i = 0; i < AttackWarList.Count; i++)
            {
                var attackerInfo = AttackWarList[i];
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
                if (M2Share.ServerIndex != M2Share.MapMgr.GetMapOfServerIndex(MapName)) return;
                var Year = DateTime.Now.Year;
                var Month = DateTime.Now.Month;
                var Day = DateTime.Now.Day;
                var wYear = IncomeToday.Year;
                var wMonth = IncomeToday.Month;
                var wDay = IncomeToday.Day;
                if (Year != wYear || Month != wMonth || Day != wDay)
                {
                    TodayIncome = 0;
                    IncomeToday = DateTime.Now;
                    IsStartWar = false;
                }
                if (!IsStartWar && !UnderWar)
                {
                    var hour = DateTime.Now.Hour;
                    if (hour == M2Share.Config.StartCastlewarTime) // 20
                    {
                        IsStartWar = true;
                        AttackGuildList.Clear();
                        for (var i = AttackWarList.Count - 1; i >= 0; i--)
                        {
                            var attackerInfo = AttackWarList[i];
                            wYear = attackerInfo.AttackDate.Year;
                            wMonth = attackerInfo.AttackDate.Month;
                            wDay = attackerInfo.AttackDate.Day;
                            if (Year == wYear && Month == wMonth && Day == wDay)
                            {
                                UnderWar = true;
                                ShowOverMsg = false;
                                m_WarDate = DateTime.Now;
                                StartCastleWarTick = HUtil32.GetTickCount();
                                AttackGuildList.Add(attackerInfo.Guild);
                                attackerInfo = null;
                                AttackWarList.RemoveAt(i);
                            }
                        }
                        if (UnderWar)
                        {
                            AttackGuildList.Add(MasterGuild);
                            StartWallconquestWar();
                            SaveAttackSabukWall();
                            M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_212, M2Share.ServerIndex, "");
                            var s20 = string.Format(sWarStartMsg, sName);
                            M2Share.WorldEngine.SendBroadCastMsgExt(s20, MsgType.System);
                            M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_204, M2Share.ServerIndex, s20);
                            _logger.Info(s20);
                            MainDoorControl(true);
                        }
                    }
                }
                for (var i = 0; i < Guards.Length; i++)
                {
                    if (Guards[i].BaseObject != null && Guards[i].BaseObject.Ghost)
                    {
                        Guards[i].BaseObject = null;
                    }
                }
                for (var i = 0; i < Archers.Length; i++)
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
                        if ((HUtil32.GetTickCount() - StartCastleWarTick) > (M2Share.Config.CastleWarTime - M2Share.Config.ShowCastleWarEndMsgTime)) // 3 * 60 * 60 * 1000 - 10 * 60 * 1000
                        {
                            ShowOverMsg = true;
                            var s20 = string.Format(sWarStopTimeMsg, sName, M2Share.Config.ShowCastleWarEndMsgTime / (60 * 1000));
                            M2Share.WorldEngine.SendBroadCastMsgExt(s20, MsgType.System);
                            M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_204, M2Share.ServerIndex, s20);
                            _logger.Warn(s20);
                        }
                    }
                    if ((HUtil32.GetTickCount() - StartCastleWarTick) > M2Share.Config.CastleWarTime)
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

        public bool InCastleWarArea(Envirnoment envir, int nX, int nY)
        {
            if (envir == null)
            {
                return false;
            }
            if (envir == CastleEnvir && Math.Abs(HomeX - nX) < WarRangeX && Math.Abs(HomeY - nY) < WarRangeY) return true;
            if (envir == PalaceEnvir || envir == SecretEnvir) return true;
            for (var i = 0; i < EnvirList.Count; i++)// 增加取得城堡所有地图列表
            {
                if (string.Compare(EnvirList[i], envir.MapName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsMember(BaseObject cert)
        {
            return IsMasterGuild(cert.MyGuild);
        }

        // 检查是否为攻城方行会的联盟行会
        public bool IsAttackAllyGuild(GuildInfo Guild)
        {
            var result = false;
            for (var i = 0; i < AttackGuildList.Count; i++)
            {
                GuildInfo AttackGuild = AttackGuildList[i];
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
            var result = false;
            for (var i = 0; i < AttackGuildList.Count; i++)
            {
                GuildInfo AttackGuild = AttackGuildList[i];
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
            if ((HUtil32.GetTickCount() - StartCastleWarTick) <= M2Share.Config.GetCastleTime)
            {
                return false;
            }
            var playObjectList = new List<BaseObject>();
            M2Share.WorldEngine.GetMapRageHuman(PalaceEnvir, 0, 0, 1000, playObjectList);
            var result = true;
            for (var i = 0; i < playObjectList.Count; i++)
            {
                var playObject = (PlayObject)playObjectList[i];
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
            OwnGuild = Guild.sGuildName;
            ChangeDate = DateTime.Now;
            SaveConfigFile();
            if (MasterGuild != null)
            {
                MasterGuild.RefMemberName();//刷新旧的行会信息
            }
            Guild.RefMemberName();
            var castleMessage = string.Format(sGetCastleMsg, sName, OwnGuild);
            M2Share.WorldEngine.SendBroadCastMsgExt(castleMessage, MsgType.System);
            M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_204, M2Share.ServerIndex, castleMessage);
            _logger.Info(castleMessage);
        }

        public void StartWallconquestWar()
        {
            PlayObject PlayObject;
            var ListC = new List<BaseObject>();
            M2Share.WorldEngine.GetMapRageHuman(PalaceEnvir, HomeX, HomeY, 100, ListC);
            for (var i = 0; i < ListC.Count; i++)
            {
                PlayObject = (PlayObject)ListC[i];
                PlayObject.RefShowName();
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
            M2Share.WorldEngine.GetMapOfRangeHumanCount(CastleEnvir, HomeX, HomeY, 100);
            for (var i = 0; i < ListC.Count; i++)
            {
                var PlayObject = ListC[i];
                PlayObject.ChangePkStatus(false);
                if (PlayObject.MyGuild != MasterGuild)
                {
                    PlayObject.MapRandomMove(PlayObject.HomeMap, 0);
                }
            }
            var stopMessage = string.Format(sWallWarStop, sName);
            M2Share.WorldEngine.SendBroadCastMsgExt(stopMessage, MsgType.System);
            M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_204, M2Share.ServerIndex, stopMessage);
            _logger.Info(stopMessage);
        }

        public int InPalaceGuildCount()
        {
            return AttackGuildList.Count;
        }

        public bool IsDefenseAllyGuild(GuildInfo guild)
        {
            if (!UnderWar) return false; // 如果未开始攻城，则无效
            return MasterGuild != null && MasterGuild.IsAllyGuild(guild);
        }

        // 检查是否为守城方行会
        public bool IsDefenseGuild(GuildInfo guild)
        {
            if (!UnderWar) return false;// 如果未开始攻城，则无效
            return guild == MasterGuild;
        }

        public bool IsMasterGuild(GuildInfo guild)
        {
            return MasterGuild != null && MasterGuild == guild;
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

        public bool CheckInPalace(int nX, int nY, BaseObject cert)
        {
            var result = IsMasterGuild(cert.MyGuild);
            if (result) return result;
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

        public string GetWarDate()
        {
            const string sMsg = "{0}年{1}月{2}日";
            var result = string.Empty;
            if (AttackWarList.Count <= 0) return result;
            var AttackerInfo = AttackWarList[0];
            var Year = AttackerInfo.AttackDate.Year;
            var Month = AttackerInfo.AttackDate.Month;
            var Day = AttackerInfo.AttackDate.Day;
            return string.Format(sMsg, Year, Month, Day);
        }

        public string GetAttackWarList()
        {
            AttackerInfo AttackerInfo;
            var result = string.Empty;
            short wYear = 0;
            short wMonth = 0;
            short wDay = 0;
            var n10 = 0;
            for (var i = 0; i < AttackWarList.Count; i++)
            {
                AttackerInfo = AttackWarList[i];
                var Year = AttackerInfo.AttackDate.Year;
                var Month = AttackerInfo.AttackDate.Month;
                var Day = AttackerInfo.AttackDate.Day;
                if (Year != wYear || Month != wMonth || Day != wDay)
                {
                    wYear = (short)Year;
                    wMonth = (short)Month;
                    wDay = (short)Day;
                    if (result != "") result = result + '\\';
                    result = result + wYear + '年' + wMonth + '月' + wDay + "日\\";
                    n10 = 0;
                }
                if (n10 > 40)
                {
                    result = result + '\\';
                    n10 = 0;
                }
                var s20 = '\"' + AttackerInfo.sGuildName + '\"';
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
            var nInGold = HUtil32.Round(nGold * (M2Share.Config.CastleTaxRate / 100));
            if (TodayIncome + nInGold <= M2Share.Config.CastleOneDayGold)
            {
                TodayIncome += nInGold;
            }
            else
            {
                if (TodayIncome >= M2Share.Config.CastleOneDayGold)
                {
                    nInGold = 0;
                }
                else
                {
                    nInGold = M2Share.Config.CastleOneDayGold - TodayIncome;
                    TodayIncome = M2Share.Config.CastleOneDayGold;
                }
            }
            if (nInGold > 0)
            {
                if (TotalGold + nInGold < M2Share.Config.CastleGoldMax)
                    TotalGold += nInGold;
                else
                    TotalGold = M2Share.Config.CastleGoldMax;
            }
            if ((HUtil32.GetTickCount() - SaveTick) > 10 * 60 * 1000)
            {
                SaveTick = HUtil32.GetTickCount();
                if (M2Share.GameLogGold)
                {
                    M2Share.EventSource.AddEventLog(GameEventLogType.CastleTodayIncome, '0' + "\t" + '0' + "\t" + '0' + "\t" + "autosave" + "\t" + Grobal2.sSTRING_GOLDNAME + "\t" + TotalGold + "\t" + '1' + "\t" + '0');
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
            var result = -1;
            if (nGold <= 0)
            {
                result = -4;
                return result;
            }
            if (MasterGuild == PlayObject.MyGuild && PlayObject.GuildRankNo == 1)
            {
                if (nGold <= TotalGold)
                {
                    if (PlayObject.Gold + nGold <= M2Share.Config.HumanMaxGold)
                    {
                        TotalGold -= nGold;
                        PlayObject.IncGold(nGold);
                        if (M2Share.GameLogGold)
                        {
                            M2Share.EventSource.AddEventLog(22, PlayObject.MapName + "\t" + PlayObject.CurrX + "\t" + PlayObject.CurrY + "\t" + PlayObject.ChrName + "\t" + Grobal2.sSTRING_GOLDNAME + "\t" + nGold + "\t" + '1' + "\t" + '0');
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
            var result = -1;
            if (nGold <= 0)
            {
                result = -4;
                return result;
            }
            if (MasterGuild == PlayObject.MyGuild && PlayObject.GuildRankNo == 1 && nGold > 0)
            {
                if (nGold <= PlayObject.Gold)
                {
                    if (TotalGold + nGold <= M2Share.Config.CastleGoldMax)
                    {
                        PlayObject.Gold -= nGold;
                        TotalGold += nGold;
                        if (M2Share.GameLogGold)
                        {
                            M2Share.EventSource.AddEventLog(GameEventLogType.CastleReceiptGolds, PlayObject.MapName + "\t" + PlayObject.CurrX + "\t" + PlayObject.CurrY + "\t" + PlayObject.ChrName + "\t" + Grobal2.sSTRING_GOLDNAME + "\t" + nGold + "\t" + '1' + "\t" + '0');
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

        /// <summary>
        /// 修复城门
        /// </summary>
        /// <returns></returns>
        public bool RepairDoor()
        {
            var result = false;
            var castleDoor = MainDoor;
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
            var result = false;
            BaseObject Wall = null;
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
            var AttackerInfo = new AttackerInfo();
            AttackerInfo.AttackDate = M2Share.AddDateTimeOfDay(DateTime.Now, M2Share.Config.StartCastleWarDays);
            AttackerInfo.sGuildName = Guild.sGuildName;
            AttackerInfo.Guild = Guild;
            AttackWarList.Add(AttackerInfo);
            SaveAttackSabukWall();
            M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_212, M2Share.ServerIndex, "");
            return true;
        }

        private bool InAttackerList(GuildInfo Guild)
        {
            var result = false;
            for (var i = 0; i < AttackWarList.Count; i++)
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
    }
}