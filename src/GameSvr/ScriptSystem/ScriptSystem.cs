using GameSvr.Npc;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using SystemModule;
using SystemModule.Common;

namespace GameSvr.ScriptSystem
{
    public class ScriptSystem
    {
        public int LoadNpcScript(NormNpc NPC, string sPatch, string sScritpName)
        {
            if (sPatch == "")
            {
                sPatch = M2Share.sNpc_def;
            }
            return LoadScriptFile(NPC, sPatch, sScritpName, false); ;
        }

        private bool LoadScriptFile_LoadCallScript(string sFileName, string sLabel, StringList List)
        {
            bool result = false;
            string sLine;
            if (File.Exists(sFileName))
            {
                var LoadStrList = new StringList();
                LoadStrList.LoadFromFile(sFileName);
                sLabel = '[' + sLabel + ']';
                var bo1D = false;
                for (var i = 0; i < LoadStrList.Count; i++)
                {
                    sLine = LoadStrList[i].Trim();
                    if (!string.IsNullOrEmpty(sLine))
                    {
                        if (!bo1D)
                        {
                            if (sLine[0] == '[' && string.Compare(sLine, sLabel, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                bo1D = true;
                                List.Add(sLine);
                            }
                        }
                        else
                        {
                            if (sLine[0] != '{')
                            {
                                if (sLine[0] == '}')
                                {
                                    bo1D = false;
                                    result = true;
                                    break;
                                }
                                else
                                {
                                    List.Add(sLine);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        private int GetScriptCallCount(string sText)
        {
            var match = Regex.Matches(sText, "#CALL", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.RightToLeft);
            return match.Count;
        }

        private string GetCallScriptPath(string path)
        {
            var sCallScriptFile = path;
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

        private readonly Dictionary<string, string> sCallScriptDict = new Dictionary<string, string>();

        private void LoadCallScript(ref StringList LoadList, ref bool success)
        {
            var sLable = string.Empty;
            var callList = new StringList();
            for (var i = 0; i < LoadList.Count; i++)
            {
                var sLine = LoadList[i].Trim();
                callList.AppendText(sLine);
                if (!string.IsNullOrEmpty(sLine) && sLine[0] == '#' && HUtil32.CompareLStr(sLine, "#CALL", "#CALL".Length))
                {
                    sLine = HUtil32.ArrestStringEx(sLine, "[", "]", ref sLable);
                    var sCallScriptFile = GetCallScriptPath(sLable.Trim());
                    var s18 = sLine.Trim();
                    var sFileName = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, "QuestDiary", sCallScriptFile);
                    if (sCallScriptDict.ContainsKey(sFileName))
                    {
                        callList[i] = "#ACT";
                        callList.InsertText(i + 1, "goto " + s18);
                        break;
                    }
                    if (LoadScriptFile_LoadCallScript(sFileName, s18, callList))
                    {
                        callList[i] = "#ACT";
                        callList.InsertText(i + 1, "goto " + s18);
                        if (!sCallScriptDict.ContainsKey(s18))
                        {
                            sCallScriptDict.Add(sFileName, s18);
                        }
                    }
                    else
                    {
                        M2Share.MainOutMessage("script error, load fail: " + sCallScriptFile + s18);
                    }
                }
            }
            LoadList = callList;
            var callCount = GetScriptCallCount(LoadList.Text);
            while (callCount <= 0)
            {
                success = true;
                break;
            }
        }

        private string LoadScriptFile_LoadDefineInfo(StringList LoadList, IList<TDefineInfo> List)
        {
            var result = string.Empty;
            var s14 = string.Empty;
            var s28 = string.Empty;
            var s1C = string.Empty;
            var s20 = string.Empty;
            var s24 = string.Empty;
            TDefineInfo DefineInfo;
            StringList LoadStrList;
            for (var i = 0; i < LoadList.Count; i++)
            {
                s14 = LoadList[i].Trim();
                if (s14 != "" && s14[0] == '#')
                {
                    if (HUtil32.CompareLStr(s14, "#SETHOME", "#SETHOME".Length))
                    {
                        result = HUtil32.GetValidStr3(s14, ref s1C, new[] { " ", "\t" }).Trim();
                        LoadList[i] = "";
                    }
                    if (HUtil32.CompareLStr(s14, "#DEFINE", "#DEFINE".Length))
                    {
                        s14 = HUtil32.GetValidStr3(s14, ref s1C, new[] { " ", "\t" });
                        s14 = HUtil32.GetValidStr3(s14, ref s20, new[] { " ", "\t" });
                        s14 = HUtil32.GetValidStr3(s14, ref s24, new[] { " ", "\t" });
                        DefineInfo = new TDefineInfo
                        {
                            sName = s20.ToUpper(),
                            sText = s24
                        };
                        List.Add(DefineInfo);
                        LoadList[i] = "";
                    }
                    if (HUtil32.CompareLStr(s14, "#INCLUDE", "#INCLUDE".Length))
                    {
                        s28 = HUtil32.GetValidStr3(s14, ref s1C, new[] { " ", "\t" }).Trim();
                        s28 = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, "Defines", s28);
                        if (File.Exists(s28))
                        {
                            LoadStrList = new StringList();
                            LoadStrList.LoadFromFile(s28);
                            result = LoadScriptFile_LoadDefineInfo(LoadStrList, List);
                        }
                        else
                        {
                            M2Share.MainOutMessage("script error, load fail: " + s28);
                        }
                        LoadList[i] = "";
                    }
                }
            }
            return result;
        }

        private TScript LoadScriptFile_MakeNewScript(NormNpc NPC)
        {
            TScript ScriptInfo = new TScript
            {
                boQuest = false,
                RecordList = new Dictionary<string, TSayingRecord>(StringComparer.OrdinalIgnoreCase)
            };
            NPC.m_ScriptList.Add(ScriptInfo);
            return ScriptInfo;
        }

        private bool LoadScriptFile_QuestCondition(string sText, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var sCmd = string.Empty;
            var sParam1 = string.Empty;
            var sParam2 = string.Empty;
            var sParam3 = string.Empty;
            var sParam4 = string.Empty;
            var sParam5 = string.Empty;
            var sParam6 = string.Empty;
            var nCMDCode = 0;
            sText = HUtil32.GetValidStrCap(sText, ref sCmd, new[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam1, new[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam2, new[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam3, new[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam4, new[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam5, new[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam6, new[] { " ", "\t" });
            sCmd = sCmd.ToUpper();
            switch (sCmd)
            {
                case ScriptDef.sCHECK:
                    {
                        nCMDCode = ScriptDef.nCHECK;
                        HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                        if (!HUtil32.IsStringNumber(sParam1))
                        {
                            nCMDCode = 0;
                        }
                        if (!HUtil32.IsStringNumber(sParam2))
                        {
                            nCMDCode = 0;
                        }
                        goto L001;
                    }
                case ScriptDef.sCHECKOPEN:
                    {
                        nCMDCode = ScriptDef.nCHECKOPEN;
                        HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                        if (!HUtil32.IsStringNumber(sParam1))
                        {
                            nCMDCode = 0;
                        }
                        if (!HUtil32.IsStringNumber(sParam2))
                        {
                            nCMDCode = 0;
                        }
                        goto L001;
                    }
                case ScriptDef.sCHECKUNIT:
                    {
                        nCMDCode = ScriptDef.nCHECKUNIT;
                        HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                        if (!HUtil32.IsStringNumber(sParam1))
                        {
                            nCMDCode = 0;
                        }
                        if (!HUtil32.IsStringNumber(sParam2))
                        {
                            nCMDCode = 0;
                        }
                        goto L001;
                    }
                case ScriptDef.sCHECKPKPOINT:
                    nCMDCode = ScriptDef.nCHECKPKPOINT;
                    goto L001;
                case ScriptDef.sCHECKGOLD:
                    nCMDCode = ScriptDef.nCHECKGOLD;
                    goto L001;
                case ScriptDef.sCHECKLEVEL:
                    nCMDCode = ScriptDef.nCHECKLEVEL;
                    goto L001;
                case ScriptDef.sCHECKJOB:
                    nCMDCode = ScriptDef.nCHECKJOB;
                    goto L001;
                case ScriptDef.sRANDOM:
                    nCMDCode = ScriptDef.nRANDOM;
                    goto L001;
                case ScriptDef.sCHECKITEM:
                    nCMDCode = ScriptDef.nCHECKITEM;
                    goto L001;
                case ScriptDef.sGENDER:
                    nCMDCode = ScriptDef.nGENDER;
                    goto L001;
                case ScriptDef.sCHECKBAGGAGE:
                    nCMDCode = ScriptDef.nCHECKBAGGAGE;
                    goto L001;
                case ScriptDef.sCHECKNAMELIST:
                    nCMDCode = ScriptDef.nCHECKNAMELIST;
                    goto L001;
                case ScriptDef.sSC_HASGUILD:
                    nCMDCode = ScriptDef.nSC_HASGUILD;
                    goto L001;
                case ScriptDef.sSC_ISGUILDMASTER:
                    nCMDCode = ScriptDef.nSC_ISGUILDMASTER;
                    goto L001;
                case ScriptDef.sSC_CHECKCASTLEMASTER:
                    nCMDCode = ScriptDef.nSC_CHECKCASTLEMASTER;
                    goto L001;
                case ScriptDef.sSC_ISNEWHUMAN:
                    nCMDCode = ScriptDef.nSC_ISNEWHUMAN;
                    goto L001;
                case ScriptDef.sSC_CHECKMEMBERTYPE:
                    nCMDCode = ScriptDef.nSC_CHECKMEMBERTYPE;
                    goto L001;
                case ScriptDef.sSC_CHECKMEMBERLEVEL:
                    nCMDCode = ScriptDef.nSC_CHECKMEMBERLEVEL;
                    goto L001;
                case ScriptDef.sSC_CHECKGAMEGOLD:
                    nCMDCode = ScriptDef.nSC_CHECKGAMEGOLD;
                    goto L001;
                case ScriptDef.sSC_CHECKGAMEPOINT:
                    nCMDCode = ScriptDef.nSC_CHECKGAMEPOINT;
                    goto L001;
                case ScriptDef.sSC_CHECKNAMELISTPOSITION:
                    nCMDCode = ScriptDef.nSC_CHECKNAMELISTPOSITION;
                    goto L001;
                case ScriptDef.sSC_CHECKGUILDLIST:
                    nCMDCode = ScriptDef.nSC_CHECKGUILDLIST;
                    goto L001;
                case ScriptDef.sSC_CHECKRENEWLEVEL:
                    nCMDCode = ScriptDef.nSC_CHECKRENEWLEVEL;
                    goto L001;
                case ScriptDef.sSC_CHECKSLAVELEVEL:
                    nCMDCode = ScriptDef.nSC_CHECKSLAVELEVEL;
                    goto L001;
                case ScriptDef.sSC_CHECKSLAVENAME:
                    nCMDCode = ScriptDef.nSC_CHECKSLAVENAME;
                    goto L001;
                case ScriptDef.sSC_CHECKCREDITPOINT:
                    nCMDCode = ScriptDef.nSC_CHECKCREDITPOINT;
                    goto L001;
                case ScriptDef.sSC_CHECKOFGUILD:
                    nCMDCode = ScriptDef.nSC_CHECKOFGUILD;
                    goto L001;
                case ScriptDef.sSC_CHECKPAYMENT:
                    nCMDCode = ScriptDef.nSC_CHECKPAYMENT;
                    goto L001;
                case ScriptDef.sSC_CHECKUSEITEM:
                    nCMDCode = ScriptDef.nSC_CHECKUSEITEM;
                    goto L001;
                case ScriptDef.sSC_CHECKBAGSIZE:
                    nCMDCode = ScriptDef.nSC_CHECKBAGSIZE;
                    goto L001;
                case ScriptDef.sSC_CHECKLISTCOUNT:
                    nCMDCode = ScriptDef.nSC_CHECKLISTCOUNT;
                    goto L001;
                case ScriptDef.sSC_CHECKDC:
                    nCMDCode = ScriptDef.nSC_CHECKDC;
                    goto L001;
                case ScriptDef.sSC_CHECKMC:
                    nCMDCode = ScriptDef.nSC_CHECKMC;
                    goto L001;
                case ScriptDef.sSC_CHECKSC:
                    nCMDCode = ScriptDef.nSC_CHECKSC;
                    goto L001;
                case ScriptDef.sSC_CHECKHP:
                    nCMDCode = ScriptDef.nSC_CHECKHP;
                    goto L001;
                case ScriptDef.sSC_CHECKMP:
                    nCMDCode = ScriptDef.nSC_CHECKMP;
                    goto L001;
                case ScriptDef.sSC_CHECKITEMTYPE:
                    nCMDCode = ScriptDef.nSC_CHECKITEMTYPE;
                    goto L001;
                case ScriptDef.sSC_CHECKEXP:
                    nCMDCode = ScriptDef.nSC_CHECKEXP;
                    goto L001;
                case ScriptDef.sSC_CHECKCASTLEGOLD:
                    nCMDCode = ScriptDef.nSC_CHECKCASTLEGOLD;
                    goto L001;
                case ScriptDef.sSC_PASSWORDERRORCOUNT:
                    nCMDCode = ScriptDef.nSC_PASSWORDERRORCOUNT;
                    goto L001;
                case ScriptDef.sSC_ISLOCKPASSWORD:
                    nCMDCode = ScriptDef.nSC_ISLOCKPASSWORD;
                    goto L001;
                case ScriptDef.sSC_ISLOCKSTORAGE:
                    nCMDCode = ScriptDef.nSC_ISLOCKSTORAGE;
                    goto L001;
                case ScriptDef.sSC_CHECKBUILDPOINT:
                    nCMDCode = ScriptDef.nSC_CHECKBUILDPOINT;
                    goto L001;
                case ScriptDef.sSC_CHECKAURAEPOINT:
                    nCMDCode = ScriptDef.nSC_CHECKAURAEPOINT;
                    goto L001;
                case ScriptDef.sSC_CHECKSTABILITYPOINT:
                    nCMDCode = ScriptDef.nSC_CHECKSTABILITYPOINT;
                    goto L001;
                case ScriptDef.sSC_CHECKFLOURISHPOINT:
                    nCMDCode = ScriptDef.nSC_CHECKFLOURISHPOINT;
                    goto L001;
                case ScriptDef.sSC_CHECKCONTRIBUTION:
                    nCMDCode = ScriptDef.nSC_CHECKCONTRIBUTION;
                    goto L001;
                case ScriptDef.sSC_CHECKRANGEMONCOUNT:
                    nCMDCode = ScriptDef.nSC_CHECKRANGEMONCOUNT;
                    goto L001;
                case ScriptDef.sSC_CHECKITEMADDVALUE:
                    nCMDCode = ScriptDef.nSC_CHECKITEMADDVALUE;
                    goto L001;
                case ScriptDef.sSC_CHECKINMAPRANGE:
                    nCMDCode = ScriptDef.nSC_CHECKINMAPRANGE;
                    goto L001;
                case ScriptDef.sSC_CASTLECHANGEDAY:
                    nCMDCode = ScriptDef.nSC_CASTLECHANGEDAY;
                    goto L001;
                case ScriptDef.sSC_CASTLEWARDAY:
                    nCMDCode = ScriptDef.nSC_CASTLEWARDAY;
                    goto L001;
                case ScriptDef.sSC_ONLINELONGMIN:
                    nCMDCode = ScriptDef.nSC_ONLINELONGMIN;
                    goto L001;
                case ScriptDef.sSC_CHECKGUILDCHIEFITEMCOUNT:
                    nCMDCode = ScriptDef.nSC_CHECKGUILDCHIEFITEMCOUNT;
                    goto L001;
                case ScriptDef.sSC_CHECKNAMEDATELIST:
                    nCMDCode = ScriptDef.nSC_CHECKNAMEDATELIST;
                    goto L001;
                case ScriptDef.sSC_CHECKMAPHUMANCOUNT:
                    nCMDCode = ScriptDef.nSC_CHECKMAPHUMANCOUNT;
                    goto L001;
                case ScriptDef.sSC_CHECKMAPMONCOUNT:
                    nCMDCode = ScriptDef.nSC_CHECKMAPMONCOUNT;
                    goto L001;
                case ScriptDef.sSC_CHECKVAR:
                    nCMDCode = ScriptDef.nSC_CHECKVAR;
                    goto L001;
                case ScriptDef.sSC_CHECKSERVERNAME:
                    nCMDCode = ScriptDef.nSC_CHECKSERVERNAME;
                    goto L001;
                case ScriptDef.sSC_ISATTACKGUILD:
                    nCMDCode = ScriptDef.nSC_ISATTACKGUILD;
                    goto L001;
                case ScriptDef.sSC_ISDEFENSEGUILD:
                    nCMDCode = ScriptDef.nSC_ISDEFENSEGUILD;
                    goto L001;
                case ScriptDef.sSC_ISATTACKALLYGUILD:
                    nCMDCode = ScriptDef.nSC_ISATTACKALLYGUILD;
                    goto L001;
                case ScriptDef.sSC_ISDEFENSEALLYGUILD:
                    nCMDCode = ScriptDef.nSC_ISDEFENSEALLYGUILD;
                    goto L001;
                case ScriptDef.sSC_ISCASTLEGUILD:
                    nCMDCode = ScriptDef.nSC_ISCASTLEGUILD;
                    goto L001;
                case ScriptDef.sSC_CHECKCASTLEDOOR:
                    nCMDCode = ScriptDef.nSC_CHECKCASTLEDOOR;
                    goto L001;
                case ScriptDef.sSC_ISSYSOP:
                    nCMDCode = ScriptDef.nSC_ISSYSOP;
                    goto L001;
                case ScriptDef.sSC_ISADMIN:
                    nCMDCode = ScriptDef.nSC_ISADMIN;
                    goto L001;
                case ScriptDef.sSC_CHECKGROUPCOUNT:
                    nCMDCode = ScriptDef.nSC_CHECKGROUPCOUNT;
                    goto L001;
                case ScriptDef.sCHECKACCOUNTLIST:
                    nCMDCode = ScriptDef.nCHECKACCOUNTLIST;
                    goto L001;
                case ScriptDef.sCHECKIPLIST:
                    nCMDCode = ScriptDef.nCHECKIPLIST;
                    goto L001;
                case ScriptDef.sCHECKBBCOUNT:
                    nCMDCode = ScriptDef.nCHECKBBCOUNT;
                    goto L001;
            }

            if (sCmd == ScriptDef.sCHECKCREDITPOINT)
            {
                nCMDCode = ScriptDef.nCHECKCREDITPOINT;
                goto L001;
            }
            if (sCmd == ScriptDef.sDAYTIME)
            {
                nCMDCode = ScriptDef.nDAYTIME;
                goto L001;
            }
            if (sCmd == ScriptDef.sCHECKITEMW)
            {
                nCMDCode = ScriptDef.nCHECKITEMW;
                goto L001;
            }
            if (sCmd == ScriptDef.sISTAKEITEM)
            {
                nCMDCode = ScriptDef.nISTAKEITEM;
                goto L001;
            }
            if (sCmd == ScriptDef.sCHECKDURA)
            {
                nCMDCode = ScriptDef.nCHECKDURA;
                goto L001;
            }
            if (sCmd == ScriptDef.sCHECKDURAEVA)
            {
                nCMDCode = ScriptDef.nCHECKDURAEVA;
                goto L001;
            }
            if (sCmd == ScriptDef.sDAYOFWEEK)
            {
                nCMDCode = ScriptDef.nDAYOFWEEK;
                goto L001;
            }
            if (sCmd == ScriptDef.sHOUR)
            {
                nCMDCode = ScriptDef.nHOUR;
                goto L001;
            }
            if (sCmd == ScriptDef.sMIN)
            {
                nCMDCode = ScriptDef.nMIN;
                goto L001;
            }
            if (sCmd == ScriptDef.sCHECKLUCKYPOINT)
            {
                nCMDCode = ScriptDef.nCHECKLUCKYPOINT;
                goto L001;
            }
            if (sCmd == ScriptDef.sCHECKMONMAP)
            {
                nCMDCode = ScriptDef.nCHECKMONMAP;
                goto L001;
            }
            if (sCmd == ScriptDef.sCHECKMONAREA)
            {
                nCMDCode = ScriptDef.nCHECKMONAREA;
                goto L001;
            }
            if (sCmd == ScriptDef.sCHECKHUM)
            {
                nCMDCode = ScriptDef.nCHECKHUM;
                goto L001;
            }
            if (sCmd == ScriptDef.sEQUAL)
            {
                nCMDCode = ScriptDef.nEQUAL;
                goto L001;
            }
            if (sCmd == ScriptDef.sLARGE)
            {
                nCMDCode = ScriptDef.nLARGE;
                goto L001;
            }
            if (sCmd == ScriptDef.sSMALL)
            {
                nCMDCode = ScriptDef.nSMALL;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_CHECKPOSEDIR)
            {
                nCMDCode = ScriptDef.nSC_CHECKPOSEDIR;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_CHECKPOSELEVEL)
            {
                nCMDCode = ScriptDef.nSC_CHECKPOSELEVEL;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_CHECKPOSEGENDER)
            {
                nCMDCode = ScriptDef.nSC_CHECKPOSEGENDER;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_CHECKLEVELEX)
            {
                nCMDCode = ScriptDef.nSC_CHECKLEVELEX;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_CHECKBONUSPOINT)
            {
                nCMDCode = ScriptDef.nSC_CHECKBONUSPOINT;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_CHECKMARRY)
            {
                nCMDCode = ScriptDef.nSC_CHECKMARRY;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_CHECKPOSEMARRY)
            {
                nCMDCode = ScriptDef.nSC_CHECKPOSEMARRY;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_CHECKMARRYCOUNT)
            {
                nCMDCode = ScriptDef.nSC_CHECKMARRYCOUNT;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_CHECKMASTER)
            {
                nCMDCode = ScriptDef.nSC_CHECKMASTER;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_HAVEMASTER)
            {
                nCMDCode = ScriptDef.nSC_HAVEMASTER;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_CHECKPOSEMASTER)
            {
                nCMDCode = ScriptDef.nSC_CHECKPOSEMASTER;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_POSEHAVEMASTER)
            {
                nCMDCode = ScriptDef.nSC_POSEHAVEMASTER;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_CHECKISMASTER)
            {
                nCMDCode = ScriptDef.nSC_CHECKISMASTER;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_CHECKPOSEISMASTER)
            {
                nCMDCode = ScriptDef.nSC_CHECKPOSEISMASTER;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_CHECKNAMEIPLIST)
            {
                nCMDCode = ScriptDef.nSC_CHECKNAMEIPLIST;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_CHECKACCOUNTIPLIST)
            {
                nCMDCode = ScriptDef.nSC_CHECKACCOUNTIPLIST;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_CHECKSLAVECOUNT)
            {
                nCMDCode = ScriptDef.nSC_CHECKSLAVECOUNT;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_CHECKPOS)
            {
                nCMDCode = ScriptDef.nSC_CHECKPOS;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_CHECKMAP)
            {
                nCMDCode = ScriptDef.nSC_CHECKMAP;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_REVIVESLAVE)
            {
                nCMDCode = ScriptDef.nSC_REVIVESLAVE;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_CHECKMAGICLVL)
            {
                nCMDCode = ScriptDef.nSC_CHECKMAGICLVL;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_CHECKGROUPCLASS)
            {
                nCMDCode = ScriptDef.nSC_CHECKGROUPCLASS;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_ISGROUPMASTER)
            {
                nCMDCode = ScriptDef.nSC_ISGROUPMASTER;
                goto L001;
            }
            if (sCmd == ScriptDef.sCheckDiemon)
            {
                nCMDCode = ScriptDef.nCheckDiemon;
                goto L001;
            }
            if (sCmd == ScriptDef.scheckkillplaymon)
            {
                nCMDCode = ScriptDef.ncheckkillplaymon;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_CHECKRANDOMNO)
            {
                nCMDCode = ScriptDef.nSC_CHECKRANDOMNO;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_CHECKISONMAP)
            {
                nCMDCode = ScriptDef.nSC_CHECKISONMAP;
                goto L001;
            }
            // 检测是否安全区
            if (sCmd == ScriptDef.sSC_CHECKINSAFEZONE)
            {
                nCMDCode = ScriptDef.nSC_CHECKINSAFEZONE;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_KILLBYHUM)
            {
                nCMDCode = ScriptDef.nSC_KILLBYHUM;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_KILLBYMON)
            {
                nCMDCode = ScriptDef.nSC_KILLBYMON;
                goto L001;
            }
            // 增加挂机
            if (sCmd == ScriptDef.sSC_OffLine)
            {
                nCMDCode = ScriptDef.nSC_OffLine;
                goto L001;
            }
            // 增加脚本特修所有装备命令
            if (sCmd == ScriptDef.sSC_REPAIRALL)
            {
                nCMDCode = ScriptDef.nSC_REPAIRALL;
                goto L001;
            }
            // 刷新包裹物品命令
            if (sCmd == ScriptDef.sSC_QUERYBAGITEMS)
            {
                nCMDCode = ScriptDef.nSC_QUERYBAGITEMS;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_SETRANDOMNO)
            {
                nCMDCode = ScriptDef.nSC_SETRANDOMNO;
                goto L001;
            }
            if (sCmd == ScriptDef.sSC_DELAYGOTO || sCmd == "DELAYCALL")
            {
                nCMDCode = ScriptDef.nSC_DELAYGOTO;
                goto L001;
            }
            if (sCmd == ScriptDef.sSCHECKDEATHPLAYMON)
            {
                nCMDCode = ScriptDef.nSCHECKDEATHPLAYMON;
                goto L001;
            }
            if (sCmd == ScriptDef.sSCHECKKILLMOBNAME)
            {
                nCMDCode = ScriptDef.nSCHECKDEATHPLAYMON;
                goto L001;
            }
        // ------------------------------
        L001:
            if (nCMDCode > 0)
            {
                QuestConditionInfo.nCmdCode = nCMDCode;
                if (sParam1 != "" && sParam1[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam1, "\"", "\"", ref sParam1);
                }
                if (sParam2 != "" && sParam2[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam2, "\"", "\"", ref sParam2);
                }
                if (sParam3 != "" && sParam3[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam3, "\"", "\"", ref sParam3);
                }
                if (sParam4 != "" && sParam4[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam4, "\"", "\"", ref sParam4);
                }
                if (sParam5 != "" && sParam5[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam5, "\"", "\"", ref sParam5);
                }
                if (sParam6 != "" && sParam6[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam6, "\"", "\"", ref sParam6);
                }
                QuestConditionInfo.sParam1 = sParam1;
                QuestConditionInfo.sParam2 = sParam2;
                QuestConditionInfo.sParam3 = sParam3;
                QuestConditionInfo.sParam4 = sParam4;
                QuestConditionInfo.sParam5 = sParam5;
                QuestConditionInfo.sParam6 = sParam6;
                if (HUtil32.IsStringNumber(sParam1))
                {
                    QuestConditionInfo.nParam1 = HUtil32.Str_ToInt(sParam1, 0);
                }
                if (HUtil32.IsStringNumber(sParam2))
                {
                    QuestConditionInfo.nParam2 = HUtil32.Str_ToInt(sParam2, 0);
                }
                if (HUtil32.IsStringNumber(sParam3))
                {
                    QuestConditionInfo.nParam3 = HUtil32.Str_ToInt(sParam3, 0);
                }
                if (HUtil32.IsStringNumber(sParam4))
                {
                    QuestConditionInfo.nParam4 = HUtil32.Str_ToInt(sParam4, 0);
                }
                if (HUtil32.IsStringNumber(sParam5))
                {
                    QuestConditionInfo.nParam5 = HUtil32.Str_ToInt(sParam5, 0);
                }
                if (HUtil32.IsStringNumber(sParam6))
                {
                    QuestConditionInfo.nParam6 = HUtil32.Str_ToInt(sParam6, 0);
                }
                result = true;
            }
            return result;
        }

        private bool LoadScriptFile_QuestAction(string sText, TQuestActionInfo QuestActionInfo)
        {
            var sCmd = string.Empty;
            var sParam1 = string.Empty;
            var sParam2 = string.Empty;
            var sParam3 = string.Empty;
            var sParam4 = string.Empty;
            var sParam5 = string.Empty;
            var sParam6 = string.Empty;
            int nCMDCode;
            var result = false;
            sText = HUtil32.GetValidStrCap(sText, ref sCmd, new[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam1, new[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam2, new[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam3, new[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam4, new[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam5, new[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam6, new[] { " ", "\t" });
            sCmd = sCmd.ToUpper();
            nCMDCode = 0;
            switch (sCmd)
            {
                case ScriptDef.sSET:
                    {
                        nCMDCode = ScriptDef.nSET;
                        HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                        if (!HUtil32.IsStringNumber(sParam1))
                        {
                            nCMDCode = 0;
                        }
                        if (!HUtil32.IsStringNumber(sParam2))
                        {
                            nCMDCode = 0;
                        }
                        break;
                    }
                case ScriptDef.sRESET:
                    {
                        nCMDCode = ScriptDef.nRESET;
                        HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                        if (!HUtil32.IsStringNumber(sParam1))
                        {
                            nCMDCode = 0;
                        }
                        if (!HUtil32.IsStringNumber(sParam2))
                        {
                            nCMDCode = 0;
                        }
                        break;
                    }
                case ScriptDef.sSETOPEN:
                    {
                        nCMDCode = ScriptDef.nSETOPEN;
                        HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                        if (!HUtil32.IsStringNumber(sParam1))
                        {
                            nCMDCode = 0;
                        }
                        if (!HUtil32.IsStringNumber(sParam2))
                        {
                            nCMDCode = 0;
                        }
                        break;
                    }
                case ScriptDef.sSETUNIT:
                    {
                        nCMDCode = ScriptDef.nSETUNIT;
                        HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                        if (!HUtil32.IsStringNumber(sParam1))
                        {
                            nCMDCode = 0;
                        }
                        if (!HUtil32.IsStringNumber(sParam2))
                        {
                            nCMDCode = 0;
                        }
                        break;
                    }
                case ScriptDef.sRESETUNIT:
                    {
                        nCMDCode = ScriptDef.nRESETUNIT;
                        HUtil32.ArrestStringEx(sParam1, "[", "]", ref sParam1);
                        if (!HUtil32.IsStringNumber(sParam1))
                        {
                            nCMDCode = 0;
                        }
                        if (!HUtil32.IsStringNumber(sParam2))
                        {
                            nCMDCode = 0;
                        }

                        break;
                    }
                case ScriptDef.sTAKE:
                    nCMDCode = ScriptDef.nTAKE;
                    goto L001;
                case ScriptDef.sSC_GIVE:
                    nCMDCode = ScriptDef.nSC_GIVE;
                    goto L001;
                case ScriptDef.sCLOSE:
                    nCMDCode = ScriptDef.nCLOSE;
                    goto L001;
                case ScriptDef.sBREAK:
                    nCMDCode = ScriptDef.nBREAK;
                    goto L001;
                case ScriptDef.sGOTO:
                    nCMDCode = ScriptDef.nGOTO;
                    goto L001;
                case ScriptDef.sADDNAMELIST:
                    nCMDCode = ScriptDef.nADDNAMELIST;
                    goto L001;
                case ScriptDef.sDELNAMELIST:
                    nCMDCode = ScriptDef.nDELNAMELIST;
                    goto L001;
                case ScriptDef.sADDGUILDLIST:
                    nCMDCode = ScriptDef.nADDGUILDLIST;
                    goto L001;
                case ScriptDef.sDELGUILDLIST:
                    nCMDCode = ScriptDef.nDELGUILDLIST;
                    goto L001;
                case ScriptDef.sSC_LINEMSG:
                    nCMDCode = ScriptDef.nSC_LINEMSG;
                    goto L001;
                case ScriptDef.sADDACCOUNTLIST:
                    nCMDCode = ScriptDef.nADDACCOUNTLIST;
                    goto L001;
                case ScriptDef.sDELACCOUNTLIST:
                    nCMDCode = ScriptDef.nDELACCOUNTLIST;
                    goto L001;
                case ScriptDef.sADDIPLIST:
                    nCMDCode = ScriptDef.nADDIPLIST;
                    goto L001;
                case ScriptDef.sDELIPLIST:
                    nCMDCode = ScriptDef.nDELIPLIST;
                    goto L001;
                case ScriptDef.sSENDMSG:
                    nCMDCode = ScriptDef.nSENDMSG;
                    goto L001;
                case ScriptDef.sCHANGEMODE:
                    nCMDCode = ScriptDef.nCHANGEMODE;
                    nCMDCode = ScriptDef.nSC_CHANGEMODE;
                    goto L001;
                case ScriptDef.sPKPOINT:
                    nCMDCode = ScriptDef.nPKPOINT;
                    goto L001;
                case ScriptDef.sCHANGEXP:
                    nCMDCode = ScriptDef.nCHANGEXP;
                    goto L001;
                case ScriptDef.sSC_RECALLMOB:
                    nCMDCode = ScriptDef.nSC_RECALLMOB;
                    goto L001;
                case ScriptDef.sTAKEW:
                    nCMDCode = ScriptDef.nTAKEW;
                    goto L001;
                case ScriptDef.sTIMERECALL:
                    nCMDCode = ScriptDef.nTIMERECALL;
                    goto L001;
                case ScriptDef.sSC_PARAM1:
                    nCMDCode = ScriptDef.nSC_PARAM1;
                    goto L001;
                case ScriptDef.sSC_PARAM2:
                    nCMDCode = ScriptDef.nSC_PARAM2;
                    goto L001;
                case ScriptDef.sSC_PARAM3:
                    nCMDCode = ScriptDef.nSC_PARAM3;
                    goto L001;
                case ScriptDef.sSC_PARAM4:
                    nCMDCode = ScriptDef.nSC_PARAM4;
                    goto L001;
                case ScriptDef.sSC_EXEACTION:
                    nCMDCode = ScriptDef.nSC_EXEACTION;
                    goto L001;
                case ScriptDef.sMAPMOVE:
                    nCMDCode = ScriptDef.nMAPMOVE;
                    goto L001;
                case ScriptDef.sMAP:
                    nCMDCode = ScriptDef.nMAP;
                    goto L001;
                case ScriptDef.sTAKECHECKITEM:
                    nCMDCode = ScriptDef.nTAKECHECKITEM;
                    goto L001;
                case ScriptDef.sMONGEN:
                    nCMDCode = ScriptDef.nMONGEN;
                    goto L001;
                case ScriptDef.sMONCLEAR:
                    nCMDCode = ScriptDef.nMONCLEAR;
                    goto L001;
                case ScriptDef.sMOV:
                    nCMDCode = ScriptDef.nMOV;
                    goto L001;
                case ScriptDef.sINC:
                    nCMDCode = ScriptDef.nINC;
                    goto L001;
                case ScriptDef.sDEC:
                    nCMDCode = ScriptDef.nDEC;
                    goto L001;
                case ScriptDef.sSUM:
                    nCMDCode = ScriptDef.nSUM;
                    goto L001;
                //变量运算
                //除法
                case ScriptDef.sSC_DIV:
                    nCMDCode = ScriptDef.nSC_DIV;
                    goto L001;
                //除法
                case ScriptDef.sSC_MUL:
                    nCMDCode = ScriptDef.nSC_MUL;
                    goto L001;
                //除法
                case ScriptDef.sSC_PERCENT:
                    nCMDCode = ScriptDef.nSC_PERCENT;
                    goto L001;
                case ScriptDef.sTHROWITEM:
                case ScriptDef.sDROPITEMMAP:
                    nCMDCode = ScriptDef.nTHROWITEM;
                    goto L001;
                case ScriptDef.sBREAKTIMERECALL:
                    nCMDCode = ScriptDef.nBREAKTIMERECALL;
                    goto L001;
                case ScriptDef.sMOVR:
                    nCMDCode = ScriptDef.nMOVR;
                    goto L001;
                case ScriptDef.sEXCHANGEMAP:
                    nCMDCode = ScriptDef.nEXCHANGEMAP;
                    goto L001;
                case ScriptDef.sRECALLMAP:
                    nCMDCode = ScriptDef.nRECALLMAP;
                    goto L001;
                case ScriptDef.sADDBATCH:
                    nCMDCode = ScriptDef.nADDBATCH;
                    goto L001;
                case ScriptDef.sBATCHDELAY:
                    nCMDCode = ScriptDef.nBATCHDELAY;
                    goto L001;
                case ScriptDef.sBATCHMOVE:
                    nCMDCode = ScriptDef.nBATCHMOVE;
                    goto L001;
                case ScriptDef.sPLAYDICE:
                    nCMDCode = ScriptDef.nPLAYDICE;
                    goto L001;
                case ScriptDef.sGOQUEST:
                    nCMDCode = ScriptDef.nGOQUEST;
                    goto L001;
                case ScriptDef.sENDQUEST:
                    nCMDCode = ScriptDef.nENDQUEST;
                    goto L001;
                case ScriptDef.sSC_HAIRCOLOR:
                    nCMDCode = ScriptDef.nSC_HAIRCOLOR;
                    goto L001;
                case ScriptDef.sSC_WEARCOLOR:
                    nCMDCode = ScriptDef.nSC_WEARCOLOR;
                    goto L001;
                case ScriptDef.sSC_HAIRSTYLE:
                    nCMDCode = ScriptDef.nSC_HAIRSTYLE;
                    goto L001;
                case ScriptDef.sSC_MONRECALL:
                    nCMDCode = ScriptDef.nSC_MONRECALL;
                    goto L001;
                case ScriptDef.sSC_HORSECALL:
                    nCMDCode = ScriptDef.nSC_HORSECALL;
                    goto L001;
                case ScriptDef.sSC_HAIRRNDCOL:
                    nCMDCode = ScriptDef.nSC_HAIRRNDCOL;
                    goto L001;
                case ScriptDef.sSC_KILLHORSE:
                    nCMDCode = ScriptDef.nSC_KILLHORSE;
                    goto L001;
                case ScriptDef.sSC_RANDSETDAILYQUEST:
                    nCMDCode = ScriptDef.nSC_RANDSETDAILYQUEST;
                    goto L001;
                case ScriptDef.sSC_CHANGELEVEL:
                    nCMDCode = ScriptDef.nSC_CHANGELEVEL;
                    goto L001;
                case ScriptDef.sSC_MARRY:
                    nCMDCode = ScriptDef.nSC_MARRY;
                    goto L001;
                case ScriptDef.sSC_UNMARRY:
                    nCMDCode = ScriptDef.nSC_UNMARRY;
                    goto L001;
                case ScriptDef.sSC_GETMARRY:
                    nCMDCode = ScriptDef.nSC_GETMARRY;
                    goto L001;
                case ScriptDef.sSC_GETMASTER:
                    nCMDCode = ScriptDef.nSC_GETMASTER;
                    goto L001;
                case ScriptDef.sSC_CLEARSKILL:
                    nCMDCode = ScriptDef.nSC_CLEARSKILL;
                    goto L001;
                case ScriptDef.sSC_DELNOJOBSKILL:
                    nCMDCode = ScriptDef.nSC_DELNOJOBSKILL;
                    goto L001;
                case ScriptDef.sSC_DELSKILL:
                    nCMDCode = ScriptDef.nSC_DELSKILL;
                    goto L001;
                case ScriptDef.sSC_ADDSKILL:
                    nCMDCode = ScriptDef.nSC_ADDSKILL;
                    goto L001;
                case ScriptDef.sSC_SKILLLEVEL:
                    nCMDCode = ScriptDef.nSC_SKILLLEVEL;
                    goto L001;
                case ScriptDef.sSC_CHANGEPKPOINT:
                    nCMDCode = ScriptDef.nSC_CHANGEPKPOINT;
                    goto L001;
                case ScriptDef.sSC_CHANGEEXP:
                    nCMDCode = ScriptDef.nSC_CHANGEEXP;
                    goto L001;
                case ScriptDef.sSC_CHANGEJOB:
                    nCMDCode = ScriptDef.nSC_CHANGEJOB;
                    goto L001;
                case ScriptDef.sSC_MISSION:
                    nCMDCode = ScriptDef.nSC_MISSION;
                    goto L001;
                case ScriptDef.sSC_MOBPLACE:
                    nCMDCode = ScriptDef.nSC_MOBPLACE;
                    goto L001;
                case ScriptDef.sSC_SETMEMBERTYPE:
                    nCMDCode = ScriptDef.nSC_SETMEMBERTYPE;
                    goto L001;
                case ScriptDef.sSC_SETMEMBERLEVEL:
                    nCMDCode = ScriptDef.nSC_SETMEMBERLEVEL;
                    goto L001;
                case ScriptDef.sSC_GAMEGOLD:
                    nCMDCode = ScriptDef.nSC_GAMEGOLD;
                    goto L001;
                case ScriptDef.sSC_GAMEPOINT:
                    nCMDCode = ScriptDef.nSC_GAMEPOINT;
                    goto L001;
                case ScriptDef.sSC_PKZONE:
                    nCMDCode = ScriptDef.nSC_PKZONE;
                    goto L001;
                case ScriptDef.sSC_RESTBONUSPOINT:
                    nCMDCode = ScriptDef.nSC_RESTBONUSPOINT;
                    goto L001;
                case ScriptDef.sSC_TAKECASTLEGOLD:
                    nCMDCode = ScriptDef.nSC_TAKECASTLEGOLD;
                    goto L001;
                case ScriptDef.sSC_HUMANHP:
                    nCMDCode = ScriptDef.nSC_HUMANHP;
                    goto L001;
                case ScriptDef.sSC_HUMANMP:
                    nCMDCode = ScriptDef.nSC_HUMANMP;
                    goto L001;
                case ScriptDef.sSC_BUILDPOINT:
                    nCMDCode = ScriptDef.nSC_BUILDPOINT;
                    goto L001;
                case ScriptDef.sSC_AURAEPOINT:
                    nCMDCode = ScriptDef.nSC_AURAEPOINT;
                    goto L001;
                case ScriptDef.sSC_STABILITYPOINT:
                    nCMDCode = ScriptDef.nSC_STABILITYPOINT;
                    goto L001;
                case ScriptDef.sSC_FLOURISHPOINT:
                    nCMDCode = ScriptDef.nSC_FLOURISHPOINT;
                    goto L001;
                case ScriptDef.sSC_OPENMAGICBOX:
                    nCMDCode = ScriptDef.nSC_OPENMAGICBOX;
                    goto L001;
                case ScriptDef.sSC_SETRANKLEVELNAME:
                    nCMDCode = ScriptDef.nSC_SETRANKLEVELNAME;
                    goto L001;
                case ScriptDef.sSC_GMEXECUTE:
                    nCMDCode = ScriptDef.nSC_GMEXECUTE;
                    goto L001;
                case ScriptDef.sSC_GUILDCHIEFITEMCOUNT:
                    nCMDCode = ScriptDef.nSC_GUILDCHIEFITEMCOUNT;
                    goto L001;
                case ScriptDef.sSC_ADDNAMEDATELIST:
                    nCMDCode = ScriptDef.nSC_ADDNAMEDATELIST;
                    goto L001;
                case ScriptDef.sSC_DELNAMEDATELIST:
                    nCMDCode = ScriptDef.nSC_DELNAMEDATELIST;
                    goto L001;
                case ScriptDef.sSC_MOBFIREBURN:
                    nCMDCode = ScriptDef.nSC_MOBFIREBURN;
                    goto L001;
                case ScriptDef.sSC_MESSAGEBOX:
                    nCMDCode = ScriptDef.nSC_MESSAGEBOX;
                    goto L001;
                case ScriptDef.sSC_SETSCRIPTFLAG:
                    nCMDCode = ScriptDef.nSC_SETSCRIPTFLAG;
                    goto L001;
                case ScriptDef.sSC_SETAUTOGETEXP:
                    nCMDCode = ScriptDef.nSC_SETAUTOGETEXP;
                    goto L001;
                case ScriptDef.sSC_VAR:
                    nCMDCode = ScriptDef.nSC_VAR;
                    goto L001;
                case ScriptDef.sSC_LOADVAR:
                    nCMDCode = ScriptDef.nSC_LOADVAR;
                    goto L001;
                case ScriptDef.sSC_SAVEVAR:
                    nCMDCode = ScriptDef.nSC_SAVEVAR;
                    goto L001;
                case ScriptDef.sSC_CALCVAR:
                    nCMDCode = ScriptDef.nSC_CALCVAR;
                    goto L001;
                case ScriptDef.sSC_AUTOADDGAMEGOLD:
                    nCMDCode = ScriptDef.nSC_AUTOADDGAMEGOLD;
                    goto L001;
                case ScriptDef.sSC_AUTOSUBGAMEGOLD:
                    nCMDCode = ScriptDef.nSC_AUTOSUBGAMEGOLD;
                    goto L001;
                case ScriptDef.sSC_RECALLGROUPMEMBERS:
                    nCMDCode = ScriptDef.nSC_RECALLGROUPMEMBERS;
                    goto L001;
                case ScriptDef.sSC_CLEARNAMELIST:
                    nCMDCode = ScriptDef.nSC_CLEARNAMELIST;
                    goto L001;
                case ScriptDef.sSC_CHANGENAMECOLOR:
                    nCMDCode = ScriptDef.nSC_CHANGENAMECOLOR;
                    goto L001;
                case ScriptDef.sSC_CLEARPASSWORD:
                    nCMDCode = ScriptDef.nSC_CLEARPASSWORD;
                    goto L001;
                case ScriptDef.sSC_RENEWLEVEL:
                    nCMDCode = ScriptDef.nSC_RENEWLEVEL;
                    goto L001;
                case ScriptDef.sSC_KILLMONEXPRATE:
                    nCMDCode = ScriptDef.nSC_KILLMONEXPRATE;
                    goto L001;
                case ScriptDef.sSC_POWERRATE:
                    nCMDCode = ScriptDef.nSC_POWERRATE;
                    goto L001;
                case ScriptDef.sSC_CHANGEPERMISSION:
                    nCMDCode = ScriptDef.nSC_CHANGEPERMISSION;
                    goto L001;
                case ScriptDef.sSC_KILL:
                    nCMDCode = ScriptDef.nSC_KILL;
                    goto L001;
                case ScriptDef.sSC_KICK:
                    nCMDCode = ScriptDef.nSC_KICK;
                    goto L001;
                case ScriptDef.sSC_BONUSPOINT:
                    nCMDCode = ScriptDef.nSC_BONUSPOINT;
                    goto L001;
                case ScriptDef.sSC_RESTRENEWLEVEL:
                    nCMDCode = ScriptDef.nSC_RESTRENEWLEVEL;
                    goto L001;
                case ScriptDef.sSC_DELMARRY:
                    nCMDCode = ScriptDef.nSC_DELMARRY;
                    goto L001;
                case ScriptDef.sSC_DELMASTER:
                    nCMDCode = ScriptDef.nSC_DELMASTER;
                    goto L001;
                case ScriptDef.sSC_MASTER:
                    nCMDCode = ScriptDef.nSC_MASTER;
                    goto L001;
                case ScriptDef.sSC_UNMASTER:
                    nCMDCode = ScriptDef.nSC_UNMASTER;
                    goto L001;
                case ScriptDef.sSC_CREDITPOINT:
                    nCMDCode = ScriptDef.nSC_CREDITPOINT;
                    goto L001;
                case ScriptDef.sSC_CLEARNEEDITEMS:
                    nCMDCode = ScriptDef.nSC_CLEARNEEDITEMS;
                    goto L001;
                case ScriptDef.sSC_CLEARMAKEITEMS:
                    nCMDCode = ScriptDef.nSC_CLEARMAEKITEMS;
                    goto L001;
                case ScriptDef.sSC_SETSENDMSGFLAG:
                    nCMDCode = ScriptDef.nSC_SETSENDMSGFLAG;
                    goto L001;
                case ScriptDef.sSC_UPGRADEITEMS:
                    nCMDCode = ScriptDef.nSC_UPGRADEITEMS;
                    goto L001;
                case ScriptDef.sSC_UPGRADEITEMSEX:
                    nCMDCode = ScriptDef.nSC_UPGRADEITEMSEX;
                    goto L001;
                case ScriptDef.sSC_MONGENEX:
                    nCMDCode = ScriptDef.nSC_MONGENEX;
                    goto L001;
                case ScriptDef.sSC_CLEARMAPMON:
                    nCMDCode = ScriptDef.nSC_CLEARMAPMON;
                    goto L001;
                case ScriptDef.sSC_SETMAPMODE:
                    nCMDCode = ScriptDef.nSC_SETMAPMODE;
                    goto L001;
                case ScriptDef.sSC_KILLSLAVE:
                    nCMDCode = ScriptDef.nSC_KILLSLAVE;
                    goto L001;
                case ScriptDef.sSC_CHANGEGENDER:
                    nCMDCode = ScriptDef.nSC_CHANGEGENDER;
                    goto L001;
                case ScriptDef.sSC_MAPTING:
                    nCMDCode = ScriptDef.nSC_MAPTING;
                    goto L001;
                case ScriptDef.sSC_GUILDRECALL:
                    nCMDCode = ScriptDef.nSC_GUILDRECALL;
                    goto L001;
                case ScriptDef.sSC_GROUPRECALL:
                    nCMDCode = ScriptDef.nSC_GROUPRECALL;
                    goto L001;
                case ScriptDef.sSC_GROUPADDLIST:
                    nCMDCode = ScriptDef.nSC_GROUPADDLIST;
                    goto L001;
                case ScriptDef.sSC_CLEARLIST:
                    nCMDCode = ScriptDef.nSC_CLEARLIST;
                    goto L001;
                case ScriptDef.sSC_GROUPMOVEMAP:
                    nCMDCode = ScriptDef.nSC_GROUPMOVEMAP;
                    goto L001;
                case ScriptDef.sSC_SAVESLAVES:
                    nCMDCode = ScriptDef.nSC_SAVESLAVES;
                    goto L001;
                case ScriptDef.sCHECKUSERDATE:
                    nCMDCode = ScriptDef.nCHECKUSERDATE;
                    goto L001;
                case ScriptDef.sADDUSERDATE:
                    nCMDCode = ScriptDef.nADDUSERDATE;
                    goto L001;
                case ScriptDef.sCheckDiemon:
                    nCMDCode = ScriptDef.nCheckDiemon;
                    goto L001;
                case ScriptDef.scheckkillplaymon:
                    nCMDCode = ScriptDef.ncheckkillplaymon;
                    goto L001;
                case ScriptDef.sSC_CHECKRANDOMNO:
                    nCMDCode = ScriptDef.nSC_CHECKRANDOMNO;
                    goto L001;
                case ScriptDef.sSC_CHECKISONMAP:
                    nCMDCode = ScriptDef.nSC_CHECKISONMAP;
                    goto L001;
                // 检测是否安全区
                case ScriptDef.sSC_CHECKINSAFEZONE:
                    nCMDCode = ScriptDef.nSC_CHECKINSAFEZONE;
                    goto L001;
                case ScriptDef.sSC_KILLBYHUM:
                    nCMDCode = ScriptDef.nSC_KILLBYHUM;
                    goto L001;
                case ScriptDef.sSC_KILLBYMON:
                    nCMDCode = ScriptDef.nSC_KILLBYMON;
                    goto L001;
                case ScriptDef.sSC_ISHIGH:
                    nCMDCode = ScriptDef.nSC_ISHIGH;
                    goto L001;
                case ScriptDef.sOPENYBDEAL:
                    nCMDCode = ScriptDef.nOPENYBDEAL;
                    goto L001;
                case ScriptDef.sQUERYYBSELL:
                    nCMDCode = ScriptDef.nQUERYYBSELL;
                    goto L001;
                case ScriptDef.sQUERYYBDEAL:
                    nCMDCode = ScriptDef.nQUERYYBDEAL;
                    goto L001;
                case ScriptDef.sDELAYGOTO:
                case ScriptDef.sDELAYCALL:
                    nCMDCode = ScriptDef.nDELAYGOTO;
                    goto L001;
                case ScriptDef.sCLEARDELAYGOTO:
                    nCMDCode = ScriptDef.nCLEARDELAYGOTO;
                    goto L001;
            }
        L001:
            if (nCMDCode > 0)
            {
                QuestActionInfo.nCmdCode = nCMDCode;
                if (sParam1 != "" && sParam1[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam1, "\"", "\"", ref sParam1);
                }
                if (sParam2 != "" && sParam2[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam2, "\"", "\"", ref sParam2);
                }
                if (sParam3 != "" && sParam3[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam3, "\"", "\"", ref sParam3);
                }
                if (sParam4 != "" && sParam4[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam4, "\"", "\"", ref sParam4);
                }
                if (sParam5 != "" && sParam5[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam5, "\"", "\"", ref sParam5);
                }
                if (sParam6 != "" && sParam6[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam6, "\"", "\"", ref sParam6);
                }
                QuestActionInfo.sParam1 = sParam1;
                QuestActionInfo.sParam2 = sParam2;
                QuestActionInfo.sParam3 = sParam3;
                QuestActionInfo.sParam4 = sParam4;
                QuestActionInfo.sParam5 = sParam5;
                QuestActionInfo.sParam6 = sParam6;
                if (HUtil32.IsStringNumber(sParam1))
                {
                    QuestActionInfo.nParam1 = HUtil32.Str_ToInt(sParam1, 0);
                }
                if (HUtil32.IsStringNumber(sParam2))
                {
                    QuestActionInfo.nParam2 = HUtil32.Str_ToInt(sParam2, 1);
                }
                if (HUtil32.IsStringNumber(sParam3))
                {
                    QuestActionInfo.nParam3 = HUtil32.Str_ToInt(sParam3, 1);
                }
                if (HUtil32.IsStringNumber(sParam4))
                {
                    QuestActionInfo.nParam4 = HUtil32.Str_ToInt(sParam4, 0);
                }
                if (HUtil32.IsStringNumber(sParam5))
                {
                    QuestActionInfo.nParam5 = HUtil32.Str_ToInt(sParam5, 0);
                }
                if (HUtil32.IsStringNumber(sParam6))
                {
                    QuestActionInfo.nParam6 = HUtil32.Str_ToInt(sParam6, 0);
                }
                result = true;
            }
            return result;
        }

        private string GetScriptCrossPath(string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return path.Replace("\\", "/");
            }
            return path;
        }

        /// <summary>
        /// 加载NPC脚本
        /// </summary>
        /// <param name="NPC"></param>
        /// <param name="sPatch"></param>
        /// <param name="sScritpName"></param>
        /// <param name="boFlag"></param>
        /// <returns></returns>
        public int LoadScriptFile(NormNpc NPC, string sPatch, string sScritpName, bool boFlag)
        {
            var s30 = string.Empty;
            var sScript = string.Empty;
            var s38 = string.Empty;
            var s3C = string.Empty;
            var s40 = string.Empty;
            var s44 = string.Empty;
            var s48 = string.Empty;
            var s4C = string.Empty;
            var s50 = string.Empty;
            StringList LoadList;
            IList<TDefineInfo> DefineList;
            TQuestConditionInfo QuestConditionInfo = null;
            TQuestActionInfo QuestActionInfo = null;
            var s54 = string.Empty;
            var s58 = string.Empty;
            var s5C = string.Empty;
            var slabName = string.Empty;
            TDefineInfo DefineInfo;
            var bo8D = false;
            TScript Script = null;
            TSayingRecord SayingRecord = null;
            TSayingProcedure SayingProcedure = null;
            IList<string> ScriptNameList = null;
            List<TQuestActionInfo> GotoList = null;
            List<TQuestActionInfo> DelayGotoList = null;
            List<TQuestActionInfo> PlayDiceList = null;
            var n6C = 0;
            var n70 = 0;
            var sScritpFileName = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, sPatch, GetScriptCrossPath(string.Concat(sScritpName, ".txt")));
            if (File.Exists(sScritpFileName))
            {
                sCallScriptDict.Clear();
                LoadList = new StringList();
                LoadList.LoadFromFile(sScritpFileName);
                var success = false;
                while (!success)
                {
                    LoadCallScript(ref LoadList, ref success);
                }

                DefineList = new List<TDefineInfo>();
                ScriptNameList = new List<string>();
                GotoList = new List<TQuestActionInfo>();
                DelayGotoList = new List<TQuestActionInfo>();
                PlayDiceList = new List<TQuestActionInfo>();
                s54 = LoadScriptFile_LoadDefineInfo(LoadList, DefineList);
                DefineInfo = new TDefineInfo
                {
                    sName = "@HOME"
                };
                if (s54 == "")
                {
                    s54 = "@main";
                }
                DefineInfo.sText = s54;
                DefineList.Add(DefineInfo);
                int n24;
                // 常量处理
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sScript = LoadList[i].Trim();
                    if (!string.IsNullOrEmpty(sScript))
                    {
                        if (sScript[0] == '[')
                        {
                            bo8D = false;
                        }
                        else
                        {
                            if (sScript[0] == '#' && (HUtil32.CompareLStr(sScript, "#IF", "#IF".Length) || HUtil32.CompareLStr(sScript, "#ACT", "#ACT".Length) || HUtil32.CompareLStr(sScript, "#ELSEACT", "#ELSEACT".Length)))
                            {
                                bo8D = true;
                            }
                            else
                            {
                                if (bo8D)
                                {
                                    // 将Define 好的常量换成指定值
                                    for (var n20 = 0; n20 < DefineList.Count; n20++)
                                    {
                                        DefineInfo = DefineList[n20];
                                        var n1C = 0;
                                        while (true)
                                        {
                                            n24 = sScript.ToUpper().IndexOf(DefineInfo.sName, StringComparison.OrdinalIgnoreCase);
                                            if (n24 <= 0)
                                            {
                                                break;
                                            }
                                            s58 = sScript.Substring(0, n24);
                                            s5C = sScript.Substring(DefineInfo.sName.Length + n24);
                                            sScript = s58 + DefineInfo.sText + s5C;
                                            LoadList[i] = sScript;
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
                for (var i = 0; i < DefineList.Count; i++)
                {
                    DefineList[i] = null;
                }
                DefineList.Clear();
                var nQuestIdx = 0;
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sScript = LoadList[i].Trim();
                    if (sScript == "" || sScript[0] == ';' || sScript[0] == '/')
                    {
                        continue;
                    }
                    if (n6C == 0 && boFlag)
                    {
                        if (sScript.StartsWith("%")) // 物品价格倍率
                        {
                            sScript = sScript.Substring(1, sScript.Length - 1);
                            var nPriceRate = HUtil32.Str_ToInt(sScript, -1);
                            if (nPriceRate >= 55)
                            {
                                ((Merchant)NPC).m_nPriceRate = nPriceRate;
                            }
                            continue;
                        }
                        if (sScript.StartsWith("+")) // 物品交易类型
                        {
                            sScript = sScript.Substring(1, sScript.Length - 1);
                            var nItemType = HUtil32.Str_ToInt(sScript, -1);
                            if (nItemType >= 0)
                            {
                                ((Merchant)NPC).m_ItemTypeList.Add(nItemType);
                            }
                            continue;
                        }
                        if (sScript.StartsWith("(")) // 增加处理NPC可执行命令设置
                        {
                            HUtil32.ArrestStringEx(sScript, "(", ")", ref sScript);
                            if (sScript != "")
                            {
                                while (sScript != "")
                                {
                                    sScript = HUtil32.GetValidStr3(sScript, ref s30, new[] { " ", ",", "\t" });
                                    if (s30.Equals(ScriptDef.sBUY, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boBuy = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptDef.sSELL, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boSell = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptDef.sMAKEDURG, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boMakeDrug = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptDef.sPRICES, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boPrices = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptDef.sSTORAGE, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boStorage = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptDef.sGETBACK, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boGetback = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptDef.sUPGRADENOW, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boUpgradenow = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptDef.sGETBACKUPGNOW, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boGetBackupgnow = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptDef.sREPAIR, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boRepair = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptDef.sSUPERREPAIR, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boS_repair = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptDef.sSL_SENDMSG, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boSendmsg = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptDef.sUSEITEMNAME, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boUseItemName = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptDef.sOFFLINEMSG, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boOffLineMsg = true;
                                        continue;
                                    }
                                    if (String.Compare(s30, (ScriptDef.sybdeal), StringComparison.OrdinalIgnoreCase) == 0)
                                    {
                                        ((Merchant)(NPC)).m_boYBDeal = true;
                                        continue;
                                    }
                                }
                            }
                            continue;
                        }
                        // 增加处理NPC可执行命令设置
                    }
                    if (sScript.StartsWith("{"))
                    {
                        if (HUtil32.CompareLStr(sScript, "{Quest", "{Quest".Length))
                        {
                            s38 = HUtil32.GetValidStr3(sScript, ref s3C, new[] { " ", "}", "\t" });
                            HUtil32.GetValidStr3(s38, ref s3C, new[] { " ", "}", "\t" });
                            n70 = HUtil32.Str_ToInt(s3C, 0);
                            Script = LoadScriptFile_MakeNewScript(NPC);
                            Script.nQuest = n70;
                            n70++;
                        }
                        if (HUtil32.CompareLStr(sScript, "{~Quest", "{~Quest".Length))
                        {
                            continue;
                        }
                    }
                    if (n6C == 1 && Script != null && sScript.StartsWith("#"))
                    {
                        s38 = HUtil32.GetValidStr3(sScript, ref s3C, new[] { "=", " ", "\t" });
                        Script.boQuest = true;
                        if (HUtil32.CompareLStr(sScript, "#IF", "#IF".Length))
                        {
                            HUtil32.ArrestStringEx(sScript, "[", "]", ref s40);
                            Script.QuestInfo[nQuestIdx].wFlag = (short)HUtil32.Str_ToInt(s40, 0);
                            HUtil32.GetValidStr3(s38, ref s44, new[] { "=", " ", "\t" });
                            n24 = HUtil32.Str_ToInt(s44, 0);
                            if (n24 != 0)
                            {
                                n24 = 1;
                            }
                            Script.QuestInfo[nQuestIdx].btValue = (byte)n24;
                        }
                        if (HUtil32.CompareLStr(sScript, "#RAND", "#RAND".Length))
                        {
                            Script.QuestInfo[nQuestIdx].nRandRage = HUtil32.Str_ToInt(s44, 0);
                        }
                        continue;
                    }
                    if (sScript.StartsWith("["))
                    {
                        n6C = 10;
                        if (Script == null)
                        {
                            Script = LoadScriptFile_MakeNewScript(NPC);
                            Script.nQuest = n70;
                        }
                        if (sScript.Equals("[goods]", StringComparison.OrdinalIgnoreCase))
                        {
                            n6C = 20;
                            continue;
                        }
                        sScript = HUtil32.ArrestStringEx(sScript, "[", "]", ref slabName);
                        SayingRecord = new TSayingRecord
                        {
                            ProcedureList = new List<TSayingProcedure>(),
                            sLabel = slabName
                        };
                        sScript = HUtil32.GetValidStrCap(sScript, ref slabName, new[] { " ", "\t" });
                        if (slabName.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
                        {
                            SayingRecord.boExtJmp = true;
                        }
                        else
                        {
                            SayingRecord.boExtJmp = false;
                        }
                        SayingProcedure = new TSayingProcedure();
                        SayingRecord.ProcedureList.Add(SayingProcedure);
                        if (Script.RecordList.ContainsKey(SayingRecord.sLabel))
                        {
                            SayingRecord.sLabel += M2Share.RandomNumber.GetRandomNumber(1, 200);
                        }
                        Script.RecordList.Add(SayingRecord.sLabel, SayingRecord);
                        ScriptNameList.Add(SayingRecord.sLabel);
                        continue;
                    }
                    if (Script != null && SayingRecord != null)
                    {
                        if (n6C >= 10 && n6C < 20 && sScript[0] == '#')
                        {
                            if (sScript.Equals("#IF", StringComparison.OrdinalIgnoreCase))
                            {
                                if (SayingProcedure.ConditionList.Count > 0 || SayingProcedure.sSayMsg != "")
                                {
                                    SayingProcedure = new TSayingProcedure();
                                    SayingRecord.ProcedureList.Add(SayingProcedure);
                                }
                                n6C = 11;
                            }
                            if (sScript.Equals("#ACT", StringComparison.OrdinalIgnoreCase))
                            {
                                n6C = 12;
                            }
                            if (sScript.Equals("#SAY", StringComparison.OrdinalIgnoreCase))
                            {
                                n6C = 10;
                            }
                            if (sScript.Equals("#ELSEACT", StringComparison.OrdinalIgnoreCase))
                            {
                                n6C = 13;
                            }
                            if (sScript.Equals("#ELSESAY", StringComparison.OrdinalIgnoreCase))
                            {
                                n6C = 14;
                            }
                            continue;
                        }
                        if (n6C == 10 && SayingProcedure != null)
                        {
                            SayingProcedure.sSayMsg += sScript;
                        }
                        if (n6C == 11)
                        {
                            QuestConditionInfo = new TQuestConditionInfo();
                            if (LoadScriptFile_QuestCondition(sScript.Trim(), QuestConditionInfo))
                            {
                                SayingProcedure.ConditionList.Add(QuestConditionInfo);
                            }
                            else
                            {
                                QuestConditionInfo = null;
                                M2Share.ErrorMessage("脚本错误: " + sScript + " 第:" + i + " 行: " + sScritpFileName);
                            }
                        }
                        if (n6C == 12)
                        {
                            QuestActionInfo = new TQuestActionInfo();
                            if (LoadScriptFile_QuestAction(sScript.Trim(), QuestActionInfo))
                            {
                                SayingProcedure.ActionList.Add(QuestActionInfo);
                            }
                            else
                            {
                                QuestActionInfo = null;
                                M2Share.ErrorMessage("脚本错误: " + sScript + " 第:" + i + " 行: " + sScritpFileName);
                            }
                        }
                        if (n6C == 13)
                        {
                            QuestActionInfo = new TQuestActionInfo();
                            if (LoadScriptFile_QuestAction(sScript.Trim(), QuestActionInfo))
                            {
                                SayingProcedure.ElseActionList.Add(QuestActionInfo);
                            }
                            else
                            {
                                QuestActionInfo = null;
                                M2Share.ErrorMessage("脚本错误: " + sScript + " 第:" + i + " 行: " + sScritpFileName);
                            }
                        }
                        if (n6C == 14)
                        {
                            SayingProcedure.sElseSayMsg = SayingProcedure.sElseSayMsg + sScript;
                        }
                    }
                    if (n6C == 20 && boFlag)
                    {
                        sScript = HUtil32.GetValidStrCap(sScript, ref s48, new[] { " ", "\t" });
                        sScript = HUtil32.GetValidStrCap(sScript, ref s4C, new[] { " ", "\t" });
                        sScript = HUtil32.GetValidStrCap(sScript, ref s50, new[] { " ", "\t" });
                        if (s48 != "" && s50 != "")
                        {
                            if (s48[0] == '\"')
                            {
                                HUtil32.ArrestStringEx(s48, "\"", "\"", ref s48);
                            }
                            if (M2Share.CanSellItem(s48))
                            {
                                var Goods = new TGoods
                                {
                                    sItemName = s48,
                                    nCount = HUtil32.Str_ToInt(s4C, 0),
                                    dwRefillTime = HUtil32.Str_ToInt(s50, 0),
                                    dwRefillTick = 0
                                };
                                ((Merchant)NPC).m_RefillGoodsList.Add(Goods);
                            }
                        }
                    }
                }
                LoadList = null;
                InitializeLabel(NPC, QuestActionInfo, ScriptNameList, PlayDiceList, GotoList,
                                DelayGotoList);
            }
            else
            {
                M2Share.MainOutMessage("Script file not found: " + sScritpFileName);
            }
            return 1;
        }

        /// <summary>
        /// 初始化脚本标签数组
        /// </summary>
        private void InitializeLabel(NormNpc NPC, TQuestActionInfo QuestActionInfo, IList<string> ScriptNameList, List<TQuestActionInfo> PlayDiceList, List<TQuestActionInfo> GotoList, List<TQuestActionInfo> DelayGotoList)
        {
            for (var i = NPC.FGotoLable.GetLowerBound(0); i <= NPC.FGotoLable.GetUpperBound(0); i++)
            {
                NPC.FGotoLable[i] = -1;
            }
            //if (NPC.m_btNPCRaceServer == DataConst.NPC_RC_FUNMERCHANT)
            //{
            //    TFunMerchant FunMerchant = (TFunMerchant)NPC;
            //    for (int i = FunMerchant.FStdModeFunc.GetLowerBound(0); i <= FunMerchant.FStdModeFunc.GetUpperBound(0); i++)
            //    {
            //        FunMerchant.FStdModeFunc[i] = -1;
            //    }
            //    for (int i = FunMerchant.FPlayLevelUp.GetLowerBound(0); i <= FunMerchant.FPlayLevelUp.GetUpperBound(0); i++)
            //    {
            //        FunMerchant.FPlayLevelUp[i] = -1;
            //    }
            //    for (int i = FunMerchant.FUserCmd.GetLowerBound(0); i <= FunMerchant.FUserCmd.GetUpperBound(0); i++)
            //    {
            //        FunMerchant.FUserCmd[i] = -1;
            //    }
            //    for (int i = FunMerchant.FClearMission.GetLowerBound(0); i <= FunMerchant.FClearMission.GetUpperBound(0); i++)
            //    {
            //        FunMerchant.FClearMission[i] = -1;
            //    }
            //    for (int i = FunMerchant.FMagSelfFunc.GetLowerBound(0); i <= FunMerchant.FMagSelfFunc.GetUpperBound(0); i++)
            //    {
            //        FunMerchant.FMagSelfFunc[i] = -1;
            //    }
            //    for (int i = FunMerchant.FMagTagFunc.GetLowerBound(0); i <= FunMerchant.FMagTagFunc.GetUpperBound(0); i++)
            //    {
            //        FunMerchant.FMagTagFunc[i] = -1;
            //    }
            //    for (int i = FunMerchant.FMagTagFuncEx.GetLowerBound(0); i <= FunMerchant.FMagTagFuncEx.GetUpperBound(0); i++)
            //    {
            //        FunMerchant.FMagTagFuncEx[i] = -1;
            //    }
            //    for (int i = FunMerchant.FMagMonFunc.GetLowerBound(0); i <= FunMerchant.FMagMonFunc.GetUpperBound(0); i++)
            //    {
            //        FunMerchant.FMagMonFunc[i] = -1;
            //    }
            //}
            int nIdx;
            // for (var i = 0; i < PlayDiceList.Count; i++)
            // {
            //     QuestActionInfo = PlayDiceList[i];
            //     nIdx = ScriptNameList.IndexOf(FormatLabelStr(QuestActionInfo.sParam2, ref boChange));
            //     QuestActionInfo.sParam2 = "@" + nIdx;
            // }
            // for (var i = 0; i < GotoList.Count; i++)
            // {
            //     QuestActionInfo = GotoList[i];
            //     nIdx = ScriptNameList.IndexOf(FormatLabelStr(QuestActionInfo.sParam1, ref boChange));
            //     QuestActionInfo.nParam1 = nIdx;
            // }
            // for (var i = 0; i < DelayGotoList.Count; i++)
            // {
            //     QuestActionInfo = DelayGotoList[i];
            //     nIdx = ScriptNameList.IndexOf(FormatLabelStr(QuestActionInfo.sParam2, ref boChange));
            //     QuestActionInfo.nParam2 = nIdx;
            // }
            for (var i = 0; i < NPC.m_ScriptList.Count; i++)
            {
                var RecordList = NPC.m_ScriptList[i];
                nIdx = 0;
                foreach (var SayingRecord in RecordList.RecordList.Values)
                {
                    // for (var k = 0; k < SayingRecord.ProcedureList.Count; k++)
                    // {
                    //     var SayingProcedure = SayingRecord.ProcedureList[k];
                    //     if (!string.IsNullOrEmpty(SayingProcedure.sSayMsg))
                    //     {
                    //         SayingProcedure.sSayMsg = InitializeSayMsg(SayingProcedure.sSayMsg, SayingProcedure.SayNewLabelList, SayingProcedure.SayOldLabelList, ScriptNameList);
                    //     }
                    //     if (!string.IsNullOrEmpty(SayingProcedure.sElseSayMsg))
                    //     {
                    //         SayingProcedure.sElseSayMsg = InitializeSayMsg(SayingProcedure.sElseSayMsg, SayingProcedure.ElseSayNewLabelList, SayingProcedure.ElseSayOldLabelList, ScriptNameList);
                    //     }
                    // }
                    InitializeAppendLabel(NPC, SayingRecord.sLabel, nIdx);
                    nIdx++;
                }
            }
        }

        /// <summary>
        /// 初始化脚本标签
        /// </summary>
        private void InitializeAppendLabel(NormNpc NPC, string sLabel, int nIdx)
        {
            switch (sLabel)
            {
                case ScriptDef.SPLAYOFFLINE:
                    NPC.FGotoLable[ScriptDef.NPLAYOFFLINE] = nIdx;
                    break;
                case ScriptDef.SMARRYERROR:
                    NPC.FGotoLable[ScriptDef.NMARRYERROR] = nIdx;
                    break;
                case ScriptDef.SMASTERERROR:
                    NPC.FGotoLable[ScriptDef.NMASTERERROR] = nIdx;
                    break;
                case ScriptDef.SMARRYCHECKDIR:
                    NPC.FGotoLable[ScriptDef.NMARRYCHECKDIR] = nIdx;
                    break;
                case ScriptDef.SHUMANTYPEERR:
                    NPC.FGotoLable[ScriptDef.NHUMANTYPEERR] = nIdx;
                    break;
                case ScriptDef.SSTARTMARRY:
                    NPC.FGotoLable[ScriptDef.NSTARTMARRY] = nIdx;
                    break;
                case ScriptDef.SMARRYSEXERR:
                    NPC.FGotoLable[ScriptDef.NMARRYSEXERR] = nIdx;
                    break;
                case ScriptDef.SMARRYDIRERR:
                    NPC.FGotoLable[ScriptDef.NMARRYDIRERR] = nIdx;
                    break;
                case ScriptDef.SWATEMARRY:
                    NPC.FGotoLable[ScriptDef.NWATEMARRY] = nIdx;
                    break;
                case ScriptDef.SREVMARRY:
                    NPC.FGotoLable[ScriptDef.NREVMARRY] = nIdx;
                    break;
                case ScriptDef.SENDMARRY:
                    NPC.FGotoLable[ScriptDef.NENDMARRY] = nIdx;
                    break;
                case ScriptDef.SENDMARRYFAIL:
                    NPC.FGotoLable[ScriptDef.NENDMARRYFAIL] = nIdx;
                    break;
                case ScriptDef.SMASTERCHECKDIR:
                    NPC.FGotoLable[ScriptDef.NMASTERCHECKDIR] = nIdx;
                    break;
                case ScriptDef.SSTARTGETMASTER:
                    NPC.FGotoLable[ScriptDef.NSTARTGETMASTER] = nIdx;
                    break;
                case ScriptDef.SMASTERDIRERR:
                    NPC.FGotoLable[ScriptDef.NMASTERDIRERR] = nIdx;
                    break;
                case ScriptDef.SWATEMASTER:
                    NPC.FGotoLable[ScriptDef.NWATEMASTER] = nIdx;
                    break;
                case ScriptDef.SREVMASTER:
                    NPC.FGotoLable[ScriptDef.NREVMASTER] = nIdx;
                    break;
                case ScriptDef.SENDMASTER:
                    NPC.FGotoLable[ScriptDef.NENDMASTER] = nIdx;
                    break;
                case ScriptDef.SSTARTMASTER:
                    NPC.FGotoLable[ScriptDef.NSTARTMASTER] = nIdx;
                    break;
                case ScriptDef.SENDMASTERFAIL:
                    NPC.FGotoLable[ScriptDef.NENDMASTERFAIL] = nIdx;
                    break;
                case ScriptDef.SEXEMARRYFAIL:
                    NPC.FGotoLable[ScriptDef.NEXEMARRYFAIL] = nIdx;
                    break;
                case ScriptDef.SUNMARRYCHECKDIR:
                    NPC.FGotoLable[ScriptDef.NUNMARRYCHECKDIR] = nIdx;
                    break;
                case ScriptDef.SUNMARRYTYPEERR:
                    NPC.FGotoLable[ScriptDef.NUNMARRYTYPEERR] = nIdx;
                    break;
                case ScriptDef.SSTARTUNMARRY:
                    NPC.FGotoLable[ScriptDef.NSTARTUNMARRY] = nIdx;
                    break;
                case ScriptDef.SUNMARRYEND:
                    NPC.FGotoLable[ScriptDef.NUNMARRYEND] = nIdx;
                    break;
                case ScriptDef.SWATEUNMARRY:
                    NPC.FGotoLable[ScriptDef.NWATEUNMARRY] = nIdx;
                    break;
                case ScriptDef.SEXEMASTERFAIL:
                    NPC.FGotoLable[ScriptDef.NEXEMASTERFAIL] = nIdx;
                    break;
                case ScriptDef.SUNMASTERCHECKDIR:
                    NPC.FGotoLable[ScriptDef.NUNMASTERCHECKDIR] = nIdx;
                    break;
                case ScriptDef.SUNMASTERTYPEERR:
                    NPC.FGotoLable[ScriptDef.NUNMASTERTYPEERR] = nIdx;
                    break;
                case ScriptDef.SUNISMASTER:
                    NPC.FGotoLable[ScriptDef.NUNISMASTER] = nIdx;
                    break;
                case ScriptDef.SUNMASTERERROR:
                    NPC.FGotoLable[ScriptDef.NUNMASTERERROR] = nIdx;
                    break;
                case ScriptDef.SSTARTUNMASTER:
                    NPC.FGotoLable[ScriptDef.NSTARTUNMASTER] = nIdx;
                    break;
                case ScriptDef.SWATEUNMASTER:
                    NPC.FGotoLable[ScriptDef.NWATEUNMASTER] = nIdx;
                    break;
                case ScriptDef.SUNMASTEREND:
                    NPC.FGotoLable[ScriptDef.NUNMASTEREND] = nIdx;
                    break;
                case ScriptDef.SREVUNMASTER:
                    NPC.FGotoLable[ScriptDef.NREVUNMASTER] = nIdx;
                    break;
                case ScriptDef.SSUPREQUEST_OK:
                    NPC.FGotoLable[ScriptDef.NSUPREQUEST_OK] = nIdx;
                    break;
                case ScriptDef.SMEMBER:
                    NPC.FGotoLable[ScriptDef.NMEMBER] = nIdx;
                    break;
                case ScriptDef.SPLAYRECONNECTION:
                    NPC.FGotoLable[ScriptDef.NPLAYRECONNECTION] = nIdx;
                    break;
                case ScriptDef.SLOGIN:
                    NPC.FGotoLable[ScriptDef.NLOGIN] = nIdx;
                    break;
                case ScriptDef.SPLAYDIE:
                    NPC.FGotoLable[ScriptDef.NPLAYDIE] = nIdx;
                    break;
                case ScriptDef.SKILLPLAY:
                    NPC.FGotoLable[ScriptDef.NKILLPLAY] = nIdx;
                    break;
                case ScriptDef.SPLAYLEVELUP:
                    NPC.FGotoLable[ScriptDef.NPLAYLEVELUP] = nIdx;
                    break;
                case ScriptDef.SKILLMONSTER:
                    NPC.FGotoLable[ScriptDef.NKILLMONSTER] = nIdx;
                    break;
                case ScriptDef.SCREATEECTYPE_IN:
                    NPC.FGotoLable[ScriptDef.NCREATEECTYPE_IN] = nIdx;
                    break;
                case ScriptDef.SCREATEECTYPE_OK:
                    NPC.FGotoLable[ScriptDef.NCREATEECTYPE_OK] = nIdx;
                    break;
                case ScriptDef.SCREATEECTYPE_FAIL:
                    NPC.FGotoLable[ScriptDef.NCREATEECTYPE_FAIL] = nIdx;
                    break;
                case ScriptDef.SRESUME:
                    NPC.FGotoLable[ScriptDef.NRESUME] = nIdx;
                    break;
                case ScriptDef.SGETLARGESSGOLD_OK:
                    NPC.FGotoLable[ScriptDef.NGETLARGESSGOLD_OK] = nIdx;
                    break;
                case ScriptDef.SGETLARGESSGOLD_FAIL:
                    NPC.FGotoLable[ScriptDef.NGETLARGESSGOLD_FAIL] = nIdx;
                    break;
                case ScriptDef.SGETLARGESSGOLD_ERROR:
                    NPC.FGotoLable[ScriptDef.NGETLARGESSGOLD_ERROR] = nIdx;
                    break;
                case ScriptDef.SMASTERISPRENTICE:
                    NPC.FGotoLable[ScriptDef.NMASTERISPRENTICE] = nIdx;
                    break;
                case ScriptDef.SMASTERISFULL:
                    NPC.FGotoLable[ScriptDef.NMASTERISFULL] = nIdx;
                    break;
                case ScriptDef.SGROUPCREATE:
                    NPC.FGotoLable[ScriptDef.NGROUPCREATE] = nIdx;
                    break;
                case ScriptDef.SSTARTGROUP:
                    NPC.FGotoLable[ScriptDef.NSTARTGROUP] = nIdx;
                    break;
                case ScriptDef.SJOINGROUP:
                    NPC.FGotoLable[ScriptDef.NJOINGROUP] = nIdx;
                    break;
                case ScriptDef.SSPEEDCLOSE:
                    NPC.FGotoLable[ScriptDef.NSPEEDCLOSE] = nIdx;
                    break;
                case ScriptDef.SUPGRADENOW_OK:
                    NPC.FGotoLable[ScriptDef.NUPGRADENOW_OK] = nIdx;
                    break;
                case ScriptDef.SUPGRADENOW_ING:
                    NPC.FGotoLable[ScriptDef.NUPGRADENOW_ING] = nIdx;
                    break;
                case ScriptDef.SUPGRADENOW_FAIL:
                    NPC.FGotoLable[ScriptDef.NUPGRADENOW_FAIL] = nIdx;
                    break;
                case ScriptDef.SGETBACKUPGNOW_OK:
                    NPC.FGotoLable[ScriptDef.NGETBACKUPGNOW_OK] = nIdx;
                    break;
                case ScriptDef.SGETBACKUPGNOW_ING:
                    NPC.FGotoLable[ScriptDef.NGETBACKUPGNOW_ING] = nIdx;
                    break;
                case ScriptDef.SGETBACKUPGNOW_FAIL:
                    NPC.FGotoLable[ScriptDef.NGETBACKUPGNOW_FAIL] = nIdx;
                    break;
                case ScriptDef.SGETBACKUPGNOW_BAGFULL:
                    NPC.FGotoLable[ScriptDef.NGETBACKUPGNOW_BAGFULL] = nIdx;
                    break;
                case ScriptDef.STAKEONITEMS:
                    NPC.FGotoLable[ScriptDef.NTAKEONITEMS] = nIdx;
                    break;
                case ScriptDef.STAKEOFFITEMS:
                    NPC.FGotoLable[ScriptDef.NTAKEOFFITEMS] = nIdx;
                    break;
                case ScriptDef.SPLAYREVIVE:
                    NPC.FGotoLable[ScriptDef.NPLAYREVIVE] = nIdx;
                    break;
                case ScriptDef.SMOVEABILITY_OK:
                    NPC.FGotoLable[ScriptDef.NMOVEABILITY_OK] = nIdx;
                    break;
                case ScriptDef.SMOVEABILITY_FAIL:
                    NPC.FGotoLable[ScriptDef.NMOVEABILITY_FAIL] = nIdx;
                    break;
                case ScriptDef.SASSEMBLEALL:
                    NPC.FGotoLable[ScriptDef.NASSEMBLEALL] = nIdx;
                    break;
                case ScriptDef.SASSEMBLEWEAPON:
                    NPC.FGotoLable[ScriptDef.NASSEMBLEWEAPON] = nIdx;
                    break;
                case ScriptDef.SASSEMBLEDRESS:
                    NPC.FGotoLable[ScriptDef.NASSEMBLEDRESS] = nIdx;
                    break;
                case ScriptDef.SASSEMBLEHELMET:
                    NPC.FGotoLable[ScriptDef.NASSEMBLEHELMET] = nIdx;
                    break;
                case ScriptDef.SASSEMBLENECKLACE:
                    NPC.FGotoLable[ScriptDef.NASSEMBLENECKLACE] = nIdx;
                    break;
                case ScriptDef.SASSEMBLERING:
                    NPC.FGotoLable[ScriptDef.NASSEMBLERING] = nIdx;
                    break;
                case ScriptDef.SASSEMBLEARMRING:
                    NPC.FGotoLable[ScriptDef.NASSEMBLEARMRING] = nIdx;
                    break;
                case ScriptDef.SASSEMBLEBELT:
                    NPC.FGotoLable[ScriptDef.NASSEMBLEBELT] = nIdx;
                    break;
                case ScriptDef.SASSEMBLEBOOT:
                    NPC.FGotoLable[ScriptDef.NASSEMBLEBOOT] = nIdx;
                    break;
                case ScriptDef.SASSEMBLEFAIL:
                    NPC.FGotoLable[ScriptDef.NASSEMBLEFAIL] = nIdx;
                    break;
                case ScriptDef.SCREATEHEROFAILEX:
                    NPC.FGotoLable[ScriptDef.NCREATEHEROFAILEX] = nIdx;// 创建英雄失败  By John 2012.08.04
                    break;
                case ScriptDef.SLOGOUTHEROFIRST:
                    NPC.FGotoLable[ScriptDef.NLOGOUTHEROFIRST] = nIdx;// 请将英雄设置下线  By John 2012.08.04
                    break;
                case ScriptDef.SNOTHAVEHERO:
                    NPC.FGotoLable[ScriptDef.NNOTHAVEHERO] = nIdx;// 没有英雄   By John 2012.08.04
                    break;
                case ScriptDef.SHERONAMEFILTER:
                    NPC.FGotoLable[ScriptDef.NHERONAMEFILTER] = nIdx;// 英雄名字中包含禁用字符   By John 2012.08.04
                    break;
                case ScriptDef.SHAVEHERO:
                    NPC.FGotoLable[ScriptDef.NHAVEHERO] = nIdx;// 有英雄    By John 2012.08.04
                    break;
                case ScriptDef.SCREATEHEROOK:
                    NPC.FGotoLable[ScriptDef.NCREATEHEROOK] = nIdx;// 创建英雄OK   By John 2012.08.04
                    break;
                case ScriptDef.SHERONAMEEXISTS:
                    NPC.FGotoLable[ScriptDef.NHERONAMEEXISTS] = nIdx;// 英雄名字已经存在  By John 2012.08.04
                    break;
                case ScriptDef.SDELETEHEROOK:
                    NPC.FGotoLable[ScriptDef.NDELETEHEROOK] = nIdx;// 删除英雄成功    By John 2012.08.04
                    break;
                case ScriptDef.SDELETEHEROFAIL:
                    NPC.FGotoLable[ScriptDef.NDELETEHEROFAIL] = nIdx;// 删除英雄失败    By John 2012.08.04
                    break;
                case ScriptDef.SHEROOVERCHRCOUNT:
                    NPC.FGotoLable[ScriptDef.NHEROOVERCHRCOUNT] = nIdx;// 你的帐号角色过多   By John 2012.08.04
                    break;
                default:
                    //if (NPC.m_btNPCRaceServer == DataConst.NPC_RC_FUNMERCHANT)
                    //{
                    //    TFunMerchant FunMerchant = (TFunMerchant)NPC;
                    //    if (HUtil32.CompareLStr(sLabel, SctiptDef.SSTDMODEFUNC, SctiptDef.SSTDMODEFUNC.Length))
                    //    {
                    //        sIdx = sLabel.Substring(SctiptDef.SSTDMODEFUNC.Length + 1 - 1, SctiptDef.SSTDMODEFUNC.Length);
                    //        nIndex = Convert.ToInt32(sIdx);
                    //        if (nIndex >= FunMerchant.FStdModeFunc.GetLowerBound(0) && nIndex <= FunMerchant.FStdModeFunc.GetUpperBound(0))
                    //        {
                    //            FunMerchant.FStdModeFunc[nIndex] = nIdx;
                    //        }
                    //    }
                    //    else if (HUtil32.CompareLStr(sLabel, SctiptDef.SPLAYLEVELUPEX, SctiptDef.SPLAYLEVELUPEX.Length))
                    //    {
                    //        sIdx = sLabel.Substring(SctiptDef.SPLAYLEVELUPEX.Length + 1 - 1, SctiptDef.SPLAYLEVELUPEX.Length);
                    //        nIndex = Convert.ToInt32(sIdx);
                    //        if (nIndex >= FunMerchant.FPlayLevelUp.GetLowerBound(0) && nIndex <= FunMerchant.FPlayLevelUp.GetUpperBound(0))
                    //        {
                    //            FunMerchant.FPlayLevelUp[nIndex] = nIdx;
                    //        }
                    //    }
                    //    else if (HUtil32.CompareLStr(sLabel, SctiptDef.SUSERCMD, SctiptDef.SUSERCMD.Length))
                    //    {
                    //        sIdx = sLabel.Substring(SctiptDef.SUSERCMD.Length + 1 - 1, SctiptDef.SUSERCMD.Length);
                    //        nIndex = Convert.ToInt32(sIdx);
                    //        if (nIndex >= FunMerchant.FUserCmd.GetLowerBound(0) && nIndex <= FunMerchant.FUserCmd.GetUpperBound(0))
                    //        {
                    //            FunMerchant.FUserCmd[nIndex] = nIdx;
                    //        }
                    //    }
                    //    else if (HUtil32.CompareLStr(sLabel, SctiptDef.SCLEARMISSION, SctiptDef.SCLEARMISSION.Length))
                    //    {
                    //        sIdx = sLabel.Substring(SctiptDef.SCLEARMISSION.Length + 1 - 1, SctiptDef.SCLEARMISSION.Length);
                    //        nIndex = Convert.ToInt32(sIdx);
                    //        if (nIndex >= FunMerchant.FClearMission.GetLowerBound(0) && nIndex <= FunMerchant.FClearMission.GetUpperBound(0))
                    //        {
                    //            FunMerchant.FClearMission[nIndex] = nIdx;
                    //        }
                    //    }
                    //    else if (HUtil32.CompareLStr(sLabel, SctiptDef.SMAGSELFFUNC, SctiptDef.SMAGSELFFUNC.Length))
                    //    {
                    //        sIdx = sLabel.Substring(SctiptDef.SMAGSELFFUNC.Length + 1 - 1, SctiptDef.SMAGSELFFUNC.Length);
                    //        nIndex = Convert.ToInt32(sIdx);
                    //        if (nIndex >= FunMerchant.FMagSelfFunc.GetLowerBound(0) && nIndex <= FunMerchant.FMagSelfFunc.GetUpperBound(0))
                    //        {
                    //            FunMerchant.FMagSelfFunc[nIndex] = nIdx;
                    //        }
                    //    }
                    //    else if (HUtil32.CompareLStr(sLabel, SctiptDef.SMAGTAGFUNC, SctiptDef.SMAGTAGFUNC.Length))
                    //    {
                    //        sIdx = sLabel.Substring(SctiptDef.SMAGTAGFUNC.Length + 1 - 1, SctiptDef.SMAGTAGFUNC.Length);
                    //        nIndex = Convert.ToInt32(sIdx);
                    //        if (nIndex >= FunMerchant.FMagTagFunc.GetLowerBound(0) && nIndex <= FunMerchant.FMagTagFunc.GetUpperBound(0))
                    //        {
                    //            FunMerchant.FMagTagFunc[nIndex] = nIdx;
                    //        }
                    //    }
                    //    else if (HUtil32.CompareLStr(sLabel, SctiptDef.SMAGTAGFUNCEX, SctiptDef.SMAGTAGFUNCEX.Length))
                    //    {
                    //        sIdx = sLabel.Substring(SctiptDef.SMAGTAGFUNCEX.Length + 1 - 1, SctiptDef.SMAGTAGFUNCEX.Length);
                    //        nIndex = Convert.ToInt32(sIdx);
                    //        if (nIndex >= FunMerchant.FMagTagFuncEx.GetLowerBound(0) && nIndex <= FunMerchant.FMagTagFuncEx.GetUpperBound(0))
                    //        {
                    //            FunMerchant.FMagTagFuncEx[nIndex] = nIdx;
                    //        }
                    //    }
                    //    else if (HUtil32.CompareLStr(sLabel, SctiptDef.SMAGMONFUNC, SctiptDef.SMAGMONFUNC.Length))
                    //    {
                    //        sIdx = sLabel.Substring(SctiptDef.SMAGMONFUNC.Length + 1 - 1, SctiptDef.SMAGMONFUNC.Length);
                    //        nIndex = Convert.ToInt32(sIdx);
                    //        if (nIndex >= FunMerchant.FMagMonFunc.GetLowerBound(0) && nIndex <= FunMerchant.FMagMonFunc.GetUpperBound(0))
                    //        {
                    //            FunMerchant.FMagMonFunc[nIndex] = nIdx;
                    //        }
                    //    }
                    //}
                    break;
            }
        }

        /// <summary>
        /// 格式化标签
        /// </summary>
        /// <param name="sLabel"></param>
        /// <param name="boChange"></param>
        /// <returns></returns>
        private string FormatLabelStr(string sLabel, ref bool boChange)
        {
            var result = sLabel;
            if (sLabel.IndexOf(")", StringComparison.OrdinalIgnoreCase) > -1)
            {
                HUtil32.GetValidStr3(sLabel, ref result, new[] { "(" });
                boChange = true;
            }
            return result;
        }

        /// <summary>
        /// 初始化脚本处理方法
        /// </summary>
        /// <returns></returns>
        internal string InitializeProcedure(string sMsg)
        {
            var nC = 0;
            var sCmd = string.Empty;
            var tempstr = sMsg;
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
                if (s10 != "")
                {
                    if (s10.IndexOf("/") > 0)
                    {
                        sLabel = HUtil32.GetValidStr3(s10, ref sname, '/');
                        if (string.Compare(sLabel, "@close", true) == 0)
                        {
                            continue;
                        }
                        if (string.Compare(sLabel, "@Exit", true) == 0)
                        {
                            continue;
                        }
                        if (string.Compare(sLabel, "@main", true) == 0)
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
                                sMsg = ScriptDef.RESETLABEL + sMsg;
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
        private void InitializeVariable(string sLabel, ref string sMsg)
        {
            var s14 = string.Empty;
            var sLabel2 = sLabel.ToUpper();
            if (sLabel2 == ScriptDef.sVAR_SERVERNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_SERVERNAME);
            }
            else if (sLabel2 == ScriptDef.sVAR_SERVERIP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_SERVERIP);
            }
            else if (sLabel2 == ScriptDef.sVAR_WEBSITE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_WEBSITE);
            }
            else if (sLabel2 == ScriptDef.sVAR_BBSSITE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_BBSSITE);
            }
            else if (sLabel2 == ScriptDef.sVAR_CLIENTDOWNLOAD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CLIENTDOWNLOAD);
            }
            else if (sLabel2 == ScriptDef.sVAR_QQ)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_QQ);
            }
            else if (sLabel2 == ScriptDef.sVAR_PHONE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_PHONE);
            }
            else if (sLabel2 == ScriptDef.sVAR_BANKACCOUNT0)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_BANKACCOUNT0);
            }
            else if (sLabel2 == ScriptDef.sVAR_BANKACCOUNT1)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_BANKACCOUNT1);
            }
            else if (sLabel2 == ScriptDef.sVAR_BANKACCOUNT2)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_BANKACCOUNT2);
            }
            else if (sLabel2 == ScriptDef.sVAR_BANKACCOUNT3)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_BANKACCOUNT3);
            }
            else if (sLabel2 == ScriptDef.sVAR_BANKACCOUNT4)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_BANKACCOUNT4);
            }
            else if (sLabel2 == ScriptDef.sVAR_BANKACCOUNT5)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_BANKACCOUNT5);
            }
            else if (sLabel2 == ScriptDef.sVAR_BANKACCOUNT6)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_BANKACCOUNT6);
            }
            else if (sLabel2 == ScriptDef.sVAR_BANKACCOUNT7)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_BANKACCOUNT7);
            }
            else if (sLabel2 == ScriptDef.sVAR_BANKACCOUNT8)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_BANKACCOUNT8);
            }
            else if (sLabel2 == ScriptDef.sVAR_BANKACCOUNT9)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_BANKACCOUNT9);
            }
            else if (sLabel2 == ScriptDef.sVAR_GAMEGOLDNAME)
            {
                //sMsg = sMsg.Replace("<" + sLabel + ">", Grobal2.sSTRING_GAMEGOLD);
            }
            else if (sLabel2 == ScriptDef.sVAR_GAMEPOINTNAME)
            {
                // sMsg = sMsg.Replace("<" + sLabel + ">", Grobal2.sSTRING_GAMEPOINT);
            }
            else if (sLabel2 == ScriptDef.sVAR_USERCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_USERCOUNT);
            }
            else if (sLabel2 == ScriptDef.sVAR_DATETIME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_DATETIME);
            }
            else if (sLabel2 == ScriptDef.sVAR_USERNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_USERNAME);
            }
            else if (sLabel2 == ScriptDef.sVAR_FBMAPNAME)
            { //副本
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_FBMAPNAME);
            }
            else if (sLabel2 == ScriptDef.sVAR_FBMAP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_FBMAP);
            }
            else if (sLabel2 == ScriptDef.sVAR_ACCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_ACCOUNT);
            }
            else if (sLabel2 == ScriptDef.sVAR_ASSEMBLEITEMNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_ASSEMBLEITEMNAME);
            }
            else if (sLabel2 == ScriptDef.sVAR_MAPNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_MAPNAME);
            }
            else if (sLabel2 == ScriptDef.sVAR_GUILDNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_GUILDNAME);
            }
            else if (sLabel2 == ScriptDef.sVAR_RANKNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_RANKNAME);
            }
            else if (sLabel2 == ScriptDef.sVAR_LEVEL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_LEVEL);
            }
            else if (sLabel2 == ScriptDef.sVAR_HP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_HP);
            }
            else if (sLabel2 == ScriptDef.sVAR_MAXHP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_MAXHP);
            }
            else if (sLabel2 == ScriptDef.sVAR_MP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_MP);
            }
            else if (sLabel2 == ScriptDef.sVAR_MAXMP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_MAXMP);
            }
            else if (sLabel2 == ScriptDef.sVAR_AC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_AC);
            }
            else if (sLabel2 == ScriptDef.sVAR_MAXAC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_MAXAC);
            }
            else if (sLabel2 == ScriptDef.sVAR_MAC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_MAC);
            }
            else if (sLabel2 == ScriptDef.sVAR_MAXMAC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_MAXMAC);
            }
            else if (sLabel2 == ScriptDef.sVAR_DC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_DC);
            }
            else if (sLabel2 == ScriptDef.sVAR_MAXDC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_MAXDC);
            }
            else if (sLabel2 == ScriptDef.sVAR_MC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_MC);
            }
            else if (sLabel2 == ScriptDef.sVAR_MAXMC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_MAXMC);
            }
            else if (sLabel2 == ScriptDef.sVAR_SC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_SC);
            }
            else if (sLabel2 == ScriptDef.sVAR_MAXSC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_MAXSC);
            }
            else if (sLabel2 == ScriptDef.sVAR_EXP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_EXP);
            }
            else if (sLabel2 == ScriptDef.sVAR_MAXEXP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_MAXEXP);
            }
            else if (sLabel2 == ScriptDef.sVAR_PKPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_PKPOINT);
            }
            else if (sLabel2 == ScriptDef.sVAR_CREDITPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CREDITPOINT);
            }
            else if (sLabel2 == ScriptDef.sVAR_GOLDCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_GOLDCOUNT);
            }
            else if (sLabel2 == ScriptDef.sVAR_GAMEGOLD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_GAMEGOLD);
            }
            else if (sLabel2 == ScriptDef.sVAR_GAMEPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_GAMEPOINT);
            }
            else if (sLabel2 == ScriptDef.sVAR_LOGINTIME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_LOGINTIME);
            }
            else if (sLabel2 == ScriptDef.sVAR_LOGINLONG)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_LOGINLONG);
            }
            else if (sLabel2 == ScriptDef.sVAR_DRESS)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_DRESS);
            }
            else if (sLabel2 == ScriptDef.sVAR_WEAPON)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_WEAPON);
            }
            else if (sLabel2 == ScriptDef.sVAR_RIGHTHAND)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_RIGHTHAND);
            }
            else if (sLabel2 == ScriptDef.sVAR_HELMET)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_HELMET);
            }
            else if (sLabel2 == ScriptDef.sVAR_NECKLACE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_NECKLACE);
            }
            else if (sLabel2 == ScriptDef.sVAR_RING_R)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_RING_R);
            }
            else if (sLabel2 == ScriptDef.sVAR_RING_L)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_RING_L);
            }
            else if (sLabel2 == ScriptDef.sVAR_ARMRING_R)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_ARMRING_R);
            }
            else if (sLabel2 == ScriptDef.sVAR_ARMRING_L)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_ARMRING_L);
            }
            else if (sLabel2 == ScriptDef.sVAR_BUJUK)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_BUJUK);
            }
            else if (sLabel2 == ScriptDef.sVAR_BELT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_BELT);
            }
            else if (sLabel2 == ScriptDef.sVAR_BOOTS)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_BOOTS);
            }
            else if (sLabel2 == ScriptDef.sVAR_CHARM)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CHARM);
            }
            //=======================================没有用到的==============================
            else if (sLabel2 == ScriptDef.sVAR_HOUSE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_HOUSE);
            }
            else if (sLabel2 == ScriptDef.sVAR_CIMELIA)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CIMELIA);
            }
            //=======================================没有用到的==============================
            else if (sLabel2 == ScriptDef.sVAR_IPADDR)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_IPADDR);
            }
            else if (sLabel2 == ScriptDef.sVAR_IPLOCAL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_IPLOCAL);
            }
            else if (sLabel2 == ScriptDef.sVAR_GUILDBUILDPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_GUILDBUILDPOINT);
            }
            else if (sLabel2 == ScriptDef.sVAR_GUILDAURAEPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_GUILDAURAEPOINT);
            }
            else if (sLabel2 == ScriptDef.sVAR_GUILDSTABILITYPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_GUILDSTABILITYPOINT);
            }
            else if (sLabel2 == ScriptDef.sVAR_GUILDFLOURISHPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_GUILDFLOURISHPOINT);
            }
            //=================================没用用到的====================================
            else if (sLabel2 == ScriptDef.sVAR_GUILDMONEYCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_GUILDMONEYCOUNT);
            }
            //=================================没用用到的结束====================================
            else if (sLabel2 == ScriptDef.sVAR_REQUESTCASTLEWARITEM)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_REQUESTCASTLEWARITEM);
            }
            else if (sLabel2 == ScriptDef.sVAR_REQUESTCASTLEWARDAY)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_REQUESTCASTLEWARDAY);
            }
            else if (sLabel2 == ScriptDef.sVAR_REQUESTBUILDGUILDITEM)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_REQUESTBUILDGUILDITEM);
            }
            else if (sLabel2 == ScriptDef.sVAR_OWNERGUILD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_OWNERGUILD);
            }
            else if (sLabel2 == ScriptDef.sVAR_CASTLENAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CASTLENAME);
            }
            else if (sLabel2 == ScriptDef.sVAR_LORD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_LORD);
            }
            else if (sLabel2 == ScriptDef.sVAR_GUILDWARFEE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_GUILDWARFEE);
            }
            else if (sLabel2 == ScriptDef.sVAR_BUILDGUILDFEE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_BUILDGUILDFEE);
            }
            else if (sLabel2 == ScriptDef.sVAR_CASTLEWARDATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CASTLEWARDATE);
            }
            else if (sLabel2 == ScriptDef.sVAR_LISTOFWAR)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_LISTOFWAR);
            }
            else if (sLabel2 == ScriptDef.sVAR_CASTLECHANGEDATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CASTLECHANGEDATE);
            }
            else if (sLabel2 == ScriptDef.sVAR_CASTLEWARLASTDATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CASTLEWARLASTDATE);
            }
            else if (sLabel2 == ScriptDef.sVAR_CASTLEGETDAYS)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CASTLEGETDAYS);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_DATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_DATE);
            }
            //===================================没用用到的======================================
            else if (sLabel2 == ScriptDef.sVAR_CMD_PRVMSG)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_PRVMSG);
            }
            //===================================没用用到的结束======================================
            else if (sLabel2 == ScriptDef.sVAR_CMD_ALLOWMSG)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_ALLOWMSG);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_LETSHOUT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_LETSHOUT);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_LETTRADE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_LETTRADE);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_LETGuild)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_LETGuild);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_ENDGUILD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_ENDGUILD);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_BANGUILDCHAT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_BANGUILDCHAT);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_AUTHALLY)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_AUTHALLY);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_AUTH)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_AUTH);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_AUTHCANCEL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_AUTHCANCEL);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_USERMOVE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_USERMOVE);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_SEARCHING)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_SEARCHING);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_ALLOWGROUPCALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_ALLOWGROUPCALL);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_GROUPRECALLL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_GROUPRECALLL);
            }
            #region 没有使用的
            //===========================================没有使用的========================================
            else if (sLabel2 == ScriptDef.sVAR_CMD_ALLOWGUILDRECALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_ALLOWGUILDRECALL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLOWGUILDRECALL, SctiptDef.sVAR_CMD_ALLOWGUILDRECALL);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_GUILDRECALLL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_GUILDRECALLL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_GUILDRECALLL, SctiptDef.sVAR_CMD_GUILDRECALLL);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_DEAR)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_DEAR);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_DEAR, SctiptDef.sVAR_CMD_DEAR);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_ALLOWDEARRCALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_ALLOWDEARRCALL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLOWDEARRCALL, SctiptDef.sVAR_CMD_ALLOWDEARRCALL);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_DEARRECALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_DEARRECALL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_DEARRECALL, SctiptDef.sVAR_CMD_DEARRECALL);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_MASTER)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_MASTER);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_MASTER, SctiptDef.sVAR_CMD_MASTER);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_ALLOWMASTERRECALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_ALLOWMASTERRECALL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLOWMASTERRECALL, SctiptDef.sVAR_CMD_ALLOWMASTERRECALL);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_MASTERECALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_MASTERECALL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_MASTERECALL, SctiptDef.sVAR_CMD_MASTERECALL);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_TAKEONHORSE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_TAKEONHORSE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_TAKEONHORSE, SctiptDef.sVAR_CMD_TAKEONHORSE);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_TAKEOFHORSE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_TAKEOFHORSE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_TAKEOFHORSE, SctiptDef.sVAR_CMD_TAKEOFHORSE);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_ALLSYSMSG)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_ALLSYSMSG);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLSYSMSG, SctiptDef.sVAR_CMD_ALLSYSMSG);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_MEMBERFUNCTION)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_MEMBERFUNCTION);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_MEMBERFUNCTION, SctiptDef.sVAR_CMD_MEMBERFUNCTION);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_MEMBERFUNCTIONEX)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_MEMBERFUNCTIONEX);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_MEMBERFUNCTIONEX, SctiptDef.sVAR_CMD_MEMBERFUNCTIONEX);
            }
            else if (sLabel2 == ScriptDef.sVAR_CASTLEGOLD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CASTLEGOLD);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CASTLEGOLD, SctiptDef.sVAR_CASTLEGOLD);
            }
            else if (sLabel2 == ScriptDef.sVAR_TODAYINCOME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_TODAYINCOME);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_TODAYINCOME, SctiptDef.sVAR_TODAYINCOME);
            }
            else if (sLabel2 == ScriptDef.sVAR_CASTLEDOORSTATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CASTLEDOORSTATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CASTLEDOORSTATE, SctiptDef.sVAR_CASTLEDOORSTATE);
            }
            else if (sLabel2 == ScriptDef.sVAR_REPAIRDOORGOLD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_REPAIRDOORGOLD);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_REPAIRDOORGOLD, SctiptDef.sVAR_REPAIRDOORGOLD);
            }
            else if (sLabel2 == ScriptDef.sVAR_REPAIRWALLGOLD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_REPAIRWALLGOLD);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_REPAIRWALLGOLD, SctiptDef.sVAR_REPAIRWALLGOLD);
            }
            else if (sLabel2 == ScriptDef.sVAR_GUARDFEE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_GUARDFEE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_GUARDFEE, SctiptDef.sVAR_GUARDFEE);
            }
            else if (sLabel2 == ScriptDef.sVAR_ARCHERFEE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_ARCHERFEE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_ARCHERFEE, SctiptDef.sVAR_ARCHERFEE);
            }
            else if (sLabel2 == ScriptDef.sVAR_GUARDRULE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_GUARDRULE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_GUARDRULE, SctiptDef.sVAR_GUARDRULE);
            }
            else if (sLabel2 == ScriptDef.sVAR_STORAGE2STATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_STORAGE2STATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_STORAGE2STATE, SctiptDef.sVAR_STORAGE2STATE);
            }
            else if (sLabel2 == ScriptDef.sVAR_STORAGE3STATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_STORAGE3STATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_STORAGE3STATE, SctiptDef.sVAR_STORAGE3STATE);
            }
            else if (sLabel2 == ScriptDef.sVAR_STORAGE4STATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_STORAGE4STATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_STORAGE4STATE, SctiptDef.sVAR_STORAGE4STATE);
            }
            else if (sLabel2 == ScriptDef.sVAR_STORAGE5STATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_STORAGE5STATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_STORAGE5STATE, SctiptDef.sVAR_STORAGE5STATE);
            }
            else if (sLabel2 == ScriptDef.sVAR_SELFNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_SELFNAME);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_SELFNAME, SctiptDef.sVAR_SELFNAME);
            }
            else if (sLabel2 == ScriptDef.sVAR_POSENAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_POSENAME);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_POSENAME, SctiptDef.sVAR_POSENAME);
            }
            else if (sLabel2 == ScriptDef.sVAR_GAMEDIAMOND)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_GAMEDIAMOND);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_GAMEDIAMOND, SctiptDef.sVAR_GAMEDIAMOND);
            }
            else if (sLabel2 == ScriptDef.sVAR_GAMEGIRD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_GAMEGIRD);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_GAMEGIRD, SctiptDef.sVAR_GAMEGIRD);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_ALLOWFIREND)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_ALLOWFIREND);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLOWFIREND, SctiptDef.sVAR_CMD_ALLOWFIREND);
            }
            else if (sLabel2 == ScriptDef.sVAR_EFFIGYSTATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_EFFIGYSTATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_EFFIGYSTATE, SctiptDef.sVAR_EFFIGYSTATE);
            }
            else if (sLabel2 == ScriptDef.sVAR_EFFIGYOFFSET)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_EFFIGYOFFSET);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_EFFIGYOFFSET, SctiptDef.sVAR_EFFIGYOFFSET);
            }
            else if (sLabel2 == ScriptDef.sVAR_YEAR)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_YEAR);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_YEAR, SctiptDef.sVAR_YEAR);
            }
            else if (sLabel2 == ScriptDef.sVAR_MONTH)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_MONTH);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_MONTH, SctiptDef.sVAR_MONTH);
            }
            else if (sLabel2 == ScriptDef.sVAR_DAY)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_DAY);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_DAY, SctiptDef.sVAR_DAY);
            }
            else if (sLabel2 == ScriptDef.sVAR_HOUR)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_HOUR);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_HOUR, SctiptDef.sVAR_HOUR);
            }
            else if (sLabel2 == ScriptDef.sVAR_MINUTE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_MINUTE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_MINUTE, SctiptDef.sVAR_MINUTE);
            }
            else if (sLabel2 == ScriptDef.sVAR_SEC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_SEC);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_SEC, SctiptDef.sVAR_SEC);
            }
            else if (sLabel2 == ScriptDef.sVAR_MAP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_MAP);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_MAP, SctiptDef.sVAR_MAP);
            }
            else if (sLabel2 == ScriptDef.sVAR_X)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_X);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_X, SctiptDef.sVAR_X);
            }
            else if (sLabel2 == ScriptDef.sVAR_Y)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_Y);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_Y, SctiptDef.sVAR_Y);
            }
            else if (sLabel2 == ScriptDef.sVAR_UNMASTER_FORCE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_UNMASTER_FORCE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_UNMASTER_FORCE, SctiptDef.sVAR_UNMASTER_FORCE);
            }
            else if (sLabel2 == ScriptDef.sVAR_USERGOLDCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_USERGOLDCOUNT);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_USERGOLDCOUNT, SctiptDef.sVAR_USERGOLDCOUNT);
            }
            else if (sLabel2 == ScriptDef.sVAR_MAXGOLDCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_MAXGOLDCOUNT);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_MAXGOLDCOUNT, SctiptDef.sVAR_MAXGOLDCOUNT);
            }
            else if (sLabel2 == ScriptDef.sVAR_STORAGEGOLDCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_STORAGEGOLDCOUNT);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_STORAGEGOLDCOUNT, SctiptDef.sVAR_STORAGEGOLDCOUNT);
            }
            else if (sLabel2 == ScriptDef.sVAR_BINDGOLDCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_BINDGOLDCOUNT);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_BINDGOLDCOUNT, SctiptDef.sVAR_BINDGOLDCOUNT);
            }
            else if (sLabel2 == ScriptDef.sVAR_UPGRADEWEAPONFEE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_UPGRADEWEAPONFEE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_UPGRADEWEAPONFEE, SctiptDef.sVAR_UPGRADEWEAPONFEE);
            }
            else if (sLabel2 == ScriptDef.sVAR_USERWEAPON)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_USERWEAPON);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_USERWEAPON, SctiptDef.sVAR_USERWEAPON);
            }
            else if (sLabel2 == ScriptDef.sVAR_CMD_STARTQUEST)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptDef.tVAR_CMD_STARTQUEST);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_STARTQUEST, SctiptDef.sVAR_CMD_STARTQUEST);
            }
            //===========================================没有使用的========================================
            #endregion
            else if (HUtil32.CompareLStr(sLabel2, ScriptDef.sVAR_TEAM, ScriptDef.sVAR_TEAM.Length))
            {
                s14 = sLabel2.Substring(ScriptDef.sVAR_TEAM.Length + 1 - 1, 1);
                if (s14 != "")
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(ScriptDef.tVAR_TEAM, s14));
                }
                else
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, ScriptDef.sVAR_HUMAN, ScriptDef.sVAR_HUMAN.Length))
            {
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (s14 != "")
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(ScriptDef.tVAR_HUMAN, s14));
                }
                else
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, ScriptDef.sVAR_GUILD, ScriptDef.sVAR_GUILD.Length))
            {
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (s14 != "")
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(ScriptDef.tVAR_GUILD, s14));
                }
                else
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, ScriptDef.sVAR_GLOBAL, ScriptDef.sVAR_GLOBAL.Length))
            {
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (s14 != "")
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(ScriptDef.tVAR_GLOBAL, s14));
                }
                else
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, ScriptDef.sVAR_STR, ScriptDef.sVAR_STR.Length))
            {
                //'欢迎使用个人银行储蓄，目前完全免费，请多利用。\ \<您的个人银行存款有/@-1>：<$46><｜/@-2><$125/G18>\ \<您的包裹里以携带有/AUTOCOLOR=249>：<$GOLDCOUNT><｜/@-2><$GOLDCOUNTX>\ \ \<存入金币/@@InPutInteger1>      <取出金币/@@InPutInteger2>      <返 回/@Main>'
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (s14 != "")
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(ScriptDef.tVAR_STR, s14));
                }
                else
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, ScriptDef.sVAR_MISSIONARITHMOMETER, ScriptDef.sVAR_MISSIONARITHMOMETER.Length))
            {
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (s14 != "")
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(ScriptDef.tVAR_MISSIONARITHMOMETER, s14));
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