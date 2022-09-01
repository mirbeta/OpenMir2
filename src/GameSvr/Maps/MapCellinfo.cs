using SystemModule;

namespace GameSvr.Maps
{
    public class MapCellInfo
    {
        public bool Valid => Attribute == CellAttribute.Walk;

        public CellAttribute Attribute;

        /// <summary>
        /// 对象数量
        /// </summary>
        public int Count;
    
        /// <summary>
        /// 地图对象列表
        /// </summary>
        public IList<CellObject> ObjList;

        public void Add(CellObject cell, EntityId entityId)
        {
            ObjList.Add(cell);
            M2Share.CellObjectSystem.Add(cell.CellObjId, entityId);
            Interlocked.Increment(ref Count);
        }

        public void Remove(CellObject cell)
        {
            if (ObjList != null && cell != null)
            {
                ObjList.Remove(cell);
                M2Share.CellObjectSystem.Remove(cell.CellObjId);
                cell = null;
            }
            Interlocked.Decrement(ref Count);
        }

        public void Dispose()
        {
            ObjList.Clear();
        }

        public MapCellInfo()
        {
            ObjList = new List<CellObject>();
            Attribute = CellAttribute.Walk;
        }
    }
}