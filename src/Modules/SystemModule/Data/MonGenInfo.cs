using SystemModule.Actors;
using SystemModule.Maps;

namespace SystemModule.Data
{
    /// <summary>
    /// 怪物刷新信息
    /// </summary>
    public class MonGenInfo
    {
        public string MapName;
        public short X;
        public short Y;
        /// <summary>
        /// 怪物名称
        /// </summary>
        public string MonName;
        /// <summary>
        /// 范围
        /// </summary>
        public byte Range;
        /// <summary>
        /// 刷新数量
        /// </summary>
        public ushort Count;
        public ushort ActiveCount;
        /// <summary>
        /// 刷新时间
        /// </summary>
        public int ZenTime;
        public byte MissionGenRate;
        /// <summary>
        /// 对象列表
        /// </summary>
        public IList<IMonsterActor> CertList;
        public IEnvirnoment Envir;
        public short Race;
        /// <summary>
        /// 创建时间
        /// </summary>
        public int StartTick;

        /// <summary>
        /// 添加对象到列表
        /// 返回true则当前线程怪物列表未满
        /// 返回false则表示当前线程怪物列表已经满了
        /// </summary>
        /// <param name="baseObject"></param>
        /// <returns></returns>
        public bool TryAdd(IMonsterActor baseObject)
        {
            if (CertList.Count > MonGenConst.ThreadMonLimit)
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
