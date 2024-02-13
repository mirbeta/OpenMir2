using OpenMir2;
using OpenMir2.Common;
using ScriptSystem.Consts;
using System.Reflection;
using System.Runtime.InteropServices;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Const;
using SystemModule.Data;

namespace ScriptSystem
{
    /// <summary>
    /// 脚本解析器
    /// </summary>
    public class ScriptParsers : IScriptParsers
    {
        private readonly Dictionary<string, int> ConditionCodeDefMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, int> ExecutionCodeDefMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> CallScriptDict = new Dictionary<string, string>();
        private readonly char[] TextSpitConst = new[] { ' ', '\t' };

        public ScriptParsers()
        {
            FieldInfo[] conditionFields = typeof(ConditionCode).GetFields();
            if (conditionFields.Length > 0)
            {
                for (int i = 0; i < conditionFields.Length; i++)
                {
                    ScriptDefName codeField = conditionFields[i].GetCustomAttribute<ScriptDefName>();
                    if (codeField != null)
                    {
                        if (ConditionCodeDefMap.ContainsKey(codeField.CommandName))
                        {
                            LogService.Warn($"重复脚本检测编码[{codeField.CommandName}]定义");
                            continue;
                        }
                        ConditionCodeDefMap.Add(codeField.CommandName, i);
                    }
                }
            }

            FieldInfo[] executionFields = typeof(ExecutionCode).GetFields();

            if (executionFields.Length > 0)
            {
                for (int i = 0; i < executionFields.Length; i++)
                {
                    ScriptDefName codeField = executionFields[i].GetCustomAttribute<ScriptDefName>();
                    if (codeField != null)
                    {
                        if (ExecutionCodeDefMap.ContainsKey(codeField.CommandName))
                        {
                            LogService.Warn($"重复脚本执行编码[{codeField.CommandName}]定义");
                            continue;
                        }
                        ExecutionCodeDefMap.Add(codeField.CommandName, i);
                    }
                }
            }

            LogService.Info("初始化脚本编码定义成功...");
        }

        public void LoadScript(INormNpc NPC, string sPatch, string scriptName)
        {
            if (string.IsNullOrEmpty(sPatch))
            {
                sPatch = ScriptFlagConst.sNpc_def;
            }
            LoadScriptFile(NPC, sPatch, scriptName, false);
        }

        private static bool LoadScriptCallScript(string sFileName, string sLabel, StringList List)
        {
            bool result = false;
            if (File.Exists(sFileName))
            {
                StringList callStrList = new StringList();
                callStrList.LoadFromFile(sFileName);
                sLabel = '[' + sLabel + ']';
                bool findLab = false;
                for (int i = 0; i < callStrList.Count; i++)
                {
                    string sLine = callStrList[i].Trim();
                    if (!string.IsNullOrEmpty(sLine))
                    {
                        if (!findLab)
                        {
                            if (sLine[0] == '[' && string.Compare(sLine, sLabel, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                findLab = true;
                                continue;
                            }
                        }
                        if (sLine[0] != '{')
                        {
                            if (sLine[0] == '}')
                            {
                                result = true;
                                break;
                            }
                            List.Add(sLine);
                        }
                    }
                }
                callStrList.Dispose();
            }
            return result;
        }

        private static string GetCallScriptPath(string path)
        {
            string sCallScriptFile = path;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (sCallScriptFile.StartsWith("\\\\"))
                {
                    sCallScriptFile = sCallScriptFile.Remove(0, 2);
                }
                else if (sCallScriptFile.StartsWith("\\"))
                {
                    sCallScriptFile = sCallScriptFile.Remove(0, 1);
                }
            }
            else
            {
                if (sCallScriptFile.StartsWith("\\\\"))
                {
                    sCallScriptFile = sCallScriptFile.Remove(0, 2);
                }
                else if (sCallScriptFile.StartsWith("\\"))
                {
                    sCallScriptFile = sCallScriptFile.Remove(0, 1);
                }
                sCallScriptFile = sCallScriptFile.Replace("\\", "/");
            }
            return sCallScriptFile;
        }

        private void LoadCallScript(ref StringList LoadList, ref bool success)
        {
            int callCount = ScriptHelper.GetScriptCallCount(LoadList.Text);
            if (callCount <= 0)
            {
                success = true;
                return;
            }
            string sLable = string.Empty;
            StringList callList = new StringList(1024);
            for (int i = 0; i < LoadList.Count; i++)
            {
                string sLine = LoadList[i].Trim();
                if (!string.IsNullOrEmpty(sLine) && sLine[0] == '#' && HUtil32.CompareLStr(sLine, "#CALL"))
                {
                    sLine = HUtil32.ArrestStringEx(sLine, "[", "]", ref sLable);
                    string sCallScriptFile = GetCallScriptPath(sLable.Trim());
                    string sLabName = sLine.Trim();
                    string sFileName = SystemShare.GetEnvirFilePath("QuestDiary", sCallScriptFile);
                    if (CallScriptDict.ContainsKey(sFileName))
                    {
                        callCount--;
                        callList[i] = "#ACT";
                        callList.InsertText(i + 1, "goto " + sLabName);
                        break;
                    }
                    if (LoadScriptCallScript(sFileName, sLabName, callList))
                    {
                        callCount--;
                        if (!CallScriptDict.ContainsKey(sLabName))
                        {
                            CallScriptDict.Add(sFileName, sLabName);
                        }
                    }
                    else
                    {
                        LogService.Error("script error, load fail: " + sCallScriptFile + sLabName);
                    }
                }
                else
                {
                    callList.AppendText(sLine);
                }
            }
            LoadList = callList;
            success = callCount <= 0;
        }

