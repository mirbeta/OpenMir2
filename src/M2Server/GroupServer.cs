using System;
using System.Linq;

namespace M2Server
{
    public class GroupServer
    {
        private readonly TPlayObject PlayObject = null;

        public GroupServer()
        { 
            
        }

        public void ProcessData(int Ident, int serverNum, string Body)
        {
            switch (Ident)
            {
                case grobal2.ISM_GROUPSERVERHEART:
                    ServerHeartMessage(serverNum, Body);
                    break;
                case grobal2.ISM_USERSERVERCHANGE:
                    MsgGetUserServerChange(serverNum, Body);
                    break;
                case grobal2.ISM_CHANGESERVERRECIEVEOK:
                    MsgGetUserChangeServerRecieveOk(serverNum, Body);
                    break;
                case grobal2.ISM_USERLOGON:
                    MsgGetUserLogon(serverNum, Body);
                    break;
                case grobal2.ISM_USERLOGOUT:
                    MsgGetUserLogout(serverNum, Body);
                    break;
                case grobal2.ISM_WHISPER:
                    MsgGetWhisper(serverNum, Body);
                    break;
                case grobal2.ISM_GMWHISPER:
                    MsgGetGMWhisper(serverNum, Body);
                    break;
                case grobal2.ISM_LM_WHISPER:
                    MsgGetLoverWhisper(serverNum, Body);
                    break;
                case grobal2.ISM_SYSOPMSG:
                    MsgGetSysopMsg(serverNum, Body);
                    break;
                case grobal2.ISM_ADDGUILD:
                    MsgGetAddGuild(serverNum, Body);
                    break;
                case grobal2.ISM_DELGUILD:
                    MsgGetDelGuild(serverNum, Body);
                    break;
                case grobal2.ISM_RELOADGUILD:
                    MsgGetReloadGuild(serverNum, Body);
                    break;
                case grobal2.ISM_GUILDMSG:
                    MsgGetGuildMsg(serverNum, Body);
                    break;
                case grobal2.ISM_GUILDWAR:
                    MsgGetGuildWarInfo(serverNum, Body);
                    break;
                case grobal2.ISM_CHATPROHIBITION:
                    MsgGetChatProhibition(serverNum, Body);
                    break;
                case grobal2.ISM_CHATPROHIBITIONCANCEL:
                    MsgGetChatProhibitionCancel(serverNum, Body);
                    break;
                case grobal2.ISM_CHANGECASTLEOWNER:
                    MsgGetChangeCastleOwner(serverNum, Body);
                    break;
                case grobal2.ISM_RELOADCASTLEINFO:
                    MsgGetReloadCastleAttackers(serverNum);
                    break;
                case grobal2.ISM_RELOADADMIN:
                    MsgGetReloadAdmin();
                    break;
                case grobal2.ISM_MARKETOPEN:
                    MsgGetMarketOpen(true);
                    break;
                case grobal2.ISM_MARKETCLOSE:
                    MsgGetMarketOpen(false);
                    break;
                case grobal2.ISM_RELOADCHATLOG:
                    MsgGetReloadChatLog();
                    break;
                case grobal2.ISM_USER_INFO:
                case grobal2.ISM_FRIEND_INFO:
                case grobal2.ISM_FRIEND_DELETE:
                case grobal2.ISM_FRIEND_OPEN:
                case grobal2.ISM_FRIEND_CLOSE:
                case grobal2.ISM_FRIEND_RESULT:
                case grobal2.ISM_TAG_SEND:
                case grobal2.ISM_TAG_RESULT:
                    MsgGetUserMgr(serverNum, Body, Ident);
                    break;
                case grobal2.ISM_RELOADMAKEITEMLIST:
                    MsgGetReloadMakeItemList();
                    break;
                case grobal2.ISM_GUILDMEMBER_RECALL:
                    MsgGetGuildMemberRecall(serverNum, Body);
                    break;
                case grobal2.ISM_RELOADGUILDAGIT:
                    MsgGetReloadGuildAgit(serverNum, Body);
                    break;
                case grobal2.ISM_LM_LOGIN:
                    MsgGetLoverLogin(serverNum, Body);
                    break;
                case grobal2.ISM_LM_LOGOUT:
                    MsgGetLoverLogout(serverNum, Body);
                    break;
                case grobal2.ISM_LM_LOGIN_REPLY:
                    MsgGetLoverLoginReply(serverNum, Body);
                    break;
                case grobal2.ISM_LM_KILLED_MSG:
                    MsgGetLoverKilledMsg(serverNum, Body);
                    break;
                case grobal2.ISM_RECALL:
                    MsgGetRecall(serverNum, Body);
                    break;
                case grobal2.ISM_REQUEST_RECALL:
                    MsgGetRequestRecall(serverNum, Body);
                    break;
                case grobal2.ISM_REQUEST_LOVERRECALL:
                    MsgGetRequestLoverRecall(serverNum, Body);
                    break;
            }
        }

