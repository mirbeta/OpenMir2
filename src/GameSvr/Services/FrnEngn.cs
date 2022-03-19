using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SystemModule;

namespace GameSvr
{
    public class TFrontEngine
    {
        private object m_UserCriticalSection = null;
        private IList<TLoadDBInfo> m_LoadRcdList = null;
        private IList<TSaveRcd> m_SaveRcdList = null;
        private IList<TGoldChangeInfo> m_ChangeGoldList = null;
        private IList<TLoadDBInfo> m_LoadRcdTempList = null;
        private readonly IList<TSaveRcd> m_SaveRcdTempList = null;
        private readonly Thread _frontEngine;

        public TFrontEngine()
        {
            m_UserCriticalSection = new object();
            m_LoadRcdList = new List<TLoadDBInfo>();
            m_SaveRcdList = new List<TSaveRcd>();
            m_ChangeGoldList = new List<TGoldChangeInfo>();
            m_LoadRcdTempList = new List<TLoadDBInfo>();
            m_SaveRcdTempList = new List<TSaveRcd>();
            _frontEngine = new Thread(Execute)
            {
                IsBackground = true
            };
        }

        public void Start()
        {
            _frontEngine.Start();
        }

        private void Execute()
        {
            const string sExceptionMsg = "[Exception] TFrontEngine::Execute";
            while (true)
            {
                try
                {
                    ProcessGameDate();
                    GetGameTime();
                }
                catch (Exception ex)
                {
                    M2Share.ErrorMessage(sExceptionMsg);
                    M2Share.ErrorMessage(ex.StackTrace);
                }
                Thread.Sleep(200);
            }
        }

        private void GetGameTime()
        {
            var Hour = DateTime.Now.Hour;
            switch (Hour)
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
            TSaveRcd SaveRcd = null;
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
                IList<TLoadDBInfo> TempList = m_LoadRcdTempList;
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
            if (RunDB.DBSocketConnected())
            {
                for (var i = 0; i < m_SaveRcdTempList.Count; i++)
                {
                    SaveRcd = m_SaveRcdTempList[i];
                    if (SaveRcd == null)
                    {
                        continue;
                    }
                    if (RunDB.SaveHumRcdToDB(SaveRcd.sAccount, SaveRcd.sChrName, SaveRcd.nSessionID, ref SaveRcd.HumanRcd) || SaveRcd.nReTryCount > 50)
                    {
                        if (SaveRcd.PlayObject != null)
                        {
                            SaveRcd.PlayObject.m_boRcdSaved = true;
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
                M2Share.ErrorMessage("DBSvr 断开链接，保存数据失败.");
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
                TLoadDBInfo LoadDBInfo = m_LoadRcdTempList[i];
                if (LoadDBInfo == null)
                {
                    continue;
                }
                if (!LoadHumFromDB(LoadDBInfo, ref boReTryLoadDB))
                {
                    M2Share.GateManager.CloseUser(LoadDBInfo.nGateIdx, LoadDBInfo.nSocket);
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
                    TGoldChangeInfo GoldChangeInfo = ChangeGoldList[i];
                    if (GoldChangeInfo == null)
                    {
                        continue;
                    }
                    ChangeUserGoldInDB(GoldChangeInfo);
                    DisPose(GoldChangeInfo);
                }
                ChangeGoldList = null;
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

        public void AddToLoadRcdList(string sAccount, string sChrName, string sIPaddr, bool boFlag, int nSessionID, int nPayMent, int nPayMode, int nSoftVersionDate, int nSocket, ushort nGSocketIdx, int nGateIdx)
        {
            TLoadDBInfo LoadRcdInfo = new TLoadDBInfo
            {
                sAccount = sAccount,
                sCharName = sChrName,
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
                nReLoadCount = 0
            };
            HUtil32.EnterCriticalSection(m_UserCriticalSection);
            try
            {
                m_LoadRcdList.Add(LoadRcdInfo);
            }
            finally
            {
                HUtil32.LeaveCriticalSection(m_UserCriticalSection);
            }
        }

        private bool LoadHumFromDB(TLoadDBInfo LoadUser, ref bool boReTry)
        {
            THumDataInfo HumanRcd = null;
            var result = false;
            boReTry = false;
            if (InSaveRcdList(LoadUser.sCharName))
            {
                boReTry = true;// 反回TRUE,则重新加入队列
                return result;
            }
            if (M2Share.UserEngine.GetPlayObjectEx(LoadUser.sCharName) != null)
            {
                M2Share.UserEngine.KickPlayObjectEx(LoadUser.sCharName);
                boReTry = true;// 反回TRUE,则重新加入队列
                return result;
            }
            if (!RunDB.LoadHumRcdFromDB(LoadUser.sAccount, LoadUser.sCharName, LoadUser.sIPaddr, ref HumanRcd, LoadUser.nSessionID))
            {
                M2Share.GateManager.SendOutConnectMsg(LoadUser.nGateIdx, LoadUser.nSocket, LoadUser.nGSocketIdx);
            }
            else
            {
                var UserOpenInfo = new TUserOpenInfo
                {
                    sChrName = LoadUser.sCharName,
                    LoadUser = LoadUser,
                    HumanRcd = HumanRcd
                };
                M2Share.UserEngine.AddUserOpenInfo(UserOpenInfo);
                result = true;
            }
            return result;
        }

        public bool InSaveRcdList(string sChrName)
        {
            var result = false;
            HUtil32.EnterCriticalSection(m_UserCriticalSection);
            try
            {
                for (var i = 0; i < m_SaveRcdList.Count; i++)
                {
                    if (m_SaveRcdList[i].sChrName == sChrName)
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

        public void AddChangeGoldList(string sGameMasterName, string sGetGoldUserName, int nGold)
        {
            TGoldChangeInfo GoldInfo = new TGoldChangeInfo
            {
                sGameMasterName = sGameMasterName,
                sGetGoldUser = sGetGoldUserName,
                nGold = nGold
            };
            m_ChangeGoldList.Add(GoldInfo);
        }

        public void AddToSaveRcdList(TSaveRcd SaveRcd)
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
            TLoadDBInfo LoadRcdInfo;
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
            THumDataInfo HumanRcd = null;
            var result = false;
            if (RunDB.LoadHumRcdFromDB("1", GoldChangeInfo.sGetGoldUser, "1", ref HumanRcd, 1))
            {
                if (HumanRcd.Data.nGold + GoldChangeInfo.nGold > 0 && HumanRcd.Data.nGold + GoldChangeInfo.nGold < 2000000000)
                {
                    HumanRcd.Data.nGold += GoldChangeInfo.nGold;
                    if (RunDB.SaveHumRcdToDB("1", GoldChangeInfo.sGetGoldUser, 1, ref HumanRcd))
                    {
                        M2Share.UserEngine.sub_4AE514(GoldChangeInfo);
                        result = true;
                    }
                }
            }
            return result;
        }

        private void DisPose(object obj)
        {
            obj = null;
        }
    }
}