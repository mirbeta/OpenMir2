using System;

namespace TouchSocket.Core;

internal class ContainerProvider : IContainerProvider
{
    private readonly Container m_container;

    public ContainerProvider(Container container)
    {
        m_container = container;
    }

    public bool IsRegistered(Type fromType, string key = "")
    {
        return m_container.IsRegistered(fromType, key);
    }

    public object Resolve(Type fromType, object[] ps = null, string key = "")
    {
        return m_container.Resolve(fromType, ps, key);
    }
}