using DBSvr.Storage.Model;
using MySqlConnector;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using SystemModule;
using SystemModule.Packet.ClientPackets;
using SystemModule.Packet.ServerPackets;

namespace DBSvr.Storage.MariaDB
{
    public class PlayDataStorage : IPlayDataStorage
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<string, int> _mirQuickMap;
        private readonly Dictionary<int, int> _quickIndexIdMap;
        private readonly QuickIdList _mirQuickIdList;
        private readonly StorageOption _storageOption;
        private MySqlConnection _connection;
        private MySqlTransaction _transaction;
        private int _recordCount;

        public PlayDataStorage(StorageOption storageOption)
        {
            _mirQuickMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            _mirQuickIdList = new QuickIdList();
            _recordCount = -1;
            _quickIndexIdMap = new Dictionary<int, int>();
            _storageOption = storageOption;
        }

        public void LoadQuickList()
        {
            const string sSqlString = "SELECT * FROM characters WHERE Deleted=0";
            _quickIndexIdMap.Clear();
            _mirQuickMap.Clear();
            _mirQuickIdList.Clear();
            bool boDeleted;
            string sAccount;
            string sChrName;
            _recordCount = -1;
            IList<QuickId> accountList = new List<QuickId>();
            IList<string> chrNameList = new List<string>();
            var success = false;
            Open(ref success);
            if (!success)
            {
                return;
            }
            try
            {
                var command = new MySqlCommand();
                command.CommandText = sSqlString;
                command.Connection = _connection;
                using var dr = command.ExecuteReader();
                var nIndex = 0;
                while (dr.Read())
                {
                    nIndex = dr.GetInt32("Id");
                    boDeleted = dr.GetBoolean("Deleted");
                    sAccount = dr.GetString("LoginID");
                    sChrName = dr.GetString("CharName");
                    if (!boDeleted && (!string.IsNullOrEmpty(sChrName)))
                    {
                        _mirQuickMap.Add(sChrName, nIndex);
                        accountList.Add(new QuickId()
                        {
                            sAccount = sAccount,
                            nSelectID = 0
                        });
                        chrNameList.Add(sChrName);
                        _quickIndexIdMap.Add(nIndex, nIndex);
                    }
                }
            }
            finally
            {
                Close(_connection);
            }
            for (var nIndex = 0; nIndex < accountList.Count; nIndex++)
            {
                _mirQuickIdList.AddRecord(accountList[nIndex].sAccount, chrNameList[nIndex], 0, accountList[nIndex].nSelectID);
            }
            accountList = null;
            chrNameList = null;
            //m_MirQuickList.SortString(0, m_MirQuickList.Count - 1);
        }

        private void Open(ref bool success)
        {
            _connection = new MySqlConnection(_storageOption.ConnectionString);
            try
            {
                _connection.Open();
                success = true;
            }
            catch (Exception e)
            {
                _logger.Error("打开数据库[MySql]失败.");
                _logger.Error(e.StackTrace);
                success = false;
            }
        }

        private void Close(MySqlConnection _connection)
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        public int Index(string sName)
        {
            if (_mirQuickMap.ContainsKey(sName))
            {
                return _mirQuickMap[sName];
            }
            return -1;
        }

        public int Get(int nIndex, ref THumDataInfo humanRcd)
        {
            var result = -1;
            if (nIndex < 0)
            {
                return result;
            }
            if (_mirQuickMap.Count < nIndex)
            {
                return result;
            }
            if (GetRecord(nIndex, ref humanRcd))
            {
                result = nIndex;
            }
            return result;
        }

