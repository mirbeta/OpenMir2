using System;
using System.Collections.Generic;
using System.IO;
using SystemModule;
using SystemModule.Common;

namespace GameSvr
{
    public partial class TNormNpc
    {
        private bool ConditionOfCheckAccountIPList(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            StringList LoadList;
            string sLine;
            string sName = string.Empty;
            string sIPaddr;
            try
            {
                var sCharName = PlayObject.m_sCharName;
                var sCharAccount = PlayObject.m_sUserID;
                var sCharIPaddr = PlayObject.m_sIPaddr;
                LoadList = new StringList();
                if (File.Exists(M2Share.g_Config.sEnvirDir + QuestConditionInfo.sParam1))
                {
                    LoadList.LoadFromFile(M2Share.g_Config.sEnvirDir + QuestConditionInfo.sParam1);
                    for (var i = 0; i < LoadList.Count; i++)
                    {
                        sLine = LoadList[i];
                        if (sLine[1] == ';')
                        {
                            continue;
                        }
                        sIPaddr = HUtil32.GetValidStr3(sLine, ref sName, new string[] { " ", "/", "\t" });
                        sIPaddr = sIPaddr.Trim();
                        if ((sName == sCharAccount) && (sIPaddr == sCharIPaddr))
                        {
                            result = true;
                            break;
                        }
                    }
                }
                else
                {
                    ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKACCOUNTIPLIST);
                }
            }
            finally
            {
                //LoadList.Free;
            }
            return result;
        }

