using System.Net.Sockets;

namespace SystemModule.AsyncSocket
{
    public sealed class SocketResult
    {
        public static SocketResult NotSocket = new SocketResult
        {
            SocketError = SocketError.NotSocket
        };
        public byte[] Data { get; set; }
        public int Length { get; set; }
        public SocketError SocketError { get; set; }
        public bool ReceiveSuccess
        {
            get
            {
                return this.Length > 0 && this.SocketError == SocketError.Success;
            }
        }
        public bool ReceiveAvailable
        {
            get;
            set;
        }
        public SocketAsyncEventArgs Args { get; set; }
    }
}