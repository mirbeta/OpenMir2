namespace SystemModule.CoreSocket
{
    /// <summary>
    /// PackageBase包结构数据。
    /// </summary>
    [IntelligentCoder.AsyncMethodPoster(Flags = IntelligentCoder.MemberFlags.Public)]
    public abstract partial class PackageBase : IPackage
    {
        /// <inheritdoc/>
        public abstract void Package(ByteBlock byteBlock);

        /// <inheritdoc/>
        public abstract void Unpackage(ByteBlock byteBlock);
    }
}