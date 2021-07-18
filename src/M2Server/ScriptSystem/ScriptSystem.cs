using mSystemModule;
using System;
using System.Collections.Generic;
using System.IO;

namespace M2Server
{
    public class ScriptSystem
    {
        public int LoadNpcScript(TNormNpc NPC, string sPatch, string sScritpName)
        {
            int result;
            if (sPatch == "")
            {
                sPatch = M2Share.sNpc_def;
            }
            result = LoadScriptFile(NPC, sPatch, sScritpName, false);
            return result;
        }

        public bool LoadScriptFile_LoadCallScript(string sFileName, string sLabel, StringList List)
        {
            bool result;
            int I;
            StringList LoadStrList;
            bool bo1D;
            string s18;
            result = false;
            if (File.Exists(sFileName))
            {
                LoadStrList = new StringList();
                LoadStrList.LoadFromFile(sFileName);
                DeCodeStringList(LoadStrList);
                sLabel = '[' + sLabel + ']';
                bo1D = false;
                for (I = 0; I < LoadStrList.Count; I++)
                {
                    s18 = LoadStrList[I].Trim();
                    if (s18 != "")
                    {
                        if (!bo1D)
                        {
                            if (s18[0] == '[' && s18.ToLower().CompareTo(sLabel.ToLower()) == 0)
                            {
                                bo1D = true;
                                List.Add(s18);
                            }
                        }
                        else
                        {

                            if (s18[0] != '{')
                            {
                                if (s18[0] == '}')
                                {
                                    bo1D = false;
                                    result = true;
                                    break;
                                }
                                else
                                {

                                    List.Add(s18);
                                }
                            }
                        }
                    }

                }
                // for I := 0 to LoadStrList.Count - 1 do begin

                //LoadStrList.Free;
            }
            return result;
        }

        public void LoadScriptFile_LoadScriptcall(StringList LoadList)
        {
            var s14 = string.Empty;
            var s18 = string.Empty;
            var s1C = string.Empty;
            var s20 = string.Empty;
            var s34 = string.Empty;
            for (var i = 0; i < LoadList.Count; i++)
            {
                s14 = LoadList[i].Trim();
                if (s14 != "" && s14[0] == '#' && HUtil32.CompareLStr(s14, "#CALL", "#CALL".Length))
                {
                    s14 = HUtil32.ArrestStringEx(s14, '[', ']', ref s1C);
                    s20 = s1C.Trim();
                    s18 = s14.Trim();
                    s34 = M2Share.g_Config.sEnvirDir + "QuestDiary\\" + s20;
                    if (LoadScriptFile_LoadCallScript(s34, s18, LoadList))
                    {
                        LoadList[i] = "#ACT";
                        LoadList.InsertText(i + 1, "goto " + s18);
                    }
                    else
                    {
                        M2Share.MainOutMessage("script error, load fail: " + s20 + s18);
                    }
                }
            }
        }