        private string LoadScriptDefineInfo(StringList stringList, ICollection<DefineInfo> List)
        {
            string result = string.Empty;
            string defFile = string.Empty;
            string defineName = string.Empty;
            string defText = string.Empty;
            for (int i = 0; i < stringList.Count; i++)
            {
                string line = stringList[i].Trim();
                if (!string.IsNullOrEmpty(line) && line[0] == '#')
                {
                    if (HUtil32.CompareLStr(line, "#SETHOME"))
                    {
                        result = HUtil32.GetValidStr3(line, ref defFile, TextSpitConst).Trim();
                        stringList[i] = "";
                        continue;
                    }
                    if (HUtil32.CompareLStr(line, "#DEFINE"))
                    {
                        line = HUtil32.GetValidStr3(line, ref defFile, TextSpitConst);
                        line = HUtil32.GetValidStr3(line, ref defineName, TextSpitConst);
                        line = HUtil32.GetValidStr3(line, ref defText, TextSpitConst);
                        DefineInfo DefineInfo = new DefineInfo { Name = defineName.ToUpper(), Text = defText };
                        List.Add(DefineInfo);
                        stringList[i] = "";
                        continue;
                    }
                    if (HUtil32.CompareLStr(line, "#INCLUDE"))
                    {
                        string definesFile = HUtil32.GetValidStr3(line, ref defFile, TextSpitConst).Trim();
                        definesFile = SystemShare.GetEnvirFilePath("Defines", definesFile);
                        if (File.Exists(definesFile))
                        {
                            using StringList LoadStrList = new StringList();
                            LoadStrList.LoadFromFile(definesFile);
                            result = LoadScriptDefineInfo(LoadStrList, List);
                        }
                        else
                        {
                            LogService.Error("script error, load fail: " + definesFile);
                        }
                        stringList[i] = "";
                        continue;
                    }
                }
            }
            return result;
        }

        private static ScriptInfo LoadMakeNewScript(INormNpc NPC)
        {
            ScriptInfo scriptInfo = new ScriptInfo
            {
                IsQuest = false,
                RecordList = new Dictionary<string, SayingRecord>(StringComparer.OrdinalIgnoreCase)
            };
            NPC.AddScript(scriptInfo);
            return scriptInfo;
        }

