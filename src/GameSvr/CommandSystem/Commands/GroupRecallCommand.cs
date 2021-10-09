using GameSvr.CommandSystem;
using SystemModule;

namespace GameSvr
{
    /// <summary>
    /// 组队传送
    /// </summary>
    [GameCommand("GroupRecall", "组队传送", 0)]
    public class GroupRecallCommand : BaseCommond
    {
        [DefaultCommand]
        public void GroupRecall(string[] @Params, TPlayObject PlayObject)
        {
            TPlayObject m_PlayObject;
            if (PlayObject.m_boRecallSuite || PlayObject.m_btPermission >= 6)
            {
                var dwValue = (HUtil32.GetTickCount() - PlayObject.m_dwGroupRcallTick) / 1000;
                PlayObject.m_dwGroupRcallTick = PlayObject.m_dwGroupRcallTick + dwValue * 1000;
                if (PlayObject.m_btPermission >= 6)
                {
                    PlayObject.m_wGroupRcallTime = 0;
                }
                if (PlayObject.m_wGroupRcallTime > dwValue)
                {
                    PlayObject.m_wGroupRcallTime -= (short)dwValue;
                }
                else
                {
                    PlayObject.m_wGroupRcallTime = 0;
                }
                if (PlayObject.m_wGroupRcallTime == 0)
                {
                    if (PlayObject.m_GroupOwner == PlayObject)
                    {
                        for (var i = 1; i < PlayObject.m_GroupMembers.Count; i++)
                        {
                            m_PlayObject = PlayObject.m_GroupMembers[i];
                            if (m_PlayObject.m_boAllowGroupReCall)
                            {
                                if (m_PlayObject.m_PEnvir.Flag.boNORECALL)
                                {
                                    PlayObject.SysMsg(string.Format("{0} 所在的地图不允许传送。", m_PlayObject.m_sCharName), TMsgColor.c_Red, TMsgType.t_Hint);
                                }
                                else
                                {
                                    PlayObject.RecallHuman(m_PlayObject.m_sCharName);
                                }
                            }
                            else
                            {
                                PlayObject.SysMsg(string.Format("{0} 不允许天地合一!!!", m_PlayObject.m_sCharName), TMsgColor.c_Red, TMsgType.t_Hint);
                            }
                        }
                        PlayObject.m_dwGroupRcallTick = HUtil32.GetTickCount();
                        PlayObject.m_wGroupRcallTime = (short)M2Share.g_Config.nGroupRecallTime;
                    }
                }
                else
                {
                    PlayObject.SysMsg(string.Format("{0} 秒之后才可以再使用此功能!!!", PlayObject.m_wGroupRcallTime), TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
            else
            {
                PlayObject.SysMsg("您现在还无法使用此功能!!!", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }
    }
}