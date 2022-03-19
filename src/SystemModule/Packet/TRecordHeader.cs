using System.IO;

namespace SystemModule
{
    public class TRecordHeader : Packets
    {
        public string sAccount;
        public string sName;
        public int nSelectID;
        public double dCreateDate;
        public bool boDeleted;
        public double UpdateDate;
        public double CreateDate;

        public TRecordHeader() { }

        public TRecordHeader(byte[] buff)
            : base(buff)
        {
            this.sAccount = ReadPascalString(16);//BitConverter.ToString(buff, 0, 16);
            this.sName = ReadPascalString(20);//BitConverter.ToString(buff, 17, 37);
            this.nSelectID = ReadInt32();//BitConverter.ToInt32(buff, 38);
            this.dCreateDate = ReadDouble();//BitConverter.ToDouble(buff, 39);
            this.boDeleted = ReadBoolean(); //BitConverter.ToBoolean(buff, 48);
            this.UpdateDate = ReadDouble();//BitConverter.ToDouble(buff, 49);
            this.CreateDate = ReadDouble();//BitConverter.ToDouble(buff, 59);
        }


        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(sAccount.ToByte(17));
            writer.Write(sName.ToByte(21));
            writer.Write(nSelectID);
            writer.Write(dCreateDate);
            writer.Write(boDeleted);
            writer.Write(UpdateDate);
            writer.Write(CreateDate);
        }
    }
}
