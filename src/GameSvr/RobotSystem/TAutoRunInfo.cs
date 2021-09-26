namespace GameSvr
{
    public class AutoRunInfo
    {
        /// <summary>
        /// 上一次运行时间记录
        /// </summary>
        public int dwRunTick;
        /// <summary>
        /// 运行间隔时间长
        /// </summary>        
        public int dwRunTimeLen;
        /// <summary>
        /// 自动运行类型
        /// </summary>
        public int nRunCmd;
        public int nMoethod;
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
        public bool boStatus;
    }
}


