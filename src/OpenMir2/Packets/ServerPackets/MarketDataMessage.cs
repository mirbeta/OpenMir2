using System.Collections.Generic;
using MemoryPack;
using OpenMir2.Data;

namespace OpenMir2.Packets.ServerPackets
{
    [MemoryPackable]
    public partial struct MarketDataMessage
    {
        public IList<MarketItem> List { get; set; }
        public int TotalCount { get; set; }
    }

    [MemoryPackable]
    public partial struct MarkerUserLoadMessage
    {
        public int SellCount { get; set; }
        public int MarketNPC { get; set; }
        public byte IsBusy { get; set; }
    }

    [MemoryPackable]
    public partial class MarketSaveDataItem
    {
        [MemoryPackAllowSerialize]
        public MarketItem Item { get; set; }
        public byte ServerIndex { get; set; }
        public string ServerName { get; set; }
        public byte GroupId { get; set; }
    }

    [MemoryPackable]
    public partial struct MarketRegisterMessage
    {
        public byte ServerIndex { get; set; }
        public string ServerName { get; set; }
        public byte GroupId { get; set; }
        public string Token { get; set; }
    }

    [MemoryPackable]
    public partial struct MarketSearchMessage
    {
        public byte ServerIndex { get; set; }
        public byte GroupId { get; set; }
        public string UserName { get; set; }
        public string MarketName { get; set; }
        public string SearchWho { get; set; }
        public string SearchItem { get; set; }
        public short ItemType { get; set; }
        public byte ItemSet { get; set; }
        public int UserMode { get; set; }
        public int MarketNPC { get; set; }
    }
}