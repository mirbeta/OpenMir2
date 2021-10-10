using SystemModule;
using System;
using System.Collections;
using System.Collections.Generic;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 开始行会争霸赛
    /// </summary>
    [GameCommand("StartContest", "开始行会争霸赛", 10)]
    public class StartContestCommand : BaseCommond
    {
        [DefaultCommand]
        public void StartContest(TPlayObject PlayObject)
        {
            IList<TBaseObject> List10;
            IList<TPlayObject> List14;
            IList<TGuild> guildList;
            TPlayObject m_PlayObject;
            TPlayObject PlayObjectA;
            bool bo19;
            if (!PlayObject.m_PEnvir.Flag.boFight3Zone)
            {
                PlayObject.SysMsg("此命令不能在当前地图中使用!!!", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject.SysMsg("开始行会争霸赛。", TMsgColor.c_Red, TMsgType.t_Hint);
            List10 = new List<TBaseObject>();
            List14 = new List<TPlayObject>();
            guildList = new List<TGuild>();
            M2Share.UserEngine.GetMapRageHuman(PlayObject.m_PEnvir, PlayObject.m_nCurrX, PlayObject.m_nCurrY, 1000, List10);
            for (var i = 0; i < List10.Count; i++)
            {
                m_PlayObject = List10[i] as TPlayObject;
                if (!m_PlayObject.m_boObMode || !m_PlayObject.m_boAdminMode)
                {
                    m_PlayObject.m_nFightZoneDieCount = 0;
                    if (m_PlayObject.m_MyGuild == null)
                    {
                        continue;
                    }
                    bo19 = false;
                    for (var j = 0; j < List14.Count; j++)
                    {
                        PlayObjectA = List14[j];
                        if (m_PlayObject.m_MyGuild == PlayObjectA.m_MyGuild)
                        {
                            bo19 = true;
                        }
                    }
                    if (!bo19)
                    {
                        guildList.Add(m_PlayObject.m_MyGuild);
                    }
                }
            }
            PlayObject.SysMsg("行会争霸赛已经开始。", TMsgColor.c_Green, TMsgType.t_Hint);
            M2Share.UserEngine.CryCry(Grobal2.RM_CRY, PlayObject.m_PEnvir, PlayObject.m_nCurrX, PlayObject.m_nCurrY, 1000, M2Share.g_Config.btCryMsgFColor, M2Share.g_Config.btCryMsgBColor, "- 行会战争已爆发。");
            var s20 = "";
            TGuild Guild;
            for (int i = 0; i < guildList.Count; i++)
            {
                Guild = guildList[i];
                Guild.StartTeamFight();
                for (int II = 0; II < List10.Count; II++)
                {
                    m_PlayObject = List10[i] as TPlayObject;
                    if (m_PlayObject.m_MyGuild == Guild)
                    {
                        Guild.AddTeamFightMember(m_PlayObject.m_sCharName);
                    }
                }
                s20 = s20 + Guild.sGuildName + ' ';
            }
            M2Share.UserEngine.CryCry(Grobal2.RM_CRY, PlayObject.m_PEnvir, PlayObject.m_nCurrX, PlayObject.m_nCurrY, 1000, M2Share.g_Config.btCryMsgFColor, M2Share.g_Config.btCryMsgBColor, " -参加的门派:" + s20);
            List10 = null;
            List14 = null;
        }
    }
}