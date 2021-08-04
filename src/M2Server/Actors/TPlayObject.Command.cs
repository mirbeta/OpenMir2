using SystemModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SystemModule.Common;

namespace M2Server
{
    public partial class TPlayObject
    {
        public void CmdTrainingSkill(TGameCmd Cmd, string sHumanName, string sSkillName, int nLevel)
        {
            TUserMagic UserMagic;
            TPlayObject PlayObject;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sSkillName == "" || nLevel <= 0)
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 人物名称  技能名称 修炼等级(0-3)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nLevel = HUtil32._MIN(3, nLevel);
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {
                SysMsg(format("{0}不在线，或在其它服务器上！！", sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            for (var i = 0; i < PlayObject.m_MagicList.Count; i++)
            {
                UserMagic = PlayObject.m_MagicList[i];
                if (string.Compare(UserMagic.MagicInfo.sMagicName, sSkillName, StringComparison.Ordinal) == 0)
                {
                    UserMagic.btLevel = (byte)nLevel;
                    PlayObject.SendMsg(PlayObject, Grobal2.RM_MAGIC_LVEXP, 0, UserMagic.MagicInfo.wMagicID, UserMagic.btLevel, UserMagic.nTranPoint, "");
                    PlayObject.SysMsg(format("%s的修改炼等级为%d", sSkillName, nLevel), TMsgColor.c_Green, TMsgType.t_Hint);
                    SysMsg(format("%s的技能%s修炼等级为%d", sHumanName, sSkillName, nLevel), TMsgColor.c_Green, TMsgType.t_Hint);
                    break;
                }
            }
        }

        public void CmdAddGameGold(string sCmd, string sHumName, int nPoint)
        {
            TPlayObject PlayObject;
            if (m_btPermission < 6)
            {
                return;
            }
            if (sHumName == "" || nPoint <= 0)
            {
                SysMsg("命令格式: @" + sCmd + " 人物名称  金币数量", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (PlayObject != null)
            {
                if (PlayObject.m_nGameGold + nPoint < 2000000)
                {
                    PlayObject.m_nGameGold += nPoint;
                }
                else
                {
                    nPoint = 2000000 - PlayObject.m_nGameGold;
                    PlayObject.m_nGameGold = 2000000;
                }
                PlayObject.GoldChanged();
                SysMsg(sHumName + "的游戏点已增加" + nPoint + '.', TMsgColor.c_Green, TMsgType.t_Hint);
                PlayObject.SysMsg("游戏点已增加" + nPoint + '.', TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdDelGameGold(string sCmd, string sHumName, int nPoint)
        {
            TPlayObject PlayObject;
            if (m_btPermission < 6)
            {
                return;
            }
            if (sHumName == "" || nPoint <= 0)
            {
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (PlayObject != null)
            {
                if (PlayObject.m_nGameGold > nPoint)
                {
                    PlayObject.m_nGameGold -= nPoint;
                }
                else
                {
                    nPoint = PlayObject.m_nGameGold;
                    PlayObject.m_nGameGold = 0;
                }
                PlayObject.GoldChanged();
                SysMsg(sHumName + "的游戏点已减少" + nPoint + '.', TMsgColor.c_Green, TMsgType.t_Hint);
                PlayObject.SysMsg("游戏点已减少" + nPoint + '.', TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdGameGold(TGameCmd Cmd, string sHumanName, string sCtr, int nGold)
        {
            TPlayObject PlayObject;
            char Ctr;
            Ctr = '1';
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sCtr != "")
            {
                Ctr = sCtr[1];
            }
            if (sHumanName == "" || !new ArrayList(new char[] { '=', '+', '-' }).Contains(Ctr) || nGold < 0 || nGold > 200000000 || sHumanName != "" && sHumanName[1] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandGameGoldHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            switch (sCtr[1])
            {
                case '=':
                    PlayObject.m_nGamePoint = nGold;
                    break;
                case '+':
                    PlayObject.m_nGameGold += nGold;
                    break;
                case '-':
                    PlayObject.m_nGameGold -= nGold;
                    break;
            }
            if (M2Share.g_boGameLogGameGold)
            {
                M2Share.AddGameDataLog(format(M2Share.g_sGameLogMsg1, Grobal2.LOG_GAMEGOLD, PlayObject.m_sMapName, PlayObject.m_nCurrX, PlayObject.m_nCurrY, PlayObject.m_sCharName, M2Share.g_Config.sGameGoldName, nGold, sCtr[1], m_sCharName));
            }
            GameGoldChanged();
            PlayObject.SysMsg(format(M2Share.g_sGameCommandGameGoldHumanMsg, M2Share.g_Config.sGameGoldName, nGold, PlayObject.m_nGameGold, M2Share.g_Config.sGameGoldName), TMsgColor.c_Green, TMsgType.t_Hint);
            SysMsg(format(M2Share.g_sGameCommandGameGoldGMMsg, sHumanName, M2Share.g_Config.sGameGoldName, nGold, PlayObject.m_nGameGold, M2Share.g_Config.sGameGoldName), TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdGamePoint(TGameCmd Cmd, string sHumanName, string sCtr, int nPoint)
        {
            TPlayObject PlayObject;
            char Ctr;
            Ctr = '1';
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sCtr != "")
            {
                Ctr = sCtr[1];
            }
            if (sHumanName == "" || !new ArrayList(new char[] { '=', '+', '-' }).Contains(Ctr) || nPoint < 0 || nPoint > 100000000 || sHumanName != "" && sHumanName[1] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandGamePointHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            switch (sCtr[1])
            {
                case '=':
                    PlayObject.m_nGamePoint = nPoint;
                    break;
                case '+':
                    PlayObject.m_nGamePoint += nPoint;
                    break;
                case '-':
                    PlayObject.m_nGamePoint -= nPoint;
                    break;
            }
            if (M2Share.g_boGameLogGamePoint)
            {
                M2Share.AddGameDataLog(format(M2Share.g_sGameLogMsg1, Grobal2.LOG_GAMEPOINT, PlayObject.m_sMapName, PlayObject.m_nCurrX, PlayObject.m_nCurrY, PlayObject.m_sCharName, M2Share.g_Config.sGamePointName, nPoint, sCtr[1], m_sCharName));
            }
            GameGoldChanged();
            PlayObject.SysMsg(format(M2Share.g_sGameCommandGamePointHumanMsg, nPoint, PlayObject.m_nGamePoint), TMsgColor.c_Green, TMsgType.t_Hint);
            SysMsg(format(M2Share.g_sGameCommandGamePointGMMsg, sHumanName, nPoint, PlayObject.m_nGamePoint), TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdCreditPoint(TGameCmd Cmd, string sHumanName, string sCtr, int nPoint)
        {
            TPlayObject PlayObject;
            char Ctr;
            int nCreditpoint;
            Ctr = '1';
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sCtr != "")
            {
                Ctr = sCtr[1];
            }
            if (sHumanName == "" || !new ArrayList(new char[] { '=', '+', '-' }).Contains(Ctr) || nPoint < 0 || nPoint > byte.MaxValue || sHumanName != "" && sHumanName[1] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandCreditPointHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {
                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            switch (sCtr[1])
            {
                case '=':
                    if (nPoint >= 0 && nPoint <= 255)
                    {
                        PlayObject.m_btCreditPoint = (byte)nPoint;
                    }
                    break;
                case '+':
                    nCreditpoint = PlayObject.m_btCreditPoint + nPoint;
                    if (nPoint >= 0 && nPoint <= 255)
                    {
                        PlayObject.m_btCreditPoint = (byte)nCreditpoint;
                    }
                    break;
                case '-':
                    nCreditpoint = PlayObject.m_btCreditPoint - nPoint;
                    if (nPoint >= 0 && nPoint <= 255)
                    {
                        PlayObject.m_btCreditPoint = (byte)nCreditpoint;
                    }
                    break;
            }
            PlayObject.SysMsg(format(M2Share.g_sGameCommandCreditPointHumanMsg, nPoint, PlayObject.m_btCreditPoint), TMsgColor.c_Green, TMsgType.t_Hint);
            SysMsg(format(M2Share.g_sGameCommandCreditPointGMMsg, sHumanName, nPoint, PlayObject.m_btCreditPoint), TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdAddGold(TGameCmd Cmd, string sHumName, int nCount)
        {
            TPlayObject PlayObject;
            var nServerIndex = 0;
            if (m_btPermission < 6)
            {
                return;
            }
            if (sHumName == "" || nCount <= 0)
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 人物名称  金币数量", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (PlayObject != null)
            {
                if (PlayObject.m_nGold + nCount < PlayObject.m_nGoldMax)
                {
                    PlayObject.m_nGold += nCount;
                }
                else
                {
                    nCount = PlayObject.m_nGoldMax - PlayObject.m_nGold;
                    PlayObject.m_nGold = PlayObject.m_nGoldMax;
                }
                PlayObject.GoldChanged();
                SysMsg(sHumName + "的金币已增加" + nCount + '.', TMsgColor.c_Green, TMsgType.t_Hint);
                if (M2Share.g_boGameLogGold)
                {
                    M2Share.AddGameDataLog("14" + "\t" + m_sMapName + "\t" + m_nCurrX + "\t" + m_nCurrY + "\t" + m_sCharName + "\t" + Grobal2.sSTRING_GOLDNAME + "\t" + nCount + "\t" + '1' + "\t" + sHumName);
                }
            }
            else
            {
                if (M2Share.UserEngine.FindOtherServerUser(sHumName, ref nServerIndex))
                {
                    SysMsg(sHumName + " 现在" + nServerIndex + "号服务器上", TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    M2Share.FrontEngine.AddChangeGoldList(m_sCharName, sHumName, nCount);
                    SysMsg(sHumName + " 现在不在线，等其上线时金币将自动增加", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
        }

        public void CmdAddGuild(TGameCmd Cmd, string sGuildName, string sGuildChief)
        {
            TPlayObject Human;
            bool boAddState;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (M2Share.nServerIndex != 0)
            {
                SysMsg("这个命令只能使用在主服务器上", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sGuildName == "" || sGuildChief == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 行会名称 掌门人名称", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            boAddState = false;
            Human = M2Share.UserEngine.GetPlayObject(sGuildChief);
            if (Human == null)
            {
                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sGuildChief), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (M2Share.GuildManager.MemberOfGuild(sGuildChief) == null)
            {
                if (M2Share.GuildManager.AddGuild(sGuildName, sGuildChief))
                {
                    M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_205, M2Share.nServerIndex, sGuildName + '/' + sGuildChief);
                    SysMsg("行会名称: " + sGuildName + " 掌门人: " + sGuildChief, TMsgColor.c_Green, TMsgType.t_Hint);
                    boAddState = true;
                }
            }
            if (boAddState)
            {
                Human.m_MyGuild = M2Share.GuildManager.MemberOfGuild(Human.m_sCharName);
                if (Human.m_MyGuild != null)
                {
                    Human.m_sGuildRankName = Human.m_MyGuild.GetRankName(this, ref Human.m_nGuildRankNo);
                    Human.RefShowName();
                }
            }
        }

        public void CmdAdjuestExp(TGameCmd Cmd, string sHumanName, string sExp)
        {
            TPlayObject PlayObject;
            int dwExp;
            int dwOExp;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 人物名称 经验值", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            dwExp = HUtil32.Str_ToInt(sExp, 0);
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject != null)
            {
                dwOExp = PlayObject.m_Abil.Exp;
                PlayObject.m_Abil.Exp = dwExp;
                PlayObject.HasLevelUp(1);
                SysMsg(sHumanName + " 经验调整完成。", TMsgColor.c_Green, TMsgType.t_Hint);
                if (M2Share.g_Config.boShowMakeItemMsg)
                {
                    M2Share.MainOutMessage("[经验调整] " + m_sCharName + '(' + PlayObject.m_sCharName + ' ' + dwOExp + " -> " + PlayObject.m_Abil.Exp + ')');
                }
            }
            else
            {
                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdAdjuestLevel(TGameCmd Cmd, string sHumanName, int nLevel)
        {
            TPlayObject PlayObject;
            int nOLevel;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 人物名称 等级", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject != null)
            {
                nOLevel = PlayObject.m_Abil.Level;
                PlayObject.m_Abil.Level = (ushort)HUtil32._MAX(1, HUtil32._MIN(M2Share.MAXUPLEVEL, nLevel));
                PlayObject.HasLevelUp(1);
                SysMsg(sHumanName + " 等级调整完成。", TMsgColor.c_Green, TMsgType.t_Hint);
                if (M2Share.g_Config.boShowMakeItemMsg)
                {
                    M2Share.MainOutMessage("[等级调整] " + m_sCharName + '(' + PlayObject.m_sCharName + ' ' + nOLevel + " -> " + PlayObject.m_Abil.Level + ')');
                }
            }
            else
            {
                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdAdjustExp(TPlayObject Human, int nExp)
        {
            if (m_btPermission < 6)
            {
                return;
            }
        }

        public void CmdBackStep(string sCmd, int nType, int nCount)
        {
            if (m_btPermission < 6)
            {
                return;
            }
            nType = HUtil32._MIN(nType, 8);
            if (nType == 0)
            {
                CharPushed(GetBackDir(m_btDirection), nCount);
            }
            else
            {
                CharPushed((byte)M2Share.RandomNumber.Random(nType), nCount);
            }
        }

        public void CmdBonuPoint(TGameCmd Cmd, string sHumName, int nCount)
        {
            TPlayObject PlayObject;
            string sMsg;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumName == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 人物名称 属性点数(不输入为查看点数)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (PlayObject == null)
            {
                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (nCount > 0)
            {
                PlayObject.m_nBonusPoint = nCount;
                PlayObject.SendMsg(this, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                return;
            }
            sMsg = format("未分配点数:%d 已分配点数:(DC:%d MC:%d SC:%d AC:%d MAC:%d HP:%d MP:%d HIT:%d SPEED:%d)", PlayObject.m_nBonusPoint, PlayObject.m_BonusAbil.DC, PlayObject.m_BonusAbil.MC, PlayObject.m_BonusAbil.SC, PlayObject.m_BonusAbil.AC, PlayObject.m_BonusAbil.MAC, PlayObject.m_BonusAbil.HP, PlayObject.m_BonusAbil.MP, PlayObject.m_BonusAbil.Hit, PlayObject.m_BonusAbil.Speed);
            SysMsg(format("%s的属性点数为:%s", sHumName, sMsg), TMsgColor.c_Red, TMsgType.t_Hint);
        }

        public void CmdChangeAdminMode(string sCmd, int nPermission, string sParam1, bool boFlag)
        {
            if (m_btPermission < nPermission)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam1 != "" && sParam1[1] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, sCmd, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            m_boAdminMode = boFlag;
            if (m_boAdminMode)
            {
                SysMsg(M2Share.sGameMasterMode, TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg(M2Share.sReleaseGameMasterMode, TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdChangeAttackMode(int nMode, string sParam1, string sParam2, string sParam3, string sParam4, string sParam5, string sParam6, string sParam7)
        {
            if (nMode >= M2Share.HAM_ALL && nMode <= M2Share.HAM_PKATTACK)
            {
                m_btAttatckMode = (byte)nMode;
            }
            else
            {
                if (m_btAttatckMode < M2Share.HAM_PKATTACK)
                {
                    m_btAttatckMode++;
                }
                else
                {
                    m_btAttatckMode = M2Share.HAM_ALL;
                }
            }
            if (nMode >= 0 && nMode <= 4)
            {
                m_btAttatckMode = (byte)nMode;
            }
            else
            {
                if (m_btAttatckMode < M2Share.HAM_PKATTACK)
                {
                    m_btAttatckMode++;
                }
                else
                {
                    m_btAttatckMode = M2Share.HAM_ALL;
                }
            }
            switch (m_btAttatckMode)
            {
                case M2Share.HAM_ALL:// [攻击模式: 全体攻击]
                    SysMsg(M2Share.sAttackModeOfAll, TMsgColor.c_Green, TMsgType.t_Hint);
                    break;
                case M2Share.HAM_PEACE: // [攻击模式: 和平攻击]
                    SysMsg(M2Share.sAttackModeOfPeaceful, TMsgColor.c_Green, TMsgType.t_Hint);
                    break;
                case M2Share.HAM_DEAR:// [攻击模式: 和平攻击]
                    SysMsg(M2Share.sAttackModeOfDear, TMsgColor.c_Green, TMsgType.t_Hint);
                    break;
                case M2Share.HAM_MASTER:// [攻击模式: 和平攻击]
                    SysMsg(M2Share.sAttackModeOfMaster, TMsgColor.c_Green, TMsgType.t_Hint);
                    break;
                case M2Share.HAM_GROUP:// [攻击模式: 编组攻击]
                    SysMsg(M2Share.sAttackModeOfGroup, TMsgColor.c_Green, TMsgType.t_Hint);
                    break;
                case M2Share.HAM_GUILD:// [攻击模式: 行会攻击]
                    SysMsg(M2Share.sAttackModeOfGuild, TMsgColor.c_Green, TMsgType.t_Hint);
                    break;
                case M2Share.HAM_PKATTACK:// [攻击模式: 红名攻击]
                    SysMsg(M2Share.sAttackModeOfRedWhite, TMsgColor.c_Green, TMsgType.t_Hint);
                    break;
            }
            SendDefMessage(Grobal2.SM_ATTACKMODE, m_btAttatckMode, 0, 0, 0, "");
        }

        public void CmdChangeDearName(TGameCmd Cmd, string sHumanName, string sDearName)
        {
            TPlayObject PlayObject;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sDearName == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 人物名称 配偶名称(如果为 无 则清除)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject != null)
            {
                if (string.Compare(sDearName.ToLower(), "无", StringComparison.Ordinal) == 0)
                {
                    PlayObject.m_sDearName = "";
                    PlayObject.RefShowName();
                    this.SysMsg(sHumanName + " 的配偶名清除成功。", TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    PlayObject.m_sDearName = sDearName;
                    PlayObject.RefShowName();
                    this.SysMsg(sHumanName + " 的配偶名更改成功。", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            else
            {
                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdChangeGender(TGameCmd Cmd, string sHumanName, string sSex)
        {
            TPlayObject PlayObject;
            int nSex;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nSex = -1;
            if (sSex == "Man" || sSex == "Male" || sSex == "0")
            {
                nSex = 0;
            }
            if (sSex == "Woman" || sSex == "Female" || sSex == "1")
            {
                nSex = 1;
            }
            if (sHumanName == "" || nSex == -1)
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 人物名称 性别(男、女)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject != null)
            {
                if (PlayObject.m_btGender != nSex)
                {
                    PlayObject.m_btGender = (byte)nSex;
                    PlayObject.FeatureChanged();
                    SysMsg(PlayObject.m_sCharName + " 的性别已改变。", TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    SysMsg(PlayObject.m_sCharName + " 的性别未改变！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
            else
            {
                SysMsg(sHumanName + "没有在线！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdChangeItemName(string sCmd, string sMakeIndex, string sItemIndex, string sItemName)
        {
            int nMakeIndex;
            int nItemIndex;
            if (m_btPermission < 6)
            {
                return;
            }
            if (sMakeIndex == "" || sItemIndex == "" || sItemName == "")
            {
                SysMsg("命令格式: @" + sCmd + " 物品编号 物品ID号 物品名称", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nMakeIndex = HUtil32.Str_ToInt(sMakeIndex, -1);
            nItemIndex = HUtil32.Str_ToInt(sItemIndex, -1);
            if (nMakeIndex <= 0 || nItemIndex < 0)
            {
                SysMsg("命令格式: @" + sCmd + " 物品编号 物品ID号 物品名称", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (M2Share.ItemUnit.AddCustomItemName(nMakeIndex, nItemIndex, sItemName))
            {
                M2Share.ItemUnit.SaveCustomItemName();
                SysMsg("物品名称设置成功。", TMsgColor.c_Green, TMsgType.t_Hint);
                return;
            }
            SysMsg("此物品，已经设置了其它的名称！！！", TMsgColor.c_Red, TMsgType.t_Hint);
        }

        public void CmdChangeJob(TGameCmd Cmd, string sHumanName, string sJobName)
        {
            TPlayObject PlayObject;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sJobName == "")
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandChangeJobHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject != null)
            {
                if (string.Compare(sJobName, M2Share.sWarrior, StringComparison.Ordinal) == 0)
                {
                    PlayObject.m_btJob = M2Share.jWarr;
                }
                if (string.Compare(sJobName, M2Share.sWizard, StringComparison.Ordinal) == 0)
                {
                    PlayObject.m_btJob = M2Share.jWizard;
                }
                if (string.Compare(sJobName, M2Share.sTaos, StringComparison.Ordinal) == 0)
                {
                    PlayObject.m_btJob = M2Share.jTaos;
                }
                PlayObject.HasLevelUp(1);
                PlayObject.SysMsg(M2Share.g_sGameCommandChangeJobHumanMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                SysMsg(format(M2Share.g_sGameCommandChangeJobMsg, sHumanName), TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdChangeLevel(TGameCmd Cmd, string sParam1)
        {
            int nOLevel;
            int nLevel;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam1 != "" && sParam1[1] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nLevel = HUtil32.Str_ToInt(sParam1, 1);
            nOLevel = m_Abil.Level;
            m_Abil.Level = (ushort)HUtil32._MIN(M2Share.MAXUPLEVEL, nLevel);
            HasLevelUp(1);
            if (M2Share.g_Config.boShowMakeItemMsg)
            {
                M2Share.MainOutMessage(format(M2Share.g_sGameCommandLevelConsoleMsg, m_sCharName, nOLevel, m_Abil.Level));
            }
        }

        public void CmdChangeMasterName(TGameCmd Cmd, string sHumanName, string sMasterName, string sIsMaster)
        {
            TPlayObject PlayObject;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sMasterName == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 人物名称 师徒名称(如果为 无 则清除)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject != null)
            {
               if (string.Compare(sMasterName, "无", StringComparison.Ordinal) == 0)
               {
                   PlayObject.m_sMasterName = "";
                   PlayObject.RefShowName();
                   PlayObject.m_boMaster = false;
                   this.SysMsg(sHumanName + " 的师徒名清除成功。", TMsgColor.c_Green, TMsgType.t_Hint);
               }
               else
               {
                   PlayObject.m_sMasterName = sMasterName;
                   if ((sIsMaster != "") && (sIsMaster[1] == '1'))
                   {
                       PlayObject.m_boMaster = true;
                   }
                   else
                   {
                       PlayObject.m_boMaster = false;
                   }
                   PlayObject.RefShowName();
                   this.SysMsg(sHumanName + " 的师徒名更改成功。", TMsgColor.c_Green, TMsgType.t_Hint);
               }
            }
            else
            {
                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdChangeObMode(string sCmd, int nPermission, string sParam1, bool boFlag)
        {
            if (m_btPermission < nPermission)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam1 != "" && sParam1[1] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, sCmd, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (boFlag)
            {
                SendRefMsg(Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, ""); // 01/21 强行发送刷新数据到客户端，解决GM登录隐身有影子问题
            }
            m_boObMode = boFlag;
            if (m_boObMode)
            {
                SysMsg(M2Share.sObserverMode, TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg(M2Share.g_sReleaseObserverMode, TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdChangeSabukLord(TGameCmd Cmd, string sCastleName, string sGuildName, bool boFlag)
        {
            TGuild Guild;
            TUserCastle Castle;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sCastleName == "" || sGuildName == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 城堡名称 行会名称", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Castle = M2Share.CastleManager.Find(sCastleName);
            if (Castle == null)
            {
                SysMsg(format(M2Share.g_sGameCommandSbkGoldCastleNotFoundMsg, sCastleName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Guild = M2Share.GuildManager.FindGuild(sGuildName);
            if (Guild != null)
            {
                M2Share.AddGameDataLog("27" + "\t" + Castle.m_sOwnGuild + "\t" + '0' + "\t" + '1' + "\t" + "sGuildName" + "\t" + m_sCharName + "\t" + '0' + "\t" + '1' + "\t" + '0');
                Castle.GetCastle(Guild);
                if (boFlag)
                {
                    M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_211, M2Share.nServerIndex, sGuildName);
                }
                SysMsg(Castle.m_sName + " 所属行会已经更改为 " + sGuildName, TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg("行会 " + sGuildName + "还没建立！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdChangeSalveStatus()
        {
            if (m_SlaveList.Count > 0)
            {
                m_boSlaveRelax = !m_boSlaveRelax;
                if (m_boSlaveRelax)
                {
                    SysMsg(M2Share.sPetRest, TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    SysMsg(M2Share.sPetAttack, TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
        }

        public void CmdChangeSuperManMode(string sCmd, int nPermission, string sParam1, bool boFlag)
        {
            if (m_btPermission < nPermission)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam1 != "" && sParam1[1] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, sCmd, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            m_boSuperMan = boFlag;
            if (m_boSuperMan)
            {
                SysMsg(M2Share.sSupermanMode, TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg(M2Share.sReleaseSupermanMode, TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdChangeUserFull(string sCmd, string sUserCount)
        {
            int nCount;
            if (m_btPermission < 6)
            {
                return;
            }
            nCount = HUtil32.Str_ToInt(sUserCount, -1);
            if (sUserCount == "" || nCount < 1 || sUserCount != "" && sUserCount[1] == '?')
            {
                SysMsg("设置服务器最高上线人数。", TMsgColor.c_Red, TMsgType.t_Hint);
                SysMsg("命令格式: @" + sCmd + " 人数", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            M2Share.g_Config.nUserFull = nCount;
            SysMsg(format("服务器上线人数限制: %d", nCount), TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdChangeZenFastStep(string sCmd, string sFastStep)
        {
            int nFastStep;
            if (m_btPermission < 6)
            {
                return;
            }
            nFastStep = HUtil32.Str_ToInt(sFastStep, -1);
            if (sFastStep == "" || nFastStep < 1 || sFastStep != "" && sFastStep[1] == '?')
            {
                SysMsg("设置怪物行动速度。", TMsgColor.c_Red, TMsgType.t_Hint);
                SysMsg("命令格式: @" + sCmd + " 速度", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            M2Share.g_Config.nZenFastStep = nFastStep;
            SysMsg(format("怪物行动速度: %d", nFastStep), TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdClearBagItem(TGameCmd Cmd, string sHumanName)
        {
            TPlayObject PlayObject;
            TUserItem UserItem;
            ArrayList DelList;
            DelList = null;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, "人物名称"), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {
                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            for (var i = 0; i < PlayObject.m_ItemList.Count; i++)
            {
                UserItem = PlayObject.m_ItemList[i];
                if (DelList == null)
                {
                    DelList = new ArrayList();
                }
                DelList.Add(UserItem.MakeIndex);
                Dispose(UserItem);
            }
            PlayObject.m_ItemList.Clear();
            if (DelList != null)
            {
                var ObjectId = HUtil32.Sequence();
                M2Share.ObjectSystem.AddOhter(ObjectId, DelList);
                PlayObject.SendMsg(PlayObject, Grobal2.RM_SENDDELITEMLIST, 0, ObjectId, 0, 0, "");
            }
        }

        public void CmdClearHumanPassword(string sCmd, int nPermission, string sHumanName)
        {
            TPlayObject PlayObject;
            if (m_btPermission < nPermission)
            {
                return;
            }
            if (sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {
                SysMsg("清除玩家的仓库密码！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                SysMsg(format("命令格式: @%s 人物名称", sCmd), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {
                return;
            }
            PlayObject.m_boPasswordLocked = false;
            PlayObject.m_boUnLockStoragePwd = false;
            PlayObject.m_sStoragePwd = "";
            PlayObject.SysMsg("你的保护密码已被清除！！！", TMsgColor.c_Green, TMsgType.t_Hint);
            SysMsg(format("%s的保护密码已被清除！！！", sHumanName), TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdClearMapMonster(TGameCmd Cmd, string sMapName, string sMonName, string sItems)
        {
            IList<TBaseObject> MonList;
            TEnvirnoment Envir;
            int nMonCount;
            bool boKillAll;
            bool boKillAllMap;
            bool boNotItem;
            TBaseObject BaseObject;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sMapName == "" || sMonName == "" || sItems == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 地图号(* 为所有) 怪物名称(* 为所有) 掉物品(0,1)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            boKillAll = false;
            boKillAllMap = false;
            boNotItem = true;
            nMonCount = 0;
            Envir = null;
            if (sMonName == "*")
            {
                boKillAll = true;
            }
            if (sMapName == "*")
            {
                boKillAllMap = true;
            }
            if (sItems == "1")
            {
                boNotItem = false;
            }
            MonList = new List<TBaseObject>();
            for (var i = 0; i < M2Share.g_MapManager.Maps.Count; i++)
            {
                Envir = M2Share.g_MapManager.Maps[i];
                if (Envir != null && (boKillAllMap || string.Compare(Envir.sMapName.ToLower(), sMapName, StringComparison.Ordinal) == 0))
                {
                    M2Share.UserEngine.GetMapMonster(Envir, MonList);
                    for (var j = 0; j < MonList.Count; j++)
                    {
                        BaseObject = MonList[j] as TBaseObject;
                        if (boKillAll || string.Compare(sMonName.ToLower(), BaseObject.m_sCharName, StringComparison.Ordinal) == 0)
                        {
                            BaseObject.m_boNoItem = boNotItem;
                            BaseObject.m_WAbil.HP = 0;
                            nMonCount++;
                        }
                    }
                }
            }
            if (Envir == null)
            {
                SysMsg("输入的地图不存在！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            SysMsg("已清除怪物数: " + nMonCount, TMsgColor.c_Red, TMsgType.t_Hint);
        }

        public void CmdClearMission(TGameCmd Cmd, string sHumanName)
        {
            TPlayObject PlayObject;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 人物名称)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName[1] == '?')
            {
                SysMsg("此命令用于清除人物的任务标志。", TMsgColor.c_Blue, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {
                SysMsg(format("%s不在线，或在其它服务器上！！", sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            //FillChar(PlayObject.m_QuestFlag, sizeof(grobal2.byte), '\0');
            SysMsg(format("%s的任务标志已经全部清零。", sHumanName), TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdContestPoint(TGameCmd Cmd, string sGuildName)
        {
            TGuild Guild;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sGuildName == "" || sGuildName != "" && sGuildName[1] == '?')
            {
                SysMsg("查看行会战的得分数。", TMsgColor.c_Red, TMsgType.t_Hint);

                SysMsg(format("命令格式: @%s 行会名称", Cmd.sCmd), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Guild = M2Share.GuildManager.FindGuild(sGuildName);
            if (Guild != null)
            {
                SysMsg(format("%s 的得分为: %d", sGuildName, Guild.nContestPoint), TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg(format("行会: %s 不存在！！！", sGuildName), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdStartContest(TGameCmd Cmd, string sParam1)
        {
            IList<TBaseObject> List10;
            ArrayList List14;
            TPlayObject PlayObject;
            TPlayObject PlayObjectA;
            bool bo19;
            string s20;
            TGuild Guild;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam1 != "" && sParam1[1] == '?')
            {
                SysMsg("开始行会争霸赛。", TMsgColor.c_Red, TMsgType.t_Hint);
                SysMsg(format("命令格式: @%s", Cmd.sCmd), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (!m_PEnvir.Flag.boFight3Zone)
            {
                SysMsg("此命令不能在当前地图中使用！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            List10 = new List<TBaseObject>();
            List14 = new ArrayList();
            M2Share.UserEngine.GetMapRageHuman(m_PEnvir, m_nCurrX, m_nCurrY, 1000, List10);
            for (var i = 0; i < List10.Count; i++)
            {
                PlayObject = List10[i] as TPlayObject;
                if (!PlayObject.m_boObMode || !PlayObject.m_boAdminMode)
                {
                    PlayObject.m_nFightZoneDieCount = 0;
                    if (PlayObject.m_MyGuild == null)
                    {
                        continue;
                    }
                    bo19 = false;
                    for (var j = 0; j < List14.Count; j++)
                    {
                        PlayObjectA = List14[j] as TPlayObject;
                        if (PlayObject.m_MyGuild == PlayObjectA.m_MyGuild)
                        {
                            bo19 = true;
                        }
                    }
                    if (!bo19)
                    {
                        List14.Add(PlayObject.m_MyGuild);
                    }
                }
            }
            SysMsg("行会争霸赛已经开始。", TMsgColor.c_Green, TMsgType.t_Hint);
            M2Share.UserEngine.CryCry(Grobal2.RM_CRY, m_PEnvir, m_nCurrX, m_nCurrY, 1000, M2Share.g_Config.btCryMsgFColor, M2Share.g_Config.btCryMsgBColor, "- 行会战争已爆发。");
            s20 = "";
            for (var i = 0; i < List14.Count; i++)
            {
                Guild = (TGuild)List14[i];
                Guild.StartTeamFight();
                for (var j = 0; j < List10.Count; j++)
                {
                    PlayObject = List10[i] as TPlayObject;
                    if (PlayObject.m_MyGuild == Guild)
                    {
                        Guild.AddTeamFightMember(PlayObject.m_sCharName);
                    }
                }
                s20 = s20 + Guild.sGuildName + ' ';
            }
            M2Share.UserEngine.CryCry(Grobal2.RM_CRY, m_PEnvir, m_nCurrX, m_nCurrY, 1000, M2Share.g_Config.btCryMsgFColor, M2Share.g_Config.btCryMsgBColor, " -参加的门派:" + s20);
            //List10.Free;
            //List14.Free;
        }

        public void CmdEndContest(TGameCmd Cmd, string sParam1)
        {
            IList<TBaseObject> List10;
            ArrayList List14;
            TPlayObject PlayObject;
            TPlayObject PlayObjectA;
            bool bo19;
            TGuild Guild;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam1 != "" && sParam1[1] == '?')
            {
                SysMsg("结束行会争霸赛。", TMsgColor.c_Red, TMsgType.t_Hint);

                SysMsg(format("命令格式: @%s", Cmd.sCmd), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (!m_PEnvir.Flag.boFight3Zone)
            {
                SysMsg("此命令不能在当前地图中使用！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            List10 = new List<TBaseObject>();
            List14 = new ArrayList();
            M2Share.UserEngine.GetMapRageHuman(m_PEnvir, m_nCurrX, m_nCurrY, 1000, List10);
            for (var i = 0; i < List10.Count; i++)
            {
                PlayObject = List10[i] as TPlayObject;
                if (!PlayObject.m_boObMode || !PlayObject.m_boAdminMode)
                {
                    if (PlayObject.m_MyGuild == null)
                    {
                        continue;
                    }
                    bo19 = false;
                    for (var j = 0; j < List14.Count; j++)
                    {
                        PlayObjectA = List14[j] as TPlayObject;
                        if (PlayObject.m_MyGuild == PlayObjectA.m_MyGuild)
                        {
                            bo19 = true;
                        }
                    }
                    if (!bo19)
                    {
                        List14.Add(PlayObject.m_MyGuild);
                    }
                }
            }
            for (var i = 0; i < List14.Count; i++)
            {
                Guild = (TGuild)List14[i];
                Guild.EndTeamFight();
                M2Share.UserEngine.CryCry(Grobal2.RM_CRY, m_PEnvir, m_nCurrX, m_nCurrY, 1000, M2Share.g_Config.btCryMsgFColor, M2Share.g_Config.btCryMsgBColor, format(" - %s 行会争霸赛已结束。", Guild.sGuildName));
            }
            //List10.Free;
            //List14.Free;
        }

        public void CmdAllowGroupReCall(string sCmd, string sParam)
        {
            if (sParam != "" && sParam[1] == '?')
            {
                SysMsg("此命令用于允许或禁止编组传送功能。", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            m_boAllowGroupReCall = !m_boAllowGroupReCall;
            if (m_boAllowGroupReCall)
            {
                // '[允许天地合一]'
                SysMsg(M2Share.g_sEnableGroupRecall, TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                // '[禁止天地合一]'
                SysMsg(M2Share.g_sDisableGroupRecall, TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdAnnouncement(TGameCmd Cmd, string sGuildName)
        {
            TGuild Guild;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sGuildName == "" || sGuildName != "" && sGuildName[1] == '?')
            {
                SysMsg("查看行会争霸赛结果。", TMsgColor.c_Red, TMsgType.t_Hint);

                SysMsg(format("命令格式: @%s 行会名称", Cmd.sCmd), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (!m_PEnvir.Flag.boFight3Zone)
            {
                SysMsg("此命令不能在当前地图中使用！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Guild = M2Share.GuildManager.FindGuild(sGuildName);
            //if (Guild != null)
            //{
            //    M2Share.UserEngine.CryCry(grobal2.RM_CRY, this.m_PEnvir, this.m_nCurrX, this.m_nCurrY, 1000, M2Share.g_Config.btCryMsgFColor, M2Share.g_Config.btCryMsgBColor, format(" - %s 行会争霸赛结果: ", new string[] { Guild.sGuildName }));
            //    for (I = 0; I < Guild.TeamFightDeadList.Count; I++)
            //    {
            //        nPoint = ((int)Guild.TeamFightDeadList.Values[I]);
            //        sHumanName = Guild.TeamFightDeadList[I];
            //        M2Share.UserEngine.CryCry(grobal2.RM_CRY, this.m_PEnvir, this.m_nCurrX, this.m_nCurrY, 1000, M2Share.g_Config.btCryMsgFColor, M2Share.g_Config.btCryMsgBColor, format(" - %s  : %d 分/死亡%d次。 ", new object[] { sHumanName, HUtil32.HiWord(nPoint), HUtil32.LoWord(nPoint) }));
            //    }
            //}
            M2Share.UserEngine.CryCry(Grobal2.RM_CRY, m_PEnvir, m_nCurrX, m_nCurrY, 1000, M2Share.g_Config.btCryMsgFColor, M2Share.g_Config.btCryMsgBColor, format(" - [%s] : %d 分。", Guild.sGuildName, Guild.nContestPoint));
            M2Share.UserEngine.CryCry(Grobal2.RM_CRY, m_PEnvir, m_nCurrX, m_nCurrY, 1000, M2Share.g_Config.btCryMsgFColor, M2Share.g_Config.btCryMsgBColor, "------------------------------------");
        }

        public void CmdDearRecall(string sCmd, string sParam)
        {
            if (sParam != "" && sParam[1] == '?')
            {
                SysMsg("命令格式: @" + sCmd + " (夫妻传送，将对方传送到自己身边，对方必须允许传送。)", TMsgColor.c_Green, TMsgType.t_Hint);
                return;
            }
            if (m_sDearName == "")
            {
                SysMsg("你没有结婚！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (m_PEnvir.Flag.boNODEARRECALL)
            {
                SysMsg("本地图禁止夫妻传送！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (m_DearHuman == null)
            {
                if (m_btGender == ObjBase.gMan)
                {
                    SysMsg("你的老婆不在线！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                }
                else
                {
                    SysMsg("你的老公不在线！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                }
                return;
            }

            if (HUtil32.GetTickCount() - m_dwDearRecallTick < 10000)
            {
                SysMsg("稍等伙才能再次使用此功能！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }

            m_dwDearRecallTick = HUtil32.GetTickCount();
            if (m_DearHuman.m_boCanDearRecall)
            {
                RecallHuman(m_DearHuman.m_sCharName);
            }
            else
            {
                SysMsg(m_DearHuman.m_sCharName + " 不允许传送！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
        }

        public void CmdMasterRecall(string sCmd, string sParam)
        {
            int I;
            TPlayObject MasterHuman;
            if (sParam != "" && sParam[1] == '?')
            {
                SysMsg("命令格式: @" + sCmd + " (师徒传送，师父可以将徒弟传送到自己身边，徒弟必须允许传送。)", TMsgColor.c_Green, TMsgType.t_Hint);
                return;
            }
            if (!m_boMaster)
            {
                SysMsg("只能师父才能使用此功能！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (m_MasterList.Count == 0)
            {
                SysMsg("你的徒弟一个都不在线！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (m_PEnvir.Flag.boNOMASTERRECALL)
            {
                SysMsg("本地图禁止师徒传送！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }

            if (HUtil32.GetTickCount() - m_dwMasterRecallTick < 10000)
            {
                SysMsg("稍等伙才能再次使用此功能！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            for (I = 0; I < m_MasterList.Count; I++)
            {
                MasterHuman = m_MasterList[I];
                if (MasterHuman.m_boCanMasterRecall)
                {
                    RecallHuman(MasterHuman.m_sCharName);
                }
                else
                {
                    SysMsg(MasterHuman.m_sCharName + " 不允许传送！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
        }

        public void CmdDelBonuPoint(TGameCmd Cmd, string sHumName)
        {
            TPlayObject PlayObject;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumName == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 人物名称", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (PlayObject != null)
            {
                //FillChar(PlayObject.m_BonusAbil, '\0');
                PlayObject.m_nBonusPoint = 0;
                PlayObject.SendMsg(PlayObject, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                PlayObject.HasLevelUp(0);
                PlayObject.SysMsg("分配点数已清除！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                SysMsg(sHumName + " 的分配点数已清除.", TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdReNewLevel(TGameCmd Cmd, string sHumanName, string sLevel)
        {
            TPlayObject PlayObject;
            int nLevel;
            if (m_btPermission < 6)
            {
                return;
            }
            if (sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 人物名称 点数(为空则查看)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nLevel = HUtil32.Str_ToInt(sLevel, -1);
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject != null)
            {
                if (nLevel >= 0 && nLevel <= 255)
                {
                    PlayObject.m_btReLevel = (byte)nLevel;
                    PlayObject.RefShowName();
                }
                SysMsg(sHumanName + " 的转生等级为 " + PlayObject.m_btReLevel, TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg(sHumanName + " 没在线上！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdRestBonuPoint(TGameCmd Cmd, string sHumName)
        {
            TPlayObject PlayObject;
            int nTotleUsePoint;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumName == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 人物名称", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (PlayObject != null)
            {
                nTotleUsePoint = PlayObject.m_BonusAbil.DC + PlayObject.m_BonusAbil.MC + PlayObject.m_BonusAbil.SC + PlayObject.m_BonusAbil.AC + PlayObject.m_BonusAbil.MAC + PlayObject.m_BonusAbil.HP + PlayObject.m_BonusAbil.MP + PlayObject.m_BonusAbil.Hit + PlayObject.m_BonusAbil.Speed + PlayObject.m_BonusAbil.X2;
                //FillChar(PlayObject.m_BonusAbil, '\0');
                PlayObject.m_nBonusPoint += nTotleUsePoint;
                PlayObject.SendMsg(PlayObject, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                PlayObject.HasLevelUp(0);
                PlayObject.SysMsg("分配点数已复位！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                SysMsg(sHumName + " 的分配点数已复位.", TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdSbkDoorControl(string sCmd, string sParam)
        {
        }

        public void CmdSearchDear(string sCmd, string sParam)
        {
            if (sParam != "" && sParam[1] == '?')
            {
                SysMsg("此命令用于查询配偶当前所在位置。", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (m_sDearName == "")
            {
                // '你都没结婚查什么？'
                SysMsg(M2Share.g_sYouAreNotMarryedMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (m_DearHuman == null)
            {
                if (m_btGender == ObjBase.gMan)
                {
                    // '你的老婆还没有上线！！！'
                    SysMsg(M2Share.g_sYourWifeNotOnlineMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                }
                else
                {
                    // '你的老公还没有上线！！！'
                    SysMsg(M2Share.g_sYourHusbandNotOnlineMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                }
                return;
            }
            if (m_btGender == ObjBase.gMan)
            {
                // '你的老婆现在位于:'
                SysMsg(M2Share.g_sYourWifeNowLocateMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                SysMsg(m_DearHuman.m_sCharName + ' ' + m_DearHuman.m_PEnvir.sMapDesc + '(' + m_DearHuman.m_nCurrX + ':' + m_DearHuman.m_nCurrY + ')', TMsgColor.c_Green, TMsgType.t_Hint);
                // '你的老公正在找你，他现在位于:'
                m_DearHuman.SysMsg(M2Share.g_sYourHusbandSearchLocateMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                m_DearHuman.SysMsg(m_sCharName + ' ' + m_PEnvir.sMapDesc + '(' + m_nCurrX + ':' + m_nCurrY + ')', TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                // '你的老公现在位于:'
                SysMsg(M2Share.g_sYourHusbandNowLocateMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                SysMsg(m_DearHuman.m_sCharName + ' ' + m_DearHuman.m_PEnvir.sMapDesc + '(' + m_DearHuman.m_nCurrX + ':' + m_DearHuman.m_nCurrY + ')', TMsgColor.c_Green, TMsgType.t_Hint);
                // '你的老婆正在找你，她现在位于:'
                m_DearHuman.SysMsg(M2Share.g_sYourWifeSearchLocateMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                m_DearHuman.SysMsg(m_sCharName + ' ' + m_PEnvir.sMapDesc + '(' + m_nCurrX + ':' + m_nCurrY + ')', TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdSearchMaster(string sCmd, string sParam)
        {
            int I;
            TPlayObject Human;
            if (sParam != "" && sParam[1] == '?')
            {
                SysMsg("此命令用于查询师徒当前所在位置。", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (m_sMasterName == "")
            {
                SysMsg(M2Share.g_sYouAreNotMasterMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (m_boMaster)
            {
                if (m_MasterList.Count <= 0)
                {
                    SysMsg(M2Share.g_sYourMasterListNotOnlineMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
                SysMsg(M2Share.g_sYourMasterListNowLocateMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                for (I = 0; I < m_MasterList.Count; I++)
                {
                    Human = m_MasterList[I];
                    SysMsg(Human.m_sCharName + ' ' + Human.m_PEnvir.sMapDesc + '(' + Human.m_nCurrX + ':' + Human.m_nCurrY + ')', TMsgColor.c_Green, TMsgType.t_Hint);
                    Human.SysMsg(M2Share.g_sYourMasterSearchLocateMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                    Human.SysMsg(m_sCharName + ' ' + m_PEnvir.sMapDesc + '(' + m_nCurrX + ':' + m_nCurrY + ')', TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            else
            {
                if (m_MasterHuman == null)
                {
                    SysMsg(M2Share.g_sYourMasterNotOnlineMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
                SysMsg(M2Share.g_sYourMasterNowLocateMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                SysMsg(m_MasterHuman.m_sCharName + ' ' + m_MasterHuman.m_PEnvir.sMapDesc + '(' + m_MasterHuman.m_nCurrX + ':' + m_MasterHuman.m_nCurrY + ')', TMsgColor.c_Green, TMsgType.t_Hint);
                m_MasterHuman.SysMsg(M2Share.g_sYourMasterListSearchLocateMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                m_MasterHuman.SysMsg(m_sCharName + ' ' + m_PEnvir.sMapDesc + '(' + m_nCurrX + ':' + m_nCurrY + ')', TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdSetPermission(TGameCmd Cmd, string sHumanName, string sPermission)
        {
            int nPerission;
            TPlayObject PlayObject;
            const string sOutFormatMsg = "[权限调整] %s (%s %d -> %d)";
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nPerission = HUtil32.Str_ToInt(sPermission, 0);
            if (sHumanName == "" || !(nPerission >= 0 && nPerission <= 10))
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 人物名称 权限等级(0 - 10)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (M2Share.g_Config.boShowMakeItemMsg)
            {
                M2Share.MainOutMessage(format(sOutFormatMsg, m_sCharName, PlayObject.m_sCharName, PlayObject.m_btPermission, nPerission));
            }
            PlayObject.m_btPermission = (byte)nPerission;
            SysMsg(sHumanName + " 当前权限为: " + PlayObject.m_btPermission, TMsgColor.c_Red, TMsgType.t_Hint);
        }

        public void CmdShowHumanFlag(string sCmd, int nPermission, string sHumanName, string sFlag)
        {
            TPlayObject PlayObject;
            int nFlag;
            if (m_btPermission < nPermission)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, sCmd, M2Share.g_sGameCommandShowHumanFlagHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nFlag = HUtil32.Str_ToInt(sFlag, 0);
            if (PlayObject.GetQuestFalgStatus(nFlag) == 1)
            {

                SysMsg(format(M2Share.g_sGameCommandShowHumanFlagONMsg, PlayObject.m_sCharName, nFlag), TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {

                SysMsg(format(M2Share.g_sGameCommandShowHumanFlagOFFMsg, PlayObject.m_sCharName, nFlag), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdShowHumanUnit(string sCmd, int nPermission, string sHumanName, string sUnit)
        {
            TPlayObject PlayObject;
            int nUnit;
            if (m_btPermission < nPermission)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, sCmd, M2Share.g_sGameCommandShowHumanUnitHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nUnit = HUtil32.Str_ToInt(sUnit, 0);
            if (PlayObject.GetQuestUnitStatus(nUnit) == 1)
            {

                SysMsg(format(M2Share.g_sGameCommandShowHumanUnitONMsg, PlayObject.m_sCharName, nUnit), TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {

                SysMsg(format(M2Share.g_sGameCommandShowHumanUnitOFFMsg, PlayObject.m_sCharName, nUnit), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdShowHumanUnitOpen(string sCmd, int nPermission, string sHumanName, string sUnit)
        {
            TPlayObject PlayObject;
            int nUnit;
            if (m_btPermission < nPermission)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, sCmd, M2Share.g_sGameCommandShowHumanUnitHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nUnit = HUtil32.Str_ToInt(sUnit, 0);
            if (PlayObject.GetQuestUnitOpenStatus(nUnit) == 1)
            {

                SysMsg(format(M2Share.g_sGameCommandShowHumanUnitONMsg, PlayObject.m_sCharName, nUnit), TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {

                SysMsg(format(M2Share.g_sGameCommandShowHumanUnitOFFMsg, PlayObject.m_sCharName, nUnit), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdShowMapInfo(TGameCmd Cmd, string sParam1)
        {
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam1 != "" && sParam1[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            SysMsg(format(M2Share.g_sGameCommandMapInfoMsg, m_PEnvir.sMapName, m_PEnvir.sMapDesc), TMsgColor.c_Green, TMsgType.t_Hint);
            SysMsg(format(M2Share.g_sGameCommandMapInfoSizeMsg, new short[] { m_PEnvir.wWidth, m_PEnvir.wHeight }), TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdShowMapMode(string sCmd, string sMapName)
        {
            TEnvirnoment Envir;
            string sMsg;
            if (m_btPermission < 6)
            {
                return;
            }
            if (sMapName == "")
            {
                SysMsg("命令格式: @" + sCmd + " 地图号", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Envir = M2Share.g_MapManager.FindMap(sMapName);
            if (Envir == null)
            {
                SysMsg(sMapName + " 不存在！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            sMsg = "地图模式: " + Envir.GetEnvirInfo();
            SysMsg(sMsg, TMsgColor.c_Blue, TMsgType.t_Hint);
        }

        public void CmdSetMapMode(string sCmd, string sMapName, string sMapMode, string sParam1, string sParam2)
        {
            TEnvirnoment Envir;
            string sMsg;
            if (m_btPermission < 6)
            {
                return;
            }
            if (sMapName == "" || sMapMode == "")
            {
                SysMsg("命令格式: @" + sCmd + " 地图号 模式", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Envir = M2Share.g_MapManager.FindMap(sMapName);
            if (Envir == null)
            {
                SysMsg(sMapName + " 不存在！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (string.Compare(sMapMode.ToLower(), "SAFE".ToLower(), StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "DARK", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "FIGHT", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "FIGHT3", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "DAY", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "QUIZ", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "NORECONNECT", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "MUSIC", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "EXPRATE", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "PKWINLEVEL", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "PKWINEXP", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "PKLOSTLEVEL", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "PKLOSTEXP", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "DECHP", StringComparison.Ordinal) == 0)
            {
                if (sParam1 != "" && sParam2 != "")
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
            else if (string.Compare(sMapMode, "DECGAMEGOLD", StringComparison.Ordinal) == 0)
            {
                if (sParam1 != "" && sParam2 != "")
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
            else if (string.Compare(sMapMode, "INCGAMEGOLD", StringComparison.Ordinal) == 0)
            {
                if (sParam1 != "" && sParam2 != "")
                {
                    Envir.Flag.boINCGAMEGOLD = true;
                    Envir.Flag.nINCGAMEGOLDTIME = HUtil32.Str_ToInt(sParam1, -1);
                    Envir.Flag.nINCGAMEGOLD = HUtil32.Str_ToInt(sParam2, -1);
                }
                else
                {
                    Envir.Flag.boINCGAMEGOLD = false;
                }
            }
            else if (string.Compare(sMapMode, "INCGAMEPOINT", StringComparison.Ordinal) == 0)
            {
                if (sParam1 != "" && sParam2 != "")
                {
                    Envir.Flag.boINCGAMEPOINT = true;
                    Envir.Flag.nINCGAMEPOINTTIME = HUtil32.Str_ToInt(sParam1, -1);
                    Envir.Flag.nINCGAMEPOINT = HUtil32.Str_ToInt(sParam2, -1);
                }
                else
                {
                    Envir.Flag.boINCGAMEGOLD = false;
                }
            }
            else if (string.Compare(sMapMode, "RUNHUMAN", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "RUNMON", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "NEEDHOLE", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "NORECALL", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "NOGUILDRECALL", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "NODEARRECALL", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "NOMASTERRECALL", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "NORANDOMMOVE", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "NODRUG", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "MINE", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "MINE2", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "NOTHROWITEM", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "NODROPITEM", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "NOPOSITIONMOVE", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "NOHORSE", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "NOCHAT", StringComparison.Ordinal) == 0)
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
            else if (string.Compare(sMapMode, "NOHUMNOMON", StringComparison.Ordinal) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNOHUMNOMON = true;
                }
                else
                {
                    Envir.Flag.boNOHUMNOMON = false;
                }
            }
            sMsg = "环境: " + Envir.GetEnvirInfo();
            SysMsg(sMsg, TMsgColor.c_Blue, TMsgType.t_Hint);
        }

        public void CmdDeleteItem(TGameCmd Cmd, string sHumanName, string sItemName, int nCount)
        {
            TPlayObject PlayObject;
            int nItemCount;
            MirItem StdItem;
            TUserItem UserItem;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sItemName == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 人物名称 物品名称 数量)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nItemCount = 0;
            for (var i = PlayObject.m_ItemList.Count - 1; i <= 0; i++)
            {
                UserItem = PlayObject.m_ItemList[i];
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (StdItem != null && string.Compare(sItemName, StdItem.Name, StringComparison.Ordinal) == 0)
                {
                    PlayObject.SendDelItems(UserItem);
                    Dispose(UserItem);
                    PlayObject.m_ItemList.RemoveAt(i);
                    nItemCount++;
                    if (nItemCount >= nCount)
                    {
                        break;
                    }
                }
            }
        }

        public void CmdDelGold(TGameCmd Cmd, string sHumName, int nCount)
        {
            TPlayObject PlayObject;
            var nServerIndex = 0;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumName == "" || nCount <= 0)
            {
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (PlayObject != null)
            {
                if (PlayObject.m_nGold > nCount)
                {
                    PlayObject.m_nGold -= nCount;
                }
                else
                {
                    nCount = PlayObject.m_nGold;
                    PlayObject.m_nGold = 0;
                }
                PlayObject.GoldChanged();
                SysMsg(sHumName + "的金币已减少" + nCount + '.', TMsgColor.c_Green, TMsgType.t_Hint);
                if (M2Share.g_boGameLogGold)
                {
                    M2Share.AddGameDataLog("13" + "\t" + m_sMapName + "\t" + m_nCurrX + "\t" + m_nCurrY + "\t" + m_sCharName + "\t" + Grobal2.sSTRING_GOLDNAME + "\t" + nCount + "\t" + '1' + "\t" + sHumName);
                }
            }
            else
            {
                if (M2Share.UserEngine.FindOtherServerUser(sHumName, ref nServerIndex))
                {
                    SysMsg(sHumName + "现在" + nServerIndex + "号服务器上", TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    M2Share.FrontEngine.AddChangeGoldList(m_sCharName, sHumName, -nCount);
                    SysMsg(sHumName + "现在不在线，等其上线时金币将自动减少", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
        }

        public void CmdDelGuild(TGameCmd Cmd, string sGuildName)
        {
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (M2Share.nServerIndex != 0)
            {
                SysMsg("只能在主服务器上才可以使用此命令删除行会！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sGuildName == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 行会名称", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (M2Share.GuildManager.DelGuild(sGuildName))
            {
                M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_206, M2Share.nServerIndex, sGuildName);
            }
            else
            {
                SysMsg("没找到" + sGuildName + "这个行会！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdDelNpc(string sCmd, int nPermission, string sParam1)
        {
            TBaseObject BaseObject;
            int I;
            const string sDelOK = "删除NPC成功...";
            if (m_btPermission < nPermission)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam1 != "" && sParam1[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, sCmd, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            BaseObject = GetPoseCreate();
            if (BaseObject != null)
            {
                for (I = 0; I < M2Share.UserEngine.m_MerchantList.Count; I++)
                {
                    if (M2Share.UserEngine.m_MerchantList[I] == BaseObject)
                    {
                        BaseObject.m_boGhost = true;

                        BaseObject.m_dwGhostTick = HUtil32.GetTickCount();
                        BaseObject.SendRefMsg(Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
                        SysMsg(sDelOK, TMsgColor.c_Red, TMsgType.t_Hint);
                        return;
                    }
                }
                for (I = 0; I < M2Share.UserEngine.QuestNPCList.Count; I++)
                {
                    if (M2Share.UserEngine.QuestNPCList[I] == BaseObject)
                    {
                        BaseObject.m_boGhost = true;

                        BaseObject.m_dwGhostTick = HUtil32.GetTickCount();
                        BaseObject.SendRefMsg(Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
                        SysMsg(sDelOK, TMsgColor.c_Red, TMsgType.t_Hint);
                        return;
                    }
                }
            }
            SysMsg(M2Share.g_sGameCommandDelNpcMsg, TMsgColor.c_Red, TMsgType.t_Hint);
        }

        public void CmdDelSkill(TGameCmd Cmd, string sHumanName, string sSkillName)
        {
            int I;
            TPlayObject PlayObject;
            bool boDelAll;
            TUserMagic UserMagic;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sSkillName == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 人物名称 技能名称)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (string.Compare(sSkillName.ToLower(), "All", StringComparison.Ordinal) == 0)
            {
                boDelAll = true;
            }
            else
            {
                boDelAll = false;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            for (I = PlayObject.m_MagicList.Count - 1; I >= 0; I--)
            {
                UserMagic = PlayObject.m_MagicList[I];
                if (boDelAll)
                {
                    PlayObject.SendDelMagic(UserMagic);

                    Dispose(UserMagic);
                    PlayObject.m_MagicList.RemoveAt(I);
                }
                else
                {
                    if (string.Compare(UserMagic.MagicInfo.sMagicName, sSkillName, StringComparison.Ordinal) == 0)
                    {
                        PlayObject.SendDelMagic(UserMagic);
                        Dispose(UserMagic);
                        PlayObject.m_MagicList.RemoveAt(I);
                        PlayObject.SysMsg(format("技能{0}已删除。", sSkillName), TMsgColor.c_Green, TMsgType.t_Hint);
                        SysMsg(format("%s的技能{0}已删除。", sHumanName, sSkillName), TMsgColor.c_Green, TMsgType.t_Hint);
                        break;
                    }
                }
            }
        }

        public void CmdDenyAccountLogon(TGameCmd Cmd, string sAccount, string sFixDeny)
        {
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sAccount == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 登录帐号 是否永久封(0,1)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            try
            {
                if (sFixDeny != "" && sFixDeny[1] == '1')
                {
                    //M2Share.g_DenyAccountList.Add(sAccount, ((1) as Object));
                    M2Share.SaveDenyAccountList();
                    SysMsg(sAccount + "已加入禁止登录帐号列表", TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    //M2Share.g_DenyAccountList.Add(sAccount, ((0) as Object));
                    SysMsg(sAccount + "已加入临时禁止登录帐号列表", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            finally
            {
            }
        }

        public void CmdDenyCharNameLogon(TGameCmd Cmd, string sCharName, string sFixDeny)
        {
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sCharName == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 人物名称 是否永久封(0,1)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            try
            {
                if (sFixDeny != "" && sFixDeny[1] == '1')
                {
                    //M2Share.g_DenyChrNameList.Add(sCharName, ((1) as Object));
                    M2Share.SaveDenyChrNameList();
                    SysMsg(sCharName + "已加入禁止人物列表", TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    //M2Share.g_DenyChrNameList.Add(sCharName, ((0) as Object));
                    SysMsg(sCharName + "已加入临时禁止人物列表", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            finally
            {
            }
        }

        public void CmdDenyIPaddrLogon(TGameCmd Cmd, string sIPaddr, string sFixDeny)
        {
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sIPaddr == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " IP地址 是否永久封(0,1)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            try
            {
                if (sFixDeny != "" && sFixDeny[1] == '1')
                {
                    //M2Share.g_DenyIPAddrList.Add(sIPaddr, ((1) as Object));
                    M2Share.SaveDenyIPAddrList();
                    SysMsg(sIPaddr + "已加入禁止登录IP列表", TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    //M2Share.g_DenyIPAddrList.Add(sIPaddr, ((0) as Object));
                    SysMsg(sIPaddr + "已加入临时禁止登录IP列表", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            finally
            {
            }
        }

        public void CmdDisableFilter(string sCmd, string sParam1)
        {
            if (m_btPermission < 6)
            {
                return;
            }
            if (sParam1 != "" && sParam1[1] == '?')
            {
                SysMsg("启用/禁止文字过滤功能。", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            M2Share.boFilterWord = !M2Share.boFilterWord;
            if (M2Share.boFilterWord)
            {
                SysMsg("已启用文字过滤。", TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg("已禁止文字过滤。", TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdDelDenyAccountLogon(TGameCmd Cmd, string sAccount, string sFixDeny)
        {
            bool boDelete;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sAccount == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 登录帐号", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            boDelete = false;
            try
            {
                //for (I = 0; I < M2Share.g_DenyAccountList.Count; I++)
                //{
                //    if ((sAccount).ToLower().CompareTo((M2Share.g_DenyAccountList[I]), StringComparison.Ordinal) == 0)
                //    {
                //        if (((int)M2Share.g_DenyAccountList.Values[I]) != 0)
                //        {
                //            M2Share.SaveDenyAccountList();
                //        }
                //        M2Share.g_DenyAccountList.Remove(I);
                //        this.SysMsg(sAccount + "已从禁止登录帐号列表中删除。", TMsgColor.c_Green, TMsgType.t_Hint);
                //        boDelete = true;
                //        break;
                //    }
                //}
            }
            finally
            {
            }
            if (!boDelete)
            {
                SysMsg(sAccount + "没有被禁止登录。", TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdDelDenyCharNameLogon(TGameCmd Cmd, string sCharName, string sFixDeny)
        {
            bool boDelete;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sCharName == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 人物名称", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            boDelete = false;
            try
            {
                //for (I = 0; I < M2Share.g_DenyChrNameList.Count; I++)
                //{
                //    if ((sCharName).ToLower().CompareTo((M2Share.g_DenyChrNameList[I]), StringComparison.Ordinal) == 0)
                //    {
                //        if (((int)M2Share.g_DenyChrNameList.Values[I]) != 0)
                //        {
                //            M2Share.SaveDenyChrNameList();
                //        }
                //        M2Share.g_DenyChrNameList.Remove(I);
                //        this.SysMsg(sCharName + "已从禁止登录人物列表中删除。", TMsgColor.c_Green, TMsgType.t_Hint);
                //        boDelete = true;
                //        break;
                //    }
                //}
            }
            finally
            {
            }
            if (!boDelete)
            {
                SysMsg(sCharName + "没有被禁止登录。", TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdDelDenyIPaddrLogon(TGameCmd Cmd, string sIPaddr, string sFixDeny)
        {
            bool boDelete;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sIPaddr == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " IP地址", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            boDelete = false;
            try
            {
                //for (I = 0; I < M2Share.g_DenyIPAddrList.Count; I++)
                //{
                //    if ((sIPaddr).ToLower().CompareTo((M2Share.g_DenyIPAddrList[I]), StringComparison.Ordinal) == 0)
                //    {
                //        if (((int)M2Share.g_DenyIPAddrList.Values[I]) != 0)
                //        {
                //            M2Share.SaveDenyIPAddrList();
                //        }
                //        M2Share.g_DenyIPAddrList.Remove(I);
                //        this.SysMsg(sIPaddr + "已从禁止登录IP列表中删除。", TMsgColor.c_Green, TMsgType.t_Hint);
                //        boDelete = true;
                //        break;
                //    }
                //}
            }
            finally
            {
            }
            if (!boDelete)
            {
                SysMsg(sIPaddr + "没有被禁止登录。", TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdShowDenyAccountLogon(TGameCmd Cmd, string sAccount, string sFixDeny)
        {
            if (m_btPermission < 6)
            {
                return;
            }
            if (M2Share.g_DenyAccountList.Count <= 0)
            {
                SysMsg("禁止登录帐号列表为空。", TMsgColor.c_Green, TMsgType.t_Hint);
                return;
            }
            for (var i = 0; i < M2Share.g_DenyAccountList.Count; i++)
            {
                //this.SysMsg(M2Share.g_DenyAccountList[I], TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdShowDenyCharNameLogon(TGameCmd Cmd, string sCharName, string sFixDeny)
        {
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (M2Share.g_DenyChrNameList.Count <= 0)
            {
                SysMsg("禁止登录角色列表为空。", TMsgColor.c_Green, TMsgType.t_Hint);
                return;
            }
            for (var i = 0; i < M2Share.g_DenyChrNameList.Count; i++)
            {
                //this.SysMsg(M2Share.g_DenyChrNameList[I], TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdShowDenyIPaddrLogon(TGameCmd Cmd, string sIPaddr, string sFixDeny)
        {
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (M2Share.g_DenyIPAddrList.Count <= 0)
            {
                SysMsg("禁止登录角色列表为空。", TMsgColor.c_Green, TMsgType.t_Hint);
                return;
            }
            for (var i = 0; i < M2Share.g_DenyIPAddrList.Count; i++)
            {
                //this.SysMsg(M2Share.g_DenyIPAddrList[I], TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdDisableSendMsg(TGameCmd Cmd, string sHumanName)
        {
            TPlayObject PlayObject;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 人物名称", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject != null)
            {
                PlayObject.m_boFilterSendMsg = true;
            }
            M2Share.g_DisableSendMsgList.Add(sHumanName);
            M2Share.SaveDisableSendMsgList();
            SysMsg(sHumanName + " 已加入禁言列表。", TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdDisableSendMsgList(TGameCmd Cmd)
        {
            int I;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (M2Share.g_DisableSendMsgList.Count <= 0)
            {
                SysMsg("禁言列表为空！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            SysMsg("禁言列表:", TMsgColor.c_Blue, TMsgType.t_Hint);
            for (I = 0; I < M2Share.g_DisableSendMsgList.Count; I++)
            {
                //this.SysMsg(M2Share.g_DisableSendMsgList[I], TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdEnableSendMsg(TGameCmd Cmd, string sHumanName)
        {
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 人物名称", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            //for (I = 0; I < M2Share.g_DisableSendMsgList.Count; I++)
            //{
            //    if ((sHumanName).ToLower().CompareTo((M2Share.g_DisableSendMsgList[I]), StringComparison.Ordinal) == 0)
            //    {
            //        PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            //        if (PlayObject != null)
            //        {
            //            PlayObject.m_boFilterSendMsg = false;
            //        }
            //        M2Share.g_DisableSendMsgList.Remove(I);
            //        M2Share.SaveDisableSendMsgList();
            //        this.SysMsg(sHumanName + " 已从禁言列表中删除。", TMsgColor.c_Green, TMsgType.t_Hint);
            //        return;
            //    }
            //}
            SysMsg(sHumanName + " 没有被禁言！！！", TMsgColor.c_Red, TMsgType.t_Hint);
        }

        // procedure GetBagUseItems(var btDc:Byte;var btSc:Byte;var btMc:Byte;var btDura:Byte); //注释掉无用
        // protected
        public void CmdEndGuild()
        {
            // 4D1A44
            if (m_MyGuild != null)
            {
                if (m_nGuildRankNo > 1)
                {
                    if (m_MyGuild.IsMember(m_sCharName) && m_MyGuild.DelMember(m_sCharName))
                    {
                        M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.nServerIndex, m_MyGuild.sGuildName);
                        m_MyGuild = null;
                        RefRankInfo(0, "");
                        RefShowName();
                        // 10/31
                        SysMsg("你已经退出行会。", TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                }
                else
                {
                    SysMsg("行会掌门人不能这样退出行会！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
            else
            {
                SysMsg("你都没加入行会！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdFireBurn(int nInt, int nTime, int nN)
        {
            TFireBurnEvent FireBurnEvent;
            if (m_btPermission < 6)
            {
                return;
            }
            if (nInt == 0 || nTime == 0 || nN == 0)
            {
                SysMsg("命令格式: @" + M2Share.g_GameCommand.FIREBURN.sCmd + " nInt nTime nN", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            FireBurnEvent = new TFireBurnEvent(this, m_nCurrX, m_nCurrY, nInt, nTime, nN);
            M2Share.EventManager.AddEvent(FireBurnEvent);
        }

        public void CmdForcedWallconquestWar(TGameCmd Cmd, string sCastleName)
        {
            TUserCastle Castle;
            string s20;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sCastleName == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 城堡名称", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Castle = M2Share.CastleManager.Find(sCastleName);
            if (Castle != null)
            {
                Castle.m_boUnderWar = !Castle.m_boUnderWar;
                if (Castle.m_boUnderWar)
                {

                    Castle.m_dwStartCastleWarTick = HUtil32.GetTickCount();
                    Castle.StartWallconquestWar();
                    M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_212, M2Share.nServerIndex, "");
                    s20 = '[' + Castle.m_sName + "攻城战已经开始]";
                    M2Share.UserEngine.SendBroadCastMsg(s20, TMsgType.t_System);
                    M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_204, M2Share.nServerIndex, s20);
                    Castle.MainDoorControl(true);
                }
                else
                {
                    Castle.StopWallconquestWar();
                }
            }
            else
            {

                SysMsg(format(M2Share.g_sGameCommandSbkGoldCastleNotFoundMsg, sCastleName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdFreePenalty(TGameCmd Cmd, string sHumanName)
        {
            // 004CC528
            TPlayObject PlayObject;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName != "" && sHumanName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandFreePKHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject.m_nPkPoint = 0;
            PlayObject.RefNameColor();
            PlayObject.SysMsg(M2Share.g_sGameCommandFreePKHumanMsg, TMsgColor.c_Green, TMsgType.t_Hint);

            SysMsg(format(M2Share.g_sGameCommandFreePKMsg, sHumanName), TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdGroupRecall(string sCMD)
        {
            short dwValue;
            TPlayObject PlayObject;
            if (m_boRecallSuite || m_btPermission >= 6)
            {
                if (!m_PEnvir.Flag.boNORECALL)
                {
                    dwValue = (short)((HUtil32.GetTickCount() - m_dwGroupRcallTick) / 1000);
                    m_dwGroupRcallTick = m_dwGroupRcallTick + dwValue * 1000;
                    if (m_btPermission >= 6)
                    {
                        m_wGroupRcallTime = 0;
                    }
                    if (m_wGroupRcallTime > dwValue)
                    {
                        m_wGroupRcallTime -= dwValue;
                    }
                    else
                    {
                        m_wGroupRcallTime = 0;
                    }
                    if (m_wGroupRcallTime == 0)
                    {
                        if (m_GroupOwner == this)
                        {
                            for (var I = 1; I < m_GroupMembers.Count; I++)
                            {
                                PlayObject = m_GroupMembers[I];
                                if (PlayObject.m_boAllowGroupReCall)
                                {
                                    if (PlayObject.m_PEnvir.Flag.boNORECALL)
                                    {

                                        SysMsg(string.Format("%s 地图不允许传唤.", new string[] { PlayObject.m_sCharName }), TMsgColor.c_Red, TMsgType.t_Hint);
                                    }
                                    else
                                    {
                                        RecallHuman(PlayObject.m_sCharName);
                                    }
                                }
                                else
                                {
                                    SysMsg(string.Format("%s 拒绝天地合一.", new string[] { PlayObject.m_sCharName }), TMsgColor.c_Red, TMsgType.t_Hint);
                                }
                            }
                            m_dwGroupRcallTick = HUtil32.GetTickCount();
                            m_wGroupRcallTime = (short)M2Share.g_Config.nGroupRecallTime;
                        }
                    }
                    else
                    {
                        SysMsg(string.Format("%d 秒后才能再次使用.", new int[] { m_wGroupRcallTime }), TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                }
                else
                {
                    SysMsg("此地图禁止使用此命令！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
            else
            {
                SysMsg("您现在还无法使用此功能！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdGuildRecall(string sCmd, string sParam)
        {
            int dwValue;
            TPlayObject PlayObject;
            TGuildRank GuildRank;
            int nRecallCount;
            int nNoRecallCount;
            TUserCastle Castle;
            if (sParam != "" && sParam[1] == '?')
            {
                SysMsg("命令功能: 行会传送，行会掌门人可以将整个行会成员全部集中。", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (!m_boGuildMove && m_btPermission < 6)
            {
                SysMsg("您现在还无法使用此功能！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (!IsGuildMaster())
            {
                SysMsg("行会掌门人才可以使用此功能！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (m_PEnvir.Flag.boNOGUILDRECALL)
            {
                SysMsg("本地图不允许使用此功能！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Castle = M2Share.CastleManager.InCastleWarArea(this);
            if (Castle != null && Castle.m_boUnderWar)
            {
                SysMsg("攻城区域不允许使用此功能！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nRecallCount = 0;
            nNoRecallCount = 0;
            dwValue = (HUtil32.GetTickCount() - m_dwGroupRcallTick) / 1000;
            m_dwGroupRcallTick = m_dwGroupRcallTick + dwValue * 1000;
            if (m_btPermission >= 6)
            {
                m_wGroupRcallTime = 0;
            }
            if (m_wGroupRcallTime > dwValue)
            {
                m_wGroupRcallTime -= (short)dwValue;
            }
            else
            {
                m_wGroupRcallTime = 0;
            }
            if (m_wGroupRcallTime > 0)
            {
                SysMsg(format("%d 秒之后才可以再使用此功能！！！", m_wGroupRcallTime), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            for (var i = 0; i < m_MyGuild.m_RankList.Count; i++)
            {
                GuildRank = m_MyGuild.m_RankList[i];
                for (var j = 0; j < GuildRank.MemberList.Count; j++)
                {
                    PlayObject = GuildRank.MemberList[j];
                    if (PlayObject != null)
                    {
                        if (PlayObject == this)
                        {
                            // Inc(nNoRecallCount);
                            continue;
                        }
                        if (PlayObject.m_boAllowGuildReCall)
                        {
                            if (PlayObject.m_PEnvir.Flag.boNORECALL)
                            {
                                SysMsg(format("%s 所在的地图不允许传送。", PlayObject.m_sCharName), TMsgColor.c_Red, TMsgType.t_Hint);
                            }
                            else
                            {
                                RecallHuman(PlayObject.m_sCharName);
                                nRecallCount++;
                            }
                        }
                        else
                        {
                            nNoRecallCount++;
                            SysMsg(format("%s 不允许行会合一！！！", PlayObject.m_sCharName), TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                    }
                }
            }
            // SysMsg('已传送' + IntToStr(nRecallCount) + '个成员，' + IntToStr(nNoRecallCount) + '个成员未被传送。',c_Green,t_Hint);
            SysMsg(format("已传送%d个成员，%d个成员未被传送。", nRecallCount, nNoRecallCount), TMsgColor.c_Green, TMsgType.t_Hint);
            m_dwGroupRcallTick = HUtil32.GetTickCount();
            m_wGroupRcallTime = (short)M2Share.g_Config.nGuildRecallTime;
        }

        public void CmdGuildWar(string sCmd, string sGuildName)
        {
            if (m_btPermission < 6)
            {
                return;
            }
        }

        public void CmdHair(TGameCmd Cmd, string sHumanName, int nHair)
        {
            TPlayObject PlayObject;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || nHair < 0)
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 人物名称 类型值", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject != null)
            {
                PlayObject.m_btHair = (byte)nHair;
                PlayObject.FeatureChanged();
                SysMsg(sHumanName + " 的头发已改变。", TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdHumanInfo(TGameCmd Cmd, string sHumanName)
        {
            // 004CFC98
            TPlayObject PlayObject;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandInfoHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            SysMsg(PlayObject.GeTBaseObjectInfo(), TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdHumanLocal(TGameCmd Cmd, string sHumanName)
        {
            TPlayObject PlayObject;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandHumanLocalHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            // GetIPLocal(PlayObject.m_sIPaddr)

            SysMsg(format(M2Share.g_sGameCommandHumanLocalMsg, sHumanName, m_sIPLocal), TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdHunger(string sCmd, string sHumanName, int nHungerPoint)
        {
            TPlayObject PlayObject;
            if (m_btPermission < 6)
            {
                return;
            }
            if (sHumanName == "" || nHungerPoint < 0)
            {
                SysMsg("命令格式: @" + sCmd + " 人物名称 能量值", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject != null)
            {
                PlayObject.m_nHungerStatus = nHungerPoint;
                PlayObject.SendMsg(PlayObject, Grobal2.RM_MYSTATUS, 0, 0, 0, 0, "");
                PlayObject.RefMyStatus();
                SysMsg(sHumanName + " 的能量值已改变。", TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg(sHumanName + "没有在线！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdIncPkPoint(TGameCmd Cmd, string sHumanName, int nPoint)
        {
            // 004BF4D4
            TPlayObject PlayObject;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName != "" && sHumanName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandIncPkPointHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject.m_nPkPoint += nPoint;
            PlayObject.RefNameColor();
            if (nPoint > 0)
            {

                SysMsg(format(M2Share.g_sGameCommandIncPkPointAddPointMsg, sHumanName, nPoint), TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {

                SysMsg(format(M2Share.g_sGameCommandIncPkPointDecPointMsg, sHumanName, -nPoint), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdKickHuman(TGameCmd Cmd, string sHumName)
        {
            TPlayObject PlayObject;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumName == "" || sHumName != "" && sHumName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandKickHumanHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (PlayObject != null)
            {
                PlayObject.m_boKickFlag = true;
                PlayObject.m_boEmergencyClose = true;
            }
            else
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdKill(TGameCmd Cmd, string sHumanName)
        {
            TBaseObject BaseObject;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName != "")
            {
                BaseObject = M2Share.UserEngine.GetPlayObject(sHumanName);
                if (BaseObject == null)
                {

                    SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
            }
            else
            {
                BaseObject = GetPoseCreate();
                if (BaseObject == null)
                {
                    SysMsg("命令使用方法不正确，必须与角色面对面站好！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
            }
            BaseObject.Die();
        }

        public void CmdLockLogin(TGameCmd Cmd)
        {
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (!M2Share.g_Config.boLockHumanLogin)
            {
                SysMsg("本服务器还没有启用登录锁功能！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (m_boLockLogon && !m_boLockLogoned)
            {
                SysMsg("您还没有打开登录锁或还没有设置锁密码！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            m_boLockLogon = !m_boLockLogon;
            if (m_boLockLogon)
            {
                SysMsg("已开启登录锁", TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg("已关闭登录锁", TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdLotteryTicket(string sCmd, int nPerMission, string sParam1)
        {
            if (m_btPermission < nPerMission)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam1 == "" || sParam1 != "" && sParam1[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, sCmd, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }

            SysMsg(format(M2Share.g_sGameCommandLotteryTicketMsg, M2Share.g_Config.nWinLotteryCount, M2Share.g_Config.nNoWinLotteryCount, M2Share.g_Config.nWinLotteryLevel1, M2Share.g_Config.nWinLotteryLevel2, M2Share.g_Config.nWinLotteryLevel3, M2Share.g_Config.nWinLotteryLevel4, M2Share.g_Config.nWinLotteryLevel5, M2Share.g_Config.nWinLotteryLevel6), TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdLuckPoint(string sCmd, int nPerMission, string sHumanName, string sCtr, string sPoint)
        {
            TPlayObject PlayObject;
            if (m_btPermission < nPerMission)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, sCmd, M2Share.g_sGameCommandLuckPointHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sCtr == "")
            {

                SysMsg(format(M2Share.g_sGameCommandLuckPointMsg, sHumanName, PlayObject.m_nBodyLuckLevel, PlayObject.m_dBodyLuck, PlayObject.m_nLuck), TMsgColor.c_Green, TMsgType.t_Hint);
                return;
            }
        }

        public void CmdMapMove(TGameCmd Cmd, string sMapName)
        {
            TEnvirnoment Envir;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sMapName == "" || sMapName != "" && sMapName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandMoveHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Envir = M2Share.g_MapManager.FindMap(sMapName);
            if (Envir == null)
            {
                // + ' 此地图号不存在！！！'

                SysMsg(format(M2Share.g_sTheMapNotFound, sMapName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (m_btPermission >= Cmd.nPerMissionMax || M2Share.CanMoveMap(sMapName))
            {
                SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                MapRandomMove(sMapName, 0);
            }
            else
            {
                // '地图 ' + sParam1 + ' 不允许传送！！！'

                SysMsg(format(M2Share.g_sTheMapDisableMove, sMapName, Envir.sMapDesc), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdPositionMove(TGameCmd Cmd, string sMapName, string sX, string sY)
        {
            TEnvirnoment Envir = null;
            short nX = 0;
            short nY = 0;
            try
            {
                if (m_btPermission < Cmd.nPerMissionMin)
                {
                    SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
                if (sMapName == "" || sX == "" || sY == "" || sMapName != "" && sMapName[1] == '?')
                {

                    SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandPositionMoveHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
                if (m_btPermission >= Cmd.nPerMissionMax || M2Share.CanMoveMap(sMapName))
                {
                    Envir = M2Share.g_MapManager.FindMap(sMapName);
                    if (Envir != null)
                    {
                        nX = (short)HUtil32.Str_ToInt(sX, 0);
                        nY = (short)HUtil32.Str_ToInt(sY, 0);
                        if (Envir.CanWalk(nX, nY, true))
                        {
                            SpaceMove(sMapName, nX, nY, 0);
                        }
                        else
                        {

                            SysMsg(format(M2Share.g_sGameCommandPositionMoveCanotMoveToMap, sMapName, sX, sY), TMsgColor.c_Green, TMsgType.t_Hint);
                        }
                    }
                }
                else
                {

                    SysMsg(format(M2Share.g_sTheMapDisableMove, sMapName, Envir.sMapDesc), TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage("[Exceptioin] TPlayObject.CmdPositionMove");
                M2Share.ErrorMessage(e.Message);
            }
        }

        public void CmdMapMoveHuman(TGameCmd Cmd, string sSrcMap, string sDenMap)
        {
            TEnvirnoment SrcEnvir;
            TEnvirnoment DenEnvir;
            IList<TBaseObject> HumanList;
            int I;
            TPlayObject MoveHuman;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sDenMap == "" || sSrcMap == "" || sSrcMap != "" && sSrcMap[1] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandMapMoveHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            SrcEnvir = M2Share.g_MapManager.FindMap(sSrcMap);
            DenEnvir = M2Share.g_MapManager.FindMap(sDenMap);
            if (SrcEnvir == null)
            {
                SysMsg(format(M2Share.g_sGameCommandMapMoveMapNotFound, sSrcMap), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (DenEnvir == null)
            {
                SysMsg(format(M2Share.g_sGameCommandMapMoveMapNotFound, sDenMap), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            HumanList = new List<TBaseObject>();
            M2Share.UserEngine.GetMapRageHuman(SrcEnvir, SrcEnvir.wWidth / 2, SrcEnvir.wHeight / 2, 1000, HumanList);
            for (I = 0; I < HumanList.Count; I++)
            {
                MoveHuman = HumanList[I] as TPlayObject;
                if (MoveHuman != this)
                {
                    MoveHuman.MapRandomMove(sDenMap, 0);
                }
            }
            //HumanList.Free;
        }

        public void CmdMemberFunction(string sCmd, string sParam)
        {
            if (sParam != "" && sParam[1] == '?')
            {
                SysMsg("打开会员功能窗口.", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (M2Share.g_ManageNPC != null)
            {
                M2Share.g_ManageNPC.GotoLable(this, "@Member", false);
            }
        }

        public void CmdMemberFunctionEx(string sCmd, string sParam)
        {
            if (sParam != "" && sParam[1] == '?')
            {
                SysMsg("打开会员功能窗口.", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(this, "@Member", false);
            }
        }

        public void CmdMission(TGameCmd Cmd, string sX, string sY)
        {
            // 004CCA08
            short nX = 0;
            short nY = 0;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sX == "" || sY == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " X  Y", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nX = (short)HUtil32.Str_ToInt(sX, 0);
            nY = (short)HUtil32.Str_ToInt(sY, 0);
            M2Share.g_boMission = true;
            M2Share.g_sMissionMap = m_sMapName;
            M2Share.g_nMissionX = nX;
            M2Share.g_nMissionY = nY;
            SysMsg("怪物集中目标已设定为: " + m_sMapName + '(' + M2Share.g_nMissionX + ':' + M2Share.g_nMissionY + ')', TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdMob(TGameCmd Cmd, string sMonName, int nCount, int nLevel, int nExpRatio)
        {
            short nX = 0;
            short nY = 0;
            TBaseObject Monster;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sMonName == "" || sMonName != "" && sMonName[1] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandMobHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (nCount <= 0)
            {
                nCount = 1;
            }
            if (!(nLevel >= 0 && nLevel <= 10))
            {
                nLevel = 0;
            }
            nCount = HUtil32._MIN(64, nCount);
            GetFrontPosition(ref nX, ref nY);
            for (var i = 0; i < nCount; i++)
            {
                Monster = M2Share.UserEngine.RegenMonsterByName(m_PEnvir.sMapName, nX, nY, sMonName);
                if (Monster != null)
                {
                    Monster.m_btSlaveMakeLevel = (byte)nLevel;
                    Monster.m_btSlaveExpLevel = (byte)nLevel;
                    Monster.RecalcAbilitys();
                    Monster.RefNameColor();
                    if (nExpRatio != -1)
                    {
                        nExpRatio = HUtil32._MIN(100, nExpRatio);
                        Monster.m_dwFightExp = Monster.m_dwFightExp * nExpRatio;
                    }
                }
                else
                {
                    SysMsg(M2Share.g_sGameCommandMobMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    break;
                }
            }
        }

        public void CmdMob(TGameCmd Cmd, string sMonName, int nCount, int nLevel)
        {
            CmdMob(Cmd, sMonName, nCount, nLevel, -1);
        }

        public void CmdMobCount(TGameCmd Cmd, string sMapName)
        {
            TEnvirnoment Envir;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sMapName == "" || sMapName != "" && sMapName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandMobCountHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Envir = M2Share.g_MapManager.FindMap(sMapName);
            if (Envir == null)
            {
                SysMsg(M2Share.g_sGameCommandMobCountMapNotFound, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }

            SysMsg(format(M2Share.g_sGameCommandMobCountMonsterCount, M2Share.UserEngine.GetMapMonster(Envir, null)), TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdHumanCount(TGameCmd Cmd, string sMapName)
        {
            TEnvirnoment Envir;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sMapName == "" || sMapName != "" && sMapName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandHumanCountHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Envir = M2Share.g_MapManager.FindMap(sMapName);
            if (Envir == null)
            {
                SysMsg(M2Share.g_sGameCommandMobCountMapNotFound, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }

            SysMsg(format(M2Share.g_sGameCommandMobCountMonsterCount, M2Share.UserEngine.GetMapHuman(sMapName)), TMsgColor.c_Green, TMsgType.t_Hint);
            SysMsg(Envir.HumCount.ToString(), TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdMobFireBurn(TGameCmd Cmd, string sMap, string sX, string sY, string sType, string sTime, string sPoint)
        {
            short nX = 0;
            short nY = 0;
            int nType;
            int nTime;
            int nPoint;
            TFireBurnEvent FireBurnEvent;
            TEnvirnoment Envir;
            TEnvirnoment OldEnvir;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sMap == "" || sMap != "" && sMap[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandMobFireBurnHelpMsg, Cmd.sCmd, sMap, sX, sY, sType, sTime, sPoint), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nX = (short)HUtil32.Str_ToInt(sX, -1);
            nY = (short)HUtil32.Str_ToInt(sY, -1);
            nType = HUtil32.Str_ToInt(sType, -1);
            nTime = HUtil32.Str_ToInt(sTime, -1);
            nPoint = HUtil32.Str_ToInt(sPoint, -1);
            if (nPoint < 0)
            {
                nPoint = 1;
            }
            if (sMap == "" || nX < 0 || nY < 0 || nType < 0 || nTime < 0 || nPoint < 0)
            {

                SysMsg(format(M2Share.g_sGameCommandMobFireBurnHelpMsg, Cmd.sCmd, sMap, sX, sY, sType, sTime, sPoint), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Envir = M2Share.g_MapManager.FindMap(sMap);
            if (Envir != null)
            {
                OldEnvir = m_PEnvir;
                m_PEnvir = Envir;
                FireBurnEvent = new TFireBurnEvent(this, nX, nY, nType, nTime * 1000, nPoint);
                M2Share.EventManager.AddEvent(FireBurnEvent);
                m_PEnvir = OldEnvir;
                return;
            }

            SysMsg(format(M2Share.g_sGameCommandMobFireBurnMapNotFountMsg, Cmd.sCmd, sMap), TMsgColor.c_Red, TMsgType.t_Hint);
        }

        public void CmdMobLevel(TGameCmd Cmd, string Param)
        {
            IList<TBaseObject> BaseObjectList;
            TBaseObject BaseObject;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (Param != "" && Param[1] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            BaseObjectList = new List<TBaseObject>();
            m_PEnvir.GetRangeBaseObject(m_nCurrX, m_nCurrY, 2, true, BaseObjectList);
            for (var i = 0; i < BaseObjectList.Count; i++)
            {
                BaseObject = BaseObjectList[i];
                SysMsg(BaseObject.GeTBaseObjectInfo(), TMsgColor.c_Green, TMsgType.t_Hint);
            }
            BaseObjectList.Clear();
            BaseObjectList = null;
        }

        public void CmdMobNpc(string sCmd, int nPermission, string sParam1, string sParam2, string sParam3, string sParam4)
        {
            int nAppr;
            bool boIsCastle;
            TMerchant Merchant;
            short nX = 0;
            short nY = 0;
            if (m_btPermission < nPermission)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam1 == "" || sParam2 == "" || sParam1 != "" && sParam1[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, sCmd, M2Share.g_sGameCommandMobNpcHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nAppr = HUtil32.Str_ToInt(sParam3, 0);
            boIsCastle = HUtil32.Str_ToInt(sParam4, 0) == 1;
            if (sParam1 == "")
            {
                SysMsg("命令格式: @" + sCmd + " NPC名称 脚本文件名 外形(数字) 属沙城(0,1)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Merchant = new TMerchant
            {
                m_sCharName = sParam1,
                m_sMapName = m_sMapName,
                m_PEnvir = m_PEnvir,
                m_wAppr = (ushort)nAppr,
                m_nFlag = 0,
                m_boCastle = boIsCastle,
                m_sScript = sParam2
            };
            GetFrontPosition(ref nX, ref nY);
            Merchant.m_nCurrX = nX;
            Merchant.m_nCurrY = nY;
            Merchant.Initialize();
            M2Share.UserEngine.AddMerchant(Merchant);
        }

        public void CmdMobPlace(TGameCmd Cmd, string sX, string sY, string sMonName, string sCount)
        {
            // 004CCBB4
            int I;
            int nCount;
            short nX = 0;
            short nY = 0;
            TEnvirnoment MEnvir;
            TBaseObject Mon;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nCount = HUtil32._MIN(500, HUtil32.Str_ToInt(sCount, 0));
            nX = (short)HUtil32.Str_ToInt(sX, 0);
            nY = (short)HUtil32.Str_ToInt(sY, 0);
            MEnvir = M2Share.g_MapManager.FindMap(M2Share.g_sMissionMap);
            if (nX <= 0 || nY <= 0 || sMonName == "" || nCount <= 0)
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " X  Y 怪物名称 怪物数量", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (!M2Share.g_boMission || MEnvir == null)
            {
                SysMsg("还没有设定怪物集中点！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                SysMsg("请先用命令" + M2Share.g_GameCommand.MISSION.sCmd + "设置怪物的集中点。", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            for (I = 0; I < nCount; I++)
            {
                Mon = M2Share.UserEngine.RegenMonsterByName(M2Share.g_sMissionMap, nX, nY, sMonName);
                if (Mon != null)
                {
                    Mon.m_boMission = true;
                    Mon.m_nMissionX = M2Share.g_nMissionX;
                    Mon.m_nMissionY = M2Share.g_nMissionY;
                }
                else
                {
                    break;
                }
            }
            SysMsg(nCount + " 只 " + sMonName + " 已正在往地图 " + M2Share.g_sMissionMap + ' ' + M2Share.g_nMissionX + ':' + M2Share.g_nMissionY + " 集中。", TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdNpcScript(string sCmd, int nPermission, string sParam1, string sParam2, string sParam3)
        {
            TBaseObject BaseObject;
            int nNPCType;
            int I;
            var sScriptFileName = string.Empty;
            TMerchant Merchant;
            TNormNpc NormNpc;
            StringList LoadList;
            string sScriptLine;
            if (m_btPermission < nPermission)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam1 == "" || sParam1 != "" && sParam1[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, sCmd, M2Share.g_sGameCommandNpcScriptHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nNPCType = -1;
            BaseObject = GetPoseCreate();
            if (BaseObject != null)
            {
                for (I = 0; I < M2Share.UserEngine.m_MerchantList.Count; I++)
                {
                    if (M2Share.UserEngine.m_MerchantList[I] == BaseObject)
                    {
                        nNPCType = 0;
                        break;
                    }
                }
                for (I = 0; I < M2Share.UserEngine.QuestNPCList.Count; I++)
                {
                    if (M2Share.UserEngine.QuestNPCList[I] == BaseObject)
                    {
                        nNPCType = 1;
                        break;
                    }
                }
            }
            if (nNPCType < 0)
            {
                SysMsg("命令使用方法不正确，必须与NPC面对面，才能使用此命令！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam1 == "")
            {
                if (nNPCType == 0)
                {
                    Merchant = (TMerchant)BaseObject;
                    sScriptFileName = M2Share.g_Config.sEnvirDir + M2Share.sMarket_Def + Merchant.m_sScript + '-' + Merchant.m_sMapName + ".txt";
                }
                if (nNPCType == 1)
                {
                    NormNpc = (TNormNpc)BaseObject;
                    sScriptFileName = M2Share.g_Config.sEnvirDir + M2Share.sNpc_def + NormNpc.m_sCharName + '-' + NormNpc.m_sMapName + ".txt";
                }
                if (File.Exists(sScriptFileName))
                {
                    LoadList = new StringList();
                    try
                    {
                        LoadList.LoadFromFile(sScriptFileName);
                    }
                    catch
                    {
                        SysMsg("读取脚本文件错误: " + sScriptFileName, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    for (I = 0; I < LoadList.Count; I++)
                    {
                        sScriptLine = LoadList[I].Trim();
                        sScriptLine = HUtil32.ReplaceChar(sScriptLine, ' ', ',');
                        SysMsg(I.ToString() + ',' + sScriptLine, TMsgColor.c_Blue, TMsgType.t_Hint);
                    }
                    //LoadList.Free;
                }
            }
        }

        public void CmdOPDeleteSkill(string sHumanName, string sSkillName)
        {
            if (m_btPermission < 6)
            {
                return;
            }
        }

        public void CmdOPTraining(string sHumanName, string sSkillName, int nLevel)
        {
            // 004CC468
            if (m_btPermission < 6)
            {
                return;
            }
        }

        public void CmdPKpoint(TGameCmd Cmd, string sHumanName)
        {
            // 004CC61C
            TPlayObject PlayObject;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName != "" && sHumanName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandPKPointHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }

            SysMsg(format(M2Share.g_sGameCommandPKPointMsg, sHumanName, PlayObject.m_nPkPoint), TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdPrvMsg(string sCmd, int nPermission, string sHumanName)
        {
            if (m_btPermission < nPermission)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, sCmd, M2Share.g_sGameCommandPrvMsgHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            //for (I = 0; I < this.m_BlockWhisperList.Count; I++)
            //{
            //    if ((this.m_BlockWhisperList[I]).ToLower().CompareTo((sHumanName), StringComparison.Ordinal) == 0)
            //    {
            //        this.m_BlockWhisperList.Remove(I);

            //        this.SysMsg(format(M2Share.g_sGameCommandPrvMsgUnLimitMsg, new string[] { sHumanName }), TMsgColor.c_Green, TMsgType.t_Hint);
            //        return;
            //    }
            //}
            m_BlockWhisperList.Add(sHumanName);

            SysMsg(format(M2Share.g_sGameCommandPrvMsgLimitMsg, sHumanName), TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdReAlive(TGameCmd Cmd, string sHumanName)
        {
            TPlayObject PlayObject;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandReAliveHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject.ReAlive();
            PlayObject.m_WAbil.HP = PlayObject.m_WAbil.MaxHP;
            PlayObject.SendMsg(PlayObject, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");

            SysMsg(format(M2Share.g_sGameCommandReAliveMsg, sHumanName), TMsgColor.c_Green, TMsgType.t_Hint);
            SysMsg(sHumanName + " 已获重生。", TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdRecallHuman(TGameCmd Cmd, string sHumanName)
        {
            // 004CE250
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandRecallHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            RecallHuman(sHumanName);
        }

        public void CmdRecallMob(TGameCmd Cmd, string sMonName, int nCount, int nLevel, int nAutoChangeColor, int nFixColor)
        {
            int I;
            short n10 = 0;
            short n14 = 0;
            TBaseObject Mon;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sMonName == "" || sMonName != "" && sMonName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandRecallMobHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (nLevel >= 10)
            {
                nLevel = 0;
            }
            if (nCount <= 0)
            {
                nCount = 1;
            }
            for (I = 0; I < nCount; I++)
            {
                if (m_SlaveList.Count >= 20)
                {
                    break;
                }
                GetFrontPosition(ref n10, ref n14);
                Mon = M2Share.UserEngine.RegenMonsterByName(m_PEnvir.sMapName, n10, n14, sMonName);
                if (Mon != null)
                {
                    Mon.m_Master = this;

                    Mon.m_dwMasterRoyaltyTick = HUtil32.GetTickCount() + 24 * 60 * 60 * 1000;
                    Mon.m_btSlaveMakeLevel = 3;
                    Mon.m_btSlaveExpLevel = (byte)nLevel;
                    if (nAutoChangeColor == 1)
                    {
                        Mon.m_boAutoChangeColor = true;
                    }
                    else if (nFixColor > 0)
                    {
                        Mon.m_boFixColor = true;
                        Mon.m_nFixColorIdx = nFixColor - 1;
                    }
                    Mon.RecalcAbilitys();
                    Mon.RefNameColor();
                    m_SlaveList.Add(Mon);
                }
            }
        }

        public void CmdReconnection(string sCmd, string sIPaddr, string sPort)
        {
            // 004CE380
            if (m_btPermission < 6)
            {
                return;
            }
            if (sIPaddr != "" && sIPaddr[1] == '?')
            {
                SysMsg("此命令用于改变客户端连接网关的IP及端口。", TMsgColor.c_Blue, TMsgType.t_Hint);
                return;
            }
            if (sIPaddr == "" || sPort == "")
            {
                SysMsg("命令格式: @" + sCmd + " IP地址 端口", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sIPaddr != "" && sPort != "")
            {
                SendMsg(this, Grobal2.RM_RECONNECTION, 0, 0, 0, 0, sIPaddr + '/' + sPort);
            }
        }

        public void CmdReGotoHuman(TGameCmd Cmd, string sHumanName)
        {
            TPlayObject PlayObject;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandReGotoHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            SpaceMove(PlayObject.m_PEnvir.sMapName, PlayObject.m_nCurrX, PlayObject.m_nCurrY, 0);
        }

        public void CmdReloadAbuse(string sCmd, int nPerMission, string sParam1)
        {
            if (m_btPermission < nPerMission)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam1 != "" && sParam1[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, sCmd, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
        }

        public void CmdReLoadAdmin(string sCmd)
        {
            if (m_btPermission < 6)
            {
                return;
            }
            M2Share.LocalDB.LoadAdminList();
            M2Share.UserEngine.SendServerGroupMsg(213, M2Share.nServerIndex, "");
            SysMsg("管理员列表重新加载成功...", TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdReloadGuild(string sCmd, int nPerMission, string sParam1)
        {
            TGuild Guild;
            if (m_btPermission < nPerMission)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam1 == "" || sParam1 != "" && sParam1[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, sCmd, M2Share.g_sGameCommandReloadGuildHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (M2Share.nServerIndex != 0)
            {
                SysMsg(M2Share.g_sGameCommandReloadGuildOnMasterserver, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Guild = M2Share.GuildManager.FindGuild(sParam1);
            if (Guild == null)
            {

                SysMsg(format(M2Share.g_sGameCommandReloadGuildNotFoundGuildMsg, sParam1), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Guild.LoadGuild();

            SysMsg(format(M2Share.g_sGameCommandReloadGuildSuccessMsg, sParam1), TMsgColor.c_Red, TMsgType.t_Hint);
            M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.nServerIndex, sParam1);
        }

        public void CmdReloadGuildAll()
        {
            // 004CE530
            if (m_btPermission < 6)
            {
                return;
            }
        }

        public void CmdReloadLineNotice(string sCmd, int nPerMission, string sParam1)
        {
            if (m_btPermission < nPerMission)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam1 != "" && sParam1[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, sCmd, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (M2Share.LoadLineNotice(M2Share.g_Config.sNoticeDir + "LineNotice.txt"))
            {
                SysMsg(M2Share.g_sGameCommandReloadLineNoticeSuccessMsg, TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg(M2Share.g_sGameCommandReloadLineNoticeFailMsg, TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdReloadManage(TGameCmd Cmd, string sParam)
        {
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam != "" && sParam[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam == "")
            {
                if (M2Share.g_ManageNPC != null)
                {
                    M2Share.g_ManageNPC.ClearScript();
                    M2Share.g_ManageNPC.LoadNPCScript();
                    SysMsg("重新加载登录脚本完成...", TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    SysMsg("重新加载登录脚本失败...", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            else
            {
                if (M2Share.g_FunctionNPC != null)
                {
                    M2Share.g_FunctionNPC.ClearScript();
                    M2Share.g_FunctionNPC.LoadNPCScript();
                    SysMsg("重新加载功能脚本完成...", TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    SysMsg("重新加载功能脚本失败...", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
        }

        public void CmdReloadRobot()
        {
            M2Share.RobotManage.ReLoadRobot();
            SysMsg("重新加载机器人配置完成...", TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdReloadRobotManage()
        {
            if (m_btPermission < 6)
            {
                return;
            }
            if (M2Share.g_RobotNPC != null)
            {
                M2Share.g_RobotNPC.ClearScript();
                M2Share.g_RobotNPC.LoadNPCScript();
                SysMsg("重新加载机器人专用脚本完成...", TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg("重新加载机器人专用脚本失败...", TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdReloadMonItems()
        {
            int I;
            TMonInfo Monster;
            if (m_btPermission < 6)
            {
                return;
            }
            try
            {
                for (I = 0; I < M2Share.UserEngine.MonsterList.Count; I++)
                {
                    Monster = M2Share.UserEngine.MonsterList[I];
                    M2Share.LocalDB.LoadMonitems(Monster.sName, ref Monster.ItemList);
                }
                SysMsg("怪物爆物品列表重加载完成...", TMsgColor.c_Green, TMsgType.t_Hint);
            }
            catch
            {
                SysMsg("怪物爆物品列表重加载失败！！！", TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void CmdReloadNpc(string sParam)
        {
            IList<TBaseObject> TmpList;
            TMerchant Merchant;
            TNormNpc Npc;
            if (m_btPermission < 6)
            {
                return;
            }
            if (string.Compare("all", sParam, StringComparison.Ordinal) == 0)
            {
                M2Share.LocalDB.ReLoadMerchants();
                M2Share.UserEngine.ReloadMerchantList();
                SysMsg("交易NPC重新加载完成！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                M2Share.UserEngine.ReloadNpcList();
                SysMsg("管理NPC重新加载完成！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            TmpList = new List<TBaseObject>();
            if (M2Share.UserEngine.GetMerchantList(m_PEnvir, m_nCurrX, m_nCurrY, 9, TmpList) > 0)
            {
                for (var i = 0; i < TmpList.Count; i++)
                {
                    Merchant = (TMerchant)TmpList[i];
                    Merchant.ClearScript();
                    Merchant.LoadNPCScript();
                    SysMsg(Merchant.m_sCharName + "重新加载成功...", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            else
            {
                SysMsg("附近未发现任何交易NPC！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
            TmpList.Clear();
            if (M2Share.UserEngine.GetNpcList(m_PEnvir, m_nCurrX, m_nCurrY, 9, TmpList) > 0)
            {
                for (var i = 0; i < TmpList.Count; i++)
                {
                    Npc = (TNormNpc)TmpList[i];
                    Npc.ClearScript();
                    Npc.LoadNPCScript();
                    SysMsg(Npc.m_sCharName + "重新加载成功...", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            else
            {
                SysMsg("附近未发现任何管理NPC！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
            //TmpList.Free;
        }

        public void CmdSearchHuman(string sCmd, string sHumanName)
        {
            TPlayObject PlayObject;
            if (m_boProbeNecklace || m_btPermission >= 6)
            {
                if (sHumanName == "")
                {
                    SysMsg("命令格式: @" + sCmd + " 人物名称", TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
                if (HUtil32.GetTickCount() - m_dwProbeTick > 10000 || m_btPermission >= 3)
                {
                    m_dwProbeTick = HUtil32.GetTickCount();
                    PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
                    if (PlayObject != null)
                    {
                        SysMsg(sHumanName + " 现在位于 " + PlayObject.m_PEnvir.sMapDesc + ' ' + PlayObject.m_nCurrX + ':' + PlayObject.m_nCurrY, TMsgColor.c_Blue, TMsgType.t_Hint);
                    }
                    else
                    {
                        SysMsg(sHumanName + " 现在不在线，或位于其它服务器上！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                }
                else
                {
                    SysMsg((HUtil32.GetTickCount() - m_dwProbeTick) / 1000 - 10 + " 秒之后才可以再使用此功能！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
            else
            {
                SysMsg("您现在还无法使用此功能！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdShowSbkGold(TGameCmd Cmd, string sCastleName, string sCtr, string sGold)
        {
            char Ctr;
            int nGold;
            TUserCastle Castle;
            ArrayList List;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sCastleName != "" && sCastleName[1] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sCastleName == "")
            {
                List = new ArrayList();
                M2Share.CastleManager.GetCastleGoldInfo(List);
                for (var i = 0; i < List.Count; i++)
                {
                    //this.SysMsg(List[I], TMsgColor.c_Green, TMsgType.t_Hint);
                }
                return;
            }
            Castle = M2Share.CastleManager.Find(sCastleName);
            if (Castle == null)
            {
                SysMsg(format(M2Share.g_sGameCommandSbkGoldCastleNotFoundMsg, sCastleName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Ctr = sCtr[1];
            nGold = HUtil32.Str_ToInt(sGold, -1);
            if (!new ArrayList(new char[] { '=', '-', '+' }).Contains(Ctr) || nGold < 0 || nGold > 100000000)
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandSbkGoldHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            switch (Ctr)
            {
                case '=':
                    Castle.m_nTotalGold = nGold;
                    break;
                case '-':
                    Castle.m_nTotalGold -= 1;
                    break;
                case '+':
                    Castle.m_nTotalGold += nGold;
                    break;
            }
            if (Castle.m_nTotalGold < 0)
            {
                Castle.m_nTotalGold = 0;
            }
        }

        public void CmdShowUseItemInfo(TGameCmd Cmd, string sHumanName)
        {
            TPlayObject PlayObject;
            TUserItem UserItem;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandShowUseItemInfoHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {
                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            for (var i = PlayObject.m_UseItems.GetLowerBound(0); i <= PlayObject.m_UseItems.GetUpperBound(0); i++)
            {
                UserItem = PlayObject.m_UseItems[i];
                if (UserItem.wIndex == 0)
                {
                    continue;
                }
                SysMsg(format("%s[%s]IDX[%d]系列号[%d]持久[%d-%d]", M2Share.GetUseItemName(i), M2Share.UserEngine.GetStdItemName(UserItem.wIndex), UserItem.wIndex, UserItem.MakeIndex, UserItem.Dura, UserItem.DuraMax), TMsgColor.c_Blue, TMsgType.t_Hint);
            }
        }

        public void CmdBindUseItem(TGameCmd Cmd, string sHumanName, string sItem, string sType)
        {
            int I;
            TPlayObject PlayObject;
            TUserItem UserItem;
            int nItem;
            int nBind;
            TItemBind ItemBind;
            int nItemIdx;
            int nMakeIdex;
            var sBindName = string.Empty;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nBind = -1;
            nItem = M2Share.GetUseItemIdx(sItem);
            if (string.Compare(sType, "帐号", StringComparison.Ordinal) == 0)
            {
                nBind = 0;
            }
            if (string.Compare(sType, "人物", StringComparison.Ordinal) == 0)
            {
                nBind = 1;
            }
            if (string.Compare(sType, "IP", StringComparison.Ordinal) == 0)
            {
                nBind = 2;
            }
            if (nItem < 0 || nBind < 0 || sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandBindUseItemHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            UserItem = PlayObject.m_UseItems[nItem];
            if (UserItem.wIndex == 0)
            {

                SysMsg(format(M2Share.g_sGameCommandBindUseItemNoItemMsg, sHumanName, sItem), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nItemIdx = UserItem.wIndex;
            nMakeIdex = UserItem.MakeIndex;
            switch (nBind)
            {
                case 0:
                    sBindName = PlayObject.m_sUserID;
                    try
                    {
                        for (I = 0; I < M2Share.g_ItemBindAccount.Count; I++)
                        {
                            ItemBind = M2Share.g_ItemBindAccount[I];
                            if (ItemBind.nItemIdx == nItemIdx && ItemBind.nMakeIdex == nMakeIdex)
                            {

                                SysMsg(format(M2Share.g_sGameCommandBindUseItemAlreadBindMsg, sHumanName, sItem), TMsgColor.c_Red, TMsgType.t_Hint);
                                return;
                            }
                        }
                        ItemBind = new TItemBind
                        {
                            nItemIdx = nItemIdx,
                            nMakeIdex = nMakeIdex,
                            sBindName = sBindName
                        };
                        M2Share.g_ItemBindAccount.Insert(0, ItemBind);
                    }
                    finally
                    {
                    }
                    M2Share.SaveItemBindAccount();

                    SysMsg(format("%s[%s]IDX[%d]系列号[%d]持久[%d-%d]，绑定到%s成功。", M2Share.GetUseItemName(nItem), M2Share.UserEngine.GetStdItemName(UserItem.wIndex), UserItem.wIndex, UserItem.MakeIndex, UserItem.Dura, UserItem.DuraMax, sBindName), TMsgColor.c_Blue, TMsgType.t_Hint);

                    PlayObject.SysMsg(format("你的%s[%s]已经绑定到%s[%s]上了。", M2Share.GetUseItemName(nItem), M2Share.UserEngine.GetStdItemName(UserItem.wIndex), sType, sBindName), TMsgColor.c_Blue, TMsgType.t_Hint);
                    break;
                case 1:
                    sBindName = PlayObject.m_sCharName;
                    try
                    {
                        for (I = 0; I < M2Share.g_ItemBindCharName.Count; I++)
                        {
                            ItemBind = M2Share.g_ItemBindCharName[I];
                            if (ItemBind.nItemIdx == nItemIdx && ItemBind.nMakeIdex == nMakeIdex)
                            {
                                SysMsg(format(M2Share.g_sGameCommandBindUseItemAlreadBindMsg, sHumanName, sItem), TMsgColor.c_Red, TMsgType.t_Hint);
                                return;
                            }
                        }
                        ItemBind = new TItemBind
                        {
                            nItemIdx = nItemIdx,
                            nMakeIdex = nMakeIdex,
                            sBindName = sBindName
                        };
                        M2Share.g_ItemBindCharName.Insert(0, ItemBind);
                    }
                    finally
                    {
                    }
                    M2Share.SaveItemBindCharName();

                    SysMsg(format("%s[%s]IDX[%d]系列号[%d]持久[%d-%d]，绑定到%s成功。", M2Share.GetUseItemName(nItem), M2Share.UserEngine.GetStdItemName(UserItem.wIndex), UserItem.wIndex, UserItem.MakeIndex, UserItem.Dura, UserItem.DuraMax, sBindName), TMsgColor.c_Blue, TMsgType.t_Hint);

                    PlayObject.SysMsg(format("你的%s[%s]已经绑定到%s[%s]上了。", M2Share.GetUseItemName(nItem), M2Share.UserEngine.GetStdItemName(UserItem.wIndex), sType, sBindName), TMsgColor.c_Blue, TMsgType.t_Hint);
                    break;
                case 2:
                    sBindName = PlayObject.m_sIPaddr;
                    try
                    {
                        for (I = 0; I < M2Share.g_ItemBindIPaddr.Count; I++)
                        {
                            ItemBind = M2Share.g_ItemBindIPaddr[I];
                            if (ItemBind.nItemIdx == nItemIdx && ItemBind.nMakeIdex == nMakeIdex)
                            {
                                SysMsg(format(M2Share.g_sGameCommandBindUseItemAlreadBindMsg, sHumanName, sItem), TMsgColor.c_Red, TMsgType.t_Hint);
                                return;
                            }
                        }
                        ItemBind = new TItemBind
                        {
                            nItemIdx = nItemIdx,
                            nMakeIdex = nMakeIdex,
                            sBindName = sBindName
                        };
                        M2Share.g_ItemBindIPaddr.Insert(0, ItemBind);
                    }
                    finally
                    {
                    }
                    M2Share.SaveItemBindIPaddr();
                    SysMsg(format("%s[%s]IDX[%d]系列号[%d]持久[%d-%d]，绑定到%s成功。", M2Share.GetUseItemName(nItem), M2Share.UserEngine.GetStdItemName(UserItem.wIndex), UserItem.wIndex, UserItem.MakeIndex, UserItem.Dura, UserItem.DuraMax, sBindName), TMsgColor.c_Blue, TMsgType.t_Hint);
                    PlayObject.SysMsg(format("你的%s[%s]已经绑定到%s[%s]上了。", M2Share.GetUseItemName(nItem), M2Share.UserEngine.GetStdItemName(UserItem.wIndex), sType, sBindName), TMsgColor.c_Blue, TMsgType.t_Hint);
                    break;
            }
        }

        public void CmdUnBindUseItem(TGameCmd Cmd, string sHumanName, string sItem, string sType)
        {
            int I;
            TPlayObject PlayObject;
            TUserItem UserItem;
            int nItem;
            int nBind;
            TItemBind ItemBind;
            int nItemIdx;
            int nMakeIdex;
            var sBindName = string.Empty;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nBind = -1;
            nItem = M2Share.GetUseItemIdx(sItem);
            if (string.Compare(sType.ToLower(), "帐号", StringComparison.Ordinal) == 0)
            {
                nBind = 0;
            }
            if (string.Compare(sType.ToLower(), "人物", StringComparison.Ordinal) == 0)
            {
                nBind = 1;
            }
            if (string.Compare(sType.ToLower(), "IP", StringComparison.Ordinal) == 0)
            {
                nBind = 2;
            }
            if (nItem < 0 || nBind < 0 || sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandBindUseItemHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            UserItem = PlayObject.m_UseItems[nItem];
            if (UserItem.wIndex == 0)
            {

                SysMsg(format(M2Share.g_sGameCommandBindUseItemNoItemMsg, sHumanName, sItem), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nItemIdx = UserItem.wIndex;
            nMakeIdex = UserItem.MakeIndex;
            switch (nBind)
            {
                case 0:
                    sBindName = PlayObject.m_sUserID;
                    try
                    {
                        for (I = 0; I < M2Share.g_ItemBindAccount.Count; I++)
                        {
                            ItemBind = M2Share.g_ItemBindAccount[I];
                            if (ItemBind.nItemIdx == nItemIdx && ItemBind.nMakeIdex == nMakeIdex)
                            {

                                SysMsg(format(M2Share.g_sGameCommandBindUseItemAlreadBindMsg, sHumanName, sItem), TMsgColor.c_Red, TMsgType.t_Hint);
                                return;
                            }
                        }
                        ItemBind = new TItemBind
                        {
                            nItemIdx = nItemIdx,
                            nMakeIdex = nMakeIdex,
                            sBindName = sBindName
                        };
                        M2Share.g_ItemBindAccount.Insert(0, ItemBind);
                    }
                    finally
                    {
                    }
                    M2Share.SaveItemBindAccount();
                    SysMsg(format("%s[%s]IDX[%d]系列号[%d]持久[%d-%d]，绑定到%s成功。", M2Share.GetUseItemName(nItem), M2Share.UserEngine.GetStdItemName(UserItem.wIndex), UserItem.wIndex, UserItem.MakeIndex, UserItem.Dura, UserItem.DuraMax, sBindName), TMsgColor.c_Blue, TMsgType.t_Hint);
                    PlayObject.SysMsg(format("你的%s[%s]已经绑定到%s[%s]上了。", M2Share.GetUseItemName(nItem), M2Share.UserEngine.GetStdItemName(UserItem.wIndex), sType, sBindName), TMsgColor.c_Blue, TMsgType.t_Hint);
                    break;
                case 1:
                    sBindName = PlayObject.m_sCharName;
                    try
                    {
                        for (I = 0; I < M2Share.g_ItemBindCharName.Count; I++)
                        {
                            ItemBind = M2Share.g_ItemBindCharName[I];
                            if (ItemBind.nItemIdx == nItemIdx && ItemBind.nMakeIdex == nMakeIdex)
                            {

                                SysMsg(format(M2Share.g_sGameCommandBindUseItemAlreadBindMsg, sHumanName, sItem), TMsgColor.c_Red, TMsgType.t_Hint);
                                return;
                            }
                        }
                        ItemBind = new TItemBind
                        {
                            nItemIdx = nItemIdx,
                            nMakeIdex = nMakeIdex,
                            sBindName = sBindName
                        };
                        M2Share.g_ItemBindCharName.Insert(0, ItemBind);
                    }
                    finally
                    {
                    }
                    M2Share.SaveItemBindCharName();
                    SysMsg(format("%s[%s]IDX[%d]系列号[%d]持久[%d-%d]，绑定到%s成功。", M2Share.GetUseItemName(nItem), M2Share.UserEngine.GetStdItemName(UserItem.wIndex), UserItem.wIndex, UserItem.MakeIndex, UserItem.Dura, UserItem.DuraMax, sBindName), TMsgColor.c_Blue, TMsgType.t_Hint);
                    PlayObject.SysMsg(format("你的%s[%s]已经绑定到%s[%s]上了。", M2Share.GetUseItemName(nItem), M2Share.UserEngine.GetStdItemName(UserItem.wIndex), sType, sBindName), TMsgColor.c_Blue, TMsgType.t_Hint);
                    break;
                case 2:
                    sBindName = PlayObject.m_sIPaddr;
                    try
                    {
                        for (I = 0; I < M2Share.g_ItemBindIPaddr.Count; I++)
                        {
                            ItemBind = M2Share.g_ItemBindIPaddr[I];
                            if (ItemBind.nItemIdx == nItemIdx && ItemBind.nMakeIdex == nMakeIdex)
                            {
                                SysMsg(format(M2Share.g_sGameCommandBindUseItemAlreadBindMsg, sHumanName, sItem), TMsgColor.c_Red, TMsgType.t_Hint);
                                return;
                            }
                        }
                        ItemBind = new TItemBind
                        {
                            nItemIdx = nItemIdx,
                            nMakeIdex = nMakeIdex,
                            sBindName = sBindName
                        };
                        M2Share.g_ItemBindIPaddr.Insert(0, ItemBind);
                    }
                    finally
                    {
                    }
                    M2Share.SaveItemBindIPaddr();

                    SysMsg(format("%s[%s]IDX[%d]系列号[%d]持久[%d-%d]，绑定到%s成功。", M2Share.GetUseItemName(nItem), M2Share.UserEngine.GetStdItemName(UserItem.wIndex), UserItem.wIndex, UserItem.MakeIndex, UserItem.Dura, UserItem.DuraMax, sBindName), TMsgColor.c_Blue, TMsgType.t_Hint);

                    PlayObject.SysMsg(format("你的%s[%s]已经绑定到%s[%s]上了。", M2Share.GetUseItemName(nItem), M2Share.UserEngine.GetStdItemName(UserItem.wIndex), sType, sBindName), TMsgColor.c_Blue, TMsgType.t_Hint);
                    break;
            }
        }

        public void CmdShutup(TGameCmd Cmd, string sHumanName, string sTime)
        {
            int dwTime;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sTime == "" || sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandShutupHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            dwTime = HUtil32.Str_ToInt(sTime, 5);
            //M2Share.g_DenySayMsgList.__Lock();
            //try
            //{
            //    nIndex = M2Share.g_DenySayMsgList.GetIndex(sHumanName);
            //    if (nIndex >= 0)
            //    {
            //        M2Share.g_DenySayMsgList[nIndex] = ((HUtil32.GetTickCount() + dwTime * 60 * 1000) as Object);
            //    }
            //    else
            //    {
            //        //M2Share.g_DenySayMsgList.AddRecord(sHumanName, HUtil32.GetTickCount() + dwTime * 60 * 1000);
            //    }
            //}
            //finally
            //{
            //    M2Share.g_DenySayMsgList.UnLock();
            //}
            SysMsg(format(M2Share.g_sGameCommandShutupHumanMsg, sHumanName, dwTime), TMsgColor.c_Red, TMsgType.t_Hint);
        }

        public void CmdShutupList(TGameCmd Cmd, string sParam1)
        {
            int I;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam1 != "" && sParam1[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (m_btPermission < 6)
            {
                return;
            }
            //M2Share.g_DenySayMsgList.__Lock();
            try
            {
                if (M2Share.g_DenySayMsgList.Count <= 0)
                {
                    SysMsg(M2Share.g_sGameCommandShutupListIsNullMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                    return;
                }
                for (I = 0; I < M2Share.g_DenySayMsgList.Count; I++)
                {
                    //this.SysMsg(M2Share.g_DenySayMsgList[I] + ' ' + ((((int)M2Share.g_DenySayMsgList.Values[I]) - HUtil32.GetTickCount()) / 60000).ToString(), TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            finally
            {
                // M2Share.g_DenySayMsgList.UnLock();
            }
        }

        public void CmdShutupRelease(TGameCmd Cmd, string sHumanName, bool boAll)
        {
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandShutupReleaseHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            //M2Share.g_DenySayMsgList.__Lock();
            try
            {
                //I = M2Share.g_DenySayMsgList.GetIndex(sHumanName);
                //if (I >= 0)
                //{
                //    M2Share.g_DenySayMsgList.Remove(I);
                //    PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
                //    if (PlayObject != null)
                //    {
                //        PlayObject.SysMsg(M2Share.g_sGameCommandShutupReleaseCanSendMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                //    }
                //    if (boAll)
                //    {
                //        M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_210, M2Share.nServerIndex, sHumanName);
                //    }

                //    this.SysMsg(format(M2Share.g_sGameCommandShutupReleaseHumanCanSendMsg, new string[] { sHumanName }), TMsgColor.c_Green, TMsgType.t_Hint);
                //}
            }
            finally
            {
                // M2Share.g_DenySayMsgList.UnLock();
            }
        }

        public void CmdSmakeItem(TGameCmd Cmd, int nWhere, int nValueType, int nValue)
        {
            string sShowMsg;
            MirItem StdItem;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (nWhere >= 0 && nWhere <= 12 && nValueType >= 0 && nValueType <= 15 && nValue >= 0 && nValue <= 255)
            {
                if (m_UseItems[nWhere].wIndex > 0)
                {
                    StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[nWhere].wIndex);
                    if (StdItem == null)
                    {
                        return;
                    }
                    if (nValueType > 13)
                    {
                        nValue = HUtil32._MIN(65, nValue);
                        if (nValueType == 14)
                        {
                            m_UseItems[nWhere].Dura = (ushort)(nValue * 1000);
                        }
                        if (nValueType == 15)
                        {
                            m_UseItems[nWhere].DuraMax = (ushort)(nValue * 1000);
                        }
                    }
                    else
                    {
                        m_UseItems[nWhere].btValue[nValueType] = (byte)nValue;
                    }
                    RecalcAbilitys();
                    SendUpdateItem(m_UseItems[nWhere]);
                    sShowMsg = m_UseItems[nWhere].wIndex.ToString() + '-' + m_UseItems[nWhere].MakeIndex + ' ' + m_UseItems[nWhere].Dura + '/' + m_UseItems[nWhere].DuraMax + ' ' + m_UseItems[nWhere].btValue[0] + '/' + m_UseItems[nWhere].btValue[1] + '/' + m_UseItems[nWhere].btValue[2] + '/' + m_UseItems[nWhere].btValue[3] + '/' + m_UseItems[nWhere].btValue[4] + '/' + m_UseItems[nWhere].btValue[5] + '/' + m_UseItems[nWhere].btValue[6] + '/' + m_UseItems[nWhere].btValue[7] + '/' + m_UseItems[nWhere].btValue[8] + '/' + m_UseItems[nWhere].btValue[9] + '/' + m_UseItems[nWhere].btValue[10] + '/' + m_UseItems[nWhere].btValue[11] + '/' + m_UseItems[nWhere].btValue[12] + '/' + m_UseItems[nWhere].btValue[13];
                    SysMsg(sShowMsg, TMsgColor.c_Blue, TMsgType.t_Hint);
                    if (M2Share.g_Config.boShowMakeItemMsg)
                    {
                        M2Share.MainOutMessage("[物品调整] " + m_sCharName + '(' + StdItem.Name + " -> " + sShowMsg + ')');
                    }
                }
                else
                {
                    SysMsg(M2Share.g_sGamecommandSuperMakeHelpMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
        }

        public void CmdSpirtStart(string sCmd, string sParam1)
        {
            int nTime;
            int dwTime;
            if (m_btPermission < 6)
            {
                return;
            }
            if (sParam1 != "" && sParam1[1] == '?')
            {
                SysMsg("此命令用于开始祈祷生效宝宝叛变。", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nTime = HUtil32.Str_ToInt(sParam1, -1);
            if (nTime > 0)
            {
                dwTime = nTime * 1000;
            }
            else
            {
                dwTime = M2Share.g_Config.dwSpiritMutinyTime;
            }

            M2Share.g_dwSpiritMutinyTick = HUtil32.GetTickCount() + dwTime;
            SysMsg("祈祷叛变已开始。持续时长 " + dwTime / 1000 + " 秒。", TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdSpirtStop(string sCmd, string sParam1)
        {
            if (m_btPermission < 6)
            {
                return;
            }
            if (sParam1 != "" && sParam1[1] == '?')
            {
                SysMsg("此命令用于停止祈祷生效导致宝宝叛变。", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            M2Share.g_dwSpiritMutinyTick = 0;
            SysMsg("祈祷叛变已停止。", TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdStartQuest(TGameCmd Cmd, string sQuestName)
        {
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sQuestName == "")
            {
                SysMsg("命令格式: @" + Cmd.sCmd + " 问答名称", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            M2Share.UserEngine.SendQuestMsg(sQuestName);
        }

        public void CmdSuperTing(TGameCmd Cmd, string sHumanName, string sRange)
        {
            TPlayObject PlayObject;
            TPlayObject MoveHuman;
            int nRange;
            IList<TBaseObject> HumanList;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sRange == "" || sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandSuperTingHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nRange = HUtil32._MAX(10, HUtil32.Str_ToInt(sRange, 2));
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject != null)
            {
                HumanList = new List<TBaseObject>();
                M2Share.UserEngine.GetMapRageHuman(PlayObject.m_PEnvir, PlayObject.m_nCurrX, PlayObject.m_nCurrY, nRange, HumanList);
                for (var i = 0; i < HumanList.Count; i++)
                {
                    MoveHuman = HumanList[i] as TPlayObject;
                    if (MoveHuman != this)
                    {
                        MoveHuman.MapRandomMove(MoveHuman.m_sHomeMap, 0);
                    }
                }
                //HumanList.Free;
            }
            else
            {
                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdTakeOffHorse(string sCmd, string sParam)
        {
            if (sParam != "" && sParam[1] == '?')
            {
                SysMsg("下马命令，在骑马状态输入此命令下马。", TMsgColor.c_Red, TMsgType.t_Hint);

                SysMsg(format("命令格式: @%s", sCmd), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (!m_boOnHorse)
            {
                return;
            }
            m_boOnHorse = false;
            FeatureChanged();
        }

        public void CmdTakeOnHorse(string sCmd, string sParam)
        {
            if (sParam != "" && sParam[1] == '?')
            {
                SysMsg("上马命令，在戴好马牌后输入此命令就可以骑上马。", TMsgColor.c_Red, TMsgType.t_Hint);

                SysMsg(format("命令格式: @%s", sCmd), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (m_boOnHorse)
            {
                return;
            }
            if (m_btHorseType == 0)
            {
                SysMsg("骑马必须先戴上马牌！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            m_boOnHorse = true;
            FeatureChanged();
        }

        public void CmdTestFire(string sCmd, int nRange, int nType, int nTime, int nPoint)
        {
            TFireBurnEvent FireBurnEvent;
            var nMinX = (short)(m_nCurrX - nRange);
            var nMaxX = (short)(m_nCurrX + nRange);
            var nMinY = (short)(m_nCurrY - nRange);
            var nMaxY = (short)(m_nCurrY + nRange);
            for (var nX = nMinX; nX <= nMaxX; nX++)
            {
                for (var nY = nMinY; nY <= nMaxY; nY++)
                {
                    if (nX < nMaxX && nY == nMinY || nY < nMaxY && nX == nMinX || nX == nMaxX || nY == nMaxY)
                    {
                        FireBurnEvent = new TFireBurnEvent(this, nX, nY, nType, nTime * 1000, nPoint);
                        M2Share.EventManager.AddEvent(FireBurnEvent);
                    }
                }
            }
        }

        public void CmdTestGetBagItems(TGameCmd Cmd, string sParam)
        {
            byte btDc;
            byte btSc;
            byte btMc;
            byte btDura;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam != "" && sParam[1] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandTestGetBagItemsHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            btDc = 0;
            btSc = 0;
            btMc = 0;
            btDura = 0;
            // GetBagUseItems(btDc,btSc,btMc,btDura);   //注释掉无用
            SysMsg(format("DC:%d SC:%d MC:%d DURA:%d", new byte[] { btDc, btSc, btMc, btDura }), TMsgColor.c_Blue, TMsgType.t_Hint);
        }

        public void CmdTestSpeedMode(TGameCmd Cmd)
        {
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            m_boTestSpeedMode = !m_boTestSpeedMode;
            if (m_boTestSpeedMode)
            {
                SysMsg("开启速度测试模式", TMsgColor.c_Red, TMsgType.t_Hint);
            }
            else
            {
                SysMsg("关闭速度测试模式", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdTestStatus(string sCmd, int nType, int nTime)
        {
            if (m_btPermission < 6)
            {
                return;
            }
            //if ((!(nType >= grobal2.short.GetLowerBound(0) && nType <= grobal2.short.GetUpperBound(0))) || (nTime < 0))
            //{
            //    this.SysMsg("命令格式: @" + sCmd + " 类型(0..11) 时长", TMsgColor.c_Red, TMsgType.t_Hint);
            //    return;
            //}
            m_wStatusTimeArr[nType] = (ushort)(nTime * 1000);
            m_dwStatusArrTick[nType] = HUtil32.GetTickCount();
            m_nCharStatus = GetCharStatus();
            StatusChanged();
            SysMsg(format("状态编号:%d 时间长度: %d 秒", nType, nTime), TMsgColor.c_Green, TMsgType.t_Hint);
        }

        public void CmdTing(TGameCmd Cmd, string sHumanName)
        {
            TPlayObject PlayObject;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {
                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandTingHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject != null)
            {
                PlayObject.MapRandomMove(m_sHomeMap, 0);
            }
            else
            {
                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdTraining(string sSkillName, int nLevel)
        {
            if (m_btPermission < 6)
            {
                return;
            }
        }

        public void CmdUserMoveXY(string sCMD, string sX, string sY)
        {
            short nX = 0;
            short nY = 0;
            if (m_boTeleport)
            {
                nX = (short)HUtil32.Str_ToInt(sX, -1);
                nY = (short)HUtil32.Str_ToInt(sY, -1);
                if (!m_PEnvir.Flag.boNOPOSITIONMOVE)
                {
                    if (m_PEnvir.CanWalkOfItem(nX, nY, M2Share.g_Config.boUserMoveCanDupObj, M2Share.g_Config.boUserMoveCanOnItem))
                    {
                        // 10000
                        if (HUtil32.GetTickCount() - m_dwTeleportTick > M2Share.g_Config.dwUserMoveTime * 1000)
                        {
                            m_dwTeleportTick = HUtil32.GetTickCount();
                            SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                            // BaseObjectMove('',sX,sY);
                            SpaceMove(m_sMapName, nX, nY, 0);
                        }
                        else
                        {
                            SysMsg(M2Share.g_Config.dwUserMoveTime - (HUtil32.GetTickCount() - m_dwTeleportTick) / 1000 + "秒之后才可以再使用此功能！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                    }
                    else
                    {
                        SysMsg(format(M2Share.g_sGameCommandPositionMoveCanotMoveToMap, m_sMapName, sX, sY), TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                }
                else
                {
                    SysMsg("此地图禁止使用此命令！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
            else
            {
                SysMsg("您现在还无法使用此功能！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void CmdViewDiary(string sCMD, int nFlag)
        {
            // 004D1B70
        }

        public void CmdViewWhisper(TGameCmd Cmd, string sCharName, string sParam2)
        {
            TPlayObject PlayObject;
            if (m_btPermission < Cmd.nPerMissionMin)
            {
                SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sCharName == "" || sCharName != "" && sCharName[1] == '?')
            {

                SysMsg(format(M2Share.g_sGameCommandParamUnKnow, Cmd.sCmd, M2Share.g_sGameCommandViewWhisperHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sCharName);
            if (PlayObject != null)
            {
                if (PlayObject.m_GetWhisperHuman == this)
                {
                    PlayObject.m_GetWhisperHuman = null;

                    SysMsg(format(M2Share.g_sGameCommandViewWhisperMsg1, sCharName), TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    PlayObject.m_GetWhisperHuman = this;

                    SysMsg(format(M2Share.g_sGameCommandViewWhisperMsg2, sCharName), TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            else
            {

                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sCharName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }
    }
}
