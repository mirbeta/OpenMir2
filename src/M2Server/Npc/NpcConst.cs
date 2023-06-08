using SystemModule.Packets.ClientPackets;

namespace M2Server.Npc {
    /// <summary>
    /// 沙巴克武器升级信息
    /// </summary>
    public record struct WeaponUpgradeInfo {
        public string UserName;
        public UserItem UserItem;
        public byte Dc;
        public byte Sc;
        public byte Mc;
        public byte Dura;
        public DateTime UpgradeTime;
        public int GetBackTick;
    }

    public struct ItemPrice {
        public ushort wIndex;
        public double nPrice;
    }

    public struct Goods {
        public string ItemName;
        public int Count;
        public int RefillTime;
        public int RefillTick;
    }


}