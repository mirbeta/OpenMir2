using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SystemModule.Common;

namespace GameSvr
{
    public class CastleManager
    {
        private readonly IList<TUserCastle> _castleList;

        public CastleManager()
        {
            _castleList = new List<TUserCastle>();
        }

        public TUserCastle Find(string sCastleName)
        {
            TUserCastle result = null;
            TUserCastle Castle = null;
            for (var i = 0; i < _castleList.Count; i++)
            {
                Castle = _castleList[i];
                if (string.Compare(Castle.m_sName, sCastleName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = Castle;
                    break;
                }
            }

            return result;
        }

        // 取得角色所在座标的城堡
        public TUserCastle InCastleWarArea(TBaseObject BaseObject)
        {
            TUserCastle result = null;
            TUserCastle Castle = null;
            for (var i = 0; i < _castleList.Count; i++)
            {
                Castle = _castleList[i];
                if (Castle.InCastleWarArea(BaseObject.m_PEnvir, BaseObject.m_nCurrX, BaseObject.m_nCurrY))
                {
                    result = Castle;
                    break;
                }
            }
            return result;
        }

        public TUserCastle InCastleWarArea(Envirnoment Envir, int nX, int nY)
        {
            TUserCastle result = null;
            TUserCastle Castle = null;
            for (var i = 0; i < _castleList.Count; i++)
            {
                Castle = _castleList[i];
                if (Castle.InCastleWarArea(Envir, nX, nY))
                {
                    result = Castle;
                    break;
                }
            }
            return result;
        }

        public void Initialize()
        {
            TUserCastle Castle;
            if (_castleList.Count <= 0)
            {
                Castle = new TUserCastle(M2Share.g_Config.sCastleDir);
                _castleList.Add(Castle);
                Castle.Initialize();
                Castle.m_sConfigDir = "0";
                Castle.m_EnvirList.Add("0151");
                Castle.m_EnvirList.Add("0152");
                Castle.m_EnvirList.Add("0153");
                Castle.m_EnvirList.Add("0154");
                Castle.m_EnvirList.Add("0155");
                Castle.m_EnvirList.Add("0156");
                for (var i = 0; i < Castle.m_EnvirList.Count; i++)
                {
                    Castle.m_EnvirList[i] = M2Share.g_MapManager.FindMap(Castle.m_EnvirList[i]).sMapName;
                }
                Save();
                return;
            }
            for (var i = 0; i < _castleList.Count; i++)
            {
                Castle = _castleList[i];
                Castle.Initialize();
            }
        }

        // 城堡皇宫所在地图
        public TUserCastle IsCastlePalaceEnvir(Envirnoment Envir)
        {
            TUserCastle result = null;
            TUserCastle Castle = null;
            for (var i = 0; i < _castleList.Count; i++)
            {
                Castle = _castleList[i];
                if (Castle.m_MapPalace == Envir)
                {
                    result = Castle;
                    break;
                }
            }
            return result;
        }

        // 城堡所在地图
        public TUserCastle IsCastleEnvir(Envirnoment Envir)
        {
            TUserCastle result = null;
            TUserCastle Castle = null;
            for (var i = 0; i < _castleList.Count; i++)
            {
                Castle = _castleList[i];
                if (Castle.m_MapCastle == Envir)
                {
                    result = Castle;
                    break;
                }
            }
            return result;
        }

        public TUserCastle IsCastleMember(TBaseObject BaseObject)
        {
            TUserCastle result = null;
            TUserCastle Castle = null;
            for (var i = 0; i < _castleList.Count; i++)
            {
                Castle = _castleList[i];
                if (Castle.IsMember(BaseObject))
                {
                    result = Castle;
                    break;
                }
            }
            return result;
        }

        public void Run()
        {
            TUserCastle UserCastle;
            for (var i = 0; i < _castleList.Count; i++)
            {
                UserCastle = _castleList[i];
                UserCastle.Run();
            }
        }

        public void GetCastleGoldInfo(ArrayList List)
        {
            TUserCastle Castle;
            for (var i = 0; i < _castleList.Count; i++)
            {
                Castle = _castleList[i];
                List.Add(string.Format(M2Share.g_sGameCommandSbkGoldShowMsg, Castle.m_sName, Castle.m_nTotalGold, Castle.m_nTodayIncome));
            }
        }

        public void Save()
        {
            TUserCastle Castle;
            SaveCastleList();
            for (var i = 0; i < _castleList.Count; i++)
            {
                Castle = _castleList[i];
                Castle.Save();
            }
        }

        public void LoadCastleList()
        {
            StringList LoadList;
            TUserCastle Castle;
            string sCastleDir;
            if (File.Exists(M2Share.g_Config.sCastleFile))
            {
                LoadList = new StringList();
                LoadList.LoadFromFile(M2Share.g_Config.sCastleFile);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sCastleDir = LoadList[i].Trim();
                    if (!string.IsNullOrEmpty(sCastleDir))
                    {
                        Castle = new TUserCastle(sCastleDir);
                        _castleList.Add(Castle);
                    }
                }
                M2Share.MainOutMessage($"已读取 [{_castleList.Count}] 个城堡信息...", messageColor: ConsoleColor.Green);
            }
            else
            {
                M2Share.MainOutMessage("城堡列表文件未找到!!!");
            }
        }

        private void SaveCastleList()
        {
            StringList LoadList;
            if (!Directory.Exists(M2Share.g_Config.sCastleDir))
            {
                Directory.CreateDirectory(M2Share.g_Config.sCastleDir);
            }
            LoadList = new StringList();
            for (var i = 0; i < _castleList.Count; i++)
            {
                LoadList.Add(i.ToString());
            }
            LoadList.SaveToFile(M2Share.g_Config.sCastleFile);
            //LoadList.Free;
        }

        public TUserCastle GetCastle(int nIndex)
        {
            TUserCastle result = null;
            if (nIndex >= 0 && nIndex < _castleList.Count)
            {
                result = _castleList[nIndex];
            }
            return result;
        }

        public void GetCastleNameList(IList<string> List)
        {
            TUserCastle Castle;
            for (var i = 0; i < _castleList.Count; i++)
            {
                Castle = _castleList[i];
                List.Add(Castle.m_sName);
            }
        }

        public void IncRateGold(int nGold)
        {
            TUserCastle Castle;
            for (var i = 0; i < _castleList.Count; i++)
            {
                Castle = _castleList[i];
                Castle.IncRateGold(nGold);
            }
        }
    }
}