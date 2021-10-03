using System.IO;
using SystemModule.Common;

namespace GameSvr.Configs
{
    public class GlobalConfig
    {
        private readonly IniFile _config;

        public GlobalConfig()
        {
            _config = new IniFile(Path.Combine(M2Share.sConfigPath, M2Share.sGlobalConfigFileName));
            _config.Load();
        }

        public void LoadConfig()
        {
            var nLoadInteger = 0;
            var sLoadString = string.Empty;
            for (var i = M2Share.g_Config.GlobalVal.GetLowerBound(0); i <= M2Share.g_Config.GlobalVal.GetUpperBound(0); i++)
            {
                nLoadInteger = _config.ReadInteger("Integer", "GlobalVal" + i, -1);
                if (nLoadInteger < 0)
                {
                    _config.WriteInteger("Integer", "GlobalVal" + i, M2Share.g_Config.GlobalVal[i]);
                }
                else
                {
                    M2Share.g_Config.GlobalVal[i] = nLoadInteger;
                }
            }
            
            for (var i = M2Share.g_Config.GlobalAVal.GetLowerBound(0); i <= M2Share.g_Config.GlobalAVal.GetUpperBound(0); i++)
            {
                sLoadString = _config.ReadString("String", "GlobalStrVal" + i, "");
                if (string.IsNullOrEmpty(sLoadString))
                {
                    _config.WriteString("String", "GlobalStrVal" + i, M2Share.g_Config.GlobalAVal[i]);
                }
                else
                {
                    M2Share.g_Config.GlobalAVal[i] = sLoadString;
                }
            }
        }
    }
}