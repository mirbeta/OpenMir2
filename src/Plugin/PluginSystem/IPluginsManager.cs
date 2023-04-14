namespace PluginEngine
{
    /// <summary>
    /// 插件管理器接口
    /// </summary>
    public interface IPluginsManager : IEnumerable<IPlugin>
    {
        /// <summary>
        /// 标识该插件是否可用。当不可用时，仅可以添加和删除插件，但不会触发插件
        /// </summary>
        bool Enable { get; set; }

        /// <summary>
        /// 添加插件
        /// </summary>
        /// <param name="plugin">插件</param>
        /// <exception cref="ArgumentNullException"></exception>
        void Add(IPlugin plugin);

        /// <summary>
        /// 移除插件
        /// </summary>
        /// <param name="plugin"></param>
        void Remove(IPlugin plugin);

        /// <summary>
        /// 移除插件
        /// </summary>
        /// <param name="type"></param>
        void Remove(Type type);

        /// <summary>
        /// 清除所有插件
        /// </summary>
        void Clear();

        /// <summary>
        /// 触发对应方法
        /// </summary>
        /// <typeparam name="TPlugin">接口类型</typeparam>
        /// <param name="name">触发名称</param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        bool Raise<TPlugin>(string name, object sender, PluginEventArgs e) where TPlugin : IPlugin;
    }
}