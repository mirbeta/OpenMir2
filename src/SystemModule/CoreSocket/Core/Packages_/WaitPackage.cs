namespace TouchSocket.Core;

/// <summary>
/// WaitPackage
/// </summary>
public class WaitPackage : PackageBase, IWaitResult
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public long Sign { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public byte Status { get; set; }

    /// <inheritdoc/>
    public override void Package(ByteBlock byteBlock)
    {
        byteBlock.Write(Sign);
        byteBlock.Write(Status);
        byteBlock.Write(Message);
    }

    /// <inheritdoc/>
    public override void Unpackage(ByteBlock byteBlock)
    {
        Sign = byteBlock.ReadInt64();
        Status = (byte)byteBlock.ReadByte();
        Message = byteBlock.ReadString();
    }
}