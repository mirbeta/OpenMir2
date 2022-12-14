using NLog;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private readonly IdWorker _idWorker = new IdWorker(1);
        private readonly ConcurrentQueue<int> _idQueue = new ConcurrentQueue<int>();
        private readonly Thread IdWorkThread;
        private readonly IList<int> ActorIds = new Collection<int>();
        /// <summary>
        /// 精灵列表
        /// </summary>
        private readonly ConcurrentDictionary<int, BaseObject> _actorsMap = new ConcurrentDictionary<int, BaseObject>();
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
                if (_idQueue.Count < 20000)
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    for (var i = 0; i < 50000; i++)
                    {
                        var sequence = Environment.TickCount + HUtil32.Sequence();
                        while (sequence < 0)
                        {
                            sequence = Environment.TickCount + HUtil32.Sequence();
                            if (sequence > 0) break;
                        }
                        _idQueue.Enqueue(sequence);
                    }
                    sw.Stop();
                    _logger.Debug($"Id生成完毕 耗时:{sw.Elapsed} 当前可用ID数:[{_idQueue.Count}]");
                }
                Thread.Sleep(5000);
            }
        }

        public int Dequeue()
        {
            return _idQueue.TryDequeue(out var sequence) ? sequence : HUtil32.Sequence();
        }

        public void Add(BaseObject actor)
        {
            _actorsMap.TryAdd(actor.ActorId, actor);
        }
        
        public BaseObject Get(int actorId)
        {
            return _actorsMap.TryGetValue(actorId, out var actor) ? actor : null;
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
        /// 清理
        /// </summary>
        public void ClearObject()
        {
            ActorIds.Clear();
            var actors = _actorsMap.GetEnumerator();
            while (actors.MoveNext())
            {
                var actor = actors.Current.Value;
                if (actors.Current.Value.Death)
                {
                    MonsterDeathCount++;
                }
                if (!actor.Ghost || actor.GhostTick <= 0) continue;
                if ((HUtil32.GetTickCount() - actor.DeathTick) <= M2Share.Config.MakeGhostTime)
                {
                    continue; //死亡对象清理时间
                }
                ActorIds.Add(actors.Current.Key);
            }
            foreach (var actorId in ActorIds)
            {
                if (_actorsMap.TryRemove(actorId, out var actor))
                {
                    if (actor.Race != ActorRace.Play)
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