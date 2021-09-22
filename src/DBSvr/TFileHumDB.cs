using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SystemModule;

namespace DBSvr
{
    public class TFileHumDB
    {
        public int m_nCurIndex = 0;
        public int m_nFileHandle = 0;
        public int n0C = 0;
        public bool m_boChanged = false;
        public TDBHeader m_Header = null;
        public Dictionary<string, int> m_QuickList = null;
        public Dictionary<int, string> m_IndexQuickList = null;
        public TQuickIDList m_QuickIDList = null;
        /// <summary>
        /// 已被删除的记录号
        /// </summary>
        public IList<int> m_DeletedList = null;
        public string m_sDBFileName = String.Empty;

        public TFileHumDB(string sFileName)
        {
            m_sDBFileName = sFileName;
            m_QuickList = new Dictionary<string, int>();
            m_IndexQuickList = new Dictionary<int, string>();
            m_QuickIDList = new TQuickIDList();
            m_DeletedList = new List<int>();
            LoadQuickList();
        }


        private void LoadQuickList()
        {
            int nRecordIndex;
            int nIndex = 0;
            IList<TQuickID> AccountList;
            IList<string> ChrNameList;
            TDBHeader DBHeader;
            THumInfo DBRecord = null;
            m_nCurIndex = 0;
            m_QuickList.Clear();
            m_QuickIDList.Clear();
            m_DeletedList.Clear();
            nRecordIndex = 0;
            AccountList = new List<TQuickID>();
            ChrNameList = new List<string>();
            try
            {
                if (Open())
                {
                    //FileSeek(m_nFileHandle, 0, 0);
                    //if (FileRead(m_nFileHandle, DBHeader, sizeof(TDBHeader)) == sizeof(TDBHeader))
                    //{
                    //    for (nIndex = 0; nIndex < DBHeader.nHumCount; nIndex++)
                    //    {
                    //        if (FileRead(m_nFileHandle, DBRecord, sizeof(THumInfo)) != sizeof(THumInfo))
                    //        {
                    //            break;
                    //        }
                    if (!DBRecord.Header.boDeleted)
                    {
                        m_QuickList.Add(DBRecord.Header.sName, nRecordIndex);
                        m_IndexQuickList.Add(nRecordIndex, DBRecord.Header.sName);
                        AccountList.Add(new TQuickID() { sAccount = DBRecord.sAccount, nSelectID = DBRecord.Header.nSelectID });
                        ChrNameList.Add(DBRecord.sChrName);
                    }
                    else
                    {
                        m_DeletedList.Add(nIndex);
                    }
                    nRecordIndex++;
                    //    }
                    //}
                }
            }
            finally
            {
                Close();
            }
            for (nIndex = 0; nIndex < AccountList.Count; nIndex++)
            {
                m_QuickIDList.AddRecord(AccountList[nIndex].sAccount, ChrNameList[nIndex], nIndex, AccountList[nIndex].nSelectID);
            }
            AccountList = null;
            ChrNameList = null;
            //m_QuickList.SortString(0, m_QuickList.Count - 1);
        }

        public void Close()
        {
            //m_nFileHandle.Close();
            //if (m_boChanged)
            //{
            //    m_OnChange(this);
            //}
            //UnLock();
        }

