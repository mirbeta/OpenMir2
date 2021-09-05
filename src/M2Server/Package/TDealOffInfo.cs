using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace M2Server
{
    /// <summary>
    /// 元宝寄售数据结构
    /// </summary>
    public class TDealOffInfo
    {
        public string sDealCharName;
        // 寄售人
        public string sBuyCharName;
        // 购买人
        public DateTime dSellDateTime;
        // 寄售时间
        public int nSellGold;
        // 交易的元宝数
        public TUserItem[] UseItems;
        // 物品
        public byte N;
    }
}
