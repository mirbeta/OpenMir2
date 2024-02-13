using System;
using System.Collections.Generic;
using System.Text;
using OpenMir2;
using OpenMir2.Data;
using OpenMir2.Packets.ClientPackets;

namespace DBSrv.Storage.MySQL
{
    public class MarketStoageService : IMarketStorage
    {
        private readonly StorageOption _storageOption;

        public MarketStoageService(StorageOption storageOption)
        {
            _storageOption = storageOption;
        }

        public IEnumerable<MarketItem> QueryMarketItems(byte groupId)
        {
            using var context = new StorageContext(_storageOption);
            var success = false;
            context.Open(ref success);
            if (!success)
            {
                return new List<MarketItem>();
            }
            const string sSqlStringAll = "select a.*,b.* from marketItems a left JOIN marketitem b on a.Id=b.MarketId where a.SellState=1";
            const string sSqlString = "select a.*,b.* from marketItems a left JOIN marketitem b on a.Id=b.MarketId where a.SellState=1 and GroupId=@GroupId";
            var command = context.CreateCommand();
            command.CommandText = groupId == 0 ? sSqlStringAll : sSqlString;
            command.Parameters.AddWithValue("@GroupId", groupId);
            using var dr = command.ExecuteReader();
            var itemList = new List<MarketItem>();
            if (dr.Read())
            {
                var marketItem = new MarketItem();
                marketItem.Index = dr.GetInt32("SellIndex");
                marketItem.SellWho = dr.GetString("SellName");
                marketItem.SellPrice = dr.GetInt32("SellPrice");
                marketItem.SellDate = dr.GetString("SellDate");
                marketItem.SellState = dr.GetByte("SellState");
                marketItem.SellItem = new ClientItem();
                marketItem.SellItem.MakeIndex = dr.GetInt32("MaekeIndex");
                marketItem.SellItem.Dura = dr.GetUInt16("Dura");
                marketItem.SellItem.DuraMax = dr.GetUInt16("DuraMax");
                for (int i = 0; i < 13; i++)
                {
                    marketItem.SellItem.Desc[i] = dr.GetByte($"Value{i}");
                }
                itemList.Add(marketItem);
            }
            dr.Close();
            dr.Dispose();
            return itemList;
        }

        public int QueryMarketItemsCount(byte groupId, string sellWho)
        {
            return 0;
        }

        public bool SaveMarketItem(MarketItem market, byte groupId, byte serverIndex)
        {
            var sb = new StringBuilder();
            sb.AppendLine("INSERT INTO marketitems (GroupId, ServerIndex, SellIndex, SellState, MarketName, ItemType, ItemSet, ItemName, MakeIndex, SellName, SellPrice, SellDate) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("(@GroupId, @ServerIndex, @SellIndex, @SellState, @MarketName, @ItemType, @ItemSet, @ItemName, @MakeIndex, @SellName, @SellPrice, @SellDate)");
            using var context = new StorageContext(_storageOption);
            var success = false;
            context.Open(ref success);
            if (!success)
            {
                return false;
            }
            var command = context.CreateCommand();
            command.CommandText = sb.ToString();
            command.Parameters.AddWithValue("@GroupId", groupId);
            command.Parameters.AddWithValue("@ServerIndex", serverIndex);
            command.Parameters.AddWithValue("@MakeIndex", market.SellItem.MakeIndex);
            command.Parameters.AddWithValue("@SellIndex", market.Index);
            command.Parameters.AddWithValue("@MarketName", "Market");
            command.Parameters.AddWithValue("@ItemName", market.SellItem.Item.Name);
            command.Parameters.AddWithValue("@SellState", market.SellState);
            command.Parameters.AddWithValue("@ItemType", market.SellItem.Item.ItemType);
            command.Parameters.AddWithValue("@ItemSet", market.SellItem.Item.ItemSet);
            command.Parameters.AddWithValue("@SellName", market.SellWho);
            command.Parameters.AddWithValue("@SellPrice", market.SellPrice);
            command.Parameters.AddWithValue("@SellDate", market.SellDate);
            try
            {
                command.ExecuteNonQuery();
                var s = (int)command.LastInsertedId;
            }
            catch (Exception ex)
            {
                context.RollBack();
                LogService.Error("[Exception] PlayDataStorage.CreateCharacters");
                LogService.Error(ex.StackTrace);
                return false;
            }
            return true;
        }

        public IEnumerable<MarketItem> SearchMarketItems(byte groupId, string marketName, string sellWho, string itemName, short itemType, byte itemSet)
        {
            if (!string.IsNullOrEmpty(itemName)) return SearchMarketNameItems(groupId, marketName, itemName);
            if (!string.IsNullOrEmpty(sellWho)) return SearchMarketItemNameItems(groupId, marketName, sellWho);
            if (itemSet != 0) return SearchMarketItemSetItems(groupId, marketName, itemSet);
            if (itemType >= 0) return SearchMarketItemTypeItems(groupId, marketName, itemType);
            return new List<MarketItem>();
        }

        private IEnumerable<MarketItem> SearchMarketItemNameItems(byte groupId, string marketName, string sellWho)
        {
            return new List<MarketItem>();
        }

        private IEnumerable<MarketItem> SearchMarketItemSetItems(byte groupId, string marketName, byte itemSet)
        {
            return new List<MarketItem>();
        }

        private IEnumerable<MarketItem> SearchMarketItemTypeItems(byte groupId, string marketName, short itemType)
        {
            var sSql = string.Empty;
            if (itemType == 0)
            {
                sSql = "select * from marketItems where SellState=1";
            }
            else
            {
                sSql = "select * from marketItems where SellState=1 and ItemType = @ItemType and MarketName = @MarketName";
            }
            return new List<MarketItem>();
        }

        private IEnumerable<MarketItem> SearchMarketNameItems(byte groupId, string marketName, string itemName)
        {
            return new List<MarketItem>();
        }
    }
}