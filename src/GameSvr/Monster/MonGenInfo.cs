using System.Collections;
using System.Collections.Concurrent;
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
    }
}