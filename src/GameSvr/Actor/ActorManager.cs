using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using NLog;
using SystemModule;
using SystemModule.Enums;

namespace GameSvr.Actor
{
    /// <summary>
    /// 精灵管理
    /// </summary>
    public class ActorMgr
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 精灵列表
        /// </summary>
        private readonly ConcurrentDictionary<int, BaseObject> _actorsMap = new ConcurrentDictionary<int, BaseObject>();
        /// <summary>
        /// 其他对象
        /// </summary>
        private readonly ConcurrentDictionary<int, object> _ohter = new ConcurrentDictionary<int, object>();

        private readonly IList<int> ActorIds = new Collection<int>();

        public void Add(int actorId, BaseObject actor)
        {
            _actorsMap.TryAdd(actorId, actor);
        }

        public void AddOhter(int objectId, object obj)
        {
            _ohter.TryAdd(objectId, obj);
        }

        public object GetOhter(int objectId)
        {
            object obj;
            if (_ohter.TryGetValue(objectId, out obj))
            {
                return obj;
            }
            return null;
        }

        public BaseObject Get(int actorId)
        {
            BaseObject actor;
            if (_actorsMap.TryGetValue(actorId, out actor))
            {
                return actor;
            }
            return actor;
        }

        public void RevomeOhter(int actorId)
        {
            object actor;
            _ohter.TryRemove(actorId, out actor);
            if (actor != null)
            {
                _logger.Debug($"清理死亡对象 [{actorId}]");
            }
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void ClearObject()
        {
            ActorIds.Clear();
            var actors = _actorsMap.GetEnumerator();
            while (actors.MoveNext())
            {
                BaseObject actor = actors.Current.Value;
                if (!actor.Ghost || actor.GhostTick <= 0) continue;
                if ((HUtil32.GetTickCount() - actor.DeathTick) <= M2Share.Config.MakeGhostTime) continue; //死亡对象清理时间
                {
                    ActorIds.Add(actors.Current.Key);
                }
                actors.Dispose();
            }
            foreach (var actorId in ActorIds)
            {
                _actorsMap.TryRemove(actorId, out var actor);
                _logger.Debug($"清理死亡对象 名称:[{actor.ChrName}] 地图:{actor.MapName} 坐标:{actor.CurrX}:{actor.CurrY}");
            }
        }
    }
}