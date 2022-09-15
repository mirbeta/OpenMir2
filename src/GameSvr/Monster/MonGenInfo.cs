using GameSvr.Actor;

namespace GameSvr.Monster
{
    /// <summary>
    /// 怪物刷新信息
    /// </summary>
    public class MonGenInfo
    {
        public string MapName;
        public int X;
        public int Y;
        public string MonName;
        public int Range;
        public int Count;
        public int ActiveCount;
        public int ZenTime;
        public int MissionGenRate;
        /// <summary>
        /// 对象列表
        /// Key:线程ID
        /// Values:怪物列表
        /// </summary>
        public IList<BaseObject> CertList;
        public int CertCount;
        public object Envir;
        public int Race;
        /// <summary>
        /// 创建时间
        /// </summary>
        public int StartTick;
        /// <summary>
        /// 怪物所在线程
        /// </summary>
        public int ThreadId;

        /// <summary>
        /// 添加对象到列表
        /// 返回true则当前线程怪物列表未满
        /// 返回false则表示当前线程怪物列表已经满了
        /// </summary>
        /// <param name="baseObject"></param>
        /// <returns></returns>
        public bool TryAdd(BaseObject baseObject)
        {
            if (CertList.Count <= MonGenConst.ThreadMonLimit)
            {
                return false;
            }
            CertList.Add(baseObject);
            return true;

        }

        public MonGenInfo Clone()
        {
            return (MonGenInfo)this.MemberwiseClone();
        }

    }

    public class MonGenConst
    {
        public const int ThreadMonLimit = 40000;
    
    }
}