        private bool LoadScriptFileQuestCondition(string sText, ref QuestConditionInfo questConditionInfo)
        {
            bool result = false;
            string sCmd = string.Empty;
            string sParam1 = string.Empty;
            string sParam2 = string.Empty;
            string sParam3 = string.Empty;
            string sParam4 = string.Empty;
            string sParam5 = string.Empty;
            string sParam6 = string.Empty;
            int nCMDCode = 0;
            sText = HUtil32.GetValidStrCap(sText, ref sCmd, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam1, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam2, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam3, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam4, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam5, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam6, TextSpitConst);
            if (sCmd.IndexOf(".", StringComparison.OrdinalIgnoreCase) > -1) //支持脚本变量
            {
                string sActName = string.Empty;
                sCmd = HUtil32.GetValidStrCap(sCmd, ref sActName, '.');
                if (!string.IsNullOrEmpty(sActName))
                {
                    questConditionInfo.sOpName = sActName;
                    if (".".IndexOf(sCmd, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        sCmd = HUtil32.GetValidStrCap(sCmd, ref sActName, '.');
                        if (string.Compare(sActName, "H", StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            questConditionInfo.sOpHName = "H";
                        }
                    }
                }
            }
            sCmd = sCmd.ToUpper();

            if (ConditionCodeDefMap.TryGetValue(sCmd, out int code))
            {
                if (code == (int)ConditionCode.CHECK)
                {
                    nCMDCode = code;
                    HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                    if (!HUtil32.IsStringNumber(sParam1))
                    {
                        nCMDCode = 0;
                    }
                    if (!HUtil32.IsStringNumber(sParam2))
                    {
                        nCMDCode = 0;
                    }
                }
                else if (code == (int)ConditionCode.CHECKOPEN)
                {
                    nCMDCode = code;
                    HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                    if (!HUtil32.IsStringNumber(sParam1))
                    {
                        nCMDCode = 0;
                    }
                    if (!HUtil32.IsStringNumber(sParam2))
                    {
                        nCMDCode = 0;
                    }
                }
                else if (code == (int)ConditionCode.CHECKUNIT)
                {
                    nCMDCode = code;
                    HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                    if (!HUtil32.IsStringNumber(sParam1))
                    {
                        nCMDCode = 0;
                    }
                    if (!HUtil32.IsStringNumber(sParam2))
                    {
                        nCMDCode = 0;
                    }
                }
                else
                {
                    nCMDCode = code - 1;
                }
            }

        L001:
            if (nCMDCode > 0)
            {
                questConditionInfo.CmdCode = nCMDCode;
                if (!string.IsNullOrEmpty(sParam1) && sParam1[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam1, "\"", "\"", ref sParam1);
                }
                if (!string.IsNullOrEmpty(sParam2) && sParam2[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam2, "\"", "\"", ref sParam2);
                }
                if (!string.IsNullOrEmpty(sParam3) && sParam3[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam3, "\"", "\"", ref sParam3);
                }
                if (!string.IsNullOrEmpty(sParam4) && sParam4[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam4, "\"", "\"", ref sParam4);
                }
                if (!string.IsNullOrEmpty(sParam5) && sParam5[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam5, "\"", "\"", ref sParam5);
                }
                if (!string.IsNullOrEmpty(sParam6) && sParam6[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam6, "\"", "\"", ref sParam6);
                }
                questConditionInfo.sParam1 = sParam1;
                questConditionInfo.sParam2 = sParam2;
                questConditionInfo.sParam3 = sParam3;
                questConditionInfo.sParam4 = sParam4;
                questConditionInfo.sParam5 = sParam5;
                questConditionInfo.sParam6 = sParam6;
                if (HUtil32.IsStringNumber(sParam1))
                {
                    questConditionInfo.nParam1 = HUtil32.StrToInt(sParam1, 0);
                }
                if (HUtil32.IsStringNumber(sParam2))
                {
                    questConditionInfo.nParam2 = HUtil32.StrToInt(sParam2, 0);
                }
                if (HUtil32.IsStringNumber(sParam3))
                {
                    questConditionInfo.nParam3 = HUtil32.StrToInt(sParam3, 0);
                }
                if (HUtil32.IsStringNumber(sParam4))
                {
                    questConditionInfo.nParam4 = HUtil32.StrToInt(sParam4, 0);
                }
                if (HUtil32.IsStringNumber(sParam5))
                {
                    questConditionInfo.nParam5 = HUtil32.StrToInt(sParam5, 0);
                }
                if (HUtil32.IsStringNumber(sParam6))
                {
                    questConditionInfo.nParam6 = HUtil32.StrToInt(sParam6, 0);
                }
                result = true;
            }
            return result;
        }

        private bool LoadScriptFileQuestAction(string sText, ref QuestActionInfo questActionInfo)
        {
            string sCmd = string.Empty;
            string sParam1 = string.Empty;
            string sParam2 = string.Empty;
            string sParam3 = string.Empty;
            string sParam4 = string.Empty;
            string sParam5 = string.Empty;
            string sParam6 = string.Empty;
            int nCMDCode = 0;
            bool result = false;
            sText = HUtil32.GetValidStrCap(sText, ref sCmd, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam1, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam2, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam3, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam4, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam5, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam6, TextSpitConst);
            if (sCmd.IndexOf(".", StringComparison.OrdinalIgnoreCase) > -1) //支持脚本变量
            {
                string sActName = string.Empty;
                sCmd = HUtil32.GetValidStrCap(sCmd, ref sActName, '.');
                if (!string.IsNullOrEmpty(sActName))
                {
                    questActionInfo.sOpName = sActName;
                    if (sCmd.IndexOf(".", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        sCmd = HUtil32.GetValidStrCap(sCmd, ref sActName, '.');
                        if (string.Compare(sActName, "H", StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            questActionInfo.sOpHName = "H";
                        }
                    }
                }
            }
            sCmd = sCmd.ToUpper();

            if (ExecutionCodeDefMap.TryGetValue(sCmd, out int code))
            {
                if (code == (int)ExecutionCode.Set)
                {
                    nCMDCode = code;
                    HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                    if (!HUtil32.IsStringNumber(sParam1))
                    {
                        nCMDCode = 0;
                    }
                    if (!HUtil32.IsStringNumber(sParam2))
                    {
                        nCMDCode = 0;
                    }
                }
                else if (code == (int)ExecutionCode.ReSet)
                {
                    nCMDCode = code;
                    HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                    if (!HUtil32.IsStringNumber(sParam1))
                    {
                        nCMDCode = 0;
                    }
                    if (!HUtil32.IsStringNumber(sParam2))
                    {
                        nCMDCode = 0;
                    }
                }
                else if (code == (int)ExecutionCode.SetOpen)
                {
                    nCMDCode = code;
                    HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                    if (!HUtil32.IsStringNumber(sParam1))
                    {
                        nCMDCode = 0;
                    }
                    if (!HUtil32.IsStringNumber(sParam2))
                    {
                        nCMDCode = 0;
                    }
                }
                else if (code == (int)ExecutionCode.SetUnit)
                {
                    nCMDCode = code;
                    HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                    if (!HUtil32.IsStringNumber(sParam1))
                    {
                        nCMDCode = 0;
                    }
                    if (!HUtil32.IsStringNumber(sParam2))
                    {
                        nCMDCode = 0;
                    }
                }
                else if (code == (int)ExecutionCode.ResetUnit)
                {
                    nCMDCode = code;
                    HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                    if (!HUtil32.IsStringNumber(sParam1))
                    {
                        nCMDCode = 0;
                    }
                    if (!HUtil32.IsStringNumber(sParam2))
                    {
                        nCMDCode = 0;
                    }
                }
                else
                {
                    nCMDCode = code - 1;
                }
            }

        L001:
            if (nCMDCode > 0)
            {
                questActionInfo.nCmdCode = nCMDCode;
                if (!string.IsNullOrEmpty(sParam1) && sParam1[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam1, "\"", "\"", ref sParam1);
                }
                if (!string.IsNullOrEmpty(sParam2) && sParam2[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam2, "\"", "\"", ref sParam2);
                }
                if (!string.IsNullOrEmpty(sParam3) && sParam3[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam3, "\"", "\"", ref sParam3);
                }
                if (!string.IsNullOrEmpty(sParam4) && sParam4[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam4, "\"", "\"", ref sParam4);
                }
                if (!string.IsNullOrEmpty(sParam5) && sParam5[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam5, "\"", "\"", ref sParam5);
                }
                if (!string.IsNullOrEmpty(sParam6) && sParam6[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam6, "\"", "\"", ref sParam6);
                }
                questActionInfo.sParam1 = sParam1;
                questActionInfo.sParam2 = sParam2;
                questActionInfo.sParam3 = sParam3;
                questActionInfo.sParam4 = sParam4;
                questActionInfo.sParam5 = sParam5;
                questActionInfo.sParam6 = sParam6;
                if (HUtil32.IsStringNumber(sParam1))
                {
                    questActionInfo.nParam1 = HUtil32.StrToInt(sParam1, 0);
                }
                if (HUtil32.IsStringNumber(sParam2))
                {
                    questActionInfo.nParam2 = HUtil32.StrToInt(sParam2, 1);
                }
                if (HUtil32.IsStringNumber(sParam3))
                {
                    questActionInfo.nParam3 = HUtil32.StrToInt(sParam3, 1);
                }
                if (HUtil32.IsStringNumber(sParam4))
                {
                    questActionInfo.nParam4 = HUtil32.StrToInt(sParam4, 0);
                }
                if (HUtil32.IsStringNumber(sParam5))
                {
                    questActionInfo.nParam5 = HUtil32.StrToInt(sParam5, 0);
                }
                if (HUtil32.IsStringNumber(sParam6))
                {
                    questActionInfo.nParam6 = HUtil32.StrToInt(sParam6, 0);
                }
                result = true;
            }
            return result;
        }

        private static string GetScriptCrossPath(string path)
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? path : path.Replace("\\", "/");
        }

        /// <summary>
        /// 加载NPC脚本
        /// </summary>
        /// <returns></returns>
        public void LoadScriptFile(INormNpc NPC, string sPatch, string sScritpName, bool boFlag)
        {
            string command = string.Empty;
            string questName = string.Empty;
            string s44 = string.Empty;
            string slabName = string.Empty;
            bool boDefine = false;
            ScriptInfo Script = null;
            SayingRecord SayingRecord = null;
            SayingProcedure SayingProcedure = null;
            int scriptType = 0;
            int questCount = 0;
            string sScritpFileName = SystemShare.GetEnvirFilePath(sPatch, GetScriptCrossPath($"{sScritpName}.txt"));
            if (File.Exists(sScritpFileName))
            {
                CallScriptDict.Clear();
                StringList stringList = new StringList();
                stringList.LoadFromFile(sScritpFileName);
                bool success = false;
                while (!success)
                {
                    LoadCallScript(ref stringList, ref success);
                }
                IList<DefineInfo> defineList = new List<DefineInfo>();
                string defline = LoadScriptDefineInfo(stringList, defineList);
                DefineInfo defineInfo = new DefineInfo { Name = "@HOME" };
                if (string.IsNullOrEmpty(defline))
                {
                    defline = "@main";
                }
                defineInfo.Text = defline;
                defineList.Add(defineInfo);
                int n24;
                // 常量处理
                for (int i = 0; i < stringList.Count; i++)
                {
                    string line = stringList[i].Trim();
                    if (!string.IsNullOrEmpty(line))
                    {
                        if (line[0] == '[')
                        {
                            boDefine = false;
                        }
                        else
                        {
                            if (line[0] == '#' && (HUtil32.CompareLStr(line, "#IF") || HUtil32.CompareLStr(line, "#ACT") || HUtil32.CompareLStr(line, "#ELSEACT")))
                            {
                                boDefine = true;
                            }
                            else
                            {
                                if (boDefine)
                                {
                                    // 将Define 好的常量换成指定值
                                    for (int n20 = 0; n20 < defineList.Count; n20++)
                                    {
                                        defineInfo = defineList[n20];
                                        int n1C = 0;
                                        while (true)
                                        {
                                            n24 = line.ToUpper().IndexOf(defineInfo.Name, StringComparison.OrdinalIgnoreCase);
                                            if (n24 <= 0)
                                            {
                                                break;
                                            }
                                            string s58 = line[..n24];
                                            string s5C = line[(defineInfo.Name.Length + n24)..];
                                            line = s58 + defineInfo.Text + s5C;
                                            stringList[i] = line;
                                            n1C++;
                                            if (n1C >= 10)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                // 释放常量定义内容
                for (int i = 0; i < defineList.Count; i++)
                {
                    defineList[i] = null;
                }
                defineList.Clear();
                int nQuestIdx = 0;
                for (int i = 0; i < stringList.Count; i++)
                {
                    string line = stringList[i].Trim();
                    if (string.IsNullOrEmpty(line) || line[0] == ';' || line[0] == '/')
                    {
                        continue;
                    }
                    if (scriptType == 0 && boFlag)
                    {
                        if (line.StartsWith("%"))// 物品价格倍率
                        {
                            line = line[1..];
                            int nPriceRate = HUtil32.StrToInt(line, -1);
                            if (nPriceRate >= 55)
                            {
                                ((IMerchant)NPC).PriceRate = nPriceRate;
                            }
                            continue;
                        }
                        if (line.StartsWith("+"))// 物品交易类型
                        {
                            line = line[1..];
                            int nItemType = HUtil32.StrToInt(line, -1);
                            if (nItemType >= 0)
                            {
                                ((IMerchant)NPC).ItemTypeList.Add(nItemType);
                            }
                            continue;
                        }
                        if (line.StartsWith("("))// 增加处理NPC可执行命令设置
                        {
                            HUtil32.ArrestStringEx(line, "(", ")", ref line);
                            if (!string.IsNullOrEmpty(line))
                            {
                                while (!string.IsNullOrEmpty(line))
                                {
                                    line = HUtil32.GetValidStr3(line, ref command, HUtil32.Separator);
                                    if (command.Equals(ScriptFlagConst.sBUY, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((IMerchant)NPC).IsBuy = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptFlagConst.sSELL, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((IMerchant)NPC).IsSell = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptFlagConst.sMAKEDURG, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((IMerchant)NPC).IsMakeDrug = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptFlagConst.sPRICES, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((IMerchant)NPC).IsPrices = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptFlagConst.sSTORAGE, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((IMerchant)NPC).IsStorage = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptFlagConst.sGETBACK, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((IMerchant)NPC).IsGetback = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptFlagConst.sUPGRADENOW, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((IMerchant)NPC).IsUpgradenow = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptFlagConst.sGETBACKUPGNOW, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((IMerchant)NPC).IsGetBackupgnow = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptFlagConst.sREPAIR, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((IMerchant)NPC).IsRepair = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptFlagConst.SuperRepair, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((IMerchant)NPC).IsSupRepair = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptFlagConst.sSL_SENDMSG, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((IMerchant)NPC).IsSendMsg = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptFlagConst.UseItemName, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((IMerchant)NPC).IsUseItemName = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptFlagConst.sOFFLINEMSG, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((IMerchant)NPC).IsOffLineMsg = true;
                                        continue;
                                    }
                                    if (string.Compare(command, ScriptFlagConst.sybdeal, StringComparison.OrdinalIgnoreCase) == 0)
                                    {
                                        ((IMerchant)NPC).IsYbDeal = true;
                                        continue;
                                    }
                                }
                            }
                            continue;
                        }
                        // 增加处理NPC可执行命令设置
                    }
                    string s38;
                    if (line.StartsWith("{"))
                    {
                        if (HUtil32.CompareLStr(line, "{Quest"))
                        {
                            s38 = HUtil32.GetValidStr3(line, ref questName, new[] { ' ', '}', '\t' });
                            HUtil32.GetValidStr3(s38, ref questName, new[] { ' ', '}', '\t' });
                            questCount = HUtil32.StrToInt(questName, 0);
                            Script = LoadMakeNewScript(NPC);
                            Script.QuestCount = questCount;
                            questCount++;
                        }
                        if (HUtil32.CompareLStr(line, "{~Quest"))
                        {
                            continue;
                        }
                    }
                    if (scriptType == 1 && Script != null && line.StartsWith("#"))
                    {
                        s38 = HUtil32.GetValidStr3(line, ref questName, new[] { '=', ' ', '\t' });
                        Script.IsQuest = true;
                        if (HUtil32.CompareLStr(line, "#IF"))
                        {
                            string questFlag = string.Empty;
                            HUtil32.ArrestStringEx(line, "[", "]", ref questFlag);
                            Script.QuestInfo[nQuestIdx].wFlag = HUtil32.StrToInt16(questFlag, 0);
                            HUtil32.GetValidStr3(s38, ref s44, new[] { '=', ' ', '\t' });
                            n24 = HUtil32.StrToInt(s44, 0);
                            if (n24 != 0)
                            {
                                n24 = 1;
                            }
                            Script.QuestInfo[nQuestIdx].btValue = (byte)n24;
                        }
                        if (HUtil32.CompareLStr(line, "#RAND"))
                        {
                            Script.QuestInfo[nQuestIdx].nRandRage = HUtil32.StrToInt(s44, 0);
                        }
                        continue;
                    }
                    if (line.StartsWith("["))
                    {
                        scriptType = 10;
                        if (Script == null)
                        {
                            Script = LoadMakeNewScript(NPC);
                            Script.QuestCount = questCount;
                        }
                        if (line.Equals("[goods]", StringComparison.OrdinalIgnoreCase))
                        {
                            scriptType = 20;
                            NPC.ProcessRefillIndex = SystemShare.CurrentMerchantIndex;
                            SystemShare.CurrentMerchantIndex++;
                            continue;
                        }
                        line = HUtil32.ArrestStringEx(line, "[", "]", ref slabName);
                        SayingRecord = new SayingRecord { sLabel = slabName };
                        line = HUtil32.GetValidStrCap(line, ref slabName, TextSpitConst);
                        if (slabName.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
                        {
                            SayingRecord.boExtJmp = true;
                        }
                        else
                        {
                            SayingRecord.boExtJmp = false;
                        }
                        SayingProcedure = new SayingProcedure();
                        SayingRecord.ProcedureList.Add(SayingProcedure);
                        if (Script.RecordList.ContainsKey(SayingRecord.sLabel))
                        {
                            SayingRecord.sLabel += SystemShare.RandomNumber.GetRandomNumber(1, 200);
                        }
                        Script.RecordList.Add(SayingRecord.sLabel, SayingRecord);
                        continue;
                    }
                    if (Script != null && SayingRecord != null)
                    {
                        if (line[0] == '#' && scriptType >= 10 && scriptType < 20)
                        {
                            if (line.Equals("#IF", StringComparison.OrdinalIgnoreCase))
                            {
                                if (SayingProcedure.ConditionList.Count > 0 || !string.IsNullOrEmpty(SayingProcedure.sSayMsg))
                                {
                                    SayingProcedure = new SayingProcedure();
                                    SayingRecord.ProcedureList.Add(SayingProcedure);
                                }
                                scriptType = 11;
                                continue;
                            }
                            if (line.Equals("#ACT", StringComparison.OrdinalIgnoreCase))
                            {
                                scriptType = 12;
                                continue;
                            }
                            if (line.Equals("#SAY", StringComparison.OrdinalIgnoreCase))
                            {
                                scriptType = 10;
                                continue;
                            }
                            if (line.Equals("#ELSEACT", StringComparison.OrdinalIgnoreCase))
                            {
                                scriptType = 13;
                                continue;
                            }
                            if (line.Equals("#ELSESAY", StringComparison.OrdinalIgnoreCase))
                            {
                                scriptType = 14;
                            }
                            continue;
                        }
                        switch (scriptType)
                        {
                            case 10:
                                SayingProcedure.sSayMsg += line;
                                break;
                            case 11:
                                {
                                    QuestConditionInfo questConditionInfo = new QuestConditionInfo();
                                    if (LoadScriptFileQuestCondition(line.Trim(), ref questConditionInfo))
                                    {
                                        SayingProcedure.ConditionList.Add(questConditionInfo);
                                    }
                                    else
                                    {
                                        LogService.Error("脚本错误: " + line + " 第:" + i + " 行: " + sScritpFileName);
                                    }
                                    break;
                                }
                            case 12:
                                {
                                    QuestActionInfo questActionInfo = new QuestActionInfo();
                                    if (LoadScriptFileQuestAction(line.Trim(), ref questActionInfo))
                                    {
                                        SayingProcedure.ActionList.Add(questActionInfo);
                                    }
                                    else
                                    {
                                        LogService.Error("脚本错误: " + line + " 第:" + i + " 行: " + sScritpFileName);
                                    }
                                    break;
                                }
                            case 13:
                                {
                                    QuestActionInfo questActionInfo = new QuestActionInfo();
                                    if (LoadScriptFileQuestAction(line.Trim(), ref questActionInfo))
                                    {
                                        SayingProcedure.ElseActionList.Add(questActionInfo);
                                    }
                                    else
                                    {
                                        LogService.Error("脚本错误: " + line + " 第:" + i + " 行: " + sScritpFileName);
                                    }
                                    break;
                                }
                            case 14:
                                SayingProcedure.sElseSayMsg += line;
                                break;
                        }
                    }
                    if (scriptType == 20 && boFlag)
                    {
                        string sItemName = string.Empty;
                        string sItemCount = string.Empty;
                        string sItemRefillTime = string.Empty;
                        line = HUtil32.GetValidStrCap(line, ref sItemName, TextSpitConst);
                        line = HUtil32.GetValidStrCap(line, ref sItemCount, TextSpitConst);
                        line = HUtil32.GetValidStrCap(line, ref sItemRefillTime, TextSpitConst);
                        if (!string.IsNullOrEmpty(sItemName) && !string.IsNullOrEmpty(sItemRefillTime))
                        {
                            if (sItemName[0] == '\"')
                            {
                                HUtil32.ArrestStringEx(sItemName, "\"", "\"", ref sItemName);
                            }
                            Goods goods = new Goods
                            {
                                ItemName = sItemName,
                                Count = HUtil32.StrToInt(sItemCount, 0),
                                RefillTime = HUtil32.StrToInt(sItemRefillTime, 0),
                                RefillTick = 0
                            };
                            if (SystemShare.CanSellItem(sItemName))
                            {
                                ((IMerchant)NPC).RefillGoodsList.Add(goods);
                            }
                        }
                    }
                }
                stringList.Dispose();
            }
            else
            {
                LogService.Error("Script file not found: " + sScritpFileName);
            }
        }

        /// <summary>
        /// 格式化标签
        /// </summary>
        /// <param name="sLabel"></param>
        /// <param name="boChange"></param>
        /// <returns></returns>
        private static string FormatLabelStr(string sLabel, ref bool boChange)
        {
            string result = sLabel;
            if (sLabel.IndexOf(")", StringComparison.OrdinalIgnoreCase) > -1)
            {
                HUtil32.GetValidStr3(sLabel, ref result, '(');
                boChange = true;
            }
            return result;
        }

        /// <summary>
        /// 初始化脚本处理方法
        /// </summary>
        /// <returns></returns>
        protected string InitializeProcedure(string sMsg)
        {
            int nC = 0;
            string sCmd = string.Empty;
            string tempstr = sMsg;
            while (true)
            {
                if (tempstr.IndexOf(">", StringComparison.OrdinalIgnoreCase) < -1)
                {
                    break;
                }
                tempstr = HUtil32.ArrestStringEx(tempstr, "<", ">", ref sCmd);
                if (!string.IsNullOrEmpty(sCmd))
                {
                    if (sCmd[0] == '$')
                    {
                        InitializeVariable(sCmd, ref sMsg);
                    }
                }
                else
                {
                    break;
                }
                nC++;
                if (nC > 100)
                {
                    break;
                }
            }
            return sMsg;
        }

        /// <summary>
        /// 初始化NPC显示内容
        /// </summary>
        /// <param name="sMsg">标签内容</param>
        /// <param name="StringList">新标签列表</param>
        /// <param name="OldStringList">老标签列表</param>
        /// <param name="ScriptNameList">标签列表</param>
        /// <returns></returns>
        private string InitializeSayMsg(string sMsg, List<string> StringList, IList<string> OldStringList, IList<string> ScriptNameList)
        {
            int nC = 0;
            string s10 = string.Empty;
            string tempstr = sMsg;
            string sLabel;
            string sname = string.Empty;
            int nIdx;
            int nChangeIndex = 1;
            int nNotIdx = -1;
            bool boChange = false;
            bool boAddResetLabel = false;
            while (true)
            {
                if (string.IsNullOrEmpty(tempstr))
                {
                    break;
                }
                if (tempstr.IndexOf('>') <= 0)
                {
                    break;
                }
                tempstr = HUtil32.ArrestStringEx(tempstr, "<", ">", ref s10);
                if (!string.IsNullOrEmpty(s10))
                {
                    if (s10.IndexOf("/", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        sLabel = HUtil32.GetValidStr3(s10, ref sname, '/');
                        if (string.Compare(sLabel, "@close", StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            continue;
                        }
                        if (string.Compare(sLabel, "@Exit", StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            continue;
                        }
                        if (string.Compare(sLabel, "@main", StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            continue;
                        }
                        if (HUtil32.CompareLStr(sLabel, "FCOLOR", 6))
                        {
                            continue;
                        }
                        if (HUtil32.CompareLStr(sLabel, "@Move(", 6))
                        {
                            continue;
                        }
                        if (HUtil32.CompareLStr(sLabel, "~@", 2))
                        {
                            continue;
                        }
                        if (HUtil32.CompareLStr(sLabel, "@@", 2))
                        {
                            if (!boAddResetLabel)
                            {
                                boAddResetLabel = true;
                                sMsg = ScriptFlagConst.RESETLABEL + sMsg;
                            }
                            continue;
                        }
                        nIdx = ScriptNameList.IndexOf(FormatLabelStr(sLabel, ref boChange));
                        if (nIdx == -1)
                        {
                            nIdx = nNotIdx;
                            nNotIdx -= 1;
                        }
                        else if (boChange)
                        {
                            nIdx = nChangeIndex * 100000 + nIdx;
                            nChangeIndex++;
                        }
                        OldStringList.Add(sLabel);
                        try
                        {
                            if (sLabel.Length >= 2 && sLabel[1] == '@' && sLabel[0] == '@')
                            {
                                sLabel = "@@" + nIdx;
                            }
                            else
                            {
                                sLabel = "@" + nIdx;
                            }
                        }
                        finally
                        {
                            StringList.Add(sLabel);
                        }
                        sMsg = sMsg.Replace("<" + s10 + ">", "<" + sname + "/" + sLabel + ">");
                    }
                    else if (s10[0] == '$')
                    {
                        //游戏变量处理
                        InitializeVariable(s10, ref sMsg);
                    }
                }
                else
                {
                    break;
                }
                nC++;
                if (nC >= 100)
                {
                    break;
                }
            }
            return sMsg;
        }

        /// <summary>
        /// 初始化全局变量脚本
        /// </summary>
        private static void InitializeVariable(string sLabel, ref string sMsg)
        {
            string s14 = string.Empty;
            string sLabel2 = sLabel.ToUpper();
            if (sLabel2 == GrobalVarCode.sVAR_SERVERNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_SERVERNAME);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_SERVERIP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_SERVERIP);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_WEBSITE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_WEBSITE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_BBSSITE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_BBSSITE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CLIENTDOWNLOAD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CLIENTDOWNLOAD);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_QQ)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_QQ);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_PHONE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_PHONE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_BANKACCOUNT0)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_BANKACCOUNT0);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_BANKACCOUNT1)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_BANKACCOUNT1);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_BANKACCOUNT2)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_BANKACCOUNT2);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_BANKACCOUNT3)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_BANKACCOUNT3);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_BANKACCOUNT4)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_BANKACCOUNT4);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_BANKACCOUNT5)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_BANKACCOUNT5);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_BANKACCOUNT6)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_BANKACCOUNT6);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_BANKACCOUNT7)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_BANKACCOUNT7);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_BANKACCOUNT8)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_BANKACCOUNT8);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_BANKACCOUNT9)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_BANKACCOUNT9);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_GAMEGOLDNAME)
            {
                //sMsg = sMsg.Replace("<" + sLabel + ">", Grobal2.sSTRING_GAMEGOLD);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_GAMEPOINTNAME)
            {
                // sMsg = sMsg.Replace("<" + sLabel + ">", Grobal2.sSTRING_GAMEPOINT);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_USERCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_USERCOUNT);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_DATETIME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_DATETIME);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_USERNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_USERNAME);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_FBMAPNAME)
            { //副本
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_FBMAPNAME);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_FBMAP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_FBMAP);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_ACCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_ACCOUNT);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_ASSEMBLEITEMNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_ASSEMBLEITEMNAME);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_MAPNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_MAPNAME);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_GUILDNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_GUILDNAME);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_RANKNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_RANKNAME);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_LEVEL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_LEVEL);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_HP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_HP);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_MAXHP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_MAXHP);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_MP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_MP);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_MAXMP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_MAXMP);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_AC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_AC);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_MAXAC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_MAXAC);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_MAC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_MAC);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_MAXMAC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_MAXMAC);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_DC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_DC);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_MAXDC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_MAXDC);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_MC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_MC);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_MAXMC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_MAXMC);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_SC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_SC);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_MAXSC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_MAXSC);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_EXP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_EXP);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_MAXEXP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_MAXEXP);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_PKPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_PKPOINT);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CREDITPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CREDITPOINT);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_GOLDCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_GOLDCOUNT);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_GAMEGOLD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_GAMEGOLD);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_GAMEPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_GAMEPOINT);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_LOGINTIME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_LOGINTIME);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_LOGINLONG)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_LOGINLONG);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_DRESS)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_DRESS);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_WEAPON)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_WEAPON);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_RIGHTHAND)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_RIGHTHAND);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_HELMET)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_HELMET);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_NECKLACE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_NECKLACE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_RING_R)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_RING_R);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_RING_L)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_RING_L);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_ARMRING_R)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_ARMRING_R);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_ARMRING_L)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_ARMRING_L);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_BUJUK)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_BUJUK);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_BELT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_BELT);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_BOOTS)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_BOOTS);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CHARM)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CHARM);
            }
            //=======================================没有用到的==============================
            else if (sLabel2 == GrobalVarCode.sVAR_HOUSE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_HOUSE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CIMELIA)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CIMELIA);
            }
            //=======================================没有用到的==============================
            else if (sLabel2 == GrobalVarCode.sVAR_IPADDR)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_IPADDR);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_IPLOCAL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_IPLOCAL);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_GUILDBUILDPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_GUILDBUILDPOINT);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_GUILDAURAEPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_GUILDAURAEPOINT);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_GUILDSTABILITYPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_GUILDSTABILITYPOINT);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_GUILDFLOURISHPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_GUILDFLOURISHPOINT);
            }
            //=================================没用用到的====================================
            else if (sLabel2 == GrobalVarCode.sVAR_GUILDMONEYCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_GUILDMONEYCOUNT);
            }
            //=================================没用用到的结束====================================
            else if (sLabel2 == GrobalVarCode.sVAR_REQUESTCASTLEWARITEM)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_REQUESTCASTLEWARITEM);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_REQUESTCASTLEWARDAY)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_REQUESTCASTLEWARDAY);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_REQUESTBUILDGUILDITEM)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_REQUESTBUILDGUILDITEM);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_OWNERGUILD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_OWNERGUILD);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CASTLENAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CASTLENAME);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_LORD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_LORD);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_GUILDWARFEE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_GUILDWARFEE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_BUILDGUILDFEE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_BUILDGUILDFEE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CASTLEWARDATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CASTLEWARDATE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_LISTOFWAR)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_LISTOFWAR);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CASTLECHANGEDATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CASTLECHANGEDATE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CASTLEWARLASTDATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CASTLEWARLASTDATE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CASTLEGETDAYS)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CASTLEGETDAYS);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_DATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_DATE);
            }
            //===================================没用用到的======================================
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_PRVMSG)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_PRVMSG);
            }
            //===================================没用用到的结束======================================
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_ALLOWMSG)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_ALLOWMSG);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_LETSHOUT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_LETSHOUT);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_LETTRADE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_LETTRADE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_LETGuild)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_LETGuild);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_ENDGUILD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_ENDGUILD);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_BANGUILDCHAT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_BANGUILDCHAT);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_AUTHALLY)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_AUTHALLY);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_AUTH)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_AUTH);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_AUTHCANCEL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_AUTHCANCEL);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_USERMOVE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_USERMOVE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_SEARCHING)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_SEARCHING);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_ALLOWGROUPCALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_ALLOWGROUPCALL);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_GROUPRECALLL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_GROUPRECALLL);
            }
            #region 没有使用的
            //===========================================没有使用的========================================
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_ALLOWGUILDRECALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_ALLOWGUILDRECALL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLOWGUILDRECALL, SctiptDef.sVAR_CMD_ALLOWGUILDRECALL);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_GUILDRECALLL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_GUILDRECALLL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_GUILDRECALLL, SctiptDef.sVAR_CMD_GUILDRECALLL);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_DEAR)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_DEAR);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_DEAR, SctiptDef.sVAR_CMD_DEAR);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_ALLOWDEARRCALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_ALLOWDEARRCALL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLOWDEARRCALL, SctiptDef.sVAR_CMD_ALLOWDEARRCALL);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_DEARRECALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_DEARRECALL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_DEARRECALL, SctiptDef.sVAR_CMD_DEARRECALL);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_MASTER)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_MASTER);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_MASTER, SctiptDef.sVAR_CMD_MASTER);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_ALLOWMASTERRECALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_ALLOWMASTERRECALL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLOWMASTERRECALL, SctiptDef.sVAR_CMD_ALLOWMASTERRECALL);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_MASTERECALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_MASTERECALL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_MASTERECALL, SctiptDef.sVAR_CMD_MASTERECALL);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_TAKEONHORSE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_TAKEONHORSE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_TAKEONHORSE, SctiptDef.sVAR_CMD_TAKEONHORSE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_TAKEOFHORSE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_TAKEOFHORSE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_TAKEOFHORSE, SctiptDef.sVAR_CMD_TAKEOFHORSE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_ALLSYSMSG)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_ALLSYSMSG);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLSYSMSG, SctiptDef.sVAR_CMD_ALLSYSMSG);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_MEMBERFUNCTION)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_MEMBERFUNCTION);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_MEMBERFUNCTION, SctiptDef.sVAR_CMD_MEMBERFUNCTION);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_MEMBERFUNCTIONEX)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_MEMBERFUNCTIONEX);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_MEMBERFUNCTIONEX, SctiptDef.sVAR_CMD_MEMBERFUNCTIONEX);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CASTLEGOLD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CASTLEGOLD);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CASTLEGOLD, SctiptDef.sVAR_CASTLEGOLD);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_TODAYINCOME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_TODAYINCOME);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_TODAYINCOME, SctiptDef.sVAR_TODAYINCOME);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CASTLEDOORSTATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CASTLEDOORSTATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CASTLEDOORSTATE, SctiptDef.sVAR_CASTLEDOORSTATE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_REPAIRDOORGOLD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_REPAIRDOORGOLD);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_REPAIRDOORGOLD, SctiptDef.sVAR_REPAIRDOORGOLD);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_REPAIRWALLGOLD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_REPAIRWALLGOLD);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_REPAIRWALLGOLD, SctiptDef.sVAR_REPAIRWALLGOLD);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_GUARDFEE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_GUARDFEE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_GUARDFEE, SctiptDef.sVAR_GUARDFEE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_ARCHERFEE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_ARCHERFEE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_ARCHERFEE, SctiptDef.sVAR_ARCHERFEE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_GUARDRULE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_GUARDRULE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_GUARDRULE, SctiptDef.sVAR_GUARDRULE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_STORAGE2STATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_STORAGE2STATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_STORAGE2STATE, SctiptDef.sVAR_STORAGE2STATE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_STORAGE3STATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_STORAGE3STATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_STORAGE3STATE, SctiptDef.sVAR_STORAGE3STATE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_STORAGE4STATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_STORAGE4STATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_STORAGE4STATE, SctiptDef.sVAR_STORAGE4STATE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_STORAGE5STATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_STORAGE5STATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_STORAGE5STATE, SctiptDef.sVAR_STORAGE5STATE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_SELFNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_SELFNAME);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_SELFNAME, SctiptDef.sVAR_SELFNAME);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_POSENAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_POSENAME);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_POSENAME, SctiptDef.sVAR_POSENAME);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_GAMEDIAMOND)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_GAMEDIAMOND);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_GAMEDIAMOND, SctiptDef.sVAR_GAMEDIAMOND);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_GAMEGIRD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_GAMEGIRD);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_GAMEGIRD, SctiptDef.sVAR_GAMEGIRD);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_ALLOWFIREND)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_ALLOWFIREND);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLOWFIREND, SctiptDef.sVAR_CMD_ALLOWFIREND);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_EFFIGYSTATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_EFFIGYSTATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_EFFIGYSTATE, SctiptDef.sVAR_EFFIGYSTATE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_EFFIGYOFFSET)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_EFFIGYOFFSET);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_EFFIGYOFFSET, SctiptDef.sVAR_EFFIGYOFFSET);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_YEAR)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_YEAR);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_YEAR, SctiptDef.sVAR_YEAR);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_MONTH)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_MONTH);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_MONTH, SctiptDef.sVAR_MONTH);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_DAY)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_DAY);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_DAY, SctiptDef.sVAR_DAY);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_HOUR)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_HOUR);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_HOUR, SctiptDef.sVAR_HOUR);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_MINUTE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_MINUTE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_MINUTE, SctiptDef.sVAR_MINUTE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_SEC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_SEC);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_SEC, SctiptDef.sVAR_SEC);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_MAP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_MAP);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_MAP, SctiptDef.sVAR_MAP);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_X)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_X);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_X, SctiptDef.sVAR_X);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_Y)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_Y);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_Y, SctiptDef.sVAR_Y);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_UNMASTER_FORCE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_UNMASTER_FORCE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_UNMASTER_FORCE, SctiptDef.sVAR_UNMASTER_FORCE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_USERGOLDCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_USERGOLDCOUNT);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_USERGOLDCOUNT, SctiptDef.sVAR_USERGOLDCOUNT);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_MAXGOLDCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_MAXGOLDCOUNT);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_MAXGOLDCOUNT, SctiptDef.sVAR_MAXGOLDCOUNT);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_STORAGEGOLDCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_STORAGEGOLDCOUNT);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_STORAGEGOLDCOUNT, SctiptDef.sVAR_STORAGEGOLDCOUNT);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_BINDGOLDCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_BINDGOLDCOUNT);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_BINDGOLDCOUNT, SctiptDef.sVAR_BINDGOLDCOUNT);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_UPGRADEWEAPONFEE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_UPGRADEWEAPONFEE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_UPGRADEWEAPONFEE, SctiptDef.sVAR_UPGRADEWEAPONFEE);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_USERWEAPON)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_USERWEAPON);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_USERWEAPON, SctiptDef.sVAR_USERWEAPON);
            }
            else if (sLabel2 == GrobalVarCode.sVAR_CMD_STARTQUEST)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCode.tVAR_CMD_STARTQUEST);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_STARTQUEST, SctiptDef.sVAR_CMD_STARTQUEST);
            }
            //===========================================没有使用的========================================
            #endregion
            else if (HUtil32.CompareLStr(sLabel2, GrobalVarCode.sVAR_TEAM))
            {
                s14 = sLabel2.Substring(GrobalVarCode.sVAR_TEAM.Length + 1 - 1, 1);
                if (!string.IsNullOrEmpty(s14))
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(GrobalVarCode.tVAR_TEAM, s14));
                }
                else
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, GrobalVarCode.sVAR_HUMAN))
            {
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (!string.IsNullOrEmpty(s14))
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(GrobalVarCode.tVAR_HUMAN, s14));
                }
                else
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, GrobalVarCode.sVAR_GUILD))
            {
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (!string.IsNullOrEmpty(s14))
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(GrobalVarCode.tVAR_GUILD, s14));
                }
                else
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, GrobalVarCode.sVAR_GLOBAL))
            {
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (!string.IsNullOrEmpty(s14))
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(GrobalVarCode.tVAR_GLOBAL, s14));
                }
                else
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, GrobalVarCode.sVAR_STR))
            {
                //'欢迎使用个人银行储蓄，目前完全免费，请多利用。\ \<您的个人银行存款有/@-1>：<$46><｜/@-2><$125/G18>\ \<您的包裹里以携带有/AUTOCOLOR=249>：<$GOLDCOUNT><｜/@-2><$GOLDCOUNTX>\ \ \<存入金币/@@InPutInteger1>      <取出金币/@@InPutInteger2>      <返 回/@Main>'
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (!string.IsNullOrEmpty(s14))
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(GrobalVarCode.tVAR_STR, s14));
                }
                else
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, GrobalVarCode.sVAR_MISSIONARITHMOMETER))
            {
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (!string.IsNullOrEmpty(s14))
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(GrobalVarCode.tVAR_MISSIONARITHMOMETER, s14));
                }
                else
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", "????");
            }
        }
    }
}