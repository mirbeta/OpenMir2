using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 删除指定玩家属性点
    /// </summary>
    [Command("DelBonuPoint", "删除指定玩家属性点", "人物名称", 10)]
    public class DelBonuPointCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var targerIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumName);
            if (targerIPlayerActor != null)
            {
                targerIPlayerActor.BonusPoint = 0;
                targerIPlayerActor.SendMsg(Messages.RM_ADJUST_BONUS, 0, 0, 0, 0);
                targerIPlayerActor.HasLevelUp(0);
                targerIPlayerActor.SysMsg("分配点数已清除!!!", MsgColor.Red, MsgType.Hint);
                PlayerActor.SysMsg(sHumName + " 的分配点数已清除.", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}