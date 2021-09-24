using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Text;
using Org.BouncyCastle.Crypto.Modes.Gcm;
using SystemModule;

namespace DBSvr
{
    public class MySqlHumDB
    {
        private Dictionary<string, int> m_MirQuickList = null;
        private TQuickIDList m_MirQuickIDList = null;
        private Dictionary<int, string> m_QuickIndexNameList = null;
        private int m_nRecordCount = 0;
        private IDbConnection _dbConnection;

        public MySqlHumDB()
        {
            m_MirQuickList = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            m_MirQuickIDList = new TQuickIDList();
            m_nRecordCount = -1;
            m_QuickIndexNameList = new Dictionary<int, string>();
            LoadQuickList();
        }

        private void LoadQuickList()
        {
            bool boDeleted;
            IList<TQuickID> AccountList;
            IList<string> ChrNameList;
            string sAccount;
            string sChrName;
            int nSelectID = 0;
            const string sSQL = "SELECT * FROM TBL_CHARACTER";
            m_MirQuickList.Clear();
            m_MirQuickIDList.Clear();
            m_nRecordCount = -1;
            AccountList = new List<TQuickID>();
            ChrNameList = new List<string>();
            try
            {
                if (!Open())
                {
                    return;
                }
                var command = new MySqlCommand();
                command.CommandText = sSQL;
                command.Connection = (MySqlConnection)_dbConnection;
                using var dr = command.ExecuteReader();
                var nIndex = 0;
                while (dr.Read())
                {
                    boDeleted = dr.GetBoolean("FLD_DELETED");
                    sAccount = dr.GetString("FLD_LOGINID");
                    sChrName = dr.GetString("FLD_CHARNAME");
                    if (!boDeleted && (sChrName != ""))
                    {
                        m_MirQuickList.Add(sChrName, nIndex);
                        AccountList.Add(new TQuickID()
                        {
                            sAccount = sAccount,
                            nSelectID = nIndex
                        });
                        ChrNameList.Add(sChrName);
                    }

                    nIndex++;
                }
            }
            finally
            {
                Close();
            }
            for (var nIndex = 0; nIndex < AccountList.Count; nIndex++)
            {
                m_MirQuickIDList.AddRecord(AccountList[nIndex].sAccount, ChrNameList[nIndex], 0, AccountList[nIndex].nSelectID);
                m_QuickIndexNameList.Add(nIndex, ChrNameList[nIndex]);
            }
            AccountList = null;
            ChrNameList = null;
            //m_MirQuickList.SortString(0, m_MirQuickList.Count - 1);
        }

        public bool Open()
        {
            try
            {
                if (_dbConnection == null)
                {
                    _dbConnection = new MySqlConnection(DBShare.DBConnection);
                }
                if (_dbConnection.State == ConnectionState.Closed)
                {
                    _dbConnection.Open();
                }
                return true;
            }
            catch (Exception e)
            {
                DBShare.MainOutMessage("打开数据库链接失败");
            }
            return false;
        }

        public void Close()
        {
            if (_dbConnection != null)
            {
                _dbConnection.Close();
                _dbConnection.Dispose();
            }
        }

        public int Index(string sName)
        {
            if (m_MirQuickList.ContainsKey(sName))
            {
                return m_MirQuickList[sName];
            }
            return -1;
        }

        public int ChrCountOfAccount(string sAccount)
        {
            return 0;
        }

