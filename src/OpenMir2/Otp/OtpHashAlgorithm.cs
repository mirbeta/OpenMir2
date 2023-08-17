namespace SystemModule.Otp
{
    public enum OtpHashAlgorithm
    {
        /// <summary>
        /// Sha1 is used as the HMAC hashing algorithm
        /// </summary>
        SHA1 = 0,

        /// <summary>
        /// Sha256 is used as the HMAC hashing algorithm
        /// </summary>
        SHA256 = 1,

        /// <summary>
        /// Sha512 is used as the HMAC hashing algorithm
        /// </summary>
        SHA512 = 2,
    }
}