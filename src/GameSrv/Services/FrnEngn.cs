using GameSrv.GameGate;
using GameSrv.Player;
using NLog;
using SystemModule.Data;

namespace GameSrv.Services
{
    public class FrontEngine
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly object UserCriticalSection;
        private readonly IList<SavePlayerRcd> m_SaveRcdList;
        private readonly IList<GoldChangeInfo> m_ChangeGoldList;
        private readonly IList<SavePlayerRcd> m_SaveRcdTempList;
        private IList<LoadDBInfo> m_LoadRcdTempList;
        private IList<LoadDBInfo> m_LoadRcdList;

        public FrontEngine()
        {
            UserCriticalSection = new object();
            m_LoadRcdList = new List<LoadDBInfo>();
            m_SaveRcdList = new List<SavePlayerRcd>();
            m_ChangeGoldList = new List<GoldChangeInfo>();
            m_LoadRcdTempList = new List<LoadDBInfo>();
            m_SaveRcdTempList = new List<SavePlayerRcd>();
        }

        internal void GetGameTime()
        {
            switch (DateTime.Now.Hour)
            {
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 16:
                case 17:
                case 18:
                case 19:
                case 21:
                    M2Share.GameTime = 1;//白天
                    break;
                case 11:
                case 23:
                case 20:
                    M2Share.GameTime = 2;//日落
                    break;
                case 4:
                case 15:
                    M2Share.GameTime = 0;//日出
                    break;
                case 0:
                case 1:
                case 2:
                case 3:
                case 12:
                case 13:
                case 14:
                case 22:
                    M2Share.GameTime = 3;//夜晚
                    break;
            }
        }

