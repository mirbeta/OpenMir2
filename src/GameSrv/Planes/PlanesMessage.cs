using GameSrv.DataSource;
using M2Server;
using M2Server.Player;
using SystemModule.Enums;

namespace GameSrv.Planes
{
    public class PlanesMessage
    {
        private readonly PlayObject PlayObject = null;

        public PlanesMessage()
        {

        }

        public void ProcessData(int ident, int serverNum, string body)
        {
            switch (ident)
            {
                case Messages.ISM_GROUPSERVERHEART:
                    ServerHeartMessage(serverNum, body);
                    break;
                case Messages.ISM_USERSERVERCHANGE:
                    MsgGetUserServerChange(serverNum, body);
                    break;
                case Messages.ISM_CHANGESERVERRECIEVEOK:
                    MsgGetUserChangeServerRecieveOk(serverNum, body);
                    break;
                case Messages.ISM_USERLOGON:
                    MsgGetUserLogon(serverNum, body);
                    break;
                case Messages.ISM_USERLOGOUT:
                    MsgGetUserLogout(serverNum, body);
                    break;
                case Messages.ISM_WHISPER:
                    MsgGetWhisper(serverNum, body);
                    break;
                case Messages.ISM_GMWHISPER:
                    MsgGetGMWhisper(serverNum, body);
                    break;
                case Messages.ISM_LM_WHISPER:
                    MsgGetLoverWhisper(serverNum, body);
                    break;
                case Messages.ISM_SYSOPMSG:
                    MsgGetSysopMsg(serverNum, body);
                    break;
                case Messages.ISM_ADDGUILD:
                    MsgGetAddGuild(serverNum, body);
                    break;
                case Messages.ISM_DELGUILD:
                    MsgGetDelGuild(serverNum, body);
                    break;
                case Messages.ISM_RELOADGUILD:
                    MsgGetReloadGuild(serverNum, body);
                    break;
                case Messages.ISM_GUILDMSG:
                    MsgGetGuildMsg(serverNum, body);
                    break;
                case Messages.ISM_GUILDWAR:
                    MsgGetGuildWarInfo(serverNum, body);
                    break;
                case Messages.ISM_CHATPROHIBITION:
                    MsgGetChatProhibition(serverNum, body);
                    break;
                case Messages.ISM_CHATPROHIBITIONCANCEL:
                    MsgGetChatProhibitionCancel(serverNum, body);
                    break;
                case Messages.ISM_CHANGECASTLEOWNER:
                    MsgGetChangeCastleOwner(serverNum, body);
                    break;
                case Messages.ISM_RELOADCASTLEINFO:
                    MsgGetReloadCastleAttackers(serverNum);
                    break;
                case Messages.ISM_RELOADADMIN:
                    MsgGetReloadAdmin();
                    break;
                case Messages.ISM_MARKETOPEN:
                    MsgGetMarketOpen(true);
                    break;
                case Messages.ISM_MARKETCLOSE:
                    MsgGetMarketOpen(false);
                    break;
                case Messages.ISM_RELOADCHATLOG:
                    MsgGetReloadChatLog();
                    break;
                case Messages.ISM_USER_INFO:
                case Messages.ISM_FRIEND_INFO:
                case Messages.ISM_FRIEND_DELETE:
                case Messages.ISM_FRIEND_OPEN:
                case Messages.ISM_FRIEND_CLOSE:
                case Messages.ISM_FRIEND_RESULT:
                case Messages.ISM_TAG_SEND:
                case Messages.ISM_TAG_RESULT:
                    MsgGetUserMgr(serverNum, body, ident);
                    break;
                case Messages.ISM_RELOADMAKEITEMLIST:
                    MsgGetReloadMakeItemList();
                    break;
                case Messages.ISM_GUILDMEMBER_RECALL:
                    MsgGetGuildMemberRecall(serverNum, body);
                    break;
                case Messages.ISM_RELOADGUILDAGIT:
                    MsgGetReloadGuildAgit(serverNum, body);
                    break;
                case Messages.ISM_LM_LOGIN:
                    MsgGetLoverLogin(serverNum, body);
                    break;
                case Messages.ISM_LM_LOGOUT:
                    MsgGetLoverLogout(serverNum, body);
                    break;
                case Messages.ISM_LM_LOGIN_REPLY:
                    MsgGetLoverLoginReply(serverNum, body);
                    break;
                case Messages.ISM_LM_KILLED_MSG:
                    MsgGetLoverKilledMsg(serverNum, body);
                    break;
                case Messages.ISM_RECALL:
                    MsgGetRecall(serverNum, body);
                    break;
                case Messages.ISM_REQUEST_RECALL:
                    MsgGetRequestRecall(serverNum, body);
                    break;
                case Messages.ISM_REQUEST_LOVERRECALL:
                    MsgGetRequestLoverRecall(serverNum, body);
                    break;
                case Messages.ISM_GRUOPMESSAGE:
                    M2Share.Logger.Info("跨服消息");
                    break;
            }
        }

