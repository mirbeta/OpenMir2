using DBSvr.Conf;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SystemModule;
using SystemModule.Packet.ClientPackets;

namespace DBSvr.DB.impl
{
    public class MySqlPlayRecordService : IPlayRecordService
    {
        private readonly MirLog _logger;
        private int RecordCount = 0;
        private readonly Dictionary<string, int> QuickList = null;
        private readonly Dictionary<int, string> IndexQuickList = null;
        private readonly TQuickIDList QuickIDList = null;
        private DBConfig Config = ConfigManager.GetConfig();
        /// <summary>
        /// 已被删除的记录号
        /// </summary>
        private readonly IList<int> DeletedList = null;

        public MySqlPlayRecordService(MirLog logger)
        {
            _logger = logger;
            QuickList = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            IndexQuickList = new Dictionary<int, string>();
            QuickIDList = new TQuickIDList();
            DeletedList = new List<int>();
            RecordCount = 0;
        }

        public void LoadQuickList()
        {
            QuickList.Clear();
            QuickIDList.Clear();
            DeletedList.Clear();
            var nRecordIndex = 1;
            IList<TQuickID> AccountList = new List<TQuickID>();
            IList<string> ChrNameList = new List<string>();
            bool result = false;
            MySqlConnection dbConnection = Open(ref result);
            if (!result)
            {
                return;
            }
            try
            {
                var command = new MySqlCommand();
                command.Connection = dbConnection;
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
                        QuickList.Add(DBRecord.Header.sName, nRecordIndex);
                        IndexQuickList.Add(nRecordIndex, DBRecord.Header.sName);
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
                        DeletedList.Add(DBRecord.Id);
                    }
                    nRecordIndex++;
                }
                dr.Close();
                dr.Dispose();
            }
            finally
            {
                Close(dbConnection);
            }
            for (var nIndex = 0; nIndex < AccountList.Count; nIndex++)
            {
                QuickIDList.AddRecord(AccountList[nIndex].sAccount, ChrNameList[nIndex], AccountList[nIndex].nIndex, AccountList[nIndex].nSelectID);
            }
            AccountList = null;
            ChrNameList = null;
        }

        private void Close(MySqlConnection dbConnection)
        {
            if (dbConnection == null) return;
            dbConnection.Close();
            dbConnection.Dispose();
        }

        private MySqlConnection Open(ref bool succes)
        {
            var dbConnection = new MySqlConnection(Config.DBConnection);
            try
            {
                dbConnection.Open();
                succes = true;
            }
            catch (Exception e)
            {
                _logger.LogError("打开数据库[MySql]失败.");
                _logger.LogError(e.StackTrace);
                succes = false;
            }
            return dbConnection;
        }

        public int Index(string sName)
        {
            if (QuickList.TryGetValue(sName, out int nIndex))
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
            MySqlConnection dbConnection = Open(ref success);
            if (!success)
            {
                success = false;
                return default;
            }
            var command = new MySqlCommand();
            command.CommandText = "select * from TBL_HUMRECORD where Id=@Id";
            command.Connection = dbConnection;
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
            for (var i = 0; i < QuickList.Count; i++)
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
            QuickIDList.GetChrList(sAccount, ref ChrNameList);
            if (ChrNameList != null)
            {
                for (var i = 0; i < ChrNameList.Count; i++)
                {
                    ChrList.Add(ChrNameList[i]);
                }
            }
            return ChrList.Count;
        }

        public int ChrCountOfAccount(string sAccount)
        {
            var result = 0;
            IList<TQuickID> ChrList = null;
            QuickIDList.GetChrList(sAccount, ref ChrList);
            var success = false;
            if (ChrList != null)
            {
                for (var i = 0; i < ChrList.Count; i++)
                {
                    HumRecordData HumDBRecord = GetBy(ChrList[i].nIndex, ref success);
                    if (success && !HumDBRecord.boDeleted)
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
            if (QuickList.ContainsKey(HumRecord.Header.sName))
            {
                if (QuickList[HumRecord.Header.sName] > 0)
                {
                    return false;
                }
            }
            else
            {
                int nIndex = 0;
                if (DeletedList.Count > 0)
                {
                    nIndex = DeletedList[0];
                    DeletedList.RemoveAt(0);
                }
                else
                {
                    nIndex = RecordCount;
                    RecordCount++;
                }
                if (UpdateRecord(HumRecord, true, ref nIndex))
                {
                    QuickList.Add(HumRecord.Header.sName, nIndex);
                    QuickIDList.AddRecord(HumRecord.sAccount, HumRecord.sChrName, nIndex, HumRecord.Header.nSelectID);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        private bool UpdateRecord(HumRecordData HumRecord, bool boNew, ref int nIndex)
        {
            bool result = false;
            MySqlConnection dbConnection = Open(ref result);
            if (!result)
            {
                return false;
            }
            try
            {
                if (boNew && (!HumRecord.Header.boDeleted) && (!string.IsNullOrEmpty(HumRecord.Header.sName)))
                {
                    var strSql = new StringBuilder();
                    strSql.AppendLine("INSERT INTO TBL_HUMRECORD (FLD_Account, FLD_CharName, FLD_SelectID, FLD_IsDeleted, FLD_CreateDate, FLD_ModifyDate) VALUES ");
                    strSql.AppendLine("(@FLD_Account, @FLD_CharName, @FLD_SelectID, @FLD_IsDeleted, now(), now());");
                    var command = new MySqlCommand();
                    command.Connection = dbConnection;
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
                    HumRecord.Header.boDeleted = false;
                    var strSql = new StringBuilder();
                    strSql.AppendLine("UPDATE TBL_HUMRECORD SET FLD_Account = @FLD_Account, FLD_CharName = @FLD_CharName, FLD_SelectID = @FLD_SelectID, FLD_IsDeleted = @FLD_IsDeleted, ");
                    strSql.AppendLine(" FLD_ModifyDate = now() WHERE Id = @Id;");
                    var command = new MySqlCommand();
                    command.Connection = dbConnection;
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
                _logger.LogError(e.Message);
            }
            finally
            {
                Close(dbConnection);
            }
            return result;
        }

        public bool Delete(string sName)
        {
            IList<TQuickID> ChrNameList = null;
            var result = false;
            int n10 = QuickList[sName];
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
                var n14 = QuickIDList.GetChrList(HumRecord.sAccount, ref ChrNameList);
                if (n14 >= 0)
                {
                    QuickIDList.DelRecord(n14, HumRecord.sChrName);
                }
            }
            return result;
        }

        private bool DeleteRecord(int nIndex)
        {
            throw new NotImplementedException();
        }

        public bool Update(int nIndex, ref HumRecordData HumDBRecord)
        {
            var result = false;
            if (nIndex < 0)
            {
                return false;
            }
            if (QuickList.Count < nIndex)
            {
                return false;
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