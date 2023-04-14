using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 师徒传送，师父可以将徒弟传送到自己身边，徒弟必须允许传送。
    /// </summary>
    [Command("MasterRecall", "师徒传送，师父可以将徒弟传送到自己身边，徒弟必须允许传送。")]
    public class MasterRecallCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (!playObject.IsMaster) {
                playObject.SysMsg("只能师父才能使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (playObject.MasterList.Count == 0) {
                playObject.SysMsg("你的徒弟一个都不在线!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            //if (PlayObject.m_PEnvir.m_boNOMASTERRECALL)
            //{
            //    PlayObject.SysMsg("本地图禁止师徒传送!!!", TMsgColor.c_Red, TMsgType.t_Hint);
            //    return;
            //}
            if ((HUtil32.GetTickCount() - playObject.MasterRecallTick) < 10000) {
                playObject.SysMsg("稍等一会才能再次使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            for (var i = 0; i < playObject.MasterList.Count; i++) {
                var masterHuman = playObject.MasterList[i];
                if (masterHuman.CanMasterRecall) {
                    playObject.RecallHuman(masterHuman.ChrName);
                }
                else {
                    playObject.SysMsg(masterHuman.ChrName + " 不允许传送!!!", MsgColor.Red, MsgType.Hint);
                }
            }
        }
    }
}