using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemModule.Data;

namespace DBSrv.Storage
{
    /// <summary>
    /// 拍卖行数据存储接口
    /// </summary>
    public interface IMarketStorage
    {
        IEnumerable<MarketItem> QueryMarketItems(byte serverGroupId);
    }
}
