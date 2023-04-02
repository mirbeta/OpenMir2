using System;
using TouchSocket.Core;

namespace TouchSocket.Sockets;

/// <summary>
/// 终端接口
/// </summary>
public interface IClient : IDependencyObject, IDisposable
{
    /// <summary>
    /// 处理未经过适配器的数据。返回值表示是否继续向下传递。
    /// </summary>
    Func<ByteBlock, bool> OnHandleRawBuffer { get; set; }

    /// <summary>
    /// 处理经过适配器后的数据。返回值表示是否继续向下传递。
    /// </summary>
    Func<ByteBlock, IRequestInfo, bool> OnHandleReceivedData { get; set; }

    /// <summary>
    /// 终端协议
    /// </summary>
    Protocol Protocol { get; set; }

    /// <summary>
    /// 简单IOC容器
    /// </summary>
    IContainer Container { get; }

    /// <summary>
    /// 最后一次接收到数据的时间
    /// </summary>
    DateTime LastReceivedTime { get; }

    /// <summary>
    /// 最后一次发送数据的时间
    /// </summary>
    DateTime LastSendTime { get; }
}