using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SystemModule;

namespace LoginSvr
{
    public class AccountDB
    {
        private IDbConnection ADOConnection = null;
        private IList<AccountQuick> m_QuickList = null;
        private int nRecordCount = 0;

        public AccountDB()
        {
            m_QuickList = new List<AccountQuick>();
            nRecordCount = -1;
            LSShare.g_boDataDBReady = false;
        }

        public void Initialization()
        {
            LSShare.MainOutMessage("0) 正在连接SQL服务器...");
            ADOConnection = new MySqlConnection(LSShare.DBConnection);
            try
            {
                ADOConnection.Open();
                LSShare.MainOutMessage("1) 连接SQL服务器成功...");
                LoadQuickList();
            }
            catch (Exception E)
            {
                LSShare.MainOutMessage("[警告] SQL 连接失败！请检查SQL设置...");
                LSShare.MainOutMessage(LSShare.DBConnection);
                LSShare.MainOutMessage(E.Message);
            }
        }

        private IDbConnection GetConnection()
        {
            return new MySqlConnection(LSShare.DBConnection);
        }

        public bool OpenEx()
        {
            return Open();
        }

        public bool Open()
        {
            __Lock();
            return true;
        }

        public void Close()
        {
            UnLock();
        }

        private void LoadQuickList()
        {
            int nIndex = 0;
            bool boDeleted;
            string sAccount;
            const string sSQL = "SELECT FLD_DELETED，FLD_LOGINID FROM TBL_ACCOUNT";
            nRecordCount = -1;
            m_QuickList.Clear();
            __Lock();
            try
            {
                using var conn = GetConnection();
                conn.Open();
                var command = new MySqlCommand();
                command.CommandText = sSQL;
                command.Connection = (MySqlConnection)conn;
                var dr = command.ExecuteReader();
                while (dr.Read())
                {
                    boDeleted = dr.GetBoolean("FLD_DELETED");
                    sAccount = dr.GetString("FLD_LOGINID");
                    nIndex++;
                    if (!boDeleted && (sAccount != ""))
                    {
                        m_QuickList.Add(new AccountQuick(sAccount, nIndex));
                    }
                }
                nRecordCount = nIndex;
                dr.Close();
                dr.Dispose();
            }
            finally
            {
                UnLock();
            }
            //m_QuickList.SortString(0, m_QuickList.Count - 1);
            LSShare.g_boDataDBReady = true;
        }

        public void __Lock()
        {
           // EnterCriticalSection(FCriticalSection);
        }

        public void UnLock()
        {
           // LeaveCriticalSection(FCriticalSection);
        }

        public int FindByName(string sName, ref IList<AccountQuick> List)
        {
            int result;
            for (var i = 0; i < m_QuickList.Count; i++)
            {
                if (HUtil32.CompareLStr(m_QuickList[i].sAccount, sName, sName.Length))
                {
                    List.Add(new AccountQuick(m_QuickList[i].sAccount, m_QuickList[i].nIndex));
                }
            }
            result = List.Count;
            return result;
        }

        public bool GetBy(int nIndex, ref TAccountDBRecord DBRecord)
        {
            bool result;
            if ((nIndex >= 0) && (m_QuickList.Count > nIndex))
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
            const string sSQL = "SELECT * FROM TBL_ACCOUNT WHERE FLD_LOGINID='{0}'";
            var result = true;
            string sAccount = m_QuickList[nIndex].sAccount;
            using var conn = GetConnection();
            conn.Open();
            var command = new MySqlCommand();
            command.CommandText = string.Format(sSQL, sAccount);
            command.Connection = (MySqlConnection)conn;
            IDataReader dr;
            try
            {
                dr = command.ExecuteReader();
            }
            catch
            {
                result = false;
                LSShare.MainOutMessage("[Exception] TFileIDDB.GetRecord (1)");
                return result;
            }
            if (DBRecord == null)
            {
                DBRecord = new TAccountDBRecord();
                DBRecord.Header = new TRecordHeader();
                DBRecord.UserEntry = new TUserEntry();
                DBRecord.UserEntryAdd = new TUserEntryAdd();
            }
            if (dr.Read())
            {
                DBRecord.Header.sAccount = dr.GetString("FLD_LOGINID");
                DBRecord.Header.boDeleted = dr.GetBoolean(dr.GetOrdinal("FLD_DELETED"));
                DBRecord.Header.CreateDate = dr.GetDateTime("FLD_CREATEDATE");
                DBRecord.Header.UpdateDate = dr.GetDateTime("FLD_LASTUPDATE");
                DBRecord.nErrorCount = dr.GetInt32("FLD_ERRORCOUNT");
                DBRecord.dwActionTick = dr.GetInt32("FLD_ACTIONTICK");
                DBRecord.UserEntry.sAccount = dr.GetString("FLD_LOGINID");
                DBRecord.UserEntry.sPassword = dr.GetString("FLD_PASSWORD");
                DBRecord.UserEntry.sUserName = dr.GetString("FLD_USERNAME");
                DBRecord.UserEntry.sSSNo = dr.GetString("FLD_SSNO");
                DBRecord.UserEntry.sPhone = dr.GetString("FLD_PHONE");
                DBRecord.UserEntry.sQuiz = dr.GetString("FLD_QUIZ1");
                DBRecord.UserEntry.sAnswer = dr.GetString("FLD_ANSWER1");
                DBRecord.UserEntry.sEMail = dr.GetString("FLD_EMAIL");
                DBRecord.UserEntryAdd.sQuiz2 = dr.GetString("FLD_QUIZ2");
                DBRecord.UserEntryAdd.sAnswer2 = dr.GetString("FLD_ANSWER2");
                DBRecord.UserEntryAdd.sBirthDay = dr.GetString("FLD_BIRTHDAY");
                DBRecord.UserEntryAdd.sMobilePhone = dr.GetString("FLD_MOBILEPHONE");
                DBRecord.UserEntryAdd.sMemo = "";
                DBRecord.UserEntryAdd.sMemo2 = "";
            }
            return result;
        }

