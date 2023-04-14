using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 设置怪物行动速度
    /// </summary>
    [Command("ChangeZenFastStep", "设置怪物行动速度", "速度", 10)]
    public class ChangeZenFastStepCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sFastStep = @params.Length > 0 ? @params[0] : "";
            var nFastStep = HUtil32.StrToInt(sFastStep, -1);
            if (string.IsNullOrEmpty(sFastStep) || nFastStep < 1 || !string.IsNullOrEmpty(sFastStep)) {
                playObject.SysMsg("设置怪物行动速度。", MsgColor.Red, MsgType.Hint);
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            M2Share.Config.ZenFastStep = nFastStep;
            playObject.SysMsg($"怪物行动速度: {nFastStep}", MsgColor.Green, MsgType.Hint);
        }
    }
}