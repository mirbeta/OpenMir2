using M2Server.Player;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 此命令用于允许或禁止编组传送功能
    /// </summary>
    [Command("GameGird", "调整指定玩家灵符", 10)]
    public class GameGirdCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            //string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            //string sCtr = @Params.Length > 1 ? @Params[1] : "";
            //int nGameGird = @Params.Length > 2 ? HUtil32.StrToInt(@Params[2],0) : 0;
            //PlayObject m_PlayObject;
            //char Ctr = '1';
            //if ((sCtr != ""))
            //{
            //    Ctr = sCtr[1];
            //}
            //if ((string.IsNullOrEmpty(sHumanName)) || !(new ArrayList(new string[] { "=", "+", "-" }).Contains(Ctr)) || (nGameGird < 0) || (nGameGird > 200000000) || ((!string.IsNullOrEmpty(sHumanName)) && (sHumanName[0] == '?')))
            //{
            //    if (Settings.Config.boGMShowFailMsg)
            //    {
            //        PlayObject.SysMsg(string.Format(Settings.GameCommandParamUnKnow, this.Attributes.Name, Settings.GameCommandGameGirdHelpMsg), MsgColor.c_Red, MsgType.t_Hint);
            //    }
            //    return;
            //}
            //m_PlayObject = M2Share.WorldEngine.GePlayObject(sHumanName);
            //if (m_PlayObject == null)
            //{
            //    PlayObject.SysMsg(string.Format(Settings.NowNotOnLineOrOnOtherServer, new string[] { sHumanName }), MsgColor.c_Red, MsgType.t_Hint);
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
            //if (Settings.g_boGameLogGameGird)
            //{
            //    M2Share.ItemEventSource.AddGameLog(string.Format(Settings.GameLogMsg1, M2Share.LOG_GameGird, m_PlayObject.MapName, m_PlayObject.CurrX, m_PlayObject.CurrY, m_PlayObject.m_sChrName, Settings.Config.sGameGird, m_PlayObject.m_nGAMEGIRD, sCtr[1] + "(" + (nGameGird).ToString() + ")", PlayObject.m_sChrName));
            //}
            //PlayObject.GameGoldChanged();
            //m_PlayObject.SysMsg(string.Format(Settings.GameCommandGameGirdHumanMsg, Settings.Config.sGameGird, nGameGird, m_PlayObject.m_nGAMEGIRD, Settings.Config.sGameGird), MsgColor.c_Green, MsgType.t_Hint);
            //PlayObject.SysMsg(string.Format(Settings.GameCommandGameGirdGMMsg, sHumanName, Settings.Config.sGameGird, nGameGird, m_PlayObject.m_nGAMEGIRD, Settings.Config.sGameGird), MsgColor.c_Green, MsgType.t_Hint);
        }
    }
}