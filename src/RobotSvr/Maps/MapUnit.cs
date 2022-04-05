using System;

namespace RobotSvr
{
    public struct TCellParams
    {
        public bool TerrainType;
        public bool TCellActor;
    }

    public struct TPathMapCell
    {
        public int Distance;
        public int Direction;
    }

    public struct TWaveCell
    {
        public int X;
        public int Y;
        public int Cost;
        public int Direction;
    }

    public struct TMapPrjInfo
    {
        public string[] ident;
        public int ColCount;
        public int RowCount;
    }

    public struct TMapHeader
    {
        public short wWidth;
        public short wHeight;
        public string[] sTitle;
        public DateTime UpdateDate;
        public char[] Reserved;
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
    }

    public struct TMapInfo
    {
        // 新地图结构
        public short wBkImg;
        public short wMidImg;
        public short wFrImg;
        public byte btDoorIndex;
        public byte btDoorOffset;
        public byte btAniFrame;
        public byte btAniTick;
        public byte btArea;// 区域
        public byte btLight;// 光
        public byte btBkIndex;
        public byte btSmIndex;
        public short btTAnimImage;
        public short btTAnimBlank;
        public short btTAnimTick;
        public byte btTAnimIndex;
        public byte btTAniFrame;
        public short btTAniOffset;
        public byte btArea2;
        public byte btLight2;// 光
        public byte btTiles2;
        public byte btSmTiles2;
        public byte[] btUnknown;
    }

    public class TWave
    {
        public TWaveCell item
        {
            get
            {
                return GetItem();
            }
        }
        public int MinCost
        {
            get
            {
                return FMinCost;
            }
        }
        private TWaveCell[] FData;
        private int FPos = 0;
        private int FCount = 0;
        private int FMinCost = 0;

        public TWave()
        {
            Clear();
        }

        private TWaveCell GetItem()
        {
            return FData[FPos];
        }

        public void Add(int NewX, int NewY, int NewCost, int NewDirection)
        {
            if (FCount >= FData.Length)
            {
                FData = new TWaveCell[FData.Length + 0x400];
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
            FMinCost = Int32.MaxValue;
        }

        public bool start()
        {
            bool result;
            FPos = 0;
            result = (FCount > 0);
            return result;
        }

        public bool Next()
        {
            bool result;
            result = (FPos < (FCount - 1));
            if (result)
            {
                FPos++;
            }
            return result;
        }
    }
}

