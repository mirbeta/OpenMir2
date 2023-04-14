using System.IO;
using SystemModule.Extensions;

namespace SystemModule.Packets.ClientPackets
{
    /// <summary>
    /// 客户端元宝寄售数据结构
    /// </summary>
    public class ClientDealOffInfo : ClientPacket
    {
        /// <summary>
        /// 寄售人
        /// </summary>        
        public string DealChrName;
        /// <summary>
        /// 购买人
        /// </summary>        
        public string BuyChrName;
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
        public ClientItem[] UseItems;
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
            writer.Write(DealChrName.ToByte(15));
            writer.Write(BuyChrName.ToByte(15));
            writer.Write(SellDateTime);
            writer.Write(SellGold);
            ClientItem nullItem = new ClientItem();
            byte[] nullBuff = nullItem.GetBuffer();
            for (int i = 0; i < UseItems.Length; i++)
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