        private bool ConditionOfCheckBagSize(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nSize = QuestConditionInfo.nParam1;
            if ((nSize <= 0) || (nSize > Grobal2.MAXBAGITEM))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKBAGSIZE);
                return result;
            }
            if (PlayObject.m_ItemList.Count + nSize <= Grobal2.MAXBAGITEM)
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckBonusPoint(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nTotlePoint = this.m_BonusAbil.DC + this.m_BonusAbil.MC + this.m_BonusAbil.SC + this.m_BonusAbil.AC + this.m_BonusAbil.MAC + this.m_BonusAbil.HP + this.m_BonusAbil.MP + this.m_BonusAbil.Hit + this.m_BonusAbil.Speed + this.m_BonusAbil.X2;
            nTotlePoint = nTotlePoint + this.m_nBonusPoint;
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nTotlePoint == QuestConditionInfo.nParam2)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (nTotlePoint > QuestConditionInfo.nParam2)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (nTotlePoint < QuestConditionInfo.nParam2)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (nTotlePoint >= QuestConditionInfo.nParam2)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckHP_CheckHigh(TPlayObject PlayObject, char cMethodMax, int nMax)
        {
            var result = false;
            switch (cMethodMax)
            {
                case '=':
                    if (PlayObject.m_WAbil.MaxHP == nMax)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_WAbil.MaxHP > nMax)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_WAbil.MaxHP < nMax)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_WAbil.MaxHP >= nMax)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckHP(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var cMethodMin = QuestConditionInfo.sParam1[0];
            var cMethodMax = QuestConditionInfo.sParam1[2];
            var nMin = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            var nMax = HUtil32.Str_ToInt(QuestConditionInfo.sParam4, -1);
            if ((nMin < 0) || (nMax < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKHP);
                return result;
            }
            switch (cMethodMin)
            {
                case '=':
                    if (this.m_WAbil.HP == nMin)
                    {
                        result = ConditionOfCheckHP_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '>':
                    if (PlayObject.m_WAbil.HP > nMin)
                    {
                        result = ConditionOfCheckHP_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '<':
                    if (PlayObject.m_WAbil.HP < nMin)
                    {
                        result = ConditionOfCheckHP_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                default:
                    if (PlayObject.m_WAbil.HP >= nMin)
                    {
                        result = ConditionOfCheckHP_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckMP_CheckHigh(TPlayObject PlayObject, char cMethodMax, int nMax)
        {
            var result = false;
            switch (cMethodMax)
            {
                case '=':
                    if (PlayObject.m_WAbil.MaxMP == nMax)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_WAbil.MaxMP > nMax)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_WAbil.MaxMP < nMax)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_WAbil.MaxMP >= nMax)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckMP(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var cMethodMin = QuestConditionInfo.sParam1[0];
            var cMethodMax = QuestConditionInfo.sParam1[2];
            var nMin = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            var nMax = HUtil32.Str_ToInt(QuestConditionInfo.sParam4, -1);
            if ((nMin < 0) || (nMax < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKMP);
                return result;
            }
            switch (cMethodMin)
            {
                case '=':
                    if (this.m_WAbil.MP == nMin)
                    {
                        result = ConditionOfCheckMP_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '>':
                    if (PlayObject.m_WAbil.MP > nMin)
                    {
                        result = ConditionOfCheckMP_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '<':
                    if (PlayObject.m_WAbil.MP < nMin)
                    {
                        result = ConditionOfCheckMP_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                default:
                    if (PlayObject.m_WAbil.MP >= nMin)
                    {
                        result = ConditionOfCheckMP_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckDC_CheckHigh(TPlayObject PlayObject, char cMethodMax, int nMax)
        {
            var result = false;
            switch (cMethodMax)
            {
                case '=':

                    if (HUtil32.HiWord(PlayObject.m_WAbil.DC) == nMax)
                    {
                        result = true;
                    }
                    break;
                case '>':

                    if (HUtil32.HiWord(PlayObject.m_WAbil.DC) > nMax)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (HUtil32.HiWord(PlayObject.m_WAbil.DC) < nMax)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (HUtil32.HiWord(PlayObject.m_WAbil.DC) >= nMax)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckDC(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var cMethodMin = QuestConditionInfo.sParam1[0];
            var cMethodMax = QuestConditionInfo.sParam1[2];
            var nMin = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            var nMax = HUtil32.Str_ToInt(QuestConditionInfo.sParam4, -1);
            if ((nMin < 0) || (nMax < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKDC);
                return result;
            }
            switch (cMethodMin)
            {
                case '=':
                    if (HUtil32.LoWord(PlayObject.m_WAbil.DC) == nMin)
                    {
                        result = ConditionOfCheckDC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '>':
                    if (HUtil32.LoWord(PlayObject.m_WAbil.DC) > nMin)
                    {
                        result = ConditionOfCheckDC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '<':
                    if (HUtil32.LoWord(PlayObject.m_WAbil.DC) < nMin)
                    {
                        result = ConditionOfCheckDC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                default:
                    if (HUtil32.LoWord(PlayObject.m_WAbil.DC) >= nMin)
                    {
                        result = ConditionOfCheckDC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
            }
            result = false;
            return result;
        }

        private bool ConditionOfCheckMC_CheckHigh(TPlayObject PlayObject, char cMethodMax, int nMax)
        {
            var result = false;
            switch (cMethodMax)
            {
                case '=':
                    if (HUtil32.HiWord(PlayObject.m_WAbil.MC) == nMax)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (HUtil32.HiWord(PlayObject.m_WAbil.MC) > nMax)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (HUtil32.HiWord(PlayObject.m_WAbil.MC) < nMax)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (HUtil32.HiWord(PlayObject.m_WAbil.MC) >= nMax)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckMC(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var cMethodMin = QuestConditionInfo.sParam1[0];
            var cMethodMax = QuestConditionInfo.sParam1[2];
            var nMin = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            var nMax = HUtil32.Str_ToInt(QuestConditionInfo.sParam4, -1);
            if ((nMin < 0) || (nMax < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKMC);
                return result;
            }
            switch (cMethodMin)
            {
                case '=':
                    if (HUtil32.LoWord(PlayObject.m_WAbil.MC) == nMin)
                    {
                        result = ConditionOfCheckMC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '>':

                    if (HUtil32.LoWord(PlayObject.m_WAbil.MC) > nMin)
                    {
                        result = ConditionOfCheckMC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '<':
                    if (HUtil32.LoWord(PlayObject.m_WAbil.MC) < nMin)
                    {
                        result = ConditionOfCheckMC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                default:
                    if (HUtil32.LoWord(PlayObject.m_WAbil.MC) >= nMin)
                    {
                        result = ConditionOfCheckMC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckSC_CheckHigh(TPlayObject PlayObject, char cMethodMax, int nMax)
        {
            var result = false;
            switch (cMethodMax)
            {
                case '=':
                    if (HUtil32.HiWord(PlayObject.m_WAbil.SC) == nMax)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (HUtil32.HiWord(PlayObject.m_WAbil.SC) > nMax)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (HUtil32.HiWord(PlayObject.m_WAbil.SC) < nMax)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (HUtil32.HiWord(PlayObject.m_WAbil.SC) >= nMax)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckSC(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var cMethodMin = QuestConditionInfo.sParam1[0];
            var cMethodMax = QuestConditionInfo.sParam1[2];
            var nMin = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            var nMax = HUtil32.Str_ToInt(QuestConditionInfo.sParam4, -1);
            if ((nMin < 0) || (nMax < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKSC);
                return result;
            }
            switch (cMethodMin)
            {
                case '=':

                    if (HUtil32.LoWord(PlayObject.m_WAbil.SC) == nMin)
                    {
                        result = ConditionOfCheckSC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '>':

                    if (HUtil32.LoWord(PlayObject.m_WAbil.SC) > nMin)
                    {
                        result = ConditionOfCheckSC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '<':

                    if (HUtil32.LoWord(PlayObject.m_WAbil.SC) < nMin)
                    {
                        result = ConditionOfCheckSC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                default:

                    if (HUtil32.LoWord(PlayObject.m_WAbil.SC) >= nMin)
                    {
                        result = ConditionOfCheckSC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckExp(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var dwExp = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, 0);
            if (dwExp == 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKEXP);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_Abil.Exp == dwExp)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_Abil.Exp > dwExp)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_Abil.Exp < dwExp)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_Abil.Exp >= dwExp)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckFlourishPoint(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nPoint = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nPoint < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKFLOURISHPOINT);
                return result;
            }
            if (PlayObject.m_MyGuild == null)
            {
                return result;
            }
            var Guild = PlayObject.m_MyGuild;
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (Guild.nFlourishing == nPoint)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (Guild.nFlourishing > nPoint)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (Guild.nFlourishing < nPoint)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (Guild.nFlourishing >= nPoint)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckChiefItemCount(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nCount = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKFLOURISHPOINT);
                return result;
            }
            if (PlayObject.m_MyGuild == null)
            {
                return result;
            }
            var Guild = PlayObject.m_MyGuild;
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (Guild.nChiefItemCount == nCount)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (Guild.nChiefItemCount > nCount)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (Guild.nChiefItemCount < nCount)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (Guild.nChiefItemCount >= nCount)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckGuildAuraePoint(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nPoint = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nPoint < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKAURAEPOINT);
                return result;
            }
            if (PlayObject.m_MyGuild == null)
            {
                return result;
            }
            var Guild = PlayObject.m_MyGuild;
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (Guild.nAurae == nPoint)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (Guild.nAurae > nPoint)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (Guild.nAurae < nPoint)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (Guild.nAurae >= nPoint)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckGuildBuildPoint(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nPoint = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nPoint < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKBUILDPOINT);
                return result;
            }
            if (PlayObject.m_MyGuild == null)
            {
                return result;
            }
            var Guild = PlayObject.m_MyGuild;
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (Guild.BuildPoint == nPoint)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (Guild.BuildPoint > nPoint)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (Guild.BuildPoint < nPoint)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (Guild.BuildPoint >= nPoint)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckStabilityPoint(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nPoint = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nPoint < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKSTABILITYPOINT);
                return result;
            }
            if (PlayObject.m_MyGuild == null)
            {
                return result;
            }
            var Guild = PlayObject.m_MyGuild;
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (Guild.nStability == nPoint)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (Guild.nStability > nPoint)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (Guild.nStability < nPoint)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (Guild.nStability >= nPoint)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckGameGold(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nGameGold = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nGameGold < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKGAMEGOLD);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_nGameGold == nGameGold)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_nGameGold > nGameGold)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_nGameGold < nGameGold)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_nGameGold >= nGameGold)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckGamePoint(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nGamePoint = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nGamePoint < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKGAMEPOINT);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_nGamePoint == nGamePoint)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_nGamePoint > nGamePoint)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_nGamePoint < nGamePoint)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_nGamePoint >= nGamePoint)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckGroupCount(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if (PlayObject.m_GroupOwner == null)
            {
                return result;
            }
            var nCount = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKGROUPCOUNT);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_GroupOwner.m_GroupMembers.Count == nCount)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_GroupOwner.m_GroupMembers.Count > nCount)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_GroupOwner.m_GroupMembers.Count < nCount)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_GroupOwner.m_GroupMembers.Count >= nCount)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfIsHigh(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if (QuestConditionInfo.sParam1 == "")
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_ISHIGH);
                return result;
            }
            var cMode = QuestConditionInfo.sParam1[0];
            switch (cMode)
            {
                case 'L':
                    result = M2Share.g_HighLevelHuman == PlayObject;
                    break;
                case 'P':
                    result = M2Share.g_HighPKPointHuman == PlayObject;
                    break;
                case 'D':
                    result = M2Share.g_HighDCHuman == PlayObject;
                    break;
                case 'M':
                    result = M2Share.g_HighMCHuman == PlayObject;
                    break;
                case 'S':
                    result = M2Share.g_HighSCHuman == PlayObject;
                    break;
                default:
                    ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_ISHIGH);
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckHaveGuild(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            return PlayObject.m_MyGuild != null;
        }

        private bool ConditionOfCheckInMapRange(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var sMapName = QuestConditionInfo.sParam1;
            var nX = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            var nY = HUtil32.Str_ToInt(QuestConditionInfo.sParam3, -1);
            var nRange = HUtil32.Str_ToInt(QuestConditionInfo.sParam4, -1);
            if ((sMapName == "") || (nX < 0) || (nY < 0) || (nRange < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKINMAPRANGE);
                return result;
            }
            if (PlayObject.m_sMapName.ToLower().CompareTo(sMapName.ToLower()) != 0)
            {
                return result;
            }
            if ((Math.Abs(PlayObject.m_nCurrX - nX) <= nRange) && (Math.Abs(PlayObject.m_nCurrY - nY) <= nRange))
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckIsAttackGuild(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if (this.m_Castle == null)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_ISATTACKGUILD);
                return result;
            }
            if (PlayObject.m_MyGuild == null)
            {
                return result;
            }
            result = this.m_Castle.IsAttackGuild(PlayObject.m_MyGuild);
            return result;
        }

        private bool ConditionOfCheckCastleChageDay(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nDay = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if ((nDay < 0) || (this.m_Castle == null))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CASTLECHANGEDAY);
                return result;
            }
            var nChangeDay = HUtil32.GetDayCount(DateTime.Now, this.m_Castle.m_ChangeDate);
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nChangeDay == nDay)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (nChangeDay > nDay)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (nChangeDay < nDay)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (nChangeDay >= nDay)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckCastleWarDay(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nDay = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if ((nDay < 0) || (this.m_Castle == null))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CASTLEWARDAY);
                return result;
            }
            var nWarDay = HUtil32.GetDayCount(DateTime.Now, this.m_Castle.m_WarDate);
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nWarDay == nDay)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (nWarDay > nDay)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (nWarDay < nDay)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (nWarDay >= nDay)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckCastleDoorStatus(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nDay = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            var nDoorStatus = -1;
            if (QuestConditionInfo.sParam1.ToLower().CompareTo("损坏".ToLower()) == 0)
            {
                nDoorStatus = 0;
            }
            if (QuestConditionInfo.sParam1.ToLower().CompareTo("开启".ToLower()) == 0)
            {
                nDoorStatus = 1;
            }
            if (QuestConditionInfo.sParam1.ToLower().CompareTo("关闭".ToLower()) == 0)
            {
                nDoorStatus = 2;
            }
            if ((nDay < 0) || (this.m_Castle == null) || (nDoorStatus < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKCASTLEDOOR);
                return result;
            }
            var CastleDoor = (TCastleDoor)this.m_Castle.m_MainDoor.BaseObject;
            switch (nDoorStatus)
            {
                case 0:
                    if (CastleDoor.m_boDeath)
                    {
                        result = true;
                    }
                    break;
                case 1:
                    if (CastleDoor.m_boOpened)
                    {
                        result = true;
                    }
                    break;
                case 2:
                    if (!CastleDoor.m_boOpened)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckIsAttackAllyGuild(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if (this.m_Castle == null)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_ISATTACKALLYGUILD);
                return result;
            }
            if (PlayObject.m_MyGuild == null)
            {
                return result;
            }
            result = this.m_Castle.IsAttackAllyGuild(PlayObject.m_MyGuild);
            return result;
        }

        private bool ConditionOfCheckIsDefenseAllyGuild(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if (this.m_Castle == null)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_ISDEFENSEALLYGUILD);
                return result;
            }
            if (PlayObject.m_MyGuild == null)
            {
                return result;
            }
            result = this.m_Castle.IsDefenseAllyGuild(PlayObject.m_MyGuild);
            return result;
        }

        private bool ConditionOfCheckIsDefenseGuild(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if (this.m_Castle == null)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_ISDEFENSEGUILD);
                return result;
            }
            if (PlayObject.m_MyGuild == null)
            {
                return result;
            }
            result = this.m_Castle.IsDefenseGuild(PlayObject.m_MyGuild);
            return result;
        }

        private bool ConditionOfCheckIsCastleaGuild(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            // if (PlayObject.m_MyGuild <> nil) and (UserCastle.m_MasterGuild = PlayObject.m_MyGuild) then
            if (M2Share.CastleManager.IsCastleMember(PlayObject) != null)
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckIsCastleMaster(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            // if PlayObject.IsGuildMaster and (UserCastle.m_MasterGuild = PlayObject.m_MyGuild) then
            if (PlayObject.IsGuildMaster() && (M2Share.CastleManager.IsCastleMember(PlayObject) != null))
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckIsGuildMaster(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            return PlayObject.IsGuildMaster();
        }

        private bool ConditionOfCheckIsMaster(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if ((PlayObject.m_sMasterName != "") && PlayObject.m_boMaster)
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckListCount(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            return result;
        }

        private bool ConditionOfCheckItemAddValue(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nWhere = HUtil32.Str_ToInt(QuestConditionInfo.sParam1, -1);
            var cMethod = QuestConditionInfo.sParam2[0];
            var nAddValue = HUtil32.Str_ToInt(QuestConditionInfo.sParam3, -1);
            if (!(nWhere >= PlayObject.m_UseItems.GetLowerBound(0) && nWhere <= PlayObject.m_UseItems.GetUpperBound(0)) || (nAddValue < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKITEMADDVALUE);
                return result;
            }
            var UserItem = PlayObject.m_UseItems[nWhere];
            if (UserItem.wIndex == 0)
            {
                return result;
            }
            var nAddAllValue = 0;
            for (var i = UserItem.btValue.GetLowerBound(0); i <= UserItem.btValue.GetUpperBound(0); i++)
            {
                nAddAllValue += UserItem.btValue[i];
            }
            cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nAddAllValue == nAddValue)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (nAddAllValue > nAddValue)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (nAddAllValue < nAddValue)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (nAddAllValue >= nAddValue)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckItemType(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nWhere = HUtil32.Str_ToInt(QuestConditionInfo.sParam1, -1);
            var nType = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (!(nWhere >= PlayObject.m_UseItems.GetLowerBound(0) && nWhere <= PlayObject.m_UseItems.GetUpperBound(0)))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKITEMTYPE);
                return result;
            }
            var UserItem = PlayObject.m_UseItems[nWhere];
            if (UserItem == null && UserItem.wIndex == 0)
            {
                return result;
            }
            var Stditem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
            if ((Stditem != null) && (Stditem.StdMode == nType))
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckLevelEx(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nLevel = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKLEVELEX);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_Abil.Level == nLevel)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_Abil.Level > nLevel)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_Abil.Level < nLevel)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_Abil.Level >= nLevel)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckNameListPostion(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            StringList LoadList;
            string sLine;
            var result = false;
            var nNamePostion = -1;
            try
            {
                var sCharName = PlayObject.m_sCharName;
                LoadList = new StringList();
                if (File.Exists(M2Share.g_Config.sEnvirDir + QuestConditionInfo.sParam1))
                {
                    LoadList.LoadFromFile(M2Share.g_Config.sEnvirDir + QuestConditionInfo.sParam1);
                    for (var i = 0; i < LoadList.Count; i++)
                    {
                        sLine = LoadList[i].Trim();
                        if (sLine[1] == ';')
                        {
                            continue;
                        }
                        if (sLine.ToLower().CompareTo(sCharName.ToLower()) == 0)
                        {
                            nNamePostion = i;
                            break;
                        }
                    }
                }
                else
                {
                    ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKNAMELISTPOSITION);
                }
            }
            finally
            {
                //LoadList.Free;
            }
            var nPostion = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nPostion < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKNAMELISTPOSITION);
                return result;
            }
            if (nNamePostion >= nPostion)
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckMarry(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if (PlayObject.m_sDearName != "")
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckMarryCount(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nCount = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKMARRYCOUNT);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_btMarryCount == nCount)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_btMarryCount > nCount)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_btMarryCount < nCount)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_btMarryCount >= nCount)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckMaster(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if ((PlayObject.m_sMasterName != "") && (!PlayObject.m_boMaster))
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckMemBerLevel(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nLevel = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKMEMBERLEVEL);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_nMemberLevel == nLevel)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_nMemberLevel > nLevel)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_nMemberLevel < nLevel)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_nMemberLevel >= nLevel)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckMemberType(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nType = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nType < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKMEMBERTYPE);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_nMemberType == nType)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_nMemberType > nType)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_nMemberType < nType)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_nMemberType >= nType)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckNameIPList(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            StringList LoadList;
            string sLine;
            string sName = string.Empty;
            string sIPaddr;
            var result = false;
            try
            {
                var sCharName = PlayObject.m_sCharName;
                var sCharAccount = PlayObject.m_sUserID;
                var sCharIPaddr = PlayObject.m_sIPaddr;
                LoadList = new StringList();
                if (File.Exists(M2Share.g_Config.sEnvirDir + QuestConditionInfo.sParam1))
                {
                    LoadList.LoadFromFile(M2Share.g_Config.sEnvirDir + QuestConditionInfo.sParam1);
                    for (var i = 0; i < LoadList.Count; i++)
                    {
                        sLine = LoadList[i];
                        if (sLine[1] == ';')
                        {
                            continue;
                        }
                        sIPaddr = HUtil32.GetValidStr3(sLine, ref sName, new string[] { " ", "/", "\t" });
                        sIPaddr = sIPaddr.Trim();
                        if ((sName == sCharName) && (sIPaddr == sCharIPaddr))
                        {
                            result = true;
                            break;
                        }
                    }
                }
                else
                {
                    ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKNAMEIPLIST);
                }
            }
            finally
            {
                //LoadList.Free;
            }
            return result;
        }

        private bool ConditionOfCheckPoseDir(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.GetPoseCreate() == PlayObject) && (PoseHuman.m_btRaceServer == Grobal2.RC_PLAYOBJECT))
            {
                switch (QuestConditionInfo.nParam1)
                {
                    case 1:// 要求相同性别
                        if (PoseHuman.m_btGender == PlayObject.m_btGender)
                        {
                            result = true;
                        }
                        break;
                    case 2:// 要求不同性别
                        if (PoseHuman.m_btGender != PlayObject.m_btGender)
                        {
                            result = true;
                        }
                        break;
                    default:// 无参数时不判别性别
                        result = true;
                        break;
                }
            }
            return result;
        }

        private bool ConditionOfCheckPoseGender(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            byte btSex = 0;
            if (QuestConditionInfo.sParam1.ToLower().CompareTo("MAN".ToLower()) == 0)
            {
                btSex = 0;
            }
            else if (QuestConditionInfo.sParam1.ToLower().CompareTo("男".ToLower()) == 0)
            {
                btSex = 0;
            }
            else if (QuestConditionInfo.sParam1.ToLower().CompareTo("WOMAN".ToLower()) == 0)
            {
                btSex = 1;
            }
            else if (QuestConditionInfo.sParam1.ToLower().CompareTo("女".ToLower()) == 0)
            {
                btSex = 1;
            }
            var PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.m_btRaceServer == Grobal2.RC_PLAYOBJECT))
            {
                if (PoseHuman.m_btGender == btSex)
                {
                    result = true;
                }
            }
            return result;
        }

        private bool ConditionOfCheckPoseIsMaster(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.m_btRaceServer == Grobal2.RC_PLAYOBJECT))
            {
                if ((((TPlayObject)PoseHuman).m_sMasterName != "") && ((TPlayObject)PoseHuman).m_boMaster)
                {
                    result = true;
                }
            }
            return result;
        }

        private bool ConditionOfCheckPoseLevel(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nLevel = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKPOSELEVEL);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            var PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.m_btRaceServer == Grobal2.RC_PLAYOBJECT))
            {
                switch (cMethod)
                {
                    case '=':
                        if (PoseHuman.m_Abil.Level == nLevel)
                        {
                            result = true;
                        }
                        break;
                    case '>':
                        if (PoseHuman.m_Abil.Level > nLevel)
                        {
                            result = true;
                        }
                        break;
                    case '<':
                        if (PoseHuman.m_Abil.Level < nLevel)
                        {
                            result = true;
                        }
                        break;
                    default:
                        if (PoseHuman.m_Abil.Level >= nLevel)
                        {
                            result = true;
                        }
                        break;
                }
            }
            return result;
        }

        private bool ConditionOfCheckPoseMarry(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            TBaseObject PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.m_btRaceServer == Grobal2.RC_PLAYOBJECT))
            {
                if (((TPlayObject)PoseHuman).m_sDearName != "")
                {
                    result = true;
                }
            }
            return result;
        }

        private bool ConditionOfCheckPoseMaster(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            TBaseObject PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.m_btRaceServer == Grobal2.RC_PLAYOBJECT))
            {
                if ((((TPlayObject)PoseHuman).m_sMasterName != "") && !((TPlayObject)PoseHuman).m_boMaster)
                {
                    result = true;
                }
            }
            return result;
        }

        private bool ConditionOfCheckServerName(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            return QuestConditionInfo.sParam1 == M2Share.g_Config.sServerName;
        }

        private bool ConditionOfCheckSlaveCount(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            int nCount = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKSLAVECOUNT);
                return result;
            }
            char cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_SlaveList.Count == nCount)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_SlaveList.Count > nCount)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_SlaveList.Count < nCount)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_SlaveList.Count >= nCount)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckMap(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result;
            if (QuestConditionInfo.sParam1 == PlayObject.m_sMapName)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        private bool ConditionOfCheckPos(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result;
            int nX = QuestConditionInfo.nParam2;
            int nY = QuestConditionInfo.nParam3;
            if ((QuestConditionInfo.sParam1 == PlayObject.m_sMapName) && (nX == PlayObject.m_nCurrX) && (nY == PlayObject.m_nCurrY))
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        private bool ConditionOfReviveSlave(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            int resultc = -1;
            string s18;
            FileInfo myFile;
            StringList LoadList;
            string SLineText = string.Empty;
            string Petname = string.Empty;
            string lvl = string.Empty;
            string lvlexp = string.Empty;
            string sFileName = Path.Combine(M2Share.g_Config.sEnvirDir, "PetData", PlayObject.m_sCharName + ".txt");
            if (File.Exists(sFileName))
            {
                LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    s18 = LoadList[i].Trim();
                    if ((s18 != "") && (s18[1] != ';'))
                    {
                        s18 = HUtil32.GetValidStr3(s18, ref Petname, "/");
                        s18 = HUtil32.GetValidStr3(s18, ref lvl, "/");
                        s18 = HUtil32.GetValidStr3(s18, ref lvlexp, "/");
                        // PlayObject.ReviveSlave(PetName,str_ToInt(lvl,0),str_ToInt(lvlexp,0),nslavecount,10 * 24 * 60 * 60);
                        resultc = i;
                    }
                }
                if (LoadList.Count > 0)
                {
                    result = true;
                    myFile = new FileInfo(sFileName);
                    StreamWriter _W_0 = myFile.CreateText();
                    _W_0.Close();
                }
            }
            return result;
        }

        private bool ConditionOfCheckMagicLvl(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            TUserMagic UserMagic;
            for (var i = 0; i < PlayObject.m_MagicList.Count; i++)
            {
                UserMagic = PlayObject.m_MagicList[i];
                if (string.Compare(UserMagic.MagicInfo.sMagicName, QuestConditionInfo.sParam1, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (UserMagic.btLevel == QuestConditionInfo.nParam2)
                    {
                        result = true;
                    }
                    break;
                }
            }
            return result;
        }

        private bool ConditionOfCheckGroupClass(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            int nCount = 0;
            int nJob = -1;
            TPlayObject PlayObjectEx;
            if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, M2Share.sWarrior, M2Share.sWarrior.Length))
            {
                nJob = M2Share.jWarr;
            }
            if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, M2Share.sWizard, M2Share.sWizard.Length))
            {
                nJob = M2Share.jWizard;
            }
            if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, M2Share.sTaos, M2Share.sTaos.Length))
            {
                nJob = M2Share.jTaos;
            }
            if (nJob < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHANGEJOB);
                return result;
            }
            if (PlayObject.m_GroupOwner != null)
            {
                for (var i = 0; i < PlayObject.m_GroupMembers.Count; i++)
                {
                    PlayObjectEx = PlayObject.m_GroupMembers[i];
                    if (PlayObjectEx.m_btJob == nJob)
                    {
                        nCount++;
                    }
                }
            }
            char cMethod = QuestConditionInfo.sParam2[0];
            switch (cMethod)
            {
                case '=':
                    if (nCount == QuestConditionInfo.nParam3)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (nCount > QuestConditionInfo.nParam3)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (nCount < QuestConditionInfo.nParam3)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (nCount >= QuestConditionInfo.nParam3)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }


        private bool ConditionOfCheckRangeMonCount(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            TBaseObject BaseObject;
            bool result = false;
            string sMapName = QuestConditionInfo.sParam1;
            int nX = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            int nY = HUtil32.Str_ToInt(QuestConditionInfo.sParam3, -1);
            int nRange = HUtil32.Str_ToInt(QuestConditionInfo.sParam4, -1);
            char cMethod = QuestConditionInfo.sParam5[0];
            int nCount = HUtil32.Str_ToInt(QuestConditionInfo.sParam6, -1);
            TEnvirnoment Envir = M2Share.g_MapManager.FindMap(sMapName);
            if ((Envir == null) || (nX < 0) || (nY < 0) || (nRange < 0) || (nCount < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKRANGEMONCOUNT);
                return result;
            }
            IList<TBaseObject> MonList = new List<TBaseObject>();
            int nMapRangeCount = Envir.GetRangeBaseObject(nX, nY, nRange, true, MonList);
            for (var i = MonList.Count - 1; i >= 0; i--)
            {
                BaseObject = MonList[i];
                if ((BaseObject.m_btRaceServer < Grobal2.RC_ANIMAL) || (BaseObject.m_btRaceServer == Grobal2.RC_ARCHERGUARD) || (BaseObject.m_Master != null))
                {
                    MonList.RemoveAt(i);
                }
            }
            nMapRangeCount = MonList.Count;
            switch (cMethod)
            {
                case '=':
                    if (nMapRangeCount == nCount)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (nMapRangeCount > nCount)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (nMapRangeCount < nCount)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (nMapRangeCount >= nCount)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckReNewLevel(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            int nLevel = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKLEVELEX);
                return result;
            }
            char cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_btReLevel == nLevel)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_btReLevel > nLevel)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_btReLevel < nLevel)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_btReLevel >= nLevel)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckSlaveLevel(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            TBaseObject BaseObject;
            bool result = false;
            int nLevel = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKLEVELEX);
                return result;
            }
            var nSlaveLevel = -1;
            for (var i = 0; i < PlayObject.m_SlaveList.Count; i++)
            {
                BaseObject = PlayObject.m_SlaveList[i];
                if (BaseObject.m_Abil.Level > nSlaveLevel)
                {
                    nSlaveLevel = BaseObject.m_Abil.Level;
                }
            }
            if (nSlaveLevel < 0)
            {
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nSlaveLevel == nLevel)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (nSlaveLevel > nLevel)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (nSlaveLevel < nLevel)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (nSlaveLevel >= nLevel)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckUseItem(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            int nWhere = HUtil32.Str_ToInt(QuestConditionInfo.sParam1, -1);
            if ((nWhere < 0) || (nWhere > PlayObject.m_UseItems.GetUpperBound(0)))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKUSEITEM);
                return result;
            }
            if (PlayObject.m_UseItems[nWhere].wIndex > 0)
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckVar(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            string sName = string.Empty;
            TDynamicVar DynamicVar;
            bool boFoundVar = false;
            var result = false;
            var sType = QuestConditionInfo.sParam1;
            var sVarName = QuestConditionInfo.sParam2;
            var sMethod = QuestConditionInfo.sParam3;
            var nVarValue = HUtil32.Str_ToInt(QuestConditionInfo.sParam4, 0);
            var sVarValue = QuestConditionInfo.sParam4;
            if ((sType == "") || (sVarName == "") || (sMethod == ""))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKVAR);
                return result;
            }
            var cMethod = sMethod[0];
            var DynamicVarList = GetDynamicVarList(PlayObject, sType, ref sName);
            if (DynamicVarList == null)
            {
                // ,format(sVarTypeError,[sType])
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKVAR);
                return result;
            }
            for (var i = 0; i < DynamicVarList.Count; i++)
            {
                DynamicVar = DynamicVarList[i];
                if (DynamicVar.sName.ToLower().CompareTo(sVarName.ToLower()) == 0)
                {
                    switch (DynamicVar.VarType)
                    {
                        case TVarType.Integer:
                            switch (cMethod)
                            {
                                case '=':
                                    if (DynamicVar.nInternet == nVarValue)
                                    {
                                        result = true;
                                    }
                                    break;
                                case '>':
                                    if (DynamicVar.nInternet > nVarValue)
                                    {
                                        result = true;
                                    }
                                    break;
                                case '<':
                                    if (DynamicVar.nInternet < nVarValue)
                                    {
                                        result = true;
                                    }
                                    break;
                                default:
                                    if (DynamicVar.nInternet >= nVarValue)
                                    {
                                        result = true;
                                    }
                                    break;
                            }
                            break;
                        case TVarType.String:
                            break;
                    }
                    boFoundVar = true;
                    break;
                }
            }
            if (!boFoundVar)
            {
                // format(sVarFound,[sVarName,sType]),
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKVAR);
            }
            return result;
        }

        private bool ConditionOfHaveMaster(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            return !string.IsNullOrEmpty(PlayObject.m_sMasterName);
        }

        private bool ConditionOfPoseHaveMaster(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.m_btRaceServer == Grobal2.RC_PLAYOBJECT))
            {
                if (((TPlayObject)PoseHuman).m_sMasterName != "")
                {
                    result = true;
                }
            }
            return result;
        }

        private bool ConditionOfCheckCastleGold(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nGold = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if ((nGold < 0) || (this.m_Castle == null))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKCASTLEGOLD);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (this.m_Castle.m_nTotalGold == nGold)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (this.m_Castle.m_nTotalGold > nGold)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (this.m_Castle.m_nTotalGold < nGold)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (this.m_Castle.m_nTotalGold >= nGold)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckContribution(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nContribution = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nContribution < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKCONTRIBUTION);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_wContribution == nContribution)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_wContribution > nContribution)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_wContribution < nContribution)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_wContribution >= nContribution)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckCreditPoint(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nCreditPoint = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nCreditPoint < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKCREDITPOINT);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_btCreditPoint == nCreditPoint)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_btCreditPoint > nCreditPoint)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_btCreditPoint < nCreditPoint)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_btCreditPoint >= nCreditPoint)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckOfGuild(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if (QuestConditionInfo.sParam1 == "")
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKOFGUILD);
                return result;
            }
            if (PlayObject.m_MyGuild != null)
            {
                if (PlayObject.m_MyGuild.sGuildName.ToLower().CompareTo(QuestConditionInfo.sParam1.ToLower()) == 0)
                {
                    result = true;
                }
            }
            return result;
        }

        private bool ConditionOfCheckOnlineLongMin(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nOnlineMin = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nOnlineMin < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_ONLINELONGMIN);
                return result;
            }
            var nOnlineTime = (HUtil32.GetTickCount() - PlayObject.m_dwLogonTick) / 60000;
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nOnlineTime == nOnlineMin)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (nOnlineTime > nOnlineMin)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (nOnlineTime < nOnlineMin)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (nOnlineTime >= nOnlineMin)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckPasswordErrorCount(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nErrorCount = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nErrorCount < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_PASSWORDERRORCOUNT);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_btPwdFailCount == nErrorCount)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_btPwdFailCount > nErrorCount)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_btPwdFailCount < nErrorCount)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_btPwdFailCount >= nErrorCount)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfIsLockPassword(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            return PlayObject.m_boPasswordLocked;
        }

        private bool ConditionOfIsLockStorage(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            return !PlayObject.m_boCanGetBackItem;
        }

        private bool ConditionOfCheckPayMent(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nPayMent = HUtil32.Str_ToInt(QuestConditionInfo.sParam1, -1);
            if (nPayMent < 1)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKPAYMENT);
                return result;
            }
            if (PlayObject.m_nPayMent == nPayMent)
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckSlaveName(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            TBaseObject BaseObject;
            var result = false;
            var sSlaveName = QuestConditionInfo.sParam1;
            if (sSlaveName == "")
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKSLAVENAME);
                return result;
            }
            for (var i = 0; i < PlayObject.m_SlaveList.Count; i++)
            {
                BaseObject = PlayObject.m_SlaveList[i];
                if (sSlaveName.ToLower().CompareTo(BaseObject.m_sCharName.ToLower()) == 0)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private bool ConditionOfCheckNameDateList(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            StringList LoadList;
            string sListFileName;
            string sLineText;
            string sHumName = string.Empty;
            string sDate = string.Empty;
            DateTime dOldDate = DateTime.Now;
            int nDay;
            var nDayCount = HUtil32.Str_ToInt(QuestConditionInfo.sParam3, -1);
            var nValNo = M2Share.GetValNameNo(QuestConditionInfo.sParam4);
            var nValNoDay = M2Share.GetValNameNo(QuestConditionInfo.sParam5);
            var boDeleteExprie = QuestConditionInfo.sParam6.ToLower().CompareTo("清理".ToLower()) == 0;
            var boNoCompareHumanName = QuestConditionInfo.sParam6.ToLower().CompareTo("1".ToLower()) == 0;
            var cMethod = QuestConditionInfo.sParam2[0];
            if (nDayCount < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKNAMEDATELIST);
                return result;
            }
            sListFileName = M2Share.g_Config.sEnvirDir + m_sPath + QuestConditionInfo.sParam1;
            if (File.Exists(sListFileName))
            {
                LoadList = new StringList();
                try
                {

                    LoadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    M2Share.MainOutMessage("loading fail.... => " + sListFileName);
                }
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i].Trim();
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sHumName, new string[] { " ", "\t" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sDate, new string[] { " ", "\t" });
                    if ((sHumName.ToLower().CompareTo(PlayObject.m_sCharName.ToLower()) == 0) || boNoCompareHumanName)
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
                                    result = true;
                                }
                                break;
                            case '>':
                                if (nDay > nDayCount)
                                {
                                    result = true;
                                }
                                break;
                            case '<':
                                if (nDay < nDayCount)
                                {
                                    result = true;
                                }
                                break;
                            default:
                                if (nDay >= nDayCount)
                                {
                                    result = true;
                                }
                                break;
                        }
                        if (nValNo >= 0)
                        {
                            switch (nValNo)
                            {
                                // Modify the A .. B: 0 .. 9
                                case 0:
                                    PlayObject.m_nVal[nValNo] = nDay;
                                    break;
                                // Modify the A .. B: 100 .. 119
                                case 100:
                                    M2Share.g_Config.GlobalVal[nValNo - 100] = nDay;
                                    break;
                                // Modify the A .. B: 200 .. 209
                                case 200:
                                    PlayObject.m_DyVal[nValNo - 200] = nDay;
                                    break;
                                // Modify the A .. B: 300 .. 399
                                case 300:
                                    PlayObject.m_nMval[nValNo - 300] = nDay;
                                    break;
                                // Modify the A .. B: 400 .. 499
                                case 400:
                                    M2Share.g_Config.GlobaDyMval[nValNo - 400] = (short)nDay;
                                    break;
                            }
                        }
                        if (nValNoDay >= 0)
                        {
                            switch (nValNoDay)
                            {
                                // Modify the A .. B: 0 .. 9
                                case 0:
                                    PlayObject.m_nVal[nValNoDay] = nDayCount - nDay;
                                    break;
                                // Modify the A .. B: 100 .. 119
                                case 100:
                                    M2Share.g_Config.GlobalVal[nValNoDay - 100] = nDayCount - nDay;
                                    break;
                                // Modify the A .. B: 200 .. 209
                                case 200:
                                    PlayObject.m_DyVal[nValNoDay - 200] = nDayCount - nDay;
                                    break;
                                // Modify the A .. B: 300 .. 399
                                case 300:
                                    PlayObject.m_nMval[nValNoDay - 300] = nDayCount - nDay;
                                    break;
                            }
                        }
                        if (!result)
                        {
                            if (boDeleteExprie)
                            {
                                LoadList.RemoveAt(i);
                                try
                                {
                                    LoadList.SaveToFile(sListFileName);
                                }
                                catch
                                {
                                    M2Share.MainOutMessage("Save fail.... => " + sListFileName);
                                }
                            }
                        }
                        break;
                    }
                }

                //LoadList.Free;
            }
            else
            {
                M2Share.MainOutMessage("file not found => " + sListFileName);
            }
            return result;
        }

        // CHECKMAPHUMANCOUNT MAP = COUNT
        private bool ConditionOfCheckMapHumanCount(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nCount = HUtil32.Str_ToInt(QuestConditionInfo.sParam3, -1);
            if (nCount < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKMAPHUMANCOUNT);
                return result;
            }
            var nHumanCount = M2Share.UserEngine.GetMapHuman(QuestConditionInfo.sParam1);
            var cMethod = QuestConditionInfo.sParam2[0];
            switch (cMethod)
            {
                case '=':
                    if (nHumanCount == nCount)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (nHumanCount > nCount)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (nHumanCount < nCount)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (nHumanCount >= nCount)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckMapMonCount(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nCount = HUtil32.Str_ToInt(QuestConditionInfo.sParam3, -1);
            var Envir = M2Share.g_MapManager.FindMap(QuestConditionInfo.sParam1);
            if ((nCount < 0) || (Envir == null))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKMAPMONCOUNT);
                return result;
            }
            var nMonCount = M2Share.UserEngine.GetMapMonster(Envir, null);
            var cMethod = QuestConditionInfo.sParam2[0];
            switch (cMethod)
            {
                case '=':
                    if (nMonCount == nCount)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (nMonCount > nCount)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (nMonCount < nCount)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (nMonCount >= nCount)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckIsOnMap(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            if (PlayObject.m_sMapFileName == QuestConditionInfo.sParam1 || PlayObject.m_sMapName == QuestConditionInfo.sParam1)
            {
                return true;
            }
            return false;
        }

        private bool EqualData(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            int n14 = 0;
            int n18 = 0;
            var s01 = string.Empty;
            var s02 = string.Empty;
            if (CheckVarNameNo(PlayObject, QuestConditionInfo, ref n14, ref n18))// 比较数值
            {
                if (n14 != n18)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else
            {
                // 比较字符串
                if (!GetValValue(PlayObject, QuestConditionInfo.sParam1, ref s01))
                {
                    s01 = GetLineVariableText(PlayObject, QuestConditionInfo.sParam1);//  支持变量
                }
                if (!GetValValue(PlayObject, QuestConditionInfo.sParam2, ref s02))
                {
                    s02 = GetLineVariableText(PlayObject, QuestConditionInfo.sParam2);//  支持变量
                }
                if ((s01).ToLower().CompareTo((s02).ToLower()) == 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        public bool CheckVarNameNo(TPlayObject PlayObject, TQuestConditionInfo CheckQuestConditionInfo, ref int n140, ref int n180)
        {
            bool result = false;
            string sParam1 = string.Empty;
            string sParam2 = string.Empty;
            string sParam3 = string.Empty;
            n140 = -1;
            n180 = -1;
            if (HUtil32.CompareLStr(CheckQuestConditionInfo.sParam1, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(CheckQuestConditionInfo.sParam1, "(", ")", ref sParam1);
            }
            else
            {
                sParam1 = CheckQuestConditionInfo.sParam1;
            }
            if (HUtil32.CompareLStr(CheckQuestConditionInfo.sParam2, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(CheckQuestConditionInfo.sParam2, "(", ")", ref sParam2);
            }
            else
            {
                sParam2 = CheckQuestConditionInfo.sParam2;
            }
            if (HUtil32.CompareLStr(CheckQuestConditionInfo.sParam3, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(CheckQuestConditionInfo.sParam3, "(", ")", ref sParam3);
            }
            else
            {
                sParam3 = CheckQuestConditionInfo.sParam3;
            }
            switch (GotoLable_CheckVarNameNo_GetDataType(CheckQuestConditionInfo))
            {
                case 0:
                    if (GotoLable_CheckVarNameNo_GetDynamicVarValue(PlayObject, sParam1, sParam2, ref n140) && GotoLable_CheckVarNameNo_GetValValue(PlayObject, sParam3, ref n180))
                    {
                        result = true;
                    }
                    break;
                case 1:
                    n180 = CheckQuestConditionInfo.nParam3;
                    if (GotoLable_CheckVarNameNo_GetDynamicVarValue(PlayObject, sParam1, sParam2, ref n140))
                    {
                        result = true;
                    }
                    break;
                case 2:
                    if (GotoLable_CheckVarNameNo_GetValValue(PlayObject, sParam1, ref n140) && GotoLable_CheckVarNameNo_GetValValue(PlayObject, sParam2, ref n180))
                    {
                        result = true;
                    }
                    break;
                case 3:
                    if (GotoLable_CheckVarNameNo_GetValValue(PlayObject, sParam1, ref n140) && GotoLable_CheckVarNameNo_GetDynamicVarValue(PlayObject, sParam2, sParam3, ref n180))
                    {
                        result = true;
                    }
                    break;
                case 4:
                    n180 = CheckQuestConditionInfo.nParam2;
                    if (GotoLable_CheckVarNameNo_GetValValue(PlayObject, sParam1, ref n140))
                    {
                        result = true;
                    }
                    break;
                case 5:
                    break;
            }
            return result;
        }

        public int GotoLable_CheckVarNameNo_GetDataType(TQuestConditionInfo CheckQuestConditionInfo)
        {
            int result;
            string sParam1 = string.Empty;
            string sParam2 = string.Empty;
            string sParam3 = string.Empty;
            result = -1;
            if (HUtil32.CompareLStr(CheckQuestConditionInfo.sParam1, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(CheckQuestConditionInfo.sParam1, "(", ")", ref sParam1);
            }
            else
            {
                sParam1 = CheckQuestConditionInfo.sParam1;
            }
            if (HUtil32.CompareLStr(CheckQuestConditionInfo.sParam2, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(CheckQuestConditionInfo.sParam2, "(", ")", ref sParam2);
            }
            else
            {
                sParam2 = CheckQuestConditionInfo.sParam2;
            }
            if (HUtil32.CompareLStr(CheckQuestConditionInfo.sParam3, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(CheckQuestConditionInfo.sParam3, "(", ")", ref sParam3);
            }
            else
            {
                sParam3 = CheckQuestConditionInfo.sParam3;
            }
            if (HUtil32.IsVarNumber(sParam1))
            {
                if ((sParam3 != "") && (M2Share.GetValNameNo(sParam3) >= 0))
                {
                    result = 0;
                }
                else if ((sParam3 != "") && HUtil32.IsStringNumber(sParam3))
                {
                    result = 1;
                }
                return result;
            }
            if (M2Share.GetValNameNo(sParam1) >= 0)
            {
                if ((sParam2 != "") && (M2Share.GetValNameNo(sParam2) >= 0))
                {
                    result = 2;
                }
                else if ((sParam2 != "") && HUtil32.IsVarNumber(sParam2) && (sParam3 != ""))
                {
                    result = 3;
                }
                else if ((sParam2 != "") && HUtil32.IsStringNumber(sParam2))
                {
                    result = 4;
                }
            }
            return result;
        }

        public bool GotoLable_CheckVarNameNo_GetDynamicVarValue(TPlayObject PlayObject, string sVarType, string sValName, ref int nValue)
        {
            bool result = false;
            TDynamicVar DynamicVar;
            string sName = string.Empty;
            IList<TDynamicVar> DynamicVarList = GetDynamicVarList(PlayObject, sVarType, ref sName);
            if (DynamicVarList == null)
            {
                return result;
            }
            else
            {
                if (DynamicVarList.Count > 0)
                {
                    for (var i = 0; i < DynamicVarList.Count; i++)
                    {
                        DynamicVar = DynamicVarList[i];
                        if (DynamicVar != null)
                        {
                            if ((DynamicVar.sName).ToLower().CompareTo((sValName).ToLower()) == 0)
                            {
                                switch (DynamicVar.VarType)
                                {
                                    case TVarType.Integer:
                                        nValue = DynamicVar.nInternet;
                                        result = true;
                                        break;
                                    case TVarType.String:
                                        break;
                                }
                                break;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public bool GotoLable_CheckVarNameNo_GetValValue(TPlayObject PlayObject, string sValName, ref int nValue)
        {
            nValue = 0;
            bool result = false;
            int n100 = M2Share.GetValNameNo(sValName);
            if (n100 >= 0)
            {
                if (HUtil32.RangeInDefined(n100, 0, 9))
                {
                    nValue = PlayObject.m_nVal[n100];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 100, 199))
                {
                    nValue = M2Share.g_Config.GlobalVal[n100 - 100];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 200, 299))
                {
                    nValue = PlayObject.m_DyVal[n100 - 200];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 300, 399))
                {
                    nValue = PlayObject.m_nMval[n100 - 300];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 400, 499))
                {
                    nValue = M2Share.g_Config.GlobaDyMval[n100 - 400];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 500, 599))
                {
                    nValue = PlayObject.m_nInteger[n100 - 500];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 600, 699))
                {
                    if (HUtil32.IsStringNumber(PlayObject.m_sString[n100 - 600]))
                    {
                        nValue = HUtil32.Str_ToInt(PlayObject.m_sString[n100 - 600], 0);
                        result = true;
                    }
                }
                else if (HUtil32.RangeInDefined(n100, 700, 799))
                {
                    if (HUtil32.IsStringNumber(M2Share.g_Config.GlobalAVal[n100 - 700]))
                    {
                        nValue = HUtil32.Str_ToInt(M2Share.g_Config.GlobalAVal[n100 - 700], 0);
                        result = true;
                    }
                }
                else if (HUtil32.RangeInDefined(n100, 800, 1199))//G变量
                {
                    nValue = M2Share.g_Config.GlobalVal[n100 - 700];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 1200, 1599))//G变量
                {
                    if (HUtil32.IsStringNumber(M2Share.g_Config.GlobalAVal[n100 - 1100]))
                    {
                        nValue = HUtil32.Str_ToInt(M2Share.g_Config.GlobalAVal[n100 - 1100], 0);
                        result = true;
                    }
                }
                else if (HUtil32.RangeInDefined(n100, 1600, 1699))//G变量
                {
                    if (HUtil32.IsStringNumber(PlayObject.m_ServerStrVal[n100 - 1600]))
                    {
                        nValue = HUtil32.Str_ToInt(PlayObject.m_ServerStrVal[n100 - 1600], 0);
                        result = true;
                    }
                }
                else if (HUtil32.RangeInDefined(n100, 1700, 1799))//G变量
                {
                    nValue = PlayObject.m_ServerIntVal[n100 - 1700];
                    result = true;
                }
            }
            return result;
        }
    }
}
