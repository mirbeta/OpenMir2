using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace M2Server
{
    public class TOSObject
    {
        public byte btType;
        public object CellObj;
        public double dwAddTime;
        public bool boObjectDisPose;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct TMapHeader
    {
        public short wWidth;
        public short wHeight;
    }

    public struct TMapUnitInfo
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

    public class TMapCellinfo
    {
        public byte chFlag;
        public IList<TOSObject> ObjList;
    }

    public class PointInfo
    {
        public short nX;
        public short nY;

        public PointInfo(short x,short y)
        {
            nX = x;
            nY = y;
        }
    }

    public class TRect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public TRect(int left,int top,int right,int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }
}