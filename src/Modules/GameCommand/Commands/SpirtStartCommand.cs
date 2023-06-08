using M2Server.Player;
using SystemModule;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 此命令用于开始祈祷生效宝宝叛变
    /// </summary>
    [Command("SpirtStart", "此命令用于开始祈祷生效宝宝叛变", 10)]
    public class SpirtStartCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sParam1 = @params.Length > 0 ? @params[0] : "";
            var nTime = HUtil32.StrToInt(sParam1, -1);
            int dwTime;
            if (nTime > 0) {
                dwTime = nTime * 1000;
            }
            else {
                dwTime = M2Share.Config.SpiritMutinyTime;
            }
            M2Share.SpiritMutinyTick = HUtil32.GetTickCount() + dwTime;
            playObject.SysMsg("祈祷叛变已开始。持续时长 " + dwTime / 1000 + " 秒。", MsgColor.Green, MsgType.Hint);
        }
    }
}