using M2Server;
using M2Server.Items;
using M2Server.Player;
using ScriptEngine.Consts;
using SystemModule;
using SystemModule.Enums;

namespace ScriptEngine.Processings
{
    /// <summary>
    /// 全局变量脚本处理模块
    /// </summary>
    public class GrobalVarProcessingSys
    {
        /// <summary>
        /// 全局变量消息处理列表
        /// </summary>
        private static Dictionary<int, HandleGrobalMessage> ProcessGrobalMessage;

        private delegate void HandleGrobalMessage(PlayObject playObject, string sVariable, ref string sMsg);

        /// <summary>
        /// 初始化全局变量脚本处理列表
        /// </summary>
        public GrobalVarProcessingSys()
        {
            ProcessGrobalMessage = new Dictionary<int, HandleGrobalMessage>();
            ProcessGrobalMessage[GrobalVarCode.nVAR_SERVERNAME] = GetServerName;
            ProcessGrobalMessage[GrobalVarCode.nVAR_SERVERIP] = GetServerIp;
            ProcessGrobalMessage[GrobalVarCode.nVAR_WEBSITE] = GetWebSite;
            ProcessGrobalMessage[GrobalVarCode.nVAR_BBSSITE] = GetBbsWeiSite;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CLIENTDOWNLOAD] = GetCilentDownLoad;
            ProcessGrobalMessage[GrobalVarCode.nVAR_QQ] = GetQQ;
            ProcessGrobalMessage[GrobalVarCode.nVAR_PHONE] = GetPhone;
            ProcessGrobalMessage[GrobalVarCode.nVAR_BANKACCOUNT0] = GetBankAccount0;
            ProcessGrobalMessage[GrobalVarCode.nVAR_BANKACCOUNT1] = GetBankAccount1;
            ProcessGrobalMessage[GrobalVarCode.nVAR_BANKACCOUNT2] = GetBankAccount2;
            ProcessGrobalMessage[GrobalVarCode.nVAR_BANKACCOUNT3] = GetBankAccount3;
            ProcessGrobalMessage[GrobalVarCode.nVAR_BANKACCOUNT4] = GetBankAccount4;
            ProcessGrobalMessage[GrobalVarCode.nVAR_BANKACCOUNT5] = GetBankAccount5;
            ProcessGrobalMessage[GrobalVarCode.nVAR_BANKACCOUNT6] = GetBankAccount6;
            ProcessGrobalMessage[GrobalVarCode.nVAR_BANKACCOUNT7] = GetBankAccount7;
            ProcessGrobalMessage[GrobalVarCode.nVAR_BANKACCOUNT8] = GetBankAccount8;
            ProcessGrobalMessage[GrobalVarCode.nVAR_BANKACCOUNT9] = GetBankAccount9;
            ProcessGrobalMessage[GrobalVarCode.nVAR_GAMEGOLDNAME] = GetGameGoldName;
            ProcessGrobalMessage[GrobalVarCode.nVAR_GAMEPOINTNAME] = GetPointName;
            ProcessGrobalMessage[GrobalVarCode.nVAR_USERCOUNT] = GetUserCount;
            ProcessGrobalMessage[GrobalVarCode.nVAR_DATETIME] = GetDateTime;
            ProcessGrobalMessage[GrobalVarCode.nVAR_USERNAME] = GetUserName;
            ProcessGrobalMessage[GrobalVarCode.nVAR_MAPNAME] = GetMapName;
            ProcessGrobalMessage[GrobalVarCode.nVAR_GUILDNAME] = GetGuilidName;
            ProcessGrobalMessage[GrobalVarCode.nVAR_RANKNAME] = GetGuilidRankName;
            ProcessGrobalMessage[GrobalVarCode.nVAR_LEVEL] = GetLevel;
            ProcessGrobalMessage[GrobalVarCode.nVAR_HP] = GetHP;
            ProcessGrobalMessage[GrobalVarCode.nVAR_MAXHP] = GetMaxHP;
            ProcessGrobalMessage[GrobalVarCode.nVAR_MP] = GetMP;
            ProcessGrobalMessage[GrobalVarCode.nVAR_MAXMP] = GetMaxHP;
            ProcessGrobalMessage[GrobalVarCode.nVAR_AC] = GetAc;
            ProcessGrobalMessage[GrobalVarCode.nVAR_MAXAC] = GetMaxAc;
            ProcessGrobalMessage[GrobalVarCode.nVAR_MAC] = GetMac;
            ProcessGrobalMessage[GrobalVarCode.nVAR_MAXMAC] = GetMaxMac;
            ProcessGrobalMessage[GrobalVarCode.nVAR_DC] = GetDC;
            ProcessGrobalMessage[GrobalVarCode.nVAR_MAXDC] = GetMaxDC;
            ProcessGrobalMessage[GrobalVarCode.nVAR_MC] = GetMc;
            ProcessGrobalMessage[GrobalVarCode.nVAR_MAXMC] = GetMaxMc;
            ProcessGrobalMessage[GrobalVarCode.nVAR_SC] = GetSc;
            ProcessGrobalMessage[GrobalVarCode.nVAR_MAXSC] = GetMaxSc;
            ProcessGrobalMessage[GrobalVarCode.nVAR_EXP] = GetExp;
            ProcessGrobalMessage[GrobalVarCode.nVAR_MAXEXP] = GetMaxExp;
            ProcessGrobalMessage[GrobalVarCode.nVAR_PKPOINT] = GetPkPoint;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CREDITPOINT] = GetCreditPoint;
            ProcessGrobalMessage[GrobalVarCode.nVAR_GOLDCOUNT] = GetGoldCount;
            ProcessGrobalMessage[GrobalVarCode.nVAR_GAMEGOLD] = GetGameGold;
            ProcessGrobalMessage[GrobalVarCode.nVAR_GAMEPOINT] = GetGamePoint;
            ProcessGrobalMessage[GrobalVarCode.nVAR_LOGINTIME] = GetLoginTime;
            ProcessGrobalMessage[GrobalVarCode.nVAR_LOGINLONG] = GetLoginTime;
            ProcessGrobalMessage[GrobalVarCode.nVAR_DRESS] = GetDress;
            ProcessGrobalMessage[GrobalVarCode.nVAR_WEAPON] = GetWeapon;
            ProcessGrobalMessage[GrobalVarCode.nVAR_RIGHTHAND] = GetRightHand;
            ProcessGrobalMessage[GrobalVarCode.nVAR_HELMET] = GetHelmet;
            ProcessGrobalMessage[GrobalVarCode.nVAR_NECKLACE] = GetNecklace;
            ProcessGrobalMessage[GrobalVarCode.nVAR_RING_R] = GetRing_R;
            ProcessGrobalMessage[GrobalVarCode.nVAR_RING_L] = GetRing_L;
            ProcessGrobalMessage[GrobalVarCode.nVAR_ARMRING_R] = GetArmring_R;
            ProcessGrobalMessage[GrobalVarCode.nVAR_ARMRING_L] = GetArmring_L;
            ProcessGrobalMessage[GrobalVarCode.nVAR_BUJUK] = GetBujuk;
            ProcessGrobalMessage[GrobalVarCode.nVAR_BELT] = GetBelt;
            ProcessGrobalMessage[GrobalVarCode.nVAR_BOOTS] = GetBoots;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CHARM] = GetChrm;
            ProcessGrobalMessage[GrobalVarCode.nVAR_IPADDR] = GetIpAddr;
            ProcessGrobalMessage[GrobalVarCode.nVAR_IPLOCAL] = GetIpLocal;
            ProcessGrobalMessage[GrobalVarCode.nVAR_GUILDBUILDPOINT] = GeTGuildBuildPoint;
            ProcessGrobalMessage[GrobalVarCode.nVAR_GUILDAURAEPOINT] = GeTGuildAuraePoint;
            ProcessGrobalMessage[GrobalVarCode.nVAR_GUILDSTABILITYPOINT] = GeTGuildStabilityPoint;
            ProcessGrobalMessage[GrobalVarCode.nVAR_GUILDFLOURISHPOINT] = GeTGuildFlourishPoint;
            ProcessGrobalMessage[GrobalVarCode.nVAR_REQUESTCASTLEWARITEM] = GetRequestCastlewarItem;
            ProcessGrobalMessage[GrobalVarCode.nVAR_REQUESTCASTLEWARDAY] = GetRequestCastleWarday;
            ProcessGrobalMessage[GrobalVarCode.nVAR_REQUESTBUILDGUILDITEM] = GetRequestBuildGuildItem;
            ProcessGrobalMessage[GrobalVarCode.nVAR_OWNERGUILD] = GetOwnerGuild;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CASTLENAME] = GetCastleName;
            ProcessGrobalMessage[GrobalVarCode.nVAR_LORD] = GetLord;
            ProcessGrobalMessage[GrobalVarCode.nVAR_GUILDWARFEE] = GeTGuildWarfee;
            ProcessGrobalMessage[GrobalVarCode.nVAR_BUILDGUILDFEE] = GetBuildGuildfee;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CASTLEWARDATE] = GetCastleWarDate;
            ProcessGrobalMessage[GrobalVarCode.nVAR_LISTOFWAR] = GetListofWar;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CASTLECHANGEDATE] = GetCastleChangeDate;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CASTLEWARLASTDATE] = GetCastlewarLastDate;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CASTLEGETDAYS] = GetCastlegetDays;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CMD_DATE] = GetCmdDate;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CMD_ALLOWMSG] = GetCmdAllowmsg;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CMD_LETSHOUT] = GetCmdletshout;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CMD_LETTRADE] = GetCmdLettrade;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CMD_LETGuild] = GetCmdLeTGuild;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CMD_ENDGUILD] = GetCmdEndGuild;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CMD_BANGUILDCHAT] = GetCmdBanGuildChat;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CMD_AUTHALLY] = GetCmdAuthally;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CMD_AUTH] = GetCmdAuth;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CMD_AUTHCANCEL] = GetCmdAythCcancel;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CMD_USERMOVE] = GetCmdUserMove;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CMD_SEARCHING] = GetCmdSearching;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CMD_ALLOWGROUPCALL] = GetCmdAllowGroupCall;
            ProcessGrobalMessage[GrobalVarCode.nVAR_CMD_GROUPRECALLL] = GetCmdGroupCall;
            ProcessGrobalMessage[GrobalVarCode.nVAR_STR] = GetGrobalVarStr;
        }

        /// <summary>
        /// 处理脚本
        /// </summary>
        /// <param name="nIdx"></param>
        public static void Handler(PlayObject playObject, int nIdx, string sVariable, ref string sMsg)
        {
            if (ProcessGrobalMessage.ContainsKey(nIdx))
            {
                ProcessGrobalMessage[nIdx](playObject, sVariable, ref sMsg);
                //if (nIdx < FProcessGrobalMessage.Count())
                //{
                //    FProcessGrobalMessage[nIdx](playObject, sVariable, ref sMsg);
                //}
                //else
                //{
                //    M2Share.Logger.Error(string.Format("未知全局变量:{0}", nIdx));
                //}
            }
        }

        internal void GetGrobalVarStr(PlayObject playObject, string sVariable, ref string sMsg)
        {
            var sIdx = sVariable.Substring(1, sVariable.Length - 1);
            var sID = HUtil32.GetValidStr3(sIdx, ref sIdx, "/");
            var n18 = M2Share.GetValNameNo(sID);
            if (n18 >= 0)
            {
                if (HUtil32.RangeInDefined(n18, 0, 499))
                {
                    sMsg = CombineStr(sMsg, '<' + sVariable + '>', M2Share.Config.GlobalVal[n18]);
                }
                else if (HUtil32.RangeInDefined(n18, 1100, 1109))
                {
                    sMsg = CombineStr(sMsg, '<' + sVariable + '>', playObject.MNVal[n18 - 1100]);
                }
                else if (HUtil32.RangeInDefined(n18, 1110, 1119))
                {
                    sMsg = CombineStr(sMsg, '<' + sVariable + '>', playObject.MDyVal[n18 - 1110]);
                }
                else if (HUtil32.RangeInDefined(n18, 1200, 1299))
                {
                    sMsg = CombineStr(sMsg, '<' + sVariable + '>', playObject.MNMval[n18 - 1200]);
                }
                else if (HUtil32.RangeInDefined(n18, 1000, 1099))
                {
                    sMsg = CombineStr(sMsg, '<' + sVariable + '>', M2Share.Config.GlobaDyMval[n18 - 1000]);
                }
                else if (HUtil32.RangeInDefined(n18, 1300, 1399))
                {
                    sMsg = CombineStr(sMsg, '<' + sVariable + '>', playObject.MNInteger[n18 - 1300]);
                }
                else if (HUtil32.RangeInDefined(n18, 1400, 1499))
                {
                    sMsg = CombineStr(sMsg, '<' + sVariable + '>', playObject.MSString[n18 - 1400]);
                }
                else if (HUtil32.RangeInDefined(n18, 2000, 2499))
                {
                    sMsg = CombineStr(sMsg, '<' + sVariable + '>', M2Share.Config.GlobalAVal[n18 - 2000]);
                }
            }
        }

        /// <summary>
        /// 取在线人数
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetUserCount(PlayObject playObject, string sVariable, ref string sMsg)
        {
           // sMsg = CombineStr(sMsg, $"<{sVariable}>", Convert.ToString(M2Share.WorldEngine.playObjectCount));
        }

        /// <summary>
        /// 取服务器名称
        /// </summary>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetServerName(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, $"<{sVariable}>", M2Share.Config.ServerName);
        }

        /// <summary>
        /// 取网站地址
        /// </summary>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetWebSite(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, $"<{sVariable}>", M2Share.Config.sWebSite);
        }

        /// <summary>
        /// 取服务器时间
        /// </summary>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetDateTime(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, $"<{sVariable}>", DateTime.Now.ToString("dddddd,dddd,hh:mm:nn"));
        }

        /// <summary>
        /// 取玩家名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetUserName(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, $"<{sVariable}>", playObject.ChrName);
        }

        /// <summary>
        /// 取行会名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetGuilidName(PlayObject playObject, string sVariable, ref string sMsg)
        {
            if (playObject.MyGuild != null)
            {
                sMsg = CombineStr(sMsg, string.Format("<0>", sVariable), playObject.MyGuild.GuildName);
            }
            else
            {
                sMsg = "无";
            }
        }

        /// <summary>
        /// 取玩行会封号名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetGuilidRankName(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, $"<{sVariable}>", playObject.GuildRankName);
        }

        /// <summary>
        /// 查看申请攻城战役行会列表
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetListofWar(PlayObject playObject, string sVariable, ref string sMsg)
        {
            if (playObject.Castle != null)
            {
                sMsg = playObject.Castle.GetAttackWarList();
            }
            else
            {
                sMsg = "????";
            }
            if (sMsg != "")
            {
                sMsg = CombineStr(sMsg, $"<{sVariable}>", sMsg);
            }
            else
            {
                sMsg = "现在没有行会申请攻城战\\ \\<返回/@main>";
            }
            return;
        }

        /// <summary>
        /// 取沙巴克行会攻城列表
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCastleWarDate(PlayObject playObject, string sVariable, ref string sMsg)
        {
            if (playObject.Castle == null)
            {
                playObject.Castle = M2Share.CastleMgr.GetCastle(0);
            }
            if (playObject.Castle != null)
            {
                if (!playObject.Castle.UnderWar)
                {
                    sMsg = playObject.Castle.GetWarDate();
                    if (sMsg != "")
                    {
                        sMsg = CombineStr(sMsg, $"<{sVariable}>", sMsg);
                    }
                    else
                    {
                        sMsg = "暂时没有行会攻城！！！\\ \\<返回/@main>";
                    }
                }
                else
                {
                    sMsg = "现正在攻城中！！！\\ \\<返回/@main>";
                }
            }
            else
            {
                sMsg = "????";
            }
        }

        /// <summary>
        /// 取交易对像
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetDealGoldPlay(PlayObject playObject, string sVariable, ref string sMsg)
        {
            var PoseHuman = playObject.GetPoseCreate();
            if ((PoseHuman != null) && (PoseHuman.GetPoseCreate() == playObject) && (PoseHuman.Race == ActorRace.Play))
            {
                sMsg = CombineStr(sMsg, $"<{sVariable}>", PoseHuman.ChrName);
            }
            else
            {
                sMsg = CombineStr(sMsg, $"<{sVariable}>", "????");
            }
        }

        /// <summary>
        /// 取服务器运行时间
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetServerRunTime(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, $"<{sVariable}>", DateTimeOffset.FromUnixTimeMilliseconds(M2Share.StartTime).ToString("YYYY-MM-DD HH:mm:ss"));
        }

        /// <summary>
        /// 取服务器运行天数
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMacrunTime(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, string.Format("<{0}>"), Convert.ToString(HUtil32.GetTickCount() / (24 * 60 * 60 * 1000)));
        }

        /// <summary>
        /// 取最高等级人物信息
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetHighLevelInfo(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            var highLevelPlay = (PlayObject)M2Share.ActorMgr.Get(M2Share.HighLevelHuman);
            if (highLevelPlay != null)
            {
                sText = highLevelPlay.GetMyInfo();
            }
            else
            {
                sText = "????";
            }
            sMsg = CombineStr(sMsg, string.Format("<0>", sVariable), sText);
        }

        /// <summary>
        /// 最高PK点数人物信息
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetHighPkInfo(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            var highPvpPlay = (PlayObject)M2Share.ActorMgr.Get(M2Share.HighLevelHuman);
            if (highPvpPlay != null)
            {
                sText = highPvpPlay.GetMyInfo();
            }
            else
            {
                sText = "????";
            }
            sMsg = CombineStr(sMsg, string.Format("<0>"), sText);
        }

        /// <summary>
        /// 最高攻击力人物信息
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetHighDcInfo(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            var highDcPlay = (PlayObject)M2Share.ActorMgr.Get(M2Share.HighLevelHuman);
            if (highDcPlay != null)
            {
                sText = highDcPlay.GetMyInfo();
            }
            else
            {
                sText = "????";
            }
            sMsg = CombineStr(sMsg, string.Format("<0>", sVariable), sText);
        }

        /// <summary>
        /// 最高魔法力人物信息
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetHighMcInfo(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            var highMcPlay = (PlayObject)M2Share.ActorMgr.Get(M2Share.HighLevelHuman);
            if (highMcPlay != null)
            {
                sText = highMcPlay.GetMyInfo();
            }
            else
            {
                sText = "????";
            }
            sMsg = CombineStr(sMsg, string.Format("<0>", sVariable), sText);
        }

        /// <summary>
        /// 最高道术人物信息
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetHighScInfo(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            var highScPlay = (PlayObject)M2Share.ActorMgr.Get(M2Share.HighLevelHuman);
            if (highScPlay != null)
            {
                sText = highScPlay.GetMyInfo();
            }
            else
            {
                sText = "????";
            }
            sMsg = CombineStr(sMsg, string.Format("<0>", sVariable), sText);
        }

        /// <summary>
        /// 最高最长在线时间人物信息
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetHighOlineInfo(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            var highOnlinePlay = (PlayObject)M2Share.ActorMgr.Get(M2Share.HighLevelHuman);
            if (highOnlinePlay != null)
            {
                sText = highOnlinePlay.GetMyInfo();
            }
            else
            {
                sText = "????";
            }
            sMsg = CombineStr(sMsg, string.Format("<0>", sVariable), sText);
        }

        /// <summary>
        /// 取玩家登录时长
        /// </summary>
        internal void GetLoginLong(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = ((HUtil32.GetTickCount() - playObject.LogonTick) / 60000) + "分钟";
            sMsg = CombineStr(sMsg, string.Format("<0>", sVariable), sText);
        }

        /// <summary>
        /// 取玩家登录时间
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetLoginTime(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, $"<{sVariable}>", Convert.ToString(playObject.LogonTime));
        }

        /// <summary>
        /// 取行会建筑度
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GeTGuildBuildPoint(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            if (playObject.MyGuild == null)
            {
                sText = "无";
            }
            else
            {
                sText = (playObject.MyGuild.BuildPoint).ToString();
            }
            sMsg = CombineStr(sMsg, $"<{sVariable}>", sText);
        }

        /// <summary>
        /// 行会人气度
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GeTGuildAuraePoint(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            if (playObject.MyGuild == null)
            {
                sText = "无";
            }
            else
            {
                sText = (playObject.MyGuild.Aurae).ToString();
            }
            sMsg = CombineStr(sMsg, $"<{sVariable}>", sText);
        }

        /// <summary>
        /// 取行会安定度
        /// </summary>
        internal void GeTGuildStabilityPoint(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            if (playObject.MyGuild == null)
            {
                sText = "无";
            }
            else
            {
                sText = (playObject.MyGuild.Stability).ToString();
            }
            sMsg = CombineStr(sMsg, $"<{sVariable}>", sText);
        }

        /// <summary>
        /// 取行会繁荣度
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GeTGuildFlourishPoint(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            if (playObject.MyGuild == null)
            {
                sText = "无";
            }
            else
            {
                sText = (playObject.MyGuild.Flourishing).ToString();
            }
            sMsg = CombineStr(sMsg, string.Format("<{0}>", sVariable), sText);
        }

        /// <summary>
        /// 取攻城需要的物品
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetRequestCastlewarItem(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, $"<{sVariable}>", M2Share.Config.ZumaPiece);
        }

        /// <summary>
        /// 取城保所属行会
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetOwnerGuild(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            if (playObject.Castle != null)
            {
                sText = playObject.Castle.OwnGuild;
                if (sText == "")
                {
                    sText = "游戏管理";
                }
            }
            else
            {
                sText = "????";
            }
            sMsg = CombineStr(sMsg, $"<{sVariable}>", sText);
        }

        /// <summary>
        /// 取城堡名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCastleName(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            if (playObject.Castle != null)
            {
                sText = playObject.Castle.sName;
            }
            else
            {
                sText = "????";
            }
            sMsg = CombineStr(sMsg, string.Format("<{0}>"), sText);

        }

        /// <summary>
        /// 城堡所属行会的老大
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetLord(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            if (playObject.Castle != null)
            {
                if (playObject.Castle.MasterGuild != null)
                {
                    sText = playObject.Castle.MasterGuild.GetChiefName();
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
            sMsg = CombineStr(sMsg, $"<{sVariable}>", sText);
        }

        /// <summary>
        /// 取沙巴克占领日期
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCastleChangeDate(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            if (playObject.Castle != null)
            {
                sText = Convert.ToString(playObject.Castle.ChangeDate);
            }
            else
            {
                sText = "????";
            }
            sMsg = CombineStr(sMsg, $"<{sVariable}>", sText);
        }

        /// <summary>
        /// 最好一次攻城战役日期
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCastlewarLastDate(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            if (playObject.Castle != null)
            {
                sText = Convert.ToString(playObject.Castle.WarDate);
            }
            else
            {
                sText = "????";
            }
            sMsg = CombineStr(sMsg, string.Format("<{0}>"), sText);
        }

        /// <summary>
        /// 沙巴克占领天数
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCastlegetDays(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = string.Empty;
            if (playObject.Castle != null)
            {
                sText = (HUtil32.GetDayCount(DateTime.Now, playObject.Castle.ChangeDate)).ToString();
            }
            else
            {
                sText = "????";
            }
            sMsg = CombineStr(sMsg, $"<{sVariable}>", sText);
        }

        /// <summary>
        /// 取地图名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMapName(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, $"<{sVariable}>", playObject.Envir.MapDesc);
        }

        /// <summary>
        /// 取地图文件名
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMapFileName(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, $"<{sVariable}>", playObject.Envir.MapName);
        }

        /// <summary>
        /// 取当前对象等级
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetLevel(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (playObject.Abil.Level).ToString();
            sMsg = CombineStr(sMsg, $"<{sVariable}>", sText);
        }

        /// <summary>
        /// 取当前对象血量
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetHP(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = Convert.ToString(playObject.WAbil.HP);
            sMsg = CombineStr(sMsg, $"<{sVariable}>", sText);
        }

        /// <summary>
        /// 取当前对象最大血量
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMaxHP(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (playObject.WAbil.MaxHP).ToString();
            sMsg = CombineStr(sMsg, $"<{sVariable}>", sText);
        }

        /// <summary>
        /// 取当前对象魔法值
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMP(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (playObject.WAbil.MP).ToString();
            sMsg = CombineStr(sMsg, $"<{sVariable}>", sText);
        }

        /// <summary>
        /// 取当前对象最大魔法值
        /// </summary>
        internal void GetMaxMP(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (playObject.WAbil.MaxMP).ToString();
            sMsg = CombineStr(sMsg, $"<{sVariable}>", sText);
        }

        /// <summary>
        /// 取当前对象攻击力
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetDC(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (HUtil32.LoWord(playObject.WAbil.DC)).ToString();
            sMsg = CombineStr(sMsg, "<$DC>", sText);
        }

        /// <summary>
        /// 取当前对象最大攻击力
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMaxDC(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (HUtil32.HiWord(playObject.WAbil.DC)).ToString();
            sMsg = CombineStr(sMsg, "<$MAXDC>", sText);
        }

        /// <summary>
        /// 取当前对象魔法防御
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMac(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (HUtil32.LoWord(playObject.WAbil.MAC)).ToString();
            sMsg = CombineStr(sMsg, "<$MAC>", sText);
        }

        /// <summary>
        /// 取当前对象魔法力
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMc(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (HUtil32.LoWord(playObject.WAbil.MC)).ToString();
            sMsg = CombineStr(sMsg, "<$MC>", sText);
        }

        /// <summary>
        /// 取当前对象最大魔法力
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMaxMc(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (HUtil32.HiWord(playObject.WAbil.MC)).ToString();
            sMsg = CombineStr(sMsg, "<$MAXMC>", sText);
        }

        /// <summary>
        /// 取当前对象道术力
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetSc(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (HUtil32.LoWord(playObject.WAbil.SC)).ToString();
            sMsg = CombineStr(sMsg, "<$SC>", sText);
        }

        /// <summary>
        /// 取当前对象最大道术力
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMaxSc(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (HUtil32.HiWord(playObject.WAbil.SC)).ToString();
            sMsg = CombineStr(sMsg, "<$MAXSC>", sText);
        }

        /// <summary>
        /// 取当前对象经验值
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetExp(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (playObject.Abil.Exp).ToString();
            sMsg = CombineStr(sMsg, "<$EXP>", sText);
        }

        /// <summary>
        /// 最当前对象最大经验值
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMaxExp(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (playObject.Abil.MaxExp).ToString();
            sMsg = CombineStr(sMsg, "<$MAXEXP>", sText);
        }

        /// <summary>
        /// 取当前最新最大魔法防御力
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMaxMac(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (HUtil32.HiWord(playObject.WAbil.MAC)).ToString();
            sMsg = CombineStr(sMsg, "<$MAXMAC>", sText);
        }

        /// <summary>
        /// 取当前对象最大防御值
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMaxAc(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$MAXAC>", HUtil32.HiWord(playObject.WAbil.AC));
        }

        /// <summary>
        /// 取当前对象负重力
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetHw(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$HW>", playObject.WAbil.HandWeight);
        }

        /// <summary>
        /// 取当前对象防御
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetAc(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$AC>", HUtil32.LoWord(playObject.WAbil.AC));
        }

        /// <summary>
        /// 取当前对象最大负重力
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMaxHw(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$MAXHW>", playObject.WAbil.MaxHandWeight);
        }

        /// <summary>
        /// 取当前对象腕力
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetWW(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$WW>", playObject.WAbil.WearWeight);
        }

        /// <summary>
        /// 取当前对象最大腕力
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMaxWW(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$MAXWW>", playObject.WAbil.MaxWearWeight);
        }

        /// <summary>
        /// 取当前对象包裹重量
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBw(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BW>", playObject.WAbil.Weight);
        }

        /// <summary>
        /// 取当前对象包裹最大重量
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMaxBw(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$MAXBW>", playObject.WAbil.MaxWeight);
        }

        /// <summary>
        /// 取当前对象怪物经验值
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetMonExp(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$MONEXP>", playObject.FightExp);
        }

        /// <summary>
        /// 取当前对象PK点
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetPkPoint(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$PKPOINT>", playObject.PkPoint);
        }

        /// <summary>
        /// 取当前对象金币数量
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetGoldCount(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = Convert.ToString(playObject.Gold + '/' + playObject.GoldMax);
            sMsg = CombineStr(sMsg, "<$GOLDCOUNT>", sText);
        }

        /// <summary>
        /// 取当前对象金币数量
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetGameGold(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (playObject.GameGold).ToString();
            sMsg = CombineStr(sMsg, "<$GAMEGOLD>", sText);
        }

        /// <summary>
        /// 取当前对象声望点
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetGamePoint(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (playObject.GamePoint).ToString();
            sMsg = CombineStr(sMsg, "<$GAMEPOINT>", sText);
        }

        /// <summary>
        /// 当前对象声望点数
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCreditPoint(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (playObject.CreditPoint).ToString();
            sMsg = CombineStr(sMsg, "<$CREDITPOINT>", sText);
        }

        /// <summary>
        /// 取金币名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetGameGoldName(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$GAMEGOLDNAME>", M2Share.Config.GameGoldName);
        }

        /// <summary>
        /// 取当前对象饥饿程度
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetHunGer(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (playObject.GetMyStatus()).ToString();
            sMsg = CombineStr(sMsg, "<$HUNGER>", sText);
        }

        /// <summary>
        /// 取当前对象所在地图X坐标
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCurrX(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (playObject.CurrX).ToString();
            sMsg = CombineStr(sMsg, "<$CURRX>", sText);
        }

        /// <summary>
        /// 取当前对象所在地图X坐标
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCurrY(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (playObject.CurrY).ToString();
            sMsg = CombineStr(sMsg, "<$CURRX>", sText);
        }

        /// <summary>
        /// 取声望点名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetPointName(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$GAMEPOINTNAME>", M2Share.Config.GamePointName);
        }

        /// <summary>
        /// 取当前对象所在IP地址
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetIpAddr(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$IPADDR>", playObject.LoginIpAddr);
        }

        /// <summary>
        /// 取当前对象所在区域
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetIpLocal(PlayObject playObject, string sVariable, ref string sMsg)
        {
            //string sText = playObject.m_sIPLocal;
            // GetIPLocal(playObject.m_sIPaddr);
            sMsg = CombineStr(sMsg, "<$IPLOCAL>", "无");
        }

        /// <summary>
        /// 行会战金币数
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GeTGuildWarfee(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$GUILDWARFEE>", (M2Share.Config.GuildWarPrice).ToString());
        }
        
        /// <summary>
        /// 建立行会所需的金币数
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBuildGuildfee(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BUILDGUILDFEE>", (M2Share.Config.BuildGuildPrice).ToString());
        }

        /// <summary>
        /// 允许建立行会的物品名字
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetRequestBuildGuildItem(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$REQUESTBUILDGUILDITEM>", M2Share.Config.WomaHorn);
        }
        
        /// <summary>
        /// 多少天后攻城
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetRequestCastleWarday(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$REQUESTCASTLEWARDAY>", M2Share.Config.ZumaPiece);
        }

        /// <summary>
        /// 取日期命令
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdDate(PlayObject playObject, string sVariable, ref string sMsg)
        {
           // sMsg = CombineStr(sMsg, "<$CMD_DATE>", CommandMgr.GameCommands.Data.CmdName);
        }

        /// <summary>
        /// 查看接收所有消息命令
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdAllowmsg(PlayObject playObject, string sVariable, ref string sMsg)
        {
           // sMsg = CombineStr(sMsg, "<$CMD_ALLOWMSG>", CommandMgr.GameCommands.AllowMsg.CmdName);
        }

        /// <summary>
        /// 查看允许群聊命令
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdletshout(PlayObject playObject, string sVariable, ref string sMsg)
        {
           // sMsg = CombineStr(sMsg, "<$CMD_LETSHOUT>", CommandMgr.GameCommands.Letshout.CmdName);
        }

        /// <summary>
        /// 查看允许交易命令
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdLettrade(PlayObject playObject, string sVariable, ref string sMsg)
        {
          //  sMsg = CombineStr(sMsg, "<$CMD_LETTRADE>", CommandMgr.GameCommands.LetTrade.CmdName);
        }

        /// <summary>
        /// 查看允许加入门派命令
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdLeTGuild(PlayObject playObject, string sVariable, ref string sMsg)
        {
          //  sMsg = CombineStr(sMsg, "<$CMD_LETGuild>", CommandMgr.GameCommands.LetGuild.CmdName);
        }

        /// <summary>
        /// 查看退出门派命令
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdEndGuild(PlayObject playObject, string sVariable, ref string sMsg)
        {
           // sMsg = CombineStr(sMsg, "<$CMD_ENDGUILD>", CommandMgr.GameCommands.EndGuild.CmdName);
        }

        /// <summary>
        /// 查看允许行会聊天命令
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdBanGuildChat(PlayObject playObject, string sVariable, ref string sMsg)
        {
           // sMsg = CombineStr(sMsg, "<$CMD_BANGUILDCHAT>", CommandMgr.GameCommands.BanGuildChat.CmdName);
        }

        /// <summary>
        /// 查看允许行会联盟命令
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdAuthally(PlayObject playObject, string sVariable, ref string sMsg)
        {
          //  sMsg = CombineStr(sMsg, "<$CMD_AUTHALLY>", CommandMgr.GameCommands.Authally.CmdName);
        }

        /// <summary>
        /// 查看行会联盟命令
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdAuth(PlayObject playObject, string sVariable, ref string sMsg)
        {
           // sMsg = CombineStr(sMsg, "<$CMD_AUTH>", CommandMgr.GameCommands.Auth.CmdName);
        }

        /// <summary>
        /// 查看取消行会联盟命令
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdAythCcancel(PlayObject playObject, string sVariable, ref string sMsg)
        {
           // sMsg = CombineStr(sMsg, "<$CMD_AUTHCANCEL>", CommandMgr.GameCommands.AuthCancel.CmdName);
        }

        /// <summary>
        /// 查看传送戒指命令
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdUserMove(PlayObject playObject, string sVariable, ref string sMsg)
        {
          //  sMsg = CombineStr(sMsg, "<$CMD_USERMOVE>", CommandMgr.GameCommands.UserMove.CmdName);
        }

        /// <summary>
        /// 查看探测项链命令
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdSearching(PlayObject playObject, string sVariable, ref string sMsg)
        {
            // sMsg = CombineStr(sMsg, "<$CMD_SEARCHING>", CommandMgr.GameCommands.Searching.CmdName);
        }

        /// <summary>
        /// 查看组队传送命令
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdGroupCall(PlayObject playObject, string sVariable, ref string sMsg)
        {
            //sMsg = CombineStr(sMsg, "<$CMD_GROUPRECALLL>", CommandMgr.GameCommands.GroupRecalll.CmdName);
        }

        /// <summary>
        /// 查看允许组队传送命令
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdAllowGroupCall(PlayObject playObject, string sVariable, ref string sMsg)
        {
           // sMsg = CombineStr(sMsg, "<$CMD_ALLOWGROUPCALL>", CommandMgr.GameCommands.AllowGroupCall.CmdName);
        }

        /// <summary>
        /// 查看更改仓库密码命令
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdStorageChgPassword(PlayObject playObject, string sVariable, ref string sMsg)
        {
           // sMsg = CombineStr(sMsg, "<$CMD_STORAGECHGPASSWORD>", CommandMgr.GameCommands.ChgPassword.CmdName);
        }

        /// <summary>
        /// 为仓库设定一个4-7位数长的仓库密码命令
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdStorageSetPassword(PlayObject playObject, string sVariable, ref string sMsg)
        {
          //  sMsg = CombineStr(sMsg, "<$CMD_STORAGESETPASSWORD>", CommandMgr.GameCommands.SetPassword.CmdName);
        }

        /// <summary>
        /// 可将仓库锁定命令
        /// </summary>
        internal void GetCmdStorageLock(PlayObject playObject, string sVariable, ref string sMsg)
        {
          //  sMsg = CombineStr(sMsg, "<$CMD_STORAGELOCK>", CommandMgr.GameCommands.Lock.CmdName);
        }

        /// <summary>
        /// 开启仓库命令
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCmdStorageUnlock(PlayObject playObject, string sVariable, ref string sMsg)
        {
          //  sMsg = CombineStr(sMsg, "<$CMD_STORAGEUNLOCK>", CommandMgr.GameCommands.UnlockStorage.CmdName);
        }

        /// <summary>
        /// 取当前对象武器名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetWeapon(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.Weapon].Index);
            sMsg = CombineStr(sMsg, "<$WEAPON>", sText);
        }

        /// <summary>
        /// 取当前对象衣服名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetDress(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.Dress].Index);
            sMsg = CombineStr(sMsg, "<$DRESS>", sText);
        }

        /// <summary>
        /// 取当前对象蜡烛名称(勋章)
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetRightHand(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.RighThand].Index);
            sMsg = CombineStr(sMsg, "<$RIGHTHAND>", sText);
        }

        /// <summary>
        /// 取当前对象头盔名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetHelmet(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.Helmet].Index);
            sMsg = CombineStr(sMsg, "<$HELMET>", sText);
        }

        /// <summary>
        /// 取当前对象项链名称
        /// </summary>
        internal void GetNecklace(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.Necklace].Index);
            sMsg = CombineStr(sMsg, "<$NECKLACE>", sText);
        }

        /// <summary>
        /// 取当前对象右戒指名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetRing_R(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.Ringr].Index);
            sMsg = CombineStr(sMsg, "<$RING_R>", sText);
        }

        /// <summary>
        /// 取当前对象左戒指名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetRing_L(PlayObject playObject, string sVariable, ref string sMsg)
        {
           string  sText = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.Ringl].Index);
            sMsg = CombineStr(sMsg, "<$RING_L>", sText);
        }

        /// <summary>
        /// 取当前对象右手镯名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetArmring_R(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.ArmRingr].Index);
            sMsg = CombineStr(sMsg, "<$ARMRING_R>", sText);
        }

        /// <summary>
        /// 取当前对象左手镯名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetArmring_L(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.ArmRingl].Index);
            sMsg = CombineStr(sMsg, "<$ARMRING_L>", sText);
        }

        /// <summary>
        /// 取当前对象护身符名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBujuk(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.Bujuk].Index);
            sMsg = CombineStr(sMsg, "<$BUJUK>", sText);
        }

        /// <summary>
        /// 取当前对象腰带名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBelt(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.Belt].Index);
            sMsg = CombineStr(sMsg, "<$BELT>", sText);
        }

        /// <summary>
        /// 取当前对象鞋子名称 
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBoots(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.Boots].Index);
            sMsg = CombineStr(sMsg, "<$BOOTS>", sText);
        }

        /// <summary>
        /// 取当前对象宝石名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetChrm(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.Charm].Index);
            sMsg = CombineStr(sMsg, "<$CHARM>", sText);
        }

        /// <summary>
        /// 取客服QQ
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetQQ(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$QQ>", M2Share.Config.sQQ);
        }

        /// <summary>
        /// 取客服手机号码
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetPhone(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$PHONE>", M2Share.Config.sPhone);
        }

        /// <summary>
        /// 取服务器IP
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetServerIp(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$SERVERIP>", M2Share.Config.ServerIPaddr);
        }

        /// <summary>
        /// 取论坛地址
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBbsWeiSite(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BBSSITE>", M2Share.Config.sBbsSite);
        }

        /// <summary>
        /// 取客户端下载地址
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetCilentDownLoad(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$CLIENTDOWNLOAD>", M2Share.Config.sClientDownload);
        }

        /// <summary>
        /// 取掉落物品名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetScatterItemName(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = playObject.ScatterItemName;
            sMsg = CombineStr(sMsg, "<$SCATTERITEMNAME>", sText);
        }

        /// <summary>
        /// 取物品掉落拥有者名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetScatterItemownerName(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = playObject.ScatterItemOwnerName;
            sMsg = CombineStr(sMsg, "<$SCATTERITEMOWNERNAME>", sText);
        }

        /// <summary>
        /// 取暴物品所在地图文件名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetScatterItemMapName(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = playObject.ScatterItemMapName;
            sMsg = CombineStr(sMsg, "<$SCATTERITEMMAPNAME>", sText);
        }

        /// <summary>
        /// 取物品掉落地图名称
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetScatterItemMapDesc(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = playObject.ScatterItemMapDesc;
            sMsg = CombineStr(sMsg, "<$SCATTERITEMMAPDESC>", sText);
        }

        /// <summary>
        /// 取物品掉落所在地图X坐标
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetScatterItemX(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (playObject.ScatterItemX).ToString();
            sMsg = CombineStr(sMsg, "<$SCATTERITEMX>", sText);
        }

        /// <summary>
        /// 取物品掉落所在地图Y坐标
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetScatterItemY(PlayObject playObject, string sVariable, ref string sMsg)
        {
            string sText = (playObject.ScatterItemY).ToString();
            sMsg = CombineStr(sMsg, "<$SCATTERITEMX>", sText);
        }

        /// <summary>
        /// 取银行帐号
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBankAccount0(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BANKACCOUNT0>", M2Share.Config.sBankAccount0);
        }

        /// <summary>
        /// 取银行帐号
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBankAccount1(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BANKACCOUNT0>", M2Share.Config.sBankAccount1);
        }

        /// <summary>
        /// 取银行帐号
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBankAccount2(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BANKACCOUNT0>", M2Share.Config.sBankAccount2);
        }

        /// <summary>
        /// 取银行帐号
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBankAccount3(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BANKACCOUNT0>", M2Share.Config.sBankAccount3);
        }

        /// <summary>
        /// 取银行帐号
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBankAccount4(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BANKACCOUNT0>", M2Share.Config.sBankAccount4);
        }

        /// <summary>
        /// 取银行帐号
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBankAccount5(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BANKACCOUNT0>", M2Share.Config.sBankAccount5);
        }

        /// <summary>
        /// 取银行帐号
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBankAccount6(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BANKACCOUNT0>", M2Share.Config.sBankAccount6);
        }

        /// <summary>
        /// 取银行帐号
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBankAccount7(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BANKACCOUNT0>", M2Share.Config.sBankAccount7);
        }

        /// <summary>
        /// 取银行帐号
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBankAccount8(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BANKACCOUNT0>", M2Share.Config.sBankAccount8);
        }

        /// <summary>
        /// 取银行帐号
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sVariable"></param>
        /// <param name="sMsg"></param>
        internal void GetBankAccount9(PlayObject playObject, string sVariable, ref string sMsg)
        {
            sMsg = CombineStr(sMsg, "<$BANKACCOUNT0>", M2Share.Config.sBankAccount9);
        }

        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <returns></returns>
        internal string CombineStr(string sMsg, string sStr, object sText)
        {
            string result;
            int n10 = sMsg.IndexOf(sStr);
            if (n10 > -1)
            {
                string s14 = sMsg[..n10];
                string s18 = sMsg[(sStr.Length + n10)..];
                result = string.Concat(s14, sText, s18);
            }
            else
            {
                result = sMsg;
            }
            return result;
        }
    }
}