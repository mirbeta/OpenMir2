using GameSvr.Castle;
using GameSvr.Guild;
using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 行会传送，行会掌门人可以将整个行会成员全部集中。
    /// </summary>
    [GameCommand("GuildRecall", "行会传送，行会掌门人可以将整个行会成员全部集中。", 0)]
    public class GuildRecallCommand : BaseCommond
    {
        [DefaultCommand]
        public void GuildRecall(PlayObject PlayObject)
        {
            if (!PlayObject.GuildMove && PlayObject.Permission < 6)
            {
                PlayObject.SysMsg("您现在还无法使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (!PlayObject.IsGuildMaster())
            {
                PlayObject.SysMsg("行会掌门人才可以使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayObject.Envir.Flag.boNOGUILDRECALL)
            {
                PlayObject.SysMsg("本地图不允许使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            TGuildRank GuildRank;
            var m_Castle = M2Share.CastleMgr.InCastleWarArea(PlayObject);
            if (m_Castle != null && m_Castle.UnderWar)
            {
                PlayObject.SysMsg("攻城区域不允许使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            var nRecallCount = 0;
            var nNoRecallCount = 0;
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
            if (PlayObject.GroupRcallTime > 0)
            {
                PlayObject.SysMsg($"{PlayObject.GroupRcallTime} 秒之后才可以再使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject m_PlayObject;
            for (var i = 0; i < PlayObject.MyGuild.m_RankList.Count; i++)
            {
                GuildRank = PlayObject.MyGuild.m_RankList[i];
                if (GuildRank == null)
                {
                    continue;
                }
                for (var j = 0; j < GuildRank.MemberList.Count; j++)
                {
                    m_PlayObject = M2Share.WorldEngine.GetPlayObject(GuildRank.MemberList[j].sMemberName);
                    if (m_PlayObject != null)
                    {
                        if (m_PlayObject == PlayObject)
                        {
                            // Inc(nNoRecallCount);
                            continue;
                        }
                        if (m_PlayObject.AllowGuildReCall)
                        {
                            if (m_PlayObject.Envir.Flag.boNORECALL)
                            {
                                PlayObject.SysMsg($"{m_PlayObject.CharName} 所在的地图不允许传送。", MsgColor.Red, MsgType.Hint);
                            }
                            else
                            {
                                PlayObject.RecallHuman(m_PlayObject.CharName);
                                nRecallCount++;
                            }
                        }
                        else
                        {
                            nNoRecallCount++;
                            PlayObject.SysMsg($"{m_PlayObject.CharName} 不允许行会合一!!!", MsgColor.Red, MsgType.Hint);
                        }
                    }
                }
            }
            PlayObject.SysMsg($"已传送{nRecallCount}个成员，{nNoRecallCount}个成员未被传送。", MsgColor.Green, MsgType.Hint);
            PlayObject.GroupRcallTick = HUtil32.GetTickCount();
            PlayObject.GroupRcallTime = (short)M2Share.Config.GuildRecallTime;
        }
    }
}