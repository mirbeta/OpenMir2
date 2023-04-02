namespace TouchSocket.Core;

/// <summary>
/// 具有插件功能的对象
/// </summary>
public interface IPluginObject
{
    /// <summary>
    /// 内置IOC容器
    /// </summary>
    IContainer Container { get; }

    /// <summary>
    /// 插件管理器
    /// </summary>
    IPluginsManager PluginsManager { get; }

    /// <summary>
    /// 是否已启用插件
    /// </summary>
    bool UsePlugin { get; }
}