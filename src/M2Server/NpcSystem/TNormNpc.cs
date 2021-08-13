using SystemModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SystemModule.Common;

namespace M2Server
{
    public partial class TNormNpc : TAnimalObject
    {
        public int n54C = 0;
        /// <summary>
        /// 用于标识此NPC是否有效，用于重新加载NPC列表(-1 为无效)
        /// </summary>
        public short m_nFlag = 0;
        public int[] FGotoLable;
        public IList<TScript> m_ScriptList = null;
        public string m_sFilePath = string.Empty;
        /// <summary>
        /// 此NPC是否是隐藏的，不显示在地图中
        /// </summary>
        public bool m_boIsHide = false;
        /// <summary>
        ///  NPC类型为地图任务型的，加载脚本时的脚本文件名为 角色名-地图号.txt
        /// </summary>
        public bool m_boIsQuest = false;
        protected string m_sPath = string.Empty;
        public IList<TScriptParams> BatchParamsList;

        private void ActionOfAddNameDateList(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sLineText;
            var sHumName = string.Empty;
            var sDate = string.Empty;
            var sListFileName = M2Share.g_Config.sEnvirDir + m_sPath + QuestActionInfo.sParam1;
            var LoadList = new StringList();
            if (File.Exists(sListFileName))
            {
                try
                {
                    LoadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    M2Share.MainOutMessage("loading fail.... => " + sListFileName);
                }
            }
            var boFound = false;
            for (var i = 0; i < LoadList.Count; i++)
            {
                sLineText = LoadList[i].Trim();
                sLineText = HUtil32.GetValidStr3(sLineText, ref sHumName, new string[] { " ", "\t" });
                sLineText = HUtil32.GetValidStr3(sLineText, ref sDate, new string[] { " ", "\t" });
                if (string.Compare(sHumName.ToLower(), PlayObject.m_sCharName.ToLower(), StringComparison.Ordinal) == 0)
                {
                    LoadList[i] = PlayObject.m_sCharName + "\t" + DateTime.Today;
                    boFound = true;
                    break;
                }
            }
            if (!boFound)
            {
                LoadList.Add(PlayObject.m_sCharName + "\t" + DateTime.Today);
            }
            try
            {
                LoadList.SaveToFile(sListFileName);
            }
            catch
            {
                M2Share.MainOutMessage("saving fail.... => " + sListFileName);
            }
            //LoadList.Free;
        }

        private void ActionOfDelNameDateList(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sLineText;
            string sHumName = string.Empty;
            string sDate = string.Empty;
            var sListFileName = M2Share.g_Config.sEnvirDir + m_sPath + QuestActionInfo.sParam1;
            var LoadList = new StringList();
            if (File.Exists(sListFileName))
            {
                try
                {
                    LoadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    M2Share.MainOutMessage("loading fail.... => " + sListFileName);
                }
            }
            var boFound = false;
            for (var i = 0; i < LoadList.Count; i++)
            {
                sLineText = LoadList[i].Trim();
                sLineText = HUtil32.GetValidStr3(sLineText, ref sHumName, new string[] { " ", "\t" });
                sLineText = HUtil32.GetValidStr3(sLineText, ref sDate, new string[] { " ", "\t" });
                if (sHumName.ToLower().CompareTo(PlayObject.m_sCharName.ToLower()) == 0)
                {
                    LoadList.RemoveAt(i);
                    boFound = true;
                    break;
                }
            }
            if (boFound)
            {
                try
                {
                    LoadList.SaveToFile(sListFileName);
                }
                catch
                {
                    M2Share.MainOutMessage("saving fail.... => " + sListFileName);
                }
            }
            //LoadList.Free;
        }

        private void ActionOfAddSkill(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nLevel = HUtil32._MIN(3, HUtil32.Str_ToInt(QuestActionInfo.sParam2, 0));
            var Magic = M2Share.UserEngine.FindMagic(QuestActionInfo.sParam1);
            if (Magic != null)
            {
                if (!PlayObject.IsTrainingSkill(Magic.wMagicID))
                {
                    var UserMagic = new TUserMagic();
                    UserMagic.MagicInfo = Magic;
                    UserMagic.wMagIdx = Magic.wMagicID;
                    UserMagic.btKey = 0;
                    UserMagic.btLevel = (byte)nLevel;
                    UserMagic.nTranPoint = 0;
                    PlayObject.m_MagicList.Add(UserMagic);
                    PlayObject.SendAddMagic(UserMagic);
                    PlayObject.RecalcAbilitys();
                    if (M2Share.g_Config.boShowScriptActionMsg)
                    {
                        PlayObject.SysMsg(Magic.sMagicName + "练习成功。", TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                }
            }
            else
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_ADDSKILL);
            }
        }

        private void ActionOfAutoAddGameGold(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo, int nPoint, int nTime)
        {
            if (QuestActionInfo.sParam1.ToLower().CompareTo("START".ToLower()) == 0)
            {
                if (nPoint > 0 && nTime > 0)
                {
                    PlayObject.m_nIncGameGold = nPoint;
                    PlayObject.m_dwIncGameGoldTime = nTime * 1000;
                    PlayObject.m_dwIncGameGoldTick = HUtil32.GetTickCount();
                    PlayObject.m_boIncGameGold = true;
                    return;
                }
            }
            if (QuestActionInfo.sParam1.ToLower().CompareTo("STOP".ToLower()) == 0)
            {
                PlayObject.m_boIncGameGold = false;
                return;
            }
            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_AUTOADDGAMEGOLD);
        }

