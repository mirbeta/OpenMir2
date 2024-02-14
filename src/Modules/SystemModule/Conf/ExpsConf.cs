using OpenMir2;
using OpenMir2.Common;

namespace SystemModule.Conf
{
    public class ExpsConf : ConfigFile
    {
        public ExpsConf(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadConfig()
        {
            SystemShare.Config.LimitExpLevel = ReadWriteInteger("Exp", "LimitExpLevel", SystemShare.Config.LimitExpLevel);
            SystemShare.Config.LimitExpValue = ReadWriteInteger("Exp", "LimitExpValue", SystemShare.Config.LimitExpValue);
            SystemShare.Config.KillMonExpMultiple = ReadWriteInteger("Exp", "KillMonExpMultiple", SystemShare.Config.KillMonExpMultiple);
            SystemShare.Config.HighLevelKillMonFixExp = ReadWriteBool("Exp", "HighLevelKillMonFixExp", SystemShare.Config.HighLevelKillMonFixExp);
            SystemShare.Config.HighLevelGroupFixExp = ReadWriteBool("Exp", "HighLevelGroupFixExp", SystemShare.Config.HighLevelGroupFixExp);

            for (int i = 0; i < SystemShare.Config.NeedExps.Length; i++)
            {
                string LoadString = ReadWriteString("Exp", "Level" + i, "");
                int LoadInteger = HUtil32.StrToInt(LoadString, 0);
                if (LoadInteger == 0)
                {
                    int oldNeedExp = SystemShare.OldNeedExps[i];
                    if (oldNeedExp <= 0)
                    {
                        oldNeedExp = ReadWriteInteger("Exp", "Level" + i, 0);
                        WriteString("Exp", "Level" + i, oldNeedExp);
                        SystemShare.Config.NeedExps[i] = oldNeedExp;
                    }
                    else
                    {
                        WriteString("Exp", "Level" + i, oldNeedExp);
                        SystemShare.Config.NeedExps[i] = oldNeedExp;
                    }
                }
                else
                {
                    SystemShare.Config.NeedExps[i] = LoadInteger;
                }
            }

            SystemShare.Config.UseFixExp = ReadWriteBool("Exp", "UseFixExp", SystemShare.Config.UseFixExp);
            SystemShare.Config.MonDelHptoExp = ReadWriteBool("Exp", "MonDelHptoExp", SystemShare.Config.MonDelHptoExp);
            SystemShare.Config.BaseExp = ReadWriteInteger("Exp", "BaseExp", SystemShare.Config.BaseExp);
            SystemShare.Config.AddExp = ReadWriteInteger("Exp", "AddExp", SystemShare.Config.AddExp);
            SystemShare.Config.MonHptoExpLevel = ReadWriteInteger("Exp", "MonHptoExpLevel", SystemShare.Config.MonHptoExpLevel);
            SystemShare.Config.MonHptoExpmax = ReadWriteInteger("Exp", "MonHptoExpmax", SystemShare.Config.MonHptoExpmax);
        }
    }
}