using SystemModule;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 此命令用于允许或禁止编组传送功能
    /// </summary>
    [Command("GameGird", "调整指定玩家灵符", 10)]
    public class GameGirdCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            //string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            //string sCtr = @Params.Length > 1 ? @Params[1] : "";
            //int nGameGird = @Params.Length > 2 ? HUtil32.StrToInt(@Params[2],0) : 0;
            //IPlayerActor m_IPlayerActor;
            //char Ctr = '1';
            //if ((sCtr != ""))
            //{
            //    Ctr = sCtr[1];
            //}
            //if ((string.IsNullOrEmpty(sHumanName)) || !(new ArrayList(new string[] { "=", "+", "-" }).Contains(Ctr)) || (nGameGird < 0) || (nGameGird > 200000000) || ((!string.IsNullOrEmpty(sHumanName)) && (sHumanName[0] == '?')))
            //{
            //    if (Settings.Config.boGMShowFailMsg)
            //    {
            //        PlayerActor.SysMsg(string.Format(Settings.GameCommandParamUnKnow, this.Attributes.Name, Settings.GameCommandGameGirdHelpMsg), MsgColor.c_Red, MsgType.t_Hint);
            //    }
            //    return;
            //}
            //m_IPlayerActor = SystemShare.WorldEngine.GeIPlayerActor(sHumanName);
            //if (m_IPlayerActor == null)
            //{
            //    PlayerActor.SysMsg(string.Format(Settings.NowNotOnLineOrOnOtherServer, new string[] { sHumanName }), MsgColor.c_Red, MsgType.t_Hint);
            //    return;
            //}
            //switch (sCtr[1])
            //{
            //    case '=':
            //        m_IPlayerActor.m_nGAMEGIRD = nGameGird;
            //        break;
            //    case '+':
            //        m_IPlayerActor.m_nGAMEGIRD += nGameGird;
            //        break;
            //    case '-':
            //        m_IPlayerActor.m_nGAMEGIRD -= nGameGird;
            //        m_IPlayerActor.m_UseGameGird = nGameGird;
            //        if (M2Share.g_FunctionNPC != null)  // 灵符使用计数
            //        {
            //            M2Share.g_FunctionNPC.GotoLable(m_IPlayerActor, "@USEGAMEGIRD", false);
            //        }
            //        break;
            //}
            //if (Settings.g_boGameLogGameGird)
            //{
            //    M2Share.ItemEventSource.AddGameLog(string.Format(Settings.GameLogMsg1, M2Share.LOG_GameGird, m_IPlayerActor.MapName, m_IPlayerActor.CurrX, m_IPlayerActor.CurrY, m_IPlayerActor.m_sChrName, Settings.Config.sGameGird, m_IPlayerActor.m_nGAMEGIRD, sCtr[1] + "(" + (nGameGird).ToString() + ")", PlayerActor.SysMsgm_sChrName));
            //}
            //PlayerActor.GameGoldChanged();
            //m_IPlayerActor.SysMsg(string.Format(Settings.GameCommandGameGirdHumanMsg, Settings.Config.sGameGird, nGameGird, m_IPlayerActor.m_nGAMEGIRD, Settings.Config.sGameGird), MsgColor.c_Green, MsgType.t_Hint);
            //PlayerActor.SysMsg(string.Format(Settings.GameCommandGameGirdGMMsg, sHumanName, Settings.Config.sGameGird, nGameGird, m_IPlayerActor.m_nGAMEGIRD, Settings.Config.sGameGird), MsgColor.c_Green, MsgType.t_Hint);
        }
    }
}