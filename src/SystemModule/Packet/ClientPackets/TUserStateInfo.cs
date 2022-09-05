using System;
using System.IO;
using SystemModule.Extensions;

namespace SystemModule.Packet.ClientPackets
{
    public class TUserStateInfo : Packets
    {
        public int Feature;
        public string UserName;
        public ushort NameColor;
        public string GuildName;
        public string GuildRankName;
        public byte btGender;
        public byte btHumAttr;
        public byte btResver1;
        public byte btResver2;
        public TClientItem[] UseItems;
        public byte ActiveTitle;
        
        public TUserStateInfo()
        {
            UseItems = new TClientItem[13];
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Feature);
            writer.Write(UserName, PacketConst.UserNameLen);
            writer.Write(NameColor);
            writer.Write(GuildName, PacketConst.GuildNameLen);
            writer.Write(GuildRankName, PacketConst.UserNameLen);
            for (var i = 0; i < UseItems.Length; i++)
            {
                writer.Write(UseItems[i].GetBuffer());
            }
        }
    }
}