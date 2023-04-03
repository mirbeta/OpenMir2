namespace PluginSystem
{
    /// <summary>
    /// PlguinObjectExtension
    /// </summary>
    public static class PlguinObjectExtension
    {
        /// <summary>
        /// 添加插件
        /// </summary>
        /// <typeparam name="TPlugin">插件类型</typeparam>
        /// <returns>插件类型实例</returns>
        public static TPlugin AddPlugin<TPlugin>(this IPluginObject plguinObject) where TPlugin : class, IPlugin
        {
            //plguinObject.Container.RegisterSingleton<TPlugin>();
            //TPlugin obj = plguinObject.Container.Resolve<TPlugin>();
            //plguinObject.AddPlugin(obj);
            //return obj;
            return default(TPlugin);
        }

        /// <summary>
        /// 添加插件
        /// </summary>
        /// <param name="plguinObject"></param>
        /// <param name="plugin">插件</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddPlugin(this IPluginObject plguinObject, IPlugin plugin)
        {
            //plguinObject.Container.RegisterSingleton(plugin);
            //plguinObject.PluginsManager.Add(plugin);
            return;
        }

        /// <summary>
        /// 清空插件
        /// </summary>
        public static void ClearPlugins(this IPluginObject plguinObject)
        {
            plguinObject.PluginsManager.Clear();
        }

        /// <summary>
        /// 移除插件
        /// </summary>
        /// <param name="plguinObject"></param>
        /// <param name="plugin"></param>
        public static void RemovePlugin(this IPluginObject plguinObject, IPlugin plugin)
        {
            plguinObject.PluginsManager.Remove(plugin);
        }

        /// <summary>
        /// 移除插件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="plguinObject"></param>
        public static void RemovePlugin<T>(this IPluginObject plguinObject) where T : IPlugin
        {
            plguinObject.PluginsManager.Remove(typeof(T));
        }
    }
}