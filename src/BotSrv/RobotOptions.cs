namespace BotSrv
{
    public class RobotOptions
    {
        /// <summary>
        /// 服务器名称
        /// </summary>
        public string ServerName { get; set; }
        /// <summary>
        /// 游戏服务器IP地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 服务器端口号
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 账号前缀
        /// </summary>
        public string LoginAccount { get; set; }
        /// <summary>
        /// 同时登录人数
        /// </summary>
        public int ChrCount { get; set; }
        /// <summary>
        /// 登录总人数
        /// </summary>
        public int TotalChrCount { get; set; }
        /// <summary>
        /// 是否创建帐号
        /// </summary>
        public bool NewAccount { get; set; }
    }
}