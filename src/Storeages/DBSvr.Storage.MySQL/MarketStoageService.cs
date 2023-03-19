using System.Collections;
using System.Collections.Generic;
using SystemModule.Data;

namespace DBSrv.Storage.MySQL
{
    public class MarketStoageService : IMarketStorage
    {
        public MarketStoageService(StorageOption storageOption)
        {

        }

        public IEnumerable<MarketItem> QueryMarketItems(byte serverGroupId)
        {
            return new List<MarketItem>();
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