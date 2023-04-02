using System;
using SystemModule.Plugins;

namespace SystemModule.CoreSocket
{
    /// <summary>
    /// 重连插件
    /// </summary>
    [SingletonPlugin]
    public sealed class ReconnectionPlugin<TClient> : TcpPluginBase where TClient : class, ITcpClient
    {
        private readonly Func<TClient, bool> m_tryCon;

        /// <summary>
        /// 初始化一个重连插件
        /// </summary>
        /// <param name="tryCon">无论如何，只要返回True，则结束本轮尝试</param>
        public ReconnectionPlugin(Func<TClient, bool> tryCon)
        {
            Order = int.MinValue;
            m_tryCon = tryCon;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="client"></param>
        /// <param name="e"></param>
        protected override void OnDisconnected(ITcpClientBase client, DisconnectEventArgs e)
        {
            base.OnDisconnected(client, e);

            if (client is ITcpClient tcpClient)
            {
                if (e.Manual)
                {
                    return;
                }
                EasyTask.Run(() =>
                {
                    while (true)
                    {
                        try
                        {
                            if (m_tryCon.Invoke((TClient)tcpClient))
                            {
                                break;
                            }
                        }
                        catch
                        {
                        }
                    }
                });
            }
        }
    }
}