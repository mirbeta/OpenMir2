using NLog;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SystemModule.Enums;
using SystemModule.Generation.Entities;

namespace GameSvr.Actor
{
    /// <summary>
    /// 精灵管理
    /// </summary>
    public sealed class ActorMgr
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ConcurrentQueue<int> GenerateQueue = new ConcurrentQueue<int>();
        private readonly StandardRandomizer standardRandomizer = new StandardRandomizer();
        private readonly Thread IdWorkThread;
        private readonly IList<int> ActorIds = new List<int>(1000);
        /// <summary>
        /// 精灵列表
        /// </summary>
        private readonly ConcurrentDictionary<int, ActorEntity> _actorsMap = new ConcurrentDictionary<int, ActorEntity>();
        /// <summary>
        /// 其他对象
        /// </summary>
        private readonly ConcurrentDictionary<int, object> _ohterMap = new ConcurrentDictionary<int, object>();
        private static int MonsterDeathCount { get; set; }
        private static int MonsterDisposeCount { get; set; }
        private static int PlayerGhostCount { get; set; }

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
                if (GenerateQueue.Count < 20000)
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    for (int i = 0; i < 100000; i++)
                    {
                        int sequence = standardRandomizer.NextInteger();
                        if (_actorsMap.ContainsKey(sequence))
                        {
                            while (true)
                            {
                                sequence = standardRandomizer.NextInteger();
                                if (!_actorsMap.ContainsKey(sequence))
                                {
                                    break;
                                }
                            }
                        }
                        while (sequence < 0)
                        {
                            sequence = Environment.TickCount + HUtil32.Sequence();
                            if (sequence > 0) break;
                        }
                        GenerateQueue.Enqueue(sequence);
                    }
                    sw.Stop();
                    _logger.Debug($"Id生成完毕 耗时:{sw.Elapsed} 可用数:[{GenerateQueue.Count}]");
                }
                Thread.Sleep(5000);
            }
        }

        public int Dequeue()
        {
            return GenerateQueue.TryDequeue(out int sequence) ? sequence : HUtil32.Sequence();
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
            _logger.Debug($"总对象数:[{_actorsMap.Count}] 角色死亡数:[{PlayerGhostCount}] 怪物死亡数:[{MonsterDeathCount}] 怪物释放数:[{MonsterDisposeCount}]");
            M2Share.Statistics.ShowServerState();
        }
    }
}