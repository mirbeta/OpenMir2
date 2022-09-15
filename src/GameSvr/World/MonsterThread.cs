using SystemModule;

namespace GameSvr.World
{
    public class MonsterThread
    {
        /// <summary>
        /// 线程ID
        /// </summary>
        public int Id = 0;
        public long LastRunTime = 0;
        public long StartTime = 0;
        public long EndTime = 0;
        public bool Stop = false;
        /// <summary>
        /// 当前怪物列表处理位置索引
        /// </summary>
        public int MonGenCertListPosition;
        public int MonGenListPosition;
        /// <summary>
        /// 处理怪物数，用于统计处理怪物个数
        /// </summary>
        public int MonsterProcessCount;
        /// <summary>
        /// 处理怪物总数位置，用于计算怪物总数
        /// </summary>
        public int MonsterProcessPostion;
        /// <summary>
        /// 当前怪物索引ID
        /// </summary>
        public int CurrMonGenIdx;
        /// <summary>
        /// 怪物刷新间隔
        /// </summary>
        public int RegenMonstersTick;

        public MonsterThread()
        {
            RegenMonstersTick = HUtil32.GetTickCount();
        }
    }
}