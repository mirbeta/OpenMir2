using System;
using System.IO;
using SystemModule;

namespace GameSvr
{
    public class TUserStateInfo : Package
    {
        public int Feature;
        public string UserName;
        public string GuildName;
        public string GuildRankName;
        public ushort NameColor;
        public TClientItem[] UseItems;

        public TUserStateInfo()
        {
            UseItems = new TClientItem[13];
        }

        public byte[] ToByte()
        {
            using var memoryStream = new MemoryStream();
            var backingStream = new BinaryWriter(memoryStream);

            backingStream.Write(Feature);

            var StrLen = 0;
            var NameBuff = HUtil32.StringToByteAry(UserName, out StrLen);
            NameBuff[0] = (byte)StrLen;
            Array.Resize(ref NameBuff, 20);
            backingStream.Write(NameBuff, 0, NameBuff.Length);

            backingStream.Write(NameColor);

            NameBuff = HUtil32.StringToByteAry(GuildName, out StrLen);
            NameBuff[0] = (byte)StrLen;
            Array.Resize(ref NameBuff, 15);
            backingStream.Write(NameBuff, 0, NameBuff.Length);

            NameBuff = HUtil32.StringToByteAry(GuildRankName, out StrLen);
            NameBuff[0] = (byte)StrLen;
            Array.Resize(ref NameBuff, 15);
            backingStream.Write(NameBuff, 0, NameBuff.Length);

            for (var i = 0; i < UseItems.Length; i++)
            {
                backingStream.Write(UseItems[i].ToByte());
            }

            var stream = backingStream.BaseStream as MemoryStream;
            return stream.ToArray();
        }
    }
}