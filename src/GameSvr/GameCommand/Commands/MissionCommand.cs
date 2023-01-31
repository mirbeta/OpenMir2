using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

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
            string sX = @Params.Length > 0 ? @Params[0] : "";
            string sY = @Params.Length > 1 ? @Params[1] : "";
            if (sX == "" || sY == "")
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            short nX = (short)HUtil32.StrToInt(sX, 0);
            short nY = (short)HUtil32.StrToInt(sY, 0);
            M2Share.BoMission = true;
            M2Share.MissionMap = PlayObject.MapName;
            M2Share.MissionX = nX;
            M2Share.MissionY = nY;
            PlayObject.SysMsg("怪物集中目标已设定为: " + PlayObject.MapName + '(' + M2Share.MissionX + ':' + M2Share.MissionY + ')', MsgColor.Green, MsgType.Hint);
        }
    }
}