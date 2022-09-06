using Collections.Pooled;
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
        public int Count => ObjList.Count;

        /// <summary>
        /// 地图对象列表
        /// </summary>
        public PooledList<CellObject> ObjList;

        public void Add(CellObject cell, EntityId entityId)
        {
            using (M2Share.syncLock.EnterWriteLock())
            {
                ObjList.Add(cell);
                M2Share.CellObjectSystem.Add(cell.CellObjId, entityId);
            }
        }

        public void Remove(CellObject cell)
        {
            using (M2Share.syncLock.EnterReadLock())
            {
                if (ObjList != null && cell != null)
                {
                    ObjList.Remove(cell);
                    M2Share.CellObjectSystem.Remove(cell.CellObjId);
                    cell = null;
                }
            }
        }

        public void Dispose()
        {
            ObjList.Clear();
            ObjList.Dispose();
        }

        public MapCellInfo()
        {
            ObjList = new PooledList<CellObject>();
            Attribute = CellAttribute.Walk;
        }
    }
}