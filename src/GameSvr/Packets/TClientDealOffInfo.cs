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

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(sDealCharName.ToByte(15));
            writer.Write(sBuyCharName.ToByte(15));
            writer.Write(dSellDateTime);
            writer.Write(nSellGold);
            var nullItem = new TClientItem();
            var nullBuff = nullItem.GetBuffer();
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