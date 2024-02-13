using MemoryPack;

namespace OpenMir2.Packets.ClientPackets
{
    [MemoryPackable]
    public partial class ClientConf
    {
        public bool boGameAssist { get; set; }
        public bool boWhisperRecord { get; set; }
        public bool boMaketSystem { get; set; }
        public bool boNoFog { get; set; }
        public bool boStallSystem { get; set; }
        public bool boShowHpBar { get; set; }
        public bool boShowHpNumber { get; set; }
        public bool boNoStruck { get; set; }
        public bool boFastMove { get; set; }
        public bool boNoWeight { get; set; }
        public bool boShowFriend { get; set; }
        public bool boShowRelationship { get; set; }
        public bool boShowMail { get; set; }
        public bool boShowRecharging { get; set; }
        public bool boShowHelp { get; set; }
        public bool boShowGameShop { get; set; }
        public bool boGamepath { get; set; }
    }
}