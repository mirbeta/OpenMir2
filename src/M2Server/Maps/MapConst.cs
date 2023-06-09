using SystemModule;
using SystemModule.Enums;

namespace M2Server.Maps
{
    /// <summary>
    /// 地图物品
    /// </summary>
    public class VisibleMapItem
    {
        public short nX;
        public short nY;
        public MapItem MapItem;
        public string sName;
        public ushort wLooks;
        public VisibleFlag VisibleFlag;
    }

    public record struct TRect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public TRect(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }
}