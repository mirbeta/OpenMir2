using OpenMir2;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 此命令用于开始祈祷生效宝宝叛变
    /// </summary>
    [Command("SpirtStart", "此命令用于开始祈祷生效宝宝叛变", 10)]
    public class SpirtStartCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            string sParam1 = @params.Length > 0 ? @params[0] : "";
            int nTime = HUtil32.StrToInt(sParam1, -1);
            int dwTime;
            if (nTime > 0)
            {
                dwTime = nTime * 1000;
            }
            else
            {
                dwTime = SystemShare.Config.SpiritMutinyTime;
            }
            SystemShare.SpiritMutinyTick = HUtil32.GetTickCount() + dwTime;
            PlayerActor.SysMsg("祈祷叛变已开始。持续时长 " + dwTime / 1000 + " 秒。", MsgColor.Green, MsgType.Hint);
        }
    }
}