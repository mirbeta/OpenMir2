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
            throw new System.NotImplementedException();
        }
    }
}