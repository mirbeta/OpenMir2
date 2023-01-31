using SystemModule.Common;

namespace GameSvr.Conf
{
    public class ExpsConf : IniFile
    {
        public ExpsConf(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadConfig()
        {
            string LoadString;
            int LoadInteger = ReadInteger("Exp", "LimitExpLevel", -1);
            if (LoadInteger < 0)
            {
                WriteInteger("Exp", "LimitExpLevel", M2Share.Config.LimitExpLevel);
            }
            else
            {
                M2Share.Config.LimitExpLevel = LoadInteger;
            }
            LoadInteger = ReadInteger("Exp", "LimitExpValue", -1);
            if (LoadInteger < 0)
            {
                WriteInteger("Exp", "LimitExpValue", M2Share.Config.LimitExpValue);
            }
            else
            {
                M2Share.Config.LimitExpValue = LoadInteger;
            }
            LoadInteger = ReadInteger("Exp", "KillMonExpMultiple", -1);
            if (LoadInteger < 0)
            {
                WriteInteger("Exp", "KillMonExpMultiple", M2Share.Config.KillMonExpMultiple);
            }
            else
            {
                M2Share.Config.KillMonExpMultiple = ReadInteger("Exp", "KillMonExpMultiple", M2Share.Config.KillMonExpMultiple);
            }
            LoadInteger = ReadInteger("Exp", "HighLevelKillMonFixExp", -1);
            if (LoadInteger < 0)
            {
                WriteBool("Exp", "HighLevelKillMonFixExp", M2Share.Config.HighLevelKillMonFixExp);
            }
            else
            {
                M2Share.Config.HighLevelKillMonFixExp = ReadBool("Exp", "HighLevelKillMonFixExp", M2Share.Config.HighLevelKillMonFixExp);
            }
            if (ReadInteger("Exp", "HighLevelGroupFixExp", -1) < 0)
            {
                WriteBool("Exp", "HighLevelGroupFixExp", M2Share.Config.HighLevelGroupFixExp);
            }
            M2Share.Config.HighLevelGroupFixExp = ReadBool("Exp", "HighLevelGroupFixExp", M2Share.Config.HighLevelGroupFixExp);
            for (var i = 0; i < M2Share.Config.NeedExps.Length; i++)
            {
                LoadString = ReadString("Exp", "Level" + i, "");
                LoadInteger = HUtil32.StrToInt(LoadString, 0);
                if (LoadInteger == 0)
                {
                    var oldNeedExp = M2Share.OldNeedExps[i];
                    if (oldNeedExp <= 0)
                    {
                        oldNeedExp = ReadInteger("Exp", "Level" + i, 0);
                        WriteString("Exp", "Level" + i, oldNeedExp);
                        M2Share.Config.NeedExps[i] = oldNeedExp;
                    }
                    else
                    {
                        WriteString("Exp", "Level" + i, oldNeedExp);
                        M2Share.Config.NeedExps[i] = oldNeedExp;
                    }
                }
                else
                {
                    M2Share.Config.NeedExps[i] = LoadInteger;
                }
            }
            LoadInteger = ReadInteger("Exp", "UseFixExp", -1);
            if (LoadInteger < 0)
            {
                WriteBool("Exp", "UseFixExp", M2Share.Config.UseFixExp);
            }
            else
            {
                M2Share.Config.UseFixExp = ReadBool("Exp", "UseFixExp", M2Share.Config.UseFixExp);
            }
            LoadInteger = ReadInteger("Exp", "MonDelHptoExp", -1);
            if (LoadInteger < 0)
            {
                WriteBool("Exp", "MonDelHptoExp", M2Share.Config.MonDelHptoExp);
            }
            else
            {
                M2Share.Config.MonDelHptoExp = ReadBool("Exp", "MonDelHptoExp", M2Share.Config.MonDelHptoExp);
            }
            LoadInteger = ReadInteger("Exp", "BaseExp", -1);
            if (LoadInteger < 0)
            {
                WriteInteger("Exp", "BaseExp", M2Share.Config.BaseExp);
            }
            else
            {
                M2Share.Config.BaseExp = LoadInteger;
            }
            LoadInteger = ReadInteger("Exp", "AddExp", -1);
            if (LoadInteger < 0)
            {
                WriteInteger("Exp", "AddExp", M2Share.Config.AddExp);
            }
            else
            {
                M2Share.Config.AddExp = LoadInteger;
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
