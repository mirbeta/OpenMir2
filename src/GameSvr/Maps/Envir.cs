using System.Collections.Generic;

namespace GameSvr
{
    /// <summary>
    /// 可见的地图物品
    /// </summary>
    public class VisibleMapItem
    {
        public int nX;
        public int nY;
        public MapItem MapItem;
        public string sName;
        public ushort wLooks;
        public int nVisibleFlag;
    }

    /// <summary>
    /// 地图上的对象
    /// </summary>
    public class CellObject
    {
        public CellType CellType;
        public object CellObj;
        public int dwAddTime;
        public bool boObjectDisPose;
    }

    public enum CellType : byte
    {
        OS_EVENTOBJECT = 1,
        OS_MOVINGOBJECT = 2,
        OS_ITEMOBJECT = 3,
        OS_GATEOBJECT = 4,
        OS_MAPEVENT = 5,
        OS_DOOR = 6,
        OS_ROON = 7
    }

    public enum CellAttribute : byte
    {
        /// <summary>
        /// 可以走动
        /// </summary>
        Walk = 0,
        HighWall = 1,
        LowWall = 2
    }

    public class MapCellinfo
    {
        public static MapCellinfo LowWall => new() { Attribute = CellAttribute.LowWall };
        public static MapCellinfo HighWall => new() { Attribute = CellAttribute.HighWall };

        public bool Valid => Attribute == CellAttribute.Walk;

        public CellAttribute Attribute;

        /// <summary>
        /// 对象数量
        /// </summary>
        public int Count => ObjList.Count;

        public IList<CellObject> ObjList;

        public void Add(CellObject @object)
        {
            ObjList.Add(@object);
        }

        public void Remove(int idx)
        {
            ObjList.RemoveAt(idx);
        }

        public void Dispose()
        {
            ObjList.Clear();
            ObjList = null;
        }

        public MapCellinfo()
        {
            ObjList = new List<CellObject>();
            Attribute = CellAttribute.Walk;
        }
    }

    public class PointInfo
    {
        public short nX;
        public short nY;

        public PointInfo(short x, short y)
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

        public TRect(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }
}