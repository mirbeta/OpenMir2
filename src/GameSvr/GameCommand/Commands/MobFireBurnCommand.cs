using GameSvr.Event.Events;
using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 调整安全去光环
    /// MOBFIREBURN  3 329 329 3 60 0
    /// </summary>
    [Command("MobFireBurn", "调整安全去光环", 10)]
    public class MobFireBurnCommand : GameCommand
    {
        [ExecuteCommand]
        public void MobFireBurn(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sMAP = @Params.Length > 0 ? @Params[0] : "";//地图号
            string sX = @Params.Length > 1 ? @Params[1] : "";//坐标X
            string sY = @Params.Length > 2 ? @Params[2] : "";//坐标Y
            string sType = @Params.Length > 3 ? @Params[3] : "";//光环效果
            string sTime = @Params.Length > 4 ? @Params[4] : "";//持续时间
            string sPoint = @Params.Length > 5 ? @Params[5] : "";//未知
            if (sMAP == "" || sMAP != "" && sMAP[1] == '?')
            {
                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandMobFireBurnHelpMsg, this.Command.Name, sMAP, sX, sY, sType, sTime, sPoint), MsgColor.Red, MsgType.Hint);
                return;
            }
            int nX = HUtil32.StrToInt(sX, -1);
            int nY = HUtil32.StrToInt(sY, -1);
            int nType = HUtil32.StrToInt(sType, -1);
            int nTime = HUtil32.StrToInt(sTime, -1);
            int nPoint = HUtil32.StrToInt(sPoint, -1);
            if (nPoint < 0)
            {
                nPoint = 1;
            }
            if (sMAP == "" || nX < 0 || nY < 0 || nType < 0 || nTime < 0 || nPoint < 0)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandMobFireBurnHelpMsg, this.Command.Name, sMAP, sX, sY,
                    sType, sTime, sPoint), MsgColor.Red, MsgType.Hint);
                return;
            }
            Maps.Envirnoment Envir = M2Share.MapMgr.FindMap(sMAP);
            if (Envir != null)
            {
                Maps.Envirnoment OldEnvir = PlayObject.Envir;
                PlayObject.Envir = Envir;
                FireBurnEvent FireBurnEvent = new FireBurnEvent(PlayObject, (short)nX, (short)nY, (byte)nType, nTime * 1000, nPoint);
                M2Share.EventMgr.AddEvent(FireBurnEvent);
                PlayObject.Envir = OldEnvir;
                return;
            }
            PlayObject.SysMsg(string.Format(CommandHelp.GameCommandMobFireBurnMapNotFountMsg, this.Command.Name, sMAP), MsgColor.Red, MsgType.Hint);
        }
    }
}