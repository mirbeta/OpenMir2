using System;
using SystemModule;

namespace M2Server
{
    /// <summary>
    /// 沙城NPC类
    /// 沙城管理人员如：沙城管理员 沙城老人
    /// </summary>
    public class TCastleOfficial : TMerchant
    {
        public override void Click(TPlayObject PlayObject)
        {
            if (this.m_Castle == null)
            {
                PlayObject.SysMsg("NPC不属于城堡！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (this.m_Castle.IsMasterGuild(PlayObject.m_MyGuild) || PlayObject.m_btPermission >= 3)
            {
                base.Click(PlayObject);
            }
        }

        protected override void GetVariableText(TPlayObject PlayObject, ref string sMsg, string sVariable)
        {
            var sText = string.Empty;
            TCastleDoor CastleDoor;
            base.GetVariableText(PlayObject, ref sMsg, sVariable);
            if (this.m_Castle == null)
            {
                sMsg = "????";
                return;
            }
            switch (sVariable)
            {
                case "$CASTLEGOLD":
                    sText = this.m_Castle.m_nTotalGold.ToString();
                    sMsg = this.ReplaceVariableText(sMsg, "<$CASTLEGOLD>", sText);
                    break;
                case "$TODAYINCOME":
                    sText = this.m_Castle.m_nTodayIncome.ToString();
                    sMsg = this.ReplaceVariableText(sMsg, "<$TODAYINCOME>", sText);
                    break;
                case "$CASTLEDOORSTATE":
                {
                    CastleDoor = (TCastleDoor)this.m_Castle.m_MainDoor.BaseObject;
                    if (CastleDoor.m_boDeath)
                    {
                        sText = "destroyed";
                    }
                    else if (CastleDoor.m_boOpened)
                    {
                        sText = "opened";
                    }
                    else
                    {
                        sText = "closed";
                    }
                    sMsg = this.ReplaceVariableText(sMsg, "<$CASTLEDOORSTATE>", sText);
                    break;
                }
                case "$REPAIRDOORGOLD":
                    sText = M2Share.g_Config.nRepairDoorPrice.ToString();
                    sMsg = this.ReplaceVariableText(sMsg, "<$REPAIRDOORGOLD>", sText);
                    break;
                case "$REPAIRWALLGOLD":
                    sText = M2Share.g_Config.nRepairWallPrice.ToString();
                    sMsg = this.ReplaceVariableText(sMsg, "<$REPAIRWALLGOLD>", sText);
                    break;
                case "$GUARDFEE":
                    sText = M2Share.g_Config.nHireGuardPrice.ToString();
                    sMsg = this.ReplaceVariableText(sMsg, "<$GUARDFEE>", sText);
                    break;
                case "$ARCHERFEE":
                    sText = M2Share.g_Config.nHireArcherPrice.ToString();
                    sMsg = this.ReplaceVariableText(sMsg, "<$ARCHERFEE>", sText);
                    break;
                case "$GUARDRULE":
                    sText = "无效";
                    sMsg = this.ReplaceVariableText(sMsg, "<$GUARDRULE>", sText);
                    break;
            }
        }

        public override void UserSelect(TPlayObject PlayObject, string sData)
        {
            var sLabel = string.Empty;
            const string sExceptionMsg = "[Exception] TCastleManager::UserSelect... ";
            base.UserSelect(PlayObject, sData);
            try
            {
                if (this.m_Castle == null)
                {
                    PlayObject.SysMsg("NPC不属于城堡！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
                string s18;
                if (sData != "" && sData[0] == '@')
                {
                    var sMsg = HUtil32.GetValidStr3(sData, ref sLabel, new char[] { '\r' });
                    s18 = "";
                    PlayObject.m_sScriptLable = sData;
                    if (this.m_Castle.IsMasterGuild(PlayObject.m_MyGuild) && PlayObject.IsGuildMaster())
                    {
                        var boCanJmp = PlayObject.LableIsCanJmp(sLabel);
                        if (string.Compare(sLabel, M2Share.sSL_SENDMSG, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (sMsg == "")
                            {
                                return;
                            }
                        }
                        this.GotoLable(PlayObject, sLabel, !boCanJmp);
                        if (!boCanJmp)
                        {
                            return;
                        }
                        string s20;
                        // 增加挂机
                        if (string.Compare(sLabel, M2Share.sOFFLINEMSG, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (this.m_boOffLineMsg)
                            {
                                this.SetOffLineMsg(PlayObject, sMsg);
                            }
                        }
                        else if (string.Compare(sLabel, M2Share.sSL_SENDMSG, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            SendCustemMsg(PlayObject, sMsg);
                            PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, s18);
                        }
                        else if (string.Compare(sLabel, M2Share.sCASTLENAME, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            sMsg = sMsg.Trim();
                            if (sMsg != "")
                            {
                                this.m_Castle.m_sName = sMsg;
                                this.m_Castle.Save();
                                this.m_Castle.m_MasterGuild.RefMemberName();
                                s18 = "城堡名称更改成功...";
                            }
                            else
                            {
                                s18 = "城堡名称更改失败！！！";
                            }
                            PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, s18);
                        }
                        else if (string.Compare(sLabel, M2Share.sWITHDRAWAL, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            switch (this.m_Castle.WithDrawalGolds(PlayObject, HUtil32.Str_ToInt(sMsg, 0)))
                            {
                                case -4:
                                    s18 = "输入的金币数不正确！！！";
                                    break;
                                case -3:
                                    s18 = "您无法携带更多的东西了。";
                                    break;
                                case -2:
                                    s18 = "该城内没有这么多金币.";
                                    break;
                                case -1:
                                    s18 = "只有行会 " + this.m_Castle.m_sOwnGuild + " 的掌门人才能使用！！！";
                                    break;
                                case 1:
                                    this.GotoLable(PlayObject, M2Share.sMAIN, false);
                                    break;
                            }
                            PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, s18);
                        }
                        else if (string.Compare(sLabel, M2Share.sRECEIPTS, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            switch (this.m_Castle.ReceiptGolds(PlayObject, HUtil32.Str_ToInt(sMsg, 0)))
                            {
                                case -4:
                                    s18 = "输入的金币数不正确！！！";
                                    break;
                                case -3:
                                    s18 = "你已经达到在城内存放货物的限制了。";
                                    break;
                                case -2:
                                    s18 = "你没有那么多金币.";
                                    break;
                                case -1:
                                    s18 = "只有行会 " + this.m_Castle.m_sOwnGuild + " 的掌门人才能使用！！！";
                                    break;
                                case 1:
                                    this.GotoLable(PlayObject, M2Share.sMAIN, false);
                                    break;
                            }
                            PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, s18);
                        }
                        else if (string.Compare(sLabel, M2Share.sOPENMAINDOOR, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            this.m_Castle.MainDoorControl(false);
                        }
                        else if (string.Compare(sLabel, M2Share.sCLOSEMAINDOOR, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            this.m_Castle.MainDoorControl(true);
                        }
                        else if (string.Compare(sLabel, M2Share.sREPAIRDOORNOW, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            RepairDoor(PlayObject);
                            this.GotoLable(PlayObject, M2Share.sMAIN, false);
                        }
                        else if (string.Compare(sLabel, M2Share.sREPAIRWALLNOW1, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            RepairWallNow(1, PlayObject);
                            this.GotoLable(PlayObject, M2Share.sMAIN, false);
                        }
                        else if (string.Compare(sLabel, M2Share.sREPAIRWALLNOW2, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            RepairWallNow(2, PlayObject);
                            this.GotoLable(PlayObject, M2Share.sMAIN, false);
                        }
                        else if (string.Compare(sLabel, M2Share.sREPAIRWALLNOW3, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            RepairWallNow(3, PlayObject);
                            this.GotoLable(PlayObject, M2Share.sMAIN, false);
                        }
                        else if (HUtil32.CompareLStr(sLabel, M2Share.sHIREGUARDNOW, M2Share.sHIREGUARDNOW.Length))
                        {
                            s20 = sLabel.Substring(M2Share.sHIREGUARDNOW.Length, sLabel.Length);
                            HireGuard(s20, PlayObject);
                            PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, "");
                            // GotoLable(PlayObject,sHIREGUARDOK,False);
                        }
                        else if (HUtil32.CompareLStr(sLabel, M2Share.sHIREARCHERNOW, M2Share.sHIREARCHERNOW.Length))
                        {
                            s20 = sLabel.Substring(M2Share.sHIREARCHERNOW.Length, sLabel.Length);
                            HireArcher(s20, PlayObject);
                            PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, "");
                        }
                        else if (string.Compare(sLabel, M2Share.sEXIT, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            PlayObject.SendMsg(this, Grobal2.RM_MERCHANTDLGCLOSE, 0, this.ObjectId, 0, 0, "");
                        }
                        else if (string.Compare(sLabel, M2Share.sBACK, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (PlayObject.m_sScriptGoBackLable == "")
                            {
                                PlayObject.m_sScriptGoBackLable = M2Share.sMAIN;
                            }
                            this.GotoLable(PlayObject, PlayObject.m_sScriptGoBackLable, false);
                        }
                    }
                }
                else
                {
                    s18 = "你没有权利使用.";
                    PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, s18);
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
        }

        private void HireGuard(string sIndex, TPlayObject PlayObject)
        {
            int n10;
            TObjUnit ObjUnit;
            if (this.m_Castle == null)
            {
                PlayObject.SysMsg("NPC不属于城堡！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (this.m_Castle.m_nTotalGold >= M2Share.g_Config.nHireGuardPrice)
            {
                n10 = HUtil32.Str_ToInt(sIndex, 0) - 1;
                if (n10 <= Castle.MAXCALSTEGUARD)
                {
                    if (this.m_Castle.m_Guard[n10].BaseObject == null)
                    {
                        if (!this.m_Castle.m_boUnderWar)
                        {
                            ObjUnit = this.m_Castle.m_Guard[n10];
                            ObjUnit.BaseObject = M2Share.UserEngine.RegenMonsterByName(this.m_Castle.m_sMapName, ObjUnit.nX, ObjUnit.nY, ObjUnit.sName);
                            if (ObjUnit.BaseObject != null)
                            {
                                this.m_Castle.m_nTotalGold -= M2Share.g_Config.nHireGuardPrice;
                                ObjUnit.BaseObject.m_Castle = this.m_Castle;
                                ((TGuardUnit)ObjUnit.BaseObject).m_nX550 = ObjUnit.nX;
                                ((TGuardUnit)ObjUnit.BaseObject).m_nY554 = ObjUnit.nY;
                                ((TGuardUnit)ObjUnit.BaseObject).m_nDirection = 3;
                                PlayObject.SysMsg("雇佣成功.", TMsgColor.c_Green, TMsgType.t_Hint);
                            }
                        }
                        else
                        {
                            PlayObject.SysMsg("现在无法雇佣！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                    }
                    else
                    {
                        PlayObject.SysMsg("早已雇佣！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                }
                else
                {
                    PlayObject.SysMsg("指令错误！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
            else
            {
                PlayObject.SysMsg("城内资金不足！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        private void HireArcher(string sIndex, TPlayObject PlayObject)
        {
            if (this.m_Castle == null)
            {
                PlayObject.SysMsg("NPC不属于城堡！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (this.m_Castle.m_nTotalGold >= M2Share.g_Config.nHireArcherPrice)
            {
                var n10 = HUtil32.Str_ToInt(sIndex, 0) - 1;
                if (n10 <= Castle.MAXCASTLEARCHER)
                {
                    if (this.m_Castle.m_Archer[n10].BaseObject == null)
                    {
                        if (!this.m_Castle.m_boUnderWar)
                        {
                            var ObjUnit = this.m_Castle.m_Archer[n10];
                            ObjUnit.BaseObject = M2Share.UserEngine.RegenMonsterByName(this.m_Castle.m_sMapName, ObjUnit.nX, ObjUnit.nY, ObjUnit.sName);
                            if (ObjUnit.BaseObject != null)
                            {
                                this.m_Castle.m_nTotalGold -= M2Share.g_Config.nHireArcherPrice;
                                ObjUnit.BaseObject.m_Castle = this.m_Castle;
                                ((TGuardUnit)ObjUnit.BaseObject).m_nX550 = ObjUnit.nX;
                                ((TGuardUnit)ObjUnit.BaseObject).m_nY554 = ObjUnit.nY;
                                ((TGuardUnit)ObjUnit.BaseObject).m_nDirection = 3;
                                PlayObject.SysMsg("雇佣成功.", TMsgColor.c_Green, TMsgType.t_Hint);
                            }
                        }
                        else
                        {
                            PlayObject.SysMsg("现在无法雇佣！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                    }
                    else
                    {
                        PlayObject.SysMsg("早已雇佣！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                }
                else
                {
                    PlayObject.SysMsg("指令错误！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
            else
            {
                PlayObject.SysMsg("城内资金不足！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        private void RepairDoor(TPlayObject PlayObject)
        {
            if (this.m_Castle == null)
            {
                PlayObject.SysMsg("NPC不属于城堡！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (this.m_Castle.m_nTotalGold >= M2Share.g_Config.nRepairDoorPrice)
            {
                if (this.m_Castle.RepairDoor())
                {
                    this.m_Castle.m_nTotalGold -= M2Share.g_Config.nRepairDoorPrice;
                    PlayObject.SysMsg("修理成功。", TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    PlayObject.SysMsg("城门不需要修理！！！", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            else
            {
                PlayObject.SysMsg("城内资金不足！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        private void RepairWallNow(int nWallIndex, TPlayObject PlayObject)
        {
            if (this.m_Castle == null)
            {
                PlayObject.SysMsg("NPC不属于城堡！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (this.m_Castle.m_nTotalGold >= M2Share.g_Config.nRepairWallPrice)
            {
                if (this.m_Castle.RepairWall(nWallIndex))
                {
                    this.m_Castle.m_nTotalGold -= M2Share.g_Config.nRepairWallPrice;
                    PlayObject.SysMsg("修理成功。", TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    PlayObject.SysMsg("城门不需要修理！！！", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            else
            {
                PlayObject.SysMsg("城内资金不足！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public TCastleOfficial() : base()
        {
            
        }

        protected override void SendCustemMsg(TPlayObject PlayObject, string sMsg)
        {
            if (!M2Share.g_Config.boSubkMasterSendMsg)
            {
                PlayObject.SysMsg(M2Share.g_sSubkMasterMsgCanNotUseNowMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (PlayObject.m_boSendMsgFlag)
            {
                PlayObject.m_boSendMsgFlag = false;
                M2Share.UserEngine.SendBroadCastMsg(PlayObject.m_sCharName + ": " + sMsg, TMsgType.t_Castle);
            }
        }
    }
}

