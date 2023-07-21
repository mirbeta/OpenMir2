using GameSrv.Player;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整自己的等级
    /// </summary>
    [Command("ChangeLevel", "调整自己的等级", "等级(1-65535)", 10)]
    public class ChangeLevelCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sParam1 = @params.Length > 0 ? @params[0] : "";
            int nLevel = HUtil32.StrToInt(sParam1, 1);
            int nOLevel = playObject.Abil.Level;
            playObject.Abil.Level = (byte)HUtil32._MIN(Settings.MAXUPLEVEL, nLevel);
            playObject.HasLevelUp(1);// 等级调整记录日志
            M2Share.EventSource.AddEventLog(17, playObject.MapName + "\t" + playObject.CurrX + "\t" + playObject.CurrY
                                                + "\t" + playObject.ChrName + "\t" + playObject.Abil.Level + "\t" + "0" + "\t" + "=(" + nLevel + ")" + "\t" + "0");
            if (M2Share.Config.ShowMakeItemMsg) {
                M2Share.Logger.Warn(string.Format(CommandHelp.GameCommandLevelConsoleMsg, playObject.ChrName, nOLevel, playObject.Abil.Level));
            }
        }
    }
}