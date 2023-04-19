using GameSrv.Actor;
using GameSrv.Guild;
using GameSrv.Items;
using GameSrv.Maps;
using GameSrv.Monster.Monsters;
using GameSrv.Player;
using GameSrv.Script;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.Npc {
    public partial class NormNpc {

        private static bool ConditionOfCheckHPCheckHigh(PlayObject PlayObject, char cMethodMax, int nMax) {
            bool result = false;
            switch (cMethodMax) {
                case '=':
                    if (PlayObject.WAbil.MaxHP == nMax) {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.WAbil.MaxHP > nMax) {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.WAbil.MaxHP < nMax) {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.WAbil.MaxHP >= nMax) {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private static bool ConditionOfCheckSC_CheckHigh(PlayObject PlayObject, char cMethodMax, int nMax) {
            bool result = false;
            switch (cMethodMax) {
                case '=':
                    if (HUtil32.HiWord(PlayObject.WAbil.SC) == nMax) {
                        result = true;
                    }
                    break;
                case '>':
                    if (HUtil32.HiWord(PlayObject.WAbil.SC) > nMax) {
                        result = true;
                    }
                    break;
                case '<':
                    if (HUtil32.HiWord(PlayObject.WAbil.SC) < nMax) {
                        result = true;
                    }
                    break;
                default:
                    if (HUtil32.HiWord(PlayObject.WAbil.SC) >= nMax) {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckFlourishPoint(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo) {
            bool result = false;
            int nPoint = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            if (nPoint < 0) {
                ScriptConditionError(PlayObject, QuestConditionInfo, ConditionCode.CHECKFLOURISHPOINT);
                return false;
            }
            if (PlayObject.MyGuild == null) {
                return false;
            }
            GuildInfo Guild = PlayObject.MyGuild;
            char cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod) {
                case '=':
                    if (Guild.Flourishing == nPoint) {
                        result = true;
                    }
                    break;
                case '>':
                    if (Guild.Flourishing > nPoint) {
                        result = true;
                    }
                    break;
                case '<':
                    if (Guild.Flourishing < nPoint) {
                        result = true;
                    }
                    break;
                default:
                    if (Guild.Flourishing >= nPoint) {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckInMapRange(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo) {
            string sMapName = QuestConditionInfo.sParam1;
            int nX = HUtil32.StrToInt(QuestConditionInfo.sParam2, -1);
            int nY = HUtil32.StrToInt(QuestConditionInfo.sParam3, -1);
            int nRange = HUtil32.StrToInt(QuestConditionInfo.sParam4, -1);
            if ((string.IsNullOrEmpty(sMapName)) || (nX < 0) || (nY < 0) || (nRange < 0)) {
                ScriptConditionError(PlayObject, QuestConditionInfo, ConditionCode.CHECKINMAPRANGE);
                return false;
            }
            if (string.Compare(PlayObject.MapName, sMapName, StringComparison.OrdinalIgnoreCase) != 0) {
                return false;
            }
            if ((Math.Abs(PlayObject.CurrX - nX) <= nRange) && (Math.Abs(PlayObject.CurrY - nY) <= nRange)) {
                return true;
            }
            return false;
        }

        private bool LargeData(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo) {
            int n14 = 0;
            int n18 = 0;
            bool result;
            if (CheckVarNameNo(PlayObject, QuestConditionInfo, ref n14, ref n18)) {
                result = n14 > n18;
            }
            else {
                if (!GetValValue(PlayObject, QuestConditionInfo.sParam1, ref n14)) {
                    n14 = HUtil32.StrToInt(GetLineVariableText(PlayObject, QuestConditionInfo.sParam1), -1);
                }
                if (!GetValValue(PlayObject, QuestConditionInfo.sParam2, ref n18)) {
                    n18 = HUtil32.StrToInt(GetLineVariableText(PlayObject, QuestConditionInfo.sParam2), -1);
                }
                result = n14 > n18;
            }
            return result;
        }

        private bool Smalldata(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo) {
            int n14 = 0;
            int n18 = 0;
            bool result;
            if (CheckVarNameNo(PlayObject, QuestConditionInfo, ref n14, ref n18)) {
                result = n14 < n18;
            }
            else {
                if (!GetValValue(PlayObject, QuestConditionInfo.sParam1, ref n14)) {
                    n14 = HUtil32.StrToInt(GetLineVariableText(PlayObject, QuestConditionInfo.sParam1), -1);
                }
                if (!GetValValue(PlayObject, QuestConditionInfo.sParam2, ref n18)) {
                    n18 = HUtil32.StrToInt(GetLineVariableText(PlayObject, QuestConditionInfo.sParam2), -1);
                }
                result = n14 < n18;
            }
            return result;
        }

        private bool EqualData(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo) {
            int n14 = 0;
            int n18 = 0;
            string s01 = string.Empty;
            string s02 = string.Empty;
            bool result;
            if (CheckVarNameNo(PlayObject, QuestConditionInfo, ref n14, ref n18))// 比较数值
            {
                result = n14 == n18;
            }
            else {
                if (!GetValValue(PlayObject, QuestConditionInfo.sParam1, ref s01)) {
                    s01 = GetLineVariableText(PlayObject, QuestConditionInfo.sParam1);//  支持变量
                }
                if (!GetValValue(PlayObject, QuestConditionInfo.sParam2, ref s02)) {
                    s02 = GetLineVariableText(PlayObject, QuestConditionInfo.sParam2);//  支持变量
                }
                result = string.Compare(s01, s02, StringComparison.OrdinalIgnoreCase) == 0;
            }
            return result;
        }

        private bool CheckVarNameNo(PlayObject PlayObject, QuestConditionInfo CheckQuestConditionInfo, ref int n140, ref int n180) {
            bool result = false;
            string sParam1 = string.Empty;
            string sParam2 = string.Empty;
            string sParam3 = string.Empty;
            n140 = -1;
            n180 = -1;
            if (HUtil32.CompareLStr(CheckQuestConditionInfo.sParam1, "<$STR(", 6)) {
                HUtil32.ArrestStringEx(CheckQuestConditionInfo.sParam1, "(", ")", ref sParam1);
            }
            else {
                sParam1 = CheckQuestConditionInfo.sParam1;
            }
            if (HUtil32.CompareLStr(CheckQuestConditionInfo.sParam2, "<$STR(", 6)) {
                HUtil32.ArrestStringEx(CheckQuestConditionInfo.sParam2, "(", ")", ref sParam2);
            }
            else {
                sParam2 = CheckQuestConditionInfo.sParam2;
            }
            if (HUtil32.CompareLStr(CheckQuestConditionInfo.sParam3, "<$STR(", 6)) {
                HUtil32.ArrestStringEx(CheckQuestConditionInfo.sParam3, "(", ")", ref sParam3);
            }
            else {
                sParam3 = CheckQuestConditionInfo.sParam3;
            }
            switch (GotoLable_CheckVarNameNo_GetDataType(CheckQuestConditionInfo)) {
                case 0:
                    if (GotoLable_CheckVarNameNo_GeDynamicVarValue(PlayObject, sParam1, sParam2, ref n140) && GotoLable_CheckVarNameNo_GetValValue(PlayObject, sParam3, ref n180)) {
                        result = true;
                    }
                    break;
                case 1:
                    n180 = CheckQuestConditionInfo.nParam3;
                    if (GotoLable_CheckVarNameNo_GeDynamicVarValue(PlayObject, sParam1, sParam2, ref n140)) {
                        result = true;
                    }
                    break;
                case 2:
                    if (GotoLable_CheckVarNameNo_GetValValue(PlayObject, sParam1, ref n140) && GotoLable_CheckVarNameNo_GetValValue(PlayObject, sParam2, ref n180)) {
                        result = true;
                    }
                    break;
                case 3:
                    if (GotoLable_CheckVarNameNo_GetValValue(PlayObject, sParam1, ref n140) && GotoLable_CheckVarNameNo_GeDynamicVarValue(PlayObject, sParam2, sParam3, ref n180)) {
                        result = true;
                    }
                    break;
                case 4:
                    n180 = CheckQuestConditionInfo.nParam2;
                    if (GotoLable_CheckVarNameNo_GetValValue(PlayObject, sParam1, ref n140)) {
                        result = true;
                    }
                    break;
                case 5:
                    break;
            }
            return result;
        }

        private static int GotoLable_CheckVarNameNo_GetDataType(QuestConditionInfo CheckQuestConditionInfo) {
            int result;
            string sParam1 = string.Empty;
            string sParam2 = string.Empty;
            string sParam3 = string.Empty;
            result = -1;
            if (HUtil32.CompareLStr(CheckQuestConditionInfo.sParam1, "<$STR(", 6)) {
                HUtil32.ArrestStringEx(CheckQuestConditionInfo.sParam1, "(", ")", ref sParam1);
            }
            else {
                sParam1 = CheckQuestConditionInfo.sParam1;
            }
            if (HUtil32.CompareLStr(CheckQuestConditionInfo.sParam2, "<$STR(", 6)) {
                HUtil32.ArrestStringEx(CheckQuestConditionInfo.sParam2, "(", ")", ref sParam2);
            }
            else {
                sParam2 = CheckQuestConditionInfo.sParam2;
            }
            if (HUtil32.CompareLStr(CheckQuestConditionInfo.sParam3, "<$STR(", 6)) {
                HUtil32.ArrestStringEx(CheckQuestConditionInfo.sParam3, "(", ")", ref sParam3);
            }
            else {
                sParam3 = CheckQuestConditionInfo.sParam3;
            }
            if (HUtil32.IsVarNumber(sParam1)) {
                if ((!string.IsNullOrEmpty(sParam3)) && (M2Share.GetValNameNo(sParam3) >= 0)) {
                    result = 0;
                }
                else if ((!string.IsNullOrEmpty(sParam3)) && HUtil32.IsStringNumber(sParam3)) {
                    result = 1;
                }
                return result;
            }
            if (M2Share.GetValNameNo(sParam1) >= 0) {
                if (((!string.IsNullOrEmpty(sParam2))) && (M2Share.GetValNameNo(sParam2) >= 0)) {
                    result = 2;
                }
                else if (((!string.IsNullOrEmpty(sParam2))) && HUtil32.IsVarNumber(sParam2) && (!string.IsNullOrEmpty(sParam3))) {
                    result = 3;
                }
                else if (((!string.IsNullOrEmpty(sParam2))) && HUtil32.IsStringNumber(sParam2)) {
                    result = 4;
                }
            }
            return result;
        }

        private static bool GotoLable_CheckVarNameNo_GeDynamicVarValue(PlayObject PlayObject, string sVarType, string sValName, ref int nValue) {
            bool result = false;
            string sName = string.Empty;
            Dictionary<string, DynamicVar> DynamicVarList = GetDynamicVarMap(PlayObject, sVarType, ref sName);
            if (DynamicVarList == null) {
                return result;
            }
            else {
                if (DynamicVarList.TryGetValue(sValName, out DynamicVar DynamicVar)) {
                    switch (DynamicVar.VarType) {
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

        private static bool GotoLable_CheckVarNameNo_GetValValue(PlayObject PlayObject, string sValName, ref int nValue) {
            nValue = 0;
            bool result = false;
            int n100 = M2Share.GetValNameNo(sValName);
            if (n100 >= 0) {
                if (HUtil32.RangeInDefined(n100, 0, 99)) {
                    nValue = PlayObject.MNVal[n100];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 100, 199)) {
                    nValue = M2Share.Config.GlobalVal[n100 - 100];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 200, 299)) {
                    nValue = PlayObject.MDyVal[n100 - 200];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 300, 399)) {
                    nValue = PlayObject.MNMval[n100 - 300];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 400, 499)) {
                    nValue = M2Share.Config.GlobaDyMval[n100 - 400];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 500, 599)) {
                    nValue = PlayObject.MNInteger[n100 - 500];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 600, 699)) {
                    if (HUtil32.IsStringNumber(PlayObject.MSString[n100 - 600])) {
                        nValue = HUtil32.StrToInt(PlayObject.MSString[n100 - 600], 0);
                        result = true;
                    }
                }
                else if (HUtil32.RangeInDefined(n100, 700, 799)) {
                    if (HUtil32.IsStringNumber(M2Share.Config.GlobalAVal[n100 - 700])) {
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
                    if (HUtil32.IsStringNumber(M2Share.Config.GlobalAVal[n100 - 1100])) {
                        nValue = HUtil32.StrToInt(M2Share.Config.GlobalAVal[n100 - 1100], 0);
                        result = true;
                    }
                }
                else if (HUtil32.RangeInDefined(n100, 1600, 1699))//G变量
                {
                    if (HUtil32.IsStringNumber(PlayObject.MServerStrVal[n100 - 1600])) {
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