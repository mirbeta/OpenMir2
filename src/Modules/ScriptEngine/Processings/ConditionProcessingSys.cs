using OpenMir2;
using OpenMir2.Common;
using OpenMir2.Data;
using OpenMir2.Enums;
using OpenMir2.Packets.ClientPackets;
using ScriptSystem.Consts;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Castles;
using SystemModule.Const;
using SystemModule.Enums;
using SystemModule.Maps;

namespace ScriptSystem.Processings
{
    /// <summary>
    /// 脚本命令检查处理模块
    /// </summary>
    public class ConditionProcessingSys : ProcessingBase
    {
        private delegate void ScriptCondition(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success);
        /// <summary>
        /// 脚本条件检查列表
        /// </summary>
        private Dictionary<int, ScriptCondition> _conditionMap;

        public ConditionProcessingSys()
        {

        }

        public bool IsRegister(int cmdCode)
        {
            return _conditionMap.ContainsKey(cmdCode);
        }

        public void Execute(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            if (_conditionMap.ContainsKey(questConditionInfo.CmdCode))
            {
                _conditionMap[questConditionInfo.CmdCode](normNpc, playerActor, questConditionInfo, ref success);
            }
        }

        public void Initialize()
        {
            _conditionMap = new Dictionary<int, ScriptCondition>();
            _conditionMap[(int)ConditionCode.CHECK] = ConditionOfCheck;
            _conditionMap[(int)ConditionCode.RANDOM] = ConditionOfRandom;
            _conditionMap[(int)ConditionCode.GENDER] = ConditionOfGender;
            _conditionMap[(int)ConditionCode.DAYTIME] = ConditionOfDayTime;
            _conditionMap[(int)ConditionCode.CHECKOPEN] = ConditionOfCheckOpen;
            _conditionMap[(int)ConditionCode.CHECKUNIT] = ConditionCheckUnit;
            _conditionMap[(int)ConditionCode.CHECKLEVEL] = ConditionOfCheckLevel;
            _conditionMap[(int)ConditionCode.CHECKJOB] = ConditionOfCheckJob;
            _conditionMap[(int)ConditionCode.CHECKITEM] = ConditionOfCheckItem;
            _conditionMap[(int)ConditionCode.CHECKGOLD] = ConditionOfCheckGold;
            _conditionMap[(int)ConditionCode.CHECKDURA] = ConditionOfCheckDura;
            _conditionMap[(int)ConditionCode.DAYOFWEEK] = ConditionOfDayOfWeek;
            _conditionMap[(int)ConditionCode.HOUR] = ConditionOfHour;
            _conditionMap[(int)ConditionCode.MIN] = ConditionOfMin;
            _conditionMap[(int)ConditionCode.CHECKPKPOINT] = ConditionOfCheckPkPoint;
            _conditionMap[(int)ConditionCode.CHECKMONMAP] = ConditionOfCheckMonMapCount;
            _conditionMap[(int)ConditionCode.CHECKHUM] = ConditionOfCheckHum;
            _conditionMap[(int)ConditionCode.CHECKBAGGAGE] = ConditionOfCheckBagGage;
            _conditionMap[(int)ConditionCode.CHECKNAMELIST] = ConditionOfCheckNameList;
            _conditionMap[(int)ConditionCode.CHECKACCOUNTLIST] = ConditionOfCheckAccountList;
            _conditionMap[(int)ConditionCode.CHECKIPLIST] = ConditionOfCheckIpList;
            _conditionMap[(int)ConditionCode.EQUAL] = ConditionOfEqual;
            _conditionMap[(int)ConditionCode.LARGE] = ConditionOfLapge;
            _conditionMap[(int)ConditionCode.SMALL] = ConditionOfSmall;
            _conditionMap[(int)ConditionCode.ISSYSOP] = ConditionOfIssysop;
            _conditionMap[(int)ConditionCode.ISADMIN] = ConditionOfIsAdmin;
            _conditionMap[(int)ConditionCode.CHECKGROUPCOUNT] = ConditionOfCheckGroupCount;
            _conditionMap[(int)ConditionCode.CHECKPOS] = ConditionOfCheckPos;
            _conditionMap[(int)ConditionCode.CHECKPOSEDIR] = ConditionOfCheckPoseDir;
            _conditionMap[(int)ConditionCode.CHECKPOSELEVEL] = ConditionOfCheckPoseLevel;
            _conditionMap[(int)ConditionCode.CHECKPOSEGENDER] = ConditionOfCheckPoseGender;
            _conditionMap[(int)ConditionCode.CHECKLEVELEX] = ConditionOfCheckLevelEx;
            _conditionMap[(int)ConditionCode.CHECKBONUSPOINT] = ConditionOfCheckBonusPoint;
            _conditionMap[(int)ConditionCode.CHECKMARRY] = ConditionOfCheckMarry;
            _conditionMap[(int)ConditionCode.CHECKPOSEMARRY] = ConditionOfCheckPoseMarry;
            _conditionMap[(int)ConditionCode.CHECKMARRYCOUNT] = ConditionOfCheckMarryCount;
            _conditionMap[(int)ConditionCode.CHECKMASTER] = ConditionOfCheckMaster;
            _conditionMap[(int)ConditionCode.HAVEMASTER] = ConditionOfHaveMaster;
            _conditionMap[(int)ConditionCode.CHECKPOSEMASTER] = ConditionOfCheckPoseMaster;
            _conditionMap[(int)ConditionCode.POSEHAVEMASTER] = ConditionOfPoseHaveMaster;
            _conditionMap[(int)ConditionCode.CHECKISMASTER] = ConditionOfCheckIsMaster;
            _conditionMap[(int)ConditionCode.HASGUILD] = ConditionOfCheckHaveGuild;
            _conditionMap[(int)ConditionCode.ISGUILDMASTER] = ConditionOfCheckIsGuildMaster;
            _conditionMap[(int)ConditionCode.CHECKCASTLEMASTER] = ConditionOfCheckIsCastleMaster;
            _conditionMap[(int)ConditionCode.ISCASTLEGUILD] = ConditionOfCheckIsCastleaGuild;
            _conditionMap[(int)ConditionCode.ISATTACKGUILD] = ConditionOfCheckIsAttackGuild;
            _conditionMap[(int)ConditionCode.ISDEFENSEGUILD] = ConditionOfCheckIsDefenseGuild;
            _conditionMap[(int)ConditionCode.CHECKCASTLEDOOR] = ConditionOfCheckCastleDoorStatus;
            _conditionMap[(int)ConditionCode.ISDEFENSEALLYGUILD] = ConditionOfCheckIsDefenseAllyGuild;
            _conditionMap[(int)ConditionCode.CHECKPOSEISMASTER] = ConditionOfCheckPoseIsMaster;
            _conditionMap[(int)ConditionCode.CHECKNAMEIPLIST] = ConditionOfCheckNameIpList;
            _conditionMap[(int)ConditionCode.CHECKACCOUNTIPLIST] = ConditionOfCheckAccountIpList;
            _conditionMap[(int)ConditionCode.CHECKSLAVECOUNT] = ConditionOfCheckSlaveCount;
            _conditionMap[(int)ConditionCode.ISNEWHUMAN] = ConditionOfIsNewHuman;
            _conditionMap[(int)ConditionCode.CHECKMEMBERTYPE] = ConditionOfCheckMemberType;
            _conditionMap[(int)ConditionCode.CHECKMEMBERLEVEL] = ConditionOfCheckMemBerLevel;
            _conditionMap[(int)ConditionCode.CHECKGAMEPOINT] = ConditionOfCheckGamePoint;
            _conditionMap[(int)ConditionCode.CHECKNAMELISTPOSITION] = ConditionOfCheckNameListPostion;
            _conditionMap[(int)ConditionCode.CHECKGUILDLIST] = ConditionOfCheckGuildList;
            _conditionMap[(int)ConditionCode.CHECKRENEWLEVEL] = ConditionOfCheckReNewLevel;
            _conditionMap[(int)ConditionCode.CHECKSLAVELEVEL] = ConditionOfCheckSlaveLevel;
            _conditionMap[(int)ConditionCode.CHECKSLAVENAME] = ConditionOfCheckSlaveName;
            _conditionMap[(int)ConditionCode.CHECKCREDITPOINT] = ConditionOfCheckCreditPoint;
            _conditionMap[(int)ConditionCode.CHECKOFGUILD] = ConditionOfCheckOfGuild;
            _conditionMap[(int)ConditionCode.CHECKUSEITEM] = ConditionOfCheckUseItem;
            _conditionMap[(int)ConditionCode.CHECKBAGSIZE] = ConditionOfCheckBagSize;
            _conditionMap[(int)ConditionCode.CHECKDC] = ConditionOfCheckDc;
            _conditionMap[(int)ConditionCode.CHECKMC] = ConditionOfCheckMC;
            _conditionMap[(int)ConditionCode.CHECKSC] = ConditionOfCheckSc;
            _conditionMap[(int)ConditionCode.CHECKHP] = ConditionOfCheckHp;
            _conditionMap[(int)ConditionCode.CHECKMP] = ConditionOfCheckMp;
            _conditionMap[(int)ConditionCode.CHECKITEMTYPE] = ConditionOfCheckItemType;
            _conditionMap[(int)ConditionCode.CHECKEXP] = ConditionOfCheckExp;
            _conditionMap[(int)ConditionCode.CHECKCASTLEGOLD] = ConditionOfCheckCastleGold;
            _conditionMap[(int)ConditionCode.CHECKBUILDPOINT] = ConditionOfCheckGuildBuildPoint;
            _conditionMap[(int)ConditionCode.CHECKAURAEPOINT] = ConditionOfCheckGuildAuraePoint;
            _conditionMap[(int)ConditionCode.CHECKSTABILITYPOINT] = ConditionOfCheckStabilityPoint;
            _conditionMap[(int)ConditionCode.CHECKFLOURISHPOINT] = ConditionOfCheckFlourishPoint;
            _conditionMap[(int)ConditionCode.CHECKCONTRIBUTION] = ConditionOfCheckContribution;
            _conditionMap[(int)ConditionCode.CHECKRANGEMONCOUNT] = ConditionOfCheckRangeMonCount;
            _conditionMap[(int)ConditionCode.CHECKINMAPRANGE] = ConditionOfCheckInMapRange;
            _conditionMap[(int)ConditionCode.CASTLECHANGEDAY] = ConditionOfCheckCastleChageDay;
            _conditionMap[(int)ConditionCode.CASTLEWARDAY] = ConditionOfCheckCastleWarDay;
            _conditionMap[(int)ConditionCode.ONLINELONGMIN] = ConditionOfCheckOnlineLongMin;
            _conditionMap[(int)ConditionCode.CHECKGUILDCHIEFITEMCOUNT] = ConditionOfCheckChiefItemCount;
            _conditionMap[(int)ConditionCode.CHECKNAMEDATELIST] = ConditionOfCheckNameDateList;
            _conditionMap[(int)ConditionCode.CHECKUSERDATE] = ConditionOfCheckNameDateList;
            _conditionMap[(int)ConditionCode.CHECKMAPHUMANCOUNT] = ConditionOfCheckMapHumanCount;
            _conditionMap[(int)ConditionCode.CHECKMAPMONCOUNT] = ConditionOfCheckMapMonCount;
            _conditionMap[(int)ConditionCode.CHECKVAR] = ConditionOfCheckVar;
            _conditionMap[(int)ConditionCode.CHECKSERVERNAME] = ConditionOfCheckServerName;
            _conditionMap[(int)ConditionCode.CHECKMAPNAME] = ConditionOfCheckMapName;
            _conditionMap[(int)ConditionCode.INSAFEZONE] = ConditionOfCheckSafeZone;
            _conditionMap[(int)ConditionCode.CHECKCONTAINSTEXT] = ConditionOfAnsiContainsText;
            _conditionMap[(int)ConditionCode.COMPARETEXT] = ConditionOfCompareText;
            _conditionMap[(int)ConditionCode.CHECKTEXTLIST] = ConditionOfCheckTextList;
            _conditionMap[(int)ConditionCode.ISGROUPMASTER] = ConditionOfIsGroupMaster;
            _conditionMap[(int)ConditionCode.CHECKCONTAINSTEXTLIST] = ConditionOfCheCkContAinsTextList;
            _conditionMap[(int)ConditionCode.CHECKONLINE] = ConditionOfCheckOnLine;
            _conditionMap[(int)ConditionCode.ISDUPMODE] = ConditionOfIsDupMode;
            _conditionMap[(int)ConditionCode.ISONMAP] = ConditionOfIosnMap;
            _conditionMap[(int)ConditionCode.CHECKGAMEGOLD] = ConditionOfCheckGameGold;
            _conditionMap[(int)ConditionCode.CHECKISONMAP] = ConditionOfCheckIsOnMap;
            _conditionMap[(int)ConditionCode.CHECKITEMADDVALUE] = ConditionOfCheckItemAddValue;
            _conditionMap[(int)ConditionCode.REVIVESLAVE] = ConditionOfReviveSlave;
            _conditionMap[(int)ConditionCode.CHECKMAGICLVL] = ConditionOfCheckMagicLvl;
            _conditionMap[(int)ConditionCode.CHECKGROUPCLASS] = ConditionOfCheckGroupClass;
            _conditionMap[(int)ConditionCode.ISHIGH] = ConditionOfIsHigh;
            _conditionMap[(int)ConditionCode.CHECKBBCOUNT] = ConditionOfCheckSlaveListCount;
            _conditionMap[(int)ConditionCode.CHECKLUCKYPOINT] = ConditionOfCheckLuckyPoint;
            _conditionMap[(int)ConditionCode.CHECKSKILL] = ConditionOfCheckSkill;
        }

