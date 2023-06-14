using GameSrv.Extensions;
using System.Text.Json;
using SystemModule;

namespace GameSrv
{
    public class ModuleConfigurationManager : IModuleConfigurationManager
    {
        public static readonly string ModulesFilename = "modules.json";

        public IEnumerable<ModuleInfo> GetModules()
        {
            var modulesPath = Path.Combine(SystemShare.BasePath, ModulesFilename);
            if (!File.Exists(modulesPath))
            {
                return null;
            }
            using var reader = new StreamReader(modulesPath);
            string content = reader.ReadToEnd();
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            return JsonSerializer.Deserialize<IList<ModuleInfo>>(content, options);
        }
    }
}