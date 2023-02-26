using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 此命令用于停止祈祷生效导致宝宝叛变
    /// </summary>
    [Command("SpirtStop", "此命令用于停止祈祷生效导致宝宝叛变", 10)]
    public class SpirtStopCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject) {
            M2Share.SpiritMutinyTick = 0;
            PlayObject.SysMsg("祈祷叛变已停止。", MsgColor.Green, MsgType.Hint);
        }
    }
}