using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace M2Server
{
    public class TClientDealOffInfo
    {
        // 客户端元宝寄售数据结构  
        public string sDealCharName;
        // 寄售人
        public string sBuyCharName;
        // 购买人
        public DateTime dSellDateTime;
        // 寄售时间
        public int nSellGold;
        // 交易的元宝数
        public TClientItem[] UseItems;
        // 物品
        public byte N;
    } 
}
