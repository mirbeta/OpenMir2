using SystemModule;

namespace GameSvr
{
    public class TOUserStateInfo : Packets
    {
        public int Feature;
        public string UserName;
        public string GuildName;
        public string GuildRankName;
        public short NameColor;
        public TOClientItem[] UseItems;
        
        protected override void ReadPacket(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}