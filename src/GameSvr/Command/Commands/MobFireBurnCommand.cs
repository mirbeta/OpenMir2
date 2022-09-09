using GameSvr.Event.Events;
using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整安全去光环
    /// MOBFIREBURN  3 329 329 3 60 0
    /// </summary>
    [GameCommand("MobFireBurn", "调整安全去光环", 10)]
    public class MobFireBurnCommand : BaseCommond
    {
        [DefaultCommand]
        public void MobFireBurn(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sMAP = @Params.Length > 0 ? @Params[0] : "";//地图号
            var sX = @Params.Length > 1 ? @Params[1] : "";//坐标X
            var sY = @Params.Length > 2 ? @Params[2] : "";//坐标Y
            var sType = @Params.Length > 3 ? @Params[3] : "";//光环效果
            var sTime = @Params.Length > 4 ? @Params[4] : "";//持续时间
            var sPoint = @Params.Length > 5 ? @Params[5] : "";//未知
            if (sMAP == "" || sMAP != "" && sMAP[1] == '?')
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.GameCommandMobFireBurnHelpMsg, this.GameCommand.Name, sMAP, sX, sY, sType, sTime, sPoint), MsgColor.Red, MsgType.Hint);
                return;
            }
            var nX = HUtil32.Str_ToInt(sX, -1);
            var nY = HUtil32.Str_ToInt(sY, -1);
            var nType = HUtil32.Str_ToInt(sType, -1);
            var nTime = HUtil32.Str_ToInt(sTime, -1);
            var nPoint = HUtil32.Str_ToInt(sPoint, -1);
            if (nPoint < 0)
            {
                nPoint = 1;
            }
            if (sMAP == "" || nX < 0 || nY < 0 || nType < 0 || nTime < 0 || nPoint < 0)
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.GameCommandMobFireBurnHelpMsg, this.GameCommand.Name, sMAP, sX, sY,
                    sType, sTime, sPoint), MsgColor.Red, MsgType.Hint);
                return;
            }
            var Envir = M2Share.MapMgr.FindMap(sMAP);
            if (Envir != null)
            {
                var OldEnvir = PlayObject.Envir;
                PlayObject.Envir = Envir;
                var FireBurnEvent = new FireBurnEvent(PlayObject, nX, nY, nType, nTime * 1000, nPoint);
                M2Share.EventMgr.AddEvent(FireBurnEvent);
                PlayObject.Envir = OldEnvir;
                return;
            }
            PlayObject.SysMsg(string.Format(GameCommandConst.GameCommandMobFireBurnMapNotFountMsg, this.GameCommand.Name, sMAP), MsgColor.Red, MsgType.Hint);
        }
    }
}