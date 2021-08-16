using SystemModule;
using System;
using System.Collections.Generic;

namespace M2Server
{
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
        public IList<TScriptParams> BatchParamsList;
       
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
                    if (HUtil32.RangeInDefined(n18, 0, 9))
                    {
                        sMsg = sub_49ADB8(sMsg, '<' + sVariable + '>', PlayObject.m_nVal[n18].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 100, 119))
                    {
                        sMsg = sub_49ADB8(sMsg, '<' + sVariable + '>', M2Share.g_Config.GlobalVal[n18 - 100].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 200, 209))
                    {
                        sMsg = sub_49ADB8(sMsg, '<' + sVariable + '>', PlayObject.m_DyVal[n18 - 200].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 300, 399))
                    {
                        sMsg = sub_49ADB8(sMsg, '<' + sVariable + '>', PlayObject.m_nMval[n18 - 300].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 400, 499))
                    {
                        sMsg = sub_49ADB8(sMsg, '<' + sVariable + '>', M2Share.g_Config.GlobaDyMval[n18 - 400].ToString());
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
    }
}