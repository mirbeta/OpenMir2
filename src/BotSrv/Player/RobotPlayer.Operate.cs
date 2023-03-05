using System;
using System.Drawing;
using BotSrv.Objects;
using BotSrv.Scenes;
using SystemModule;
using SystemModule.Enums;
using SystemModule.Packets;
using SystemModule.Packets.ClientPackets;
using SystemModule.Packets.ServerPackets;

namespace BotSrv.Player
{
    public partial class RobotPlayer
    {
        public void ProcessPacket(string str)
        {
            var data = string.Empty;
            if (!string.IsNullOrEmpty(str))
            {
                while (str.Length >= 2)
                {
                    if (MShare.MapMovingWait)
                    {
                        break;
                    }
                    if (str.IndexOf("!", StringComparison.OrdinalIgnoreCase) <= 0)
                    {
                        break;
                    }
                    str = HUtil32.ArrestStringEx(str, "#", "!", ref data);
                    if (!string.IsNullOrEmpty(data))
                    {
                        DecodeMessagePacket(data);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        
        private void DecodeMessagePacket(string datablock)
        {
            var body = string.Empty;
            string data;
            var str = string.Empty;
            var str3 = string.Empty;
            CharDesc desc;
            MessageBodyWL wl;
            int i;
            int j;
            var n = 0;
            Actor Actor;
            Actor Actor2;
            //TClEvent __event;
            if (datablock[0] == '+') //动作包
            {
                ProcessActMsg(datablock);
                return;
            }
            if (datablock.Length < Messages.DefBlockSize)
            {
                return;
            }
            if (datablock.Length > Messages.DefBlockSize)
            {
                body = datablock[Messages.DefBlockSize..];
            }
            var head = datablock[..Messages.DefBlockSize];
            var msg = EDCode.DecodePacket(head);
            if (msg.Ident == 0)
            {
                return;
            }
            if (MShare.MySelf == null)
            {
                switch (msg.Ident)
                {
                    case Messages.SM_NEWID_SUCCESS:
                        MainOutMessage("您的帐号创建成功。请妥善保管您的帐号和密码，并且不要因任何原因把帐号和密码告诉任何其他人。如果忘记了密码,你可以通过我们的主页重新找回。");
                        LoginScene.ClientNewIdSuccess();
                        break;
                    case Messages.SM_NEWID_FAIL:
                        switch (msg.Recog)
                        {
                            case 0:
                                MainOutMessage($"帐号 [{LoginId}] 已被其他的玩家使用了。请选择其它帐号名注册");
                                break;
                            case -2:
                                MainOutMessage("此帐号名被禁止使用！");
                                break;
                            default:
                                MainOutMessage("帐号创建失败，请确认帐号是否包括空格、及非法字符！Code: " + msg.Recog.ToString());
                                break;
                        }
                        break;
                    case Messages.SM_PASSWD_FAIL:
                        switch (msg.Recog)
                        {
                            case -1:
                                MainOutMessage("密码错误！");
                                break;
                            case -2:
                                MainOutMessage("密码输入错误超过3次，此帐号被暂时锁定，请稍候再登录！");
                                break;
                            case -3:
                                MainOutMessage("此帐号已经登录或被异常锁定，请稍候再登录！");
                                break;
                            case -4:
                                MainOutMessage("这个帐号访问失败！\\请使用其他帐号登录，\\或者申请付费注册");
                                break;
                            case -5:
                                MainOutMessage("这个帐号被锁定！");
                                break;
                            case -6:
                                MainOutMessage("请使用专用登陆器登陆游戏！");
                                break;
                            default:
                                MainOutMessage("此帐号不存在，或出现未知错误！");
                                break;
                        }
                        LoginScene.PassWdFail();
                        break;
                    case Messages.SM_NEEDUPDATE_ACCOUNT:
                        ClientGetNeedUpdateAccount(body);
                        break;
                    case Messages.SM_UPDATEID_SUCCESS:
                        MainOutMessage("您的帐号信息更新成功。请妥善保管您的帐号和密码。并且不要因任何原因把帐号和密码告诉任何其他人。如果忘记了密码，你可以通过我们的主页重新找回。");
                        LoginScene.ClientGetSelectServer();
                        break;
                    case Messages.SM_UPDATEID_FAIL:
                        MainOutMessage("更新帐号失败！");
                        LoginScene.ClientGetSelectServer();
                        break;
                    case Messages.SM_PASSOK_SELECTSERVER:
                        LoginScene.ClientGetPasswordOk(msg, body);
                        break;
                    case Messages.SM_SELECTSERVER_OK:
                        LoginScene.ClientGetPasswdSuccess(body);
                        DScreen.ChangeScene(SceneType.SelectChr);
                        break;
                    case Messages.SM_QUERYCHR:
                        SelectChrScene.ClientGetReceiveChrs(body);
                        break;
                    case Messages.SM_QUERYCHR_FAIL:
                        MainOutMessage("服务器认证失败！");
                        break;
                    case Messages.SM_NEWCHR_SUCCESS:
                        SelectChrScene.SendQueryChr();
                        break;
                    case Messages.SM_NEWCHR_FAIL:
                        switch (msg.Recog)
                        {
                            case 0:
                                MainOutMessage("[错误信息] 输入的角色名称包含非法字符！ 错误代码 = 0");
                                break;
                            case 2:
                                MainOutMessage("[错误信息] 创建角色名称已被其他人使用！ 错误代码 = 2");
                                break;
                            case 3:
                                MainOutMessage("[错误信息] 您只能创建二个游戏角色！ 错误代码 = 3");
                                break;
                            case 4:
                                MainOutMessage("[错误信息] 创建角色时出现错误！ 错误代码 = 4");
                                break;
                            default:
                                MainOutMessage("[错误信息] 创建角色时出现未知错误！");
                                break;
                        }
                        break;
                    case Messages.SM_CHGPASSWD_SUCCESS:
                        MainOutMessage("密码修改成功");
                        break;
                    case Messages.SM_CHGPASSWD_FAIL:
                        switch (msg.Recog)
                        {
                            case -1:
                                MainOutMessage("输入的原始密码不正确！");
                                break;
                            case -2:
                                MainOutMessage("此帐号被锁定！");
                                break;
                            default:
                                MainOutMessage("输入的新密码长度小于四位！");
                                break;
                        }
                        break;
                    case Messages.SM_DELCHR_SUCCESS:
                        SelectChrScene.SendQueryChr();
                        break;
                    case Messages.SM_DELCHR_FAIL:
                        MainOutMessage("[错误信息] 删除游戏角色时出现错误！");
                        break;
                    case Messages.SM_STARTPLAY:
                        SelectChrScene.ClientGetStartPlay(body);
                        DScreen.ChangeScene(SceneType.PlayGame);
                        break;
                    case Messages.SM_STARTFAIL:
                        MainOutMessage("此服务器满员！");
                        LoginScene.ClientGetSelectServer();
                        break;
                    case Messages.SM_VERSION_FAIL:
                        MainOutMessage("游戏程序版本不正确，请下载最新版本游戏程序！");
                        break;
                    //case Messages.SM_OVERCLIENTCOUNT:
                    //    MShare.g_boDoFastFadeOut = false;
                    //    DebugOutStr("客户端开启数量过多，连接被断开！！！");
                    //    break;
                    case Messages.SM_OUTOFCONNECTION:
                    case Messages.SM_NEWMAP:
                    case Messages.SM_LOGON:
                    case Messages.SM_RECONNECT:
                    case Messages.SM_SENDNOTICE:
                    case Messages.SM_DLGMSG:
                        break;
                }
            }
            if (MShare.g_boMapMoving)
            {
                if (msg.Ident == Messages.SM_CHANGEMAP)
                {
                    _waitingMsg = msg;
                    _waitingStr = EDCode.DeCodeString(body);
                    MShare.MapMovingWait = true;
                    //WaitMsgTimer.Enabled = true;
                }
                return;
            }
            string body2;
            switch (msg.Ident)
            {
                case Messages.SM_PLAYERCONFIG:
                    switch (msg.Recog)
                    {
                        case -1:
                            ScreenManager.AddChatBoardString("切换时装外显操作太快了！");
                            break;
                    }
                    break;
                case Messages.SM_NEWMAP:
                    MShare.MapTitle = "";
                    str = EDCode.DeCodeString(body);
                    PlayScene.SendMsg(Messages.SM_NEWMAP, 0, msg.Param, msg.Tag, (byte)msg.Series, 0, 0, str);
                    break;
                case Messages.SM_LOGON:
                    MShare.g_dwFirstServerTime = 0;
                    MShare.g_dwFirstClientTime = 0;
                    wl = EDCode.DecodeBuffer<MessageBodyWL>(body);
                    if (msg.Series > 8)
                    {
                        msg.Series = (byte)RandomNumber.GetInstance().Random(8);
                    }
                    PlayScene.SendMsg(Messages.SM_LOGON, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, wl.Param1, wl.Param2, "");
                    SendClientMessage(Messages.CM_QUERYBAGITEMS, 1, 0, 0, 0);
                    if (HUtil32.LoByte(HUtil32.LoWord(wl.Tag1)) == 1)
                    {
                        MShare.g_boAllowGroup = true;
                    }
                    else
                    {
                        MShare.g_boAllowGroup = false;
                    }
                    MShare.g_boServerChanging = false;
                    if (MShare.g_wAvailIDDay > 0)
                    {
                        ScreenManager.AddChatBoardString("您当前通过包月帐号充值");
                    }
                    else if (MShare.g_wAvailIPDay > 0)
                    {
                        ScreenManager.AddChatBoardString("您当前通过包月IP 充值");
                    }
                    else if (MShare.g_wAvailIPHour > 0)
                    {
                        ScreenManager.AddChatBoardString("您当前通过计时IP 充值");
                    }
                    else if (MShare.g_wAvailIDHour > 0)
                    {
                        ScreenManager.AddChatBoardString("您当前通过计时帐号充值");
                    }
                    MShare.LoadUserConfig(ChrName);
                    MShare.LoadItemFilter2();
                    //SendClientMessage(Messages.CM_HIDEDEATHBODY, MShare.g_MySelf.m_nRecogId, (int)MShare.g_gcGeneral[8], 0, 0);
                    MainOutMessage("成功进入游戏");
                    MainOutMessage("-----------------------------------------------");
                    break;
                case Messages.SM_SERVERCONFIG:
                    ClientGetServerConfig(msg, body);
                    break;
                case Messages.SM_RECONNECT:
                    SelectChrScene.ClientGetReconnect(body);
                    break;
                case Messages.SM_TIMECHECK_MSG:
                    CheckSpeedHack(msg.Recog);
                    break;
                case Messages.SM_AREASTATE:
                    MShare.g_nAreaStateValue = msg.Recog;
                    break;
                case Messages.SM_MAPDESCRIPTION:
                    ClientGetMapDescription(msg, body);
                    break;
                case Messages.SM_GAMEGOLDNAME:
                    ClientGetGameGoldName(msg, body);
                    break;
                case Messages.SM_ADJUST_BONUS:
                    ClientGetAdjustBonus(msg.Recog, body);
                    break;
                case Messages.SM_MYSTATUS:
                    MShare.g_nMyHungryState = msg.Param;
                    break;
                case Messages.SM_TURN:
                    n = ClFunc.GetCodeMsgSize((float)8 * 4 / 3);//sizeof(CharDesc)
                    if (body.Length > n)
                    {
                        body2 = body[n..];
                        data = EDCode.DeCodeString(body2);
                        str = HUtil32.GetValidStr3(data, ref data, HUtil32.Backslash);
                        body2 = body[..n];
                    }
                    else
                    {
                        body2 = body;
                        data = string.Empty;
                    }
                    desc = EDCode.DecodeBuffer<CharDesc>(body2);
                    PlayScene.SendMsg(Messages.SM_TURN, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, desc.Feature, desc.Status, "", 0);
                    if (!string.IsNullOrEmpty(data))
                    {
                        Actor = PlayScene.FindActor(msg.Recog);
                        if (Actor != null)
                        {
                            Actor.m_sDescUserName = HUtil32.GetValidStr3(data, ref Actor.UserName, "\\");
                            if (Actor.UserName.IndexOf("(", StringComparison.Ordinal) != 0)
                            {
                                HUtil32.ArrestStringEx(Actor.UserName, "(", ")", ref data);
                                if (string.Compare(data, MShare.MySelf.UserName, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    j = 0;
                                    for (i = 0; i < MShare.MySelf.SlaveObject.Count; i++)
                                    {
                                        if (MShare.MySelf.SlaveObject[i] == Actor)
                                        {
                                            j = 1;
                                            break;
                                        }
                                    }
                                    if (j == 0)
                                    {
                                        MShare.MySelf.SlaveObject.Add(Actor);
                                    }
                                }
                            }
                            Actor.NameColor = (byte)HUtil32.StrToInt(str, 0);
                        }
                    }
                    break;
                case Messages.SM_BACKSTEP:
                    n = ClFunc.GetCodeMsgSize((float)8 * 4 / 3);//sizeof(CharDesc)
                    if (body.Length > n)
                    {
                        body2 = body.Substring(n + 1 - 1, body.Length);
                        data = EDCode.DeCodeString(body2);
                        body2 = body[..n];
                        str = HUtil32.GetValidStr3(data, ref data, HUtil32.Backslash);
                    }
                    else
                    {
                        body2 = body;
                        data = "";
                    }
                    desc = EDCode.DecodeBuffer<CharDesc>(body2);
                    PlayScene.SendMsg(Messages.SM_BACKSTEP, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, desc.Feature, desc.Status, "", 0);
                    if (!string.IsNullOrEmpty(data))
                    {
                        Actor = PlayScene.FindActor(msg.Recog);
                        if (Actor != null)
                        {
                            Actor.m_sDescUserName = HUtil32.GetValidStr3(data, ref Actor.UserName, "\\");
                            Actor.NameColor = (byte)HUtil32.StrToInt(str, 0);
                        }
                    }
                    break;
                case Messages.SM_SPACEMOVE_HIDE:
                case Messages.SM_SPACEMOVE_HIDE2:
                    if (msg.Recog != MShare.MySelf.RecogId)
                    {
                        PlayScene.SendMsg(msg.Ident, msg.Recog, msg.Param, msg.Tag, 0, 0, 0, "");
                    }
                    break;
                case Messages.SM_SPACEMOVE_SHOW:
                case Messages.SM_SPACEMOVE_SHOW2:
                    n = ClFunc.GetCodeMsgSize((float)8 * 4 / 3);//sizeof(CharDesc)
                    if (body.Length > n)
                    {
                        body2 = body.Substring(n, body.Length);
                        data = EDCode.DeCodeString(body2);
                        body2 = body[..n];
                        str = HUtil32.GetValidStr3(data, ref data, HUtil32.Backslash);
                    }
                    else
                    {
                        body2 = body;
                        data = string.Empty;
                    }
                    desc = EDCode.DecodeBuffer<CharDesc>(body2);
                    if (msg.Recog != MShare.MySelf.RecogId)
                    {
                        PlayScene.NewActor(msg.Recog, msg.Param, msg.Tag, msg.Series, desc.Feature, desc.Status);
                    }
                    PlayScene.SendMsg(msg.Ident, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, desc.Feature, desc.Status, "", 0);
                    if (!string.IsNullOrEmpty(data))
                    {
                        Actor = PlayScene.FindActor(msg.Recog);
                        if (Actor != null)
                        {
                            Actor.m_sDescUserName = HUtil32.GetValidStr3(data, ref Actor.UserName, "\\");
                            Actor.NameColor = (byte)HUtil32.StrToInt(str, 0);
                        }
                    }
                    break;
                case Messages.SM_RUSH:
                case Messages.SM_RUSHKUNG:
                    desc = EDCode.DecodeBuffer<CharDesc>(body);
                    if (msg.Recog == MShare.MySelf.RecogId)
                    {
                        PlayScene.SendMsg(msg.Ident, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, desc.Feature, desc.Status, "", 0);
                    }
                    else
                    {
                        PlayScene.SendMsg(msg.Ident, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, desc.Feature, desc.Status, "", 0);
                    }
                    if (msg.Ident == Messages.SM_RUSH)
                    {
                        MShare.g_dwLatestRushRushTick = MShare.GetTickCount();
                    }
                    break;
                case Messages.SM_WALK:
                case Messages.SM_RUN:
                case Messages.SM_HORSERUN:
                    desc = EDCode.DecodeBuffer<CharDesc>(body);
                    if (msg.Recog != MShare.MySelf.RecogId)
                    {
                        PlayScene.SendMsg(msg.Ident, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, desc.Feature, desc.Status, "", 0);
                    }
                    break;
                case Messages.SM_CHANGELIGHT:
                    Actor = PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        Actor.m_nChrLight = msg.Param;
                    }
                    break;
                case Messages.SM_LAMPCHANGEDURA:
                    if (MShare.UseItems[ItemLocation.RighThand].Item.Name != "")
                    {
                        MShare.UseItems[ItemLocation.RighThand].Dura = (ushort)msg.Recog;
                    }
                    break;
                case Messages.SM_MOVEFAIL:
                    ActionFailed();
                    ActionLock = false;
                    RecalcAutoMovePath();
                    desc = EDCode.DecodeBuffer<CharDesc>(body);
                    ActionFailLock = false;
                    PlayScene.SendMsg(Messages.SM_TURN, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, desc.Feature, desc.Status, "", 0);
                    break;
                case Messages.SM_BUTCH:// 挖肉动作封包
                    desc = EDCode.DecodeBuffer<CharDesc>(body);
                    if (msg.Recog != MShare.MySelf.RecogId)
                    {
                        Actor = PlayScene.FindActor(msg.Recog);
                        if (Actor != null)
                        {
                            Actor.SendMsg(Messages.SM_SITDOWN, msg.Param, msg.Tag, msg.Series, 0, 0, "", 0);
                        }
                    }
                    break;
                case Messages.SM_SITDOWN:// 蹲下动作封包
                    desc = EDCode.DecodeBuffer<CharDesc>(body);
                    if (msg.Recog != MShare.MySelf.RecogId)
                    {
                        Actor = PlayScene.FindActor(msg.Recog);
                        if (Actor != null)
                        {
                            Actor.SendMsg(Messages.SM_SITDOWN, msg.Param, msg.Tag, msg.Series, 0, 0, "", 0);
                        }
                    }
                    break;
                case Messages.SM_HIT:
                case Messages.SM_HEAVYHIT:
                case Messages.SM_POWERHIT:
                case Messages.SM_LONGHIT:
                case Messages.SM_CRSHIT:
                case Messages.SM_WIDEHIT:
                case Messages.SM_BIGHIT:
                case Messages.SM_FIREHIT:
                    if (msg.Recog != MShare.MySelf.RecogId)
                    {
                        Actor = PlayScene.FindActor(msg.Recog);
                        if (Actor != null)
                        {
                            Actor.SendMsg(msg.Ident, msg.Param, msg.Tag, msg.Series, 0, 0, body, 0);
                            if (msg.Ident == Messages.SM_HEAVYHIT)
                            {
                                if (!string.IsNullOrEmpty(body))
                                {
                                    Actor.m_boDigFragment = true;
                                }
                            }
                        }
                    }
                    break;
                case Messages.SM_FLYAXE:
                    var mbw = EDCode.DecodeBuffer<MessageBodyW>(body);
                    Actor = PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        Actor.SendMsg(msg.Ident, msg.Param, msg.Tag, msg.Series, 0, 0, "", 0);
                        Actor.m_nTargetX = mbw.Param1;
                        Actor.m_nTargetY = mbw.Param2;
                        Actor.m_nTargetRecog = HUtil32.MakeLong(mbw.Tag1, mbw.Tag2);
                    }
                    break;
                // Modify the A .. B: Messages.SM_LIGHTING, Messages.SM_LIGHTING_1 .. Messages.SM_LIGHTING_3
                case Messages.SM_LIGHTING:
                    wl = EDCode.DecodeBuffer<MessageBodyWL>(body);
                    Actor = PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        Actor.SendMsg(msg.Ident, msg.Param, msg.Tag, msg.Series, 0, 0, "", 0);
                        Actor.m_nTargetX = wl.Param1;
                        Actor.m_nTargetY = wl.Param2;
                        Actor.m_nTargetRecog = wl.Tag1;
                        Actor.m_nMagicNum = wl.Tag2;
                    }
                    break;
                case Messages.SM_SPELL:
                    UseMagicSpell(msg.Recog, msg.Series, msg.Param, msg.Tag, HUtil32.StrToInt(body, 0));
                    break;
                case Messages.SM_MAGICFIRE:
                    desc = EDCode.DecodeBuffer<CharDesc>(body);
                    UseMagicFire(msg.Recog, HUtil32.LoByte(msg.Series), HUtil32.HiByte(msg.Series), msg.Param, msg.Tag, desc.Feature, desc.Status);
                    break;
                case Messages.SM_MAGICFIRE_FAIL:
                    UseMagicFireFail(msg.Recog);
                    break;
                case Messages.SM_OUTOFCONNECTION:
                    MainOutMessage("服务器连接被强行中断。连接时间可能超过限制");
                    LoginOut();
                    break;
                case Messages.SM_DEATH:
                case Messages.SM_NOWDEATH:
                    desc = EDCode.DecodeBuffer<CharDesc>(body);
                    Actor = PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        Actor.SendMsg(msg.Ident, msg.Param, msg.Tag, msg.Series, desc.Feature, desc.Status, "", 0);
                        Actor.Abil.HP = 0;
                        Actor.m_nIPower = -1;
                    }
                    else
                    {
                        PlayScene.SendMsg(Messages.SM_DEATH, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, desc.Feature, desc.Status, "", 0);
                    }
                    MainOutMessage("啊,我死了,快救我");
                    break;
                case Messages.SM_SKELETON:
                    desc = EDCode.DecodeBuffer<CharDesc>(body);
                    PlayScene.SendMsg(Messages.SM_SKELETON, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, desc.Feature, desc.Status, "", 0);
                    break;
                case Messages.SM_ALIVE:
                    desc = EDCode.DecodeBuffer<CharDesc>(body);
                    PlayScene.SendMsg(Messages.SM_ALIVE, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, desc.Feature, desc.Status, "", 0);
                    break;
                case Messages.SM_ABILITY:
                    MShare.MySelf.m_nGold = msg.Recog;
                    MShare.MySelf.Job = HUtil32.LoByte(msg.Param);
                    MShare.MySelf.m_nIPowerLvl = HUtil32.HiByte(msg.Param);
                    MShare.MySelf.m_nGameGold = HUtil32.MakeLong(msg.Tag, msg.Series);
                    MShare.MySelf.Abil = EDCode.DecodeBuffer<Ability>(body);
                    break;
                case Messages.SM_SUBABILITY:
                    MShare.g_nMyHitPoint = HUtil32.LoByte(msg.Param);
                    MShare.g_nMySpeedPoint = HUtil32.HiByte(msg.Param);
                    MShare.g_nMyAntiPoison = HUtil32.LoByte(msg.Tag);
                    MShare.g_nMyPoisonRecover = HUtil32.HiByte(msg.Tag);
                    MShare.g_nMyHealthRecover = HUtil32.LoByte(msg.Series);
                    MShare.g_nMySpellRecover = HUtil32.HiByte(msg.Series);
                    MShare.g_nMyAntiMagic = HUtil32.LoByte(HUtil32.LoWord(msg.Recog));
                    MShare.g_nMyIPowerRecover = HUtil32.HiByte(HUtil32.LoWord(msg.Recog));
                    MShare.g_nMyAddDamage = HUtil32.LoByte(HUtil32.HiWord(msg.Recog));
                    MShare.g_nMyDecDamage = HUtil32.HiByte(HUtil32.HiWord(msg.Recog));
                    break;
                case Messages.SM_DAYCHANGING:
                    
                    break;
                case Messages.SM_WINEXP:
                    MShare.MySelf.Abil.Exp = msg.Recog;
                    if (!MShare.g_gcGeneral[3] || HUtil32.MakeLong(msg.Param, msg.Tag) > MShare.g_MaxExpFilter)
                    {
                        DScreen.AddSysMsg($"经验值 +{HUtil32.MakeLong(msg.Param, msg.Tag)}");
                    }
                    break;
                case Messages.SM_LEVELUP:
                    MShare.MySelf.Abil.Level = (byte)msg.Param;
                    DScreen.AddSysMsg("您的等级已升级！");
                    break;
                case Messages.SM_HEALTHSPELLCHANGED:
                    Actor = PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        Actor.Abil.HP = msg.Param;
                        Actor.Abil.MP = msg.Tag;
                        Actor.Abil.MaxHP = msg.Series;
                    }
                    break;
                case Messages.SM_STRUCK:
                    wl = EDCode.DecodeBuffer<MessageBodyWL>(body);
                    Actor = PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        if (MShare.g_gcGeneral[13] && msg.Series > 0)
                        {
                            Actor.GetMoveHPShow(msg.Series);
                        }
                        if (Actor != MShare.MySelf)
                        {
                            if (Actor.CanCancelAction())
                            {
                                Actor.CancelAction();
                            }
                        }
                        if (Actor != MShare.MySelf)
                        {
                            if (Actor.Race != 0 || !MShare.g_gcGeneral[15])
                            {
                                Actor.UpdateMsg(Messages.SM_STRUCK, (ushort)wl.Tag2, 0, msg.Series, wl.Param1, wl.Param2, "", wl.Tag1);
                            }
                        }
                        Actor.Abil.HP = msg.Param;
                        Actor.Abil.MaxHP = msg.Tag;
                        if (MShare.OpenAutoPlay && TimerAutoPlay.Enabled) //  自己受人攻击,小退
                        {
                            Actor2 = PlayScene.FindActor(wl.Tag1);
                            if (Actor2 == null || (Actor2.Race != 0 && Actor2.m_btIsHero != 1))
                            {
                                return;
                            }
                            if (MShare.MySelf != null)
                            {
                                if (Actor == MShare.MySelf) // 自己受人攻击,小退
                                {
                                    MShare.g_nAPReLogon = 1;
                                    MShare.g_nOverAPZone2 = MShare.g_nOverAPZone;
                                    MShare.g_APGoBack2 = MShare.g_APGoBack;
                                    if (MShare.MapPath != null)
                                    {
                                        MShare.g_APMapPath2 = new Point[MShare.MapPath.Length + 1];
                                        for (i = 0; i < MShare.MapPath.Length; i++)
                                        {
                                            MShare.g_APMapPath2[i] = MShare.MapPath[i];
                                        }
                                    }
                                    MShare.AutoLastPoint2 = MShare.AutoLastPoint;
                                    MShare.g_APStep2 = MShare.AutoStep;
                                    AppLogout();
                                    // SaveBagsData();
                                }
                            }
                        }
                    }
                    break;
                case Messages.SM_CHANGEFACE:
                    Actor = PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        desc = EDCode.DecodeBuffer<CharDesc>(body);
                        Actor.m_nWaitForRecogId = HUtil32.MakeLong(msg.Param, msg.Tag);
                        Actor.m_nWaitForFeature = desc.Feature;
                        Actor.m_nWaitForStatus = desc.Status;
                        ClFunc.AddChangeFace(Actor.m_nWaitForRecogId);
                    }
                    break;
                case Messages.SM_PASSWORD:
                    break;
                case Messages.SM_OPENHEALTH:
                    Actor = PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        if (Actor != MShare.MySelf)
                        {
                            Actor.Abil.HP = msg.Param;
                            Actor.Abil.MaxHP = msg.Tag;
                        }
                        Actor.OpenHealth = true;
                    }
                    break;
                case Messages.SM_CLOSEHEALTH:
                    Actor = PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        Actor.OpenHealth = false;
                    }
                    break;
                case Messages.SM_INSTANCEHEALGUAGE:
                    Actor = PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        Actor.Abil.HP = msg.Param;
                        Actor.Abil.MaxHP = msg.Tag;
                        Actor.m_noInstanceOpenHealth = true;
                        Actor.m_dwOpenHealthTime = 2 * 1000;
                        Actor.m_dwOpenHealthStart = MShare.GetTickCount();
                    }
                    break;
                case Messages.SM_BREAKWEAPON:
                    Actor = PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        if (Actor is THumActor)
                        {
                            ((THumActor)Actor).DoWeaponBreakEffect();
                        }
                    }
                    break;
                case Messages.SM_HEAR:
                case Messages.SM_CRY:
                case Messages.SM_GROUPMESSAGE:
                case Messages.SM_GUILDMESSAGE:
                case Messages.SM_WHISPER:
                case Messages.SM_SYSMESSAGE:
                    str = EDCode.DeCodeString(body);
                    if (msg.Tag > 0)
                    {
                        ScreenManager.AddChatBoardString(str);
                        return;
                    }
                    if (msg.Ident == Messages.SM_WHISPER)
                    {
                        HUtil32.GetValidStr3(str, ref str3, new string[] { " ", "=", ">" });
                        ScreenManager.AddChatBoardString(str);
                    }
                    else
                    {
                        ScreenManager.AddChatBoardString(str);
                    }
                    if (msg.Ident == Messages.SM_HEAR)
                    {
                        Actor = PlayScene.FindActor(msg.Recog);
                        if (Actor != null)
                        {
                            Actor.Say(str);
                        }
                    }
                    break;
                case Messages.SM_USERNAME:
                    str = EDCode.DeCodeString(body);
                    Actor = PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        Actor.m_sDescUserName = HUtil32.GetValidStr3(str, ref Actor.UserName, "\\");
                        Actor.NameColor = (byte)msg.Param;
                        if (msg.Tag >= 1 && msg.Tag <= 5)
                        {
                            Actor.m_btAttribute = (byte)msg.Tag;
                        }
                    }
                    break;
                case Messages.SM_CHANGENAMECOLOR:
                    Actor = PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        Actor.NameColor = (byte)msg.Param;
                    }
                    break;
                case Messages.SM_HIDE:
                case Messages.SM_GHOST:
                case Messages.SM_DISAPPEAR:
                    if (MShare.MySelf.RecogId != msg.Recog)
                    {
                        PlayScene.SendMsg(Messages.SM_HIDE, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, 0, 0, "");
                    }
                    break;
                case Messages.SM_DIGUP:
                    wl = EDCode.DecodeBuffer<MessageBodyWL>(body);
                    Actor = PlayScene.FindActor(msg.Recog);
                    if (Actor == null)
                    {
                        Actor = PlayScene.NewActor(msg.Recog, msg.Param, msg.Tag, msg.Series, wl.Param1, wl.Param2);
                    }
                    Actor.m_nCurrentEvent = wl.Tag1;
                    Actor.SendMsg(Messages.SM_DIGUP, msg.Param, msg.Tag, msg.Series, wl.Param1, wl.Param2, "", 0);
                    break;
                case Messages.SM_DIGDOWN:
                    PlayScene.SendMsg(Messages.SM_DIGDOWN, msg.Recog, msg.Param, msg.Tag, 0, 0, 0, "");
                    break;
                case Messages.SM_SHOWEVENT:
                    //ShortMessage sMsg = EDCode.DecodeBuffer<ShortMessage>(body);
                    //__event = new TClEvent(msg.Recog, HUtil32.LoWord(msg.Tag), msg.Series, msg.Param);
                    //__event.m_nDir = 0;
                    //__event.m_nEventParam = sMsg.Ident;
                    //__event.m_nEventLevel = sMsg.wMsg;
                    //EventMan.AddEvent(__event);
                    break;
                case Messages.SM_HIDEEVENT:
                    //EventMan.DelEventById(msg.Recog);
                    break;
                case Messages.SM_ADDITEM:
                    ClientGetAddItem(msg.Series, body);
                    break;
                case Messages.SM_BAGITEMS:
                    ClientGetBagItmes(body);
                    break;
                case Messages.SM_UPDATEITEM:
                    ClientGetUpdateItem(body);
                    break;
                case Messages.SM_DELITEM:
                    ClientGetDelItem(body);
                    break;
                case Messages.SM_DELITEMS:
                    ClientGetDelItems(body, msg.Param);
                    break;
                case Messages.SM_DROPITEM_SUCCESS:
                    ClFunc.DelDropItem(EDCode.DeCodeString(body), msg.Recog);
                    break;
                case Messages.SM_DROPITEM_FAIL:
                    ClientGetDropItemFail(EDCode.DeCodeString(body), msg.Recog);
                    break;
                case Messages.SM_ITEMSHOW:
                    ClientGetShowItem(msg.Recog, (short)msg.Param, (short)msg.Tag, msg.Series, EDCode.DeCodeString(body));
                    break;
                case Messages.SM_ITEMHIDE:
                    ClientGetHideItem(msg.Recog, msg.Param, msg.Tag);
                    break;
                case Messages.SM_OPENDOOR_OK:
                    Map.OpenDoor(msg.Param, msg.Tag);
                    break;
                case Messages.SM_OPENDOOR_LOCK:
                    DScreen.AddSysMsg("此门被锁定");
                    break;
                case Messages.SM_CLOSEDOOR:
                    Map.CloseDoor(msg.Param, msg.Tag);
                    break;
                case Messages.SM_TAKEON_OK:
                    MShare.MySelf.m_nFeature = msg.Recog;
                    MShare.MySelf.FeatureChanged();
                    if (MShare.g_WaitingUseItem.Item.Item.Name != "")
                    {
                        if (MShare.g_WaitingUseItem.Index >= 0 && MShare.g_WaitingUseItem.Index <= 13)
                        {
                            MShare.UseItems[MShare.g_WaitingUseItem.Index] = MShare.g_WaitingUseItem.Item;
                        }
                        MShare.g_WaitingUseItem.Item.Item.Name = "";
                    }
                    break;
                case Messages.SM_TAKEON_FAIL:
                    if (MShare.g_WaitingUseItem.Item.Item.Name != "")
                    {
                        ClFunc.AddItemBag(MShare.g_WaitingUseItem.Item);
                        MShare.g_WaitingUseItem.Item.Item.Name = "";
                    }
                    break;
                case Messages.SM_TAKEOFF_OK:
                    MShare.MySelf.m_nFeature = msg.Recog;
                    MShare.MySelf.FeatureChanged();
                    MShare.g_WaitingUseItem.Item.Item.Name = "";
                    break;
                case Messages.SM_TAKEOFF_FAIL:
                    if (MShare.g_WaitingUseItem.Item.Item.Name != "")
                    {
                        if (MShare.g_WaitingUseItem.Index < 0)
                        {
                            n = -(MShare.g_WaitingUseItem.Index + 1);
                            MShare.UseItems[n] = MShare.g_WaitingUseItem.Item;
                        }
                        MShare.g_WaitingUseItem.Item.Item.Name = "";
                    }
                    break;
                case Messages.SM_SENDUSEITEMS:
                    ClientGetSendUseItems(body);
                    break;
                case Messages.SM_WEIGHTCHANGED:
                    MShare.MySelf.Abil.Weight = (ushort)msg.Recog;
                    MShare.MySelf.Abil.WearWeight = (byte)msg.Param;
                    MShare.MySelf.Abil.HandWeight = (byte)msg.Tag;
                    break;
                case Messages.SM_GOLDCHANGED:
                    if (msg.Recog > MShare.MySelf.m_nGold)
                    {
                        DScreen.AddSysMsg("获得 " + (msg.Recog - MShare.MySelf.m_nGold).ToString() + BotConst.GoldName);
                    }
                    MShare.MySelf.m_nGold = msg.Recog;
                    MShare.MySelf.m_nGameGold = HUtil32.MakeLong(msg.Param, msg.Tag);
                    break;
                case Messages.SM_FEATURECHANGED:
                    PlayScene.SendMsg(msg.Ident, msg.Recog, 0, 0, 0, HUtil32.MakeLong(msg.Param, msg.Tag), HUtil32.MakeLong(msg.Series, 0), body);
                    break;
                case Messages.SM_CHARSTATUSCHANGED:
                    if (!string.IsNullOrEmpty(body))
                    {
                        PlayScene.SendMsg(msg.Ident, msg.Recog, 0, 0, 0, HUtil32.MakeLong(msg.Param, msg.Tag), msg.Series, EDCode.DeCodeString(body));
                    }
                    else
                    {
                        PlayScene.SendMsg(msg.Ident, msg.Recog, 0, 0, 0, HUtil32.MakeLong(msg.Param, msg.Tag), msg.Series, "");
                    }
                    break;
                case Messages.SM_CLEAROBJECTS:
                    MShare.g_boMapMoving = true;
                    break;
                case Messages.SM_EAT_OK:
                    if (msg.Recog != 0)
                    {
                        str = "";
                        if (msg.Recog != MShare.g_EatingItem.MakeIndex)
                        {
                            for (i = BotConst.MaxBagItemcl - 1; i >= 0; i--)
                            {
                                if (MShare.ItemArr[i].Item.Name != "")
                                {
                                    if (MShare.ItemArr[i].MakeIndex == MShare.g_EatingItem.MakeIndex)
                                    {
                                        ClFunc.DelStallItem(MShare.ItemArr[i]);
                                        str = MShare.ItemArr[i].Item.Name;
                                        MShare.ItemArr[i].Item.Name = "";
                                        break;
                                    }
                                }
                            }
                        }
                        if (str == "")
                        {
                            str = MShare.g_EatingItem.Item.Name;
                            if (MBoSupplyItem)
                            {
                                if (MNEatRetIdx >= 0 && MNEatRetIdx <= 5)
                                {
                                    AutoSupplyBeltItem(MShare.g_EatingItem.Item.AniCount, MNEatRetIdx, str);
                                }
                                else
                                {
                                    AutoSupplyBagItem(MShare.g_EatingItem.Item.AniCount, str);
                                }
                                MBoSupplyItem = false;
                            }
                        }
                        MShare.g_EatingItem.Item.Name = "";
                        ClFunc.ArrangeItembag();
                        MNEatRetIdx = -1;
                    }
                    break;
                case Messages.SM_EAT_FAIL:
                    if (msg.Recog == MShare.g_EatingItem.MakeIndex)
                    {
                        // DScreen.AddChatBoardString(g_EatingItem.Item.Name + ' ' + IntToStr(msg.tag), clRed, clWhite);
                        if (msg.Tag > 0)
                        {
                            MShare.g_EatingItem.Dura = msg.Tag;
                        }
                        ClFunc.AddItemBag(MShare.g_EatingItem, MNEatRetIdx);
                        MShare.g_EatingItem.Item.Name = "";
                        MNEatRetIdx = -1;
                    }
                    MBoSupplyItem = false;
                    switch (msg.Series)
                    {
                        case 1:
                            ScreenManager.AddChatBoardString("[失败] 你的金币不足，不能释放积灵珠！");
                            break;
                        case 2:
                            ScreenManager.AddChatBoardString("[失败] 你的元宝不足，不能释放积灵珠！");
                            break;
                        case 3:
                            ScreenManager.AddChatBoardString("[失败] 你的金刚石不足，不能释放积灵珠！");
                            break;
                        case 4:
                            ScreenManager.AddChatBoardString("[失败] 你的灵符不足，不能释放积灵珠！");
                            break;
                    }
                    break;
                case Messages.SM_ADDMAGIC:
                    if (!string.IsNullOrEmpty(body))
                    {
                        ClientGetAddMagic(body);
                    }
                    break;
                case Messages.SM_SENDMYMAGIC:
                    if (!string.IsNullOrEmpty(body))
                    {
                        ClientGetMyMagics(body);
                    }
                    break;
                case Messages.SM_DELMAGIC:
                    ClientGetDelMagic(msg.Recog, msg.Param);
                    break;
                case Messages.SM_MAGIC_LVEXP:
                    ClientGetMagicLvExp(msg.Recog, msg.Param, HUtil32.MakeLong(msg.Tag, msg.Series));
                    break;
                case Messages.SM_DURACHANGE:
                    ClientGetDuraChange(msg.Param, msg.Recog, HUtil32.MakeLong(msg.Tag, msg.Series));
                    break;
                case Messages.SM_MERCHANTSAY:
                    ClientGetMerchantSay(msg.Recog, msg.Param, EDCode.DeCodeString(body));
                    break;
                case Messages.SM_SENDGOODSLIST:
                    ClientGetSendGoodsList(msg.Recog, msg.Param, body);
                    break;
                case Messages.SM_SENDUSERMAKEDRUGITEMLIST:
                    ClientGetSendMakeDrugList(msg.Recog, body);
                    break;
                case Messages.SM_SENDUSERSELL:
                    ClientGetSendUserSell(msg.Recog);
                    break;
                case Messages.SM_SENDUSERREPAIR:
                    ClientGetSendUserRepair(msg.Recog);
                    break;
                case Messages.SM_SENDBUYPRICE:
                    if (MShare.g_SellDlgItem.Item.Name != "")
                    {
                        if (msg.Recog > 0)
                        {
                            MShare.g_sSellPriceStr = msg.Recog.ToString() + BotConst.GoldName;
                        }
                        else
                        {
                            MShare.g_sSellPriceStr = "???? " + BotConst.GoldName;
                        }
                    }
                    break;
                case Messages.SM_USERSELLITEM_OK:
                    LastestClickTime = MShare.GetTickCount();
                    MShare.MySelf.m_nGold = msg.Recog;
                    MShare.g_SellDlgItemSellWait.Item.Item.Name = "";
                    break;
                case Messages.SM_USERSELLITEM_FAIL:
                    LastestClickTime = MShare.GetTickCount();
                    ClFunc.AddItemBag(MShare.g_SellDlgItemSellWait.Item);
                    MShare.g_SellDlgItemSellWait.Item.Item.Name = "";
                    MainOutMessage("此物品不能出售");
                    break;
                case Messages.SM_SENDREPAIRCOST:
                    if (MShare.g_SellDlgItem.Item.Name != "")
                    {
                        if (msg.Recog >= 0)
                        {
                            // 金币
                            MShare.g_sSellPriceStr = msg.Recog.ToString() + " " + BotConst.GoldName;
                        }
                        else
                        {
                            // 金币
                            MShare.g_sSellPriceStr = "???? " + BotConst.GoldName;
                        }
                    }
                    break;
                case Messages.SM_USERREPAIRITEM_OK:
                    if (MShare.g_SellDlgItemSellWait.Item.Item.Name != "")
                    {
                        LastestClickTime = MShare.GetTickCount();
                        MShare.MySelf.m_nGold = msg.Recog;
                        MShare.g_SellDlgItemSellWait.Item.Dura = msg.Param;
                        MShare.g_SellDlgItemSellWait.Item.DuraMax = msg.Tag;
                        ClFunc.AddItemBag(MShare.g_SellDlgItemSellWait.Item);
                        MShare.g_SellDlgItemSellWait.Item.Item.Name = "";
                    }
                    break;
                case Messages.SM_USERREPAIRITEM_FAIL:
                    LastestClickTime = MShare.GetTickCount();
                    ClFunc.AddItemBag(MShare.g_SellDlgItemSellWait.Item);
                    MShare.g_SellDlgItemSellWait.Item.Item.Name = "";
                    MainOutMessage("您不能修理此物品");
                    break;
                case Messages.SM_STORAGE_OK:
                case Messages.SM_STORAGE_FULL:
                case Messages.SM_STORAGE_FAIL:
                    //LastestClickTime = MShare.GetTickCount();
                    //if (msg.Ident != Messages.SM_STORAGE_OK)
                    //{
                    //    if (msg.Ident == Messages.SM_STORAGE_FULL)
                    //    {
                    //        DebugOutStr("您的个人仓库已经满了，不能再保管任何东西了");
                    //    }
                    //    else
                    //    {
                    //        if (msg.Recog == 2)
                    //        {
                    //            DebugOutStr("寄存物品失败,同类单个物品最高重叠数量是 " + Grobal2.MAX_OVERLAPITEM.ToString());
                    //        }
                    //        else if (msg.Recog == 3)
                    //        {
                    //            MShare.g_SellDlgItemSellWait.Item.Dura = MShare.g_SellDlgItemSellWait.Item.Dura - msg.Param;
                    //            DScreen.AddChatBoardString(string.Format("成功寄存 %s %d个", MShare.g_SellDlgItemSellWait.Item.Item.Name, msg.Param), Color.Blue);
                    //        }
                    //        else
                    //        {
                    //            DebugOutStr("您不能寄存物品");
                    //        }
                    //    }
                    //    ClFunc.AddItemBag(MShare.g_SellDlgItemSellWait.Item);
                    //}
                    MShare.g_SellDlgItemSellWait.Item.Item.Name = "";
                    break;
                case Messages.SM_SAVEITEMLIST:
                    ClientGetSaveItemList(msg.Recog, body);
                    break;
                case Messages.SM_TAKEBACKSTORAGEITEM_OK:
                case Messages.SM_TAKEBACKSTORAGEITEM_FAIL:
                case Messages.SM_TAKEBACKSTORAGEITEM_FULLBAG:
                    LastestClickTime = MShare.GetTickCount();
                    if (msg.Ident != Messages.SM_TAKEBACKSTORAGEITEM_OK)
                    {
                        if (msg.Ident == Messages.SM_TAKEBACKSTORAGEITEM_FULLBAG)
                        {
                            MainOutMessage("您无法携带更多物品了");
                        }
                        else
                        {
                            MainOutMessage("您无法取回物品");
                        }
                    }
                    break;
                case Messages.SM_BUYITEM_SUCCESS:
                    LastestClickTime = MShare.GetTickCount();
                    MShare.MySelf.m_nGold = msg.Recog;
                    break;
                case Messages.SM_BUYITEM_FAIL:
                    LastestClickTime = MShare.GetTickCount();
                    switch (msg.Recog)
                    {
                        case 1:
                            MainOutMessage("此物品被卖出");
                            break;
                        case 2:
                            MainOutMessage("您无法携带更多物品了");
                            break;
                        case 3:
                            MainOutMessage("您没有足够的钱来购买此物品");
                            break;
                    }
                    break;
                case Messages.SM_MAKEDRUG_SUCCESS:
                    LastestClickTime = MShare.GetTickCount();
                    MShare.MySelf.m_nGold = msg.Recog;
                    MainOutMessage("您要的物品已经搞定了");
                    break;
                case Messages.SM_MAKEDRUG_FAIL:
                    LastestClickTime = MShare.GetTickCount();
                    switch (msg.Recog)
                    {
                        case 1:
                            MainOutMessage("未知错误");
                            break;
                        case 2:
                            MainOutMessage("发生了错误");
                            break;
                        case 3:
                            MainOutMessage(BotConst.GoldName + "不足");
                            break;
                        case 4:
                            MainOutMessage("你缺乏所必需的物品");
                            break;
                    }
                    break;
                case Messages.SM_SENDDETAILGOODSLIST:
                    ClientGetSendDetailGoodsList(msg.Recog, msg.Param, msg.Tag, body);
                    break;
                case Messages.SM_TEST:
                    MShare.g_nTestReceiveCount++;
                    break;
                case Messages.SM_SENDNOTICE:
                    ClientGetSendNotice(body);
                    break;
                case Messages.SM_GROUPMODECHANGED:
                    if (msg.Param > 0)
                    {
                        MShare.g_boAllowGroup = true;
                        ScreenManager.AddChatBoardString("[开启组队开关]");
                    }
                    else
                    {
                        MShare.g_boAllowGroup = false;
                        ScreenManager.AddChatBoardString("[关闭组队开关]");
                    }
                    MShare.g_dwChangeGroupModeTick = MShare.GetTickCount();
                    break;
                case Messages.SM_CREATEGROUP_OK:
                    MShare.g_dwChangeGroupModeTick = MShare.GetTickCount();
                    MShare.g_boAllowGroup = true;
                    break;
                case Messages.SM_CREATEGROUP_FAIL:
                    MShare.g_dwChangeGroupModeTick = MShare.GetTickCount();
                    switch (msg.Recog)
                    {
                        case -1:
                            MainOutMessage("编组还未成立或者你还不够等级创建！");
                            break;
                        case -2:
                            MainOutMessage("输入的人物名称不正确！");
                            break;
                        case -3:
                            MainOutMessage("您想邀请加入编组的人已经加入了其它组！");
                            break;
                        case -4:
                            MainOutMessage("对方不允许编组！");
                            break;
                    }
                    break;
                case Messages.SM_GROUPADDMEM_OK:
                    MShare.g_dwChangeGroupModeTick = MShare.GetTickCount();
                    break;
                case Messages.SM_GROUPADDMEM_FAIL:
                    // GroupMembers.Add (DeCodeString(body));
                    MShare.g_dwChangeGroupModeTick = MShare.GetTickCount();
                    switch (msg.Recog)
                    {
                        case -1:
                            MainOutMessage("编组还未成立或者你还不够等级创建！");
                            break;
                        case -2:
                            MainOutMessage("输入的人物名称不正确！");
                            break;
                        case -3:
                            MainOutMessage("已经加入编组！");
                            break;
                        case -4:
                            MainOutMessage("对方不允许编组！");
                            break;
                        case -5:
                            MainOutMessage("您想邀请加入编组的人已经加入了其它组！");
                            break;
                    }
                    break;
                case Messages.SM_GROUPDELMEM_OK:
                    MShare.g_dwChangeGroupModeTick = MShare.GetTickCount();
                    break;
                case Messages.SM_GROUPDELMEM_FAIL:
                    MShare.g_dwChangeGroupModeTick = MShare.GetTickCount();
                    switch (msg.Recog)
                    {
                        case -1:
                            MainOutMessage("编组还未成立或者您还不够等级创建");
                            break;
                        case -2:
                            MainOutMessage("输入的人物名称不正确！");
                            break;
                        case -3:
                            MainOutMessage("此人不在本组中！");
                            break;
                    }
                    break;
                case Messages.SM_GROUPCANCEL:
                    MShare.g_GroupMembers.Clear();
                    break;
                case Messages.SM_GROUPMEMBERS:
                    ClientGetGroupMembers(EDCode.DeCodeString(body));
                    break;
                case Messages.SM_OPENGUILDDLG:
                    MShare.g_dwQueryMsgTick = MShare.GetTickCount();
                    break;
                case Messages.SM_SENDGUILDMEMBERLIST:
                    MShare.g_dwQueryMsgTick = MShare.GetTickCount();
                    break;
                case Messages.SM_OPENGUILDDLG_FAIL:
                    MShare.g_dwQueryMsgTick = MShare.GetTickCount();
                    MainOutMessage("您还没有加入行会！");
                    break;
                case Messages.SM_DEALTRY_FAIL:
                    MShare.g_dwQueryMsgTick = MShare.GetTickCount();
                    MainOutMessage("只有二人面对面才能进行交易");
                    break;
                case Messages.SM_DEALMENU:
                    MShare.g_dwQueryMsgTick = MShare.GetTickCount();
                    MShare.g_sDealWho = EDCode.DeCodeString(body);
                    break;
                case Messages.SM_DEALCANCEL:
                    ClFunc.MoveDealItemToBag();
                    if (MShare.g_DealDlgItem.Item.Name != "")
                    {
                        ClFunc.AddItemBag(MShare.g_DealDlgItem);
                        MShare.g_DealDlgItem.Item.Name = "";
                    }
                    if (MShare.g_nDealGold > 0)
                    {
                        MShare.MySelf.m_nGold = MShare.MySelf.m_nGold + MShare.g_nDealGold;
                        MShare.g_nDealGold = 0;
                    }
                    break;
                case Messages.SM_DEALADDITEM_OK:
                    MShare.g_dwDealActionTick = MShare.GetTickCount();
                    if (MShare.g_DealDlgItem.Item.Name != "")
                    {
                        ClFunc.ResultDealItem(MShare.g_DealDlgItem, msg.Recog, msg.Param);
                        MShare.g_DealDlgItem.Item.Name = "";
                    }
                    break;
                case Messages.SM_DEALADDITEM_FAIL:
                    MShare.g_dwDealActionTick = MShare.GetTickCount();
                    if (MShare.g_DealDlgItem.Item.Name != "")
                    {
                        ClFunc.AddItemBag(MShare.g_DealDlgItem);
                        MShare.g_DealDlgItem.Item.Name = "";
                    }
                    if (msg.Recog != 0)
                    {
                        // DScreen.AddChatBoardString("重叠失败,物品最高数量是 " + Grobal2.MAX_OVERLAPITEM.ToString(), Color.Red);
                    }
                    break;
                case Messages.SM_DEALDELITEM_OK:
                    MShare.g_dwDealActionTick = MShare.GetTickCount();
                    if (MShare.g_DealDlgItem.Item.Name != "")
                    {
                        MShare.g_DealDlgItem.Item.Name = "";
                    }
                    break;
                case Messages.SM_DEALDELITEM_FAIL:
                    MShare.g_dwDealActionTick = MShare.GetTickCount();
                    if (MShare.g_DealDlgItem.Item.Name != "")
                    {
                        ClFunc.AddDealItem(MShare.g_DealDlgItem);
                        MShare.g_DealDlgItem.Item.Name = "";
                    }
                    break;
                case Messages.SM_DEALREMOTEADDITEM:
                    ClientGetDealRemoteAddItem(body);
                    break;
                case Messages.SM_DEALREMOTEDELITEM:
                    ClientGetDealRemoteDelItem(body);
                    break;
                case Messages.SM_DEALCHGGOLD_OK:
                    MShare.g_nDealGold = msg.Recog;
                    MShare.MySelf.m_nGold = HUtil32.MakeLong(msg.Param, msg.Tag);
                    MShare.g_dwDealActionTick = MShare.GetTickCount();
                    break;
                case Messages.SM_DEALCHGGOLD_FAIL:
                    MShare.g_nDealGold = msg.Recog;
                    MShare.MySelf.m_nGold = HUtil32.MakeLong(msg.Param, msg.Tag);
                    MShare.g_dwDealActionTick = MShare.GetTickCount();
                    break;
                case Messages.SM_DEALREMOTECHGGOLD:
                    MShare.g_nDealRemoteGold = msg.Recog;
                    break;
                case Messages.SM_SENDUSERSTORAGEITEM:
                    ClientGetSendUserStorage(msg.Recog);
                    break;
                case Messages.SM_READMINIMAP_OK:
                    MShare.g_dwQueryMsgTick = MShare.GetTickCount();
                    break;
                case Messages.SM_READMINIMAP_FAIL:
                    MShare.g_dwQueryMsgTick = MShare.GetTickCount();
                    ScreenManager.AddChatBoardString("没有小地图");
                    break;
                case Messages.SM_CHANGEGUILDNAME:
                    ClientGetChangeGuildName(EDCode.DeCodeString(body));
                    break;
                case Messages.SM_SENDUSERSTATE:
                    ClientGetSendUserState(body);
                    break;
                case Messages.SM_GUILDADDMEMBER_OK:
                    SendGuildMemberList();
                    break;
                case Messages.SM_GUILDADDMEMBER_FAIL:
                    switch (msg.Recog)
                    {
                        case 1:
                            MainOutMessage("你没有权利使用这个命令");
                            break;
                        case 2:
                            MainOutMessage("想加入进来的成员应该来面对掌门人");
                            break;
                        case 3:
                            MainOutMessage("对方已经加入我们的行会");
                            break;
                        case 4:
                            MainOutMessage("对方已经加入其他行会");
                            break;
                        case 5:
                            MainOutMessage("对方不允许加入行会");
                            break;
                    }
                    break;
                case Messages.SM_GUILDDELMEMBER_OK:
                    SendGuildMemberList();
                    break;
                case Messages.SM_GUILDDELMEMBER_FAIL:
                    switch (msg.Recog)
                    {
                        case 1:
                            MainOutMessage("不能使用命令！");
                            break;
                        case 2:
                            MainOutMessage("此人非本行会成员！");
                            break;
                        case 3:
                            MainOutMessage("行会掌门人不能开除自己！");
                            break;
                        case 4:
                            MainOutMessage("不能使用命令！");
                            break;
                    }
                    break;
                case Messages.SM_GUILDRANKUPDATE_FAIL:
                    switch (msg.Recog)
                    {
                        case -2:
                            MainOutMessage("[提示信息] 掌门人位置不能为空");
                            break;
                        case -3:
                            MainOutMessage("[提示信息] 新的行会掌门人已经被传位");
                            break;
                        case -4:
                            MainOutMessage("[提示信息] 一个行会最多只能有二个掌门人");
                            break;
                        case -5:
                            MainOutMessage("[提示信息] 掌门人位置不能为空");
                            break;
                        case -6:
                            MainOutMessage("[提示信息] 不能添加成员/删除成员");
                            break;
                        case -7:
                            MainOutMessage("[提示信息] 职位重复或者出错");
                            break;
                    }
                    break;
                case Messages.SM_GUILDMAKEALLY_OK:
                case Messages.SM_GUILDMAKEALLY_FAIL:
                    switch (msg.Recog)
                    {
                        case -1:
                            MainOutMessage("您无此权限！");
                            break;
                        case -2:
                            MainOutMessage("结盟失败！");
                            break;
                        case -3:
                            MainOutMessage("行会结盟必须双方掌门人面对面！");
                            break;
                        case -4:
                            MainOutMessage("对方行会掌门人不允许结盟！");
                            break;
                    }
                    break;
                case Messages.SM_GUILDBREAKALLY_OK:
                case Messages.SM_GUILDBREAKALLY_FAIL:
                    switch (msg.Recog)
                    {
                        case -1:
                            MainOutMessage("解除结盟！");
                            break;
                        case -2:
                            MainOutMessage("此行会不是您行会的结盟行会！");
                            break;
                        case -3:
                            MainOutMessage("没有此行会！");
                            break;
                    }
                    break;
                case Messages.SM_BUILDGUILD_OK:
                    LastestClickTime = MShare.GetTickCount();
                    MainOutMessage("行会建立成功");
                    break;
                case Messages.SM_BUILDGUILD_FAIL:
                    LastestClickTime = MShare.GetTickCount();
                    switch (msg.Recog)
                    {
                        case -1:
                            MainOutMessage("您已经加入其它行会");
                            break;
                        case -2:
                            MainOutMessage("缺少创建费用");
                            break;
                        case -3:
                            MainOutMessage("你没有准备好需要的全部物品");
                            break;
                        default:
                            MainOutMessage("创建行会失败！！！");
                            break;
                    }
                    break;
                case Messages.SM_MENU_OK:
                    LastestClickTime = MShare.GetTickCount();
                    if (!string.IsNullOrEmpty(body))
                    {
                        MainOutMessage(EDCode.DeCodeString(body));
                    }
                    break;
                case Messages.SM_DLGMSG:
                    if (!string.IsNullOrEmpty(body))
                    {
                        MainOutMessage(EDCode.DeCodeString(body));
                    }
                    break;
                case Messages.SM_DONATE_OK:
                    LastestClickTime = MShare.GetTickCount();
                    break;
                case Messages.SM_DONATE_FAIL:
                    LastestClickTime = MShare.GetTickCount();
                    break;
                case Messages.SM_PLAYDICE:
                    //n = HUtil32.GetCodeMsgSize(sizeof(TMessageBodyWL) * 4 / 3);
                    body2 = body.Substring(n + 1 - 1, body.Length);
                    data = EDCode.DeCodeString(body2);
                    body2 = body[..n];
                    wl = EDCode.DecodeBuffer<MessageBodyWL>(body2);
                    //FrmDlg.m_nDiceCount = msg.Param;
                    //FrmDlg.m_Dice[0].nDicePoint = HUtil32.LoByte(HUtil32.LoWord(wl.lParam1));
                    //FrmDlg.m_Dice[1].nDicePoint = HUtil32.HiByte(HUtil32.LoWord(wl.lParam1));
                    //FrmDlg.m_Dice[2].nDicePoint = HUtil32.LoByte(HUtil32.HiWord(wl.lParam1));
                    //FrmDlg.m_Dice[3].nDicePoint = HUtil32.HiByte(HUtil32.HiWord(wl.lParam1));
                    //FrmDlg.m_Dice[4].nDicePoint = HUtil32.LoByte(HUtil32.LoWord(wl.lParam2));
                    //FrmDlg.m_Dice[5].nDicePoint = HUtil32.HiByte(HUtil32.LoWord(wl.lParam2));
                    //FrmDlg.m_Dice[6].nDicePoint = HUtil32.LoByte(HUtil32.HiWord(wl.lParam2));
                    //FrmDlg.m_Dice[7].nDicePoint = HUtil32.HiByte(HUtil32.HiWord(wl.lParam2));
                    //FrmDlg.m_Dice[8].nDicePoint = HUtil32.LoByte(HUtil32.LoWord(wl.lTag1));
                    //FrmDlg.m_Dice[9].nDicePoint = HUtil32.HiByte(HUtil32.LoWord(wl.lTag1));
                    //FrmDlg.DialogSize = 0;
                    SendMerchantDlgSelect(msg.Recog, data);
                    break;
                case Messages.SM_PASSWORDSTATUS:
                    ClientGetPasswordStatus(msg, body);
                    break;
                default:
                    break;
                    // if g_MySelf = nil then Exit;
                    // g_PlayScene.MemoLog.Lines.Add('Ident: ' + IntToStr(Msg.ident));
                    // g_PlayScene.MemoLog.Lines.Add('Recog: ' + IntToStr(Msg.Recog));
                    // g_PlayScene.MemoLog.Lines.Add('Param: ' + IntToStr(Msg.param));
                    // g_PlayScene.MemoLog.Lines.Add('Tag: ' + IntToStr(Msg.tag));
                    // g_PlayScene.MemoLog.Lines.Add('Series: ' + IntToStr(Msg.series));
            }
        }
    }
}