        public int Index(string sName)
        {
            var quick = m_QuickList.SingleOrDefault(o => o.sAccount == sName);
            if (quick == null)
            {
                return 0;
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
            if (m_QuickList.Count <= nIndex)
            {
                return result;
            }
            if (GetRecord(nIndex, ref DBRecord))
            {
                result = nIndex;
            }
            return result;
        }

        private bool UpdateRecord(int nIndex, TAccountDBRecord DBRecord, byte btFlag)
        {
            bool result = true;
            string sdt = "now()";
            const string sUpdateRecord1 = "INSERT INTO TBL_ACCOUNT (FLD_LOGINID, FLD_PASSWORD, FLD_USERNAME, FLD_CREATEDATE, FLD_LASTUPDATE, FLD_DELETED, FLD_ERRORCOUNT, FLD_ACTIONTICK, FLD_SSNO, FLD_BIRTHDAY, FLD_PHONE, FLD_MOBILEPHONE, FLD_EMAIL, FLD_QUIZ1, FLD_ANSWER1, FLD_QUIZ2, FLD_ANSWER2) VALUES('{0}', '{1}', '{2}', {3}, {4}, 0, 0, 0,'{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}')";
            const string sUpdateRecord2 = "UPDATE TBL_ACCOUNT SET FLD_DELETED=1, FLD_CREATEDATE='{0}' WHERE FLD_LOGINID='{1}'";
            const string sUpdateRecord0 = "UPDATE TBL_ACCOUNT SET FLD_PASSWORD='{0}', FLD_USERNAME='{1}',FLD_LASTUPDATE={2}, FLD_ERRORCOUNT={3}, FLD_ACTIONTICK={4},FLD_SSNO='{5}', FLD_BIRTHDAY='{6}', FLD_PHONE='{7}',FLD_MOBILEPHONE='{8}', FLD_EMAIL='{9}', FLD_QUIZ1='{10}', FLD_ANSWER1='{11}', FLD_QUIZ2='{12}',FLD_ANSWER2='{13}' WHERE FLD_LOGINID='{14}'";
            try
            {
                using var conn = GetConnection();
                conn.Open();
                var command = new MySqlCommand();
                command.Connection = (MySqlConnection)conn;
                switch (btFlag)
                {
                    case 1:
                        command.CommandText = string.Format(sUpdateRecord1, new object[] { DBRecord.UserEntry.sAccount, DBRecord.UserEntry.sPassword, DBRecord.UserEntry.sUserName, sdt, sdt, DBRecord.UserEntry.sSSNo, DBRecord.UserEntryAdd.sBirthDay, DBRecord.UserEntry.sPhone, DBRecord.UserEntryAdd.sMobilePhone, DBRecord.UserEntry.sEMail, DBRecord.UserEntry.sQuiz, DBRecord.UserEntry.sAnswer, DBRecord.UserEntryAdd.sQuiz2, DBRecord.UserEntryAdd.sAnswer2 });
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (Exception E)
                        {
                            result = false;
                            LSShare.MainOutMessage("[Exception] TFileIDDB.UpdateRecord");
                            LSShare.MainOutMessage(E.Message);
                            return result;
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
                            result = false;
                            LSShare.MainOutMessage("[Exception] TFileIDDB.UpdateRecord (3)");
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
                            result = false;
                            LSShare.MainOutMessage("[Exception] TFileIDDB.UpdateRecord (0)");
                            LSShare.MainOutMessage(E.Message);
                            return result;
                        }
                        break;
                }
            }
            finally
            {

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
            if (m_QuickList.Count <= nIndex)
            {
                return result;
            }
            if (UpdateRecord(nIndex, DBRecord, 0))
            {
                result = true;
            }
            return result;
        }

        public bool Add(ref TAccountDBRecord DBRecord)
        {
            bool result;
            int nIndex;
            var sAccount = DBRecord.UserEntry.sAccount;
            if (Index(sAccount) > 0)
            {
                result = false;
            }
            else
            {
                nIndex = nRecordCount;
                nRecordCount++;
                if (UpdateRecord(nIndex, DBRecord, 1))
                {
                    m_QuickList.Add(new AccountQuick(sAccount, nIndex));
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
            bool result = false;
            if (nIndex < 0)
            {
                return result;
            }
            if (m_QuickList.Count <= nIndex)
            {
                return result;
            }
            if (UpdateRecord(nIndex, DBRecord, 2))
            {
                m_QuickList.RemoveAt(nIndex);
                result = true;
            }
            return result;
        }

    }
}
