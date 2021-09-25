using System.IO;

namespace SystemModule
{
    public class TLoadHuman : Package
    {
        public string sAccount;
        public string sChrName;
        public string sUserAddr;
        public int nSessionID;

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);
                backingStream.Write(sAccount.ToByte(17));
                backingStream.Write(sChrName.ToByte(21));
                backingStream.Write(sUserAddr.ToByte(18));
                backingStream.Write(nSessionID);
                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }

        public TLoadHuman()
        {

        }

        public TLoadHuman(byte[] buffer) : base(buffer)
        {
            sAccount = ReadPascalString(16);
            sChrName = ReadPascalString(20);
            sUserAddr = ReadPascalString(17);
            nSessionID = ReadInt32();
        }
    }
}