        public bool Get(string chrName, ref THumDataInfo humanRcd)
        {
            if (string.IsNullOrEmpty(chrName))
            {
                return false;
            }
            if (_mirQuickMap.TryGetValue(chrName, out var nIndex))
            {
                if (GetRecord(nIndex, ref humanRcd))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool Update(string chrName, ref THumDataInfo humanRcd)
        {
            if (_mirQuickMap.TryGetValue(chrName, out var playerId))
            {
                if (UpdateRecord(playerId, ref humanRcd))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool UpdateQryChar(int nIndex, QueryChr queryChrRcd)
        {
            var result = false;
            if ((nIndex >= 0) && (_mirQuickMap.Count > nIndex))
            {
                if (UpdateChrRecord(nIndex, queryChrRcd))
                {
                    result = true;
                }
            }
            return result;
        }

        private bool UpdateChrRecord(int playerId, QueryChr queryChrRcd)
        {
            const string sStrString = "UPDATE characters SET Sex=@Sex, Job=@Job WHERE ID=@Id";
            var result = false;
            Open(ref result);
            try
            {
                if (!result)
                {
                    return false;
                }
                var command = new MySqlCommand();
                command.CommandText = sStrString;
                command.Parameters.AddWithValue("@Sex", queryChrRcd.btSex);
                command.Parameters.AddWithValue("@Job", queryChrRcd.btJob);
                command.Parameters.AddWithValue("@Id", playerId);
                command.Connection = _connection;
                try
                {
                    command.ExecuteNonQuery();
                    result = true;
                }
                catch
                {
                    _logger.Error("[Exception] UpdateChrRecord");
                    result = false;
                }
            }
            finally
            {
                Close(_connection);
            }
            return result;
        }

        public bool Add(ref THumDataInfo humanRcd)
        {
            var result = false;
            int nIndex;
            var sChrName = humanRcd.Header.sName;
            if (_mirQuickMap.TryGetValue(sChrName, out nIndex))
            {
                if (nIndex >= 0)
                {
                    return false;
                }
            }
            else
            {
                nIndex = _recordCount;
                _recordCount++;
                if (AddRecord(ref nIndex, ref humanRcd))
                {
                    _mirQuickMap.Add(sChrName, nIndex);
                    _quickIndexIdMap.Add(nIndex, nIndex);
                    result = true;
                }
            }
            return result;
        }

        private bool GetRecord(int nIndex, ref THumDataInfo humanRcd)
        {
            var playerId = 0;
            if (humanRcd == null)
            {
                playerId = _quickIndexIdMap[nIndex];
            }
            if (playerId == 0)
            {
                return false;
            }
            var success = false;
            Open(ref success);
            if (!success)
            {
                return false;
            }
            try
            {
                GetChrRecord(playerId, ref humanRcd);
                GetAbilGetRecord(playerId, ref humanRcd);
                GetBonusAbilRecord(playerId, ref humanRcd);
                GetMagicRecord(playerId, ref humanRcd);
                GetItemRecord(playerId, ref humanRcd);
                GetBagItemRecord(playerId, ref humanRcd);
                GetStorageRecord(playerId, ref humanRcd);
                GetPlayerStatus(playerId, ref humanRcd);
            }
            catch (Exception e)
            {
                _logger.Error("获取角色数据失败." + e.StackTrace);
                return false;
            }
            return true;
        }

        private void GetChrRecord(int playerId, ref THumDataInfo humanRcd)
        {
            const string sSqlString = "SELECT * FROM characters WHERE ID=@ID";
            var command = new MySqlCommand();
            try
            {
                command.CommandText = sSqlString;
                command.Parameters.AddWithValue("@Id", playerId);
                command.Connection = _connection;
                using var dr = command.ExecuteReader();
                while (dr.Read())
                {
                    humanRcd = new THumDataInfo();
                    humanRcd.Data.Initialization();
                    humanRcd.Data.Account = dr.GetString("LOGINID");
                    humanRcd.Header.sName = dr.GetString("CharName");
                    humanRcd.Header.Deleted = dr.GetBoolean("DELETED");
                    humanRcd.Header.dCreateDate = HUtil32.DateTimeToDouble(dr.GetDateTime("CREATEDATE"));
                    humanRcd.Data.sCharName = dr.GetString("CharName");
                    if (!dr.IsDBNull(dr.GetOrdinal("MapName")))
                    {
                        humanRcd.Data.sCurMap = dr.GetString("MapName");
                    }
                    humanRcd.Data.CurX = dr.GetInt16("CX");
                    humanRcd.Data.CurY = dr.GetInt16("CY");
                    humanRcd.Data.Dir = dr.GetByte("DIR");
                    humanRcd.Data.btHair = dr.GetByte("HAIR");
                    humanRcd.Data.Sex = dr.GetByte("SEX");
                    humanRcd.Data.Job = dr.GetByte("JOB");
                    humanRcd.Data.nGold = dr.GetInt32("Gold");
                    if (!dr.IsDBNull(dr.GetOrdinal("HomeMap")))
                    {
                        humanRcd.Data.sHomeMap = dr.GetString("HomeMap");
                    }
                    humanRcd.Data.wHomeX = dr.GetInt16("HOMEX");
                    humanRcd.Data.wHomeY = dr.GetInt16("HOMEY");
                    if (!dr.IsDBNull(dr.GetOrdinal("DearName")))
                    {
                        humanRcd.Data.sDearName = dr.GetString("DearName");
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("MasterName")))
                    {
                        humanRcd.Data.sMasterName = dr.GetString("MasterName");
                    }
                    humanRcd.Data.boMaster = dr.GetBoolean("IsMaster");
                    humanRcd.Data.btCreditPoint = (byte)dr.GetInt32("CREDITPOINT");
                    if (!dr.IsDBNull(dr.GetOrdinal("StoragePwd")))
                    {
                        humanRcd.Data.sStoragePwd = dr.GetString("StoragePwd");
                    }
                    humanRcd.Data.btReLevel = dr.GetByte("ReLevel");
                    humanRcd.Data.boLockLogon = dr.GetBoolean("LOCKLOGON");
                    humanRcd.Data.nBonusPoint = dr.GetInt32("BONUSPOINT");
                    humanRcd.Data.nGameGold = dr.GetInt32("Gold");
                    humanRcd.Data.nGamePoint = dr.GetInt32("GamePoint");
                    humanRcd.Data.nPayMentPoint = dr.GetInt32("PayMentPoint");
                    humanRcd.Data.nHungerStatus = dr.GetInt32("HungerStatus");
                    humanRcd.Data.btAllowGroup = (byte)dr.GetInt32("AllowGroup");
                    humanRcd.Data.btAttatckMode = dr.GetByte("AttatckMode");
                    humanRcd.Data.btIncHealth = dr.GetByte("IncHealth");
                    humanRcd.Data.btIncSpell = dr.GetByte("IncSpell");
                    humanRcd.Data.btIncHealing = dr.GetByte("IncHealing");
                    humanRcd.Data.btFightZoneDieCount = dr.GetByte("FightZoneDieCount");
                    humanRcd.Data.boAllowGuildReCall = dr.GetBoolean("AllowGuildReCall");
                    humanRcd.Data.boAllowGroupReCall = dr.GetBoolean("AllowGroupReCall");
                    humanRcd.Data.wGroupRcallTime = dr.GetInt16("GroupRcallTime");
                    humanRcd.Data.dBodyLuck = dr.GetDouble("BodyLuck");
                }
                dr.Close();
                dr.Dispose();
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.GetChrRecord");
                _logger.Error(ex.StackTrace);
            }
        }

        private void GetAbilGetRecord(int playerId, ref THumDataInfo humanRcd)
        {
            int dw;
            try
            {
                var command = new MySqlCommand();
                command.CommandText = "select * from CHARACTER_ABLITY where playerId=@playerId";
                command.Parameters.AddWithValue("@playerId", playerId);
                command.Connection = _connection;
                using var dr = command.ExecuteReader();
                if (dr.Read())
                {
                    humanRcd.Data.Abil.Level = dr.GetByte("Level");
                    dw = dr.GetInt32("HP");
                    humanRcd.Data.Abil.HP = HUtil32.LoWord(dw);
                    humanRcd.Data.Abil.AC = HUtil32.HiWord(dw);
                    dw = dr.GetInt32("MP");
                    humanRcd.Data.Abil.MP = HUtil32.LoWord(dw);
                    humanRcd.Data.Abil.MAC = HUtil32.HiWord(dw);
                    humanRcd.Data.Abil.DC = dr.GetUInt16("DC");
                    humanRcd.Data.Abil.MC = dr.GetUInt16("MC");
                    humanRcd.Data.Abil.SC = dr.GetUInt16("SC");
                    humanRcd.Data.Abil.Exp = dr.GetInt32("EXP");
                    humanRcd.Data.Abil.MaxExp = dr.GetInt32("MaxExp");
                    humanRcd.Data.Abil.Weight = dr.GetUInt16("Weight");
                    humanRcd.Data.Abil.MaxWeight = dr.GetUInt16("MaxWeight");
                    humanRcd.Data.Abil.WearWeight = dr.GetByte("WearWeight");
                    humanRcd.Data.Abil.MaxWearWeight = dr.GetByte("MaxWearWeight");
                    humanRcd.Data.Abil.HandWeight = dr.GetByte("HandWeight");
                    humanRcd.Data.Abil.MaxHandWeight = dr.GetByte("MaxHandWeight");
                }
                dr.Close();
                dr.Dispose();
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.GetAbilGetRecord");
                _logger.Error(ex.StackTrace);
            }
        }

        private void GetBonusAbilRecord(int playerId, ref THumDataInfo humanRcd)
        {
            const string sSqlString = "SELECT * FROM characters_bonusability WHERE PlayerId=@PlayerId";
            var command = new MySqlCommand();
            command.Connection = _connection;
            try
            {
                command.CommandText = sSqlString;
                command.Parameters.AddWithValue("@PlayerId", playerId);
                using var dr = command.ExecuteReader();
                if (dr.Read())
                {
                    if (humanRcd.Data.BonusAbil == null)
                    {
                        humanRcd.Data.BonusAbil = new NakedAbility();
                    }
                    humanRcd.Data.BonusAbil.AC = dr.GetUInt16("AC");
                    humanRcd.Data.BonusAbil.MAC = dr.GetUInt16("MAC");
                    humanRcd.Data.BonusAbil.DC = dr.GetUInt16("DC");
                    humanRcd.Data.BonusAbil.MC = dr.GetUInt16("MC");
                    humanRcd.Data.BonusAbil.SC = dr.GetUInt16("SC");
                    humanRcd.Data.BonusAbil.HP = dr.GetUInt16("HP");
                    humanRcd.Data.BonusAbil.MP = dr.GetUInt16("MP");
                    humanRcd.Data.BonusAbil.Hit = dr.GetByte("HIT");
                    humanRcd.Data.BonusAbil.Speed = dr.GetInt32("SPEED");
                    humanRcd.Data.BonusAbil.Reserved = dr.GetByte("RESERVED");
                }
                dr.Close();
                dr.Dispose();
            }
            catch (Exception)
            {
                _logger.Error("[Exception] PlayDataStorage.GetBonusAbilRecord");
            }
        }

        private void GetMagicRecord(int playerId, ref THumDataInfo humanRcd)
        {
            const string sSqlString = "SELECT * FROM characters_magic WHERE PlayerId=@PlayerId";
            var command = new MySqlCommand();
            try
            {
                for (var i = 0; i < humanRcd.Data.Magic.Length; i++)
                {
                    humanRcd.Data.Magic[i] = new MagicRcd();
                }
                command.Connection = _connection;
                command.CommandText = sSqlString;
                command.Parameters.AddWithValue("@PlayerId", playerId);
                using var dr = command.ExecuteReader();
                var position = 0;
                while (dr.Read())
                {
                    humanRcd.Data.Magic[position].MagIdx = dr.GetUInt16("MAGICID");
                    humanRcd.Data.Magic[position].MagicKey = dr.GetChar("USEKEY");
                    humanRcd.Data.Magic[position].Level = (byte)dr.GetInt32("Level");
                    humanRcd.Data.Magic[position].TranPoint = dr.GetInt32("CURRTRAIN");
                    position++;
                }
                dr.Close();
                dr.Dispose();
            }
            catch (Exception ex)
            {
                _logger.Error($"[Exception] GetPlayMagicRecord ChrId:{playerId}");
                _logger.Error(ex.StackTrace);
            }
        }

        private void GetItemRecord(int playerId, ref THumDataInfo humanRcd)
        {
            const string sSqlString = "SELECT * FROM characters_item WHERE PlayerId=@PlayerId";
            var command = new MySqlCommand();
            try
            {
                command.Connection = _connection;
                command.Parameters.AddWithValue("@PlayerId", playerId);
                command.CommandText = sSqlString;
                using var dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var nPosition = dr.GetInt32("Position");
                    humanRcd.Data.HumItems[nPosition] = new UserItem();
                    humanRcd.Data.HumItems[nPosition].MakeIndex = dr.GetInt32("MakeIndex");
                    humanRcd.Data.HumItems[nPosition].Index = dr.GetUInt16("StdIndex");
                    humanRcd.Data.HumItems[nPosition].Dura = dr.GetUInt16("Dura");
                    humanRcd.Data.HumItems[nPosition].DuraMax = dr.GetUInt16("DuraMax");
                }
                QueryItemAttr(playerId, ref humanRcd.Data.HumItems);
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.GetItemRecord:" + ex.StackTrace);
            }
        }

        private void GetBagItemRecord(int playerId, ref THumDataInfo humanRcd)
        {
            const string sSqlString = "SELECT * FROM characters_bagitem WHERE PlayerId=@PlayerId";
            var command = new MySqlCommand();
            try
            {
                command.Connection = _connection;
                command.Parameters.AddWithValue("@PlayerId", playerId);
                command.CommandText = sSqlString;
                using var dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var nPosition = dr.GetInt32("Position");
                    humanRcd.Data.BagItems[nPosition] = new UserItem();
                    humanRcd.Data.BagItems[nPosition].MakeIndex = dr.GetInt32("MakeIndex");
                    humanRcd.Data.BagItems[nPosition].Index = dr.GetUInt16("StdIndex");
                    humanRcd.Data.BagItems[nPosition].Dura = dr.GetUInt16("Dura");
                    humanRcd.Data.BagItems[nPosition].DuraMax = dr.GetUInt16("DuraMax");
                }
                QueryItemAttr(playerId, ref humanRcd.Data.BagItems);
            }
            catch
            {
                _logger.Error("[Exception] PlayDataStorage.GetBagItemRecord");
            }
        }

        private void GetStorageRecord(int playerId, ref THumDataInfo humanRcd)
        {
            const string sSqlString = "SELECT * FROM characters_storageitem WHERE PlayerId=@PlayerId";
            var command = new MySqlCommand();
            try
            {
                command.CommandText = sSqlString;
                command.Parameters.AddWithValue("@PlayerId", playerId);
                command.Connection = _connection;
                using  var dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var nPosition = dr.GetInt32("Position");
                    humanRcd.Data.StorageItems[nPosition] = new UserItem();
                    humanRcd.Data.StorageItems[nPosition].MakeIndex = dr.GetInt32("MakeIndex");
                    humanRcd.Data.StorageItems[nPosition].Index = dr.GetUInt16("StdIndex");
                    humanRcd.Data.StorageItems[nPosition].Dura = dr.GetUInt16("Dura");
                    humanRcd.Data.StorageItems[nPosition].DuraMax = dr.GetUInt16("DuraMax");
                }
                QueryItemAttr(playerId, ref humanRcd.Data.StorageItems);
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.GetStorageRecord");
                _logger.Error(ex.StackTrace);
            }
        }

