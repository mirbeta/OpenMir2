using SystemModule.Common;

namespace SystemModule{
    public class GlobalConf : ConfigFile
    {
        public GlobalConf(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadConfig()
        {
            for (int i = 0; i < ModuleShare.Config.GlobalVal.Length; i++)
            {
                ModuleShare.Config.GlobalVal[i] = ReadWriteInteger("Integer", "GlobalVal" + i, ModuleShare.Config.GlobalVal[i]);
            }
            for (int i = 0; i < ModuleShare.Config.GlobalAVal.Length; i++)
            {
                ModuleShare.Config.GlobalAVal[i] = ReadWriteString("String", "GlobalStrVal" + i, ModuleShare.Config.GlobalAVal[i]);
            }
        }
    }
}