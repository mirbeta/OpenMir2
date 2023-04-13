using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整指定玩家等级
    /// </summary>
    [Command("AdjuestLevel", "调整指定玩家等级", "人物名称 等级", 10)]
    public class AdjuestLevelCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            int nLevel = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0;
            if (string.IsNullOrEmpty(sHumanName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject != null) {
                int nOLevel = mPlayObject.Abil.Level;
                mPlayObject.HasLevelUp(1);
                M2Share.EventSource.AddEventLog(17, mPlayObject.MapName + "\09" + mPlayObject.CurrX + "\09" + mPlayObject.CurrY + "\09"
                                                    + mPlayObject.ChrName + "\09" + mPlayObject.Abil.Level + "\09" + playObject.ChrName + "\09" + "+(" + nLevel + ")" + "\09" + "0");
                playObject.SysMsg(sHumanName + " 等级调整完成。", MsgColor.Green, MsgType.Hint);
                if (M2Share.Config.ShowMakeItemMsg) {
                    M2Share.Logger.Warn("[等级调整] " + playObject.ChrName + "(" + mPlayObject.ChrName + " " + nOLevel + " -> " + mPlayObject.Abil.Level + ")");
                }
            }
            else
            {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}