using GameSvr.Player;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packets.ServerPackets;

namespace GameSvr.Services
{
    public class TFrontEngine
    {
        private readonly object m_UserCriticalSection;
        private IList<LoadDBInfo> m_LoadRcdList;
        private readonly IList<SavePlayerRcd> m_SaveRcdList;
        private readonly IList<TGoldChangeInfo> m_ChangeGoldList;
        private IList<LoadDBInfo> m_LoadRcdTempList;
        private readonly IList<SavePlayerRcd> m_SaveRcdTempList;

        public TFrontEngine()
        {
            m_UserCriticalSection = new object();
            m_LoadRcdList = new List<LoadDBInfo>();
            m_SaveRcdList = new List<SavePlayerRcd>();
            m_ChangeGoldList = new List<TGoldChangeInfo>();
            m_LoadRcdTempList = new List<LoadDBInfo>();
            m_SaveRcdTempList = new List<SavePlayerRcd>();
        }

        public void Start(CancellationToken stoppingToken)
        {
            Task.Factory.StartNew(() =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    Execute();
                    Thread.Sleep(20);
                }
            }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        private void Execute()
        {
            const string sExceptionMsg = "[Exception] TFrontEngine::Execute";
            try
            {
                ProcessGameDate();
                GetGameTime();
            }
            catch (Exception ex)
            {
                M2Share.Log.LogError(sExceptionMsg);
                M2Share.Log.LogError(ex.StackTrace);
            }
        }