        // SETAUTOGETEXP 时间 点数 是否安全区 地图号
        private void ActionOfAutoGetExp(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TEnvirnoment Envir = null;
            var nTime = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            var nPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            var boIsSafeZone = QuestActionInfo.sParam3[1] == '1';
            var sMap = QuestActionInfo.sParam4;
            if (sMap != "")
            {
                Envir = M2Share.g_MapManager.FindMap(sMap);
            }
            if ((nTime <= 0) || (nPoint <= 0) || ((sMap != "") && (Envir == null)))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_SETAUTOGETEXP);
                return;
            }
            PlayObject.m_boAutoGetExpInSafeZone = boIsSafeZone;
            PlayObject.m_AutoGetExpEnvir = Envir;
            PlayObject.m_nAutoGetExpTime = nTime * 1000;
            PlayObject.m_nAutoGetExpPoint = nPoint;
        }

        /// <summary>
        /// 增加挂机
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="QuestActionInfo"></param>
        private void ActionOfOffLine(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var sOffLineStartMsg = "系统已经为你开启了脱机泡点功能，你现在可以下线了……";
            PlayObject.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SYSMESSAGE, PlayObject.ObjectId, HUtil32.MakeWord(M2Share.g_Config.btCustMsgFColor, M2Share.g_Config.btCustMsgBColor), 0, 1);
            PlayObject.SendSocket(PlayObject.m_DefMsg, EDcode.EncodeString(sOffLineStartMsg));
            var nTime = HUtil32.Str_ToInt(QuestActionInfo.sParam1, 5);
            var nPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam2, 500);
            var nKickOffLine = HUtil32.Str_ToInt(QuestActionInfo.sParam3, 1440 * 15);
            PlayObject.m_boAutoGetExpInSafeZone = true;
            PlayObject.m_AutoGetExpEnvir = PlayObject.m_PEnvir;
            PlayObject.m_nAutoGetExpTime = nTime * 1000;
            PlayObject.m_nAutoGetExpPoint = nPoint;
            PlayObject.m_boOffLineFlag = true;
            PlayObject.m_dwKickOffLineTick = HUtil32.GetTickCount() + (nKickOffLine * 60 * 1000);
            IdSrvClient.Instance.SendHumanLogOutMsgA(PlayObject.m_sUserID, PlayObject.m_nSessionID);
            PlayObject.SendDefMessage(Grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0, "");
        }

        private void ActionOfAutoSubGameGold(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo, int nPoint, int nTime)
        {
            if (QuestActionInfo.sParam1.ToLower().CompareTo("START".ToLower()) == 0)
            {
                if ((nPoint > 0) && (nTime > 0))
                {
                    PlayObject.m_nDecGameGold = nPoint;
                    PlayObject.m_dwDecGameGoldTime = nTime * 1000;
                    PlayObject.m_dwDecGameGoldTick = 0;
                    PlayObject.m_boDecGameGold = true;
                    return;
                }
            } 
            if (QuestActionInfo.sParam1.ToLower().CompareTo("STOP".ToLower()) == 0)
            {
                PlayObject.m_boDecGameGold = false;
                return;
            }
            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_AUTOSUBGAMEGOLD);
        }

        private void ActionOfChangeCreditPoint(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nCreditPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nCreditPoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CREDITPOINT);
                return;
            }
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nCreditPoint >= 0)
                    {
                        if (nCreditPoint > byte.MaxValue)
                        {
                            PlayObject.m_btCreditPoint = byte.MaxValue;
                        }
                        else
                        {
                            PlayObject.m_btCreditPoint = (byte)nCreditPoint;
                        }
                    }
                    break;
                case '-':
                    if (PlayObject.m_btCreditPoint > (byte)nCreditPoint)
                    {
                        PlayObject.m_btCreditPoint -= (byte)nCreditPoint;
                    }
                    else
                    {
                        PlayObject.m_btCreditPoint = 0;
                    }
                    break;
                case '+':
                    if (PlayObject.m_btCreditPoint + (byte)nCreditPoint > byte.MaxValue)
                    {
                        PlayObject.m_btCreditPoint = byte.MaxValue;
                    }
                    else
                    {
                        PlayObject.m_btCreditPoint += (byte)nCreditPoint;
                    }
                    break;
                default:
                    ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CREDITPOINT);
                    return;
            }
        }

        private void ActionOfChangeExp(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int dwInt;
            var nExp = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nExp < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CHANGEEXP);
                return;
            }
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nExp >= 0)
                    {
                        PlayObject.m_Abil.Exp = nExp;
                        dwInt = nExp;
                    }
                    break;
                case '-':
                    if (PlayObject.m_Abil.Exp > nExp)
                    {
                        PlayObject.m_Abil.Exp -= nExp;
                    }
                    else
                    {
                        PlayObject.m_Abil.Exp = 0;
                    }
                    break;
                case '+':
                    if (PlayObject.m_Abil.Exp >= nExp)
                    {
                        if ((PlayObject.m_Abil.Exp - nExp) > (int.MaxValue - PlayObject.m_Abil.Exp))
                        {
                            dwInt = int.MaxValue - PlayObject.m_Abil.Exp;
                        }
                        else
                        {
                            dwInt = nExp;
                        }
                    }
                    else
                    {
                        if ((nExp - PlayObject.m_Abil.Exp) > (int.MaxValue - nExp))
                        {
                            dwInt = int.MaxValue - nExp;
                        }
                        else
                        {
                            dwInt = nExp;
                        }
                    }
                    PlayObject.m_Abil.Exp += dwInt;
                    // PlayObject.GetExp(dwInt);
                    PlayObject.SendMsg(PlayObject, Grobal2.RM_WINEXP, 0, dwInt, 0, 0, "");
                    break;
            }
        }

        private void ActionOfChangeHairStyle(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nHair = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            if ((QuestActionInfo.sParam1 != "") && (nHair >= 0))
            {
                PlayObject.m_btHair = (byte)nHair;
                PlayObject.FeatureChanged();
            }
            else
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_HAIRSTYLE);
            }
        }

        private void ActionOfChangeJob(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nJob = -1;
            if (HUtil32.CompareLStr(QuestActionInfo.sParam1, M2Share.sWarrior, M2Share.sWarrior.Length))
            {
                nJob = M2Share.jWarr;
            }
            if (HUtil32.CompareLStr(QuestActionInfo.sParam1, M2Share.sWizard, M2Share.sWizard.Length))
            {
                nJob = M2Share.jWizard;
            }
            if (HUtil32.CompareLStr(QuestActionInfo.sParam1, M2Share.sTaos, M2Share.sTaos.Length))
            {
                nJob = M2Share.jTaos;
            }
            if (nJob < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CHANGEJOB);
                return;
            }
            if (PlayObject.m_btJob != nJob)
            {
                PlayObject.m_btJob = (byte)nJob;
                // 
                // PlayObject.RecalcLevelAbilitys();
                // PlayObject.RecalcAbilitys();
                // 
                PlayObject.HasLevelUp(0);
            }
        }

        private void ActionOfChangeLevel(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nLv;
            var boChgOK = false;
            int nOldLevel = PlayObject.m_Abil.Level;
            var nLevel = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CHANGELEVEL);
                return;
            }
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if ((nLevel > 0) && (nLevel <= Grobal2.MAXLEVEL))
                    {
                        PlayObject.m_Abil.Level = (ushort)nLevel;
                        boChgOK = true;
                    }
                    break;
                case '-':
                    nLv = HUtil32._MAX(0, PlayObject.m_Abil.Level - nLevel);
                    nLv = HUtil32._MIN(Grobal2.MAXLEVEL, nLv);
                    PlayObject.m_Abil.Level = (ushort)nLv;
                    boChgOK = true;
                    break;
                case '+':
                    nLv = HUtil32._MAX(0, PlayObject.m_Abil.Level + nLevel);
                    nLv = HUtil32._MIN(Grobal2.MAXLEVEL, nLv);
                    PlayObject.m_Abil.Level = (ushort)nLv;
                    boChgOK = true;
                    break;
            }
            if (boChgOK)
            {
                PlayObject.HasLevelUp(nOldLevel);
            }
        }

        private void ActionOfChangePkPoint(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nPoint;
            var nOldPKLevel = PlayObject.PKLevel();
            var nPKPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nPKPoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CHANGEPKPOINT);
                return;
            }
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nPKPoint >= 0)
                    {
                        PlayObject.m_nPkPoint = nPKPoint;
                    }
                    break;
                case '-':
                    nPoint = HUtil32._MAX(0, PlayObject.m_nPkPoint - nPKPoint);
                    PlayObject.m_nPkPoint = nPoint;
                    break;
                case '+':
                    nPoint = HUtil32._MAX(0, PlayObject.m_nPkPoint + nPKPoint);
                    PlayObject.m_nPkPoint = nPoint;
                    break;
            }
            if (nOldPKLevel != PlayObject.PKLevel())
            {
                PlayObject.RefNameColor();
            }
        }

        private void ActionOfClearMapMon(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TBaseObject Mon;
            IList<TBaseObject> MonList = new List<TBaseObject>();
            M2Share.UserEngine.GetMapMonster(M2Share.g_MapManager.FindMap(QuestActionInfo.sParam1), MonList);
            for (var i = 0; i < MonList.Count; i++)
            {
                Mon = MonList[i];
                if (Mon.m_Master != null)
                {
                    continue;
                }
                if (M2Share.GetNoClearMonList(Mon.m_sCharName))
                {
                    continue;
                }
                Mon.m_boNoItem = true;
                Mon.MakeGhost();
            }
        }

        private void ActionOfClearNameList(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var sListFileName = M2Share.g_Config.sEnvirDir + m_sPath + QuestActionInfo.sParam1;
            var LoadList = new StringList();
            LoadList.Clear();
            try
            {
                LoadList.SaveToFile(sListFileName);
            }
            catch
            {
                M2Share.MainOutMessage("saving fail.... => " + sListFileName);
            }
            //LoadList.Free;
        }

        private void ActionOfClearSkill(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TUserMagic UserMagic;
            for (var i = PlayObject.m_MagicList.Count - 1; i >= 0; i--)
            {
                UserMagic = PlayObject.m_MagicList[i];
                PlayObject.SendDelMagic(UserMagic);
                Dispose(UserMagic);
                PlayObject.m_MagicList.RemoveAt(i);
            }
            PlayObject.RecalcAbilitys();
        }

        private void ActionOfDelNoJobSkill(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TUserMagic UserMagic;
            for (var i = PlayObject.m_MagicList.Count - 1; i >= 0; i--)
            {
                UserMagic = PlayObject.m_MagicList[i];
                if (UserMagic.MagicInfo.btJob != PlayObject.m_btJob)
                {
                    PlayObject.SendDelMagic(UserMagic);
                    Dispose(UserMagic);
                    PlayObject.m_MagicList.RemoveAt(i);
                }
            }
        }

        private void ActionOfDelSkill(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TUserMagic UserMagic;
            var sMagicName = QuestActionInfo.sParam1;
            var Magic = M2Share.UserEngine.FindMagic(sMagicName);
            if (Magic == null)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_DELSKILL);
                return;
            }
            for (var i = 0; i < PlayObject.m_MagicList.Count; i++)
            {
                UserMagic = PlayObject.m_MagicList[i];
                if (UserMagic.MagicInfo == Magic)
                {
                    PlayObject.m_MagicList.RemoveAt(i);
                    PlayObject.SendDelMagic(UserMagic);
                    Dispose(UserMagic);
                    PlayObject.RecalcAbilitys();
                    break;
                }
            }
        }

        private void ActionOfGameGold(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nOldGameGold = PlayObject.m_nGameGold;
            var nGameGold = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nGameGold < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_GAMEGOLD);
                return;
            }
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nGameGold >= 0)
                    {
                        PlayObject.m_nGameGold = nGameGold;
                    }
                    break;
                case '-':
                    nGameGold = HUtil32._MAX(0, PlayObject.m_nGameGold - nGameGold);
                    PlayObject.m_nGameGold = nGameGold;
                    break;
                case '+':
                    nGameGold = HUtil32._MAX(0, PlayObject.m_nGameGold + nGameGold);
                    PlayObject.m_nGameGold = nGameGold;
                    break;
            }
            if (M2Share.g_boGameLogGameGold)
            {
                M2Share.AddGameDataLog(format(M2Share.g_sGameLogMsg1, Grobal2.LOG_GAMEGOLD, PlayObject.m_sMapName, PlayObject.m_nCurrX, PlayObject.m_nCurrY, PlayObject.m_sCharName, M2Share.g_Config.sGameGoldName, nGameGold, cMethod, this.m_sCharName));
            }
            if (nOldGameGold != PlayObject.m_nGameGold)
            {
                PlayObject.GameGoldChanged();
            }
        }

        private void ActionOfGamePoint(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nOldGamePoint = PlayObject.m_nGamePoint;
            var nGamePoint = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nGamePoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_GAMEPOINT);
                return;
            }
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nGamePoint >= 0)
                    {
                        PlayObject.m_nGamePoint = nGamePoint;
                    }
                    break;
                case '-':
                    nGamePoint = HUtil32._MAX(0, PlayObject.m_nGamePoint - nGamePoint);
                    PlayObject.m_nGamePoint = nGamePoint;
                    break;
                case '+':
                    nGamePoint = HUtil32._MAX(0, PlayObject.m_nGamePoint + nGamePoint);
                    PlayObject.m_nGamePoint = nGamePoint;
                    break;
            }
            if (M2Share.g_boGameLogGamePoint)
            {
                M2Share.AddGameDataLog(format(M2Share.g_sGameLogMsg1, new object[] { Grobal2.LOG_GAMEPOINT, PlayObject.m_sMapName, PlayObject.m_nCurrX, PlayObject.m_nCurrY, PlayObject.m_sCharName, M2Share.g_Config.sGamePointName, nGamePoint, cMethod, this.m_sCharName }));
            }
            if (nOldGamePoint != PlayObject.m_nGamePoint)
            {
                PlayObject.GameGoldChanged();
            }
        }

        private void ActionOfGetMarry(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var PoseBaseObject = PlayObject.GetPoseCreate();
            if ((PoseBaseObject != null) && (PoseBaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT) && (PoseBaseObject.m_btGender != PlayObject.m_btGender))
            {
                PlayObject.m_sDearName = PoseBaseObject.m_sCharName;
                PlayObject.RefShowName();
                PoseBaseObject.RefShowName();
            }
            else
            {
                GotoLable(PlayObject, "@MarryError", false);
            }
        }

        private void ActionOfGetMaster(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var PoseBaseObject = PlayObject.GetPoseCreate();
            if ((PoseBaseObject != null) && (PoseBaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT) && (PoseBaseObject.m_btGender != PlayObject.m_btGender))
            {
                PlayObject.m_sMasterName = PoseBaseObject.m_sCharName;
                PlayObject.RefShowName();
                PoseBaseObject.RefShowName();
            }
            else
            {
                GotoLable(PlayObject, "@MasterError", false);
            }
        }

        private void ActionOfLineMsg(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var sMsg = GetLineVariableText(PlayObject, QuestActionInfo.sParam2);
            sMsg = sMsg.Replace("%s", PlayObject.m_sCharName);
            sMsg = sMsg.Replace("%d", this.m_sCharName);
            switch (QuestActionInfo.nParam1)
            {
                case 0:
                    M2Share.UserEngine.SendBroadCastMsg(sMsg, TMsgType.t_System);
                    break;
                case 1:
                    M2Share.UserEngine.SendBroadCastMsg("(*) " + sMsg, TMsgType.t_System);
                    break;
                case 2:
                    M2Share.UserEngine.SendBroadCastMsg('[' + this.m_sCharName + ']' + sMsg, TMsgType.t_System);
                    break;
                case 3:
                    M2Share.UserEngine.SendBroadCastMsg('[' + PlayObject.m_sCharName + ']' + sMsg, TMsgType.t_System);
                    break;
                case 4:
                    this.ProcessSayMsg(sMsg);
                    break;
                case 5:
                    PlayObject.SysMsg(sMsg, TMsgColor.c_Red, TMsgType.t_Say);
                    break;
                case 6:
                    PlayObject.SysMsg(sMsg, TMsgColor.c_Green, TMsgType.t_Say);
                    break;
                case 7:
                    PlayObject.SysMsg(sMsg, TMsgColor.c_Blue, TMsgType.t_Say);
                    break;
                case 8:
                    PlayObject.SendGroupText(sMsg);
                    break;
                case 9:
                    if (PlayObject.m_MyGuild != null)
                    {
                        PlayObject.m_MyGuild.SendGuildMsg(sMsg);
                        M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_208, M2Share.nServerIndex, PlayObject.m_MyGuild.sGuildName + "/" + PlayObject.m_sCharName + "/" + sMsg);
                    }
                    break;
                default:
                    ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSENDMSG);
                    break;
            }
        }

        private void ActionOfMapTing(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
        }

        private void ActionOfMarry(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sSayMsg;
            if (PlayObject.m_sDearName != "")
            {
                return;
            }
            var PoseHuman = (TPlayObject)PlayObject.GetPoseCreate();
            if (PoseHuman == null)
            {
                GotoLable(PlayObject, "@MarryCheckDir", false);
                return;
            }
            if (QuestActionInfo.sParam1 == "")
            {
                if (PoseHuman.m_btRaceServer != Grobal2.RC_PLAYOBJECT)
                {
                    GotoLable(PlayObject, "@HumanTypeErr", false);
                    return;
                }
                if (PoseHuman.GetPoseCreate() == PlayObject)
                {
                    if (PlayObject.m_btGender != PoseHuman.m_btGender)
                    {
                        GotoLable(PlayObject, "@StartMarry", false);
                        GotoLable(PoseHuman, "@StartMarry", false);
                        if ((PlayObject.m_btGender == ObjBase.gMan) && (PoseHuman.m_btGender == ObjBase.gWoMan))
                        {
                            sSayMsg = M2Share.g_sStartMarryManMsg.Replace("%n", this.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%s", PlayObject.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%d", PoseHuman.m_sCharName);
                            M2Share.UserEngine.SendBroadCastMsg(sSayMsg, TMsgType.t_Say);
                            sSayMsg = M2Share.g_sStartMarryManAskQuestionMsg.Replace("%n", this.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%s", PlayObject.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%d", PoseHuman.m_sCharName);
                            M2Share.UserEngine.SendBroadCastMsg(sSayMsg, TMsgType.t_Say);
                        }
                        else if ((PlayObject.m_btGender == ObjBase.gWoMan) && (PoseHuman.m_btGender == ObjBase.gMan))
                        {
                            sSayMsg = M2Share.g_sStartMarryWoManMsg.Replace("%n", this.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%s", PlayObject.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%d", PoseHuman.m_sCharName);
                            M2Share.UserEngine.SendBroadCastMsg(sSayMsg, TMsgType.t_Say);
                            sSayMsg = M2Share.g_sStartMarryWoManAskQuestionMsg.Replace("%n", this.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%s", PlayObject.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%d", PoseHuman.m_sCharName);
                            M2Share.UserEngine.SendBroadCastMsg(sSayMsg, TMsgType.t_Say);
                        }
                        PlayObject.m_boStartMarry = true;
                        PoseHuman.m_boStartMarry = true;
                    }
                    else
                    {
                        GotoLable(PoseHuman, "@MarrySexErr", false);
                        GotoLable(PlayObject, "@MarrySexErr", false);
                    }
                }
                else
                {
                    GotoLable(PlayObject, "@MarryDirErr", false);
                    GotoLable(PoseHuman, "@MarryCheckDir", false);
                }
                return;
            }
            // sREQUESTMARRY
            if (QuestActionInfo.sParam1.ToLower().CompareTo("REQUESTMARRY".ToLower()) == 0)
            {
                if (PlayObject.m_boStartMarry && PoseHuman.m_boStartMarry)
                {
                    if ((PlayObject.m_btGender == ObjBase.gMan) && (PoseHuman.m_btGender == ObjBase.gWoMan))
                    {
                        sSayMsg = M2Share.g_sMarryManAnswerQuestionMsg.Replace("%n", this.m_sCharName);
                        sSayMsg = sSayMsg.Replace("%s", PlayObject.m_sCharName);
                        sSayMsg = sSayMsg.Replace("%d", PoseHuman.m_sCharName);
                        M2Share.UserEngine.SendBroadCastMsg(sSayMsg, TMsgType.t_Say);
                        sSayMsg = M2Share.g_sMarryManAskQuestionMsg.Replace("%n", this.m_sCharName);
                        sSayMsg = sSayMsg.Replace("%s", PlayObject.m_sCharName);
                        sSayMsg = sSayMsg.Replace("%d", PoseHuman.m_sCharName);
                        M2Share.UserEngine.SendBroadCastMsg(sSayMsg, TMsgType.t_Say);
                        GotoLable(PlayObject, "@WateMarry", false);
                        GotoLable(PoseHuman, "@RevMarry", false);
                    }
                }
                return;
            }
            // sRESPONSEMARRY
            if (QuestActionInfo.sParam1.ToLower().CompareTo("RESPONSEMARRY".ToLower()) == 0)
            {
                if ((PlayObject.m_btGender == ObjBase.gWoMan) && (PoseHuman.m_btGender == ObjBase.gMan))
                {
                    if (QuestActionInfo.sParam2.ToLower().CompareTo("OK".ToLower()) == 0)
                    {
                        if (PlayObject.m_boStartMarry && PoseHuman.m_boStartMarry)
                        {
                            sSayMsg = M2Share.g_sMarryWoManAnswerQuestionMsg.Replace("%n", this.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%s", PlayObject.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%d", PoseHuman.m_sCharName);
                            M2Share.UserEngine.SendBroadCastMsg(sSayMsg, TMsgType.t_Say);
                            sSayMsg = M2Share.g_sMarryWoManGetMarryMsg.Replace("%n", this.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%s", PlayObject.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%d", PoseHuman.m_sCharName);
                            M2Share.UserEngine.SendBroadCastMsg(sSayMsg, TMsgType.t_Say);
                            GotoLable(PlayObject, "@EndMarry", false);
                            GotoLable(PoseHuman, "@EndMarry", false);
                            PlayObject.m_boStartMarry = false;
                            PoseHuman.m_boStartMarry = false;
                            PlayObject.m_sDearName = PoseHuman.m_sCharName;
                            PlayObject.m_DearHuman = PoseHuman;
                            PoseHuman.m_sDearName = PlayObject.m_sCharName;
                            PoseHuman.m_DearHuman = PlayObject;
                            PlayObject.RefShowName();
                            PoseHuman.RefShowName();
                        }
                    }
                    else
                    {
                        if (PlayObject.m_boStartMarry && PoseHuman.m_boStartMarry)
                        {
                            GotoLable(PlayObject, "@EndMarryFail", false);
                            GotoLable(PoseHuman, "@EndMarryFail", false);
                            PlayObject.m_boStartMarry = false;
                            PoseHuman.m_boStartMarry = false;
                            sSayMsg = M2Share.g_sMarryWoManDenyMsg.Replace("%n", this.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%s", PlayObject.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%d", PoseHuman.m_sCharName);
                            M2Share.UserEngine.SendBroadCastMsg(sSayMsg, TMsgType.t_Say);
                            sSayMsg = M2Share.g_sMarryWoManCancelMsg.Replace("%n", this.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%s", PlayObject.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%d", PoseHuman.m_sCharName);
                            M2Share.UserEngine.SendBroadCastMsg(sSayMsg, TMsgType.t_Say);
                        }
                    }
                }
                return;
            }
        }

        private void ActionOfMaster(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            if (PlayObject.m_sMasterName != "")
            {
                return;
            }
            var PoseHuman = (TPlayObject)PlayObject.GetPoseCreate();
            if (PoseHuman == null)
            {
                GotoLable(PlayObject, "@MasterCheckDir", false);
                return;
            }
            if (QuestActionInfo.sParam1 == "")
            {
                if (PoseHuman.m_btRaceServer != Grobal2.RC_PLAYOBJECT)
                {
                    GotoLable(PlayObject, "@HumanTypeErr", false);
                    return;
                }
                if (PoseHuman.GetPoseCreate() == PlayObject)
                {
                    GotoLable(PlayObject, "@StartGetMaster", false);
                    GotoLable(PoseHuman, "@StartMaster", false);
                    PlayObject.m_boStartMaster = true;
                    PoseHuman.m_boStartMaster = true;
                }
                else
                {
                    GotoLable(PlayObject, "@MasterDirErr", false);
                    GotoLable(PoseHuman, "@MasterCheckDir", false);
                }
                return;
            }
            if (QuestActionInfo.sParam1.ToLower().CompareTo("REQUESTMASTER".ToLower()) == 0)
            {
                if (PlayObject.m_boStartMaster && PoseHuman.m_boStartMaster)
                {
                    PlayObject.m_PoseBaseObject = PoseHuman;
                    PoseHuman.m_PoseBaseObject = PlayObject;
                    GotoLable(PlayObject, "@WateMaster", false);
                    GotoLable(PoseHuman, "@RevMaster", false);
                }
                return;
            }
            if (QuestActionInfo.sParam1.ToLower().CompareTo("RESPONSEMASTER".ToLower()) == 0)
            {
                if (QuestActionInfo.sParam2.ToLower().CompareTo("OK".ToLower()) == 0)
                {
                    if ((PlayObject.m_PoseBaseObject == PoseHuman) && (PoseHuman.m_PoseBaseObject == PlayObject))
                    {
                        if (PlayObject.m_boStartMaster && PoseHuman.m_boStartMaster)
                        {
                            GotoLable(PlayObject, "@EndMaster", false);
                            GotoLable(PoseHuman, "@EndMaster", false);
                            PlayObject.m_boStartMaster = false;
                            PoseHuman.m_boStartMaster = false;
                            if (PlayObject.m_sMasterName == "")
                            {
                                PlayObject.m_sMasterName = PoseHuman.m_sCharName;
                                PlayObject.m_boMaster = true;
                            }
                            PlayObject.m_MasterList.Add(PoseHuman);
                            PoseHuman.m_sMasterName = PlayObject.m_sCharName;
                            PoseHuman.m_boMaster = false;
                            PlayObject.RefShowName();
                            PoseHuman.RefShowName();
                        }
                    }
                }
                else
                {
                    if (PlayObject.m_boStartMaster && PoseHuman.m_boStartMaster)
                    {
                        GotoLable(PlayObject, "@EndMasterFail", false);
                        GotoLable(PoseHuman, "@EndMasterFail", false);
                        PlayObject.m_boStartMaster = false;
                        PoseHuman.m_boStartMaster = false;
                    }
                }
                return;
            }
        }

        private void ActionOfMessageBox(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, PlayObject.ObjectId, 0, 0, GetLineVariableText(PlayObject, QuestActionInfo.sParam1));
        }

        private void ActionOfMission(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            if ((QuestActionInfo.sParam1 != "") && (QuestActionInfo.nParam2 > 0) && (QuestActionInfo.nParam3 > 0))
            {
                M2Share.g_sMissionMap = QuestActionInfo.sParam1;
                M2Share.g_nMissionX = (short)QuestActionInfo.nParam2;
                M2Share.g_nMissionY = (short)QuestActionInfo.nParam3;
            }
            else
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_MISSION);
            }
        }

        // MOBFIREBURN MAP X Y TYPE TIME POINT
        private void ActionOfMobFireBurn(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var sMap = QuestActionInfo.sParam1;
            var nX = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            var nY = HUtil32.Str_ToInt(QuestActionInfo.sParam3, -1);
            var nType = HUtil32.Str_ToInt(QuestActionInfo.sParam4, -1);
            var nTime = HUtil32.Str_ToInt(QuestActionInfo.sParam5, -1);
            var nPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam6, -1);
            if ((sMap == "") || (nX < 0) || (nY < 0) || (nType < 0) || (nTime < 0) || (nPoint < 0))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_MOBFIREBURN);
                return;
            }
            var Envir = M2Share.g_MapManager.FindMap(sMap);
            if (Envir != null)
            {
                var OldEnvir = PlayObject.m_PEnvir;
                PlayObject.m_PEnvir = Envir;
                var FireBurnEvent = new TFireBurnEvent(PlayObject, nX, nY, nType, nTime * 1000, nPoint);
                M2Share.EventManager.AddEvent(FireBurnEvent);
                PlayObject.m_PEnvir = OldEnvir;
                return;
            }
            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_MOBFIREBURN);
        }

        private void ActionOfMobPlace(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo, int nX, int nY, int nCount, int nRange)
        {
            short nRandX;
            short nRandY;
            TBaseObject Mon;
            for (var i = 0; i < nCount; i++)
            {
                nRandX = (short)(M2Share.RandomNumber.Random(nRange * 2 + 1) + (nX - nRange));
                nRandY = (short)(M2Share.RandomNumber.Random(nRange * 2 + 1) + (nY - nRange));
                Mon = M2Share.UserEngine.RegenMonsterByName(M2Share.g_sMissionMap, nRandX, nRandY, QuestActionInfo.sParam1);
                if (Mon != null)
                {
                    Mon.m_boMission = true;
                    Mon.m_nMissionX = M2Share.g_nMissionX;
                    Mon.m_nMissionY = M2Share.g_nMissionY;
                }
                else
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_MOBPLACE);
                    break;
                }
            }
        }

        private void ActionOfRecallGroupMembers(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
        }

        private void ActionOfSetRankLevelName(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var sRankLevelName = QuestActionInfo.sParam1;
            if (sRankLevelName == "")
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_SKILLLEVEL);
                return;
            }
            PlayObject.m_sRankLevelName = sRankLevelName;
            PlayObject.RefShowName();
        }

        private void ActionOfSetScriptFlag(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nWhere = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            var boFlag = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1) == 1;
            switch (nWhere)
            {
                case 0:
                    PlayObject.m_boSendMsgFlag = boFlag;
                    break;
                case 1:
                    PlayObject.m_boChangeItemNameFlag = boFlag;
                    break;
                default:
                    ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_SETSCRIPTFLAG);
                    break;
            }
        }

        private void ActionOfSkillLevel(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TUserMagic UserMagic;
            var nLevel = HUtil32.Str_ToInt(QuestActionInfo.sParam3, 0);
            if (nLevel < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_SKILLLEVEL);
                return;
            }
            var cMethod = QuestActionInfo.sParam2[0];
            var Magic = M2Share.UserEngine.FindMagic(QuestActionInfo.sParam1);
            if (Magic != null)
            {
                for (var i = 0; i < PlayObject.m_MagicList.Count; i++)
                {
                    UserMagic = PlayObject.m_MagicList[i];
                    if (UserMagic.MagicInfo == Magic)
                    {
                        switch (cMethod)
                        {
                            case '=':
                                if (nLevel >= 0)
                                {
                                    nLevel = HUtil32._MAX(3, nLevel);
                                    UserMagic.btLevel = (byte)nLevel;
                                }
                                break;
                            case '-':
                                if (UserMagic.btLevel >= nLevel)
                                {
                                    UserMagic.btLevel -= (byte)nLevel;
                                }
                                else
                                {
                                    UserMagic.btLevel = 0;
                                }
                                break;
                            case '+':
                                if (UserMagic.btLevel + nLevel <= 3)
                                {
                                    UserMagic.btLevel += (byte)nLevel;
                                }
                                else
                                {
                                    UserMagic.btLevel = 3;
                                }
                                break;
                        }
                        PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_MAGIC_LVEXP, 0, UserMagic.MagicInfo.wMagicID, UserMagic.btLevel, UserMagic.nTranPoint, "", 100);
                        break;
                    }
                }
            }
        }

        private void ActionOfTakeCastleGold(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nGold = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            if ((nGold < 0) || (this.m_Castle == null))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_TAKECASTLEGOLD);
                return;
            }
            if (nGold <= this.m_Castle.m_nTotalGold)
            {
                this.m_Castle.m_nTotalGold -= nGold;
            }
            else
            {
                this.m_Castle.m_nTotalGold = 0;
            }
        }

        private void ActionOfUnMarry(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            if (PlayObject.m_sDearName == "")
            {
                GotoLable(PlayObject, "@ExeMarryFail", false);
                return;
            }
            var PoseHuman = (TPlayObject)PlayObject.GetPoseCreate();
            if (PoseHuman == null)
            {
                GotoLable(PlayObject, "@UnMarryCheckDir", false);
            }
            if (PoseHuman != null)
            {
                if (QuestActionInfo.sParam1 == "")
                {
                    if (PoseHuman.m_btRaceServer != Grobal2.RC_PLAYOBJECT)
                    {
                        GotoLable(PlayObject, "@UnMarryTypeErr", false);
                        return;
                    }
                    if (PoseHuman.GetPoseCreate() == PlayObject)
                    {
                        // and (PosHum.AddInfo.sDearName = Hum.sName)
                        if (PlayObject.m_sDearName == PoseHuman.m_sCharName)
                        {
                            GotoLable(PlayObject, "@StartUnMarry", false);
                            GotoLable(PoseHuman, "@StartUnMarry", false);
                            return;
                        }
                    }
                }
            }
            // sREQUESTUNMARRY
            if (QuestActionInfo.sParam1.ToLower().CompareTo("REQUESTUNMARRY".ToLower()) == 0)
            {
                if (QuestActionInfo.sParam2 == "")
                {
                    if (PoseHuman != null)
                    {
                        PlayObject.m_boStartUnMarry = true;
                        if (PlayObject.m_boStartUnMarry && PoseHuman.m_boStartUnMarry)
                        {
                            // sUnMarryMsg8
                            // sMarryMsg0
                            // sUnMarryMsg9
                            M2Share.UserEngine.SendBroadCastMsg('[' + this.m_sCharName + "]: " + "我宣布" + PoseHuman.m_sCharName + ' ' + '与' + PlayObject.m_sCharName + ' ' + ' ' + "正式脱离夫妻关系。", TMsgType.t_Say);
                            PlayObject.m_sDearName = "";
                            PoseHuman.m_sDearName = "";
                            PlayObject.m_btMarryCount++;
                            PoseHuman.m_btMarryCount++;
                            PlayObject.m_boStartUnMarry = false;
                            PoseHuman.m_boStartUnMarry = false;
                            PlayObject.RefShowName();
                            PoseHuman.RefShowName();
                            GotoLable(PlayObject, "@UnMarryEnd", false);
                            GotoLable(PoseHuman, "@UnMarryEnd", false);
                        }
                        else
                        {
                            GotoLable(PlayObject, "@WateUnMarry", false);
                            // GotoLable(PoseHuman,'@RevUnMarry',False);
                        }
                    }
                    return;
                }
                else
                {
                    // 强行离婚
                    if (QuestActionInfo.sParam2.ToLower().CompareTo("FORCE".ToLower()) == 0)
                    {
                        M2Share.UserEngine.SendBroadCastMsg('[' + this.m_sCharName + "]: " + "我宣布" + PlayObject.m_sCharName + ' ' + '与' + PlayObject.m_sDearName + ' ' + ' ' + "已经正式脱离夫妻关系！！！", TMsgType.t_Say);
                        PoseHuman = M2Share.UserEngine.GetPlayObject(PlayObject.m_sDearName);
                        if (PoseHuman != null)
                        {
                            PoseHuman.m_sDearName = "";
                            PoseHuman.m_btMarryCount++;
                            PoseHuman.RefShowName();
                        }
                        else
                        {
                            //sUnMarryFileName = M2Share.g_Config.sEnvirDir + "UnMarry.txt";
                            //LoadList = new StringList();
                            //if (File.Exists(sUnMarryFileName))
                            //{
                            //    LoadList.LoadFromFile(sUnMarryFileName);
                            //}
                            //LoadList.Add(PlayObject.m_sDearName);
                            //LoadList.SaveToFile(sUnMarryFileName);
                            //LoadList.Free;
                        }
                        PlayObject.m_sDearName = "";
                        PlayObject.m_btMarryCount++;
                        GotoLable(PlayObject, "@UnMarryEnd", false);
                        PlayObject.RefShowName();
                    }
                    return;
                }
            }
        }

        public virtual void ClearScript()
        {
            //for (I = 0; I < m_ScriptList.Count; I ++ )
            //{
            //    Script = m_ScriptList[I];
            //    for (II = 0; II < Script.RecordList.Count; II ++ )
            //    {
            //        SayingRecord = Script.RecordList[II];
            //        for (III = 0; III < SayingRecord.ProcedureList.Count; III ++ )
            //        {
            //            SayingProcedure = SayingRecord.ProcedureList[III];
            //            for (IIII = 0; IIII < SayingProcedure.ConditionList.Count; IIII ++ )
            //            {
            //                Dispose(((SayingProcedure.ConditionList[IIII]) as TQuestConditionInfo));
            //            }
            //            for (IIII = 0; IIII < SayingProcedure.ActionList.Count; IIII ++ )
            //            {
            //                Dispose(((SayingProcedure.ActionList[IIII]) as TQuestActionInfo));
            //            }
            //            for (IIII = 0; IIII < SayingProcedure.ElseActionList.Count; IIII ++ )
            //            {
            //                Dispose(((SayingProcedure.ElseActionList[IIII]) as TQuestActionInfo));
            //            }
            //            //SayingProcedure.ConditionList.Free;
            //            //SayingProcedure.ActionList.Free;
            //            //SayingProcedure.ElseActionList.Free;
            //            Dispose(SayingProcedure);
            //        }
            //        //SayingRecord.ProcedureList.Free;
            //        Dispose(SayingRecord);
            //    }
            //    //Script.RecordList.Free;
            //    Dispose(Script);
            //}
            m_ScriptList.Clear();
        }

        public virtual void Click(TPlayObject PlayObject)
        {
            PlayObject.m_nScriptGotoCount = 0;
            PlayObject.m_sScriptGoBackLable = "";
            PlayObject.m_sScriptCurrLable = "";
            GotoLable(PlayObject, "@main", false);
        }

        private bool ConditionOfCheckAccountIPList(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            StringList LoadList;
            string sLine;
            string sName = string.Empty;
            string sIPaddr;
            try
            {
                var sCharName = PlayObject.m_sCharName;
                var sCharAccount = PlayObject.m_sUserID;
                var sCharIPaddr = PlayObject.m_sIPaddr;
                LoadList = new StringList();
                if (File.Exists(M2Share.g_Config.sEnvirDir + QuestConditionInfo.sParam1))
                {
                    LoadList.LoadFromFile(M2Share.g_Config.sEnvirDir + QuestConditionInfo.sParam1);
                    for (var i = 0; i < LoadList.Count; i++)
                    {
                        sLine = LoadList[i];
                        if (sLine[1] == ';')
                        {
                            continue;
                        }
                        sIPaddr = HUtil32.GetValidStr3(sLine, ref sName, new string[] { " ", "/", "\t" });
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
                    ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKACCOUNTIPLIST);
                }
            }
            finally
            {
                //LoadList.Free;
            }
            return result;
        }

        private bool ConditionOfCheckBagSize(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nSize = QuestConditionInfo.nParam1;
            if ((nSize <= 0) || (nSize > Grobal2.MAXBAGITEM))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKBAGSIZE);
                return result;
            }
            if (PlayObject.m_ItemList.Count + nSize <= Grobal2.MAXBAGITEM)
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckBonusPoint(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nTotlePoint = this.m_BonusAbil.DC + this.m_BonusAbil.MC + this.m_BonusAbil.SC + this.m_BonusAbil.AC + this.m_BonusAbil.MAC + this.m_BonusAbil.HP + this.m_BonusAbil.MP + this.m_BonusAbil.Hit + this.m_BonusAbil.Speed + this.m_BonusAbil.X2;
            nTotlePoint = nTotlePoint + this.m_nBonusPoint;
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

        private bool ConditionOfCheckHP_CheckHigh(TPlayObject PlayObject, char cMethodMax, int nMax)
        {
            var result = false;
            switch (cMethodMax)
            {
                case '=':
                    if (PlayObject.m_WAbil.MaxHP == nMax)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_WAbil.MaxHP > nMax)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_WAbil.MaxHP < nMax)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_WAbil.MaxHP >= nMax)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckHP(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var cMethodMin = QuestConditionInfo.sParam1[0];
            var cMethodMax = QuestConditionInfo.sParam1[2];
            var nMin = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            var nMax = HUtil32.Str_ToInt(QuestConditionInfo.sParam4, -1);
            if ((nMin < 0) || (nMax < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKHP);
                return result;
            }
            switch (cMethodMin)
            {
                case '=':
                    if (this.m_WAbil.HP == nMin)
                    {
                        result = ConditionOfCheckHP_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '>':
                    if (PlayObject.m_WAbil.HP > nMin)
                    {
                        result = ConditionOfCheckHP_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '<':
                    if (PlayObject.m_WAbil.HP < nMin)
                    {
                        result = ConditionOfCheckHP_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                default:
                    if (PlayObject.m_WAbil.HP >= nMin)
                    {
                        result = ConditionOfCheckHP_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckMP_CheckHigh(TPlayObject PlayObject, char cMethodMax, int nMax)
        {
            var result = false;
            switch (cMethodMax)
            {
                case '=':
                    if (PlayObject.m_WAbil.MaxMP == nMax)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_WAbil.MaxMP > nMax)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_WAbil.MaxMP < nMax)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_WAbil.MaxMP >= nMax)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckMP(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var cMethodMin = QuestConditionInfo.sParam1[0];
            var cMethodMax = QuestConditionInfo.sParam1[2];
            var nMin = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            var nMax = HUtil32.Str_ToInt(QuestConditionInfo.sParam4, -1);
            if ((nMin < 0) || (nMax < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKMP);
                return result;
            }
            switch (cMethodMin)
            {
                case '=':
                    if (this.m_WAbil.MP == nMin)
                    {
                        result = ConditionOfCheckMP_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '>':
                    if (PlayObject.m_WAbil.MP > nMin)
                    {
                        result = ConditionOfCheckMP_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '<':
                    if (PlayObject.m_WAbil.MP < nMin)
                    {
                        result = ConditionOfCheckMP_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                default:
                    if (PlayObject.m_WAbil.MP >= nMin)
                    {
                        result = ConditionOfCheckMP_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckDC_CheckHigh(TPlayObject PlayObject, char cMethodMax, int nMax)
        {
            var result = false;
            switch (cMethodMax)
            {
                case '=':

                    if (HUtil32.HiWord(PlayObject.m_WAbil.DC) == nMax)
                    {
                        result = true;
                    }
                    break;
                case '>':

                    if (HUtil32.HiWord(PlayObject.m_WAbil.DC) > nMax)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (HUtil32.HiWord(PlayObject.m_WAbil.DC) < nMax)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (HUtil32.HiWord(PlayObject.m_WAbil.DC) >= nMax)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckDC(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var cMethodMin = QuestConditionInfo.sParam1[0];
            var cMethodMax = QuestConditionInfo.sParam1[2];
            var nMin = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            var nMax = HUtil32.Str_ToInt(QuestConditionInfo.sParam4, -1);
            if ((nMin < 0) || (nMax < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKDC);
                return result;
            }
            switch (cMethodMin)
            {
                case '=':
                    if (HUtil32.LoWord(PlayObject.m_WAbil.DC) == nMin)
                    {
                        result = ConditionOfCheckDC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '>':
                    if (HUtil32.LoWord(PlayObject.m_WAbil.DC) > nMin)
                    {
                        result = ConditionOfCheckDC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '<':
                    if (HUtil32.LoWord(PlayObject.m_WAbil.DC) < nMin)
                    {
                        result = ConditionOfCheckDC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                default:
                    if (HUtil32.LoWord(PlayObject.m_WAbil.DC) >= nMin)
                    {
                        result = ConditionOfCheckDC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
            }
            result = false;
            return result;
        }

        private bool ConditionOfCheckMC_CheckHigh(TPlayObject PlayObject, char cMethodMax, int nMax)
        {
            var result = false;
            switch (cMethodMax)
            {
                case '=':
                    if (HUtil32.HiWord(PlayObject.m_WAbil.MC) == nMax)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (HUtil32.HiWord(PlayObject.m_WAbil.MC) > nMax)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (HUtil32.HiWord(PlayObject.m_WAbil.MC) < nMax)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (HUtil32.HiWord(PlayObject.m_WAbil.MC) >= nMax)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckMC(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var cMethodMin = QuestConditionInfo.sParam1[0];
            var cMethodMax = QuestConditionInfo.sParam1[2];
            var nMin = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            var nMax = HUtil32.Str_ToInt(QuestConditionInfo.sParam4, -1);
            if ((nMin < 0) || (nMax < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKMC);
                return result;
            }
            switch (cMethodMin)
            {
                case '=':
                    if (HUtil32.LoWord(PlayObject.m_WAbil.MC) == nMin)
                    {
                        result = ConditionOfCheckMC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '>':

                    if (HUtil32.LoWord(PlayObject.m_WAbil.MC) > nMin)
                    {
                        result = ConditionOfCheckMC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '<':
                    if (HUtil32.LoWord(PlayObject.m_WAbil.MC) < nMin)
                    {
                        result = ConditionOfCheckMC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                default:
                    if (HUtil32.LoWord(PlayObject.m_WAbil.MC) >= nMin)
                    {
                        result = ConditionOfCheckMC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckSC_CheckHigh(TPlayObject PlayObject, char cMethodMax, int nMax)
        {
            var result = false;
            switch (cMethodMax)
            {
                case '=':
                    if (HUtil32.HiWord(PlayObject.m_WAbil.SC) == nMax)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (HUtil32.HiWord(PlayObject.m_WAbil.SC) > nMax)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (HUtil32.HiWord(PlayObject.m_WAbil.SC) < nMax)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (HUtil32.HiWord(PlayObject.m_WAbil.SC) >= nMax)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckSC(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var cMethodMin = QuestConditionInfo.sParam1[0];
            var cMethodMax = QuestConditionInfo.sParam1[2];
            var nMin = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            var nMax = HUtil32.Str_ToInt(QuestConditionInfo.sParam4, -1);
            if ((nMin < 0) || (nMax < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKSC);
                return result;
            }
            switch (cMethodMin)
            {
                case '=':

                    if (HUtil32.LoWord(PlayObject.m_WAbil.SC) == nMin)
                    {
                        result = ConditionOfCheckSC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '>':

                    if (HUtil32.LoWord(PlayObject.m_WAbil.SC) > nMin)
                    {
                        result = ConditionOfCheckSC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                case '<':

                    if (HUtil32.LoWord(PlayObject.m_WAbil.SC) < nMin)
                    {
                        result = ConditionOfCheckSC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
                default:

                    if (HUtil32.LoWord(PlayObject.m_WAbil.SC) >= nMin)
                    {
                        result = ConditionOfCheckSC_CheckHigh(PlayObject, cMethodMax, nMax);
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckExp(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var dwExp = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, 0);
            if (dwExp == 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKEXP);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_Abil.Exp == dwExp)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_Abil.Exp > dwExp)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_Abil.Exp < dwExp)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_Abil.Exp >= dwExp)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckFlourishPoint(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nPoint = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nPoint < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKFLOURISHPOINT);
                return result;
            }
            if (PlayObject.m_MyGuild == null)
            {
                return result;
            }
            var Guild = PlayObject.m_MyGuild;
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (Guild.nFlourishing == nPoint)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (Guild.nFlourishing > nPoint)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (Guild.nFlourishing < nPoint)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (Guild.nFlourishing >= nPoint)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckChiefItemCount(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nCount = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKFLOURISHPOINT);
                return result;
            }
            if (PlayObject.m_MyGuild == null)
            {
                return result;
            }
            var Guild = PlayObject.m_MyGuild;
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (Guild.nChiefItemCount == nCount)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (Guild.nChiefItemCount > nCount)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (Guild.nChiefItemCount < nCount)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (Guild.nChiefItemCount >= nCount)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckGuildAuraePoint(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nPoint = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nPoint < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKAURAEPOINT);
                return result;
            }
            if (PlayObject.m_MyGuild == null)
            {
                return result;
            }
            var Guild = PlayObject.m_MyGuild;
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (Guild.nAurae == nPoint)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (Guild.nAurae > nPoint)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (Guild.nAurae < nPoint)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (Guild.nAurae >= nPoint)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckGuildBuildPoint(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nPoint = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nPoint < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKBUILDPOINT);
                return result;
            }
            if (PlayObject.m_MyGuild == null)
            {
                return result;
            }
            var Guild = PlayObject.m_MyGuild;
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (Guild.nBuildPoint == nPoint)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (Guild.nBuildPoint > nPoint)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (Guild.nBuildPoint < nPoint)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (Guild.nBuildPoint >= nPoint)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckStabilityPoint(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nPoint = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nPoint < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKSTABILITYPOINT);
                return result;
            }
            if (PlayObject.m_MyGuild == null)
            {
                return result;
            }
            var Guild = PlayObject.m_MyGuild;
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (Guild.nStability == nPoint)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (Guild.nStability > nPoint)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (Guild.nStability < nPoint)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (Guild.nStability >= nPoint)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckGameGold(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nGameGold = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nGameGold < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKGAMEGOLD);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_nGameGold == nGameGold)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_nGameGold > nGameGold)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_nGameGold < nGameGold)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_nGameGold >= nGameGold)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckGamePoint(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nGamePoint = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nGamePoint < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKGAMEPOINT);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_nGamePoint == nGamePoint)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_nGamePoint > nGamePoint)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_nGamePoint < nGamePoint)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_nGamePoint >= nGamePoint)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckGroupCount(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if (PlayObject.m_GroupOwner == null)
            {
                return result;
            }
            var nCount = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKGROUPCOUNT);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_GroupOwner.m_GroupMembers.Count == nCount)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_GroupOwner.m_GroupMembers.Count > nCount)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_GroupOwner.m_GroupMembers.Count < nCount)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_GroupOwner.m_GroupMembers.Count >= nCount)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfIsHigh(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if (QuestConditionInfo.sParam1 == "")
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_ISHIGH);
                return result;
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
                    ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_ISHIGH);
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckHaveGuild(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            return PlayObject.m_MyGuild != null;
        }

        private bool ConditionOfCheckInMapRange(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var sMapName = QuestConditionInfo.sParam1;
            var nX = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            var nY = HUtil32.Str_ToInt(QuestConditionInfo.sParam3, -1);
            var nRange = HUtil32.Str_ToInt(QuestConditionInfo.sParam4, -1);
            if ((sMapName == "") || (nX < 0) || (nY < 0) || (nRange < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKINMAPRANGE);
                return result;
            }
            if (PlayObject.m_sMapName.ToLower().CompareTo(sMapName.ToLower()) != 0)
            {
                return result;
            }
            if ((Math.Abs(PlayObject.m_nCurrX - nX) <= nRange) && (Math.Abs(PlayObject.m_nCurrY - nY) <= nRange))
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckIsAttackGuild(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if (this.m_Castle == null)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_ISATTACKGUILD);
                return result;
            }
            if (PlayObject.m_MyGuild == null)
            {
                return result;
            }
            result = this.m_Castle.IsAttackGuild(PlayObject.m_MyGuild);
            return result;
        }

        private bool ConditionOfCheckCastleChageDay(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nDay = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if ((nDay < 0) || (this.m_Castle == null))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CASTLECHANGEDAY);
                return result;
            }
            var nChangeDay = HUtil32.GetDayCount(DateTime.Now, this.m_Castle.m_ChangeDate);
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

        private bool ConditionOfCheckCastleWarDay(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nDay = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if ((nDay < 0) || (this.m_Castle == null))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CASTLEWARDAY);
                return result;
            }
            var nWarDay = HUtil32.GetDayCount(DateTime.Now, this.m_Castle.m_WarDate);
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

        private bool ConditionOfCheckCastleDoorStatus(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nDay = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            var nDoorStatus = -1;
            if (QuestConditionInfo.sParam1.ToLower().CompareTo("损坏".ToLower()) == 0)
            {
                nDoorStatus = 0;
            }
            if (QuestConditionInfo.sParam1.ToLower().CompareTo("开启".ToLower()) == 0)
            {
                nDoorStatus = 1;
            }
            if (QuestConditionInfo.sParam1.ToLower().CompareTo("关闭".ToLower()) == 0)
            {
                nDoorStatus = 2;
            }
            if ((nDay < 0) || (this.m_Castle == null) || (nDoorStatus < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKCASTLEDOOR);
                return result;
            }
            var CastleDoor = (TCastleDoor)this.m_Castle.m_MainDoor.BaseObject;
            switch (nDoorStatus)
            {
                case 0:
                    if (CastleDoor.m_boDeath)
                    {
                        result = true;
                    }
                    break;
                case 1:
                    if (CastleDoor.m_boOpened)
                    {
                        result = true;
                    }
                    break;
                case 2:
                    if (!CastleDoor.m_boOpened)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckIsAttackAllyGuild(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if (this.m_Castle == null)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_ISATTACKALLYGUILD);
                return result;
            }
            if (PlayObject.m_MyGuild == null)
            {
                return result;
            }
            result = this.m_Castle.IsAttackAllyGuild(PlayObject.m_MyGuild);
            return result;
        }

        private bool ConditionOfCheckIsDefenseAllyGuild(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if (this.m_Castle == null)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_ISDEFENSEALLYGUILD);
                return result;
            }
            if (PlayObject.m_MyGuild == null)
            {
                return result;
            }
            result = this.m_Castle.IsDefenseAllyGuild(PlayObject.m_MyGuild);
            return result;
        }

        private bool ConditionOfCheckIsDefenseGuild(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if (this.m_Castle == null)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_ISDEFENSEGUILD);
                return result;
            }
            if (PlayObject.m_MyGuild == null)
            {
                return result;
            }
            result = this.m_Castle.IsDefenseGuild(PlayObject.m_MyGuild);
            return result;
        }

        private bool ConditionOfCheckIsCastleaGuild(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            // if (PlayObject.m_MyGuild <> nil) and (UserCastle.m_MasterGuild = PlayObject.m_MyGuild) then
            if (M2Share.CastleManager.IsCastleMember(PlayObject) != null)
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckIsCastleMaster(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            // if PlayObject.IsGuildMaster and (UserCastle.m_MasterGuild = PlayObject.m_MyGuild) then
            if (PlayObject.IsGuildMaster() && (M2Share.CastleManager.IsCastleMember(PlayObject) != null))
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckIsGuildMaster(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            return PlayObject.IsGuildMaster();
        }

        private bool ConditionOfCheckIsMaster(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if ((PlayObject.m_sMasterName != "") && PlayObject.m_boMaster)
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckListCount(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            return result;
        }

        private bool ConditionOfCheckItemAddValue(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nWhere = HUtil32.Str_ToInt(QuestConditionInfo.sParam1, -1);
            var cMethod = QuestConditionInfo.sParam2[0];
            var nAddValue = HUtil32.Str_ToInt(QuestConditionInfo.sParam3, -1);
            if (!(nWhere >= PlayObject.m_UseItems.GetLowerBound(0) && nWhere <= PlayObject.m_UseItems.GetUpperBound(0)) || (nAddValue < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKITEMADDVALUE);
                return result;
            }
            var UserItem = PlayObject.m_UseItems[nWhere];
            if (UserItem.wIndex == 0)
            {
                return result;
            }
            var nAddAllValue = 0;
            for (var i = UserItem.btValue.GetLowerBound(0); i <= UserItem.btValue.GetUpperBound(0); i++)
            {
                nAddAllValue += UserItem.btValue[i];
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

        private bool ConditionOfCheckItemType(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nWhere = HUtil32.Str_ToInt(QuestConditionInfo.sParam1, -1);
            var nType = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (!(nWhere >= PlayObject.m_UseItems.GetLowerBound(0) && nWhere <= PlayObject.m_UseItems.GetUpperBound(0)))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKITEMTYPE);
                return result;
            }
            var UserItem = PlayObject.m_UseItems[nWhere];
            if (UserItem == null && UserItem.wIndex == 0)
            {
                return result;
            }
            var Stditem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
            if ((Stditem != null) && (Stditem.StdMode == nType))
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckLevelEx(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nLevel = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKLEVELEX);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_Abil.Level == nLevel)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_Abil.Level > nLevel)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_Abil.Level < nLevel)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_Abil.Level >= nLevel)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckNameListPostion(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            StringList LoadList;
            string sLine;
            var result = false;
            var nNamePostion = -1;
            try
            {
                var sCharName = PlayObject.m_sCharName;
                LoadList = new StringList();
                if (File.Exists(M2Share.g_Config.sEnvirDir + QuestConditionInfo.sParam1))
                {
                    LoadList.LoadFromFile(M2Share.g_Config.sEnvirDir + QuestConditionInfo.sParam1);
                    for (var i = 0; i < LoadList.Count; i++)
                    {
                        sLine = LoadList[i].Trim();
                        if (sLine[1] == ';')
                        {
                            continue;
                        }
                        if (sLine.ToLower().CompareTo(sCharName.ToLower()) == 0)
                        {
                            nNamePostion = i;
                            break;
                        }
                    }
                }
                else
                {
                    ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKNAMELISTPOSITION);
                }
            }
            finally
            {
                //LoadList.Free;
            }
            var nPostion = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nPostion < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKNAMELISTPOSITION);
                return result;
            }
            if (nNamePostion >= nPostion)
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckMarry(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if (PlayObject.m_sDearName != "")
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckMarryCount(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nCount = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKMARRYCOUNT);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_btMarryCount == nCount)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_btMarryCount > nCount)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_btMarryCount < nCount)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_btMarryCount >= nCount)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckMaster(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if ((PlayObject.m_sMasterName != "") && (!PlayObject.m_boMaster))
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckMemBerLevel(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nLevel = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKMEMBERLEVEL);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_nMemberLevel == nLevel)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_nMemberLevel > nLevel)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_nMemberLevel < nLevel)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_nMemberLevel >= nLevel)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckMemberType(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nType = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nType < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKMEMBERTYPE);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_nMemberType == nType)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_nMemberType > nType)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_nMemberType < nType)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_nMemberType >= nType)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckNameIPList(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            StringList LoadList;
            string sLine;
            string sName = string.Empty;
            string sIPaddr;
            var result = false;
            try
            {
                var sCharName = PlayObject.m_sCharName;
                var sCharAccount = PlayObject.m_sUserID;
                var sCharIPaddr = PlayObject.m_sIPaddr;
                LoadList = new StringList();
                if (File.Exists(M2Share.g_Config.sEnvirDir + QuestConditionInfo.sParam1))
                {
                    LoadList.LoadFromFile(M2Share.g_Config.sEnvirDir + QuestConditionInfo.sParam1);
                    for (var i = 0; i < LoadList.Count; i++)
                    {
                        sLine = LoadList[i];
                        if (sLine[1] == ';')
                        {
                            continue;
                        }
                        sIPaddr = HUtil32.GetValidStr3(sLine, ref sName, new string[] { " ", "/", "\t" });
                        sIPaddr = sIPaddr.Trim();
                        if ((sName == sCharName) && (sIPaddr == sCharIPaddr))
                        {
                            result = true;
                            break;
                        }
                    }
                }
                else
                {
                    ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKNAMEIPLIST);
                }
            }
            finally
            {
                //LoadList.Free;
            }
            return result;
        }

        private bool ConditionOfCheckPoseDir(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.GetPoseCreate() == PlayObject) && (PoseHuman.m_btRaceServer == Grobal2.RC_PLAYOBJECT))
            {
                switch (QuestConditionInfo.nParam1)
                {
                    case 1:// 要求相同性别
                        if (PoseHuman.m_btGender == PlayObject.m_btGender)
                        {
                            result = true;
                        }
                        break;
                    case 2:// 要求不同性别
                        if (PoseHuman.m_btGender != PlayObject.m_btGender)
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

        private bool ConditionOfCheckPoseGender(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            byte btSex = 0;
            if (QuestConditionInfo.sParam1.ToLower().CompareTo("MAN".ToLower()) == 0)
            {
                btSex = 0;
            }
            else if (QuestConditionInfo.sParam1.ToLower().CompareTo("男".ToLower()) == 0)
            {
                btSex = 0;
            }
            else if (QuestConditionInfo.sParam1.ToLower().CompareTo("WOMAN".ToLower()) == 0)
            {
                btSex = 1;
            }
            else if (QuestConditionInfo.sParam1.ToLower().CompareTo("女".ToLower()) == 0)
            {
                btSex = 1;
            }
            var PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.m_btRaceServer == Grobal2.RC_PLAYOBJECT))
            {
                if (PoseHuman.m_btGender == btSex)
                {
                    result = true;
                }
            }
            return result;
        }

        private bool ConditionOfCheckPoseIsMaster(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.m_btRaceServer == Grobal2.RC_PLAYOBJECT))
            {
                if ((((TPlayObject)PoseHuman).m_sMasterName != "") && ((TPlayObject)PoseHuman).m_boMaster)
                {
                    result = true;
                }
            }
            return result;
        }

        private bool ConditionOfCheckPoseLevel(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nLevel = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKPOSELEVEL);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            var PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.m_btRaceServer == Grobal2.RC_PLAYOBJECT))
            {
                switch (cMethod)
                {
                    case '=':
                        if (PoseHuman.m_Abil.Level == nLevel)
                        {
                            result = true;
                        }
                        break;
                    case '>':
                        if (PoseHuman.m_Abil.Level > nLevel)
                        {
                            result = true;
                        }
                        break;
                    case '<':
                        if (PoseHuman.m_Abil.Level < nLevel)
                        {
                            result = true;
                        }
                        break;
                    default:
                        if (PoseHuman.m_Abil.Level >= nLevel)
                        {
                            result = true;
                        }
                        break;
                }
            }
            return result;
        }

        private bool ConditionOfCheckPoseMarry(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            TBaseObject PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.m_btRaceServer == Grobal2.RC_PLAYOBJECT))
            {
                if (((TPlayObject)PoseHuman).m_sDearName != "")
                {
                    result = true;
                }
            }
            return result;
        }

        private bool ConditionOfCheckPoseMaster(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            TBaseObject PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.m_btRaceServer == Grobal2.RC_PLAYOBJECT))
            {
                if ((((TPlayObject)PoseHuman).m_sMasterName != "") && !((TPlayObject)PoseHuman).m_boMaster)
                {
                    result = true;
                }
            }
            return result;
        }

        private bool ConditionOfCheckServerName(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            return QuestConditionInfo.sParam1 == M2Share.g_Config.sServerName;
        }

        private bool ConditionOfCheckSlaveCount(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            int nCount = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nCount < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKSLAVECOUNT);
                return result;
            }
            char cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_SlaveList.Count == nCount)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_SlaveList.Count > nCount)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_SlaveList.Count < nCount)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_SlaveList.Count >= nCount)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckMap(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result;
            if (QuestConditionInfo.sParam1 == PlayObject.m_sMapName)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        private bool ConditionOfCheckPos(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result;
            int nX = QuestConditionInfo.nParam2;
            int nY = QuestConditionInfo.nParam3;
            if ((QuestConditionInfo.sParam1 == PlayObject.m_sMapName) && (nX == PlayObject.m_nCurrX) && (nY == PlayObject.m_nCurrY))
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        private bool ConditionOfReviveSlave(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            int resultc = -1;
            string s18;
            FileInfo myFile;
            StringList LoadList;
            string SLineText = string.Empty;
            string Petname = string.Empty;
            string lvl = string.Empty;
            string lvlexp = string.Empty;
            string sFileName = Path.Combine(M2Share.g_Config.sEnvirDir, "PetData", PlayObject.m_sCharName + ".txt");
            if (File.Exists(sFileName))
            {
                LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    s18 = LoadList[i].Trim();
                    if ((s18 != "") && (s18[1] != ';'))
                    {
                        s18 = HUtil32.GetValidStr3(s18, ref Petname, "/");
                        s18 = HUtil32.GetValidStr3(s18, ref lvl, "/");
                        s18 = HUtil32.GetValidStr3(s18, ref lvlexp, "/");
                        // PlayObject.ReviveSlave(PetName,str_ToInt(lvl,0),str_ToInt(lvlexp,0),nslavecount,10 * 24 * 60 * 60);
                        resultc = i;
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

        private bool ConditionOfCheckMagicLvl(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            TUserMagic UserMagic;
            for (var i = 0; i < PlayObject.m_MagicList.Count; i++)
            {
                UserMagic = PlayObject.m_MagicList[i];
                if (string.Compare(UserMagic.MagicInfo.sMagicName, QuestConditionInfo.sParam1, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (UserMagic.btLevel == QuestConditionInfo.nParam2)
                    {
                        result = true;
                    }
                    break;
                }
            }
            return result;
        }

        private bool ConditionOfCheckGroupClass(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            int nCount = 0;
            int nJob = -1;
            TPlayObject PlayObjectEx;
            if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, M2Share.sWarrior, M2Share.sWarrior.Length))
            {
                nJob = M2Share.jWarr;
            }
            if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, M2Share.sWizard, M2Share.sWizard.Length))
            {
                nJob = M2Share.jWizard;
            }
            if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, M2Share.sTaos, M2Share.sTaos.Length))
            {
                nJob = M2Share.jTaos;
            }
            if (nJob < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHANGEJOB);
                return result;
            }
            if (PlayObject.m_GroupOwner != null)
            {
                for (var i = 0; i < PlayObject.m_GroupMembers.Count; i++)
                {
                    PlayObjectEx = PlayObject.m_GroupMembers[i];
                    if (PlayObjectEx.m_btJob == nJob)
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

        public TNormNpc() : base()
        {
            this.m_boSuperMan = true;
            this.m_btRaceServer = Grobal2.RC_NPC;
            this.m_nLight = 2;
            this.m_btAntiPoison = 99;
            this.m_ScriptList = new List<TScript>();
            this.m_boStickMode = true;
            this.m_sFilePath = "";
            this.m_boIsHide = false;
            this.m_boIsQuest = true;
            this.FGotoLable = new int[100];
        }

        ~TNormNpc()
        {
            ClearScript();
        }

        private void ExeAction(TPlayObject PlayObject, string sParam1, string sParam2, string sParam3, int nParam1, int nParam2, int nParam3)
        {
            int nInt1;
            int dwInt;
            // ================================================
            // 更改人物当前经验值
            // EXEACTION CHANGEEXP 0 经验数  设置为指定经验值
            // EXEACTION CHANGEEXP 1 经验数  增加指定经验
            // EXEACTION CHANGEEXP 2 经验数  减少指定经验
            // ================================================
            if (sParam1.ToLower().CompareTo("CHANGEEXP".ToLower()) == 0)
            {
                nInt1 = HUtil32.Str_ToInt(sParam2, -1);
                switch (nInt1)
                {
                    case 0:
                        if (nParam3 >= 0)
                        {
                            PlayObject.m_Abil.Exp = nParam3;
                            PlayObject.HasLevelUp(PlayObject.m_Abil.Level - 1);
                        }
                        break;
                    case 1:
                        if (PlayObject.m_Abil.Exp >= nParam3)
                        {
                            if ((PlayObject.m_Abil.Exp - nParam3) > (int.MaxValue - PlayObject.m_Abil.Exp))
                            {
                                dwInt = int.MaxValue - PlayObject.m_Abil.Exp;
                            }
                            else
                            {
                                dwInt = nParam3;
                            }
                        }
                        else
                        {
                            if ((nParam3 - PlayObject.m_Abil.Exp) > (int.MaxValue - nParam3))
                            {
                                dwInt = int.MaxValue - nParam3;
                            }
                            else
                            {
                                dwInt = nParam3;
                            }
                        }
                        PlayObject.m_Abil.Exp += dwInt;
                        PlayObject.HasLevelUp(PlayObject.m_Abil.Level - 1);
                        break;
                    case 2:
                        if (PlayObject.m_Abil.Exp > nParam3)
                        {
                            PlayObject.m_Abil.Exp -= nParam3;
                        }
                        else
                        {
                            PlayObject.m_Abil.Exp = 0;
                        }
                        PlayObject.HasLevelUp(PlayObject.m_Abil.Level - 1);
                        break;
                }
                PlayObject.SysMsg("您当前经验点数为: " + PlayObject.m_Abil.Exp + '/' + PlayObject.m_Abil.MaxExp, TMsgColor.c_Green, TMsgType.t_Hint);
                return;
            }
            // ================================================
            // 更改人物当前等级
            // EXEACTION CHANGELEVEL 0 等级数  设置为指定等级
            // EXEACTION CHANGELEVEL 1 等级数  增加指定等级
            // EXEACTION CHANGELEVEL 2 等级数  减少指定等级
            // ================================================
            if (sParam1.ToLower().CompareTo("CHANGELEVEL".ToLower()) == 0)
            {
                nInt1 = HUtil32.Str_ToInt(sParam2, -1);
                switch (nInt1)
                {
                    case 0:
                        if (nParam3 >= 0)
                        {
                            PlayObject.m_Abil.Level = (ushort)nParam3;
                            PlayObject.HasLevelUp(PlayObject.m_Abil.Level - 1);
                        }
                        break;
                    case 1:
                        if (PlayObject.m_Abil.Level >= nParam3)
                        {
                            if ((PlayObject.m_Abil.Level - nParam3) > (short.MaxValue - PlayObject.m_Abil.Level))
                            {
                                dwInt = short.MaxValue - PlayObject.m_Abil.Level;
                            }
                            else
                            {
                                dwInt = nParam3;
                            }
                        }
                        else
                        {
                            if ((nParam3 - PlayObject.m_Abil.Level) > (int.MaxValue - nParam3))
                            {
                                dwInt = int.MaxValue - nParam3;
                            }
                            else
                            {
                                dwInt = nParam3;
                            }
                        }
                        PlayObject.m_Abil.Level += (ushort)dwInt;
                        PlayObject.HasLevelUp(PlayObject.m_Abil.Level - 1);
                        break;
                    case 2:
                        if (PlayObject.m_Abil.Level > nParam3)
                        {
                            PlayObject.m_Abil.Level -= (ushort)nParam3;
                        }
                        else
                        {
                            PlayObject.m_Abil.Level = 0;
                        }
                        PlayObject.HasLevelUp(PlayObject.m_Abil.Level - 1);
                        break;
                }
                PlayObject.SysMsg("您当前等级为: " + PlayObject.m_Abil.Level, TMsgColor.c_Green, TMsgType.t_Hint);
                return;
            }
            // ================================================
            // 杀死人物
            // EXEACTION KILL 0 人物死亡,不显示凶手信息
            // EXEACTION KILL 1 人物死亡不掉物品,不显示凶手信息
            // EXEACTION KILL 2 人物死亡,显示凶手信息为NPC
            // EXEACTION KILL 3 人物死亡不掉物品,显示凶手信息为NPC
            // ================================================
            if (sParam1.ToLower().CompareTo("KILL".ToLower()) == 0)
            {
                nInt1 = HUtil32.Str_ToInt(sParam2, -1);
                switch (nInt1)
                {
                    case 1:
                        PlayObject.m_boNoItem = true;
                        PlayObject.Die();
                        break;
                    case 2:
                        PlayObject.SetLastHiter(this);
                        PlayObject.Die();
                        break;
                    case 3:
                        PlayObject.m_boNoItem = true;
                        PlayObject.SetLastHiter(this);
                        PlayObject.Die();
                        break;
                    default:
                        PlayObject.Die();
                        break;
                }
                return;
            }
            // ================================================
            // 踢人物下线
            // EXEACTION KICK
            // ================================================
            if (sParam1.ToLower().CompareTo("KICK".ToLower()) == 0)
            {
                PlayObject.m_boKickFlag = true;
                return;
            }
        }

        public string GetLineVariableText(TPlayObject PlayObject, string sMsg)
        {
            string result;
            int nC = 0;
            string s10 = string.Empty;
            while (true)
            {
                if (HUtil32.TagCount(sMsg, '>') < 1)
                {
                    break;
                }
                HUtil32.ArrestStringEx(sMsg, '<', '>', ref s10);
                GetVariableText(PlayObject, ref sMsg, s10);
                nC++;
                if (nC >= 101)
                {
                    break;
                }
            }
            result = sMsg;
            return result;
        }

        protected virtual void GetVariableText(TPlayObject PlayObject, ref string sMsg, string sVariable)
        {
            string sText = string.Empty;
            string s14 = string.Empty;
            int I;
            int n18;
            TDynamicVar DynamicVar;
            bool boFoundVar;
            // 全局信息
            if (sVariable == "$SERVERNAME")
            {
                sMsg = sub_49ADB8(sMsg, "<$SERVERNAME>", M2Share.g_Config.sServerName);
                return;
            }
            if (sVariable == "$SERVERIP")
            {
                sMsg = sub_49ADB8(sMsg, "<$SERVERIP>", M2Share.g_Config.sServerIPaddr);
                return;
            }
            if (sVariable == "$WEBSITE")
            {
                sMsg = sub_49ADB8(sMsg, "<$WEBSITE>", M2Share.g_Config.sWebSite);
                return;
            }
            if (sVariable == "$BBSSITE")
            {
                sMsg = sub_49ADB8(sMsg, "<$BBSSITE>", M2Share.g_Config.sBbsSite);
                return;
            }
            if (sVariable == "$CLIENTDOWNLOAD")
            {
                sMsg = sub_49ADB8(sMsg, "<$CLIENTDOWNLOAD>", M2Share.g_Config.sClientDownload);
                return;
            }
            if (sVariable == "$QQ")
            {
                sMsg = sub_49ADB8(sMsg, "<$QQ>", M2Share.g_Config.sQQ);
                return;
            }
            if (sVariable == "$PHONE")
            {
                sMsg = sub_49ADB8(sMsg, "<$PHONE>", M2Share.g_Config.sPhone);
                return;
            }
            if (sVariable == "$BANKACCOUNT0")
            {
                sMsg = sub_49ADB8(sMsg, "<$BANKACCOUNT0>", M2Share.g_Config.sBankAccount0);
                return;
            }
            if (sVariable == "$BANKACCOUNT1")
            {
                sMsg = sub_49ADB8(sMsg, "<$BANKACCOUNT1>", M2Share.g_Config.sBankAccount1);
                return;
            }
            if (sVariable == "$BANKACCOUNT2")
            {
                sMsg = sub_49ADB8(sMsg, "<$BANKACCOUNT2>", M2Share.g_Config.sBankAccount2);
                return;
            }
            if (sVariable == "$BANKACCOUNT3")
            {
                sMsg = sub_49ADB8(sMsg, "<$BANKACCOUNT3>", M2Share.g_Config.sBankAccount3);
                return;
            }
            if (sVariable == "$BANKACCOUNT4")
            {
                sMsg = sub_49ADB8(sMsg, "<$BANKACCOUNT4>", M2Share.g_Config.sBankAccount4);
                return;
            }
            if (sVariable == "$BANKACCOUNT5")
            {
                sMsg = sub_49ADB8(sMsg, "<$BANKACCOUNT5>", M2Share.g_Config.sBankAccount5);
                return;
            }
            if (sVariable == "$BANKACCOUNT6")
            {
                sMsg = sub_49ADB8(sMsg, "<$BANKACCOUNT6>", M2Share.g_Config.sBankAccount6);
                return;
            }
            if (sVariable == "$BANKACCOUNT7")
            {
                sMsg = sub_49ADB8(sMsg, "<$BANKACCOUNT7>", M2Share.g_Config.sBankAccount7);
                return;
            }
            if (sVariable == "$BANKACCOUNT8")
            {
                sMsg = sub_49ADB8(sMsg, "<$BANKACCOUNT8>", M2Share.g_Config.sBankAccount8);
                return;
            }
            if (sVariable == "$BANKACCOUNT9")
            {
                sMsg = sub_49ADB8(sMsg, "<$BANKACCOUNT9>", M2Share.g_Config.sBankAccount9);
                return;
            }
            if (sVariable == "$GAMEGOLDNAME")
            {
                sMsg = sub_49ADB8(sMsg, "<$GAMEGOLDNAME>", M2Share.g_Config.sGameGoldName);
                return;
            }
            if (sVariable == "$GAMEPOINTNAME")
            {
                sMsg = sub_49ADB8(sMsg, "<$GAMEPOINTNAME>", M2Share.g_Config.sGamePointName);
                return;
            }
            if (sVariable == "$USERCOUNT")
            {
                sText = M2Share.UserEngine.PlayObjectCount.ToString();
                sMsg = sub_49ADB8(sMsg, "<$USERCOUNT>", sText);
                return;
            }
            if (sVariable == "$MACRUNTIME")
            {

                sText = (HUtil32.GetTickCount() / (24 * 60 * 60 * 1000)).ToString();
                sMsg = sub_49ADB8(sMsg, "<$MACRUNTIME>", sText);
                return;
            }
            if (sVariable == "$SERVERRUNTIME")
            {
                //nSecond = (HUtil32.GetTickCount() - M2Share.g_dwStartTick) / 1000;
                //wHour = nSecond / 3600;
                //wMinute = (nSecond / 60) % 60;
                //wSecond = nSecond % 60;
                //sText = format("%d:%d:%d", new short[] {wHour, wMinute, wSecond});
                sMsg = sub_49ADB8(sMsg, "<$SERVERRUNTIME>", sText);
                return;
            }
            if (sVariable == "$DATETIME")
            {
                // sText:=DateTimeToStr(Now);
                sText = DateTime.Now.ToString("dddddd,dddd,hh:mm:nn");
                sMsg = sub_49ADB8(sMsg, "<$DATETIME>", sText);
                return;
            }
            if (sVariable == "$HIGHLEVELINFO")
            {
                if (M2Share.g_HighLevelHuman != null)
                {
                    sText = ((TPlayObject)M2Share.g_HighLevelHuman).GetMyInfo();
                }
                else
                {
                    sText = "????";
                }
                sMsg = sub_49ADB8(sMsg, "<$HIGHLEVELINFO>", sText);
                return;
            }
            if (sVariable == "$HIGHPKINFO")
            {
                if (M2Share.g_HighPKPointHuman != null)
                {
                    sText = ((TPlayObject)M2Share.g_HighPKPointHuman).GetMyInfo();
                }
                else
                {
                    sText = "????";
                }
                sMsg = sub_49ADB8(sMsg, "<$HIGHPKINFO>", sText);
                return;
            }
            if (sVariable == "$HIGHDCINFO")
            {
                if (M2Share.g_HighDCHuman != null)
                {
                    sText = ((TPlayObject)M2Share.g_HighDCHuman).GetMyInfo();
                }
                else
                {
                    sText = "????";
                }
                sMsg = sub_49ADB8(sMsg, "<$HIGHDCINFO>", sText);
                return;
            }
            if (sVariable == "$HIGHMCINFO")
            {
                if (M2Share.g_HighMCHuman != null)
                {
                    sText = ((TPlayObject)M2Share.g_HighMCHuman).GetMyInfo();
                }
                else
                {
                    sText = "????";
                }
                sMsg = sub_49ADB8(sMsg, "<$HIGHMCINFO>", sText);
                return;
            }
            if (sVariable == "$HIGHSCINFO")
            {
                if (M2Share.g_HighSCHuman != null)
                {
                    sText = ((TPlayObject)M2Share.g_HighSCHuman).GetMyInfo();
                }
                else
                {
                    sText = "????";
                }
                sMsg = sub_49ADB8(sMsg, "<$HIGHSCINFO>", sText);
                return;
            }
            if (sVariable == "$HIGHONLINEINFO")
            {
                if (M2Share.g_HighOnlineHuman != null)
                {
                    sText = ((TPlayObject)M2Share.g_HighOnlineHuman).GetMyInfo();
                }
                else
                {
                    sText = "????";
                }
                sMsg = sub_49ADB8(sMsg, "<$HIGHONLINEINFO>", sText);
                return;
            }
            // 个人信息
            if (sVariable == "$RANDOMNO")
            {
                sMsg = sub_49ADB8(sMsg, "<$RANDOMNO>", PlayObject.m_sRandomNo);
                return;
            }
            if (sVariable == "$RELEVEL")
            {
                sText = PlayObject.m_btReLevel.ToString();
                sMsg = sub_49ADB8(sMsg, "<$RELEVEL>", sText);
                return;
            }
            if (sVariable == "$HUMANSHOWNAME")
            {
                sMsg = sub_49ADB8(sMsg, "<$HUMANSHOWNAME>", PlayObject.GetShowName());
                return;
            }
            if (sVariable == "$MONKILLER")
            {
                if (PlayObject.m_LastHiter != null)
                {
                    if (PlayObject.m_LastHiter.m_btRaceServer != Grobal2.RC_PLAYOBJECT)
                    {
                        sMsg = sub_49ADB8(sMsg, "<$MONKILLER>", PlayObject.m_LastHiter.m_sCharName);
                    }
                }
                else
                {
                    sMsg = sub_49ADB8(sMsg, "<$MONKILLER>", "未知");
                }
                return;
            }
            if (sVariable == "$KILLER")
            {
                if (PlayObject.m_LastHiter != null)
                {
                    if (PlayObject.m_LastHiter.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                    {
                        sMsg = sub_49ADB8(sMsg, "<$KILLER>", PlayObject.m_LastHiter.m_sCharName);
                    }
                }
                else
                {
                    sMsg = sub_49ADB8(sMsg, "<$KILLER>", "未知");
                }
                return;
            }
            if (sVariable == "$USERNAME")
            {
                sMsg = sub_49ADB8(sMsg, "<$USERNAME>", PlayObject.m_sCharName);
                return;
            }
            if (sVariable == "$GUILDNAME")
            {
                if (PlayObject.m_MyGuild != null)
                {
                    sMsg = sub_49ADB8(sMsg, "<$GUILDNAME>", PlayObject.m_MyGuild.sGuildName);
                }
                else
                {
                    sMsg = "无";
                }
                return;
            }
            if (sVariable == "$RANKNAME")
            {
                sMsg = sub_49ADB8(sMsg, "<$RANKNAME>", PlayObject.m_sGuildRankName);
                return;
            }
            if (sVariable == "$LEVEL")
            {
                sText = PlayObject.m_Abil.Level.ToString();
                sMsg = sub_49ADB8(sMsg, "<$LEVEL>", sText);
                return;
            }
            if (sVariable == "$HP")
            {
                sText = PlayObject.m_WAbil.HP.ToString();
                sMsg = sub_49ADB8(sMsg, "<$HP>", sText);
                return;
            }
            if (sVariable == "$MAXHP")
            {
                sText = PlayObject.m_WAbil.MaxHP.ToString();
                sMsg = sub_49ADB8(sMsg, "<$MAXHP>", sText);
                return;
            }
            if (sVariable == "$MP")
            {
                sText = PlayObject.m_WAbil.MP.ToString();
                sMsg = sub_49ADB8(sMsg, "<$MP>", sText);
                return;
            }
            if (sVariable == "$MAXMP")
            {
                sText = PlayObject.m_WAbil.MaxMP.ToString();
                sMsg = sub_49ADB8(sMsg, "<$MAXMP>", sText);
                return;
            }
            if (sVariable == "$AC")
            {

                sText = HUtil32.LoWord(PlayObject.m_WAbil.AC).ToString();
                sMsg = sub_49ADB8(sMsg, "<$AC>", sText);
                return;
            }
            if (sVariable == "$MAXAC")
            {

                sText = HUtil32.HiWord(PlayObject.m_WAbil.AC).ToString();
                sMsg = sub_49ADB8(sMsg, "<$MAXAC>", sText);
                return;
            }
            if (sVariable == "$MAC")
            {

                sText = HUtil32.LoWord(PlayObject.m_WAbil.MAC).ToString();
                sMsg = sub_49ADB8(sMsg, "<$MAC>", sText);
                return;
            }
            if (sVariable == "$MAXMAC")
            {

                sText = HUtil32.HiWord(PlayObject.m_WAbil.MAC).ToString();
                sMsg = sub_49ADB8(sMsg, "<$MAXMAC>", sText);
                return;
            }
            if (sVariable == "$DC")
            {

                sText = HUtil32.LoWord(PlayObject.m_WAbil.DC).ToString();
                sMsg = sub_49ADB8(sMsg, "<$DC>", sText);
                return;
            }
            if (sVariable == "$MAXDC")
            {

                sText = HUtil32.HiWord(PlayObject.m_WAbil.DC).ToString();
                sMsg = sub_49ADB8(sMsg, "<$MAXDC>", sText);
                return;
            }
            if (sVariable == "$MC")
            {
                sText = HUtil32.LoWord(PlayObject.m_WAbil.MC).ToString();
                sMsg = sub_49ADB8(sMsg, "<$MC>", sText);
                return;
            }
            if (sVariable == "$MAXMC")
            {
                sText = HUtil32.HiWord(PlayObject.m_WAbil.MC).ToString();
                sMsg = sub_49ADB8(sMsg, "<$MAXMC>", sText);
                return;
            }
            if (sVariable == "$SC")
            {
                sText = HUtil32.LoWord(PlayObject.m_WAbil.SC).ToString();
                sMsg = sub_49ADB8(sMsg, "<$SC>", sText);
                return;
            }
            if (sVariable == "$MAXSC")
            {

                sText = HUtil32.HiWord(PlayObject.m_WAbil.SC).ToString();
                sMsg = sub_49ADB8(sMsg, "<$MAXSC>", sText);
                return;
            }
            if (sVariable == "$EXP")
            {
                sText = PlayObject.m_Abil.Exp.ToString();
                sMsg = sub_49ADB8(sMsg, "<$EXP>", sText);
                return;
            }
            if (sVariable == "$MAXEXP")
            {
                sText = PlayObject.m_Abil.MaxExp.ToString();
                sMsg = sub_49ADB8(sMsg, "<$MAXEXP>", sText);
                return;
            }
            if (sVariable == "$PKPOINT")
            {
                sText = PlayObject.m_nPkPoint.ToString();
                sMsg = sub_49ADB8(sMsg, "<$PKPOINT>", sText);
                return;
            }
            if (sVariable == "$CREDITPOINT")
            {
                sText = PlayObject.m_btCreditPoint.ToString();
                sMsg = sub_49ADB8(sMsg, "<$CREDITPOINT>", sText);
                return;
            }
            if (sVariable == "$HW")
            {
                sText = PlayObject.m_WAbil.HandWeight.ToString();
                sMsg = sub_49ADB8(sMsg, "<$HW>", sText);
                return;
            }
            if (sVariable == "$MAXHW")
            {
                sText = PlayObject.m_WAbil.MaxHandWeight.ToString();
                sMsg = sub_49ADB8(sMsg, "<$MAXHW>", sText);
                return;
            }
            if (sVariable == "$BW")
            {
                sText = PlayObject.m_WAbil.Weight.ToString();
                sMsg = sub_49ADB8(sMsg, "<$BW>", sText);
                return;
            }
            if (sVariable == "$MAXBW")
            {
                sText = PlayObject.m_WAbil.MaxWeight.ToString();
                sMsg = sub_49ADB8(sMsg, "<$MAXBW>", sText);
                return;
            }
            if (sVariable == "$WW")
            {
                sText = PlayObject.m_WAbil.WearWeight.ToString();
                sMsg = sub_49ADB8(sMsg, "<$WW>", sText);
                return;
            }
            if (sVariable == "$MAXWW")
            {
                sText = PlayObject.m_WAbil.MaxWearWeight.ToString();
                sMsg = sub_49ADB8(sMsg, "<$MAXWW>", sText);
                return;
            }
            if (sVariable == "$GOLDCOUNT")
            {
                sText = PlayObject.m_nGold.ToString() + '/' + PlayObject.m_nGoldMax;
                sMsg = sub_49ADB8(sMsg, "<$GOLDCOUNT>", sText);
                return;
            }
            if (sVariable == "$GAMEGOLD")
            {
                sText = PlayObject.m_nGameGold.ToString();
                sMsg = sub_49ADB8(sMsg, "<$GAMEGOLD>", sText);
                return;
            }
            if (sVariable == "$GAMEPOINT")
            {
                sText = PlayObject.m_nGamePoint.ToString();
                sMsg = sub_49ADB8(sMsg, "<$GAMEPOINT>", sText);
                return;
            }
            if (sVariable == "$HUNGER")
            {
                sText = PlayObject.GetMyStatus().ToString();
                sMsg = sub_49ADB8(sMsg, "<$HUNGER>", sText);
                return;
            }
            if (sVariable == "$LOGINTIME")
            {
                sText = PlayObject.m_dLogonTime.ToString();
                sMsg = sub_49ADB8(sMsg, "<$LOGINTIME>", sText);
                return;
            }
            if (sVariable == "$LOGINLONG")
            {
                sText = ((HUtil32.GetTickCount() - PlayObject.m_dwLogonTick) / 60000) + "分钟";
                sMsg = sub_49ADB8(sMsg, "<$LOGINLONG>", sText);
                return;
            }
            if (sVariable == "$DRESS")
            {
                sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_DRESS].wIndex);
                sMsg = sub_49ADB8(sMsg, "<$DRESS>", sText);
                return;
            }
            else if (sVariable == "$WEAPON")
            {
                sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_WEAPON].wIndex);
                sMsg = sub_49ADB8(sMsg, "<$WEAPON>", sText);
                return;
            }
            else if (sVariable == "$RIGHTHAND")
            {
                sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_RIGHTHAND].wIndex);
                sMsg = sub_49ADB8(sMsg, "<$RIGHTHAND>", sText);
                return;
            }
            else if (sVariable == "$HELMET")
            {
                sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_HELMET].wIndex);
                sMsg = sub_49ADB8(sMsg, "<$HELMET>", sText);
                return;
            }
            else if (sVariable == "$NECKLACE")
            {
                sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_NECKLACE].wIndex);
                sMsg = sub_49ADB8(sMsg, "<$NECKLACE>", sText);
                return;
            }
            else if (sVariable == "$RING_R")
            {
                sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_RINGR].wIndex);
                sMsg = sub_49ADB8(sMsg, "<$RING_R>", sText);
                return;
            }
            else if (sVariable == "$RING_L")
            {
                sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_RINGL].wIndex);
                sMsg = sub_49ADB8(sMsg, "<$RING_L>", sText);
                return;
            }
            else if (sVariable == "$ARMRING_R")
            {
                sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_ARMRINGR].wIndex);
                sMsg = sub_49ADB8(sMsg, "<$ARMRING_R>", sText);
                return;
            }
            else if (sVariable == "$ARMRING_L")
            {
                sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_ARMRINGL].wIndex);
                sMsg = sub_49ADB8(sMsg, "<$ARMRING_L>", sText);
                return;
            }
            else if (sVariable == "$BUJUK")
            {
                sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_BUJUK].wIndex);
                sMsg = sub_49ADB8(sMsg, "<$BUJUK>", sText);
                return;
            }
            else if (sVariable == "$BELT")
            {
                sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_BELT].wIndex);
                sMsg = sub_49ADB8(sMsg, "<$BELT>", sText);
                return;
            }
            else if (sVariable == "$BOOTS")
            {
                sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_BOOTS].wIndex);
                sMsg = sub_49ADB8(sMsg, "<$BOOTS>", sText);
                return;
            }
            else if (sVariable == "$CHARM")
            {
                sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_CHARM].wIndex);
                sMsg = sub_49ADB8(sMsg, "<$CHARM>", sText);
                return;
            }
            else if (sVariable == "$IPADDR")
            {
                sText = PlayObject.m_sIPaddr;
                sMsg = sub_49ADB8(sMsg, "<$IPADDR>", sText);
                return;
            }
            else if (sVariable == "$IPLOCAL")
            {
                sText = PlayObject.m_sIPLocal;
                // GetIPLocal(PlayObject.m_sIPaddr);
                sMsg = sub_49ADB8(sMsg, "<$IPLOCAL>", sText);
                return;
            }
            else if (sVariable == "$GUILDBUILDPOINT")
            {
                if (PlayObject.m_MyGuild == null)
                {
                    sText = "无";
                }
                else
                {
                    sText = PlayObject.m_MyGuild.nBuildPoint.ToString();
                }
                sMsg = sub_49ADB8(sMsg, "<$GUILDBUILDPOINT>", sText);
                return;
            }
            else if (sVariable == "$GUILDAURAEPOINT")
            {
                if (PlayObject.m_MyGuild == null)
                {
                    sText = "无";
                }
                else
                {
                    sText = PlayObject.m_MyGuild.nAurae.ToString();
                }
                sMsg = sub_49ADB8(sMsg, "<$GUILDAURAEPOINT>", sText);
                return;
            }
            else if (sVariable == "$GUILDSTABILITYPOINT")
            {
                if (PlayObject.m_MyGuild == null)
                {
                    sText = "无";
                }
                else
                {
                    sText = PlayObject.m_MyGuild.nStability.ToString();
                }
                sMsg = sub_49ADB8(sMsg, "<$GUILDSTABILITYPOINT>", sText);
                return;
            }
            if (sVariable == "$GUILDFLOURISHPOINT")
            {
                if (PlayObject.m_MyGuild == null)
                {
                    sText = "无";
                }
                else
                {
                    sText = PlayObject.m_MyGuild.nFlourishing.ToString();
                }
                sMsg = sub_49ADB8(sMsg, "<$GUILDFLOURISHPOINT>", sText);
                return;
            }
            // 其它信息
            if (sVariable == "$REQUESTCASTLEWARITEM")
            {
                sText = M2Share.g_Config.sZumaPiece;
                sMsg = sub_49ADB8(sMsg, "<$REQUESTCASTLEWARITEM>", sText);
                return;
            }
            if (sVariable == "$REQUESTCASTLEWARDAY")
            {
                sText = M2Share.g_Config.sZumaPiece;
                sMsg = sub_49ADB8(sMsg, "<$REQUESTCASTLEWARDAY>", sText);
                return;
            }
            if (sVariable == "$REQUESTBUILDGUILDITEM")
            {
                sText = M2Share.g_Config.sWomaHorn;
                sMsg = sub_49ADB8(sMsg, "<$REQUESTBUILDGUILDITEM>", sText);
                return;
            }
            if (sVariable == "$OWNERGUILD")
            {
                if (this.m_Castle != null)
                {
                    sText = this.m_Castle.m_sOwnGuild;
                    if (sText == "")
                    {
                        sText = "游戏管理";
                    }
                }
                else
                {
                    sText = "????";
                }
                sMsg = sub_49ADB8(sMsg, "<$OWNERGUILD>", sText);
                return;
            }
            // 0049AF32
            if (sVariable == "$CASTLENAME")
            {
                if (this.m_Castle != null)
                {
                    sText = this.m_Castle.m_sName;
                }
                else
                {
                    sText = "????";
                }
                sMsg = sub_49ADB8(sMsg, "<$CASTLENAME>", sText);
                return;
            }
            if (sVariable == "$LORD")
            {
                if (this.m_Castle != null)
                {
                    if (this.m_Castle.m_MasterGuild != null)
                    {
                        sText = this.m_Castle.m_MasterGuild.GetChiefName();
                    }
                    else
                    {
                        sText = "管理员";
                    }
                }
                else
                {
                    sText = "????";
                }
                sMsg = sub_49ADB8(sMsg, "<$LORD>", sText);
                return;
            }
            // 0049AF32
            if (sVariable == "$GUILDWARFEE")
            {
                sMsg = sub_49ADB8(sMsg, "<$GUILDWARFEE>", M2Share.g_Config.nGuildWarPrice.ToString());
                return;
            }
            if (sVariable == "$BUILDGUILDFEE")
            {
                sMsg = sub_49ADB8(sMsg, "<$BUILDGUILDFEE>", M2Share.g_Config.nBuildGuildPrice.ToString());
                return;
            }
            if (sVariable == "$CASTLEWARDATE")
            {
                if (this.m_Castle == null)
                {
                    this.m_Castle = M2Share.CastleManager.GetCastle(0);
                }
                if (this.m_Castle != null)
                {
                    if (!this.m_Castle.m_boUnderWar)
                    {
                        sText = this.m_Castle.GetWarDate();
                        if (sText != "")
                        {
                            sMsg = sub_49ADB8(sMsg, "<$CASTLEWARDATE>", sText);
                        }
                        else
                        {
                            sMsg = "Well I guess there may be no wall conquest war in the mean time .\\ \\<back/@main>";
                        }
                    }
                    else
                    {
                        sMsg = "Now is on wall conquest war.\\ \\<back/@main>";
                    }
                }
                else
                {
                    sText = "????";
                }
                return;
            }
            if (sVariable == "$LISTOFWAR")
            {
                if (this.m_Castle != null)
                {
                    sText = this.m_Castle.GetAttackWarList();
                }
                else
                {
                    sText = "????";
                }
                if (sText != "")
                {
                    sMsg = sub_49ADB8(sMsg, "<$LISTOFWAR>", sText);
                }
                else
                {
                    sMsg = "We have no schedule...\\ \\<back/@main>";
                }
                return;
            }
            if (sVariable == "$CASTLECHANGEDATE")
            {
                if (this.m_Castle != null)
                {
                    sText = this.m_Castle.m_ChangeDate.ToString();
                }
                else
                {
                    sText = "????";
                }
                sMsg = sub_49ADB8(sMsg, "<$CASTLECHANGEDATE>", sText);
                return;
            }
            if (sVariable == "$CASTLEWARLASTDATE")
            {
                if (this.m_Castle != null)
                {
                    sText = this.m_Castle.m_WarDate.ToString();
                }
                else
                {
                    sText = "????";
                }
                sMsg = sub_49ADB8(sMsg, "<$CASTLEWARLASTDATE>", sText);
                return;
            }
            if (sVariable == "$CASTLEGETDAYS")
            {
                if (this.m_Castle != null)
                {
                    sText = HUtil32.GetDayCount(DateTime.Now, this.m_Castle.m_ChangeDate).ToString();
                }
                else
                {
                    sText = "????";
                }
                sMsg = sub_49ADB8(sMsg, "<$CASTLEGETDAYS>", sText);
                return;
            }
            if (sVariable == "$CMD_DATE")
            {
                sMsg = sub_49ADB8(sMsg, "<$CMD_DATE>", M2Share.g_GameCommand.DATA.sCmd);
                return;
            }
            if (sVariable == "$CMD_ALLOWMSG")
            {
                sMsg = sub_49ADB8(sMsg, "<$CMD_ALLOWMSG>", M2Share.g_GameCommand.ALLOWMSG.sCmd);
                return;
            }
            if (sVariable == "$CMD_LETSHOUT")
            {
                sMsg = sub_49ADB8(sMsg, "<$CMD_LETSHOUT>", M2Share.g_GameCommand.LETSHOUT.sCmd);
                return;
            }
            if (sVariable == "$CMD_LETTRADE")
            {
                sMsg = sub_49ADB8(sMsg, "<$CMD_LETTRADE>", M2Share.g_GameCommand.LETTRADE.sCmd);
                return;
            }
            if (sVariable == "$CMD_LETGUILD")
            {
                sMsg = sub_49ADB8(sMsg, "<$CMD_LETGUILD>", M2Share.g_GameCommand.LETGUILD.sCmd);
                return;
            }
            if (sVariable == "$CMD_ENDGUILD")
            {
                sMsg = sub_49ADB8(sMsg, "<$CMD_ENDGUILD>", M2Share.g_GameCommand.ENDGUILD.sCmd);
                return;
            }
            if (sVariable == "$CMD_BANGUILDCHAT")
            {
                sMsg = sub_49ADB8(sMsg, "<$CMD_BANGUILDCHAT>", M2Share.g_GameCommand.BANGUILDCHAT.sCmd);
                return;
            }
            if (sVariable == "$CMD_AUTHALLY")
            {
                sMsg = sub_49ADB8(sMsg, "<$CMD_AUTHALLY>", M2Share.g_GameCommand.AUTHALLY.sCmd);
                return;
            }
            if (sVariable == "$CMD_AUTH")
            {
                sMsg = sub_49ADB8(sMsg, "<$CMD_AUTH>", M2Share.g_GameCommand.AUTH.sCmd);
                return;
            }
            if (sVariable == "$CMD_AUTHCANCEL")
            {
                sMsg = sub_49ADB8(sMsg, "<$CMD_AUTHCANCEL>", M2Share.g_GameCommand.AUTHCANCEL.sCmd);
                return;
            }
            if (sVariable == "$CMD_USERMOVE")
            {
                sMsg = sub_49ADB8(sMsg, "<$CMD_USERMOVE>", M2Share.g_GameCommand.USERMOVE.sCmd);
                return;
            }
            if (sVariable == "$CMD_SEARCHING")
            {
                sMsg = sub_49ADB8(sMsg, "<$CMD_SEARCHING>", M2Share.g_GameCommand.SEARCHING.sCmd);
                return;
            }
            if (sVariable == "$CMD_ALLOWGROUPCALL")
            {
                sMsg = sub_49ADB8(sMsg, "<$CMD_ALLOWGROUPCALL>", M2Share.g_GameCommand.ALLOWGROUPCALL.sCmd);
                return;
            }
            if (sVariable == "$CMD_GROUPRECALLL")
            {
                sMsg = sub_49ADB8(sMsg, "<$CMD_GROUPRECALLL>", M2Share.g_GameCommand.GROUPRECALLL.sCmd);
                return;
            }
            if (sVariable == "$CMD_ATTACKMODE")
            {
                sMsg = sub_49ADB8(sMsg, "<$CMD_ATTACKMODE>", M2Share.g_GameCommand.ATTACKMODE.sCmd);
                return;
            }
            if (sVariable == "$CMD_REST")
            {
                sMsg = sub_49ADB8(sMsg, "<$CMD_REST>", M2Share.g_GameCommand.REST.sCmd);
                return;
            }
            if (sVariable == "$CMD_STORAGESETPASSWORD")
            {
                sMsg = sub_49ADB8(sMsg, "<$CMD_STORAGESETPASSWORD>", M2Share.g_GameCommand.SETPASSWORD.sCmd);
                return;
            }
            if (sVariable == "$CMD_STORAGECHGPASSWORD")
            {
                sMsg = sub_49ADB8(sMsg, "<$CMD_STORAGECHGPASSWORD>", M2Share.g_GameCommand.CHGPASSWORD.sCmd);
                return;
            }
            if (sVariable == "$CMD_STORAGELOCK")
            {
                sMsg = sub_49ADB8(sMsg, "<$CMD_STORAGELOCK>", M2Share.g_GameCommand.__LOCK.sCmd);
                return;
            }
            if (sVariable == "$CMD_STORAGEUNLOCK")
            {
                sMsg = sub_49ADB8(sMsg, "<$CMD_STORAGEUNLOCK>", M2Share.g_GameCommand.UNLOCKSTORAGE.sCmd);
                return;
            }
            if (sVariable == "$CMD_UNLOCK")
            {
                sMsg = sub_49ADB8(sMsg, "<$CMD_UNLOCK>", M2Share.g_GameCommand.UNLOCK.sCmd);
                return;
            }
            if (HUtil32.CompareLStr(sVariable, "$HUMAN(", "$HUMAN(".Length))
            {
                HUtil32.ArrestStringEx(sVariable, '(', ')', ref s14);
                boFoundVar = false;
                for (I = 0; I < PlayObject.m_DynamicVarList.Count; I++)
                {
                    DynamicVar = PlayObject.m_DynamicVarList[I];
                    if (DynamicVar.sName.ToLower().CompareTo(s14.ToLower()) == 0)
                    {
                        switch (DynamicVar.VarType)
                        {
                            case TVarType.VInteger:
                                sMsg = sub_49ADB8(sMsg, '<' + sVariable + '>', DynamicVar.nInternet.ToString());
                                boFoundVar = true;
                                break;
                            case TVarType.VString:
                                sMsg = sub_49ADB8(sMsg, '<' + sVariable + '>', DynamicVar.sString);
                                boFoundVar = true;
                                break;
                        }
                        break;
                    }
                }
                if (!boFoundVar)
                {
                    sMsg = "??";
                }
                return;
            }
            if (HUtil32.CompareLStr(sVariable, "$GUILD(", "$GUILD(".Length))
            {
                if (PlayObject.m_MyGuild == null)
                {
                    return;
                }
                HUtil32.ArrestStringEx(sVariable, '(', ')', ref s14);
                boFoundVar = false;
                for (I = 0; I < PlayObject.m_MyGuild.m_DynamicVarList.Count; I++)
                {
                    DynamicVar = PlayObject.m_MyGuild.m_DynamicVarList[I];
                    if (DynamicVar.sName.ToLower().CompareTo(s14.ToLower()) == 0)
                    {
                        switch (DynamicVar.VarType)
                        {
                            case TVarType.VInteger:
                                sMsg = sub_49ADB8(sMsg, '<' + sVariable + '>', DynamicVar.nInternet.ToString());
                                boFoundVar = true;
                                break;
                            case TVarType.VString:
                                sMsg = sub_49ADB8(sMsg, '<' + sVariable + '>', DynamicVar.sString);
                                boFoundVar = true;
                                break;
                        }
                        break;
                    }
                }
                if (!boFoundVar)
                {
                    sMsg = "??";
                }
                return;
            }
            if (HUtil32.CompareLStr(sVariable, "$GLOBAL(", "$GLOBAL(".Length))
            {
                HUtil32.ArrestStringEx(sVariable, '(', ')', ref s14);
                boFoundVar = false;
                for (I = 0; I < M2Share.g_DynamicVarList.Count; I++)
                {
                    DynamicVar = M2Share.g_DynamicVarList[I];
                    if (DynamicVar.sName.ToLower().CompareTo(s14.ToLower()) == 0)
                    {
                        switch (DynamicVar.VarType)
                        {
                            case TVarType.VInteger:
                                sMsg = sub_49ADB8(sMsg, '<' + sVariable + '>', DynamicVar.nInternet.ToString());
                                boFoundVar = true;
                                break;
                            case TVarType.VString:
                                sMsg = sub_49ADB8(sMsg, '<' + sVariable + '>', DynamicVar.sString);
                                boFoundVar = true;
                                break;
                        }
                        break;
                    }
                }
                if (!boFoundVar)
                {
                    sMsg = "??";
                }
                return;
            }
            if (HUtil32.CompareLStr(sVariable, "$STR(", "$STR(".Length))
            {
                HUtil32.ArrestStringEx(sVariable, '(', ')', ref s14);
                n18 = M2Share.GetValNameNo(s14);
                if (n18 >= 0)
                {
                    switch (n18)
                    {
                        // Modify the A .. B: 0 .. 9
                        case 0:
                            sMsg = sub_49ADB8(sMsg, '<' + sVariable + '>', PlayObject.m_nVal[n18].ToString());
                            break;
                        // Modify the A .. B: 100 .. 119
                        case 100:
                            sMsg = sub_49ADB8(sMsg, '<' + sVariable + '>', M2Share.g_Config.GlobalVal[n18 - 100].ToString());
                            break;
                        // Modify the A .. B: 200 .. 209
                        case 200:
                            sMsg = sub_49ADB8(sMsg, '<' + sVariable + '>', PlayObject.m_DyVal[n18 - 200].ToString());
                            break;
                        // Modify the A .. B: 300 .. 399
                        case 300:
                            sMsg = sub_49ADB8(sMsg, '<' + sVariable + '>', PlayObject.m_nMval[n18 - 300].ToString());
                            break;
                        // Modify the A .. B: 400 .. 499
                        case 400:
                            sMsg = sub_49ADB8(sMsg, '<' + sVariable + '>', M2Share.g_Config.GlobaDyMval[n18 - 400].ToString());
                            break;
                    }
                }
            }
        }

        public void LoadNPCScript()
        {
            string s08;
            if (m_boIsQuest)
            {
                m_sPath = M2Share.sNpc_def;
                s08 = this.m_sCharName + '-' + this.m_sMapName;
                M2Share.ScriptSystem.LoadNpcScript(this, m_sFilePath, s08);
            }
            else
            {
                m_sPath = m_sFilePath;
                M2Share.ScriptSystem.LoadNpcScript(this, m_sFilePath, this.m_sCharName);
            }
        }

        public override bool Operate(TProcessMessage ProcessMsg)
        {
            return base.Operate(ProcessMsg);
        }

        public override void Run()
        {
            if (this.m_Master != null)// 不允许召唤为宝宝
            {
                this.m_Master = null;
            }
            base.Run();
        }

        private void ScriptActionError(TPlayObject PlayObject, string sErrMsg, TQuestActionInfo QuestActionInfo, string sCmd)
        {
            string sMsg;
            const string sOutMessage = "[脚本错误] {0} 脚本命令:{1} NPC名称:{2} 地图:{3}({4}:{5}) 参数1:{6} 参数2:{7} 参数3:{8} 参数4:{9} 参数5:{10} 参数6:{11}";
            sMsg = format(sOutMessage, sErrMsg, sCmd, this.m_sCharName, this.m_sMapName, this.m_nCurrX, this.m_nCurrY, QuestActionInfo.sParam1, QuestActionInfo.sParam2, QuestActionInfo.sParam3, QuestActionInfo.sParam4, QuestActionInfo.sParam5, QuestActionInfo.sParam6);
            M2Share.MainOutMessage(sMsg);
        }

        private void ScriptConditionError(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo, string sCmd)
        {
            string sMsg;
            sMsg = "Cmd:" + sCmd + " NPC名称:" + this.m_sCharName + " 地图:" + this.m_sMapName + " 座标:" + this.m_nCurrX + ':' + this.m_nCurrY + " 参数1:" + QuestConditionInfo.sParam1 + " 参数2:" + QuestConditionInfo.sParam2 + " 参数3:" + QuestConditionInfo.sParam3 + " 参数4:" + QuestConditionInfo.sParam4 + " 参数5:" + QuestConditionInfo.sParam5;
            M2Share.MainOutMessage("[脚本参数不正确] " + sMsg);
        }

        protected void SendMsgToUser(TPlayObject PlayObject, string sMsg)
        {
            PlayObject.SendMsg(this, Grobal2.RM_MERCHANTSAY, 0, 0, 0, 0, this.m_sCharName + '/' + sMsg);
        }

        public string sub_49ADB8(string sMsg, string sStr, string sText)
        {
            string result;
            string s14;
            string s18;
            int n10 = sMsg.IndexOf(sStr);
            if (n10 > -1)
            {
                s14 = sMsg.Substring(0, n10);
                s18 = sMsg.Substring(sStr.Length + n10, sMsg.Length - (sStr.Length + n10));
                result = s14 + sText + s18;
            }
            else
            {
                result = sMsg;
            }
            return result;
        }

        public virtual void UserSelect(TPlayObject PlayObject, string sData)
        {
            string sLabel = string.Empty;
            PlayObject.m_nScriptGotoCount = 0;
            if ((!string.IsNullOrEmpty(sData)) && (sData[0] == '@'))// 处理脚本命令 @back 返回上级标签内容
            {
                HUtil32.GetValidStr3(sData, ref sLabel, new char[] { '\r' });
                if (PlayObject.m_sScriptCurrLable != sLabel)
                {
                    if (sLabel != M2Share.sBACK)
                    {
                        PlayObject.m_sScriptGoBackLable = PlayObject.m_sScriptCurrLable;
                        PlayObject.m_sScriptCurrLable = sLabel;
                    }
                    else
                    {
                        if (PlayObject.m_sScriptCurrLable != "")
                        {
                            PlayObject.m_sScriptCurrLable = "";
                        }
                        else
                        {
                            PlayObject.m_sScriptGoBackLable = "";
                        }
                    }
                }
            }
        }

        private void ActionOfChangeNameColor(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nColor = QuestActionInfo.nParam1;
            if ((nColor < 0) || (nColor > 255))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CHANGENAMECOLOR);
                return;
            }
            PlayObject.m_btNameColor = (byte)nColor;
            PlayObject.RefNameColor();
        }

        private void ActionOfClearPassword(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            PlayObject.m_sStoragePwd = "";
            PlayObject.m_boPasswordLocked = false;
        }

        // 挂机的
        // RECALLMOB 怪物名称 等级 叛变时间 变色(0,1) 固定颜色(1 - 7)
        // 变色为0 时固定颜色才起作用
        private void ActionOfRecallmob(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TBaseObject Mon;
            if (QuestActionInfo.nParam3 <= 1)
            {
                Mon = PlayObject.MakeSlave(QuestActionInfo.sParam1, 3, HUtil32.Str_ToInt(QuestActionInfo.sParam2, 0), 100, 10 * 24 * 60 * 60);
            }
            else
            {
                Mon = PlayObject.MakeSlave(QuestActionInfo.sParam1, 3, HUtil32.Str_ToInt(QuestActionInfo.sParam2, 0), 100, QuestActionInfo.nParam3 * 60);
            }
            if (Mon != null)
            {
                if ((QuestActionInfo.sParam4 != "") && (QuestActionInfo.sParam4[1] == '1'))
                {
                    Mon.m_boAutoChangeColor = true;
                }
                else if (QuestActionInfo.nParam5 > 0)
                {
                    Mon.m_boFixColor = true;
                    Mon.m_nFixColorIdx = QuestActionInfo.nParam5 - 1;
                }
            }
        }

        private void ActionOfReNewLevel(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nReLevel = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            int nLevel = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            int nBounsuPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam3, -1);
            if ((nReLevel < 0) || (nLevel < 0) || (nBounsuPoint < 0))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_RENEWLEVEL);
                return;
            }
            if ((PlayObject.m_btReLevel + nReLevel) <= 255)
            {
                PlayObject.m_btReLevel += (byte)nReLevel;
                if (nLevel > 0)
                {
                    PlayObject.m_Abil.Level = (ushort)nLevel;
                }
                if (M2Share.g_Config.boReNewLevelClearExp)
                {
                    PlayObject.m_Abil.Exp = 0;
                }
                PlayObject.m_nBonusPoint += nBounsuPoint;
                PlayObject.SendMsg(PlayObject, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                PlayObject.HasLevelUp(0);
                PlayObject.RefShowName();
            }
        }

        private void ActionOfChangeGender(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nGender = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            if (nGender > 1)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CHANGEGENDER);
                return;
            }
            PlayObject.m_btGender = (byte)nGender;
            PlayObject.FeatureChanged();
        }

        private void ActionOfKillSlave(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TBaseObject Slave;
            for (var i = 0; i < PlayObject.m_SlaveList.Count; i++)
            {
                Slave = PlayObject.m_SlaveList[i];
                Slave.m_WAbil.HP = 0;
            }
        }

        private void ActionOfKillMonExpRate(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nRate = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            int nTime = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if ((nRate < 0) || (nTime < 0))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_KILLMONEXPRATE);
                return;
            }
            PlayObject.m_nKillMonExpRate = nRate;
            // PlayObject.m_dwKillMonExpRateTime:=_MIN(High(Word),nTime);
            PlayObject.m_dwKillMonExpRateTime = nTime;
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sChangeKillMonExpRateMsg, new object[] { PlayObject.m_nKillMonExpRate / 100, PlayObject.m_dwKillMonExpRateTime }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfMonGenEx(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sMapName = QuestActionInfo.sParam1;
            int nMapX = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            int nMapY = HUtil32.Str_ToInt(QuestActionInfo.sParam3, -1);
            string sMonName = QuestActionInfo.sParam4;
            int nRange = QuestActionInfo.nParam5;
            int nCount = QuestActionInfo.nParam6;
            if ((sMapName == "") || (nMapX <= 0) || (nMapY <= 0) || (sMapName == "") || (nRange <= 0) || (nCount <= 0))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_MONGENEX);
                return;
            }
            for (var i = 0; i < nCount; i++)
            {
                int nRandX = M2Share.RandomNumber.Random(nRange * 2 + 1) + (nMapX - nRange);
                int nRandY = M2Share.RandomNumber.Random(nRange * 2 + 1) + (nMapY - nRange);
                if (M2Share.UserEngine.RegenMonsterByName(sMapName, (short)nRandX, (short)nRandY, sMonName) == null)
                {
                    // ScriptActionError(PlayObject,'',QuestActionInfo,sSC_MONGENEX);
                    break;
                }
            }
        }

        private void ActionOfOpenMagicBox(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            short nX = 0;
            short nY = 0;
            string sMonName = QuestActionInfo.sParam1;
            if (sMonName == "")
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_OPENMAGICBOX);
                return;
            }
            PlayObject.GetFrontPosition(ref nX, ref nY);
            var Monster = M2Share.UserEngine.RegenMonsterByName(PlayObject.m_PEnvir.sMapName, nX, nY, sMonName);
            if (Monster == null)
            {
                return;
            }
            Monster.Die();
        }

        private void ActionOfPkZone(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TFireBurnEvent FireBurnEvent;
            int nRange = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            int nType = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            int nTime = HUtil32.Str_ToInt(QuestActionInfo.sParam3, -1);
            int nPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam4, -1);
            if ((nRange < 0) || (nType < 0) || (nTime < 0) || (nPoint < 0))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_PKZONE);
                return;
            }
            int nMinX = this.m_nCurrX - nRange;
            int nMaxX = this.m_nCurrX + nRange;
            int nMinY = this.m_nCurrY - nRange;
            int nMaxY = this.m_nCurrY + nRange;
            for (int nX = nMinX; nX <= nMaxX; nX++)
            {
                for (int nY = nMinY; nY <= nMaxY; nY++)
                {
                    if (((nX < nMaxX) && (nY == nMinY)) || ((nY < nMaxY) && (nX == nMinX)) || (nX == nMaxX) ||
                        (nY == nMaxY))
                    {
                        FireBurnEvent = new TFireBurnEvent(PlayObject, nX, nY, nType, nTime * 1000, nPoint);
                        M2Share.EventManager.AddEvent(FireBurnEvent);
                    }
                }
            }
        }

        private void ActionOfPowerRate(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nRate = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            var nTime = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if ((nRate < 0) || (nTime < 0))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_POWERRATE);
                return;
            }
            PlayObject.m_nPowerRate = nRate;
            // PlayObject.m_dwPowerRateTime:=_MIN(High(Word),nTime);
            PlayObject.m_dwPowerRateTime = nTime;
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sChangePowerRateMsg, new object[] { PlayObject.m_nPowerRate / 100, PlayObject.m_dwPowerRateTime }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfChangeMode(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nMode = QuestActionInfo.nParam1;
            var boOpen = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1) == 1;
            if (nMode >= 1 && nMode <= 3)
            {
                switch (nMode)
                {
                    case 1:
                        PlayObject.m_boAdminMode = boOpen;
                        if (PlayObject.m_boAdminMode)
                        {
                            PlayObject.SysMsg(M2Share.sGameMasterMode, TMsgColor.c_Green, TMsgType.t_Hint);
                        }
                        else
                        {
                            PlayObject.SysMsg(M2Share.sReleaseGameMasterMode, TMsgColor.c_Green, TMsgType.t_Hint);
                        }
                        break;
                    case 2:
                        PlayObject.m_boSuperMan = boOpen;
                        if (PlayObject.m_boSuperMan)
                        {
                            PlayObject.SysMsg(M2Share.sSupermanMode, TMsgColor.c_Green, TMsgType.t_Hint);
                        }
                        else
                        {
                            PlayObject.SysMsg(M2Share.sReleaseSupermanMode, TMsgColor.c_Green, TMsgType.t_Hint);
                        }
                        break;
                    case 3:
                        PlayObject.m_boObMode = boOpen;
                        if (PlayObject.m_boObMode)
                        {
                            PlayObject.SysMsg(M2Share.sObserverMode, TMsgColor.c_Green, TMsgType.t_Hint);
                        }
                        else
                        {
                            PlayObject.SysMsg(M2Share.g_sReleaseObserverMode, TMsgColor.c_Green, TMsgType.t_Hint);
                        }
                        break;
                }
            }
            else
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CHANGEMODE);
            }
        }

        private void ActionOfChangePerMission(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nPermission = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            if (nPermission >= 0 && nPermission <= 10)
            {
                PlayObject.m_btPermission = (byte)nPermission;
            }
            else
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CHANGEPERMISSION);
                return;
            }
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sChangePermissionMsg, new byte[] { PlayObject.m_btPermission }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfGiveItem(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TUserItem UserItem;
            MirItem StdItem;
            var sItemName = QuestActionInfo.sParam1;
            var nItemCount = QuestActionInfo.nParam2;
            if ((sItemName == "") || (nItemCount <= 0))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_GIVE);
                return;
            }
            if (sItemName.ToLower().CompareTo(Grobal2.sSTRING_GOLDNAME.ToLower()) == 0)
            {
                PlayObject.IncGold(nItemCount);
                PlayObject.GoldChanged();
                if (M2Share.g_boGameLogGold)
                {
                    M2Share.AddGameDataLog('9' + "\t" + PlayObject.m_sMapName + "\t" + PlayObject.m_nCurrX + "\t" + PlayObject.m_nCurrY + "\t" + PlayObject.m_sCharName + "\t" + Grobal2.sSTRING_GOLDNAME + "\t" + nItemCount + "\t" + '1' + "\t" + this.m_sCharName);
                }
                return;
            }
            if (M2Share.UserEngine.GetStdItemIdx(sItemName) > 0)
            {
                if (!(nItemCount >= 1 && nItemCount <= 50))
                {
                    nItemCount = 1;
                }
                // 12.28 改上一条
                for (var I = 0; I < nItemCount; I++)
                {
                    // nItemCount 为0时出死循环
                    if (PlayObject.IsEnoughBag())
                    {
                        UserItem = new TUserItem();
                        if (M2Share.UserEngine.CopyToUserItemFromName(sItemName, ref UserItem))
                        {
                            PlayObject.m_ItemList.Add(UserItem);
                            PlayObject.SendAddItem(UserItem);
                            StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                            if (StdItem.NeedIdentify == 1)
                            {
                                M2Share.AddGameDataLog('9' + "\t" + PlayObject.m_sMapName + "\t" + PlayObject.m_nCurrX + "\t" + PlayObject.m_nCurrY + "\t" + PlayObject.m_sCharName + "\t" + sItemName + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + this.m_sCharName);
                            }
                        }
                        else
                        {
                            Dispose(UserItem);
                        }
                    }
                    else
                    {
                        UserItem = new TUserItem();
                        if (M2Share.UserEngine.CopyToUserItemFromName(sItemName, ref UserItem))
                        {
                            StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                            if (StdItem.NeedIdentify == 1)
                            {
                                M2Share.AddGameDataLog('9' + "\t" + PlayObject.m_sMapName + "\t" + PlayObject.m_nCurrX + "\t" + PlayObject.m_nCurrY + "\t" + PlayObject.m_sCharName + "\t" + sItemName + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + this.m_sCharName);
                            }
                            PlayObject.DropItemDown(UserItem, 3, false, PlayObject, null);
                        }
                        Dispose(UserItem);
                    }
                }
            }
        }

        private void ActionOfGmExecute(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sParam1 = QuestActionInfo.sParam1;
            string sParam2 = QuestActionInfo.sParam2;
            string sParam3 = QuestActionInfo.sParam3;
            string sParam4 = QuestActionInfo.sParam4;
            string sParam5 = QuestActionInfo.sParam5;
            string sParam6 = QuestActionInfo.sParam6;
            if (sParam2.ToLower().CompareTo("Self".ToLower()) == 0)
            {
                sParam2 = PlayObject.m_sCharName;
            }
            string sData = format("@{0} {1} {2} {3} {4} {5}", sParam1, sParam2, sParam3, sParam4, sParam5, sParam6);
            byte btOldPermission = PlayObject.m_btPermission;
            try
            {
                PlayObject.m_btPermission = 10;
                PlayObject.ProcessUserLineMsg(sData);
            }
            finally
            {
                PlayObject.m_btPermission = btOldPermission;
            }
        }

        private void ActionOfGuildAuraePoint(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nAuraePoint = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nAuraePoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_AURAEPOINT);
                return;
            }
            if (PlayObject.m_MyGuild == null)
            {
                PlayObject.SysMsg(M2Share.g_sScriptGuildAuraePointNoGuild, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var Guild = PlayObject.m_MyGuild;
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    Guild.nAurae = nAuraePoint;
                    break;
                case '-':
                    if (Guild.nAurae >= nAuraePoint)
                    {
                        Guild.nAurae = Guild.nAurae - nAuraePoint;
                    }
                    else
                    {
                        Guild.nAurae = 0;
                    }
                    break;
                case '+':
                    if ((int.MaxValue - Guild.nAurae) >= nAuraePoint)
                    {
                        Guild.nAurae = Guild.nAurae + nAuraePoint;
                    }
                    else
                    {
                        Guild.nAurae = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sScriptGuildAuraePointMsg, new int[] { Guild.nAurae }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfGuildBuildPoint(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nBuildPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nBuildPoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_BUILDPOINT);
                return;
            }
            if (PlayObject.m_MyGuild == null)
            {
                PlayObject.SysMsg(M2Share.g_sScriptGuildBuildPointNoGuild, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var Guild = PlayObject.m_MyGuild;
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    Guild.nBuildPoint = nBuildPoint;
                    break;
                case '-':
                    if (Guild.nBuildPoint >= nBuildPoint)
                    {
                        Guild.nBuildPoint = Guild.nBuildPoint - nBuildPoint;
                    }
                    else
                    {
                        Guild.nBuildPoint = 0;
                    }
                    break;
                case '+':
                    if ((int.MaxValue - Guild.nBuildPoint) >= nBuildPoint)
                    {
                        Guild.nBuildPoint = Guild.nBuildPoint + nBuildPoint;
                    }
                    else
                    {
                        Guild.nBuildPoint = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sScriptGuildBuildPointMsg, new[] { Guild.nBuildPoint }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfGuildChiefItemCount(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nItemCount = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nItemCount < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_GUILDCHIEFITEMCOUNT);
                return;
            }
            if (PlayObject.m_MyGuild == null)
            {
                PlayObject.SysMsg(M2Share.g_sScriptGuildFlourishPointNoGuild, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var Guild = PlayObject.m_MyGuild;
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    Guild.nChiefItemCount = nItemCount;
                    break;
                case '-':
                    if (Guild.nChiefItemCount >= nItemCount)
                    {
                        Guild.nChiefItemCount = Guild.nChiefItemCount - nItemCount;
                    }
                    else
                    {
                        Guild.nChiefItemCount = 0;
                    }
                    break;
                case '+':
                    if ((int.MaxValue - Guild.nChiefItemCount) >= nItemCount)
                    {
                        Guild.nChiefItemCount = Guild.nChiefItemCount + nItemCount;
                    }
                    else
                    {
                        Guild.nChiefItemCount = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sScriptChiefItemCountMsg, new int[] { Guild.nChiefItemCount }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfGuildFlourishPoint(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nFlourishPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nFlourishPoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_FLOURISHPOINT);
                return;
            }
            if (PlayObject.m_MyGuild == null)
            {
                PlayObject.SysMsg(M2Share.g_sScriptGuildFlourishPointNoGuild, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var Guild = PlayObject.m_MyGuild;
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    Guild.nFlourishing = nFlourishPoint;
                    break;
                case '-':
                    if (Guild.nFlourishing >= nFlourishPoint)
                    {
                        Guild.nFlourishing = Guild.nFlourishing - nFlourishPoint;
                    }
                    else
                    {
                        Guild.nFlourishing = 0;
                    }
                    break;
                case '+':
                    if ((int.MaxValue - Guild.nFlourishing) >= nFlourishPoint)
                    {
                        Guild.nFlourishing = Guild.nFlourishing + nFlourishPoint;
                    }
                    else
                    {
                        Guild.nFlourishing = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sScriptGuildFlourishPointMsg, new int[] { Guild.nFlourishing }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfGuildstabilityPoint(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nStabilityPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nStabilityPoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_STABILITYPOINT);
                return;
            }
            if (PlayObject.m_MyGuild == null)
            {
                PlayObject.SysMsg(M2Share.g_sScriptGuildStabilityPointNoGuild, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var Guild = PlayObject.m_MyGuild;
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    Guild.nStability = nStabilityPoint;
                    break;
                case '-':
                    if (Guild.nStability >= nStabilityPoint)
                    {
                        Guild.nStability = Guild.nStability - nStabilityPoint;
                    }
                    else
                    {
                        Guild.nStability = 0;
                    }
                    break;
                case '+':
                    if ((int.MaxValue - Guild.nStability) >= nStabilityPoint)
                    {
                        Guild.nStability = Guild.nStability + nStabilityPoint;
                    }
                    else
                    {
                        Guild.nStability = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sScriptGuildStabilityPointMsg, new int[] { Guild.nStability }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfHumanHP(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nHP = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nHP < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_HUMANHP);
                return;
            }
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    PlayObject.m_WAbil.HP = (ushort)nHP;
                    break;
                case '-':
                    if (PlayObject.m_WAbil.HP >= nHP)
                    {
                        PlayObject.m_WAbil.HP -= (ushort)nHP;
                    }
                    else
                    {
                        PlayObject.m_WAbil.HP = 0;
                    }
                    break;
                case '+':
                    PlayObject.m_WAbil.HP += (ushort)nHP;
                    if (PlayObject.m_WAbil.HP > PlayObject.m_WAbil.MaxHP)
                    {
                        PlayObject.m_WAbil.HP = PlayObject.m_WAbil.MaxHP;
                    }
                    break;
            }
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sScriptChangeHumanHPMsg, new ushort[] { PlayObject.m_WAbil.MaxHP }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfHumanMP(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nMP = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nMP < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_HUMANMP);
                return;
            }
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    PlayObject.m_WAbil.MP = (ushort)nMP;
                    break;
                case '-':
                    if (PlayObject.m_WAbil.MP >= nMP)
                    {
                        PlayObject.m_WAbil.MP -= (ushort)nMP;
                    }
                    else
                    {
                        PlayObject.m_WAbil.MP = 0;
                    }
                    break;
                case '+':
                    PlayObject.m_WAbil.MP += (ushort)nMP;
                    if (PlayObject.m_WAbil.MP > PlayObject.m_WAbil.MaxMP)
                    {
                        PlayObject.m_WAbil.MP = PlayObject.m_WAbil.MaxMP;
                    }
                    break;
            }
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sScriptChangeHumanMPMsg, new ushort[] { PlayObject.m_WAbil.MaxMP }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfKick(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            PlayObject.m_boKickFlag = true;
        }

        private void ActionOfKill(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nMode = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            if (nMode >= 0 && nMode <= 3)
            {
                switch (nMode)
                {
                    case 1:
                        PlayObject.m_boNoItem = true;
                        PlayObject.Die();
                        break;
                    case 2:
                        PlayObject.SetLastHiter(this);
                        PlayObject.Die();
                        break;
                    case 3:
                        PlayObject.m_boNoItem = true;
                        PlayObject.SetLastHiter(this);
                        PlayObject.Die();
                        break;
                    default:
                        PlayObject.Die();
                        break;
                }
            }
            else
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_KILL);
            }
        }

        private void ActionOfBonusPoint(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nBonusPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if ((nBonusPoint < 0) || (nBonusPoint > 10000))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_BONUSPOINT);
                return;
            }
            char cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    //FillChar(PlayObject.m_BonusAbil, sizeof(TNakedAbility), '\0');
                    PlayObject.HasLevelUp(0);
                    PlayObject.m_nBonusPoint = nBonusPoint;
                    PlayObject.SendMsg(PlayObject, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                    break;
                case '-':
                    break;
                case '+':
                    PlayObject.m_nBonusPoint += nBonusPoint;
                    PlayObject.SendMsg(PlayObject, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                    break;
            }
        }

        private void ActionOfDelMarry(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            PlayObject.m_sDearName = "";
            PlayObject.RefShowName();
        }

        private void ActionOfDelMaster(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            PlayObject.m_sMasterName = "";
            PlayObject.RefShowName();
        }

        private void ActionOfRestBonusPoint(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nTotleUsePoint = PlayObject.m_BonusAbil.DC + PlayObject.m_BonusAbil.MC + PlayObject.m_BonusAbil.SC + PlayObject.m_BonusAbil.AC + PlayObject.m_BonusAbil.MAC + PlayObject.m_BonusAbil.HP + PlayObject.m_BonusAbil.MP + PlayObject.m_BonusAbil.Hit + PlayObject.m_BonusAbil.Speed + PlayObject.m_BonusAbil.X2;
            //FillChar(PlayObject.m_BonusAbil, sizeof(TNakedAbility), '\0');
            PlayObject.m_nBonusPoint += nTotleUsePoint;
            PlayObject.SendMsg(PlayObject, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
            PlayObject.HasLevelUp(0);
            PlayObject.SysMsg("分配点数已复位！！！", TMsgColor.c_Red, TMsgType.t_Hint);
        }

        private void ActionOfRestReNewLevel(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            PlayObject.m_btReLevel = 0;
            PlayObject.HasLevelUp(0);
        }

        private void ActionOfSetMapMode(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sMapName = QuestActionInfo.sParam1;
            string sMapMode = QuestActionInfo.sParam2;
            string sParam1 = QuestActionInfo.sParam3;
            string sParam2 = QuestActionInfo.sParam4;
            TEnvirnoment Envir = M2Share.g_MapManager.FindMap(sMapName);
            if ((Envir == null) || (sMapMode == ""))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_SETMAPMODE);
                return;
            }
            if (sMapMode.ToLower().CompareTo("SAFE".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boSAFE = true;
                }
                else
                {
                    Envir.Flag.boSAFE = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("DARK".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boDarkness = true;
                }
                else
                {
                    Envir.Flag.boDarkness = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("FIGHT".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boFightZone = true;
                }
                else
                {
                    Envir.Flag.boFightZone = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("FIGHT3".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boFight3Zone = true;
                }
                else
                {
                    Envir.Flag.boFight3Zone = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("DAY".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boDayLight = true;
                }
                else
                {
                    Envir.Flag.boDayLight = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("QUIZ".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boQUIZ = true;
                }
                else
                {
                    Envir.Flag.boQUIZ = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NORECONNECT".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNORECONNECT = true;
                    Envir.Flag.sNoReConnectMap = sParam1;
                }
                else
                {
                    Envir.Flag.boNORECONNECT = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("MUSIC".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boMUSIC = true;
                    Envir.Flag.nMUSICID = HUtil32.Str_ToInt(sParam1, -1);
                }
                else
                {
                    Envir.Flag.boMUSIC = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("EXPRATE".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boEXPRATE = true;
                    Envir.Flag.nEXPRATE = HUtil32.Str_ToInt(sParam1, -1);
                }
                else
                {
                    Envir.Flag.boEXPRATE = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("PKWINLEVEL".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boPKWINLEVEL = true;
                    Envir.Flag.nPKWINLEVEL = HUtil32.Str_ToInt(sParam1, -1);
                }
                else
                {
                    Envir.Flag.boPKWINLEVEL = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("PKWINEXP".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boPKWINEXP = true;
                    Envir.Flag.nPKWINEXP = HUtil32.Str_ToInt(sParam1, -1);
                }
                else
                {
                    Envir.Flag.boPKWINEXP = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("PKLOSTLEVEL".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boPKLOSTLEVEL = true;
                    Envir.Flag.nPKLOSTLEVEL = HUtil32.Str_ToInt(sParam1, -1);
                }
                else
                {
                    Envir.Flag.boPKLOSTLEVEL = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("PKLOSTEXP".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boPKLOSTEXP = true;
                    Envir.Flag.nPKLOSTEXP = HUtil32.Str_ToInt(sParam1, -1);
                }
                else
                {
                    Envir.Flag.boPKLOSTEXP = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("DECHP".ToLower()) == 0)
            {
                if ((sParam1 != "") && (sParam2 != ""))
                {
                    Envir.Flag.boDECHP = true;
                    Envir.Flag.nDECHPTIME = HUtil32.Str_ToInt(sParam1, -1);
                    Envir.Flag.nDECHPPOINT = HUtil32.Str_ToInt(sParam2, -1);
                }
                else
                {
                    Envir.Flag.boDECHP = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("DECGAMEGOLD".ToLower()) == 0)
            {
                if ((sParam1 != "") && (sParam2 != ""))
                {
                    Envir.Flag.boDECGAMEGOLD = true;
                    Envir.Flag.nDECGAMEGOLDTIME = HUtil32.Str_ToInt(sParam1, -1);
                    Envir.Flag.nDECGAMEGOLD = HUtil32.Str_ToInt(sParam2, -1);
                }
                else
                {
                    Envir.Flag.boDECGAMEGOLD = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("RUNHUMAN".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boRUNHUMAN = true;
                }
                else
                {
                    Envir.Flag.boRUNHUMAN = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("RUNMON".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boRUNMON = true;
                }
                else
                {
                    Envir.Flag.boRUNMON = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NEEDHOLE".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNEEDHOLE = true;
                }
                else
                {
                    Envir.Flag.boNEEDHOLE = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NORECALL".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNORECALL = true;
                }
                else
                {
                    Envir.Flag.boNORECALL = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NOGUILDRECALL".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNOGUILDRECALL = true;
                }
                else
                {
                    Envir.Flag.boNOGUILDRECALL = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NODEARRECALL".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNODEARRECALL = true;
                }
                else
                {
                    Envir.Flag.boNODEARRECALL = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NOMASTERRECALL".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNOMASTERRECALL = true;
                }
                else
                {
                    Envir.Flag.boNOMASTERRECALL = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NORANDOMMOVE".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNORANDOMMOVE = true;
                }
                else
                {
                    Envir.Flag.boNORANDOMMOVE = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NODRUG".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNODRUG = true;
                }
                else
                {
                    Envir.Flag.boNODRUG = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("MINE".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boMINE = true;
                }
                else
                {
                    Envir.Flag.boMINE = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("MINE2".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boMINE2 = true;
                }
                else
                {
                    Envir.Flag.boMINE2 = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NOTHROWITEM".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNOTHROWITEM = true;
                }
                else
                {
                    Envir.Flag.boNOTHROWITEM = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NODROPITEM".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNODROPITEM = true;
                }
                else
                {
                    Envir.Flag.boNODROPITEM = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NOPOSITIONMOVE".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNOPOSITIONMOVE = true;
                }
                else
                {
                    Envir.Flag.boNOPOSITIONMOVE = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NOHORSE".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNOHORSE = true;
                }
                else
                {
                    Envir.Flag.boNOHORSE = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NOCHAT".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNOCHAT = true;
                }
                else
                {
                    Envir.Flag.boNOCHAT = false;
                }
            }
        }

        private void ActionOfSetMemberLevel(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nLevel = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_SETMEMBERLEVEL);
                return;
            }
            char cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    PlayObject.m_nMemberLevel = nLevel;
                    break;
                case '-':
                    PlayObject.m_nMemberLevel -= nLevel;
                    if (PlayObject.m_nMemberLevel < 0)
                    {
                        PlayObject.m_nMemberLevel = 0;
                    }
                    break;
                case '+':
                    PlayObject.m_nMemberLevel += nLevel;
                    if (PlayObject.m_nMemberLevel > 65535)
                    {
                        PlayObject.m_nMemberLevel = 65535;
                    }
                    break;
            }
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sChangeMemberLevelMsg, new int[] { PlayObject.m_nMemberLevel }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfSetMemberType(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nType = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nType < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_SETMEMBERTYPE);
                return;
            }
            char cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    PlayObject.m_nMemberType = nType;
                    break;
                case '-':
                    PlayObject.m_nMemberType -= nType;
                    if (PlayObject.m_nMemberType < 0)
                    {
                        PlayObject.m_nMemberType = 0;
                    }
                    break;
                case '+':
                    PlayObject.m_nMemberType += nType;
                    if (PlayObject.m_nMemberType > 65535)
                    {
                        PlayObject.m_nMemberType = 65535;
                    }
                    break;
            }
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sChangeMemberTypeMsg, new int[] { PlayObject.m_nMemberType }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private bool ConditionOfCheckRangeMonCount(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            TBaseObject BaseObject;
            bool result = false;
            string sMapName = QuestConditionInfo.sParam1;
            int nX = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            int nY = HUtil32.Str_ToInt(QuestConditionInfo.sParam3, -1);
            int nRange = HUtil32.Str_ToInt(QuestConditionInfo.sParam4, -1);
            char cMethod = QuestConditionInfo.sParam5[0];
            int nCount = HUtil32.Str_ToInt(QuestConditionInfo.sParam6, -1);
            TEnvirnoment Envir = M2Share.g_MapManager.FindMap(sMapName);
            if ((Envir == null) || (nX < 0) || (nY < 0) || (nRange < 0) || (nCount < 0))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKRANGEMONCOUNT);
                return result;
            }
            IList<TBaseObject> MonList = new List<TBaseObject>();
            int nMapRangeCount = Envir.GetRangeBaseObject(nX, nY, nRange, true, MonList);
            for (var i = MonList.Count - 1; i >= 0; i--)
            {
                BaseObject = MonList[i];
                if ((BaseObject.m_btRaceServer < Grobal2.RC_ANIMAL) || (BaseObject.m_btRaceServer == Grobal2.RC_ARCHERGUARD) || (BaseObject.m_Master != null))
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

        private bool ConditionOfCheckReNewLevel(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            int nLevel = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKLEVELEX);
                return result;
            }
            char cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_btReLevel == nLevel)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_btReLevel > nLevel)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_btReLevel < nLevel)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_btReLevel >= nLevel)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckSlaveLevel(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            TBaseObject BaseObject;
            bool result = false;
            int nLevel = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKLEVELEX);
                return result;
            }
            var nSlaveLevel = -1;
            for (var i = 0; i < PlayObject.m_SlaveList.Count; i++)
            {
                BaseObject = PlayObject.m_SlaveList[i];
                if (BaseObject.m_Abil.Level > nSlaveLevel)
                {
                    nSlaveLevel = BaseObject.m_Abil.Level;
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

        private bool ConditionOfCheckUseItem(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            int nWhere = HUtil32.Str_ToInt(QuestConditionInfo.sParam1, -1);
            if ((nWhere < 0) || (nWhere > PlayObject.m_UseItems.GetUpperBound(0)))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKUSEITEM);
                return result;
            }
            if (PlayObject.m_UseItems[nWhere].wIndex > 0)
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckVar(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            string sName = string.Empty;
            TDynamicVar DynamicVar;
            bool boFoundVar = false;
            var result = false;
            var sType = QuestConditionInfo.sParam1;
            var sVarName = QuestConditionInfo.sParam2;
            var sMethod = QuestConditionInfo.sParam3;
            var nVarValue = HUtil32.Str_ToInt(QuestConditionInfo.sParam4, 0);
            var sVarValue = QuestConditionInfo.sParam4;
            if ((sType == "") || (sVarName == "") || (sMethod == ""))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKVAR);
                return result;
            }
            var cMethod = sMethod[0];
            var DynamicVarList = GetDynamicVarList(PlayObject, sType, ref sName);
            if (DynamicVarList == null)
            {
                // ,format(sVarTypeError,[sType])
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKVAR);
                return result;
            }
            for (var i = 0; i < DynamicVarList.Count; i++)
            {
                DynamicVar = DynamicVarList[i];
                if (DynamicVar.sName.ToLower().CompareTo(sVarName.ToLower()) == 0)
                {
                    switch (DynamicVar.VarType)
                    {
                        case TVarType.VInteger:
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
                        case TVarType.VString:
                            break;
                    }
                    boFoundVar = true;
                    break;
                }
            }
            if (!boFoundVar)
            {
                // format(sVarFound,[sVarName,sType]),
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKVAR);
            }
            return result;
        }

        private bool ConditionOfHaveMaster(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            return !string.IsNullOrEmpty(PlayObject.m_sMasterName);
        }

        private bool ConditionOfPoseHaveMaster(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var PoseHuman = PlayObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.m_btRaceServer == Grobal2.RC_PLAYOBJECT))
            {
                if (((TPlayObject)PoseHuman).m_sMasterName != "")
                {
                    result = true;
                }
            }
            return result;
        }

        private void ActionOfUnMaster(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sMsg;
            if (PlayObject.m_sMasterName == "")
            {
                GotoLable(PlayObject, "@ExeMasterFail", false);
                return;
            }
            var PoseHuman = (TPlayObject)PlayObject.GetPoseCreate();
            if (PoseHuman == null)
            {
                GotoLable(PlayObject, "@UnMasterCheckDir", false);
            }
            if (PoseHuman != null)
            {
                if (QuestActionInfo.sParam1 == "")
                {
                    if (PoseHuman.m_btRaceServer != Grobal2.RC_PLAYOBJECT)
                    {
                        GotoLable(PlayObject, "@UnMasterTypeErr", false);
                        return;
                    }
                    if (PoseHuman.GetPoseCreate() == PlayObject)
                    {
                        if (PlayObject.m_sMasterName == PoseHuman.m_sCharName)
                        {
                            if (PlayObject.m_boMaster)
                            {
                                GotoLable(PlayObject, "@UnIsMaster", false);
                                return;
                            }
                            if (PlayObject.m_sMasterName != PoseHuman.m_sCharName)
                            {
                                GotoLable(PlayObject, "@UnMasterError", false);
                                return;
                            }
                            GotoLable(PlayObject, "@StartUnMaster", false);
                            GotoLable(PoseHuman, "@WateUnMaster", false);
                            return;
                        }
                    }
                }
            }
            // sREQUESTUNMARRY
            if (QuestActionInfo.sParam1.ToLower().CompareTo("REQUESTUNMASTER".ToLower()) == 0)
            {
                if (QuestActionInfo.sParam2 == "")
                {
                    if (PoseHuman != null)
                    {
                        PlayObject.m_boStartUnMaster = true;
                        if (PlayObject.m_boStartUnMaster && PoseHuman.m_boStartUnMaster)
                        {
                            sMsg = M2Share.g_sNPCSayUnMasterOKMsg.Replace("%n", this.m_sCharName);
                            sMsg = sMsg.Replace("%s", PlayObject.m_sCharName);
                            sMsg = sMsg.Replace("%d", PoseHuman.m_sCharName);
                            M2Share.UserEngine.SendBroadCastMsg(sMsg, TMsgType.t_Say);
                            PlayObject.m_sMasterName = "";
                            PoseHuman.m_sMasterName = "";
                            PlayObject.m_boStartUnMaster = false;
                            PoseHuman.m_boStartUnMaster = false;
                            PlayObject.RefShowName();
                            PoseHuman.RefShowName();
                            GotoLable(PlayObject, "@UnMasterEnd", false);
                            GotoLable(PoseHuman, "@UnMasterEnd", false);
                        }
                        else
                        {
                            GotoLable(PlayObject, "@WateUnMaster", false);
                            GotoLable(PoseHuman, "@RevUnMaster", false);
                        }
                    }
                    return;
                }
                else
                {
                    // 强行出师
                    if (QuestActionInfo.sParam2.ToLower().CompareTo("FORCE".ToLower()) == 0)
                    {
                        sMsg = M2Share.g_sNPCSayForceUnMasterMsg.Replace("%n", this.m_sCharName);
                        sMsg = sMsg.Replace("%s", PlayObject.m_sCharName);
                        sMsg = sMsg.Replace("%d", PlayObject.m_sMasterName);
                        M2Share.UserEngine.SendBroadCastMsg(sMsg, TMsgType.t_Say);
                        PoseHuman = M2Share.UserEngine.GetPlayObject(PlayObject.m_sMasterName);
                        if (PoseHuman != null)
                        {
                            PoseHuman.m_sMasterName = "";
                            PoseHuman.RefShowName();
                        }
                        else
                        {
                            M2Share.g_UnForceMasterList.Add(PlayObject.m_sMasterName);
                            M2Share.SaveUnForceMasterList();
                        }
                        PlayObject.m_sMasterName = "";
                        GotoLable(PlayObject, "@UnMasterEnd", false);
                        PlayObject.RefShowName();
                    }
                    return;
                }
            }
        }

        private bool ConditionOfCheckCastleGold(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nGold = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if ((nGold < 0) || (this.m_Castle == null))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKCASTLEGOLD);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (this.m_Castle.m_nTotalGold == nGold)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (this.m_Castle.m_nTotalGold > nGold)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (this.m_Castle.m_nTotalGold < nGold)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (this.m_Castle.m_nTotalGold >= nGold)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckContribution(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nContribution = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nContribution < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKCONTRIBUTION);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_wContribution == nContribution)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_wContribution > nContribution)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_wContribution < nContribution)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_wContribution >= nContribution)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfCheckCreditPoint(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nCreditPoint = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nCreditPoint < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKCREDITPOINT);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_btCreditPoint == nCreditPoint)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_btCreditPoint > nCreditPoint)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_btCreditPoint < nCreditPoint)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_btCreditPoint >= nCreditPoint)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private void ActionOfClearNeedItems(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TUserItem UserItem;
            MirItem StdItem;
            var nNeed = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            if (nNeed < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CLEARNEEDITEMS);
                return;
            }
            for (var i = PlayObject.m_ItemList.Count - 1; i >= 0; i--)
            {
                UserItem = PlayObject.m_ItemList[i];
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if ((StdItem != null) && (StdItem.Need == nNeed))
                {
                    PlayObject.SendDelItems(UserItem);
                    Dispose(UserItem);
                    PlayObject.m_ItemList.RemoveAt(i);
                }
            }
            for (var i = PlayObject.m_StorageItemList.Count - 1; i >= 0; i--)
            {
                UserItem = PlayObject.m_StorageItemList[i];
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if ((StdItem != null) && (StdItem.Need == nNeed))
                {
                    Dispose(UserItem);
                    PlayObject.m_StorageItemList.RemoveAt(i);
                }
            }
        }

        private void ActionOfClearMakeItems(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TUserItem UserItem;
            MirItem StdItem;
            string sItemName = QuestActionInfo.sParam1;
            var nMakeIndex = QuestActionInfo.nParam2;
            var boMatchName = QuestActionInfo.sParam3 == "1";
            if ((sItemName == "") || (nMakeIndex <= 0))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CLEARMAKEITEMS);
                return;
            }
            for (var i = PlayObject.m_ItemList.Count - 1; i >= 0; i--)
            {
                UserItem = PlayObject.m_ItemList[i];
                if (UserItem.MakeIndex != nMakeIndex)
                {
                    continue;
                }
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (!boMatchName || ((StdItem != null) && (StdItem.Name.ToLower().CompareTo(sItemName.ToLower()) == 0)))
                {
                    PlayObject.SendDelItems(UserItem);
                    Dispose(UserItem);
                    PlayObject.m_ItemList.RemoveAt(i);
                }
            }
            for (var i = PlayObject.m_StorageItemList.Count - 1; i >= 0; i--)
            {
                UserItem = PlayObject.m_ItemList[i];
                if (UserItem.MakeIndex != nMakeIndex)
                {
                    continue;
                }
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (!boMatchName || ((StdItem != null) && (StdItem.Name.ToLower().CompareTo(sItemName.ToLower()) == 0)))
                {
                    Dispose(UserItem);
                    PlayObject.m_StorageItemList.RemoveAt(i);
                }
            }
            for (var i = PlayObject.m_UseItems.GetLowerBound(0); i <= PlayObject.m_UseItems.GetUpperBound(0); i++)
            {
                UserItem = PlayObject.m_UseItems[i];
                if (UserItem.MakeIndex != nMakeIndex)
                {
                    continue;
                }
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (!boMatchName || ((StdItem != null) && (StdItem.Name.ToLower().CompareTo(sItemName.ToLower()) == 0)))
                {
                    UserItem.wIndex = 0;
                }
            }
        }

        public virtual void SendCustemMsg(TPlayObject PlayObject, string sMsg)
        {
            if (!M2Share.g_Config.boSendCustemMsg)
            {
                PlayObject.SysMsg(M2Share.g_sSendCustMsgCanNotUseNowMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (PlayObject.m_boSendMsgFlag)
            {
                PlayObject.m_boSendMsgFlag = false;
                M2Share.UserEngine.SendBroadCastMsg(PlayObject.m_sCharName + ": " + sMsg, TMsgType.t_Cust);
            }
        }

        private bool ConditionOfCheckOfGuild(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            if (QuestConditionInfo.sParam1 == "")
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKOFGUILD);
                return result;
            }
            if (PlayObject.m_MyGuild != null)
            {
                if (PlayObject.m_MyGuild.sGuildName.ToLower().CompareTo(QuestConditionInfo.sParam1.ToLower()) == 0)
                {
                    result = true;
                }
            }
            return result;
        }

        private bool ConditionOfCheckOnlineLongMin(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nOnlineMin = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nOnlineMin < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_ONLINELONGMIN);
                return result;
            }
            var nOnlineTime = (HUtil32.GetTickCount() - PlayObject.m_dwLogonTick) / 60000;
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

        private bool ConditionOfCheckPasswordErrorCount(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nErrorCount = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, -1);
            if (nErrorCount < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_PASSWORDERRORCOUNT);
                return result;
            }
            var cMethod = QuestConditionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (PlayObject.m_btPwdFailCount == nErrorCount)
                    {
                        result = true;
                    }
                    break;
                case '>':
                    if (PlayObject.m_btPwdFailCount > nErrorCount)
                    {
                        result = true;
                    }
                    break;
                case '<':
                    if (PlayObject.m_btPwdFailCount < nErrorCount)
                    {
                        result = true;
                    }
                    break;
                default:
                    if (PlayObject.m_btPwdFailCount >= nErrorCount)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private bool ConditionOfIsLockPassword(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            return PlayObject.m_boPasswordLocked;
        }

        private bool ConditionOfIsLockStorage(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            return !PlayObject.m_boCanGetBackItem;
        }

        private bool ConditionOfCheckPayMent(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nPayMent = HUtil32.Str_ToInt(QuestConditionInfo.sParam1, -1);
            if (nPayMent < 1)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKPAYMENT);
                return result;
            }
            if (PlayObject.m_nPayMent == nPayMent)
            {
                result = true;
            }
            return result;
        }

        private bool ConditionOfCheckSlaveName(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            TBaseObject BaseObject;
            var result = false;
            var sSlaveName = QuestConditionInfo.sParam1;
            if (sSlaveName == "")
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKSLAVENAME);
                return result;
            }
            for (var i = 0; i < PlayObject.m_SlaveList.Count; i++)
            {
                BaseObject = PlayObject.m_SlaveList[i];
                if (sSlaveName.ToLower().CompareTo(BaseObject.m_sCharName.ToLower()) == 0)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private void ActionOfUpgradeItems(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nAddPoint;
            var nWhere = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            var nRate = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            var nPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam3, -1);
            if ((nWhere < 0) || (nWhere > PlayObject.m_UseItems.GetUpperBound(0)) || (nRate < 0) || (nPoint < 0) || (nPoint > 255))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_UPGRADEITEMS);
                return;
            }
            var UserItem = PlayObject.m_UseItems[nWhere];
            var StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
            if ((UserItem.wIndex <= 0) || (StdItem == null))
            {
                PlayObject.SysMsg("你身上没有戴指定物品！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nRate = M2Share.RandomNumber.Random(nRate);
            nPoint = M2Share.RandomNumber.Random(nPoint);
            var nValType = M2Share.RandomNumber.Random(14);
            if (nRate != 0)
            {
                PlayObject.SysMsg("装备升级失败！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (nValType == 14)
            {
                nAddPoint = nPoint * 1000;
                if (UserItem.DuraMax + nAddPoint > ushort.MaxValue)
                {
                    nAddPoint = ushort.MaxValue - UserItem.DuraMax;
                }
                UserItem.DuraMax = (ushort)(UserItem.DuraMax + nAddPoint);
            }
            else
            {
                nAddPoint = nPoint;
                if (UserItem.btValue[nValType] + nAddPoint > byte.MaxValue)
                {
                    nAddPoint = byte.MaxValue - UserItem.btValue[nValType];
                }
                UserItem.btValue[nValType] = (byte)(UserItem.btValue[nValType] + nAddPoint);
            }
            PlayObject.SendUpdateItem(UserItem);
            PlayObject.SysMsg("装备升级成功", TMsgColor.c_Green, TMsgType.t_Hint);
            PlayObject.SysMsg(StdItem.Name + ": " + UserItem.Dura + '/' + UserItem.DuraMax + '/' + UserItem.btValue[0] + '/' + UserItem.btValue[1] + '/' + UserItem.btValue[2] + '/' + UserItem.btValue[3] + '/' + UserItem.btValue[4] + '/' + UserItem.btValue[5] + '/' + UserItem.btValue[6] + '/' + UserItem.btValue[7] + '/' + UserItem.btValue[8] + '/' + UserItem.btValue[9] + '/' + UserItem.btValue[10] + '/' + UserItem.btValue[11] + '/' + UserItem.btValue[12] + '/' + UserItem.btValue[13], TMsgColor.c_Blue, TMsgType.t_Hint);
        }

        private void ActionOfUpgradeItemsEx(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nAddPoint;
            var nWhere = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            var nValType = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            var nRate = HUtil32.Str_ToInt(QuestActionInfo.sParam3, -1);
            var nPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam4, -1);
            var nUpgradeItemStatus = HUtil32.Str_ToInt(QuestActionInfo.sParam5, -1);
            if ((nValType < 0) || (nValType > 14) || (nWhere < 0) || (nWhere > PlayObject.m_UseItems.GetUpperBound(0)) || (nRate < 0) || (nPoint < 0) || (nPoint > 255))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_UPGRADEITEMSEX);
                return;
            }
            var UserItem = PlayObject.m_UseItems[nWhere];
            var StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
            if ((UserItem.wIndex <= 0) || (StdItem == null))
            {
                PlayObject.SysMsg("你身上没有戴指定物品！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var nRatePoint = M2Share.RandomNumber.Random(nRate * 10);
            nPoint = HUtil32._MAX(1, M2Share.RandomNumber.Random(nPoint));
            if (!(nRatePoint >= 0 && nRatePoint <= 10))
            {
                switch (nUpgradeItemStatus)
                {
                    case 0:
                        PlayObject.SysMsg("装备升级未成功！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                        break;
                    case 1:
                        PlayObject.SendDelItems(UserItem);
                        UserItem.wIndex = 0;
                        PlayObject.SysMsg("装备破碎！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                        break;
                    case 2:
                        PlayObject.SysMsg("装备升级失败，装备属性恢复默认！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                        if (nValType != 14)
                        {
                            UserItem.btValue[nValType] = 0;
                        }
                        break;
                }
                return;
            }
            if (nValType == 14)
            {
                nAddPoint = nPoint * 1000;
                if (UserItem.DuraMax + nAddPoint > ushort.MaxValue)
                {
                    nAddPoint = ushort.MaxValue - UserItem.DuraMax;
                }
                UserItem.DuraMax = (ushort)(UserItem.DuraMax + nAddPoint);
            }
            else
            {
                nAddPoint = nPoint;
                if (UserItem.btValue[nValType] + nAddPoint > byte.MaxValue)
                {
                    nAddPoint = byte.MaxValue - UserItem.btValue[nValType];
                }
                UserItem.btValue[nValType] = (byte)(UserItem.btValue[nValType] + nAddPoint);
            }
            PlayObject.SendUpdateItem(UserItem);
            PlayObject.SysMsg("装备升级成功", TMsgColor.c_Green, TMsgType.t_Hint);
            PlayObject.SysMsg(StdItem.Name + ": " + UserItem.Dura + '/' + UserItem.DuraMax + '-' + UserItem.btValue[0] + '/' + UserItem.btValue[1] + '/' + UserItem.btValue[2] + '/' + UserItem.btValue[3] + '/' + UserItem.btValue[4] + '/' + UserItem.btValue[5] + '/' + UserItem.btValue[6] + '/' + UserItem.btValue[7] + '/' + UserItem.btValue[8] + '/' + UserItem.btValue[9] + '/' + UserItem.btValue[10] + '/' + UserItem.btValue[11] + '/' + UserItem.btValue[12] + '/' + UserItem.btValue[13], TMsgColor.c_Blue, TMsgType.t_Hint);
        }

        // 声明变量
        // VAR 数据类型(Integer String) 类型(HUMAN GUILD GLOBAL) 变量值
        private void ActionOfVar(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sName = string.Empty;
            TDynamicVar DynamicVar;
            bool boFoundVar;
            IList<TDynamicVar> DynamicVarList;
            const string sVarFound = "变量%s已存在，变量类型:%s";
            const string sVarTypeError = "变量类型错误，错误类型:%s 当前支持类型(HUMAN、GUILD、GLOBAL)";
            var sType = QuestActionInfo.sParam2;
            var sVarName = QuestActionInfo.sParam3;
            var sVarValue = QuestActionInfo.sParam4;
            var nVarValue = HUtil32.Str_ToInt(QuestActionInfo.sParam4, 0);
            var VarType = TVarType.vNone;
            if (QuestActionInfo.sParam1.ToLower().CompareTo("Integer".ToLower()) == 0)
            {
                VarType = TVarType.VInteger;
            }
            if (QuestActionInfo.sParam1.ToLower().CompareTo("String".ToLower()) == 0)
            {
                VarType = TVarType.VString;
            }
            if ((sType == "") || (sVarName == "") || (VarType == TVarType.vNone))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_VAR);
                return;
            }
            DynamicVar = new TDynamicVar();
            DynamicVar.sName = sVarName;
            DynamicVar.VarType = VarType;
            DynamicVar.nInternet = nVarValue;
            DynamicVar.sString = sVarValue;
            boFoundVar = false;
            DynamicVarList = GetDynamicVarList(PlayObject, sType, ref sName);
            if (DynamicVarList == null)
            {
                Dispose(DynamicVar);
                ScriptActionError(PlayObject, format(sVarTypeError, new string[] { sType }), QuestActionInfo, M2Share.sSC_VAR);
                return;
            }
            for (var i = 0; i < DynamicVarList.Count; i++)
            {
                if (DynamicVarList[i].sName.ToLower().CompareTo(sVarName.ToLower()) == 0)
                {
                    boFoundVar = true;
                    break;
                }
            }
            if (!boFoundVar)
            {
                DynamicVarList.Add(DynamicVar);
            }
            else
            {
                ScriptActionError(PlayObject, format(sVarFound, new string[] { sVarName, sType }), QuestActionInfo, M2Share.sSC_VAR);
            }
        }

        // 读取变量值
        // LOADVAR 变量类型 变量名 文件名
        private void ActionOfLoadVar(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sName = string.Empty;
            TDynamicVar DynamicVar = null;
            bool boFoundVar;
            IList<TDynamicVar> DynamicVarList;
            IniFile IniFile;
            const string sVarFound = "变量%s不存在，变量类型:%s";
            const string sVarTypeError = "变量类型错误，错误类型:%s 当前支持类型(HUMAN、GUILD、GLOBAL)";
            var sType = QuestActionInfo.sParam1;
            var sVarName = QuestActionInfo.sParam2;
            var sFileName = M2Share.g_Config.sEnvirDir + m_sPath + QuestActionInfo.sParam3;
            if ((sType == "") || (sVarName == "") || !File.Exists(sFileName))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_LOADVAR);
                return;
            }
            boFoundVar = false;
            DynamicVarList = GetDynamicVarList(PlayObject, sType, ref sName);
            if (DynamicVarList == null)
            {
                Dispose(DynamicVar);
                ScriptActionError(PlayObject, format(sVarTypeError, new string[] { sType }), QuestActionInfo, M2Share.sSC_VAR);
                return;
            }
            IniFile = new IniFile(sFileName);
            for (var i = 0; i < DynamicVarList.Count; i++)
            {
                DynamicVar = DynamicVarList[i];
                if (DynamicVar.sName.ToLower().CompareTo(sVarName.ToLower()) == 0)
                {
                    switch (DynamicVar.VarType)
                    {
                        case TVarType.VInteger:
                            DynamicVar.nInternet = IniFile.ReadInteger(sName, DynamicVar.sName, 0);
                            break;
                        case TVarType.VString:
                            DynamicVar.sString = IniFile.ReadString(sName, DynamicVar.sName, "");
                            break;
                    }
                    boFoundVar = true;
                    break;
                }
            }
            if (!boFoundVar)
            {
                ScriptActionError(PlayObject, format(sVarFound, new string[] { sVarName, sType }), QuestActionInfo, M2Share.sSC_LOADVAR);
            }
            //IniFile.Free;
        }

        // 保存变量值
        // SAVEVAR 变量类型 变量名 文件名
        private void ActionOfSaveVar(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sName = string.Empty;
            TDynamicVar DynamicVar = null;
            bool boFoundVar;
            IList<TDynamicVar> DynamicVarList;
            IniFile IniFile;
            const string sVarFound = "变量%s不存在，变量类型:%s";
            const string sVarTypeError = "变量类型错误，错误类型:%s 当前支持类型(HUMAN、GUILD、GLOBAL)";
            string sType = QuestActionInfo.sParam1;
            string sVarName = QuestActionInfo.sParam2;
            string sFileName = M2Share.g_Config.sEnvirDir + m_sPath + QuestActionInfo.sParam3;
            if ((sType == "") || (sVarName == "") || !File.Exists(sFileName))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_SAVEVAR);
                return;
            }
            boFoundVar = false;
            DynamicVarList = GetDynamicVarList(PlayObject, sType, ref sName);
            if (DynamicVarList == null)
            {
                Dispose(DynamicVar);
                ScriptActionError(PlayObject, format(sVarTypeError, new string[] { sType }), QuestActionInfo, M2Share.sSC_VAR);
                return;
            }
            IniFile = new IniFile(sFileName);
            for (var i = 0; i < DynamicVarList.Count; i++)
            {
                DynamicVar = DynamicVarList[i];
                if (DynamicVar.sName.ToLower().CompareTo(sVarName.ToLower()) == 0)
                {
                    switch (DynamicVar.VarType)
                    {
                        case TVarType.VInteger:
                            IniFile.WriteInteger(sName, DynamicVar.sName, DynamicVar.nInternet);
                            break;
                        case TVarType.VString:
                            IniFile.WriteString(sName, DynamicVar.sName, DynamicVar.sString);
                            break;
                    }
                    boFoundVar = true;
                    break;
                }
            }
            if (!boFoundVar)
            {
                ScriptActionError(PlayObject, format(sVarFound, new string[] { sVarName, sType }), QuestActionInfo, M2Share.sSC_SAVEVAR);
            }
            //IniFile.Free;
        }

        private void ActionOfDelayCall(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            PlayObject.m_nDelayCall = HUtil32._MAX(1, QuestActionInfo.nParam1);
            PlayObject.m_sDelayCallLabel = QuestActionInfo.sParam2;
            PlayObject.m_dwDelayCallTick = HUtil32.GetTickCount();
            PlayObject.m_boDelayCall = true;
            PlayObject.m_DelayCallNPC = this.ObjectId;
        }

        // 对变量进行运算(+、-、*、/)
        private void ActionOfCalcVar(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sName = string.Empty;
            TDynamicVar DynamicVar = null;
            IList<TDynamicVar> DynamicVarList;
            const string sVarFound = "变量%s不存在，变量类型:%s";
            const string sVarTypeError = "变量类型错误，错误类型:%s 当前支持类型(HUMAN、GUILD、GLOBAL)";
            string sType = QuestActionInfo.sParam1;
            string sVarName = QuestActionInfo.sParam2;
            string sMethod = QuestActionInfo.sParam3;
            string sVarValue = QuestActionInfo.sParam4;
            int nVarValue = HUtil32.Str_ToInt(QuestActionInfo.sParam4, 0);
            if ((sType == "") || (sVarName == "") || (sMethod == ""))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CALCVAR);
                return;
            }
            bool boFoundVar = false;
            char cMethod = sMethod[0];
            DynamicVarList = GetDynamicVarList(PlayObject, sType, ref sName);
            if (DynamicVarList == null)
            {
                Dispose(DynamicVar);
                ScriptActionError(PlayObject, format(sVarTypeError, new string[] { sType }), QuestActionInfo, M2Share.sSC_CALCVAR);
                return;
            }
            for (var i = 0; i < DynamicVarList.Count; i++)
            {
                DynamicVar = DynamicVarList[i];
                if (DynamicVar.sName.ToLower().CompareTo(sVarName.ToLower()) == 0)
                {
                    switch (DynamicVar.VarType)
                    {
                        case TVarType.VInteger:
                            switch (cMethod)
                            {
                                case '=':
                                    DynamicVar.nInternet = nVarValue;
                                    break;
                                case '+':
                                    DynamicVar.nInternet = DynamicVar.nInternet + nVarValue;
                                    break;
                                case '-':
                                    DynamicVar.nInternet = DynamicVar.nInternet - nVarValue;
                                    break;
                                case '*':
                                    DynamicVar.nInternet = DynamicVar.nInternet * nVarValue;
                                    break;
                                case '/':
                                    DynamicVar.nInternet = DynamicVar.nInternet / nVarValue;
                                    break;
                            }
                            break;
                        case TVarType.VString:
                            break;
                    }
                    boFoundVar = true;
                    break;
                }
            }
            if (!boFoundVar)
            {
                ScriptActionError(PlayObject, format(sVarFound, new string[] { sVarName, sType }), QuestActionInfo, M2Share.sSC_CALCVAR);
            }
        }

        private void ActionOfGuildRecall(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            if (PlayObject.m_MyGuild != null)
            {
                // PlayObject.GuildRecall('GuildRecall','');
            }
        }

        private void ActionOfGroupAddList(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string ffile = QuestActionInfo.sParam1;
            if (PlayObject.m_GroupOwner != null)
            {
                for (var I = 0; I < PlayObject.m_GroupMembers.Count; I++)
                {
                    PlayObject = PlayObject.m_GroupMembers[I];
                    // AddListEx(PlayObject.m_sCharName,ffile);
                }
            }
        }

        private void ActionOfClearList(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var ffile = M2Share.g_Config.sEnvirDir + QuestActionInfo.sParam1;
            if (File.Exists(ffile))
            {
                //myFile = new FileInfo(ffile);
                //_W_0 = myFile.CreateText();
                //_W_0.Close();
            }
        }

        private void ActionOfGroupRecall(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            if (PlayObject.m_GroupOwner != null)
            {
                // PlayObject.GroupRecall('GroupRecall');
            }
        }

        // 脚本特修身上所有装备命令
        private void ActionOfRepairAllItem(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sUserItemName;
            bool boIsHasItem = false;
            for (var i = PlayObject.m_UseItems.GetLowerBound(0); i <= PlayObject.m_UseItems.GetUpperBound(0); i++)
            {
                if (PlayObject.m_UseItems[i].wIndex <= 0)
                {
                    continue;
                }
                sUserItemName = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[i].wIndex);
                if (!(i != Grobal2.U_CHARM))
                {
                    PlayObject.SysMsg(sUserItemName + " 禁止修理...", TMsgColor.c_Red, TMsgType.t_Hint);
                    continue;
                }
                PlayObject.m_UseItems[i].Dura = PlayObject.m_UseItems[i].DuraMax;
                PlayObject.SendMsg(this, Grobal2.RM_DURACHANGE, (short)i, PlayObject.m_UseItems[i].Dura, PlayObject.m_UseItems[i].DuraMax, 0, "");
                boIsHasItem = true;
            }
            if (boIsHasItem)
            {
                PlayObject.SysMsg("您身上的的装备修复成功了...", TMsgColor.c_Blue, TMsgType.t_Hint);
            }
        }

        private void ActionOfGroupMoveMap(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TPlayObject PlayObjectEx;
            bool boFlag = false;
            if (PlayObject.m_GroupOwner != null)
            {
                var Envir = M2Share.g_MapManager.FindMap(QuestActionInfo.sParam1);
                if (Envir != null)
                {
                    if (Envir.CanWalk(QuestActionInfo.nParam2, QuestActionInfo.nParam3, true))
                    {
                        for (var i = 0; i < PlayObject.m_GroupMembers.Count; i++)
                        {
                            PlayObjectEx = PlayObject.m_GroupMembers[i];
                            PlayObjectEx.SpaceMove(QuestActionInfo.sParam1, (short)QuestActionInfo.nParam2, (short)QuestActionInfo.nParam3, 0);
                        }
                        boFlag = true;
                    }
                }
            }
            if (!boFlag)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_GROUPMOVEMAP);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            this.m_Castle = M2Share.CastleManager.InCastleWarArea(this);
        }

        private bool ConditionOfCheckNameDateList(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            bool result = false;
            StringList LoadList;
            string sListFileName;
            string sLineText;
            string sHumName = string.Empty;
            string sDate = string.Empty;
            DateTime dOldDate = DateTime.Now;
            int nDay;
            var nDayCount = HUtil32.Str_ToInt(QuestConditionInfo.sParam3, -1);
            var nValNo = M2Share.GetValNameNo(QuestConditionInfo.sParam4);
            var nValNoDay = M2Share.GetValNameNo(QuestConditionInfo.sParam5);
            var boDeleteExprie = QuestConditionInfo.sParam6.ToLower().CompareTo("清理".ToLower()) == 0;
            var boNoCompareHumanName = QuestConditionInfo.sParam6.ToLower().CompareTo("1".ToLower()) == 0;
            var cMethod = QuestConditionInfo.sParam2[0];
            if (nDayCount < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKNAMEDATELIST);
                return result;
            }
            sListFileName = M2Share.g_Config.sEnvirDir + m_sPath + QuestConditionInfo.sParam1;
            if (File.Exists(sListFileName))
            {
                LoadList = new StringList();
                try
                {

                    LoadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    M2Share.MainOutMessage("loading fail.... => " + sListFileName);
                }
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i].Trim();
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sHumName, new string[] { " ", "\t" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sDate, new string[] { " ", "\t" });
                    if ((sHumName.ToLower().CompareTo(PlayObject.m_sCharName.ToLower()) == 0) || boNoCompareHumanName)
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
                                    PlayObject.m_nVal[nValNo] = nDay;
                                    break;
                                // Modify the A .. B: 100 .. 119
                                case 100:
                                    M2Share.g_Config.GlobalVal[nValNo - 100] = nDay;
                                    break;
                                // Modify the A .. B: 200 .. 209
                                case 200:
                                    PlayObject.m_DyVal[nValNo - 200] = nDay;
                                    break;
                                // Modify the A .. B: 300 .. 399
                                case 300:
                                    PlayObject.m_nMval[nValNo - 300] = nDay;
                                    break;
                                // Modify the A .. B: 400 .. 499
                                case 400:
                                    M2Share.g_Config.GlobaDyMval[nValNo - 400] = (short)nDay;
                                    break;
                            }
                        }
                        if (nValNoDay >= 0)
                        {
                            switch (nValNoDay)
                            {
                                // Modify the A .. B: 0 .. 9
                                case 0:
                                    PlayObject.m_nVal[nValNoDay] = nDayCount - nDay;
                                    break;
                                // Modify the A .. B: 100 .. 119
                                case 100:
                                    M2Share.g_Config.GlobalVal[nValNoDay - 100] = nDayCount - nDay;
                                    break;
                                // Modify the A .. B: 200 .. 209
                                case 200:
                                    PlayObject.m_DyVal[nValNoDay - 200] = nDayCount - nDay;
                                    break;
                                // Modify the A .. B: 300 .. 399
                                case 300:
                                    PlayObject.m_nMval[nValNoDay - 300] = nDayCount - nDay;
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
                                    M2Share.MainOutMessage("Save fail.... => " + sListFileName);
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
                M2Share.MainOutMessage("file not found => " + sListFileName);
            }
            return result;
        }

        // CHECKMAPHUMANCOUNT MAP = COUNT
        private bool ConditionOfCheckMapHumanCount(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nCount = HUtil32.Str_ToInt(QuestConditionInfo.sParam3, -1);
            if (nCount < 0)
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKMAPHUMANCOUNT);
                return result;
            }
            var nHumanCount = M2Share.UserEngine.GetMapHuman(QuestConditionInfo.sParam1);
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

        private bool ConditionOfCheckMapMonCount(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            var result = false;
            var nCount = HUtil32.Str_ToInt(QuestConditionInfo.sParam3, -1);
            var Envir = M2Share.g_MapManager.FindMap(QuestConditionInfo.sParam1);
            if ((nCount < 0) || (Envir == null))
            {
                ScriptConditionError(PlayObject, QuestConditionInfo, M2Share.sSC_CHECKMAPMONCOUNT);
                return result;
            }
            var nMonCount = M2Share.UserEngine.GetMapMonster(Envir, null);
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

        private bool ConditionOfCheckIsOnMap(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo)
        {
            if (PlayObject.m_sMapFileName == QuestConditionInfo.sParam1 || PlayObject.m_sMapName == QuestConditionInfo.sParam1)
            {
                return true;
            }
            return false;
        }

        private IList<TDynamicVar> GetDynamicVarList(TPlayObject PlayObject, string sType, ref string sName)
        {
            IList<TDynamicVar> result = null;
            if (HUtil32.CompareLStr(sType, "HUMAN", "HUMAN".Length))
            {
                result = PlayObject.m_DynamicVarList;
                sName = PlayObject.m_sCharName;
            }
            else if (HUtil32.CompareLStr(sType, "GUILD", "GUILD".Length))
            {
                if (PlayObject.m_MyGuild == null)
                {
                    return result;
                }
                result = PlayObject.m_MyGuild.m_DynamicVarList;
                sName = PlayObject.m_MyGuild.sGuildName;
            }
            else if (HUtil32.CompareLStr(sType, "GLOBAL", "GLOBAL".Length))
            {
                result = M2Share.g_DynamicVarList;
                sName = "GLOBAL";
            }
            return result;
        }
    }
}