        private void ConditionOfCheckLuckyPoint(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = playerActor.BodyLuckLevel < questConditionInfo.nParam1;
        }

        private void ConditionOfDayTime(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (string.Compare(questConditionInfo.sParam1, ScriptFlagConst.sSUNRAISE, StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (SystemShare.GameTime != 0)
                {
                    success = false;
                }
            }
            if (string.Compare(questConditionInfo.sParam1, ScriptFlagConst.sDAY, StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (SystemShare.GameTime != 1)
                {
                    success = false;
                }
            }
            if (string.Compare(questConditionInfo.sParam1, ScriptFlagConst.sSUNSET, StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (SystemShare.GameTime != 2)
                {
                    success = false;
                }
            }
            if (string.Compare(questConditionInfo.sParam1, ScriptFlagConst.sNIGHT, StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (SystemShare.GameTime != 3)
                {
                    success = false;
                }
            }
        }

        private void ConditionOfCheckOpen(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int n14 = HUtil32.StrToInt(questConditionInfo.sParam1, 0);
            int n18 = HUtil32.StrToInt(questConditionInfo.sParam2, 0);
            int n10 = playerActor.GetQuestUnitOpenStatus(n14);
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

        private void ConditionOfCheckSlaveListCount(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (playerActor.SlaveList.Count < questConditionInfo.nParam1)
            {
                success = false;
            }
        }

        private void ConditionOfIsHigh(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (string.IsNullOrEmpty(questConditionInfo.sParam1))
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.ISHIGH);
                return;
            }
            char cMode = questConditionInfo.sParam1[0];
            switch (cMode)
            {
                case 'L':
                    success = SystemShare.HighLevelHuman == playerActor.ActorId;
                    break;
                case 'P':
                    success = SystemShare.HighPKPointHuman == playerActor.ActorId;
                    break;
                case 'D':
                    success = SystemShare.HighDCHuman == playerActor.ActorId;
                    break;
                case 'M':
                    success = SystemShare.HighMCHuman == playerActor.ActorId;
                    break;
                case 'S':
                    success = SystemShare.HighSCHuman == playerActor.ActorId;
                    break;
                default:
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.ISHIGH);
                    break;
            }
        }

        private void ConditionOfCheckGroupClass(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nCount = 0;
            PlayerJob nJob = PlayerJob.None;
            IPlayerActor IPlayerActorEx;
            if (HUtil32.CompareLStr(questConditionInfo.sParam1, ScriptFlagConst.sWarrior))
            {
                nJob = PlayerJob.Warrior;
            }
            if (HUtil32.CompareLStr(questConditionInfo.sParam1, ScriptFlagConst.sWizard))
            {
                nJob = PlayerJob.Wizard;
            }
            if (HUtil32.CompareLStr(questConditionInfo.sParam1, ScriptFlagConst.sTaos))
            {
                nJob = PlayerJob.Taoist;
            }
            if (nJob < 0)
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ExecutionCode.ChangeJob.ToString());
                return;
            }
            if (playerActor.GroupOwner != 0)
            {
                for (int i = 0; i < playerActor.GroupMembers.Count; i++)
                {
                    IPlayerActorEx = playerActor.GroupMembers[i];
                    if (IPlayerActorEx.Job == nJob)
                    {
                        nCount++;
                    }
                }
            }
            char cMethod = questConditionInfo.sParam2[0];
            switch (cMethod)
            {
                case '=':
                    if (nCount == questConditionInfo.nParam3)
                    {
                        success = true;
                    }
                    break;
                case '>':
                    if (nCount > questConditionInfo.nParam3)
                    {
                        success = true;
                    }
                    break;
                case '<':
                    if (nCount < questConditionInfo.nParam3)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (nCount >= questConditionInfo.nParam3)
                    {
                        success = true;
                    }
                    break;
            }
        }


        private void ConditionOfCheckMagicLvl(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            UserMagic UserMagic;
            for (int i = 0; i < playerActor.MagicList.Count; i++)
            {
                UserMagic = playerActor.MagicList[i];
                if (string.Compare(UserMagic.Magic.MagicName, questConditionInfo.sParam1, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (UserMagic.Level == questConditionInfo.nParam2)
                    {
                        success = true;
                    }
                    break;
                }
            }
        }

        private void ConditionOfReviveSlave(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            string s18;
            FileInfo myFile;
            StringList LoadList;
            string Petname = string.Empty;
            string lvl = string.Empty;
            string lvlexp = string.Empty;
            string sFileName = SystemShare.GetEnvirFilePath("PetData", playerActor.ChrName + ".txt");
            if (File.Exists(sFileName))
            {
                LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (int i = 0; i < LoadList.Count; i++)
                {
                    s18 = LoadList[i].Trim();
                    if ((!string.IsNullOrEmpty(s18)) && (s18[1] != ';'))
                    {
                        s18 = HUtil32.GetValidStr3(s18, ref Petname, HUtil32.Backslash);
                        s18 = HUtil32.GetValidStr3(s18, ref lvl, HUtil32.Backslash);
                        s18 = HUtil32.GetValidStr3(s18, ref lvlexp, HUtil32.Backslash);
                        // playerActor.ReviveSlave(PetName,StrToInt(lvl,0),StrToInt(lvlexp,0),nslavecount,10 * 24 * 60 * 60);
                    }
                }
                if (LoadList.Count > 0)
                {
                    success = true;
                    myFile = new FileInfo(sFileName);
                    StreamWriter _W_0 = myFile.CreateText();
                    _W_0.Close();
                }
            }
        }

        private void ConditionOfCheckPos(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nX = questConditionInfo.nParam2;
            int nY = questConditionInfo.nParam3;
            if ((questConditionInfo.sParam1 == playerActor.MapName) && (nX == playerActor.CurrX) && (nY == playerActor.CurrY))
            {
                success = true;
            }
            else
            {
                success = false;
            }
        }

        private void ConditionOfCheckItemAddValue(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nWhere = HUtil32.StrToInt(questConditionInfo.sParam1, -1);
            char cMethod = questConditionInfo.sParam2[0];
            int nAddValue = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
            if (!(nWhere >= 0 && nWhere <= playerActor.UseItems.Length) || (nAddValue < 0))
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKITEMADDVALUE);
                return;
            }
            UserItem UserItem = playerActor.UseItems[nWhere];
            if (UserItem.Index == 0)
            {
                return;
            }
            int nAddAllValue = 0;
            for (int i = 0; i < UserItem.Desc.Length; i++)
            {
                nAddAllValue += UserItem.Desc[i];
            }
            cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nAddAllValue == nAddValue)
                    {
                        success = true;
                    }
                    break;
                case '>':
                    if (nAddAllValue > nAddValue)
                    {
                        success = true;
                    }
                    break;
                case '<':
                    if (nAddAllValue < nAddValue)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (nAddAllValue >= nAddValue)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckIsOnMap(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            if (playerActor.MapFileName == questConditionInfo.sParam1 || playerActor.MapName == questConditionInfo.sParam1)
            {
                success = true;
            }
            success = false;
        }

        private void ConditionOfCheckBagGage(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            if (playerActor.IsEnoughBag())
            {
                if ((!string.IsNullOrEmpty(questConditionInfo.sParam1)))
                {
                    success = false;
                    //StdItem stdItem = SystemShare.ItemSystem.GetStdItem(questConditionInfo.sParam1);
                    //if (stdItem != null)
                    //{
                    //    if (playerActor.IsAddWeightAvailable(stdItem.Weight))
                    //    {
                    //        success = true;
                    //    }
                    //}
                }
            }
            else
            {
                success = false;
            }
        }


