using System.IO;
using OpenMir2.Data;

namespace OpenMir2.Packets.ClientPackets
{
    public class UserMagic : ClientPacket
    {
        public MagicInfo Magic;
        public ushort MagIdx;
        public byte Level;
        public char Key;
        /// <summary>
        /// 技能熟练点
        /// </summary>
        public int TranPoint;

        public UserMagic()
        {
            Magic = new MagicInfo();
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Magic.GetBuffer());
            writer.Write(MagIdx);
            writer.Write(Level);
            writer.Write(Key);
            writer.Write(TranPoint);
        }
    }
}