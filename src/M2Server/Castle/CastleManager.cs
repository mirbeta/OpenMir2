using M2Server.Player;
using NLog;
using SystemModule;
using SystemModule.Common;

namespace M2Server.Castle
{
    public class CastleManager
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public readonly IList<UserCastle> CastleList;

        public CastleManager()
        {
            CastleList = new List<UserCastle>();
        }

        public UserCastle Find(string sCastleName)
        {
            for (int i = 0; i < CastleList.Count; i++)
            {
                if (string.Compare(CastleList[i].sName, sCastleName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return CastleList[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 是否沙巴克攻城战役区域
        /// </summary>
        /// <param name="BaseObject"></param>
        /// <returns></returns>
        public UserCastle InCastleWarArea(IActor BaseObject)
        {
            for (int i = 0; i < CastleList.Count; i++)
            {
                if (CastleList[i].InCastleWarArea(BaseObject.Envir, BaseObject.CurrX, BaseObject.CurrY))
                {
                    return CastleList[i];
                }
            }
            return null;
        }

        public UserCastle InCastleWarArea(IEnvirnoment Envir, int nX, int nY)
        {
            for (int i = 0; i < CastleList.Count; i++)
            {
                if (CastleList[i].InCastleWarArea(Envir, nX, nY))
                {
                    return CastleList[i];
                }
            }
            return null;
        }

        public void Initialize()
        {
            UserCastle castle;
            if (CastleList.Count <= 0)
            {
                castle = new UserCastle(ModuleShare.Config.CastleDir);
                castle.Initialize();
                castle.ConfigDir = "0";
                castle.EnvirList.Add("0151");
                castle.EnvirList.Add("0152");
                castle.EnvirList.Add("0153");
                castle.EnvirList.Add("0154");
                castle.EnvirList.Add("0155");
                castle.EnvirList.Add("0156");
                CastleList.Add(castle);
                Save();
                return;
            }
            for (int i = 0; i < CastleList.Count; i++)
            {
                castle = CastleList[i];
                castle.Initialize();
            }
            _logger.Debug("城堡城初始完成...");
        }

        // 城堡皇宫所在地图
        public UserCastle IsCastlePalaceEnvir(IEnvirnoment Envir)
        {
            for (int i = 0; i < CastleList.Count; i++)
            {
                if (CastleList[i].PalaceEnvir == Envir)
                {
                    return CastleList[i];
                }
            }
            return null;
        }

        // 城堡所在地图
        public UserCastle IsCastleEnvir(IEnvirnoment envir)
        {
            for (int i = 0; i < CastleList.Count; i++)
            {
                if (CastleList[i].CastleEnvir == envir)
                {
                    return CastleList[i];
                }
            }
            return null;
        }

        public UserCastle IsCastleMember(PlayObject playObject)
        {
            for (int i = 0; i < CastleList.Count; i++)
            {
                if (CastleList[i].IsMember(playObject))
                {
                    return CastleList[i];
                }
            }
            return null;
        }

        public void Run()
        {
            for (int i = 0; i < CastleList.Count; i++)
            {
                CastleList[i].Run();
            }
        }

        public void GetCastleGoldInfo(IList<string> List)
        {
            for (int i = 0; i < CastleList.Count; i++)
            {
                UserCastle castle = CastleList[i];
                //List.Add(string.Format(CommandHelp.GameCommandSbkGoldShowMsg, castle.sName, castle.TotalGold, castle.TodayIncome));
            }
        }

        public void Save()
        {
            SaveCastleList();
            for (int i = 0; i < CastleList.Count; i++)
            {
                UserCastle castle = CastleList[i];
                castle.Save();
            }
        }

        public void LoadCastleList()
        {
            string castleFile = Path.Combine(M2Share.BasePath, ModuleShare.Config.CastleFile);
            if (File.Exists(castleFile))
            {
                using StringList loadList = new StringList();
                loadList.LoadFromFile(castleFile);
                for (int i = 0; i < loadList.Count; i++)
                {
                    string sCastleDir = loadList[i].Trim();
                    if (!string.IsNullOrEmpty(sCastleDir))
                    {
                        UserCastle castle = new UserCastle(sCastleDir);
                        CastleList.Add(castle);
                    }
                }
                _logger.Info($"已读取 [{CastleList.Count}] 个城堡信息...");
            }
            else
            {
                _logger.Error("城堡列表文件未找到!!!");
            }
        }

        private void SaveCastleList()
        {
            string castleDirPath = Path.Combine(M2Share.BasePath, ModuleShare.Config.CastleDir);
            if (!Directory.Exists(castleDirPath))
            {
                Directory.CreateDirectory(castleDirPath);
            }
            using StringList loadList = new StringList(CastleList.Count);
            for (int i = 0; i < CastleList.Count; i++)
            {
                loadList.Add(i.ToString());
            }
            string savePath = Path.Combine(M2Share.BasePath, ModuleShare.Config.CastleFile);
            loadList.SaveToFile(savePath);
        }

        public UserCastle GetCastle(int nIndex)
        {
            if (nIndex >= 0 && nIndex < CastleList.Count)
            {
                return CastleList[nIndex];
            }
            return null;
        }

        public void GetCastleNameList(IList<string> List)
        {
            for (int i = 0; i < CastleList.Count; i++)
            {
                List.Add(CastleList[i].sName);
            }
        }

        public void IncRateGold(int nGold)
        {
            for (int i = 0; i < CastleList.Count; i++)
            {
                CastleList[i].IncRateGold(nGold);
            }
        }
    }
}