using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;

namespace SystemModule.CoreSocket;

/// <summary>
/// NameValueCollectionDebugView
/// </summary>
public class NameValueCollectionDebugView
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly NameValueCollection m_nameValue;

    /// <summary>
    /// NameValueCollectionDebugView
    /// </summary>
    /// <param name="nameValue"></param>
    public NameValueCollectionDebugView(NameValueCollection nameValue)
    {
        m_nameValue = nameValue;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private Dictionary<string, string> KV
    {
        get
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (string item in m_nameValue.AllKeys)
            {
                dic.TryAdd(item, m_nameValue[item]);
            }
            return dic;
        }
    }
}