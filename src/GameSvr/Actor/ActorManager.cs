using System.Collections.Concurrent;
using System.Diagnostics;
using SystemModule;

namespace GameSvr.Actor
{
    /// <summary>
    /// 对象管理系统
    /// </summary>
    public class ActorMgr
    {
        /// <summary>
        /// 精灵对象列表
        /// </summary>
        private readonly ConcurrentDictionary<int, TBaseObject> _actorsMap = new ConcurrentDictionary<int, TBaseObject>();
        /// <summary>
        /// 其他对象
        /// </summary>
        private readonly ConcurrentDictionary<int, object> _ohter = new ConcurrentDictionary<int, object>();

        public void Add(int actorId, TBaseObject actor)
        {
            _actorsMap.TryAdd(actorId, actor);
        }

        public void AddOhter(int objectId, object obj)
        {
            _ohter.TryAdd(objectId, obj);
        }

        public object GetOhter(int objectId)
        {
            object obj = null;
            if (_ohter.TryGetValue(objectId, out obj))
            {
                return obj;
            }
            return null;
        }

        public TBaseObject Get(int actorId)
        {
            TBaseObject actor = null;
            if (_actorsMap.TryGetValue(actorId, out actor))
            {
                return actor;
            }
            return actor;
        }

        public void Remove(int actorId)
        {
            TBaseObject ghostactor = null;
            _actorsMap.TryRemove(actorId, out ghostactor);
            if (ghostactor != null)
            {
                Debug.WriteLine($"清理死亡对象 名称:[{ghostactor.CharName}] 地图:{ghostactor.MapName} 坐标:{ghostactor.CurrX}:{ghostactor.CurrY}");
            }
        }

        public void RevomeOhter(int actorId)
        {
            object actor = null;
            _ohter.TryRemove(actorId, out actor);
            if (actor != null)
            {
                Debug.WriteLine($"清理死亡对象 [{actorId}]");
            }
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void ClearObject()
        {
            var actorIds = _actorsMap.Keys;
            TBaseObject actor = null;
            var playCount = 0;
            var monsterCount = 0;
            foreach (var actorId in actorIds)
            {
                if (_actorsMap.TryGetValue(actorId, out actor))
                {
                    if (actor.Race == Grobal2.RC_PLAYOBJECT)
                    {
                        playCount++;
                    }
                    else
                    {
                        monsterCount++;
                    }
                    if (actor.Ghost && actor.GhostTick > 0)
                    {
                        if ((HUtil32.GetTickCount() - actor.DeathTick) > M2Share.Config.dwMakeGhostTime) //超过清理时间
                        {
                            Remove(actorId);
                        }
                    }
                }
            }
            Debug.WriteLine($"在线人物:[{playCount}] 怪物总数:[{monsterCount}]");
        }
    }
}