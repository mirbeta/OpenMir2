using System;
using System.Collections.Generic;
using SystemModule;

namespace GameSvr
{
    public partial class TNormNpc
    {
        private void MovrData(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var n14 = 0;
            var s34 = string.Empty;
            if (HUtil32.CompareLStr(QuestActionInfo.sParam1, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam1, "(", ")", ref s34);
                n14 = M2Share.GetValNameNo(s34);
            }
            else
            {
                n14 = M2Share.GetValNameNo(QuestActionInfo.sParam1);
            }
            if (n14 >= 0)
            {
                if (HUtil32.RangeInDefined(n14, 0, 99))
                {
                    if (QuestActionInfo.nParam3 > QuestActionInfo.nParam2)
                    {
                        PlayObject.m_nVal[n14] = QuestActionInfo.nParam2 + M2Share.RandomNumber.Random(QuestActionInfo.nParam3 - QuestActionInfo.nParam2);
                    }
                    else
                    {
                        PlayObject.m_nVal[n14] = M2Share.RandomNumber.Random(QuestActionInfo.nParam2);
                    }
                }
                else if (HUtil32.RangeInDefined(n14, 100, 119))
                {
                    if (QuestActionInfo.nParam3 > QuestActionInfo.nParam2)
                    {
                        M2Share.g_Config.GlobalVal[n14 - 100] = QuestActionInfo.nParam2 + M2Share.RandomNumber.Random(QuestActionInfo.nParam3 - QuestActionInfo.nParam2);
                    }
                    else
                    {
                        M2Share.g_Config.GlobalVal[n14 - 100] = M2Share.RandomNumber.Random(QuestActionInfo.nParam2);
                    }
                }
                else if (HUtil32.RangeInDefined(n14, 200, 299))
                {
                    if (QuestActionInfo.nParam3 > QuestActionInfo.nParam2)
                    {
                        PlayObject.m_DyVal[n14 - 200] = QuestActionInfo.nParam2 + M2Share.RandomNumber.Random(QuestActionInfo.nParam3 - QuestActionInfo.nParam2);
                    }
                    else
                    {
                        PlayObject.m_DyVal[n14 - 200] = M2Share.RandomNumber.Random(QuestActionInfo.nParam2);
                    }
                }
                else if (HUtil32.RangeInDefined(n14, 300, 399))
                {
                    if (QuestActionInfo.nParam3 > QuestActionInfo.nParam2)
                    {
                        PlayObject.m_nMval[n14 - 300] = QuestActionInfo.nParam2 + M2Share.RandomNumber.Random(QuestActionInfo.nParam3 - QuestActionInfo.nParam2);
                    }
                    else
                    {
                        PlayObject.m_nMval[n14 - 300] = M2Share.RandomNumber.Random(QuestActionInfo.nParam2);
                    }
                }
                else if (HUtil32.RangeInDefined(n14, 400, 499))
                {
                    if (QuestActionInfo.nParam3 > QuestActionInfo.nParam2)
                    {
                        M2Share.g_Config.GlobaDyMval[n14 - 400] = QuestActionInfo.nParam2 + M2Share.RandomNumber.Random(QuestActionInfo.nParam3 - QuestActionInfo.nParam2);
                    }
                    else
                    {
                        M2Share.g_Config.GlobaDyMval[n14 - 400] = M2Share.RandomNumber.Random(QuestActionInfo.nParam2);
                    }
                }
                else if (HUtil32.RangeInDefined(n14, 500, 599))
                {
                    if (QuestActionInfo.nParam3 > QuestActionInfo.nParam2)
                    {
                        PlayObject.m_nInteger[n14 - 500] = QuestActionInfo.nParam2 + M2Share.RandomNumber.Random(QuestActionInfo.nParam3 - QuestActionInfo.nParam2);
                    }
                    else
                    {
                        PlayObject.m_nInteger[n14 - 500] = M2Share.RandomNumber.Random(QuestActionInfo.nParam2);
                    }
                }
                else if (HUtil32.RangeInDefined(n14, 800, 1199))
                {
                    if (QuestActionInfo.nParam3 > QuestActionInfo.nParam2)
                    {
                        M2Share.g_Config.GlobalVal[n14 - 700] = QuestActionInfo.nParam2 + M2Share.RandomNumber.Random(QuestActionInfo.nParam3 - QuestActionInfo.nParam2);
                    }
                    else
                    {
                        M2Share.g_Config.GlobalVal[n14 - 700] = M2Share.RandomNumber.Random(QuestActionInfo.nParam2);
                    }
                }
                else
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sMOVR);
                }
            }
            else
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sMOVR);
            }
        }

        private void MovData(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sParam1 = string.Empty;
            string sParam2 = string.Empty;
            string sParam3 = string.Empty;
            string sValue = string.Empty;
            int nValue = 0;
            int nDataType = 0;
            const string sVarFound = "变量{0}不存在，变量类型:{1}";
            if (HUtil32.CompareLStr(QuestActionInfo.sParam1, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam1, "(", ")", ref sParam1);
            }
            else
            {
                sParam1 = QuestActionInfo.sParam1;
            }
            if (HUtil32.CompareLStr(QuestActionInfo.sParam2, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam2, "(", ")", ref sParam2);
            }
            else
            {
                sParam2 = QuestActionInfo.sParam2;
            }
            if (HUtil32.CompareLStr(QuestActionInfo.sParam3, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam3, "(", ")", ref sParam3);
            }
            else
            {
                sParam3 = QuestActionInfo.sParam3;
            }
            if (sParam1 == "")
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sMOV);
                return;
            }
            switch (GetMovDataType(QuestActionInfo))
            {
                case 0:
                    if (GetMovDataHumanInfoValue(PlayObject, sParam3, ref sValue, ref nValue, ref nDataType))
                    {
                        if (!SetMovDataDynamicVarValue(PlayObject, sParam1, sParam2, sValue, nValue, nDataType))
                        {
                            ScriptActionError(PlayObject, string.Format(sVarFound, sParam1, sParam2), QuestActionInfo, M2Share.sMOV);
                        }
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sMOV);
                    }
                    break;
                case 1:
                    if (GetMovDataValNameValue(PlayObject, sParam3, ref sValue, ref nValue, ref nDataType))
                    {
                        if (!SetMovDataDynamicVarValue(PlayObject, sParam1, sParam2, sValue, nValue, nDataType))
                        {
                            ScriptActionError(PlayObject, string.Format(sVarFound, sParam1, sParam2), QuestActionInfo, M2Share.sMOV);
                        }
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sMOV);
                    }
                    break;
                case 2:
                    if (!SetMovDataDynamicVarValue(PlayObject, sParam1, sParam2, QuestActionInfo.sParam3, QuestActionInfo.nParam3, 1))
                    {
                        ScriptActionError(PlayObject, string.Format(sVarFound, new string[] { sParam1, sParam2 }), QuestActionInfo, M2Share.sMOV);
                    }
                    break;
                case 3:
                    if (!SetMovDataDynamicVarValue(PlayObject, sParam1, sParam2, QuestActionInfo.sParam3, QuestActionInfo.nParam3, 0))
                    {
                        ScriptActionError(PlayObject, string.Format(sVarFound, sParam1, sParam2), QuestActionInfo, M2Share.sMOV);
                    }
                    break;
                case 4:
                    if (GetMovDataHumanInfoValue(PlayObject, sParam2, ref sValue, ref nValue, ref nDataType))
                    {
                        if (!SetMovDataValNameValue(PlayObject, sParam1, sValue, nValue, nDataType))
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sMOV);
                        }
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sMOV);
                    }
                    break;
                case 5:
                    if (GetMovDataValNameValue(PlayObject, sParam2, ref sValue, ref nValue, ref nDataType))
                    {
                        if (!SetMovDataValNameValue(PlayObject, sParam1, sValue, nValue, nDataType))
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sMOV);
                        }
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sMOV);
                    }
                    break;
                case 6:
                    if (GetMovDataDynamicVarValue(PlayObject, sParam2, sParam3, ref sValue, ref nValue, ref nDataType))
                    {
                        if (!SetMovDataValNameValue(PlayObject, sParam1, sValue, nValue, nDataType))
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sMOV);
                        }
                    }
                    else
                    {
                        ScriptActionError(PlayObject, string.Format(sVarFound, sParam2, sParam3), QuestActionInfo, M2Share.sMOV);
                    }
                    break;
                case 7:
                    if (GetMovDataValNameValue(PlayObject, sParam1, ref sValue, ref nValue, ref nDataType))
                    {
                        if ((sParam2 != "") && (sParam2[0] == '<') && (sParam2[1] == '$'))//  支持:MOV A14 <$USERALLNAME>\天下第一战士 的传值
                        {
                            GetMovDataHumanInfoValue(PlayObject, sParam2, ref sValue, ref nValue, ref nDataType);// 取人物信息
                            sValue = sValue + sParam2.Substring(sParam2.IndexOf("\\", StringComparison.CurrentCultureIgnoreCase) - 1, sParam2.Length - sParam2.IndexOf("\\", StringComparison.CurrentCultureIgnoreCase) + 1);
                            if (!SetMovDataValNameValue(PlayObject, sParam1, sValue, nValue, nDataType))
                            {
                                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sMOV);
                                return;
                            }
                        }
                        else
                        {
                            if (!SetMovDataValNameValue(PlayObject, sParam1, QuestActionInfo.sParam2, QuestActionInfo.nParam2, nDataType))
                            {
                                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sMOV);
                                return;
                            }
                        }
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sMOV);
                    }
                    break;
                default:
                    ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sMOV);
                    break;
            }
        }

        private void IncInteger(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int n14;
            int n3C = 0;
            string s01 = string.Empty;
            TDynamicVar DynamicVar;
            IList<TDynamicVar> DynamicVarList;
            string sName = string.Empty;
            bool boVarFound;
            string sParam1 = string.Empty;
            string sParam2 = string.Empty;
            string sParam3 = string.Empty;
            const string sVarFound = "变量{0}不存在，变量类型:{1}";
            const string sVarTypeError = "变量类型错误，错误类型:{0} 当前支持类型(HUMAN、GUILD、GLOBAL)";
            int n10 = 0;
            if (HUtil32.CompareLStr(QuestActionInfo.sParam1, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam1, "(", ")", ref sParam1);
            }
            else
            {
                sParam1 = QuestActionInfo.sParam1;
            }
            if (HUtil32.CompareLStr(QuestActionInfo.sParam2, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam2, "(", ")", ref sParam2);
            }
            else
            {
                sParam2 = QuestActionInfo.sParam2;
            }
            if (HUtil32.CompareLStr(QuestActionInfo.sParam3, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam3, "(", ")", ref sParam3);
            }
            else
            {
                sParam3 = QuestActionInfo.sParam3;
            }
            if ((sParam1 == "") || (sParam2 == ""))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sINC);
                return;
            }
            if (sParam3 != "")
            {
                if ((!HUtil32.IsVarNumber(sParam1)) && (HUtil32.IsVarNumber(sParam2)))
                {
                    n10 = 1;
                    boVarFound = false;
                    DynamicVarList = GetDynamicVarList(PlayObject, sParam2, ref sName);
                    if (DynamicVarList == null)
                    {
                        ScriptActionError(PlayObject, string.Format(sVarTypeError, sParam2), QuestActionInfo, M2Share.sINC);
                        return;
                    }
                    if (DynamicVarList.Count > 0)
                    {
                        for (var i = 0; i < DynamicVarList.Count; i++)
                        {
                            DynamicVar = DynamicVarList[i];
                            if (string.Compare(DynamicVar.sName, sParam3, StringComparison.CurrentCultureIgnoreCase) == 0)
                            {
                                switch (DynamicVar.VarType)
                                {
                                    case TVarType.Integer:
                                        n3C = DynamicVar.nInternet;
                                        break;
                                    case TVarType.String:
                                        s01 = DynamicVar.sString;
                                        break;
                                }
                                boVarFound = true;
                                break;
                            }
                        }
                    }
                    if (!boVarFound)
                    {
                        ScriptActionError(PlayObject, string.Format(sVarFound, sParam3, sParam2), QuestActionInfo, M2Share.sINC);
                        return;
                    }
                    n14 = M2Share.GetValNameNo(sParam1);
                    if (n14 >= 0)
                    {
                        if (HUtil32.RangeInDefined(n14, 0, 99))
                        {
                            if (n3C > 1)
                            {
                                PlayObject.m_nVal[n14] += n3C;
                            }
                            else
                            {
                                PlayObject.m_nVal[n14]++;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 100, 199))
                        {
                            if (n3C > 1)
                            {
                                M2Share.g_Config.GlobalVal[n14 - 100] += n3C;
                            }
                            else
                            {
                                M2Share.g_Config.GlobalVal[n14 - 100]++;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 200, 299))
                        {
                            if (n3C > 1)
                            {
                                PlayObject.m_DyVal[n14 - 200] += n3C;
                            }
                            else
                            {
                                PlayObject.m_DyVal[n14 - 200]++;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 300, 399))
                        {
                            if (n3C > 1)
                            {
                                PlayObject.m_nMval[n14 - 300] += n3C;
                            }
                            else
                            {
                                PlayObject.m_nMval[n14 - 300]++;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 400, 499))
                        {
                            if (n3C > 1)
                            {
                                M2Share.g_Config.GlobaDyMval[n14 - 400] += n3C;
                            }
                            else
                            {
                                M2Share.g_Config.GlobaDyMval[n14 - 400]++;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 500, 599))
                        {
                            if (n3C > 1)
                            {
                                PlayObject.m_nInteger[n14 - 500] += n3C;
                            }
                            else
                            {
                                PlayObject.m_nInteger[n14 - 500]++;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 600, 699))
                        {
                            PlayObject.m_sString[n14 - 600] = PlayObject.m_sString[n14 - 600] + s01;
                        }
                        else if (HUtil32.RangeInDefined(n14, 700, 799))
                        {
                            M2Share.g_Config.GlobalAVal[n14 - 700] = M2Share.g_Config.GlobalAVal[n14 - 700] + s01;
                        }
                        else if (HUtil32.RangeInDefined(n14, 800, 1199)) // G变量
                        {
                            if (n3C > 1)
                            {
                                M2Share.g_Config.GlobalVal[n14 - 700] += n3C;
                            }
                            else
                            {
                                M2Share.g_Config.GlobalVal[n14 - 700]++;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 1200, 1599)) // A变量
                        {
                            M2Share.g_Config.GlobalAVal[n14 - 1100] = M2Share.g_Config.GlobalAVal[n14 - 1100] + s01;
                        }
                        else
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sINC);
                        }
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sINC);
                        return;
                    }
                    return;
                }
                if ((HUtil32.IsVarNumber(sParam1)) && (!HUtil32.IsVarNumber(sParam2)))
                {
                    if ((sParam3 != "") && (!HUtil32.IsStringNumber(sParam3)))
                    {
                        n10 = 1;
                        n14 = M2Share.GetValNameNo(sParam3);
                        if (n14 >= 0)
                        {
                            if (HUtil32.RangeInDefined(n14, 0, 99))
                            {
                                n3C = PlayObject.m_nVal[n14];
                            }
                            else if (HUtil32.RangeInDefined(n14, 100, 199))
                            {
                                n3C = M2Share.g_Config.GlobalVal[n14 - 100];
                            }
                            else if (HUtil32.RangeInDefined(n14, 200, 299))
                            {
                                n3C = PlayObject.m_DyVal[n14 - 200];
                            }
                            else if (HUtil32.RangeInDefined(n14, 300, 399))
                            {
                                n3C = PlayObject.m_nMval[n14 - 300];
                            }
                            else if (HUtil32.RangeInDefined(n14, 400, 499))
                            {
                                n3C = M2Share.g_Config.GlobaDyMval[n14 - 400];
                            }
                            else if (HUtil32.RangeInDefined(n14, 500, 599))
                            {
                                n3C = PlayObject.m_nInteger[n14 - 500];
                            }
                            else if (HUtil32.RangeInDefined(n14, 600, 699))
                            {
                                s01 = PlayObject.m_sString[n14 - 600];
                            }
                            else if (HUtil32.RangeInDefined(n14, 700, 799))
                            {
                                s01 = M2Share.g_Config.GlobalAVal[n14 - 700];
                            }
                            else if (HUtil32.RangeInDefined(n14, 800, 1199)) // G变量
                            {
                                n3C = M2Share.g_Config.GlobalVal[n14 - 700];
                            }
                            else if (HUtil32.RangeInDefined(n14, 1200, 1599)) // A变量
                            {
                                s01 = M2Share.g_Config.GlobalAVal[n14 - 1100];
                            }
                            else
                            {
                                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sINC);
                            }
                        }
                        else
                        {
                            s01 = sParam3;
                        }
                    }
                    else
                    {
                        n3C = QuestActionInfo.nParam3;
                    }
                    boVarFound = false;
                    DynamicVarList = GetDynamicVarList(PlayObject, sParam1, ref sName);
                    if (DynamicVarList == null)
                    {
                        ScriptActionError(PlayObject, string.Format(sVarTypeError, sParam1), QuestActionInfo, M2Share.sINC);
                        return;
                    }
                    if (DynamicVarList.Count > 0)
                    {
                        for (var i = 0; i < DynamicVarList.Count; i++)
                        {
                            DynamicVar = DynamicVarList[i];
                            if (string.Compare(DynamicVar.sName, sParam2, StringComparison.CurrentCultureIgnoreCase) == 0)
                            {
                                switch (DynamicVar.VarType)
                                {
                                    case TVarType.Integer:
                                        if (n3C > 1)
                                        {
                                            DynamicVar.nInternet += n3C;
                                        }
                                        else
                                        {
                                            DynamicVar.nInternet++;
                                        }
                                        break;
                                    case TVarType.String:
                                        DynamicVar.sString = DynamicVar.sString + s01;
                                        break;
                                }
                                boVarFound = true;
                                break;
                            }
                        }
                    }
                    if (!boVarFound)
                    {
                        ScriptActionError(PlayObject, string.Format(sVarFound, sParam2, sParam1), QuestActionInfo, M2Share.sINC);
                        return;
                    }
                    return;
                }
                if (n10 == 0)
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sINC);
                }
            }
            else
            {
                if ((sParam2 != "") && (!HUtil32.IsStringNumber(sParam2)))
                {
                    // 获取第2个变量值
                    n14 = M2Share.GetValNameNo(sParam2);
                    if (n14 >= 0)
                    {
                        if (HUtil32.RangeInDefined(n14, 0, 99))
                        {
                            n3C = PlayObject.m_nVal[n14];
                        }
                        else if (HUtil32.RangeInDefined(n14, 100, 199))
                        {
                            n3C = M2Share.g_Config.GlobalVal[n14 - 100];
                        }
                        else if (HUtil32.RangeInDefined(n14, 200, 299))
                        {
                            n3C = PlayObject.m_DyVal[n14 - 200];
                        }
                        else if (HUtil32.RangeInDefined(n14, 300, 399))
                        {
                            n3C = PlayObject.m_nMval[n14 - 300];
                        }
                        else if (HUtil32.RangeInDefined(n14, 400, 499))
                        {
                            n3C = M2Share.g_Config.GlobaDyMval[n14 - 400];
                        }
                        else if (HUtil32.RangeInDefined(n14, 500, 599))
                        {
                            n3C = PlayObject.m_nInteger[n14 - 500];
                        }
                        else if (HUtil32.RangeInDefined(n14, 600, 699))
                        {
                            s01 = PlayObject.m_sString[n14 - 600];
                        }
                        else if (HUtil32.RangeInDefined(n14, 700, 799))
                        {
                            s01 = M2Share.g_Config.GlobalAVal[n14 - 700];
                        }
                        else if (HUtil32.RangeInDefined(n14, 800, 1199)) // G变量
                        {
                            n3C = M2Share.g_Config.GlobalVal[n14 - 700];
                        }
                        else if (HUtil32.RangeInDefined(n14, 1200, 1599)) // A变量
                        {
                            s01 = M2Share.g_Config.GlobalAVal[n14 - 1100];
                        }
                        else
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sINC);
                        }
                    }
                    else
                    {
                        n3C = HUtil32.Str_ToInt(GetLineVariableText(PlayObject, sParam2), 0);// 个人变量
                        s01 = sParam2;
                    }
                }
                else
                {
                    n3C = QuestActionInfo.nParam2;
                }
                n14 = M2Share.GetValNameNo(sParam1);
                if (n14 >= 0)
                {
                    if (HUtil32.RangeInDefined(n14, 0, 99))
                    {
                        if (n3C > 1)
                        {
                            PlayObject.m_nVal[n14] += n3C;
                        }
                        else
                        {
                            PlayObject.m_nVal[n14]++;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 100, 199))
                    {
                        if (n3C > 1)
                        {
                            M2Share.g_Config.GlobalVal[n14 - 100] += n3C;
                        }
                        else
                        {
                            M2Share.g_Config.GlobalVal[n14 - 100]++;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 200, 299))
                    {
                        if (n3C > 1)
                        {
                            PlayObject.m_DyVal[n14 - 200] += n3C;
                        }
                        else
                        {
                            PlayObject.m_DyVal[n14 - 200]++;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 300, 399))
                    {
                        if (n3C > 1)
                        {
                            PlayObject.m_nMval[n14 - 300] += n3C;
                        }
                        else
                        {
                            PlayObject.m_nMval[n14 - 300]++;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 400, 499))
                    {
                        if (n3C > 1)
                        {
                            M2Share.g_Config.GlobaDyMval[n14 - 400] += n3C;
                        }
                        else
                        {
                            M2Share.g_Config.GlobaDyMval[n14 - 400]++;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 500, 599))
                    {
                        if (n3C > 1)
                        {
                            PlayObject.m_nInteger[n14 - 500] += n3C;
                        }
                        else
                        {
                            PlayObject.m_nInteger[n14 - 500]++;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 600, 699))
                    {
                        PlayObject.m_sString[n14 - 600] = PlayObject.m_sString[n14 - 600] + s01;
                    }
                    else if (HUtil32.RangeInDefined(n14, 700, 799))
                    {
                        M2Share.g_Config.GlobalAVal[n14 - 700] = M2Share.g_Config.GlobalAVal[n14 - 700] + s01;
                    }
                    else if (HUtil32.RangeInDefined(n14, 800, 1199)) // G变量
                    {
                        if (n3C > 1)
                        {
                            M2Share.g_Config.GlobalVal[n14 - 700] += n3C;
                        }
                        else
                        {
                            M2Share.g_Config.GlobalVal[n14 - 700]++;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 1200, 1599)) // A变量
                    {
                        M2Share.g_Config.GlobalAVal[n14 - 1100] = M2Share.g_Config.GlobalAVal[n14 - 1100] + s01;
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sINC);
                    }
                }
                else
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sINC);
                    return;
                }
            }
        }

        private void DecInteger(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int n14;
            int n3C = 0;
            TDynamicVar DynamicVar;
            IList<TDynamicVar> DynamicVarList;
            string sName = string.Empty;
            string s01 = string.Empty;
            string s02 = string.Empty;
            string s03 = string.Empty;
            bool boVarFound = false;
            string sParam1 = string.Empty;
            string sParam2 = string.Empty;
            string sParam3 = string.Empty;
            const string sVarFound = "变量{0}不存在，变量类型:{1}";
            const string sVarTypeError = "变量类型错误，错误类型:{0} 当前支持类型(HUMAN、GUILD、GLOBAL)";
            var n10 = 0;
            if (HUtil32.CompareLStr(QuestActionInfo.sParam1, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam1, "(", ")", ref sParam1);
            }
            else
            {
                sParam1 = QuestActionInfo.sParam1;
            }
            if (HUtil32.CompareLStr(QuestActionInfo.sParam2, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam2, "(", ")", ref sParam2);
            }
            else
            {
                sParam2 = QuestActionInfo.sParam2;
            }
            if (HUtil32.CompareLStr(QuestActionInfo.sParam3, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam3, "(", ")", ref sParam3);
            }
            else
            {
                sParam3 = QuestActionInfo.sParam3;
            }
            if ((sParam1 == "") || (sParam2 == ""))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sDEC);
                return;
            }
            if (sParam3 != "")
            {
                if ((!HUtil32.IsVarNumber(sParam1)) && (HUtil32.IsVarNumber(sParam2)))
                {
                    n10 = 1;
                    boVarFound = false;
                    DynamicVarList = GetDynamicVarList(PlayObject, sParam2, ref sName);
                    if (DynamicVarList == null)
                    {
                        ScriptActionError(PlayObject, string.Format(sVarTypeError, sParam2), QuestActionInfo, M2Share.sDEC);
                        return;
                    }
                    if (DynamicVarList.Count > 0)
                    {
                        for (var i = 0; i < DynamicVarList.Count; i++)
                        {
                            DynamicVar = DynamicVarList[i];
                            if (string.Compare(DynamicVar.sName, sParam3, StringComparison.CurrentCultureIgnoreCase) == 0)
                            {
                                switch (DynamicVar.VarType)
                                {
                                    case TVarType.Integer:
                                        n3C = DynamicVar.nInternet;
                                        break;
                                    case TVarType.String:
                                        s01 = DynamicVar.sString;
                                        break;
                                }
                                boVarFound = true;
                                break;
                            }
                        }
                    }
                    if (!boVarFound)
                    {
                        ScriptActionError(PlayObject, string.Format(sVarFound, sParam3, sParam2), QuestActionInfo, M2Share.sDEC);
                        return;
                    }
                    n14 = M2Share.GetValNameNo(sParam1);
                    if (n14 >= 0)
                    {
                        if (HUtil32.RangeInDefined(n14, 0, 99))
                        {
                            if (n3C > 1)
                            {
                                PlayObject.m_nVal[n14] -= n3C;
                            }
                            else
                            {
                                PlayObject.m_nVal[n14] -= 1;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 100, 199))
                        {
                            if (n3C > 1)
                            {
                                M2Share.g_Config.GlobalVal[n14 - 100] -= n3C;
                            }
                            else
                            {
                                M2Share.g_Config.GlobalVal[n14 - 100] -= 1;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 200, 299))
                        {
                            if (n3C > 1)
                            {
                                PlayObject.m_DyVal[n14 - 200] -= n3C;
                            }
                            else
                            {
                                PlayObject.m_DyVal[n14 - 200] -= 1;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 300, 399))
                        {
                            if (n3C > 1)
                            {
                                PlayObject.m_nMval[n14 - 300] -= n3C;
                            }
                            else
                            {
                                PlayObject.m_nMval[n14 - 300] -= 1;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 400, 499))
                        {
                            if (n3C > 1)
                            {
                                M2Share.g_Config.GlobaDyMval[n14 - 400] -= n3C;
                            }
                            else
                            {
                                M2Share.g_Config.GlobaDyMval[n14 - 400] -= 1;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 500, 599))
                        {
                            if (n3C > 1)
                            {
                                PlayObject.m_nInteger[n14 - 500] -= n3C;
                            }
                            else
                            {
                                PlayObject.m_nInteger[n14 - 500] -= 1;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 600, 699))
                        {
                            n10 = PlayObject.m_sString[n14 - 600].IndexOf(s01, StringComparison.CurrentCultureIgnoreCase);
                            s02 = PlayObject.m_sString[n14 - 600].Substring(1, n10 - 1);
                            s03 = PlayObject.m_sString[n14 - 600].Substring(s01.Length + n10, PlayObject.m_sString[n14 - 600].Length);
                            PlayObject.m_sString[n14 - 600] = s02 + s03;
                        }
                        else if (HUtil32.RangeInDefined(n14, 700, 799))
                        {
                            n10 = M2Share.g_Config.GlobalAVal[n14 - 700].IndexOf(s01, StringComparison.CurrentCultureIgnoreCase);
                            s02 = M2Share.g_Config.GlobalAVal[n14 - 700].Substring(1, n10 - 1);
                            s03 = M2Share.g_Config.GlobalAVal[n14 - 700].Substring(s01.Length + n10, M2Share.g_Config.GlobalAVal[n14 - 700].Length);
                            M2Share.g_Config.GlobalAVal[n14 - 700] = s02 + s03;
                        }
                        else if (HUtil32.RangeInDefined(n14, 800, 1199)) // G变量
                        {
                            if (n3C > 1)
                            {
                                M2Share.g_Config.GlobalVal[n14 - 700] -= n3C;
                            }
                            else
                            {
                                M2Share.g_Config.GlobalVal[n14 - 700] -= 1;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 1200, 1599)) // A变量
                        {
                            n10 = M2Share.g_Config.GlobalAVal[n14 - 1100].IndexOf(s01, StringComparison.CurrentCultureIgnoreCase);
                            s02 = M2Share.g_Config.GlobalAVal[n14 - 1100].Substring(1, n10 - 1);
                            s03 = M2Share.g_Config.GlobalAVal[n14 - 1100].Substring(s01.Length + n10, M2Share.g_Config.GlobalAVal[n14 - 1100].Length);
                            M2Share.g_Config.GlobalAVal[n14 - 1100] = s02 + s03;
                        }
                        else
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sDEC);
                        }
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sDEC);
                        return;
                    }
                    return;
                }
                if ((HUtil32.IsVarNumber(sParam1)) && (!HUtil32.IsVarNumber(sParam2)))
                {
                    if ((sParam3 != "") && (!HUtil32.IsStringNumber(sParam3)))
                    {
                        n10 = 1;
                        n14 = M2Share.GetValNameNo(sParam3);
                        if (n14 >= 0)
                        {
                            if (HUtil32.RangeInDefined(n14, 0, 99))
                            {
                                n3C = PlayObject.m_nVal[n14];
                            }
                            else if (HUtil32.RangeInDefined(n14, 100, 199))
                            {
                                n3C = M2Share.g_Config.GlobalVal[n14 - 100];
                            }
                            else if (HUtil32.RangeInDefined(n14, 200, 299))
                            {
                                n3C = PlayObject.m_DyVal[n14 - 200];
                            }
                            else if (HUtil32.RangeInDefined(n14, 300, 399))
                            {
                                n3C = PlayObject.m_nMval[n14 - 300];
                            }
                            else if (HUtil32.RangeInDefined(n14, 400, 499))
                            {
                                n3C = M2Share.g_Config.GlobaDyMval[n14 - 400];
                            }
                            else if (HUtil32.RangeInDefined(n14, 500, 599))
                            {
                                n3C = PlayObject.m_nInteger[n14 - 500];
                            }
                            else if (HUtil32.RangeInDefined(n14, 600, 699))
                            {
                                s01 = PlayObject.m_sString[n14 - 600];
                            }
                            else if (HUtil32.RangeInDefined(n14, 700, 799))
                            {
                                s01 = M2Share.g_Config.GlobalAVal[n14 - 700];
                            }
                            else if (HUtil32.RangeInDefined(n14, 800, 1199)) // G变量
                            {
                                n3C = M2Share.g_Config.GlobalVal[n14 - 700];
                            }
                            else if (HUtil32.RangeInDefined(n14, 1200, 1599)) // A变量
                            {
                                s01 = M2Share.g_Config.GlobalAVal[n14 - 1100];
                            }
                            else
                            {
                                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sDEC);
                            }
                        }
                        else
                        {
                            s01 = sParam3;
                        }
                    }
                    else
                    {
                        n3C = QuestActionInfo.nParam3;
                    }
                    boVarFound = false;
                    DynamicVarList = GetDynamicVarList(PlayObject, sParam1, ref sName);
                    if (DynamicVarList == null)
                    {
                        ScriptActionError(PlayObject, string.Format(sVarTypeError, sParam1), QuestActionInfo, M2Share.sDEC);
                        return;
                    }
                    if (DynamicVarList.Count > 0)
                    {
                        for (var i = 0; i < DynamicVarList.Count; i++)
                        {
                            DynamicVar = DynamicVarList[i];
                            if (string.Compare(DynamicVar.sName, sParam2, StringComparison.CurrentCultureIgnoreCase) == 0)
                            {
                                switch (DynamicVar.VarType)
                                {
                                    case TVarType.Integer:
                                        if (n3C > 1)
                                        {
                                            DynamicVar.nInternet -= n3C;
                                        }
                                        else
                                        {
                                            DynamicVar.nInternet -= 1;
                                        }
                                        break;
                                    case TVarType.String:
                                        n10 = DynamicVar.sString.IndexOf(s01, StringComparison.CurrentCultureIgnoreCase);
                                        s02 = DynamicVar.sString.Substring(0, n10 - 1);
                                        s03 = DynamicVar.sString.Substring(s01.Length + n10 - 1, DynamicVar.sString.Length);
                                        DynamicVar.sString = s02 + s03;
                                        break;
                                }
                                boVarFound = true;
                                break;
                            }
                        }
                    }
                    if (!boVarFound)
                    {
                        ScriptActionError(PlayObject, string.Format(sVarFound, sParam2, sParam1), QuestActionInfo, M2Share.sDEC);
                        return;
                    }
                    return;
                }
                if (n10 == 0)
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sDEC);
                }
            }
            else
            {
                if ((sParam2 != "") && (!HUtil32.IsStringNumber(sParam2)))
                {
                    // 获取第2个变量值
                    n14 = M2Share.GetValNameNo(sParam2);
                    if (n14 >= 0)
                    {
                        if (HUtil32.RangeInDefined(n14, 0, 99))
                        {
                            n3C = PlayObject.m_nVal[n14];
                        }
                        else if (HUtil32.RangeInDefined(n14, 100, 199))
                        {
                            n3C = M2Share.g_Config.GlobalVal[n14 - 100];
                        }
                        else if (HUtil32.RangeInDefined(n14, 200, 299))
                        {
                            n3C = PlayObject.m_DyVal[n14 - 200];
                        }
                        else if (HUtil32.RangeInDefined(n14, 300, 399))
                        {
                            n3C = PlayObject.m_nMval[n14 - 300];
                        }
                        else if (HUtil32.RangeInDefined(n14, 400, 499))
                        {
                            n3C = M2Share.g_Config.GlobaDyMval[n14 - 400];
                        }
                        else if (HUtil32.RangeInDefined(n14, 500, 599))
                        {
                            n3C = PlayObject.m_nInteger[n14 - 500];
                        }
                        else if (HUtil32.RangeInDefined(n14, 600, 699))
                        {
                            s01 = PlayObject.m_sString[n14 - 600];
                        }
                        else if (HUtil32.RangeInDefined(n14, 700, 799))
                        {
                            s01 = M2Share.g_Config.GlobalAVal[n14 - 700];
                        }
                        else if (HUtil32.RangeInDefined(n14, 800, 1199)) // G变量
                        {
                            n3C = M2Share.g_Config.GlobalVal[n14 - 700];
                        }
                        else if (HUtil32.RangeInDefined(n14, 1200, 1599)) // A变量
                        {
                            s01 = M2Share.g_Config.GlobalAVal[n14 - 1100];
                        }
                        else
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sDEC);
                        }
                    }
                    else
                    {
                        n3C = HUtil32.Str_ToInt(GetLineVariableText(PlayObject, sParam2), 0);// 个人变量
                        s01 = sParam2;
                    }
                }
                else
                {
                    n3C = QuestActionInfo.nParam2;
                }
                n14 = M2Share.GetValNameNo(sParam1);
                if (n14 >= 0)
                {
                    if (HUtil32.RangeInDefined(n14, 0, 99))
                    {
                        if (n3C > 1)
                        {
                            PlayObject.m_nVal[n14] -= n3C;
                        }
                        else
                        {
                            PlayObject.m_nVal[n14] -= 1;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 100, 199))
                    {
                        if (n3C > 1)
                        {
                            M2Share.g_Config.GlobalVal[n14 - 100] -= n3C;
                        }
                        else
                        {
                            M2Share.g_Config.GlobalVal[n14 - 100] -= 1;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 200, 299))
                    {
                        if (n3C > 1)
                        {
                            PlayObject.m_DyVal[n14 - 200] -= n3C;
                        }
                        else
                        {
                            PlayObject.m_DyVal[n14 - 200] -= 1;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 300, 399))
                    {
                        if (n3C > 1)
                        {
                            PlayObject.m_nMval[n14 - 300] -= n3C;
                        }
                        else
                        {
                            PlayObject.m_nMval[n14 - 300] -= 1;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 400, 499))
                    {
                        if (n3C > 1)
                        {
                            M2Share.g_Config.GlobaDyMval[n14 - 400] -= n3C;
                        }
                        else
                        {
                            M2Share.g_Config.GlobaDyMval[n14 - 400] -= 1;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 500, 599))
                    {
                        if (n3C > 1)
                        {
                            PlayObject.m_nInteger[n14 - 500] -= n3C;
                        }
                        else
                        {
                            PlayObject.m_nInteger[n14 - 500] -= 1;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 600, 699))
                    {
                        n10 = PlayObject.m_sString[n14 - 600].IndexOf(s01);
                        s02 = PlayObject.m_sString[n14 - 600].Substring(1, n10 - 1);
                        s03 = PlayObject.m_sString[n14 - 600].Substring(s01.Length + n10, PlayObject.m_sString[n14 - 600].Length);
                        PlayObject.m_sString[n14 - 600] = s02 + s03;
                    }
                    else if (HUtil32.RangeInDefined(n14, 700, 799))
                    {
                        n10 = M2Share.g_Config.GlobalAVal[n14 - 700].IndexOf(s01);
                        s02 = M2Share.g_Config.GlobalAVal[n14 - 700].Substring(1, n10 - 1);
                        s03 = M2Share.g_Config.GlobalAVal[n14 - 700].Substring(s01.Length + n10, M2Share.g_Config.GlobalAVal[n14 - 700].Length);
                        M2Share.g_Config.GlobalAVal[n14 - 700] = s02 + s03;
                    }
                    else if (HUtil32.RangeInDefined(n14, 800, 1199)) // G变量
                    {
                        if (n3C > 1)
                        {
                            M2Share.g_Config.GlobalVal[n14 - 700] -= n3C;
                        }
                        else
                        {
                            M2Share.g_Config.GlobalVal[n14 - 700] -= 1;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 1200, 1599)) // A变量
                    {
                        n10 = M2Share.g_Config.GlobalAVal[n14 - 1100].IndexOf(s01);
                        s02 = M2Share.g_Config.GlobalAVal[n14 - 1100].Substring(1, n10 - 1);
                        s03 = M2Share.g_Config.GlobalAVal[n14 - 1100].Substring(s01.Length + n10, M2Share.g_Config.GlobalAVal[n14 - 1100].Length);
                        M2Share.g_Config.GlobalAVal[n14 - 1100] = s02 + s03;
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sDEC);
                    }
                }
                else
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sDEC);
                    return;
                }
            }
        }

        /// <summary>
        /// 变量运算 除法  格式: DIV N1 N2 N3 即N1=N2/N3
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="QuestActionInfo"></param>
        private void DivData(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var s34 = string.Empty;
            var n18 = 0;
            var n14 = HUtil32.Str_ToInt(GetLineVariableText(PlayObject, QuestActionInfo.sParam2), -1);
            if (n14 < 0)
            {
                n14 = M2Share.GetValNameNo(QuestActionInfo.sParam2);
                if (n14 >= 0)
                {
                    if (HUtil32.RangeInDefined(n14, 0, 99))
                    {
                        n18 = PlayObject.m_nVal[n14];
                    }
                    else if (HUtil32.RangeInDefined(n14, 100, 199))
                    {
                        n18 = M2Share.g_Config.GlobalVal[n14 - 100];
                    }
                    else if (HUtil32.RangeInDefined(n14, 200, 299))
                    {
                        n18 = PlayObject.m_DyVal[n14 - 200];
                    }
                    else if (HUtil32.RangeInDefined(n14, 300, 399))
                    {
                        n18 = PlayObject.m_nMval[n14 - 300];
                    }
                    else if (HUtil32.RangeInDefined(n14, 400, 499))
                    {
                        n18 = M2Share.g_Config.GlobaDyMval[n14 - 400];
                    }
                    else if (HUtil32.RangeInDefined(n14, 500, 599))
                    {
                        n18 = PlayObject.m_nInteger[n14 - 500];
                    }
                    else if (HUtil32.RangeInDefined(n14, 800, 1199))//G变量
                    {
                        n18 = M2Share.g_Config.GlobalVal[n14 - 700];
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_DIV);
                    }
                }
                else
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_DIV);
                }
            }
            else
            {
                n18 = n14;
            }
            var n1C = 0;
            n14 = HUtil32.Str_ToInt(GetLineVariableText(PlayObject, QuestActionInfo.sParam3), -1);
            if (n14 < 0)
            {
                n14 = M2Share.GetValNameNo(QuestActionInfo.sParam3);
                if (n14 >= 0)
                {
                    if (HUtil32.RangeInDefined(n14, 0, 99))
                    {
                        n1C = PlayObject.m_nVal[n14];
                    }
                    else if (HUtil32.RangeInDefined(n14, 100, 199))
                    {
                        n1C = M2Share.g_Config.GlobalVal[n14 - 100];
                    }
                    else if (HUtil32.RangeInDefined(n14, 200, 299))
                    {
                        n1C = PlayObject.m_DyVal[n14 - 200];
                    }
                    else if (HUtil32.RangeInDefined(n14, 300, 399))
                    {
                        n1C = PlayObject.m_nMval[n14 - 300];
                    }
                    else if (HUtil32.RangeInDefined(n14, 400, 499))
                    {
                        n1C = M2Share.g_Config.GlobaDyMval[n14 - 400];
                    }
                    else if (HUtil32.RangeInDefined(n14, 500, 599))
                    {
                        n1C = PlayObject.m_nInteger[n14 - 500];
                    }
                    else if (HUtil32.RangeInDefined(n14, 800, 1199))//G变量
                    {
                        n1C = M2Share.g_Config.GlobalVal[n14 - 700];
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_DIV);
                    }
                }
                else
                {
                    // ScriptActionError(PlayObject,'',QuestActionInfo,sSC_DIV);
                }
            }
            else
            {
                n1C = n14;
            }
            if (HUtil32.CompareLStr(QuestActionInfo.sParam1, "<$STR(", 6))//支持字符串变量
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam1, '(', ')', ref s34);
                n14 = M2Share.GetValNameNo(s34);
            }
            else
            {
                n14 = M2Share.GetValNameNo(QuestActionInfo.sParam1);
            }
            if (n14 >= 0)
            {
                if (HUtil32.RangeInDefined(n14, 0, 99))
                {
                    PlayObject.m_nVal[n14] = n18 / n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 100, 199))
                {
                    M2Share.g_Config.GlobalVal[n14 - 100] = n18 / n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 200, 299))
                {
                    PlayObject.m_DyVal[n14 - 200] = n18 / n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 300, 399))
                {
                    PlayObject.m_nMval[n14 - 300] = n18 / n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 400, 499))
                {
                    M2Share.g_Config.GlobaDyMval[n14 - 400] = n18 / n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 500, 599))
                {
                    PlayObject.m_nInteger[n14 - 500] = n18 / n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 800, 1199))//G变量
                {
                    M2Share.g_Config.GlobalVal[n14 - 700] = n18 / n1C;
                }
            }
        }

        /// <summary>
        /// 变量运算 乘法  格式: MUL N1 N2 N3 即N1=N2*N3
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="QuestActionInfo"></param>
        private void MulData(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var s34 = string.Empty;
            var n18 = 0;
            var n14 = HUtil32.Str_ToInt(GetLineVariableText(PlayObject, QuestActionInfo.sParam2), -1);
            if (n14 < 0)
            {
                n14 = M2Share.GetValNameNo(QuestActionInfo.sParam2);
                if (n14 >= 0)
                {
                    if (HUtil32.RangeInDefined(n14, 0, 99))
                    {
                        n18 = PlayObject.m_nVal[n14];
                    }
                    else if (HUtil32.RangeInDefined(n14, 100, 199))
                    {
                        n18 = M2Share.g_Config.GlobalVal[n14 - 100];
                    }
                    else if (HUtil32.RangeInDefined(n14, 200, 299))
                    {
                        n18 = PlayObject.m_DyVal[n14 - 200];
                    }
                    else if (HUtil32.RangeInDefined(n14, 300, 399))
                    {
                        n18 = PlayObject.m_nMval[n14 - 300];
                    }
                    else if (HUtil32.RangeInDefined(n14, 400, 499))
                    {
                        n18 = M2Share.g_Config.GlobaDyMval[n14 - 400];
                    }
                    else if (HUtil32.RangeInDefined(n14, 800, 1999))//G变量
                    {
                        n18 = M2Share.g_Config.GlobalVal[n14 - 700];
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_MUL);
                    }
                }
                else
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_MUL);
                }
            }
            else
            {
                n18 = n14;
            }
            var n1C = 0;
            n14 = HUtil32.Str_ToInt(GetLineVariableText(PlayObject, QuestActionInfo.sParam3), -1);
            if (n14 < 0)
            {
                n14 = M2Share.GetValNameNo(QuestActionInfo.sParam3);
                if (n14 >= 0)
                {
                    if (HUtil32.RangeInDefined(n14, 0, 99))
                    {
                        n1C = PlayObject.m_nVal[n14];
                    }
                    else if (HUtil32.RangeInDefined(n14, 100, 199))
                    {
                        n1C = M2Share.g_Config.GlobalVal[n14 - 100];
                    }
                    else if (HUtil32.RangeInDefined(n14, 200, 299))
                    {
                        n1C = PlayObject.m_DyVal[n14 - 200];
                    }
                    else if (HUtil32.RangeInDefined(n14, 300, 399))
                    {
                        n1C = PlayObject.m_nMval[n14 - 300];
                    }
                    else if (HUtil32.RangeInDefined(n14, 400, 499))
                    {
                        n1C = M2Share.g_Config.GlobaDyMval[n14 - 400];
                    }
                    else if (HUtil32.RangeInDefined(n14, 800, 1999))//G变量
                    {
                        n1C = M2Share.g_Config.GlobalVal[n14 - 700];
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_MUL);
                    }
                }
                else
                {
                    // ScriptActionError(PlayObject,'',QuestActionInfo,sSC_MUL;
                }
            }
            else
            {
                n1C = n14;
            }
            if (HUtil32.CompareLStr(QuestActionInfo.sParam1, "<$STR(", 6))// 支持字符串变量
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam1, '(', ')', ref s34);
                n14 = M2Share.GetValNameNo(s34);
            }
            else
            {
                n14 = M2Share.GetValNameNo(QuestActionInfo.sParam1);// 取第一个变量,并传值给n18
            }
            if (n14 >= 0)
            {
                if (HUtil32.RangeInDefined(n14, 0, 99))
                {
                    PlayObject.m_nVal[n14] = n18 * n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 100, 199))
                {
                    M2Share.g_Config.GlobalVal[n14 - 100] = n18 * n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 200, 299))
                {
                    PlayObject.m_DyVal[n14 - 200] = n18 * n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 300, 399))
                {
                    PlayObject.m_nMval[n14 - 300] = n18 * n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 400, 499))
                {
                    M2Share.g_Config.GlobaDyMval[n14 - 400] = n18 * n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 800, 1999))//G变量
                {
                    M2Share.g_Config.GlobalVal[n14 - 700] = n18 * n1C;
                }
            }
        }

        /// <summary>
        /// 变量运算 百分比  格式: PERCENT N1 N2 N3 即N1=(N2/N3)*100
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="QuestActionInfo"></param>
        private void PercentData(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var s34 = string.Empty;
            var n18 = 0;
            var n14 = HUtil32.Str_ToInt(GetLineVariableText(PlayObject, QuestActionInfo.sParam2), -1);
            if (n14 < 0)
            {
                n14 = M2Share.GetValNameNo(QuestActionInfo.sParam2); // 取第一个变量,并传值给n18
                if (n14 >= 0)
                {
                    if (HUtil32.RangeInDefined(n14, 0, 99))
                    {
                        n18 = PlayObject.m_nVal[n14];
                    }
                    else if (HUtil32.RangeInDefined(n14, 100, 199))
                    {
                        n18 = M2Share.g_Config.GlobalVal[n14 - 100];
                    }
                    else if (HUtil32.RangeInDefined(n14, 200, 299))
                    {
                        n18 = PlayObject.m_DyVal[n14 - 200];
                    }
                    else if (HUtil32.RangeInDefined(n14, 300, 399))
                    {
                        n18 = PlayObject.m_nMval[n14 - 300];
                    }
                    else if (HUtil32.RangeInDefined(n14, 400, 499))
                    {
                        n18 = M2Share.g_Config.GlobaDyMval[n14 - 400];
                    }
                    else if (HUtil32.RangeInDefined(n14, 800, 1999))//G变量
                    {
                        n18 = M2Share.g_Config.GlobalVal[n14 - 700];
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_PERCENT);
                    }
                }
                else
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_PERCENT);
                }
            }
            else
            {
                n18 = n14;
            }
            var n1C = 0;
            n14 = HUtil32.Str_ToInt(GetLineVariableText(PlayObject, QuestActionInfo.sParam3), -1);
            if (n14 < 0)
            {
                n14 = M2Share.GetValNameNo(QuestActionInfo.sParam3); // 取第一个变量,并传值给n1C
                if (n14 >= 0)
                {
                    if (HUtil32.RangeInDefined(n14, 0, 99))
                    {
                        n1C = PlayObject.m_nVal[n14];
                    }
                    else if (HUtil32.RangeInDefined(n14, 100, 199))
                    {
                        n1C = M2Share.g_Config.GlobalVal[n14 - 100];
                    }
                    else if (HUtil32.RangeInDefined(n14, 200, 299))
                    {
                        n1C = PlayObject.m_DyVal[n14 - 200];
                    }
                    else if (HUtil32.RangeInDefined(n14, 300, 399))
                    {
                        n1C = PlayObject.m_nMval[n14 - 300];
                    }
                    else if (HUtil32.RangeInDefined(n14, 400, 499))
                    {
                        n1C = M2Share.g_Config.GlobaDyMval[n14 - 400];
                    }
                    else if (HUtil32.RangeInDefined(n14, 800, 1999))//G变量
                    {
                        n1C = M2Share.g_Config.GlobalVal[n14 - 700];
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_PERCENT);
                    }
                }
                else
                {
                    // ScriptActionError(PlayObject,'',QuestActionInfo,sSC_PERCENT);
                }
            }
            else
            {
                n1C = n14;
            }
            if (HUtil32.CompareLStr(QuestActionInfo.sParam1, "<$STR(", 6))// 支持字符串变量
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam1, '(', ')', ref s34);
                n14 = M2Share.GetValNameNo(s34);
            }
            else
            {
                n14 = M2Share.GetValNameNo(QuestActionInfo.sParam1);
            }
            if (n14 >= 0)
            {
                if (HUtil32.RangeInDefined(n14, 0, 99))
                {
                    PlayObject.m_nVal[n14] = n18 * n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 100, 199))
                {
                    M2Share.g_Config.GlobalVal[n14 - 100] = n18 / n1C * 100;
                }
                else if (HUtil32.RangeInDefined(n14, 200, 299))
                {
                    PlayObject.m_DyVal[n14 - 200] = n18 / n1C * 100;
                }
                else if (HUtil32.RangeInDefined(n14, 300, 399))
                {
                    PlayObject.m_nMval[n14 - 300] = n18 / n1C * 100;
                }
                else if (HUtil32.RangeInDefined(n14, 400, 499))
                {
                    M2Share.g_Config.GlobaDyMval[n14 - 400] = n18 / n1C * 100;
                }
                else if (HUtil32.RangeInDefined(n14, 500, 599))
                {
                    PlayObject.m_nInteger[n14 - 500] = n18 / n1C * 100;
                }
                else if (HUtil32.RangeInDefined(n14, 600, 699))
                {
                    PlayObject.m_sString[n14 - 600] = $"{n18 / n1C * 100}%";
                }
                else if (HUtil32.RangeInDefined(n14, 700, 799))
                {
                    M2Share.g_Config.GlobalAVal[n14 - 700] = $"{n18 / n1C * 100}%";
                }
                else if (HUtil32.RangeInDefined(n14, 800, 1199))//G变量
                {
                    M2Share.g_Config.GlobalVal[n14 - 700] = n18 / n1C * 100;
                }
                else if (HUtil32.RangeInDefined(n14, 1200, 1599))//G变量
                {
                    M2Share.g_Config.GlobalAVal[n14 - 1100] = $"{n18 / n1C * 100}%";
                }
            }
        }

        private void SumData(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var n18 = 0;
            var n14 = 0;
            var s34 = string.Empty;
            var s44 = string.Empty;
            var s48 = string.Empty;
            if (HUtil32.CompareLStr(QuestActionInfo.sParam1, "<$STR(", 6)) //  SUM 支持字符串变量
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam1, "(", ")", ref s34);
                n14 = M2Share.GetValNameNo(s34);
            }
            else
            {
                n14 = M2Share.GetValNameNo(QuestActionInfo.sParam1);
            }
            if (n14 >= 0)
            {
                if (HUtil32.RangeInDefined(n14, 0, 99))
                {
                    n18 = PlayObject.m_nVal[n14];
                }
                else if (HUtil32.RangeInDefined(n14, 100, 199))
                {
                    n18 = M2Share.g_Config.GlobalVal[n14 - 100];
                }
                else if (HUtil32.RangeInDefined(n14, 200, 299))
                {
                    n18 = PlayObject.m_DyVal[n14 - 200];
                }
                else if (HUtil32.RangeInDefined(n14, 300, 399))
                {
                    n18 = PlayObject.m_nMval[n14 - 300];
                }
                else if (HUtil32.RangeInDefined(n14, 400, 499))
                {
                    n18 = M2Share.g_Config.GlobaDyMval[n14 - 400];
                }
                else if (HUtil32.RangeInDefined(n14, 500, 599))
                {
                    n18 = PlayObject.m_nInteger[n14 - 500];
                }
                else if (HUtil32.RangeInDefined(n14, 600, 699))
                {
                    s44 = PlayObject.m_sString[n14 - 600];
                }
                else if (HUtil32.RangeInDefined(n14, 700, 799))
                {
                    s44 = M2Share.g_Config.GlobalAVal[n14 - 700];
                }
                else if (HUtil32.RangeInDefined(n14, 800, 1199))
                {
                    n18 = M2Share.g_Config.GlobalVal[n14 - 700];//G变量
                }
                else if (HUtil32.RangeInDefined(n14, 1200, 1599))
                {
                    s44 = M2Share.g_Config.GlobalAVal[n14 - 1100];//A变量
                }
                else
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSUM);
                }
            }
            else
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSUM);
            }
            var n1C = 0;
            if (HUtil32.CompareLStr(QuestActionInfo.sParam2, "<$STR(", 6)) //SUM 支持字符串变量
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam2, "(", ")", ref s34);
                n14 = M2Share.GetValNameNo(s34);
            }
            else
            {
                n14 = M2Share.GetValNameNo(QuestActionInfo.sParam2);
            }
            if (n14 >= 0)
            {
                if (HUtil32.RangeInDefined(n14, 0, 99))
                {
                    n1C = PlayObject.m_nVal[n14];
                }
                else if (HUtil32.RangeInDefined(n14, 100, 199))
                {
                    n1C = M2Share.g_Config.GlobalVal[n14 - 100];
                }
                else if (HUtil32.RangeInDefined(n14, 200, 299))
                {
                    n1C = PlayObject.m_DyVal[n14 - 200];
                }
                else if (HUtil32.RangeInDefined(n14, 300, 399))
                {
                    n1C = PlayObject.m_nMval[n14 - 300];
                }
                else if (HUtil32.RangeInDefined(n14, 400, 499))
                {
                    n1C = M2Share.g_Config.GlobaDyMval[n14 - 400];
                }
                else if (HUtil32.RangeInDefined(n14, 500, 599))
                {
                    n1C = PlayObject.m_nInteger[n14 - 500];
                }
                else if (HUtil32.RangeInDefined(n14, 600, 699))
                {
                    s48 = PlayObject.m_sString[n14 - 600];
                }
                else if (HUtil32.RangeInDefined(n14, 700, 799))
                {
                    s48 = M2Share.g_Config.GlobalAVal[n14 - 700];
                }
                else if (HUtil32.RangeInDefined(n14, 800, 1199))
                {
                    n1C = M2Share.g_Config.GlobalVal[n14 - 700];//G变量
                }
                else if (HUtil32.RangeInDefined(n14, 1200, 1599))
                {
                    s48 = M2Share.g_Config.GlobalAVal[n14 - 1100];//A变量
                }
                else
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSUM);
                }
            }
            if (HUtil32.CompareLStr(QuestActionInfo.sParam1, "<$STR(", 6)) // SUM 支持字符串变量
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam1, "(", ")", ref s34);
                n14 = M2Share.GetValNameNo(s34);
            }
            else
            {
                n14 = M2Share.GetValNameNo(QuestActionInfo.sParam1);
            }
            if (n14 >= 0)
            {
                if (HUtil32.RangeInDefined(n14, 0, 99))
                {
                    PlayObject.m_nVal[n14] = n18 + n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 100, 199))
                {
                    M2Share.g_Config.GlobalVal[n14 - 100] = n18 + n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 200, 299))
                {
                    PlayObject.m_DyVal[n14 - 200] = n18 + n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 300, 399))
                {
                    PlayObject.m_nMval[n14 - 300] = n18 + n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 400, 499))
                {
                    M2Share.g_Config.GlobaDyMval[n14 - 400] = n18 + n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 500, 599))
                {
                    PlayObject.m_nInteger[n14 - 500] = n18 + n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 600, 699))
                {
                    PlayObject.m_sString[n14 - 600] = s44 + s48;
                }
                else if (HUtil32.RangeInDefined(n14, 700, 799))
                {
                    M2Share.g_Config.GlobalAVal[n14 - 700] = s44 + s48;
                }
                else if (HUtil32.RangeInDefined(n14, 800, 1199))
                {
                    M2Share.g_Config.GlobalVal[n14 - 700] = n18 + n1C;//G变量
                }
                else if (HUtil32.RangeInDefined(n14, 1200, 1599))
                {
                    M2Share.g_Config.GlobalAVal[n14 - 1100] = s44 + s48;//A变量
                }
            }
        }

        private bool GetMovDataHumanInfoValue(TPlayObject PlayObject, string sVariable, ref string sValue, ref int nValue, ref int nDataType)
        {
            string s10 = string.Empty;
            string sVarValue2 = string.Empty;
            int nSecond;
            TDynamicVar DynamicVar;
            short wHour;
            short wMinute;
            short wSecond;
            long IntDays;
            sValue = "";
            nValue = -1;
            nDataType = -1;
            var result = false;
            if (sVariable == "")
            {
                return result;
            }
            var sMsg = sVariable;
            HUtil32.ArrestStringEx(sMsg, "<", ">", ref s10);
            if (s10 == "")
            {
                return result;
            }
            sVariable = s10;
            //全局信息
            switch (sVariable)
            {
                case "$SERVERNAME":
                    sValue = M2Share.g_Config.sServerName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$SERVERIP":
                    sValue = M2Share.g_Config.sServerIPaddr;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$WEBSITE":
                    sValue = M2Share.g_Config.sWebSite;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BBSSITE":
                    sValue = M2Share.g_Config.sBbsSite;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$CLIENTDOWNLOAD":
                    sValue = M2Share.g_Config.sClientDownload;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$QQ":
                    sValue = M2Share.g_Config.sQQ;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$PHONE":
                    sValue = M2Share.g_Config.sPhone;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT0":
                    sValue = M2Share.g_Config.sBankAccount0;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT1":
                    sValue = M2Share.g_Config.sBankAccount1;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT2":
                    sValue = M2Share.g_Config.sBankAccount2;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT3":
                    sValue = M2Share.g_Config.sBankAccount3;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT4":
                    sValue = M2Share.g_Config.sBankAccount4;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT5":
                    sValue = M2Share.g_Config.sBankAccount5;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT6":
                    sValue = M2Share.g_Config.sBankAccount6;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT7":
                    sValue = M2Share.g_Config.sBankAccount7;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT8":
                    sValue = M2Share.g_Config.sBankAccount8;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT9":
                    sValue = M2Share.g_Config.sBankAccount9;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$GAMEGOLDNAME":
                    sValue = M2Share.g_Config.sGameGoldName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$GAMEPOINTNAME":
                    sValue = M2Share.g_Config.sGamePointName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$USERCOUNT":
                    sValue = (M2Share.UserEngine.PlayObjectCount).ToString();
                    nDataType = 0;
                    result = true;
                    return result;
                case "$MACRUNTIME":
                    // (24 * 60 * 60 * 1000)
                    sValue = (HUtil32.GetTickCount() / 86400000).ToString();
                    nDataType = 0;
                    result = true;
                    return result;
                case "$SERVERRUNTIME":
                    nSecond = (HUtil32.GetTickCount() - M2Share.g_dwStartTick) / 1000;
                    //wHour = nSecond / 3600;
                    //wMinute = (nSecond / 60) % 60;
                    //wSecond = nSecond % 60;
                    //sValue = string.Format("%d:%d:%d", new short[] { wHour, wMinute, wSecond });
                    nDataType = 0;
                    result = true;
                    return result;
                case "$DATETIME":
                    //sValue = FormatDateTime("dddddd,dddd,hh:mm:nn", DateTime.Now);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$DATE":
                    //sValue = FormatDateTime("dddddd", DateTime.Now);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$CASTLEWARDATE":
                    {
                        // 申请攻城的日期
                        if (m_Castle == null)
                        {
                            m_Castle = M2Share.CastleManager.GetCastle(0);
                        }
                        if (m_Castle != null)
                        {
                            if (!m_Castle.m_boUnderWar)
                            {
                                sValue = m_Castle.GetWarDate();
                                if (sValue != "")
                                {
                                    sMsg = ReplaceVariableText(sMsg, "<$CASTLEWARDATE>", sValue);
                                }
                                else
                                {
                                    sMsg = "暂时没有行会攻城！！！\\ \\<返回/@main>";
                                }
                            }
                            else
                            {
                                sMsg = "现正在攻城中！！！\\ \\<返回/@main>";
                            }
                        }
                        else
                        {
                            sValue = "????";
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
                case "$USERNAME":// 个人信息
                    sValue = PlayObject.m_sCharName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$KILLER":// 杀人者变量 
                    {
                        if (PlayObject.m_boDeath && (PlayObject.m_LastHiter != null))
                        {
                            if ((PlayObject.m_LastHiter.m_btRaceServer == Grobal2.RC_PLAYOBJECT))
                            {
                                sValue = PlayObject.m_LastHiter.m_sCharName;
                            }
                        }
                        else
                        {
                            sValue = "未知";
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
                case "$MONKILLER":// 杀人的怪物变量 
                    {
                        if (PlayObject.m_boDeath && (PlayObject.m_LastHiter != null))
                        {
                            if ((PlayObject.m_LastHiter.m_btRaceServer != Grobal2.RC_PLAYOBJECT))
                            {
                                sValue = PlayObject.m_LastHiter.m_sCharName;
                            }
                        }
                        else
                        {
                            sValue = "未知";
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
                case "$USERALLNAME":// 全名 
                    sValue = PlayObject.GetShowName();
                    nDataType = 0;
                    result = true;
                    return result;
                case "$SFNAME":// 师傅名 
                    sValue = PlayObject.m_sMasterName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$STATSERVERTIME":// 显示M2启动时间
                                       // sValue = FormatDateTime("dddddd,dddd,hh:mm:nn", M2Share.g_dwRunStartTick);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$RUNDATETIME":// 开区间隔时间 显示为XX小时。
                                    // IntDays = MinutesBetween(DateTime.Now, M2Share.g_dwDiyStartTick);
                                    //sValue = (IntDays / 60).ToString();
                    nDataType = 0;
                    result = true;
                    return result;
                case "$RANDOMNO":// 随机值变量
                    nValue = (new System.Random(Int32.MaxValue)).Next();
                    nDataType = 1;
                    result = true;
                    return result;
                case "$USERID":// 登录账号
                    sValue = PlayObject.m_sUserID;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$IPADDR":// 登录IP
                    sValue = PlayObject.m_sIPaddr;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$X": // 人物X坐标
                    nValue = PlayObject.m_nCurrX;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$Y": // 人物Y坐标
                    nValue = PlayObject.m_nCurrY;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAP":
                    sValue = PlayObject.m_PEnvir.sMapName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$GUILDNAME":
                    {
                        if (PlayObject.m_MyGuild != null)
                        {
                            sValue = PlayObject.m_MyGuild.sGuildName;
                        }
                        else
                        {
                            sValue = "无";
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
                case "$RANKNAME":
                    sValue = PlayObject.m_sGuildRankName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$RELEVEL":
                    nValue = PlayObject.m_btReLevel;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$LEVEL":
                    nValue = PlayObject.m_Abil.Level;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$HP":
                    nValue = PlayObject.m_WAbil.HP;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXHP":
                    nValue = PlayObject.m_WAbil.MaxHP;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MP":
                    nValue = PlayObject.m_WAbil.MP;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXMP":
                    nValue = PlayObject.m_WAbil.MaxMP;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$AC":
                    nValue = HUtil32.LoWord(PlayObject.m_WAbil.AC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXAC":
                    nValue = HUtil32.HiWord(PlayObject.m_WAbil.AC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAC":
                    nValue = HUtil32.LoWord(PlayObject.m_WAbil.MAC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXMAC":
                    nValue = HUtil32.HiWord(PlayObject.m_WAbil.MAC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$DC":
                    nValue = HUtil32.LoWord(PlayObject.m_WAbil.DC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXDC":
                    nValue = HUtil32.HiWord(PlayObject.m_WAbil.DC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MC":
                    nValue = HUtil32.LoWord(PlayObject.m_WAbil.MC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXMC":
                    nValue = HUtil32.HiWord(PlayObject.m_WAbil.MC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$SC":
                    nValue = HUtil32.LoWord(PlayObject.m_WAbil.SC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXSC":
                    nValue = HUtil32.HiWord(PlayObject.m_WAbil.SC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$EXP":
                    nValue = PlayObject.m_Abil.Exp;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXEXP":
                    nValue = PlayObject.m_Abil.MaxExp;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$PKPOINT":
                    nValue = PlayObject.m_nPkPoint;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$CREDITPOINT":
                    nValue = PlayObject.m_btCreditPoint;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$HW":
                    nValue = PlayObject.m_WAbil.HandWeight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXHW":
                    nValue = PlayObject.m_WAbil.MaxHandWeight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$BW":
                    nValue = PlayObject.m_WAbil.Weight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXBW":
                    nValue = PlayObject.m_WAbil.MaxWeight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$WW":
                    nValue = PlayObject.m_WAbil.WearWeight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXWW":
                    nValue = PlayObject.m_WAbil.MaxWearWeight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$GOLDCOUNT":
                    nValue = PlayObject.m_nGold;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$GOLDCOUNTX":
                    nValue = PlayObject.m_nGoldMax;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$GAMEGOLD":
                    nValue = PlayObject.m_nGameGold;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$GAMEPOINT":
                    nValue = PlayObject.m_nGamePoint;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$HUNGER":
                    nValue = PlayObject.GetMyStatus();
                    nDataType = 1;
                    result = true;
                    return result;
                case "$LOGINTIME":
                    sValue = (PlayObject.m_dLogonTime).ToString();
                    nDataType = 0;
                    result = true;
                    return result;
                case "$LOGINLONG":
                    nValue = (HUtil32.GetTickCount() - PlayObject.m_dwLogonTick) / 60000;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$DRESS":
                    sValue = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_DRESS].wIndex);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$WEAPON":
                    sValue = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_WEAPON].wIndex);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$RIGHTHAND":
                    sValue = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_RIGHTHAND].wIndex);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$HELMET":
                    sValue = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_HELMET].wIndex);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$NECKLACE":
                    sValue = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_NECKLACE].wIndex);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$RING_R":
                    sValue = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_RINGR].wIndex);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$RING_L":
                    sValue = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_RINGL].wIndex);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$ARMRING_R":
                    sValue = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_ARMRINGR].wIndex);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$ARMRING_L":
                    sValue = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_ARMRINGL].wIndex);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BUJUK":
                    sValue = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_BUJUK].wIndex);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BELT":
                    sValue = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_BELT].wIndex);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BOOTS":
                    sValue = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_BOOTS].wIndex);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$CHARM":
                    sValue = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_CHARM].wIndex);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$IPLOCAL":
                    sValue = PlayObject.m_sIPLocal;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$GUILDBUILDPOINT":
                    {
                        if (PlayObject.m_MyGuild != null)
                        {
                            //nValue = PlayObject.m_MyGuild.nBuildPoint;
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
                case "$GUILDAURAEPOINT":
                    {
                        if (PlayObject.m_MyGuild != null)
                        {
                            nValue = PlayObject.m_MyGuild.nAurae;
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
                case "$GUILDSTABILITYPOINT":
                    {
                        if (PlayObject.m_MyGuild != null)
                        {
                            nValue = PlayObject.m_MyGuild.nStability;
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
                case "$GUILDFLOURISHPOINT":
                    {
                        if (PlayObject.m_MyGuild != null)
                        {
                            nValue = PlayObject.m_MyGuild.nFlourishing;
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
            }
            if (HUtil32.CompareLStr(sVariable, "$HUMAN", 6))//  人物变量
            {
                HUtil32.ArrestStringEx(sVariable, "(", ")", ref sVarValue2);
                if (PlayObject.m_DynamicVarList.Count > 0)
                {
                    for (var i = 0; i < PlayObject.m_DynamicVarList.Count; i++)
                    {
                        DynamicVar = PlayObject.m_DynamicVarList[i];
                        if (string.Compare(DynamicVar.sName, sVarValue2, StringComparison.CurrentCultureIgnoreCase) == 0)
                        {
                            switch (DynamicVar.VarType)
                            {
                                case TVarType.Integer:
                                    nValue = DynamicVar.nInternet;
                                    nDataType = 1;
                                    result = true;
                                    return result;
                                case TVarType.String:
                                    sValue = DynamicVar.sString;
                                    nDataType = 0;
                                    result = true;
                                    return result;
                            }
                            break;
                        }
                    }
                }
            }
            if (HUtil32.CompareLStr(sVariable, "$ACCOUNT", 8)) //  人物变量
            {
                HUtil32.ArrestStringEx(sVariable, "(", ")", ref sVarValue2);
                if (PlayObject.m_DynamicVarList.Count > 0)
                {
                    for (var i = 0; i < PlayObject.m_DynamicVarList.Count; i++)
                    {
                        DynamicVar = PlayObject.m_DynamicVarList[i];
                        if (string.Compare(DynamicVar.sName, sVarValue2, StringComparison.CurrentCultureIgnoreCase) == 0)
                        {
                            switch (DynamicVar.VarType)
                            {
                                case TVarType.Integer:
                                    nValue = DynamicVar.nInternet;
                                    nDataType = 1;
                                    result = true;
                                    return result;
                                case TVarType.String:
                                    sValue = DynamicVar.sString;
                                    nDataType = 0;
                                    result = true;
                                    return result;
                            }
                            break;
                        }
                    }
                }
            }
            return result;
        }

        private bool SetMovDataValNameValue(TPlayObject PlayObject, string sVarName, string sValue, int nValue, int nDataType)
        {
            bool result = false;
            var n100 = M2Share.GetValNameNo(sVarName);
            if (n100 >= 0)
            {
                switch (nDataType)
                {
                    case 1:
                        if (HUtil32.RangeInDefined(n100, 0, 99))
                        {   
                            PlayObject.m_nVal[n100] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 100, 199))
                        {
                            M2Share.g_Config.GlobalVal[n100 - 100] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 200, 299))
                        {
                            PlayObject.m_DyVal[n100 - 200] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 300, 399))
                        {
                            PlayObject.m_nMval[n100 - 300] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 400, 499))
                        {
                            M2Share.g_Config.GlobaDyMval[n100 - 400] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 500, 599))
                        {
                            PlayObject.m_nInteger[n100 - 500] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 600, 699))
                        {
                            PlayObject.m_sString[n100 - 600] = (nValue).ToString();
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 700, 799))
                        {
                            M2Share.g_Config.GlobalAVal[n100 - 700] = (nValue).ToString();
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 800, 1199)) // G变量
                        {
                            M2Share.g_Config.GlobalVal[n100 - 700] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 1200, 1599)) // A变量
                        {
                            M2Share.g_Config.GlobalAVal[n100 - 1100] = (nValue).ToString();
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                    case 0:
                        if (HUtil32.RangeInDefined(n100, 0, 99))
                        {
                            PlayObject.m_nVal[n100] = HUtil32.Str_ToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 100, 199))
                        {
                            M2Share.g_Config.GlobalVal[n100 - 100] = HUtil32.Str_ToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 200, 299))
                        {
                            PlayObject.m_DyVal[n100 - 200] = HUtil32.Str_ToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 300, 399))
                        {
                            PlayObject.m_nMval[n100 - 300] = HUtil32.Str_ToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 400, 499))
                        {
                            M2Share.g_Config.GlobaDyMval[n100 - 400] = HUtil32.Str_ToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 500, 599))
                        {
                            PlayObject.m_nInteger[n100 - 500] = HUtil32.Str_ToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 600, 699))
                        {
                            PlayObject.m_sString[n100 - 600] = sValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 700, 799))
                        {
                            M2Share.g_Config.GlobalAVal[n100 - 700] = sValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 800, 1199)) // G变量
                        {
                            M2Share.g_Config.GlobalVal[n100 - 700] = HUtil32.Str_ToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 1200, 1599)) // A变量
                        {
                            M2Share.g_Config.GlobalAVal[n100 - 1100] = sValue;
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                    case 3:
                        if (HUtil32.RangeInDefined(n100, 0, 99))
                        {
                            PlayObject.m_nVal[n100] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 100, 199))
                        {
                            M2Share.g_Config.GlobalVal[n100 - 100] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 200, 299))
                        {
                            PlayObject.m_DyVal[n100 - 200] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 300, 399))
                        {
                            PlayObject.m_nMval[n100 - 300] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 400, 499))
                        {
                            M2Share.g_Config.GlobaDyMval[n100 - 400] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 500, 599))
                        {
                            PlayObject.m_nInteger[n100 - 500] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 600, 699))
                        {
                            PlayObject.m_sString[n100 - 600] = sValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 700, 799))
                        {
                            M2Share.g_Config.GlobalAVal[n100 - 700] = sValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 800, 1199)) // G变量
                        {
                            M2Share.g_Config.GlobalVal[n100 - 700] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 1200, 1599)) // A变量
                        {
                            M2Share.g_Config.GlobalAVal[n100 - 1100] = sValue;
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        private bool GetMovDataValNameValue(TPlayObject PlayObject, string sVarName, ref string sValue, ref int nValue, ref int nDataType)
        {
            bool result = false;
            nValue = -1;
            sValue = "";
            nDataType = -1;
            int n100 = M2Share.GetValNameNo(sVarName);
            if (n100 >= 0)
            {
                if (HUtil32.RangeInDefined(n100, 0, 99))
                {
                    nValue = PlayObject.m_nVal[n100];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 100, 199))
                {
                    nValue = M2Share.g_Config.GlobalVal[n100 - 100];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 200, 299))
                {
                    nValue = PlayObject.m_DyVal[n100 - 200];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 300, 399))
                {
                    nValue = PlayObject.m_nMval[n100 - 300];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 400, 499))
                {
                    nValue = M2Share.g_Config.GlobaDyMval[n100 - 400];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 500, 599))
                {
                    nValue = PlayObject.m_nInteger[n100 - 500];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 600, 699))
                {
                    sValue = PlayObject.m_sString[n100 - 600];
                    nDataType = 0;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 700, 799))
                {
                    sValue = M2Share.g_Config.GlobalAVal[n100 - 700];
                    nDataType = 0;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 800, 1199))//G变量
                {
                    nValue = M2Share.g_Config.GlobalVal[n100 - 700];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 1200, 1599))//A变量
                {
                    sValue = M2Share.g_Config.GlobalAVal[n100 - 1100];
                    nDataType = 0;
                    result = true;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        private bool GetMovDataDynamicVarValue(TPlayObject PlayObject, string sVarType, string sVarName, ref string sValue, ref int nValue, ref int nDataType)
        {
            bool result;
            TDynamicVar DynamicVar;
            string sName = string.Empty;
            bool boVarFound = false;
            sValue = "";
            nValue = -1;
            nDataType = -1;
            IList<TDynamicVar> DynamicVarList = GetDynamicVarList(PlayObject, sVarType, ref sName);
            if (DynamicVarList == null)
            {
                result = false;
                return result;
            }
            if (DynamicVarList.Count > 0)
            {
                for (var i = 0; i < DynamicVarList.Count; i++)
                {
                    DynamicVar = DynamicVarList[i];
                    if (string.Compare(DynamicVar.sName, sVarName, StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        switch (DynamicVar.VarType)
                        {
                            case TVarType.Integer:
                                nValue = DynamicVar.nInternet;
                                nDataType = 1;
                                break;
                            case TVarType.String:
                                sValue = DynamicVar.sString;
                                nDataType = 0;
                                break;
                        }
                        boVarFound = true;
                        break;
                    }
                }
            }
            if (!boVarFound)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        private bool SetMovDataDynamicVarValue(TPlayObject PlayObject, string sVarType, string sVarName, string sValue, int nValue, int nDataType)
        {
            bool result;
            TDynamicVar DynamicVar;
            string sName = string.Empty;
            bool boVarFound = false;
            IList<TDynamicVar> DynamicVarList = GetDynamicVarList(PlayObject, sVarType, ref sName);
            if (DynamicVarList == null)
            {
                result = false;
                return result;
            }
            if (DynamicVarList.Count > 0)
            {
                for (var i = 0; i < DynamicVarList.Count; i++)
                {
                    DynamicVar = DynamicVarList[i];
                    if (string.Compare(DynamicVar.sName, sVarName, StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        if (nDataType == 1)
                        {
                            switch (DynamicVar.VarType)
                            {
                                case TVarType.Integer:
                                    DynamicVar.nInternet = nValue;
                                    boVarFound = true;
                                    break;
                            }
                        }
                        else
                        {
                            switch (DynamicVar.VarType)
                            {
                                case TVarType.String:
                                    DynamicVar.sString = sValue;
                                    boVarFound = true;
                                    break;
                            }
                        }
                    }
                }
            }
            if (!boVarFound)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        private int GetMovDataType(TQuestActionInfo QuestActionInfo)
        {
            string sParam1 = string.Empty;
            string sParam2 = string.Empty;
            string sParam3 = string.Empty;
            int result = -1;
            if (HUtil32.CompareLStr(QuestActionInfo.sParam1, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam1, "(", ")", ref sParam1);
            }
            else
            {
                sParam1 = QuestActionInfo.sParam1;
            }
            if (HUtil32.CompareLStr(QuestActionInfo.sParam2, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam2, "(", ")", ref sParam2);
            }
            else
            {
                sParam2 = QuestActionInfo.sParam2;
            }
            if (HUtil32.CompareLStr(QuestActionInfo.sParam3, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam3, "(", ")", ref sParam3);
            }
            else
            {
                sParam3 = QuestActionInfo.sParam3;
            }
            if (HUtil32.IsVarNumber(sParam1))
            {
                if ((sParam3 != "") && (sParam3[0] == '<') && (sParam3[sParam3.Length - 1] == '>'))
                {
                    result = 0;
                }
                else if ((sParam3 != "") && (M2Share.GetValNameNo(sParam3) >= 0))
                {
                    result = 1;
                }
                else if ((sParam3 != "") && HUtil32.IsStringNumber(sParam3))
                {
                    result = 2;
                }
                else
                {
                    result = 3;
                }
                return result;
            }
            int n01 = M2Share.GetValNameNo(sParam1);
            if (n01 >= 0)
            {
                if ((sParam2 != "") && (sParam2[0] == '<') && (sParam2[sParam2.Length - 1] == '>'))
                {
                    result = 4;
                }
                else if ((sParam2 != "") && (M2Share.GetValNameNo(sParam2) >= 0))
                {
                    result = 5;
                }
                else if ((sParam2 != "") && HUtil32.IsVarNumber(sParam2))
                {
                    result = 6;
                }
                else
                {
                    result = 7;
                }
                return result;
            }
            return result;
        }

    }
}