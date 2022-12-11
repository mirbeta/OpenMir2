using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using NLog;
using System.Diagnostics;
using SystemModule;

namespace GameSvr.Actor
{
    /// <summary>
    /// 精灵管理
    /// </summary>
    public class ActorMgr
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IdWorker _idWorker = new IdWorker(M2Share.RandomNumber.Random(15));
        private readonly ConcurrentQueue<int> _idQueue = new ConcurrentQueue<int>();
        private readonly Thread IdWorkThread;
        /// <summary>
        /// 精灵列表
        /// </summary>
        private readonly ConcurrentDictionary<int, BaseObject> _actorsMap = new ConcurrentDictionary<int, BaseObject>();
        /// <summary>
        /// 其他对象
        /// </summary>
        private readonly ConcurrentDictionary<int, object> _ohter = new ConcurrentDictionary<int, object>();
        private readonly IList<int> ActorIds = new Collection<int>();

        public ActorMgr()
        {
            IdWorkThread = new Thread(GenerateId)
            {
                IsBackground = true
            };
            IdWorkThread.Start();
        }

        private void GenerateId(object obj)
        {
            while (true)
            {
                if (_idQueue.Count < 1000)
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    Parallel.For(0, 10, i =>
                    {
                        _logger.Debug($"线程[{i}]开始生成Id");
                        for (var j = 0; j < 5000; j++)
                        {
                            _idQueue.Enqueue((int)_idWorker.NextId());
                        }
                    });
                    sw.Stop();
                    _logger.Debug($"Id生成完毕 耗时:{sw.Elapsed}");
                }
                Thread.Sleep(5000);
            }
        }

        public int Dequeue()
        {
            if (_idQueue.TryDequeue(out var sequence))
            {
                return sequence;
            }
            return (int)_idWorker.NextId();
        }
        
        public int Add(BaseObject actor)
        {
            var actorId = Dequeue();
            _actorsMap.TryAdd(actorId, actor);
            return actorId;
        }

        public void AddOhter(int objectId, object obj)
        {
            _ohter.TryAdd(objectId, obj);
        }

        public object GetOhter(int objectId)
        {
            return _ohter.TryGetValue(objectId, out var obj) ? obj : null;
        }

        public BaseObject Get(int actorId)
        {
            return _actorsMap.TryGetValue(actorId, out var actor) ? actor : null;
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
                if ((HUtil32.GetTickCount() - actor.DeathTick) <= M2Share.Config.MakeGhostTime)
                {
                    continue; //死亡对象清理时间
                }
                ActorIds.Add(actors.Current.Key);
                actors.Dispose();
            }
            foreach (var actorId in ActorIds)
            {
                if (_actorsMap.TryRemove(actorId, out var actor))
                {
                    _logger.Debug($"清理死亡对象 名称:[{actor.ChrName}] 地图:{actor.MapName} 坐标:{actor.CurrX}:{actor.CurrY}");
                }
            }
        }
    }
}