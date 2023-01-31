using SystemModule.Common;

namespace GameSvr.Conf
{
    public class GlobalConf : IniFile
    {
        public GlobalConf(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadConfig()
        {
            for (int i = 0; i < M2Share.Config.GlobalVal.Length; i++)
            {
                int nLoadInteger = ReadInteger("Integer", "GlobalVal" + i, -1);
                if (nLoadInteger < 0)
                {
                    WriteInteger("Integer", "GlobalVal" + i, M2Share.Config.GlobalVal[i]);
                }
                else
                {
                    M2Share.Config.GlobalVal[i] = nLoadInteger;
                }
            }
            for (int i = 0; i < M2Share.Config.GlobalAVal.Length; i++)
            {
                string sLoadString = ReadString("String", "GlobalStrVal" + i, "");
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