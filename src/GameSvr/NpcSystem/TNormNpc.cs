using SystemModule;
using System;
using System.Collections.Generic;

namespace GameSvr
{
    /// <summary>
    /// 管理类NPC
    /// 如 月老
    /// </summary>
    public partial class TNormNpc : TAnimalObject
    {
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
        private IList<TScriptParams> BatchParamsList;

        public virtual void ClearScript()
        {
            m_ScriptList.Clear();
        }

        public virtual void Click(TPlayObject PlayObject)
        {
            PlayObject.m_nScriptGotoCount = 0;
            PlayObject.m_sScriptGoBackLable = "";
            PlayObject.m_sScriptCurrLable = "";
            GotoLable(PlayObject, "@main", false);
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
            if (string.Compare(sParam1, "CHANGEEXP", StringComparison.OrdinalIgnoreCase) == 0)
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
            if (string.Compare(sParam1, "CHANGELEVEL", StringComparison.OrdinalIgnoreCase) == 0)
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
            if (string.Compare(sParam1, "KILL", StringComparison.OrdinalIgnoreCase) == 0)
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
            if (string.Compare(sParam1, "KICK", StringComparison.OrdinalIgnoreCase) == 0)
            {
                PlayObject.m_boKickFlag = true;
                return;
            }
        }

        public string GetLineVariableText(TPlayObject PlayObject, string sMsg)
        {
            var nC = 0;
            var s10 = string.Empty;
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
            return sMsg;
        }

