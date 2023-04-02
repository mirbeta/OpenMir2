using System.Collections.Specialized;

namespace TouchSocket.Core;

/// <summary>
/// 元数据键值对。
/// </summary>
public class Metadata : NameValueCollection, IPackage
{
    /// <summary>
    /// 元数据键值对。
    /// </summary>
    public Metadata()
    {
    }

    /// <summary>
    /// 添加。如果键存在，将被覆盖。
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public new Metadata Add(string name, string value)
    {
        base.Add(name, value);
        return this;
    }

    /// <summary>
    /// 打包
    /// </summary>
    /// <param name="byteBlock"></param>
    public void Package(ByteBlock byteBlock)
    {
        byteBlock.Write(Count);
        foreach (string item in AllKeys)
        {
            byteBlock.Write(item);
            byteBlock.Write(this[item]);
        }
    }

    /// <summary>
    /// 解包
    /// </summary>
    /// <param name="byteBlock"></param>
    public void Unpackage(ByteBlock byteBlock)
    {
        int count = byteBlock.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            string key = byteBlock.ReadString();
            string value = byteBlock.ReadString();
            Add(key, value);
        }
    }
}