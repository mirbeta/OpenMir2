using OpenMir2;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 搜索指定玩家所在地图XY坐标
    /// </summary>
    [Command("SearchHuman", "搜索指定玩家所在地图XY坐标", "人物名称")]
    public class SearchHumanCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            if (PlayerActor.ProbeNecklace || PlayerActor.Permission >= 6)
            {
                if (string.IsNullOrEmpty(sHumanName))
                {
                    PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                    return;
                }
                if (HUtil32.GetTickCount() - PlayerActor.ProbeTick > 10000 || PlayerActor.Permission >= 3)
                {
                    PlayerActor.ProbeTick = HUtil32.GetTickCount();
                    IPlayerActor mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumanName);
                    if (mIPlayerActor != null)
                    {
                        PlayerActor.SysMsg(sHumanName + " 现在位于 " + mIPlayerActor.Envir.MapDesc + '(' + mIPlayerActor.Envir.MapName + ") " + mIPlayerActor.CurrX + ':'
                            + PlayerActor.CurrY, MsgColor.Blue, MsgType.Hint);
                    }
                    else
                    {
                        PlayerActor.SysMsg(sHumanName + " 现在不在线，或位于其它服务器上!!!", MsgColor.Red, MsgType.Hint);
                    }
                }
                else
                {
                    PlayerActor.SysMsg((HUtil32.GetTickCount() - PlayerActor.ProbeTick) / 1000 - 10 + " 秒之后才可以再使用此功能!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                PlayerActor.SysMsg("您现在还无法使用此功能!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}