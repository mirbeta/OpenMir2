using M2Server.Player;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 此命令用于停止祈祷生效导致宝宝叛变
    /// </summary>
    [Command("SpirtStop", "此命令用于停止祈祷生效导致宝宝叛变", 10)]
    public class SpirtStopCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            M2Share.SpiritMutinyTick = 0;
            playObject.SysMsg("祈祷叛变已停止。", MsgColor.Green, MsgType.Hint);
        }
    }
}