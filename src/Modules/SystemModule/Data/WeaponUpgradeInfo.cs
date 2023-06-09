using SystemModule.Packets.ClientPackets;

namespace SystemModule.Data
{
    /// <summary>
    /// 沙巴克武器升级信息
    /// </summary>
    public record struct WeaponUpgradeInfo
    {
        public string UserName;
        public UserItem UserItem;
        public byte Dc;
        public byte Sc;
        public byte Mc;
        public byte Dura;
        public DateTime UpgradeTime;
        public int GetBackTick;
    }
}
