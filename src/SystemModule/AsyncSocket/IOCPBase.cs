using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SystemModule.AsyncSocket
{
    public abstract class IOCPBase
    {
        #region Properties
        public abstract int BufferSize { get; protected set; }
        protected internal int DefaultConcurrencyLevel
        {
            get
            {
                return 4 * PlatformHelper.ProcessorCount;
            }
        }
        #endregion

        #region Set Socket Options

        protected void SetSocketOptions(Socket socket)
        {
            socket.ReceiveBufferSize = this.BufferSize;
            socket.SendBufferSize = this.BufferSize;
        }

        #endregion

        #region Connect

        protected internal void ProcessConnect(SocketAsyncEventArgs e, SocketResult result)
        {
            Socket s = e.AcceptSocket;
            var userToken = e.UserToken as UserToken;
            try
            {
                userToken.SetResult(result);
            }
            catch (Exception ex)
            {
                userToken.SetException(ex);
            }
            finally
            {
                RecycleToken(userToken);
            }
        }

        #endregion

        #region Disconnect

        protected internal void ProcessDisconnect(SocketAsyncEventArgs e, SocketResult result)
        {
            var userToken = e.UserToken as UserToken;
            try
            {
                userToken.SetResult(result);
            }
            catch (Exception ex)
            {
                userToken.SetException(ex);
            }
            finally
            {
                RecycleToken(userToken);
            }
        }

        #endregion

        #region Receive

        protected internal void ProcessReceive(SocketAsyncEventArgs e, SocketResult result)
        {
            var userToken = e.UserToken as UserToken;
            Socket s = userToken.ConnectSocket;
            try
            {
                result.Length = e.BytesTransferred;
                result.ReceiveAvailable = s.Available > 0;
                if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
                {
                    result.Data = new byte[e.BytesTransferred];
                    Array.Copy(e.Buffer, e.Offset, result.Data, 0, result.Data.Length);
                    Array.Clear(e.Buffer, e.Offset, result.Data.Length);
                }
                userToken.SetResult(result);
            }
            catch (Exception ex)
            {
                userToken.SetException(ex);
            }
            finally
            {
                RecycleToken(userToken);
            }
        }

        #endregion

        #region Send

        protected internal void ProcessSend(SocketAsyncEventArgs e, SocketResult result)
        {
            var userToken = e.UserToken as UserToken;
            try
            {
                userToken.SetResult(result);
            }
            catch (Exception ex)
            {
                userToken.SetException(ex);
            }
            finally
            {
                RecycleToken(userToken);
            }
        }

        protected internal void ProcessSendPackets(SocketAsyncEventArgs e, SocketResult result)
        {
            var userToken = e.UserToken as UserToken;
            try
            {
                userToken.SetResult(result);
            }
            catch (Exception ex)
            {
                userToken.SetException(ex);
            }
            finally
            {
                userToken.ReceiveArgs.SendPacketsElements = null;
                userToken.ReceiveArgs.SendPacketsFlags = TransmitFileOptions.UseDefaultWorkerThread;
                RecycleToken(userToken);
            }

        }

        #endregion

        #region Abstract

        #region Close
        public abstract void CloseClientSocket(Socket socket);

        protected abstract void RecycleToken(UserToken token);
        #endregion

        #region Async Receive

        public abstract Task<SocketResult> ReceiveAsync(Socket socket, int timeOut = -1);

        #endregion

        #region Async Send

        public abstract Task<SocketResult> SendAsync(Socket socket, byte[] data, int timeOut = -1);

        #endregion

        #region Async Send File

        public abstract Task<SocketResult> SendFileAsync(Socket socket, string fileName, int timeOut = -1);

        #endregion

        #region Async Disconnect

        public abstract Task<SocketResult> DisconnectAsync(Socket socket, int timeOut = -1);

        #endregion

        #endregion

        #region Callback
        protected void OnIOCompleted(object serder, SocketAsyncEventArgs e)
        {
            SocketResult result = new SocketResult { SocketError = e.SocketError, Args = e };
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    ProcessConnect(e, result);
                    break;
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e, result);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e, result);
                    break;
                case SocketAsyncOperation.SendPackets:
                    ProcessSendPackets(e, result);
                    break;
                case SocketAsyncOperation.Disconnect:
                    ProcessDisconnect(e, result);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }
        #endregion
    }
}