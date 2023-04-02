using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace SystemModule.CoreSocket.Common.Options
{
    /// <summary>
    /// Ssl配置
    /// </summary>
    public abstract class SslOption
    {
        /// <summary>
        /// Ssl配置
        /// </summary>
        public SslOption()
        {
            CertificateValidationCallback = this.OnCertificateValidationCallback;
        }

        private bool OnCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// 协议版本
        /// </summary>
        public SslProtocols SslProtocols { get; set; } = SslProtocols.Tls12 | SslProtocols.Ssl2 | SslProtocols.Ssl3 | SslProtocols.Default |
                                                         SslProtocols.Tls | SslProtocols.Tls11;

        /// <summary>
        /// 该值指定身份验证期间是否检查证书吊销列表
        /// </summary>
        public bool CheckCertificateRevocation { get; set; } = false;

        /// <summary>
        /// SSL验证回调。
        /// </summary>
        public RemoteCertificateValidationCallback CertificateValidationCallback { get; set; }
    }
}