        private static void ServerHeartMessage(int sNu, string Body)
        {

        }

        private static void MsgGetUserServerChange(int sNum, string Body)
        {
            const string sExceptionMsg = "[Exception] TFrmSrvMsg::MsgGetUserServerChange";
            int shifttime = HUtil32.GetTickCount();
            string ufilename = Body;
            if (M2Share.ServerIndex == sNum)
            {
                try
                {
                    // M2Share.WorldEngine.AddSwitchData(new SwitchDataInfo());
                    M2Share.WorldEngine.SendServerGroupMsg(Messages.ISM_CHANGESERVERRECIEVEOK, M2Share.ServerIndex, ufilename);
                }
                catch
                {
                    M2Share.Logger.Error(sExceptionMsg);
                }
            }
        }

        private static void MsgGetUserChangeServerRecieveOk(int sNum, string Body)
        {
            string ufilename = Body;
            //M2Share.WorldEngine.GetIsmChangeServerReceive(ufilename);
        }

        private static void MsgGetUserLogon(int sNum, string Body)
        {
            string uname = Body;
            //M2Share.WorldEngine.OtherServerUserLogon(sNum, uname);
        }

        private static void MsgGetUserLogout(int sNum, string Body)
        {
            string uname = Body;
            //M2Share.WorldEngine.OtherServerUserLogout(sNum, uname);
        }

