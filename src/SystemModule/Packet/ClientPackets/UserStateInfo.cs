using System;
using System.IO;
using SystemModule.Extensions;

namespace SystemModule.Packet.ClientPackets
{
    public class UserStateInfo : Packets
    {
        public int Feature;
        public string UserName;
        public ushort NameColor;
        public string GuildName;
        public string GuildRankName;
        public ClientItem[] UseItems;
        public bool ExistLover;
        public string LoverName;
        
        public UserStateInfo()
        {
            UseItems = new ClientItem[13];
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Feature);
            writer.WriteAsciiString(UserName, 14);
            writer.Write(NameColor);
            writer.WriteAsciiString(GuildName, 20);
            writer.WriteAsciiString(GuildRankName, 14);
            for (var i = 0; i < UseItems.Length; i++)
            {
                writer.Write(UseItems[i].GetBuffer());
            }
            writer.Write(ExistLover);
            writer.WriteAsciiString(GuildName, 14);
        }
    }
}