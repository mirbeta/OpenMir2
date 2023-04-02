namespace SystemModule.CoreSocket;

/// <summary>
/// PluginsManagerExtension
/// </summary>
public static class PluginsManagerExtension
{
    /// <summary>
    /// 添加插件
    /// </summary>
    /// <typeparam name="TPlugin">插件类型</typeparam>
    /// <returns>插件类型实例</returns>
    public static TPlugin Add<TPlugin>(this IPluginsManager pluginsManager) where TPlugin : class, IPlugin
    {
        pluginsManager.Container.RegisterSingleton<TPlugin>();
        TPlugin obj = pluginsManager.Container.Resolve<TPlugin>();
        pluginsManager.Add(obj);
        return obj;
    }

    /// <summary>
    /// 添加插件
    /// </summary>
    /// <typeparam name="TPlugin">插件类型</typeparam>
    /// <param name="pluginsManager"></param>
    /// <param name="ps">创建插件相关构造函数插件</param>
    /// <returns>插件类型实例</returns>
    public static TPlugin Add<TPlugin>(this IPluginsManager pluginsManager, params object[] ps) where TPlugin : class, IPlugin
    {
        pluginsManager.Container.RegisterSingleton<TPlugin>();
        TPlugin obj = pluginsManager.Container.Resolve<TPlugin>(ps);
        pluginsManager.Add(obj);
        return obj;
    }

    /// <summary>
    /// 清空插件
    /// </summary>
    public static void Clear(this IPluginsManager pluginsManager)
    {
        pluginsManager.Clear();
    }

    /// <summary>
    /// 移除插件
    /// </summary>
    /// <param name="pluginsManager"></param>
    /// <param name="plugin"></param>
    public static void Remove(this IPluginsManager pluginsManager, IPlugin plugin)
    {
        pluginsManager.Remove(plugin);
    }

    /// <summary>
    /// 移除插件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pluginsManager"></param>
    public static void Remove<T>(this IPluginsManager pluginsManager) where T : IPlugin
    {
        pluginsManager.Remove(typeof(T));
    }
}