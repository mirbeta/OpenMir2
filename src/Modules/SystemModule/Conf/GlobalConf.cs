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
            for (int i = 0; i < SystemShare.Config.GlobalVal.Length; i++)
            {
                SystemShare.Config.GlobalVal[i] = ReadWriteInteger("Integer", "GlobalVal" + i, SystemShare.Config.GlobalVal[i]);
            }
            for (int i = 0; i < SystemShare.Config.GlobalAVal.Length; i++)
            {
                SystemShare.Config.GlobalAVal[i] = ReadWriteString("String", "GlobalStrVal" + i, SystemShare.Config.GlobalAVal[i]);
            }
        }
    }
}