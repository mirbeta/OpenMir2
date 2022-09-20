﻿using System;
using SystemModule.Packet.ClientPackets;

namespace SystemModule.Data
{
    /// <summary>
    /// 元宝寄售数据结构
    /// </summary>
    public class DealOffInfo
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
        public UserItem[] UseItems;
        public byte Flag;
    }
}
