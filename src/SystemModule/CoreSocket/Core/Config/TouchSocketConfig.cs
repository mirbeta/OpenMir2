namespace SystemModule.CoreSocket
{
    /// <summary>
    /// 配置文件基类
    /// </summary>
    public class TouchSocketConfig : DependencyObject
    {
        private IContainer m_container;
        private IPluginsManager m_pluginsManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TouchSocketConfig()
        {
            SetContainer(new Container());
        }

        /// <summary>
        /// IOC容器。
        /// </summary>
        public IContainer Container => m_container;

        /// <summary>
        /// 使用插件
        /// </summary>
        public bool IsUsePlugin { get; set; }

        /// <summary>
        /// 插件管理器
        /// </summary>
        public IPluginsManager PluginsManager => m_pluginsManager;

        /// <summary>
        /// 设置注入容器。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public TouchSocketConfig SetContainer(IContainer value)
        {
            m_container = value;
            SetPluginsManager(new PluginsManager(m_container));
            return this;
        }

        /// <summary>
        /// 设置PluginsManager
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public TouchSocketConfig SetPluginsManager(IPluginsManager value)
        {
            m_pluginsManager = value;
            m_container.RegisterSingleton<IPluginsManager>(value);
            return this;
        }

        /// <summary>
        /// 启用插件
        /// </summary>
        /// <returns></returns>
        public TouchSocketConfig UsePlugin()
        {
            IsUsePlugin = true;
            return this;
        }
    }
}