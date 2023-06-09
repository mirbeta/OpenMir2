using SystemModule.Common;

namespace SystemModule
{
    public class ExpsConf : ConfigFile
    {
        public ExpsConf(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadConfig()
        {
            ModuleShare.Config.LimitExpLevel = ReadWriteInteger("Exp", "LimitExpLevel", ModuleShare.Config.LimitExpLevel);
            ModuleShare.Config.LimitExpValue = ReadWriteInteger("Exp", "LimitExpValue", ModuleShare.Config.LimitExpValue);
            ModuleShare.Config.KillMonExpMultiple = ReadWriteInteger("Exp", "KillMonExpMultiple", ModuleShare.Config.KillMonExpMultiple);
            ModuleShare.Config.HighLevelKillMonFixExp = ReadWriteBool("Exp", "HighLevelKillMonFixExp", ModuleShare.Config.HighLevelKillMonFixExp);
            ModuleShare.Config.HighLevelGroupFixExp = ReadWriteBool("Exp", "HighLevelGroupFixExp", ModuleShare.Config.HighLevelGroupFixExp);

            for (int i = 0; i < ModuleShare.Config.NeedExps.Length; i++)
            {
                string LoadString = ReadWriteString("Exp", "Level" + i, "");
                int LoadInteger = HUtil32.StrToInt(LoadString, 0);
                if (LoadInteger == 0)
                {
                    int oldNeedExp = ModuleShare.OldNeedExps[i];
                    if (oldNeedExp <= 0)
                    {
                        oldNeedExp = ReadWriteInteger("Exp", "Level" + i, 0);
                        WriteString("Exp", "Level" + i, oldNeedExp);
                        ModuleShare.Config.NeedExps[i] = oldNeedExp;
                    }
                    else
                    {
                        WriteString("Exp", "Level" + i, oldNeedExp);
                        ModuleShare.Config.NeedExps[i] = oldNeedExp;
                    }
                }
                else
                {
                    ModuleShare.Config.NeedExps[i] = LoadInteger;
                }
            }

            ModuleShare.Config.UseFixExp = ReadWriteBool("Exp", "UseFixExp", ModuleShare.Config.UseFixExp);
            ModuleShare.Config.MonDelHptoExp = ReadWriteBool("Exp", "MonDelHptoExp", ModuleShare.Config.MonDelHptoExp);
            ModuleShare.Config.BaseExp = ReadWriteInteger("Exp", "BaseExp", ModuleShare.Config.BaseExp);
            ModuleShare.Config.AddExp = ReadWriteInteger("Exp", "AddExp", ModuleShare.Config.AddExp);
            ModuleShare.Config.MonHptoExpLevel = ReadWriteInteger("Exp", "MonHptoExpLevel", ModuleShare.Config.MonHptoExpLevel);
            ModuleShare.Config.MonHptoExpmax = ReadWriteInteger("Exp", "MonHptoExpmax", ModuleShare.Config.MonHptoExpmax);
        }
    }
}