using GameSvr.Player;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 此命令用于允许或禁止编组传送功能
    /// </summary>
    [Command("GameGird", "调整指定玩家灵符", 10)]
    public class GameGirdCommand : Command
    {
        [ExecuteCommand]
        public void GameGird(string[] @Params, PlayObject PlayObject)
        {
            //string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            //string sCtr = @Params.Length > 1 ? @Params[1] : "";
            //int nGameGird = @Params.Length > 2 ? int.Parse(@Params[2]) : 0;
            //TPlayObject m_PlayObject;
            //char Ctr = '1';
            //if ((sCtr != ""))
            //{
            //    Ctr = sCtr[1];
            //}
            //if ((string.IsNullOrEmpty(sHumanName)) || !(new ArrayList(new string[] { "=", "+", "-" }).Contains(Ctr)) || (nGameGird < 0) || (nGameGird > 200000000) || ((!string.IsNullOrEmpty(sHumanName)) && (sHumanName[0] == '?')))
            //{
            //    if (M2Share.g_Config.boGMShowFailMsg)
            //    {
            //        PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, M2Share.g_sGameCommandGameGirdHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
            //    }
            //    return;
            //}
            //m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            //if (m_PlayObject == null)
            //{
            //    PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, new string[] { sHumanName }), TMsgColor.c_Red, TMsgType.t_Hint);
            //    return;
            //}
            //switch (sCtr[1])
            //{
            //    case '=':
            //        m_PlayObject.m_nGAMEGIRD = nGameGird;
            //        break;
            //    case '+':
            //        m_PlayObject.m_nGAMEGIRD += nGameGird;
            //        break;
            //    case '-':
            //        m_PlayObject.m_nGAMEGIRD -= nGameGird;
            //        m_PlayObject.m_UseGameGird = nGameGird;
            //        if (M2Share.g_FunctionNPC != null)  // 灵符使用计数
            //        {
            //            M2Share.g_FunctionNPC.GotoLable(m_PlayObject, "@USEGAMEGIRD", false);
            //        }
            //        break;
            //}
            //if (M2Share.g_boGameLogGameGird)
            //{
            //    M2Share.ItemEventSource.AddGameLog(string.Format(M2Share.g_sGameLogMsg1, M2Share.LOG_GameGird, m_PlayObject.m_sMapName, m_PlayObject.m_nCurrX, m_PlayObject.m_nCurrY, m_PlayObject.m_sChrName, M2Share.g_Config.sGameGird, m_PlayObject.m_nGAMEGIRD, sCtr[1] + "(" + (nGameGird).ToString() + ")", PlayObject.m_sChrName));
            //}
            //PlayObject.GameGoldChanged();
            //m_PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandGameGirdHumanMsg, M2Share.g_Config.sGameGird, nGameGird, m_PlayObject.m_nGAMEGIRD, M2Share.g_Config.sGameGird), TMsgColor.c_Green, TMsgType.t_Hint);
            //PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandGameGirdGMMsg, sHumanName, M2Share.g_Config.sGameGird, nGameGird, m_PlayObject.m_nGAMEGIRD, M2Share.g_Config.sGameGird), TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}