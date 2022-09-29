using DBSvr.Storage.Model;
using MySqlConnector;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SystemModule.Packet.ServerPackets;

namespace DBSvr.Storage.MySQL
{
    public class PlayRecordStorage : IPlayRecordStorage
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private int _recordCount;
        private readonly Dictionary<string, int> _quickList;
        private readonly Dictionary<int, string> _indexQuickList;
        private readonly QuickIdList _quickIdList;
        /// <summary>
        /// 已被删除的记录号
        /// </summary>
        private readonly IList<int> _deletedList;
        private readonly StorageOption _storageOption;
        private MySqlConnection _connection;

        public PlayRecordStorage(StorageOption option)
        {
            _quickList = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            _indexQuickList = new Dictionary<int, string>();
            _quickIdList = new QuickIdList();
            _deletedList = new List<int>();
            _recordCount = 0;
            _storageOption = option;
        }

        public void LoadQuickList()
        {
            _quickList.Clear();
            _quickIdList.Clear();
            _deletedList.Clear();
            var nRecordIndex = 1;
            IList<QuickId> AccountList = new List<QuickId>();
            IList<string> ChrNameList = new List<string>();
            bool result = false;
            Open(ref result);
            if (!result)
            {
                return;
            }
            try
            {
                var command = new MySqlCommand();
                command.Connection = _connection;
                command.CommandText = "select * from characters_indexes";
                using var dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var DBRecord = new HumRecordData();
                    DBRecord.Id = dr.GetInt32("Id");
                    DBRecord.sAccount = dr.GetString("Account");
                    DBRecord.sChrName = dr.GetString("ChrName");
                    DBRecord.Selected = (byte)dr.GetInt32("SelectID");
                    DBRecord.Deleted = dr.GetBoolean("IsDeleted");
                    DBRecord.Header = new RecordHeader();
                    DBRecord.Header.sAccount = DBRecord.sAccount;
                    DBRecord.Header.sName = DBRecord.sChrName;
                    DBRecord.Header.Deleted = DBRecord.Deleted;
                    if (!DBRecord.Header.Deleted)
                    {
                        _quickList.Add(DBRecord.Header.sName, nRecordIndex);
                        _indexQuickList.Add(nRecordIndex, DBRecord.Header.sName);
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
                        _deletedList.Add(DBRecord.Id);
                    }
                    nRecordIndex++;
                }
                dr.Close();
                dr.Dispose();
            }
            finally
            {
                Close(_connection);
            }
            for (var nIndex = 0; nIndex < AccountList.Count; nIndex++)
            {
                _quickIdList.AddRecord(AccountList[nIndex].sAccount, ChrNameList[nIndex], AccountList[nIndex].nIndex, AccountList[nIndex].nSelectID);
            }
            AccountList = null;
            ChrNameList = null;
        }

        private void Close(MySqlConnection _connection)
        {
            if (_connection == null) return;
            _connection.Close();
            _connection.Dispose();
        }

        private void Open(ref bool succes)
        {
            _connection = new MySqlConnection(_storageOption.ConnectionString);
            try
            {
                _connection.Open();
                succes = true;
            }
            catch (Exception e)
            {
                _logger.Error("打开数据库[MySql]失败.");
                _logger.Error(e.StackTrace);
                succes = false;
            }
        }

        public int Index(string sName)
        {
            if (_quickList.TryGetValue(sName, out int nIndex))
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
            Open(ref success);
            if (!success)
            {
                success = false;
                return default;
            }
            var command = new MySqlCommand();
            command.CommandText = "select * from characters_indexes where Id=@Id";
            command.Connection = _connection;
            command.Parameters.AddWithValue("@Id", nIndex);
            var humRecord = new HumRecordData();
            using var dr = command.ExecuteReader();
            if (dr.Read())
            {
                humRecord.sAccount = dr.GetString("Account");
                humRecord.sChrName = dr.GetString("ChrName");
                humRecord.Selected = (byte)dr.GetUInt32("SelectID");
                humRecord.Deleted = dr.GetBoolean("IsDeleted");
                humRecord.Header = new RecordHeader();
                humRecord.Header.sAccount = humRecord.sAccount;
                humRecord.Header.sName = humRecord.sChrName;
                humRecord.Header.SelectID = humRecord.Selected;
                humRecord.Header.Deleted = humRecord.Deleted;
                success = true;
            }
            Close(_connection);
            return humRecord;
        }

        public int FindByName(string sChrName, ArrayList ChrList)
        {
            for (var i = 0; i < _quickList.Count; i++)
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
            _quickIdList.GetChrList(sAccount, ref ChrNameList);
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
            _quickIdList.GetChrList(sAccount, ref ChrList);
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
            if (_quickList.ContainsKey(HumRecord.Header.sName))
            {
                if (_quickList[HumRecord.Header.sName] > 0)
                {
                    return false;
                }
            }
            else
            {
                int nIndex = 0;
                if (_deletedList.Count > 0)
                {
                    nIndex = _deletedList[0];
                    _deletedList.RemoveAt(0);
                }
                else
                {
                    nIndex = _recordCount;
                    _recordCount++;
                }
                if (UpdateRecord(HumRecord, true, ref nIndex))
                {
                    _quickList.Add(HumRecord.Header.sName, nIndex);
                    _quickIdList.AddRecord(HumRecord.sAccount, HumRecord.sChrName, nIndex, HumRecord.Header.SelectID);
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
            Open(ref result);
            if (!result)
            {
                return false;
            }
            try
            {
                if (boNew && (!HumRecord.Header.Deleted) && (!string.IsNullOrEmpty(HumRecord.Header.sName)))
                {
                    var strSql = new StringBuilder();
                    strSql.AppendLine("INSERT INTO characters_indexes (Account, ChrName, SelectID, IsDeleted, CreateDate, ModifyDate) VALUES ");
                    strSql.AppendLine("(@Account, @ChrName, @SelectID, @IsDeleted, now(), now());");
                    var command = new MySqlCommand();
                    command.Connection = _connection;
                    command.CommandText = strSql.ToString();
                    command.Parameters.AddWithValue("@Account", HumRecord.sAccount);
                    command.Parameters.AddWithValue("@ChrName", HumRecord.sChrName);
                    command.Parameters.AddWithValue("@SelectID", HumRecord.Selected);
                    command.Parameters.AddWithValue("@IsDeleted", HumRecord.Deleted);
                    command.ExecuteNonQuery();
                    var id = command.LastInsertedId;
                    nIndex = (int)id;
                    result = true;
                }
                else
                {
                    HumRecord.Header.Deleted = false;
                    var strSql = new StringBuilder();
                    strSql.AppendLine("UPDATE characters_indexes SET Account = @Account, ChrName = @ChrName, SelectID = @SelectID, IsDeleted = @IsDeleted, ");
                    strSql.AppendLine(" ModifyDate = now() WHERE Id = @Id;");
                    var command = new MySqlCommand();
                    command.Connection = _connection;
                    command.CommandText = strSql.ToString();
                    command.Parameters.AddWithValue("@Account", HumRecord.sAccount);
                    command.Parameters.AddWithValue("@ChrName", HumRecord.sChrName);
                    command.Parameters.AddWithValue("@SelectID", HumRecord.Selected);
                    command.Parameters.AddWithValue("@IsDeleted", HumRecord.Deleted);
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
                Close(_connection);
            }
            return result;
        }

        public bool Delete(string sName)
        {
            IList<QuickId> ChrNameList = null;
            var result = false;
            int n10 = _quickList[sName];
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
                var n14 = _quickIdList.GetChrList(HumRecord.sAccount, ref ChrNameList);
                if (n14 >= 0)
                {
                    _quickIdList.DelRecord(n14, HumRecord.sChrName);
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
            if (_quickList.Count < nIndex)
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