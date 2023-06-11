using SystemModule;
using SystemModule.Enums;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 调整指定玩家等级
    /// </summary>
    [Command("AdjuestLevel", "调整指定玩家等级", "人物名称 等级", 10)]
    public class AdjuestLevelCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var nLevel = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0;
            if (string.IsNullOrEmpty(sHumanName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor != null)
            {
                int nOLevel = mIPlayerActor.Abil.Level;
                mIPlayerActor.HasLevelUp(1);
                //M2Share.EventSource.AddEventLog(17, mIPlayerActor.MapName + "\09" + mIPlayerActor.CurrX + "\09" + mIPlayerActor.CurrY + "\09"
                //                                    + mIPlayerActor.ChrName + "\09" + mIPlayerActor.Abil.Level + "\09" + PlayerActor.ChrName + "\09" + "+(" + nLevel + ")" + "\09" + "0");
                PlayerActor.SysMsg(sHumanName + " 等级调整完成。", MsgColor.Green, MsgType.Hint);
                if (SystemShare.Config.ShowMakeItemMsg)
                {
                    SystemShare.Logger.Warn("[等级调整] " + PlayerActor.ChrName + "(" + mIPlayerActor.ChrName + " " + nOLevel + " -> " + mIPlayerActor.Abil.Level + ")");
                }
            }
            else
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}