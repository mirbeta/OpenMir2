using System;
using System.Text;
using SystemModule.Extensions;

namespace SystemModule.Otp
{
    public sealed class TotpOptions
    {
        /// <summary>
        /// 计算 code 的算法
        /// The algorithm for calculating the totp code
        /// </summary>
        public OtpHashAlgorithm Algorithm { get; set; } = OtpHashAlgorithm.SHA1;

        /// <summary>
        /// 生成的 code 长度
        /// The expected code length, 4-9 expected
        /// </summary>
        public int Size
        {
            get => _size;
            set
            {
                if (value is > 9 or < 4)
                    throw new ArgumentOutOfRangeException(nameof(value), value, @"Size out of range, allowed range 4~9");
                _size = value;
            }
        }

        /// <summary>
        /// 过期时间，单位是秒
        /// The code expire time, 300s by default, should be 30,60,90..., the min value is 30s
        /// </summary>
        public int ExpiresIn { get; set; } = 300;

        private string? _salt;
        private int _size = 6;

        /// <summary>
        /// Salt for security consideration
        /// </summary>
        public string? Salt
        {
            get => _salt;
            set
            {
                _salt = value;
                SaltBytes = value.IsNullOrEmpty() ? null : Encoding.UTF8.GetBytes(value);
            }
        }

        internal byte[]? SaltBytes { get; private set; }
    }
}
