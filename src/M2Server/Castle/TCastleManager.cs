using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using mSystemModule;

namespace M2Server
{
    public class TCastleManager
    {
        private readonly object _criticalSection;
        private readonly IList<TUserCastle> m_CastleList;

        public TCastleManager()
        {
            m_CastleList = new List<TUserCastle>();
            _criticalSection = new object();
        }

        public TUserCastle Find(string sCastleName)
        {
            TUserCastle result = null;
            TUserCastle Castle = null;
            for (var i = 0; i < m_CastleList.Count; i++)
            {
                Castle = m_CastleList[i];
                if (string.Compare(Castle.m_sName.ToLower(), sCastleName.ToLower(), StringComparison.Ordinal) == 0)
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
            for (var i = 0; i < m_CastleList.Count; i++)
            {
                Castle = m_CastleList[i];
                if (Castle.InCastleWarArea(BaseObject.m_PEnvir, BaseObject.m_nCurrX, BaseObject.m_nCurrY))
                {
                    result = Castle;
                    break;
                }
            }
            return result;
        }

        public TUserCastle InCastleWarArea(TEnvirnoment Envir, int nX, int nY)
        {
            TUserCastle result = null;
            TUserCastle Castle = null;
            for (var i = 0; i < m_CastleList.Count; i++)
            {
                Castle = m_CastleList[i];
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
            if (m_CastleList.Count <= 0)
            {
                Castle = new TUserCastle(M2Share.g_Config.sCastleDir);
                m_CastleList.Add(Castle);
                Castle.Initialize();
                Castle.m_sConfigDir = "0";
                Castle.m_EnvirList.Add("0151");
                Castle.m_EnvirList.Add("0152");
                Castle.m_EnvirList.Add("0153");
                Castle.m_EnvirList.Add("0154");
                Castle.m_EnvirList.Add("0155");
                Castle.m_EnvirList.Add("0156");
                for (var i = 0; i < Castle.m_EnvirList.Count; i++)
                    Castle.m_EnvirList[i] = M2Share.g_MapManager.FindMap(Castle.m_EnvirList[i]).sMapName;
                Save();
                return;
            }

            for (var i = 0; i < m_CastleList.Count; i++)
            {
                Castle = m_CastleList[i];
                Castle.Initialize();
            }
        }

        // 城堡皇宫所在地图
        public TUserCastle IsCastlePalaceEnvir(TEnvirnoment Envir)
        {
            TUserCastle result = null;
            TUserCastle Castle = null;
            for (var i = 0; i < m_CastleList.Count; i++)
            {
                Castle = m_CastleList[i];
                if (Castle.m_MapPalace == Envir)
                {
                    result = Castle;
                    break;
                }
            }

            return result;
        }

        // 城堡所在地图
        public TUserCastle IsCastleEnvir(TEnvirnoment Envir)
        {
            TUserCastle result = null;
            TUserCastle Castle = null;
            for (var i = 0; i < m_CastleList.Count; i++)
            {
                Castle = m_CastleList[i];
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
            for (var i = 0; i < m_CastleList.Count; i++)
            {
                Castle = m_CastleList[i];
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
            __Lock();
            try
            {
                for (var i = 0; i < m_CastleList.Count; i++)
                {
                    UserCastle = m_CastleList[i];
                    UserCastle.Run();
                }
            }
            finally
            {
                UnLock();
            }
        }

        public void GetCastleGoldInfo(ArrayList List)
        {
            TUserCastle Castle;
            for (var i = 0; i < m_CastleList.Count; i++)
            {
                Castle = m_CastleList[i];
                List.Add(string.Format(M2Share.g_sGameCommandSbkGoldShowMsg,
                    new object[] {Castle.m_sName, Castle.m_nTotalGold, Castle.m_nTodayIncome}));
            }
        }

        public void Save()
        {
            TUserCastle Castle;
            SaveCastleList();
            for (var i = 0; i < m_CastleList.Count; i++)
            {
                Castle = m_CastleList[i];
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
                    if (sCastleDir != "")
                    {
                        Castle = new TUserCastle(sCastleDir);
                        m_CastleList.Add(Castle);
                    }
                }
                M2Share.MainOutMessage("已读取 " + m_CastleList.Count + "个城堡信息...", messageColor: ConsoleColor.Green);
            }
            else
            {
                M2Share.MainOutMessage("城堡列表文件未找到！！！");
            }
        }

        public void SaveCastleList()
        {
            ArrayList LoadList;
            if (!Directory.Exists(M2Share.g_Config.sCastleDir)) Directory.CreateDirectory(M2Share.g_Config.sCastleDir);
            LoadList = new ArrayList();
            for (var i = 0; i < m_CastleList.Count; i++) LoadList.Add(i.ToString());
            //LoadList.SaveToFile(M2Share.g_Config.sCastleFile);
            //LoadList.Free;
        }

        public TUserCastle GetCastle(int nIndex)
        {
            TUserCastle result = null;
            if (nIndex >= 0 && nIndex < m_CastleList.Count) result = m_CastleList[nIndex];
            return result;
        }

        public void GetCastleNameList(ArrayList List)
        {
            TUserCastle Castle;
            for (var i = 0; i < m_CastleList.Count; i++)
            {
                Castle = m_CastleList[i];
                List.Add(Castle.m_sName);
            }
        }

        public void IncRateGold(int nGold)
        {
            TUserCastle Castle;
            __Lock();
            try
            {
                for (var i = 0; i < m_CastleList.Count; i++)
                {
                    Castle = m_CastleList[i];
                    Castle.IncRateGold(nGold);
                }
            }
            finally
            {
                UnLock();
            }
        }

        private void __Lock()
        {
            HUtil32.EnterCriticalSection(_criticalSection);
        }

        private void UnLock()
        {
            HUtil32.LeaveCriticalSection(_criticalSection);
        }
    }
}