using System.Collections.Concurrent;
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
        private readonly IdWorker _idWorker = new IdWorker(M2Share.RandomNumber.Random(15));
        private readonly ConcurrentQueue<int> _idQueue = new ConcurrentQueue<int>();
        private Thread IdWorkThread;
        /// <summary>
        /// 精灵列表
        /// </summary>
        private readonly ConcurrentDictionary<int, BaseObject> _actorsMap = new ConcurrentDictionary<int, BaseObject>();
        /// <summary>
        /// 其他对象
        /// </summary>
        private readonly ConcurrentDictionary<int, object> _ohter = new ConcurrentDictionary<int, object>();

        public void Start()
        {
            IdWorkThread = new Thread(Initialization)
            {
                IsBackground = true
            };
            IdWorkThread.Start();
        }

        private void Initialization(object obj)
        {
            while (true)
            {
                if (_idQueue.Count < 1000)
                {
                    for (var i = 0; i < 100000; i++)
                    {
                        _idQueue.Enqueue((int)_idWorker.NextId());
                    }
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
            if (_actorsMap.ContainsKey(actorId))
            {
                actorId = Dequeue();
            }
            _actorsMap.TryAdd(actorId, actor);
            return actorId;
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

        private void Remove(int actorId)
        {
            BaseObject ghostactor;
            _actorsMap.TryRemove(actorId, out ghostactor);
            if (ghostactor != null)
            {
                Debug.WriteLine($"清理死亡对象 名称:[{ghostactor.ChrName}] 地图:{ghostactor.MapName} 坐标:{ghostactor.CurrX}:{ghostactor.CurrY}");
            }
        }

        public void RevomeOhter(int actorId)
        {
            object actor;
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
            var playCount = 0;
            var monsterCount = 0;
            foreach (var actorId in actorIds)
            {
                BaseObject actor;
                if (_actorsMap.TryGetValue(actorId, out actor))
                {
                    if (actor.Race == ActorRace.Play)
                    {
                        playCount++;
                    }
                    else
                    {
                        monsterCount++;
                    }
                    if (actor.Ghost && actor.GhostTick > 0)
                    {
                        if ((HUtil32.GetTickCount() - actor.DeathTick) > M2Share.Config.MakeGhostTime) //超过清理时间
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