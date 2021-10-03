using System.IO;
using SystemModule;
using SystemModule.Common;

namespace GameSvr.Configs
{
    public class ExpsConfig
    {
        private readonly IniFile _conf;

        public ExpsConfig()
        { 
            _conf = new IniFile(Path.Combine(M2Share.sConfigPath, M2Share.sExpConfigFileName));
            _conf.Load();
        }

        public void LoadConfig()
        {
            string LoadString;
            int LoadInteger = _conf.ReadInteger("Exp", "LimitExpLevel", -1);
            if (LoadInteger < 0)
            {
                _conf.WriteInteger("Exp", "LimitExpLevel", M2Share.g_Config.nLimitExpLevel);
            }
            else
            {
                M2Share.g_Config.nLimitExpLevel = LoadInteger;
            }
            LoadInteger = _conf.ReadInteger("Exp", "LimitExpValue", -1);
            if (LoadInteger < 0)
            {
                _conf.WriteInteger("Exp", "LimitExpValue", M2Share.g_Config.nLimitExpValue);
            }
            else
            {
                M2Share.g_Config.nLimitExpValue = LoadInteger;
            }
            LoadInteger = _conf.ReadInteger("Exp", "KillMonExpMultiple", -1);
            if (LoadInteger < 0)
            {
                _conf.WriteInteger("Exp", "KillMonExpMultiple", M2Share.g_Config.dwKillMonExpMultiple);
            }
            else
            {
                M2Share.g_Config.dwKillMonExpMultiple = _conf.ReadInteger("Exp", "KillMonExpMultiple", M2Share.g_Config.dwKillMonExpMultiple);
            }
            LoadInteger = _conf.ReadInteger("Exp", "HighLevelKillMonFixExp", -1);
            if (LoadInteger < 0)
            {
                _conf.WriteBool("Exp", "HighLevelKillMonFixExp", M2Share.g_Config.boHighLevelKillMonFixExp);
            }
            else
            {
                M2Share.g_Config.boHighLevelKillMonFixExp = _conf.ReadBool("Exp", "HighLevelKillMonFixExp", M2Share.g_Config.boHighLevelKillMonFixExp);
            }
            if (_conf.ReadInteger("Exp", "HighLevelGroupFixExp", -1) < 0)
            {
                _conf.WriteBool("Exp", "HighLevelGroupFixExp", M2Share.g_Config.boHighLevelGroupFixExp);
            }
            M2Share.g_Config.boHighLevelGroupFixExp = _conf.ReadBool("Exp", "HighLevelGroupFixExp", M2Share.g_Config.boHighLevelGroupFixExp);
            for (var i = 1; i <= M2Share.g_Config.dwNeedExps.GetUpperBound(0); i++)
            {
                LoadString = _conf.ReadString("Exp", "Level" + i, "");
                LoadInteger = HUtil32.Str_ToInt(LoadString, 0);
                if (LoadInteger == 0)
                {
                    var oldNeedExp = M2Share.g_dwOldNeedExps[i];
                    if (oldNeedExp <= 0)
                    {
                        oldNeedExp = _conf.ReadInteger("Exp", "Level" + i, 0);
                        _conf.WriteString("Exp", "Level" + i, oldNeedExp);
                        M2Share.g_Config.dwNeedExps[i - 1] = oldNeedExp;
                    }
                    else
                    {
                        _conf.WriteString("Exp", "Level" + i, oldNeedExp);
                        M2Share.g_Config.dwNeedExps[i - 1] = oldNeedExp;
                    }
                }
                else
                {
                    M2Share.g_Config.dwNeedExps[i - 1] = LoadInteger;
                }
            }
            LoadInteger = _conf.ReadInteger("Exp", "UseFixExp", -1);
            if (LoadInteger < 0)
            {
                _conf.WriteBool("Exp", "UseFixExp", M2Share.g_Config.boUseFixExp);
            }
            else
            {
                M2Share.g_Config.boUseFixExp = _conf.ReadBool("Exp", "UseFixExp", M2Share.g_Config.boUseFixExp);
            }
            LoadInteger = _conf.ReadInteger("Exp", "MonDelHptoExp", -1);
            if (LoadInteger < 0)
            {
                _conf.WriteBool("Exp", "MonDelHptoExp", M2Share.g_Config.boMonDelHptoExp);
            }
            else
            {
                M2Share.g_Config.boMonDelHptoExp = _conf.ReadBool("Exp", "MonDelHptoExp", M2Share.g_Config.boMonDelHptoExp);
            }
            LoadInteger = _conf.ReadInteger("Exp", "BaseExp", -1);
            if (LoadInteger < 0)
            {
                _conf.WriteInteger("Exp", "BaseExp", M2Share.g_Config.nBaseExp);
            }
            else
            {
                M2Share.g_Config.nBaseExp = LoadInteger;
            }
            LoadInteger = _conf.ReadInteger("Exp", "AddExp", -1);
            if (LoadInteger < 0)
            {
                _conf.WriteInteger("Exp", "AddExp", M2Share.g_Config.nAddExp);
            }
            else
            {
                M2Share.g_Config.nAddExp = LoadInteger;
            }
            LoadInteger = _conf.ReadInteger("Exp", "MonHptoExpLevel", -1);
            if (LoadInteger < 0)
            {
                _conf.WriteInteger("Exp", "MonHptoExpLevel", M2Share.g_Config.MonHptoExpLevel);
            }
            else
            {
                M2Share.g_Config.MonHptoExpLevel = LoadInteger;
            }
            LoadInteger = _conf.ReadInteger("Exp", "MonHptoExpmax", -1);
            if (LoadInteger < 0)
            {
                _conf.WriteInteger("Exp", "MonHptoExpmax", M2Share.g_Config.MonHptoExpmax);
            }
            else
            {
                M2Share.g_Config.MonHptoExpmax = LoadInteger;
            }
        }
    }
}
