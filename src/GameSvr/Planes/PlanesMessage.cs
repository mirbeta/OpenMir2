using GameSvr.GameCommand;
using GameSvr.Guild;
using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.Planes
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
                    M2Share.Log.Info("跨服消息");
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
                    M2Share.WorldEngine.AddSwitchData(new SwitchDataInfo());
                    World.WorldServer.SendServerGroupMsg(Grobal2.ISM_CHANGESERVERRECIEVEOK, M2Share.ServerIndex, ufilename);
                }
                catch
                {
                    M2Share.Log.Error(sExceptionMsg);
                }
            }
        }

        private static void MsgGetUserChangeServerRecieveOk(int sNum, string Body)
        {
            var ufilename = Body;
            M2Share.WorldEngine.GetIsmChangeServerReceive(ufilename);
        }

        private static void MsgGetUserLogon(int sNum, string Body)
        {
            var uname = Body;
            M2Share.WorldEngine.OtherServerUserLogon(sNum, uname);
        }

        private static void MsgGetUserLogout(int sNum, string Body)
        {
            var uname = Body;
            M2Share.WorldEngine.OtherServerUserLogout(sNum, uname);
        }

        private static void MsgGetWhisper(int sNum, string Body)
        {
            var uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                var Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                PlayObject hum = M2Share.WorldEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    if (hum.HearWhisper)
                    {
                        hum.WhisperRe(Str, 1);
                    }
                }
            }
        }

        private static void MsgGetGMWhisper(int sNum, string Body)
        {
            var uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                var Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                PlayObject hum = M2Share.WorldEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    if (hum.HearWhisper)
                    {
                        hum.WhisperRe(Str, 0);
                    }
                }
            }
        }

        private static void MsgGetLoverWhisper(int sNum, string Body)
        {
            var uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                var Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                PlayObject hum = M2Share.WorldEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    if (hum.HearWhisper)
                    {
                        hum.WhisperRe(Str, 2);
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
            var gname = string.Empty;
            var mname = HUtil32.GetValidStr3(Body, ref gname, HUtil32.Backslash);
            M2Share.GuildMgr.AddGuild(gname, mname);
        }

        private static void MsgGetDelGuild(int sNum, string Body)
        {
            var gname = Body;
            M2Share.GuildMgr.DelGuild(gname);
        }

        private static void MsgGetReloadGuild(int sNum, string Body)
        {
            var gname = Body;
            GuildInfo guild;
            if (sNum == 0)
            {
                guild = M2Share.GuildMgr.FindGuild(gname);
                if (guild != null)
                {
                    guild.LoadGuild();
                    M2Share.WorldEngine.GuildMemberReGetRankName(guild);
                }
            }
            else if (M2Share.ServerIndex != sNum)
            {
                guild = M2Share.GuildMgr.FindGuild(gname);
                if (guild != null)
                {
                    guild.LoadGuildFile(gname + '.' + sNum);
                    M2Share.WorldEngine.GuildMemberReGetRankName(guild);
                    guild.SaveGuildInfoFile();
                }
            }
        }

        private static void MsgGetGuildMsg(int sNum, string Body)
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

        private static void MsgGetGuildWarInfo(int sNum, string Body)
        {
            string Str;
            var gname = string.Empty;
            var warguildname = string.Empty;
            var StartTime = string.Empty;
            var remaintime = string.Empty;
            GuildInfo g;
            GuildInfo WarGuild;
            WarGuild pgw;
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
                        if (M2Share.ServerTickDifference == 0)
                        {
                            M2Share.ServerTickDifference = Convert.ToInt32(StartTime) - currenttick;
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
                                    pgw.dwWarTick = Convert.ToInt32(StartTime) - M2Share.ServerTickDifference;
                                    pgw.dwWarTime = Convert.ToInt32(remaintime);
                                    M2Share.Log.Info("[行会战] " + g.sGuildName + "<->" + WarGuild.sGuildName + ", 开战: " + StartTime + ", 持久: " + remaintime + ", 现在: " + pgw.dwWarTick + ", 时差: " + M2Share.ServerTickDifference);
                                    break;
                                }
                            }
                        }
                        if (pgw == null)
                        {
                            if (!g.GuildWarList.Select(x => x.Guild).Contains(WarGuild))
                            {
                                pgw = new WarGuild();
                                pgw.Guild = WarGuild;
                                pgw.dwWarTick = int.Parse(StartTime) - M2Share.ServerTickDifference;
                                pgw.dwWarTime = int.Parse(remaintime);
                                g.GuildWarList.Add(pgw);
                            }
                            M2Share.Log.Info("[行会战] " + g.sGuildName + "<->" + WarGuild.sGuildName + ", 开战: " + StartTime + ", 持久: " + remaintime + ", 现在: " + (Convert.ToUInt32(StartTime) - M2Share.ServerTickDifference) + ", 时差: " + M2Share.ServerTickDifference);
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
                //PlayObject.CmdShutup(Settings.g_GameCommand.SHUTUP, whostr, minstr);
                CommandMgr.GetInstance().ExecCmd("Shutup", PlayObject);
            }
        }

        private static void MsgGetChatProhibitionCancel(int sNum, string Body)
        {
            var whostr = Body;
            if (whostr != "")
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
            DataSource.LocalDB.LoadAdminList();
        }

        private static void MsgGetReloadChatLog()
        {
            // FrmDB.LoadChatLogFiles;
        }

        private static void MsgGetUserMgr(int sNum, string Body, int Ident_)
        {
            var UserName = string.Empty;
            string Str = Body;
            string msgbody = HUtil32.GetValidStr3(Str, ref UserName, HUtil32.Backslash);
            // UserMgrEngine.OnExternInterMsg(sNum, Ident_, UserName, msgbody);
        }

        private static void MsgGetReloadMakeItemList()
        {
            //M2Share.LocalDB.LoadMakeItemList();
            M2Share.LocalDb.LoadMakeItem();
        }

        private static void MsgGetGuildMemberRecall(int sNum, string Body)
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
                var dx = (short)HUtil32.StrToInt(dxstr, 0);
                var dy = (short)HUtil32.StrToInt(dystr, 0);
                var hum = M2Share.WorldEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    if (hum.AllowGuildReCall)
                    {
                        hum.SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        hum.SpaceMove(Str, dx, dy, 0);
                    }
                }
            }
        }

        private static void MsgGetReloadGuildAgit(int sNum, string Body)
        {
            // GuildAgitMan.ClearGuildAgitList;
            // GuildAgitMan.LoadGuildAgitList;
        }

        private static void MsgGetLoverLogin(int sNum, string Body)
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
                humlover = M2Share.WorldEngine.GetPlayObject(lovername);
                if (humlover != null)
                {
                    int svidx = 0;
                    if (M2Share.WorldEngine.FindOtherServerUser(uname, ref svidx))
                    {
                        World.WorldServer.SendServerGroupMsg(Grobal2.ISM_LM_LOGIN_REPLY, svidx, lovername + '/' + uname + '/' + humlover.Envir.MapDesc);
                    }
                }
            }
        }

        private static void MsgGetLoverLogout(int sNum, string Body)
        {
            var uname = string.Empty;
            const string sLoverFindYouMsg = "正在找你...";
            if (sNum == M2Share.ServerIndex)
            {
                var Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                var lovername = Str;
                var hum = M2Share.WorldEngine.GetPlayObject(lovername);
                if (hum != null)
                {
                    hum.SysMsg(uname + sLoverFindYouMsg, MsgColor.Red, MsgType.Hint);
                }
            }
        }

        private static void MsgGetLoverLoginReply(int sNum, string Body)
        {
        }

        private static void MsgGetLoverKilledMsg(int sNum, string Body)
        {
            var uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                var Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                var hum = M2Share.WorldEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    hum.SysMsg(Str, MsgColor.Red, MsgType.Hint);
                }
            }
        }

        private static void MsgGetRecall(int sNum, string Body)
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
                var dx = (short)HUtil32.StrToInt(dxstr, 0);
                var dy = (short)HUtil32.StrToInt(dystr, 0);
                var hum = M2Share.WorldEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    hum.SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                    hum.SpaceMove(Str, dx, dy, 0);
                }
            }
        }

        private static void MsgGetRequestRecall(int sNum, string Body)
        {
            var uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                var Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                var hum = M2Share.WorldEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    hum.RecallHuman(Str);
                }
            }
        }

        private static void MsgGetRequestLoverRecall(int sNum, string Body)
        {
            var uname = string.Empty;
            if (sNum == M2Share.ServerIndex)
            {
                var Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, HUtil32.Backslash);
                var hum = M2Share.WorldEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    if (!hum.Envir.Flag.boNORECALL)
                    {
                        hum.RecallHuman(Str);
                    }
                }
            }
        }

        private static void MsgGetMarketOpen(bool WantOpen)
        {
            // SQLEngine.Open(WantOpen);
        }
    }
}