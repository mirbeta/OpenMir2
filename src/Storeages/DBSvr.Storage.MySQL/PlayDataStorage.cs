using DBSvr.Storage.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using SystemModule;
using SystemModule.Packet.ClientPackets;
using SystemModule.Packet.ServerPackets;

namespace DBSvr.Storage.MySQL
{
    public class PlayDataStorage : IPlayDataStorage
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<string, int> _mirQuickMap;
        private readonly Dictionary<int, int> _quickIndexIdMap;
        private readonly QuickIdList _mirQuickIdList;
        private readonly StorageOption _storageOption;
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
            _recordCount = -1;
            IList<QuickId> accountList = new List<QuickId>();
            IList<string> chrNameList = new List<string>();
            using var context = new StorageContext(_storageOption);
            var success = false;
            context.Open(ref success);
            if (!success)
            {
                return;
            }
            try
            {
                var command = context.CreateCommand();
                command.CommandText = sSqlString;
                using var dr = command.ExecuteReader();
                var nIndex = 0;
                while (dr.Read())
                {
                    nIndex = dr.GetInt32("Id");
                    var boDeleted = dr.GetBoolean("Deleted");
                    var sAccount = dr.GetString("LoginID");
                    var sChrName = dr.GetString("ChrName");
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
                dr.Close();
                dr.Dispose();
            }
            finally
            {
                context.Dispose();
            }
            for (var nIndex = 0; nIndex < accountList.Count; nIndex++)
            {
                _mirQuickIdList.AddRecord(accountList[nIndex].sAccount, chrNameList[nIndex], 0, accountList[nIndex].nSelectID);
            }
            accountList = null;
            chrNameList = null;
            //m_MirQuickList.SortString(0, m_MirQuickList.Count - 1);
        }

        public int Index(string sName)
        {
            if (_mirQuickMap.ContainsKey(sName))
            {
                return _mirQuickMap[sName];
            }
            return -1;
        }

        public int Get(int nIndex, ref HumDataInfo humanRcd)
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

        public bool Get(string chrName, ref HumDataInfo humanRcd)
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

        public HumInfoData Query(int playerId)
        {
            HumInfoData playData;
            using var context = new StorageContext(_storageOption);
            try
            {
                var success = false;
                context.Open(ref success);
                if (!success)
                {
                    return null;
                }
                playData = GetChrRecord(playerId, context);
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                context.Dispose();
            }
            return playData;
        }

        public bool Update(string chrName, ref HumDataInfo humanRcd)
        {
            if (_mirQuickMap.TryGetValue(chrName, out var playerId))
            {
                if (SaveRecord(playerId, ref humanRcd))
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
            using var context = new StorageContext(_storageOption);
            var success = false;
            context.Open(ref success);
            if (!success)
            {
                return false;
            }
            var result = false;
            try
            {
                try
                {
                    var command = context.CreateCommand();
                    command.CommandText = sStrString;
                    command.Parameters.AddWithValue("@Sex", queryChrRcd.Sex);
                    command.Parameters.AddWithValue("@Job", queryChrRcd.Job);
                    command.Parameters.AddWithValue("@Id", playerId);
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
                context.Dispose();
            }
            return result;
        }

        public bool Add(ref HumDataInfo humanRcd)
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
                if (AddRecord(ref nIndex, ref humanRcd))
                {
                    _mirQuickMap.Add(sChrName, nIndex);
                    _quickIndexIdMap.Add(nIndex, nIndex);
                    result = true;
                    _recordCount++;
                }
            }
            return result;
        }

