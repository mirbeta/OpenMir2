using System.Collections.Concurrent;

namespace GameSvr.Maps
{
    public class CellObjectMgr
    {
        //todo 从当前地图管理对象，而不是统一管理
        private readonly ConcurrentDictionary<int, object> _cellObject = new ConcurrentDictionary<int, object>();

        public void Add(int cellId, object cell)
        {
            if (!_cellObject.TryAdd(cellId, cell))
            {
                //Console.WriteLine($"添加失败. cellId:{cellId} cell:{cell.ActorId}");
            }
        }

        public T Get<T>(int cellId)
        {
            return _cellObject.TryGetValue(cellId, out object cell) ? (T)cell : default(T);
        }

        public void Remove(int cellId)
        {
            if (!_cellObject.TryRemove(cellId, out _))
            {
                //Console.WriteLine($"删除失败. cellId:{cellId}");
            }
        }

        public void Dispose(int cellId)
        {
            if (_cellObject.TryRemove(cellId, out _))
            {
                //cell.Dispose();
            }
        }
    }
}