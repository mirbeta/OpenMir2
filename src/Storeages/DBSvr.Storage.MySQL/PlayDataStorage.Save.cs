using DBSrv.Storage.Model;
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
        public bool Update(string chrName, CharacterDataInfo humanRcd)
        {
            if (_NameQuickMap.TryGetValue(chrName, out int playerId))
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
            bool result = false;
            if ((nIndex >= 0) && (_NameQuickMap.Count > nIndex))
            {
                if (UpdateChrRecord(nIndex, queryChrRcd))
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 保存玩家数据
        /// todo 保存前要先获取一次数据，部分数据要进行对比
        /// </summary>
        /// <returns></returns>
        private bool SaveRecord(int playerId, ref CharacterDataInfo humanRcd)
        {
            using StorageContext context = new StorageContext(_storageOption);
            bool success = false;
            context.Open(ref success);
            if (!success)
            {
                return false;
            }
            bool result = true;
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
                LogService.Debug($"保存角色[{humanRcd.Header.Name}]数据成功");
            }
            catch (Exception ex)
            {
                result = false;
                context.RollBack();
                LogService.Error($"保存角色[{humanRcd.Header.Name}]数据失败. " + ex.Message);
            }
            finally
            {
                context.Dispose();
            }
            return result;
        }

        private void SaveRecord(StorageContext context, int playerId, CharacterData hd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("UPDATE characters SET ServerIndex = @ServerIndex, LoginID = @LoginID,MapName = @MapName, CX = @CX, CY = @CY, Level = @Level, Dir = @Dir, Hair = @Hair, Sex = @Sex, Job = Job, Gold = @Gold, ");
            strSql.AppendLine("GamePoint = @GamePoint, HomeMap = @HomeMap, HomeX = @HomeX, HomeY = @HomeY, PkPoint = @PkPoint, ReLevel = @ReLevel, AttatckMode = @AttatckMode, FightZoneDieCount = @FightZoneDieCount, BodyLuck = @BodyLuck, IncHealth = @IncHealth, IncSpell = @IncSpell,");
            strSql.AppendLine("IncHealing = @IncHealing, CreditPoint = @CreditPoint, BonusPoint =@BonusPoint, HungerStatus =@HungerStatus, PayMentPoint = @PayMentPoint, LockLogon = @LockLogon, MarryCount = @MarryCount, AllowGroupReCall = @AllowGroupReCall, ");
            strSql.AppendLine("GroupRcallTime = @GroupRcallTime, AllowGuildReCall = @AllowGuildReCall, IsMaster = @IsMaster, MasterName = @MasterName, DearName = @DearName, StoragePwd = @StoragePwd, Deleted = @Deleted,LASTUPDATE = now() WHERE ID = @ID;");
            MySqlConnector.MySqlCommand command = context.CreateCommand();
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
            command.Parameters.AddWithValue("@IsMaster", hd.IsMaster);
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
                LogService.Error("[Exception] PlayDataStorage.UpdateRecord:" + ex.Message);
            }
        }

        private void SaveAblity(StorageContext context, int playerId, Ability Abil)
        {
            const string UpdateAblitySql = "UPDATE characters_ablity SET Level = @Level,Ac = @Ac, Mac = @Mac, Dc = @Dc, Mc = @Mc, Sc = @Sc, Hp = @Hp, Mp = @Mp, MaxHP = @MaxHP,MAxMP = @MAxMP, Exp = @Exp, MaxExp = @MaxExp, Weight = @Weight, MaxWeight = @MaxWeight, WearWeight = @WearWeight,MaxWearWeight = @MaxWearWeight, HandWeight = @HandWeight, MaxHandWeight = @MaxHandWeight,ModifyTime=now() WHERE PlayerId = @PlayerId;";
            MySqlConnector.MySqlCommand command = context.CreateCommand();
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
                LogService.Error("[Exception] PlayDataStorage.UpdateRecord");
            }
        }

        private void ComparerUserItem(ServerUserItem[] newItems, ServerUserItem[] oldItems, ref ServerUserItem[] chg, ref ServerUserItem[] del)
        {
            for (int i = 0; i < newItems.Length; i++)
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

        private const string ClearUseItemSql = "UPDATE characters_item SET Position = @Position, MakeIndex = 0, StdIndex = 0, Dura = 0, DuraMax = 0 WHERE PlayerId = @PlayerId  AND Position = @Position AND MakeIndex = @MakeIndex AND StdIndex = @StdIndex;";
        private const string UpdateUseItemSql = "UPDATE characters_item SET Position = @Position, MakeIndex =@MakeIndex, StdIndex = @StdIndex, Dura = @Dura, DuraMax = @DuraMax WHERE PlayerId = @PlayerId AND Position = @Position;";

        private void SaveItem(StorageContext context, int playerId, ServerUserItem[] userItems)
        {
            int useSize = userItems.Length;
            CharacterDataInfo playData = new CharacterDataInfo();
            GetItemRecord(playerId, context, ref playData);
            ServerUserItem[] oldItems = playData.Data.HumItems;
            int useItemCount = oldItems.Where(x => x != null).Count(x => x.MakeIndex == 0 && x.Index == 0);
            ServerUserItem[] delItem = new ServerUserItem[useSize];
            ServerUserItem[] chgList = new ServerUserItem[useSize];
            ComparerUserItem(userItems, oldItems, ref chgList, ref delItem);
            try
            {
                if (delItem.Length > 0)
                {
                    for (int i = 0; i < delItem.Length; i++)
                    {
                        if (delItem[i] == null)
                        {
                            continue;
                        }
                        MySqlConnector.MySqlCommand command = context.CreateCommand();
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
                        LogService.Error("[Exception] PlayDataStorage.SaveItem (Clear Item)");
                        LogService.Error(ex.StackTrace);
                    }
                }

                if (chgList.Length > 0)
                {
                    for (int i = 0; i < chgList.Length; i++)
                    {
                        if (chgList[i] == null)
                        {
                            continue;
                        }
                        MySqlConnector.MySqlCommand command = context.CreateCommand();
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
                        LogService.Error("[Exception] PlayDataStorage.SaveItem (Update Item)");
                        LogService.Error(ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Error("[Exception] PlayDataStorage.SaveItem");
                LogService.Error(ex.StackTrace);
            }
        }

        private const string ClearBagItemSql = "UPDATE characters_bagitem SET Position = @Position, MakeIndex = 0, StdIndex = 0, Dura = 0, DuraMax = 0 WHERE PlayerId = @PlayerId  AND Position = @Position AND MakeIndex = @MakeIndex AND StdIndex = @StdIndex;";
        private const string UpdateBagItemSql = "UPDATE characters_bagitem SET Position = @Position, MakeIndex =@MakeIndex, StdIndex = @StdIndex, Dura = @Dura, DuraMax = @DuraMax WHERE PlayerId = @PlayerId AND Position = @Position;";

        private void SaveBagItem(StorageContext context, int playerId, ServerUserItem[] bagItems)
        {
            try
            {
                CharacterDataInfo playData = new CharacterDataInfo();
                GetBagItemRecord(playerId, context, ref playData);
                ServerUserItem[] oldItems = playData.Data.BagItems;
                int bagSize = bagItems.Length;
                ServerUserItem[] newItems = bagItems;
                ServerUserItem[] delItem = new ServerUserItem[bagSize];
                ServerUserItem[] chgList = new ServerUserItem[bagSize];
                ComparerUserItem(newItems, oldItems, ref chgList, ref delItem);
                if (delItem.Length > 0)
                {
                    for (int i = 0; i < delItem.Length; i++)
                    {
                        if (delItem[i] == null)
                        {
                            continue;
                        }
                        MySqlConnector.MySqlCommand command = context.CreateCommand();
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
                        LogService.Error("[Exception] PlayDataStorage.UpdateBagItem (Delete Item)");
                        LogService.Error(ex.StackTrace);
                    }
                }
                if (chgList.Length > 0)
                {
                    for (int i = 0; i < chgList.Length; i++)
                    {
                        if (chgList[i] == null)
                        {
                            continue;
                        }
                        MySqlConnector.MySqlCommand command = context.CreateCommand();
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
                        LogService.Error("[Exception] PlayDataStorage.UpdateBagItem (Update Item)");
                        LogService.Error(ex.StackTrace);
                    }
                }
            }
            catch
            {
                LogService.Error("[Exception] PlayDataStorage.UpdateBagItem");
            }
        }

        private const string ClearStorageItemSql = "UPDATE characters_storageitem SET Position = @Position, MakeIndex = 0, StdIndex = 0, Dura = 0, DuraMax = 0 WHERE PlayerId = @PlayerId  AND Position = @Position AND MakeIndex = @MakeIndex AND StdIndex = @StdIndex;";
        private const string UpdateStorageItemSql = "UPDATE characters_storageitem SET Position = @Position, MakeIndex =@MakeIndex, StdIndex = @StdIndex, Dura = @Dura, DuraMax = @DuraMax WHERE PlayerId = @PlayerId AND Position = @Position;";

        private void SaveStorageItem(StorageContext context, int playerId, ServerUserItem[] storageItems)
        {
            try
            {
                int storageSize = storageItems.Length;
                CharacterDataInfo playData = new CharacterDataInfo();
                GetStorageRecord(playerId, context, ref playData);
                ServerUserItem[] oldItems = playData.Data.StorageItems;
                ServerUserItem[] newItems = storageItems;
                ServerUserItem[] delItem = new ServerUserItem[storageSize];
                ServerUserItem[] chgList = new ServerUserItem[storageSize];
                ComparerUserItem(newItems, oldItems, ref chgList, ref delItem);
                if (delItem.Length > 0)
                {
                    for (int i = 0; i < delItem.Length; i++)
                    {
                        if (delItem[i] == null)
                        {
                            continue;
                        }
                        MySqlConnector.MySqlCommand command = context.CreateCommand();
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
                        LogService.Error("[Exception] PlayDataStorage.SaveStorageItem (Delete Item)");
                        LogService.Error(ex.StackTrace);
                    }
                }

                if (chgList.Length > 0)
                {
                    for (int i = 0; i < chgList.Length; i++)
                    {
                        if (chgList[i] == null)
                        {
                            continue;
                        }
                        MySqlConnector.MySqlCommand command = context.CreateCommand();
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
                        LogService.Error("[Exception] PlayDataStorage.SaveStorageItem (Update Item)");
                        LogService.Error(ex.StackTrace);
                    }
                }
            }
            catch
            {
                LogService.Error("[Exception] PlayDataStorage.SaveStorageItem");
            }
        }

        private void SaveMagics(StorageContext context, int playerId, MagicRcd[] humanRcd)
        {
            MySqlConnector.MySqlCommand delcommand = context.CreateCommand();
            delcommand.CommandText = "DELETE FROM characters_magic WHERE PlayerId=@PlayerId";
            delcommand.Parameters.AddWithValue("@PlayerId", playerId);
            delcommand.ExecuteNonQuery();
            try
            {
                const string sStrSql = "INSERT INTO characters_magic(PlayerId,MagicId,Level,Usekey,CurrTrain) VALUES ({0},{1},{2},'{3}',{4});";
                List<string> strSqlList = new List<string>();
                for (int i = 0; i < humanRcd.Length; i++)
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
                MySqlConnector.MySqlCommand command = context.CreateCommand();
                command.CommandText = string.Join("\r\n", strSqlList);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogService.Error("[Exception] PlayDataStorage.SaveMagics");
                LogService.Error(ex.StackTrace);
            }
        }

        private void SaveBonusability(StorageContext context, int playerId, NakedAbility bonusAbil)
        {
            const string sSqlStr = "UPDATE characters_bonusability SET AC=@AC, MAC=@MAC, DC=@DC, MC=@MC, SC=@SC, HP=@HP, MP=@MP, HIT=@HIT, SPEED=@SPEED, RESERVED=@RESERVED WHERE PlayerId=@PlayerId";
            try
            {
                MySqlConnector.MySqlCommand command = context.CreateCommand();
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
                LogService.Error("[Exception] PlayDataStorage.SaveBonusability");
                LogService.Error(ex.StackTrace);
            }
        }

        private void SaveQuest(StorageContext context, int id, CharacterDataInfo humanRcd)
        {
            const string sSqlStr4 = "DELETE FROM characters_quest WHERE PlayerId=@PlayerId";
            try
            {
                MySqlConnector.MySqlCommand command = context.CreateCommand();
                command.CommandText = sSqlStr4;
                command.Parameters.AddWithValue("@PlayerId", id);
                command.ExecuteNonQuery();
            }
            catch
            {
                LogService.Error("[Exception] PlayDataStorage.SaveQuest");
            }
        }

        private void SaveStatus(StorageContext context, int playerId, ushort[] statusTimeArr)
        {
            try
            {
                const string updatrStatusSql = "UPDATE characters_status SET Status0=@Status0,Status1=@Status1,Status2=@Status2,Status3=@Status3,Status4=@Status4,Status5=@Status5,Status6=@Status6,Status7=@Status7,Status8=@Status8,Status9=@Status9,Status10=@Status10,Status11=@Status11,Status12=@Status12,Status13=@Status13,Status14=@Status14 WHERE PlayerId=@PlayerId;";
                MySqlConnector.MySqlCommand command = context.CreateCommand();
                command.CommandText = updatrStatusSql;
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
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogService.Error("[Exception] PlayDataStorage.UpdateStatus (Update characters_status)");
                LogService.Error(ex.StackTrace);
            }
        }

        private void UpdateItemAttr(StorageContext context, int playerId, ServerUserItem[] userItems)
        {
            try
            {
                const string updateItemAttrSql = "UPDATE characters_item_attr SET VALUE0={0},VALUE1={1},VALUE2={2},VALUE3={3},VALUE4={4},VALUE5={5},VALUE6={6},VALUE7={7},VALUE8={8},VALUE9={9},VALUE10={10},VALUE11={11},VALUE12={12},VALUE13={13} WHERE PlayerId={14} AND MakeIndex={15};";
                List<string> strSqlList = new List<string>();
                for (int i = 0; i < userItems.Length; i++)
                {
                    if (userItems[i] == null)
                    {
                        continue;
                    }
                    ServerUserItem userItem = userItems[i];
                    strSqlList.Add(string.Format(updateItemAttrSql, userItem.Desc[0], userItem.Desc[1],
                        userItem.Desc[2], userItem.Desc[3], userItem.Desc[4], userItem.Desc[5], userItem.Desc[6], userItem.Desc[7], userItem.Desc[8], userItem.Desc[9],
                        userItem.Desc[10], userItem.Desc[11], userItem.Desc[12], userItem.Desc[13], playerId, userItem.MakeIndex));
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
                LogService.Error("[Exception] PlayDataStorage.UpdateItemAttr (Update item attr)");
                LogService.Error(e.StackTrace);
            }
        }

        private bool UpdateChrRecord(int playerId, QueryChr queryChrRcd)
        {
            const string sStrString = "UPDATE characters SET Sex=@Sex, Job=@Job WHERE ID=@Id";
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
                try
                {
                    MySqlConnector.MySqlCommand command = context.CreateCommand();
                    command.CommandText = sStrString;
                    command.Parameters.AddWithValue("@Sex", queryChrRcd.Sex);
                    command.Parameters.AddWithValue("@Job", queryChrRcd.Job);
                    command.Parameters.AddWithValue("@Id", playerId);
                    command.ExecuteNonQuery();
                    result = true;
                }
                catch
                {
                    LogService.Error("[Exception] UpdateChrRecord");
                    result = false;
                }
            }
            finally
            {
                context.Dispose();
            }
            return result;
        }

        private void DeleteItemAttr(StorageContext context, int playerId, IEnumerable<int> makeIndex)
        {
            try
            {
                MySqlConnector.MySqlCommand command = context.CreateCommand();
                command.CommandText = "DELETE FROM characters_item_attr WHERE PlayerId=@PlayerId AND MakeIndex in (@MakeIndex)";
                command.Parameters.AddWithValue("@PlayerId", playerId);
                command.Parameters.AddWithValue("@MakeIndex", string.Join(",", makeIndex));
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                LogService.Error("[Exception] PlayDataStorage.UpdateRecord (Delete item attr)");
                LogService.Error(e.StackTrace);
            }
        }

        private void ClearItemAttr(StorageContext context, int playerId, IList<int> makeIndex)
        {
            if (!makeIndex.Any())
            {
                return;
            }
            const string clearItemAttrSql = "UPDATE characters_item_attr SET MakeIndex=0,VALUE0=0,VALUE1=0,VALUE2=0,VALUE3=0,VALUE4=0,VALUE5=0,VALUE6=0,VALUE7=0,VALUE8=0,VALUE9=0,VALUE10=0,VALUE11=0,VALUE12=0,VALUE13=0 WHERE PlayerId={0} AND MakeIndex ={1};";
            try
            {
                List<string> strSqlList = new List<string>();
                for (int i = 0; i < makeIndex.Count(); i++)
                {
                    strSqlList.Add(string.Format(clearItemAttrSql, playerId, makeIndex[i]));
                }
                MySqlConnector.MySqlCommand command = context.CreateCommand();
                command.CommandText = string.Join("\r\n", strSqlList);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogService.Error("[Exception] PlayDataStorage.ClearItemAttr");
                LogService.Error(ex.StackTrace);
            }
        }

    }
}