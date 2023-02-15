using SystemModule.NativeList.Utils;

namespace GameSvr.Maps
{
    public sealed class MapCellInfo : IDisposable
    {
        public static MapCellInfo LowWall = new()
        {
            Attribute = CellAttribute.LowWall
        };

        public static MapCellInfo HighWall = new()
        {
            Attribute = CellAttribute.HighWall
        };

        public bool Valid => Attribute == CellAttribute.Walk;

        public CellAttribute Attribute;

        /// <summary>
        /// 对象数量
        /// </summary>
        public int? Count => ObjList?.Count;

        /// <summary>
        /// 地图对象列表
        /// </summary>
        public NativeList<CellObject> ObjList;

        public bool IsAvailable => ObjList != null && ObjList.Count > 0;

        private bool IsDisposed { get; set; }

        public void Add(CellObject cell, ActorEntity entityId)
        {
            ObjList.Add(cell);
            M2Share.CellObjectMgr.Add(cell.CellObjId, entityId);
        }

        public void Update(int index, CellObject cell)
        {
            cell.AddTime = HUtil32.GetTickCount();
            ObjList[index] = cell;
            //M2Share.CellObjectMgr.Update(cell.CellObjId);
        }

        public void Remove(int index, CellObject cell)
        {
            if (ObjList != null && cell.CellObjId > 0)
            {
                ObjList.RemoveAt(index);
                M2Share.CellObjectMgr.Remove(cell.CellObjId);
            }
        }

        public MapCellInfo()
        {
            Attribute = CellAttribute.Walk;
            ObjList = new NativeList<CellObject>();
        }

        /// <summary>
        /// 为了防止忘记显式的调用Dispose方法
        /// </summary>
        ~MapCellInfo()
        {
            //必须为false
            Dispose(false);
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
                //ObjList = null;
            }
            //告诉自己已经被释放
            IsDisposed = true;
        }
    }
}