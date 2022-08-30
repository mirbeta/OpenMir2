using System;
using System.IO;

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

            var StrLen = 0;
            var NameBuff = HUtil32.StringToByteAry(UserName, out StrLen);
            NameBuff[0] = (byte)StrLen;
            Array.Resize(ref NameBuff, PacketConst.UserNameLen);
            writer.Write(NameBuff, 0, NameBuff.Length);

            writer.Write(NameColor);

            NameBuff = HUtil32.StringToByteAry(GuildName, out StrLen);
            NameBuff[0] = (byte)StrLen;
            Array.Resize(ref NameBuff, PacketConst.GuildNameLen);
            writer.Write(NameBuff, 0, NameBuff.Length);

            NameBuff = HUtil32.StringToByteAry(GuildRankName, out StrLen);
            NameBuff[0] = (byte)StrLen;
            Array.Resize(ref NameBuff, PacketConst.UserNameLen);
            writer.Write(NameBuff, 0, NameBuff.Length);

            for (var i = 0; i < UseItems.Length; i++)
            {
                writer.Write(UseItems[i].GetBuffer());
            }
        }
    }
}