namespace GameGate.Packet
{
    public class HardwareHeader : ClientPacket
    {
        public uint dwMagicCode;
        public byte[] xMd5Digest;

        protected override void ReadPacket(BinaryReader reader)
        {
            dwMagicCode = reader.ReadUInt32();
            xMd5Digest = reader.ReadBytes(16);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}