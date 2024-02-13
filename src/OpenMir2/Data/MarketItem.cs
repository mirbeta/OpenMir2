using System.IO;
using OpenMir2.Extensions;
using OpenMir2.Packets.ClientPackets;

namespace OpenMir2.Data
{
    public class MarketItem : ClientPacket
    {
        /// <summary>
        /// 出售物品
        /// </summary>
        public ClientItem SellItem;
        /// <summary>
        /// 极品次数
        /// </summary>
        public int UpgCount;
        /// <summary>
        /// 索引
        /// </summary>
        public int Index;
        /// <summary>
        /// 出售价格
        /// </summary>
        public int SellPrice;
        /// <summary>
        /// 出售人
        /// </summary>
        public string SellWho;
        /// <summary>
        /// 出售时间（0312311210 = 2003-12-31 12:10）
        /// </summary>
        public string SellDate;
        /// <summary>
        /// 出售状态(1:销售中 2:已售出)
        /// </summary>
        public byte SellState;

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(SellItem.GetBuffer());
            writer.Write(UpgCount);
            writer.Write(Index);
            writer.Write(SellPrice);
            writer.WriteAsciiString(SellWho, 20);
            writer.WriteAsciiString(SellDate, 10);
            writer.Write(SellState);
        }
    }
}