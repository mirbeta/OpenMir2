using MemoryPack;
using System.Collections.Generic;
using SystemModule.Data;

namespace SystemModule.Packets.ServerPackets
{
    [MemoryPackable]
    public partial struct MarketDataMessage
    {
        public IList<MarketItem> List;
        public int TotalCount;
    }

    [MemoryPackable]
    public partial class MarketDataItem
    {
        [MemoryPackAllowSerialize]
        public MarketItem Item { get; set; }
    }
    
    [MemoryPackable]
    public partial struct MarketRegisterMessage
    {
        public byte ServerIndex;
        public string ServerName;
        public byte GroupId;
    }
}