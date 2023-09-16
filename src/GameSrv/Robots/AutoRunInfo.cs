namespace GameSrv.Robots
{
    public class AutoRunInfo
    {
        /// <summary>
        /// 上一次运行时间记录
        /// </summary>
        public long RunTick;
        /// <summary>
        /// 执行时间
        /// </summary>        
        public long RunTimeTick;
        /// <summary>
        /// 运行类型
        /// </summary>
        public int Moethod;
        /// <summary>
        /// 运行脚本标签
        /// </summary>
        public string sParam1;
        /// <summary>
        /// 传送到脚本参数内容
        /// </summary>
        public string sParam2;
        public string sParam3;
        public string sParam4;
        public int nParam1;
        public int nParam2;
        public int nParam3;
        public int nParam4;
        /// <summary>
        /// 状态
        /// </summary>
        public bool Status;
    }
}


