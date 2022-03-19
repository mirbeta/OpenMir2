using GameSvr.CommandSystem;
using SystemModule;

namespace GameSvr
{
    /// <summary>
    /// 此命令用于停止祈祷生效导致宝宝叛变
    /// </summary>
    [GameCommand("SpirtStop", "此命令用于停止祈祷生效导致宝宝叛变", 10)]
    public class SpirtStopCommand : BaseCommond
    {
        [DefaultCommand]
        public void SpirtStop(TPlayObject PlayObject)
        {
            M2Share.g_dwSpiritMutinyTick = 0;
            PlayObject.SysMsg("祈祷叛变已停止。", MsgColor.Green, MsgType.Hint);
        }
    }
}