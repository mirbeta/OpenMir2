using GameSrv.Player;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 结束行会争霸赛
    /// </summary>
    [Command("EndContest", "结束行会争霸赛", 10)]
    public class EndContestCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            //string sParam1 = @Params.Length > 0 ? @Params[0] : "";
            //List<PlayObject> List10;
            //ArrayList List14;
            //PlayObject m_PlayObject;
            //PlayObject PlayObjectA;
            //bool bo19;
            //string s20;
            //TGUild Guild;
            //if (((if(!string.IsNullOrEmpty(sParam1))) && (sParam1[0] == '?')))
            //{
            //    PlayObject.SysMsg("结束行会争霸赛。", MsgColor.c_Red, MsgType.t_Hint);
            //    PlayObject.SysMsg(string.Format("命令格式: @{0}", this.Attributes.Name), MsgColor.c_Red, MsgType.t_Hint);
            //    return;
            //}
            //if (!PlayObject.m_PEnvir.m_boFight3Zone)
            //{
            //    PlayObject.SysMsg("此命令不能在当前地图中使用!!!", MsgColor.c_Red, MsgType.t_Hint);
            //    return;
            //}
            //List10 = new List<PlayObject>();
            //List14 = new ArrayList();
            //M2Share.WorldEngine.GetMapRageHuman(PlayObject.m_PEnvir, PlayObject.CurrX, PlayObject.CurrY, 1000, List10);
            //for (int i = 0; i < List10.Count; i++)
            //{
            //    m_PlayObject = List10[i];
            //    if (!m_PlayObject.m_boObMode || !m_PlayObject.m_boAdminMode)
            //    {
            //        if (m_PlayObject.MyGuild == null)
            //        {
            //            continue;
            //        }
            //        bo19 = false;
            //        for (int II = 0; II < List14.Count; II++)
            //        {
            //            PlayObjectA = ((List14[II]) as PlayObject);
            //            if (m_PlayObject.MyGuild == PlayObjectA.MyGuild)
            //            {
            //                bo19 = true;
            //            }
            //        }
            //        if (!bo19)
            //        {
            //            List14.Add(m_PlayObject.MyGuild);
            //        }
            //    }
            //}
            //for (int i = 0; i < List14.Count; i++)
            //{
            //    Guild = ((TGUild)(List14[i]));
            //    Guild.EndTeamFight();
            //    M2Share.WorldEngine.CryCry(Messages.RM_CRY, PlayObject.m_PEnvir, PlayObject.CurrX, PlayObject.CurrY, 1000, Settings.Config.btCryMsgFColor,
            //        Settings.Config.btCryMsgBColor, string.Format(" - {0} 行会争霸赛已结束。", Guild.sGuildName));
            //}
            //HUtil32.Dispose(List10);
            //HUtil32.Dispose(List14);
        }
    }
}