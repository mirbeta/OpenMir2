using LoginSvr.Conf;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SystemModule;
using SystemModule.Extensions;
using SystemModule.Packet.ClientPackets;

namespace LoginSvr.DB
{
    public class AccountDB
    {
        private readonly MirLog _logger;
        private readonly ConfigManager _configManager;
        private readonly IList<AccountQuick> _quickList = null;

        public AccountDB(MirLog logQueue, ConfigManager configManager)
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
            catch (Exception E)
            {
                _logger.Information("[警告] SQL 连接失败!请检查SQL设置...");
                _logger.Information(Config.ConnctionString);
                _logger.Information(E.StackTrace);
            }
        }

        public bool Open(ref MySqlConnection dbConnection)
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
                        _logger.Information("打开数据库[MySql]失败.");
                        _logger.LogError(e);
                        result = false;
                    }
                    break;
            }
            return result;
        }

        public void Close(ref MySqlConnection dbConnection)
        {
            if (dbConnection != null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }
        }

        private void LoadQuickList()
        {
            int nIndex = 0;
            bool boDeleted;
            string sAccount;
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
                command.Connection = (MySqlConnection) dbConnection;
                using var dr = command.ExecuteReader();
                while (dr.Read())
                {
                    nIndex = dr.GetInt32("Id");
                    boDeleted = dr.GetBoolean("DELETED");
                    sAccount = dr.GetString("LOGINID");
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
                System.Console.WriteLine(ex.Message);
            }
            finally
            {
                Close(ref dbConnection);
            }
            //m_QuickList.SortString(0, m_QuickList.Count - 1);
        }

        public int FindByName(string sName, ref IList<AccountQuick> List)
        {
            int result;
            for (var i = 0; i < _quickList.Count; i++)
            {
                if (HUtil32.CompareLStr(_quickList[i].sAccount, sName))
                {
                    List.Add(new AccountQuick(_quickList[i].sAccount, _quickList[i].nIndex));
                }
            }
            result = List.Count;
            return result;
        }

        public bool GetBy(int nIndex, ref TAccountDBRecord DBRecord)
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

        private bool GetRecord(int nIndex, ref TAccountDBRecord DBRecord)
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
            command.Connection = (MySqlConnection)dbConnection;
            IDataReader dr;
            try
            {
                dr = command.ExecuteReader();
                if (DBRecord == null)
                {
                    DBRecord = new TAccountDBRecord();
                    DBRecord.Header = new TRecordHeader();
                    DBRecord.UserEntry = new UserEntry();
                    DBRecord.UserEntryAdd = new UserEntryAdd();
                }
                if (dr.Read())
                {
                    DBRecord.Header.sAccount = dr.GetString("LOGINID");
                    DBRecord.Header.boDeleted = dr.GetBoolean(dr.GetOrdinal("DELETED"));
                    DBRecord.Header.CreateDate = dr.GetDateTime("CREATEDATE");
                    DBRecord.Header.UpdateDate = dr.GetDateTime("LASTUPDATE");
                    DBRecord.nErrorCount = dr.GetInt32("ERRORCOUNT");
                    DBRecord.dwActionTick = dr.GetInt32("ACTIONTICK");
                    DBRecord.UserEntry.sAccount = dr.GetString("LOGINID");
                    DBRecord.UserEntry.sPassword = dr.GetString("PASSWORD");
                    DBRecord.UserEntry.sUserName = dr.GetString("USERNAME");
                    DBRecord.UserEntry.sSSNo = dr.GetString("SSNO");
                    DBRecord.UserEntry.sPhone = dr.GetString("PHONE");
                    DBRecord.UserEntry.sQuiz = dr.GetString("QUIZ1");
                    DBRecord.UserEntry.sAnswer = dr.GetString("ANSWER1");
                    DBRecord.UserEntry.sEMail = dr.GetString("EMAIL");
                    DBRecord.UserEntryAdd.sQuiz2 = dr.GetString("QUIZ2");
                    DBRecord.UserEntryAdd.sAnswer2 = dr.GetString("ANSWER2");
                    DBRecord.UserEntryAdd.sBirthDay = dr.GetString("BIRTHDAY");
                    DBRecord.UserEntryAdd.sMobilePhone = dr.GetString("MOBILEPHONE");
                    DBRecord.UserEntryAdd.sMemo = "";
                    DBRecord.UserEntryAdd.sMemo2 = "";
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
            }
            catch
            {
                result = false;
                _logger.Information("[Exception] TFileIDDB.GetRecord (1)");
                return result;
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

        public int Get(int nIndex, ref TAccountDBRecord DBRecord)
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
            if (GetRecord(nIndex, ref DBRecord))
            {
                result = nIndex;
            }
            return result;
        }

        private int UpdateRecord(TAccountDBRecord DBRecord, byte btFlag)
        {
            var result = 0;
            string sdt = "now()";
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
                command.Connection = (MySqlConnection)dbConnection;
                switch (btFlag)
                {
                    case 1:
                        command.CommandText = string.Format(sUpdateRecord1, new object[] { DBRecord.UserEntry.sAccount, DBRecord.UserEntry.sPassword, DBRecord.UserEntry.sUserName, sdt, sdt, DBRecord.UserEntry.sSSNo, DBRecord.UserEntryAdd.sBirthDay, DBRecord.UserEntry.sPhone, DBRecord.UserEntryAdd.sMobilePhone, DBRecord.UserEntry.sEMail, DBRecord.UserEntry.sQuiz, DBRecord.UserEntry.sAnswer, DBRecord.UserEntryAdd.sQuiz2, DBRecord.UserEntryAdd.sAnswer2 });
                        try
                        {
                            command.ExecuteNonQuery();
                            result = (int)command.LastInsertedId;
                        }
                        catch (Exception E)
                        {
                            _logger.Information("[Exception] TFileIDDB.UpdateRecord");
                            _logger.Information(E.Message);
                            return 0;
                        }
                        break;
                    case 2:
                        command.CommandText = string.Format(sUpdateRecord2, sdt, DBRecord.UserEntry.sAccount);
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            result = 0;
                            _logger.Information("[Exception] TFileIDDB.UpdateRecord (3)");
                        }
                        break;
                    default:
                        command.CommandText = string.Format(sUpdateRecord0, new object[] { DBRecord.UserEntry.sPassword, DBRecord.UserEntry.sUserName, sdt, DBRecord.nErrorCount, DBRecord.dwActionTick, DBRecord.UserEntry.sSSNo, DBRecord.UserEntryAdd.sBirthDay, DBRecord.UserEntry.sPhone, DBRecord.UserEntryAdd.sMobilePhone, DBRecord.UserEntry.sEMail, DBRecord.UserEntry.sQuiz, DBRecord.UserEntry.sAnswer, DBRecord.UserEntryAdd.sQuiz2, DBRecord.UserEntryAdd.sAnswer2, DBRecord.UserEntry.sAccount });
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (Exception E)
                        {
                            result = 0;
                            _logger.Information("[Exception] TFileIDDB.UpdateRecord (0)");
                            _logger.Information(E.Message);
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

        public bool Update(int nIndex, ref TAccountDBRecord DBRecord)
        {
            bool result = false;
            if (nIndex < 0)
            {
                return result;
            }
            if (_quickList.Count <= nIndex)
            {
                return result;
            }
            if (UpdateRecord(DBRecord, 0) > 0)
            {
                result = true;
            }
            return result;
        }

        public bool Add(ref TAccountDBRecord DBRecord)
        {
            bool result;
            var sAccount = DBRecord.UserEntry.sAccount;
            if (Index(sAccount) > 0)
            {
                result = false;
            }
            else
            {
                var nIndex = UpdateRecord(DBRecord, 1);
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

        public bool Delete(int nIndex, ref TAccountDBRecord DBRecord)
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
            var up = UpdateRecord(DBRecord, 2);
            if (up > 0)
            {
                _quickList.RemoveAt(nIndex);
                result = true;
            }
            return result;
        }

    }
}