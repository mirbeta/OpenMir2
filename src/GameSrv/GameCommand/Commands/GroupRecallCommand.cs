using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 组队传送
    /// </summary>
    [Command("GroupRecall", "组队传送")]
    public class GroupRecallCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (playObject.RecallSuite || playObject.Permission >= 6) {
                short dwValue = (short)((HUtil32.GetTickCount() - playObject.GroupRcallTick) / 1000);
                playObject.GroupRcallTick = playObject.GroupRcallTick + dwValue * 1000;
                if (playObject.Permission >= 6) {
                    playObject.GroupRcallTime = 0;
                }
                if (playObject.GroupRcallTime > dwValue) {
                    playObject.GroupRcallTime -= dwValue;
                }
                else {
                    playObject.GroupRcallTime = 0;
                }
                if (playObject.GroupRcallTime == 0) {
                    if (playObject.GroupOwner == playObject.ActorId) {
                        for (int i = 0; i < playObject.GroupMembers.Count; i++) {
                            PlayObject mPlayObject = playObject.GroupMembers[i];
                            if (mPlayObject.AllowGroupReCall) {
                                if (mPlayObject.Envir.Flag.NoReCall) {
                                    playObject.SysMsg($"{mPlayObject.ChrName} 所在的地图不允许传送。", MsgColor.Red, MsgType.Hint);
                                }
                                else {
                                    playObject.RecallHuman(mPlayObject.ChrName);
                                }
                            }
                            else {
                                playObject.SysMsg($"{mPlayObject.ChrName} 不允许天地合一!!!", MsgColor.Red, MsgType.Hint);
                            }
                        }
                        playObject.GroupRcallTick = HUtil32.GetTickCount();
                        playObject.GroupRcallTime = M2Share.Config.GroupRecallTime;
                    }
                }
                else {
                    playObject.SysMsg($"{playObject.GroupRcallTime} 秒之后才可以再使用此功能!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else {
                playObject.SysMsg("您现在还无法使用此功能!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}