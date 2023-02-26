using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 组队传送
    /// </summary>
    [Command("GroupRecall", "组队传送", 0)]
    public class GroupRecallCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject) {
            if (PlayObject.RecallSuite || PlayObject.Permission >= 6) {
                short dwValue = (short)((HUtil32.GetTickCount() - PlayObject.GroupRcallTick) / 1000);
                PlayObject.GroupRcallTick = PlayObject.GroupRcallTick + dwValue * 1000;
                if (PlayObject.Permission >= 6) {
                    PlayObject.GroupRcallTime = 0;
                }
                if (PlayObject.GroupRcallTime > dwValue) {
                    PlayObject.GroupRcallTime -= dwValue;
                }
                else {
                    PlayObject.GroupRcallTime = 0;
                }
                if (PlayObject.GroupRcallTime == 0) {
                    if (PlayObject.GroupOwner == PlayObject.ActorId) {
                        for (int i = 0; i < PlayObject.GroupMembers.Count; i++) {
                            PlayObject m_PlayObject = PlayObject.GroupMembers[i];
                            if (m_PlayObject.AllowGroupReCall) {
                                if (m_PlayObject.Envir.Flag.NoReCall) {
                                    PlayObject.SysMsg($"{m_PlayObject.ChrName} 所在的地图不允许传送。", MsgColor.Red, MsgType.Hint);
                                }
                                else {
                                    PlayObject.RecallHuman(m_PlayObject.ChrName);
                                }
                            }
                            else {
                                PlayObject.SysMsg($"{m_PlayObject.ChrName} 不允许天地合一!!!", MsgColor.Red, MsgType.Hint);
                            }
                        }
                        PlayObject.GroupRcallTick = HUtil32.GetTickCount();
                        PlayObject.GroupRcallTime = M2Share.Config.GroupRecallTime;
                    }
                }
                else {
                    PlayObject.SysMsg($"{PlayObject.GroupRcallTime} 秒之后才可以再使用此功能!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else {
                PlayObject.SysMsg("您现在还无法使用此功能!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}