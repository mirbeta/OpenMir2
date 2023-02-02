using SystemModule.Common;

namespace GameSvr.Conf
{
    public class ExpsConf : ConfigFile
    {
        public ExpsConf(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadConfig()
        {
            string LoadString;
            int LoadInteger = ReadWriteInteger("Exp", "LimitExpLevel", -1);
            if (LoadInteger < 0)
            {
                WriteInteger("Exp", "LimitExpLevel", M2Share.Config.LimitExpLevel);
            }
            else
            {
                M2Share.Config.LimitExpLevel = LoadInteger;
            }
            LoadInteger = ReadWriteInteger("Exp", "LimitExpValue", -1);
            if (LoadInteger < 0)
            {
                WriteInteger("Exp", "LimitExpValue", M2Share.Config.LimitExpValue);
            }
            else
            {
                M2Share.Config.LimitExpValue = LoadInteger;
            }
            LoadInteger = ReadWriteInteger("Exp", "KillMonExpMultiple", -1);
            if (LoadInteger < 0)
            {
                WriteInteger("Exp", "KillMonExpMultiple", M2Share.Config.KillMonExpMultiple);
            }
            else
            {
                M2Share.Config.KillMonExpMultiple = ReadWriteInteger("Exp", "KillMonExpMultiple", M2Share.Config.KillMonExpMultiple);
            }
            LoadInteger = ReadWriteInteger("Exp", "HighLevelKillMonFixExp", -1);
            if (LoadInteger < 0)
            {
                WriteBool("Exp", "HighLevelKillMonFixExp", M2Share.Config.HighLevelKillMonFixExp);
            }
            else
            {
                M2Share.Config.HighLevelKillMonFixExp = ReadWriteBool("Exp", "HighLevelKillMonFixExp", M2Share.Config.HighLevelKillMonFixExp);
            }
            if (ReadWriteInteger("Exp", "HighLevelGroupFixExp", -1) < 0)
            {
                WriteBool("Exp", "HighLevelGroupFixExp", M2Share.Config.HighLevelGroupFixExp);
            }
            M2Share.Config.HighLevelGroupFixExp = ReadWriteBool("Exp", "HighLevelGroupFixExp", M2Share.Config.HighLevelGroupFixExp);
            for (int i = 0; i < M2Share.Config.NeedExps.Length; i++)
            {
                LoadString = ReadWriteString("Exp", "Level" + i, "");
                LoadInteger = HUtil32.StrToInt(LoadString, 0);
                if (LoadInteger == 0)
                {
                    int oldNeedExp = M2Share.OldNeedExps[i];
                    if (oldNeedExp <= 0)
                    {
                        oldNeedExp = ReadWriteInteger("Exp", "Level" + i, 0);
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
            LoadInteger = ReadWriteInteger("Exp", "UseFixExp", -1);
            if (LoadInteger < 0)
            {
                WriteBool("Exp", "UseFixExp", M2Share.Config.UseFixExp);
            }
            else
            {
                M2Share.Config.UseFixExp = ReadWriteBool("Exp", "UseFixExp", M2Share.Config.UseFixExp);
            }
            LoadInteger = ReadWriteInteger("Exp", "MonDelHptoExp", -1);
            if (LoadInteger < 0)
            {
                WriteBool("Exp", "MonDelHptoExp", M2Share.Config.MonDelHptoExp);
            }
            else
            {
                M2Share.Config.MonDelHptoExp = ReadWriteBool("Exp", "MonDelHptoExp", M2Share.Config.MonDelHptoExp);
            }
            LoadInteger = ReadWriteInteger("Exp", "BaseExp", -1);
            if (LoadInteger < 0)
            {
                WriteInteger("Exp", "BaseExp", M2Share.Config.BaseExp);
            }
            else
            {
                M2Share.Config.BaseExp = LoadInteger;
            }
            LoadInteger = ReadWriteInteger("Exp", "AddExp", -1);
            if (LoadInteger < 0)
            {
                WriteInteger("Exp", "AddExp", M2Share.Config.AddExp);
            }
            else
            {
                M2Share.Config.AddExp = LoadInteger;
            }
            LoadInteger = ReadWriteInteger("Exp", "MonHptoExpLevel", -1);
            if (LoadInteger < 0)
            {
                WriteInteger("Exp", "MonHptoExpLevel", M2Share.Config.MonHptoExpLevel);
            }
            else
            {
                M2Share.Config.MonHptoExpLevel = LoadInteger;
            }
            LoadInteger = ReadWriteInteger("Exp", "MonHptoExpmax", -1);
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
