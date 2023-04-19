using GameSrv.Actor;
using GameSrv.Event.Events;
using GameSrv.GameCommand;
using GameSrv.Items;
using GameSrv.Maps;
using GameSrv.Npc;
using GameSrv.Player;
using GameSrv.Script;
using GameSrv.Services;
using GameSrv.World;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.ScriptSystem
{
    /// <summary>
    /// 脚本命令执行处理模块
    /// </summary>
    internal class ExecutionProcessingSys : ProcessingBase
    {
        /// <summary>
        /// 全局变量消息处理列表
        /// </summary>
        private static Dictionary<int, HandleExecutionMessage> ProcessExecutionMessage;
        private delegate void HandleExecutionMessage(NormNpc normNpc, PlayObject PlayObject, QuestActionInfo QuestActionInfo, ref bool Success);

        public void Initialize()
        {
            ProcessExecutionMessage = new Dictionary<int, HandleExecutionMessage>();
            ProcessExecutionMessage[(int)ExecutionCode.Set] = ActionOfSet;
            ProcessExecutionMessage[(int)ExecutionCode.ReSet] = ActionOfReSet;
            ProcessExecutionMessage[(int)ExecutionCode.SetOpen] = ActionOfSetOpen;
            ProcessExecutionMessage[(int)ExecutionCode.SetUnit] = ActionOfSetUnit;
            ProcessExecutionMessage[(int)ExecutionCode.ResetUnit] = ActionOfResetUnit;
            ProcessExecutionMessage[(int)ExecutionCode.Give] = ActionOfGiveItem;
            ProcessExecutionMessage[(int)ExecutionCode.ThrowItem] = ActionOfThrowitem;
            ProcessExecutionMessage[(int)ExecutionCode.RandomMove] = ActionOfRandomMove;
            ProcessExecutionMessage[(int)ExecutionCode.ReCallMob] = ActionOfRecallmob;
            ProcessExecutionMessage[(int)ExecutionCode.Hairstyle] = ActionOfChangeHairStyle;
            ProcessExecutionMessage[(int)ExecutionCode.ReCallgroupMembers] = ActionOfRecallGroupMembers;
            ProcessExecutionMessage[(int)ExecutionCode.ClearNameList] = ActionOfClearList;
            ProcessExecutionMessage[(int)ExecutionCode.Mapting] = ActionOfMapTing;
            ProcessExecutionMessage[(int)ExecutionCode.ChangeLevel] = ActionOfChangeLevel;
            ProcessExecutionMessage[(int)ExecutionCode.Marry] = ActionOfMarry;
            ProcessExecutionMessage[(int)ExecutionCode.Master] = ActionOfMaster;
            ProcessExecutionMessage[(int)ExecutionCode.UnMaster] = ActionOfUnMaster;
            ProcessExecutionMessage[(int)ExecutionCode.UnMarry] = ActionOfUnMarry;
            ProcessExecutionMessage[(int)ExecutionCode.GetMarry] = ActionOfGetMarry;
            ProcessExecutionMessage[(int)ExecutionCode.GetMaster] = ActionOfGetMaster;
            ProcessExecutionMessage[(int)ExecutionCode.ClearSkill] = ActionOfClearSkill;
            ProcessExecutionMessage[(int)ExecutionCode.DelnoJobSkill] = ActionOfDelNoJobSkill;
            ProcessExecutionMessage[(int)ExecutionCode.DelSkill] = ActionOfDelSkill;
            ProcessExecutionMessage[(int)ExecutionCode.AddSkill] = ActionOfAddSkill;
            ProcessExecutionMessage[(int)ExecutionCode.SkillLevel] = ActionOfSkillLevel;
            ProcessExecutionMessage[(int)ExecutionCode.ChangePkPoint] = ActionOfChangePkPoint;
            ProcessExecutionMessage[(int)ExecutionCode.ChangeExp] = ActionOfChangeExp;
            ProcessExecutionMessage[(int)ExecutionCode.ChangeJob] = ActionOfChangeJob;
            ProcessExecutionMessage[(int)ExecutionCode.Mission] = ActionOfMission;
            ProcessExecutionMessage[(int)ExecutionCode.MobPlace] = ActionOfMobPlace;
            ProcessExecutionMessage[(int)ExecutionCode.SetMemberType] = ActionOfSetMemberType;
            ProcessExecutionMessage[(int)ExecutionCode.SetMemberLevel] = ActionOfSetMemberLevel;
            ProcessExecutionMessage[(int)ExecutionCode.GameGold] = ActionOfGameGold;
            ProcessExecutionMessage[(int)ExecutionCode.GamePoint] = ActionOfGamePoint;
            ProcessExecutionMessage[(int)ExecutionCode.OffLine] = ActionOfOffLine;
            ProcessExecutionMessage[(int)ExecutionCode.AutoAddGameGold] = ActionOfAutoAddGameGold;
            ProcessExecutionMessage[(int)ExecutionCode.AutoSubGameGold] = ActionOfAutoSubGameGold;
            ProcessExecutionMessage[(int)ExecutionCode.ChangeNameColor] = ActionOfChangeNameColor;
            ProcessExecutionMessage[(int)ExecutionCode.ClearPassword] = ActionOfClearPassword;
            ProcessExecutionMessage[(int)ExecutionCode.Renewlevel] = ActionOfReNewLevel;
            ProcessExecutionMessage[(int)ExecutionCode.KillSlave] = ActionOfKillSlave;
            ProcessExecutionMessage[(int)ExecutionCode.ChangeGender] = ActionOfChangeGender;
            ProcessExecutionMessage[(int)ExecutionCode.KillMonExpRate] = ActionOfKillMonExpRate;
            ProcessExecutionMessage[(int)ExecutionCode.PowerRate] = ActionOfPowerRate;
            ProcessExecutionMessage[(int)ExecutionCode.ChangePerMission] = ActionOfChangePerMission;
            ProcessExecutionMessage[(int)ExecutionCode.BonusPoint] = ActionOfBonusPoint;
            ProcessExecutionMessage[(int)ExecutionCode.Restrenewlevel] = ActionOfRestReNewLevel;
            ProcessExecutionMessage[(int)ExecutionCode.DelMarry] = ActionOfDelMarry;
            ProcessExecutionMessage[(int)ExecutionCode.DelMaster] = ActionOfDelMaster;
            ProcessExecutionMessage[(int)ExecutionCode.CreditPoint] = ActionOfChangeCreditPoint;
            ProcessExecutionMessage[(int)ExecutionCode.ClearNeedItems] = ActionOfClearNeedItems;
            ProcessExecutionMessage[(int)ExecutionCode.ClearMakeItems] = ActionOfClearMakeItems;
            ProcessExecutionMessage[(int)ExecutionCode.SetSendMsgFlag] = ActionOfSetSendMsgFlag;
            ProcessExecutionMessage[(int)ExecutionCode.UpgradeItems] = ActionOfUpgradeItems;
            ProcessExecutionMessage[(int)ExecutionCode.UpgradeItemSex] = ActionOfUpgradeItemsEx;
            ProcessExecutionMessage[(int)ExecutionCode.MonGenex] = ActionOfMonGenEx;
            ProcessExecutionMessage[(int)ExecutionCode.ClearMapMon] = ActionOfClearMapMon;
            ProcessExecutionMessage[(int)ExecutionCode.SetMapMode] = ActionOfSetMapMode;
            ProcessExecutionMessage[(int)ExecutionCode.PvpZone] = ActionOfPkZone;
            ProcessExecutionMessage[(int)ExecutionCode.RestBonusPoint] = ActionOfRestBonusPoint;
            ProcessExecutionMessage[(int)ExecutionCode.TakeCastleGold] = ActionOfTakeCastleGold;
            ProcessExecutionMessage[(int)ExecutionCode.HumanHp] = ActionOfHumanHp;
            ProcessExecutionMessage[(int)ExecutionCode.HumanMp] = ActionOfHumanMp;
            ProcessExecutionMessage[(int)ExecutionCode.BuildPoint] = ActionOfGuildBuildPoint;
            ProcessExecutionMessage[(int)ExecutionCode.AuraePoint] = ActionOfGuildAuraePoint;
            ProcessExecutionMessage[(int)ExecutionCode.StabilityPoint] = ActionOfGuildstabilityPoint;
            ProcessExecutionMessage[(int)ExecutionCode.FlourishPoint] = ActionOfGuildFlourishPoint;
            ProcessExecutionMessage[(int)ExecutionCode.OpenMagicbox] = ActionOfOpenMagicBox;
            ProcessExecutionMessage[(int)ExecutionCode.SetRankLevelName] = ActionOfSetRankLevelName;
            ProcessExecutionMessage[(int)ExecutionCode.GmExecute] = ActionOfGmExecute;
            ProcessExecutionMessage[(int)ExecutionCode.GuildChiefItemCount] = ActionOfGuildChiefItemCount;
            ProcessExecutionMessage[(int)ExecutionCode.AddNameDateList] = ActionOfAddNameDateList;
            ProcessExecutionMessage[(int)ExecutionCode.DelNameDateList] = ActionOfDelNameDateList;
            ProcessExecutionMessage[(int)ExecutionCode.MobFireburn] = ActionOfMobFireBurn;
            ProcessExecutionMessage[(int)ExecutionCode.MessageBox] = ActionOfMessageBox;
            ProcessExecutionMessage[(int)ExecutionCode.SetscriptFlag] = ActionOfSetScriptFlag;
            ProcessExecutionMessage[(int)ExecutionCode.SetautogetExp] = ActionOfAutoGetExp;
            ProcessExecutionMessage[(int)ExecutionCode.Var] = ActionOfVar;
            ProcessExecutionMessage[(int)ExecutionCode.LoadVar] = ActionOfLoadVar;
            ProcessExecutionMessage[(int)ExecutionCode.SaveVar] = ActionOfSaveVar;
            ProcessExecutionMessage[(int)ExecutionCode.CalcVar] = ActionOfCalcVar;
            ProcessExecutionMessage[(int)ExecutionCode.GuildReCall] = ActionOfGuildRecall;
            ProcessExecutionMessage[(int)ExecutionCode.GroupAddList] = ActionOfGroupAddList;
            ProcessExecutionMessage[(int)ExecutionCode.ClearList] = ActionOfClearList;
            ProcessExecutionMessage[(int)ExecutionCode.GroupReCall] = ActionOfGroupRecall;
            ProcessExecutionMessage[(int)ExecutionCode.GroupMoveMap] = ActionOfGroupMoveMap;
            ProcessExecutionMessage[(int)ExecutionCode.RepairAll] = ActionOfRepairAllItem;
            ProcessExecutionMessage[(int)ExecutionCode.ChangeMode] = ActionOfChangeMode;
            ProcessExecutionMessage[(int)ExecutionCode.Kill] = ActionOfKill;
            ProcessExecutionMessage[(int)ExecutionCode.Kick] = ActionOfKick;
            ProcessExecutionMessage[(int)ExecutionCode.DelayGoto] = ActionOfDelayCall;
            ProcessExecutionMessage[(int)ExecutionCode.OpenYbDeal] = ActionOfOpenSaleDeal;
            ProcessExecutionMessage[(int)ExecutionCode.QueryYbSell] = ActionOfQuerySaleSell;
            ProcessExecutionMessage[(int)ExecutionCode.QueryYbDeal] = ActionOfQueryTrustDeal;
            ProcessExecutionMessage[(int)ExecutionCode.LineMsg] = ActionOfLineMsg;
            ProcessExecutionMessage[(int)ExecutionCode.SendMsg] = ActionOfLineMsg;
            ProcessExecutionMessage[(int)ExecutionCode.QueryBagItems] = ActionOfQueryBagItems;
            ProcessExecutionMessage[(int)ExecutionCode.QueryValue] = ActionOfQueryValue;
            ProcessExecutionMessage[(int)ExecutionCode.KillSlaveName] = ActionOfKillSlaveName;
            ProcessExecutionMessage[(int)ExecutionCode.QueryItemDlg] = ActionOfQueryItemDlg;
            ProcessExecutionMessage[(int)ExecutionCode.UpgradeDlgItem] = ActionOfUpgradeDlgItem;
            ProcessExecutionMessage[(int)ExecutionCode.ExchangeMap] = ActionOfExchangeMap;
            ProcessExecutionMessage[(int)ExecutionCode.ReCallMap] = ActionOfReCallMap;
            ProcessExecutionMessage[(int)ExecutionCode.Close] = ActionOfClose;
            ProcessExecutionMessage[(int)ExecutionCode.MapMove] = ActionOfMapMove;
            ProcessExecutionMessage[(int)ExecutionCode.Map] = ActionOfMap;
            ProcessExecutionMessage[(int)ExecutionCode.MonClear] = ActionOfMonClear;
            ProcessExecutionMessage[(int)ExecutionCode.MonGen] = ActionOfMonGen;
            ProcessExecutionMessage[(int)ExecutionCode.TimereCall] = ActionOfTimereCall;
            ProcessExecutionMessage[(int)ExecutionCode.BreakTimereCall] = ActionOfBreakTimereCall;
            ProcessExecutionMessage[(int)ExecutionCode.PkPoint] = ActionOfPkPoint;
            ProcessExecutionMessage[(int)ExecutionCode.AddNameList] = ActionOfAddNameList;
            ProcessExecutionMessage[(int)ExecutionCode.DelNameList] = ActionOfDelNameList;
            ProcessExecutionMessage[(int)ExecutionCode.AddUserDate] = ActionOfAddUseDateList;
            ProcessExecutionMessage[(int)ExecutionCode.DelUserDate] = ActionOfDelUseDateList;
            ProcessExecutionMessage[(int)ExecutionCode.AddGuildList] = ActionOfAddGuildList;
            ProcessExecutionMessage[(int)ExecutionCode.DelGuildList] = ActionOfDelGuildList;
            ProcessExecutionMessage[(int)ExecutionCode.AddAccountList] = ActionOfAddAccountList;
            ProcessExecutionMessage[(int)ExecutionCode.DelAccountList] = ActionOfDelAccountList;
            ProcessExecutionMessage[(int)ExecutionCode.AddIpList] = ActionOfAddIpList;
            ProcessExecutionMessage[(int)ExecutionCode.DelIpList] = ActionOfDelIpList;
            ProcessExecutionMessage[(int)ExecutionCode.ClearDelayGoto] = ActionOfClearDelayGoto;
            ProcessExecutionMessage[(int)ExecutionCode.SetRandomNo] = ActionOfSetRandomNo;
            ProcessExecutionMessage[(int)ExecutionCode.PlayDice] = ActionOfPlayDice;
            ProcessExecutionMessage[(int)ExecutionCode.Exeaction] = ActionOfExeaction;
        }

        public bool IsRegister(int cmdCode)
        {
            return ProcessExecutionMessage.ContainsKey(cmdCode);
        }

        public void Execute(NormNpc normNpc, PlayObject playObject, QuestActionInfo questConditionInfo, ref bool success)
        {
            if (ProcessExecutionMessage.ContainsKey(questConditionInfo.nCmdCode))
            {
                ProcessExecutionMessage[questConditionInfo.nCmdCode](normNpc, playObject, questConditionInfo, ref success);
            }
        }
        private void ActionOfExeaction(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var n40 = questActionInfo.nParam1;
            normNpc.ExeAction(playObject, questActionInfo.sParam1, questActionInfo.sParam2, questActionInfo.sParam3, questActionInfo.nParam1, questActionInfo.nParam2, questActionInfo.nParam3);
        }

        private void ActionOfPlayDice(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            playObject.PlayDiceLabel = questActionInfo.sParam2;
            playObject.SendMsg(normNpc, Messages.RM_PLAYDICE, (short)questActionInfo.nParam1, HUtil32.MakeLong(HUtil32.MakeWord((ushort)playObject.MDyVal[0], (ushort)playObject.MDyVal[1]), HUtil32.MakeWord((ushort)playObject.MDyVal[2], (ushort)playObject.MDyVal[3])), HUtil32.MakeLong(HUtil32.MakeWord((ushort)playObject.MDyVal[4], (ushort)playObject.MDyVal[5]), HUtil32.MakeWord((ushort)playObject.MDyVal[6], (ushort)playObject.MDyVal[7])), HUtil32.MakeLong(HUtil32.MakeWord((ushort)playObject.MDyVal[8], (ushort)playObject.MDyVal[9]), 0), questActionInfo.sParam2);
        }

        private void ActionOfSet(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var n28 = HUtil32.StrToInt(questActionInfo.sParam1, 0);
            var n2C = HUtil32.StrToInt(questActionInfo.sParam2, 0);
            playObject.SetQuestFlagStatus(n28, n2C);
        }

        private void ActionOfReSet(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            for (int k = 0; k < questActionInfo.nParam2; k++)
            {
                playObject.SetQuestFlagStatus(questActionInfo.nParam1 + k, 0);
            }
        }

        private void ActionOfSetOpen(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
           var n28 = HUtil32.StrToInt(questActionInfo.sParam1, 0);
            var n2C = HUtil32.StrToInt(questActionInfo.sParam2, 0);
            playObject.SetQuestUnitOpenStatus(n28, n2C);
        }

        private void ActionOfSetUnit(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var n28 = HUtil32.StrToInt(questActionInfo.sParam1, 0);
            var n2C = HUtil32.StrToInt(questActionInfo.sParam2, 0);
            playObject.SetQuestUnitStatus(n28, n2C);
        }

        private void ActionOfResetUnit(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            for (int k = 0; k < questActionInfo.nParam2; k++)
            {
                playObject.SetQuestUnitStatus(questActionInfo.nParam1 + k, 0);
            }
        }

        private void ActionOfPkPoint(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            if (questActionInfo.nParam1 == 0)
            {
                playObject.PkPoint = 0;
            }
            else
            {
                if (questActionInfo.nParam1 < 0)
                {
                    if ((playObject.PkPoint + questActionInfo.nParam1) >= 0)
                    {
                        playObject.PkPoint += questActionInfo.nParam1;
                    }
                    else
                    {
                        playObject.PkPoint = 0;
                    }
                }
                else
                {
                    if ((playObject.PkPoint + questActionInfo.nParam1) > 10000)
                    {
                        playObject.PkPoint = 10000;
                    }
                    else
                    {
                        playObject.PkPoint += questActionInfo.nParam1;
                    }
                }
            }
            playObject.RefNameColor();
        }

        private void ActionOfBreakTimereCall(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            playObject.IsTimeRecall = false;
        }

        private void ActionOfSetRandomNo(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            while (true)
            {
                var n2C = M2Share.RandomNumber.Random(999999);
                if ((n2C >= 1000) && (n2C.ToString() != playObject.RandomNo))
                {
                    playObject.RandomNo = n2C.ToString();
                    break;
                }
            }
        }

        private void ActionOfClearDelayGoto(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            playObject.IsTimeGoto = false;
            playObject.TimeGotoLable = "";
            playObject.TimeGotoNpc = null;
        }

        private void ActionOfTimereCall(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            playObject.IsTimeRecall = true;
            playObject.TimeRecallMoveMap = playObject.MapName;
            playObject.TimeRecallMoveX = playObject.CurrX;
            playObject.TimeRecallMoveY = playObject.CurrY;
            playObject.TimeRecallTick = HUtil32.GetTickCount() + (questActionInfo.nParam1 * 60 * 1000);
        }

        private void ActionOfMonGen(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var n3C = questActionInfo.nParam1;
            var n38 = questActionInfo.nParam1;
            var sMap = questActionInfo.sParam1;
            for (int k = 0; k < questActionInfo.nParam2; k++)
            {
                var n20X = M2Share.RandomNumber.Random(questActionInfo.nParam3 * 2 + 1) + (n38 - questActionInfo.nParam3);
                var n24Y = M2Share.RandomNumber.Random(questActionInfo.nParam3 * 2 + 1) + (n3C - questActionInfo.nParam3);
                M2Share.WorldEngine.RegenMonsterByName(sMap, (short)n20X, (short)n24Y, questActionInfo.sParam1);
            }
        }

        private void ActionOfMonClear(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var list58 = new List<BaseObject>();
            M2Share.WorldEngine.GetMapMonster(M2Share.MapMgr.FindMap(questActionInfo.sParam1), list58);
            for (int k = 0; k < list58.Count; k++)
            {
                list58[k].NoItem = true;
                list58[k].WAbil.HP = 0;
                list58[k].MakeGhost();
            }
            list58.Clear();
        }

        private void ActionOfMap(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            playObject.SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
            playObject.MapRandomMove(questActionInfo.sParam1, 0);
        }

        private void ActionOfMapMove(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            playObject.SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
            playObject.SpaceMove(questActionInfo.sParam1, (short)questActionInfo.nParam2, (short)questActionInfo.nParam3, 0);
        }

        private void ActionOfClose(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            playObject.SendMsg(normNpc, Messages.RM_MERCHANTDLGCLOSE, 0, normNpc.ActorId, 0, 0);
        }

        private void ActionOfReCallMap(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var recallEnvir = M2Share.MapMgr.FindMap(questActionInfo.sParam1);
            if (recallEnvir != null)
            {
                IList<BaseObject> recallList = new List<BaseObject>();
                M2Share.WorldEngine.GetMapRageHuman(recallEnvir, 0, 0, 1000, ref recallList);
                for (int k = 0; k < recallList.Count; k++)
                {
                    var user = (PlayObject)recallList[k];
                    user.MapRandomMove(recallEnvir.MapName, 0);
                    if (k > 20)
                    {
                        break;
                    }
                }
            }
            else
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.ReCallMap);
            }
        }

        private void ActionOfExchangeMap(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var envir = M2Share.MapMgr.FindMap(questActionInfo.sParam1);
            if (envir != null)
            {
                //IList<BaseObject> exchangeList = new List<BaseObject>();
                //M2Share.WorldEngine.GetMapRageHuman(envir, 0, 0, 1000, ref exchangeList);
                //if (exchangeList.Count > 0)
                //{
                //    var user = (PlayObject)exchangeList[0];
                //    user.MapRandomMove(normNpc.MapName, 0);
                //}
                playObject.MapRandomMove(questActionInfo.sParam1, 0);
            }
            else
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.ExchangeMap);
            }
        }

        private void ActionOfUpgradeDlgItem(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {

        }

        private void ActionOfQueryItemDlg(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            playObject.TakeDlgItem = questActionInfo.nParam3 != 0;
            playObject.GotoNpcLabel = questActionInfo.sParam2;
            string sHint = questActionInfo.sParam1;
            if (string.IsNullOrEmpty(sHint)) sHint = "请输入:";
            playObject.SendDefMessage(Messages.SM_QUERYITEMDLG, normNpc.ActorId, 0, 0, 0, sHint);
        }

        private void ActionOfKillSlaveName(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            string sSlaveName = questActionInfo.sParam1;
            if (string.IsNullOrEmpty(sSlaveName))
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.KillSlaveName);
                return;
            }
            if (sSlaveName.Equals("*") || string.Compare(sSlaveName, "ALL", StringComparison.OrdinalIgnoreCase) == 0)
            {
                for (int i = 0; i < playObject.SlaveList.Count; i++)
                {
                    playObject.SlaveList[i].WAbil.HP = 0;
                }
                return;
            }
            for (int i = 0; i < playObject.SlaveList.Count; i++)
            {
                BaseObject baseObject = playObject.SlaveList[i];
                if (!baseObject.Death && (string.Compare(sSlaveName, baseObject.ChrName, StringComparison.OrdinalIgnoreCase) == 0))
                {
                    baseObject.WAbil.HP = 0;
                }
            }
        }

        private void ActionOfQueryValue(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            int btStrLabel = questActionInfo.nParam1;
            if (btStrLabel < 100)
            {
                btStrLabel = 0;
            }
            playObject.ValLabel = (byte)btStrLabel;
            byte btType = (byte)questActionInfo.nParam2;
            if (btType > 3)
            {
                btType = 0;
            }
            playObject.ValType = btType;
            int btLen = HUtil32._MAX(1, questActionInfo.nParam3);
            playObject.GotoNpcLabel = questActionInfo.sParam4;
            string sHint = questActionInfo.sParam5;
            playObject.ValNpcType = 0;
            if (string.Compare(questActionInfo.sParam6, "QF", StringComparison.OrdinalIgnoreCase) == 0)
            {
                playObject.ValNpcType = 1;
            }
            else if (string.Compare(questActionInfo.sParam6, "QM", StringComparison.OrdinalIgnoreCase) == 0)
            {
                playObject.ValNpcType = 2;
            }
            if (string.IsNullOrEmpty(sHint))
            {
                sHint = "请输入：";
            }
            playObject.SendDefMessage(Messages.SM_QUERYVALUE, 0, HUtil32.MakeWord(btType, (ushort)btLen), 0, 0, sHint);
        }

        private void ActionOfQueryBagItems(NormNpc normNpc, PlayObject PlayObject, QuestActionInfo QuestActionInfo, ref bool Success)
        {
            Success = true;
            if ((HUtil32.GetTickCount() - PlayObject.QueryBagItemsTick) > M2Share.Config.QueryBagItemsTick)
            {
                PlayObject.QueryBagItemsTick = HUtil32.GetTickCount();
                PlayObject.ClientQueryBagItems();
            }
            else
            {
                PlayObject.SysMsg(Settings.QUERYBAGITEMS, MsgColor.Red, MsgType.Hint);
            }
        }

        private void ActionOfSetSendMsgFlag(NormNpc normNpc, PlayObject PlayObject, QuestActionInfo QuestActionInfo, ref bool Success)
        {
            Success = true;
            PlayObject.BoSendMsgFlag = true;
        }

        /// <summary>
        /// 开通元宝交易
        /// </summary>
        private void ActionOfOpenSaleDeal(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nGameGold = 0;
            try
            {
                if (playObject.SaleDeal)
                {
                    playObject.SendMsg(normNpc, Messages.RM_MERCHANTSAY, 0, 0, 0, 0, playObject.ChrName + "/您已开通寄售服务,不需要再开通!!!\\ \\<返回/@main>");
                    return;// 如已开通元宝服务则退出
                }
                if (!GetValValue(playObject, questActionInfo.sParam1, ref nGameGold))
                {
                    nGameGold = HUtil32.StrToInt(GetLineVariableText(playObject, questActionInfo.sParam1), 0);
                }
                if (playObject.GameGold >= nGameGold)// 玩家的元宝数大于或等于开通所需的元宝数
                {
                    playObject.GameGold -= nGameGold;
                    playObject.SaleDeal = true;
                    playObject.SendMsg(normNpc, Messages.RM_MERCHANTSAY, 0, 0, 0, 0, playObject.ChrName + "/开通寄售服务成功!!!\\ \\<返回/@main>");
                }
                else
                {
                    playObject.SendMsg(normNpc, Messages.RM_MERCHANTSAY, 0, 0, 0, 0, playObject.ChrName + "/您身上没有" + M2Share.Config.GameGoldName + ",或" + M2Share.Config.GameGoldName + "数不够!!!\\ \\<返回/@main>");
                }
            }
            catch
            {
                M2Share.Logger.Error("{异常} TNormNpc.ActionOfOPENYBDEAL");
            }
        }

        /// <summary>
        /// 查询正在出售的物品
        /// </summary>
        private void ActionOfQuerySaleSell(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            try
            {
                var bo12 = false;
                if (playObject.SaleDeal) // 已开通元宝服务
                {
                    if (playObject.SellOffInTime(0))
                    {
                        if (M2Share.SellOffItemList.Count > 0)
                        {
                            var sClientDealOffInfo = new ClientDealOffInfo();
                            sClientDealOffInfo.UseItems = new ClientItem[9];
                            for (var i = 0; i < M2Share.SellOffItemList.Count; i++)
                            {
                                var dealOffInfo = M2Share.SellOffItemList[i];
                                if (string.Compare(dealOffInfo.sDealChrName, playObject.ChrName, StringComparison.OrdinalIgnoreCase) == 0 && (dealOffInfo.Flag == 0 || dealOffInfo.Flag == 3))
                                {
                                    for (var j = 0; j < 9; j++)
                                    {
                                        if (dealOffInfo.UseItems[j] == null)
                                        {
                                            continue;
                                        }
                                        var stdItem = M2Share.WorldEngine.GetStdItem(dealOffInfo.UseItems[j].Index);
                                        if (stdItem == null)
                                        {
                                            // 是金刚石
                                            if (!bo12 && dealOffInfo.UseItems[j].MakeIndex > 0 && dealOffInfo.UseItems[j].Index == ushort.MaxValue && dealOffInfo.UseItems[j].Dura == ushort.MaxValue && dealOffInfo.UseItems[j].DuraMax == ushort.MaxValue)
                                            {
                                                var wvar1 = sClientDealOffInfo.UseItems[j];// '金刚石'
                                                //_wvar1.S.Name = Settings.Config.sGameDiaMond + '(' + (DealOffInfo.UseItems[K].MakeIndex).ToString() + ')';
                                                //_wvar1.S.Price = DealOffInfo.UseItems[K].MakeIndex;// 金刚石数量
                                                wvar1.Dura = ushort.MaxValue;// 客户端金刚石特征
                                                wvar1.Item.DuraMax = ushort.MaxValue;// 客户端金刚石特征
                                                wvar1.Item.Looks = ushort.MaxValue;// 不显示图片
                                                bo12 = true;
                                            }
                                            else
                                            {
                                                sClientDealOffInfo.UseItems[j].Item.Name = "";
                                            }
                                            continue;
                                        }
                                        //M2Share.ItemUnit.GetItemAddValue(DealOffInfo.UseItems[K], ref StdItem80);
                                        //Move(StdItem80, sClientDealOffInfo.UseItems[K].S, sizeof(TStdItem));
                                        sClientDealOffInfo.UseItems[j] = new ClientItem();
                                        ItemSystem.GetUpgradeStdItem(stdItem, dealOffInfo.UseItems[j], ref sClientDealOffInfo.UseItems[j]);
                                        //sClientDealOffInfo.UseItems[j].S = StdItem80;
                                        // 取自定义物品名称
                                        var sUserItemName = string.Empty;
                                        if (dealOffInfo.UseItems[j].Desc[13] == 1)
                                        {
                                            sUserItemName = M2Share.CustomItemMgr.GetCustomItemName(dealOffInfo.UseItems[j].MakeIndex, dealOffInfo.UseItems[j].Index);
                                            if (!string.IsNullOrEmpty(sUserItemName))
                                            {
                                                sClientDealOffInfo.UseItems[j].Item.Name = sUserItemName;
                                            }
                                        }
                                        sClientDealOffInfo.UseItems[j].MakeIndex = dealOffInfo.UseItems[j].MakeIndex;
                                        sClientDealOffInfo.UseItems[j].Dura = dealOffInfo.UseItems[j].Dura;
                                        sClientDealOffInfo.UseItems[j].DuraMax = dealOffInfo.UseItems[j].DuraMax;
                                        switch (stdItem.StdMode)
                                        {
                                            case 15:
                                            case 19:
                                            case 26:
                                                if (dealOffInfo.UseItems[j].Desc[8] != 0)
                                                {
                                                    sClientDealOffInfo.UseItems[j].Item.Shape = 130;
                                                }
                                                break;
                                        }
                                    }
                                    sClientDealOffInfo.DealChrName = dealOffInfo.sDealChrName;
                                    sClientDealOffInfo.BuyChrName = dealOffInfo.sBuyChrName;
                                    sClientDealOffInfo.SellDateTime = HUtil32.DateTimeToDouble(dealOffInfo.dSellDateTime);
                                    sClientDealOffInfo.SellGold = dealOffInfo.nSellGold;
                                    sClientDealOffInfo.N = dealOffInfo.Flag;
                                    var sSendStr = EDCode.EncodeBuffer(sClientDealOffInfo);
                                    playObject.SendMsg(normNpc, Messages.RM_QUERYYBSELL, 0, 0, 0, 0, sSendStr);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        normNpc.GotoLable(playObject, "@AskYBSellFail", false);
                    }
                }
                else
                {
                    playObject.SendMsg(playObject, Messages.RM_MENU_OK, 0, playObject.ActorId, 0, 0, "您未开通寄售服务,请先开通!!!");
                }
            }
            catch
            {
                M2Share.Logger.Error("{异常} TNormNpc.ActionOfQUERYYBSELL");
            }
        }

        /// <summary>
        /// 查询可以的购买物品
        /// </summary>
        private void ActionOfQueryTrustDeal(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            try
            {
                var bo12 = false;
                if (playObject.SaleDeal)  // 已开通元宝服务
                {
                    if (playObject.SellOffInTime(1))
                    {
                        if (M2Share.SellOffItemList.Count > 0)
                        {
                            var sClientDealOffInfo = new ClientDealOffInfo();
                            sClientDealOffInfo.UseItems = new ClientItem[9];
                            for (var i = 0; i < M2Share.SellOffItemList.Count; i++)
                            {
                                var dealOffInfo = M2Share.SellOffItemList[i];
                                if (string.Compare(dealOffInfo.sBuyChrName, playObject.ChrName, StringComparison.OrdinalIgnoreCase) == 0 && dealOffInfo.Flag == 0)
                                {
                                    for (var k = 0; k < 9; k++)
                                    {
                                        if (dealOffInfo.UseItems[k] == null)
                                        {
                                            continue;
                                        }
                                        var stdItem = M2Share.WorldEngine.GetStdItem(dealOffInfo.UseItems[k].Index);
                                        if (stdItem == null)
                                        {
                                            // 是金刚石
                                            if (!bo12 && dealOffInfo.UseItems[k].MakeIndex > 0 && dealOffInfo.UseItems[k].Index == short.MaxValue && dealOffInfo.UseItems[k].Dura == short.MaxValue && dealOffInfo.UseItems[k].DuraMax == short.MaxValue)
                                            {
                                                var wvar1 = sClientDealOffInfo.UseItems[k];// '金刚石'
                                                //_wvar1.S.Name = Settings.Config.sGameDiaMond + '(' + (DealOffInfo.UseItems[K].MakeIndex).ToString() + ')';
                                                //_wvar1.S.Price = DealOffInfo.UseItems[K].MakeIndex;
                                                //// 金刚石数量
                                                //_wvar1.Dura = UInt16.MaxValue;// 客户端金刚石特征
                                                //_wvar1.S.DuraMax = Int16.MaxValue;// 客户端金刚石特征
                                                //_wvar1.S.Looks = UInt16.MaxValue;// 不显示图片
                                                bo12 = true;
                                            }
                                            else
                                            {
                                                sClientDealOffInfo.UseItems[k].Item.Name = "";
                                            }
                                            continue;
                                        }

                                        //M2Share.ItemUnit.GetItemAddValue(DealOffInfo.UseItems[K], ref StdItem80);
                                        //Move(StdItem80, sClientDealOffInfo.UseItems[K].S);// 取自定义物品名称
                                        //sClientDealOffInfo.UseItems[K].S = StdItem80;
                                        sClientDealOffInfo.UseItems[k] = new ClientItem();
                                        //StdItem80.GetStandardItem(ref sClientDealOffInfo.UseItems[k].Item);
                                        var sUserItemName = string.Empty;
                                        if (dealOffInfo.UseItems[k].Desc[13] == 1)
                                        {
                                            sUserItemName = M2Share.CustomItemMgr.GetCustomItemName(dealOffInfo.UseItems[k].MakeIndex, dealOffInfo.UseItems[k].Index);
                                        }
                                        if (!string.IsNullOrEmpty(sUserItemName))
                                        {
                                            sClientDealOffInfo.UseItems[k].Item.Name = sUserItemName;
                                        }
                                        sClientDealOffInfo.UseItems[k].MakeIndex = dealOffInfo.UseItems[k].MakeIndex;
                                        sClientDealOffInfo.UseItems[k].Dura = dealOffInfo.UseItems[k].Dura;
                                        sClientDealOffInfo.UseItems[k].DuraMax = dealOffInfo.UseItems[k].DuraMax;
                                        switch (stdItem.StdMode)
                                        {
                                            // Modify the A .. B: 15, 19 .. 24, 26
                                            case 15:
                                            case 19:
                                            case 26:
                                                if (dealOffInfo.UseItems[k].Desc[8] != 0)
                                                {
                                                    sClientDealOffInfo.UseItems[k].Item.Shape = 130;
                                                }
                                                break;
                                        }
                                    }
                                    sClientDealOffInfo.DealChrName = dealOffInfo.sDealChrName;
                                    sClientDealOffInfo.BuyChrName = dealOffInfo.sBuyChrName;
                                    sClientDealOffInfo.SellDateTime = HUtil32.DateTimeToDouble(dealOffInfo.dSellDateTime);
                                    sClientDealOffInfo.SellGold = dealOffInfo.nSellGold;
                                    sClientDealOffInfo.N = dealOffInfo.Flag;
                                    var sSendStr = EDCode.EncodeBuffer(sClientDealOffInfo);
                                    playObject.SendMsg(normNpc, Messages.RM_QUERYYBDEAL, 0, 0, 0, 0, sSendStr);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        normNpc.GotoLable(playObject, "@AskYBDealFail", false);
                    }
                }
                else
                {
                    playObject.SendMsg(playObject, Messages.RM_MENU_OK, 0, playObject.ActorId, 0, 0, "您未开通元宝寄售服务,请先开通!!!");
                }
            }
            catch
            {
                M2Share.Logger.Error("{异常} TNormNpc.ActionOfQueryTrustDeal");
            }
        }

        private void ActionOfAddNameDateList(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var sHumName = string.Empty;
            var sDate = string.Empty;
            var sListFileName = M2Share.GetEnvirFilePath(normNpc.m_sPath, questActionInfo.sParam1);
            using var loadList = new StringList();
            if (File.Exists(sListFileName))
            {
                loadList.LoadFromFile(sListFileName);
            }
            var boFound = false;
            for (var i = 0; i < loadList.Count; i++)
            {
                var sLineText = loadList[i].Trim();
                sLineText = HUtil32.GetValidStr3(sLineText, ref sHumName, new[] { ' ', '\t' });
                sLineText = HUtil32.GetValidStr3(sLineText, ref sDate, new[] { ' ', '\t' });
                if (string.Compare(sHumName, playObject.ChrName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    loadList[i] = playObject.ChrName + "\t" + DateTime.Today;
                    boFound = true;
                    break;
                }
            }
            if (!boFound)
            {
                loadList.Add(playObject.ChrName + "\t" + DateTime.Today);
            }
            try
            {
                loadList.SaveToFile(sListFileName);
            }
            catch
            {
                M2Share.Logger.Error("saving fail.... => " + sListFileName);
            }
        }

        private void ActionOfDelNameDateList(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var sHumName = string.Empty;
            var sDate = string.Empty;
            var sListFileName = M2Share.GetEnvirFilePath(normNpc.m_sPath, questActionInfo.sParam1);
            using var loadList = new StringList();
            if (File.Exists(sListFileName))
            {
                loadList.LoadFromFile(sListFileName);
            }
            var boFound = false;
            for (var i = 0; i < loadList.Count; i++)
            {
                var sLineText = loadList[i].Trim();
                sLineText = HUtil32.GetValidStr3(sLineText, ref sHumName, new[] { ' ', '\t' });
                sLineText = HUtil32.GetValidStr3(sLineText, ref sDate, new[] { ' ', '\t' });
                if (string.Compare(sHumName, playObject.ChrName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    loadList.RemoveAt(i);
                    boFound = true;
                    break;
                }
            }
            if (boFound)
            {
                loadList.SaveToFile(sListFileName);
            }
        }

        private void ActionOfAddSkill(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nLevel = HUtil32._MIN(3, HUtil32.StrToInt(questActionInfo.sParam2, 0));
            var magic = M2Share.WorldEngine.FindMagic(questActionInfo.sParam1);
            if (magic != null)
            {
                if (!playObject.IsTrainingSkill(magic.MagicId))
                {
                    var userMagic = new UserMagic();
                    userMagic.Magic = magic;
                    userMagic.MagIdx = magic.MagicId;
                    userMagic.Key = (char)0;
                    userMagic.Level = (byte)nLevel;
                    userMagic.TranPoint = 0;
                    playObject.MagicList.Add(userMagic);
                    playObject.SendAddMagic(userMagic);
                    playObject.RecalcAbilitys();
                    if (M2Share.Config.ShowScriptActionMsg)
                    {
                        playObject.SysMsg(magic.MagicName + "练习成功。", MsgColor.Green, MsgType.Hint);
                    }
                }
            }
            else
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.AddSkill);
            }
        }

        private void ActionOfAutoAddGameGold(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            int nPoint = 0; 
            int nTime = 0;
            if (string.Compare(questActionInfo.sParam1, "START", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (nPoint > 0 && nTime > 0)
                {
                    playObject.IncGameGold = nPoint;
                    playObject.IncGameGoldTime = nTime * 1000;
                    playObject.IncGameGoldTick = HUtil32.GetTickCount();
                    playObject.BoIncGameGold = true;
                    return;
                }
            }
            if (string.Compare(questActionInfo.sParam1, "STOP", StringComparison.OrdinalIgnoreCase) == 0)
            {
                playObject.BoIncGameGold = false;
                return;
            }
            ScriptActionError(playObject, "", questActionInfo, ExecutionCode.AutoAddGameGold);
        }

        // SETAUTOGETEXP 时间 点数 是否安全区 地图号
        private void ActionOfAutoGetExp(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            Envirnoment envir = null;
            var nTime = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            var nPoint = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            var boIsSafeZone = questActionInfo.sParam3[1] == '1';
            var sMap = questActionInfo.sParam4;
            if (string.IsNullOrEmpty(sMap))
            {
                envir = M2Share.MapMgr.FindMap(sMap);
            }
            if (nTime <= 0 || nPoint <= 0 || string.IsNullOrEmpty(sMap) && envir == null)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.SetautogetExp);
                return;
            }
            playObject.AutoGetExpInSafeZone = boIsSafeZone;
            playObject.AutoGetExpEnvir = envir;
            playObject.AutoGetExpTime = nTime * 1000;
            playObject.AutoGetExpPoint = nPoint;
        }

        /// <summary>
        /// 增加挂机
        /// </summary>
        private void ActionOfOffLine(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            const string sOffLineStartMsg = "系统已经为你开启了脱机泡点功能，你现在可以下线了……";
            playObject.ClientMsg = Messages.MakeMessage(Messages.SM_SYSMESSAGE, playObject.ActorId, HUtil32.MakeWord(M2Share.Config.CustMsgFColor, M2Share.Config.CustMsgBColor), 0, 1);
            playObject.SendSocket(playObject.ClientMsg, EDCode.EncodeString(sOffLineStartMsg));
            var nTime = HUtil32.StrToInt(questActionInfo.sParam1, 5);
            var nPoint = HUtil32.StrToInt(questActionInfo.sParam2, 500);
            var nKickOffLine = HUtil32.StrToInt(questActionInfo.sParam3, 1440 * 15);
            playObject.AutoGetExpInSafeZone = true;
            playObject.AutoGetExpEnvir = playObject.Envir;
            playObject.AutoGetExpTime = nTime * 1000;
            playObject.AutoGetExpPoint = nPoint;
            playObject.OffLineFlag = true;
            playObject.KickOffLineTick = HUtil32.GetTickCount() + nKickOffLine * 60 * 1000;
            IdSrvClient.Instance.SendHumanLogOutMsgA(playObject.UserAccount, playObject.SessionId);
            playObject.SendDefMessage(Messages.SM_OUTOFCONNECTION, 0, 0, 0, 0);
        }

        private void ActionOfAutoSubGameGold(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            int nPoint = 0;
            int nTime = 0;
            if (string.Compare(questActionInfo.sParam1, "START", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (nPoint > 0 && nTime > 0)
                {
                    playObject.DecGameGold = nPoint;
                    playObject.DecGameGoldTime = nTime * 1000;
                    playObject.DecGameGoldTick = 0;
                    playObject.BoDecGameGold = true;
                    return;
                }
            }
            if (string.Compare(questActionInfo.sParam1, "STOP", StringComparison.OrdinalIgnoreCase) == 0)
            {
                playObject.BoDecGameGold = false;
                return;
            }
            ScriptActionError(playObject, "", questActionInfo, ExecutionCode.AutoSubGameGold);
        }

        private void ActionOfChangeCreditPoint(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nCreditPoint = (byte)HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nCreditPoint < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.CreditPoint);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nCreditPoint >= 0)
                    {
                        if (nCreditPoint > byte.MaxValue)
                        {
                            playObject.CreditPoint = byte.MaxValue;
                        }
                        else
                        {
                            playObject.CreditPoint = nCreditPoint;
                        }
                    }
                    break;
                case '-':
                    if (playObject.CreditPoint > nCreditPoint)
                    {
                        playObject.CreditPoint -= nCreditPoint;
                    }
                    else
                    {
                        playObject.CreditPoint = 0;
                    }
                    break;
                case '+':
                    if (playObject.CreditPoint + nCreditPoint > byte.MaxValue)
                    {
                        playObject.CreditPoint = byte.MaxValue;
                    }
                    else
                    {
                        playObject.CreditPoint += nCreditPoint;
                    }
                    break;
                default:
                    ScriptActionError(playObject, "", questActionInfo, ExecutionCode.CreditPoint);
                    return;
            }
        }

        private void ActionOfChangeExp(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            int dwInt;
            var nExp = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nExp < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.ChangeExp);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nExp > 0)
                    {
                        playObject.Abil.Exp = nExp;
                    }
                    break;
                case '-':
                    if (playObject.Abil.Exp > nExp)
                    {
                        playObject.Abil.Exp -= nExp;
                    }
                    else
                    {
                        playObject.Abil.Exp = 0;
                    }
                    break;
                case '+':
                    if (playObject.Abil.Exp >= nExp)
                    {
                        if (playObject.Abil.Exp - nExp > int.MaxValue - playObject.Abil.Exp)
                        {
                            dwInt = int.MaxValue - playObject.Abil.Exp;
                        }
                        else
                        {
                            dwInt = nExp;
                        }
                    }
                    else
                    {
                        if (nExp - playObject.Abil.Exp > int.MaxValue - nExp)
                        {
                            dwInt = int.MaxValue - nExp;
                        }
                        else
                        {
                            dwInt = nExp;
                        }
                    }
                    playObject.Abil.Exp += dwInt;
                    // PlayObject.GetExp(dwInt);
                    playObject.SendMsg(playObject, Messages.RM_WINEXP, 0, dwInt, 0, 0);
                    break;
            }
        }

        private void ActionOfChangeHairStyle(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nHair = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            if ((!string.IsNullOrEmpty(questActionInfo.sParam1)) && nHair >= 0)
            {
                playObject.Hair = (byte)nHair;
                playObject.FeatureChanged();
            }
            else
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.Hairstyle);
            }
        }

        private void ActionOfChangeJob(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nJob = PlayJob.None;
            if (HUtil32.CompareLStr(questActionInfo.sParam1, ScriptConst.sWarrior))
            {
                nJob = PlayJob.Warrior;
            }
            if (HUtil32.CompareLStr(questActionInfo.sParam1, ScriptConst.sWizard))
            {
                nJob = PlayJob.Wizard;
            }
            if (HUtil32.CompareLStr(questActionInfo.sParam1, ScriptConst.sTaos))
            {
                nJob = PlayJob.Taoist;
            }
            if (nJob == PlayJob.None)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.ChangeJob);
                return;
            }
            if (playObject.Job != nJob)
            {
                playObject.Job = nJob;
                playObject.HasLevelUp(0);
            }
        }

        private void ActionOfChangeLevel(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            int nLv;
            var boChgOk = false;
            int nOldLevel = playObject.Abil.Level;
            var nLevel = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.ChangeLevel);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nLevel > 0 && nLevel <= Grobal2.MaxLevel)
                    {
                        playObject.Abil.Level = (byte)nLevel;
                        boChgOk = true;
                    }
                    break;
                case '-':
                    nLv = HUtil32._MAX(0, playObject.Abil.Level - nLevel);
                    nLv = HUtil32._MIN(Grobal2.MaxLevel, nLv);
                    playObject.Abil.Level = (byte)nLv;
                    boChgOk = true;
                    break;
                case '+':
                    nLv = HUtil32._MAX(0, playObject.Abil.Level + nLevel);
                    nLv = HUtil32._MIN(Grobal2.MaxLevel, nLv);
                    playObject.Abil.Level = (byte)nLv;
                    boChgOk = true;
                    break;
            }
            if (boChgOk)
            {
                playObject.HasLevelUp(nOldLevel);
            }
        }

        private void ActionOfChangePkPoint(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            int nPoint;
            var nOldPkLevel = playObject.PvpLevel();
            var nPkPoint = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nPkPoint < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.ChangePkPoint);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nPkPoint >= 0)
                    {
                        playObject.PkPoint = nPkPoint;
                    }
                    break;
                case '-':
                    nPoint = HUtil32._MAX(0, playObject.PkPoint - nPkPoint);
                    playObject.PkPoint = nPoint;
                    break;
                case '+':
                    nPoint = HUtil32._MAX(0, playObject.PkPoint + nPkPoint);
                    playObject.PkPoint = nPoint;
                    break;
            }
            if (nOldPkLevel != playObject.PvpLevel())
            {
                playObject.RefNameColor();
            }
        }

        private void ActionOfClearMapMon(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            IList<BaseObject> monList = new List<BaseObject>();
            var monsterCount = M2Share.WorldEngine.GetMapMonster(M2Share.MapMgr.FindMap(questActionInfo.sParam1), monList);
            for (var i = 0; i < monsterCount; i++)
            {
                var mon = monList[i];
                if (mon.Master != null)
                {
                    continue;
                }
                if (M2Share.GetNoClearMonList(mon.ChrName))
                {
                    continue;
                }
                mon.NoItem = true;
                mon.WAbil.HP = 0;
                mon.MakeGhost();
            }
        }

        private void ActionOfClearList(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var sListFileName = M2Share.GetEnvirFilePath(questActionInfo.sParam1);
            File.WriteAllBytes(sListFileName, Array.Empty<byte>());
        }

        private void ActionOfClearSkill(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            for (var i = playObject.MagicList.Count - 1; i >= 0; i--)
            {
                var userMagic = playObject.MagicList[i];
                playObject.SendDelMagic(userMagic);
                playObject.MagicList.RemoveAt(i);
                Dispose(userMagic);
            }
            playObject.RecalcAbilitys();
        }

        private void ActionOfDelNoJobSkill(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            for (var i = playObject.MagicList.Count - 1; i >= 0; i--)
            {
                var userMagic = playObject.MagicList[i];
                if (userMagic.Magic.Job != (byte)playObject.Job)
                {
                    playObject.SendDelMagic(userMagic);
                    playObject.MagicList.RemoveAt(i);
                    Dispose(userMagic);
                }
            }
        }

        private void ActionOfDelSkill(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var magic = M2Share.WorldEngine.FindMagic(questActionInfo.sParam1);
            if (magic == null)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.DelSkill);
                return;
            }
            for (var i = 0; i < playObject.MagicList.Count; i++)
            {
                var userMagic = playObject.MagicList[i];
                if (string.CompareOrdinal(userMagic.Magic.MagicName, magic.MagicName) == 0)
                {
                    playObject.MagicList.RemoveAt(i);
                    playObject.SendDelMagic(userMagic);
                    Dispose(userMagic);
                    playObject.RecalcAbilitys();
                    break;
                }
            }
        }

        private void ActionOfGameGold(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nOldGameGold = playObject.GameGold;
            var nGameGold = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nGameGold < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.GameGold);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nGameGold >= 0)
                    {
                        playObject.GameGold = nGameGold;
                    }
                    break;
                case '-':
                    nGameGold = HUtil32._MAX(0, playObject.GameGold - nGameGold);
                    playObject.GameGold = nGameGold;
                    break;
                case '+':
                    nGameGold = HUtil32._MAX(0, playObject.GameGold + nGameGold);
                    playObject.GameGold = nGameGold;
                    break;
            }
            if (M2Share.GameLogGameGold)
            {
                M2Share.EventSource.AddEventLog(Grobal2.LogGameGold, string.Format(CommandHelp.GameLogMsg1, playObject.MapName, playObject.CurrX, playObject.CurrY, playObject.ChrName, M2Share.Config.GameGoldName, nGameGold, cMethod, normNpc.ChrName));
            }
            if (nOldGameGold != playObject.GameGold)
            {
                playObject.GameGoldChanged();
            }
        }

        private void ActionOfGamePoint(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nOldGamePoint = playObject.GamePoint;
            var nGamePoint = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nGamePoint < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.GamePoint);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nGamePoint >= 0)
                    {
                        playObject.GamePoint = nGamePoint;
                    }
                    break;
                case '-':
                    nGamePoint = HUtil32._MAX(0, playObject.GamePoint - nGamePoint);
                    playObject.GamePoint = nGamePoint;
                    break;
                case '+':
                    nGamePoint = HUtil32._MAX(0, playObject.GamePoint + nGamePoint);
                    playObject.GamePoint = nGamePoint;
                    break;
            }
            if (M2Share.GameLogGamePoint)
            {
                M2Share.EventSource.AddEventLog(Grobal2.LogGamePoint, string.Format(CommandHelp.GameLogMsg1, playObject.MapName, playObject.CurrX, playObject.CurrY, playObject.ChrName, M2Share.Config.GamePointName, nGamePoint, cMethod, normNpc.ChrName));
            }
            if (nOldGamePoint != playObject.GamePoint)
            {
                playObject.GameGoldChanged();
            }
        }

        private void ActionOfGetMarry(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var poseHuman = playObject.GetPoseCreate();
            if (poseHuman != null && poseHuman.Race == ActorRace.Play && ((PlayObject)poseHuman).Gender != playObject.Gender)
            {
                playObject.DearName = poseHuman.ChrName;
                playObject.RefShowName();
                poseHuman.RefShowName();
            }
            else
            {
                normNpc.GotoLable(playObject, "@MarryError", false);
            }
        }

        private void ActionOfGetMaster(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var poseHuman = playObject.GetPoseCreate();
            if (poseHuman != null && poseHuman.Race == ActorRace.Play && ((PlayObject)poseHuman).Gender != playObject.Gender)
            {
                playObject.MasterName = poseHuman.ChrName;
                playObject.RefShowName();
                poseHuman.RefShowName();
            }
            else
            {
                normNpc.GotoLable(playObject, "@MasterError", false);
            }
        }

        private void ActionOfLineMsg(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var sMsg = GetLineVariableText(playObject, questActionInfo.sParam2);
            sMsg = sMsg.Replace("%s", playObject.ChrName);
            sMsg = sMsg.Replace("%d", normNpc.ChrName);
            switch (questActionInfo.nParam1)
            {
                case 0:
                    M2Share.WorldEngine.SendBroadCastMsg(sMsg, MsgType.System);
                    break;
                case 1:
                    M2Share.WorldEngine.SendBroadCastMsg("(*) " + sMsg, MsgType.System);
                    break;
                case 2:
                    M2Share.WorldEngine.SendBroadCastMsg('[' + normNpc.ChrName + ']' + sMsg, MsgType.System);
                    break;
                case 3:
                    M2Share.WorldEngine.SendBroadCastMsg('[' + playObject.ChrName + ']' + sMsg, MsgType.System);
                    break;
                case 4:
                    normNpc.SendSayMsg(sMsg);
                    break;
                case 5:
                    playObject.SysMsg(sMsg, MsgColor.Red, MsgType.Say);
                    break;
                case 6:
                    playObject.SysMsg(sMsg, MsgColor.Green, MsgType.Say);
                    break;
                case 7:
                    playObject.SysMsg(sMsg, MsgColor.Blue, MsgType.Say);
                    break;
                case 8:
                    playObject.SendGroupText(sMsg);
                    break;
                case 9:
                    if (playObject.MyGuild != null)
                    {
                        playObject.MyGuild.SendGuildMsg(sMsg);
                        WorldServer.SendServerGroupMsg(Messages.SS_208, M2Share.ServerIndex, playObject.MyGuild.GuildName + "/" + playObject.ChrName + "/" + sMsg);
                    }
                    break;
                default:
                    ScriptActionError(playObject, "", questActionInfo, ExecutionCode.SendMsg);
                    break;
            }
        }

        private void ActionOfMapTing(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {

        }

        private void ActionOfMarry(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            string sSayMsg;
            if (!string.IsNullOrEmpty(playObject.DearName))
            {
                return;
            }
            var poseHuman = (PlayObject)playObject.GetPoseCreate();
            if (poseHuman == null)
            {
                normNpc.GotoLable(playObject, "@MarryCheckDir", false);
                return;
            }
            if (string.IsNullOrEmpty(questActionInfo.sParam1))
            {
                if (poseHuman.Race != ActorRace.Play)
                {
                    normNpc.GotoLable(playObject, "@HumanTypeErr", false);
                    return;
                }
                if (poseHuman.GetPoseCreate() == playObject)
                {
                    if (playObject.Gender != poseHuman.Gender)
                    {
                        normNpc.GotoLable(playObject, "@StartMarry", false);
                        normNpc.GotoLable(poseHuman, "@StartMarry", false);
                        if (playObject.Gender == PlayGender.Man && poseHuman.Gender == PlayGender.WoMan)
                        {
                            sSayMsg = string.Format(Settings.StartMarryManMsg, normNpc.ChrName, playObject.ChrName, poseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                            sSayMsg = string.Format(Settings.StartMarryManAskQuestionMsg, normNpc.ChrName, playObject.ChrName, poseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                        }
                        else if (playObject.Gender == PlayGender.WoMan && poseHuman.Gender == PlayGender.Man)
                        {
                            sSayMsg = string.Format(Settings.StartMarryWoManMsg, normNpc.ChrName, playObject.ChrName, poseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                            sSayMsg = string.Format(Settings.StartMarryWoManAskQuestionMsg, normNpc.ChrName, playObject.ChrName, poseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                        }
                        playObject.IsStartMarry = true;
                        poseHuman.IsStartMarry = true;
                    }
                    else
                    {
                        normNpc.GotoLable(poseHuman, "@MarrySexErr", false);
                        normNpc.GotoLable(playObject, "@MarrySexErr", false);
                    }
                }
                else
                {
                    normNpc.GotoLable(playObject, "@MarryDirErr", false);
                    normNpc.GotoLable(poseHuman, "@MarryCheckDir", false);
                }
                return;
            }
            // sREQUESTMARRY
            if (string.Compare(questActionInfo.sParam1, "REQUESTMARRY", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (playObject.IsStartMarry && poseHuman.IsStartMarry)
                {
                    if (playObject.Gender == PlayGender.Man && poseHuman.Gender == PlayGender.WoMan)
                    {
                        sSayMsg = Settings.MarryManAnswerQuestionMsg.Replace("%n", normNpc.ChrName);
                        sSayMsg = sSayMsg.Replace("%s", playObject.ChrName);
                        sSayMsg = sSayMsg.Replace("%d", poseHuman.ChrName);
                        M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                        sSayMsg = Settings.MarryManAskQuestionMsg.Replace("%n", normNpc.ChrName);
                        sSayMsg = sSayMsg.Replace("%s", playObject.ChrName);
                        sSayMsg = sSayMsg.Replace("%d", poseHuman.ChrName);
                        M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                        normNpc.GotoLable(playObject, "@WateMarry", false);
                        normNpc.GotoLable(poseHuman, "@RevMarry", false);
                    }
                }
                return;
            }
            // sRESPONSEMARRY
            if (string.Compare(questActionInfo.sParam1, "RESPONSEMARRY", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (playObject.Gender == PlayGender.WoMan && poseHuman.Gender == PlayGender.Man)
                {
                    if (string.Compare(questActionInfo.sParam2, "OK", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        if (playObject.IsStartMarry && poseHuman.IsStartMarry)
                        {
                            sSayMsg = string.Format(Settings.MarryWoManAnswerQuestionMsg, normNpc.ChrName, playObject.ChrName, poseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                            sSayMsg = string.Format(Settings.MarryWoManGetMarryMsg, normNpc.ChrName, playObject.ChrName, poseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                            normNpc.GotoLable(playObject, "@EndMarry", false);
                            normNpc.GotoLable(poseHuman, "@EndMarry", false);
                            playObject.IsStartMarry = false;
                            poseHuman.IsStartMarry = false;
                            playObject.DearName = poseHuman.ChrName;
                            playObject.DearHuman = poseHuman;
                            poseHuman.DearName = playObject.ChrName;
                            poseHuman.DearHuman = playObject;
                            playObject.RefShowName();
                            poseHuman.RefShowName();
                        }
                    }
                    else
                    {
                        if (playObject.IsStartMarry && poseHuman.IsStartMarry)
                        {
                            normNpc.GotoLable(playObject, "@EndMarryFail", false);
                            normNpc.GotoLable(poseHuman, "@EndMarryFail", false);
                            playObject.IsStartMarry = false;
                            poseHuman.IsStartMarry = false;
                            sSayMsg = string.Format(Settings.MarryWoManDenyMsg, normNpc.ChrName, playObject.ChrName, poseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                            sSayMsg = string.Format(Settings.MarryWoManCancelMsg, normNpc.ChrName, playObject.ChrName, poseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                        }
                    }
                }
            }
        }

        private void ActionOfMaster(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            if (!string.IsNullOrEmpty(playObject.MasterName))
            {
                return;
            }
            var poseHuman = (PlayObject)playObject.GetPoseCreate();
            if (poseHuman == null)
            {
                normNpc.GotoLable(playObject, "@MasterCheckDir", false);
                return;
            }
            if ((string.IsNullOrEmpty(questActionInfo.sParam1)))
            {
                if (poseHuman.Race != ActorRace.Play)
                {
                    normNpc.GotoLable(playObject, "@HumanTypeErr", false);
                    return;
                }
                if (poseHuman.GetPoseCreate() == playObject)
                {
                    normNpc.GotoLable(playObject, "@StartGetMaster", false);
                    normNpc.GotoLable(poseHuman, "@StartMaster", false);
                    playObject.IsStartMaster = true;
                    poseHuman.IsStartMaster = true;
                }
                else
                {
                    normNpc.GotoLable(playObject, "@MasterDirErr", false);
                    normNpc.GotoLable(poseHuman, "@MasterCheckDir", false);
                }
                return;
            }
            if (string.Compare(questActionInfo.sParam1, "REQUESTMASTER", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (playObject.IsStartMaster && poseHuman.IsStartMaster)
                {
                    playObject.PoseBaseObject = poseHuman.ActorId;
                    poseHuman.PoseBaseObject = playObject.ActorId;
                    normNpc.GotoLable(playObject, "@WateMaster", false);
                    normNpc.GotoLable(poseHuman, "@RevMaster", false);
                }
                return;
            }
            if (string.Compare(questActionInfo.sParam1, "RESPONSEMASTER", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (string.Compare(questActionInfo.sParam2, "OK", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (playObject.PoseBaseObject == poseHuman.ActorId && poseHuman.PoseBaseObject == playObject.ActorId)
                    {
                        if (playObject.IsStartMaster && poseHuman.IsStartMaster)
                        {
                            normNpc.GotoLable(playObject, "@EndMaster", false);
                            normNpc.GotoLable(poseHuman, "@EndMaster", false);
                            playObject.IsStartMaster = false;
                            poseHuman.IsStartMaster = false;
                            if (string.IsNullOrEmpty(playObject.MasterName))
                            {
                                playObject.MasterName = poseHuman.ChrName;
                                playObject.IsMaster = true;
                            }
                            playObject.MasterList.Add(poseHuman);
                            poseHuman.MasterName = playObject.ChrName;
                            poseHuman.IsMaster = false;
                            playObject.RefShowName();
                            poseHuman.RefShowName();
                        }
                    }
                }
                else
                {
                    if (playObject.IsStartMaster && poseHuman.IsStartMaster)
                    {
                        normNpc.GotoLable(playObject, "@EndMasterFail", false);
                        normNpc.GotoLable(poseHuman, "@EndMasterFail", false);
                        playObject.IsStartMaster = false;
                        poseHuman.IsStartMaster = false;
                    }
                }
            }
        }

        private void ActionOfMessageBox(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            playObject.SendMsg(normNpc, Messages.RM_MENU_OK, 0, playObject.ActorId, 0, 0, GetLineVariableText(playObject, questActionInfo.sParam1));
        }

        private void ActionOfMission(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            if (!string.IsNullOrEmpty(questActionInfo.sParam1) && questActionInfo.nParam2 > 0 && questActionInfo.nParam3 > 0)
            {
                M2Share.MissionMap = questActionInfo.sParam1;
                M2Share.MissionX = (short)questActionInfo.nParam2;
                M2Share.MissionY = (short)questActionInfo.nParam3;
            }
            else
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.Mission);
            }
        }

        // MOBFIREBURN MAP X Y TYPE TIME POINT
        private void ActionOfMobFireBurn(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var sMap = questActionInfo.sParam1;
            var nX = HUtil32.StrToInt16(questActionInfo.sParam2, -1);
            var nY = HUtil32.StrToInt16(questActionInfo.sParam3, -1);
            var nType = (byte)HUtil32.StrToInt(questActionInfo.sParam4, -1);
            var nTime = HUtil32.StrToInt(questActionInfo.sParam5, -1);
            var nPoint = HUtil32.StrToInt(questActionInfo.sParam6, -1);
            if (string.IsNullOrEmpty(sMap) || nX < 0 || nY < 0 || nType < 0 || nTime < 0 || nPoint < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.MobFireburn);
                return;
            }
            var envir = M2Share.MapMgr.FindMap(sMap);
            if (envir != null)
            {
                var oldEnvir = playObject.Envir;
                playObject.Envir = envir;
                var fireBurnEvent = new FireBurnEvent(playObject, nX, nY, nType, nTime * 1000, nPoint);
                M2Share.EventMgr.AddEvent(fireBurnEvent);
                playObject.Envir = oldEnvir;
                return;
            }
            ScriptActionError(playObject, "", questActionInfo, ExecutionCode.MobFireburn);
        }

        private void ActionOfMobPlace(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            int nX = 0;
            int nY = 0;
            int nCount = 0;
            int nRange = 0;
            for (var i = 0; i < nCount; i++)
            {
                var nRandX = (short)(M2Share.RandomNumber.Random(nRange * 2 + 1) + (nX - nRange));
                var nRandY = (short)(M2Share.RandomNumber.Random(nRange * 2 + 1) + (nY - nRange));
                var mon = M2Share.WorldEngine.RegenMonsterByName(M2Share.MissionMap, nRandX, nRandY, questActionInfo.sParam1);
                if (mon != null)
                {
                    mon.Mission = true;
                    mon.MissionX = M2Share.MissionX;
                    mon.MissionY = M2Share.MissionY;
                }
                else
                {
                    ScriptActionError(playObject, "", questActionInfo, ExecutionCode.MobPlace);
                    break;
                }
            }
        }

        private void ActionOfRecallGroupMembers(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
        }

        private void ActionOfSetRankLevelName(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var sRankLevelName = questActionInfo.sParam1;
            if (string.IsNullOrEmpty(sRankLevelName))
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.SkillLevel);
                return;
            }
            playObject.RankLevelName = sRankLevelName;
            playObject.RefShowName();
        }

        private void ActionOfSetScriptFlag(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nWhere = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            var boFlag = HUtil32.StrToInt(questActionInfo.sParam2, -1) == 1;
            switch (nWhere)
            {
                case 0:
                    playObject.BoSendMsgFlag = boFlag;
                    break;
                case 1:
                    playObject.BoChangeItemNameFlag = boFlag;
                    break;
                default:
                    ScriptActionError(playObject, "", questActionInfo, ExecutionCode.SetscriptFlag);
                    break;
            }
        }

        private void ActionOfSkillLevel(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nLevel = HUtil32.StrToInt(questActionInfo.sParam3, 0);
            if (nLevel < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.SkillLevel);
                return;
            }
            var cMethod = questActionInfo.sParam2[0];
            var magic = M2Share.WorldEngine.FindMagic(questActionInfo.sParam1);
            if (magic != null)
            {
                for (var i = 0; i < playObject.MagicList.Count; i++)
                {
                    var userMagic = playObject.MagicList[i];
                    if (userMagic.Magic == magic)
                    {
                        switch (cMethod)
                        {
                            case '=':
                                if (nLevel >= 0)
                                {
                                    nLevel = HUtil32._MAX(3, nLevel);
                                    userMagic.Level = (byte)nLevel;
                                }
                                break;
                            case '-':
                                if (userMagic.Level >= nLevel)
                                {
                                    userMagic.Level -= (byte)nLevel;
                                }
                                else
                                {
                                    userMagic.Level = 0;
                                }
                                break;
                            case '+':
                                if (userMagic.Level + nLevel <= 3)
                                {
                                    userMagic.Level += (byte)nLevel;
                                }
                                else
                                {
                                    userMagic.Level = 3;
                                }
                                break;
                        }
                        playObject.SendSelfDelayMsg(Messages.RM_MAGIC_LVEXP, 0, userMagic.Magic.MagicId, userMagic.Level, userMagic.TranPoint, "", 100);
                        break;
                    }
                }
            }
        }

        private void ActionOfTakeCastleGold(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nGold = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            if (nGold < 0 || normNpc.Castle == null)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.TakeCastleGold);
                return;
            }
            if (nGold <= normNpc.Castle.TotalGold)
            {
                normNpc.Castle.TotalGold -= nGold;
            }
            else
            {
                normNpc.Castle.TotalGold = 0;
            }
        }

        private void ActionOfUnMarry(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            if (string.IsNullOrEmpty(playObject.DearName))
            {
                normNpc.GotoLable(playObject, "@ExeMarryFail", false);
                return;
            }
            var poseHuman = (PlayObject)playObject.GetPoseCreate();
            if (poseHuman == null)
            {
                normNpc.GotoLable(playObject, "@UnMarryCheckDir", false);
            }
            if (poseHuman != null)
            {
                if (string.IsNullOrEmpty(questActionInfo.sParam1))
                {
                    if (poseHuman.Race != ActorRace.Play)
                    {
                        normNpc.GotoLable(playObject, "@UnMarryTypeErr", false);
                        return;
                    }
                    if (poseHuman.GetPoseCreate() == playObject)
                    {
                        if (playObject.DearName == poseHuman.ChrName)
                        {
                            normNpc.GotoLable(playObject, "@StartUnMarry", false);
                            normNpc.GotoLable(poseHuman, "@StartUnMarry", false);
                            return;
                        }
                    }
                }
            }
            // sREQUESTUNMARRY
            if (string.Compare(questActionInfo.sParam1, "REQUESTUNMARRY", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (string.IsNullOrEmpty(questActionInfo.sParam2))
                {
                    if (poseHuman != null)
                    {
                        playObject.IsStartUnMarry = true;
                        if (playObject.IsStartUnMarry && poseHuman.IsStartUnMarry)
                        {
                            // sUnMarryMsg8
                            // sMarryMsg0
                            // sUnMarryMsg9
                            M2Share.WorldEngine.SendBroadCastMsg('[' + normNpc.ChrName + "]: " + "我宣布" + poseHuman.ChrName + ' ' + '与' + playObject.ChrName + ' ' + ' ' + "正式脱离夫妻关系。", MsgType.Say);
                            playObject.DearName = "";
                            poseHuman.DearName = "";
                            playObject.MarryCount++;
                            poseHuman.MarryCount++;
                            playObject.IsStartUnMarry = false;
                            poseHuman.IsStartUnMarry = false;
                            playObject.RefShowName();
                            poseHuman.RefShowName();
                            normNpc.GotoLable(playObject, "@UnMarryEnd", false);
                            normNpc.GotoLable(poseHuman, "@UnMarryEnd", false);
                        }
                        else
                        {
                            normNpc.GotoLable(playObject, "@WateUnMarry", false);
                            // GotoLable(PoseHuman,'@RevUnMarry',False);
                        }
                    }
                }
                else
                {
                    // 强行离婚
                    if (string.Compare(questActionInfo.sParam2, "FORCE", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        M2Share.WorldEngine.SendBroadCastMsg('[' + normNpc.ChrName + "]: " + "我宣布" + playObject.ChrName + ' ' + '与' + playObject.DearName + ' ' + ' ' + "已经正式脱离夫妻关系!!!", MsgType.Say);
                        poseHuman = M2Share.WorldEngine.GetPlayObject(playObject.DearName);
                        if (poseHuman != null)
                        {
                            poseHuman.DearName = string.Empty;
                            poseHuman.MarryCount++;
                            poseHuman.RefShowName();
                        }
                        else
                        {
                            //sUnMarryFileName = Settings.Config.sEnvirDir + "UnMarry.txt";
                            //LoadList = new StringList();
                            //if (File.Exists(sUnMarryFileName))
                            //{
                            //    LoadList.LoadFromFile(sUnMarryFileName);
                            //}
                            //LoadList.Add(PlayObject.m_sDearName);
                            //LoadList.SaveToFile(sUnMarryFileName);
                            //LoadList.Free;
                        }
                        playObject.DearName = string.Empty;
                        playObject.MarryCount++;
                        normNpc.GotoLable(playObject, "@UnMarryEnd", false);
                        playObject.RefShowName();
                    }
                }
            }
        }

        /// <summary>
        /// 保存变量值
        /// SAVEVAR 变量类型 变量名 文件名
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questActionInfo"></param>
        private void ActionOfSaveVar(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var sName = string.Empty;
            DynamicVar dynamicVar = null;
            const string sVarFound = "变量{0}不存在，变量类型:{1}";
            const string sVarTypeError = "变量类型错误，错误类型:{0} 当前支持类型(HUMAN、GUILD、GLOBAL)";
            var sType = questActionInfo.sParam1;
            var sVarName = questActionInfo.sParam2;
            var sFileName = GetLineVariableText(playObject, questActionInfo.sParam3);
            if (sFileName[0] == '\\')
            {
                sFileName = sFileName[1..];
            }
            if (sFileName[1] == '\\')
            {
                sFileName = sFileName[2..];
            }
            if (sFileName[2] == '\\')
            {
                sFileName = sFileName[3..];
            }
            sFileName = M2Share.GetEnvirFilePath(sFileName);
            if (string.IsNullOrEmpty(sType) || string.IsNullOrEmpty(sVarName) || !File.Exists(sFileName))
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.SaveVar);
                return;
            }
            var boFoundVar = false;
            var dynamicVarList = NormNpc.GetDynamicVarMap(playObject, sType, ref sName);
            if (dynamicVarList == null)
            {
                Dispose(dynamicVar);
                ScriptActionError(playObject, string.Format(sVarTypeError, sType), questActionInfo, ExecutionCode.Var);
                return;
            }
            if (dynamicVarList.TryGetValue(sVarName, out dynamicVar))
            {
                var iniFile = new ConfFile(sFileName);
                iniFile.Load();
                if (dynamicVar.VarType == VarType.Integer)
                {
                    dynamicVarList[sVarName].nInternet = dynamicVar.nInternet;
                    iniFile.WriteInteger(sName, dynamicVar.sName, dynamicVar.nInternet);
                }
                else
                {
                    dynamicVarList[sVarName].sString = dynamicVar.sString;
                    iniFile.WriteString(sName, dynamicVar.sName, dynamicVar.sString);
                }
                boFoundVar = true;
            }
            if (!boFoundVar)
            {
                ScriptActionError(playObject, string.Format(sVarFound, sVarName, sType), questActionInfo, ExecutionCode.SaveVar);
            }
        }

        private void ActionOfDelayCall(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            playObject.DelayCall = HUtil32._MAX(1, questActionInfo.nParam1);
            playObject.DelayCallLabel = questActionInfo.sParam2;
            playObject.DelayCallTick = HUtil32.GetTickCount();
            playObject.IsDelayCall = true;
            playObject.DelayCallNpc = normNpc.ActorId;

            playObject.IsTimeGoto = true;
            int mDelayGoto = HUtil32.StrToInt(GetLineVariableText(playObject, questActionInfo.sParam1), 0);//变量操作
            if (mDelayGoto == 0)
            {
                int delayCount = 0;
                GetValValue(playObject, questActionInfo.sParam1, ref delayCount);
                mDelayGoto = delayCount;
            }
            if (mDelayGoto > 0)
            {
                playObject.TimeGotoTick = HUtil32.GetTickCount() + mDelayGoto;
            }
            else
            {
                playObject.TimeGotoTick = HUtil32.GetTickCount() + questActionInfo.nParam1;//毫秒
            }
            playObject.TimeGotoLable = questActionInfo.sParam2;
            playObject.TimeGotoNpc = normNpc;
        }

        /// <summary>
        /// 对变量进行运算(+、-、*、/)
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questActionInfo"></param>
        private void ActionOfCalcVar(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var sName = string.Empty;
            DynamicVar dynamicVar = null;
            var sVarValue2 = string.Empty;
            const string sVarFound = "变量{0}不存在，变量类型:{1}";
            const string sVarTypeError = "变量类型错误，错误类型:{0} 当前支持类型(HUMAN、GUILD、GLOBAL)";
            var sType = questActionInfo.sParam1;//类型
            var sVarName = questActionInfo.sParam2;//自定义变量
            var sMethod = questActionInfo.sParam3;//操作符 +-*/=
            var sVarValue = questActionInfo.sParam4;//变量
            var nVarValue = HUtil32.StrToInt(questActionInfo.sParam4, 0);
            if (string.IsNullOrEmpty(sType) || string.IsNullOrEmpty(sVarName) || string.IsNullOrEmpty(sMethod))
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.CalcVar);
                return;
            }
            var boFoundVar = false;
            if (!string.IsNullOrEmpty(sVarValue) && !HUtil32.IsStringNumber(sVarValue))
            {
                if (HUtil32.CompareLStr(sVarValue, "<$HUMAN(", 8))
                {
                    HUtil32.ArrestStringEx(sVarValue, "(", ")", ref sVarValue2);
                    sVarValue = sVarValue2;
                    if (playObject.DynamicVarMap.Count > 0)
                    {
                        if (playObject.DynamicVarMap.TryGetValue(sVarValue, out dynamicVar))
                        {
                            switch (dynamicVar.VarType)
                            {
                                case VarType.Integer:
                                    nVarValue = dynamicVar.nInternet;
                                    break;
                                case VarType.String:
                                    sVarValue = dynamicVar.sString;
                                    break;
                            }
                            boFoundVar = true;
                        }
                    }
                    if (!boFoundVar)
                    {
                        ScriptActionError(playObject, string.Format(sVarFound, sVarValue, sType), questActionInfo, ExecutionCode.CalcVar);
                        return;
                    }
                }
                else
                {
                    nVarValue = HUtil32.StrToInt(GetLineVariableText(playObject, sVarValue), 0);
                    sVarValue = GetLineVariableText(playObject, sVarValue);
                }
            }
            else
            {
                nVarValue = HUtil32.StrToInt(questActionInfo.sParam4, 0);
            }
            var cMethod = sMethod[0];
            var dynamicVarList = NormNpc.GetDynamicVarMap(playObject, sType, ref sName);
            if (dynamicVarList == null)
            {
                Dispose(dynamicVar);
                ScriptActionError(playObject, string.Format(sVarTypeError, sType), questActionInfo, ExecutionCode.CalcVar);
                return;
            }

            if (playObject.DynamicVarMap.TryGetValue(sVarName, out dynamicVar))
            {
                switch (dynamicVar.VarType)
                {
                    case VarType.Integer:
                        switch (cMethod)
                        {
                            case '=':
                                dynamicVar.nInternet = nVarValue;
                                break;
                            case '+':
                                dynamicVar.nInternet = dynamicVar.nInternet + nVarValue;
                                break;
                            case '-':
                                dynamicVar.nInternet = dynamicVar.nInternet - nVarValue;
                                break;
                            case '*':
                                dynamicVar.nInternet = dynamicVar.nInternet * nVarValue;
                                break;
                            case '/':
                                dynamicVar.nInternet = dynamicVar.nInternet / nVarValue;
                                break;
                        }
                        break;
                    case VarType.String:
                        switch (cMethod)
                        {
                            case '=':
                                dynamicVar.sString = sVarValue;
                                break;
                            case '+':
                                dynamicVar.sString = dynamicVar.sString + sVarValue;
                                break;
                            case '-':
                                break;
                        }
                        break;
                }
                boFoundVar = true;
            }
            if (!boFoundVar)
            {
                ScriptActionError(playObject, string.Format(sVarFound, sVarName, sType), questActionInfo, ExecutionCode.CalcVar);
            }
        }

        private void ActionOfGuildRecall(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            if (playObject.MyGuild != null)
            {
                // PlayObject.GuildRecall('GuildRecall','');
            }
        }

        private void ActionOfGroupAddList(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var ffile = questActionInfo.sParam1;
            if (playObject.GroupOwner != 0)
            {
                for (var i = 0; i < playObject.GroupMembers.Count; i++)
                {
                    playObject = playObject.GroupMembers[i];
                    // AddListEx(PlayObject.m_sChrName,ffile);
                }
            }
        }

        private void ActionOfGroupRecall(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            if (playObject.GroupOwner != 0)
            {
                // PlayObject.GroupRecall('GroupRecall');
            }
        }

        /// <summary>
        /// 特修身上所有装备
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questActionInfo"></param>
        private void ActionOfRepairAllItem(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var boIsHasItem = false;
            for (var i = 0; i < playObject.UseItems.Length; i++)
            {
                if (playObject.UseItems[i].Index <= 0)
                {
                    continue;
                }
                var sUserItemName = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[i].Index);
                if (!(i != ItemLocation.Charm))
                {
                    playObject.SysMsg(sUserItemName + " 禁止修理...", MsgColor.Red, MsgType.Hint);
                    continue;
                }
                playObject.UseItems[i].Dura = playObject.UseItems[i].DuraMax;
                playObject.SendMsg(normNpc, Messages.RM_DURACHANGE, (short)i, playObject.UseItems[i].Dura, playObject.UseItems[i].DuraMax, 0);
                boIsHasItem = true;
            }
            if (boIsHasItem)
            {
                playObject.SysMsg("您身上的的装备修复成功了...", MsgColor.Blue, MsgType.Hint);
            }
        }

        private void ActionOfGroupMoveMap(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var boFlag = false;
            if (playObject.GroupOwner != 0)
            {
                var envir = M2Share.MapMgr.FindMap(questActionInfo.sParam1);
                if (envir != null)
                {
                    if (envir.CanWalk(questActionInfo.nParam2, questActionInfo.nParam3, true))
                    {
                        for (var i = 0; i < playObject.GroupMembers.Count; i++)
                        {
                            var playObjectEx = playObject.GroupMembers[i];
                            playObjectEx.SpaceMove(questActionInfo.sParam1, (short)questActionInfo.nParam2, (short)questActionInfo.nParam3, 0);
                        }
                        boFlag = true;
                    }
                }
            }
            if (!boFlag)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.GroupMoveMap);
            }
        }

        private void ActionOfUpgradeItems(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            int nAddPoint;
            var nWhere = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            var nRate = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            var nPoint = HUtil32.StrToInt(questActionInfo.sParam3, -1);
            if (nWhere < 0 || nWhere > playObject.UseItems.Length || nRate < 0 || nPoint < 0 || nPoint > 255)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.UpgradeItems);
                return;
            }
            var userItem = playObject.UseItems[nWhere];
            var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
            if (userItem.Index <= 0 || stdItem == null)
            {
                playObject.SysMsg("你身上没有戴指定物品!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            nRate = M2Share.RandomNumber.Random(nRate);
            nPoint = M2Share.RandomNumber.Random(nPoint);
            var nValType = M2Share.RandomNumber.Random(14);
            if (nRate != 0)
            {
                playObject.SysMsg("装备升级失败!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (nValType == 14)
            {
                nAddPoint = nPoint * 1000;
                if (userItem.DuraMax + nAddPoint > ushort.MaxValue)
                {
                    nAddPoint = ushort.MaxValue - userItem.DuraMax;
                }
                userItem.DuraMax = (ushort)(userItem.DuraMax + nAddPoint);
            }
            else
            {
                nAddPoint = nPoint;
                if (userItem.Desc[nValType] + nAddPoint > byte.MaxValue)
                {
                    nAddPoint = byte.MaxValue - userItem.Desc[nValType];
                }
                userItem.Desc[nValType] = (byte)(userItem.Desc[nValType] + nAddPoint);
            }
            playObject.SendUpdateItem(userItem);
            playObject.SysMsg("装备升级成功", MsgColor.Green, MsgType.Hint);
            playObject.SysMsg(stdItem.Name + ": " + userItem.Dura + '/' + userItem.DuraMax + '/' + userItem.Desc[0] + '/' + userItem.Desc[1] + '/' + userItem.Desc[2] + '/' + userItem.Desc[3] + '/' + userItem.Desc[4] + '/' + userItem.Desc[5] + '/' + userItem.Desc[6] + '/' + userItem.Desc[7] + '/' + userItem.Desc[8] + '/' + userItem.Desc[9] + '/' + userItem.Desc[ItemAttr.WeaponUpgrade] + '/' + userItem.Desc[11] + '/' + userItem.Desc[12] + '/' + userItem.Desc[13], MsgColor.Blue, MsgType.Hint);
        }

        private void ActionOfUpgradeItemsEx(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            int nAddPoint;
            var nWhere = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            var nValType = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            var nRate = HUtil32.StrToInt(questActionInfo.sParam3, -1);
            var nPoint = HUtil32.StrToInt(questActionInfo.sParam4, -1);
            var nUpgradeItemStatus = HUtil32.StrToInt(questActionInfo.sParam5, -1);
            if (nValType < 0 || nValType > 14 || nWhere < 0 || nWhere > playObject.UseItems.Length || nRate < 0 || nPoint < 0 || nPoint > 255)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.UpgradeItemSex);
                return;
            }
            var userItem = playObject.UseItems[nWhere];
            var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
            if (userItem.Index <= 0 || stdItem == null)
            {
                playObject.SysMsg("你身上没有戴指定物品!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            var nRatePoint = M2Share.RandomNumber.Random(nRate * 10);
            nPoint = HUtil32._MAX(1, M2Share.RandomNumber.Random(nPoint));
            if (!(nRatePoint >= 0 && nRatePoint <= 10))
            {
                switch (nUpgradeItemStatus)
                {
                    case 0:
                        playObject.SysMsg("装备升级未成功!!!", MsgColor.Red, MsgType.Hint);
                        break;
                    case 1:
                        playObject.SendDelItems(userItem);
                        userItem.Index = 0;
                        playObject.SysMsg("装备破碎!!!", MsgColor.Red, MsgType.Hint);
                        break;
                    case 2:
                        playObject.SysMsg("装备升级失败，装备属性恢复默认!!!", MsgColor.Red, MsgType.Hint);
                        if (nValType != 14)
                        {
                            userItem.Desc[nValType] = 0;
                        }
                        break;
                }
                return;
            }
            if (nValType == 14)
            {
                nAddPoint = nPoint * 1000;
                if (userItem.DuraMax + nAddPoint > ushort.MaxValue)
                {
                    nAddPoint = ushort.MaxValue - userItem.DuraMax;
                }
                userItem.DuraMax = (ushort)(userItem.DuraMax + nAddPoint);
            }
            else
            {
                nAddPoint = nPoint;
                if (userItem.Desc[nValType] + nAddPoint > byte.MaxValue)
                {
                    nAddPoint = byte.MaxValue - userItem.Desc[nValType];
                }
                userItem.Desc[nValType] = (byte)(userItem.Desc[nValType] + nAddPoint);
            }
            playObject.SendUpdateItem(userItem);
            playObject.SysMsg("装备升级成功", MsgColor.Green, MsgType.Hint);
            playObject.SysMsg(stdItem.Name + ": " + userItem.Dura + '/' + userItem.DuraMax + '-' + userItem.Desc[0] + '/' + userItem.Desc[1] + '/' + userItem.Desc[2] + '/' + userItem.Desc[3] + '/' + userItem.Desc[4] + '/' + userItem.Desc[5] + '/' + userItem.Desc[6] + '/' + userItem.Desc[7] + '/' + userItem.Desc[8] + '/' + userItem.Desc[9] + '/' + userItem.Desc[ItemAttr.WeaponUpgrade] + '/' + userItem.Desc[11] + '/' + userItem.Desc[12] + '/' + userItem.Desc[13], MsgColor.Blue, MsgType.Hint);
        }

        /// <summary>
        /// 声明变量
        /// VAR 数据类型(Integer String) 类型(HUMAN GUILD GLOBAL) 变量值
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questActionInfo"></param>
        private void ActionOfVar(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var sName = string.Empty;
            const string sVarFound = "变量{0}已存在，变量类型:{1}";
            const string sVarTypeError = "变量类型错误，错误类型:{0} 当前支持类型(HUMAN、GUILD、GLOBAL)";
            var sType = questActionInfo.sParam2;
            var sVarName = questActionInfo.sParam3;
            var sVarValue = questActionInfo.sParam4;
            var nVarValue = HUtil32.StrToInt(questActionInfo.sParam4, 0);
            var varType = VarType.None;
            if (string.Compare(questActionInfo.sParam1, "Integer", StringComparison.OrdinalIgnoreCase) == 0)
            {
                varType = VarType.Integer;
            }
            if (string.Compare(questActionInfo.sParam1, "String", StringComparison.OrdinalIgnoreCase) == 0)
            {
                varType = VarType.String;
            }
            if (string.IsNullOrEmpty(sType) || string.IsNullOrEmpty(sVarName) || varType == VarType.None)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.Var);
                return;
            }
            if (string.Compare(questActionInfo.sParam1, "Integer", StringComparison.OrdinalIgnoreCase) == 0)
            {
                varType = VarType.Integer;
            }
            if (string.Compare(questActionInfo.sParam1, "String", StringComparison.OrdinalIgnoreCase) == 0)
            {
                varType = VarType.String;
            }
            if (string.IsNullOrEmpty(sType) || string.IsNullOrEmpty(sVarName) || varType == VarType.None)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.Var);
                return;
            }
            var dynamicVar = new DynamicVar();
            dynamicVar.sName = sVarName;
            dynamicVar.VarType = varType;
            dynamicVar.nInternet = nVarValue;
            dynamicVar.sString = sVarValue;
            var boFoundVar = false;
            var dynamicVarList = NormNpc.GetDynamicVarMap(playObject, sType, ref sName);
            if (dynamicVarList == null)
            {
                Dispose(dynamicVar);
                ScriptActionError(playObject, string.Format(sVarTypeError, sType), questActionInfo, ExecutionCode.Var);
                return;
            }
            if (dynamicVarList.ContainsKey(sVarName))
            {
                boFoundVar = true;
            }
            if (!boFoundVar)
            {
                dynamicVarList.Add(sVarName, dynamicVar);
            }
            else
            {
                ScriptActionError(playObject, string.Format(sVarFound, sVarName, sType), questActionInfo, ExecutionCode.Var);
            }
        }

        /// <summary>
        /// 读取变量值
        /// LOADVAR 变量类型 变量名 文件名
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questActionInfo"></param>
        private void ActionOfLoadVar(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var sName = string.Empty;
            const string sVarFound = "变量{0}不存在，变量类型:{1}";
            const string sVarTypeError = "变量类型错误，错误类型:{0} 当前支持类型(HUMAN、GUILD、GLOBAL)";
            var sType = questActionInfo.sParam1;
            var sVarName = questActionInfo.sParam2;
            var sFileName = GetLineVariableText(playObject, questActionInfo.sParam3);
            if (sFileName[0] == '\\')
            {
                sFileName = sFileName[1..];
            }
            if (sFileName[1] == '\\')
            {
                sFileName = sFileName[2..];
            }
            if (sFileName[2] == '\\')
            {
                sFileName = sFileName[3..];
            }
            sFileName = M2Share.GetEnvirFilePath(sFileName);
            if (string.IsNullOrEmpty(sType) || string.IsNullOrEmpty(sVarName) || !File.Exists(sFileName))
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.LoadVar);
                return;
            }
            var boFoundVar = false;
            var dynamicVarList = NormNpc.GetDynamicVarMap(playObject, sType, ref sName);
            if (dynamicVarList == null)
            {
                ScriptActionError(playObject, string.Format(sVarTypeError, sType), questActionInfo, ExecutionCode.Var);
                return;
            }
            if (dynamicVarList.TryGetValue(sVarName, out _))
            {
                //IniFile = new ConfFile(sFileName);
                //IniFile.Load();
                /*switch (DynamicVar.VarType)
                {
                    case VarType.Integer:
                        DynamicVar.nInternet = IniFile.ReadWriteInteger(sName, DynamicVar.sName, 0);
                        break;
                    case VarType.String:
                        DynamicVar.sString = IniFile.ReadWriteString(sName, DynamicVar.sName, "");
                        break;
                }*/
                boFoundVar = true;
            }
            else
            {
                var iniFile = new ConfFile(sFileName);
                iniFile.Load();
                var str = iniFile.ReadString(sName, sVarName, "");
                if (!string.IsNullOrEmpty(str))
                {
                    if (!dynamicVarList.ContainsKey(sVarName))
                    {
                        dynamicVarList.Add(sVarName, new DynamicVar()
                        {
                            sName = sVarName,
                            sString = str,
                            VarType = VarType.String
                        });
                    }
                    boFoundVar = true;
                }
            }
            if (!boFoundVar)
            {
                ScriptActionError(playObject, string.Format(sVarFound, sVarName, sType), questActionInfo, ExecutionCode.LoadVar);
            }
        }

        private void ActionOfClearNeedItems(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nNeed = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            if (nNeed < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.ClearNeedItems);
                return;
            }
            StdItem stdItem;
            UserItem userItem;
            for (var i = playObject.ItemList.Count - 1; i >= 0; i--)
            {
                userItem = playObject.ItemList[i];
                stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (stdItem != null && stdItem.Need == nNeed)
                {
                    playObject.SendDelItems(userItem);
                    Dispose(userItem);
                    playObject.ItemList.RemoveAt(i);
                }
            }
            for (var i = playObject.StorageItemList.Count - 1; i >= 0; i--)
            {
                userItem = playObject.StorageItemList[i];
                stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (stdItem != null && stdItem.Need == nNeed)
                {
                    Dispose(userItem);
                    playObject.StorageItemList.RemoveAt(i);
                }
            }
        }

        private void ActionOfClearMakeItems(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            UserItem userItem;
            var sItemName = questActionInfo.sParam1;
            var nMakeIndex = questActionInfo.nParam2;
            var boMatchName = questActionInfo.sParam3 == "1";
            if (string.IsNullOrEmpty(sItemName) || nMakeIndex <= 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.ClearMakeItems);
                return;
            }
            StdItem stdItem;
            for (var i = playObject.ItemList.Count - 1; i >= 0; i--)
            {
                userItem = playObject.ItemList[i];
                if (userItem.MakeIndex != nMakeIndex)
                {
                    continue;
                }
                stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (!boMatchName || stdItem != null && string.Compare(stdItem.Name, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    playObject.SendDelItems(userItem);
                    Dispose(userItem);
                    playObject.ItemList.RemoveAt(i);
                }
            }
            for (var i = playObject.StorageItemList.Count - 1; i >= 0; i--)
            {
                userItem = playObject.ItemList[i];
                if (userItem.MakeIndex != nMakeIndex)
                {
                    continue;
                }
                stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (!boMatchName || stdItem != null && string.Compare(stdItem.Name, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    Dispose(userItem);
                    playObject.StorageItemList.RemoveAt(i);
                }
            }
            for (var i = 0; i < playObject.UseItems.Length; i++)
            {
                userItem = playObject.UseItems[i];
                if (userItem.MakeIndex != nMakeIndex)
                {
                    continue;
                }
                stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (!boMatchName || stdItem != null && string.Compare(stdItem.Name, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    userItem.Index = 0;
                }
            }
        }

        private void ActionOfUnMaster(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            if (string.IsNullOrEmpty(playObject.MasterName))
            {
                normNpc.GotoLable(playObject, "@ExeMasterFail", false);
                return;
            }
            var poseHuman = (PlayObject)playObject.GetPoseCreate();
            if (poseHuman == null)
            {
                normNpc.GotoLable(playObject, "@UnMasterCheckDir", false);
            }
            if (poseHuman != null)
            {
                if ((string.IsNullOrEmpty(questActionInfo.sParam1)))
                {
                    if (poseHuman.Race != ActorRace.Play)
                    {
                        normNpc.GotoLable(playObject, "@UnMasterTypeErr", false);
                        return;
                    }
                    if (poseHuman.GetPoseCreate() == playObject)
                    {
                        if (playObject.MasterName == poseHuman.ChrName)
                        {
                            if (playObject.IsMaster)
                            {
                                normNpc.GotoLable(playObject, "@UnIsMaster", false);
                                return;
                            }
                            if (playObject.MasterName != poseHuman.ChrName)
                            {
                                normNpc.GotoLable(playObject, "@UnMasterError", false);
                                return;
                            }
                            normNpc.GotoLable(playObject, "@StartUnMaster", false);
                            normNpc.GotoLable(poseHuman, "@WateUnMaster", false);
                            return;
                        }
                    }
                }
            }
            // sREQUESTUNMARRY
            if (string.Compare(questActionInfo.sParam1, "REQUESTUNMASTER", StringComparison.OrdinalIgnoreCase) == 0)
            {
                string sMsg;
                if (string.IsNullOrEmpty(questActionInfo.sParam2))
                {
                    if (poseHuman != null)
                    {
                        playObject.IsStartUnMaster = true;
                        if (playObject.IsStartUnMaster && poseHuman.IsStartUnMaster)
                        {
                            sMsg = string.Format(Settings.NPCSayUnMasterOKMsg, normNpc.ChrName, playObject.ChrName, poseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sMsg, MsgType.Say);
                            playObject.MasterName = "";
                            poseHuman.MasterName = "";
                            playObject.IsStartUnMaster = false;
                            poseHuman.IsStartUnMaster = false;
                            playObject.RefShowName();
                            poseHuman.RefShowName();
                            normNpc.GotoLable(playObject, "@UnMasterEnd", false);
                            normNpc.GotoLable(poseHuman, "@UnMasterEnd", false);
                        }
                        else
                        {
                            normNpc.GotoLable(playObject, "@WateUnMaster", false);
                            normNpc.GotoLable(poseHuman, "@RevUnMaster", false);
                        }
                    }
                    return;
                }
                // 强行出师
                if (string.Compare(questActionInfo.sParam2, "FORCE", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    sMsg = string.Format(Settings.NPCSayForceUnMasterMsg, normNpc.ChrName, playObject.ChrName, playObject.MasterName);
                    M2Share.WorldEngine.SendBroadCastMsg(sMsg, MsgType.Say);
                    poseHuman = M2Share.WorldEngine.GetPlayObject(playObject.MasterName);
                    if (poseHuman != null)
                    {
                        poseHuman.MasterName = "";
                        poseHuman.RefShowName();
                    }
                    else
                    {
                        M2Share.UnForceMasterList.Add(playObject.MasterName);
                        M2Share.SaveUnForceMasterList();
                    }
                    playObject.MasterName = "";
                    normNpc.GotoLable(playObject, "@UnMasterEnd", false);
                    playObject.RefShowName();
                }
            }
        }

        private void ActionOfSetMapMode(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var sMapName = questActionInfo.sParam1;
            var sMapMode = questActionInfo.sParam2;
            var sParam1 = questActionInfo.sParam3;
            var sParam2 = questActionInfo.sParam4;
            var envir = M2Share.MapMgr.FindMap(sMapName);
            if (envir == null || string.IsNullOrEmpty(sMapMode))
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.SetMapMode);
                return;
            }
            if (string.Compare(sMapMode, "SAFE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.SafeArea = true;
                }
                else
                {
                    envir.Flag.SafeArea = false;
                }
            }
            else if (string.Compare(sMapMode, "DARK", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boDarkness = true;
                }
                else
                {
                    envir.Flag.boDarkness = false;
                }
            }
            else if (string.Compare(sMapMode, "FIGHT", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.FightZone = true;
                }
                else
                {
                    envir.Flag.FightZone = false;
                }
            }
            else if (string.Compare(sMapMode, "FIGHT3", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.Fight3Zone = true;
                }
                else
                {
                    envir.Flag.Fight3Zone = false;
                }
            }
            else if (string.Compare(sMapMode, "DAY", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.DayLight = true;
                }
                else
                {
                    envir.Flag.DayLight = false;
                }
            }
            else if (string.Compare(sMapMode, "QUIZ", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boQUIZ = true;
                }
                else
                {
                    envir.Flag.boQUIZ = false;
                }
            }
            else if (string.Compare(sMapMode, "NORECONNECT", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boNORECONNECT = true;
                    envir.Flag.sNoReConnectMap = sParam1;
                }
                else
                {
                    envir.Flag.boNORECONNECT = false;
                }
            }
            else if (string.Compare(sMapMode, "MUSIC", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.Music = true;
                    envir.Flag.MusicId = HUtil32.StrToInt(sParam1, -1);
                }
                else
                {
                    envir.Flag.Music = false;
                }
            }
            else if (string.Compare(sMapMode, "EXPRATE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boEXPRATE = true;
                    envir.Flag.ExpRate = HUtil32.StrToInt(sParam1, -1);
                }
                else
                {
                    envir.Flag.boEXPRATE = false;
                }
            }
            else if (string.Compare(sMapMode, "PKWINLEVEL", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boPKWINLEVEL = true;
                    envir.Flag.nPKWINLEVEL = HUtil32.StrToInt(sParam1, -1);
                }
                else
                {
                    envir.Flag.boPKWINLEVEL = false;
                }
            }
            else if (string.Compare(sMapMode, "PKWINEXP", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boPKWINEXP = true;
                    envir.Flag.nPKWINEXP = HUtil32.StrToInt(sParam1, -1);
                }
                else
                {
                    envir.Flag.boPKWINEXP = false;
                }
            }
            else if (string.Compare(sMapMode, "PKLOSTLEVEL", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boPKLOSTLEVEL = true;
                    envir.Flag.nPKLOSTLEVEL = HUtil32.StrToInt(sParam1, -1);
                }
                else
                {
                    envir.Flag.boPKLOSTLEVEL = false;
                }
            }
            else if (string.Compare(sMapMode, "PKLOSTEXP", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boPKLOSTEXP = true;
                    envir.Flag.nPKLOSTEXP = HUtil32.StrToInt(sParam1, -1);
                }
                else
                {
                    envir.Flag.boPKLOSTEXP = false;
                }
            }
            else if (string.Compare(sMapMode, "DECHP", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if ((!string.IsNullOrEmpty(sParam1)) && (!string.IsNullOrEmpty(sParam2)))
                {
                    envir.Flag.boDECHP = true;
                    envir.Flag.nDECHPTIME = HUtil32.StrToInt(sParam1, -1);
                    envir.Flag.nDECHPPOINT = HUtil32.StrToInt(sParam2, -1);
                }
                else
                {
                    envir.Flag.boDECHP = false;
                }
            }
            else if (string.Compare(sMapMode, "DECGAMEGOLD", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if ((!string.IsNullOrEmpty(sParam1)) && (!string.IsNullOrEmpty(sParam2)))
                {
                    envir.Flag.boDECGAMEGOLD = true;
                    envir.Flag.nDECGAMEGOLDTIME = HUtil32.StrToInt(sParam1, -1);
                    envir.Flag.nDECGAMEGOLD = HUtil32.StrToInt(sParam2, -1);
                }
                else
                {
                    envir.Flag.boDECGAMEGOLD = false;
                }
            }
            else if (string.Compare(sMapMode, "RUNHUMAN", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.RunHuman = true;
                }
                else
                {
                    envir.Flag.RunHuman = false;
                }
            }
            else if (string.Compare(sMapMode, "RUNMON", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.RunMon = true;
                }
                else
                {
                    envir.Flag.RunMon = false;
                }
            }
            else if (string.Compare(sMapMode, "NEEDHOLE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boNEEDHOLE = true;
                }
                else
                {
                    envir.Flag.boNEEDHOLE = false;
                }
            }
            else if (string.Compare(sMapMode, "NORECALL", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.NoReCall = true;
                }
                else
                {
                    envir.Flag.NoReCall = false;
                }
            }
            else if (string.Compare(sMapMode, "NOGUILDRECALL", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.NoGuildReCall = true;
                }
                else
                {
                    envir.Flag.NoGuildReCall = false;
                }
            }
            else if (string.Compare(sMapMode, "NODEARRECALL", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boNODEARRECALL = true;
                }
                else
                {
                    envir.Flag.boNODEARRECALL = false;
                }
            }
            else if (string.Compare(sMapMode, "NOMASTERRECALL", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.MasterReCall = true;
                }
                else
                {
                    envir.Flag.MasterReCall = false;
                }
            }
            else if (string.Compare(sMapMode, "NORANDOMMOVE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boNORANDOMMOVE = true;
                }
                else
                {
                    envir.Flag.boNORANDOMMOVE = false;
                }
            }
            else if (string.Compare(sMapMode, "NODRUG", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boNODRUG = true;
                }
                else
                {
                    envir.Flag.boNODRUG = false;
                }
            }
            else if (string.Compare(sMapMode, "MINE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.Mine = true;
                }
                else
                {
                    envir.Flag.Mine = false;
                }
            }
            else if (string.Compare(sMapMode, "MINE2", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boMINE2 = true;
                }
                else
                {
                    envir.Flag.boMINE2 = false;
                }
            }
            else if (string.Compare(sMapMode, "NOTHROWITEM", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.NoThrowItem = true;
                }
                else
                {
                    envir.Flag.NoThrowItem = false;
                }
            }
            else if (string.Compare(sMapMode, "NODROPITEM", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.NoDropItem = true;
                }
                else
                {
                    envir.Flag.NoDropItem = false;
                }
            }
            else if (string.Compare(sMapMode, "NOPOSITIONMOVE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boNOPOSITIONMOVE = true;
                }
                else
                {
                    envir.Flag.boNOPOSITIONMOVE = false;
                }
            }
            else if (string.Compare(sMapMode, "NOHORSE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.NoHorse = true;
                }
                else
                {
                    envir.Flag.NoHorse = false;
                }
            }
            else if (string.Compare(sMapMode, "NOCHAT", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boNOCHAT = true;
                }
                else
                {
                    envir.Flag.boNOCHAT = false;
                }
            }
        }

        private void ActionOfSetMemberLevel(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nLevel = (byte)HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.SetMemberLevel);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    playObject.MemberLevel = nLevel;
                    break;
                case '-':
                    playObject.MemberLevel -= nLevel;
                    if (playObject.MemberLevel <= 0)
                    {
                        playObject.MemberLevel = 0;
                    }
                    break;
                case '+':
                    playObject.MemberLevel += nLevel;
                    if (playObject.MemberLevel >= byte.MaxValue)
                    {
                        playObject.MemberLevel = 255;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(string.Format(Settings.ChangeMemberLevelMsg, playObject.MemberLevel), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfSetMemberType(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nType = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nType < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.SetMemberType);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    playObject.MemberType = nType;
                    break;
                case '-':
                    playObject.MemberType -= nType;
                    if (playObject.MemberType < 0)
                    {
                        playObject.MemberType = 0;
                    }
                    break;
                case '+':
                    playObject.MemberType += nType;
                    if (playObject.MemberType > 65535)
                    {
                        playObject.MemberType = 65535;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(string.Format(Settings.ChangeMemberTypeMsg, playObject.MemberType), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfGiveItem(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var sItemName = questActionInfo.sParam1;
            var nItemCount = questActionInfo.nParam2;
            if (string.IsNullOrEmpty(sItemName) || nItemCount <= 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.Give);
                return;
            }
            if (string.Compare(sItemName, Grobal2.StringGoldName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                playObject.IncGold(nItemCount);
                playObject.GoldChanged();
                if (M2Share.GameLogGold)
                {
                    M2Share.EventSource.AddEventLog(9, playObject.MapName + "\t" + playObject.CurrX + "\t" + playObject.CurrY + "\t" + playObject.ChrName + "\t" + Grobal2.StringGoldName + "\t" + nItemCount + "\t" + '1' + "\t" + normNpc.ChrName);
                }
                return;
            }
            if (M2Share.WorldEngine.GetStdItemIdx(sItemName) > 0)
            {
                if (!(nItemCount >= 1 && nItemCount <= 50))
                {
                    nItemCount = 1;
                }
                for (var i = 0; i < nItemCount; i++)
                {
                    StdItem stdItem;
                    // nItemCount 为0时出死循环
                    UserItem userItem;
                    if (playObject.IsEnoughBag())
                    {
                        userItem = new UserItem();
                        if (M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref userItem))
                        {
                            playObject.ItemList.Add(userItem);
                            playObject.SendAddItem(userItem);
                            stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                            if (stdItem.NeedIdentify == 1)
                            {
                                M2Share.EventSource.AddEventLog(9, playObject.MapName + "\t" + playObject.CurrX + "\t" + playObject.CurrY + "\t" + playObject.ChrName + "\t" + sItemName + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + normNpc.ChrName);
                            }
                        }
                        else
                        {
                            Dispose(userItem);
                        }
                    }
                    else
                    {
                        userItem = new UserItem();
                        if (M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref userItem))
                        {
                            stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                            if (stdItem.NeedIdentify == 1)
                            {
                                M2Share.EventSource.AddEventLog(9, playObject.MapName + "\t" + playObject.CurrX + "\t" + playObject.CurrY + "\t" + playObject.ChrName + "\t" + sItemName + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + normNpc.ChrName);
                            }
                            playObject.DropItemDown(userItem, 3, false, playObject.ActorId, 0);
                        }
                        Dispose(userItem);
                    }
                }
            }
        }

        private void ActionOfGmExecute(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var sParam1 = questActionInfo.sParam1;
            var sParam2 = questActionInfo.sParam2;
            var sParam3 = questActionInfo.sParam3;
            var sParam4 = questActionInfo.sParam4;
            var sParam5 = questActionInfo.sParam5;
            var sParam6 = questActionInfo.sParam6;
            if (string.Compare(sParam2, "Self", StringComparison.OrdinalIgnoreCase) == 0)
            {
                sParam2 = playObject.ChrName;
            }
            var sData = string.Format("@{0} {1} {2} {3} {4} {5}", sParam1, sParam2, sParam3, sParam4, sParam5, sParam6);
            var btOldPermission = playObject.Permission;
            try
            {
                playObject.Permission = 10;
                playObject.ProcessUserLineMsg(sData);
            }
            finally
            {
                playObject.Permission = btOldPermission;
            }
        }

        private void ActionOfGuildAuraePoint(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nAuraePoint = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nAuraePoint < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.AuraePoint);
                return;
            }
            if (playObject.MyGuild == null)
            {
                playObject.SysMsg(Settings.ScriptGuildAuraePointNoGuild, MsgColor.Red, MsgType.Hint);
                return;
            }
            var guild = playObject.MyGuild;
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    guild.Aurae = nAuraePoint;
                    break;
                case '-':
                    if (guild.Aurae >= nAuraePoint)
                    {
                        guild.Aurae = guild.Aurae - nAuraePoint;
                    }
                    else
                    {
                        guild.Aurae = 0;
                    }
                    break;
                case '+':
                    if (int.MaxValue - guild.Aurae >= nAuraePoint)
                    {
                        guild.Aurae = guild.Aurae + nAuraePoint;
                    }
                    else
                    {
                        guild.Aurae = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(string.Format(Settings.ScriptGuildAuraePointMsg, new[] { guild.Aurae }), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfGuildBuildPoint(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nBuildPoint = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nBuildPoint < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.BuildPoint);
                return;
            }
            if (playObject.MyGuild == null)
            {
                playObject.SysMsg(Settings.ScriptGuildBuildPointNoGuild, MsgColor.Red, MsgType.Hint);
                return;
            }
            var guild = playObject.MyGuild;
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    guild.BuildPoint = nBuildPoint;
                    break;
                case '-':
                    if (guild.BuildPoint >= nBuildPoint)
                    {
                        guild.BuildPoint = guild.BuildPoint - nBuildPoint;
                    }
                    else
                    {
                        guild.BuildPoint = 0;
                    }
                    break;
                case '+':
                    if (int.MaxValue - guild.BuildPoint >= nBuildPoint)
                    {
                        guild.BuildPoint = guild.BuildPoint + nBuildPoint;
                    }
                    else
                    {
                        guild.BuildPoint = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(string.Format(Settings.ScriptGuildBuildPointMsg, guild.BuildPoint), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfGuildChiefItemCount(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nItemCount = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nItemCount < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.GuildChiefItemCount);
                return;
            }
            if (playObject.MyGuild == null)
            {
                playObject.SysMsg(Settings.ScriptGuildFlourishPointNoGuild, MsgColor.Red, MsgType.Hint);
                return;
            }
            var guild = playObject.MyGuild;
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    guild.ChiefItemCount = nItemCount;
                    break;
                case '-':
                    if (guild.ChiefItemCount >= nItemCount)
                    {
                        guild.ChiefItemCount = guild.ChiefItemCount - nItemCount;
                    }
                    else
                    {
                        guild.ChiefItemCount = 0;
                    }
                    break;
                case '+':
                    if (int.MaxValue - guild.ChiefItemCount >= nItemCount)
                    {
                        guild.ChiefItemCount = guild.ChiefItemCount + nItemCount;
                    }
                    else
                    {
                        guild.ChiefItemCount = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(string.Format(Settings.ScriptChiefItemCountMsg, guild.ChiefItemCount), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfGuildFlourishPoint(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nFlourishPoint = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nFlourishPoint < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.FlourishPoint);
                return;
            }
            if (playObject.MyGuild == null)
            {
                playObject.SysMsg(Settings.ScriptGuildFlourishPointNoGuild, MsgColor.Red, MsgType.Hint);
                return;
            }
            var guild = playObject.MyGuild;
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    guild.Flourishing = nFlourishPoint;
                    break;
                case '-':
                    if (guild.Flourishing >= nFlourishPoint)
                    {
                        guild.Flourishing = guild.Flourishing - nFlourishPoint;
                    }
                    else
                    {
                        guild.Flourishing = 0;
                    }
                    break;
                case '+':
                    if (int.MaxValue - guild.Flourishing >= nFlourishPoint)
                    {
                        guild.Flourishing = guild.Flourishing + nFlourishPoint;
                    }
                    else
                    {
                        guild.Flourishing = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(string.Format(Settings.ScriptGuildFlourishPointMsg, guild.Flourishing), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfGuildstabilityPoint(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nStabilityPoint = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nStabilityPoint < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.StabilityPoint);
                return;
            }
            if (playObject.MyGuild == null)
            {
                playObject.SysMsg(Settings.ScriptGuildStabilityPointNoGuild, MsgColor.Red, MsgType.Hint);
                return;
            }
            var guild = playObject.MyGuild;
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    guild.Stability = nStabilityPoint;
                    break;
                case '-':
                    if (guild.Stability >= nStabilityPoint)
                    {
                        guild.Stability = guild.Stability - nStabilityPoint;
                    }
                    else
                    {
                        guild.Stability = 0;
                    }
                    break;
                case '+':
                    if (int.MaxValue - guild.Stability >= nStabilityPoint)
                    {
                        guild.Stability = guild.Stability + nStabilityPoint;
                    }
                    else
                    {
                        guild.Stability = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(string.Format(Settings.ScriptGuildStabilityPointMsg, guild.Stability), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfHumanHp(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nHp = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nHp < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.HumanHp);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    playObject.WAbil.HP = (ushort)nHp;
                    break;
                case '-':
                    if (playObject.WAbil.HP >= nHp)
                    {
                        playObject.WAbil.HP -= (ushort)nHp;
                    }
                    else
                    {
                        playObject.WAbil.HP = 0;
                    }
                    break;
                case '+':
                    playObject.WAbil.HP += (ushort)nHp;
                    if (playObject.WAbil.HP > playObject.WAbil.MaxHP)
                    {
                        playObject.WAbil.HP = playObject.WAbil.MaxHP;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(string.Format(Settings.ScriptChangeHumanHPMsg, playObject.WAbil.MaxHP), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfHumanMp(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nMp = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nMp < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.HumanMp);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    playObject.WAbil.MP = (ushort)nMp;
                    break;
                case '-':
                    if (playObject.WAbil.MP >= nMp)
                    {
                        playObject.WAbil.MP -= (ushort)nMp;
                    }
                    else
                    {
                        playObject.WAbil.MP = 0;
                    }
                    break;
                case '+':
                    playObject.WAbil.MP += (ushort)nMp;
                    if (playObject.WAbil.MP > playObject.WAbil.MaxMP)
                    {
                        playObject.WAbil.MP = playObject.WAbil.MaxMP;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(string.Format(Settings.ScriptChangeHumanMPMsg, new[] { playObject.WAbil.MaxMP }), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfKick(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            playObject.BoKickFlag = true;
            playObject.BoReconnection = true;
            playObject.BoSoftClose = true;
        }

        private void ActionOfKill(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nMode = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            if (nMode >= 0 && nMode <= 3)
            {
                switch (nMode)
                {
                    case 1:
                        playObject.NoItem = true;
                        playObject.Die();
                        break;
                    case 2:
                        playObject.SetLastHiter(normNpc);
                        playObject.Die();
                        break;
                    case 3:
                        playObject.NoItem = true;
                        playObject.SetLastHiter(normNpc);
                        playObject.Die();
                        break;
                    default:
                        playObject.Die();
                        break;
                }
            }
            else
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.Kill);
            }
        }

        private void ActionOfBonusPoint(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nBonusPoint = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nBonusPoint < 0 || nBonusPoint > 10000)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.BonusPoint);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    playObject.HasLevelUp(0);
                    playObject.BonusPoint = nBonusPoint;
                    playObject.SendMsg(Messages.RM_ADJUST_BONUS, 0, 0, 0, 0);
                    break;
                case '-':
                    break;
                case '+':
                    playObject.BonusPoint += nBonusPoint;
                    playObject.SendMsg(Messages.RM_ADJUST_BONUS, 0, 0, 0, 0);
                    break;
            }
        }

        private void ActionOfDelMarry(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            playObject.DearName = "";
            playObject.RefShowName();
        }

        private void ActionOfDelMaster(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            playObject.MasterName = "";
            playObject.RefShowName();
        }

        private void ActionOfRestBonusPoint(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nTotleUsePoint = playObject.BonusAbil.DC + playObject.BonusAbil.MC + playObject.BonusAbil.SC + playObject.BonusAbil.AC + playObject.BonusAbil.MAC + playObject.BonusAbil.HP + playObject.BonusAbil.MP + playObject.BonusAbil.Hit + playObject.BonusAbil.Speed + playObject.BonusAbil.Reserved;
            playObject.BonusPoint += nTotleUsePoint;
            playObject.SendMsg(Messages.RM_ADJUST_BONUS, 0, 0, 0, 0);
            playObject.HasLevelUp(0);
            playObject.SysMsg("分配点数已复位!!!", MsgColor.Red, MsgType.Hint);
        }

        private void ActionOfRestReNewLevel(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            playObject.ReLevel = 0;
            playObject.HasLevelUp(0);
        }

        private void ActionOfChangeNameColor(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nColor = questActionInfo.nParam1;
            if (nColor < 0 || nColor > 255)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.ChangeNameColor);
                return;
            }
            playObject.NameColor = (byte)nColor;
            playObject.RefNameColor();
        }

        private void ActionOfClearPassword(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            playObject.StoragePwd = "";
            playObject.IsPasswordLocked = false;
        }

        // RECALLMOB 怪物名称 等级 叛变时间 变色(0,1) 固定颜色(1 - 7)
        // 变色为0 时固定颜色才起作用
        private void ActionOfRecallmob(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            BaseObject mon;
            if (questActionInfo.nParam3 <= 1)
            {
                mon = playObject.MakeSlave(questActionInfo.sParam1, 3, HUtil32.StrToInt(questActionInfo.sParam2, 0), 100, 10 * 24 * 60 * 60);
            }
            else
            {
                mon = playObject.MakeSlave(questActionInfo.sParam1, 3, HUtil32.StrToInt(questActionInfo.sParam2, 0), 100, questActionInfo.nParam3 * 60);
            }
            if (mon != null)
            {
                if (!string.IsNullOrEmpty(questActionInfo.sParam4) && questActionInfo.sParam4[1] == '1')
                {
                    mon.AutoChangeColor = true;
                }
                else if (questActionInfo.nParam5 > 0)
                {
                    mon.FixColor = true;
                    mon.FixColorIdx = (byte)(questActionInfo.nParam5 - 1);
                }
            }
        }

        private void ActionOfReNewLevel(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nReLevel = (byte)HUtil32.StrToInt(questActionInfo.sParam1, -1);
            var nLevel = (byte)HUtil32.StrToInt(questActionInfo.sParam2, -1);
            var nBounsuPoint = HUtil32.StrToInt(questActionInfo.sParam3, -1);
            if (nReLevel < 0 || nLevel < 0 || nBounsuPoint < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.Renewlevel);
                return;
            }
            if (playObject.ReLevel + nReLevel <= 255)
            {
                playObject.ReLevel += nReLevel;
                if (nLevel > 0)
                {
                    playObject.Abil.Level = nLevel;
                }
                if (M2Share.Config.ReNewLevelClearExp)
                {
                    playObject.Abil.Exp = 0;
                }
                playObject.BonusPoint += nBounsuPoint;
                playObject.SendMsg(Messages.RM_ADJUST_BONUS, 0, 0, 0, 0);
                playObject.HasLevelUp(0);
                playObject.RefShowName();
            }
        }

        private void ActionOfChangeGender(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nGender = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            if (nGender > 1)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.ChangeGender);
                return;
            }
            playObject.Gender = Enum.Parse<PlayGender>(nGender.ToString());
            playObject.FeatureChanged();
        }

        private void ActionOfKillSlave(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            for (var i = 0; i < playObject.SlaveList.Count; i++)
            {
                var slave = playObject.SlaveList[i];
                slave.WAbil.HP = 0;
            }
        }

        private void ActionOfKillMonExpRate(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nRate = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            var nTime = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nRate < 0 || nTime < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.KillMonExpRate);
                return;
            }
            playObject.KillMonExpRate = nRate;
            playObject.KillMonExpRateTime = nTime;
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(string.Format(Settings.ChangeKillMonExpRateMsg, playObject.KillMonExpRate / 100, playObject.KillMonExpRateTime), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfMonGenEx(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var sMapName = questActionInfo.sParam1;
            var nMapX = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            var nMapY = HUtil32.StrToInt(questActionInfo.sParam3, -1);
            var sMonName = questActionInfo.sParam4;
            var nRange = questActionInfo.nParam5;
            var nCount = questActionInfo.nParam6;
            if (string.IsNullOrEmpty(sMapName) || nMapX <= 0 || nMapY <= 0 || string.IsNullOrEmpty(sMapName) || nRange <= 0 || nCount <= 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.MonGenex);
                return;
            }
            for (var i = 0; i < nCount; i++)
            {
                var nRandX = (short)(M2Share.RandomNumber.Random(nRange * 2 + 1) + (nMapX - nRange));
                var nRandY = (short)(M2Share.RandomNumber.Random(nRange * 2 + 1) + (nMapY - nRange));
                if (M2Share.WorldEngine.RegenMonsterByName(sMapName, nRandX, nRandY, sMonName) == null)
                {
                    break;
                }
            }
        }

        private void ActionOfOpenMagicBox(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            short nX = 0;
            short nY = 0;
            var sMonName = questActionInfo.sParam1;
            if (string.IsNullOrEmpty(sMonName))
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.OpenMagicbox);
                return;
            }
            playObject.GetFrontPosition(ref nX, ref nY);
            var monster = M2Share.WorldEngine.RegenMonsterByName(playObject.Envir.MapName, nX, nY, sMonName);
            if (monster == null)
            {
                return;
            }
            monster.Die();
        }

        private void ActionOfPkZone(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            FireBurnEvent fireBurnEvent;
            var nRange = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            var nType = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            var nTime = HUtil32.StrToInt(questActionInfo.sParam3, -1);
            var nPoint = HUtil32.StrToInt(questActionInfo.sParam4, -1);
            if (nRange < 0 || nType < 0 || nTime < 0 || nPoint < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.PvpZone);
                return;
            }
            var nMinX = normNpc.CurrX - nRange;
            var nMaxX = normNpc.CurrX + nRange;
            var nMinY = normNpc.CurrY - nRange;
            var nMaxY = normNpc.CurrY + nRange;
            for (var nX = nMinX; nX <= nMaxX; nX++)
            {
                for (var nY = nMinY; nY <= nMaxY; nY++)
                {
                    if (nX < nMaxX && nY == nMinY || nY < nMaxY && nX == nMinX || nX == nMaxX ||
                        nY == nMaxY)
                    {
                        fireBurnEvent = new FireBurnEvent(playObject, (short)nX, (short)nY, (byte)nType, nTime * 1000, nPoint);
                        M2Share.EventMgr.AddEvent(fireBurnEvent);
                    }
                }
            }
        }

        private void ActionOfPowerRate(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nRate = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            var nTime = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nRate < 0 || nTime < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.PowerRate);
                return;
            }
            playObject.PowerRate = nRate;
            playObject.PowerRateTime = nTime;
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(string.Format(Settings.ChangePowerRateMsg, playObject.PowerRate / 100, playObject.PowerRateTime), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfChangeMode(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            //switch (questActionInfo.nParam1)
            //{
            //    case 1:
            //        CommandMgr.Execute(playObject, "ChangeAdminMode");
            //        break;
            //    case 2:
            //        CommandMgr.Execute(playObject, "ChangeSuperManMode");
            //        break;
            //    case 3:
            //        CommandMgr.Execute(playObject, "ChangeObMode");
            //        break;
            //    default:
            //        ScriptActionError(playObject, "", questActionInfo, ExecutionCodeDef.CHANGEMODE);
            //        break;
            //}
            var nMode = questActionInfo.nParam1;
            var boOpen = HUtil32.StrToInt(questActionInfo.sParam2, -1) == 1;
            if (nMode >= 1 && nMode <= 3)
            {
                switch (nMode)
                {
                    case 1:
                        playObject.AdminMode = boOpen;
                        if (playObject.AdminMode)
                        {
                            playObject.SysMsg(Settings.GameMasterMode, MsgColor.Green, MsgType.Hint);
                        }
                        else
                        {
                            playObject.SysMsg(Settings.ReleaseGameMasterMode, MsgColor.Green, MsgType.Hint);
                        }
                        break;
                    case 2:
                        playObject.SuperMan = boOpen;
                        if (playObject.SuperMan)
                        {
                            playObject.SysMsg(Settings.SupermanMode, MsgColor.Green, MsgType.Hint);
                        }
                        else
                        {
                            playObject.SysMsg(Settings.ReleaseSupermanMode, MsgColor.Green, MsgType.Hint);
                        }
                        break;
                    case 3:
                        playObject.ObMode = boOpen;
                        if (playObject.ObMode)
                        {
                            playObject.SysMsg(Settings.ObserverMode, MsgColor.Green, MsgType.Hint);
                        }
                        else
                        {
                            playObject.SysMsg(Settings.ReleaseObserverMode, MsgColor.Green, MsgType.Hint);
                        }
                        break;
                }
            }
            else
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.ChangeMode);
            }
        }

        private void ActionOfChangePerMission(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var nPermission = (byte)HUtil32.StrToInt(questActionInfo.sParam1, -1);
            if (nPermission <= 10)
            {
                playObject.Permission = nPermission;
            }
            else
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.ChangePerMission);
                return;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(string.Format(Settings.ChangePermissionMsg, playObject.Permission), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfThrowitem(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            var sMap = string.Empty;
            var sItemName = string.Empty;
            var nX = 0;
            var nY = 0;
            var nRange = 0;
            var nCount = 0;
            var dX = 0;
            var dY = 0;
            UserItem userItem = null;
            try
            {
                if (!GetValValue(playObject, questActionInfo.sParam1, ref sMap))
                {
                    sMap = GetLineVariableText(playObject, questActionInfo.sParam1);
                }
                if (!GetValValue(playObject, questActionInfo.sParam2, ref nX))
                {
                    nX = HUtil32.StrToInt(GetLineVariableText(playObject, questActionInfo.sParam2), -1);
                }
                if (!GetValValue(playObject, questActionInfo.sParam3, ref nY))
                {
                    nY = HUtil32.StrToInt(GetLineVariableText(playObject, questActionInfo.sParam3), -1);
                }
                if (!GetValValue(playObject, questActionInfo.sParam4, ref nRange))
                {
                    nRange = HUtil32.StrToInt(GetLineVariableText(playObject, questActionInfo.sParam4), -1);
                }
                if (!GetValValue(playObject, questActionInfo.sParam5, ref sItemName))
                {
                    sItemName = GetLineVariableText(playObject, questActionInfo.sParam5);
                }
                if (!GetValValue(playObject, questActionInfo.sParam6, ref nCount))
                {
                    nCount = HUtil32.StrToInt(GetLineVariableText(playObject, questActionInfo.sParam6), -1);
                }
                if (string.IsNullOrEmpty(sMap) || nX < 0 || nY < 0 || nRange < 0 || string.IsNullOrEmpty(sItemName) || nCount <= 0)
                {
                    ScriptActionError(playObject, "", questActionInfo, ExecutionCode.ThrowItem);
                    return;
                }
                var envir = M2Share.MapMgr.FindMap(sMap);
                if (envir == null)
                {
                    return;
                }
                if (nCount <= 0)
                {
                    nCount = 1;
                }
                MapItem mapItem;
                if (string.Compare(sItemName, Grobal2.StringGoldName, StringComparison.OrdinalIgnoreCase) == 0)// 金币
                {
                    if (GetActionOfThrowitemDropPosition(envir, nX, nY, nRange, ref dX, ref dY))
                    {
                        mapItem = new MapItem();
                        mapItem.Name = Grobal2.StringGoldName;
                        mapItem.Count = nCount;
                        mapItem.Looks = M2Share.GetGoldShape(nCount);
                        mapItem.OfBaseObject = playObject.ActorId;
                        mapItem.CanPickUpTick = HUtil32.GetTickCount();
                        mapItem.DropBaseObject = playObject.ActorId;
                        if (envir.AddItemToMap(dX, dY, mapItem))
                        {
                            normNpc.SendRefMsg(Messages.RM_ITEMSHOW, mapItem.Looks, mapItem.ItemId, dX, dY, mapItem.Name + "@0");
                        }
                        else
                        {
                            Dispose(mapItem);
                        }
                        return;
                    }
                }
                for (var i = 0; i < nCount; i++)
                {
                    if (GetActionOfThrowitemDropPosition(envir, nX, nY, nRange, ref dX, ref dY)) // 修正出现在一个坐标上
                    {
                        if (M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref userItem))
                        {
                            var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                            if (stdItem != null)
                            {
                                if (stdItem.StdMode == 40)
                                {
                                    var idura = userItem.Dura - 2000;
                                    if (idura < 0)
                                    {
                                        idura = 0;
                                    }
                                    userItem.Dura = (ushort)idura;
                                }
                                mapItem = new MapItem();
                                mapItem.UserItem = new UserItem(userItem);
                                mapItem.Name = stdItem.Name;
                                var nameCorlr = "@" + CustomItem.GetItemAddValuePointColor(userItem); // 取自定义物品名称
                                if (userItem.Desc[13] == 1)
                                {
                                    var sUserItemName = M2Share.CustomItemMgr.GetCustomItemName(userItem.MakeIndex, userItem.Index);
                                    if (!string.IsNullOrEmpty(sUserItemName))
                                    {
                                        mapItem.Name = sUserItemName;
                                    }
                                }
                                mapItem.Looks = stdItem.Looks;
                                if (stdItem.StdMode == 45)
                                {
                                    mapItem.Looks = (ushort)M2Share.GetRandomLook(mapItem.Looks, stdItem.Shape);
                                }
                                mapItem.AniCount = stdItem.AniCount;
                                mapItem.Reserved = 0;
                                mapItem.Count = nCount;
                                mapItem.OfBaseObject = playObject.ActorId;
                                mapItem.CanPickUpTick = HUtil32.GetTickCount();
                                mapItem.DropBaseObject = playObject.ActorId;
                                // GetDropPosition(nX, nY, nRange, dx, dy);//取掉物的位置
                                if (envir.AddItemToMap(dX, dY, mapItem))
                                {
                                    normNpc.SendRefMsg(Messages.RM_ITEMSHOW, mapItem.Looks, mapItem.ItemId, dX, dY, mapItem.Name + nameCorlr);
                                }
                                else
                                {
                                    Dispose(mapItem);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Dispose(userItem);
                        }
                    }
                }
            }
            catch
            {
                M2Share.Logger.Error("{异常} TNormNpc.ActionOfTHROWITEM");
            }
        }

        private static bool GetActionOfThrowitemDropPosition(Envirnoment envir, int nOrgX, int nOrgY, int nRange, ref int nDx, ref int nDy)
        {
            var nItemCount = 0;
            var n24 = 999;
            var result = false;
            var n28 = 0;
            var n2C = 0;
            for (var i = 0; i < nRange; i++)
            {
                for (var j = -i; j <= i; j++)
                {
                    for (var k = -i; k <= i; k++)
                    {
                        nDx = nOrgX + k;
                        nDy = nOrgY + j;
                        if (envir.GetItemEx(nDx, nDy, ref nItemCount) == 0)
                        {
                            if (envir.ChFlag)
                            {
                                result = true;
                                break;
                            }
                        }
                        else
                        {
                            if (envir.ChFlag && n24 > nItemCount)
                            {
                                n24 = nItemCount;
                                n28 = nDx;
                                n2C = nDy;
                            }
                        }
                    }
                    if (result)
                    {
                        break;
                    }
                }
                if (result)
                {
                    break;
                }
            }
            if (!result)
            {
                if (n24 < 8)
                {
                    nDx = n28;
                    nDy = n2C;
                }
                else
                {
                    nDx = nOrgX;
                    nDy = nOrgY;
                }
            }
            return result;
        }

        private void ActionOfRandomMove(NormNpc normNpc, PlayObject PlayObject, QuestActionInfo QuestActionInfo, ref bool Success)
        {
            Success = true;
        }

        private static void ActionOfAddUseDateList(NormNpc normNpc, PlayObject playObject, QuestActionInfo questConditionInfo, ref bool success)
        {
            string sHumName = playObject.ChrName;
            string sListFileName = normNpc.m_sPath + questConditionInfo.sParam1;
            string s10 = string.Empty;
            string sText;
            bool bo15;
            sListFileName = M2Share.GetEnvirFilePath(sListFileName);
            using StringList loadList = new StringList();
            if (File.Exists(sListFileName))
            {
                loadList.LoadFromFile(sListFileName);
            }
            bo15 = false;
            for (int i = 0; i < loadList.Count; i++)
            {
                sText = loadList[i].Trim();
                sText = HUtil32.GetValidStrCap(sText, ref s10, new[] { ' ', '\t' });
                if (string.Compare(sHumName, s10, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    bo15 = true;
                    break;
                }
            }
            if (!bo15)
            {
                s10 = $"{sHumName}    {DateTime.Today}";
                loadList.Add(s10);
                try
                {
                    loadList.SaveToFile(sListFileName);
                }
                catch
                {
                    M2Share.Logger.Error("saving fail.... => " + sListFileName);
                }
            }
        }

        private void ActionOfAddNameList(NormNpc normNpc, PlayObject playObject, QuestActionInfo questConditionInfo, ref bool success)
        {
            string sListFileName = normNpc.m_sPath + questConditionInfo.sParam1;
            ActionOfAddList(playObject.ChrName, sListFileName);
        }

        private void ActionOfDelUseDateList(NormNpc normNpc, PlayObject playObject, QuestActionInfo questConditionInfo, ref bool success)
        {
            string sListFileName = normNpc.m_sPath + questConditionInfo.sParam1;
            ActionOfDelList(playObject.ChrName, sListFileName);
        }

        private void ActionOfDelNameList(NormNpc normNpc, PlayObject playObject, QuestActionInfo questConditionInfo, ref bool success)
        {
            string sListFileName = normNpc.m_sPath + questConditionInfo.sParam1;
            ActionOfDelList(playObject.ChrName, sListFileName);
        }

        private void ActionOfDelAccountList(NormNpc normNpc, PlayObject playObject, QuestActionInfo questConditionInfo, ref bool success)
        {
            string playName = playObject.UserAccount;
            string sListFileName = normNpc.m_sPath + questConditionInfo.sParam1;
            ActionOfDelList(playObject.UserAccount, sListFileName);
        }

        private void ActionOfAddAccountList(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            ActionOfAddList(playObject.UserAccount, normNpc.m_sPath + questActionInfo.sParam1);
        }

        private void ActionOfAddIpList(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            ActionOfAddList(playObject.LoginIpAddr, normNpc.m_sPath + questActionInfo.sParam1);
        }

        private void ActionOfDelIpList(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            ActionOfAddList(playObject.LoginIpAddr, normNpc.m_sPath + questActionInfo.sParam1);
        }

        private void ActionOfAddGuildList(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            if (playObject.MyGuild != null)
            {
                ActionOfAddList(playObject.MyGuild.GuildName, normNpc.m_sPath + questActionInfo.sParam1);
            }
        }

        private void ActionOfDelGuildList(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool Success)
        {
            if (playObject.MyGuild != null)
            {
                ActionOfDelList(playObject.MyGuild.GuildName, normNpc.m_sPath + questActionInfo.sParam1);
            }
        }

        private void ActionOfAddList(string val, string fileName)
        {
            var sListFileName = M2Share.GetEnvirFilePath(fileName);
            using StringList loadList = new StringList();
            if (File.Exists(sListFileName))
            {
                loadList.LoadFromFile(sListFileName);
            }
            bool bo15 = false;
            for (int i = 0; i < loadList.Count; i++)
            {
                string s10 = loadList[i].Trim();
                if (string.Compare(val, s10, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    loadList.RemoveAt(i);
                    bo15 = true;
                    break;
                }
            }
            if (bo15)
            {
                loadList.SaveToFile(sListFileName);
            }
        }

        private void ActionOfDelList(string val, string fileName)
        {
            var sListFileName = M2Share.GetEnvirFilePath(fileName);
            using StringList loadList = new StringList();
            if (File.Exists(sListFileName))
            {
                loadList.LoadFromFile(sListFileName);
            }
            bool bo15 = false;
            for (int i = 0; i < loadList.Count; i++)
            {
                string s10 = loadList[i].Trim();
                if (string.Compare(val, s10, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    loadList.RemoveAt(i);
                    bo15 = true;
                    break;
                }
            }
            if (bo15)
            {
                loadList.SaveToFile(sListFileName);
            }
        }

        private void ScriptActionError(PlayObject PlayObject, string sErrMsg, QuestActionInfo QuestActionInfo, ExecutionCode sCmd)
        {
            //const string sOutMessage = "[脚本错误] {0} 脚本命令:{1} NPC名称:{2} 地图:{3}({4}:{5}) 参数1:{6} 参数2:{7} 参数3:{8} 参数4:{9} 参数5:{10} 参数6:{11}";
            //string sMsg = Format(sOutMessage, sErrMsg, sCmd, ChrName, MapName, CurrX, CurrY, QuestActionInfo.sParam1, QuestActionInfo.sParam2, QuestActionInfo.sParam3, QuestActionInfo.sParam4, QuestActionInfo.sParam5, QuestActionInfo.sParam6);
            //M2Share.Logger.Error(sMsg);
        }

        public void Dispose(object obj)
        {
            obj = null;
        }
    }
}