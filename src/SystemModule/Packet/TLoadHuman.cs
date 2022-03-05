using System.IO;

namespace SystemModule
{
    public class TLoadHuman : Packets
    {
        public string sAccount;
        public string sChrName;
        public string sUserAddr;
        public int nSessionID;

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

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(sAccount.ToByte(17));
            writer.Write(sChrName.ToByte(21));
            writer.Write(sUserAddr.ToByte(18));
            writer.Write(nSessionID);
        }
    }
}