using OpenMir2;
using OpenMir2.Packets.ClientPackets;
using OpenMir2.Packets.ServerPackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBSrv.Storage.MySQL
{
    public partial class PlayDataStorage : IPlayDataStorage
    {
        /// <summary>
        /// 检查角色名称是否存在
        /// </summary>
        /// <returns></returns>
        private bool CheckChrExists(string sChrName)
        {
            if (_NameQuickMap.ContainsKey(sChrName))
            {
                return true;
            }
            if (_NameQuickMap.TryGetValue(sChrName, out int nIndex))
            {
                if (nIndex == 0)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public bool Add(CharacterDataInfo humanRcd)
        {
            string sChrName = humanRcd.Header.Name;
            if (CheckChrExists(sChrName))
            {
                return false;
            }
            int nIndex = 0;
            if (AddRecord(ref nIndex, ref humanRcd))
            {
                _NameQuickMap.Add(sChrName, nIndex);
                _IndexQuickIdMap.Add(nIndex, nIndex);
                return true;
            }
            return false;
        }

        private bool AddRecord(ref int nIndex, ref CharacterDataInfo humanRcd)
        {
            using StorageContext context = new StorageContext(_storageOption);
            bool success = false;
            context.Open(ref success);
            if (!success)
            {
                return false;
            }
            bool result = false;
            try
            {
                context.BeginTransaction();
                int playerId = CreateCharacters(context, humanRcd);
                if (playerId <= 0)
                {
                    return false;
                }
                CreateAblity(context, playerId, humanRcd.Data);
                CreateStatus(context, playerId);
                CreateUseItem(context, playerId);
                CreateBagItem(context, playerId);
                CreateStorageItem(context, playerId);
                result = true;
                nIndex = playerId;
                context.Commit();
            }
            catch (Exception e)
            {
                context.RollBack();
                LogService.Error("创建角色失败" + e.StackTrace);
            }
            finally
            {
                context.Dispose();
            }
            return result;
        }

        private int CreateCharacters(StorageContext context, CharacterDataInfo humanRcd)
        {
            CharacterData hd = humanRcd.Data;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("insert into characters (ServerIndex, LoginID, ChrName, MapName, CX, CY, Level, Dir, Hair, Sex, Job, Gold, GamePoint, HomeMap,");
            strSql.AppendLine("HomeX, HomeY, PkPoint, ReLevel, AttatckMode, FightZoneDieCount, BodyLuck, IncHealth,IncSpell, IncHealing, CreditPoint, BonusPoint,");
            strSql.AppendLine("HungerStatus, PayMentPoint, LockLogon, MarryCount, AllowGroupReCall, GroupRcallTime, AllowGuildReCall, IsMaster, MasterName, DearName");
            strSql.AppendLine(",StoragePwd, Deleted, CREATEDATE, LASTUPDATE) VALUES ");
            strSql.AppendLine("(@ServerIndex, @LoginID, @ChrName, @MapName, @CX, @CY, @Level, @Dir, @Hair, @Sex, @Job, @Gold, @GamePoint, @HomeMap,");
            strSql.AppendLine("@HomeX, @HomeY, @PkPoint, @ReLevel, @AttatckMode, @FightZoneDieCount, @BodyLuck, @IncHealth,@IncSpell, @IncHealing, @CreditPoint, @BonusPoint,");
            strSql.AppendLine("@HungerStatus, @PayMentPoint, @LockLogon, @MarryCount, @AllowGroupReCall, @GroupRcallTime, @AllowGuildReCall, @IsMaster, @MasterName, @DearName");
            strSql.AppendLine(",@StoragePwd, @Deleted,now(),now());");

            MySqlConnector.MySqlCommand command = context.CreateCommand();
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
            command.Parameters.AddWithValue("@IsMaster", hd.IsMaster);
            command.Parameters.AddWithValue("@MasterName", hd.MasterName);
            command.Parameters.AddWithValue("@DearName", hd.DearName);
            command.Parameters.AddWithValue("@StoragePwd", hd.StoragePwd);
            command.Parameters.AddWithValue("@Deleted", 0);
            try
            {
                command.ExecuteNonQuery();
                return (int)command.LastInsertedId;
            }
            catch (Exception ex)
            {
                context.RollBack();
                LogService.Error("[Exception] PlayDataStorage.CreateCharacters");
                LogService.Error(ex.StackTrace);
                return 0;
            }
        }

        private void CreateAblity(StorageContext context, int playerId, CharacterData hd)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine("INSERT INTO characters_ablity (PlayerId, Level, Ac, Mac, Dc, Mc, Sc, Hp, Mp, MaxHP, MAxMP, Exp, MaxExp,");
                strSql.AppendLine(" Weight, MaxWeight, WearWeight,MaxWearWeight, HandWeight, MaxHandWeight) VALUES ");
                strSql.AppendLine(" (@PlayerId, @Level, @Ac, @Mac, @Dc, @Mc, @Sc, @Hp, @Mp, @MaxHP, @MAxMP, @Exp, @MaxExp, @Weight, @MaxWeight, @WearWeight, @MaxWearWeight, @HandWeight, @MaxHandWeight) ");
                MySqlConnector.MySqlCommand command = context.CreateCommand();
                command.CommandText = strSql.ToString();
                command.Parameters.Clear();
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
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                LogService.Error(e.StackTrace);
            }
        }

        private void CreateStatus(StorageContext context, int playerId)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendLine("INSERT INTO characters_status (PlayerId, Status0, Status1, Status2, Status3, Status4, Status5, Status6, Status7, Status8, Status9, Status10, Status11, Status12, Status13, Status14, Status15) VALUES ");
                strSql.AppendLine("(@PlayerId, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);");
                MySqlConnector.MySqlCommand command = context.CreateCommand();
                command.CommandText = strSql.ToString();
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@PlayerId", playerId);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                LogService.Error(e.StackTrace);
            }
        }

        private void CreateUseItem(StorageContext context, int playerId)
        {
            const string InsertUseItemSql = "INSERT INTO characters_item (PlayerId,Position,MakeIndex,StdIndex,Dura,DuraMax) VALUES (@PlayerId, @Position, @MakeIndex, @StdIndex, @Dura, @DuraMax);";
            CharacterDataInfo playData = new CharacterDataInfo();
            GetItemRecord(playerId, context, ref playData);
            ServerUserItem[] oldItems = playData.Data.HumItems;
            int useItemCount = oldItems.Where(x => x != null).Count(x => x.MakeIndex == 0 && x.Index == 0);
            int useSize = oldItems.Length;
            if (useItemCount <= 0)
            {
                UserItem[] addItem = new UserItem[useSize];
                int addItemCount = 0;
                if (useItemCount == 0)
                {
                    addItemCount = useSize;
                }
                addItemCount = useSize - addItemCount < 0 ? useSize : addItemCount - (useSize - addItemCount);
                if (addItemCount > 0)
                {
                    for (int i = 0; i < addItemCount; i++)
                    {
                        addItem[i] = new UserItem();
                    }
                }
                for (int i = 0; i < addItem.Length; i++)
                {
                    if (addItem[i] == null)
                    {
                        continue;
                    }
                    MySqlConnector.MySqlCommand command = context.CreateCommand();
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
                    LogService.Error("[Exception] PlayDataStorage.SaveItem (Insert Item)");
                    LogService.Error(ex.StackTrace);
                }
            }
        }

        private void CreateBagItem(StorageContext context, int playerId)
        {
            const string InsertBagItemSql = "INSERT INTO characters_bagitem (PlayerId, Position, MakeIndex, StdIndex, Dura, DuraMax) VALUES (@PlayerId, @Position, @MakeIndex, @StdIndex, @Dura, @DuraMax);";
            CharacterDataInfo playData = new CharacterDataInfo();
            GetBagItemRecord(playerId, context, ref playData);
            ServerUserItem[] oldItems = playData.Data.BagItems;
            int bagItemCount = oldItems.Where(x => x != null).Count(x => x.MakeIndex == 0 && x.Index == 0);
            int bagSize = oldItems.Length;
            if (bagItemCount <= 0)
            {
                UserItem[] addItem = new UserItem[bagSize];
                int addItemCount = 0;
                if (bagItemCount == 0)
                {
                    addItemCount = bagSize;
                }
                addItemCount = bagSize - addItemCount < 0 ? 0 : addItemCount - (bagSize - addItemCount);
                if (addItemCount > 0)
                {
                    for (int i = 0; i < addItemCount; i++)
                    {
                        addItem[i] = new UserItem();
                    }
                }
                for (int i = 0; i < addItem.Length; i++)
                {
                    MySqlConnector.MySqlCommand command = context.CreateCommand();
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
                    LogService.Error("[Exception] PlayDataStorage.UpdateBagItem (Insert Item)");
                    LogService.Error(ex.StackTrace);
                }
            }
        }

        private void CreateStorageItem(StorageContext context, int playerId)
        {
            const string InsertStorageItemSql = "INSERT INTO characters_storageitem (PlayerId, Position, MakeIndex, StdIndex, Dura, DuraMax) VALUES (@PlayerId, @Position, @MakeIndex, @StdIndex, @Dura, @DuraMax);";
            CharacterDataInfo playData = new CharacterDataInfo();
            GetStorageRecord(playerId, context, ref playData);
            ServerUserItem[] oldItems = playData.Data.StorageItems;
            int storageItemCount = oldItems.Where(x => x != null).Count(x => x.MakeIndex == 0 && x.Index == 0);
            int storageSize = oldItems.Length;
            if (storageItemCount <= 0)
            {
                UserItem[] addItem = new UserItem[storageSize];
                int addItemCount = 0;
                if (storageItemCount == 0)
                {
                    addItemCount = storageSize;
                }
                addItemCount = storageSize - addItemCount < 0 ? 0 : addItemCount - (storageSize - addItemCount);
                if (addItemCount > 0)
                {
                    for (int i = 0; i < addItemCount; i++)
                    {
                        addItem[i] = new UserItem();
                    }
                }
                try
                {
                    for (int i = 0; i < addItem.Length; i++)
                    {
                        MySqlConnector.MySqlCommand command = context.CreateCommand();
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
                    LogService.Error("[Exception] PlayDataStorage.SaveStorageItem (Insert Item)");
                    LogService.Error(e.StackTrace);
                }
            }
        }

        private void CreateItemAttr(StorageContext context, int playerId, UserItem[] userItems)
        {
            try
            {
                const string insertItemAttrSql = "INSERT INTO characters_item_attr (PlayerId,MakeIndex,VALUE0,VALUE1,VALUE2,VALUE3,VALUE4,VALUE5,VALUE6,VALUE7,VALUE8,VALUE9,VALUE10,VALUE11,VALUE12,VALUE13) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15});";
                List<string> strSqlList = new List<string>();
                for (int i = 0; i < userItems.Length; i++)
                {
                    if (userItems[i] == null || userItems[i].MakeIndex <= 0)
                    {
                        continue;
                    }
                    UserItem userItem = userItems[i];
                    strSqlList.Add(string.Format(insertItemAttrSql, playerId, userItem.MakeIndex, userItem.Desc[0], userItem.Desc[1], userItem.Desc[2], userItem.Desc[3], userItem.Desc[4], userItem.Desc[5], userItem.Desc[6], userItem.Desc[7], userItem.Desc[8], userItem.Desc[9], userItem.Desc[10], userItem.Desc[11], userItem.Desc[12], userItem.Desc[13]));
                }
                if (strSqlList.Count <= 0)
                {
                    return;
                }
                MySqlConnector.MySqlCommand command = context.CreateCommand();
                command.CommandText = string.Join("\r\n", strSqlList);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                LogService.Error("[Exception] PlayDataStorage.UpdateRecord (Insert item attr)");
                LogService.Error(e.StackTrace);
            }
        }
    }
}