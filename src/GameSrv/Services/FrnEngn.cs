using NLog;
using SystemModule.Data;

namespace GameSrv.Services
{
    public class FrontEngine
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public readonly object UserCriticalSection;
        public readonly IList<SavePlayerRcd> m_SaveRcdList;
        private readonly IList<GoldChangeInfo> m_ChangeGoldList;
        public readonly IList<SavePlayerRcd> m_SaveRcdTempList;
        public IList<LoadDBInfo> m_LoadRcdTempList;
        public IList<LoadDBInfo> m_LoadRcdList;

        public FrontEngine()
        {
            UserCriticalSection = new object();
            m_LoadRcdList = new List<LoadDBInfo>();
            m_SaveRcdList = new List<SavePlayerRcd>();
            m_ChangeGoldList = new List<GoldChangeInfo>();
            m_LoadRcdTempList = new List<LoadDBInfo>();
            m_SaveRcdTempList = new List<SavePlayerRcd>();
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

        public void ProcessGameDate()
        {
            IList<GoldChangeInfo> changeGoldList = null;
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
                    for (var i = 0; i < m_ChangeGoldList.Count; i++)
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
                }
            }
        }

        public bool IsFull()
        {
            bool result = false;
            HUtil32.EnterCriticalSection(UserCriticalSection);
            try
            {
                if (m_SaveRcdList.Count >= int.MaxValue)
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
    }
}