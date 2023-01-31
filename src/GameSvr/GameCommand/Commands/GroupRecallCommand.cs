using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 组队传送
    /// </summary>
    [Command("GroupRecall", "组队传送", 0)]
    public class GroupRecallCommand : Command
    {
        [ExecuteCommand]
        public void GroupRecall(PlayObject PlayObject)
        {
            if (PlayObject.RecallSuite || PlayObject.Permission >= 6)
            {
                var dwValue = (HUtil32.GetTickCount() - PlayObject.GroupRcallTick) / 1000;
                PlayObject.GroupRcallTick = PlayObject.GroupRcallTick + dwValue * 1000;
                if (PlayObject.Permission >= 6)
                {
                    PlayObject.GroupRcallTime = 0;
                }
                if (PlayObject.GroupRcallTime > dwValue)
                {
                    PlayObject.GroupRcallTime -= (short)dwValue;
                }
                else
                {
                    PlayObject.GroupRcallTime = 0;
                }
                if (PlayObject.GroupRcallTime == 0)
                {
                    if (PlayObject.GroupOwner == PlayObject)
                    {
                        for (var i = 0; i < PlayObject.GroupMembers.Count; i++)
                        {
                            var m_PlayObject = PlayObject.GroupMembers[i];
                            if (m_PlayObject.AllowGroupReCall)
                            {
                                if (m_PlayObject.Envir.Flag.boNORECALL)
                                {
                                    PlayObject.SysMsg($"{m_PlayObject.ChrName} 所在的地图不允许传送。", MsgColor.Red, MsgType.Hint);
                                }
                                else
                                {
                                    PlayObject.RecallHuman(m_PlayObject.ChrName);
                                }
                            }
                            else
                            {
                                PlayObject.SysMsg($"{m_PlayObject.ChrName} 不允许天地合一!!!", MsgColor.Red, MsgType.Hint);
                            }
                        }
                        PlayObject.GroupRcallTick = HUtil32.GetTickCount();
                        PlayObject.GroupRcallTime = (short)M2Share.Config.GroupRecallTime;
                    }
                }
                else
                {
                    PlayObject.SysMsg($"{PlayObject.GroupRcallTime} 秒之后才可以再使用此功能!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                PlayObject.SysMsg("您现在还无法使用此功能!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}