        public void ServerHeartMessage(int sNu,string Body)
        {

        }

        public void MsgGetUserServerChange(int sNum, string Body)
        {
            string ufilename;
            int shifttime;
            const string sExceptionMsg = "[Exception] TFrmSrvMsg::MsgGetUserServerChange";
            shifttime = HUtil32.GetTickCount();
            ufilename = Body;
            if (M2Share.nServerIndex == sNum)
            {
                try
                {
                    //fHandle = File.Open(M2Share.g_Config.sBaseDir + ufilename, (FileMode)FileAccess.Read | FileShare.ReadWrite);
                    //if (fHandle > 0)
                    //{
                    //    psui = new TSwitchDataInfo();
                    //    FileRead(fHandle, psui, sizeof(TSwitchDataInfo));
                    //    FileRead(fHandle, FileCheckSum, sizeof(int));
                    //    fHandle.Close();
                    //    File.Delete((M2Share.g_Config.sBaseDir + ufilename as string));
                    //    CheckSum = 0;
                    //    for (i = 0; i < sizeof(TSwitchDataInfo); i++)
                    //    {
                    //        CheckSum = CheckSum + ((byte)psui + i);
                    //    }
                    //    if (CheckSum == FileCheckSum)
                    //    {
                    //        M2Share.UserEngine.AddSwitchData(psui);
                    //        M2Share.UserEngine.SendInterMsg(grobal2.ISM_CHANGESERVERRECIEVEOK, M2Share.nServerIndex, ufilename);
                    //        // MainOutMessageAPI('DeleteFile: ' + g_Config.sBaseDir + ufilename);
                    //    }
                    //    else
                    //    {
                    //        this.Dispose(psui);
                    //    }
                    //}
                }
                catch
                {
                    M2Share.ErrorMessage(sExceptionMsg);
                }
            }
        }

        public void MsgGetUserChangeServerRecieveOk(int sNum, string Body)
        {
            var ufilename = Body;
            M2Share.UserEngine.GetISMChangeServerReceive(ufilename);
        }

        public void MsgGetUserLogon(int sNum, string Body)
        {
            var uname = Body;
            M2Share.UserEngine.OtherServerUserLogon(sNum, uname);
        }

        public void MsgGetUserLogout(int sNum, string Body)
        {
            var uname = Body;
            M2Share.UserEngine.OtherServerUserLogout(sNum, uname);
        }

