
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using SystemModule.Plugins;

namespace SystemModule.CoreSocket;

/// <summary>
/// TCP终端基础接口。
/// <para>
/// 注意：该接口并不仅表示客户端。<see cref="SocketClient"/>也实现了该接口。
/// </para>
/// </summary>
public interface ITcpClientBase : IClient, ISender, IDefaultSender, IPluginObject, IRequsetInfoSender
{
    /// <summary>
    /// 缓存池大小
    /// </summary>
    int BufferLength { get; }

    /// <summary>
    /// 是否允许自由调用<see cref="SetDataHandlingAdapter"/>进行赋值。
    /// </summary>
    bool CanSetDataHandlingAdapter { get; }

    /// <summary>
    /// 客户端配置
    /// </summary>
    TouchSocketConfig Config { get; }

    /// <summary>
    /// 数据处理适配器
    /// </summary>
    DataHandlingAdapter DataHandlingAdapter { get; }

    /// <summary>
    /// 断开连接
    /// </summary>
    DisconnectEventHandler<ITcpClientBase> Disconnected { get; set; }

    /// <summary>
    /// 即将断开连接(仅主动断开时有效)。
    /// <para>
    /// 当主动调用Close断开时，可通过<see cref="TouchSocketEventArgs.IsPermitOperation"/>终止断开行为。
    /// </para>
    /// </summary>
    DisconnectEventHandler<ITcpClientBase> Disconnecting { get; set; }
    /// <summary>
    /// IP地址
    /// </summary>
    string IP { get; }

    /// <summary>
    /// 表示是否为客户端。
    /// </summary>
    bool IsClient { get; }
    /// <summary>
    /// 主通信器
    /// </summary>
    Socket MainSocket { get; }

    /// <summary>
    /// 判断是否在线
    /// <para>该属性仅表示TCP状态是否在线</para>
    /// </summary>
    bool Online { get; }

    /// <summary>
    /// 端口号
    /// </summary>
    int Port { get; }

    /// <summary>
    /// 接收模式
    /// </summary>
    public ReceiveType ReceiveType { get; }

    /// <summary>
    /// 使用Ssl加密
    /// </summary>
    bool UseSsl { get; }


    /// <summary>
    /// 中断终端
    /// </summary>
    void Close();

    /// <summary>
    /// 中断终端，传递中断消息
    /// </summary>
    /// <param name="msg"></param>
    void Close(string msg);

    /// <summary>
    /// 获取流，在正常模式下为<see cref="NetworkStream"/>，在Ssl模式下为<see cref="SslStream"/>。
    /// </summary>
    /// <returns></returns>
    Stream GetStream();

    /// <summary>
    /// 设置数据处理适配器
    /// </summary>
    /// <param name="adapter"></param>
    void SetDataHandlingAdapter(DataHandlingAdapter adapter);
}