using System.Collections.Concurrent;

namespace GameSvr.Maps
{
    public class CellObjectMgr
    {
        //todo 从当前地图管理对象，而不是统一管理
        private readonly ConcurrentDictionary<int, ActorEntity> _cellObject = new ConcurrentDictionary<int, ActorEntity>();

        public void Add(int cellId, ActorEntity cell)
        {
            //_cellObject.TryAdd(cellId, cell);
            if (!_cellObject.TryAdd(cellId, cell))
            {
                //Console.WriteLine($"添加失败. cellId:{cellId} cell:{cell.ActorId}");
            }
        }

        public ActorEntity Get(int cellId)
        {
            return _cellObject.TryGetValue(cellId, out var cell) ? cell : null;
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
            if (_cellObject.TryRemove(cellId, out var cell))
            {
                cell.Dispose();
            }
        }
    }
}