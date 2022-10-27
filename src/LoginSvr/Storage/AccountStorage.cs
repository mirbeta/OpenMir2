using LoginSvr.Conf;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SystemModule;
using SystemModule.Extensions;
using SystemModule.Packet.ClientPackets;

namespace LoginSvr.Storage
{
    public class AccountStorage
    {
        private readonly MirLog _logger;
        private readonly ConfigManager _configManager;
        private readonly IList<AccountQuick> _quickList;

        public AccountStorage(MirLog logQueue, ConfigManager configManager)
        {
            _logger = logQueue;
            _configManager = configManager;
            _quickList = new List<AccountQuick>();
        }

        private Config Config => _configManager.Config;

        public void Initialization()
        {
            _logger.Information("正在连接SQL服务器...");
            var dbConnection = new MySqlConnection(Config.ConnctionString);
            try
            {
                dbConnection.Open();
                _logger.Information("连接SQL服务器成功...");
                LoadQuickList();
            }
            catch (Exception ex)
            {
                _logger.LogError("[错误] SQL 连接失败!请检查SQL设置...");
                _logger.LogError(ex);
            }
            finally
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }
        }

        private bool Open(ref MySqlConnection dbConnection)
        {
            bool result = false;
            if (dbConnection == null)
            {
                dbConnection = new MySqlConnection(Config.ConnctionString);
            }
            switch (dbConnection.State)
            {
                case ConnectionState.Open:
                    return true;
                case ConnectionState.Closed:
                    try
                    {
                        dbConnection.Open();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        _logger.LogError("打开数据库[MySql]失败.");
                        _logger.LogError(e);
                        result = false;
                    }
                    break;
            }
            return result;
        }

        private void Close(ref MySqlConnection dbConnection)
        {
            if (dbConnection != null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }
        }

