namespace SystemModule.CoreSocket;

/// <summary>
/// PluginBase
/// </summary>
public class PluginBase : DisposableObject, IPlugin
{
    /// <inheritdoc/>
    public int Order { get; set; }
}