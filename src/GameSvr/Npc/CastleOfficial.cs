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
            if (Castle == null)
            {
                PlayObject.SysMsg("NPC不属于城堡!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (Castle.IsMasterGuild(PlayObject.MyGuild) || PlayObject.Permission >= 3)
            {
                base.Click(PlayObject);
            }
        }

        protected override void GetVariableText(PlayObject PlayObject, string sVariable, ref string sMsg)
        {
            base.GetVariableText(PlayObject, sVariable, ref sMsg);
            if (Castle == null)
            {
                sMsg = "????";
                return;
            }
            string sText;
            switch (sVariable)
            {
                case "$CASTLEGOLD":
                    sText = Castle.TotalGold.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$CASTLEGOLD>", sText);
                    break;
                case "$TODAYINCOME":
                    sText = Castle.TodayIncome.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$TODAYINCOME>", sText);
                    break;
                case "$CASTLEDOORSTATE":
                    {
                        var castleDoor = (CastleDoor)Castle.MainDoor.BaseObject;
                        if (castleDoor.Death)
                        {
                            sText = "destroyed";
                        }
                        else if (castleDoor.IsOpened)
                        {
                            sText = "opened";
                        }
                        else
                        {
                            sText = "closed";
                        }
                        sMsg = ReplaceVariableText(sMsg, "<$CASTLEDOORSTATE>", sText);
                        break;
                    }
                case "$REPAIRDOORGOLD":
                    sText = M2Share.Config.RepairDoorPrice.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$REPAIRDOORGOLD>", sText);
                    break;
                case "$REPAIRWALLGOLD":
                    sText = M2Share.Config.RepairWallPrice.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$REPAIRWALLGOLD>", sText);
                    break;
                case "$GUARDFEE":
                    sText = M2Share.Config.HireGuardPrice.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$GUARDFEE>", sText);
                    break;
                case "$ARCHERFEE":
                    sText = M2Share.Config.HireArcherPrice.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$ARCHERFEE>", sText);
                    break;
                case "$GUARDRULE":
                    sText = "无效";
                    sMsg = ReplaceVariableText(sMsg, "<$GUARDRULE>", sText);
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
                if (Castle == null)
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
                    if (Castle.IsMasterGuild(PlayObject.MyGuild) && PlayObject.IsGuildMaster())
                    {
                        var boCanJmp = PlayObject.LableIsCanJmp(sLabel);
                        if (string.Compare(sLabel, ScriptConst.sSL_SENDMSG, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (sMsg == "")
                            {
                                return;
                            }
                        }
                        GotoLable(PlayObject, sLabel, !boCanJmp);
                        if (!boCanJmp)
                        {
                            return;
                        }
                        string s20;
                        if (string.Compare(sLabel, ScriptConst.sOFFLINEMSG, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (m_boOffLineMsg)
                            {
                                SetOffLineMsg(PlayObject, sMsg);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sSL_SENDMSG, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            SendCustemMsg(PlayObject, sMsg);
                            PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, s18);
                        }
                        else if (string.Compare(sLabel, ScriptConst.sCASTLENAME, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            sMsg = sMsg.Trim();
                            if (sMsg != "")
                            {
                                Castle.sName = sMsg;
                                Castle.Save();
                                Castle.MasterGuild.RefMemberName();
                                s18 = "城堡名称更改成功...";
                            }
                            else
                            {
                                s18 = "城堡名称更改失败!!!";
                            }
                            PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, s18);
                        }
                        else if (string.Compare(sLabel, ScriptConst.sWITHDRAWAL, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            switch (Castle.WithDrawalGolds(PlayObject, HUtil32.StrToInt(sMsg, 0)))
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
                                    s18 = "只有行会 " + Castle.OwnGuild + " 的掌门人才能使用!!!";
                                    break;
                                case 1:
                                    GotoLable(PlayObject, ScriptConst.sMAIN, false);
                                    break;
                            }
                            PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, s18);
                        }
                        else if (string.Compare(sLabel, ScriptConst.sRECEIPTS, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            switch (Castle.ReceiptGolds(PlayObject, HUtil32.StrToInt(sMsg, 0)))
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
                                    s18 = "只有行会 " + Castle.OwnGuild + " 的掌门人才能使用!!!";
                                    break;
                                case 1:
                                    GotoLable(PlayObject, ScriptConst.sMAIN, false);
                                    break;
                            }
                            PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, s18);
                        }
                        else if (string.Compare(sLabel, ScriptConst.sOPENMAINDOOR, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            Castle.MainDoorControl(false);
                        }
                        else if (string.Compare(sLabel, ScriptConst.sCLOSEMAINDOOR, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            Castle.MainDoorControl(true);
                        }
                        else if (string.Compare(sLabel, ScriptConst.sREPAIRDOORNOW, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            RepairDoor(PlayObject);
                            GotoLable(PlayObject, ScriptConst.sMAIN, false);
                        }
                        else if (string.Compare(sLabel, ScriptConst.sREPAIRWALLNOW1, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            RepairWallNow(1, PlayObject);
                            GotoLable(PlayObject, ScriptConst.sMAIN, false);
                        }
                        else if (string.Compare(sLabel, ScriptConst.sREPAIRWALLNOW2, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            RepairWallNow(2, PlayObject);
                            GotoLable(PlayObject, ScriptConst.sMAIN, false);
                        }
                        else if (string.Compare(sLabel, ScriptConst.sREPAIRWALLNOW3, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            RepairWallNow(3, PlayObject);
                            GotoLable(PlayObject, ScriptConst.sMAIN, false);
                        }
                        else if (HUtil32.CompareLStr(sLabel, ScriptConst.sHIREGUARDNOW))
                        {
                            s20 = sLabel.Substring(ScriptConst.sHIREGUARDNOW.Length, sLabel.Length);
                            HireGuard(s20, PlayObject);
                            PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, "");
                        }
                        else if (HUtil32.CompareLStr(sLabel, ScriptConst.sHIREARCHERNOW))
                        {
                            s20 = sLabel.Substring(ScriptConst.sHIREARCHERNOW.Length, sLabel.Length);
                            HireArcher(s20, PlayObject);
                            PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, "");
                        }
                        else if (string.Compare(sLabel, ScriptConst.sEXIT, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            PlayObject.SendMsg(this, Grobal2.RM_MERCHANTDLGCLOSE, 0, ActorId, 0, 0, "");
                        }
                        else if (string.Compare(sLabel, ScriptConst.sBACK, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (PlayObject.m_sScriptGoBackLable == "")
                            {
                                PlayObject.m_sScriptGoBackLable = ScriptConst.sMAIN;
                            }
                            GotoLable(PlayObject, PlayObject.m_sScriptGoBackLable, false);
                        }
                    }
                }
                else
                {
                    s18 = "你没有权利使用.";
                    PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, s18);
                }
            }
            catch
            {
                M2Share.Log.LogError(sExceptionMsg);
            }
        }

        private void HireGuard(string sIndex, PlayObject PlayObject)
        {
            if (Castle == null)
            {
                PlayObject.SysMsg("NPC不属于城堡!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (Castle.TotalGold >= M2Share.Config.HireGuardPrice)
            {
                var n10 = HUtil32.StrToInt(sIndex, 0) - 1;
                if (n10 <= CastleConst.MaxCalsteGuard)
                {
                    if (Castle.Guards[n10].BaseObject == null)
                    {
                        if (!Castle.UnderWar)
                        {
                            var ObjUnit = Castle.Guards[n10];
                            ObjUnit.BaseObject = M2Share.WorldEngine.RegenMonsterByName(Castle.MapName, ObjUnit.nX, ObjUnit.nY, ObjUnit.sName);
                            if (ObjUnit.BaseObject != null)
                            {
                                Castle.TotalGold -= M2Share.Config.HireGuardPrice;
                                ObjUnit.BaseObject.Castle = Castle;
                                ((GuardUnit)ObjUnit.BaseObject).GuardDirection = 3;
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
            if (Castle == null)
            {
                PlayObject.SysMsg("NPC不属于城堡!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (Castle.TotalGold >= M2Share.Config.HireArcherPrice)
            {
                var n10 = HUtil32.StrToInt(sIndex, 0) - 1;
                if (n10 <= CastleConst.MaxCastleArcher)
                {
                    if (Castle.Archers[n10].BaseObject == null)
                    {
                        if (!Castle.UnderWar)
                        {
                            var ObjUnit = Castle.Archers[n10];
                            ObjUnit.BaseObject = M2Share.WorldEngine.RegenMonsterByName(Castle.MapName, ObjUnit.nX, ObjUnit.nY, ObjUnit.sName);
                            if (ObjUnit.BaseObject != null)
                            {
                                Castle.TotalGold -= M2Share.Config.HireArcherPrice;
                                ObjUnit.BaseObject.Castle = Castle;
                                ((GuardUnit)ObjUnit.BaseObject).GuardDirection = 3;
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
            if (Castle == null)
            {
                PlayObject.SysMsg("NPC不属于城堡!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (Castle.TotalGold >= M2Share.Config.RepairDoorPrice)
            {
                if (Castle.RepairDoor())
                {
                    Castle.TotalGold -= M2Share.Config.RepairDoorPrice;
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
            if (Castle == null)
            {
                PlayObject.SysMsg("NPC不属于城堡!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (Castle.TotalGold >= M2Share.Config.RepairWallPrice)
            {
                if (Castle.RepairWall(nWallIndex))
                {
                    Castle.TotalGold -= M2Share.Config.RepairWallPrice;
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
                M2Share.WorldEngine.SendBroadCastMsg(PlayObject.ChrName + ": " + sMsg, MsgType.Castle);
            }
        }
    }
}