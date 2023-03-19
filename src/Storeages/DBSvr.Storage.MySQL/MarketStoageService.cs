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

        int IMarketStorage.QueryMarketItemsCount(byte serverGroupId)
        {
            throw new System.NotImplementedException();
        }

        bool IMarketStorage.SaveMarketItem(MarketItem item, byte serverGroupId, byte serverIndex)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<MarketItem> SearchMarketItems(byte serverGroupId, string sellWho, string itemName, short itemType, byte itemSet)
        {
            var SearchStr = string.Empty;
            if (!string.IsNullOrEmpty(itemName)) SearchStr = "EXEC UM_LOAD_ITEMNAME ''' +marketname+''','''+itemname+'''";
            else if (!string.IsNullOrEmpty(sellWho)) SearchStr = "EXEC UM_LOAD_USERNAME ''' +marketname+''','''+sellwho+'''";
            else if (itemSet != 0) SearchStr = "EXEC UM_LOAD_ITEMSET ''' +marketname+''','+intToStr(itemset)";
            else if (itemType >= 0) SearchStr = "EXEC UM_LOAD_ITEMTYPE '''+marketname+''','+intToStr(itemtype)";
            throw new System.NotImplementedException();
        }
    }
}