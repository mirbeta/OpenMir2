namespace SystemModule
{
    /// <summary>
    /// 怪物刷新信息
    /// </summary>
    public class MonGenInfo
    {
        public string MapName;
        public int X;
        public int Y;
        /// <summary>
        /// 怪物名称
        /// </summary>
        public string MonName;
        /// <summary>
        /// 范围
        /// </summary>
        public int Range;
        /// <summary>
        /// 刷新数量
        /// </summary>
        public int Count;
        public int ActiveCount;
        /// <summary>
        /// 刷新时间
        /// </summary>
        public int ZenTime;
        public int MissionGenRate;
        /// <summary>
        /// 对象列表
        /// </summary>
        public IList<IActor> CertList;
        public int CertCount;
        public IEnvirnoment Envir;
        public int Race;
        /// <summary>
        /// 创建时间
        /// </summary>
        public int StartTick;
        /// <summary>
        /// 死亡释放
        /// </summary>
        public bool DeathRelease;

        /// <summary>
        /// 添加对象到列表
        /// 返回true则当前线程怪物列表未满
        /// 返回false则表示当前线程怪物列表已经满了
        /// </summary>
        /// <param name="baseObject"></param>
        /// <returns></returns>
        public bool TryAdd(IActor baseObject)
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
