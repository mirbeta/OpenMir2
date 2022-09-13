using GameSvr.Player;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 结束行会争霸赛
    /// </summary>
    [GameCommand("EndContest", "结束行会争霸赛", 10)]
    public class EndContestCommand : BaseCommond
    {
        [DefaultCommand]
        public void EndContest(string[] @Params, PlayObject PlayObject)
        {
            //string sParam1 = @Params.Length > 0 ? @Params[0] : "";
            //List<TPlayObject> List10;
            //ArrayList List14;
            //TPlayObject m_PlayObject;
            //TPlayObject PlayObjectA;
            //bool bo19;
            //string s20;
            //TGUild Guild;
            //if (((sParam1 != "") && (sParam1[0] == '?')))
            //{
            //    PlayObject.SysMsg("结束行会争霸赛。", TMsgColor.c_Red, TMsgType.t_Hint);
            //    PlayObject.SysMsg(string.Format("命令格式: @{0}", this.Attributes.Name), TMsgColor.c_Red, TMsgType.t_Hint);
            //    return;
            //}
            //if (!PlayObject.m_PEnvir.m_boFight3Zone)
            //{
            //    PlayObject.SysMsg("此命令不能在当前地图中使用!!!", TMsgColor.c_Red, TMsgType.t_Hint);
            //    return;
            //}
            //List10 = new List<TPlayObject>();
            //List14 = new ArrayList();
            //M2Share.WorldEngine.GetMapRageHuman(PlayObject.m_PEnvir, PlayObject.m_nCurrX, PlayObject.m_nCurrY, 1000, List10);
            //for (int i = 0; i < List10.Count; i++)
            //{
            //    m_PlayObject = List10[i];
            //    if (!m_PlayObject.m_boObMode || !m_PlayObject.m_boAdminMode)
            //    {
            //        if (m_PlayObject.m_MyGuild == null)
            //        {
            //            continue;
            //        }
            //        bo19 = false;
            //        for (int II = 0; II < List14.Count; II++)
            //        {
            //            PlayObjectA = ((List14[II]) as TPlayObject);
            //            if (m_PlayObject.m_MyGuild == PlayObjectA.m_MyGuild)
            //            {
            //                bo19 = true;
            //            }
            //        }
            //        if (!bo19)
            //        {
            //            List14.Add(m_PlayObject.m_MyGuild);
            //        }
            //    }
            //}
            //for (int i = 0; i < List14.Count; i++)
            //{
            //    Guild = ((TGUild)(List14[i]));
            //    Guild.EndTeamFight();
            //    M2Share.WorldEngine.CryCry(Grobal2.RM_CRY, PlayObject.m_PEnvir, PlayObject.m_nCurrX, PlayObject.m_nCurrY, 1000, M2Share.g_Config.btCryMsgFColor,
            //        M2Share.g_Config.btCryMsgBColor, string.Format(" - {0} 行会争霸赛已结束。", Guild.sGuildName));
            //}
            //HUtil32.Dispose(List10);
            //HUtil32.Dispose(List14);
        }
    }
}