using SystemModule.Common;

namespace GameSvr.Conf
{
    public class GlobalConfig : IniFile
    {
        public GlobalConfig(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadConfig()
        {
            for (var i = 0; i < M2Share.Config.GlobalVal.Length; i++)
            {
                var nLoadInteger = ReadInteger("Integer", "GlobalVal" + i, -1);
                if (nLoadInteger < 0)
                {
                    WriteInteger("Integer", "GlobalVal" + i, M2Share.Config.GlobalVal[i]);
                }
                else
                {
                    M2Share.Config.GlobalVal[i] = nLoadInteger;
                }
            }
            for (var i = 0; i < M2Share.Config.GlobalAVal.Length; i++)
            {
                var sLoadString = ReadString("String", "GlobalStrVal" + i, "");
                if (string.IsNullOrEmpty(sLoadString))
                {
                    WriteString("String", "GlobalStrVal" + i, M2Share.Config.GlobalAVal[i]);
                }
                else
                {
                    M2Share.Config.GlobalAVal[i] = sLoadString;
                }
            }
        }
    }
}