using GameSvr.Player;

namespace GameSvr.Command.Commands
{
    [Command("GameGlory", "调整玩家灵符", 10)]
    public class GameGloryCommand : Command
    {
        [ExecuteCommand]
        public void GameGlory(string[] @Params, PlayObject PlayObject)
        {
            //string sHumanName = @Params.Length > 0 ? @Params[1] : "";
            //string sCtr = @Params.Length > 1 ? @Params[2] : "";
            //int nGameGlory = @Params.Length > 2 ? Convert.ToInt32(@Params[3]) : 0;
            //var Ctr = '1';
            //if ((PlayObject.m_btPermission < this.Attributes.nPermissionMin))
            //{
            //    PlayObject.SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint); // 权限不够
            //    return;
            //}

            //if ((sCtr != ""))
            //{
            //    Ctr = sCtr[0];
            //}

            //if ((string.IsNullOrEmpty(sHumanName)) || !(new ArrayList(new string[] { "=", "+", "-" }).Contains(Ctr)) ||
            //    (nGameGlory < 0) || (nGameGlory > 255) || ((!string.IsNullOrEmpty(sHumanName)) && (sHumanName[0] == '?')))
            //{
            //    {
            //        PlayObject.SysMsg(
            //            string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name,
            //                M2Share.g_sGameCommandGameGloryHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
            //        return;
            //    }

            //    var TargerObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            //    if (TargerObject == null)
            //    {
            //        PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, new string[] { sHumanName }),
            //            TMsgColor.c_Red, TMsgType.t_Hint);
            //        return;
            //    }

            //    switch (sCtr[0])
            //    {
            //        case '=':
            //            TargerObject.m_btGameGlory = (byte)nGameGlory;
            //            break;
            //        case '+':
            //            TargerObject.m_btGameGlory += (byte)nGameGlory;
            //            break;
            //        case '-':
            //            TargerObject.m_btGameGlory -= (byte)nGameGlory;
            //            break;
            //    }

            //    if (M2Share.g_boGameLogGameGlory)
            //    {
            //        M2Share.AddGameDataLog(string.Format(M2Share.g_sGameLogMsg1, M2Share.LOG_GameGlory,
            //            TargerObject.m_sMapName, TargerObject.m_nCurrX, TargerObject.m_nCurrY, TargerObject.m_sChrName,
            //            M2Share.g_Config.sGameGlory, TargerObject.m_btGameGlory,
            //            sCtr[1] + "(" + (nGameGlory).ToString() + ")", PlayObject.m_sChrName));
            //    }

            //    TargerObject.GameGloryChanged();
            //    TargerObject.SysMsg(
            //        string.Format(M2Share.g_sGameCommandGameGirdHumanMsg, M2Share.g_Config.sGameGlory, nGameGlory,
            //            TargerObject.m_btGameGlory, M2Share.g_Config.sGameGlory), TMsgColor.c_Green, TMsgType.t_Hint);
            //    PlayObject.SysMsg(
            //        string.Format(M2Share.g_sGameCommandGameGirdGMMsg, sHumanName, M2Share.g_Config.sGameGlory,
            //            nGameGlory, TargerObject.m_btGameGlory, M2Share.g_Config.sGameGlory), TMsgColor.c_Green,
            //        TMsgType.t_Hint);
            //}
        }
    }
}