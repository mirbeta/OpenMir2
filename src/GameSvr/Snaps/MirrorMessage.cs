using GameSvr.Guild;
using GameSvr.Player;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Snaps
{
    public class SpapsMessage
    {
        private readonly PlayObject PlayObject = null;

        public SpapsMessage()
        {

        }

        public void ProcessData(int ident, int serverNum, string body)
        {
            switch (ident)
            {
                case Grobal2.ISM_GROUPSERVERHEART:
                    ServerHeartMessage(serverNum, body);
                    break;
                case Grobal2.ISM_USERSERVERCHANGE:
                    MsgGetUserServerChange(serverNum, body);
                    break;
                case Grobal2.ISM_CHANGESERVERRECIEVEOK:
                    MsgGetUserChangeServerRecieveOk(serverNum, body);
                    break;
                case Grobal2.ISM_USERLOGON:
                    MsgGetUserLogon(serverNum, body);
                    break;
                case Grobal2.ISM_USERLOGOUT:
                    MsgGetUserLogout(serverNum, body);
                    break;
                case Grobal2.ISM_WHISPER:
                    MsgGetWhisper(serverNum, body);
                    break;
                case Grobal2.ISM_GMWHISPER:
                    MsgGetGMWhisper(serverNum, body);
                    break;
                case Grobal2.ISM_LM_WHISPER:
                    MsgGetLoverWhisper(serverNum, body);
                    break;
                case Grobal2.ISM_SYSOPMSG:
                    MsgGetSysopMsg(serverNum, body);
                    break;
                case Grobal2.ISM_ADDGUILD:
                    MsgGetAddGuild(serverNum, body);
                    break;
                case Grobal2.ISM_DELGUILD:
                    MsgGetDelGuild(serverNum, body);
                    break;
                case Grobal2.ISM_RELOADGUILD:
                    MsgGetReloadGuild(serverNum, body);
                    break;
                case Grobal2.ISM_GUILDMSG:
                    MsgGetGuildMsg(serverNum, body);
                    break;
                case Grobal2.ISM_GUILDWAR:
                    MsgGetGuildWarInfo(serverNum, body);
                    break;
                case Grobal2.ISM_CHATPROHIBITION:
                    MsgGetChatProhibition(serverNum, body);
                    break;
                case Grobal2.ISM_CHATPROHIBITIONCANCEL:
                    MsgGetChatProhibitionCancel(serverNum, body);
                    break;
                case Grobal2.ISM_CHANGECASTLEOWNER:
                    MsgGetChangeCastleOwner(serverNum, body);
                    break;
                case Grobal2.ISM_RELOADCASTLEINFO:
                    MsgGetReloadCastleAttackers(serverNum);
                    break;
                case Grobal2.ISM_RELOADADMIN:
                    MsgGetReloadAdmin();
                    break;
                case Grobal2.ISM_MARKETOPEN:
                    MsgGetMarketOpen(true);
                    break;
                case Grobal2.ISM_MARKETCLOSE:
                    MsgGetMarketOpen(false);
                    break;
                case Grobal2.ISM_RELOADCHATLOG:
                    MsgGetReloadChatLog();
                    break;
                case Grobal2.ISM_USER_INFO:
                case Grobal2.ISM_FRIEND_INFO:
                case Grobal2.ISM_FRIEND_DELETE:
                case Grobal2.ISM_FRIEND_OPEN:
                case Grobal2.ISM_FRIEND_CLOSE:
                case Grobal2.ISM_FRIEND_RESULT:
                case Grobal2.ISM_TAG_SEND:
                case Grobal2.ISM_TAG_RESULT:
                    MsgGetUserMgr(serverNum, body, ident);
                    break;
                case Grobal2.ISM_RELOADMAKEITEMLIST:
                    MsgGetReloadMakeItemList();
                    break;
                case Grobal2.ISM_GUILDMEMBER_RECALL:
                    MsgGetGuildMemberRecall(serverNum, body);
                    break;
                case Grobal2.ISM_RELOADGUILDAGIT:
                    MsgGetReloadGuildAgit(serverNum, body);
                    break;
                case Grobal2.ISM_LM_LOGIN:
                    MsgGetLoverLogin(serverNum, body);
                    break;
                case Grobal2.ISM_LM_LOGOUT:
                    MsgGetLoverLogout(serverNum, body);
                    break;
                case Grobal2.ISM_LM_LOGIN_REPLY:
                    MsgGetLoverLoginReply(serverNum, body);
                    break;
                case Grobal2.ISM_LM_KILLED_MSG:
                    MsgGetLoverKilledMsg(serverNum, body);
                    break;
                case Grobal2.ISM_RECALL:
                    MsgGetRecall(serverNum, body);
                    break;
                case Grobal2.ISM_REQUEST_RECALL:
                    MsgGetRequestRecall(serverNum, body);
                    break;
                case Grobal2.ISM_REQUEST_LOVERRECALL:
                    MsgGetRequestLoverRecall(serverNum, body);
                    break;
                case Grobal2.ISM_GRUOPMESSAGE:
                    Console.WriteLine("跨服消息");
                    break;
            }
        }

        private void ServerHeartMessage(int sNu, string Body)
        {

        }

        private void MsgGetUserServerChange(int sNum, string Body)
        {
            const string sExceptionMsg = "[Exception] TFrmSrvMsg::MsgGetUserServerChange";
            int shifttime = HUtil32.GetTickCount();
            string ufilename = Body;
            if (M2Share.ServerIndex == sNum)
            {
                try
                {
                    M2Share.UserEngine.AddSwitchData(new TSwitchDataInfo());
                    M2Share.UserEngine.SendServerGroupMsg(Grobal2.ISM_CHANGESERVERRECIEVEOK, M2Share.ServerIndex, ufilename);
                }
                catch
                {
                    M2Share.Log.Error(sExceptionMsg);
                }
            }
        }

        private void MsgGetUserChangeServerRecieveOk(int sNum, string Body)
        {
            var ufilename = Body;
            M2Share.UserEngine.GetIsmChangeServerReceive(ufilename);
        }

        private void MsgGetUserLogon(int sNum, string Body)
        {
            var uname = Body;
            M2Share.UserEngine.OtherServerUserLogon(sNum, uname);
        }

        private void MsgGetUserLogout(int sNum, string Body)
        {
            var uname = Body;
            M2Share.UserEngine.OtherServerUserLogout(sNum, uname);
        }

        private void MsgGetWhisper(int sNum, string Body)
        {
            var uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                var Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                PlayObject hum = M2Share.UserEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    if (hum.HearWhisper)
                    {
                        hum.WhisperRe(Str, 1);
                    }
                }
            }
        }

        private void MsgGetGMWhisper(int sNum, string Body)
        {
            var uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                var Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                PlayObject hum = M2Share.UserEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    if (hum.HearWhisper)
                    {
                        hum.WhisperRe(Str, 0);
                    }
                }
            }
        }

        private void MsgGetLoverWhisper(int sNum, string Body)
        {
            var uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                var Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                PlayObject hum = M2Share.UserEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    if (hum.HearWhisper)
                    {
                        hum.WhisperRe(Str, 2);
                    }
                }
            }
        }

        private void MsgGetSysopMsg(int sNum, string Body)
        {
            M2Share.UserEngine.SendBroadCastMsg(Body, MsgType.System);
        }

        private void MsgGetAddGuild(int sNum, string Body)
        {
            var gname = string.Empty;
            var mname = HUtil32.GetValidStr3(Body, ref gname, HUtil32.Backslash);
            M2Share.GuildMgr.AddGuild(gname, mname);
        }

        private void MsgGetDelGuild(int sNum, string Body)
        {
            var gname = Body;
            M2Share.GuildMgr.DelGuild(gname);
        }

        private void MsgGetReloadGuild(int sNum, string Body)
        {
            var gname = Body;
            GuildInfo guild;
            if (sNum == 0)
            {
                guild = M2Share.GuildMgr.FindGuild(gname);
                if (guild != null)
                {
                    guild.LoadGuild();
                    M2Share.UserEngine.GuildMemberReGetRankName(guild);
                }
            }
            else if (M2Share.ServerIndex != sNum)
            {
                guild = M2Share.GuildMgr.FindGuild(gname);
                if (guild != null)
                {
                    guild.LoadGuildFile(gname + '.' + sNum);
                    M2Share.UserEngine.GuildMemberReGetRankName(guild);
                    guild.SaveGuildInfoFile();
                }
            }
        }

        private void MsgGetGuildMsg(int sNum, string Body)
        {
            var gname = string.Empty;
            string Str = Body;
            Str = HUtil32.GetValidStr3(Str, ref gname, HUtil32.Backslash);
            if (gname != "")
            {
                var g = M2Share.GuildMgr.FindGuild(gname);
                if (g != null)
                {
                    g.SendGuildMsg(Str);
                }
            }
        }

        private void MsgGetGuildWarInfo(int sNum, string Body)
        {
            string Str;
            var gname = string.Empty;
            var warguildname = string.Empty;
            var StartTime = string.Empty;
            var remaintime = string.Empty;
            GuildInfo g;
            GuildInfo WarGuild;
            TWarGuild pgw;
            if (sNum == 0)
            {
                Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref gname, HUtil32.Backslash);
                Str = HUtil32.GetValidStr3(Str, ref warguildname, HUtil32.Backslash);
                Str = HUtil32.GetValidStr3(Str, ref StartTime, HUtil32.Backslash);
                remaintime = Str;
                if (gname != "" && warguildname != "")
                {
                    g = M2Share.GuildMgr.FindGuild(gname);
                    WarGuild = M2Share.GuildMgr.FindGuild(warguildname);
                    if (g != null && WarGuild != null)
                    {
                        int currenttick = HUtil32.GetTickCount();
                        if (M2Share.g_nServerTickDifference == 0)
                        {
                            M2Share.g_nServerTickDifference = Convert.ToInt32(StartTime) - currenttick;
                        }
                        pgw = null;
                        for (var i = 0; i < g.GuildWarList.Count; i++)
                        {
                            pgw = g.GuildWarList[i];
                            if (pgw != null)
                            {
                                if (pgw.Guild == WarGuild)
                                {
                                    pgw.Guild = WarGuild;
                                    pgw.dwWarTick = Convert.ToInt32(StartTime) - M2Share.g_nServerTickDifference;
                                    pgw.dwWarTime = Convert.ToInt32(remaintime);
                                    M2Share.Log.Info("[行会战] " + g.sGuildName + "<->" + WarGuild.sGuildName + ", 开战: " + StartTime + ", 持久: " + remaintime + ", 现在: " + pgw.dwWarTick + ", 时差: " + M2Share.g_nServerTickDifference);
                                    break;
                                }
                            }
                        }
                        if (pgw == null)
                        {
                            if (!g.GuildWarList.Select(x => x.Guild).Contains(WarGuild))
                            {
                                pgw = new TWarGuild();
                                pgw.Guild = WarGuild;
                                pgw.dwWarTick = int.Parse(StartTime) - M2Share.g_nServerTickDifference;
                                pgw.dwWarTime = int.Parse(remaintime);
                                g.GuildWarList.Add(pgw);
                            }
                            M2Share.Log.Info("[行会战] " + g.sGuildName + "<->" + WarGuild.sGuildName + ", 开战: " + StartTime + ", 持久: " + remaintime + ", 现在: " + (Convert.ToUInt32(StartTime) - M2Share.g_nServerTickDifference) + ", 时差: " + M2Share.g_nServerTickDifference);
                        }
                        g.RefMemberName();
                        g.UpdateGuildFile();
                    }
                }
            }
        }

        private void MsgGetChatProhibition(int sNum, string Body)
        {
            var whostr = string.Empty;
            var minstr = string.Empty;
            string Str = Body;
            Str = HUtil32.GetValidStr3(Str, ref whostr, HUtil32.Backslash);
            Str = HUtil32.GetValidStr3(Str, ref minstr, HUtil32.Backslash);
            if (whostr != "")
            {
                //PlayObject.CmdShutup(M2Share.g_GameCommand.SHUTUP, whostr, minstr);
                M2Share.CommandMgr.ExecCmd("Shutup", PlayObject);
            }
        }

        private void MsgGetChatProhibitionCancel(int sNum, string Body)
        {
            var whostr = Body;
            if (whostr != "")
            {
                //PlayObject.CmdShutup(M2Share.g_GameCommand.SHUTUP, whostr, "");
            }
        }

        private void MsgGetChangeCastleOwner(int sNum, string Body)
        {
            throw new Exception("TODO MsgGetChangeCastleOwner...");
        }

        private void MsgGetReloadCastleAttackers(int sNum)
        {
            M2Share.CastleMgr.Initialize();
        }

        private void MsgGetReloadAdmin()
        {
            M2Share.LocalDB.LoadAdminList();
        }

        private void MsgGetReloadChatLog()
        {
            // FrmDB.LoadChatLogFiles;
        }

        private void MsgGetUserMgr(int sNum, string Body, int Ident_)
        {
            var UserName = string.Empty;
            string Str = Body;
            string msgbody = HUtil32.GetValidStr3(Str, ref UserName, HUtil32.Backslash);
            // UserMgrEngine.OnExternInterMsg(sNum, Ident_, UserName, msgbody);
        }

        private void MsgGetReloadMakeItemList()
        {
            //M2Share.LocalDB.LoadMakeItemList();
            M2Share.LocalDB.LoadMakeItem();
        }

        private void MsgGetGuildMemberRecall(int sNum, string Body)
        {
            var dxstr = string.Empty;
            var dystr = string.Empty;
            var uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                var Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                Str = HUtil32.GetValidStr3(Str, ref dxstr, HUtil32.Backslash);
                Str = HUtil32.GetValidStr3(Str, ref dystr, HUtil32.Backslash);
                var dx = (short)HUtil32.Str_ToInt(dxstr, 0);
                var dy = (short)HUtil32.Str_ToInt(dystr, 0);
                var hum = M2Share.UserEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    if (hum.m_boAllowGuildReCall)
                    {
                        hum.SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        hum.SpaceMove(Str, dx, dy, 0);
                    }
                }
            }
        }

        private void MsgGetReloadGuildAgit(int sNum, string Body)
        {
            // GuildAgitMan.ClearGuildAgitList;
            // GuildAgitMan.LoadGuildAgitList;
        }

        private void MsgGetLoverLogin(int sNum, string Body)
        {
            PlayObject humlover;
            string Str;
            var uname = string.Empty;
            var lovername = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                Str = HUtil32.GetValidStr3(Str, ref lovername, HUtil32.Backslash);
                humlover = M2Share.UserEngine.GetPlayObject(lovername);
                if (humlover != null)
                {
                    int svidx = 0;
                    if (M2Share.UserEngine.FindOtherServerUser(uname, ref svidx))
                    {
                        M2Share.UserEngine.SendServerGroupMsg(Grobal2.ISM_LM_LOGIN_REPLY, svidx, lovername + '/' + uname + '/' + humlover.Envir.MapDesc);
                    }
                }
            }
        }

        private void MsgGetLoverLogout(int sNum, string Body)
        {
            var uname = string.Empty;
            const string sLoverFindYouMsg = "正在找你...";
            if (sNum == M2Share.ServerIndex)
            {
                var Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                var lovername = Str;
                var hum = M2Share.UserEngine.GetPlayObject(lovername);
                if (hum != null)
                {
                    hum.SysMsg(uname + sLoverFindYouMsg, MsgColor.Red, MsgType.Hint);
                }
            }
        }

        private void MsgGetLoverLoginReply(int sNum, string Body)
        {
            var uname = string.Empty;
        }

        private void MsgGetLoverKilledMsg(int sNum, string Body)
        {
            var uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                var Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                var hum = M2Share.UserEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    hum.SysMsg(Str, MsgColor.Red, MsgType.Hint);
                }
            }
        }

        private void MsgGetRecall(int sNum, string Body)
        {
            var dxstr = string.Empty;
            var dystr = string.Empty;
            var uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                var Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                Str = HUtil32.GetValidStr3(Str, ref dxstr, HUtil32.Backslash);
                Str = HUtil32.GetValidStr3(Str, ref dystr, HUtil32.Backslash);
                var dx = (short)HUtil32.Str_ToInt(dxstr, 0);
                var dy = (short)HUtil32.Str_ToInt(dystr, 0);
                var hum = M2Share.UserEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    hum.SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                    hum.SpaceMove(Str, dx, dy, 0);
                }
            }
        }

        private void MsgGetRequestRecall(int sNum, string Body)
        {
            var uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                var Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                var hum = M2Share.UserEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    hum.RecallHuman(Str);
                }
            }
        }

        private void MsgGetRequestLoverRecall(int sNum, string Body)
        {
            var uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                var Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                var hum = M2Share.UserEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    if (!hum.Envir.Flag.boNORECALL)
                    {
                        hum.RecallHuman(Str);
                    }
                }
            }
        }

        private void MsgGetMarketOpen(bool WantOpen)
        {
            // SQLEngine.Open(WantOpen);
        }
    }
}