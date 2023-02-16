using SystemModule.NativeList.Utils;

namespace GameSvr.Maps
{
    public struct MapCellInfo
    {
        public static readonly MapCellInfo LowWall = new()
        {
            Attribute = CellAttribute.LowWall
        };

        public static readonly MapCellInfo HighWall = new()
        {
            Attribute = CellAttribute.HighWall
        };
        
        public bool Valid => Attribute == CellAttribute.Walk;

        public CellAttribute Attribute = CellAttribute.Walk;

        /// <summary>
        /// 对象数量
        /// </summary>
        public int Count => ObjList.Count;

        /// <summary>
        /// 地图对象列表
        /// </summary>
        public NativeList<CellObject> ObjList;

        public bool IsAvailable => ObjList?.Count > 0 && !IsDisposed;

        private bool IsDisposed { get; set; }

        public MapCellInfo()
        {
            IsDisposed = false;
        }

        public void Create()
        {
            ObjList = new NativeList<CellObject>();
        }
        
        public void Add(CellObject cell, ActorEntity entityId)
        {
            ObjList.Add(cell);
        }

        public void Update(int index, ref CellObject cell)
        {
            cell.AddTime = HUtil32.GetTickCount();
            ObjList[index] = cell;
        }

        public void Remove(int index)
        {
            ObjList.RemoveAt(index);
        }
        
        public void SetAttribute(CellAttribute cellAttribute)
        {
            Attribute = cellAttribute;
        }

        public void Clear()
        {
            ObjList.Clear();
        }
    }
}