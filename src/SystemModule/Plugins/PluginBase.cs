using SystemModule.CoreSocket;

namespace SystemModule.Plugins
{
    /// <summary>
    /// PluginBase
    /// </summary>
    public class PluginBase : DisposableObject, IPlugin
    {
        /// <inheritdoc/>
        public int Order { get; set; }
    }
}