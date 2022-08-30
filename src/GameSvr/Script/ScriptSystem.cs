using GameSvr.Npc;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using SystemModule;
using SystemModule.Common;

namespace GameSvr.Script
{
    public class ScriptSystem
    {
        private readonly Dictionary<string, string> sCallScriptDict = new Dictionary<string, string>();

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
            if (File.Exists(sFileName))
            {
                var LoadStrList = new StringList();
                LoadStrList.LoadFromFile(sFileName);
                sLabel = '[' + sLabel + ']';
                var bo1D = false;
                for (var i = 0; i < LoadStrList.Count; i++)
                {
                    string sLine = LoadStrList[i].Trim();
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
                        TDefineInfo DefineInfo = new TDefineInfo
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
                            StringList LoadStrList = new StringList();
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
                case ScriptCommandConst.sCHECK:
                    {
                        nCMDCode = ScriptCommandConst.nCHECK;
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
                case ScriptCommandConst.sCHECKOPEN:
                    {
                        nCMDCode = ScriptCommandConst.nCHECKOPEN;
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
                case ScriptCommandConst.sCHECKUNIT:
                    {
                        nCMDCode = ScriptCommandConst.nCHECKUNIT;
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
                case ScriptCommandConst.sCHECKPKPOINT:
                    nCMDCode = ScriptCommandConst.nCHECKPKPOINT;
                    goto L001;
                case ScriptCommandConst.sCHECKGOLD:
                    nCMDCode = ScriptCommandConst.nCHECKGOLD;
                    goto L001;
                case ScriptCommandConst.sCHECKLEVEL:
                    nCMDCode = ScriptCommandConst.nCHECKLEVEL;
                    goto L001;
                case ScriptCommandConst.sCHECKJOB:
                    nCMDCode = ScriptCommandConst.nCHECKJOB;
                    goto L001;
                case ScriptCommandConst.sRANDOM:
                    nCMDCode = ScriptCommandConst.nRANDOM;
                    goto L001;
                case ScriptCommandConst.sCHECKITEM:
                    nCMDCode = ScriptCommandConst.nCHECKITEM;
                    goto L001;
                case ScriptCommandConst.sGENDER:
                    nCMDCode = ScriptCommandConst.nGENDER;
                    goto L001;
                case ScriptCommandConst.sCHECKBAGGAGE:
                    nCMDCode = ScriptCommandConst.nCHECKBAGGAGE;
                    goto L001;
                case ScriptCommandConst.sCHECKNAMELIST:
                    nCMDCode = ScriptCommandConst.nCHECKNAMELIST;
                    goto L001;
                case ScriptCommandConst.sSC_HASGUILD:
                    nCMDCode = ScriptCommandConst.nSC_HASGUILD;
                    goto L001;
                case ScriptCommandConst.sSC_ISGUILDMASTER:
                    nCMDCode = ScriptCommandConst.nSC_ISGUILDMASTER;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKCASTLEMASTER:
                    nCMDCode = ScriptCommandConst.nSC_CHECKCASTLEMASTER;
                    goto L001;
                case ScriptCommandConst.sSC_ISNEWHUMAN:
                    nCMDCode = ScriptCommandConst.nSC_ISNEWHUMAN;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKMEMBERTYPE:
                    nCMDCode = ScriptCommandConst.nSC_CHECKMEMBERTYPE;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKMEMBERLEVEL:
                    nCMDCode = ScriptCommandConst.nSC_CHECKMEMBERLEVEL;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKGAMEGOLD:
                    nCMDCode = ScriptCommandConst.nSC_CHECKGAMEGOLD;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKGAMEPOINT:
                    nCMDCode = ScriptCommandConst.nSC_CHECKGAMEPOINT;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKNAMELISTPOSITION:
                    nCMDCode = ScriptCommandConst.nSC_CHECKNAMELISTPOSITION;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKGUILDLIST:
                    nCMDCode = ScriptCommandConst.nSC_CHECKGUILDLIST;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKRENEWLEVEL:
                    nCMDCode = ScriptCommandConst.nSC_CHECKRENEWLEVEL;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKSLAVELEVEL:
                    nCMDCode = ScriptCommandConst.nSC_CHECKSLAVELEVEL;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKSLAVENAME:
                    nCMDCode = ScriptCommandConst.nSC_CHECKSLAVENAME;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKCREDITPOINT:
                    nCMDCode = ScriptCommandConst.nSC_CHECKCREDITPOINT;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKOFGUILD:
                    nCMDCode = ScriptCommandConst.nSC_CHECKOFGUILD;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKPAYMENT:
                    nCMDCode = ScriptCommandConst.nSC_CHECKPAYMENT;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKUSEITEM:
                    nCMDCode = ScriptCommandConst.nSC_CHECKUSEITEM;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKBAGSIZE:
                    nCMDCode = ScriptCommandConst.nSC_CHECKBAGSIZE;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKLISTCOUNT:
                    nCMDCode = ScriptCommandConst.nSC_CHECKLISTCOUNT;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKDC:
                    nCMDCode = ScriptCommandConst.nSC_CHECKDC;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKMC:
                    nCMDCode = ScriptCommandConst.nSC_CHECKMC;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKSC:
                    nCMDCode = ScriptCommandConst.nSC_CHECKSC;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKHP:
                    nCMDCode = ScriptCommandConst.nSC_CHECKHP;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKMP:
                    nCMDCode = ScriptCommandConst.nSC_CHECKMP;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKITEMTYPE:
                    nCMDCode = ScriptCommandConst.nSC_CHECKITEMTYPE;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKEXP:
                    nCMDCode = ScriptCommandConst.nSC_CHECKEXP;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKCASTLEGOLD:
                    nCMDCode = ScriptCommandConst.nSC_CHECKCASTLEGOLD;
                    goto L001;
                case ScriptCommandConst.sSC_PASSWORDERRORCOUNT:
                    nCMDCode = ScriptCommandConst.nSC_PASSWORDERRORCOUNT;
                    goto L001;
                case ScriptCommandConst.sSC_ISLOCKPASSWORD:
                    nCMDCode = ScriptCommandConst.nSC_ISLOCKPASSWORD;
                    goto L001;
                case ScriptCommandConst.sSC_ISLOCKSTORAGE:
                    nCMDCode = ScriptCommandConst.nSC_ISLOCKSTORAGE;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKBUILDPOINT:
                    nCMDCode = ScriptCommandConst.nSC_CHECKBUILDPOINT;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKAURAEPOINT:
                    nCMDCode = ScriptCommandConst.nSC_CHECKAURAEPOINT;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKSTABILITYPOINT:
                    nCMDCode = ScriptCommandConst.nSC_CHECKSTABILITYPOINT;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKFLOURISHPOINT:
                    nCMDCode = ScriptCommandConst.nSC_CHECKFLOURISHPOINT;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKCONTRIBUTION:
                    nCMDCode = ScriptCommandConst.nSC_CHECKCONTRIBUTION;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKRANGEMONCOUNT:
                    nCMDCode = ScriptCommandConst.nSC_CHECKRANGEMONCOUNT;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKITEMADDVALUE:
                    nCMDCode = ScriptCommandConst.nSC_CHECKITEMADDVALUE;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKINMAPRANGE:
                    nCMDCode = ScriptCommandConst.nSC_CHECKINMAPRANGE;
                    goto L001;
                case ScriptCommandConst.sSC_CASTLECHANGEDAY:
                    nCMDCode = ScriptCommandConst.nSC_CASTLECHANGEDAY;
                    goto L001;
                case ScriptCommandConst.sSC_CASTLEWARDAY:
                    nCMDCode = ScriptCommandConst.nSC_CASTLEWARDAY;
                    goto L001;
                case ScriptCommandConst.sSC_ONLINELONGMIN:
                    nCMDCode = ScriptCommandConst.nSC_ONLINELONGMIN;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKGUILDCHIEFITEMCOUNT:
                    nCMDCode = ScriptCommandConst.nSC_CHECKGUILDCHIEFITEMCOUNT;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKNAMEDATELIST:
                    nCMDCode = ScriptCommandConst.nSC_CHECKNAMEDATELIST;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKMAPHUMANCOUNT:
                    nCMDCode = ScriptCommandConst.nSC_CHECKMAPHUMANCOUNT;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKMAPMONCOUNT:
                    nCMDCode = ScriptCommandConst.nSC_CHECKMAPMONCOUNT;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKVAR:
                    nCMDCode = ScriptCommandConst.nSC_CHECKVAR;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKSERVERNAME:
                    nCMDCode = ScriptCommandConst.nSC_CHECKSERVERNAME;
                    goto L001;
                case ScriptCommandConst.sSC_ISATTACKGUILD:
                    nCMDCode = ScriptCommandConst.nSC_ISATTACKGUILD;
                    goto L001;
                case ScriptCommandConst.sSC_ISDEFENSEGUILD:
                    nCMDCode = ScriptCommandConst.nSC_ISDEFENSEGUILD;
                    goto L001;
                case ScriptCommandConst.sSC_ISATTACKALLYGUILD:
                    nCMDCode = ScriptCommandConst.nSC_ISATTACKALLYGUILD;
                    goto L001;
                case ScriptCommandConst.sSC_ISDEFENSEALLYGUILD:
                    nCMDCode = ScriptCommandConst.nSC_ISDEFENSEALLYGUILD;
                    goto L001;
                case ScriptCommandConst.sSC_ISCASTLEGUILD:
                    nCMDCode = ScriptCommandConst.nSC_ISCASTLEGUILD;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKCASTLEDOOR:
                    nCMDCode = ScriptCommandConst.nSC_CHECKCASTLEDOOR;
                    goto L001;
                case ScriptCommandConst.sSC_ISSYSOP:
                    nCMDCode = ScriptCommandConst.nSC_ISSYSOP;
                    goto L001;
                case ScriptCommandConst.sSC_ISADMIN:
                    nCMDCode = ScriptCommandConst.nSC_ISADMIN;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKGROUPCOUNT:
                    nCMDCode = ScriptCommandConst.nSC_CHECKGROUPCOUNT;
                    goto L001;
                case ScriptCommandConst.sCHECKACCOUNTLIST:
                    nCMDCode = ScriptCommandConst.nCHECKACCOUNTLIST;
                    goto L001;
                case ScriptCommandConst.sCHECKIPLIST:
                    nCMDCode = ScriptCommandConst.nCHECKIPLIST;
                    goto L001;
                case ScriptCommandConst.sCHECKBBCOUNT:
                    nCMDCode = ScriptCommandConst.nCHECKBBCOUNT;
                    goto L001;
            }

            if (sCmd == ScriptCommandConst.sCHECKCREDITPOINT)
            {
                nCMDCode = ScriptCommandConst.nCHECKCREDITPOINT;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sDAYTIME)
            {
                nCMDCode = ScriptCommandConst.nDAYTIME;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sCHECKITEMW)
            {
                nCMDCode = ScriptCommandConst.nCHECKITEMW;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sISTAKEITEM)
            {
                nCMDCode = ScriptCommandConst.nISTAKEITEM;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sCHECKDURA)
            {
                nCMDCode = ScriptCommandConst.nCHECKDURA;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sCHECKDURAEVA)
            {
                nCMDCode = ScriptCommandConst.nCHECKDURAEVA;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sDAYOFWEEK)
            {
                nCMDCode = ScriptCommandConst.nDAYOFWEEK;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sHOUR)
            {
                nCMDCode = ScriptCommandConst.nHOUR;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sMIN)
            {
                nCMDCode = ScriptCommandConst.nMIN;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sCHECKLUCKYPOINT)
            {
                nCMDCode = ScriptCommandConst.nCHECKLUCKYPOINT;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sCHECKMONMAP)
            {
                nCMDCode = ScriptCommandConst.nCHECKMONMAP;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sCHECKMONAREA)
            {
                nCMDCode = ScriptCommandConst.nCHECKMONAREA;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sCHECKHUM)
            {
                nCMDCode = ScriptCommandConst.nCHECKHUM;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sEQUAL)
            {
                nCMDCode = ScriptCommandConst.nEQUAL;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sLARGE)
            {
                nCMDCode = ScriptCommandConst.nLARGE;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSMALL)
            {
                nCMDCode = ScriptCommandConst.nSMALL;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_CHECKPOSEDIR)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKPOSEDIR;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_CHECKPOSELEVEL)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKPOSELEVEL;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_CHECKPOSEGENDER)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKPOSEGENDER;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_CHECKLEVELEX)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKLEVELEX;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_CHECKBONUSPOINT)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKBONUSPOINT;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_CHECKMARRY)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKMARRY;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_CHECKPOSEMARRY)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKPOSEMARRY;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_CHECKMARRYCOUNT)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKMARRYCOUNT;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_CHECKMASTER)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKMASTER;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_HAVEMASTER)
            {
                nCMDCode = ScriptCommandConst.nSC_HAVEMASTER;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_CHECKPOSEMASTER)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKPOSEMASTER;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_POSEHAVEMASTER)
            {
                nCMDCode = ScriptCommandConst.nSC_POSEHAVEMASTER;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_CHECKISMASTER)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKISMASTER;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_CHECKPOSEISMASTER)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKPOSEISMASTER;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_CHECKNAMEIPLIST)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKNAMEIPLIST;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_CHECKACCOUNTIPLIST)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKACCOUNTIPLIST;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_CHECKSLAVECOUNT)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKSLAVECOUNT;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_CHECKPOS)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKPOS;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_CHECKMAP)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKMAP;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_REVIVESLAVE)
            {
                nCMDCode = ScriptCommandConst.nSC_REVIVESLAVE;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_CHECKMAGICLVL)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKMAGICLVL;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_CHECKGROUPCLASS)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKGROUPCLASS;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_ISGROUPMASTER)
            {
                nCMDCode = ScriptCommandConst.nSC_ISGROUPMASTER;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sCheckDiemon)
            {
                nCMDCode = ScriptCommandConst.nCheckDiemon;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.scheckkillplaymon)
            {
                nCMDCode = ScriptCommandConst.ncheckkillplaymon;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_CHECKRANDOMNO)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKRANDOMNO;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_CHECKISONMAP)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKISONMAP;
                goto L001;
            }
            // 检测是否安全区
            if (sCmd == ScriptCommandConst.sSC_CHECKINSAFEZONE)
            {
                nCMDCode = ScriptCommandConst.nSC_CHECKINSAFEZONE;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_KILLBYHUM)
            {
                nCMDCode = ScriptCommandConst.nSC_KILLBYHUM;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_KILLBYMON)
            {
                nCMDCode = ScriptCommandConst.nSC_KILLBYMON;
                goto L001;
            }
            // 增加挂机
            if (sCmd == ScriptCommandConst.sSC_OffLine)
            {
                nCMDCode = ScriptCommandConst.nSC_OffLine;
                goto L001;
            }
            // 增加脚本特修所有装备命令
            if (sCmd == ScriptCommandConst.sSC_REPAIRALL)
            {
                nCMDCode = ScriptCommandConst.nSC_REPAIRALL;
                goto L001;
            }
            // 刷新包裹物品命令
            if (sCmd == ScriptCommandConst.sSC_QUERYBAGITEMS)
            {
                nCMDCode = ScriptCommandConst.nSC_QUERYBAGITEMS;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_SETRANDOMNO)
            {
                nCMDCode = ScriptCommandConst.nSC_SETRANDOMNO;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSC_DELAYGOTO || sCmd == "DELAYCALL")
            {
                nCMDCode = ScriptCommandConst.nSC_DELAYGOTO;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSCHECKDEATHPLAYMON)
            {
                nCMDCode = ScriptCommandConst.nSCHECKDEATHPLAYMON;
                goto L001;
            }
            if (sCmd == ScriptCommandConst.sSCHECKKILLMOBNAME)
            {
                nCMDCode = ScriptCommandConst.nSCHECKDEATHPLAYMON;
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
                case ScriptCommandConst.sSET:
                    {
                        nCMDCode = ScriptCommandConst.nSET;
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
                case ScriptCommandConst.sRESET:
                    {
                        nCMDCode = ScriptCommandConst.nRESET;
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
                case ScriptCommandConst.sSETOPEN:
                    {
                        nCMDCode = ScriptCommandConst.nSETOPEN;
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
                case ScriptCommandConst.sSETUNIT:
                    {
                        nCMDCode = ScriptCommandConst.nSETUNIT;
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
                case ScriptCommandConst.sRESETUNIT:
                    {
                        nCMDCode = ScriptCommandConst.nRESETUNIT;
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
                case ScriptCommandConst.sTAKE:
                    nCMDCode = ScriptCommandConst.nTAKE;
                    goto L001;
                case ScriptCommandConst.sSC_GIVE:
                    nCMDCode = ScriptCommandConst.nSC_GIVE;
                    goto L001;
                case ScriptCommandConst.sCLOSE:
                    nCMDCode = ScriptCommandConst.nCLOSE;
                    goto L001;
                case ScriptCommandConst.sBREAK:
                    nCMDCode = ScriptCommandConst.nBREAK;
                    goto L001;
                case ScriptCommandConst.sGOTO:
                    nCMDCode = ScriptCommandConst.nGOTO;
                    goto L001;
                case ScriptCommandConst.sADDNAMELIST:
                    nCMDCode = ScriptCommandConst.nADDNAMELIST;
                    goto L001;
                case ScriptCommandConst.sDELNAMELIST:
                    nCMDCode = ScriptCommandConst.nDELNAMELIST;
                    goto L001;
                case ScriptCommandConst.sADDGUILDLIST:
                    nCMDCode = ScriptCommandConst.nADDGUILDLIST;
                    goto L001;
                case ScriptCommandConst.sDELGUILDLIST:
                    nCMDCode = ScriptCommandConst.nDELGUILDLIST;
                    goto L001;
                case ScriptCommandConst.sSC_LINEMSG:
                    nCMDCode = ScriptCommandConst.nSC_LINEMSG;
                    goto L001;
                case ScriptCommandConst.sADDACCOUNTLIST:
                    nCMDCode = ScriptCommandConst.nADDACCOUNTLIST;
                    goto L001;
                case ScriptCommandConst.sDELACCOUNTLIST:
                    nCMDCode = ScriptCommandConst.nDELACCOUNTLIST;
                    goto L001;
                case ScriptCommandConst.sADDIPLIST:
                    nCMDCode = ScriptCommandConst.nADDIPLIST;
                    goto L001;
                case ScriptCommandConst.sDELIPLIST:
                    nCMDCode = ScriptCommandConst.nDELIPLIST;
                    goto L001;
                case ScriptCommandConst.sSENDMSG:
                    nCMDCode = ScriptCommandConst.nSENDMSG;
                    goto L001;
                case ScriptCommandConst.sCHANGEMODE:
                    nCMDCode = ScriptCommandConst.nCHANGEMODE;
                    nCMDCode = ScriptCommandConst.nSC_CHANGEMODE;
                    goto L001;
                case ScriptCommandConst.sPKPOINT:
                    nCMDCode = ScriptCommandConst.nPKPOINT;
                    goto L001;
                case ScriptCommandConst.sCHANGEXP:
                    nCMDCode = ScriptCommandConst.nCHANGEXP;
                    goto L001;
                case ScriptCommandConst.sSC_RECALLMOB:
                    nCMDCode = ScriptCommandConst.nSC_RECALLMOB;
                    goto L001;
                case ScriptCommandConst.sTAKEW:
                    nCMDCode = ScriptCommandConst.nTAKEW;
                    goto L001;
                case ScriptCommandConst.sTIMERECALL:
                    nCMDCode = ScriptCommandConst.nTIMERECALL;
                    goto L001;
                case ScriptCommandConst.sSC_PARAM1:
                    nCMDCode = ScriptCommandConst.nSC_PARAM1;
                    goto L001;
                case ScriptCommandConst.sSC_PARAM2:
                    nCMDCode = ScriptCommandConst.nSC_PARAM2;
                    goto L001;
                case ScriptCommandConst.sSC_PARAM3:
                    nCMDCode = ScriptCommandConst.nSC_PARAM3;
                    goto L001;
                case ScriptCommandConst.sSC_PARAM4:
                    nCMDCode = ScriptCommandConst.nSC_PARAM4;
                    goto L001;
                case ScriptCommandConst.sSC_EXEACTION:
                    nCMDCode = ScriptCommandConst.nSC_EXEACTION;
                    goto L001;
                case ScriptCommandConst.sMAPMOVE:
                    nCMDCode = ScriptCommandConst.nMAPMOVE;
                    goto L001;
                case ScriptCommandConst.sMAP:
                    nCMDCode = ScriptCommandConst.nMAP;
                    goto L001;
                case ScriptCommandConst.sTAKECHECKITEM:
                    nCMDCode = ScriptCommandConst.nTAKECHECKITEM;
                    goto L001;
                case ScriptCommandConst.sMONGEN:
                    nCMDCode = ScriptCommandConst.nMONGEN;
                    goto L001;
                case ScriptCommandConst.sMONCLEAR:
                    nCMDCode = ScriptCommandConst.nMONCLEAR;
                    goto L001;
                case ScriptCommandConst.sMOV:
                    nCMDCode = ScriptCommandConst.nMOV;
                    goto L001;
                case ScriptCommandConst.sINC:
                    nCMDCode = ScriptCommandConst.nINC;
                    goto L001;
                case ScriptCommandConst.sDEC:
                    nCMDCode = ScriptCommandConst.nDEC;
                    goto L001;
                case ScriptCommandConst.sSUM:
                    nCMDCode = ScriptCommandConst.nSUM;
                    goto L001;
                //变量运算
                //除法
                case ScriptCommandConst.sSC_DIV:
                    nCMDCode = ScriptCommandConst.nSC_DIV;
                    goto L001;
                //除法
                case ScriptCommandConst.sSC_MUL:
                    nCMDCode = ScriptCommandConst.nSC_MUL;
                    goto L001;
                //除法
                case ScriptCommandConst.sSC_PERCENT:
                    nCMDCode = ScriptCommandConst.nSC_PERCENT;
                    goto L001;
                case ScriptCommandConst.sTHROWITEM:
                case ScriptCommandConst.sDROPITEMMAP:
                    nCMDCode = ScriptCommandConst.nTHROWITEM;
                    goto L001;
                case ScriptCommandConst.sBREAKTIMERECALL:
                    nCMDCode = ScriptCommandConst.nBREAKTIMERECALL;
                    goto L001;
                case ScriptCommandConst.sMOVR:
                    nCMDCode = ScriptCommandConst.nMOVR;
                    goto L001;
                case ScriptCommandConst.sEXCHANGEMAP:
                    nCMDCode = ScriptCommandConst.nEXCHANGEMAP;
                    goto L001;
                case ScriptCommandConst.sRECALLMAP:
                    nCMDCode = ScriptCommandConst.nRECALLMAP;
                    goto L001;
                case ScriptCommandConst.sADDBATCH:
                    nCMDCode = ScriptCommandConst.nADDBATCH;
                    goto L001;
                case ScriptCommandConst.sBATCHDELAY:
                    nCMDCode = ScriptCommandConst.nBATCHDELAY;
                    goto L001;
                case ScriptCommandConst.sBATCHMOVE:
                    nCMDCode = ScriptCommandConst.nBATCHMOVE;
                    goto L001;
                case ScriptCommandConst.sPLAYDICE:
                    nCMDCode = ScriptCommandConst.nPLAYDICE;
                    goto L001;
                case ScriptCommandConst.sGOQUEST:
                    nCMDCode = ScriptCommandConst.nGOQUEST;
                    goto L001;
                case ScriptCommandConst.sENDQUEST:
                    nCMDCode = ScriptCommandConst.nENDQUEST;
                    goto L001;
                case ScriptCommandConst.sSC_HAIRCOLOR:
                    nCMDCode = ScriptCommandConst.nSC_HAIRCOLOR;
                    goto L001;
                case ScriptCommandConst.sSC_WEARCOLOR:
                    nCMDCode = ScriptCommandConst.nSC_WEARCOLOR;
                    goto L001;
                case ScriptCommandConst.sSC_HAIRSTYLE:
                    nCMDCode = ScriptCommandConst.nSC_HAIRSTYLE;
                    goto L001;
                case ScriptCommandConst.sSC_MONRECALL:
                    nCMDCode = ScriptCommandConst.nSC_MONRECALL;
                    goto L001;
                case ScriptCommandConst.sSC_HORSECALL:
                    nCMDCode = ScriptCommandConst.nSC_HORSECALL;
                    goto L001;
                case ScriptCommandConst.sSC_HAIRRNDCOL:
                    nCMDCode = ScriptCommandConst.nSC_HAIRRNDCOL;
                    goto L001;
                case ScriptCommandConst.sSC_KILLHORSE:
                    nCMDCode = ScriptCommandConst.nSC_KILLHORSE;
                    goto L001;
                case ScriptCommandConst.sSC_RANDSETDAILYQUEST:
                    nCMDCode = ScriptCommandConst.nSC_RANDSETDAILYQUEST;
                    goto L001;
                case ScriptCommandConst.sSC_CHANGELEVEL:
                    nCMDCode = ScriptCommandConst.nSC_CHANGELEVEL;
                    goto L001;
                case ScriptCommandConst.sSC_MARRY:
                    nCMDCode = ScriptCommandConst.nSC_MARRY;
                    goto L001;
                case ScriptCommandConst.sSC_UNMARRY:
                    nCMDCode = ScriptCommandConst.nSC_UNMARRY;
                    goto L001;
                case ScriptCommandConst.sSC_GETMARRY:
                    nCMDCode = ScriptCommandConst.nSC_GETMARRY;
                    goto L001;
                case ScriptCommandConst.sSC_GETMASTER:
                    nCMDCode = ScriptCommandConst.nSC_GETMASTER;
                    goto L001;
                case ScriptCommandConst.sSC_CLEARSKILL:
                    nCMDCode = ScriptCommandConst.nSC_CLEARSKILL;
                    goto L001;
                case ScriptCommandConst.sSC_DELNOJOBSKILL:
                    nCMDCode = ScriptCommandConst.nSC_DELNOJOBSKILL;
                    goto L001;
                case ScriptCommandConst.sSC_DELSKILL:
                    nCMDCode = ScriptCommandConst.nSC_DELSKILL;
                    goto L001;
                case ScriptCommandConst.sSC_ADDSKILL:
                    nCMDCode = ScriptCommandConst.nSC_ADDSKILL;
                    goto L001;
                case ScriptCommandConst.sSC_SKILLLEVEL:
                    nCMDCode = ScriptCommandConst.nSC_SKILLLEVEL;
                    goto L001;
                case ScriptCommandConst.sSC_CHANGEPKPOINT:
                    nCMDCode = ScriptCommandConst.nSC_CHANGEPKPOINT;
                    goto L001;
                case ScriptCommandConst.sSC_CHANGEEXP:
                    nCMDCode = ScriptCommandConst.nSC_CHANGEEXP;
                    goto L001;
                case ScriptCommandConst.sSC_CHANGEJOB:
                    nCMDCode = ScriptCommandConst.nSC_CHANGEJOB;
                    goto L001;
                case ScriptCommandConst.sSC_MISSION:
                    nCMDCode = ScriptCommandConst.nSC_MISSION;
                    goto L001;
                case ScriptCommandConst.sSC_MOBPLACE:
                    nCMDCode = ScriptCommandConst.nSC_MOBPLACE;
                    goto L001;
                case ScriptCommandConst.sSC_SETMEMBERTYPE:
                    nCMDCode = ScriptCommandConst.nSC_SETMEMBERTYPE;
                    goto L001;
                case ScriptCommandConst.sSC_SETMEMBERLEVEL:
                    nCMDCode = ScriptCommandConst.nSC_SETMEMBERLEVEL;
                    goto L001;
                case ScriptCommandConst.sSC_GAMEGOLD:
                    nCMDCode = ScriptCommandConst.nSC_GAMEGOLD;
                    goto L001;
                case ScriptCommandConst.sSC_GAMEPOINT:
                    nCMDCode = ScriptCommandConst.nSC_GAMEPOINT;
                    goto L001;
                case ScriptCommandConst.sSC_PKZONE:
                    nCMDCode = ScriptCommandConst.nSC_PKZONE;
                    goto L001;
                case ScriptCommandConst.sSC_RESTBONUSPOINT:
                    nCMDCode = ScriptCommandConst.nSC_RESTBONUSPOINT;
                    goto L001;
                case ScriptCommandConst.sSC_TAKECASTLEGOLD:
                    nCMDCode = ScriptCommandConst.nSC_TAKECASTLEGOLD;
                    goto L001;
                case ScriptCommandConst.sSC_HUMANHP:
                    nCMDCode = ScriptCommandConst.nSC_HUMANHP;
                    goto L001;
                case ScriptCommandConst.sSC_HUMANMP:
                    nCMDCode = ScriptCommandConst.nSC_HUMANMP;
                    goto L001;
                case ScriptCommandConst.sSC_BUILDPOINT:
                    nCMDCode = ScriptCommandConst.nSC_BUILDPOINT;
                    goto L001;
                case ScriptCommandConst.sSC_AURAEPOINT:
                    nCMDCode = ScriptCommandConst.nSC_AURAEPOINT;
                    goto L001;
                case ScriptCommandConst.sSC_STABILITYPOINT:
                    nCMDCode = ScriptCommandConst.nSC_STABILITYPOINT;
                    goto L001;
                case ScriptCommandConst.sSC_FLOURISHPOINT:
                    nCMDCode = ScriptCommandConst.nSC_FLOURISHPOINT;
                    goto L001;
                case ScriptCommandConst.sSC_OPENMAGICBOX:
                    nCMDCode = ScriptCommandConst.nSC_OPENMAGICBOX;
                    goto L001;
                case ScriptCommandConst.sSC_SETRANKLEVELNAME:
                    nCMDCode = ScriptCommandConst.nSC_SETRANKLEVELNAME;
                    goto L001;
                case ScriptCommandConst.sSC_GMEXECUTE:
                    nCMDCode = ScriptCommandConst.nSC_GMEXECUTE;
                    goto L001;
                case ScriptCommandConst.sSC_GUILDCHIEFITEMCOUNT:
                    nCMDCode = ScriptCommandConst.nSC_GUILDCHIEFITEMCOUNT;
                    goto L001;
                case ScriptCommandConst.sSC_ADDNAMEDATELIST:
                    nCMDCode = ScriptCommandConst.nSC_ADDNAMEDATELIST;
                    goto L001;
                case ScriptCommandConst.sSC_DELNAMEDATELIST:
                    nCMDCode = ScriptCommandConst.nSC_DELNAMEDATELIST;
                    goto L001;
                case ScriptCommandConst.sSC_MOBFIREBURN:
                    nCMDCode = ScriptCommandConst.nSC_MOBFIREBURN;
                    goto L001;
                case ScriptCommandConst.sSC_MESSAGEBOX:
                    nCMDCode = ScriptCommandConst.nSC_MESSAGEBOX;
                    goto L001;
                case ScriptCommandConst.sSC_SETSCRIPTFLAG:
                    nCMDCode = ScriptCommandConst.nSC_SETSCRIPTFLAG;
                    goto L001;
                case ScriptCommandConst.sSC_SETAUTOGETEXP:
                    nCMDCode = ScriptCommandConst.nSC_SETAUTOGETEXP;
                    goto L001;
                case ScriptCommandConst.sSC_VAR:
                    nCMDCode = ScriptCommandConst.nSC_VAR;
                    goto L001;
                case ScriptCommandConst.sSC_LOADVAR:
                    nCMDCode = ScriptCommandConst.nSC_LOADVAR;
                    goto L001;
                case ScriptCommandConst.sSC_SAVEVAR:
                    nCMDCode = ScriptCommandConst.nSC_SAVEVAR;
                    goto L001;
                case ScriptCommandConst.sSC_CALCVAR:
                    nCMDCode = ScriptCommandConst.nSC_CALCVAR;
                    goto L001;
                case ScriptCommandConst.sSC_AUTOADDGAMEGOLD:
                    nCMDCode = ScriptCommandConst.nSC_AUTOADDGAMEGOLD;
                    goto L001;
                case ScriptCommandConst.sSC_AUTOSUBGAMEGOLD:
                    nCMDCode = ScriptCommandConst.nSC_AUTOSUBGAMEGOLD;
                    goto L001;
                case ScriptCommandConst.sSC_RECALLGROUPMEMBERS:
                    nCMDCode = ScriptCommandConst.nSC_RECALLGROUPMEMBERS;
                    goto L001;
                case ScriptCommandConst.sSC_CLEARNAMELIST:
                    nCMDCode = ScriptCommandConst.nSC_CLEARNAMELIST;
                    goto L001;
                case ScriptCommandConst.sSC_CHANGENAMECOLOR:
                    nCMDCode = ScriptCommandConst.nSC_CHANGENAMECOLOR;
                    goto L001;
                case ScriptCommandConst.sSC_CLEARPASSWORD:
                    nCMDCode = ScriptCommandConst.nSC_CLEARPASSWORD;
                    goto L001;
                case ScriptCommandConst.sSC_RENEWLEVEL:
                    nCMDCode = ScriptCommandConst.nSC_RENEWLEVEL;
                    goto L001;
                case ScriptCommandConst.sSC_KILLMONEXPRATE:
                    nCMDCode = ScriptCommandConst.nSC_KILLMONEXPRATE;
                    goto L001;
                case ScriptCommandConst.sSC_POWERRATE:
                    nCMDCode = ScriptCommandConst.nSC_POWERRATE;
                    goto L001;
                case ScriptCommandConst.sSC_CHANGEPERMISSION:
                    nCMDCode = ScriptCommandConst.nSC_CHANGEPERMISSION;
                    goto L001;
                case ScriptCommandConst.sSC_KILL:
                    nCMDCode = ScriptCommandConst.nSC_KILL;
                    goto L001;
                case ScriptCommandConst.sSC_KICK:
                    nCMDCode = ScriptCommandConst.nSC_KICK;
                    goto L001;
                case ScriptCommandConst.sSC_BONUSPOINT:
                    nCMDCode = ScriptCommandConst.nSC_BONUSPOINT;
                    goto L001;
                case ScriptCommandConst.sSC_RESTRENEWLEVEL:
                    nCMDCode = ScriptCommandConst.nSC_RESTRENEWLEVEL;
                    goto L001;
                case ScriptCommandConst.sSC_DELMARRY:
                    nCMDCode = ScriptCommandConst.nSC_DELMARRY;
                    goto L001;
                case ScriptCommandConst.sSC_DELMASTER:
                    nCMDCode = ScriptCommandConst.nSC_DELMASTER;
                    goto L001;
                case ScriptCommandConst.sSC_MASTER:
                    nCMDCode = ScriptCommandConst.nSC_MASTER;
                    goto L001;
                case ScriptCommandConst.sSC_UNMASTER:
                    nCMDCode = ScriptCommandConst.nSC_UNMASTER;
                    goto L001;
                case ScriptCommandConst.sSC_CREDITPOINT:
                    nCMDCode = ScriptCommandConst.nSC_CREDITPOINT;
                    goto L001;
                case ScriptCommandConst.sSC_CLEARNEEDITEMS:
                    nCMDCode = ScriptCommandConst.nSC_CLEARNEEDITEMS;
                    goto L001;
                case ScriptCommandConst.sSC_CLEARMAKEITEMS:
                    nCMDCode = ScriptCommandConst.nSC_CLEARMAEKITEMS;
                    goto L001;
                case ScriptCommandConst.sSC_SETSENDMSGFLAG:
                    nCMDCode = ScriptCommandConst.nSC_SETSENDMSGFLAG;
                    goto L001;
                case ScriptCommandConst.sSC_UPGRADEITEMS:
                    nCMDCode = ScriptCommandConst.nSC_UPGRADEITEMS;
                    goto L001;
                case ScriptCommandConst.sSC_UPGRADEITEMSEX:
                    nCMDCode = ScriptCommandConst.nSC_UPGRADEITEMSEX;
                    goto L001;
                case ScriptCommandConst.sSC_MONGENEX:
                    nCMDCode = ScriptCommandConst.nSC_MONGENEX;
                    goto L001;
                case ScriptCommandConst.sSC_CLEARMAPMON:
                    nCMDCode = ScriptCommandConst.nSC_CLEARMAPMON;
                    goto L001;
                case ScriptCommandConst.sSC_SETMAPMODE:
                    nCMDCode = ScriptCommandConst.nSC_SETMAPMODE;
                    goto L001;
                case ScriptCommandConst.sSC_KILLSLAVE:
                    nCMDCode = ScriptCommandConst.nSC_KILLSLAVE;
                    goto L001;
                case ScriptCommandConst.sSC_CHANGEGENDER:
                    nCMDCode = ScriptCommandConst.nSC_CHANGEGENDER;
                    goto L001;
                case ScriptCommandConst.sSC_MAPTING:
                    nCMDCode = ScriptCommandConst.nSC_MAPTING;
                    goto L001;
                case ScriptCommandConst.sSC_GUILDRECALL:
                    nCMDCode = ScriptCommandConst.nSC_GUILDRECALL;
                    goto L001;
                case ScriptCommandConst.sSC_GROUPRECALL:
                    nCMDCode = ScriptCommandConst.nSC_GROUPRECALL;
                    goto L001;
                case ScriptCommandConst.sSC_GROUPADDLIST:
                    nCMDCode = ScriptCommandConst.nSC_GROUPADDLIST;
                    goto L001;
                case ScriptCommandConst.sSC_CLEARLIST:
                    nCMDCode = ScriptCommandConst.nSC_CLEARLIST;
                    goto L001;
                case ScriptCommandConst.sSC_GROUPMOVEMAP:
                    nCMDCode = ScriptCommandConst.nSC_GROUPMOVEMAP;
                    goto L001;
                case ScriptCommandConst.sSC_SAVESLAVES:
                    nCMDCode = ScriptCommandConst.nSC_SAVESLAVES;
                    goto L001;
                case ScriptCommandConst.sCHECKUSERDATE:
                    nCMDCode = ScriptCommandConst.nCHECKUSERDATE;
                    goto L001;
                case ScriptCommandConst.sADDUSERDATE:
                    nCMDCode = ScriptCommandConst.nADDUSERDATE;
                    goto L001;
                case ScriptCommandConst.sCheckDiemon:
                    nCMDCode = ScriptCommandConst.nCheckDiemon;
                    goto L001;
                case ScriptCommandConst.scheckkillplaymon:
                    nCMDCode = ScriptCommandConst.ncheckkillplaymon;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKRANDOMNO:
                    nCMDCode = ScriptCommandConst.nSC_CHECKRANDOMNO;
                    goto L001;
                case ScriptCommandConst.sSC_CHECKISONMAP:
                    nCMDCode = ScriptCommandConst.nSC_CHECKISONMAP;
                    goto L001;
                // 检测是否安全区
                case ScriptCommandConst.sSC_CHECKINSAFEZONE:
                    nCMDCode = ScriptCommandConst.nSC_CHECKINSAFEZONE;
                    goto L001;
                case ScriptCommandConst.sSC_KILLBYHUM:
                    nCMDCode = ScriptCommandConst.nSC_KILLBYHUM;
                    goto L001;
                case ScriptCommandConst.sSC_KILLBYMON:
                    nCMDCode = ScriptCommandConst.nSC_KILLBYMON;
                    goto L001;
                case ScriptCommandConst.sSC_ISHIGH:
                    nCMDCode = ScriptCommandConst.nSC_ISHIGH;
                    goto L001;
                case ScriptCommandConst.sOPENYBDEAL:
                    nCMDCode = ScriptCommandConst.nOPENYBDEAL;
                    goto L001;
                case ScriptCommandConst.sQUERYYBSELL:
                    nCMDCode = ScriptCommandConst.nQUERYYBSELL;
                    goto L001;
                case ScriptCommandConst.sQUERYYBDEAL:
                    nCMDCode = ScriptCommandConst.nQUERYYBDEAL;
                    goto L001;
                case ScriptCommandConst.sDELAYGOTO:
                case ScriptCommandConst.sDELAYCALL:
                    nCMDCode = ScriptCommandConst.nDELAYGOTO;
                    goto L001;
                case ScriptCommandConst.sCLEARDELAYGOTO:
                    nCMDCode = ScriptCommandConst.nCLEARDELAYGOTO;
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
                                    if (s30.Equals(ScriptCommandConst.sBUY, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boBuy = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptCommandConst.sSELL, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boSell = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptCommandConst.sMAKEDURG, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boMakeDrug = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptCommandConst.sPRICES, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boPrices = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptCommandConst.sSTORAGE, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boStorage = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptCommandConst.sGETBACK, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boGetback = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptCommandConst.sUPGRADENOW, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boUpgradenow = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptCommandConst.sGETBACKUPGNOW, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boGetBackupgnow = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptCommandConst.sREPAIR, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boRepair = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptCommandConst.sSUPERREPAIR, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boS_repair = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptCommandConst.sSL_SENDMSG, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boSendmsg = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptCommandConst.sUSEITEMNAME, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boUseItemName = true;
                                        continue;
                                    }
                                    if (s30.Equals(ScriptCommandConst.sOFFLINEMSG, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((Merchant)NPC).m_boOffLineMsg = true;
                                        continue;
                                    }
                                    if (String.Compare(s30, (ScriptCommandConst.sybdeal), StringComparison.OrdinalIgnoreCase) == 0)
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
            for (var i = 0; i < NPC.FGotoLable.Length; i++)
            {
                NPC.FGotoLable[i] = -1;
            }
            //if (NPC.m_btNPCRaceServer == DataConst.NPC_RC_FUNMERCHANT)
            //{
            //    TFunMerchant FunMerchant = (TFunMerchant)NPC;
            //    for (int i = FunMerchant.FStdModeFunc.GetLowerBound(0); i <= FunMerchant.FStdModeFunc.Length; i++)
            //    {
            //        FunMerchant.FStdModeFunc[i] = -1;
            //    }
            //    for (int i = FunMerchant.FPlayLevelUp.GetLowerBound(0); i <= FunMerchant.FPlayLevelUp.Length; i++)
            //    {
            //        FunMerchant.FPlayLevelUp[i] = -1;
            //    }
            //    for (int i = FunMerchant.FUserCmd.GetLowerBound(0); i <= FunMerchant.FUserCmd.Length; i++)
            //    {
            //        FunMerchant.FUserCmd[i] = -1;
            //    }
            //    for (int i = FunMerchant.FClearMission.GetLowerBound(0); i <= FunMerchant.FClearMission.Length; i++)
            //    {
            //        FunMerchant.FClearMission[i] = -1;
            //    }
            //    for (int i = FunMerchant.FMagSelfFunc.GetLowerBound(0); i <= FunMerchant.FMagSelfFunc.Length; i++)
            //    {
            //        FunMerchant.FMagSelfFunc[i] = -1;
            //    }
            //    for (int i = FunMerchant.FMagTagFunc.GetLowerBound(0); i <= FunMerchant.FMagTagFunc.Length; i++)
            //    {
            //        FunMerchant.FMagTagFunc[i] = -1;
            //    }
            //    for (int i = FunMerchant.FMagTagFuncEx.GetLowerBound(0); i <= FunMerchant.FMagTagFuncEx.Length; i++)
            //    {
            //        FunMerchant.FMagTagFuncEx[i] = -1;
            //    }
            //    for (int i = FunMerchant.FMagMonFunc.GetLowerBound(0); i <= FunMerchant.FMagMonFunc.Length; i++)
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
                case ScriptCommandConst.SPLAYOFFLINE:
                    NPC.FGotoLable[ScriptCommandConst.NPLAYOFFLINE] = nIdx;
                    break;
                case ScriptCommandConst.SMARRYERROR:
                    NPC.FGotoLable[ScriptCommandConst.NMARRYERROR] = nIdx;
                    break;
                case ScriptCommandConst.SMASTERERROR:
                    NPC.FGotoLable[ScriptCommandConst.NMASTERERROR] = nIdx;
                    break;
                case ScriptCommandConst.SMARRYCHECKDIR:
                    NPC.FGotoLable[ScriptCommandConst.NMARRYCHECKDIR] = nIdx;
                    break;
                case ScriptCommandConst.SHUMANTYPEERR:
                    NPC.FGotoLable[ScriptCommandConst.NHUMANTYPEERR] = nIdx;
                    break;
                case ScriptCommandConst.SSTARTMARRY:
                    NPC.FGotoLable[ScriptCommandConst.NSTARTMARRY] = nIdx;
                    break;
                case ScriptCommandConst.SMARRYSEXERR:
                    NPC.FGotoLable[ScriptCommandConst.NMARRYSEXERR] = nIdx;
                    break;
                case ScriptCommandConst.SMARRYDIRERR:
                    NPC.FGotoLable[ScriptCommandConst.NMARRYDIRERR] = nIdx;
                    break;
                case ScriptCommandConst.SWATEMARRY:
                    NPC.FGotoLable[ScriptCommandConst.NWATEMARRY] = nIdx;
                    break;
                case ScriptCommandConst.SREVMARRY:
                    NPC.FGotoLable[ScriptCommandConst.NREVMARRY] = nIdx;
                    break;
                case ScriptCommandConst.SENDMARRY:
                    NPC.FGotoLable[ScriptCommandConst.NENDMARRY] = nIdx;
                    break;
                case ScriptCommandConst.SENDMARRYFAIL:
                    NPC.FGotoLable[ScriptCommandConst.NENDMARRYFAIL] = nIdx;
                    break;
                case ScriptCommandConst.SMASTERCHECKDIR:
                    NPC.FGotoLable[ScriptCommandConst.NMASTERCHECKDIR] = nIdx;
                    break;
                case ScriptCommandConst.SSTARTGETMASTER:
                    NPC.FGotoLable[ScriptCommandConst.NSTARTGETMASTER] = nIdx;
                    break;
                case ScriptCommandConst.SMASTERDIRERR:
                    NPC.FGotoLable[ScriptCommandConst.NMASTERDIRERR] = nIdx;
                    break;
                case ScriptCommandConst.SWATEMASTER:
                    NPC.FGotoLable[ScriptCommandConst.NWATEMASTER] = nIdx;
                    break;
                case ScriptCommandConst.SREVMASTER:
                    NPC.FGotoLable[ScriptCommandConst.NREVMASTER] = nIdx;
                    break;
                case ScriptCommandConst.SENDMASTER:
                    NPC.FGotoLable[ScriptCommandConst.NENDMASTER] = nIdx;
                    break;
                case ScriptCommandConst.SSTARTMASTER:
                    NPC.FGotoLable[ScriptCommandConst.NSTARTMASTER] = nIdx;
                    break;
                case ScriptCommandConst.SENDMASTERFAIL:
                    NPC.FGotoLable[ScriptCommandConst.NENDMASTERFAIL] = nIdx;
                    break;
                case ScriptCommandConst.SEXEMARRYFAIL:
                    NPC.FGotoLable[ScriptCommandConst.NEXEMARRYFAIL] = nIdx;
                    break;
                case ScriptCommandConst.SUNMARRYCHECKDIR:
                    NPC.FGotoLable[ScriptCommandConst.NUNMARRYCHECKDIR] = nIdx;
                    break;
                case ScriptCommandConst.SUNMARRYTYPEERR:
                    NPC.FGotoLable[ScriptCommandConst.NUNMARRYTYPEERR] = nIdx;
                    break;
                case ScriptCommandConst.SSTARTUNMARRY:
                    NPC.FGotoLable[ScriptCommandConst.NSTARTUNMARRY] = nIdx;
                    break;
                case ScriptCommandConst.SUNMARRYEND:
                    NPC.FGotoLable[ScriptCommandConst.NUNMARRYEND] = nIdx;
                    break;
                case ScriptCommandConst.SWATEUNMARRY:
                    NPC.FGotoLable[ScriptCommandConst.NWATEUNMARRY] = nIdx;
                    break;
                case ScriptCommandConst.SEXEMASTERFAIL:
                    NPC.FGotoLable[ScriptCommandConst.NEXEMASTERFAIL] = nIdx;
                    break;
                case ScriptCommandConst.SUNMASTERCHECKDIR:
                    NPC.FGotoLable[ScriptCommandConst.NUNMASTERCHECKDIR] = nIdx;
                    break;
                case ScriptCommandConst.SUNMASTERTYPEERR:
                    NPC.FGotoLable[ScriptCommandConst.NUNMASTERTYPEERR] = nIdx;
                    break;
                case ScriptCommandConst.SUNISMASTER:
                    NPC.FGotoLable[ScriptCommandConst.NUNISMASTER] = nIdx;
                    break;
                case ScriptCommandConst.SUNMASTERERROR:
                    NPC.FGotoLable[ScriptCommandConst.NUNMASTERERROR] = nIdx;
                    break;
                case ScriptCommandConst.SSTARTUNMASTER:
                    NPC.FGotoLable[ScriptCommandConst.NSTARTUNMASTER] = nIdx;
                    break;
                case ScriptCommandConst.SWATEUNMASTER:
                    NPC.FGotoLable[ScriptCommandConst.NWATEUNMASTER] = nIdx;
                    break;
                case ScriptCommandConst.SUNMASTEREND:
                    NPC.FGotoLable[ScriptCommandConst.NUNMASTEREND] = nIdx;
                    break;
                case ScriptCommandConst.SREVUNMASTER:
                    NPC.FGotoLable[ScriptCommandConst.NREVUNMASTER] = nIdx;
                    break;
                case ScriptCommandConst.SSUPREQUEST_OK:
                    NPC.FGotoLable[ScriptCommandConst.NSUPREQUEST_OK] = nIdx;
                    break;
                case ScriptCommandConst.SMEMBER:
                    NPC.FGotoLable[ScriptCommandConst.NMEMBER] = nIdx;
                    break;
                case ScriptCommandConst.SPLAYRECONNECTION:
                    NPC.FGotoLable[ScriptCommandConst.NPLAYRECONNECTION] = nIdx;
                    break;
                case ScriptCommandConst.SLOGIN:
                    NPC.FGotoLable[ScriptCommandConst.NLOGIN] = nIdx;
                    break;
                case ScriptCommandConst.SPLAYDIE:
                    NPC.FGotoLable[ScriptCommandConst.NPLAYDIE] = nIdx;
                    break;
                case ScriptCommandConst.SKILLPLAY:
                    NPC.FGotoLable[ScriptCommandConst.NKILLPLAY] = nIdx;
                    break;
                case ScriptCommandConst.SPLAYLEVELUP:
                    NPC.FGotoLable[ScriptCommandConst.NPLAYLEVELUP] = nIdx;
                    break;
                case ScriptCommandConst.SKILLMONSTER:
                    NPC.FGotoLable[ScriptCommandConst.NKILLMONSTER] = nIdx;
                    break;
                case ScriptCommandConst.SCREATEECTYPE_IN:
                    NPC.FGotoLable[ScriptCommandConst.NCREATEECTYPE_IN] = nIdx;
                    break;
                case ScriptCommandConst.SCREATEECTYPE_OK:
                    NPC.FGotoLable[ScriptCommandConst.NCREATEECTYPE_OK] = nIdx;
                    break;
                case ScriptCommandConst.SCREATEECTYPE_FAIL:
                    NPC.FGotoLable[ScriptCommandConst.NCREATEECTYPE_FAIL] = nIdx;
                    break;
                case ScriptCommandConst.SRESUME:
                    NPC.FGotoLable[ScriptCommandConst.NRESUME] = nIdx;
                    break;
                case ScriptCommandConst.SGETLARGESSGOLD_OK:
                    NPC.FGotoLable[ScriptCommandConst.NGETLARGESSGOLD_OK] = nIdx;
                    break;
                case ScriptCommandConst.SGETLARGESSGOLD_FAIL:
                    NPC.FGotoLable[ScriptCommandConst.NGETLARGESSGOLD_FAIL] = nIdx;
                    break;
                case ScriptCommandConst.SGETLARGESSGOLD_ERROR:
                    NPC.FGotoLable[ScriptCommandConst.NGETLARGESSGOLD_ERROR] = nIdx;
                    break;
                case ScriptCommandConst.SMASTERISPRENTICE:
                    NPC.FGotoLable[ScriptCommandConst.NMASTERISPRENTICE] = nIdx;
                    break;
                case ScriptCommandConst.SMASTERISFULL:
                    NPC.FGotoLable[ScriptCommandConst.NMASTERISFULL] = nIdx;
                    break;
                case ScriptCommandConst.SGROUPCREATE:
                    NPC.FGotoLable[ScriptCommandConst.NGROUPCREATE] = nIdx;
                    break;
                case ScriptCommandConst.SSTARTGROUP:
                    NPC.FGotoLable[ScriptCommandConst.NSTARTGROUP] = nIdx;
                    break;
                case ScriptCommandConst.SJOINGROUP:
                    NPC.FGotoLable[ScriptCommandConst.NJOINGROUP] = nIdx;
                    break;
                case ScriptCommandConst.SSPEEDCLOSE:
                    NPC.FGotoLable[ScriptCommandConst.NSPEEDCLOSE] = nIdx;
                    break;
                case ScriptCommandConst.SUPGRADENOW_OK:
                    NPC.FGotoLable[ScriptCommandConst.NUPGRADENOW_OK] = nIdx;
                    break;
                case ScriptCommandConst.SUPGRADENOW_ING:
                    NPC.FGotoLable[ScriptCommandConst.NUPGRADENOW_ING] = nIdx;
                    break;
                case ScriptCommandConst.SUPGRADENOW_FAIL:
                    NPC.FGotoLable[ScriptCommandConst.NUPGRADENOW_FAIL] = nIdx;
                    break;
                case ScriptCommandConst.SGETBACKUPGNOW_OK:
                    NPC.FGotoLable[ScriptCommandConst.NGETBACKUPGNOW_OK] = nIdx;
                    break;
                case ScriptCommandConst.SGETBACKUPGNOW_ING:
                    NPC.FGotoLable[ScriptCommandConst.NGETBACKUPGNOW_ING] = nIdx;
                    break;
                case ScriptCommandConst.SGETBACKUPGNOW_FAIL:
                    NPC.FGotoLable[ScriptCommandConst.NGETBACKUPGNOW_FAIL] = nIdx;
                    break;
                case ScriptCommandConst.SGETBACKUPGNOW_BAGFULL:
                    NPC.FGotoLable[ScriptCommandConst.NGETBACKUPGNOW_BAGFULL] = nIdx;
                    break;
                case ScriptCommandConst.STAKEONITEMS:
                    NPC.FGotoLable[ScriptCommandConst.NTAKEONITEMS] = nIdx;
                    break;
                case ScriptCommandConst.STAKEOFFITEMS:
                    NPC.FGotoLable[ScriptCommandConst.NTAKEOFFITEMS] = nIdx;
                    break;
                case ScriptCommandConst.SPLAYREVIVE:
                    NPC.FGotoLable[ScriptCommandConst.NPLAYREVIVE] = nIdx;
                    break;
                case ScriptCommandConst.SMOVEABILITY_OK:
                    NPC.FGotoLable[ScriptCommandConst.NMOVEABILITY_OK] = nIdx;
                    break;
                case ScriptCommandConst.SMOVEABILITY_FAIL:
                    NPC.FGotoLable[ScriptCommandConst.NMOVEABILITY_FAIL] = nIdx;
                    break;
                case ScriptCommandConst.SASSEMBLEALL:
                    NPC.FGotoLable[ScriptCommandConst.NASSEMBLEALL] = nIdx;
                    break;
                case ScriptCommandConst.SASSEMBLEWEAPON:
                    NPC.FGotoLable[ScriptCommandConst.NASSEMBLEWEAPON] = nIdx;
                    break;
                case ScriptCommandConst.SASSEMBLEDRESS:
                    NPC.FGotoLable[ScriptCommandConst.NASSEMBLEDRESS] = nIdx;
                    break;
                case ScriptCommandConst.SASSEMBLEHELMET:
                    NPC.FGotoLable[ScriptCommandConst.NASSEMBLEHELMET] = nIdx;
                    break;
                case ScriptCommandConst.SASSEMBLENECKLACE:
                    NPC.FGotoLable[ScriptCommandConst.NASSEMBLENECKLACE] = nIdx;
                    break;
                case ScriptCommandConst.SASSEMBLERING:
                    NPC.FGotoLable[ScriptCommandConst.NASSEMBLERING] = nIdx;
                    break;
                case ScriptCommandConst.SASSEMBLEARMRING:
                    NPC.FGotoLable[ScriptCommandConst.NASSEMBLEARMRING] = nIdx;
                    break;
                case ScriptCommandConst.SASSEMBLEBELT:
                    NPC.FGotoLable[ScriptCommandConst.NASSEMBLEBELT] = nIdx;
                    break;
                case ScriptCommandConst.SASSEMBLEBOOT:
                    NPC.FGotoLable[ScriptCommandConst.NASSEMBLEBOOT] = nIdx;
                    break;
                case ScriptCommandConst.SASSEMBLEFAIL:
                    NPC.FGotoLable[ScriptCommandConst.NASSEMBLEFAIL] = nIdx;
                    break;
                case ScriptCommandConst.SCREATEHEROFAILEX:
                    NPC.FGotoLable[ScriptCommandConst.NCREATEHEROFAILEX] = nIdx;// 创建英雄失败  By John 2012.08.04
                    break;
                case ScriptCommandConst.SLOGOUTHEROFIRST:
                    NPC.FGotoLable[ScriptCommandConst.NLOGOUTHEROFIRST] = nIdx;// 请将英雄设置下线  By John 2012.08.04
                    break;
                case ScriptCommandConst.SNOTHAVEHERO:
                    NPC.FGotoLable[ScriptCommandConst.NNOTHAVEHERO] = nIdx;// 没有英雄   By John 2012.08.04
                    break;
                case ScriptCommandConst.SHERONAMEFILTER:
                    NPC.FGotoLable[ScriptCommandConst.NHERONAMEFILTER] = nIdx;// 英雄名字中包含禁用字符   By John 2012.08.04
                    break;
                case ScriptCommandConst.SHAVEHERO:
                    NPC.FGotoLable[ScriptCommandConst.NHAVEHERO] = nIdx;// 有英雄    By John 2012.08.04
                    break;
                case ScriptCommandConst.SCREATEHEROOK:
                    NPC.FGotoLable[ScriptCommandConst.NCREATEHEROOK] = nIdx;// 创建英雄OK   By John 2012.08.04
                    break;
                case ScriptCommandConst.SHERONAMEEXISTS:
                    NPC.FGotoLable[ScriptCommandConst.NHERONAMEEXISTS] = nIdx;// 英雄名字已经存在  By John 2012.08.04
                    break;
                case ScriptCommandConst.SDELETEHEROOK:
                    NPC.FGotoLable[ScriptCommandConst.NDELETEHEROOK] = nIdx;// 删除英雄成功    By John 2012.08.04
                    break;
                case ScriptCommandConst.SDELETEHEROFAIL:
                    NPC.FGotoLable[ScriptCommandConst.NDELETEHEROFAIL] = nIdx;// 删除英雄失败    By John 2012.08.04
                    break;
                case ScriptCommandConst.SHEROOVERCHRCOUNT:
                    NPC.FGotoLable[ScriptCommandConst.NHEROOVERCHRCOUNT] = nIdx;// 你的帐号角色过多   By John 2012.08.04
                    break;
                default:
                    //if (NPC.m_btNPCRaceServer == DataConst.NPC_RC_FUNMERCHANT)
                    //{
                    //    TFunMerchant FunMerchant = (TFunMerchant)NPC;
                    //    if (HUtil32.CompareLStr(sLabel, SctiptDef.SSTDMODEFUNC, SctiptDef.SSTDMODEFUNC.Length))
                    //    {
                    //        sIdx = sLabel.Substring(SctiptDef.SSTDMODEFUNC.Length + 1 - 1, SctiptDef.SSTDMODEFUNC.Length);
                    //        nIndex = Convert.ToInt32(sIdx);
                    //        if (nIndex >= FunMerchant.FStdModeFunc.GetLowerBound(0) && nIndex <= FunMerchant.FStdModeFunc.Length)
                    //        {
                    //            FunMerchant.FStdModeFunc[nIndex] = nIdx;
                    //        }
                    //    }
                    //    else if (HUtil32.CompareLStr(sLabel, SctiptDef.SPLAYLEVELUPEX, SctiptDef.SPLAYLEVELUPEX.Length))
                    //    {
                    //        sIdx = sLabel.Substring(SctiptDef.SPLAYLEVELUPEX.Length + 1 - 1, SctiptDef.SPLAYLEVELUPEX.Length);
                    //        nIndex = Convert.ToInt32(sIdx);
                    //        if (nIndex >= FunMerchant.FPlayLevelUp.GetLowerBound(0) && nIndex <= FunMerchant.FPlayLevelUp.Length)
                    //        {
                    //            FunMerchant.FPlayLevelUp[nIndex] = nIdx;
                    //        }
                    //    }
                    //    else if (HUtil32.CompareLStr(sLabel, SctiptDef.SUSERCMD, SctiptDef.SUSERCMD.Length))
                    //    {
                    //        sIdx = sLabel.Substring(SctiptDef.SUSERCMD.Length + 1 - 1, SctiptDef.SUSERCMD.Length);
                    //        nIndex = Convert.ToInt32(sIdx);
                    //        if (nIndex >= FunMerchant.FUserCmd.GetLowerBound(0) && nIndex <= FunMerchant.FUserCmd.Length)
                    //        {
                    //            FunMerchant.FUserCmd[nIndex] = nIdx;
                    //        }
                    //    }
                    //    else if (HUtil32.CompareLStr(sLabel, SctiptDef.SCLEARMISSION, SctiptDef.SCLEARMISSION.Length))
                    //    {
                    //        sIdx = sLabel.Substring(SctiptDef.SCLEARMISSION.Length + 1 - 1, SctiptDef.SCLEARMISSION.Length);
                    //        nIndex = Convert.ToInt32(sIdx);
                    //        if (nIndex >= FunMerchant.FClearMission.GetLowerBound(0) && nIndex <= FunMerchant.FClearMission.Length)
                    //        {
                    //            FunMerchant.FClearMission[nIndex] = nIdx;
                    //        }
                    //    }
                    //    else if (HUtil32.CompareLStr(sLabel, SctiptDef.SMAGSELFFUNC, SctiptDef.SMAGSELFFUNC.Length))
                    //    {
                    //        sIdx = sLabel.Substring(SctiptDef.SMAGSELFFUNC.Length + 1 - 1, SctiptDef.SMAGSELFFUNC.Length);
                    //        nIndex = Convert.ToInt32(sIdx);
                    //        if (nIndex >= FunMerchant.FMagSelfFunc.GetLowerBound(0) && nIndex <= FunMerchant.FMagSelfFunc.Length)
                    //        {
                    //            FunMerchant.FMagSelfFunc[nIndex] = nIdx;
                    //        }
                    //    }
                    //    else if (HUtil32.CompareLStr(sLabel, SctiptDef.SMAGTAGFUNC, SctiptDef.SMAGTAGFUNC.Length))
                    //    {
                    //        sIdx = sLabel.Substring(SctiptDef.SMAGTAGFUNC.Length + 1 - 1, SctiptDef.SMAGTAGFUNC.Length);
                    //        nIndex = Convert.ToInt32(sIdx);
                    //        if (nIndex >= FunMerchant.FMagTagFunc.GetLowerBound(0) && nIndex <= FunMerchant.FMagTagFunc.Length)
                    //        {
                    //            FunMerchant.FMagTagFunc[nIndex] = nIdx;
                    //        }
                    //    }
                    //    else if (HUtil32.CompareLStr(sLabel, SctiptDef.SMAGTAGFUNCEX, SctiptDef.SMAGTAGFUNCEX.Length))
                    //    {
                    //        sIdx = sLabel.Substring(SctiptDef.SMAGTAGFUNCEX.Length + 1 - 1, SctiptDef.SMAGTAGFUNCEX.Length);
                    //        nIndex = Convert.ToInt32(sIdx);
                    //        if (nIndex >= FunMerchant.FMagTagFuncEx.GetLowerBound(0) && nIndex <= FunMerchant.FMagTagFuncEx.Length)
                    //        {
                    //            FunMerchant.FMagTagFuncEx[nIndex] = nIdx;
                    //        }
                    //    }
                    //    else if (HUtil32.CompareLStr(sLabel, SctiptDef.SMAGMONFUNC, SctiptDef.SMAGMONFUNC.Length))
                    //    {
                    //        sIdx = sLabel.Substring(SctiptDef.SMAGMONFUNC.Length + 1 - 1, SctiptDef.SMAGMONFUNC.Length);
                    //        nIndex = Convert.ToInt32(sIdx);
                    //        if (nIndex >= FunMerchant.FMagMonFunc.GetLowerBound(0) && nIndex <= FunMerchant.FMagMonFunc.Length)
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
                                sMsg = ScriptCommandConst.RESETLABEL + sMsg;
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
            if (sLabel2 == ScriptCommandConst.sVAR_SERVERNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_SERVERNAME);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_SERVERIP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_SERVERIP);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_WEBSITE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_WEBSITE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_BBSSITE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_BBSSITE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CLIENTDOWNLOAD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CLIENTDOWNLOAD);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_QQ)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_QQ);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_PHONE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_PHONE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_BANKACCOUNT0)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_BANKACCOUNT0);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_BANKACCOUNT1)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_BANKACCOUNT1);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_BANKACCOUNT2)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_BANKACCOUNT2);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_BANKACCOUNT3)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_BANKACCOUNT3);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_BANKACCOUNT4)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_BANKACCOUNT4);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_BANKACCOUNT5)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_BANKACCOUNT5);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_BANKACCOUNT6)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_BANKACCOUNT6);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_BANKACCOUNT7)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_BANKACCOUNT7);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_BANKACCOUNT8)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_BANKACCOUNT8);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_BANKACCOUNT9)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_BANKACCOUNT9);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_GAMEGOLDNAME)
            {
                //sMsg = sMsg.Replace("<" + sLabel + ">", Grobal2.sSTRING_GAMEGOLD);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_GAMEPOINTNAME)
            {
                // sMsg = sMsg.Replace("<" + sLabel + ">", Grobal2.sSTRING_GAMEPOINT);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_USERCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_USERCOUNT);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_DATETIME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_DATETIME);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_USERNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_USERNAME);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_FBMAPNAME)
            { //副本
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_FBMAPNAME);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_FBMAP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_FBMAP);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_ACCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_ACCOUNT);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_ASSEMBLEITEMNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_ASSEMBLEITEMNAME);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_MAPNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_MAPNAME);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_GUILDNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_GUILDNAME);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_RANKNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_RANKNAME);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_LEVEL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_LEVEL);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_HP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_HP);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_MAXHP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_MAXHP);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_MP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_MP);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_MAXMP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_MAXMP);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_AC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_AC);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_MAXAC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_MAXAC);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_MAC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_MAC);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_MAXMAC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_MAXMAC);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_DC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_DC);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_MAXDC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_MAXDC);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_MC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_MC);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_MAXMC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_MAXMC);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_SC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_SC);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_MAXSC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_MAXSC);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_EXP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_EXP);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_MAXEXP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_MAXEXP);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_PKPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_PKPOINT);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CREDITPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CREDITPOINT);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_GOLDCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_GOLDCOUNT);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_GAMEGOLD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_GAMEGOLD);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_GAMEPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_GAMEPOINT);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_LOGINTIME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_LOGINTIME);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_LOGINLONG)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_LOGINLONG);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_DRESS)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_DRESS);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_WEAPON)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_WEAPON);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_RIGHTHAND)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_RIGHTHAND);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_HELMET)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_HELMET);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_NECKLACE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_NECKLACE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_RING_R)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_RING_R);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_RING_L)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_RING_L);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_ARMRING_R)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_ARMRING_R);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_ARMRING_L)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_ARMRING_L);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_BUJUK)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_BUJUK);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_BELT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_BELT);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_BOOTS)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_BOOTS);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CHARM)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CHARM);
            }
            //=======================================没有用到的==============================
            else if (sLabel2 == ScriptCommandConst.sVAR_HOUSE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_HOUSE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CIMELIA)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CIMELIA);
            }
            //=======================================没有用到的==============================
            else if (sLabel2 == ScriptCommandConst.sVAR_IPADDR)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_IPADDR);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_IPLOCAL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_IPLOCAL);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_GUILDBUILDPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_GUILDBUILDPOINT);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_GUILDAURAEPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_GUILDAURAEPOINT);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_GUILDSTABILITYPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_GUILDSTABILITYPOINT);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_GUILDFLOURISHPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_GUILDFLOURISHPOINT);
            }
            //=================================没用用到的====================================
            else if (sLabel2 == ScriptCommandConst.sVAR_GUILDMONEYCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_GUILDMONEYCOUNT);
            }
            //=================================没用用到的结束====================================
            else if (sLabel2 == ScriptCommandConst.sVAR_REQUESTCASTLEWARITEM)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_REQUESTCASTLEWARITEM);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_REQUESTCASTLEWARDAY)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_REQUESTCASTLEWARDAY);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_REQUESTBUILDGUILDITEM)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_REQUESTBUILDGUILDITEM);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_OWNERGUILD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_OWNERGUILD);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CASTLENAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CASTLENAME);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_LORD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_LORD);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_GUILDWARFEE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_GUILDWARFEE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_BUILDGUILDFEE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_BUILDGUILDFEE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CASTLEWARDATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CASTLEWARDATE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_LISTOFWAR)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_LISTOFWAR);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CASTLECHANGEDATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CASTLECHANGEDATE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CASTLEWARLASTDATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CASTLEWARLASTDATE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CASTLEGETDAYS)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CASTLEGETDAYS);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_DATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_DATE);
            }
            //===================================没用用到的======================================
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_PRVMSG)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_PRVMSG);
            }
            //===================================没用用到的结束======================================
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_ALLOWMSG)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_ALLOWMSG);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_LETSHOUT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_LETSHOUT);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_LETTRADE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_LETTRADE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_LETGuild)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_LETGuild);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_ENDGUILD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_ENDGUILD);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_BANGUILDCHAT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_BANGUILDCHAT);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_AUTHALLY)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_AUTHALLY);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_AUTH)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_AUTH);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_AUTHCANCEL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_AUTHCANCEL);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_USERMOVE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_USERMOVE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_SEARCHING)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_SEARCHING);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_ALLOWGROUPCALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_ALLOWGROUPCALL);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_GROUPRECALLL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_GROUPRECALLL);
            }
            #region 没有使用的
            //===========================================没有使用的========================================
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_ALLOWGUILDRECALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_ALLOWGUILDRECALL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLOWGUILDRECALL, SctiptDef.sVAR_CMD_ALLOWGUILDRECALL);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_GUILDRECALLL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_GUILDRECALLL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_GUILDRECALLL, SctiptDef.sVAR_CMD_GUILDRECALLL);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_DEAR)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_DEAR);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_DEAR, SctiptDef.sVAR_CMD_DEAR);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_ALLOWDEARRCALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_ALLOWDEARRCALL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLOWDEARRCALL, SctiptDef.sVAR_CMD_ALLOWDEARRCALL);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_DEARRECALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_DEARRECALL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_DEARRECALL, SctiptDef.sVAR_CMD_DEARRECALL);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_MASTER)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_MASTER);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_MASTER, SctiptDef.sVAR_CMD_MASTER);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_ALLOWMASTERRECALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_ALLOWMASTERRECALL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLOWMASTERRECALL, SctiptDef.sVAR_CMD_ALLOWMASTERRECALL);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_MASTERECALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_MASTERECALL);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_MASTERECALL, SctiptDef.sVAR_CMD_MASTERECALL);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_TAKEONHORSE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_TAKEONHORSE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_TAKEONHORSE, SctiptDef.sVAR_CMD_TAKEONHORSE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_TAKEOFHORSE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_TAKEOFHORSE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_TAKEOFHORSE, SctiptDef.sVAR_CMD_TAKEOFHORSE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_ALLSYSMSG)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_ALLSYSMSG);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLSYSMSG, SctiptDef.sVAR_CMD_ALLSYSMSG);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_MEMBERFUNCTION)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_MEMBERFUNCTION);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_MEMBERFUNCTION, SctiptDef.sVAR_CMD_MEMBERFUNCTION);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_MEMBERFUNCTIONEX)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_MEMBERFUNCTIONEX);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_MEMBERFUNCTIONEX, SctiptDef.sVAR_CMD_MEMBERFUNCTIONEX);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CASTLEGOLD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CASTLEGOLD);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CASTLEGOLD, SctiptDef.sVAR_CASTLEGOLD);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_TODAYINCOME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_TODAYINCOME);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_TODAYINCOME, SctiptDef.sVAR_TODAYINCOME);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CASTLEDOORSTATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CASTLEDOORSTATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CASTLEDOORSTATE, SctiptDef.sVAR_CASTLEDOORSTATE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_REPAIRDOORGOLD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_REPAIRDOORGOLD);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_REPAIRDOORGOLD, SctiptDef.sVAR_REPAIRDOORGOLD);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_REPAIRWALLGOLD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_REPAIRWALLGOLD);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_REPAIRWALLGOLD, SctiptDef.sVAR_REPAIRWALLGOLD);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_GUARDFEE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_GUARDFEE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_GUARDFEE, SctiptDef.sVAR_GUARDFEE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_ARCHERFEE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_ARCHERFEE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_ARCHERFEE, SctiptDef.sVAR_ARCHERFEE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_GUARDRULE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_GUARDRULE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_GUARDRULE, SctiptDef.sVAR_GUARDRULE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_STORAGE2STATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_STORAGE2STATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_STORAGE2STATE, SctiptDef.sVAR_STORAGE2STATE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_STORAGE3STATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_STORAGE3STATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_STORAGE3STATE, SctiptDef.sVAR_STORAGE3STATE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_STORAGE4STATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_STORAGE4STATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_STORAGE4STATE, SctiptDef.sVAR_STORAGE4STATE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_STORAGE5STATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_STORAGE5STATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_STORAGE5STATE, SctiptDef.sVAR_STORAGE5STATE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_SELFNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_SELFNAME);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_SELFNAME, SctiptDef.sVAR_SELFNAME);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_POSENAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_POSENAME);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_POSENAME, SctiptDef.sVAR_POSENAME);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_GAMEDIAMOND)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_GAMEDIAMOND);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_GAMEDIAMOND, SctiptDef.sVAR_GAMEDIAMOND);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_GAMEGIRD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_GAMEGIRD);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_GAMEGIRD, SctiptDef.sVAR_GAMEGIRD);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_ALLOWFIREND)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_ALLOWFIREND);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLOWFIREND, SctiptDef.sVAR_CMD_ALLOWFIREND);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_EFFIGYSTATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_EFFIGYSTATE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_EFFIGYSTATE, SctiptDef.sVAR_EFFIGYSTATE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_EFFIGYOFFSET)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_EFFIGYOFFSET);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_EFFIGYOFFSET, SctiptDef.sVAR_EFFIGYOFFSET);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_YEAR)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_YEAR);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_YEAR, SctiptDef.sVAR_YEAR);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_MONTH)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_MONTH);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_MONTH, SctiptDef.sVAR_MONTH);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_DAY)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_DAY);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_DAY, SctiptDef.sVAR_DAY);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_HOUR)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_HOUR);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_HOUR, SctiptDef.sVAR_HOUR);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_MINUTE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_MINUTE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_MINUTE, SctiptDef.sVAR_MINUTE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_SEC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_SEC);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_SEC, SctiptDef.sVAR_SEC);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_MAP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_MAP);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_MAP, SctiptDef.sVAR_MAP);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_X)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_X);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_X, SctiptDef.sVAR_X);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_Y)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_Y);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_Y, SctiptDef.sVAR_Y);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_UNMASTER_FORCE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_UNMASTER_FORCE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_UNMASTER_FORCE, SctiptDef.sVAR_UNMASTER_FORCE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_USERGOLDCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_USERGOLDCOUNT);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_USERGOLDCOUNT, SctiptDef.sVAR_USERGOLDCOUNT);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_MAXGOLDCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_MAXGOLDCOUNT);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_MAXGOLDCOUNT, SctiptDef.sVAR_MAXGOLDCOUNT);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_STORAGEGOLDCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_STORAGEGOLDCOUNT);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_STORAGEGOLDCOUNT, SctiptDef.sVAR_STORAGEGOLDCOUNT);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_BINDGOLDCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_BINDGOLDCOUNT);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_BINDGOLDCOUNT, SctiptDef.sVAR_BINDGOLDCOUNT);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_UPGRADEWEAPONFEE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_UPGRADEWEAPONFEE);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_UPGRADEWEAPONFEE, SctiptDef.sVAR_UPGRADEWEAPONFEE);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_USERWEAPON)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_USERWEAPON);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_USERWEAPON, SctiptDef.sVAR_USERWEAPON);
            }
            else if (sLabel2 == ScriptCommandConst.sVAR_CMD_STARTQUEST)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", ScriptCommandConst.tVAR_CMD_STARTQUEST);
                //SctiptDef.g_GrobalManage.Add(SctiptDef.nVAR_CMD_STARTQUEST, SctiptDef.sVAR_CMD_STARTQUEST);
            }
            //===========================================没有使用的========================================
            #endregion
            else if (HUtil32.CompareLStr(sLabel2, ScriptCommandConst.sVAR_TEAM, ScriptCommandConst.sVAR_TEAM.Length))
            {
                s14 = sLabel2.Substring(ScriptCommandConst.sVAR_TEAM.Length + 1 - 1, 1);
                if (s14 != "")
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(ScriptCommandConst.tVAR_TEAM, s14));
                }
                else
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, ScriptCommandConst.sVAR_HUMAN, ScriptCommandConst.sVAR_HUMAN.Length))
            {
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (s14 != "")
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(ScriptCommandConst.tVAR_HUMAN, s14));
                }
                else
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, ScriptCommandConst.sVAR_GUILD, ScriptCommandConst.sVAR_GUILD.Length))
            {
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (s14 != "")
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(ScriptCommandConst.tVAR_GUILD, s14));
                }
                else
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, ScriptCommandConst.sVAR_GLOBAL, ScriptCommandConst.sVAR_GLOBAL.Length))
            {
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (s14 != "")
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(ScriptCommandConst.tVAR_GLOBAL, s14));
                }
                else
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, ScriptCommandConst.sVAR_STR, ScriptCommandConst.sVAR_STR.Length))
            {
                //'欢迎使用个人银行储蓄，目前完全免费，请多利用。\ \<您的个人银行存款有/@-1>：<$46><｜/@-2><$125/G18>\ \<您的包裹里以携带有/AUTOCOLOR=249>：<$GOLDCOUNT><｜/@-2><$GOLDCOUNTX>\ \ \<存入金币/@@InPutInteger1>      <取出金币/@@InPutInteger2>      <返 回/@Main>'
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (s14 != "")
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(ScriptCommandConst.tVAR_STR, s14));
                }
                else
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, ScriptCommandConst.sVAR_MISSIONARITHMOMETER, ScriptCommandConst.sVAR_MISSIONARITHMOMETER.Length))
            {
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (s14 != "")
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(ScriptCommandConst.tVAR_MISSIONARITHMOMETER, s14));
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