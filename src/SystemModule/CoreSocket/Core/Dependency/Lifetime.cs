namespace SystemModule.CoreSocket;

/// <summary>
/// 注入项的生命周期。
/// </summary>
public enum Lifetime
{
    /// <summary>
    /// 单例对象
    /// </summary>
    Singleton,

    /// <summary>
    /// 以<see cref="IContainerProvider"/>接口为区域实例单例。
    /// </summary>
    Scoped,

    /// <summary>
    /// 瞬时对象
    /// </summary>
    Transient
}