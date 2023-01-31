using GameSvr.Player;
using GameSvr.Script;
using SystemModule;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.Npc
{
    public partial class NormNpc
    {
        /// <summary>
        /// 取随机值赋给变量
        /// 拓展可以随机参数2到参数3之间的数
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="QuestActionInfo"></param>
        private void MovrData(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var s34 = string.Empty;
            int n14;
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
                        PlayObject.MNVal[n14] = QuestActionInfo.nParam2 + M2Share.RandomNumber.Random(QuestActionInfo.nParam3 - QuestActionInfo.nParam2);
                    }
                    else
                    {
                        PlayObject.MNVal[n14] = M2Share.RandomNumber.Random(QuestActionInfo.nParam2);
                    }
                }
                else if (HUtil32.RangeInDefined(n14, 100, 119))
                {
                    if (QuestActionInfo.nParam3 > QuestActionInfo.nParam2)
                    {
                        M2Share.Config.GlobalVal[n14 - 100] = QuestActionInfo.nParam2 + M2Share.RandomNumber.Random(QuestActionInfo.nParam3 - QuestActionInfo.nParam2);
                    }
                    else
                    {
                        M2Share.Config.GlobalVal[n14 - 100] = M2Share.RandomNumber.Random(QuestActionInfo.nParam2);
                    }
                }
                else if (HUtil32.RangeInDefined(n14, 200, 299))
                {
                    if (QuestActionInfo.nParam3 > QuestActionInfo.nParam2)
                    {
                        PlayObject.MDyVal[n14 - 200] = QuestActionInfo.nParam2 + M2Share.RandomNumber.Random(QuestActionInfo.nParam3 - QuestActionInfo.nParam2);
                    }
                    else
                    {
                        PlayObject.MDyVal[n14 - 200] = M2Share.RandomNumber.Random(QuestActionInfo.nParam2);
                    }
                }
                else if (HUtil32.RangeInDefined(n14, 300, 399))
                {
                    if (QuestActionInfo.nParam3 > QuestActionInfo.nParam2)
                    {
                        PlayObject.MNMval[n14 - 300] = QuestActionInfo.nParam2 + M2Share.RandomNumber.Random(QuestActionInfo.nParam3 - QuestActionInfo.nParam2);
                    }
                    else
                    {
                        PlayObject.MNMval[n14 - 300] = M2Share.RandomNumber.Random(QuestActionInfo.nParam2);
                    }
                }
                else if (HUtil32.RangeInDefined(n14, 400, 499))
                {
                    if (QuestActionInfo.nParam3 > QuestActionInfo.nParam2)
                    {
                        M2Share.Config.GlobaDyMval[n14 - 400] = QuestActionInfo.nParam2 + M2Share.RandomNumber.Random(QuestActionInfo.nParam3 - QuestActionInfo.nParam2);
                    }
                    else
                    {
                        M2Share.Config.GlobaDyMval[n14 - 400] = M2Share.RandomNumber.Random(QuestActionInfo.nParam2);
                    }
                }
                else if (HUtil32.RangeInDefined(n14, 500, 599))
                {
                    if (QuestActionInfo.nParam3 > QuestActionInfo.nParam2)
                    {
                        PlayObject.MNInteger[n14 - 500] = QuestActionInfo.nParam2 + M2Share.RandomNumber.Random(QuestActionInfo.nParam3 - QuestActionInfo.nParam2);
                    }
                    else
                    {
                        PlayObject.MNInteger[n14 - 500] = M2Share.RandomNumber.Random(QuestActionInfo.nParam2);
                    }
                }
                else if (HUtil32.RangeInDefined(n14, 800, 1199))
                {
                    if (QuestActionInfo.nParam3 > QuestActionInfo.nParam2)
                    {
                        M2Share.Config.GlobalVal[n14 - 700] = QuestActionInfo.nParam2 + M2Share.RandomNumber.Random(QuestActionInfo.nParam3 - QuestActionInfo.nParam2);
                    }
                    else
                    {
                        M2Share.Config.GlobalVal[n14 - 700] = M2Share.RandomNumber.Random(QuestActionInfo.nParam2);
                    }
                }
                else
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sMOVR);
                }
            }
            else
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sMOVR);
            }
        }

        private void MovData(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
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
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sMOV);
                return;
            }
            switch (GetMovDataType(QuestActionInfo))
            {
                case 0:
                    if (GetMovDataHumanInfoValue(PlayObject, sParam3, ref sValue, ref nValue, ref nDataType))
                    {
                        if (!SetMovDataDynamicVarValue(PlayObject, sParam1, sParam2, sValue, nValue, nDataType))
                        {
                            ScriptActionError(PlayObject, string.Format(sVarFound, sParam1, sParam2), QuestActionInfo, ScriptConst.sMOV);
                        }
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sMOV);
                    }
                    break;
                case 1:
                    if (GetMovDataValNameValue(PlayObject, sParam3, ref sValue, ref nValue, ref nDataType))
                    {
                        if (!SetMovDataDynamicVarValue(PlayObject, sParam1, sParam2, sValue, nValue, nDataType))
                        {
                            ScriptActionError(PlayObject, string.Format(sVarFound, sParam1, sParam2), QuestActionInfo, ScriptConst.sMOV);
                        }
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sMOV);
                    }
                    break;
                case 2:
                    if (!SetMovDataDynamicVarValue(PlayObject, sParam1, sParam2, QuestActionInfo.sParam3, QuestActionInfo.nParam3, 1))
                    {
                        ScriptActionError(PlayObject, string.Format(sVarFound, sParam1, sParam2), QuestActionInfo, ScriptConst.sMOV);
                    }
                    break;
                case 3:
                    if (!SetMovDataDynamicVarValue(PlayObject, sParam1, sParam2, QuestActionInfo.sParam3, QuestActionInfo.nParam3, 0))
                    {
                        ScriptActionError(PlayObject, string.Format(sVarFound, sParam1, sParam2), QuestActionInfo, ScriptConst.sMOV);
                    }
                    break;
                case 4:
                    if (GetMovDataHumanInfoValue(PlayObject, sParam2, ref sValue, ref nValue, ref nDataType))
                    {
                        if (!SetMovDataValNameValue(PlayObject, sParam1, sValue, nValue, nDataType))
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sMOV);
                        }
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sMOV);
                    }
                    break;
                case 5:
                    if (GetMovDataValNameValue(PlayObject, sParam2, ref sValue, ref nValue, ref nDataType))
                    {
                        if (!SetMovDataValNameValue(PlayObject, sParam1, sValue, nValue, nDataType))
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sMOV);
                        }
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sMOV);
                    }
                    break;
                case 6:
                    if (GetMovDataDynamicVarValue(PlayObject, sParam2, sParam3, ref sValue, ref nValue, ref nDataType))
                    {
                        if (!SetMovDataValNameValue(PlayObject, sParam1, sValue, nValue, nDataType))
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sMOV);
                        }
                    }
                    else
                    {
                        ScriptActionError(PlayObject, string.Format(sVarFound, sParam2, sParam3), QuestActionInfo, ScriptConst.sMOV);
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
                                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sMOV);
                                return;
                            }
                        }
                        else
                        {
                            if (!SetMovDataValNameValue(PlayObject, sParam1, QuestActionInfo.sParam2, QuestActionInfo.nParam2, nDataType))
                            {
                                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sMOV);
                                return;
                            }
                        }
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sMOV);
                    }
                    break;
                default:
                    ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sMOV);
                    break;
            }
        }

        private void IncInteger(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            int n14;
            int n3C = 0;
            string s01 = string.Empty;
            DynamicVar DynamicVar;
            Dictionary<string, DynamicVar> DynamicVarList;
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
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sINC);
                return;
            }
            if (sParam3 != "")
            {
                if ((!HUtil32.IsVarNumber(sParam1)) && HUtil32.IsVarNumber(sParam2))
                {
                    boVarFound = false;
                    DynamicVarList = GetDynamicVarList(PlayObject, sParam2, ref sName);
                    if (DynamicVarList == null)
                    {
                        ScriptActionError(PlayObject, string.Format(sVarTypeError, sParam2), QuestActionInfo, ScriptConst.sINC);
                        return;
                    }
                    if (DynamicVarList.TryGetValue(sParam3, out DynamicVar))
                    {
                        switch (DynamicVar.VarType)
                        {
                            case VarType.Integer:
                                n3C = DynamicVar.nInternet;
                                break;
                            case VarType.String:
                                s01 = DynamicVar.sString;
                                break;
                        }
                        boVarFound = true;
                    }
                    if (!boVarFound)
                    {
                        ScriptActionError(PlayObject, string.Format(sVarFound, sParam3, sParam2), QuestActionInfo, ScriptConst.sINC);
                        return;
                    }
                    n14 = M2Share.GetValNameNo(sParam1);
                    if (n14 >= 0)
                    {
                        if (HUtil32.RangeInDefined(n14, 0, 99))
                        {
                            if (n3C > 1)
                            {
                                PlayObject.MNVal[n14] += n3C;
                            }
                            else
                            {
                                PlayObject.MNVal[n14]++;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 100, 199))
                        {
                            if (n3C > 1)
                            {
                                M2Share.Config.GlobalVal[n14 - 100] += n3C;
                            }
                            else
                            {
                                M2Share.Config.GlobalVal[n14 - 100]++;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 200, 299))
                        {
                            if (n3C > 1)
                            {
                                PlayObject.MDyVal[n14 - 200] += n3C;
                            }
                            else
                            {
                                PlayObject.MDyVal[n14 - 200]++;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 300, 399))
                        {
                            if (n3C > 1)
                            {
                                PlayObject.MNMval[n14 - 300] += n3C;
                            }
                            else
                            {
                                PlayObject.MNMval[n14 - 300]++;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 400, 499))
                        {
                            if (n3C > 1)
                            {
                                M2Share.Config.GlobaDyMval[n14 - 400] += n3C;
                            }
                            else
                            {
                                M2Share.Config.GlobaDyMval[n14 - 400]++;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 500, 599))
                        {
                            if (n3C > 1)
                            {
                                PlayObject.MNInteger[n14 - 500] += n3C;
                            }
                            else
                            {
                                PlayObject.MNInteger[n14 - 500]++;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 600, 699))
                        {
                            PlayObject.MSString[n14 - 600] = PlayObject.MSString[n14 - 600] + s01;
                        }
                        else if (HUtil32.RangeInDefined(n14, 700, 799))
                        {
                            M2Share.Config.GlobalAVal[n14 - 700] = M2Share.Config.GlobalAVal[n14 - 700] + s01;
                        }
                        else if (HUtil32.RangeInDefined(n14, 800, 1199)) // G变量
                        {
                            if (n3C > 1)
                            {
                                M2Share.Config.GlobalVal[n14 - 700] += n3C;
                            }
                            else
                            {
                                M2Share.Config.GlobalVal[n14 - 700]++;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 1200, 1599)) // A变量
                        {
                            M2Share.Config.GlobalAVal[n14 - 1100] = M2Share.Config.GlobalAVal[n14 - 1100] + s01;
                        }
                        else
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sINC);
                        }
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sINC);
                        return;
                    }
                    return;
                }
                if (HUtil32.IsVarNumber(sParam1) && (!HUtil32.IsVarNumber(sParam2)))
                {
                    if ((sParam3 != "") && (!HUtil32.IsStringNumber(sParam3)))
                    {
                        n14 = M2Share.GetValNameNo(sParam3);
                        if (n14 >= 0)
                        {
                            if (HUtil32.RangeInDefined(n14, 0, 99))
                            {
                                n3C = PlayObject.MNVal[n14];
                            }
                            else if (HUtil32.RangeInDefined(n14, 100, 199))
                            {
                                n3C = M2Share.Config.GlobalVal[n14 - 100];
                            }
                            else if (HUtil32.RangeInDefined(n14, 200, 299))
                            {
                                n3C = PlayObject.MDyVal[n14 - 200];
                            }
                            else if (HUtil32.RangeInDefined(n14, 300, 399))
                            {
                                n3C = PlayObject.MNMval[n14 - 300];
                            }
                            else if (HUtil32.RangeInDefined(n14, 400, 499))
                            {
                                n3C = M2Share.Config.GlobaDyMval[n14 - 400];
                            }
                            else if (HUtil32.RangeInDefined(n14, 500, 599))
                            {
                                n3C = PlayObject.MNInteger[n14 - 500];
                            }
                            else if (HUtil32.RangeInDefined(n14, 600, 699))
                            {
                                s01 = PlayObject.MSString[n14 - 600];
                            }
                            else if (HUtil32.RangeInDefined(n14, 700, 799))
                            {
                                s01 = M2Share.Config.GlobalAVal[n14 - 700];
                            }
                            else if (HUtil32.RangeInDefined(n14, 800, 1199)) // G变量
                            {
                                n3C = M2Share.Config.GlobalVal[n14 - 700];
                            }
                            else if (HUtil32.RangeInDefined(n14, 1200, 1599)) // A变量
                            {
                                s01 = M2Share.Config.GlobalAVal[n14 - 1100];
                            }
                            else
                            {
                                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sINC);
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
                        ScriptActionError(PlayObject, string.Format(sVarTypeError, sParam1), QuestActionInfo, ScriptConst.sINC);
                        return;
                    }
                    if (DynamicVarList.TryGetValue(sParam2, out DynamicVar))
                    {
                        switch (DynamicVar.VarType)
                        {
                            case VarType.Integer:
                                if (n3C > 1)
                                {
                                    DynamicVar.nInternet += n3C;
                                }
                                else
                                {
                                    DynamicVar.nInternet++;
                                }
                                break;
                            case VarType.String:
                                DynamicVar.sString = DynamicVar.sString + s01;
                                break;
                        }
                        boVarFound = true;
                    }
                    if (!boVarFound)
                    {
                        ScriptActionError(PlayObject, string.Format(sVarFound, sParam2, sParam1), QuestActionInfo, ScriptConst.sINC);
                        return;
                    }
                    return;
                }
                if (n10 == 0)
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sINC);
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
                            n3C = PlayObject.MNVal[n14];
                        }
                        else if (HUtil32.RangeInDefined(n14, 100, 199))
                        {
                            n3C = M2Share.Config.GlobalVal[n14 - 100];
                        }
                        else if (HUtil32.RangeInDefined(n14, 200, 299))
                        {
                            n3C = PlayObject.MDyVal[n14 - 200];
                        }
                        else if (HUtil32.RangeInDefined(n14, 300, 399))
                        {
                            n3C = PlayObject.MNMval[n14 - 300];
                        }
                        else if (HUtil32.RangeInDefined(n14, 400, 499))
                        {
                            n3C = M2Share.Config.GlobaDyMval[n14 - 400];
                        }
                        else if (HUtil32.RangeInDefined(n14, 500, 599))
                        {
                            n3C = PlayObject.MNInteger[n14 - 500];
                        }
                        else if (HUtil32.RangeInDefined(n14, 600, 699))
                        {
                            s01 = PlayObject.MSString[n14 - 600];
                        }
                        else if (HUtil32.RangeInDefined(n14, 700, 799))
                        {
                            s01 = M2Share.Config.GlobalAVal[n14 - 700];
                        }
                        else if (HUtil32.RangeInDefined(n14, 800, 1199)) // G变量
                        {
                            n3C = M2Share.Config.GlobalVal[n14 - 700];
                        }
                        else if (HUtil32.RangeInDefined(n14, 1200, 1599)) // A变量
                        {
                            s01 = M2Share.Config.GlobalAVal[n14 - 1100];
                        }
                        else
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sINC);
                        }
                    }
                    else
                    {
                        n3C = HUtil32.StrToInt(GetLineVariableText(PlayObject, sParam2), 0);// 个人变量
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
                            PlayObject.MNVal[n14] += n3C;
                        }
                        else
                        {
                            PlayObject.MNVal[n14]++;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 100, 199))
                    {
                        if (n3C > 1)
                        {
                            M2Share.Config.GlobalVal[n14 - 100] += n3C;
                        }
                        else
                        {
                            M2Share.Config.GlobalVal[n14 - 100]++;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 200, 299))
                    {
                        if (n3C > 1)
                        {
                            PlayObject.MDyVal[n14 - 200] += n3C;
                        }
                        else
                        {
                            PlayObject.MDyVal[n14 - 200]++;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 300, 399))
                    {
                        if (n3C > 1)
                        {
                            PlayObject.MNMval[n14 - 300] += n3C;
                        }
                        else
                        {
                            PlayObject.MNMval[n14 - 300]++;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 400, 499))
                    {
                        if (n3C > 1)
                        {
                            M2Share.Config.GlobaDyMval[n14 - 400] += n3C;
                        }
                        else
                        {
                            M2Share.Config.GlobaDyMval[n14 - 400]++;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 500, 599))
                    {
                        if (n3C > 1)
                        {
                            PlayObject.MNInteger[n14 - 500] += n3C;
                        }
                        else
                        {
                            PlayObject.MNInteger[n14 - 500]++;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 600, 699))
                    {
                        PlayObject.MSString[n14 - 600] = PlayObject.MSString[n14 - 600] + s01;
                    }
                    else if (HUtil32.RangeInDefined(n14, 700, 799))
                    {
                        M2Share.Config.GlobalAVal[n14 - 700] = M2Share.Config.GlobalAVal[n14 - 700] + s01;
                    }
                    else if (HUtil32.RangeInDefined(n14, 800, 1199)) // G变量
                    {
                        if (n3C > 1)
                        {
                            M2Share.Config.GlobalVal[n14 - 700] += n3C;
                        }
                        else
                        {
                            M2Share.Config.GlobalVal[n14 - 700]++;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 1200, 1599)) // A变量
                    {
                        M2Share.Config.GlobalAVal[n14 - 1100] = M2Share.Config.GlobalAVal[n14 - 1100] + s01;
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sINC);
                    }
                }
                else
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sINC);
                    return;
                }
            }
        }

        private void DecInteger(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            int n14;
            int n3C = 0;
            DynamicVar DynamicVar;
            Dictionary<string, DynamicVar> DynamicVarList;
            string sName = string.Empty;
            string s01 = string.Empty;
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
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sDEC);
                return;
            }
            string s02;
            string s03;
            if (sParam3 != "")
            {
                bool boVarFound;
                if ((!HUtil32.IsVarNumber(sParam1)) && HUtil32.IsVarNumber(sParam2))
                {
                    boVarFound = false;
                    DynamicVarList = GetDynamicVarList(PlayObject, sParam2, ref sName);
                    if (DynamicVarList == null)
                    {
                        ScriptActionError(PlayObject, string.Format(sVarTypeError, sParam2), QuestActionInfo, ScriptConst.sDEC);
                        return;
                    }
                    if (DynamicVarList.TryGetValue(sParam3, out DynamicVar))
                    {
                        switch (DynamicVar.VarType)
                        {
                            case VarType.Integer:
                                n3C = DynamicVar.nInternet;
                                break;
                            case VarType.String:
                                s01 = DynamicVar.sString;
                                break;
                        }
                        boVarFound = true;
                    }
                    if (!boVarFound)
                    {
                        ScriptActionError(PlayObject, string.Format(sVarFound, sParam3, sParam2), QuestActionInfo, ScriptConst.sDEC);
                        return;
                    }
                    n14 = M2Share.GetValNameNo(sParam1);
                    if (n14 >= 0)
                    {
                        if (HUtil32.RangeInDefined(n14, 0, 99))
                        {
                            if (n3C > 1)
                            {
                                PlayObject.MNVal[n14] -= n3C;
                            }
                            else
                            {
                                PlayObject.MNVal[n14] -= 1;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 100, 199))
                        {
                            if (n3C > 1)
                            {
                                M2Share.Config.GlobalVal[n14 - 100] -= n3C;
                            }
                            else
                            {
                                M2Share.Config.GlobalVal[n14 - 100] -= 1;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 200, 299))
                        {
                            if (n3C > 1)
                            {
                                PlayObject.MDyVal[n14 - 200] -= n3C;
                            }
                            else
                            {
                                PlayObject.MDyVal[n14 - 200] -= 1;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 300, 399))
                        {
                            if (n3C > 1)
                            {
                                PlayObject.MNMval[n14 - 300] -= n3C;
                            }
                            else
                            {
                                PlayObject.MNMval[n14 - 300] -= 1;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 400, 499))
                        {
                            if (n3C > 1)
                            {
                                M2Share.Config.GlobaDyMval[n14 - 400] -= n3C;
                            }
                            else
                            {
                                M2Share.Config.GlobaDyMval[n14 - 400] -= 1;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 500, 599))
                        {
                            if (n3C > 1)
                            {
                                PlayObject.MNInteger[n14 - 500] -= n3C;
                            }
                            else
                            {
                                PlayObject.MNInteger[n14 - 500] -= 1;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 600, 699))
                        {
                            n10 = PlayObject.MSString[n14 - 600].AsSpan().IndexOf(s01, StringComparison.CurrentCultureIgnoreCase);
                            s02 = PlayObject.MSString[n14 - 600].Substring(1, n10 - 1);
                            s03 = PlayObject.MSString[n14 - 600].Substring(s01.Length + n10, PlayObject.MSString[n14 - 600].Length);
                            PlayObject.MSString[n14 - 600] = s02 + s03;
                        }
                        else if (HUtil32.RangeInDefined(n14, 700, 799))
                        {
                            n10 = M2Share.Config.GlobalAVal[n14 - 700].AsSpan().IndexOf(s01, StringComparison.CurrentCultureIgnoreCase);
                            s02 = M2Share.Config.GlobalAVal[n14 - 700].Substring(1, n10 - 1);
                            s03 = M2Share.Config.GlobalAVal[n14 - 700].Substring(s01.Length + n10, M2Share.Config.GlobalAVal[n14 - 700].Length);
                            M2Share.Config.GlobalAVal[n14 - 700] = s02 + s03;
                        }
                        else if (HUtil32.RangeInDefined(n14, 800, 1199)) // G变量
                        {
                            if (n3C > 1)
                            {
                                M2Share.Config.GlobalVal[n14 - 700] -= n3C;
                            }
                            else
                            {
                                M2Share.Config.GlobalVal[n14 - 700] -= 1;
                            }
                        }
                        else if (HUtil32.RangeInDefined(n14, 1200, 1599)) // A变量
                        {
                            n10 = M2Share.Config.GlobalAVal[n14 - 1100].AsSpan().IndexOf(s01, StringComparison.CurrentCultureIgnoreCase);
                            s02 = M2Share.Config.GlobalAVal[n14 - 1100].Substring(1, n10 - 1);
                            s03 = M2Share.Config.GlobalAVal[n14 - 1100].Substring(s01.Length + n10, M2Share.Config.GlobalAVal[n14 - 1100].Length);
                            M2Share.Config.GlobalAVal[n14 - 1100] = s02 + s03;
                        }
                        else
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sDEC);
                        }
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sDEC);
                        return;
                    }
                    return;
                }
                if (HUtil32.IsVarNumber(sParam1) && (!HUtil32.IsVarNumber(sParam2)))
                {
                    if ((sParam3 != "") && (!HUtil32.IsStringNumber(sParam3)))
                    {
                        n14 = M2Share.GetValNameNo(sParam3);
                        if (n14 >= 0)
                        {
                            if (HUtil32.RangeInDefined(n14, 0, 99))
                            {
                                n3C = PlayObject.MNVal[n14];
                            }
                            else if (HUtil32.RangeInDefined(n14, 100, 199))
                            {
                                n3C = M2Share.Config.GlobalVal[n14 - 100];
                            }
                            else if (HUtil32.RangeInDefined(n14, 200, 299))
                            {
                                n3C = PlayObject.MDyVal[n14 - 200];
                            }
                            else if (HUtil32.RangeInDefined(n14, 300, 399))
                            {
                                n3C = PlayObject.MNMval[n14 - 300];
                            }
                            else if (HUtil32.RangeInDefined(n14, 400, 499))
                            {
                                n3C = M2Share.Config.GlobaDyMval[n14 - 400];
                            }
                            else if (HUtil32.RangeInDefined(n14, 500, 599))
                            {
                                n3C = PlayObject.MNInteger[n14 - 500];
                            }
                            else if (HUtil32.RangeInDefined(n14, 600, 699))
                            {
                                s01 = PlayObject.MSString[n14 - 600];
                            }
                            else if (HUtil32.RangeInDefined(n14, 700, 799))
                            {
                                s01 = M2Share.Config.GlobalAVal[n14 - 700];
                            }
                            else if (HUtil32.RangeInDefined(n14, 800, 1199)) // G变量
                            {
                                n3C = M2Share.Config.GlobalVal[n14 - 700];
                            }
                            else if (HUtil32.RangeInDefined(n14, 1200, 1599)) // A变量
                            {
                                s01 = M2Share.Config.GlobalAVal[n14 - 1100];
                            }
                            else
                            {
                                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sDEC);
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
                        ScriptActionError(PlayObject, string.Format(sVarTypeError, sParam1), QuestActionInfo, ScriptConst.sDEC);
                        return;
                    }
                    if (DynamicVarList.TryGetValue(sParam2, out DynamicVar))
                    {
                        switch (DynamicVar.VarType)
                        {
                            case VarType.Integer:
                                if (n3C > 1)
                                {
                                    DynamicVar.nInternet -= n3C;
                                }
                                else
                                {
                                    DynamicVar.nInternet -= 1;
                                }
                                break;
                            case VarType.String:
                                n10 = DynamicVar.sString.AsSpan().IndexOf(s01, StringComparison.CurrentCultureIgnoreCase);
                                s02 = DynamicVar.sString[..(n10 - 1)];
                                s03 = DynamicVar.sString.Substring(s01.Length + n10 - 1, DynamicVar.sString.Length);
                                DynamicVar.sString = s02 + s03;
                                break;
                        }
                        boVarFound = true;
                    }
                    if (!boVarFound)
                    {
                        ScriptActionError(PlayObject, string.Format(sVarFound, sParam2, sParam1), QuestActionInfo, ScriptConst.sDEC);
                        return;
                    }
                    return;
                }
                if (n10 == 0)
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sDEC);
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
                            n3C = PlayObject.MNVal[n14];
                        }
                        else if (HUtil32.RangeInDefined(n14, 100, 199))
                        {
                            n3C = M2Share.Config.GlobalVal[n14 - 100];
                        }
                        else if (HUtil32.RangeInDefined(n14, 200, 299))
                        {
                            n3C = PlayObject.MDyVal[n14 - 200];
                        }
                        else if (HUtil32.RangeInDefined(n14, 300, 399))
                        {
                            n3C = PlayObject.MNMval[n14 - 300];
                        }
                        else if (HUtil32.RangeInDefined(n14, 400, 499))
                        {
                            n3C = M2Share.Config.GlobaDyMval[n14 - 400];
                        }
                        else if (HUtil32.RangeInDefined(n14, 500, 599))
                        {
                            n3C = PlayObject.MNInteger[n14 - 500];
                        }
                        else if (HUtil32.RangeInDefined(n14, 600, 699))
                        {
                            s01 = PlayObject.MSString[n14 - 600];
                        }
                        else if (HUtil32.RangeInDefined(n14, 700, 799))
                        {
                            s01 = M2Share.Config.GlobalAVal[n14 - 700];
                        }
                        else if (HUtil32.RangeInDefined(n14, 800, 1199)) // G变量
                        {
                            n3C = M2Share.Config.GlobalVal[n14 - 700];
                        }
                        else if (HUtil32.RangeInDefined(n14, 1200, 1599)) // A变量
                        {
                            s01 = M2Share.Config.GlobalAVal[n14 - 1100];
                        }
                        else
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sDEC);
                        }
                    }
                    else
                    {
                        n3C = HUtil32.StrToInt(GetLineVariableText(PlayObject, sParam2), 0);// 个人变量
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
                            PlayObject.MNVal[n14] -= n3C;
                        }
                        else
                        {
                            PlayObject.MNVal[n14] -= 1;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 100, 199))
                    {
                        if (n3C > 1)
                        {
                            M2Share.Config.GlobalVal[n14 - 100] -= n3C;
                        }
                        else
                        {
                            M2Share.Config.GlobalVal[n14 - 100] -= 1;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 200, 299))
                    {
                        if (n3C > 1)
                        {
                            PlayObject.MDyVal[n14 - 200] -= n3C;
                        }
                        else
                        {
                            PlayObject.MDyVal[n14 - 200] -= 1;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 300, 399))
                    {
                        if (n3C > 1)
                        {
                            PlayObject.MNMval[n14 - 300] -= n3C;
                        }
                        else
                        {
                            PlayObject.MNMval[n14 - 300] -= 1;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 400, 499))
                    {
                        if (n3C > 1)
                        {
                            M2Share.Config.GlobaDyMval[n14 - 400] -= n3C;
                        }
                        else
                        {
                            M2Share.Config.GlobaDyMval[n14 - 400] -= 1;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 500, 599))
                    {
                        if (n3C > 1)
                        {
                            PlayObject.MNInteger[n14 - 500] -= n3C;
                        }
                        else
                        {
                            PlayObject.MNInteger[n14 - 500] -= 1;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 600, 699))
                    {
                        n10 = PlayObject.MSString[n14 - 600].AsSpan().IndexOf(s01, StringComparison.OrdinalIgnoreCase);
                        s02 = PlayObject.MSString[n14 - 600].Substring(1, n10 - 1);
                        s03 = PlayObject.MSString[n14 - 600].Substring(s01.Length + n10, PlayObject.MSString[n14 - 600].Length);
                        PlayObject.MSString[n14 - 600] = s02 + s03;
                    }
                    else if (HUtil32.RangeInDefined(n14, 700, 799))
                    {
                        n10 = M2Share.Config.GlobalAVal[n14 - 700].AsSpan().IndexOf(s01, StringComparison.OrdinalIgnoreCase);
                        s02 = M2Share.Config.GlobalAVal[n14 - 700].Substring(1, n10 - 1);
                        s03 = M2Share.Config.GlobalAVal[n14 - 700].Substring(s01.Length + n10, M2Share.Config.GlobalAVal[n14 - 700].Length);
                        M2Share.Config.GlobalAVal[n14 - 700] = s02 + s03;
                    }
                    else if (HUtil32.RangeInDefined(n14, 800, 1199)) // G变量
                    {
                        if (n3C > 1)
                        {
                            M2Share.Config.GlobalVal[n14 - 700] -= n3C;
                        }
                        else
                        {
                            M2Share.Config.GlobalVal[n14 - 700] -= 1;
                        }
                    }
                    else if (HUtil32.RangeInDefined(n14, 1200, 1599)) // A变量
                    {
                        n10 = M2Share.Config.GlobalAVal[n14 - 1100].AsSpan().IndexOf(s01, StringComparison.OrdinalIgnoreCase);
                        s02 = M2Share.Config.GlobalAVal[n14 - 1100].Substring(1, n10 - 1);
                        s03 = M2Share.Config.GlobalAVal[n14 - 1100].Substring(s01.Length + n10, M2Share.Config.GlobalAVal[n14 - 1100].Length);
                        M2Share.Config.GlobalAVal[n14 - 1100] = s02 + s03;
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sDEC);
                    }
                }
                else
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sDEC);
                    return;
                }
            }
        }

        /// <summary>
        /// 变量运算 除法  格式: DIV N1 N2 N3 即N1=N2/N3
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="QuestActionInfo"></param>
        private void DivData(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var s34 = string.Empty;
            var n18 = 0;
            var n14 = HUtil32.StrToInt(GetLineVariableText(PlayObject, QuestActionInfo.sParam2), -1);
            if (n14 < 0)
            {
                n14 = M2Share.GetValNameNo(QuestActionInfo.sParam2);
                if (n14 >= 0)
                {
                    if (HUtil32.RangeInDefined(n14, 0, 99))
                    {
                        n18 = PlayObject.MNVal[n14];
                    }
                    else if (HUtil32.RangeInDefined(n14, 100, 199))
                    {
                        n18 = M2Share.Config.GlobalVal[n14 - 100];
                    }
                    else if (HUtil32.RangeInDefined(n14, 200, 299))
                    {
                        n18 = PlayObject.MDyVal[n14 - 200];
                    }
                    else if (HUtil32.RangeInDefined(n14, 300, 399))
                    {
                        n18 = PlayObject.MNMval[n14 - 300];
                    }
                    else if (HUtil32.RangeInDefined(n14, 400, 499))
                    {
                        n18 = M2Share.Config.GlobaDyMval[n14 - 400];
                    }
                    else if (HUtil32.RangeInDefined(n14, 500, 599))
                    {
                        n18 = PlayObject.MNInteger[n14 - 500];
                    }
                    else if (HUtil32.RangeInDefined(n14, 800, 1199))//G变量
                    {
                        n18 = M2Share.Config.GlobalVal[n14 - 700];
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_DIV);
                    }
                }
                else
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_DIV);
                }
            }
            else
            {
                n18 = n14;
            }
            var n1C = 0;
            n14 = HUtil32.StrToInt(GetLineVariableText(PlayObject, QuestActionInfo.sParam3), -1);
            if (n14 < 0)
            {
                n14 = M2Share.GetValNameNo(QuestActionInfo.sParam3);
                if (n14 >= 0)
                {
                    if (HUtil32.RangeInDefined(n14, 0, 99))
                    {
                        n1C = PlayObject.MNVal[n14];
                    }
                    else if (HUtil32.RangeInDefined(n14, 100, 199))
                    {
                        n1C = M2Share.Config.GlobalVal[n14 - 100];
                    }
                    else if (HUtil32.RangeInDefined(n14, 200, 299))
                    {
                        n1C = PlayObject.MDyVal[n14 - 200];
                    }
                    else if (HUtil32.RangeInDefined(n14, 300, 399))
                    {
                        n1C = PlayObject.MNMval[n14 - 300];
                    }
                    else if (HUtil32.RangeInDefined(n14, 400, 499))
                    {
                        n1C = M2Share.Config.GlobaDyMval[n14 - 400];
                    }
                    else if (HUtil32.RangeInDefined(n14, 500, 599))
                    {
                        n1C = PlayObject.MNInteger[n14 - 500];
                    }
                    else if (HUtil32.RangeInDefined(n14, 800, 1199))//G变量
                    {
                        n1C = M2Share.Config.GlobalVal[n14 - 700];
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_DIV);
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
                    PlayObject.MNVal[n14] = n18 / n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 100, 199))
                {
                    M2Share.Config.GlobalVal[n14 - 100] = n18 / n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 200, 299))
                {
                    PlayObject.MDyVal[n14 - 200] = n18 / n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 300, 399))
                {
                    PlayObject.MNMval[n14 - 300] = n18 / n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 400, 499))
                {
                    M2Share.Config.GlobaDyMval[n14 - 400] = n18 / n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 500, 599))
                {
                    PlayObject.MNInteger[n14 - 500] = n18 / n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 800, 1199))//G变量
                {
                    M2Share.Config.GlobalVal[n14 - 700] = n18 / n1C;
                }
            }
        }

        /// <summary>
        /// 变量运算 乘法  格式: MUL N1 N2 N3 即N1=N2*N3
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="QuestActionInfo"></param>
        private void MulData(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var s34 = string.Empty;
            var n18 = 0;
            var n14 = HUtil32.StrToInt(GetLineVariableText(PlayObject, QuestActionInfo.sParam2), -1);
            if (n14 < 0)
            {
                n14 = M2Share.GetValNameNo(QuestActionInfo.sParam2);
                if (n14 >= 0)
                {
                    if (HUtil32.RangeInDefined(n14, 0, 99))
                    {
                        n18 = PlayObject.MNVal[n14];
                    }
                    else if (HUtil32.RangeInDefined(n14, 100, 199))
                    {
                        n18 = M2Share.Config.GlobalVal[n14 - 100];
                    }
                    else if (HUtil32.RangeInDefined(n14, 200, 299))
                    {
                        n18 = PlayObject.MDyVal[n14 - 200];
                    }
                    else if (HUtil32.RangeInDefined(n14, 300, 399))
                    {
                        n18 = PlayObject.MNMval[n14 - 300];
                    }
                    else if (HUtil32.RangeInDefined(n14, 400, 499))
                    {
                        n18 = M2Share.Config.GlobaDyMval[n14 - 400];
                    }
                    else if (HUtil32.RangeInDefined(n14, 500, 599))
                    {
                        n18 = PlayObject.MNInteger[n14 - 500];
                    }
                    else if (HUtil32.RangeInDefined(n14, 600, 699))
                    {
                        n18 = HUtil32.StrToInt(PlayObject.MSString[n14 - 600], 1);
                    }
                    else if (HUtil32.RangeInDefined(n14, 700, 799))
                    {
                        n18 = HUtil32.StrToInt(M2Share.Config.GlobalAVal[n14 - 700], 1);
                    }
                    else if (HUtil32.RangeInDefined(n14, 800, 1199))//A变量
                    {
                        n18 = M2Share.Config.GlobalVal[n14 - 700];
                    }
                    else if (HUtil32.RangeInDefined(n14, 1200, 1599))//G变量
                    {
                        n18 = HUtil32.StrToInt(M2Share.Config.GlobalAVal[n14 - 1100], 1);
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_MUL);
                    }
                }
                else
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_MUL);
                }
            }
            else
            {
                n18 = n14;
            }
            var n1C = 0;
            n14 = HUtil32.StrToInt(GetLineVariableText(PlayObject, QuestActionInfo.sParam3), -1);
            if (n14 < 0)
            {
                n14 = M2Share.GetValNameNo(QuestActionInfo.sParam3);
                if (n14 >= 0)
                {
                    if (HUtil32.RangeInDefined(n14, 0, 99))
                    {
                        n1C = PlayObject.MNVal[n14];
                    }
                    else if (HUtil32.RangeInDefined(n14, 100, 199))
                    {
                        n1C = M2Share.Config.GlobalVal[n14 - 100];
                    }
                    else if (HUtil32.RangeInDefined(n14, 200, 299))
                    {
                        n1C = PlayObject.MDyVal[n14 - 200];
                    }
                    else if (HUtil32.RangeInDefined(n14, 300, 399))
                    {
                        n1C = PlayObject.MNMval[n14 - 300];
                    }
                    else if (HUtil32.RangeInDefined(n14, 400, 499))
                    {
                        n1C = M2Share.Config.GlobaDyMval[n14 - 400];
                    }
                    else if (HUtil32.RangeInDefined(n14, 500, 599))
                    {
                        n1C = PlayObject.MNInteger[n14 - 500];
                    }
                    else if (HUtil32.RangeInDefined(n14, 600, 699))
                    {
                        n1C = HUtil32.StrToInt(PlayObject.MSString[n14 - 600], 1);
                    }
                    else if (HUtil32.RangeInDefined(n14, 700, 799))
                    {
                        n1C = HUtil32.StrToInt(M2Share.Config.GlobalAVal[n14 - 700], 1);
                    }
                    else if (HUtil32.RangeInDefined(n14, 800, 1199)) //G变量
                    {
                        n1C = M2Share.Config.GlobalVal[n14 - 700];
                    }
                    else if (HUtil32.RangeInDefined(n14, 1200, 1599))//A变量
                    {
                        n1C = HUtil32.StrToInt(M2Share.Config.GlobalAVal[n14 - 1100], 1);
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_MUL);
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
                HUtil32.ArrestStringEx(QuestActionInfo.sParam1, "(", ")", ref s34);
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
                    PlayObject.MNVal[n14] = n18 * n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 100, 199))
                {
                    M2Share.Config.GlobalVal[n14 - 100] = n18 * n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 200, 299))
                {
                    PlayObject.MDyVal[n14 - 200] = n18 * n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 300, 399))
                {
                    PlayObject.MNMval[n14 - 300] = n18 * n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 400, 499))
                {
                    M2Share.Config.GlobaDyMval[n14 - 400] = n18 * n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 500, 599))
                {
                    PlayObject.MNInteger[n14 - 500] = n18 * n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 600, 699))
                {
                    PlayObject.MSString[n14 - 600] = (n18 * n1C).ToString();
                }
                else if (HUtil32.RangeInDefined(n14, 700, 799))
                {
                    M2Share.Config.GlobalAVal[n14 - 700] = (n18 * n1C).ToString();
                }
                else if (HUtil32.RangeInDefined(n14, 800, 1199)) //G变量
                {
                    M2Share.Config.GlobalVal[n14 - 700] = n18 * n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 1200, 1599))//A变量(100-499)
                {
                    M2Share.Config.GlobalAVal[n14 - 1100] = (n18 * n1C).ToString();
                }
            }
        }

        /// <summary>
        /// 变量运算 百分比  格式: PERCENT N1 N2 N3 即N1=(N2/N3)*100
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="QuestActionInfo"></param>
        private void PercentData(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var s34 = string.Empty;
            var n18 = 0;
            var n14 = HUtil32.StrToInt(GetLineVariableText(PlayObject, QuestActionInfo.sParam2), -1);
            if (n14 < 0)
            {
                n14 = M2Share.GetValNameNo(QuestActionInfo.sParam2); // 取第一个变量,并传值给n18
                if (n14 >= 0)
                {
                    if (HUtil32.RangeInDefined(n14, 0, 99))
                    {
                        n18 = PlayObject.MNVal[n14];
                    }
                    else if (HUtil32.RangeInDefined(n14, 100, 199))
                    {
                        n18 = M2Share.Config.GlobalVal[n14 - 100];
                    }
                    else if (HUtil32.RangeInDefined(n14, 200, 299))
                    {
                        n18 = PlayObject.MDyVal[n14 - 200];
                    }
                    else if (HUtil32.RangeInDefined(n14, 300, 399))
                    {
                        n18 = PlayObject.MNMval[n14 - 300];
                    }
                    else if (HUtil32.RangeInDefined(n14, 400, 499))
                    {
                        n18 = M2Share.Config.GlobaDyMval[n14 - 400];
                    }
                    else if (HUtil32.RangeInDefined(n14, 500, 599))
                    {
                        n18 = PlayObject.MNInteger[n14 - 500];
                    }
                    else if (HUtil32.RangeInDefined(n14, 800, 1199))//G变量
                    {
                        n18 = M2Share.Config.GlobalVal[n14 - 700];
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_PERCENT);
                    }
                }
                else
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_PERCENT);
                }
            }
            else
            {
                n18 = n14;
            }
            var n1C = 0;
            n14 = HUtil32.StrToInt(GetLineVariableText(PlayObject, QuestActionInfo.sParam3), -1);
            if (n14 < 0)
            {
                n14 = M2Share.GetValNameNo(QuestActionInfo.sParam3); // 取第一个变量,并传值给n1C
                if (n14 >= 0)
                {
                    if (HUtil32.RangeInDefined(n14, 0, 99))
                    {
                        n1C = PlayObject.MNVal[n14];
                    }
                    else if (HUtil32.RangeInDefined(n14, 100, 199))
                    {
                        n1C = M2Share.Config.GlobalVal[n14 - 100];
                    }
                    else if (HUtil32.RangeInDefined(n14, 200, 299))
                    {
                        n1C = PlayObject.MDyVal[n14 - 200];
                    }
                    else if (HUtil32.RangeInDefined(n14, 300, 399))
                    {
                        n1C = PlayObject.MNMval[n14 - 300];
                    }
                    else if (HUtil32.RangeInDefined(n14, 400, 499))
                    {
                        n1C = M2Share.Config.GlobaDyMval[n14 - 400];
                    }
                    else if (HUtil32.RangeInDefined(n14, 500, 599))
                    {
                        n1C = PlayObject.MNInteger[n14 - 500];
                    }
                    else if (HUtil32.RangeInDefined(n14, 800, 1199))//G变量
                    {
                        n1C = M2Share.Config.GlobalVal[n14 - 700];
                    }
                    else
                    {
                        ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_PERCENT);
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
                    PlayObject.MNVal[n14] = n18 * n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 100, 199))
                {
                    M2Share.Config.GlobalVal[n14 - 100] = n18 / n1C * 100;
                }
                else if (HUtil32.RangeInDefined(n14, 200, 299))
                {
                    PlayObject.MDyVal[n14 - 200] = n18 / n1C * 100;
                }
                else if (HUtil32.RangeInDefined(n14, 300, 399))
                {
                    PlayObject.MNMval[n14 - 300] = n18 / n1C * 100;
                }
                else if (HUtil32.RangeInDefined(n14, 400, 499))
                {
                    M2Share.Config.GlobaDyMval[n14 - 400] = n18 / n1C * 100;
                }
                else if (HUtil32.RangeInDefined(n14, 500, 599))
                {
                    PlayObject.MNInteger[n14 - 500] = n18 / n1C * 100;
                }
                else if (HUtil32.RangeInDefined(n14, 600, 699))
                {
                    PlayObject.MSString[n14 - 600] = $"{n18 / n1C * 100}%";
                }
                else if (HUtil32.RangeInDefined(n14, 700, 799))
                {
                    M2Share.Config.GlobalAVal[n14 - 700] = $"{n18 / n1C * 100}%";
                }
                else if (HUtil32.RangeInDefined(n14, 800, 1199))//G变量
                {
                    M2Share.Config.GlobalVal[n14 - 700] = n18 / n1C * 100;
                }
                else if (HUtil32.RangeInDefined(n14, 1200, 1599))//A变量
                {
                    M2Share.Config.GlobalAVal[n14 - 1100] = $"{n18 / n1C * 100}%";
                }
            }
        }

        private void SumData(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var n18 = 0;
            var s34 = string.Empty;
            var s44 = string.Empty;
            var s48 = string.Empty;
            int n14;
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
                    n18 = PlayObject.MNVal[n14];
                }
                else if (HUtil32.RangeInDefined(n14, 100, 199))
                {
                    n18 = M2Share.Config.GlobalVal[n14 - 100];
                }
                else if (HUtil32.RangeInDefined(n14, 200, 299))
                {
                    n18 = PlayObject.MDyVal[n14 - 200];
                }
                else if (HUtil32.RangeInDefined(n14, 300, 399))
                {
                    n18 = PlayObject.MNMval[n14 - 300];
                }
                else if (HUtil32.RangeInDefined(n14, 400, 499))
                {
                    n18 = M2Share.Config.GlobaDyMval[n14 - 400];
                }
                else if (HUtil32.RangeInDefined(n14, 500, 599))
                {
                    n18 = PlayObject.MNInteger[n14 - 500];
                }
                else if (HUtil32.RangeInDefined(n14, 600, 699))
                {
                    s44 = PlayObject.MSString[n14 - 600];
                }
                else if (HUtil32.RangeInDefined(n14, 700, 799))
                {
                    s44 = M2Share.Config.GlobalAVal[n14 - 700];
                }
                else if (HUtil32.RangeInDefined(n14, 800, 1199))
                {
                    n18 = M2Share.Config.GlobalVal[n14 - 700];//G变量
                }
                else if (HUtil32.RangeInDefined(n14, 1200, 1599))
                {
                    s44 = M2Share.Config.GlobalAVal[n14 - 1100];//A变量
                }
                else
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSUM);
                }
            }
            else
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSUM);
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
                    n1C = PlayObject.MNVal[n14];
                }
                else if (HUtil32.RangeInDefined(n14, 100, 199))
                {
                    n1C = M2Share.Config.GlobalVal[n14 - 100];
                }
                else if (HUtil32.RangeInDefined(n14, 200, 299))
                {
                    n1C = PlayObject.MDyVal[n14 - 200];
                }
                else if (HUtil32.RangeInDefined(n14, 300, 399))
                {
                    n1C = PlayObject.MNMval[n14 - 300];
                }
                else if (HUtil32.RangeInDefined(n14, 400, 499))
                {
                    n1C = M2Share.Config.GlobaDyMval[n14 - 400];
                }
                else if (HUtil32.RangeInDefined(n14, 500, 599))
                {
                    n1C = PlayObject.MNInteger[n14 - 500];
                }
                else if (HUtil32.RangeInDefined(n14, 600, 699))
                {
                    s48 = PlayObject.MSString[n14 - 600];
                }
                else if (HUtil32.RangeInDefined(n14, 700, 799))
                {
                    s48 = M2Share.Config.GlobalAVal[n14 - 700];
                }
                else if (HUtil32.RangeInDefined(n14, 800, 1199))
                {
                    n1C = M2Share.Config.GlobalVal[n14 - 700];//G变量
                }
                else if (HUtil32.RangeInDefined(n14, 1200, 1599))
                {
                    s48 = M2Share.Config.GlobalAVal[n14 - 1100];//A变量
                }
                else
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSUM);
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
                    PlayObject.MNVal[n14] = n18 + n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 100, 199))
                {
                    M2Share.Config.GlobalVal[n14 - 100] = n18 + n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 200, 299))
                {
                    PlayObject.MDyVal[n14 - 200] = n18 + n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 300, 399))
                {
                    PlayObject.MNMval[n14 - 300] = n18 + n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 400, 499))
                {
                    M2Share.Config.GlobaDyMval[n14 - 400] = n18 + n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 500, 599))
                {
                    PlayObject.MNInteger[n14 - 500] = n18 + n1C;
                }
                else if (HUtil32.RangeInDefined(n14, 600, 699))
                {
                    PlayObject.MSString[n14 - 600] = s44 + s48;
                }
                else if (HUtil32.RangeInDefined(n14, 700, 799))
                {
                    M2Share.Config.GlobalAVal[n14 - 700] = s44 + s48;
                }
                else if (HUtil32.RangeInDefined(n14, 800, 1199))
                {
                    M2Share.Config.GlobalVal[n14 - 700] = n18 + n1C;//G变量
                }
                else if (HUtil32.RangeInDefined(n14, 1200, 1599))
                {
                    M2Share.Config.GlobalAVal[n14 - 1100] = s44 + s48;//A变量
                }
            }
        }

        private bool GetMovDataHumanInfoValue(PlayObject PlayObject, string sVariable, ref string sValue, ref int nValue, ref int nDataType)
        {
            string s10 = string.Empty;
            string sVarValue2 = string.Empty;
            int nSecond;
            DynamicVar DynamicVar;
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
                    sValue = M2Share.Config.ServerName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$SERVERIP":
                    sValue = M2Share.Config.ServerIPaddr;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$WEBSITE":
                    sValue = M2Share.Config.sWebSite;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BBSSITE":
                    sValue = M2Share.Config.sBbsSite;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$CLIENTDOWNLOAD":
                    sValue = M2Share.Config.sClientDownload;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$QQ":
                    sValue = M2Share.Config.sQQ;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$PHONE":
                    sValue = M2Share.Config.sPhone;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT0":
                    sValue = M2Share.Config.sBankAccount0;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT1":
                    sValue = M2Share.Config.sBankAccount1;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT2":
                    sValue = M2Share.Config.sBankAccount2;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT3":
                    sValue = M2Share.Config.sBankAccount3;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT4":
                    sValue = M2Share.Config.sBankAccount4;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT5":
                    sValue = M2Share.Config.sBankAccount5;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT6":
                    sValue = M2Share.Config.sBankAccount6;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT7":
                    sValue = M2Share.Config.sBankAccount7;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT8":
                    sValue = M2Share.Config.sBankAccount8;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT9":
                    sValue = M2Share.Config.sBankAccount9;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$GAMEGOLDNAME":
                    sValue = M2Share.Config.GameGoldName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$GAMEPOINTNAME":
                    sValue = M2Share.Config.GamePointName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$USERCOUNT":
                    sValue = M2Share.WorldEngine.PlayObjectCount.ToString();
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
                    nSecond = (HUtil32.GetTickCount() - M2Share.StartTick) / 1000;
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
                case "$CASTLEWARDATE":// 申请攻城的日期
                    {
                        if (Castle == null)
                        {
                            Castle = M2Share.CastleMgr.GetCastle(0);
                        }
                        if (Castle != null)
                        {
                            if (!Castle.UnderWar)
                            {
                                sValue = Castle.GetWarDate();
                                if (sValue != "")
                                {
                                    sMsg = ReplaceVariableText(sMsg, "<$CASTLEWARDATE>", sValue);
                                }
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
                    sValue = PlayObject.ChrName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$KILLER":// 杀人者变量 
                    {
                        if (PlayObject.Death && (PlayObject.LastHiter != null))
                        {
                            if (PlayObject.LastHiter.Race == ActorRace.Play)
                            {
                                sValue = PlayObject.LastHiter.ChrName;
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
                        if (PlayObject.Death && (PlayObject.LastHiter != null))
                        {
                            if (PlayObject.LastHiter.Race != ActorRace.Play)
                            {
                                sValue = PlayObject.LastHiter.ChrName;
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
                    sValue = PlayObject.MSMasterName;
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
                    nValue = M2Share.RandomNumber.Random(int.MaxValue);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$USERID":// 登录账号
                    sValue = PlayObject.UserAccount;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$IPADDR":// 登录IP
                    sValue = PlayObject.LoginIpAddr;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$X": // 人物X坐标
                    nValue = PlayObject.CurrX;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$Y": // 人物Y坐标
                    nValue = PlayObject.CurrY;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAP":
                    sValue = PlayObject.Envir.MapName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$GUILDNAME":
                    {
                        if (PlayObject.MyGuild != null)
                        {
                            sValue = PlayObject.MyGuild.sGuildName;
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
                    sValue = PlayObject.GuildRankName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$RELEVEL":
                    nValue = PlayObject.MBtReLevel;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$LEVEL":
                    nValue = PlayObject.Abil.Level;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$HP":
                    nValue = PlayObject.WAbil.HP;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXHP":
                    nValue = PlayObject.WAbil.MaxHP;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MP":
                    nValue = PlayObject.WAbil.MP;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXMP":
                    nValue = PlayObject.WAbil.MaxMP;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$AC":
                    nValue = HUtil32.LoWord(PlayObject.WAbil.AC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXAC":
                    nValue = HUtil32.HiWord(PlayObject.WAbil.AC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAC":
                    nValue = HUtil32.LoWord(PlayObject.WAbil.MAC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXMAC":
                    nValue = HUtil32.HiWord(PlayObject.WAbil.MAC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$DC":
                    nValue = HUtil32.LoWord(PlayObject.WAbil.DC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXDC":
                    nValue = HUtil32.HiWord(PlayObject.WAbil.DC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MC":
                    nValue = HUtil32.LoWord(PlayObject.WAbil.MC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXMC":
                    nValue = HUtil32.HiWord(PlayObject.WAbil.MC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$SC":
                    nValue = HUtil32.LoWord(PlayObject.WAbil.SC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXSC":
                    nValue = HUtil32.HiWord(PlayObject.WAbil.SC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$EXP":
                    nValue = PlayObject.Abil.Exp;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXEXP":
                    nValue = PlayObject.Abil.MaxExp;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$PKPOINT":
                    nValue = PlayObject.PkPoint;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$CREDITPOINT":
                    nValue = PlayObject.MBtCreditPoint;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$HW":
                    nValue = PlayObject.WAbil.HandWeight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXHW":
                    nValue = PlayObject.WAbil.MaxHandWeight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$BW":
                    nValue = PlayObject.WAbil.Weight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXBW":
                    nValue = PlayObject.WAbil.MaxWeight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$WW":
                    nValue = PlayObject.WAbil.WearWeight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXWW":
                    nValue = PlayObject.WAbil.MaxWearWeight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$GOLDCOUNT":
                    nValue = PlayObject.Gold;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$GOLDCOUNTX":
                    nValue = PlayObject.GoldMax;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$GAMEGOLD":
                    nValue = PlayObject.MNGameGold;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$GAMEPOINT":
                    nValue = PlayObject.MNGamePoint;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$HUNGER":
                    nValue = PlayObject.GetMyStatus();
                    nDataType = 1;
                    result = true;
                    return result;
                case "$LOGINTIME":
                    sValue = PlayObject.LogonTime.ToString();
                    nDataType = 0;
                    result = true;
                    return result;
                case "$LOGINLONG":
                    nValue = (HUtil32.GetTickCount() - PlayObject.LogonTick) / 60000;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$DRESS":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[Grobal2.U_DRESS].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$WEAPON":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[Grobal2.U_WEAPON].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$RIGHTHAND":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[Grobal2.U_RIGHTHAND].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$HELMET":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[Grobal2.U_HELMET].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$NECKLACE":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[Grobal2.U_NECKLACE].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$RING_R":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[Grobal2.U_RINGR].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$RING_L":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[Grobal2.U_RINGL].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$ARMRING_R":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[Grobal2.U_ARMRINGR].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$ARMRING_L":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[Grobal2.U_ARMRINGL].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BUJUK":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[Grobal2.U_BUJUK].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BELT":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[Grobal2.U_BELT].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BOOTS":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[Grobal2.U_BOOTS].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$CHARM":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[Grobal2.U_CHARM].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$IPLOCAL":
                    sValue = PlayObject.LoginIpLocal;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$GUILDBUILDPOINT":
                    {
                        if (PlayObject.MyGuild != null)
                        {
                            //nValue = PlayObject.m_MyGuild.nBuildPoint;
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
                case "$GUILDAURAEPOINT":
                    {
                        if (PlayObject.MyGuild != null)
                        {
                            nValue = PlayObject.MyGuild.Aurae;
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
                case "$GUILDSTABILITYPOINT":
                    {
                        if (PlayObject.MyGuild != null)
                        {
                            nValue = PlayObject.MyGuild.Stability;
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
                case "$GUILDFLOURISHPOINT":
                    {
                        if (PlayObject.MyGuild != null)
                        {
                            nValue = PlayObject.MyGuild.Flourishing;
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
            }
            if (HUtil32.CompareLStr(sVariable, "$GLOBAL", 6))//  全局变量
            {
                HUtil32.ArrestStringEx(sVariable, "(", ")", ref sVarValue2);
                if (M2Share.g_DynamicVarList.TryGetValue(sVarValue2, out DynamicVar))
                {
                    switch (DynamicVar.VarType)
                    {
                        case VarType.Integer:
                            nValue = DynamicVar.nInternet;
                            nDataType = 1;
                            result = true;
                            return result;
                        case VarType.String:
                            sValue = DynamicVar.sString;
                            nDataType = 0;
                            result = true;
                            return result;
                    }
                }
            }
            if (HUtil32.CompareLStr(sVariable, "$HUMAN", 6))//  人物变量
            {
                HUtil32.ArrestStringEx(sVariable, "(", ")", ref sVarValue2);
                if (PlayObject.DynamicVarMap.TryGetValue(sVarValue2, out DynamicVar))
                {
                    switch (DynamicVar.VarType)
                    {
                        case VarType.Integer:
                            nValue = DynamicVar.nInternet;
                            nDataType = 1;
                            result = true;
                            return result;
                        case VarType.String:
                            sValue = DynamicVar.sString;
                            nDataType = 0;
                            result = true;
                            return result;
                    }
                }
            }
            if (HUtil32.CompareLStr(sVariable, "$ACCOUNT", 8)) //  人物变量
            {
                HUtil32.ArrestStringEx(sVariable, "(", ")", ref sVarValue2);
                if (PlayObject.DynamicVarMap.TryGetValue(sVarValue2, out DynamicVar))
                {
                    switch (DynamicVar.VarType)
                    {
                        case VarType.Integer:
                            nValue = DynamicVar.nInternet;
                            nDataType = 1;
                            result = true;
                            return result;
                        case VarType.String:
                            sValue = DynamicVar.sString;
                            nDataType = 0;
                            result = true;
                            return result;
                    }
                }
            }
            return result;
        }

        private bool SetMovDataValNameValue(PlayObject PlayObject, string sVarName, string sValue, int nValue, int nDataType)
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
                            PlayObject.MNVal[n100] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 100, 199))
                        {
                            M2Share.Config.GlobalVal[n100 - 100] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 200, 299))
                        {
                            PlayObject.MDyVal[n100 - 200] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 300, 399))
                        {
                            PlayObject.MNMval[n100 - 300] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 400, 499))
                        {
                            M2Share.Config.GlobaDyMval[n100 - 400] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 500, 599))
                        {
                            PlayObject.MNInteger[n100 - 500] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 600, 699))
                        {
                            PlayObject.MSString[n100 - 600] = nValue.ToString();
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 700, 799))
                        {
                            M2Share.Config.GlobalAVal[n100 - 700] = nValue.ToString();
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 800, 1199)) // G变量
                        {
                            M2Share.Config.GlobalVal[n100 - 700] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 1200, 1599)) // A变量
                        {
                            M2Share.Config.GlobalAVal[n100 - 1100] = nValue.ToString();
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
                            PlayObject.MNVal[n100] = HUtil32.StrToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 100, 199))
                        {
                            M2Share.Config.GlobalVal[n100 - 100] = HUtil32.StrToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 200, 299))
                        {
                            PlayObject.MDyVal[n100 - 200] = HUtil32.StrToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 300, 399))
                        {
                            PlayObject.MNMval[n100 - 300] = HUtil32.StrToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 400, 499))
                        {
                            M2Share.Config.GlobaDyMval[n100 - 400] = HUtil32.StrToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 500, 599))
                        {
                            PlayObject.MNInteger[n100 - 500] = HUtil32.StrToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 600, 699))
                        {
                            PlayObject.MSString[n100 - 600] = sValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 700, 799))
                        {
                            M2Share.Config.GlobalAVal[n100 - 700] = sValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 800, 1199)) // G变量
                        {
                            M2Share.Config.GlobalVal[n100 - 700] = HUtil32.StrToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 1200, 1599)) // A变量
                        {
                            M2Share.Config.GlobalAVal[n100 - 1100] = sValue;
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
                            PlayObject.MNVal[n100] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 100, 199))
                        {
                            M2Share.Config.GlobalVal[n100 - 100] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 200, 299))
                        {
                            PlayObject.MDyVal[n100 - 200] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 300, 399))
                        {
                            PlayObject.MNMval[n100 - 300] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 400, 499))
                        {
                            M2Share.Config.GlobaDyMval[n100 - 400] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 500, 599))
                        {
                            PlayObject.MNInteger[n100 - 500] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 600, 699))
                        {
                            PlayObject.MSString[n100 - 600] = sValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 700, 799))
                        {
                            M2Share.Config.GlobalAVal[n100 - 700] = sValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 800, 1199)) // G变量
                        {
                            M2Share.Config.GlobalVal[n100 - 700] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 1200, 1599)) // A变量
                        {
                            M2Share.Config.GlobalAVal[n100 - 1100] = sValue;
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

        private bool GetMovDataValNameValue(PlayObject PlayObject, string sVarName, ref string sValue, ref int nValue, ref int nDataType)
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
                    nValue = PlayObject.MNVal[n100];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 100, 199))
                {
                    nValue = M2Share.Config.GlobalVal[n100 - 100];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 200, 299))
                {
                    nValue = PlayObject.MDyVal[n100 - 200];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 300, 399))
                {
                    nValue = PlayObject.MNMval[n100 - 300];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 400, 499))
                {
                    nValue = M2Share.Config.GlobaDyMval[n100 - 400];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 500, 599))
                {
                    nValue = PlayObject.MNInteger[n100 - 500];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 600, 699))
                {
                    sValue = PlayObject.MSString[n100 - 600];
                    nDataType = 0;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 700, 799))
                {
                    sValue = M2Share.Config.GlobalAVal[n100 - 700];
                    nDataType = 0;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 800, 1199))//G变量
                {
                    nValue = M2Share.Config.GlobalVal[n100 - 700];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 1200, 1599))//A变量
                {
                    sValue = M2Share.Config.GlobalAVal[n100 - 1100];
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

        private bool GetMovDataDynamicVarValue(PlayObject PlayObject, string sVarType, string sVarName, ref string sValue, ref int nValue, ref int nDataType)
        {
            DynamicVar DynamicVar;
            string sName = string.Empty;
            sValue = "";
            nValue = -1;
            nDataType = -1;
            Dictionary<string, DynamicVar> DynamicVarList = GetDynamicVarList(PlayObject, sVarType, ref sName);
            if (DynamicVarList == null)
            {
                return false;
            }
            if (DynamicVarList.TryGetValue(sVarName, out DynamicVar))
            {
                switch (DynamicVar.VarType)
                {
                    case VarType.Integer:
                        nValue = DynamicVar.nInternet;
                        nDataType = 1;
                        break;
                    case VarType.String:
                        sValue = DynamicVar.sString;
                        nDataType = 0;
                        break;
                }
                return true;
            }
            return false;
        }

        private bool SetMovDataDynamicVarValue(PlayObject PlayObject, string sVarType, string sVarName, string sValue, int nValue, int nDataType)
        {
            DynamicVar DynamicVar;
            string sName = string.Empty;
            bool boVarFound = false;
            Dictionary<string, DynamicVar> DynamicVarList = GetDynamicVarList(PlayObject, sVarType, ref sName);
            if (DynamicVarList == null)
            {
                return false;
            }
            if (DynamicVarList.TryGetValue(sVarName, out DynamicVar))
            {
                if (nDataType == 1 && DynamicVar.VarType == VarType.Integer)
                {
                    DynamicVar.nInternet = nValue;
                    boVarFound = true;
                }
                else if (DynamicVar.VarType == VarType.String)
                {
                    DynamicVar.sString = sValue;
                    boVarFound = true;
                }
            }
            return boVarFound;
        }

        private int GetMovDataType(QuestActionInfo QuestActionInfo)
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