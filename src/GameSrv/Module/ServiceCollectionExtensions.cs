using System.Reflection;
using System.Runtime.Loader;

namespace GameSrv.Module
{
    public static class ServiceCollectionExtensions
    {
        private static readonly IModuleConfigurationManager _modulesConfig = new ModuleConfigurationManager();

        public static IServiceCollection AddModules(this IServiceCollection services)
        {
            IEnumerable<ModuleInfo> modules = _modulesConfig.GetModules();
            if (modules == null)
            {
                return services;
            }
            foreach (ModuleInfo module in modules)
            {
                string modulePath = Path.Combine(AppContext.BaseDirectory, "Plugins", module.Id);
                if (!File.Exists(modulePath))
                {
                    continue;
                }
                if (!module.IsBundledWithHost)
                {
                    TryLoadModuleAssembly(module.Id, module);
                    if (module.Assembly == null)
                    {
                        throw new Exception($"Cannot find main assembly for module {module.Id}");
                    }
                }
                else
                {
                    module.Assembly = Assembly.LoadFile(modulePath);
                }

                GameShare.Modules.Add(module);
            }
            return services;
        }

        private static void TryLoadModuleAssembly(string moduleFolderPath, ModuleInfo module)
        {
            const string binariesFolderName = "plugins";
            string modulePath = Path.Combine(AppContext.BaseDirectory, binariesFolderName, module.Id);

            DirectoryInfo binariesFolder = new DirectoryInfo(modulePath);

            if (Directory.Exists(modulePath))
            {
                AssemblyLoadContext context = new AssemblyLoadContext(modulePath);
                context.Resolving += ContextResolving;
                foreach (FileSystemInfo file in binariesFolder.GetFileSystemInfos("*.dll", SearchOption.AllDirectories))
                {
                    Assembly assembly;
                    try
                    {
                        assembly = context.LoadFromAssemblyPath(file.FullName);
                        if (assembly == null)
                        {
                            throw new Exception($"初始化{file.FullName}功能模块失败.");
                        }
                    }
                    catch (FileLoadException)
                    {
                        // Get loaded assembly. This assembly might be loaded
                        assembly = Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(file.Name)));

                        if (assembly == null)
                        {
                            throw;
                        }

                        string loadedAssemblyVersion = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
                        string tryToLoadAssemblyVersion = FileVersionInfo.GetVersionInfo(file.FullName).FileVersion;

                        // Or log the exception somewhere and don't add the module to list so that it will not be initialized
                        if (tryToLoadAssemblyVersion != loadedAssemblyVersion)
                        {
                            throw new Exception($"Cannot load {file.FullName} {tryToLoadAssemblyVersion} because {assembly.Location} {loadedAssemblyVersion} has been loaded");
                        }
                    }

                    if (Path.GetFileNameWithoutExtension(assembly.ManifestModule.Name) == module.Id)
                    {
                        module.Assembly = assembly;
                    }
                }
            }
        }

        /// <summary>
        /// 加载依赖项
        /// </summary>
        /// <returns></returns>
        private static Assembly ContextResolving(AssemblyLoadContext context, AssemblyName assemblyName)
        {
            string expectedPath = Path.Combine(AppContext.BaseDirectory, assemblyName.Name + ".dll");
            if (File.Exists(expectedPath))
            {
                try
                {
                    using FileStream stream = File.OpenRead(expectedPath);
                    return context.LoadFromStream(stream);
                }
                catch (Exception ex)
                {
                    LogService.Error($"加载依赖项{expectedPath} 发生异常:{ex.Message},{ex.StackTrace}");
                }
            }
            else
            {
                LogService.Error($"依赖项不存在:{expectedPath}");
            }
            return null;
        }
    }
}
