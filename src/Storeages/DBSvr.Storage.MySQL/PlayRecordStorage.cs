using DBSvr.Storage.Model;
using MySqlConnector;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using SystemModule.Data;

namespace DBSvr.Storage.MySQL
{
    public class PlayRecordStorage : IPlayRecordStorage
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private int _recordCount;
        private readonly Dictionary<string, int> _quickList;
        private readonly Dictionary<int, string> _indexQuickList;
        private readonly PlayQuickList _quickIdList;
        /// <summary>
        /// 已被删除的记录号
        /// </summary>
        private readonly IList<int> _deletedList;
        private readonly StorageOption _storageOption;

        public PlayRecordStorage(StorageOption storageOption)
        {
            _quickList = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            _indexQuickList = new Dictionary<int, string>();
            _quickIdList = new PlayQuickList();
            _deletedList = new List<int>();
            _recordCount = 0;
            _storageOption = storageOption;
        }

        public void LoadQuickList()
        {
            _quickList.Clear();
            _quickIdList.Clear();
            _deletedList.Clear();
            var connSuccess = false;
            var connection = Open(ref connSuccess);
            if (!connSuccess)
            {
                return;
            }
            IList<PlayQuick> AccountList = new List<PlayQuick>();
            IList<string> ChrNameList = new List<string>();
            try
            {
                var command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "select * from characters_indexes where IsDeleted < 2"; //排除异常数据
                using var dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var DBRecord = new PlayerRecordData();
                    DBRecord.Id = dr.GetInt32("Id");
                    DBRecord.sAccount = dr.GetString("Account");
                    DBRecord.sChrName = dr.GetString("ChrName");
                    DBRecord.Selected = (byte)dr.GetInt32("SelectID");
                    DBRecord.Deleted = dr.GetBoolean("IsDeleted");
                    DBRecord.Header = new RecordHeader();
                    DBRecord.Header.sAccount = DBRecord.sAccount;
                    DBRecord.Header.Name = DBRecord.sChrName;
                    DBRecord.Header.Deleted = DBRecord.Deleted;
                    if (!DBRecord.Header.Deleted)
                    {
                        _quickList.Add(DBRecord.Header.Name, DBRecord.Id);
                        _indexQuickList.Add(DBRecord.Id, DBRecord.Header.Name);
                        AccountList.Add(new PlayQuick()
                        {
                            Index = DBRecord.Id,
                            Account = DBRecord.sAccount,
                            SelectID = 0
                        });
                        ChrNameList.Add(DBRecord.sChrName);
                    }
                    else
                    {
                        _deletedList.Add(DBRecord.Id);
                    }
                }
                dr.Close();
                dr.Dispose();
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            for (var nIndex = 0; nIndex < AccountList.Count; nIndex++)
            {
                _quickIdList.AddRecord(AccountList[nIndex].Account, ChrNameList[nIndex], AccountList[nIndex].Index, AccountList[nIndex].SelectID);
            }
            AccountList = null;
            ChrNameList = null;
        }

        private MySqlConnection Open(ref bool succes)
        {
            var connection = new MySqlConnection(_storageOption.ConnectionString);
            try
            {
                connection.Open();
                succes = true;
            }
            catch (Exception e)
            {
                _logger.Error("打开数据库[MySql]失败.");
                _logger.Error(e.StackTrace);
                succes = false;
            }
            return connection;
        }

        public int Index(string sName)
        {
            if (_quickList.TryGetValue(sName, out int nIndex))
            {
                return nIndex;
            }
            return -1;
        }

        public PlayerRecordData Get(int nIndex, ref bool success)
        {
            return GetRecord(nIndex, ref success);
        }

