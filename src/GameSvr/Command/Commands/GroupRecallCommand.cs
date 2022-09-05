﻿using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 组队传送
    /// </summary>
    [GameCommand("GroupRecall", "组队传送", 0)]
    public class GroupRecallCommand : BaseCommond
    {
        [DefaultCommand]
        public void GroupRecall(PlayObject PlayObject)
        {
            if (PlayObject.MBoRecallSuite || PlayObject.Permission >= 6)
            {
                var dwValue = (HUtil32.GetTickCount() - PlayObject.GroupRcallTick) / 1000;
                PlayObject.GroupRcallTick = PlayObject.GroupRcallTick + dwValue * 1000;
                if (PlayObject.Permission >= 6)
                {
                    PlayObject.MWGroupRcallTime = 0;
                }
                if (PlayObject.MWGroupRcallTime > dwValue)
                {
                    PlayObject.MWGroupRcallTime -= (short)dwValue;
                }
                else
                {
                    PlayObject.MWGroupRcallTime = 0;
                }
                if (PlayObject.MWGroupRcallTime == 0)
                {
                    if (PlayObject.MGroupOwner == PlayObject)
                    {
                        for (var i = 0; i < PlayObject.GroupMembers.Count; i++)
                        {
                            var m_PlayObject = PlayObject.GroupMembers[i];
                            if (m_PlayObject.MBoAllowGroupReCall)
                            {
                                if (m_PlayObject.Envir.Flag.boNORECALL)
                                {
                                    PlayObject.SysMsg($"{m_PlayObject.CharName} 所在的地图不允许传送。", MsgColor.Red, MsgType.Hint);
                                }
                                else
                                {
                                    PlayObject.RecallHuman(m_PlayObject.CharName);
                                }
                            }
                            else
                            {
                                PlayObject.SysMsg($"{m_PlayObject.CharName} 不允许天地合一!!!", MsgColor.Red, MsgType.Hint);
                            }
                        }
                        PlayObject.GroupRcallTick = HUtil32.GetTickCount();
                        PlayObject.MWGroupRcallTime = (short)M2Share.Config.nGroupRecallTime;
                    }
                }
                else
                {
                    PlayObject.SysMsg($"{PlayObject.MWGroupRcallTime} 秒之后才可以再使用此功能!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                PlayObject.SysMsg("您现在还无法使用此功能!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}