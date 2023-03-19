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

        IEnumerable<MarketItem> IMarketStorage.QueryMarketItems(byte serverGroupId)
        {
            throw new System.NotImplementedException();
        }

        int IMarketStorage.QueryMarketItemsCount(byte serverGroupId)
        {
            throw new System.NotImplementedException();
        }

        bool IMarketStorage.SaveMarketItem(MarketItem item, byte serverGroupId, byte serverIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}