        public bool Open()
        {
            bool result;
            m_nCurIndex = 0;
            m_boChanged = false;
            if (File.Exists(m_sDBFileName))
            {
                //m_nFileHandle = File.Open(m_sDBFileName, (FileMode) FileAccess.ReadWrite | FileShare.ReadWrite);
                //if (m_nFileHandle > 0)
                //{
                //    FileRead(m_nFileHandle, m_Header, sizeof(TDBHeader));
                //}
            }
            else
            {
                //m_nFileHandle = File.Create(m_sDBFileName);
                //if (m_nFileHandle > 0)
                //{
                //    FillChar(m_Header, sizeof(TDBHeader), '\0');
                //    m_Header.sDesc = Units.HumDB.sDBHeaderDesc;
                //    m_Header.nHumCount = 0;
                //    m_Header.n6C = 0;
                //    FileWrite(m_nFileHandle, m_Header, sizeof(TDBHeader));
                //}
            }
            if (m_nFileHandle > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public bool OpenEx()
        {
            bool result = false;
            TDBHeader DBHeader;
            m_boChanged = false;
            //m_nFileHandle = File.Open(m_sDBFileName, (FileMode) FileAccess.ReadWrite | FileShare.ReadWrite);
            //if (m_nFileHandle > 0)
            //{
            //    result = true;
            //    if (FileRead(m_nFileHandle, DBHeader, sizeof(TDBHeader)) == sizeof(TDBHeader))
            //    {
            //        m_Header = DBHeader;
            //    }
            //    m_nCurIndex = 0;
            //}
            //else
            //{
            //    result = false;
            //}
            return result;
        }

        public int Index(string sName)
        {
            if (m_QuickList.TryGetValue(sName, out int nIndex))
            {
                return nIndex;
            }
            return -1;
        }

        public int Get(int nIndex, ref THumInfo HumDBRecord)
        {
            //int nResult = m_IndexQuickList[nIndex];
            int result;
            if (GetRecord(nIndex, ref HumDBRecord))
            {
                result = nIndex;
            }
            else
            {
                result = -1;
            }
            return result;
        }

        private bool GetRecord(int nIndex, ref THumInfo HumDBRecord)
        {
            bool result = false;
            //if (FileSeek(m_nFileHandle, sizeof(THumInfo) * nIndex + sizeof(TDBHeader), 0) !=  -1)
            //{
            //    FileRead(m_nFileHandle, HumDBRecord, sizeof(THumInfo));
            //    FileSeek(m_nFileHandle,  -sizeof(THumInfo) * nIndex + sizeof(TDBHeader), 1);
            //    m_nCurIndex = nIndex;
            //    result = true;
            //}
            //else
            //{
            //    result = false;
            //}
            return result;
        }

        public int FindByName(string sChrName, ArrayList ChrList)
        {
            int result;

            for (var i = 0; i < m_QuickList.Count; i ++ )
            {
                //if (HUtil32.CompareLStr(m_QuickList[i], sChrName, sChrName.Length))
                //{
                //    ChrList.Add(m_QuickList[i], m_QuickList.Values[i]);
                //}
            }
            result = ChrList.Count;
            return result;
        }

        public bool GetBy(int nIndex, ref THumInfo HumDBRecord)
        {
            bool result;
            if (nIndex >= 0)
            {
                result = GetRecord(nIndex, ref HumDBRecord);
            }
            else
            {
                result = false;
            }
            return result;
        }

        public int FindByAccount(string sAccount, ref IList<TQuickID> ChrList)
        {
            int result;
            IList<TQuickID> ChrNameList = null;
            TQuickID QuickID;
            m_QuickIDList.GetChrList(sAccount, ref ChrNameList);
            if (ChrNameList != null)
            {
                for (var i = 0; i < ChrNameList.Count; i ++ )
                {
                    QuickID = ChrNameList[i];
                    ChrList.Add(QuickID);
                }
            }
            result = ChrList.Count;
            return result;
        }

        public int ChrCountOfAccount(string sAccount)
        {
            THumInfo HumDBRecord = null;
            var result = 0;
            IList<TQuickID> ChrList = null;
            m_QuickIDList.GetChrList(sAccount, ref ChrList);
            if (ChrList != null)
            {
                for (var i = 0; i < ChrList.Count; i ++ )
                {
                    if (GetBy(ChrList[i].nIndex, ref HumDBRecord) && (!HumDBRecord.boDeleted))
                    {
                        result ++;
                    }
                }
            }
            return result;
        }

        public bool Add(THumInfo HumRecord)
        {
            bool result;
            TDBHeader Header;
            int nIndex;
            if (m_QuickList[HumRecord.Header.sName] >= 0)
            {
                result = false;
            }
            else
            {
                Header = m_Header;
                if (m_DeletedList.Count > 0)
                {
                    nIndex = ((int)m_DeletedList[0]);
                    m_DeletedList.RemoveAt(0);
                }
                else
                {
                    nIndex = m_Header.nHumCount;
                    m_Header.nHumCount ++;
                }
                if (UpdateRecord(nIndex, HumRecord, true))
                {
                    m_QuickList.Add(HumRecord.Header.sName, nIndex);
                    m_QuickIDList.AddRecord(HumRecord.sAccount, HumRecord.sChrName, nIndex, HumRecord.Header.nSelectID);
                    result = true;
                }
                else
                {
                    m_Header = Header;
                    result = false;
                }
            }
            return result;
        }

        private bool UpdateRecord(int nIndex, THumInfo HumRecord, bool boNew)
        {
            bool result;
            THumInfo HumRcd = null;
            //int nPosion = nIndex * sizeof(THumInfo) + sizeof(TDBHeader);
            //if (FileSeek(m_nFileHandle, nPosion, 0) == nPosion)
            //{
            //int n10 = FileSeek(m_nFileHandle, 0, 1);//&& (FileRead(m_nFileHandle, HumRcd, sizeof(THumInfo)) == sizeof(THumInfo)) &&
            if (boNew && (!HumRcd.Header.boDeleted) && (HumRcd.Header.sName != ""))
            {
                result = true;
            }
            else
            {
                HumRecord.Header.boDeleted = false;
                HumRecord.Header.dCreateDate = HUtil32.DateTimeToDouble(DateTime.Now);
                m_Header.dUpdateDate = DateTime.Now;
                //FileSeek(m_nFileHandle, 0, 0);
                //FileWrite(m_nFileHandle, m_Header, sizeof(TDBHeader));
                //FileSeek(m_nFileHandle, n10, 0);
                //FileWrite(m_nFileHandle, HumRecord, sizeof(THumInfo));
                //FileSeek(m_nFileHandle,  -sizeof(THumInfo), 1);
                m_nCurIndex = nIndex;
                m_boChanged = true;
                result = true;
            }
            //}
            //else
            //{
            //     result = false;
            // }
            return result;
        }

        public bool Delete(string sName)
        {
            THumInfo HumRecord = null;
            IList<TQuickID> ChrNameList = null;
            var result = false;
            int n10 = m_QuickList[sName];
            if (n10 < 0)
            {
                return result;
            }
            Get(n10, ref HumRecord);
            //if (DeleteRecord(m_IndexQuickList[n10]))
            //{
            //    m_QuickList.Remove(n10);
            //    result = true;
            //}
            int n14 = m_QuickIDList.GetChrList(HumRecord.sAccount, ref ChrNameList);
            if (n14 >= 0)
            {
                m_QuickIDList.DelRecord(n14, HumRecord.sChrName);
            }
            return result;
        }

        private bool DeleteRecord(int nIndex)
        {
            TRecordHeader HumRcdHeader = null;
            var result = false;
            //if (FileSeek(m_nFileHandle, nIndex * sizeof(THumInfo) + sizeof(TDBHeader), 0) ==  -1)
            //{
            //    return result;
            //}
            HumRcdHeader.boDeleted = true;
            HumRcdHeader.dCreateDate = HUtil32.DateTimeToDouble(DateTime.Now);
            //FileWrite(m_nFileHandle, HumRcdHeader, sizeof(TRecordHeader));
            m_DeletedList.Add(nIndex);
            m_boChanged = true;
            result = true;
            return result;
        }

        public bool Update(int nIndex, ref THumInfo HumDBRecord)
        {
            var result = false;
            if (nIndex < 0)
            {
                return result;
            }
            if (m_QuickList.Count <= nIndex)
            {
                return result;
            }
            //if (UpdateRecord(m_QuickList[nIndex], HumDBRecord, false))
            //{
            //    result = true;
            //}
            return result;
        }

        public bool UpdateBy(int nIndex, ref THumInfo HumDBRecord)
        {
            var result = false;
            if (UpdateRecord(nIndex, HumDBRecord, false))
            {
                result = true;
            }
            return result;
        }

    } 
}