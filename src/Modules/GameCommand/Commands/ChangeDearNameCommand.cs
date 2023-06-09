using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 调整指定玩家配偶名称
    /// </summary>
    [Command("ChangeDearName", "调整指定玩家配偶名称", help: "人物名称 配偶名称(如果为 无 则清除)", 10)]
    public class ChangeDearNameCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sDearName = @params.Length > 1 ? @params[1] : "";
            if (string.IsNullOrEmpty(sHumanName) || string.IsNullOrEmpty(sDearName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mIPlayerActor = ModuleShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor != null)
            {
                if (string.Compare(sDearName, "无", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    mIPlayerActor.DearName = "";
                    mIPlayerActor.RefShowName();
                    PlayerActor.SysMsg(sHumanName + " 的配偶名清除成功。", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    mIPlayerActor.DearName = sDearName;
                    mIPlayerActor.RefShowName();
                    PlayerActor.SysMsg(sHumanName + " 的配偶名更改成功。", MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}