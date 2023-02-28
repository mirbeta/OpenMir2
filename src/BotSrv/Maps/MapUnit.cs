using System.IO;

namespace BotSrv.Maps
{
    public struct CellParams
    {
        public bool TerrainType;
        public bool TCellActor;
    }

    public struct PathcellSuccess
    {
        /// <summary>
        /// 离起点的距离
        /// </summary>
        public int Distance;
        public int Direction;
    }

    public struct WaveCell
    {
        public int X;
        public int Y;
        public int Cost;
        public int Direction;
    }

    public struct MapHeader
    {
        public ushort wWidth;
        public ushort wHeight;
        public char[] sTitle;
        public double UpdateDate;
        public char[] Reserved;

        public MapHeader(byte[] data)
        {
            using var stream = new MemoryStream(data, 0, data.Length);
            using var reader = new BinaryReader(stream);

            wWidth = reader.ReadUInt16();
            wHeight = reader.ReadUInt16();
            sTitle = reader.ReadChars(16);
            UpdateDate = reader.ReadDouble();
            Reserved = reader.ReadChars(24);
        }

        public const int PacketSize = 52;
    }

    public struct TMapInfo_Old
    {
        public short wBkImg;
        public short wMidImg;
        public short wFrImg;
        public byte btDoorIndex;
        public byte btDoorOffset;
        public byte btAniFrame;
        public byte btAniTick;
        public byte btArea;
        public byte btLight;

        public const int PacketSize = 12;
    }

    public struct TMapInfo_2
    {
        public short wBkImg;
        public short wMidImg;
        public short wFrImg;
        public byte btDoorIndex;
        public byte btDoorOffset;
        public byte btAniFrame;
        public byte btAniTick;
        public byte btArea;
        public byte btLight;
        public byte btBkIndex;
        public byte btSmIndex;

        public const int PacketSize = 14;
    }

    public struct MapInfo
    {
        public ushort wBkImg;
        public ushort wMidImg;
        public ushort wFrImg;
        public byte btDoorIndex;
        public byte btDoorOffset;
        public byte btAniFrame;
        public byte btAniTick;
        public byte btArea;
        public byte btLight;
        public byte btBkIndex;
        public byte btSmIndex;
        public ushort btTAnimImage;
        public ushort btTAnimBlank;
        public ushort btTAnimTick;
        public byte btTAnimIndex;
        public byte btTAniFrame;
        public ushort btTAniOffset;
        public byte btArea2;
        public byte btLight2;
        public byte btTiles2;
        public byte btSmTiles2;
        public byte[] btUnknown;

        public const int PacketSize = 36;

        public MapInfo(byte[] data)
        {
            using var stream = new MemoryStream(data, 0, data.Length);
            using var reader = new BinaryReader(stream);

            wBkImg = reader.ReadUInt16();
            wMidImg = reader.ReadUInt16();
            wFrImg = reader.ReadUInt16();
            btDoorIndex = reader.ReadByte();
            btDoorOffset = reader.ReadByte();
            btAniFrame = reader.ReadByte();
            btAniTick = reader.ReadByte();
            btArea = reader.ReadByte();
            btLight = reader.ReadByte();
            if (data.Length > TMapInfo_2.PacketSize)
            {
                btBkIndex = reader.ReadByte();
                btSmIndex = reader.ReadByte();
                btTAnimImage = reader.ReadUInt16();
                btTAnimBlank = reader.ReadUInt16();
                btTAnimTick = reader.ReadUInt16();
                btTAnimIndex = reader.ReadByte();
                btTAniFrame = reader.ReadByte();
                btTAniOffset = reader.ReadUInt16();
                btArea2 = reader.ReadByte();
                btLight2 = reader.ReadByte();
                btTiles2 = reader.ReadByte();
                btSmTiles2 = reader.ReadByte();
                btUnknown = reader.ReadBytes(8);
            }
            else
            {
                btBkIndex = 0;
                btSmIndex = 0;
                btTAnimImage = 0;
                btTAnimBlank = 0;
                btTAnimTick = 0;
                btTAnimIndex = 0;
                btTAniFrame = 0;
                btTAniOffset = 0;
                btArea2 = 0;
                btLight2 = 0;
                btTiles2 = 0;
                btSmTiles2 = 0;
                btUnknown = null;
            }
        }
    }

    public class TWave
    {
        public WaveCell item => GetItem();
        public int MinCost => FMinCost;
        private WaveCell[] FData;
        private int FPos = 0;
        private int FCount = 0;
        private int FMinCost = 0;

        public TWave()
        {
            Clear();
        }

        private WaveCell GetItem()
        {
            return FData[FPos];
        }

        public void Add(int NewX, int NewY, int NewCost, int NewDirection)
        {
            if (FCount >= FData.Length)
            {
                FData = new WaveCell[FData.Length + 0x400];
            }
            FData[FCount].X = NewX;
            FData[FCount].Y = NewY;
            FData[FCount].Cost = NewCost;
            FData[FCount].Direction = NewDirection;
            FCount++;
            if (NewCost < FMinCost)
            {
                FMinCost = NewCost;
            }
        }

        public void Clear()
        {
            FPos = 0;
            FCount = 0;
            FMinCost = int.MaxValue;
        }

        public bool start()
        {
            FPos = 0;
            return FCount > 0;
        }

        public bool Next()
        {
            var result = FPos < (FCount - 1);
            if (result)
            {
                FPos++;
            }
            return result;
        }
    }
}

