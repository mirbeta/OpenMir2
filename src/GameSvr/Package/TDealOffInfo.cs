using System;

namespace GameSvr
{
    /// <summary>
    /// 元宝寄售数据结构
    /// </summary>
    public class TDealOffInfo
    {
        /// <summary>
        /// 寄售人
        /// </summary>
        public string sDealCharName;
        /// <summary>
        /// 购买人
        /// </summary>        
        public string sBuyCharName;
        /// <summary>
        /// 寄售时间
        /// </summary>        
        public DateTime dSellDateTime;
        /// <summary>
        /// 交易的元宝数
        /// </summary>        
        public int nSellGold;
        /// <summary>
        /// 物品
        /// </summary>        
        public TUserItem[] UseItems;
        public byte N;
    }
}