        private void LoadQuickList()
        {
            const string sSQL = "SELECT Id,DELETED,LOGINID FROM account";
            _quickList.Clear();
            MySqlConnection dbConnection = null;
            if (!Open(ref dbConnection))
            {
                return;
            }
            try
            {
                var command = new MySqlCommand();
                command.CommandText = sSQL;
                command.Connection = dbConnection;
                using var dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var nIndex = dr.GetInt32("Id");
                    var boDeleted = dr.GetBoolean("DELETED");
                    var sAccount = dr.GetString("LOGINID");
                    if (!boDeleted && (!string.IsNullOrEmpty(sAccount)))
                    {
                        _quickList.Add(new AccountQuick(sAccount, nIndex));
                    }
                }
                dr.Close();
                dr.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError("读取账号列表失败.");
                _logger.LogError(ex);
            }
            finally
            {
                Close(ref dbConnection);
            }
            //m_QuickList.SortString(0, m_QuickList.Count - 1);
        }

        public int FindByName(string sName, ref IList<AccountQuick> List)
        {
            for (var i = 0; i < _quickList.Count; i++)
            {
                if (HUtil32.CompareLStr(_quickList[i].sAccount, sName))
                {
                    List.Add(new AccountQuick(_quickList[i].sAccount, _quickList[i].nIndex));
                }
            }
            return List.Count;
        }

        public bool GetBy(int nIndex, ref AccountRecord DBRecord)
        {
            bool result;
            if ((nIndex >= 0) && (_quickList.Count > nIndex))
            {
                result = GetRecord(nIndex, ref DBRecord);
            }
            else
            {
                result = false;
            }
            return result;
        }

        private bool GetRecord(int nIndex, ref AccountRecord DBRecord)
        {
            const string sSQL = "SELECT * FROM account WHERE ID={0}";
            var result = true;
            MySqlConnection dbConnection = null;
            if (!Open(ref dbConnection))
            {
                return false;
            }
            var command = new MySqlCommand();
            command.CommandText = string.Format(sSQL, nIndex);
            command.Connection = dbConnection;
            IDataReader dr;
            try
            {
                dr = command.ExecuteReader();
                if (DBRecord == null)
                {
                    DBRecord = new AccountRecord();
                    DBRecord.Header = new RecordHeader();
                    DBRecord.UserEntry = new UserEntry();
                    DBRecord.UserEntryAdd = new UserEntryAdd();
                }
                if (dr.Read())
                {
                    DBRecord.Header.sAccount = dr.GetString("LOGINID");
                    DBRecord.Header.boDeleted = dr.GetBoolean(dr.GetOrdinal("DELETED"));
                    DBRecord.Header.CreateDate = dr.GetDateTime("CREATEDATE");
                    DBRecord.Header.UpdateDate = dr.GetDateTime("LASTUPDATE");
                    DBRecord.ErrorCount = dr.GetInt32("ERRORCOUNT");
                    DBRecord.ActionTick = dr.GetInt32("ACTIONTICK");
                    DBRecord.UserEntry.Account = dr.GetString("LOGINID");
                    DBRecord.UserEntry.Password = dr.GetString("PASSWORD");
                    DBRecord.UserEntry.UserName = dr.GetString("USERNAME");
                    DBRecord.UserEntry.SSNo = dr.GetString("SSNO");
                    DBRecord.UserEntry.Phone = dr.GetString("PHONE");
                    DBRecord.UserEntry.Quiz = dr.GetString("QUIZ1");
                    DBRecord.UserEntry.Answer = dr.GetString("ANSWER1");
                    DBRecord.UserEntry.EMail = dr.GetString("EMAIL");
                    DBRecord.UserEntryAdd.Quiz2 = dr.GetString("QUIZ2");
                    DBRecord.UserEntryAdd.Answer2 = dr.GetString("ANSWER2");
                    DBRecord.UserEntryAdd.BirthDay = dr.GetString("BIRTHDAY");
                    DBRecord.UserEntryAdd.MobilePhone = dr.GetString("MOBILEPHONE");
                    DBRecord.UserEntryAdd.Memo = "";
                    DBRecord.UserEntryAdd.Memo2 = "";
                }
                var quickAccount = _quickList.SingleOrDefault(x => x.nIndex == nIndex);
                if (quickAccount != null)
                {
                    if (DBRecord.Header.sAccount == quickAccount.sAccount)
                    {
                        result = true;
                    }
                }
                else
                {
                    result = false;
                }
                dr.Close();
                dr.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError("[Exception] TFileIDDB.GetRecord");
                _logger.LogError(ex);
                return false;
            }
            finally
            {
                Close(ref dbConnection);
            }
            return result;
        }

        public int Index(string sName)
        {
            var quick = _quickList.SingleOrDefault(o => o.sAccount == sName);
            if (quick == null)
            {
                return -1;
            }
            return quick.nIndex;
        }

        public int Get(int nIndex, ref AccountRecord accountRecord)
        {
            int result = -1;
            if (nIndex < 0)
            {
                return result;
            }
            // if (m_QuickList.Count < nIndex)
            // {
            //     return result;
            // }
            if (GetRecord(nIndex, ref accountRecord))
            {
                result = nIndex;
            }
            return result;
        }

        public long GetAccountPlayTime(string account)
        {
            var strSql = $"SELECT SECONDS FROM ACCOUNT WHERE FLD_LOGINID='{account}'";
            _logger.LogDebug("[SQL QUERY] " + strSql);
            MySqlConnection dbConnection = null;
            if (!Open(ref dbConnection))
            {
                return 0;
            }
            long result = 0;
            try
            {
                var command = new MySqlCommand();
                command.Connection = dbConnection;
                command.CommandText = strSql;
                result = (long)command.ExecuteScalar();
            }
            catch (Exception e)
            {
                _logger.LogError($"获取账号[{account}]游戏时间失败");
                _logger.LogError(e);
            }
            finally
            {
                Close(ref dbConnection);
            }
            return result;
        }

        public void UpdateAccountPlayTime(string account,long gameTime)
        {
            var strSql = $"UPDATE ACCOUNT SET SECONDS={gameTime} WHERE LOGINID='{account}'";
            _logger.LogDebug("[SQL QUERY] " + strSql);
            MySqlConnection dbConnection = null;
            if (!Open(ref dbConnection))
            {
                return;
            }
            try
            {
                var command = new MySqlCommand();
                command.Connection = dbConnection;
                command.CommandText = strSql;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.LogError($"更新账号[{account}]游戏时间失败");
                _logger.LogError(e);
            }
            finally
            {
                Close(ref dbConnection);
            }
        }

        private int UpdateRecord(AccountRecord accountRecord, byte btFlag)
        {
            var result = 0;
            const string sdt = "now()";
            const string sUpdateRecord1 = "INSERT INTO account (LOGINID, PASSWORD, USERNAME, CREATEDATE, LASTUPDATE, DELETED, ERRORCOUNT, ACTIONTICK, SSNO, BIRTHDAY, PHONE, MOBILEPHONE, EMAIL, QUIZ1, ANSWER1, QUIZ2, ANSWER2) VALUES('{0}', '{1}', '{2}', {3}, {4}, 0, 0, 0,'{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}');";
            const string sUpdateRecord2 = "UPDATE account SET DELETED=1, CREATEDATE='{0}' WHERE LOGINID='{1}'";
            const string sUpdateRecord0 = "UPDATE account SET PASSWORD='{0}', USERNAME='{1}',LASTUPDATE={2}, ERRORCOUNT={3}, ACTIONTICK={4},SSNO='{5}', BIRTHDAY='{6}', PHONE='{7}',MOBILEPHONE='{8}', EMAIL='{9}', QUIZ1='{10}', ANSWER1='{11}', QUIZ2='{12}',ANSWER2='{13}' WHERE LOGINID='{14}'";
            MySqlConnection dbConnection = null;
            if (!Open(ref dbConnection))
            {
                return 0;
            }
            try
            {
                var command = new MySqlCommand();
                command.Connection = dbConnection;
                switch (btFlag)
                {
                    case 1:
                        command.CommandText = string.Format(sUpdateRecord1, accountRecord.UserEntry.Account, accountRecord.UserEntry.Password, accountRecord.UserEntry.UserName, sdt, sdt, accountRecord.UserEntry.SSNo, accountRecord.UserEntryAdd.BirthDay, accountRecord.UserEntry.Phone, accountRecord.UserEntryAdd.MobilePhone, accountRecord.UserEntry.EMail, accountRecord.UserEntry.Quiz, accountRecord.UserEntry.Answer, accountRecord.UserEntryAdd.Quiz2, accountRecord.UserEntryAdd.Answer2);
                        try
                        {
                            command.ExecuteNonQuery();
                            result = (int)command.LastInsertedId;
                        }
                        catch (Exception E)
                        {
                            _logger.LogError("[Exception] TFileIDDB.UpdateRecord");
                            _logger.LogError(E);
                            return 0;
                        }
                        break;
                    case 2:
                        command.CommandText = string.Format(sUpdateRecord2, sdt, accountRecord.UserEntry.Account);
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            result = 0;
                            _logger.LogError("[Exception] TFileIDDB.UpdateRecord");
                            _logger.LogError(ex);
                        }
                        break;
                    default:
                        command.CommandText = string.Format(sUpdateRecord0, accountRecord.UserEntry.Password, accountRecord.UserEntry.UserName, sdt, accountRecord.ErrorCount, accountRecord.ActionTick, accountRecord.UserEntry.SSNo, accountRecord.UserEntryAdd.BirthDay, accountRecord.UserEntry.Phone, accountRecord.UserEntryAdd.MobilePhone, accountRecord.UserEntry.EMail, accountRecord.UserEntry.Quiz, accountRecord.UserEntry.Answer, accountRecord.UserEntryAdd.Quiz2, accountRecord.UserEntryAdd.Answer2, accountRecord.UserEntry.Account);
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (Exception E)
                        {
                            result = 0;
                            _logger.LogError("[Exception] TFileIDDB.UpdateRecord");
                            _logger.LogError(E);
                            return result;
                        }
                        break;
                }
            }
            finally
            {
                Close(ref dbConnection);
            }
            return result;
        }

        public bool Update(int nIndex, ref AccountRecord accountRecord)
        {
            bool result = false;
            if (nIndex < 0)
            {
                return false;
            }
            if (_quickList.Count <= nIndex)
            {
                return false;
            }
            if (UpdateRecord(accountRecord, 0) > 0)
            {
                result = true;
            }
            return result;
        }

        public bool Add(ref AccountRecord accountRecord)
        {
            bool result;
            var sAccount = accountRecord.UserEntry.Account;
            if (Index(sAccount) > 0)
            {
                result = false;
            }
            else
            {
                var nIndex = UpdateRecord(accountRecord, 1);
                if (nIndex > 0)
                {
                    _quickList.Add(new AccountQuick(sAccount, nIndex));
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        public bool Delete(int nIndex, ref AccountRecord accountRecord)
        {
            var result = false;
            if (nIndex < 0)
            {
                return false;
            }
            if (_quickList.Count <= nIndex)
            {
                return false;
            }
            var up = UpdateRecord(accountRecord, 2);
            if (up > 0)
            {
                _quickList.RemoveAt(nIndex);
                result = true;
            }
            return result;
        }
    }
}