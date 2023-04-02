using System.Net;
using SystemModule.CoreSocket;

/// <summary>
/// 显示信息
/// </summary>
/// <param name="client"></param>
/// <param name="e"></param>
public delegate void MessageEventHandler<TClient>(TClient client, MsgEventArgs e);

/// <summary>
/// 普通通知
/// </summary>
/// <typeparam name="TClient"></typeparam>
/// <param name="client"></param>
/// <param name="e"></param>
public delegate void TouchSocketEventHandler<TClient>(TClient client, TouchSocketEventArgs e);

/// <summary>
/// ID修改通知
/// </summary>
/// <typeparam name="TClient"></typeparam>
/// <param name="client"></param>
/// <param name="e"></param>
public delegate void IDChangedEventHandler<TClient>(TClient client, IDChangedEventArgs e);

/// <summary>
/// Connecting
/// </summary>
/// <typeparam name="TClient"></typeparam>
/// <param name="client"></param>
/// <param name="e"></param>
public delegate void ConnectingEventHandler<TClient>(TClient client, ConnectingEventArgs e);

/// <summary>
/// 客户端断开连接
/// </summary>
/// <typeparam name="TClient"></typeparam>
/// <param name="client"></param>
/// <param name="e"></param>
public delegate void DisconnectEventHandler<TClient>(TClient client, DisconnectEventArgs e);

/// <summary>
/// 正在连接事件
/// </summary>
/// <typeparam name="TClient"></typeparam>
/// <param name="client"></param>
/// <param name="e"></param>
public delegate void OperationEventHandler<TClient>(TClient client, OperationEventArgs e);

/// <summary>
/// 插件数据
/// </summary>
/// <param name="client"></param>
/// <param name="e"></param>
public delegate void PluginReceivedEventHandler<TClient>(TClient client, ReceivedDataEventArgs e);

/// <summary>
/// 普通数据
/// </summary>
/// <param name="client"></param>
/// <param name="byteBlock"></param>
/// <param name="requestInfo"></param>
public delegate void ReceivedEventHandler<TClient>(TClient client, ByteBlock byteBlock, IRequestInfo requestInfo);

/// <summary>
/// UDP接收
/// </summary>
/// <param name="endpoint"></param>
/// <param name="byteBlock"></param>
/// <param name="requestInfo"></param>
public delegate void UdpReceivedEventHandler(EndPoint endpoint, ByteBlock byteBlock, IRequestInfo requestInfo);