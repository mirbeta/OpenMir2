using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using SystemModule;

namespace DBSvr
{
    public class MySqlHumRecordDB
    {
        public TDBHeader m_Header = null;
        public Dictionary<string, int> m_QuickList = null;
        public Dictionary<int, string> m_IndexQuickList = null;
        public TQuickIDList m_QuickIDList = null;
        /// <summary>
        /// 已被删除的记录号
        /// </summary>
        public IList<int> m_DeletedList = null;

        private IDbConnection _dbConnection = null;

        public MySqlHumRecordDB()
        {
            m_QuickList = new Dictionary<string, int>();
            m_IndexQuickList = new Dictionary<int, string>();
            m_QuickIDList = new TQuickIDList();
            m_DeletedList = new List<int>();
            LoadQuickList();
            m_Header = new TDBHeader();
        }

        private void LoadQuickList()
        {
            int nIndex = 0;
            THumInfo DBRecord = null;
            m_QuickList.Clear();
            m_QuickIDList.Clear();
            m_DeletedList.Clear();
            var nRecordIndex = 0;
            IList<TQuickID> AccountList = new List<TQuickID>();
            IList<string> ChrNameList = new List<string>();
            try
            {
                if (Open())
                {
                    var command = new MySqlCommand();
                    command.Connection = (MySqlConnection)_dbConnection;
                    command.CommandText = "select * from TBL_HUMRECORD";
                    using var dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        DBRecord = new THumInfo();
                        DBRecord.Id = dr.GetInt32("Id");
                        DBRecord.sAccount = dr.GetString("FLD_Account");
                        DBRecord.sChrName = dr.GetString("FLD_ChrName");
                        DBRecord.boSelected = dr.GetBoolean("FLD_SelectID");
                        DBRecord.boDeleted = dr.GetBoolean("FLD_IsDeleted");
                        DBRecord.Header.sAccount = DBRecord.sAccount;
                        DBRecord.Header.sName = DBRecord.sChrName;
                        DBRecord.Header.nSelectID = DBRecord.boSelected ? 1 : 0;
                        DBRecord.Header.boDeleted = DBRecord.boDeleted;
                        if (!DBRecord.Header.boDeleted)
                        {
                            m_QuickList.Add(DBRecord.Header.sName, nRecordIndex);
                            m_IndexQuickList.Add(nRecordIndex, DBRecord.Header.sName);
                            AccountList.Add(new TQuickID()
                            {
                                nIndex = DBRecord.Id, 
                                sAccount = DBRecord.sAccount,
                                nSelectID = DBRecord.Header.nSelectID
                            });
                            ChrNameList.Add(DBRecord.sChrName);
                        }
                        else
                        {
                            m_DeletedList.Add(nIndex);
                        }
                        nRecordIndex++;
                        nIndex++;
                    }
                    dr.Close();
                    dr.Dispose();
                }
            }
            finally
            {
                Close();
            }
            for (nIndex = 0; nIndex < AccountList.Count; nIndex++)
            {
                m_QuickIDList.AddRecord(AccountList[nIndex].sAccount, ChrNameList[nIndex], AccountList[nIndex].nIndex, AccountList[nIndex].nSelectID);
            }
            AccountList = null;
            ChrNameList = null;
            //m_QuickList.SortString(0, m_QuickList.Count - 1);
        }

        private void Close()
        {
            if (_dbConnection != null)
            {
                _dbConnection.Close();
                _dbConnection.Dispose();
            }
        }

        public bool Open()
        {
            bool result = false;
            if (_dbConnection == null)
            {
                _dbConnection = new MySqlConnection(DBShare.DBConnection);
            }
            if (_dbConnection.State == ConnectionState.Open)
            {
                return true;
            }
            if (_dbConnection.State == ConnectionState.Closed)
            {
                try
                {
                    _dbConnection.Open();
                    result = true;
                }
                catch (Exception e)
                {
                    DBShare.OutMainMessage("打开数据库[MySql]失败.");
                    result = false;
                }
            }
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
            if (!Open())
            {
                return result;
            }
            var command = new MySqlCommand();
            command.CommandText = "select * from TBL_HUMRECORD where Id=@Id";
            command.Connection = (MySqlConnection)_dbConnection;
            command.Parameters.AddWithValue("@Id", nIndex);
            using var dr = command.ExecuteReader();
            if (dr.Read())
            {
                HumDBRecord = new THumInfo();
                HumDBRecord.sAccount = dr.GetString("FLD_Account");
                HumDBRecord.sChrName = dr.GetString("FLD_ChrName");
                HumDBRecord.boSelected = dr.GetBoolean("FLD_SelectID");
                HumDBRecord.boDeleted = dr.GetBoolean("FLD_IsDeleted");
                HumDBRecord.Header.sAccount = HumDBRecord.sAccount;
                HumDBRecord.Header.sName = HumDBRecord.sChrName;
                HumDBRecord.Header.nSelectID = HumDBRecord.boSelected ? 1 : 0;
                HumDBRecord.Header.boDeleted = HumDBRecord.boDeleted;
                result = true;
            }
            return result;
        }

