namespace GameSvr.Command
{
    public class GameCmd
    {
        /// <summary>
        /// 命令名称
        /// </summary>
        public string CommandName { get; set; }
        /// <summary>
        /// 最小权限
        /// </summary>
        public int nPerMissionMin { get; set; }
        /// <summary>
        /// 最大权限
        /// </summary>
        public int nPerMissionMax { get; set; }
    }
}