using SystemModule;
using SystemModule.Enums;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 师徒传送，师父可以将徒弟传送到自己身边，徒弟必须允许传送。
    /// </summary>
    [Command("MasterRecall", "师徒传送，师父可以将徒弟传送到自己身边，徒弟必须允许传送。")]
    public class MasterRecallCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            if (!PlayerActor.IsMaster)
            {
                PlayerActor.SysMsg("只能师父才能使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayerActor.MasterList.Count == 0)
            {
                PlayerActor.SysMsg("你的徒弟一个都不在线!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            //if (PlayerActor.SysMsgm_PEnvir.m_boNOMASTERRECALL)
            //{
            //    PlayerActor.SysMsg("本地图禁止师徒传送!!!", MsgColor.c_Red, MsgType.t_Hint);
            //    return;
            //}
            if ((HUtil32.GetTickCount() - PlayerActor.MasterRecallTick) < 10000)
            {
                PlayerActor.SysMsg("稍等一会才能再次使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            for (var i = 0; i < PlayerActor.MasterList.Count; i++)
            {
                var masterHuman = (IPlayerActor)PlayerActor.MasterList[i];
                if (masterHuman.CanMasterRecall)
                {
                    PlayerActor.RecallHuman(masterHuman.ChrName);
                }
                else
                {
                    PlayerActor.SysMsg(masterHuman.ChrName + " 不允许传送!!!", MsgColor.Red, MsgType.Hint);
                }
            }
        }
    }
}