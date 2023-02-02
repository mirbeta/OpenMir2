using System;
using System.IO;
using SystemModule.Extensions;

namespace SystemModule.Packets.ClientPackets
{
    public class UserStateInfo : ClientPacket
    {
        public int Feature;
        public string UserName;
        public int NameColor;
        public string GuildName;
        public string GuildRankName;
        public ClientItem[] UseItems;
        public bool ExistLover;
        public string LoverName;

        private static readonly byte[] nullItemBuff = new ClientItem().GetBuffer();

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
            writer.Write((byte)0);
            writer.Write(NameColor);
            writer.WriteAsciiString(GuildName, 20);
            writer.WriteAsciiString(GuildRankName, 14);
            for (var i = 0; i < UseItems.Length; i++)
            {
                if (UseItems[i] == null)
                {
                    writer.Write(nullItemBuff);
                }
                else
                {
                    writer.Write(UseItems[i].GetBuffer());
                }
            }
            writer.Write(ExistLover);
            writer.WriteAsciiString(LoverName, 14);
        }
    }
}