        private bool GetRecord(int nIndex, ref HumDataInfo humanRcd)
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
            using var context = new StorageContext(_storageOption);
            try
            {
                humanRcd = new HumDataInfo();
                var success = false;
                context.Open(ref success);
                if (!success)
                {
                    return false;
                }
                humanRcd.Data = GetChrRecord(playerId, context);
                humanRcd.Header.sName = humanRcd.Data.ChrName;
                GetAbilGetRecord(playerId, context, ref humanRcd);
                GetBonusAbilRecord(playerId, context, ref humanRcd);
                GetMagicRecord(playerId, context, ref humanRcd);
                GetItemRecord(playerId, context, ref humanRcd);
                GetBagItemRecord(playerId, context, ref humanRcd);
                GetStorageRecord(playerId, context, ref humanRcd);
                GetPlayerStatus(playerId, context, ref humanRcd);
            }
            catch (Exception e)
            {
                _logger.Error("获取角色数据失败." + e.StackTrace);
                return false;
            }
            finally
            {
                context.Dispose();
            }
            return true;
        }

        private HumInfoData GetChrRecord(int playerId, StorageContext context)
        {
            const string sSqlString = "SELECT * FROM characters WHERE ID=@ID";
            try
            {
                HumInfoData humInfoData = null;
                var command = context.CreateCommand();
                command.CommandText = sSqlString;
                command.Parameters.AddWithValue("@Id", playerId);
                using var dr = command.ExecuteReader();
                if (dr.Read())
                {
                    humInfoData = new HumInfoData();
                    humInfoData.Account = dr.GetString("LoginID");
                    humInfoData.ChrName = dr.GetString("ChrName");
                    if (!dr.IsDBNull(dr.GetOrdinal("MapName")))
                    {
                        humInfoData.CurMap = dr.GetString("MapName");
                    }
                    humInfoData.CurX = dr.GetInt16("CX");
                    humInfoData.CurY = dr.GetInt16("CY");
                    humInfoData.Dir = dr.GetByte("Dir");
                    humInfoData.Hair = dr.GetByte("Hair");
                    humInfoData.Sex = dr.GetByte("Sex");
                    humInfoData.Job = dr.GetByte("Job");
                    humInfoData.Gold = dr.GetInt32("Gold");
                    if (!dr.IsDBNull(dr.GetOrdinal("HomeMap")))
                    {
                        humInfoData.HomeMap = dr.GetString("HomeMap");
                    }
                    humInfoData.HomeX = dr.GetInt16("HomeX");
                    humInfoData.HomeY = dr.GetInt16("HomeY");
                    if (!dr.IsDBNull(dr.GetOrdinal("DearName")))
                    {
                        humInfoData.DearName = dr.GetString("DearName");
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("MasterName")))
                    {
                        humInfoData.MasterName = dr.GetString("MasterName");
                    }
                    humInfoData.boMaster = dr.GetBoolean("IsMaster");
                    humInfoData.CreditPoint = (byte)dr.GetInt32("CreditPoint");
                    if (!dr.IsDBNull(dr.GetOrdinal("StoragePwd")))
                    {
                        humInfoData.StoragePwd = dr.GetString("StoragePwd");
                    }
                    humInfoData.ReLevel = dr.GetByte("ReLevel");
                    humInfoData.LockLogon = dr.GetBoolean("LockLogon");
                    humInfoData.BonusPoint = dr.GetInt32("BonusPoint");
                    humInfoData.GameGold = dr.GetInt32("Gold");
                    humInfoData.GamePoint = dr.GetInt32("GamePoint");
                    humInfoData.PayMentPoint = dr.GetInt32("PayMentPoint");
                    humInfoData.HungerStatus = dr.GetInt32("HungerStatus");
                    humInfoData.AllowGroup = (byte)dr.GetInt32("AllowGroup");
                    humInfoData.AttatckMode = dr.GetByte("AttatckMode");
                    humInfoData.IncHealth = dr.GetByte("IncHealth");
                    humInfoData.IncSpell = dr.GetByte("IncSpell");
                    humInfoData.IncHealing = dr.GetByte("IncHealing");
                    humInfoData.FightZoneDieCount = dr.GetByte("FightZoneDieCount");
                    humInfoData.AllowGuildReCall = dr.GetBoolean("AllowGuildReCall");
                    humInfoData.AllowGroupReCall = dr.GetBoolean("AllowGroupReCall");
                    humInfoData.GroupRcallTime = dr.GetInt16("GroupRcallTime");
                    humInfoData.BodyLuck = dr.GetDouble("BodyLuck");
                }
                dr.Close();
                dr.Dispose();
                return humInfoData;
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.GetChrRecord");
                _logger.Error(ex.StackTrace);
                return null;
            }
        }

        private void GetAbilGetRecord(int playerId, StorageContext context, ref HumDataInfo humanRcd)
        {
            int dw;
            try
            {
                var command = context.CreateCommand();
                command.CommandText = "select * from characters_ablity where playerId=@playerId";
                command.Parameters.AddWithValue("@playerId", playerId);
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

        private void GetBonusAbilRecord(int playerId, StorageContext context, ref HumDataInfo humanRcd)
        {
            const string sSqlString = "SELECT * FROM characters_bonusability WHERE PlayerId=@PlayerId";
            try
            {
                var command = context.CreateCommand();
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

        private void GetMagicRecord(int playerId, StorageContext context, ref HumDataInfo humanRcd)
        {
            const string sSqlString = "SELECT * FROM characters_magic WHERE PlayerId=@PlayerId";
            try
            {
                for (var i = 0; i < humanRcd.Data.Magic.Length; i++)
                {
                    humanRcd.Data.Magic[i] = new MagicRcd();
                }
                var command = context.CreateCommand();
                command.CommandText = sSqlString;
                command.Parameters.AddWithValue("@PlayerId", playerId);
                using var dr = command.ExecuteReader();
                var position = 0;
                while (dr.Read())
                {
                    humanRcd.Data.Magic[position].MagIdx = dr.GetUInt16("MagicId");
                    humanRcd.Data.Magic[position].MagicKey = dr.GetChar("UseKey");
                    humanRcd.Data.Magic[position].Level = dr.GetByte("Level");
                    humanRcd.Data.Magic[position].TranPoint = dr.GetInt32("CurrTrain");
                    position++;
                }
                dr.Close();
                dr.Dispose();
            }
            catch (Exception ex)
            {
                _logger.Error($"[Exception] GetMagicRecord");
                _logger.Error(ex.StackTrace);
            }
        }

        private void GetItemRecord(int playerId, StorageContext context, ref HumDataInfo humanRcd)
        {
            const string sSqlString = "SELECT * FROM characters_item WHERE PlayerId=@PlayerId";
            try
            {
                var command = context.CreateCommand();
                command.CommandText = sSqlString;
                command.Parameters.AddWithValue("@PlayerId", playerId);
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
                dr.Close();
                dr.Dispose();
                QueryItemAttr(context, playerId, ref humanRcd.Data.HumItems);
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.GetItemRecord:" + ex.StackTrace);
            }
        }

        private void GetBagItemRecord(int playerId, StorageContext context, ref HumDataInfo humanRcd)
        {
            const string sSqlString = "SELECT * FROM characters_bagitem WHERE PlayerId=@PlayerId";
            try
            {
                var command = context.CreateCommand();
                command.CommandText = sSqlString;
                command.Parameters.AddWithValue("@PlayerId", playerId);
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
                dr.Close();
                dr.Dispose();
                QueryItemAttr(context, playerId, ref humanRcd.Data.BagItems);
            }
            catch
            {
                _logger.Error("[Exception] PlayDataStorage.GetBagItemRecord");
            }
        }

        private void GetStorageRecord(int playerId, StorageContext context, ref HumDataInfo humanRcd)
        {
            const string sSqlString = "SELECT * FROM characters_storageitem WHERE PlayerId=@PlayerId";
            try
            {
                var command = context.CreateCommand();
                command.CommandText = sSqlString;
                command.Parameters.AddWithValue("@PlayerId", playerId);
                using var dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var nPosition = dr.GetInt32("Position");
                    humanRcd.Data.StorageItems[nPosition] = new UserItem();
                    humanRcd.Data.StorageItems[nPosition].MakeIndex = dr.GetInt32("MakeIndex");
                    humanRcd.Data.StorageItems[nPosition].Index = dr.GetUInt16("StdIndex");
                    humanRcd.Data.StorageItems[nPosition].Dura = dr.GetUInt16("Dura");
                    humanRcd.Data.StorageItems[nPosition].DuraMax = dr.GetUInt16("DuraMax");
                }
                dr.Close();
                dr.Dispose();
                QueryItemAttr(context, playerId, ref humanRcd.Data.StorageItems);
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.GetStorageRecord");
                _logger.Error(ex.StackTrace);
            }
        }

        private void GetPlayerStatus(int playerId, StorageContext context, ref HumDataInfo humanRcd)
        {
            const string sSqlString = "SELECT * FROM characters_status WHERE PlayerId=@PlayerId";
            try
            {
                var command = context.CreateCommand();
                command.CommandText = sSqlString;
                command.Parameters.AddWithValue("@PlayerId", playerId);
                using var dr = command.ExecuteReader();
                if (dr.Read())
                {
                    for (var i = 0; i < 15; i++)
                    {
                        humanRcd.Data.StatusTimeArr[i] = dr.GetUInt16($"Status{i}");
                    }
                }
                dr.Close();
                dr.Dispose();
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.GetPlayerStatus");
            }
        }

        private bool AddRecord(ref int nIndex, ref HumDataInfo humanRcd)
        {
            using var context = new StorageContext(_storageOption);
            var success = false;
            context.Open(ref success);
            if (!success)
            {
                return false;
            }
            var result = false;
            try
            {
                context.BeginTransaction();
                var hd = humanRcd.Data;
                var strSql = new StringBuilder();
                strSql.AppendLine("INSERT INTO CHARACTERS (ServerIndex, LoginID, ChrName, MapName, CX, CY, Level, Dir, Hair, Sex, Job, Gold, GamePoint, HomeMap,");
                strSql.AppendLine("HomeX, HomeY, PkPoint, ReLevel, AttatckMode, FightZoneDieCount, BodyLuck, IncHealth,IncSpell, IncHealing, CreditPoint, BonusPoint,");
                strSql.AppendLine("HungerStatus, PayMentPoint, LockLogon, MarryCount, AllowGroupReCall, GroupRcallTime, AllowGuildReCall, IsMaster, MasterName, DearName");
                strSql.AppendLine(",StoragePwd, Deleted, CREATEDATE, LASTUPDATE) VALUES ");
                strSql.AppendLine("(@ServerIndex, @LoginID, @ChrName, @MapName, @CX, @CY, @Level, @Dir, @Hair, @Sex, @Job, @Gold, @GamePoint, @HomeMap,");
                strSql.AppendLine("@HomeX, @HomeY, @PkPoint, @ReLevel, @AttatckMode, @FightZoneDieCount, @BodyLuck, @IncHealth,@IncSpell, @IncHealing, @CreditPoint, @BonusPoint,");
                strSql.AppendLine("@HungerStatus, @PayMentPoint, @LockLogon, @MarryCount, @AllowGroupReCall, @GroupRcallTime, @AllowGuildReCall, @IsMaster, @MasterName, @DearName");
                strSql.AppendLine(",@StoragePwd, @Deleted,now(),now()) ");

                var command = context.CreateCommand();
                command.CommandText = strSql.ToString();
                command.Parameters.AddWithValue("@ServerIndex", hd.ServerIndex);
                command.Parameters.AddWithValue("@LoginID", hd.Account);
                command.Parameters.AddWithValue("@ChrName", hd.ChrName);
                command.Parameters.AddWithValue("@MapName", hd.CurMap);
                command.Parameters.AddWithValue("@CX", hd.CurX);
                command.Parameters.AddWithValue("@CY", hd.CurY);
                command.Parameters.AddWithValue("@Level", hd.Abil.Level);
                command.Parameters.AddWithValue("@Dir", hd.Dir);
                command.Parameters.AddWithValue("@Hair", hd.Hair);
                command.Parameters.AddWithValue("@Sex", hd.Sex);
                command.Parameters.AddWithValue("@Job", hd.Job);
                command.Parameters.AddWithValue("@Gold", hd.Gold);
                command.Parameters.AddWithValue("@GamePoint", hd.GamePoint);
                command.Parameters.AddWithValue("@HomeMap", hd.HomeMap);
                command.Parameters.AddWithValue("@HomeX", hd.HomeX);
                command.Parameters.AddWithValue("@HomeY", hd.HomeY);
                command.Parameters.AddWithValue("@PkPoint", hd.PKPoint);
                command.Parameters.AddWithValue("@ReLevel", hd.ReLevel);
                command.Parameters.AddWithValue("@AttatckMode", hd.AttatckMode);
                command.Parameters.AddWithValue("@FightZoneDieCount", hd.FightZoneDieCount);
                command.Parameters.AddWithValue("@BodyLuck", hd.BodyLuck);
                command.Parameters.AddWithValue("@IncHealth", hd.IncHealth);
                command.Parameters.AddWithValue("@IncSpell", hd.IncSpell);
                command.Parameters.AddWithValue("@IncHealing", hd.IncHealing);
                command.Parameters.AddWithValue("@CreditPoint", hd.CreditPoint);
                command.Parameters.AddWithValue("@BonusPoint", hd.BonusPoint);
                command.Parameters.AddWithValue("@HungerStatus", hd.HungerStatus);
                command.Parameters.AddWithValue("@PayMentPoint", hd.PayMentPoint);
                command.Parameters.AddWithValue("@LockLogon", hd.LockLogon);
                command.Parameters.AddWithValue("@MarryCount", hd.MarryCount);
                command.Parameters.AddWithValue("@AllowGroupReCall", hd.AllowGroup);
                command.Parameters.AddWithValue("@GroupRcallTime", hd.GroupRcallTime);
                command.Parameters.AddWithValue("@AllowGuildReCall", hd.AllowGuildReCall);
                command.Parameters.AddWithValue("@IsMaster", hd.boMaster);
                command.Parameters.AddWithValue("@MasterName", hd.MasterName);
                command.Parameters.AddWithValue("@DearName", hd.DearName);
                command.Parameters.AddWithValue("@StoragePwd", hd.StoragePwd);
                command.Parameters.AddWithValue("@Deleted", 0);
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

                    strSql.Clear();
                    strSql.AppendLine("INSERT INTO characters_status (PlayerId, Status0, Status1, Status2, Status3, Status4, Status5, Status6, Status7, Status8, Status9, Status10, Status11, Status12, Status13, Status14, Status15) VALUES ");
                    strSql.AppendLine("(@PlayerId, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);");
                    command.CommandText = strSql.ToString();
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@PlayerId", nIndex);
                    command.ExecuteNonQuery();

                    context.Commit();
                    result = true;
                }
                catch (Exception ex)
                {
                    context.RollBack();
                    _logger.Error("[Exception] PlayDataStorage.InsertRecord");
                    _logger.Error(ex.StackTrace);
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.Error("创建角色失败" + e.StackTrace);
            }
            finally
            {
                context.Dispose();
            }
            return result;
        }

        /// <summary>
        /// 保存玩家数据
        /// todo 保存前要先获取一次数据，部分数据要进行对比
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="humanRcd"></param>
        /// <returns></returns>
        private bool SaveRecord(int playerId, ref HumDataInfo humanRcd)
        {
            using var context = new StorageContext(_storageOption);
            var success = false;
            context.Open(ref success);
            if (!success)
            {
                return false;
            }
            var result = true;
            try
            {
                context.BeginTransaction();
                SaveRecord(context, playerId, humanRcd.Data);
                SaveAblity(context, playerId, humanRcd.Data.Abil);
                SaveItem(context, playerId, humanRcd.Data.HumItems);
                SaveBagItem(context, playerId, humanRcd.Data.BagItems);
                SaveStorageItem(context, playerId, humanRcd.Data.StorageItems);
                SaveMagics(context, playerId, humanRcd.Data.Magic);
                SaveBonusability(context, playerId, humanRcd.Data.BonusAbil);
                SaveStatus(context, playerId, humanRcd.Data.StatusTimeArr);
                context.Commit();
                _logger.Debug($"保存角色[{humanRcd.Header.sName}]数据成功");
            }
            catch (Exception ex)
            {
                result = false;
                context.RollBack();
                _logger.Error($"保存角色[{humanRcd.Header.sName}]数据失败. " + ex.Message);
            }
            finally
            {
                context.Dispose();
            }
            return result;
        }

        private void SaveRecord(StorageContext context, int playerId, HumInfoData hd)
        {
            var strSql = new StringBuilder();
            strSql.AppendLine("UPDATE characters SET ServerIndex = @ServerIndex, LoginID = @LoginID,MapName = @MapName, CX = @CX, CY = @CY, Level = @Level, Dir = @Dir, Hair = @Hair, Sex = @Sex, Job = Job, Gold = @Gold, ");
            strSql.AppendLine("GamePoint = @GamePoint, HomeMap = @HomeMap, HomeX = @HomeX, HomeY = @HomeY, PkPoint = @PkPoint, ReLevel = @ReLevel, AttatckMode = @AttatckMode, FightZoneDieCount = @FightZoneDieCount, BodyLuck = @BodyLuck, IncHealth = @IncHealth, IncSpell = @IncSpell,");
            strSql.AppendLine("IncHealing = @IncHealing, CreditPoint = @CreditPoint, BonusPoint =@BonusPoint, HungerStatus =@HungerStatus, PayMentPoint = @PayMentPoint, LockLogon = @LockLogon, MarryCount = @MarryCount, AllowGroupReCall = @AllowGroupReCall, ");
            strSql.AppendLine("GroupRcallTime = @GroupRcallTime, AllowGuildReCall = @AllowGuildReCall, IsMaster = @IsMaster, MasterName = @MasterName, DearName = @DearName, StoragePwd = @StoragePwd, Deleted = @Deleted,LASTUPDATE = now() WHERE ID = @ID;");
            var command = context.CreateCommand();
            command.CommandText = strSql.ToString();
            command.Parameters.AddWithValue("@Id", playerId);
            command.Parameters.AddWithValue("@ServerIndex", hd.ServerIndex);
            command.Parameters.AddWithValue("@LoginID", hd.Account);
            command.Parameters.AddWithValue("@MapName", hd.CurMap);
            command.Parameters.AddWithValue("@CX", hd.CurX);
            command.Parameters.AddWithValue("@CY", hd.CurY);
            command.Parameters.AddWithValue("@Level", hd.Abil.Level);
            command.Parameters.AddWithValue("@Dir", hd.Dir);
            command.Parameters.AddWithValue("@Hair", hd.Hair);
            command.Parameters.AddWithValue("@Sex", hd.Sex);
            command.Parameters.AddWithValue("@Job", hd.Job);
            command.Parameters.AddWithValue("@Gold", hd.Gold);
            command.Parameters.AddWithValue("@GamePoint", hd.GamePoint);
            command.Parameters.AddWithValue("@HomeMap", hd.HomeMap);
            command.Parameters.AddWithValue("@HomeX", hd.HomeX);
            command.Parameters.AddWithValue("@HomeY", hd.HomeY);
            command.Parameters.AddWithValue("@PkPoint", hd.PKPoint);
            command.Parameters.AddWithValue("@ReLevel", hd.ReLevel);
            command.Parameters.AddWithValue("@AttatckMode", hd.AttatckMode);
            command.Parameters.AddWithValue("@FightZoneDieCount", hd.FightZoneDieCount);
            command.Parameters.AddWithValue("@BodyLuck", hd.BodyLuck);
            command.Parameters.AddWithValue("@IncHealth", hd.IncHealth);
            command.Parameters.AddWithValue("@IncSpell", hd.IncSpell);
            command.Parameters.AddWithValue("@IncHealing", hd.IncHealing);
            command.Parameters.AddWithValue("@CreditPoint", hd.CreditPoint);
            command.Parameters.AddWithValue("@BonusPoint", hd.BonusPoint);
            command.Parameters.AddWithValue("@HungerStatus", hd.HungerStatus);
            command.Parameters.AddWithValue("@PayMentPoint", hd.PayMentPoint);
            command.Parameters.AddWithValue("@LockLogon", hd.LockLogon);
            command.Parameters.AddWithValue("@MarryCount", hd.MarryCount);
            command.Parameters.AddWithValue("@AllowGroupReCall", hd.AllowGroup);
            command.Parameters.AddWithValue("@GroupRcallTime", hd.GroupRcallTime);
            command.Parameters.AddWithValue("@AllowGuildReCall", hd.AllowGuildReCall);
            command.Parameters.AddWithValue("@IsMaster", hd.boMaster);
            command.Parameters.AddWithValue("@MasterName", hd.MasterName);
            command.Parameters.AddWithValue("@DearName", hd.DearName);
            command.Parameters.AddWithValue("@StoragePwd", hd.StoragePwd);
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

        private const string UpdateAblitySql = "UPDATE characters_ablity SET Level = @Level,Ac = @Ac, Mac = @Mac, Dc = @Dc, Mc = @Mc, Sc = @Sc, Hp = @Hp, Mp = @Mp, MaxHP = @MaxHP,MAxMP = @MAxMP, Exp = @Exp, MaxExp = @MaxExp, Weight = @Weight, MaxWeight = @MaxWeight, WearWeight = @WearWeight,MaxWearWeight = @MaxWearWeight, HandWeight = @HandWeight, MaxHandWeight = @MaxHandWeight,ModifyTime=now() WHERE PlayerId = @PlayerId;";
        
        private void SaveAblity(StorageContext context, int playerId, Ability Abil)
        {
            var command = context.CreateCommand();
            command.CommandText = UpdateAblitySql;
            command.Parameters.AddWithValue("@PlayerId", playerId);
            command.Parameters.AddWithValue("@Level", Abil.Level);
            command.Parameters.AddWithValue("@Ac", Abil.Level);
            command.Parameters.AddWithValue("@Mac", Abil.MAC);
            command.Parameters.AddWithValue("@Dc", Abil.DC);
            command.Parameters.AddWithValue("@Mc", Abil.MC);
            command.Parameters.AddWithValue("@Sc", Abil.SC);
            command.Parameters.AddWithValue("@Hp", Abil.HP);
            command.Parameters.AddWithValue("@Mp", Abil.MP);
            command.Parameters.AddWithValue("@MaxHP", Abil.MaxHP);
            command.Parameters.AddWithValue("@MAxMP", Abil.MaxMP);
            command.Parameters.AddWithValue("@Exp", Abil.Exp);
            command.Parameters.AddWithValue("@MaxExp", Abil.MaxExp);
            command.Parameters.AddWithValue("@Weight", Abil.Weight);
            command.Parameters.AddWithValue("@MaxWeight", Abil.MaxWeight);
            command.Parameters.AddWithValue("@WearWeight", Abil.WearWeight);
            command.Parameters.AddWithValue("@MaxWearWeight", Abil.MaxWearWeight);
            command.Parameters.AddWithValue("@HandWeight", Abil.HandWeight);
            command.Parameters.AddWithValue("@MaxHandWeight", Abil.MaxHandWeight);
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                _logger.Error("[Exception] PlayDataStorage.UpdateRecord");
            }
        }

        private void ComparerUserItem(UserItem[] newItems, UserItem[] oldItems, ref UserItem[] chg, ref UserItem[] del)
        {
            for (var i = 0; i < newItems.Length; i++)
            {
                if (oldItems[i] == null)
                {
                    continue;
                }
                if (newItems[i].MakeIndex == 0 && oldItems[i].MakeIndex > 0)
                {
                    del[i] = oldItems[i];
                    continue;
                }
                if (newItems[i].MakeIndex > 0 || newItems[i].MakeIndex > oldItems[i].MakeIndex)
                {
                    chg[i] = newItems[i];//差异化的数据
                    continue;
                }
                if (oldItems[i] == null && newItems[i].MakeIndex > 0)//历史位置没有物品，但是需要保存的位置有物品时，对数据进行更新操作
                {
                    chg[i] = newItems[i];//差异化的数据
                    continue;
                }
                if (oldItems[i].Index == 0 && oldItems[i].MakeIndex == 0 && newItems[i].MakeIndex > 0 && newItems[i].Index > 0)//穿戴位置没有任何数据
                {
                    del[i] = newItems[i];
                }
            }
        }

        private const string ClearUseItemSql= "UPDATE characters_item SET Position = @Position, MakeIndex = 0, StdIndex = 0, Dura = 0, DuraMax = 0 WHERE PlayerId = @PlayerId  AND Position = @Position AND MakeIndex = @MakeIndex AND StdIndex = @StdIndex;";
        private const string UpdateUseItemSql = "UPDATE characters_item SET Position = @Position, MakeIndex =@MakeIndex, StdIndex = @StdIndex, Dura = @Dura, DuraMax = @DuraMax WHERE PlayerId = @PlayerId AND Position = @Position;";
        private const string InsertUseItemSql = "INSERT INTO characters_item (PlayerId,ChrName, Position, MakeIndex, StdIndex, Dura, DuraMax) VALUES (@PlayerId,@ChrName, @Position, @MakeIndex, @StdIndex, @Dura, @DuraMax);";

        private void SaveItem(StorageContext context, int playerId, UserItem[] userItems)
        {
            var useSize = userItems.Length;
            var playData = new HumDataInfo();
            GetItemRecord(playerId, context, ref playData);
            var oldItems = playData.Data.HumItems;
            var useItemCount = oldItems.Where(x => x != null).Count(x => x.MakeIndex == 0 && x.Index == 0);
            var delItem = new UserItem[useSize];
            var chgList = new UserItem[useSize];
            ComparerUserItem(userItems, oldItems, ref chgList, ref delItem);
            try
            {
                if (delItem.Length > 0)
                {
                    for (var i = 0; i < delItem.Length; i++)
                    {
                        if (delItem[i] == null)
                        {
                            continue;
                        }
                        var command = context.CreateCommand();
                        command.CommandText = ClearUseItemSql;
                        command.Parameters.AddWithValue("@PlayerId", playerId);
                        command.Parameters.AddWithValue("@Position", i);
                        command.Parameters.AddWithValue("@MakeIndex", delItem[i].MakeIndex);
                        command.Parameters.AddWithValue("@StdIndex", delItem[i].Index);
                        command.ExecuteNonQuery();
                    }
                    try
                    {
                        ClearItemAttr(context, playerId, delItem.Where(x => x != null && x.MakeIndex > 0).Select(x => x.MakeIndex).ToList());
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("[Exception] PlayDataStorage.SaveItem (Clear Item)");
                        _logger.Error(ex.StackTrace);
                    }
                }

                if (chgList.Length > 0)
                {
                    for (var i = 0; i < chgList.Length; i++)
                    {
                        if (chgList[i] == null)
                        {
                            continue;
                        }
                        var command = context.CreateCommand();
                        command.CommandText = UpdateUseItemSql;
                        command.Parameters.AddWithValue("@PlayerId", playerId);
                        command.Parameters.AddWithValue("@Position", i);
                        command.Parameters.AddWithValue("@MakeIndex", chgList[i].MakeIndex);
                        command.Parameters.AddWithValue("@StdIndex", chgList[i].Index);
                        command.Parameters.AddWithValue("@Dura", chgList[i].Dura);
                        command.Parameters.AddWithValue("@DuraMax", chgList[i].DuraMax);
                        command.ExecuteNonQuery();
                    }
                    try
                    {
                        UpdateItemAttr(context, playerId, chgList);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("[Exception] PlayDataStorage.SaveItem (Update Item)");
                        _logger.Error(ex.StackTrace);
                    }
                }

                if (useItemCount <= 0)
                {
                    var addItem = new UserItem[useSize];
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
                    for (var i = 0; i < addItem.Length; i++)
                    {
                        if (addItem[i] == null)
                        {
                            continue;
                        }
                        var command = context.CreateCommand();
                        command.CommandText = InsertUseItemSql;
                        command.Parameters.AddWithValue("@PlayerId", playerId);
                        command.Parameters.AddWithValue("@Position", i);
                        command.Parameters.AddWithValue("@MakeIndex", addItem[i].MakeIndex);
                        command.Parameters.AddWithValue("@StdIndex", addItem[i].Index);
                        command.Parameters.AddWithValue("@Dura", addItem[i].Dura);
                        command.Parameters.AddWithValue("@DuraMax", addItem[i].DuraMax);
                        command.ExecuteNonQuery();
                    }
                    try
                    {
                        CreateItemAttr(context, playerId, addItem);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("[Exception] PlayDataStorage.SaveItem (Insert Item)");
                        _logger.Error(ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.SaveItem");
                _logger.Error(ex.StackTrace);
            }
        }

        private const string ClearBagItemSql= "UPDATE characters_bagitem SET Position = @Position, MakeIndex = 0, StdIndex = 0, Dura = 0, DuraMax = 0 WHERE PlayerId = @PlayerId  AND Position = @Position AND MakeIndex = @MakeIndex AND StdIndex = @StdIndex;";
        private const string UpdateBagItemSql = "UPDATE characters_bagitem SET Position = @Position, MakeIndex =@MakeIndex, StdIndex = @StdIndex, Dura = @Dura, DuraMax = @DuraMax WHERE PlayerId = @PlayerId AND Position = @Position;";
        private const string InsertBagItemSql = "INSERT INTO characters_bagitem (PlayerId,ChrName, Position, MakeIndex, StdIndex, Dura, DuraMax) VALUES (@PlayerId,@ChrName, @Position, @MakeIndex, @StdIndex, @Dura, @DuraMax);";
        
        private void SaveBagItem(StorageContext context, int playerId, UserItem[] bagItems)
        {
            try
            {
                var bagSize = bagItems.Length;
                var playData = new HumDataInfo();
                GetBagItemRecord(playerId, context, ref playData);
                var oldItems = playData.Data.BagItems;
                var newItems = bagItems;
                var delItem = new UserItem[bagSize];
                var chgList = new UserItem[bagSize];
                var bagItemCount = oldItems.Where(x => x != null).Count(x => x.MakeIndex == 0 && x.Index == 0);

                ComparerUserItem(newItems, oldItems, ref chgList, ref delItem);

                if (delItem.Length > 0)
                {
                    for (var i = 0; i < delItem.Length; i++)
                    {
                        if (delItem[i] == null)
                        {
                            continue;
                        }
                        var command = context.CreateCommand();
                        command.CommandText = ClearBagItemSql;
                        command.Parameters.AddWithValue("@PlayerId", playerId);
                        command.Parameters.AddWithValue("@Position", i);
                        command.Parameters.AddWithValue("@MakeIndex", delItem[i].MakeIndex);
                        command.Parameters.AddWithValue("@StdIndex", delItem[i].Index);
                        command.ExecuteNonQuery();
                    }
                    try
                    {
                        ClearItemAttr(context, playerId, delItem.Where(x => x != null && x.MakeIndex > 0).Select(x => x.MakeIndex).ToList());
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("[Exception] PlayDataStorage.UpdateBagItem (Delete Item)");
                        _logger.Error(ex.StackTrace);
                    }
                }
                if (chgList.Length > 0)
                {
                    for (var i = 0; i < chgList.Length; i++)
                    {
                        if (chgList[i] == null)
                        {
                            continue;
                        }
                        var command = context.CreateCommand();
                        command.CommandText = UpdateBagItemSql;
                        command.Parameters.AddWithValue("@PlayerId", playerId);
                        command.Parameters.AddWithValue("@Position", i);
                        command.Parameters.AddWithValue("@MakeIndex", chgList[i].MakeIndex);
                        command.Parameters.AddWithValue("@StdIndex", chgList[i].Index);
                        command.Parameters.AddWithValue("@Dura", chgList[i].Dura);
                        command.Parameters.AddWithValue("@DuraMax", chgList[i].DuraMax);
                        command.ExecuteNonQuery();
                    }
                    try
                    {
                        UpdateItemAttr(context, playerId, chgList);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("[Exception] PlayDataStorage.UpdateBagItem (Update Item)");
                        _logger.Error(ex.StackTrace);
                    }
                }
                if (bagItemCount <= 0)
                {
                    var addItem = new UserItem[bagSize];
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
                    for (var i = 0; i < addItem.Length; i++)
                    {
                        var command = context.CreateCommand();
                        command.CommandText = InsertBagItemSql;
                        command.Parameters.AddWithValue("@PlayerId", playerId);
                        command.Parameters.AddWithValue("@Position", i);
                        command.Parameters.AddWithValue("@MakeIndex", addItem[i].MakeIndex);
                        command.Parameters.AddWithValue("@StdIndex", addItem[i].Index);
                        command.Parameters.AddWithValue("@Dura", addItem[i].Dura);
                        command.Parameters.AddWithValue("@DuraMax", addItem[i].DuraMax);
                        command.ExecuteNonQuery();
                    }
                    try
                    {
                        CreateItemAttr(context, playerId, addItem);
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

        private const string ClearStorageItemSql="UPDATE characters_storageitem SET Position = @Position, MakeIndex = 0, StdIndex = 0, Dura = 0, DuraMax = 0 WHERE PlayerId = @PlayerId  AND Position = @Position AND MakeIndex = @MakeIndex AND StdIndex = @StdIndex;";
        private const string UpdateStorageItemSql = "UPDATE characters_storageitem SET Position = @Position, MakeIndex =@MakeIndex, StdIndex = @StdIndex, Dura = @Dura, DuraMax = @DuraMax WHERE PlayerId = @PlayerId AND Position = @Position;";
        private const string InsertStorageItemSql = "INSERT INTO characters_storageitem (PlayerId,ChrName, Position, MakeIndex, StdIndex, Dura, DuraMax) VALUES (@PlayerId,@ChrName, @Position, @MakeIndex, @StdIndex, @Dura, @DuraMax);";
        
        private void SaveStorageItem(StorageContext context, int playerId, UserItem[] storageItems)
        {
            try
            {
                var storageSize = storageItems.Length;
                var playData = new HumDataInfo();
                GetStorageRecord(playerId, context, ref playData);
                var oldItems = playData.Data.StorageItems;
                var newItems = storageItems;
                var delItem = new UserItem[storageSize];
                var chgList = new UserItem[storageSize];
                var storageItemCount = oldItems.Where(x => x != null).Count(x => x.MakeIndex == 0 && x.Index == 0);
                ComparerUserItem(newItems, oldItems, ref chgList, ref delItem);

                if (delItem.Length > 0)
                {
                    for (var i = 0; i < delItem.Length; i++)
                    {
                        if (delItem[i] == null)
                        {
                            continue;
                        }
                        var command = context.CreateCommand();
                        command.CommandText = ClearStorageItemSql;
                        command.Parameters.AddWithValue("@PlayerId", playerId);
                        command.Parameters.AddWithValue("@Position", i);
                        command.Parameters.AddWithValue("@MakeIndex", delItem[i].MakeIndex);
                        command.Parameters.AddWithValue("@StdIndex", delItem[i].Index);
                        command.ExecuteNonQuery();
                    }
                    try
                    {
                        ClearItemAttr(context, playerId, delItem.Where(x => x != null && x.MakeIndex > 0).Select(x => x.MakeIndex).ToList());
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("[Exception] PlayDataStorage.SaveStorageItem (Delete Item)");
                        _logger.Error(ex.StackTrace);
                    }
                }

                if (chgList.Length > 0)
                {
                    for (var i = 0; i < chgList.Length; i++)
                    {
                        if (chgList[i] == null)
                        {
                            continue;
                        }
                        var command = context.CreateCommand();
                        command.CommandText = UpdateStorageItemSql;
                        command.Parameters.AddWithValue("@PlayerId", playerId);
                        command.Parameters.AddWithValue("@Position", i);
                        command.Parameters.AddWithValue("@MakeIndex", chgList[i].MakeIndex);
                        command.Parameters.AddWithValue("@StdIndex", chgList[i].Index);
                        command.Parameters.AddWithValue("@Dura", chgList[i].Dura);
                        command.Parameters.AddWithValue("@DuraMax", chgList[i].DuraMax);
                        command.ExecuteNonQuery();
                    }
                    try
                    {
                        UpdateItemAttr(context, playerId, chgList);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("[Exception] PlayDataStorage.SaveStorageItem (Update Item)");
                        _logger.Error(ex.StackTrace);
                    }
                }

                if (storageItemCount <= 0)
                {
                    var addItem = new UserItem[storageSize];
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
                    try
                    {
                        for (var i = 0; i < addItem.Length; i++)
                        {
                            var command = context.CreateCommand();
                            command.CommandText = InsertStorageItemSql;
                            command.Parameters.AddWithValue("@PlayerId", playerId);
                            command.Parameters.AddWithValue("@Position", i);
                            command.Parameters.AddWithValue("@MakeIndex", addItem[i].MakeIndex);
                            command.Parameters.AddWithValue("@StdIndex", addItem[i].Index);
                            command.Parameters.AddWithValue("@Dura", addItem[i].Dura);
                            command.Parameters.AddWithValue("@DuraMax", addItem[i].DuraMax);
                            command.ExecuteNonQuery();
                        }
                        CreateItemAttr(context, playerId, addItem);
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

        private void SaveMagics(StorageContext context, int playerId, MagicRcd[] humanRcd)
        {
            var delcommand = context.CreateCommand();
            delcommand.CommandText = "DELETE FROM characters_magic WHERE PlayerId=@PlayerId";
            delcommand.Parameters.AddWithValue("@PlayerId", playerId);
            delcommand.ExecuteNonQuery();
            try
            {
                const string sStrSql = "INSERT INTO characters_magic(PlayerId,MagicId,Level,Usekey,CurrTrain) VALUES ({0},{1},{2},{3},{4});";
                var strSqlList = new List<string>();
                for (var i = 0; i < humanRcd.Length; i++)
                {
                    if (humanRcd[i].MagIdx > 0)
                    {
                        strSqlList.Add(string.Format(sStrSql, playerId, humanRcd[i].MagIdx, humanRcd[i].Level, humanRcd[i].MagicKey, humanRcd[i].TranPoint));
                    }
                }
                if (strSqlList.Count <= 0)
                {
                    return;
                }
                var command = context.CreateCommand();
                command.CommandText = string.Join("\r\n", strSqlList);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.SaveMagics");
                _logger.Error(ex.StackTrace);
            }
        }

        private void SaveBonusability(StorageContext context, int playerId, NakedAbility bonusAbil)
        {
            const string sSqlStr = "UPDATE characters_bonusability SET AC=@AC, MAC=@MAC, DC=@DC, MC=@MC, SC=@SC, HP=@HP, MP=@MP, HIT=@HIT, SPEED=@SPEED, RESERVED=@RESERVED WHERE PlayerId=@PlayerId";
            try
            {
                var command = context.CreateCommand();
                command.CommandText = sSqlStr;
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
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.SaveBonusability");
                _logger.Error(ex.StackTrace);
            }
        }

        private void SaveQuest(StorageContext context, int id, HumDataInfo humanRcd)
        {
            const string sSqlStr4 = "DELETE FROM characters_quest WHERE PlayerId=@PlayerId";
            const string sSqlStr5 = "INSERT INTO characters_quest (PlayerId, QUESTOPENINDEX, QUESTFININDEX, QUEST) VALUES(@PlayerId, @QUESTOPENINDEX, @QUESTFININDEX, @QUEST)";
            try
            {
                var command = context.CreateCommand();
                command.CommandText = sSqlStr4;
                command.Parameters.AddWithValue("@PlayerId", id);
                command.ExecuteNonQuery();
            }
            catch
            {
                _logger.Error("[Exception] PlayDataStorage.SaveQuest");
            }
        }


        private void SaveStatus(StorageContext context, int playerId, ushort[] statusTimeArr)
        {
            try
            {
                const string UpdatrStatusSql = "UPDATE characters_status SET Status0=@Status0,Status1=@Status1,Status2=@Status2,Status3=@Status3,Status4=@Status4,Status5=@Status5,Status6=@Status6,Status7=@Status7,Status8=@Status8,Status9=@Status9,Status10=@Status10,Status11=@Status11,Status12=@Status12,Status13=@Status13,Status14=@Status14,Status15=@Status15 WHERE PlayerId=@PlayerId;";
                var command = context.CreateCommand();
                command.CommandText = UpdatrStatusSql;
                command.Parameters.AddWithValue("@PlayerId", playerId);
                command.Parameters.AddWithValue("@Status0", statusTimeArr[0]);
                command.Parameters.AddWithValue("@Status1", statusTimeArr[1]);
                command.Parameters.AddWithValue("@Status2", statusTimeArr[2]);
                command.Parameters.AddWithValue("@Status3", statusTimeArr[3]);
                command.Parameters.AddWithValue("@Status4", statusTimeArr[4]);
                command.Parameters.AddWithValue("@Status5", statusTimeArr[5]);
                command.Parameters.AddWithValue("@Status6", statusTimeArr[6]);
                command.Parameters.AddWithValue("@Status7", statusTimeArr[7]);
                command.Parameters.AddWithValue("@Status8", statusTimeArr[8]);
                command.Parameters.AddWithValue("@Status9", statusTimeArr[9]);
                command.Parameters.AddWithValue("@Status10", statusTimeArr[10]);
                command.Parameters.AddWithValue("@Status11", statusTimeArr[11]);
                command.Parameters.AddWithValue("@Status12", statusTimeArr[12]);
                command.Parameters.AddWithValue("@Status13", statusTimeArr[13]);
                command.Parameters.AddWithValue("@Status14", statusTimeArr[14]);
                command.Parameters.AddWithValue("@Status15", statusTimeArr[15]);
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
            const string sqlString = "UPDATE characters SET DELETED=1, LastUpdate=now() WHERE ID=@Id";
            var result = true;
            var playerId = _quickIndexIdMap[nIndex];
            using var context = new StorageContext(_storageOption);
            var success = false;
            context.Open(ref success);
            if (!success)
            {
                return false;
            }
            try
            {
                var command = context.CreateCommand();
                command.CommandText = sqlString;
                command.Parameters.AddWithValue("@Id", playerId);
                command.ExecuteNonQuery();
            }
            catch
            {
                result = false;
                _logger.Error("[Exception] PlayDataStorage.DeleteRecord");
            }
            finally
            {
                context.Dispose();
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

        public bool GetQryChar(int nIndex, ref QueryChr queryChrRcd)
        {
            const string sSql = "SELECT * FROM characters WHERE ID=@Id";
            if (nIndex < 0)
            {
                return false;
            }
            if (_quickIndexIdMap.Count <= nIndex)
            {
                return false;
            }
            var playerId = _quickIndexIdMap[nIndex];
            using var context = new StorageContext(_storageOption);
            var success = false;
            context.Open(ref success);
            if (!success)
            {
                return false;
            }
            try
            {
                var command = context.CreateCommand();
                command.CommandText = sSql;
                command.Parameters.AddWithValue("@Id", playerId);
                using var dr = command.ExecuteReader();
                if (dr.Read())
                {
                    queryChrRcd = new QueryChr();
                    queryChrRcd.Name = dr.GetString("ChrName");
                    queryChrRcd.Job = dr.GetByte("Job");
                    queryChrRcd.Hair = dr.GetByte("Hair");
                    queryChrRcd.Sex = dr.GetByte("Sex");
                    queryChrRcd.Level = dr.GetUInt16("Level");
                }
                dr.Close();
                dr.Dispose();
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.GetQryChar");
                return false;
            }
            finally
            {
                context.Dispose();
            }
            return true;
        }

        private void ClearItemAttr(StorageContext context, int playerId, IList<int> makeIndex)
        {
            if (!makeIndex.Any())
            {
                return;
            }
            const string ClearItemAttrSql = "UPDATE characters_item_attr SET MakeIndex=0,VALUE0=0,VALUE1=0,VALUE2=0,VALUE3=0,VALUE4=0,VALUE5=0,VALUE6=0,VALUE7=0,VALUE8=0,VALUE9=0,VALUE10=0,VALUE11=0,VALUE12=0,VALUE13=0 WHERE PlayerId={0} AND MakeIndex ={1};";
            try
            {
                var strSqlList = new List<string>();
                for (int i = 0; i < makeIndex.Count(); i++)
                {
                    strSqlList.Add(string.Format(ClearItemAttrSql, playerId, makeIndex[i]));
                }
                var command = context.CreateCommand();
                command.CommandText = string.Join("\r\n", strSqlList);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.ClearItemAttr");
                _logger.Error(ex.StackTrace);
            }
        }

        private void UpdateItemAttr(StorageContext context, int playerId, UserItem[] userItems)
        {
            try
            {
                const string updateItemAttrSql = "UPDATE characters_item_attr SET VALUE0={0},VALUE1={1},VALUE2={2},VALUE3={3},VALUE4={4},VALUE5={5},VALUE6={6},VALUE7={7},VALUE8={8},VALUE9={9},VALUE10={10},VALUE11={11},VALUE12={12},VALUE13={13} WHERE PlayerId={14} AND MakeIndex={15};";
                var strSqlList = new List<string>();
                for (var i = 0; i < userItems.Length; i++)
                {
                    if (userItems[i] == null)
                    {
                        continue;
                    }
                    var userItem = userItems[i];
                    strSqlList.Add(string.Format(updateItemAttrSql, userItem.Desc[0], userItem.Desc[1],
                        userItem.Desc[2], userItem.Desc[3], userItem.Desc[4], userItem.Desc[5], userItem.Desc[6], userItem.Desc[7], userItem.Desc[8], userItem.Desc[9],
                        userItem.Desc[10], userItem.Desc[11], userItem.Desc[12], userItem.Desc[13], playerId, userItem.MakeIndex));
                }
                if (strSqlList.Count <= 0)
                {
                    return;
                }
                var command = context.CreateCommand();
                command.CommandText = string.Join("\r\n", strSqlList);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Error("[Exception] PlayDataStorage.UpdateItemAttr (Update item attr)");
                _logger.Error(e.StackTrace);
            }
        }


        private void CreateItemAttr(StorageContext context, int playerId, UserItem[] userItems)
        {
            try
            {
                const string insertItemAttrSql = "INSERT INTO characters_item_attr (PlayerId,MakeIndex,VALUE0,VALUE1,VALUE2,VALUE3,VALUE4,VALUE5,VALUE6,VALUE7,VALUE8,VALUE9,VALUE10,VALUE11,VALUE12,VALUE13) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15});";
                var strSqlList = new List<string>();
                for (var i = 0; i < userItems.Length; i++)
                {
                    if (userItems[i] == null || userItems[i].MakeIndex <= 0)
                    {
                        continue;
                    }
                    var userItem = userItems[i];
                    strSqlList.Add(string.Format(insertItemAttrSql, playerId, userItem.MakeIndex, userItem.Desc[0], userItem.Desc[1], userItem.Desc[2], userItem.Desc[3], userItem.Desc[4], userItem.Desc[5], userItem.Desc[6], userItem.Desc[7], userItem.Desc[8], userItem.Desc[9], userItem.Desc[10], userItem.Desc[11], userItem.Desc[12]));
                }
                if (strSqlList.Count <= 0)
                {
                    return;
                }
                var command = context.CreateCommand();
                command.CommandText = string.Join("\r\n", strSqlList);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Error("[Exception] PlayDataStorage.UpdateRecord (Insert item attr)");
                _logger.Error(e.StackTrace);
            }
        }

        private void DeleteItemAttr(StorageContext context, int playerId, IEnumerable<int> makeIndex)
        {
            try
            {
                var command = context.CreateCommand();
                command.CommandText = "DELETE FROM characters_item_attr WHERE PlayerId=@PlayerId AND MakeIndex in (@MakeIndex)";
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

        private void QueryItemAttr(StorageContext context, int playerId, ref UserItem[] userItems)
        {
            var makeIndexs = userItems.Where(x => x != null && x.MakeIndex > 0).Select(x => x.MakeIndex);
            if (!makeIndexs.Any())
            {
                return;
            }
            const string sSqlString = "SELECT * FROM characters_item_attr WHERE PlayerId=@PlayerId and MakeIndex in (@MakeIndex)";
            try
            {
                var command = context.CreateCommand();
                command.CommandText = sSqlString;
                command.Parameters.AddWithValue("@PlayerId", playerId);
                command.Parameters.AddWithValue("@MakeIndex", string.Join(",", makeIndexs));
                using var dr = command.ExecuteReader();
                var nPosition = 0;
                while (dr.Read())
                {
                    userItems[nPosition].Desc[nPosition] = dr.GetByte($"VALUE{nPosition}");
                    nPosition++;
                }
                dr.Close();
                dr.Dispose();
            }
            catch (Exception ex)
            {
                _logger.Error("[Exception] PlayDataStorage.QueryItemAttr:" + ex.StackTrace);
            }
        }
    }
}