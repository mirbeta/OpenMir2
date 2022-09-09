using DBSvr.Storage.Model;
using MySql.Data.MySqlClient;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SystemModule.Packet.ClientPackets;
using SystemModule.Packet.ServerPackets;

namespace DBSvr.Storage.MySQL
{
    public class MySqlPlayRecordService : IPlayRecordService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private int RecordCount = 0;
        private readonly Dictionary<string, int> QuickList = null;
        private readonly Dictionary<int, string> IndexQuickList = null;
        private readonly QuickIdList QuickIDList = null;
        /// <summary>
        /// 已被删除的记录号
        /// </summary>
        private readonly IList<int> DeletedList = null;

        private readonly StorageOption _storageOption;

        public MySqlPlayRecordService(StorageOption option)
        {
            QuickList = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            IndexQuickList = new Dictionary<int, string>();
            QuickIDList = new QuickIdList();
            DeletedList = new List<int>();
            RecordCount = 0;
            _storageOption = option;
        }

        public void LoadQuickList()
        {
            QuickList.Clear();
            QuickIDList.Clear();
            DeletedList.Clear();
            var nRecordIndex = 1;
            IList<QuickId> AccountList = new List<QuickId>();
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
                    DBRecord.Selected = (byte)dr.GetInt32("FLD_SelectID");
                    DBRecord.Deleted = dr.GetBoolean("FLD_IsDeleted");
                    DBRecord.Header = new TRecordHeader();
                    DBRecord.Header.sAccount = DBRecord.sAccount;
                    DBRecord.Header.sName = DBRecord.sChrName;
<<<<<<< HEAD
                    DBRecord.Header.Deleted = DBRecord.Deleted;
=======
                    DBRecord.Header.Deleted = DBRecord.boDeleted;
>>>>>>> 4aee4eb831d80ccdcb0b3029c343b73a91bccc80
                    if (!DBRecord.Header.Deleted)
                    {
                        QuickList.Add(DBRecord.Header.sName, nRecordIndex);
                        IndexQuickList.Add(nRecordIndex, DBRecord.Header.sName);
                        AccountList.Add(new QuickId()
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
            var dbConnection = new MySqlConnection(_storageOption.ConnectionString);
            try
            {
                dbConnection.Open();
                succes = true;
            }
            catch (Exception e)
            {
                _logger.Error("打开数据库[MySql]失败.");
                _logger.Error(e.StackTrace);
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
            var humRecord = new HumRecordData();
            using var dr = command.ExecuteReader();
            if (dr.Read())
            {
<<<<<<< HEAD
                var HumRecord = new HumRecordData();
                HumRecord.sAccount = dr.GetString("FLD_Account");
                HumRecord.sChrName = dr.GetString("FLD_CharName");
                HumRecord.Selected = (byte)dr.GetUInt32("FLD_SelectID");
                HumRecord.Deleted = dr.GetBoolean("FLD_IsDeleted");
                HumRecord.Header = new TRecordHeader();
                HumRecord.Header.sAccount = HumRecord.sAccount;
                HumRecord.Header.sName = HumRecord.sChrName;
                HumRecord.Header.SelectID = HumRecord.Selected;
                HumRecord.Header.Deleted = HumRecord.Deleted;
=======
                humRecord.sAccount = dr.GetString("FLD_Account");
                humRecord.sChrName = dr.GetString("FLD_CharName");
                humRecord.boSelected = (byte)dr.GetUInt32("FLD_SelectID");
                humRecord.boDeleted = dr.GetBoolean("FLD_IsDeleted");
                humRecord.Header = new TRecordHeader();
                humRecord.Header.sAccount = humRecord.sAccount;
                humRecord.Header.sName = humRecord.sChrName;
                humRecord.Header.nSelectID = humRecord.boSelected;
                humRecord.Header.Deleted = humRecord.boDeleted;
>>>>>>> 4aee4eb831d80ccdcb0b3029c343b73a91bccc80
                success = true;
            }
            Close(dbConnection);
            return humRecord;
        }

        public int FindByName(string sChrName, ArrayList ChrList)
        {
            for (var i = 0; i < QuickList.Count; i++)
            {
                //if (HUtil32.CompareLStr(m_QuickList[i], sChrName, sChrName.Length))
                //{
                //    ChrList.Add(m_QuickList[i], m_QuickList.Values[i]);
                //}
            }
            return ChrList.Count;
        }

        public HumRecordData GetBy(int nIndex, ref bool success)
        {
            if (nIndex > 0) return GetRecord(nIndex, ref success);
            success = false;
            return default;
        }

        public int FindByAccount(string sAccount, ref IList<QuickId> ChrList)
        {
            IList<QuickId> ChrNameList = null;
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
            IList<QuickId> ChrList = null;
            QuickIDList.GetChrList(sAccount, ref ChrList);
            var success = false;
            if (ChrList != null)
            {
                for (var i = 0; i < ChrList.Count; i++)
                {
                    HumRecordData HumDBRecord = GetBy(ChrList[i].nIndex, ref success);
                    if (success && !HumDBRecord.Deleted)
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
                    QuickIDList.AddRecord(HumRecord.sAccount, HumRecord.sChrName, nIndex, HumRecord.Header.SelectID);
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
                if (boNew && (!HumRecord.Header.Deleted) && (!string.IsNullOrEmpty(HumRecord.Header.sName)))
                {
                    var strSql = new StringBuilder();
                    strSql.AppendLine("INSERT INTO TBL_HUMRECORD (FLD_Account, FLD_CharName, FLD_SelectID, FLD_IsDeleted, FLD_CreateDate, FLD_ModifyDate) VALUES ");
                    strSql.AppendLine("(@FLD_Account, @FLD_CharName, @FLD_SelectID, @FLD_IsDeleted, now(), now());");
                    var command = new MySqlCommand();
                    command.Connection = dbConnection;
                    command.CommandText = strSql.ToString();
                    command.Parameters.AddWithValue("@FLD_Account", HumRecord.sAccount);
                    command.Parameters.AddWithValue("@FLD_CharName", HumRecord.sChrName);
                    command.Parameters.AddWithValue("@FLD_SelectID", HumRecord.Selected);
                    command.Parameters.AddWithValue("@FLD_IsDeleted", HumRecord.Deleted);
                    command.ExecuteNonQuery();
                    var id = command.LastInsertedId;
                    nIndex = (int)id;
                    result = true;
                }
                else
                {
                    HumRecord.Header.Deleted = false;
                    var strSql = new StringBuilder();
                    strSql.AppendLine("UPDATE TBL_HUMRECORD SET FLD_Account = @FLD_Account, FLD_CharName = @FLD_CharName, FLD_SelectID = @FLD_SelectID, FLD_IsDeleted = @FLD_IsDeleted, ");
                    strSql.AppendLine(" FLD_ModifyDate = now() WHERE Id = @Id;");
                    var command = new MySqlCommand();
                    command.Connection = dbConnection;
                    command.CommandText = strSql.ToString();
                    command.Parameters.AddWithValue("@FLD_Account", HumRecord.sAccount);
                    command.Parameters.AddWithValue("@FLD_CharName", HumRecord.sChrName);
                    command.Parameters.AddWithValue("@FLD_SelectID", HumRecord.Selected);
                    command.Parameters.AddWithValue("@FLD_IsDeleted", HumRecord.Deleted);
                    command.Parameters.AddWithValue("@Id", nIndex);
                    command.ExecuteNonQuery();
                    result = true;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
            }
            finally
            {
                Close(dbConnection);
            }
            return result;
        }

        public bool Delete(string sName)
        {
            IList<QuickId> ChrNameList = null;
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