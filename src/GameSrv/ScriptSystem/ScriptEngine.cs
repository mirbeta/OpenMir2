using GameSrv.Npc;
using GameSrv.ScriptSystem;
using NLog;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using SystemModule.Common;

namespace GameSrv.Script
{
    public partial class ScriptEngine {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<string, string> CallScriptDict = new Dictionary<string, string>();
        private readonly char[] TextSpitConst = new[] { ' ', '\t' };

        public void LoadScript(NormNpc NPC, string sPatch, string sScritpName) {
            if (string.IsNullOrEmpty(sPatch)) {
                sPatch = ScriptConst.sNpc_def;
            }
            LoadScriptFile(NPC, sPatch, sScritpName, false); ;
        }

        private static bool LoadScriptCallScript(string sFileName, string sLabel, StringList List) {
            var result = false;
            if (File.Exists(sFileName)) {
                var callStrList = new StringList();
                callStrList.LoadFromFile(sFileName);
                sLabel = '[' + sLabel + ']';
                var findLab = false;
                for (var i = 0; i < callStrList.Count; i++)
                {
                    var sLine = callStrList[i].Trim();
                    if (!string.IsNullOrEmpty(sLine))
                    {
                        if (!findLab) {
                            if (sLine[0] == '[' && string.Compare(sLine, sLabel, StringComparison.OrdinalIgnoreCase) == 0) {
                                findLab = true;
                                continue;
                            }
                        }
                        if (sLine[0] != '{') {
                            if (sLine[0] == '}') {
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

        private static int GetScriptCallCount(string sText) {
            return RegexCallCount().Count(sText);
        }

        private static string GetCallScriptPath(string path) {
            var sCallScriptFile = path;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                if (sCallScriptFile.StartsWith("\\\\")) {
                    sCallScriptFile = sCallScriptFile.Remove(0, 2);
                }
                else if (sCallScriptFile.StartsWith("\\")) {
                    sCallScriptFile = sCallScriptFile.Remove(0, 1);
                }
            }
            else {
                if (sCallScriptFile.StartsWith("\\\\")) {
                    sCallScriptFile = sCallScriptFile.Remove(0, 2);
                }
                else if (sCallScriptFile.StartsWith("\\")) {
                    sCallScriptFile = sCallScriptFile.Remove(0, 1);
                }
                sCallScriptFile = sCallScriptFile.Replace("\\", "/");
            }
            return sCallScriptFile;
        }

        private void LoadCallScript(ref StringList LoadList, ref bool success)
        {
            var callCount = GetScriptCallCount(LoadList.Text);
            if (callCount <= 0) {
                success = true;
                return;
            }
            var sLable = string.Empty;
            var callList = new StringList(1024);
            for (var i = 0; i < LoadList.Count; i++)
            {
                var sLine = LoadList[i].Trim();
                if (!string.IsNullOrEmpty(sLine) && sLine[0] == '#' && HUtil32.CompareLStr(sLine, "#CALL"))
                {
                    sLine = HUtil32.ArrestStringEx(sLine, "[", "]", ref sLable);
                    var sCallScriptFile = GetCallScriptPath(sLable.Trim());
                    var sLabName = sLine.Trim();
                    var sFileName = M2Share.GetEnvirFilePath("QuestDiary", sCallScriptFile);
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
                        _logger.Error("script error, load fail: " + sCallScriptFile + sLabName);
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

        private string LoadScriptDefineInfo(StringList stringList, ICollection<DefineInfo> List) {
            var result = string.Empty;
            var defFile = string.Empty;
            var defineName = string.Empty;
            var defText = string.Empty;
            for (var i = 0; i < stringList.Count; i++) {
                var line = stringList[i].Trim();
                if (!string.IsNullOrEmpty(line) && line[0] == '#') {
                    if (HUtil32.CompareLStr(line, "#SETHOME"))
                    {
                        result = HUtil32.GetValidStr3(line, ref defFile, TextSpitConst).Trim();
                        stringList[i] = "";
                        continue;
                    }
                    if (HUtil32.CompareLStr(line, "#DEFINE")) {
                        line = HUtil32.GetValidStr3(line, ref defFile, TextSpitConst);
                        line = HUtil32.GetValidStr3(line, ref defineName, TextSpitConst);
                        line = HUtil32.GetValidStr3(line, ref defText, TextSpitConst);
                        var DefineInfo = new DefineInfo { Name = defineName.ToUpper(), Text = defText };
                        List.Add(DefineInfo);
                        stringList[i] = "";
                        continue;
                    }
                    if (HUtil32.CompareLStr(line, "#INCLUDE"))
                    {
                        var definesFile = HUtil32.GetValidStr3(line, ref defFile, TextSpitConst).Trim();
                        definesFile = M2Share.GetEnvirFilePath("Defines", definesFile);
                        if (File.Exists(definesFile))
                        {
                            using var LoadStrList = new StringList();
                            LoadStrList.LoadFromFile(definesFile);
                            result = LoadScriptDefineInfo(LoadStrList, List);
                        }
                        else
                        {
                            _logger.Error("script error, load fail: " + definesFile);
                        }
                        stringList[i] = "";
                        continue;
                    }
                }
            }
            return result;
        }

        private static ScriptInfo LoadMakeNewScript(NormNpc NPC)
        {
            var scriptInfo = new ScriptInfo
            {
                IsQuest = false,
                RecordList = new Dictionary<string, SayingRecord>(StringComparer.OrdinalIgnoreCase)
            };
            NPC.m_ScriptList.Add(scriptInfo);
            return scriptInfo;
        }

        private bool LoadScriptFileQuestCondition(string sText, ref QuestConditionInfo QuestConditionInfo) {
            var result = false;
            var sCmd = string.Empty;
            var sParam1 = string.Empty;
            var sParam2 = string.Empty;
            var sParam3 = string.Empty;
            var sParam4 = string.Empty;
            var sParam5 = string.Empty;
            var sParam6 = string.Empty;
            var nCMDCode = 0;
            sText = HUtil32.GetValidStrCap(sText, ref sCmd, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam1, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam2, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam3, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam4, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam5, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam6, TextSpitConst);
            if (sCmd.IndexOf(".", StringComparison.OrdinalIgnoreCase) > -1) //支持脚本变量
            {
                var sActName = string.Empty;
                sCmd = HUtil32.GetValidStrCap(sCmd, ref sActName, '.');
                if (!string.IsNullOrEmpty(sActName)) {
                    QuestConditionInfo.sOpName = sActName;
                    if (".".IndexOf(sCmd, StringComparison.OrdinalIgnoreCase) > -1) {
                        sCmd = HUtil32.GetValidStrCap(sCmd, ref sActName, '.');
                        if (string.Compare(sActName, "H", StringComparison.OrdinalIgnoreCase) == 0) {
                            QuestConditionInfo.sOpHName = "H";
                        }
                    }
                }
            }
            sCmd = sCmd.ToUpper();
            switch (sCmd) {
                case ConditionCodeDef.sCHECK: {
                        nCMDCode = ConditionCodeDef.nCHECK;
                        HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                        if (!HUtil32.IsStringNumber(sParam1)) {
                            nCMDCode = 0;
                        }
                        if (!HUtil32.IsStringNumber(sParam2)) {
                            nCMDCode = 0;
                        }
                        goto L001;
                    }
                case ConditionCodeDef.sCHECKOPEN: {
                        nCMDCode = ConditionCodeDef.nCHECKOPEN;
                        HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                        if (!HUtil32.IsStringNumber(sParam1)) {
                            nCMDCode = 0;
                        }
                        if (!HUtil32.IsStringNumber(sParam2)) {
                            nCMDCode = 0;
                        }
                        goto L001;
                    }
                case ConditionCodeDef.sCHECKUNIT: {
                        nCMDCode = ConditionCodeDef.nCHECKUNIT;
                        HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                        if (!HUtil32.IsStringNumber(sParam1)) {
                            nCMDCode = 0;
                        }
                        if (!HUtil32.IsStringNumber(sParam2)) {
                            nCMDCode = 0;
                        }
                        goto L001;
                    }
                case ConditionCodeDef.sCHECKPKPOINT:
                    nCMDCode = ConditionCodeDef.nCHECKPKPOINT;
                    goto L001;
                case ConditionCodeDef.sCHECKGOLD:
                    nCMDCode = ConditionCodeDef.nCHECKGOLD;
                    goto L001;
                case ConditionCodeDef.sCHECKLEVEL:
                    nCMDCode = ConditionCodeDef.nCHECKLEVEL;
                    goto L001;
                case ConditionCodeDef.sCHECKJOB:
                    nCMDCode = ConditionCodeDef.nCHECKJOB;
                    goto L001;
                case ConditionCodeDef.sRANDOM:
                    nCMDCode = ConditionCodeDef.nRANDOM;
                    goto L001;
                case ConditionCodeDef.sCHECKITEM:
                    nCMDCode = ConditionCodeDef.nCHECKITEM;
                    goto L001;
                case ConditionCodeDef.sGENDER:
                    nCMDCode = ConditionCodeDef.nGENDER;
                    goto L001;
                case ConditionCodeDef.sCHECKBAGGAGE:
                    nCMDCode = ConditionCodeDef.nCHECKBAGGAGE;
                    goto L001;
                case ConditionCodeDef.sCHECKNAMELIST:
                    nCMDCode = ConditionCodeDef.nCHECKNAMELIST;
                    goto L001;
                case ConditionCodeDef.sSC_HASGUILD:
                    nCMDCode = ConditionCodeDef.nSC_HASGUILD;
                    goto L001;
                case ConditionCodeDef.sSC_ISGUILDMASTER:
                    nCMDCode = ConditionCodeDef.nSC_ISGUILDMASTER;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKCASTLEMASTER:
                    nCMDCode = ConditionCodeDef.nSC_CHECKCASTLEMASTER;
                    goto L001;
                case ConditionCodeDef.sSC_ISNEWHUMAN:
                    nCMDCode = ConditionCodeDef.nSC_ISNEWHUMAN;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKMEMBERTYPE:
                    nCMDCode = ConditionCodeDef.nSC_CHECKMEMBERTYPE;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKMEMBERLEVEL:
                    nCMDCode = ConditionCodeDef.nSC_CHECKMEMBERLEVEL;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKGAMEGOLD:
                    nCMDCode = ConditionCodeDef.nSC_CHECKGAMEGOLD;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKGAMEPOINT:
                    nCMDCode = ConditionCodeDef.nSC_CHECKGAMEPOINT;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKNAMELISTPOSITION:
                    nCMDCode = ConditionCodeDef.nSC_CHECKNAMELISTPOSITION;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKGUILDLIST:
                    nCMDCode = ConditionCodeDef.nSC_CHECKGUILDLIST;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKRENEWLEVEL:
                    nCMDCode = ConditionCodeDef.nSC_CHECKRENEWLEVEL;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKSLAVELEVEL:
                    nCMDCode = ConditionCodeDef.nSC_CHECKSLAVELEVEL;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKSLAVENAME:
                    nCMDCode = ConditionCodeDef.nSC_CHECKSLAVENAME;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKCREDITPOINT:
                    nCMDCode = ConditionCodeDef.nSC_CHECKCREDITPOINT;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKOFGUILD:
                    nCMDCode = ConditionCodeDef.nSC_CHECKOFGUILD;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKPAYMENT:
                    nCMDCode = ConditionCodeDef.nSC_CHECKPAYMENT;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKUSEITEM:
                    nCMDCode = ConditionCodeDef.nSC_CHECKUSEITEM;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKBAGSIZE:
                    nCMDCode = ConditionCodeDef.nSC_CHECKBAGSIZE;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKLISTCOUNT:
                    nCMDCode = ConditionCodeDef.nSC_CHECKLISTCOUNT;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKDC:
                    nCMDCode = ConditionCodeDef.nSC_CHECKDC;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKMC:
                    nCMDCode = ConditionCodeDef.nSC_CHECKMC;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKSC:
                    nCMDCode = ConditionCodeDef.nSC_CHECKSC;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKHP:
                    nCMDCode = ConditionCodeDef.nSC_CHECKHP;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKMP:
                    nCMDCode = ConditionCodeDef.nSC_CHECKMP;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKITEMTYPE:
                    nCMDCode = ConditionCodeDef.nSC_CHECKITEMTYPE;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKEXP:
                    nCMDCode = ConditionCodeDef.nSC_CHECKEXP;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKCASTLEGOLD:
                    nCMDCode = ConditionCodeDef.nSC_CHECKCASTLEGOLD;
                    goto L001;
                case ConditionCodeDef.sSC_PASSWORDERRORCOUNT:
                    nCMDCode = ConditionCodeDef.nSC_PASSWORDERRORCOUNT;
                    goto L001;
                case ConditionCodeDef.sSC_ISLOCKPASSWORD:
                    nCMDCode = ConditionCodeDef.nSC_ISLOCKPASSWORD;
                    goto L001;
                case ConditionCodeDef.sSC_ISLOCKSTORAGE:
                    nCMDCode = ConditionCodeDef.nSC_ISLOCKSTORAGE;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKBUILDPOINT:
                    nCMDCode = ConditionCodeDef.nSC_CHECKBUILDPOINT;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKAURAEPOINT:
                    nCMDCode = ConditionCodeDef.nSC_CHECKAURAEPOINT;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKSTABILITYPOINT:
                    nCMDCode = ConditionCodeDef.nSC_CHECKSTABILITYPOINT;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKFLOURISHPOINT:
                    nCMDCode = ConditionCodeDef.nSC_CHECKFLOURISHPOINT;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKCONTRIBUTION:
                    nCMDCode = ConditionCodeDef.nSC_CHECKCONTRIBUTION;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKRANGEMONCOUNT:
                    nCMDCode = ConditionCodeDef.nSC_CHECKRANGEMONCOUNT;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKITEMADDVALUE:
                    nCMDCode = ConditionCodeDef.nSC_CHECKITEMADDVALUE;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKINMAPRANGE:
                    nCMDCode = ConditionCodeDef.nSC_CHECKINMAPRANGE;
                    goto L001;
                case ConditionCodeDef.sSC_CASTLECHANGEDAY:
                    nCMDCode = ConditionCodeDef.nSC_CASTLECHANGEDAY;
                    goto L001;
                case ConditionCodeDef.sSC_CASTLEWARDAY:
                    nCMDCode = ConditionCodeDef.nSC_CASTLEWARDAY;
                    goto L001;
                case ConditionCodeDef.sSC_ONLINELONGMIN:
                    nCMDCode = ConditionCodeDef.nSC_ONLINELONGMIN;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKGUILDCHIEFITEMCOUNT:
                    nCMDCode = ConditionCodeDef.nSC_CHECKGUILDCHIEFITEMCOUNT;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKNAMEDATELIST:
                    nCMDCode = ConditionCodeDef.nSC_CHECKNAMEDATELIST;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKMAPHUMANCOUNT:
                    nCMDCode = ConditionCodeDef.nSC_CHECKMAPHUMANCOUNT;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKMAPMONCOUNT:
                    nCMDCode = ConditionCodeDef.nSC_CHECKMAPMONCOUNT;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKVAR:
                    nCMDCode = ConditionCodeDef.nSC_CHECKVAR;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKSERVERNAME:
                    nCMDCode = ConditionCodeDef.nSC_CHECKSERVERNAME;
                    goto L001;
                case ConditionCodeDef.sSC_ISATTACKGUILD:
                    nCMDCode = ConditionCodeDef.nSC_ISATTACKGUILD;
                    goto L001;
                case ConditionCodeDef.sSC_ISDEFENSEGUILD:
                    nCMDCode = ConditionCodeDef.nSC_ISDEFENSEGUILD;
                    goto L001;
                case ConditionCodeDef.sSC_ISATTACKALLYGUILD:
                    nCMDCode = ConditionCodeDef.nSC_ISATTACKALLYGUILD;
                    goto L001;
                case ConditionCodeDef.sSC_ISDEFENSEALLYGUILD:
                    nCMDCode = ConditionCodeDef.nSC_ISDEFENSEALLYGUILD;
                    goto L001;
                case ConditionCodeDef.sSC_ISCASTLEGUILD:
                    nCMDCode = ConditionCodeDef.nSC_ISCASTLEGUILD;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKCASTLEDOOR:
                    nCMDCode = ConditionCodeDef.nSC_CHECKCASTLEDOOR;
                    goto L001;
                case ConditionCodeDef.sSC_ISSYSOP:
                    nCMDCode = ConditionCodeDef.nSC_ISSYSOP;
                    goto L001;
                case ConditionCodeDef.sSC_ISADMIN:
                    nCMDCode = ConditionCodeDef.nSC_ISADMIN;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKGROUPCOUNT:
                    nCMDCode = ConditionCodeDef.nSC_CHECKGROUPCOUNT;
                    goto L001;
                case ConditionCodeDef.sCHECKACCOUNTLIST:
                    nCMDCode = ConditionCodeDef.nCHECKACCOUNTLIST;
                    goto L001;
                case ConditionCodeDef.sCHECKIPLIST:
                    nCMDCode = ConditionCodeDef.nCHECKIPLIST;
                    goto L001;
                case ConditionCodeDef.sCHECKBBCOUNT:
                    nCMDCode = ConditionCodeDef.nCHECKBBCOUNT;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKDLGITEMTYPE:
                    nCMDCode = ConditionCodeDef.nSC_CHECKDLGITEMTYPE;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKDLGITEMNAME:
                    nCMDCode = ConditionCodeDef.nSC_CHECKDLGITEMNAME;
                    goto L001;
                case ConditionCodeDef.sDAYTIME:
                    nCMDCode = ConditionCodeDef.nDAYTIME;
                    goto L001;
                case ConditionCodeDef.sCHECKITEMW:
                    nCMDCode = ConditionCodeDef.nCHECKITEMW;
                    goto L001;
                case ConditionCodeDef.sISTAKEITEM:
                    nCMDCode = ConditionCodeDef.nISTAKEITEM;
                    goto L001;
                case ConditionCodeDef.sCHECKDURA:
                    nCMDCode = ConditionCodeDef.nCHECKDURA;
                    goto L001;
                case ConditionCodeDef.sCHECKDURAEVA:
                    nCMDCode = ConditionCodeDef.nCHECKDURAEVA;
                    goto L001;
                case ConditionCodeDef.sDAYOFWEEK:
                    nCMDCode = ConditionCodeDef.nDAYOFWEEK;
                    goto L001;
                case ConditionCodeDef.sHOUR:
                    nCMDCode = ConditionCodeDef.nHOUR;
                    goto L001;
                case ConditionCodeDef.sMIN:
                    nCMDCode = ConditionCodeDef.nMIN;
                    goto L001;
                case ConditionCodeDef.sCHECKLUCKYPOINT:
                    nCMDCode = ConditionCodeDef.nCHECKLUCKYPOINT;
                    goto L001;
                case ConditionCodeDef.sCHECKMONMAP:
                    nCMDCode = ConditionCodeDef.nCHECKMONMAP;
                    goto L001;
                case ConditionCodeDef.sCHECKMONAREA:
                    nCMDCode = ConditionCodeDef.nCHECKMONAREA;
                    goto L001;
                case ConditionCodeDef.sCHECKHUM:
                    nCMDCode = ConditionCodeDef.nCHECKHUM;
                    goto L001;
                case ConditionCodeDef.sEQUAL:
                    nCMDCode = ConditionCodeDef.nEQUAL;
                    goto L001;
                case ConditionCodeDef.sLARGE:
                    nCMDCode = ConditionCodeDef.nLARGE;
                    goto L001;
                case ConditionCodeDef.sSMALL:
                    nCMDCode = ConditionCodeDef.nSMALL;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKPOSEDIR:
                    nCMDCode = ConditionCodeDef.nSC_CHECKPOSEDIR;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKPOSELEVEL:
                    nCMDCode = ConditionCodeDef.nSC_CHECKPOSELEVEL;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKPOSEGENDER:
                    nCMDCode = ConditionCodeDef.nSC_CHECKPOSEGENDER;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKLEVELEX:
                    nCMDCode = ConditionCodeDef.nSC_CHECKLEVELEX;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKBONUSPOINT:
                    nCMDCode = ConditionCodeDef.nSC_CHECKBONUSPOINT;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKMARRY:
                    nCMDCode = ConditionCodeDef.nSC_CHECKMARRY;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKPOSEMARRY:
                    nCMDCode = ConditionCodeDef.nSC_CHECKPOSEMARRY;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKMARRYCOUNT:
                    nCMDCode = ConditionCodeDef.nSC_CHECKMARRYCOUNT;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKMASTER:
                    nCMDCode = ConditionCodeDef.nSC_CHECKMASTER;
                    goto L001;
                case ConditionCodeDef.sSC_HAVEMASTER:
                    nCMDCode = ConditionCodeDef.nSC_HAVEMASTER;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKPOSEMASTER:
                    nCMDCode = ConditionCodeDef.nSC_CHECKPOSEMASTER;
                    goto L001;
                case ConditionCodeDef.sSC_POSEHAVEMASTER:
                    nCMDCode = ConditionCodeDef.nSC_POSEHAVEMASTER;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKISMASTER:
                    nCMDCode = ConditionCodeDef.nSC_CHECKISMASTER;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKPOSEISMASTER:
                    nCMDCode = ConditionCodeDef.nSC_CHECKPOSEISMASTER;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKNAMEIPLIST:
                    nCMDCode = ConditionCodeDef.nSC_CHECKNAMEIPLIST;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKACCOUNTIPLIST:
                    nCMDCode = ConditionCodeDef.nSC_CHECKACCOUNTIPLIST;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKSLAVECOUNT:
                    nCMDCode = ConditionCodeDef.nSC_CHECKSLAVECOUNT;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKPOS:
                    nCMDCode = ConditionCodeDef.nSC_CHECKPOS;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKMAP:
                    nCMDCode = ConditionCodeDef.nSC_CHECKMAP;
                    goto L001;
                case ConditionCodeDef.sSC_REVIVESLAVE:
                    nCMDCode = ConditionCodeDef.nSC_REVIVESLAVE;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKMAGICLVL:
                    nCMDCode = ConditionCodeDef.nSC_CHECKMAGICLVL;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKGROUPCLASS:
                    nCMDCode = ConditionCodeDef.nSC_CHECKGROUPCLASS;
                    goto L001;
                case ConditionCodeDef.sSC_ISGROUPMASTER:
                    nCMDCode = ConditionCodeDef.nSC_ISGROUPMASTER;
                    goto L001;
                case ConditionCodeDef.sCheckDiemon:
                    nCMDCode = ConditionCodeDef.nCheckDiemon;
                    goto L001;
                case ConditionCodeDef.scheckkillplaymon:
                    nCMDCode = ConditionCodeDef.ncheckkillplaymon;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKRANDOMNO:
                    nCMDCode = ConditionCodeDef.nSC_CHECKRANDOMNO;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKISONMAP:
                    nCMDCode = ConditionCodeDef.nSC_CHECKISONMAP;
                    goto L001;
                // 检测是否安全区
                case ConditionCodeDef.sSC_CHECKINSAFEZONE:
                    nCMDCode = ConditionCodeDef.nSC_CHECKINSAFEZONE;
                    goto L001;
                case ConditionCodeDef.sSC_KILLBYHUM:
                    nCMDCode = ConditionCodeDef.nSC_KILLBYHUM;
                    goto L001;
                case ConditionCodeDef.sSC_KILLBYMON:
                    nCMDCode = ConditionCodeDef.nSC_KILLBYMON;
                    goto L001;
                // 增加挂机
                case ExecutionCodeDef.sSC_OffLine:
                    nCMDCode = ExecutionCodeDef.nSC_OffLine;
                    goto L001;
                // 增加脚本特修所有装备命令
                case ExecutionCodeDef.sSC_REPAIRALL:
                    nCMDCode = ExecutionCodeDef.nSC_REPAIRALL;
                    goto L001;
                // 刷新包裹物品命令
                case ExecutionCodeDef.sSC_QUERYBAGITEMS:
                    nCMDCode = ExecutionCodeDef.nSC_QUERYBAGITEMS;
                    goto L001;
                case ExecutionCodeDef.sSC_SETRANDOMNO:
                    nCMDCode = ExecutionCodeDef.nSC_SETRANDOMNO;
                    goto L001;
                case ExecutionCodeDef.sSC_DELAYGOTO:
                case "DELAYCALL":
                    nCMDCode = ExecutionCodeDef.nSC_DELAYGOTO;
                    goto L001;
                case ConditionCodeDef.sSCHECKDEATHPLAYMON:
                    nCMDCode = ConditionCodeDef.nSCHECKDEATHPLAYMON;
                    goto L001;
                case ConditionCodeDef.sSCHECKKILLMOBNAME:
                    nCMDCode = ConditionCodeDef.nSCHECKDEATHPLAYMON;
                    goto L001;
            }
        L001:
            if (nCMDCode > 0) {
                QuestConditionInfo.CmdCode = nCMDCode;
                if (!string.IsNullOrEmpty(sParam1) && sParam1[0] == '\"') {
                    HUtil32.ArrestStringEx(sParam1, "\"", "\"", ref sParam1);
                }
                if (!string.IsNullOrEmpty(sParam2) && sParam2[0] == '\"') {
                    HUtil32.ArrestStringEx(sParam2, "\"", "\"", ref sParam2);
                }
                if (!string.IsNullOrEmpty(sParam3) && sParam3[0] == '\"') {
                    HUtil32.ArrestStringEx(sParam3, "\"", "\"", ref sParam3);
                }
                if (!string.IsNullOrEmpty(sParam4) && sParam4[0] == '\"') {
                    HUtil32.ArrestStringEx(sParam4, "\"", "\"", ref sParam4);
                }
                if (!string.IsNullOrEmpty(sParam5) && sParam5[0] == '\"') {
                    HUtil32.ArrestStringEx(sParam5, "\"", "\"", ref sParam5);
                }
                if (!string.IsNullOrEmpty(sParam6) && sParam6[0] == '\"') {
                    HUtil32.ArrestStringEx(sParam6, "\"", "\"", ref sParam6);
                }
                QuestConditionInfo.sParam1 = sParam1;
                QuestConditionInfo.sParam2 = sParam2;
                QuestConditionInfo.sParam3 = sParam3;
                QuestConditionInfo.sParam4 = sParam4;
                QuestConditionInfo.sParam5 = sParam5;
                QuestConditionInfo.sParam6 = sParam6;
                if (HUtil32.IsStringNumber(sParam1)) {
                    QuestConditionInfo.nParam1 = HUtil32.StrToInt(sParam1, 0);
                }
                if (HUtil32.IsStringNumber(sParam2)) {
                    QuestConditionInfo.nParam2 = HUtil32.StrToInt(sParam2, 0);
                }
                if (HUtil32.IsStringNumber(sParam3)) {
                    QuestConditionInfo.nParam3 = HUtil32.StrToInt(sParam3, 0);
                }
                if (HUtil32.IsStringNumber(sParam4)) {
                    QuestConditionInfo.nParam4 = HUtil32.StrToInt(sParam4, 0);
                }
                if (HUtil32.IsStringNumber(sParam5)) {
                    QuestConditionInfo.nParam5 = HUtil32.StrToInt(sParam5, 0);
                }
                if (HUtil32.IsStringNumber(sParam6)) {
                    QuestConditionInfo.nParam6 = HUtil32.StrToInt(sParam6, 0);
                }
                result = true;
            }
            return result;
        }

        private bool LoadScriptFileQuestAction(string sText, ref QuestActionInfo QuestActionInfo) {
            var sCmd = string.Empty;
            var sParam1 = string.Empty;
            var sParam2 = string.Empty;
            var sParam3 = string.Empty;
            var sParam4 = string.Empty;
            var sParam5 = string.Empty;
            var sParam6 = string.Empty;
            int nCMDCode= 0;
            var result = false;
            sText = HUtil32.GetValidStrCap(sText, ref sCmd, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam1, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam2, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam3, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam4, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam5, TextSpitConst);
            sText = HUtil32.GetValidStrCap(sText, ref sParam6, TextSpitConst);
            if (sCmd.IndexOf(".", StringComparison.OrdinalIgnoreCase) > -1) //支持脚本变量
            {
                var sActName = string.Empty;
                sCmd = HUtil32.GetValidStrCap(sCmd, ref sActName, '.');
                if (!string.IsNullOrEmpty(sActName)) {
                    QuestActionInfo.sOpName = sActName;
                    if (sCmd.IndexOf(".", StringComparison.OrdinalIgnoreCase) > -1) {
                        sCmd = HUtil32.GetValidStrCap(sCmd, ref sActName, '.');
                        if (string.Compare(sActName, "H", StringComparison.OrdinalIgnoreCase) == 0) {
                            QuestActionInfo.sOpHName = "H";
                        }
                    }
                }
            }
            sCmd = sCmd.ToUpper();
            switch (sCmd) {
                case ExecutionCodeDef.sSET: {
                        nCMDCode = ExecutionCodeDef.nSET;
                        HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                        if (!HUtil32.IsStringNumber(sParam1)) {
                            nCMDCode = 0;
                        }
                        if (!HUtil32.IsStringNumber(sParam2)) {
                            nCMDCode = 0;
                        }
                        break;
                    }
                case ExecutionCodeDef.sRESET: {
                        nCMDCode = ExecutionCodeDef.nRESET;
                        HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                        if (!HUtil32.IsStringNumber(sParam1)) {
                            nCMDCode = 0;
                        }
                        if (!HUtil32.IsStringNumber(sParam2)) {
                            nCMDCode = 0;
                        }
                        break;
                    }
                case ExecutionCodeDef.sSETOPEN: {
                        nCMDCode = ExecutionCodeDef.nSETOPEN;
                        HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                        if (!HUtil32.IsStringNumber(sParam1)) {
                            nCMDCode = 0;
                        }
                        if (!HUtil32.IsStringNumber(sParam2)) {
                            nCMDCode = 0;
                        }
                        break;
                    }
                case ExecutionCodeDef.sSETUNIT: {
                        nCMDCode = ExecutionCodeDef.nSETUNIT;
                        HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                        if (!HUtil32.IsStringNumber(sParam1)) {
                            nCMDCode = 0;
                        }
                        if (!HUtil32.IsStringNumber(sParam2)) {
                            nCMDCode = 0;
                        }
                        break;
                    }
                case ExecutionCodeDef.sRESETUNIT: {
                        nCMDCode = ExecutionCodeDef.nRESETUNIT;
                        HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                        if (!HUtil32.IsStringNumber(sParam1)) {
                            nCMDCode = 0;
                        }
                        if (!HUtil32.IsStringNumber(sParam2)) {
                            nCMDCode = 0;
                        }

                        break;
                    }
                case ExecutionCodeDef.sTAKE:
                    nCMDCode = ExecutionCodeDef.nTAKE;
                    goto L001;
                case ExecutionCodeDef.sSC_GIVE:
                    nCMDCode = ExecutionCodeDef.nSC_GIVE;
                    goto L001;
                case ExecutionCodeDef.sCLOSE:
                    nCMDCode = ExecutionCodeDef.nCLOSE;
                    goto L001;
                case ExecutionCodeDef.sBREAK:
                    nCMDCode = ExecutionCodeDef.nBREAK;
                    goto L001;
                case ExecutionCodeDef.sGOTO:
                    nCMDCode = ExecutionCodeDef.nGOTO;
                    goto L001;
                case ExecutionCodeDef.sADDNAMELIST:
                    nCMDCode = ExecutionCodeDef.nADDNAMELIST;
                    goto L001;
                case ExecutionCodeDef.sDELNAMELIST:
                    nCMDCode = ExecutionCodeDef.nDELNAMELIST;
                    goto L001;
                case ExecutionCodeDef.sADDGUILDLIST:
                    nCMDCode = ExecutionCodeDef.nADDGUILDLIST;
                    goto L001;
                case ExecutionCodeDef.sDELGUILDLIST:
                    nCMDCode = ExecutionCodeDef.nDELGUILDLIST;
                    goto L001;
                case ExecutionCodeDef.sSC_LINEMSG:
                    nCMDCode = ExecutionCodeDef.nSC_LINEMSG;
                    goto L001;
                case ExecutionCodeDef.sADDACCOUNTLIST:
                    nCMDCode = ExecutionCodeDef.nADDACCOUNTLIST;
                    goto L001;
                case ExecutionCodeDef.sDELACCOUNTLIST:
                    nCMDCode = ExecutionCodeDef.nDELACCOUNTLIST;
                    goto L001;
                case ExecutionCodeDef.sADDIPLIST:
                    nCMDCode = ExecutionCodeDef.nADDIPLIST;
                    goto L001;
                case ExecutionCodeDef.sDELIPLIST:
                    nCMDCode = ExecutionCodeDef.nDELIPLIST;
                    goto L001;
                case ExecutionCodeDef.sSENDMSG:
                    nCMDCode = ExecutionCodeDef.nSENDMSG;
                    goto L001;
                case ExecutionCodeDef.sCHANGEMODE:
                    nCMDCode = ExecutionCodeDef.nSC_CHANGEMODE;
                    goto L001;
                case ExecutionCodeDef.sPKPOINT:
                    nCMDCode = ExecutionCodeDef.nPKPOINT;
                    goto L001;
                case ExecutionCodeDef.sCHANGEXP:
                    nCMDCode = ExecutionCodeDef.nCHANGEXP;
                    goto L001;
                case ExecutionCodeDef.sSC_RECALLMOB:
                    nCMDCode = ExecutionCodeDef.nSC_RECALLMOB;
                    goto L001;
                case ExecutionCodeDef.sTAKEW:
                    nCMDCode = ExecutionCodeDef.nTAKEW;
                    goto L001;
                case ExecutionCodeDef.sTIMERECALL:
                    nCMDCode = ExecutionCodeDef.nTIMERECALL;
                    goto L001;
                case ExecutionCodeDef.sSC_PARAM1:
                    nCMDCode = ExecutionCodeDef.nSC_PARAM1;
                    goto L001;
                case ExecutionCodeDef.sSC_PARAM2:
                    nCMDCode = ExecutionCodeDef.nSC_PARAM2;
                    goto L001;
                case ExecutionCodeDef.sSC_PARAM3:
                    nCMDCode = ExecutionCodeDef.nSC_PARAM3;
                    goto L001;
                case ExecutionCodeDef.sSC_PARAM4:
                    nCMDCode = ExecutionCodeDef.nSC_PARAM4;
                    goto L001;
                case ExecutionCodeDef.sSC_EXEACTION:
                    nCMDCode = ExecutionCodeDef.nSC_EXEACTION;
                    goto L001;
                case ExecutionCodeDef.sMAPMOVE:
                    nCMDCode = ExecutionCodeDef.nMAPMOVE;
                    goto L001;
                case ExecutionCodeDef.sMAP:
                    nCMDCode = ExecutionCodeDef.nMAP;
                    goto L001;
                case ExecutionCodeDef.sTAKECHECKITEM:
                    nCMDCode = ExecutionCodeDef.nTAKECHECKITEM;
                    goto L001;
                case ExecutionCodeDef.sMONGEN:
                    nCMDCode = ExecutionCodeDef.nMONGEN;
                    goto L001;
                case ExecutionCodeDef.sMONCLEAR:
                    nCMDCode = ExecutionCodeDef.nMONCLEAR;
                    goto L001;
                case ExecutionCodeDef.sMOV:
                    nCMDCode = ExecutionCodeDef.nMOV;
                    goto L001;
                case ExecutionCodeDef.sINC:
                    nCMDCode = ExecutionCodeDef.nINC;
                    goto L001;
                case ExecutionCodeDef.sDEC:
                    nCMDCode = ExecutionCodeDef.nDEC;
                    goto L001;
                case ExecutionCodeDef.sSUM:
                    nCMDCode = ExecutionCodeDef.nSUM;
                    goto L001;
                //变量运算
                //除法
                case ExecutionCodeDef.sSC_DIV:
                    nCMDCode = ExecutionCodeDef.nSC_DIV;
                    goto L001;
                //除法
                case ExecutionCodeDef.sSC_MUL:
                    nCMDCode = ExecutionCodeDef.nSC_MUL;
                    goto L001;
                //除法
                case ExecutionCodeDef.sSC_PERCENT:
                    nCMDCode = ExecutionCodeDef.nSC_PERCENT;
                    goto L001;
                case ExecutionCodeDef.sTHROWITEM:
                case ExecutionCodeDef.sDROPITEMMAP:
                    nCMDCode = ExecutionCodeDef.nTHROWITEM;
                    goto L001;
                case ExecutionCodeDef.sBREAKTIMERECALL:
                    nCMDCode = ExecutionCodeDef.nBREAKTIMERECALL;
                    goto L001;
                case ExecutionCodeDef.sMOVR:
                    nCMDCode = ExecutionCodeDef.nMOVR;
                    goto L001;
                case ExecutionCodeDef.sEXCHANGEMAP:
                    nCMDCode = ExecutionCodeDef.nEXCHANGEMAP;
                    goto L001;
                case ExecutionCodeDef.sRECALLMAP:
                    nCMDCode = ExecutionCodeDef.nRECALLMAP;
                    goto L001;
                case ExecutionCodeDef.sADDBATCH:
                    nCMDCode = ExecutionCodeDef.nADDBATCH;
                    goto L001;
                case ExecutionCodeDef.sBATCHDELAY:
                    nCMDCode = ExecutionCodeDef.nBATCHDELAY;
                    goto L001;
                case ExecutionCodeDef.sBATCHMOVE:
                    nCMDCode = ExecutionCodeDef.nBATCHMOVE;
                    goto L001;
                case ExecutionCodeDef.sPLAYDICE:
                    nCMDCode = ExecutionCodeDef.nPLAYDICE;
                    goto L001;
                case ExecutionCodeDef.sGOQUEST:
                    nCMDCode = ExecutionCodeDef.nGOQUEST;
                    goto L001;
                case ExecutionCodeDef.sENDQUEST:
                    nCMDCode = ExecutionCodeDef.nENDQUEST;
                    goto L001;
                case ExecutionCodeDef.sSC_HAIRCOLOR:
                    nCMDCode = ExecutionCodeDef.nSC_HAIRCOLOR;
                    goto L001;
                case ExecutionCodeDef.sSC_WEARCOLOR:
                    nCMDCode = ExecutionCodeDef.nSC_WEARCOLOR;
                    goto L001;
                case ExecutionCodeDef.sSC_HAIRSTYLE:
                    nCMDCode = ExecutionCodeDef.nSC_HAIRSTYLE;
                    goto L001;
                case ExecutionCodeDef.sSC_MONRECALL:
                    nCMDCode = ExecutionCodeDef.nSC_MONRECALL;
                    goto L001;
                case ExecutionCodeDef.sSC_HORSECALL:
                    nCMDCode = ExecutionCodeDef.nSC_HORSECALL;
                    goto L001;
                case ExecutionCodeDef.sSC_HAIRRNDCOL:
                    nCMDCode = ExecutionCodeDef.nSC_HAIRRNDCOL;
                    goto L001;
                case ExecutionCodeDef.sSC_KILLHORSE:
                    nCMDCode = ExecutionCodeDef.nSC_KILLHORSE;
                    goto L001;
                case ExecutionCodeDef.sSC_RANDSETDAILYQUEST:
                    nCMDCode = ExecutionCodeDef.nSC_RANDSETDAILYQUEST;
                    goto L001;
                case ExecutionCodeDef.sSC_CHANGELEVEL:
                    nCMDCode = ExecutionCodeDef.nSC_CHANGELEVEL;
                    goto L001;
                case ExecutionCodeDef.sSC_MARRY:
                    nCMDCode = ExecutionCodeDef.nSC_MARRY;
                    goto L001;
                case ExecutionCodeDef.sSC_UNMARRY:
                    nCMDCode = ExecutionCodeDef.nSC_UNMARRY;
                    goto L001;
                case ExecutionCodeDef.sSC_GETMARRY:
                    nCMDCode = ExecutionCodeDef.nSC_GETMARRY;
                    goto L001;
                case ExecutionCodeDef.sSC_GETMASTER:
                    nCMDCode = ExecutionCodeDef.nSC_GETMASTER;
                    goto L001;
                case ExecutionCodeDef.sSC_CLEARSKILL:
                    nCMDCode = ExecutionCodeDef.nSC_CLEARSKILL;
                    goto L001;
                case ExecutionCodeDef.sSC_DELNOJOBSKILL:
                    nCMDCode = ExecutionCodeDef.nSC_DELNOJOBSKILL;
                    goto L001;
                case ExecutionCodeDef.sSC_DELSKILL:
                    nCMDCode = ExecutionCodeDef.nSC_DELSKILL;
                    goto L001;
                case ExecutionCodeDef.sSC_ADDSKILL:
                    nCMDCode = ExecutionCodeDef.nSC_ADDSKILL;
                    goto L001;
                case ExecutionCodeDef.sSC_SKILLLEVEL:
                    nCMDCode = ExecutionCodeDef.nSC_SKILLLEVEL;
                    goto L001;
                case ExecutionCodeDef.sSC_CHANGEPKPOINT:
                    nCMDCode = ExecutionCodeDef.nSC_CHANGEPKPOINT;
                    goto L001;
                case ExecutionCodeDef.sSC_CHANGEEXP:
                    nCMDCode = ExecutionCodeDef.nSC_CHANGEEXP;
                    goto L001;
                case ExecutionCodeDef.sSC_CHANGEJOB:
                    nCMDCode = ExecutionCodeDef.nSC_CHANGEJOB;
                    goto L001;
                case ExecutionCodeDef.sSC_MISSION:
                    nCMDCode = ExecutionCodeDef.nSC_MISSION;
                    goto L001;
                case ExecutionCodeDef.sSC_MOBPLACE:
                    nCMDCode = ExecutionCodeDef.nSC_MOBPLACE;
                    goto L001;
                case ExecutionCodeDef.sSC_SETMEMBERTYPE:
                    nCMDCode = ExecutionCodeDef.nSC_SETMEMBERTYPE;
                    goto L001;
                case ExecutionCodeDef.sSC_SETMEMBERLEVEL:
                    nCMDCode = ExecutionCodeDef.nSC_SETMEMBERLEVEL;
                    goto L001;
                case ExecutionCodeDef.sSC_GAMEGOLD:
                    nCMDCode = ExecutionCodeDef.nSC_GAMEGOLD;
                    goto L001;
                case ExecutionCodeDef.sSC_GAMEPOINT:
                    nCMDCode = ExecutionCodeDef.nSC_GAMEPOINT;
                    goto L001;
                case ExecutionCodeDef.sSC_PKZONE:
                    nCMDCode = ExecutionCodeDef.nSC_PKZONE;
                    goto L001;
                case ExecutionCodeDef.sSC_RESTBONUSPOINT:
                    nCMDCode = ExecutionCodeDef.nSC_RESTBONUSPOINT;
                    goto L001;
                case ExecutionCodeDef.sSC_TAKECASTLEGOLD:
                    nCMDCode = ExecutionCodeDef.nSC_TAKECASTLEGOLD;
                    goto L001;
                case ExecutionCodeDef.sSC_HUMANHP:
                    nCMDCode = ExecutionCodeDef.nSC_HUMANHP;
                    goto L001;
                case ExecutionCodeDef.sSC_HUMANMP:
                    nCMDCode = ExecutionCodeDef.nSC_HUMANMP;
                    goto L001;
                case ExecutionCodeDef.sSC_BUILDPOINT:
                    nCMDCode = ExecutionCodeDef.nSC_BUILDPOINT;
                    goto L001;
                case ExecutionCodeDef.sSC_AURAEPOINT:
                    nCMDCode = ExecutionCodeDef.nSC_AURAEPOINT;
                    goto L001;
                case ExecutionCodeDef.sSC_STABILITYPOINT:
                    nCMDCode = ExecutionCodeDef.nSC_STABILITYPOINT;
                    goto L001;
                case ExecutionCodeDef.sSC_FLOURISHPOINT:
                    nCMDCode = ExecutionCodeDef.nSC_FLOURISHPOINT;
                    goto L001;
                case ExecutionCodeDef.sSC_OPENMAGICBOX:
                    nCMDCode = ExecutionCodeDef.nSC_OPENMAGICBOX;
                    goto L001;
                case ExecutionCodeDef.sSC_SETRANKLEVELNAME:
                    nCMDCode = ExecutionCodeDef.nSC_SETRANKLEVELNAME;
                    goto L001;
                case ExecutionCodeDef.sSC_GMEXECUTE:
                    nCMDCode = ExecutionCodeDef.nSC_GMEXECUTE;
                    goto L001;
                case ExecutionCodeDef.sSC_GUILDCHIEFITEMCOUNT:
                    nCMDCode = ExecutionCodeDef.nSC_GUILDCHIEFITEMCOUNT;
                    goto L001;
                case ExecutionCodeDef.sSC_ADDNAMEDATELIST:
                    nCMDCode = ExecutionCodeDef.nSC_ADDNAMEDATELIST;
                    goto L001;
                case ExecutionCodeDef.sSC_DELNAMEDATELIST:
                    nCMDCode = ExecutionCodeDef.nSC_DELNAMEDATELIST;
                    goto L001;
                case ExecutionCodeDef.sSC_MOBFIREBURN:
                    nCMDCode = ExecutionCodeDef.nSC_MOBFIREBURN;
                    goto L001;
                case ExecutionCodeDef.sSC_MESSAGEBOX:
                    nCMDCode = ExecutionCodeDef.nSC_MESSAGEBOX;
                    goto L001;
                case ExecutionCodeDef.sSC_SETSCRIPTFLAG:
                    nCMDCode = ExecutionCodeDef.nSC_SETSCRIPTFLAG;
                    goto L001;
                case ExecutionCodeDef.sSC_SETAUTOGETEXP:
                    nCMDCode = ExecutionCodeDef.nSC_SETAUTOGETEXP;
                    goto L001;
                case ExecutionCodeDef.sSC_VAR:
                    nCMDCode = ExecutionCodeDef.nSC_VAR;
                    goto L001;
                case ExecutionCodeDef.sSC_LOADVAR:
                    nCMDCode = ExecutionCodeDef.nSC_LOADVAR;
                    goto L001;
                case ExecutionCodeDef.sSC_SAVEVAR:
                    nCMDCode = ExecutionCodeDef.nSC_SAVEVAR;
                    goto L001;
                case ExecutionCodeDef.sSC_CALCVAR:
                    nCMDCode = ExecutionCodeDef.nSC_CALCVAR;
                    goto L001;
                case ExecutionCodeDef.sSC_AUTOADDGAMEGOLD:
                    nCMDCode = ExecutionCodeDef.nSC_AUTOADDGAMEGOLD;
                    goto L001;
                case ExecutionCodeDef.sSC_AUTOSUBGAMEGOLD:
                    nCMDCode = ExecutionCodeDef.nSC_AUTOSUBGAMEGOLD;
                    goto L001;
                case ExecutionCodeDef.sSC_RECALLGROUPMEMBERS:
                    nCMDCode = ExecutionCodeDef.nSC_RECALLGROUPMEMBERS;
                    goto L001;
                case ExecutionCodeDef.sSC_CLEARNAMELIST:
                    nCMDCode = ExecutionCodeDef.nSC_CLEARNAMELIST;
                    goto L001;
                case ExecutionCodeDef.sSC_CHANGENAMECOLOR:
                    nCMDCode = ExecutionCodeDef.nSC_CHANGENAMECOLOR;
                    goto L001;
                case ExecutionCodeDef.sSC_CLEARPASSWORD:
                    nCMDCode = ExecutionCodeDef.nSC_CLEARPASSWORD;
                    goto L001;
                case ExecutionCodeDef.sSC_RENEWLEVEL:
                    nCMDCode = ExecutionCodeDef.nSC_RENEWLEVEL;
                    goto L001;
                case ExecutionCodeDef.sSC_KILLMONEXPRATE:
                    nCMDCode = ExecutionCodeDef.nSC_KILLMONEXPRATE;
                    goto L001;
                case ExecutionCodeDef.sSC_POWERRATE:
                    nCMDCode = ExecutionCodeDef.nSC_POWERRATE;
                    goto L001;
                case ExecutionCodeDef.sSC_CHANGEPERMISSION:
                    nCMDCode = ExecutionCodeDef.nSC_CHANGEPERMISSION;
                    goto L001;
                case ExecutionCodeDef.sSC_KILL:
                    nCMDCode = ExecutionCodeDef.nSC_KILL;
                    goto L001;
                case ExecutionCodeDef.sSC_KICK:
                    nCMDCode = ExecutionCodeDef.nSC_KICK;
                    goto L001;
                case ExecutionCodeDef.sSC_BONUSPOINT:
                    nCMDCode = ExecutionCodeDef.nSC_BONUSPOINT;
                    goto L001;
                case ExecutionCodeDef.sSC_RESTRENEWLEVEL:
                    nCMDCode = ExecutionCodeDef.nSC_RESTRENEWLEVEL;
                    goto L001;
                case ExecutionCodeDef.sSC_DELMARRY:
                    nCMDCode = ExecutionCodeDef.nSC_DELMARRY;
                    goto L001;
                case ExecutionCodeDef.sSC_DELMASTER:
                    nCMDCode = ExecutionCodeDef.nSC_DELMASTER;
                    goto L001;
                case ExecutionCodeDef.sSC_MASTER:
                    nCMDCode = ExecutionCodeDef.nSC_MASTER;
                    goto L001;
                case ExecutionCodeDef.sSC_UNMASTER:
                    nCMDCode = ExecutionCodeDef.nSC_UNMASTER;
                    goto L001;
                case ExecutionCodeDef.sSC_CREDITPOINT:
                    nCMDCode = ExecutionCodeDef.nSC_CREDITPOINT;
                    goto L001;
                case ExecutionCodeDef.sSC_CLEARNEEDITEMS:
                    nCMDCode = ExecutionCodeDef.nSC_CLEARNEEDITEMS;
                    goto L001;
                case ExecutionCodeDef.sSC_CLEARMAKEITEMS:
                    nCMDCode = ExecutionCodeDef.nSC_CLEARMAEKITEMS;
                    goto L001;
                case ExecutionCodeDef.sSC_SETSENDMSGFLAG:
                    nCMDCode = ExecutionCodeDef.nSC_SETSENDMSGFLAG;
                    goto L001;
                case ExecutionCodeDef.sSC_UPGRADEITEMS:
                    nCMDCode = ExecutionCodeDef.nSC_UPGRADEITEMS;
                    goto L001;
                case ExecutionCodeDef.sSC_UPGRADEITEMSEX:
                    nCMDCode = ExecutionCodeDef.nSC_UPGRADEITEMSEX;
                    goto L001;
                case ExecutionCodeDef.sSC_MONGENEX:
                    nCMDCode = ExecutionCodeDef.nSC_MONGENEX;
                    goto L001;
                case ExecutionCodeDef.sSC_CLEARMAPMON:
                    nCMDCode = ExecutionCodeDef.nSC_CLEARMAPMON;
                    goto L001;
                case ExecutionCodeDef.sSC_SETMAPMODE:
                    nCMDCode = ExecutionCodeDef.nSC_SETMAPMODE;
                    goto L001;
                case ExecutionCodeDef.sSC_KILLSLAVE:
                    nCMDCode = ExecutionCodeDef.nSC_KILLSLAVE;
                    goto L001;
                case ExecutionCodeDef.sSC_CHANGEGENDER:
                    nCMDCode = ExecutionCodeDef.nSC_CHANGEGENDER;
                    goto L001;
                case ExecutionCodeDef.sSC_MAPTING:
                    nCMDCode = ExecutionCodeDef.nSC_MAPTING;
                    goto L001;
                case ExecutionCodeDef.sSC_GUILDRECALL:
                    nCMDCode = ExecutionCodeDef.nSC_GUILDRECALL;
                    goto L001;
                case ExecutionCodeDef.sSC_GROUPRECALL:
                    nCMDCode = ExecutionCodeDef.nSC_GROUPRECALL;
                    goto L001;
                case ExecutionCodeDef.sSC_GROUPADDLIST:
                    nCMDCode = ExecutionCodeDef.nSC_GROUPADDLIST;
                    goto L001;
                case ExecutionCodeDef.sSC_CLEARLIST:
                    nCMDCode = ExecutionCodeDef.nSC_CLEARLIST;
                    goto L001;
                case ExecutionCodeDef.sSC_GROUPMOVEMAP:
                    nCMDCode = ExecutionCodeDef.nSC_GROUPMOVEMAP;
                    goto L001;
                case ExecutionCodeDef.sSC_SAVESLAVES:
                    nCMDCode = ExecutionCodeDef.nSC_SAVESLAVES;
                    goto L001;
                case ExecutionCodeDef.sCHECKUSERDATE:
                    nCMDCode = ExecutionCodeDef.nCHECKUSERDATE;
                    goto L001;
                case ExecutionCodeDef.sADDUSERDATE:
                    nCMDCode = ExecutionCodeDef.nADDUSERDATE;
                    goto L001;
                case ConditionCodeDef.sCheckDiemon:
                    nCMDCode = ConditionCodeDef.nCheckDiemon;
                    goto L001;
                case ConditionCodeDef.scheckkillplaymon:
                    nCMDCode = ConditionCodeDef.ncheckkillplaymon;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKRANDOMNO:
                    nCMDCode = ConditionCodeDef.nSC_CHECKRANDOMNO;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKISONMAP:
                    nCMDCode = ConditionCodeDef.nSC_CHECKISONMAP;
                    goto L001;
                case ConditionCodeDef.sSC_CHECKINSAFEZONE:
                    nCMDCode = ConditionCodeDef.nSC_CHECKINSAFEZONE;
                    goto L001;
                case ConditionCodeDef.sSC_KILLBYHUM:
                    nCMDCode = ConditionCodeDef.nSC_KILLBYHUM;
                    goto L001;
                case ConditionCodeDef.sSC_KILLBYMON:
                    nCMDCode = ConditionCodeDef.nSC_KILLBYMON;
                    goto L001;
                case ExecutionCodeDef.sSC_ISHIGH:
                    nCMDCode = ExecutionCodeDef.nSC_ISHIGH;
                    goto L001;
                case ExecutionCodeDef.sOPENYBDEAL:
                    nCMDCode = ExecutionCodeDef.nOPENYBDEAL;
                    goto L001;
                case ExecutionCodeDef.sQUERYYBSELL:
                    nCMDCode = ExecutionCodeDef.nQUERYYBSELL;
                    goto L001;
                case ExecutionCodeDef.sQUERYYBDEAL:
                    nCMDCode = ExecutionCodeDef.nQUERYYBDEAL;
                    goto L001;
                case ExecutionCodeDef.sDELAYGOTO:
                case ExecutionCodeDef.sDELAYCALL:
                    nCMDCode = ExecutionCodeDef.nDELAYGOTO;
                    goto L001;
                case ExecutionCodeDef.sCLEARDELAYGOTO:
                    nCMDCode = ExecutionCodeDef.nCLEARDELAYGOTO;
                    goto L001;
                case ExecutionCodeDef.sSC_QUERYVALUE:
                    nCMDCode = ExecutionCodeDef.nSC_QUERYVALUE;
                    goto L001;
                case ExecutionCodeDef.sSC_QUERYITEMDLG:
                    nCMDCode = ExecutionCodeDef.nSC_QUERYITEMDLG;
                    goto L001;
                case ExecutionCodeDef.sSC_UPGRADEDLGITEM:
                    nCMDCode = ExecutionCodeDef.nSC_UPGRADEDLGITEM;
                    goto L001;
                case ExecutionCodeDef.sSC_GETDLGITEMVALUE:
                    nCMDCode = ExecutionCodeDef.nSC_GETDLGITEMVALUE;
                    goto L001;
                case ExecutionCodeDef.sSC_TAKEDLGITEM:
                    nCMDCode = ExecutionCodeDef.nSC_TAKEDLGITEM;
                    goto L001;
            }
        L001:
            if (nCMDCode > 0) {
                QuestActionInfo.nCmdCode = nCMDCode;
                if (!string.IsNullOrEmpty(sParam1) && sParam1[0] == '\"') {
                    HUtil32.ArrestStringEx(sParam1, "\"", "\"", ref sParam1);
                }
                if (!string.IsNullOrEmpty(sParam2) && sParam2[0] == '\"') {
                    HUtil32.ArrestStringEx(sParam2, "\"", "\"", ref sParam2);
                }
                if (!string.IsNullOrEmpty(sParam3) && sParam3[0] == '\"') {
                    HUtil32.ArrestStringEx(sParam3, "\"", "\"", ref sParam3);
                }
                if (!string.IsNullOrEmpty(sParam4) && sParam4[0] == '\"') {
                    HUtil32.ArrestStringEx(sParam4, "\"", "\"", ref sParam4);
                }
                if (!string.IsNullOrEmpty(sParam5) && sParam5[0] == '\"') {
                    HUtil32.ArrestStringEx(sParam5, "\"", "\"", ref sParam5);
                }
                if (!string.IsNullOrEmpty(sParam6) && sParam6[0] == '\"') {
                    HUtil32.ArrestStringEx(sParam6, "\"", "\"", ref sParam6);
                }
                QuestActionInfo.sParam1 = sParam1;
                QuestActionInfo.sParam2 = sParam2;
                QuestActionInfo.sParam3 = sParam3;
                QuestActionInfo.sParam4 = sParam4;
                QuestActionInfo.sParam5 = sParam5;
                QuestActionInfo.sParam6 = sParam6;
                if (HUtil32.IsStringNumber(sParam1)) {
                    QuestActionInfo.nParam1 = HUtil32.StrToInt(sParam1, 0);
                }
                if (HUtil32.IsStringNumber(sParam2)) {
                    QuestActionInfo.nParam2 = HUtil32.StrToInt(sParam2, 1);
                }
                if (HUtil32.IsStringNumber(sParam3)) {
                    QuestActionInfo.nParam3 = HUtil32.StrToInt(sParam3, 1);
                }
                if (HUtil32.IsStringNumber(sParam4)) {
                    QuestActionInfo.nParam4 = HUtil32.StrToInt(sParam4, 0);
                }
                if (HUtil32.IsStringNumber(sParam5)) {
                    QuestActionInfo.nParam5 = HUtil32.StrToInt(sParam5, 0);
                }
                if (HUtil32.IsStringNumber(sParam6)) {
                    QuestActionInfo.nParam6 = HUtil32.StrToInt(sParam6, 0);
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
        public void LoadScriptFile(NormNpc NPC, string sPatch, string sScritpName, bool boFlag) {
            var command = string.Empty;
            var questName = string.Empty;
            var s44 = string.Empty;
            var slabName = string.Empty;
            var boDefine = false;
            ScriptInfo Script = null;
            SayingRecord SayingRecord = null;
            SayingProcedure SayingProcedure = null;
            var scriptType = 0;
            var questCount = 0;
            var sScritpFileName = M2Share.GetEnvirFilePath(sPatch, GetScriptCrossPath($"{sScritpName}.txt"));
            if (File.Exists(sScritpFileName)) {
                CallScriptDict.Clear();
                var stringList = new StringList();
                stringList.LoadFromFile(sScritpFileName);
                var success = false;
                while (!success) {
                    LoadCallScript(ref stringList, ref success);
                }
                IList<DefineInfo> defineList = new List<DefineInfo>();
                var defline = LoadScriptDefineInfo(stringList, defineList);
                var defineInfo = new DefineInfo { Name = "@HOME" };
                if (string.IsNullOrEmpty(defline)) {
                    defline = "@main";
                }
                defineInfo.Text = defline;
                defineList.Add(defineInfo);
                int n24;
                // 常量处理
                for (var i = 0; i < stringList.Count; i++) {
                    var line = stringList[i].Trim();
                    if (!string.IsNullOrEmpty(line)) {
                        if (line[0] == '[') {
                            boDefine = false;
                        }
                        else {
                            if (line[0] == '#' && (HUtil32.CompareLStr(line, "#IF") || HUtil32.CompareLStr(line, "#ACT") || HUtil32.CompareLStr(line, "#ELSEACT"))) {
                                boDefine = true;
                            }
                            else {
                                if (boDefine) {
                                    // 将Define 好的常量换成指定值
                                    for (var n20 = 0; n20 < defineList.Count; n20++) {
                                        defineInfo = defineList[n20];
                                        var n1C = 0;
                                        while (true) {
                                            n24 = line.ToUpper().IndexOf(defineInfo.Name, StringComparison.OrdinalIgnoreCase);
                                            if (n24 <= 0) {
                                                break;
                                            }
                                            var s58 = line[..n24];
                                            var s5C = line[(defineInfo.Name.Length + n24)..];
                                            line = s58 + defineInfo.Text + s5C;
                                            stringList[i] = line;
                                            n1C++;
                                            if (n1C >= 10) {
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
                for (var i = 0; i < defineList.Count; i++) {
                    defineList[i] = null;
                }
                defineList.Clear();
                var nQuestIdx = 0;
                for (var i = 0; i < stringList.Count; i++) {
                    var line = stringList[i].Trim();
                    if (string.IsNullOrEmpty(line) || line[0] == ';' || line[0] == '/') {
                        continue;
                    }
                    if (scriptType == 0 && boFlag)
                    {
                        if (line.StartsWith("%"))// 物品价格倍率
                        {
                            line = line[1..];
                            var nPriceRate = HUtil32.StrToInt(line, -1);
                            if (nPriceRate >= 55)
                            {
                                ((Merchant)NPC).PriceRate = nPriceRate;
                            }
                            continue;
                        }
                        if (line.StartsWith("+"))// 物品交易类型
                        {
                            line = line[1..];
                            var nItemType = HUtil32.StrToInt(line, -1);
                            if (nItemType >= 0)
                            {
                                ((Merchant)NPC).ItemTypeList.Add(nItemType);
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
                                    if (command.Equals(ScriptConst.sBUY, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).IsBuy = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptConst.sSELL, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).IsSell = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptConst.sMAKEDURG, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).IsMakeDrug = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptConst.sPRICES, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).IsPrices = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptConst.sSTORAGE, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).IsStorage = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptConst.sGETBACK, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).IsGetback = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptConst.sUPGRADENOW, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).IsUpgradenow = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptConst.sGETBACKUPGNOW, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).IsGetBackupgnow = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptConst.sREPAIR, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).IsRepair = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptConst.SuperRepair, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).IsSupRepair = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptConst.sSL_SENDMSG, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).IsSendMsg = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptConst.UseItemName, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).IsUseItemName = true;
                                        continue;
                                    }
                                    if (command.Equals(ScriptConst.sOFFLINEMSG, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).IsOffLineMsg = true;
                                        continue;
                                    }
                                    if (string.Compare(command, ScriptConst.sybdeal, StringComparison.OrdinalIgnoreCase) == 0)
                                    {
                                        ((Merchant)NPC).IsYbDeal = true;
                                        continue;
                                    }
                                }
                            }
                            continue;
                        }
                        // 增加处理NPC可执行命令设置
                    }
                    string s38;
                    if (line.StartsWith("{")) {
                        if (HUtil32.CompareLStr(line, "{Quest")) {
                            s38 = HUtil32.GetValidStr3(line, ref questName, new[] { ' ', '}', '\t' });
                            HUtil32.GetValidStr3(s38, ref questName, new[] { ' ', '}', '\t' });
                            questCount = HUtil32.StrToInt(questName, 0);
                            Script = LoadMakeNewScript(NPC);
                            Script.QuestCount = questCount;
                            questCount++;
                        }
                        if (HUtil32.CompareLStr(line, "{~Quest")) {
                            continue;
                        }
                    }
                    if (scriptType == 1 && Script != null && line.StartsWith("#")) {
                        s38 = HUtil32.GetValidStr3(line, ref questName, new[] { '=', ' ', '\t' });
                        Script.IsQuest = true;
                        if (HUtil32.CompareLStr(line, "#IF")) {
                            var questFlag = string.Empty;
                            HUtil32.ArrestStringEx(line, "[", "]", ref questFlag);
                            Script.QuestInfo[nQuestIdx].wFlag = HUtil32.StrToInt16(questFlag, 0);
                            HUtil32.GetValidStr3(s38, ref s44, new[] { '=', ' ', '\t' });
                            n24 = HUtil32.StrToInt(s44, 0);
                            if (n24 != 0) {
                                n24 = 1;
                            }
                            Script.QuestInfo[nQuestIdx].btValue = (byte)n24;
                        }
                        if (HUtil32.CompareLStr(line, "#RAND")) {
                            Script.QuestInfo[nQuestIdx].nRandRage = HUtil32.StrToInt(s44, 0);
                        }
                        continue;
                    }
                    if (line.StartsWith("[")) {
                        scriptType = 10;
                        if (Script == null) {
                            Script = LoadMakeNewScript(NPC);
                            Script.QuestCount = questCount;
                        }
                        if (line.Equals("[goods]", StringComparison.OrdinalIgnoreCase)) {
                            scriptType = 20;
                            NPC.ProcessRefillIndex = M2Share.CurrentMerchantIndex;
                            M2Share.CurrentMerchantIndex++;
                            continue;
                        }
                        line = HUtil32.ArrestStringEx(line, "[", "]", ref slabName);
                        SayingRecord = new SayingRecord { sLabel = slabName };
                        line = HUtil32.GetValidStrCap(line, ref slabName, TextSpitConst);
                        if (slabName.Equals("TRUE", StringComparison.OrdinalIgnoreCase)) {
                            SayingRecord.boExtJmp = true;
                        }
                        else {
                            SayingRecord.boExtJmp = false;
                        }
                        SayingProcedure = new SayingProcedure();
                        SayingRecord.ProcedureList.Add(SayingProcedure);
                        if (Script.RecordList.ContainsKey(SayingRecord.sLabel)) {
                            SayingRecord.sLabel += M2Share.RandomNumber.GetRandomNumber(1, 200);
                        }
                        Script.RecordList.Add(SayingRecord.sLabel, SayingRecord);
                        continue;
                    }
                    if (Script != null && SayingRecord != null) {
                        if (line[0] == '#' && scriptType >= 10 && scriptType < 20) {
                            if (line.Equals("#IF", StringComparison.OrdinalIgnoreCase)) {
                                if (SayingProcedure.ConditionList.Count > 0 || !string.IsNullOrEmpty(SayingProcedure.sSayMsg)) {
                                    SayingProcedure = new SayingProcedure();
                                    SayingRecord.ProcedureList.Add(SayingProcedure);
                                }
                                scriptType = 11;
                                continue;
                            }
                            if (line.Equals("#ACT", StringComparison.OrdinalIgnoreCase)) {
                                scriptType = 12;
                                continue;
                            }
                            if (line.Equals("#SAY", StringComparison.OrdinalIgnoreCase)) {
                                scriptType = 10;
                                continue;
                            }
                            if (line.Equals("#ELSEACT", StringComparison.OrdinalIgnoreCase)) {
                                scriptType = 13;
                                continue;
                            }
                            if (line.Equals("#ELSESAY", StringComparison.OrdinalIgnoreCase)) {
                                scriptType = 14;
                            }
                            continue;
                        }
                        switch (scriptType) {
                            case 10:
                                SayingProcedure.sSayMsg += line;
                                break;
                            case 11: {
                                    var questConditionInfo = new QuestConditionInfo();
                                    if (LoadScriptFileQuestCondition(line.Trim(), ref questConditionInfo)) {
                                        SayingProcedure.ConditionList.Add(questConditionInfo);
                                    }
                                    else {
                                        _logger.Error("脚本错误: " + line + " 第:" + i + " 行: " + sScritpFileName);
                                    }
                                    break;
                                }
                            case 12: {
                                    var questActionInfo = new QuestActionInfo();
                                    if (LoadScriptFileQuestAction(line.Trim(), ref questActionInfo)) {
                                        SayingProcedure.ActionList.Add(questActionInfo);
                                    }
                                    else {
                                        _logger.Error("脚本错误: " + line + " 第:" + i + " 行: " + sScritpFileName);
                                    }
                                    break;
                                }
                            case 13: {
                                    var questActionInfo = new QuestActionInfo();
                                    if (LoadScriptFileQuestAction(line.Trim(), ref questActionInfo)) {
                                        SayingProcedure.ElseActionList.Add(questActionInfo);
                                    }
                                    else {
                                        _logger.Error("脚本错误: " + line + " 第:" + i + " 行: " + sScritpFileName);
                                    }
                                    break;
                                }
                            case 14:
                                SayingProcedure.sElseSayMsg += line;
                                break;
                        }
                    }
                    if (scriptType == 20 && boFlag) {
                        var sItemName = string.Empty;
                        var sItemCount = string.Empty;
                        var sItemRefillTime = string.Empty;
                        line = HUtil32.GetValidStrCap(line, ref sItemName, TextSpitConst);
                        line = HUtil32.GetValidStrCap(line, ref sItemCount, TextSpitConst);
                        line = HUtil32.GetValidStrCap(line, ref sItemRefillTime, TextSpitConst);
                        if (!string.IsNullOrEmpty(sItemName) && !string.IsNullOrEmpty(sItemRefillTime)) {
                            if (sItemName[0] == '\"') {
                                HUtil32.ArrestStringEx(sItemName, "\"", "\"", ref sItemName);
                            }
                            var goods = new Goods {
                                ItemName = sItemName,
                                Count = HUtil32.StrToInt(sItemCount, 0),
                                RefillTime = HUtil32.StrToInt(sItemRefillTime, 0),
                                RefillTick = 0
                            };
                            if (M2Share.CanSellItem(sItemName)) {
                                ((Merchant)NPC).RefillGoodsList.Add(goods);
                            }
                        }
                    }
                }
                stringList.Dispose();
            }
            else {
                _logger.Error("Script file not found: " + sScritpFileName);
            }
        }
       
        /// <summary>
        /// 格式化标签
        /// </summary>
        /// <param name="sLabel"></param>
        /// <param name="boChange"></param>
        /// <returns></returns>
        private static string FormatLabelStr(string sLabel, ref bool boChange) {
            var result = sLabel;
            if (sLabel.IndexOf(")", StringComparison.OrdinalIgnoreCase) > -1) {
                HUtil32.GetValidStr3(sLabel, ref result, '(');
                boChange = true;
            }
            return result;
        }

        /// <summary>
        /// 初始化脚本处理方法
        /// </summary>
        /// <returns></returns>
        protected string InitializeProcedure(string sMsg) {
            var nC = 0;
            var sCmd = string.Empty;
            var tempstr = sMsg;
            while (true) {
                if (tempstr.IndexOf(">", StringComparison.OrdinalIgnoreCase) < -1) {
                    break;
                }
                tempstr = HUtil32.ArrestStringEx(tempstr, "<", ">", ref sCmd);
                if (!string.IsNullOrEmpty(sCmd)) {
                    if (sCmd[0] == '$') {
                        InitializeVariable(sCmd, ref sMsg);
                    }
                }
                else {
                    break;
                }
                nC++;
                if (nC > 100) {
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
        private string InitializeSayMsg(string sMsg, List<string> StringList, IList<string> OldStringList, IList<string> ScriptNameList) {
            var nC = 0;
            var s10 = string.Empty;
            var tempstr = sMsg;
            string sLabel;
            var sname = string.Empty;
            int nIdx;
            var nChangeIndex = 1;
            var nNotIdx = -1;
            var boChange = false;
            var boAddResetLabel = false;
            while (true) {
                if (string.IsNullOrEmpty(tempstr)) {
                    break;
                }
                if (tempstr.IndexOf('>') <= 0) {
                    break;
                }
                tempstr = HUtil32.ArrestStringEx(tempstr, "<", ">", ref s10);
                if (!string.IsNullOrEmpty(s10)) {
                    if (s10.IndexOf("/", StringComparison.OrdinalIgnoreCase) > 0) {
                        sLabel = HUtil32.GetValidStr3(s10, ref sname, '/');
                        if (string.Compare(sLabel, "@close", StringComparison.OrdinalIgnoreCase) == 0) {
                            continue;
                        }
                        if (string.Compare(sLabel, "@Exit", StringComparison.OrdinalIgnoreCase) == 0) {
                            continue;
                        }
                        if (string.Compare(sLabel, "@main", StringComparison.OrdinalIgnoreCase) == 0) {
                            continue;
                        }
                        if (HUtil32.CompareLStr(sLabel, "FCOLOR", 6)) {
                            continue;
                        }
                        if (HUtil32.CompareLStr(sLabel, "@Move(", 6)) {
                            continue;
                        }
                        if (HUtil32.CompareLStr(sLabel, "~@", 2)) {
                            continue;
                        }
                        if (HUtil32.CompareLStr(sLabel, "@@", 2)) {
                            if (!boAddResetLabel) {
                                boAddResetLabel = true;
                                sMsg = ScriptConst.RESETLABEL + sMsg;
                            }
                            continue;
                        }
                        nIdx = ScriptNameList.IndexOf(FormatLabelStr(sLabel, ref boChange));
                        if (nIdx == -1) {
                            nIdx = nNotIdx;
                            nNotIdx -= 1;
                        }
                        else if (boChange) {
                            nIdx = nChangeIndex * 100000 + nIdx;
                            nChangeIndex++;
                        }
                        OldStringList.Add(sLabel);
                        try {
                            if (sLabel.Length >= 2 && sLabel[1] == '@' && sLabel[0] == '@') {
                                sLabel = "@@" + nIdx;
                            }
                            else {
                                sLabel = "@" + nIdx;
                            }
                        }
                        finally {
                            StringList.Add(sLabel);
                        }
                        sMsg = sMsg.Replace("<" + s10 + ">", "<" + sname + "/" + sLabel + ">");
                    }
                    else if (s10[0] == '$') {
                        //游戏变量处理
                        InitializeVariable(s10, ref sMsg);
                    }
                }
                else {
                    break;
                }
                nC++;
                if (nC >= 100) {
                    break;
                }
            }
            return sMsg;
        }

        /// <summary>
        /// 初始化全局变量脚本
        /// </summary>
        private static void InitializeVariable(string sLabel, ref string sMsg) {
            var s14 = string.Empty;
            var sLabel2 = sLabel.ToUpper();
            if (sLabel2 == GrobalVarCodeDef.sVAR_SERVERNAME) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_SERVERNAME);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_SERVERIP) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_SERVERIP);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_WEBSITE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_WEBSITE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_BBSSITE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_BBSSITE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CLIENTDOWNLOAD) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CLIENTDOWNLOAD);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_QQ) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_QQ);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_PHONE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_PHONE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_BANKACCOUNT0) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_BANKACCOUNT0);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_BANKACCOUNT1) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_BANKACCOUNT1);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_BANKACCOUNT2) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_BANKACCOUNT2);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_BANKACCOUNT3) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_BANKACCOUNT3);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_BANKACCOUNT4) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_BANKACCOUNT4);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_BANKACCOUNT5) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_BANKACCOUNT5);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_BANKACCOUNT6) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_BANKACCOUNT6);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_BANKACCOUNT7) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_BANKACCOUNT7);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_BANKACCOUNT8) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_BANKACCOUNT8);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_BANKACCOUNT9) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_BANKACCOUNT9);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_GAMEGOLDNAME) {
                //sMsg = sMsg.Replace("<" + sLabel + ">", Grobal2.sSTRING_GAMEGOLD);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_GAMEPOINTNAME) {
                // sMsg = sMsg.Replace("<" + sLabel + ">", Grobal2.sSTRING_GAMEPOINT);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_USERCOUNT) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_USERCOUNT);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_DATETIME) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_DATETIME);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_USERNAME) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_USERNAME);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_FBMAPNAME) { //副本
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_FBMAPNAME);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_FBMAP) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_FBMAP);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_ACCOUNT) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_ACCOUNT);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_ASSEMBLEITEMNAME) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_ASSEMBLEITEMNAME);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_MAPNAME) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_MAPNAME);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_GUILDNAME) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_GUILDNAME);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_RANKNAME) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_RANKNAME);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_LEVEL) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_LEVEL);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_HP) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_HP);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_MAXHP) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_MAXHP);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_MP) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_MP);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_MAXMP) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_MAXMP);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_AC) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_AC);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_MAXAC) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_MAXAC);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_MAC) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_MAC);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_MAXMAC) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_MAXMAC);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_DC) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_DC);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_MAXDC) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_MAXDC);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_MC) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_MC);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_MAXMC) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_MAXMC);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_SC) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_SC);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_MAXSC) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_MAXSC);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_EXP) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_EXP);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_MAXEXP) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_MAXEXP);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_PKPOINT) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_PKPOINT);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CREDITPOINT) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CREDITPOINT);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_GOLDCOUNT) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_GOLDCOUNT);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_GAMEGOLD) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_GAMEGOLD);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_GAMEPOINT) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_GAMEPOINT);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_LOGINTIME) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_LOGINTIME);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_LOGINLONG) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_LOGINLONG);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_DRESS) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_DRESS);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_WEAPON) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_WEAPON);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_RIGHTHAND) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_RIGHTHAND);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_HELMET) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_HELMET);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_NECKLACE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_NECKLACE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_RING_R) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_RING_R);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_RING_L) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_RING_L);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_ARMRING_R) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_ARMRING_R);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_ARMRING_L) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_ARMRING_L);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_BUJUK) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_BUJUK);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_BELT) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_BELT);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_BOOTS) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_BOOTS);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CHARM) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CHARM);
            }
            //=======================================没有用到的==============================
            else if (sLabel2 == GrobalVarCodeDef.sVAR_HOUSE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_HOUSE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CIMELIA) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CIMELIA);
            }
            //=======================================没有用到的==============================
            else if (sLabel2 == GrobalVarCodeDef.sVAR_IPADDR) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_IPADDR);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_IPLOCAL) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_IPLOCAL);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_GUILDBUILDPOINT) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_GUILDBUILDPOINT);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_GUILDAURAEPOINT) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_GUILDAURAEPOINT);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_GUILDSTABILITYPOINT) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_GUILDSTABILITYPOINT);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_GUILDFLOURISHPOINT) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_GUILDFLOURISHPOINT);
            }
            //=================================没用用到的====================================
            else if (sLabel2 == GrobalVarCodeDef.sVAR_GUILDMONEYCOUNT) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_GUILDMONEYCOUNT);
            }
            //=================================没用用到的结束====================================
            else if (sLabel2 == GrobalVarCodeDef.sVAR_REQUESTCASTLEWARITEM) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_REQUESTCASTLEWARITEM);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_REQUESTCASTLEWARDAY) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_REQUESTCASTLEWARDAY);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_REQUESTBUILDGUILDITEM) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_REQUESTBUILDGUILDITEM);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_OWNERGUILD) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_OWNERGUILD);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CASTLENAME) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CASTLENAME);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_LORD) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_LORD);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_GUILDWARFEE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_GUILDWARFEE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_BUILDGUILDFEE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_BUILDGUILDFEE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CASTLEWARDATE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CASTLEWARDATE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_LISTOFWAR) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_LISTOFWAR);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CASTLECHANGEDATE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CASTLECHANGEDATE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CASTLEWARLASTDATE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CASTLEWARLASTDATE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CASTLEGETDAYS) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CASTLEGETDAYS);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_DATE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_DATE);
            }
            //===================================没用用到的======================================
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_PRVMSG) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_PRVMSG);
            }
            //===================================没用用到的结束======================================
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_ALLOWMSG) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_ALLOWMSG);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_LETSHOUT) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_LETSHOUT);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_LETTRADE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_LETTRADE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_LETGuild) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_LETGuild);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_ENDGUILD) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_ENDGUILD);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_BANGUILDCHAT) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_BANGUILDCHAT);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_AUTHALLY) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_AUTHALLY);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_AUTH) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_AUTH);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_AUTHCANCEL) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_AUTHCANCEL);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_USERMOVE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_USERMOVE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_SEARCHING) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_SEARCHING);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_ALLOWGROUPCALL) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_ALLOWGROUPCALL);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_GROUPRECALLL) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_GROUPRECALLL);
            }
            #region 没有使用的
            //===========================================没有使用的========================================
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_ALLOWGUILDRECALL) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_ALLOWGUILDRECALL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLOWGUILDRECALL, SctiptDef.sVAR_CMD_ALLOWGUILDRECALL);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_GUILDRECALLL) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_GUILDRECALLL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_GUILDRECALLL, SctiptDef.sVAR_CMD_GUILDRECALLL);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_DEAR) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_DEAR);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_DEAR, SctiptDef.sVAR_CMD_DEAR);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_ALLOWDEARRCALL) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_ALLOWDEARRCALL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLOWDEARRCALL, SctiptDef.sVAR_CMD_ALLOWDEARRCALL);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_DEARRECALL) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_DEARRECALL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_DEARRECALL, SctiptDef.sVAR_CMD_DEARRECALL);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_MASTER) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_MASTER);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_MASTER, SctiptDef.sVAR_CMD_MASTER);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_ALLOWMASTERRECALL) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_ALLOWMASTERRECALL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLOWMASTERRECALL, SctiptDef.sVAR_CMD_ALLOWMASTERRECALL);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_MASTERECALL) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_MASTERECALL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_MASTERECALL, SctiptDef.sVAR_CMD_MASTERECALL);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_TAKEONHORSE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_TAKEONHORSE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_TAKEONHORSE, SctiptDef.sVAR_CMD_TAKEONHORSE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_TAKEOFHORSE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_TAKEOFHORSE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_TAKEOFHORSE, SctiptDef.sVAR_CMD_TAKEOFHORSE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_ALLSYSMSG) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_ALLSYSMSG);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLSYSMSG, SctiptDef.sVAR_CMD_ALLSYSMSG);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_MEMBERFUNCTION) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_MEMBERFUNCTION);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_MEMBERFUNCTION, SctiptDef.sVAR_CMD_MEMBERFUNCTION);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_MEMBERFUNCTIONEX) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_MEMBERFUNCTIONEX);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_MEMBERFUNCTIONEX, SctiptDef.sVAR_CMD_MEMBERFUNCTIONEX);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CASTLEGOLD) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CASTLEGOLD);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CASTLEGOLD, SctiptDef.sVAR_CASTLEGOLD);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_TODAYINCOME) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_TODAYINCOME);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_TODAYINCOME, SctiptDef.sVAR_TODAYINCOME);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CASTLEDOORSTATE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CASTLEDOORSTATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CASTLEDOORSTATE, SctiptDef.sVAR_CASTLEDOORSTATE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_REPAIRDOORGOLD) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_REPAIRDOORGOLD);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_REPAIRDOORGOLD, SctiptDef.sVAR_REPAIRDOORGOLD);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_REPAIRWALLGOLD) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_REPAIRWALLGOLD);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_REPAIRWALLGOLD, SctiptDef.sVAR_REPAIRWALLGOLD);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_GUARDFEE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_GUARDFEE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_GUARDFEE, SctiptDef.sVAR_GUARDFEE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_ARCHERFEE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_ARCHERFEE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_ARCHERFEE, SctiptDef.sVAR_ARCHERFEE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_GUARDRULE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_GUARDRULE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_GUARDRULE, SctiptDef.sVAR_GUARDRULE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_STORAGE2STATE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_STORAGE2STATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_STORAGE2STATE, SctiptDef.sVAR_STORAGE2STATE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_STORAGE3STATE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_STORAGE3STATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_STORAGE3STATE, SctiptDef.sVAR_STORAGE3STATE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_STORAGE4STATE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_STORAGE4STATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_STORAGE4STATE, SctiptDef.sVAR_STORAGE4STATE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_STORAGE5STATE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_STORAGE5STATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_STORAGE5STATE, SctiptDef.sVAR_STORAGE5STATE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_SELFNAME) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_SELFNAME);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_SELFNAME, SctiptDef.sVAR_SELFNAME);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_POSENAME) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_POSENAME);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_POSENAME, SctiptDef.sVAR_POSENAME);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_GAMEDIAMOND) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_GAMEDIAMOND);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_GAMEDIAMOND, SctiptDef.sVAR_GAMEDIAMOND);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_GAMEGIRD) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_GAMEGIRD);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_GAMEGIRD, SctiptDef.sVAR_GAMEGIRD);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_ALLOWFIREND) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_ALLOWFIREND);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLOWFIREND, SctiptDef.sVAR_CMD_ALLOWFIREND);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_EFFIGYSTATE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_EFFIGYSTATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_EFFIGYSTATE, SctiptDef.sVAR_EFFIGYSTATE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_EFFIGYOFFSET) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_EFFIGYOFFSET);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_EFFIGYOFFSET, SctiptDef.sVAR_EFFIGYOFFSET);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_YEAR) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_YEAR);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_YEAR, SctiptDef.sVAR_YEAR);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_MONTH) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_MONTH);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_MONTH, SctiptDef.sVAR_MONTH);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_DAY) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_DAY);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_DAY, SctiptDef.sVAR_DAY);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_HOUR) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_HOUR);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_HOUR, SctiptDef.sVAR_HOUR);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_MINUTE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_MINUTE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_MINUTE, SctiptDef.sVAR_MINUTE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_SEC) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_SEC);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_SEC, SctiptDef.sVAR_SEC);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_MAP) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_MAP);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_MAP, SctiptDef.sVAR_MAP);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_X) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_X);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_X, SctiptDef.sVAR_X);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_Y) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_Y);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_Y, SctiptDef.sVAR_Y);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_UNMASTER_FORCE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_UNMASTER_FORCE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_UNMASTER_FORCE, SctiptDef.sVAR_UNMASTER_FORCE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_USERGOLDCOUNT) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_USERGOLDCOUNT);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_USERGOLDCOUNT, SctiptDef.sVAR_USERGOLDCOUNT);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_MAXGOLDCOUNT) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_MAXGOLDCOUNT);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_MAXGOLDCOUNT, SctiptDef.sVAR_MAXGOLDCOUNT);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_STORAGEGOLDCOUNT) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_STORAGEGOLDCOUNT);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_STORAGEGOLDCOUNT, SctiptDef.sVAR_STORAGEGOLDCOUNT);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_BINDGOLDCOUNT) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_BINDGOLDCOUNT);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_BINDGOLDCOUNT, SctiptDef.sVAR_BINDGOLDCOUNT);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_UPGRADEWEAPONFEE) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_UPGRADEWEAPONFEE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_UPGRADEWEAPONFEE, SctiptDef.sVAR_UPGRADEWEAPONFEE);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_USERWEAPON) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_USERWEAPON);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_USERWEAPON, SctiptDef.sVAR_USERWEAPON);
            }
            else if (sLabel2 == GrobalVarCodeDef.sVAR_CMD_STARTQUEST) {
                sMsg = sMsg.Replace("<" + sLabel + ">", GrobalVarCodeDef.tVAR_CMD_STARTQUEST);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_STARTQUEST, SctiptDef.sVAR_CMD_STARTQUEST);
            }
            //===========================================没有使用的========================================
            #endregion
            else if (HUtil32.CompareLStr(sLabel2, GrobalVarCodeDef.sVAR_TEAM)) {
                s14 = sLabel2.Substring(GrobalVarCodeDef.sVAR_TEAM.Length + 1 - 1, 1);
                if (!string.IsNullOrEmpty(s14)) {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(GrobalVarCodeDef.tVAR_TEAM, s14));
                }
                else {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, GrobalVarCodeDef.sVAR_HUMAN)) {
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (!string.IsNullOrEmpty(s14)) {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(GrobalVarCodeDef.tVAR_HUMAN, s14));
                }
                else {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, GrobalVarCodeDef.sVAR_GUILD)) {
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (!string.IsNullOrEmpty(s14)) {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(GrobalVarCodeDef.tVAR_GUILD, s14));
                }
                else {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, GrobalVarCodeDef.sVAR_GLOBAL)) {
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (!string.IsNullOrEmpty(s14)) {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(GrobalVarCodeDef.tVAR_GLOBAL, s14));
                }
                else {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, GrobalVarCodeDef.sVAR_STR)) {
                //'欢迎使用个人银行储蓄，目前完全免费，请多利用。\ \<您的个人银行存款有/@-1>：<$46><｜/@-2><$125/G18>\ \<您的包裹里以携带有/AUTOCOLOR=249>：<$GOLDCOUNT><｜/@-2><$GOLDCOUNTX>\ \ \<存入金币/@@InPutInteger1>      <取出金币/@@InPutInteger2>      <返 回/@Main>'
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (!string.IsNullOrEmpty(s14)) {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(GrobalVarCodeDef.tVAR_STR, s14));
                }
                else {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, GrobalVarCodeDef.sVAR_MISSIONARITHMOMETER)) {
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (!string.IsNullOrEmpty(s14)) {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(GrobalVarCodeDef.tVAR_MISSIONARITHMOMETER, s14));
                }
                else {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else {
                sMsg = sMsg.Replace("<" + sLabel + ">", "????");
            }
        }

        [GeneratedRegex("#CALL", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.RightToLeft, "zh-CN")]
        private static partial Regex RegexCallCount();
    }
}