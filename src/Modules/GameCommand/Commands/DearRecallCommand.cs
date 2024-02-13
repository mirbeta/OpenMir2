using OpenMir2;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 夫妻传送，将对方传送到自己身边，对方必须允许传送。
    /// </summary>
    [Command("DearRecall", "夫妻传送", "(夫妻传送，将对方传送到自己身边，对方必须允许传送。)")]
    public class DearRecallCommond : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            if (string.IsNullOrEmpty(PlayerActor.DearName))
            {
                PlayerActor.SysMsg("你没有结婚!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayerActor.Envir.Flag.boNODEARRECALL)
            {
                PlayerActor.SysMsg("本地图禁止夫妻传送!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayerActor.DearHuman == null)
            {
                if (PlayerActor.Gender == 0)
                {
                    PlayerActor.SysMsg("你的老婆不在线!!!", MsgColor.Red, MsgType.Hint);
                }
                else
                {
                    PlayerActor.SysMsg("你的老公不在线!!!", MsgColor.Red, MsgType.Hint);
                }
                return;
            }
            if (HUtil32.GetTickCount() - PlayerActor.DearRecallTick < 10000)
            {
                PlayerActor.SysMsg("稍等会才能再次使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayerActor.DearRecallTick = HUtil32.GetTickCount();
            if (PlayerActor.DearHuman.CanDearRecall)
            {
                PlayerActor.RecallHuman(PlayerActor.DearHuman.ChrName);
            }
            else
            {
                PlayerActor.SysMsg(PlayerActor.DearHuman.ChrName + " 不允许传送!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}