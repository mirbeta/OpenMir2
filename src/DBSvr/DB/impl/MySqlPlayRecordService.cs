using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SystemModule;

namespace DBSvr
{
    public class MySqlPlayRecordService : IPlayRecordService
    {
        private TDBHeader m_Header = null;
        private readonly Dictionary<string, int> m_QuickList = null;
        private readonly Dictionary<int, string> m_IndexQuickList = null;
        private readonly TQuickIDList m_QuickIDList = null;
        /// <summary>
        /// 已被删除的记录号
        /// </summary>
        private readonly IList<int> m_DeletedList = null;
        private IDbConnection _dbConnection = null;

        public MySqlPlayRecordService()
        {
            m_QuickList = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            m_IndexQuickList = new Dictionary<int, string>();
            m_QuickIDList = new TQuickIDList();
            m_DeletedList = new List<int>();
            m_Header = new TDBHeader();
        }

        public void LoadQuickList()
        {
            m_QuickList.Clear();
            m_QuickIDList.Clear();
            m_DeletedList.Clear();
            var nRecordIndex = 1;
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
                        var DBRecord = new HumRecordData();
                        DBRecord.Id = dr.GetInt32("Id");
                        DBRecord.sAccount = dr.GetString("FLD_Account");
                        DBRecord.sChrName = dr.GetString("FLD_CharName");
                        DBRecord.boSelected = (byte)dr.GetInt32("FLD_SelectID");
                        DBRecord.boDeleted = dr.GetBoolean("FLD_IsDeleted");
                        DBRecord.Header = new TRecordHeader();
                        DBRecord.Header.sAccount = DBRecord.sAccount;
                        DBRecord.Header.sName = DBRecord.sChrName;
                        DBRecord.Header.boDeleted = DBRecord.boDeleted;
                        if (!DBRecord.Header.boDeleted)
                        {
                            m_QuickList.Add(DBRecord.Header.sName, nRecordIndex);
                            m_IndexQuickList.Add(nRecordIndex, DBRecord.Header.sName);
                            AccountList.Add(new TQuickID()
                            {
                                nIndex = DBRecord.Id,
                                sAccount = DBRecord.sAccount,
                                nSelectID = 0
                            });
                            ChrNameList.Add(DBRecord.sChrName);
                        }
                        else
                        {
                            m_DeletedList.Add(DBRecord.Id);
                        }
                        nRecordIndex++;
                    }
                    dr.Close();
                    dr.Dispose();
                }
            }
            finally
            {
                Close();
            }
            for (var nIndex = 0; nIndex < AccountList.Count; nIndex++)
            {
                m_QuickIDList.AddRecord(AccountList[nIndex].sAccount, ChrNameList[nIndex], AccountList[nIndex].nIndex, AccountList[nIndex].nSelectID);
            }
            AccountList = null;
            ChrNameList = null;
            //m_QuickList.SortString(0, m_QuickList.Count - 1);
        }

        private void Close()
        {
            if (_dbConnection == null) return;
            _dbConnection.Close();
            _dbConnection.Dispose();
        }

        private bool Open()
        {
            bool result = false;
            _dbConnection ??= new MySqlConnection(DBShare.DBConnection);
            switch (_dbConnection.State)
            {
                case ConnectionState.Open:
                    return true;
                case ConnectionState.Closed:
                    try
                    {
                        _dbConnection.Open();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        DBShare.MainOutMessage("打开数据库[MySql]失败.");
                        DBShare.MainOutMessage(e.StackTrace);
                        result = false;
                    }
                    break;
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

        public HumRecordData Get(int nIndex, ref bool success)
        {
            return GetRecord(nIndex, ref success);
        }

        private HumRecordData GetRecord(int nIndex, ref bool success)
        {
            if (!Open())
            {
                success = false;
                return default;
            }
            var command = new MySqlCommand();
            command.CommandText = "select * from TBL_HUMRECORD where Id=@Id";
            command.Connection = (MySqlConnection)_dbConnection;
            command.Parameters.AddWithValue("@Id", nIndex);
            using var dr = command.ExecuteReader();
            if (dr.Read())
            {
                var HumRecord = new HumRecordData();
                HumRecord.sAccount = dr.GetString("FLD_Account");
                HumRecord.sChrName = dr.GetString("FLD_CharName");
                HumRecord.boSelected = (byte)dr.GetUInt32("FLD_SelectID");
                HumRecord.boDeleted = dr.GetBoolean("FLD_IsDeleted");
                HumRecord.Header = new TRecordHeader();
                HumRecord.Header.sAccount = HumRecord.sAccount;
                HumRecord.Header.sName = HumRecord.sChrName;
                HumRecord.Header.nSelectID = HumRecord.boSelected;
                HumRecord.Header.boDeleted = HumRecord.boDeleted;
                success = true;
                return HumRecord;
            }
            return default;
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

        public HumRecordData GetBy(int nIndex, ref bool success)
        {
            if (nIndex > 0) return GetRecord(nIndex, ref success);
            success = false;
            return default;
        }

        public int FindByAccount(string sAccount, ref IList<TQuickID> ChrList)
        {
            IList<TQuickID> ChrNameList = null;
            m_QuickIDList.GetChrList(sAccount, ref ChrNameList);
            if (ChrNameList != null)
            {
                for (var i = 0; i < ChrNameList.Count; i++)
                {
                    var quickId = ChrNameList[i];
                    ChrList.Add(quickId);
                }
            }
            return ChrList.Count;
        }

        public int ChrCountOfAccount(string sAccount)
        {
            HumRecordData HumDBRecord;
            var result = 0;
            IList<TQuickID> ChrList = null;
            m_QuickIDList.GetChrList(sAccount, ref ChrList);
            var success = false;
            if (ChrList != null)
            {
                for (var i = 0; i < ChrList.Count; i++)
                {
                    HumDBRecord = GetBy(ChrList[i].nIndex, ref success);
                    if (success && (!HumDBRecord.boDeleted))
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        public bool Add(HumRecordData HumRecord)
        {
            bool result = false;
            if (m_QuickList.ContainsKey(HumRecord.Header.sName))
            {
                if (m_QuickList[HumRecord.Header.sName] > 0)
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
                if (UpdateRecord(HumRecord, true, ref nIndex))
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

        private bool UpdateRecord(HumRecordData HumRecord, bool boNew, ref int nIndex)
        {
            bool result = false;
            try
            {
                if (boNew && (!HumRecord.Header.boDeleted) && (!string.IsNullOrEmpty(HumRecord.Header.sName)))
                {
                    if (!Open())
                    {
                        return false;
                    }
                    var strSql = new StringBuilder();
                    strSql.AppendLine("INSERT INTO TBL_HUMRECORD (FLD_Account, FLD_CharName, FLD_SelectID, FLD_IsDeleted, FLD_CreateDate, FLD_ModifyDate) VALUES ");
                    strSql.AppendLine("(@FLD_Account, @FLD_CharName, @FLD_SelectID, @FLD_IsDeleted, now(), now());");
                    var command = new MySqlCommand();
                    command.Connection = (MySqlConnection)_dbConnection;
                    command.CommandText = strSql.ToString();
                    command.Parameters.AddWithValue("@FLD_Account", HumRecord.sAccount);
                    command.Parameters.AddWithValue("@FLD_CharName", HumRecord.sChrName);
                    command.Parameters.AddWithValue("@FLD_SelectID", HumRecord.boSelected);
                    command.Parameters.AddWithValue("@FLD_IsDeleted", HumRecord.boDeleted);
                    command.ExecuteNonQuery();
                    var id = command.LastInsertedId;
                    nIndex = (int)id;
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
                    strSql.AppendLine("UPDATE TBL_HUMRECORD SET FLD_Account = @FLD_Account, FLD_CharName = @FLD_CharName, FLD_SelectID = @FLD_SelectID, FLD_IsDeleted = @FLD_IsDeleted, ");
                    strSql.AppendLine(" FLD_ModifyDate = now() WHERE Id = @Id;");
                    var command = new MySqlCommand();
                    command.Connection = (MySqlConnection)_dbConnection;
                    command.CommandText = strSql.ToString();
                    command.Parameters.AddWithValue("@FLD_Account", HumRecord.sAccount);
                    command.Parameters.AddWithValue("@FLD_CharName", HumRecord.sChrName);
                    command.Parameters.AddWithValue("@FLD_SelectID", HumRecord.boSelected);
                    command.Parameters.AddWithValue("@FLD_IsDeleted", HumRecord.boDeleted);
                    command.Parameters.AddWithValue("@Id", nIndex);
                    command.ExecuteNonQuery();
                    result = true;
                }
            }
            catch (Exception e)
            {
                Close();
                DBShare.MainOutMessage(e.Message);
            }
            return result;
        }

        public bool Delete(string sName)
        {
            IList<TQuickID> ChrNameList = null;
            var result = false;
            int n10 = m_QuickList[sName];
            if (n10 < 0)
            {
                return result;
            }
            HumRecordData HumRecord = Get(n10, ref result);
            //if (DeleteRecord(m_IndexQuickList[n10]))
            //{
            //    m_QuickList.Remove(n10);
            //    result = true;
            //}
            if (result)
            {
                var n14 = m_QuickIDList.GetChrList(HumRecord.sAccount, ref ChrNameList);
                if (n14 >= 0)
                {
                    m_QuickIDList.DelRecord(n14, HumRecord.sChrName);
                }
            }
            return result;
        }

        private bool DeleteRecord(int nIndex)
        {
            TRecordHeader HumRcdHeader = null;
            var result = false;
            HumRcdHeader.boDeleted = true;
            HumRcdHeader.dCreateDate = HUtil32.DateTimeToDouble(DateTime.Now);
            m_DeletedList.Add(nIndex);
            result = true;
            return result;
        }

        public bool Update(int nIndex, ref HumRecordData HumDBRecord)
        {
            var result = false;
            if (nIndex < 0)
            {
                return result;
            }
            if (m_QuickList.Count < nIndex)
            {
                return result;
            }
            if (UpdateRecord(HumDBRecord, false, ref nIndex))
            {
                result = true;
            }
            return result;
        }

        public void UpdateBy(int nIndex, ref HumRecordData HumDBRecord)
        {
            UpdateRecord(HumDBRecord, false, ref nIndex);
        }
    }
}