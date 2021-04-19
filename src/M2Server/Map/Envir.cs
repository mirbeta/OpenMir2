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
        // 32768 $8000 Îª½ûÖ¹ÒÆ¶¯ÇøÓò
        public short wMidImg;
        public short wFrImg;
        public byte btDoorIndex;
        // $80 (¹®Â¦), ¹®ÀÇ ½Äº° ÀÎµ¦½º
        public byte btDoorOffset;
        // ´ÝÈù ¹®ÀÇ ±×¸²ÀÇ »ó´ë À§Ä¡, $80 (¿­¸²/´ÝÈû(±âº»))
        public byte btAniFrame;
        // $80(Draw Alpha) +  ÇÁ·¡ÀÓ ¼ö
        public byte btAniTick;
        public byte btArea;
        // Áö¿ª Á¤º¸
        public byte btLight;
    }

    public class TMapCellinfo
    {
        public byte chFlag;
        public IList<TOSObject> ObjList;
    }
}

