using System.Collections;
using System.Collections.Generic;
using SystemModule.Data;
using SystemModule.Packets.ClientPackets;

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
                marketItem.Selldate = dr.GetString("SellDate");
                marketItem.SellState = dr.GetByte("SellState");
                marketItem.Item = new ClientItem();
                marketItem.Item.MakeIndex = dr.GetInt32("MaekeIndex");
                marketItem.Item.Dura = dr.GetUInt16("Dura");
                marketItem.Item.DuraMax = dr.GetUInt16("DuraMax");
                for (int i = 0; i < 13; i++)
                {
                    marketItem.Item.Desc[i] = dr.GetByte($"Value{i}");
                }
                itemList.Add(marketItem);
            }
            dr.Close();
            dr.Dispose();
            return itemList;
        }

        public int QueryMarketItemsCount(byte serverGroupId)
        {
            return 0;
        }

        public bool SaveMarketItem(MarketItem item, byte serverGroupId, byte serverIndex)
        {
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
            const string sSql = "select * from marketItems where SellState!=20 and SellWho = @SellWho and MarketName = @MarketName";
            return new List<MarketItem>();
        }

        private IEnumerable<MarketItem> SearchMarketItemSetItems(byte groupId, string marketName, byte itemSet)
        {
            const string sSql = "select * from marketItems where SellState=1 and ItemSet = @ItemSet and MarketName = @MarketName";
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
            const string sSql = "select * from marketItems where MarketName = @MarketName and ItemName = @ItemName";
            return new List<MarketItem>();
        }
    }
}