        public int Get(int nIndex, ref THumDataInfo HumanRCD)
        {
            int result = -1;
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
                if (!Open())
                {
                    return false;
                }
                var command = new MySqlCommand();
                command.CommandText = string.Format("UPDATE TBL_CHARACTER SET FLD_SEX={0}, FLD_JOB={1} WHERE FLD_CHARNAME='{2}'", new object[] { QueryChrRcd.btSex, QueryChrRcd.btJob, QueryChrRcd.sName });
                command.Connection = (MySqlConnection)_dbConnection;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    DBShare.MainOutMessage("[Exception] MySqlHumDB.UpdateChrRecord");
                    result = false;
                }
            }
            finally
            {
                Close();
            }
            return result;
        }

        public bool Add(ref THumDataInfo HumanRCD)
        {
            bool result = false;
            int nIndex;
            string sChrName = HumanRCD.Header.sName;
            if (m_MirQuickList.TryGetValue(sChrName, out nIndex))
            {
                if (nIndex >= 0)
                {
                    result = false;
                }
            }
            else
            {
                nIndex = m_nRecordCount;
                m_nRecordCount++;
                if (UpdateRecord(nIndex, ref HumanRCD, true))
                {
                    m_MirQuickList.Add(sChrName, nIndex);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        private bool GetRecord(int nIndex, ref THumDataInfo HumanRCD)
        {
            const string sSQL3 = "SELECT * FROM TBL_QUEST WHERE FLD_CHARNAME='{0}'";
            const string sSQL7 = "SELECT * FROM TBL_ADDON WHERE FLD_CHARNAME='{0}'";
            bool result = true;
            string sChrName = m_QuickIndexNameList[nIndex];
            try
            {
                GetChrRecord(sChrName, ref HumanRCD);
                GetAbilGetRecord(sChrName, ref HumanRCD);
                GetBonusAbil(sChrName, ref HumanRCD);
                GetChrMagic(sChrName, ref HumanRCD);
                GetChrItems(sChrName, ref HumanRCD);
                GetChrStorage(sChrName, ref HumanRCD);
                //try
                //{
                //    command.CommandText = string.Format(sSQL3, sChrName);
                //}
                //catch (Exception)
                //{
                //    DBShare.MainOutMessage("[Exception] MySqlHumDB.GetRecord (3)");
                //    return false;
                //}
                //dr = command.ExecuteReader();
                //while (dr.Read())
                //{
                //    sTmp = dr.GetString("FLD_QUESTOPENINDEX").AsString.Trim();
                //    if (sTmp != "")
                //    {
                //        EDcode.Decode6BitBuf((sTmp as string), HumanRCD.Data.QuestUnitOpen, sTmp.Length, sizeof(HumanRCD.Data.QuestUnitOpen));
                //    }
                //    sTmp = dr.GetString("FLD_QUESTFININDEX").AsString;
                //    if (sTmp != "")
                //    {
                //        EDcode.Decode6BitBuf((sTmp as string), HumanRCD.Data.QuestUnit, sTmp.Length, sizeof(HumanRCD.Data.QuestUnit));
                //    }
                //    sTmp = dr.GetString("FLD_QUEST").AsString;
                //    if (sTmp != "")
                //    {
                //        EDcode.Decode6BitBuf((sTmp as string), HumanRCD.Data.QuestFlag, sTmp.Length, sizeof(HumanRCD.Data.QuestFlag));
                //    }
                //}

                //try
                //{
                //    command.CommandText = string.Format(sSQL7, sChrName);
                //}
                //catch (Exception)
                //{
                //    DBShare.MainOutMessage("[Exception] MySqlHumDB.GetRecord (7)");
                //    return false;
                //}
                //dr = command.ExecuteReader();
                //while (dr.Read())
                //{
                //    TStatusTime;
                //    sTmp = dr.GetString("FLD_STATUS").AsString;
                //    i = HumanRCD.Data.wStatusTimeArr.GetLowerBound(0);
                //    while (sTmp != "")
                //    {
                //        sTmp = HUtil32.GetValidStr3(sTmp, ref str, new string[] { "/" });
                //        HumanRCD.Data.wStatusTimeArr[i] = Convert.ToInt32(str);
                //        i++;
                //        if (i > HumanRCD.Data.wStatusTimeArr.GetUpperBound(0))
                //        {
                //            break;
                //        }
                //    }
                //    // TSeriesSkillArr;
                //    sTmp = dr.GetString("FLD_SERIESSKILLORDER").AsString;
                //    i = HumanRCD.Data.SeriesSkillArr.GetLowerBound(0);
                //    while (sTmp != "")
                //    {
                //        sTmp = HUtil32.GetValidStr3(sTmp, ref str, new string[] { "/" });
                //        HumanRCD.Data.SeriesSkillArr[i] = Convert.ToInt32(str);
                //        i++;
                //        if (i > HumanRCD.Data.SeriesSkillArr.GetUpperBound(0))
                //        {
                //            break;
                //        }
                //    }
                //    sTmp = dr.GetString("FLD_MISSION").AsString;
                //    if (sTmp != "")
                //    {
                //        EDcode.Decode6BitBuf((sTmp as string), HumanRCD.Data.MissionFlag[0], sTmp.Length, sizeof(HumanRCD.Data.MissionFlag));
                //    }
                //    sTmp = dr.GetString("FLD_VENATION").AsString;
                //    if (sTmp != "")
                //    {
                //        EDcode.Decode6BitBuf((sTmp as string), HumanRCD.Data.VenationInfos, sTmp.Length, sizeof(HumanRCD.Data.VenationInfos));
                //    }
                //}
                //dr.Close();
                //dr.Dispose();
            }
            catch(Exception ex)
            {
                DBShare.MainOutMessage("GetRecord:" + ex.Message);
            }
            return result;
        }

        private bool GetChrRecord(string sChrName, ref THumDataInfo HumanRCD)
        {
            bool result = false;
            if (!Open())
            {
                return result;
            }
            const string sSQL1 = "SELECT * FROM TBL_CHARACTER WHERE FLD_CHARNAME='{0}'";
            var command = new MySqlCommand();
            try
            {
                command.CommandText = string.Format(sSQL1, sChrName);
                command.Connection = (MySqlConnection)_dbConnection;
                var dr = command.ExecuteReader();
                while (dr.Read())
                {
                    HumanRCD = new THumDataInfo();
                    HumanRCD.Data.sAccount = dr.GetString("FLD_LOGINID");
                    HumanRCD.Header.sName = dr.GetString("FLD_CHARNAME");
                    HumanRCD.Header.boDeleted = dr.GetBoolean("FLD_DELETED");
                    HumanRCD.Header.dCreateDate = HUtil32.DateTimeToDouble(dr.GetDateTime("FLD_CREATEDATE"));
                    HumanRCD.Data.sChrName = dr.GetString("FLD_CHARNAME");
                    if (!dr.IsDBNull(dr.GetOrdinal("FLD_MAPNAME")))
                    {
                        HumanRCD.Data.sCurMap = dr.GetString("FLD_MAPNAME");
                    }

                    HumanRCD.Data.wCurX = dr.GetInt16("FLD_CX");
                    HumanRCD.Data.wCurY = dr.GetInt16("FLD_CY");
                    HumanRCD.Data.btDir = dr.GetByte("FLD_DIR");
                    HumanRCD.Data.btHair = dr.GetByte("FLD_HAIR");
                    HumanRCD.Data.btSex = dr.GetByte("FLD_SEX");
                    HumanRCD.Data.btJob = dr.GetByte("FLD_JOB");
                    HumanRCD.Data.nGold = dr.GetInt32("FLD_GOLD");
                    if (!dr.IsDBNull(dr.GetOrdinal("FLD_HOMEMAP")))
                    {
                        HumanRCD.Data.sHomeMap = dr.GetString("FLD_HOMEMAP");
                    }

                    HumanRCD.Data.wHomeX = dr.GetInt16("FLD_HOMEX");
                    HumanRCD.Data.wHomeY = dr.GetInt16("FLD_HOMEY");
                    if (!dr.IsDBNull(dr.GetOrdinal("FLD_DearName")))
                    {
                        HumanRCD.Data.sDearName = dr.GetString("FLD_DearName");
                    }

                    if (!dr.IsDBNull(dr.GetOrdinal("FLD_MasterName")))
                    {
                        HumanRCD.Data.sMasterName = dr.GetString("FLD_MasterName");
                    }

                    HumanRCD.Data.boMaster = dr.GetBoolean("FLD_IsMaster");
                    HumanRCD.Data.btCreditPoint = (byte)dr.GetInt32("FLD_CREDITPOINT");
                    if (!dr.IsDBNull(dr.GetOrdinal("FLD_StoragePwd")))
                    {
                        HumanRCD.Data.sStoragePwd = dr.GetString("FLD_StoragePwd");
                    }

                    HumanRCD.Data.btReLevel = dr.GetByte("FLD_ReLevel");
                    HumanRCD.Data.boLockLogon = dr.GetBoolean("FLD_LOCKLOGON");
                    HumanRCD.Data.nBonusPoint = dr.GetInt32("FLD_BONUSPOINT");
                    HumanRCD.Data.nGameGold = dr.GetInt32("FLD_Gold");
                    HumanRCD.Data.nGamePoint = dr.GetInt32("FLD_GamePoint");
                    HumanRCD.Data.nPayMentPoint = dr.GetInt32("FLD_PayMentPoint");
                    HumanRCD.Data.nHungerStatus = dr.GetInt32("FLD_HungerStatus");
                    HumanRCD.Data.btAllowGroup = dr.GetBoolean("FLD_AllowGroup") ? (byte)1 : (byte)0;
                    HumanRCD.Data.btAttatckMode = dr.GetByte("FLD_AttatckMode");
                    HumanRCD.Data.btIncHealth = dr.GetByte("FLD_IncHealth");
                    HumanRCD.Data.btIncSpell = dr.GetByte("FLD_IncSpell");
                    HumanRCD.Data.btIncHealing = dr.GetByte("FLD_IncHealing");
                    HumanRCD.Data.btFightZoneDieCount = dr.GetByte("FLD_FightZoneDieCount");
                    HumanRCD.Data.boAllowGuildReCall = dr.GetBoolean("FLD_AllowGuildReCall");
                    HumanRCD.Data.boAllowGroupReCall = dr.GetBoolean("FLD_AllowGroupReCall");
                    HumanRCD.Data.wGroupRcallTime = dr.GetInt16("FLD_GroupRcallTime");
                    HumanRCD.Data.dBodyLuck = dr.GetDouble("FLD_BodyLuck");
                    result = true;
                }
                dr.Close();
                dr.Dispose();
            }
            catch (Exception ex)
            {
                DBShare.MainOutMessage("[Exception] MySqlHumDB.GetRecord (1)");
                return false;
            }
            finally
            {
                Close();
            }
            return result;
        }

        private bool GetAbilGetRecord(string sChrName, ref THumDataInfo HumanRCD)
        {
            bool reslut = false;
            if (!Open())
            {
                return reslut;
            }
            int dw;
            var command = new MySqlCommand();
            try
            {
                command.CommandText = $"select * from TBL_CHARACTER_ABLITY where FLD_ChrName='{sChrName}'";
                command.Connection = (MySqlConnection)_dbConnection;
                var dr = command.ExecuteReader();
                if (dr.Read())
                {
                    HumanRCD.Data.Abil.Level = dr.GetUInt16("FLD_LEVEL");
                    dw = dr.GetInt32("FLD_HP");
                    HumanRCD.Data.Abil.HP = HUtil32.LoWord(dw);
                    HumanRCD.Data.Abil.AC = HUtil32.HiWord(dw);
                    dw = dr.GetInt32("FLD_MP");
                    HumanRCD.Data.Abil.MP = HUtil32.LoWord(dw);
                    HumanRCD.Data.Abil.MAC = HUtil32.HiWord(dw);
                    HumanRCD.Data.Abil.DC = dr.GetInt32("FLD_DC");
                    HumanRCD.Data.Abil.MC = dr.GetInt32("FLD_MC");
                    HumanRCD.Data.Abil.SC = dr.GetInt32("FLD_SC");
                    HumanRCD.Data.Abil.Exp = dr.GetInt32("FLD_EXP");
                    HumanRCD.Data.Abil.MaxExp = dr.GetInt32("FLD_MaxExp");
                    HumanRCD.Data.Abil.Weight = dr.GetUInt16("FLD_Weight");
                    HumanRCD.Data.Abil.MaxWeight = dr.GetUInt16("FLD_MaxWeight");
                    HumanRCD.Data.Abil.WearWeight = dr.GetUInt16("FLD_WearWeight");
                    HumanRCD.Data.Abil.MaxWearWeight = dr.GetUInt16("FLD_MaxWearWeight");
                    HumanRCD.Data.Abil.HandWeight = dr.GetUInt16("FLD_HandWeight");
                    HumanRCD.Data.Abil.MaxHandWeight = dr.GetUInt16("FLD_MaxHandWeight");
                    reslut = true;
                }
                dr.Close();
                dr.Dispose();
            }
            catch (Exception ex)
            {
                DBShare.MainOutMessage("[Exception] MySqlHumDB.GetAbilGetRecord");
                return false;
            }
            finally
            {
                Close();
            }
            return reslut;
        }

        private bool GetBonusAbil(string sChrName, ref THumDataInfo HumanRCD)
        {
            bool reslut = false;
            if (!Open())
            {
                return reslut;
            }
            const string sSQL2 = "SELECT * FROM TBL_BONUSABILITY WHERE FLD_CHARNAME='{0}'";
            var command = new MySqlCommand();
            command.Connection = (MySqlConnection)_dbConnection;
            try
            {
                command.CommandText = string.Format(sSQL2, sChrName);
                var dr = command.ExecuteReader();
                if (dr.Read())
                {
                    if (HumanRCD.Data.BonusAbil == null)
                    {
                        HumanRCD.Data.BonusAbil = new TNakedAbility();
                    }
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
                    reslut = true;
                }
                dr.Close();
                dr.Dispose();
            }
            catch (Exception)
            {
                DBShare.MainOutMessage("[Exception] MySqlHumDB.GetRecord (2)");
                return false;
            }
            finally
            {
                Close();
            }
            return reslut;
        }

        private bool GetChrMagic(string sChrName, ref THumDataInfo HumanRCD)
        {
            bool reslut = false;
            if (!Open())
            {
                return reslut;
            }
            const string sSQL4 = "SELECT * FROM TBL_MAGIC WHERE FLD_CHARNAME='{0}'";
            var command = new MySqlCommand();
            try
            {
                command.Connection = (MySqlConnection)_dbConnection;
                command.CommandText = string.Format(sSQL4, sChrName);
                var dr = command.ExecuteReader();
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
                dr.Close();
                dr.Dispose();
                for (int j = 0; j < magicList.Count; j++)
                {
                    HumanRCD.Data.Magic[j] = magicList[j];
                }
                reslut = true;
            }
            catch (Exception)
            {
                DBShare.MainOutMessage("[Exception] MySqlHumDB.GetRecord (4)");
                return false;
            }
            finally
            {
                Close();
            }
            return reslut;
        }

        private bool GetChrItems(string sChrName, ref THumDataInfo HumanRCD)
        {
            bool reslut = false;
            if (!Open())
            {
                return reslut;
            }
            const string sSQL5 = "SELECT * FROM TBL_ITEM WHERE FLD_CHARNAME='{0}'";
            var command = new MySqlCommand();
            try
            {
                command.Connection = (MySqlConnection)_dbConnection;
                command.CommandText = string.Format(sSQL5, sChrName);
                var dr = command.ExecuteReader();
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
                dr.Close();
                dr.Dispose();
                reslut = true;
            }
            catch (Exception)
            {
                DBShare.MainOutMessage("[Exception] MySqlHumDB.GetRecord (5)");
                return false;
            }
            finally
            {
                Close();
            }
            return reslut;
        }

        private bool GetChrStorage(string sChrName, ref THumDataInfo HumanRCD)
        {
            bool result = false;
            if (!Open())
            {
                return result;
            }
            const string sSQL6 = "SELECT * FROM TBL_STORAGE WHERE FLD_CHARNAME='{0}'";
            var command = new MySqlCommand();
            try
            {
                command.CommandText = string.Format(sSQL6, sChrName);
                command.Connection = (MySqlConnection)_dbConnection;
                var dr = command.ExecuteReader();
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
                dr.Close();
                dr.Dispose();
                result = true;
            }
            catch (Exception)
            {
                DBShare.MainOutMessage("[Exception] MySqlHumDB.GetChrStorage");
                return false;
            }
            finally
            {
                Close();
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
            const string sSqlStr3 = "UPDATE TBL_BONUSABILITY SET FLD_AC=@FLD_AC, FLD_MAC=@FLD_MAC, FLD_DC=@FLD_DC, FLD_MC=@FLD_MC, FLD_SC=@FLD_SC, FLD_HP=@FLD_HP, FLD_MP=@FLD_MP, FLD_HIT=@FLD_HIT, FLD_SPEED=@FLD_SPEED, FLD_RESERVED=@FLD_RESERVED WHERE FLD_CHARNAME='{0}'";
            const string sSqlStr4 = "DELETE FROM TBL_QUEST WHERE FLD_CHARNAME='{0}'";
            const string sSqlStr5 = "INSERT INTO TBL_QUEST (FLD_CHARNAME, FLD_QUESTOPENINDEX, FLD_QUESTFININDEX, FLD_QUEST) VALUES(@FLD_CHARNAME, @FLD_QUESTOPENINDEX, @FLD_QUESTFININDEX, @FLD_QUEST)";
            try
            {
                hd = HumanRCD.Data;
                var command = new MySqlCommand();
                if (boNew)
                {
                    result = InsertRecord(hd);
                }
                else
                {
                    UpdateRecord(nIndex, hd);

                    command.CommandText = string.Format(sSqlStr3, new object[] { hd.BonusAbil.AC, hd.BonusAbil.MAC, hd.BonusAbil.DC, hd.BonusAbil.MC, hd.BonusAbil.SC, hd.BonusAbil.HP, hd.BonusAbil.MP, hd.BonusAbil.Hit, hd.BonusAbil.Speed, hd.BonusAbil.X2, HumanRCD.Header.sName });
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result = false;
                        DBShare.MainOutMessage("[Exception] MySqlHumDB.UpdateRecord (3)");
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
                        DBShare.MainOutMessage("[Exception] MySqlHumDB.UpdateRecord (DELETE TBL_QUEST)");
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
                        DBShare.MainOutMessage("[Exception] MySqlHumDB.UpdateRecord (INSERT TBL_QUEST)");
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
                        DBShare.MainOutMessage("[Exception] MySqlHumDB.UpdateRecord (DELETE TBL_MAGIC)");
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
                                DBShare.MainOutMessage("[Exception] MySqlHumDB.UpdateRecord (INSERT TBL_MAGIC)");
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
                        DBShare.MainOutMessage("[Exception] MySqlHumDB.UpdateRecord (DELETE TBL_ITEM)");
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
                                DBShare.MainOutMessage("[Exception] MySqlHumDB.UpdateRecord (INSERT TBL_ITEM)");
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
                                DBShare.MainOutMessage("[Exception] MySqlHumDB.UpdateRecord (13)");
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
                        DBShare.MainOutMessage("[Exception] MySqlHumDB.UpdateRecord (10)");
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
                                DBShare.MainOutMessage("[Exception] MySqlHumDB.UpdateRecord (11)");
                            }
                        }
                    }
                    command.CommandText = $"DELETE FROM TBL_ADDON WHERE FLD_CHARNAME='{HumanRCD.Header.sName}'";
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        result = false;
                        DBShare.MainOutMessage("[Exception] MySqlHumDB.UpdateRecord (DELETE TBL_ADDON)");
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
                    //    DBShare.MainOutMessage("[Exception] MySqlHumDB.UpdateRecord (INSERT TBL_ADDON (FLD_STATUS))");
                    //}
                }
            }
            finally
            {
                Close();
            }
            return result;
        }

        private bool InsertRecord(THumInfoData hd)
        {
            bool result = true;
            var strSql = new StringBuilder();
            strSql.AppendLine("INSERT INTO TBL_CHARACTER (FLD_ServerNum, FLD_LoginID, FLD_CharName, FLD_MapName, FLD_CX, FLD_CY, FLD_Level, FLD_Dir, FLD_Hair, FLD_Sex, FLD_Job, FLD_Gold, FLD_GamePoint, FLD_HomeMap,");
            strSql.AppendLine("FLD_HomeX, FLD_HomeY, FLD_PkPoint, FLD_ReLevel, FLD_AttatckMode, FLD_FightZoneDieCount, FLD_BodyLuck, FLD_IncHealth,FLD_IncSpell, FLD_IncHealing, FLD_CreditPoint, FLD_BonusPoint,");
            strSql.AppendLine("FLD_HungerStatus, FLD_PayMentPoint, FLD_LockLogon, FLD_MarryCount, FLD_AllowGroupReCall, FLD_GroupRcallTime, FLD_AllowGuildReCall, FLD_IsMaster, FLD_MasterName, FLD_DearName");
            strSql.AppendLine(",FLD_StoragePwd, FLD_Deleted, FLD_CREATEDATE, FLD_LASTUPDATE) VALUES ");
            strSql.AppendLine("(@FLD_ServerNum, @FLD_LoginID, @FLD_CharName, @FLD_MapName, @FLD_CX, @FLD_CY, @FLD_Level, @FLD_Dir, @FLD_Hair, @FLD_Sex, @FLD_Job, @FLD_Gold, @FLD_GamePoint, @FLD_HomeMap,");
            strSql.AppendLine("@FLD_HomeX, @FLD_HomeY, @FLD_PkPoint, @FLD_ReLevel, @FLD_AttatckMode, @FLD_FightZoneDieCount, @FLD_BodyLuck, @FLD_IncHealth,@FLD_IncSpell, @FLD_IncHealing, @FLD_CreditPoint, @FLD_BonusPoint,");
            strSql.AppendLine("@FLD_HungerStatus, @FLD_PayMentPoint, @FLD_LockLogon, @FLD_MarryCount, @FLD_AllowGroupReCall, @FLD_GroupRcallTime, @FLD_AllowGuildReCall, @FLD_IsMaster, @FLD_MasterName, @FLD_DearName");
            strSql.AppendLine(",@FLD_StoragePwd, @FLD_Deleted, now(), now()) ");

            var command = new MySqlCommand();
            if (!Open())
            {
                return false;
            }
            command.Parameters.AddWithValue("@FLD_ServerNum", 1);
            command.Parameters.AddWithValue("@FLD_LoginID", 1);
            command.Parameters.AddWithValue("@FLD_CharName", hd.sChrName);
            command.Parameters.AddWithValue("@FLD_MapName", hd.sCurMap);
            command.Parameters.AddWithValue("@FLD_CX", hd.wCurX);
            command.Parameters.AddWithValue("@FLD_CY", hd.wCurY);
            command.Parameters.AddWithValue("@FLD_Level", hd.Abil.Level);
            command.Parameters.AddWithValue("@FLD_Dir", hd.btDir);
            command.Parameters.AddWithValue("@FLD_Hair", hd.btHair);
            command.Parameters.AddWithValue("@FLD_Sex", hd.btSex);
            command.Parameters.AddWithValue("@FLD_Job", hd.btJob);
            command.Parameters.AddWithValue("@FLD_Gold", hd.nGold);
            command.Parameters.AddWithValue("@FLD_GamePoint", hd.nGamePoint);
            command.Parameters.AddWithValue("@FLD_HomeMap", hd.sHomeMap);
            command.Parameters.AddWithValue("@FLD_HomeX", hd.wHomeX);
            command.Parameters.AddWithValue("@FLD_HomeY", hd.wHomeY);
            command.Parameters.AddWithValue("@FLD_PkPoint", hd.nPKPoint);
            command.Parameters.AddWithValue("@FLD_ReLevel", hd.btReLevel);
            command.Parameters.AddWithValue("@FLD_AttatckMode", hd.btAttatckMode);
            command.Parameters.AddWithValue("@FLD_FightZoneDieCount", hd.btFightZoneDieCount);
            command.Parameters.AddWithValue("@FLD_BodyLuck", hd.dBodyLuck);
            command.Parameters.AddWithValue("@FLD_IncHealth", hd.btIncHealth);
            command.Parameters.AddWithValue("@FLD_IncSpell", hd.btIncSpell);
            command.Parameters.AddWithValue("@FLD_IncHealing", hd.btIncHealing);
            command.Parameters.AddWithValue("@FLD_CreditPoint", hd.btCreditPoint);
            command.Parameters.AddWithValue("@FLD_BonusPoint", hd.nBonusPoint);
            command.Parameters.AddWithValue("@FLD_HungerStatus", hd.nHungerStatus);
            command.Parameters.AddWithValue("@FLD_PayMentPoint", hd.nPayMentPoint);
            command.Parameters.AddWithValue("@FLD_LockLogon", hd.boLockLogon);
            command.Parameters.AddWithValue("@FLD_MarryCount", hd.btMarryCount);
            command.Parameters.AddWithValue("@FLD_AllowGroupReCall", hd.btAllowGroup);
            command.Parameters.AddWithValue("@FLD_GroupRcallTime", hd.wGroupRcallTime);
            command.Parameters.AddWithValue("@FLD_AllowGuildReCall", hd.boAllowGuildReCall);
            command.Parameters.AddWithValue("@FLD_IsMaster", hd.boMaster);
            command.Parameters.AddWithValue("@FLD_MasterName", hd.sMasterName);
            command.Parameters.AddWithValue("@FLD_DearName", hd.sDearName);
            command.Parameters.AddWithValue("@FLD_StoragePwd", hd.sStoragePwd);
            command.Parameters.AddWithValue("@FLD_Deleted", 0);
            command.CommandText = string.Format(strSql.ToString());
            command.Connection = (MySqlConnection)_dbConnection;
            try
            {
                command.ExecuteNonQuery();

                strSql.Clear();
                strSql.AppendLine("INSERT INTO TBL_CHARACTER_ABLITY (FLD_ChrId, FLD_Level, FLD_Ac, FLD_Mac, FLD_Dc, FLD_Mc, FLD_Sc, FLD_Hp, FLD_Mp, FLD_MaxHP, FLD_MAxMP, FLD_Exp, FLD_MaxExp,");
                strSql.AppendLine(" FLD_Weight, FLD_MaxWeight, FLD_WearWeight,FLD_MaxWearWeight, FLD_HandWeight, FLD_MaxHandWeight) VALUES ");
                strSql.AppendLine(" (@FLD_ChrId, @FLD_Level, @FLD_Ac, @FLD_Mac, @FLD_Dc, @FLD_Mc, @FLD_Sc, @FLD_Hp, @FLD_Mp, @FLD_MaxHP, @FLD_MAxMP, @FLD_Exp, @FLD_MaxExp, @FLD_Weight, @FLD_MaxWeight, @FLD_WearWeight, @FLD_MaxWearWeight, @FLD_HandWeight, @FLD_MaxHandWeight) ");

                command.CommandText = strSql.ToString();
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@FLD_ChrId", 1);
                command.Parameters.AddWithValue("@FLD_Level", hd.Abil.Level);
                command.Parameters.AddWithValue("@FLD_Ac", hd.Abil.Level);
                command.Parameters.AddWithValue("@FLD_Mac", hd.Abil.MAC);
                command.Parameters.AddWithValue("@FLD_Dc", hd.Abil.DC);
                command.Parameters.AddWithValue("@FLD_Mc", hd.Abil.MC);
                command.Parameters.AddWithValue("@FLD_Sc", hd.Abil.SC);
                command.Parameters.AddWithValue("@FLD_Hp", hd.Abil.HP);
                command.Parameters.AddWithValue("@FLD_Mp", hd.Abil.MP);
                command.Parameters.AddWithValue("@FLD_MaxHP", hd.Abil.MaxHP);
                command.Parameters.AddWithValue("@FLD_MAxMP", hd.Abil.MaxMP);
                command.Parameters.AddWithValue("@FLD_Exp", hd.Abil.Exp);
                command.Parameters.AddWithValue("@FLD_MaxExp", hd.Abil.MaxExp);
                command.Parameters.AddWithValue("@FLD_Weight", hd.Abil.Weight);
                command.Parameters.AddWithValue("@FLD_MaxWeight", hd.Abil.MaxWeight);
                command.Parameters.AddWithValue("@FLD_WearWeight", hd.Abil.WearWeight);
                command.Parameters.AddWithValue("@FLD_MaxWearWeight", hd.Abil.MaxWearWeight);
                command.Parameters.AddWithValue("@FLD_HandWeight", hd.Abil.HandWeight);
                command.Parameters.AddWithValue("@FLD_MaxHandWeight", hd.Abil.MaxHandWeight);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = false;
                DBShare.MainOutMessage("[Exception] MySqlHumDB.InsertRecord (1)");
                return result;
            }
            finally
            {
                Close();
            }
            return result;
        }

        private bool UpdateRecord(int Id, THumInfoData hd)
        {
            bool result = true;
            var dwHP = HUtil32.MakeLong(hd.Abil.HP, hd.Abil.AC);
            var dwMP = HUtil32.MakeLong(hd.Abil.MP, hd.Abil.MAC);
            var command = new MySqlCommand();
            if (!Open())
            {
                return false;
            }
            var strSql = new StringBuilder();
            strSql.AppendLine("UPDATE TBL_CHARACTER SET FLD_ServerNum = @FLD_ServerNum, FLD_LoginID = @FLD_LoginID, FLD_CharName = @FLD_CharName, FLD_MapName = @FLD_MapName, FLD_CX = @FLD_CX, FLD_CY = @FLD_CY, FLD_Level = @FLD_Level, FLD_Dir = @FLD_Dir, FLD_Hair = @FLD_Hair, FLD_Sex = @FLD_Sex, FLD_Job = FLD_Job, FLD_Gold = @FLD_Gold, ");
            strSql.AppendLine("FLD_GamePoint = @FLD_GamePoint, FLD_HomeMap = @FLD_HomeMap, FLD_HomeX = @FLD_HomeX, FLD_HomeY = @FLD_HomeY, FLD_PkPoint = @FLD_PkPoint, FLD_ReLevel = @FLD_ReLevel, FLD_AttatckMode = @FLD_AttatckMode, FLD_FightZoneDieCount = @FLD_FightZoneDieCount, FLD_BodyLuck = @FLD_BodyLuck, FLD_IncHealth = @FLD_IncHealth, FLD_IncSpell = @FLD_IncSpell,");
            strSql.AppendLine("FLD_IncHealing = @FLD_IncHealing, FLD_CreditPoint = @FLD_CreditPoint, FLD_BonusPoint =@FLD_BonusPoint, FLD_HungerStatus =@FLD_HungerStatus, FLD_PayMentPoint = @FLD_PayMentPoint, FLD_LockLogon = @FLD_LockLogon, FLD_MarryCount = @FLD_MarryCount, FLD_AllowGroupReCall = @FLD_AllowGroupReCall, ");
            strSql.AppendLine("FLD_GroupRcallTime = @FLD_GroupRcallTime, FLD_AllowGuildReCall = @FLD_AllowGuildReCall, FLD_IsMaster = @FLD_IsMaster, FLD_MasterName = @FLD_MasterName, FLD_DearName = @FLD_DearName, FLD_StoragePwd = @FLD_StoragePwd, FLD_Deleted = @FLD_Deleted,FLD_LASTUPDATE = now() WHERE Id = @Id;");

            command.Parameters.AddWithValue("@FLD_ServerNum", 1);
            command.Parameters.AddWithValue("@FLD_LoginID", 1);
            command.Parameters.AddWithValue("@FLD_CharName", hd.sChrName);
            command.Parameters.AddWithValue("@FLD_MapName", hd.sCurMap);
            command.Parameters.AddWithValue("@FLD_CX", hd.wCurX);
            command.Parameters.AddWithValue("@FLD_CY", hd.wCurY);
            command.Parameters.AddWithValue("@FLD_Level", hd.Abil.Level);
            command.Parameters.AddWithValue("@FLD_Dir", hd.btDir);
            command.Parameters.AddWithValue("@FLD_Hair", hd.btHair);
            command.Parameters.AddWithValue("@FLD_Sex", hd.btSex);
            command.Parameters.AddWithValue("@FLD_Job", hd.btJob);
            command.Parameters.AddWithValue("@FLD_Gold", hd.nGold);
            command.Parameters.AddWithValue("@FLD_GamePoint", hd.nGamePoint);
            command.Parameters.AddWithValue("@FLD_HomeMap", hd.sHomeMap);
            command.Parameters.AddWithValue("@FLD_HomeX", hd.wHomeX);
            command.Parameters.AddWithValue("@FLD_HomeY", hd.wHomeY);
            command.Parameters.AddWithValue("@FLD_PkPoint", hd.nPKPoint);
            command.Parameters.AddWithValue("@FLD_ReLevel", hd.btReLevel);
            command.Parameters.AddWithValue("@FLD_AttatckMode", hd.btAttatckMode);
            command.Parameters.AddWithValue("@FLD_FightZoneDieCount", hd.btFightZoneDieCount);
            command.Parameters.AddWithValue("@FLD_BodyLuck", hd.dBodyLuck);
            command.Parameters.AddWithValue("@FLD_IncHealth", hd.btIncHealth);
            command.Parameters.AddWithValue("@FLD_IncSpell", hd.btIncSpell);
            command.Parameters.AddWithValue("@FLD_IncHealing", hd.btIncHealing);
            command.Parameters.AddWithValue("@FLD_CreditPoint", hd.btCreditPoint);
            command.Parameters.AddWithValue("@FLD_BonusPoint", hd.nBonusPoint);
            command.Parameters.AddWithValue("@FLD_HungerStatus", hd.nHungerStatus);
            command.Parameters.AddWithValue("@FLD_PayMentPoint", hd.nPayMentPoint);
            command.Parameters.AddWithValue("@FLD_LockLogon", hd.boLockLogon);
            command.Parameters.AddWithValue("@FLD_MarryCount", hd.btMarryCount);
            command.Parameters.AddWithValue("@FLD_AllowGroupReCall", hd.btAllowGroup);
            command.Parameters.AddWithValue("@FLD_GroupRcallTime", hd.wGroupRcallTime);
            command.Parameters.AddWithValue("@FLD_AllowGuildReCall", hd.boAllowGuildReCall);
            command.Parameters.AddWithValue("@FLD_IsMaster", hd.boMaster);
            command.Parameters.AddWithValue("@FLD_MasterName", hd.sMasterName);
            command.Parameters.AddWithValue("@FLD_DearName", hd.sDearName);
            command.Parameters.AddWithValue("@FLD_StoragePwd", hd.sStoragePwd);
            command.Parameters.AddWithValue("@FLD_Deleted", 0);
            command.Parameters.AddWithValue("@Id", Id);
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result = false;
                DBShare.MainOutMessage("[Exception] MySqlHumDB.UpdateRecord");
                return result;
            }
            finally
            {
                Close();
            }
            return result;
        }

        public int Find(string sChrName, StringDictionary List)
        {
            int result;
            for (var i = 0; i < m_MirQuickList.Count; i++)
            {
                //if (HUtil32.CompareLStr(m_MirQuickList[i], sChrName, sChrName.Length))
                //{
                //    List.Add(m_MirQuickList[i], m_MirQuickList.Values[i]);
                //}
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
            string sChrName = m_QuickIndexNameList[nIndex];
            var command = new MySqlCommand();
            if (!Open())
            {
                return false;
            }
            command.CommandText = $"UPDATE TBL_CHARACTER SET FLD_DELETED=1, FLD_CREATEDATE=now() WHERE FLD_CHARNAME='{sChrName}'";
            command.Connection = (MySqlConnection)_dbConnection;
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result = false;
                DBShare.MainOutMessage("[Exception] MySqlHumDB.DeleteRecord");
            }
            finally
            {
                Close();
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
                if (!Open())
                {
                    return -1;
                }
                var command = new MySqlCommand();
                try
                {
                    command.CommandText = string.Format(sSQL, sChrName);
                    command.Connection = (MySqlConnection)_dbConnection;
                }
                catch (Exception)
                {
                    DBShare.MainOutMessage("[Exception] MySqlHumDB.GetQryChar (1)");
                    return result;
                }
                using var dr = command.ExecuteReader();
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
                Close();
            }
            result = nIndex;
            return result;
        }
    }
}