        private void GetGameTime()
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
                    M2Share.g_nGameTime = 1;//白天
                    break;
                case 11:
                case 23:
                case 20:
                    M2Share.g_nGameTime = 2;//日落
                    break;
                case 4:
                case 15:
                    M2Share.g_nGameTime = 0;//日出
                    break;
                case 0:
                case 1:
                case 2:
                case 3:
                case 12:
                case 13:
                case 14:
                case 22:
                    //M2Share.g_nGameTime = 3;//夜晚
                    M2Share.g_nGameTime = 1;//白天
                    break;
            }
        }

        public bool IsIdle()
        {
            var result = false;
            HUtil32.EnterCriticalSection(m_UserCriticalSection);
            try
            {
                if (m_SaveRcdList.Count == 0)
                {
                    result = true;
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(m_UserCriticalSection);
            }
            return result;
        }

        public int SaveListCount()
        {
            var result = 0;
            HUtil32.EnterCriticalSection(m_UserCriticalSection);
            try
            {
                result = m_SaveRcdList.Count;
            }
            finally
            {
                HUtil32.LeaveCriticalSection(m_UserCriticalSection);
            }
            return result;
        }

        private void ProcessGameDate()
        {
            IList<TGoldChangeInfo> ChangeGoldList = null;
            var boReTryLoadDB = false;
            HUtil32.EnterCriticalSection(m_UserCriticalSection);
            try
            {
                if (m_SaveRcdList.Any())
                {
                    for (var i = 0; i < m_SaveRcdList.Count; i++)
                    {
                        m_SaveRcdTempList.Add(m_SaveRcdList[i]);
                    }
                }
                IList<LoadDBInfo> TempList = m_LoadRcdTempList;
                m_LoadRcdTempList = m_LoadRcdList;
                m_LoadRcdList = TempList;
                if (m_ChangeGoldList.Any())
                {
                    ChangeGoldList = new List<TGoldChangeInfo>();
                    for (var i = 0; i < m_ChangeGoldList.Count; i++)
                    {
                        ChangeGoldList.Add(m_ChangeGoldList[i]);
                    }
                    m_ChangeGoldList.Clear();
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(m_UserCriticalSection);
            }
            if (PlayerDataService.SocketConnected())
            {
                PlayerDataService.ProcessQueryList();
                PlayerDataService.ProcessSaveList();
                for (var i = 0; i < m_SaveRcdTempList.Count; i++)
                {
                    SavePlayerRcd SaveRcd = m_SaveRcdTempList[i];
                    if (SaveRcd == null)
                    {
                        continue;
                    }
                    if (PlayerDataService.SaveHumRcdToDB(SaveRcd.sAccount, SaveRcd.sChrName, SaveRcd.nSessionID, SaveRcd.HumanRcd) || SaveRcd.nReTryCount > 50)
                    {
                        if (SaveRcd.PlayObject != null)
                        {
                            ((PlayObject)SaveRcd.PlayObject).m_boRcdSaved = true;
                        }
                        HUtil32.EnterCriticalSection(m_UserCriticalSection);
                        try
                        {
                            for (var j = 0; j < m_SaveRcdList.Count; j++)
                            {
                                if (m_SaveRcdList[j] == SaveRcd)
                                {
                                    m_SaveRcdList.RemoveAt(j);
                                    DisPose(SaveRcd);
                                    break;
                                }
                            }
                        }
                        finally
                        {
                            HUtil32.LeaveCriticalSection(m_UserCriticalSection);
                        }
                    }
                    else
                    {
                        SaveRcd.nReTryCount++;
                    }
                }
            }
            else
            {
                // 如果DB已经关闭，不在保存
                M2Share.Log.LogError("DBSvr 断开链接，保存数据失败.");
                HUtil32.EnterCriticalSection(m_UserCriticalSection);
                try
                {
                    for (var i = 0; i < m_SaveRcdList.Count; i++)
                    {
                        if (m_SaveRcdList[i] != null)
                        {
                            DisPose(m_SaveRcdList[i]);
                        }
                    }
                }
                finally
                {
                    HUtil32.LeaveCriticalSection(m_UserCriticalSection);
                }
                m_SaveRcdList.Clear();
            }
            m_SaveRcdTempList.Clear();
            for (var i = 0; i < m_LoadRcdTempList.Count; i++)
            {
                var LoadDBInfo = m_LoadRcdTempList[i];
                if (LoadDBInfo == null)
                {
                    continue;
                }
                if (!LoadHumFromDB(LoadDBInfo, ref boReTryLoadDB))
                {
                    M2Share.GateMgr.CloseUser(LoadDBInfo.nGateIdx, LoadDBInfo.nSocket);
                }
                else
                {
                    if (!boReTryLoadDB)
                    {
                        DisPose(LoadDBInfo);
                    }
                    else
                    {
                        // 如果读取人物数据失败(数据还没有保存),则重新加入队列
                        HUtil32.EnterCriticalSection(m_UserCriticalSection);
                        try
                        {
                            m_LoadRcdList.Add(LoadDBInfo);
                        }
                        finally
                        {
                            HUtil32.LeaveCriticalSection(m_UserCriticalSection);
                        }
                    }
                }
            }
            m_LoadRcdTempList.Clear();
            if (ChangeGoldList != null)
            {
                for (var i = 0; i < ChangeGoldList.Count; i++)
                {
                    var GoldChangeInfo = ChangeGoldList[i];
                    if (GoldChangeInfo == null)
                    {
                        continue;
                    }
                    ChangeUserGoldInDB(GoldChangeInfo);
                    DisPose(GoldChangeInfo);
                }
            }
        }

        public bool IsFull()
        {
            var result = false;
            HUtil32.EnterCriticalSection(m_UserCriticalSection);
            try
            {
                if (m_SaveRcdList.Count >= 1000)
                {
                    result = true;
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(m_UserCriticalSection);
            }
            return result;
        }

        public void AddToLoadRcdList(string sAccount, string sChrName, string sIPaddr, bool boFlag, int nSessionID, int nPayMent, int nPayMode, int nSoftVersionDate, int nSocket, ushort nGSocketIdx, int nGateIdx, long playTime)
        {
            var loadRcdInfo = new LoadDBInfo
            {
                Account = sAccount,
                ChrName = sChrName,
                sIPaddr = sIPaddr,
                nSessionID = nSessionID,
                nSoftVersionDate = nSoftVersionDate,
                nPayMent = nPayMent,
                nPayMode = nPayMode,
                nSocket = nSocket,
                nGSocketIdx = nGSocketIdx,
                nGateIdx = nGateIdx,
                dwNewUserTick = HUtil32.GetTickCount(),
                PlayObject = null,
                nReLoadCount = 0,
                PlayTime = playTime
            };
            HUtil32.EnterCriticalSection(m_UserCriticalSection);
            try
            {
                m_LoadRcdList.Add(loadRcdInfo);
            }
            finally
            {
                HUtil32.LeaveCriticalSection(m_UserCriticalSection);
            }
        }

        private bool LoadHumFromDB(LoadDBInfo LoadUser, ref bool boReTry)
        {
            int queryId = 0;
            var result = false;
            boReTry = false;
            if (InSaveRcdList(LoadUser.ChrName))
            {
                boReTry = true;// 反回TRUE,则重新加入队列
                return result;
            }
            if (M2Share.WorldEngine.GetPlayObjectEx(LoadUser.ChrName) != null)
            {
                M2Share.WorldEngine.KickPlayObjectEx(LoadUser.ChrName);
                boReTry = true;// 反回TRUE,则重新加入队列
                return result;
            }
            if (!PlayerDataService.LoadHumRcdFromDB(LoadUser.Account, LoadUser.ChrName, LoadUser.sIPaddr, ref queryId, LoadUser.nSessionID))
            {
                M2Share.GateMgr.SendOutConnectMsg(LoadUser.nGateIdx, LoadUser.nSocket, LoadUser.nGSocketIdx);
            }
            else
            {
                var userOpenInfo = new UserOpenInfo
                {
                    sChrName = LoadUser.ChrName,
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
            var result = false;
            HUtil32.EnterCriticalSection(m_UserCriticalSection);
            try
            {
                for (var i = 0; i < m_SaveRcdList.Count; i++)
                {
                    if (string.Compare(m_SaveRcdList[i].sChrName, sChrName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = true;
                        break;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(m_UserCriticalSection);
            }
            return result;
        }

        /// <summary>
        /// 添加到加载队列中
        /// </summary>
        public void AddChangeGoldList(string sGameMasterName, string sGetGoldUserName, int nGold)
        {
            var GoldInfo = new TGoldChangeInfo
            {
                sGameMasterName = sGameMasterName,
                sGetGoldUser = sGetGoldUserName,
                nGold = nGold
            };
            m_ChangeGoldList.Add(GoldInfo);
        }

        /// <summary>
        /// 添加到保存队列中
        /// </summary>
        public void AddToSaveRcdList(SavePlayerRcd SaveRcd)
        {
            HUtil32.EnterCriticalSection(m_UserCriticalSection);
            try
            {
                m_SaveRcdList.Add(SaveRcd);
            }
            finally
            {
                HUtil32.LeaveCriticalSection(m_UserCriticalSection);
            }
        }

        public void DeleteHuman(int nGateIndex, int nSocket)
        {
            LoadDBInfo LoadRcdInfo;
            HUtil32.EnterCriticalSection(m_UserCriticalSection);
            try
            {
                for (var i = 0; i < m_LoadRcdList.Count; i++)
                {
                    LoadRcdInfo = m_LoadRcdList[i];
                    if (LoadRcdInfo.nGateIdx == nGateIndex && LoadRcdInfo.nSocket == nSocket)
                    {
                        DisPose(LoadRcdInfo);
                        m_LoadRcdList.RemoveAt(i);
                        break;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(m_UserCriticalSection);
            }
        }

        private bool ChangeUserGoldInDB(TGoldChangeInfo GoldChangeInfo)
        {
            PlayerDataInfo HumanRcd = null;
            var result = false;
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

        private void DisPose(object obj)
        {
        }
    }
}