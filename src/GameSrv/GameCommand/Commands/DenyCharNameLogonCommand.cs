using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 将指定玩家添加到禁止人物列表
    /// </summary>
    [Command("DenyChrNameLogon", "将指定玩家添加到禁止人物列表", "人物名称 是否永久封(0,1)", 10)]
    public class DenyChrNameLogonCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sChrName = @params.Length > 0 ? @params[0] : "";
            string sFixDeny = @params.Length > 1 ? @params[1] : "";
            if (string.IsNullOrEmpty(sChrName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            try {
                if (!string.IsNullOrEmpty(sFixDeny) && sFixDeny[0] == '1') {
                    //Settings.g_DenyChrNameList.Add(sChrName, ((1) as Object));
                    M2Share.SaveDenyChrNameList();
                    playObject.SysMsg(sChrName + "已加入禁止人物列表", MsgColor.Green, MsgType.Hint);
                }
                else {
                    //Settings.g_DenyChrNameList.Add(sChrName, ((0) as Object));
                    playObject.SysMsg(sChrName + "已加入临时禁止人物列表", MsgColor.Green, MsgType.Hint);
                }
            }
            finally {
            }
        }
    }
}