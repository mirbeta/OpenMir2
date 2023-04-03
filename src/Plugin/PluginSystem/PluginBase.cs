using SystemModule.CoreSocket;

namespace PluginSystem
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