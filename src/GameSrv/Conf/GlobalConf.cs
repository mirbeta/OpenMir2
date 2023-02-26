using SystemModule.Common;

namespace GameSrv.Conf {
    public class GlobalConf : ConfigFile {
        public GlobalConf(string fileName) : base(fileName) {
            Load();
        }

        public void LoadConfig() {
            for (int i = 0; i < M2Share.Config.GlobalVal.Length; i++) {
                M2Share.Config.GlobalVal[i] = ReadWriteInteger("Integer", "GlobalVal" + i, M2Share.Config.GlobalVal[i]);
            }
            for (int i = 0; i < M2Share.Config.GlobalAVal.Length; i++) {
                M2Share.Config.GlobalAVal[i] = ReadWriteString("String", "GlobalStrVal" + i, M2Share.Config.GlobalAVal[i]);
            }
        }
    }
}