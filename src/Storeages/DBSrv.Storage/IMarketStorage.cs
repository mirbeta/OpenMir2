using System;
using System.Collections.Generic;
using SystemModule.Data;

namespace DBSrv.Storage
{
    /// <summary>
    /// 拍卖行数据存储接口
    /// </summary>
    public interface IMarketStorage
    {
        IEnumerable<MarketItem> QueryMarketItems(byte groupId);

        int QueryMarketItemsCount(byte groupId, string sellWho);

        bool SaveMarketItem(MarketItem item, byte groupId, byte serverIndex);

        IEnumerable<MarketItem> SearchMarketItems(byte groupId,string marketName, string sellWho, string itemName, short itemType, byte itemSet);
        
        
    }
}