        public int FindByName(string sChrName, ArrayList ChrList)
        {
            int result;
            for (var i = 0; i < m_QuickList.Count; i++)
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
                for (var i = 0; i < ChrNameList.Count; i++)
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
                for (var i = 0; i < ChrList.Count; i++)
                {
                    if (GetBy(ChrList[i].nIndex, ref HumDBRecord) && (!HumDBRecord.boDeleted))
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        public bool Add(THumInfo HumRecord)
        {
            bool result = false;
            if (m_QuickList.ContainsKey(HumRecord.Header.sName))
            {
                if (m_QuickList[HumRecord.Header.sName] >= 0)
                {
                    return false;
                }
            }
            else
            {
                var oldHeader = m_Header;
                int nIndex = 0;
                if (m_DeletedList.Count > 0)
                {
                    nIndex = m_DeletedList[0];
                    m_DeletedList.RemoveAt(0);
                }
                else
                {
                    nIndex = m_Header.nHumCount;
                    m_Header.nHumCount++;
                }
                if (UpdateRecord(nIndex, HumRecord, true))
                {
                    m_QuickList.Add(HumRecord.Header.sName, nIndex);
                    m_QuickIDList.AddRecord(HumRecord.sAccount, HumRecord.sChrName, nIndex, HumRecord.Header.nSelectID);
                    result = true;
                }
                else
                {
                    m_Header = oldHeader;
                    result = false;
                }
            }
            return result;
        }

        private bool UpdateRecord(int nIndex, THumInfo HumRecord, bool boNew)
        {
            bool result = false;
            THumInfo HumRcd = HumRecord;
            try
            {
                if (boNew && (!HumRcd.Header.boDeleted) && (HumRcd.Header.sName != ""))
                {
                    if (!Open())
                    {
                        return false;
                    }
                    var strSql = new StringBuilder();
                    strSql.AppendLine("INSERT INTO `TBL_HUMRECORD` (`FLD_Account`, `FLD_ChrName`, `FLD_SelectID`, `FLD_IsDeleted`, `FLD_CreateDate`, `FLD_ModifyDate`) VALUES ");
                    strSql.AppendLine("(@FLD_Account, @FLD_ChrName, @FLD_SelectID, @FLD_IsDeleted, now(), now());");
                    var command = new MySqlCommand();
                    command.Connection = (MySqlConnection)_dbConnection;
                    command.CommandText = strSql.ToString();
                    command.Parameters.AddWithValue("@FLD_Account", HumRecord.sAccount);
                    command.Parameters.AddWithValue("@FLD_ChrName", HumRecord.sChrName);
                    command.Parameters.AddWithValue("@FLD_SelectID", HumRecord.boSelected);
                    command.Parameters.AddWithValue("@FLD_IsDeleted", HumRecord.boDeleted);
                    command.ExecuteNonQuery();
                    result = true;
                }
                else
                {
                    if (!Open())
                    {
                        return false;
                    }
                    HumRecord.Header.boDeleted = false;
                    var strSql = new StringBuilder();
                    strSql.AppendLine("UPDATE TBL_HUMRECORD SET FLD_Account = @FLD_Account, FLD_ChrName = @FLD_ChrName, FLD_SelectID = @FLD_SelectID, FLD_IsDeleted = @FLD_IsDeleted, ");
                    strSql.AppendLine(" FLD_ModifyDate = now() WHERE Id = @Id;");
                    var command = new MySqlCommand();
                    command.Connection = (MySqlConnection)_dbConnection;
                    command.CommandText = strSql.ToString();
                    command.Parameters.AddWithValue("@FLD_Account", HumRecord.sAccount);
                    command.Parameters.AddWithValue("@FLD_ChrName", HumRecord.sChrName);
                    command.Parameters.AddWithValue("@FLD_SelectID", HumRecord.boSelected);
                    command.Parameters.AddWithValue("@FLD_IsDeleted", HumRecord.Header.boDeleted);
                    command.Parameters.AddWithValue("@Id", nIndex);
                    command.ExecuteNonQuery();
                    result = true;
                }
            }
            catch (Exception e)
            {
                Close();
                DBShare.OutMainMessage(e.Message);
            }
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
            if (UpdateRecord(m_QuickList[HumDBRecord.sAccount], HumDBRecord, false))
            {
                result = true;
            }
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