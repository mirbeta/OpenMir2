using NLog;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using SystemModule.Enums;

namespace GameSvr.Actor
{
    /// <summary>
    /// 精灵管理
    /// </summary>
    public sealed class ActorMgr
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ConcurrentQueue<int> IdQueue = new ConcurrentQueue<int>();
        private readonly IdWorker IdWorker = new IdWorker(1);
        private readonly Thread IdWorkThread;
        private readonly IList<int> ActorIds = new Collection<int>();
        /// <summary>
        /// 精灵列表
        /// </summary>
        private readonly ConcurrentDictionary<int, ActorEntity> _actorsMap = new ConcurrentDictionary<int, ActorEntity>();
        /// <summary>
        /// 其他对象
        /// </summary>
        private readonly ConcurrentDictionary<int, object> _ohterMap = new ConcurrentDictionary<int, object>();
        private int MonsterDeathCount { get; set; }
        private int MonsterDisposeCount { get; set; }
        private int PlayerGhostCount { get; set; }

        public ActorMgr()
        {
            IdWorkThread = new Thread(GenerateIdThread)
            {
                IsBackground = true
            };
            IdWorkThread.Start();
        }

        private void GenerateIdThread(object obj)
        {
            while (true)
            {
                if (IdQueue.Count < 20000)
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    HashSet<long> hashMap = new HashSet<long>();
                    for (int i = 0; i < 50000; i++)
                    {
                        int sequence = IdWorker.NextId();
                        if (hashMap.Contains(sequence))
                        {
                            while (true)
                            {
                                sequence = IdWorker.NextId();
                                if (!hashMap.Contains(sequence))
                                {
                                    break;
                                }
                            }
                        }
                        hashMap.Add(sequence);

                        while (sequence < 0)
                        {
                            sequence = Environment.TickCount + HUtil32.Sequence();
                            if (sequence > 0) break;
                        }
                        IdQueue.Enqueue(sequence);
                    }
                    sw.Stop();
                    _logger.Debug($"Id生成完毕 耗时:{sw.Elapsed} 可用数:[{IdQueue.Count}] 是否有重复:{IdQueue.Count != IdQueue.Distinct().Count()}");
                }
                Thread.Sleep(5000);
            }
        }

        public int Dequeue()
        {
            return IdQueue.TryDequeue(out int sequence) ? sequence : HUtil32.Sequence();
        }

        public void Add(BaseObject actor)
        {
            _actorsMap.TryAdd(actor.ActorId, actor);
        }

        public BaseObject Get(int actorId)
        {
            return _actorsMap.TryGetValue(actorId, out ActorEntity actor) ? (BaseObject)actor : null;
        }

        public T Get<T>(int actorId) where T : ActorEntity
        {
            return _actorsMap.TryGetValue(actorId, out ActorEntity actor) ? (T)actor : default;
        }

        public void AddOhter(int objectId, object obj)
        {
            _ohterMap.TryAdd(objectId, obj);
        }

        public object GetOhter(int objectId)
        {
            return _ohterMap.TryGetValue(objectId, out object obj) ? obj : null;
        }

        public void RevomeOhter(int actorId)
        {
            _ohterMap.TryRemove(actorId, out object actor);
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void ClearObject()
        {
            ActorIds.Clear();
            using IEnumerator<KeyValuePair<int, ActorEntity>> actors = _actorsMap.GetEnumerator();
            while (actors.MoveNext())
            {
                BaseObject actor = (BaseObject)actors.Current.Value;
                if (actor.Death)
                {
                    MonsterDeathCount++;
                }
                if (!actor.Ghost || actor.GhostTick <= 0) continue;
                if ((HUtil32.GetTickCount() - actor.GhostTick) <= 20000)
                {
                    continue; //死亡对象清理时间
                }
                ActorIds.Add(actors.Current.Key);
            }
            foreach (int actorId in ActorIds)
            {
                if (_actorsMap.TryRemove(actorId, out ActorEntity actor))
                {
                    if (((BaseObject)actor).Race != ActorRace.Play)
                    {
                        MonsterDisposeCount++;
                    }
                    else
                    {
                        PlayerGhostCount++;
                    }
                    actors.Dispose();
                    //_logger.Debug($"清理死亡对象 名称:[{actor.ChrName}] 地图:{actor.MapName} 坐标:{actor.CurrX}:{actor.CurrY}");
                }
            }
            _logger.Debug($"当前总对象:[{_actorsMap.Count}] 累计角色死亡次数:[{PlayerGhostCount}] 累计怪物死亡次数:[{MonsterDeathCount}] 累计怪物释放次数:[{MonsterDisposeCount}]");
        }
    }
}