        private void ConditionOfCheckOfGuild(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            string sGuildName;
            success = false;
            if (string.IsNullOrEmpty(questConditionInfo.sParam1))
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKOFGUILD);
                return;
            }
            if (playerActor.MyGuild != null)
            {
                sGuildName = questConditionInfo.sParam1;
                GetVarValue(playerActor, questConditionInfo.sParam1, ref sGuildName);
                if (string.Compare(playerActor.MyGuild.GuildName, sGuildName, StringComparison.Ordinal) == 0)
                {
                    success = true;
                }
            }
        }

        private void ConditionOfCheckOnlineLongMin(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            int nOnlineMin = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nOnlineMin < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam2, ref nOnlineMin);
                if (nOnlineMin < 0)
                {
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.ONLINELONGMIN);
                    return;
                }
            }
            long nOnlineTime = (HUtil32.GetTickCount() - playerActor.LogonTick) / 60000;
            char cMethod = questConditionInfo.sParam1[0];
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

        private void ConditionOfCheckPasswordErrorCount(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            int nErrorCount = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nErrorCount < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam2, ref nErrorCount);
                if (nErrorCount < 0)
                {
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.PASSWORDERRORCOUNT);
                    return;
                }
            }
            char cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (playerActor.PwdFailCount == nErrorCount)
                    {
                        success = true;
                    }
                    break;
                case '>':
                    if (playerActor.PwdFailCount > nErrorCount)
                    {
                        success = true;
                    }
                    break;
                case '<':
                    if (playerActor.PwdFailCount < nErrorCount)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (playerActor.PwdFailCount >= nErrorCount)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfIsLockPassword(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = playerActor.IsPasswordLocked;
        }

        private void ConditionOfIsLockStorage(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = !playerActor.IsCanGetBackItem;
        }

        private void ConditionOfCheckPayMent(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            int payMent = HUtil32.StrToInt(questConditionInfo.sParam1, -1);
            if (payMent < 1)
            {
                GetVarValue(playerActor, questConditionInfo.sParam1, ref payMent);
                if (payMent < 1)
                {
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKPAYMENT);
                    return;
                }
            }
            if (playerActor.PayMent == payMent)
            {
                success = true;
            }
        }

        private void ConditionOfCheckNameDateList(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo,
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
            int nDayCount = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
            bool boDeleteExprie = string.Compare(questConditionInfo.sParam6, "清理", StringComparison.Ordinal) == 0;
            bool boNoCompareHumanName = string.Compare(questConditionInfo.sParam6, "1", StringComparison.Ordinal) == 0;
            char cMethod = questConditionInfo.sParam2[0];
            if (nDayCount < 0)
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKNAMEDATELIST);
                return;
            }
            string sListFileName = SystemShare.GetEnvirFilePath(normNpc.Path, questConditionInfo.sParam1);
            if (File.Exists(sListFileName))
            {
                loadList = new StringList();
                try
                {
                    loadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    LogService.Error("loading fail.... => " + sListFileName);
                }
                for (int i = 0; i < loadList.Count; i++)
                {
                    sLineText = loadList[i].Trim();
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sHumName, new string[] { " ", "\09" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sDate, new string[] { " ", "\09" });
                    if (string.Compare(sHumName, playerActor.ChrName, StringComparison.Ordinal) == 0 ||
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
                        varInfo = GetVarValue(playerActor, questConditionInfo.sParam4, ref sVar, ref sValue, ref nValue);
                        switch (varInfo.VarAttr)
                        {
                            case VarAttr.aNone:
                            case VarAttr.aConst:
                                ScriptConditionError(normNpc, playerActor, questConditionInfo,
                                    ConditionCode.CHECKNAMEDATELIST);
                                break;
                            case VarAttr.aFixStr:
                                SetValNameValue(playerActor, sVar, sValue, nDay);
                                break;
                            case VarAttr.aDynamic:
                                SetDynamicValue(playerActor, sVar, sValue, nDay);
                                break;
                        }
                        varInfo = GetVarValue(playerActor, questConditionInfo.sParam5, ref sVar, ref sValue, ref nValue);
                        switch (varInfo.VarAttr)
                        {
                            case VarAttr.aNone:
                            case VarAttr.aConst:
                                ScriptConditionError(normNpc, playerActor, questConditionInfo,
                                    ConditionCode.CHECKNAMEDATELIST);
                                break;
                            case VarAttr.aFixStr:
                                SetValNameValue(playerActor, sVar, sValue, nDayCount - nDay);
                                break;
                            case VarAttr.aDynamic:
                                SetDynamicValue(playerActor, sVar, sValue, nDayCount - nDay);
                                break;
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
                                    LogService.Error("Save fail.... => " + sListFileName);
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
                LogService.Error("file not found => " + sListFileName);
            }
        }

        private void ConditionOfCheckGuildNameDateList(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo,
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
            if (playerActor.MyGuild != null)
            {
                nDayCount = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
                boDeleteExprie = string.Compare(questConditionInfo.sParam6, "清理", StringComparison.Ordinal) == 0;
                boNoCompareHumanName = string.Compare(questConditionInfo.sParam6, "1", StringComparison.Ordinal) == 0;
                cMethod = questConditionInfo.sParam2[1];
                if (nDayCount < 0)
                {
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKGUILDNAMEDATELIST);
                    return;
                }
                sListFileName = SystemShare.GetEnvirFilePath(normNpc.Path, questConditionInfo.sParam1);
                if (File.Exists(sListFileName))
                {
                    loadList = new StringList();
                    try
                    {
                        loadList.LoadFromFile(sListFileName);
                    }
                    catch
                    {
                        LogService.Error("loading fail.... => " + sListFileName);
                    }
                    for (int i = 0; i < loadList.Count; i++)
                    {
                        sLineText = loadList[i].Trim();
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sHumName, new string[] { " ", "\09" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sDate, new string[] { " ", "\09" });
                        if (string.Compare(sHumName, playerActor.MyGuild.GuildName, StringComparison.Ordinal) == 0 ||
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
                                varInfo = GetVarValue(playerActor, questConditionInfo.sParam4, ref sVar, ref sValue, ref nValue);
                                switch (varInfo.VarAttr)
                                {
                                    case VarAttr.aNone:
                                    case VarAttr.aConst:
                                        ScriptConditionError(normNpc, playerActor, questConditionInfo,
                                            ConditionCode.CHECKGUILDNAMEDATELIST);
                                        break;

                                    case VarAttr.aFixStr:
                                        SetValNameValue(playerActor, sVar, sValue, nDay);
                                        break;

                                    case VarAttr.aDynamic:
                                        SetDynamicValue(playerActor, sVar, sValue, nDay);
                                        break;
                                }
                            }
                            if (questConditionInfo.sParam5 != "")
                            {
                                varInfo = GetVarValue(playerActor, questConditionInfo.sParam5, ref sVar, ref sValue, ref nValue);
                                switch (varInfo.VarAttr)
                                {
                                    case VarAttr.aNone:
                                    case VarAttr.aConst:
                                        ScriptConditionError(normNpc, playerActor, questConditionInfo,
                                            ConditionCode.CHECKGUILDNAMEDATELIST);
                                        break;

                                    case VarAttr.aFixStr:
                                        SetValNameValue(playerActor, sVar, sValue, nDayCount - nDay);
                                        break;

                                    case VarAttr.aDynamic:
                                        SetDynamicValue(playerActor, sVar, sValue, nDayCount - nDay);
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
                                        LogService.Error("Save fail.... => " + sListFileName);
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
                    LogService.Error("file not found => " + sListFileName);
                }
            }
        }

        // CHECKMAPHUMANCOUNT MAP = COUNT
        private void ConditionOfCheckMapHumanCount(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            int nCount = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
            if (nCount < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue(baseObject, questConditionInfo.sParam3, ref nCount);
                    if (nCount < 0)
                    {
                        ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKMAPHUMANCOUNT);
                        return;
                    }
                }
                else
                {
                    ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKMAPHUMANCOUNT);
                    return;
                }
            }
            string sMapName = questConditionInfo.sParam1;
            IEnvirnoment envir = SystemShare.MapMgr.FindMap(sMapName);
            if (envir == null)
            {
                GetVarValue(baseObject, questConditionInfo.sParam1, ref sMapName);
                envir = SystemShare.MapMgr.FindMap(sMapName);
            }
            if (envir == null)
            {
                ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKMAPHUMANCOUNT);
                return;
            }
            int nHumanCount = SystemShare.WorldEngine.GetMapHuman(envir.MapName);
            char cMethod = questConditionInfo.sParam2[1];
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

        private void ConditionOfCheckMapMonCount(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nCount = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
            string sMapName = questConditionInfo.sParam1;
            IEnvirnoment envir = SystemShare.MapMgr.FindMap(sMapName);
            if (baseObject.Race == ActorRace.Play)
            {
                if (nCount < 0)
                {
                    GetVarValue(baseObject, questConditionInfo.sParam3, ref nCount);
                }
                if (envir == null)
                {
                    GetVarValue(baseObject, questConditionInfo.sParam1, ref sMapName);
                    envir = SystemShare.MapMgr.FindMap(sMapName);
                }
            }
            if (nCount < 0 || envir == null)
            {
                ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKMAPMONCOUNT);
                return;
            }
            int nMonCount = SystemShare.WorldEngine.GetMapMonster(envir, null);
            char cMethod = questConditionInfo.sParam2[1];
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
        /// <returns></returns>
        private void ConditionOfIosnMap(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            string sMapName = string.Empty;
            if (questConditionInfo.sParam6 == "88")
            {
                //playerActor = SystemShare.WorldEngine.GetPlayObject(GetLineVariableText(playerActor, questConditionInfo.sParam7));
                //if (playerActor == null)
                //{

                //}
            }
            GetVarValue(playerActor, questConditionInfo.sParam1, ref sMapName);
            sMapName = GetLineVariableText(playerActor, questConditionInfo.sParam1); // 地图支持变量
            IEnvirnoment envir = SystemShare.MapMgr.FindMap(sMapName);
            if (envir == null)
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.ISONMAP);
            }
            if (playerActor.Envir == envir)
            {
                success = true;
            }
        }

        private void ConditionOfCheckMonMapCount(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            IActor aObject;
            string sMapName = questConditionInfo.sParam1;
            int nCount = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam2, ref nCount);
            }
            IEnvirnoment envir = SystemShare.MapMgr.FindMap(sMapName);
            if (envir == null)
            {
                GetVarValue(baseObject, questConditionInfo.sParam1, ref sMapName);
                envir = SystemShare.MapMgr.FindMap(sMapName);
            }
            if (envir == null || nCount < 0)
            {
                ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKMONMAP);
                return;
            }
            List<IActor> monList = new List<IActor>();
            int nMapRangeCount = SystemShare.WorldEngine.GetMapMonster(envir, monList);
            for (int i = monList.Count - 1; i >= 0; i--)
            {
                if (monList.Count <= 0)
                {
                    break;
                }
                aObject = monList[i];
                if (aObject.Race < ActorRace.Animal || aObject.Race == ActorRace.ArcherGuard ||
                    aObject.Master != null || aObject.Race == ActorRace.NPC ||
                    aObject.Race == ActorRace.PeaceNpc)
                {
                    monList.RemoveAt(i);
                }
            }
            nMapRangeCount = monList.Count;
            if (nMapRangeCount >= nCount)
            {
                success = true;
            }
            Dispose(monList);
        }

        private void ConditionOfCheckRangeMonCount(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            IActor aObject;
            success = false;
            string sMapName = questConditionInfo.sParam1;
            int nX = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            int nY = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
            int nRange = HUtil32.StrToInt(questConditionInfo.sParam4, -1);
            char cMethod = questConditionInfo.sParam5[1];
            int nCount = HUtil32.StrToInt(questConditionInfo.sParam6, -1);
            if (nX < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam2, ref nX);
            }
            if (nY < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam3, ref nY);
            }
            if (nRange < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam4, ref nRange);
            }
            if (nCount < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam6, ref nCount);
            }
            IEnvirnoment envir = SystemShare.MapMgr.FindMap(sMapName);
            if (envir == null || nX < 0 || nY < 0 || nRange < 0 || nCount < 0)
            {
                ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKRANGEMONCOUNT);
                return;
            }
            List<IActor> monList = new List<IActor>();
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

        private void ConditionOfCheckRangeGroupCount(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            int nRange;
            int nCount;
            char cMethod = ' ';
            int nMapRangeCount;
            IPlayerActor groupOwnerPlay = (IPlayerActor)SystemShare.ActorMgr.Get(playerActor.GroupOwner);
            if (playerActor.GroupOwner != 0 && groupOwnerPlay != null)
            {
                nRange = HUtil32.StrToInt(questConditionInfo.sParam1, -1);
                if (questConditionInfo.sParam2 != "")
                {
                    cMethod = questConditionInfo.sParam2[1];
                }
                nCount = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
                if (nRange < 0)
                {
                    GetVarValue(playerActor, questConditionInfo.sParam1, ref nRange);
                }
                if (nCount < 0)
                {
                    GetVarValue(playerActor, questConditionInfo.sParam3, ref nCount);
                }
                if (nRange < 0 || nCount < 0 || cMethod == ' ')
                {
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKRANGEROUPCOUNT);
                    return;
                }
                nMapRangeCount = 0;
                for (int i = 0; i < groupOwnerPlay.GroupMembers.Count; i++)
                {
                    IPlayerActor groupHuman = groupOwnerPlay.GroupMembers[i];
                    if (groupHuman.Envir == playerActor.Envir)
                    {
                        if (Math.Abs(playerActor.CurrX - groupHuman.CurrX) <= nRange &&
                            Math.Abs(playerActor.CurrY - groupHuman.CurrY) <= nRange)
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

        private void ConditionOfCheckOnLinePlayCount(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            if (questConditionInfo.sParam1 == "" || questConditionInfo.sParam2 == "")
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKONLINEPLAYCOUNT);
                return;
            }
            char cMethod = questConditionInfo.sParam1[0];
            int nCount = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam2, ref nCount);
            }
            if (nCount < 0)
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKONLINEPLAYCOUNT);
                return;
            }
            switch (cMethod)
            {
                case '=':
                    if (SystemShare.WorldEngine.PlayObjectCount == nCount)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (SystemShare.WorldEngine.PlayObjectCount > nCount)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (SystemShare.WorldEngine.PlayObjectCount < nCount)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (SystemShare.WorldEngine.PlayObjectCount >= nCount)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private bool ConditionOfCheckItemBindUse(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            UserItem userItem = null;
            int nWhere = HUtil32.StrToInt(questConditionInfo.sParam1, -1);
            if (nWhere < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam1, ref nWhere); // 增加变量支持
            }
            if (nWhere < 0)
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKUSEITEMBIND);
                return false;
            }
            userItem = playerActor.UseItems[nWhere];
            StdItem stdItem = SystemShare.EquipmentSystem.GetStdItem(userItem.Index);
            if (userItem.Index <= 0 || stdItem == null)
            {
                playerActor.SysMsg("你身上没有戴指定物品！！！", MsgColor.Red, MsgType.Hint);
                return false;
            }
            return playerActor.CheckItemBindUse(userItem);
        }

        public bool ConditionOfCheckMemoryItemIsRememberItem(UserItem userItem)
        {
            StdItem stdItem = SystemShare.EquipmentSystem.GetStdItem(userItem.Index);
            if (stdItem == null)
            {
                return false;
            }
            return stdItem.StdMode == 31 && stdItem.AniCount != 0 && stdItem.Shape == 1;
        }

        private void ConditionOfCheckReNewLevel(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            int nLevel = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam2, ref nLevel);
                if (nLevel < 0)
                {
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKLEVELEX);
                    return;
                }
            }
            char cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (playerActor.ReLevel == nLevel)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (playerActor.ReLevel > nLevel)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (playerActor.ReLevel < nLevel)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (playerActor.ReLevel >= nLevel)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckSlaveLevel(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            int nLevel = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            IActor aObject;
            if (!(nLevel >= 0 && nLevel <= 7))
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue(baseObject, questConditionInfo.sParam2, ref nLevel);
                    if (!(nLevel >= 0 && nLevel <= 7))
                    {
                        ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKSLAVELEVEL);
                        return;
                    }
                }
            }
            char cMethod = questConditionInfo.sParam1[0];
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

        private void ConditionOfCheckSlaveRange(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nSlavenRange;
            IActor aObject;
            string sChrName = questConditionInfo.sParam1;
            int nRange = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
            if (nRange < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam3, ref nRange);
                if (nRange < 0)
                {
                    ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKSLAVERANGE);
                    return;
                }
            }
            bool boFind = false;
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
                GetVarValue(baseObject, questConditionInfo.sParam1, ref sChrName);
            }
            char cMethod = questConditionInfo.sParam2[1];
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

        private void ConditionOfCheckVar(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            string sName = string.Empty;
            bool boFoundVar = false;
            string sType = questConditionInfo.sParam1;
            string sVarName = questConditionInfo.sParam2;
            string sMethod = questConditionInfo.sParam3;
            long nVarValue = HUtil32.StrToInt(questConditionInfo.sParam4, 0);
            string sVarValue = questConditionInfo.sParam4;
            if (sType == "" || sVarName == "" || sMethod == "")
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKVAR);
                return;
            }
            char cMethod = sMethod[1];
            Dictionary<string, DynamicVar> dynamicVarList = GeDynamicVarList(playerActor, sType, ref sName);
            if (dynamicVarList == null)
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKVAR);
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
                                    if (dynamic.Internet == nVarValue)
                                    {
                                        success = true;
                                    }
                                    break;
                                case '>':
                                    if (dynamic.Internet > nVarValue)
                                    {
                                        success = true;
                                    }
                                    break;
                                case '<':
                                    if (dynamic.Internet < nVarValue)
                                    {
                                        success = true;
                                    }
                                    break;
                                default:
                                    if (dynamic.Internet >= nVarValue)
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
                                    if (dynamic.String == sVarValue)
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
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKVAR);
                }
            }
        }

        private void ConditionOfHaveMaster(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            if (!string.IsNullOrEmpty(playerActor.MasterName))
            {
                success = true;
            }
        }

        private void ConditionOfPoseHaveMaster(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            IPlayerActor poseHuman = (IPlayerActor)playerActor.GetPoseCreate();
            if (poseHuman != null && poseHuman.Race == ActorRace.Play)
            {
                if ((poseHuman).MasterName != "")
                {
                    success = true;
                }
            }
        }

        private void ConditionOfCheckCastleGold(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            int nGold = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nGold < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam2, ref nGold);
            }
            if (nGold < 0 || playerActor.Castle == null)
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKCASTLEGOLD);
                return;
            }
            char cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (playerActor.Castle.TotalGold == nGold)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (playerActor.Castle.TotalGold > nGold)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (playerActor.Castle.TotalGold < nGold)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (playerActor.Castle.TotalGold >= nGold)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckContribution(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            int nContribution = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nContribution < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam2, ref nContribution);
                if (nContribution < 0)
                {
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKCONTRIBUTION);
                    return;
                }
            }
            char cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (playerActor.Contribution == nContribution)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (playerActor.Contribution > nContribution)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (playerActor.Contribution < nContribution)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (playerActor.Contribution >= nContribution)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckCreditPoint(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo,
            ref bool success)
        {
            success = false;
            int nCreditPoint = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nCreditPoint < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam2, ref nCreditPoint);
                if (nCreditPoint < 0)
                {
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKCREDITPOINT);
                    return;
                }
            }
            char cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (playerActor.CreditPoint == nCreditPoint)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (playerActor.CreditPoint > nCreditPoint)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (playerActor.CreditPoint < nCreditPoint)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (playerActor.CreditPoint >= nCreditPoint)
                    {
                        success = true;
                    }
                    break;
            }
        }


        private void ConditionOfCheckAccountIpList(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
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
                sChrName = playerActor.ChrName;
                sCharAccount = playerActor.UserAccount;
                sCharIPaddr = playerActor.LoginIpAddr;
                if (File.Exists(SystemShare.GetEnvirFilePath(normNpc.Path, questConditionInfo.sParam1)))
                {
                    loadList = new StringList();
                    loadList.LoadFromFile(SystemShare.GetEnvirFilePath(normNpc.Path, questConditionInfo.sParam1));
                    for (int i = 0; i < loadList.Count; i++)
                    {
                        sLine = loadList[i];
                        if (sLine[0] == ';')
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
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKACCOUNTIPLIST);
                }
            }
            finally
            {
                Dispose(loadList);
            }
        }

        private void ConditionOfCheckBagSize(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nSize = HUtil32.StrToInt(questConditionInfo.sParam1, -1);
            if (nSize < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam1, ref nSize);
            }
            if (nSize <= 0 || nSize > Grobal2.MaxBagItem)
            {
                ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKBAGSIZE);
                return;
            }
            if (baseObject.ItemList.Count + nSize <= Grobal2.MaxBagItem)
            {
                success = true;
            }
        }

        private void ConditionOfCheckBonusPoint(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nTotlePoint = playerActor.BonusAbil.DC + playerActor.BonusAbil.MC + playerActor.BonusAbil.SC + playerActor.BonusAbil.AC + playerActor.BonusAbil.MAC + playerActor.BonusAbil.HP + playerActor.BonusAbil.MP + playerActor.BonusAbil.Hit + playerActor.BonusAbil.Speed;
            nTotlePoint = nTotlePoint + playerActor.BonusPoint;
            char cMethod = questConditionInfo.sParam1[0];
            int nCount = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam2, ref nCount);
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

        private bool ConditionOfCheckHP_CheckHigh(char cMethodMax, IActor baseObject, long nMax)
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

        private void ConditionOfCheckHp(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            char cMethodMin = questConditionInfo.sParam1[0];
            char cMethodMax = questConditionInfo.sParam3[1];
            int nMin = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            int nMax = HUtil32.StrToInt(questConditionInfo.sParam4, -1);
            if (nMin < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam2, ref nMin);
            }
            if (nMax < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam4, ref nMax);
            }
            if (nMin < 0 || nMax < 0)
            {
                ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKHP);
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

        public bool ConditionOfCheckMP_CheckHigh(char cMethodMax, IActor baseObject, long nMax)
        {
            bool success = false;
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

        private void ConditionOfCheckMp(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            char cMethodMin = questConditionInfo.sParam1[0];
            char cMethodMax = questConditionInfo.sParam3[1];
            int nMin = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            int nMax = HUtil32.StrToInt(questConditionInfo.sParam4, -1);
            if (nMin < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam2, ref nMin);
            }
            if (nMax < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam4, ref nMax);
            }
            if (nMin < 0 || nMax < 0)
            {
                ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKMP);
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

        private bool ConditionOfCheckDC_CheckHigh(char cMethodMax, IActor baseObject, long nMax)
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

        private void ConditionOfCheckDc(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            char cMethodMin = questConditionInfo.sParam1[0];
            char cMethodMax = questConditionInfo.sParam3[1];
            int nMin = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            int nMax = HUtil32.StrToInt(questConditionInfo.sParam4, -1);
            if (nMin < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam2, ref nMin);
            }
            if (nMax < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam4, ref nMax);
            }
            if (nMin < 0 || nMax < 0)
            {
                ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKDC);
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
        }

        private void ConditionOfCheckMC(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            char cMethodMin = questConditionInfo.sParam1[0];
            char cMethodMax = questConditionInfo.sParam1[2];
            int nMin = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            int nMax = HUtil32.StrToInt(questConditionInfo.sParam4, -1);
            if ((nMin < 0) || (nMax < 0))
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKMC);
                return;
            }
            switch (cMethodMin)
            {
                case '=':
                    if (HUtil32.LoWord(playerActor.WAbil.MC) == nMin)
                    {
                        success = ConditionOfCheckMC_CheckHigh(playerActor, cMethodMax, nMax);
                    }
                    break;
                case '>':
                    if (HUtil32.LoWord(playerActor.WAbil.MC) > nMin)
                    {
                        success = ConditionOfCheckMC_CheckHigh(playerActor, cMethodMax, nMax);
                    }
                    break;
                case '<':
                    if (HUtil32.LoWord(playerActor.WAbil.MC) < nMin)
                    {
                        success = ConditionOfCheckMC_CheckHigh(playerActor, cMethodMax, nMax);
                    }
                    break;
                default:
                    if (HUtil32.LoWord(playerActor.WAbil.MC) >= nMin)
                    {
                        success = ConditionOfCheckMC_CheckHigh(playerActor, cMethodMax, nMax);
                    }
                    break;
            }
        }

        public bool ConditionOfCheckMC_CheckHigh(IActor baseObject, char cMethodMax, long nMax)
        {
            bool success = false;
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

        private bool ConditionOfCheckSC_CheckHigh(char cMethodMax, IActor baseObject, long nMax)
        {
            bool success = false;
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

        private void ConditionOfCheckSc(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            char cMethodMin = questConditionInfo.sParam1[0];
            char cMethodMax = questConditionInfo.sParam3[1];
            int nMin = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            int nMax = HUtil32.StrToInt(questConditionInfo.sParam4, -1);
            if (nMin < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam2, ref nMin);
            }
            if (nMax < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam4, ref nMax);
            }
            if (nMin < 0 || nMax < 0)
            {
                ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKSC);
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

        private void ConditionOfCheckExp(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int dwExp = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (dwExp < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam2, ref dwExp);
            }
            if (dwExp < 0)
            {
                ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKEXP);
                return;
            }
            char cMethod = questConditionInfo.sParam1[0];
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

        private void ConditionOfCheckFlourishPoint(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nPoint = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nPoint < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam2, ref nPoint);
                if (nPoint < 0)
                {
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKFLOURISHPOINT);
                    return;
                }
            }
            if (playerActor.MyGuild == null)
            {
                return;
            }
            IGuild guild = playerActor.MyGuild;
            char cMethod = questConditionInfo.sParam1[0];
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

        private void ConditionOfCheckChiefItemCount(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            long nCount = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKFLOURISHPOINT);
                return;
            }
            if (playerActor.MyGuild == null)
            {
                return;
            }
            IGuild guild = playerActor.MyGuild;
            char cMethod = questConditionInfo.sParam1[0];
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

        private void ConditionOfCheckGuildAuraePoint(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nPoint = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nPoint < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam2, ref nPoint);
                if (nPoint < 0)
                {
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKAURAEPOINT);
                    return;
                }
            }
            if (playerActor.MyGuild == null)
            {
                return;
            }
            IGuild guild = playerActor.MyGuild;
            char cMethod = questConditionInfo.sParam1[0];
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

        private void ConditionOfCheckGuildBuildPoint(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nPoint = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nPoint < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam2, ref nPoint);
                if (nPoint < 0)
                {
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKBUILDPOINT);
                    return;
                }
            }
            if (playerActor.MyGuild == null)
            {
                return;
            }
            IGuild guild = playerActor.MyGuild;
            char cMethod = questConditionInfo.sParam1[0];
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

        private void ConditionOfCheckStabilityPoint(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nPoint = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nPoint < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam2, ref nPoint);
                if (nPoint < 0)
                {
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKSTABILITYPOINT);
                    return;
                }
            }
            if (playerActor.MyGuild == null)
            {
                return;
            }
            IGuild guild = playerActor.MyGuild;
            char cMethod = questConditionInfo.sParam1[0];
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

        private void ConditionOfCheckGameGold(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nGameGold = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nGameGold < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam2, ref nGameGold);
                if (nGameGold < 0)
                {
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKGAMEGOLD);
                    return;
                }
            }
            char cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (playerActor.GameGold == nGameGold)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (playerActor.GameGold > nGameGold)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (playerActor.GameGold < nGameGold)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (playerActor.GameGold >= nGameGold)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckGamePoint(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nGamePoint = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nGamePoint < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam2, ref nGamePoint);
                if (nGamePoint < 0)
                {
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKGAMEPOINT);
                    return;
                }
            }
            char cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (playerActor.GamePoint == nGamePoint)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (playerActor.GamePoint > nGamePoint)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (playerActor.GamePoint < nGamePoint)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (playerActor.GamePoint >= nGamePoint)
                    {
                        success = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 检查组队成员数
        /// </summary>
        private void ConditionOfCheckGroupCount(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (playerActor.GroupOwner == 0)
            {
                return;
            }
            int nCount = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam2, ref nCount);
                if (nCount < 0)
                {
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKGROUPCOUNT);
                    return;
                }
            }
            char cMethod = questConditionInfo.sParam1[0];
            //var groupOwner = (IPlayerActor)M2Share.ActorMgr.Get(playerActor.GroupOwner);
            //switch (cMethod)
            //{
            //    case '=':
            //        if (groupOwner.GroupMembers.Count == nCount)
            //        {
            //            success = true;
            //        }
            //        break;

            //    case '>':
            //        if (groupOwner.GroupMembers.Count > nCount)
            //        {
            //            success = true;
            //        }
            //        break;

            //    case '<':
            //        if (groupOwner.GroupMembers.Count < nCount)
            //        {
            //            success = true;
            //        }
            //        break;
            //    default:
            //        if (groupOwner.GroupMembers.Count >= nCount)
            //        {
            //            success = true;
            //        }
            //        break;
            //}
        }

        /// <summary>
        /// 是否加入行会
        /// </summary>
        private void ConditionOfCheckHaveGuild(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = playerActor.MyGuild != null;
        }

        /// <summary>
        /// 是否为攻城方
        /// </summary>
        private void ConditionOfCheckIsAttackGuild(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            if (playerActor.Castle == null)
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.ISATTACKGUILD);
                return;
            }
            if (playerActor.MyGuild == null)
            {
                return;
            }
            success = playerActor.Castle.IsAttackGuild(playerActor.MyGuild);
        }

        /// <summary>
        /// 检查占领沙巴克天数
        /// 格式：CASTLECHANGEDAY 空字符（>） (天数)7 
        /// </summary>
        /// <returns></returns>
        private void ConditionOfCheckCastleChageDay(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nDay = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nDay < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam2, ref nDay);
            }
            if (nDay < 0 || playerActor.Castle == null)
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CASTLECHANGEDAY);
                return;
            }
            int nChangeDay = HUtil32.GetDayCount(DateTime.Now, playerActor.Castle.ChangeDate);
            char cMethod = questConditionInfo.sParam1[0];
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
        /// <returns></returns>
        private void ConditionOfCheckCastleWarDay(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nDay = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nDay < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam2, ref nDay);
            }
            if (nDay < 0 || playerActor.Castle == null)
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CASTLEWARDAY);
                return;
            }
            int nWarDay = HUtil32.GetDayCount(DateTime.Now, playerActor.Castle.WarDate);
            char cMethod = questConditionInfo.sParam1[0];
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
        /// <returns></returns>
        private void ConditionOfCheckCastleDoorStatus(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nDay = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nDay < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam2, ref nDay);
            }
            int nDoorStatus = -1;
            if (string.Compare(questConditionInfo.sParam1, "损坏", StringComparison.OrdinalIgnoreCase) == 0)
            {
                nDoorStatus = 0;
            }
            if (string.Compare(questConditionInfo.sParam1, "开启", StringComparison.OrdinalIgnoreCase) == 0)
            {
                nDoorStatus = 1;
            }
            if (string.Compare(questConditionInfo.sParam1, "关闭", StringComparison.OrdinalIgnoreCase) == 0)
            {
                nDoorStatus = 2;
            }
            if (nDay < 0 || playerActor.Castle == null || nDoorStatus < 0)
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKCASTLEDOOR);
                return;
            }
            //CastleDoor castleDoor = (CastleDoor)playerActor.Castle.MainDoor.BaseObject;
            //switch (nDoorStatus)
            //{
            //    case 0:
            //        if (castleDoor.Death)
            //        {
            //            success = true;
            //        }
            //        break;
            //    case 1:
            //        if (castleDoor.IsOpened)
            //        {
            //            success = true;
            //        }
            //        break;
            //    case 2:
            //        if (!castleDoor.IsOpened)
            //        {
            //            success = true;
            //        }
            //        break;
            //}
        }

        private void ConditionOfCheckIsAttackAllyGuild(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (playerActor.Castle == null)
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.ISATTACKALLYGUILD);
                return;
            }
            if (playerActor.MyGuild == null)
            {
                return;
            }
            success = playerActor.Castle.IsAttackAllyGuild(playerActor.MyGuild);
        }

        /// <summary>
        /// 检查当前行会是否为守城方联盟行会
        /// </summary>
        /// <returns></returns>
        private void ConditionOfCheckIsDefenseAllyGuild(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (playerActor.Castle == null)
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.ISDEFENSEALLYGUILD);
                return;
            }
            if (playerActor.MyGuild == null)
            {
                return;
            }
            success = playerActor.Castle.IsDefenseAllyGuild(playerActor.MyGuild);
        }

        /// <summary>
        /// 检查当前行会是否为守城方行会
        /// </summary>
        /// <returns></returns>
        private void ConditionOfCheckIsDefenseGuild(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (playerActor.Castle == null)
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.ISDEFENSEGUILD);
                return;
            }
            if (playerActor.MyGuild == null)
            {
                return;
            }
            success = playerActor.Castle.IsDefenseGuild(playerActor.MyGuild);
        }

        /// <summary>
        /// 检查当前行会是否属于沙巴克
        /// </summary>
        /// <returns></returns>
        private void ConditionOfCheckIsCastleaGuild(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            //success = M2Share.CastleMgr.IsCastleMember != null;
        }

        /// <summary>
        /// 检查玩家是否是沙巴克城主
        /// </summary>
        /// <returns></returns>
        private void ConditionOfCheckIsCastleMaster(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            //success = playerActor.IsGuildMaster() && M2Share.CastleMgr.IsCastleMember != null;
        }

        /// <summary>
        /// 检查玩家是否是行会掌门人
        /// </summary>
        /// <returns></returns>
        private void ConditionOfCheckIsGuildMaster(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = playerActor.IsGuildMaster();
        }

        /// <summary>
        /// 检查玩家是否是别人师傅
        /// </summary>
        private void ConditionOfCheckIsMaster(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = !string.IsNullOrEmpty(playerActor.MasterName) && playerActor.IsMaster;
        }

        private void ConditionOfCheckListCount(IPlayerActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
        }

        private void ConditionOfCheckItemType(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nWhere = HUtil32.StrToInt(questConditionInfo.sParam1, -1);
            int nType = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nWhere < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam1, ref nWhere);
            }
            if (nType < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam2, ref nType);
            }
            if (!(nWhere >= baseObject.UseItems.GetLowerBound(0) && nWhere <= baseObject.UseItems.GetUpperBound(0)))
            {
                ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKITEMTYPE);
                success = false;
            }
            UserItem userItem = baseObject.UseItems[nWhere];
            if (userItem.Index == 0)
            {
                success = false;
            }
            StdItem stdItem = SystemShare.EquipmentSystem.GetStdItem(userItem.Index);
            if (stdItem != null && stdItem.StdMode == nType)
            {
                success = true;
            }
        }

        private void ConditionOfCheckLevelEx(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nLevel = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam2, ref nLevel);
                if (nLevel < 0)
                {
                    ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKLEVELEX);
                    return;
                }
            }
            char cMethod = questConditionInfo.sParam1[0];
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

        private void ConditionOfCheckBagItemInList(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            string sLineText;
            string sVar = string.Empty;
            string sValue = string.Empty;
            int nValue = 0;
            string sListFileName = SystemShare.GetEnvirFilePath(normNpc.Path, questConditionInfo.sParam1);
            int nItemCount = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            StringList loadList = new StringList();
            if (File.Exists(sListFileName))
            {
                try
                {
                    loadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    LogService.Error("loading fail.... => " + sListFileName);
                }
            }
            else
            {
                LogService.Info("file not found => " + sListFileName);
            }
            if (nItemCount < 0 && baseObject.Race == ActorRace.Play)
            {
                GetVarValue(baseObject, questConditionInfo.sParam2, ref nItemCount);
            }
            if (nItemCount < 0)
            {
                ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKBAGITEMINLIST);
                return;
            }
            for (int i = 0; i < loadList.Count; i++)
            {
                sLineText = loadList[i].Trim();
                if (sLineText == "" || sLineText[1] == ';')
                {
                    continue;
                }
                string sItemName = loadList[i];
                if (nItemCount <= 0)
                {
                    break;
                }
                for (int j = 0; j < baseObject.ItemList.Count; j++)
                {
                    UserItem userItem = baseObject.ItemList[j];
                    if (string.Compare(SystemShare.EquipmentSystem.GetStdItemName(userItem.Index), sItemName, StringComparison.Ordinal) == 0)
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
                    VarInfo varInfo = GetVarValue(baseObject, questConditionInfo.sParam3, ref sVar, ref sValue, ref nValue);
                    switch (varInfo.VarAttr)
                    {
                        case VarAttr.aNone:
                        case VarAttr.aConst:
                            ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKBAGITEMINLIST);
                            break;
                        case VarAttr.aFixStr:
                            SetValNameValue(baseObject, sVar, sValue, nItemCount);
                            break;
                        case VarAttr.aDynamic:
                            SetDynamicValue(baseObject, sVar, sValue, nItemCount);
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

        private void ConditionOfCheckMarry(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (!string.IsNullOrEmpty(playerActor.DearName))
            {
                success = true;
            }
        }

        private void ConditionOfCheckMarryCount(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nCount = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam2, ref nCount);
                if (nCount < 0)
                {
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKMARRYCOUNT);
                    return;
                }
            }
            char cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (playerActor.MarryCount == nCount)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (playerActor.MarryCount > nCount)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (playerActor.MarryCount < nCount)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (playerActor.MarryCount >= nCount)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckMaster(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = !string.IsNullOrEmpty(playerActor.MasterName) && !playerActor.IsMaster;
        }

        private void ConditionOfCheckMemBerLevel(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nLevel = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam2, ref nLevel);
                if (nLevel < 0)
                {
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKMEMBERLEVEL);
                    return;
                }
            }
            char cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (playerActor.MemberLevel == nLevel)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (playerActor.MemberLevel > nLevel)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (playerActor.MemberLevel < nLevel)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (playerActor.MemberLevel >= nLevel)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckMemberType(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nType = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nType < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam2, ref nType);
                if (nType < 0)
                {
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKMEMBERTYPE);
                    return;
                }
            }
            char cMethod = questConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (playerActor.MemberType == nType)
                    {
                        success = true;
                    }
                    break;

                case '>':
                    if (playerActor.MemberType > nType)
                    {
                        success = true;
                    }
                    break;

                case '<':
                    if (playerActor.MemberType < nType)
                    {
                        success = true;
                    }
                    break;
                default:
                    if (playerActor.MemberType >= nType)
                    {
                        success = true;
                    }
                    break;
            }
        }

        private void ConditionOfCheckNameIpList(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            string sName = string.Empty;
            StringList loadList = new StringList();
            try
            {
                string sChrName = playerActor.ChrName;
                string sCharIPaddr = playerActor.LoginIpAddr;
                if (File.Exists(SystemShare.GetEnvirFilePath(normNpc.Path, questConditionInfo.sParam1)))
                {
                    loadList.LoadFromFile(SystemShare.GetEnvirFilePath(normNpc.Path, questConditionInfo.sParam1));
                    for (int i = 0; i < loadList.Count; i++)
                    {
                        string sLine = loadList[i];
                        if (sLine[0] == ';')
                        {
                            continue;
                        }
                        string sIPaddr = HUtil32.GetValidStr3(sLine, ref sName, new string[] { " ", "/", "\09" });
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
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKNAMEIPLIST);
                }
            }
            finally
            {
                Dispose(loadList);
            }
        }

        private void ConditionOfCheckPoseDir(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            IPlayerActor poseHuman = (IPlayerActor)baseObject.GetPoseCreate();
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

        private void ConditionOfCheckPoseGender(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
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
            IPlayerActor poseHuman = (IPlayerActor)baseObject.GetPoseCreate();
            if (poseHuman != null)
            {
                if (poseHuman.Gender == (PlayerGender)btSex)
                {
                    success = true;
                }
            }
        }

        private void ConditionOfCheckPoseIsMaster(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            IPlayerActor poseHuman = (IPlayerActor)playerActor.GetPoseCreate();
            if (poseHuman != null && poseHuman.Race == ActorRace.Play)
            {
                if ((poseHuman).MasterName != "" && (poseHuman).IsMaster)
                {
                    success = true;
                }
            }
        }

        private void ConditionOfCheckPoseLevel(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nLevel = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue(baseObject, questConditionInfo.sParam2, ref nLevel);
                    if (nLevel < 0)
                    {
                        ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKPOSELEVEL);
                        return;
                    }
                }
                else
                {
                    ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKPOSELEVEL);
                    return;
                }
            }
            char cMethod = questConditionInfo.sParam1[0];
            IActor poseHuman = baseObject.GetPoseCreate();
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

        private void ConditionOfCheckPoseMarry(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            IPlayerActor poseHuman = (IPlayerActor)playerActor.GetPoseCreate();
            if (poseHuman != null && poseHuman.Race == ActorRace.Play)
            {
                if ((poseHuman).DearName != "")
                {
                    success = true;
                }
            }
        }

        private void ConditionOfCheckPoseMaster(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            IPlayerActor poseHuman = (IPlayerActor)playerActor.GetPoseCreate();
            if (poseHuman != null && poseHuman.Race == ActorRace.Play)
            {
                if ((poseHuman).MasterName != "" && !(poseHuman).IsMaster)
                {
                    success = true;
                }
            }
        }

        private void ConditionOfCheckServerName(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (questConditionInfo.sParam1 == SystemShare.Config.ServerName)
            {
                success = true;
            }
        }

        private void ConditionOfCheckSlaveCount(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nCount = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue(baseObject, questConditionInfo.sParam2, ref nCount);
                    if (nCount < 0)
                    {
                        ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKSLAVECOUNT);
                        return;
                    }
                }
                else
                {
                    ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKSLAVECOUNT);
                    return;
                }
            }
            char cMethod = questConditionInfo.sParam1[0];
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
        private void ConditionOfCheckSafeZone(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = baseObject.InSafeZone();
        }

        private void ConditionOfCheckMapName(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            string sChrName = baseObject.ChrName;
            string sMapName = questConditionInfo.sParam1;
            if (SystemShare.MapMgr.FindMap(sMapName) == null)
            {
                GetVarValue(baseObject, questConditionInfo.sParam1, ref sMapName);
            }
            if (sMapName == baseObject.MapName)
            {
                success = true;
            }
        }

        private void ConditionOfCheckSkill(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nSkillLevel = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
            if (nSkillLevel < 0)
            {
                if (baseObject.Race == ActorRace.Play)
                {
                    GetVarValue(baseObject, questConditionInfo.sParam3, ref nSkillLevel);
                    if (nSkillLevel < 0)
                    {
                        ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKSKILL);
                        return;
                    }
                }
            }
            if (baseObject.Race == ActorRace.Play)
            {
                UserMagic userMagic = baseObject.GetMagicInfo(questConditionInfo.sParam1);
                if (userMagic == null)
                {
                    return;
                }
                char cMethod = questConditionInfo.sParam2[1];
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
        }

        private void ConditionOfAnsiContainsText(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            string sValue1 = questConditionInfo.sParam1;
            string sValue2 = questConditionInfo.sParam2;
            sValue1 = GetLineVariableText(playerActor, questConditionInfo.sParam1);
            sValue2 = GetLineVariableText(playerActor, questConditionInfo.sParam2);
            GetVarValue(playerActor, sValue1, ref sValue1);
            GetVarValue(playerActor, sValue2, ref sValue2);
            success = sValue1.IndexOf(sValue2, StringComparison.OrdinalIgnoreCase) != -1;
        }

        private void ConditionOfCompareText(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            string sValue1 = GetLineVariableText(playerActor, questConditionInfo.sParam1);
            string sValue2 = GetLineVariableText(playerActor, questConditionInfo.sParam2);
            GetVarValue(playerActor, sValue1, ref sValue1);
            GetVarValue(playerActor, sValue2, ref sValue2);
            success = string.Compare(sValue1, sValue2, StringComparison.OrdinalIgnoreCase) == 0;
        }

        private void ConditionOfCheckPkPoint(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int pvpPoint = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            if (pvpPoint < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam2, ref pvpPoint);
            }
            if (pvpPoint < 0)
            {
                ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKPKPOINTEX);
                return;
            }
            char cMethod = questConditionInfo.sParam1[0];
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

        private void ConditionOfIsUnderWar(IActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            //UserCastle userCastle;
            //string sCastleName = questConditionInfo.sParam1;
            //if (sCastleName != "")
            //{
            //    HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            //    try
            //    {
            //        for (int i = 0; i < M2Share.CastleMgr.CastleList.Count; i++)
            //        {
            //            userCastle = M2Share.CastleMgr.CastleList[i];
            //            if (string.Compare(userCastle.sName, sCastleName, StringComparison.Ordinal) == 0)
            //            {
            //                success = userCastle.UnderWar;
            //                break;
            //            }
            //        }
            //    }
            //    finally
            //    {
            //        HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            //    }
            //}
        }

        private void ConditionOfInCastleWarArea(IActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            //HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            //try
            //{
            //    success = M2Share.CastleMgr.InCastleWarArea(baseObject.Envir, baseObject.CurrX, baseObject.CurrY) != null;
            //}
            //finally
            //{
            //    HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            //}
        }

        private void ConditionOfCheckInCurrRect(INormNpc normNpc, IPlayerActor baseObject, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nX = HUtil32.StrToInt(questConditionInfo.sParam1, -1);
            int nY = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            int nRange = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
            if (nX < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam1, ref nX);
            }
            if (nX < 0)
            {
                ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKINCURRRECT);
                return;
            }
            if (nY < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam2, ref nY);
            }
            if (nY < 0)
            {
                ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKINCURRRECT);
                return;
            }
            if (nRange < 0)
            {
                GetVarValue(baseObject, questConditionInfo.sParam3, ref nRange);
            }
            if (nRange < 0)
            {
                ScriptConditionError(normNpc, baseObject, questConditionInfo, ConditionCode.CHECKINCURRRECT);
                return;
            }
            int nLeft = HUtil32._MAX(nX - nRange, 0);
            int nRight = HUtil32._MIN(nX + nRange, baseObject.Envir.Width);
            int nTop = HUtil32._MAX(nY - nRange, 0);
            int nBottom = HUtil32._MIN(nY + nRange, baseObject.Envir.Height);
            success = false;
            //var currRect = new TRect(nLeft, nTop, nRight, nBottom);
            //success = baseObject.CurrX >= currRect.Left && baseObject.CurrY >= currRect.Top && baseObject.CurrX <= currRect.Right && baseObject.CurrY <= currRect.Bottom;
        }

        private void ConditionOfCheckGuildMember(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (playerActor.MyGuild != null)
            {
                string sChrName = questConditionInfo.sParam1;
                sChrName = GetLineVariableText(playerActor, sChrName);
                GetVarValue(playerActor, sChrName, ref sChrName);
                success = playerActor.MyGuild.IsMember(sChrName);
            }
        }

        private void ConditionOfIndexOf(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            string sVar = string.Empty;
            string sValue = string.Empty;
            int nValue = 0;
            int nPostion = -1;
            GetVarValue(playerActor, questConditionInfo.sParam2, ref sValue);
            StringList loadList = new StringList();
            try
            {
                if (File.Exists(SystemShare.GetEnvirFilePath(normNpc.Path, questConditionInfo.sParam1)))
                {
                    loadList.LoadFromFile(SystemShare.GetEnvirFilePath(normNpc.Path, questConditionInfo.sParam1));
                    nPostion = loadList.IndexOf(sValue);
                    success = nPostion >= 0;
                }
                else
                {
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.INDEXOF);
                }
            }
            finally
            {
                Dispose(loadList);
            }
            if (questConditionInfo.sParam3 != "")
            {
                VarInfo varInfo = GetVarValue(playerActor, questConditionInfo.sParam3, ref sVar, ref sValue, ref nValue);
                switch (varInfo.VarAttr)
                {
                    case VarAttr.aNone:
                    case VarAttr.aConst:
                        ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.INDEXOF);
                        break;
                    case VarAttr.aFixStr:
                        SetValNameValue(playerActor, sVar, sValue, nPostion);
                        break;
                    case VarAttr.aDynamic:
                        SetDynamicValue(playerActor, sVar, sValue, nPostion);
                        break;
                }
            }
            else
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.INDEXOF);
            }
        }

        /// <summary>
        /// 检查指定变量是否大于指定参数
        /// </summary>
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfLapge(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int n14 = 0;
            int n18 = 0;
            if (CheckVarNameNo(normNpc, playerActor, questConditionInfo, n14, n18) && n14 > n18)
            {
                success = true;
            }
        }

        /// <summary>
        /// 检查指定变量是否小于指定参数
        /// </summary>
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfSmall(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int n14 = 0;
            int n18 = 0;
            if (CheckVarNameNo(normNpc, playerActor, questConditionInfo, n14, n18) && n14 < n18)
            {
                success = true;
            }
        }

        /// <summary>
        /// 检查玩家权限是否大于4
        /// </summary>
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfIssysop(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            if (!(playerActor.Permission >= 4))
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查玩家权限是否大于6
        /// </summary>
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfIsAdmin(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            if (!(playerActor.Permission >= 6))
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查小时是否相等
        /// </summary>
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfHour(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
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
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfMin(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
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
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfDayOfWeek(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
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
        private void ConditionOfCheckDura(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            int n1C = 0;
            int nMaxDura = 0;
            int nDura = 0;
            playerActor.QuestCheckItem(questConditionInfo.sParam1, ref n1C, ref nMaxDura, ref nDura);
            if (HUtil32.Round(nDura / 1000.0) < questConditionInfo.nParam2)
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查游戏物品
        /// </summary>
        /// <param name="playerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckItem(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            int n1C = 0;
            int nMaxDura = 0;
            int nDura = 0;
            string s01 = questConditionInfo.sParam1;
            GetVarValue(playerActor, questConditionInfo.sParam1, ref s01);
            playerActor.QuestCheckItem(s01, ref n1C, ref nMaxDura, ref nDura);
            if (n1C < questConditionInfo.nParam2)
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查玩家职业
        /// </summary>
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckJob(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            if (HUtil32.CompareLStr(questConditionInfo.sParam1, ScriptFlagConst.sWarrior))
            {
                if (playerActor.Job != PlayerJob.Warrior)
                {
                    success = false;
                }
            }
            if (HUtil32.CompareLStr(questConditionInfo.sParam1, ScriptFlagConst.sWizard))
            {
                if (playerActor.Job != PlayerJob.Wizard)
                {
                    success = false;
                }
            }
            if (HUtil32.CompareLStr(questConditionInfo.sParam1, ScriptFlagConst.sTaos))
            {
                if (playerActor.Job != PlayerJob.Taoist)
                {
                    success = false;
                }
            }
        }

        /// <summary>
        /// 检查玩家等级
        /// </summary>
        /// <param name="IPlayerActor">玩家对象</param>
        /// <param name="questConditionInfo">脚本参数</param>
        /// <param name="success">返回结果</param>
        private void ConditionOfCheckLevel(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            if (playerActor.Abil.Level < questConditionInfo.nParam1)
            {
                success = false;
            }
        }

        private void ConditionCheckUnit(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            int n14 = HUtil32.StrToInt(questConditionInfo.sParam1, 0);
            int n18 = HUtil32.StrToInt(questConditionInfo.sParam2, 0);
            int n10 = playerActor.GetQuestUnitStatus(n14);
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
        /// 检查玩家性别
        /// </summary>
        /// <param name="IPlayerActor">玩家对象</param>
        /// <param name="questConditionInfo">脚本参数</param>
        /// <param name="success">返回结果</param>
        private void ConditionOfGender(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            if (string.Compare(questConditionInfo.sParam1, ScriptFlagConst.sMAN, StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (playerActor.Gender != PlayerGender.Man)
                {
                    success = false;
                }
            }
            else
            {
                if (playerActor.Gender != PlayerGender.WoMan)
                {
                    success = false;
                }
            }
        }

        private void ConditionOfRandom(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            if (SystemShare.RandomNumber.Random(questConditionInfo.nParam1) != 0)
            {
                success = false;
            }
        }

        private void ConditionOfCheckGold(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            int n14 = HUtil32.StrToInt(questConditionInfo.sParam1, -1);
            if (n14 < 0)
            {
                GetVarValue(playerActor, questConditionInfo.sParam1, ref n14);
            }
            if (playerActor.Gold < n14)
            {
                success = false;
            }
        }

        private void ConditionOfCheck(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            int n14 = HUtil32.StrToInt(questConditionInfo.sParam1, 0);
            int n18 = HUtil32.StrToInt(questConditionInfo.sParam2, 0);
            int n10 = playerActor.GetQuestFlagStatus(n14);
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
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfIsNewHuman(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = playerActor.IsNewHuman;
        }

        /// <summary>
        /// 检查人物名字在文件中的位置
        /// </summary>
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckNameListPostion(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            string sChrName;
            int nNamePostion = -1;
            string sVar = string.Empty;
            string sValue = string.Empty;
            int nValue = 0;
            VarInfo varInfo;
            StringList loadList = null;
            try
            {
                sChrName = playerActor.ChrName;
                if (File.Exists(SystemShare.GetEnvirFilePath(normNpc.Path, questConditionInfo.sParam1)))
                {
                    loadList = new StringList();
                    loadList.LoadFromFile(SystemShare.GetEnvirFilePath(normNpc.Path, questConditionInfo.sParam1));
                    for (int i = 0; i < loadList.Count; i++)
                    {
                        string sLine = loadList[i].Trim();
                        if (sLine == "" || sLine[0] == ';')
                        {
                            continue;
                        }
                        if (string.Compare(sLine, sChrName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            nNamePostion = i + 1;
                            break;
                        }
                    }
                }
                else
                {
                    ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKNAMELISTPOSITION);
                }
            }
            finally
            {
                Dispose(loadList);
            }
            char cMethod = questConditionInfo.sParam2[1];
            long nPostion = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
            if (questConditionInfo.sParam4 != "" && playerActor.Race == ActorRace.Play)
            {
                varInfo = GetVarValue(playerActor, questConditionInfo.sParam4, ref sVar, ref sValue, ref nValue);
                switch (varInfo.VarAttr)
                {
                    case VarAttr.aNone:
                    case VarAttr.aConst:
                        ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKNAMELISTPOSITION);
                        return;
                    case VarAttr.aFixStr:
                        SetValNameValue(playerActor, sVar, sValue, nNamePostion);
                        break;

                    case VarAttr.aDynamic:
                        SetDynamicValue(playerActor, sVar, sValue, nNamePostion);
                        break;
                }
            }
            if (nPostion < 0)
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKNAMELISTPOSITION);
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
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckGuildList(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            if (playerActor.MyGuild != null)
            {
                if (!GotoLableCheckStringList(playerActor.MyGuild.GuildName, normNpc.Path + questConditionInfo.sParam1))
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
        /// <param name="playerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckSlaveName(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            if (questConditionInfo.sParam1 == "")
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKSLAVENAME);
            }
            string sSlaveName = questConditionInfo.sParam1;
            if (playerActor.Race == ActorRace.Play)
            {
                GetVarValue(playerActor, questConditionInfo.sParam1, ref sSlaveName);
            }
            for (int i = 0; i < playerActor.SlaveList.Count; i++)
            {
                IActor aObject = playerActor.SlaveList[i];
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
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckUseItem(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int nWhere = HUtil32.StrToInt(questConditionInfo.sParam1, -1);
            if (playerActor.Race == ActorRace.Play)
            {
                if (nWhere < 0)
                {
                    GetVarValue(playerActor, questConditionInfo.sParam1, ref nWhere);
                }
            }
            if (nWhere < 0 || nWhere > playerActor.UseItems.GetUpperBound(0))
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKUSEITEM);
                return;
            }
            if (playerActor.UseItems[nWhere].Index > 0)
            {
                success = true;
            }
        }

        /// <summary>
        /// 检测是否在同一地图范围内
        /// </summary>
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <returns></returns>
        private void ConditionOfCheckInMapRange(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            string sMapName = questConditionInfo.sParam1;
            int nX = HUtil32.StrToInt(questConditionInfo.sParam2, -1);
            int nY = HUtil32.StrToInt(questConditionInfo.sParam3, -1);
            int nRange = HUtil32.StrToInt(questConditionInfo.sParam4, -1);
            if (playerActor.Race == ActorRace.Play)
            {
                if (nX < 0)
                {
                    GetVarValue(playerActor, questConditionInfo.sParam2, ref nX);
                }
                if (nY < 0)
                {
                    GetVarValue(playerActor, questConditionInfo.sParam3, ref nY);
                }
                if (nRange < 0)
                {
                    GetVarValue(playerActor, questConditionInfo.sParam4, ref nRange);
                }
            }
            if (sMapName == "" || nX < 0 || nY < 0 || nRange < 0)
            {
                ScriptConditionError(normNpc, playerActor, questConditionInfo, ConditionCode.CHECKINMAPRANGE);
                return;
            }
            if (string.Compare(playerActor.MapName, sMapName, StringComparison.Ordinal) != 0)
            {
                return;
            }
            if (Math.Abs(playerActor.CurrX - nX) <= nRange && Math.Abs(playerActor.CurrY - nY) <= nRange)
            {
                success = true;
            }
        }

        /// <summary>
        /// 检查字符串是否在指定文件中
        /// </summary>
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckTextList(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            string s01 = questConditionInfo.sParam1;
            string s02 = GetLineVariableText(playerActor, questConditionInfo.sParam2);// 路径支持变量
            if (s02[0] == '\\')
            {
                s02 = s02[1..];
            }
            if (s02[1] == '\\')
            {
                s02 = s02[2..];
            }
            if (s02[2] == '\\')
            {
                s02 = s02[3..];
            }
            if (!GotoLableCheckStringList(s01, s02))
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查当前玩家是否队伍队长
        /// </summary>
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfIsGroupMaster(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            if (playerActor.GroupOwner != 0)
            {
                if (playerActor.GroupOwner != playerActor.ActorId)
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
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheCkContAinsTextList(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            string s02 = string.Empty;
            string s01 = questConditionInfo.sParam1;
            GetValValue(playerActor, questConditionInfo.sParam1, ref s01);
            if (questConditionInfo.sParam2[0] == '\\')
            {
                s02 = questConditionInfo.sParam2[1..];
            }
            if (questConditionInfo.sParam2[1] == '\\')
            {
                s02 = questConditionInfo.sParam2[2..];
            }
            if (questConditionInfo.sParam2[2] == '\\')
            {
                s02 = questConditionInfo.sParam2[3..];
            }
            s02 = GetLineVariableText(playerActor, s02);
            if (!GotoLable_CheckAnsiContainsTextList(s01, s02))
            {
                success = false;
            }
        }

        private bool GotoLable_CheckAnsiContainsTextList(string sTest, string sListFileName)
        {
            bool success = false;
            StringList loadList;
            sListFileName = SystemShare.GetEnvirFilePath(sListFileName);
            if (File.Exists(sListFileName))
            {
                loadList = new StringList();
                try
                {
                    loadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    LogService.Error("loading fail.... => " + sListFileName);
                }
                for (int i = 0; i < loadList.Count; i++)
                {
                    if (sTest.IndexOf(loadList[i].Trim()) != -1)
                    {
                        success = true;
                        break;
                    }
                }
                Dispose(loadList);
            }
            else
            {
                LogService.Error("file not found => " + sListFileName);
            }
            return success;
        }

        /// <summary>
        /// 检查玩家是否在线
        /// </summary>
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckOnLine(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            string s01 = questConditionInfo.sParam1;
            if ((s01 != "") && (s01[0] == '<') && (s01[1] == '$'))
            {
                s01 = GetLineVariableText(playerActor, questConditionInfo.sParam1);
            }
            else
            {
                GetValValue(playerActor, questConditionInfo.sParam1, ref s01);
            }
            if ((string.IsNullOrEmpty(s01)) || (SystemShare.WorldEngine.GetPlayObject(s01) == null))
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查人物是否重叠
        /// </summary>
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfIsDupMode(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            if (playerActor.Envir != null)
            {
                if (playerActor.Envir.GetXYObjCount(playerActor.CurrX, playerActor.CurrY) <= 1)
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
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckHum(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            int n14 = 0;
            if (!GetValValue(playerActor, questConditionInfo.sParam2, ref n14))
            {
                n14 = questConditionInfo.nParam2;
            }
            if (SystemShare.WorldEngine.GetMapHuman(questConditionInfo.sParam1) < n14)
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查玩家姓名是否存在指定文本中
        /// </summary>
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckNameList(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            if (!GotoLableCheckStringList(playerActor.ChrName, questConditionInfo.sParam1))
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查玩家帐号是否存在指定文本中
        /// </summary>
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckAccountList(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            if (!GotoLableCheckStringList(playerActor.UserAccount, normNpc.Path + questConditionInfo.sParam1))
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查玩家IP是否存在指定文本中
        /// </summary>
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfCheckIpList(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = true;
            if (!GotoLableCheckStringList(playerActor.LoginIpAddr, normNpc.Path + questConditionInfo.sParam1))
            {
                success = false;
            }
        }

        /// <summary>
        /// 检查指定变量是否相等
        /// </summary>
        /// <param name="IPlayerActor"></param>
        /// <param name="questConditionInfo"></param>
        /// <param name="success"></param>
        private void ConditionOfEqual(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, ref bool success)
        {
            success = false;
            int n14 = 0;
            int n18 = 0;
            if (CheckVarNameNo(normNpc, playerActor, questConditionInfo, n14, n18) && n14 == n18)
            {
                success = true;
            }
        }

        private bool CheckVarNameNo(INormNpc normNpc, IPlayerActor playerActor, QuestConditionInfo questConditionInfo, int n140, int n180)
        {
            bool success = false;
            n140 = -1;
            n180 = questConditionInfo.nParam2;
            int n100 = SystemShare.GetValNameNo(questConditionInfo.sParam1);
            if (n100 <= 0)
            {
                return success;
            }

            if (HUtil32.RangeInDefined(n100, 0, 99))
            {
                n140 = SystemShare.Config.GlobalVal[n100];// G
                success = true;
            }
            else if (HUtil32.RangeInDefined(n100, 1000, 1099))
            {
                n140 = SystemShare.Config.GlobaDyMval[n100 - 1000];// I
                success = true;
            }
            else if (HUtil32.RangeInDefined(n100, 1100, 1109))
            {
                n140 = playerActor.MNVal[n100 - 1100];// P
                success = true;
            }
            else if (HUtil32.RangeInDefined(n100, 1110, 1119))
            {
                n140 = playerActor.MDyVal[n100 - 1110];// D
                success = true;
            }
            else if (HUtil32.RangeInDefined(n100, 1200, 1299))
            {
                n140 = playerActor.MNMval[n100 - 1200];// M
                success = true;
            }
            else if (HUtil32.RangeInDefined(n100, 1300, 1399))
            {
                n140 = playerActor.MNInteger[n100 - 1300];// N
                success = true;
            }
            else if (HUtil32.RangeInDefined(n100, 2000, 2499))
            {
                if (string.Compare(SystemShare.Config.GlobalAVal[n100 - 2000], questConditionInfo.sParam2, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    n140 = 0;
                    n180 = 0;
                    success = true;
                }
            }
            else if (HUtil32.RangeInDefined(n100, 1400, 1499))
            {
                if (string.Compare(playerActor.MSString[n100 - 1400], questConditionInfo.sParam2, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    n140 = 0;
                    n180 = 0;
                    success = true;
                }
            }
            return success;
        }

        private void ScriptConditionError(INormNpc normNpc, IActor baseObject, QuestConditionInfo questConditionInfo, ConditionCode sCmd)
        {
            string sMsg = "Cmd:" + sCmd + " NPC名称:" + normNpc.ChrName + " 地图:" + normNpc.MapName + " 座标:" + normNpc.CurrX + ':' + normNpc.CurrY + " 参数1:"
                  + questConditionInfo.sParam1 + " 参数2:" + questConditionInfo.sParam2 + " 参数3:" + questConditionInfo.sParam3 + " 参数4:" + questConditionInfo.sParam4 + " 参数5:"
                  + questConditionInfo.sParam5;
            LogService.Error("[脚本参数不正确] " + sMsg);
        }

        private void ScriptConditionError(INormNpc normNpc, IActor baseObject, QuestConditionInfo questConditionInfo, string sCmd)
        {
            string sMsg = "Cmd:" + sCmd + " NPC名称:" + normNpc.ChrName + " 地图:" + normNpc.MapName + " 座标:" + normNpc.CurrX + ':' + normNpc.CurrY + " 参数1:"
                 + questConditionInfo.sParam1 + " 参数2:" + questConditionInfo.sParam2 + " 参数3:" + questConditionInfo.sParam3 + " 参数4:" + questConditionInfo.sParam4 + " 参数5:"
                  + questConditionInfo.sParam5;
            LogService.Error("[脚本参数不正确] " + sMsg);
        }

        private void Dispose(object obj)
        {
            obj = null;
        }
    }
}