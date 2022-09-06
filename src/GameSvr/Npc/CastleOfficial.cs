using GameSvr.Castle;
using GameSvr.Monster.Monsters;
using GameSvr.Player;
using GameSvr.Script;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Npc
{
    /// <summary>
    /// 沙城NPC类
    /// 沙城管理人员如：沙城管理员 沙城老人
    /// </summary>
    public class CastleOfficial : Merchant
    {
        public CastleOfficial() : base()
        {

        }

        public override void Click(PlayObject PlayObject)
        {
            if (this.Castle == null)
            {
                PlayObject.SysMsg("NPC不属于城堡!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (this.Castle.IsMasterGuild(PlayObject.MyGuild) || PlayObject.Permission >= 3)
            {
                base.Click(PlayObject);
            }
        }

        protected override void GetVariableText(PlayObject PlayObject, ref string sMsg, string sVariable)
        {
            var sText = string.Empty;
            base.GetVariableText(PlayObject, ref sMsg, sVariable);
            if (this.Castle == null)
            {
                sMsg = "????";
                return;
            }
            switch (sVariable)
            {
                case "$CASTLEGOLD":
                    sText = this.Castle.m_nTotalGold.ToString();
                    sMsg = this.ReplaceVariableText(sMsg, "<$CASTLEGOLD>", sText);
                    break;
                case "$TODAYINCOME":
                    sText = this.Castle.m_nTodayIncome.ToString();
                    sMsg = this.ReplaceVariableText(sMsg, "<$TODAYINCOME>", sText);
                    break;
                case "$CASTLEDOORSTATE":
                    {
                        var castleDoor = (CastleDoor)this.Castle.m_MainDoor.BaseObject;
                        if (castleDoor.Death)
                        {
                            sText = "destroyed";
                        }
                        else if (castleDoor.m_boOpened)
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
                    sText = M2Share.Config.nRepairDoorPrice.ToString();
                    sMsg = this.ReplaceVariableText(sMsg, "<$REPAIRDOORGOLD>", sText);
                    break;
                case "$REPAIRWALLGOLD":
                    sText = M2Share.Config.nRepairWallPrice.ToString();
                    sMsg = this.ReplaceVariableText(sMsg, "<$REPAIRWALLGOLD>", sText);
                    break;
                case "$GUARDFEE":
                    sText = M2Share.Config.nHireGuardPrice.ToString();
                    sMsg = this.ReplaceVariableText(sMsg, "<$GUARDFEE>", sText);
                    break;
                case "$ARCHERFEE":
                    sText = M2Share.Config.nHireArcherPrice.ToString();
                    sMsg = this.ReplaceVariableText(sMsg, "<$ARCHERFEE>", sText);
                    break;
                case "$GUARDRULE":
                    sText = "无效";
                    sMsg = this.ReplaceVariableText(sMsg, "<$GUARDRULE>", sText);
                    break;
            }
        }

        public override void UserSelect(PlayObject PlayObject, string sData)
        {
            var sLabel = string.Empty;
            const string sExceptionMsg = "[Exception] TCastleManager::UserSelect... ";
            base.UserSelect(PlayObject, sData);
            try
            {
                if (this.Castle == null)
                {
                    PlayObject.SysMsg("NPC不属于城堡!!!", MsgColor.Red, MsgType.Hint);
                    return;
                }
                string s18;
                if (sData != "" && sData[0] == '@')
                {
                    var sMsg = HUtil32.GetValidStr3(sData, ref sLabel, new char[] { '\r' });
                    s18 = "";
                    PlayObject.ScriptLable = sData;
                    if (this.Castle.IsMasterGuild(PlayObject.MyGuild) && PlayObject.IsGuildMaster())
                    {
                        var boCanJmp = PlayObject.LableIsCanJmp(sLabel);
                        if (string.Compare(sLabel, ScriptConst.sSL_SENDMSG, StringComparison.OrdinalIgnoreCase) == 0)
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
                        if (string.Compare(sLabel, ScriptConst.sOFFLINEMSG, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (this.m_boOffLineMsg)
                            {
                                this.SetOffLineMsg(PlayObject, sMsg);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sSL_SENDMSG, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            SendCustemMsg(PlayObject, sMsg);
                            PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, s18);
                        }
                        else if (string.Compare(sLabel, ScriptConst.sCASTLENAME, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            sMsg = sMsg.Trim();
                            if (sMsg != "")
                            {
                                this.Castle.m_sName = sMsg;
                                this.Castle.Save();
                                this.Castle.m_MasterGuild.RefMemberName();
                                s18 = "城堡名称更改成功...";
                            }
                            else
                            {
                                s18 = "城堡名称更改失败!!!";
                            }
                            PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, s18);
                        }
                        else if (string.Compare(sLabel, ScriptConst.sWITHDRAWAL, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            switch (this.Castle.WithDrawalGolds(PlayObject, HUtil32.Str_ToInt(sMsg, 0)))
                            {
                                case -4:
                                    s18 = "输入的金币数不正确!!!";
                                    break;
                                case -3:
                                    s18 = "您无法携带更多的东西了。";
                                    break;
                                case -2:
                                    s18 = "该城内没有这么多金币.";
                                    break;
                                case -1:
                                    s18 = "只有行会 " + this.Castle.m_sOwnGuild + " 的掌门人才能使用!!!";
                                    break;
                                case 1:
                                    this.GotoLable(PlayObject, ScriptConst.sMAIN, false);
                                    break;
                            }
                            PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, s18);
                        }
                        else if (string.Compare(sLabel, ScriptConst.sRECEIPTS, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            switch (this.Castle.ReceiptGolds(PlayObject, HUtil32.Str_ToInt(sMsg, 0)))
                            {
                                case -4:
                                    s18 = "输入的金币数不正确!!!";
                                    break;
                                case -3:
                                    s18 = "你已经达到在城内存放货物的限制了。";
                                    break;
                                case -2:
                                    s18 = "你没有那么多金币.";
                                    break;
                                case -1:
                                    s18 = "只有行会 " + this.Castle.m_sOwnGuild + " 的掌门人才能使用!!!";
                                    break;
                                case 1:
                                    this.GotoLable(PlayObject, ScriptConst.sMAIN, false);
                                    break;
                            }
                            PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, s18);
                        }
                        else if (string.Compare(sLabel, ScriptConst.sOPENMAINDOOR, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            this.Castle.MainDoorControl(false);
                        }
                        else if (string.Compare(sLabel, ScriptConst.sCLOSEMAINDOOR, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            this.Castle.MainDoorControl(true);
                        }
                        else if (string.Compare(sLabel, ScriptConst.sREPAIRDOORNOW, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            RepairDoor(PlayObject);
                            this.GotoLable(PlayObject, ScriptConst.sMAIN, false);
                        }
                        else if (string.Compare(sLabel, ScriptConst.sREPAIRWALLNOW1, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            RepairWallNow(1, PlayObject);
                            this.GotoLable(PlayObject, ScriptConst.sMAIN, false);
                        }
                        else if (string.Compare(sLabel, ScriptConst.sREPAIRWALLNOW2, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            RepairWallNow(2, PlayObject);
                            this.GotoLable(PlayObject, ScriptConst.sMAIN, false);
                        }
                        else if (string.Compare(sLabel, ScriptConst.sREPAIRWALLNOW3, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            RepairWallNow(3, PlayObject);
                            this.GotoLable(PlayObject, ScriptConst.sMAIN, false);
                        }
                        else if (HUtil32.CompareLStr(sLabel, ScriptConst.sHIREGUARDNOW, ScriptConst.sHIREGUARDNOW.Length))
                        {
                            s20 = sLabel.Substring(ScriptConst.sHIREGUARDNOW.Length, sLabel.Length);
                            HireGuard(s20, PlayObject);
                            PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, "");
                        }
                        else if (HUtil32.CompareLStr(sLabel, ScriptConst.sHIREARCHERNOW, ScriptConst.sHIREARCHERNOW.Length))
                        {
                            s20 = sLabel.Substring(ScriptConst.sHIREARCHERNOW.Length, sLabel.Length);
                            HireArcher(s20, PlayObject);
                            PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, this.ObjectId, 0, 0, "");
                        }
                        else if (string.Compare(sLabel, ScriptConst.sEXIT, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            PlayObject.SendMsg(this, Grobal2.RM_MERCHANTDLGCLOSE, 0, this.ObjectId, 0, 0, "");
                        }
                        else if (string.Compare(sLabel, ScriptConst.sBACK, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (PlayObject.m_sScriptGoBackLable == "")
                            {
                                PlayObject.m_sScriptGoBackLable = ScriptConst.sMAIN;
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
                M2Share.Log.Error(sExceptionMsg);
            }
        }

        private void HireGuard(string sIndex, PlayObject PlayObject)
        {
            if (this.Castle == null)
            {
                PlayObject.SysMsg("NPC不属于城堡!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (this.Castle.m_nTotalGold >= M2Share.Config.nHireGuardPrice)
            {
                var n10 = HUtil32.Str_ToInt(sIndex, 0) - 1;
                if (n10 <= CastleConst.MaxCalsteGuard)
                {
                    if (this.Castle.m_Guard[n10].BaseObject == null)
                    {
                        if (!this.Castle.m_boUnderWar)
                        {
                            var ObjUnit = this.Castle.m_Guard[n10];
                            ObjUnit.BaseObject = M2Share.UserEngine.RegenMonsterByName(this.Castle.m_sMapName, ObjUnit.nX, ObjUnit.nY, ObjUnit.sName);
                            if (ObjUnit.BaseObject != null)
                            {
                                this.Castle.m_nTotalGold -= M2Share.Config.nHireGuardPrice;
                                ObjUnit.BaseObject.Castle = this.Castle;
                                ((GuardUnit)ObjUnit.BaseObject).Direction = 3;
                                PlayObject.SysMsg("雇佣成功.", MsgColor.Green, MsgType.Hint);
                            }
                        }
                        else
                        {
                            PlayObject.SysMsg("现在无法雇佣!!!", MsgColor.Red, MsgType.Hint);
                        }
                    }
                    else
                    {
                        PlayObject.SysMsg("早已雇佣!!!", MsgColor.Red, MsgType.Hint);
                    }
                }
                else
                {
                    PlayObject.SysMsg("指令错误!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                PlayObject.SysMsg("城内资金不足!!!", MsgColor.Red, MsgType.Hint);
            }
        }

        private void HireArcher(string sIndex, PlayObject PlayObject)
        {
            if (this.Castle == null)
            {
                PlayObject.SysMsg("NPC不属于城堡!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (this.Castle.m_nTotalGold >= M2Share.Config.nHireArcherPrice)
            {
                var n10 = HUtil32.Str_ToInt(sIndex, 0) - 1;
                if (n10 <= CastleConst.MaxCastleArcher)
                {
                    if (this.Castle.Archer[n10].BaseObject == null)
                    {
                        if (!this.Castle.m_boUnderWar)
                        {
                            var ObjUnit = this.Castle.Archer[n10];
                            ObjUnit.BaseObject = M2Share.UserEngine.RegenMonsterByName(this.Castle.m_sMapName, ObjUnit.nX, ObjUnit.nY, ObjUnit.sName);
                            if (ObjUnit.BaseObject != null)
                            {
                                this.Castle.m_nTotalGold -= M2Share.Config.nHireArcherPrice;
                                ObjUnit.BaseObject.Castle = this.Castle;
                                ((GuardUnit)ObjUnit.BaseObject).Direction = 3;
                                PlayObject.SysMsg("雇佣成功.", MsgColor.Green, MsgType.Hint);
                            }
                        }
                        else
                        {
                            PlayObject.SysMsg("现在无法雇佣!!!", MsgColor.Red, MsgType.Hint);
                        }
                    }
                    else
                    {
                        PlayObject.SysMsg("早已雇佣!!!", MsgColor.Red, MsgType.Hint);
                    }
                }
                else
                {
                    PlayObject.SysMsg("指令错误!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                PlayObject.SysMsg("城内资金不足!!!", MsgColor.Red, MsgType.Hint);
            }
        }

        private void RepairDoor(PlayObject PlayObject)
        {
            if (this.Castle == null)
            {
                PlayObject.SysMsg("NPC不属于城堡!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (this.Castle.m_nTotalGold >= M2Share.Config.nRepairDoorPrice)
            {
                if (this.Castle.RepairDoor())
                {
                    this.Castle.m_nTotalGold -= M2Share.Config.nRepairDoorPrice;
                    PlayObject.SysMsg("修理成功。", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    PlayObject.SysMsg("城门不需要修理!!!", MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                PlayObject.SysMsg("城内资金不足!!!", MsgColor.Red, MsgType.Hint);
            }
        }

        private void RepairWallNow(int nWallIndex, PlayObject PlayObject)
        {
            if (this.Castle == null)
            {
                PlayObject.SysMsg("NPC不属于城堡!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (this.Castle.m_nTotalGold >= M2Share.Config.nRepairWallPrice)
            {
                if (this.Castle.RepairWall(nWallIndex))
                {
                    this.Castle.m_nTotalGold -= M2Share.Config.nRepairWallPrice;
                    PlayObject.SysMsg("修理成功。", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    PlayObject.SysMsg("城门不需要修理!!!", MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                PlayObject.SysMsg("城内资金不足!!!", MsgColor.Red, MsgType.Hint);
            }
        }

        protected override void SendCustemMsg(PlayObject PlayObject, string sMsg)
        {
            if (!M2Share.Config.SubkMasterSendMsg)
            {
                PlayObject.SysMsg(M2Share.g_sSubkMasterMsgCanNotUseNowMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayObject.m_boSendMsgFlag)
            {
                PlayObject.m_boSendMsgFlag = false;
                M2Share.UserEngine.SendBroadCastMsg(PlayObject.CharName + ": " + sMsg, MsgType.Castle);
            }
        }
    }
}