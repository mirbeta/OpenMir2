using GameSrv.Event.Events;
using GameSrv.Maps;
using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整安全去光环
    /// MOBFIREBURN  3 329 329 3 60 0
    /// </summary>
    [Command("MobFireBurn", "调整安全去光环", 10)]
    public class MobFireBurnCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sMap = @params.Length > 0 ? @params[0] : "";//地图号
            string sX = @params.Length > 1 ? @params[1] : "";//坐标X
            string sY = @params.Length > 2 ? @params[2] : "";//坐标Y
            string sType = @params.Length > 3 ? @params[3] : "";//光环效果
            string sTime = @params.Length > 4 ? @params[4] : "";//持续时间
            string sPoint = @params.Length > 5 ? @params[5] : "";//未知
            if (string.IsNullOrEmpty(sMap) || sMap != "" && sMap[1] == '?') {
                playObject.SysMsg(string.Format(CommandHelp.GameCommandMobFireBurnHelpMsg, this.Command.Name, sMap, sX, sY, sType, sTime, sPoint), MsgColor.Red, MsgType.Hint);
                return;
            }
            int nX = HUtil32.StrToInt(sX, -1);
            int nY = HUtil32.StrToInt(sY, -1);
            int nType = HUtil32.StrToInt(sType, -1);
            int nTime = HUtil32.StrToInt(sTime, -1);
            int nPoint = HUtil32.StrToInt(sPoint, -1);
            if (nPoint < 0) {
                nPoint = 1;
            }
            if (string.IsNullOrEmpty(sMap ) || nX < 0 || nY < 0 || nType < 0 || nTime < 0 || nPoint < 0) {
                playObject.SysMsg(string.Format(CommandHelp.GameCommandMobFireBurnHelpMsg, this.Command.Name, sMap, sX, sY,
                    sType, sTime, sPoint), MsgColor.Red, MsgType.Hint);
                return;
            }
            Envirnoment envir = M2Share.MapMgr.FindMap(sMap);
            if (envir != null) {
                Envirnoment oldEnvir = playObject.Envir;
                playObject.Envir = envir;
                FireBurnEvent fireBurnEvent = new FireBurnEvent(playObject, (short)nX, (short)nY, (byte)nType, nTime * 1000, nPoint);
                M2Share.EventMgr.AddEvent(fireBurnEvent);
                playObject.Envir = oldEnvir;
                return;
            }
            playObject.SysMsg(string.Format(CommandHelp.GameCommandMobFireBurnMapNotFountMsg, this.Command.Name, sMap), MsgColor.Red, MsgType.Hint);
        }
    }
}