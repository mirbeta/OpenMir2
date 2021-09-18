using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using SystemModule;

namespace DBSvr
{
    public class THumDB
    {
        public bool m_boChanged = false;
        public IList<string> m_MirQuickList = null;
        public IList<string> m_MirQuickIDList = null;
        private Dictionary<int, string> m_QuickIndexNameList = null;
        public int m_nRecordCount = 0;

        public THumDB(string sFileName)
        {
            DBShare.boDataDBReady = false;
            //m_MirQuickList = new TQuickList();
            //m_MirQuickIDList = new TQuickList();
            DBShare.n4ADAE4 = 0;
            DBShare.n4ADAF0 = 0;
            m_nRecordCount = -1;
            m_QuickIndexNameList = new Dictionary<int, string>();
            //if (Units.HumDB_SQL.g_boSQLIsReady)
            //{
                LoadQuickList();
            //}
        }

        private IDbConnection GetConnection()
        {
            return new MySqlConnection(DBShare.DBConnection);
        }

        private void LoadQuickList()
        {
            int nIndex;
            bool boDeleted;
            ArrayList AccountList;
            ArrayList ChrNameList;
            string sAccount;
            string sChrName;
            const string sSQL = "SELECT * FROM TBL_CHARACTER";
            //m_MirQuickList.Clear();
            //m_MirQuickIDList.Clear();
            DBShare.n4ADAE4 = 0;
            DBShare.n4ADAE8 = 0;
            DBShare.n4ADAF0 = 0;
            m_nRecordCount = -1;
            AccountList = new ArrayList();
            ChrNameList = new ArrayList();
            __Lock();
            try
            {
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
                        DBShare.n4ADAF0 = m_nRecordCount;
                        for (nIndex = 0; nIndex < m_nRecordCount; nIndex++)
                        {
                            DBShare.n4ADAE4++;
                            boDeleted = dr.GetBoolean("FLD_DELETED");
                            sAccount = dr.GetString("FLD_LOGINID");
                            sChrName = dr.GetString("FLD_CHARNAME");
                            if (!boDeleted && (sChrName != ""))
                            {
                                //m_MirQuickList.Add(sChrName, nIndex);
                                //AccountList.Add(sAccount, nIndex);
                                //ChrNameList.Add(sChrName, nIndex);
                                DBShare.n4ADAE8++;
                            }
                            else
                            {
                                DBShare.n4ADAEC++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DBShare.MainOutMessage("[Exception] TFileDB.LoadQuickList");
                }
                finally
                {

                }
            }
            finally
            {
                Close();
            }
            for (nIndex = 0; nIndex < AccountList.Count; nIndex++)
            {
                //m_MirQuickIDList.AddRecord(AccountList[nIndex], ChrNameList[nIndex], ((int)AccountList.Values[nIndex]));
                //m_QuickIndexNameList.Add(nIndex, ChrNameList[nIndex]);
            }
            AccountList = null;
            ChrNameList = null;
            //m_MirQuickList.SortString(0, m_MirQuickList.Count - 1);
            DBShare.boDataDBReady = true;
        }

        public void __Lock()
        {
        }

        public void UnLock()
        {
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

        public bool OpenEx()
        {
            bool result;
            result = Open();
            return result;
        }

        public int Index(string sName)
        {
            return 0;
            //return m_MirQuickList.GetIndex(sName);
        }

        public int Get(int nIndex, ref THumDataInfo HumanRCD)
        {
            int result = -1;
            int nIdx;
            if (nIndex < 0)
            {
                return result;
            }
            if (m_MirQuickList.Count <= nIndex)
            {
                return result;
            }
            if (GetRecord(nIndex, ref HumanRCD))
            {
                result = nIndex;
            }
            return result;
        }

        public bool Update(int nIndex, ref THumDataInfo HumanRCD)
        {
            bool result = false;
            if ((nIndex >= 0) && (m_MirQuickList.Count > nIndex))
            {
                if (UpdateRecord(nIndex, ref HumanRCD, false))
                {
                    result = true;
                }
            }
            return result;
        }

        public bool UpdateQryChar(int nIndex, ref TQueryChr QueryChrRcd)
        {
            bool result = false;
            if ((nIndex >= 0) && (m_MirQuickList.Count > nIndex))
            {
                if (UpdateChrRecord(nIndex, ref QueryChrRcd, false))
                {
                    result = true;
                }
            }
            return result;
        }

        private bool UpdateChrRecord(int nIndex, ref TQueryChr QueryChrRcd, bool boNew)
        {
            bool result = true;
            try
            {
                using var conn = GetConnection();
                conn.Open();
                var command = new MySqlCommand();
                command.CommandText = string.Format("UPDATE TBL_CHARACTER SET FLD_SEX={0}, FLD_JOB={1} WHERE FLD_CHARNAME='{2}'", new object[] { QueryChrRcd.btSex, QueryChrRcd.btJob, QueryChrRcd.sName });
                command.Connection = (MySqlConnection)conn;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    result = false;
                    DBShare.MainOutMessage("[Exception] TFileDB.UpdateRecord (1)");
                    return result;
                }
                m_boChanged = true;
            }
            finally
            {
            }
            return result;
        }

        public bool Add(ref THumDataInfo HumanRCD)
        {
            bool result = false;
            int nIndex;
            string sChrName = HumanRCD.Header.sName;
            //if (m_MirQuickList.GetIndex(sChrName) >= 0)
            //{
            //    result = false;
            //}
            //else
            //{
            //    nIndex = m_nRecordCount;
            //    m_nRecordCount++;
            //    if (UpdateRecord(nIndex, ref HumanRCD, true))
            //    {
            //        m_MirQuickList.AddRecord(sChrName, nIndex);
            //        result = true;
            //    }
            //    else
            //    {
            //        result = false;
            //    }
            //}
            return result;
        }

        private bool GetRecord(int nIndex, ref THumDataInfo HumanRCD)
        {
            bool result;
            string sChrName;
            string sTmp;
            string str;
            int i;
            int ii;
            int nCount;
            int nPosition;
            //TBlob Blob;
            int dw;
            const string sSQL1 = "SELECT * FROM TBL_CHARACTER WHERE FLD_CHARNAME='{0}'";
            const string sSQL2 = "SELECT * FROM TBL_BONUSABILITY WHERE FLD_CHARNAME='{0}'";
            const string sSQL3 = "SELECT * FROM TBL_QUEST WHERE FLD_CHARNAME='{0}'";
            const string sSQL4 = "SELECT * FROM TBL_MAGIC WHERE FLD_CHARNAME='{0}'";
            const string sSQL5 = "SELECT * FROM TBL_ITEM WHERE FLD_CHARNAME='{0}'";
            const string sSQL6 = "SELECT * FROM TBL_STORAGE WHERE FLD_CHARNAME='{0}'";
            const string sSQL7 = "SELECT * FROM TBL_ADDON WHERE FLD_CHARNAME='{0}'";
            result = true;
            sChrName = m_MirQuickList[nIndex];
            try
            {
                var command = new MySqlCommand();
                try
                {
                    using var conn = GetConnection();
                    conn.Open();
                    command.CommandText = string.Format(sSQL1, sChrName);
                    command.Connection = (MySqlConnection)conn;
                }
                catch (Exception)
                {
                    DBShare.MainOutMessage("[Exception] TFileDB.GetRecord (1)");
                    return false;
                }
                var dr = command.ExecuteReader();
                while (dr.Read())
                {
                    HumanRCD.Header.sName = dr.GetString("FLD_CHARNAME");
                    HumanRCD.Header.boDeleted = dr.GetBoolean("FLD_DELETED");
                    HumanRCD.Header.dCreateDate = HUtil32.DateTimeToDouble(dr.GetDateTime("FLD_CREATEDATE"));
                    HumanRCD.Data.sChrName = dr.GetString("FLD_CHARNAME");
                    HumanRCD.Data.sCurMap = dr.GetString("FLD_MAPNAME");
                    HumanRCD.Data.wCurX = dr.GetInt16("FLD_CX");
                    HumanRCD.Data.wCurY = dr.GetInt16("FLD_CY");
                    HumanRCD.Data.btDir = dr.GetByte("FLD_DIR");
                    HumanRCD.Data.btHair = dr.GetByte("FLD_HAIR");
                    HumanRCD.Data.btSex = dr.GetByte("FLD_SEX");
                    HumanRCD.Data.btJob = dr.GetByte("FLD_JOB");
                    HumanRCD.Data.nGold = dr.GetInt32("FLD_GOLD");
                    // TAbility
                    HumanRCD.Data.Abil.Level = dr.GetUInt16("FLD_LEVEL");
                    dw = dr.GetInt32("FLD_HP");
                    HumanRCD.Data.Abil.HP = HUtil32.LoWord(dw);
                    HumanRCD.Data.Abil.AC = HUtil32.HiWord(dw);
                    dw = dr.GetInt32("FLD_MP");
                    HumanRCD.Data.Abil.MP = HUtil32.LoWord(dw);
                    HumanRCD.Data.Abil.MAC = HUtil32.HiWord(dw);
                    HumanRCD.Data.Abil.Exp = dr.GetInt32("FLD_EXP");
                    HumanRCD.Data.sHomeMap = dr.GetString("FLD_HOMEMAP");
                    HumanRCD.Data.wHomeX = dr.GetInt16("FLD_HOMECX");
                    HumanRCD.Data.wHomeY = dr.GetInt16("FLD_HOMECY");
                    HumanRCD.Data.sDearName = dr.GetString("FLD_DEARCHARNAME");
                    HumanRCD.Data.sMasterName = dr.GetString("FLD_MASTERCHARNAME");
                    HumanRCD.Data.boMaster = dr.GetBoolean("FLD_MASTER");
                    HumanRCD.Data.btCreditPoint = dr.GetByte("FLD_CREDITPOINT");
                    //HumanRCD.Data.btInPowerLevel = dr.GetInt32("FLD_IPLEVEL");
                    // word
                    HumanRCD.Data.sStoragePwd = dr.GetString("FLD_STORAGEPASSWD");
                    HumanRCD.Data.btReLevel = dr.GetByte("FLD_REBIRTHLEVEL");
                    HumanRCD.Data.boLockLogon = dr.GetBoolean("FLD_LOCKLOGON");
                    //HumanRCD.Data.wInPowerPoint = dr.GetInt32("FLD_IPPOINT");
                    // word
                    // TNakedAbility
                    HumanRCD.Data.nBonusPoint = dr.GetInt32("FLD_BONUSPOINT");
                    HumanRCD.Data.nGameGold = dr.GetInt32("FLD_GAMEGOLD");
                    HumanRCD.Data.nGamePoint = dr.GetInt32("FLD_GAMEPOINT");
                    HumanRCD.Data.nPayMentPoint = dr.GetInt32("FLD_PAYPOINT");
                    HumanRCD.Data.nHungerStatus = dr.GetInt32("FLD_HUNGRYSTATE");
                    //HumanRCD.Data.nPKPOINT = dr.GetInt32("FLD_PKPOINT");
                    HumanRCD.Data.btAllowGroup = dr.GetBoolean("FLD_ALLOWPARTY") == true ? (byte)1 : (byte)0;
                    //HumanRCD.Data.btClPkPoint = dr.GetInt32("FLD_FREEGULITYCOUNT");
                    HumanRCD.Data.btAttatckMode = dr.GetByte("FLD_ATTACKMODE");
                    HumanRCD.Data.btIncHealth = dr.GetByte("FLD_INCHEALTH");
                    HumanRCD.Data.btIncSpell = dr.GetByte("FLD_INCSPELL");
                    HumanRCD.Data.btIncHealing = dr.GetByte("FLD_INCHEALING");
                    HumanRCD.Data.btFightZoneDieCount = dr.GetByte("FLD_FIGHTZONEDIE");
                    HumanRCD.Data.sAccount = dr.GetString("FLD_LOGINID");
                    //HumanRCD.Data.btNewHuman = dr.GetInt32("FLD_TESTSERVERRESETCOUNT");
                    //HumanRCD.Data.dwInPowerExp = dr.GetInt32("FLD_IPEXP");
                    //HumanRCD.Data.dwGatherNimbus = dr.GetInt32("FLD_NIMBUSPOINT");
                    //HumanRCD.Data.btAttribute = dr.GetInt32("FLD_NATUREELEMENT");
                    HumanRCD.Data.boAllowGuildReCall = dr.GetBoolean("FLD_ENABLEGRECALL");
                    HumanRCD.Data.boAllowGroupReCall = dr.GetBoolean("FLD_ENABLEGROUPRECALL");
                    //HumanRCD.Data.nKillMonExpRate = dr.GetInt32("FLD_GAINEXPRATE");
                    //HumanRCD.Data.dwKillMonExpRateTime = dr.GetInt32("FLD_GAINEXPRATETIME");
                    //HumanRCD.Data.sHeroName = dr.GetString("FLD_HERONAME");
                    //HumanRCD.Data.sHeroMasterName = dr.GetString("FLD_HEROMASTERNAME");
                    //HumanRCD.Data.btOptnYBDeal = dr.GetInt32("FLD_OPENGAMEGOLDDEAL");
                    HumanRCD.Data.wGroupRcallTime = dr.GetInt16("FLD_GROUPRECALLTIME");
                    HumanRCD.Data.dBodyLuck = dr.GetDouble("FLD_BODYLUCK");
                    //HumanRCD.Data.sMarkerMap = dr.GetString("FLD_MARKMAP");
                    //HumanRCD.Data.wMarkerX = dr.GetInt32("FLD_MARKMAPX");
                    //HumanRCD.Data.wMarkerY = dr.GetInt32("FLD_MARKMAPY");
                }
                try
                {
                    using var conn = GetConnection();
                    conn.Open();
                    command.CommandText = string.Format(sSQL2, sChrName);
                    command.Connection = (MySqlConnection)conn;
                }
                catch (Exception)
                {
                    DBShare.MainOutMessage("[Exception] TFileDB.GetRecord (2)");
                    return false;
                }
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    HumanRCD.Data.BonusAbil.AC = dr.GetUInt16("FLD_AC");
                    HumanRCD.Data.BonusAbil.MAC = dr.GetUInt16("FLD_MAC");
                    HumanRCD.Data.BonusAbil.DC = dr.GetUInt16("FLD_DC");
                    HumanRCD.Data.BonusAbil.MC = dr.GetUInt16("FLD_MC");
                    HumanRCD.Data.BonusAbil.SC = dr.GetUInt16("FLD_SC");
                    HumanRCD.Data.BonusAbil.HP = dr.GetUInt16("FLD_HP");
                    HumanRCD.Data.BonusAbil.MP = dr.GetUInt16("FLD_MP");
                    HumanRCD.Data.BonusAbil.Hit = dr.GetByte("FLD_HIT");
                    HumanRCD.Data.BonusAbil.Speed = dr.GetInt32("FLD_SPEED");
                    HumanRCD.Data.BonusAbil.X2 = dr.GetByte("FLD_RESERVED");
                }
                try
                {
                    using var conn = GetConnection();
                    conn.Open();
                    command.CommandText = string.Format(sSQL3, sChrName);
                    command.Connection = (MySqlConnection)conn;
                }
                catch (Exception)
                {
                    DBShare.MainOutMessage("[Exception] TFileDB.GetRecord (3)");
                    return false;
                }
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    //sTmp = dr.GetString("FLD_QUESTOPENINDEX").AsString.Trim();
                    //if (sTmp != "")
                    //{
                    //    EDcode.Decode6BitBuf((sTmp as string), HumanRCD.Data.QuestUnitOpen, sTmp.Length, sizeof(HumanRCD.Data.QuestUnitOpen));
                    //}
                    //sTmp = dr.GetString("FLD_QUESTFININDEX").AsString;
                    //if (sTmp != "")
                    //{
                    //    EDcode.Decode6BitBuf((sTmp as string), HumanRCD.Data.QuestUnit, sTmp.Length, sizeof(HumanRCD.Data.QuestUnit));
                    //}
                    //sTmp = dr.GetString("FLD_QUEST").AsString;
                    //if (sTmp != "")
                    //{
                    //    EDcode.Decode6BitBuf((sTmp as string), HumanRCD.Data.QuestFlag, sTmp.Length, sizeof(HumanRCD.Data.QuestFlag));
                    //}
                }
                try
                {
                    using var conn = GetConnection();
                    conn.Open();
                    command.CommandText = string.Format(sSQL4, sChrName);
                    command.Connection = (MySqlConnection)conn;
                }
                catch (Exception)
                {
                    DBShare.MainOutMessage("[Exception] TFileDB.GetRecord (4)");
                    return false;
                }
                dr = command.ExecuteReader();
                var magicList = new List<TMagicRcd>();
                while (dr.Read())
                {
                    magicList.Add(new TMagicRcd()
                    {
                        wMagIdx = dr.GetUInt16("FLD_MAGICID"),
                        btKey = dr.GetByte("FLD_USEKEY"),
                        btLevel = dr.GetByte("FLD_LEVEL"),
                        nTranPoint = dr.GetInt32("FLD_CURRTRAIN")
                    });
                }
                for (int j = 0; j < magicList.Count; j++)
                {
                    HumanRCD.Data.Magic[j] = magicList[j];
                }
                try
                {
                    using var conn = GetConnection();
                    conn.Open();
                    command.CommandText = string.Format(sSQL5, sChrName);
                    command.Connection = (MySqlConnection)conn;
                }
                catch (Exception)
                {
                    DBShare.MainOutMessage("[Exception] TFileDB.GetRecord (5)");
                    return false;
                }
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    //nPosition = dr.GetInt32("FLD_POSITION") - 1;
                    //if ((nPosition >= 0) && (nPosition <= High(THumItems)))
                    //{
                    //    HumanRCD.Data.HumItems[nPosition].MakeIndex = dr.GetInt32("FLD_MAKEINDEX");
                    //    HumanRCD.Data.HumItems[nPosition].wIndex = dr.GetInt32("FLD_STDINDEX");
                    //    HumanRCD.Data.HumItems[nPosition].Dura = dr.GetInt32("FLD_DURA");
                    //    HumanRCD.Data.HumItems[nPosition].DuraMax = dr.GetInt32("FLD_DURAMAX");
                    //    for (ii = HumanRCD.Data.HumItems[nPosition].btValue.GetLowerBound(0); ii <= HumanRCD.Data.HumItems[nPosition].btValue.GetUpperBound(0); ii++)
                    //    {
                    //        HumanRCD.Data.HumItems[nPosition].btValue[ii] = dr.GetInt32(string.Format("FLD_VALUE{0}", ii}));
                    //    }
                    //}
                    //else
                    //{
                    //    HumanRCD.Data.BagItems[i].MakeIndex = dr.GetInt32("FLD_MAKEINDEX");
                    //    HumanRCD.Data.BagItems[i].wIndex = dr.GetInt32("FLD_STDINDEX");
                    //    HumanRCD.Data.BagItems[i].Dura = dr.GetInt32("FLD_DURA");
                    //    HumanRCD.Data.BagItems[i].DuraMax = dr.GetInt32("FLD_DURAMAX");
                    //    for (ii = HumanRCD.Data.HumItems[i].btValue.GetLowerBound(0); ii <= HumanRCD.Data.HumItems[i].btValue.GetUpperBound(0); ii++)
                    //    {
                    //        HumanRCD.Data.HumItems[i].btValue[ii] = dr.GetInt32(string.Format("FLD_VALUE{0}", ii));
                    //    }
                    //}
                }
                try
                {
                    using var conn = GetConnection();
                    conn.Open();
                    command.CommandText = string.Format(sSQL6, sChrName);
                    command.Connection = (MySqlConnection)conn;
                }
                catch (Exception)
                {
                    DBShare.MainOutMessage("[Exception] TFileDB.GetRecord (6)");
                    return false;
                }
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    //HumanRCD.Data.StorageItems[i].MakeIndex = dr.GetInt32("FLD_MAKEINDEX");
                    //HumanRCD.Data.StorageItems[i].wIndex = dr.GetUInt16("FLD_STDINDEX");
                    //HumanRCD.Data.StorageItems[i].Dura = dr.GetUInt16("FLD_DURA");
                    //HumanRCD.Data.StorageItems[i].DuraMax = dr.GetUInt16("FLD_DURAMAX");
                    //for (ii = HumanRCD.Data.StorageItems[i].btValue.GetLowerBound(0); ii <= HumanRCD.Data.StorageItems[i].btValue.GetUpperBound(0); ii++)
                    //{
                    //    HumanRCD.Data.StorageItems[i].btValue[ii] = dr.GetByte(string.Format("FLD_VALUE{0}", ii));
                    //}
                }
                try
                {
                    using var conn = GetConnection();
                    conn.Open();
                    command.CommandText = string.Format(sSQL7, sChrName);
                    command.Connection = (MySqlConnection)conn;
                }
                catch (Exception)
                {
                    DBShare.MainOutMessage("[Exception] TFileDB.GetRecord (7)");
                    return false;
                }
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    // TStatusTime;
                    //sTmp = dr.GetString("FLD_STATUS").AsString;
                    //i = HumanRCD.Data.wStatusTimeArr.GetLowerBound(0);
                    //while (sTmp != "")
                    //{
                    //    sTmp = HUtil32.GetValidStr3(sTmp, ref str, new string[] { "/" });
                    //    HumanRCD.Data.wStatusTimeArr[i] = Convert.ToInt32(str);
                    //    i++;
                    //    if (i > HumanRCD.Data.wStatusTimeArr.GetUpperBound(0))
                    //    {
                    //        break;
                    //    }
                    //}
                    //// TSeriesSkillArr;
                    //sTmp = dr.GetString("FLD_SERIESSKILLORDER").AsString;
                    //i = HumanRCD.Data.SeriesSkillArr.GetLowerBound(0);
                    //while (sTmp != "")
                    //{
                    //    sTmp = HUtil32.GetValidStr3(sTmp, ref str, new string[] { "/" });
                    //    HumanRCD.Data.SeriesSkillArr[i] = Convert.ToInt32(str);
                    //    i++;
                    //    if (i > HumanRCD.Data.SeriesSkillArr.GetUpperBound(0))
                    //    {
                    //        break;
                    //    }
                    //}
                    //sTmp = dr.GetString("FLD_MISSION").AsString;
                    //if (sTmp != "")
                    //{
                    //    EDcode.Decode6BitBuf((sTmp as string), HumanRCD.Data.MissionFlag[0], sTmp.Length, sizeof(HumanRCD.Data.MissionFlag));
                    //}
                    //sTmp = dr.GetString("FLD_VENATION").AsString;
                    //if (sTmp != "")
                    //{
                    //    EDcode.Decode6BitBuf((sTmp as string), HumanRCD.Data.VenationInfos, sTmp.Length, sizeof(HumanRCD.Data.VenationInfos));
                    //}
                }
                dr.Close();
                dr.Dispose();
            }
            finally
            {

            }
            return result;
        }

        private bool UpdateRecord(int nIndex, ref THumDataInfo HumanRCD, bool boNew)
        {
            bool result = true;
            string sdt;
            int i;
            string sTmp;
            string sTmp2;
            string sTmp3;
            THumInfoData hd = null;
            MemoryStream m;
            double dwHP;
            double dwMP;
            char[] TempBuf = new char[Convert.ToInt32(Grobal2.BUFFERSIZE - 1) + 1];
            const string sSqlStr = "INSERT INTO TBL_CHARACTER (FLD_CHARNAME, FLD_LOGINID, FLD_DELETED, FLD_CREATEDATE, FLD_MAPNAME,FLD_CX, FLD_CY, FLD_DIR, FLD_HAIR, FLD_SEX, FLD_JOB, FLD_LEVEL, FLD_GOLD," + "FLD_HOMEMAP, FLD_HOMECX, FLD_HOMECY, FLD_PKPOINT, FLD_ATTACKMODE, FLD_FIGHTZONEDIE," + "FLD_BODYLUCK, FLD_INCHEALTH, FLD_INCSPELL, FLD_INCHEALING, FLD_BONUSPOINT," + "FLD_HUNGRYSTATE, FLD_TESTSERVERRESETCOUNT, FLD_ENABLEGRECALL) VALUES" + "( '%s', '%s', 0, GETDATE(), '," + "0, 0, 0, %d, %d, %d, 0, 0," + "', 0, 0, 0, 0, 0," + "0, 0, 0, 0, 0," + "0, 0, 0)";
            const string sSqlStr2 = "UPDATE TBL_CHARACTER SET FLD_DELETED=%d, FLD_CREATEDATE='%s', " + "FLD_MAPNAME='%s', FLD_CX=%d, FLD_CY=%d, FLD_DIR=%d, FLD_HAIR=%d, FLD_SEX=%d, " + "FLD_JOB=%d, FLD_GOLD=%d, FLD_LEVEL=%d, FLD_HP=%d, FLD_MP=%d, FLD_EXP=%d, " + "FLD_HOMEMAP='%s', FLD_HOMECX=%d, FLD_HOMECY=%d, FLD_DEARCHARNAME='%s', " + "FLD_MASTERCHARNAME='%s', FLD_MASTER=%d, FLD_CREDITPOINT=%d, FLD_IPLEVEL=%d, " + "FLD_STORAGEPASSWD='%s', FLD_REBIRTHLEVEL=%d, FLD_LOCKLOGON=%d, FLD_IPPOINT=%d, " + "FLD_BONUSPOINT=%d, FLD_GAMEGOLD=%d, FLD_GAMEPOINT=%d, FLD_PAYPOINT=%d, " + "FLD_HUNGRYSTATE=%d, FLD_PKPOINT=%d, FLD_ALLOWPARTY=%d, FLD_FREEGULITYCOUNT=%d, " + "FLD_ATTACKMODE=%d, FLD_INCHEALTH=%d, FLD_INCSPELL=%d, FLD_INCHEALING=%d, " + "FLD_FIGHTZONEDIE=%d, FLD_TESTSERVERRESETCOUNT=%d, FLD_IPEXP=%d, " + "FLD_NIMBUSPOINT=%d, FLD_NATUREELEMENT=%d, FLD_ENABLEGRECALL=%d, " + "FLD_ENABLEGROUPRECALL=%d, FLD_GAINEXPRATE=%d, FLD_GAINEXPRATETIME=%d, " + "FLD_HERONAME='%s', FLD_HEROMASTERNAME='%s', FLD_OPENGAMEGOLDDEAL=%d, " + "FLD_GROUPRECALLTIME=%d, FLD_BODYLUCK=%f, FLD_MARKMAP='%s', " + "FLD_MARKMAPX=%d, FLD_MARKMAPY=%d WHERE FLD_CHARNAME='%s'";
            const string sSqlStr3 = "UPDATE TBL_BONUSABILITY SET FLD_AC=%d, FLD_MAC=%d, FLD_DC=%d, FLD_MC=%d, FLD_SC=%d, FLD_HP=%d, FLD_MP=%d, FLD_HIT=%d, FLD_SPEED=%d, FLD_RESERVED=%d, " + "WHERE FLD_CHARNAME='%s'";
            const string sSqlStr4 = "DELETE FROM TBL_QUEST WHERE FLD_CHARNAME='%s'";
            const string sSqlStr5 = "INSERT INTO TBL_QUEST (FLD_CHARNAME, FLD_QUESTOPENINDEX, FLD_QUESTFININDEX, FLD_QUEST) VALUES(:FLD_CHARNAME, :FLD_QUESTOPENINDEX, :FLD_QUESTFININDEX, :FLD_QUEST)";
            try
            {
                hd = HumanRCD.Data;
                var command = new MySqlCommand();
                using var conn = GetConnection();
                conn.Open();
                command.CommandText = string.Format(sSqlStr, new object[] { hd.sChrName, hd.sAccount, hd.btHair, hd.btSex, hd.btJob });
                command.Connection = (MySqlConnection)conn;
                if (boNew)
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result = false;
                        DBShare.MainOutMessage("[Exception] TFileDB.UpdateRecord (1)");
                        return result;
                    }
                }
                else
                {
                    dwHP = HUtil32.MakeLong(hd.Abil.HP, hd.Abil.AC);
                    dwMP = HUtil32.MakeLong(hd.Abil.MP, hd.Abil.MAC);
                    //command.CommandText = string.Format(sSqlStr2, new object[] { 0, FormatDateTime(Grobal2.SQLDTFORMAT, HumanRCD.Header.dCreateDate), hd.sCurMap, hd.wCurX, hd.wCurY, hd.btDir, hd.btHair, hd.btSex, hd.btJob, hd.nGold, hd.Abil.Level, dwHP, dwMP, hd.Abil.Exp, hd.sHomeMap, hd.wHomeX, hd.wHomeY, hd.sDearName, hd.sMasterName, ((byte)hd.boMaster), hd.btCreditPoint, hd.btInPowerLevel, hd.sStoragePwd, hd.btReLevel, ((byte)hd.boLockLogon), hd.wInPowerPoint, hd.nBonusPoint, hd.nGameGold, hd.nGamePoint, hd.nPayMentPoint, hd.nHungerStatus, hd.nPKPOINT, ((byte)hd.btAllowGroup), hd.btClPkPoint, hd.btAttatckMode, hd.btIncHealth, hd.btIncSpell, hd.btIncHealing, hd.btFightZoneDieCount, hd.btNewHuman, hd.dwInPowerExp, hd.dwGatherNimbus, hd.btAttribute, ((byte)hd.boAllowGuildReCall), hd.boAllowGroupReCall, hd.nKillMonExpRate, hd.dwKillMonExpRateTime, hd.sHeroName, hd.sHeroMasterName, hd.btOptnYBDeal, hd.wGroupRcallTime, hd.dBodyLuck, hd.sMarkerMap, hd.wMarkerX, hd.wMarkerY, HumanRCD.Header.sName });
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result = false;
                        DBShare.MainOutMessage("[Exception] TFileDB.UpdateRecord (2)");
                        return result;
                    }
                    command.CommandText = string.Format(sSqlStr3, new object[] { hd.BonusAbil.AC, hd.BonusAbil.MAC, hd.BonusAbil.DC, hd.BonusAbil.MC, hd.BonusAbil.SC, hd.BonusAbil.HP, hd.BonusAbil.MP, hd.BonusAbil.Hit, hd.BonusAbil.Speed, hd.BonusAbil.X2, HumanRCD.Header.sName });
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result = false;
                        DBShare.MainOutMessage("[Exception] TFileDB.UpdateRecord (3)");
                    }
                    // Delete Quest Data
                    command.CommandText = string.Format(sSqlStr4, HumanRCD.Header.sName);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result = false;
                        DBShare.MainOutMessage("[Exception] TFileDB.UpdateRecord (DELETE TBL_QUEST)");
                    }
                    try
                    {
                        //command.CommandText = sSqlStr5;
                        //Units.HumDB_SQL.dbQry.ParamByName("FLD_CHARNAME").Value = HumanRCD.Header.sName;
                        //EDcode.Encode6BitBuf(HumanRCD.Data.QuestUnitOpen, TempBuf, sizeof(HumanRCD.Data.QuestUnitOpen), sizeof(TempBuf));
                        //Units.HumDB_SQL.dbQry.ParamByName("FLD_QUESTOPENINDEX").Value = TempBuf;
                        //EDcode.Encode6BitBuf(HumanRCD.Data.QuestUnit, TempBuf, sizeof(HumanRCD.Data.QuestUnit), sizeof(TempBuf));
                        //Units.HumDB_SQL.dbQry.ParamByName("FLD_QUESTFININDEX").Value = TempBuf;
                        //EDcode.Encode6BitBuf(HumanRCD.Data.QuestFlag, TempBuf, sizeof(HumanRCD.Data.QuestFlag), sizeof(TempBuf));
                        //Units.HumDB_SQL.dbQry.ParamByName("FLD_QUEST").Value = TempBuf;
                        //Units.HumDB_SQL.dbQry.Execute;
                    }
                    catch
                    {
                        result = false;
                        DBShare.MainOutMessage("[Exception] TFileDB.UpdateRecord (INSERT TBL_QUEST)");
                    }
                    // Delete Magic Data
                    command.CommandText = string.Format("DELETE FROM TBL_MAGIC WHERE FLD_CHARNAME='%s'", new object[] { HumanRCD.Header.sName });
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result = false;
                        DBShare.MainOutMessage("[Exception] TFileDB.UpdateRecord (DELETE TBL_MAGIC)");
                    }
                    for (i = 0; i <= hd.Magic.GetUpperBound(0); i++)
                    {
                        if (hd.Magic[i].wMagIdx > 0)
                        {
                            //command.CommandText = string.Format("INSERT TBL_MAGIC(FLD_CHARNAME, FLD_MAGICID, FLD_TYPE, FLD_LEVEL, FLD_USEKEY, FLD_CURRTRAIN) VALUES " + "( '%s', %d, %d, %d, %d, %d )", new object[] { HumanRCD.Header.sName, hd.Magic[i].btClass, hd.Magic[i].wMagIdx, hd.Magic[i].btLevel, hd.Magic[i].btKey, hd.Magic[i].nTranPoint });
                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch
                            {
                                result = false;
                                DBShare.MainOutMessage("[Exception] TFileDB.UpdateRecord (INSERT TBL_MAGIC)");
                            }
                        }
                    }
                    // Delete Item Data
                    command.CommandText = string.Format("DELETE FROM TBL_ITEM WHERE FLD_CHARNAME='%s'", new object[] { HumanRCD.Header.sName });
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result = false;
                        DBShare.MainOutMessage("[Exception] TFileDB.UpdateRecord (DELETE TBL_ITEM)");
                    }
                    for (i = 0; i <= hd.BagItems.GetUpperBound(0); i++)
                    {
                        if ((hd.BagItems[i].wIndex > 0) && (hd.BagItems[i].MakeIndex > 0))
                        {
                            command.CommandText = string.Format("INSERT TBL_ITEM(FLD_CHARNAME, FLD_POSITION, " + "FLD_MAKEINDEX, FLD_STDINDEX, FLD_DURA, FLD_DURAMAX, FLD_VALUE0, FLD_VALUE1, " + "FLD_VALUE2, FLD_VALUE3, FLD_VALUE4, FLD_VALUE5, FLD_VALUE6, FLD_VALUE7, FLD_VALUE8, FLD_VALUE9, " + "FLD_VALUE10, FLD_VALUE11, FLD_VALUE12, FLD_VALUE13, FLD_VALUE14, FLD_VALUE15, FLD_VALUE16, " + "FLD_VALUE17, FLD_VALUE18, FLD_VALUE19, FLD_VALUE20, FLD_VALUE21, FLD_VALUE22, FLD_VALUE23, " + "FLD_VALUE24, FLD_VALUE25) VALUES " + "( '%s', 0, " + "%d, %d, %d, %d, %d, %d, %d, %d, %d, %d, %d, %d, %d, %d, %d, " + "%d, %d, %d, %d, %d, %d, %d, %d, %d, %d, %d, %d, %d, %d, %d )", new object[] { HumanRCD.Header.sName, 0, hd.BagItems[i].MakeIndex, hd.BagItems[i].wIndex, hd.BagItems[i].Dura, hd.BagItems[i].DuraMax, hd.BagItems[i].btValue[0], hd.BagItems[i].btValue[1], hd.BagItems[i].btValue[2], hd.BagItems[i].btValue[3], hd.BagItems[i].btValue[4], hd.BagItems[i].btValue[5], hd.BagItems[i].btValue[6], hd.BagItems[i].btValue[7], hd.BagItems[i].btValue[8], hd.BagItems[i].btValue[9], hd.BagItems[i].btValue[10], hd.BagItems[i].btValue[11], hd.BagItems[i].btValue[12], hd.BagItems[i].btValue[13], hd.BagItems[i].btValue[14], hd.BagItems[i].btValue[15], hd.BagItems[i].btValue[16], hd.BagItems[i].btValue[17], hd.BagItems[i].btValue[18], hd.BagItems[i].btValue[19], hd.BagItems[i].btValue[20], hd.BagItems[i].btValue[21], hd.BagItems[i].btValue[22], hd.BagItems[i].btValue[23], hd.BagItems[i].btValue[24], hd.BagItems[i].btValue[25] });
                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch
                            {
                                result = false;
                                DBShare.MainOutMessage("[Exception] TFileDB.UpdateRecord (INSERT TBL_ITEM)");
                            }
                        }
                    }

                    for (i = 0; i <= hd.HumItems.GetUpperBound(0); i++)
                    {
                        if ((hd.HumItems[i].wIndex > 0) && (hd.HumItems[i].MakeIndex > 0))
                        {
                            command.CommandText = string.Format("INSERT TBL_ITEM(FLD_CHARNAME, FLD_POSITION, FLD_MAKEINDEX, FLD_STDINDEX, FLD_DURA, FLD_DURAMAX, " + "FLD_VALUE0, FLD_VALUE1, FLD_VALUE2, FLD_VALUE3, FLD_VALUE4, FLD_VALUE5, FLD_VALUE6, FLD_VALUE7, FLD_VALUE8, " + "FLD_VALUE9, FLD_VALUE10, FLD_VALUE11, FLD_VALUE12, FLD_VALUE13, FLD_VALUE14, FLD_VALUE15, FLD_VALUE16, FLD_VALUE17, FLD_VALUE18, " + "FLD_VALUE19, FLD_VALUE20, FLD_VALUE21, FLD_VALUE22, FLD_VALUE23, FLD_VALUE24, FLD_VALUE25) VALUES " + "( '%s', %d, %d, %d, " + "%d, %d, %d, %d, %d, %d, %d, %d, %d, %d, " + "%d, %d, %d, %d, %d, %d, %d, %d, %d, %d, " + "%d, %d, %d, %d, %d, %d, %d, %d )", new object[] { HumanRCD.Header.sName, i + 1, hd.HumItems[i].MakeIndex, hd.HumItems[i].wIndex, hd.HumItems[i].Dura, hd.HumItems[i].DuraMax, hd.HumItems[i].btValue[0], hd.HumItems[i].btValue[1], hd.HumItems[i].btValue[2], hd.HumItems[i].btValue[3], hd.HumItems[i].btValue[4], hd.HumItems[i].btValue[5], hd.HumItems[i].btValue[6], hd.HumItems[i].btValue[7], hd.HumItems[i].btValue[8], hd.HumItems[i].btValue[9], hd.HumItems[i].btValue[10], hd.HumItems[i].btValue[11], hd.HumItems[i].btValue[12], hd.HumItems[i].btValue[13], hd.HumItems[i].btValue[14], hd.HumItems[i].btValue[15], hd.HumItems[i].btValue[16], hd.HumItems[i].btValue[17], hd.HumItems[i].btValue[18], hd.HumItems[i].btValue[19], hd.HumItems[i].btValue[20], hd.HumItems[i].btValue[21], hd.HumItems[i].btValue[22], hd.HumItems[i].btValue[23], hd.HumItems[i].btValue[24], hd.HumItems[i].btValue[25] });
                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch
                            {
                                result = false;
                                DBShare.MainOutMessage("[Exception] TFileDB.UpdateRecord (13)");
                            }
                        }
                    }
                    // Delete Store Item Data
                    command.CommandText = string.Format("DELETE FROM TBL_STORAGE WHERE FLD_CHARNAME='{0}'", HumanRCD.Header.sName);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result = false;
                        DBShare.MainOutMessage("[Exception] TFileDB.UpdateRecord (10)");
                    }
                    for (i = 0; i <= hd.StorageItems.GetUpperBound(0); i++)
                    {
                        if ((hd.StorageItems[i].wIndex > 0) && (hd.StorageItems[i].MakeIndex > 0))
                        {
                            command.CommandText = string.Format("INSERT TBL_STORAGE( FLD_CHARNAME, FLD_MAKEINDEX, FLD_STDINDEX, FLD_DURA, FLD_DURAMAX, " + "FLD_VALUE0, FLD_VALUE1, FLD_VALUE2, FLD_VALUE3, FLD_VALUE4, FLD_VALUE5, FLD_VALUE6, FLD_VALUE7, FLD_VALUE8, " + "FLD_VALUE9, FLD_VALUE10, FLD_VALUE11, FLD_VALUE12, FLD_VALUE13, FLD_VALUE14, FLD_VALUE15, FLD_VALUE16, FLD_VALUE17, FLD_VALUE18, " + "FLD_VALUE19, FLD_VALUE20, FLD_VALUE21, FLD_VALUE22, FLD_VALUE23, FLD_VALUE24, FLD_VALUE25) VALUES " + "( '%s', %d, %d, %d, " + "%d, %d, %d, %d, %d, %d, %d, %d, %d, %d, " + "%d, %d, %d, %d, %d, %d, %d, %d, %d, %d, " + "%d, %d, %d, %d, %d, %d, %d, %d )", new object[] { HumanRCD.Header.sName, hd.StorageItems[i].MakeIndex, hd.StorageItems[i].wIndex, hd.StorageItems[i].Dura, hd.StorageItems[i].DuraMax, hd.StorageItems[i].btValue[0], hd.StorageItems[i].btValue[1], hd.StorageItems[i].btValue[2], hd.StorageItems[i].btValue[3], hd.StorageItems[i].btValue[4], hd.StorageItems[i].btValue[5], hd.StorageItems[i].btValue[6], hd.StorageItems[i].btValue[7], hd.StorageItems[i].btValue[8], hd.StorageItems[i].btValue[9], hd.StorageItems[i].btValue[10], hd.StorageItems[i].btValue[11], hd.StorageItems[i].btValue[12], hd.StorageItems[i].btValue[13] });
                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch
                            {
                                result = false;
                                DBShare.MainOutMessage("[Exception] TFileDB.UpdateRecord (11)");
                            }
                        }
                    }
                    command.CommandText = string.Format("DELETE FROM TBL_ADDON WHERE FLD_CHARNAME='{0}'", HumanRCD.Header.sName);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result = false;
                        DBShare.MainOutMessage("[Exception] TFileDB.UpdateRecord (DELETE TBL_ADDON)");
                    }
                    sTmp = "";
                    //for (i = 0; i <= HumanRCD.Data.wStatusTimeArr.GetUpperBound(0); i++)
                    //{
                    //    sTmp = sTmp + (HumanRCD.Data.wStatusTimeArr[i]).ToString() + "/";
                    //}
                    //sTmp2 = "";
                    //for (i = 0; i <= HumanRCD.Data.SeriesSkillArr.GetUpperBound(0); i++)
                    //{
                    //    sTmp2 = sTmp2 + (HumanRCD.Data.SeriesSkillArr[i]).ToString() + "/";
                    //}
                    //EDcode.Encode6BitBuf(HumanRCD.Data.MissionFlag[0], TempBuf, sizeof(HumanRCD.Data.MissionFlag), sizeof(TempBuf));
                    //sTmp3 = TempBuf;
                    //EDcode.Encode6BitBuf(HumanRCD.Data.VenationInfos, TempBuf, sizeof(HumanRCD.Data.VenationInfos), sizeof(TempBuf));
                    //command.CommandText = string.Format("INSERT TBL_ADDON (FLD_CHARNAME, FLD_STATUS, FLD_SERIESSKILLORDER, FLD_MISSION, FLD_VENATION) " + "VALUES ('%s', '%s', '%s', '%s', '%s')", new string[] { HumanRCD.Header.sName, sTmp, sTmp2, sTmp3, TempBuf });
                    //try
                    //{
                    //    command.ExecuteNonQuery();
                    //}
                    //catch
                    //{
                    //    result = false;
                    //    DBShare.MainOutMessage("[Exception] TFileDB.UpdateRecord (INSERT TBL_ADDON (FLD_STATUS))");
                    //}
                }
                m_boChanged = true;
            }
            finally
            {
                 
            }
            return result;
        }

        public int Find(string sChrName, StringDictionary List)
        {
            int result;
            for (var i = 0; i < m_MirQuickList.Count; i++)
            {
                if (HUtil32.CompareLStr(m_MirQuickList[i], sChrName, sChrName.Length))
                {
                    //List.Add(m_MirQuickList[i], m_MirQuickList.Values[i]);
                }
            }
            result = List.Count;
            return result;
        }

        public bool Delete(int nIndex)
        {
            bool result = false;
            string s14;
            for (var i = 0; i < m_MirQuickList.Count; i++)
            {
                //if (((int)m_MirQuickList.Values[i]) == nIndex)
                //{
                //    s14 = m_MirQuickList[i];
                //    if (DeleteRecord(nIndex))
                //    {
                //        m_MirQuickList.Remove(i);
                //        result = true;
                //        break;
                //    }
                //}
            }
            return result;
        }

        private bool DeleteRecord(int nIndex)
        {
            bool result = true;
            string sChrName = m_MirQuickList[nIndex];
            var command = new MySqlCommand();
            using var conn = GetConnection();
            conn.Open();
            command.CommandText = string.Format("UPDATE TBL_CHARACTER SET FLD_DELETED=1, FLD_CREATEDATE=now() WHERE FLD_CHARNAME='{0}'", sChrName);
            command.Connection = (MySqlConnection)conn;
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result = false;
                DBShare.MainOutMessage("[Exception] TFileDB.DeleteRecord");
            }
            return result;
        }

        public void Rebuild()
        {
        }

        public int Count()
        {
            return m_MirQuickList.Count;
        }

        public bool Delete(string sChrName)
        {
            bool result = false;
            //int nIndex = m_MirQuickList.GetIndex(sChrName);
            //if (nIndex < 0)
            //{
            //    return result;
            //}
            //if (DeleteRecord(nIndex))
            //{
            //    m_MirQuickList.Remove(nIndex);
            //    result = true;
            //}
            return result;
        }

        public int GetQryChar(int nIndex, ref TQueryChr QueryChrRcd)
        {
            int result = -1;
            string sChrName;
            const string sSQL = "SELECT * FROM TBL_CHARACTER WHERE FLD_CHARNAME='{0}'";
            if (nIndex < 0)
            {
                return result;
            }
            if (m_QuickIndexNameList.Count <= nIndex)
            {
                return result;
            }
            sChrName = m_QuickIndexNameList[nIndex];
            try
            {
                var command = new MySqlCommand();
                try
                {
                    using var conn = GetConnection();
                    conn.Open();
                    command.CommandText = string.Format(sSQL, sChrName);
                    command.Connection = (MySqlConnection)conn;
                }
                catch (Exception)
                {
                    DBShare.MainOutMessage("[Exception] TFileDB.GetQryChar (1)");
                    return result;
                }
                var dr = command.ExecuteReader();
                while (dr.Read())
                {
                    QueryChrRcd.sName = dr.GetString("FLD_CHARNAME");
                    QueryChrRcd.btJob = dr.GetByte("FLD_JOB");
                    QueryChrRcd.btHair = dr.GetByte("FLD_HAIR");
                    QueryChrRcd.btSex = dr.GetByte("FLD_SEX");
                    QueryChrRcd.wLevel = dr.GetUInt16("FLD_LEVEL");
                }
            }
            finally
            {

            }
            result = nIndex;
            return result;
        }
    }
}

namespace DBSvr
{
    public class HumDB_SQL
    {
        public static bool g_boSQLIsReady = false;
        public static THumDB HumDataDB = null;

        public static bool InitializeSQL()
        {
            bool result;
            result = false;
            if (g_boSQLIsReady)
            {
                return result;
            }
            //ADOConnection.Database = DBShare.g_sSQLDatabase;
            //ADOConnection.Server = DBShare.g_sSQLHost;
            //ADOConnection.UserName = DBShare.g_sSQLUserName;
            //ADOConnection.Password = DBShare.g_sSQLPassword;
            try
            {
                g_boSQLIsReady = true;
            }
            catch
            {
                DBShare.MainOutMessage("SQLÁ¬½ÓÊ§°Ü£¡");
                g_boSQLIsReady = false;
                result = false;
                return result;
            }
            result = true;
            return result;
        }

        //public void initialization()
        //{
        //    CoInitialize(null);
        //    ADOConnection = new object(null);
        //    dbQry = new object(null);
        //}

        //public void finalization()
        //{
        //    dbQry.Free;
        //    ADOConnection.Free;
        //    CoUnInitialize;
        //}
    }
}