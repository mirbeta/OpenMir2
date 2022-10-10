using GameSvr.Actor;
using GameSvr.Command;
using GameSvr.Maps;
using NLog;
using SystemModule.Common;

namespace GameSvr.Castle
{
    public class CastleManager
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IList<TUserCastle> _castleList;

        public CastleManager()
        {
            _castleList = new List<TUserCastle>();
        }

        public TUserCastle Find(string sCastleName)
        {
            for (var i = 0; i < _castleList.Count; i++)
            {
                if (string.Compare(_castleList[i].sName, sCastleName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return _castleList[i];
                }
            }
            return null;
        }

        // 取得角色所在座标的城堡
        public TUserCastle InCastleWarArea(BaseObject BaseObject)
        {
            for (var i = 0; i < _castleList.Count; i++)
            {
                if (_castleList[i].InCastleWarArea(BaseObject.Envir, BaseObject.CurrX, BaseObject.CurrY))
                {
                    return _castleList[i];
                }
            }
            return null;
        }

        public TUserCastle InCastleWarArea(Envirnoment Envir, int nX, int nY)
        {
            for (var i = 0; i < _castleList.Count; i++)
            {
                if (_castleList[i].InCastleWarArea(Envir, nX, nY))
                {
                    return _castleList[i];
                }
            }
            return null;
        }

        public void Initialize()
        {
            TUserCastle castle;
            if (_castleList.Count <= 0)
            {
                castle = new TUserCastle(M2Share.Config.CastleDir);
                castle.Initialize();
                castle.ConfigDir = "0";
                castle.EnvirList.Add("0151");
                castle.EnvirList.Add("0152");
                castle.EnvirList.Add("0153");
                castle.EnvirList.Add("0154");
                castle.EnvirList.Add("0155");
                castle.EnvirList.Add("0156");
                for (var i = 0; i < castle.EnvirList.Count; i++)
                {
                    castle.EnvirList[i] = M2Share.MapMgr.FindMap(castle.EnvirList[i]).MapName;
                }
                _castleList.Add(castle);
                Save();
                return;
            }
            for (var i = 0; i < _castleList.Count; i++)
            {
                castle = _castleList[i];
                castle.Initialize();
            }
            _logger.Debug("城堡城初始完成...");
        }

        // 城堡皇宫所在地图
        public TUserCastle IsCastlePalaceEnvir(Envirnoment Envir)
        {
            for (var i = 0; i < _castleList.Count; i++)
            {
                if (_castleList[i].PalaceEnvir == Envir)
                {
                    return _castleList[i];
                }
            }
            return null;
        }

        // 城堡所在地图
        public TUserCastle IsCastleEnvir(Envirnoment Envir)
        {
            for (var i = 0; i < _castleList.Count; i++)
            {
                if (_castleList[i].CastleEnvir == Envir)
                {
                    return _castleList[i];
                }
            }
            return null;
        }

        public TUserCastle IsCastleMember(BaseObject BaseObject)
        {
            for (var i = 0; i < _castleList.Count; i++)
            {
                if (_castleList[i].IsMember(BaseObject))
                {
                    return _castleList[i];
                }
            }
            return null;
        }

        public void Run()
        {
            for (var i = 0; i < _castleList.Count; i++)
            {
                _castleList[i].Run();
            }
        }

        public void GetCastleGoldInfo(IList<string> List)
        {
            for (var i = 0; i < _castleList.Count; i++)
            {
                var castle = _castleList[i];
                List.Add(string.Format(CommandHelp.GameCommandSbkGoldShowMsg, castle.sName, castle.TotalGold, castle.TodayIncome));
            }
        }

        public void Save()
        {
            SaveCastleList();
            for (var i = 0; i < _castleList.Count; i++)
            {
                var castle = _castleList[i];
                castle.Save();
            }
        }

        public void LoadCastleList()
        {
            var castleFile = Path.Combine(M2Share.BasePath, M2Share.Config.CastleFile);
            if (File.Exists(castleFile))
            {
                using (var loadList = new StringList())
                {
                    loadList.LoadFromFile(castleFile);
                    for (var i = 0; i < loadList.Count; i++)
                    {
                        var sCastleDir = loadList[i].Trim();
                        if (!string.IsNullOrEmpty(sCastleDir))
                        {
                            var castle = new TUserCastle(sCastleDir);
                            _castleList.Add(castle);
                        }
                    }
                }
                _logger.Info($"已读取 [{_castleList.Count}] 个城堡信息...");
            }
            else
            {
                _logger.Error("城堡列表文件未找到!!!");
            }
        }

        private void SaveCastleList()
        {
            var castleDirPath = Path.Combine(M2Share.BasePath, M2Share.Config.CastleDir);
            if (!Directory.Exists(castleDirPath))
            {
                Directory.CreateDirectory(castleDirPath);
            }
            var loadList = new StringList();
            for (var i = 0; i < _castleList.Count; i++)
            {
                loadList.Add(i.ToString());
            }
            var savePath = Path.Combine(M2Share.BasePath, M2Share.Config.CastleFile);
            loadList.SaveToFile(savePath);
        }

        public TUserCastle GetCastle(int nIndex)
        {
            if (nIndex >= 0 && nIndex < _castleList.Count)
            {
                return _castleList[nIndex];
            }
            return null;
        }

        public void GetCastleNameList(IList<string> List)
        {
            for (var i = 0; i < _castleList.Count; i++)
            {
                List.Add(_castleList[i].sName);
            }
        }

        public void IncRateGold(int nGold)
        {
            for (var i = 0; i < _castleList.Count; i++)
            {
                _castleList[i].IncRateGold(nGold);
            }
        }
    }
}