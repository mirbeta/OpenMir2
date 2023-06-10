using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 组队传送
    /// </summary>
    [Command("GroupRecall", "组队传送")]
    public class GroupRecallCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            if (PlayerActor.RecallSuite || PlayerActor.Permission >= 6)
            {
                var dwValue = (short)((HUtil32.GetTickCount() - PlayerActor.GroupRcallTick) / 1000);
                PlayerActor.GroupRcallTick = PlayerActor.GroupRcallTick + dwValue * 1000;
                if (PlayerActor.Permission >= 6)
                {
                    PlayerActor.GroupRcallTime = 0;
                }
                if (PlayerActor.GroupRcallTime > dwValue)
                {
                    PlayerActor.GroupRcallTime -= dwValue;
                }
                else
                {
                    PlayerActor.GroupRcallTime = 0;
                }
                if (PlayerActor.GroupRcallTime == 0)
                {
                    if (PlayerActor.GroupOwner == PlayerActor.ActorId)
                    {
                        for (var i = 0; i < PlayerActor.GroupMembers.Count; i++)
                        {
                            var mIPlayerActor = (IPlayerActor)PlayerActor.GroupMembers[i];
                            if (mIPlayerActor.AllowGroupReCall)
                            {
                                if (mIPlayerActor.Envir.Flag.NoReCall)
                                {
                                    PlayerActor.SysMsg($"{mIPlayerActor.ChrName} 所在的地图不允许传送。", MsgColor.Red, MsgType.Hint);
                                }
                                else
                                {
                                    PlayerActor.RecallHuman(mIPlayerActor.ChrName);
                                }
                            }
                            else
                            {
                                PlayerActor.SysMsg($"{mIPlayerActor.ChrName} 不允许天地合一!!!", MsgColor.Red, MsgType.Hint);
                            }
                        }
                        PlayerActor.GroupRcallTick = HUtil32.GetTickCount();
                        PlayerActor.GroupRcallTime = SystemShare.Config.GroupRecallTime;
                    }
                }
                else
                {
                    PlayerActor.SysMsg($"{PlayerActor.GroupRcallTime} 秒之后才可以再使用此功能!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                PlayerActor.SysMsg("您现在还无法使用此功能!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}