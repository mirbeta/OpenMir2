using SystemModule;
using mSystemModule;

namespace M2Server.Configs
{
    public class ExpsConfig
    {
        private readonly IniFile ExpConf;
        private readonly IniFile Config;

        public ExpsConfig()
        { 
            ExpConf = new IniFile(M2Share.sExpConfigFileName);
            Config = new IniFile(M2Share.sConfigFileName);
        }

        public void LoadConfig()
        {
            int LoadInteger;
            string LoadString;
            LoadInteger = ExpConf.ReadInteger("Exp", "KillMonExpMultiple", -1);
            if (LoadInteger < 0)
            {
                ExpConf.WriteInteger("Exp", "KillMonExpMultiple", M2Share.g_Config.dwKillMonExpMultiple);
            }
            else
            {
                M2Share.g_Config.dwKillMonExpMultiple = ExpConf.ReadInteger("Exp", "KillMonExpMultiple", M2Share.g_Config.dwKillMonExpMultiple);
            }
            LoadInteger = ExpConf.ReadInteger("Exp", "HighLevelKillMonFixExp", -1);
            if (LoadInteger < 0)
            {
                ExpConf.WriteBool("Exp", "HighLevelKillMonFixExp", M2Share.g_Config.boHighLevelKillMonFixExp);
            }
            else
            {
                M2Share.g_Config.boHighLevelKillMonFixExp = ExpConf.ReadBool("Exp", "HighLevelKillMonFixExp", M2Share.g_Config.boHighLevelKillMonFixExp);
            }
            if (ExpConf.ReadInteger("Exp", "HighLevelGroupFixExp", -1) < 0)
            {
                ExpConf.WriteBool("Exp", "HighLevelGroupFixExp", M2Share.g_Config.boHighLevelGroupFixExp);
            }
            M2Share.g_Config.boHighLevelGroupFixExp = ExpConf.ReadBool("Exp", "HighLevelGroupFixExp", M2Share.g_Config.boHighLevelGroupFixExp);
            for (var i = 1; i <= M2Share.g_Config.dwNeedExps.GetUpperBound(0); i++)
            {
                LoadString = ExpConf.ReadString("Exp", "Level" + i, "");
                LoadInteger = HUtil32.Str_ToInt(LoadString, 0);
                if (LoadInteger == 0)
                {
                    var oldNeedExp = M2Share.g_dwOldNeedExps[i];
                    if (oldNeedExp <= 0)
                    {
                        oldNeedExp = Config.ReadInteger("Exp", "Level" + i, 0);
                        ExpConf.WriteString("Exp", "Level" + i, oldNeedExp);
                        M2Share.g_Config.dwNeedExps[i - 1] = oldNeedExp;
                    }
                    else
                    {
                        ExpConf.WriteString("Exp", "Level" + i, oldNeedExp);
                        M2Share.g_Config.dwNeedExps[i - 1] = oldNeedExp;
                    }
                }
                else
                {
                    M2Share.g_Config.dwNeedExps[i - 1] = LoadInteger;
                }
            }
            LoadInteger = ExpConf.ReadInteger("Exp", "UseFixExp", -1);
            if (LoadInteger < 0)
            {
                ExpConf.WriteBool("Exp", "UseFixExp", M2Share.g_Config.boUseFixExp);
            }
            else
            {
                M2Share.g_Config.boUseFixExp = ExpConf.ReadBool("Exp", "UseFixExp", M2Share.g_Config.boUseFixExp);
            }
            LoadInteger = ExpConf.ReadInteger("Exp", "MonDelHptoExp", -1);
            if (LoadInteger < 0)
            {
                ExpConf.WriteBool("Exp", "MonDelHptoExp", M2Share.g_Config.boMonDelHptoExp);
            }
            else
            {
                M2Share.g_Config.boMonDelHptoExp = ExpConf.ReadBool("Exp", "MonDelHptoExp", M2Share.g_Config.boMonDelHptoExp);
            }
            LoadInteger = ExpConf.ReadInteger("Exp", "BaseExp", -1);
            if (LoadInteger < 0)
            {
                ExpConf.WriteInteger("Exp", "BaseExp", M2Share.g_Config.nBaseExp);
            }
            else
            {
                M2Share.g_Config.nBaseExp = LoadInteger;
            }
            LoadInteger = ExpConf.ReadInteger("Exp", "AddExp", -1);
            if (LoadInteger < 0)
            {
                ExpConf.WriteInteger("Exp", "AddExp", M2Share.g_Config.nAddExp);
            }
            else
            {
                M2Share.g_Config.nAddExp = LoadInteger;
            }
            LoadInteger = ExpConf.ReadInteger("Exp", "MonHptoExpLevel", -1);
            if (LoadInteger < 0)
            {
                ExpConf.WriteInteger("Exp", "MonHptoExpLevel", M2Share.g_Config.MonHptoExpLevel);
            }
            else
            {
                M2Share.g_Config.MonHptoExpLevel = LoadInteger;
            }
            LoadInteger = ExpConf.ReadInteger("Exp", "MonHptoExpmax", -1);
            if (LoadInteger < 0)
            {
                ExpConf.WriteInteger("Exp", "MonHptoExpmax", M2Share.g_Config.MonHptoExpmax);
            }
            else
            {
                M2Share.g_Config.MonHptoExpmax = LoadInteger;
            }
        }
    }
}