        public string LoadScriptFile_LoadDefineInfo(StringList LoadList, IList<TDefineInfo> List)
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
                        result = HUtil32.GetValidStr3(s14, ref s1C, new string[] { " ", "\t" }).Trim();
                        LoadList[i] = "";
                    }
                    if (HUtil32.CompareLStr(s14, "#DEFINE", "#DEFINE".Length))
                    {
                        s14 = HUtil32.GetValidStr3(s14, ref s1C, new string[] { " ", "\t" });
                        s14 = HUtil32.GetValidStr3(s14, ref s20, new string[] { " ", "\t" });
                        s14 = HUtil32.GetValidStr3(s14, ref s24, new string[] { " ", "\t" });
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
                        s28 = HUtil32.GetValidStr3(s14, ref s1C, new string[] { " ", "\t" }).Trim();
                        s28 = M2Share.g_Config.sEnvirDir + "Defines\\" + s28;
                        if (File.Exists(s28))
                        {
                            LoadStrList = new StringList();
                            LoadStrList.LoadFromFile(s28);
                            result = LoadScriptFile_LoadDefineInfo(LoadStrList, List);
                            //LoadStrList.Free;
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

        public TScript LoadScriptFile_MakeNewScript(TNormNpc NPC)
        {
            TScript result;
            TScript ScriptInfo;
            ScriptInfo = new TScript
            {
                boQuest = false,
                //FillChar(ScriptInfo.QuestInfo, sizeof(grobal2.TScriptQuestInfo), '\0');
                //nQuestIdx = 0;
                RecordList = new Dictionary<string, TSayingRecord>()
            };
            NPC.m_ScriptList.Add(ScriptInfo);
            result = ScriptInfo;
            return result;
        }

        public bool LoadScriptFile_QuestCondition(string sText, TQuestConditionInfo QuestConditionInfo)
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
            sText = HUtil32.GetValidStrCap(sText, ref sCmd, new string[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam1, new string[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam2, new string[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam3, new string[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam4, new string[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam5, new string[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam6, new string[] { " ", "\t" });
            sCmd = sCmd.ToUpper();
            if (sCmd == M2Share.sCHECK)
            {
                nCMDCode = M2Share.nCHECK;
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
            if (sCmd == M2Share.sCHECKOPEN)
            {
                nCMDCode = M2Share.nCHECKOPEN;
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
            if (sCmd == M2Share.sCHECKUNIT)
            {
                nCMDCode = M2Share.nCHECKUNIT;
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
            if (sCmd == M2Share.sCHECKPKPOINT)
            {
                nCMDCode = M2Share.nCHECKPKPOINT;
                goto L001;
            }
            if (sCmd == M2Share.sCHECKGOLD)
            {
                nCMDCode = M2Share.nCHECKGOLD;
                goto L001;
            }
            if (sCmd == M2Share.sCHECKLEVEL)
            {
                nCMDCode = M2Share.nCHECKLEVEL;
                goto L001;
            }
            if (sCmd == M2Share.sCHECKJOB)
            {
                nCMDCode = M2Share.nCHECKJOB;
                goto L001;
            }
            if (sCmd == M2Share.sRANDOM)
            {

                nCMDCode = M2Share.nRANDOM;
                goto L001;
            }
            if (sCmd == M2Share.sCHECKITEM)
            {
                nCMDCode = M2Share.nCHECKITEM;
                goto L001;
            }
            if (sCmd == M2Share.sGENDER)
            {
                nCMDCode = M2Share.nGENDER;
                goto L001;
            }
            if (sCmd == M2Share.sCHECKBAGGAGE)
            {
                nCMDCode = M2Share.nCHECKBAGGAGE;
                goto L001;
            }
            if (sCmd == M2Share.sCHECKNAMELIST)
            {
                nCMDCode = M2Share.nCHECKNAMELIST;
                goto L001;
            }
            if (sCmd == M2Share.sSC_HASGUILD)
            {
                nCMDCode = M2Share.nSC_HASGUILD;
                goto L001;
            }
            if (sCmd == M2Share.sSC_ISGUILDMASTER)
            {
                nCMDCode = M2Share.nSC_ISGUILDMASTER;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKCASTLEMASTER)
            {
                nCMDCode = M2Share.nSC_CHECKCASTLEMASTER;
                goto L001;
            }
            if (sCmd == M2Share.sSC_ISNEWHUMAN)
            {
                nCMDCode = M2Share.nSC_ISNEWHUMAN;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKMEMBERTYPE)
            {
                nCMDCode = M2Share.nSC_CHECKMEMBERTYPE;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKMEMBERLEVEL)
            {
                nCMDCode = M2Share.nSC_CHECKMEMBERLEVEL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKGAMEGOLD)
            {
                nCMDCode = M2Share.nSC_CHECKGAMEGOLD;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKGAMEPOINT)
            {
                nCMDCode = M2Share.nSC_CHECKGAMEPOINT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKNAMELISTPOSITION)
            {
                nCMDCode = M2Share.nSC_CHECKNAMELISTPOSITION;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKGUILDLIST)
            {
                nCMDCode = M2Share.nSC_CHECKGUILDLIST;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKRENEWLEVEL)
            {
                nCMDCode = M2Share.nSC_CHECKRENEWLEVEL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKSLAVELEVEL)
            {
                nCMDCode = M2Share.nSC_CHECKSLAVELEVEL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKSLAVENAME)
            {
                nCMDCode = M2Share.nSC_CHECKSLAVENAME;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKCREDITPOINT)
            {
                nCMDCode = M2Share.nSC_CHECKCREDITPOINT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKOFGUILD)
            {
                nCMDCode = M2Share.nSC_CHECKOFGUILD;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKPAYMENT)
            {
                nCMDCode = M2Share.nSC_CHECKPAYMENT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKUSEITEM)
            {
                nCMDCode = M2Share.nSC_CHECKUSEITEM;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKBAGSIZE)
            {
                nCMDCode = M2Share.nSC_CHECKBAGSIZE;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKLISTCOUNT)
            {
                nCMDCode = M2Share.nSC_CHECKLISTCOUNT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKDC)
            {
                nCMDCode = M2Share.nSC_CHECKDC;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKMC)
            {
                nCMDCode = M2Share.nSC_CHECKMC;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKSC)
            {
                nCMDCode = M2Share.nSC_CHECKSC;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKHP)
            {
                nCMDCode = M2Share.nSC_CHECKHP;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKMP)
            {
                nCMDCode = M2Share.nSC_CHECKMP;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKITEMTYPE)
            {
                nCMDCode = M2Share.nSC_CHECKITEMTYPE;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKEXP)
            {
                nCMDCode = M2Share.nSC_CHECKEXP;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKCASTLEGOLD)
            {
                nCMDCode = M2Share.nSC_CHECKCASTLEGOLD;
                goto L001;
            }
            if (sCmd == M2Share.sSC_PASSWORDERRORCOUNT)
            {
                nCMDCode = M2Share.nSC_PASSWORDERRORCOUNT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_ISLOCKPASSWORD)
            {
                nCMDCode = M2Share.nSC_ISLOCKPASSWORD;
                goto L001;
            }
            if (sCmd == M2Share.sSC_ISLOCKSTORAGE)
            {
                nCMDCode = M2Share.nSC_ISLOCKSTORAGE;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKBUILDPOINT)
            {
                nCMDCode = M2Share.nSC_CHECKBUILDPOINT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKAURAEPOINT)
            {
                nCMDCode = M2Share.nSC_CHECKAURAEPOINT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKSTABILITYPOINT)
            {
                nCMDCode = M2Share.nSC_CHECKSTABILITYPOINT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKFLOURISHPOINT)
            {
                nCMDCode = M2Share.nSC_CHECKFLOURISHPOINT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKCONTRIBUTION)
            {
                nCMDCode = M2Share.nSC_CHECKCONTRIBUTION;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKRANGEMONCOUNT)
            {
                nCMDCode = M2Share.nSC_CHECKRANGEMONCOUNT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKITEMADDVALUE)
            {
                nCMDCode = M2Share.nSC_CHECKITEMADDVALUE;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKINMAPRANGE)
            {
                nCMDCode = M2Share.nSC_CHECKINMAPRANGE;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CASTLECHANGEDAY)
            {
                nCMDCode = M2Share.nSC_CASTLECHANGEDAY;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CASTLEWARDAY)
            {
                nCMDCode = M2Share.nSC_CASTLEWARDAY;
                goto L001;
            }
            if (sCmd == M2Share.sSC_ONLINELONGMIN)
            {
                nCMDCode = M2Share.nSC_ONLINELONGMIN;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKGUILDCHIEFITEMCOUNT)
            {
                nCMDCode = M2Share.nSC_CHECKGUILDCHIEFITEMCOUNT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKNAMEDATELIST)
            {
                nCMDCode = M2Share.nSC_CHECKNAMEDATELIST;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKMAPHUMANCOUNT)
            {
                nCMDCode = M2Share.nSC_CHECKMAPHUMANCOUNT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKMAPMONCOUNT)
            {
                nCMDCode = M2Share.nSC_CHECKMAPMONCOUNT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKVAR)
            {
                nCMDCode = M2Share.nSC_CHECKVAR;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKSERVERNAME)
            {
                nCMDCode = M2Share.nSC_CHECKSERVERNAME;
                goto L001;
            }
            if (sCmd == M2Share.sSC_ISATTACKGUILD)
            {
                nCMDCode = M2Share.nSC_ISATTACKGUILD;
                goto L001;
            }
            if (sCmd == M2Share.sSC_ISDEFENSEGUILD)
            {
                nCMDCode = M2Share.nSC_ISDEFENSEGUILD;
                goto L001;
            }
            if (sCmd == M2Share.sSC_ISATTACKALLYGUILD)
            {
                nCMDCode = M2Share.nSC_ISATTACKALLYGUILD;
                goto L001;
            }
            if (sCmd == M2Share.sSC_ISDEFENSEALLYGUILD)
            {
                nCMDCode = M2Share.nSC_ISDEFENSEALLYGUILD;
                goto L001;
            }
            if (sCmd == M2Share.sSC_ISCASTLEGUILD)
            {
                nCMDCode = M2Share.nSC_ISCASTLEGUILD;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKCASTLEDOOR)
            {
                nCMDCode = M2Share.nSC_CHECKCASTLEDOOR;
                goto L001;
            }
            if (sCmd == M2Share.sSC_ISSYSOP)
            {
                nCMDCode = M2Share.nSC_ISSYSOP;
                goto L001;
            }
            if (sCmd == M2Share.sSC_ISADMIN)
            {
                nCMDCode = M2Share.nSC_ISADMIN;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKGROUPCOUNT)
            {
                nCMDCode = M2Share.nSC_CHECKGROUPCOUNT;
                goto L001;
            }
            if (sCmd == M2Share.sCHECKACCOUNTLIST)
            {
                nCMDCode = M2Share.nCHECKACCOUNTLIST;
                goto L001;
            }
            if (sCmd == M2Share.sCHECKIPLIST)
            {
                nCMDCode = M2Share.nCHECKIPLIST;
                goto L001;
            }
            if (sCmd == M2Share.sCHECKBBCOUNT)
            {
                nCMDCode = M2Share.nCHECKBBCOUNT;
                goto L001;
            }
            if (sCmd == M2Share.sCHECKCREDITPOINT)
            {
                nCMDCode = M2Share.nCHECKCREDITPOINT;
                goto L001;
            }
            if (sCmd == M2Share.sDAYTIME)
            {
                nCMDCode = M2Share.nDAYTIME;
                goto L001;
            }
            if (sCmd == M2Share.sCHECKITEMW)
            {
                nCMDCode = M2Share.nCHECKITEMW;
                goto L001;
            }
            if (sCmd == M2Share.sISTAKEITEM)
            {
                nCMDCode = M2Share.nISTAKEITEM;
                goto L001;
            }
            if (sCmd == M2Share.sCHECKDURA)
            {
                nCMDCode = M2Share.nCHECKDURA;
                goto L001;
            }
            if (sCmd == M2Share.sCHECKDURAEVA)
            {
                nCMDCode = M2Share.nCHECKDURAEVA;
                goto L001;
            }
            if (sCmd == M2Share.sDAYOFWEEK)
            {
                nCMDCode = M2Share.nDAYOFWEEK;
                goto L001;
            }
            if (sCmd == M2Share.sHOUR)
            {
                nCMDCode = M2Share.nHOUR;
                goto L001;
            }
            if (sCmd == M2Share.sMIN)
            {
                nCMDCode = M2Share.nMIN;
                goto L001;
            }
            if (sCmd == M2Share.sCHECKLUCKYPOINT)
            {
                nCMDCode = M2Share.nCHECKLUCKYPOINT;
                goto L001;
            }
            if (sCmd == M2Share.sCHECKMONMAP)
            {
                nCMDCode = M2Share.nCHECKMONMAP;
                goto L001;
            }
            if (sCmd == M2Share.sCHECKMONAREA)
            {
                nCMDCode = M2Share.nCHECKMONAREA;
                goto L001;
            }
            if (sCmd == M2Share.sCHECKHUM)
            {
                nCMDCode = M2Share.nCHECKHUM;
                goto L001;
            }
            if (sCmd == M2Share.sEQUAL)
            {
                nCMDCode = M2Share.nEQUAL;
                goto L001;
            }
            if (sCmd == M2Share.sLARGE)
            {
                nCMDCode = M2Share.nLARGE;
                goto L001;
            }
            if (sCmd == M2Share.sSMALL)
            {
                nCMDCode = M2Share.nSMALL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKPOSEDIR)
            {
                nCMDCode = M2Share.nSC_CHECKPOSEDIR;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKPOSELEVEL)
            {
                nCMDCode = M2Share.nSC_CHECKPOSELEVEL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKPOSEGENDER)
            {
                nCMDCode = M2Share.nSC_CHECKPOSEGENDER;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKLEVELEX)
            {
                nCMDCode = M2Share.nSC_CHECKLEVELEX;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKBONUSPOINT)
            {
                nCMDCode = M2Share.nSC_CHECKBONUSPOINT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKMARRY)
            {
                nCMDCode = M2Share.nSC_CHECKMARRY;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKPOSEMARRY)
            {
                nCMDCode = M2Share.nSC_CHECKPOSEMARRY;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKMARRYCOUNT)
            {
                nCMDCode = M2Share.nSC_CHECKMARRYCOUNT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKMASTER)
            {
                nCMDCode = M2Share.nSC_CHECKMASTER;
                goto L001;
            }
            if (sCmd == M2Share.sSC_HAVEMASTER)
            {
                nCMDCode = M2Share.nSC_HAVEMASTER;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKPOSEMASTER)
            {
                nCMDCode = M2Share.nSC_CHECKPOSEMASTER;
                goto L001;
            }
            if (sCmd == M2Share.sSC_POSEHAVEMASTER)
            {
                nCMDCode = M2Share.nSC_POSEHAVEMASTER;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKISMASTER)
            {
                nCMDCode = M2Share.nSC_CHECKISMASTER;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKPOSEISMASTER)
            {
                nCMDCode = M2Share.nSC_CHECKPOSEISMASTER;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKNAMEIPLIST)
            {
                nCMDCode = M2Share.nSC_CHECKNAMEIPLIST;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKACCOUNTIPLIST)
            {
                nCMDCode = M2Share.nSC_CHECKACCOUNTIPLIST;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKSLAVECOUNT)
            {
                nCMDCode = M2Share.nSC_CHECKSLAVECOUNT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKPOS)
            {
                nCMDCode = M2Share.nSC_CHECKPOS;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKMAP)
            {
                nCMDCode = M2Share.nSC_CHECKMAP;
                goto L001;
            }
            if (sCmd == M2Share.sSC_REVIVESLAVE)
            {
                nCMDCode = M2Share.nSC_REVIVESLAVE;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKMAGICLVL)
            {
                nCMDCode = M2Share.nSC_CHECKMAGICLVL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKGROUPCLASS)
            {
                nCMDCode = M2Share.nSC_CHECKGROUPCLASS;
                goto L001;
            }
            if (sCmd == M2Share.sSC_ISGROUPMASTER)
            {
                nCMDCode = M2Share.nSC_ISGROUPMASTER;
                goto L001;
            }
            if (sCmd == M2Share.sCheckDiemon)
            {
                nCMDCode = M2Share.nCheckDiemon;
                goto L001;
            }
            if (sCmd == M2Share.scheckkillplaymon)
            {
                nCMDCode = M2Share.ncheckkillplaymon;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKRANDOMNO)
            {
                nCMDCode = M2Share.nSC_CHECKRANDOMNO;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKISONMAP)
            {
                nCMDCode = M2Share.nSC_CHECKISONMAP;
                goto L001;
            }
            // 检测是否安全区
            if (sCmd == M2Share.sSC_CHECKINSAFEZONE)
            {
                nCMDCode = M2Share.nSC_CHECKINSAFEZONE;
                goto L001;
            }
            if (sCmd == M2Share.sSC_KILLBYHUM)
            {
                nCMDCode = M2Share.nSC_KILLBYHUM;
                goto L001;
            }
            if (sCmd == M2Share.sSC_KILLBYMON)
            {
                nCMDCode = M2Share.nSC_KILLBYMON;
                goto L001;
            }
            // 增加挂机
            if (sCmd == M2Share.sSC_OffLine)
            {
                nCMDCode = M2Share.nSC_OffLine;
                goto L001;
            }
            // 增加脚本特修所有装备命令
            if (sCmd == M2Share.sSC_REPAIRALL)
            {
                nCMDCode = M2Share.nSC_REPAIRALL;
                goto L001;
            }
            // 刷新包裹物品命令
            if (sCmd == M2Share.sSC_QUERYBAGITEMS)
            {
                nCMDCode = M2Share.nSC_QUERYBAGITEMS;
                goto L001;
            }
            if (sCmd == M2Share.sSC_SETRANDOMNO)
            {
                nCMDCode = M2Share.nSC_SETRANDOMNO;
                goto L001;
            }
            if (sCmd == M2Share.sSC_DELAYGOTO || sCmd == "DELAYCALL")
            {
                nCMDCode = M2Share.nSC_DELAYGOTO;
                goto L001;
            }
            // ------------------------------
            L001:
            if (nCMDCode > 0)
            {
                QuestConditionInfo.nCmdCode = nCMDCode;
                if (sParam1 != "" && sParam1[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam1, '\"', '\"', ref sParam1);
                }
                if (sParam2 != "" && sParam2[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam2, '\"', '\"', ref sParam2);
                }
                if (sParam3 != "" && sParam3[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam3, '\"', '\"', ref sParam3);
                }
                if (sParam4 != "" && sParam4[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam4, '\"', '\"', ref sParam4);
                }
                if (sParam5 != "" && sParam5[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam5, '\"', '\"', ref sParam5);
                }
                if (sParam6 != "" && sParam6[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam6, '\"', '\"', ref sParam6);
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

        public bool LoadScriptFile_QuestAction(string sText, TQuestActionInfo QuestActionInfo)
        {
            bool result;
            var sCmd = string.Empty;
            var sParam1 = string.Empty;
            var sParam2 = string.Empty;
            var sParam3 = string.Empty;
            var sParam4 = string.Empty;
            var sParam5 = string.Empty;
            var sParam6 = string.Empty;
            int nCMDCode;
            result = false;
            sText = HUtil32.GetValidStrCap(sText, ref sCmd, new string[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam1, new string[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam2, new string[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam3, new string[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam4, new string[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam5, new string[] { " ", "\t" });
            sText = HUtil32.GetValidStrCap(sText, ref sParam6, new string[] { " ", "\t" });
            sCmd = sCmd.ToUpper();
            nCMDCode = 0;
            if (sCmd == M2Share.sSET)
            {
                nCMDCode = M2Share.nSET;
                HUtil32.ArrestStringEx(sParam1, '[', ']', ref sParam1);
                if (!HUtil32.IsStringNumber(sParam1))
                {
                    nCMDCode = 0;
                }
                if (!HUtil32.IsStringNumber(sParam2))
                {
                    nCMDCode = 0;
                }
            }
            if (sCmd == M2Share.sRESET)
            {
                nCMDCode = M2Share.nRESET;
                HUtil32.ArrestStringEx(sParam1, '[', ']', ref sParam1);
                if (!HUtil32.IsStringNumber(sParam1))
                {
                    nCMDCode = 0;
                }
                if (!HUtil32.IsStringNumber(sParam2))
                {
                    nCMDCode = 0;
                }
            }
            if (sCmd == M2Share.sSETOPEN)
            {
                nCMDCode = M2Share.nSETOPEN;
                HUtil32.ArrestStringEx(sParam1, '[', ']', ref sParam1);
                if (!HUtil32.IsStringNumber(sParam1))
                {
                    nCMDCode = 0;
                }
                if (!HUtil32.IsStringNumber(sParam2))
                {
                    nCMDCode = 0;
                }
            }
            if (sCmd == M2Share.sSETUNIT)
            {
                nCMDCode = M2Share.nSETUNIT;
                HUtil32.ArrestStringEx(sParam1, '[', ']', ref sParam1);
                if (!HUtil32.IsStringNumber(sParam1))
                {
                    nCMDCode = 0;
                }
                if (!HUtil32.IsStringNumber(sParam2))
                {
                    nCMDCode = 0;
                }
            }
            if (sCmd == M2Share.sRESETUNIT)
            {
                nCMDCode = M2Share.nRESETUNIT;
                HUtil32.ArrestStringEx(sParam1, '[', ']', ref sParam1);
                if (!HUtil32.IsStringNumber(sParam1))
                {
                    nCMDCode = 0;
                }
                if (!HUtil32.IsStringNumber(sParam2))
                {
                    nCMDCode = 0;
                }
            }
            if (sCmd == M2Share.sTAKE)
            {
                nCMDCode = M2Share.nTAKE;
                goto L001;
            }
            if (sCmd == M2Share.sSC_GIVE)
            {
                nCMDCode = M2Share.nSC_GIVE;
                goto L001;
            }
            if (sCmd == M2Share.sCLOSE)
            {
                nCMDCode = M2Share.nCLOSE;
                goto L001;
            }
            if (sCmd == M2Share.sBREAK)
            {
                nCMDCode = M2Share.nBREAK;
                goto L001;
            }
            if (sCmd == M2Share.sGOTO)
            {
                nCMDCode = M2Share.nGOTO;
                goto L001;
            }
            if (sCmd == M2Share.sADDNAMELIST)
            {
                nCMDCode = M2Share.nADDNAMELIST;
                goto L001;
            }
            if (sCmd == M2Share.sDELNAMELIST)
            {
                nCMDCode = M2Share.nDELNAMELIST;
                goto L001;
            }
            if (sCmd == M2Share.sADDGUILDLIST)
            {
                nCMDCode = M2Share.nADDGUILDLIST;
                goto L001;
            }
            if (sCmd == M2Share.sDELGUILDLIST)
            {
                nCMDCode = M2Share.nDELGUILDLIST;
                goto L001;
            }
            if (sCmd == M2Share.sSC_MAPTING)
            {
                nCMDCode = M2Share.nSC_MAPTING;
                goto L001;
            }
            if (sCmd == M2Share.sSC_LINEMSG)
            {
                nCMDCode = M2Share.nSC_LINEMSG;
                goto L001;
            }
            if (sCmd == M2Share.sADDACCOUNTLIST)
            {
                nCMDCode = M2Share.nADDACCOUNTLIST;
                goto L001;
            }
            if (sCmd == M2Share.sDELACCOUNTLIST)
            {
                nCMDCode = M2Share.nDELACCOUNTLIST;
                goto L001;
            }
            if (sCmd == M2Share.sADDIPLIST)
            {
                nCMDCode = M2Share.nADDIPLIST;
                goto L001;
            }
            if (sCmd == M2Share.sDELIPLIST)
            {
                nCMDCode = M2Share.nDELIPLIST;
                goto L001;
            }
            if (sCmd == M2Share.sSENDMSG)
            {
                nCMDCode = M2Share.nSENDMSG;
                goto L001;
            }
            if (sCmd == M2Share.sCHANGEMODE)
            {
                nCMDCode = M2Share.nCHANGEMODE;
                goto L001;
            }
            if (sCmd == M2Share.sPKPOINT)
            {
                nCMDCode = M2Share.nPKPOINT;
                goto L001;
            }
            if (sCmd == M2Share.sCHANGEXP)
            {
                nCMDCode = M2Share.nCHANGEXP;
                goto L001;
            }
            if (sCmd == M2Share.sSC_RECALLMOB)
            {
                nCMDCode = M2Share.nSC_RECALLMOB;
                goto L001;
            }
            if (sCmd == M2Share.sKICK)
            {
                nCMDCode = M2Share.nKICK;
                goto L001;
            }
            if (sCmd == M2Share.sTAKEW)
            {
                nCMDCode = M2Share.nTAKEW;
                goto L001;
            }
            if (sCmd == M2Share.sTIMERECALL)
            {
                nCMDCode = M2Share.nTIMERECALL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_PARAM1)
            {
                nCMDCode = M2Share.nSC_PARAM1;
                goto L001;
            }
            if (sCmd == M2Share.sSC_PARAM2)
            {
                nCMDCode = M2Share.nSC_PARAM2;
                goto L001;
            }
            if (sCmd == M2Share.sSC_PARAM3)
            {
                nCMDCode = M2Share.nSC_PARAM3;
                goto L001;
            }
            if (sCmd == M2Share.sSC_PARAM4)
            {
                nCMDCode = M2Share.nSC_PARAM4;
                goto L001;
            }
            if (sCmd == M2Share.sSC_EXEACTION)
            {
                nCMDCode = M2Share.nSC_EXEACTION;
                goto L001;
            }
            if (sCmd == M2Share.sMAPMOVE)
            {
                nCMDCode = M2Share.nMAPMOVE;
                goto L001;
            }
            if (sCmd == M2Share.sMAP)
            {
                nCMDCode = M2Share.nMAP;
                goto L001;
            }
            if (sCmd == M2Share.sTAKECHECKITEM)
            {
                nCMDCode = M2Share.nTAKECHECKITEM;
                goto L001;
            }
            if (sCmd == M2Share.sMONGEN)
            {
                nCMDCode = M2Share.nMONGEN;
                goto L001;
            }
            if (sCmd == M2Share.sMONCLEAR)
            {
                nCMDCode = M2Share.nMONCLEAR;
                goto L001;
            }
            if (sCmd == M2Share.sMOV)
            {
                nCMDCode = M2Share.nMOV;
                goto L001;
            }
            if (sCmd == M2Share.sINC)
            {
                nCMDCode = M2Share.nINC;
                goto L001;
            }
            if (sCmd == M2Share.sDEC)
            {
                nCMDCode = M2Share.nDEC;
                goto L001;
            }
            if (sCmd == M2Share.sSUM)
            {
                nCMDCode = M2Share.nSUM;
                goto L001;
            }
            if (sCmd == M2Share.sBREAKTIMERECALL)
            {
                nCMDCode = M2Share.nBREAKTIMERECALL;
                goto L001;
            }
            if (sCmd == M2Share.sMOVR)
            {
                nCMDCode = M2Share.nMOVR;
                goto L001;
            }
            if (sCmd == M2Share.sEXCHANGEMAP)
            {
                nCMDCode = M2Share.nEXCHANGEMAP;
                goto L001;
            }
            if (sCmd == M2Share.sRECALLMAP)
            {
                nCMDCode = M2Share.nRECALLMAP;
                goto L001;
            }
            if (sCmd == M2Share.sADDBATCH)
            {
                nCMDCode = M2Share.nADDBATCH;
                goto L001;
            }
            if (sCmd == M2Share.sBATCHDELAY)
            {
                nCMDCode = M2Share.nBATCHDELAY;
                goto L001;
            }
            if (sCmd == M2Share.sBATCHMOVE)
            {
                nCMDCode = M2Share.nBATCHMOVE;
                goto L001;
            }
            if (sCmd == M2Share.sPLAYDICE)
            {
                nCMDCode = M2Share.nPLAYDICE;
                goto L001;
            }
            if (sCmd == M2Share.sGOQUEST)
            {
                nCMDCode = M2Share.nGOQUEST;
                goto L001;
            }
            if (sCmd == M2Share.sENDQUEST)
            {
                nCMDCode = M2Share.nENDQUEST;
                goto L001;
            }
            if (sCmd == M2Share.sSC_HAIRCOLOR)
            {
                nCMDCode = M2Share.nSC_HAIRCOLOR;
                goto L001;
            }
            if (sCmd == M2Share.sSC_WEARCOLOR)
            {
                nCMDCode = M2Share.nSC_WEARCOLOR;
                goto L001;
            }
            if (sCmd == M2Share.sSC_HAIRSTYLE)
            {
                nCMDCode = M2Share.nSC_HAIRSTYLE;
                goto L001;
            }
            if (sCmd == M2Share.sSC_MONRECALL)
            {
                nCMDCode = M2Share.nSC_MONRECALL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_HORSECALL)
            {
                nCMDCode = M2Share.nSC_HORSECALL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_HAIRRNDCOL)
            {
                nCMDCode = M2Share.nSC_HAIRRNDCOL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_KILLHORSE)
            {
                nCMDCode = M2Share.nSC_KILLHORSE;
                goto L001;
            }
            if (sCmd == M2Share.sSC_RANDSETDAILYQUEST)
            {
                nCMDCode = M2Share.nSC_RANDSETDAILYQUEST;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHANGELEVEL)
            {
                nCMDCode = M2Share.nSC_CHANGELEVEL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_MARRY)
            {
                nCMDCode = M2Share.nSC_MARRY;
                goto L001;
            }
            if (sCmd == M2Share.sSC_UNMARRY)
            {
                nCMDCode = M2Share.nSC_UNMARRY;
                goto L001;
            }
            if (sCmd == M2Share.sSC_GETMARRY)
            {
                nCMDCode = M2Share.nSC_GETMARRY;
                goto L001;
            }
            if (sCmd == M2Share.sSC_GETMASTER)
            {
                nCMDCode = M2Share.nSC_GETMASTER;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CLEARSKILL)
            {
                nCMDCode = M2Share.nSC_CLEARSKILL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_DELNOJOBSKILL)
            {
                nCMDCode = M2Share.nSC_DELNOJOBSKILL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_DELSKILL)
            {
                nCMDCode = M2Share.nSC_DELSKILL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_ADDSKILL)
            {
                nCMDCode = M2Share.nSC_ADDSKILL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_SKILLLEVEL)
            {
                nCMDCode = M2Share.nSC_SKILLLEVEL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHANGEPKPOINT)
            {
                nCMDCode = M2Share.nSC_CHANGEPKPOINT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHANGEEXP)
            {
                nCMDCode = M2Share.nSC_CHANGEEXP;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHANGEJOB)
            {
                nCMDCode = M2Share.nSC_CHANGEJOB;
                goto L001;
            }
            if (sCmd == M2Share.sSC_MISSION)
            {
                nCMDCode = M2Share.nSC_MISSION;
                goto L001;
            }
            if (sCmd == M2Share.sSC_MOBPLACE)
            {
                nCMDCode = M2Share.nSC_MOBPLACE;
                goto L001;
            }
            if (sCmd == M2Share.sSC_SETMEMBERTYPE)
            {
                nCMDCode = M2Share.nSC_SETMEMBERTYPE;
                goto L001;
            }
            if (sCmd == M2Share.sSC_SETMEMBERLEVEL)
            {
                nCMDCode = M2Share.nSC_SETMEMBERLEVEL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_GAMEGOLD)
            {
                nCMDCode = M2Share.nSC_GAMEGOLD;
                goto L001;
            }
            if (sCmd == M2Share.sSC_GAMEPOINT)
            {
                nCMDCode = M2Share.nSC_GAMEPOINT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_PKZONE)
            {
                nCMDCode = M2Share.nSC_PKZONE;
                goto L001;
            }
            if (sCmd == M2Share.sSC_RESTBONUSPOINT)
            {
                nCMDCode = M2Share.nSC_RESTBONUSPOINT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_TAKECASTLEGOLD)
            {
                nCMDCode = M2Share.nSC_TAKECASTLEGOLD;
                goto L001;
            }
            if (sCmd == M2Share.sSC_HUMANHP)
            {
                nCMDCode = M2Share.nSC_HUMANHP;
                goto L001;
            }
            if (sCmd == M2Share.sSC_HUMANMP)
            {
                nCMDCode = M2Share.nSC_HUMANMP;
                goto L001;
            }
            if (sCmd == M2Share.sSC_BUILDPOINT)
            {
                nCMDCode = M2Share.nSC_BUILDPOINT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_AURAEPOINT)
            {
                nCMDCode = M2Share.nSC_AURAEPOINT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_STABILITYPOINT)
            {
                nCMDCode = M2Share.nSC_STABILITYPOINT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_FLOURISHPOINT)
            {
                nCMDCode = M2Share.nSC_FLOURISHPOINT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_OPENMAGICBOX)
            {
                nCMDCode = M2Share.nSC_OPENMAGICBOX;
                goto L001;
            }
            if (sCmd == M2Share.sSC_SETRANKLEVELNAME)
            {
                nCMDCode = M2Share.nSC_SETRANKLEVELNAME;
                goto L001;
            }
            if (sCmd == M2Share.sSC_GMEXECUTE)
            {
                nCMDCode = M2Share.nSC_GMEXECUTE;
                goto L001;
            }
            if (sCmd == M2Share.sSC_GUILDCHIEFITEMCOUNT)
            {
                nCMDCode = M2Share.nSC_GUILDCHIEFITEMCOUNT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_ADDNAMEDATELIST)
            {
                nCMDCode = M2Share.nSC_ADDNAMEDATELIST;
                goto L001;
            }
            if (sCmd == M2Share.sSC_DELNAMEDATELIST)
            {
                nCMDCode = M2Share.nSC_DELNAMEDATELIST;
                goto L001;
            }
            if (sCmd == M2Share.sSC_MOBFIREBURN)
            {
                nCMDCode = M2Share.nSC_MOBFIREBURN;
                goto L001;
            }
            if (sCmd == M2Share.sSC_MESSAGEBOX)
            {
                nCMDCode = M2Share.nSC_MESSAGEBOX;
                goto L001;
            }
            if (sCmd == M2Share.sSC_SETSCRIPTFLAG)
            {
                nCMDCode = M2Share.nSC_SETSCRIPTFLAG;
                goto L001;
            }
            if (sCmd == M2Share.sSC_SETAUTOGETEXP)
            {
                nCMDCode = M2Share.nSC_SETAUTOGETEXP;
                goto L001;
            }
            if (sCmd == M2Share.sSC_VAR)
            {
                nCMDCode = M2Share.nSC_VAR;
                goto L001;
            }
            if (sCmd == M2Share.sSC_LOADVAR)
            {
                nCMDCode = M2Share.nSC_LOADVAR;
                goto L001;
            }
            if (sCmd == M2Share.sSC_SAVEVAR)
            {
                nCMDCode = M2Share.nSC_SAVEVAR;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CALCVAR)
            {
                nCMDCode = M2Share.nSC_CALCVAR;
                goto L001;
            }
            if (sCmd == M2Share.sSC_AUTOADDGAMEGOLD)
            {
                nCMDCode = M2Share.nSC_AUTOADDGAMEGOLD;
                goto L001;
            }
            if (sCmd == M2Share.sSC_AUTOSUBGAMEGOLD)
            {
                nCMDCode = M2Share.nSC_AUTOSUBGAMEGOLD;
                goto L001;
            }
            if (sCmd == M2Share.sSC_RECALLGROUPMEMBERS)
            {
                nCMDCode = M2Share.nSC_RECALLGROUPMEMBERS;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CLEARNAMELIST)
            {
                nCMDCode = M2Share.nSC_CLEARNAMELIST;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHANGENAMECOLOR)
            {
                nCMDCode = M2Share.nSC_CHANGENAMECOLOR;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CLEARPASSWORD)
            {
                nCMDCode = M2Share.nSC_CLEARPASSWORD;
                goto L001;
            }
            if (sCmd == M2Share.sSC_RENEWLEVEL)
            {
                nCMDCode = M2Share.nSC_RENEWLEVEL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_KILLMONEXPRATE)
            {
                nCMDCode = M2Share.nSC_KILLMONEXPRATE;
                goto L001;
            }
            if (sCmd == M2Share.sSC_POWERRATE)
            {
                nCMDCode = M2Share.nSC_POWERRATE;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHANGEMODE)
            {
                nCMDCode = M2Share.nSC_CHANGEMODE;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHANGEPERMISSION)
            {
                nCMDCode = M2Share.nSC_CHANGEPERMISSION;
                goto L001;
            }
            if (sCmd == M2Share.sSC_KILL)
            {
                nCMDCode = M2Share.nSC_KILL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_KICK)
            {
                nCMDCode = M2Share.nSC_KICK;
                goto L001;
            }
            if (sCmd == M2Share.sSC_BONUSPOINT)
            {
                nCMDCode = M2Share.nSC_BONUSPOINT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_RESTRENEWLEVEL)
            {
                nCMDCode = M2Share.nSC_RESTRENEWLEVEL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_DELMARRY)
            {
                nCMDCode = M2Share.nSC_DELMARRY;
                goto L001;
            }
            if (sCmd == M2Share.sSC_DELMASTER)
            {
                nCMDCode = M2Share.nSC_DELMASTER;
                goto L001;
            }
            if (sCmd == M2Share.sSC_MASTER)
            {
                nCMDCode = M2Share.nSC_MASTER;
                goto L001;
            }
            if (sCmd == M2Share.sSC_UNMASTER)
            {
                nCMDCode = M2Share.nSC_UNMASTER;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CREDITPOINT)
            {
                nCMDCode = M2Share.nSC_CREDITPOINT;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CLEARNEEDITEMS)
            {
                nCMDCode = M2Share.nSC_CLEARNEEDITEMS;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CLEARMAKEITEMS)
            {
                nCMDCode = M2Share.nSC_CLEARMAEKITEMS;
                goto L001;
            }
            if (sCmd == M2Share.sSC_SETSENDMSGFLAG)
            {
                nCMDCode = M2Share.nSC_SETSENDMSGFLAG;
                goto L001;
            }
            if (sCmd == M2Share.sSC_UPGRADEITEMS)
            {
                nCMDCode = M2Share.nSC_UPGRADEITEMS;
                goto L001;
            }
            if (sCmd == M2Share.sSC_UPGRADEITEMSEX)
            {
                nCMDCode = M2Share.nSC_UPGRADEITEMSEX;
                goto L001;
            }
            if (sCmd == M2Share.sSC_MONGENEX)
            {
                nCMDCode = M2Share.nSC_MONGENEX;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CLEARMAPMON)
            {
                nCMDCode = M2Share.nSC_CLEARMAPMON;
                goto L001;
            }
            if (sCmd == M2Share.sSC_SETMAPMODE)
            {
                nCMDCode = M2Share.nSC_SETMAPMODE;
                goto L001;
            }
            if (sCmd == M2Share.sSC_KILLSLAVE)
            {
                nCMDCode = M2Share.nSC_KILLSLAVE;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHANGEGENDER)
            {
                nCMDCode = M2Share.nSC_CHANGEGENDER;
                goto L001;
            }
            if (sCmd == M2Share.sSC_MAPTING)
            {
                nCMDCode = M2Share.nSC_MAPTING;
                goto L001;
            }
            if (sCmd == M2Share.sSC_GUILDRECALL)
            {
                nCMDCode = M2Share.nSC_GUILDRECALL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_GROUPRECALL)
            {
                nCMDCode = M2Share.nSC_GROUPRECALL;
                goto L001;
            }
            if (sCmd == M2Share.sSC_GROUPADDLIST)
            {
                nCMDCode = M2Share.nSC_GROUPADDLIST;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CLEARLIST)
            {
                nCMDCode = M2Share.nSC_CLEARLIST;
                goto L001;
            }
            if (sCmd == M2Share.sSC_GROUPMOVEMAP)
            {
                nCMDCode = M2Share.nSC_GROUPMOVEMAP;
                goto L001;
            }
            if (sCmd == M2Share.sSC_SAVESLAVES)
            {
                nCMDCode = M2Share.nSC_SAVESLAVES;
                goto L001;
            }
            if (sCmd == M2Share.sCHECKUSERDATE)
            {
                nCMDCode = M2Share.nCHECKUSERDATE;
                goto L001;
            }
            if (sCmd == M2Share.sADDUSERDATE)
            {
                nCMDCode = M2Share.nADDUSERDATE;
                goto L001;
            }
            if (sCmd == M2Share.sCheckDiemon)
            {
                nCMDCode = M2Share.nCheckDiemon;
                goto L001;
            }
            if (sCmd == M2Share.scheckkillplaymon)
            {
                nCMDCode = M2Share.ncheckkillplaymon;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKRANDOMNO)
            {
                nCMDCode = M2Share.nSC_CHECKRANDOMNO;
                goto L001;
            }
            if (sCmd == M2Share.sSC_CHECKISONMAP)
            {
                nCMDCode = M2Share.nSC_CHECKISONMAP;
                goto L001;
            }
            // 检测是否安全区
            if (sCmd == M2Share.sSC_CHECKINSAFEZONE)
            {
                nCMDCode = M2Share.nSC_CHECKINSAFEZONE;
                goto L001;
            }
            if (sCmd == M2Share.sSC_KILLBYHUM)
            {
                nCMDCode = M2Share.nSC_KILLBYHUM;
                goto L001;
            }
            if (sCmd == M2Share.sSC_KILLBYMON)
            {
                nCMDCode = M2Share.nSC_KILLBYMON;
                goto L001;
            }
            if (sCmd == M2Share.sSC_ISHIGH)
            {
                nCMDCode = M2Share.nSC_ISHIGH;
                goto L001;
            }
            L001:
            if (nCMDCode > 0)
            {
                QuestActionInfo.nCmdCode = nCMDCode;
                if (sParam1 != "" && sParam1[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam1, '\"', '\"', ref sParam1);
                }
                if (sParam2 != "" && sParam2[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam2, '\"', '\"', ref sParam2);
                }
                if (sParam3 != "" && sParam3[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam3, '\"', '\"', ref sParam3);
                }
                if (sParam4 != "" && sParam4[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam4, '\"', '\"', ref sParam4);
                }
                if (sParam5 != "" && sParam5[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam5, '\"', '\"', ref sParam5);
                }
                if (sParam6 != "" && sParam6[0] == '\"')
                {
                    HUtil32.ArrestStringEx(sParam6, '\"', '\"', ref sParam6);
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

        public int LoadScriptFile(TNormNpc NPC, string sPatch, string sScritpName, bool boFlag)
        {
            int I;
            var s30 = string.Empty;
            var s34 = string.Empty;
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
            var s74 = string.Empty;
            TDefineInfo DefineInfo;
            var bo8D = false;
            TScript Script = null;
            TSayingRecord SayingRecord = null;
            TSayingProcedure SayingProcedure = null;
            IList<string> ScriptNameList = null;
            List<TQuestActionInfo> GotoList = null;
            List<TQuestActionInfo> DelayGotoList = null;
            List<TQuestActionInfo> PlayDiceList = null;
            var result = -1;
            var n6C = 0;
            var n70 = 0;
            if (sScritpName=="-4")
            {
                Console.WriteLine("asdasd");
            }
            var sScritpFileName = Path.Combine(M2Share.g_Config.sEnvirDir, sPatch, string.Concat(sScritpName, ".txt"));
            if (File.Exists(sScritpFileName))
            {
                LoadList = new StringList();
                try
                {
                    LoadList.LoadFromFile(sScritpFileName);
                    DeCodeStringList(LoadList);
                }
                catch
                {
                    LoadList = null;
                    return result;
                }
                I = 0;
                while (true)
                {
                    LoadScriptFile_LoadScriptcall(LoadList);
                    I++;
                    if (I >= 101)
                    {
                        break;
                    }
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
                for (I = 0; I < LoadList.Count; I++)
                {
                    s34 = LoadList[I].Trim();
                    if (s34 != "")
                    {
                        if (s34[0] == '[')
                        {
                            bo8D = false;
                        }
                        else
                        {
                            if (s34[0] == '#' && (HUtil32.CompareLStr(s34, "#IF", "#IF".Length) || HUtil32.CompareLStr(s34, "#ACT", "#ACT".Length) || HUtil32.CompareLStr(s34, "#ELSEACT", "#ELSEACT".Length)))
                            {
                                bo8D = true;
                            }
                            else
                            {
                                if (bo8D)
                                {
                                    int n20;
                                    // 将Define 好的常量换成指定值
                                    for (n20 = 0; n20 < DefineList.Count; n20++)
                                    {
                                        DefineInfo = DefineList[n20];
                                        var n1C = 0;
                                        while (true)
                                        {
                                            n24 = s34.ToUpper().IndexOf(DefineInfo.sName);
                                            if (n24 <= 0)
                                            {
                                                break;
                                            }
                                            s58 = s34.Substring(1 - 1, n24 - 1);
                                            s5C = s34.Substring(DefineInfo.sName.Length + n24 - 1, 256);
                                            s34 = s58 + DefineInfo.sText + s5C;
                                            LoadList[I] = s34;
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
                // 常量处理
                // 释放常量定义内容
                for (I = 0; I < DefineList.Count; I++)
                {
                    DefineList[I] = null;
                }
                DefineList.Clear();
                var nQuestIdx = 0;
                for (I = 0; I < LoadList.Count; I++)
                {
                    s34 = LoadList[I].Trim();
                    if (s34 == "" || s34[0] == ';' || s34[0] == '/')
                    {
                        continue;
                    }
                    if (n6C == 0 && boFlag)
                    {
                        if (s34.StartsWith("%")) // 物品价格倍率
                        {
                            s34 = s34.Substring(1, s34.Length - 1);
                            var nPriceRate = HUtil32.Str_ToInt(s34, -1);
                            if (nPriceRate >= 55)
                            {
                                ((TMerchant)NPC).m_nPriceRate = nPriceRate;
                            }
                            continue;
                        }
                        if (s34.StartsWith("+")) // 物品交易类型
                        {
                            s34 = s34.Substring(1, s34.Length - 1);
                            var nItemType = HUtil32.Str_ToInt(s34, -1);
                            if (nItemType >= 0)
                            {
                                ((TMerchant)NPC).m_ItemTypeList.Add(nItemType);
                            }
                            continue;
                        }
                        if (s34.StartsWith("(")) // 增加处理NPC可执行命令设置
                        {
                            HUtil32.ArrestStringEx(s34, "(", ")", ref s34);
                            if (s34 != "")
                            {
                                while (s34 != "")
                                {
                                    s34 = HUtil32.GetValidStr3(s34, ref s30, new string[] { " ", ",", "\t" });
                                    if (s30.Equals(M2Share.sBUY, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((TMerchant)NPC).m_boBuy = true;
                                        continue;
                                    }
                                    else if (s30.Equals(M2Share.sSELL, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((TMerchant)NPC).m_boSell = true;
                                        continue;
                                    }
                                    else if (s30.Equals(M2Share.sMAKEDURG, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((TMerchant)NPC).m_boMakeDrug = true;
                                        continue;
                                    }
                                    else if (s30.Equals(M2Share.sPRICES, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((TMerchant)NPC).m_boPrices = true;
                                        continue;
                                    }
                                    else if (s30.Equals(M2Share.sSTORAGE, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((TMerchant)NPC).m_boStorage = true;
                                        continue;
                                    }
                                    else if (s30.Equals(M2Share.sGETBACK, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((TMerchant)NPC).m_boGetback = true;
                                        continue;
                                    }
                                    else if (s30.Equals(M2Share.sUPGRADENOW, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((TMerchant)NPC).m_boUpgradenow = true;
                                        continue;
                                    }
                                    else if (s30.Equals(M2Share.sGETBACKUPGNOW, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((TMerchant)NPC).m_boGetBackupgnow = true;
                                        continue;
                                    }
                                    else if (s30.Equals(M2Share.sREPAIR, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((TMerchant)NPC).m_boRepair = true;
                                        continue;
                                    }
                                    else if (s30.Equals(M2Share.sSUPERREPAIR, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((TMerchant)NPC).m_boS_repair = true;
                                        continue;
                                    }
                                    else if (s30.Equals(M2Share.sSL_SENDMSG, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((TMerchant)NPC).m_boSendmsg = true;
                                        continue;
                                    }
                                    else if (s30.Equals(M2Share.sUSEITEMNAME, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((TMerchant)NPC).m_boUseItemName = true;
                                        continue;
                                    }
                                    else if (s30.Equals(M2Share.sOFFLINEMSG, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ((TMerchant)NPC).m_boOffLineMsg = true;
                                        continue;
                                    }
                                }
                            }
                            continue;
                        }
                        // 增加处理NPC可执行命令设置
                    }
                    if (s34.StartsWith("{"))
                    {
                        if (HUtil32.CompareLStr(s34, "{Quest", "{Quest".Length))
                        {
                            s38 = HUtil32.GetValidStr3(s34, ref s3C, new string[] { " ", "}", "\t" });
                            HUtil32.GetValidStr3(s38, ref s3C, new string[] { " ", "}", "\t" });
                            n70 = HUtil32.Str_ToInt(s3C, 0);
                            Script = LoadScriptFile_MakeNewScript(NPC);
                            Script.nQuest = n70;
                            n70++;
                        }
                        if (HUtil32.CompareLStr(s34, "{~Quest", "{~Quest".Length))
                        {
                            continue;
                        }
                    }
                    if (n6C == 1 && Script != null && s34.StartsWith("#"))
                    {
                        s38 = HUtil32.GetValidStr3(s34, ref s3C, new string[] { "=", " ", "\t" });
                        Script.boQuest = true;
                        if (HUtil32.CompareLStr(s34, "#IF", "#IF".Length))
                        {
                            HUtil32.ArrestStringEx(s34, "[", "]", ref s40);
                            Script.QuestInfo[nQuestIdx].wFlag = (short)HUtil32.Str_ToInt(s40, 0);
                            HUtil32.GetValidStr3(s38, ref s44, new string[] { "=", " ", "\t" });
                            n24 = HUtil32.Str_ToInt(s44, 0);
                            if (n24 != 0)
                            {
                                n24 = 1;
                            }
                            Script.QuestInfo[nQuestIdx].btValue = (byte)n24;
                        }
                        if (HUtil32.CompareLStr(s34, "#RAND", "#RAND".Length))
                        {
                            Script.QuestInfo[nQuestIdx].nRandRage = HUtil32.Str_ToInt(s44, 0);
                        }
                        continue;
                    }
                    if (s34.StartsWith("["))
                    {
                        n6C = 10;
                        if (Script == null)
                        {
                            Script = LoadScriptFile_MakeNewScript(NPC);
                            Script.nQuest = n70;
                        }
                        if (s34.Equals("[goods]", StringComparison.OrdinalIgnoreCase))
                        {
                            n6C = 20;
                            continue;
                        }
                        s34 = HUtil32.ArrestStringEx(s34, "[", "]", ref s74);
                        SayingRecord = new TSayingRecord
                        {
                            ProcedureList = new List<TSayingProcedure>(),
                            sLabel = s74
                        };
                        s34 = HUtil32.GetValidStrCap(s34, ref s74, new string[] { " ", "\t" });
                        if (s74.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
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
                            SayingRecord.sLabel = SayingRecord.sLabel + M2Share.RandomNumber.GetRandomNumber(1, 200);
                        }
                        Script.RecordList.Add(SayingRecord.sLabel, SayingRecord);
                        ScriptNameList.Add(SayingRecord.sLabel);
                        continue;
                    }
                    if (Script != null && SayingRecord != null)
                    {
                        if (n6C >= 10 && n6C < 20 && s34[0] == '#')
                        {
                            if (s34.Equals("#IF", StringComparison.OrdinalIgnoreCase))
                            {
                                if (SayingProcedure.ConditionList.Count > 0 || SayingProcedure.sSayMsg != "")
                                {
                                    SayingProcedure = new TSayingProcedure();
                                    SayingRecord.ProcedureList.Add(SayingProcedure);
                                }
                                n6C = 11;
                            }
                            if (s34.Equals("#ACT", StringComparison.OrdinalIgnoreCase))
                            {
                                n6C = 12;
                            }
                            if (s34.Equals("#SAY", StringComparison.OrdinalIgnoreCase))
                            {
                                n6C = 10;
                            }
                            if (s34.Equals("#ELSEACT", StringComparison.OrdinalIgnoreCase))
                            {
                                n6C = 13;
                            }
                            if (s34.Equals("#ELSESAY", StringComparison.OrdinalIgnoreCase))
                            {
                                n6C = 14;
                            }
                            continue;
                        }
                        if (n6C == 10 && SayingProcedure != null)
                        {
                            SayingProcedure.sSayMsg = SayingProcedure.sSayMsg + s34;
                        }
                        if (n6C == 11)
                        {
                            QuestConditionInfo = new TQuestConditionInfo();
                            if (LoadScriptFile_QuestCondition(s34.Trim(), QuestConditionInfo))
                            {
                                SayingProcedure.ConditionList.Add(QuestConditionInfo);
                            }
                            else
                            {
                                QuestConditionInfo = null;
                                M2Share.MainOutMessage("脚本错误: " + s34 + " 第:" + I + " 行: " + sScritpFileName);
                            }
                        }
                        if (n6C == 12)
                        {
                            QuestActionInfo = new TQuestActionInfo();
                            if (LoadScriptFile_QuestAction(s34.Trim(), QuestActionInfo))
                            {
                                SayingProcedure.ActionList.Add(QuestActionInfo);
                            }
                            else
                            {
                                QuestActionInfo = null;
                                M2Share.MainOutMessage("脚本错误: " + s34 + " 第:" + I + " 行: " + sScritpFileName);
                            }
                        }
                        if (n6C == 13)
                        {
                            QuestActionInfo = new TQuestActionInfo();
                            if (LoadScriptFile_QuestAction(s34.Trim(), QuestActionInfo))
                            {
                                SayingProcedure.ElseActionList.Add(QuestActionInfo);
                            }
                            else
                            {
                                QuestActionInfo = null;
                                M2Share.MainOutMessage("脚本错误: " + s34 + " 第:" + I + " 行: " + sScritpFileName);
                            }
                        }
                        if (n6C == 14)
                        {
                            SayingProcedure.sElseSayMsg = SayingProcedure.sElseSayMsg + s34;
                        }
                    }
                    if (n6C == 20 && boFlag)
                    {
                        s34 = HUtil32.GetValidStrCap(s34, ref s48, new string[] { " ", "\t" });
                        s34 = HUtil32.GetValidStrCap(s34, ref s4C, new string[] { " ", "\t" });
                        s34 = HUtil32.GetValidStrCap(s34, ref s50, new string[] { " ", "\t" });
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
                                ((TMerchant)NPC).m_RefillGoodsList.Add(Goods);
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
            result = 1;
            return result;
        }

        private void DeCodeStringList(StringList StringList)
        {
            string sLine;
            if (StringList.Count > 0)
            {
                sLine = StringList[0];
                if (!HUtil32.CompareLStr(sLine, grobal2.sENCYPTSCRIPTFLAG, grobal2.sENCYPTSCRIPTFLAG.Length))
                {
                    return;
                }
            }
            for (var i = 0; i < StringList.Count; i++)
            {
                sLine = StringList[i];
                sLine = LocalDB.DeCodeString(sLine);
                StringList[i] = sLine;
            }
        }


        /// <summary>
        /// 初始化脚本标签数组
        /// </summary>
        internal void InitializeLabel(TNormNpc NPC, TQuestActionInfo QuestActionInfo, IList<string> ScriptNameList, List<TQuestActionInfo> PlayDiceList, List<TQuestActionInfo> GotoList, List<TQuestActionInfo> DelayGotoList)
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
            var boChange = false;
            for (var i = 0; i < PlayDiceList.Count; i++)
            {
                QuestActionInfo = PlayDiceList[i];
                nIdx = ScriptNameList.IndexOf(FormatLabelStr(QuestActionInfo.sParam2, ref boChange));
                QuestActionInfo.sParam2 = "@" + nIdx;
            }
            for (var i = 0; i < GotoList.Count; i++)
            {
                QuestActionInfo = GotoList[i];
                nIdx = ScriptNameList.IndexOf(FormatLabelStr(QuestActionInfo.sParam1, ref boChange));
                QuestActionInfo.nParam1 = nIdx;
            }
            for (var i = 0; i < DelayGotoList.Count; i++)
            {
                QuestActionInfo = DelayGotoList[i];
                nIdx = ScriptNameList.IndexOf(FormatLabelStr(QuestActionInfo.sParam2, ref boChange));
                QuestActionInfo.nParam2 = nIdx;
            }
            for (var i = 0; i < NPC.m_ScriptList.Count; i++)
            {
                var RecordList = NPC.m_ScriptList[i];
                nIdx = 0;
                foreach (var SayingRecord in RecordList.RecordList.Values)
                {
                    for (var k = 0; k < SayingRecord.ProcedureList.Count; k++)
                    {
                        var SayingProcedure = SayingRecord.ProcedureList[k];
                        if (!string.IsNullOrEmpty(SayingProcedure.sSayMsg))
                        {
                            SayingProcedure.sSayMsg = InitializeSayMsg(SayingProcedure.sSayMsg, SayingProcedure.SayNewLabelList, SayingProcedure.SayOldLabelList, ScriptNameList);
                        }
                        if (!string.IsNullOrEmpty(SayingProcedure.sElseSayMsg))
                        {
                            SayingProcedure.sElseSayMsg = InitializeSayMsg(SayingProcedure.sElseSayMsg, SayingProcedure.ElseSayNewLabelList, SayingProcedure.ElseSayOldLabelList, ScriptNameList);
                        }
                    }
                    InitializeAppendLabel(NPC, SayingRecord.sLabel, nIdx);
                    nIdx++;
                }
            }
        }

        /// <summary>
        /// 初始化脚本标签
        /// </summary>
        internal void InitializeAppendLabel(TNormNpc NPC, string sLabel, int nIdx)
        {
            if (sLabel == SctiptDef.SPLAYOFFLINE)
            {
                NPC.FGotoLable[SctiptDef.NPLAYOFFLINE] = nIdx;
            }
            else if (sLabel == SctiptDef.SMARRYERROR)
            {
                NPC.FGotoLable[SctiptDef.NMARRYERROR] = nIdx;
            }
            else if (sLabel == SctiptDef.SMASTERERROR)
            {
                NPC.FGotoLable[SctiptDef.NMASTERERROR] = nIdx;
            }
            else if (sLabel == SctiptDef.SMARRYCHECKDIR)
            {
                NPC.FGotoLable[SctiptDef.NMARRYCHECKDIR] = nIdx;
            }
            else if (sLabel == SctiptDef.SHUMANTYPEERR)
            {
                NPC.FGotoLable[SctiptDef.NHUMANTYPEERR] = nIdx;
            }
            else if (sLabel == SctiptDef.SSTARTMARRY)
            {
                NPC.FGotoLable[SctiptDef.NSTARTMARRY] = nIdx;
            }
            else if (sLabel == SctiptDef.SMARRYSEXERR)
            {
                NPC.FGotoLable[SctiptDef.NMARRYSEXERR] = nIdx;
            }
            else if (sLabel == SctiptDef.SMARRYDIRERR)
            {
                NPC.FGotoLable[SctiptDef.NMARRYDIRERR] = nIdx;
            }
            else if (sLabel == SctiptDef.SWATEMARRY)
            {
                NPC.FGotoLable[SctiptDef.NWATEMARRY] = nIdx;
            }
            else if (sLabel == SctiptDef.SREVMARRY)
            {
                NPC.FGotoLable[SctiptDef.NREVMARRY] = nIdx;
            }
            else if (sLabel == SctiptDef.SENDMARRY)
            {
                NPC.FGotoLable[SctiptDef.NENDMARRY] = nIdx;
            }
            else if (sLabel == SctiptDef.SENDMARRYFAIL)
            {
                NPC.FGotoLable[SctiptDef.NENDMARRYFAIL] = nIdx;
            }
            else if (sLabel == SctiptDef.SMASTERCHECKDIR)
            {
                NPC.FGotoLable[SctiptDef.NMASTERCHECKDIR] = nIdx;
            }
            else if (sLabel == SctiptDef.SSTARTGETMASTER)
            {
                NPC.FGotoLable[SctiptDef.NSTARTGETMASTER] = nIdx;
            }
            else if (sLabel == SctiptDef.SMASTERDIRERR)
            {
                NPC.FGotoLable[SctiptDef.NMASTERDIRERR] = nIdx;
            }
            else if (sLabel == SctiptDef.SWATEMASTER)
            {
                NPC.FGotoLable[SctiptDef.NWATEMASTER] = nIdx;
            }
            else if (sLabel == SctiptDef.SREVMASTER)
            {
                NPC.FGotoLable[SctiptDef.NREVMASTER] = nIdx;
            }
            else if (sLabel == SctiptDef.SENDMASTER)
            {
                NPC.FGotoLable[SctiptDef.NENDMASTER] = nIdx;
            }
            else if (sLabel == SctiptDef.SSTARTMASTER)
            {
                NPC.FGotoLable[SctiptDef.NSTARTMASTER] = nIdx;
            }
            else if (sLabel == SctiptDef.SENDMASTERFAIL)
            {
                NPC.FGotoLable[SctiptDef.NENDMASTERFAIL] = nIdx;
            }
            else if (sLabel == SctiptDef.SEXEMARRYFAIL)
            {
                NPC.FGotoLable[SctiptDef.NEXEMARRYFAIL] = nIdx;
            }
            else if (sLabel == SctiptDef.SUNMARRYCHECKDIR)
            {
                NPC.FGotoLable[SctiptDef.NUNMARRYCHECKDIR] = nIdx;
            }
            else if (sLabel == SctiptDef.SUNMARRYTYPEERR)
            {
                NPC.FGotoLable[SctiptDef.NUNMARRYTYPEERR] = nIdx;
            }
            else if (sLabel == SctiptDef.SSTARTUNMARRY)
            {
                NPC.FGotoLable[SctiptDef.NSTARTUNMARRY] = nIdx;
            }
            else if (sLabel == SctiptDef.SUNMARRYEND)
            {
                NPC.FGotoLable[SctiptDef.NUNMARRYEND] = nIdx;
            }
            else if (sLabel == SctiptDef.SWATEUNMARRY)
            {
                NPC.FGotoLable[SctiptDef.NWATEUNMARRY] = nIdx;
            }
            else if (sLabel == SctiptDef.SEXEMASTERFAIL)
            {
                NPC.FGotoLable[SctiptDef.NEXEMASTERFAIL] = nIdx;
            }
            else if (sLabel == SctiptDef.SUNMASTERCHECKDIR)
            {
                NPC.FGotoLable[SctiptDef.NUNMASTERCHECKDIR] = nIdx;
            }
            else if (sLabel == SctiptDef.SUNMASTERTYPEERR)
            {
                NPC.FGotoLable[SctiptDef.NUNMASTERTYPEERR] = nIdx;
            }
            else if (sLabel == SctiptDef.SUNISMASTER)
            {
                NPC.FGotoLable[SctiptDef.NUNISMASTER] = nIdx;
            }
            else if (sLabel == SctiptDef.SUNMASTERERROR)
            {
                NPC.FGotoLable[SctiptDef.NUNMASTERERROR] = nIdx;
            }
            else if (sLabel == SctiptDef.SSTARTUNMASTER)
            {
                NPC.FGotoLable[SctiptDef.NSTARTUNMASTER] = nIdx;
            }
            else if (sLabel == SctiptDef.SWATEUNMASTER)
            {
                NPC.FGotoLable[SctiptDef.NWATEUNMASTER] = nIdx;
            }
            else if (sLabel == SctiptDef.SUNMASTEREND)
            {
                NPC.FGotoLable[SctiptDef.NUNMASTEREND] = nIdx;
            }
            else if (sLabel == SctiptDef.SREVUNMASTER)
            {
                NPC.FGotoLable[SctiptDef.NREVUNMASTER] = nIdx;
            }
            else if (sLabel == SctiptDef.SSUPREQUEST_OK)
            {
                NPC.FGotoLable[SctiptDef.NSUPREQUEST_OK] = nIdx;
            }
            else if (sLabel == SctiptDef.SMEMBER)
            {
                NPC.FGotoLable[SctiptDef.NMEMBER] = nIdx;
            }
            else if (sLabel == SctiptDef.SPLAYRECONNECTION)
            {
                NPC.FGotoLable[SctiptDef.NPLAYRECONNECTION] = nIdx;
            }
            else if (sLabel == SctiptDef.SLOGIN)
            {
                NPC.FGotoLable[SctiptDef.NLOGIN] = nIdx;
            }
            else if (sLabel == SctiptDef.SPLAYDIE)
            {
                NPC.FGotoLable[SctiptDef.NPLAYDIE] = nIdx;
            }
            else if (sLabel == SctiptDef.SKILLPLAY)
            {
                NPC.FGotoLable[SctiptDef.NKILLPLAY] = nIdx;
            }
            else if (sLabel == SctiptDef.SPLAYLEVELUP)
            {
                NPC.FGotoLable[SctiptDef.NPLAYLEVELUP] = nIdx;
            }
            else if (sLabel == SctiptDef.SKILLMONSTER)
            {
                NPC.FGotoLable[SctiptDef.NKILLMONSTER] = nIdx;
            }
            else if (sLabel == SctiptDef.SCREATEECTYPE_IN)
            {
                NPC.FGotoLable[SctiptDef.NCREATEECTYPE_IN] = nIdx;
            }
            else if (sLabel == SctiptDef.SCREATEECTYPE_OK)
            {
                NPC.FGotoLable[SctiptDef.NCREATEECTYPE_OK] = nIdx;
            }
            else if (sLabel == SctiptDef.SCREATEECTYPE_FAIL)
            {
                NPC.FGotoLable[SctiptDef.NCREATEECTYPE_FAIL] = nIdx;
            }
            else if (sLabel == SctiptDef.SRESUME)
            {
                NPC.FGotoLable[SctiptDef.NRESUME] = nIdx;
            }
            else if (sLabel == SctiptDef.SGETLARGESSGOLD_OK)
            {
                NPC.FGotoLable[SctiptDef.NGETLARGESSGOLD_OK] = nIdx;
            }
            else if (sLabel == SctiptDef.SGETLARGESSGOLD_FAIL)
            {
                NPC.FGotoLable[SctiptDef.NGETLARGESSGOLD_FAIL] = nIdx;
            }
            else if (sLabel == SctiptDef.SGETLARGESSGOLD_ERROR)
            {
                NPC.FGotoLable[SctiptDef.NGETLARGESSGOLD_ERROR] = nIdx;
            }
            else if (sLabel == SctiptDef.SMASTERISPRENTICE)
            {
                NPC.FGotoLable[SctiptDef.NMASTERISPRENTICE] = nIdx;
            }
            else if (sLabel == SctiptDef.SMASTERISFULL)
            {
                NPC.FGotoLable[SctiptDef.NMASTERISFULL] = nIdx;
            }
            else if (sLabel == SctiptDef.SGROUPCREATE)
            {
                NPC.FGotoLable[SctiptDef.NGROUPCREATE] = nIdx;
            }
            else if (sLabel == SctiptDef.SSTARTGROUP)
            {
                NPC.FGotoLable[SctiptDef.NSTARTGROUP] = nIdx;
            }
            else if (sLabel == SctiptDef.SJOINGROUP)
            {
                NPC.FGotoLable[SctiptDef.NJOINGROUP] = nIdx;
            }
            else if (sLabel == SctiptDef.SSPEEDCLOSE)
            {
                NPC.FGotoLable[SctiptDef.NSPEEDCLOSE] = nIdx;
            }
            else if (sLabel == SctiptDef.SUPGRADENOW_OK)
            {
                NPC.FGotoLable[SctiptDef.NUPGRADENOW_OK] = nIdx;
            }
            else if (sLabel == SctiptDef.SUPGRADENOW_ING)
            {
                NPC.FGotoLable[SctiptDef.NUPGRADENOW_ING] = nIdx;
            }
            else if (sLabel == SctiptDef.SUPGRADENOW_FAIL)
            {
                NPC.FGotoLable[SctiptDef.NUPGRADENOW_FAIL] = nIdx;
            }
            else if (sLabel == SctiptDef.SGETBACKUPGNOW_OK)
            {
                NPC.FGotoLable[SctiptDef.NGETBACKUPGNOW_OK] = nIdx;
            }
            else if (sLabel == SctiptDef.SGETBACKUPGNOW_ING)
            {
                NPC.FGotoLable[SctiptDef.NGETBACKUPGNOW_ING] = nIdx;
            }
            else if (sLabel == SctiptDef.SGETBACKUPGNOW_FAIL)
            {
                NPC.FGotoLable[SctiptDef.NGETBACKUPGNOW_FAIL] = nIdx;
            }
            else if (sLabel == SctiptDef.SGETBACKUPGNOW_BAGFULL)
            {
                NPC.FGotoLable[SctiptDef.NGETBACKUPGNOW_BAGFULL] = nIdx;
            }
            else if (sLabel == SctiptDef.STAKEONITEMS)
            {
                NPC.FGotoLable[SctiptDef.NTAKEONITEMS] = nIdx;
            }
            else if (sLabel == SctiptDef.STAKEOFFITEMS)
            {
                NPC.FGotoLable[SctiptDef.NTAKEOFFITEMS] = nIdx;
            }
            else if (sLabel == SctiptDef.SPLAYREVIVE)
            {
                NPC.FGotoLable[SctiptDef.NPLAYREVIVE] = nIdx;
            }
            else if (sLabel == SctiptDef.SMOVEABILITY_OK)
            {
                NPC.FGotoLable[SctiptDef.NMOVEABILITY_OK] = nIdx;
            }
            else if (sLabel == SctiptDef.SMOVEABILITY_FAIL)
            {
                NPC.FGotoLable[SctiptDef.NMOVEABILITY_FAIL] = nIdx;
            }
            else if (sLabel == SctiptDef.SASSEMBLEALL)
            {
                NPC.FGotoLable[SctiptDef.NASSEMBLEALL] = nIdx;
            }
            else if (sLabel == SctiptDef.SASSEMBLEWEAPON)
            {
                NPC.FGotoLable[SctiptDef.NASSEMBLEWEAPON] = nIdx;
            }
            else if (sLabel == SctiptDef.SASSEMBLEDRESS)
            {
                NPC.FGotoLable[SctiptDef.NASSEMBLEDRESS] = nIdx;
            }
            else if (sLabel == SctiptDef.SASSEMBLEHELMET)
            {
                NPC.FGotoLable[SctiptDef.NASSEMBLEHELMET] = nIdx;
            }
            else if (sLabel == SctiptDef.SASSEMBLENECKLACE)
            {
                NPC.FGotoLable[SctiptDef.NASSEMBLENECKLACE] = nIdx;
            }
            else if (sLabel == SctiptDef.SASSEMBLERING)
            {
                NPC.FGotoLable[SctiptDef.NASSEMBLERING] = nIdx;
            }
            else if (sLabel == SctiptDef.SASSEMBLEARMRING)
            {
                NPC.FGotoLable[SctiptDef.NASSEMBLEARMRING] = nIdx;
            }
            else if (sLabel == SctiptDef.SASSEMBLEBELT)
            {
                NPC.FGotoLable[SctiptDef.NASSEMBLEBELT] = nIdx;
            }
            else if (sLabel == SctiptDef.SASSEMBLEBOOT)
            {
                NPC.FGotoLable[SctiptDef.NASSEMBLEBOOT] = nIdx;
            }
            else if (sLabel == SctiptDef.SASSEMBLEFAIL)
            {
                NPC.FGotoLable[SctiptDef.NASSEMBLEFAIL] = nIdx;
            }
            else if (sLabel == SctiptDef.SCREATEHEROFAILEX)
            {
                NPC.FGotoLable[SctiptDef.NCREATEHEROFAILEX] = nIdx;// 创建英雄失败  By John 2012.08.04
            }
            else if (sLabel == SctiptDef.SLOGOUTHEROFIRST)
            {
                NPC.FGotoLable[SctiptDef.NLOGOUTHEROFIRST] = nIdx;// 请将英雄设置下线  By John 2012.08.04
            }
            else if (sLabel == SctiptDef.SNOTHAVEHERO)
            {
                NPC.FGotoLable[SctiptDef.NNOTHAVEHERO] = nIdx;// 没有英雄   By John 2012.08.04
            }
            else if (sLabel == SctiptDef.SHERONAMEFILTER)
            {
                NPC.FGotoLable[SctiptDef.NHERONAMEFILTER] = nIdx;// 英雄名字中包含禁用字符   By John 2012.08.04
            }
            else if (sLabel == SctiptDef.SHAVEHERO)
            {
                NPC.FGotoLable[SctiptDef.NHAVEHERO] = nIdx;// 有英雄    By John 2012.08.04
            }
            else if (sLabel == SctiptDef.SCREATEHEROOK)
            {
                NPC.FGotoLable[SctiptDef.NCREATEHEROOK] = nIdx;// 创建英雄OK   By John 2012.08.04
            }
            else if (sLabel == SctiptDef.SHERONAMEEXISTS)
            {
                NPC.FGotoLable[SctiptDef.NHERONAMEEXISTS] = nIdx;// 英雄名字已经存在  By John 2012.08.04
            }
            else if (sLabel == SctiptDef.SDELETEHEROOK)
            {
                NPC.FGotoLable[SctiptDef.NDELETEHEROOK] = nIdx;// 删除英雄成功    By John 2012.08.04
            }
            else if (sLabel == SctiptDef.SDELETEHEROFAIL)
            {
                NPC.FGotoLable[SctiptDef.NDELETEHEROFAIL] = nIdx;// 删除英雄失败    By John 2012.08.04
            }
            else if (sLabel == SctiptDef.SHEROOVERCHRCOUNT)
            {
                NPC.FGotoLable[SctiptDef.NHEROOVERCHRCOUNT] = nIdx;// 你的帐号角色过多   By John 2012.08.04
            }
            else
            {
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
            }
        }

        /// <summary>
        /// 格式化标签
        /// </summary>
        /// <param name="sLabel"></param>
        /// <param name="boChange"></param>
        /// <returns></returns>
        internal string FormatLabelStr(string sLabel, ref bool boChange)
        {
            var result = sLabel;
            if (sLabel.IndexOf(")") > -1)
            {
                HUtil32.GetValidStr3(sLabel, ref result, new string[] { "(" });
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
                if (tempstr.IndexOf(">") < -1)
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
        internal string InitializeSayMsg(string sMsg, List<string> StringList, IList<string> OldStringList, IList<string> ScriptNameList)
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
                                sMsg = SctiptDef.RESETLABEL + sMsg;
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
        internal void InitializeVariable(string sLabel, ref string sMsg)
        {
            var s14 = string.Empty;
            var sLabel2 = sLabel.ToUpper();
            if (sLabel2 == SctiptDef.sVAR_SERVERNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_SERVERNAME);
            }
            else if (sLabel2 == SctiptDef.sVAR_SERVERIP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_SERVERIP);
            }
            else if (sLabel2 == SctiptDef.sVAR_WEBSITE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_WEBSITE);
            }
            else if (sLabel2 == SctiptDef.sVAR_BBSSITE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_BBSSITE);
            }
            else if (sLabel2 == SctiptDef.sVAR_CLIENTDOWNLOAD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CLIENTDOWNLOAD);
            }
            else if (sLabel2 == SctiptDef.sVAR_QQ)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_QQ);
            }
            else if (sLabel2 == SctiptDef.sVAR_PHONE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_PHONE);
            }
            else if (sLabel2 == SctiptDef.sVAR_BANKACCOUNT0)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_BANKACCOUNT0);
            }
            else if (sLabel2 == SctiptDef.sVAR_BANKACCOUNT1)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_BANKACCOUNT1);
            }
            else if (sLabel2 == SctiptDef.sVAR_BANKACCOUNT2)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_BANKACCOUNT2);
            }
            else if (sLabel2 == SctiptDef.sVAR_BANKACCOUNT3)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_BANKACCOUNT3);
            }
            else if (sLabel2 == SctiptDef.sVAR_BANKACCOUNT4)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_BANKACCOUNT4);
            }
            else if (sLabel2 == SctiptDef.sVAR_BANKACCOUNT5)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_BANKACCOUNT5);
            }
            else if (sLabel2 == SctiptDef.sVAR_BANKACCOUNT6)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_BANKACCOUNT6);
            }
            else if (sLabel2 == SctiptDef.sVAR_BANKACCOUNT7)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_BANKACCOUNT7);
            }
            else if (sLabel2 == SctiptDef.sVAR_BANKACCOUNT8)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_BANKACCOUNT8);
            }
            else if (sLabel2 == SctiptDef.sVAR_BANKACCOUNT9)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_BANKACCOUNT9);
            }
            else if (sLabel2 == SctiptDef.sVAR_GAMEGOLDNAME)
            {
                //sMsg = sMsg.Replace("<" + sLabel + ">", Grobal2.sSTRING_GAMEGOLD);
            }
            else if (sLabel2 == SctiptDef.sVAR_GAMEPOINTNAME)
            {
                // sMsg = sMsg.Replace("<" + sLabel + ">", Grobal2.sSTRING_GAMEPOINT);
            }
            else if (sLabel2 == SctiptDef.sVAR_USERCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_USERCOUNT);
            }
            else if (sLabel2 == SctiptDef.sVAR_DATETIME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_DATETIME);
            }
            else if (sLabel2 == SctiptDef.sVAR_USERNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_USERNAME);
            }
            else if (sLabel2 == SctiptDef.sVAR_FBMAPNAME)
            { //副本
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_FBMAPNAME);
            }
            else if (sLabel2 == SctiptDef.sVAR_FBMAP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_FBMAP);
            }
            else if (sLabel2 == SctiptDef.sVAR_ACCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_ACCOUNT);
            }
            else if (sLabel2 == SctiptDef.sVAR_ASSEMBLEITEMNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_ASSEMBLEITEMNAME);
            }
            else if (sLabel2 == SctiptDef.sVAR_MAPNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_MAPNAME);
            }
            else if (sLabel2 == SctiptDef.sVAR_GUILDNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_GUILDNAME);
            }
            else if (sLabel2 == SctiptDef.sVAR_RANKNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_RANKNAME);
            }
            else if (sLabel2 == SctiptDef.sVAR_LEVEL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_LEVEL);
            }
            else if (sLabel2 == SctiptDef.sVAR_HP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_HP);
            }
            else if (sLabel2 == SctiptDef.sVAR_MAXHP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_MAXHP);
            }
            else if (sLabel2 == SctiptDef.sVAR_MP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_MP);
            }
            else if (sLabel2 == SctiptDef.sVAR_MAXMP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_MAXMP);
            }
            else if (sLabel2 == SctiptDef.sVAR_AC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_AC);
            }
            else if (sLabel2 == SctiptDef.sVAR_MAXAC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_MAXAC);
            }
            else if (sLabel2 == SctiptDef.sVAR_MAC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_MAC);
            }
            else if (sLabel2 == SctiptDef.sVAR_MAXMAC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_MAXMAC);
            }
            else if (sLabel2 == SctiptDef.sVAR_DC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_DC);
            }
            else if (sLabel2 == SctiptDef.sVAR_MAXDC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_MAXDC);
            }
            else if (sLabel2 == SctiptDef.sVAR_MC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_MC);
            }
            else if (sLabel2 == SctiptDef.sVAR_MAXMC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_MAXMC);
            }
            else if (sLabel2 == SctiptDef.sVAR_SC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_SC);
            }
            else if (sLabel2 == SctiptDef.sVAR_MAXSC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_MAXSC);
            }
            else if (sLabel2 == SctiptDef.sVAR_EXP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_EXP);
            }
            else if (sLabel2 == SctiptDef.sVAR_MAXEXP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_MAXEXP);
            }
            else if (sLabel2 == SctiptDef.sVAR_PKPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_PKPOINT);
            }
            else if (sLabel2 == SctiptDef.sVAR_CREDITPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CREDITPOINT);
            }
            else if (sLabel2 == SctiptDef.sVAR_GOLDCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_GOLDCOUNT);
            }
            else if (sLabel2 == SctiptDef.sVAR_GAMEGOLD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_GAMEGOLD);
            }
            else if (sLabel2 == SctiptDef.sVAR_GAMEPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_GAMEPOINT);
            }
            else if (sLabel2 == SctiptDef.sVAR_LOGINTIME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_LOGINTIME);
            }
            else if (sLabel2 == SctiptDef.sVAR_LOGINLONG)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_LOGINLONG);
            }
            else if (sLabel2 == SctiptDef.sVAR_DRESS)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_DRESS);
            }
            else if (sLabel2 == SctiptDef.sVAR_WEAPON)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_WEAPON);
            }
            else if (sLabel2 == SctiptDef.sVAR_RIGHTHAND)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_RIGHTHAND);
            }
            else if (sLabel2 == SctiptDef.sVAR_HELMET)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_HELMET);
            }
            else if (sLabel2 == SctiptDef.sVAR_NECKLACE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_NECKLACE);
            }
            else if (sLabel2 == SctiptDef.sVAR_RING_R)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_RING_R);
            }
            else if (sLabel2 == SctiptDef.sVAR_RING_L)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_RING_L);
            }
            else if (sLabel2 == SctiptDef.sVAR_ARMRING_R)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_ARMRING_R);
            }
            else if (sLabel2 == SctiptDef.sVAR_ARMRING_L)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_ARMRING_L);
            }
            else if (sLabel2 == SctiptDef.sVAR_BUJUK)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_BUJUK);
            }
            else if (sLabel2 == SctiptDef.sVAR_BELT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_BELT);
            }
            else if (sLabel2 == SctiptDef.sVAR_BOOTS)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_BOOTS);
            }
            else if (sLabel2 == SctiptDef.sVAR_CHARM)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CHARM);
            }
            //=======================================没有用到的==============================
            else if (sLabel2 == SctiptDef.sVAR_HOUSE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_HOUSE);
            }
            else if (sLabel2 == SctiptDef.sVAR_CIMELIA)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CIMELIA);
            }
            //=======================================没有用到的==============================
            else if (sLabel2 == SctiptDef.sVAR_IPADDR)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_IPADDR);
            }
            else if (sLabel2 == SctiptDef.sVAR_IPLOCAL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_IPLOCAL);
            }
            else if (sLabel2 == SctiptDef.sVAR_GUILDBUILDPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_GUILDBUILDPOINT);
            }
            else if (sLabel2 == SctiptDef.sVAR_GUILDAURAEPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_GUILDAURAEPOINT);
            }
            else if (sLabel2 == SctiptDef.sVAR_GUILDSTABILITYPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_GUILDSTABILITYPOINT);
            }
            else if (sLabel2 == SctiptDef.sVAR_GUILDFLOURISHPOINT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_GUILDFLOURISHPOINT);
            }
            //=================================没用用到的====================================
            else if (sLabel2 == SctiptDef.sVAR_GUILDMONEYCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_GUILDMONEYCOUNT);
            }
            //=================================没用用到的结束====================================
            else if (sLabel2 == SctiptDef.sVAR_REQUESTCASTLEWARITEM)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_REQUESTCASTLEWARITEM);
            }
            else if (sLabel2 == SctiptDef.sVAR_REQUESTCASTLEWARDAY)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_REQUESTCASTLEWARDAY);
            }
            else if (sLabel2 == SctiptDef.sVAR_REQUESTBUILDGUILDITEM)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_REQUESTBUILDGUILDITEM);
            }
            else if (sLabel2 == SctiptDef.sVAR_OWNERGUILD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_OWNERGUILD);
            }
            else if (sLabel2 == SctiptDef.sVAR_CASTLENAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CASTLENAME);
            }
            else if (sLabel2 == SctiptDef.sVAR_LORD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_LORD);
            }
            else if (sLabel2 == SctiptDef.sVAR_GUILDWARFEE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_GUILDWARFEE);
            }
            else if (sLabel2 == SctiptDef.sVAR_BUILDGUILDFEE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_BUILDGUILDFEE);
            }
            else if (sLabel2 == SctiptDef.sVAR_CASTLEWARDATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CASTLEWARDATE);
            }
            else if (sLabel2 == SctiptDef.sVAR_LISTOFWAR)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_LISTOFWAR);
            }
            else if (sLabel2 == SctiptDef.sVAR_CASTLECHANGEDATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CASTLECHANGEDATE);
            }
            else if (sLabel2 == SctiptDef.sVAR_CASTLEWARLASTDATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CASTLEWARLASTDATE);
            }
            else if (sLabel2 == SctiptDef.sVAR_CASTLEGETDAYS)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CASTLEGETDAYS);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_DATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_DATE);
            }
            //===================================没用用到的======================================
            else if (sLabel2 == SctiptDef.sVAR_CMD_PRVMSG)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_PRVMSG);
            }
            //===================================没用用到的结束======================================
            else if (sLabel2 == SctiptDef.sVAR_CMD_ALLOWMSG)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_ALLOWMSG);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_LETSHOUT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_LETSHOUT);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_LETTRADE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_LETTRADE);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_LETGuild)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_LETGuild);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_ENDGUILD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_ENDGUILD);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_BANGUILDCHAT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_BANGUILDCHAT);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_AUTHALLY)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_AUTHALLY);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_AUTH)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_AUTH);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_AUTHCANCEL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_AUTHCANCEL);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_USERMOVE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_USERMOVE);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_SEARCHING)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_SEARCHING);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_ALLOWGROUPCALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_ALLOWGROUPCALL);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_GROUPRECALLL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_GROUPRECALLL);
            }
            #region 没有使用的
            //===========================================没有使用的========================================
            else if (sLabel2 == SctiptDef.sVAR_CMD_ALLOWGUILDRECALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_ALLOWGUILDRECALL);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLOWGUILDRECALL, SctiptDef.sVAR_CMD_ALLOWGUILDRECALL);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_GUILDRECALLL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_GUILDRECALLL);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_CMD_GUILDRECALLL, SctiptDef.sVAR_CMD_GUILDRECALLL);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_DEAR)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_DEAR);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_CMD_DEAR, SctiptDef.sVAR_CMD_DEAR);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_ALLOWDEARRCALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_ALLOWDEARRCALL);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLOWDEARRCALL, SctiptDef.sVAR_CMD_ALLOWDEARRCALL);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_DEARRECALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_DEARRECALL);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_CMD_DEARRECALL, SctiptDef.sVAR_CMD_DEARRECALL);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_MASTER)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_MASTER);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_CMD_MASTER, SctiptDef.sVAR_CMD_MASTER);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_ALLOWMASTERRECALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_ALLOWMASTERRECALL);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLOWMASTERRECALL, SctiptDef.sVAR_CMD_ALLOWMASTERRECALL);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_MASTERECALL)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_MASTERECALL);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_CMD_MASTERECALL, SctiptDef.sVAR_CMD_MASTERECALL);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_TAKEONHORSE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_TAKEONHORSE);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_CMD_TAKEONHORSE, SctiptDef.sVAR_CMD_TAKEONHORSE);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_TAKEOFHORSE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_TAKEOFHORSE);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_CMD_TAKEOFHORSE, SctiptDef.sVAR_CMD_TAKEOFHORSE);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_ALLSYSMSG)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_ALLSYSMSG);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLSYSMSG, SctiptDef.sVAR_CMD_ALLSYSMSG);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_MEMBERFUNCTION)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_MEMBERFUNCTION);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_CMD_MEMBERFUNCTION, SctiptDef.sVAR_CMD_MEMBERFUNCTION);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_MEMBERFUNCTIONEX)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_MEMBERFUNCTIONEX);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_CMD_MEMBERFUNCTIONEX, SctiptDef.sVAR_CMD_MEMBERFUNCTIONEX);
            }
            else if (sLabel2 == SctiptDef.sVAR_CASTLEGOLD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CASTLEGOLD);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_CASTLEGOLD, SctiptDef.sVAR_CASTLEGOLD);
            }
            else if (sLabel2 == SctiptDef.sVAR_TODAYINCOME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_TODAYINCOME);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_TODAYINCOME, SctiptDef.sVAR_TODAYINCOME);
            }
            else if (sLabel2 == SctiptDef.sVAR_CASTLEDOORSTATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CASTLEDOORSTATE);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_CASTLEDOORSTATE, SctiptDef.sVAR_CASTLEDOORSTATE);
            }
            else if (sLabel2 == SctiptDef.sVAR_REPAIRDOORGOLD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_REPAIRDOORGOLD);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_REPAIRDOORGOLD, SctiptDef.sVAR_REPAIRDOORGOLD);
            }
            else if (sLabel2 == SctiptDef.sVAR_REPAIRWALLGOLD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_REPAIRWALLGOLD);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_REPAIRWALLGOLD, SctiptDef.sVAR_REPAIRWALLGOLD);
            }
            else if (sLabel2 == SctiptDef.sVAR_GUARDFEE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_GUARDFEE);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_GUARDFEE, SctiptDef.sVAR_GUARDFEE);
            }
            else if (sLabel2 == SctiptDef.sVAR_ARCHERFEE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_ARCHERFEE);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_ARCHERFEE, SctiptDef.sVAR_ARCHERFEE);
            }
            else if (sLabel2 == SctiptDef.sVAR_GUARDRULE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_GUARDRULE);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_GUARDRULE, SctiptDef.sVAR_GUARDRULE);
            }
            else if (sLabel2 == SctiptDef.sVAR_STORAGE2STATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_STORAGE2STATE);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_STORAGE2STATE, SctiptDef.sVAR_STORAGE2STATE);
            }
            else if (sLabel2 == SctiptDef.sVAR_STORAGE3STATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_STORAGE3STATE);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_STORAGE3STATE, SctiptDef.sVAR_STORAGE3STATE);
            }
            else if (sLabel2 == SctiptDef.sVAR_STORAGE4STATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_STORAGE4STATE);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_STORAGE4STATE, SctiptDef.sVAR_STORAGE4STATE);
            }
            else if (sLabel2 == SctiptDef.sVAR_STORAGE5STATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_STORAGE5STATE);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_STORAGE5STATE, SctiptDef.sVAR_STORAGE5STATE);
            }
            else if (sLabel2 == SctiptDef.sVAR_SELFNAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_SELFNAME);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_SELFNAME, SctiptDef.sVAR_SELFNAME);
            }
            else if (sLabel2 == SctiptDef.sVAR_POSENAME)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_POSENAME);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_POSENAME, SctiptDef.sVAR_POSENAME);
            }
            else if (sLabel2 == SctiptDef.sVAR_GAMEDIAMOND)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_GAMEDIAMOND);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_GAMEDIAMOND, SctiptDef.sVAR_GAMEDIAMOND);
            }
            else if (sLabel2 == SctiptDef.sVAR_GAMEGIRD)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_GAMEGIRD);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_GAMEGIRD, SctiptDef.sVAR_GAMEGIRD);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_ALLOWFIREND)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_ALLOWFIREND);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_CMD_ALLOWFIREND, SctiptDef.sVAR_CMD_ALLOWFIREND);
            }
            else if (sLabel2 == SctiptDef.sVAR_EFFIGYSTATE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_EFFIGYSTATE);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_EFFIGYSTATE, SctiptDef.sVAR_EFFIGYSTATE);
            }
            else if (sLabel2 == SctiptDef.sVAR_EFFIGYOFFSET)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_EFFIGYOFFSET);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_EFFIGYOFFSET, SctiptDef.sVAR_EFFIGYOFFSET);
            }
            else if (sLabel2 == SctiptDef.sVAR_YEAR)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_YEAR);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_YEAR, SctiptDef.sVAR_YEAR);
            }
            else if (sLabel2 == SctiptDef.sVAR_MONTH)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_MONTH);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_MONTH, SctiptDef.sVAR_MONTH);
            }
            else if (sLabel2 == SctiptDef.sVAR_DAY)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_DAY);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_DAY, SctiptDef.sVAR_DAY);
            }
            else if (sLabel2 == SctiptDef.sVAR_HOUR)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_HOUR);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_HOUR, SctiptDef.sVAR_HOUR);
            }
            else if (sLabel2 == SctiptDef.sVAR_MINUTE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_MINUTE);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_MINUTE, SctiptDef.sVAR_MINUTE);
            }
            else if (sLabel2 == SctiptDef.sVAR_SEC)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_SEC);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_SEC, SctiptDef.sVAR_SEC);
            }
            else if (sLabel2 == SctiptDef.sVAR_MAP)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_MAP);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_MAP, SctiptDef.sVAR_MAP);
            }
            else if (sLabel2 == SctiptDef.sVAR_X)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_X);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_X, SctiptDef.sVAR_X);
            }
            else if (sLabel2 == SctiptDef.sVAR_Y)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_Y);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_Y, SctiptDef.sVAR_Y);
            }
            else if (sLabel2 == SctiptDef.sVAR_UNMASTER_FORCE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_UNMASTER_FORCE);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_UNMASTER_FORCE, SctiptDef.sVAR_UNMASTER_FORCE);
            }
            else if (sLabel2 == SctiptDef.sVAR_USERGOLDCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_USERGOLDCOUNT);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_USERGOLDCOUNT, SctiptDef.sVAR_USERGOLDCOUNT);
            }
            else if (sLabel2 == SctiptDef.sVAR_MAXGOLDCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_MAXGOLDCOUNT);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_MAXGOLDCOUNT, SctiptDef.sVAR_MAXGOLDCOUNT);
            }
            else if (sLabel2 == SctiptDef.sVAR_STORAGEGOLDCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_STORAGEGOLDCOUNT);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_STORAGEGOLDCOUNT, SctiptDef.sVAR_STORAGEGOLDCOUNT);
            }
            else if (sLabel2 == SctiptDef.sVAR_BINDGOLDCOUNT)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_BINDGOLDCOUNT);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_BINDGOLDCOUNT, SctiptDef.sVAR_BINDGOLDCOUNT);
            }
            else if (sLabel2 == SctiptDef.sVAR_UPGRADEWEAPONFEE)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_UPGRADEWEAPONFEE);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_UPGRADEWEAPONFEE, SctiptDef.sVAR_UPGRADEWEAPONFEE);
            }
            else if (sLabel2 == SctiptDef.sVAR_USERWEAPON)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_USERWEAPON);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_USERWEAPON, SctiptDef.sVAR_USERWEAPON);
            }
            else if (sLabel2 == SctiptDef.sVAR_CMD_STARTQUEST)
            {
                sMsg = sMsg.Replace("<" + sLabel + ">", SctiptDef.tVAR_CMD_STARTQUEST);
                //M2Share.g_GrobalManage.Add(SctiptDef.nVAR_CMD_STARTQUEST, SctiptDef.sVAR_CMD_STARTQUEST);
            }
            //===========================================没有使用的========================================
            #endregion
            else if (HUtil32.CompareLStr(sLabel2, SctiptDef.sVAR_TEAM, SctiptDef.sVAR_TEAM.Length))
            {
                s14 = sLabel2.Substring(SctiptDef.sVAR_TEAM.Length + 1 - 1, 1);
                if (s14 != "")
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(SctiptDef.tVAR_TEAM, s14));
                }
                else
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, SctiptDef.sVAR_HUMAN, SctiptDef.sVAR_HUMAN.Length))
            {
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (s14 != "")
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(SctiptDef.tVAR_HUMAN, s14));
                }
                else
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, SctiptDef.sVAR_GUILD, SctiptDef.sVAR_GUILD.Length))
            {
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (s14 != "")
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(SctiptDef.tVAR_GUILD, s14));
                }
                else
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, SctiptDef.sVAR_GLOBAL, SctiptDef.sVAR_GLOBAL.Length))
            {
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (s14 != "")
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(SctiptDef.tVAR_GLOBAL, s14));
                }
                else
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, SctiptDef.sVAR_STR, SctiptDef.sVAR_STR.Length))
            {
                //'欢迎使用个人银行储蓄，目前完全免费，请多利用。\ \<您的个人银行存款有/@-1>：<$46><｜/@-2><$125/G18>\ \<您的包裹里以携带有/AUTOCOLOR=249>：<$GOLDCOUNT><｜/@-2><$GOLDCOUNTX>\ \ \<存入金币/@@InPutInteger1>      <取出金币/@@InPutInteger2>      <返 回/@Main>'
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (s14 != "")
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(SctiptDef.tVAR_STR, s14));
                }
                else
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", "????");
                }
            }
            else if (HUtil32.CompareLStr(sLabel2, SctiptDef.sVAR_MISSIONARITHMOMETER, SctiptDef.sVAR_MISSIONARITHMOMETER.Length))
            {
                HUtil32.ArrestStringEx(sLabel, "(", ")", ref s14);
                if (s14 != "")
                {
                    sMsg = sMsg.Replace("<" + sLabel + ">", string.Format(SctiptDef.tVAR_MISSIONARITHMOMETER, s14));
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
