using System.Diagnostics;
using Collections.Pooled;
using SystemModule;

namespace GameSvr.Maps
{
    public class CellObjectMgr
    {
        //todo 从当前地图管理对象，而不是统一管理
        private readonly PooledDictionary<int, ActorEntity> _cellObject = new PooledDictionary<int, ActorEntity>();

        public void Add(int cellId, ActorEntity cell)
        {
            _cellObject.TryAdd(cellId, cell);
        }

        public ActorEntity Get(int cellId)
        {
            return _cellObject.TryGetValue(cellId, out var cell) ? cell : null;
        }

        public void Remove(int cellId)
        {
            _cellObject.Remove(cellId);
        }

        public void Dispose(int cellId)
        {
            if (_cellObject.TryGetValue(cellId, out var cell))
            {
                cell.Dispose();
            }
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void ClearObject()
        {
            var actorIds = _cellObject.Keys;
            var playCount = 0;
            var monsterCount = 0;

            Debug.WriteLine($"在线人物:[{playCount}] 怪物总数:[{monsterCount}]");
        }
    }
}