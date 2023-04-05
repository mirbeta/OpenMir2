using System.Collections.Concurrent;
using NLog;
using SystemModule.Enums;

namespace GameSrv.Actor
{
    /// <summary>
    /// 精灵管理
    /// </summary>
    public sealed class ActorMgr
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ConcurrentQueue<int> _generateQueue = new ConcurrentQueue<int>();
        private readonly IList<int> ActorIds = new List<int>(1000);
        /// <summary>
        /// 精灵列表
        /// </summary>
        private readonly ConcurrentDictionary<int, ActorEntity> _actorsMap = new ConcurrentDictionary<int, ActorEntity>();
        /// <summary>
        /// 其他对象
        /// </summary>
        private readonly ConcurrentDictionary<int, object> _ohterMap = new ConcurrentDictionary<int, object>();
        private static int PlayerCount { get; set; }
        private static int MonsterCount { get; set; }
        private static int MonsterDeathCount { get; set; }
        private static int MonsterDisposeCount { get; set; }
        private static int PlayerGhostCount { get; set; }

        public int GetNextIdentity()
        {
            return _generateQueue.TryDequeue(out var sequence) ? sequence : HUtil32.Sequence();
        }

        public int GetGenerateQueueCount()
        {
            return _generateQueue.Count;
        }

        public void AddToQueue(int sequence)
        {
            _generateQueue.Enqueue(sequence);
        }

        public bool ContainsKey(int actorId)
        {
            return _actorsMap.ContainsKey(actorId);
        }

        public void Add(BaseObject actor)
        {
            _actorsMap.TryAdd(actor.ActorId, actor);
        }

        public BaseObject Get(int actorId)
        {
            return _actorsMap.TryGetValue(actorId, out var actor) ? (BaseObject)actor : null;
        }

        public T Get<T>(int actorId) where T : ActorEntity
        {
            return _actorsMap.TryGetValue(actorId, out var actor) ? (T)actor : default;
        }

        public void SendMessage(int actorId, SendMessage sendMessage)
        {
            if (_actorsMap.TryGetValue(actorId, out var actor))
            {
                actor.AddMessage(sendMessage);
            }
        }

        public void AddOhter(int objectId, object obj)
        {
            _ohterMap.TryAdd(objectId, obj);
        }

        public object GetOhter(int objectId)
        {
            return _ohterMap.TryGetValue(objectId, out var obj) ? obj : null;
        }

        public void RevomeOhter(int actorId)
        {
            _ohterMap.TryRemove(actorId, out var actor);
        }

        /// <summary>
        /// 清理无效或者死亡对象
        /// </summary>
        public void CleanObject()
        {
            foreach (var actorId in ActorIds)
            {
                if (_actorsMap.TryRemove(actorId, out var actor))
                {
                    if (((BaseObject)actor).Race != ActorRace.Play)
                    {
                        MonsterDisposeCount++;
                    }
                    actor.Dispose();
                }
            }
            ActorIds.Clear();
        }

        /// <summary>
        /// 统计对象数量
        /// </summary>
        public void Analytics()
        {
            ActorIds.Clear();
            PlayerCount = 0;
            MonsterCount = 0;
            using var actors = _actorsMap.GetEnumerator();
            while (actors.MoveNext())
            {
                var actor = (BaseObject)actors.Current.Value;
                if (!actor.Death && !actor.Ghost)
                {
                    if (actor.Race == ActorRace.Play)
                    {
                        PlayerCount++;
                        PlayerGhostCount++;
                    }
                    else
                    {
                        MonsterCount++;
                    }
                }
                else if (actor.Race != ActorRace.Play && actor.Death || actor.Ghost)
                {
                    MonsterDeathCount++;
                }
                if (!actor.Ghost || actor.GhostTick <= 0) continue;
                if ((HUtil32.GetTickCount() - actor.GhostTick) <= 20000)//死亡对象清理时间
                {
                    continue;
                }
                ActorIds.Add(actors.Current.Key);
            }
            _logger.Debug($"对象数:[{_actorsMap.Count}] 玩家/怪物:[{PlayerCount}/{MonsterCount}] 死亡角色:[{PlayerGhostCount}] 死亡怪物:[{MonsterDeathCount}] 释放怪物:[{MonsterDisposeCount}]");
        }
    }
}