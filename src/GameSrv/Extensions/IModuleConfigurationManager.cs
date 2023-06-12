namespace GameSrv.Extensions
{
    public interface IModuleConfigurationManager
    {
        IEnumerable<ModuleInfo> GetModules();
    }
}
