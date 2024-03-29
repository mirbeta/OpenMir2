﻿using System.Threading.Tasks;

namespace SocketPlugin.Impl
{
    /// <summary>
    /// ServicePlugin
    /// </summary>
    public class ServicePlugin<TService> : PluginBase, IServicePlugin
    {
        /// <inheritdoc cref="IServicePlugin.OnStarted(object, ServiceStateEventArgs)"/>
        protected virtual void OnStarted(TService sender, ServiceStateEventArgs e)
        {

        }

        /// <inheritdoc cref="IServicePlugin.OnStartedAsync(object, ServiceStateEventArgs)"/>
        protected virtual Task OnStartedAsync(TService sender, ServiceStateEventArgs e)
        {
            return EasyTask.CompletedTask;
        }

        /// <inheritdoc cref="IServicePlugin.OnStoped(object, ServiceStateEventArgs)"/>
        protected virtual void OnStoped(TService sender, ServiceStateEventArgs e)
        {

        }

        /// <inheritdoc cref="IServicePlugin.OnStopedAsync(object, ServiceStateEventArgs)"/>
        protected virtual Task OnStopedAsync(TService sender, ServiceStateEventArgs e)
        {
            return EasyTask.CompletedTask;
        }

        void IServicePlugin.OnStarted(object sender, ServiceStateEventArgs e)
        {
            OnStarted((TService)sender, e);
        }

        Task IServicePlugin.OnStartedAsync(object sender, ServiceStateEventArgs e)
        {
            return OnStartedAsync((TService)sender, e);
        }

        void IServicePlugin.OnStoped(object sender, ServiceStateEventArgs e)
        {
            OnStoped((TService)sender, e);
        }

        Task IServicePlugin.OnStopedAsync(object sender, ServiceStateEventArgs e)
        {
            return OnStopedAsync((TService)sender, e);
        }
    }

    /// <summary>
    /// TcpServicePlugin
    /// </summary>
    public class TcpServicePlugin : ServicePlugin<TcpService>
    {

    }

    /// <summary>
    /// UdpServicePlugin
    /// </summary>
    public class UdpServicePlugin : ServicePlugin<UdpSession>
    {

    }
}