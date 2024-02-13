using M2Server.Castle;
using M2Server.Monster.Monsters;
using SystemModule.Actors;
using SystemModule.Const;
using SystemModule.Enums;

namespace GameSrv.Npc
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

        public override void Click(IPlayerActor PlayObject)
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

        protected override void GetVariableText(IPlayerActor PlayObject, string sVariable, ref string sMsg)
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
                        CastleDoor castleDoor = (CastleDoor)Castle.MainDoor.BaseObject;
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
                    sText = SystemShare.Config.RepairDoorPrice.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$REPAIRDOORGOLD>", sText);
                    break;
                case "$REPAIRWALLGOLD":
                    sText = SystemShare.Config.RepairWallPrice.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$REPAIRWALLGOLD>", sText);
                    break;
                case "$GUARDFEE":
                    sText = SystemShare.Config.HireGuardPrice.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$GUARDFEE>", sText);
                    break;
                case "$ARCHERFEE":
                    sText = SystemShare.Config.HireArcherPrice.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$ARCHERFEE>", sText);
                    break;
                case "$GUARDRULE":
                    sText = "无效";
                    sMsg = ReplaceVariableText(sMsg, "<$GUARDRULE>", sText);
                    break;
            }
        }

        public override void UserSelect(IPlayerActor PlayObject, string sData)
        {
            string sLabel = string.Empty;
            const string sExceptionMsg = "[Exception] TCastleManager::UserSelect... ";
            base.UserSelect(PlayObject, sData);
            try
            {
                if (Castle == null)
                {
                    PlayObject.SysMsg("NPC不属于城堡!!!", MsgColor.Red, MsgType.Hint);
                    return;
                }

                if (!string.IsNullOrEmpty(sData) && sData[0] == '@')
                {
                    string sMsg = HUtil32.GetValidStr3(sData, ref sLabel, '\r');
                    PlayObject.ScriptLable = sData;
                    if (Castle.IsMasterGuild(PlayObject.MyGuild) && PlayObject.IsGuildMaster())
                    {
                        bool boCanJmp = PlayObject.LableIsCanJmp(sLabel);
                        if (string.Compare(sLabel, ScriptFlagConst.sSL_SENDMSG, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (string.IsNullOrEmpty(sMsg))
                            {
                                return;
                            }
                        }
                        M2Share.ScriptEngine.GotoLable(PlayObject, this.ActorId, sLabel, !boCanJmp);
                        if (!boCanJmp)
                        {
                            return;
                        }
                        string s20;
                        string s18 = string.Empty;
                        if (string.Compare(sLabel, ScriptFlagConst.sOFFLINEMSG, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (IsOffLineMsg)
                            {
                                SetOffLineMsg(PlayObject, sMsg);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sSL_SENDMSG, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            SendCustemMsg(PlayObject, sMsg);
                            PlayObject.SendMsg(this, Messages.RM_MENU_OK, 0, ActorId, 0, 0, s18);
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sCASTLENAME, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            sMsg = sMsg.Trim();
                            if (!string.IsNullOrEmpty(sMsg))
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
                            PlayObject.SendMsg(this, Messages.RM_MENU_OK, 0, ActorId, 0, 0, s18);
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sWITHDRAWAL, StringComparison.OrdinalIgnoreCase) == 0)
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
                                    M2Share.ScriptEngine.GotoLable(PlayObject, this.ActorId, ScriptFlagConst.sMAIN);
                                    break;
                            }
                            PlayObject.SendMsg(this, Messages.RM_MENU_OK, 0, ActorId, 0, 0, s18);
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sRECEIPTS, StringComparison.OrdinalIgnoreCase) == 0)
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
                                    M2Share.ScriptEngine.GotoLable(PlayObject, this.ActorId, ScriptFlagConst.sMAIN);
                                    break;
                            }
                            PlayObject.SendMsg(this, Messages.RM_MENU_OK, 0, ActorId, 0, 0, s18);
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sOPENMAINDOOR, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            Castle.MainDoorControl(false);
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sCLOSEMAINDOOR, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            Castle.MainDoorControl(true);
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sREPAIRDOORNOW, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            RepairDoor(PlayObject);
                            M2Share.ScriptEngine.GotoLable(PlayObject, this.ActorId, ScriptFlagConst.sMAIN);
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sREPAIRWALLNOW1, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            RepairWallNow(1, PlayObject);
                            M2Share.ScriptEngine.GotoLable(PlayObject, this.ActorId, ScriptFlagConst.sMAIN);
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sREPAIRWALLNOW2, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            RepairWallNow(2, PlayObject);
                            M2Share.ScriptEngine.GotoLable(PlayObject, this.ActorId, ScriptFlagConst.sMAIN);
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sREPAIRWALLNOW3, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            RepairWallNow(3, PlayObject);
                            M2Share.ScriptEngine.GotoLable(PlayObject, this.ActorId, ScriptFlagConst.sMAIN);
                        }
                        else if (HUtil32.CompareLStr(sLabel, ScriptFlagConst.sHIREGUARDNOW))
                        {
                            s20 = sLabel.Substring(ScriptFlagConst.sHIREGUARDNOW.Length, sLabel.Length);
                            HireGuard(s20, PlayObject);
                            PlayObject.SendMsg(this, Messages.RM_MENU_OK, 0, ActorId, 0, 0);
                        }
                        else if (HUtil32.CompareLStr(sLabel, ScriptFlagConst.sHIREARCHERNOW))
                        {
                            s20 = sLabel.Substring(ScriptFlagConst.sHIREARCHERNOW.Length, sLabel.Length);
                            HireArcher(s20, PlayObject);
                            PlayObject.SendMsg(this, Messages.RM_MENU_OK, 0, ActorId, 0, 0);
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sEXIT, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            PlayObject.SendMsg(this, Messages.RM_MERCHANTDLGCLOSE, 0, ActorId, 0, 0);
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sBACK, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (string.IsNullOrEmpty(PlayObject.ScriptGoBackLable))
                            {
                                PlayObject.ScriptGoBackLable = ScriptFlagConst.sMAIN;
                            }
                            M2Share.ScriptEngine.GotoLable(PlayObject, this.ActorId, PlayObject.ScriptGoBackLable);
                        }
                    }
                }
                else
                {
                    PlayObject.SendMsg(this, Messages.RM_MENU_OK, 0, ActorId, 0, 0, "你没有权利使用.");
                }
            }
            catch
            {
                LogService.Error(sExceptionMsg);
            }
        }

        private void HireGuard(string sIndex, IPlayerActor PlayObject)
        {
            if (Castle == null)
            {
                PlayObject.SysMsg("NPC不属于城堡!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (Castle.TotalGold >= SystemShare.Config.HireGuardPrice)
            {
                int n10 = HUtil32.StrToInt(sIndex, 0) - 1;
                if (n10 <= CastleConst.MaxCalsteGuard)
                {
                    if (Castle.Guards[n10].BaseObject == null)
                    {
                        if (!Castle.UnderWar)
                        {
                            ArcherUnit ObjUnit = Castle.Guards[n10];
                            ObjUnit.BaseObject = SystemShare.WorldEngine.RegenMonsterByName(Castle.MapName, ObjUnit.nX, ObjUnit.nY, ObjUnit.sName);
                            if (ObjUnit.BaseObject != null)
                            {
                                Castle.TotalGold -= SystemShare.Config.HireGuardPrice;
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

        private void HireArcher(string sIndex, IPlayerActor PlayObject)
        {
            if (Castle == null)
            {
                PlayObject.SysMsg("NPC不属于城堡!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (Castle.TotalGold >= SystemShare.Config.HireArcherPrice)
            {
                int n10 = HUtil32.StrToInt(sIndex, 0) - 1;
                if (n10 <= CastleConst.MaxCastleArcher)
                {
                    if (Castle.Archers[n10].BaseObject == null)
                    {
                        if (!Castle.UnderWar)
                        {
                            ArcherUnit ObjUnit = Castle.Archers[n10];
                            ObjUnit.BaseObject = SystemShare.WorldEngine.RegenMonsterByName(Castle.MapName, ObjUnit.nX, ObjUnit.nY, ObjUnit.sName);
                            if (ObjUnit.BaseObject != null)
                            {
                                Castle.TotalGold -= SystemShare.Config.HireArcherPrice;
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

        private void RepairDoor(IPlayerActor PlayObject)
        {
            if (Castle == null)
            {
                PlayObject.SysMsg("NPC不属于城堡!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (Castle.TotalGold >= SystemShare.Config.RepairDoorPrice)
            {
                if (Castle.RepairDoor())
                {
                    Castle.TotalGold -= SystemShare.Config.RepairDoorPrice;
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

        private void RepairWallNow(int nWallIndex, IPlayerActor PlayObject)
        {
            if (Castle == null)
            {
                PlayObject.SysMsg("NPC不属于城堡!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (Castle.TotalGold >= SystemShare.Config.RepairWallPrice)
            {
                if (Castle.RepairWall(nWallIndex))
                {
                    Castle.TotalGold -= SystemShare.Config.RepairWallPrice;
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

        protected override void SendCustemMsg(IPlayerActor PlayObject, string sMsg)
        {
            if (!SystemShare.Config.SubkMasterSendMsg)
            {
                PlayObject.SysMsg(MessageSettings.SubkMasterMsgCanNotUseNowMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayObject.BoSendMsgFlag)
            {
                PlayObject.BoSendMsgFlag = false;
                SystemShare.WorldEngine.SendBroadCastMsg(PlayObject.ChrName + ": " + sMsg, MsgType.Castle);
            }
        }
    }
}