namespace SystemModule.Data
{
    /// <summary>
    /// 假人登陆结构
    /// </summary>
    public struct RoBotLogon
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string sChrName;
        /// <summary>
        /// 地图
        /// </summary>
        public string sMapName;
        /// <summary>
        /// 人物配置路径
        /// </summary>
        public string sConfigFileName;
        /// <summary>
        /// 英雄配置路径
        /// </summary>
        public string sHeroConfigFileName;
        public string sFilePath;
        /// <summary>
        /// 人物配置列表目录
        /// </summary>
        /// <returns></returns>
        public string sConfigListFileName;
        /// <summary>
        /// 英雄配置列表目录
        /// </summary>
        public string sHeroConfigListFileName;
        /// <summary>
        /// X坐标
        /// </summary>
        public short nX;
        /// <summary>
        /// Y坐标
        /// </summary>
        public short nY;
    }
}