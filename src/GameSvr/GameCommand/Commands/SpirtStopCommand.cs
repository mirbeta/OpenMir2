using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 此命令用于停止祈祷生效导致宝宝叛变
    /// </summary>
    [Command("SpirtStop", "此命令用于停止祈祷生效导致宝宝叛变", 10)]
    public class SpirtStopCommand : Command
    {
        [ExecuteCommand]
        public static void SpirtStop(PlayObject PlayObject)
        {
            M2Share.g_dwSpiritMutinyTick = 0;
            PlayObject.SysMsg("祈祷叛变已停止。", MsgColor.Green, MsgType.Hint);
        }
    }
}