using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整指定玩家经验值
    /// </summary>
    [Command("AdjuestExp", "调整指定人物的经验值", "物名称 经验值", 10)]
    public class AdjuestExpCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            string sExp = @params.Length > 1 ? @params[1] : "";
            if (string.IsNullOrEmpty(sHumanName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            int dwExp = HUtil32.StrToInt(sExp, 0);
            PlayObject mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject != null) {
                int dwOExp = playObject.Abil.Exp;
                mPlayObject.Abil.Exp = dwExp;
                mPlayObject.HasLevelUp(mPlayObject.Abil.Level - 1);
                playObject.SysMsg(sHumanName + " 经验调整完成。", MsgColor.Green, MsgType.Hint);
                if (M2Share.Config.ShowMakeItemMsg) {
                    M2Share.Logger.Warn("[经验调整] " + playObject.ChrName + '(' + mPlayObject.ChrName + ' ' + dwOExp + " -> " + mPlayObject.Abil.Exp + ')');
                }
            }
            else {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}