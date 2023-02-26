using SystemModule.Common;

namespace GameSrv.Conf {
    public class ExpsConf : ConfigFile {
        public ExpsConf(string fileName) : base(fileName) {
            Load();
        }

        public void LoadConfig() {
            M2Share.Config.LimitExpLevel = ReadWriteInteger("Exp", "LimitExpLevel", M2Share.Config.LimitExpLevel);
            M2Share.Config.LimitExpValue = ReadWriteInteger("Exp", "LimitExpValue", M2Share.Config.LimitExpValue);
            M2Share.Config.KillMonExpMultiple = ReadWriteInteger("Exp", "KillMonExpMultiple", M2Share.Config.KillMonExpMultiple);
            M2Share.Config.HighLevelKillMonFixExp = ReadWriteBool("Exp", "HighLevelKillMonFixExp", M2Share.Config.HighLevelKillMonFixExp);
            M2Share.Config.HighLevelGroupFixExp = ReadWriteBool("Exp", "HighLevelGroupFixExp", M2Share.Config.HighLevelGroupFixExp);

            for (int i = 0; i < M2Share.Config.NeedExps.Length; i++) {
                string LoadString = ReadWriteString("Exp", "Level" + i, "");
                int LoadInteger = HUtil32.StrToInt(LoadString, 0);
                if (LoadInteger == 0) {
                    int oldNeedExp = M2Share.OldNeedExps[i];
                    if (oldNeedExp <= 0) {
                        oldNeedExp = ReadWriteInteger("Exp", "Level" + i, 0);
                        WriteString("Exp", "Level" + i, oldNeedExp);
                        M2Share.Config.NeedExps[i] = oldNeedExp;
                    }
                    else {
                        WriteString("Exp", "Level" + i, oldNeedExp);
                        M2Share.Config.NeedExps[i] = oldNeedExp;
                    }
                }
                else {
                    M2Share.Config.NeedExps[i] = LoadInteger;
                }
            }

            M2Share.Config.UseFixExp = ReadWriteBool("Exp", "UseFixExp", M2Share.Config.UseFixExp);
            M2Share.Config.MonDelHptoExp = ReadWriteBool("Exp", "MonDelHptoExp", M2Share.Config.MonDelHptoExp);
            M2Share.Config.BaseExp = ReadWriteInteger("Exp", "BaseExp", M2Share.Config.BaseExp);
            M2Share.Config.AddExp = ReadWriteInteger("Exp", "AddExp", M2Share.Config.AddExp);
            M2Share.Config.MonHptoExpLevel = ReadWriteInteger("Exp", "MonHptoExpLevel", M2Share.Config.MonHptoExpLevel);
            M2Share.Config.MonHptoExpmax = ReadWriteInteger("Exp", "MonHptoExpmax", M2Share.Config.MonHptoExpmax);
        }
    }
}