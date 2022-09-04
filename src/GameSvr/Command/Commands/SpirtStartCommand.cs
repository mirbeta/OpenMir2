using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 此命令用于开始祈祷生效宝宝叛变
    /// </summary>
    [GameCommand("SpirtStart", "此命令用于开始祈祷生效宝宝叛变", 10)]
    public class SpirtStartCommand : BaseCommond
    {
        [DefaultCommand]
        public void SpirtStart(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sParam1 = @Params.Length > 0 ? @Params[0] : "";
            var nTime = HUtil32.Str_ToInt(sParam1, -1);
            var dwTime = 0;
            if (nTime > 0)
            {
                dwTime = nTime * 1000;
            }
            else
            {
                dwTime = M2Share.Config.dwSpiritMutinyTime;
            }
            M2Share.g_dwSpiritMutinyTick = HUtil32.GetTickCount() + dwTime;
            PlayObject.SysMsg("祈祷叛变已开始。持续时长 " + dwTime / 1000 + " 秒。", MsgColor.Green, MsgType.Hint);
        }
    }
}