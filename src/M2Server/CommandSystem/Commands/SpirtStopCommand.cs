using SystemModule;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 此命令用于停止祈祷生效导致宝宝叛变
    /// </summary>
    [GameCommand("SpirtStop", "此命令用于停止祈祷生效导致宝宝叛变", 10)]
    public class SpirtStopCommand : BaseCommond
    {
        [DefaultCommand]
        public void SpirtStop(string[] @Params, TPlayObject PlayObject)
        {
            var sParam1 = @Params.Length > 0 ? @Params[0] : "";
            if (PlayObject.m_btPermission < 6)
            {
                return;
            }
            if (sParam1 != "" && sParam1[1] == '?')
            {
                PlayObject.SysMsg("此命令用于停止祈祷生效导致宝宝叛变。", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            M2Share.g_dwSpiritMutinyTick = 0;
            PlayObject.SysMsg("祈祷叛变已停止。", TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}