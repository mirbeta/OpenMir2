using System;

namespace SystemModule.CoreSocket;

/// <summary>
/// 具有区域效应的容器。
/// </summary>
public interface IContainerProvider
{
    /// <summary>
    /// 创建目标类型的对应实例。
    /// </summary>
    /// <param name="fromType"></param>
    /// <param name="ps"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    object Resolve(Type fromType, object[] ps = null, string key = "");

    /// <summary>
    /// 判断某类型是否已经注册
    /// </summary>
    /// <param name="fromType"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    bool IsRegistered(Type fromType, string key = "");
}