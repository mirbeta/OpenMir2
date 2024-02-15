namespace GameGate.Packet
{
    public class HardwareHeader : ClientPacket
    {
        public uint MagicCode;
        public byte[] Md5Digest;

        protected override void ReadPacket(BinaryReader reader)
        {
            MagicCode = reader.ReadUInt32();
            Md5Digest = reader.ReadBytes(16);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}