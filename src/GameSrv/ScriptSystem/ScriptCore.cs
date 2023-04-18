using GameSrv.GameCommand;
using GameSrv.Player;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSrv
{
    internal class ScriptCore
    {
        /// <summary>
        /// 取文本变量
        /// </summary>
        /// <returns></returns>
        public VarInfo GetVarValue(PlayObject PlayObject, string sData, ref int nValue)
        {
            string sVar = string.Empty;
            string sValue = string.Empty;
            return GetVarValue(PlayObject, sData, ref sVar, ref sValue, ref nValue);
        }

        /// <summary>
        /// 取文本变量
        /// </summary>
        /// <returns></returns>
        public VarInfo GetVarValue(PlayObject PlayObject, string sData, ref string sValue)
        {
            string sVar = string.Empty;
            int nValue = 0;
            return GetVarValue(PlayObject, sData, ref sVar, ref sValue, ref nValue);
        }

        /// <summary>
        /// 取文本变量
        /// </summary>
        /// <returns></returns>
        public VarInfo GetVarValue(PlayObject PlayObject, string sData, ref string sVar, ref string sValue, ref int nValue)
        {
            long n10;
            sVar = sData;
            sValue = sData;
            var result = new VarInfo {VarType = VarType.None, VarAttr = VarAttr.aNone};
            if (sData == "")
            {
                return result;
            }
            var sVarName = sData;
            var sName = sData;
            if (sData[0] == '<' && sData[sData.Length - 1] == '>')// <$STR(S0)>
            {
                sData = HUtil32.ArrestStringEx(sData, "<", ">", ref sName);
            }
            if (HUtil32.CompareLStr(sName, "$STR("))// $STR(S0)
            {
                sVar = '<' + sName + '>';
                result.VarType = GetValNameValue(PlayObject, sVar, ref sValue, ref nValue);
                result.VarAttr = VarAttr.aFixStr;
            }
            else if (HUtil32.CompareLStr(sName, "$HUMAN("))
            {
                sVar = '<' + sName + '>';
                result.VarType = GetDynamicValue(PlayObject, sVar, ref sValue, ref nValue);
                result.VarAttr = VarAttr.aDynamic;
            }
            else if (HUtil32.CompareLStr(sName, "$GUILD("))
            {
                sVar = '<' + sName + '>';
                result.VarType = GetDynamicValue(PlayObject, sVar, ref sValue, ref nValue);
                result.VarAttr = VarAttr.aDynamic;
            }
            else if (HUtil32.CompareLStr(sName, "$GLOBAL("))
            {
                sVar = '<' + sName + '>';
                result.VarType = GetDynamicValue(PlayObject, sVar, ref sValue, ref nValue);
                result.VarAttr = VarAttr.aDynamic;
            }
            else if (sName[0] == '$')
            {
                sName = sName.Substring(2 - 1, sName.Length - 1);
                n10 = M2Share.GetValNameNo(sName);
                if (n10 >= 0)
                {
                    sVar = "<$STR(" + sName + ")>";
                    result.VarType = GetValNameValue(PlayObject, sVar, ref sValue, ref nValue);
                    result.VarAttr = VarAttr.aFixStr;
                }
                else
                {
                    sVar = "<$" + sName + '>';
                    sValue = GetLineVariableText(PlayObject, sVar);
                    if (string.Compare(sValue, sVar, StringComparison.Ordinal) == 0)
                    {
                        sValue = sVarName;
                        nValue = HUtil32.StrToInt(sVarName, 0);
                    }
                    else
                    {
                        result.VarType = VarType.String;
                        nValue = HUtil32.StrToInt(sValue, 0);
                        if (HUtil32.IsStringNumber(sValue))
                        {
                            result.VarType = VarType.Integer;
                        }
                    }
                    result.VarAttr = VarAttr.aConst;
                }
            }
            else
            {
                n10 = M2Share.GetValNameNo(sName);
                if (n10 >= 0)
                {
                    sVar = "<$STR(" + sName + ")>";
                    result.VarType = GetValNameValue(PlayObject, sVar, ref sValue, ref nValue);
                    result.VarAttr = VarAttr.aFixStr;
                }
                else
                {
                    result.VarType = VarType.String;
                    nValue = HUtil32.StrToInt(sValue, 0);
                    if (HUtil32.IsStringNumber(sValue))
                    {
                        result.VarType = VarType.Integer;
                    }
                    result.VarAttr = VarAttr.aConst;
                }
            }
            return result;
        }

        public VarType GetDynamicValue(PlayObject PlayObject, string sVar, ref string sValue, ref int nValue)
        {
            string sVarName = "";
            string sVarType = "";
            string sData = sVar;
            string sName = "";
            var result = VarType.None;
            if (sData[0] == '<' && sData[sData.Length - 1] == '>')
            {
                sData = HUtil32.ArrestStringEx(sData, "<", ">", ref sName);
            }
            if (HUtil32.CompareLStr(sName, "$HUMAN("))
            {
                sData = HUtil32.ArrestStringEx(sName, "(", ")", ref sVarName);
                sVarType = "HUMAN";
            }
            else if (HUtil32.CompareLStr(sName, "$GUILD("))
            {
                sData = HUtil32.ArrestStringEx(sName, "(", ")", ref sVarName);
                sVarType = "GUILD";
            }
            else if (HUtil32.CompareLStr(sName, "$GLOBAL("))
            {
                sData = HUtil32.ArrestStringEx(sName, "(", ")", ref sVarName);
                sVarType = "GLOBAL";
            }
            if (sVarName == "" || sVarType == "")
            {
                return result;
            }
            Dictionary<string, DynamicVar> DynamicVarList = GeDynamicVarList(PlayObject, sVarType, ref sName);
            if (DynamicVarList == null)
            {
                return result;
            }
            if (DynamicVarList.TryGetValue(sVarName, out DynamicVar DynamicVar))
            {
                if (string.Compare(DynamicVar.sName, sVarName, StringComparison.Ordinal) == 0)
                {
                    switch (DynamicVar.VarType)
                    {
                        case VarType.Integer:
                            nValue = DynamicVar.nInternet;
                            sValue = nValue.ToString();
                            result = VarType.Integer;
                            break;

                        case VarType.String:
                            sValue = DynamicVar.sString;
                            nValue = HUtil32.StrToInt(sValue, 0);
                            result = VarType.String;
                            break;
                    }
                }
            }
            return result;
        }

        public VarType GetValNameValue(PlayObject PlayObject, string sVar, ref string sValue, ref int nValue)
        {
            var result = VarType.None;
            var sName = string.Empty;
            if (sVar == "")
            {
                return result;
            }
            var sData = sVar;
            if (sData[0] == '<' && sData[^1] == '>')
            {
                sData = HUtil32.ArrestStringEx(sData, "<", ">", ref sName); // <$STR(S0)>
            }
            if (HUtil32.CompareLStr(sName, "$STR("))
            {
                sData = HUtil32.ArrestStringEx(sName, "(", ")", ref sName); // $STR(S0)
            }
            if (sName[0] == '$')
            {
                sName = sName.Substring(1, sName.Length - 1);// $S0
            }
            var n01 = M2Share.GetValNameNo(sName);
            if (n01 >= 0)
            {
                if (HUtil32.RangeInDefined(n01, 0, 99))
                {
                    nValue = M2Share.Config.GlobalVal[n01];// G
                    sValue = nValue.ToString();
                    result = VarType.Integer;
                }
                else if (HUtil32.RangeInDefined(n01, 1000, 1099))
                {
                    nValue = M2Share.Config.GlobaDyMval[n01 - 1000];// I
                    sValue = nValue.ToString();
                    result = VarType.Integer;
                }
                else if (HUtil32.RangeInDefined(n01, 1100, 1109))
                {
                    nValue = PlayObject.MNVal[n01 - 1100];// P
                    sValue = nValue.ToString();
                    result = VarType.Integer;
                }
                else if (HUtil32.RangeInDefined(n01, 1110, 1119))
                {
                    nValue = PlayObject.MDyVal[n01 - 1110];// D
                    sValue = nValue.ToString();
                    result = VarType.Integer;
                }
                else if (HUtil32.RangeInDefined(n01, 1200, 1299))
                {
                    nValue = PlayObject.MNMval[n01 - 1200];// M
                    sValue = nValue.ToString();
                    result = VarType.Integer;
                }
                else if (HUtil32.RangeInDefined(n01, 1300, 1399))
                {
                    nValue = PlayObject.MNInteger[n01 - 1300];// N
                    sValue = nValue.ToString();
                    result = VarType.Integer;
                }
                else if (HUtil32.RangeInDefined(n01, 2000, 2499))
                {
                    sValue = M2Share.Config.GlobalAVal[n01 - 2000];// A
                    nValue = HUtil32.StrToInt(sValue, 0);
                    result = VarType.String;
                }
                else if (HUtil32.RangeInDefined(n01, 1400, 1499))
                {
                    sValue = PlayObject.MSString[n01 - 1400];// S
                    nValue = HUtil32.StrToInt(sValue, 0);
                    result = VarType.String;
                }
            }
            else if (sName != "" && char.ToUpper(sName[0]) == 'S')
            {
                if (PlayObject.m_StringList.ContainsKey(sName))
                {
                    sValue = PlayObject.m_StringList[sName];
                }
                else
                {
                    PlayObject.m_StringList.Add(sName, "");
                    sValue = "";
                }
                result = VarType.String;
            }
            else if (sName != "" && char.ToUpper(sName[0]) == 'N')
            {
                if (PlayObject.m_IntegerList.ContainsKey(sName))
                {
                    nValue = PlayObject.m_IntegerList[sName];
                }
                else
                {
                    nValue = 0;
                    PlayObject.m_IntegerList.Add(sName, nValue);
                }
                result = VarType.Integer;
            }
            return result;
        }

        public string GetLineVariableText(PlayObject PlayObject, string sMsg)
        {
            var nC = 0;
            var sText = string.Empty;
            var tempstr = sMsg;
            while (true)
            {
                if (tempstr.IndexOf('>') <= 0)
                {
                    break;
                }
                tempstr = HUtil32.ArrestStringEx(tempstr, "<", ">", ref sText);
                if (!string.IsNullOrEmpty(sText) && sText[0] == '$')
                {
                    GetVariableText(PlayObject, ref sMsg, sText);
                }
                nC++;
                if (nC >= 101)
                {
                    break;
                }
            }
            return sMsg;
        }

        public virtual void GetVariableText(PlayObject PlayObject, ref string sMsg, string sVariable)
        {
            string dynamicName = string.Empty;
            DynamicVar DynamicVar;
            bool boFoundVar;
            if (HUtil32.IsStringNumber(sVariable))//检查发送字符串是否有数字
            {
                string sIdx = sVariable.Substring(1, sVariable.Length - 1);
                int nIdx = HUtil32.StrToInt(sIdx, -1);
                if (nIdx == -1)
                {
                    return;
                }
                GrobalVarScript.Handler(PlayObject, nIdx, sVariable, ref sMsg);
                return;
            }
            // 个人信息
            if (sVariable == "$CMD_ATTACKMODE")
            {
                sMsg = CombineStr(sMsg, "<$CMD_ATTACKMODE>", CommandMgr.GameCommands.AttackMode.CmdName);
                return;
            }
            if (sVariable == "$CMD_REST")
            {
                sMsg = CombineStr(sMsg, "<$CMD_REST>", CommandMgr.GameCommands.Rest.CmdName);
                return;
            }
            if (sVariable == "$CMD_UNLOCK")
            {
                sMsg = CombineStr(sMsg, "<$CMD_UNLOCK>", CommandMgr.GameCommands.Unlock.CmdName);
                return;
            }

            if (HUtil32.CompareLStr(sVariable, "$HUMAN("))
            {
                HUtil32.ArrestStringEx(sVariable, "(", ")", ref dynamicName);
                boFoundVar = false;
                if (PlayObject.DynamicVarMap.TryGetValue(dynamicName, out DynamicVar))
                {
                    if (string.Compare(DynamicVar.sName, dynamicName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        switch (DynamicVar.VarType)
                        {
                            case VarType.Integer:
                                sMsg = CombineStr(sMsg, '<' + sVariable + '>', DynamicVar.nInternet.ToString());
                                boFoundVar = true;
                                break;

                            case VarType.String:
                                sMsg = CombineStr(sMsg, '<' + sVariable + '>', DynamicVar.sString);
                                boFoundVar = true;
                                break;
                        }
                    }
                }
                if (!boFoundVar)
                {
                    sMsg = "??";
                }
                return;
            }
            if (HUtil32.CompareLStr(sVariable, "$GUILD("))
            {
                if (PlayObject.MyGuild == null)
                {
                    return;
                }
                HUtil32.ArrestStringEx(sVariable, "(", ")", ref dynamicName);
                boFoundVar = false;
                if (PlayObject.MyGuild.DynamicVarList.TryGetValue(dynamicName, out DynamicVar))
                {
                    if (string.Compare(DynamicVar.sName, dynamicName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        switch (DynamicVar.VarType)
                        {
                            case VarType.Integer:
                                sMsg = CombineStr(sMsg, '<' + sVariable + '>', DynamicVar.nInternet);
                                boFoundVar = true;
                                break;

                            case VarType.String:
                                sMsg = CombineStr(sMsg, '<' + sVariable + '>', DynamicVar.sString);
                                boFoundVar = true;
                                break;
                        }
                    }
                }
                if (!boFoundVar)
                {
                    sMsg = "??";
                }
                return;
            }
            if (HUtil32.CompareLStr(sVariable, "$GLOBAL("))
            {
                HUtil32.ArrestStringEx(sVariable, "(", ")", ref dynamicName);
                boFoundVar = false;
                if (M2Share.DynamicVarList.TryGetValue(dynamicName, out DynamicVar))
                {
                    if (string.Compare(DynamicVar.sName, dynamicName, StringComparison.Ordinal) == 0)
                    {
                        switch (DynamicVar.VarType)
                        {
                            case VarType.Integer:
                                sMsg = CombineStr(sMsg, '<' + sVariable + '>', DynamicVar.nInternet.ToString());
                                boFoundVar = true;
                                break;
                            case VarType.String:
                                sMsg = CombineStr(sMsg, '<' + sVariable + '>', DynamicVar.sString);
                                boFoundVar = true;
                                break;
                        }
                    }
                }
                if (!boFoundVar)
                {
                    sMsg = "??";
                }
                return;
            }
            if (HUtil32.CompareLStr(sVariable, "$STR("))
            {
                HUtil32.ArrestStringEx(sVariable, "(", ")", ref dynamicName);
                int n18 = M2Share.GetValNameNo(dynamicName);
                if (n18 >= 0)
                {
                    if (HUtil32.RangeInDefined(n18, 0, 499))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', M2Share.Config.GlobalVal[n18].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 1100, 1109))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', PlayObject.MNVal[n18 - 1100].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 1110, 1119))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', PlayObject.MDyVal[n18 - 1110].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 1200, 1299))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', PlayObject.MNMval[n18 - 1200].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 1000, 1099))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', M2Share.Config.GlobaDyMval[n18 - 1000].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 1300, 1399))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', PlayObject.MNInteger[n18 - 1300].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 1400, 1499))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', PlayObject.MSString[n18 - 1400]);
                    }
                    else if (HUtil32.RangeInDefined(n18, 2000, 2499))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', M2Share.Config.GlobalAVal[n18 - 2000]);
                    }
                }
                else if (dynamicName != "" && char.ToUpper(dynamicName[1]) == 'S')
                {
                    sMsg = CombineStr(sMsg, '<' + sVariable + '>', PlayObject.m_StringList.ContainsKey(dynamicName) ? PlayObject.m_StringList[dynamicName] : "");
                }
                else if (dynamicName != "" && char.ToUpper(dynamicName[1]) == 'N')
                {
                    sMsg = CombineStr(sMsg, '<' + sVariable + '>', PlayObject.m_IntegerList.ContainsKey(dynamicName) ? Convert.ToString(PlayObject.m_IntegerList[dynamicName]) : "-1");
                }
            }
        }

        public bool SetDynamicValue(PlayObject PlayObject, string sVar, string sValue, int nValue)
        {
            var result = false;
            var sVarName = "";
            var sVarType = "";
            var sName = "";
            var sData = sVar;
            if (sData[0] == '<' && sData[sData.Length - 1] == '>')
            {
                sData = HUtil32.ArrestStringEx(sData, "<", ">", ref sName);
            }
            if (HUtil32.CompareLStr(sName, "$HUMAN("))
            {
                sData = HUtil32.ArrestStringEx(sName, "(", ")", ref sVarName);
                sVarType = "HUMAN";
            }
            else if (HUtil32.CompareLStr(sName, "$GUILD("))
            {
                sData = HUtil32.ArrestStringEx(sName, "(", ")", ref sVarName);
                sVarType = "GUILD";
            }
            else if (HUtil32.CompareLStr(sName, "$GLOBAL("))
            {
                sData = HUtil32.ArrestStringEx(sName, "(", ")", ref sVarName);
                sVarType = "GLOBAL";
            }
            if (sVarName == "" || sVarType == "")
            {
                return false;
            }
            var dynamicVarList = GeDynamicVarList(PlayObject, sVarType, ref sName);
            if (dynamicVarList == null)
            {
                return false;
            }
            if (dynamicVarList.TryGetValue(sVarName, out var dynamicVar))
            {
                switch (dynamicVar.VarType)
                {
                    case VarType.Integer:
                        dynamicVar.nInternet = nValue;
                        break;

                    case VarType.String:
                        dynamicVar.sString = sValue;
                        break;
                }
                result = true;
            }
            return result;
        }

        public bool SetValNameValue(PlayObject PlayObject, string sVar, string sValue, int nValue)
        {
            var sName = string.Empty;
            var result = false;
            if (sVar == "")
            {
                return false;
            }
            var sData = sVar;
            if (sData[0] == '<' && sData[sData.Length - 1] == '>') // <$STR(S0)>
            {
                sData = HUtil32.ArrestStringEx(sData, "<", ">", ref sName);
            }
            if (HUtil32.CompareLStr(sName, "$STR("))// $STR(S0)
            {
                sData = HUtil32.ArrestStringEx(sName, "(", ")", ref sName);
            }
            if (sName[0] == '$')// $S0
            {
                sName = sName.Substring(1, sName.Length - 1);
            }
            var n01 = M2Share.GetValNameNo(sName);
            if (n01 >= 0)
            {
                if (HUtil32.RangeInDefined(n01, 0, 499))
                {
                    M2Share.Config.GlobalVal[n01] = nValue;// G
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1000, 1099))
                {
                    M2Share.Config.GlobaDyMval[n01 - 1000] = nValue;// I
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1100, 1109))
                {
                    PlayObject.MNVal[n01 - 1100] = nValue;// P
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1110, 1119))
                {
                    PlayObject.MDyVal[n01 - 1110] = nValue;// D
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1200, 1299))
                {
                    PlayObject.MNMval[n01 - 1200] = nValue;// M
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1300, 1399))
                {
                    PlayObject.MNInteger[n01 - 1300] = nValue;// N
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 2000, 2499))
                {
                    M2Share.Config.GlobalAVal[n01 - 2000] = sValue;// A
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1400, 1499))
                {
                    PlayObject.MSString[n01 - 1400] = sValue;// S
                    result = true;
                }
            }
            else if (sName != "" && char.ToUpper(sName[0]) == 'S')
            {
                if (PlayObject.m_StringList.ContainsKey(sName))
                {
                    PlayObject.m_StringList[sName] = sValue;
                    result = true;
                }
                else
                {
                    PlayObject.m_StringList.Add(sName, sValue);
                    result = true;
                }
            }
            else if (sName != "" && char.ToUpper(sName[0]) == 'N')
            {
                if (PlayObject.m_IntegerList.ContainsKey(sName))
                {
                    PlayObject.m_IntegerList[sName] = nValue;
                    result = true;
                }
                else
                {
                    PlayObject.m_IntegerList.Add(sName, nValue);
                    result = true;
                }
            }
            return result;
        }

        public Dictionary<string, DynamicVar> GeDynamicVarList(PlayObject PlayObject, string sType, ref string sName)
        {
            Dictionary<string, DynamicVar> result = null;
            if (HUtil32.CompareLStr(sType, "HUMAN"))
            {
                result = PlayObject.DynamicVarMap;
                sName = PlayObject.ChrName;
            }
            else if (HUtil32.CompareLStr(sType, "GUILD"))
            {
                if (PlayObject.MyGuild == null)
                {
                    return null;
                }
                result = PlayObject.MyGuild.DynamicVarList;
                sName = PlayObject.MyGuild.GuildName;
            }
            else if (HUtil32.CompareLStr(sType, "GLOBAL"))
            {
                result = M2Share.DynamicVarList;
                sName = "GLOBAL";
            }
            return result;
        }

        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <param name="sMsg"></param>
        /// <param name="variable"></param>
        /// <param name="variableVal"></param>
        /// <returns></returns>
        public string CombineStr(string sMsg, string variable, object variableVal)
        {
            string result;
            var n10 = sMsg.IndexOf(variable, StringComparison.Ordinal);
            if (n10 > -1)
            {
                var s14 = sMsg.Substring(1 - 1, n10);
                var s18 = sMsg.Substring(variable.Length + n10, sMsg.Length - (variable.Length + n10));
                result = s14 + Convert.ToString(variableVal) + s18;
            }
            else
            {
                result = sMsg;
            }
            return result;
        }

        public bool GotoLable_CheckStringList(string sHumName, string sListFileName)
        {
            bool result = false;
            StringList LoadList;
            sListFileName = M2Share.Config.EnvirDir + sListFileName;
            if (File.Exists(sListFileName))
            {
                LoadList = new StringList();
                try
                {
                    LoadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    M2Share.Logger.Error("loading fail.... => " + sListFileName);
                }
                for (int i = 0; i < LoadList.Count; i++)
                {
                    if (string.Compare(LoadList[i].Trim(), sHumName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = true;
                        break;
                    }
                }
                LoadList = null;
            }
            else
            {
                M2Share.Logger.Error("file not found => " + sListFileName);
            }
            return result;
        }
    }
}
