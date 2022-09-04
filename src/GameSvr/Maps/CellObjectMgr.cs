using GameSvr.Actor;
using System.Collections.Concurrent;
using System.Diagnostics;
using SystemModule;

namespace GameSvr.Maps
{
    public class CellObjectMgr
    {
        //todo 从当前地图获取地图对象，而不是统一管理
        private readonly ConcurrentDictionary<int, EntityId> _cellObject = new ConcurrentDictionary<int, EntityId>();

        public void Add(int cellId, EntityId cell)
        {
            _cellObject.TryAdd(cellId, cell);
        }
        
        public EntityId Get(int cellId)
        {
            EntityId cell = null;
            if (_cellObject.TryGetValue(cellId, out cell))
            {
                return cell;
            }
            return null;
        }

        public void Remove(int cellId)
        {
            //var obj = M2Share.ObjectManager.Get(cellId);
            //if (obj != null)
            //{
            //   M2Share.ObjectManager.Remove(cellId);
            //   obj = null;
            //}
            _cellObject.Remove(cellId, out var cell);
        }

        public void Dispose(int cellId)
        {
            if (_cellObject.TryGetValue(cellId, out var cell))
            {
                cell = null;
            }
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void ClearObject()
        {
            var actorIds = _cellObject.Keys;
            TBaseObject actor = null;
            var playCount = 0;
            var monsterCount = 0;

            Debug.WriteLine($"在线人物:[{playCount}] 怪物总数:[{monsterCount}]");
        }
    }
}