using GameSvr.Actor;
using GameSvr.Maps;
using GameSvr.Monster.Monsters;
using GameSvr.Player;
using GameSvr.Script;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.Npc
{
    public partial class NormNpc
    {
        private bool ConditionOfCheckAccountIPList(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            string sLine;
            string sName = string.Empty;
            string sIPaddr;
            var sChrName = PlayObject.ChrName;
            var sCharAccount = PlayObject.UserAccount;
            var sCharIPaddr = PlayObject.LoginIpAddr;
            var LoadList = new StringList();
            if (File.Exists(M2Share.Config.EnvirDir + QuestConditionInfo.sParam1))
            {
                LoadList.LoadFromFile(M2Share.Config.EnvirDir + QuestConditionInfo.sParam1);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sLine = LoadList[i];
                    if (sLine[0] == ';')
                    {
                        continue;
                    }
                    sIPaddr = HUtil32.GetValidStr3(sLine, ref sName, new[] { ' ', '/', '\t' });
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
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKACCOUNTIPLIST);
            }
            return result;
        }

        private bool ConditionOfCheckBagSize(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nSize = QuestConditionInfo.nParam1;
            if ((nSize <= 0) || (nSize > Grobal2.MAXBAGITEM))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKBAGSIZE);
                return result;
            }
            if (PlayObject.ItemList.Count + nSize <= Grobal2.MAXBAGITEM)
            {
                result = true;
            }
            return result;
        }

        private static bool ConditionOfCheckBonusPoint(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var BonusAbil = PlayObject.BonusAbil;
            var nTotlePoint = BonusAbil.DC + BonusAbil.MC + BonusAbil.SC + BonusAbil.AC + BonusAbil.MAC + BonusAbil.HP + BonusAbil.MP + BonusAbil.Hit + BonusAbil.Speed + BonusAbil.Reserved;
            nTotlePoint += PlayObject.BonusPoint;
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

        private static bool ConditionOfCheckHPCheckHigh(PlayObject PlayObject, char cMethodMax, int nMax)
        {
            var result = false;
            switch (cMethodMax)
            {
                case '=':
                    if (PlayObject.WAbil.MaxHP == nMax)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.WAbil.MaxHP > nMax)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.WAbil.MaxHP < nMax)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.WAbil.MaxHP >= nMax)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckHP(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var cMethodMin = QuestConditionInfo.sParam1[0];
            var cMethodMax = QuestConditionInfo.sParam1[2];
            var nMin = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            var nMax = HUtil32.StrToInt(QuestConditionInfo.sParam4, -1);
            if ((nMin < 0) || (nMax < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKHP);
                return result;
            }
            switch (cMethodMin)
            {
                case '=':
                    if (WAbil.HP == nMin)
                    {
                        result = ConditionOfCheckHPCheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '>':
                    if (PlayObject.WAbil.HP > nMin)
                    {
                        result = ConditionOfCheckHPCheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '<':
                    if (PlayObject.WAbil.HP < nMin)
                    {
                        result = ConditionOfCheckHPCheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                default:
                    if (PlayObject.WAbil.HP >= nMin)
                    {
                        result = ConditionOfCheckHPCheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
            }
            return result;
        }

        private static bool ConditionOfCheckMP_CheckHigh(PlayObject PlayObject, char cMethodMax, int nMax)
        {
            var result = false;
            switch (cMethodMax)
            {
                case '=':
                    if (PlayObject.WAbil.MaxMP == nMax)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.WAbil.MaxMP > nMax)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.WAbil.MaxMP < nMax)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.WAbil.MaxMP >= nMax)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckMP(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var cMethodMin = QuestConditionInfo.sParam1[0];
            var cMethodMax = QuestConditionInfo.sParam1[2];
            var nMin = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            var nMax = HUtil32.StrToInt(QuestConditionInfo.sParam4, -1);
            if ((nMin < 0) || (nMax < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKMP);
                return result;
            }
            switch (cMethodMin)
            {
                case '=':
                    if (WAbil.MP == nMin)
                    {
                        result = ConditionOfCheckMP_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '>':
                    if (PlayObject.WAbil.MP > nMin)
                    {
                        result = ConditionOfCheckMP_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '<':
                    if (PlayObject.WAbil.MP < nMin)
                    {
                        result = ConditionOfCheckMP_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                default:
                    if (PlayObject.WAbil.MP >= nMin)
                    {
                        result = ConditionOfCheckMP_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
            }
            return result;
        }

        private static bool ConditionOfCheckDC_CheckHigh(PlayObject PlayObject, char cMethodMax, int nMax)
        {
            var result = false;
            switch (cMethodMax)
            {
                case '=':
                    if (HUtil32.HiWord(PlayObject.WAbil.DC) == nMax)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (HUtil32.HiWord(PlayObject.WAbil.DC) > nMax)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (HUtil32.HiWord(PlayObject.WAbil.DC) < nMax)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (HUtil32.HiWord(PlayObject.WAbil.DC) >= nMax)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckDC(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var cMethodMin = QuestConditionInfo.sParam1[0];
            var cMethodMax = QuestConditionInfo.sParam1[2];
            var nMin = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            var nMax = HUtil32.StrToInt(QuestConditionInfo.sParam4, -1);
            if ((nMin < 0) || (nMax < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKDC);
                return result;
            }
            switch (cMethodMin)
            {
                case '=':
                    if (HUtil32.LoWord(PlayObject.WAbil.DC) == nMin)
                    {
                        result = ConditionOfCheckDC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '>':
                    if (HUtil32.LoWord(PlayObject.WAbil.DC) > nMin)
                    {
                        result = ConditionOfCheckDC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '<':
                    if (HUtil32.LoWord(PlayObject.WAbil.DC) < nMin)
                    {
                        result = ConditionOfCheckDC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                default:
                    if (HUtil32.LoWord(PlayObject.WAbil.DC) >= nMin)
                    {
                        result = ConditionOfCheckDC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
            }
            return result;
        }

        private static bool ConditionOfCheckMC_CheckHigh(PlayObject PlayObject, char cMethodMax, int nMax)
        {
            var result = false;
            switch (cMethodMax)
            {
                case '=':
                    if (HUtil32.HiWord(PlayObject.WAbil.MC) == nMax)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (HUtil32.HiWord(PlayObject.WAbil.MC) > nMax)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (HUtil32.HiWord(PlayObject.WAbil.MC) < nMax)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (HUtil32.HiWord(PlayObject.WAbil.MC) >= nMax)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckMC(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var cMethodMin = QuestConditionInfo.sParam1[0];
            var cMethodMax = QuestConditionInfo.sParam1[2];
            var nMin = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            var nMax = HUtil32.StrToInt(QuestConditionInfo.sParam4, -1);
            if ((nMin < 0) || (nMax < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKMC);
                return result;
            }
            switch (cMethodMin)
            {
                case '=':
                    if (HUtil32.LoWord(PlayObject.WAbil.MC) == nMin)
                    {
                        result = ConditionOfCheckMC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '>':
                    if (HUtil32.LoWord(PlayObject.WAbil.MC) > nMin)
                    {
                        result = ConditionOfCheckMC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '<':
                    if (HUtil32.LoWord(PlayObject.WAbil.MC) < nMin)
                    {
                        result = ConditionOfCheckMC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                default:
                    if (HUtil32.LoWord(PlayObject.WAbil.MC) >= nMin)
                    {
                        result = ConditionOfCheckMC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
            }
            return result;
        }

        private static bool ConditionOfCheckSC_CheckHigh(PlayObject PlayObject, char cMethodMax, int nMax)
        {
            var result = false;
            switch (cMethodMax)
            {
                case '=':
                    if (HUtil32.HiWord(PlayObject.WAbil.SC) == nMax)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (HUtil32.HiWord(PlayObject.WAbil.SC) > nMax)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (HUtil32.HiWord(PlayObject.WAbil.SC) < nMax)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (HUtil32.HiWord(PlayObject.WAbil.SC) >= nMax)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckSC(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var cMethodMin = QuestConditionInfo.sParam1[0];
            var cMethodMax = QuestConditionInfo.sParam1[2];
            var nMin = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            var nMax = HUtil32.StrToInt(QuestConditionInfo.sParam4, -1);
            if ((nMin < 0) || (nMax < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKSC);
                return result;
            }
            switch (cMethodMin)
            {
                case '=':
                    if (HUtil32.LoWord(PlayObject.WAbil.SC) == nMin)
                    {
                        result = ConditionOfCheckSC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '>':
                    if (HUtil32.LoWord(PlayObject.WAbil.SC) > nMin)
                    {
                        result = ConditionOfCheckSC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '<':
                    if (HUtil32.LoWord(PlayObject.WAbil.SC) < nMin)
                    {
                        result = ConditionOfCheckSC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                default:
                    if (HUtil32.LoWord(PlayObject.WAbil.SC) >= nMin)
                    {
                        result = ConditionOfCheckSC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckExp(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var dwExp = HUtil32.StrToInt(QuestConditionInfo.sParam2, 0);
            if (dwExp == 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKEXP);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.Abil.Exp == dwExp)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.Abil.Exp > dwExp)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.Abil.Exp < dwExp)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.Abil.Exp >= dwExp)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckFlourishPoint(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nPoint = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nPoint < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKFLOURISHPOINT);
                return false;
            }
            if (PlayObject.MyGuild == null)
            {
                return false;
            }
            var Guild = PlayObject.MyGuild;
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (Guild.Flourishing == nPoint)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (Guild.Flourishing > nPoint)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (Guild.Flourishing < nPoint)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (Guild.Flourishing >= nPoint)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckChiefItemCount(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nCount = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKFLOURISHPOINT);
                return false;
            }
            if (PlayObject.MyGuild == null)
            {
                return false;
            }
            var Guild = PlayObject.MyGuild;
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (Guild.ChiefItemCount == nCount)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (Guild.ChiefItemCount > nCount)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (Guild.ChiefItemCount < nCount)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (Guild.ChiefItemCount >= nCount)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckGuildAuraePoint(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nPoint = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nPoint < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKAURAEPOINT);
                return false;
            }
            if (PlayObject.MyGuild == null)
            {
                return false;
            }
            var Guild = PlayObject.MyGuild;
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (Guild.Aurae == nPoint)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (Guild.Aurae > nPoint)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (Guild.Aurae < nPoint)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (Guild.Aurae >= nPoint)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckGuildBuildPoint(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nPoint = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nPoint < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKBUILDPOINT);
                return false;
            }
            if (PlayObject.MyGuild == null)
            {
                return false;
            }
            var Guild = PlayObject.MyGuild;
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

        private bool ConditionOfCheckStabilityPoint(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nPoint = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nPoint < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKSTABILITYPOINT);
                return result;
            }
            if (PlayObject.MyGuild == null)
            {
                return result;
            }
            var Guild = PlayObject.MyGuild;
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (Guild.Stability == nPoint)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (Guild.Stability > nPoint)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (Guild.Stability < nPoint)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (Guild.Stability >= nPoint)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckGameGold(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nGameGold = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nGameGold < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKGAMEGOLD);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.GameGold == nGameGold)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.GameGold > nGameGold)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.GameGold < nGameGold)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.GameGold >= nGameGold)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckGamePoint(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nGamePoint = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nGamePoint < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKGAMEPOINT);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.GamePoint == nGamePoint)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.GamePoint > nGamePoint)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.GamePoint < nGamePoint)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.GamePoint >= nGamePoint)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckGroupCount(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if (PlayObject.GroupOwner == null)
            {
                return false;
            }
            var nCount = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKGROUPCOUNT);
                return false;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.GroupOwner.GroupMembers.Count == nCount)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.GroupOwner.GroupMembers.Count > nCount)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.GroupOwner.GroupMembers.Count < nCount)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.GroupOwner.GroupMembers.Count >= nCount)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfIsHigh(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if (QuestConditionInfo.sParam1 == "")
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_ISHIGH);
                return false;
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
                    ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_ISHIGH);
                    break;
            }
            return result;
        }

        private static bool ConditionOfCheckHaveGuild(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            return PlayObject.MyGuild != null;
        }

        private bool ConditionOfCheckInMapRange(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var sMapName = QuestConditionInfo.sParam1;
            var nX = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            var nY = HUtil32.StrToInt(QuestConditionInfo.sParam3, -1);
            var nRange = HUtil32.StrToInt(QuestConditionInfo.sParam4, -1);
            if ((sMapName == "") || (nX < 0) || (nY < 0) || (nRange < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKINMAPRANGE);
                return false;
            }
            if (string.Compare(PlayObject.MapName, sMapName, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return false;
            }
            if ((Math.Abs(PlayObject.CurrX - nX) <= nRange) && (Math.Abs(PlayObject.CurrY - nY) <= nRange))
            {
                return true;
            }
            return false;
        }

        private bool ConditionOfCheckIsAttackGuild(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            if (Castle == null)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_ISATTACKGUILD);
                return false;
            }
            if (PlayObject.MyGuild == null)
            {
                return false;
            }
            return Castle.IsAttackGuild(PlayObject.MyGuild);
        }

        private bool ConditionOfCheckCastleChageDay(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nDay = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if ((nDay < 0) || (Castle == null))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CASTLECHANGEDAY);
                return result;
            }
            var nChangeDay = HUtil32.GetDayCount(DateTime.Now, Castle.ChangeDate);
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

        private bool ConditionOfCheckCastleWarDay(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nDay = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if ((nDay < 0) || (Castle == null))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CASTLEWARDAY);
                return false;
            }
            var nWarDay = HUtil32.GetDayCount(DateTime.Now, Castle.m_WarDate);
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

        private bool ConditionOfCheckCastleDoorStatus(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nDay = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            var nDoorStatus = -1;
            if (string.Compare(QuestConditionInfo.sParam1, "损坏", StringComparison.OrdinalIgnoreCase) == 0)
            {
                nDoorStatus = 0;
            }
            if (string.Compare(QuestConditionInfo.sParam1, "开启", StringComparison.OrdinalIgnoreCase) == 0)
            {
                nDoorStatus = 1;
            }
            if (string.Compare(QuestConditionInfo.sParam1, "关闭", StringComparison.OrdinalIgnoreCase) == 0)
            {
                nDoorStatus = 2;
            }
            if ((nDay < 0) || (Castle == null) || (nDoorStatus < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKCASTLEDOOR);
                return result;
            }
            var CastleDoor = (CastleDoor)Castle.MainDoor.BaseObject;
            switch (nDoorStatus)
            {
                case 0:
                    if (CastleDoor.Death)
                    {
                        result = true;
                    }
                    break;
                case 1:
                    if (CastleDoor.IsOpened)
                    {
                        result = true;
                    }
                    break;
                case 2:
                    if (!CastleDoor.IsOpened)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckIsAttackAllyGuild(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            if (Castle == null)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_ISATTACKALLYGUILD);
                return false;
            }
            if (PlayObject.MyGuild == null)
            {
                return false;
            }
            return Castle.IsAttackAllyGuild(PlayObject.MyGuild);
        }

        private bool ConditionOfCheckIsDefenseAllyGuild(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            if (Castle == null)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_ISDEFENSEALLYGUILD);
                return false;
            }
            if (PlayObject.MyGuild == null)
            {
                return false;
            }
            return Castle.IsDefenseAllyGuild(PlayObject.MyGuild);
        }

        private bool ConditionOfCheckIsDefenseGuild(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            if (Castle == null)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_ISDEFENSEGUILD);
                return false;
            }
            if (PlayObject.MyGuild == null)
            {
                return false;
            }
            return Castle.IsDefenseGuild(PlayObject.MyGuild);
        }

        private static bool ConditionOfCheckIsCastleaGuild(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            return M2Share.CastleMgr.IsCastleMember(PlayObject) != null;
        }

        private static bool ConditionOfCheckIsCastleMaster(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            return PlayObject.IsGuildMaster() && (M2Share.CastleMgr.IsCastleMember(PlayObject) != null);
        }

        private static bool ConditionOfCheckIsGuildMaster(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            return PlayObject.IsGuildMaster();
        }

        private static bool ConditionOfCheckIsMaster(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            return !string.IsNullOrEmpty(PlayObject.MasterName) && PlayObject.MBoMaster;
        }

        private static bool ConditionOfCheckListCount(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            return false;
        }

        private bool ConditionOfCheckItemAddValue(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nWhere = HUtil32.StrToInt(QuestConditionInfo.sParam1, -1);
            var cMethod = QuestConditionInfo.sParam2[0];
            var nAddValue = HUtil32.StrToInt(QuestConditionInfo.sParam3, -1);
            if (!(nWhere >= 0 && nWhere <= PlayObject.UseItems.Length) || (nAddValue < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKITEMADDVALUE);
                return result;
            }
            var UserItem = PlayObject.UseItems[nWhere];
            if (UserItem.Index == 0)
            {
                return result;
            }
            var nAddAllValue = 0;
            for (var i = 0; i < UserItem.Desc.Length; i++)
            {
                nAddAllValue += UserItem.Desc[i];
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

        private bool ConditionOfCheckItemType(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nWhere = HUtil32.StrToInt(QuestConditionInfo.sParam1, -1);
            var nType = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (!(nWhere >= 0 && nWhere <= PlayObject.UseItems.Length))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKITEMTYPE);
                return result;
            }
            var UserItem = PlayObject.UseItems[nWhere];
            if (UserItem == null && UserItem.Index == 0)
            {
                return result;
            }
            var Stditem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
            if ((Stditem != null) && (Stditem.StdMode == nType))
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckLevelEx(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nLevel = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKLEVELEX);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.Abil.Level == nLevel)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.Abil.Level > nLevel)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.Abil.Level < nLevel)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.Abil.Level >= nLevel)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckNameListPostion(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            string sLine;
            var result = false;
            var nNamePostion = -1;
            var sChrName = PlayObject.ChrName;
            if (File.Exists(M2Share.Config.EnvirDir + QuestConditionInfo.sParam1))
            {
                var LoadList = new StringList();
                LoadList.LoadFromFile(M2Share.Config.EnvirDir + QuestConditionInfo.sParam1);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sLine = LoadList[i].Trim();
                    if (sLine[0] == ';')
                    {
                        continue;
                    }
                    if (string.Compare(sLine, sChrName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        nNamePostion = i;
                        break;
                    }
                }
            }
            else
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKNAMELISTPOSITION);
            }
            var nPostion = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nPostion < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKNAMELISTPOSITION);
                return result;
            }
            if (nNamePostion >= nPostion)
            {
                result = true;
            }
            return result;
        }

        private static bool ConditionOfCheckMarry(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            return !string.IsNullOrEmpty(PlayObject.MSDearName);
        }

        private bool ConditionOfCheckMarryCount(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nCount = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKMARRYCOUNT);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.MBtMarryCount == nCount)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.MBtMarryCount > nCount)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.MBtMarryCount < nCount)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.MBtMarryCount >= nCount)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private static bool ConditionOfCheckMaster(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            return !string.IsNullOrEmpty(PlayObject.MasterName) && !PlayObject.MBoMaster;
        }

        private bool ConditionOfCheckMemBerLevel(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nLevel = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKMEMBERLEVEL);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.MemberLevel == nLevel)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.MemberLevel > nLevel)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.MemberLevel < nLevel)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.MemberLevel >= nLevel)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckMemberType(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nType = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nType < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKMEMBERTYPE);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.MemberType == nType)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.MemberType > nType)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.MemberType < nType)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.MemberType >= nType)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckNameIPList(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            StringList LoadList;
            string sLine;
            string sName = string.Empty;
            string sIPaddr;
            var result = false;
            var sChrName = PlayObject.ChrName;
            var sCharAccount = PlayObject.UserAccount;
            var sCharIPaddr = PlayObject.LoginIpAddr;
            LoadList = new StringList();
            if (File.Exists(M2Share.Config.EnvirDir + QuestConditionInfo.sParam1))
            {
                LoadList.LoadFromFile(M2Share.Config.EnvirDir + QuestConditionInfo.sParam1);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sLine = LoadList[i];
                    if (sLine[0] == ';')
                    {
                        continue;
                    }
                    sIPaddr = HUtil32.GetValidStr3(sLine, ref sName, new[] { ' ', '/', '\t' });
                    sIPaddr = sIPaddr.Trim();
                    if ((sName == sChrName) && (sIPaddr == sCharIPaddr))
                    {
                        result = true;
                        break;
                    }
                }
            }
            else
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKNAMEIPLIST);
            }
            return result;
        }

        private static bool ConditionOfCheckPoseDir(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.GetPoseCreate() == PlayObject) && (PoseHuman.Race == ActorRace.Play))
            {
                switch (QuestConditionInfo.nParam1)
                {
                    case 1:// 要求相同性别
                        if (((PlayObject)PoseHuman).Gender == PlayObject.Gender)
                        {
                            result = true;
                        }
                        break;
                    case 2:// 要求不同性别
                        if (((PlayObject)PoseHuman).Gender != PlayObject.Gender)
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

        private static bool ConditionOfCheckPoseGender(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            byte btSex = 0;
            if (string.Compare(QuestConditionInfo.sParam1, "MAN", StringComparison.OrdinalIgnoreCase) == 0)
            {
                btSex = 0;
            }
            else if (string.Compare(QuestConditionInfo.sParam1, "男", StringComparison.OrdinalIgnoreCase) == 0)
            {
                btSex = 0;
            }
            else if (string.Compare(QuestConditionInfo.sParam1, "WOMAN", StringComparison.OrdinalIgnoreCase) == 0)
            {
                btSex = 1;
            }
            else if (string.Compare(QuestConditionInfo.sParam1, "女", StringComparison.OrdinalIgnoreCase) == 0)
            {
                btSex = 1;
            }
            var PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.Race == ActorRace.Play))
            {
                if (((PlayObject)PoseHuman).Gender == Enum.Parse<PlayGender>(btSex.ToString()))
                {
                    result = true;
                }
            }
            return result;
        }

        private static bool ConditionOfCheckPoseIsMaster(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.Race == ActorRace.Play))
            {
                if ((((PlayObject)PoseHuman).MasterName != "") && ((PlayObject)PoseHuman).MBoMaster)
                {
                    result = true;
                }
            }
            return result;
        }

        private bool ConditionOfCheckPoseLevel(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nLevel = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKPOSELEVEL);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            var PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.Race == ActorRace.Play))
            {
                switch (cMethod)
                {
                    case '=':
                        if (PoseHuman.Abil.Level == nLevel)
                        {
                            result = true;
                        }
                        break;
                    case '>':
                        if (PoseHuman.Abil.Level > nLevel)
                        {
                            result = true;
                        }
                        break;
                    case '<':
                        if (PoseHuman.Abil.Level < nLevel)
                        {
                            result = true;
                        }
                        break;
                    default:
                        if (PoseHuman.Abil.Level >= nLevel)
                        {
                            result = true;
                        }
                        break;
                }
            }
            return result;
        }

        private static bool ConditionOfCheckPoseMarry(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            BaseObject PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.Race == ActorRace.Play))
            {
                if (((PlayObject)PoseHuman).MSDearName != "")
                {
                    result = true;
                }
            }
            return result;
        }

        private static bool ConditionOfCheckPoseMaster(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            BaseObject PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.Race == ActorRace.Play))
            {
                if ((((PlayObject)PoseHuman).MasterName != "") && !((PlayObject)PoseHuman).MBoMaster)
                {
                    result = true;
                }
            }
            return result;
        }

        private static bool ConditionOfCheckServerName(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            return QuestConditionInfo.sParam1 == M2Share.Config.ServerName;
        }

        private bool ConditionOfCheckSlaveCount(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            int nCount = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKSLAVECOUNT);
                return result;
            }
            char cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.SlaveList.Count == nCount)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.SlaveList.Count > nCount)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.SlaveList.Count < nCount)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.SlaveList.Count >= nCount)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private static bool ConditionOfCheckMap(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            return QuestConditionInfo.sParam1 == PlayObject.MapName;
        }

        private static bool ConditionOfCheckPos(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            bool result;
            int nX = QuestConditionInfo.nParam2;
            int nY = QuestConditionInfo.nParam3;
            if ((QuestConditionInfo.sParam1 == PlayObject.MapName) && (nX == PlayObject.CurrX) && (nY == PlayObject.CurrY))
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        private static bool ConditionOfReviveSlave(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            string s18;
            FileInfo myFile;
            StringList LoadList;
            string Petname = string.Empty;
            string lvl = string.Empty;
            string lvlexp = string.Empty;
            string sFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "PetData", PlayObject.ChrName + ".txt");
            if (File.Exists(sFileName))
            {
                LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    s18 = LoadList[i].Trim();
                    if ((s18 != "") && (s18[1] != ';'))
                    {
                        s18 = HUtil32.GetValidStr3(s18, ref Petname, HUtil32.Backslash);
                        s18 = HUtil32.GetValidStr3(s18, ref lvl, HUtil32.Backslash);
                        s18 = HUtil32.GetValidStr3(s18, ref lvlexp, HUtil32.Backslash);
                        // PlayObject.ReviveSlave(PetName,str_ToInt(lvl,0),str_ToInt(lvlexp,0),nslavecount,10 * 24 * 60 * 60);
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

        private static bool ConditionOfCheckMagicLvl(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            UserMagic UserMagic;
            for (var i = 0; i < PlayObject.MagicList.Count; i++)
            {
                UserMagic = PlayObject.MagicList[i];
                if (string.Compare(UserMagic.Magic.MagicName, QuestConditionInfo.sParam1, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (UserMagic.Level == QuestConditionInfo.nParam2)
                    {
                        result = true;
                    }
                    break;
                }
            }
            return result;
        }

        private bool ConditionOfCheckGroupClass(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            int nCount = 0;
            PlayJob nJob = PlayJob.None;
            PlayObject PlayObjectEx;
            if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, ScriptConst.sWarrior))
            {
                nJob = PlayJob.Warrior;
            }
            if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, ScriptConst.sWizard))
            {
                nJob = PlayJob.Wizard;
            }
            if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, ScriptConst.sTaos))
            {
                nJob = PlayJob.Taoist;
            }
            if (nJob < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHANGEJOB);
                return result;
            }
            if (PlayObject.GroupOwner != null)
            {
                for (var i = 0; i < PlayObject.GroupMembers.Count; i++)
                {
                    PlayObjectEx = PlayObject.GroupMembers[i];
                    if (PlayObjectEx.Job == nJob)
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

        private bool ConditionOfCheckRangeMonCount(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            BaseObject BaseObject;
            bool result = false;
            string sMapName = QuestConditionInfo.sParam1;
            int nX = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            int nY = HUtil32.StrToInt(QuestConditionInfo.sParam3, -1);
            int nRange = HUtil32.StrToInt(QuestConditionInfo.sParam4, -1);
            char cMethod = QuestConditionInfo.sParam5[0];
            int nCount = HUtil32.StrToInt(QuestConditionInfo.sParam6, -1);
            Envirnoment Envir = M2Share.MapMgr.FindMap(sMapName);
            if ((Envir == null) || (nX < 0) || (nY < 0) || (nRange < 0) || (nCount < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKRANGEMONCOUNT);
                return result;
            }
            IList<BaseObject> MonList = new List<BaseObject>();
            int nMapRangeCount = Envir.GetRangeBaseObject(nX, nY, nRange, true, MonList);
            for (var i = MonList.Count - 1; i >= 0; i--)
            {
                BaseObject = MonList[i];
                if ((BaseObject.Race < ActorRace.Animal) || (BaseObject.Race == ActorRace.ArcherGuard) || (BaseObject.Master != null))
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

        private bool ConditionOfCheckReNewLevel(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            int nLevel = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKLEVELEX);
                return result;
            }
            char cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.MBtReLevel == nLevel)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.MBtReLevel > nLevel)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.MBtReLevel < nLevel)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.MBtReLevel >= nLevel)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckSlaveLevel(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            BaseObject BaseObject;
            bool result = false;
            int nLevel = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKLEVELEX);
                return result;
            }
            var nSlaveLevel = -1;
            for (var i = 0; i < PlayObject.SlaveList.Count; i++)
            {
                BaseObject = PlayObject.SlaveList[i];
                if (BaseObject.Abil.Level > nSlaveLevel)
                {
                    nSlaveLevel = BaseObject.Abil.Level;
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

        private bool ConditionOfCheckUseItem(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            int nWhere = HUtil32.StrToInt(QuestConditionInfo.sParam1, -1);
            if ((nWhere < 0) || (nWhere > PlayObject.UseItems.Length))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKUSEITEM);
                return result;
            }
            if (PlayObject.UseItems[nWhere].Index > 0)
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckVar(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            string sName = string.Empty;
            DynamicVar DynamicVar;
            bool boFoundVar = false;
            var result = false;
            var sType = QuestConditionInfo.sParam1;
            var sVarName = QuestConditionInfo.sParam2;
            var sMethod = QuestConditionInfo.sParam3;
            var nVarValue = HUtil32.StrToInt(QuestConditionInfo.sParam4, 0);
            var sVarValue = QuestConditionInfo.sParam4;
            if ((sType == "") || (sVarName == "") || (sMethod == ""))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKVAR);
                return result;
            }
            var cMethod = sMethod[0];
            var DynamicVarList = GetDynamicVarList(PlayObject, sType, ref sName);
            if (DynamicVarList == null)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKVAR);
                return result;
            }
            if (DynamicVarList.TryGetValue(sVarName, out DynamicVar))
            {
                switch (DynamicVar.VarType)
                {
                    case VarType.Integer:
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
                    case VarType.String:
                        break;
                }
                boFoundVar = true;
            }
            if (!boFoundVar)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKVAR);
            }
            return result;
        }

        private static bool ConditionOfHaveMaster(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            return !string.IsNullOrEmpty(PlayObject.MasterName);
        }

        private static bool ConditionOfPoseHaveMaster(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.Race == ActorRace.Play))
            {
                if (((PlayObject)PoseHuman).MasterName != "")
                {
                    result = true;
                }
            }
            return result;
        }

        private bool ConditionOfCheckCastleGold(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nGold = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if ((nGold < 0) || (Castle == null))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKCASTLEGOLD);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (Castle.TotalGold == nGold)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (Castle.TotalGold > nGold)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (Castle.TotalGold < nGold)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (Castle.TotalGold >= nGold)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckContribution(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nContribution = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nContribution < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKCONTRIBUTION);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.MWContribution == nContribution)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.MWContribution > nContribution)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.MWContribution < nContribution)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.MWContribution >= nContribution)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckCreditPoint(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nCreditPoint = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nCreditPoint < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKCREDITPOINT);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.MBtCreditPoint == nCreditPoint)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.MBtCreditPoint > nCreditPoint)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.MBtCreditPoint < nCreditPoint)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.MBtCreditPoint >= nCreditPoint)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckOfGuild(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if (QuestConditionInfo.sParam1 == "")
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKOFGUILD);
                return result;
            }
            if (PlayObject.MyGuild != null)
            {
                if (string.Compare(PlayObject.MyGuild.sGuildName, QuestConditionInfo.sParam1, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = true;
                }
            }
            return result;
        }

        private bool ConditionOfCheckOnlineLongMin(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nOnlineMin = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nOnlineMin < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_ONLINELONGMIN);
                return result;
            }
            var nOnlineTime = (HUtil32.GetTickCount() - PlayObject.LogonTick) / 60000;
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

        private bool ConditionOfCheckPasswordErrorCount(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nErrorCount = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nErrorCount < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_PASSWORDERRORCOUNT);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.MBtPwdFailCount == nErrorCount)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.MBtPwdFailCount > nErrorCount)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.MBtPwdFailCount < nErrorCount)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.MBtPwdFailCount >= nErrorCount)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private static bool ConditionOfIsLockPassword(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            return PlayObject.MBoPasswordLocked;
        }

        private static bool ConditionOfIsLockStorage(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            return !PlayObject.BoCanGetBackItem;
        }

        private bool ConditionOfCheckPayMent(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nPayMent = HUtil32.StrToInt(QuestConditionInfo.sParam1, -1);
            if (nPayMent < 1)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKPAYMENT);
                return result;
            }
            if (PlayObject.PayMent == nPayMent)
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckSlaveName(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            BaseObject BaseObject;
            var result = false;
            var sSlaveName = QuestConditionInfo.sParam1;
            if (sSlaveName == "")
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKSLAVENAME);
                return result;
            }
            for (var i = 0; i < PlayObject.SlaveList.Count; i++)
            {
                BaseObject = PlayObject.SlaveList[i];
                if (string.Compare(sSlaveName, BaseObject.ChrName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private bool ConditionOfCheckNameDateList(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            StringList LoadList;
            string sListFileName;
            string sLineText;
            string sHumName = string.Empty;
            string sDate = string.Empty;
            DateTime dOldDate = DateTime.Now;
            int nDay;
            var nDayCount = HUtil32.StrToInt(QuestConditionInfo.sParam3, -1);
            var nValNo = M2Share.GetValNameNo(QuestConditionInfo.sParam4);
            var nValNoDay = M2Share.GetValNameNo(QuestConditionInfo.sParam5);
            var boDeleteExprie = string.Compare(QuestConditionInfo.sParam6, "清理", StringComparison.OrdinalIgnoreCase) == 0;
            var boNoCompareHumanName = string.Compare(QuestConditionInfo.sParam6, "1", StringComparison.OrdinalIgnoreCase) == 0;
            var cMethod = QuestConditionInfo.sParam2[0];
            if (nDayCount < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKNAMEDATELIST);
                return result;
            }
            sListFileName = M2Share.Config.EnvirDir + m_sPath + QuestConditionInfo.sParam1;
            if (File.Exists(sListFileName))
            {
                LoadList = new StringList();
                try
                {

                    LoadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    M2Share.Log.Error("loading fail.... => " + sListFileName);
                }
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i].Trim();
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sHumName, new[] { ' ', '\t' });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sDate, new[] { ' ', '\t' });
                    if ((string.Compare(sHumName, PlayObject.ChrName, StringComparison.OrdinalIgnoreCase) == 0) || boNoCompareHumanName)
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
                                    PlayObject.MNVal[nValNo] = nDay;
                                    break;
                                // Modify the A .. B: 100 .. 119
                                case 100:
                                    M2Share.Config.GlobalVal[nValNo - 100] = nDay;
                                    break;
                                // Modify the A .. B: 200 .. 209
                                case 200:
                                    PlayObject.MDyVal[nValNo - 200] = nDay;
                                    break;
                                // Modify the A .. B: 300 .. 399
                                case 300:
                                    PlayObject.MNMval[nValNo - 300] = nDay;
                                    break;
                                // Modify the A .. B: 400 .. 499
                                case 400:
                                    M2Share.Config.GlobaDyMval[nValNo - 400] = (short)nDay;
                                    break;
                            }
                        }
                        if (nValNoDay >= 0)
                        {
                            switch (nValNoDay)
                            {
                                // Modify the A .. B: 0 .. 9
                                case 0:
                                    PlayObject.MNVal[nValNoDay] = nDayCount - nDay;
                                    break;
                                // Modify the A .. B: 100 .. 119
                                case 100:
                                    M2Share.Config.GlobalVal[nValNoDay - 100] = nDayCount - nDay;
                                    break;
                                // Modify the A .. B: 200 .. 209
                                case 200:
                                    PlayObject.MDyVal[nValNoDay - 200] = nDayCount - nDay;
                                    break;
                                // Modify the A .. B: 300 .. 399
                                case 300:
                                    PlayObject.MNMval[nValNoDay - 300] = nDayCount - nDay;
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
                                    M2Share.Log.Error("Save fail.... => " + sListFileName);
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
                M2Share.Log.Error("file not found => " + sListFileName);
            }
            return result;
        }

        // CHECKMAPHUMANCOUNT MAP = COUNT
        private bool ConditionOfCheckMapHumanCount(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nCount = HUtil32.StrToInt(QuestConditionInfo.sParam3, -1);
            if (nCount < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKMAPHUMANCOUNT);
                return result;
            }
            var nHumanCount = M2Share.WorldEngine.GetMapHuman(QuestConditionInfo.sParam1);
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

        private bool ConditionOfCheckMapMonCount(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nCount = HUtil32.StrToInt(QuestConditionInfo.sParam3, -1);
            var Envir = M2Share.MapMgr.FindMap(QuestConditionInfo.sParam1);
            if ((nCount < 0) || (Envir == null))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, ScriptConst.sSC_CHECKMAPMONCOUNT);
                return result;
            }
            var nMonCount = M2Share.WorldEngine.GetMapMonster(Envir, null);
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

        private static bool ConditionOfCheckIsOnMap(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            if (PlayObject.MapFileName == QuestConditionInfo.sParam1 || PlayObject.MapName == QuestConditionInfo.sParam1)
            {
                return true;
            }
            return false;
        }

        private bool LargeData(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            int n14 = 0;
            int n18 = 0;
            bool result;
            if (CheckVarNameNo(PlayObject, QuestConditionInfo, ref n14, ref n18))
            {
                result = n14 > n18;
            }
            else
            {
                if (!GetValValue(PlayObject, QuestConditionInfo.sParam1, ref n14))
                {
                    n14 = HUtil32.StrToInt(GetLineVariableText(PlayObject, QuestConditionInfo.sParam1), -1);
                }
                if (!GetValValue(PlayObject, QuestConditionInfo.sParam2, ref n18))
                {
                    n18 = HUtil32.StrToInt(GetLineVariableText(PlayObject, QuestConditionInfo.sParam2), -1);
                }
                result = n14 > n18;
            }
            return result;
        }

        private bool Smalldata(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            int n14 = 0;
            int n18 = 0;
            bool result;
            if (CheckVarNameNo(PlayObject, QuestConditionInfo, ref n14, ref n18))
            {
                result = n14 < n18;
            }
            else
            {
                if (!GetValValue(PlayObject, QuestConditionInfo.sParam1, ref n14))
                {
                    n14 = HUtil32.StrToInt(GetLineVariableText(PlayObject, QuestConditionInfo.sParam1), -1);
                }
                if (!GetValValue(PlayObject, QuestConditionInfo.sParam2, ref n18))
                {
                    n18 = HUtil32.StrToInt(GetLineVariableText(PlayObject, QuestConditionInfo.sParam2), -1);
                }
                result = n14 < n18;
            }
            return result;
        }

        private bool EqualData(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo)
        {
            int n14 = 0;
            int n18 = 0;
            var s01 = string.Empty;
            var s02 = string.Empty;
            bool result;
            if (CheckVarNameNo(PlayObject, QuestConditionInfo, ref n14, ref n18))// 比较数值
            {
                result = n14 == n18;
            }
            else
            {
                if (!GetValValue(PlayObject, QuestConditionInfo.sParam1, ref s01))
                {
                    s01 = GetLineVariableText(PlayObject, QuestConditionInfo.sParam1);//  支持变量
                }
                if (!GetValValue(PlayObject, QuestConditionInfo.sParam2, ref s02))
                {
                    s02 = GetLineVariableText(PlayObject, QuestConditionInfo.sParam2);//  支持变量
                }
                result = string.Compare(s01, s02, StringComparison.OrdinalIgnoreCase) == 0;
            }
            return result;
        }

        private bool CheckVarNameNo(PlayObject PlayObject, QuestConditionInfo CheckQuestConditionInfo, ref int n140, ref int n180)
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

        private static int GotoLable_CheckVarNameNo_GetDataType(QuestConditionInfo CheckQuestConditionInfo)
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

        private bool GotoLable_CheckVarNameNo_GetDynamicVarValue(PlayObject PlayObject, string sVarType, string sValName, ref int nValue)
        {
            bool result = false;
            DynamicVar DynamicVar;
            string sName = string.Empty;
            Dictionary<string, DynamicVar> DynamicVarList = GetDynamicVarList(PlayObject, sVarType, ref sName);
            if (DynamicVarList == null)
            {
                return result;
            }
            else
            {
                if (DynamicVarList.TryGetValue(sValName, out DynamicVar))
                {
                    switch (DynamicVar.VarType)
                    {
                        case VarType.Integer:
                            nValue = DynamicVar.nInternet;
                            result = true;
                            break;
                        case VarType.String:
                            break;
                    }
                }
            }
            return result;
        }

        private static bool GotoLable_CheckVarNameNo_GetValValue(PlayObject PlayObject, string sValName, ref int nValue)
        {
            nValue = 0;
            bool result = false;
            int n100 = M2Share.GetValNameNo(sValName);
            if (n100 >= 0)
            {
                if (HUtil32.RangeInDefined(n100, 0, 99))
                {
                    nValue = PlayObject.MNVal[n100];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 100, 199))
                {
                    nValue = M2Share.Config.GlobalVal[n100 - 100];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 200, 299))
                {
                    nValue = PlayObject.MDyVal[n100 - 200];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 300, 399))
                {
                    nValue = PlayObject.MNMval[n100 - 300];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 400, 499))
                {
                    nValue = M2Share.Config.GlobaDyMval[n100 - 400];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 500, 599))
                {
                    nValue = PlayObject.MNInteger[n100 - 500];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 600, 699))
                {
                    if (HUtil32.IsStringNumber(PlayObject.MSString[n100 - 600]))
                    {
                        nValue = HUtil32.StrToInt(PlayObject.MSString[n100 - 600], 0);
                        result = true;
                    }
                }
                else if (HUtil32.RangeInDefined(n100, 700, 799))
                {
                    if (HUtil32.IsStringNumber(M2Share.Config.GlobalAVal[n100 - 700]))
                    {
                        nValue = HUtil32.StrToInt(M2Share.Config.GlobalAVal[n100 - 700], 0);
                        result = true;
                    }
                }
                else if (HUtil32.RangeInDefined(n100, 800, 1199))//G变量
                {
                    nValue = M2Share.Config.GlobalVal[n100 - 700];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 1200, 1599))//G变量
                {
                    if (HUtil32.IsStringNumber(M2Share.Config.GlobalAVal[n100 - 1100]))
                    {
                        nValue = HUtil32.StrToInt(M2Share.Config.GlobalAVal[n100 - 1100], 0);
                        result = true;
                    }
                }
                else if (HUtil32.RangeInDefined(n100, 1600, 1699))//G变量
                {
                    if (HUtil32.IsStringNumber(PlayObject.MServerStrVal[n100 - 1600]))
                    {
                        nValue = HUtil32.StrToInt(PlayObject.MServerStrVal[n100 - 1600], 0);
                        result = true;
                    }
                }
                else if (HUtil32.RangeInDefined(n100, 1700, 1799))//G变量
                {
                    nValue = PlayObject.MServerIntVal[n100 - 1700];
                    result = true;
                }
            }
            return result;
        }
    }
}
