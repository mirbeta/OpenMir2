using System.Text.Json;

namespace GameSrv.Module
{
    public class ModuleConfigurationManager : IModuleConfigurationManager
    {
        public static readonly string ModulesFilename = "modules.json";

        public IEnumerable<ModuleInfo> GetModules()
        {
            string modulesPath = Path.Combine(SystemShare.BasePath, ModulesFilename);
            if (!File.Exists(modulesPath))
            {
                return null;
            }
            using StreamReader reader = new StreamReader(modulesPath);
            string content = reader.ReadToEnd();
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            return JsonSerializer.Deserialize<IList<ModuleInfo>>(content, options);
        }
    }
}