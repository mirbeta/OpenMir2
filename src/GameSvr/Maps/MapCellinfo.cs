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

        public List<CellObject> ObjList;

        public void Add(CellObject cell, EntityId entityId)
        {
            //HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            ObjList.Add(cell);
            M2Share.CellObjectSystem.Add(cell.CellObjId, entityId);
            Interlocked.Increment(ref Count);
            //HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
        }

        public void Remove(CellObject cell)
        {
            //HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            if (ObjList != null && cell != null)
            {
                ObjList.Remove(cell);
                M2Share.CellObjectSystem.Remove(cell.CellObjId);
                cell = null;
            }
            Interlocked.Decrement(ref Count);
            //HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
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