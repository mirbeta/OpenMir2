using System.Security.Cryptography.X509Certificates;

namespace TouchSocket.Sockets
{
    /// <summary>
    /// 客户端Ssl验证
    /// </summary>
    public class ClientSslOption : SslOption
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ClientSslOption()
        {
            X509Store store = new X509Store(StoreName.Root);
            store.Open(OpenFlags.ReadWrite);
            ClientCertificates = store.Certificates;
            store.Close();
        }

        private string targetHost;

        /// <summary>
        /// 目标Host
        /// </summary>
        public string TargetHost
        {
            get => targetHost;
            set => targetHost = value;
        }

        /// <summary>
        /// 验证组合
        /// </summary>
        public X509CertificateCollection ClientCertificates { get; set; }
    }
}