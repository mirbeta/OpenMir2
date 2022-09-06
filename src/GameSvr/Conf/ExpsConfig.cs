using SystemModule;
using SystemModule.Common;

namespace GameSvr.Conf
{
    public class ExpsConfig : IniFile
    {
        public ExpsConfig(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadConfig()
        {
            string LoadString;
            int LoadInteger = ReadInteger("Exp", "LimitExpLevel", -1);
            if (LoadInteger < 0)
            {
                WriteInteger("Exp", "LimitExpLevel", M2Share.Config.nLimitExpLevel);
            }
            else
            {
                M2Share.Config.nLimitExpLevel = LoadInteger;
            }
            LoadInteger = ReadInteger("Exp", "LimitExpValue", -1);
            if (LoadInteger < 0)
            {
                WriteInteger("Exp", "LimitExpValue", M2Share.Config.nLimitExpValue);
            }
            else
            {
                M2Share.Config.nLimitExpValue = LoadInteger;
            }
            LoadInteger = ReadInteger("Exp", "KillMonExpMultiple", -1);
            if (LoadInteger < 0)
            {
                WriteInteger("Exp", "KillMonExpMultiple", M2Share.Config.dwKillMonExpMultiple);
            }
            else
            {
                M2Share.Config.dwKillMonExpMultiple = ReadInteger("Exp", "KillMonExpMultiple", M2Share.Config.dwKillMonExpMultiple);
            }
            LoadInteger = ReadInteger("Exp", "HighLevelKillMonFixExp", -1);
            if (LoadInteger < 0)
            {
                WriteBool("Exp", "HighLevelKillMonFixExp", M2Share.Config.boHighLevelKillMonFixExp);
            }
            else
            {
                M2Share.Config.boHighLevelKillMonFixExp = ReadBool("Exp", "HighLevelKillMonFixExp", M2Share.Config.boHighLevelKillMonFixExp);
            }
            if (ReadInteger("Exp", "HighLevelGroupFixExp", -1) < 0)
            {
                WriteBool("Exp", "HighLevelGroupFixExp", M2Share.Config.boHighLevelGroupFixExp);
            }
            M2Share.Config.boHighLevelGroupFixExp = ReadBool("Exp", "HighLevelGroupFixExp", M2Share.Config.boHighLevelGroupFixExp);
            for (var i = 0; i < M2Share.Config.dwNeedExps.Length; i++)
            {
                LoadString = ReadString("Exp", "Level" + i, "");
                LoadInteger = HUtil32.Str_ToInt(LoadString, 0);
                if (LoadInteger == 0)
                {
                    var oldNeedExp = M2Share.g_dwOldNeedExps[i];
                    if (oldNeedExp <= 0)
                    {
                        oldNeedExp = ReadInteger("Exp", "Level" + i, 0);
                        WriteString("Exp", "Level" + i, oldNeedExp);
                        M2Share.Config.dwNeedExps[i] = oldNeedExp;
                    }
                    else
                    {
                        WriteString("Exp", "Level" + i, oldNeedExp);
                        M2Share.Config.dwNeedExps[i] = oldNeedExp;
                    }
                }
                else
                {
                    M2Share.Config.dwNeedExps[i] = LoadInteger;
                }
            }
            LoadInteger = ReadInteger("Exp", "UseFixExp", -1);
            if (LoadInteger < 0)
            {
                WriteBool("Exp", "UseFixExp", M2Share.Config.boUseFixExp);
            }
            else
            {
                M2Share.Config.boUseFixExp = ReadBool("Exp", "UseFixExp", M2Share.Config.boUseFixExp);
            }
            LoadInteger = ReadInteger("Exp", "MonDelHptoExp", -1);
            if (LoadInteger < 0)
            {
                WriteBool("Exp", "MonDelHptoExp", M2Share.Config.boMonDelHptoExp);
            }
            else
            {
                M2Share.Config.boMonDelHptoExp = ReadBool("Exp", "MonDelHptoExp", M2Share.Config.boMonDelHptoExp);
            }
            LoadInteger = ReadInteger("Exp", "BaseExp", -1);
            if (LoadInteger < 0)
            {
                WriteInteger("Exp", "BaseExp", M2Share.Config.nBaseExp);
            }
            else
            {
                M2Share.Config.nBaseExp = LoadInteger;
            }
            LoadInteger = ReadInteger("Exp", "AddExp", -1);
            if (LoadInteger < 0)
            {
                WriteInteger("Exp", "AddExp", M2Share.Config.nAddExp);
            }
            else
            {
                M2Share.Config.nAddExp = LoadInteger;
            }
            LoadInteger = ReadInteger("Exp", "MonHptoExpLevel", -1);
            if (LoadInteger < 0)
            {
                WriteInteger("Exp", "MonHptoExpLevel", M2Share.Config.MonHptoExpLevel);
            }
            else
            {
                M2Share.Config.MonHptoExpLevel = LoadInteger;
            }
            LoadInteger = ReadInteger("Exp", "MonHptoExpmax", -1);
            if (LoadInteger < 0)
            {
                WriteInteger("Exp", "MonHptoExpmax", M2Share.Config.MonHptoExpmax);
            }
            else
            {
                M2Share.Config.MonHptoExpmax = LoadInteger;
            }
        }
    }
}