        public bool IsIdle()
        {
            bool result = false;
            HUtil32.EnterCriticalSection(UserCriticalSection);
            try
            {
                if (m_SaveRcdList.Count == 0)
                {
                    result = true;
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(UserCriticalSection);
            }
            return result;
        }

        public int SaveListCount()
        {
            int result = 0;
            HUtil32.EnterCriticalSection(UserCriticalSection);
            try
            {
                result = m_SaveRcdList.Count;
            }
            finally
            {
                HUtil32.LeaveCriticalSection(UserCriticalSection);
            }
            return result;
        }

        public void RemoveSaveList(int queryId)
        {
            HUtil32.EnterCriticalSection(UserCriticalSection);
            try
            {
                for (int j = 0; j < m_SaveRcdList.Count; j++)
                {
                    if (m_SaveRcdList[j].QueryId == queryId)
                    {
                        m_SaveRcdList.RemoveAt(j);
                        break;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(UserCriticalSection);
            }
        }

        internal void ProcessGameDate()
        {
            IList<GoldChangeInfo> changeGoldList = null;
            bool boReTryLoadDb = false;
            HUtil32.EnterCriticalSection(UserCriticalSection);
            try
            {
                if (m_SaveRcdList.Any())
                {
                    for (int i = 0; i < m_SaveRcdList.Count; i++)
                    {
                        m_SaveRcdTempList.Add(m_SaveRcdList[i]);
                    }
                }
                IList<LoadDBInfo> TempList = m_LoadRcdTempList;
                m_LoadRcdTempList = m_LoadRcdList;
                m_LoadRcdList = TempList;
                if (m_ChangeGoldList.Any())
                {
                    changeGoldList = new List<GoldChangeInfo>();
                    for (int i = 0; i < m_ChangeGoldList.Count; i++)
                    {
                        changeGoldList.Add(m_ChangeGoldList[i]);
                    }
                    m_ChangeGoldList.Clear();
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(UserCriticalSection);
            }
            if (PlayerDataService.SocketConnected())
            {
                PlayerDataService.ProcessQueryList();
                PlayerDataService.ProcessSaveList();
                for (int i = 0; i < m_SaveRcdTempList.Count; i++)
                {
                    SavePlayerRcd SaveRcd = m_SaveRcdTempList[i];
                    if (SaveRcd == null)
                    {
                        continue;
                    }
                    if (SaveRcd.IsSaveing)
                    {
                        continue;
                    }
                    SaveRcd.IsSaveing = true;
                    if (!PlayerDataService.SaveHumRcdToDB(SaveRcd, ref SaveRcd.QueryId) || SaveRcd.ReTryCount > 50)
                    {
                        SaveRcd.ReTryCount++;
                    }
                    else
                    {
                        if (SaveRcd.PlayObject != null)
                        {
                            ((PlayObject)SaveRcd.PlayObject).RcdSaved = true;
                        }
                    }
                }
            }
            else
            {
                // 如果DB已经关闭，不在保存
                _logger.Error("DBSrv 断开链接，保存数据失败.");
                HUtil32.EnterCriticalSection(UserCriticalSection);
                try
                {
                    for (int i = 0; i < m_SaveRcdList.Count; i++)
                    {
                        if (m_SaveRcdList[i] != null)
                        {
                            DisPose(m_SaveRcdList[i]);
                        }
                    }
                }
                finally
                {
                    HUtil32.LeaveCriticalSection(UserCriticalSection);
                }
                m_SaveRcdList.Clear();
            }
            m_SaveRcdTempList.Clear();
            for (int i = 0; i < m_LoadRcdTempList.Count; i++)
            {
                LoadDBInfo loadDbInfo = m_LoadRcdTempList[i];
                if (loadDbInfo.SessionID == 0)
                {
                    continue;
                }
                if (!LoadHumFromDB(loadDbInfo, ref boReTryLoadDb))
                {
                    M2Share.GateMgr.CloseUser(loadDbInfo.GateIdx, loadDbInfo.SocketId);
                    _logger.Debug("读取用户数据失败，踢出用户.");
                }
                else
                {
                    if (!boReTryLoadDb)
                    {
                        DisPose(loadDbInfo);
                    }
                    else
                    {
                        // 如果读取人物数据失败(数据还没有保存),则重新加入队列
                        HUtil32.EnterCriticalSection(UserCriticalSection);
                        try
                        {
                            m_LoadRcdList.Add(loadDbInfo);
                        }
                        finally
                        {
                            HUtil32.LeaveCriticalSection(UserCriticalSection);
                        }
                    }
                }
            }
            m_LoadRcdTempList.Clear();
            if (changeGoldList != null)
            {
                for (int i = 0; i < changeGoldList.Count; i++)
                {
                    GoldChangeInfo goldChangeInfo = changeGoldList[i];
                    if (goldChangeInfo.nGold <= 0)
                    {
                        continue;
                    }
                    ChangeUserGoldInDB(goldChangeInfo);
                    DisPose(goldChangeInfo);
                }
            }
        }

        public bool IsFull()
        {
            bool result = false;
            HUtil32.EnterCriticalSection(UserCriticalSection);
            try
            {
                if (m_SaveRcdList.Count >= ushort.MaxValue)
                {
                    result = true;
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(UserCriticalSection);
            }
            return result;
        }

        public void AddToLoadRcdList(LoadDBInfo loadRcdInfo)
        {
            HUtil32.EnterCriticalSection(UserCriticalSection);
            try
            {
                m_LoadRcdList.Add(loadRcdInfo);
            }
            finally
            {
                HUtil32.LeaveCriticalSection(UserCriticalSection);
            }
        }

        private bool LoadHumFromDB(LoadDBInfo LoadUser, ref bool boReTry)
        {
            int queryId = 0;
            bool result = false;
            boReTry = false;
            if (InSaveRcdList(LoadUser.ChrName))
            {
                boReTry = true;// 反回TRUE,则重新加入队列
                return false;
            }
            if (M2Share.WorldEngine.GetPlayObjectEx(LoadUser.ChrName) != null)
            {
                M2Share.WorldEngine.KickPlayObjectEx(LoadUser.ChrName);
                boReTry = true;// 反回TRUE,则重新加入队列
                return false;
            }
            if (!PlayerDataService.LoadHumRcdFromDB(LoadUser.Account, LoadUser.ChrName, LoadUser.sIPaddr, ref queryId, LoadUser.SessionID))
            {
                GameGateMgr.SendOutConnectMsg(LoadUser.GateIdx, LoadUser.SocketId, LoadUser.GSocketIdx);
            }
            else
            {
                UserOpenInfo userOpenInfo = new UserOpenInfo
                {
                    ChrName = LoadUser.ChrName,
                    LoadUser = LoadUser,
                    HumanRcd = null,
                    QueryId = queryId
                };
                M2Share.WorldEngine.AddUserOpenInfo(userOpenInfo);
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 检查是否在保存队列中
        /// </summary>
        /// <param name="sChrName"></param>
        /// <returns></returns>
        public bool InSaveRcdList(string sChrName)
        {
            bool result = false;
            HUtil32.EnterCriticalSection(UserCriticalSection);
            try
            {
                for (int i = 0; i < m_SaveRcdList.Count; i++)
                {
                    if (string.Compare(m_SaveRcdList[i].ChrName, sChrName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = true;
                        break;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(UserCriticalSection);
            }
            return result;
        }

        /// <summary>
        /// 添加到加载队列中
        /// </summary>
        public void AddChangeGoldList(string sGameMasterName, string sGetGoldUserName, int nGold)
        {
            var goldInfo = new GoldChangeInfo
            {
                sGameMasterName = sGameMasterName,
                sGetGoldUser = sGetGoldUserName,
                nGold = nGold
            };
            m_ChangeGoldList.Add(goldInfo);
        }

        /// <summary>
        /// 添加到保存队列中
        /// </summary>
        public void AddToSaveRcdList(SavePlayerRcd SaveRcd)
        {
            HUtil32.EnterCriticalSection(UserCriticalSection);
            try
            {
                m_SaveRcdList.Add(SaveRcd);
            }
            finally
            {
                HUtil32.LeaveCriticalSection(UserCriticalSection);
            }
        }

        public void DeleteHuman(int nGateIndex, int nSocket)
        {
            HUtil32.EnterCriticalSection(UserCriticalSection);
            try
            {
                for (int i = 0; i < m_LoadRcdList.Count; i++)
                {
                    var loadRcdInfo = m_LoadRcdList[i];
                    if (loadRcdInfo.GateIdx == nGateIndex && loadRcdInfo.SocketId == nSocket)
                    {
                        DisPose(loadRcdInfo);
                        m_LoadRcdList.RemoveAt(i);
                        break;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(UserCriticalSection);
            }
        }

        private static bool ChangeUserGoldInDB(GoldChangeInfo GoldChangeInfo)
        {
            bool result = false;
            /*if (PlayerDataService.LoadHumRcdFromDB("1", GoldChangeInfo.sGetGoldUser, "1", ref HumanRcd, 1))
            {
                if (HumanRcd.Data.Gold + GoldChangeInfo.nGold > 0 && HumanRcd.Data.Gold + GoldChangeInfo.nGold < 2000000000)
                {
                    HumanRcd.Data.Gold += GoldChangeInfo.nGold;
                    if (PlayerDataService.SaveHumRcdToDB("1", GoldChangeInfo.sGetGoldUser, 1, HumanRcd))
                    {
                        M2Share.WorldEngine.sub_4AE514(GoldChangeInfo);
                        result = true;
                    }
                }
            }*/
            return result;
        }

        private static void DisPose(object obj)
        {
        }
    }
}