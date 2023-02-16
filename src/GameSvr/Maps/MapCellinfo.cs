using SystemModule.NativeList.Utils;

namespace GameSvr.Maps
{
    public struct MapCellInfo : IDisposable
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
        
        public void Init()
        {
            ObjList = new NativeList<CellObject>();
        }
        
        public void Add(CellObject cell, ActorEntity entityId)
        {
            ObjList.Add(cell);
        }

        public void Update(int index, CellObject cell)
        {
            cell.AddTime = HUtil32.GetTickCount();
            ObjList[index] = cell;
        }

        public void Remove(int index)
        {
            ObjList.RemoveAt(index);
        }

        public void Clear()
        {
            ObjList.Clear();
        }

        /// <summary>执行与释放或重置非托管资源关联的应用程序定义的任务。</summary>
        public void Dispose()
        {
            //必须为true
            Dispose(true);
            //通知垃圾回收器不再调用终结器
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 非密封类可重写的Dispose方法，方便子类继承时可重写
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }
            //清理托管资源
            if (disposing)
            {
                ObjList.Clear();
                ObjList.Dispose();
            }
            //告诉自己已经被释放
            IsDisposed = true;
        }
    }
}