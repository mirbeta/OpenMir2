using System;

namespace TouchSocket.Core;

/// <summary>
/// 插件接口
/// </summary>
public interface IPlugin : IDisposable
{
    /// <summary>
    /// 插件执行顺序
    /// <para>该属性值越大，越靠前执行。值相等时，按添加先后顺序</para>
    /// <para>该属性效果，仅在<see cref="IPluginsManager.Add(IPlugin)"/>之前设置有效。</para>
    /// </summary>
    int Order { get; set; }
}