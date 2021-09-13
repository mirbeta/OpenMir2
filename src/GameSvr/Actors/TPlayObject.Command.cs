using SystemModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SystemModule.Common;

namespace GameSvr
{
    public partial class TPlayObject
    {
        public void CmdChangeAdminMode(string sCmd, int nPermission, string sParam1, bool boFlag)
        {
            if (m_btPermission < nPermission)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam1 != "" && sParam1[1] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, sCmd, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            m_boAdminMode = boFlag;
            if (m_boAdminMode)
            {
                SysMsg(M2Share.sGameMasterMode, TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg(M2Share.sReleaseGameMasterMode, TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdChangeItemName(string sCmd, string sMakeIndex, string sItemIndex, string sItemName)
        {
            int nMakeIndex;
            int nItemIndex;
            if (m_btPermission < 6)
            {
                return;
            }
            if (sMakeIndex == "" || sItemIndex == "" || sItemName == "")
            {
                SysMsg("命令格式: @" + sCmd + " 物品编号 物品ID号 物品名称", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nMakeIndex = HUtil32.Str_ToInt(sMakeIndex, -1);
            nItemIndex = HUtil32.Str_ToInt(sItemIndex, -1);
            if (nMakeIndex <= 0 || nItemIndex < 0)
            {
                SysMsg("命令格式: @" + sCmd + " 物品编号 物品ID号 物品名称", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (M2Share.ItemUnit.AddCustomItemName(nMakeIndex, nItemIndex, sItemName))
            {
                M2Share.ItemUnit.SaveCustomItemName();
                SysMsg("物品名称设置成功。", TMsgColor.c_Green, TMsgType.t_Hint);
                return;
            }
            SysMsg("此物品，已经设置了其它的名称！！！", TMsgColor.c_Red, TMsgType.t_Hint);
        }

        public void CmdChangeObMode(string sCmd, int nPermission, string sParam1, bool boFlag)
        {
            if (m_btPermission < nPermission)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam1 != "" && sParam1[1] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, sCmd, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (boFlag)
            {
                SendRefMsg(Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, ""); // 01/21 强行发送刷新数据到客户端，解决GM登录隐身有影子问题
            }
            m_boObMode = boFlag;
            if (m_boObMode)
            {
                SysMsg(M2Share.sObserverMode, TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg(M2Share.g_sReleaseObserverMode, TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdChangeSabukLord(TGameCmd Cmd, string sCastleName, string sGuildName, bool boFlag)
        {
            TGuild Guild;
            TUserCastle Castle;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sCastleName == "" || sGuildName == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 城堡名称 行会名称", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Castle = M2Share.CastleManager.Find(sCastleName);
            if (Castle == null)
            {
                SysMsg(format(M2Share.g_sGameCommandSbkGoldCastleNotFoundMsg, sCastleName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Guild = M2Share.GuildManager.FindGuild(sGuildName);
            if (Guild != null)
            {
                M2Share.AddGameDataLog("27" + "\t" + Castle.m_sOwnGuild + "\t" + '0' + "\t" + '1' + "\t" + "sGuildName" + "\t" + m_sCharName + "\t" + '0' + "\t" + '1' + "\t" + '0');
                Castle.GetCastle(Guild);
                if (boFlag)
                {
                    M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_211, M2Share.nServerIndex, sGuildName);
                }
                SysMsg(Castle.m_sName + " 所属行会已经更改为 " + sGuildName, TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg("行会 " + sGuildName + "还没建立！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdChangeSuperManMode(string sCmd, int nPermission, string sParam1, bool boFlag)
        {
            if (m_btPermission < nPermission)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam1 != "" && sParam1[1] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, sCmd, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            m_boSuperMan = boFlag;
            if (m_boSuperMan)
            {
                SysMsg(M2Share.sSupermanMode, TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg(M2Share.sReleaseSupermanMode, TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdClearBagItem(TGameCmd Cmd, string sHumanName)
        {
            TPlayObject PlayObject;
            TUserItem UserItem;
            IList<TDeleteItem> DelList = null;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sHumanName != "" && sHumanName[0] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, "人物名称"), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {
                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            for (var i = 0; i < PlayObject.m_ItemList.Count; i++)
            {
                UserItem = PlayObject.m_ItemList[i];
                if (DelList == null)
                {
                    DelList = new List<TDeleteItem>();
                }
                DelList.Add(new TDeleteItem()
                {
                    sItemName = M2Share.UserEngine.GetStdItemName(UserItem.wIndex),
                    MakeIndex = UserItem.MakeIndex
                });
                Dispose(UserItem);
            }
            PlayObject.m_ItemList.Clear();
            if (DelList != null)
            {
                var ObjectId = HUtil32.Sequence();
                M2Share.ObjectSystem.AddOhter(ObjectId, DelList);
                PlayObject.SendMsg(PlayObject, Grobal2.RM_SENDDELITEMLIST, 0, ObjectId, 0, 0, "");
            }
        }

        public void CmdSearchDear(string sCmd, string sParam)
        {
            if (sParam != "" && sParam[1] == '?')
            {
                SysMsg("此命令用于查询配偶当前所在位置。", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (m_sDearName == "")
            {
                // '你都没结婚查什么？'
                SysMsg(M2Share.g_sYouAreNotMarryedMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (m_DearHuman == null)
            {
                if (m_btGender == ObjBase.gMan)
                {
                    // '你的老婆还没有上线！！！'
                    SysMsg(M2Share.g_sYourWifeNotOnlineMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                }
                else
                {
                    // '你的老公还没有上线！！！'
                    SysMsg(M2Share.g_sYourHusbandNotOnlineMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                }
                return;
            }
            if (m_btGender == ObjBase.gMan)
            {
                // '你的老婆现在位于:'
                SysMsg(M2Share.g_sYourWifeNowLocateMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                SysMsg(m_DearHuman.m_sCharName + ' ' + m_DearHuman.m_PEnvir.sMapDesc + '(' + m_DearHuman.m_nCurrX + ':' + m_DearHuman.m_nCurrY + ')', TMsgColor.c_Green, TMsgType.t_Hint);
                // '你的老公正在找你，他现在位于:'
                m_DearHuman.SysMsg(M2Share.g_sYourHusbandSearchLocateMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                m_DearHuman.SysMsg(m_sCharName + ' ' + m_PEnvir.sMapDesc + '(' + m_nCurrX + ':' + m_nCurrY + ')', TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                // '你的老公现在位于:'
                SysMsg(M2Share.g_sYourHusbandNowLocateMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                SysMsg(m_DearHuman.m_sCharName + ' ' + m_DearHuman.m_PEnvir.sMapDesc + '(' + m_DearHuman.m_nCurrX + ':' + m_DearHuman.m_nCurrY + ')', TMsgColor.c_Green, TMsgType.t_Hint);
                // '你的老婆正在找你，她现在位于:'
                m_DearHuman.SysMsg(M2Share.g_sYourWifeSearchLocateMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                m_DearHuman.SysMsg(m_sCharName + ' ' + m_PEnvir.sMapDesc + '(' + m_nCurrX + ':' + m_nCurrY + ')', TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdSearchMaster(string sCmd, string sParam)
        {
            int I;
            TPlayObject Human;
            if (sParam != "" && sParam[1] == '?')
            {
                SysMsg("此命令用于查询师徒当前所在位置。", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (m_sMasterName == "")
            {
                SysMsg(M2Share.g_sYouAreNotMasterMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (m_boMaster)
            {
                if (m_MasterList.Count <= 0)
                {
                    SysMsg(M2Share.g_sYourMasterListNotOnlineMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
                SysMsg(M2Share.g_sYourMasterListNowLocateMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                for (I = 0; I < m_MasterList.Count; I++)
                {
                    Human = m_MasterList[I];
                    SysMsg(Human.m_sCharName + ' ' + Human.m_PEnvir.sMapDesc + '(' + Human.m_nCurrX + ':' + Human.m_nCurrY + ')', TMsgColor.c_Green, TMsgType.t_Hint);
                    Human.SysMsg(M2Share.g_sYourMasterSearchLocateMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                    Human.SysMsg(m_sCharName + ' ' + m_PEnvir.sMapDesc + '(' + m_nCurrX + ':' + m_nCurrY + ')', TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            else
            {
                if (m_MasterHuman == null)
                {
                    SysMsg(M2Share.g_sYourMasterNotOnlineMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
                SysMsg(M2Share.g_sYourMasterNowLocateMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                SysMsg(m_MasterHuman.m_sCharName + ' ' + m_MasterHuman.m_PEnvir.sMapDesc + '(' + m_MasterHuman.m_nCurrX + ':' + m_MasterHuman.m_nCurrY + ')', TMsgColor.c_Green, TMsgType.t_Hint);
                m_MasterHuman.SysMsg(M2Share.g_sYourMasterListSearchLocateMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                m_MasterHuman.SysMsg(m_sCharName + ' ' + m_PEnvir.sMapDesc + '(' + m_nCurrX + ':' + m_nCurrY + ')', TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }
        
        public void CmdForcedWallconquestWar(TGameCmd Cmd, string sCastleName)
        {
            TUserCastle Castle;
            string s20;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sCastleName == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 城堡名称", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Castle = M2Share.CastleManager.Find(sCastleName);
            if (Castle != null)
            {
                Castle.m_boUnderWar = !Castle.m_boUnderWar;
                if (Castle.m_boUnderWar)
                {

                    Castle.m_dwStartCastleWarTick = HUtil32.GetTickCount();
                    Castle.StartWallconquestWar();
                    M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_212, M2Share.nServerIndex, "");
                    s20 = '[' + Castle.m_sName + "攻城战已经开始]";
                    M2Share.UserEngine.SendBroadCastMsg(s20, TMsgType.t_System);
                    M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_204, M2Share.nServerIndex, s20);
                    Castle.MainDoorControl(true);
                }
                else
                {
                    Castle.StopWallconquestWar();
                }
            }
            else
            {

                SysMsg(format(M2Share.g_sGameCommandSbkGoldCastleNotFoundMsg, sCastleName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdHunger(string sCmd, string sHumanName, int nHungerPoint)
        {
            TPlayObject PlayObject;
            if (m_btPermission < 6)
            {
                return;
            }
            if (sHumanName == "" || nHungerPoint < 0)
            {
                SysMsg("命令格式: @" + sCmd + " 人物名称 能量值", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject != null)
            {
                PlayObject.m_nHungerStatus = nHungerPoint;
                PlayObject.SendMsg(PlayObject, Grobal2.RM_MYSTATUS, 0, 0, 0, 0, "");
                PlayObject.RefMyStatus();
                SysMsg(sHumanName + " 的能量值已改变。", TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg(sHumanName + "没有在线！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdLockLogin(TGameCmd Cmd)
        {
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (!M2Share.g_Config.boLockHumanLogin)
            {
                SysMsg("本服务器还没有启用登录锁功能！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (m_boLockLogon && !m_boLockLogoned)
            {
                SysMsg("您还没有打开登录锁或还没有设置锁密码！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            m_boLockLogon = !m_boLockLogon;
            if (m_boLockLogon)
            {
                SysMsg("已开启登录锁", TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg("已关闭登录锁", TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }
        
        public void CmdPrvMsg(string sCmd, int nPermission, string sHumanName)
        {
            if (m_btPermission < nPermission)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, sCmd, M2Share.g_sGameCommandPrvMsgHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            //for (I = 0; I < this.m_BlockWhisperList.Count; I++)
            //{
            //    if ((this.m_BlockWhisperList[I]).ToLower().CompareTo((sHumanName), StringComparison.OrdinalIgnoreCase) == 0)
            //    {
            //        this.m_BlockWhisperList.Remove(I);
            //        this.SysMsg(format(M2Share.g_sGameCommandPrvMsgUnLimitMsg, new string[] { sHumanName }), TMsgColor.c_Green, TMsgType.t_Hint);
            //        return;
            //    }
            //}
            m_BlockWhisperList.Add(sHumanName);
            SysMsg(format(M2Share.g_sGameCommandPrvMsgLimitMsg, sHumanName), TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdReLoadAdmin(string sCmd)
        {
            if (m_btPermission < 6)
            {
                return;
            }
            M2Share.LocalDB.LoadAdminList();
            M2Share.UserEngine.SendServerGroupMsg(213, M2Share.nServerIndex, "");
            SysMsg("管理员列表重新加载成功...", TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdReloadManage(TGameCmd Cmd, string sParam)
        {
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam != "" && sParam[1] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam == "")
            {
                if (M2Share.g_ManageNPC != null)
                {
                    M2Share.g_ManageNPC.ClearScript();
                    M2Share.g_ManageNPC.LoadNPCScript();
                    SysMsg("重新加载登录脚本完成...", TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    SysMsg("重新加载登录脚本失败...", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            else
            {
                if (M2Share.g_FunctionNPC != null)
                {
                    M2Share.g_FunctionNPC.ClearScript();
                    M2Share.g_FunctionNPC.LoadNPCScript();
                    SysMsg("重新加载功能脚本完成...", TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    SysMsg("重新加载功能脚本失败...", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
        }

        public void CmdReloadRobot()
        {
            M2Share.RobotManage.ReLoadRobot();
            SysMsg("重新加载机器人配置完成...", TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdReloadRobotManage()
        {
            if (m_btPermission < 6)
            {
                return;
            }
            if (M2Share.g_RobotNPC != null)
            {
                M2Share.g_RobotNPC.ClearScript();
                M2Share.g_RobotNPC.LoadNPCScript();
                SysMsg("重新加载机器人专用脚本完成...", TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg("重新加载机器人专用脚本失败...", TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdReloadMonItems()
        {
            int I;
            TMonInfo Monster;
            if (m_btPermission < 6)
            {
                return;
            }
            try
            {
                for (I = 0; I < M2Share.UserEngine.MonsterList.Count; I++)
                {
                    Monster = M2Share.UserEngine.MonsterList[I];
                    M2Share.LocalDB.LoadMonitems(Monster.sName, ref Monster.ItemList);
                }
                SysMsg("怪物爆物品列表重加载完成...", TMsgColor.c_Green, TMsgType.t_Hint);
            }
            catch
            {
                SysMsg("怪物爆物品列表重加载失败！！！", TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdReloadNpc(string sParam)
        {
            IList<TBaseObject> TmpList;
            TMerchant Merchant;
            TNormNpc Npc;
            if (m_btPermission < 6)
            {
                return;
            }
            if (string.Compare("all", sParam, StringComparison.OrdinalIgnoreCase) == 0)
            {
                M2Share.LocalDB.ReLoadMerchants();
                M2Share.UserEngine.ReloadMerchantList();
                SysMsg("交易NPC重新加载完成！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                M2Share.UserEngine.ReloadNpcList();
                SysMsg("管理NPC重新加载完成！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            TmpList = new List<TBaseObject>();
            if (M2Share.UserEngine.GetMerchantList(m_PEnvir, m_nCurrX, m_nCurrY, 9, TmpList) > 0)
            {
                for (var i = 0; i < TmpList.Count; i++)
                {
                    Merchant = (TMerchant)TmpList[i];
                    Merchant.ClearScript();
                    Merchant.LoadNPCScript();
                    SysMsg(Merchant.m_sCharName + "重新加载成功...", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            else
            {
                SysMsg("附近未发现任何交易NPC！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
            TmpList.Clear();
            if (M2Share.UserEngine.GetNpcList(m_PEnvir, m_nCurrX, m_nCurrY, 9, TmpList) > 0)
            {
                for (var i = 0; i < TmpList.Count; i++)
                {
                    Npc = (TNormNpc)TmpList[i];
                    Npc.ClearScript();
                    Npc.LoadNPCScript();
                    SysMsg(Npc.m_sCharName + "重新加载成功...", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            else
            {
                SysMsg("附近未发现任何管理NPC！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
            //TmpList.Free;
        }

        public void CmdSearchHuman(string sCmd, string sHumanName)
        {
            TPlayObject PlayObject;
            if (m_boProbeNecklace || m_btPermission >= 6)
            {
                if (sHumanName == "")
                {
                    SysMsg("命令格式: @" + sCmd + " 人物名称", TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
                if (((HUtil32.GetTickCount() - m_dwProbeTick) > 10000) || m_btPermission >= 3)
                {
                    m_dwProbeTick = HUtil32.GetTickCount();
                    PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
                    if (PlayObject != null)
                    {
                        SysMsg(sHumanName + " 现在位于 " + PlayObject.m_PEnvir.sMapDesc + ' ' + PlayObject.m_nCurrX + ':' + PlayObject.m_nCurrY, TMsgColor.c_Blue, TMsgType.t_Hint);
                    }
                    else
                    {
                        SysMsg(sHumanName + " 现在不在线，或位于其它服务器上！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                }
                else
                {
                    SysMsg((HUtil32.GetTickCount() - m_dwProbeTick) / 1000 - 10 + " 秒之后才可以再使用此功能！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
            else
            {
                SysMsg("您现在还无法使用此功能！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdShutup(TGameCmd Cmd, string sHumanName, string sTime)
        {
            int dwTime;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sTime == "" || sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandShutupHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            dwTime = HUtil32.Str_ToInt(sTime, 5);
            //M2Share.g_DenySayMsgList.__Lock();
            //try
            //{
            //    nIndex = M2Share.g_DenySayMsgList.GetIndex(sHumanName);
            //    if (nIndex >= 0)
            //    {
            //        M2Share.g_DenySayMsgList[nIndex] = ((HUtil32.GetTickCount() + dwTime * 60 * 1000) as Object);
            //    }
            //    else
            //    {
            //        //M2Share.g_DenySayMsgList.AddRecord(sHumanName, HUtil32.GetTickCount() + dwTime * 60 * 1000);
            //    }
            //}
            //finally
            //{
            //    M2Share.g_DenySayMsgList.UnLock();
            //}
            SysMsg(format(M2Share.g_sGameCommandShutupHumanMsg, sHumanName, dwTime), TMsgColor.c_Red, TMsgType.t_Hint);
        }

        public void CmdSpirtStart(string sCmd, string sParam1)
        {
            int nTime;
            int dwTime;
            if (m_btPermission < 6)
            {
                return;
            }
            if (sParam1 != "" && sParam1[1] == '?')
            {
                SysMsg("此命令用于开始祈祷生效宝宝叛变。", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nTime = HUtil32.Str_ToInt(sParam1, -1);
            if (nTime > 0)
            {
                dwTime = nTime * 1000;
            }
            else
            {
                dwTime = M2Share.g_Config.dwSpiritMutinyTime;
            }

            M2Share.g_dwSpiritMutinyTick = HUtil32.GetTickCount() + dwTime;
            SysMsg("祈祷叛变已开始。持续时长 " + dwTime / 1000 + " 秒。", TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdSpirtStop(string sCmd, string sParam1)
        {
            if (m_btPermission < 6)
            {
                return;
            }
            if (sParam1 != "" && sParam1[1] == '?')
            {
                SysMsg("此命令用于停止祈祷生效导致宝宝叛变。", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            M2Share.g_dwSpiritMutinyTick = 0;
            SysMsg("祈祷叛变已停止。", TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdTakeOffHorse(string sCmd, string sParam)
        {
            if (sParam != "" && sParam[1] == '?')
            {
                SysMsg("下马命令，在骑马状态输入此命令下马。", TMsgColor.c_Red, TMsgType.t_Hint);

                SysMsg(format("命令格式: @%s", sCmd), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (!m_boOnHorse)
            {
                return;
            }
            m_boOnHorse = false;
            FeatureChanged();
        }

        public void CmdTakeOnHorse(string sCmd, string sParam)
        {
            if (sParam != "" && sParam[1] == '?')
            {
                SysMsg("上马命令，在戴好马牌后输入此命令就可以骑上马。", TMsgColor.c_Red, TMsgType.t_Hint);

                SysMsg(format("命令格式: @%s", sCmd), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (m_boOnHorse)
            {
                return;
            }
            if (m_btHorseType == 0)
            {
                SysMsg("骑马必须先戴上马牌！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            m_boOnHorse = true;
            FeatureChanged();
        }
    }
}