        private static void MsgGetWhisper(int sNum, string Body)
        {
            string uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                string Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                IPlayerActor hum = M2Share.WorldEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    if (hum.HearWhisper)
                    {
                        //hum.WhisperRe(Str, 1);
                    }
                }
            }
        }

        private static void MsgGetGMWhisper(int sNum, string Body)
        {
            string uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                string Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                IPlayerActor hum = M2Share.WorldEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    if (hum.HearWhisper)
                    {
                        // hum.WhisperRe(Str, 0);
                    }
                }
            }
        }

        private static void MsgGetLoverWhisper(int sNum, string Body)
        {
            string uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                string Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                IPlayerActor hum = M2Share.WorldEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    if (hum.HearWhisper)
                    {
                        //   hum.WhisperRe(Str, 2);
                    }
                }
            }
        }

        private static void MsgGetSysopMsg(int sNum, string Body)
        {
            M2Share.WorldEngine.SendBroadCastMsg(Body, MsgType.System);
        }

        private static void MsgGetAddGuild(int sNum, string Body)
        {
            string gname = string.Empty;
            string mname = HUtil32.GetValidStr3(Body, ref gname, HUtil32.Backslash);
            M2Share.GuildMgr.AddGuild(gname, mname);
        }

        private static void MsgGetDelGuild(int sNum, string Body)
        {
            string gname = Body;
            M2Share.GuildMgr.DelGuild(gname);
        }

        private static void MsgGetReloadGuild(int sNum, string Body)
        {
            string gname = Body;
            //GuildInfo guild;
            //if (sNum == 0) {
            //    guild = M2Share.GuildMgr.FindGuild(gname);
            //    if (guild != null) {
            //        guild.LoadGuild();
            //        M2Share.WorldEngine.GuildMemberReGetRankName(guild);
            //    }
            //}
            //else if (M2Share.ServerIndex != sNum) {
            //    guild = M2Share.GuildMgr.FindGuild(gname);
            //    if (guild != null) {
            //        guild.LoadGuildFile(gname + '.' + sNum);
            //        M2Share.WorldEngine.GuildMemberReGetRankName(guild);
            //        guild.SaveGuildInfoFile();
            //    }
            //}
        }

        private static void MsgGetGuildMsg(int sNum, string Body)
        {
            //string gname = string.Empty;
            //string Str = Body;
            //Str = HUtil32.GetValidStr3(Str, ref gname, HUtil32.Backslash);
            //if (!string.IsNullOrEmpty(gname)) {
            //    GuildInfo g = M2Share.GuildMgr.FindGuild(gname);
            //    if (g != null) {
            //        g.SendGuildMsg(Str);
            //    }
            //}
        }

        private static void MsgGetGuildWarInfo(int sNum, string Body)
        {
            //string Str;
            //string gname = string.Empty;
            //string warguildname = string.Empty;
            //string StartTime = string.Empty;
            //string remaintime = string.Empty;
            //GuildInfo g;
            //GuildInfo WarGuild;
            //WarGuild pgw = default;
            //if (sNum == 0) {
            //    Str = Body;
            //    Str = HUtil32.GetValidStr3(Str, ref gname, HUtil32.Backslash);
            //    Str = HUtil32.GetValidStr3(Str, ref warguildname, HUtil32.Backslash);
            //    Str = HUtil32.GetValidStr3(Str, ref StartTime, HUtil32.Backslash);
            //    remaintime = Str;
            //    if (!string.IsNullOrEmpty(gname) && !string.IsNullOrEmpty(warguildname)) {
            //        g = M2Share.GuildMgr.FindGuild(gname);
            //        WarGuild = M2Share.GuildMgr.FindGuild(warguildname);
            //        if (g != null && WarGuild != null) {
            //            int currenttick = HUtil32.GetTickCount();
            //            if (M2Share.ServerTickDifference == 0) {
            //                M2Share.ServerTickDifference = Convert.ToInt32(StartTime) - currenttick;
            //            }
            //            for (int i = 0; i < g.GuildWarList.Count; i++) {
            //                pgw = g.GuildWarList[i];
            //                if (pgw.dwWarTick > 0) {
            //                    if (pgw.Guild == WarGuild) {
            //                        pgw.Guild = WarGuild;
            //                        pgw.dwWarTick = Convert.ToInt32(StartTime) - M2Share.ServerTickDifference;
            //                        pgw.dwWarTime = Convert.ToInt32(remaintime);
            //                        M2Share.Logger.Info("[行会战] " + g.GuildName + "<->" + WarGuild.GuildName + ", 开战: " + StartTime + ", 持久: " + remaintime + ", 现在: " + pgw.dwWarTick + ", 时差: " + M2Share.ServerTickDifference);
            //                        break;
            //                    }
            //                }
            //            }
            //            if (pgw.dwWarTick > 0) {
            //                if (!g.GuildWarList.Select(x => x.Guild).Contains(WarGuild)) {
            //                    pgw = new WarGuild();
            //                    pgw.Guild = WarGuild;
            //                    pgw.dwWarTick = int.Parse(StartTime) - M2Share.ServerTickDifference;
            //                    pgw.dwWarTime = int.Parse(remaintime);
            //                    g.GuildWarList.Add(pgw);
            //                }
            //                M2Share.Logger.Info("[行会战] " + g.GuildName + "<->" + WarGuild.GuildName + ", 开战: " + StartTime + ", 持久: " + remaintime + ", 现在: " + (Convert.ToUInt32(StartTime) - M2Share.ServerTickDifference) + ", 时差: " + M2Share.ServerTickDifference);
            //            }
            //            g.RefMemberName();
            //            g.UpdateGuildFile();
            //        }
            //    }
            //}
        }

        private void MsgGetChatProhibition(int sNum, string Body)
        {
            string whostr = string.Empty;
            string minstr = string.Empty;
            string Str = Body;
            Str = HUtil32.GetValidStr3(Str, ref whostr, HUtil32.Backslash);
            Str = HUtil32.GetValidStr3(Str, ref minstr, HUtil32.Backslash);
            if (!string.IsNullOrEmpty(whostr))
            {
                //PlayObject.CmdShutup(Settings.g_GameCommand.SHUTUP, whostr, minstr);
                //CommandMgr.Execute(PlayObject, "Shutup");
            }
        }

        private static void MsgGetChatProhibitionCancel(int sNum, string Body)
        {
            string whostr = Body;
            if (!string.IsNullOrEmpty(whostr))
            {
                //PlayObject.CmdShutup(Settings.g_GameCommand.SHUTUP, whostr, "");
            }
        }

        private static void MsgGetChangeCastleOwner(int sNum, string Body)
        {
            throw new Exception("TODO MsgGetChangeCastleOwner...");
        }

        private static void MsgGetReloadCastleAttackers(int sNum)
        {
            M2Share.CastleMgr.Initialize();
        }

        private static void MsgGetReloadAdmin()
        {
            LocalDb.LoadAdminList();
        }

        private static void MsgGetReloadChatLog()
        {
            // FrmDB.LoadChatLogFiles;
        }

        private static void MsgGetUserMgr(int sNum, string Body, int Ident_)
        {
            string UserName = string.Empty;
            string Str = Body;
            string msgbody = HUtil32.GetValidStr3(Str, ref UserName, HUtil32.Backslash);
            // UserMgrEngine.OnExternInterMsg(sNum, Ident_, UserName, msgbody);
        }

        private static void MsgGetReloadMakeItemList()
        {
            //M2Share.LocalDB.LoadMakeItemList();
            GameShare.LocalDb.LoadMakeItem();
        }

        private static void MsgGetGuildMemberRecall(int sNum, string Body)
        {
            string dxstr = string.Empty;
            string dystr = string.Empty;
            string uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                string Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                Str = HUtil32.GetValidStr3(Str, ref dxstr, HUtil32.Backslash);
                Str = HUtil32.GetValidStr3(Str, ref dystr, HUtil32.Backslash);
                short dx = HUtil32.StrToInt16(dxstr, 0);
                short dy = HUtil32.StrToInt16(dystr, 0);
                //PlayObject hum = M2Share.WorldEngine.GetPlayObject(uname);
                //if (hum != null) {
                //    if (hum.AllowGuildReCall) {
                //        hum.SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                //        hum.SpaceMove(Str, dx, dy, 0);
                //    }
                //}
            }
        }

        private static void MsgGetReloadGuildAgit(int sNum, string Body)
        {
            // GuildAgitMan.ClearGuildAgitList;
            // GuildAgitMan.LoadGuildAgitList;
        }

        private static void MsgGetLoverLogin(int sNum, string Body)
        {
            string Str;
            string uname = string.Empty;
            string lovername = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                Str = HUtil32.GetValidStr3(Str, ref lovername, HUtil32.Backslash);
                //humlover = M2Share.WorldEngine.GetPlayObject(lovername);
                //if (humlover != null) {
                //    int svidx = 0;
                //    if (M2Share.WorldEngine.FindOtherServerUser(uname, ref svidx)) {
                //        WorldServer.SendServerGroupMsg(Messages.ISM_LM_LOGIN_REPLY, svidx, lovername + '/' + uname + '/' + humlover.Envir.MapDesc);
                //    }
                //}
            }
        }

        private static void MsgGetLoverLogout(int sNum, string Body)
        {
            string uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                string Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                string lovername = Str;
                //PlayObject hum = M2Share.WorldEngine.GetPlayObject(lovername);
                //if (hum != null) {
                //    hum.SysMsg(uname + sLoverFindYouMsg, MsgColor.Red, MsgType.Hint);
                //}
            }
        }

        private static void MsgGetLoverLoginReply(int sNum, string Body)
        {
        }

        private static void MsgGetLoverKilledMsg(int sNum, string Body)
        {
            string uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                string Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                //PlayObject hum = M2Share.WorldEngine.GetPlayObject(uname);
                //if (hum != null) {
                //    hum.SysMsg(Str, MsgColor.Red, MsgType.Hint);
                //}
            }
        }

        private static void MsgGetRecall(int sNum, string Body)
        {
            string dxstr = string.Empty;
            string dystr = string.Empty;
            string uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                string Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                Str = HUtil32.GetValidStr3(Str, ref dxstr, HUtil32.Backslash);
                Str = HUtil32.GetValidStr3(Str, ref dystr, HUtil32.Backslash);
                short dx = HUtil32.StrToInt16(dxstr, 0);
                short dy = HUtil32.StrToInt16(dystr, 0);
                //PlayObject hum = M2Share.WorldEngine.GetPlayObject(uname);
                //if (hum != null) {
                //    hum.SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                //    hum.SpaceMove(Str, dx, dy, 0);
                //}
            }
        }

        private static void MsgGetRequestRecall(int sNum, string Body)
        {
            string uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                string Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                //PlayObject hum = M2Share.WorldEngine.GetPlayObject(uname);
                //if (hum != null) {
                //    hum.RecallHuman(Str);
                //}
            }
        }

        private static void MsgGetRequestLoverRecall(int sNum, string Body)
        {
            string uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                string Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                //PlayObject hum = M2Share.WorldEngine.GetPlayObject(uname);
                //if (hum != null) {
                //    if (!hum.Envir.Flag.NoReCall) {
                //        hum.RecallHuman(Str);
                //    }
                //}
            }
        }

        private static void MsgGetMarketOpen(bool WantOpen)
        {
            // SQLEngine.Open(WantOpen);
        }
    }
}