using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 设置怪物集中目标
    /// </summary>
    [Command("Mission", "设置怪物集中目标", " X Y", 10)]
    public class MissionCommand : Command
    {
        [ExecuteCommand]
        public void Mission(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sX = @Params.Length > 0 ? @Params[0] : "";
            var sY = @Params.Length > 1 ? @Params[1] : "";
            if (sX == "" || sY == "")
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var nX = (short)HUtil32.StrToInt(sX, 0);
            var nY = (short)HUtil32.StrToInt(sY, 0);
            M2Share.g_boMission = true;
            M2Share.g_sMissionMap = PlayObject.MapName;
            M2Share.g_nMissionX = nX;
            M2Share.g_nMissionY = nY;
            PlayObject.SysMsg("怪物集中目标已设定为: " + PlayObject.MapName + '(' + M2Share.g_nMissionX + ':' + M2Share.g_nMissionY + ')', MsgColor.Green, MsgType.Hint);
        }
    }
}