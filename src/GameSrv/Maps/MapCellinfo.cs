using SystemModule.NativeList.Utils;

namespace GameSrv.Maps
{
    public struct MapCellInfo
    {
        /// <summary>
        /// 对象数量
        /// </summary>
        public int Count;
        /// <summary>
        /// 地图对象列表
        /// </summary>
        public NativeList<CellObject> ObjList { get; set; }
        /// <summary>
        /// 是否可以移动
        /// </summary>
        public bool Valid => Attribute == CellAttribute.Walk;
        /// <summary>
        /// 移动标识
        /// </summary>
        public CellAttribute Attribute = CellAttribute.Walk;

        public MapCellInfo()
        {
            ObjList = null;
        }

        public bool IsAvailable => Count > 0;

        public void Add(CellObject cell)
        {
            ObjList.Add(cell);
            Interlocked.Increment(ref Count);
        }

        public void Update(int index, ref CellObject cell)
        {
            cell.AddTime = HUtil32.GetTickCount();
            ObjList[index] = cell;
        }

        public void Remove(CellObject index)
        {
            //todo 异步通知处理并移除
            ObjList.Remove(index);
            Interlocked.Decrement(ref Count);
        }

        public void Remove(int index)
        {
            ObjList.RemoveAt(index);
            Interlocked.Decrement(ref Count);
        }

        public void SetAttribute(CellAttribute cellAttribute)
        {
            Attribute = cellAttribute;
        }

        public void Clear()
        {
            ObjList.Clear();
            Count = 0;
            //ObjList = null;
        }
    }
}