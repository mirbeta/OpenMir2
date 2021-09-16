using System.IO;

namespace SystemModule
{
    public class TLoadHuman
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

        public TLoadHuman(byte[] buffer)
        {

        }
    }
}