        public void MsgGetWhisper(int sNum, string Body)
        {
            string Str;
            var uname = string.Empty;
            if (sNum == M2Share.nServerIndex)
            {
                Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, "/");
                TPlayObject hum = M2Share.UserEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    if (!hum.m_boHearWhisper)
                    {
                        hum.WhisperRe(Str, 1);
                    }
                }
            }
        }

        public void MsgGetGMWhisper(int sNum, string Body)
        {
            string Str;
            var uname = string.Empty;
            if (sNum == M2Share.nServerIndex)
            {
                Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, "/");
                TPlayObject hum = M2Share.UserEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    if (!hum.m_boHearWhisper)
                    {
                        hum.WhisperRe(Str, 0);
                    }
                }
            }
        }

        private void MsgGetLoverWhisper(int sNum, string Body)
        {
            var Str = string.Empty;
            var uname = string.Empty;
            if (sNum == M2Share.nServerIndex)
            {
                Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, "/");
                TPlayObject hum = M2Share.UserEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    if (!hum.m_boHearWhisper)
                    {
                        hum.WhisperRe(Str, 2);
                    }
                }
            }
        }

        public void MsgGetSysopMsg(int sNum, string Body)
        {
            M2Share.UserEngine.SendBroadCastMsg(Body, TMsgType.t_System);
        }

        public void MsgGetAddGuild(int sNum, string Body)
        {
            var gname = string.Empty;
            var mname = HUtil32.GetValidStr3(Body, ref gname, "/");
            M2Share.GuildManager.AddGuild(gname, mname);
        }

        public void MsgGetDelGuild(int sNum, string Body)
        {
            var gname = string.Empty;
            gname = Body;
            M2Share.GuildManager.DelGuild(gname);
        }

        public void MsgGetReloadGuild(int sNum, string Body)
        {
            var gname = Body;
            TGuild guild;
            if (sNum == 0)
            {
                guild = M2Share.GuildManager.FindGuild(gname);
                if (guild != null)
                {
                    guild.LoadGuild();
                    M2Share.UserEngine.GuildMemberReGetRankName(guild);
                }
            }
            else if (M2Share.nServerIndex != sNum)
            {
                guild = M2Share.GuildManager.FindGuild(gname);
                if (guild != null)
                {
                    guild.LoadGuildFile(gname + '.' + sNum);
                    M2Share.UserEngine.GuildMemberReGetRankName(guild);
                    guild.SaveGuildInfoFile();
                }
            }
        }

        public void MsgGetGuildMsg(int sNum, string Body)
        {
            var gname = string.Empty;
            TGuild g;
            string Str = Body;
            Str = HUtil32.GetValidStr3(Str, ref gname, "/");
            if (gname != "")
            {
                g = M2Share.GuildManager.FindGuild(gname);
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
            TGuild g;
            TGuild WarGuild;
            TWarGuild pgw;
            int currenttick;
            if (sNum == 0)
            {
                Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref gname, "/");
                Str = HUtil32.GetValidStr3(Str, ref warguildname, "/");
                Str = HUtil32.GetValidStr3(Str, ref StartTime, "/");
                remaintime = Str;
                if (gname != "" && warguildname != "")
                {
                    g = M2Share.GuildManager.FindGuild(gname);
                    WarGuild = M2Share.GuildManager.FindGuild(warguildname);
                    if (g != null && WarGuild != null)
                    {
                        currenttick = HUtil32.GetTickCount();
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
                                    M2Share.MainOutMessage("[行会战] " + g.sGuildName + "<->" + WarGuild.sGuildName + ", 开战: " + StartTime + ", 持久: " + remaintime + ", 现在: " + pgw.dwWarTick + ", 时差: " + M2Share.g_nServerTickDifference);
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
                            M2Share.MainOutMessage("[行会战] " + g.sGuildName + "<->" + WarGuild.sGuildName + ", 开战: " + StartTime + ", 持久: " + remaintime + ", 现在: " + (Convert.ToUInt32(StartTime) - M2Share.g_nServerTickDifference) + ", 时差: " + M2Share.g_nServerTickDifference);
                        }
                        g.RefMemberName();
                        g.UpdateGuildFile();
                    }
                }
            }
        }

        public void MsgGetChatProhibition(int sNum, string Body)
        {
            byte obtPermission;
            var Str = string.Empty;
            var whostr = string.Empty;
            var minstr = string.Empty;
            Str = Body;
            Str = HUtil32.GetValidStr3(Str, ref whostr, "/");
            Str = HUtil32.GetValidStr3(Str, ref minstr, "/");
            if (whostr != "")
            {
                obtPermission = PlayObject.m_btPermission;
                PlayObject.m_btPermission = 10;
                PlayObject.CmdShutup(M2Share.g_GameCommand.SHUTUP, whostr, minstr);
                PlayObject.m_btPermission = obtPermission;
            }
        }

        public void MsgGetChatProhibitionCancel(int sNum, string Body)
        {
            byte obtPermission;
            string whostr;
            whostr = Body;
            if (whostr != "")
            {
                obtPermission = PlayObject.m_btPermission;
                PlayObject.m_btPermission = 10;
                PlayObject.CmdShutup(M2Share.g_GameCommand.SHUTUP, whostr, "");
                PlayObject.m_btPermission = obtPermission;
            }
        }

        public void MsgGetChangeCastleOwner(int sNum, string Body)
        {
            throw new Exception("TODO MsgGetChangeCastleOwner...");
        }

        public void MsgGetReloadCastleAttackers(int sNum)
        {
            M2Share.CastleManager.Initialize();
        }

        public void MsgGetReloadAdmin()
        {
            M2Share.LocalDB.LoadAdminList();
        }

        public void MsgGetReloadChatLog()
        {
            // FrmDB.LoadChatLogFiles;
        }

        public void MsgGetUserMgr(int sNum, string Body, int Ident_)
        {
            var UserName = string.Empty;
            var msgbody = string.Empty;
            var Str = string.Empty;
            Str = Body;
            msgbody = HUtil32.GetValidStr3(Str, ref UserName, "/");
            // UserMgrEngine.OnExternInterMsg(sNum, Ident_, UserName, msgbody);
        }

        // procedure MsgGetRelationShipDelete(sNum: Integer; Body: string);

        public void MsgGetReloadMakeItemList()
        {
            //M2Share.LocalDB.LoadMakeItemList();
            M2Share.LocalDB.LoadMakeItem();
        }

        public void MsgGetGuildMemberRecall(int sNum, string Body)
        {
            TPlayObject hum;
            short dx;
            short dy;
            var dxstr = string.Empty;
            var dystr = string.Empty;
            var Str = string.Empty;
            var uname = string.Empty;
            if (sNum == M2Share.nServerIndex)
            {
                Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, "/");
                Str = HUtil32.GetValidStr3(Str, ref dxstr, "/");
                Str = HUtil32.GetValidStr3(Str, ref dystr, "/");
                dx = (short)HUtil32.Str_ToInt(dxstr, 0);
                dy = (short)HUtil32.Str_ToInt(dystr, 0);
                hum = M2Share.UserEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    if (hum.m_boAllowGuildReCall)
                    {
                        hum.SendRefMsg(grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        hum.SpaceMove(Str, dx, dy, 0);
                    }
                }
            }
        }

        public void MsgGetReloadGuildAgit(int sNum, string Body)
        {
            // GuildAgitMan.ClearGuildAgitList;
            // GuildAgitMan.LoadGuildAgitList;

        }

        public void MsgGetLoverLogin(int sNum, string Body)
        {
            TPlayObject humlover;
            string Str;
            var uname = string.Empty;
            var lovername = string.Empty;
            if (sNum == M2Share.nServerIndex)
            {
                Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, "/");
                Str = HUtil32.GetValidStr3(Str, ref lovername, "/");
                humlover = M2Share.UserEngine.GetPlayObject(lovername);
                if (humlover != null)
                {
                    // humlover.SysMsg(uname + '丛捞 ' + Str + '俊 甸绢坷继嚼聪促.', 6);
                    // if UserEngine.FindOtherServerUser(uname, svidx) then
                    // UserEngine.SendInterMsg(ISM_LM_LOGIN_REPLY, svidx, lovername + '/' + uname + '/' + humlover.penvir.MapTitle);
                }
            }
        }

        public void MsgGetLoverLogout(int sNum, string Body)
        {
            TPlayObject hum;
            string Str;
            var uname = string.Empty;
            string lovername;
            const string sLoverFindYouMsg = "正在找你...";
            if (sNum == M2Share.nServerIndex)
            {
                Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, "/");
                lovername = Str;
                hum = M2Share.UserEngine.GetPlayObject(lovername);
                if (hum != null)
                {
                    hum.SysMsg(uname + sLoverFindYouMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
        }

        public void MsgGetLoverLoginReply(int sNum, string Body)
        {
            var uname = string.Empty;
        }

        public void MsgGetLoverKilledMsg(int sNum, string Body)
        {
            TPlayObject hum;
            string Str;
            var uname = string.Empty;
            if (sNum == M2Share.nServerIndex)
            {
                Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, "/");
                hum = M2Share.UserEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    hum.SysMsg(Str, TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
        }

        public void MsgGetRecall(int sNum, string Body)
        {
            TPlayObject hum;
            short dx;
            short dy;
            var dxstr = string.Empty;
            var dystr = string.Empty;
            var Str = string.Empty;
            var uname = string.Empty;
            if (sNum == M2Share.nServerIndex)
            {
                Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, "/");
                Str = HUtil32.GetValidStr3(Str, ref dxstr, "/");
                Str = HUtil32.GetValidStr3(Str, ref dystr, "/");
                dx = (short)HUtil32.Str_ToInt(dxstr, 0);
                dy = (short)HUtil32.Str_ToInt(dystr, 0);
                hum = M2Share.UserEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    hum.SendRefMsg(grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                    hum.SpaceMove(Str, dx, dy, 0);// 傍埃捞悼
                }
            }
        }

        public void MsgGetRequestRecall(int sNum, string Body)
        {
            TPlayObject hum;
            string Str;
            var uname = string.Empty;
            if (sNum == M2Share.nServerIndex)
            {
                Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, "/");
                hum = M2Share.UserEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    hum.RecallHuman(Str);
                }
            }
        }

        public void MsgGetRequestLoverRecall(int sNum, string Body)
        {
            TPlayObject hum;
            string Str;
            var uname = string.Empty;
            if (sNum == M2Share.nServerIndex)
            {
                Str = Body;
                Str = HUtil32.GetValidStr3(Str, ref uname, "/");
                hum = M2Share.UserEngine.GetPlayObject(uname);
                if (hum != null)
                {
                    if (!hum.m_PEnvir.Flag.boNORECALL)
                    {
                        hum.RecallHuman(Str);
                    }
                }
            }
        }

        public void MsgGetMarketOpen(bool WantOpen)
        {
            // SQLEngine.Open(WantOpen);
        }
    }
}