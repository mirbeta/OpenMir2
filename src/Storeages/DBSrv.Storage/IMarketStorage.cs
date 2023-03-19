using System.Collections.Generic;
using SystemModule.Data;

namespace DBSrv.Storage
{
    /// <summary>
    /// 拍卖行数据存储接口
    /// </summary>
    public interface IMarketStorage
    {
        IEnumerable<MarketItem> QueryMarketItems(byte serverGroupId);

        int QueryMarketItemsCount(byte serverGroupId);

        bool SaveMarketItem(MarketItem item, byte serverGroupId, byte serverIndex);
    }
}