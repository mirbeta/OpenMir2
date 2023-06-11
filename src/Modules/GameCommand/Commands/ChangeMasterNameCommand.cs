using SystemModule;
using SystemModule.Enums;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 调整指定玩家师傅名称
    /// </summary>
    [Command("ChangeMasterName", "调整指定玩家师傅名称", "人物名称 师徒名称(如果为 无 则清除)", 10)]
    public class ChangeMasterNameCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sMasterName = @params.Length > 1 ? @params[1] : "";
            var sIsMaster = @params.Length > 2 ? @params[2] : "";
            if (string.IsNullOrEmpty(sHumanName) || string.IsNullOrEmpty(sMasterName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor != null)
            {
                if (string.Compare(sMasterName, "无", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    mIPlayerActor.MasterName = "";
                    mIPlayerActor.RefShowName();
                    mIPlayerActor.IsMaster = false;
                    PlayerActor.SysMsg(sHumanName + " 的师徒名清除成功。", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    mIPlayerActor.MasterName = sMasterName;
                    if (!string.IsNullOrEmpty(sIsMaster) && sIsMaster[0] == '1')
                    {
                        mIPlayerActor.IsMaster = true;
                    }
                    else
                    {
                        mIPlayerActor.IsMaster = false;
                    }
                    mIPlayerActor.RefShowName();
                    PlayerActor.SysMsg(sHumanName + " 的师徒名更改成功。", MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}