        private PlayerRecordData GetRecord(int nIndex, ref bool success)
        {
            var connSuccess = false;
            var connection = Open(ref connSuccess);
            if (!connSuccess)
            {
                return default;
            }
            const string sSqlStrig = "select * from characters_indexes where Id=@Id";
            PlayerRecordData humRecord = null;
            try
            {
                var command = new MySqlCommand();
                command.CommandText = sSqlStrig;
                command.Connection = connection;
                command.Parameters.AddWithValue("@Id", nIndex);
                using var dr = command.ExecuteReader();
                if (dr.Read())
                {
                    humRecord = new PlayerRecordData();
                    humRecord.sAccount = dr.GetString("Account");
                    humRecord.sChrName = dr.GetString("ChrName");
                    humRecord.Selected = (byte)dr.GetUInt32("SelectID");
                    humRecord.Deleted = dr.GetBoolean("IsDeleted");
                    humRecord.Header = new RecordHeader();
                    humRecord.Header.sAccount = humRecord.sAccount;
                    humRecord.Header.Name = humRecord.sChrName;
                    humRecord.Header.SelectID = humRecord.Selected;
                    humRecord.Header.Deleted = humRecord.Deleted;
                    success = true;
                }
                dr.Close();
                dr.Dispose();
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
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

        public PlayerRecordData GetBy(int nIndex, ref bool success)
        {
            if (nIndex > 0) return GetRecord(nIndex, ref success);
            success = false;
            return default;
        }

        public int FindByAccount(string sAccount, ref IList<PlayQuick> ChrList)
        {
            IList<PlayQuick> ChrNameList = null;
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
            IList<PlayQuick> ChrList = null;
            _quickIdList.GetChrList(sAccount, ref ChrList);
            var success = false;
            if (ChrList != null)
            {
                for (var i = 0; i < ChrList.Count; i++)
                {
                    PlayerRecordData HumDBRecord = GetBy(ChrList[i].Index, ref success);
                    if (success && !HumDBRecord.Deleted)
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        public bool Add(PlayerRecordData HumRecord)
        {
            bool result = false;
            if (_quickList.ContainsKey(HumRecord.Header.Name))
            {
                if (_quickList[HumRecord.Header.Name] > 0)
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
                    _quickList.Add(HumRecord.Header.Name, nIndex);
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

        private const string InsertChrIndexes = "INSERT INTO characters_indexes (Account, ChrName, SelectID, IsDeleted, CreateDate, ModifyDate) VALUES (@Account, @ChrName, @SelectID, @IsDeleted, now(), now());";
        private const string UpdateChrIndexes = "UPDATE characters_indexes SET Account = @Account, ChrName = @ChrName, SelectID = @SelectID, IsDeleted = @IsDeleted,ModifyDate = now() WHERE Id = @Id;";

        private bool UpdateRecord(PlayerRecordData HumRecord, bool boNew, ref int nIndex)
        {
            var connSuccess = false;
            var connection = Open(ref connSuccess);
            if (!connSuccess)
            {
                return false;
            }
            var result = false;
            try
            {
                if (boNew && (!HumRecord.Header.Deleted) && (!string.IsNullOrEmpty(HumRecord.Header.Name)))
                {
                    var command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandText = InsertChrIndexes;
                    command.Parameters.AddWithValue("@Account", HumRecord.sAccount);
                    command.Parameters.AddWithValue("@ChrName", HumRecord.sChrName);
                    command.Parameters.AddWithValue("@SelectID", HumRecord.Selected);
                    command.Parameters.AddWithValue("@IsDeleted", HumRecord.Deleted);
                    command.ExecuteNonQuery();
                    var id = command.LastInsertedId;
                    nIndex = (int)id;
                    result = true;
                    _indexQuickList.Add(nIndex, HumRecord.sChrName);
                }
                else
                {
                    HumRecord.Header.Deleted = false;
                    var command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandText = UpdateChrIndexes;
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
                connection.Close();
                connection.Dispose();
            }
            return result;
        }

        public bool Delete(string sName)
        {
            IList<PlayQuick> ChrNameList = null;
            var result = false;
            if (_quickList.TryGetValue(sName, out int nIndex))
            {
                if (nIndex < 0)
                {
                    return true;
                }
            }
            if (DeleteRecord(nIndex))
            {
                _indexQuickList.Remove(nIndex);
                result = true;
            }
            PlayerRecordData HumRecord = Get(nIndex, ref result);
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
            const string DeleteChrIndexes = "UPDATE characters_indexes SET IsDeleted = @IsDeleted,ModifyDate = now() WHERE Id = @Id;";
            var connSuccess = false;
            var connection = Open(ref connSuccess);
            if (!connSuccess)
            {
                return false;
            }
            try
            {
                var command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = DeleteChrIndexes;
                command.Parameters.AddWithValue("@IsDeleted", 2);
                command.Parameters.AddWithValue("@Id", nIndex);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e.Source);
                return false;
            }
            finally
            {
                connection.Clone();
                connection.Dispose();
            }
        }

        public bool Update(int nIndex, ref PlayerRecordData HumDBRecord)
        {
            if (nIndex < 0)
            {
                return false;
            }
            if (!_indexQuickList.ContainsKey(nIndex))
            {
                return false;
            }
            if (UpdateRecord(HumDBRecord, false, ref nIndex))
            {
                return true;
            }
            return false;
        }

        public void UpdateBy(int nIndex, ref PlayerRecordData HumDBRecord)
        {
            UpdateRecord(HumDBRecord, false, ref nIndex);
        }
    }
}