using SystemModule;

namespace CommandSystem {
    /// <summary>
    /// 结束行会争霸赛
    /// </summary>
    [Command("EndContest", "结束行会争霸赛", 10)]
    public class EndContestCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor) {
            //string sParam1 = @Params.Length > 0 ? @Params[0] : "";
            //List<IPlayerActor> List10;
            //ArrayList List14;
            //IPlayerActor m_IPlayerActor;
            //IPlayerActor IPlayerActorA;
            //bool bo19;
            //string s20;
            //TGUild Guild;
            //if (((if(!string.IsNullOrEmpty(sParam1))) && (sParam1[0] == '?')))
            //{
            //    PlayerActor.SysMsg("结束行会争霸赛。", MsgColor.c_Red, MsgType.t_Hint);
            //    PlayerActor.SysMsg(string.Format("命令格式: @{0}", this.Attributes.Name), MsgColor.c_Red, MsgType.t_Hint);
            //    return;
            //}
            //if (!PlayerActor.SysMsgm_PEnvir.m_boFight3Zone)
            //{
            //    PlayerActor.SysMsg("此命令不能在当前地图中使用!!!", MsgColor.c_Red, MsgType.t_Hint);
            //    return;
            //}
            //List10 = new List<IPlayerActor>();
            //List14 = new ArrayList();
            //M2Share.WorldEngine.GetMapRageHuman(PlayerActor.SysMsgm_PEnvir, PlayerActor.CurrX, PlayerActor.CurrY, 1000, List10);
            //for (int i = 0; i < List10.Count; i++)
            //{
            //    m_IPlayerActor = List10[i];
            //    if (!m_IPlayerActor.m_boObMode || !m_IPlayerActor.m_boAdminMode)
            //    {
            //        if (m_IPlayerActor.MyGuild == null)
            //        {
            //            continue;
            //        }
            //        bo19 = false;
            //        for (int II = 0; II < List14.Count; II++)
            //        {
            //            IPlayerActorA = ((List14[II]) as IPlayerActor);
            //            if (m_IPlayerActor.MyGuild == IPlayerActorA.MyGuild)
            //            {
            //                bo19 = true;
            //            }
            //        }
            //        if (!bo19)
            //        {
            //            List14.Add(m_IPlayerActor.MyGuild);
            //        }
            //    }
            //}
            //for (int i = 0; i < List14.Count; i++)
            //{
            //    Guild = ((TGUild)(List14[i]));
            //    Guild.EndTeamFight();
            //    M2Share.WorldEngine.CryCry(Messages.RM_CRY, PlayerActor.SysMsgm_PEnvir, PlayerActor.CurrX, PlayerActor.CurrY, 1000, Settings.Config.btCryMsgFColor,
            //        Settings.Config.btCryMsgBColor, string.Format(" - {0} 行会争霸赛已结束。", Guild.sGuildName));
            //}
            //HUtil32.Dispose(List10);
            //HUtil32.Dispose(List14);
        }
    }
}