using SystemModule;
using SystemModule.Enums;

namespace CommandSystem {
    /// <summary>
    /// 查看指定玩家所在IP地址
    /// </summary>
    [Command("HumanLocal", "查看指定玩家所在IP地址", CommandHelp.GameCommandHumanLocalHelpMsg, 10)]
    public class HumanLocalCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var mSIpLocal = "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName)) {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mIPlayerActor = ModuleShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor == null) {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            // GetIPLocal(PlayerActor.SysMsgm_sIPaddr)
            PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandHumanLocalMsg, sHumanName, mSIpLocal), MsgColor.Green, MsgType.Hint);
        }
    }
}