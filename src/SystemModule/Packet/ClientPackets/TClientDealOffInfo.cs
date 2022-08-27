using System.IO;
using SystemModule.Extensions;

namespace SystemModule.Packet.ClientPackets
{
    /// <summary>
    /// 客户端元宝寄售数据结构
    /// </summary>
    public class TClientDealOffInfo : Packets
    {
        /// <summary>
        /// 寄售人
        /// </summary>        
        public string DealCharName;
        /// <summary>
        /// 购买人
        /// </summary>        
        public string BuyCharName;
        /// <summary>
        /// 寄售时间
        /// </summary>        
        public double SellDateTime;
        /// <summary>
        /// 交易的元宝数
        /// </summary>        
        public int SellGold;
        /// <summary>
        /// 物品
        /// </summary>        
        public TClientItem[] UseItems;
        /// <summary>
        /// 操作状态标识
        /// </summary>
        public byte N;

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(DealCharName.ToByte(15));
            writer.Write(BuyCharName.ToByte(15));
            writer.Write(SellDateTime);
            writer.Write(SellGold);
            var nullItem = new TClientItem();
            var nullBuff = nullItem.GetBuffer();
            for (var i = 0; i < UseItems.Length; i++)
            {
                if (UseItems[i] == null)
                {
                    writer.Write(nullBuff);
                }
                else
                {
                    writer.Write(UseItems[i].GetBuffer());
                }
            }
            writer.Write(N);
        }
    }
}