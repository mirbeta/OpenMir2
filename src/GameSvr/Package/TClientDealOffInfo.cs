using System;
using System.IO;
using SystemModule;

namespace GameSvr
{
    /// <summary>
    /// 客户端元宝寄售数据结构
    /// </summary>
    public class TClientDealOffInfo : Packets
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
        public double dSellDateTime;
        /// <summary>
        /// 交易的元宝数
        /// </summary>        
        public int nSellGold;
        /// <summary>
        /// 物品
        /// </summary>        
        public TClientItem[] UseItems;
        /// <summary>
        /// 操作状态标识
        /// </summary>
        public byte N;


        public byte[] GetPacket()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);
                backingStream.Write(sDealCharName.ToByte(15));
                backingStream.Write(sBuyCharName.ToByte(15));
                backingStream.Write(dSellDateTime);
                backingStream.Write(nSellGold);
                var userItem = new TClientItem();
                for (int i = 0; i < UseItems.Length; i++)
                {
                    if (UseItems[i] == null)
                    {
                        backingStream.Write(userItem.GetPacket());
                    }
                    else
                    {
                        backingStream.Write(UseItems[i].GetPacket());
                    }
                }
                backingStream.Write(N);

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }
}