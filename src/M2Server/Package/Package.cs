using System;
using System.IO;
using System.Text;

namespace M2Server
{
    /// <summary>
    /// Reads primitive data types from an array of binary data.
    /// </summary>
    public abstract class Package 
    {
        public readonly BinaryReader binaryReader;

        public Package() { }

        public Package(byte[] segment)
        {
            if (segment == null)
            {
                throw new Exception("segment is null");
            }
            binaryReader = new BinaryReader(new MemoryStream(segment));
        }

        public string ReadPascalString(int size)
        {
            var packegeLen = binaryReader.ReadByte();
            var strbuff = binaryReader.ReadBytes(size);
            return Encoding.GetEncoding("gb2312").GetString(strbuff, 0, packegeLen);
        }

        public int ReadInt32()
        {
            return binaryReader.ReadInt32();
        }

        public uint ReadUInt32()
        {
            return binaryReader.ReadUInt32();
        }

        public short ReadInt16()
        {
            return binaryReader.ReadInt16();
        }

        public ushort ReadUInt16()
        {
            return binaryReader.ReadUInt16();
        }

        public double ReadDouble()
        {
            return binaryReader.ReadDouble();
        }

        public byte ReadByte()
        {
            return binaryReader.ReadByte();
        }

        public bool ReadBoolean()
        {
            return binaryReader.ReadBoolean();
        }

        public byte[] ReadBytes(int size)
        {
            return binaryReader.ReadBytes(size);
        }

        public ushort[] ReadUInt16(int size)
        {
            var shortarr = new ushort[size];
            for (var i = 0; i < shortarr.Length; i++)
            {
                shortarr[i] = binaryReader.ReadUInt16();
            }
            return shortarr;
        }
    } 
}

