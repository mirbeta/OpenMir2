using SystemModule;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 查看行会争霸赛结果
    /// </summary>
    [Command("Announcement", "查看行会争霸赛结果", 10)]
    public class AnnouncementCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            //string sGuildName = @Params.Length > 0 ? @Params[0] : "";
            //TGUild Guild;
            //string sHumanName;
            //int nPoint;
            //if ((sGuildName == "") || ((sGuildName != "") && (sGuildName[0] == '?')))
            //{
            //    PlayerActor.SysMsg("查看行会争霸赛结果。", MsgColor.c_Red, MsgType.t_Hint);
            //    PlayerActor.SysMsg(string.Format("命令格式: @{0} 行会名称", this.Attributes.Name), MsgColor.c_Red, MsgType.t_Hint);
            //    return;
            //}
            //if (!PlayerActor.SysMsgm_PEnvir.m_boFight3Zone)
            //{
            //    PlayerActor.SysMsg("此命令不能在当前地图中使用!!!", MsgColor.c_Red, MsgType.t_Hint);
            //    return;
            //}
            //Guild = Settings.g_GuildManager.FindGuild(sGuildName);
            //if (Guild != null)
            //{
            //    SystemShare.WorldEngine.CryCry(Messages.RM_CRY, PlayerActor.SysMsgm_PEnvir, PlayerActor.CurrX, PlayerActor.CurrY, 1000, Settings.Config.btCryMsgFColor,
            //        Settings.Config.btCryMsgBColor, string.Format(" - %s 行会争霸赛结果: ", Guild.sGuildName));
            //    for (int i = 0; i < Guild.TeamFightDeadList.Count; i++)
            //    {
            //        nPoint = HUtil32.ObjectToInt(Guild.TeamFightDeadList[i]);
            //        sHumanName = Guild.TeamFightDeadList[i];
            //        SystemShare.WorldEngine.CryCry(Messages.RM_CRY, PlayerActor.SysMsgm_PEnvir, PlayerActor.CurrX, PlayerActor.CurrY, 1000,
            //            Settings.Config.btCryMsgFColor, Settings.Config.btCryMsgBColor, string.Format(" - %s  : %d 分/死亡%d次。 ",
            //            sHumanName, HUtil32.HiWord(nPoint), HUtil32.LoWord(nPoint)));
            //    }
            //}
            //SystemShare.WorldEngine.CryCry(Messages.RM_CRY, PlayerActor.SysMsgm_PEnvir, PlayerActor.CurrX, PlayerActor.CurrY, 1000,
            //    Settings.Config.btCryMsgFColor, Settings.Config.btCryMsgBColor, string.Format(" - [%s] : %d 分。", Guild.sGuildName, Guild.nContestPoint));
            //SystemShare.WorldEngine.CryCry(Messages.RM_CRY, PlayerActor.SysMsgm_PEnvir, PlayerActor.CurrX, PlayerActor.CurrY, 1000,
            //    Settings.Config.btCryMsgFColor, Settings.Config.btCryMsgBColor, "------------------------------------");
        }
    }
}