        private void GetPlayerStatus(int playerId, ref THumDataInfo humanRcd)
        {
            const string sSqlString = "SELECT * FROM characters_status WHERE PlayerId=@PlayerId";
            var command = new MySqlCommand();
            command.Connection = _connection;
            try
            {
                command.Parameters.AddWithValue("@PlayerId", playerId);
                command.CommandText = sSqlString;
                using var dr = command.ExecuteReader();
                if (dr.Read())
                {
                    var sTmp = dr.GetString("STATUS");
                    var i = 0;
                    var str = string.Empty;
                    while (!string.IsNullOrEmpty(sTmp))
                    {
                        sTmp = HUtil32.GetValidStr3(sTmp, ref str, new[] { "/" });
                        humanRcd.Data.StatusTimeArr[i] = Convert.ToUInt16(str);
                        i++;
                        if (i > humanRcd.Data.StatusTimeArr.Length)
                        {
                            break;
                        }
                    }
                }
                dr.Close();
                dr.Dispose();
            }
            catch (Exception)
            {
                _logger.Error("[Exception] PlayDataStorage.GetPlayerStatus");
            }
        }

        private bool AddRecord(ref int nIndex, ref THumDataInfo humanRcd)
        {
            var success = false;
            Open(ref success);
            if (!success)
            {
                return false;
            }
            var result = false;
            try
            {
                result = CreateRecord(humanRcd.Data, ref nIndex);
            }
            catch (Exception e)
            {
                _logger.Error("创建角色失败" + e.StackTrace);
            }
            finally
            {
                Close(_connection);
            }
            return result;
        }

        private bool CreateRecord(THumInfoData hd, ref int nIndex)
        {
            var strSql = new StringBuilder();
            strSql.AppendLine("INSERT INTO CHARACTERS (ServerIndex, LoginID, CharName, MapName, CX, CY, Level, Dir, Hair, Sex, Job, Gold, GamePoint, HomeMap,");
            strSql.AppendLine("HomeX, HomeY, PkPoint, ReLevel, AttatckMode, FightZoneDieCount, BodyLuck, IncHealth,IncSpell, IncHealing, CreditPoint, BonusPoint,");
            strSql.AppendLine("HungerStatus, PayMentPoint, LockLogon, MarryCount, AllowGroupReCall, GroupRcallTime, AllowGuildReCall, IsMaster, MasterName, DearName");
            strSql.AppendLine(",StoragePwd, Deleted, CREATEDATE, LASTUPDATE) VALUES ");
            strSql.AppendLine("(@ServerIndex, @LoginID, @CharName, @MapName, @CX, @CY, @Level, @Dir, @Hair, @Sex, @Job, @Gold, @GamePoint, @HomeMap,");
            strSql.AppendLine("@HomeX, @HomeY, @PkPoint, @ReLevel, @AttatckMode, @FightZoneDieCount, @BodyLuck, @IncHealth,@IncSpell, @IncHealing, @CreditPoint, @BonusPoint,");
            strSql.AppendLine("@HungerStatus, @PayMentPoint, @LockLogon, @MarryCount, @AllowGroupReCall, @GroupRcallTime, @AllowGuildReCall, @IsMaster, @MasterName, @DearName");
            strSql.AppendLine(",@StoragePwd, @Deleted, now(), now()) ");
            var command = new MySqlCommand();
            command.Connection = _connection;
            command.Parameters.AddWithValue("@ServerIndex", hd.ServerIndex);
            command.Parameters.AddWithValue("@LoginID", hd.Account);
            command.Parameters.AddWithValue("@CharName", hd.sCharName);
            command.Parameters.AddWithValue("@MapName", hd.sCurMap);
            command.Parameters.AddWithValue("@CX", hd.CurX);
            command.Parameters.AddWithValue("@CY", hd.CurY);
            command.Parameters.AddWithValue("@Level", hd.Abil.Level);
            command.Parameters.AddWithValue("@Dir", hd.Dir);
            command.Parameters.AddWithValue("@Hair", hd.btHair);
            command.Parameters.AddWithValue("@Sex", hd.Sex);
            command.Parameters.AddWithValue("@Job", hd.Job);
            command.Parameters.AddWithValue("@Gold", hd.nGold);
            command.Parameters.AddWithValue("@GamePoint", hd.nGamePoint);
            command.Parameters.AddWithValue("@HomeMap", hd.sHomeMap);
            command.Parameters.AddWithValue("@HomeX", hd.wHomeX);
            command.Parameters.AddWithValue("@HomeY", hd.wHomeY);
            command.Parameters.AddWithValue("@PkPoint", hd.nPKPoint);
            command.Parameters.AddWithValue("@ReLevel", hd.btReLevel);
            command.Parameters.AddWithValue("@AttatckMode", hd.btAttatckMode);
            command.Parameters.AddWithValue("@FightZoneDieCount", hd.btFightZoneDieCount);
            command.Parameters.AddWithValue("@BodyLuck", hd.dBodyLuck);
            command.Parameters.AddWithValue("@IncHealth", hd.btIncHealth);
            command.Parameters.AddWithValue("@IncSpell", hd.btIncSpell);
            command.Parameters.AddWithValue("@IncHealing", hd.btIncHealing);
            command.Parameters.AddWithValue("@CreditPoint", hd.btCreditPoint);
            command.Parameters.AddWithValue("@BonusPoint", hd.nBonusPoint);
            command.Parameters.AddWithValue("@HungerStatus", hd.nHungerStatus);
            command.Parameters.AddWithValue("@PayMentPoint", hd.nPayMentPoint);
            command.Parameters.AddWithValue("@LockLogon", hd.boLockLogon);
            command.Parameters.AddWithValue("@MarryCount", hd.MarryCount);
            command.Parameters.AddWithValue("@AllowGroupReCall", hd.btAllowGroup);
            command.Parameters.AddWithValue("@GroupRcallTime", hd.wGroupRcallTime);
            command.Parameters.AddWithValue("@AllowGuildReCall", hd.boAllowGuildReCall);
            command.Parameters.AddWithValue("@IsMaster", hd.boMaster);
            command.Parameters.AddWithValue("@MasterName", hd.sMasterName);
            command.Parameters.AddWithValue("@DearName", hd.sDearName);
            command.Parameters.AddWithValue("@StoragePwd", hd.sStoragePwd);
            command.Parameters.AddWithValue("@Deleted", 0);
            command.CommandText = strSql.ToString();
            try
            {
                command.ExecuteNonQuery();
                nIndex = (int)command.LastInsertedId;

                strSql.Clear();
                strSql.AppendLine("INSERT INTO characters_ablity (PlayerId, Level, Ac, Mac, Dc, Mc, Sc, Hp, Mp, MaxHP, MAxMP, Exp, MaxExp,");
                strSql.AppendLine(" Weight, MaxWeight, WearWeight,MaxWearWeight, HandWeight, MaxHandWeight) VALUES ");
                strSql.AppendLine(" (@PlayerId, @Level, @Ac, @Mac, @Dc, @Mc, @Sc, @Hp, @Mp, @MaxHP, @MAxMP, @Exp, @MaxExp, @Weight, @MaxWeight, @WearWeight, @MaxWearWeight, @HandWeight, @MaxHandWeight) ");

                command.CommandText = strSql.ToString();
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@PlayerId", nIndex);
                command.Parameters.AddWithValue("@Level", hd.Abil.Level);
                command.Parameters.AddWithValue("@Ac", hd.Abil.Level);
                command.Parameters.AddWithValue("@Mac", hd.Abil.MAC);
                command.Parameters.AddWithValue("@Dc", hd.Abil.DC);
                command.Parameters.AddWithValue("@Mc", hd.Abil.MC);
                command.Parameters.AddWithValue("@Sc", hd.Abil.SC);
                command.Parameters.AddWithValue("@Hp", hd.Abil.HP);
                command.Parameters.AddWithValue("@Mp", hd.Abil.MP);
                command.Parameters.AddWithValue("@MaxHP", hd.Abil.MaxHP);
                command.Parameters.AddWithValue("@MAxMP", hd.Abil.MaxMP);
                command.Parameters.AddWithValue("@Exp", hd.Abil.Exp);
                command.Parameters.AddWithValue("@MaxExp", hd.Abil.MaxExp);
                command.Parameters.AddWithValue("@Weight", hd.Abil.Weight);
                command.Parameters.AddWithValue("@MaxWeight", hd.Abil.MaxWeight);
                command.Parameters.AddWithValue("@WearWeight", hd.Abil.WearWeight);
                command.Parameters.AddWithValue("@MaxWearWeight", hd.Abil.MaxWearWeight);
                command.Parameters.AddWithValue("@HandWeight", hd.Abil.HandWeight);
                command.Parameters.AddWithValue("@MaxHandWeight", hd.Abil.MaxHandWeight);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.InsertRecord");
                _logger.Error(ex.StackTrace);
                return false;
            }
            return true;
        }

