using SystemModule;

namespace CommandSystem {
    [Command("GameDiaMond", "调整玩家金刚石", 10)]
    public class GameDiaMondCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor) {
            //string sHumanName = @Params.Length > 0 ? @Params[1] : "";
            //string sCtr = @Params.Length > 1 ? @Params[2] : "";
            //int nGameDiaMond = @Params.Length > 2 ? HUtil32.StrToInt(@Params[3]) : 0;
            //var Ctr = '1';
            //if ((PlayerActor.SysMsgm_btPermission < this.Attributes.nPermissionMin))
            //{
            //    PlayerActor.SysMsg(Settings.GameCommandPermissionTooLow, MsgColor.c_Red, MsgType.t_Hint); // 权限不够
            //    return;
            //}
            //if ((sCtr != ""))
            //{
            //    Ctr = sCtr[1];
            //}
            //if ((string.IsNullOrEmpty(sHumanName)) || !(new ArrayList(new string[] {"=", "+", "-"}).Contains(Ctr)) ||
            //    (nGameDiaMond < 0) || (nGameDiaMond > 200000000) || ((!string.IsNullOrEmpty(sHumanName)) && (sHumanName[0] == '?')))
            //{
            //    if (Settings.Config.boGMShowFailMsg)
            //    {
            //        PlayerActor.SysMsg(
            //            string.Format(Settings.GameCommandParamUnKnow, this.Attributes.Name,
            //                Settings.GameCommandGameDiaMondHelpMsg), MsgColor.c_Red, MsgType.t_Hint);
            //    }
            //    return;
            //}
            //IPlayerActor TargetObject = M2Share.WorldEngine.GeIPlayerActor(sHumanName);
            //if (TargetObject == null)
            //{
            //    PlayerActor.SysMsg(string.Format(Settings.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.c_Red,
            //        MsgType.t_Hint);
            //    return;
            //}
            //switch (sCtr[1])
            //{
            //    case '=':
            //        TargetObject.m_nGAMEDIAMOND = nGameDiaMond;
            //        break;
            //    case '+':
            //        TargetObject.m_nGAMEDIAMOND += nGameDiaMond;
            //        break;
            //    case '-':
            //        TargetObject.m_nGAMEDIAMOND -= nGameDiaMond;
            //        break;
            //}
            //if (Settings.g_boGameLogGameDiaMond)
            //{
            //    M2Share.ItemEventSource.AddGameLog(string.Format(Settings.GameLogMsg1, M2Share.LOG_GameDiaMond,
            //        TargetObject.MapName,
            //        TargetObject.CurrX, TargetObject.CurrY, TargetObject.m_sChrName,
            //        Settings.Config.sGameDiaMond,
            //        TargetObject.m_nGAMEDIAMOND, sCtr[1] + "(" + (nGameDiaMond).ToString() + ")",
            //        PlayerActor.SysMsgm_sChrName));
            //}
            //TargetObject.GameGoldChanged();
            //TargetObject.SysMsg(
            //    string.Format(Settings.GameCommandGameDiaMondHumanMsg, Settings.Config.sGameDiaMond, nGameDiaMond,
            //        PlayerActor.SysMsgm_nGAMEDIAMOND, Settings.Config.sGameDiaMond), MsgColor.c_Green, MsgType.t_Hint);
            //PlayerActor.SysMsg(
            //    string.Format(Settings.GameCommandGameDiaMondGMMsg, sHumanName, Settings.Config.sGameDiaMond,
            //        nGameDiaMond, PlayerActor.SysMsgm_nGAMEDIAMOND, Settings.Config.sGameDiaMond), MsgColor.c_Green,
            //    MsgType.t_Hint);
        }
    }
}