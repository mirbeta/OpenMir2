using System;
using System.IO;
using SystemModule.Packets.ClientPackets;

namespace CloudGate.Packet
{
    public class HardwareHeader : ClientPackage
    {
        public uint dwMagicCode;
        public byte[] xMd5Digest;

        public HardwareHeader(byte[] buffer) : base(buffer)
        {

        }

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