        private bool UpdateRecord(int playerId, ref THumDataInfo humanRcd)
        {
            var success = false;
            Open(ref success);
            if (!success)
            {
                return false;
            }
            _transaction = _connection.BeginTransaction();
            var result = true;
            try
            {
                SaveRecord(playerId, humanRcd);
                SaveAblity(playerId, humanRcd);
                SaveItem(playerId, humanRcd);
                SaveBagItem(playerId, humanRcd);
                SaveStorageItem(playerId, humanRcd);
                SaveMagics(playerId, humanRcd.Data.Magic);
                SaveStatus(playerId, humanRcd);
                SaveBonusability(playerId, humanRcd);
                _transaction.Commit();
            }
            catch (Exception ex)
            {
                result = false;
                _transaction.Rollback();
                _logger.Error($"保存玩家[{humanRcd.Header.sName}]数据失败. " + ex.Message);
            }
            finally
            {
                Close(_connection);
            }
            return result;
        }

        private void SaveRecord(int playerId, THumDataInfo humanRcd)
        {
            var hd = humanRcd.Data;
            var strSql = new StringBuilder();
            strSql.AppendLine("UPDATE characters SET ServerIndex = @ServerIndex, LoginID = @LoginID,MapName = @MapName, CX = @CX, CY = @CY, Level = @Level, Dir = @Dir, Hair = @Hair, Sex = @Sex, Job = Job, Gold = @Gold, ");
            strSql.AppendLine("GamePoint = @GamePoint, HomeMap = @HomeMap, HomeX = @HomeX, HomeY = @HomeY, PkPoint = @PkPoint, ReLevel = @ReLevel, AttatckMode = @AttatckMode, FightZoneDieCount = @FightZoneDieCount, BodyLuck = @BodyLuck, IncHealth = @IncHealth, IncSpell = @IncSpell,");
            strSql.AppendLine("IncHealing = @IncHealing, CreditPoint = @CreditPoint, BonusPoint =@BonusPoint, HungerStatus =@HungerStatus, PayMentPoint = @PayMentPoint, LockLogon = @LockLogon, MarryCount = @MarryCount, AllowGroupReCall = @AllowGroupReCall, ");
            strSql.AppendLine("GroupRcallTime = @GroupRcallTime, AllowGuildReCall = @AllowGuildReCall, IsMaster = @IsMaster, MasterName = @MasterName, DearName = @DearName, StoragePwd = @StoragePwd, Deleted = @Deleted,LASTUPDATE = now() WHERE ID = @ID;");
            var command = new MySqlCommand();
            command.CommandText = strSql.ToString();
            command.Connection = _connection;
            command.Transaction = _transaction;
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@Id", playerId);
            command.Parameters.AddWithValue("@ServerIndex", hd.ServerIndex);
            command.Parameters.AddWithValue("@LoginID", hd.Account);
            command.Parameters.AddWithValue("@MapName", hd.sCurMap);
            command.Parameters.AddWithValue("@CX", hd.CurX);
            command.Parameters.AddWithValue("@CY", hd.CurY);
            command.Parameters.AddWithValue("@Level", hd.Abil.Level);
            command.Parameters.AddWithValue("@Dir", hd.Dir);
            command.Parameters.AddWithValue("@Hair", hd.btHair);
            command.Parameters.AddWithValue("@Sex", hd.Sex);
            command.Parameters.AddWithValue("@Job", hd.Job);
            command.Parameters.AddWithValue("@Gold", hd.nGold);
            command.Parameters.AddWithValue("@GamePoint", hd.nGamePoint);
            command.Parameters.AddWithValue("@HomeMap", hd.sHomeMap);
            command.Parameters.AddWithValue("@HomeX", hd.wHomeX);
            command.Parameters.AddWithValue("@HomeY", hd.wHomeY);
            command.Parameters.AddWithValue("@PkPoint", hd.nPKPoint);
            command.Parameters.AddWithValue("@ReLevel", hd.btReLevel);
            command.Parameters.AddWithValue("@AttatckMode", hd.btAttatckMode);
            command.Parameters.AddWithValue("@FightZoneDieCount", hd.btFightZoneDieCount);
            command.Parameters.AddWithValue("@BodyLuck", hd.dBodyLuck);
            command.Parameters.AddWithValue("@IncHealth", hd.btIncHealth);
            command.Parameters.AddWithValue("@IncSpell", hd.btIncSpell);
            command.Parameters.AddWithValue("@IncHealing", hd.btIncHealing);
            command.Parameters.AddWithValue("@CreditPoint", hd.btCreditPoint);
            command.Parameters.AddWithValue("@BonusPoint", hd.nBonusPoint);
            command.Parameters.AddWithValue("@HungerStatus", hd.nHungerStatus);
            command.Parameters.AddWithValue("@PayMentPoint", hd.nPayMentPoint);
            command.Parameters.AddWithValue("@LockLogon", hd.boLockLogon);
            command.Parameters.AddWithValue("@MarryCount", hd.MarryCount);
            command.Parameters.AddWithValue("@AllowGroupReCall", hd.btAllowGroup);
            command.Parameters.AddWithValue("@GroupRcallTime", hd.wGroupRcallTime);
            command.Parameters.AddWithValue("@AllowGuildReCall", hd.boAllowGuildReCall);
            command.Parameters.AddWithValue("@IsMaster", hd.boMaster);
            command.Parameters.AddWithValue("@MasterName", hd.sMasterName);
            command.Parameters.AddWithValue("@DearName", hd.sDearName);
            command.Parameters.AddWithValue("@StoragePwd", hd.sStoragePwd);
            command.Parameters.AddWithValue("@Deleted", 0);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.UpdateRecord:" + ex.Message);
            }
        }

        private void SaveAblity(int playerId, THumDataInfo humanRcd)
        {
            var hd = humanRcd.Data;
            var strSql = new StringBuilder();
            strSql.AppendLine("UPDATE characters_ablity SET Level = @Level,");
            strSql.AppendLine("Ac = @Ac, Mac = @Mac, Dc = @Dc, Mc = @Mc, Sc = @Sc, Hp = @Hp, Mp = @Mp, MaxHP = @MaxHP,");
            strSql.AppendLine("MAxMP = @MAxMP, Exp = @Exp, MaxExp = @MaxExp, Weight = @Weight, MaxWeight = @MaxWeight, WearWeight = @WearWeight,");
            strSql.AppendLine("MaxWearWeight = @MaxWearWeight, HandWeight = @HandWeight, MaxHandWeight = @MaxHandWeight,ModifyTime=now() WHERE PlayerId = @PlayerId;");
            var command = new MySqlCommand();
            command.Connection = _connection;
            command.Transaction = _transaction;
            command.CommandText = strSql.ToString();
            command.Parameters.AddWithValue("@PlayerId", playerId);
            command.Parameters.AddWithValue("@Level", hd.Abil.Level);
            command.Parameters.AddWithValue("@Ac", hd.Abil.Level);
            command.Parameters.AddWithValue("@Mac", hd.Abil.MAC);
            command.Parameters.AddWithValue("@Dc", hd.Abil.DC);
            command.Parameters.AddWithValue("@Mc", hd.Abil.MC);
            command.Parameters.AddWithValue("@Sc", hd.Abil.SC);
            command.Parameters.AddWithValue("@Hp", hd.Abil.HP);
            command.Parameters.AddWithValue("@Mp", hd.Abil.MP);
            command.Parameters.AddWithValue("@MaxHP", hd.Abil.MaxHP);
            command.Parameters.AddWithValue("@MAxMP", hd.Abil.MaxMP);
            command.Parameters.AddWithValue("@Exp", hd.Abil.Exp);
            command.Parameters.AddWithValue("@MaxExp", hd.Abil.MaxExp);
            command.Parameters.AddWithValue("@Weight", hd.Abil.Weight);
            command.Parameters.AddWithValue("@MaxWeight", hd.Abil.MaxWeight);
            command.Parameters.AddWithValue("@WearWeight", hd.Abil.WearWeight);
            command.Parameters.AddWithValue("@MaxWearWeight", hd.Abil.MaxWearWeight);
            command.Parameters.AddWithValue("@HandWeight", hd.Abil.HandWeight);
            command.Parameters.AddWithValue("@MaxHandWeight", hd.Abil.MaxHandWeight);
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                _logger.Error("[Exception] PlayDataStorage.UpdateRecord");
            }
        }

        private void SaveItem(int playerId, THumDataInfo humanRcd)
        {
            var playData = new THumDataInfo();
            playData.Data.Initialization();
            GetItemRecord(playerId, ref playData);
            var useSize = playData.Data.HumItems.Length;
            var oldItems = playData.Data.HumItems;
            var newItems = humanRcd.Data.HumItems;
            var addItem = new UserItem[useSize];
            var delItem = new UserItem[useSize];
            var chgList = new UserItem[useSize];

            var useItemCount = oldItems.Where(x => x != null).Count(x => x.MakeIndex == 0 && x.Index == 0);

            for (var i = 0; i < newItems.Length; i++)
            {
                if (oldItems[i] == null)
                {
                    continue;
                }
                if (newItems[i].MakeIndex == 0 && oldItems[i].MakeIndex > 0)
                {
                    delItem[i] = oldItems[i];
                    continue;
                }
                if (newItems[i].MakeIndex > 0 || newItems[i].MakeIndex > oldItems[i].MakeIndex)
                {
                    chgList[i] = newItems[i]; //差异化的数据
                    continue;
                }
                if (oldItems[i] == null && newItems[i].MakeIndex > 0) //历史位置没有物品，但是需要保存的位置有物品时，对数据进行更新操作
                {
                    chgList[i] = newItems[i]; //差异化的数据
                    continue;
                }
                if (oldItems[i].Index == 0 && oldItems[i].MakeIndex == 0 && newItems[i].MakeIndex > 0 && newItems[i].Index > 0) //穿戴位置没有任何数据
                {
                    delItem[i] = newItems[i];
                    continue;
                }
            }
            try
            {
                if (delItem.Length > 0)
                {
                    var strSql = new StringBuilder();
                    strSql.AppendLine("UPDATE characters_item SET Position = @Position, MakeIndex = 0, StdIndex = 0, Dura = 0, DuraMax = 0");
                    strSql.AppendLine("WHERE PlayerId = @PlayerId AND Position = @Position AND MakeIndex = @MakeIndex AND StdIndex = @StdIndex;");
                    for (var i = 0; i < delItem.Length; i++)
                    {
                        if (delItem[i] == null)
                        {
                            continue;
                        }
                        var command = new MySqlCommand(strSql.ToString());
                        command.Connection = _connection;
                        command.Transaction = _transaction;
                        command.CommandText = strSql.ToString();
                        command.Parameters.AddWithValue("@PlayerId", playerId);
                        command.Parameters.AddWithValue("@Position", i);
                        command.Parameters.AddWithValue("@MakeIndex", delItem[i].MakeIndex);
                        command.Parameters.AddWithValue("@StdIndex", delItem[i].Index);
                        command.ExecuteNonQuery();
                    }
                    try
                    {
                        ClearItemAttr(playerId, delItem.Where(x => x != null).Select(x => x.MakeIndex));
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("[Exception] PlayDataStorage.SaveItem (Clear Item)");
                        _logger.Error(ex.StackTrace);
                    }
                }

                if (chgList.Length > 0)
                {
                    var strSql = new StringBuilder();
                    strSql.AppendLine("UPDATE characters_item SET Position = @Position, MakeIndex =@MakeIndex, StdIndex = @StdIndex, Dura = @Dura, DuraMax = @DuraMax ");
                    strSql.AppendLine("WHERE PlayerId = @PlayerId AND Position = @Position");

                    for (var i = 0; i < chgList.Length; i++)
                    {
                        if (chgList[i] == null)
                        {
                            continue;
                        }
                        var command = new MySqlCommand();
                        command.Connection = _connection;
                        command.Transaction = _transaction;
                        command.CommandText = strSql.ToString();
                        command.Parameters.AddWithValue("@PlayerId", playerId);
                        command.Parameters.AddWithValue("@CharName", humanRcd.Data.sCharName);
                        command.Parameters.AddWithValue("@Position", i);
                        command.Parameters.AddWithValue("@MakeIndex", chgList[i].MakeIndex);
                        command.Parameters.AddWithValue("@StdIndex", chgList[i].Index);
                        command.Parameters.AddWithValue("@Dura", chgList[i].Dura);
                        command.Parameters.AddWithValue("@DuraMax", chgList[i].DuraMax);
                        command.ExecuteNonQuery();
                    }
                    try
                    {
                        UpdateItemAttr(playerId, chgList);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("[Exception] PlayDataStorage.SaveItem (Update Item)");
                        _logger.Error(ex.StackTrace);
                    }
                }

                if (useItemCount <= 0)
                {
                    var addItemCount = 0;
                    if (useItemCount == 0)
                    {
                        addItemCount = useSize;
                    }
                    addItemCount = useSize - addItemCount < 0 ? useSize : addItemCount - (useSize - addItemCount);
                    if (addItemCount > 0)
                    {
                        for (var i = 0; i < addItemCount; i++)
                        {
                            addItem[i] = new UserItem();
                        }
                    }

                    var strSql = new StringBuilder();
                    strSql.AppendLine("INSERT INTO characters_item (PlayerId,ChrName, Position, MakeIndex, StdIndex, Dura, DuraMax)");
                    strSql.AppendLine(" VALUES ");
                    strSql.AppendLine("(@PlayerId,@ChrName, @Position, @MakeIndex, @StdIndex, @Dura, @DuraMax)");

                    for (var i = 0; i < addItem.Length; i++)
                    {
                        if (addItem[i] == null)
                        {
                            continue;
                        }
                        var command = new MySqlCommand();
                        command.Connection = _connection;
                        command.Transaction = _transaction;
                        command.CommandText = strSql.ToString();
                        command.Parameters.AddWithValue("@PlayerId", playerId);
                        command.Parameters.AddWithValue("@ChrName", humanRcd.Data.sCharName);
                        command.Parameters.AddWithValue("@Position", i);
                        command.Parameters.AddWithValue("@MakeIndex", addItem[i].MakeIndex);
                        command.Parameters.AddWithValue("@StdIndex", addItem[i].Index);
                        command.Parameters.AddWithValue("@Dura", addItem[i].Dura);
                        command.Parameters.AddWithValue("@DuraMax", addItem[i].DuraMax);
                        command.ExecuteNonQuery();
                    }
                    try
                    {
                        CreateItemAttr(playerId, addItem);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("[Exception] PlayDataStorage.SaveItem (Insert Item)");
                        _logger.Error(ex.StackTrace);
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.UpdateRecord");
                _logger.Error(ex.StackTrace);
            }
        }

        private void SaveBagItem(int playerId, THumDataInfo humanRcd)
        {
            try
            {
                var playData = new THumDataInfo();
                playData.Data.Initialization();
                GetBagItemRecord(playerId, ref playData);
                var bagSize = humanRcd.Data.BagItems.Length;
                var oldItems = playData.Data.BagItems;
                var newItems = humanRcd.Data.BagItems;
                var bagItemCount = oldItems.Where(x => x != null).Count(x => x.MakeIndex == 0 && x.Index == 0);
                var addItem = new UserItem[bagSize];
                var delItem = new UserItem[bagSize];
                var chgList = new UserItem[bagSize];

                for (var i = 0; i < newItems.Length; i++)
                {
                    if (oldItems[i] == null)
                    {
                        continue;
                    }
                    if (newItems[i].MakeIndex == 0 && oldItems[i].MakeIndex > 0)
                    {
                        delItem[i] = oldItems[i];
                        continue;
                    }
                    if (newItems[i].MakeIndex > 0 || newItems[i].MakeIndex > oldItems[i].MakeIndex)
                    {
                        chgList[i] = newItems[i]; //差异化的数据
                        continue;
                    }
                    if (oldItems[i] == null && newItems[i].MakeIndex > 0) //历史位置没有物品，但是需要保存的位置有物品时，对数据进行更新操作
                    {
                        chgList[i] = newItems[i]; //差异化的数据
                        continue;
                    }
                    if (oldItems[i].Index == 0 && oldItems[i].MakeIndex == 0 && newItems[i].MakeIndex > 0 && newItems[i].Index > 0) //穿戴位置没有任何数据
                    {
                        delItem[i] = newItems[i];
                        continue;
                    }
                }

                if (delItem.Length > 0)
                {
                    var strSql = new StringBuilder();
                    strSql.AppendLine("UPDATE characters_bagitem SET Position = @Position, MakeIndex = 0, StdIndex = 0, Dura = 0, DuraMax = 0");
                    strSql.AppendLine("WHERE PlayerId = @PlayerId  AND Position = @Position AND MakeIndex = @MakeIndex AND StdIndex = @StdIndex;");
                    for (var i = 0; i < delItem.Length; i++)
                    {
                        if (delItem[i] == null)
                        {
                            continue;
                        }
                        var command = new MySqlCommand();
                        command.Connection = _connection;
                        command.Transaction = _transaction;
                        command.CommandText = strSql.ToString();
                        command.Parameters.AddWithValue("@PlayerId", playerId);
                        command.Parameters.AddWithValue("@Position", i);
                        command.Parameters.AddWithValue("@MakeIndex", delItem[i].MakeIndex);
                        command.Parameters.AddWithValue("@StdIndex", delItem[i].Index);
                        command.ExecuteNonQuery();
                    }
                    try
                    {
                        ClearItemAttr(playerId, delItem.Where(x => x != null).Select(x => x.MakeIndex));
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("[Exception] PlayDataStorage.UpdateBagItem (Delete Item)");
                        _logger.Error(ex.StackTrace);
                    }
                }
                if (chgList.Length > 0)
                {
                    var strSql = new StringBuilder();
                    strSql.AppendLine("UPDATE characters_bagitem SET Position = @Position, MakeIndex =@MakeIndex, StdIndex = @StdIndex, Dura = @Dura, DuraMax = @DuraMax ");
                    strSql.AppendLine("WHERE PlayerId = @PlayerId AND Position = @Position;");
                    for (var i = 0; i < chgList.Length; i++)
                    {
                        if (chgList[i] == null)
                        {
                            continue;
                        }
                        var command = new MySqlCommand();
                        command.Connection = _connection;
                        command.Transaction = _transaction;
                        command.CommandText = strSql.ToString();
                        command.Parameters.AddWithValue("@PlayerId", playerId);
                        command.Parameters.AddWithValue("@CharName", humanRcd.Data.sCharName);
                        command.Parameters.AddWithValue("@Position", i);
                        command.Parameters.AddWithValue("@MakeIndex", chgList[i].MakeIndex);
                        command.Parameters.AddWithValue("@StdIndex", chgList[i].Index);
                        command.Parameters.AddWithValue("@Dura", chgList[i].Dura);
                        command.Parameters.AddWithValue("@DuraMax", chgList[i].DuraMax);
                        command.ExecuteNonQuery();
                    }
                    try
                    {
                        UpdateItemAttr(playerId, chgList);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("[Exception] PlayDataStorage.UpdateBagItem (Update Item)");
                        _logger.Error(ex.StackTrace);
                    }
                }
                if (bagItemCount <= 0)
                {
                    var addItemCount = 0;
                    if (bagItemCount == 0)
                    {
                        addItemCount = bagSize;
                    }
                    addItemCount = bagSize - addItemCount < 0 ? 0 : addItemCount - (bagSize - addItemCount);
                    if (addItemCount > 0)
                    {
                        for (var i = 0; i < addItemCount; i++)
                        {
                            addItem[i] = new UserItem();
                        }
                    }

                    var strSql = new StringBuilder();
                    strSql.AppendLine("INSERT INTO characters_bagitem (PlayerId,ChrName, Position, MakeIndex, StdIndex, Dura, DuraMax)");
                    strSql.AppendLine(" VALUES ");
                    strSql.AppendLine("(@PlayerId,@ChrName, @Position, @MakeIndex, @StdIndex, @Dura, @DuraMax);");

                    for (var i = 0; i < addItem.Length; i++)
                    {
                        var command = new MySqlCommand();
                        command.Connection = _connection;
                        command.Transaction = _transaction;
                        command.CommandText = strSql.ToString();
                        command.Parameters.AddWithValue("@PlayerId", playerId);
                        command.Parameters.AddWithValue("@ChrName", humanRcd.Data.sCharName);
                        command.Parameters.AddWithValue("@Position", i);
                        command.Parameters.AddWithValue("@MakeIndex", addItem[i].MakeIndex);
                        command.Parameters.AddWithValue("@StdIndex", addItem[i].Index);
                        command.Parameters.AddWithValue("@Dura", addItem[i].Dura);
                        command.Parameters.AddWithValue("@DuraMax", addItem[i].DuraMax);
                        command.ExecuteNonQuery();
                    }
                    try
                    {
                        CreateItemAttr(playerId, addItem);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("[Exception] PlayDataStorage.UpdateBagItem (Insert Item)");
                        _logger.Error(ex.StackTrace);
                    }
                }
            }
            catch
            {
                _logger.Error("[Exception] PlayDataStorage.UpdateBagItem");
            }
        }

        private void SaveStorageItem(int playerId, THumDataInfo humanRcd)
        {
            try
            {
                var playData = new THumDataInfo();
                playData.Data.Initialization();
                GetStorageRecord(playerId, ref playData);
                var storageSize = humanRcd.Data.StorageItems.Length;
                var oldItems = playData.Data.StorageItems;
                var newItems = humanRcd.Data.StorageItems;
                var storageItemCount = oldItems.Where(x => x != null).Count(x => x.MakeIndex == 0 && x.Index == 0);
                var addItem = new UserItem[storageSize];
                var delItem = new UserItem[storageSize];
                var chgList = new UserItem[storageSize];

                for (var i = 0; i < newItems.Length; i++)
                {
                    if (oldItems[i] == null)
                    {
                        continue;
                    }
                    if (newItems[i].MakeIndex == 0 && oldItems[i].MakeIndex > 0)
                    {
                        delItem[i] = oldItems[i];
                        continue;
                    }
                    if (newItems[i].MakeIndex > 0 || newItems[i].MakeIndex > oldItems[i].MakeIndex)
                    {
                        chgList[i] = newItems[i]; //差异化的数据
                        continue;
                    }
                    if (oldItems[i] == null && newItems[i].MakeIndex > 0) //历史位置没有物品，但是需要保存的位置有物品时，对数据进行更新操作
                    {
                        chgList[i] = newItems[i]; //差异化的数据
                        continue;
                    }
                    if (oldItems[i].Index == 0 && oldItems[i].MakeIndex == 0 && newItems[i].MakeIndex > 0 && newItems[i].Index > 0) //穿戴位置没有任何数据
                    {
                        delItem[i] = newItems[i];
                        continue;
                    }
                }

                if (delItem.Length > 0)
                {
                    var strSql = new StringBuilder();
                    strSql.AppendLine("UPDATE characters_storageitem SET Position = @Position, MakeIndex = 0, StdIndex = 0, Dura = 0, DuraMax = 0");
                    strSql.AppendLine("WHERE PlayerId = @PlayerId  AND Position = @Position AND MakeIndex = @MakeIndex AND StdIndex = @StdIndex;");
                    for (var i = 0; i < delItem.Length; i++)
                    {
                        if (delItem[i] == null)
                        {
                            continue;
                        }
                        var command = new MySqlCommand();
                        command.Connection = _connection;
                        command.Transaction = _transaction;
                        command.CommandText = strSql.ToString();
                        command.Parameters.AddWithValue("@PlayerId", playerId);
                        command.Parameters.AddWithValue("@Position", i);
                        command.Parameters.AddWithValue("@MakeIndex", delItem[i].MakeIndex);
                        command.Parameters.AddWithValue("@StdIndex", delItem[i].Index);
                        command.ExecuteNonQuery();
                    }
                    try
                    {
                        ClearItemAttr(playerId, delItem.Where(x => x != null).Select(x => x.MakeIndex));
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("[Exception] PlayDataStorage.SaveStorageItem (Delete Item)");
                        _logger.Error(ex.StackTrace);
                    }
                }

                if (chgList.Length > 0)
                {
                    var strSql = new StringBuilder();
                    strSql.AppendLine("UPDATE characters_storageitem SET Position = @Position, MakeIndex =@MakeIndex, StdIndex = @StdIndex, Dura = @Dura, DuraMax = @DuraMax ");
                    strSql.AppendLine("WHERE PlayerId = @PlayerId AND Position = @Position;");
                    for (var i = 0; i < chgList.Length; i++)
                    {
                        if (chgList[i] == null)
                        {
                            continue;
                        }
                        var command = new MySqlCommand();
                        command.Connection = _connection;
                        command.Transaction = _transaction;
                        command.CommandText = strSql.ToString();
                        command.Parameters.AddWithValue("@PlayerId", playerId);
                        command.Parameters.AddWithValue("@ChrName", humanRcd.Data.sCharName);
                        command.Parameters.AddWithValue("@Position", i);
                        command.Parameters.AddWithValue("@MakeIndex", chgList[i].MakeIndex);
                        command.Parameters.AddWithValue("@StdIndex", chgList[i].Index);
                        command.Parameters.AddWithValue("@Dura", chgList[i].Dura);
                        command.Parameters.AddWithValue("@DuraMax", chgList[i].DuraMax);
                        command.ExecuteNonQuery();
                    }
                    try
                    {
                        UpdateItemAttr(playerId, chgList);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("[Exception] PlayDataStorage.SaveStorageItem (Update Item)");
                        _logger.Error(ex.StackTrace);
                    }
                }

                if (storageItemCount <= 0)
                {
                    var addItemCount = 0;
                    if (storageItemCount == 0)
                    {
                        addItemCount = storageSize;
                    }
                    addItemCount = storageSize - addItemCount < 0 ? 0 : addItemCount - (storageSize - addItemCount);
                    if (addItemCount > 0)
                    {
                        for (var i = 0; i < addItemCount; i++)
                        {
                            addItem[i] = new UserItem();
                        }
                    }

                    var strSql = new StringBuilder();
                    strSql.AppendLine("INSERT INTO characters_storageitem (PlayerId,ChrName, Position, MakeIndex, StdIndex, Dura, DuraMax)");
                    strSql.AppendLine(" VALUES ");
                    strSql.AppendLine("(@PlayerId,@ChrName, @Position, @MakeIndex, @StdIndex, @Dura, @DuraMax);");
                    try
                    {
                        for (var i = 0; i < addItem.Length; i++)
                        {
                            var command = new MySqlCommand();
                            command.Connection = _connection;
                            command.Transaction = _transaction;
                            command.CommandText = strSql.ToString();
                            command.Parameters.AddWithValue("@PlayerId", playerId);
                            command.Parameters.AddWithValue("@ChrName", humanRcd.Data.sCharName);
                            command.Parameters.AddWithValue("@Position", i);
                            command.Parameters.AddWithValue("@MakeIndex", addItem[i].MakeIndex);
                            command.Parameters.AddWithValue("@StdIndex", addItem[i].Index);
                            command.Parameters.AddWithValue("@Dura", addItem[i].Dura);
                            command.Parameters.AddWithValue("@DuraMax", addItem[i].DuraMax);
                            command.ExecuteNonQuery();
                        }
                        CreateItemAttr(playerId, addItem);
                    }
                    catch (Exception e)
                    {
                        _logger.Error("[Exception] PlayDataStorage.SaveStorageItem (Insert Item)");
                        _logger.Error(e.StackTrace);
                    }
                }
            }
            catch
            {
                _logger.Error("[Exception] PlayDataStorage.SaveStorageItem");
            }
        }

        private void SaveMagics(int playerId, MagicRcd[] magicRcds)
        {
            var command = new MySqlCommand();
            command.Connection = _connection;
            command.Transaction = _transaction;
            command.CommandText = "DELETE FROM characters_magic WHERE PlayerId=@PlayerId";
            command.Parameters.AddWithValue("@PlayerId", playerId);
            command.ExecuteNonQuery();
            try
            {
                const string sStrSql = "INSERT INTO characters_magic(PlayerId, MagicId, Level, USEKEY, CurrTrain) VALUES (@PlayerId, @MagicId, @Level, @USEKEY, @CurrTrain)";
                for (var i = 0; i < magicRcds.Length; i++)
                {
                    if (magicRcds[i].MagIdx > 0)
                    {
                        command = new MySqlCommand();
                        command.CommandText = sStrSql;
                        command.Connection = _connection;
                        command.Parameters.AddWithValue("@PlayerId", playerId);
                        command.Parameters.AddWithValue("@MagicId", magicRcds[i].MagIdx);
                        command.Parameters.AddWithValue("@Level", magicRcds[i].Level);
                        command.Parameters.AddWithValue("@Usekey", magicRcds[i].MagicKey);
                        command.Parameters.AddWithValue("@CurrTrain", magicRcds[i].TranPoint);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.SaveMagics");
                _logger.Error(ex.StackTrace);
            }
        }

        private void SaveBonusability(int playerId, THumDataInfo humanRcd)
        {
            const string sSqlStr = "UPDATE characters_bonusability SET AC=@AC, MAC=@MAC, DC=@DC, MC=@MC, SC=@SC, HP=@HP, MP=@MP, HIT=@HIT, SPEED=@SPEED, RESERVED=@RESERVED WHERE PlayerId=@PlayerId";
            var command = new MySqlCommand();
            command.Connection = _connection;
            command.Transaction = _transaction;
            command.CommandText = sSqlStr;
            var bonusAbil = humanRcd.Data.BonusAbil;
            command.Parameters.AddWithValue("@PlayerId", playerId);
            command.Parameters.AddWithValue("@AC", bonusAbil.AC);
            command.Parameters.AddWithValue("@MAC", bonusAbil.MAC);
            command.Parameters.AddWithValue("@DC", bonusAbil.DC);
            command.Parameters.AddWithValue("@MC", bonusAbil.MC);
            command.Parameters.AddWithValue("@SC", bonusAbil.SC);
            command.Parameters.AddWithValue("@HP", bonusAbil.HP);
            command.Parameters.AddWithValue("@MP", bonusAbil.MP);
            command.Parameters.AddWithValue("@HIT", bonusAbil.Hit);
            command.Parameters.AddWithValue("@SPEED", bonusAbil.Speed);
            command.Parameters.AddWithValue("@RESERVED", bonusAbil.Reserved);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.SaveBonusability");
                _logger.Error(ex.StackTrace);
            }
        }

        private void SaveQuest(int id, THumDataInfo humanRcd)
        {
            const string sSqlStr4 = "DELETE FROM characters_quest WHERE PlayerId=@PlayerId";
            const string sSqlStr5 = "INSERT INTO characters_quest (PlayerId, QUESTOPENINDEX, QUESTFININDEX, QUEST) VALUES(@PlayerId, @QUESTOPENINDEX, @QUESTFININDEX, @QUEST)";
            var command = new MySqlCommand();
            command.Connection = _connection;
            command.Transaction = _transaction;
            command.Parameters.AddWithValue("@PlayerId", id);
            command.CommandText = sSqlStr4;
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                _logger.Error("[Exception] PlayDataStorage.SaveQuest");
            }
        }

        private void SaveStatus(int playerId, THumDataInfo humanRcd)
        {
            const string sSqlStr4 = "DELETE FROM characters_status WHERE PlayerId=@PlayerId";
            const string sSqlStr5 = "INSERT INTO characters_status (PlayerId, CharName, Status) VALUES(@PlayerId, @CharName, @Status)";
            var command = new MySqlCommand();
            command.Connection = _connection;
            command.Transaction = _transaction;
            command.Parameters.AddWithValue("@PlayerId", playerId);
            command.CommandText = sSqlStr4;
            try
            {
                command.ExecuteNonQuery();
                command.CommandText = sSqlStr5;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@PlayerId", playerId);
                command.Parameters.AddWithValue("@CharName", humanRcd.Data.sCharName);
                command.Parameters.AddWithValue("@Status", string.Join("/", humanRcd.Data.StatusTimeArr));
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.UpdateStatus (INSERT characters_status)");
                _logger.Error(ex.StackTrace);
            }
        }

        public int Find(string sChrName, StringDictionary list)
        {
            int result;
            for (var i = 0; i < _mirQuickMap.Count; i++)
            {
                //if (HUtil32.CompareLStr(m_MirQuickList[i], sChrName, sChrName.Length))
                //{
                //    List.Add(m_MirQuickList[i], m_MirQuickList.Values[i]);
                //}
            }
            return list.Count;
        }

        public bool Delete(int nIndex)
        {
            for (var i = 0; i < _mirQuickMap.Count; i++)
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
            return false;
        }

        private bool DeleteRecord(int nIndex)
        {
            var result = true;
            var playerId = _quickIndexIdMap[nIndex];
            var success = false;
            Open(ref success);
            if (!success)
            {
                return false;
            }
            const string sqlString = "UPDATE characters SET DELETED=1, LastUpdate=now() WHERE ID=@Id";
            var command = new MySqlCommand();
            command.Connection = _connection;
            command.Transaction = _transaction;
            command.CommandText = sqlString;
            command.Parameters.AddWithValue("@Id", playerId);
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                result = false;
                _logger.Error("[Exception] PlayDataStorage.DeleteRecord");
            }
            finally
            {
                Close(_connection);
            }
            return result;
        }

        public int Count()
        {
            return _mirQuickMap.Count;
        }

        public bool Delete(string sChrName)
        {
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
            return false;
        }

        public int GetQryChar(int nIndex, ref QueryChr queryChrRcd)
        {
            var result = -1;
            const string sSql = "SELECT * FROM characters WHERE ID=@Id";
            if (nIndex < 0)
            {
                return -1;
            }
            if (_quickIndexIdMap.Count <= nIndex)
            {
                return -1;
            }
            var playerId = _quickIndexIdMap[nIndex];
            var command = new MySqlCommand();
            try
            {
                command.Connection = _connection;
                command.CommandText = sSql;
                command.Parameters.AddWithValue("@Id", playerId);
                using var dr = command.ExecuteReader();
                while (dr.Read())
                {
                    queryChrRcd.sName = dr.GetString("CharName");
                    queryChrRcd.btJob = dr.GetByte("Job");
                    queryChrRcd.btHair = dr.GetByte("Hair");
                    queryChrRcd.btSex = dr.GetByte("Sec");
                    queryChrRcd.wLevel = dr.GetUInt16("Level");
                }
            }
            catch (Exception)
            {
                _logger.Error("[Exception] PlayDataStorage.GetQryChar");
                return result;
            }
            return nIndex;
        }
        
        private long QueryUseItemCount(int playerId)
        {
            var success = false;
            Open(ref success);
            if (!success)
            {
                return 0;
            }
            long result = 0;
            const string sSqlString = "SELECT count(0) FROM characters_item WHERE PlayerId=@PlayerId AND MakeIndex=0 AND StdIndex=0";
            var command = new MySqlCommand();
            try
            {
                command.Connection = _connection;
                command.CommandText = sSqlString;
                command.Parameters.AddWithValue("@PlayerId", playerId);
                result = (long)command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.QueryUseItemCount:" + ex.StackTrace);
            }
            return result;
        }

        private long QueryBagItemCount(int playerId)
        {
            var success = false;
            Open(ref success);
            if (!success)
            {
                return 0;
            }
            long result = 0;
            const string sSqlString = "SELECT count(0) FROM characters_bagitem WHERE PlayerId=@PlayerId AND MakeIndex=0 AND StdIndex=0";
            var command = new MySqlCommand();
            try
            {
                command.Connection = _connection;
                command.CommandText = sSqlString;
                command.Parameters.AddWithValue("@PlayerId", playerId);
                result = (long)command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.QueryBagItemCount:" + ex.StackTrace);
            }
            return result;
        }

        private long QueryStorageItemCount(int playerId)
        {
            var success = false;
            Open(ref success);
            if (!success)
            {
                return 0;
            }
            long result = 0;
            const string sSqlString = "SELECT count(0) FROM characters_storageitem WHERE PlayerId=@PlayerId AND MakeIndex=0 AND StdIndex=0";
            var command = new MySqlCommand();
            try
            {
                command.Connection = _connection;
                command.CommandText = sSqlString;
                command.Parameters.AddWithValue("@PlayerId", playerId);
                result = (long)command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.QueryStorageItemCount:" + ex.StackTrace);
            }
            return result;
        }

        private void ClearItemAttr(int playerId,IEnumerable<int> makeIndex)
        {
            if (!makeIndex.Any())
            {
                return;
            }
            var strSql = new StringBuilder();
            strSql.AppendLine("UPDATE characters_item_attr SET MakeIndex = 0,VALUE0 = 0, VALUE1 = 0, VALUE2 = 0, VALUE3 = 0, VALUE4 = 0, VALUE5 = 0");
            strSql.AppendLine(", VALUE6 = 0, VALUE7 = 0, VALUE8 = 0, VALUE9 = 0, VALUE10 = 0, VALUE11 = 0, VALUE12 = 0, VALUE13 = 0 WHERE PlayerId = @PlayerId AND MakeIndex in (@MakeIndex);");
            
            var command = new MySqlCommand();
            command.Connection = _connection;
            command.Transaction = _transaction;
            command.CommandText = strSql.ToString();
            command.Parameters.AddWithValue("@PlayerId", playerId);
            command.Parameters.AddWithValue("@StdIndex", string.Join(",", makeIndex));
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.ClearItemAttr");
                _logger.Error(ex.StackTrace);
            }
        }

        private void UpdateItemAttr(int playerId, UserItem[] userItems)
        {
            var strSql = new StringBuilder();
            strSql.AppendLine("UPDATE characters_item_attr SET ");
            strSql.AppendLine("VALUE0 = @VALUE0, VALUE1 = @VALUE1, VALUE2 =@VALUE2, VALUE3 = @VALUE3, VALUE4 = @VALUE4,VALUE5 = @VALUE5, VALUE6 = @VALUE6, VALUE7 = @VALUE7, ");
            strSql.AppendLine("VALUE8 = @VALUE8, VALUE9 = @VALUE9, VALUE10 = @VALUE10, VALUE11 = @VALUE11, VALUE12 = @VALUE12, VALUE13 = @VALUE13");
            strSql.AppendLine("WHERE PlayerId = @PlayerId AND MakeIndex = @MakeIndex");
            try
            {
                for (var i = 0; i < userItems.Length; i++)
                {
                    if (userItems[i] == null)
                    {
                        continue;
                    }
                    var command = new MySqlCommand();
                    command.Connection = _connection;
                    command.Transaction = _transaction;
                    command.CommandText = strSql.ToString();
                    command.Parameters.AddWithValue("@PlayerId", playerId);
                    command.Parameters.AddWithValue("@MakeIndex", userItems[i].MakeIndex);
                    command.Parameters.AddWithValue("@VALUE0", userItems[i].Desc[0]);
                    command.Parameters.AddWithValue("@VALUE1", userItems[i].Desc[1]);
                    command.Parameters.AddWithValue("@VALUE2", userItems[i].Desc[2]);
                    command.Parameters.AddWithValue("@VALUE3", userItems[i].Desc[3]);
                    command.Parameters.AddWithValue("@VALUE4", userItems[i].Desc[4]);
                    command.Parameters.AddWithValue("@VALUE5", userItems[i].Desc[5]);
                    command.Parameters.AddWithValue("@VALUE6", userItems[i].Desc[6]);
                    command.Parameters.AddWithValue("@VALUE7", userItems[i].Desc[7]);
                    command.Parameters.AddWithValue("@VALUE8", userItems[i].Desc[8]);
                    command.Parameters.AddWithValue("@VALUE9", userItems[i].Desc[9]);
                    command.Parameters.AddWithValue("@VALUE10", userItems[i].Desc[ItemAttr.WeaponUpgrade]);
                    command.Parameters.AddWithValue("@VALUE11", userItems[i].Desc[11]);
                    command.Parameters.AddWithValue("@VALUE12", userItems[i].Desc[12]);
                    command.Parameters.AddWithValue("@VALUE13", userItems[i].Desc[13]);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                _logger.Error("[Exception] PlayDataStorage.UpdateItemAttr (Update item attr)");
                _logger.Error(e.StackTrace);
            }
        }

        private void CreateItemAttr(int playerId, UserItem[] userItems)
        {
            var strSql = new StringBuilder();
            strSql.AppendLine("INSERT INTO characters_item_attr (PlayerId,MakeIndex,VALUE0, VALUE1, VALUE2, VALUE3, VALUE4, VALUE5, VALUE6, VALUE7, VALUE8, VALUE9, VALUE10, VALUE11, VALUE12, VALUE13)");
            strSql.AppendLine(" VALUES ");
            strSql.AppendLine("(@PlayerId, @MakeIndex,@VALUE0, @VALUE1, @VALUE2, @VALUE3, @VALUE4, @VALUE5,@VALUE6, @VALUE7, @VALUE8, @VALUE9, @VALUE10, @VALUE11, @VALUE12, @VALUE13)");
            try
            {
                for (var i = 0; i < userItems.Length; i++)
                {
                    if (userItems[i] == null)
                    {
                        continue;
                    }
                    var command = new MySqlCommand();
                    command.Connection = _connection;
                    command.Transaction = _transaction;
                    command.CommandText = strSql.ToString();
                    command.Parameters.AddWithValue("@PlayerId", playerId);
                    command.Parameters.AddWithValue("@MakeIndex", userItems[i].MakeIndex);
                    command.Parameters.AddWithValue("@VALUE0", userItems[i].Desc[0]);
                    command.Parameters.AddWithValue("@VALUE1", userItems[i].Desc[1]);
                    command.Parameters.AddWithValue("@VALUE2", userItems[i].Desc[2]);
                    command.Parameters.AddWithValue("@VALUE3", userItems[i].Desc[3]);
                    command.Parameters.AddWithValue("@VALUE4", userItems[i].Desc[4]);
                    command.Parameters.AddWithValue("@VALUE5", userItems[i].Desc[5]);
                    command.Parameters.AddWithValue("@VALUE6", userItems[i].Desc[6]);
                    command.Parameters.AddWithValue("@VALUE7", userItems[i].Desc[7]);
                    command.Parameters.AddWithValue("@VALUE8", userItems[i].Desc[8]);
                    command.Parameters.AddWithValue("@VALUE9", userItems[i].Desc[9]);
                    command.Parameters.AddWithValue("@VALUE10", userItems[i].Desc[ItemAttr.WeaponUpgrade]);
                    command.Parameters.AddWithValue("@VALUE11", userItems[i].Desc[11]);
                    command.Parameters.AddWithValue("@VALUE12", userItems[i].Desc[12]);
                    command.Parameters.AddWithValue("@VALUE13", userItems[i].Desc[13]);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                _logger.Error("[Exception] PlayDataStorage.UpdateRecord (Insert item attr)");
                _logger.Error(e.StackTrace);
            }
        }

        private void DeleteItemAttr(int playerId, IEnumerable<int> makeIndex)
        {
            var command = new MySqlCommand();
            command.Connection = _connection;
            command.Transaction = _transaction;
            try
            {
                command.CommandText = "DELETE FROM characters_item_attr WHERE PlayerId=@PlayerId AND MakeIndex in (@MakeIndex)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@PlayerId", playerId);
                command.Parameters.AddWithValue("@MakeIndex", string.Join(",", makeIndex));
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Error("[Exception] PlayDataStorage.UpdateRecord (Delete item attr)");
                _logger.Error(e.StackTrace);
            }
        }

        private void QueryItemAttr(int playerId, ref UserItem[] userItems)
        {
            var makeIndexs = userItems.Where(x => x != null).Select(x => x.MakeIndex);
            if (makeIndexs == null || makeIndexs.Count() <= 0)
            {
                return;
            }
            const string sSqlString = "SELECT * FROM characters_item_attr WHERE PlayerId=@PlayerId and MakeIndex in (@MakeIndex)";
            var command = new MySqlCommand();
            command.Connection = _connection;
            try
            {
                command.CommandText = sSqlString;
                command.Parameters.AddWithValue("@PlayerId", playerId);
                command.Parameters.AddWithValue("@MakeIndex", string.Join(",", userItems.Select(x => x.MakeIndex)));
                using var dr = command.ExecuteReader();
                var nPosition = 0;
                while (dr.Read())
                {
                    userItems[nPosition].Desc[nPosition] = dr.GetByte($"VALUE{nPosition}");
                    nPosition++;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.GetStorageItemCount:" + ex.StackTrace);
            }
        }
    }
}