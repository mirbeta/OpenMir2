using GameSvr.Actor;
using GameSvr.Guild;
using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 开始行会争霸赛
    /// </summary>
    [Command("StartContest", "开始行会争霸赛", 10)]
    public class StartContestCommand : Command
    {
        [ExecuteCommand]
        public void StartContest(PlayObject PlayObject)
        {
            PlayObject m_PlayObject;
            PlayObject PlayObjectA;
            bool bo19;
            if (!PlayObject.Envir.Flag.boFight3Zone)
            {
                PlayObject.SysMsg("此命令不能在当前地图中使用!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject.SysMsg("开始行会争霸赛。", MsgColor.Red, MsgType.Hint);
            IList<BaseObject> List10 = new List<BaseObject>();
            IList<PlayObject> List14 = new List<PlayObject>();
            IList<GuildInfo> guildList = new List<GuildInfo>();
            M2Share.WorldEngine.GetMapRageHuman(PlayObject.Envir, PlayObject.CurrX, PlayObject.CurrY, 1000, List10);
            for (var i = 0; i < List10.Count; i++)
            {
                m_PlayObject = List10[i] as PlayObject;
                if (!m_PlayObject.ObMode || !m_PlayObject.AdminMode)
                {
                    m_PlayObject.FightZoneDieCount = 0;
                    if (m_PlayObject.MyGuild == null)
                    {
                        continue;
                    }
                    bo19 = false;
                    for (var j = 0; j < List14.Count; j++)
                    {
                        PlayObjectA = List14[j];
                        if (m_PlayObject.MyGuild == PlayObjectA.MyGuild)
                        {
                            bo19 = true;
                        }
                    }
                    if (!bo19)
                    {
                        guildList.Add(m_PlayObject.MyGuild);
                    }
                }
            }
            PlayObject.SysMsg("行会争霸赛已经开始。", MsgColor.Green, MsgType.Hint);
            M2Share.WorldEngine.CryCry(Grobal2.RM_CRY, PlayObject.Envir, PlayObject.CurrX, PlayObject.CurrY, 1000, M2Share.Config.CryMsgFColor, M2Share.Config.CryMsgBColor, "- 行会战争已爆发。");
            var s20 = "";
            GuildInfo Guild;
            for (int i = 0; i < guildList.Count; i++)
            {
                Guild = guildList[i];
                Guild.StartTeamFight();
                for (int II = 0; II < List10.Count; II++)
                {
                    m_PlayObject = List10[i] as PlayObject;
                    if (m_PlayObject.MyGuild == Guild)
                    {
                        Guild.AddTeamFightMember(m_PlayObject.ChrName);
                    }
                }
                s20 = s20 + Guild.sGuildName + ' ';
            }
            M2Share.WorldEngine.CryCry(Grobal2.RM_CRY, PlayObject.Envir, PlayObject.CurrX, PlayObject.CurrY, 1000, M2Share.Config.CryMsgFColor, M2Share.Config.CryMsgBColor, " -参加的门派:" + s20);
        }
    }
}