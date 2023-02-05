using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 搜索指定玩家所在地图XY坐标
    /// </summary>
    [Command("SearchHuman", "搜索指定玩家所在地图XY坐标", "人物名称", 0)]
    public class SearchHumanCommand : GameCommand
    {
        [ExecuteCommand]
        public void SearchHuman(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            if (PlayObject.ProbeNecklace || PlayObject.Permission >= 6)
            {
                if (string.IsNullOrEmpty(sHumanName))
                {
                    PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                    return;
                }
                if (HUtil32.GetTickCount() - PlayObject.ProbeTick > 10000 || PlayObject.Permission >= 3)
                {
                    PlayObject.ProbeTick = HUtil32.GetTickCount();
                    PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
                    if (m_PlayObject != null)
                    {
                        PlayObject.SysMsg(sHumanName + " 现在位于 " + m_PlayObject.Envir.MapDesc + '(' + m_PlayObject.Envir.MapName + ") " + m_PlayObject.CurrX + ':'
                            + PlayObject.CurrY, MsgColor.Blue, MsgType.Hint);
                    }
                    else
                    {
                        PlayObject.SysMsg(sHumanName + " 现在不在线，或位于其它服务器上!!!", MsgColor.Red, MsgType.Hint);
                    }
                }
                else
                {
                    PlayObject.SysMsg((HUtil32.GetTickCount() - PlayObject.ProbeTick) / 1000 - 10 + " 秒之后才可以再使用此功能!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                PlayObject.SysMsg("您现在还无法使用此功能!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}