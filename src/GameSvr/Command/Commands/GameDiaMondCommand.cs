using GameSvr.Player;

namespace GameSvr.Command.Commands
{
    [GameCommand("GameDiaMond", "调整玩家金刚石", 10)]
    public class GameDiaMondCommand : BaseCommond
    {
        [DefaultCommand]
        public void GameDiaMond(string[] @Params, PlayObject PlayObject)
        {
            //string sHumanName = @Params.Length > 0 ? @Params[1] : "";
            //string sCtr = @Params.Length > 1 ? @Params[2] : "";
            //int nGameDiaMond = @Params.Length > 2 ? Convert.ToInt32(@Params[3]) : 0;
            //var Ctr = '1';
            //if ((PlayObject.m_btPermission < this.Attributes.nPermissionMin))
            //{
            //    PlayObject.SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint); // 权限不够
            //    return;
            //}
            //if ((sCtr != ""))
            //{
            //    Ctr = sCtr[1];
            //}
            //if ((string.IsNullOrEmpty(sHumanName)) || !(new ArrayList(new string[] {"=", "+", "-"}).Contains(Ctr)) ||
            //    (nGameDiaMond < 0) || (nGameDiaMond > 200000000) || ((!string.IsNullOrEmpty(sHumanName)) && (sHumanName[0] == '?')))
            //{
            //    if (M2Share.g_Config.boGMShowFailMsg)
            //    {
            //        PlayObject.SysMsg(
            //            string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name,
            //                M2Share.g_sGameCommandGameDiaMondHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
            //    }
            //    return;
            //}
            //TPlayObject TargetObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            //if (TargetObject == null)
            //{
            //    PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red,
            //        TMsgType.t_Hint);
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
            //if (M2Share.g_boGameLogGameDiaMond)
            //{
            //    M2Share.AddGameDataLog(string.Format(M2Share.g_sGameLogMsg1, M2Share.LOG_GameDiaMond,
            //        TargetObject.m_sMapName,
            //        TargetObject.m_nCurrX, TargetObject.m_nCurrY, TargetObject.m_sChrName,
            //        M2Share.g_Config.sGameDiaMond,
            //        TargetObject.m_nGAMEDIAMOND, sCtr[1] + "(" + (nGameDiaMond).ToString() + ")",
            //        PlayObject.m_sChrName));
            //}
            //TargetObject.GameGoldChanged();
            //TargetObject.SysMsg(
            //    string.Format(M2Share.g_sGameCommandGameDiaMondHumanMsg, M2Share.g_Config.sGameDiaMond, nGameDiaMond,
            //        PlayObject.m_nGAMEDIAMOND, M2Share.g_Config.sGameDiaMond), TMsgColor.c_Green, TMsgType.t_Hint);
            //PlayObject.SysMsg(
            //    string.Format(M2Share.g_sGameCommandGameDiaMondGMMsg, sHumanName, M2Share.g_Config.sGameDiaMond,
            //        nGameDiaMond, PlayObject.m_nGAMEDIAMOND, M2Share.g_Config.sGameDiaMond), TMsgColor.c_Green,
            //    TMsgType.t_Hint);
        }
    }
}