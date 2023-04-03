using System.Security.Cryptography.X509Certificates;

namespace SystemModule.Sockets.Common.Options
{
    /// <summary>
    /// 服务器Ssl设置
    /// </summary>
    public class ServiceSslOption : SslOption
    {
        private X509Certificate certificate;

        /// <summary>
        /// 证书
        /// </summary>
        public X509Certificate Certificate
        {
            get => certificate;
            set => certificate = value;
        }

        private bool clientCertificateRequired;

        /// <summary>
        /// 该值指定是否向客户端请求证书用于进行身份验证。 请注意，这只是一个请求 - 如果没有提供任何证书，服务器仍然可接受连接请求
        /// </summary>
        public bool ClientCertificateRequired
        {
            get => clientCertificateRequired;
            set => clientCertificateRequired = value;
        }
    }
}