using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace M2Server
{
    public class TOSObject
    {
        public byte btType;
        public object CellObj;
        public double dwAddTime;
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
        /// <summary>
        /// 为0的的时候可以表示该区域可以移动
        /// </summary>
        public byte chFlag;
        public IList<TOSObject> ObjList;
    }
}