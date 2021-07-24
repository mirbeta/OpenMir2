using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace M2Server
{
    public class ObjectSystem
    {
        /// <summary>
        /// 精灵对象列表
        /// </summary>
        private readonly ConcurrentDictionary<int, TBaseObject> Actors = new ConcurrentDictionary<int, TBaseObject>();
        /// <summary>
        /// 其他对象
        /// </summary>
        private readonly ConcurrentDictionary<int, object> Ohter = new ConcurrentDictionary<int, object>();
        private readonly Timer _actorTime = null;

        public ObjectSystem()
        {
            _actorTime = new Timer(DoWork, null, 30000, 60000);//定时移除已经挂掉的对象
            Debug.WriteLine("Start Clear Actor Thread...");
        }

        public void Add(int actorId, TBaseObject actor)
        {
            Actors.TryAdd(actorId, actor);
        }

        public void AddOhter(int objectId, object obj)
        {
            Ohter.TryAdd(objectId, obj);
        }

        public object GetOhter(int objectId)
        {
            object obj = null;
            if (Ohter.TryGetValue(objectId, out obj))
            {
                return obj;
            }
            return null;
        }

        public TBaseObject Get(int actorId)
        {
            TBaseObject actor = null;
            if (Actors.TryGetValue(actorId, out actor))
            {
                return actor;
            }
            return actor;
        }

        public void Revome(int actorId)
        {
            TBaseObject ghostactor = null;
            Actors.TryRemove(actorId, out ghostactor);
            if (ghostactor != null)
            {
                M2Share.MainOutMessage(string.Format("清理死亡对象 名称:[{0}] 地图:{1} 坐标:{2}:{3}", ghostactor.m_sCharName, ghostactor.m_sMapName, ghostactor.m_nCurrX, ghostactor.m_nCurrY));
            }
        }

        public void RevomeOhter(int actorId)
        {
            object actor = null;
            Ohter.TryRemove(actorId, out actor);
            if (actor != null)
            {
                M2Share.MainOutMessage($"清理死亡对象 [{actorId}]");
            }
        }

        private void DoWork(object obj)
        {
            var actorIds = Actors.Keys;
            TBaseObject actor = null;
            foreach (var actorId in actorIds)
            {
                if (Actors.TryGetValue(actorId, out actor))
                {
                    if (actor.m_boGhost && actor.m_dwGhostTick > 0)
                    {
                        if (HUtil32.GetTickCount() - actor.m_dwDeathTick > M2Share.g_Config.dwMakeGhostTime) //超过清理时间
                        {
                            Revome(actorId);
                        }
                    }
                }
            }
        }
    }
}