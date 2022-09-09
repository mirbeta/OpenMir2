namespace SystemModule.Data
{
    public class SlaveInfo
    {
        /// <summary>
        /// 宝宝名字
        /// </summary>
        public string SlaveName;
        /// <summary>
        /// 宝宝等级
        /// </summary>
        public byte SlaveLevel;
        /// <summary>
        /// 叛变时间
        /// </summary>
        public int RoyaltySec;
        /// <summary>
        /// 杀怪数量
        /// </summary>
        public int KillCount;
        public byte SalveLevel;
        public byte SlaveExpLevel;
        public ushort nHP;
        public ushort nMP;

        public SlaveInfo()
        {

        }
    }
}