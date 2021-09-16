using System.IO;
using SystemModule;

namespace SystemModule
{
    public class TRecordHeader : Package
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

        public byte[] ToByte()
        {
            using var memoryStream = new MemoryStream();
            var backingStream = new BinaryWriter(memoryStream);

            backingStream.Write(sAccount.ToByte(17));
            backingStream.Write(sName.ToByte(21));
            backingStream.Write(nSelectID);
            backingStream.Write(dCreateDate);
            backingStream.Write(boDeleted);
            backingStream.Write(UpdateDate);
            backingStream.Write(CreateDate);

            var stream = backingStream.BaseStream as MemoryStream;
            return stream.ToArray();
        }
    }
}

