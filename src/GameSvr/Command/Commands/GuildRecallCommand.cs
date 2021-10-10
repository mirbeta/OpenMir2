using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 行会传送，行会掌门人可以将整个行会成员全部集中。
    /// </summary>
    [GameCommand("GuildRecall", "行会传送，行会掌门人可以将整个行会成员全部集中。", 0)]
    public class GuildRecallCommand : BaseCommond
    {
        [DefaultCommand]
        public void GuildRecall(TPlayObject PlayObject)
        {
            if (!PlayObject.m_boGuildMove && PlayObject.m_btPermission < 6)
            {
                PlayObject.SysMsg("您现在还无法使用此功能!!!", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (!PlayObject.IsGuildMaster())
            {
                PlayObject.SysMsg("行会掌门人才可以使用此功能!!!", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (PlayObject.m_PEnvir.Flag.boNOGUILDRECALL)
            {
                PlayObject.SysMsg("本地图不允许使用此功能!!!", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            TGuildRank GuildRank;
            TUserCastle m_Castle;
            m_Castle = M2Share.CastleManager.InCastleWarArea(PlayObject);
            if (m_Castle != null && m_Castle.m_boUnderWar)
            {
                PlayObject.SysMsg("攻城区域不允许使用此功能!!!", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var nRecallCount = 0;
            var nNoRecallCount = 0;
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
            if (PlayObject.m_wGroupRcallTime > 0)
            {
                PlayObject.SysMsg($"{PlayObject.m_wGroupRcallTime} 秒之后才可以再使用此功能!!!", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            TPlayObject m_PlayObject;
            for (var i = 0; i < PlayObject.m_MyGuild.m_RankList.Count; i++)
            {
                GuildRank = PlayObject.m_MyGuild.m_RankList[i];
                if (GuildRank == null)
                {
                    continue;
                }
                for (var j = 0; j < GuildRank.MemberList.Count; j++)
                {
                    m_PlayObject = M2Share.UserEngine.GetPlayObject(GuildRank.MemberList[j].sMemberName);
                    if (m_PlayObject != null)
                    {
                        if (m_PlayObject == PlayObject)
                        {
                            // Inc(nNoRecallCount);
                            continue;
                        }
                        if (m_PlayObject.m_boAllowGuildReCall)
                        {
                            if (m_PlayObject.m_PEnvir.Flag.boNORECALL)
                            {
                                PlayObject.SysMsg($"{m_PlayObject.m_sCharName} 所在的地图不允许传送。", TMsgColor.c_Red, TMsgType.t_Hint);
                            }
                            else
                            {
                                PlayObject.RecallHuman(m_PlayObject.m_sCharName);
                                nRecallCount++;
                            }
                        }
                        else
                        {
                            nNoRecallCount++;
                            PlayObject.SysMsg($"{m_PlayObject.m_sCharName} 不允许行会合一!!!", TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                    }
                }
            }
            PlayObject.SysMsg($"已传送{nRecallCount}个成员，{nNoRecallCount}个成员未被传送。", TMsgColor.c_Green, TMsgType.t_Hint);
            PlayObject.m_dwGroupRcallTick = HUtil32.GetTickCount();
            PlayObject.m_wGroupRcallTime = (short)M2Share.g_Config.nGuildRecallTime;
        }
    }
}