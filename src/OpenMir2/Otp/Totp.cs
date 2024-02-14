using System;
using System.Security.Cryptography;

namespace OpenMir2.Otp
{
    /// <summary>
    /// Time-Based One-Time Password
    /// https://datatracker.ietf.org/doc/html/rfc6238
    /// </summary>
    public class Totp
    {
        private readonly OtpHashAlgorithm _hashAlgorithm;
        private readonly int _codeSize;
        private readonly int _base;

        /// <summary>
        /// Create a totp instance with with default algorithm(SHA1 by default) and default code size(6 by default)
        /// </summary>
        public Totp() : this(OtpHashAlgorithm.SHA1)
        {
        }

        /// <summary>
        /// Create a totp instance with with default algorithm(SHA1 by default) and expected code size
        /// </summary>
        /// <param name="codeSize">The expected code size, 6 by default, should between 1 and 9</param>
        /// <exception cref="ArgumentOutOfRangeException">Exception when codeSize invalid</exception>
        public Totp(int codeSize) : this(OtpHashAlgorithm.SHA1, codeSize)
        {
        }

        /// <summary>
        /// Create a totp instance
        /// </summary>
        /// <param name="otpHashAlgorithm">The hash algorithm to compute, SHA1 by default</param>
        /// <param name="codeSize">The expected code size, 6 by default, should between 1 and 9</param>
        /// <exception cref="ArgumentOutOfRangeException">Exception when codeSize invalid</exception>
        public Totp(OtpHashAlgorithm otpHashAlgorithm, int codeSize = 6)
        {
            // valid input parameter
            if (codeSize is <= 0 or >= 10)
            {
                throw new ArgumentOutOfRangeException(nameof(codeSize), codeSize, @"The codeSize must between 1 and 9");
            }
            _codeSize = codeSize;
            _hashAlgorithm = otpHashAlgorithm;
            _base = (int)Math.Pow(10, _codeSize);
        }

        /// <summary>
        /// Compute totp
        /// </summary>
        /// <param name="securityToken">base32 encoded token/secret</param>
        /// <returns>computed totp code</returns>
        public virtual string Compute(string securityToken) => Compute(Base32EncodeHelper.GetBytes(securityToken));

        /// <summary>
        /// Compute totp
        /// </summary>
        /// <param name="securityToken">security token/secret</param>
        /// <returns>computed totp code</returns>
        public virtual string Compute(byte[] securityToken) => Compute(securityToken, GetCurrentTimeStepNumber());

        /// <summary>
        /// Compute totp with ttl
        /// </summary>
        /// <param name="securityToken">security token/secret</param>
        /// <returns>computed totp code and code ttl</returns>
        public virtual (string Code, int Ttl) ComputeWithTtl(byte[] securityToken)
        {
            long currentStep = GetCurrentTimeStepNumber();
            int ttl = Ttl(currentStep);
            if (ttl < 1)
            {
                //going to be expired
                currentStep++;
                ttl = TimeStepSeconds;
            }
            string totp = Compute(securityToken, currentStep);
            return (totp, ttl);
        }

        /// <summary>
        /// Verify whether the input code is correct
        /// </summary>
        /// <param name="securityToken">base32 encoded token/secret</param>
        /// <param name="code">The code to validate</param>
        /// <param name="timeToleration">The time that could be treated as valid</param>
        /// <returns>whether the code is valid, <c>true</c> valid, otherwise invalid</returns>
        public virtual bool Verify(string securityToken, string code, TimeSpan? timeToleration = null) => Verify(Base32EncodeHelper.GetBytes(securityToken), code, timeToleration);

        /// <summary>
        /// Verify whether the input code is correct
        /// </summary>
        /// <param name="securityToken">base32 encoded token/secret</param>
        /// <param name="code">The code to validate</param>
        /// <param name="timeToleration">The time that could be treated as valid</param>
        /// <returns>whether the code is valid, <c>true</c> valid, otherwise invalid</returns>
        public virtual bool Verify(byte[] securityToken, string code, TimeSpan? timeToleration = null)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return false;
            }

            if (code.Length != _codeSize)
            {
                return false;
            }

            long step = GetCurrentTimeStepNumber();
            int futureStep = timeToleration is { TotalSeconds: > TimeStepSeconds }
                ? Math.Min((int)(timeToleration.Value.TotalSeconds / TimeStepSeconds), MaxTimeSteps)
                : 1;
            for (int i = 0; i < futureStep; i++)
            {
                string totp = Compute(securityToken, step - i);
                if (totp == code)
                {
                    return true;
                }
            }
            return false;
        }

        private string Compute(byte[] securityToken, long counter)
        {
            using HMAC hmac = _hashAlgorithm switch
            {
                OtpHashAlgorithm.SHA256 => new HMACSHA256(securityToken),
                OtpHashAlgorithm.SHA512 => new HMACSHA512(securityToken),
                _ => new HMACSHA1(securityToken)
            };
            byte[] stepBytes = BitConverter.GetBytes(counter);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(stepBytes); // need BigEndian
            }
            // See https://tools.ietf.org/html/rfc4226
            byte[] hashResult = hmac.ComputeHash(stepBytes);

            int offset = hashResult[^1] & 0xf;
            string p = $"{hashResult[offset]:X2}{hashResult[offset + 1]:X2}{hashResult[offset + 2]:X2}{hashResult[offset + 3]:X2}";
            long num = Convert.ToInt64(p, 16) & 0x7FFFFFFF;
            string code = (num % _base).ToString("");
            return code.PadLeft(_codeSize, '0');
        }

        /// <summary>
        /// time step
        /// 30s(Recommend)
        /// </summary>
        public const int TimeStepSeconds = 30;

        /// <summary>
        /// MaxTimeSteps
        /// </summary>
        public const int MaxTimeSteps = 20;

        /// <summary>
        /// MaxTimeStepSeconds
        /// </summary>
        public const int MaxTimeStepSeconds = TimeStepSeconds * MaxTimeSteps;

        // More info: https://tools.ietf.org/html/rfc6238#section-4
        private static long GetCurrentTimeStepNumber() => DateTimeOffset.UtcNow.ToUnixTimeSeconds() / TimeStepSeconds;

        private static int Ttl(long step) => (int)((step + 1) * TimeStepSeconds - DateTimeOffset.UtcNow.ToUnixTimeSeconds());
    }
}