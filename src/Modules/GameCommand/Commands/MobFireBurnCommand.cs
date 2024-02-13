using OpenMir2;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;
using SystemModule.MagicEvent.Events;

namespace CommandModule.Commands
{
    /// <summary>
    /// 调整安全去光环
    /// MOBFIREBURN  3 329 329 3 60 0
    /// </summary>
    [Command("MobFireBurn", "调整安全去光环", 10)]
    public class MobFireBurnCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sMap = @params.Length > 0 ? @params[0] : "";//地图号
            var sX = @params.Length > 1 ? @params[1] : "";//坐标X
            var sY = @params.Length > 2 ? @params[2] : "";//坐标Y
            var sType = @params.Length > 3 ? @params[3] : "";//光环效果
            var sTime = @params.Length > 4 ? @params[4] : "";//持续时间
            var sPoint = @params.Length > 5 ? @params[5] : "";//未知
            if (string.IsNullOrEmpty(sMap) || sMap != "" && sMap[1] == '?')
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandMobFireBurnHelpMsg, this.Command.Name, sMap, sX, sY, sType, sTime, sPoint), MsgColor.Red, MsgType.Hint);
                return;
            }
            var nX = (short)HUtil32.StrToInt(sX, -1);
            var nY = (short)HUtil32.StrToInt(sY, -1);
            var nType = (byte)HUtil32.StrToInt(sType, -1);
            var nTime = HUtil32.StrToInt(sTime, -1);
            var nPoint = HUtil32.StrToInt(sPoint, -1);
            if (nPoint < 0)
            {
                nPoint = 1;
            }
            if (string.IsNullOrEmpty(sMap) || nX < 0 || nY < 0 || nType < 0 || nTime < 0 || nPoint < 0)
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandMobFireBurnHelpMsg, this.Command.Name, sMap, sX, sY,
                    sType, sTime, sPoint), MsgColor.Red, MsgType.Hint);
                return;
            }
            var envir = SystemShare.MapMgr.FindMap(sMap);
            if (envir != null)
            {
                var oldEnvir = PlayerActor.Envir;
                PlayerActor.Envir = envir;
                var fireBurnEvent = new FireBurnEvent(PlayerActor, nX, nY, nType, nTime * 1000, nPoint);
                SystemShare.EventMgr.AddEvent(fireBurnEvent);
                PlayerActor.Envir = oldEnvir;
                return;
            }
            PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandMobFireBurnMapNotFountMsg, this.Command.Name, sMap), MsgColor.Red, MsgType.Hint);
        }
    }
}