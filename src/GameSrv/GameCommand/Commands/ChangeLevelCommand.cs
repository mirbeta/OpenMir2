using GameSrv.Player;
using M2Server;

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
            var sParam1 = @params.Length > 0 ? @params[0] : "";
            var nLevel = HUtil32.StrToInt(sParam1, 1);
            int nOLevel = playObject.Abil.Level;
            playObject.Abil.Level = (byte)HUtil32._MIN(Settings.MAXUPLEVEL, nLevel);
            playObject.HasLevelUp(1);// 等级调整记录日志
            GameShare.EventSource.AddEventLog(17, playObject.MapName + "\09" + playObject.CurrX + "\09" + playObject.CurrY
                                                + "\09" + playObject.ChrName + "\09" + playObject.Abil.Level + "\09" + "0" + "\09" + "=(" + nLevel + ")" + "\09" + "0");
            if (GameShare.Config.ShowMakeItemMsg) {
                GameShare.Logger.Warn(string.Format(CommandHelp.GameCommandLevelConsoleMsg, playObject.ChrName, nOLevel, playObject.Abil.Level));
            }
        }
    }
}