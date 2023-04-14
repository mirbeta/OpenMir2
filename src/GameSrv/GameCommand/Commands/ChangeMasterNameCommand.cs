using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整指定玩家师傅名称
    /// </summary>
    [Command("ChangeMasterName", "调整指定玩家师傅名称", "人物名称 师徒名称(如果为 无 则清除)", 10)]
    public class ChangeMasterNameCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            string sMasterName = @params.Length > 1 ? @params[1] : "";
            string sIsMaster = @params.Length > 2 ? @params[2] : "";
            if (string.IsNullOrEmpty(sHumanName) || string.IsNullOrEmpty(sMasterName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject != null) {
                if (string.Compare(sMasterName, "无", StringComparison.OrdinalIgnoreCase) == 0) {
                    mPlayObject.MasterName = "";
                    mPlayObject.RefShowName();
                    mPlayObject.IsMaster = false;
                    playObject.SysMsg(sHumanName + " 的师徒名清除成功。", MsgColor.Green, MsgType.Hint);
                }
                else {
                    mPlayObject.MasterName = sMasterName;
                    if (!string.IsNullOrEmpty(sIsMaster) && sIsMaster[0] == '1') {
                        mPlayObject.IsMaster = true;
                    }
                    else {
                        mPlayObject.IsMaster = false;
                    }
                    mPlayObject.RefShowName();
                    playObject.SysMsg(sHumanName + " 的师徒名更改成功。", MsgColor.Green, MsgType.Hint);
                }
            }
            else {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}