using GameSrv.Actor;
using GameSrv.Guild;
using GameSrv.Maps;
using GameSrv.Npc;
using GameSrv.Player;
using GameSrv.Castle;
using GameSrv.Items;
using GameSrv.Monster.Monsters;
using GameSrv.Script;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv
{
    internal class ConditionFunction : ScriptCore
    {
        private delegate void ScriptCondition(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success);
        /// <summary>
        /// 脚本条件检查列表
        /// </summary>
        private Dictionary<int, ScriptCondition> _conditionMap;

        private string _mSPath;
        private string _chrName;
        private string _mMapName;
        private int _mNX;
        private int _mNY;

        public ConditionFunction(string sPath, string chrName, string sMapName, int nX, int nY)
        {
            _mSPath = sPath;
            chrName = chrName;
            _mMapName = sMapName;
            _mNX = nX;
            _mNY = nY;
        }

        public bool IsRegister(int cmdCode)
        {
            return _conditionMap.ContainsKey(cmdCode);
        }
        
        public void Execute(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            if (_conditionMap.ContainsKey(questConditionInfo.CmdCode))
            {
                _conditionMap[questConditionInfo.CmdCode](playObject, questConditionInfo, ref success);
            }
        }

        public void Initialize()
        {
            _conditionMap = new Dictionary<int, ScriptCondition>();
            _conditionMap[CheckScriptDef.nCHECK] = ConditionOfCheck;
            _conditionMap[CheckScriptDef.nRANDOM] = ConditionOfRandom;
            _conditionMap[CheckScriptDef.nGENDER] = ConditionOfGender;
            _conditionMap[CheckScriptDef.nCHECKLEVEL] = ConditionOfCheckLevel;
            _conditionMap[CheckScriptDef.nCHECKJOB] = ConditionOfCheckJob;
            _conditionMap[CheckScriptDef.nCHECKITEM] = ConditionOfCheckItem;
            _conditionMap[CheckScriptDef.nCHECKGOLD] = ConditionOfCheckGold;
            _conditionMap[CheckScriptDef.nCHECKDURA] = ConditionOfCheckDura;
            _conditionMap[CheckScriptDef.nDAYOFWEEK] = ConditionOfDayOfWeek;
            _conditionMap[CheckScriptDef.nHOUR] = ConditionOfHour;
            _conditionMap[CheckScriptDef.nMIN] = ConditionOfMin;
            //FNpcCondition[CheckScriptDef.nCHECKPKPOINT] = ConditionOfCheckPkPoint;
            _conditionMap[CheckScriptDef.nCHECKMONMAP] = ConditionOfCheckMonMapCount;
            _conditionMap[CheckScriptDef.nCHECKHUM] = ConditionOfCheckHum;
            //FNpcCondition[CheckScriptDef.nCHECKBAGGAGE] = ConditionOfCheckBagGage;
            _conditionMap[CheckScriptDef.nCHECKNAMELIST] = ConditionOfCheckNameList;
            _conditionMap[CheckScriptDef.nCHECKACCOUNTLIST] = ConditionOfCheckAccountList;
            _conditionMap[CheckScriptDef.nCHECKIPLIST] = ConditionOfCheckIpList;
            _conditionMap[CheckScriptDef.nEQUAL] = ConditionOfEqual;
            _conditionMap[CheckScriptDef.nLARGE] = ConditionOfLapge;
            _conditionMap[CheckScriptDef.nSMALL] = ConditionOfSmall;
            _conditionMap[CheckScriptDef.nSC_ISSYSOP] = ConditionOfIssysop;
            _conditionMap[CheckScriptDef.nSC_ISADMIN] = ConditionOfIsAdmin;
            _conditionMap[CheckScriptDef.nSC_CHECKGROUPCOUNT] = ConditionOfCheckGroupCount;
            _conditionMap[CheckScriptDef.nSC_CHECKPOSEDIR] = ConditionOfCheckPoseDir;
            _conditionMap[CheckScriptDef.nSC_CHECKPOSELEVEL] = ConditionOfCheckPoseLevel;
            _conditionMap[CheckScriptDef.nSC_CHECKPOSEGENDER] = ConditionOfCheckPoseGender;
            _conditionMap[CheckScriptDef.nSC_CHECKLEVELEX] = ConditionOfCheckLevelEx;
            _conditionMap[CheckScriptDef.nSC_CHECKBONUSPOINT] = ConditionOfCheckBonusPoint;
            _conditionMap[CheckScriptDef.nSC_CHECKMARRY] = ConditionOfCheckMarry;
            _conditionMap[CheckScriptDef.nSC_CHECKPOSEMARRY] = ConditionOfCheckPoseMarry;
            _conditionMap[CheckScriptDef.nSC_CHECKMARRYCOUNT] = ConditionOfCheckMarryCount;
            _conditionMap[CheckScriptDef.nSC_CHECKMASTER] = ConditionOfCheckMaster;
            _conditionMap[CheckScriptDef.nSC_HAVEMASTER] = ConditionOfHaveMaster;
            _conditionMap[CheckScriptDef.nSC_CHECKPOSEMASTER] = ConditionOfCheckPoseMaster;
            _conditionMap[CheckScriptDef.nSC_POSEHAVEMASTER] = ConditionOfPoseHaveMaster;
            _conditionMap[CheckScriptDef.nSC_CHECKISMASTER] = ConditionOfCheckIsMaster;
            _conditionMap[CheckScriptDef.nSC_HASGUILD] = ConditionOfCheckHaveGuild;
            _conditionMap[CheckScriptDef.nSC_ISGUILDMASTER] = ConditionOfCheckIsGuildMaster;
            _conditionMap[CheckScriptDef.nSC_CHECKCASTLEMASTER] = ConditionOfCheckIsCastleMaster;
            _conditionMap[CheckScriptDef.nSC_ISCASTLEGUILD] = ConditionOfCheckIsCastleaGuild;
            _conditionMap[CheckScriptDef.nSC_ISATTACKGUILD] = ConditionOfCheckIsAttackGuild;
            _conditionMap[CheckScriptDef.nSC_ISDEFENSEGUILD] = ConditionOfCheckIsDefenseGuild;
            _conditionMap[CheckScriptDef.nSC_CHECKCASTLEDOOR] = ConditionOfCheckCastleDoorStatus;
            _conditionMap[CheckScriptDef.nSC_ISDEFENSEALLYGUILD] = ConditionOfCheckIsDefenseAllyGuild;
            _conditionMap[CheckScriptDef.nSC_CHECKPOSEISMASTER] = ConditionOfCheckPoseIsMaster;
            _conditionMap[CheckScriptDef.nSC_CHECKNAMEIPLIST] = ConditionOfCheckNameIpList;
            _conditionMap[CheckScriptDef.nSC_CHECKACCOUNTIPLIST] = ConditionOfCheckAccountIpList;
            _conditionMap[CheckScriptDef.nSC_CHECKSLAVECOUNT] = ConditionOfCheckSlaveCount;
            _conditionMap[CheckScriptDef.nSC_ISNEWHUMAN] = ConditionOfIsNewHuman;
            _conditionMap[CheckScriptDef.nSC_CHECKMEMBERTYPE] = ConditionOfCheckMemberType;
            _conditionMap[CheckScriptDef.nSC_CHECKMEMBERLEVEL] = ConditionOfCheckMemBerLevel;
            _conditionMap[CheckScriptDef.nSC_CHECKGAMEPOINT] = ConditionOfCheckGamePoint;
            _conditionMap[CheckScriptDef.nSC_CHECKNAMELISTPOSITION] = ConditionOfCheckNameListPostion;
            _conditionMap[CheckScriptDef.nSC_CHECKGUILDLIST] = ConditionOfCheckGuildList;
            _conditionMap[CheckScriptDef.nSC_CHECKRENEWLEVEL] = ConditionOfCheckReNewLevel;
            _conditionMap[CheckScriptDef.nSC_CHECKSLAVELEVEL] = ConditionOfCheckSlaveLevel;
            _conditionMap[CheckScriptDef.nSC_CHECKSLAVENAME] = ConditionOfCheckSlaveName;
            _conditionMap[CheckScriptDef.nSC_CHECKCREDITPOINT] = ConditionOfCheckCreditPoint;
            _conditionMap[CheckScriptDef.nSC_CHECKOFGUILD] = ConditionOfCheckOfGuild;
            _conditionMap[CheckScriptDef.nSC_CHECKUSEITEM] = ConditionOfCheckUseItem;
            _conditionMap[CheckScriptDef.nSC_CHECKBAGSIZE] = ConditionOfCheckBagSize;
            _conditionMap[CheckScriptDef.nSC_CHECKDC] = ConditionOfCheckDc;
            //FNpcCondition[CheckScriptDef.nSC_CHECKMC] = ConditionOfCheckMC;
            _conditionMap[CheckScriptDef.nSC_CHECKSC] = ConditionOfCheckSc;
            _conditionMap[CheckScriptDef.nSC_CHECKHP] = ConditionOfCheckHp;
            _conditionMap[CheckScriptDef.nSC_CHECKMP] = ConditionOfCheckMp;
            //FNpcCondition[CheckScriptDef.nSC_CHECKITEMTYPE] = ConditionOfCheckItemType;
            _conditionMap[CheckScriptDef.nSC_CHECKEXP] = ConditionOfCheckExp;
            _conditionMap[CheckScriptDef.nSC_CHECKCASTLEGOLD] = ConditionOfCheckCastleGold;
            _conditionMap[CheckScriptDef.nSC_CHECKBUILDPOINT] = ConditionOfCheckGuildBuildPoint;
            _conditionMap[CheckScriptDef.nSC_CHECKAURAEPOINT] = ConditionOfCheckGuildAuraePoint;
            _conditionMap[CheckScriptDef.nSC_CHECKSTABILITYPOINT] = ConditionOfCheckStabilityPoint;
            _conditionMap[CheckScriptDef.nSC_CHECKFLOURISHPOINT] = ConditionOfCheckFlourishPoint;
            _conditionMap[CheckScriptDef.nSC_CHECKCONTRIBUTION] = ConditionOfCheckContribution;
            _conditionMap[CheckScriptDef.nSC_CHECKRANGEMONCOUNT] = ConditionOfCheckRangeMonCount;
            _conditionMap[CheckScriptDef.nSC_CHECKINMAPRANGE] = ConditionOfCheckInMapRange;
            _conditionMap[CheckScriptDef.nSC_CASTLECHANGEDAY] = ConditionOfCheckCastleChageDay;
            _conditionMap[CheckScriptDef.nSC_CASTLEWARDAY] = ConditionOfCheckCastleWarDay;
            _conditionMap[CheckScriptDef.nSC_ONLINELONGMIN] = ConditionOfCheckOnlineLongMin;
            _conditionMap[CheckScriptDef.nSC_CHECKGUILDCHIEFITEMCOUNT] = ConditionOfCheckChiefItemCount;
            _conditionMap[CheckScriptDef.nSC_CHECKNAMEDATELIST] = ConditionOfCheckNameDateList;
            _conditionMap[CheckScriptDef.nSC_CHECKUSERDATE] = ConditionOfCheckNameDateList;
            _conditionMap[CheckScriptDef.nSC_CHECKMAPHUMANCOUNT] = ConditionOfCheckMapHumanCount;
            _conditionMap[CheckScriptDef.nSC_CHECKMAPMONCOUNT] = ConditionOfCheckMapMonCount;
            _conditionMap[CheckScriptDef.nSC_CHECKVAR] = ConditionOfCheckVar;
            _conditionMap[CheckScriptDef.nSC_CHECKSERVERNAME] = ConditionOfCheckServerName;
            _conditionMap[CheckScriptDef.nCHECKMAPNAME] = ConditionOfCheckMapName;
            _conditionMap[CheckScriptDef.nINSAFEZONE] = ConditionOfCheckSafeZone;
            //FNpcCondition[CheckScriptDef.nCHECKSKILL] = ConditionOfCheckSkill;
            _conditionMap[CheckScriptDef.nSC_CHECKCONTAINSTEXT] = ConditionOfAnsiContainsText;
            _conditionMap[CheckScriptDef.nSC_COMPARETEXT] = ConditionOfCompareText;
            _conditionMap[CheckScriptDef.nSC_CHECKTEXTLIST] = ConditionOfCheckTextList;
            _conditionMap[CheckScriptDef.nSC_ISGROUPMASTER] = ConditionOfIsGroupMaster;
            _conditionMap[CheckScriptDef.nSC_CHECKCONTAINSTEXTLIST] = ConditionOfCheCkContAinsTextList;
            _conditionMap[CheckScriptDef.nSC_CHECKONLINE] = ConditionOfCheckOnLine;
            _conditionMap[CheckScriptDef.nSC_ISDUPMODE] = ConditionOfIsDupMode;
            _conditionMap[CheckScriptDef.nSC_ISONMAP] = ConditionOfIosnMap;
        }

        private void ConditionOfCheckOfGuild(PlayObject playObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            string sGuildName;
            success = false;
            if (questConditionInfo.sParam1 == "")
            {
                ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKOFGUILD);
                return;
            }
            if (playObject.MyGuild != null)
            {
                sGuildName = questConditionInfo.sParam1;
                GetVarValue(playObject, questConditionInfo.sParam1, ref sGuildName);
                if (String.Compare(playObject.MyGuild.GuildName, sGuildName, StringComparison.Ordinal) == 0)
                {
                    success = true;
                }
            }
        }

        private void ConditionOfCheckOnlineLongMin(PlayObject playObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            var nOnlineMin = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nOnlineMin < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam2, ref nOnlineMin);
                if (nOnlineMin < 0)
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_ONLINELONGMIN);
                    return;
                }
            }
            long nOnlineTime = (HUtil32.GetTickCount() - playObject.LogonTick) / 60000;
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nOnlineTime == nOnlineMin)
                    {
                        success = true;
                    }
                    break;
                case '>':
                    if (nOnlineTime > nOnlineMin)
                    {
                        success = true;
                    }
                    break;
                case '<':
                    if (nOnlineTime < nOnlineMin)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (nOnlineTime >= nOnlineMin)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckPasswordErrorCount(PlayObject playObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            var nErrorCount = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nErrorCount < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam2, ref nErrorCount);
                if (nErrorCount < 0)
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_PASSWORDERRORCOUNT);
                    return;
                }
            }
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (playObject.PwdFailCount == nErrorCount)
                    {
                        success = true;
                    }
                    break;
                case '>':
                    if (playObject.PwdFailCount > nErrorCount)
                    {
                        success = true;
                    }
                    break;
                case '<':
                    if (playObject.PwdFailCount < nErrorCount)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (playObject.PwdFailCount >= nErrorCount)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfIsLockPassword(PlayObject playObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = playObject.IsPasswordLocked;
        }

        private void ConditionOfIsLockStorage(PlayObject playObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = !playObject.IsCanGetBackItem;
        }

        private void ConditionOfCheckPayMent(PlayObject playObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            var nPayMent = HUtil32.StrToInt(questConditionInfo.sParam1, -1);
            if (nPayMent < 1)
            {
                GetVarValue(playObject, questConditionInfo.sParam1, ref nPayMent);
                if (nPayMent < 1)
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKPAYMENT);
                    return;
                }
            }
            if (playObject.PayMent == nPayMent)
            {
                success = true;
            }
        }

        private void ConditionOfCheckSlaveName(BaseObject baseObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            BaseObject aObject;
            success = false;
            if (questConditionInfo.sParam1 == "")
            {
                ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKSLAVENAME);
                return;
            }
            var sSlaveName = questConditionInfo.sParam1;
            if (baseObject.Race == ActorRace.Play)
            {
                GetVarValue((PlayObject)baseObject, questConditionInfo.sParam1, ref sSlaveName);
            }
            for (int i = 0; i < baseObject.SlaveList.Count; i++)
            {
                aObject = baseObject.SlaveList[i];
                if (string.Compare(sSlaveName, aObject.ChrName, StringComparison.Ordinal) == 0)
                {
                    success = true;
                    break;
                }
            }
        }

        private void ConditionOfCheckNameDateList(PlayObject playObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            StringList loadList;
            string sLineText;
            string sHumName = string.Empty;
            string sDate = string.Empty;
            int nDay;
            string sVar = string.Empty;
            string sValue = string.Empty;
            int nValue = 0;
            VarInfo varInfo;
            success = false;
            var nDayCount = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
            var boDeleteExprie = string.Compare(questConditionInfo.sParam6, "清理", StringComparison.Ordinal) == 0;
            var boNoCompareHumanName = string.Compare(questConditionInfo.sParam6, "1", StringComparison.Ordinal) == 0;
            var cMethod = questConditionInfo.sParam2[0];
            if (nDayCount < 0)
            {
                ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKNAMEDATELIST);
                return;
            }
            var sListFileName = M2Share.Config.EnvirDir + _mSPath + questConditionInfo.sParam1;
            if (File.Exists(sListFileName))
            {
                loadList = new StringList();
                try
                {
                    loadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    M2Share.Logger.Error("loading fail.... => " + sListFileName);
                }
                for (int I = 0; I < loadList.Count; I++)
                {
                    sLineText = loadList[I].Trim();
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sHumName, new string[] { " ", "\09" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sDate, new string[] { " ", "\09" });
                    if (string.Compare(sHumName, playObject.ChrName, StringComparison.Ordinal) == 0 ||
                        boNoCompareHumanName)
                    {
                        nDay = int.MaxValue;
                        //if (TryStrToDateTime(sDate, dOldDate))
                        //{
                        //    nDay = HUtil32.GetDayCount(DateTime.Now, dOldDate);
                        //}
                        switch (cMethod)
                        {
                            case '=':
                                if (nDay == nDayCount)
                                {
                                    success = true;
                                }
                                break;

                            case '>':
                                if (nDay > nDayCount)
                                {
                                    success = true;
                                }
                                break;

                            case '<':
                                if (nDay < nDayCount)
                                {
                                    success = true;
                                }
                                break;
                            default:
                                if (nDay >= nDayCount)
                                {
                                    success = true;
                                }
                                break;
                        }
                        varInfo = GetVarValue(playObject, questConditionInfo.sParam4, ref sVar, ref sValue, ref nValue);
                        switch (varInfo.VarAttr)
                        {
                            case VarAttr.aNone:
                            case VarAttr.aConst:
                                ScriptConditionError(playObject, questConditionInfo,
                                    CheckScriptDef.sSC_CHECKNAMEDATELIST);
                                break;
                            case VarAttr.aFixStr:
                                SetValNameValue(playObject, sVar, sValue, nDay);
                                break;
                            case VarAttr.aDynamic:
                                SetDynamicValue(playObject, sVar, sValue, nDay);
                                break;
                        }
                        varInfo = GetVarValue(playObject, questConditionInfo.sParam5, ref sVar, ref sValue, ref nValue);
                        switch (varInfo.VarAttr)
                        {
                            case VarAttr.aNone:
                            case VarAttr.aConst:
                                ScriptConditionError(playObject, questConditionInfo,
                                    CheckScriptDef.sSC_CHECKNAMEDATELIST);
                                break;
                            case VarAttr.aFixStr:
                                SetValNameValue(playObject, sVar, sValue, nDayCount - nDay);
                                break;
                            case VarAttr.aDynamic:
                                SetDynamicValue(playObject, sVar, sValue, nDayCount - nDay);
                                break;
                        }
                        if (!success)
                        {
                            if (boDeleteExprie)
                            {
                                loadList.RemoveAt(I);
                                try
                                {
                                    loadList.SaveToFile(sListFileName);
                                }
                                catch
                                {
                                    M2Share.Logger.Error("Save fail.... => " + sListFileName);
                                }
                            }
                        }
                        break;
                    }
                }
                Dispose(loadList);
            }
            else
            {
                M2Share.Logger.Error("file not found => " + sListFileName);
            }
        }

        private void ConditionOfCheckGuildNameDateList(PlayObject playObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            StringList loadList;
            string sListFileName;
            string sLineText;
            string sHumName = string.Empty;
            string sDate = string.Empty;
            bool boDeleteExprie;
            bool boNoCompareHumanName;
            char cMethod;
            int nDayCount;
            int nDay;
            string sVar = string.Empty;
            string sValue = string.Empty;
            int nValue = 0;
            VarInfo varInfo;
            success = false;
            if (playObject.MyGuild != null)
            {
                nDayCount = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
                boDeleteExprie = string.Compare(questConditionInfo.sParam6, "清理", StringComparison.Ordinal) == 0;
                boNoCompareHumanName = string.Compare(questConditionInfo.sParam6, "1", StringComparison.Ordinal) == 0;
                cMethod = questConditionInfo.sParam2[1];
                if (nDayCount < 0)
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKGUILDNAMEDATELIST);
                    return;
                }
                sListFileName = M2Share.Config.EnvirDir + _mSPath + questConditionInfo.sParam1;
                if (File.Exists(sListFileName))
                {
                    loadList = new StringList();
                    try
                    {
                        loadList.LoadFromFile(sListFileName);
                    }
                    catch
                    {
                        M2Share.Logger.Error("loading fail.... => " + sListFileName);
                    }
                    for (int i = 0; i < loadList.Count; i++)
                    {
                        sLineText = loadList[i].Trim();
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sHumName, new string[] { " ", "\09" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sDate, new string[] { " ", "\09" });
                        if (string.Compare(sHumName, playObject.MyGuild.GuildName, StringComparison.Ordinal) == 0 ||
                            boNoCompareHumanName)
                        {
                            nDay = int.MaxValue;

                            //if (TryStrToDateTime(sDate, dOldDate))
                            //{
                            //    nDay = HUtil32.GetDayCount(DateTime.Now, dOldDate);
                            //}
                            switch (cMethod)
                            {
                                case '=':
                                    if (nDay == nDayCount)
                                    {
                                        success = true;
                                    }
                                    break;

                                case '>':
                                    if (nDay > nDayCount)
                                    {
                                        success = true;
                                    }
                                    break;

                                case '<':
                                    if (nDay < nDayCount)
                                    {
                                        success = true;
                                    }
                                    break;
                                default:
                                    if (nDay >= nDayCount)
                                    {
                                        success = true;
                                    }
                                    break;
                            }
                            if (questConditionInfo.sParam4 != "")
                            {
                                varInfo = GetVarValue(playObject, questConditionInfo.sParam4, ref sVar, ref sValue, ref nValue);
                                switch (varInfo.VarAttr)
                                {
                                    case VarAttr.aNone:
                                    case VarAttr.aConst:
                                        ScriptConditionError(playObject, questConditionInfo,
                                            CheckScriptDef.sSC_CHECKGUILDNAMEDATELIST);
                                        break;

                                    case VarAttr.aFixStr:
                                        SetValNameValue(playObject, sVar, sValue, nDay);
                                        break;

                                    case VarAttr.aDynamic:
                                        SetDynamicValue(playObject, sVar, sValue, nDay);
                                        break;
                                }
                            }
                            if (questConditionInfo.sParam5 != "")
                            {
                                varInfo = GetVarValue(playObject, questConditionInfo.sParam5, ref sVar, ref sValue, ref nValue);
                                switch (varInfo.VarAttr)
                                {
                                    case VarAttr.aNone:
                                    case VarAttr.aConst:
                                        ScriptConditionError(playObject, questConditionInfo,
                                            CheckScriptDef.sSC_CHECKGUILDNAMEDATELIST);
                                        break;

                                    case VarAttr.aFixStr:
                                        SetValNameValue(playObject, sVar, sValue, nDayCount - nDay);
                                        break;

                                    case VarAttr.aDynamic:
                                        SetDynamicValue(playObject, sVar, sValue, nDayCount - nDay);
                                        break;
                                }
                            }
                            if (!success)
                            {
                                if (boDeleteExprie)
                                {
                                    loadList.RemoveAt(i);
                                    try
                                    {
                                        loadList.SaveToFile(sListFileName);
                                    }
                                    catch
                                    {
                                        M2Share.Logger.Error("Save fail.... => " + sListFileName);
                                    }
                                }
                            }
                            break;
                        }
                    }
                    Dispose(loadList);
                }
                else
                {
                    M2Share.Logger.Error("file not found => " + sListFileName);
                }
            }
        }

        // CHECKMAPHUMANCOUNT MAP = COUNT
        private void ConditionOfCheckMapHumanCount(BaseObject baseObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            var nCount = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
            if (nCount < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam3, ref nCount);
                    if (nCount < 0)
                    {
                        ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKMAPHUMANCOUNT);
                        return;
                    }
                }
                else
                {
                    ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKMAPHUMANCOUNT);
                    return;
                }
            }
            var sMapName = questConditionInfo.sParam1;
            var envir = M2Share.MapMgr.FindMap(sMapName);
            if (envir == null)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam1, ref sMapName);
                    envir = M2Share.MapMgr.FindMap(sMapName);
                }
            }
            if (envir == null)
            {
                ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKMAPHUMANCOUNT);
                return;
            }
            var nHumanCount = M2Share.WorldEngine.GetMapHuman(envir.MapName);
            var cMethod = questConditionInfo.sParam2[1];
            switch (cMethod)
            {
                case '=':
                    if (nHumanCount == nCount)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (nHumanCount > nCount)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (nHumanCount < nCount)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (nHumanCount >= nCount)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckMapMonCount(BaseObject baseObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            int nCount = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
            string sMapName = questConditionInfo.sParam1;
            Envirnoment envir = M2Share.MapMgr.FindMap(sMapName);
            if (baseObject.Race == ActorRace.Play)
            {
                if (nCount < 0)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam3, ref nCount);
                }
                if (envir == null)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam1, ref sMapName);
                    envir = M2Share.MapMgr.FindMap(sMapName);
                }
            }
            if (nCount < 0 || envir == null)
            {
                ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKMAPMONCOUNT);
                return;
            }
            var nMonCount = M2Share.WorldEngine.GetMapMonster(envir, null);
            var cMethod = questConditionInfo.sParam2[1];
            switch (cMethod)
            {
                case '=':
                    if (nMonCount == nCount)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (nMonCount > nCount)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (nMonCount < nCount)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (nMonCount >= nCount)
                    {
                        success = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 检测地图命令
        /// 格式:ISONMAP 地图
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <returns></returns>
        private void ConditionOfIosnMap(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            string sMapName = string.Empty;
            try
            {
                if (questConditionInfo.sParam6 == "88")
                {
                    playObject = M2Share.WorldEngine.GetPlayObject(GetLineVariableText(playObject, questConditionInfo.sParam7));
                    if (playObject == null)
                    {
                    }
                }
                GetVarValue(playObject, questConditionInfo.sParam1, ref sMapName);
                sMapName = GetLineVariableText(playObject, questConditionInfo.sParam1); // 地图支持变量
                var envir = M2Share.MapMgr.FindMap(sMapName);
                if (envir == null)
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_ISONMAP);
                }
                if (playObject.Envir == envir)
                {
                    success = true;
                }
            }
            catch
            {
                M2Share.Logger.Error("{异常} TNormNpc.ConditionOfISONMAP");
            }
        }

        private void ConditionOfCheckMonMapCount(BaseObject baseObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            BaseObject aObject;
            string sMapName = questConditionInfo.sParam1;
            int nCount = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (baseObject.Race == ActorRace.Play)
            {
                if (nCount < 0)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam2, ref nCount);
                }
            }
            var envir = M2Share.MapMgr.FindMap(sMapName);
            if (envir == null)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam1, ref sMapName);
                    envir = M2Share.MapMgr.FindMap(sMapName);
                }
            }
            if (envir == null || nCount < 0)
            {
                ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sCHECKMONMAP);
                return;
            }
            var monList = new List<BaseObject>();
            int nMapRangeCount = M2Share.WorldEngine.GetMapMonster(envir, monList);
            for (int I = monList.Count - 1; I >= 0; I--)
            {
                if (monList.Count <= 0)
                {
                    break;
                }
                aObject = monList[I];
                if (aObject.Race < ActorRace.Animal || aObject.Race == ActorRace.ArcherGuard ||
                    aObject.Master != null || aObject.Race == ActorRace.NPC ||
                    aObject.Race == ActorRace.PeaceNpc)
                {
                    monList.RemoveAt(I);
                }
            }
            nMapRangeCount = monList.Count;
            if (nMapRangeCount >= nCount)
            {
                success = true;
            }
            Dispose(monList);
        }

        private void ConditionOfCheckRangeMonCount(BaseObject baseObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            BaseObject aObject;
            success = false;
            var sMapName = questConditionInfo.sParam1;
            var nX = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            var nY = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
            var nRange = HUtil32.StrToInt(questConditionInfo.sParam4, -1);
            var cMethod = questConditionInfo.sParam5[1];
            var nCount = HUtil32.StrToInt(questConditionInfo.sParam6, -1);
            if (baseObject.Race == ActorRace.Play)
            {
                if (nX < 0)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam2, ref nX);
                }
                if (nY < 0)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam3, ref nY);
                }
                if (nRange < 0)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam4, ref nRange);
                }
                if (nCount < 0)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam6, ref nCount);
                }
            }
            var envir = M2Share.MapMgr.FindMap(sMapName);
            if (envir == null || nX < 0 || nY < 0 || nRange < 0 || nCount < 0)
            {
                ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKRANGEMONCOUNT);
                return;
            }
            var monList = new List<BaseObject>();
            int nMapRangeCount = envir.GetRangeBaseObject(nX, nY, nRange, true, monList);
            for (int i = monList.Count - 1; i >= 0; i--)
            {
                if (monList.Count <= 0)
                {
                    break;
                }
                aObject = monList[i];
                if (aObject.Race < ActorRace.Animal || aObject.Race == ActorRace.ArcherGuard || aObject.Master != null || aObject.Race == ActorRace.NPC ||
                    aObject.Race == ActorRace.PeaceNpc)
                {
                    monList.RemoveAt(i);
                }
            }
            nMapRangeCount = monList.Count;
            Dispose(monList);
            switch (cMethod)
            {
                case '=':
                    if (nMapRangeCount == nCount)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (nMapRangeCount > nCount)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (nMapRangeCount < nCount)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (nMapRangeCount >= nCount)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckRangeGroupCount(PlayObject playObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            int nRange;
            int nCount;
            char cMethod = ' ';
            int nMapRangeCount;
            var groupOwnerPlay = (PlayObject)M2Share.ActorMgr.Get(playObject.GroupOwner);
            if (playObject.GroupOwner != 0 && groupOwnerPlay != null)
            {
                nRange = HUtil32.StrToInt(questConditionInfo.sParam1, -1);
                if (questConditionInfo.sParam2 != "")
                {
                    cMethod = questConditionInfo.sParam2[1];
                }
                nCount = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
                if (nRange < 0)
                {
                    GetVarValue(playObject, questConditionInfo.sParam1, ref nRange);
                }
                if (nCount < 0)
                {
                    GetVarValue(playObject, questConditionInfo.sParam3, ref nCount);
                }
                if (nRange < 0 || nCount < 0 || cMethod == ' ')
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKRANGEROUPCOUNT);
                    return;
                }
                nMapRangeCount = 0;
                for (int i = 0; i < groupOwnerPlay.GroupMembers.Count; i++)
                {
                    var groupHuman = groupOwnerPlay.GroupMembers[i];
                    if (groupHuman.Envir == playObject.Envir)
                    {
                        if (Math.Abs(playObject.CurrX - groupHuman.CurrX) <= nRange &&
                            Math.Abs(playObject.CurrY - groupHuman.CurrY) <= nRange)
                        {
                            nMapRangeCount++;
                        }
                    }
                }
                switch (cMethod)
                {
                    case '=':
                        if (nMapRangeCount == nCount)
                        {
                            success = true;
                        }
                        break;

                    case '>':
                        if (nMapRangeCount > nCount)
                        {
                            success = true;
                        }
                        break;

                    case '<':
                        if (nMapRangeCount < nCount)
                        {
                            success = true;
                        }
                        break;
                    default:
                        if (nMapRangeCount >= nCount)
                        {
                            success = true;
                        }
                        break;
                }
            }
        }

        private void ConditionOfCheckOnLinePlayCount(PlayObject playObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            if (questConditionInfo.sParam1 == "" || questConditionInfo.sParam2 == "")
            {
                ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKONLINEPLAYCOUNT);
                return;
            }
            var cMethod = questConditionInfo.sParam1[0];
            var nCount = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam2, ref nCount);
            }
            if (nCount < 0)
            {
                ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKONLINEPLAYCOUNT);
                return;
            }
            switch (cMethod)
            {
                case '=':
                    if (M2Share.WorldEngine.PlayObjectCount == nCount)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (M2Share.WorldEngine.PlayObjectCount > nCount)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (M2Share.WorldEngine.PlayObjectCount < nCount)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (M2Share.WorldEngine.PlayObjectCount >= nCount)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private bool ConditionOfCheckItemBindUse(PlayObject playObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            UserItem userItem = null;
            int nWhere = HUtil32.StrToInt(questConditionInfo.sParam1, -1);
            if (nWhere < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam1, ref nWhere); // 增加变量支持
            }
            if (nWhere < 0)
            {
                ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKUSEITEMBIND);
                return false;
            }
            userItem = playObject.UseItems[nWhere];
            var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
            if (userItem.Index <= 0 || stdItem == null)
            {
                playObject.SysMsg("你身上没有戴指定物品！！！", MsgColor.Red, MsgType.Hint);
                return false;
            }
            return playObject.CheckItemBindUse(userItem);
        }

        public bool ConditionOfCheckMemoryItemIsRememberItem(UserItem userItem)
        {
            StdItem stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
            if (stdItem == null)
            {
                return false;
            }
            return stdItem.StdMode == 31 && stdItem.AniCount != 0 && stdItem.Shape == 1;
        }
      
        private void ConditionOfCheckReNewLevel(PlayObject playObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            var nLevel = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam2, ref nLevel);
                if (nLevel < 0)
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKLEVELEX);
                    return;
                }
            }
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (playObject.ReLevel == nLevel)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (playObject.ReLevel > nLevel)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (playObject.ReLevel < nLevel)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (playObject.ReLevel >= nLevel)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckSlaveLevel(BaseObject baseObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            int nLevel = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            BaseObject aObject;
            if (!(nLevel >= 0 && nLevel <= 7))
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam2, ref nLevel);
                    if (!(nLevel >= 0 && nLevel <= 7))
                    {
                        ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKSLAVELEVEL);
                        return;
                    }
                }
            }
            var cMethod = questConditionInfo.sParam1[0];
            for (int i = 0; i < baseObject.SlaveList.Count; i++)
            {
                aObject = baseObject.SlaveList[i];
                switch (cMethod)
                {
                    case '=':
                        if (aObject.Abil.Level == nLevel)
                        {
                            success = true;
                            break;
                        }
                        break;

                    case '>':
                        if (aObject.Abil.Level > nLevel)
                        {
                            success = true;
                            break;
                        }
                        break;

                    case '<':
                        if (aObject.Abil.Level < nLevel)
                        {
                            success = true;
                            break;
                        }
                        break;
                    default:
                        if (aObject.Abil.Level >= nLevel)
                        {
                            success = true;
                            break;
                        }
                        break;
                }
            }
        }
        
        private void ConditionOfCheckSlaveRange(BaseObject baseObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            int nSlavenRange;
            BaseObject aObject;
            success = false;
            var sChrName = questConditionInfo.sParam1;
            var nRange = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
            if (nRange < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam3, ref nRange);
                    if (nRange < 0)
                    {
                        ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKSLAVERANGE);
                        return;
                    }
                }
            }
            var boFind = false;
            for (int i = 0; i < baseObject.SlaveList.Count; i++)
            {
                aObject = baseObject.SlaveList[i];
                if (string.Compare(sChrName, aObject.ChrName, StringComparison.Ordinal) == 0)
                {
                    boFind = true;
                    break;
                }
            }
            if (!boFind)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam1, ref sChrName);
                }
            }
            var cMethod = questConditionInfo.sParam2[1];
            for (int i = 0; i < baseObject.SlaveList.Count; i++)
            {
                aObject = baseObject.SlaveList[i];
                if (string.Compare(sChrName, aObject.ChrName, StringComparison.Ordinal) == 0)
                {
                    nSlavenRange = Math.Abs(aObject.CurrX - baseObject.CurrX) + Math.Abs(aObject.CurrY - baseObject.CurrY);
                    switch (cMethod)
                    {
                        case '=':
                            if (nSlavenRange == nRange)
                            {
                                success = true;
                            }
                            break;

                        case '>':
                            if (nSlavenRange > nRange)
                            {
                                success = true;
                            }
                            break;

                        case '<':
                            if (nSlavenRange < nRange)
                            {
                                success = true;
                            }
                            break;
                        default:
                            if (nSlavenRange >= nRange)
                            {
                                success = true;
                            }
                            break;
                    }
                    break;
                }
            }
        }

        private void ConditionOfCheckVar(PlayObject playObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            string sName = string.Empty;
            bool boFoundVar = false;
            var sType = questConditionInfo.sParam1;
            var sVarName = questConditionInfo.sParam2;
            var sMethod = questConditionInfo.sParam3;
            long nVarValue = HUtil32.StrToInt(questConditionInfo.sParam4, 0);
            var sVarValue = questConditionInfo.sParam4;
            if (sType == "" || sVarName == "" || sMethod == "")
            {
                ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKVAR);
                return;
            }
            var cMethod = sMethod[1];
            Dictionary<string, DynamicVar> dynamicVarList = GeDynamicVarList(playObject, sType, ref sName);
            if (dynamicVarList == null)
            {
                ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKVAR);
                return;
            }
            else
            {
                if (dynamicVarList.TryGetValue(sVarName, out DynamicVar dynamic))
                {
                    switch (dynamic.VarType)
                    {
                        case VarType.Integer:
                            switch (cMethod)
                            {
                                case '=':
                                    if (dynamic.nInternet == nVarValue)
                                    {
                                        success = true;
                                    }
                                    break;
                                case '>':
                                    if (dynamic.nInternet > nVarValue)
                                    {
                                        success = true;
                                    }
                                    break;
                                case '<':
                                    if (dynamic.nInternet < nVarValue)
                                    {
                                        success = true;
                                    }
                                    break;
                                default:
                                    if (dynamic.nInternet >= nVarValue)
                                    {
                                        success = true;
                                    }
                                    break;
                            }
                            break;
                        case VarType.String:
                            switch (cMethod)
                            {
                                case '=':
                                    if (dynamic.sString == sVarValue)
                                    {
                                        success = true;
                                    }
                                    break;
                            }
                            break;
                    }
                    boFoundVar = true;
                }
                if (!boFoundVar)
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKVAR);
                }
            }
        }

        private void ConditionOfHaveMaster(PlayObject playObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            if (playObject.MasterName != "")
            {
                success = true;
            }
        }

        private void ConditionOfPoseHaveMaster(PlayObject playObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            BaseObject poseHuman = playObject.GetPoseCreate();
            if (poseHuman != null && poseHuman.Race == ActorRace.Play)
            {
                if (((PlayObject)poseHuman).MasterName != "")
                {
                    success = true;
                }
            }
        }

        private void ConditionOfCheckCastleGold(PlayObject playObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            var nGold = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nGold < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam2, ref nGold);
            }
            if (nGold < 0 || playObject.Castle == null)
            {
                ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKCASTLEGOLD);
                return;
            }
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (playObject.Castle.TotalGold == nGold)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (playObject.Castle.TotalGold > nGold)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (playObject.Castle.TotalGold < nGold)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (playObject.Castle.TotalGold >= nGold)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckContribution(PlayObject playObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            var nContribution = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nContribution < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam2, ref nContribution);
                if (nContribution < 0)
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKCONTRIBUTION);
                    return;
                }
            }
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (playObject.Contribution == nContribution)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (playObject.Contribution > nContribution)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (playObject.Contribution < nContribution)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (playObject.Contribution >= nContribution)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckCreditPoint(PlayObject playObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            var nCreditPoint = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nCreditPoint < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam2, ref nCreditPoint);
                if (nCreditPoint < 0)
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKCREDITPOINT);
                    return;
                }
            }
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (playObject.CreditPoint == nCreditPoint)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (playObject.CreditPoint > nCreditPoint)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (playObject.CreditPoint < nCreditPoint)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (playObject.CreditPoint >= nCreditPoint)
                    {
                        success = true;
                    }
                    break;
            }
        }


        private void ConditionOfCheckAccountIpList(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            string sChrName;
            string sCharAccount;
            string sCharIPaddr;
            string sLine;
            string sName = string.Empty;
            string sIPaddr;
            success = false;
            StringList loadList = null;
            try
            {
                sChrName = playObject.ChrName;
                sCharAccount = playObject.UserAccount;
                sCharIPaddr = playObject.LoginIpAddr;
                if (File.Exists(M2Share.Config.EnvirDir + _mSPath + questConditionInfo.sParam1))
                {
                    loadList = new StringList();
                    loadList.LoadFromFile(M2Share.Config.EnvirDir + _mSPath + questConditionInfo.sParam1);
                    for (int i = 0; i < loadList.Count; i++)
                    {
                        sLine = loadList[i];
                        if (sLine[1] == ';')
                        {
                            continue;
                        }
                        sIPaddr = HUtil32.GetValidStr3(sLine, ref sName, new string[] { " ", "/", "\09" });
                        sIPaddr = sIPaddr.Trim();
                        if (sName == sCharAccount && sIPaddr == sCharIPaddr)
                        {
                            success = true;
                            break;
                        }
                    }
                }
                else
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKACCOUNTIPLIST);
                }
            }
            finally
            {
                Dispose(loadList);
            }
        }

        private void ConditionOfCheckBagSize(BaseObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nSize = HUtil32.StrToInt(questConditionInfo.sParam1, -1);
            if (baseObject.Race == ActorRace.Play)
            {
                if (nSize < 0)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam1, ref nSize);
                }
            }
            if (baseObject.Race == ActorRace.Play)
            {
                if (nSize <= 0 || nSize > Grobal2.MaxBagItem)
                {
                    ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKBAGSIZE);
                    return;
                }
                if (baseObject.Race == ActorRace.Play)
                {
                    if (baseObject.ItemList.Count + nSize <= Grobal2.MaxBagItem)
                    {
                        success = true;
                    }
                }
            }
        }

        private void ConditionOfCheckBonusPoint(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var nTotlePoint = playObject.BonusAbil.DC + playObject.BonusAbil.MC + playObject.BonusAbil.SC + playObject.BonusAbil.AC + playObject.BonusAbil.MAC + playObject.BonusAbil.HP + playObject.BonusAbil.MP + playObject.BonusAbil.Hit + playObject.BonusAbil.Speed;
            nTotlePoint = nTotlePoint + playObject.BonusPoint;
            var cMethod = questConditionInfo.sParam1[0];
            var nCount = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam2, ref nCount);
            }
            switch (cMethod)
            {
                case '=':
                    if (nTotlePoint == nCount)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (nTotlePoint > nCount)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (nTotlePoint < nCount)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (nTotlePoint >= nCount)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private bool ConditionOfCheckHP_CheckHigh(char cMethodMax, BaseObject baseObject, long nMax)
        {
            bool success = false;
            switch (cMethodMax)
            {
                case '=':
                    if (baseObject.WAbil.MaxHP == nMax)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (baseObject.WAbil.MaxHP > nMax)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (baseObject.WAbil.MaxHP < nMax)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (baseObject.WAbil.MaxHP >= nMax)
                    {
                        success = true;
                    }
                    break;
            }
            return success;
        }

        private void ConditionOfCheckHp(BaseObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var cMethodMin = questConditionInfo.sParam1[0];
            var cMethodMax = questConditionInfo.sParam3[1];
            var nMin = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            var nMax = HUtil32.StrToInt(questConditionInfo.sParam4, -1);
            if (nMin < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam2, ref nMin);
                }
            }
            if (nMax < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam4, ref nMax);
                }
            }
            if (nMin < 0 || nMax < 0)
            {
                ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKHP);
                return;
            }
            switch (cMethodMin)
            {
                case '=':
                    if (baseObject.WAbil.HP == nMin)
                    {
                        success = ConditionOfCheckHP_CheckHigh(cMethodMin, baseObject, nMax);
                    }
                    break;

                case '>':
                    if (baseObject.WAbil.HP > nMin)
                    {
                        success = ConditionOfCheckHP_CheckHigh(cMethodMin, baseObject, nMax);
                    }
                    break;

                case '<':
                    if (baseObject.WAbil.HP < nMin)
                    {
                        success = ConditionOfCheckHP_CheckHigh(cMethodMin, baseObject, nMax);
                    }
                    break;
                default:
                    if (baseObject.WAbil.HP >= nMin)
                    {
                        success = ConditionOfCheckHP_CheckHigh(cMethodMin, baseObject, nMax);
                    }
                    break;
            }
        }

        public bool ConditionOfCheckMP_CheckHigh(char cMethodMax, BaseObject baseObject, long nMax)
        {
            var success = false;
            switch (cMethodMax)
            {
                case '=':
                    if (baseObject.WAbil.MaxMP == nMax)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (baseObject.WAbil.MaxMP > nMax)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (baseObject.WAbil.MaxMP < nMax)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (baseObject.WAbil.MaxMP >= nMax)
                    {
                        success = true;
                    }
                    break;
            }
            return success;
        }

        private void ConditionOfCheckMp(BaseObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var cMethodMin = questConditionInfo.sParam1[0];
            var cMethodMax = questConditionInfo.sParam3[1];
            var nMin = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            var nMax = HUtil32.StrToInt(questConditionInfo.sParam4, -1);
            if (nMin < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam2, ref nMin);
                }
            }
            if (nMax < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam4, ref nMax);
                }
            }
            if (nMin < 0 || nMax < 0)
            {
                ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKMP);
                return;
            }
            switch (cMethodMin)
            {
                case '=':
                    if (baseObject.WAbil.MP == nMin)
                    {
                        success = ConditionOfCheckMP_CheckHigh(cMethodMax, baseObject, nMax);
                    }
                    break;

                case '>':
                    if (baseObject.WAbil.MP > nMin)
                    {
                        success = ConditionOfCheckMP_CheckHigh(cMethodMax, baseObject, nMax);
                    }
                    break;

                case '<':
                    if (baseObject.WAbil.MP < nMin)
                    {
                        success = ConditionOfCheckMP_CheckHigh(cMethodMax, baseObject, nMax);
                    }
                    break;
                default:
                    if (baseObject.WAbil.MP >= nMin)
                    {
                        success = ConditionOfCheckMP_CheckHigh(cMethodMax, baseObject, nMax);
                    }
                    break;
            }
        }

        private bool ConditionOfCheckDC_CheckHigh(char cMethodMax, BaseObject baseObject, long nMax)
        {
            bool success = false;
            switch (cMethodMax)
            {
                case '=':
                    if (HUtil32.HiWord(baseObject.WAbil.DC) == nMax)
                    {
                        success = true;
                    }
                    break;
                case '>':
                    if (HUtil32.HiWord(baseObject.WAbil.DC) > nMax)
                    {
                        success = true;
                    }
                    break;
                case '<':
                    if (HUtil32.HiWord(baseObject.WAbil.DC) < nMax)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (HUtil32.HiWord(baseObject.WAbil.DC) >= nMax)
                    {
                        success = true;
                    }
                    break;
            }
            return success;
        }

        private void ConditionOfCheckDc(BaseObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var cMethodMin = questConditionInfo.sParam1[0];
            var cMethodMax = questConditionInfo.sParam3[1];
            var nMin = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            var nMax = HUtil32.StrToInt(questConditionInfo.sParam4, -1);
            if (nMin < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam2, ref nMin);
                }
            }
            if (nMax < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam4, ref nMax);
                }
            }
            if (nMin < 0 || nMax < 0)
            {
                ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKDC);
                return;
            }
            switch (cMethodMin)
            {
                case '=':
                    if (HUtil32.LoWord(baseObject.WAbil.DC) == nMin)
                    {
                        success = ConditionOfCheckDC_CheckHigh(cMethodMin, baseObject, nMax);
                    }
                    break;

                case '>':
                    if (HUtil32.LoWord(baseObject.WAbil.DC) > nMin)
                    {
                        success = ConditionOfCheckDC_CheckHigh(cMethodMin, baseObject, nMax);
                    }
                    break;

                case '<':
                    if (HUtil32.LoWord(baseObject.WAbil.DC) < nMin)
                    {
                        success = ConditionOfCheckDC_CheckHigh(cMethodMin, baseObject, nMax);
                    }
                    break;
                default:
                    if (HUtil32.LoWord(baseObject.WAbil.DC) >= nMin)
                    {
                        success = ConditionOfCheckDC_CheckHigh(cMethodMin, baseObject, nMax);
                    }
                    break;
            }
            success = false;
        }

        public bool ConditionOfCheckMC_CheckHigh(char cMethodMax, BaseObject baseObject, long nMax)
        {
            var success = false;
            switch (cMethodMax)
            {
                case '=':
                    if (HUtil32.HiWord(baseObject.WAbil.MC) == nMax)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (HUtil32.HiWord(baseObject.WAbil.MC) > nMax)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (HUtil32.HiWord(baseObject.WAbil.MC) < nMax)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (HUtil32.HiWord(baseObject.WAbil.MC) >= nMax)
                    {
                        success = true;
                    }
                    break;
            }
            return success;
        }

        private bool ConditionOfCheckSC_CheckHigh(char cMethodMax, BaseObject baseObject, long nMax)
        {
            var success = false;
            switch (cMethodMax)
            {
                case '=':
                    if (HUtil32.HiWord(baseObject.WAbil.SC) == nMax)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (HUtil32.HiWord(baseObject.WAbil.SC) > nMax)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (HUtil32.HiWord(baseObject.WAbil.SC) < nMax)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (HUtil32.HiWord(baseObject.WAbil.SC) >= nMax)
                    {
                        success = true;
                    }
                    break;
            }
            return success;
        }

        private void ConditionOfCheckSc(BaseObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var cMethodMin = questConditionInfo.sParam1[0];
            var cMethodMax = questConditionInfo.sParam3[1];
            var nMin = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            var nMax = HUtil32.StrToInt(questConditionInfo.sParam4, -1);
            if (nMin < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam2, ref nMin);
                }
            }
            if (nMax < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam4, ref nMax);
                }
            }
            if (nMin < 0 || nMax < 0)
            {
                ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKSC);
                return;
            }
            switch (cMethodMin)
            {
                case '=':
                    if (HUtil32.LoWord(baseObject.WAbil.SC) == nMin)
                    {
                        success = ConditionOfCheckSC_CheckHigh(cMethodMin, baseObject, nMax);
                    }
                    break;

                case '>':
                    if (HUtil32.LoWord(baseObject.WAbil.SC) > nMin)
                    {
                        success = ConditionOfCheckSC_CheckHigh(cMethodMin, baseObject, nMax);
                    }
                    break;

                case '<':
                    if (HUtil32.LoWord(baseObject.WAbil.SC) < nMin)
                    {
                        success = ConditionOfCheckSC_CheckHigh(cMethodMin, baseObject, nMax);
                    }
                    break;
                default:
                    if (HUtil32.LoWord(baseObject.WAbil.SC) >= nMin)
                    {
                        success = ConditionOfCheckSC_CheckHigh(cMethodMin, baseObject, nMax);
                    }
                    break;
            }
        }

        private void ConditionOfCheckExp(BaseObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var dwExp = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (dwExp < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam2, ref dwExp);
                }
            }
            if (dwExp < 0)
            {
                ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKEXP);
                return;
            }
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (baseObject.Abil.Exp == dwExp)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (baseObject.Abil.Exp > dwExp)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (baseObject.Abil.Exp < dwExp)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (baseObject.Abil.Exp >= dwExp)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckFlourishPoint(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var nPoint = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nPoint < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam2, ref nPoint);
                if (nPoint < 0)
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKFLOURISHPOINT);
                    return;
                }
            }
            if (playObject.MyGuild == null)
            {
                return;
            }
            var guild = playObject.MyGuild;
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (guild.Flourishing == nPoint)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (guild.Flourishing > nPoint)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (guild.Flourishing < nPoint)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (guild.Flourishing >= nPoint)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckChiefItemCount(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            long nCount = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKFLOURISHPOINT);
                return;
            }
            if (playObject.MyGuild == null)
            {
                return;
            }
            GuildInfo guild = playObject.MyGuild;
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (guild.ChiefItemCount == nCount)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (guild.ChiefItemCount > nCount)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (guild.ChiefItemCount < nCount)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (guild.ChiefItemCount >= nCount)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckGuildAuraePoint(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var nPoint = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nPoint < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam2, ref nPoint);
                if (nPoint < 0)
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKAURAEPOINT);
                    return;
                }
            }
            if (playObject.MyGuild == null)
            {
                return;
            }
            var guild = playObject.MyGuild;
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (guild.Aurae == nPoint)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (guild.Aurae > nPoint)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (guild.Aurae < nPoint)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (guild.Aurae >= nPoint)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckGuildBuildPoint(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var nPoint = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nPoint < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam2, ref nPoint);
                if (nPoint < 0)
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKBUILDPOINT);
                    return;
                }
            }
            if (playObject.MyGuild == null)
            {
                return;
            }
            var guild = playObject.MyGuild;
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (guild.BuildPoint == nPoint)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (guild.BuildPoint > nPoint)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (guild.BuildPoint < nPoint)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (guild.BuildPoint >= nPoint)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckStabilityPoint(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var nPoint = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nPoint < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam2, ref nPoint);
                if (nPoint < 0)
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKSTABILITYPOINT);
                    return;
                }
            }
            if (playObject.MyGuild == null)
            {
                return;
            }
            var guild = playObject.MyGuild;
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (guild.Stability == nPoint)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (guild.Stability > nPoint)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (guild.Stability < nPoint)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (guild.Stability >= nPoint)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckGameGold(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var nGameGold = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nGameGold < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam2, ref nGameGold);
                if (nGameGold < 0)
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKGAMEGOLD);
                    return;
                }
            }
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (playObject.GameGold == nGameGold)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (playObject.GameGold > nGameGold)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (playObject.GameGold < nGameGold)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (playObject.GameGold >= nGameGold)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckGamePoint(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var nGamePoint = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nGamePoint < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam2, ref nGamePoint);
                if (nGamePoint < 0)
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKGAMEPOINT);
                    return;
                }
            }
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (playObject.GamePoint == nGamePoint)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (playObject.GamePoint > nGamePoint)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (playObject.GamePoint < nGamePoint)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (playObject.GamePoint >= nGamePoint)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckGroupCount(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (playObject.GroupOwner == 0)
            {
                return;
            }
            var nCount = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam2, ref nCount);
                if (nCount < 0)
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKGROUPCOUNT);
                    return;
                }
            }
            var cMethod = questConditionInfo.sParam1[0];
            var groupOwner = M2Share.ActorMgr.Get<PlayObject>(playObject.GroupOwner);
            switch (cMethod)
            {
                case '=':
                    if (groupOwner.GroupMembers.Count == nCount)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (groupOwner.GroupMembers.Count > nCount)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (groupOwner.GroupMembers.Count < nCount)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (groupOwner.GroupMembers.Count >= nCount)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckHaveGuild(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = playObject.MyGuild != null;
        }

        private void ConditionOfCheckIsAttackGuild(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (playObject.Castle == null)
            {
                ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_ISATTACKGUILD);
                return;
            }
            if (playObject.MyGuild == null)
            {
                return;
            }
            success = playObject.Castle.IsAttackGuild(playObject.MyGuild);
        }

        /// <summary>
        /// 检查占领沙巴克天数
        /// 格式：CASTLECHANGEDAY 空字符（>） (天数)7 
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <returns></returns>
        private void ConditionOfCheckCastleChageDay(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var nDay = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nDay < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam2, ref nDay);
            }
            if (nDay < 0 || playObject.Castle == null)
            {
                ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CASTLECHANGEDAY);
                return;
            }
            var nChangeDay = HUtil32.GetDayCount(DateTime.Now, playObject.Castle.ChangeDate);
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nChangeDay == nDay)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (nChangeDay > nDay)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (nChangeDay < nDay)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (nChangeDay >= nDay)
                    {
                        success = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 检查上次攻城到现在的天数
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        private void ConditionOfCheckCastleWarDay(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var nDay = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nDay < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam2, ref nDay);
            }
            if (nDay < 0 || playObject.Castle == null)
            {
                ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CASTLEWARDAY);
                return;
            }
            var nWarDay = HUtil32.GetDayCount(DateTime.Now, playObject.Castle.WarDate);
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nWarDay == nDay)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (nWarDay > nDay)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (nWarDay < nDay)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (nWarDay >= nDay)
                    {
                        success = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 检查沙巴克城门状态
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        private void ConditionOfCheckCastleDoorStatus(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var nDay = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nDay < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam2, ref nDay);
            }
            var nDoorStatus = -1;
            if (string.Compare(questConditionInfo.sParam1, "损坏", StringComparison.Ordinal) == 0)
            {
                nDoorStatus = 0;
            }
            if (string.Compare(questConditionInfo.sParam1, "开启", StringComparison.Ordinal) == 0)
            {
                nDoorStatus = 1;
            }
            if (string.Compare(questConditionInfo.sParam1, "关闭", StringComparison.Ordinal) == 0)
            {
                nDoorStatus = 2;
            }
            if (nDay < 0 || playObject.Castle == null || nDoorStatus < 0)
            {
                ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKCASTLEDOOR);
                return;
            }
            CastleDoor castleDoor = (CastleDoor)playObject.Castle.MainDoor.BaseObject;
            switch (nDoorStatus)
            {
                case 0:
                    if (castleDoor.Death)
                    {
                        success = true;
                    }
                    break;
                case 1:
                    if (castleDoor.IsOpened)
                    {
                        success = true;
                    }
                    break;
                case 2:
                    if (!castleDoor.IsOpened)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckIsAttackAllyGuild(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (playObject.Castle == null)
            {
                ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_ISATTACKALLYGUILD);
                return;
            }
            if (playObject.MyGuild == null)
            {
                return;
            }
            success = playObject.Castle.IsAttackAllyGuild(playObject.MyGuild);
        }

        /// <summary>
        /// 检查当前行会是否为守城方联盟行会
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        private void ConditionOfCheckIsDefenseAllyGuild(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (playObject.Castle == null)
            {
                ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_ISDEFENSEALLYGUILD);
                return;
            }
            if (playObject.MyGuild == null)
            {
                return;
            }
            success = playObject.Castle.IsDefenseAllyGuild(playObject.MyGuild);
        }

        /// <summary>
        /// 检查当前行会是否为守城方行会
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        private void ConditionOfCheckIsDefenseGuild(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (playObject.Castle == null)
            {
                ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_ISDEFENSEGUILD);
                return;
            }
            if (playObject.MyGuild == null)
            {
                return;
            }
            success = playObject.Castle.IsDefenseGuild(playObject.MyGuild);
        }

        /// <summary>
        /// 检查当前行会是否属于沙巴克
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        private void ConditionOfCheckIsCastleaGuild(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (M2Share.CastleMgr.IsCastleMember(playObject) != null)
            {
                success = true;
            }
        }

        /// <summary>
        /// 检查玩家是否是沙巴克城主
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        private void ConditionOfCheckIsCastleMaster(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (playObject.IsGuildMaster() && M2Share.CastleMgr.IsCastleMember(playObject) != null)
            {
                success = true;
            }
        }

        /// <summary>
        /// 检查玩家是否是行会掌门人
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        private void ConditionOfCheckIsGuildMaster(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = playObject.IsGuildMaster();
        }

        private void ConditionOfCheckIsMaster(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (playObject.MasterName != "" && playObject.IsMaster)
            {
                success = true;
            }
        }

        private void ConditionOfCheckListCount(BaseObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
        }

        private bool ConditionOfCheckItemType(PlayObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            UserItem userItem = null;
            var result = false;
            var nWhere = HUtil32.StrToInt(questConditionInfo.sParam1, -1);
            var nType = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (baseObject.Race == ActorRace.Play)
            {
                if (nWhere < 0)
                {
                    GetVarValue(baseObject, questConditionInfo.sParam1, ref nWhere);
                }
                if (nType < 0)
                {
                    GetVarValue(baseObject, questConditionInfo.sParam2, ref nType);
                }
            }
            if (!(nWhere >= baseObject.UseItems.GetLowerBound(0) && nWhere <= baseObject.UseItems.GetUpperBound(0)))
            {
                ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKITEMTYPE);
                return false;
            }
            userItem = baseObject.UseItems[nWhere];
            if (userItem.Index == 0)
            {
                return false;
            }
            var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
            if (stdItem != null && stdItem.StdMode == nType)
            {
                result = true;
            }
            return result;
        }

        private void ConditionOfCheckLevelEx(BaseObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var nLevel = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam2, ref nLevel);
                    if (nLevel < 0)
                    {
                        ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKLEVELEX);
                        return;
                    }
                }
            }
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (baseObject.Abil.Level == nLevel)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (baseObject.Abil.Level > nLevel)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (baseObject.Abil.Level < nLevel)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (baseObject.Abil.Level >= nLevel)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckNameListPostion(BaseObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            string sChrName;
            string sLine;
            string sVar = string.Empty;
            string sValue = string.Empty;
            int nValue = 0;
            VarInfo varInfo;
            var nNamePostion = -1;
            var loadList = new StringList();
            try
            {
                sChrName = baseObject.ChrName;
                if (File.Exists(M2Share.Config.EnvirDir + _mSPath + questConditionInfo.sParam1))
                {
                    loadList.LoadFromFile(M2Share.Config.EnvirDir + _mSPath + questConditionInfo.sParam1);
                    for (int I = 0; I < loadList.Count; I++)
                    {
                        sLine = loadList[I].Trim();
                        if (sLine == "" || sLine[1] == ';')
                        {
                            continue;
                        }
                        if (string.Compare(sLine, sChrName, StringComparison.Ordinal) == 0)
                        {
                            nNamePostion = I + 1;
                            break;
                        }
                    }
                }
                else
                {
                    ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKNAMELISTPOSITION);
                }
            }
            finally
            {
                Dispose(loadList);
            }
            var cMethod = questConditionInfo.sParam2[1];
            long nPostion = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
            if (questConditionInfo.sParam4 != "" && baseObject.Race == ActorRace.Play)
            {
                varInfo = GetVarValue((PlayObject)baseObject, questConditionInfo.sParam4, ref sVar, ref sValue, ref nValue);
                switch (varInfo.VarAttr)
                {
                    case VarAttr.aNone:
                    case VarAttr.aConst:
                        ScriptConditionError((PlayObject)baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKNAMELISTPOSITION);
                        return;
                    case VarAttr.aFixStr:
                        SetValNameValue((PlayObject)baseObject, sVar, sValue, nNamePostion);
                        break;

                    case VarAttr.aDynamic:
                        SetDynamicValue((PlayObject)baseObject, sVar, sValue, nNamePostion);
                        break;
                }
            }
            if (nPostion < 0)
            {
                ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKNAMELISTPOSITION);
                return;
            }
            switch (cMethod)
            {
                case '=':
                    if (nNamePostion == nPostion)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (nNamePostion > nPostion)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (nNamePostion < nPostion)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (nNamePostion >= nPostion)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckBagItemInList(BaseObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            string sLineText;
            string sVar = string.Empty;
            string sValue = string.Empty;
            int nValue = 0;
            var sListFileName = M2Share.Config.EnvirDir + _mSPath + questConditionInfo.sParam1;
            var nItemCount = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            var loadList = new StringList();
            if (File.Exists(sListFileName))
            {
                try
                {
                    loadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    M2Share.Logger.Error("loading fail.... => " + sListFileName);
                }
            }
            else
            {
                M2Share.Logger.Info("file not found => " + sListFileName);
            }
            if (nItemCount < 0 && baseObject.Race == ActorRace.Play)
            {
                GetVarValue((PlayObject)baseObject, questConditionInfo.sParam2, ref nItemCount);
            }
            if (nItemCount < 0)
            {
                ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKBAGITEMINLIST);
                return;
            }
            for (int i = 0; i < loadList.Count; i++)
            {
                sLineText = loadList[i].Trim();
                if (sLineText == "" || sLineText[1] == ';')
                {
                    continue;
                }
                var sItemName = loadList[i];
                if (nItemCount <= 0)
                {
                    break;
                }
                for (int j = 0; j < baseObject.ItemList.Count; j++)
                {
                    var userItem = baseObject.ItemList[j];
                    if (string.Compare(M2Share.WorldEngine.GetStdItemName(userItem.Index), sItemName, StringComparison.Ordinal) == 0)
                    {
                        nItemCount -= 1;
                        if (nItemCount <= 0)
                        {
                            break;
                        }
                    }
                }
            }
            if (nItemCount > 0)
            {
                if (questConditionInfo.sParam3 != "" && baseObject.Race == ActorRace.Play)
                {
                    VarInfo varInfo = GetVarValue((PlayObject)baseObject, questConditionInfo.sParam3, ref sVar, ref sValue, ref nValue);
                    switch (varInfo.VarAttr)
                    {
                        case VarAttr.aNone:
                        case VarAttr.aConst:
                            ScriptConditionError((PlayObject)baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKBAGITEMINLIST);
                            break;
                        case VarAttr.aFixStr:
                            SetValNameValue((PlayObject)baseObject, sVar, sValue, nItemCount);
                            break;
                        case VarAttr.aDynamic:
                            SetDynamicValue((PlayObject)baseObject, sVar, sValue, nItemCount);
                            break;
                    }
                }
                success = false;
            }
            else
            {
                success = true;
            }
            Dispose(loadList);
        }

        private void ConditionOfCheckMarry(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (playObject.DearName != "")
            {
                success = true;
            }
        }

        private void ConditionOfCheckMarryCount(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var nCount = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam2, ref nCount);
                if (nCount < 0)
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKMARRYCOUNT);
                    return;
                }
            }
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (playObject.MarryCount == nCount)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (playObject.MarryCount > nCount)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (playObject.MarryCount < nCount)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (playObject.MarryCount >= nCount)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckMaster(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (playObject.MasterName != "" && !playObject.IsMaster)
            {
                success = true;
            }
        }

        private void ConditionOfCheckMemBerLevel(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var nLevel = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam2, ref nLevel);
                if (nLevel < 0)
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKMEMBERLEVEL);
                    return;
                }
            }
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (playObject.MemberLevel == nLevel)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (playObject.MemberLevel > nLevel)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (playObject.MemberLevel < nLevel)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (playObject.MemberLevel >= nLevel)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckMemberType(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var nType = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nType < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam2, ref nType);
                if (nType < 0)
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKMEMBERTYPE);
                    return;
                }
            }
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (playObject.MemberType == nType)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (playObject.MemberType > nType)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (playObject.MemberType < nType)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (playObject.MemberType >= nType)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckNameIpList(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            string sName = string.Empty;
            var loadList = new StringList();
            try
            {
                var sChrName = playObject.ChrName;
                var sCharIPaddr = playObject.LoginIpAddr;
                if (File.Exists(M2Share.Config.EnvirDir + _mSPath + questConditionInfo.sParam1))
                {
                    loadList.LoadFromFile(M2Share.Config.EnvirDir + _mSPath + questConditionInfo.sParam1);
                    for (int i = 0; i < loadList.Count; i++)
                    {
                        var sLine = loadList[i];
                        if (sLine[0] == ';')
                        {
                            continue;
                        }
                        var sIPaddr = HUtil32.GetValidStr3(sLine, ref sName, new string[] { " ", "/", "\09" });
                        sIPaddr = sIPaddr.Trim();
                        if (sName == sChrName && sIPaddr == sCharIPaddr)
                        {
                            success = true;
                            break;
                        }
                    }
                }
                else
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKNAMEIPLIST);
                }
            }
            finally
            {
                Dispose(loadList);
            }
        }

        private void ConditionOfCheckPoseDir(PlayObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var poseHuman = (PlayObject)baseObject.GetPoseCreate();
            if (poseHuman != null && poseHuman.GetPoseCreate() == baseObject)
            {
                switch (questConditionInfo.nParam1)
                {
                    case 1: // 要求相同性别
                        if (poseHuman.Gender == baseObject.Gender)
                        {
                            success = true;
                        }
                        break;
                    case 2: // 要求不同性别
                        if (poseHuman.Gender != baseObject.Gender)
                        {
                            success = true;
                        }
                        break;
                    default:// 无参数时不判别性别
                        success = true;
                        break;
                }
            }
        }

        private void ConditionOfCheckPoseGender(BaseObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            byte btSex = 0;
            if (string.Compare(questConditionInfo.sParam1, "MAN", StringComparison.Ordinal) == 0)
            {
                btSex = 0;
            }
            else if (string.Compare(questConditionInfo.sParam1, "男", StringComparison.Ordinal) == 0)
            {
                btSex = 0;
            }
            else if (string.Compare(questConditionInfo.sParam1, "WOMAN", StringComparison.Ordinal) == 0)
            {
                btSex = 1;
            }
            else if (string.Compare(questConditionInfo.sParam1, "女", StringComparison.Ordinal) == 0)
            {
                btSex = 1;
            }
            var poseHuman = (PlayObject)baseObject.GetPoseCreate();
            if (poseHuman != null)
            {
                if (poseHuman.Gender == (PlayGender)btSex)
                {
                    success = true;
                }
            }
        }

        private void ConditionOfCheckPoseIsMaster(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            BaseObject poseHuman = playObject.GetPoseCreate();
            if (poseHuman != null && poseHuman.Race == ActorRace.Play)
            {
                if (((PlayObject)poseHuman).MasterName != "" && ((PlayObject)poseHuman).IsMaster)
                {
                    success = true;
                }
            }
        }

        private void ConditionOfCheckPoseLevel(BaseObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var nLevel = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam2, ref nLevel);
                    if (nLevel < 0)
                    {
                        ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKPOSELEVEL);
                        return;
                    }
                }
                else
                {
                    ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKPOSELEVEL);
                    return;
                }
            }
            var cMethod = questConditionInfo.sParam1[0];
            BaseObject poseHuman = baseObject.GetPoseCreate();
            if (poseHuman != null)
            {
                switch (cMethod)
                {
                    case '=':
                        if (poseHuman.Abil.Level == nLevel)
                        {
                            success = true;
                        }
                        break;

                    case '>':
                        if (poseHuman.Abil.Level > nLevel)
                        {
                            success = true;
                        }
                        break;

                    case '<':
                        if (poseHuman.Abil.Level < nLevel)
                        {
                            success = true;
                        }
                        break;
                    default:
                        if (poseHuman.Abil.Level >= nLevel)
                        {
                            success = true;
                        }
                        break;
                }
            }
        }

        private void ConditionOfCheckPoseMarry(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            BaseObject poseHuman = playObject.GetPoseCreate();
            if (poseHuman != null && poseHuman.Race == ActorRace.Play)
            {
                if (((PlayObject)poseHuman).DearName != "")
                {
                    success = true;
                }
            }
        }

        private void ConditionOfCheckPoseMaster(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            BaseObject poseHuman = playObject.GetPoseCreate();
            if (poseHuman != null && poseHuman.Race == ActorRace.Play)
            {
                if (((PlayObject)poseHuman).MasterName != "" && !((PlayObject)poseHuman).IsMaster)
                {
                    success = true;
                }
            }
        }

        private void ConditionOfCheckServerName(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (questConditionInfo.sParam1 == M2Share.Config.ServerName)
            {
                success = true;
            }
        }

        private void ConditionOfCheckSlaveCount(BaseObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var nCount = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam2, ref nCount);
                    if (nCount < 0)
                    {
                        ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKSLAVECOUNT);
                        return;
                    }
                }
                else
                {
                    ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKSLAVECOUNT);
                    return;
                }
            }
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (baseObject.SlaveList.Count == nCount)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (baseObject.SlaveList.Count > nCount)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (baseObject.SlaveList.Count < nCount)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (baseObject.SlaveList.Count >= nCount)
                    {
                        success = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 检查人物是否在安全区域内
        /// </summary>
        /// <param name="baseObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckSafeZone(BaseObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = baseObject.InSafeZone();
        }

        private void ConditionOfCheckMapName(PlayObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var sChrName = baseObject.ChrName;
            var sMapName = questConditionInfo.sParam1;
            if (M2Share.MapMgr.FindMap(sMapName) == null)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue(baseObject, questConditionInfo.sParam1, ref sMapName);
                }
            }
            if (sMapName == baseObject.MapName)
            {
                success = true;
            }
        }

        private bool ConditionOfCheckSkill(PlayObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            var result = false;
            var nSkillLevel = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
            if (nSkillLevel < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue(baseObject, questConditionInfo.sParam3, ref nSkillLevel);
                    if (nSkillLevel < 0)
                    {
                        ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sCHECKSKILL);
                        return false;
                    }
                }
            }
            if (baseObject.Race == ActorRace.Play)
            {
                var userMagic = baseObject.GetMagicInfo(questConditionInfo.sParam1);
                if (userMagic == null)
                {
                    return false;
                }
                var cMethod = questConditionInfo.sParam2[1];
                switch (cMethod)
                {
                    case '=':
                        if (userMagic.Level == nSkillLevel)
                        {
                            success = true;
                        }
                        break;

                    case '>':
                        if (userMagic.Level > nSkillLevel)
                        {
                            success = true;
                        }
                        break;

                    case '<':
                        if (userMagic.Level < nSkillLevel)
                        {
                            success = true;
                        }
                        break;
                    default:
                        if (userMagic.Level >= nSkillLevel)
                        {
                            success = true;
                        }
                        break;
                }
            }
            return result;
        }

        private void ConditionOfAnsiContainsText(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var sValue1 = questConditionInfo.sParam1;
            var sValue2 = questConditionInfo.sParam2;
            sValue1 = GetLineVariableText(playObject, questConditionInfo.sParam1);
            sValue2 = GetLineVariableText(playObject, questConditionInfo.sParam2);
            GetVarValue(playObject, sValue1, ref sValue1);
            GetVarValue(playObject, sValue2, ref sValue2);
            success = sValue1.IndexOf(sValue2, StringComparison.OrdinalIgnoreCase) != -1;
        }

        private void ConditionOfCompareText(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var sValue1 = GetLineVariableText(playObject, questConditionInfo.sParam1);
            var sValue2 = GetLineVariableText(playObject, questConditionInfo.sParam2);
            GetVarValue(playObject, sValue1, ref sValue1);
            GetVarValue(playObject, sValue2, ref sValue2);
            success = string.Compare(sValue1, sValue2, StringComparison.OrdinalIgnoreCase) == 0;
        }

        private void ConditionOfCheckPkPoint(PlayObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            var pvpPoint = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (pvpPoint < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue(baseObject, questConditionInfo.sParam2, ref pvpPoint);
                }
            }
            if (pvpPoint < 0)
            {
                ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKPKPOINTEX);
                return;
            }
            var cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (baseObject.PvpLevel() == pvpPoint)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (baseObject.PvpLevel() > pvpPoint)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (baseObject.PvpLevel() < pvpPoint)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (baseObject.PvpLevel() >= pvpPoint)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfIsUnderWar(BaseObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            UserCastle userCastle;
            string sCastleName = questConditionInfo.sParam1;
            if (sCastleName != "")
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                try
                {
                    for (int i = 0; i < M2Share.CastleMgr.CastleList.Count; i++)
                    {
                        userCastle = M2Share.CastleMgr.CastleList[i];
                        if (string.Compare(userCastle.sName, sCastleName, StringComparison.Ordinal) == 0)
                        {
                            success = userCastle.UnderWar;
                            break;
                        }
                    }
                }
                finally
                {
                    HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
                }
            }
        }

        private void ConditionOfInCastleWarArea(BaseObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            try
            {
                success = M2Share.CastleMgr.InCastleWarArea(baseObject.Envir, baseObject.CurrX, baseObject.CurrY) != null;
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
        }

        private void ConditionOfCheckInCurrRect(BaseObject baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nX = HUtil32.StrToInt(questConditionInfo.sParam1, -1);
            int nY = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            int nRange = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
            if (nX < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam1, ref nX);
                }
            }
            if (nX < 0)
            {
                ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKINCURRRECT);
                return;
            }
            if (nY < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam2, ref nY);
                }
            }
            if (nY < 0)
            {
                ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKINCURRRECT);
                return;
            }
            if (nRange < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue((PlayObject)baseObject, questConditionInfo.sParam3, ref nRange);
                }
            }
            if (nRange < 0)
            {
                ScriptConditionError(baseObject, questConditionInfo, CheckScriptDef.sSC_CHECKINCURRRECT);
                return;
            }
            var nLeft = HUtil32._MAX(nX - nRange, 0);
            var nRight = HUtil32._MIN(nX + nRange, baseObject.Envir.Width);
            var nTop = HUtil32._MAX(nY - nRange, 0);
            var nBottom = HUtil32._MIN(nY + nRange, baseObject.Envir.Height);
            var currRect = new TRect(nLeft, nTop, nRight, nBottom);
            success = baseObject.CurrX >= currRect.Left && baseObject.CurrY >= currRect.Top && baseObject.CurrX <= currRect.Right && baseObject.CurrY <= currRect.Bottom;
        }

        private void ConditionOfCheckGuildMember(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (playObject.MyGuild != null)
            {
                var sChrName = questConditionInfo.sParam1;
                sChrName = GetLineVariableText(playObject, sChrName);
                GetVarValue(playObject, sChrName, ref sChrName);
                success = playObject.MyGuild.IsMember(sChrName);
            }
        }

        private void ConditionOfIndexOf(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            string sVar = string.Empty;
            string sValue = string.Empty;
            int nValue = 0;
            var nPostion = -1;
            GetVarValue(playObject, questConditionInfo.sParam2, ref sValue);
            var loadList = new StringList();
            try
            {
                if (File.Exists(M2Share.Config.EnvirDir + _mSPath + questConditionInfo.sParam1))
                {
                    loadList.LoadFromFile(M2Share.Config.EnvirDir + _mSPath + questConditionInfo.sParam1);
                    nPostion = loadList.IndexOf(sValue);
                    success = nPostion >= 0;
                }
                else
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_INDEXOF);
                }
            }
            finally
            {
                Dispose(loadList);
            }
            if (questConditionInfo.sParam3 != "")
            {
                var varInfo = GetVarValue(playObject, questConditionInfo.sParam3, ref sVar, ref sValue, ref nValue);
                switch (varInfo.VarAttr)
                {
                    case VarAttr.aNone:
                    case VarAttr.aConst:
                        ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_INDEXOF);
                        break;
                    case VarAttr.aFixStr:
                        SetValNameValue(playObject, sVar, sValue, nPostion);
                        break;
                    case VarAttr.aDynamic:
                        SetDynamicValue(playObject, sVar, sValue, nPostion);
                        break;
                }
            }
            else
            {
                ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_INDEXOF);
            }
        }

        /// <summary>
        /// 检查指定变量是否大于指定参数
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfLapge(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int n14 = 0;
            int n18 = 0;
            if (CheckVarNameNo(playObject, questConditionInfo, n14, n18) && n14 > n18)
            {
                success = true;
            }
        }

        /// <summary>
        /// 检查指定变量是否小于指定参数
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfSmall(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int n14 = 0;
            int n18 = 0;
            if (CheckVarNameNo(playObject, questConditionInfo, n14, n18) && n14 < n18)
            {
                success = true;
            }
        }

        /// <summary>
        /// 检查玩家权限是否大于4
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfIssysop(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            if (!(playObject.Permission >= 4))
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查玩家权限是否大于6
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfIsAdmin(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            if (!(playObject.Permission >= 6))
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查小时是否相等
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfHour(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            if (questConditionInfo.nParam1 != 0 && questConditionInfo.nParam2 == 0)
            {
                questConditionInfo.nParam2 = questConditionInfo.nParam1;
            }
            int hour = DateTime.Now.Hour;
            int min = DateTime.Now.Minute;
            int sec = DateTime.Now.Second;
            int mSec = DateTime.Now.Millisecond;
            if (hour < questConditionInfo.nParam1 || hour > questConditionInfo.nParam2)
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查分钟是否相等
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfMin(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            if (questConditionInfo.nParam1 != 0 && questConditionInfo.nParam2 == 0)
            {
                questConditionInfo.nParam2 = questConditionInfo.nParam1;
            }
            int hour = DateTime.Now.Hour;
            int min = DateTime.Now.Minute;
            int sec = DateTime.Now.Second;
            int mSec = DateTime.Now.Millisecond;
            if (min < questConditionInfo.nParam1 || min > questConditionInfo.nParam2)
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查日期是否相等
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfDayOfWeek(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            if (HUtil32.CompareLStr(questConditionInfo.sParam1, "SUN", 3))
            {
                if (DateTime.Now.DayOfWeek.ToString() != "1")
                {
                    success = false;
                }
            }
            if (HUtil32.CompareLStr(questConditionInfo.sParam1, "MON", 3))
            {
                if (DateTime.Now.DayOfWeek.ToString() != "2")
                {
                    success = false;
                }
            }
            if (HUtil32.CompareLStr(questConditionInfo.sParam1, "TUE", 3))
            {
                if (DateTime.Now.DayOfWeek.ToString() != "3")
                {
                    success = false;
                }
            }
            if (HUtil32.CompareLStr(questConditionInfo.sParam1, "WED", 3))
            {
                if (DateTime.Now.DayOfWeek.ToString() != "4")
                {
                    success = false;
                }
            }
            if (HUtil32.CompareLStr(questConditionInfo.sParam1, "THU", 3))
            {
                if (DateTime.Now.DayOfWeek.ToString() != "5")
                {
                    success = false;
                }
            }
            if (HUtil32.CompareLStr(questConditionInfo.sParam1, "FRI", 3))
            {
                if (DateTime.Now.DayOfWeek.ToString() != "6")
                {
                    success = false;
                }
            }
            if (HUtil32.CompareLStr(questConditionInfo.sParam1, "SAT", 3))
            {
                if (DateTime.Now.DayOfWeek.ToString() != "7")
                {
                    success = false;
                }
            }
        }

        /// <summary>
        /// 检查物品持久
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckDura(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            var n1C = 0;
            var nMaxDura = 0;
            var nDura = 0;
            playObject.QuestCheckItem(questConditionInfo.sParam1, ref n1C, ref nMaxDura, ref nDura);
            if (HUtil32.Round(nDura / 1000) < questConditionInfo.nParam2)
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查游戏物品
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckItem(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            var n1C = 0;
            var nMaxDura = 0;
            var nDura = 0;
            var s01 = questConditionInfo.sParam1;
            GetVarValue(playObject, questConditionInfo.sParam1, ref s01);
            playObject.QuestCheckItem(s01, ref n1C, ref nMaxDura, ref nDura);
            if (n1C < questConditionInfo.nParam2)
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查玩家职业
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckJob(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            if (HUtil32.CompareLStr(questConditionInfo.sParam1, "WARRIOR", 3))
            {
                if (playObject.Job != 0)
                {
                    success = false;
                }
            }
            if (HUtil32.CompareLStr(questConditionInfo.sParam1, "WIZARD", 3))
            {
                if (playObject.Job != (PlayJob)1)
                {
                    success = false;
                }
            }
            if (HUtil32.CompareLStr(questConditionInfo.sParam1, "TAOIST", 3))
            {
                if (playObject.Job != (PlayJob)2)
                {
                    success = false;
                }
            }
        }

        /// <summary>
        /// 检查玩家等级
        /// </summary>
        /// <param name="playObject">玩家对象</param>
        /// <param name="questConditionInfo">脚本参数</param>
        /// <param name="success">返回结果</param>
        private void ConditionOfCheckLevel(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            if (playObject.Abil.Level < questConditionInfo.nParam1)
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查玩家性别
        /// </summary>
        /// <param name="playObject">玩家对象</param>
        /// <param name="questConditionInfo">脚本参数</param>
        /// <param name="success">返回结果</param>
        private void ConditionOfGender(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            if (string.Compare(questConditionInfo.sParam1, ScriptConst.sMAN, StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (playObject.Gender != 0)
                {
                    success = false;
                }
            }
            else
            {
                if (playObject.Gender != (PlayGender)1)
                {
                    success = false;
                }
            }
        }

        private void ConditionOfRandom(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            if (M2Share.RandomNumber.Random(questConditionInfo.nParam1) != 0)
            {
                success = false;
            }
        }

        private void ConditionOfCheckGold(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            var n14 = HUtil32.StrToInt(questConditionInfo.sParam1, -1);
            if (n14 < 0)
            {
                GetVarValue(playObject, questConditionInfo.sParam1, ref n14);
            }
            if (playObject.Gold < n14)
            {
                success = false;
            }
        }

        private void ConditionOfCheck(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            var n14 = HUtil32.StrToInt(questConditionInfo.sParam1, 0);
            var n18 = HUtil32.StrToInt(questConditionInfo.sParam2, 0);
            var n10 = playObject.GetQuestFalgStatus(n14);
            if (n10 == 0)
            {
                if (n18 != 0)
                {
                    success = false;
                }
            }
            else
            {
                if (n18 == 0)
                {
                    success = false;
                }
            }
        }

        /// <summary>
        /// 检查玩家是否是新玩家
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfIsNewHuman(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = playObject.IsNewHuman;
        }

        /// <summary>
        /// 检查人物名字在文件中的位置
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckNameListPostion(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            string sChrName;
            int nNamePostion = -1;
            string sLine;
            string sVar = string.Empty;
            string sValue = string.Empty;
            int nValue = 0;
            VarInfo varInfo;
            StringList loadList = null;
            try
            {
                sChrName = playObject.ChrName;
                if (File.Exists(M2Share.Config.EnvirDir + _mSPath + questConditionInfo.sParam1))
                {
                    loadList = new StringList();
                    loadList.LoadFromFile(M2Share.Config.EnvirDir + _mSPath + questConditionInfo.sParam1);
                    for (int i = 0; i < loadList.Count; i++)
                    {
                        sLine = loadList[i].Trim();
                        if (sLine == "" || sLine[1] == ';')
                        {
                            continue;
                        }
                        if (string.Compare(sLine, sChrName, StringComparison.Ordinal) == 0)
                        {
                            nNamePostion = i + 1;
                            break;
                        }
                    }
                }
                else
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKNAMELISTPOSITION);
                }
            }
            finally
            {
                Dispose(loadList);
            }
            var cMethod = questConditionInfo.sParam2[1];
            long nPostion = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
            if (questConditionInfo.sParam4 != "" && playObject.Race == ActorRace.Play)
            {
                varInfo = GetVarValue(playObject, questConditionInfo.sParam4, ref sVar, ref sValue, ref nValue);
                switch (varInfo.VarAttr)
                {
                    case VarAttr.aNone:
                    case VarAttr.aConst:
                        ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKNAMELISTPOSITION);
                        return;
                    case VarAttr.aFixStr:
                        SetValNameValue(playObject, sVar, sValue, nNamePostion);
                        break;

                    case VarAttr.aDynamic:
                        SetDynamicValue(playObject, sVar, sValue, nNamePostion);
                        break;
                }
            }
            if (nPostion < 0)
            {
                ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKNAMELISTPOSITION);
                return;
            }
            switch (cMethod)
            {
                case '=':
                    if (nNamePostion == nPostion)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (nNamePostion > nPostion)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (nNamePostion < nPostion)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (nNamePostion >= nPostion)
                    {
                        success = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 检查行会是否在列表中
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckGuildList(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            if (playObject.MyGuild != null)
            {
                if (!GotoLable_CheckStringList(playObject.MyGuild.GuildName, _mSPath + questConditionInfo.sParam1))
                {
                    success = false;
                }
            }
            else
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查宝宝的名字
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckSlaveName(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (questConditionInfo.sParam1 == "")
            {
                ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKSLAVENAME);
            }
            var sSlaveName = questConditionInfo.sParam1;
            if (playObject.Race == ActorRace.Play)
            {
                GetVarValue(playObject, questConditionInfo.sParam1, ref sSlaveName);
            }
            for (var i = 0; i < playObject.SlaveList.Count; i++)
            {
                BaseObject aObject = playObject.SlaveList[i];
                if (string.Compare(sSlaveName, aObject.ChrName, StringComparison.Ordinal) == 0)
                {
                    success = true;
                    break;
                }
            }
        }
        
        /// <summary>
        /// 检查人物身上指定物品位置是否佩带指定物品名称（为空则检查人物身上指定位置是否佩带物品）
        /// 格式:CHECKUSEITEM 物品位置(0-13) 物品名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckUseItem(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nWhere = HUtil32.StrToInt(questConditionInfo.sParam1, -1);
            if (playObject.Race == ActorRace.Play)
            {
                if (nWhere < 0)
                {
                    GetVarValue(playObject, questConditionInfo.sParam1, ref nWhere);
                }
            }
            if (nWhere < 0 || nWhere > playObject.UseItems.GetUpperBound(0))
            {
                ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKUSEITEM);
                return;
            }
            if (playObject.UseItems[nWhere].Index > 0)
            {
                success = true;
            }
        }

        /// <summary>
        /// 检查人物经验
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <returns></returns>
        private void ConditionOfCheckExp(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            try
            {
                if (questConditionInfo.sParam6 == "88")
                {
                    playObject = M2Share.WorldEngine.GetPlayObject(GetLineVariableText(playObject, questConditionInfo.sParam7));
                    if (playObject == null)
                    {
                        return;
                    }
                }
                success = false;
                var dwExp = (uint)HUtil32.StrToInt(questConditionInfo.sParam2, 0);
                if (dwExp == 0)
                {
                    ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKEXP);
                    return;
                }
                var cMethod = questConditionInfo.sParam1[0];
                switch (cMethod)
                {
                    case '=':
                        if (playObject.Abil.Exp == dwExp)
                        {
                            success = true;
                        }
                        break;
                    case '>':
                        if (playObject.Abil.Exp > dwExp)
                        {
                            success = true;
                        }
                        break;
                    case '<':
                        if (playObject.Abil.Exp < dwExp)
                        {
                            success = true;
                        }
                        break;
                    default:
                        if (playObject.Abil.Exp >= dwExp)
                        {
                            success = true;
                        }
                        break;
                }
            }
            catch
            {
                M2Share.Logger.Error("{异常} TNormNpc.ConditionOfCheckExp");
            }
        }

        /// <summary>
        /// 检测是否在同一地图范围内
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <returns></returns>
        private void ConditionOfCheckInMapRange(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            string sMapName = questConditionInfo.sParam1;
            int nX = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            int nY = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
            int nRange = HUtil32.StrToInt(questConditionInfo.sParam4, -1);
            if (playObject.Race == ActorRace.Play)
            {
                if (nX < 0)
                {
                    GetVarValue(playObject, questConditionInfo.sParam2, ref nX);
                }
                if (nY < 0)
                {
                    GetVarValue(playObject, questConditionInfo.sParam3, ref nY);
                }
                if (nRange < 0)
                {
                    GetVarValue(playObject, questConditionInfo.sParam4, ref nRange);
                }
            }
            if (sMapName == "" || nX < 0 || nY < 0 || nRange < 0)
            {
                ScriptConditionError(playObject, questConditionInfo, CheckScriptDef.sSC_CHECKINMAPRANGE);
                return;
            }
            if (string.Compare(playObject.MapName, sMapName, StringComparison.Ordinal) != 0)
            {
                return;
            }
            if (Math.Abs(playObject.CurrX - nX) <= nRange && Math.Abs(playObject.CurrY - nY) <= nRange)
            {
                success = true;
            }
        }

        /// <summary>
        /// 检查字符串是否在指定文件中
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckTextList(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            string s01 = questConditionInfo.sParam1;
            string s02 = GetLineVariableText(playObject, questConditionInfo.sParam2);// 路径支持变量
            if (s02[0] == '\\')
            {
                s02 = s02.Substring(2 - 1, s02.Length - 1);
            }
            if (s02[1] == '\\')
            {
                s02 = s02.Substring(3 - 1, s02.Length - 2);
            }
            if (s02[2] == '\\')
            {
                s02 = s02.Substring(4 - 1, s02.Length - 3);
            }
            if (!GotoLable_CheckStringList(s01, s02))
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查当前玩家是否队伍队长
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfIsGroupMaster(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            if (playObject.GroupOwner != 0)
            {
                if (playObject.GroupOwner != playObject.ActorId)
                {
                    success = false;
                }
            }
            else
            {
                success = false;
            }
        }

        /// <summary>
        /// 判断字符串包含在文件里
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheCkContAinsTextList(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            string s02 = string.Empty;
            string s01 = questConditionInfo.sParam1;
            //GetValValue(PlayObject, QuestConditionInfo.sParam1, ref s01);
            if (questConditionInfo.sParam2[0] == '\\')
            {
                s02 = questConditionInfo.sParam2.Substring(2 - 1, questConditionInfo.sParam2.Length - 1);
            }
            if (questConditionInfo.sParam2[1] == '\\')
            {
                s02 = questConditionInfo.sParam2.Substring(3 - 1, questConditionInfo.sParam2.Length - 2);
            }
            if (questConditionInfo.sParam2[2] == '\\')
            {
                s02 = questConditionInfo.sParam2.Substring(4 - 1, questConditionInfo.sParam2.Length - 3);
            }
            s02 = GetLineVariableText(playObject, s02);
            if (!GotoLable_CheckAnsiContainsTextList(s01, s02))
            {
                success = false;
            }
        }

        private bool GotoLable_CheckAnsiContainsTextList(string sTest, string sListFileName)
        {
            bool success = false;
            StringList loadList;
            sListFileName = M2Share.Config.EnvirDir + sListFileName;
            if (File.Exists(sListFileName))
            {
                loadList = new StringList();
                try
                {
                    loadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    M2Share.Logger.Error("loading fail.... => " + sListFileName);
                }
                for (int I = 0; I < loadList.Count; I++)
                {
                    if (sTest.IndexOf(loadList[I].Trim()) != -1)
                    {
                        success = true;
                        break;
                    }
                }
                Dispose(loadList);
            }
            else
            {
                M2Share.Logger.Error("file not found => " + sListFileName);
            }
            return success;
        }

        /// <summary>
        /// 检查玩家是否在线
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckOnLine(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            //string s01 = QuestConditionInfo.sParam1;
            //if ((s01 != "") && (s01[0] == '<') && (s01[1] == '$'))
            //{
            //    s01 = GetLineVariableText(PlayObject, QuestConditionInfo.sParam1);
            //}
            //else
            //{
            //    GetValValue(PlayObject, QuestConditionInfo.sParam1, ref s01);
            //}
            //if ((s01 == "") || (M2Share.WorldEngine.GePlayObject(s01) == null))
            //{
            //    Success = false;
            //}
        }

        /// <summary>
        /// 检查人物是否重叠
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfIsDupMode(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            if (playObject.Envir != null)
            {
                if (playObject.Envir.GetXYObjCount(playObject.CurrX, playObject.CurrY) <= 1)
                {
                    success = false;
                }
            }
            else
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查指定地图玩家数量
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckHum(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            //int n14 = 0;
            //if (!GetValValue(PlayObject, QuestConditionInfo.sParam2, ref n14))
            //{
            //    n14 = QuestConditionInfo.nParam2;
            //}
            //if (M2Share.WorldEngine.GetMapHuman(QuestConditionInfo.sParam1) < n14)
            //{
            //    Success = false;
            //}
        }

        /// <summary>
        /// 检查玩家姓名是否存在指定文本中
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckNameList(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            if (!GotoLable_CheckStringList(playObject.ChrName, _mSPath + questConditionInfo.sParam1))
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查玩家帐号是否存在指定文本中
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckAccountList(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            if (!GotoLable_CheckStringList(playObject.UserAccount, _mSPath + questConditionInfo.sParam1))
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查玩家IP是否存在指定文本中
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckIpList(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            if (!GotoLable_CheckStringList(playObject.LoginIpAddr, _mSPath + questConditionInfo.sParam1))
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查指定变量是否相等
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfEqual(PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int n14 = 0;
            int n18 = 0;
            if (CheckVarNameNo(playObject, questConditionInfo, n14, n18) && n14 == n18)
            {
                success = true;
            }
        }

        private bool CheckVarNameNo(PlayObject playObject, QuestConditionInfo questConditionInfo, int n140, int n180)
        {
            bool success = false;
            n140 = -1;
            n180 = questConditionInfo.nParam2;
            var n100 = M2Share.GetValNameNo(questConditionInfo.sParam1);
            if (n100 <= 0) return success;
            if (HUtil32.RangeInDefined(n100, 0, 99))
            {
                n140 = M2Share.Config.GlobalVal[n100];// G
                success = true;
            }
            else if (HUtil32.RangeInDefined(n100, 1000, 1099))
            {
                n140 = M2Share.Config.GlobaDyMval[n100 - 1000];// I
                success = true;
            }
            else if (HUtil32.RangeInDefined(n100, 1100, 1109))
            {
                n140 = playObject.MNVal[n100 - 1100];// P
                success = true;
            }
            else if (HUtil32.RangeInDefined(n100, 1110, 1119))
            {
                n140 = playObject.MDyVal[n100 - 1110];// D
                success = true;
            }
            else if (HUtil32.RangeInDefined(n100, 1200, 1299))
            {
                n140 = playObject.MNMval[n100 - 1200];// M
                success = true;
            }
            else if (HUtil32.RangeInDefined(n100, 1300, 1399))
            {
                n140 = playObject.MNInteger[n100 - 1300];// N
                success = true;
            }
            else if (HUtil32.RangeInDefined(n100, 2000, 2499))
            {
                if (String.Compare(M2Share.Config.GlobalAVal[n100 - 2000], questConditionInfo.sParam2, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    n140 = 0;
                    n180 = 0;
                    success = true;
                }
            }
            else if (HUtil32.RangeInDefined(n100, 1400, 1499))
            {
                if (String.Compare(playObject.MSString[n100 - 1400], questConditionInfo.sParam2, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    n140 = 0;
                    n180 = 0;
                    success = true;
                }
            }
            return success;
        }
        
        private void ScriptConditionError(BaseObject baseObject, QuestConditionInfo questConditionInfo, string sCmd)
        {
            string sMsg = "Cmd:" + sCmd + " NPC名称:" + _chrName + " 地图:" + _mMapName + " 座标:" + _mNX + ':' + _mNY + " 参数1:"
                  + questConditionInfo.sParam1 + " 参数2:" + questConditionInfo.sParam2 + " 参数3:" + questConditionInfo.sParam3 + " 参数4:" + questConditionInfo.sParam4 + " 参数5:"
                  + questConditionInfo.sParam5;
            M2Share.Logger.Error("[脚本参数不正确] " + sMsg);
        }

        private void Dispose(object obj)
        {
            obj = null;
        }
    }
}