        /// <summary>
        /// 获取全局变量信息
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sMsg"></param>
        /// <param name="sVariable"></param>
        protected virtual void GetVariableText(TPlayObject PlayObject, ref string sMsg, string sVariable)
        {
            string sText = string.Empty;
            string s14 = string.Empty;
            TDynamicVar DynamicVar;
            bool boFoundVar;
            switch (sVariable)
            {
                case "$SERVERNAME":
                    sMsg = ReplaceVariableText(sMsg, "<$SERVERNAME>", M2Share.g_Config.sServerName);
                    return;
                case "$SERVERIP":
                    sMsg = ReplaceVariableText(sMsg, "<$SERVERIP>", M2Share.g_Config.sServerIPaddr);
                    return;
                case "$WEBSITE":
                    sMsg = ReplaceVariableText(sMsg, "<$WEBSITE>", M2Share.g_Config.sWebSite);
                    return;
                case "$BBSSITE":
                    sMsg = ReplaceVariableText(sMsg, "<$BBSSITE>", M2Share.g_Config.sBbsSite);
                    return;
                case "$CLIENTDOWNLOAD":
                    sMsg = ReplaceVariableText(sMsg, "<$CLIENTDOWNLOAD>", M2Share.g_Config.sClientDownload);
                    return;
                case "$QQ":
                    sMsg = ReplaceVariableText(sMsg, "<$QQ>", M2Share.g_Config.sQQ);
                    return;
                case "$PHONE":
                    sMsg = ReplaceVariableText(sMsg, "<$PHONE>", M2Share.g_Config.sPhone);
                    return;
                case "$BANKACCOUNT0":
                    sMsg = ReplaceVariableText(sMsg, "<$BANKACCOUNT0>", M2Share.g_Config.sBankAccount0);
                    return;
                case "$BANKACCOUNT1":
                    sMsg = ReplaceVariableText(sMsg, "<$BANKACCOUNT1>", M2Share.g_Config.sBankAccount1);
                    return;
                case "$BANKACCOUNT2":
                    sMsg = ReplaceVariableText(sMsg, "<$BANKACCOUNT2>", M2Share.g_Config.sBankAccount2);
                    return;
                case "$BANKACCOUNT3":
                    sMsg = ReplaceVariableText(sMsg, "<$BANKACCOUNT3>", M2Share.g_Config.sBankAccount3);
                    return;
                case "$BANKACCOUNT4":
                    sMsg = ReplaceVariableText(sMsg, "<$BANKACCOUNT4>", M2Share.g_Config.sBankAccount4);
                    return;
                case "$BANKACCOUNT5":
                    sMsg = ReplaceVariableText(sMsg, "<$BANKACCOUNT5>", M2Share.g_Config.sBankAccount5);
                    return;
                case "$BANKACCOUNT6":
                    sMsg = ReplaceVariableText(sMsg, "<$BANKACCOUNT6>", M2Share.g_Config.sBankAccount6);
                    return;
                case "$BANKACCOUNT7":
                    sMsg = ReplaceVariableText(sMsg, "<$BANKACCOUNT7>", M2Share.g_Config.sBankAccount7);
                    return;
                case "$BANKACCOUNT8":
                    sMsg = ReplaceVariableText(sMsg, "<$BANKACCOUNT8>", M2Share.g_Config.sBankAccount8);
                    return;
                case "$BANKACCOUNT9":
                    sMsg = ReplaceVariableText(sMsg, "<$BANKACCOUNT9>", M2Share.g_Config.sBankAccount9);
                    return;
                case "$GAMEGOLDNAME":
                    sMsg = ReplaceVariableText(sMsg, "<$GAMEGOLDNAME>", M2Share.g_Config.sGameGoldName);
                    return;
                case "$GAMEPOINTNAME":
                    sMsg = ReplaceVariableText(sMsg, "<$GAMEPOINTNAME>", M2Share.g_Config.sGamePointName);
                    return;
                case "$USERCOUNT":
                    sText = M2Share.UserEngine.PlayObjectCount.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$USERCOUNT>", sText);
                    return;
                case "$MACRUNTIME":
                    sText = (HUtil32.GetTickCount() / (24 * 60 * 60 * 1000)).ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$MACRUNTIME>", sText);
                    return;
                case "$SERVERRUNTIME":
                    var nSecond = (HUtil32.GetTickCount() - M2Share.g_dwStartTick) / 1000;
                    var wHour = nSecond / 3600;
                    var wMinute = (nSecond / 60) % 60;
                    var wSecond = nSecond % 60;
                    sText = format("%d:%d:%d", wHour, wMinute, wSecond);
                    sMsg = ReplaceVariableText(sMsg, "<$SERVERRUNTIME>", sText);
                    return;
                case "$DATETIME":
                    sText = DateTime.Now.ToString("dddddd,dddd,hh:mm:nn");
                    sMsg = ReplaceVariableText(sMsg, "<$DATETIME>", sText);
                    return;
                case "$HIGHLEVELINFO":
                    {
                        if (M2Share.g_HighLevelHuman != null)
                        {
                            sText = ((TPlayObject)M2Share.g_HighLevelHuman).GetMyInfo();
                        }
                        else
                        {
                            sText = "????";
                        }
                        sMsg = ReplaceVariableText(sMsg, "<$HIGHLEVELINFO>", sText);
                        return;
                    }
                case "$HIGHPKINFO":
                    {
                        if (M2Share.g_HighPKPointHuman != null)
                        {
                            sText = ((TPlayObject)M2Share.g_HighPKPointHuman).GetMyInfo();
                        }
                        else
                        {
                            sText = "????";
                        }
                        sMsg = ReplaceVariableText(sMsg, "<$HIGHPKINFO>", sText);
                        return;
                    }
                case "$HIGHDCINFO":
                    {
                        if (M2Share.g_HighDCHuman != null)
                        {
                            sText = ((TPlayObject)M2Share.g_HighDCHuman).GetMyInfo();
                        }
                        else
                        {
                            sText = "????";
                        }
                        sMsg = ReplaceVariableText(sMsg, "<$HIGHDCINFO>", sText);
                        return;
                    }
                case "$HIGHMCINFO":
                    {
                        if (M2Share.g_HighMCHuman != null)
                        {
                            sText = ((TPlayObject)M2Share.g_HighMCHuman).GetMyInfo();
                        }
                        else
                        {
                            sText = "????";
                        }
                        sMsg = ReplaceVariableText(sMsg, "<$HIGHMCINFO>", sText);
                        return;
                    }
                case "$HIGHSCINFO":
                    {
                        if (M2Share.g_HighSCHuman != null)
                        {
                            sText = ((TPlayObject)M2Share.g_HighSCHuman).GetMyInfo();
                        }
                        else
                        {
                            sText = "????";
                        }
                        sMsg = ReplaceVariableText(sMsg, "<$HIGHSCINFO>", sText);
                        return;
                    }
                case "$HIGHONLINEINFO":
                    {
                        if (M2Share.g_HighOnlineHuman != null)
                        {
                            sText = ((TPlayObject)M2Share.g_HighOnlineHuman).GetMyInfo();
                        }
                        else
                        {
                            sText = "????";
                        }
                        sMsg = ReplaceVariableText(sMsg, "<$HIGHONLINEINFO>", sText);
                        return;
                    }
                case "$RANDOMNO":
                    sMsg = ReplaceVariableText(sMsg, "<$RANDOMNO>", PlayObject.m_sRandomNo);
                    return;
                case "$RELEVEL":
                    sText = PlayObject.m_btReLevel.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$RELEVEL>", sText);
                    return;
                case "$HUMANSHOWNAME":
                    sMsg = ReplaceVariableText(sMsg, "<$HUMANSHOWNAME>", PlayObject.GetShowName());
                    return;
                case "$MONKILLER":
                    {
                        if (PlayObject.m_LastHiter != null)
                        {
                            if (PlayObject.m_LastHiter.m_btRaceServer != Grobal2.RC_PLAYOBJECT)
                            {
                                sMsg = ReplaceVariableText(sMsg, "<$MONKILLER>", PlayObject.m_LastHiter.m_sCharName);
                            }
                        }
                        else
                        {
                            sMsg = ReplaceVariableText(sMsg, "<$MONKILLER>", "未知");
                        }
                        return;
                    }
                case "$QUERYYBDEALLOG":// 查看元宝交易记录 
                    {
                        sMsg = ReplaceVariableText(sMsg, "<$QUERYYBDEALLOG>", PlayObject.SelectSellDate());
                        return;
                    }
                case "$KILLER":
                    {
                        if (PlayObject.m_LastHiter != null)
                        {
                            if (PlayObject.m_LastHiter.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                            {
                                sMsg = ReplaceVariableText(sMsg, "<$KILLER>", PlayObject.m_LastHiter.m_sCharName);
                            }
                        }
                        else
                        {
                            sMsg = ReplaceVariableText(sMsg, "<$KILLER>", "未知");
                        }
                        return;
                    }
                case "$USERNAME":
                    sMsg = ReplaceVariableText(sMsg, "<$USERNAME>", PlayObject.m_sCharName);
                    return;
                case "$GUILDNAME":
                    {
                        if (PlayObject.m_MyGuild != null)
                        {
                            sMsg = ReplaceVariableText(sMsg, "<$GUILDNAME>", PlayObject.m_MyGuild.sGuildName);
                        }
                        else
                        {
                            sMsg = "无";
                        }
                        return;
                    }
                case "$RANKNAME":
                    sMsg = ReplaceVariableText(sMsg, "<$RANKNAME>", PlayObject.m_sGuildRankName);
                    return;
                case "$LEVEL":
                    sText = PlayObject.m_Abil.Level.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$LEVEL>", sText);
                    return;
                case "$HP":
                    sText = PlayObject.m_WAbil.HP.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$HP>", sText);
                    return;
                case "$MAXHP":
                    sText = PlayObject.m_WAbil.MaxHP.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$MAXHP>", sText);
                    return;
                case "$MP":
                    sText = PlayObject.m_WAbil.MP.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$MP>", sText);
                    return;
                case "$MAXMP":
                    sText = PlayObject.m_WAbil.MaxMP.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$MAXMP>", sText);
                    return;
                case "$AC":
                    sText = HUtil32.LoWord(PlayObject.m_WAbil.AC).ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$AC>", sText);
                    return;
                case "$MAXAC":
                    sText = HUtil32.HiWord(PlayObject.m_WAbil.AC).ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$MAXAC>", sText);
                    return;
                case "$MAC":
                    sText = HUtil32.LoWord(PlayObject.m_WAbil.MAC).ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$MAC>", sText);
                    return;
                case "$MAXMAC":
                    sText = HUtil32.HiWord(PlayObject.m_WAbil.MAC).ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$MAXMAC>", sText);
                    return;
                case "$DC":
                    sText = HUtil32.LoWord(PlayObject.m_WAbil.DC).ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$DC>", sText);
                    return;
                case "$MAXDC":
                    sText = HUtil32.HiWord(PlayObject.m_WAbil.DC).ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$MAXDC>", sText);
                    return;
                case "$MC":
                    sText = HUtil32.LoWord(PlayObject.m_WAbil.MC).ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$MC>", sText);
                    return;
                case "$MAXMC":
                    sText = HUtil32.HiWord(PlayObject.m_WAbil.MC).ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$MAXMC>", sText);
                    return;
                case "$SC":
                    sText = HUtil32.LoWord(PlayObject.m_WAbil.SC).ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$SC>", sText);
                    return;
                case "$MAXSC":
                    sText = HUtil32.HiWord(PlayObject.m_WAbil.SC).ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$MAXSC>", sText);
                    return;
                case "$EXP":
                    sText = PlayObject.m_Abil.Exp.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$EXP>", sText);
                    return;
                case "$MAXEXP":
                    sText = PlayObject.m_Abil.MaxExp.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$MAXEXP>", sText);
                    return;
                case "$PKPOINT":
                    sText = PlayObject.m_nPkPoint.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$PKPOINT>", sText);
                    return;
                case "$CREDITPOINT":
                    sText = PlayObject.m_btCreditPoint.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$CREDITPOINT>", sText);
                    return;
                case "$HW":
                    sText = PlayObject.m_WAbil.HandWeight.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$HW>", sText);
                    return;
                case "$MAXHW":
                    sText = PlayObject.m_WAbil.MaxHandWeight.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$MAXHW>", sText);
                    return;
                case "$BW":
                    sText = PlayObject.m_WAbil.Weight.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$BW>", sText);
                    return;
                case "$MAXBW":
                    sText = PlayObject.m_WAbil.MaxWeight.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$MAXBW>", sText);
                    return;
                case "$WW":
                    sText = PlayObject.m_WAbil.WearWeight.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$WW>", sText);
                    return;
                case "$MAXWW":
                    sText = PlayObject.m_WAbil.MaxWearWeight.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$MAXWW>", sText);
                    return;
                case "$GOLDCOUNT":
                    sText = PlayObject.m_nGold.ToString() + '/' + PlayObject.m_nGoldMax;
                    sMsg = ReplaceVariableText(sMsg, "<$GOLDCOUNT>", sText);
                    return;
                case "$GAMEGOLD":
                    sText = PlayObject.m_nGameGold.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$GAMEGOLD>", sText);
                    return;
                case "$GAMEPOINT":
                    sText = PlayObject.m_nGamePoint.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$GAMEPOINT>", sText);
                    return;
                case "$HUNGER":
                    sText = PlayObject.GetMyStatus().ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$HUNGER>", sText);
                    return;
                case "$LOGINTIME":
                    sText = PlayObject.m_dLogonTime.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$LOGINTIME>", sText);
                    return;
                case "$LOGINLONG":
                    sText = ((HUtil32.GetTickCount() - PlayObject.m_dwLogonTick) / 60000) + "分钟";
                    sMsg = ReplaceVariableText(sMsg, "<$LOGINLONG>", sText);
                    return;
                case "$DRESS":
                    sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_DRESS].wIndex);
                    sMsg = ReplaceVariableText(sMsg, "<$DRESS>", sText);
                    return;
                case "$WEAPON":
                    sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_WEAPON].wIndex);
                    sMsg = ReplaceVariableText(sMsg, "<$WEAPON>", sText);
                    return;
                case "$RIGHTHAND":
                    sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_RIGHTHAND].wIndex);
                    sMsg = ReplaceVariableText(sMsg, "<$RIGHTHAND>", sText);
                    return;
                case "$HELMET":
                    sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_HELMET].wIndex);
                    sMsg = ReplaceVariableText(sMsg, "<$HELMET>", sText);
                    return;
                case "$NECKLACE":
                    sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_NECKLACE].wIndex);
                    sMsg = ReplaceVariableText(sMsg, "<$NECKLACE>", sText);
                    return;
                case "$RING_R":
                    sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_RINGR].wIndex);
                    sMsg = ReplaceVariableText(sMsg, "<$RING_R>", sText);
                    return;
                case "$RING_L":
                    sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_RINGL].wIndex);
                    sMsg = ReplaceVariableText(sMsg, "<$RING_L>", sText);
                    return;
                case "$ARMRING_R":
                    sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_ARMRINGR].wIndex);
                    sMsg = ReplaceVariableText(sMsg, "<$ARMRING_R>", sText);
                    return;
                case "$ARMRING_L":
                    sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_ARMRINGL].wIndex);
                    sMsg = ReplaceVariableText(sMsg, "<$ARMRING_L>", sText);
                    return;
                case "$BUJUK":
                    sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_BUJUK].wIndex);
                    sMsg = ReplaceVariableText(sMsg, "<$BUJUK>", sText);
                    return;
                case "$BELT":
                    sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_BELT].wIndex);
                    sMsg = ReplaceVariableText(sMsg, "<$BELT>", sText);
                    return;
                case "$BOOTS":
                    sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_BOOTS].wIndex);
                    sMsg = ReplaceVariableText(sMsg, "<$BOOTS>", sText);
                    return;
                case "$CHARM":
                    sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_CHARM].wIndex);
                    sMsg = ReplaceVariableText(sMsg, "<$CHARM>", sText);
                    return;
                case "$IPADDR":
                    sText = PlayObject.m_sIPaddr;
                    sMsg = ReplaceVariableText(sMsg, "<$IPADDR>", sText);
                    return;
                case "$IPLOCAL":
                    sText = PlayObject.m_sIPLocal;
                    // GetIPLocal(PlayObject.m_sIPaddr);
                    sMsg = ReplaceVariableText(sMsg, "<$IPLOCAL>", sText);
                    return;
                case "$GUILDBUILDPOINT":
                    {
                        if (PlayObject.m_MyGuild == null)
                        {
                            sText = "无";
                        }
                        else
                        {
                            sText = PlayObject.m_MyGuild.BuildPoint.ToString();
                        }
                        sMsg = ReplaceVariableText(sMsg, "<$GUILDBUILDPOINT>", sText);
                        return;
                    }
                case "$GUILDAURAEPOINT":
                    {
                        if (PlayObject.m_MyGuild == null)
                        {
                            sText = "无";
                        }
                        else
                        {
                            sText = PlayObject.m_MyGuild.nAurae.ToString();
                        }
                        sMsg = ReplaceVariableText(sMsg, "<$GUILDAURAEPOINT>", sText);
                        return;
                    }
                case "$GUILDSTABILITYPOINT":
                    {
                        if (PlayObject.m_MyGuild == null)
                        {
                            sText = "无";
                        }
                        else
                        {
                            sText = PlayObject.m_MyGuild.nStability.ToString();
                        }
                        sMsg = ReplaceVariableText(sMsg, "<$GUILDSTABILITYPOINT>", sText);
                        return;
                    }
                case "$GUILDFLOURISHPOINT":
                    {
                        if (PlayObject.m_MyGuild == null)
                        {
                            sText = "无";
                        }
                        else
                        {
                            sText = PlayObject.m_MyGuild.nFlourishing.ToString();
                        }
                        sMsg = ReplaceVariableText(sMsg, "<$GUILDFLOURISHPOINT>", sText);
                        return;
                    }
                // 其它信息
                case "$REQUESTCASTLEWARITEM":
                    sText = M2Share.g_Config.sZumaPiece;
                    sMsg = ReplaceVariableText(sMsg, "<$REQUESTCASTLEWARITEM>", sText);
                    return;
                case "$REQUESTCASTLEWARDAY":
                    sText = M2Share.g_Config.sZumaPiece;
                    sMsg = ReplaceVariableText(sMsg, "<$REQUESTCASTLEWARDAY>", sText);
                    return;
                case "$REQUESTBUILDGUILDITEM":
                    sText = M2Share.g_Config.sWomaHorn;
                    sMsg = ReplaceVariableText(sMsg, "<$REQUESTBUILDGUILDITEM>", sText);
                    return;
                case "$OWNERGUILD":
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
                        sMsg = ReplaceVariableText(sMsg, "<$OWNERGUILD>", sText);
                        return;
                    }
                case "$CASTLENAME":
                    {
                        if (this.m_Castle != null)
                        {
                            sText = this.m_Castle.m_sName;
                        }
                        else
                        {
                            sText = "????";
                        }
                        sMsg = ReplaceVariableText(sMsg, "<$CASTLENAME>", sText);
                        return;
                    }
                case "$LORD":
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
                        sMsg = ReplaceVariableText(sMsg, "<$LORD>", sText);
                        return;
                    }
                case "$GUILDWARFEE":
                    sMsg = ReplaceVariableText(sMsg, "<$GUILDWARFEE>", M2Share.g_Config.nGuildWarPrice.ToString());
                    return;
                case "$BUILDGUILDFEE":
                    sMsg = ReplaceVariableText(sMsg, "<$BUILDGUILDFEE>", M2Share.g_Config.nBuildGuildPrice.ToString());
                    return;
                case "$CASTLEWARDATE":
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
                                    sMsg = ReplaceVariableText(sMsg, "<$CASTLEWARDATE>", sText);
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
                case "$LISTOFWAR":
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
                            sMsg = ReplaceVariableText(sMsg, "<$LISTOFWAR>", sText);
                        }
                        else
                        {
                            sMsg = "We have no schedule...\\ \\<back/@main>";
                        }
                        return;
                    }
                case "$CASTLECHANGEDATE":
                    {
                        if (this.m_Castle != null)
                        {
                            sText = this.m_Castle.m_ChangeDate.ToString();
                        }
                        else
                        {
                            sText = "????";
                        }
                        sMsg = ReplaceVariableText(sMsg, "<$CASTLECHANGEDATE>", sText);
                        return;
                    }
                case "$CASTLEWARLASTDATE":
                    {
                        if (this.m_Castle != null)
                        {
                            sText = this.m_Castle.m_WarDate.ToString();
                        }
                        else
                        {
                            sText = "????";
                        }
                        sMsg = ReplaceVariableText(sMsg, "<$CASTLEWARLASTDATE>", sText);
                        return;
                    }
                case "$CASTLEGETDAYS":
                    {
                        if (this.m_Castle != null)
                        {
                            sText = HUtil32.GetDayCount(DateTime.Now, this.m_Castle.m_ChangeDate).ToString();
                        }
                        else
                        {
                            sText = "????";
                        }
                        sMsg = ReplaceVariableText(sMsg, "<$CASTLEGETDAYS>", sText);
                        return;
                    }
                case "$CMD_DATE":
                    sMsg = ReplaceVariableText(sMsg, "<$CMD_DATE>", M2Share.g_GameCommand.DATA.sCmd);
                    return;
                case "$CMD_ALLOWMSG":
                    sMsg = ReplaceVariableText(sMsg, "<$CMD_ALLOWMSG>", M2Share.g_GameCommand.ALLOWMSG.sCmd);
                    return;
                case "$CMD_LETSHOUT":
                    sMsg = ReplaceVariableText(sMsg, "<$CMD_LETSHOUT>", M2Share.g_GameCommand.LETSHOUT.sCmd);
                    return;
                case "$CMD_LETTRADE":
                    sMsg = ReplaceVariableText(sMsg, "<$CMD_LETTRADE>", M2Share.g_GameCommand.LETTRADE.sCmd);
                    return;
                case "$CMD_LETGUILD":
                    sMsg = ReplaceVariableText(sMsg, "<$CMD_LETGUILD>", M2Share.g_GameCommand.LETGUILD.sCmd);
                    return;
                case "$CMD_ENDGUILD":
                    sMsg = ReplaceVariableText(sMsg, "<$CMD_ENDGUILD>", M2Share.g_GameCommand.ENDGUILD.sCmd);
                    return;
                case "$CMD_BANGUILDCHAT":
                    sMsg = ReplaceVariableText(sMsg, "<$CMD_BANGUILDCHAT>", M2Share.g_GameCommand.BANGUILDCHAT.sCmd);
                    return;
                case "$CMD_AUTHALLY":
                    sMsg = ReplaceVariableText(sMsg, "<$CMD_AUTHALLY>", M2Share.g_GameCommand.AUTHALLY.sCmd);
                    return;
                case "$CMD_AUTH":
                    sMsg = ReplaceVariableText(sMsg, "<$CMD_AUTH>", M2Share.g_GameCommand.AUTH.sCmd);
                    return;
                case "$CMD_AUTHCANCEL":
                    sMsg = ReplaceVariableText(sMsg, "<$CMD_AUTHCANCEL>", M2Share.g_GameCommand.AUTHCANCEL.sCmd);
                    return;
                case "$CMD_USERMOVE":
                    sMsg = ReplaceVariableText(sMsg, "<$CMD_USERMOVE>", M2Share.g_GameCommand.USERMOVE.sCmd);
                    return;
                case "$CMD_SEARCHING":
                    sMsg = ReplaceVariableText(sMsg, "<$CMD_SEARCHING>", M2Share.g_GameCommand.SEARCHING.sCmd);
                    return;
                case "$CMD_ALLOWGROUPCALL":
                    sMsg = ReplaceVariableText(sMsg, "<$CMD_ALLOWGROUPCALL>", M2Share.g_GameCommand.ALLOWGROUPCALL.sCmd);
                    return;
                case "$CMD_GROUPRECALLL":
                    sMsg = ReplaceVariableText(sMsg, "<$CMD_GROUPRECALLL>", M2Share.g_GameCommand.GROUPRECALLL.sCmd);
                    return;
                case "$CMD_ATTACKMODE":
                    sMsg = ReplaceVariableText(sMsg, "<$CMD_ATTACKMODE>", M2Share.g_GameCommand.ATTACKMODE.sCmd);
                    return;
                case "$CMD_REST":
                    sMsg = ReplaceVariableText(sMsg, "<$CMD_REST>", M2Share.g_GameCommand.REST.sCmd);
                    return;
                case "$CMD_STORAGESETPASSWORD":
                    sMsg = ReplaceVariableText(sMsg, "<$CMD_STORAGESETPASSWORD>", M2Share.g_GameCommand.SETPASSWORD.sCmd);
                    return;
                case "$CMD_STORAGECHGPASSWORD":
                    sMsg = ReplaceVariableText(sMsg, "<$CMD_STORAGECHGPASSWORD>", M2Share.g_GameCommand.CHGPASSWORD.sCmd);
                    return;
                case "$CMD_STORAGELOCK":
                    sMsg = ReplaceVariableText(sMsg, "<$CMD_STORAGELOCK>", M2Share.g_GameCommand.__LOCK.sCmd);
                    return;
                case "$CMD_STORAGEUNLOCK":
                    sMsg = ReplaceVariableText(sMsg, "<$CMD_STORAGEUNLOCK>", M2Share.g_GameCommand.UNLOCKSTORAGE.sCmd);
                    return;
                case "$CMD_UNLOCK":
                    sMsg = ReplaceVariableText(sMsg, "<$CMD_UNLOCK>", M2Share.g_GameCommand.UNLOCK.sCmd);
                    return;
            }
            if (HUtil32.CompareLStr(sVariable, "$HUMAN(", "$HUMAN(".Length))
            {
                HUtil32.ArrestStringEx(sVariable, '(', ')', ref s14);
                boFoundVar = false;
                for (var i = 0; i < PlayObject.m_DynamicVarList.Count; i++)
                {
                    DynamicVar = PlayObject.m_DynamicVarList[i];
                    if (string.Compare(DynamicVar.sName, s14, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        switch (DynamicVar.VarType)
                        {
                            case TVarType.VInteger:
                                sMsg = ReplaceVariableText(sMsg, '<' + sVariable + '>', DynamicVar.nInternet.ToString());
                                boFoundVar = true;
                                break;
                            case TVarType.VString:
                                sMsg = ReplaceVariableText(sMsg, '<' + sVariable + '>', DynamicVar.sString);
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
                for (var i = 0; i < PlayObject.m_MyGuild.m_DynamicVarList.Count; i++)
                {
                    DynamicVar = PlayObject.m_MyGuild.m_DynamicVarList[i];
                    if (String.Compare(DynamicVar.sName, s14, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        switch (DynamicVar.VarType)
                        {
                            case TVarType.VInteger:
                                sMsg = ReplaceVariableText(sMsg, '<' + sVariable + '>', DynamicVar.nInternet.ToString());
                                boFoundVar = true;
                                break;
                            case TVarType.VString:
                                sMsg = ReplaceVariableText(sMsg, '<' + sVariable + '>', DynamicVar.sString);
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
                for (var i = 0; i < M2Share.g_DynamicVarList.Count; i++)
                {
                    DynamicVar = M2Share.g_DynamicVarList[i];
                    if (String.Compare(DynamicVar.sName, s14, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        switch (DynamicVar.VarType)
                        {
                            case TVarType.VInteger:
                                sMsg = ReplaceVariableText(sMsg, '<' + sVariable + '>', DynamicVar.nInternet.ToString());
                                boFoundVar = true;
                                break;
                            case TVarType.VString:
                                sMsg = ReplaceVariableText(sMsg, '<' + sVariable + '>', DynamicVar.sString);
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
                var n18 = M2Share.GetValNameNo(s14);
                if (n18 >= 0)
                {
                    if (HUtil32.RangeInDefined(n18, 0, 9))
                    {
                        sMsg = ReplaceVariableText(sMsg, '<' + sVariable + '>', PlayObject.m_nVal[n18].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 100, 119))
                    {
                        sMsg = ReplaceVariableText(sMsg, '<' + sVariable + '>', M2Share.g_Config.GlobalVal[n18 - 100].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 200, 299))
                    {
                        sMsg = ReplaceVariableText(sMsg, '<' + sVariable + '>', PlayObject.m_DyVal[n18 - 200].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 300, 399))
                    {
                        sMsg = ReplaceVariableText(sMsg, '<' + sVariable + '>', PlayObject.m_nMval[n18 - 300].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 400, 499))
                    {
                        sMsg = ReplaceVariableText(sMsg, '<' + sVariable + '>', M2Share.g_Config.GlobaDyMval[n18 - 400].ToString());
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

        protected string ReplaceVariableText(string sMsg, string sStr, string sText)
        {
            string result;
            var n10 = sMsg.IndexOf(sStr, StringComparison.OrdinalIgnoreCase);
            if (n10 > -1)
            {
                var s14 = sMsg.Substring(0, n10);
                var s18 = sMsg.Substring(sStr.Length + n10, sMsg.Length - (sStr.Length + n10));
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

        protected virtual void SendCustemMsg(TPlayObject PlayObject, string sMsg)
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

        public override void Initialize()
        {
            base.Initialize();
            this.m_Castle = M2Share.CastleManager.InCastleWarArea(this);
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

        private bool GetValValue(TPlayObject PlayObject, string sMsg, ref int nValue)
        {
            bool result = false;
            int n01;
            try
            {
                if (sMsg == "")
                {
                    return result;
                }
                n01 = M2Share.GetValNameNo(sMsg);
                if (n01 >= 0)
                {
                    if (HUtil32.RangeInDefined(n01, 0, 99))
                    {
                        nValue = PlayObject.m_nVal[n01];
                        result = true;
                    }
                    else if (HUtil32.RangeInDefined(n01, 100, 199))
                    {
                        nValue = M2Share.g_Config.GlobalVal[n01 - 100];
                        result = true;
                    }
                    else if (HUtil32.RangeInDefined(n01, 300, 399))
                    {
                        nValue = PlayObject.m_nMval[n01 - 300];
                        result = true;
                    }
                    else if (HUtil32.RangeInDefined(n01, 400, 499))
                    {
                        nValue = M2Share.g_Config.GlobaDyMval[n01 - 400];
                        result = true;
                    }
                    else if (HUtil32.RangeInDefined(n01, 500, 599))
                    {
                        nValue = PlayObject.m_nInteger[n01 - 500];
                        result = true;
                    }
                    else if (HUtil32.RangeInDefined(n01, 800, 1199))
                    {
                        nValue = M2Share.g_Config.GlobalVal[n01 - 700];
                        result = true;
                    }
                    else if (HUtil32.RangeInDefined(n01, 1700, 1799))
                    {
                        nValue = PlayObject.m_ServerIntVal[n01 - 1700];
                        result = true;
                    }
                }
            }
            catch
            {
                M2Share.MainOutMessage("{异常} TNormNpc.GetValValue1");
            }
            return result;
        }

        private bool GetValValue(TPlayObject PlayObject, string sMsg, ref string sValue)
        {
            bool result = false;
            int n01;
            try
            {
                if (sMsg == "")
                {
                    return result;
                }
                n01 = M2Share.GetValNameNo(sMsg);
                if (n01 >= 0)
                {
                    if (HUtil32.RangeInDefined(n01, 600, 699))
                    {
                        sValue = PlayObject.m_sString[n01 - 600];
                        result = true;
                    }
                    else if (HUtil32.RangeInDefined(n01, 700, 799))
                    {
                        sValue = M2Share.g_Config.GlobalAVal[n01 - 700];
                        result = true;
                    }
                    else if (HUtil32.RangeInDefined(n01, 1200, 1599))
                    {
                        sValue = M2Share.g_Config.GlobalAVal[n01 - 1100];// A变量(100-499)
                        result = true;
                    }
                    else if (HUtil32.RangeInDefined(n01, 1600, 1699))
                    {
                        sValue = PlayObject.m_ServerStrVal[n01 - 1600];
                        result = true;
                    }
                }
            }
            catch
            {
                M2Share.MainOutMessage("{异常} TNormNpc.GetValValue2");
            }
            return result;
        }

        private bool SetValValue(TPlayObject PlayObject, string sMsg, int nValue)
        {
            bool result = false;
            int n01;
            try
            {
                if (sMsg == "")
                {
                    return result;
                }
                n01 = M2Share.GetValNameNo(sMsg);
                if (n01 >= 0)
                {
                    if (HUtil32.RangeInDefined(n01, 0, 99))
                    {
                        PlayObject.m_nVal[n01] = nValue;
                        result = true;
                    }
                    else if (HUtil32.RangeInDefined(n01, 100, 199))
                    {
                        M2Share.g_Config.GlobalVal[n01 - 100] = nValue;
                        result = true;
                    }
                    else if (HUtil32.RangeInDefined(n01, 200, 299))
                    {
                        PlayObject.m_DyVal[n01 - 200] = nValue;
                        result = true;
                    }
                    else if (HUtil32.RangeInDefined(n01, 300, 399))
                    {
                        PlayObject.m_nMval[n01 - 300] = nValue;
                        result = true;
                    }
                    else if (HUtil32.RangeInDefined(n01, 400, 499))
                    {
                        M2Share.g_Config.GlobaDyMval[n01 - 400] = nValue;
                        result = true;
                    }
                    else if (HUtil32.RangeInDefined(n01, 500, 599))
                    {
                        PlayObject.m_nInteger[n01 - 500] = nValue;
                        result = true;
                    }
                    else if (HUtil32.RangeInDefined(n01, 800, 1199))
                    {
                        M2Share.g_Config.GlobalVal[n01 - 700] = nValue;//G变量
                        result = true;
                    }
                    else if (HUtil32.RangeInDefined(n01, 1700, 1799))
                    {
                        PlayObject.m_ServerIntVal[n01 - 1700] = nValue;
                        result = true;
                    }
                }
            }
            catch
            {
                M2Share.MainOutMessage("{异常} TNormNpc.SetValValue1");
            }
            return result;
        }

        private bool SetValValue(TPlayObject PlayObject, string sMsg, string sValue)
        {
            bool result = false;
            int n01;
            try
            {
                if (sMsg == "")
                {
                    return result;
                }
                n01 = M2Share.GetValNameNo(sMsg);
                if (n01 >= 0)
                {
                    if (HUtil32.RangeInDefined(n01, 600, 699))
                    {
                        PlayObject.m_sString[n01 - 600] = sValue;
                        result = true;
                    }
                    else if (HUtil32.RangeInDefined(n01, 700, 799))
                    {
                        M2Share.g_Config.GlobalAVal[n01 - 700] = sValue;
                        result = true;
                    }
                    else if (HUtil32.RangeInDefined(n01, 1200, 1599))
                    {
                        M2Share.g_Config.GlobalAVal[n01 - 1100] = sValue;// A变量(100-499)
                        result = true;
                    }
                }
            }
            catch
            {
                M2Share.MainOutMessage("{异常} TNormNpc.SetValValue2");
            }
            return result;
        }
    }
}